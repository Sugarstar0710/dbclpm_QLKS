using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQLKS.Models;
using WebQLKS.App_Start;

namespace WebQLKS.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {

        DAQLKSEntities db = new DAQLKSEntities();
        // GET: Admin/Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string TaiKhoan, string MatKhau)
        {
            if (ModelState.IsValid)
            {
                var user = db.tbl_NhanVien.Where(nv => nv.TaiKhoan == TaiKhoan && nv.MatKhau == MatKhau).FirstOrDefault();
                if (user != null)
                {
                    TempData["LoginSuccess"] = "Đăng nhập thành công!";
                    Session["user"] = user;
                    return RedirectToAction("TrangNhanVien", "Admin");
                }
                else
                {
                    TempData["SaiThongTin"] = "Sai tài khoản hoặc mật khẩu.";
                    ViewBag.ErroInfo = "Sai tai khoan";
                    return RedirectToAction("Login", "Admin");
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["user"] != null)
                return RedirectToAction("TrangNhanVien", "Admin");
            return View();
        }
        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Login", "Admin");
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TrangNhanVien()
        {
            tbl_NhanVien nvSession = new tbl_NhanVien();
            nvSession = (tbl_NhanVien)Session["user"];
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                var nhanvien = db.tbl_NhanVien.Where(s => s.MaNV == nvSession.MaNV).FirstOrDefault();
                return View(nhanvien);
            }
        }
        public ActionResult EditNV(string maNV)
        {
            return View(db.tbl_NhanVien.Where(s => s.MaNV == maNV).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult EditNV(string maNV, tbl_NhanVien nv)
        {
            db.Entry(nv).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("TrangNhanVien", "Admin");
        }
    }
}