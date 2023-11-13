using MedStarHospital.Model;
using MedStarHospital.View;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace MedStarHospital.ViewModel
{
    public class VMPatient : ViewModelBase
    {
        private string _find;
        public string Find
        {
            get { return _find; }
            set { _find = value; OnPropertyChanged(); AutoApply(); }
        }

        private DateTime _finddate;

        public DateTime FindDate
        {
            get { return _finddate; }
            set { _finddate = value; OnPropertyChanged(); }
        }


        private string _field = "Patient ID";
        public string Field { get { return _field; } set { _field = value; OnPropertyChanged(); } }

        private string _column;
        public string Column
        {
            get { return _column; }
            set { _column = value; OnPropertyChanged(); }
        }


        public VMPatient(UserModel user)
        {
            User = user;
            FindDate = DateTime.Now;
            fnView();
            AppEvents.RefreshPatient += OnPatientRefresh;
            WithDoctor = "Visible";
            WithoutDoctor = "Hidden";
            WithDoc = true;
            Withoutdoc = false;

           
        }

        private UserModel userr;

        public UserModel User
        {
            get { return userr; }
            set { userr = value; OnPropertyChanged(); }
        }


        bool fnCanExecuteUser(object o)
        {
            if (User.Role.ToLower() == "doctor" && User.Role.ToLower() == "admin")
                return true;

            return true;
        }

        void OnPatientRefresh()
        {
            fnView();
        }


        public ICommand cmdAdd { get { return new RelayCommand(fnAdd, fnCanExecuteUser); } }

        public ICommand cmdApply { get { return new RelayCommand(fnApply); } }
        public ICommand cmdReset { get { return new RelayCommand(fnReset); } }
        public ICommand cmdEdit { get { return new RelayCommand(fnEdit, fnCanExecuteUser); } }


        void fnEdit(object param) 
        {

            PatientModel pm = (PatientModel)param;
            pm.WithoutDoc = Withoutdoc;
            pm.WithDoc = WithDoc;
            AddPatientView addPatient = new AddPatientView(pm, WithDoc);
            addPatient.ShowDialog();

        }

        void fnAdd(object param)
        {
            AddPatientView pv = new AddPatientView();
            pv.ShowDialog();
        }

        void fnApply(object param)
        {
            if (WithDoc)
            {
                Patients = new ObservableCollection<PatientModel>();
                Sql_Connection.sql_connection();

                var SerchQuary1 = $"select pat.PatientID, pat.PatientName, pat.PhoneNo, pat.BookingDate, pat.BookingDepID, pat.BookingDocID from tblMAppointment pat left join tblMWithout_doctor withoutdoc on pat.PatientID = withoutdoc.BookingID where pat.BookingDepID is not null and BookingDate = '" + FindDate.ToString("yyyy-MM-dd ") + "'";
               
                var Command1 = new SqlCommand(SerchQuary1, Sql_Connection.getconnection());
                var reader1 = Command1.ExecuteReader();
                while (reader1.Read())
                {
                    Patients.Add(new PatientModel
                    {
                        PatientId = int.Parse(reader1.GetValue(0).ToString()),
                        PatientName = reader1.GetValue(1).ToString(),
                        Phonenumber = reader1.GetValue(2).ToString(),
                        Bookingdate = DateTime.Parse(reader1.GetValue(3).ToString()),
                        Department = new DepartmentModel
                        {
                            DepartmentID = int.Parse(nullvalue(reader1.GetValue(4).ToString())),
                            DepartmentName = nullvalue(Sql_Connection.SpaficDataISINTable("tblDepartment", "DepartmentName", "DepartmentID", reader1.GetValue(4).ToString())),
                        },
                        Doctor = new DoctorModel
                        {
                            Doctorid = int.Parse(nullvalue(reader1.GetValue(5).ToString())),
                            Doctorname = nullvalue(Sql_Connection.SpaficDataISINTable("tblDoctor", "DoctorName", "DoctorID", reader1.GetValue(5).ToString())),
                            Visitingtime = nullvalue(Sql_Connection.SpaficDataISINTable("tblDoctor", "VisitingTime", "DoctorID", reader1.GetValue(5).ToString()))
                        },
                    });
                }
                Sql_Connection.close_connection();
            }
            if (Withoutdoc)
            {
                Patients = new ObservableCollection<PatientModel>();
                Sql_Connection.sql_connection();

                var SerchQuary1 = $"select pat.PatientID, pat.PatientName, pat.PhoneNo, pat.BookingDate, withoutdoc.TestingTypeID, withoutdoc.ResultDate  from tblMAppointment pat join tblMWithout_doctor withoutdoc on pat.PatientID = withoutdoc.BookingID where BookingDate = '" + FindDate.ToString("yyyy-MM-dd") + "'";
               
                var Command1 = new SqlCommand(SerchQuary1, Sql_Connection.getconnection());
                var reader1 = Command1.ExecuteReader();
                while (reader1.Read())
                {
                    Patients.Add(new PatientModel
                    {
                        PatientId = int.Parse(reader1.GetValue(0).ToString()),
                        PatientName = (reader1.GetValue(1).ToString()),
                        Phonenumber = reader1.GetValue(2).ToString(),
                        Bookingdate = DateTime.Parse(reader1.GetValue(3).ToString()),

                        TestingType = new TestingTypeModel
                        {
                            TestingTypeID = int.Parse(reader1.GetValue(4).ToString()),
                            TestingTypeName = Sql_Connection.SpaficDataISINTable("tblTestingType", "TypeName", "TestingTypeID", reader1.GetValue(4).ToString())
                        },
                        ResultDate = DateTime.Parse(nullvalue(reader1.GetValue(5).ToString()))
                    });
                }
                Sql_Connection.close_connection();
            }
           
        }

        void AutoApply()
        {
            if (WithDoc)
            {
                switch (Field)
                {
                    case "Patient ID":
                        Column = "PatientID";
                        break;

                    case "Patient Name":
                        Column = "PatientName";
                        break;


                    case "Department ID":
                        Column = "BookingDepID";
                        break;

                    case "Doctor ID":
                        Column = "BookingDocID";
                        break;


                    default:
                        MessageBox.Show("Invalid Option");
                        break;
                }
          
                Patients = new ObservableCollection<PatientModel>();
                Sql_Connection.sql_connection();

                
                var SerchQuary1 = $"select pat.PatientID, pat.PatientName, pat.PhoneNo, pat.BookingDate, pat.BookingDepID, pat.BookingDocID from tblMAppointment pat left join tblMWithout_doctor withoutdoc on pat.PatientID = withoutdoc.BookingID where pat.BookingDepID is not null and {Column} Like '%" + Find + "%'";
             
                var Command1 = new SqlCommand(SerchQuary1, Sql_Connection.getconnection());
                var reader1 = Command1.ExecuteReader();
                while (reader1.Read())
                {
                    Patients.Add(new PatientModel
                    {
                        PatientId = int.Parse(reader1.GetValue(0).ToString()),
                        PatientName = reader1.GetValue(1).ToString(),
                        Phonenumber = reader1.GetValue(2).ToString(),
                        Bookingdate = DateTime.Parse(reader1.GetValue(3).ToString()),
                        Department = new DepartmentModel
                        {

                            DepartmentID = int.Parse((reader1.GetValue(4).ToString())),
                            DepartmentName = nullvalue(Sql_Connection.SpaficDataISINTable("tblDepartment", "DepartmentName", "DepartmentID", reader1.GetValue(4).ToString())),

                        },
                        Doctor = new DoctorModel
                        {
                            Doctorid = int.Parse(nullvalue(reader1.GetValue(5).ToString())),
                            Doctorname = nullvalue(Sql_Connection.SpaficDataISINTable("tblDoctor", "DoctorName", "DoctorID", reader1.GetValue(5).ToString())),
                            Visitingtime = nullvalue(Sql_Connection.SpaficDataISINTable("tblDoctor", "VisitingTime", "DoctorID", reader1.GetValue(5).ToString()))
                        },

                    });
                }
                Sql_Connection.close_connection();
            }
            if(Withoutdoc)
            {
                switch (Field)
                {
                    case "Patient ID":
                        Column = "PatientID";
                        break;

                    case "Patient Name":
                        Column = "PatientName";
                        break;

                    case "Test Type ID":
                        Column = "TestingTypeID";
                        break;

                    default:
                        MessageBox.Show("Invalid Option");
                        break;
                }
                Patients = new ObservableCollection<PatientModel>();
                Sql_Connection.sql_connection();

                var SerchQuary1 = $"select pat.PatientID, pat.PatientName, pat.PhoneNo, pat.BookingDate, withoutdoc.TestingTypeID, withoutdoc.ResultDate  from tblMAppointment pat join tblMWithout_doctor withoutdoc on pat.PatientID = withoutdoc.BookingID where {Column} Like '%" + Find + "%'";

                var Command1 = new SqlCommand(SerchQuary1, Sql_Connection.getconnection());
                var reader1 = Command1.ExecuteReader();
                while (reader1.Read())
                {
                    Patients.Add(new PatientModel
                    {
                        PatientId = int.Parse(reader1.GetValue(0).ToString()),
                        PatientName = (reader1.GetValue(1).ToString()),
                        Phonenumber = reader1.GetValue(2).ToString(),
                        Bookingdate = DateTime.Parse(reader1.GetValue(3).ToString()),

                        TestingType = new TestingTypeModel
                        {
                            TestingTypeID = int.Parse(reader1.GetValue(4).ToString()),
                            TestingTypeName = Sql_Connection.SpaficDataISINTable("tblTestingType", "TypeName", "TestingTypeID", reader1.GetValue(4).ToString())
                        },
                        ResultDate = DateTime.Parse(nullvalue(reader1.GetValue(5).ToString()))
                    });
                }
                Sql_Connection.close_connection();
            }
                   
        }

        void fnReset(object param)
        {
            //fnView();
            if (WithDoc)
            {
                Patients = new ObservableCollection<PatientModel>();

                Sql_Connection.DBConnection();
                Sql_Connection.sql_connection();

                var SerchQuary2 = $"select pat.PatientID, pat.PatientName, pat.PhoneNo, pat.BookingDate, pat.BookingDepID, pat.BookingDocID from tblMAppointment pat left join tblMWithout_doctor withoutdoc on pat.PatientID = withoutdoc.BookingID where pat.BookingDepID is not null";
                var Command2 = new SqlCommand(SerchQuary2, Sql_Connection.getconnection());
                var reader2 = Command2.ExecuteReader();
                while (reader2.Read())
                {
                    Patients.Add(new PatientModel
                    {
                        PatientId = int.Parse(reader2.GetValue(0).ToString()),
                        PatientName = reader2.GetValue(1).ToString(),
                        Phonenumber = reader2.GetValue(2).ToString(),
                        Bookingdate = DateTime.Parse(reader2.GetValue(3).ToString()),
                        Department = new DepartmentModel
                        {
                            DepartmentID = int.Parse(reader2.GetValue(4).ToString()),
                            DepartmentName = Sql_Connection.SpaficDataISINTable("tblDepartment", "DepartmentName", "DepartmentID", reader2.GetValue(4).ToString())
                        },
                        Doctor = new DoctorModel
                        {
                            Doctorid = int.Parse(reader2.GetValue(5).ToString()),
                            Doctorname = Sql_Connection.SpaficDataISINTable("tblDoctor", "DoctorName", "DoctorID", reader2.GetValue(5).ToString()),
                            Visitingtime = Sql_Connection.SpaficDataISINTable("tblDoctor", "VisitingTime", "DoctorID", reader2.GetValue(5).ToString())
                        }
                    });
                }
                Sql_Connection.close_connection();
            }
            if (Withoutdoc)
            {

                Patients = new ObservableCollection<PatientModel>();

                Sql_Connection.sql_connection();

                var SerchQuary1 = $"select pat.PatientID, pat.PatientName, pat.PhoneNo, pat.BookingDate, withoutdoc.TestingTypeID, withoutdoc.ResultDate  from tblMAppointment pat join tblMWithout_doctor withoutdoc on pat.PatientID = withoutdoc.BookingID";
                var Command1 = new SqlCommand(SerchQuary1, Sql_Connection.getconnection());
                var reader1 = Command1.ExecuteReader();
                while (reader1.Read())
                {
                    Patients.Add(new PatientModel
                    {
                        PatientId = int.Parse(reader1.GetValue(0).ToString()),
                        PatientName = (reader1.GetValue(1).ToString()),
                        Phonenumber = reader1.GetValue(2).ToString(),
                        Bookingdate = DateTime.Parse(reader1.GetValue(3).ToString()),

                        TestingType = new TestingTypeModel
                        {
                            TestingTypeID = int.Parse(nullvalue(reader1.GetValue(4).ToString())),
                            TestingTypeName = nullvalue(Sql_Connection.SpaficDataISINTable("tblTestingType", "TypeName", "TestingTypeID", reader1.GetValue(4).ToString()))
                        },
                        ResultDate = DateTime.Parse(nullvalue(reader1.GetValue(5).ToString()))
                    });
                }
                Sql_Connection.close_connection();
            }

        }

        void fnView()
        {
            if(WithDoc)
            {
                Patients = new ObservableCollection<PatientModel>();

                Sql_Connection.DBConnection();
                Sql_Connection.sql_connection();

                var SerchQuary = $"select pat.PatientID, pat.PatientName, pat.PhoneNo, pat.BookingDate, pat.BookingDepID, pat.BookingDocID from tblMAppointment pat left join tblMWithout_doctor withoutdoc on pat.PatientID = withoutdoc.BookingID where pat.BookingDepID is not null";
                var Command = new SqlCommand(SerchQuary, Sql_Connection.getconnection());
                var reader = Command.ExecuteReader();
                while (reader.Read())
                {
                    Patients.Add(new PatientModel
                    {
                        PatientId = int.Parse(reader.GetValue(0).ToString()),
                        PatientName = reader.GetValue(1).ToString(),
                        Phonenumber = reader.GetValue(2).ToString(),
                        Bookingdate = DateTime.Parse(reader.GetValue(3).ToString()),
                        Department = new DepartmentModel
                        {
                            DepartmentID = int.Parse(reader.GetValue(4).ToString()),
                            DepartmentName = Sql_Connection.SpaficDataISINTable("tblDepartment", "DepartmentName", "DepartmentID", reader.GetValue(4).ToString())
                        },
                        Doctor = new DoctorModel
                        {
                            Doctorid = int.Parse(reader.GetValue(5).ToString()),
                            Doctorname = Sql_Connection.SpaficDataISINTable("tblDoctor", "DoctorName", "DoctorID", reader.GetValue(5).ToString()),
                            Visitingtime = Sql_Connection.SpaficDataISINTable("tblDoctor", "VisitingTime", "DoctorID", reader.GetValue(5).ToString())
                        }
                    });
                    Sql_Connection.close_connection();
                }
            }
            if (Withoutdoc)
            {
                if (Withoutdoc)
                {
                    WithDoctor = "Hidden";
                    WithoutDoctor = "Visible";

                    Patients = new ObservableCollection<PatientModel>();

                    Sql_Connection.sql_connection();

                    var SerchQuary1 = $"select pat.PatientID, pat.PatientName, pat.PhoneNo, pat.BookingDate, withoutdoc.TestingTypeID, withoutdoc.ResultDate  from tblMAppointment pat join tblMWithout_doctor withoutdoc on pat.PatientID = withoutdoc.BookingID";
                    var Command1 = new SqlCommand(SerchQuary1, Sql_Connection.getconnection());
                    var reader1 = Command1.ExecuteReader();
                    while (reader1.Read())
                    {
                        Patients.Add(new PatientModel
                        {
                            PatientId = int.Parse(reader1.GetValue(0).ToString()),
                            PatientName = (reader1.GetValue(1).ToString()),
                            Phonenumber = reader1.GetValue(2).ToString(),
                            Bookingdate = DateTime.Parse(reader1.GetValue(3).ToString()),

                            TestingType = new TestingTypeModel
                            {
                                TestingTypeID = int.Parse(nullvalue(reader1.GetValue(4).ToString())),
                                TestingTypeName = nullvalue(Sql_Connection.SpaficDataISINTable("tblTestingType", "TypeName", "TestingTypeID", reader1.GetValue(4).ToString()))
                            },
                            ResultDate = DateTime.Parse(nullvalue(reader1.GetValue(5).ToString()))
                        });
                    }
                    Sql_Connection.close_connection();
                }
            }
          
        }
        
        private ObservableCollection<PatientModel> patientModels;
        public ObservableCollection<PatientModel> Patients
        {
            get { return patientModels; }
            set
            {
                patientModels = value;
                OnPropertyChanged();
            }
        }

        private PatientModel _patient;
        public PatientModel Patient
        {
            get { return _patient; }
            set
            {
                _patient = value;
                OnPropertyChanged();

            }
        }

        private bool _withdoc;

        public bool WithDoc
        {
            get { return _withdoc; }
            set
            {
                _withdoc = value; OnPropertyChanged();
                if (WithDoc)
                {
                    WithDoctor = "Visible";
                    WithoutDoctor = "Hidden";


                    Patients = new ObservableCollection<PatientModel>();
                    fnView();
                    
                }
            }
        }

        private bool _withoutdoc;

        public bool Withoutdoc
        {
            get { return _withoutdoc; }
            set
            {
                _withoutdoc = value; OnPropertyChanged();
                if (Withoutdoc)
                {
                    WithDoctor = "Hidden";
                    WithoutDoctor = "Visible";

                    Patients = new ObservableCollection<PatientModel>();
                   
                    Sql_Connection.sql_connection();

                    var SerchQuary1 = $"select pat.PatientID, pat.PatientName, pat.PhoneNo, pat.BookingDate, withoutdoc.TestingTypeID, withoutdoc.ResultDate  from tblMAppointment pat join tblMWithout_doctor withoutdoc on pat.PatientID = withoutdoc.BookingID";
                    var Command1 = new SqlCommand(SerchQuary1, Sql_Connection.getconnection());
                    var reader1 = Command1.ExecuteReader();
                    while (reader1.Read())
                    {
                        Patients.Add(new PatientModel
                        {
                            PatientId = int.Parse(reader1.GetValue(0).ToString()),
                            PatientName = (reader1.GetValue(1).ToString()),
                            Phonenumber = reader1.GetValue(2).ToString(),
                            Bookingdate = DateTime.Parse(reader1.GetValue(3).ToString()),

                            TestingType = new TestingTypeModel
                            {
                                TestingTypeID = int.Parse(nullvalue(reader1.GetValue(4).ToString())),
                                TestingTypeName = nullvalue(Sql_Connection.SpaficDataISINTable("tblTestingType", "TypeName", "TestingTypeID", reader1.GetValue(4).ToString()))
                            },
                            ResultDate = DateTime.Parse(nullvalue(reader1.GetValue(5).ToString()))
                        });
                    }
                    Sql_Connection.close_connection();

                }
            }
        }

        private string _withdoctor;

        public string WithDoctor
        {
            get { return _withdoctor; }
            set { _withdoctor = value; OnPropertyChanged(); }
        }

        private string _withoutdoctor;

        public string WithoutDoctor
        {
            get { return _withoutdoctor; }
            set { _withoutdoctor = value; OnPropertyChanged(); }
        }

        public string nullvalue(string value)
        {
            if(value!=string.Empty)
            {
                return value;
            }
            else
            {
                return null;
            }
        }
    }
}
