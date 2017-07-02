using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LuckyNumber.ViewModel;
using LuckyNumber.Models;

namespace LuckyNumber.Controllers
{
    public class DieuChinhGiaiThuongController : Controller
    {
        LuckyNumContext db = new LuckyNumContext();
        //
        // GET: /DieuChinhGiaiThuong/
        public ActionResult Index()
        {
            if (Session["userName"] != null && Session["Role"].ToString() == "Admin")
                return View();
            else return Redirect("~/Admin/Login");
        }

        public ActionResult DieuChinhGiaiThuong()
        {
            if (Session["userName"] != null && Session["Role"].ToString() == "Admin")
            {
                String name = Session["userName"].ToString();
                ViewBag.Name = name;

                List<DieuChinhGiaiThuongViewModel> model = new List<DieuChinhGiaiThuongViewModel>();
                var join = (from CC in db.CuocChois
                            join DSTT in db.DanhSachTrungThuongs
                                on CC.MaCuocChoi equals DSTT.MaCuocChoi

                            select new
                            {
                                maDSTrungThuong = DSTT.MaDSTrungThuong,
                                ngayDoanSo = CC.NgayDoanSo,
                                tienThuong = DSTT.TongTienThuong
                            }).ToList().Distinct();
                foreach (var item in join)
                {
                    model.Add(new DieuChinhGiaiThuongViewModel()
                    {
                        MaDSTrungThuong = item.maDSTrungThuong,
                        NgayDoanSo = item.ngayDoanSo,
                        TienThuong = item.tienThuong
                    });
                }
                return View(model);
            }
            else return Redirect("~/Admin/Login");
        }
	}
}