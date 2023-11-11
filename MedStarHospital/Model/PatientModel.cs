using MedStarHospital.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MedStarHospital.Model
{
    public class PatientModel : ViewModelBase
    {
        private int _patientId;
        private string _patientname;
        private string _phonenumber;
        private DepartmentModel _department;
        private DoctorModel _doctor;
        private DateTime _bookingdate = DateTime.Now;
        private TestingTypeModel _testingType;

        private ObservableCollection<WithoutDoctorModel> _colwitoutdoctor;

        public ObservableCollection<WithoutDoctorModel> ColWithOutDoctor
        {
            get { return _colwitoutdoctor; }
            set { _colwitoutdoctor = value; OnPropertyChanged(); }
        }


        public int PatientId
        {
            get { return _patientId; }
            set { _patientId = value; OnPropertyChanged(); }
        }

        public string PatientName
        {
            get { return _patientname; }
            set { _patientname = value; OnPropertyChanged();
                if (string.IsNullOrEmpty(value))
                    PatientNameError = "Name Cannot be empty";
                else if (value.Any(v => Char.IsDigit(v)))
                    PatientNameError = "Name must be Text only";
                else if (PatientName.All(sp => !char.IsLetter(sp)))
                    PatientNameError = "Name must be Text only";
                else
                    PatientNameError = "";
            }
        }

        public TestingTypeModel TestingType
        {
            get { return _testingType; }
            set { _testingType = value; OnPropertyChanged(); }
        }

        private DateTime _resultdate = DateTime.Now;

        public DateTime ResultDate
        {
            get { return _resultdate; }
            set
            {
                _resultdate = value; OnPropertyChanged();
                DateTime Dates1 = Bookingdate;
                DateTime RDate = Dates1.AddDays(7);
                DateTime Result_date;
                if (!(ResultDate.Date >= Bookingdate.Date && ResultDate <= RDate))
                {
                    ResultDateerror = "Result date must be with in seven days from Booking date";
                }
            }
        }
        public string Phonenumber
        {
            get { return _phonenumber; }
            set
            {
                _phonenumber = value;
                OnPropertyChanged();

                if (!(Phonenumber.Count(char.IsDigit) == 9 ))
                    PatientPhoneError = "You don't type first 0 value, It have 9 digits";
                else
                    PatientPhoneError = "";
                
            }
        }

        private float _doctorfee ;

        public float DoctorFee
        {
            get { return _doctorfee ; }
            set { _doctorfee  = value; OnPropertyChanged(); }
        }

        private float _HospitalFees;

        public float HospitalFee
        {
            get { return _HospitalFees; }
            set { _HospitalFees = value; OnPropertyChanged(); }
        }

        public DepartmentModel Department
        {
            get { return _department; }
            set
            {
                _department = value;
                OnPropertyChanged();
            }
        }


        public DoctorModel Doctor
        {
            get { return _doctor; }
            set
            {
                _doctor = value;
                OnPropertyChanged();

                
            }
        }

        private decimal _amount;

        public decimal Amount
        {
            get { return _amount; }
            set { _amount = value;OnPropertyChanged(); }
        }

        public DateTime Bookingdate
        {
            get { return _bookingdate; }
            set
            {
                _bookingdate = value;
                OnPropertyChanged();

                DateTime Dates = DateTime.Now;
                DateTime BDate = Dates.AddDays(7);
                DateTime booking_date;
                if (!(Bookingdate >= Dates && Bookingdate <= BDate))
                    BookingError = "Booking date must be within 7 days...!";
                else
                    BookingError = "";
                
            }
        }

        private string _patientnameerror;

        public string PatientNameError
        {
            get { return _patientnameerror; }
            set { _patientnameerror = value;OnPropertyChanged(); }
        }

        private string _resultDateerror;

        public string ResultDateerror
        {
            get { return _resultDateerror; }
            set { _resultDateerror = value; OnPropertyChanged(); }
        }

        private string _patientPhoneError;

        public string PatientPhoneError
        {
            get { return _patientPhoneError; }
            set { _patientPhoneError = value; OnPropertyChanged(); }
        }

        private string _bookingerror;

        public string BookingError
        {
            get { return _bookingerror; }
            set { _bookingerror = value; OnPropertyChanged(); }
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }

        //public string DoctorName
        //{
        //    get { return _doctorname; }
        //    set
        //    {
        //        _doctorname = value;
        //        OnPropertyChanged();
        //    }
        //}

        //public string Bookingtime
        //{
        //    get { return _bookingtime; }
        //    set { _bookingtime = value; OnPropertyChanged(); }
        //}

        private int _withid;

        public int WithID
        {
            get { return _withid; }
            set { _withid = value; OnPropertyChanged(); }
        }

        private int _withoutid;

        public int WithoutID
        {
            get { return _withoutid; }
            set { _withoutid = value; OnPropertyChanged(); }
        }

        private bool _testrequired;

        public bool TestRequired
        {
            get { return _testrequired; }
            set { _testrequired = value;OnPropertyChanged(); }
        }

        private bool _Withdoc;

        public bool WithDoc
        {
            get { return _Withdoc; }
            set
            {
                _Withdoc = value;
                OnPropertyChanged();


             
            }
        }

        private bool _Withoutdoc;

        public bool WithoutDoc
        {
            get { return _Withoutdoc; }
            set
            {
                _Withoutdoc = value;
                OnPropertyChanged();
               
            }
        }



        private ObservableCollection<PatientModel> patientModels;
        public ObservableCollection<PatientModel> Patients
        {
            get { return patientModels; }
            set
            {
                patientModels = value;
                OnPropertyChanged();
            }
        }
    }
}