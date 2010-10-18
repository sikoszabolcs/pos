using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pos
{
    public class Product : pos.IProduct
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
}
