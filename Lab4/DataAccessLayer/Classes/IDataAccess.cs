using Models;

namespace DataAccessLayer
{
    interface IDataAccess
    {
        IStorage<Products> Products { get; }
    }
}
