using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace FunctionalTestCases_Rinan
{
    internal class SeleniumHelper
    {
        private IWebDriver driver;

        public SeleniumHelper(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void Click(string xpath, int timeout = 10)
        {
            try
            {
                IWebElement element = WaitUntilVisible(By.XPath(xpath), timeout);
                ScrollIntoView(element);
                element.Click();
                Console.WriteLine($" Clicked: {xpath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Click failed for {xpath}\n{ex.Message}");
            }
        }

        public void SendKeys(string xpath, string data, int timeout = 10)
        {
            try
            {
                IWebElement element = WaitUntilVisible(By.XPath(xpath), timeout);
                element.Clear();
                element.SendKeys(data);
                Console.WriteLine($" Sent keys to: {xpath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" SendKeys failed for {xpath}\n{ex.Message}");
            }
        }

        public string GetElementText(string cssSelector, int timeout = 10)
        {
            try
            {
                By by = By.CssSelector(cssSelector);
                IWebElement element = WaitUntilVisible(by, timeout);
                return element.Text;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" GetElementText failed for {cssSelector}\n{ex.Message}");
                return string.Empty;
            }
        }

        public IWebElement FindCssSelector(string cssSelector, int timeout = 10)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(cssSelector)));
                return element;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" FindCssSelector failed for {cssSelector}\n{ex.Message}");
                return null;
            }
        }

        public IWebElement WaitUntilVisible(By by, int timeout)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until(ExpectedConditions.ElementIsVisible(by));
        }

        public void ScrollIntoView(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
            Thread.Sleep(300);
        }

        public string GetElementTextByXpath(string xpath, int timeout = 10)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                var element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpath)));
                return element.Text;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get text for element: {xpath}\n{ex.Message}");
                return string.Empty;
            }
        }
        public IWebElement ScrollAndWaitForElement(string xpath, int scrollBy = 12000, int timeout = 20)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript($"window.scrollBy(0, {scrollBy});");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpath)));

                return element;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ScrollAndWaitForElement failed for {xpath} {ex.Message}");
                return null;
            }
        }
        public void ScrollToElementByXPath(string xpath, int timeout = 10)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                IWebElement element = wait.Until(ExpectedConditions.ElementExists(By.XPath(xpath)));

                ScrollIntoView(element);
                Console.WriteLine($"Scrolled to element with XPath: {xpath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ScrollToElementByXPath failed for {xpath}\n{ex.Message}");
            }
        }

        public void JsClickWithRetry(string xpath)
        {
            try
            {
                JsClick(xpath);
            }
            catch (StaleElementReferenceException)
            {
                Console.WriteLine("Stale element, retrying...");
                JsClick(xpath);
            }
        }


        public void JsClick(string xpath)
        {
            try
            {
                var element = driver.FindElement(By.XPath(xpath));
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].click();", element);
                Console.WriteLine($"JsClick: Clicked {xpath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JsClick failed for {xpath}: {ex.Message}");
            }
        }

        public void Screenshot(string ScreenShotFileName, string ScreenShotsFolder = "./screenshots")
        {

            try
            {
                Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                if (!Directory.Exists(ScreenShotsFolder)) { Directory.CreateDirectory(ScreenShotsFolder); }
                ss.SaveAsFile(Path.Combine(ScreenShotsFolder, ScreenShotFileName));
                Console.WriteLine($"Screenshot taken and saved to ./screenshots/{ScreenShotFileName}");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error taking screenshot: " + e.Message);
            }

        }

        public void ScrollBy(int x, int y)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript($"window.scrollBy({x}, {y});");
                Thread.Sleep(300);  // Give some time for lazy content to load
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ScrollBy failed: {ex.Message}");
            }
        }


    }

}















