using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LuckyNumber.Models;
using System.Text;
using System.Data.Entity;
using LuckyNumber.SMSAPI;
using Facebook;
using System.Configuration;
using LuckyNumber.ViewModel;

namespace LuckyNumber.Controllers
{
    public class UserController : Controller
    {
        //
        LuckyNumContext db = new LuckyNumContext();
        // GET: /User/
        public ActionResult Index()
        {
            if (Session["userName"] == null)
            {
                string day = DateTime.Now.Day.ToString();
                string month = DateTime.Now.Month.ToString();
                string year = DateTime.Now.Year.ToString();

                string timeEnd = DateTime.Parse("11:59 PM").ToString("t");
                string timeStart = DateTime.Parse("00:00 AM").ToString("t");
                string timeNow = DateTime.Now.ToString("t");

                DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
                CuocChoi cuocchoi = db.CuocChois.SingleOrDefault(x => x.NgayDoanSo == datetime);
                //int mamax = db.CuocChois.Max(x => x.MaCuocChoi);
                //CuocChoi cuocchoi = db.CuocChois.SingleOrDefault(x => x.MaCuocChoi == mamax);

                int machoi;

                if (cuocchoi.TrangThai == true)
                {
                    machoi = cuocchoi.MaCuocChoi - 1;

                }
                //if (cuocchoi == null) machoi = 7;
                //else machoi = cuocchoi.MaCuocChoi;

                else machoi = cuocchoi.MaCuocChoi;

                string dayNow = day + "/" + month + "/" + year;
                Session["dayNow"] = dayNow;
                ViewBag.dayNow = Session["dayNow"].ToString();


                List<DanhSachTrungThuongViewModel> model = new List<DanhSachTrungThuongViewModel>();
                var join = (from US in db.Users
                            join CTC in db.ChiTietCuocChois
                                on US.ID equals CTC.UserID
                            join CC in db.CuocChois on CTC.MaCuocChoi equals CC.MaCuocChoi
                            join DSTT in db.DanhSachTrungThuongs on CC.MaCuocChoi equals DSTT.MaCuocChoi
                            join CTTT in db.ChiTietTrungThuongs on DSTT.MaDSTrungThuong equals CTTT.MaDSTrungThuong
                            where CC.MaCuocChoi == machoi && CTTT.SoDuDoan == CTC.SoDuDoan
                            select new
                            {
                                userName = US.username,
                                Phone = US.phone.Substring(0, 6) + "xxxx",
                                Email = "xxxx" + US.email.Substring(4),
                                ngayDoanSo = CC.NgayDoanSo
                            }).ToList().Distinct();
                foreach (var item in join)
                {
                    model.Add(new DanhSachTrungThuongViewModel()
                    {
                        username = item.userName,
                        phone = item.Phone,
                        email = item.Email,
                        NgayDoanSo = item.ngayDoanSo
                    });
                }
                if (cuocchoi != null)
                {

                    if (cuocchoi.TrangThai == true && ((DateTime.Compare(DateTime.Parse(timeNow), DateTime.Parse(timeEnd)) > 0) || (DateTime.Compare(DateTime.Parse(timeNow), DateTime.Parse(timeStart)) < 0)))
                        ketthucphien();
                    return View(model);

                }
                return View(model);
            }
            else return Redirect("~/User/userProfile");
        }


