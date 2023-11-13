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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedStarHospital.View
{
    /// <summary>
    /// Interaction logic for vw_BillView.xaml
    /// </summary>
    public partial class vw_BillView : UserControl
    {
        public vw_BillView()
        {
            InitializeComponent(); this.DataContext = new VMBill();
        }
    }
}
