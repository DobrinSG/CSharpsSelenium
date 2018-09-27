using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
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
            driver.Navigate().GoToUrl("https://fluxday.io/");

            var demoLink = driver.FindElement(By.XPath("//a[contains(text(), 'Demo')]"));
            demoLink.Click();
            Thread.Sleep(1000);

            var tryLiveDemo = driver.FindElement(By.CssSelector(".text-center.spacer a"));
            tryLiveDemo.Click();
            Thread.Sleep(1000);

            var tabs = driver.WindowHandles;
            driver.SwitchTo().Window(tabs[tabs.Count - 1]);

            LoginAsAnAdmin("admin@fluxday.io", "password");
            Thread.Sleep(2000);

            VerifyLoginAsAnAdmin();
        }

        [TestCategory("AdminUserTests")]
        [TestMethod]
        public void Test002AssignTeamLeadAsManagerInFinanceDepartment()
        {
            driver.Navigate().GoToUrl("https://app.fluxday.io/users/sign_in");

            LoginAsAnAdmin("admin@fluxday.io", "password");

            var departmentsLink = driver.FindElement(By.LinkText("Departments"));
            departmentsLink.Click();
            Thread.Sleep(2000);

            var financeDepartment = driver.FindElement(By.XPath("//*[@id=\"pane2\"]/div[2]/a[3]/div"));
            financeDepartment.Click();
            Thread.Sleep(1000);

            AssignManager();

            var managersLink = driver.FindElement(By.XPath("//*[@id=\"pane3\"]/div/div[2]/dl/dd[3]/a"));
            managersLink.Click();
            
            var teamLeadManager = driver.FindElement(By.LinkText("Team Lead"));
            
            var expectedResult = "Team Lead";
            var actualResult = teamLeadManager.Text; 

            Assert.AreEqual(expectedResult, actualResult);

            AssignManager();
        }

        [TestCategory("AdminUserTests")]
        [TestMethod]
        public void Test003AddOkr()
        {
            driver.Navigate().GoToUrl("https://app.fluxday.io/users/sign_in");

            LoginAsAnAdmin("admin@fluxday.io", "password");

            var okrLink = driver.FindElement(By.LinkText("OKR"));
            okrLink.Click();
            Thread.Sleep(2000);

            var newOkrLink = driver.FindElement(By.LinkText("New OKR"));
            newOkrLink.Click();

            var nameField = driver.FindElement(By.Id("okr_name"));
            DateTime dateTime = DateTime.Now;
            var name = dateTime.Millisecond;
            nameField.SendKeys($"Test{name}");

            var objectives = driver.FindElement(By.Id("okr_objectives_attributes_0_name"));
            objectives.SendKeys("Test");

            var keyResult = driver.FindElement(By.Id("okr_objectives_attributes_0_key_results_attributes_0_name"));
            keyResult.SendKeys("Test");

            var keyResult1 = driver.FindElement(By.Id("okr_objectives_attributes_0_key_results_attributes_1_name"));
            keyResult1.SendKeys("Test");

            var saveButton = driver.FindElement(By.ClassName("button"));
            saveButton.Click();

            var okrName = driver.FindElement(By.XPath("//*[@id=\"pane3\"]/div/div[2]/div[1]"));

            var expectedResult = $"Test{name}";
            var actualResult = okrName.Text;

            Assert.AreEqual(expectedResult, actualResult);           
        }

        [TestCategory("AdminUserTests")]
        [TestMethod]
        public void Test004AddUser()
        {
            driver.Navigate().GoToUrl("https://app.fluxday.io/users/sign_in");

            LoginAsAnAdmin("admin@fluxday.io", "password");

            var users = driver.FindElement(By.XPath("/html/body/div[2]/div[1]/ul[2]/li[5]/a"));
            users.Click();
            Thread.Sleep(2000);

            var addUser = driver.FindElement(By.LinkText("Add user"));
            addUser.Click();

            DateTime dateTime = DateTime.Now;
            var userName = dateTime.Millisecond;

            var nameField = driver.FindElement(By.Id("user_name"));
            nameField.SendKeys($"Test{userName}");

            var nickname = driver.FindElement(By.Id("user_nickname"));
            nickname.SendKeys($"T{userName}");

            var email = driver.FindElement(By.Id("user_email"));
            email.SendKeys($"Test{userName}@fluxday.io");

            var employeeCode = driver.FindElement(By.Id("user_employee_code"));
            employeeCode.SendKeys($"Test{userName}");

            var password = driver.FindElement(By.Id("user_password"));
            password.SendKeys("password");

            var confirmPassword = driver.FindElement(By.Id("user_password_confirmation"));
            confirmPassword.SendKeys("password");

            var saveButton = driver.FindElement(By.ClassName("button"));
            saveButton.Click();

            var verifyUserName = driver.FindElement(By.LinkText($"Test{userName}"));

            var expectedResult = $"Test{userName}";
            var actualResult = verifyUserName.Text;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCategory("AdminUserTests")]
        [TestMethod]
        public void Test005ChangeYourOwnPasswordAsAdmin()
        {
            driver.Navigate().GoToUrl("https://app.fluxday.io/users/sign_in");

            var emailInput = "admin@fluxday.io";
            var newPassword = "123456789";
            var oldPassword = "password";

            LoginAsAnAdmin(emailInput, oldPassword);

            var adminUserLink = driver.FindElement(By.LinkText("Admin User"));
            adminUserLink.Click();
            Thread.Sleep(1000);

            ChangePassword(newPassword);

            LoginAsAnAdmin(emailInput, newPassword);

            VerifyLoginAsAnAdmin();

            ChangePassword(oldPassword);

            LoginAsAnAdmin(emailInput, oldPassword);
        }

        private void LoginAsAnAdmin(string email, string password)
        {
            var emailInput = driver.FindElement(By.Id("user_email"));
            emailInput.SendKeys(email);

            var passwordInput = driver.FindElement(By.Id("user_password"));
            passwordInput.SendKeys(password);
            Thread.Sleep(1000);

            var clickLogin = driver.FindElement(By.ClassName("btn-login"));
            clickLogin.Click();
        }

        private void VerifyLoginAsAnAdmin()
        {
            var adminUserLink = driver.FindElement(By.LinkText("Admin User"));

            var actualResult = adminUserLink.Text;

            var expectedResult = "Admin User";

            Assert.AreEqual(expectedResult, actualResult);
        }

        private void AssignManager()
        {
            var gearIcon = driver.FindElement(By.XPath("//*[@id=\"pane3\"]/div/div[1]/div[2]/a/div"));
            gearIcon.Click();

            var editOption = driver.FindElement(By.LinkText("Edit"));
            editOption.Click();
            Thread.Sleep(2000); 

            var textField = driver.FindElement(By.Id("s2id_project_user_ids"));
            textField.Click();

            var teamLeadRole = driver.FindElement(By.XPath("//*[@id=\"project_user_ids\"]/option[2]"));
            teamLeadRole.Click();

            var saveButton = driver.FindElement(By.ClassName("button"));
            saveButton.Click();
        }

        private void ChangePassword(string password)
        {
            var gearIcon = driver.FindElement(By.XPath("//*[@id=\"pane3\"]/div/div[1]/div[2]/a/div"));
            gearIcon.Click();

            var changePasswordOption = driver.FindElement(By.LinkText("Change password"));
            changePasswordOption.Click();
            Thread.Sleep(2000);

            var passwordField = driver.FindElement(By.Id("user_password"));
            passwordField.SendKeys(password);

            var confirmPasswordField = driver.FindElement(By.Id("user_password_confirmation"));
            confirmPasswordField.SendKeys(password);

            var saveButton = driver.FindElement(By.ClassName("button"));
            saveButton.Click();
        }
    }
}
