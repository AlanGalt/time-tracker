using System.ComponentModel.DataAnnotations;

namespace GetThingsDoneAPI.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        
        [Required]
        public string ProjectName { get; set; } = string.Empty;
        
        public string ProjectCode { get; set; } = string.Empty;
        public bool ProjectActive { get; set; }
    }
}
