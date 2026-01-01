using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmokeTestCases
{
    public class seleniumHelper
    {
        private IWebDriver driver;

        public seleniumHelper(IWebDriver driver)
        {

            this.driver = driver;
        }

        public void Click(string xpath, int timeout = 10)
        {
            IWebElement element = WaitUntilVisible(By.XPath(xpath), timeout);
            ScrollIntoView(element); // still keeps scroll
            element.Click();
        }

        public void SendKeys(string xpath, string data, int timeout = 10)
        {
            IWebElement element = WaitUntilVisible(By.XPath(xpath), timeout);
            element.Clear();
            element.SendKeys(data);
        }

        public string GetElementText(string cssSelector, int timeout = 10)
        {
            IWebElement element = WaitUntilVisible(By.CssSelector(cssSelector), timeout);
            return element.Text;
        }

        public IWebElement FindCssSelector(string cssSelector, int timeout = 10)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(cssSelector)));
        }

        public IWebElement WaitUntilVisible(By by, int timeout)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until(ExpectedConditions.ElementIsVisible(by));

        }

        private void ScrollIntoView(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
            Thread.Sleep(300);
        }

        public void ScrollBy(int x, int y)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript($"window.scrollBy({x}, {y});");
            
        }

        public IWebElement SafeFindElement(By by, int timeout = 10)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(by);
                    bool displayed = element.Displayed;
                    return element;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });
        }

        public IWebElement WaitUntilNotStale(By by, int timeout = 10)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(by);
                    bool displayed = element.Displayed; // Force DOM check
                    return element;
                }
                catch (StaleElementReferenceException)
                {
                    return null; // Retry
                }
                catch (NoSuchElementException)
                {
                    return null; // Retry
                }
            });
        }

    }
}
