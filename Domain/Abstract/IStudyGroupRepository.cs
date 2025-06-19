namespace TestAppAPI;

using TestApp;

public interface IStudyGroupRepository
{
    Task CreateStudyGroup(StudyGroup studyGroup);
    Task<object?> GetStudyGroups();
    Task<object?> SearchStudyGroups(string subject);
    Task JoinStudyGroup(int studyGroupId, int userId);
    Task LeaveStudyGroup(int studyGroupId, int userId);
}