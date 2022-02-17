namespace Membership.Core.Members.DataModels
{
    public class MemberRemoval
    {
        public MemberRemoval() { }
        public MemberRemoval(int reasonId, string description)
        {
            ReasonID = reasonId;
            Description = description;
        }

        public int ReasonID { get; set; }
        public string Description { get; set; }

    }
}
