using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
