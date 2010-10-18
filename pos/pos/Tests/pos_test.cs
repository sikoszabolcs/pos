using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using System.Collections;
//using NMock2;

namespace pos
{
    public class ManualDisplayMock : IDisplay
    {
        public decimal Price = 0;
        public void PrintPrice(Product iProduct)
        {
            Price = BarcodeScanner.AddTax(iProduct);
        }

        public string PrintBill()
        {
            return "";
        }
            
    }

    [TestFixture]
    class pos_test
    {
        private MockRepository mocks;
        private IDisplay mockDisplay;
        private BarcodeScanner mBarcodeScanner;
        List<Product> mPoducts;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            mockDisplay = mocks.StrictMock<IDisplay>();
            
            mPoducts = new List<Product>() {new Product("xyz", 12, false), 
                new Product("abc", 123, false), new Product("alma", 23, true),
                new Product("apple", 34, false), new Product("orange", 36, false), 
                new Product("kiwi", 19, true), new Product("grapes", 98, true)
            };

            mBarcodeScanner = new BarcodeScanner(mPoducts, mockDisplay);
        }

        [Test]
        public void Test_AddMultipleProductsToShoppingCartAndPrintBill()
        {
            Display lDs = new Display();
            BarcodeScanner lBs = new BarcodeScanner(mPoducts,lDs);

            lBs.Scan("apple");
            lBs.Scan("orange");
            lBs.Scan("kiwi");
            lBs.Scan("grapes");

            string lsBill = lDs.PrintBill();

            Assert.AreEqual("34;35.7|36;37.8|19;21.47|98;110.74|", lsBill);
        }

        [Test]
        public void Test_AddMultipleProductsToShoppingCart()
        {
            Expect.Call(delegate { mockDisplay.PrintPrice(mPoducts.Find(delegate(Product prod) { return prod.Code == "apple"; })); });
            Expect.Call(delegate { mockDisplay.PrintPrice(mPoducts.Find(delegate(Product prod) { return prod.Code == "orange"; })); });
            Expect.Call(delegate { mockDisplay.PrintPrice(mPoducts.Find(delegate(Product prod) { return prod.Code == "kiwi"; })); });
            Expect.Call(delegate { mockDisplay.PrintPrice(mPoducts.Find(delegate(Product prod) { return prod.Code == "grapes"; })); });

            mocks.Replay(mockDisplay);

            mBarcodeScanner.Scan("apple");
            mBarcodeScanner.Scan("orange");
            mBarcodeScanner.Scan("kiwi");
            mBarcodeScanner.Scan("grapes");

            mocks.Verify(mockDisplay);
        }


        [Test]
        public void Test_ProductPriceWithFederalTaxOnly_With_ManualIDisplayMock()
        {
            var displayMock = new ManualDisplayMock();
            BarcodeScanner lBs = new BarcodeScanner(mPoducts, displayMock);

            lBs.Scan("xyz");
            Assert.AreEqual(Convert.ToDecimal(12.6), displayMock.Price);
        }

        [Test]
        public void Test_PriceShouldBeDisplayed()
        {
            string lProductCode = "xyz";
            Product lProd = mPoducts.Find(delegate(Product prod) { return prod.Code == lProductCode; });
            Expect.Call(delegate { mockDisplay.PrintPrice(lProd); });
            mocks.Replay(mockDisplay);

            mBarcodeScanner.Scan(lProductCode);
            mocks.Verify(mockDisplay);
        }

        [Test]
        public void Test_PriceShouldBeDisplayed_Duplicate()
        {
            string lProductCode = "abc";
            Product lProd = mPoducts.Find(delegate(Product prod) { return prod.Code == lProductCode; });
            Expect.Call(delegate { mockDisplay.PrintPrice(lProd); });
            mocks.Replay(mockDisplay);

            mBarcodeScanner.Scan(lProductCode);
            mocks.Verify(mockDisplay);
        }

        [Test]
        public void Test_DisplayPriceWithAddedFederalTax()
        {
            string lProductCode = "xyz";
            Product lProd = mPoducts.Find(delegate(Product prod) { return prod.Code == lProductCode; });
            Expect.Call(delegate { mockDisplay.PrintPrice(lProd); });
            mocks.Replay(mockDisplay);

            mBarcodeScanner.Scan("xyz");
            mocks.Verify(mockDisplay);

        }

        [Test]
        public void Test_DisplayPriceWithAddedFederalAndProvincialTax()
        {

            string lProductCode = "xyz";
            Product lProd = mPoducts.Find(delegate(Product prod) { return prod.Code == lProductCode; });
            //Expect.Call(delegate { mockDisplay.PrintPrice(lProd); });

            using (mocks.Record())
            {
                Expect.Call(delegate { mockDisplay.PrintPrice(lProd); });
            }

            using (mocks.Playback())
            {
                mBarcodeScanner.Scan(lProductCode);
            }
        }

        [Test]
        public void Test_BarcodeScanner_And_PrintPrice_Interaction_With_LastCall()
        {
            mockDisplay.PrintPrice(null);
            LastCall.IgnoreArguments();
            mocks.Replay(mockDisplay);

            mBarcodeScanner.Scan("xyz");
            mocks.Verify(mockDisplay);
        }

       
        [Test]
        public void Test_DailyCardReport()
        {
        }

        [Test]
        public void Test_DailyCashReport()
        {
        }

        [Test]
        public void Test_DailyReport()
        {

        }
    }
}
