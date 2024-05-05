using HugAutomation.Resources.Utilities;
using OpenQA.Selenium;

namespace HugAutomation.Resources.Pages;

public class HomePage : CommonConstants
{
    private const string BaseUrl = "https://www.hughug.co.il/myhughug/demo";
    private static readonly By SearchInput = By.XPath("//input[@class='q']");
    private static readonly By SearchButton = By.XPath("//input[@value='חפש']");
    private static readonly By DayFilter = By.Id("fltr-dday");
    private static readonly By HoursFilter = By.Id("fltr-dhours");
    

    public static HomePage Instance { get; } = new();

    public HomePage Navigate()
    {
        ExtentReportHolder.LogMessage("Navigating to Home Page...");
        Selenium.Instance.NavigateToUrl(BaseUrl);
        return this;
    }

    public void Search(string text)
    {
        ExtentReportHolder.LogMessage($"Searching for: {text}");
        Selenium.Instance.TypeToElement(SearchInput, text);
        Selenium.Instance.ClickOnElement(SearchButton);
    }
    
    public HomePage ApplyDayFilter(DayOfWeek day)
    {
        ExtentReportHolder.LogMessage($"Day filter applied for: {day}");
        
        Selenium.Instance.ClickOnElement(DayFilter);
        Selenium.Instance.ClickOnElement(GetDaySelector(day));
        return this;
    }
    
    private By GetDaySelector(DayOfWeek day) => By.XPath($"//select[@id='fltr-dday']/option[@value='{(int)day}']");
    
    public HomePage ApplyHoursFilter(TimePeriod period)
    {
        ExtentReportHolder.LogMessage($"Hour filter applied for: {period}");
    
        Selenium.Instance.ClickOnElement(HoursFilter);
        Selenium.Instance.ClickOnElement(GetHourSelector(period));
        return this;
    }

    private By GetHourSelector(TimePeriod period)
    {
        var value = period switch
        {
            TimePeriod.All => "-1",
            TimePeriod.Morning => "06:00-10:59",
            TimePeriod.Noon => "11:00-14:59",
            TimePeriod.Afternoon => "15:00-17:59",
            TimePeriod.Evening => "18:00-23:59",
            _ => throw new ArgumentOutOfRangeException(nameof(period), $"Not expected period value: {period}")
        };
        return By.XPath($"//select[@id='fltr-dhours']/option[@value='{value}']");
    }
}