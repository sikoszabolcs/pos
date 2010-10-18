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
    [TestFixture]
    class pos_test
    {
        private MockRepository mocks;
        private IDisplay mockDisplay;
        private BarcodeScanner mBarcodeScanner;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            mockDisplay = mocks.StrictMock<IDisplay>();
            mBarcodeScanner = new BarcodeScanner(mockDisplay);
        }

        [Test]
        public void Test_PriceShouldBeDisplayed()
        {
            Expect.Call(delegate { mockDisplay.PrintPrice((decimal)12.6); });
            mocks.Replay(mockDisplay);

            mBarcodeScanner.Scan("xyz");
            mocks.Verify(mockDisplay);
        }

        [Test]
        public void Test_PriceShouldBeDisplayed_Duplicate()
        {
            Expect.Call(delegate { mockDisplay.PrintPrice((decimal)129.15); });
            mocks.Replay(mockDisplay);

            mBarcodeScanner.Scan("abc");
            mocks.Verify(mockDisplay);
        }

        [Test]
        public void Test_DisplayPriceWithAddedFederalTax()
        {
            decimal lInitialPrice = 12.0M;
            decimal lPriceWithFederalTax = lInitialPrice + lInitialPrice * 5  / 100;

            Expect.Call(delegate { mockDisplay.PrintPrice(lPriceWithFederalTax); });
            mocks.Replay(mockDisplay);

            mBarcodeScanner.Scan("xyz");
            mocks.Verify(mockDisplay);

        }

        [Test]
        public void Test_DisplayPriceWithAddedFederalAndProvincialTax()
        {
            List<Product> products = new List<Product>() {new Product("xyz", 12, false), 
                new Product("abc", 123, false), new Product("alma", 23, true)};
            decimal lPriceWithProvincialTax = Convert.ToDecimal(23 * 1.13);
            BarcodeScanner mbs = new BarcodeScanner(products, mockDisplay);

            Expect.Call(delegate { mockDisplay.PrintPrice(lPriceWithProvincialTax); });
            mocks.Replay(mockDisplay);

            mbs.Scan("alma");
            mocks.Verify(mockDisplay);
        }

        [Test]
        public void Test_CreateBill()
        {



        }

        [Test]
        public void Test_CardReport()
        {
        }

        [Test]
        public void Test_CashReport()
        {
        }

        [Test]
        public void Test_EndOfTheDayReport()
        {

        }
    }
}
