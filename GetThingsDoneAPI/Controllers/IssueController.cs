using GetThingsDoneAPI.Models;
using GetThingsDoneAPI.Models.RequestModels;
using GetThingsDoneAPI.Services.ConcreteServices.IssueService;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GetThingsDoneAPI.Controllers
{

    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly IIssueService _issueService;
        public IssueController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpGet("/api/issues")]
        public ActionResult GetIssues([FromQuery] bool? activeOnly)
        {
            var issues = _issueService.GetEntities(activeOnly);
            return Ok(issues);
        }

        [HttpGet("/api/issues/{id}")]
        public ActionResult GetSingleIssue(int id)
        {
            var issue = _issueService.GetSingleEntity(id);

            if (issue == null) 
                return NotFound($"Issue with id={id} doesn't exist.");
            return Ok(issue);
        }

        [HttpPost("/api/issues")]
        public ActionResult AddIssue([FromBody] NewIssueRequest issueRequest)
        {
            var issue = new Issue
            {
                IssueName = issueRequest.Name,
                ProjectId = issueRequest.ProjectId,
                IssueActive = true
            };

            if (_issueService.AddEntity(issue) == 404)
                return NotFound($"Project with id={issue.ProjectId} doesn't exist.");

            return Created(new Uri(
               $"{Request.GetEncodedUrl()}/{issue.IssueId}"), issue);
        }

        [HttpDelete("/api/issues/{id}")]
        public ActionResult DeleteIssue(int id)
        {
            if (_issueService.DeleteEntity(id) == 404) 
                return NotFound($"Issue with id={id} doesn't exist.");
            return Ok("Issue deleted.");
        }

        [HttpPut("/api/issues/{id}")]
        public ActionResult EditIssue(
            int id, [FromBody] EditIssueRequest issueRequest)
        {
            var issue = new Issue
            {
                IssueId = id,
                IssueName = issueRequest.Name,
                ProjectId = issueRequest.ProjectId,
                IssueActive = issueRequest.Active
            };

            if (_issueService.EditEntity(id, issue) == 404) 
                return NotFound($"Issue with {id} doesn't exist.");
            return Ok("Issue edited.");
        }

        [HttpGet("/api/issues/{id}/time-entries")]
        public ActionResult GetEntriesForIssue(int id)
        {
            var entries = _issueService.GetChildEntries(id);
            if (entries == null)
                return NotFound($"Issue with id={id} doesn't exist.");
            return Ok(entries);
        }
    }
}
