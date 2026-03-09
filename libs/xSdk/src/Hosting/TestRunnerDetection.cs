namespace xSdk.Hosting;

public static class TestRunnerDetection
{
    private static bool? _areUnitTestsRunning;

    public static bool AreUnitTestsRunning => _areUnitTestsRunning ??= IsTestRunnerFound();

    private static bool IsTestRunnerFound()
    {
        const string testRunnerPrefix = "xunit.runner";
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var runnerFound = assemblies.Any(x => x.FullName.StartsWith(testRunnerPrefix, StringComparison.Ordinal));
        return runnerFound;
    }
}
