using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LuckyNumber.Models;

namespace LuckyNumber.Controllers
{
    public class TableViewController : Controller
    {

        LuckyNumContext db = new LuckyNumContext();
        //
        // GET: /TableView/
        public ActionResult DSTrungThuong()
        {
            //var model = new DS().Show_All();
            var model = db.Users.ToList();
            return View(model);
        }
	}
}