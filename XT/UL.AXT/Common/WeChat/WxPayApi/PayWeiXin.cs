using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoo.WeChat.WxPayAPI
{
    public class PayWeiXin
    {
      public string nonce_str { get; set; }
      public string sign { get; set; }
      public string mch_billno{ get; set; }
      public string mch_id { get; set; }
      public string wxappid { get; set; }
      public string nick_name{ get; set; }
      public string send_name { get; set; }
      public string re_openid { get; set; }
      public int total_amount { get; set; }      public int min_value{ get; set; }
      public int max_value{ get; set; }
      public int total_num{ get; set; }
      public string wishing { get; set; }      public string client_ip{ get; set; }
      public string act_id { get; set; }
      public string act_name{ get; set; }
      public string remark { get; set; }      public string logo_imgurl { get; set; }      public string share_content{ get; set; }
      public string share_url { get; set; }
      public string share_imgurl{ get; set; }

    }
}
