using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Membership.Annotations;

namespace Membership.Core.DataModels
{
    public class DuesRecord : INotifyPropertyChanged
    {
        public DuesRecord() { }
        public DuesRecord(Member memberRec, int year, int month, float amount, bool isPaid)
        {
            MemberRec = memberRec;
            Year = year;
            Month = month;
            Amount = amount;
            IsPaid = isPaid;
        }

        public Member MemberRec { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public float Amount { get; set; }
        public bool IsPaid { get; set; }


        public string PaidOnDate
        {
            get
            {
                if (Year == 0) return "";
                if (Month > 0)
                {
                    return DateTime.ParseExact($"{Month}/{Year}", "M'/'yyyy", CultureInfo.InvariantCulture).Date
                        .ToString("MM/yyyy");
                }
                return DateTime.ParseExact($"01/{Year}", "M'/'yyyy", CultureInfo.InvariantCulture).Date
                    .ToString("yyyy");
            }
            set {}
        }

        public Guid MemberId => MemberRec.MemberId;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
