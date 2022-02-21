using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.Meetings.DataModels;
using Membership.Core.Meetings.Repositories;

namespace Membership.Core.Meetings.Presenters
{
    public class MeetingSelectionPresenter : IMeetingSelectionView
    {
        private readonly IMeetingSelectionView _view;
        private readonly IMeetingRepository _meetingRepository;
        public ICollection<Meeting> MeetingList { get; set; }

        public MeetingSelectionPresenter(IMeetingSelectionView view)
        {
            _meetingRepository = new MeetingRepository();
            _view = view;
        }

        public void LoadMeetings(int year = 0)
        {
            _view.MeetingList = year > 0 
                ? _meetingRepository.GetMeetings().Where(x => x.MeetingDate.Year == year).OrderByDescending(x => x.MeetingDate).ToList()
                : _meetingRepository.GetMeetings().OrderByDescending(x => x.MeetingDate).ToList();
        }

        public IEnumerable<int> LoadYearsOnFile()
        {
            return _meetingRepository.GetYearsOnFile();
        }


        public Meeting GetByMeetingId(Guid meetingId)
        {
            return _meetingRepository.Get(meetingId);
        }

    }
}
