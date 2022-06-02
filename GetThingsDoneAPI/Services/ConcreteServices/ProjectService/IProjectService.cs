using GetThingsDoneAPI.Models;

namespace GetThingsDoneAPI.Services.ConcreteServices.ProjectService
{
    public interface IProjectService : IGenericDbService<Project>
    {
        public List<Issue> GetChildIssues(int projectId);
    }
}
