using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pos
{
    public class Display : pos.IDisplay
    {
        private decimal mCurrentlyOnDisplay = 0;
        public virtual void PrintPrice(decimal iVal)
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

    public class Product
    {
        public Product(string iName, decimal iPrice, bool iIsProvincialTaxApplicable)
        {
            mCode = iName;
            mPrice = iPrice;
            mIsProvincialTaxApplicable = iIsProvincialTaxApplicable;
        }

        private string mCode;
        public string Code { get { return mCode; } }

        private decimal mPrice;
        public decimal Price { get { return mPrice; } }

        private bool mIsProvincialTaxApplicable;
        public bool IsProvincialTaxApplicable { get { return mIsProvincialTaxApplicable; } }

    }
    /// <summary>
    /// 
    /// </summary>
    public class BarcodeScanner
    {
        private IDisplay mDisplay;

        public BarcodeScanner(IDisplay iDisp)
        {
            mDisplay = iDisp;
        }

        private decimal GetFedaralTax(decimal nRawPrice)
        {
            return nRawPrice * 5 / 100;
        }

        private decimal GetProvincialTax(decimal nRawPrice)
        {
            return nRawPrice * 8 / 100;
        }

        public decimal AddTax(Product iProduct)
        {
            return iProduct.Price + GetFedaralTax(iProduct.Price) +
                (iProduct.IsProvincialTaxApplicable ? GetProvincialTax(iProduct.Price) : 0);
        }

        public void Scan(string iCode)
        {
            //Dictionary<string, decimal> prices = new Dictionary<string, decimal> { { "xyz", 12 }, { "abc", 123 } };
            List<Product> products = new List<Product>() {new Product("xyz", 12, false), 
                new Product("abc", 123, false), new Product("alma", 23, true)};

            Product pr = products.Find(delegate(Product prod) { return prod.Code == iCode; });
            mDisplay.PrintPrice(AddTax(pr));
        }
    }
}
