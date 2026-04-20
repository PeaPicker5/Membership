using System;
using Dapper.Contrib.Extensions;

namespace Membership.Core.Members.DataModels;

[Table("MEMBER_List")]
public class Member
{
    public Member() { }

    private int RemovedStatus = 4;
    public Member(Guid memberId, int memberTypeId, bool isActive, int fireFighterNum, string lastName, string firstName,
        string mi, string suffix, string comment, string emailAddress, 
        Guid sponsorId1, Guid sponsorId2, Guid sponsorId3,
        string address1, string address2, string city, string state, string zip, string phone, 
        DateTime? dateOfBirth, DateTime? dateObligated, DateTime? dateRemoved, int removalReasonId,
        DateTime? dateDeceased, Guid pageId, byte[] pageThumb, float legacyLosapYears)
    {
        MemberId = memberId;
        MemberTypeId = memberTypeId;
        IsActive = isActive;
        FireFighterNum = fireFighterNum;
        LastName = lastName;
        FirstName = firstName;
        MI = mi;
        Suffix = suffix;
        Comment = comment;
        EmailAddress = emailAddress;
        SponsorId1 = sponsorId1;
        SponsorId2 = sponsorId2;
        SponsorId3 = sponsorId3;
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
        LegacyLosapYears = legacyLosapYears;
    }

    [Computed]
    public string Status
    {
        get
        {
            switch (MemberTypeId)
            {
                case 1: return "Active";
                case 2: return "Life";
                case 3: return "Social";
                case 4: return "Removed";
                case 5: return "Social";
                case 6: return "Honorary";
                case 7: return "Hon. Active";
                case 8: return "Hon. Social";
                default:
                    return "Unknown";
            }
        }
    }
    [Computed] public float DuesAmount { get; set; }


    [Computed] public bool WasRemoved => MemberTypeId == RemovedStatus;

    [Computed]
    public bool IsCurrent
    {
        get => (MemberTypeId >= 1 && MemberTypeId <= 3) && DateDeceased is null;
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
    public int FireFighterNum { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MI { get; set; }
    public string Suffix { get; set; }
    public string Comment { get; set; }
    public string EmailAddress { get; set; }
    public Guid SponsorId1 { get; set; }
    public Guid SponsorId2 { get; set; }
    public Guid SponsorId3 { get; set; }
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
    public float LegacyLosapYears { get; set; }

    [Computed] public int LosapYears { get; set; }
}