namespace AbobusMobile.BLL.Services.Abstractions.Accounts
{
    public class AccountStatisticsServiceModel
    {
        public int RoutesCount { get; set; }
        public int VisitedCitiesCount { get; set; }
        public int FriendsCount { get; set; }
        public int PassedDistance { get; set; }
        public string DistanceUnit { get; set; }
    }
}