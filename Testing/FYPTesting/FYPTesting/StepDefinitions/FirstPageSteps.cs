using FYPTesting.Drivers;
using OpenQA.Selenium;
using FluentAssertions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace FYPTesting.StepDefinitions
{
    [Binding]
    public class FirstPageSteps
    {
        IWebDriver driver;
        private readonly ScenarioContext _scenarioContext;

        public FirstPageSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        public IWebElement WaitForElementToVisible(By elementLocator)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                return wait.Until(ExpectedConditions.ElementIsVisible(elementLocator));
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Element with locator: '" + elementLocator + "' was not found in current context page.");
                throw;
            }
        }

        public IWebElement WaitForElementToClickable(By elementLocator)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                return wait.Until(ExpectedConditions.ElementToBeClickable(elementLocator));
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Element with locator: '" + elementLocator + "' was not found in current context page.");
                throw;
            }
        }

        [Given(@"the app is running")]
        public void GivenTheAppIsRunning()
        {
            driver = _scenarioContext.Get<SeleniumDriver>("SeleniumDriver").Setup();
        }

        [When(@"I input ""([^""]*)"" for Eircode")]
        public void WhenIInputForEircode(string Input)
        {
            var TextBox = WaitForElementToClickable(By.XPath("//div[@data-type='panel'][1]//input"));
            TextBox.SendKeys(Input);
            TextBox.SendKeys(Keys.Enter);
        }

        [When(@"I Press submit")]
        public void WhenIPressSubmit()
        {
            var SubmitButton = WaitForElementToClickable(By.XPath("//div[@data-type='button']//div"));
            SubmitButton.Click();
        }

        [Then(@"the input is ""([^""]*)""")]
        public void ThenTheInputIs(string Validity)
        {
            if (Validity.ToLower().Equals("valid"))
            {
                var SuccessMessage = WaitForElementToClickable(By.XPath("(//div[@data-type='panel'][2]//div)[4]//span"));
                SuccessMessage.Text.Should().Be("Eircode successfully submitted", "because no error message is displayed");
            }
            else if(Validity.ToLower().Equals("invalid"))
            {
                var ErrorMessage = WaitForElementToClickable(By.XPath("//div[@data-type='panel'][1]//div[3]//span"));
                ErrorMessage.Text.Should().Be("Please check your input!", "because an incorrect input was entered");
            }
        }
    }
}