using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedStarHospital.ViewModel
{
    public static class AppEvents
    {
        public static Action RefreshPatient;
        public static void OnRefreshPatient()
        {
            RefreshPatient.Invoke();
        }

        public static Action RefreshDoctor;
        public static void OnRefreshDoctor()
        {
            RefreshDoctor.Invoke();
        }

        public static Action DepartmentRefresh;
        public static void OnRefreshDepartment()
        {
            DepartmentRefresh.Invoke();
        }


        public static Action RefreshBill;
        public static void OnRefreshBill()
        {
            RefreshBill.Invoke();
        }

        public static Action RefreshTestingType;
        public static void OnRefreshTestingType()
        {
            RefreshTestingType.Invoke();
        }

        public static Action RefreshUser;
        public static void OnRefreshUser()
        {
            RefreshUser.Invoke();
        }


        public static Action RefreshPharmacy;
        public static void OnRefreshPharmacy()
        {
            RefreshPharmacy.Invoke();
        }

        //public static Action RefreshBookingTesting;
        //public static void OnRefreshBookingTesting()
        //{
        //    RefreshBookingTesting.Invoke();
        //}
    }
}
