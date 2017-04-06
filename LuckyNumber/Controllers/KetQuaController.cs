using LuckyNumber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LuckyNumber.ViewModel;

namespace LuckyNumber.Controllers
{
    public class KetQuaController : Controller
    {
        LuckyNumContext db = new LuckyNumContext();
        //
        // GET: /KetQua/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DanhSachTrungThuongTrongNgay()
        {
            if (Session["userName"] != null)
            {
                string name = Session["userName"].ToString();
                ViewBag.Name = name;
                string day = DateTime.Now.Day.ToString();
                string month = DateTime.Now.Month.ToString();
                string year = DateTime.Now.Year.ToString();
                //int machoi;
                DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));



                //DateTime datetime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
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
            else return Redirect("~/User/Login");
        }
	}
}