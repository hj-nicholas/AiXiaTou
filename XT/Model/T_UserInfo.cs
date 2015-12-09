using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_UserInfo
    public class T_UserInfo
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
        /// WeChatName
        /// </summary>		
        private string _wechatname;
        public string WeChatName
        {
            get { return _wechatname; }
            set { _wechatname = value; }
        }
        /// <summary>
        /// UserPassword
        /// </summary>		
        private string _userpassword;
        public string UserPassword
        {
            get { return _userpassword; }
            set { _userpassword = value; }
        }
        /// <summary>
        /// AccountBalance
        /// </summary>		
        private int _accountbalance;
        public int AccountBalance
        {
            get { return _accountbalance; }
            set { _accountbalance = value; }
        }
        /// <summary>
        /// PhotoPath
        /// </summary>		
        private string _photopath;
        public string PhotoPath
        {
            get { return _photopath; }
            set { _photopath = value; }
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

    }
}

