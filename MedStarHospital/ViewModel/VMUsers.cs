using MedStarHospital.Model;
using MedStarHospital.View;
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
    public class VMUsers : ViewModelBase
    {
        public VMUsers()
        {
            fnView();
            AppEvents.RefreshUser += OnUserRefresh;
        }

        public void OnUserRefresh()
        {
            fnView();
        }

        private string _find;

        public string Find
        {
            get { return _find; }
            set { _find = value; OnPropertyChanged(); fnAutoApply(); }
        }


        private string _field = "User ID";

        public string Field
        {
            get { return _field; }
            set { _field = value; OnPropertyChanged(); }
        }

        private string _column;

        public string Column
        {
            get { return _column; }
            set { _column = value; OnPropertyChanged(); }
        }

        void fnAutoApply()
        {


            switch (Field) 
            {
                case "User ID":
                    Column = "userID";
                    break;
                case "User Name":
                    Column = "UserName";
                    break;

                case "Designation":
                    Column = "Role_name";
                    break;

                case "Status":
                    Column = "status_state";
                    break;
            }
            UserList = new ObservableCollection<UserModel>();
            Sql_Connection.sql_connection();
            //string Query = $"select * from tblUser where {Column}  Like '%" + Find + "%'";
            string Query = $"select * from tblUser where {Column} Like '%{Find}%'";
            SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                UserList.Add(new UserModel
                {
                    UserID = int.Parse(reader.GetValue(0).ToString()),
                    UserName = reader.GetValue(1).ToString(),
                    Password = reader.GetValue(2).ToString(),
                    Role = reader.GetValue(3).ToString(),
                    Status = reader.GetValue(4).ToString()
                });
            }
            reader.Close();
            Sql_Connection.close_connection();

        }



        void fnView()
        {
            UserList = new ObservableCollection<UserModel>();
            Sql_Connection.DBConnection();
            Sql_Connection.sql_connection();
            string Query = $"select * from tblUser";
            SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                UserList.Add(new UserModel
                {
                    UserID = int.Parse(reader.GetValue(0).ToString()),
                    UserName = reader.GetValue(1).ToString(),
                    Password = reader.GetValue(2).ToString(),
                    Role = reader.GetValue(3).ToString(),
                    Status = reader.GetValue(4).ToString()
                });
            }
        }

        public ICommand cmdReset { get { return new RelayCommand(fnReset); } }

        public ICommand cmdAdd { get { return new RelayCommand(fnAdd); } }

        public ICommand cmdEdit { get { return new RelayCommand(fnEdit); } }

        void fnReset(object param)
        {
            fnView();
        }


        void fnEdit(object param)
        {
            UserModel us = param as UserModel;
            AddUserView av = new AddUserView();
            av.DataContext = new VMAddUserView(us);
            av.ShowDialog();
        }
        private UserModel _user = new UserModel();

        public UserModel User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(); }
        }


        private ObservableCollection<UserModel> _userlist;

        public ObservableCollection<UserModel> UserList
        {
            get { return _userlist; }
            set { _userlist = value; OnPropertyChanged(); }
        }

        void fnAdd(object param)
        {
            AddUserView av = new AddUserView();
            av.DataContext = new VMAddUserView();
            av.ShowDialog();
        }
    }
}
