using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;

namespace UL.AXT.Background.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product

        private BLL.Product prod = new Product();
        private BLL.UserInfo user = new UserInfo();
        private string strUploadPath = ConfigurationManager.AppSettings["UploadFilePath"].ToString();
        private string XUserId= ConfigurationManager.AppSettings["XUserId"].ToString();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddProduct()
        {
            return View();
        }

        public JsonResult AddProdStock(int prodType, string prodName, decimal stockPrice, int stockNum,
            string productUrl, string prodDesc)
        {
            BaseResult result = new BaseResult();
            result = prod.AddProdStock(prodType, prodName, stockPrice, stockNum, productUrl, prodDesc);
            return Json(result);
        }

        public ActionResult PublishProd()
        {
            var lst= prod.GetAllProds();
            return View(lst);
        }

        public JsonResult PublishProdData(decimal totalPrice,int unitPrice,int needNum,decimal prodAC, int prodType,int prodId)
        {
            BaseResult result = new BaseResult();
            result = prod.PublishProdData(totalPrice, unitPrice, needNum, prodAC, prodType, prodId);
            return Json(result);
        }

        public ActionResult OneDollarBuy()
        {
            var lst = prod.GetAllProdPeriods();
            return View(lst);
        }

        public JsonResult OpenLot(int periodId,string lotteryNo)
        {
            BaseResult result = new BaseResult();
            //未分享完全部返还给分享者
            List<T_User_Share> lst = prod.GetRestShare(periodId);
            foreach (var shareDto in lst)
            {
                var num = shareDto.ShareNum - shareDto.RevGiftNum;
                string codes = GenerateCode(periodId, num);

                result = prod.AddLotByXUser(shareDto.ShareUserId, num, periodId, codes,1);
                if (!result.Succeeded)
                {
                    return Json(result);
                }
            }

            result = prod.OpenLot(periodId, lotteryNo);
            return Json(result);
        }

        public ActionResult UpdPic(int prodId, int picType)
        {
            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string imgPath = "";
            string relPath = "";
            BaseResult result = new BaseResult();
            if (hfc.Count > 0)
            {
                relPath = "/ProductPhotos/" + DateTime.Now.ToString("yyyyMMdd") + "/";
                string path = AppDomain.CurrentDomain.BaseDirectory + strUploadPath + relPath;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
               
                imgPath = strUploadPath + relPath + hfc[0].FileName;
                string PhysicalPath = Server.MapPath(imgPath);
                hfc[0].SaveAs(PhysicalPath);

                string copyRoot = "E:/WebSite/AiXiaTou" + strUploadPath;
                //复制文件到其他路径
                if (!Directory.Exists(copyRoot + relPath))
                {
                    Directory.CreateDirectory(copyRoot + relPath);
                    
                }
                System.IO.File.Copy(PhysicalPath, copyRoot + relPath + hfc[0].FileName);
                //添加照片
                result = prod.AddPic(prodId, relPath + hfc[0].FileName, 2);
            }
            return Json(new { Succeeded = result.Succeeded, errMsg = result.ErrMsg, imgPath1 = imgPath }, "text/html", JsonRequestBehavior.AllowGet);

        }

        public JsonResult AddXNum(int periodId, int xnum,decimal rate)
        {
            BaseResult result = new BaseResult();
            result.Succeeded = true;
            //string codes = GenerateCode(periodId, xnum);
            //int userId = Convert.ToInt32(XUserId);
            
            //result = prod.AddLotByXUser(userId, xnum, periodId, codes,2);
            //if (result.Succeeded)
            //    result.ResultId = codes;

            string path = "/BgAddLot/UL.AXT.TimerTask.exe";
            string PhysicalPath = Server.MapPath(path);

            Process proc = new Process();
            proc.StartInfo.FileName = PhysicalPath;
            proc.StartInfo.Arguments = periodId.ToString()+" "+ rate.ToString();
            proc.Start();

            return Json(result);
        }

       
        //计算用户当期的随机虾仔号码
        public string GenerateCode(int periodId, int codeNum)
        {
            string codes = string.Empty;
            var code = "";
            Random rd = new Random();
            var flag = false;
            //生成不重复的抽奖号码
            for (int i = 0; i < codeNum; i++)
            {
                do
                {
                    code = rd.Next(10000, 100000).ToString();
                } while (isExistsNo(code, periodId));

                codes += code + ",";
            }
            if (codes.Length > 0)
                codes = codes.Substring(0, codes.Length - 1);
            return codes;
        }

        public bool isExistsNo(string code, int periodId)
        {
            BLL.Order order = new Order();
            bool flag = order.IsExistsLottery(code, periodId);
            return flag;
        }

        public JsonResult GetAwardAddr(int periodId)
        {
           T_UserAddr addr= user.GetAwardAddr(periodId);
            return Json(addr);
        }

        public JsonResult AddAwardInfo(int periodId,string awardNo)
        {
            BaseResult result = user.AddAwardInfo(periodId, awardNo);
            return Json(result);
        }

        public ActionResult LoginPage()
        {
            return View();
        }

        public JsonResult Login(string loginUser, string loginPwd)
        {
            UserInfo user = new UserInfo();
            BaseResult result = user.Login(loginUser, loginPwd);
            return Json(result);
        }
    }
}