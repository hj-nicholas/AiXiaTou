using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_Account
    public partial class T_Account
    {

        /// <summary>
        /// AccountId
        /// </summary>		
        private int _accountid;
        public int AccountId
        {
            get { return _accountid; }
            set { _accountid = value; }
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
        /// CosteAmount
        /// </summary>		
        private int _costeamount;
        public int CosteAmount
        {
            get { return _costeamount; }
            set { _costeamount = value; }
        }
        /// <summary>
        /// AccountDesc
        /// </summary>		
        private string _accountdesc;
        public string AccountDesc
        {
            get { return _accountdesc; }
            set { _accountdesc = value; }
        }
        /// <summary>
        /// ConsumeTime
        /// </summary>		
        private DateTime _consumetime;
        public DateTime ConsumeTime
        {
            get { return _consumetime; }
            set { _consumetime = value; }
        }

        /// <summary>
        /// CosteAmount
        /// </summary>		
        private int _accountye;
        public int AccountYE
        {
            get { return _accountye; }
            set { _accountye = value; }
        }

    }

    public partial class T_Account
    {
        public int AccountBalance { get; set; }
    }
}

