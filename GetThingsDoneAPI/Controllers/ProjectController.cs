using GetThingsDoneAPI.Models;
using GetThingsDoneAPI.Models.RequestModels;
using GetThingsDoneAPI.Services.ConcreteServices.ProjectService;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GetThingsDoneAPI.Controllers
{
    
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("/api/projects")]
        public ActionResult GetProjects()
        {
            var projects = _projectService.GetEntities()
                .OrderByDescending(p => p.ProjectId);

            return Ok(projects);
        }

        [HttpGet("/api/projects/{id}")]
        public ActionResult GetSingleProject(int id)
        {
            var project = _projectService.GetSingleEntity(id);
            
            if (project == null) 
                return NotFound($"Project with id={id} doesn't exist.");
            return Ok(project);
        }
        
        [HttpPost("/api/projects")]
        public ActionResult AddProject(
            [FromBody] NewProjectRequest projectRequest)
        {
            var project = new Project
            {
                ProjectName = projectRequest.Name,
                ProjectCode = projectRequest.Code,
                ProjectActive = true
            };

            _projectService.AddEntity(project);
            return Created(new Uri(
                $"{Request.GetEncodedUrl()}/{project.ProjectId}"), project);
        }

        [HttpDelete("/api/projects/{id}")]
        public ActionResult DeleteProject(int id)
        {
            if (_projectService.DeleteEntity(id) == 404) 
                return NotFound($"Project with id={id} doesn't exist.");

            return Ok("Project deleted.");
        }

        [HttpPut("/api/projects/{id}")]
        public ActionResult EditProject(
            int id, [FromBody] EditProjectRequest projectRequest)
        {
            var project = new Project
            {
                ProjectId = id,
                ProjectName = projectRequest.Name,
                ProjectCode = projectRequest.Code,
                ProjectActive = projectRequest.Active
            };

            if (_projectService.EditEntity(id, project) == 404) 
                return NotFound($"Project with id={id} doesn't exist.");
            return Ok("Project edited.");
        }

        [HttpGet("/api/projects/{id}/issues")]
        public ActionResult GetIssuesForProject(int id)
        {
            var issues = _projectService.GetChildIssues(id)
                ?.OrderByDescending(i => i.IssueId);

            if (issues == null) 
                return NotFound($"Project with id={id} doesn't exist.");
            return Ok(issues);
        }

    }
}
