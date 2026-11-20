namespace laboratorio3.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        int Insert(T entity);
        int Update(T entity);
        int Delete(int id);
    }
}
