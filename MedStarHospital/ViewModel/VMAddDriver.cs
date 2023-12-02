using MedStarHospital.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace MedStarHospital.ViewModel
{
    public class VMAddDriver  :ViewModelBase
    {
        public ICommand cmdback { get { return new RelayCommand(fnBack); } }

        public ICommand cmdOption { get { return new RelayCommand(fnOption); } }

        public static Action exit;

        private bool _isEnable;

        public bool IsEnable
        {
            get { return _isEnable; }
            set { _isEnable = value; OnPropertyChanged(); }
        }

        private bool _isEnableEdit;

        public bool IsEnableEdit
        {
            get { return _isEnableEdit; }
            set { _isEnableEdit = value; OnPropertyChanged(); }
        }

        public VMAddDriver(UserModel user,DriverModel driver = null)
        {
            User = user;
            if(User.Role == "Admin")
            {
                IsEnable = false;
                IsEnableEdit = true;
            }
            else
            {
                IsEnableEdit = false;
                  IsEnable = true;
            }
            ISDriver();
            ISUser();
            if (driver == null)
            {
                btn = "ADD";
                btnback = "CLOSE";
            }
            else
            {
                DriverModel = driver;
                DriverModel = new DriverModel
                {
                    DriverID = driver.DriverID,
                    DriverName = driver.DriverName,
                    PhoneNumber = driver.PhoneNumber,
                    Password = driver.Password,
                    CurrentLocation = driver.CurrentLocation,
                    ServiceLocation = driver.ServiceLocation,
                    Status = driver.Status
                };
                btn = "UPDATE";
                btnback = "CLOSE";
            }
            DriverList = new ObservableCollection<DriverModel>();
            //fngetRole();
        }

        private UserModel _user = new UserModel();

        public UserModel User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(); }
        }

        void ISDriver()
        {
            DriverModel = new();
            var number = Sql_Connection.SpaficDataISINTable("tblDriver", "DriverID", "DriverID");
            if (number == "")
            {
                DriverModel.DriverID = "Dri001";
            }
            else
            {
                string numericPart = number.Substring(3);

                if (int.TryParse(numericPart, out int numericValue))
                {
                    numericValue++;

                    string newdriverid = "Dri" + numericValue.ToString("D3");
                    DriverModel.DriverID = newdriverid;

                }
            }
        }


        void ISUser()
        {
            User = new();
            User.UserID = Sql_Connection.IsData("tblUser") ? int.Parse(Sql_Connection.SpaficDataISINTable("tblUser", "userID", "userID")) + 1 : 1;
        }

        void fnBack(object param)
        {
            if (MessageBox.Show("Do you want to close this page?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                exit.Invoke();
            }
        }
        void fnOption(object param)
        {

            switch (btn)
            {

                case "ADD":
                    User.Role = "Driver";
                    if (validation())
                    {
                        Sql_Connection.sql_connection();
                        string Query = $"insert into tblDriver values('" + DriverModel.DriverID + "','" + DriverModel.DriverName + "','" + DriverModel.PhoneNumber + "','" + DriverModel.Password + "','" + DriverModel.CurrentLocation + "','" + DriverModel.ServiceLocation + "','" + DriverModel.Status + "')";
                        SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.InsertCommand = new SqlCommand(Query, Sql_Connection.getconnection());
                        adapter.InsertCommand.ExecuteReader();
                        adapter.Dispose();
                        command.Dispose();
                        Sql_Connection.close_connection();

                        Sql_Connection.sql_connection();
                        string Query1 = $"insert into tblUser values('" + User.UserID + "','" + DriverModel.DriverName + "','" + DriverModel.Password + "','" + User.Role + "','" + DriverModel.Status + "','" + DriverModel.DriverID + "')";
                        SqlCommand command1 = new SqlCommand(Query1, Sql_Connection.getconnection());
                        SqlDataAdapter adapter1 = new SqlDataAdapter();
                        adapter1.InsertCommand = new SqlCommand(Query1, Sql_Connection.getconnection());
                        adapter1.InsertCommand.ExecuteReader();
                        adapter1.Dispose();
                        command1.Dispose();
                        Sql_Connection.close_connection();
                        MessageBox.Show("Successfully Added", "Add", MessageBoxButton.OK, MessageBoxImage.Information);
                        exit.Invoke();
                    }
                    else
                    {
                        MessageBox.Show("Invalid datas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    VMDriver.driverrefresh.Invoke();
                    break;


                case "UPDATE":
                    User.Role = "Driver";
                    if (validation1())
                    {
                        Sql_Connection.sql_connection();
                        string query = $"update tblDriver set DriverName = '" + DriverModel.DriverName + "',PhoneNumber='" + DriverModel.PhoneNumber + "',Password='" + DriverModel.Password + "',CurrentLocation='" + DriverModel.CurrentLocation + "',ServiceLocation='" + DriverModel.ServiceLocation + "',Status='" + DriverModel.Status + "'where DriverID='" + DriverModel.DriverID + "'";
                        SqlCommand command1 = new SqlCommand(query, Sql_Connection.getconnection());
                        SqlDataAdapter adapter1 = new SqlDataAdapter();
                        adapter1.UpdateCommand = new SqlCommand(query, Sql_Connection.getconnection());
                        adapter1.UpdateCommand.ExecuteReader();
                        adapter1.Dispose();
                        command1.Dispose();

                        //string query2 = $"update tblUser set UserName = '" + DriverModel.DriverName + "',Password_no='" + DriverModel.Password + "',Role_name='" + User.Role + "',status_state='" + User.Status + "'where userID='" + User.UserID + "'";
                        //SqlCommand command2 = new SqlCommand(query2, Sql_Connection.getconnection());
                        //SqlDataAdapter adapter2 = new SqlDataAdapter();
                        //adapter2.UpdateCommand = new SqlCommand(query2, Sql_Connection.getconnection());
                        //adapter2.UpdateCommand.ExecuteReader();
                        //adapter2.Dispose();
                        //command2.Dispose();

                        Sql_Connection.close_connection();
                        MessageBox.Show("Update successfully!", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                        exit.Invoke();
                    }
                    else
                    {
                        MessageBox.Show("Invalid datas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    VMDriver.driverrefresh.Invoke();
                    break;
            }

            //VMPharmacy.RefreshPharmacy.Invoke();
            //AppEvents.OnRefreshUser();
        }



        public bool validation()
        {
            bool result = false;
            int name;
            bool crtnam = !string.IsNullOrWhiteSpace(DriverModel.DriverName) && !(int.TryParse(DriverModel.DriverName, out name)) && !DriverModel.DriverName.All(sp => !char.IsLetter(sp));
            //bool cate = !string.IsNullOrWhiteSpace(Pharmacy.Categary) && !string.IsNullOrEmpty(Pharmacy.Categary);


            if (crtnam )
            {
                result = true;
            }
            return result;
        }

        public bool validation1()
        {
            bool result = false;
            int name;
            bool crtnam = !string.IsNullOrWhiteSpace(DriverModel.DriverName) && !(int.TryParse(DriverModel.DriverName, out name)) && !DriverModel.DriverName.All(sp => !char.IsLetter(sp));
            //bool cate = !string.IsNullOrWhiteSpace(Pharmacy.Categary) && !string.IsNullOrEmpty(Pharmacy.Categary);
            //bool price = !string.IsNullOrWhiteSpace(Pharmacy.UnitPrice) && !string.IsNullOrEmpty(User.Role);
            //bool quan = !string.IsNullOrWhiteSpace(User.Status) && !string.IsNullOrEmpty(User.Status);

            if (crtnam)
            {
                result = true;
            }
            return result;
        }
        private DriverModel _drivermodel = new DriverModel();

        public DriverModel DriverModel
        {
            get { return _drivermodel; }
            set { _drivermodel = value; OnPropertyChanged(); }
        }

        private string _btn;

        public string btn
        {
            get { return _btn; }
            set { _btn = value; OnPropertyChanged(); }
        }

        private string _btnback;

        public string btnback
        {
            get { return _btnback; }
            set { _btnback = value; OnPropertyChanged(); }
        }


        private ObservableCollection<DriverModel> _driverlist;

        public ObservableCollection<DriverModel> DriverList
        {
            get { return _driverlist; }
            set { _driverlist = value; OnPropertyChanged(); }
        }
    }
}
