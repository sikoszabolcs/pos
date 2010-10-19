using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pos
{
    public class Product : pos.IProduct
    {
        public Product(string iName, decimal iPrice, bool iPstExempt)
        {
            _mCode = iName;
            _mPrice = iPrice;
            _mPstExempt = iPstExempt;
        }

        private readonly string _mCode;
        public string Code { get { return _mCode; } }

        private readonly decimal _mPrice;
        public decimal Price { get { return _mPrice; } }

        private readonly bool _mPstExempt;
        public bool PstExempt { get { return _mPstExempt; } }

    }
}
