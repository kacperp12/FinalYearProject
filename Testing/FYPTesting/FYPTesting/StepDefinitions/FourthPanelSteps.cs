using OpenQA.Selenium;
using FluentAssertions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Interactions;

namespace FYPTesting.StepDefinitions
{
    [Binding]
    public class FourthPanelSteps
    {
        private static DriverHierarchy driver = DriverHierarchy.Instance;
        private readonly ScenarioContext _scenarioContext;

        public FourthPanelSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [When(@"I select the ""([^""]*)"" shape")]
        public void WhenISelectTheShape(string shapeName)
        {
            var shapeCheckboxPath = shapeName switch
            {
                "Triangle" => "//div[@data-type='rectangle']//div//div[2]//div",
                "Rectangle" => "//div[@data-type='rectangle']//div//div[3]//div",
                _ => throw new Exception("No such shape exists!")
            };

            var checkbox = driver.WaitForElementToClickable(By.XPath(shapeCheckboxPath));
            checkbox.Click();
        }

        [Then(@"the ""([^""]*)"" shape is displayed")]
        public void ThenTheShapeIsDisplayed(string shapeName)
        {
            var shape = driver.WaitForElementToClickable(By.XPath("//div[@data-type='advancedSVGImage']//div//div/*[name()='svg']"));

            shape.GetAttribute("data-src").Should().Contain(shapeName);
        }

        [Then(@"""([^""]*)"" amount of edge selection buttons are displayed")]
        public void ThenAmountOfEdgeSelectionButtonsAreDisplayed(int numEdges)
        { 
            var numRadioButtons = driver.GetElementCount("//div[3]//div//label[@data-type='radioButton']");
            numRadioButtons.Should().Be(numEdges + 4); //plus 4 to include the 4 colours
        }

        [When(@"I select ""([^""]*)"" for edge and ""([^""]*)"" for colour")]
        public void WhenISelectForEdgeAndForColour(string p0, string black)
        {
            throw new PendingStepException();
        }

        [When(@"I press the edit button to confirm adjustments")]
        public void WhenIPressTheEditButtonToConfirmAdjustments()
        {
            throw new PendingStepException();
        }

        [Then(@"the colour of edge ""([^""]*)"" is changed to ""([^""]*)""")]
        public void ThenTheColourOfEdgeIsChangedTo(string p0, string black)
        {
            throw new PendingStepException();
        }

    }
}