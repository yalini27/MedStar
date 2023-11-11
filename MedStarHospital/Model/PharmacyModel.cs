using MedStarHospital.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Ribbon.Primitives;

namespace MedStarHospital.Model
{
    public class PharmacyModel : ViewModelBase
    {
		private string _medicineid;

		public string MedicineID
		{
			get { return _medicineid; }
			set { _medicineid = value;OnPropertyChanged(); }
		}

		private string _medicinename;

		public string MedicineName
		{
			get { return _medicinename; }
			set { _medicinename = value;OnPropertyChanged(); }
		}

		private string _categary;

		public string Categary
		{
			get { return _categary; }
			set { _categary = value;OnPropertyChanged(); }
		}

		private decimal _unitprice;

		public decimal UnitPrice
		{
			get { return _unitprice; }
			set { _unitprice = value; OnPropertyChanged(); }
		}

		private int _quantity;

		public int Quantity
		{
			get { return _quantity; }
			set { _quantity = value; OnPropertyChanged(); }
		}

	}
}
