using HugAutomation.Resources.Pages;
using HugAutomation.Resources.Utilities;
using NUnit.Framework;

namespace HugAutomation.Tests;

[TestFixture]
public class FilterTests : BaseClass
{
    
    [Test, Description("Test checks if there is no results found for english language input")]
    public void WrongInputTest()
    {
        HomePage.Instance
            .Navigate()
            .Search("test");

        Assert.That(SearchResultPage.Instance.IsSomethingFound(), Is.False);
    }

    [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.DaysFilterTests)),
     Description("Test checks day of the week filter for all possible days")]
    public void DaysFilterTest(DayOfWeek day)
    {
        HomePage.Instance
            .Navigate()
            .ApplyDayFilter(day);
        
        Assert.That(SearchResultPage.Instance.IsSomethingFound(), Is.True);
    }
    
    [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.HoursFilterTests)),
     Description("Test checks hours of the week filter for all possible options")]
    public void HoursFilterTest(TimePeriod time)
    {
        HomePage.Instance
            .Navigate()
            .ApplyHoursFilter(time);
        
        Assert.That(SearchResultPage.Instance.IsSomethingFound(), Is.True);
    }
}