using MedStarHospital.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MedStarHospital.Model
{
    public class TestingBillModel  : ViewModelBase
    {
		private PatientModel _patient;

		public PatientModel Patient
		{
			get { return _patient; }
			set { _patient = value; OnPropertyChanged();}
		}

	}
}
