namespace TestApp.Tests;

using Attributes;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TestAppAPI;

[TestFixture]
public class StudyGroupComponentTests
{
    private Mock<IStudyGroupRepository> _repositoryMock;
    private StudyGroupController _controller;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IStudyGroupRepository>();
        _controller = new StudyGroupController(_repositoryMock.Object);
    }

    [Test]
    [TestCaseForTestRail("001", TestCaseType.Component, [TestCategory.Smoke])]
    public async Task CreateStudyGroup_WithValidStudyGroup_CallRepositoryAndReturnOk()
    {
        // Arrange
        var studyGroup = CreateValidStudyGroupWithUser();

        // Act
        var result = await _controller.CreateStudyGroup(studyGroup);

        // Assert
        result.Should().BeOfType<OkResult>();
        _repositoryMock.Verify(r => r.CreateStudyGroup(studyGroup), Times.Once);
    }

    [Test]
    [TestCaseForTestRail("002", TestCaseType.Component, [TestCategory.Smoke])]
    public async Task GetStudyGroups_WithValidStudyGroups_ReturnListOfStudyGroups()
    {
        // Arrange
        var groups = new List<StudyGroup>
            { CreateValidStudyGroupWithUser(), CreateValidStudyGroupWithUser(subject: Subject.Chemistry) };
        _repositoryMock.Setup(r => r.GetStudyGroups()).ReturnsAsync(groups);

        // Act
        var result = await _controller.GetStudyGroups();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(groups);
    }

    [Test]
    [TestCaseForTestRail("003", TestCaseType.Component, [TestCategory.Smoke])]
    public async Task SearchStudyGroups_WithValidGroups_ReturnMatchingGroups()
    {
        // Arrange
        var subject = "Math";
        var matchedGroups = new List<StudyGroup> { CreateValidStudyGroupWithUser() };
        _repositoryMock.Setup(r => r.SearchStudyGroups(subject)).ReturnsAsync(matchedGroups);

        // Act
        var result = await _controller.SearchStudyGroups(subject);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(matchedGroups);
    }

    [Test]
    [TestCaseForTestRail("004", TestCaseType.Component, [TestCategory.Smoke])]
    public async Task JoinStudyGroup_WithValidGroup_CallRepositoryAndReturnOk()
    {
        // Arrange
        int groupId = 1, userId = 42;

        // Act
        var result = await _controller.JoinStudyGroup(groupId, userId);

        // Assert
        result.Should().BeOfType<OkResult>();
        _repositoryMock.Verify(r => r.JoinStudyGroup(groupId, userId), Times.Once);
    }

    [Test]
    [TestCaseForTestRail("005", TestCaseType.Component, [TestCategory.Smoke])]
    public async Task LeaveStudyGroup_WithValidGroupAndUser_CallRepositoryAndReturnOk()
    {
        // Arrange
        int groupId = 1, userId = 42;

        // Act
        var result = await _controller.LeaveStudyGroup(groupId, userId);

        // Assert
        result.Should().BeOfType<OkResult>();
        _repositoryMock.Verify(r => r.LeaveStudyGroup(groupId, userId), Times.Once);
    }
        
    [Test]
    [TestCaseForTestRail("006", TestCaseType.Component)]
    public void Constructor_WithNullRepository_ThrowsArgumentNullException()
    {
        Action act = () => new StudyGroupController(null);
        act.Should().Throw<ArgumentNullException>();
    }

    private static StudyGroup CreateValidStudyGroupWithUser(int groupId = 1, string groupName = "TestGroup",
        Subject subject = Subject.Math, int userId = 1, string userName = "John Doe") =>
        new(groupId, groupName, subject, DateTime.Now, [new User { Id = userId, Name = userName }]);
}