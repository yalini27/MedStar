using MedStarHospital.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MedStarHospital.ViewModel
{
    public class VMBillMethod : ViewModelBase
    {

        private string _withdoctorvisiblity;

        public string WithDoctorVisibility
        {
            get { return _withdoctorvisiblity; }
            set { _withdoctorvisiblity = value; OnPropertyChanged(); }
        }

        private string _withoutdoctorvisiblity;

        public string WithoutDoctorVisibility
        {
            get { return _withoutdoctorvisiblity; }
            set { _withoutdoctorvisiblity = value; OnPropertyChanged(); }
        }

        

        public VMBillMethod(PatientModel patient, bool withoutdoc)
        {
            Patient = patient;
            Patient.WithoutDoc = withoutdoc;
            fnloadTestingType(Patient);
            SelectedPatient = new PatientModel();
            Patient.TestingType = new TestingTypeModel();
            Patient.PatientId = patient.PatientId;
            Patient.Bookingdate = patient.Bookingdate;
            Patient.PatientName = patient.PatientName;

            if (Patient.WithoutDoc)
            {
                foreach (var item in TestingTypeList)
                {
                    patient.TestingType.TestingTypeName = item.TestingTypeName;
                    patient.TestingType.Amount = item.Amount;
                    Patient.Amount += item.Amount;
                }
            }

        }

        void fnloadTestingType(PatientModel patient)
        {
            if (patient.Department.DepartmentID <= 0)
            {
                Sql_Connection.sql_connection();
                TestingTypeList = new ObservableCollection<TestingTypeModel>();
                string Query = $"select tt.* from tblMWithout_doctor wd join tblTestingType TT ON wd.TestingTypeID = tt.TestingTypeID where wd.BookingID = {patient.PatientId}";
                SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TestingTypeList.Add(new TestingTypeModel
                    {
                        TestingTypeID = (int)reader.GetValue(0),
                        TestingTypeName = (string)reader.GetValue(1),
                        Amount = (decimal)reader.GetValue(2)
                    });
                }
                Sql_Connection.close_connection();

            }
        }
        public VMBillMethod(PatientModel patient)
        {
            Bill = new();
            Bill.Patient = new PatientModel();
           
            PatientBill.PatientName = patient.PatientName;
            
            PatientBill.Bookingdate = patient.Bookingdate;
            
        }

        public VMBillMethod(PatientModel patient, ObservableCollection<TestingTypeModel> TestingTypeModelsCollention = null)
        {
            Bill = new BillDesignModel
            {
                Patient = new PatientModel
                {
                    PatientId = patient.PatientId,
                    PatientName = patient.PatientName,
                    Bookingdate = patient.Bookingdate,

                },

            };



            if (Patient.WithoutDoc)
            {
                foreach (var item in TestingTypeList)
                {
                    Patient.Amount += item.Amount;
                }
            }

            if (TestingTypeModelsCollention != null)
            {
                WithDoctorVisibility = "Collapsed";
                WithoutDoctorVisibility = "Visible";

                TestingTypeList = TestingTypeModelsCollention;
            }
            else
            {
                WithDoctorVisibility = "Visible";
                WithoutDoctorVisibility = "Collapsed";
            }
        }


        private PatientModel _selectedpatient;

        public PatientModel SelectedPatient
        {
            get { return _selectedpatient; }
            set { _selectedpatient = value; OnPropertyChanged(); }
        }

        private PaymentModel _payment;

        public PaymentModel Payment
        {
            get { return _payment; }
            set { _payment = value; OnPropertyChanged(); }
        }

        private ObservableCollection<TestingTypeModel> _testingtypelist;
        public ObservableCollection<TestingTypeModel> TestingTypeList
        {
            get { return _testingtypelist; }
            set { _testingtypelist = value; OnPropertyChanged(); }
        }
        public static Action exit;

        public ICommand cmdExit { get { return new RelayCommand(fnExit); } }

        void fnExit(object param)
        {
            exit.Invoke();
        }
       
        private PatientModel _patientbill = new PatientModel();

        public PatientModel PatientBill
        {
            get { return _patientbill; }
            set { _patientbill = value; OnPropertyChanged(); }
        }

        private PatientModel _patient = new PatientModel();

        public PatientModel Patient
        {
            get { return _patient; }
            set { _patient = value; OnPropertyChanged(); }
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

        private BillDesignModel _bill;

        public BillDesignModel Bill
        {
            get { return _bill; }
            set { _bill = value; OnPropertyChanged(); }
        }
    }
   
}
