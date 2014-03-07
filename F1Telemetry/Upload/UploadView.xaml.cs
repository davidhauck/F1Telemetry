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
    }
}
