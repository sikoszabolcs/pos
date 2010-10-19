using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;

namespace pos.Tests
{
    public class ManualDisplayMock : IDisplay
    {
        public decimal Price = 0;
        public void PrintPrice(Product iProduct)
        {
            Price = 0;
        }

        public string PrintBill()
        {
            return "";
        }
            
    }

    [TestFixture]
    class PosTest
    {
        private MockRepository _mocks;
        private IDisplay _mockDisplay;
        private BarcodeScannerDataProcessor _mBarcodeScannerDataProcessor;
        List<Product> _mPoducts;
        private TaxManager _mTaxManager;
        private Display _mDisplay;

        private decimal _mFederalTaxRate = (decimal) 0.05;
        private decimal _mProvincialTaxRate = (decimal) 0.08;

        [SetUp]
        public void SetUp()
        {
            _mocks = new MockRepository();
            _mockDisplay = _mocks.StrictMock<IDisplay>();
            
            _mPoducts = new List<Product>() {new Product("xyz", 12, false), 
                new Product("abc", 123, false), new Product("alma", 23, true),
                new Product("apple", 34, false), new Product("orange", 36, false), 
                new Product("kiwi", 19, true), new Product("grapes", 98, true)
            };

            _mBarcodeScannerDataProcessor = new BarcodeScannerDataProcessor(_mPoducts, _mockDisplay);

            _mTaxManager = new TaxManager(_mFederalTaxRate, _mProvincialTaxRate);
            _mDisplay = new Display(_mTaxManager);
        }


        [Test]
        public void TestAddMultipleProductsToShoppingCartAndPrintBill()
        {
            var lPrinter = new ConsolePrinter();
            var lBs = new BarcodeScannerDataProcessor(_mPoducts, _mDisplay, lPrinter);

            lBs.Scan("apple");
            lBs.Scan("orange");
            lBs.Scan("kiwi");
            lBs.Scan("grapes");

            string lsBill = lPrinter.PrintBill(_mDisplay.ShoppingCart);

            Assert.AreEqual("$34GP\r\n$36GP\r\n$19G\r\n$98G\r\nSubtotal: 187\r\nGST: 9.35\r\nPST: 5.60\r\n------\r\nTOTAL: 201.95\r\n", lsBill);
        }

        [Test]
        public void TestAddMultipleProductsToShoppingCart()
        {
            Expect.Call(() => _mockDisplay.PrintPrice(_mPoducts.Find(p => p.Code == "apple")));
            Expect.Call(() => _mockDisplay.PrintPrice(_mPoducts.Find(p => p.Code == "orange")));
            Expect.Call(() => _mockDisplay.PrintPrice(_mPoducts.Find(p => p.Code == "kiwi")));
            Expect.Call(() => _mockDisplay.PrintPrice(_mPoducts.Find(p => p.Code == "grapes")));

            _mocks.Replay(_mockDisplay);

            _mBarcodeScannerDataProcessor.Scan("apple");
            _mBarcodeScannerDataProcessor.Scan("orange");
            _mBarcodeScannerDataProcessor.Scan("kiwi");
            _mBarcodeScannerDataProcessor.Scan("grapes");

            _mocks.Verify(_mockDisplay);
        }


        [Test]
        public void TestProductPriceWithFederalTaxOnlyWithManualIDisplayMock()
        {
            var displayMock = new ManualDisplayMock();
            var lBs = new BarcodeScannerDataProcessor(_mPoducts, displayMock);

            lBs.Scan("xyz");
            Assert.AreEqual(Convert.ToDecimal(13.56), displayMock.Price);
        }

        [Test]
        public void TestPriceShouldBeDisplayed()
        {
            const string lProductCode = "xyz";
            Product lProd = _mPoducts.Find(p => p.Code == lProductCode);
            Expect.Call(() => _mockDisplay.PrintPrice(lProd));
            _mocks.Replay(_mockDisplay);

            _mBarcodeScannerDataProcessor.Scan(lProductCode);
            _mocks.Verify(_mockDisplay);
        }

        [Test]
        public void TestNoSuchProductFound()
        {
            const string lProductCode = "xyz";
            Product lProd = _mPoducts.Find(p => p.Code == lProductCode);
            Expect.Call(() => _mockDisplay.PrintPrice(lProd));
            _mocks.Replay(_mockDisplay);

            _mBarcodeScannerDataProcessor.Scan(lProductCode);
            _mocks.Verify(_mockDisplay);

            
        }

        [Test]
        public void TestPriceShouldBeDisplayedDuplicate()
        {
            const string lProductCode = "abc";
            Product lProd = _mPoducts.Find(p => p.Code == lProductCode);
            Expect.Call(() => _mockDisplay.PrintPrice(lProd));
            _mocks.Replay(_mockDisplay);

            _mBarcodeScannerDataProcessor.Scan(lProductCode);
            _mocks.Verify(_mockDisplay);
        }

        [Test]
        public void TestDisplayPriceWithAddedFederalTax()
        {
            const string lProductCode = "xyz";
            Product lProd = _mPoducts.Find(p => p.Code == lProductCode);
            Expect.Call(delegate { _mockDisplay.PrintPrice(lProd); });
            _mocks.Replay(_mockDisplay);

            _mBarcodeScannerDataProcessor.Scan("xyz");
            _mocks.Verify(_mockDisplay);

        }

        [Test]
        public void TestDisplayPriceWithAddedFederalAndProvincialTax()
        {
            const string lProductCode = "xyz";
            Product lProd = _mPoducts.Find(p => p.Code == lProductCode);

            using (_mocks.Record())
            {
                Expect.Call(() => _mockDisplay.PrintPrice(lProd));
            }

            using (_mocks.Playback())
            {
                _mBarcodeScannerDataProcessor.Scan(lProductCode);
            }
        }

        [Test]
        public void TestBarcodeScannerAndPrintPriceInteractionWithLastCall()
        {
            _mockDisplay.PrintPrice(null);
            LastCall.IgnoreArguments();
            _mocks.Replay(_mockDisplay);

            _mBarcodeScannerDataProcessor.Scan("xyz");
            _mocks.Verify(_mockDisplay);
        }


        [Test]
        public void TestTaxManagerFederalTaxRate()
        {
            decimal lFederalTaxRate = (decimal) 0.05;
            decimal lProvincialTaxRate = (decimal) 0.08;

            TaxManager lMyTaxManager = new TaxManager(lFederalTaxRate, lProvincialTaxRate);
            Product lProduct = new Product("test", (decimal)245.76, true);

            Assert.AreEqual(lProduct.Price + Decimal.Multiply(lProduct.Price, lFederalTaxRate), 
                lMyTaxManager.GetPriceWithAppliedFederalTax(lProduct));
        }

        [Test]
        public void TestTaxManagerPrivincialTaxRate()
        {
            decimal lFederalTaxRate = (decimal)0.05;
            decimal lProvincialTaxRate = (decimal)0.08;

            TaxManager lMyTaxManager = new TaxManager(lFederalTaxRate, lProvincialTaxRate);
            Product lProduct = new Product("test", (decimal)245.76, true);

            Assert.AreEqual(lProduct.Price + 
                (lProduct.PstExempt ? 0 : Decimal.Multiply(lProduct.Price, lProvincialTaxRate)) 
            ,lMyTaxManager.GetPriceWithAppliedProvincialTax(lProduct));
        }
       
        [Test]
        public void TestPrintBill()
        {

        }

      }

   
}
