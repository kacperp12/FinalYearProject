using FYPTesting.Drivers;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace FYPTesting.Hooks
{
    [Binding]
    public sealed class Hook
    {
        private readonly ScenarioContext _scenarioContext;

        public Hook(ScenarioContext scenarioContext) => _scenarioContext = scenarioContext;

        [BeforeScenario]
        public void BeforeScenario()
        {
            SeleniumDriver seleniumDriver = new SeleniumDriver(_scenarioContext);
            _scenarioContext.Set(seleniumDriver, "SeleniumDriver");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _scenarioContext.Get<IWebDriver>("WebDriver").Quit();
        }
    }
}