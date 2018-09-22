using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace SeleniumWebDriver
{
    [TestClass]
    public class HomePageTests
    {
        IWebDriver driver;

        [TestInitialize]
        public void TestSetup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://fluxday.io/");
        }

        [TestCleanup]
        public void TestTeardown()
        {
            driver.Quit();
        }

        [TestCategory("HomePageTests")]
        [TestMethod]
        public void Test001NavigateToHomePage()
        {

            IWebElement pageTitle = driver.FindElement(By.ClassName("name"));
            var actualResult = pageTitle.Text;

            var expectedResult = "opensource task & productivity management tool for startups".ToUpper();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCategory("HomePageTests")]
        [TestMethod]
        public void Test002NavigateToLiveDemoLoginPage()
        {
            var demoLink = driver.FindElement(By.XPath("//a[contains(text(), 'Demo')]"));
            demoLink.Click();
            Thread.Sleep(3000);

            var tryLiveDemo = driver.FindElement(By.CssSelector(".text-center.spacer a"));
            tryLiveDemo.Click();
            Thread.Sleep(1000);

            var tabs = driver.WindowHandles;
            driver.SwitchTo().Window(tabs[tabs.Count - 1]);

            var emailInput = driver.FindElement(By.Id("user_email"));

            Assert.IsNotNull(emailInput);
        }
    }
}
