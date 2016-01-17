using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_Red_Envelope
    public partial class T_Red_Envelope
    {

        /// <summary>
        /// EnvelopeId
        /// </summary>		
        private int _envelopeid;
        public int EnvelopeId
        {
            get { return _envelopeid; }
            set { _envelopeid = value; }
        }
        /// <summary>
        /// DonateUserId
        /// </summary>		
        private int _donateuserid;
        public int DonateUserId
        {
            get { return _donateuserid; }
            set { _donateuserid = value; }
        }
        /// <summary>
        /// RevUserId
        /// </summary>		
        private int _revuserid;
        public int RevUserId
        {
            get { return _revuserid; }
            set { _revuserid = value; }
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
        /// RevTime
        /// </summary>		
        private DateTime _revtime;
        public DateTime RevTime
        {
            get { return _revtime; }
            set { _revtime = value; }
        }

    }

    public partial class T_Red_Envelope
    {
        public string RevUserName { get; set; }
        public string RevUserPhoto { get; set; }
    }
}

