using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace SeleniumWebDriver
{
    [TestClass]
    public class AdminUserTests
    {
        IWebDriver driver;

        [TestInitialize]
        public void TestSetup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://fluxday.io/");
            var demoLink = driver.FindElement(By.XPath("//a[contains(text(), 'Demo')]"));
            demoLink.Click();
            Thread.Sleep(1000);

            var tryLiveDemo = driver.FindElement(By.CssSelector(".text-center.spacer a"));
            tryLiveDemo.Click();
            Thread.Sleep(1000);

            var tabs = driver.WindowHandles;
            driver.SwitchTo().Window(tabs[tabs.Count - 1]);

            var emailInput = driver.FindElement(By.Id("user_email"));
            emailInput.SendKeys("admin@fluxday.io");

            var passwordInput = driver.FindElement(By.Id("user_password"));
            passwordInput.SendKeys("password");

            var clickLogin = driver.FindElement(By.ClassName("btn-login"));
            clickLogin.Click();   
        }

        [TestCleanup]
        public void TestTeardown()
        {
            driver.Quit();
        }
       
        [TestCategory("AdminUserTests")]
        [TestMethod]
        public void Test001LogInAsAnAdmin()
        {           
            var adminUserLink = driver.FindElement(By.XPath("/html/body/div[2]/div[1]/ul[3]/li[1]/a"));

            var actualResult = adminUserLink.Text;

            var expectedResult = "Admin User";

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
