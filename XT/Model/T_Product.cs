using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_Product
    public partial class T_Product
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
        /// <summary>
		/// IsActual
        /// </summary>		
		private int _isactual;
        public int IsActual
        {
            get { return _isactual; }
            set { _isactual = value; }
        }

        /// <summary>
        /// ProductUrl
        /// </summary>		
        private string _producturl;
        public string ProductUrl
        {
            get { return _producturl; }
            set { _producturl = value; }
        }

        /// <summary>
        /// ProductUrl
        /// </summary>		
        private string _productno;
        public string ProductNO
        {
            get { return _productno; }
            set { _productno = value; }
        }
        /// <summary>
        /// IsActual
        /// </summary>		
        private int _inventorynum;
        public int InventoryNum
        {
            get { return _inventorynum; }
            set { _inventorynum = value; }
        }

        /// <summary>
        /// IsActual
        /// </summary>		
        private int _incomingprice;
        public int IncomingPrice
        {
            get { return _incomingprice; }
            set { _incomingprice = value; }
        }
    }

    public partial class T_Product
    {
        public List<Model.T_ProductPeriods> prodPeriods { get; set; }

        //已购总数
        public int BuyTotalNum { get; set; }
        //参与人数
        public int OrderUserNum { get; set; }
        //已购X总数
        public int BuyXNum { get; set; }
        //参与X人数
        public int XUserNum { get; set; }
        //已购S总数
        public int BuySNum { get; set; }
        //参与S人数
        public int SUserNum { get; set; }
        //S领取数
        public int RevSNum { get; set; }
        //点击数量
        public int ClickNum { get; set; }
        //收藏数量
        public int Enshrine { get; set; }
        //总价格
        public int TotalProdPrice { get; set; }
        //总期数
        public int TotalPeriodNum { get; set; }
    }
}

