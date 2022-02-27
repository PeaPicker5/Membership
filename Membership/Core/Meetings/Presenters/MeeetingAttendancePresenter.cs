using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.Meetings.DataModels;
using Membership.Core.Members.DataModels;
using Membership.Core.Members.Repositories;
using Membership.Core.Officers.DataModels;
using Membership.Core.Officers.Repositories;

namespace Membership.Core.Meetings.Presenters
{
    public class MeetingAttendancePresenter :IMeetingAttendanceView
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IOfficerRepository _officerRepository;
        private readonly IMeetingAttendanceView _view;
        public Meeting CurrentMeeting { get; set; }
        public ICollection<SelectableMember> MemberCheckList { get; set; }

        public MeetingAttendancePresenter(IMeetingAttendanceView view)
        {
            _memberRepository = new MemberRepository();
            _officerRepository = new OfficerRepository();
            _view = view;
            LoadMemberLists();
        }

        private void LoadMemberLists()
        {
            const bool notSelected = false;
            
            var currentMembers = _memberRepository.GetCurrentMembers().ToList();
            _view.MemberCheckList = currentMembers.Select(member => new SelectableMember(member, notSelected)).ToList();
        }


        public void UpdateCurrentMeetingAttendance(ICollection<SelectableMember> attendees)
        {
            var additionalMembers = attendees
                .Where(att => _view.MemberCheckList.All(mem => mem.MemberId != att.MemberId));
            AddMembersToMemberList(additionalMembers);

            foreach (var attendee in attendees)
            {
                var found = _view.MemberCheckList.ToList().Find(x => x.MemberId == attendee.MemberId);
                found.IsSelected = attendee.IsSelected;
            }

        }
        private void AddMembersToMemberList(IEnumerable<SelectableMember> additionalMembers)
        {
            foreach (var additionalMember in additionalMembers)
            {
                if (_view.MemberCheckList.Contains(additionalMember)) continue;
                _view.MemberCheckList.Add(additionalMember);
            }
            _view.MemberCheckList = _view.MemberCheckList.OrderBy(x => x.LFName).ToList();
        }

        public ICollection<Tuple<Guid, string>> GetInChargeList(int meetingYear)
        {
            var officerList = _officerRepository.GetOfficersByYear(meetingYear)
                .Where(off => off.OfficeRec.GroupId > 20).ToList();
            foreach (var mem in _view.MemberCheckList)
            {
                if (officerList.Any(office => office.MemberId == mem.MemberId)) continue;
                officerList.Add(new Officer(mem,new Office(99,"",99,99,true),1900,DateTime.Now, DateTime.Now));
            }
            return officerList.OrderBy(x => x.OfficeId).
                Select(off => new Tuple<Guid, string>(off.MemberId, off.MemberRec.LFName)).ToList();
        }
    }
}
