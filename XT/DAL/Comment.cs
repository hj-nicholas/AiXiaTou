using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Model;

namespace DAL
{
  public  class Comment
    {
        private string strConn = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;

       

      public IList<CommentDTO> GetCommentListByPeroid(int periodId)
      {
            try
            {
                IList<CommentDTO> lstComment = new List<CommentDTO>();
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetCommentListByPeriodId", strConn);
                cmd.Parameters["@i_periodId"].Value = periodId.ToString();
                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        CommentDTO comm = new CommentDTO();
                        comm.CommentContent = Convert.ToString(rdr["CommentContent"]);
                        comm.CommentDate = Convert.ToDateTime(rdr["CommentDate"]);
                        comm.CommentID = Convert.ToInt32(rdr["CommentID"]);
                        if (!Convert.IsDBNull(rdr["CommentRefID"]))
                            comm.CommentRefID = Convert.ToInt32(rdr["CommentRefID"]);
                        comm.UserId = Convert.ToInt32(rdr["UserId"]);
                        comm.PeriodID = Convert.ToInt32(rdr["PeriodId"]);
                        comm.Commenter= Convert.ToString(rdr["Commenter"]);
                        comm.CommentRefer = Convert.ToString(rdr["CommentRefer"]);
                        comm.UserImage = Convert.ToString(rdr["UserImage"]);

                        lstComment.Add(comm);
                    }
                }

                return lstComment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //添加点赞
        public BaseResult AddSupport(int userId, int periodId)
        {
            BaseResult br = new BaseResult();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pAddSupport", strConn);
                cmd.Parameters["@p_userId"].Value = userId;
                cmd.Parameters["@p_periodId"].Value = periodId;
                cmd.Parameters["@o_count"].Value = 0;
                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                br.Succeeded = true;
                br.ResultId = cmd.Parameters["@o_count"].Value.ToString();
            }
            catch (Exception ex)
            {
                br.Succeeded = false;
                br.ErrMsg = ex.Message;
            }
            return br;
        }

        public BaseResult AddReply(int userId,int commRefId, int periodId,string replyContent)
        {
            BaseResult br = new BaseResult();
            try
            {
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pAddReply", strConn);
                cmd.Parameters["@p_userId"].Value = userId;
                cmd.Parameters["@p_periodId"].Value = periodId;
                cmd.Parameters["@p_commRefId"].Value = commRefId;
                cmd.Parameters["@p_replyContent"].Value = replyContent;
                SQLHelper.Instance().ExecuteNonQuery(strConn, cmd);

                br.Succeeded = true;
                
            }
            catch (Exception ex)
            {
                br.Succeeded = false;
                br.ErrMsg = ex.Message;
            }
            return br;
        }

        public IList<CommentDTO> GetCommentListByUserid(int userId)
        {
            try
            {
                IList<CommentDTO> lstComment = new List<CommentDTO>();
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetCommentListByUserId", strConn);
                cmd.Parameters["@p_userId"].Value = userId;
                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        CommentDTO comm = new CommentDTO();
                        comm.CommentContent = Convert.ToString(rdr["CommentContent"]);
                        comm.CommentDate = Convert.ToDateTime(rdr["CommentDate"]);
                        comm.CommentID = Convert.ToInt32(rdr["CommentID"]);
                        if (!Convert.IsDBNull(rdr["CommentRefID"]))
                            comm.CommentRefID = Convert.ToInt32(rdr["CommentRefID"]);
                        comm.UserId = Convert.ToInt32(rdr["UserId"]);
                        comm.PeriodID = Convert.ToInt32(rdr["PeriodId"]);
                        comm.Commenter = Convert.ToString(rdr["Commenter"]);
                        comm.CommentRefer = Convert.ToString(rdr["CommentRefer"]);
                        comm.UserImage = Convert.ToString(rdr["UserImage"]);

                        lstComment.Add(comm);
                    }
                }

                return lstComment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
