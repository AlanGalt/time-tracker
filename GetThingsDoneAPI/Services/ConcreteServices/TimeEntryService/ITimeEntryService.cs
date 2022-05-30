using GetThingsDoneAPI.Models;

namespace GetThingsDoneAPI.Services.ConcreteServices.TimeEntryService
{
    public interface ITimeEntryService : IGenericDbService<TimeEntry>
    {
        public List<TimeEntry> GetEntities(int? month, DateOnly? date);
        public new int AddEntity(TimeEntry entry);
    }
}
