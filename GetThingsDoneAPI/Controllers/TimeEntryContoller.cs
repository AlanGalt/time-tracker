using GetThingsDoneAPI.Models;
using GetThingsDoneAPI.Models.RequestModels;
using GetThingsDoneAPI.Services.ConcreteServices.TimeEntryService;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GetThingsDoneAPI.Controllers
{

    [ApiController]
    public class TimeEntryController : ControllerBase
    {
        private readonly ITimeEntryService _timeEntryService;
        public TimeEntryController(ITimeEntryService timeEntryService)
        {
            _timeEntryService = timeEntryService;
        }

        [HttpGet("/api/time-entries")]
        public ActionResult GetTimeEntries(
            [FromQuery] int? month,
            [FromQuery] DateOnly? date)
        {
            var entries = _timeEntryService.GetEntities(month, date);
            return Ok(entries);
        }

        [HttpGet("/api/time-entries/{id}")]
        public ActionResult GetSingleTimeEntry(int id)
        {
            var entry = _timeEntryService.GetSingleEntity(id);

            if (entry == null) 
                return NotFound($"Time entry with id={id} doesn't exist.");
            return Ok(entry);
        }

        [HttpPost("/api/time-entries")]
        public ActionResult AddTimeEntry(
            [FromBody] GeneralTimeEntryRequest timeEntryRequest)
        {
            var entry = new TimeEntry
            {
                TimeEntryDate = timeEntryRequest.Date,
                TimeEntryHours = timeEntryRequest.Hours,
                TimeEntryDescription = timeEntryRequest.Description,
                IssueId = timeEntryRequest.IssueId
            };

            var result = _timeEntryService.AddEntity(entry);

            if (result == 404)
                return NotFound($"Issue with id={entry.IssueId} doesn't exist.");

            if (result == 422) 
                return UnprocessableEntity("24 hour limit exceeded.");

            return Created(new Uri(
                $"{Request.GetEncodedUrl()}/{entry.TimeEntryId}"), entry);
        }

        [HttpDelete("/api/time-entries/{id}")]
        public ActionResult DeleteTimeEntry(int id)
        {
            if (_timeEntryService.DeleteEntity(id) == 404)
                return NotFound($"Time entry with id={id} doesn't exist.");
            return Ok("Issue deleted.");
        }

        [HttpPut("/api/time-entries/{id}")]
        public ActionResult EditTimeEntry(
            int id, [FromBody] GeneralTimeEntryRequest timeEntryRequest)
        {
            var entry = new TimeEntry
            {
                TimeEntryDate = timeEntryRequest.Date,
                TimeEntryHours = timeEntryRequest.Hours,
                TimeEntryDescription = timeEntryRequest.Description,
                IssueId = timeEntryRequest.IssueId
            };

            if (_timeEntryService.EditEntity(id, entry) == 404)
                return NotFound($"Time entry with id={id} doesn't exist.");
            return Ok("Issue edited.");
        }
    }
}
