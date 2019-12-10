using System;
using ControlzEx.Standard;

namespace Membership.Core.DataModels
{
    public class Member
    {
        public Member() { }

        public Member(Guid memberId, int memberTypeId, string lastName, string firstName,
            string mi, string comment, string emailAddress, Guid sponsorID1, Guid sponsorID2, Guid sponsorID3,
            string address1, string address2, string city, string state, string zip, string phone, 
            DateTime dateOfBirth, DateTime dateObligated, DateTime dateActive, DateTime dateOffActive,
            DateTime dateRemoved, int removalReasonCode, DateTime dateDeceased)
        {
            MemberId = memberId;
            MemberTypeId = memberTypeId;
            LastName = lastName;
            FirstName = firstName;
            MI = mi;
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
            DateActive = dateActive;
            DateOffActive = dateOffActive;
            DateRemoved = dateRemoved;
            RemovalReasonCode = removalReasonCode;
            DateDeceased = dateDeceased;

        }

        public string Status { get; set; }
        public float DuesAmount { get; set; }
    
        public bool IsCurrent
        {
            get { return (MemberTypeId == 1 || MemberTypeId == 2) && !IsDeceased; }
            set { }
        }

        public bool IsDeceased => DateDeceased > DateObligated;
        public string FullName => $"{FirstName} {LastName}";
        public string CityStateZip => $"{City}, {State}  {ZIP}";




        public Guid MemberId { get; set; }
        public int MemberTypeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MI { get; set; }
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
        public DateTime DateOfBirth { get; set; }
        public DateTime DateObligated { get; set; }
        public DateTime DateActive { get; set; }
        public DateTime DateOffActive { get; set; }
        public DateTime DateRemoved { get; set; }
        public int RemovalReasonCode { get; set; }
        public DateTime DateDeceased { get; set; }


    }
}
