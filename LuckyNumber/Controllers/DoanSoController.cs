using LuckyNumber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace LuckyNumber.Controllers
{
    public class DoanSoController : Controller
    {
        //
        LuckyNumContext db = new LuckyNumContext();
        // GET: /DoanSo/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DoanSoPage()
        {
            
            string name = Session["userName"].ToString();
            ViewBag.Name = name;
            string day = DateTime.Now.Day.ToString();
            string month = DateTime.Now.Month.ToString();
            string year = DateTime.Now.Year.ToString();



            string time = DateTime.Parse("11:00 PM").ToString("t");
            string timeNow = DateTime.Now.ToString("t");

            int userID = int.Parse(Session["IDs"].ToString());
            User user = db.Users.SingleOrDefault(x => x.ID == userID);

            DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
            CuocChoi cuocchoi = db.CuocChois.SingleOrDefault(x => x.NgayDoanSo == datetime);

            if (cuocchoi == null) return Redirect("~/DoanSo/Error5");

            //if (DateTime.Compare(DateTime.Parse(timeNow), DateTime.Parse(time)) > 0)
            //{
            //    return Redirect("~/DoanSo/Error3");
            //}
            //if (DateTime.Compare(DateTime.Parse(timeNow), DateTime.Parse(time)) > 0)
            //    return Redirect("~/DoanSo/Error3");

            if (user.soluotchoi > 0 && user.xacnhan == true && /*((DateTime.Compare(DateTime.Parse(timeNow), DateTime.Parse(time)) < 0)||*/cuocchoi.TrangThai==true)
            {
                
                return View();
            }

            else if (user.soluotchoi > 0 && user.xacnhan == true && /*(DateTime.Compare(DateTime.Parse(timeNow), DateTime.Parse(time)) > 0)*/cuocchoi.TrangThai==false) return Redirect("~/DoanSo/Error3");
            else if (user.soluotchoi <= 0 && user.xacnhan == true) return Redirect("~/DoanSo/Error1");

            else return Redirect("~/DoanSo/Error2");
            
        }

        public ActionResult Error1()
        {
            return View();
        }
        //public ActionResult KiemTraLuotChoi()
        //{
        //    int userID = int.Parse(Session["IDs"].ToString());
        //    User user = db.Users.SingleOrDefault(x => x.ID == userID);
            //if (user.soluotchoi > 0)
            //{
        //        return Redirect("~/DoanSo/DoanSoPage");
        //    }
        //    else return Redirect("~/DoanSo/Error_1");
        //}

        public ActionResult Error2()
        {
            return View();
        }

        public ActionResult Error3()
        {
            return View();
        }

        public ActionResult Error4()
        {
            return View();
        }

        public ActionResult Error5()
        {
            return View();
        }


        bool isNumber(string scr)
        {
            int strLength = scr.Length;
            for (int i = 0; i < strLength; i++)
            {
                if (scr[i] < '0' || scr[i] > '9')
                    return false;
            }
            return true;
        }

        public ActionResult DoanSo(ChiTietCuocChoi chitietcuocchoi)
        {
            string name = Session["userName"].ToString();
            ViewBag.Name = name;
            int userID = int.Parse(Session["IDs"].ToString());
            User user = db.Users.SingleOrDefault(x => x.ID == userID);

            

            string day = DateTime.Now.Day.ToString();
            string month = DateTime.Now.Month.ToString();
            string year = DateTime.Now.Year.ToString();

            DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));


            CuocChoi cuocchoi = db.CuocChois.SingleOrDefault(x => x.NgayDoanSo == datetime);

            if (cuocchoi == null) return Redirect("~/DoanSo/Error5"); 

            int machoi = int.Parse(cuocchoi.MaCuocChoi.ToString());

            string duDoan = Request.Form["SoDuDoan"].ToString();
            if (isNumber(duDoan) == false) return Json("Số dự đoán không hợp lệ", JsonRequestBehavior.AllowGet);

            int soDuDoan = int.Parse(duDoan);
            if (soDuDoan < 0 || soDuDoan > 999) return Json("Số dự đoán phải nằm trong khoảng 0-999", JsonRequestBehavior.AllowGet);
            else
            {
                ChiTietCuocChoi chitiet2 = db.ChiTietCuocChois.SingleOrDefault(x => x.SoDuDoan == soDuDoan && x.MaCuocChoi == machoi && x.UserID == userID);
                if (chitiet2 != null)
                {
                    return Redirect("~/DoanSo/Error4");
                }

                else
                {
                    Session["soDuDoan"] = soDuDoan;
                    chitietcuocchoi.UserID = int.Parse(Session["IDs"].ToString());
                    chitietcuocchoi.MaCuocChoi = machoi;
                    db.ChiTietCuocChois.Add(chitietcuocchoi);
                    user.soluotchoi--;
                    Session["soLuotChoi"] = user.soluotchoi;
                    db.SaveChanges();



                    return Redirect("~/DoanSo/ThongBaoPage");

                }
            }
            
        }

        public ActionResult ThongBaoPage()
        {
           
            string day = DateTime.Now.Day.ToString();
            string month = DateTime.Now.Month.ToString();
            string year = DateTime.Now.Year.ToString();
    
            DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));


         
            CuocChoi cuocchoi = db.CuocChois.SingleOrDefault(x=>x.NgayDoanSo == datetime);
   
            int machoi = int.Parse(cuocchoi.MaCuocChoi.ToString());



            DanhSachTrungThuong danhsach = db.DanhSachTrungThuongs.SingleOrDefault(x => x.MaCuocChoi == machoi);
            string TienThuong = danhsach.TongTienThuong.ToString();
     

            int userID = int.Parse(Session["IDs"].ToString());
            int soDuDoans = int.Parse(Session["soDuDoan"].ToString());
            ChiTietCuocChoi chitiet = db.ChiTietCuocChois.SingleOrDefault(x => x.UserID == userID && x.MaCuocChoi == machoi && x.SoDuDoan == soDuDoans);
            
            int soDuDoan = chitiet.SoDuDoan;
            var tongSoLan = from u in db.ChiTietCuocChois
                            where u.MaCuocChoi == machoi
                            group u by u.SoDuDoan into Counted
                            select new
                            {
                                soDuDoan = Counted.Key,
                                soLan = Counted.Count()
                            };
            var soLanTheoUser = (from y in tongSoLan
                                where y.soDuDoan == soDuDoan
                                select y).Single();
            int soLan = soLanTheoUser.soLan;
            ViewBag.soLan = soLan;

            int soLanItNhat = tongSoLan.Min(x => x.soLan);
            var tongSoLanItNhat = from t in tongSoLan
                                  where t.soLan == soLanItNhat
                                  select t;
                //int tongSoItNhat = soLanItNhat;
            int tongSoItNhat = tongSoLanItNhat.Count();
            ViewBag.soLanItNhat = tongSoItNhat;

            int soConLai = 1000 - tongSoLan.Count();
            ViewBag.soConLai = soConLai;


            ViewBag.tienThuong = TienThuong;

            return View();
        }
	}
}