using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartFactory_DataConsistencyCheck.Models;

public interface IOrderDataSource
{
    Task<Order> GetOrderByIdAsync(int orderId);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task UpdateOrderAsync(int orderId, Order updatedOrder);
}
