namespace GetThingsDoneAPI.Services
{
    public interface IGenericDbService<T> where T : class
    {
        public List<T> GetEntities();
        public T GetSingleEntity(int Id);
        public int AddEntity(T entity);
        public int DeleteEntity(int Id);
        public int EditEntity(int Id, T enitity);
    }
}
