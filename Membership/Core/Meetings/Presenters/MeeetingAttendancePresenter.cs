using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.DataModels;
using Membership.Core.Meetings.DataModels;
using Membership.Core.Meetings.Repositories;
using Membership.Core.Members.Repositories;
using Membership.Core.Officers.DataModels;
using Membership.Core.Officers.Repositories;

namespace Membership.Core.Meetings.Presenters
{
    public class MeetingAttendancePresenter :IMeetingAttendanceView
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IOfficerRepository _officerRepository;
        private readonly IMeetingAttendanceView _view;
        private const bool notSelected = false;

        public Meeting CurrentMeeting { get; set; }
        public ICollection<SelectableItem> MemberCheckList { get; set; }

        public MeetingAttendancePresenter(IMeetingAttendanceView view)
        {
            _meetingRepository = new MeetingRepository();
            _memberRepository = new MemberRepository();
            _officerRepository = new OfficerRepository();
            _view = view;
            LoadMemberCheckListWithCurrent();
        }

        private void LoadMemberCheckListWithCurrent()
        {
            var currentMembers = _memberRepository.GetCurrentMembers().ToList();
            _view.MemberCheckList = currentMembers.Select(member => new SelectableItem(member.MemberId, member.LFName, notSelected)).ToList();
        }

        public void LoadMeetingAttendance(Guid meetingId)
        {
            var attendingMembers = _meetingRepository.GetMeetingAttendees(meetingId);

            var attendingNonCurrentMembers = attendingMembers.Where(att => !_view.MemberCheckList.Contains(att)).ToList();
            if (attendingNonCurrentMembers.Any())
                AddMissingMembersToMemberList(attendingNonCurrentMembers);
            CheckMembersInAttendance(attendingMembers);
        }
        private void AddMissingMembersToMemberList(IEnumerable<SelectableItem> additionalMembers)
        {
            foreach (var additionalMember in additionalMembers)
            {
                if (_view.MemberCheckList.Contains(additionalMember)) continue;
                _view.MemberCheckList.Add(additionalMember);
            }
            _view.MemberCheckList = _view.MemberCheckList.OrderBy(x => x.Name).ToList();
        }
        private void CheckMembersInAttendance(IEnumerable<SelectableItem> attendees)
        {
            foreach (var attendee in attendees)
            {
                _view.MemberCheckList.First(x => x.ItemId == attendee.ItemId).IsSelected = true;
            }
        }

        public ICollection<Tuple<Guid, string>> GetInChargeList(int meetingYear)
        {
            var officerList = _officerRepository.GetOfficersByYear(meetingYear)
                .Where(off => off.OfficeRec.GroupId > 20).ToList();
            foreach (var mem in _view.MemberCheckList)
            {
                if (officerList.Any(office => office.MemberId == mem.ItemId)) continue;
                officerList.Add(new Officer(_memberRepository.Get(mem.ItemId), 
                    new Office(99,"",99,99,true),1900,DateTime.Now, DateTime.Now));
            }
            return officerList.OrderBy(x => x.OfficeId).
                Select(off => new Tuple<Guid, string>(off.MemberId, off.MemberRec.LFName)).ToList();
        }

    }
}
