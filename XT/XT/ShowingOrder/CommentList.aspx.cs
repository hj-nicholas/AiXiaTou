using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;

namespace XT.ShowingOrder
{
    public partial class CommentList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int periodId=Convert.ToInt32( Request.QueryString["peroid"]);
            BLL.Comment comment = new Comment();
            List<T_Comment> lst=comment.GetCommentListByPeroid(periodId).ToList();
        }
    }
}