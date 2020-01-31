using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Annotations;

namespace Membership.UI_Controls.Members
{
    public partial class ImagePopupWindow : INotifyPropertyChanged
    {

        public byte[] ImageSource
        {
            get { return (byte[])GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(byte[]),
                typeof(ImagePopupWindow));


        public ImagePopupWindow()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
