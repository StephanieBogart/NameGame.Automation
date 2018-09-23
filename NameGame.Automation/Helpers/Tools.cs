using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace NameGame.Automation.Helpers
{
    public class Tools
    {
        public static void WaitForElementToLoad(string CssSelectionString)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Browser.WebDriver, TimeSpan.FromSeconds(20));
                wait.Until(ExpectedConditions.ElementExists(By.CssSelector(CssSelectionString)));
                Logging.Log($"Element loaded: {CssSelectionString}.");
            }
            catch (NoSuchElementException)
            {
                Logging.Log($"Element using CssIdentifer {CssSelectionString} not found.");
                throw;
            }
        }
    }
}
