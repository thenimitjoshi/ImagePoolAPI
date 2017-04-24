using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImagePool.Models
{    
    public class GameSchedule
    {
        public string GameKey { get; set; }
        public string SeasonType { get; set; }
        public string Season { get; set; }
        public int Week { get; set; }
        public string Date { get; set; }
        public string AwayTeam { get; set; }
        public string HomeTeam { get; set; }
        public string Channel { get; set; }
        public string PointSpread { get; set; }
        public string OverUnder { get; set; }
        public string StadiumID { get; set; }
        public string Canceled { get; set; }
        public string GeoLat { get; set; }
        public string GeoLong { get; set; }
        public string ForecastTempLow { get; set; }
        public string ForecastTempHigh { get; set; }
        public string ForecastDescription { get; set; }
        public string ForecastWindChill { get; set; }
        public string ForecastWindSpeed { get; set; }
        public string AwayTeamMoneyLine { get; set; }
        public string HomeTeamMoneyLine { get; set; }
        public string Day { get; set; }
        public string DateTime { get; set; }
        public string GlobalGameID { get; set; }
        public string GlobalAwayTeamID { get; set; }
        public string GlobalHomeTeamID { get; set; }
        public StadiumInfo StadiumDetails { get; set; }
    }

    public class StadiumInfo
    {
        public string StadiumID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Capacity { get; set; }
        public string PlayingSurface { get; set; }
        public string GeoLat { get; set; }
        public string GeoLong { get; set; }
    }

    public class Team
    {
        public string Key { get; set; }
        public string TeamID { get; set; }
        public string PlayerID { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
        public string FullName { get; set; }
        public string StadiumID { get; set; }
        public string ByeWeek { get; set; }
        public string AverageDraftPosition { get; set; }
        public string AverageDraftPositionPPR { get; set; }
        public string HeadCoach { get; set; }
        public string OffensiveCoordinator { get; set; }
        public string DefensiveCoordinator { get; set; }
        public string SpecialTeamsCoach { get; set; }
        public string OffensiveScheme { get; set; }
        public string DefensiveScheme { get; set; }
        public string UpcomingSalary { get; set; }
        public string UpcomingOpponent { get; set; }
        public string UpcomingOpponentRank { get; set; }
        public string UpcomingOpponentPositionRank { get; set; }
        public string UpcomingFanDuelSalary { get; set; }
        public string UpcomingDraftKingsSalary { get; set; }
        public string UpcomingYahooSalary { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public string TertiaryColor { get; set; }
        public string QuaternaryColor { get; set; }
        public string WikipediaLogoUrl { get; set; }
        public string WikipediaWordMarkUrl { get; set; }
        public string GlobalTeamID { get; set; }
        public StadiumInfo StadiumDetails { get; set; }
    }

    /// <summary>
    /// This class is created for Team
    /// </summary>
    public class TeamInfo
    {
        #region Properties
        /// <summary>
        /// get and set the GameScheduleId
        /// </summary>
        public long GameScheduleID { get; set; }
        /// <summary>
        /// get and set the HomeTeamID
        /// </summary>
        public long HomeTeamID { get; set; }
        /// <summary>
        /// get and set the AwayTeamId
        /// </summary>
        public long AwayTeamId { get; set; }
        // <summary>
        /// get and set the HomeTeamName
        /// </summary>
        public string HomeTeamName { get; set; }
        /// <summary>
        /// get and set the HomeTeamUrl
        /// </summary>
        public string HomeTeamUrl { get; set; }
        /// <summary>
        /// get and set the HomeTeamLogoUrl
        /// </summary>
        public string HomeTeamLogoUrl { get; set; }
        /// <summary>
        /// get and set the AwayTeamName
        /// </summary>
        public string AwayTeamName { get; set; }
        /// <summary>
        /// get and set the AwayTeamUrl
        /// </summary>
        public string AwayTeamUrl { get; set; }
        /// <summary>
        /// get and set the AwayTeamLogoUrl
        /// </summary>
        public string AwayTeamLogoUrl { get; set; }
        /// <summary>
        /// get and set the StartTime
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// get and set the EndTime
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// get and set the Network
        /// </summary>
        public string Network { get; set; }
        /// <summary>
        /// get and set the TotalCastersCount
        /// </summary>
        public long TotalCastersCount { get; set; }
        /// <summary>
        /// get and set the CasterList
        /// </summary>
        public List<GameCasters> CasterList { get; set; }
        #endregion
    }
    /// <summary>
    /// This class is created for League Info
    /// </summary>
    public class LeagueInfo
    {
        #region Properties
        /// <summary>
        /// get and set the League
        /// </summary>
        public League League { get; set; }
        /// <summary>
        /// get and set the Teams
        /// </summary>
        public List<TeamInfo> Teams { get; set; }
        #endregion
    }
    /// <summary>
    /// This class is created to get all game public game casters.
    /// </summary>
    public class GameCasters
    {
        #region Properties
        /// <summary>
        /// get and set the UserProfileId
        /// </summary>
        public long UserProfileId { get; set; }
        /// <summary>
        /// get and set the UserName
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// get and set the ProfilePhotoURL
        /// </summary>
        public string ProfilePhotoURL { get; set; }
        /// <summary>
        /// get and set the TotalListernersCount
        /// </summary>
        public long TotalListenersCount { get; set; }
        /// <summary>
        /// get and set the IsFavourite
        /// </summary>
        public bool IsFavourite { get; set; }
        /// <summary>
        /// get and set the MatchInformation
        /// </summary>
        public string MatchInformation { get; set; }
        #endregion
    }
    /// <summary>
    /// Class for Today's LineUp
    /// </summary>
    public class TodaysLineUp
    {
        #region Properties
        /// <summary>
        /// get and set the HomeTeamID
        /// </summary>
        public long HomeTeamID { get; set; }
        /// <summary>
        /// get and set the AwayTeamId
        /// </summary>
        public long AwayTeamId { get; set; }
        /// <summary>
        /// get and set the GameScheduleId
        /// </summary>
        public long GameScheduleID { get; set; }
        /// <summary>
        /// get and set the GameTypeName
        /// </summary>
        public string GameTypeName { get; set; }
        /// <summary>
        /// get and set the GameTypeURL
        /// </summary>
        public string GameTypeURL { get; set; }
        /// <summary>
        /// get and set the HomeTeamName
        /// </summary>
        public string HomeTeamName { get; set; }
        /// <summary>
        /// get and set the HomeTeamUrl
        /// </summary>
        public string HomeTeamUrl { get; set; }
        /// <summary>
        /// get and set the HomeTeamLogoUrl
        /// </summary>
        public string HomeTeamLogoUrl { get; set; }
        /// <summary>
        /// get and set the AwayTeamName
        /// </summary>
        public string AwayTeamName { get; set; }
        /// <summary>
        /// get and set the AwayTeamUrl
        /// </summary>
        public string AwayTeamUrl { get; set; }
        /// <summary>
        /// get and set the AwayTeamLogoUrl
        /// </summary>
        public string AwayTeamLogoUrl { get; set; }
        /// <summary>
        /// get and set the StartTime
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// get and set the EndTime
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// get and set the Network
        /// </summary>
        public string Network { get; set; }
        /// <summary>
        /// get and se the LeagueTotalListenersCount
        /// </summary>
        public long LeagueTotalListenersCount { get; set; }
        /// <summary>
        /// get and set the LeagueTotalCastersCount
        /// </summary>
        public long LeagueTotalCastersCount { get; set; }
        #endregion
    }
    /// <summary>
    /// This class is created for League
    /// </summary>
    public class League
    {
        #region Properties
        /// <summary>
        /// get and set the LeagueName
        /// </summary>
        public string LeagueName { get; set; }
        /// <summary>
        /// get and set the LeagueUrl
        /// </summary>
        public string LeagueURL { get; set; }
        /// <summary>
        /// get and set the LeagueTotalListenersCount
        /// </summary>
        public long LeagueTotalListenersCount { get; set; }
        #endregion
    }
}