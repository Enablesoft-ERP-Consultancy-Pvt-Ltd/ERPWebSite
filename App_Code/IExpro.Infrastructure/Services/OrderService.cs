using IExpro.Core.Interfaces.Repository;
using IExpro.Core.Interfaces.Service;
using IExpro.Core.Models.Reports;
using System.Collections.Generic;

namespace IExpro.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private IUnitOfWork IU { get; set; }
        //private IOrderRepository OrdRepo { get; set; }

        public OrderService(IUnitOfWork _IU)
        {
            this.IU = _IU;
        }

        public IEnumerable<OrderStatusModel> GetOrderList(int CompanyId)
        {
            return this.IU.OrdRepo.GetOrderList(CompanyId);
        }

        public IEnumerable<OrderDetailModel> GetOrderDetail(int OrderId)
        {
            return this.IU.OrdRepo.GetOrderDetail(OrderId);
        }
        public IEnumerable<VendorPOStatusModel> GetVendorPOStatus(int CompanyId)
        {
            return this.IU.OrdRepo.GetVendorPOStatus(CompanyId);
        }

        public IEnumerable<DyeingStatusModel> DyeingStatus(int CompanyId)
        {
            return this.IU.OrdRepo.DyeingStatus(CompanyId);
        }

        public IEnumerable<PurchaseRawMaterialModel> GetPurchaseList(int OrderId)
        {
            return this.IU.OrdRepo.GetPurchaseList(OrderId);
        }
        public IEnumerable<IndentRawMaterialModel> GetOrderByIndentDetail(int OrderId, int ProcessId)
        {
            return this.IU.OrdRepo.GetOrderByIndentDetail(OrderId, ProcessId);
        }

        public IEnumerable<IssueMaterialModel> GetOrderByIssueId(int OrderId, int ProcessId)
        {
            return this.IU.OrdRepo.GetOrderByIssueId(OrderId, ProcessId);
        }


    }
}
