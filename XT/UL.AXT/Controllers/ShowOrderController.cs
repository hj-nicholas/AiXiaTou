using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Model;

namespace UL.AXT.Controllers
{
    public class ShowOrderController : Controller
    {
        // GET: ShowOrder
        public ActionResult Index()
        {
            BLL.ShowOrder showOrder = new BLL.ShowOrder();
            List<ShowOrderModel> lst = showOrder.GetShowingOrders().ToList();
            return View(lst);
        }

        public ActionResult CommentList(int periodId)
        {
            BLL.Comment comment = new Comment();
            List<T_Comment> lst = comment.GetCommentListByPeroid(periodId).ToList();
            return View(lst);
        }
    }
}