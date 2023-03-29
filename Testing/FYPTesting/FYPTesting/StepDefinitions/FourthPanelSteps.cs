using OpenQA.Selenium;
using FluentAssertions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Interactions;
using System.Drawing;

namespace FYPTesting.StepDefinitions
{
    [Binding]
    public class FourthPanelSteps
    {
        private static DriverHierarchy driver = DriverHierarchy.Instance;
        private readonly ScenarioContext _scenarioContext;
        private readonly TestContext _testContext;

        public FourthPanelSteps(ScenarioContext scenarioContext, TestContext testContext)
        {
            _scenarioContext = scenarioContext;
            _testContext = testContext;
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
            driver.TakeScreenshot(_testContext);
        }

        [Then(@"""([^""]*)"" amount of edge selection buttons are displayed")]
        public void ThenAmountOfEdgeSelectionButtonsAreDisplayed(int numEdges)
        { 
            var numRadioButtons = driver.GetElementCount("//div[@data-type='rectangle'][2]/div/div/div[1]/label");
            numRadioButtons.Should().Be(numEdges);
            driver.TakeScreenshot(_testContext);
        }

        [When(@"I select ""([^""]*)"" for edge and ""([^""]*)"" for colour")]
        public void WhenISelectForEdgeAndForColour(string selectedEdge, string selectedCol)
        {
            var edge = driver.WaitForElementToClickable(By.XPath($"//span[contains(text(), '{selectedEdge}')]/../div"));
            edge.Click();

            var number = driver.WaitForElementToClickable(By.XPath($"//span[contains(text(), '{selectedCol}')]/../div"));
            number.Click();
        }

        [When(@"I press the edit button to confirm adjustments")]
        public void WhenIPressTheEditButtonToConfirmAdjustments()
        {
            var editButton = driver.WaitForElementToClickable(By.XPath("//div[contains(text(), 'Edit')]"));
            editButton.Click();
        }

        [Then(@"the colour of edge ""([^""]*)"" in shape ""([^""]*)"" is changed to ""([^""]*)""")]
        public void ThenTheColourOfEdgeIsChangedTo(string selectedEdge, string shapeName, string selectedColour)
        {
            var correspondingEdgeID = selectedEdge switch
            {
                "Edge 1" => "c1",
                "Edge 2" => "c2",
                "Edge 3" => "c3",
                "Edge 4" => "c4",
                _ => throw new ArgumentException("No such edge exists!")
            };

            var correspondingRGBValue = selectedColour switch
            {
                "Orange" => "rgb(255, 165, 0)",
                "Red" => "rgb(255, 0, 0)",
                "Green" => "rgb(0, 255, 0)",
                "Blue" => "rgb(0, 0, 255)",
                _ => throw new ArgumentException("No such colour exists!")
            };

            IWebElement edge = null;

            // This if statement is required as Selenium had issues picking up Edge 3 of the triangle shape.
            // I'm not sure why but this was the only edge out of the 2 shapes that Selenium could not find.
            // XPath used was definitely correct as I was able to locate the element through Google.
            // JS works fine finding the edge.
            if (correspondingEdgeID.Equals("c3"))
            {
                var svgElement = driver.WaitForElementToClickable(By.XPath("//*[local-name() = 'svg']"));

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver.getDriver();

                // Select the third child element of the SVG image
                string script = "var svg = arguments[0]; " +
                                "var thirdChild = svg.children[2]; " +
                                "return thirdChild;";

                edge = (IWebElement)js.ExecuteScript(script, svgElement);
            }
            else
            {
                edge = driver.WaitForElementToClickable(By.XPath($"//*[local-name() = 'svg']/*[@id='{correspondingEdgeID}']"));
            }

            if (shapeName.ToLower().Equals("rectangle"))
                edge.GetAttribute("fill").Should().Be(correspondingRGBValue);
            else if(shapeName.ToLower().Equals("triangle"))
                edge.GetAttribute("stroke").Should().Be(correspondingRGBValue);

            driver.TakeScreenshot(_testContext);
        }
    }
}