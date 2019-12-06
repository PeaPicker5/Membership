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
    }
}
