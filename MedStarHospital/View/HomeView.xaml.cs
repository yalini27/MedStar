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
using LiveCharts.Wpf;
using LiveCharts;

namespace MedStarHospital.View
{
    public partial class HomeView : Window
    {
        public HomeView()
        {
            InitializeComponent();
            //this.DataContext = new VMHome();
            VMHome.exit = new Action(this.Close);
        }
    } 
}

    