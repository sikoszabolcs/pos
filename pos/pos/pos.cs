using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pos
{
    public class Display : pos.IDisplay
    {
        private int mCurrentlyOnDisplay = 0;
        public virtual int PrintPrice(int iVal)
        {
            mCurrentlyOnDisplay = iVal;
            return iVal;
        }

        public void Dummy()
        {

        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class BarcodeScanner
    {   
        private Dictionary<string, int> mDB = new Dictionary<string, int>();
        private IDisplay mDisplay;
        
        public BarcodeScanner(Dictionary<string, int> iDB)
        {
            mDB = iDB;
        }

        public BarcodeScanner(IDisplay iDisp)
        {
            mDisplay = iDisp;
        }

        public int Scan(string iCode)
        {
            return mDisplay.PrintPrice(12);
        }
    }
}
