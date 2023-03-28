using BoDi;
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

        static DriverHierarchy()
        {
            var chromeOptions = new ChromeOptions();
            driver = new ChromeDriver(chromeOptions);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Navigate().GoToUrl("http://127.0.0.2:8081");
            driver.Manage().Window.Maximize();
            ClearScreenshots();
        }

        public static DriverHierarchy Instance
        {
            get
            {
                return instance = new DriverHierarchy();
            }
        }

        private static void ClearScreenshots()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
            if(Directory.GetFiles(filePath).Length > 0)
            {
                string[] filesToDelete = Directory.GetFiles(filePath);
                foreach (var file in filesToDelete)
                {
                    File.Delete(file);
                }
            }
        }

        public void TakeScreenshot(TestContext testContext)
        {
            var folderName = "Screenshots";
            var fileName = "screenshot_" + testContext.TestName + ".png";
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName, fileName);
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile(filePath, ScreenshotImageFormat.Png);
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