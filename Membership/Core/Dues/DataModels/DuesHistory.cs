using System;
using Dapper.Contrib.Extensions;

namespace Membership.Core.Dues.DataModels
{
    [Table("DUES_History")]
    public class DuesHistory
    {

        [ExplicitKey] public Guid MemberId { get; set; }
        [ExplicitKey] public int Year { get; set; }
        public int Month { get; set; }
        public float Amount { get; set; }
    }
}

