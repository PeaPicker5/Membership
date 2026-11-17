using System.Windows;
using System.Windows.Threading;

namespace Membership
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        /// <summary>
        /// Last-chance handler: report the error instead of letting the app close
        /// (e.g. the database is unreachable or a record is no longer on file).
        /// </summary>
        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var ex = e.Exception.GetBaseException();
            MessageBox.Show(
                $"Something went wrong and the last action may not have been completed.\n\n{ex.Message}",
                "Membership - Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
