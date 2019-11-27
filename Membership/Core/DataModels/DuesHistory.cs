using System;
using Dapper.Contrib.Extensions;

namespace Membership.Core.DataModels
{
    [Table("DUES_History")]
    public class DuesHistory
    {

        [ExplicitKey]
        public Guid MemberId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public float Amount { get; set; }
    }
}

