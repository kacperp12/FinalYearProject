using OpenQA.Selenium;
using FluentAssertions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Interactions;

namespace FYPTesting.StepDefinitions
{
    [Binding]
    public class ThirdPanelSteps
    {
        private static DriverHierarchy driver = DriverHierarchy.Instance;
        private readonly ScenarioContext _scenarioContext;

        public ThirdPanelSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [When(@"I input ""([^""]*)"" into the input field")]
        public void WhenIInputIntoTheInputField(string Input)
        {
            var InputField = driver.WaitForElementToClickable(By.XPath("(//div[@data-type='rectangle']//div)[2]//input"));
            InputField.SendKeys(Input + Keys.Enter);
        }

        [When(@"I Press the submit button")]
        public void WhenIPressTheSubmitButton()
        {
            var Button = driver.WaitForElementToClickable(By.XPath("(//div[@data-type='rectangle']//div)[6]//div"));
            Button.Click();
        }

        [Then(@"the input ""([^""]*)"" is displayed under the generated barcode")]
        public void ThenTheInputIsDisplayedUnderTheGeneratedBarcode(string input)
        {
            var LabelSpan = driver.WaitForElementToClickable(By.XPath("(//div[@data-type='rectangle']//div)[3]//span"));
            LabelSpan.Text.Should().Match(input);
        }


        [Then(@"the input is considered invalid")]
        public void ThenTheInputIsConsideredInvalid()
        {
            var errorSpan = driver.WaitForElementToClickable(By.XPath("(//div[@data-type='rectangle']//div)[5]//span"));
            errorSpan.Text.Should().Match("Please check your input! (15 chars max)", "because max input is 15 chars.");
        }

        [Then(@"a barcode for this input is generated")]
        public void ThenABarcodeForThisInputIsGenerated()
        {
            var barcodeImg = driver.WaitForElementToClickable(By.XPath("//div//img"));
            barcodeImg.Should().NotBeNull();
        }
    }
}