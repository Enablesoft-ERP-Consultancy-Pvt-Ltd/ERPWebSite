using IExpro.Core.Models.Reports;
using System.Collections.Generic;

namespace IExpro.Core.Interfaces.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<OrderStatusModel> GetOrderList(int CompanyId);
        IEnumerable<OrderDetailModel> GetOrderDetail(int OrderId);
        IEnumerable<VendorPOStatusModel> GetVendorPOStatus(int CompanyId);
        IEnumerable<DyeingStatusModel> DyeingStatus(int CompanyId);
        IEnumerable<PurchaseRawMaterialModel> GetPurchaseList(int OrderId);
        IEnumerable<IndentRawMaterialModel> GetOrderByIndentDetail(int OrderId, int ProcessId);
        IEnumerable<IssueMaterialModel> GetOrderByIssueId(int OrderId, int ProcessId);
        IEnumerable<IssueMaterialModel> GetFinishedItem(int OrderId);

    }
}
