using System;
using Dapper.Contrib.Extensions;

namespace Membership.Core.Members.DataModels
{
    [Table("MEMBER_List")]
    public class Member
    {
        public Member() { }

        private int RemovedStatus = 4;
        public Member(Guid memberId, int memberTypeId, bool isActive, string lastName, string firstName,
            string mi, string suffix, string comment, string emailAddress, 
            Guid sponsorID1, Guid sponsorID2, Guid sponsorID3,
            string address1, string address2, string city, string state, string zip, string phone, 
            DateTime? dateOfBirth, DateTime? dateObligated, DateTime? dateRemoved, int removalReasonId,
            DateTime? dateDeceased, Guid pageId, byte[] pageThumb)
        {
            MemberId = memberId;
            MemberTypeId = memberTypeId;
            IsActive = isActive;
            LastName = lastName;
            FirstName = firstName;
            MI = mi;
            Suffix = suffix;
            Comment = comment;
            EmailAddress = emailAddress;
            SponsorID1 = sponsorID1;
            SponsorID2 = sponsorID2;
            SponsorID3 = sponsorID3;
            Address1 = address1;
            Address2 = address2;
            City = city;
            State = state;
            ZIP = zip;
            Phone = phone;
            DateOfBirth = dateOfBirth;
            DateObligated = dateObligated;
            DateRemoved = dateRemoved;
            RemovalReasonId = removalReasonId;
            DateDeceased = dateDeceased;
            PageId = pageId;
            PageThumb = pageThumb;
        }

        [Computed]
        public string Status
        {
            get
            {
                switch (MemberTypeId)
                {
                    case 1:
                        return "Active";
                    case 2:
                        return "Life";
                    case 3:
                        return "Social";
                    default:
                        return "";
                }
            }
         }
        [Computed] public float DuesAmount { get; set; }


        [Computed] public bool WasRemoved => MemberTypeId == RemovedStatus;
        [Computed] public bool IsCurrent
        {
            get { return (MemberTypeId >= 1 && MemberTypeId <= 3) && DateDeceased is null; }
            set { }
        }

        [Computed] public bool IsDeceased => DateDeceased > DateObligated;
        [Computed] public string FullName => $"{FirstName} {LastName}";
        [Computed] public string LFName => string.IsNullOrEmpty(Suffix) 
            ? $"{LastName}, {FirstName}" 
            : $"{LastName} {Suffix}, {FirstName}";
        [Computed] public string CityStateZip => $"{City}, {State}  {ZIP}";


        [ExplicitKey] public Guid MemberId { get; set; }
        public int MemberTypeId { get; set; }
        public bool IsActive { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MI { get; set; }
        public string Suffix { get; set; }
        public string Comment { get; set; }
        public string EmailAddress { get; set; }
        public Guid SponsorID1 { get; set; }
        public Guid SponsorID2 { get; set; }
        public Guid SponsorID3 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIP { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateObligated { get; set; }
        public DateTime? DateRemoved { get; set; }
        public int RemovalReasonId { get; set; }
        public DateTime? DateDeceased { get; set; }
        public Guid PageId { get; set; }
        public byte[] PageThumb { get; set; }
    }
}
