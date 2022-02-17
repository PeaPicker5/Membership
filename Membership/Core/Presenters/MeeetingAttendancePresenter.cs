﻿using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.DataModels;
using Membership.Core.Repositories;

namespace Membership.Core.Presenters
{
    public class MeetingAttendancePresenter :IMeetingAttendanceView
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMeetingRepository _meetingRepository;
        private readonly IMemberTypeRepository _memberTypeRepository;
        private readonly IMeetingAttendanceView _view;
        public Meeting CurrentMeeting { get; set; }
        public ICollection<Member> MemberList { get; set; }
        public ICollection<Member> AdditionalMembers { get; set; }

        public MeetingAttendancePresenter(IMeetingAttendanceView view)
        {
            _meetingRepository = new MeetingRepository();
            _memberTypeRepository = new MemberTypeRepository();
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
