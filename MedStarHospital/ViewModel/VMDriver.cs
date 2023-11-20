using MedStarHospital.Model;
using MedStarHospital.View;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;

namespace MedStarHospital.ViewModel
{
    public class VMDriver  :ViewModelBase
    {
        public ICommand cmdEdit { get { return new RelayCommand(fnEdit); } }

        private string _find;
        public string Find
        {
            get { return _find; }
            set { _find = value; OnPropertyChanged(); AutoApply(); }
        }

        private string _field = "Driver ID";
        public string Field { get { return _field; } set { _field = value; OnPropertyChanged(); } }

        private string _column;
        public string Column
        {
            get { return _column; }
            set { _column = value; OnPropertyChanged(); }
        }

        public static Action driverrefresh;

        public VMDriver(UserModel user)
        {
            User = user;
            fnView();
            driverrefresh += OnDriverRefresh;
            //AppEvents.RefreshTestingType += OnRefreshTestingType;
        }
        void OnDriverRefresh()
        {
            fnView();
        }

        public ICommand cmdAdd { get { return new RelayCommand(fnAdd); } }
        //public ICommand cmdDelete { get { return new RelayCommand(fnDelete, fnCanExecuteUser); } }
        //public ICommand cmdApply { get { return new RelayCommand(fnApply); } }
        public ICommand cmdReset { get { return new RelayCommand(fnReset); } }

        void fnAdd(object param)
        {
            AddDriverView dv = new AddDriverView();
            dv.DataContext = new VMAddDriver(User);
            dv.ShowDialog();
        }

        void fnEdit(object param)
        {
            DriverModel db = (DriverModel)param;
            AddDriverView addtype = new AddDriverView();
            addtype.DataContext = new VMAddDriver(User,db);
            addtype.ShowDialog();
        }

        private UserModel userr;

        public UserModel User
        {
            get { return userr; }
            set { userr = value; OnPropertyChanged(); }
        }


        bool fnCanExecuteUser(object o)
        {
            return User.Role.ToLower() == "admin" ? true : false;
        }

        void AutoApply()
        {
            switch (Field)
            {
                case "Driver ID":
                    Column = "DriverID";
                    break;

                case "Driver Name":
                    Column = "DriverName";
                    break;

                default:
                    MessageBox.Show("Invalid Option");
                    break;
            }


            DriverList = new ObservableCollection<DriverModel>();
            Sql_Connection.sql_connection();
            string QUERY = $"select * from tblDriver where {Column} Like '%" + Find + "%'";
            SqlCommand command = new SqlCommand(QUERY, Sql_Connection.getconnection());
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                DriverList.Add(new DriverModel
                {
                    DriverID = (string)reader.GetValue(0),
                    DriverName = (reader.GetValue(1).ToString()),
                    PhoneNumber = (long)reader.GetValue(2),
                    Password = (string)reader.GetValue(3),
                    CurrentLocation = (string)reader.GetValue(4),
                    ServiceLocation = (string)reader.GetValue(5),
                    Status = (string)reader.GetValue(6),
                });
            }


            Sql_Connection.close_connection();

        }

        void fnReset(object param)
        {
            DriverList = new ObservableCollection<DriverModel>();
            Sql_Connection.sql_connection();
            string Query = $"select *  from tblDriver ";
            SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                DriverList.Add(new DriverModel
                {
                    DriverID = (string)reader.GetValue(0),
                    DriverName = (reader.GetValue(1).ToString()),
                    PhoneNumber = (long)reader.GetValue(2),
                    Password = (string)reader.GetValue(3),
                    CurrentLocation = (string)reader.GetValue(4),
                    ServiceLocation = (string)reader.GetValue(5),
                    Status = (string)reader.GetValue(6),
                });
            }

            Sql_Connection.close_connection();
        }

        void fnView()
        {

            DriverList = new ObservableCollection<DriverModel>();
            Sql_Connection.DBConnection();
            Sql_Connection.sql_connection();


            var SerchQuary = $"select *  from tblDriver ";

            var Command = new SqlCommand(SerchQuary, Sql_Connection.getconnection());
            var reader = Command.ExecuteReader();
            while (reader.Read())
            {
                DriverList.Add(new DriverModel
                {
                    DriverID = (string)reader.GetValue(0),
                    DriverName = (reader.GetValue(1).ToString()),
                    PhoneNumber = (long)reader.GetValue(2),
                    Password = (string)reader.GetValue(3),
                    CurrentLocation = (string) reader.GetValue(4),
                    ServiceLocation = (string)reader.GetValue(5),
                    Status = (string)reader.GetValue(6),
                });
            }

            Sql_Connection.close_connection();
        }
        public static Action exit;
        //void fnDelete(object param)
        //{

        //    DriverModel bv = param as DriverModel;
        //    try
        //    {
        //        if (MessageBox.Show("Do you want to delete this Testing Type?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        //        {
        //            Sql_Connection.sql_connection();
        //            string QUERY = $"delete from tblTestingType where TestingTypeID = {bv.TestingTypeID}";
        //            SqlCommand command = new SqlCommand(QUERY, Sql_Connection.getconnection());
        //            SqlDataAdapter adapter = new SqlDataAdapter();
        //            adapter.InsertCommand = new SqlCommand(QUERY, Sql_Connection.getconnection());
        //            adapter.InsertCommand.ExecuteNonQuery();
        //            adapter.Dispose();
        //            command.Dispose();
        //            Sql_Connection.close_connection();
        //            MessageBox.Show("Delete Successfully!", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
        //            fnView();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Sql_Connection.close_connection();
        //        MessageBox.Show("You can not delete", "delete Testing Type", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        private DriverModel _driver;
        public DriverModel Driver
        {
            get { return _driver; }
            set { _driver = value; OnPropertyChanged(); }
        }
        private ObservableCollection<DriverModel> _driverlist;
        public ObservableCollection<DriverModel> DriverList
        {
            get { return _driverlist; }
            set { _driverlist = value; OnPropertyChanged(); }
        }
    }
}
