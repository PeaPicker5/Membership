using Dapper.Contrib.Extensions;

namespace Membership.Core.Officers.DataModels
{
    [Table("OFFICE_List")]
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

        [ExplicitKey] public int OfficeId { get; set; }
        public string Title { get; set; }
        public int GroupId { get; set; }
        public int GroupOrder { get; set; }
        public bool CurrentlyUsed { get; set; }
    }
}
