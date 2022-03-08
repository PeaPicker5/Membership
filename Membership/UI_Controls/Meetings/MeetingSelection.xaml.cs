using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Membership.Core.DataModels;
using Membership.Core.Meetings.DataModels;
using Membership.Core.Meetings.Presenters;

namespace Membership.UI_Controls.Meetings
{

    public partial class MeetingSelection : IMeetingSelectionView, INotifyPropertyChanged
    {
        private readonly MeetingSelectionPresenter _presenter;
        private MeetingAttendance ucAttendanceControl;


        #region Dependency Properties
        public ICollection<Meeting> MeetingList 
        {
            get => (ICollection<Meeting>)GetValue(MeetingListProperty);
            set
            {
                SetValue(MeetingListProperty, value); 
                OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty MeetingListProperty =
            DependencyProperty.Register("MeetingList", typeof(ICollection<Meeting>), typeof(MeetingSelection));

        public ICollection<SelectableItem> Attendees
        {
            get => (ICollection<SelectableItem>)GetValue(AttendeeNamesProperty);
            set { SetValue(AttendeeNamesProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty AttendeeNamesProperty =
            DependencyProperty.Register("Attendees", typeof(ICollection<SelectableItem>), typeof(MeetingSelection));

        public bool IsEditing
        {
            get => (bool)GetValue(IsEditingProperty);
            set { SetValue(IsEditingProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty IsEditingProperty =
            DependencyProperty.Register("IsEditing", typeof(bool), typeof(MeetingSelection));

        public bool IsAdding
        {
            get => (bool)GetValue(IsAddingProperty);
            set { SetValue(IsAddingProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty IsAddingProperty =
            DependencyProperty.Register("IsAdding", typeof(bool), typeof(MeetingSelection));


        #endregion

        public MeetingSelection()
        {
            MeetingList = new ObservableCollection<Meeting>();
            Attendees = new ObservableCollection<SelectableItem>();
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


        private void MeetingGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MeetingSelectionGrid.SelectedItem == null) return;
            RefreshNamesList();
        }

        private void RefreshNamesList()
        {
            Attendees = _presenter.GetMeetingAttendees(((Meeting)MeetingSelectionGrid.SelectedItem).MeetingId);
        }

        private void ShowMeetingEditControl()
        {
            MainGrid.RowDefinitions[0].Height = new GridLength(0);
            MainGrid.RowDefinitions[1].Height = new GridLength(0);
            MainGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
        }

        private void HideMeetingEditControl()
        {
            MainGrid.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
            MainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
            MainGrid.RowDefinitions[2].Height = new GridLength(0);
        }

        private void EditMeetingButton_OnClick(object sender, RoutedEventArgs e)
        {
            IsEditing = true;
            ucAttendanceControl = new MeetingAttendance
            {
                CurrentMeeting = (Meeting) MeetingSelectionGrid.SelectedItem
            };

            AddEditStack.Children.Clear();
            AddEditStack.Children.Add(ucAttendanceControl);
            ShowMeetingEditControl();
        }

        private void AddMeetingButton_OnClick(object sender, RoutedEventArgs e)
        {
            IsAdding = true;
            IsEditing = true;
            
            ucAttendanceControl = new MeetingAttendance
            {
                CurrentMeeting = new Meeting(Guid.NewGuid(), DateTime.Now, $"{DateTime.Now:MMMM} Monthly Meeting", "", Guid.Empty, true)
            };
            AddEditStack.Children.Clear();
            AddEditStack.Children.Add(ucAttendanceControl);
            ShowMeetingEditControl();
        }

        private void DeleteMeetingButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this Meeting?", "User Action Is Required",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes) 
                return;

            var selectedMeetingRec = (Meeting) MeetingSelectionGrid.SelectedItem;
            _presenter.DeleteMeeting(selectedMeetingRec);
            MeetingList.Remove(selectedMeetingRec);
            LoadMeetings();
        }

        private bool ValidateData()
        {
            var currMeetingDate = ucAttendanceControl.CurrentMeeting.MeetingDate.ToString("MM/DD/YYYY");
            if (MeetingList.Where(meeting => meeting.MeetingDate.ToString("MM/DD/YYYY") == currMeetingDate)
                .Any(meeting => MessageBox.Show($"There is already a meeting record for this date.  Is this meeting different from {meeting.Description}?",
                "User Action Is Required", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes))
            {
                return false;
            }

            return true;
        }
        private void SaveMemberButton_OnClick(object sender, RoutedEventArgs e)
        {
            _presenter.SaveMeeting(ucAttendanceControl.CurrentMeeting, ucAttendanceControl.Attendees, IsAdding);
            if (IsAdding)
            {
                LoadMeetings();
            }
            IsEditing = false;
            IsAdding = false;
            HideMeetingEditControl();
            RefreshNamesList();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            IsEditing = false;
            IsAdding = false;
            LoadMeetings();
            HideMeetingEditControl();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        [Annotations.NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
