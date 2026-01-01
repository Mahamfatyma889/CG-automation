using NUnit.Framework;
using OpenQA.Selenium;

namespace SmokeTestCases
{
    [TestFixture]
    public class SmokeTests
    {
        private IWebDriver driver;
        private seleniumHelper helper;

        [SetUp]
        public void SetUp()
        {
            driver = Driver.GetDriver(); 
            driver.Manage().Window.Maximize();
            helper = new seleniumHelper(driver);
            driver.Navigate().GoToUrl("https://rinan.klabs.co/");

        }

        [Test]
        public void Test_Login()
        {
            var loginIcon = helper.FindCssSelector(".c-header__buttons .ip-user");
            loginIcon.Click();
            helper.FindCssSelector(".c-header__buttons .ip-user").Click();
            helper.SendKeys("//input[@name='username']", "shehzabahmedklabs@gmail.com");
            helper.SendKeys("//input[@name='password']", "&XD^fJnlt1Cy4Or*@EOR%yDp");
            helper.Click("//button[@type='submit']");

            Assert.That(driver.Url, Does.Contain("my-account"), "Login failed");
        }

        [Test]
        public void Test_AddToCart()
        {
            driver.Navigate().GoToUrl("https://rinan.klabs.co/product/butterfly-earrings/");
            helper.Click("//button[contains(text(),'Add to cart')]");

            string message = helper.GetElementText(".woocommerce-notices-atc-wrap");
            Assert.That(message, Does.Contain("has been added"), "Product not added to cart");
        }

        [Test]
        public void Test_Verify_Item_AddedToCart()
        {
            driver.Navigate().GoToUrl("https://rinan.klabs.co/product/butterfly-earrings/");
            helper.Click("//button[contains(text(),'Add to cart')]");
            helper.GetElementText(".woocommerce-notices-atc-wrap");
            driver.Navigate().GoToUrl("https://rinan.klabs.co/cart/");
            string cartProduct = helper.GetElementText("td.c-cart__shop-td--product-name a");

            Assert.That(cartProduct.ToLower(), Does.Contain("butterfly earrings"), "Product missing in cart");

        }

        [Test]
        public void Test_ProceedToCheckoutPage()
        {
            driver.Navigate().GoToUrl("https://rinan.klabs.co/product/butterfly-earrings/");
            helper.Click("//button[contains(text(),'Add to cart')]");
            helper.GetElementText(".woocommerce-notices-atc-wrap");
            driver.Navigate().GoToUrl("https://rinan.klabs.co/cart/");
            helper.Click("//a[contains(@class,'checkout-button') and contains(text(),'Checkout')]");
            Assert.That(driver.Url.ToLower(), Does.Contain("/checkout"), "Failed to navigate to Checkout page");
            string header = helper.GetElementText(".woocommerce-billing-fields h3");
            Assert.That(header.ToLower(), Does.Contain("billing details"), "Billing details section not found");

        }

        [Test]
        public void Test_Logout()
        {
            var loginIcon = helper.FindCssSelector(".c-header__buttons .ip-user");
            loginIcon.Click();
            helper.FindCssSelector(".c-header__buttons .ip-user").Click();
            helper.SendKeys("//input[@name='username']", "shehzabahmedklabs@gmail.com");
            helper.SendKeys("//input[@name='password']", "&XD^fJnlt1Cy4Or*@EOR%yDp");
            helper.Click("//button[@type='submit']");
            driver.Navigate().GoToUrl("https://rinan.klabs.co/my-account/");
            helper.Click("//a[contains(@class, 'c-page-header__logout')]");

            Assert.That(driver.Url, Does.Contain("customer-logout").Or.Contain("my-account"), "Logout failed");
        }

        [Test]
        public void Test_ConsultationNavigation_VisiblityOfCalendar()
        {
            helper.Click("//ul[@id='top-menu-desktop']//a[contains(text(),'Consultation')]");
            Assert.That(driver.Url.ToLower(), Does.Contain("consultation"), "Consultation page failed");
            helper.ScrollBy(0, 800);
            IWebElement iframe = helper.WaitUntilVisible(By.XPath("//iframe[contains(@src, 'calendly.com')]"), 10);
            driver.SwitchTo().Frame(iframe);
            IWebElement heading = helper.WaitUntilNotStale(By.XPath("//h1[contains(text(),'30 Minute Meeting')]"), 10);
            Assert.That(heading.Displayed, "Calendly calendar not visible inside iframe.");
            driver.SwitchTo().DefaultContent();

        }


            [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }
   
    }
}
