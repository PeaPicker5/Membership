using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Membership.Core.Meetings.DataModels
{
    [Table("MEETING_List")]
    public class Meeting
    {
        public Meeting() { }
        public Meeting(Guid meetingId, DateTime meetingDate, string description, string comments, Guid inCharge, bool isRegularScheduled)
        {
            MeetingId = meetingId;
            MeetingDate = meetingDate;
            Description = description;
            Comments = comments;
            InCharge = inCharge;
            IsRegularScheduled = isRegularScheduled;
        }

        [ExplicitKey] public Guid MeetingId  { get; set; }
        public DateTime MeetingDate { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public Guid InCharge { get; set; }
        public bool IsRegularScheduled { get; set; }


        public ICollection<Tuple<Guid, string>> Attendees { get; set; }

        public int MemberCount => Attendees?.Count ?? 0;

    }
}
