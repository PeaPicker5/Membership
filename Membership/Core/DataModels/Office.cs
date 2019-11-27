namespace Membership.Core.DataModels
{
    public class Office
    {
        public Office() { }
        public Office(int officeId, string title, int groupId, int order)
        {
            OfficeId = officeId;
            Title = title;
            GroupId = groupId;
            Order = order;
        }

        public int OfficeId { get; set; }
        public string Title { get; set; }
        public int GroupId { get; set; }
        public int Order { get; set; }

    }
}
