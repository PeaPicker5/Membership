using System;
using System.Collections.Generic;
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
            _view.DuesRecs = _duesRepository.GetDuesByMember(memberId);
        }

        public IEnumerable<int> LoadYearsOnFile()
        {
            return _duesRepository.GetYearsOnFile();
        }

        public IEnumerable<DuesRecord> GetByYear(int year)
        {
            return _duesRepository.GetDuesByYear(year);
        }

        public void UpdateDuesRecords()
        {
            var recs = _view.DuesRecs.Where(rec => rec.IsPaid && rec.Year == 0).ToList();
            var x = MapFromData(recs);
            _duesRepository.UpdateDuesPayments(x);
        }
        public void MembersCurrentlyOweDues()
        {
            _view.DuesRecs = _duesRepository.CurrentlyOweDues().ToList();
        }


        public List<DuesHistory> MapFromData(IEnumerable<DuesRecord> duesRecords)
        {
            //foreach (var rec in duesRecords)
            //    retValue.Add(new DuesRecord()
            //    {
            //        MemberRec = rec.MemberRec,
            //        Amount = rec.Amount,
            //        Month = rec.Month,
            //        Year = rec.Year
            //    });

            var x = duesRecords.Select(rec => new DuesHistory()
            {
                MemberId = rec.MemberRec.MemberId, 
                Amount = rec.Amount, 
                Month = DateTime.Now.Month, 
                Year = DateTime.Now.Year
            }).ToList();
            return x;
        }

    }
}
