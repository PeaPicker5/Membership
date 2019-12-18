using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Membership.Core.DataModels;
using Membership.Core.Repositories;

namespace Membership.Core.Presenters
{
    public class DuesPresenter : IDuesRecordView
    {
        public ICollection<DuesRecord> DuesRecs { get; set; }
        private readonly IDuesRepository _duesRepository;
        private readonly IDuesRecordView _view;

        public DuesPresenter(IDuesRecordView view)
        {
            _duesRepository = new DuesRepository();
            _view = view;
        }

        public void GetByMemberId(Guid memberId)
        {
            _view.DuesRecs = _duesRepository.GetDuesRecordByMemberId(memberId);
        }

        public IEnumerable<int> LoadYearsOnFile()
        {
            return _duesRepository.GetYearsOnFile();
        }

        public IEnumerable<DuesRecord> GetByYear(int year)
        {
            return _duesRepository.GetDuesByYear(year);
        }

        public void InsertDuesRecords()
        {
            _duesRepository.InsertDuesPayments(_view.DuesRecs.Where(rec => rec.IsPaid && rec.Year == 0).ToList());
        }


        public void MembersCurrentlyOweDues()
        {
            _view.DuesRecs = _duesRepository.CurrentlyOweDues().ToList();
        }




    }
}
