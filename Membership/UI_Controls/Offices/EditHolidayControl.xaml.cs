using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Mavin.Core.Business.Structure.Data;
using Mavin.Core.Business.Structure.Presentation;
using Mavin.Core.BusinessPCL.User.Constants;
using Mavin.UI.Windows.Common;

namespace Structure.Controls.Holidays
{
    /// <summary>
    /// Interaction logic for EditHolidayControl.xaml
    /// </summary>
    public partial class EditHolidayControl
    {
        private readonly EditHolidayPresenter _presenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditHolidayControl"/> class.
        /// </summary>
        /// <exception cref="System.NullReferenceException"></exception>
        public EditHolidayControl()
        {
            InitializeComponent();

            // Rest needed when not in design mode
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            var application = Application.Current as IMavinApp;
            if (application == null)
            {
                throw new NullReferenceException();
            }

            _presenter = new EditHolidayPresenter(this, application.RepositoryFactory, application.OperatorId);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is modified.
        /// </summary>
        public bool IsModified
        {
            get
            {
                if (ItemData == null)
                {
                    return false;
                }

                return IsAdding || ItemData != null && (
                           DescriptionTextBox.Text != ItemData.Description ||
                           DatePicker.SelectedDate != ItemData.Date ||
                           NumberOfDaysUpDown.Value != ItemData.ExtendedDays + 1);
            }
        }

        /// <summary>
        /// Gets the title text.
        /// </summary>
        public override string TitleText
        {
            get
            {
                if (IsAdding)
                {
                    return "Adding Holiday";
                }

                return ItemData != null
                    ? (IsEditing ? $"Editing {ItemData.Description}" : ItemData.Description)
                    : string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the name of the holiday.  Required for UI validation.
        /// </summary>
        public string HolidayName { get; set; }

        public void ShowDetails()
        {
            if (ItemData != null)
            {
                Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Saves the holiday.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveHoliday()
        {
            if (!ValidateBoundedControls())
            {
                return false;
            }

            Holiday holiday;
            try
            {
                CreateHolidayObject(out holiday, IsAdding ? await _presenter.GetNextHolidayId() : ItemData.HolidayId);
            }
            catch (NullReferenceException excpetion)
            {
                ShowErrorMessage(excpetion.Message);
                return false;
            }

            return await UpdateHoliday(holiday);
        }

        #region Event Handlers

        private async void DetailsHeaderControlOnAddItem(object sender, RoutedEventArgs e)
        {
            await AddHoliday();
        }

        private void DetailsHeaderControlOnCancelItem(object sender, RoutedEventArgs e)
        {
            if (!IsModified)
            {
                ResetControl();
            }

            if (IsModified &&
                ShowUserYesNoQuestion(
                    "The Holiday has changes that haven't been saved. Are you sure you want to cancel?"))
            {
                ResetControl();
            }
        }

        private void DetailsHeaderControlOnEditItem(object sender, RoutedEventArgs e)
        {
            if (!AuthorizeOperator(OperatorPrivilege.HolidayEdit))
            {
                return;
            }

            IsEditing = true;
        }

        private void DetailsHeaderControlOnHideDetails(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        private async void DetailsHeaderControlOnRemoveItem(object sender, RoutedEventArgs e)
        {
            if (!AuthorizeOperator(OperatorPrivilege.HolidayDelete))
            {
                return;
            }

            if (await _presenter.RemoveHoliday(ItemData))
            {
                InvokeItemUpdatedEvent();
            }
        }

        private async void DetailsHeaderControlOnSaveItem(object sender, RoutedEventArgs e)
        {
            if (!await SaveHoliday())
            {
                return;
            }

            ResetControl();
        }

        #endregion

        private async Task AddHoliday()
        {
            SetCurrentValue(ItemDataProperty, await _presenter.CreateNewHoliday());

            IsAdding = true;
            IsEditing = true;
        }

        protected override void LoadItem()
        {
            SetControlValues();

            base.LoadItem();
        }

        private void SetControlValues()
        {
            if (ItemData == null)
            {
                return;
            }

            NumberOfDaysUpDown.Value = (short) (ItemData.ExtendedDays + 1);

            // For validation
            DescriptionTextBox.Text = ItemData.Description;
        }

        private void DatePickerOnPreviewTextInput(object sender, TextCompositionEventArgs eventArgs)
        {
            eventArgs.Handled = eventArgs.Text.Any(
                character => !char.IsDigit(character) && character != '/' && character != '-');
        }


        private async Task<bool> UpdateHoliday(Holiday holiday)
        {
            if (IsAdding)
            {
                try
                {
                    return await _presenter.AddHoliday(holiday);
                }
                catch (ArgumentException exception)
                {
                    ShowErrorMessage(exception.Message);
                    return false;
                }
            }

            try
            {
                await _presenter.UpdateHoliday(holiday);
                return true;
            }
            catch (ArgumentException exception)
            {
                ShowErrorMessage(exception.Message);
                return false;
            }
        }

        private void CreateHolidayObject(out Holiday holiday, short holidayId)
        {
            DateTime date;
            if (DatePicker.SelectedDate != null)
            {
                date = DatePicker.SelectedDate.Value;
            }
            else
            {
                throw new NullReferenceException("The date is not set.");
            }

            holiday = new Holiday(holidayId,
                DescriptionTextBox.Text, date,
                NumberOfDaysUpDown.Value ?? 1);
        }
    }
}