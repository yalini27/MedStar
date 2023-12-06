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
    public class VMTestingType  : ViewModelBase
    {
        public ICommand cmdEdit { get { return new RelayCommand(fnEdit, fnCanExecuteUser); } }

        private string _find;
        public string Find
        {
            get { return _find; }
            set { _find = value; OnPropertyChanged(); AutoApply(); }
        }

        private string _field = "Testing ID";
        public string Field { get { return _field; } set { _field = value; OnPropertyChanged(); } }

        private string _column;
        public string Column
        {
            get { return _column; }
            set { _column = value; OnPropertyChanged(); }
        }


        public VMTestingType( UserModel user)
        {
            User = user;
            fnView();
            AppEvents.RefreshTestingType += OnRefreshTestingType;
        }
        void OnRefreshTestingType()
        {
            fnView();
        }

        public ICommand cmdAdd { get { return new RelayCommand(fnAdd, fnCanExecuteUser); } }
        public ICommand cmdDelete { get { return new RelayCommand(fnDelete, fnCanExecuteUser); } }
        //public ICommand cmdApply { get { return new RelayCommand(fnApply); } }
        public ICommand cmdReset { get { return new RelayCommand(fnReset); } }

        void fnAdd(object param)
        {
            ViewAddTestingType dv = new ViewAddTestingType();
            dv.DataContext = new VMAddTestingType();
            dv.ShowDialog();
        }

        void fnEdit(object param)
        {
            TestingTypeModel db = (TestingTypeModel)param;
            ViewAddTestingType addtype = new ViewAddTestingType();
            addtype.DataContext = new VMAddTestingType(db);
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
                case "Testing ID":
                    Column = "TestingTypeID";
                    break;

                case "Testing Type":
                    Column = "TypeName";
                    break;

                case "Amount":
                    Column = "Amount";
                    break;

                default:
                    MessageBox.Show("Invalid Option");
                    break;
            }


            TestingTypeList = new ObservableCollection<TestingTypeModel>();
            Sql_Connection.sql_connection();
            string QUERY = $"select * from tblTestingType where {Column} Like '%" + Find + "%'";
            SqlCommand command = new SqlCommand(QUERY, Sql_Connection.getconnection());
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                TestingTypeList.Add(new TestingTypeModel
                {
                    TestingTypeID = (int)reader.GetValue(0),
                    TestingTypeName = (reader.GetValue(1).ToString()),
                    Amount = (decimal)reader.GetValue(2),
                });
            }


            Sql_Connection.close_connection();

        }

        void fnReset(object param)
        {
            TestingTypeList = new ObservableCollection<TestingTypeModel>();
            Sql_Connection.sql_connection();
            string Query = $"select *  from tblTestingType ";
            SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                TestingTypeList.Add(new TestingTypeModel
                {
                    TestingTypeID = (int)reader.GetValue(0),
                    TestingTypeName = (reader.GetValue(1).ToString()),
                    Amount = (decimal)reader.GetValue(2),
                });
            }

            Sql_Connection.close_connection();
        }

        void fnView()
        {

            TestingTypeList = new ObservableCollection<TestingTypeModel>();
            Sql_Connection.DBConnection();
            Sql_Connection.sql_connection();


            var SerchQuary = $"select *  from tblTestingType ";

            var Command = new SqlCommand(SerchQuary, Sql_Connection.getconnection());
            var reader = Command.ExecuteReader();
            while (reader.Read())
            {
                TestingTypeList.Add(new TestingTypeModel
                {
                    TestingTypeID = (int)reader.GetValue(0),
                    TestingTypeName = (reader.GetValue(1).ToString()),
                    Amount =(decimal)reader.GetValue(2),
                });
            }

            Sql_Connection.close_connection();
        }
        public static Action exit;
        void fnDelete(object param)
        {

            TestingTypeModel bv = param as TestingTypeModel;
            try
            {
                if (MessageBox.Show("Do you want to delete this Testing Type?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Sql_Connection.sql_connection();
                    string QUERY = $"delete from tblTestingType where TestingTypeID = {bv.TestingTypeID}";
                    SqlCommand command = new SqlCommand(QUERY, Sql_Connection.getconnection());
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.DeleteCommand = new SqlCommand(QUERY, Sql_Connection.getconnection());
                    adapter.DeleteCommand.ExecuteNonQuery();
                    adapter.Dispose();
                    command.Dispose();
                    Sql_Connection.close_connection();
                    MessageBox.Show("Delete Successfully!", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
                    fnView();
                }

            }
            catch (Exception ex)
            {
                Sql_Connection.close_connection();
                MessageBox.Show("You can not delete", "delete Testing Type", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private TestingTypeModel _testingtype;
        public TestingTypeModel TestingType
        {
            get { return _testingtype; }
            set { _testingtype = value; OnPropertyChanged(); }
        }
        private ObservableCollection<TestingTypeModel> _testingTypelist;
        public ObservableCollection<TestingTypeModel> TestingTypeList
        {
            get { return _testingTypelist; }
            set { _testingTypelist = value; OnPropertyChanged(); }
        }
    }
}
