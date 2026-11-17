
namespace Membership.Core.Officers.DataModels;

public class Office
{
    public Office() { }
    public Office(int officeId, string title, int groupId, 
        int groupOrder, bool currentlyUsed)
    {
        OfficeId = officeId;
        Title = title;
        GroupId = groupId;
        GroupOrder = groupOrder;
        CurrentlyUsed = CurrentlyUsed;
    }

    public int OfficeId { get; set; }
    public string Title { get; set; }
    public int GroupId { get; set; }
    public int GroupOrder { get; set; }
    public bool CurrentlyUsed { get; set; }
}