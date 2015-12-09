using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BLL
{
    public class Comment
    {
        public IList<T_Comment> GetCommentListByPeroid(int periodId)
        {
            DAL.Comment comment = new DAL.Comment();
            return comment.GetCommentListByPeroid(periodId);
        }
    }
}
