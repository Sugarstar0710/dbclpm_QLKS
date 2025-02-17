using Newtonsoft.Json.Linq;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WebQLKS.Models;

namespace WebQLKS.Controllers
{
    public class AccountController : Controller
    {
        DAQLKSEntities db = new DAQLKSEntities();
        // GET: Account
        public ActionResult historyService()
        {
            if (Session["KH"] == null)
            {
                TempData["SessionKhNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("LoginAcountKH", "LoginAcount");
            }
            var maKH = Session["KH"].ToString();
            var service = db.tbl_DichVuDaDat.Where(s => s.MaKH == maKH).ToList().AsEnumerable().Reverse().ToList();
            if (service == null)
            {
                ViewBag.Notification = "Quý khách chưa sử dụng dịch vụ nào";
                return RedirectToAction("historyService", "Account");
            }
            return View(service);
        }
        public ActionResult historyOrdRoom()
        {
            if (Session["KH"] == null)
            {
                TempData["SessionKhNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("LoginAcountKH", "LoginAcount");
            }
            var maKH = Session["KH"].ToString();
            var HD = db.tbl_PhieuThuePhong.Where(hd => hd.MaKH == maKH).ToList().AsEnumerable().Reverse().ToList();
            if (HD == null)
            {
                ViewBag.Notification = "Quý khách chưa đặt phòng nào";
                return RedirectToAction("historyOrdRoom", "Account");
            }
            return View(HD);
        }
        public ActionResult ChiTietPhieuThue(string maPT)
        {
            if (Session["KH"] == null)
            {
                TempData["SessionKhNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("LoginAcountKH", "LoginAcount");
            }
            var phieuThue = db.tbl_PhieuThuePhong.Where(a => a.MaPhieuThuePhong == maPT).FirstOrDefault();
            return View(phieuThue);
        }
        public ActionResult ChiTietDichVu(string id)
        {
            if (Session["KH"] == null)
            {
                TempData["SessionKhNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("LoginAcountKH", "LoginAcount");
            }
            var chitiet = db.tbl_DichVuDaDat.Where(a => a.ID == id).FirstOrDefault();
            return View(chitiet);
        }
        public ActionResult Bill()
        {
            if (Session["KH"] == null)
            {
                TempData["SessionKhNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("LoginAcountKH", "LoginAcount");
            }
            return View();
        }
        public ActionResult Logout()
        {
            if (Session["KH"] == null)
            {
                TempData["SessionKhNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("LoginAcountKH", "LoginAcount");
            }
            Session["KH"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult UserInfor()
        {
            if (Session["KH"] == null)
            {
                TempData["SessionKhNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("LoginAcountKH", "LoginAcount");
            }
            var user = Session["KH"].ToString();
            if (user == null)
            {
                return RedirectToAction("LoginAcountKH", "LoginAcount");
            }
            else
            {
                var khachhang = db.tbl_KhachHang.Where(s => s.MaKH == user).FirstOrDefault();
                return View(khachhang);
            }
        }
        [HttpGet]
        public ActionResult EditUser(string makh)
        {
            if (Session["KH"] == null)
            {
                TempData["SessionKhNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("LoginAcountKH", "LoginAcount");
            }
            var user = db.tbl_KhachHang.Where(s => s.MaKH == makh).FirstOrDefault();
            ViewBag.khachhang = user;
            return View(db.tbl_KhachHang.Where(s => s.MaKH == makh).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult EditUser(string makh, tbl_KhachHang kh)
        {
            if (Session["KH"] == null)
            {
                TempData["SessionKhNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("LoginAcountKH", "LoginAcount");
            }
            if (kh.QuocTich == "Việt Nam")
            {
                kh.MaLoaiKH = 2;
            }
            else
            {
                kh.MaLoaiKH = 1;
            }
            db.Entry(kh).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("UserInfor", "Account");
        }

        public ActionResult HuyDatPhong(string maPT)
        {
            if (Session["KH"] == null)
            {
                TempData["SessionKhNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("LoginAcountKH", "LoginAcount");
            }
            if (ModelState.IsValid)
            {
                var phieuthue = db.tbl_PhieuThuePhong.Where(s => s.MaPhieuThuePhong == maPT).FirstOrDefault();
                if (phieuthue.TrangThai == "Chưa xác nhận")
                {
                    phieuthue.TrangThai = "Đã hủy";
                    db.Entry(phieuthue).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("historyOrdRoom", "Account");
            }
            return View();
        }
        public ActionResult HuyDichVu(string id)
        {
            if (Session["KH"] == null)
            {
                TempData["SessionKhNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("LoginAcountKH", "LoginAcount");
            }
            if (ModelState.IsValid)
            {
                var phieudichvu = db.tbl_DichVuDaDat.Where(s => s.ID == id).FirstOrDefault();
                phieudichvu.MaTrangThaiDV = "TT04";
                db.Entry(phieudichvu).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("historyService", "Account");
            }
            return View();
        }
        //HÓA ĐƠN
        public ActionResult XemHoaDon()
        {
            if (Session["KH"] == null)
            {
                TempData["SessionKhNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("LoginAcountKH", "LoginAcount");
            }
            var maKH = Session["KH"].ToString();
            var hoadon = db.tbl_HoaDon.Where(i => i.MaKH == maKH).ToList().AsEnumerable().Reverse().ToList();
            return View(hoadon);
        }
        public ActionResult ChiTietHoaDon(string maHD)
        {
            if (Session["KH"] == null)
            {
                TempData["SessionKhNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("LoginAcountKH", "LoginAcount");
            }
            var hd = db.tbl_HoaDon.Where(i => i.MaHD == maHD).FirstOrDefault();
            Session["HD"] = hd;
            return View(hd);
        }


        public ActionResult FailureView()
        {

            return View();
        }
        public ActionResult SuccessView()
        {
            return View();
        }
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Account/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (PayPal.HttpException ex)
            {
                string logFilePath = Server.MapPath("C:\\Users\\PHONG VAN PC\\OneDrive\\Máy tính\\test.txt\"");
                System.IO.File.AppendAllText(logFilePath, DateTime.Now.ToString() + ": " + ex.Message + Environment.NewLine);
                return View("FailureView");
            }

            //on successful payment, show success page to user.
            DateTime systemTime = DateTime.Now; // Thời gian hệ thống
            TimeZoneInfo vietNamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vietNamTime = TimeZoneInfo.ConvertTime(systemTime, vietNamTimeZone);
            tbl_HoaDon hoaDon = new tbl_HoaDon();
            hoaDon = (tbl_HoaDon)Session["HD"];
            tbl_HoaDon hd = db.tbl_HoaDon.Where(i => i.MaHD == hoaDon.MaHD).FirstOrDefault();
            hd.TrangThai = "Đã thanh toán";
            hd.NgayThanhToan = vietNamTime;
            db.Entry(hd).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var hoaDon = Session["HD"] as tbl_HoaDon;
            double subTotal = (double)hoaDon.TongTien;
            double convertUSD = Math.Round(subTotal / 25380, 2);
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = hoaDon.MaPhieuThuePhong,
                currency = "USD",
                price = convertUSD.ToString(CultureInfo.InvariantCulture),
                quantity = "1",
                sku = hoaDon.MaHD
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = convertUSD.ToString(CultureInfo.InvariantCulture)
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = convertUSD.ToString(CultureInfo.InvariantCulture), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }
    }
}