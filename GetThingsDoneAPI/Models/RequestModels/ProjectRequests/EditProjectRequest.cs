using System.ComponentModel.DataAnnotations;

namespace GetThingsDoneAPI.Models.RequestModels
{
    public class EditProjectRequest : NewProjectRequest
    {
        [Required]
        public bool Active { get; set; }
    }
}
