using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LuckyNumber.ViewModel;
using LuckyNumber.Models;

namespace LuckyNumber.Controllers
{
    public class AdminController : Controller
    {
        LuckyNumContext db = new LuckyNumContext();   
        //
        // GET: /Admin/
        public ActionResult Login()
        {
            if(Session["userName"]==null)
            return View();
            else return Redirect("~/Admin/adminProfile");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect("~/Admin/Login");
        }

        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            using (LuckyNumContext db = new LuckyNumContext())
            {
                Admin admin2 = db.Admins.SingleOrDefault(x => x.username == admin.username && x.password == admin.password);
                if (admin2 != null)
                {
                    Session["userName"] = admin2.nickname;
                    Session["email"] = admin2.email;
                    Session["IDs"] = admin2.AdminID;
                    //Session["phone"] = admin2.phone;
                    return Redirect("~/Admin/adminProfile");
                }
                return Redirect("~/Admin/signError");
            }
        }

        public ActionResult adminProfile()
        {
            if (Session["userName"] != null)
            {
                String name = Session["userName"].ToString();
                ViewBag.Name = name;
                string email = Session["email"].ToString();
                ViewBag.Email = email;
                return View();
            }
            else return RedirectToAction("Login");
        }

        public ActionResult signError()
        {
            return View();
        }

        

        public ActionResult QuanLyPhienChoi()
        {
            if (Session["userName"] != null)
            {
                String name = Session["userName"].ToString();
                ViewBag.Name = name;
                var model = db.CuocChois.ToList();
                return View(model);
            }
            else return RedirectToAction("Login");
        }

        public ActionResult ThemPhienChoi(CuocChoi cuocchoi)
        {
            db.CuocChois.Add(cuocchoi);
            cuocchoi.TrangThai = true;

           

            db.SaveChanges();
            int ma = cuocchoi.MaCuocChoi;
            DanhSachTrungThuong danhsach = new DanhSachTrungThuong();

            db.DanhSachTrungThuongs.Add(danhsach);
            danhsach.MaCuocChoi = ma;
            danhsach.TongTienThuong = 5000000;
            db.SaveChanges();

            return Redirect("~/Admin/QuanLyPhienChoi");
        }

        public ActionResult KetThucPhienChoi()
        {
            if (Session["userName"] != null)
            {
                String name = Session["userName"].ToString();
                ViewBag.Name = name;
                return View();
            }
            else return RedirectToAction("Login");
        }


        public ActionResult ThemLuotChoiAo()
        {
            if (Session["userName"] != null)
            {
                String name = Session["userName"].ToString();
                ViewBag.Name = name;
                return View();
            }
            else return RedirectToAction("Login");
        }

        public ActionResult ThemNguoiChoiAo()
        {

            int a = int.Parse(Request.Form["SoLuongNumber"].ToString());

            int[] listInt;
            string day = DateTime.Now.Day.ToString();
            string month = DateTime.Now.Month.ToString();
            string year = DateTime.Now.Year.ToString();

            DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
            CuocChoi cuocchoi = db.CuocChois.SingleOrDefault(x => x.NgayDoanSo == datetime);

            int maChoi = int.Parse(cuocchoi.MaCuocChoi.ToString()); // chỗ này là lấy ra mã chơi nha đạt


            listInt = new int[a];
            int ptr = 0;
            int maxInt;
            int idInt = 0;
            int soDuDoan;
            bool isNext = true;
            Random rd = new Random();
            var list = from u in db.Users
                       select new { Id = u.ID };
            maxInt = list.Count() - 1;
            foreach (var i in list)
            {
                listInt[ptr++] = i.Id;
            }
            for (int i = 0; i < a; i++)
            {
                ptr = rd.Next(0, maxInt);
                ChiTietCuocChoi chitiet = new ChiTietCuocChoi();
                idInt = listInt[ptr];
                chitiet.UserID = idInt;
                chitiet.MaCuocChoi = maChoi;
                isNext = true;
                do
                {
                    soDuDoan = rd.Next(0, 999);
                    ChiTietCuocChoi chitiet2 = db.ChiTietCuocChois.SingleOrDefault(x => x.MaCuocChoi == maChoi && x.UserID == idInt && x.SoDuDoan == soDuDoan);
                    if (chitiet2 == null)
                        isNext = false;
                } while (isNext);
                chitiet.SoDuDoan = soDuDoan;
                db.ChiTietCuocChois.Add(chitiet);
                db.SaveChanges();
            }
         
                return Redirect("~/Admin/adminProfile");
        }

