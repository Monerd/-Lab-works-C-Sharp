using System;

namespace Models
{
    public class ProductsToClient
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public decimal TotalPrice { get; set; }
        public Int16 UnitsOnOrder { get; set; }
    }
}