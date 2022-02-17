using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.Dues.DataModels;
using Membership.Core.Dues.Repositories;
using Membership.Core.Repositories;

namespace Membership.Core.Dues.Presenters
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
            _view.DuesRecs = _duesRepository.GetDuesRecordByMemberId(memberId).OrderBy(x => x.Year).ToList();
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
            var recordsToUpdate = _view.DuesRecs.Where(rec => rec.IsPaid && rec.Year == 0).ToList();
            
            foreach (var r in recordsToUpdate)
            {
                r.Month = DateTime.Today.Month;
                r.Year = DateTime.Today.Year;
            }

            _duesRepository.InsertDuesPayments(recordsToUpdate);
        }


        public void MembersCurrentlyOweDues()
        {
            _view.DuesRecs = _duesRepository.CurrentlyOweDues().ToList();
        }




    }
}
