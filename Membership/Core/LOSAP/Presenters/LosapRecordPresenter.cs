using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.LOSAP.DataModels;
using Membership.Core.LOSAP.Repositories;

namespace Membership.Core.LOSAP.Presenters
{
    public class LosapRecordPresenter : ILosapRecordView
    {
        public ICollection<LosapRecord> LosapRecs { get; set; }
        private readonly ILosapRepository _losapRepository;
        private readonly ILosapRecordView _view;

        public LosapRecordPresenter(ILosapRecordView view)
        {
            _losapRepository = new LosapRepository();
            _view = view;
        }

        public void GetByMemberId(Guid memberId)
        {
            _view.LosapRecs = _losapRepository.GetLosapRecordsByMemberId(memberId).OrderBy(x => x.Year).ToList();
        }

        public IEnumerable<int> LoadYearsOnFile()
        {
            return _losapRepository.GetYearsOnFile();
        }

        public IEnumerable<LosapRecord> GetByYear(int year)
        {
            return _losapRepository.GetLosapByYear(year);
        }

        public void InsertLosapRecords()
        {
            var recordsToUpdate = _view.LosapRecs.Where(rec => rec.HasCredit && rec.Year == 0).ToList();
            
            foreach (var r in recordsToUpdate)
            {
                r.Year = DateTime.Today.Year;
            }

            _losapRepository.InsertLosapCredits(recordsToUpdate);
        }


        public void GetCurrentMembers()
        {
            _view.LosapRecs = _losapRepository.CurrentActiveMembers().ToList();
        }




    }
}
