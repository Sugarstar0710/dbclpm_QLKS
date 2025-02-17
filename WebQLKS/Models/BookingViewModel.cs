using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQLKS.Models
{
    public class BookingViewModel
    {
        public string MaLoaiPhong { get; set; }
        public DateTime dateStart { get; set; }
        public DateTime dateEnd { get; set; }
    }

}