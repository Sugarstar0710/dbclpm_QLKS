using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebQLKS.App_Start;
using WebQLKS.Models;

namespace WebQLKS.Areas.Admin.Controllers
{
    [AdminAuthorize(maCV = "QLKH")]
    public class QuanLyKHController : Controller
    {
        DAQLKSEntities db = new DAQLKSEntities();
        // GET: Admin/QuanLyKH
        public ActionResult DanhSachKH()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Current = "DanhSachKH";
            var khachhang = db.tbl_KhachHang.ToList();
            return View(khachhang);
        }
        public ActionResult ChiTietKH(string maKH)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var user = maKH;
            if (user == null)
            {
                return RedirectToAction("DanhSachKH", "QuanLyKH");
            }
            else
            {
                var khachhang = db.tbl_KhachHang.Where(s => s.MaKH == user).FirstOrDefault();
                return View(khachhang);
            }
        }
        public ActionResult XoaKH(string maKH)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            return View(db.tbl_KhachHang.Where(s=>s.MaKH==maKH).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult XoaKH(string maKH, tbl_KhachHang kh)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            try
            {
                kh = db.tbl_KhachHang.Where(s=>s.MaKH==maKH).FirstOrDefault();
                db.tbl_KhachHang.Remove(kh);
                db.SaveChanges();
                return RedirectToAction("DanhSachKH");
            }
            catch
            {
                return Content("Gặp lỗi! Không thể xóa khách hàng.");
            }
        }
    }
}