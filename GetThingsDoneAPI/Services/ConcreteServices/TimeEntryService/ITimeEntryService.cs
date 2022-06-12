using GetThingsDoneAPI.Models;

namespace GetThingsDoneAPI.Services.ConcreteServices.TimeEntryService
{
    public interface ITimeEntryService : IGenericDbService<TimeEntry>
    {
        public List<TimeEntry> GetEntities(DateOnly? date, bool? monthOnly);
        public new int AddEntity(TimeEntry entry);

        public new int EditEntity(int id, TimeEntry entry);
    }
}
