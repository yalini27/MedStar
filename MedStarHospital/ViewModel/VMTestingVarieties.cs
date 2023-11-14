using MedStarHospital.Model;
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
    public class VMTestingVarieties :  ViewModelBase
    {
        public ICommand cmdExit { get { return new RelayCommand(fnExit); } }
        public VMTestingVarieties()
        {
            fngetTestingType();
            SelectedTestingType = new TestingTypeModel();
        }

        public static Action exit;
        void fnExit(object param)
        {
            exit.Invoke();
        }
        void fngetTestingType()
        {
            Sql_Connection.sql_connection();
            TestingTypeList = new ObservableCollection<TestingTypeModel>();
            string Query = $"Select * from tblTestingType";
            SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                TestingTypeList.Add(new TestingTypeModel
                {
                    TestingTypeID = (int)reader.GetValue(0),
                    TestingTypeName = (string)reader.GetValue(1)
                });
            }
            Sql_Connection.close_connection();
        }

        private TestingTypeModel _testingType;
        public TestingTypeModel TestingType
        {
            get { return _testingType; }
            set { _testingType = value; OnPropertyChanged(); }
        }

        private ObservableCollection<TestingTypeModel> _testingtypelist;
        public ObservableCollection<TestingTypeModel> TestingTypeList
        {
            get { return _testingtypelist; }
            set { _testingtypelist = value; OnPropertyChanged(); }
        }

        private TestingTypeModel _selectedtestingtype;
        public TestingTypeModel SelectedTestingType
        {
            get { return _selectedtestingtype; }
            set
            {
                _selectedtestingtype = value; OnPropertyChanged();

            }
        }

    }
}
