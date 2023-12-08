using MedStarHospital.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MedStarHospital.ViewModel
{
    public class VMTestingBillDesign :ViewModelBase
    {
        public VMTestingBillDesign(PatientModel patient)
        {
            Bill = new();
            Bill.Patient = new PatientModel();

            PatientBill.PatientName = patient.PatientName;

            PatientBill.Bookingdate = patient.Bookingdate;

        }
        public static Action exit;

        public ICommand cmdExit { get { return new RelayCommand(fnExit); } }

        void fnExit(object param)
        {
            exit.Invoke();
        }

        private double amount;

        public double Amount
        {
            get { return amount; }
            set { amount = value; OnPropertyChanged(); }
        }



        private PatientModel _patientbill;

        public PatientModel PatientBill
        {
            get { return _patientbill; }
            set { _patientbill = value; OnPropertyChanged(); }
        }

        private ObservableCollection<PatientModel> _patientbilllist;
        public ObservableCollection<PatientModel> PatientBillList
        {
            get { return _patientbilllist; }
            set
            {
                _patientbilllist = value;
                OnPropertyChanged();
            }
        }

        private TestingBillModel _bill = new TestingBillModel();

        public TestingBillModel Bill
        {
            get { return _bill; }
            set { _bill = value; OnPropertyChanged(); }
        }
    }
}
