using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_User_Enshrine
    public class T_User_Enshrine
    {

        /// <summary>
        /// EnshrineId
        /// </summary>		
        private int _enshrineid;
        public int EnshrineId
        {
            get { return _enshrineid; }
            set { _enshrineid = value; }
        }
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
        /// PeriodId
        /// </summary>		
        private int _periodid;
        public int PeriodId
        {
            get { return _periodid; }
            set { _periodid = value; }
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

