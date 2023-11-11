using MedStarHospital.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedStarHospital.Model
{
    public class BillDesignModel : ViewModelBase
    {
		private PatientModel _patient;

		public PatientModel Patient
		{
			get { return _patient; }
			set { _patient = value; OnPropertyChanged(); }
		}
		private ObservableCollection<TestingTypeModel> _testingcollection;

		public ObservableCollection<TestingTypeModel> TestingCollection
		{
			get { return _testingcollection; }
			set { _testingcollection = value; OnPropertyChanged(); }
		}



	}
}
