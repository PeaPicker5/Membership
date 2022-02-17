using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Membership.Core.DataModels;
using Membership.Core.Dues.DataModels;
using Membership.Core.Dues.Presenters;
using Button = System.Windows.Controls.Button;

namespace Membership.UI_Controls.Dues
{

    public partial class DuesEmailList : IDuesRecordView
    {
        public string EmailText => @"Fellow Members,  

        As a reminder, it is that time of year again to pay your annual Association dues.  The dues amounts are $25 for Social Members and $10 for Active Members.
        
        Methods of payment available:
            - Venmo.  You may pay via Venmo(@PointPleasant-FiremensASSN)
            - USPS - 55 Ewer Ave, Roch NY, 14622.  MUST be postmarked before April 1st.
            - Drop off at Station#1 - Put in an envelope with 'Attn: Kevin Doran' and place it in the OUTSIDE mailbox on the front of the building.
        
        PLEASE DO NOT leave anything on the bulletin board.  Not responsible if I don't get to it first.

        Thanks for your cooperation,
        Kevin";



        public string EmailList
        {
            get => (string)GetValue(EmailListProperty);
            set => SetValue(EmailListProperty, value);
        }
        public static readonly DependencyProperty EmailListProperty =
            DependencyProperty.Register("EmailList", typeof(string), typeof(DuesEmailList));

        public ICollection<DuesRecord> DuesRecs { get; set; }


        private readonly DuesPresenter _presenter;

        public DuesEmailList()
        {
            InitializeComponent();
            _presenter = new DuesPresenter(this);
            LoadCurrentDuesRecords();
        }

        private void LoadCurrentDuesRecords()
        {
            DuesRecs = _presenter.GetByYear(DateTime.Now.Year)
                .OrderBy(name => name.MemberRec.LastName)
                .ToList();
            EmailList = DuesRecs.Where(x => !string.IsNullOrEmpty(x.MemberRec.EmailAddress))
                                .Aggregate("", (current, rec) => current + rec.MemberRec.EmailAddress + ", ");
        }
        private void CopyTextOnClick(object sender, RoutedEventArgs e)
        {
            var buttonTag = ((Button)sender).Tag;
            
            switch(buttonTag)
            {
                case "1":
                {
                    Clipboard.SetText(SubjectText.Text);
                    break;
                }
                case "2":
                {
                    Clipboard.SetText(BCCText.Text);
                    break;
                }
                case "3":
                {
                    Clipboard.SetText(EmailBodyText.Text);
                    break;
                }
            }
        }
    }
}
