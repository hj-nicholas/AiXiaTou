using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_Share_Get
    public class T_Share_Get
    {

        /// <summary>
        /// ShareId
        /// </summary>		
        private int _shareid;
        public int ShareId
        {
            get { return _shareid; }
            set { _shareid = value; }
        }
        /// <summary>
        /// GetId
        /// </summary>		
        private int _getid;
        public int GetId
        {
            get { return _getid; }
            set { _getid = value; }
        }
        /// <summary>
        /// GetUserId
        /// </summary>		
        private int _getuserid;
        public int GetUserId
        {
            get { return _getuserid; }
            set { _getuserid = value; }
        }
        /// <summary>
        /// GetNum
        /// </summary>		
        private int _getnum;
        public int GetNum
        {
            get { return _getnum; }
            set { _getnum = value; }
        }

    }
}

