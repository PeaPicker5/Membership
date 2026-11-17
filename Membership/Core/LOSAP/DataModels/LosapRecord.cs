using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Membership.Annotations;

namespace Membership.Core.LOSAP.DataModels;

/// <summary>
/// One member's LOSAP standing, optionally for a specific year.
/// On the credit entry screen, <see cref="HasCredit"/> is whether the member is credited
/// for <see cref="Year"/>; <see cref="OriginalHasCredit"/> is what the database holds,
/// so the screen can tell which rows changed.
/// </summary>
public class LosapRecord : INotifyPropertyChanged
{
    public LosapRecord() { }
    public LosapRecord(Members.DataModels.Member memberRec, int year, bool hasCredit)
    {
        MemberRec = memberRec;
        Year = year;
        _hasCredit = hasCredit;
        OriginalHasCredit = hasCredit;
    }

    public Members.DataModels.Member MemberRec { get; set; }
    public int Year { get; set; }

    private bool _hasCredit;
    public bool HasCredit
    {
        get => _hasCredit;
        set
        {
            if (value == _hasCredit) return;
            _hasCredit = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsChanged));
        }
    }

    /// <summary>Credit state as stored in LOSAP_History when the screen was loaded.</summary>
    public bool OriginalHasCredit { get; private set; }
    public bool IsChanged => HasCredit != OriginalHasCredit;

    private int _losapYears;
    /// <summary>Number of years the member has been credited in LOSAP_History.</summary>
    public int LosapYears
    {
        get => _losapYears;
        set
        {
            if (value == _losapYears) return;
            _losapYears = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CombinedTotalYears));
        }
    }

    /// <summary>Credited years plus legacy years; legacy years can be fractional (e.g. 9.5).</summary>
    public float CombinedTotalYears => LosapYears + MemberRec.LegacyLosapYears;

    public Guid MemberId => MemberRec.MemberId;

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
