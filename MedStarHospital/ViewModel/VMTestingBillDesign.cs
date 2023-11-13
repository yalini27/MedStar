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
            //PatientBill = patient;
            //PatientBill.PatientId = patient.PatientId;
            PatientBill.PatientName = patient.PatientName;
            //PatientBill.Phonenumber = patient.Phonenumber;
            //PatientBill.Department.DepartmentID = patient.Department.DepartmentID;
            //PatientBill.Doctor.Doctorid = patient.Doctor.Doctorid;
            PatientBill.Bookingdate = patient.Bookingdate;
            //Bill.Patient.HospitalFee = 500;
            //Bill.Patient.DoctorFee = 1500;
        }
        public static Action exit;

        public ICommand cmdExit { get { return new RelayCommand(fnExit); } }

        void fnExit(object param)
        {
            exit.Invoke();
        }

        //void billview((ObservableCollection<PatientModel> PatientBillList, PatientModel PatientBill, DateTime Bookingdate, string TestingType))
        //{
        //    this.PatientBillList = PatientBillList;
        //    PatientBill = new PatientModel
        //    {
        //        PatientId = PatientBill.PatientId,
        //        PatientName = PatientBill.PatientName,
        //        Bookingdate = PatientBill.Bookingdate,
        //        TestingType = new TestingTypeModel
        //        {
        //            TestingTypeName = PatientBill.TestingType.TestingTypeName,
        //            Amount = PatientBill.TestingType.Amount
        //        }

        //    }
        //    ;
        //}


        private double amount;

        public double Amount
        {
            get { return amount; }
            set { amount = value; OnPropertyChanged(); }
        }


        //public VMTestingBillDesign(PatientModel patient)
        //{
        //    Bill = new();
        //    Bill.Patient = new PatientModel();
        //    //PatientBill = patient;
        //    //PatientBill.PatientId = patient.PatientId;
        //    PatientBill.PatientName = patient.PatientName;
        //    //PatientBill.Phonenumber = patient.Phonenumber;
        //    //PatientBill.Department.DepartmentID = patient.Department.DepartmentID;
        //    //PatientBill.Doctor.Doctorid = patient.Doctor.Doctorid;
        //    PatientBill.Bookingdate = patient.Bookingdate;
        //    //Bill.Patient.HospitalFee = 500;
        //    //Bill.Patient.DoctorFee = 1500;
        //}


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
