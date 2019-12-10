namespace Membership.Core.DataModels
{
    public class MemberRemoval
    {
        public MemberRemoval() { }
        public MemberRemoval(int removalId, string description)
        {
            RemovalId = removalId;
            Description = description;
        }

        public int RemovalId { get; set; }
        public string Description { get; set; }

    }
}
