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

        public List<TimeEntry> GetEntities(DateOnly? date, bool? monthOnly)
        {
            var entries = _db.TimeEntries;

            if (date == null) return entries.ToList();

            var cur = DateOnly.Parse(date.ToString());

            var byMonth = entries
                            .Where(e => e.TimeEntryDate.Month == cur.Month &&
                                        e.TimeEntryDate.Year == cur.Year);

            if (monthOnly == true) return byMonth.ToList();

            return byMonth
                .Where(e => e.TimeEntryDate.Day == cur.Day)
                .ToList();

        }

        public new int AddEntity(TimeEntry entry)
        {
            if (_db.Issues.Find(entry.IssueId) == null) return 404;

            var hoursTotal = _db.TimeEntries
                .Where(e => e.TimeEntryDate == entry.TimeEntryDate)
                .ToList()
                .Aggregate(0, (acc, x) => acc + x.TimeEntryHours);

            if (hoursTotal + entry.TimeEntryHours > 24) return 422;
            
            base.AddEntity(entry);
            
            return 201;
        }

        public new int EditEntity(int id, TimeEntry entry)
        {
            var toUpdate = _db.TimeEntries.Find(id);

            if (toUpdate == null) return 404;

            var hoursTotal = _db.TimeEntries
                .Where(e => 
                    e.TimeEntryId != entry.TimeEntryId && 
                    e.TimeEntryDate == entry.TimeEntryDate)
                .ToList()
                .Aggregate(0, (acc, x) => acc + x.TimeEntryHours);

            if (hoursTotal + entry.TimeEntryHours > 24) return 422;


            _db.Entry(toUpdate).CurrentValues.SetValues(entry);
            _db.SaveChanges();
            return 200;
        }
    }
}
