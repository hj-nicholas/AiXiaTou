using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_Comment
    public partial class CommentDTO
    {

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
        /// UserId
        /// </summary>		
        private int _userid;
        public int UserId
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        /// CommentRefID
        /// </summary>		
        private int _commentrefid;
        public int CommentRefID
        {
            get { return _commentrefid; }
            set { _commentrefid = value; }
        }
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
        /// valid
        /// </summary>		
        private int _valid;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
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

    public partial class CommentDTO
    {
        //���� ���۵���
        public string Commenter { get; set; }

        //�����۵���
        public string CommentRefer { get; set; }
        
        public string getEel{get;set;}

        public string UserImage { get; set; }
    }
}

