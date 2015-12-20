using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
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

        public ActionResult SubmitOrder(int periodId)
        {
            var product = prod.GetProductByPeriodId(periodId);
            return View(product);
        }

        public ActionResult PayOrder()
        {
            return View();
        }

        public ActionResult PaySuccess()
        {
            return View();
        }

    }
}