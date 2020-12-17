using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using DataAccessLayer;

namespace ServiceLayer
{
    public class ProductsServiceDBH : IProductsServiceDBH
    {
        public DataAccess dataAccess;
        public ProductsServiceDBH(string connectionstring, string storedprocedure)
        {
            dataAccess = new DataAccess(connectionstring, storedprocedure);
        }
        public List<ProductsToClient> ModelChangeToClient(List<Products> products)
        {
            List<string> productsNames = new List<string>();
            List<ProductsToClient> readyProductsToClient = new List<ProductsToClient>();
            foreach (Products product in products)
            {
                if (!productsNames.Contains(product.ProductName))
                {
                    productsNames.Add(product.ProductName);
                }
            }
            decimal totalPrice;
            Int16 countUnits;
            int countID = 0;
            foreach (string name in productsNames)
            {
                countUnits = 0;
                totalPrice = 0;
                List<Products> itmProduct = new List<Products>();
                itmProduct = products.FindAll(productName => productName.ProductName == name);
                ProductsToClient productsToClient = new ProductsToClient();
                foreach (Products product in itmProduct)
                {
                    countUnits += product.UnitsOnOrder;
                    totalPrice += product.UnitsOnOrder * product.UnitPrice;
                }
                productsToClient.ProductName = itmProduct.First().ProductName;
                productsToClient.CategoryName = itmProduct.First().CategoryName;
                productsToClient.UnitsOnOrder = countUnits;
                productsToClient.TotalPrice = totalPrice;
                productsToClient.ProductId = countID;
                readyProductsToClient.Add(productsToClient);
                countID++;
            }
            return readyProductsToClient;
        }
    }
}
