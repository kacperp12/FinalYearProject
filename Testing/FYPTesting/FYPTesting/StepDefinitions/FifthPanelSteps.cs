using OpenQA.Selenium;
using FluentAssertions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace FYPTesting.StepDefinitions
{
    [Binding]
    public class FifthPanelSteps
    {
        //IWebDriver driver;

        private static DriverHierarchy driver = DriverHierarchy.Instance;
        private readonly ScenarioContext _scenarioContext;
        private readonly TestContext _testContext;

        public FifthPanelSteps(ScenarioContext scenarioContext, TestContext testContext)
        {
            _scenarioContext = scenarioContext;
            _testContext = testContext;
        }

        [When(@"I select ""([^""]*)"" from the selection of input types")]
        public void WhenISelectFromTheSelectionOfInputTypes(string inputType)
        {
            var inputNumberInList = inputType.ToLower() switch
            {
                "eircode" => 1,
                "email" => 2,
                "number" => 3,
                _ => throw new ArgumentException("No such input exists!")
            };

            var inputXPath = driver.WaitForElementToClickable(By.XPath($"//span[@class='ScrollbarsCustom-Wrapper']//div//div//div//div[{inputNumberInList}]"));
                

            // Selection of input type persists across page refresh, therefore need to deselect value before moving on.
            if(inputXPath.GetAttribute("class").Contains("rowSelected"))
            {
                inputXPath.Click();
            }
            inputXPath.Click();
        }

        [Then(@"the label ""([^""]*)"" is displayed above the textbox")]
        public void ThenTheLabelIsDisplayedAboveTheTextbox(string inputType)
        {
            var inputLabel = driver.WaitForElementToClickable(By.XPath("(//div[@data-type='rectangle']//div//span)[2]"));
            inputLabel.Text.Should().Match(inputType, "because this input type was selected previously in this scenario.");
            driver.TakeScreenshot(_testContext);
        }

        [When(@"I input ""([^""]*)"" into the textbox")]
        public void WhenIInputIntoTheTextbox(string inputString)
        {
            var inputBox = driver.WaitForElementToClickable(By.XPath("//div[@data-type='rectangle']//input"));
            inputBox.SendKeys(inputString + Keys.Enter);
        }

        [When(@"I press the submit button")]
        public void WhenIPressTheSubmitButton()
        {
            var submitButton = driver.WaitForElementToClickable(By.XPath("//div[@data-type='rectangle']//div[3]//div"));
            submitButton.Click();
        }

        [When(@"I acknowledge the submission")]
        public void WhenIAcknowledgeTheSubmission()
        {
            var okButton = driver.WaitForElementToClickable(By.XPath("((//div[@data-type='panel'])[6]//div//div)[3]"));
            okButton.Click();
        }

        [Then(@"a new entry of type ""([^""]*)"" is placed at the top of the gridbox")]
        public void ThenANewEntryOfTypeIsPlacedAtTheTopOfTheGridbox(string inputType)
        {
            // Hard-coded search for first entry in gridbox to force search for first entry.
            var firstRowIndexEntry = driver.WaitForElementToClickable(By.XPath("//div[@aria-rowindex='1']//div[2]//span"));

            firstRowIndexEntry.Text.Should().Match(inputType, "because the new entry is expected to be of this type.");
            driver.TakeScreenshot(_testContext);
        }

        [Then(@"I am prompted that my submission is successful")]
        public void ThenIAmPromptedThatMySubmissionIsSuccessful()
        {
            var confirmationMsg = driver.WaitForElementToClickable(By.XPath("(//div[@data-type='panel'])[6]//span"));
            confirmationMsg.Text.Should().Match("Input has been successfully submitted", "because input is successful.");
            driver.TakeScreenshot(_testContext);
        }

        [Then(@"the submit button is disabled")]
        public void ThenTheSubmitButtonIsDisabled()
        {
            var submitButton = driver.WaitForElementToClickable(By.XPath("//div[@data-type='rectangle']//div[3]"));
            submitButton.GetCssValue("opacity").Should().Be("0.5", "Object should be partially greyed out when disabled");
            driver.TakeScreenshot(_testContext);
        }

        [Then(@"an error message is displayed")]
        public void ThenAnErrorMessageIsDisplayed()
        {
            var errorMsg = driver.WaitForElementToClickable(By.XPath("(//div[@data-type='rectangle']//div//span)[3]"));
            errorMsg.Text.Should().Contain("Please check your input.");
            driver.TakeScreenshot(_testContext);
        }
    }
}