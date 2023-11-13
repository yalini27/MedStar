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
using MedStarHospital.Model;
using MedStarHospital.ViewModel;

namespace MedStarHospital.View
{
    /// <summary>
    /// Interaction logic for AddPatientView.xaml
    /// </summary>
    public partial class AddPatientView : Window
    {
        public AddPatientView(PatientModel patient = null, bool withDoc = true)
        {
            InitializeComponent();
            this.DataContext = new VMAddPatient(patient, withDoc);
            VMAddPatient.Exit = new Action(this.Close);
        }

    }
}
