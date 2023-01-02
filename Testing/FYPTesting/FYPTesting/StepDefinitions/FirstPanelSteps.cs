using FYPTesting.Drivers;
using OpenQA.Selenium;
using FluentAssertions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace FYPTesting.StepDefinitions
{
    [Binding]
    public class FirstPanelSteps
    {
        IWebDriver driver;
        private readonly ScenarioContext _scenarioContext;

        public FirstPanelSteps(ScenarioContext scenarioContext)
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

        //[Given(@"the app is running")]
        //public void GivenTheAppIsRunning()
        //{
        //    driver = _scenarioContext.Get<SeleniumDriver>("SeleniumDriver").Setup();
        //}

        [Given(@"the tab ""([^""]*)"" is open")]
        public void GivenTheTabIsOpen(string PanelName)
        {
            var PanelNumber = PanelName.ToLower() switch
            {
                "input validation" => 1,
                "button patterns" => 2,
                _ => throw new ArgumentException("No such button name exists!")
            };

            var TabButton = WaitForElementToClickable(By.XPath($"//div//button[{PanelNumber}]"));
            TabButton.Click();
        }

        [When(@"I input ""([^""]*)"" for Eircode")]
        public void WhenIInputForEircode(string Input)
        {
            var TextBox = WaitForElementToClickable(By.XPath("//div[@data-type='panel'][1]//input"));

            //if blank input is being tested, send a character and delete it to display the error message.
            if(Input.ToLower().Equals(""))
            {
                TextBox.SendKeys(Keys.Space);
                TextBox.SendKeys(Keys.Backspace);
            }
            else
            {
                TextBox.SendKeys(Input);
            }

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

        [Then(@"""([^""]*)"" exists")]
        public void ThenExists(string ObjectToFind)
        {
            if(ObjectToFind.ToLower().Equals("eircode input"))
            {
                var InputButton = WaitForElementToClickable(By.XPath("//div[@data-type='panel'][1]//input"));
                InputButton.Should().NotBeNull();
            } 
            else if(ObjectToFind.ToLower().Equals("submit button"))
            {
                var SubmitButton = WaitForElementToClickable(By.XPath("//div[@data-type='panel'][2]//div//div"));
                SubmitButton.Should().NotBeNull();
            }
        }

        [Then(@"a success message is displayed to the user")]
        public void ThenASuccessMessageIsDisplayedToTheUser()
        {
            var Message = "Eircode successfully submitted";

            var SuccessulSubmissionMsg = WaitForElementToClickable(By.XPath("//div[@data-type='panel'][2]//span")).Text;

            SuccessulSubmissionMsg.Should().Match(Message);
        }
    }
}