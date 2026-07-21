using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.LOSAP.DataModels;
using Membership.Core.LOSAP.Repositories;
using Membership.Core.Members.DataModels;
using Membership.Core.Members.Repositories;

namespace Membership.Core.LOSAP.Presenters
{
    public class LosapRecordPresenter : ILosapRecordView
    {
        public ICollection<LosapRecord> LosapRecs { get; set; }
        public ICollection<Member> MemberRecs { get; set; }
        private readonly ILosapRepository _losapRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly ILosapRecordView _view;

        public LosapRecordPresenter(ILosapRecordView view)
        {
            _losapRepository = new LosapRepository();
            _memberRepository = new MemberRepository();
            _view = view;
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
            var countByMember = _losapRepository.GetLosapForActiveMembers()
                .GroupBy(r => r.MemberId)
                .ToDictionary(g => g.Key, g => g.Count());

            _view.LosapRecs = _memberRepository.GetCurrentMembers()
                .Where(m => m.MemberTypeId == 1)
                .OrderBy(m => m.LastName)
                .Select(m =>
                {
                    var rec = new LosapRecord(m, 0, false);
                    if (countByMember.TryGetValue(m.MemberId, out var count))
                        rec.LosapYears = count;
                    return rec;
                })
                .ToList();
        }




    }
}
