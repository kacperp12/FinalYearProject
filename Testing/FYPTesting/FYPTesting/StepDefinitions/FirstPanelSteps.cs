using OpenQA.Selenium;
using FluentAssertions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace FYPTesting.StepDefinitions
{
    [Binding]
    public class FirstPanelSteps
    {
        //IWebDriver driver;

        private static DriverHierarchy driver = DriverHierarchy.Instance;
        private readonly ScenarioContext _scenarioContext;

        public FirstPanelSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }


        [BeforeTestRun]
        public static void SetUp()
        {
            //driver.RefreshPage();
        }

        [Given(@"the content is refreshed")]
        public void GivenTheContentIsRefreshed()
        {
            driver.RefreshPage();
        }

        [Given(@"the tab ""([^""]*)"" is open")]
        public void IsTabOpen(string PanelName)
        {
            var PanelNumber = PanelName.ToLower() switch
            {
                "input validation" => 1,
                "button patterns" => 2,
                _ => throw new ArgumentException("No such button name exists!")
            };

            var TabButton = driver.WaitForElementToClickable(By.XPath($"//div//button[{PanelNumber}]"));
            TabButton.Click();
        }

        [When(@"I input ""([^""]*)"" for Eircode")]
        public void InputEircode(string Input)
        {
            var TextBox = driver.WaitForElementToClickable(By.XPath("//div[@data-type='panel'][1]//input"));

            //if blank input is being tested, send a character and delete it to display the error message.
            if (Input.ToLower().Equals(""))
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
        public void PressSubmit()
        {
            var SubmitButton = driver.WaitForElementToClickable(By.XPath("//div[@data-type='button']//div"));
            SubmitButton.Click();
        }

        [Then(@"the input is ""([^""]*)""")]
        public void CheckInput(string Validity)
        {
            if (Validity.ToLower().Equals("valid"))
            {
                var SuccessMessage = driver.WaitForElementToClickable(By.XPath("//div[@data-type='label'][3]//span"));
                SuccessMessage.Text.Should().Be("Eircode successfully submitted", "because no error message is displayed");
            }
            else if (Validity.ToLower().Equals("invalid"))
            {
                var ErrorMessage = driver.WaitForElementToClickable(By.XPath("//div[@data-type='label'][2]//span"));
                ErrorMessage.Text.Should().Be("Please check your input!", "because an incorrect input was entered");
            }
        }

        [Then(@"""([^""]*)"" exists")]
        public void DoesObjExist(string Obj)
        {
            var ObjXPath = Obj switch
            {
                "Eircode Input" => "//div[@data-type='panel'][1]//input",
                "Submit Button" => "//div[@data-type='button']//div",
                "Slider" => "(//div[@role='button'])[1]",
                "WarningMinInput" => "(//div[@data-type='panel'][2]//input)[1]",
                "WarningMaxInput" => "(//div[@data-type='panel'][2]//input)[2]",
                "WarningUpdateButton" => "//div[@data-type='panel'][2]//div[6]",
                "DangerMinInput" => "(//div[@data-type='panel'][3]//input)[1]",
                "DangerMaxInput" => "(//div[@data-type='panel'][3]//input)[2]",
                "DangerUpdateButton" => "//div[@data-type='panel'][3]//div[6]",
                "GreenLED" => "//div[@data-type='led'][1]",
                "OrangeLED" => "//div[@data-type='led'][2]",
                "RedLED" => "//div[@data-type='led'][3]",
                _ => throw new Exception("No such element exists!")
            };

            var ObjToLocate = driver.WaitForElementToClickable(By.XPath(ObjXPath));
            ObjToLocate.Should().NotBeNull();
        }

        [Then(@"a success message is displayed to the user")]
        public void SuccessMessageDisplayed()
        {
            var Message = "Eircode successfully submitted";

            var SuccessulSubmissionMsg = driver.WaitForElementToClickable(By.XPath("//div[@data-type='label'][3]//span")).Text;

            SuccessulSubmissionMsg.Should().Match(Message);
        }
    }
}