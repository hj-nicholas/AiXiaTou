using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace XT.ASHX
{
    /// <summary>
    /// ShowOrder 的摘要说明
    /// </summary>
    public class ShowOrder : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string ation = context.Request["action"];
            switch (ation)
            {
                //展示晒单页面
                case "ShowOrder":
                    GetShowOrder(context);
                    break; 

            }
        }

        private void GetShowOrder(HttpContext context)
        {
            BLL.ShowOrder showOrder=new BLL.ShowOrder();
            List<ShowOrderModel>  lst = showOrder.GetShowingOrders().ToList();
            string jsonStr =JSONHelp.Instance().ToJson(lst);
            context.Response.Write(jsonStr);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}