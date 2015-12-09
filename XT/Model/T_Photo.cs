using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_Photo
    public class T_Photo
    {

        /// <summary>
        /// PhotoID
        /// </summary>		
        private int _photoid;
        public int PhotoID
        {
            get { return _photoid; }
            set { _photoid = value; }
        }
        /// <summary>
        /// PhotoPath
        /// </summary>		
        private string _photopath;
        public string PhotoPath
        {
            get { return _photopath; }
            set { _photopath = value; }
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
        /// PhotoType
        /// </summary>		
        private int _phototype;
        public int PhotoType
        {
            get { return _phototype; }
            set { _phototype = value; }
        }
        /// <summary>
        /// RelId
        /// </summary>		
        private int _relid;
        public int RelId
        {
            get { return _relid; }
            set { _relid = value; }
        }

    }
}

