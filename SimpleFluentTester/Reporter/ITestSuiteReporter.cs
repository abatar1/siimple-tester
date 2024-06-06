using System;
using SimpleFluentTester.TestSuite;

namespace SimpleFluentTester.Reporter;

public interface ITestSuiteReporter
{
    /// <summary>
    /// Prints the results of test execution using the standard reporter or, if specified, using a custom reporter.
    /// </summary>
    void Report(Action<ITestSuiteReporterConfigurationBuilder, TestSuiteRunResult>? configurationBuilder = null);
    
    /// <summary>
    /// Represents test suite run results.
    /// </summary>
    TestSuiteRunResult TestSuiteRunResult { get; }
}