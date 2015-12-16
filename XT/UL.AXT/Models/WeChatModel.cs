using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL.AXT.Models
{
   public class WeChatModel
    {
    }

    public class PostModel
    {
        public string Signature { get; set; }
        public string Timestamp { get; set; }
        public string Nonce { get; set; }

        public string echoStr { get; set; }
    }
}
