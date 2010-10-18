using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pos
{
    public class Display : pos.IDisplay
    {
        private int mCurrentlyOnDisplay = 0;
        public virtual void PrintPrice(int iVal)
        {
            mCurrentlyOnDisplay = iVal;
        }
    }

    public class DisplayWithCurrency : Display
    {
        private string mCurrency = "RON";
        public DisplayWithCurrency(string iCurrency)
        {
            mCurrency = iCurrency;
        }
        public DisplayWithCurrency()
        {
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class BarcodeScanner
    {
        //private Dictionary<string, int> mDB = new Dictionary<string, int>();
        private IDisplay mDisplay;

        //public BarcodeScanner(Dictionary<string, int> iDB)
        //{
        //    mDB = iDB;
        //}

        public BarcodeScanner(IDisplay iDisp)
        {
            mDisplay = iDisp;
        }

        public int AddFedaralTax(int nRawPrice)
        {
            return nRawPrice + nRawPrice * 5 / 100;
        }

        public void Scan(string iCode)
        {
            Dictionary<string, int> prices = new Dictionary<string, int> { { "xyz", 12 }, { "abc", 123 } };

            mDisplay.PrintPrice(AddFedaralTax(prices[iCode]));
        }
    }
}
