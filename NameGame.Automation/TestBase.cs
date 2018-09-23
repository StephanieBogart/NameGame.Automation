using NameGame.Automation.Helpers;
using NameGame.Automation.PageObjects;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace NameGame.Automation
{
    public class TestBase
    {
        public HomePage Home;
        public CountersPage Counters;

        [SetUp]
        public void BeforeTest()
        {
            Home = new HomePage();
            Counters = new CountersPage();

            Logging.Log($"Test Start: {TestContext.CurrentContext.Test.Name}");
        }

        [TearDown]
        public void ExecutionFailure()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
            {
                Logging.Log($"Test Failed. {TestContext.CurrentContext.Test.Name}. ");
            }
        }
    }
}
