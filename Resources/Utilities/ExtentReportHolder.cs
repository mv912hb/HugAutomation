﻿using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace HugAutomation.Resources.Utilities;

public static class ExtentReportHolder
{
    private const string ReportDirectory = @"C:\MES\HugResults";
    private static ExtentReports? _extent;
    private static ExtentTest? _currentTest;
    private static string? _reportPath;

    public static void InitializeReport()
    {
        var suiteName = TestContext.CurrentContext.Test.DisplayName;
        var timestamp = DateTime.Now.ToString("yyyyMMdd");
        var reportFileName = $"{suiteName}_TestReport_{timestamp}.html";

        _reportPath = Path.Combine(ReportDirectory, reportFileName);

        if (!Directory.Exists(ReportDirectory)) Directory.CreateDirectory(ReportDirectory);
        if (File.Exists(_reportPath)) File.Delete(_reportPath);

        var sparkReporter = new ExtentSparkReporter(_reportPath)
        {
            Config =
            {
                Theme = Theme.Dark,
                ReportName = $"{suiteName} Automation Test Run"
            }
        };
        _extent = new ExtentReports();
        _extent.AttachReporter(sparkReporter);
    }

    public static void SetCurrentTest(ExtentTest? test) => _currentTest = test;

    public static void LogMessage(string message)
    {
        Console.WriteLine(message);
        _currentTest?.Info(message);
    }

    public static ExtentTest? CreateTest(string testName)
    {
        var test = _extent!.CreateTest(testName);
        LogTestDescription(test);

        return test;
    }

    private static void LogTestDescription(ExtentTest? test)
    {
        var description = TestContext.CurrentContext.Test.Properties.Get("Description") as string;

        if (!string.IsNullOrEmpty(description)) test?.Info($"Description: {description}");
    }

    public static void FlushReport() => _extent!.Flush();

    public static void LogTestResult(ExtentTest? test, TestStatus status, string testName,
        string message = "",
        string? stackTrace = "")
    {
        var actions = new Dictionary<TestStatus, Action>
        {
            {
                TestStatus.Failed, () =>
                {
                    var screenshotPath = TakeScreenshot(testName);
                    test?.Fail(message)
                        .Fail(stackTrace)
                        .Fail(MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
                }
            },
            { TestStatus.Passed, () => test?.Pass(message) },
            { TestStatus.Skipped, () => test?.Skip("Test skipped") }
        };

        actions.GetValueOrDefault(status, () => { })();
    }

    private static string TakeScreenshot(string testName)
    {
        var driver = Selenium.Instance.Driver;

        var screenshotDirectory = Path.Combine(ReportDirectory, "Screenshots");
        if (!Directory.Exists(screenshotDirectory)) Directory.CreateDirectory(screenshotDirectory);

        var screenshotFilePath = Path.Combine(screenshotDirectory, $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");

        if (driver is not ITakesScreenshot takesScreenshot)
            throw new InvalidOperationException("Driver instance does not support taking screenshots.");
        var screenshot = takesScreenshot.GetScreenshot();
        screenshot.SaveAsFile(screenshotFilePath);
        return screenshotFilePath;
    }
}