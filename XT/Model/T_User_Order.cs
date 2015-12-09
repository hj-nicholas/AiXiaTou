using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_User_Order
    public class T_User_Order
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
        /// PeriodID
        /// </summary>		
        private int _periodid;
        public int PeriodID
        {
            get { return _periodid; }
            set { _periodid = value; }
        }
        /// <summary>
        /// IsWinner
        /// </summary>		
        private int _iswinner;
        public int IsWinner
        {
            get { return _iswinner; }
            set { _iswinner = value; }
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
        /// SupportNum
        /// </summary>		
        private int _supportnum;
        public int SupportNum
        {
            get { return _supportnum; }
            set { _supportnum = value; }
        }
        /// <summary>
        /// LotteryNum
        /// </summary>		
        private string _lotterynum;
        public string LotteryNum
        {
            get { return _lotterynum; }
            set { _lotterynum = value; }
        }

    }
}

