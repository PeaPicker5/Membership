using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Membership.Core.Meetings.DataModels
{
    [Table("MEETING_List")]
    public class Meeting
    {
        public Meeting() { }
        public Meeting(Guid meetingId, DateTime meetingDate, string description, string comments, Guid inCharge)
        {
            MeetingId = meetingId;
            MeetingDate = meetingDate;
            Description = description;
            Comments = comments;
            InCharge = inCharge;
        }

        [ExplicitKey] public Guid MeetingId  { get; set; }
        public DateTime MeetingDate { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public Guid InCharge { get; set; }


        public IEnumerable<Guid> Attendees { get; set; }



    }
}
