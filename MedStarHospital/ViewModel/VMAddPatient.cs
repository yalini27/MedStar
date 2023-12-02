using MedStarHospital.Model;
using MedStarHospital.View;
using Microsoft.VisualBasic.FileIO;
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
using System.Windows.Documents;
using System.Windows.Input;

namespace MedStarHospital.ViewModel
{
    public class VMAddPatient : ViewModelBase
    {
        
        private string _btnBack;
        public string btnBack
        {
            get { return _btnBack; }
            set { _btnBack = value; OnPropertyChanged(); }
        }

        private string _btn;

        public string btn
        {
            get { return _btn; }
            set { _btn = value; OnPropertyChanged(); }
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

        private bool _Withdoc;

        public bool WithDoc
        {
            get { return _Withdoc; }
            set
            {
                _Withdoc = value;
                OnPropertyChanged();
                if (WithDoc)
                {
                 
                    WithDoctor = "Visible";
                    WithoutDoctor = "Hidden";
                }
                else
                {
                    WithDoctor = "Hidden";
                    WithoutDoctor = "Visible";
                }
            }
        }

        private bool _Withoutdoc;

        public bool WithoutDoc
        {
            get { return _Withoutdoc; }
            set
            {
                _Withoutdoc = value;
                OnPropertyChanged();
               
            }
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

  
        void fngetDoctor()
        {
            Sql_Connection.sql_connection();
            Doctors = new ObservableCollection<DoctorModel>();
            string Query = $"Select * from tblDoctor where DepID = {SelectedDepartment.DepartmentID}";
            SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Doctors.Add(new DoctorModel
                {
                    Doctorid = (int)reader.GetValue(0),
                    Doctorname = (string)reader.GetValue(1),
                    Visitingtime = reader.GetValue(3).ToString()

                });
            }
            Sql_Connection.close_connection();
        }

        public VMAddPatient(PatientModel patient = null, bool withDoc = true)
        {
            WithDoc = withDoc;
            WithoutDoc = !withDoc;
            WithDoctor = withDoc ? "Visible" : "Hidden";

            fngetDepartment();
            SelectedDepartment = new DepartmentModel();
            fngetTestingType();
            SelectedTestingType = new TestingTypeModel();
            if (patient == null)
            {
                ISPatient();
                //ISTesting();
                btn = "ADD";
                btnBack = "CLOSE";
            }
            else
            {
                
                Patient = patient;
               

                if (WithDoc)
                {
                    
                    SelectedDepartment = Departments.FirstOrDefault(d => d.DepartmentID == patient.Department.DepartmentID);
                    SelectedDoctor = Doctors.FirstOrDefault(e => e.Doctorid == patient.Doctor.Doctorid);
                }
                if (WithoutDoc)
                {
                   
                    fngetselectedTestType();

                    SelectedTestingType = TestingTypeList.FirstOrDefault(d => d.TestingTypeID == patient.TestingType.TestingTypeID);
                    
                }

                btn = "UPDATE";
                btnBack = "CLOSE";
            }
            Patients = new ObservableCollection<PatientModel>();
        }
        public ICommand cmdOption { get { return new RelayCommand(fnOption); } }

        void fnOption(object param)
        {
          
            switch (btn)
            {
                case "ADD":
                    try
                    {

                        if (WithDoc)
                        {
                            if (validation())
                            {

                                Sql_Connection.sql_connection();
                                string QUERY = $"Insert into tblMAppointment values('" + Patient.PatientId + "','" + Patient.PatientName + "','" + Patient.Phonenumber + "','" + Patient.Bookingdate.ToString("yyyy-MM-dd") + "','" + SelectedDepartment.DepartmentID + "','" + SelectedDoctor.Doctorid + "')";
                                SqlCommand command = new SqlCommand(QUERY, Sql_Connection.getconnection());
                                SqlDataAdapter adapter = new SqlDataAdapter();
                                adapter.InsertCommand = new SqlCommand(QUERY, Sql_Connection.getconnection());
                                adapter.InsertCommand.ExecuteNonQuery();
                                adapter.Dispose();
                                command.Dispose();
                                Sql_Connection.close_connection();
                                MessageBox.Show("Successfully Added!", "Add Booking", MessageBoxButton.OK, MessageBoxImage.Information);
                                AddPaymentView ad = new AddPaymentView();
                                ad.DataContext = new VMPayment(Patient, WithDoc, WithoutDoc);
                                ad.ShowDialog();

                                Exit.Invoke();

                                BillMethod bm = new BillMethod();
                                bm.DataContext = new VMBillMethod(Patient);
                                bm.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Datas", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }

                      

                        if (WithoutDoc)
                        {
                            if (validation1())
                            {
                                Sql_Connection.sql_connection();
                                string QUERY1 = $"insert into tblMAppointment(PatientID,PatientName,PhoneNo,BookingDate) values('" + Patient.PatientId + "','" + Patient.PatientName + "','" + Patient.Phonenumber + "','" + Patient.Bookingdate.ToString("yyyy-MM-dd") + "')";
                                SqlCommand command1 = new SqlCommand(QUERY1, Sql_Connection.getconnection());
                                SqlDataAdapter adapter1 = new SqlDataAdapter();
                                adapter1.InsertCommand = new SqlCommand(QUERY1, Sql_Connection.getconnection());
                                adapter1.InsertCommand.ExecuteNonQuery();
                                adapter1.Dispose();
                                command1.Dispose();
                                Sql_Connection.close_connection();
                                if (WithoutDoc)
                                {
                                    Sql_Connection.sql_connection();

                                    foreach (var item in TestingTypeList.Where(t => t.IsChecked == true))
                                    {
                                        int pkid = Sql_Connection.GetPK("tblMWithout_doctor", "ID");
                                        string QUERY2 = $"Insert into tblMWithout_doctor values('" + pkid + "','" + Patient.PatientId + "','" + item.TestingTypeID + "','" + Patient.ResultDate.ToString("yyyy-MM-dd") + "')";
                                        SqlCommand command2 = new SqlCommand(QUERY2, Sql_Connection.getconnection());
                                        SqlDataAdapter adapter2 = new SqlDataAdapter();
                                        adapter2.InsertCommand = new SqlCommand(QUERY2, Sql_Connection.getconnection());
                                        adapter2.InsertCommand.ExecuteNonQuery();
                                        adapter2.Dispose();
                                        command2.Dispose();
                                    }
                                }

                                Sql_Connection.close_connection();
                                MessageBox.Show("Successfully Added!", "Add Booking", MessageBoxButton.OK, MessageBoxImage.Information);
                                AddPaymentView ad = new AddPaymentView();
                                ad.DataContext = new VMPayment(Patient, WithDoc, WithoutDoc);
                                ad.ShowDialog();
                                Exit.Invoke();

                                TestingBillDesign bm = new TestingBillDesign();
                                bm.DataContext = new VMBillMethod(Patient, WithoutDoc);
                                bm.ShowDialog();
                               
                            }
                            else
                            {
                                MessageBox.Show("Invalid Datas", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                
                            }

                        }
       
                    }
                    catch (Exception ex)
                    {
                        Sql_Connection.close_connection();
                        MessageBox.Show(ex.Message+"This Data invalid!", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    break;

                case "UPDATE":
                    try
                    {

                        if (WithDoc)
                        {
                            if (validation())
                            {
                                Sql_Connection.sql_connection();
                                string Query = $"update tblMAppointment set PatientName ='" + Patient.PatientName + "',PhoneNo='" + Patient.Phonenumber + "',BookingDate='" + Patient.Bookingdate.ToString("yyyy-MM-dd") + "',BookingDepID = '" + SelectedDepartment.DepartmentID + "', BookingDocID = '" + SelectedDoctor.Doctorid + "'where PatientID='" + Patient.PatientId + "'";
                                SqlCommand command1 = new SqlCommand(Query, Sql_Connection.getconnection());
                                SqlDataAdapter adapter1 = new SqlDataAdapter();
                                adapter1.UpdateCommand = new SqlCommand(Query, Sql_Connection.getconnection());
                                adapter1.UpdateCommand.ExecuteNonQuery();
                                adapter1.Dispose();
                                command1.Dispose();
                                Sql_Connection.close_connection();

                                MessageBox.Show("Update successfully", "update Booking", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("Invalid Datas", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                     

                        if (WithoutDoc)
                        {
                            if (validation1())
                            {
                                Sql_Connection.sql_connection();
                                string Query = $"update tblMAppointment set PatientName ='" + Patient.PatientName + "',PhoneNo='" + Patient.Phonenumber + "',BookingDate='" + Patient.Bookingdate.ToString("yyyy-MM-dd") + "'where PatientID='" + Patient.PatientId + "'";
                                SqlCommand command1 = new SqlCommand(Query, Sql_Connection.getconnection());
                                SqlDataAdapter adapter1 = new SqlDataAdapter();
                                adapter1.UpdateCommand = new SqlCommand(Query, Sql_Connection.getconnection());
                                adapter1.UpdateCommand.ExecuteNonQuery();
                                adapter1.Dispose();
                                command1.Dispose();
                                Sql_Connection.close_connection();
                                if (WithoutDoc)
                                {
                                    Sql_Connection.sql_connection();
                                    foreach (var item in TestingTypeList.Where(t => t.IsChecked == true))
                                    {
                                        var av = Patient.ColWithOutDoctor.FirstOrDefault(d => d.TestingTypeID == item.TestingTypeID);
                                        if (av == null) return;

                                        string Query1 = $"update tblMWithout_doctor set TestingTypeID ='" + item.TestingTypeID + "',ResultDate='" + Patient.ResultDate.ToString("yyyy-MM-dd") + "'where ID='" + av.ID + "'";
                                        SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
                                        SqlDataAdapter adapter = new SqlDataAdapter();
                                        adapter.UpdateCommand = new SqlCommand(Query1, Sql_Connection.getconnection());
                                        adapter.UpdateCommand.ExecuteNonQuery();
                                        adapter.Dispose();
                                        command.Dispose();
                                    }
                                    Sql_Connection.close_connection();
                                    MessageBox.Show("Update successfully", "update Booking", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                               
                            }
                            else
                            {
                                MessageBox.Show("Invalid Datas", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            Exit.Invoke();
                        }
     
                    }

                    catch
                    {
                        Sql_Connection.close_connection();
                        MessageBox.Show("You can't update", "update patient", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    break;
            }
            AppEvents.OnRefreshPatient();
     
        }


        private double amount;

        public double Amount
        {
            get { return amount; }
            set { amount = value; OnPropertyChanged(); }
        }

        public void ISPatient()
        {
            Patient = new();
            Patient.Department = new DepartmentModel();
            var id = Sql_Connection.SpaficDataISINTable("tblMAppointment", "PatientID", "PatientID");

            if (id == "")
            {
                Patient.PatientId = 1;
            }

            else
            {
                Patient.PatientId = Convert.ToInt32(id) + 1;
            }
        }


        public ICommand cmdExit { get { return new RelayCommand(fnExit); } }

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
                    TestingTypeName = (string)reader.GetValue(1),
                    Amount = (decimal)reader.GetValue(2)
                });
            }
            Sql_Connection.close_connection();


        }

        void fngetselectedTestType()
        {
  
            {
                Sql_Connection.sql_connection();
                Patient.ColWithOutDoctor = new ObservableCollection<WithoutDoctorModel>();
                string Query = $"select * from tblMWithout_doctor wd join tblTestingType TT ON wd.TestingTypeID = tt.TestingTypeID where wd.BookingID = {Patient.PatientId}";
                SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    foreach (var item in TestingTypeList.Where(d => d.TestingTypeID == (int)reader.GetValue(4)))
                    {
                        item.IsChecked = true;
                    }

                    Patient.ColWithOutDoctor.Add(new WithoutDoctorModel
                    {
                        ID = (int)reader.GetValue(0),
                        BookingID = (int)reader.GetValue(1),
                        TestingTypeID = (int)reader.GetValue(2),
                        ResultDate = (DateTime)reader.GetValue(3),

                    });

                }
                Sql_Connection.close_connection();

            }
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

        public static Action Exit;
        void fnExit(object param)
        {
            Exit.Invoke();
        }

        public bool validation()
        {
            
            bool result = false;

         

            bool crtid = Patient.PatientId > 0;
            int name;

            bool crtnam = !string.IsNullOrWhiteSpace(Patient.PatientName) && !(int.TryParse(Patient.PatientName, out name)) && !Patient.PatientName.All(sp => !char.IsLetter(sp));
            bool crtphone = !(string.IsNullOrWhiteSpace(Patient.Phonenumber) && (string.IsNullOrEmpty(Patient.Phonenumber)));
          
            bool crtdepid = !(SelectedDepartment.DepartmentID <= 0);
            
            bool crtdocid = crtdepid ? !(SelectedDoctor.Doctorid<=0):false;
         
            if (crtdepid & crtnam & crtphone & crtdocid)
            {
                return true;
            }
            DateTime Dates = DateTime.Now;
            DateTime BDate = Dates.AddDays(7);
            DateTime booking_date;
            if (!(Patient.Bookingdate.Date >= Dates.Date && Patient.Bookingdate <= BDate))
            {
                return false;
            }

            if ((!(Patient.Phonenumber != null) || (Patient.Phonenumber.Length == 9)))
            {
                return false;
            }

            if (!(Regex.IsMatch(Patient.PatientName, @"^[a-zA-Z\s]*$")))
            {
                return false;
            }




            return result;
        }

        public bool validation1()
        {
            

            DateTime Dates1 = Patient.Bookingdate;
            DateTime RDate = Dates1.AddDays(7);
            DateTime Result_date;
            if (!(Patient.ResultDate.Date >= Patient.Bookingdate.Date && Patient.ResultDate <= RDate))
            {
                return false;

            }
            bool result = false;

            bool crtid = Patient.PatientId > 0;
            int name;

            bool crtnam = !string.IsNullOrWhiteSpace(Patient.PatientName) && !(int.TryParse(Patient.PatientName, out name)) && !Patient.PatientName.All(sp => !char.IsLetter(sp));
            bool crtphone = !(string.IsNullOrWhiteSpace(Patient.Phonenumber) && (string.IsNullOrEmpty(Patient.Phonenumber)));

    

            DateTime Dates = DateTime.Now;
            DateTime BDate = Dates.AddDays(7);
            DateTime booking_date;
            if (!(Patient.Bookingdate.Date >= Dates.Date && Patient.Bookingdate <= BDate))
            {
                return false;
            }

            if ((!(Patient.Phonenumber != null) && (Patient.Phonenumber.Count(char.IsDigit) == 9)))
            {
                return false;
            }

            if (!(Regex.IsMatch(Patient.PatientName, @"^[a-zA-Z\s]*$")))
            {
                return false;
            }

            if (crtnam && crtid)
            {
                result = true;
            }
            return result;

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

        private DepartmentModel department;
        public DepartmentModel Department
        {
            get { return department; }
            set { department = value; OnPropertyChanged(); }
        }

        private DoctorModel _doctor;

        public DoctorModel Doctor
        {
            get { return _doctor; }
            set { _doctor = value; OnPropertyChanged(); }
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

        private ObservableCollection<DoctorModel> _doctors;
        public ObservableCollection<DoctorModel> Doctors
        {
            get { return _doctors; }
            set { _doctors = value; OnPropertyChanged(); }
        }

        private List<DepartmentModel> _departments;
        public List<DepartmentModel> Departments
        {
            get { return _departments; }
            set { _departments = value; OnPropertyChanged(); }
        }

        private DepartmentModel _selecteddepartment;
        public DepartmentModel SelectedDepartment
        {
            get { return _selecteddepartment; }
            set
            {
                _selecteddepartment = value; OnPropertyChanged();
                if (!string.IsNullOrEmpty(value.DepartmentName))
                {
                    fngetDoctor();
                    SelectedDoctor = new DoctorModel();
                }
            }
        }

        private DoctorModel _selecteddoctor;

        public DoctorModel SelectedDoctor
        {
            get { return _selecteddoctor; }
            set { _selecteddoctor = value; OnPropertyChanged(); }
        }


        private TestingBookingModel _testingbooking;
        public TestingBookingModel TestingBooking
        {
            get { return _testingbooking; }
            set
            {
                _testingbooking = value;
                OnPropertyChanged();
            }
        }


    }

}
