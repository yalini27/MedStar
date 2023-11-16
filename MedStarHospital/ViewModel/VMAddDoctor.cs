using MedStarHospital.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MedStarHospital.ViewModel
{
    public class VMAddDoctor :  ViewModelBase
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

        void fngetDepartment()
        {
            Sql_Connection.sql_connection();
            Departments = new List<DepartmentModel>();
            string Query = $"Select * from tblDepartment";
            SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Departments.Add(new DepartmentModel
                {
                    DepartmentID = (int)reader.GetValue(0),
                    DepartmentName = (string)reader.GetValue(1)

                });
            }
            Sql_Connection.close_connection();
        }


        public VMAddDoctor(DoctorModel doctor = null)
        {
            fngetDepartment();
            SelectedDepartment = new DepartmentModel();
            if (doctor == null)
            {
                ISDoctor();
                btn = "ADD";
                btnBack = "BACK";
            }
            else
            {
                Doctor = new DoctorModel
                {
                    Doctorid = doctor.Doctorid,
                    Doctorname = doctor.Doctorname,
                    Department = doctor.Department,
                    Visitingtime = doctor.Visitingtime

                };

                SelectedDepartment = Departments.FirstOrDefault(d => d.DepartmentID == Doctor.Department.DepartmentID);
                btn = "UPDATE";
                btnBack = "BACK";
            }

        }

        public void ISDoctor()
        {
            Doctor = new();
            Doctor.Doctorid = Sql_Connection.IsData("tblDoctor") ? int.Parse(Sql_Connection.SpaficDataISINTable("tblDoctor", "DoctorID", "DoctorID")) + 1 : 1;
        }

        public ICommand cmdOption { get { return new RelayCommand(fnOption); } }

        public ICommand cmdExit { get { return new RelayCommand(fnExit); } }

        public static Action exit;

        public void fnOption(object param)
        {
            switch(btn)
            {
                case "ADD":
                    try
                    {
                        if (validation())
                        {

                            Sql_Connection.sql_connection();
                            string QUERY = $"Insert into tblDoctor values('" + Doctor.Doctorid + "','" + Doctor.Doctorname + "','" +SelectedDepartment.DepartmentID +"','" + Doctor.Visitingtime + "')";
                            SqlCommand command = new SqlCommand(QUERY, Sql_Connection.getconnection());
                            SqlDataAdapter adapter = new SqlDataAdapter();
                            adapter.InsertCommand = new SqlCommand(QUERY, Sql_Connection.getconnection());
                            adapter.InsertCommand.ExecuteNonQuery();
                            adapter.Dispose();
                            command.Dispose();
                            Sql_Connection.close_connection();

                            MessageBox.Show("Successfully Added!", "Add Doctor", MessageBoxButton.OK, MessageBoxImage.Information);
                            exit.Invoke();
                        }
                        else
                        {
                            Sql_Connection.close_connection();
                            MessageBox.Show("Enter the correct value", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch(Exception ex)
                    {
                        Sql_Connection.close_connection();
       
                        MessageBox.Show("This Data invalid!", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;

                case "UPDATE":
                    try
                    {
                        if (validation())
                        {
                            Sql_Connection.sql_connection();
                            string Query = $"update tblDoctor set DoctorName = '" + Doctor.Doctorname+ "',DepID='" + SelectedDepartment.DepartmentID + "',VisitingTime='" + Doctor.Visitingtime  + "' where DoctorID ='" + Doctor.Doctorid + "'";
                            SqlCommand command1 = new SqlCommand(Query, Sql_Connection.getconnection());
                            SqlDataAdapter adapter1 = new SqlDataAdapter();
                            adapter1.InsertCommand = new SqlCommand(Query, Sql_Connection.getconnection());
                            adapter1.InsertCommand.ExecuteNonQuery();
                            adapter1.Dispose();
                            command1.Dispose();
                            Sql_Connection.close_connection();
                            MessageBox.Show("Successfully Updated!", "Update Doctor", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            Sql_Connection.close_connection();
                            MessageBox.Show("Invalid Datas",  "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch
                    {
                        Sql_Connection.close_connection();
                        MessageBox.Show("You can't update", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
            }
            AppEvents.OnRefreshDoctor();
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
            bool crtid = Doctor.Doctorid > 0;
            if(!(Regex.IsMatch(Doctor.Doctorname, @"^[a-zA-Z\s]*$")))
            {
                return false;
            }

            int name;
            bool crtTime = !string.IsNullOrEmpty(Doctor.Visitingtime);
            bool crtnam = !string.IsNullOrWhiteSpace(Doctor.Doctorname) && !(int.TryParse(Doctor.Doctorname,out name)) && !Doctor.Doctorname.All(sp => !char.IsLetter(sp));
            DateTime visitingTime;
            if (DateTime.TryParse(Doctor.Visitingtime, out visitingTime))
            {
                Regex regex = new Regex("([01]?[0-9]|2[0-3]):[0-5][0-9]");
                var match = regex.Match(visitingTime.ToString()).Success;

                if (!match)
                {
                   return false;
                }
            }

            if (crtnam && crtid && crtTime)
            {
                result = true;
            }
            return result;
        }

        private DepartmentModel department;
        public DepartmentModel Department
        {
            get { return department; }
            set { department = value; OnPropertyChanged(); }
        }

        private DepartmentModel _selecteddepartment;
        public DepartmentModel SelectedDepartment
        {
            get { return _selecteddepartment; }
            set { _selecteddepartment = value; OnPropertyChanged(); }
        }

        private List<DepartmentModel> _departments;
        public List<DepartmentModel> Departments
        {
            get { return _departments; }
            set { _departments = value; OnPropertyChanged(); }
        }

        private DoctorModel _doctor;
        public DoctorModel Doctor
        {
            get { return _doctor; }
            set
            {
                _doctor = value;
                OnPropertyChanged();
            }
        }
    }
}
