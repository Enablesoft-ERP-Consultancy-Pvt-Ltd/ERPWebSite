using System;
using System.Web.Services.Description;
using System.Windows;
using IExpro.Core.Common;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

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

        public string PODate { get; set; }
        public string SupplierName { get; set; }
        public string ItemName { get; set; }
        public string Rate { get; set; }
        public double POQty { get; set; }
        public double RecQty { get; set; }
        public double RetQty { get; set; }
        public string ChallanNo { get; set; }
        public string LotNo { get; set; }
        public string BillNo { get; set; }
        public string RetDate { get; set; }
        public string ReceiveRemark { get; set; }
        public string OrderRemark { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string DelvDate { get { return DeliveryDate.ToString("dd MMM yyyy"); } }
        public string RecDate { get { return ReceiveDate.ToString("dd MMM yyyy"); } }

        public double PendingQty
        {
            get { return (POQty - (RetQty + RecQty)); }
        }

        public int DelayDays { get { return (ReceiveDate.Date - DeliveryDate.Date).Days; } }
        public ProcessStatus ItemStatus { get { return PendingQty > 0 ? ProcessStatus.Pending : ProcessStatus.Completed; } }
        public string POStatus { get { return this.ItemStatus.ToString(); } }

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

    public class IssueMaterialModel
    {
        public int IssueId { get; set; }
        public string IssueNo { get; set; }
        public int ProcessId { get; set; }
        public int EmpId { get; set; }
        public int OrderId { get; set; }
        public int FinishedId { get; set; }
        public int MaterialId { get; set; }
        public DateTime AssignDate { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string VendorName { get; set; }
        public string MaterialName { get; set; }
        public decimal Rate { get; set; }
        public decimal IssueQuantity { get; set; }
        public decimal RecQuantity { get; set; }
        public string IssueDate { get { return AssignDate.ToString("dd MMM yyyy"); } }
        public string ReqDate { get { return RequestDate.ToString("dd MMM yyyy"); } }

        public decimal PendingQty
        {
            get { return (IssueQuantity - RecQuantity); }
        }
        public string RecDate
        {
            get
            {
                return ReceiveDate.HasValue ? ReceiveDate.Value.ToString("dd MMM yyyy") : "---";
            }
        }
        public int DelayDays
        {
            get
            {
                int result = 0;
                if (ReceiveDate.HasValue)
                {
                    result = (ReceiveDate.Value.Date - RequestDate.Date).Days;

                }
                return result;
            }
        }
        public ProcessStatus ItemStatus
        {
            get
            {
                if (IssueQuantity == 0)
                {
                    return ProcessStatus.Pending;
                }
                else
                {
                    return PendingQty > 0 ? ProcessStatus.Pending : ProcessStatus.Completed;
                }
            }
        }
        public string IStatus { get { return this.ItemStatus.ToString(); } }

    }





    public class IndentRawMaterialModel
    {
        public string VendorName { get; set; }
        public string Category { get; set; }
        public string MaterialName { get; set; }
        public string QualityName { get; set; }
        public string ColorName { get; set; }
        public string ShadeName { get; set; }
        public int PPNo { get; set; }
        public int ProcessId { get; set; }
        public int CompanyId { get; set; }
        public int OrderId { get; set; }
        public int OrderDetailId { get; set; }
        public int IndentId { get; set; }
        public string IndentNo { get; set; }
        public int PartyId { get; set; }
        public int IFinishedId { get; set; }
        public int OFinishedId { get; set; }
        public decimal IndentQty { get; set; }
        public decimal IssueQuantity { get; set; }
        public decimal RecQuantity { get; set; }
        public decimal RequiredQty { get; set; }

        public decimal ReturnQty { get; set; }
        public decimal ExtraQty { get; set; }
        public decimal CancelQty { get; set; }
        public decimal Quantity { get; set; }
        public int IssueId { get; set; }

        public decimal Moisture { get; set; }

        public decimal ConsmpQty { get; set; }
        public decimal LossQty { get; set; }

        public int ReturnId { get; set; }

        public string TagRemarks { get; set; }
        public DateTime ReqDate { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public DateTime ReturnDate { get; set; }




        public string RequestDate { get { return ReqDate.ToString("dd MMM yyyy"); } }
        public string IndentDate { get { return IssueDate.ToString("dd MMM yyyy"); } }


        public string RecDate
        {
            get
            {


                return ReceiveDate != null ? ReceiveDate.ToString("dd MMM yyyy") : "---";
            }
        }

        public int DelayDays
        {
            get
            {
                var result = (ReceiveDate.Date - ReqDate.Date).Days;
                return result < 0 ? 0 : result;
            }
        }


        public decimal PendingQty
        {
            get { return (IssueQuantity - (RecQuantity - ReturnQty)); }
        }



        public ProcessStatus ItemStatus
        {
            get
            {
                if (IssueQuantity == 0)
                {
                    return ProcessStatus.Pending;
                }
                else
                {
                    return PendingQty > 0 ? ProcessStatus.Pending : ProcessStatus.Completed;
                }
            }
        }
        public string IStatus { get { return this.ItemStatus.ToString(); } }

    }











}
