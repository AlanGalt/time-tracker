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

        public T GetSingleEntity(int id)
        {
            return _db.Set<T>().Find(id);
        }

        public int AddEntity(T entity)
        {
            _db.Set<T>().Add(entity);
            _db.SaveChanges();
            return 201;
        }

        public int DeleteEntity(int id)
        {
            var toDelete = _db.Set<T>().Find(id);

            if (toDelete == null) return 404;

            _db.Remove(toDelete);
            _db.SaveChanges();
            return 200;
        }

        public int EditEntity(int id, T entity)
        {
            var toUpdate = _db.Set<T>().Find(id);

            if (toUpdate == null) return 404;

            _db.Entry(toUpdate).CurrentValues.SetValues(entity);
            _db.SaveChanges();
            return 200;
        }
    }
}
