using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_User_Award
    public class T_User_Award
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
        /// AddressId
        /// </summary>		
        private int _addressid;
        public int AddressId
        {
            get { return _addressid; }
            set { _addressid = value; }
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
        /// AwardNo
        /// </summary>		
        private string _awardno;
        public string AwardNo
        {
            get { return _awardno; }
            set { _awardno = value; }
        }
        /// <summary>
        /// AwartTime
        /// </summary>		
        private DateTime _awarttime;
        public DateTime AwartTime
        {
            get { return _awarttime; }
            set { _awarttime = value; }
        }

    }
}

