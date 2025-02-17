using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebQLKS.App_Start;
using WebQLKS.Models;

namespace WebQLKS.Areas.Admin.Controllers
{
    [AdminAuthorize(maCV = "QLDV")]
    public class QuanLyDVController : Controller
    {
        DAQLKSEntities db = new DAQLKSEntities();
        //DANH SÁCH PHÒNG
        public ActionResult DanhSachPhong()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Current = "DanhSachPhong";
            var room = db.tbl_Phong.ToList();
            return View(room);
        }
        //Thêm Phòng
        [HttpGet]
        public ActionResult ThemPhong()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var maTT = db.tbl_TrangThaiPhong.ToList();
            var maLoai = db.tbl_LoaiPhong.ToList();
            ViewBag.maTT = new SelectList(maTT, "MaTrangThai", "TenTrangThai"); ;
            ViewBag.MaLoai = new SelectList(maLoai, "MaLoaiPhong", "TenLoaiPhong"); ;
            return View();
        }
        [HttpPost]
        public ActionResult ThemPhong(tbl_Phong room, HttpPostedFileBase img)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (ModelState.IsValid)
            {
                var check = db.tbl_Phong.Any(r => r.MaPhong == room.MaPhong || r.SoPhong == room.SoPhong);
                if (check)
                {
                    ViewBag.check = "Mã phòng hoặc số phòng đã tồn tại";
                    return RedirectToAction("ThemPhong");
                }
                if (img != null && img.ContentLength > 0)
                {
                    // Đặt tên file
                    var fileName = Path.GetFileName(img.FileName);

                    // Đặt đường dẫn để lưu trữ ảnh
                    var path = Path.Combine(Server.MapPath("~/Content/Room/img/"), fileName);

                    // Lưu ảnh vào thư mục ~/Content/Room/img/
                    img.SaveAs(path);

                    // Lưu đường dẫn ảnh vào model
                    room.HinhAnh = fileName;
                }
                db.tbl_Phong.Add(room);
                db.SaveChanges();
                return RedirectToAction("DanhSachPhong");

            }
            return View();
        }
        [HttpGet]
        public ActionResult XoaPhong(string maPhong)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (maPhong == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var roomType = db.tbl_Phong.Where(r => r.MaPhong == maPhong).FirstOrDefault();
            if (roomType == null)
            {
                return HttpNotFound();
            }
            return View(roomType);
        }
        [HttpPost, ActionName("XoaPhong")]
        public ActionResult XacNhanXoaPhong(string maPhong)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            try
            {
                var roomType = db.tbl_Phong.Where(r => r.MaPhong == maPhong).FirstOrDefault();
                db.tbl_Phong.Remove(roomType);
                db.SaveChanges();
                return RedirectToAction("DanhSachPhong");
            }
            catch
            {
                return Content("Không xóa được do có liên quan đến bảng khác");
            }
        }
        [HttpGet]
        public ActionResult ChinhSuaPhong(string maPhong)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var maTT = db.tbl_TrangThaiPhong.ToList();
            var maLoai = db.tbl_LoaiPhong.ToList();
            ViewBag.maTT = new SelectList(maTT, "MaTrangThai", "TenTrangThai"); ;
            ViewBag.MaLoai = new SelectList(maLoai, "MaLoaiPhong", "TenLoaiPhong"); ;
            var roomType = db.tbl_Phong.Where(r => r.MaPhong == maPhong).FirstOrDefault();
            return View(roomType);
        }
        [HttpPost]
        public ActionResult ChinhSuaPhong(tbl_Phong roomType, HttpPostedFileBase img)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    var fileName = Path.GetFileName(img.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Room/img/"), fileName);
                    img.SaveAs(path);
                    roomType.HinhAnh = fileName;
                }
                db.Entry(roomType).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhSachPhong");
            }
            return View();
        }
        public ActionResult ChiTietPhong(string maPhong)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var item = db.tbl_Phong.Where(i => i.MaPhong.Equals(maPhong)).FirstOrDefault();
            return View(item);
        }

        //DANH MỤC PHÒNG
        public ActionResult DanhMucPhong()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Current = "DanhMucPhong";
            var roomType = db.tbl_LoaiPhong.ToList();
            return View(roomType);
        }
        [HttpGet]
        public ActionResult ThemLoaiPhong()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ThemLoaiPhong(tbl_LoaiPhong type, HttpPostedFileBase img)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (ModelState.IsValid)
            {
                if (img != null && img.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(img.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Home/Image/"), fileName);
                    img.SaveAs(path);
                    type.img = fileName;
                }
                db.tbl_LoaiPhong.Add(type);
                db.SaveChanges();
                return RedirectToAction("DanhMucPhong");
            }
            return View();
        }
        [HttpGet]
        public ActionResult XoaLoaiPhong(string maLoaiPhong)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (maLoaiPhong == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var roomType = db.tbl_LoaiPhong.Where(r => r.MaLoaiPhong == maLoaiPhong).FirstOrDefault();
            if (roomType == null)
            {
                return HttpNotFound();
            }
            return View(roomType);
        }
        [HttpPost, ActionName("XoaLoaiPhong")]
        public ActionResult DeleteConfirmed(string maLoaiPhong)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            try
            {
                var roomType = db.tbl_LoaiPhong.Where(r => r.MaLoaiPhong == maLoaiPhong).FirstOrDefault();
                db.tbl_LoaiPhong.Remove(roomType);
                db.SaveChanges();
                return RedirectToAction("DanhMucPhong");
            }
            catch
            {
                return Content("Không xóa được do có liên quan đến bảng khác");
            }
        }
        [HttpGet]
        public ActionResult ChinhSuaLoaiPhong(string maLoaiPhong)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var roomType = db.tbl_LoaiPhong.Where(r => r.MaLoaiPhong == maLoaiPhong).FirstOrDefault();
            return View(roomType);
        }
        [HttpPost]
        public ActionResult ChinhSuaLoaiPhong(tbl_LoaiPhong roomType, HttpPostedFileBase img)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    // Đặt tên file
                    var fileName = Path.GetFileName(img.FileName);

                    // Đặt đường dẫn để lưu trữ ảnh
                    var path = Path.Combine(Server.MapPath("~/Content/Home/Image/"), fileName);

                    // Lưu ảnh vào thư mục ~/Content/Room/img/
                    img.SaveAs(path);

                    // Lưu đường dẫn ảnh vào model
                    roomType.img = fileName;
                }
            }
            db.Entry(roomType).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DanhMucPhong");
        }
        public ActionResult ChiTietLoaiPhong(string maLoaiPhong)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var roomType = db.tbl_LoaiPhong.Where(r => r.MaLoaiPhong == maLoaiPhong).FirstOrDefault();
            return View(roomType);

        }
        //MENU
        private string maMonAn()
        {
            var lastBooking = db.tbl_DichVu.Where(p => p.MaDV.StartsWith("DA")).OrderByDescending(p => p.MaDV).FirstOrDefault();
            if (lastBooking != null)
            {
                int maDV = int.Parse(lastBooking.MaDV.Substring(3));
                int nextMPT = maDV + 1;
                if (nextMPT >= 10)
                {
                    return "DA" + nextMPT.ToString();
                }
                return "DA0" + nextMPT.ToString();
            }
            return "DA01";
        }
        public ActionResult ThucDon()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Current = "ThucDon";
            var menu = db.tbl_DichVu.Where(m => m.MaLoaiDV == "DV02").ToList();
            return View(menu);
        }
        [HttpGet]
        public ActionResult ThemMon()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var maDV = maMonAn();
            ViewBag.maDV = maDV;
            return View();
        }
        [HttpPost]
        public ActionResult ThemMon(tbl_DichVu monAn, HttpPostedFileBase img)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (ModelState.IsValid)
            {
                if (img != null && img.ContentLength > 0)
                {
                    // Đặt tên file
                    var fileName = Path.GetFileName(img.FileName);

                    // Đặt đường dẫn để lưu trữ ảnh
                    var path = Path.Combine(Server.MapPath("~/Content/Service/menu/"), fileName);

                    // Lưu ảnh vào thư mục ~/Content/Room/img/
                    img.SaveAs(path);

                    // Lưu đường dẫn ảnh vào model
                    monAn.img = fileName;
                }
                monAn.MaDV = maMonAn();
                monAn.MaLoaiDV = "DV02";
                db.tbl_DichVu.Add(monAn);
                db.SaveChanges();
                return RedirectToAction("ThucDon");

            }
            return View();
        }
        [HttpGet]
        public ActionResult SuaMon(string maDV)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var monAn = db.tbl_DichVu.Where(m => m.MaDV == maDV).FirstOrDefault();
            return View(monAn);
        }
        [HttpPost]
        public ActionResult SuaMon(tbl_DichVu monAn, HttpPostedFileBase img)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (ModelState.IsValid)
            {
                var mon = db.tbl_DichVu.FirstOrDefault(m => m.MaDV == monAn.MaDV);
                if (mon != null)
                {
                    if (img != null && img.ContentLength > 0)
                    {
                        // Đặt tên file
                        var fileName = Path.GetFileName(img.FileName);

                        // Đặt đường dẫn để lưu trữ ảnh
                        var path = Path.Combine(Server.MapPath("~/Content/Service/menu/"), fileName);

                        // Lưu ảnh vào thư mục ~/Content/Service/menu/
                        img.SaveAs(path);

                        // Lưu đường dẫn ảnh vào model
                        mon.img = fileName;
                    }

                    // Cập nhật các trường khác của model
                    mon.TenDV = monAn.TenDV;
                    mon.DonGia = monAn.DonGia;
                    mon.MaLoaiDV = monAn.MaLoaiDV;
                    mon.MoTa = monAn.MoTa;

                    // Đánh dấu thực thể là đã sửa đổi
                    db.Entry(mon).State = System.Data.Entity.EntityState.Modified;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    db.SaveChanges();

                    return RedirectToAction("ThucDon");
                }
                else
                {
                    // Xử lý trường hợp không tìm thấy thực thể
                    ModelState.AddModelError("", "Không tìm thấy món ăn cần cập nhật.");
                }
            }

            // Nếu có lỗi, hiển thị lại view cùng với thông báo lỗi
            return View(monAn);
        }

        public ActionResult ChiTietMon(string maDV)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var monAn = db.tbl_DichVu.Where(m => m.MaDV == maDV).FirstOrDefault();
            return View(monAn);
        }

        [HttpGet]
        public ActionResult XoaMon(string maDV)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (maDV == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var mon = db.tbl_DichVu.Where(r => r.MaDV == maDV).FirstOrDefault();
            if (mon == null)
            {
                return HttpNotFound();
            }
            return View(mon);
        }
        [HttpPost, ActionName("XoaMon")]
        public ActionResult XacNhanXoaMon(string maDV)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            try
            {
                var mon = db.tbl_DichVu.Where(r => r.MaDV == maDV).FirstOrDefault();
                db.tbl_DichVu.Remove(mon);
                db.SaveChanges();
                return RedirectToAction("ThucDon");
            }
            catch
            {
                return Content("Không xóa được do có liên quan đến bảng khác");
            }
        }
        //SPA
        public ActionResult Spa()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Current = "Spa";
            var spa = db.tbl_DichVu.Where(s => s.MaLoaiDV == "DV03").ToList();
            return View(spa);
        }

        public ActionResult ChiTietSpa(string maDV)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var spa = db.tbl_DichVu.Where(m => m.MaDV == maDV).FirstOrDefault();
            return View(spa);
        }

        [HttpGet]
        public ActionResult ChinhSuaSpa(string maDV)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var spa = db.tbl_DichVu.Where(m => m.MaDV == maDV).FirstOrDefault();
            return View(spa);
        }
        [HttpPost]
        public ActionResult ChinhSuaSpa(tbl_DichVu spa, HttpPostedFileBase img)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (ModelState.IsValid)
            {
                var mon = db.tbl_DichVu.FirstOrDefault(m => m.MaDV == spa.MaDV);
                if (mon != null)
                {
                    if (img != null && img.ContentLength > 0)
                    {
                        // Đặt tên file
                        var fileName = Path.GetFileName(img.FileName);

                        // Đặt đường dẫn để lưu trữ ảnh
                        var path = Path.Combine(Server.MapPath("~/Content/Service/spa/"), fileName);

                        // Lưu ảnh vào thư mục ~/Content/Service/menu/
                        img.SaveAs(path);

                        // Lưu đường dẫn ảnh vào model
                        mon.img = fileName;
                    }

                    // Cập nhật các trường khác của model
                    mon.TenDV = spa.TenDV;
                    mon.DonGia = spa.DonGia;
                    mon.MaLoaiDV = spa.MaLoaiDV;
                    mon.MoTa = spa.MoTa;

                    // Đánh dấu thực thể là đã sửa đổi
                    db.Entry(mon).State = System.Data.Entity.EntityState.Modified;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    db.SaveChanges();

                    return RedirectToAction("Spa");
                }
                else
                {
                    // Xử lý trường hợp không tìm thấy thực thể
                    ModelState.AddModelError("", "Không tìm thấy món ăn cần cập nhật.");
                }
            }

            // Nếu có lỗi, hiển thị lại view cùng với thông báo lỗi
            return View(spa);
        }
        [HttpGet]
        public ActionResult XoaSpa(string maDV)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (maDV == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var spa = db.tbl_DichVu.Where(r => r.MaDV == maDV).FirstOrDefault();
            if (spa == null)
            {
                return HttpNotFound();
            }
            return View(spa);
        }
        [HttpPost, ActionName("XoaSpa")]
        public ActionResult XacNhanXoaSpa(string maDV)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            try
            {
                var spa = db.tbl_DichVu.Where(r => r.MaDV == maDV).FirstOrDefault();
                db.tbl_DichVu.Remove(spa);
                db.SaveChanges();
                return RedirectToAction("Spa");
            }
            catch
            {
                return Content("Không xóa được do có liên quan đến bảng khác");
            }
        }
        private string maSpa()
        {
            var lastBooking = db.tbl_DichVu.Where(p => p.MaDV.StartsWith("LD")).OrderByDescending(p => p.MaDV).FirstOrDefault();
            if (lastBooking != null)
            {
                int maDV = int.Parse(lastBooking.MaDV.Substring(3));
                int nextMPT = maDV + 1;
                if (nextMPT >= 10)
                {
                    return "LD" + nextMPT.ToString();
                }
                return "LD0" + nextMPT.ToString();
            }
            return "LD01";
        }
        [HttpGet]
        public ActionResult ThemSpa()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var maDV = maSpa();
            ViewBag.maDV = maDV;
            return View();
        }
        [HttpPost]
        public ActionResult ThemSpa(tbl_DichVu spa, HttpPostedFileBase img)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (ModelState.IsValid)
            {
                if (img != null && img.ContentLength > 0)
                {
                    // Đặt tên file
                    var fileName = Path.GetFileName(img.FileName);

                    // Đặt đường dẫn để lưu trữ ảnh
                    var path = Path.Combine(Server.MapPath("~/Content/Service/spa/"), fileName);

                    // Lưu ảnh vào thư mục ~/Content/Room/img/
                    img.SaveAs(path);

                    // Lưu đường dẫn ảnh vào model
                    spa.img = fileName;
                }
                spa.MaDV = maMonAn();
                spa.MaLoaiDV = "DV03";
                db.tbl_DichVu.Add(spa);
                db.SaveChanges();
                return RedirectToAction("Spa");

            }
            return View();
        }


        //DỌN DẸP
        private string maDonDep()
        {
            var lastBooking = db.tbl_DichVu.Where(p => p.MaDV.StartsWith("DD")).OrderByDescending(p => p.MaDV).FirstOrDefault();
            if (lastBooking != null)
            {
                int maDV = int.Parse(lastBooking.MaDV.Substring(3));
                int nextMPT = maDV + 1;
                if (nextMPT >= 10)
                {
                    return "DD" + nextMPT.ToString();
                }
                return "DD0" + nextMPT.ToString();
            }
            return "DD01";
        }
        public ActionResult DonDep()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Current = "Dondep";
            var lstRequest = db.tbl_DichVu.Where(r => r.MaLoaiDV == "DV01").ToList();
            return View(lstRequest);
        }

        public ActionResult ChiTietDonDep(string maDV)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var detai = db.tbl_DichVu.Where(d => d.MaDV == maDV).FirstOrDefault();
            return View(detai);
        }
        [HttpGet]
        public ActionResult ThemDonDep()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var maDV = maDonDep();
            ViewBag.maDV = maDV;
            return View();
        }
        [HttpPost]
        public ActionResult ThemDonDep(tbl_DichVu vs, HttpPostedFileBase img)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (ModelState.IsValid)
            {
                if (img != null && img.ContentLength > 0)
                {
                    // Đặt tên file
                    var fileName = Path.GetFileName(img.FileName);

                    // Đặt đường dẫn để lưu trữ ảnh
                    var path = Path.Combine(Server.MapPath("~/Content/Service/DonDep/"), fileName);

                    // Lưu ảnh vào thư mục ~/Content/Room/img/
                    img.SaveAs(path);

                    // Lưu đường dẫn ảnh vào model
                    vs.img = fileName;
                }
                vs.MaDV = maDonDep();
                vs.MaLoaiDV = "DV01";
                db.tbl_DichVu.Add(vs);
                db.SaveChanges();
                return RedirectToAction("DonDep");

            }
            return View();
        }
        [HttpGet]
        public ActionResult XoaDonDep(string maDV)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (maDV == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var vs = db.tbl_DichVu.Where(r => r.MaDV == maDV).FirstOrDefault();
            if (vs == null)
            {
                return HttpNotFound();
            }
            return View(vs);
        }
        [HttpPost, ActionName("XoaDonDep")]
        public ActionResult XacNhanXoaDonDep(string maDV)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            try
            {
                var vs = db.tbl_DichVu.Where(r => r.MaDV == maDV).FirstOrDefault();
                db.tbl_DichVu.Remove(vs);
                db.SaveChanges();
                return RedirectToAction("DonDep");
            }
            catch
            {
                return Content("Không xóa được do có liên quan đến bảng khác");
            }
        }

        [HttpGet]
        public ActionResult ChinhSuaDonDep(string maDV)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var vs = db.tbl_DichVu.Where(m => m.MaDV == maDV).FirstOrDefault();
            return View(vs);
        }
        [HttpPost]
        public ActionResult ChinhSuaDonDep(tbl_DichVu vs, HttpPostedFileBase img)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (ModelState.IsValid)
            {
                var mon = db.tbl_DichVu.FirstOrDefault(m => m.MaDV == vs.MaDV);
                if (mon != null)
                {
                    if (img != null && img.ContentLength > 0)
                    {
                        // Đặt tên file
                        var fileName = Path.GetFileName(img.FileName);

                        // Đặt đường dẫn để lưu trữ ảnh
                        var path = Path.Combine(Server.MapPath("~/Content/Service/DonDep/"), fileName);

                        // Lưu ảnh vào thư mục ~/Content/Service/menu/
                        img.SaveAs(path);

                        // Lưu đường dẫn ảnh vào model
                        mon.img = fileName;
                    }

                    // Cập nhật các trường khác của model
                    mon.TenDV = vs.TenDV;
                    mon.DonGia = vs.DonGia;
                    mon.MaLoaiDV = vs.MaLoaiDV;
                    mon.MoTa = vs.MoTa;

                    // Đánh dấu thực thể là đã sửa đổi
                    db.Entry(mon).State = System.Data.Entity.EntityState.Modified;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    db.SaveChanges();

                    return RedirectToAction("DonDep");
                }
                else
                {
                    // Xử lý trường hợp không tìm thấy thực thể
                    ModelState.AddModelError("", "Không tìm thấy món ăn cần cập nhật.");
                }
            }

            // Nếu có lỗi, hiển thị lại view cùng với thông báo lỗi
            return View(vs);
        }
        //ĐƯA RƯỚC


        //DANH SÁCH PHIẾU THUÊ PHÒNG
        public ActionResult lstRoomOrder()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Current = "LstRoomOrder";
            var lstOrder = db.tbl_PhieuThuePhong.ToList().AsEnumerable().Reverse().ToList();
            return View(lstOrder);
        }
        //Chỉnh sửa phiếu thuê phòng
        [HttpGet]
        public ActionResult ChinhSuaPhieuThue(string maPhieuThue)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var phieuThue = db.tbl_PhieuThuePhong.Where(i => i.MaPhieuThuePhong == maPhieuThue).FirstOrDefault();
            List<string> lstTrangThai = new List<string>()
            {
                "Chưa nhận phòng",
                "Đã nhận phòng",
                "Đã hủy",
                "Chưa xác nhận"
            };
            var maPhong = db.tbl_Phong.ToList();
            ViewBag.mphong = new SelectList(maPhong, "MaPhong", "SoPhong");
            ViewBag.TT = new SelectList(lstTrangThai);
            return View(phieuThue);
        }
        [HttpPost]
        public ActionResult ChinhSuaPhieuThue(string maPhieuThue, tbl_PhieuThuePhong phieuThue)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var maPhong = phieuThue.MaPhong;
            var maLoaiPhong = db.tbl_Phong.Where(i => i.MaPhong == maPhong).Select(i => i.MaLoaiPhong).FirstOrDefault();
            var donGia = db.tbl_LoaiPhong.Where(i => i.MaLoaiPhong == maLoaiPhong).Select(i => i.DonGia).FirstOrDefault();
            tbl_HoaDon hd = db.tbl_HoaDon.Where(i => i.MaPhieuThuePhong == maPhieuThue).FirstOrDefault();
            hd.TongTien = donGia;
            db.Entry(hd).State = System.Data.Entity.EntityState.Modified;
            db.Entry(phieuThue).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("lstRoomOrder");
        }

        public ActionResult ChiTietPhieuThue(string maPhieuThue)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var detail = db.tbl_PhieuThuePhong.Where(i => i.MaPhieuThuePhong == maPhieuThue).FirstOrDefault();
            return View(detail);
        }

        [HttpGet]
        public ActionResult XoaPhieuThue(string maPhieuThue)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (maPhieuThue == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var phieuThue = db.tbl_PhieuThuePhong.Where(r => r.MaPhieuThuePhong == maPhieuThue).FirstOrDefault();
            if (phieuThue == null)
            {
                return HttpNotFound();
            }
            return View(phieuThue);
        }
        [HttpPost, ActionName("XoaPhieuThue")]
        public ActionResult XacNhanXoaPhieuThue(string maPhieuThue)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            try
            {
                var phieuThue = db.tbl_PhieuThuePhong.Where(r => r.MaPhieuThuePhong == maPhieuThue).FirstOrDefault();
                db.tbl_PhieuThuePhong.Remove(phieuThue);
                db.SaveChanges();
                return RedirectToAction("lstRoomOrder");
            }
            catch
            {
                return Content("Không xóa được do có liên quan đến bảng khác");
            }
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
        public ActionResult XacNhanPhieuThue(string maPhieuThue)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var phieuThue = db.tbl_PhieuThuePhong.Where(i => i.MaPhieuThuePhong == maPhieuThue).FirstOrDefault();
            if (phieuThue.TrangThai == "Chưa xác nhận")
            {
                var maKH = phieuThue.MaKH;
                tbl_NhanVien nv = new tbl_NhanVien();
                nv = (tbl_NhanVien)Session["user"];
                string maHD = maHoaDon();
                var maPhong = phieuThue.MaPhong;
                var donGia = (from lp in db.tbl_LoaiPhong
                              join p in db.tbl_Phong on lp.MaLoaiPhong equals p.MaLoaiPhong
                              where p.MaPhong == maPhong
                              select lp.DonGia).FirstOrDefault();
                phieuThue.TrangThai = "Chưa nhận phòng";
                db.Entry(phieuThue).State = System.Data.Entity.EntityState.Modified;
                tbl_HoaDon hoaDon = new tbl_HoaDon
                {
                    MaHD = maHD,
                    NgayThanhToan = null,
                    TongTien = donGia,
                    MaKH = maKH,
                    MaPhieuThuePhong = maPhieuThue,
                    MaNV = nv.MaNV,
                    TrangThai = "Chưa thanh toán"
                };
                db.tbl_HoaDon.Add(hoaDon);
                db.SaveChanges();
                return RedirectToAction("lstRoomOrder");
            }
            return RedirectToAction("lstRoomOrder");
        }
        //KIỂM TRA PHÒNG TRỐNG
        [HttpGet]
        public JsonResult HienThiPhongTrong(DateTime dateS, DateTime dateE, string maPhong)
        {
            var room = db.tbl_Phong.Where(t => !(db.tbl_PhieuThuePhong
                    .Where(m => (m.TrangThai == "Chưa nhận phòng" || m.TrangThai == "Đã nhận phòng" || m.TrangThai == "Chưa xác nhận")
                                && m.NgayBatDauThue < dateE && m.NgayKetThucThue > dateS))
                    .Select(m => m.MaPhong)
                    .ToList().Contains(t.MaPhong)).Select(p => new
                    {
                        p.MaPhong,
                        p.SoPhong
                    })
            .ToList();

            var roomCurrent = db.tbl_Phong.Where(i => i.MaPhong == maPhong).Select(p => new
            {
                p.MaPhong,
                p.SoPhong
            }).FirstOrDefault();

            room.Insert(0, roomCurrent);

            return Json(room, JsonRequestBehavior.AllowGet);
        }

        //DANH SÁCH YÊU CẦU ĐẶT DỊCH VỤ
        public ActionResult DanhSachDatDoAn()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.current = "DanhSachDatDoAn";
            var lst = db.tbl_DichVuDaDat.Where(i => i.MaDV.StartsWith("DA")).ToList().AsEnumerable().Reverse().ToList();
            return View(lst);
        }
        public ActionResult XacNhanDonHang(string id)
        {
            DateTime systemTime = DateTime.Now; // Thời gian hệ thống
            TimeZoneInfo vietNamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vietNamTime = TimeZoneInfo.ConvertTime(systemTime, vietNamTimeZone);
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var donHang = db.tbl_DichVuDaDat.Where(i => i.ID == id).FirstOrDefault();
            if (donHang.MaTrangThaiDV == "TT01")
            {
                var maKH = donHang.MaKH;
                tbl_NhanVien nv = new tbl_NhanVien();
                nv = (tbl_NhanVien)Session["user"];
                string maHD = db.tbl_HoaDon
                         .Where(hd => hd.MaKH == maKH && hd.TrangThai == "Chưa thanh toán")
                         .OrderByDescending(hd => hd.MaHD)
                         .Select(hd => hd.MaHD)
                         .FirstOrDefault();
                var hoaDon = db.tbl_HoaDon.SingleOrDefault(hd => hd.MaHD == maHD);
                var phieuThue = db.tbl_PhieuThuePhong.Where(i => i.MaKH == maKH && i.NgayBatDauThue <= vietNamTime && vietNamTime <= i.NgayKetThucThue && (i.TrangThai != "Chưa xác nhận" || i.TrangThai != "Đã hủy")).OrderByDescending(i => i.MaPhieuThuePhong).FirstOrDefault();
                var donGia = db.tbl_DichVu.Where(i => i.MaDV == donHang.MaDV).Select(i => i.DonGia).FirstOrDefault();

                if (hoaDon != null)
                {
                    hoaDon.TongTien += donGia;
                }
                if (hoaDon == null)
                {
                    tbl_HoaDon hd = new tbl_HoaDon
                    {
                        MaHD = maHoaDon(),
                        NgayThanhToan = null,
                        TongTien = donGia,
                        MaKH = maKH,
                        MaPhieuThuePhong = phieuThue.MaPhieuThuePhong,
                        MaNV = nv.MaNV,
                        TrangThai = "Chưa thanh toán"
                    };
                    db.tbl_HoaDon.Add(hd);
                    donHang.MaHD = hd.MaHD;
                }
                donHang.MaTrangThaiDV = "TT02";
                donHang.MaNV = nv.MaNV;
                db.Entry(donHang).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhSachDatDoAn");
            }
            return RedirectToAction("DanhSachDatDoAn");
        }
        public ActionResult chiTietDonHang(string id)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var chiTiet = db.tbl_DichVuDaDat.Where(i => i.ID == id).FirstOrDefault();
            return View(chiTiet);
        }
        [HttpGet]
        public ActionResult suaDonHang(string id)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var hd = db.tbl_DichVuDaDat.Where(i => i.ID == id).FirstOrDefault();
            ViewBag.maHD = hd.MaHD;
            var maTT = db.tbl_TrangThaiDichVu.ToList();
            ViewBag.status = new SelectList(maTT, "MaTrangThaiDV", "TenTrangThai");
            var chiTidonHang = db.tbl_DichVuDaDat.Where(i => i.ID == id).FirstOrDefault();
            return View(chiTidonHang);
        }
        [HttpPost]
        public ActionResult suaDonHang(tbl_DichVuDaDat dv, string id)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            tbl_NhanVien nv = new tbl_NhanVien();
            nv = (tbl_NhanVien)Session["user"];
            dv.MaNV = nv.MaNV;
            if (dv.MaTrangThaiDV == "TT03")
            {
                var hd = db.tbl_HoaDon.Where(m => m.MaHD == dv.MaHD).FirstOrDefault();
                var donGia = db.tbl_DichVu.Where(i => i.MaDV == dv.MaDV).Select(i => i.DonGia).FirstOrDefault();
                hd.TongTien += donGia;
                db.Entry(hd).State = System.Data.Entity.EntityState.Modified;
            }
            db.Entry(dv).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DanhSachDatDoAn");
        }

        [HttpGet]
        public ActionResult XoaDonHang(string id)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var donhang = db.tbl_DichVuDaDat.Where(r => r.ID == id).FirstOrDefault();
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return View(donhang);
        }
        [HttpPost, ActionName("XoaDonHang")]
        public ActionResult XacNhanXoaDonHang(string id)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            try
            {
                var donhang = db.tbl_DichVuDaDat.Where(r => r.ID == id).FirstOrDefault();
                db.tbl_DichVuDaDat.Remove(donhang);
                db.SaveChanges();
                return RedirectToAction("DanhSachDatDoAn");
            }
            catch
            {
                return Content("Không xóa được do có liên quan đến bảng khác");
            }
        }

        //ĐẶT LỊCH SPA
        // https://localhost:44381/Admin/QuanLyDV/LichSpa
        public ActionResult LichSpa()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Current = "LichSpa";
            var lstSpa = db.tbl_DichVuDaDat.Where(i => i.MaDV.StartsWith("LD")).ToList().AsEnumerable().Reverse().ToList();
            return View(lstSpa);
        }
        public ActionResult XacNhanLichHen(string id)
        {

            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            DateTime systemTime = DateTime.Now; // Thời gian hệ thống
            TimeZoneInfo vietNamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vietNamTime = TimeZoneInfo.ConvertTime(systemTime, vietNamTimeZone);
            var donHang = db.tbl_DichVuDaDat.Where(i => i.ID == id).FirstOrDefault();
            if (donHang.MaTrangThaiDV == "TT01")
            {
                var maKH = donHang.MaKH;
                tbl_NhanVien nv = new tbl_NhanVien();
                nv = (tbl_NhanVien)Session["user"];
                string maHD = db.tbl_HoaDon
                         .Where(hd => hd.MaKH == maKH && hd.TrangThai == "Chưa thanh toán")
                         .OrderByDescending(hd => hd.MaHD)
                         .Select(hd => hd.MaHD)
                         .FirstOrDefault();
                var hoaDon = db.tbl_HoaDon.SingleOrDefault(hd => hd.MaHD == maHD);
                var phieuThue = db.tbl_PhieuThuePhong.Where(i => i.MaKH == maKH && i.NgayBatDauThue <= vietNamTime && vietNamTime <= i.NgayKetThucThue && (i.TrangThai != "Chưa xác nhận" || i.TrangThai != "Đã hủy")).OrderByDescending(i => i.MaPhieuThuePhong).FirstOrDefault();
                var donGia = db.tbl_DichVu.Where(i => i.MaDV == donHang.MaDV).Select(i => i.DonGia).FirstOrDefault();

                if (hoaDon != null)
                {
                    hoaDon.TongTien += donGia;
                }
                if (hoaDon == null)
                {
                    tbl_HoaDon hd = new tbl_HoaDon
                    {
                        MaHD = maHoaDon(),
                        NgayThanhToan = null,
                        TongTien = donGia,
                        MaKH = maKH,
                        MaPhieuThuePhong = phieuThue.MaPhieuThuePhong,
                        MaNV = nv.MaNV,
                        TrangThai = "Chưa thanh toán"
                    };
                    db.tbl_HoaDon.Add(hd);
                    donHang.MaHD = hd.MaHD;
                }
                donHang.MaTrangThaiDV = "TT02";
                donHang.MaNV = nv.MaNV;
                db.Entry(donHang).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("LichSpa");
            }
            return RedirectToAction("LichSpa");
        }
        public ActionResult ChiTietLichSpa(string id)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var chiTiet = db.tbl_DichVuDaDat.Where(i => i.ID == id).FirstOrDefault();
            return View(chiTiet);
        }
        [HttpGet]
        public ActionResult SuaLichHen(string id)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var hd = db.tbl_DichVuDaDat.Where(i => i.ID == id).FirstOrDefault();
            ViewBag.maHD = hd.MaHD;
            var maTT = db.tbl_TrangThaiDichVu.ToList();
            ViewBag.status = new SelectList(maTT, "MaTrangThaiDV", "TenTrangThai");
            var chiTidonHang = db.tbl_DichVuDaDat.Where(i => i.ID == id).FirstOrDefault();
            return View(chiTidonHang);
        }
        [HttpPost]
        public ActionResult SuaLichHen(tbl_DichVuDaDat dv, string id)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            tbl_NhanVien nv = new tbl_NhanVien();
            nv = (tbl_NhanVien)Session["user"];
            dv.MaNV = nv.MaNV;
            if (dv.MaTrangThaiDV == "TT03")
            {
                var hd = db.tbl_HoaDon.Where(m => m.MaHD == dv.MaHD).FirstOrDefault();
                var donGia = db.tbl_DichVu.Where(i => i.MaDV == dv.MaDV).Select(i => i.DonGia).FirstOrDefault();
                hd.TongTien += donGia;
                db.Entry(hd).State = System.Data.Entity.EntityState.Modified;
            }
            db.Entry(dv).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("LichSpa");
        }
        [HttpGet]
        public ActionResult ThemLichHen()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ThemLichHen(tbl_DichVuDaDat dv)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            db.tbl_DichVuDaDat.Add(dv);
            return RedirectToAction("LichSpa");
        }

        [HttpGet]
        public ActionResult XoaLichHen(string id)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var donhang = db.tbl_DichVuDaDat.Where(r => r.ID == id).FirstOrDefault();
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return View(donhang);
        }
        [HttpPost, ActionName("XoaLichHen")]
        public ActionResult XacNhanXoaLichHen(string id)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            try
            {
                var donhang = db.tbl_DichVuDaDat.Where(r => r.ID == id).FirstOrDefault();
                if (donhang.MaTrangThaiDV == "TT04")
                {
                    db.tbl_DichVuDaDat.Remove(donhang);
                    db.SaveChanges();
                    return RedirectToAction("LichSpa");
                }
                else
                {
                    @TempData["ErrorMessage"] = "Không thể xóa";
                    return RedirectToAction("LichSpa");
                }
            }
            catch
            {
                return Content("Không xóa được do có liên quan đến bảng khác");
            }
        }

        //DỊCH VỤ DỌN DẸP
        public ActionResult LichDonDep()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Current = "LichDonDep";

            var lst = db.tbl_DichVuDaDat.Where(i => i.MaDV.StartsWith("DD")).ToList().AsEnumerable().Reverse().ToList();
            return View(lst);
        }
        public ActionResult XacNhanLichDonDep(string id)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            DateTime systemTime = DateTime.Now; // Thời gian hệ thống
            TimeZoneInfo vietNamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vietNamTime = TimeZoneInfo.ConvertTime(systemTime, vietNamTimeZone);
            var donHang = db.tbl_DichVuDaDat.Where(i => i.ID == id).FirstOrDefault();
            if (donHang.MaTrangThaiDV == "TT01")
            {
                var maKH = donHang.MaKH;
                tbl_NhanVien nv = new tbl_NhanVien();
                nv = (tbl_NhanVien)Session["user"];
                string maHD = db.tbl_HoaDon
                         .Where(hd => hd.MaKH == maKH && hd.TrangThai == "Chưa thanh toán")
                         .OrderByDescending(hd => hd.MaHD)
                         .Select(hd => hd.MaHD)
                         .FirstOrDefault();
                var hoaDon = db.tbl_HoaDon.SingleOrDefault(hd => hd.MaHD == maHD);
                var phieuThue = db.tbl_PhieuThuePhong.Where(i => i.MaKH == maKH && i.NgayBatDauThue <= vietNamTime && vietNamTime <= i.NgayKetThucThue && (i.TrangThai != "Chưa xác nhận" || i.TrangThai != "Đã hủy")).OrderByDescending(i => i.MaPhieuThuePhong).FirstOrDefault();
                var donGia = db.tbl_DichVu.Where(i => i.MaDV == donHang.MaDV).Select(i => i.DonGia).FirstOrDefault();

                if (hoaDon != null)
                {
                    hoaDon.TongTien += donGia;
                }
                if (hoaDon == null)
                {
                    tbl_HoaDon hd = new tbl_HoaDon
                    {
                        MaHD = maHoaDon(),
                        NgayThanhToan = null,
                        TongTien = donGia,
                        MaKH = maKH,
                        MaPhieuThuePhong = phieuThue.MaPhieuThuePhong,
                        MaNV = nv.MaNV,
                        TrangThai = "Chưa thanh toán"
                    };
                    db.tbl_HoaDon.Add(hd);
                    donHang.MaHD = hd.MaHD;
                }
                donHang.MaTrangThaiDV = "TT02";
                donHang.MaNV = nv.MaNV;
                db.Entry(donHang).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("LichDonDep");
            }
            return RedirectToAction("LichDonDep");
        }
        [HttpGet]
        public ActionResult SuaLichDon(string id)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var hd = db.tbl_DichVuDaDat.Where(i => i.ID == id).FirstOrDefault();
            ViewBag.maHD = hd.MaHD;
            var maTT = db.tbl_TrangThaiDichVu.ToList();
            ViewBag.status = new SelectList(maTT, "MaTrangThaiDV", "TenTrangThai");
            var chiTidonHang = db.tbl_DichVuDaDat.Where(i => i.ID == id).FirstOrDefault();
            return View(chiTidonHang);
        }
        [HttpPost]
        public ActionResult SuaLichDon(tbl_DichVuDaDat dv, string id)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            tbl_NhanVien nv = new tbl_NhanVien();
            nv = (tbl_NhanVien)Session["user"];
            dv.MaNV = nv.MaNV;
            if (dv.MaTrangThaiDV == "TT03")
            {
                var hd = db.tbl_HoaDon.Where(m => m.MaHD == dv.MaHD).FirstOrDefault();
                var donGia = db.tbl_DichVu.Where(i => i.MaDV == dv.MaDV).Select(i => i.DonGia).FirstOrDefault();
                hd.TongTien += donGia;
                db.Entry(hd).State = System.Data.Entity.EntityState.Modified;
            }
            db.Entry(dv).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("LichDonDep");
        }
        public ActionResult ChiTietLichDon(string id)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var detail = db.tbl_DichVuDaDat.Where(i => i.ID == id).FirstOrDefault();
            return View(detail);
        }
        [HttpGet]
        public ActionResult XoaLichDon(string id)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var donhang = db.tbl_DichVuDaDat.Where(r => r.ID == id).FirstOrDefault();
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return View(donhang);
        }
        [HttpPost, ActionName("XoaLichDon")]
        public ActionResult XacNhanXoaLichDon(string id)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            try
            {
                var donhang = db.tbl_DichVuDaDat.Where(r => r.ID == id).FirstOrDefault();
                db.tbl_DichVuDaDat.Remove(donhang);
                db.SaveChanges();
                return RedirectToAction("LichDonDep");

            }
            catch
            {
                return Content("Không xóa được do có liên quan đến bảng khác");
            }
        }

        public ActionResult HoaDon()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Current = "HoaDon";
            var hoadon = db.tbl_HoaDon.ToList().AsEnumerable().Reverse().ToList();
            return View(hoadon);
        }

        public ActionResult ChiTietHoaDon(string maHD)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            var hoadon = db.tbl_HoaDon.Where(i => i.MaHD == maHD).FirstOrDefault();
            return View(hoadon);
        }
        [HttpGet]
        public ActionResult ChinhSuaHoaDon(string maHD)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            List<string> lstTrangThai = new List<string>()
            {
                "Chưa thanh toán",
                "Đã thanh toán",
                "Đã đặt cọc"
            };
            ViewBag.TT = new SelectList(lstTrangThai);
            var hd = db.tbl_HoaDon.Where(i => i.MaHD == maHD).FirstOrDefault();
            return View(hd);
        }
        [HttpPost]
        public ActionResult ChinhSuaHoaDon(tbl_HoaDon hd)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        [HttpGet]
        public ActionResult XoaHoaDon(string maHD)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            if (maHD == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var HD = db.tbl_HoaDon.Where(r => r.MaHD == maHD).FirstOrDefault();
            if (HD == null)
            {
                return HttpNotFound();
            }
            return View(HD);
        }
        [HttpPost, ActionName("XoaHoaDon")]
        public ActionResult XacNhanXoaHoaDon(string maHD)
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            try
            {
                var HD = db.tbl_HoaDon.Where(r => r.MaHD == maHD).FirstOrDefault();
                db.tbl_HoaDon.Remove(HD);
                db.SaveChanges();
                return RedirectToAction("HoaDon");

            }
            catch
            {
                return Content("Không xóa được do có liên quan đến bảng khác");
            }
        }
    }
}