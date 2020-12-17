using System.Collections.Generic;

namespace DataAccessLayer
{
    public interface IStorage<T> where T : new()
    {
        public List<T> GetAllObj();
        public List<T> GetOneObj(int id);
    }
}
