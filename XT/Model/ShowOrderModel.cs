using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
   public class ShowOrderModel
    {
        /// <summary>
        /// UserID
        /// </summary>		
        private int _userid;
        public int UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        /// UserName
        /// </summary>		
        private string _username;
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        /// <summary>
        /// CommentID
        /// </summary>		
        private int _commentid;
        public int CommentID
        {
            get { return _commentid; }
            set { _commentid = value; }
        }
        /// <summary>
        /// ProductID
        /// </summary>		
        private int _periodid;
        public int PeriodID
        {
            get { return _periodid; }
            set { _periodid = value; }
        }
        /// <summary>
        /// PeriodNum
        /// </summary>		
        private string _periodnum;
        public string PeriodNum
        {
            get { return _periodnum; }
            set { _periodnum = value; }
        }
        /// <summary>
        /// ProductId
        /// </summary>		
        private int _productid;
        public int ProductId
        {
            get { return _productid; }
            set { _productid = value; }
        }
        /// <summary>
        /// ProductName
        /// </summary>		
        private string _productname;
        public string ProductName
        {
            get { return _productname; }
            set { _productname = value; }
        }

        //晒单照片
       public List<T_Photo> Photos { get; set; }

       public int CommentNum { get; set; }
       public int SupportNum { get; set; }

        /// <summary>
        /// CommentContent
        /// </summary>		
        private string _commentcontent;
        public string CommentContent
        {
            get { return _commentcontent; }
            set { _commentcontent = value; }
        }
        /// <summary>
        /// CommentDatet
        /// </summary>		
        private DateTime _commentdate;
        public DateTime CommentDate
        {
            get { return _commentdate; }
            set { _commentdate = value; }
        }

    }
}
