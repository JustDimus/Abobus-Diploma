using AbobusMobile.DAL.Services.Abstractions.Authorization;
using AbobusMobile.DAL.Services.Abstractions.Configurations;
using AbobusMobile.Database.Models;
using AbobusMobile.Database.Services.Abstractions;
using AbobusMobile.Utilities.Exceptions;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Authorization
{
    public class AuthorizationDataManager : IAuthorizationDataManager
    {
        private readonly IConfigurationsDataManager _configurationManager;

        public AuthorizationDataManager(
            IConfigurationsDataManager configurationManager)
        {
            _configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
        }

        public async Task<bool> CheckAuthorizationDataAvailabilityAsync()
        {
            var configurations = await SelectAuthorizationConfigurations();

            return configurations.Count == AuthorizationDataConstants.AUTHORIZATION_CONFIG_COUNT;
        }

        public async Task<AuthorizationDataModel> GetAuthorizationDataAsync()
        {
            var configurations = await SelectAuthorizationConfigurations();

            AuthorizationDataModel result = null;

            if (configurations.Count == AuthorizationDataConstants.AUTHORIZATION_CONFIG_COUNT)
            {
                result = new AuthorizationDataModel()
                {
                    AuthorizationToken = configurations.GetString(AuthorizationDataConstants.AUTHORIZATION_TOKEN),
                    RefreshToken = configurations.GetString(AuthorizationDataConstants.REFRESH_TOKEN),
                    ExpirationTime = configurations.GetDateTime(AuthorizationDataConstants.AUTHORIZATION_EXPIRATION),
                };
            }

            return result;
        }

        public async Task SetAuthorizationData(AuthorizationDataModel authorizationData)
        {
            ValidateAuthorizationData(authorizationData);

            await UpdateConfiguration(
                AuthorizationDataConstants.AUTHORIZATION_TOKEN, authorizationData.AuthorizationToken);

            await UpdateConfiguration(
                AuthorizationDataConstants.AUTHORIZATION_EXPIRATION, authorizationData.ExpirationTime.ToString());

            await UpdateConfiguration(
                AuthorizationDataConstants.REFRESH_TOKEN, authorizationData.RefreshToken);
        }

        public async Task ClearAuthorizationDataAsync()
        {
            var configurations = await SelectAuthorizationConfigurations();

            foreach (var configuration in configurations)
            {
                await _configurationManager.DeleteAsync(configuration.Name);
            }
        }

        private async Task<List<ConfigurationModel>> SelectAuthorizationConfigurations()
        {
            return await _configurationManager.SelectAsync(configuration
                => configuration.Name == AuthorizationDataConstants.AUTHORIZATION_EXPIRATION
                || configuration.Name == AuthorizationDataConstants.AUTHORIZATION_TOKEN
                || configuration.Name == AuthorizationDataConstants.REFRESH_TOKEN);
        }

        private void ValidateAuthorizationData(AuthorizationDataModel authorizationData)
        {
            if (authorizationData == null
                || !authorizationData.AuthorizationToken.IsNotNullOrWhiteSpace()
                || !authorizationData.RefreshToken.IsNotNullOrWhiteSpace())
            {
                throw new ValidationException($"Model {nameof(authorizationData)} is not valid");
            }
        }

        private async Task UpdateConfiguration(string name, string value)
        {
            await _configurationManager.UpdateAsync(name, value);
        }
    }
}
