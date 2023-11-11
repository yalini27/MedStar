using MedStarHospital.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedStarHospital.Model
{
    public class PaymentModel :ViewModelBase
    {
        private int _paymentid;

        public int PaymentID
        {
            get { return _paymentid; }
            set { _paymentid = value; OnPropertyChanged(); }
        }

        private PatientModel _patient;

        public PatientModel Patient
        {
            get { return _patient; }
            set { _patient = value; OnPropertyChanged(); }
        }

        private int _extradays;

        public int ExtraDays
        {
            get { return _extradays; }
            set { _extradays = value; OnPropertyChanged(); }
        }

        private double? _amount;

        public double? Amount
        {
            get { return _amount; }
            set { _amount = value; OnPropertyChanged(); }
        }
        //private int _fineamount;

        //      public int FineAmount
        //      {
        //          get { return _fineamount; }
        //          set { _fineamount = value; OnPropertyChanged(); }
        //      }


        private decimal _paid;

        public decimal Paid
        {
            get { return _paid; }
            set { _paid = value; OnPropertyChanged(); }
        }

        private decimal _balance;

        public decimal Balance
        {
            get { return _balance; }
            set { _balance = value; OnPropertyChanged(); }
        }

    }
}
