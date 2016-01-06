using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Model;

namespace UL.AXT.Controllers
{
    public class UserController : Controller
    {
        private string strUploadPath = ConfigurationManager.AppSettings["UploadFilePath"].ToString();
        BLL.UserInfo user = new UserInfo();
        private BLL.Product prod = new Product();
        // GET: User
        public ActionResult Index(int userId=1)
        {
            var userInfo= user.GetUserInfo(userId);
            return View(userInfo);
        }

        public ActionResult ReceiveAddrs(int userId = 1)
        {
            var addrs = user.GetReceiveAddrs(userId);
            return View(addrs);
        }

        public ActionResult HomePage(int userId)
        {
            //用户信息
            //参与信息
            //中奖信息
            //晒单信息
            //回复消息

            var userInfo = user.GetUserInfo(userId);
            List<ProductModel> lst = prod.GetProducts(1,userId).ToList();

            BLL.ShowOrder showOrder = new BLL.ShowOrder();
            List<ShowOrderModel> showOrders = showOrder.GetShowingOrders(0,userId).ToList();

            BLL.Comment comment = new Comment();
            List<CommentDTO> comments= comment.GetCommentListByUserid(userId).ToList();

            ViewBag.UserInfo = userInfo;
            ViewBag.ShowOrders = showOrders;
            ViewBag.UploadPath = strUploadPath;
            ViewBag.Comments = comments;

            return View(lst);
        }

        public ActionResult MyEnshrine(int userId)
        {
            List<ProductModel> lst = prod.GetEnshrineProducts(userId).ToList();
            return View(lst);
            
        }

        public JsonResult EditAddr(T_UserAddr addr, int type)
        {
            BaseResult br = user.EditAddr(addr,type);
            return Json(br);
        }

        public JsonResult GetAddrs(int userId)
        {
            var addrs = user.GetReceiveAddrs(userId);
            return Json(addrs);
        }

        public ActionResult AccountDetail(int userId)
        {

            var userInfo = user.GetUserInfo(userId);
            return View(userInfo);
        }

        public ActionResult Joined(int userId)
        {
            List<ProductModel> lst = prod.GetProducts(1, userId).ToList();
            return View(lst);
        }

        public ActionResult Gifts(int userId)
        {
            return View();
        }
    }
}