using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL
{
  public  class Comment
    {
        private string strConn = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;

       

      public IList<T_Comment> GetCommentListByPeroid(int periodId)
      {
            try
            {
                IList<T_Comment> lstComment = new List<T_Comment>();
                SqlCommand cmd = SQLHelper.Instance().CreateSqlCommand("pGetCommentListByPeriodId", strConn);
                //cmd.Parameters.Add(new SqlParameter ( "@i_periodId", SqlDbType.Int));
                cmd.Parameters["@i_periodId"].Value = periodId.ToString();
                using (SqlDataReader rdr = SQLHelper.Instance().ExecuteReader(strConn, cmd))
                {
                    while (rdr.Read())
                    {
                        T_Comment comm = new T_Comment();
                        comm.CommentContent = Convert.ToString(rdr["CommentContent"]);
                        comm.CommentDate = Convert.ToDateTime(rdr["CommentDate"]);
                        comm.CommentID = Convert.ToInt32(rdr["CommentID"]);
                        //if(string.IsNullOrEmpty(rdr["CommentRefID"].ToString()) )
                        //    comm.CommentRefID = Convert.ToInt32(rdr["CommentRefID"]);
                        comm.UserId = Convert.ToInt32(rdr["UserId"]);
                        comm.PeriodID = Convert.ToInt32(rdr["PeriodId"]);
                        
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
