using GetThingsDoneAPI.Data;
using GetThingsDoneAPI.Models;

namespace GetThingsDoneAPI.Services.ConcreteServices.IssueService
{
    public class IssueService : GenericDbService<Issue>, IIssueService
    {
        private readonly GetThingsDoneDbContext _db;
        public IssueService(GetThingsDoneDbContext db) : base(db)
        {
            _db = db;
        }

        public List<Issue> GetEntities(bool? activeOnly)
        {
            var issues = _db.Issues;
            if (activeOnly == true) 
                return issues.Where(i => i.IssueActive).ToList();
            return issues.ToList();
        }

        public new int AddEntity(Issue issue)
        {
            if (_db.Projects.Find(issue.ProjectId) == null) return 404;
            return base.AddEntity(issue);
        }

        public List<TimeEntry> GetChildEntries (int issueId)
        {
            var parent = _db.Issues.Find(issueId);
            if (parent == null) return null;
            
            return _db.TimeEntries.Where(e => e.IssueId == issueId).ToList();
        }
    }
}
