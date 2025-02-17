using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebQLKS.Areas.Admin.Controllers
{
    public class BaoLoiController : Controller
    {
        // GET: Admin/BaoLoi
        public ActionResult KhongCoQuyen()
        {
            if (Session["user"] == null)
            {
                TempData["SessionNull"] = "Phiên đăng nhập đã hết hạn. Hãy đăng nhập lại để tiếp tục";
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
    }
}