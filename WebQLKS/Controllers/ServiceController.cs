using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQLKS.Models;

namespace WebQLKS.Controllers
{
    public class ServiceController : Controller
    {
        DAQLKSEntities db = new DAQLKSEntities();
        // GET: Service
        public ActionResult Index()
        {
            var DV = db.tbl_LoaiDichVu.ToList();
            return View(DV);
        }
        public ActionResult chiTietLoaiDV(string maLoaiDV)
        {
            var ct = db.tbl_DichVu.Where(dv => dv.MaLoaiDV == maLoaiDV).ToList();
            return View(ct);
        }
        public ActionResult detailService(string maDV)
        {
            Session["PreviousUrl"] = Request.Url.AbsoluteUri;
            Session["Previous"] = Request.Url.AbsoluteUri;
            var ctDV = db.tbl_DichVu.Where(r => r.MaDV == maDV).FirstOrDefault();
            return View(ctDV);
        }
        private string ID()
        {
            var lastID = db.tbl_DichVuDaDat.OrderByDescending(m => m.ID).FirstOrDefault();
            if (lastID != null)
            {
                int iD = int.Parse(lastID.ID.Substring(2));
                int newID = iD + 1;
                if (newID >= 10)
                {
                    return "SD" + newID.ToString();
                }
                return "SD0" + newID.ToString();
            }
            return "SD01";

        }
        private string maHoaDon()
        {
            var lastBill = db.tbl_HoaDon.OrderByDescending(p => p.MaHD).FirstOrDefault();
            if (lastBill != null)
            {
                int MHD = int.Parse(lastBill.MaHD.Substring(2));
                int nextMHD = MHD + 1;
                if (nextMHD >= 10)
                {
                    return "HD" + nextMHD.ToString();
                }
                return "HD0" + nextMHD.ToString();
            }
            return "HD01";
        }
        /*[HttpPost]*/
        public ActionResult orderService(string maDV)
        {
            DateTime systemTime = DateTime.Now; // Thời gian hệ thống
            TimeZoneInfo vietNamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vietNamTime = TimeZoneInfo.ConvertTime(systemTime, vietNamTimeZone);
            if (Session["KH"] == null)
            {
                TempData["SessionKhNull"] = "Vui lòng đăng nhập để tiếp tục!";
                Session["MaDichVu"] = maDV;
                return RedirectToAction("LoginAcountKH", "LoginAcount");
            }
            var maKH = Session["KH"].ToString();
            var phieuThue = db.tbl_PhieuThuePhong.Where(i => i.MaKH == maKH && i.NgayBatDauThue <= vietNamTime && vietNamTime <= i.NgayKetThucThue && (i.TrangThai == "Đã nhận phòng")).OrderByDescending(i => i.MaPhieuThuePhong).FirstOrDefault();
            if (phieuThue == null)
            {
                TempData["ErrorService"] = "Cần nhận phòng để đặt dịch vụ!";
            }
            else
            {
                string id = ID();
                var donGia = db.tbl_DichVu.Where(dv => dv.MaDV == maDV).Select(dv => dv.DonGia).FirstOrDefault();
                if (donGia == null)
                {
                    ViewBag.Message = "Dịch vụ không tồn tại hoặc không có đơn giá.";
                    return RedirectToAction("Index", "Home");
                }
                string maHD = db.tbl_HoaDon
                             .Where(hd => hd.MaKH == maKH && hd.TrangThai == "Chưa thanh toán")
                             .OrderByDescending(hd => hd.MaHD)
                             .Select(hd => hd.MaHD)
                             .FirstOrDefault();
                var hoaDon = db.tbl_HoaDon.SingleOrDefault(hd => hd.MaHD == maHD);
                tbl_DichVuDaDat ord_service = new tbl_DichVuDaDat
                {
                    ID = id,
                    NgaySuDungDV = vietNamTime,
                    MaHD = maHD,
                    MaTrangThaiDV = "TT01",
                    MaNV = null,
                    MaKH = Session["KH"].ToString(),
                    MaDV = maDV
                };
                db.tbl_DichVuDaDat.Add(ord_service);
                db.SaveChanges();
                TempData["SuccessServiceMessage"] = "Đặt dịch vụ thành công!";
            }
            db.Configuration.ValidateOnSaveEnabled = false;
            if (Session["Previous"] != null && !string.IsNullOrEmpty(Session["Previous"].ToString()))
            {
                string Previous = Session["Previous"].ToString();
                Session.Remove("Previous"); // Xóa session sau khi sử dụng

                Response.Redirect(Previous);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}