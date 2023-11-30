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
    public class VMAddUserView : ViewModelBase
    {
        public ICommand cmdback { get { return new RelayCommand(fnBack); } }

        public ICommand cmdOption { get { return new RelayCommand(fnOption); } }

        public static Action exit;

        public VMAddUserView(UserModel user = null)
        {
            ISUser();
            if (user == null)
            {
                btn = "ADD";
                btnback = "BACK";
            }
            else
            {
                User = user;
                User = new UserModel
                {
                    UserID = user.UserID,
                    UserName = user.UserName,
                    Password = user.Password,
                    Role = user.Role,
                    Status = user.Status
                };
                btn = "UPDATE";
                btnback = "BACK";
            }
            UserList = new ObservableCollection<UserModel>();
            //fngetRole();
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
                    if (validation())
                    {
                        Sql_Connection.sql_connection();
                        string Query = $"insert into  tblUser (userID, UserName, Password_no, Role_name,status_state) values('" + User.UserID + "','" + User.UserName + "','" + User.Password + "','" + User.Role + "','" + User.Status + "')";
                        SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.InsertCommand = new SqlCommand(Query, Sql_Connection.getconnection());
                        adapter.InsertCommand.ExecuteReader();
                        adapter.Dispose();
                        command.Dispose();
                        Sql_Connection.close_connection();
                        MessageBox.Show("Successfully Added", "Add", MessageBoxButton.OK, MessageBoxImage.Information);
                        exit.Invoke();
                    }
                    else
                    {
                        MessageBox.Show("Invalid datas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                   

                    case "UPDATE":
                    if (validation1())
                    {
                        Sql_Connection.sql_connection();
                        string query = $"update tblUser set UserName = '" + User.UserName + "',Password_no='" + User.Password + "',Role_name='" + User.Role + "',status_state='" + User.Status + "'where userID='" + User.UserID + "'";
                        SqlCommand command1 = new SqlCommand(query, Sql_Connection.getconnection());
                        SqlDataAdapter adapter1 = new SqlDataAdapter();
                        adapter1.UpdateCommand = new SqlCommand(query, Sql_Connection.getconnection());
                        adapter1.UpdateCommand.ExecuteReader();
                        adapter1.Dispose();
                        command1.Dispose();
                        Sql_Connection.close_connection();
                        MessageBox.Show("Update successfully!", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                        exit.Invoke();
                    }
                    else
                    {
                        MessageBox.Show("Invalid datas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    break;
                }


            AppEvents.OnRefreshUser();
        }

   



        public bool validation()
        {
            bool result = false;
            int name;
            bool crtnam = !string.IsNullOrWhiteSpace(User.UserName) && !(int.TryParse(User.UserName, out name)) && !User.UserName.All(sp => !char.IsLetter(sp));
            bool password = !string.IsNullOrWhiteSpace(User.Password) && !string.IsNullOrEmpty(User.Password);
            bool role = !string.IsNullOrWhiteSpace(User.Role) && !string.IsNullOrEmpty(User.Role);
            bool status = !string.IsNullOrWhiteSpace(User.Status) && !string.IsNullOrEmpty(User.Status);
            if (Sql_Connection.IsData("tblUser", "UserName", User.UserName))
            {
                MessageBox.Show("This user name already insert", "Add user", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (crtnam && password && role && status)
            {
                result = true;
            }
            return result;
        }

        public bool validation1()
        {
            bool result = false;
            int name;
            bool crtnam = !string.IsNullOrWhiteSpace(User.UserName) && !(int.TryParse(User.UserName, out name)) && !User.UserName.All(sp => !char.IsLetter(sp));
            bool password = !string.IsNullOrWhiteSpace(User.Password) && !string.IsNullOrEmpty(User.Password);
            bool role = !string.IsNullOrWhiteSpace(User.Role) && !string.IsNullOrEmpty(User.Role);
            bool status = !string.IsNullOrWhiteSpace(User.Status) && !string.IsNullOrEmpty(User.Status);
      
            if (crtnam && password && role && status)
            {
                result = true;
            }
            return result;
        }
        private UserModel _user = new UserModel();

        public UserModel User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(); }
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


        private ObservableCollection<UserModel> _userlist;

        public ObservableCollection<UserModel> UserList
        {
            get { return _userlist; }
            set { _userlist = value; OnPropertyChanged(); }
        }

    }
}
