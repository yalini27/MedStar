using MedStarHospital.Model;
using MedStarHospital.View;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace MedStarHospital.ViewModel
{
    public class VMDoctor : ViewModelBase
    {
        public ICommand cmdEdit { get { return new RelayCommand(fnEdit, fnCanExecuteUser); } }

        private string _find;
        public string Find
        {
            get { return _find; }
            set { _find = value; OnPropertyChanged(); AutoApply(); }
        }

        private string _field = "Doctor ID";
        public string Field { get { return _field; } set { _field = value; OnPropertyChanged(); } }

        private string _column;
        public string Column
        {
            get { return _column; }
            set { _column = value; OnPropertyChanged(); }
        }

       
        public VMDoctor(UserModel user)
        {
            User = user;
            fnView();
            AppEvents.RefreshDoctor += OnRefreshDoctor;
        }
        void OnRefreshDoctor()
        {
            fnView();
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

        public ICommand cmdAdd { get { return new RelayCommand(fnAdd, fnCanExecuteUser); } }
        public ICommand cmdDelete { get { return new RelayCommand(fnDelete, fnCanExecuteUser); } }

        public ICommand cmdReset { get { return new RelayCommand(fnReset); } }

        void fnAdd(object param)
        {
            AddDoctorView dv = new AddDoctorView();
            dv.DataContext = new VMAddDoctor();
            dv.ShowDialog();
        }

        void fnEdit(object param)
        {
            DoctorModel db = (DoctorModel)param;
            AddDoctorView addDoctor = new AddDoctorView();
            addDoctor.DataContext = new VMAddDoctor(db);
            addDoctor.ShowDialog();
        }
       
        void AutoApply()
        {
            switch (Field)
            {
                case "Doctor ID":
                    Column = "DoctorID";
                    break;

                case "Doctor Name":
                    Column = "DoctorName";
                    break;

                case "Visiting Time":
                    Column = "VisitingTime";
                    break;
                case "Department ID":
                    Column = "DepID";
                    break;

                default:
                    MessageBox.Show("Invalid Option");
                    break;
            }


                Doctors = new List<DoctorModel>();
                Sql_Connection.sql_connection();
                string QUERY = $"select * from tblDoctor where {Column} Like '%" + Find + "%'";
                SqlCommand command = new SqlCommand(QUERY, Sql_Connection.getconnection());
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Doctors.Add(new DoctorModel
                    {
                        Doctorid = (int)reader.GetValue(0),
                        Doctorname = (reader.GetValue(1).ToString()),
                        Visitingtime = reader.GetValue(3).ToString(),
                        Department = new DepartmentModel
                        {
                            DepartmentID = (int)reader.GetValue(2),
                            DepartmentName = Sql_Connection.SpaficDataISINTable("tblDepartment", "DepartmentName", "DepartmentID", reader.GetValue(2).ToString())
                        },
                        PhoneNumber = reader.GetValue(4).ToString(),
                        Qualification = reader.GetValue(5).ToString(),
                        Address = reader.GetValue(6).ToString()
                    });
                }


            Sql_Connection.close_connection();

        }

        void fnReset(object param)
        {
            Doctors = new List<DoctorModel>();
            Sql_Connection.sql_connection();
            string Query = $"select *  from tblDoctor ";
            SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Doctors.Add(new DoctorModel
                {
                    Doctorid = (int)reader.GetValue(0),
                    Doctorname = (reader.GetValue(1).ToString()),
                    Visitingtime = reader.GetValue(3).ToString(),
                    Department = new DepartmentModel
                    {
                        DepartmentID = (int)reader.GetValue(2),
                        DepartmentName = Sql_Connection.SpaficDataISINTable("tblDepartment", "DepartmentName", "DepartmentID", reader.GetValue(2).ToString())
                    },
                    PhoneNumber = reader.GetValue(4).ToString(),
                    Qualification = reader.GetValue(5).ToString(),
                    Address = reader.GetValue(6).ToString()
                });
            }

            Sql_Connection.close_connection();
        }

        void fnView()
        {

            Doctors = new List<DoctorModel>();
            Sql_Connection.DBConnection();
            Sql_Connection.sql_connection();

          
            var SerchQuary = $"select *  from tblDoctor ";

            var Command = new SqlCommand(SerchQuary, Sql_Connection.getconnection());
            var reader = Command.ExecuteReader();
            while (reader.Read())
            {
                Doctors.Add(new DoctorModel
                {
                    Doctorid = (int)reader.GetValue(0),
                    Doctorname = (reader.GetValue(1).ToString()),
                    Visitingtime = reader.GetValue(3).ToString(),
                    Department = new DepartmentModel
                    {
                        DepartmentID = (int)reader.GetValue(2),
                        DepartmentName = Sql_Connection.SpaficDataISINTable("tblDepartment", "DepartmentName", "DepartmentID", reader.GetValue(2).ToString())
                    },
                    PhoneNumber = reader.GetValue(4).ToString(),
                    Qualification = reader.GetValue(5).ToString(),
                    Address = reader.GetValue(6).ToString()
                });
            }

            Sql_Connection.close_connection();
        }
        public static Action exit;
        void fnDelete(object param)
        {
            
            DoctorModel bv = param as DoctorModel;
            try
            {
                if (MessageBox.Show("Do you want to delete this doctor?","Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                { 
                    Sql_Connection.sql_connection();
                    string QUERY = $"delete from tblDoctor where DoctorID = {bv.Doctorid}";
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
                MessageBox.Show("This Doctor already Booked. So you can't delete", "delete Doctor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private DoctorModel _doctor;
        public DoctorModel Doctor
        {
            get { return _doctor; }
            set { _doctor = value; OnPropertyChanged(); }
        }
        private List<DoctorModel> _doctormoddel;


        public List<DoctorModel> Doctors
        {
            get { return _doctormoddel; }
            set { _doctormoddel = value; OnPropertyChanged(); }
        }
    }
}