        private void ketthucphien()
        {
            string day = DateTime.Now.Day.ToString();
            string month = DateTime.Now.Month.ToString();
            string year = DateTime.Now.Year.ToString();

            DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
            CuocChoi cuocchoi = db.CuocChois.SingleOrDefault(x => x.NgayDoanSo == datetime);


            // ---------- End -------------------




            int maChoi = int.Parse(cuocchoi.MaCuocChoi.ToString()); // Lấy ra mã cuộc chơi từ ngày chơi

            // ----- Lấy ra danh sách theo mã cuộc chơi --------
            DanhSachTrungThuong danhsach = db.DanhSachTrungThuongs.SingleOrDefault(x => x.MaCuocChoi == maChoi);

            int maDS = int.Parse(danhsach.MaDSTrungThuong.ToString());

            // ----------- End --------------

            var tongSoLan = from u in db.ChiTietCuocChois
                            where u.MaCuocChoi == maChoi
                            group u by u.SoDuDoan into Counted
                            select new
                            {
                                soDuDoan = Counted.Key,
                                soLan = Counted.Count(),
                                soTrongSo=Counted.Sum(u=>u.TrongSo)
                                
                            };
            int soLanItNhat = tongSoLan.Min(x => x.soLan);
            var tongSoLanItNhat = from t in tongSoLan
                                  where t.soLan == soLanItNhat
                                  select t;
            int tongSoItNhat = tongSoLanItNhat.Count();
            int? tongTrongSo = tongSoLanItNhat.Sum(x => x.soTrongSo);
            float? tienThuong = float.Parse(danhsach.TongTienThuong.ToString()) / tongTrongSo; // số tiền

            foreach (var i in tongSoLanItNhat)
            {
                var danhSachTrung = from y in db.ChiTietCuocChois
                                    where y.SoDuDoan == i.soDuDoan && y.MaCuocChoi == maChoi && y.TrongSo==i.soTrongSo
                                    select y;
                foreach (var o in danhSachTrung)
                {
                    
                    ChiTietTrungThuong chiTietTrungThuong = new ChiTietTrungThuong();
                    chiTietTrungThuong.UserID = o.UserID;
                    chiTietTrungThuong.MaDSTrungThuong = maDS;
                    chiTietTrungThuong.SoDuDoan = o.SoDuDoan;
                    chiTietTrungThuong.TienThuong = tienThuong*o.TrongSo;

                    User user = db.Users.SingleOrDefault(x => x.ID == o.UserID);
                    user.taikhoan += tienThuong*o.TrongSo;
                  

                    db.ChiTietTrungThuongs.Add(chiTietTrungThuong);
                }
            }
            cuocchoi.TrangThai = false;
            var list = from u in db.Users
                       select u;
            foreach (var i in list)
            {
                i.soluotchoi = 50;


            }
            db.SaveChanges();
        }
        public ActionResult CreateUser()
        {
            return View();
        }
        public ActionResult Create(User user)
        {
            StringBuilder sb = new StringBuilder();
            char c;
            string c1;
            Random rand = new Random();
            for (int i = 0; i < 9; i++)
            {
                c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 87)));
                sb.Append(c);
            }
            c1 = sb.ToString();

            user.mamoi = c1;
            user.soluotchoi = 5;
            user.xacnhan = false;
            user.taikhoan = 0;
            user.status = false;
            db.Users.Add(user);
            db.SaveChanges();
            return Redirect("~/User");
        }

        public ActionResult Login()
        {
            if(Session["userName"]==null || Session["Role"].ToString()!= "User")
            return View();
            else return Redirect("~/User/userProfile");


        }

        public ActionResult LienHe()
        {
            if (Session["userName"] != null && Session["Role"].ToString()=="User")
            {
                string name = Session["userName"].ToString();
                ViewBag.Name = name;
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        [HttpPost]
        public ActionResult Login(User user)
        {
            using (LuckyNumContext db = new LuckyNumContext())
            {
                User user2 = db.Users.SingleOrDefault(x => x.username == user.username && x.password == user.password);
                if (user2 != null)
                {
                    string Role = "User";
                    Session["Role"] = Role; 
                    Session["userName"] = user2.nickname;
                    Session["IDs"] = user2.ID;
                    Session["eMail"] = user2.email;
                    Session["pHone"] = user2.phone;
                    Session["soLuotChoi"] = user2.soluotchoi.ToString();
                    Session["taiKhoan"] = user2.taikhoan.ToString();
                    Session["maMoi"] = user2.mamoi.ToString();

                    return Redirect("~/User/userProfile");
                }
            }
            return Redirect("~/User/signError");
        }

        public ActionResult signError()
        {
            return View();
        }

        //public ActionResult CapNhatPhone()
        //{
        //    int userID = int.Parse(Session["IDs"].ToString());
        //    User user = db.Users.SingleOrDefault(x => x.ID == userID);

        //    string phone = Request.Form["txtPhone"].ToString();

        //    user.phone = phone;
        //    Session["pHone"] = user.phone;
        //    db.SaveChanges();
        //    return Redirect("~/User/userProfile");

        //}

        public ActionResult CapNhatPass()
        {
            int userID = int.Parse(Session["IDs"].ToString());
            User user = db.Users.SingleOrDefault(x => x.ID == userID);

            string PassOld = Request.Form["txtPassOld"].ToString();
            string Pass = Request.Form["txtPass"].ToString();

            if (PassOld == user.password)
            {
                user.password = Pass;
                db.SaveChanges();
                return Redirect("~/User/userProfile");
            }

            return Redirect("~/User/signError");
        }

        public ActionResult NapThe()
        {
            
            int userID = int.Parse(Session["IDs"].ToString());
            User user = db.Users.SingleOrDefault(x => x.ID == userID);

            string Serial = Request.Form["txtSerial"].ToString();
            string Pin = Request.Form["txtPin"].ToString();

            if (Serial == "123"  && Pin == "123")
            {
                user.soluotchoi++;
                Session["soLuotChoi"] = user.soluotchoi;
                db.SaveChanges();
                return Redirect("~/User/userProfile");
            }

            else return Redirect("~/User/userProfile");
            
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect("~/User/Index");
        }

        public ActionResult NapThePage()
        {
            if (Session["userName"] != null && Session["Role"].ToString()=="User")
            {
                string name = Session["userName"].ToString();
                ViewBag.Name = name;
                return View();
            }
            else return RedirectToAction("Login");
        }



        public ActionResult ThongBao4()
        {
            return View();
        }
        public ActionResult confirm()
        {
            if (Session["userName"] != null && Session["Role"].ToString()=="User")
            {
                string name = Session["userName"].ToString();
                ViewBag.Name = name;
                int userID = int.Parse(Session["IDs"].ToString());
                User user = db.Users.SingleOrDefault(x => x.ID == userID);

                if (user.xacnhan == true)
                {
                    return Redirect("~/User/ThongBao_1");
                }

                else if (user.phone == null) return Redirect("~/User/ThongBao4");

                StringBuilder sb = new StringBuilder();
                char c;
                string c1;
                string phone = Session["pHone"].ToString();
                Random rand = new Random();
                for (int i = 0; i < 5; i++)
                {
                    c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 87)));
                    sb.Append(c);
                }

                c1 = sb.ToString();
                Session["MaXacNhan"] = c1;

                SpeedSMSAPI api = new SpeedSMSAPI();
                String userInfo = api.getUserInfo();
                String response = api.sendSMS(phone, "Ma xac nhan cua ban la: " + c1, 2, "");
                return Redirect("~/User/confirmNum");
            }
            else return RedirectToAction("Login");
        }

        public ActionResult confirmPhone()
        {
            StringBuilder sb = new StringBuilder();
            char c;
            string c1;
            string phone = Session["pHone"].ToString();
            Random rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 87)));
                sb.Append(c);
            }
            
            c1 = sb.ToString();
            Session["MaXacNhan"] = c1;

            SpeedSMSAPI api = new SpeedSMSAPI();
            String userInfo = api.getUserInfo();
            String response = api.sendSMS(phone, "Ma xac nhan cua ban la: " + c1, 2, "");
            return Redirect("~/User/confirmNum");
        }

        public ActionResult confirmNum()
        {
            return View();
        }
        public ActionResult confirmPhoneNum()
        {
            int userID = int.Parse(Session["IDs"].ToString());
            User user = db.Users.SingleOrDefault(x => x.ID == userID);

            string maxacnhan = Session["MaXacNhan"].ToString();
            string xacnhan = Request.Form["txtMaXacNhan"].ToString();

            if (maxacnhan == xacnhan)
            {
                user.xacnhan = true;
                db.SaveChanges();
                return Redirect("~/User/userProfile");
            }

            return Redirect("~/User/Error1");
        }

        public ActionResult Error1()
        {
            return View();
        }

        public ActionResult ThongBao_1()
        {
            return View();
        }
        public long InsertForFacebook(User user)
        {
            
            var user2 = db.Users.SingleOrDefault(x => x.username == user.username);
            if (user2 == null)
            {
                user.soluotchoi = 5;
                user.xacnhan = true;
                db.Users.Add(user);

                db.SaveChanges();
                return user.ID;
                //return Redirect("~/User");
            }
            else
            {
                return user.ID;
            }
        }

        public ActionResult LoginFacebook()
        {
            
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "554898694702243",
                client_secret = "7bb97aae1513eae5f51832f2dee5e80c",
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "554898694702243",
                client_secret = "7bb97aae1513eae5f51832f2dee5e80c",
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name, middle_name, last_name, email");
                string email = me.email;
                string firstname = me.first_name;
                string midname = me.middle_name;
                string lastname = me.last_name;

                var user = new User();
                user.email = email;
                user.username = email;
                user.nickname = firstname + " " + midname + " " + lastname;

                var resultInsert = new UserController().InsertForFacebook(user);
                if (resultInsert > 0)
                {
                    Session["userName"] = user.nickname;
                    Session["IDs"] = user.ID;
                    Session["eMail"] = user.email;
                    Session["pHone"] = user.phone;
                    Session["soLuotChoi"] = user.soluotchoi.ToString();
                    Session["maMoi"] = "ACDTH";
                    Session["taiKhoan"] = 0;
                }
            }
            return Redirect("~/User/userProfile");
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public ActionResult userProfile()
        {
            if (Session["IDs"] != null)
            {
                if (Session["Role"].ToString() == "User")
                {
                    int userID = int.Parse(Session["IDs"].ToString());
                    User user = db.Users.SingleOrDefault(x => x.ID == userID);

                    string name = Session["userName"].ToString();
                    ViewBag.Name = name;
                    string mail = Session["eMail"].ToString();
                    ViewBag.Mail = mail;
                    string id = Session["IDs"].ToString();
                    ViewBag.Id = id;
                    string luotchoi = Session["soLuotChoi"].ToString();
                    ViewBag.LuotChoi = luotchoi;
                    string mamoi = Session["maMoi"].ToString();
                    ViewBag.MaMoi = mamoi;
                    string sodu = Session["taiKhoan"].ToString();
                    ViewBag.SoDu = sodu;

                    if (user.phone == null)
                    {
                        string phone = "Bạn vui lòng xác nhận số điện thoại";
                        //string phone = Session["pHone"].ToString();
                        ViewBag.phone = phone;
                    }
                    else
                    {
                        string phone = user.phone.ToString();
                        ViewBag.phone = phone;
                    }

                    return View();
                }
                else return Redirect("Login");
            }
            else return RedirectToAction("Login");
        }

        public ActionResult CachChoi()
        {
            if (Session["IDs"] != null && Session["Role"].ToString()=="User")
            {
                int userID = int.Parse(Session["IDs"].ToString());
                User user = db.Users.SingleOrDefault(x => x.ID == userID);

                string name = Session["userName"].ToString();
                ViewBag.Name = name;
                string mail = Session["eMail"].ToString();
                ViewBag.Mail = mail;
                string id = Session["IDs"].ToString();
                ViewBag.Id = id;
                string luotchoi = Session["soLuotChoi"].ToString();
                ViewBag.LuotChoi = luotchoi;
                string mamoi = Session["maMoi"].ToString();
                ViewBag.MaMoi = mamoi;

                if (user.phone == null)
                {
                    string phone = "Bạn vui lòng xác nhận số điện thoại";
                    //string phone = Session["pHone"].ToString();
                    ViewBag.phone = phone;
                }
                else
                {
                    string phone = user.phone.ToString();
                    ViewBag.phone = phone;
                }
                return View();
            }
            else return RedirectToAction("Login");
        }

        public ActionResult reDirect()
        {
            return Redirect("~/User/userProfile");
        }

        
        [AllowAnonymous]
        public ActionResult CheckUserName(string User_name)
        {
            User user = db.Users.SingleOrDefault(x => x.username == User_name);
            if (user == null)
                return Json("Bạn có thể dùng tài khoản này để đăng ký", JsonRequestBehavior.AllowGet);
            return Json("Đã có người sử dụng tài khoản này", JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult CheckPhone(string Phone)
        {
            int userID = int.Parse(Session["IDs"].ToString());
            User user = db.Users.SingleOrDefault(x => x.phone == Phone);
            if (user == null)
            {
                //string phone = Request.Form["txtPhone"].ToString();
                user = db.Users.SingleOrDefault(x => x.ID == userID);
                user.phone = Phone;
                Session["pHone"] = user.phone;
                db.SaveChanges();
                return Json("Cập nhật số điện thoại thành công", JsonRequestBehavior.AllowGet);
            }
            return Json("Số điện thoại này đã có người dùng", JsonRequestBehavior.AllowGet);
        }


        public ActionResult LichSuDoanSo()
        {
            if (Session["userName"] != null && Session["Role"].ToString()=="User")
            {
                string name = Session["userName"].ToString();
                ViewBag.Name = name;

                int userID = int.Parse(Session["IDs"].ToString());



                List<ChiTietChoiViewModel> model = new List<ChiTietChoiViewModel>();
                var join = (from US in db.Users
                            join CTC in db.ChiTietCuocChois
                                on US.ID equals CTC.UserID
                            join CC in db.CuocChois on CTC.MaCuocChoi equals CC.MaCuocChoi
                            where US.ID == userID
                            select new
                            {
                                userName = US.username,
                                soDuDoan = CTC.SoDuDoan,
                                ngayDoanSo = CC.NgayDoanSo,
                                trongSo=CTC.TrongSo
                            }).ToList();
                foreach (var item in join)
                {
                    model.Add(new ChiTietChoiViewModel()
                    {
                        username = item.userName,
                        SoDuDoan = item.soDuDoan,
                        NgayDoanSo = item.ngayDoanSo,
                        TrongSo=item.trongSo
                    });
                }

                return View(model);
            }
            else return RedirectToAction("Login");
        }

        public ActionResult LichSuTrungThuong()
        {
            if (Session["IDs"] != null && Session["Role"].ToString()=="User")
            {
                int userID = int.Parse(Session["IDs"].ToString());
                //User user = db.Users.SingleOrDefault(x => x.ID == userID);
                string name = Session["userName"].ToString();
                ViewBag.Name = name;


                List<LichSuTrungThuongViewModel> model = new List<LichSuTrungThuongViewModel>();
                var join = (from US in db.Users
                            join CTC in db.ChiTietCuocChois
                                on US.ID equals CTC.UserID
                            join CC in db.CuocChois on CTC.MaCuocChoi equals CC.MaCuocChoi
                            join DSTT in db.DanhSachTrungThuongs on CC.MaCuocChoi equals DSTT.MaCuocChoi
                            join CTTT in db.ChiTietTrungThuongs on DSTT.MaDSTrungThuong equals CTTT.MaDSTrungThuong
                            where US.ID == userID && CTTT.UserID == userID && CTTT.SoDuDoan == CTC.SoDuDoan
                            select new
                            {
                                userName = US.username,
                                soDuDoan = CTC.SoDuDoan,
                                ngayDoanSo = CC.NgayDoanSo,
                                tienThuong = CTTT.TienThuong
                            }).ToList();

                foreach (var item in join)
                {
                    model.Add(new LichSuTrungThuongViewModel()
                    {
                        username = item.userName,
                        NgayDoanSo = item.ngayDoanSo,
                        SoDuDoan = item.soDuDoan,
                        TienThuong = float.Parse(item.tienThuong.ToString())
                    });
                }

                return View(model);
            }
            else return RedirectToAction("Login");
        }

        public ActionResult ThongBao3()
        {
            return View();
        }
        public ActionResult MoiBanBe()
        {
            if (Session["userName"] != null && Session["Role"].ToString() == "User")
            {
                string name = Session["userName"].ToString();
                ViewBag.Name = name;
                int userID = int.Parse(Session["IDs"].ToString());
                User user = db.Users.SingleOrDefault(x => x.ID == userID);

                if (user.status == true) return Redirect("~/User/ThongBao3");

                else return View();
            }
            else return RedirectToAction("Login");
        }

        public ActionResult LoiMoiBan()
        {
            return View();
        }

        public ActionResult MoiBan()
        {
            if (Session["IDs"] != null && Session["Role"].ToString() == "User")
            {
                int userID = int.Parse(Session["IDs"].ToString());
                User user = db.Users.SingleOrDefault(x => x.ID == userID);

                string maMoi = Request.Form["MaMoi"];

                var list = from u in db.Users
                           select u;

                foreach (var i in list)
                {
                    if (i.mamoi == maMoi)
                    {
                        user.soluotchoi += 5;
                        user.status = true;
                        i.soluotchoi += 5;
                        Session["soLuotChoi"] = user.soluotchoi;
                        db.SaveChanges();
                        return Redirect("~/User/userProfile");
                    }
                }
                return Redirect("~/User/LoiMoiBan");
            }
            else return RedirectToAction("Login");
        }


        public ActionResult DoiThuongPage()
        {
            if (Session["IDs"] != null && Session["Role"].ToString() == "User")
            {
                int userID = int.Parse(Session["IDs"].ToString());
                User user = db.Users.SingleOrDefault(x => x.ID == userID);

                string name = Session["userName"].ToString();
                ViewBag.Name = name;

                string id = Session["IDs"].ToString();
                ViewBag.Id = id;

                string taikhoan = Session["taiKhoan"].ToString();
                ViewBag.TaiKhoan = taikhoan;
                return View();
            }
            else return RedirectToAction("Login");
        }


        [AllowAnonymous]
        public ActionResult DoiThuong()
        {
            int userID = int.Parse(Session["IDs"].ToString());
            User user = db.Users.SingleOrDefault(x => x.ID == userID);

            int giatri = int.Parse(Request.Form["msi"].ToString());
            if (user.taikhoan >= giatri)
            {
                user.taikhoan -= giatri;

                StringBuilder sb = new StringBuilder();
                char c;
                string c1;
                //string phone = Session["pHone"].ToString();
                Random rand = new Random();
                for (int i = 0; i < 10; i++)
                {
                    c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 87)));
                    sb.Append(c);
                }

                c1 = sb.ToString();

                Session["taiKhoan"] = user.taikhoan;

                db.SaveChanges();

                return Json("Mã thẻ của bạn là: " + c1 + 
                "Bạn vui lòng ghi lại mã thẻ trước khi trở về!", JsonRequestBehavior.AllowGet);

                //return Json("Tài khoản của bạn hiện tại bé hơn giá trị thẻ bạn chọn!", JsonRequestBehavior.AllowGet);
            }

            return Json("Tài khoản của bạn không đủ tiền", JsonRequestBehavior.AllowGet);
        }

        public ActionResult XacNhanSDT()
        {
            if (Session["userName"] != null && Session["Role"].ToString() == "User" )
            {
                string name = Session["userName"].ToString();
                ViewBag.Name = name;
                int userID = int.Parse(Session["IDs"].ToString());
                User user = db.Users.SingleOrDefault(x => x.ID == userID);
                if (user.phone != null)
                {
                    return Redirect("~/User/ThongBao_1");
                }

                return View();
            }
            else return RedirectToAction("Login");
        }


        public ActionResult ThongBao2(){
            return View();
        }
        public ActionResult XacNhanSoDT()
        {
            if (Session["userName"] != null && Session["Role"].ToString() == "User" )
            {
                string sdt = Request.Form["sdt"].ToString();

                int userID = int.Parse(Session["IDs"].ToString());
                //User user = db.Users.SingleOrDefault(x => x.ID == userID);
                User user = db.Users.SingleOrDefault(x => x.phone == sdt);
                if (user == null)
                {
                    user = db.Users.SingleOrDefault(x => x.ID == userID);
                    user.phone = sdt;
                    Session["pHone"] = user.phone;
                    db.SaveChanges();
                    return Redirect("~/User/userProfile");
                }



                return Redirect("~/User/ThongBao2"); // chỗ này nhớ viết
            }
            else return RedirectToAction("Login");
        }

        public ActionResult DoiMatKhau()
        {
            if (Session["userName"] != null && Session["Role"].ToString() == "User") 
            {
                return View();
            }
            else return RedirectToAction("Login");
        }

        public ActionResult ChangePass()
        {
            if (Session["IDs"] != null && Session["Role"].ToString() == "User")
            {
                int userID = int.Parse(Session["IDs"].ToString());
                User user = db.Users.SingleOrDefault(x => x.ID == userID);

                string pass1 = Request.Form["pass1"].ToString();
                string pass2 = Request.Form["pass2"].ToString();
                string pass3 = Request.Form["pass3"].ToString();

                if (user.password != pass1) return Json("Mật khẩu hiện tại của bạn không chính xác", JsonRequestBehavior.AllowGet);
                else if (pass2 != pass3) return Json("Sai Mật Khẩu Nhập Lại", JsonRequestBehavior.AllowGet);

                user.password = pass2;
                db.SaveChanges();

                return Redirect("~/User/userProfile");
            }
            else return RedirectToAction("Login");
        }

        public ActionResult ChangeName()
        {
            if (Session["IDs"] != null && Session["Role"].ToString() == "User" )
            {
                int userID = int.Parse(Session["IDs"].ToString());
                User user = db.Users.SingleOrDefault(x => x.ID == userID);
                string newname = Request.Form["newname"].ToString();

                user.nickname = newname;
                Session["userName"] = user.nickname;
                db.SaveChanges();
                return Redirect("~/User/userProfile");
            }
            else
                return RedirectToAction("Login");
        }
	}
}