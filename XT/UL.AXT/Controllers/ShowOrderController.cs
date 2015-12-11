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
            List<CommentDTO> lst = comment.GetCommentListByPeroid(periodId).ToList();
            //定义排序完成后的评论记录
            List<CommentDTO> lstCommentInOrder = new List<CommentDTO>();
            //第一级评论
            List<CommentDTO> lstFirstLev =
                lst.Where(c => c.CommentRefID == 0).OrderBy(c => c.CommentDate).ToList();

            PackageComment(0, lstCommentInOrder, lst);
            
            return View(lstCommentInOrder);
        }

        //递归添加回复它的记录
        public void PackageComment(int CommentRefID, List<CommentDTO> lstCommentInOrder, List<CommentDTO> lst)
        {
            
            List<CommentDTO> lstcommRef = lst.Where(c => c.CommentRefID == CommentRefID).OrderBy(c=>c.CommentDate).ToList();
            foreach (var commRef in lstcommRef)
           {
                lstCommentInOrder.Add(commRef);
                PackageComment(commRef.CommentID, lstCommentInOrder,lst);
            }
            
        }
    }
}