using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQLKS.Models
{
    public class RoomDetailViewModel
    {
        public string maPhong { get; set; }
        public string tenPhong { get; set; }
        public decimal? donGia { get; set; }
        public string MoTa { get; set; }
        public List<string> TienIch { get; set; }
    }
}