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

            Thread.Sleep(1000);

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
            var adminUserLink = driver.FindElement(By.LinkText("Admin User"));

            var actualResult = adminUserLink.Text;

            var expectedResult = "Admin User";

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCategory("AdminUserTests")]
        [TestMethod]
        public void Test002AssignTeamLeadAsManagerInFinanceDepartment()
        {
            var departmentsLink = driver.FindElement(By.LinkText("Departments"));
            departmentsLink.Click();

            Thread.Sleep(1000);

            var financeDepartment = driver.FindElement(By.XPath("//*[@id=\"pane2\"]/div[2]/a[3]/div"));
            financeDepartment.Click();

            Thread.Sleep(1000);

            var gearIcon = driver.FindElement(By.XPath("//*[@id=\"pane3\"]/div/div[1]/div[2]/a/div"));
            gearIcon.Click();

            var editOption = driver.FindElement(By.LinkText("Edit"));
            editOption.Click();

            Thread.Sleep(1000);

            var textField = driver.FindElement(By.Id("s2id_autogen1"));
            textField.Click();

            var teamLeadRole = driver.FindElement(By.XPath("//*[@id=\"project_user_ids\"]/option[2]"));
            teamLeadRole.Click();

            var saveButton = driver.FindElement(By.ClassName("button"));
            saveButton.Click();

            var managersLink = driver.FindElement(By.XPath("//*[@id=\"pane3\"]/div/div[2]/dl/dd[3]/a"));
            managersLink.Click();

            var teamLeadManager = driver.FindElement(By.LinkText("Team Lead"));

            var expectedResult = "Team Lead";
            var actualResult = teamLeadManager.Text;

            Assert.AreEqual(expectedResult, actualResult);

            gearIcon = driver.FindElement(By.XPath("//*[@id=\"pane3\"]/div/div[1]/div[2]/a/div"));
            gearIcon.Click();
            editOption = driver.FindElement(By.LinkText("Edit"));
            editOption.Click();
            Thread.Sleep(1000);
            teamLeadRole = driver.FindElement(By.XPath("//*[@id=\"project_user_ids\"]/option[2]"));
            teamLeadRole.Click();
            saveButton = driver.FindElement(By.ClassName("button"));
            saveButton.Click();
        }
    }
}
