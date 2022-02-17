using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Membership.Core.DataModels;
using Membership.Core.Members.DataModels;
using Membership.Core.Members.Presenters;
using Membership.Properties;
using Microsoft.Win32;
using static Membership.Common.CommonBaseClass;
using Image = System.Drawing.Image;
using MessageBox = System.Windows.MessageBox;

namespace Membership.UI_Controls.Members
{
    public partial class MemberInfo : IMemberView, INotifyPropertyChanged
    {

        private byte[] _fullResPage; //Set with Load or Clear Image, Add/Update/Delete on Member Save, Load on Expand Button

        public static IEnumerable<string> StateInits => new List<string>()
        {
            "AK","AL","AR","AZ","CA","CO","CT","DC","DE","FL","GA","HI","IA","ID","IL","IN","KS","KY","LA","MA","MD","ME","MI","MN","MO","MS","MT",
            "NC","ND","NE","NH","NJ","NM","NV","NY","OH","OK","OR","PA","RI","SC","SD","TN","TX","UT","VA","VT","WA","WI","WV","WY"
        };
        private readonly MemberInfoPresenter _presenter;

        private Guid _selectedMemberId;

        #region Dependency Properties

        public Member MemberRec
        {
            get { return (Member)GetValue(MemberRecProperty); }
            set { 
                    SetValue(MemberRecProperty, value);
                    LocalPageThumb = value.PageThumb;
                    OnPropertyChanged();
                }
        }
        public static readonly DependencyProperty MemberRecProperty =
            DependencyProperty.Register("MemberRec", typeof(Member),typeof(MemberInfo));

        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            set { SetValue(IsEditingProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty IsEditingProperty =
            DependencyProperty.Register("IsEditing", typeof(bool), typeof(MemberInfo));

        public bool IsAdding
        {
            get { return (bool)GetValue(IsAddingProperty); }
            set { SetValue(IsAddingProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty IsAddingProperty =
            DependencyProperty.Register("IsAdding", typeof(bool), typeof(MemberInfo));

        public ICollection<Tuple<int,string>> MemberTypeLookups 
        {
            get { return (ICollection<Tuple<int, string>>)GetValue(MemberTypeLookupsProperty); }
            set { SetValue(MemberTypeLookupsProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty MemberTypeLookupsProperty =
            DependencyProperty.Register("MemberTypeLookups", typeof(ICollection<Tuple<int, string>>),
                typeof(MemberInfo));

        public ICollection<Tuple<Guid,string>> SponsorLookups
        {
            get { return (ICollection<Tuple<Guid,string>>)GetValue(SponsorLookupsProperty); }
            set { SetValue(SponsorLookupsProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty SponsorLookupsProperty =
            DependencyProperty.Register("SponsorLookups", typeof(ICollection<Tuple<Guid,string>>),
                typeof(MemberInfo));

        public ICollection<MemberRemoval> RemovalCodes   
        {
            get { return (ICollection<MemberRemoval>)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("RemovalCodes", typeof(ICollection<MemberRemoval>),
                typeof(MemberInfo));

        public byte[] LocalPageThumb    
        {
            get { return (byte[])GetValue(LocalPageThumbProperty); }
            set { SetValue(LocalPageThumbProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty LocalPageThumbProperty =
            DependencyProperty.Register("LocalPageThumb", typeof(byte[]),
                typeof(MemberInfo));

        #endregion

        public MemberInfo()
        {
            SponsorLookups = new ObservableCollection<Tuple<Guid, string>>();
            MemberTypeLookups = new ObservableCollection<Tuple<int, string>>();
            InitializeComponent();
            _presenter = new MemberInfoPresenter(this);
            _presenter.LoadLookups();
        }

        #region Button Events
        private void AddMemberButtonOnClick(object sender, RoutedEventArgs e)
        {
            MemberRec = new Member {MemberId = Guid.NewGuid(),MemberTypeId = 3};
            IsAdding = true;
            IsEditing = true;
            _selectedMemberId = MemberRec.MemberId;
        }

        private void EditMemberButtonOnClick(object sender, RoutedEventArgs e)
        {
            IsEditing = true;
            _selectedMemberId = MemberRec.MemberId;
        }

        private void DeleteMemberButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
                    $"{MemberRec.FullName} will be deleted from the database, as well as all Dues History, Meeting History, and Officer History records! Do you wish to continue?",
                    "Deleting a Member Record", MessageBoxButton.OKCancel, MessageBoxImage.Hand) !=
                MessageBoxResult.OK) return;
 
            // Need to get the officer and dues CRUD working before this part.

            
            if (!_presenter.DeleteMemberRecord(MemberRec)) return;

            IsAdding = false;
            IsEditing = false;
            RaiseEvent(new RoutedEventArgs(OnMemberDeletedEvent));
        }
        private void SaveMemberButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (!VerifyControls()) return;
            SaveMemberRecord();
        }
        private void CancelButtonOnClick(object sender, RoutedEventArgs e)
        {
            CancelMemberRecord();
        }
        #endregion

        private void SaveMemberRecord()
        {
            MemberRec.PageThumb = LocalPageThumb;
            if (IsAdding)
                _presenter.InsertMemberRecord(MemberRec, _fullResPage);
            else
                _presenter.UpdateMemberRecord(MemberRec, _fullResPage);

            IsAdding = false;
            IsEditing = false;
            RaiseEvent(new RoutedEventArgs(OnMemberUpdatedEvent));
        }

        private void CancelMemberRecord()
        {
            IsAdding = false;
            IsEditing = false;
            MemberRec = _presenter.GetMember(_selectedMemberId);
        }
        private void DatePickerOnPreviewTextInput(object sender, TextCompositionEventArgs eventArgs)
        {
            eventArgs.Handled = eventArgs.Text.Any(
                character => !char.IsDigit(character) && character != '/' && character != '-');
        }


        private bool VerifyControls()
        {
            // same name not on file (Warn)
            // Blank dates are converted to null.
            if (MemberRec.FirstName == "" || MemberRec.LastName == "") return false;

            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public static readonly RoutedEvent OnMemberUpdatedEvent =
            EventManager.RegisterRoutedEvent("OnMemberUpdatedEvent",
                RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MemberInfo));
        public event RoutedEventHandler OnMemberUpdated
        {
            add => AddHandler(OnMemberUpdatedEvent, value);
            remove => RemoveHandler(OnMemberUpdatedEvent, value);
        }
        
        public static readonly RoutedEvent OnMemberDeletedEvent =
            EventManager.RegisterRoutedEvent("OnMemberDeletedEvent",
                RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MemberInfo));
        public event RoutedEventHandler OnMemberDeleted
        {
            add => AddHandler(OnMemberDeletedEvent, value);
            remove => RemoveHandler(OnMemberDeletedEvent, value);
        }


        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = MemberRec.LastName;
            if (openFileDialog.ShowDialog() == true)
            {
                var img = Image.FromFile(openFileDialog.FileName);

                _fullResPage = ImageToByteArray(img);
                LocalPageThumb = ImageToByteArray(ScaleImage(img, 300));
            }

        }


        private void ClearImageButton_Click(object sender, RoutedEventArgs e)
        {
            _fullResPage = null;
            LocalPageThumb = null;
        }

        private void ExpandImageButton_Click(object sender, RoutedEventArgs e)
        {
            var popup = new ImagePopupWindow {ImageSource = _presenter.GetImageData(MemberRec.PageId)};
            popup.ShowDialog();
        }


        public void MemberSave(Member memrec, byte[]imageData)
        {
            _presenter.UpdateMemberRecord(memrec, imageData);
        }


        private void StatusCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MemberRec.MemberTypeId == 1)
                ChkboxIsActive.IsChecked = true;
            else if (MemberRec.MemberTypeId > 2)
                ChkboxIsActive.IsChecked = false;
        }
    }
}
