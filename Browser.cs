using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace WillowTree.NameGame.Automation
{
    public enum BrowserDriver
    {
        IE,
        Chrome,
        Firefox
    }

    public class Browser
    {
        public static IWebDriver WebDriver { get; private set; }

        public static string Title = WebDriver.Title;
        public static string NameGame = "http://www.ericrochester.com/name-game/";

        private static IWebDriver GetDriver(BrowserDriver driver)
        {
            switch (driver)
            {

                case BrowserDriver.Firefox:
                    return new FirefoxDriver();
                case BrowserDriver.IE:
                    return new InternetExplorerDriver();
                case BrowserDriver.Chrome:
                default:
                    return new ChromeDriver();
            }
        }

        public static void LaunchAndGoToURL(BrowserDriver Browser)
        {
            WebDriver = GetDriver(Browser);
            WebDriver.Manage().Window.Maximize();
            WebDriver.Navigate().GoToUrl(NameGame);
        }

        public static void Quit()
        {
            WebDriver.Quit();
        }
    }
}
