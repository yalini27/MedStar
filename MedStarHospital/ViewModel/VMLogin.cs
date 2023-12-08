using MedStarHospital.Model;
using MedStarHospital.View;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MedStarHospital.ViewModel
{
    public class VMLogin:ViewModelBase
    {
        public VMLogin()
        {
            fngetUser();
            SelectedUser = new UserModel();  
        }
        public ICommand cmdExit { get { return new RelayCommand(fnExit); } }
        public ICommand cmdAbout { get { return new RelayCommand(fnAbout); } }
        public static Action exit;

        void fnExit(object param)
        {
            if (MessageBox.Show("Do you want to close this page?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                exit.Invoke();
            }
        }
        

        void fnAbout(object param)
        {
            AboutView hv = new AboutView();
            hv.DataContext = new VMAbout();
            exit.Invoke();
            hv.ShowDialog();
        }


        private UserModel _selecteduser = new UserModel();

        public UserModel SelectedUser
        {
            get { return _selecteduser; }
            set { _selecteduser = value; OnPropertyChanged(); }
        }

        private ObservableCollection<UserModel> _userlist;

        public ObservableCollection<UserModel> UserList
        {
            get { return _userlist; }
            set { _userlist = value; OnPropertyChanged(); }
        }

        public ICommand cmdLogin { get { return new RelayCommand(fnLogin); } }


        void fnLogin(object param)
        {
            var password = param as PasswordBox;
           
            if (string.IsNullOrWhiteSpace(SelectedUser.UserName))
            {
                MessageBox.Show("Enter valid Username ", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrWhiteSpace(SelectedUser.Password))
            {
                MessageBox.Show("Enter valid Password ", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!(SelectedUser.Password == password.Password))
            {
                MessageBox.Show("Incorrect Password ", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                HomeView main = new HomeView();
                main.DataContext = new VMHome(SelectedUser);
                exit.Invoke();
                main.ShowDialog();
            }
        }

        void fngetUser()
        {
            Sql_Connection.DBConnection();
            Sql_Connection.sql_connection();
            UserList = new ObservableCollection<UserModel>();
           
            string Query = $"select * from tblUser where status_state = 'Active'";

            SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                UserList.Add(new UserModel
                {
                    UserID = int.Parse(reader.GetValue(0).ToString()),
                    UserName = reader.GetValue(1).ToString(),
                    Role = reader.GetValue(3).ToString(),
                    Password = reader.GetValue(2).ToString(),
                    Status = reader.GetValue(4).ToString(),    
                    DriverID = reader.GetValue(5).ToString()
                });
            }

            Sql_Connection.close_connection();
        }


        private UserModel _user = new UserModel();

        public UserModel User
        {
            get { return _user; }
            set { _user = value;OnPropertyChanged(); }
        }


        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

    }
}
