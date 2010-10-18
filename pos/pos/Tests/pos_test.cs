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
        List<Product> mPoducts;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            mockDisplay = mocks.StrictMock<IDisplay>();
            
            mPoducts = new List<Product>() {new Product("xyz", 12, false), 
                new Product("abc", 123, false), new Product("alma", 23, true)};

            mBarcodeScanner = new BarcodeScanner(mPoducts, mockDisplay);
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
            decimal lInitialPrice = Convert.ToDecimal(12);
            decimal lPriceWithFederalTax = lInitialPrice * Convert.ToDecimal(1.05);

            Expect.Call(delegate { mockDisplay.PrintPrice(lPriceWithFederalTax); });
            mocks.Replay(mockDisplay);

            mBarcodeScanner.Scan("xyz");
            mocks.Verify(mockDisplay);

        }

        [Test]
        public void Test_DisplayPriceWithAddedFederalAndProvincialTax()
        {
            decimal lPriceWithFederalAndProvincialTax = Convert.ToDecimal(23 * 1.13);

            using (mocks.Record())
            {
                Expect.Call(delegate { mockDisplay.PrintPrice(lPriceWithFederalAndProvincialTax); });
            }

            using (mocks.Playback())
            {
                mBarcodeScanner.Scan("alma");    
            }
        }


        [Test]
        public void Test_LastCall()
        {
            //decimal lInitialPrice = Convert.ToDecimal(12);
            //decimal lPriceWithFederalTax = lInitialPrice * Convert.ToDecimal(1.05);

            //Expect.Call(delegate { mockDisplay.PrintPrice(lPriceWithFederalTax); });
            mockDisplay.PrintPrice(0);
            LastCall.IgnoreArguments().Repeat.Once();

            mocks.Replay(mockDisplay);

            mBarcodeScanner.Scan("xyz");
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
