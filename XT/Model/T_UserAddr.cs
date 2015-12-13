using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_UserAddr
    public class T_UserAddr
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
        /// AddressDesc
        /// </summary>		
        private string _addressdesc;
        public string AddressDesc
        {
            get { return _addressdesc; }
            set { _addressdesc = value; }
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

        //收件人
        private string _receiver;
        public string Receiver
        {
            get { return _receiver; }
            set { _receiver = value; }
        }

        //收件人电话
        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

    }
}

