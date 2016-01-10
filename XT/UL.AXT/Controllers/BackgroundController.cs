using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;

namespace UL.AXT.Controllers
{
    public class BackgroundController : Controller
    {
        // GET: Background
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult OpenLottery(string lotteryTicket,int periodId)
        {
            //根据彩票号码在数据库中查询出中奖号码
            Product prod = new Product();
            prod.ConfirmLottery(lotteryTicket, periodId);
            return Json("");
        }
    }
}