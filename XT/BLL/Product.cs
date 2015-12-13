using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BLL
{
    public class Product
    {
        DAL.Product pro = new DAL.Product();
        public IList<ProductModel> GetProducts(int proType)
        {
            IList<ProductModel> lst = pro.GetProducts(proType);
            return lst;
        }

        public ProductModel GetProductByPeriodId(int periodId)
        {
            var product = pro.GetProductByPeriodId(periodId);
            return product;

        }

        public IList<T_ProductPeriods> GetPeriods(int productId)
        {
            var periods = pro.GetPeriods(productId);
            return periods;
        }
    }
}
