using System.Collections.Generic;
using System.Linq;
using Models;

namespace DataAccessLayer
{
    class ProductsStroage : IStorage<Products>
    {
        private readonly DataBaseHandler worker;
        public ProductsStroage(DataBaseHandler worker)
        {
            this.worker = worker;
            WriteStorage();
        }
        private void WriteStorage()
        {
            worker.ReadDataBase();
            worker.ListFromDataSet();
        }
        public List<Products> GetAllObj()
        {
            return worker.Products;
        }

        public List<Products> GetOneObj(int id)
        {
            return (List<Products>)worker.Products.Where(product => product.ProductID == id);
        }
    }
}