using NUnit.Framework;

namespace HugAutomation.Resources.Utilities;

public class TestCaseSources : CommonConstants
{
    public static IEnumerable<TestCaseData> DaysFilterTests =>
        from DayOfWeek dayOfWeek in Enum.GetValues(typeof(DayOfWeek))
        select new TestCaseData(dayOfWeek);
    
    public static IEnumerable<TestCaseData> HoursFilterTests =>
        from TimePeriod timePeriod in Enum.GetValues(typeof(TimePeriod))
        select new TestCaseData(timePeriod);
}