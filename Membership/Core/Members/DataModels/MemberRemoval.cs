namespace Membership.Core.Members.DataModels;

public class MemberRemoval
{
    public MemberRemoval() { }
    public MemberRemoval(int reasonId, string description)
    {
        ReasonId = reasonId;
        Description = description;
    }

    public int ReasonId { get; set; }
    public string Description { get; set; }

}