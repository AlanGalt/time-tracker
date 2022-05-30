using GetThingsDoneAPI.Data;
using GetThingsDoneAPI.Models;

namespace GetThingsDoneAPI.Services.ConcreteServices.TimeEntryService
{
    public class TimeEntryService : GenericDbService<TimeEntry>, ITimeEntryService
    {
        private readonly GetThingsDoneDbContext _db;
        public TimeEntryService(GetThingsDoneDbContext db) : base(db)
        {
            _db = db;
        }

        public List<TimeEntry> GetEntities(int? month, DateOnly? date)
        {
            var entries = _db.TimeEntries;
            

            if (month == null && date == null) return entries.ToList();
            
            if (month != null && date == null)
                return entries
                    .Where(e => e.TimeEntryDate.Month == month)
                    .ToList();

            var cur = DateOnly.Parse(date.ToString());
            return entries
                .Where(e => e.TimeEntryDate.Day == cur.Day &&
                            e.TimeEntryDate.Month == cur.Month &&
                            e.TimeEntryDate.Year == cur.Year)
                .ToList();

        }

        public new int AddEntity(TimeEntry entry)
        {
            if (_db.Issues.Find(entry.IssueId) == null) return 404;

            var hoursToday = _db.TimeEntries
                .Where(e => e.TimeEntryDate == DateOnly.FromDateTime(DateTime.Now))
                .ToList()
                .Aggregate(0, (acc, x) => acc + x.TimeEntryHours);

            if (hoursToday + entry.TimeEntryHours > 24) return 422;
            
            base.AddEntity(entry);
            
            return 201;
        }
    }
}
