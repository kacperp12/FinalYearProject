using FYPTesting.Drivers;
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
        IWebDriver driver;
        private readonly ScenarioContext _scenarioContext;

        public SecondPanelSteps(ScenarioContext scenarioContext)
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

        [Then(@"the object ""([^""]*)"" Exists")]
        public void ThenTheObjectExists(string Obj)
        {
            var ObjXPath = Obj switch
            {
                "Slider" => "//div[@data-type='panel'][1]//div",
                "WarningMinInput" => "(//div[@data-type='panel'][2]//input)[1]",
                "WarningMaxInput" => "(//div[@data-type='panel'][2]//input)[2]",
                "WarningUpdateButton" => "//div[@data-type='panel'][2]//div[6]",
                "DangerMinInput" => "(//div[@data-type='panel'][3]//input)[1]",
                "DangerMaxInput" => "(//div[@data-type='panel'][3]//input)[2]",
                "DangerUpdateButton" => "//div[@data-type='panel'][3]//div[6]",
                "GreenLED" => "//div[@data-type='panel'][4]//div//div[2]",
                "OrangeLED" => "(//div[@data-type='panel'][4]//div//div)[4]",
                "RedLED" => "(//div[@data-type='panel'][4]//div//div)[6]",
                _ => throw new Exception("No such element exists!")
            };

            var ObjToLocate = WaitForElementToClickable(By.XPath(ObjXPath));
            ObjToLocate.Should().NotBeNull();
        }

        [When(@"I drag the slider to value ""([^""]*)""")]
        public void WhenIDragTheSliderToValue(string SliderValue)
        {
            Actions builder = new Actions(driver);
            var Source = WaitForElementToClickable(By.XPath("//div[@data-type='circularGauge']//div[@class='sliderHandle-0-3-7']"));
            var Destination = WaitForElementToClickable(By.XPath($"//div[@data-type='circularGauge']//span[text()='{SliderValue}']"));

            var DragAndDrop = builder.ClickAndHold(Source)
                .MoveToElement(Destination)
                .Release(Destination)
                .Build();

            DragAndDrop.Perform();
        }

        [Then(@"the slider value is set to ""([^""]*)""")]
        public void ThenTheSliderValueIsSetTo(string SliderValue)
        {
            var TooltipValue = WaitForElementToClickable(By.XPath("//div[@data-type='circularGauge']/div/div/div/div/div/div//span")).Text;
            TooltipValue.Should().Match(SliderValue);
        }

        [Then(@"the ""([^""]*)"" LED is flashing")]
        public void ThenTheLEDIsFlashing(string LEDType)
        {
            (int, string) LEDXpathNumber = LEDType switch
            {
                "Green" => (1, "rgba(26, 232, 79, 1)"),
                "Orange" => (2, "rgba(242, 176, 42, 1)"),
                "Red" => (3, "rgba(245, 19, 19, 1)"),
                _ => throw new ArgumentException("No such item exists.")
            };

            var LEDXPath = WaitForElementToClickable(By.XPath($"//div[@data-type='led'][{LEDXpathNumber.Item1}]//div[1]"));

            bool isFlashed = false;
            //Need to catch the flashing LED at filled state, indicating it is flashed.
            for(int i = 0; i < 50; i++)
            {
                if(LEDXPath.GetCssValue("background-color").Equals(LEDXpathNumber.Item2))
                {
                    isFlashed = true;
                    break;
                }

                //If false, sleep for 0.25 of a second and try again.
                System.Threading.Thread.Sleep(250);
            }

            isFlashed.Should().BeTrue();
        }

        [When(@"I input ""([^""]*)"" for ""([^""]*)"" ""([^""]*)"" input box")]
        public void WhenIInputForInputBox(string Value, string ParameterType, string InputBox)
        {
            var InputBoxToUseXPath = (ParameterType, InputBox) switch
            {
                ("Warning", "Min") => "(//div[@data-type='panel'][2]//input)[1]",
                ("Warning", "Max") => "(//div[@data-type='panel'][2]//input)[2]",
                ("Danger", "Min") => "(//div[@data-type='panel'][3]//input)[1]",
                ("Danger", "Max") => "(//div[@data-type='panel'][3]//input)[2]",
                _ => throw new ArgumentException("No such input box exists.")
            };

            var InputBoxToUse = WaitForElementToClickable(By.XPath(InputBoxToUseXPath));
            InputBoxToUse.SendKeys(Value + Keys.Enter);
        }

        [Then(@"the ""([^""]*)"" update button is clickable")]
        public void WhenTheUpdateButtonIsClickable(string ButtonType)
        {
            var UpdateButtonXPath = ButtonType switch
            {
                "Warning" => "(//div[@data-type='button'])[1]",
                "Danger" => "(//div[@data-type='button'])[2]",
                _ => throw new ArgumentException("No such update button exists")
            };

            var UpdateButton = WaitForElementToClickable(By.XPath(UpdateButtonXPath));
            UpdateButton.GetCssValue("opacity").Should().Be("1", "Object should be fully visible once it is enabled");
        }

        [Then(@"the ""([^""]*)"" update button is greyed out")]
        public void WhenTheUpdateButtonIsGreyedOut(string ButtonType)
        {
            var UpdateButtonXPath = ButtonType switch
            {
                "Warning" => "(//div[@data-type='button'])[1]",
                "Danger" => "(//div[@data-type='button'])[2]",
                _ => throw new ArgumentException("No such update button exists")
            };

            var UpdateButton = WaitForElementToClickable(By.XPath(UpdateButtonXPath));
            UpdateButton.GetCssValue("opacity").Should().Be("0.5", "Object should be partially greyed out when disabled");
        }

        [When(@"I press the ""([^""]*)"" update button")]
        public void WhenIPressTheUpdateButton(string ButtonType)
        {
            var UpdateButtonXPath = ButtonType switch
            {
                "Warning" => "(//div[@data-type='button'])[1]",
                "Danger" => "(//div[@data-type='button'])[2]",
                _ => throw new ArgumentException("No such update button exists")
            };

            var UpdateButton = WaitForElementToClickable(By.XPath(UpdateButtonXPath));
            UpdateButton.Click();
        }


        [Then(@"the ""([^""]*)"" slider parameter range is updated accordingly")]
        public void ThenTheSliderParameterRangeIsUpdatedAccordingly(string InputType)
        {
            var SliderParaXPath = InputType switch
            {
                "Warning" => "(//div[@data-type='circularGauge']//div//div//div/*[name()='svg']/*)[1]",
                "Danger" => "(//div[@data-type='circularGauge']//div//div//div/*[name()='svg']/*)[2]",
                _ => throw new ArgumentException("No such update button exists")
            };

            var SliderParameter = WaitForElementToClickable(By.XPath(SliderParaXPath));
            var test = SliderParameter.GetAttribute("d");

            if (InputType.Equals("Warning"))
                SliderParameter.GetAttribute("d").Should().Match("M 45.95 13.17 A 75 75 0 0 0 8.67 103.17");
            else
                SliderParameter.GetAttribute("d").Should().Match("M 143.94 119.18 A 75 75 0 0 0 114.04 13.17");
        }

    }
}