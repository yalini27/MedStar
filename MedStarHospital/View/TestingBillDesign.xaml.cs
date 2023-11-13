using MedStarHospital.ViewModel;
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

namespace MedStarHospital.View
{
    /// <summary>
    /// Interaction logic for TestingBillDesign.xaml
    /// </summary>
    public partial class TestingBillDesign : Window
    {
        public TestingBillDesign()
        {
            InitializeComponent();
            //this.DataContext = new VMTestingBillDesign();
            VMBillMethod.exit = new Action(this.Close);
        }
    }
}
