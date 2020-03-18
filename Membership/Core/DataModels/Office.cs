using Dapper.Contrib.Extensions;

namespace Membership.Core.DataModels
{
    [Table("OFFICE_List")]
    public class Office
    {
        public Office() { }
        public Office(int officeId, string title, int groupId, 
                      int groupOrder, int currentlyUsed)
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
        public int CurrentlyUsed { get; set; }
    }
}
