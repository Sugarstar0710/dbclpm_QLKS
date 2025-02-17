using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQLKS.App_Start;
using WebQLKS.Models;

namespace WebQLKS.Areas.Admin.Controllers
{
    [AdminAuthorize(maCV = "TPNS")]
    public class QuanLyNSController : Controller
    {
        // GET: Admin/QuanLyNS
        DAQLKSEntities db = new DAQLKSEntities();
        public ActionResult DanhSachNV()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Current = "DanhSachNV";
            var nhanvien = db.tbl_NhanVien.ToList();
            return View(nhanvien);
        }

        public ActionResult ChiTietNV(string maNV)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var user = maNV;
            if (user == null)
            {
                return RedirectToAction("DanhSachNV", "QuanLyNV");
            }
            else
            {
                var nhanvien = db.tbl_NhanVien.Where(s => s.MaNV == user).FirstOrDefault();
                return View(nhanvien);
            }
        }
        public ActionResult XoaNV(string maNV)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            return View(db.tbl_NhanVien.Where(s => s.MaNV == maNV).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult XoaNV(string maNV, tbl_NhanVien nv)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            try
            {
                nv = db.tbl_NhanVien.Where(s => s.MaNV == maNV).FirstOrDefault();
                db.tbl_NhanVien.Remove(nv);
                db.SaveChanges();
                return RedirectToAction("DanhSachNV");
            }
            catch
            {
                return Content("Gặp lỗi! Không thể xóa nhân viên.");
            }
        }
        //Thêm nhân viên
        private string MaNhanVien()
        {

            var CheckMa = db.tbl_NhanVien.OrderByDescending(p => p.MaNV).FirstOrDefault();
            if (CheckMa != null)
            {
                int MNV = int.Parse(CheckMa.MaNV.Substring(2));
                int nextMNV = MNV + 1;
                return "NV" + nextMNV.ToString();
            }
            return "NV1";
        }
        [HttpGet]
        public ActionResult ThemNhanVien()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var chucVu = db.tbl_ChucVu.ToList();
            ViewBag.MaCV = new SelectList(chucVu, "MaCV", "TenChucVu");
            return View();
        }

        [HttpPost]
        public ActionResult ThemNhanVien(string HoTen, string TaiKhoan, string Email, DateTime NgaySinh,string MatKhau, string MaCV)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }

            if (ModelState.IsValid)
            {
                string maNhanV = MaNhanVien();
                tbl_NhanVien nhanvien = new tbl_NhanVien()
                {
                    MaNV = maNhanV,
                    HoTen = HoTen,
                    TaiKhoan = TaiKhoan,
                    MatKhau = MatKhau,
                    Email = Email,
                    NgaySinh = NgaySinh,
                    MaCV = MaCV
                };
                var checkTK = db.tbl_NhanVien.Where(s => s.TaiKhoan == nhanvien.TaiKhoan).FirstOrDefault();
                if (checkTK == null)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.tbl_NhanVien.Add(nhanvien);
                    db.SaveChanges();
                    return RedirectToAction("DanhSachNV");
                }
                else
                {
                    ViewBag.ErrorRegister = "Tài khoảng đã được tạo";
                    return View();
                }
            }
            return View();
        }
        public ActionResult EditNV(string maNV)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var chucVu = db.tbl_ChucVu.ToList();
            ViewBag.MaCV = new SelectList(chucVu, "MaCV", "TenChucVu");
            return View(db.tbl_NhanVien.Where(s => s.MaNV == maNV).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult EditNV(string maNV, tbl_NhanVien nv)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            db.Entry(nv).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DanhSachNV", "QuanLyNS");
        }
    }
}