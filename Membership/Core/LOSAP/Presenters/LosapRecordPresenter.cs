using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.LOSAP.DataModels;
using Membership.Core.LOSAP.Repositories;
using Membership.Core.Members.Repositories;

namespace Membership.Core.LOSAP.Presenters
{
    /// <summary>
    /// LOSAP credit entry: shows who is credited for a chosen year and saves changes.
    /// </summary>
    public class LosapRecordPresenter
    {
        private const int ActiveMemberType = 1;
        private const int YearsBackToOffer = 10;

        private readonly ILosapRepository _losapRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly ILosapRecordView _view;

        public LosapRecordPresenter(ILosapRecordView view)
        {
            _losapRepository = new LosapRepository();
            _memberRepository = new MemberRepository();
            _view = view;
        }

        /// <summary>The year credit is normally entered for: last calendar year.</summary>
        public static int DefaultYear => DateTime.Today.Year - 1;

        /// <summary>
        /// Years to offer: this year back <see cref="YearsBackToOffer"/> years,
        /// plus any older year already on file, newest first.
        /// </summary>
        public IList<int> GetSelectableYears()
        {
            var thisYear = DateTime.Today.Year;
            return Enumerable.Range(thisYear - YearsBackToOffer, YearsBackToOffer + 1)
                .Union(_losapRepository.GetYearsOnFile())
                .OrderByDescending(y => y)
                .ToList();
        }

        /// <summary>
        /// Loads current Active members, plus anyone already credited for <paramref name="year"/>
        /// (so a credit can still be removed after a member's status changes).
        /// </summary>
        public void LoadYear(int year)
        {
            var yearCounts = _losapRepository.GetCreditedYearCounts();
            var creditedThisYear = _losapRepository.GetCreditedMemberIds(year);

            _view.LosapRecs = _memberRepository.GetMembers()
                .Where(m => (m.IsCurrent && m.MemberTypeId == ActiveMemberType)
                            || creditedThisYear.Contains(m.MemberId))
                .Select(m => new LosapRecord(m, year, creditedThisYear.Contains(m.MemberId))
                {
                    LosapYears = yearCounts.TryGetValue(m.MemberId, out var count) ? count : 0
                })
                .OrderByDescending(r => r.CombinedTotalYears)
                .ThenBy(r => r.MemberRec.LastName)
                .ThenBy(r => r.MemberRec.FirstName)
                .ToList();
        }

        /// <summary>Saves every row whose checkbox differs from the database.</summary>
        public void SaveChanges(int year)
        {
            var changed = _view.LosapRecs.Where(r => r.IsChanged).ToList();
            if (!changed.Any()) return;

            _losapRepository.SaveCredits(year,
                changed.Where(r => r.HasCredit).Select(r => r.MemberId),
                changed.Where(r => !r.HasCredit).Select(r => r.MemberId));
        }
    }
}
