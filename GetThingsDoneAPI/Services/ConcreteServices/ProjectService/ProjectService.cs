using GetThingsDoneAPI.Data;
using GetThingsDoneAPI.Models;

namespace GetThingsDoneAPI.Services.ConcreteServices.ProjectService
{
    public class ProjectService : GenericDbService<Project>, IProjectService
    {
        private readonly GetThingsDoneDbContext _db;
        public ProjectService(GetThingsDoneDbContext db) : base(db)
        {
            _db = db;
        }

        public List<Issue> GetChildIssues(int projectId)
        { 
            var parent = _db.Projects.Find(projectId);
            if (parent == null) return null;
            
            return _db.Issues.Where(t => t.ProjectId == projectId).ToList();
        }
    }
}