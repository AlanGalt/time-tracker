using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GetThingsDoneAPI.Models
{
    public class Issue
    {
        public int IssueId { get; set; }
        
        [Required]
        public string IssueName { get; set; } = string.Empty;

        [Required]
        [ForeignKey("ProjectId")]
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public bool IssueActive { get; set; }
    }
}
