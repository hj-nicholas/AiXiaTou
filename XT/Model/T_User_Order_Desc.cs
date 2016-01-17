using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_User_Order_Desc
    public class T_User_Order_Desc
    {

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
        /// PeriodId
        /// </summary>		
        private int _periodid;
        public int PeriodId
        {
            get { return _periodid; }
            set { _periodid = value; }
        }
        /// <summary>
        /// OrderNO
        /// </summary>		
        private string _orderno;
        public string OrderNO
        {
            get { return _orderno; }
            set { _orderno = value; }
        }
        /// <summary>
        /// LotteryNO
        /// </summary>		
        private string _lotteryno;
        public string LotteryNO
        {
            get { return _lotteryno; }
            set { _lotteryno = value; }
        }
        /// <summary>
        /// UserDesc
        /// </summary>		
        private string _userdesc;
        public string UserDesc
        {
            get { return _userdesc; }
            set { _userdesc = value; }
        }
        /// <summary>
        /// UserAddr
        /// </summary>		
        private string _useraddr;
        public string UserAddr
        {
            get { return _useraddr; }
            set { _useraddr = value; }
        }
        /// <summary>
        /// UserPhone
        /// </summary>		
        private string _userphone;
        public string UserPhone
        {
            get { return _userphone; }
            set { _userphone = value; }
        }
        /// <summary>
		/// OrderId
        /// </summary>		
		private int _orderid;
        public int OrderId
        {
            get { return _orderid; }
            set { _orderid = value; }
        }

        /// <summary>
        /// UserAddr
        /// </summary>		
        private string _orderemail;
        public string OrderEmail
        {
            get { return _orderemail; }
            set { _orderemail = value; }
        }
        /// <summary>
		/// OrderId
        /// </summary>		
		private int _ispay;
        public int IsPay
        {
            get { return _ispay; }
            set { _ispay = value; }
        }

        /// <summary>
        /// OrderId
        /// </summary>		
        private int _buyNum;
        public int BuyNum
        {
            get { return _buyNum; }
            set { _buyNum = value; }
        }

        //使用的余额
        public int UsedYE { get; set; }
    }
}

