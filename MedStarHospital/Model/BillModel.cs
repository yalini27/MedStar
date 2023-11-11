using MedStarHospital.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedStarHospital.Model
{
    public class BillModel : ViewModelBase
    {
        private int _billno;

        private PatientModel _patient;
        private double _amount;

        public int Billno
        {
            get { return _billno; }
            set { _billno = value; OnPropertyChanged(); }
        }
        
        public PatientModel Patient
        {
            get { return _patient; }
            set { _patient = value; OnPropertyChanged(); }
        }

        private TestingTypeModel _testingtype;

        public TestingTypeModel TestingType
        {
            get { return _testingtype; }
            set { _testingtype = value;OnPropertyChanged(); }
        }


        public double Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                OnPropertyChanged();
                double amo;
                if (Amount < 0)
                    AmountError = "Enter the positive value";
                //else if (!double.TryParse(Amount, out amo))
                //    AmountError = "Enter the correct amount";
                else
                    AmountError = "";
            }
        }

        private string _amounterror;

        public string AmountError
        {
            get { return _amounterror; }
            set { _amounterror = value; OnPropertyChanged(); }
        }

    }
}
