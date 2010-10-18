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
            Expect.Call(delegate { mockDisplay.PrintPrice(12); });
            mocks.ReplayAll();

            mBarcodeScanner.Scan("xyz");
            mocks.Verify(mockDisplay);
        }

        [Test]
        public void Test_PriceShouldBeDisplayed_Rec()
        {
            Expect.Call(delegate { mockDisplay.PrintPrice(123); });
            mocks.Replay(mockDisplay);

            mBarcodeScanner.Scan("abc");
            mocks.Verify(mockDisplay);
        }

        [Test]
        public void Test_DisplayPriceWithAddedFederalTax()
        {
            int lInitialPrice = 12;
            int lPriceWithFederalTax = lInitialPrice + 5 / 100;

            Expect.Call(delegate { mockDisplay.PrintPrice(lPriceWithFederalTax); });
            mocks.Replay(mockDisplay);

            mBarcodeScanner.Scan("xyz");
            mocks.Verify(mockDisplay);

        }
    }
}
