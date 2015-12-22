using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Hoo.WeChat.WxPayAPI;
using Model;

namespace UL.AXT.Controllers
{
    public class ProductController : Controller
    {
        BLL.Product prod=new Product();
        // GET: Product
        public ActionResult Index(int proType=1)
        {

            List<ProductModel> lst = prod.GetProducts(proType).ToList();
            return View(lst);
        }

        public ActionResult ProductDetail(int periodId)
        {
            //1.产品相关信息
            var product = prod.GetProductByPeriodId(periodId);
            //2.该类型产品进行进度
            var periods = prod.GetPeriods(product.ProductId).Take(3).ToList();
            //3.晒单记录
            //4.抢购记录
            //5.赠送记录

            ViewBag.Product = product;
            ViewBag.Periods = periods;

            return View(product);
        }

        public ActionResult SubmitOrder(int periodId,int userId=0)
        {
            userId = 3;
            var product = prod.GetProductByPeriodId(periodId);
            //用户信息
            BLL.UserInfo bllUser = new UserInfo();
           UserDTO user= bllUser.GetUserInfo(userId);
            List<T_UserAddr> lstAddr =bllUser.GetReceiveAddrs(userId).ToList();

            ViewBag.UserInfo = user;
            ViewBag.Addrs = lstAddr;
            return View(product);
        }

        public ActionResult PayOrder(string lottery,string phone,string email)
        {
            ViewBag.Lottery = lottery;
            ViewBag.Phone = phone;
            ViewBag.Email = email;

            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            JsApiPay jsApiPay = new JsApiPay();
            jsApiPay.openid = "orIUqxN88RpM6MLpS8K45cM8qNOc";
            jsApiPay.total_fee = int.Parse("1");
            //JSAPI支付预处理
            try
            {
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
              string  wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);
                //在页面上显示订单信息
                //Response.Write("<span style='color:#00CD00;font-size:20px'>订单详情：</span><br/>");
                //Response.Write("<span style='color:#00CD00;font-size:20px'>" + unifiedOrderResult.ToPrintStr() + "</span>");
                ViewBag.OrderResult = unifiedOrderResult.ToPrintStr();
            }
            catch (Exception ex)
            {
                //Response.Write("<span style='color:#FF0000;font-size:20px'>" + "下单失败，请返回重试" + "</span>");
                //submit.Visible = false;
            }
            
            return View();
        }

        public ActionResult PaySuccess()
        {
            return View();
        }

    }
}