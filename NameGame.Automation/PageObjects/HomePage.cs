using NameGame.Automation.Helpers;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NameGame.Automation.PageObjects
{
    public enum MatchType
    {
        Match,
        NonMatch
    }

    public class HomePage
    {
        public HomePage()
        {
            PageFactory.InitElements(Browser.WebDriver, this);
        }

        public const string CssSelectorPhoto = ".photo";
        public const string CssSelectorCustomIdentifierForFindingUniquePhotos = "data-n";


        [FindsBy(How = How.ClassName, Using = "text-muted")]
        public IWebElement PageTitle { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".container .text-center:first-of-type")]
        public IWebElement WhoIsQuery { get; set; }

        [FindsBy(How = How.Id, Using = "name")]
        public IWebElement EmployeeNameToMatch { get; set; }
               
        [FindsBy(How = How.ClassName, Using = "name")]
        public IWebElement EmployeePhotoName { get; set; }
               
        [FindsBy(How = How.CssSelector, Using = CssSelectorPhoto)]
        public IWebElement FirstGalleryImage { get; set; }


        public static void VerifyEmployeeNeverToMatchIsSeenMoreOftenThanOtherCorrectlyChosenEmployees(string EmployeeNameToNeverMatch, Dictionary<string, int> EmployeesAndNumberOfTimesSeen)
        {
            int NumberOfTimesAnotherEmployeeWasSeenMoreThanTarget = 0;
            int NumberOfTimesAnotherEmployeeWasSeenEqualTimesToTheTarget = 0;

            var keys = new List<string>(EmployeesAndNumberOfTimesSeen.Keys);
            foreach (string key in keys)
            {
                Logging.Log($"------- {key} seen {EmployeesAndNumberOfTimesSeen[key]} times");
                if (key != EmployeeNameToNeverMatch)
                {
                    if (EmployeesAndNumberOfTimesSeen[EmployeeNameToNeverMatch] < EmployeesAndNumberOfTimesSeen[key])
                    {
                        Logging.Log($"Employee {key} has been seen more times than the target employee {EmployeeNameToNeverMatch}");
                        NumberOfTimesAnotherEmployeeWasSeenMoreThanTarget += 1;
                    }
                    else if (EmployeesAndNumberOfTimesSeen[EmployeeNameToNeverMatch] == EmployeesAndNumberOfTimesSeen[key])
                    {
                        Logging.Log($"Employee {key} has been seen the same number of times as the target employee {EmployeeNameToNeverMatch}");
                        NumberOfTimesAnotherEmployeeWasSeenEqualTimesToTheTarget += 1;
                    }
                    else
                    {
                        //Logging.Log($"Employee {key} has been seen less often than the target employee {EmployeeNameToNeverMatch}");
                    }
                }
            }

            if (NumberOfTimesAnotherEmployeeWasSeenEqualTimesToTheTarget > 0 || NumberOfTimesAnotherEmployeeWasSeenMoreThanTarget > 0)
            {
                Assert.Fail($"Correctly matched employees were seen more often ({NumberOfTimesAnotherEmployeeWasSeenMoreThanTarget}) or the same number of times ({NumberOfTimesAnotherEmployeeWasSeenEqualTimesToTheTarget} as the target.");
                Logging.Log($"Correctly matched employees were seen more often ({NumberOfTimesAnotherEmployeeWasSeenMoreThanTarget}) or the same number of times ({NumberOfTimesAnotherEmployeeWasSeenEqualTimesToTheTarget} as the target.");
            }
        }

        public static Dictionary<string, int> AlwaysFailOnTargetEmployeeAndSucceedOnOthers(string EmployeeNameToNeverMatch, Dictionary<string, int> EmployeesAndNumberOfTimesSeen)
        {
            int NumberOfTestRuns = 0;
            while (NumberOfTestRuns < 200)
            {
                string Employee = GetEmployeeNameToMatch();
                if (EmployeesAndNumberOfTimesSeen.ContainsKey(Employee))
                {
                    EmployeesAndNumberOfTimesSeen[Employee] += 1;
                    Logging.Log($"Number of times {Employee} has been seen {EmployeesAndNumberOfTimesSeen[Employee]}.");
                }
                else
                {
                    EmployeesAndNumberOfTimesSeen.Add(Employee, 1);
                    Logging.Log($"Number of times {Employee} added {EmployeesAndNumberOfTimesSeen[Employee].ToString()}.");
                }

                if (GetEmployeeNameToMatch() == EmployeeNameToNeverMatch)
                {
                    ClickOnImageByDesiredOutcome(MatchType.NonMatch);
                    ClickOnImageByDesiredOutcome(MatchType.Match);
                    Logging.Log("Encountered employee to match.");
                }
                else
                {
                    ClickOnImageByDesiredOutcome(MatchType.Match);
                }
                NumberOfTestRuns += 1;
            }
            return EmployeesAndNumberOfTimesSeen;
        }

        public static void ClickOnImageByDesiredOutcome(MatchType Match)
        {
            HomePage Home = new HomePage();
            Thread.Sleep(TimeSpan.FromSeconds(4));
            var ImageNumber = Convert.ToInt32(Home.EmployeeNameToMatch.GetAttribute(CssSelectorCustomIdentifierForFindingUniquePhotos));

            if (Match == MatchType.Match)
            {
                var DetermineMatchingImage = IWebElementForSpecificPhoto(ImageNumber);
                DetermineMatchingImage.Click();
                Logging.Log($"Selecting {Home.EmployeeNameToMatch.Text}");
            }
            else if (Match == MatchType.NonMatch)
            {
                // Determine a random number between 1 and 5, verify it doesn't match the target image
                Random random = new Random();
                int NonMatchingNumber = random.Next(0, 5);

                while (true)
                {
                    if (NonMatchingNumber != Convert.ToInt32(ImageNumber))
                        break;
                    else
                        NonMatchingNumber = random.Next(0, 5);
                }
                var DetermineNonMatchingImageToSelect = IWebElementForSpecificPhoto(NonMatchingNumber);
                DetermineNonMatchingImageToSelect.Click();
                Logging.Log($"Selecting {GetEmployeeNameByImageNumber(NonMatchingNumber)}");
            }
        }

        public static void ClickOnImageDefinedByImageNumber(int SpecifiedPhotoNumber)
        {
            //Check for a photo to load to determine when the dynamic data has refreshed after a reload/first load
            Tools.WaitForElementToLoad(CssSelectorPhoto);
            SpecifiedPhotoNumber += 1;
            string PhotoNumberConvertedToString = SpecifiedPhotoNumber.ToString();
            Logging.Log($"Element number clicked {PhotoNumberConvertedToString}");

            Tools.WaitForElementToLoad($"{CssSelectorPhoto}:nth-child({PhotoNumberConvertedToString})");
            Browser.WebDriver.FindElement(By.CssSelector($"{CssSelectorPhoto}:nth-child({PhotoNumberConvertedToString})")).Click();
        }

        public static string GetEmployeeNameToMatch()
        {
            HomePage Home = new HomePage();
            Tools.WaitForElementToLoad(CssSelectorPhoto);

            Logging.Log($"Getting Employee Name to Match: {Home.EmployeeNameToMatch.Text}");
            return Home.EmployeeNameToMatch.Text;
        }

        public static List<string> GetDisplayedEmployeeChoices()
        {
            List<string> StartingDisplayedPhotos = new List<string>();

            int counter = 0;
            while (counter < 5)
            {
                var PhotoIdentifier = counter + 1;
                var Photo = Browser.WebDriver.FindElement(By.CssSelector($".photo:nth-child({PhotoIdentifier}) .name"));
                StartingDisplayedPhotos.Add(Photo.Text);
                Logging.Log($"Adding element name {Photo.Text}");
                counter += 1;
            }

            return StartingDisplayedPhotos;
        }

        public static string GetEmployeeNameByImageNumber(int SpecifiedPhotoNumber)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            SpecifiedPhotoNumber += 1;
            return Browser.WebDriver.FindElement(By.CssSelector($"{CssSelectorPhoto}:nth-child({SpecifiedPhotoNumber.ToString()}) .name")).Text;
        }

        public static void PerformSuccessfulMatchSpecifiedNumberOfTimes(int NumberOfSuccessfulMatches)
        {
            CountersPage Counters = new CountersPage();

            var counter = 0;
            while (counter < NumberOfSuccessfulMatches)
            {
                Tools.WaitForElementToLoad(CssSelectorPhoto);
                ClickOnImageByDesiredOutcome(MatchType.Match);
                counter += 1;
                Logging.Log($"Streak Counter after match made: {Counters.SuccessStreakCounter.Text}");
            }
        }
        
        private static IWebElement IWebElementForSpecificPhoto(int PhotoNumberAsInt)
        {
            // Numbers for photos start at 0, nth-child starts at 1
            PhotoNumberAsInt += 1;
            return Browser.WebDriver.FindElement(By.CssSelector($"{CssSelectorPhoto}:nth-child({PhotoNumberAsInt.ToString()}"));
        }


    }
}