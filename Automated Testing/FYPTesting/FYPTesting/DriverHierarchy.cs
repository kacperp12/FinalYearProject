using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace FYPTesting
{
    public sealed class DriverHierarchy
    {
        // Declare the driver, wait, and instance variables
        private static IWebDriver driver;
        private static WebDriverWait wait;
        private static volatile DriverHierarchy instance = null;

        // Static constructor to initialize the driver, wait, and other settings
        static DriverHierarchy()
        {
            var chromeOptions = new ChromeOptions();
            driver = new ChromeDriver(chromeOptions);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Navigate().GoToUrl("http://127.0.0.2:8081");
            driver.Manage().Window.Maximize();
            ClearScreenshots();
        }

        // Create a public static property to access the instance of the DriverHierarchy
        public static DriverHierarchy Instance
        {
            get
            {
                return instance = new DriverHierarchy();
            }
        }

        // Public method to get the driver
        public IWebDriver getDriver()
        {
            return driver;
        }

        // Private method to clear the screenshots folder from previous test runs
        private static void ClearScreenshots()
        {
            // Get the file path of the screenshots folder
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");

            // If there are any files in the folder
            if (Directory.GetFiles(filePath).Length > 0)
            {
                // Get an array of all the files in the folder
                string[] filesToDelete = Directory.GetFiles(filePath);

                // Loop through the array and delete each file
                foreach (var file in filesToDelete)
                {
                    File.Delete(file);
                }
            }
        }

        // Public method to take a screenshot
        public void TakeScreenshot(TestContext testContext)
        {
            // Get the folder name and file name for the screenshot
            var folderName = "Screenshots";
            var fileName = "screenshot_" + testContext.TestName + ".png";
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName, fileName);

            // Take a screenshot and save it to the specified file path
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile(filePath, ScreenshotImageFormat.Png);
        }

        public void RefreshPage()
        {
            driver.Navigate().Refresh();
        }

        // Public method to wait for an element to become clickable
        public IWebElement WaitForElementToClickable(By elementLocator)
        {
            try
            {
                return wait.Until(ExpectedConditions.ElementToBeClickable(elementLocator));
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Element with locator: '" + elementLocator + "' was not found in current context page.");
                throw;
            }
        }

        // Public method to wait for an element to become visible
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

        // Public method to get the element count at the specified XPath value
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
}