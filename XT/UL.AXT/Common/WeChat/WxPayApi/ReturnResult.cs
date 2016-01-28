using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoo.WeChat.WxPayAPI
{
    public class ReturnResult
    {
      public string return_code{ get; set; }
      public string return_msg{ get; set; }
      public string sign{ get; set; }
      public string result_code{ get; set; }
      public string err_code{ get; set; }
      public string err_code_des{ get; set; }
      public string mch_billno{ get; set; }
      public string mch_id { get; set; }
      public string wxappid { get; set; }
      public string re_openid { get; set; }
      public string total_amount { get; set; }
    }
}
