namespace GetThingsDoneAPI.Services
{
    public interface IGenericDbService<T> where T : class
    {
        public List<T> GetEntities();
        public T GetSingleEntity(int id);
        public int AddEntity(T entity);
        public int DeleteEntity(int id);
        public int EditEntity(int id, T enitity);
    }
}
