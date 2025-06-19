namespace Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TestCaseForTestRail : Attribute
{
    public string TestCaseNumber { get; }

    public TestCaseType Type { get; }

    public TestCategory[] Categories { get; }

    public string Description { get; set; }
    
    public TestCaseForTestRail(string testCaseNumber, TestCaseType type, TestCategory[] categories = null,
        string description = "")
    {
        if (string.IsNullOrWhiteSpace(testCaseNumber))
            throw new ArgumentException("Test case number cannot be null or empty", nameof(testCaseNumber));

        TestCaseNumber = testCaseNumber;
        Type = type;
        Categories = categories ?? [TestCategory.Smoke, TestCategory.Regression];
    }
}