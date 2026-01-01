namespace xSdk.Hosting
{
    public static class TestRunnerDetection
    {
        private static bool? areUnitTestsRunning;

        public static bool AreUnitTestsRunning => areUnitTestsRunning ??= IsTestRunnerFound();

        private static bool IsTestRunnerFound()
        {
            const string testRunnerPrefix = "xunit.runner";
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var runnerFound = assemblies.Any(x => x.FullName.StartsWith(testRunnerPrefix, StringComparison.Ordinal));
            return runnerFound;
        }
    }
}
