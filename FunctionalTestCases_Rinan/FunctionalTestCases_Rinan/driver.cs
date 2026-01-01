using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace FunctionalTestCases_Rinan
{
    internal class Driver
    {
        public static IWebDriver GetDriver()
        {
            var driver = new ChromeDriver();

            return driver;
        }

    }
}
