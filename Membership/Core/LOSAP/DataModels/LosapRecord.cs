using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Membership.Annotations;

namespace Membership.Core.LOSAP.DataModels
{
    public class LosapRecord : INotifyPropertyChanged, IEquatable<LosapRecord>
    {
        public LosapRecord() { }
        public LosapRecord(Members.DataModels.Member memberRec, int year,  bool hasCredit)
        {
            MemberRec = memberRec;
            Year = year;
            HasCredit = hasCredit;
        }

        public Members.DataModels.Member MemberRec { get; set; }
        public int Year { get; set; }
        public bool HasCredit { get; set; }


        public int TotalYears { get; set; }

        public Guid MemberId => MemberRec.MemberId;

        public bool Equals(LosapRecord other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return MemberId == other.MemberId && Year == other.Year;
        }

        public override bool Equals(object obj) => Equals(obj as LosapRecord);

        public override int GetHashCode()
        {
            unchecked
            {
                return (MemberId.GetHashCode() * 397) ^ Year.GetHashCode();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}