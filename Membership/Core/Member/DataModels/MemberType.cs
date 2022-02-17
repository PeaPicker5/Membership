namespace Membership.Core.Member.DataModels
{
    public class MemberType
    {
        public MemberType() { }
        public MemberType(int typeId, string description, float duesAmount)
        {
            TypeId = typeId;
            Description = description;
            DuesAmount = duesAmount;
        }

        public int TypeId { get; set; }
        public string Description { get; set; }
        public float DuesAmount { get; set; }

    }
}
