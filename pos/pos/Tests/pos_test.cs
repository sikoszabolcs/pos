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
            Expect.Call(mockDisplay.Dummy);
            //Expect.Call(mockDisplay.PrintPrice(12)).Return(0);
            mocks.ReplayAll();
            int result = mBarcodeScanner.Scan("xyz");

            mocks.VerifyAll();
        }
    }

}
