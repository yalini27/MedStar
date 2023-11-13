using MedStarHospital.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MedStarHospital.ViewModel
{
    public class VMPayment : ViewModelBase
    {

        public VMPayment()
        {

        }


        public ICommand cmdcalculate { get { return new RelayCommand(fncalculate); } }
        public ICommand cmdExit { get { return new RelayCommand(fnExit); } }

        void fnExit(object param)
        {
            exit.Invoke();
        }

        void fncalculate(object param)
        {
            //if(Patient.WithDoc)
            {
                if (Payment.Paid < Patient.Amount)
                {
                    MessageBox.Show("Enter the correct value...!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Payment.Balance = Payment.Paid - (decimal)Patient.Amount;
                }
            }
            //else
            //{
            //    if (Payment.Paid < Patient.Amount)
            //    {
            //        MessageBox.Show("Enter the correct value...!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //    else
            //    {
            //        Payment.Balance = Payment.Paid - (double)Patient.Amount;
            //    }
            //}
            //if(Patient.WithoutDoc)
            //{

            //    if (Payment.Paid < Patient.Amount)
            //    {
            //        MessageBox.Show("Enter the correct value...!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //    else
            //    {
            //        Payment.Balance = Payment.Paid - (double)Patient.Amount;
            //    }
            //}
            //if (Payment.Paid < Payment.Amount)
            //{
            //    MessageBox.Show("Enter the correct value...!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //else
            //{
            //    Payment.Balance = Payment.Paid - (double)Payment.Amount;
            //}

        }
        public ICommand cmdAdd { get { return new RelayCommand(fnAdd); } }
        public VMPayment(PatientModel patient, bool withDoc,bool WithoutDoc)
        {
            Patient.WithDoc = withDoc;
            Patient.WithoutDoc = WithoutDoc;
            ISData();
            fnloadTestingType(patient);
            SelectedPatient = new PatientModel();
            Patient.PatientId = patient.PatientId;
            Patient.Amount = patient.Amount;
            if (Patient.WithDoc)
            {
                Patient.Amount = patient.Amount = 2000;
            }
            //else
            //{
            //    foreach (var item in TestingTypeList.Where(t => t.IsChecked == true))
            //    {
            //        Patient.Amount = patient.TestingType.Amount;
            //    }
            //}
            if (Patient.WithoutDoc)
            {
                foreach (var item in TestingTypeList)
                {
                    Patient.Amount += item.Amount;
                }
            }
        }
        void ISData()
        {
            Payment = new();

            //Payment = new();
            Payment.Patient = new PatientModel();
            Payment.Patient.TestingType = new TestingTypeModel();
            //Payment.Member = new MemberModel();
            Payment.PaymentID = Sql_Connection.IsData("tblPayment") ? int.Parse(Sql_Connection.SpaficDataISINTable("tblPayment", "PaymentID", "PaymentID")) + 1 : 1;
        }

        void fnloadTestingType(PatientModel patient)
        {
            if(patient.Department.DepartmentID <= 0)
            {
                Sql_Connection.sql_connection();
                TestingTypeList = new ObservableCollection<TestingTypeModel>();
                string Query = $"select tt.* from tblMWithout_doctor wd join tblTestingType TT ON wd.TestingTypeID = tt.TestingTypeID where wd.BookingID = {patient.PatientId}";
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
        }

        public static Action exit;
        void fnAdd(object param)
        {
            if (validation())
            {
                try
                {
                    var amount = Patient.Amount;
                    Sql_Connection.sql_connection();
                    string Query = $"insert into tblPayment values('" + Payment.PaymentID + "','" + Patient.PatientId + "','" + Patient.Amount + "','" + Payment.Paid + "','" + Payment.Balance + "')";
                    SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.InsertCommand = new SqlCommand(Query, Sql_Connection.getconnection());
                    adapter.InsertCommand.ExecuteNonQuery();
                    //adapter.Dispose();
                    //command.Dispose();
                    //Sql_Connection.close_connection();
                    MessageBox.Show("Successfully added", "Add", MessageBoxButton.OK, MessageBoxImage.Information);
                    exit.Invoke();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                //BorrowModel bm = param as BorrowModel;
                //Sql_Connection.sql_connection();
                //string Query = $"insert into tblPayment(PaymentID,Borrowid,NoOfDays,Amount,paid,balance) values('"+Payment.PaymentID+"','"+ selectedBorrow.Borrowid+"','"+Payment.ExtraDays+"','"+Payment.FineAmount+"','"+Payment.Paid+"','"+Payment.Balance+"')";
                //SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
                //SqlDataAdapter adapter = new SqlDataAdapter();
                //adapter.InsertCommand = new SqlCommand(Query, Sql_Connection.getconnection());
                //adapter.InsertCommand.ExecuteNonQuery();
                //adapter.Dispose();
                //command.Dispose();
                //Sql_Connection.close_connection();
                //MessageBox.Show("Successfully added", "Add", MessageBoxButton.OK, MessageBoxImage.Information);
                //exit.Invoke();
            }
            else
            {
                MessageBox.Show("Enter the correct value", "error", MessageBoxButton.OK, MessageBoxImage.Error);
            };

        }

        private PaymentModel _payment;

        public PaymentModel Payment
        {
            get { return _payment; }
            set { _payment = value; OnPropertyChanged(); }
        }

        private PatientModel _patient = new PatientModel();

        public PatientModel Patient
        {
            get { return _patient; }
            set { _patient = value; OnPropertyChanged(); }
        }

        private PatientModel _selectedpatient;

        public PatientModel SelectedPatient
        {
            get { return _selectedpatient; }
            set { _selectedpatient = value;OnPropertyChanged(); }
        }


        public bool validation()
        {
            bool result = false;
            double amount;
            //bool crtamount = !string.IsNullOrWhiteSpace(Payment.Paid.ToString()) && (double.TryParse(Payment.Paid.ToString(), out amount)) && !Payment.Paid.ToString().All(sp => !char.IsLetter(sp));
            bool crtamount = double.TryParse(Payment.Paid.ToString(), out amount);


            if (crtamount)
            {
                result = true;

            }
            return result;
        }

        private ObservableCollection<TestingTypeModel> _testingtypelist;
        public ObservableCollection<TestingTypeModel> TestingTypeList
        {
            get { return _testingtypelist; }
            set { _testingtypelist = value; OnPropertyChanged(); }
        }
        //private PaymentModel _payment;

        //public PaymentModel Payment
        //{
        //    get { return _payment; }
        //    set { _payment = value; OnPropertyChanged(); }
        //}

        //private PaymentModel _payment;

        //public PaymentModel Payment
        //{
        //    get { return _payment; }
        //    set { _payment = value; OnPropertyChanged(); }
        //}
    }
}
