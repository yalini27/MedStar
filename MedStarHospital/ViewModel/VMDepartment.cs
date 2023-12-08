using MedStarHospital.Model;
using MedStarHospital.View;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Reflection.Metadata.BlobBuilder;

namespace MedStarHospital.ViewModel
{
    public class VMDepartment:ViewModelBase
    {
        public ICommand cmdEdit { get { return new RelayCommand(fnEdit, fnCanExecuteUser); } }

        private string _find;
        public string Find
        {
            get { return _find; }
            set { _find = value; OnPropertyChanged(); AutoApply(); }
        }

        private string _field = "Department ID";
        public string Field { get { return _field; } set { _field = value; OnPropertyChanged(); } }

        private string _column;
        public string Column
        {
            get { return _column; }
            set { _column = value; OnPropertyChanged(); }
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

        public VMDepartment(UserModel user)
        {
            User = user;
            fnView();
            AppEvents.DepartmentRefresh += OnDepartmentRefresh;
        }

        void OnDepartmentRefresh()
        {
            fnView();
        }
        public ICommand cmdAddDep { get { return new RelayCommand(fnAdd, fnCanExecuteUser); } }
        public ICommand cmdDelete { get { return new RelayCommand(fnDelete, fnCanExecuteUser); } }
        public ICommand cmdReset { get { return new RelayCommand(fnReset); } }
        void fnAdd(object param)
        {
            AddDepartmentView dv = new AddDepartmentView();
            dv.DataContext = new VMAddDepartment();
            dv.ShowDialog();
        }

        void fnEdit(object param)
        {
            DepartmentModel ed = (DepartmentModel)param;
            AddDepartmentView addDepartment = new AddDepartmentView();
            addDepartment.DataContext = new VMAddDepartment(ed);
            addDepartment.ShowDialog();
        }
        void fnView()
        {

            Departments = new List<DepartmentModel>();
            Sql_Connection.DBConnection();
            Sql_Connection.sql_connection();

            var SerchQuary = $"select * from tblDepartment";
            var Command = new SqlCommand(SerchQuary, Sql_Connection.getconnection());
            var reader = Command.ExecuteReader();
            while (reader.Read())
            {
                Departments.Add(new DepartmentModel
                {
                    DepartmentID = int.Parse(reader.GetValue(0).ToString()),
                    DepartmentName = (reader.GetValue(1).ToString()),

                });
            }

            Sql_Connection.close_connection();
        }

        void AutoApply()
        {
            switch (Field)
            {
                case "Department ID":
                    Column = "DepartmentID";
                    break;

                case "Department Name":
                    Column = "DepartmentName";
                    break;

                default:
                    MessageBox.Show("Invalid Option");
                    break;
            }
      
                Departments = new List<DepartmentModel>();
                Sql_Connection.sql_connection();
                string QUERY = $"select * from tblDepartment where {Column} Like '%" + Find + "%'";

                SqlCommand command = new SqlCommand(QUERY, Sql_Connection.getconnection());
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Departments.Add(new DepartmentModel
                    {
                        DepartmentID = int.Parse(reader.GetValue(0).ToString()),
                        DepartmentName = (reader.GetValue(1).ToString()),


                    });
                }

                Sql_Connection.close_connection();
        }

        void fnReset(object param)
        {
            Find = string.Empty;
            fnView();
        }
        void fnDelete(object param)
        {
            DepartmentModel bv = param as DepartmentModel;
            try
            {
                if (MessageBox.Show("Do you want to delete this Department?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Sql_Connection.sql_connection();
                    string QUERY = $"delete from tblDepartment where DepartmentID = {bv.DepartmentID}";
                    SqlCommand command = new SqlCommand(QUERY, Sql_Connection.getconnection());
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.InsertCommand = new SqlCommand(QUERY, Sql_Connection.getconnection());
                    adapter.InsertCommand.ExecuteNonQuery();
                    adapter.Dispose();
                    command.Dispose();
                    Sql_Connection.close_connection();
                    MessageBox.Show("Delete Successfully!","Delete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                fnView();
            }
            catch (Exception ex)
            {
                Sql_Connection.close_connection();
                MessageBox.Show("This Department already Booked. So you can't delete", "delete Department", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private DepartmentModel  _department;
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

    }
}
