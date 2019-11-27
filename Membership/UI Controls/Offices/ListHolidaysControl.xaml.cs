using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Mavin.Core.Business.Structure.Presentation;
using Mavin.UI.Windows.Common;
using Xceed.Wpf.DataGrid;

namespace Structure.Controls.Holidays
{
    /// <summary>
    /// Interaction logic for ListHolidaysControl.xaml
    /// </summary>
    public partial class ListHolidaysControl : IListHolidaysView
    {
        private readonly ListHolidaysPresenter _presenter;

        public ListHolidaysControl()
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

            _presenter = new ListHolidaysPresenter(this, application.RepositoryFactory);
        }

        #region Overrides of ListBaseControl<Holiday>

        protected override async Task LoadItems()
        {
            await _presenter.Load();
        }

        protected override DataGridControl DataGrid => HolidayDataGrid;

        protected override string GroupByPropertyName => "Year";

        protected override IEnumerable<string> SortPropertyNames => new[] { "DateRange" };

        #endregion
    }
}