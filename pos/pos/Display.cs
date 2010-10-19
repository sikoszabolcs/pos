using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pos
{
    public class Display : pos.IDisplay
    {
        private TaxManager _mTaxManager;
        protected decimal MCurrentlyOnDisplay;
        public readonly List<Product> ShoppingCart = new List<Product>();

        public Display(TaxManager iTaxManager)
        {
            _mTaxManager = iTaxManager;
        }

        public virtual void PrintPrice(Product iProduct)
        {
            MCurrentlyOnDisplay = _mTaxManager.GetTotalApplicableTax(iProduct);
            ShoppingCart.Add(iProduct);
        }
    }

    //public class ConsoleCurrencyDisplayer : Display
    //{
    //    private string _mCurrency = "$";
    //    public ConsoleCurrencyDisplayer(string iCurrency)
    //    {
    //        _mCurrency = iCurrency;
    //    }
    //    public ConsoleCurrencyDisplayer()
    //    {
    //    }
    //}
}
