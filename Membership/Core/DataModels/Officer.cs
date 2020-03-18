using System;
using System.Globalization;
using Dapper.Contrib.Extensions;

namespace Membership.Core.DataModels
{
    [Table("OFFICE_Assignments")]
    public class Officer
    {
        public Officer() {}
        public Officer(Member memberRec, Office officeRec, int year, 
            DateTime fromDate, DateTime toDate)
        {
            MemberRec = memberRec;
            OfficeRec = officeRec;
            Year = year;
            FromDate = fromDate;
            ToDate = toDate;
        }

        [Computed] public Member MemberRec { get; set; }
        [Computed] public Office OfficeRec { get; set; }
        public int Year { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        [ExplicitKey] public Guid MemberId => MemberRec.MemberId;
        public int OfficeId => OfficeRec.OfficeId;



        [Computed] public int FromMonth => FromDate.Month;
        [Computed] public int ToMonth => ToDate.Month;


        [Computed] public bool PartialYear => FromMonth > 1 || (ToMonth > 0 && ToMonth < 12);
    }
}
