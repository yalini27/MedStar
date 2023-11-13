using MedStarHospital.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace MedStarHospital.ViewModel
{
    public class VMAddPharmacy : ViewModelBase
    {
        public ICommand cmdback { get { return new RelayCommand(fnBack); } }

        public ICommand cmdOption { get { return new RelayCommand(fnOption); } }

        public static Action exit;

        public VMAddPharmacy(PharmacyModel pharmacy = null)
        {
            ISPharmacy();
            if (pharmacy == null)
            {
                btn = "ADD";
                btnback = "BACK";
            }
            else
            {
                Pharmacy = pharmacy;
                Pharmacy = new PharmacyModel
                {
                    MedicineID = pharmacy.MedicineID,
                    MedicineName = pharmacy.MedicineName,
                    Categary = pharmacy.Categary,
                    Quantity = pharmacy.Quantity,
                    UnitPrice = pharmacy.UnitPrice
                };
                btn = "UPDATE";
                btnback = "BACK";
            }
            PharmacyList = new ObservableCollection<PharmacyModel>();
            //fngetRole();
        }

        void ISPharmacy()
        {
            Pharmacy = new();

            //Pharmacy.MedicineID = Sql_Connection.IsData("tblPharmacy") ? Sql_Connection.SpaficDataISINTable("tblPharmacy", "MedicineID", "MedicineID") + 1 : "1";
            //var number = Sql_Connection.IsData("tblPharmacy");
            var number = Sql_Connection.SpaficDataISINTable("tblPharmacy", "MedicineID", "MedicineID");
            if(number == null)
            {
                Pharmacy.MedicineID = "MED001";
            }
            else
            {
                string numericPart = number.Substring(3);

                if (int.TryParse(numericPart, out int numericValue))
                {
                    numericValue++;

                    string newMedicineId = "MED" + numericValue.ToString("D3");
                    Pharmacy.MedicineID = newMedicineId;
                    
                }
            }
        }

        void fnBack(object param)
        {
            if (MessageBox.Show("Do you want to close this page?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                exit.Invoke();
            }
        }
        void fnOption(object param)
        {

            switch (btn)
            {

                case "ADD":
                    if (validation())
                    {
                        Sql_Connection.sql_connection();
                        string Query = $"insert into tblPharmacy values('" + Pharmacy.MedicineID + "','" + Pharmacy.MedicineName + "','" + Pharmacy.Categary + "','" + Pharmacy.Quantity + "','" + Pharmacy.UnitPrice + "')";
                        SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.InsertCommand = new SqlCommand(Query, Sql_Connection.getconnection());
                        adapter.InsertCommand.ExecuteReader();
                        adapter.Dispose();
                        command.Dispose();
                        Sql_Connection.close_connection();
                        MessageBox.Show("Successfully Added", "Add", MessageBoxButton.OK, MessageBoxImage.Information);
                        exit.Invoke();
                    }
                    else
                    {
                        MessageBox.Show("Invalid datas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;


                case "UPDATE":
                    if (validation1())
                    {
                        Sql_Connection.sql_connection();
                        string query = $"update tblPharmacy set MedicineName = '" + Pharmacy. MedicineName + "',Categary='" + Pharmacy.Categary + "',Quantity='" + Pharmacy.Quantity + "',UnitPrice='" + Pharmacy.UnitPrice + "'where MedicineID='" + Pharmacy.MedicineID + "'";
                        SqlCommand command1 = new SqlCommand(query, Sql_Connection.getconnection());
                        SqlDataAdapter adapter1 = new SqlDataAdapter();
                        adapter1.UpdateCommand = new SqlCommand(query, Sql_Connection.getconnection());
                        adapter1.UpdateCommand.ExecuteReader();
                        adapter1.Dispose();
                        command1.Dispose();
                        Sql_Connection.close_connection();
                        MessageBox.Show("Update successfully!", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                        exit.Invoke();
                    }
                    else
                    {
                        MessageBox.Show("Invalid datas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    break;
            }

            VMPharmacy.RefreshPharmacy.Invoke();
            //AppEvents.OnRefreshUser();
        }





        public bool validation()
        {
            bool result = false;
            int name;
            bool crtnam = !string.IsNullOrWhiteSpace(Pharmacy.MedicineName) && !(int.TryParse(Pharmacy.MedicineName, out name)) && !Pharmacy.MedicineName.All(sp => !char.IsLetter(sp));
            bool cate = !string.IsNullOrWhiteSpace(Pharmacy.Categary) && !string.IsNullOrEmpty(Pharmacy.Categary);
            //bool role = !string.IsNullOrWhiteSpace(User.Role) && !string.IsNullOrEmpty(User.Role);
            //bool status = !string.IsNullOrWhiteSpace(User.Status) && !string.IsNullOrEmpty(User.Status);
            //if (Sql_Connection.IsData("tblUser", "UserName", User.UserName))
            //{
            //    MessageBox.Show("This user name already insert", "Add user", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return false;
            //}

            if (crtnam /*&& password && role && status*/)
            {
                result = true;
            }
            return result;
        }

        public bool validation1()
        {
            bool result = false;
            int name;
            bool crtnam = !string.IsNullOrWhiteSpace(Pharmacy.MedicineName) && !(int.TryParse(Pharmacy.MedicineName, out name)) && !Pharmacy.MedicineName.All(sp => !char.IsLetter(sp));
            bool cate = !string.IsNullOrWhiteSpace(Pharmacy.Categary) && !string.IsNullOrEmpty(Pharmacy.Categary);
            //bool price = !string.IsNullOrWhiteSpace(Pharmacy.UnitPrice) && !string.IsNullOrEmpty(User.Role);
            //bool quan = !string.IsNullOrWhiteSpace(User.Status) && !string.IsNullOrEmpty(User.Status);

            if (crtnam && cate /*&& price && quan*/)
            {
                result = true;
            }
            return result;
        }
        private PharmacyModel _pharmacy = new PharmacyModel();

        public PharmacyModel Pharmacy
        {
            get { return _pharmacy; }
            set { _pharmacy = value; OnPropertyChanged(); }
        }

        private string _btn;

        public string btn
        {
            get { return _btn; }
            set { _btn = value; OnPropertyChanged(); }
        }

        private string _btnback;

        public string btnback
        {
            get { return _btnback; }
            set { _btnback = value; OnPropertyChanged(); }
        }


        private ObservableCollection<PharmacyModel> _pharmacylist;

        public ObservableCollection<PharmacyModel> PharmacyList
        {
            get { return _pharmacylist; }
            set { _pharmacylist = value; OnPropertyChanged(); }
        }

    }
}
