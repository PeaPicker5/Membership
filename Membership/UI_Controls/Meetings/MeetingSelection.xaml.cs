using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Membership.Core.Meetings.DataModels;
using Membership.Core.Meetings.Presenters;
using Membership.Properties;

namespace Membership.UI_Controls.Meetings
{

    public partial class MeetingSelection : IMeetingSelectionView, INotifyPropertyChanged
    {
        private readonly MeetingSelectionPresenter _presenter;


        #region Dependency Properties


        public ICollection<Meeting> MeetingList 
        {
            get => (ICollection<Meeting>)GetValue(MeetingListProperty);
            set { SetValue(MeetingListProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty MeetingListProperty =
            DependencyProperty.Register("MeetingListProperty", typeof(ICollection<Meeting>), typeof(MeetingSelection));

        public ICollection<string> AttendeeNames
        {
            get => (ICollection<string>)GetValue(AttendeeNamesProperty);
            set { SetValue(AttendeeNamesProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty AttendeeNamesProperty =
            DependencyProperty.Register("AttendeeNames", typeof(ICollection<string>), typeof(MeetingSelection));

        #endregion

        public MeetingSelection()
        {
            MeetingList = new ObservableCollection<Meeting>();
            AttendeeNames = new ObservableCollection<string>();
            InitializeComponent();
            _presenter = new MeetingSelectionPresenter(this);
            LoadMeetings();
            SetMinAndMaxYear();
        }

        private void LoadMeetings(int year = 0)
        {
            _presenter.LoadMeetings(year);
            if (MeetingList.Any())
                MeetingSelectionGrid.SelectedIndex = 0;
        }

        private void SetMinAndMaxYear()
        {
            var minYear = 2099;
            var maxYear = 1900;
            foreach (var m in MeetingList)
            {
                if (m.MeetingDate.Year < minYear)
                    minYear = m.MeetingDate.Year;
                if (m.MeetingDate.Year > maxYear)
                    maxYear = m.MeetingDate.Year;
            }
            YearUpDown.Minimum = minYear;
            YearUpDown.Maximum = maxYear;
        }

        private void MeetingGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MeetingSelectionGrid.SelectedItem == null) return;
            AttendeeNames.Clear();
            var att = ((Meeting)MeetingSelectionGrid.SelectedItem).Attendees;
            foreach (var mem in att.OrderBy(x => x.LFName))
                AttendeeNames.Add(mem.LFName);
        }

        private void YearUpDown_Changed(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            LoadMeetings(Convert.ToInt32(e.NewValue));
        }


        private void YearFilter_OnChanged(object sender, RoutedEventArgs e)
        {
            if (YearFilterCheckBox.IsChecked == true)
                YearUpDown.Value = YearUpDown.Maximum;
            else
                YearUpDown.Value = null;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void EditMeetingButton_OnClick(object sender, RoutedEventArgs e)
        {
            MainGrid.RowDefinitions[0].Height = new GridLength(0);
            MainGrid.RowDefinitions[1].Height = new GridLength(0);
            MainGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
            var ucMeetingAddUpdate = new MeetingAttendance { CurrentMeeting = (Meeting) MeetingSelectionGrid.SelectedValue };
            AddEditStack.Children.Clear();
            AddEditStack.Children.Add(ucMeetingAddUpdate);
        }

        private void AddMeetingButton_OnClick(object sender, RoutedEventArgs e)
        {
            MainGrid.RowDefinitions[0].Height = new GridLength(0);
            MainGrid.RowDefinitions[1].Height = new GridLength(0);
            MainGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
            var ucMeetingAddUpdate = new MeetingAttendance
            {
                CurrentMeeting = new Meeting(Guid.NewGuid(), DateTime.Now, $"{DateTime.Now:MMMM} Monthly Meeting", "", Guid.Empty, true)
            };
            AddEditStack.Children.Clear();
            AddEditStack.Children.Add(ucMeetingAddUpdate);
        }

        private void DeleteMeetingButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void SaveMemberButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
