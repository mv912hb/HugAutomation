using AventStack.ExtentReports;
using HugAutomation.Resources;
using HugAutomation.Resources.Utilities;
using NUnit.Framework;

namespace HugAutomation.Tests;

public class BaseClass : CommonConstants
{
    private ExtentTest? _test;

    [OneTimeSetUp]
    public void BeforeSuite()
    {
        ExtentReportHolder.InitializeReport();
        Selenium.Instance.OpenBrowser();
    }

    [SetUp]
    public void BeforeMethod()
    {
        var testName = TestContext.CurrentContext.Test.Name;

        _test = ExtentReportHolder.CreateTest(testName);
        ExtentReportHolder.SetCurrentTest(_test);
    }

    [TearDown]
    public void AfterMethod()
    {
        var status = TestContext.CurrentContext.Result.Outcome.Status;
        var stackTrace = TestContext.CurrentContext.Result.StackTrace;
        var errorMessage = TestContext.CurrentContext.Result.Message;

        ExtentReportHolder.LogTestResult(_test, status, TestContext.CurrentContext.Test.Name, errorMessage, stackTrace);
        ExtentReportHolder.FlushReport();
    }

    [OneTimeTearDown]
    public void AfterSuite() => Selenium.Instance.CloseBrowser();
}