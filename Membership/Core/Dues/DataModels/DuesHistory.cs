using System;

namespace Membership.Core.Dues.DataModels;

public class DuesHistory
{

    public Guid MemberId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public float Amount { get; set; }
}