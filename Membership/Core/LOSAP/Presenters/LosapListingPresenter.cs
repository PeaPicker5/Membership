using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.LOSAP.DataModels;
using Membership.Core.LOSAP.Repositories;

namespace Membership.Core.LOSAP.Presenters
{
    public class LosapListingPresenter : ILosapListingView
    {
        public ICollection<LosapRecord> LosapRecs { get; set; }
        private readonly ILosapRepository _losapRepository;
        private readonly ILosapListingView _view;

        public LosapListingPresenter(ILosapListingView view)
        {
            _losapRepository = new LosapRepository();
            _view = view;
        }

        public IEnumerable<LosapRecord> GetByYear(int year)
        {
            return _losapRepository.GetLosapByYear(year);
        }

        public void LoadMemberRecords()
    {
            var losapRecs = _losapRepository.GetLosapForActiveMembers();
            if (losapRecs == null || !losapRecs.Any()) return;

            var countByMember = losapRecs
                .GroupBy(r => r.MemberId)
                .ToDictionary(g => g.Key, g => g.Select(r => r.Year).Distinct().Count());

            foreach (var rec in losapRecs)
            {
                rec.TotalYears = countByMember[rec.MemberId];
                rec.Year = 0;  // To allow .Distinct to filter out duplicate records
            }
            _view.LosapRecs = losapRecs.Distinct().ToList();
        }

    }
}
