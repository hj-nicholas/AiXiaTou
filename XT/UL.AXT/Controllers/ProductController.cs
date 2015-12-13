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
    }
}