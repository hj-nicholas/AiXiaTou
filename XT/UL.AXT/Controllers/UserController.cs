using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;

namespace UL.AXT.Controllers
{
    public class UserController : Controller
    {
        BLL.UserInfo user = new UserInfo();
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
    }
}