using MedStarHospital.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedStarHospital.Model
{
    public class DriverModel  :ViewModelBase
    {

		private string _driverid;

		public string DriverID
        {
			get { return _driverid; }
			set { _driverid = value; OnPropertyChanged(); }
		}

        private string _drivername;

        public string DriverName
        {
            get { return _drivername; }
            set { _drivername = value; OnPropertyChanged(); }
        }

        private long _phonenumber;

        public long PhoneNumber
        {
            get { return _phonenumber; }
            set { _phonenumber = value;OnPropertyChanged(); }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        private string _currentlocation;

        public string CurrentLocation
        {
            get { return _currentlocation; }
            set { _currentlocation = value; OnPropertyChanged(); }
        }

        private string _servicelocation;

        public string ServiceLocation
        {
            get { return _servicelocation; }
            set { _servicelocation = value;OnPropertyChanged(); }
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }

    }
}
