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

namespace F1Telemetry.Forces
{
    /// <summary>
    /// Interaction logic for ForcesView.xaml
    /// </summary>
    public partial class ForcesView : Window
    {
        public ForcesViewModel Forces { get { return DataContext as ForcesViewModel; } }

        public ForcesView()
        {
            InitializeComponent();
            DataContext = new ForcesViewModel();
        }
    }
}
