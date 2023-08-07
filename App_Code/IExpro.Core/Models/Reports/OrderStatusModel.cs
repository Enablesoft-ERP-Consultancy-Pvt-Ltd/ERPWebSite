using System;

namespace IExpro.Core.Models.Reports
{
    public class OrderStatusModel
    {
        public int OrderId { get; set; }
        public int PackingId { get; set; }
        public string ShortName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerOrderNo { get; set; }
        public string OrderDate { get; set; }
        public string Quantity { get; set; }
        public string DispatchDate { get; set; }
        public string PackingDate { get; set; }
        public string DelayDays { get; set; }
        public int OrderStatus { get { return PackingDate != null ? 1 : 0; } }
    }

    public class PurchaseRawMaterialModel
    {
        public string Category { get; set; }
        public string PONo { get; set; }
        public string POStatus { get; set; }
        public string PODate { get; set; }
        public string SupplierName { get; set; }
        public string ItemName { get; set; }
        public string Rate { get; set; }
        public string POQty { get; set; }
        public string DelvDate { get; set; }
        public string DelayDays { get; set; }
        public string RecDate { get; set; }
        public string RecQty { get; set; }
        public string ChallanNo { get; set; }
        public string LotNo { get; set; }
        public string BillNo { get; set; }
        public string RetDate { get; set; }
        public string RetQty { get; set; }
        public string PendingQty { get; set; }
        public string ReceiveRemark { get; set; }
        public string OrderRemark { get; set; }

    }


















    public class VendorPOStatusModel
    {
        public int POId { get; set; }
        public string VendorPO { get; set; }
        public string VendorName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        public int Quantity { get { return (IssueQuantity - ReceiveQuantity); } }
        public int IssueQuantity { get; set; }
        public int ReceiveQuantity { get; set; }
        public int DelayDays { get { return (DateTime.Now.Date - ExpectedDate.Date).Days; } }
        public int OrderStatus { get { return Quantity > 0 ? 0 : 1; } }


    }


    public class DyeingStatusModel
    {
        public int IndentId { get; set; }
        public string IndentNo { get; set; }
        public string DyerName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        public double IssueQuantity { get; set; }
        public double ReceiveQuantity { get; set; }
        public double Quantity { get { return (IssueQuantity - ReceiveQuantity); } }
        public int DelayDays { get { return (DateTime.Now.Date - ExpectedDate.Date).Days; } }
        public int DyeingStatus { get { return Quantity > 0 ? 0 : 1; } }


    }



}
