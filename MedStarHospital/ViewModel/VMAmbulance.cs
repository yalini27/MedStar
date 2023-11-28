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
using System.Windows;
using System.Windows.Media;

namespace MedStarHospital.ViewModel
{
    public class VMAmbulance  :ViewModelBase
    {
        public ICommand cmdEdit { get { return new RelayCommand(fnEdit); } }

        public Brush SliceColor { get; set; }

        private string _find;
        public string Find
        {
            get { return _find; }
            set { _find = value; OnPropertyChanged(); AutoApply(); }
        }

        private string _field = "Ambulance ID";
        public string Field { get { return _field; } set { _field = value; OnPropertyChanged(); } }

        private string _column;
        public string Column
        {
            get { return _column; }
            set { _column = value; OnPropertyChanged(); }
        }


        public VMAmbulance(UserModel user)
        {
            User = user;
            fnView();
            AppEvents.RefreshTestingType += OnRefreshTestingType;
        }
        void OnRefreshTestingType()
        {
            fnView();
        }

        public ICommand cmdAdd { get { return new RelayCommand(fnAdd); } }
        public ICommand cmdDelete { get { return new RelayCommand(fnDelete); } }
        //public ICommand cmdApply { get { return new RelayCommand(fnApply); } }
        public ICommand cmdReset { get { return new RelayCommand(fnReset); } }

        void fnAdd(object param)
        {
            AddAmbulanceView dv = new AddAmbulanceView();
            dv.DataContext = new VMAddAmbulance();
            dv.ShowDialog();
        }

        void fnEdit(object param)
        {
            AmbulanceModel db = (AmbulanceModel)param;
            AddAmbulanceView addtype = new AddAmbulanceView();
            addtype.DataContext = new VMAddAmbulance(db);
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
                case "Ambulance ID":
                    Column = "AmbulanceID";
                    break;

                //case "Status":
                //    Column = "ActiveStatus";
                //    break;

                case "Driver Name":
                    Column = "DriverName";
                    break;

                default:
                    MessageBox.Show("Invalid Option");
                    break;
            }


            AmbulanceList = new ObservableCollection<AmbulanceModel>();
            Sql_Connection.sql_connection();
            string QUERY = $"select * from tblAmbulance where {Column} Like '%" + Find + "%'";
            SqlCommand command = new SqlCommand(QUERY, Sql_Connection.getconnection());
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                AmbulanceList.Add(new AmbulanceModel
                {
                    AmbulanceID = (int)reader.GetValue(0),
                    ActiveStatus = (bool)reader.GetValue(2),
                    AmbulanceNumber = reader.GetValue(3).ToString(),
                    Driver = new DriverModel
                    {
                        DriverID = (string)reader.GetValue(1),
                        DriverName = Sql_Connection.SpaficDataISINTable1("tblDriver", "DriverName", "DriverID", reader.GetValue(1).ToString()),
                    }
                });
            }

            foreach (var data in AmbulanceList)
            {
                if (data.ActiveStatus == true)
                {
                    data.ActiveData = "Active";
                    data.SliceColor = new SolidColorBrush(Colors.Green);
                }
                else if (data.ActiveStatus == false)
                {
                    data.ActiveData = "Inactive";
                    data.SliceColor = new SolidColorBrush(Colors.IndianRed);
                }
                //AmbulanceList.Add(data);
            }
            Sql_Connection.close_connection();

        }

        void fnReset(object param)
        {
            fnView();
            //AmbulanceList = new ObservableCollection<AmbulanceModel>();
            //Sql_Connection.sql_connection();
            //string Query = $"select *  from tblAmbulance ";
            //SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
            //var reader = command.ExecuteReader();
            //while (reader.Read())
            //{
            //    AmbulanceList.Add(new AmbulanceModel
            //    {
            //        AmbulanceID = (int)reader.GetValue(0),
            //        DriverName = (reader.GetValue(1).ToString()),
            //        ActiveStatus = (bool)reader.GetValue(2),
            //        AmbulanceNumber = reader.GetValue(3).ToString(),
            //    });
            //}

            //Sql_Connection.close_connection();
        }

        void fnView()
        {

            AmbulanceList = new ObservableCollection<AmbulanceModel>();
            Sql_Connection.DBConnection();
            Sql_Connection.sql_connection();


            var SerchQuary = $"select *  from tblAmbulance ";

            var Command = new SqlCommand(SerchQuary, Sql_Connection.getconnection());
            var reader = Command.ExecuteReader();
            while (reader.Read())
            {
                AmbulanceList.Add(new AmbulanceModel
                {
                    AmbulanceID = (int)reader.GetValue(0),
                    ActiveStatus = (bool)reader.GetValue(2),
                    AmbulanceNumber = reader.GetValue(3).ToString(),

                    Driver = new DriverModel
                    {
                        DriverID = (string)reader.GetValue(1),
                        DriverName = Sql_Connection.SpaficDataISINTable1("tblDriver", "DriverName", "DriverID", reader.GetValue(1).ToString()),
                    }

                });
            }

            foreach(var data in AmbulanceList)
            {
                if(data.ActiveStatus == true)
                {
                    data.ActiveData = "Active";
                    data.SliceColor = new SolidColorBrush(Colors.Green);
                }
                else if(data.ActiveStatus == false)
                {
                    data.ActiveData = "Inactive";
                    data.SliceColor = new SolidColorBrush(Colors.IndianRed);
                }
                //AmbulanceList.Add(data);
            }

            Sql_Connection.close_connection();
        }
        public static Action exit;
        void fnDelete(object param)
        {

            AmbulanceModel bv = param as AmbulanceModel;
            try
            {
                if (MessageBox.Show("Do you want to delete this Ambulance Service?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Sql_Connection.sql_connection();
                    string QUERY = $"delete from tblAmbulance where AmbulanceID = {bv.AmbulanceID}";
                    SqlCommand command = new SqlCommand(QUERY, Sql_Connection.getconnection());
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.InsertCommand = new SqlCommand(QUERY, Sql_Connection.getconnection());
                    adapter.InsertCommand.ExecuteNonQuery();
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

        private AmbulanceModel _ambulancemodel;
        public AmbulanceModel AmbulanceModel
        {
            get { return _ambulancemodel; }
            set { _ambulancemodel = value; OnPropertyChanged(); }
        }
        private ObservableCollection<AmbulanceModel> _ambulanceList;
        public ObservableCollection<AmbulanceModel> AmbulanceList
        {
            get { return _ambulanceList; }
            set { _ambulanceList = value; OnPropertyChanged(); }
        }
    }
}
