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
            decimal lInitialPrice = Convert.ToDecimal(23);
            decimal lPriceWithProvincialTax = lInitialPrice + decimal.Multiply(lInitialPrice, 8) / 100 +
                decimal.Multiply(lInitialPrice, 5) / 100;

            Expect.Call(delegate { mockDisplay.PrintPrice(lPriceWithProvincialTax); });
            mocks.Replay(mockDisplay);

            mBarcodeScanner.Scan("alma");
            mocks.Verify(mockDisplay);
        }
    }
}
