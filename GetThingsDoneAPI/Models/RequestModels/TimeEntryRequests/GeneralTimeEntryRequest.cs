using System.ComponentModel.DataAnnotations;

namespace GetThingsDoneAPI.Models.RequestModels
{
    public class GeneralTimeEntryRequest
    {
        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public int Hours { get; set; }

        public string Description { get; set; }

        [Required]
        public int IssueId { get; set; }
    }
}