        public ActionResult KetThucPhien()
        {
            // ---- Lấy ra ngày tương ứng ------
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
                                soLan = Counted.Count()
                            };
            int soLanItNhat = tongSoLan.Min(x => x.soLan);
            var tongSoLanItNhat = from t in tongSoLan
                                  where t.soLan == soLanItNhat
                                  select t;
            int tongSoItNhat = tongSoLanItNhat.Count();
            float tienThuong = float.Parse(danhsach.TongTienThuong.ToString()) / tongSoItNhat; // số tiền

            foreach (var i in tongSoLanItNhat)
            {
                var danhSachTrung = from y in db.ChiTietCuocChois
                                    where y.SoDuDoan == i.soDuDoan && y.MaCuocChoi == maChoi
                                    select y;
                foreach(var o in danhSachTrung)
                {
                    ChiTietTrungThuong chiTietTrungThuong = new ChiTietTrungThuong();
                    chiTietTrungThuong.UserID = o.UserID;
                    chiTietTrungThuong.MaDSTrungThuong = maDS;
                    chiTietTrungThuong.SoDuDoan = o.SoDuDoan;
                    chiTietTrungThuong.TienThuong = tienThuong;
                    User user = db.Users.SingleOrDefault(x => x.ID == o.UserID);
                    user.taikhoan += tienThuong;
                    

                    db.ChiTietTrungThuongs.Add(chiTietTrungThuong);
                }
            }



            cuocchoi.TrangThai = false;
            var list = from u in db.Users
                       select u;
            foreach (var i in list)
            {
                i.soluotchoi = 5;
                
                
            }
            db.SaveChanges();
            return Redirect("~/Admin/adminProfile");
        }

        public ActionResult DieuChinhGiaiThuong()
        {
            int ma = int.Parse(Request.Form["mads"].ToString());
            string a = Request.Form["giaithuong"].ToString();
            double tien = double.Parse(a);


            DanhSachTrungThuong ds = db.DanhSachTrungThuongs.SingleOrDefault(x => x.MaDSTrungThuong == ma);

            if(ds == null) return Json("Mã danh sách không tồn tại", JsonRequestBehavior.AllowGet);
            else if (a == null) return Json("Sai Định Dạng", JsonRequestBehavior.AllowGet);
            else
            {
                ds.TongTienThuong = tien;
                db.SaveChanges();
            }

            return Redirect("~/DieuChinhGiaiThuong/DieuChinhGiaiThuong");
        }

        public ActionResult DanhSachTrungThuong()
        {

            string day = DateTime.Now.Day.ToString();
            string month = DateTime.Now.Month.ToString();
            string year = DateTime.Now.Year.ToString();


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


            



            //int maChoi = int.Parse(cuocchoi.MaCuocChoi.ToString());
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


            return View(model);
        }

        public ActionResult DoiMatKhau()
        {
            return View();
        }

        public ActionResult ChangePass()
        {
            int adID = int.Parse(Session["IDs"].ToString());
            Admin admin = db.Admins.SingleOrDefault(x => x.AdminID == adID);

            string pass1 = Request.Form["pass1"].ToString();
            string pass2 = Request.Form["pass2"].ToString();
            string pass3 = Request.Form["pass3"].ToString();

            if (admin.password != pass1) return Json("Mật khẩu hiện tại của bạn không chính xác", JsonRequestBehavior.AllowGet);
            else if (pass2 != pass3) return Json("Sai Mật Khẩu Nhập Lại", JsonRequestBehavior.AllowGet);

            admin.password = pass2;
            db.SaveChanges();

            return Redirect("~/Admin/adminProfile");
        }
	}
}