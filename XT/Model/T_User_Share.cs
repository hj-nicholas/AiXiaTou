using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_User_Share
    public partial class T_User_Share
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
        /// PeriodId
        /// </summary>		
        private int _periodid;
        public int PeriodId
        {
            get { return _periodid; }
            set { _periodid = value; }
        }
        /// <summary>
        /// UnGetNum
        /// </summary>		
        private int _ungetnum;
        public int UnGetNum
        {
            get { return _ungetnum; }
            set { _ungetnum = value; }
        }
        /// <summary>
        /// ShareUserId
        /// </summary>		
        private int _shareuserid;
        public int ShareUserId
        {
            get { return _shareuserid; }
            set { _shareuserid = value; }
        }
        /// <summary>
        /// ShareNum
        /// </summary>		
        private int _sharenum;
        public int ShareNum
        {
            get { return _sharenum; }
            set { _sharenum = value; }
        }
        /// <summary>
        /// PeopleNum
        /// </summary>		
        private int _peoplenum;
        public int PeopleNum
        {
            get { return _peoplenum; }
            set { _peoplenum = value; }
        }
        /// <summary>
        /// Valid
        /// </summary>		
        private int _valid;
        public int Valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        /// <summary>
        /// IsPay
        /// </summary>		
        private int _ispay;
        public int IsPay
        {
            get { return _ispay; }
            set { _ispay = value; }
        }

    }

    public partial class T_User_Share
    {
        //领取礼物人数
        public int RevPeopleNum { get; set; }
        //领取礼物数量
        public int RevGiftNum { get; set; }
    }
}

