using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GetThingsDoneAPI.Models
{
    public class TimeEntry
    {
        public int TimeEntryId { get; set; }
        
        [Required]
        public DateOnly TimeEntryDate { get; set; }
        
        [Required]
        public int TimeEntryHours { get; set; }
        
        public string TimeEntryDescription { get; set; } = string.Empty;
        
        [Required]
        [ForeignKey("IssueId")]
        public int IssueId { get; set; }
        public Issue Issue { get; set; }
    }
}
