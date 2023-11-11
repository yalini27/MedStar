using MedStarHospital.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedStarHospital.Model
{
    public class PaitentBill : ViewModelBase
    {
        private int _paitentid;
        public int Paitentid 
        {
            get { return _paitentid; }
            set { _paitentid = value; OnPropertyChanged(); }
        }

        private string _paitentname;
        public string PaitentName
        {
            get { return _paitentname; }
            set { _paitentname = value; OnPropertyChanged(); }
        }

        private int? _depId;
        public int? DepId
        {
            get { return _depId; }
            set { _depId = value; OnPropertyChanged(); }
        }

        private int? _billID;
        public int? BillId
        {
            get { return _billID; }
            set { _billID = value; OnPropertyChanged(); }
        }
    }
}
