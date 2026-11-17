using System;

namespace Membership.Core.Meetings.DataModels;

public class MeetingMember
{
    public MeetingMember() { }
    public MeetingMember(Guid meetingId, Guid memberId)
    {
        MeetingId = meetingId;
        MemberId = memberId;
    }

    public Guid MeetingId { get; set; }
    public Guid MemberId { get; set; }

}