using HugAutomation.Resources.Utilities;
using OpenQA.Selenium;

namespace HugAutomation.Resources.Pages;

public class SearchResultPage
{
    private static readonly By NotFound = By.Id("search-tr");
    
    public static SearchResultPage Instance { get; } = new();

    public bool IsSomethingFound()
    {
        ExtentReportHolder.LogMessage("Checking if there are any results found...");
        try { return !Selenium.Instance.Driver!.FindElement(NotFound).Displayed; }
        catch (NoSuchElementException) { return true; }
    }
}