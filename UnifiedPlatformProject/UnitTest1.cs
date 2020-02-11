using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Selenium3.Nunit.Scripts.OnboardingTests
{
    [TestFixture]
    [Category("InstantSauceTest"), Category("NUnit"), Category("Instant")]
    public class InstantSauceTest
    {
        private IWebDriver _driver;
        public String sauceUserName = "YOUR USERNAME";
        public String sauceAccessKey = "YOUR ACCESS KEY";

        [Test]
        public void UnifiedPlatformWebEMUSIM()
        {

            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability("deviceName", "Android GoogleAPI Emulator");
            capabilities.SetCapability("deviceOrientation", "portrait");
            capabilities.SetCapability("browserName", "Chrome");
            capabilities.SetCapability("platformVersion", "8.0");
            capabilities.SetCapability("platformName", "Android");
            capabilities.SetCapability("name", TestContext.CurrentContext.Test.Name);


            _driver = new AndroidDriver<IWebElement>(new Uri("https://"+ sauceUserName + ":" + sauceAccessKey + "@ondemand.us-west-1.saucelabs.com:443/wd/hub"),
                capabilities, TimeSpan.FromSeconds(600));
            //navigate to the url of the Sauce Labs Sample app
            _driver.Navigate().GoToUrl("https://www.saucedemo.com");

            //Create an instance of a Selenium explicit wait so that we can dynamically wait for an element
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            //wait for the user name field to be visible and store that element into a variable
            var userNameField = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[type='text']")));
            //type the user name string into the user name field
            userNameField.SendKeys("standard_user");
            //type the password into the password field
            _driver.FindElement(By.CssSelector("[type='password']")).SendKeys("secret_sauce");
            //hit Login button
            _driver.FindElement(By.CssSelector("[type='submit']")).Click();

            //Synchronize on the next page and make sure it loads
            var inventoryPageLocator =
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("inventory_container")));
            //Assert that the inventory page displayed appropriately
            Assert.IsTrue(inventoryPageLocator.Displayed);
        }


        [Test]
        public void UnifiedPlatformNativeEMUSIM()
        {
            // Download app from https://github.com/saucelabs/sample-app-mobile/releases/download/2.2.1/Android.SauceLabs.Mobile.Sample.app.2.2.1.apk
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability("deviceName", "Android GoogleAPI Emulator");
            capabilities.SetCapability("deviceOrientation", "portrait");
            capabilities.SetCapability("browserName", "");
            capabilities.SetCapability("app", "sauce-storage:sample-sauce.apk");
            capabilities.SetCapability("platformVersion", "7.0");
            capabilities.SetCapability("platformName", "Android");
            capabilities.SetCapability("name", TestContext.CurrentContext.Test.Name);
            capabilities.SetCapability("appWaitActivity", "com.swaglabsmobileapp.MainActivity");


            _driver = new AndroidDriver<IWebElement>(new Uri("https://" + sauceUserName + ":" + sauceAccessKey + "@ondemand.us-west-1.saucelabs.com:443/wd/hub"),
                capabilities, TimeSpan.FromSeconds(600));

            String source = _driver.PageSource;
            Console.WriteLine(source);
            

    
        }

        [Test]
        public void UnifiedPlatformNativeRDC()
        {
            // Download app from https://github.com/saucelabs/sample-app-mobile/releases/download/2.2.1/Android.SauceLabs.Mobile.Sample.app.2.2.1.apk
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability("deviceName", "Samsung Galaxy.*");
            capabilities.SetCapability("deviceOrientation", "portrait");
            capabilities.SetCapability("browserName", "");
            capabilities.SetCapability("app", "sauce-storage:sample-sauce.apk");
            capabilities.SetCapability("platformName", "Android");
            capabilities.SetCapability("name", TestContext.CurrentContext.Test.Name);
            capabilities.SetCapability("appWaitActivity", "com.swaglabsmobileapp.MainActivity");


            _driver = new AndroidDriver<IWebElement>(new Uri("https://" + sauceUserName + ":" + sauceAccessKey + "@ondemand.us-west-1.saucelabs.com:443/wd/hub"),
                capabilities, TimeSpan.FromSeconds(600));

            String source = _driver.PageSource;
            Console.WriteLine(source);



        }
        /*
         *Below we are performing 2 critical actions. Quitting the driver and passing
         * the test result to Sauce Labs user interface.
         */
        [TearDown]
        public void CleanUpAfterEveryTestMethod()
        {
            var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            ((IJavaScriptExecutor)_driver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
            _driver?.Quit();
        }
    }
}