using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pos
{
    /// <summary>
    /// 
    /// </summary>
    public class BarcodeScanner
    {
        private IDisplay mDisplay;
        private List<Product> mProductList;

        public BarcodeScanner(List<Product> prlist, IDisplay iDisplay)
        {
            mProductList = prlist;
            mDisplay = iDisplay;
        }

        public BarcodeScanner(IDisplay iDisp)
        {
            mDisplay = iDisp;
        }

        public static decimal GetFedaralTax(decimal nRawPrice)
        {
            return nRawPrice * 5 / 100;
        }

        public static decimal GetProvincialTax(decimal nRawPrice)
        {
            return nRawPrice * 8 / 100;
        }

        public static decimal AddTax(Product iProduct)
        {
            return iProduct.Price + GetFedaralTax(iProduct.Price) +
                (iProduct.IsProvincialTaxApplicable ? GetProvincialTax(iProduct.Price) : 0);
        }

        public void Scan(string iCode)
        {
            Product pr = mProductList.Find(delegate(Product prod) { return prod.Code == iCode; });
            //decimal priceWithTax = AddTax(pr);
            mDisplay.PrintPrice(pr);
        }
    }
}
