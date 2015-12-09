using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_ProductType
    public class T_ProductType
    {

        /// <summary>
        /// ProductTypeId
        /// </summary>		
        private int _producttypeid;
        public int ProductTypeId
        {
            get { return _producttypeid; }
            set { _producttypeid = value; }
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

    }
}

