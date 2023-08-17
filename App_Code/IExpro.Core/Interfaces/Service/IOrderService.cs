using IExpro.Core.Models.Reports;
using System.Collections.Generic;

namespace IExpro.Core.Interfaces.Service
{
    public interface IOrderService
    {
        IEnumerable<OrderStatusModel> GetOrderList(int CompanyId);
        IEnumerable<VendorPOStatusModel> GetVendorPOStatus(int CompanyId);
        IEnumerable<DyeingStatusModel> DyeingStatus(int CompanyId);
        IEnumerable<PurchaseRawMaterialModel> GetPurchaseList(int OrderId);


        IEnumerable<IndentRawMaterialModel> GetOrderByIndentDetail(int OrderId, int ProcessId);



        }
}
