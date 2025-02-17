using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebQLKS.Models;
using System.Transactions;
using System.Threading.Tasks;
using System.Threading;

namespace WebQLKS.Controllers
{
    public class RoomController : Controller
    {
        DAQLKSEntities database = new DAQLKSEntities();
        // GET: Room
        public ActionResult CategoryRoom()
        {
            var room = database.tbl_LoaiPhong.ToList();
            var tienIchDict = new Dictionary<string, List<string>>();
            foreach (var item in room)
            {
                var maLoaiPhong = item.MaLoaiPhong;
                var lstTienIch = database.tbl_ChiTietPhong.Where(ct => ct.MaLoaiPhong == maLoaiPhong).Select(ct => ct.TienIch).Distinct().ToList();
                tienIchDict[maLoaiPhong] = lstTienIch;
            }
            ViewBag.TienIch = tienIchDict;
            return View(room);
        }
        //Load Phòng Theo Loại Phòng
        public ActionResult DetailRoom(string MaLoaiPhong)
        {
            Session["MaLoaiPhong"] = MaLoaiPhong;
            ViewBag.imgLoaiPhong = database.tbl_Phong.Where(ha => ha.MaLoaiPhong == MaLoaiPhong).ToList();
            if ((MaLoaiPhong.ToString().Trim() == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ten = database.tbl_LoaiPhong.Where(r => r.MaLoaiPhong == MaLoaiPhong).Select(r => r.TenLoaiPhong).FirstOrDefault();
            var donGia = database.tbl_LoaiPhong.Where(dg => dg.MaLoaiPhong == MaLoaiPhong).Select(dg => dg.DonGia).FirstOrDefault();
            var moTa = database.tbl_LoaiPhong.Where(lp => lp.MaLoaiPhong == MaLoaiPhong).Select(lp => lp.MoTa).FirstOrDefault();
            if (moTa == null)
            {
                return HttpNotFound("Không tìm thấy thông tin phòng");
            }
            var tienIch = database.tbl_ChiTietPhong.Where(ct => ct.MaLoaiPhong == MaLoaiPhong).Select(ct => ct.TienIch).Distinct().ToList();
            var viewModel = new RoomDetailViewModel
            {
                maPhong = MaLoaiPhong,
                tenPhong = ten,
                donGia = donGia,
                MoTa = moTa,
                TienIch = tienIch
            };
            return View(viewModel);
        }
        // GET: TimPhong/Create
        [HttpGet]
        public ActionResult TimPhong(string MaLoaiPhong)
        {
            DateTime systemTime = DateTime.Now; // Thời gian hệ thống
            TimeZoneInfo vietNamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vietNamTime = TimeZoneInfo.ConvertTime(systemTime, vietNamTimeZone);
            var model = new BookingViewModel
            {
                MaLoaiPhong = Session["MaLoaiPhong"].ToString(),
                dateStart = vietNamTime,
                dateEnd = vietNamTime.AddDays(1)
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult TimPhong(string dateStart, string dateEnd, string MaLoaiPhong)
        {
            DateTime systemTime = DateTime.Now; // Thời gian hệ thống
            TimeZoneInfo vietNamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vietNamTime = TimeZoneInfo.ConvertTime(systemTime, vietNamTimeZone);
            List<tbl_Phong> lst = new List<tbl_Phong>();

            Session["Check-in"] = dateStart;
            Session["Check-out"] = dateEnd;
            if (string.IsNullOrEmpty(dateStart) || string.IsNullOrEmpty(dateEnd))
            {
                lst = database.tbl_Phong.ToList();
            }
            else
            {
                DateTime dateS, dateE;
                bool isDateStartValid = DateTime.TryParseExact(dateStart, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateS);
                bool isDateEndValid = DateTime.TryParseExact(dateEnd, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateE);

                if (!isDateStartValid || !isDateEndValid)
                {
                    ModelState.AddModelError("", "Định dạng ngày không hợp lệ. Vui lòng nhập ngày theo định dạng yyyy/MM/dd.");
                    return View(new BookingViewModel { dateStart = vietNamTime, dateEnd = vietNamTime.AddDays(1) });
                }
                if (dateE < dateS)
                {
                    TempData["DateError"] = "Ngày check-out phải lớn hơn ngày Check-in";
                    return RedirectToAction("TimPhong");
                }
                if (dateS < DateTime.Today)
                {
                    TempData["DateError"] = "Ngày check-in không được nhỏ hơn ngày hiện tại";
                    return RedirectToAction("TimPhong");
                }

                dateS = dateS.AddHours(12);
                dateE = dateE.AddHours(12);

                lst = database.tbl_Phong.Where(t => !(database.tbl_PhieuThuePhong
                    .Where(m => (m.TrangThai == "Chưa nhận phòng" || m.TrangThai == "Đã nhận phòng" || m.TrangThai == "Chưa xác nhận")
                                && m.NgayBatDauThue < dateE && m.NgayKetThucThue > dateS))
                    .Select(m => m.MaPhong)
                    .ToList().Contains(t.MaPhong) && t.MaLoaiPhong == MaLoaiPhong).ToList();

                ViewData["test"] = lst;
            }

            return View("DanhSachPhongTrong", lst);
        }
        private string maPhieuThue()
        {
            var lastBooking = database.tbl_PhieuThuePhong.OrderByDescending(p => p.MaPhieuThuePhong).FirstOrDefault();
            if (lastBooking != null)
            {
                int MPT = int.Parse(lastBooking.MaPhieuThuePhong.Substring(2));
                int nextMPT = MPT + 1;
                if (nextMPT >= 10)
                {
                    return "PT" + nextMPT.ToString();
                }
                return "PT0" + nextMPT.ToString();
            }
            return "PT01";
        }
        private string maHoaDon()
        {
            var lastBill = database.tbl_HoaDon.OrderByDescending(p => p.MaHD).FirstOrDefault();
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
        public bool IsRoomAvailable(string roomId, DateTime checkIn, DateTime checkOut)
        {
            var existingBookings = database.tbl_PhieuThuePhong
                .Where(b => b.MaPhong == roomId &&
                            ((b.NgayBatDauThue <= checkIn && b.NgayKetThucThue >= checkIn) ||
                             (b.NgayBatDauThue <= checkOut && b.NgayKetThucThue >= checkOut) ||
                             (b.NgayBatDauThue >= checkIn && b.NgayKetThucThue <= checkOut)) && b.TrangThai != "Đã hủy")
                .ToList();

            return !existingBookings.Any();
        }

        [HttpGet]
        public ActionResult DatPhong(string maPhong)
        {
            if (Session["KH"] == null)
            {
                Session["PreviousUrl"] = Request.Url.AbsoluteUri;
                TempData["SessionKhNull"] = "Vui lòng đăng nhập để tiếp tục đặt phòng";
                return RedirectToAction("LoginAcountKH", "LoginAcount");
            }
            Session["MP"] = maPhong;
            ViewBag.MP = maPhong;
            return View();
        }
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        [HttpPost]
        public async Task<ActionResult> DatPhong(string maPhong, int SLK, int SLNN)
        {
            string maphong = Session["MP"].ToString();
            if (Session["KH"] == null)
            {
                TempData["SessionKhNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("LoginAcountKH", "LoginAcount");
            }
            if (SLK <= 0 || SLNN < 0)
            {
                Session["PreviousUrl"] = Request.Url.AbsoluteUri;
                TempData["ErrorMessage"] = "Số lượng không được nhỏ hơn 0!";
                return RedirectToAction("DatPhong", new { maPhong = maphong });
            }
            if (SLK < SLNN)
            {
                TempData["ErrorMessage"] = "Số lượng khách nước ngoài phải nhỏ hơn tổng số khách!";
                return RedirectToAction("DatPhong", new { maPhong = maphong });
            }

            // Đảm bảo rằng chỉ một yêu cầu đặt phòng được xử lý tại một thời điểm
            await _semaphoreSlim.WaitAsync();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (ModelState.IsValid)
                    {
                        DateTime checkIn = DateTime.Parse(Session["Check-in"].ToString());
                        DateTime checkOut = DateTime.Parse(Session["Check-out"].ToString());

                        if (!IsRoomAvailable(maPhong, checkIn, checkOut))
                        {
                            TempData["ErrorMessage"] = "Phòng này đã được đặt trong khoảng thời gian bạn chọn.";
                            return RedirectToAction("DatPhong", new { maPhong = maphong });
                        }

                        string maPT = maPhieuThue();
                        var donGia = (from lp in database.tbl_LoaiPhong
                                      join p in database.tbl_Phong on lp.MaLoaiPhong equals p.MaLoaiPhong
                                      where p.MaPhong == maPhong
                                      select lp.DonGia).FirstOrDefault();

                        tbl_PhieuThuePhong phieuThuePhong = new tbl_PhieuThuePhong
                        {
                            MaPhieuThuePhong = maPT,
                            MaPhong = maPhong,
                            NgayBatDauThue = checkIn.Date,
                            NgayKetThucThue = checkOut.Date,
                            SLKhach = SLK,
                            SLKhachNuocNgoai = SLNN,
                            TrangThai = "Chưa xác nhận",
                            MaKH = Session["KH"].ToString(),
                            MaNV = null
                        };

                        database.tbl_PhieuThuePhong.Add(phieuThuePhong);
                        await database.SaveChangesAsync();

                        scope.Complete();

                        TempData["SuccessMessage"] = "Đặt phòng thành công!";
                        return RedirectToAction("DatPhong", "Room");
                    }

                    TempData["ErrorMessage"] = "Đặt phòng thất bại. Vui lòng thử lại.";
                    return RedirectToAction("DatPhong", "Room");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.ToString();
                TempData["ErrorMessage"] = "Đặt phòng thất bại. Vui lòng thử lại.";
                return RedirectToAction("DatPhong", "Room");
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

    }
}