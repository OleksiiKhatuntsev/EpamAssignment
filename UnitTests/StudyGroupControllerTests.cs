namespace TestApp.Tests;

using Attributes;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TestAppAPI;

[TestFixture]
public class StudyGroupControllerTests
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
    [TestCaseForTestRail("008", TestCaseType.Unit)]
    public async Task CreateStudyGroup_WithValidStudyGroup_ReturnsOkResult()
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
    [TestCaseForTestRail("009", TestCaseType.Unit)]
    public async Task GetStudyGroups_WhenCalled_ReturnsOkObjectResultWithGroups()
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
    [TestCaseForTestRail("010", TestCaseType.Unit)]
    public async Task CreateStudyGroup_WhenRepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var expectedExceptionMessage = "Duplicate subject";
        var studyGroup = CreateValidStudyGroupWithUser();
        _repositoryMock.Setup(r => r.CreateStudyGroup(It.IsAny<StudyGroup>()))
            .ThrowsAsync(new InvalidOperationException(expectedExceptionMessage));

        // Act & Assert
        var act = async () => await _controller.CreateStudyGroup(studyGroup);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Test]
    [TestCaseForTestRail("011", TestCaseType.Unit)]
    public async Task GetStudyGroups_WhenRepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var expectedExceptionMessage = "Database error";
        _repositoryMock.Setup(r => r.GetStudyGroups())
            .ThrowsAsync(new InvalidOperationException(expectedExceptionMessage));

        // Act & Assert
        var act = async () => await _controller.GetStudyGroups();
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Test]
    [TestCaseForTestRail("012", TestCaseType.Unit)]
    public async Task JoinStudyGroup_WhenRepositoryThrowsNotFoundException_PropagatesException()
    {
        // Arrange
        var expectedExceptionMessage = "Study group not found";
        int groupId = 1, userId = 42;
        _repositoryMock.Setup(r => r.JoinStudyGroup(groupId, userId))
            .ThrowsAsync(new ArgumentException(expectedExceptionMessage));

        // Act & Assert
        var act = async () => await _controller.JoinStudyGroup(groupId, userId);
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Test]
    [TestCaseForTestRail("013", TestCaseType.Unit)]
    public async Task CreateStudyGroup_WithNullStudyGroup_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.CreateStudyGroup(null);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
        
        _repositoryMock.Verify(r => r.CreateStudyGroup(null), Times.Once);
    }

    [Test]
    [TestCaseForTestRail("014", TestCaseType.Unit)]
    public async Task JoinStudyGroup_WithInvalidIds_ShouldValidateInput()
    {
        // Act
        var result = await _controller.JoinStudyGroup(-1, -1);

        // Assert
        result.Should().BeOfType<BadRequestResult>();

        _repositoryMock.Verify(r => r.JoinStudyGroup(-1, -1), Times.Once);
    }
    
    [Test]
    [TestCaseForTestRail("015", TestCaseType.Unit)]
    public void Constructor_WithNullRepository_ThrowsArgumentNullException()
    {
        // Act & Assert
        var act = () => new StudyGroupController(null);
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("studyGroupRepository");
    }

    [Test]
    [TestCaseForTestRail("016", TestCaseType.Unit)]
    public async Task SearchStudyGroups_WithEmptySubject_CallsRepositoryWithEmptyString()
    {
        // Arrange
        var emptyGroups = new List<StudyGroup>();
        _repositoryMock.Setup(r => r.SearchStudyGroups("")).ReturnsAsync(emptyGroups);

        // Act
        var result = await _controller.SearchStudyGroups("");

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _repositoryMock.Verify(r => r.SearchStudyGroups(""), Times.Once);
    }

    [Test]
    [TestCaseForTestRail("017", TestCaseType.Unit)]
    public async Task GetStudyGroups_WhenNoGroupsExist_ReturnsEmptyList()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetStudyGroups()).ReturnsAsync(new List<StudyGroup>());

        // Act
        var result = await _controller.GetStudyGroups();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var groups = okResult.Value as List<StudyGroup>;
        groups.Should().BeEmpty();
    }
    
    private static StudyGroup CreateValidStudyGroupWithUser(
        int groupId = 1, 
        string groupName = "TestGroup",
        Subject subject = Subject.Math,
        int userId = 1,
        string userName = "John Doe") =>
        new(
                studyGroupId: groupId,
                name: groupName,
                subject: subject,
                createDate: DateTime.Now,
                users: [new User { Id = userId, Name = userName }]
            );
}