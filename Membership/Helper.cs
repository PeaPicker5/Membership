using System.Configuration;

namespace Membership
{
    public static class Helper
    {
        public static string ConnVal(string name)
        {
            var retValue = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            return retValue;
        }

        /// <summary>
        /// Meetings a member must attend to vote. Set "MeetingsNeededToVote" in App.config; defaults to 5.
        /// </summary>
        public static int MeetingsNeededToVote =>
            int.TryParse(ConfigurationManager.AppSettings["MeetingsNeededToVote"], out var count) && count > 0
                ? count
                : 5;
    }
}
