using System.ComponentModel.DataAnnotations;

namespace GetThingsDoneAPI.Models.RequestModels
{
    public class NewProjectRequest
    {
        [Required]
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
