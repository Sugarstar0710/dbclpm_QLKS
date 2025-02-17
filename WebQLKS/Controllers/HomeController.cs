using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQLKS.Models;
using System.Globalization;

namespace WebQLKS.Controllers
{
    public class HomeController : Controller
    {
        DAQLKSEntities db = new DAQLKSEntities();
        public string FormatValue(decimal amount)
        {
            CultureInfo culture = new CultureInfo("vi-VN");
            return string.Format(culture, "{0:N0}", amount); // Định dạng số thành chuỗi tiền tệ
        }
        public ActionResult Index()
        {
            ViewBag.tenMon = db.tbl_DichVu.Where(m => m.MaDV.StartsWith("DA")).ToList();
            var room = db.tbl_LoaiPhong.ToList();
            return View(room);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}