using System.Linq;
using Membership.Core.LOSAP.DataModels;
using Membership.Core.LOSAP.Repositories;
using Membership.Core.Members.Repositories;

namespace Membership.Core.LOSAP.Presenters
{
    /// <summary>
    /// LOSAP listing: every current Active member with their credited, legacy and total years.
    /// </summary>
    public class LosapListingPresenter
    {
        private const int ActiveMemberType = 1;

        private readonly ILosapRepository _losapRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly ILosapListingView _view;

        public LosapListingPresenter(ILosapListingView view)
        {
            _losapRepository = new LosapRepository();
            _memberRepository = new MemberRepository();
            _view = view;
        }

        public void LoadMemberRecords()
        {
            var yearCounts = _losapRepository.GetCreditedYearCounts();

            _view.LosapRecs = _memberRepository.GetCurrentMembers()
                .Where(m => m.MemberTypeId == ActiveMemberType)
                .Select(m => new LosapRecord(m, 0, false)
                {
                    LosapYears = yearCounts.TryGetValue(m.MemberId, out var count) ? count : 0
                })
                .OrderByDescending(r => r.CombinedTotalYears)
                .ThenBy(r => r.MemberRec.LastName)
                .ThenBy(r => r.MemberRec.FirstName)
                .ToList();
        }
    }
}
