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
        public IList<ProductModel> GetProducts(int proType)
        {
            DAL.Product pro = new DAL.Product();
            IList<ProductModel> lst = pro.GetProducts(proType);
            return lst;
        }
    }
}
