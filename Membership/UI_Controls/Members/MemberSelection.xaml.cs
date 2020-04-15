using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Membership.Core.DataModels;
using Membership.Core.Presenters;
using Membership.Properties;
using Image = System.Drawing.Image;
using static Membership.Common.CommonBaseClass;

namespace Membership.UI_Controls.Members
{

    public partial class MemberSelection : IMemberSelectionView, INotifyPropertyChanged
    {
        private readonly MemberSelectionPresenter _presenter;

        #region Dependency Properties
        public Member SelectedMember
        {
            get { return (Member)GetValue(SelectedMemberProperty); }
            set { SetValue(SelectedMemberProperty, value); }
        }
        public static readonly DependencyProperty SelectedMemberProperty =
            DependencyProperty.Register("SelectedMember", typeof(Member),
                typeof(MemberSelection));

        public ICollection<Member> Members
        {
            get { return (ICollection<Member>)GetValue(MembersProperty); }
            set { SetValue(MembersProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty MembersProperty =
            DependencyProperty.Register("MembersProperty", typeof(ICollection<Member>), typeof(MemberSelection));

        public IEnumerable<MemberType> MemberTypes
        {
            get { return (IEnumerable<MemberType>)GetValue(MemberTypesProperty); }
            set { SetValue(MemberTypesProperty, value); }
        }
        public static readonly DependencyProperty MemberTypesProperty =
            DependencyProperty.Register("MemberTypes", typeof(IEnumerable<MemberType>), typeof(MemberSelection));

        public IEnumerable<Member> FilteredMembers
        {
            get { return (IEnumerable<Member>)GetValue(FilteredMembersProperty); }
            set
            {
                SetValue(FilteredMembersProperty, value);
                FilteredMembersCount = value.Count();
                OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty FilteredMembersProperty =
            DependencyProperty.Register("FilteredMembers", typeof(IEnumerable<Member>), typeof(MemberSelection));

        public int FilteredMembersCount
        {
            get { return (int)GetValue(FilteredMembersCountProperty); }
            set { SetValue(FilteredMembersCountProperty, value); }
        }
        public static readonly DependencyProperty FilteredMembersCountProperty =
            DependencyProperty.Register("FilteredMembersCount", typeof(int), typeof(MemberSelection));

        #endregion

        public MemberSelection()
        {
            FilteredMembers = new ObservableCollection<Member>();
            InitializeComponent();
            _presenter = new MemberSelectionPresenter(this);
            _presenter.LoadMemberTypes();
            FilterCurrent.IsChecked = true;
        }

        public void LoadMembers()
        {
            _presenter.LoadMembers();
            FilteredMembers = Members;
            if (SelectedMember == null)
                MemberSelectionGrid.SelectedIndex = 0;
            ApplyFilterSettings();
        }

        private void MemberGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dg = (DataGrid) sender;
            if (!(dg.SelectedItem is Member selectedMemberRec)) return;
            SelectedMember = (Member)dg.SelectedItem;
            DuesPaidControl.LoadDuesPaidRecords(selectedMemberRec.MemberId);
            OfficesHeldControl.LoadOfficeRecords(selectedMemberRec.MemberId);
            MemberInfoControl.MemberRec = selectedMemberRec;
        }


        private void ApplyFilterSettings()
        {
            FilteredMembers = Members;
            if (FilterFirstName.Text != "" || FilterLastName.Text != "")
                FilteredMembers = Members.Where(x =>
                    x.FirstName.StartsWith(FilterFirstName.Text, StringComparison.OrdinalIgnoreCase) &&
                    x.LastName.StartsWith(FilterLastName.Text, StringComparison.OrdinalIgnoreCase) );
            if (FilterCurrent.IsChecked == true)
                FilteredMembers = FilteredMembers.Where(y => y.IsCurrent);
            if (FilterStatus.SelectedIndex > 0)
                FilteredMembers = FilteredMembers.Where(y => y.MemberTypeId == (int) FilterStatus.SelectedValue);
            if (FilterObligated.Text.Length == 4)
            {
                var obligationYear = Convert.ToInt32(FilterObligated.Text);
                if (obligationYear >= 1900 && obligationYear < 2100)
                    FilteredMembers = FilteredMembers.Where(y =>
                        y.DateObligated != null && y.DateObligated.Value.Year == obligationYear);
            }
            if (FilterRemoved.Text.Length == 4)
            {
                var removalYear = Convert.ToInt32(FilterRemoved.Text);
                if (removalYear >= 1900 && removalYear < 2100)
                    FilteredMembers = FilteredMembers.Where(y =>
                        y.DateRemoved != null && y.DateRemoved.Value.Year == removalYear);
            }
        }
        private void FilterTextBoxOnTextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilterSettings();
        }
        private void FilterComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilterSettings();
        }
        private void FilterCheckBoxOnClick(object sender, RoutedEventArgs e)
        {
            ApplyFilterSettings();
        }
        private void FilterRemovedTextChanged(object sender, TextChangedEventArgs e)
        {
            if (FilterRemoved.Text.Length < 4) return;

            var removedYear = Convert.ToInt32(FilterRemoved.Text);
            if (removedYear < 1900 || removedYear > 2100) return;
            ApplyFilterSettings();
        }
        private void FilterObligatedTextChanged(object sender, TextChangedEventArgs e)
        {
            if (FilterObligated.Text.Length < 4) return;

            var obligationYear = Convert.ToInt32(FilterObligated.Text);
            if (obligationYear < 1900 || obligationYear > 2100) return;
            ApplyFilterSettings();
        }


        private void MemberInfoControl_OnUnloaded(object sender, RoutedEventArgs e)
        {
            //if (!MemberInfoControl.IsEditing) return;
            //if (MessageBox.Show("Do you want to save your changes before leaving?", "Save or Cancel", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            //    MemberInfoControl.SaveMemberRecord();
            //else
            //    MemberInfoControl.CancelMemberRecord();
        }
        private void MemberInfoControl_OnOnMemberUpdated(object sender, RoutedEventArgs e)
        {
            LoadMembers();
            ApplyFilterSettings();
        }
        private void MemberInfoControl_OnOnMemberDeleted(object sender, RoutedEventArgs e)
        {
            var x = MemberSelectionGrid.SelectedIndex;
            Members.Remove(SelectedMember);
            if (MemberSelectionGrid.Items.Count > 1)
                MemberSelectionGrid.SelectedIndex = x - 1;
            SelectedMember = (Member)MemberSelectionGrid.SelectedItem;
            
            ApplyFilterSettings();
        }

        private void ClearSelectionOnClick(object sender, RoutedEventArgs e)
        {
            FilterCurrent.IsChecked = true;
            FilterLastName.Text = "";
            FilterFirstName.Text = "";
            FilterStatus.SelectedIndex = 0;
            FilterObligated.Text = "";
            FilterRemoved.Text = "";
            ApplyFilterSettings();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void BulkLoadMemberCardImages()
        {
            var cnt = 0;
            Image img;
            foreach (var mem in Members)
            {
                var filename = "C:\\Membership Cards\\" + mem.LastName + ", " + mem.FirstName + ".jpg";
                try
                { 
                    img = Image.FromFile(filename);
                }
                catch (Exception ex)
                {
                    var x = ex;
                    Debug.Print($"Filename not found - {filename}");
                    continue;
                }
                var imgByte = ImageToByteArray(img);
                mem.PageThumb = ImageToByteArray(ScaleImage(img, 300));
                MemberInfoControl.MemberSave(mem, imgByte);
                Debug.Print(filename);
                cnt += 1;

            }

            Debug.Print($"Total processed - {cnt}");
        }



    }
}
