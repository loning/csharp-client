using BitPayAPI;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BitPayTest
{
    [TestClass]
    public class BitPayTest
    {
        private BitPay bitpay;
        private Invoice basicInvoice;
        private static String API_KEY = "6b2ffa7a-40d2-4ff6-ae75-05cee02fe93b	";
        private static decimal BTC_EPSILON = .000000001M;
        private static decimal EPSILON = .001M;

        public BitPayTest()
        {
            decimal price = 100.0M;
            this.bitpay = new BitPay(API_KEY, "BTC");
            basicInvoice = this.bitpay.createInvoice(price);
        }

        [TestMethod]
        public void testShouldGetInvoiceId()
        {
            Assert.IsNotNull(basicInvoice.id, "Invoice created with id=NULL");
        }

        [TestMethod]
        public void testShouldGetInvoiceURL()
        {
            Assert.IsNotNull(basicInvoice.url, "Invoice created with url=NULL");
        }

        [TestMethod]
        public void testShouldGetInvoiceStatusL()
        {
            Assert.IsNotNull(basicInvoice.status, "Invoice created with status=NULL");
        }

        [TestMethod]
        public void testShouldGetInvoiceBTCPrice()
        {
            Assert.IsNotNull(basicInvoice.btcPrice, "Invoice created with btcPrice=NULL");
        }

        [TestMethod]
        public void testShouldCreateInvoiceOneTenthBTC()
        {
            try
            {
                // Arrange
                decimal price = 0.1M;
                decimal expected = 0.1M;

                // Act
                this.bitpay = new BitPay(API_KEY, "BTC");
                Invoice invoice = this.bitpay.createInvoice(price);

                // Assert
                decimal actual = invoice.btcPrice;
                Assert.AreEqual((double)expected, (double)actual, (double)BTC_EPSILON, "Invoice not created correctly: 0.1BTC");
            }
            catch (BitPayException ex)
            {
                Assert.Fail(ex.getMessage());
            }
        }

        [TestMethod]
        public void testShouldCreateInvoice100USD()
        {
            try
            {
                // Arrange
                decimal price = 100.0M;
                decimal expected = 100.0M;

                // Act
                this.bitpay = new BitPay(API_KEY, "USD");
                Invoice invoice = this.bitpay.createInvoice(price);

                // Assert
                var actual = invoice.price;
                Assert.AreEqual((double)expected, (double)actual, (double)EPSILON, "Invoice not created correctly: 100USD");
            }
            catch (BitPayException ex)
            {
                Assert.Fail(ex.getMessage());
            }
        }
        /*
        [TestMethod]
        public void testShouldCreateInvoice100EUR()
        {
            try
            {
                // Arrange
                decimal price = 100.0M;
                decimal expected = 100.0M;

                // Act
                this.bitpay = new BitPay(API_KEY, "EUR");
                Invoice invoice = this.bitpay.createInvoice(price);

                // Assert
                var actual = invoice.price;
                Assert.AreEqual((double)expected, (double)actual, (double)EPSILON, "Invoice not created correctly: 100EUR");
            }
            catch (BitPayException ex)
            {
                Assert.Fail(ex.getMessage());
            }
        }*/

        [TestMethod]
        public void testShouldGetInvoice()
        {
            try
            {
                Invoice retreivedInvoice = this.bitpay.getInvoice(basicInvoice.id);

                // Assert
                string expected = basicInvoice.id;
                string actual = retreivedInvoice.id;
                Assert.AreEqual(expected, actual, "Expected invoice not retreived");
            }
            catch (BitPayException ex)
            {
                Assert.Fail(ex.getMessage());
            }
        }

        [TestMethod]
        public void testShouldCreateInvoiceWithAdditionalParams()
        {
            try
            {
                // Arrange
                decimal price = 100.0M;
                InvoiceParams parameters = new InvoiceParams();
                parameters.buyerName = "Satoshi";
                parameters.buyerEmail = "satoshi@bitpay.com";
                parameters.fullNotifications = true;
                parameters.notificationEmail = "satoshi@bitpay.com";
                parameters.buyerAddress1 = "add1";
                parameters.buyerAddress2 = "2";
                parameters.buyerCity = "city";
                parameters.buyerCountry = "country";
                parameters.buyerEmail = "email";
                parameters.buyerName = "name";
                parameters.buyerPhone = "phone";
                parameters.buyerState = "state";
                parameters.buyerZip = "zip";
                parameters.itemCode = "itemcode";
                parameters.itemDesc = "itemdesc";
                parameters.orderId = "orderid";
                parameters.posData = "jkdsjlkfdsajklfdsjlkfsajdlkfdsjalkjfdslkjfdslk";
                parameters.redirectURL = "kjfdskjdfsal";
                parameters.notificationURL = "jklsafdjklfdsjl";
                parameters.payouts = new Dictionary<string, decimal>();
                parameters.payouts["address1"] = 1;
                parameters.payouts["address2"] = 2;
                // Act
                this.bitpay = new BitPay(API_KEY, "BTC");
                Invoice invoice = this.bitpay.createInvoice(price, parameters);

                // Assert
                Assert.IsNotNull(invoice, "Invoice not created");
            }
            catch (BitPayException ex)
            {
                Assert.Fail(ex.getMessage());
            }
        }
	
        [TestMethod]
        public void testShouldGetExchangeRates() 
        {
            try
            {
                // Arrange

                // Act
                this.bitpay = new BitPay(API_KEY, "EUR");		
                Rates rates = this.bitpay.getRates();		

                // Assert
                List<Rate> listRates = rates.getRates();
                Assert.IsNotNull(listRates, "Exchange rates not retrieved");
            }
            catch (BitPayException ex)
            {
                Assert.Fail(ex.getMessage());
            }
        }

        [TestMethod]
        public void testShouldGetUSDExchangeRate()
        {
            // Arrange

            // Act
            this.bitpay = new BitPay(API_KEY, "USD");		
            Rates rates = this.bitpay.getRates();

            // Assert
            decimal rate = rates.getRate("USD");
            Assert.IsTrue(rate != 0, "Exchange rate not retrieved: USD");
        }
	
        /*
        [TestMethod]
        public void testShouldGetEURExchangeRate()
        {
            // Arrange

            // Act
            this.bitpay = new BitPay(API_KEY, "EUR");		
            Rates rates = this.bitpay.getRates();

            // Assert
            decimal rate = rates.getRate("EUR");
            Assert.IsTrue(rate != 0, "Exchange rate not retrieved: EUR");
        }*/
	
        [TestMethod]
        public void testShouldGetCNYExchangeRate() 
        {
            // Arrange

            // Act
            this.bitpay = new BitPay(API_KEY, "CNY");
            Rates rates = this.bitpay.getRates();

            // Assert
            decimal rate = rates.getRate("CNY");
            Assert.IsTrue(rate != 0, "Exchange rate not retrieved: CNY");
        }
	
        [TestMethod]
        public void testShouldUpdateExchangeRates() 
        {
            // Arrange

            // Act
            this.bitpay = new BitPay(API_KEY, "EUR");		
            Rates rates = this.bitpay.getRates();		
            rates.update();
		
            // Assert
            List<Rate> listRates = rates.getRates();
            Assert.IsNotNull(listRates, "Exchange rates not retrieved after update");
        }
    }
}
