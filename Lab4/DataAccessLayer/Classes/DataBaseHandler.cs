using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Models;

namespace DataAccessLayer
{
    class DataBaseHandler
    {
        private readonly string connectionString;
        private readonly string storedProcedure;
        public List<Products> Products { get; set; }
        public DataSet ProductsSet { get; set; }
        public DataBaseHandler(string connectionString, string storedProcedure)
        {
            this.connectionString = connectionString;
            this.storedProcedure = storedProcedure;
            ProductsSet = new DataSet();
        }
        public void ReadDataBase()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            using SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ProductsSet);
        }
        public void ListFromDataSet()
        {
            var ProductList = ProductsSet.Tables[0].AsEnumerable().Select(dataRow => new Products
            {
                ProductID = dataRow.Field<int>("ProductID"),
                ProductName = dataRow.Field<string>("ProductName"),
                QuantityPerUnit = dataRow.Field<string>("QuantityPerUnit"),
                UnitPrice = dataRow.Field<decimal>("UnitPrice"),
                UnitsOnOrder = dataRow.Field<Int16>("UnitsOnOrder"),
                CategoryName = dataRow.Field<string>("CategoryName"),
            }).ToList<Products>();
            Products = ProductList;
        }

    }
}