using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace NameGame.Automation
{
    public class Browser
    {
        public static IWebDriver WebDriver { get; private set; }

        //public static string Title = WebDriver.Title;

        public static void LaunchAndGoToURL(string url)
        {
            WebDriver = new ChromeDriver();
            WebDriver.Manage().Window.Maximize();
            WebDriver.Navigate().GoToUrl(url);
        }

        public static void Quit()
        {
            WebDriver.Quit();
        }
    }
}
