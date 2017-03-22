using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LuckyNumber.Models;
using System.Text;
using LuckyNumber.SMSAPI;

namespace LuckyNumber.Controllers
{
    public class GuestController : Controller
    {
        LuckyNumContext db = new LuckyNumContext();   
        //
        // GET: /Guest/

        
        public ActionResult QuenMatKhau_Ac()
        {
            string taikhoan = Request.Form["phone"].ToString();

            Session["taikhoan"] = taikhoan;

            User user = db.Users.SingleOrDefault(x => x.username == taikhoan);

            if (user != null)
            {

                StringBuilder sb = new StringBuilder();
                char c;
                string c1;
                Random rand = new Random();
                for (int i = 0; i < 5; i++)
                {
                    c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 87)));
                    sb.Append(c);
                }
                c1 = sb.ToString();
                Session["maxacnhan"] = c1;

                SpeedSMSAPI api = new SpeedSMSAPI();
                String userInfo = api.getUserInfo();
                String response = api.sendSMS(user.phone, "Ma xac nhan cua ban la: " + c1, 2, "");

                return Redirect("~/Guest/XacNhanMatKhau");
            }


            return Json("Tài khoản của bạn không tồn tại", JsonRequestBehavior.AllowGet);
        }


        public ActionResult QuenMatKhau()
        {
            return View();
        }
        public ActionResult XacNhanMatKhau()
        {
            return View();
        }


        public ActionResult test()
        {
            return View();
        }

        public ActionResult ThayDoiMatKhau()
        {
            string maXN = Request.Form["phone"].ToString();
            string passNew = Request.Form["password"].ToString();


            string taikhoan = Session["taikhoan"].ToString();
            string maxacnhan = Session["maxacnhan"].ToString();

            User user = db.Users.SingleOrDefault(x => x.username == taikhoan);

            if (maXN == maxacnhan)
            {
                user.password = passNew;
                db.SaveChanges();
                return Redirect("~/User/Index");
            }

            return Json("Mã xác nhận không chính xác", JsonRequestBehavior.AllowGet);
        }
	}
}