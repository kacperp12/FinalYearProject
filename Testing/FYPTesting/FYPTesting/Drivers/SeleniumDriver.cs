using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYPTesting.Drivers
{
    internal class SeleniumDriver
    {
        private IWebDriver driver;
        private readonly ScenarioContext _scenarioContext;

        public SeleniumDriver(ScenarioContext scenarioContext) => _scenarioContext = scenarioContext;

        public IWebDriver Setup()
        {
            try
            {
                var chromeOptions = new ChromeOptions();

                driver = new ChromeDriver(chromeOptions);
                driver.Navigate().GoToUrl("http://127.0.0.2:8081");
                driver.Manage().Window.Maximize();

                _scenarioContext.Set(driver, "WebDriver");

                return driver;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
