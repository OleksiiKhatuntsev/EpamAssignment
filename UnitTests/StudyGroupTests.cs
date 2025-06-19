namespace TestApp.Tests
{
    using FluentAssertions;
    using TestAssignmentEpam;

    [TestFixture]
    public class StudyGroupTests
    {
        [Test]
        public void Constructor_WithValidParameters_CreatesStudyGroup()
        {
            // Arrange
            var studyGroupId = 1;
            var name = "Advanced Math Group";
            var subject = Subject.Math;
            var createDate = DateTime.Now;
            var users = new List<User> { new() { Id = 1, Name = "Oleksii" } };

            // Act
            var studyGroup = new StudyGroup(studyGroupId, name, subject, createDate, users);

            // Assert
            studyGroup.StudyGroupId.Should().Be(studyGroupId);
            studyGroup.Name.Should().Be(name);
            studyGroup.Subject.Should().Be(subject);
            studyGroup.CreateDate.Should().Be(createDate);
            studyGroup.Users.Count.Should().Be(users.Count);
            studyGroup.Users.Should().Contain(users);
        }

        [Test]
        public void Constructor_WithMinimumValidNameLength_CreatesStudyGroup()
        {
            // Arrange
            var name = "Study"; // 5 characters - minimum valid
            var subject = Subject.Physics;
            var createDate = DateTime.Now;
            var users = new List<User>();

            // Act & Assert
            Assert.DoesNotThrow(() => new StudyGroup(1, name, subject, createDate, users));
        }

        [Test]
        public void Constructor_WithMaximumValidNameLength_CreatesStudyGroup()
        {
            // Arrange
            var name = "Advanced Mathematics Study Gr"; // 30 characters - maximum valid
            var subject = Subject.Chemistry;
            var createDate = DateTime.Now;
            var users = new List<User>();

            // Act & Assert
            Assert.DoesNotThrow(() => new StudyGroup(1, name, subject, createDate, users));
        }

        [Test]
        public void Constructor_WithNameBelowMinimumLength_ThrowsException()
        {
            // Arrange
            var name = "Math"; // 4 characters - below minimum
            var subject = Subject.Math;
            var createDate = DateTime.Now;
            var users = new List<User>();

            Action act = () => new StudyGroup(1, name, subject, createDate, users);

            // Act & Assert
            act.Should().Throw<WrongStudyGroupException>();
        }

        [Test]
        public void Constructor_WithNameAboveMaximumLength_ThrowsException()
        {
            // Arrange
            var name = "Advanced Mathematics Study Group"; // 31 characters - above maximum
            var subject = Subject.Math;
            var createDate = DateTime.Now;
            var users = new List<User>();

            Action act = () => new StudyGroup(1, name, subject, createDate, users);

            // Act & Assert
            act.Should().Throw<WrongStudyGroupException>();
        }

        [Test]
        public void AddUser_WithValidUser_AddsUserToList()
        {
            // Arrange
            var studyGroup = CreateValidStudyEmptyGroup();
            var user = new User { Id = 1, Name = "John Doe" };

            // Act
            studyGroup.AddUser(user);

            // Assert
            studyGroup.Users.Should().Contain(user);
            studyGroup.Users.Count.Should().Be(1);
        }

        [Test]
        public void RemoveUser_WithExistingUser_RemovesUserFromList()
        {
            // Arrange
            var studyGroup = CreateValidStudyGroupWithUser();

            // Act
            studyGroup.RemoveUser(studyGroup.Users.First());

            // Assert
            studyGroup.Users.Count.Should().Be(0);
        }

        private static StudyGroup CreateValidStudyGroupWithUser() =>
            new(1, "Test Group", Subject.Math, DateTime.Now,
                [new User { Id = 1, Name = "John Doe" }]);

        private static StudyGroup CreateValidStudyEmptyGroup() =>
            new(1, "Test Group", Subject.Math, DateTime.Now, []);
    }
}