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

            List<T_Comment> lstCommentInOrder = new List<T_Comment>();
            int commentId = lst.Where(c => c.CommentRefID == 0).FirstOrDefault().CommentID;
            List<T_Comment> lstFirst =
                lst.Where(c => c.CommentRefID == commentId).OrderBy(c => c.CommentDate).ToList();

            foreach (var comm in lstFirst)
            {
                //添加第一条记录
                lstCommentInOrder.Add(comm);
                PackageComment(comm.CommentID, lstCommentInOrder, lst);
            }
            return View(lstCommentInOrder);
        }

        //递归添加回复它的记录
        public void PackageComment(int CommentRefID, List<T_Comment> lstCommentInOrder, List<T_Comment> lst)
        {
            
            List<T_Comment> lstcommRef = lst.Where(c => c.CommentRefID == CommentRefID).OrderBy(c=>c.CommentDate).ToList();
            if (lstcommRef.Count > 0)
            {
                lstCommentInOrder.Add(lstcommRef[0]);
                PackageComment(lstcommRef[0].CommentID, lstCommentInOrder,lst);
            }
            else
            {
                lstCommentInOrder.Add(lstcommRef[0]);
            }
        }
    }
}