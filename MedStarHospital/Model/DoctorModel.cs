using MedStarHospital.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace MedStarHospital.Model
{
    public class DoctorModel : ViewModelBase
    {
        private int _doctorid;
        private string _doctorname;
        private string _visitingtime;
        private DepartmentModel _department;

        public int Doctorid
        {
            get { return _doctorid; }
            set { _doctorid = value; OnPropertyChanged(); }
        }


        public DepartmentModel Department
        {
            get { return _department; }
            set { _department = value; OnPropertyChanged(); }
        }

        public string Doctorname
        {
            get { return _doctorname; }
            set { _doctorname = value; OnPropertyChanged();
                if (string.IsNullOrEmpty(value))
                    DoctorNameError = "Name Cannot be empty";
                else if (value.Any(v => Char.IsDigit(v)))
                    DoctorNameError = "Name must be Text only";
                else if (Doctorname.All(sp => !char.IsLetter(sp)))
                    DoctorNameError = "Name must be Text only";
                else
                    DoctorNameError = "";
            }
        }


        public string Visitingtime
        {
            get { return _visitingtime; }
            set
            {
                _visitingtime = value;
                OnPropertyChanged();

                DateTime visitingTime;
                if (DateTime.TryParse(Visitingtime, out visitingTime))
                {

                    Regex regex = new Regex("([01]?[0-9]|2[0-3]):[0-5][0-9]");
                    var match = regex.Match(visitingTime.ToString()).Success;

                    if (!match)
                        TimeError = "Enter (hh:mm) the format";

                    //{
                    //    MessageBox.Show("Invalid Visiting Time", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                    //    return false;
                    //}
                }
                else if (string.IsNullOrEmpty(value))
                    TimeError = "Visiting Time cannot be empty";
                else
                    TimeError = "";
                    

            }
        }

        private string _doctornameerror;

        public string DoctorNameError
        {
            get { return _doctornameerror; }
            set { _doctornameerror = value; OnPropertyChanged(); }
        }

        private string _timeerror;

        public string TimeError
        {
            get { return _timeerror; }
            set { _timeerror = value;OnPropertyChanged(); }
        }


        private string _phonenumber;

        public string PhoneNumber
        {
            get { return _phonenumber; }
            set { _phonenumber = value;OnPropertyChanged(); }
        }

        private string _address;

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }


        private string _qualification;

        public string Qualification
        {
            get { return _qualification; }
            set { _qualification = value; }
        }



    }
}
