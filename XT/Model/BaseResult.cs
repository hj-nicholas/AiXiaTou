using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
   public class BaseResult
    {
        public BaseResult()
        {
            Errors = new List<string>();
        }

        public bool Succeeded { get; set; }
       public string ErrMsg { get; set; }
       public List<string> Errors { get; set; }
        public string ResultId { get; set; }

    }
}
