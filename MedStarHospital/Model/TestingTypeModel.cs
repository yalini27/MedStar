using MedStarHospital.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedStarHospital.Model
{
    public class TestingTypeModel : ViewModelBase
    {
		private int _testingTypeid;

		public int TestingTypeID
		{
			get { return _testingTypeid; }
			set { _testingTypeid = value; OnPropertyChanged(); }
		}

		private string _testingTypename;

		public string TestingTypeName
		{
			get { return _testingTypename; }
			set { _testingTypename = value; OnPropertyChanged();

                if (string.IsNullOrEmpty(value))
                    TypeNameError = "Name Cannot be empty";
                else if (value.Any(v => Char.IsDigit(v)))
                    TypeNameError = "Name must be Text only";
                else if (TestingTypeName.All(sp => !char.IsLetter(sp)))
                    TypeNameError = "Name must be Text only";
                else
                    TypeNameError = "";
            }
		}

		private decimal _amount;

		public decimal Amount
		{
			get { return _amount; }
			set { _amount = value;OnPropertyChanged(); }
		}

		private bool _isChecked;

		public bool IsChecked
		{
			get { return _isChecked; }
			set { _isChecked = value;OnPropertyChanged(); }
		}


		private string _typenameerror;

        public string TypeNameError
        {
            get { return _typenameerror; }
            set { _typenameerror = value; OnPropertyChanged(); }
        }
    }
}
