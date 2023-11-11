using MedStarHospital.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedStarHospital.Model
{
    public class DepartmentModel:ViewModelBase
    {
        private int _departmentid;
        private string _departmentname;
        //private DoctorModel _doctor;
  

        public int DepartmentID
        { get { return _departmentid; } set { _departmentid = value; OnPropertyChanged(); } }

        public string DepartmentName
        {
            get { return _departmentname; }
            set { _departmentname = value; OnPropertyChanged();
                if (string.IsNullOrEmpty(value))
                    DepNameError = "Name Cannot be empty";
                else if (value.Any(v => Char.IsDigit(v)))
                    DepNameError = "Name must be Text only";
                else if (DepartmentName.All(sp => !char.IsLetter(sp)))
                    DepNameError = "Name must be Text only";
                else
                    DepNameError = "";
            }
        }

        //public DoctorModel Doctor
        //{
        //    get { return _doctor; }
        //    set { _doctor = value; OnPropertyChanged(); }
        //}

        private string _depnameerror;

        public string DepNameError
        {
            get { return _depnameerror; }
            set { _depnameerror = value; OnPropertyChanged(); }
        }




    }
}
