using MedStarHospital.Model;
using MedStarHospital.View;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MedStarHospital.ViewModel
{
    public class VMHome : ViewModelBase
    {
        public VMHome()
        {

        }
        public VMHome( UserModel user)
        {
            User = user;
            TotalDoctor = Sql_Connection.DataCount("tblDoctor");
            TotalDepartment = Sql_Connection.DataCount("tblDepartment");
            TotalTestingTypes = Sql_Connection.DataCount("tblTestingType");
        }

        private UserControl _Control = new UserControl();

        public UserControl Control
        {
            get { return _Control; }
            set { _Control = value; OnPropertyChanged(); }
        }


        private UserModel userr;

        public UserModel User
        {
            get { return userr; }
            set { userr = value; OnPropertyChanged(); }
        }
        public ICommand cmdUsers { get { return new RelayCommand(fnUser, fnCanExecuteUser); } }
        public ICommand cmdDepartment { get { return new RelayCommand(fnDepartment); } }

        public ICommand cmdCommonAll { get { return new RelayCommand(fncommon, fncanExecuteCommon); } }

        private void fncommon(object obj)
        {
           switch(obj.ToString())
            {
                case "Users":
                    VMUsers vm = new VMUsers();
                    Control.Content = new UserView();
                    Control.DataContext = vm;
                    break;
                case "Department":
                    VMDepartment vd = new VMDepartment(User);
                    Control.Content = new DepartmentView();
                    Control.DataContext = vd;
                    break; 
                case "Doctor":
                    VMDoctor vdoc = new VMDoctor(User);
                    Control.Content = new DoctorView();
                    Control.DataContext = vdoc;
                    break;
                case "Booking":
                    VMPatient vp = new VMPatient(User);
                    Control.Content = new PatientView();
                    Control.DataContext = vp;
                    break;
                case "Payment":
                    VMBill vb = new VMBill();
                    Control.Content = new vw_BillView();
                    Control.DataContext = vb;
                    break;
                case "Testing":
                    VMTestingType st = new VMTestingType(User);
                    Control.Content = new TestingTypeView();
                    Control.DataContext = st;
                    break;

                case "Ambulance":
                    VMAmbulance am = new VMAmbulance(User);
                    Control.Content = new AmbulanceView();
                    Control.DataContext = am;
                    break;

                case "Pharmacy":
                    VMPharmacy pa = new VMPharmacy(User);
                    Control.Content = new PharmacyView();
                    Control.DataContext = pa;
                    break;

                case "Driver":
                    VMDriver re = new VMDriver(User);
                    Control.Content = new DriverView();
                    Control.DataContext = re;
                    break;

                case "Back":
                    if (MessageBox.Show("Do you want to Log out?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        LoginPageView mv = new LoginPageView();
                        mv.DataContext = new VMLogin();
                        exit.Invoke();
                        mv.ShowDialog();
                    }
                    break;

            }

        }


        public ICommand cmdExit { get { return new RelayCommand(fnBack); } }
        void fnDepartment(object param)
        {
            VMDepartment vd = new VMDepartment(User);
            Control.Content = new DepartmentView();
            Control.DataContext = vd;
        }

        void fnUser(object param)
        {
            VMUsers vm = new VMUsers();
            Control.Content = new UserView();
            Control.DataContext = vm;
        }

        bool fncanExecuteCommon(object o)
        {
            if(User.Role.ToLower() == "admin")
                return  true;
            if (User.Role.ToLower() == "doctor" && (o.ToString() == "Users"  || o.ToString() == "Booking" || o.ToString() == "Payment" || o.ToString() == "Ambulance" || o.ToString() == "Pharmacy" || o.ToString() == "Driver"))
                return false;  
            if (User.Role.ToLower() == "receptionist" && (o.ToString() == "Users" || o.ToString() == "Pharmacy" || o.ToString() == "Driver"))
                return false;
            if(User.Role.ToLower() == "pharmacist" &&  (o.ToString() == "Users" || o.ToString() == "Department" || o.ToString() == "Doctor" || o.ToString() == "Booking" || o.ToString() == "Payment" || o.ToString() == "Testing" || o.ToString() == "Ambulance" || o.ToString() == "Driver"))
                return false;
            if(User.Role.ToLower() =="driver" && (o.ToString() == "Users" || o.ToString() == "Department" || o.ToString() == "Doctor" || o.ToString() == "Booking" || o.ToString() == "Payment" || o.ToString() == "Testing" || o.ToString() == "Ambulance" || o.ToString() == "Pharmacy"))
                return false;
            return true;

        }

        bool fnCanExecuteUser(object o)
        {
            return User.Role.ToLower() == "admin" ? true : false;
        }

        bool fnCanExecutereceptionist(object o)
        {
            return User.Role.ToLower() == "receptionist" ? true : false;
        }

        bool fnCanExecuteDoctor(object o)
        {
            return User.Role.ToLower() == "doctor" ? true : false;
        }



      
        public static Action exit;
        void fnBack(object param)
        {
            if (MessageBox.Show("Do you want to close this page?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                LoginPageView mv = new LoginPageView();
                mv.DataContext = new VMLogin();
                exit.Invoke();
                mv.ShowDialog();
            }
        }

       


        private DepartmentModel _department;

        public DepartmentModel Department
        {
            get { return _department; }
            set { _department = value; OnPropertyChanged(); }
        }

        private List<DepartmentModel> _departments;
        public List<DepartmentModel> Departments
        {
            get { return _departments; }
            set { _departments = value; OnPropertyChanged(); }
        }

        

        private int _totalDoctor;

        public int TotalDoctor
        {
            get { return _totalDoctor; }
            set { _totalDoctor = value; OnPropertyChanged(); }
        }

        private int _totaldepartment;

        public int TotalDepartment
        {
            get { return _totaldepartment; }
            set { _totaldepartment = value; OnPropertyChanged(); }
        }

        private int _totaltestingtypes;

        public int TotalTestingTypes
        {
            get { return _totaltestingtypes; }
            set { _totaltestingtypes = value; OnPropertyChanged(); }
        }

    }
}
