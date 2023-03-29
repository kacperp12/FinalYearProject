using OpenQA.Selenium;
using FluentAssertions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Interactions;

namespace FYPTesting.StepDefinitions
{
    [Binding]
    public class SecondPanelSteps
    {
        private static DriverHierarchy driver = DriverHierarchy.Instance;
        private readonly ScenarioContext _scenarioContext;
        private readonly TestContext _testContext;

        public SecondPanelSteps(ScenarioContext scenarioContext, TestContext testContext)
        {
            _scenarioContext = scenarioContext;
            _testContext = testContext;
        }

        [When(@"I drag the slider to value ""([^""]*)""")]
        public void DragSliderValue(string SliderValue)
        {
            Actions builder = driver.CreateAction();
            var Source = driver.WaitForElementToClickable(By.XPath("//div[@data-type='circularGauge']//div[@class='sliderHandle-0-3-7']"));
            var Destination = driver.WaitForElementToClickable(By.XPath($"//div[@data-type='circularGauge']//span[text()='{SliderValue}']"));

            var DragAndDrop = builder.ClickAndHold(Source)
                .MoveToElement(Destination)
                .Release(Destination)
                .Build();

            DragAndDrop.Perform();
        }

        [Then(@"the slider value is set to ""([^""]*)""")]
        public void SliderSettoValue(string SliderValue)
        {
            var TooltipValue = driver.WaitForElementToClickable(By.XPath("//div[@data-type='circularGauge']/div/div/div/div/div/div//span")).Text;
            TooltipValue.Should().Match(SliderValue);
            driver.TakeScreenshot(_testContext);
        }

        [Then(@"the ""([^""]*)"" LED is flashing")]
        public void IsLEDFlashing(string LEDType)
        {
            (int, string) LEDXpathNumber = LEDType switch
            {
                "Green" => (1, "rgba(26, 232, 79, 1)"),
                "Orange" => (2, "rgba(242, 176, 42, 1)"),
                "Red" => (3, "rgba(245, 19, 19, 1)"),
                _ => throw new ArgumentException("No such item exists.")
            };

            var LEDXPath = driver.WaitForElementToClickable(By.XPath($"//div[@data-type='led'][{LEDXpathNumber.Item1}]//div[1]"));

            bool isFlashed = false;
            //Need to catch the flashing LED at filled state, indicating it is flashed.
            for (int i = 0; i < 50; i++)
            {
                if (LEDXPath.GetCssValue("background-color").Equals(LEDXpathNumber.Item2))
                {
                    isFlashed = true;
                    break;
                }

                //If false, sleep for 0.25 of a second and try again.
                System.Threading.Thread.Sleep(250);
            }

            isFlashed.Should().BeTrue();
            driver.TakeScreenshot(_testContext);
        }

        [When(@"I input ""([^""]*)"" for ""([^""]*)"" ""([^""]*)"" input box")]
        public void ParameterInput(string Value, string ParameterType, string InputBox)
        {
            var InputBoxToUseXPath = (ParameterType, InputBox) switch
            {
                ("Warning", "Min") => "(//div[@data-type='panel'][2]//input)[1]",
                ("Warning", "Max") => "(//div[@data-type='panel'][2]//input)[2]",
                ("Danger", "Min") => "(//div[@data-type='panel'][3]//input)[1]",
                ("Danger", "Max") => "(//div[@data-type='panel'][3]//input)[2]",
                _ => throw new ArgumentException("No such input box exists.")
            };

            var InputBoxToUse = driver.WaitForElementToClickable(By.XPath(InputBoxToUseXPath));
            InputBoxToUse.SendKeys(Value + Keys.Enter);
        }

        [Then(@"the ""([^""]*)"" update button is clickable")]
        public void IsUpdateButtonClickable(string ButtonType)
        {
            var UpdateButtonXPath = ButtonType switch
            {
                "Warning" => "(//div[@data-type='button'])[1]",
                "Danger" => "(//div[@data-type='button'])[2]",
                _ => throw new ArgumentException("No such update button exists")
            };

            var UpdateButton = driver.WaitForElementToClickable(By.XPath(UpdateButtonXPath));
            UpdateButton.GetCssValue("opacity").Should().Be("1", "Object should be fully visible once it is enabled");
            driver.TakeScreenshot(_testContext);
        }

        [Then(@"the ""([^""]*)"" update button is greyed out")]
        public void IsUpdateButtonGreyedOut(string ButtonType)
        {
            var UpdateButtonXPath = ButtonType switch
            {
                "Warning" => "(//div[@data-type='button'])[1]",
                "Danger" => "(//div[@data-type='button'])[2]",
                _ => throw new ArgumentException("No such update button exists")
            };

            var UpdateButton = driver.WaitForElementToClickable(By.XPath(UpdateButtonXPath));
            UpdateButton.GetCssValue("opacity").Should().Be("0.5", "Object should be partially greyed out when disabled");
            driver.TakeScreenshot(_testContext);
        }

        [When(@"I press the ""([^""]*)"" update button")]
        public void PressUpdateButton(string ButtonType)
        {
            var UpdateButtonXPath = ButtonType switch
            {
                "Warning" => "(//div[@data-type='button'])[1]",
                "Danger" => "(//div[@data-type='button'])[2]",
                _ => throw new ArgumentException("No such update button exists")
            };

            var UpdateButton = driver.WaitForElementToClickable(By.XPath(UpdateButtonXPath));
            UpdateButton.Click();
        }

        [Then(@"the ""([^""]*)"" slider parameter range is updated accordingly")]
        public void SliderParameterUpdatedSuccessfully(string InputType)
        {
            var SliderParaXPath = InputType switch
            {
                "Warning" => "(//div[@data-type='circularGauge']//div//div//div/*[name()='svg']/*)[1]",
                "Danger" => "(//div[@data-type='circularGauge']//div//div//div/*[name()='svg']/*)[2]",
                _ => throw new ArgumentException("No such update button exists")
            };

            var SliderParameter = driver.WaitForElementToClickable(By.XPath(SliderParaXPath));
            var test = SliderParameter.GetAttribute("d");

            if (InputType.Equals("Warning"))
                SliderParameter.GetAttribute("d").Should().Match("M 224.69 113.38 A 100.5 100.5 0 0 0 124.5 5");
            else
                SliderParameter.GetAttribute("d").Should().Match("M 195.56 176.56 A 100.5 100.5 0 0 0 224.69 113.38");

            driver.TakeScreenshot(_testContext);
        }

    }
}