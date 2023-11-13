using MedStarHospital.Model;
using MedStarHospital.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MedStarHospital.ViewModel
{
    public class VMPharmacy : ViewModelBase
    {
        public static Action RefreshPharmacy;
        public VMPharmacy(UserModel user)
        {
            User = user;
            fnView();
            RefreshPharmacy += OnPharmacyRefresh;
        }

        public void OnPharmacyRefresh()
        {
            fnView();
        }

        private UserModel userr;

        public UserModel User
        {
            get { return userr; }
            set { userr = value; OnPropertyChanged(); }
        }

        private string _find;

        public string Find
        {
            get { return _find; }
            set { _find = value; OnPropertyChanged(); fnAutoApply(); }
        }


        private string _field = "Medicine ID";

        public string Field
        {
            get { return _field; }
            set { _field = value; OnPropertyChanged(); }
        }

        private string _column;

        public string Column
        {
            get { return _column; }
            set { _column = value; OnPropertyChanged(); }
        }

        void fnAutoApply()
        {


            switch (Field) // ithu null aa irukku. Sele
            {
                case "Medicine ID":
                    Column = "MedicineID";
                    break;
                case "Medicine Name":
                    Column = "MedicineName";
                    break;

                case "BrandName":
                    Column = "Categary";
                    break;

                //case "Status":
                //    Column = "status_state";
                //    break;
            }
            PharmacyList = new ObservableCollection<PharmacyModel>();
            Sql_Connection.sql_connection();
            //string Query = $"select * from tblUser where {Column}  Like '%" + Find + "%'";
            string Query = $"select * from tblPharmacy where {Column} Like '%{Find}%'";
            SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                PharmacyList.Add(new PharmacyModel
                {
                    MedicineID = reader.GetValue(0).ToString(),
                    MedicineName = reader.GetValue(1).ToString(),
                    Categary = reader.GetValue(2).ToString(),
                    Quantity = (int)reader.GetValue(3),
                    UnitPrice = (decimal)reader.GetValue(4)
                });
            }
            reader.Close();
            Sql_Connection.close_connection();

        }



        void fnView()
        {
            PharmacyList = new ObservableCollection<PharmacyModel>();
            Sql_Connection.DBConnection();
            Sql_Connection.sql_connection();
            string Query = $"select * from tblPharmacy";
            SqlCommand command = new SqlCommand(Query, Sql_Connection.getconnection());
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                PharmacyList.Add(new PharmacyModel
                {
                    MedicineID = reader.GetValue(0).ToString(),
                    MedicineName = reader.GetValue(1).ToString(),
                    Categary = reader.GetValue(2).ToString(),
                    Quantity = (int)reader.GetValue(3),
                    UnitPrice = (decimal)reader.GetValue(4)
                });
            }
        }

        public ICommand cmdReset { get { return new RelayCommand(fnReset); } }

        public ICommand cmdAdd { get { return new RelayCommand(fnAdd); } }

        public ICommand cmdEdit { get { return new RelayCommand(fnEdit); } }

        void fnReset(object param)
        {
            fnView();
        }


        void fnEdit(object param)
        {
            PharmacyModel us = param as PharmacyModel;
            AddPharmacyView av = new AddPharmacyView();
            av.DataContext = new VMAddPharmacy(us);
            av.ShowDialog();
        }
        private PharmacyModel _pharmacy = new PharmacyModel();

        public PharmacyModel Pharmacy
        {
            get { return _pharmacy; }
            set { _pharmacy = value; OnPropertyChanged(); }
        }

        private ObservableCollection<PharmacyModel> _pharmacylist;

        public ObservableCollection<PharmacyModel> PharmacyList
        {
            get { return _pharmacylist; }
            set { _pharmacylist = value; OnPropertyChanged(); }
        }

        void fnAdd(object param)
        {
            AddPharmacyView av = new AddPharmacyView();
            av.DataContext = new VMAddPharmacy();
            av.ShowDialog();
        }
    }
}
