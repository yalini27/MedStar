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
    /// <summary>
    /// Interaction logic for AddDepartmentView.xaml
    /// </summary>
    public partial class AddDepartmentView : Window
    {
        public AddDepartmentView()
        {
            InitializeComponent();
            this.DataContext = new VMAddDepartment();
            VMAddDepartment.exit = new Action(this.Close);
        }
    }
}
