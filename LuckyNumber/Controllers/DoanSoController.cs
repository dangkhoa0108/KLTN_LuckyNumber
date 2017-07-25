using LuckyNumber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LuckyNumber.ViewModel;
using System.Net;
using System.Data.Entity;

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
            if (Session["userName"] != null && Session["Role"].ToString() == "User")
            {
                if (int.Parse(Session["diemdanh"].ToString()) == 0)
                {
                    string name = Session["userName"].ToString();
                    ViewBag.Name = name;
                    if (Session["tt"] != null)
                    {
                        string tt = Session["tt"].ToString();
                        ViewBag.ttt = tt;
                    }
                    else ViewBag.ttt = null;
                

                    //string day = DateTime.Now.Day.ToString();
                    //string month = DateTime.Now.Month.ToString();
                    //string year = DateTime.Now.Year.ToString();

                    DateTime serverTime = DateTime.Now;
                    DateTime utcTime = DateTime.UtcNow;

                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
                    string timeNow = localTime.ToString("t");

                    ////////////////////////////////////

                    string day = localTime.ToString("dd");
                    string month = localTime.ToString("MM");
                    string year = localTime.ToString("yyyy");




                    //string timeNow = DateTime.Now.ToString("t");

                    int userID = int.Parse(Session["IDs"].ToString());
                    User user = db.Users.SingleOrDefault(x => x.ID == userID);

                    DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
                    CuocChoi cuocchoi = db.CuocChois.FirstOrDefault(x => x.NgayDoanSo == datetime);

                    if (cuocchoi == null) return Redirect("~/DoanSo/Error5");

                    int soluotchoi = int.Parse(user.soluotchoi.ToString());
                    int soluotchoi_km = int.Parse(user.soluotchoi_km.ToString());
                    int sumluotchoi = soluotchoi + soluotchoi_km;
                    Session["km_view"] = soluotchoi_km;
                    Session["ct_view"] = soluotchoi;

                    //if (DateTime.Compare(DateTime.Parse(timeNow), DateTime.Parse(time)) > 0)
                    //{
                    //    return Redirect("~/DoanSo/Error3");
                    //}
                    //if (DateTime.Compare(DateTime.Parse(timeNow), DateTime.Parse(time)) > 0)
                    //    return Redirect("~/DoanSo/Error3");

                    if (sumluotchoi > 0 && user.xacnhan == true && cuocchoi.TrangThai == true)
                    {

                        return View();
                    }

                    else if (sumluotchoi > 0 && user.xacnhan == true && cuocchoi.TrangThai == false) return Redirect("~/DoanSo/Error3");
                    else if (sumluotchoi <= 0 && user.xacnhan == true) return Redirect("~/DoanSo/Error1");

                    else return Redirect("~/DoanSo/Error2");
                }
                else return Redirect("~/User/DiemDanh");
            }
            else return Redirect("~/User/Login");
            
        }

        public ActionResult BaoLo10Page()
        {
            if (Session["userName"] != null && Session["Role"].ToString() == "User")
            {
                if (int.Parse(Session["diemdanh"].ToString()) == 0)
                {
                    string name = Session["userName"].ToString();
                    ViewBag.Name = name;
                    if (Session["tt"] != null)
                    {
                        string tt = Session["tt"].ToString();
                        ViewBag.ttt = tt;
                    }
                    else ViewBag.ttt = null;
                    //string day = DateTime.Now.Day.ToString();
                    //string month = DateTime.Now.Month.ToString();
                    //string year = DateTime.Now.Year.ToString();

                    DateTime serverTime = DateTime.Now;
                    DateTime utcTime = DateTime.UtcNow;

                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
                    string timeNow = localTime.ToString("t");

                    ////////////////////////////////////

                    string day = localTime.ToString("dd");
                    string month = localTime.ToString("MM");
                    string year = localTime.ToString("yyyy");



                    string time = DateTime.Parse("11:00 PM").ToString("t");
                    //string timeNow = DateTime.Now.ToString("t");

                    int userID = int.Parse(Session["IDs"].ToString());
                    User user = db.Users.SingleOrDefault(x => x.ID == userID);

                    int soluotchoi = int.Parse(user.soluotchoi.ToString());
                    int soluotchoi_km = int.Parse(user.soluotchoi_km.ToString());
                    int sumluotchoi = soluotchoi + soluotchoi_km;
                    Session["km_view"] = soluotchoi_km;
                    Session["ct_view"] = soluotchoi;
                    DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
                    CuocChoi cuocchoi = db.CuocChois.SingleOrDefault(x => x.NgayDoanSo == datetime);
                    if (cuocchoi == null) return Redirect("~/DoanSo/Error5");
                    if (sumluotchoi >= 10 && user.xacnhan == true && /*((DateTime.Compare(DateTime.Parse(timeNow), DateTime.Parse(time)) < 0)||*/cuocchoi.TrangThai == true)
                    {

                        return View();
                    }

                    else if (sumluotchoi >= 10 && user.xacnhan == true && /*(DateTime.Compare(DateTime.Parse(timeNow), DateTime.Parse(time)) > 0)*/cuocchoi.TrangThai == false) return Redirect("~/DoanSo/Error3");
                    else if (sumluotchoi < 10 && user.xacnhan == true) return Redirect("~/DoanSo/Error1");

                    else return Redirect("~/DoanSo/Error2");
                }
                else return Redirect("~/User/DiemDanh");
            }
            else return Redirect("~/User/Login");
        }

        public ActionResult BaoLo100Page()
        {
            if (Session["userName"] != null && Session["Role"].ToString() == "User")
            {
                if (int.Parse(Session["diemdanh"].ToString()) == 0)
                {
                    string name = Session["userName"].ToString();
                    ViewBag.Name = name;
                    if (Session["tt"] != null)
                    {
                        string tt = Session["tt"].ToString();
                        ViewBag.ttt = tt;
                    }
                    else ViewBag.ttt = null;
                    //string day = DateTime.Now.Day.ToString();
                    //string month = DateTime.Now.Month.ToString();
                    //string year = DateTime.Now.Year.ToString();

                    DateTime serverTime = DateTime.Now;
                    DateTime utcTime = DateTime.UtcNow;

                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);

                    ////////////////////////////////////

                    string day = localTime.ToString("dd");
                    string month = localTime.ToString("MM");
                    string year = localTime.ToString("yyyy");




                    //string timeNow = DateTime.Now.ToString("t");

                    int userID = int.Parse(Session["IDs"].ToString());
                    User user = db.Users.SingleOrDefault(x => x.ID == userID);

                    int soluotchoi = int.Parse(user.soluotchoi.ToString());
                    int soluotchoi_km = int.Parse(user.soluotchoi_km.ToString());
                    int sumluotchoi = soluotchoi + soluotchoi_km;
                    Session["km_view"] = soluotchoi_km;
                    Session["ct_view"] = soluotchoi;
                    DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
                    CuocChoi cuocchoi = db.CuocChois.SingleOrDefault(x => x.NgayDoanSo == datetime);
                    if (cuocchoi == null) return Redirect("~/DoanSo/Error5");
                    if (sumluotchoi >= 100 && user.xacnhan == true && cuocchoi.TrangThai == true)
                    {

                        return View();
                    }

                    else if (sumluotchoi >= 100 && user.xacnhan == true && /*(DateTime.Compare(DateTime.Parse(timeNow), DateTime.Parse(time)) > 0)*/cuocchoi.TrangThai == false) return Redirect("~/DoanSo/Error3");
                    else if (sumluotchoi < 100 && user.xacnhan == true) return Redirect("~/DoanSo/Error1");

                    else return Redirect("~/DoanSo/Error2");
                }
                else return Redirect("~/User/DiemDanh");
            }
            else return Redirect("~/User/Login");
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

        public ActionResult BaoLoDauCuoi()
        {

            List<DanhSachSoDaDoanViewModel> ds = new List<DanhSachSoDaDoanViewModel>();
            List<DanhSachSoChuaDoanViewModel> ds2 = new List<DanhSachSoChuaDoanViewModel>();
            List<object> oj = new List<object>();
            oj.Add(ds);
            oj.Add(ds2);
            if (Session["userName"] != null)
            {
                string name = Session["userName"].ToString();
                ViewBag.Name = name;
                int userID = int.Parse(Session["IDs"].ToString());
                User user = db.Users.SingleOrDefault(x => x.ID == userID);

                

                //string day = DateTime.Now.Day.ToString();
                //string month = DateTime.Now.Month.ToString();
                //string year = DateTime.Now.Year.ToString();

                DateTime serverTime = DateTime.Now;
                DateTime utcTime = DateTime.UtcNow;

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
                string timeNow = localTime.ToString("t");

                ////////////////////////////////////

                string day = localTime.ToString("dd");
                string month = localTime.ToString("MM");
                string year = localTime.ToString("yyyy");

                DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));

                CuocChoi cuocchoi = db.CuocChois.SingleOrDefault(x => x.NgayDoanSo == datetime);

                if (cuocchoi == null) return Redirect("~/DoanSo/Error5");

                int machoi = int.Parse(cuocchoi.MaCuocChoi.ToString());

                string soDau = Request.Form["sodau"].ToString();
                if (isNumber(soDau) == false)
                {
                    return Content("<script language='javascript' type='text/javascript'> " +
                        "alert('Số đầu không hợp lệ');" +
                        "window.location= '/DoanSo/BaoLo10Page';" +
                        "</script>");

                }

                string soCuoi = Request.Form["socuoi"].ToString();
                if (isNumber(soCuoi) == false)
                {
                    return Content("<script language='javascript' type='text/javascript'> " +
                        "alert('Số cuối không hợp lệ');" +
                        "window.location= '/DoanSo/BaoLo10Page';" +
                        "</script>");

                }

                int sodudoanDau = int.Parse(soDau);
                if (sodudoanDau < 0 || sodudoanDau > 9)
                {
                    return Content("<script language='javascript' type='text/javascript'> " +
                        "alert('Số dự đoán đầu phải nằm trong khoảng từ 0 đến 9');" +
                        "window.location= '/DoanSo/BaoLo10Page';" +
                        "</script>");

                }
                else
                {
                    int sodudoanCuoi = int.Parse(soCuoi);
                    if (sodudoanCuoi < 0 || sodudoanCuoi > 9)
                    {
                        return Content("<script language='javascript' type='text/javascript'> " +
                            "alert('Số dự đoán cuối phải nằm trong khoảng từ 0 đến 9');" +
                            "window.location= '/DoanSo/BaoLo10Page';" +
                            "</script>");

                    }
                    else
                    {


                        for (int i = 0; i <= 9; i++)
                        {
                            ChiTietCuocChoi chitietcuocchoi1 = new ChiTietCuocChoi();
                            int sodudoan = sodudoanDau * 100 + i * 10 + sodudoanCuoi;
                            int trongsodefault = 1;

                            int soluotchoi = int.Parse(user.soluotchoi.ToString());
                            int soluotchoi_km = int.Parse(user.soluotchoi_km.ToString());
                            int sumluotchoi = soluotchoi + soluotchoi_km;

                            ChiTietCuocChoi chitiet3 = db.ChiTietCuocChois.SingleOrDefault(x => x.SoDuDoan == sodudoan && x.MaCuocChoi == machoi && x.UserID == userID);
                            {
                                if (chitiet3 != null)
                                {
                                    ds2.Add(new DanhSachSoChuaDoanViewModel()
                                    {
                                        id = chitietcuocchoi1.id,
                                        sodadoan = sodudoan
                                    });
                                }
                                else
                                {
                                    chitietcuocchoi1.SoDuDoan = sodudoan;
                                    chitietcuocchoi1.UserID = int.Parse(Session["IDs"].ToString());
                                    chitietcuocchoi1.MaCuocChoi = machoi;
                                    chitietcuocchoi1.TrongSo = trongsodefault;



                                    db.ChiTietCuocChois.Add(chitietcuocchoi1);


                                    sumluotchoi--;
                                    if (sumluotchoi <= int.Parse(user.soluotchoi.ToString()))
                                    {
                                        user.soluotchoi = sumluotchoi;
                                        user.soluotchoi_km = 0;
                                    }
                                    else
                                    {
                                        user.soluotchoi_km = sumluotchoi - int.Parse(user.soluotchoi.ToString());
                                    }
                                    Session["soLuotChoi"] = user.soluotchoi;
                                    Session["soLuotChoi_km"] = user.soluotchoi_km;
                                    Session["km_view"] = user.soluotchoi_km.ToString();
                                    Session["ct_view"] = user.soluotchoi.ToString();
                                    db.SaveChanges();

                                    ds.Add(new DanhSachSoDaDoanViewModel()
                                    {
                                        id = chitietcuocchoi1.id,
                                        sodadoan = sodudoan,
                                        trongso = chitietcuocchoi1.TrongSo,

                                        luotchoi = int.Parse(Session["soLuotChoi"].ToString()),
                                        luotchoi_km = int.Parse(Session["soLuotChoi_km"].ToString())
                                    });

                                }
                            }
                        }
                        return View(oj);
                    }
                }
            }
            else return Redirect("~/User/Login");

        }


        public ActionResult Edit(int? id)
        {
            if (Session["userName"] != null && Session["Role"].ToString() == "User")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ChiTietCuocChoi ct = db.ChiTietCuocChois.SingleOrDefault(m => m.id == id);
                if (ct == null)
                {
                    return HttpNotFound();
                }
                return View(ct);
            }
            else return Redirect("~/User/Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,MaCuocChoi,SoDuDoan,TrongSo,id")] ChiTietCuocChoi ct)
        {
            User user = db.Users.SingleOrDefault(x => x.ID == ct.UserID);
            int soluotchoi = int.Parse(user.soluotchoi.ToString());
            int soluotchoi_km = int.Parse(user.soluotchoi_km.ToString());
            int sumluotchoi = soluotchoi + soluotchoi_km;

            if (ModelState.IsValid)
            {               
                db.Entry(ct).State = EntityState.Modified;
                int trongsonew = int.Parse((ct.TrongSo).ToString());
                //int luotchoi = int.Parse(user.soluotchoi.ToString());
                if(trongsonew>1)
                { sumluotchoi = sumluotchoi + 1 - trongsonew;
                    if (sumluotchoi > 0)
                    {
                        if (sumluotchoi <= int.Parse(user.soluotchoi.ToString()))
                        {
                            user.soluotchoi = sumluotchoi;
                            user.soluotchoi_km = 0;
                        }
                        else
                        {
                            user.soluotchoi_km = sumluotchoi - int.Parse(user.soluotchoi.ToString());
                        }
                        db.SaveChanges();
                        return Redirect("~/User/LichSuDoanSo");
                    }
                    else return Content("<script language='javascript' type='text/javascript'> " +
                        "alert('Thay đổi trọng số không thành công. Lượt chơi còn lại nhỏ hơn trọng số thay đổi.');" +
                        "window.location= '/User/LichSuDoanSo';" +
                        "</script>");
                }
                else return Content("<script language='javascript' type='text/javascript'> " +
                        "alert('Trọng số không được nhỏ hơn 1');" +
                        "window.location= '/User/LichSuDoanSo';" +
                        "</script>");
            }
            return View(ct);
        }



        public ActionResult BaoLoGiuaCuoi()
        {
            List<DanhSachSoDaDoanViewModel> ds = new List<DanhSachSoDaDoanViewModel>();
            List<DanhSachSoChuaDoanViewModel> ds2 = new List<DanhSachSoChuaDoanViewModel>();
            List<object> oj = new List<object>();
            oj.Add(ds);
            oj.Add(ds2);
            if (Session["userName"] != null && Session["Role"].ToString() == "User")
            {
                string name = Session["userName"].ToString();
                ViewBag.Name = name;
                int userID = int.Parse(Session["IDs"].ToString());
                User user = db.Users.SingleOrDefault(x => x.ID == userID);

                //string day = DateTime.Now.Day.ToString();
                //string month = DateTime.Now.Month.ToString();
                //string year = DateTime.Now.Year.ToString();

                DateTime serverTime = DateTime.Now;
                DateTime utcTime = DateTime.UtcNow;

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
                string timeNow = localTime.ToString("t");

                ////////////////////////////////////

                string day = localTime.ToString("dd");
                string month = localTime.ToString("MM");
                string year = localTime.ToString("yyyy");

                DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));

                CuocChoi cuocchoi = db.CuocChois.SingleOrDefault(x => x.NgayDoanSo == datetime);

                if (cuocchoi == null) return Redirect("~/DoanSo/Error5");

                int machoi = int.Parse(cuocchoi.MaCuocChoi.ToString());

                string soGiua = Request.Form["sogiua"].ToString();
                if (isNumber(soGiua) == false)
                {
                    return Content("<script language='javascript' type='text/javascript'> " +
                        "alert('Số dự đoán giữa không hợp lệ');" +
                        "window.location= '/DoanSo/BaoLo10Page';" +
                        "</script>");

                }

                string soCuoi = Request.Form["socuoi"].ToString();
                if (isNumber(soCuoi) == false)
                {
                    return Content("<script language='javascript' type='text/javascript'> " +
                        "alert('Số dự đoán cuối không hợp lệ');" +
                        "window.location= '/DoanSo/BaoLo10Page';" +
                        "</script>");

                }

                int sodudoanGiua = int.Parse(soGiua);
                if (sodudoanGiua < 0 || sodudoanGiua > 9)
                {
                    return Content("<script language='javascript' type='text/javascript'> " +
                        "alert('Số dự đoán giữa phải nằm trong khoảng từ 0 đến 9');" +
                        "window.location= '/DoanSo/BaoLo10Page';" +
                        "</script>");

                }
                else
                {
                    int sodudoanCuoi = int.Parse(soCuoi);
                    if (sodudoanCuoi < 0 || sodudoanCuoi > 9)
                    {
                        return Content("<script language='javascript' type='text/javascript'> " +
                            "alert('Số dự đoán cuối phải nằm trong khoảng từ 0 đến 9');" +
                            "window.location= '/DoanSo/BaoLo10Page';" +
                            "</script>");

                    }
                    else
                    {


                        for (int i = 0; i <= 9; i++)
                        {
                            ChiTietCuocChoi chitietcuocchoi1 = new ChiTietCuocChoi();
                            int sodudoan = i * 100 + sodudoanGiua * 10 + sodudoanCuoi;
                            int trongsodefault = 1;

                            int soluotchoi = int.Parse(user.soluotchoi.ToString());
                            int soluotchoi_km = int.Parse(user.soluotchoi_km.ToString());
                            int sumluotchoi = soluotchoi + soluotchoi_km;

                            ChiTietCuocChoi chitiet3 = db.ChiTietCuocChois.SingleOrDefault(x => x.SoDuDoan == sodudoan && x.MaCuocChoi == machoi && x.UserID == userID && x.TrongSo == trongsodefault);
                            {
                                if (chitiet3 != null)
                                {
                                    ds2.Add(new DanhSachSoChuaDoanViewModel()
                                    {
                                        id = chitietcuocchoi1.id,
                                        sodadoan = sodudoan
                                    });
                                }
                                else
                                {
                                    chitietcuocchoi1.SoDuDoan = sodudoan;
                                    chitietcuocchoi1.UserID = int.Parse(Session["IDs"].ToString());
                                    chitietcuocchoi1.MaCuocChoi = machoi;
                                    chitietcuocchoi1.TrongSo = trongsodefault;
                                    db.ChiTietCuocChois.Add(chitietcuocchoi1);

                                    sumluotchoi--;
                                    if (sumluotchoi <= int.Parse(user.soluotchoi.ToString()))
                                    {
                                        user.soluotchoi = sumluotchoi;
                                        user.soluotchoi_km = 0;
                                    }
                                    else
                                    {
                                        user.soluotchoi_km = sumluotchoi - int.Parse(user.soluotchoi.ToString());
                                    }
                                    Session["soLuotChoi"] = user.soluotchoi;
                                    Session["soLuotChoi_km"] = user.soluotchoi_km;
                                    Session["km_view"] = user.soluotchoi_km.ToString();
                                    Session["ct_view"] = user.soluotchoi.ToString();
                                    db.SaveChanges();




                                    ds.Add(new DanhSachSoDaDoanViewModel()
                                    {
                                        id = chitietcuocchoi1.id,
                                        sodadoan = sodudoan,
                                        trongso = chitietcuocchoi1.TrongSo,
                                        luotchoi = int.Parse(Session["soLuotChoi"].ToString()),
                                        luotchoi_km = int.Parse(Session["soLuotChoi_km"].ToString())
                                    });
                                }
                            }
                        }
                     return View(oj);
                        
                    }
                }
            }
            else return Redirect("~/User/Login");
        }


        public ActionResult BaoLoDauGiua()
        {
            List<DanhSachSoDaDoanViewModel> ds = new List<DanhSachSoDaDoanViewModel>();
            List<DanhSachSoChuaDoanViewModel> ds2 = new List<DanhSachSoChuaDoanViewModel>();
            List<object> oj = new List<object>();
            oj.Add(ds);
            oj.Add(ds2);
            if (Session["userName"] != null && Session["Role"].ToString() == "User")
            {
                string name = Session["userName"].ToString();
                ViewBag.Name = name;
                int userID = int.Parse(Session["IDs"].ToString());
                User user = db.Users.SingleOrDefault(x => x.ID == userID);

                //string day = DateTime.Now.Day.ToString();
                //string month = DateTime.Now.Month.ToString();
                //string year = DateTime.Now.Year.ToString();

                DateTime serverTime = DateTime.Now;
                DateTime utcTime = DateTime.UtcNow;

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
                string timeNow = localTime.ToString("t");

                ////////////////////////////////////

                string day = localTime.ToString("dd");
                string month = localTime.ToString("MM");
                string year = localTime.ToString("yyyy");

                DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));

                CuocChoi cuocchoi = db.CuocChois.SingleOrDefault(x => x.NgayDoanSo == datetime);

                if (cuocchoi == null) return Redirect("~/DoanSo/Error5");

                int machoi = int.Parse(cuocchoi.MaCuocChoi.ToString());

                string soDau = Request.Form["sodau"].ToString();
                if (isNumber(soDau) == false)
                {
                    return Content("<script language='javascript' type='text/javascript'> " +
                        "alert('Số dự đoán đầu không hợp lệ');" +
                        "window.location= '/DoanSo/BaoLo10Page';" +
                        "</script>");

                }

                string soGiua = Request.Form["sogiua"].ToString();
                if (isNumber(soGiua) == false)
                {
                    return Content("<script language='javascript' type='text/javascript'> " +
                        "alert('Số dự đoán giữa không hợ lệ');" +
                        "window.location= '/DoanSo/BaoLo10Page';" +
                        "</script>");

                }

                int sodudoanDau = int.Parse(soDau);
                if (sodudoanDau < 0 || sodudoanDau > 9)
                {
                    return Content("<script language='javascript' type='text/javascript'> " +
                        "alert('Số dự đoán đầu phải nằm trong khoảng từ 0 đến 9');" +
                        "window.location= '/DoanSo/BaoLo10Page';" +
                        "</script>");

                }
                else
                {
                    int sodudoanGiữa = int.Parse(soGiua);
                    if (sodudoanGiữa < 0 || sodudoanGiữa > 9)
                    {
                        return Content("<script language='javascript' type='text/javascript'> " +
                            "alert('Số dự đoán giữa phải nằm trong khoảng từ 0 đến 9');" +
                            "window.location= '/DoanSo/BaoLo10Page';" +
                            "</script>");

                    }
                    else
                    {


                        for (int i = 0; i <= 9; i++)
                        {
                            ChiTietCuocChoi chitietcuocchoi1 = new ChiTietCuocChoi();
                            int sodudoan = sodudoanDau * 100 + sodudoanGiữa * 10 + i;
                            int trongsodefault = 1;

                            int soluotchoi = int.Parse(user.soluotchoi.ToString());
                            int soluotchoi_km = int.Parse(user.soluotchoi_km.ToString());
                            int sumluotchoi = soluotchoi + soluotchoi_km;


                            ChiTietCuocChoi chitiet3 = db.ChiTietCuocChois.SingleOrDefault(x => x.SoDuDoan == sodudoan && x.MaCuocChoi == machoi && x.UserID == userID && x.TrongSo == trongsodefault);
                            {
                                if (chitiet3 != null)
                                {
                                    ds2.Add(new DanhSachSoChuaDoanViewModel()
                                    {
                                        id = chitietcuocchoi1.id,
                                        sodadoan = sodudoan
                                    });
                                }
                                else
                                {
                                    chitietcuocchoi1.SoDuDoan = sodudoan;
                                    chitietcuocchoi1.UserID = int.Parse(Session["IDs"].ToString());
                                    chitietcuocchoi1.MaCuocChoi = machoi;
                                    chitietcuocchoi1.TrongSo = trongsodefault;
                                    db.ChiTietCuocChois.Add(chitietcuocchoi1);
                                    sumluotchoi--;
                                    if (sumluotchoi <= int.Parse(user.soluotchoi.ToString()))
                                    {
                                        user.soluotchoi = sumluotchoi;
                                        user.soluotchoi_km = 0;
                                    }
                                    else
                                    {
                                        user.soluotchoi_km = sumluotchoi - int.Parse(user.soluotchoi.ToString());
                                    }
                                    Session["soLuotChoi"] = user.soluotchoi;
                                    Session["soLuotChoi_km"] = user.soluotchoi_km;
                                    Session["km_view"] = user.soluotchoi_km.ToString();
                                    Session["ct_view"] = user.soluotchoi.ToString();
                                    db.SaveChanges();



                                    ds.Add(new DanhSachSoDaDoanViewModel()
                                    {
                                        id = chitietcuocchoi1.id,
                                        sodadoan = sodudoan,
                                        trongso = chitietcuocchoi1.TrongSo,

                                        luotchoi = int.Parse(Session["soLuotChoi"].ToString()),
                                        luotchoi_km = int.Parse(Session["soLuotChoi_km"].ToString())
                                    });
                                }
                            }
                        }
                        return View(oj);
                    }
                }
            }
            else return Redirect("~/User/Login");

        }


        public ActionResult BaoLoDau()
        {
            List<DanhSachSoDaDoanViewModel> ds = new List<DanhSachSoDaDoanViewModel>();
            List<DanhSachSoChuaDoanViewModel> ds2 = new List<DanhSachSoChuaDoanViewModel>();
            List<object> oj = new List<object>();
            oj.Add(ds);
            oj.Add(ds2);
            if (Session["userName"] != null && Session["Role"].ToString() == "User")
            {
                string name = Session["userName"].ToString();
                ViewBag.Name = name;
                int userID = int.Parse(Session["IDs"].ToString());
                User user = db.Users.SingleOrDefault(x => x.ID == userID);
                //string day = DateTime.Now.Day.ToString();
                //string month = DateTime.Now.Month.ToString();
                //string year = DateTime.Now.Year.ToString();

                DateTime serverTime = DateTime.Now;
                DateTime utcTime = DateTime.UtcNow;

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
                string timeNow = localTime.ToString("t");

                ////////////////////////////////////

                string day = localTime.ToString("dd");
                string month = localTime.ToString("MM");
                string year = localTime.ToString("yyyy");



                DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));

                CuocChoi cuocchoi = db.CuocChois.SingleOrDefault(x => x.NgayDoanSo == datetime);

                if (cuocchoi == null) return Redirect("~/DoanSo/Error5");

                int machoi = int.Parse(cuocchoi.MaCuocChoi.ToString());

                string soDau = Request.Form["sodau"].ToString();
                if (isNumber(soDau) == false)
                {
                    return Content("<script language='javascript' type='text/javascript'> " +
                        "alert('Số dự đoán đầu không hợp lệ');" +
                        "window.location= '/DoanSo/BaoLo100Page';" +
                        "</script>");

                }

                int sodudoanDau = int.Parse(soDau);
                if (sodudoanDau < 0 || sodudoanDau > 9)
                {
                    return Content("<script language='javascript' type='text/javascript'> " +
                        "alert('Số dự đoán đầu phải nằm trong khoảng từ 0 đến 9');" +
                        "window.location= '/DoanSo/BaoLo100Page';" +
                        "</script>");

                }
                else
                {

                    for (int i = 0; i <= 9; i++)
                        for (int j = 0; j <= 9; j++)
                        {
                            ChiTietCuocChoi chitietcuocchoi1 = new ChiTietCuocChoi();
                            int sodudoan = sodudoanDau * 100 + i * 10 + j;
                            int trongsodefault = 1;

                            int soluotchoi = int.Parse(user.soluotchoi.ToString());
                            int soluotchoi_km = int.Parse(user.soluotchoi_km.ToString());
                            int sumluotchoi = soluotchoi + soluotchoi_km;

                            ChiTietCuocChoi chitiet3 = db.ChiTietCuocChois.SingleOrDefault(x => x.SoDuDoan == sodudoan && x.MaCuocChoi == machoi && x.UserID == userID);
                            {
                                if (chitiet3 != null)
                                {
                                    ds2.Add(new DanhSachSoChuaDoanViewModel()
                                    {
                                        id = chitietcuocchoi1.id,
                                        sodadoan = sodudoan
                                    });
                                }
                                else
                                {
                                    chitietcuocchoi1.SoDuDoan = sodudoan;
                                    chitietcuocchoi1.UserID = int.Parse(Session["IDs"].ToString());
                                    chitietcuocchoi1.MaCuocChoi = machoi;
                                    chitietcuocchoi1.TrongSo = trongsodefault;
                                    db.ChiTietCuocChois.Add(chitietcuocchoi1);
                                    sumluotchoi--;
                                    if (sumluotchoi <= int.Parse(user.soluotchoi.ToString()))
                                    {
                                        user.soluotchoi = sumluotchoi;
                                        user.soluotchoi_km = 0;
                                    }
                                    else
                                    {
                                        user.soluotchoi_km = sumluotchoi - int.Parse(user.soluotchoi.ToString());
                                    }
                                    Session["soLuotChoi"] = user.soluotchoi;
                                    Session["soLuotChoi_km"] = user.soluotchoi_km;
                                    Session["km_view"] = user.soluotchoi_km.ToString();
                                    Session["ct_view"] = user.soluotchoi.ToString();
                                    db.SaveChanges();



                                    ds.Add(new DanhSachSoDaDoanViewModel()
                                    {
                                        id = chitietcuocchoi1.id,
                                        sodadoan = sodudoan,
                                        trongso = chitietcuocchoi1.TrongSo,

                                        luotchoi = int.Parse(Session["soLuotChoi"].ToString()),
                                        luotchoi_km = int.Parse(Session["soLuotChoi_km"].ToString())
                                    });

                                }
                            }
                        }
                    return View(oj);
                }
            }
            else return Redirect("~/User/Login");
        }

        public ActionResult BaoLoGiua()
        {
            List<DanhSachSoDaDoanViewModel> ds = new List<DanhSachSoDaDoanViewModel>();
            List<DanhSachSoChuaDoanViewModel> ds2 = new List<DanhSachSoChuaDoanViewModel>();
            List<object> oj = new List<object>();
            oj.Add(ds);
            oj.Add(ds2);
            if (Session["userName"] != null && Session["Role"].ToString() == "User")
            {
                string name = Session["userName"].ToString();
                ViewBag.Name = name;
                int userID = int.Parse(Session["IDs"].ToString());
                User user = db.Users.SingleOrDefault(x => x.ID == userID);

                //string day = DateTime.Now.Day.ToString();
                //string month = DateTime.Now.Month.ToString();
                //string year = DateTime.Now.Year.ToString();

                DateTime serverTime = DateTime.Now;
                DateTime utcTime = DateTime.UtcNow;

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
                string timeNow = localTime.ToString("t");

                ////////////////////////////////////

                string day = localTime.ToString("dd");
                string month = localTime.ToString("MM");
                string year = localTime.ToString("yyyy");

                DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));

                CuocChoi cuocchoi = db.CuocChois.SingleOrDefault(x => x.NgayDoanSo == datetime);

                if (cuocchoi == null) return Redirect("~/DoanSo/Error5");

                int machoi = int.Parse(cuocchoi.MaCuocChoi.ToString());

                string soGiua = Request.Form["sogiua"].ToString();
                if (isNumber(soGiua) == false)
                {
                    return Content("<script language='javascript' type='text/javascript'> " +
                        "alert('Số dự đoán giữa không hợp lệ');" +
                        "window.location= '/DoanSo/BaoLo100Page';" +
                        "</script>");

                }

                int sodudoanGiua = int.Parse(soGiua);
                if (sodudoanGiua < 0 || sodudoanGiua > 9)
                {
                    return Content("<script language='javascript' type='text/javascript'> " +
                        "alert('Số dự đoán giữa phải nằm trong khoảng từ 0 đến 9');" +
                        "window.location= '/DoanSo/BaoLo100Page';" +
                        "</script>");

                }
                else
                {

                    for (int i = 0; i <= 9; i++)
                        for (int j = 0; j <= 9; j++)
                        {
                            ChiTietCuocChoi chitietcuocchoi1 = new ChiTietCuocChoi();
                            int sodudoan = i * 100 + sodudoanGiua * 10 + j;
                            int trongsodefault = 1;

                            int soluotchoi = int.Parse(user.soluotchoi.ToString());
                            int soluotchoi_km = int.Parse(user.soluotchoi_km.ToString());
                            int sumluotchoi = soluotchoi + soluotchoi_km;

                            ChiTietCuocChoi chitiet3 = db.ChiTietCuocChois.SingleOrDefault(x => x.SoDuDoan == sodudoan && x.MaCuocChoi == machoi && x.UserID == userID);
                            {
                                if (chitiet3 != null)
                                {
                                    ds2.Add(new DanhSachSoChuaDoanViewModel()
                                    {
                                        id = chitietcuocchoi1.id,
                                        sodadoan = sodudoan
                                    });
                                }
                                else
                                {
                                    chitietcuocchoi1.SoDuDoan = sodudoan;
                                    chitietcuocchoi1.UserID = int.Parse(Session["IDs"].ToString());
                                    chitietcuocchoi1.MaCuocChoi = machoi;
                                    chitietcuocchoi1.TrongSo = trongsodefault;
                                    db.ChiTietCuocChois.Add(chitietcuocchoi1);
                                    sumluotchoi--;
                                    if (sumluotchoi <= int.Parse(user.soluotchoi.ToString()))
                                    {
                                        user.soluotchoi = sumluotchoi;
                                        user.soluotchoi_km = 0;
                                    }
                                    else
                                    {
                                        user.soluotchoi_km = sumluotchoi - int.Parse(user.soluotchoi.ToString());
                                    }
                                    Session["soLuotChoi"] = user.soluotchoi;
                                    Session["soLuotChoi_km"] = user.soluotchoi_km;
                                    Session["km_view"] = user.soluotchoi_km.ToString();
                                    Session["ct_view"] = user.soluotchoi.ToString();
                                    db.SaveChanges();



                                    ds.Add(new DanhSachSoDaDoanViewModel()
                                    {
                                        id = chitietcuocchoi1.id,
                                        sodadoan = sodudoan,
                                        trongso = chitietcuocchoi1.TrongSo,

                                        luotchoi = int.Parse(Session["soLuotChoi"].ToString()),
                                        luotchoi_km = int.Parse(Session["soLuotChoi_km"].ToString())
                                    });
                                }
                            }
                        }
                    return View(oj);
                }
            }
            else return Redirect("~/User/Login");
        }


        public ActionResult BaoLoCuoi()
        {
            List<DanhSachSoDaDoanViewModel> ds = new List<DanhSachSoDaDoanViewModel>();
            List<DanhSachSoChuaDoanViewModel> ds2 = new List<DanhSachSoChuaDoanViewModel>();
            List<object> oj = new List<object>();
            oj.Add(ds);
            oj.Add(ds2);
            if (Session["userName"] != null && Session["Role"].ToString() == "User")
            {
                string name = Session["userName"].ToString();
                ViewBag.Name = name;
                int userID = int.Parse(Session["IDs"].ToString());
                User user = db.Users.SingleOrDefault(x => x.ID == userID);



                //string day = DateTime.Now.Day.ToString();
                //string month = DateTime.Now.Month.ToString();
                //string year = DateTime.Now.Year.ToString();

                DateTime serverTime = DateTime.Now;
                DateTime utcTime = DateTime.UtcNow;

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
                string timeNow = localTime.ToString("t");

                ////////////////////////////////////

                string day = localTime.ToString("dd");
                string month = localTime.ToString("MM");
                string year = localTime.ToString("yyyy");

                DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));

                CuocChoi cuocchoi = db.CuocChois.SingleOrDefault(x => x.NgayDoanSo == datetime);

                if (cuocchoi == null) return Redirect("~/DoanSo/Error5");

                int machoi = int.Parse(cuocchoi.MaCuocChoi.ToString());

                string soCuoi = Request.Form["socuoi"].ToString();
                if (isNumber(soCuoi) == false)
                {
                    return Content("<script language='javascript' type='text/javascript'> " +
                        "alert('Số dự đoán cuối không hợp lệ');" +
                        "window.location= '/DoanSo/BaoLo100Page';" +
                        "</script>");

                }

                int sodudoanCuoi = int.Parse(soCuoi);
                if (sodudoanCuoi < 0 || sodudoanCuoi > 9)
                {
                    return Content("<script language='javascript' type='text/javascript'> " +
                        "alert('Số dự đoán phải cuối phải nằm trong khoảng từ 0 đến 9');" +
                        "window.location= '/DoanSo/BaoLo100Page';" +
                        "</script>");

                }
                else
                {

                    for (int i = 0; i <= 9; i++)
                        for (int j = 0; j <= 9; j++)
                        {
                            ChiTietCuocChoi chitietcuocchoi1 = new ChiTietCuocChoi();
                            int sodudoan = i * 100 + j * 10 + sodudoanCuoi;
                            int trongsodefault = 1;

                            int soluotchoi = int.Parse(user.soluotchoi.ToString());
                            int soluotchoi_km = int.Parse(user.soluotchoi_km.ToString());
                            int sumluotchoi = soluotchoi + soluotchoi_km;

                            ChiTietCuocChoi chitiet3 = db.ChiTietCuocChois.SingleOrDefault(x => x.SoDuDoan == sodudoan && x.MaCuocChoi == machoi && x.UserID == userID);
                            {
                                if (chitiet3 != null)
                                {
                                    ds2.Add(new DanhSachSoChuaDoanViewModel()
                                    {
                                        id = chitietcuocchoi1.id,
                                        sodadoan = sodudoan
                                    });
                                }
                                else
                                {
                                    chitietcuocchoi1.SoDuDoan = sodudoan;
                                    chitietcuocchoi1.UserID = int.Parse(Session["IDs"].ToString());
                                    chitietcuocchoi1.MaCuocChoi = machoi;
                                    chitietcuocchoi1.TrongSo = trongsodefault;
                                    db.ChiTietCuocChois.Add(chitietcuocchoi1);
                                    sumluotchoi--;
                                    if (sumluotchoi <= int.Parse(user.soluotchoi.ToString()))
                                    {
                                        user.soluotchoi = sumluotchoi;
                                        user.soluotchoi_km = 0;
                                    }
                                    else
                                    {
                                        user.soluotchoi_km = sumluotchoi - int.Parse(user.soluotchoi.ToString());
                                    }
                                    Session["soLuotChoi"] = user.soluotchoi;
                                    Session["soLuotChoi_km"] = user.soluotchoi_km;
                                    Session["km_view"] = user.soluotchoi_km.ToString();
                                    Session["ct_view"] = user.soluotchoi.ToString();
                                    db.SaveChanges();



                                    ds.Add(new DanhSachSoDaDoanViewModel()
                                    {
                                        id = chitietcuocchoi1.id,
                                        sodadoan = sodudoan,
                                        trongso = chitietcuocchoi1.TrongSo,

                                        luotchoi = int.Parse(Session["soLuotChoi"].ToString()),
                                        luotchoi_km = int.Parse(Session["soLuotChoi_km"].ToString())
                                    });
                                }
                            }
                        }
                    return View(oj);
                }
            }
            else return Redirect("~/User/Login");

        }


        public ActionResult DoanSo(ChiTietCuocChoi chitietcuocchoi)
        {
            if (Session["userName"] != null && Session["Role"].ToString() == "User")
            {
                string name = Session["userName"].ToString();
                ViewBag.Name = name;
                int userID = int.Parse(Session["IDs"].ToString());
                User user = db.Users.SingleOrDefault(x => x.ID == userID);


                int soluotchoi = int.Parse(user.soluotchoi.ToString());
                int soluotchoi_km = int.Parse(user.soluotchoi_km.ToString());
                int sumluotchoi = soluotchoi + soluotchoi_km;


                //string day = DateTime.Now.Day.ToString();
                //string month = DateTime.Now.Month.ToString();
                //string year = DateTime.Now.Year.ToString();

                DateTime serverTime = DateTime.Now;
                DateTime utcTime = DateTime.UtcNow;

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
                string timeNow = localTime.ToString("t");

                ////////////////////////////////////

                string day = localTime.ToString("dd");
                string month = localTime.ToString("MM");
                string year = localTime.ToString("yyyy");

                DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));


                CuocChoi cuocchoi = db.CuocChois.SingleOrDefault(x => x.NgayDoanSo == datetime);

                if (cuocchoi == null) return Redirect("~/DoanSo/Error5");

                int machoi = int.Parse(cuocchoi.MaCuocChoi.ToString());

                string duDoan = Request.Form["SoDuDoan"].ToString();
                string trongSo = Request.Form["TrongSo"].ToString();
                if (isNumber(trongSo) == true)
                {
                    int temp_trongso = int.Parse(trongSo);
                    if (temp_trongso <= sumluotchoi)
                    {
                        if (isNumber(duDoan) == false)
                        {
                            return Content("<script language='javascript' type='text/javascript'> " +
                                "alert('Số dự đoán không hợp lệ');" +
                                "window.location= '/DoanSo/DoanSoPage';" +
                                "</script>");

                        }
                        if (isNumber(trongSo) == false)
                        {
                            return Content("<script language='javascript' type='text/javascript'> " +
                                "alert('Trọng số không hợp lệ');" +
                                "window.location= '/DoanSo/DoanSoPage';" +
                                "</script>");

                        }

                        int soDuDoan = int.Parse(duDoan);
                        int soTrongSo = int.Parse(trongSo);
                        if (soDuDoan < 0 || soDuDoan > 999)
                        {

                            return Content("<script language='javascript' type='text/javascript'> " +
                                "alert('Số dự đoán phải nằm trong khoảng từ 0 đến 999');" +
                                "window.location= '/DoanSo/DoanSoPage';" +
                                "</script>");

                        }
                        if (soTrongSo <= 0)
                        {
                            return Content("<script language='javascript' type='text/javascript'> " +
                                "alert('Trọng số phải nằm trong khoảng từ 0 đến 99');" +
                                "window.location= '/DoanSo/DoanSoPage';" +
                                "</script>");
                        }
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
                                Session["soTrongSo"] = soTrongSo;
                                chitietcuocchoi.UserID = int.Parse(Session["IDs"].ToString());
                                chitietcuocchoi.MaCuocChoi = machoi;
                                db.ChiTietCuocChois.Add(chitietcuocchoi);

                                int tongluot = sumluotchoi - soTrongSo;
                                if (tongluot <= int.Parse(user.soluotchoi.ToString()))
                                {
                                    user.soluotchoi = tongluot;
                                    user.soluotchoi_km = 0;
                                }
                                else
                                {
                                    user.soluotchoi = soluotchoi;
                                    user.soluotchoi_km = soluotchoi_km-soTrongSo;
                                }
                                Session["soLuotChoi"] = user.soluotchoi;
                                Session["soLuotChoi_km"] = user.soluotchoi_km;
                                db.SaveChanges();



                                return Redirect("~/DoanSo/ThongBaoPage");

                            }
                        }
                    }
                    else return Content("<script language='javascript' type='text/javascript'> " +
                               "alert('Trọng số lớn hơn lượt chơi còn lại');" +
                               "window.location= '/DoanSo/DoanSoPage';" +
                               "</script>");
                } else return Content("<script language='javascript' type='text/javascript'> " +
                               "alert('Trọng số không hợp lệ');" +
                               "window.location= '/DoanSo/DoanSoPage';" +
                               "</script>");
            }
            else return Redirect("~User/Login");
            
        }


        public ActionResult ThongBaoPage()
        {
            if (Session["userName"] != null && Session["Role"].ToString() == "User")
            {
                //string day = DateTime.Now.Day.ToString();
                //string month = DateTime.Now.Month.ToString();
                //string year = DateTime.Now.Year.ToString();


                DateTime serverTime = DateTime.Now;
                DateTime utcTime = DateTime.UtcNow;

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
                string timeNow = localTime.ToString("t");

                ////////////////////////////////////

                string day = localTime.ToString("dd");
                string month = localTime.ToString("MM");
                string year = localTime.ToString("yyyy");
                DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));



                CuocChoi cuocchoi = db.CuocChois.SingleOrDefault(x => x.NgayDoanSo == datetime);

                int machoi = int.Parse(cuocchoi.MaCuocChoi.ToString());



                DanhSachTrungThuong danhsach = db.DanhSachTrungThuongs.SingleOrDefault(x => x.MaCuocChoi == machoi);
                string TienThuong = danhsach.TongTienThuong.ToString();


                int userID = int.Parse(Session["IDs"].ToString());
                int soDuDoans = int.Parse(Session["soDuDoan"].ToString());
                int soTrongSos = int.Parse(Session["soTrongSo"].ToString());
                ChiTietCuocChoi chitiet = db.ChiTietCuocChois.SingleOrDefault(x => x.UserID == userID && x.MaCuocChoi == machoi && x.SoDuDoan == soDuDoans && x.TrongSo==soTrongSos);

                int soDuDoan = chitiet.SoDuDoan;
                int? soTrongSo = chitiet.TrongSo;
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
            else return Redirect("~User/Login");
        } 
	} 
}