using System;
using Membership.Core.Members.DataModels;

namespace Membership.Core.Officers.DataModels;

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

    public Member MemberRec { get; set; }
    public Office OfficeRec { get; set; }
    public int Year { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    public Guid MemberId => MemberRec.MemberId;
    public int OfficeId => OfficeRec.OfficeId;



    public int FromMonth => FromDate.Month;
    public int ToMonth => ToDate.Month;

    public string PartialYear
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