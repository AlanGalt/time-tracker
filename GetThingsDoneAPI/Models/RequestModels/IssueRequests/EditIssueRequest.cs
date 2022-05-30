using System.ComponentModel.DataAnnotations;

namespace GetThingsDoneAPI.Models.RequestModels
{
    public class EditIssueRequest : NewIssueRequest
    {
        [Required]
        public bool Active { get; set; }
    }
}
