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
        DAL.Comment comment = new DAL.Comment();

        public IList<CommentDTO> GetCommentListByPeroid(int periodId)
        {
            return comment.GetCommentListByPeroid(periodId);
        }

        public BaseResult AddSupport(int userId, int periodId)
        {
            return comment.AddSupport(userId, periodId);
        }

        public BaseResult AddReply(int userId, int commRefId, int periodId, string replyContent)
        {
            return comment.AddReply(userId, commRefId, periodId, replyContent);
        }

        public IList<CommentDTO> GetCommentListByUserid(int userId)
        {
            return comment.GetCommentListByUserid(userId);
        }
    }
}