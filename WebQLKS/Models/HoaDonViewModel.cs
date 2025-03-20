using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQLKS.Models
{
    public class HoaDonViewModel
    {
        public tbl_HoaDon HoaDon { get; set; }
        public List<DichVuViewModel> DanhSachDichVu { get; set; }
    }

    public class DichVuViewModel
    {
        public string MaDV { get; set; }
        public string TenDichVu { get; set; }
        public DateTime NgaySuDungDV { get; set; }
        public decimal DonGia { get; set; }
        public string MaTrangThaiDV { get; set; }
    }

}