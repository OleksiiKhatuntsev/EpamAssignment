namespace TestAppAPI;

using Microsoft.AspNetCore.Mvc;
using TestApp;

public class StudyGroupController
{
    private readonly IStudyGroupRepository _studyGroupRepository;

    public StudyGroupController(IStudyGroupRepository studyGroupRepository)
    {
        _studyGroupRepository = studyGroupRepository ?? throw new ArgumentNullException(nameof(studyGroupRepository));
    }

    public async Task<IActionResult> CreateStudyGroup(StudyGroup studyGroup)
    {
        if (studyGroup == null)
        {
            return new BadRequestResult();
        }
        await _studyGroupRepository.CreateStudyGroup(studyGroup);
        return new OkResult();
    }

    public async Task<IActionResult> GetStudyGroups()
    {
        var studyGroups = await _studyGroupRepository.GetStudyGroups();
        return new OkObjectResult(studyGroups);
    }

    public async Task<IActionResult> SearchStudyGroups(string subject)
    {
        var studyGroups = await _studyGroupRepository.SearchStudyGroups(subject);
        return new OkObjectResult(studyGroups);
    }

    public async Task<IActionResult> JoinStudyGroup(int studyGroupId, int userId)
    {
        // we need to add a good verification here, but for an assignment purpose it's ok
        if (studyGroupId < 1 || userId < 1)
        {
            return new BadRequestResult();
        }
        await _studyGroupRepository.JoinStudyGroup(studyGroupId, userId);
        return new OkResult();
    }

    public async Task<IActionResult> LeaveStudyGroup(int studyGroupId, int userId)
    {
        await _studyGroupRepository.LeaveStudyGroup(studyGroupId, userId);
        return new OkResult();
    }
}