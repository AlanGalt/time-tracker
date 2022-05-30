using System.ComponentModel.DataAnnotations;

namespace GetThingsDoneAPI.Models.RequestModels
{
    public class NewIssueRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int ProjectId { get; set; }
    }
}
