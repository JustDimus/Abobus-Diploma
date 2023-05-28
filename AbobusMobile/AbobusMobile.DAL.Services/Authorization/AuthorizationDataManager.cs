﻿using AbobusMobile.DAL.Services.Abstractions.Authorization;
using AbobusMobile.Database.Models;
using AbobusMobile.Database.Services.Abstractions;
using AbobusMobile.Utilities.Exceptions;
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
        private readonly IRepository<ConfigurationModel> _configurations;

        public AuthorizationDataManager(IRepository<ConfigurationModel> configurationsRepository)
        {
            _configurations = configurationsRepository ?? throw new ArgumentNullException(nameof(configurationsRepository));
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
                    AuthorizationToken = configurations.First(config => config.Name == AuthorizationDataConstants.AUTHORIZATION_TOKEN).Value,
                    RefreshToken = configurations.First(config => config.Name == AuthorizationDataConstants.REFRESH_TOKEN).Value,
                    ExpirationTime = DateTime.Parse(
                        configurations.First(config => config.Name == AuthorizationDataConstants.AUTHORIZATION_EXPIRATION).Value),
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
                await _configurations.DeleteAsync(configuration);
            }
        }

        private async Task<List<ConfigurationModel>> SelectAuthorizationConfigurations()
        {
            return await _configurations.Select(configuration
                => configuration.Name == AuthorizationDataConstants.AUTHORIZATION_EXPIRATION
                || configuration.Name == AuthorizationDataConstants.AUTHORIZATION_TOKEN
                || configuration.Name == AuthorizationDataConstants.REFRESH_TOKEN);
        }

        private void ValidateAuthorizationData(AuthorizationDataModel authorizationData)
        {
            if (authorizationData == null
                || authorizationData.AuthorizationToken == null
                || authorizationData.RefreshToken == null)
            {
                throw new ValidationException($"Model {nameof(authorizationData)} is not valid");
            }
        }

        private async Task UpdateConfiguration(string name, string value)
        {
            var configuration = new ConfigurationModel()
            {
                Id = 0,
                Name = name,
                Value = value
            };

            await _configurations.UpdateAsync(configuration);
        }
    }
}
