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
using MedStarHospital.ViewModel;

namespace MedStarHospital.View
{
    public partial class BillMethod : Window
    {
        public BillMethod()
        {
            InitializeComponent();
            //this.DataContext = new VMBillDesign();
            //this.DataContext = new VMBillMethod(Patie);
            VMBillMethod.exit = new Action(this.Close);
        }
    }
}
