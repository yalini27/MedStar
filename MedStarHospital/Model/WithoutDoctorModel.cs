using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedStarHospital.Model
{
    public class WithoutDoctorModel
    {
        public int ID { get; set; }
        public int BookingID { get; set; }
        public int TestingTypeID { get; set; }
        public DateTime ResultDate { get; set; }

    }
}
