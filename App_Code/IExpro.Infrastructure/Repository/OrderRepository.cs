using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using IExpro.Core.Interfaces.Repository;
using IExpro.Core.Models.Reports;
using Dapper;
using IExpro.Core.Entity;
using IExpro.Core.Interfaces.Common;

namespace IExpro.Infrastructure.Repository
{

    public class OrderRepository : GenericRepository<FormName>, IOrderRepository
    {
        public OrderRepository(IDataContext context)
: base(context)
        {
        }
        public IExproContext Context
        {
            get { return base.entities as IExproContext; }
        }
















        public IEnumerable<OrderStatusModel> GetOrderList(int CompanyId)
        {

            using (IDbConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {

                string sqlQuery = @"Select p.CompanyId,r.OrderId,q.ShortName,t.CustomerCode,r.CustomerOrderNo,CONVERT(NVARCHAR(11), r.OrderDate, 106) OrderDate,
CONVERT(NVARCHAR(11), r.DispatchDate, 106) DispatchDate,CONVERT(NVARCHAR(11), r.PackingDate, 106) PackingDate,
r.PackingId,r.ItemQuantity Quantity,r.DelayDays
from Master_Company p inner join CompanyInfo q on p.CompanyId=q.MasterCompanyid
inner join (
Select p.CompanyId,p.CustomerId,p.OrderId,--q.Item_Finished_Id as FinishedId,
p.CustomerOrderNo,p.OrderDate,p.DispatchDate,sum(q.QtyRequired) ItemQuantity,
DATEDIFF(day,CAST(p.DispatchDate as date),IsNULL(CAST(s.PackingDate as date),GetDate())) DelayDays ,IsNULL(s.PackingId,0) PackingId,s.PackingDate
from OrderMaster p inner join OrderDetail q on p.OrderId=q.OrderId
Left join PackingInformation r on p.OrderId=r.OrderId
Left join PACKING s on r.PackingId=s.PackingId
group By p.CompanyId,p.CustomerId,p.OrderId,
--q.Item_Finished_Id ,
p.CustomerOrderNo,p.OrderDate,p.DispatchDate,DATEDIFF(day,CAST(p.DispatchDate as date),IsNULL(CAST(s.PackingDate as date),GetDate())),s.PackingId,s.PackingDate
Having s.PackingDate is null
) as r on q.CompanyId=r.CompanyId
inner join customerinfo t on r.CustomerId=t.CustomerId
Where (r.OrderDate >= DATEADD(month, -6, GetDate())) AND  p.CompanyId=@CompanyId
";
                return (conn.Query<OrderStatusModel>(sqlQuery, new { @CompanyId = CompanyId, }));



            }

        }

        public IEnumerable<VendorPOStatusModel> GetVendorPOStatus(int CompanyId)
        {
            using (IDbConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {

                try
                {



                    string sqlQuery = @"Select pii.PindentIssueid, pii.Partyid, vendor.EmpName, vendor.Address, vendor.PhoneNo, pii.Challanno, pii.OrderNo PurchaseOrderNo, pii.Date as IssueDate,
pii.DueDate, pit.quantity, pit.Delivery_Date, x.ReceiveDate, y.FinishedId, y.QTY, y.PIndentIssueTranId, y.LotNo, y.Returndate
from OrderMaster om INNER
join PurchaseIndentIssue PII  on om.OrderId = PII.OrderId
INNER JOIN PURCHASEINDENTISSUETRAN PIT
ON PII.PINDENTISSUEID = PIT.PINDENTISSUEID
INNER JOIN empinfo vendor on pii.Partyid = vendor.EmpId
Left JOIN   PurchaseReceiveDetail y on PIT.Pindentissuetranid = y.PIndentIssueTranId
Left JOIN PurchaseReceiveMaster x
on x.PurchaseReceiveId = y.PurchaseReceiveId
Where om.CompanyId=@CompanyId";
                    var result = conn.Query(sqlQuery, new { @CompanyId = CompanyId, });


                    var objItem = (from itm in result
                                   group itm by new { itm.PindentIssueid } into itmGroup
                                   orderby itmGroup.Key.PindentIssueid descending
                                   select new VendorPOStatusModel
                                   {
                                       POId = itmGroup.Key.PindentIssueid,
                                       IssueDate = itmGroup.FirstOrDefault().IssueDate != null ? (DateTime)itmGroup.FirstOrDefault().IssueDate : DateTime.Now,
                                       ExpectedDate = itmGroup.FirstOrDefault().DueDate != null ? (DateTime)itmGroup.FirstOrDefault().DueDate : DateTime.Now,
                                       IssueQuantity = itmGroup.Sum(x => (int)(x.quantity ?? 0)),
                                       ReceiveQuantity = itmGroup.Sum(x => (int)(x.QTY ?? 0)),
                                       VendorName = itmGroup.FirstOrDefault().EmpName != null ? itmGroup.FirstOrDefault().EmpName : string.Empty,
                                       //VendorName = itmGroup.FirstOrDefault().EmpName + itmGroup.FirstOrDefault().Address + itmGroup.FirstOrDefault().PhoneNo,
                                       VendorPO = itmGroup.FirstOrDefault().Challanno != null ? itmGroup.FirstOrDefault().Challanno : string.Empty,
                                   });


                    return objItem;

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }


        }

        public IEnumerable<DyeingStatusModel> DyeingStatus(int CompanyId)
        {
            using (IDbConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {
                try
                {
                    string sqlQuery = @"Select x.IndentId, x.PartyId,xy.EmpName, xy.Address, xy.PhoneNo,x.IndentNo,
x.Date as IssueDate,x.ReqDate,yy.PRTid IssueId,yy.IssueQuantity,
yy.CanQty,yyy.PRTid ReceiveId,yyy.RecQuantity,yyy.LossQty,yyy.ReturnQty
from  INDENTMASTER x INNER JOIN empinfo xy on x.PartyId = xy.EmpId
inner join PP_PROCESSRAWTRAN yy on x.IndentId= yy.IndentID
inner join PP_ProcessRecTran yyy on yy.IndentId= yyy.IndentID
Where x.CompanyId=@CompanyId";

                    var result = conn.Query(sqlQuery, new { @CompanyId = CompanyId, });

                    var objItem = (from itm in result
                                   group itm by new { itm.IndentId } into itmGroup
                                   orderby itmGroup.Key.IndentId descending
                                   select new DyeingStatusModel
                                   {
                                       IndentId = itmGroup.Key.IndentId,
                                       IndentNo = itmGroup.FirstOrDefault().IndentNo != null ? itmGroup.FirstOrDefault().IndentNo : string.Empty,
                                       IssueDate = itmGroup.FirstOrDefault().IssueDate != null ? (DateTime)itmGroup.FirstOrDefault().IssueDate : DateTime.Now,
                                       ExpectedDate = itmGroup.FirstOrDefault().ReqDate != null ? (DateTime)itmGroup.FirstOrDefault().ReqDate : DateTime.Now,
                                       IssueQuantity = itmGroup.Select(x => new { x.IssueId, x.IssueQuantity }).Distinct().Sum(x => (double)x.IssueQuantity),
                                       ReceiveQuantity = itmGroup.Sum(x => (double)x.RecQuantity),
                                       DyerName = itmGroup.FirstOrDefault().EmpName != null ? itmGroup.FirstOrDefault().EmpName : string.Empty,


                                   });
                    var dsd = objItem.ToList();

                    return objItem;

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }




        }





        public IEnumerable<PurchaseRawMaterialModel> GetPurchaseList(int OrderId)
        {
            using (IDbConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {

                string sqlQuery = @"SELECT E.EMPNAME Vendor, EMPID EMPID, ITEM_NAME+' '+QUALITYNAME+' '+DESIGNNAME+' '+COLORNAME+' '+SHAPENAME+' '+ShadeColorName+' '+CASE WHEN FLAGSIZE=0 THEN SIZEFT    
ELSE CASE WHEN FLAGSIZE=1 THEN SIZEMTR ELSE SIZEINCH END END + ' '+CASE WHEN VF.SIZEID>0 THEN ST.TYPE ELSE '' END Technique,     
VF.QUALITYNAME, COLORNAME+' '+SHAPENAME+' '+SHADECOLORNAME COLOUR, PIIT.QUANTITY IssueQty, ISNULL(SUM(PIIR.QTY-ISNULL(PIIR.BELLWT, 0) - ISNULL(V.RETURNQTY, 0)), 0) ReceiveQty,     
PIIT.DELIVERY_DATE DeliveryDate, PIIT.ORDERID, PII.PINDENTISSUEID, PII.DUEDATE,PRM.RECEIVEDATE  ,PRM.BILLNO,   
--REPLACE(CONVERT(NVARCHAR(11), PRM.RECEIVEDATE, 106), ' ', '-') + '/'+ BILLNO RECDATE, PRM.BILLNO, PRM.GATEINNO, ISNULL(PRM.PURCHASERECEIVEID, 0) PURCHASERECEIVEID,     
'' RECDATE, '' BILLNO, '' GATEINNO, 0 PURCHASERECEIVEID,     
PIIT.RATE, Sum(ISNULL(V.RETURNQTY, 0)) ReturnQty, BILLNO1, PII.DATE IssueDate, ISNULL(PIIT.CANQTY, 0) CANQTY, PII.USERID, PII.PAYEMENTTERMID PAYEMENTTERMID,     
PII.DELIVERYTERMID DELIVERYTERMID, ISNULL(SUM(PIIR.PENALTY), 0) PENALTYORDEBITNOTE, ISNULL(PIIR.VAT, 0) VAT,     
SUM(((ISNULL(PIIR.QTY, 0)-ISNULL(PIIR.BELLWT, 0)- ISNULL(V.RETURNQTY, 0))*ISNULL(PIIR.RATE, 0) )+((((ISNULL(PIIR.QTY, 0)-ISNULL(PIIR.BELLWT, 0)- ISNULL(V.RETURNQTY, 0))*ISNULL(PIIR.RATE, 0))- ISNULL(PIIR.PENALTY, 0))*ISNULL(PIIR.VAT, 0)/100)) HISSSAB,    
 
VF.ITEM_FINISHED_ID FINISHEDID, PIIT.PINDENTISSUETRANID, --REPLACE(CONVERT(NVARCHAR(11), V.DATE, 106), ' ', '-')     
'' RETURNDATE, '' RETURNCHALLAN, PII.CHALLANNO PurchaseOrderNo,     
ISNULL(OM.LOCALORDER, 'STOCK') LOCALORDER, ISNULL(CI.CUSTOMERCODE, 'STOCK') CUSTOMERCODE, ISNULL(SUM(PIIR.LSHORTPERCENTAGE), 0) LSHORTPERCENTAGE, OM.CUSTOMERID,     
PIIT.FLAGSIZE, VF.CATEGORY_NAME Category, PII.STATUS, ISNULL(SUM(PIIR.QTY-ISNULL(PIIR.BELLWT, 0)), 0) Recqty_beforeRetnqty, pii.Companyid, OM.CustomerOrderNo,PIIT.GSTType,PIIT.SGST,PIIT.IGST,PII.requestby,PII.requestfor     
FROM PURCHASEINDENTISSUE PII     
INNER JOIN EMPINFO E ON PII.PARTYID=EMPID     
INNER JOIN PURCHASEINDENTISSUETRAN PIIT ON PIIT.PINDENTISSUEID=PII.PINDENTISSUEID     
LEFT JOIN V_FINISHEDITEMDETAIL VF ON PIIT.FINISHEDID=VF.ITEM_FINISHED_ID     
LEFT JOIN PURCHASERECEIVEDETAIL PIIR ON PIIR.PINDENTISSUETRANID=PIIT.PINDENTISSUETRANID AND PIIR.PINDENTISSUEID=PII.PINDENTISSUEID     
LEFT JOIN PURCHASERECEIVEMASTER PRM ON PRM.PURCHASERECEIVEID=PIIR.PURCHASERECEIVEID    
LEFT JOIN V_PURCHASERETURNDETAIL V ON PIIR.PURCHASERECEIVEDETAILID=V.PURCHASERECEIVEDETAILID    
--LEFT JOIN V_PURCHASEWITHBUYER VP ON VP.PINDENTISSUEID=PII.PINDENTISSUEID    
INNER JOIN OrderMaster OM ON OM.OrderID = PIIT.OrderID     
LEFT JOIN CustomerInfo CI ON CI.CustomerId = OM.CustomerId     
LEFT JOIN SIZETYPE ST ON PIIT.FLAGSIZE=ST.VAL    
GROUP BY E.EMPNAME, EMPID, ITEM_NAME, DESIGNNAME, VF.QUALITYNAME, COLORNAME, SHADECOLORNAME, SHAPENAME, PII.PINDENTISSUEID, PIIT.DELIVERY_DATE,     
PIIT.ORDERID, PII.PINDENTISSUEID, PII.DUEDATE,PRM.RECEIVEDATE, PRM.BILLNO, --PRM.GATEINNO, PRM.PURCHASERECEIVEID,     
PIIT.RATE, QTYRETURN, BILLNO1, PII.DATE, PIIT.CANQTY,     
PII.USERID, PII.PAYEMENTTERMID, PII.DELIVERYTERMID, PIIR.VAT, VF.ITEM_FINISHED_ID, PIIT.QUANTITY, PIIT.PINDENTISSUETRANID, --V.RETURNQTY, V.DATE,     
FLAGSIZE, SIZEFT, SIZEMTR, SIZEINCH, PII.CHALLANNO, OM.LOCALORDER, CI.CUSTOMERCODE, OM.CUSTOMERID, ST.TYPE, VF.SIZEID, VF.CATEGORY_NAME, PII.STATUS,     
PIIT.CANQTY, pii.Companyid, OM.OrderID,OM.CustomerOrderNo ,PIIT.GSTType,PIIT.SGST,PIIT.IGST,PII.requestby,PII.requestfor     
  having OM.OrderID=@OrderId";

                var result = conn.Query(sqlQuery, new { @OrderId = OrderId, });
                var lst = result.Select(x => new PurchaseRawMaterialModel
                {
                    Category = x.Category != null ? x.Category : string.Empty,
                    PONo = x.PurchaseOrderNo != null ? x.PurchaseOrderNo : string.Empty,
                    POStatus = x.STATUS != null ? x.STATUS : string.Empty,
                    PODate = x.IssueDate != null ? x.IssueDate.ToString("dd MMM yyyy") : string.Empty,
                    SupplierName = x.Vendor != null ? x.Vendor : string.Empty,
                    ItemName = x.Technique != null ? x.Technique : string.Empty,
                    Rate = x.Rate != null ? x.Rate.ToString() : string.Empty,
                    POQty = x.IssueQty != null ? x.IssueQty.ToString() : string.Empty,
                    DelvDate = x.DeliveryDate != null ? x.DeliveryDate.ToString("dd MMM yyyy") : string.Empty,
                    RecDate = x.ReceiveDate != null ? x.ReceiveDate.ToString("dd MMM yyyy") : string.Empty,
                    RecQty = x.ReceiveQty != null ? x.ReceiveQty.ToString() : string.Empty,
                    ChallanNo = x.PurchaseOrderNo != null ? x.PurchaseOrderNo : string.Empty,
                    RetDate = x.Returndate != null ? x.Returndate.ToString() : string.Empty,
                    RetQty = x.ReturnQty != null ? x.ReturnQty.ToString() : string.Empty,
                    BillNo = x.BillNo != null ? x.BillNo : string.Empty,
                });
                return (lst);
            }
        }
    }
}
