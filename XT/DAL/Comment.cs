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
