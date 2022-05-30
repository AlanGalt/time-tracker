using GetThingsDoneAPI.Data;

namespace GetThingsDoneAPI.Services
{
    public class GenericDbService<T> : IGenericDbService<T> where T : class
    {
        private readonly GetThingsDoneDbContext _db;
        public GenericDbService(GetThingsDoneDbContext db)
        {
            _db = db;
        }

        public List<T> GetEntities()
        {
            return _db.Set<T>().ToList();
        }

        public T GetSingleEntity(int Id)
        {
            return _db.Set<T>().Find(Id);
        }

        public int AddEntity(T entity)
        {
            _db.Set<T>().Add(entity);
            _db.SaveChanges();
            return 201;
        }

        public int DeleteEntity(int Id)
        {
            var toDelete = _db.Set<T>().Find(Id);

            if (toDelete == null) return 404;

            _db.Remove(toDelete);
            _db.SaveChanges();
            return 200;
        }

        public int EditEntity(int Id, T entity)
        {
            var toUpdate = _db.Set<T>().Find(Id);

            if (toUpdate == null) return 404;

            _db.Entry(toUpdate).CurrentValues.SetValues(entity);
            _db.SaveChanges();
            return 200;
        }
    }
}
