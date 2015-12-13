using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_ProductPeriods
    public class T_ProductPeriods
    {

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
        /// PeriodID
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
        /// valid
        /// </summary>		
        private int _valid;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        /// <summary>
        /// ProLotteryNum
        /// </summary>		
        private string _prolotterynum;
        public string ProLotteryNum
        {
            get { return _prolotterynum; }
            set { _prolotterynum = value; }
        }
        /// <summary>
        /// ProductExpires
        /// </summary>		
        private DateTime _productexpires;
        public DateTime ProductExpires
        {
            get { return _productexpires; }
            set { _productexpires = value; }
        }
        /// <summary>
        /// JoinedNum
        /// </summary>		
        private int _joinednum;
        public int JoinedNum
        {
            get { return _joinednum; }
            set { _joinednum = value; }
        }
        /// <summary>
        /// EnvyNum
        /// </summary>		
        private int _envynum;
        public int EnvyNum
        {
            get { return _envynum; }
            set { _envynum = value; }
        }
        /// <summary>
        /// ProductPrice
        /// </summary>		
        private decimal _productprice;
        public decimal ProductPrice
        {
            get { return _productprice; }
            set { _productprice = value; }
        }
        /// <summary>
        /// IsShowOrder
        /// </summary>		
        private int _isshoworder;
        public int IsShowOrder
        {
            get { return _isshoworder; }
            set { _isshoworder = value; }
        }
        /// <summary>
		/// 创建时间
        /// </summary>		
		private DateTime _createtime;
        public DateTime CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }

    }
}

