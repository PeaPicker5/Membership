using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dapper.Contrib.Extensions;
using Membership.Properties;

namespace Membership.Core.Members.DataModels
{
    [Table("MEMBER_List")]
    public class SelectableMember : Member, INotifyPropertyChanged
    {
        public SelectableMember() { }
        public enum enumCheckStatus { Original, Added, Removed }

        private bool _isSelected;
        public SelectableMember(Member member, bool isSelected)
            : base(
                member.MemberId, member.MemberTypeId, member.IsActive, member.LastName, member.FirstName,
                member.MI, member.Suffix, member.Comment, member.EmailAddress, member.SponsorID1, member.SponsorID2, member.SponsorID3,
                member.Address1, member.Address2, member.City, member.State, member.ZIP, member.Phone,
                member.DateOfBirth, member.DateObligated, member.DateRemoved, member.RemovalReasonId, member.DateDeceased,
                member.PageId, member.PageThumb)
        {
            IsSelected = isSelected;
        }

        [Computed] public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        [Computed] public enumCheckStatus CheckStatus { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
