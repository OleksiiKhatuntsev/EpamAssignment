using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TestApp;

namespace TestAppAPI.ComponentTests
{
    [TestFixture]
    public class StudyGroupComponentTests
    {
        private Mock<IStudyGroupRepository> _repositoryMock;
        private StudyGroupController _controller;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IStudyGroupRepository>();
            _controller = new StudyGroupController(_repositoryMock.Object);
        }

        [Test]
        public async Task SearchStudyGroups_WithValidSubject_ReturnsMatchingGroups()
        {
            var subject = "Math";
            var expected = new List<StudyGroup>
            {
                new StudyGroup(1, "Math Legends", Subject.Math, DateTime.UtcNow, [])
            };
            _repositoryMock.Setup(r => r.SearchStudyGroups(subject)).ReturnsAsync(expected);

            var result = await _controller.SearchStudyGroups(subject);

            result.Should().BeOfType<OkObjectResult>();
            var returned = (result as OkObjectResult)?.Value as List<StudyGroup>;
            returned.Should().NotBeNull().And.HaveCount(1);
            returned[0].Name.Should().Be("Math Legends");
        }

        [Test]
        public async Task JoinStudyGroup_WhenUserAlreadyInOtherMathGroup_ShouldThrowInvalidOperationException()
        {
            var userId = 42;
            var existingMathGroup = new StudyGroup(1, "Algebra Masters", Subject.Math, DateTime.UtcNow, new List<User>
            {
                new User { Id = 42, Name = "Mark" }
            });
            var newMathGroupId = 2;

            _repositoryMock.Setup(r => r.GetStudyGroups()).ReturnsAsync(new List<StudyGroup> { existingMathGroup });
            _repositoryMock.Setup(r => r.JoinStudyGroup(newMathGroupId, userId))
                .ThrowsAsync(new InvalidOperationException("User already in a Math group"));

            Func<Task> act = async () => await _controller.JoinStudyGroup(newMathGroupId, userId);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("User already in a Math group");
        }
    }
}