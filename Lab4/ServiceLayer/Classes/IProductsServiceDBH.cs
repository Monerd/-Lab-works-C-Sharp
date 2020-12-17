using System.Collections.Generic;
using Models;

namespace ServiceLayer
{
    public interface IProductsServiceDBH
    {
        List<ProductsToClient> ModelChangeToClient(List<Products> orders);
    }
}
