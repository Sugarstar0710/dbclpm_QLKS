using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WebQLKS.Models;

namespace WebQLKS.Controllers
{
    public class LoginAcountController : Controller
    {
        DAQLKSEntities db = new DAQLKSEntities();
        // GET: Login

        public ActionResult LoginAcountKH()
        {

            return View();
        }
        [HttpPost]
        public ActionResult LoginAcountKH(string TaiKhoan, string MatKhau)
        {
            tbl_KhachHang khachHang = new tbl_KhachHang()
            {
                TaiKhoan = TaiKhoan,
                MatKhau = MatKhau
            };
            var checkkh = db.tbl_KhachHang.Where(s => s.TaiKhoan == khachHang.TaiKhoan && s.MatKhau == khachHang.MatKhau).FirstOrDefault();

            if (checkkh == null)
            {
                TempData["SaiThongTin"] = "Sai tài khoản hoặc mật khẩu.";
                ViewBag.ErroInfo = "Sai tai khoan";
                return View("LoginAcountKH");
            }
            else
            {
                TempData["LoginSuccess"] = "Đăng nhập thành công!";
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["KH"] = checkkh.MaKH;
                ViewBag.SessionValue = Session["KH"];
                if (Session["PreviousUrl"] != null && !string.IsNullOrEmpty(Session["PreviousUrl"].ToString()))
                {
                    string previousUrl = Session["PreviousUrl"].ToString();
                    string maDichVu = Session["MaDichVu"] != null ? Session["MaDichVu"].ToString() : null;
                    Session.Remove("PreviousUrl"); // Xóa session sau khi sử dụng

                    Response.Redirect(previousUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", "Home");
            }
        }

        private string MaKhachHang()
        {
            var CheckMa = db.tbl_KhachHang.OrderByDescending(p => p.MaKH).FirstOrDefault();
            if (CheckMa != null)
            {
                int MKH = int.Parse(CheckMa.MaKH.Substring(2));
                int nextMKH = MKH + 1;
                return "KH" + nextMKH.ToString();
            }
            return "KH1";
        }
        [HttpGet]
        public ActionResult RegisterKH()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult RegisterKH(string HoTen, string TaiKhoan, string Email, string SDT, string CCCD, string DiaChi, DateTime NgaySinh,
            string MatKhau, string QuocTich)
        {

            if (ModelState.IsValid)
            {
                string loai = QuocTich.ToLower();
                string makhachH = MaKhachHang();
                int maLoaiKH;
                if (loai == "việt nam")
                {
                     maLoaiKH = 002;
                    
                }
                else
                {
                     maLoaiKH = 001;
                    
                }
                tbl_KhachHang khachhang = new tbl_KhachHang()
                {
                    MaKH = makhachH,
                    HoTen = HoTen,
                    TaiKhoan = TaiKhoan,
                    MatKhau = MatKhau,
                    Email = Email,
                    SDT = SDT,
                    NgaySinh = NgaySinh,
                    CCCD = CCCD,
                    DiaChi = DiaChi,
                    QuocTich = QuocTich,
                    MaLoaiKH = maLoaiKH
                };
                var checkTK = db.tbl_KhachHang.Where(s => s.TaiKhoan == khachhang.TaiKhoan).FirstOrDefault();
                if (checkTK == null)
                {
                    TempData["RegisterSuccess"] = "Đăng Ký thành công!";
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.tbl_KhachHang.Add(khachhang);
                    db.SaveChanges();
                    return RedirectToAction("LoginAcountKH");
                }
                else
                {
                    TempData["ErrorRegister"] = "Tài khoản đã có người đăng ký! Vui lòng thử lại";
                    return RedirectToAction("RegisterKH");
                }

            }
            return View();
        }

    }
}