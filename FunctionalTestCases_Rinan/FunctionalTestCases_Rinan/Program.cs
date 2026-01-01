using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Globalization;

namespace FunctionalTestCases_Rinan
{
    [TestFixture]
    public class LoginTest
    {
        private IWebDriver driver;
        private SeleniumHelper helper;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://rinan.klabs.co/");
            helper = new SeleniumHelper(driver);
        }

        //Login Function
        public void Login(string username, string password)
        {
            var loginIcon = helper.FindCssSelector(".c-header__buttons .ip-user");
            loginIcon?.Click();
            helper.SendKeys("//input[@name='username']", username);
            helper.SendKeys("//input[@name='password']", password);
            helper.Click("//button[@type='submit']");
        }

        // Add To Cart Function
        public void AddToCart(string productName)
        {
            helper.Click("//ul[@id='top-menu-desktop']//a[text()='Shop']");
            string productXpath = $"//h2[@class='woocommerce-loop-product__title' and text()='{productName}']";
            helper.ScrollToElementByXPath(productXpath);
            helper.Click(productXpath);
            string addToCartButton = "//button[contains(@class,'single_add_to_cart_button')]";
            helper.ScrollToElementByXPath(addToCartButton);
            helper.Click(addToCartButton);
        }

        [Test]
        public void Login_With_Valid_Credentials()
        {
            Login("shehzabahmedklabs@gmail.com", "&XD^fJnlt1Cy4Or*@EOR%yDp");
            string currentUrl = driver.Url;
            Assert.That(currentUrl, Does.Contain("account").IgnoreCase, "Login failed: URL does not contain 'account'");

        }

        [Test]
        public void Login_With_Invalid_Credentials()
        {
            var loginIcon = helper.FindCssSelector(".c-header__buttons .ip-user");
            loginIcon?.Click();
            helper.SendKeys("//input[@name='username']", "shehzabahmedklabs@gmail.com");
            helper.SendKeys("//input[@name='password']", "12345");
            helper.Click("//button[@type='submit']");
            string errorXpath = "//div[contains(@class,'woocommerce-notice') and @role='alert']";
            string actualError = helper.GetElementTextByXpath(errorXpath);
            Assert.That(actualError, Does.Contain("username or password you entered is incorrect").IgnoreCase, "Expected error message not displayed");
            Console.WriteLine("Error message correctly shown for invalid login");
        }

        [Test]
        public void Add_To_Cart()
        {
            AddToCart("Butterfly earrings");
            string actualMessage = helper.GetElementText(".woocommerce-notices-atc-wrap");
            Assert.That(actualMessage, Does.Contain("has been added to your cart"),
            "ERROR: Confirmation message does not indicate that a product was added to the cart.");
        }

        [Test]
        public void Remove_From_Cart()
        {
            AddToCart("Butterfly earrings");
            string actualMessage = helper.GetElementText(".woocommerce-notices-atc-wrap");
            Assert.That(actualMessage, Does.Contain("has been added to your cart"),
            "ERROR: Confirmation message does not indicate that a product was added to the cart.");
            driver.Navigate().GoToUrl("https://rinan.klabs.co/cart/");
            helper.ScrollToElementByXPath("//a[i[@class='ip-close-small c-cart__shop-remove-icon']]");
            helper.JsClick("//a[i[@class='ip-close-small c-cart__shop-remove-icon']]");
            Assert.That(driver.Url, Does.Contain("cart/?removed"),
                "ERROR: The URL does not indicate an item was removed from the cart.");
            string cartMessage = helper.GetElementText("div.woocommerce");
            Assert.That(cartMessage.ToLower(), Does.Contain("your cart is currently empty"),
            "ERROR: Cart is not empty after removing item.");
        }

        [Test]
        public void Opening_Tap_Modal_For_Checkout()
        {
            Login("shehzabahmedklabs@gmail.com", "&XD^fJnlt1Cy4Or*@EOR%yDp");
            AddToCart("Butterfly earrings");
            driver.Navigate().GoToUrl("https://rinan.klabs.co/cart/");
            helper.ScrollToElementByXPath("//a[contains(@class, 'checkout-button')]");
            helper.Click("//a[contains(@class, 'checkout-button')]");
            helper.ScrollToElementByXPath("//input[@type='checkbox']");
            helper.JsClick("//input[@type='checkbox' and @name='terms']");
            var button = helper.ScrollAndWaitForElement("//form//button[@type='submit']");
            Console.WriteLine($"Visible: {button.Displayed}, Enabled: {button.Enabled}");
            helper.ScrollToElementByXPath("//form//button[@type='submit']");
            helper.JsClickWithRetry("//form//button[@type='submit']");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.UrlContains("checkout.tap.company"));
            Assert.That(driver.Url, Does.Contain("checkout.tap.company"), "Tap modal did not open");
            helper.Screenshot("TapModalOpened.png");
            Console.WriteLine("Checkout test successful till Tap modal opened.");

        }

        [Test]
        public void Edit_Personal_Detail()
        {

                string newFirstName = "shezab123";     // provide custom values here
                string newLastName = "ahmed123";

                Login("shehzabahmedklabs@gmail.com", "&XD^fJnlt1Cy4Or*@EOR%yDp");
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                By EditLinkLocator = By.XPath("//p/a[3]");
                IWebElement EditLink = wait.Until(ExpectedConditions.ElementIsVisible(EditLinkLocator));
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", EditLink);
                wait.Until(ExpectedConditions.ElementToBeClickable(EditLinkLocator)).Click();
                helper.ScrollBy(0, 300);
                IWebElement firstNameInput = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='account_first_name']")));
                firstNameInput.Clear();
                firstNameInput.SendKeys(newFirstName);
                Console.WriteLine($"First name updated to: {newFirstName}");
                IWebElement lastNameInput = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='account_last_name']")));
                lastNameInput.Clear();
                lastNameInput.SendKeys(newLastName);
                Console.WriteLine($"Last name updated to: {newLastName}");
                helper.ScrollBy(0, 750);
                IWebElement saveChangesBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
                  By.XPath("//button[@name='save_account_details' and text()='Save changes']")));
                saveChangesBtn.Click();
                IWebElement successMessage = wait.Until(ExpectedConditions.ElementIsVisible(
                  By.CssSelector("div.woocommerce-notice")));
                Assert.That(successMessage.Text.Contains("Account details changed successfully."));
                Console.WriteLine("Account details updated successfully!");

            }


        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
     
    }
}
