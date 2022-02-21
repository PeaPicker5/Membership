using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.Meetings.DataModels;
using Membership.Core.Meetings.Repositories;
using Membership.Core.Members.DataModels;
using Membership.Core.Members.Repositories;

namespace Membership.Core.Meetings.Presenters
{
    public class MeetingAttendancePresenter :IMeetingAttendanceView
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IMeetingAttendanceView _view;
        public Meeting CurrentMeeting { get; set; }
        public ICollection<Member> MemberList { get; set; }
        public ICollection<Member> AdditionalMembers { get; set; }

        public MeetingAttendancePresenter(IMeetingAttendanceView view)
        {
            _meetingRepository = new MeetingRepository();
            _memberRepository = new MemberRepository();
            _view = view;
        }

        public void LoadMeeting(Guid meetingId)
        {
            _view.CurrentMeeting = _meetingRepository.Get(meetingId);
        }
        public void LoadAttendanceList()
        {
            var currentMembers = _memberRepository.GetCurrentMembers().ToList();

            _view.MemberList = currentMembers.Where(x => x.ZIP.StartsWith("14")).ToList(); // local members
            _view.AdditionalMembers = currentMembers.Except(_view.MemberList).ToList();
        }

    }
}
