using System;
using Dapper.Contrib.Extensions;
using Membership.Core.Members.DataModels;

namespace Membership.Core.Officers.DataModels
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
        [ExplicitKey] public int OfficeId => OfficeRec.OfficeId;



        [Computed] public int FromMonth => FromDate.Month;
        [Computed] public int ToMonth => ToDate.Month;

        [Computed] public string PartialYear
        {
            get
            {
                if (ToDate == DateTime.MinValue) return "";
                if (FromMonth <= 1 && (ToMonth <= 0 || ToMonth >= 12)) return "";
                var fMonth = new DateTime(2020, FromMonth, 1).ToString("MMM");
                var tMonth = new DateTime(2020, ToMonth, 1).ToString("MMM");
                return $"({fMonth}-{tMonth})";
            }
        }
    }
}
