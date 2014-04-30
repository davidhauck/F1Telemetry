using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace F1Telemetry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        private MainWindowViewModel ViewModel
        {
            get
            {
                return DataContext as MainWindowViewModel;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        bool TestConnectionToServer()
        {
            OnlineWebReference.WebServiceSoapClient service = new OnlineWebReference.WebServiceSoapClient();
            //LocalWebReference.WebServiceSoapClient service = new LocalWebReference.WebServiceSoapClient();
            return service.TestLogin(UsernameTb.Text, PasswordBox.Password);
        }

        private void StartUploading_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TestConnectionToServer())
                {
                    ViewModel.UploadLap = UploadLapToServer;
                    ViewModel.Output += "\r\nConnected to server. Laps will now be uploaded.";
                    OutputScroller.ScrollToBottom();
                }
                else
                {
                    ViewModel.Output += "\r\nInvalid Username or Password.";
                    OutputScroller.ScrollToBottom();
                }
            }
            catch (Exception ex)
            {
                ViewModel.Output += "\r\nCould not connect to a network. Please check your internet connection.";
                OutputScroller.ScrollToBottom();
            }
        }

        private void StopUploading_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.UploadLap = null;
            ViewModel.Output += "\r\nDisconnected from server. Laps will no longer be uploaded.";
            OutputScroller.ScrollToBottom();
        }

        private void UploadLapToServer(float totalLapTime, float sector1, float sector2, float sector3, int trackNum)
        {
            if (sector1 == 0 || sector2 == 0 || sector3 == 0 || totalLapTime == 0)
            {
                ViewModel.Output += "\r\nLap not uploaded: do not have data for the whole lap.";
                OutputScroller.ScrollToBottom();
            }
            else
            {
                try
                {
                    OutputScroller.ScrollToBottom();
                    OnlineWebReference.WebServiceSoapClient service = new OnlineWebReference.WebServiceSoapClient();
                    //LocalWebReference.WebServiceSoapClient service = new LocalWebReference.WebServiceSoapClient();
                    service.UploadLap(UsernameTb.Text, PasswordBox.Password, trackNum, totalLapTime.ToString(), sector1, sector2, sector3);
                    ViewModel.Output += String.Format("\r\nLap uploaded: Total:{0}, Sector 1: {1}, Sector 2: {2}, Sector 3: {3}.", totalLapTime, sector1, sector2, sector3);
                }
                catch (Exception e)
                {
                    ViewModel.Output += "\r\nLap not uploaded: lost connection to server. Please check your internet connection.";
                    OutputScroller.ScrollToBottom();
                    ViewModel.UploadLap = null;
                }
            }
        }
    }
}
