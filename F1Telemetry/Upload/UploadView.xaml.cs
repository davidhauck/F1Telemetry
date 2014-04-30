using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace F1Telemetry.Upload
{
    /// <summary>
    /// Interaction logic for UploadView.xaml
    /// </summary>
    public partial class UploadView : Window
    {
        public Upload UploadModel
        {
            get     
            {
                return DataContext as Upload;
            }
        }

        public UploadView()
        {
            InitializeComponent();
            DataContext = new Upload();
        }

        public enum TrackName
        {
            Australia,
            Malaysia,
            China,
            Bahrain,
            Spain,
            Monaco,
            Canada,
            Britain,
            Germany,
            Hungary,
            Belgium,
            Italy,
            Singapore,
            Korea,
            Japan,
            India,
            Abu_Dhabi,
            United_States,
            Brazil
        };

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UploadModel.Message = "Uploading";
            OnlineWebReference.WebServiceSoapClient service = new OnlineWebReference.WebServiceSoapClient();
            //LocalWebReference.WebServiceSoapClient service = new LocalWebReference.WebServiceSoapClient();
            try
            {
                UploadModel.Message = service.UploadRace(UploadModel.Username, PassBox.Password, (int)Enum.Parse(typeof(TrackName), UploadModel.Track), UploadModel.BestLap.ToString(), UploadModel.Sector1Float, UploadModel.Sector2Float, UploadModel.Sector3Float);
            }
            catch (WebException ex)
            {
                UploadModel.Message = "Could not connect to server. Check your connection.";
            }
        }
    }
}
