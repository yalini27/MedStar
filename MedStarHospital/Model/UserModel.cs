using MedStarHospital.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedStarHospital.Model
{
    public class UserModel : ViewModelBase
    {
        private int _userid;

        public int UserID
        {
            get { return _userid; }
            set { _userid = value; OnPropertyChanged(); }
        }


        private string _username;

        public string UserName
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        private string _role;

        public string Role
        {
            get { return _role; }
            set { _role = value; OnPropertyChanged(); }
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }

        private string _driverid;

        public string DriverID
        {
            get { return _driverid; }
            set { _driverid = value; OnPropertyChanged(); }
        }
    }
}
