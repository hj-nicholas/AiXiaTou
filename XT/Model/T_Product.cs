using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_Product
    public class T_Product
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
        /// ProductName
        /// </summary>		
        private string _productname;
        public string ProductName
        {
            get { return _productname; }
            set { _productname = value; }
        }
        /// <summary>
        /// ProductDesc
        /// </summary>		
        private string _productdesc;
        public string ProductDesc
        {
            get { return _productdesc; }
            set { _productdesc = value; }
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
        /// ProductType
        /// </summary>		
        private int _producttype;
        public int ProductType
        {
            get { return _producttype; }
            set { _producttype = value; }
        }

    }
}

