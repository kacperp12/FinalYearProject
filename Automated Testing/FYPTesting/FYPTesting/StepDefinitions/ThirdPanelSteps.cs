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
        private readonly TestContext _testContext;

        public ThirdPanelSteps(ScenarioContext scenarioContext, TestContext testContext)
        {
            _scenarioContext = scenarioContext;
            _testContext = testContext;
        }

        [When(@"I input ""([^""]*)"" into the input field")]
        public void InsertInputIntoField(string Input)
        {
            var InputField = driver.WaitForElementToClickable(By.XPath("(//div[@data-type='rectangle']//div)[2]//input"));
            InputField.SendKeys(Input + Keys.Enter);
        }

        [Then(@"the input ""([^""]*)"" is displayed under the generated barcode")]
        public void InputDisplayedUnderBarcode(string input)
        {
            var LabelSpan = driver.WaitForElementToClickable(By.XPath("(//div[@data-type='rectangle']//div)[3]//span"));
            LabelSpan.Text.Should().Match(input);
            driver.TakeScreenshot(_testContext);
        }


        [Then(@"the input is considered invalid")]
        public void InvalidInput()
        {
            var errorSpan = driver.WaitForElementToClickable(By.XPath("(//div[@data-type='rectangle']//div)[5]//span"));
            errorSpan.Text.Should().Match("Please check your input! (15 chars max)", "because max input is 15 chars.");
            driver.TakeScreenshot(_testContext);
        }

        [Then(@"a barcode for this input is generated")]
        public void BarcodeGenerated()
        {
            var barcodeImg = driver.WaitForElementToClickable(By.XPath("//div//img"));
            barcodeImg.Should().NotBeNull();
            driver.TakeScreenshot(_testContext);
        }
    }
}