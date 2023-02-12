using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYPTesting
{
    public sealed class DriverHierarchy
    {
        private static IWebDriver driver;
        private static WebDriverWait wait;
        private static volatile DriverHierarchy instance = null;

        //private DriverHierarchy()
        //{
        //    var chromeOptions = new ChromeOptions();
        //    driver = new ChromeDriver(chromeOptions);
        //    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        //    driver.Navigate().GoToUrl("http://127.0.0.2:8081");
        //    driver.Manage().Window.Maximize();
        //}

        static DriverHierarchy()
        {
            var chromeOptions = new ChromeOptions();
            driver = new ChromeDriver(chromeOptions);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Navigate().GoToUrl("http://127.0.0.2:8081");
            driver.Manage().Window.Maximize();
        }

        public static DriverHierarchy Instance
        {
            get
            {
                return instance = new DriverHierarchy();
            }
        }

        public void RefreshPage()
        {
            driver.Navigate().Refresh();
        }

        public IWebElement WaitForElementToClickable(By elementLocator)
        {
            try
            {
                return wait.Until(ExpectedConditions.ElementToBeClickable(elementLocator));
            }
            catch (NoSuchElementException)
            {
                //Console.WriteLine("Element with locator: '" + elementLocator + "' was not found in current context page.");
                //Test.context - Research this.
                throw;
            }
        }

        public IWebElement WaitForElementToVisible(By elementLocator)
        {
            try
            {
                return wait.Until(ExpectedConditions.ElementIsVisible(elementLocator));
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Element with locator: '" + elementLocator + "' was not found in current context page.");
                throw;
            }
        }

        public int GetElementCount(String XPath)
        {
            var el = WaitForElementToClickable(By.XPath(XPath));
            return driver.FindElements(By.XPath(XPath)).Count;
        }

        public Actions CreateAction()
        {
            Actions builder = new Actions(driver);
            return builder;
        }
    }

















    //private static IWebDriver driver;
    //private readonly ScenarioContext _scenarioContext;

    ////public SeleniumDriver(ScenarioContext scenarioContext) => _scenarioContext = scenarioContext;

    //[BeforeTestRun]
    //public static void BeforeTestRun()
    //{
    //    _driver = new SeleniumDriver();
    //    _driver.Setup();
    //}

    //[AfterTestRun]
    //public static void AfterTestRun()
    //{
    //    _driver.Quit();
    //}

    //[BeforeTestRun]
    //public static IWebDriver Setup()
    //{
    //    try
    //    {
    //        var chromeOptions = new ChromeOptions();

    //        driver = new ChromeDriver(chromeOptions);
    //        driver.Navigate().GoToUrl("http://127.0.0.2:8081");
    //        driver.Manage().Window.Maximize();

    //        //_scenarioContext.Set(driver, "WebDriver");

    //        return driver;
    //    }
    //    catch (Exception e)
    //    {
    //        throw new Exception(e.Message);
    //    }
    //}

    //public void Quit()
    //{
    //    try
    //    {
    //        driver.Quit();
    //    }
    //    catch (Exception e)
    //    {
    //        throw new Exception(e.Message);
    //    }
    //}
}

