using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Models;

namespace VMS.Application.Interfaces
{
    public interface IManageOrderRepository
    {
        Task<int> SaveFoodOrder(FoodOrder_Request parameters);

        Task<IEnumerable<FoodOrder_Response>> GetFoodOrderList(FoodOrderSearch_Request parameters);

        Task<FoodOrder_Response?> GetFoodOrderById(int Id);

        Task<int> SaveFoodOrderItem(FoodOrderItem_Request parameters);

        Task<IEnumerable<FoodOrderItem_Response>> GetFoodOrderItemList(FoodOrderItemSearch_Request parameters);
    }
}
