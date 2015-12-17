using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_UserInfo
    public partial class UserDTO
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

        private string _cellphone;
        public string Cellphone
        {
            get { return _cellphone; }
            set { _cellphone = value; }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        private string _openId;
        public string OpenId
        {
            get { return _openId; }
            set { _openId = value; }
        }

    }

    public  partial class  UserDTO
    {
        //用户地址
         public List<T_UserAddr> UserAddrs { get; set; }
        //用户参加数量
        public int JoinedNum { get; set; }
    }
}

