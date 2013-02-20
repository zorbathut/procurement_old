using System.Windows;

namespace Procurement
{
    public partial class App : Application
    {
        public App()
        {
            Application.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
        }

        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            System.IO.File.AppendAllText("DebugInfo.log", e.Exception.ToString());
            MessageBox.Show("There was an unhandled error - Sorry! Please send the debuginfo.log to one of us devs :)");
        }
    }
}