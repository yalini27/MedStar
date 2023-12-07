using MedStarHospital.Model;
using MedStarHospital.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MedStarHospital.ViewModel
{
    public class VMBill : ViewModelBase
    {
        private string _find;
        public string Find
        {
            get { return _find; }
            set { _find = value; OnPropertyChanged(); AutoApply(); }
        }

        private string _field = "Payment ID";
        public string Field { get { return _field; } set { _field = value; OnPropertyChanged(); } }

        private string _column;
        public string Column
        {
            get { return _column; }
            set { _column = value; OnPropertyChanged(); }
        }

        public VMBill()
        {
            //Bill.Amount = 2300;

            fnView();
            AppEvents.RefreshBill += OnBillRefresh;
        }

        void OnBillRefresh()
        {
            fnView();
        }
        //public ICommand cmdAddBill { get { return new RelayCommand(fnAdd); } }


        public ICommand cmdReset { get { return new RelayCommand(fnReset); } }

        void AutoApply()
        {
            switch (Field)
            {
                case "Payment ID":
                    Column = "PaymentID";
                    break;

                case "Patient ID":
                    Column = "PatientID";
                    break;

                case "Amount":
                    Column = "Amount";
                    break;

                default:
                    MessageBox.Show("Invalid Option");
                    break;
            }


                PaymentList = new ObservableCollection<PaymentModel>();
                Sql_Connection.sql_connection();
                string QUERY = $"select * from tblPayment where {Column} Like '%" + Find + "%'";
                SqlCommand command = new SqlCommand(QUERY, Sql_Connection.getconnection());
                var reader = command.ExecuteReader();

            while (reader.Read())
            {
                PaymentList.Add(new PaymentModel
                {
                    PaymentID = int.Parse(reader.GetValue(0).ToString()),


                    Patient = new PatientModel
                    {
                        PatientId = int.Parse(reader.GetValue(1).ToString()),
                        PatientName = Sql_Connection.SpaficDataISINTable("tblMAppointment", "PatientName", "PatientID", reader.GetValue(1).ToString())
                    },
                    Amount = double.Parse(reader.GetValue(2).ToString()),
                    Paid = decimal.Parse(reader.GetValue(3).ToString()),
                    Balance = decimal.Parse(reader.GetValue(4).ToString())

                });
            }

            Sql_Connection.close_connection();
        }
        void fnReset(object param)
        {
            fnView();
            
        }

        void fnView()
        {
            PaymentList = new ObservableCollection<PaymentModel>();
            Sql_Connection.DBConnection();
            Sql_Connection.sql_connection();

            var SerchQuary = $"select * from tblPayment";
            var Command = new SqlCommand(SerchQuary, Sql_Connection.getconnection());
            var reader = Command.ExecuteReader();
            while (reader.Read())
            {
                PaymentList.Add(new PaymentModel
                {
                    PaymentID = int.Parse(reader.GetValue(0).ToString()),

                    
                    Patient = new PatientModel
                    {
                        PatientId = int.Parse(reader.GetValue(1).ToString()),
                        PatientName = Sql_Connection.SpaficDataISINTable("tblMAppointment", "PatientName", "PatientID", reader.GetValue(1).ToString())
                    },
                    Amount = double.Parse(reader.GetValue(2).ToString()),
                    Paid = decimal.Parse(reader.GetValue(3).ToString()),
                    Balance = decimal.Parse(reader.GetValue(4).ToString())

                });
            }
            Sql_Connection.close_connection();
        }

       

        private PaymentModel _bill;
        public PaymentModel Bill
        {
            get { return _bill; }
            set { _bill = value; OnPropertyChanged(); }
        }
        private ObservableCollection<PaymentModel> _paymentlist;
        public ObservableCollection<PaymentModel> PaymentList
        {
            get { return _paymentlist; }
            set { _paymentlist = value; OnPropertyChanged(); }
        }
    }
}
