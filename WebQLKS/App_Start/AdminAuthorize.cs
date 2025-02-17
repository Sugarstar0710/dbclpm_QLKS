using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebQLKS.Models;

namespace WebQLKS.App_Start
{
    public class AdminAuthorize : AuthorizeAttribute
    {
        public string maCV { get;set; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            tbl_NhanVien nvLeTanSession = (tbl_NhanVien) HttpContext.Current.Session["user"];
            if(nvLeTanSession != null)
            {
                DAQLKSEntities db = new DAQLKSEntities();
                var count = db.tbl_NhanVien.Count(nv => nv.MaNV== nvLeTanSession.MaNV && nv.MaCV==maCV);
                if(count != 0 )
                {
                    return;
                }
                else
                {
                    var returnURL = filterContext.HttpContext.Request.RawUrl;
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        Controller = "BaoLoi",
                        action = "KhongCoQuyen",
                        area = "Admin",
                        returnUrl = returnURL.ToString()
                    }));
                }
            }

        }
    }
}