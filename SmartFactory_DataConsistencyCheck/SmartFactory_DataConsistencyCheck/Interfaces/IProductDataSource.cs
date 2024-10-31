using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartFactory_DataConsistencyCheck.Models;

public interface IProductDataSource
{
    Task<Product> GetProductByIdAsync(int productId);
    Task<IEnumerable<Product>> GetAllProductsAsync();
}