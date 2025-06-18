using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TestAppAPI;

namespace TestApp.Tests
{
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
        public void Constructor_WithNullRepository_ThrowsArgumentNullException()
        {
            Action act = () => new StudyGroupController(null);
            act.Should().Throw<ArgumentNullException>();
        }

        private static StudyGroup CreateValidStudyGroupWithUser(int groupId = 1, string groupName = "TestGroup",
            Subject subject = Subject.Math, int userId = 1, string userName = "John Doe") =>
            new(groupId, groupName, subject, DateTime.Now, [new User { Id = userId, Name = userName }]);
    }
}