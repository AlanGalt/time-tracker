using GetThingsDoneAPI.Models;

namespace GetThingsDoneAPI.Services.ConcreteServices.IssueService
{
    public interface IIssueService : IGenericDbService<Issue>
    {
        public new int AddEntity(Issue issue);
        public List<Issue> GetEntities(bool? activeOnly);
        public List<TimeEntry> GetChildEntries(int issueId);
    }
}
