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
using System.Collections.ObjectModel;

namespace MedStarHospital.ViewModel
{
    public class VMAddAmbulance  :ViewModelBase
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

        private ObservableCollection<DriverModel> _driverCollection;

        public ObservableCollection<DriverModel> DriverCollection
        {
            get { return _driverCollection ; }
            set { _driverCollection = value; OnPropertyChanged(); }
        }

        private DriverModel _selecteddriver = new DriverModel();

        public DriverModel SelectedDriver
        {
            get { return _selecteddriver; }
            set { _selecteddriver = value; OnPropertyChanged(); }
        }


        void fnGetDriver()
        {

            DriverCollection = new ObservableCollection<DriverModel>();
            Sql_Connection.DBConnection();
            Sql_Connection.sql_connection();


            var SerchQuary = $"select *  from tblDriver ";

            var Command = new SqlCommand(SerchQuary, Sql_Connection.getconnection());
            var reader = Command.ExecuteReader();
            while (reader.Read())
            {
                DriverModel model = new DriverModel()
                {
                    DriverID = (string)reader.GetValue(0),
                    DriverName = (reader.GetValue(1).ToString()),
                    PhoneNumber = (long)reader.GetValue(2),
                    Password = (string)reader.GetValue(3),
                    CurrentLocation = (string)reader.GetValue(4),
                    ServiceLocation = (string)reader.GetValue(5),
                    Status = (string)reader.GetValue(6),
                };
                DriverCollection.Add(model);                    
                    
                    


               
            }

            Sql_Connection.close_connection();
            
        }

        public VMAddAmbulance(AmbulanceModel ambulance = null)
        {

            fnGetDriver();
            if (ambulance == null)
            {
                ISAmbulance();
                btn = "ADD";
                btnBack = "BACK";
            }
            else
            {
                Ambulance = new AmbulanceModel
                {
                    AmbulanceID = ambulance.AmbulanceID,
                    Driver = ambulance.Driver,
                    AmbulanceNumber = ambulance.AmbulanceNumber,
                    ActiveStatus = ambulance.ActiveStatus
                  
                    //Active = ambulance.Active,
                    //Inactive = ambulance.Inactive
                };
                if (ambulance.ActiveStatus == true)
                {
                    Ambulance.Active = true;
                }
                else if(ambulance.ActiveStatus == false)
                {
                    Ambulance.Inactive = true;
                }
                btn = "UPDATE";
                btnBack = "BACK";
            }

        }

        public void ISAmbulance()
        {
            Ambulance = new();
            Ambulance.AmbulanceID = Sql_Connection.IsData("tblAmbulance") ? int.Parse(Sql_Connection.SpaficDataISINTable("tblAmbulance", "AmbulanceID", "AmbulanceID")) + 1 : 1;
        }

        public ICommand cmdOption { get { return new RelayCommand(fnOption); } }

        public ICommand cmdExit { get { return new RelayCommand(fnExit); } }

        public static Action exit;

        public void fnOption(object param)
        {
            if(Ambulance.Active==true)
            {
                Ambulance.ActiveStatus = true;
            }
            else if(Ambulance.Inactive==true)
            {
                Ambulance.ActiveStatus = false;
            }
            switch (btn)
            {
                case "ADD":
                    try
                    {
                        if (validation())
                        {

                            Sql_Connection.sql_connection();
                            string QUERY = $"Insert into tblAmbulance values('" + Ambulance.AmbulanceID + "','" + SelectedDriver.DriverID + "','"+ Ambulance.ActiveStatus + "','" +Ambulance.AmbulanceNumber + "')";
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
                            string Query = $"update tblAmbulance set DriverID = '" + Ambulance.Driver.DriverID + "',ActiveStatus='" + Ambulance.ActiveStatus + "',AmbulanceNumber='" + Ambulance.AmbulanceNumber + "' where AmbulanceID ='" + Ambulance.AmbulanceID + "'";
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
            bool crtid = Ambulance.AmbulanceID > 0;
            //if (!(Regex.IsMatch(Ambulance.DriverName, @"^[a-zA-Z\s]*$")))
            //{
            //    return false;
            //}


            if (crtid)
            {
                result = true;
            }
            return result;
        }

        private AmbulanceModel _ambulance;
        public AmbulanceModel Ambulance
        {
            get { return _ambulance; }
            set
            {
                _ambulance = value;
                OnPropertyChanged();
            }
        }
    }
}
