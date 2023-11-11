using MedStarHospital.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedStarHospital.Model
{
    public class TestingBookingModel :  ViewModelBase
    {
		private int _bookingid;

		public int BookingID
		{
			get { return _bookingid; }
			set { _bookingid = value; OnPropertyChanged(); }
		}


		private string _patientname;

		public string PatientName
		{
			get { return _patientname; }
			set { _patientname = value;OnPropertyChanged();
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

		private string _phoneno;

		public string PhoneNumber
		{
			get { return _phoneno; }
			set { _phoneno = value; OnPropertyChanged();
                if (!(PhoneNumber.Count(char.IsDigit) == 9))
                    PatientPhoneError = "You don't type first 0 value, It have 9 digits";
                else
                    PatientPhoneError = "";
            }
		}


        private string _patientnameerror;

        public string PatientNameError
        {
            get { return _patientnameerror; }
            set { _patientnameerror = value; OnPropertyChanged(); }
        }

        private string _patientPhoneError;

        public string PatientPhoneError
        {
            get { return _patientPhoneError; }
            set { _patientPhoneError = value; OnPropertyChanged(); }
        }

        private TestingTypeModel _testingtype;

		public TestingTypeModel TestingType
		{
			get { return _testingtype; }
			set { _testingtype = value; OnPropertyChanged(); }
		}

		private DateTime _resultdate =  DateTime.Now;

		public DateTime ResultDate
		{
			get { return _resultdate; }
			set { _resultdate = value; OnPropertyChanged();
                DateTime Dates1 = Bookingdate;
                DateTime RDate = Dates1.AddDays(7);
                DateTime Result_date;
                if (!(ResultDate.Date >= Bookingdate.Date && ResultDate <= RDate))
                {
                    ResultDateerror = "Result date must be with in seven days from Booking date";
                }
            }
		}

        private string _status;

        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }



        private DateTime _bookingdate = DateTime.Now;

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

        private string _bookingerror;

        public string BookingError
        {
            get { return _bookingerror; }
            set { _bookingerror = value; OnPropertyChanged(); }
        }

        private string _resultDateerror;

        public string ResultDateerror
        {
            get { return _resultDateerror; }
            set { _resultDateerror = value; OnPropertyChanged(); }
        }


    }
}
