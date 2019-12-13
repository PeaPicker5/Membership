using System;
using System.Globalization;
using Dapper.Contrib.Extensions;

namespace Membership.Core.DataModels
{
    [Table("OFFICE_Assignments")]
    public class Officer
    {
        public Officer() {}
        public Officer(Member memberRec, Office officeRec, int year, int fromMonth, int toMonth)
        {
            MemberRec = memberRec;
            OfficeRec = officeRec;
            Year = year;
            FromMonth = fromMonth;
            ToMonth = toMonth;
        }

        [ExplicitKey] public Member MemberRec { get; set; }
        [Computed] public Office OfficeRec { get; set; }
        public int Year { get; set; }
        public int FromMonth { get; set; }
        public int ToMonth { get; set; }

        public int OfficeId => OfficeRec.OfficeId;
        [Computed] public string FromDate => FromMonth > 0
            ? DateTime.ParseExact($"{FromMonth:D2}/{Year:D4}", "MM'/'yyyy", CultureInfo.InvariantCulture).Date.ToString("MM/yyyy")
            : DateTime.ParseExact($"01/{Year}", "M'/'yyyy", CultureInfo.InvariantCulture).Date.ToString("MM/yyyy");


        [Computed] public string ToDate => ToMonth > 0
            ? DateTime.ParseExact($"{ToMonth:D2}/{Year:D4}", "MM'/'yyyy", CultureInfo.InvariantCulture).Date.ToString("MM/yyyy")
            : DateTime.ParseExact($"12/{Year}", "MM'/'yyyy", CultureInfo.InvariantCulture).Date.ToString("MM/yyyy");

        [Computed] public bool PartialYear => FromMonth > 1 || (ToMonth > 0 && ToMonth < 12);
    }
}
