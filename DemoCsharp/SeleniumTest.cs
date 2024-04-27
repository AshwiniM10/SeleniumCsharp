//Inside SeleniumTest.cs

using OpenQA.Selenium;

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Support.UI;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;

namespace SeleniumCsharp

{

    public class Tests : IDisposable

    {
        IWebDriver driver;
        private const string mobile = "9561995192";
        private const string otp = "951753";
        private const string DOB = "02/02/2000";

        private const string mobile2 = "9561995192";
        private const string otp2 = "951753";
        private const string DOB2 = "52/02/2000";


        private const string URL = "https://uat-oneweb.bajajfinserv.in/myaccountlogin";


        [OneTimeSetUp]

        public void Setup()

        {

            //Below code is to get the drivers folder path dynamically.

            //You can also specify chromedriver.exe path dircly ex: C:/MyProject/Project/drivers

            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            //Creates the ChomeDriver object, Executes tests on Google Chrome

            driver = new ChromeDriver(path + @"\drivers\");
            driver.Manage().Window.Maximize();

            //If you want to Execute Tests on Firefox uncomment the below code

            // Specify Correct location of geckodriver.exe folder path. Ex: C:/Project/drivers

            //driver= new FirefoxDriver(path + @"\drivers\");

        }

        [Test]
        [TestCase(mobile, otp, DOB, ExpectedResult =true)]
        //[TestCase(mobile2, otp2, DOB2, ExpectedResult =false)]
        
        public bool verifyLogin(string Mobile, string otp, string DOB)

        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            driver.Navigate().GoToUrl(URL);

            driver.FindElement(By.Id("txtEmailMobile_Individual")).SendKeys(Mobile);
            driver.FindElement(By.Id("Individual_GetOTP")).Click();
            wait.Until(d => d.FindElement(By.Id("o1")));
            Thread.Sleep(2000);
            var optarr = otp.ToCharArray();
            for (int i = 0; i < optarr.Length; i++)
            {
                driver.FindElement(By.Id("o" + (i + 1))).SendKeys(optarr[i].ToString());
            }
            driver.FindElement(By.Id("submitOTP")).Click();
            Thread.Sleep(1500);
            wait.Until(d => d.FindElement(By.Id("EnterDOBdate_Individual")));
            driver.FindElement(By.Id("EnterDOBdate_Individual")).SendKeys(DOB);
            driver.FindElement(By.XPath("//*[@id=\"Dobflow\"]/form/div[6]/button")).Click();

            var invalidtext = driver.FindElement(By.Id("EnterDOB_Errormsg_Individual")).Text;

            if (invalidtext== "Enter valid date")
            {
                return false;
            }
            Thread.Sleep(2500);
            var LogoVisble = wait.Until(d => d.FindElement(By.XPath("//*[@id=\"MasterBody \"]/app-root/app-home/div/lib-b2c-header/div/header/div/ul[1]/li/a/img")).Displayed);
            Assert.IsTrue(LogoVisble);
            return true;
        }

        [Test]
        [TestCase(mobile, otp, DOB)]


        public void verifyMenuItemcount(string Mobile, string otp, string DOB)


        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            verifyLogin(Mobile, otp, DOB);
            //wait.Until(d=> d.FindElement(By.ClassName("userprofile ng-tns-c7-0 ng-star-inserted")));
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("userprofile")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//*[@id=\"MasterBody \"]/app-root/app-home/div/lib-b2c-header/div/header/div/ul[2]/li[2]/ul/li[1]/div/div[2]/a")).Click();
            Thread.Sleep(5000);
            //ReadOnlyCollection<IWebElement> menuItem = driver.FindElements(By.XPath("//ul[contains(@class,'horizontal-list product-menu')]/li"));

            //Assert.AreEqual(menuItem.Count, 4);

        }

        [Test]

        public void verifyPricingPage()

        {

            driver.Navigate().GoToUrl("https://browserstack.com/pricing");

            IWebElement contactUsPageHeader = driver.FindElement(By.TagName("h1"));

            Assert.IsTrue(contactUsPageHeader.Text.Contains("Replace your device lab and VMs with any of these plans"));

        }




        [OneTimeTearDown]

        public void TearDown()

        {

            driver.Quit();

        }

        public void Dispose()
        {

        }
    }

}

