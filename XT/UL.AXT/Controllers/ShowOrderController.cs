using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using BLL;
using Hoo.Common.WeChat;
using Model;
using UL.AXT.Common;

namespace UL.AXT.Controllers
{
    public class ShowOrderController : Controller
    {
        private string strUploadPath = ConfigurationManager.AppSettings["UploadFilePath"].ToString();

        BLL.Comment comment = new Comment();
        private BLL.Product product = new Product();
        // GET: ShowOrder
        public ActionResult Index(string code = "")
        {
            //js分享功能
            //JSSDK jssdk = new JSSDK();
            //Hashtable ht= jssdk.getSignPackage();
            //ViewBag.timestamp = ht["timestamp"].ToString();
            //ViewBag.nonceStr = ht["nonceStr"].ToString();
            //ViewBag.signature = ht["signature"].ToString();

            //HOO Test-1
            //code = "0211e2a86877e52f073a3dad2042c25b";
            //获取登录微信用户信息
            UserDTO userDto = new UserDTO();
            if (!string.IsNullOrEmpty(code))
            {
                WeChatBusiness business = new WeChatBusiness();
                Hoo.Common.WeChat.UserInfo userInfo = business.GetWeChatUser(code);
                if (userInfo != null)
                {
                    userDto = business.ChangeUserByWeChatInfo(userInfo);
                    Session["UserId"] = userDto.UserID;
                }
            }
            else
            {
                if (Session["UserId"] != null)
                {
                    BLL.UserInfo user = new BLL.UserInfo();
                    var userId = Convert.ToInt32(Session["UserId"]);
                    userDto = user.GetUserInfo(userId);
                }


            }

            //WeChatBusiness business = new WeChatBusiness();
            //Session["UserId"] = "6";

            //Hoo.Common.WeChat.UserInfo userInfo = new Hoo.Common.WeChat.UserInfo("ooSaOwsnQbC52N-srS25TaEV-DeU");
            //userDto = new BLL.UserInfo().GetUserInfo(Convert.ToInt32(6));

            //Session["UserId"] = "9";

            //Hoo.Common.WeChat.UserInfo userInfo = new Hoo.Common.WeChat.UserInfo("ooSaOwvwiW_Do1cN3w0TGRMIRNrk");
            //userDto = new BLL.UserInfo().GetUserInfo(Convert.ToInt32(9));

            //userDto = business.ChangeUserByWeChatInfo(userInfo);
            //Log.WriteLog("username:", userDto.UserName);
            ViewBag.UserInfo = userDto;
            ViewBag.UploadPath =  strUploadPath;
           
            BLL.ShowOrder showOrder = new BLL.ShowOrder();
            List<ShowOrderModel> lst = showOrder.GetShowingOrders(0,0,userDto.UserID).ToList();
            return View(lst);
        }

        public ActionResult CommentList(int periodId)
        {
            List<CommentDTO> lst = comment.GetCommentListByPeroid(periodId).ToList();
           //定义排序完成后的评论记录
            List<CommentDTO> lstCommentInOrder = new List<CommentDTO>();
            //第一级评论
            List<CommentDTO> lstFirstLev =
                lst.Where(c => c.CommentRefID == 0).OrderBy(c => c.CommentDate).ToList();

            PackageComment(0, lstCommentInOrder, lst);

            int CommNum = 0;
            int suppNum = 0;
            product.GetRelPeriodInfo(periodId, out suppNum,out  CommNum);
            ViewBag.CommNum = CommNum;
            ViewBag.SuppNum = suppNum;
            ViewBag.PeriodId = periodId;
            ViewBag.RootCommId = lstCommentInOrder.Where(c => c.CommentRefID == 0).First().CommentID;
            return View(lstCommentInOrder.Where(c=>c.CommentRefID !=0).ToList());
        }

        //递归添加回复它的记录
        public void PackageComment(int CommentRefID, List<CommentDTO> lstCommentInOrder, List<CommentDTO> lst)
        {

            List<CommentDTO> lstcommRef = lst.Where(c => c.CommentRefID == CommentRefID).OrderBy(c => c.CommentDate).ToList();
            foreach (var commRef in lstcommRef)
            {
                lstCommentInOrder.Add(commRef);
                PackageComment(commRef.CommentID, lstCommentInOrder, lst);
            }

        }

        public JsonResult AddSupport(int userId, int periodId)
        {
            BaseResult br = new BaseResult();
            br = comment.AddSupport(userId, periodId);
            return Json(br);
        }

        public JsonResult AddReply(int userId, int commRefId, int periodId, string replyContent)
        {
            BaseResult br = new BaseResult();
            br = comment.AddReply(userId, commRefId, periodId, replyContent);

            return Json(br);
        }

        public JsonResult AddShow(string content, int periodId, int userId)
        {
            BLL.ShowOrder show = new ShowOrder();
            BaseResult br = new BaseResult();
            br = show.AddShow(content, periodId, userId);

            return Json(br);
        }
    }
}