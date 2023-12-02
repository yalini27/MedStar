using MedStarHospital.Model;
using MedStarHospital.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MedStarHospital.ViewModel
{
    public class VMAddDepartment : ViewModelBase
    {
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

        void fngetDoctors()
        {
            Sql_Connection.sql_connection();
            Doctors = new ObservableCollection<DoctorModel>();
            string Query = $"select * from tblDoctor";
            SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Doctors.Add(new DoctorModel
                {

                        Doctorid = (int)reader.GetValue(0),
                        Doctorname = reader.GetValue(1).ToString()

                });
            }
            Sql_Connection.close_connection();
        }

        public VMAddDepartment(DepartmentModel department = null)
        {
            fngetDoctors();
            SelectedDoctor = new DoctorModel();
            if (department == null)
            {
                ISDepartment();
                btn = "ADD";
                btnback = "CLOSE";

            }
            else
            {
                Department = new DepartmentModel
                {
                    DepartmentID = department.DepartmentID,
                    DepartmentName = department.DepartmentName
                };
                
                btn = "UPDATE";
                btnback = "BACK";
                
            }
            
        }
        public void ISDepartment()
        {
            Department = new();
            var id =Sql_Connection.SpaficDataISINTable("tblDepartment", "DepartmentID", "DepartmentID");
            
            if (id == "")
            {
                Department.DepartmentID = 1;
            }

            else
            {
                Department.DepartmentID = Convert.ToInt32(id) + 1;
            }
        }

    public ICommand cmdOptions { get { return new RelayCommand(fnOptions); } }

        public void fnOptions(object param)
        {
            switch(btn)
            {
                case "ADD":
                    try
                    {
                        if (validation())
                        {

                            Sql_Connection.sql_connection();
                            string QUERY = $"Insert into tblDepartment values('" + Department.DepartmentID + "','" + Department.DepartmentName +"')";
                            SqlCommand command = new SqlCommand(QUERY, Sql_Connection.getconnection());
                            SqlDataAdapter adapter = new SqlDataAdapter();
                            adapter.InsertCommand = new SqlCommand(QUERY, Sql_Connection.getconnection());
                            adapter.InsertCommand.ExecuteNonQuery();
                            adapter.Dispose();
                            command.Dispose();
                            Sql_Connection.close_connection();

                            MessageBox.Show("Successfully Added!", "Add Department", MessageBoxButton.OK, MessageBoxImage.Information);
                            exit.Invoke();
                        }
                        else
                        {
                            Sql_Connection.close_connection();
                            MessageBox.Show("Enter the correct value", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch
                    {
                        Sql_Connection.close_connection();
                        MessageBox.Show("This Data invalid!", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
              
            break;

                case "UPDATE":
                    try
                    {
                        if(validation())
                        { 
                        Sql_Connection.sql_connection();
                        string Query = $"update tblDepartment set DepartmentName ='" + Department.DepartmentName  + "' where DepartmentID='" + Department.DepartmentID + "'";
                        SqlCommand command1 = new SqlCommand(Query, Sql_Connection.getconnection());
                        SqlDataAdapter adapter1 = new SqlDataAdapter();
                        adapter1.InsertCommand = new SqlCommand(Query, Sql_Connection.getconnection());
                        adapter1.InsertCommand.ExecuteNonQuery();
                        adapter1.Dispose();
                        command1.Dispose();
                        Sql_Connection.close_connection();

                        MessageBox.Show("Successfully updated!", "Add Book", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            Sql_Connection.close_connection();
                            MessageBox.Show("Enter the correct value", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                    catch
                    {
                        Sql_Connection.close_connection();
                        MessageBox.Show("You can't update", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    break;
            }
            AppEvents.OnRefreshDepartment();

        }
        public ICommand cmdExit { get { return new RelayCommand(fnExit); } }
        public static Action exit;
        

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
            bool crtid = Department.DepartmentID > 0;

            if(!(Regex.IsMatch(Department.DepartmentName, @"^[a-zA-Z\s]*$")))
            {
                return false;
            }
            int name;

            bool crtnam = !string.IsNullOrWhiteSpace(Department.DepartmentName) && !(int.TryParse(Department.DepartmentName, out name)) && !Department.DepartmentName.All(sp => !char.IsLetter(sp));
 

            if ( crtnam && crtid)
            {
                result = true;
            }
            return result;
        }

        private DepartmentModel _department;
        public DepartmentModel Department
        {
            get { return _department; }
            set
            {
                _department = value;
                OnPropertyChanged();
            }
        }

        private List<DepartmentModel> _departments;

        public List<DepartmentModel> Departments
        {
            get { return _departments; }
            set { _departments = value; OnPropertyChanged(); }
        }

        private ObservableCollection <DoctorModel> _doctormoddel;
        public ObservableCollection<DoctorModel> Doctors
        {
            get { return _doctormoddel; }
            set { _doctormoddel = value; OnPropertyChanged(); }
        }
        private DoctorModel _selectedDoctor;

        public DoctorModel SelectedDoctor
        {
            get { return _selectedDoctor; }
            set { _selectedDoctor = value; OnPropertyChanged(); }
        }

    }
}
