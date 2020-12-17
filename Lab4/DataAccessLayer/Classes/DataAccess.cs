using Models;

namespace DataAccessLayer
{
    public class DataAccess : IDataAccess
    {
        private readonly DataBaseHandler worker;
        private ProductsStroage storage;
        public DataAccess(string connectionString, string storedProcedure)
        {
            worker = new DataBaseHandler(connectionString, storedProcedure);
        }
        public IStorage<Products> Products
        {
            get
            {
                if (storage == null)
                {
                    storage = new ProductsStroage(worker);
                }
                return storage;
            }
        }
    }

}