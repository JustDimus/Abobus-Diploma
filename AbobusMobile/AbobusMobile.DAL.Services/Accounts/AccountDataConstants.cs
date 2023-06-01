using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.DAL.Services.Accounts
{
    public static class AccountDataConstants
    {
        public const string DETAILS_ID = "CurrentAccountId";
        public const string DETAILS_USERNAME = "CurrentAccountUsername";
        public const string DETAILS_EMAIL = "CurrentAccountEmail";
        public const string DETAILS_PROFILE_PHOTO_NAME = "CurrentAccountPhoto";

        public const int DETAILS_CONFIG_COUNT = 4;

        public const string STATISTICS_ROUTES = "CurrentAccountRoutesCount";
        public const string STATISTICS_CITIES = "CurrentAccountVisitedCitiesCount";
        public const string STATISTICS_FRIENDS = "CurrentAccountFriendsCount";
        public const string STATISTICS_DISTANCE = "CurrentAccountPassedDistance";
        public const string STATISTICS_DISTANCE_UNIT = "DistanceUnit";

        public const int STATISTICS_CONFIG_COUNT = 5;
    }
}
