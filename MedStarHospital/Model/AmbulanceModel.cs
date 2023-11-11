using MedStarHospital.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MedStarHospital.Model
{
    public class AmbulanceModel : ViewModelBase
    {
		private int _ambulanceid;

		public int AmbulanceID
		{
			get { return _ambulanceid; }
			set { _ambulanceid = value; OnPropertyChanged(); }
		}

		private string _drivername;

		public string DriverName
		{
			get { return _drivername; }
			set { _drivername = value;OnPropertyChanged(); }
		}

		private bool _activestatus;

		public bool ActiveStatus
		{
			get { return _activestatus; }
			set { _activestatus = value;OnPropertyChanged(); }
		}

		private string _ambulanceNumber;

		public string AmbulanceNumber
		{
			get { return _ambulanceNumber; }
			set { _ambulanceNumber = value;OnPropertyChanged(); }
		}



		public Brush SliceColor { get; set; }

        private string _activedata;

        public string ActiveData
        {
            get { return _activedata; }
            set
            {
                _activedata = value; OnPropertyChanged();

            }
        }


		private bool _active;

		public bool Active
		{
			get { return _active; }
			set { _active = value; OnPropertyChanged(); }
		}

		private bool _inactive;

		public bool Inactive
		{
			get { return _inactive; }
			set { _inactive = value; OnPropertyChanged(); }
		}

	}
}
