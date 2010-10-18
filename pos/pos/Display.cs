using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pos
{
    public class Display : pos.IDisplay
    {
        private decimal mCurrentlyOnDisplay;

        private List<Product> mShoppingCart = new List<Product>();
        public virtual void PrintPrice(Product iProduct)
        {
            mCurrentlyOnDisplay = BarcodeScanner.AddTax(iProduct);
            mShoppingCart.Add(iProduct);
        }

        public virtual string PrintBill()
        {
            string lOutputBill = "";

            foreach (Product aProduct in mShoppingCart)
            {
                lOutputBill += aProduct.Price.ToString() + ";" + BarcodeScanner.AddTax(aProduct).ToString() + "|";
            }
            mShoppingCart.Clear();

            return lOutputBill;
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
}
