using MedStarHospital.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace MedStarHospital.ViewModel
{
    public class VMAddTestingType  : ViewModelBase
    {
        private string _btn;

        public string btn
        {
            get { return _btn; }
            set { _btn = value; OnPropertyChanged(); }
        }

        private string _btnBack;

        public string btnBack
        {
            get { return _btnBack; }
            set { _btnBack = value; OnPropertyChanged(); }
        }

        public VMAddTestingType(TestingTypeModel testing = null)
        {
            if (testing == null)
            {
                ISDoctor();
                btn = "ADD";
                btnBack = "CLOSE";
            }
            else
            {
                TestingType = new TestingTypeModel
                {
                    TestingTypeID = testing.TestingTypeID,
                    TestingTypeName = testing.TestingTypeName,
                    Amount = testing.Amount,
                };
                btn = "UPDATE";
                btnBack = "CLOSE";
            }

        }

        public void ISDoctor()
        {
            TestingType = new();
            TestingType.TestingTypeID = Sql_Connection.IsData("tblTestingType") ? int.Parse(Sql_Connection.SpaficDataISINTable("tblTestingType", "TestingTypeID", "TestingTypeID")) + 1 : 1;
        }

        public ICommand cmdOption { get { return new RelayCommand(fnOption); } }

        public ICommand cmdExit { get { return new RelayCommand(fnExit); } }

        public static Action exit;

        public void fnOption(object param)
        {
            switch (btn)
            {
                case "ADD":
                    try
                    {
                        if (validation())
                        {

                            Sql_Connection.sql_connection();
                            string QUERY = $"Insert into tblTestingType values('" + TestingType.TestingTypeID + "','" + TestingType.TestingTypeName + "','" + TestingType.Amount + "')";
                            SqlCommand command = new SqlCommand(QUERY, Sql_Connection.getconnection());
                            SqlDataAdapter adapter = new SqlDataAdapter();
                            adapter.InsertCommand = new SqlCommand(QUERY, Sql_Connection.getconnection());
                            adapter.InsertCommand.ExecuteNonQuery();
                            adapter.Dispose();
                            command.Dispose();
                            Sql_Connection.close_connection();

                            MessageBox.Show("Successfully Added!", "Add Testing Type", MessageBoxButton.OK, MessageBoxImage.Information);
                            exit.Invoke();
                        }
                        else
                        {
                            Sql_Connection.close_connection();
                            MessageBox.Show("Enter the correct value", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        Sql_Connection.close_connection();
                        MessageBox.Show(ex.Message);

                        MessageBox.Show("This Data invalid!", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;

                case "UPDATE":
                    try
                    {
                        if (validation())
                        {
                            Sql_Connection.sql_connection();
                            string Query = $"update tblTestingType set TypeName = '" + TestingType.TestingTypeName + "',Amount='" + TestingType.Amount + "' where TestingTypeID ='" + TestingType.TestingTypeID + "'";
                            SqlCommand command1 = new SqlCommand(Query, Sql_Connection.getconnection());
                            SqlDataAdapter adapter1 = new SqlDataAdapter();
                            adapter1.InsertCommand = new SqlCommand(Query, Sql_Connection.getconnection());
                            adapter1.InsertCommand.ExecuteNonQuery();
                            adapter1.Dispose();
                            command1.Dispose();
                            Sql_Connection.close_connection();
                            MessageBox.Show("Successfully Updated!", "Update Testing Type", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            Sql_Connection.close_connection();
                            MessageBox.Show("Invalid Datas", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch
                    {
                        Sql_Connection.close_connection();
                        MessageBox.Show("You can't update", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
            }
            AppEvents.OnRefreshTestingType();
        }

        void fnExit(object param)
        {
            if (MessageBox.Show("Do you want to close this page?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                exit.Invoke();
            }
        }

        public bool validation()
        {
            bool result = false;
            bool crtid = TestingType.TestingTypeID > 0;
            if (!(Regex.IsMatch(TestingType.TestingTypeName, @"^[a-zA-Z\s]*$")))
            {
                return false;
            }
            

            if (crtid)
            {
                result = true;
            }
            return result;
        }

        private TestingTypeModel _testingtype;
        public TestingTypeModel TestingType
        {
            get { return _testingtype; }
            set
            {
                _testingtype = value;
                OnPropertyChanged();
            }
        }
    }
}
