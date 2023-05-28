using AbobusCore.Models.Session;
using AbobusMobile.BLL.Services.Abstractions.Authorization;
using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.Communication.Services.Abstractions.Handlers;
using AbobusMobile.Communication.Services.Abstractions.Models;
using AbobusMobile.Communication.Services.Abstractions.Models.Requests;
using AbobusMobile.DAL.Services.Abstractions.Authorization;
using AbobusMobile.Utilities.Exceptions;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizationDataManager _authorizationManager;
        private readonly IRequestFactory _requestFactory;
        private readonly IAuthorizationHandler _authorizationHandler;

        private Subject<bool> _authorizationNeededSubject = new Subject<bool>();

        private LoginRequest loginRequest;
        private LogoutRequest logoutRequest;
        private RefreshRequest refreshRequest;

        public AuthorizationService(
            IAuthorizationDataManager authorizationManager,
            IRequestFactory requestFactory,
            IAuthorizationHandler authorizationHandler)
        {
            _authorizationManager = authorizationManager ?? throw new ArgumentNullException(nameof(authorizationManager));
            _requestFactory = requestFactory ?? throw new ArgumentNullException(nameof(requestFactory));
            _authorizationHandler = authorizationHandler ?? throw new ArgumentNullException(nameof(authorizationHandler));

            _authorizationHandler.OnAuthorizationFailed += OnAuthorizationFailed;
        }

        private LoginRequest LoginRequest => loginRequest ?? (loginRequest = _requestFactory.CreateRequest<LoginRequest>());
        private LogoutRequest LogoutRequest => logoutRequest ?? (logoutRequest = _requestFactory.CreateRequest<LogoutRequest>());
        private RefreshRequest RefreshRequest => refreshRequest ?? (refreshRequest = _requestFactory.CreateRequest<RefreshRequest>());

        public IObservable<bool> AuthorizationNeededObservable => _authorizationNeededSubject.AsObservable();

        public async Task<AuthorizationStatus> CheckAuthorizationStatusAsync()
        {
            var result = AuthorizationStatus.Unknown;

            var authorizationDataAvailable = await _authorizationManager.CheckAuthorizationDataAvailabilityAsync();

            if (authorizationDataAvailable)
            {
                var authorizationData = await _authorizationManager.GetAuthorizationDataAsync();

                if (authorizationData.ExpirationTime.AddMinutes(-1) <= DateTime.UtcNow)
                {
                    result = AuthorizationStatus.AuthorizationTokenExpired;
                }
                else
                {
                    result = AuthorizationStatus.Authorized;
                }
            }
            else
            {
                result = AuthorizationStatus.Unauthorized;
            }

            return result;
        }

        public async Task<AuthorizationStatus> LoginAsync(LoginAuthorizationModel loginData)
        {
            ValidateLoginData(loginData);

            LoginRequest.Initialize(loginData.Email, loginData.Password);

            var result = await LoginRequest.SendRequestAsync();

            if (result.Succeeded)
            {
                var sessionModel = result.As<SessionResultModel>();

                await UpdateAuthorizationData(sessionModel);

                return AuthorizationStatus.Authorized;
            }

            return AuthorizationStatus.OperationFailed;
        }

        public async Task<AuthorizationStatus> LogoutAsync()
        {
            var result = await LogoutRequest.SendRequestAsync();

            if (result.Succeeded)
            {
                await _authorizationManager.ClearAuthorizationDataAsync();

                return AuthorizationStatus.Unauthorized;
            }

            return AuthorizationStatus.OperationFailed;
        }

        private async Task OnAuthorizationFailed(BaseRequest failedRequest)
        {
            var authorizationDataAvailable = await _authorizationManager.CheckAuthorizationDataAvailabilityAsync();

            if (authorizationDataAvailable)
            {
                var authorizationData = await _authorizationManager.GetAuthorizationDataAsync();

                RefreshRequest.Initialize(authorizationData.RefreshToken);

                var result = await RefreshRequest.SendRequestAsync();
                
                if (result.Succeeded)
                {
                    var sessionModel = result.As<SessionResultModel>();

                    await UpdateAuthorizationData(sessionModel);

                    await failedRequest.SendRequestAsync();

                    return;
                }
            }

            _authorizationNeededSubject.OnNext(false);
        }

        private async Task UpdateAuthorizationData(SessionResultModel sessionModel)
        {
            sessionModel.ValidateIsNotNull();

            await _authorizationManager.SetAuthorizationData(new AuthorizationDataModel()
            {
                AuthorizationToken = sessionModel.AuthorizationToken,
                RefreshToken = sessionModel.RefreshToken,
                ExpirationTime = sessionModel.ExpirationTime,
            });
        }

        private void ValidateLoginData(LoginAuthorizationModel loginData)
        {
            if (loginData == null
                || string.IsNullOrWhiteSpace(loginData.Email)
                || string.IsNullOrWhiteSpace(loginData.Password))
            {
                throw new ValidationException($"Model {nameof(loginData)} is not valid");
            }
        }
    }
}
