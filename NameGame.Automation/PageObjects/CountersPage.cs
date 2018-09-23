using NameGame.Automation.Helpers;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;

namespace NameGame.Automation.PageObjects
{
    public enum CounterType
    {
        Attempt,
        Correct,
        Streak
    }
    public class CountersPage : HomePage
    {
        public CountersPage()
        {
            PageFactory.InitElements(Browser.WebDriver, this);
        }

        public const string CssSelectorCorrectStreak = ".streak";
        public const string CssSelectorTotalCorrect = ".correct";
        public const string CssSelectorTotalAttempts = ".attempts";

        [FindsBy(How = How.CssSelector, Using = CssSelectorTotalAttempts)]
        public IWebElement TotalAttempts { get; set; }

        [FindsBy(How = How.CssSelector, Using = CssSelectorTotalCorrect)]
        public IWebElement TotalCorrectAttempts { get; set; }

        [FindsBy(How = How.CssSelector, Using = CssSelectorCorrectStreak)]
        public IWebElement SuccessStreakCounter { get; set; }

        public static int GetCounterValue(CounterType CounterType)
        {
            CountersPage Counters = new CountersPage();

            if (CounterType == CounterType.Attempt)
            {
                Tools.WaitForElementToLoad(CssSelectorPhoto);

                int Attempts = Convert.ToInt32(Counters.TotalAttempts.Text);
                Logging.Log($"Total Attempts counter: {Counters.TotalAttempts.ToString()}");
                return Attempts;
            }
            else if (CounterType == CounterType.Correct)
            {
                Tools.WaitForElementToLoad(CssSelectorPhoto);

                int Correct = Convert.ToInt32(Counters.TotalCorrectAttempts.Text);
                Logging.Log($"Correct Attempts counter: {Counters.TotalCorrectAttempts.ToString()}");
                return Correct;
            }
            else
            {
                Tools.WaitForElementToLoad(CssSelectorPhoto);

                int Streak = Convert.ToInt32(Counters.SuccessStreakCounter.Text);
                Logging.Log($"Correct Attempts counter: {Counters.SuccessStreakCounter.ToString()}");
                return Streak;
            }
        }
    }
}
