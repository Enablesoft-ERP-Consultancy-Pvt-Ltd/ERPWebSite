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
using IExpro.Core.Common;
using AjaxControlToolkit;

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

        public IEnumerable<OrderDetailModel> GetOrderDetail(int OrderId)
        {

            using (IDbConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {
                string sqlQuery = @"Select p.CompanyId BranchId,p.CustomerId,p.OrderId,p.CustomerOrderNo,
p.LocalOrder,CONVERT(NVARCHAR(11), p.OrderDate, 106) OrderDate,CONVERT(NVARCHAR(11), p.DispatchDate, 106) DispatchDate,CONVERT(NVARCHAR(11), p.DueDate, 106) DueDate,
p.Remarks,q.Item_Finished_Id as FinishedId,
VF.ITEM_NAME Technique,VF.QUALITYNAME Quality,VF.DESIGNNAME Design, VF.COLORNAME Color,VF.SHAPENAME Shape,
VF.SHADECOLORNAME Shade,(CASE WHEN q.FLAGSIZE=0 THEN VF.SIZEFT ELSE CASE WHEN q.FLAGSIZE=1 THEN VF.SIZEMTR ELSE VF.SIZEINCH END END) SIZE,
CASE WHEN VF.SIZEID>0 THEN ST.TYPE ELSE '' END Unit,q.OrderDetailId, IsNULL(q.CancelQty,0.0) CancelQty,
IsNULL(q.HoldQty,0.0)  HoldQty,q.QtyRequired OrderQty,IsNULL(q.extraqty,0.0) ExtraQty,q.extraqty, 
IsNULL(q.filler,'') Filler from OrderMaster p  WITH (NOLOCK) inner join OrderDetail q   WITH (NOLOCK)
on p.OrderId=q.OrderId inner join V_FINISHEDITEMDETAIL VF 
ON Q.Item_Finished_Id=VF.ITEM_FINISHED_ID LEFT JOIN SIZETYPE ST  WITH (NOLOCK) ON q.FLAGSIZE=ST.VAL  
Where p.OrderId=@OrderId";

                return (conn.Query<OrderDetailModel>(sqlQuery, new { @OrderId = OrderId }));

            }

        }


        public IEnumerable<OrderStatusModel> GetOrderList(int CompanyId)
        {

            using (IDbConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {

                //                string sqlQuery = @"Select p.CompanyId,r.OrderId,q.ShortName,t.CustomerCode,r.CustomerOrderNo,CONVERT(NVARCHAR(11), r.OrderDate, 106) OrderDate,
                //CONVERT(NVARCHAR(11), r.DispatchDate, 106) DispatchDate,CONVERT(NVARCHAR(11), r.PackingDate, 106) PackingDate,
                //r.PackingId,r.ItemQuantity Quantity,r.DelayDays
                //from Master_Company p inner join CompanyInfo q on p.CompanyId=q.MasterCompanyid
                //inner join (
                //Select p.CompanyId,p.CustomerId,p.OrderId,--q.Item_Finished_Id as FinishedId,
                //p.CustomerOrderNo,p.OrderDate,p.DispatchDate,sum(q.QtyRequired) ItemQuantity,
                //DATEDIFF(day,CAST(p.DispatchDate as date),IsNULL(CAST(s.PackingDate as date),GetDate())) DelayDays ,IsNULL(s.PackingId,0) PackingId,s.PackingDate
                //from OrderMaster p inner join OrderDetail q on p.OrderId=q.OrderId
                //Left join PackingInformation r on p.OrderId=r.OrderId
                //Left join PACKING s on r.PackingId=s.PackingId
                //group By p.CompanyId,p.CustomerId,p.OrderId,
                //--q.Item_Finished_Id ,
                //p.CustomerOrderNo,p.OrderDate,p.DispatchDate,DATEDIFF(day,CAST(p.DispatchDate as date),IsNULL(CAST(s.PackingDate as date),GetDate())),s.PackingId,s.PackingDate
                //Having s.PackingDate is null
                //) as r on q.CompanyId=r.CompanyId
                //inner join customerinfo t on r.CustomerId=t.CustomerId
                //Where (r.OrderDate >= DATEADD(month, -6, GetDate())) AND  p.CompanyId=@CompanyId
                //";


                string sqlQuery = @"With OrderItem(CompanyId,CustomerId,OrderId,CustomerOrderNo,LocalOrder,OrderDate,DispatchDate,
DueDate,Remarks,OrderQty,ExtraQty,CancelQty,HoldQty,RowNo)
AS (Select p.CompanyId,p.CustomerId,p.OrderId,p.CustomerOrderNo,
p.LocalOrder,p.OrderDate,p.DispatchDate,p.DueDate,p.Remarks,
SUM(IsNULL(q.QtyRequired,0.0)) OVER (PARTITION BY p.CompanyId,p.OrderId) Quantity,
SUM(IsNULL(q.extraqty,0.0)) OVER (PARTITION BY p.CompanyId,p.OrderId) ExtraQty,
SUM(IsNULL(q.CancelQty,0.0)) OVER (PARTITION BY p.CompanyId,p.OrderId) CancelQty,
SUM(IsNULL(q.HoldQty,0.0)) OVER (PARTITION BY p.CompanyId,p.OrderId) HoldQty,
ROW_NUMBER() OVER(PARTITION BY p.CompanyId,p.OrderId ORDER BY p.CompanyId,p.OrderId) RowNo
from OrderMaster p WITH (NOLOCK) inner join OrderDetail q WITH (NOLOCK) on p.OrderId=q.OrderId),
PackItem(PackingId,OrderId,PackingDate,RowNo)
AS (Select r.PackingId ,r.OrderId,Max(s.PackingDate) OVER (PARTITION BY r.OrderId,r.PackingId) PackingDate,
ROW_NUMBER() OVER(PARTITION BY r.OrderId,r.PackingId ORDER BY r.OrderId,r.PackingId) RowNo
from PackingInformation r WITH (NOLOCK)  
Inner join PACKING s WITH (NOLOCK) on r.PackingId=s.PackingId)
Select p.CompanyId IExproId,q.CompanyId,p.CompanyName,q.ShortName,t.CustomerCode,x.OrderId,x.CustomerOrderNo,
CONVERT(NVARCHAR(11), x.OrderDate, 106) OrderDate,
CONVERT(NVARCHAR(11), x.DispatchDate, 106) DispatchDate,
IsNULL(y.PackingId,0) PackingId,
CONVERT(NVARCHAR(11), y.PackingDate, 106) PackingDate,
DATEDIFF(day,CAST(x.DispatchDate as date),IsNULL(CAST(y.PackingDate as date),GetDate())) DelayDays 
from OrderItem x  Left Join PackItem y  
ON x.OrderId=y.OrderId and x.RowNo=y.RowNo
Inner Join  CompanyInfo q WITH (NOLOCK) ON x.CompanyId=q.CompanyId
inner join Master_Company p WITH (NOLOCK)  ON  p.CompanyId=q.MasterCompanyId
inner join customerinfo t WITH (NOLOCK) on x.CustomerId=t.CustomerId
Where x.RowNo=1 and (x.OrderDate >= DATEADD(month, -6, GetDate())) AND  x.CompanyId=@CompanyId";

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

                string sqlQuery = @"SELECT E.EMPNAME Vendor, EMPID EMPID,VF.ITEM_NAME+' '+VF.QUALITYNAME+' '+VF.DESIGNNAME+' '+VF.COLORNAME+' '+VF.SHAPENAME+' '+VF.ShadeColorName+' '+CASE WHEN PIIT.FLAGSIZE=0 THEN VF.SIZEFT    
ELSE CASE WHEN PIIT.FLAGSIZE=1 THEN VF.SIZEMTR ELSE VF.SIZEINCH END END + ' '+CASE WHEN VF.SIZEID>0 THEN ST.TYPE ELSE '' END Technique, 
VF.QUALITYNAME, VF.COLORNAME+' '+VF.SHAPENAME+' '+VF.SHADECOLORNAME COLOUR, PIIT.QUANTITY IssueQty, ISNULL(SUM(PIIR.QTY-ISNULL(PIIR.BELLWT, 0) - ISNULL(V.RETURNQTY, 0)), 0) ReceiveQty,     
PIIT.DELIVERY_DATE DeliveryDate, PIIT.ORDERID, PII.PINDENTISSUEID, CONVERT(NVARCHAR(11), PII.DUEDATE, 106) DUEDATE,PRM.RECEIVEDATE,PRM.BILLNO,   
'' RECDATE, '' BILLNO, '' GATEINNO, 0 PURCHASERECEIVEID,PIIT.RATE, Sum(ISNULL(V.RETURNQTY, 0)) ReturnQty, BILLNO1,CONVERT(NVARCHAR(11), PII.DATE, 106) IssueDate, ISNULL(PIIT.CANQTY, 0) CANQTY, PII.USERID, PII.PAYEMENTTERMID PAYEMENTTERMID,     
PII.DELIVERYTERMID DELIVERYTERMID, ISNULL(SUM(PIIR.PENALTY), 0) PENALTYORDEBITNOTE, ISNULL(PIIR.VAT, 0) VAT,     
SUM(((ISNULL(PIIR.QTY, 0)-ISNULL(PIIR.BELLWT, 0)- ISNULL(V.RETURNQTY, 0))*ISNULL(PIIR.RATE, 0) )+((((ISNULL(PIIR.QTY, 0)-ISNULL(PIIR.BELLWT, 0)- ISNULL(V.RETURNQTY, 0))*ISNULL(PIIR.RATE, 0))- ISNULL(PIIR.PENALTY, 0))*ISNULL(PIIR.VAT, 0)/100)) HISSSAB,     
VF.ITEM_FINISHED_ID FINISHEDID, PIIT.PINDENTISSUETRANID, '' RETURNDATE, '' RETURNCHALLAN, PII.CHALLANNO PurchaseOrderNo,     
ISNULL(OM.LOCALORDER, 'STOCK') LOCALORDER, ISNULL(CI.CUSTOMERCODE, 'STOCK') CUSTOMERCODE, ISNULL(SUM(PIIR.LSHORTPERCENTAGE), 0) LSHORTPERCENTAGE, OM.CUSTOMERID,     
PIIT.FLAGSIZE, VF.CATEGORY_NAME Category, PII.STATUS, ISNULL(SUM(PIIR.QTY-ISNULL(PIIR.BELLWT, 0)), 0) Recqty_beforeRetnqty, pii.Companyid, OM.CustomerOrderNo,PIIT.GSTType,PIIT.SGST,PIIT.IGST,PII.requestby,PII.requestfor     
FROM PURCHASEINDENTISSUE PII INNER JOIN EMPINFO E ON PII.PARTYID=EMPID     
INNER JOIN PURCHASEINDENTISSUETRAN PIIT ON PIIT.PINDENTISSUEID=PII.PINDENTISSUEID     
INNER JOIN OrderMaster OM ON OM.OrderID = PIIT.OrderID     
LEFT JOIN V_FINISHEDITEMDETAIL VF ON PIIT.FINISHEDID=VF.ITEM_FINISHED_ID     
LEFT JOIN PURCHASERECEIVEDETAIL PIIR ON PIIR.PINDENTISSUETRANID=PIIT.PINDENTISSUETRANID AND PIIR.PINDENTISSUEID=PII.PINDENTISSUEID     
LEFT JOIN PURCHASERECEIVEMASTER PRM ON PRM.PURCHASERECEIVEID=PIIR.PURCHASERECEIVEID    
LEFT JOIN V_PURCHASERETURNDETAIL V ON PIIR.PURCHASERECEIVEDETAILID=V.PURCHASERECEIVEDETAILID       
LEFT JOIN CustomerInfo CI ON CI.CustomerId = OM.CustomerId LEFT JOIN SIZETYPE ST ON PIIT.FLAGSIZE=ST.VAL    
GROUP BY E.EMPNAME, EMPID, ITEM_NAME, DESIGNNAME, VF.QUALITYNAME, COLORNAME, SHADECOLORNAME, SHAPENAME, PII.PINDENTISSUEID, PIIT.DELIVERY_DATE,     
PIIT.ORDERID, PII.PINDENTISSUEID, PII.DUEDATE,PRM.RECEIVEDATE, PRM.BILLNO, PIIT.RATE, QTYRETURN, BILLNO1, PII.DATE, 
PIIT.CANQTY, PII.USERID, PII.PAYEMENTTERMID, PII.DELIVERYTERMID, PIIR.VAT, VF.ITEM_FINISHED_ID, PIIT.QUANTITY, PIIT.PINDENTISSUETRANID,     
FLAGSIZE, SIZEFT, SIZEMTR, SIZEINCH, PII.CHALLANNO, OM.LOCALORDER, CI.CUSTOMERCODE, OM.CUSTOMERID, ST.TYPE, VF.SIZEID, VF.CATEGORY_NAME, PII.STATUS,     
PIIT.CANQTY, pii.Companyid, OM.OrderID,OM.CustomerOrderNo ,PIIT.GSTType,PIIT.SGST,PIIT.IGST,PII.requestby,PII.requestfor         
Having OM.OrderID=@OrderId";

                var result = conn.Query(sqlQuery, new { @OrderId = OrderId, });
                var lst = result.Select(x => new PurchaseRawMaterialModel
                {
                    Category = x.Category != null ? x.Category : string.Empty,
                    SupplierName = x.Vendor != null ? x.Vendor : string.Empty,
                    BillNo = x.BillNo != null ? x.BillNo : string.Empty,
                    ItemName = x.Technique != null ? x.Technique : string.Empty,
                    ChallanNo = x.PurchaseOrderNo != null ? x.PurchaseOrderNo : string.Empty,
                    PONo = x.PurchaseOrderNo != null ? x.PurchaseOrderNo : string.Empty,
                    POStatus = x.STATUS != null ? x.STATUS : string.Empty,
                    PODate = x.IssueDate != null ? x.IssueDate : string.Empty,
                    Rate = x.RATE != null ? x.RATE.ToString() : string.Empty,
                    POQty = x.IssueQty != null ? Convert.ToDouble(x.IssueQty) : 0,
                    RetQty = x.ReturnQty != null ? Convert.ToDouble(x.ReturnQty) : 0,
                    RecQty = x.ReceiveQty != null ? Convert.ToDouble(x.ReceiveQty) : 0,
                    DeliveryDate = x.DeliveryDate != null ? x.DeliveryDate : DateTime.Now,
                    ReceiveDate = x.ReceiveDate != null ? x.ReceiveDate : DateTime.Now,
                    RetDate = x.Returndate != null ? x.Returndate.ToString() : string.Empty,


                });
                return (lst);
            }
        }

        public IEnumerable<IndentRawMaterialModel> GetOrderByIndentDetail(int OrderId, int ProcessId)
        {
            using (IDbConnection conn = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING))
            {

                string sqlQuery = @"WITH IndentItem(PPNo,ProcessId,OrderId,OrderDetailId,IndentNo,IndentId,PartyId,IFinishedId,OFinishedId,IssueDate,ReqDate,IndentQty,ExtraQty,CancelQty,Quantity,FLAGSIZE,RowNo) AS 
(SELECT ID.PPNo,IM.ProcessID,ID.ORDERID,ID.Orderdetailid,IM.IndentNo,IM.IndentId,IM.PartyId,Id.IFinishedId,Id.OFinishedId,IM.Date IssueDate,IM.ReqDate,
SUM(IsNull(ID.IndentQty,0.00)) OVER (PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId) IndentQty,
SUM(IsNull(ID.ExtraQty,0.00)) OVER (PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId) ExtraQty,
SUM(IsNull(ID.CancelQty,0.00)) OVER (PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId) CancelQty,
SUM(IsNull(ID.Quantity,0.00)) OVER (PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId) Quantity,Id.FLAGSIZE,
ROW_NUMBER() OVER(PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId ORDER BY IM.ReqDate DESC) RowNo
FROM  INDENTDETAIL ID WITH (NOLOCK)  Inner Join INDENTMASTER IM WITH (NOLOCK)   on IM.IndentId=ID.IndentId 
),
 ReceiveItem(IssueId,IndentId,EmpId,GodownId,IFinishedid,OFinishedId,ReceiveDate,RecQuantity,Moisture,IssueQuantity,RowNo) AS 
(Select PREMT.IssPrtId,PREMT.IndentId,PREM.EmpId,PREMT.GodownId,PRT.Finishedid IFinishedid,PREMT.FinishedId OFinishedId, 
PREMT.AddedDate as ReceiveDate,SUM(IsNull(PREMT.RecQuantity,0.00)) OVER (PARTITION BY PREMT.IndentId,PREMT.Orderdetailid,PREMT.FinishedId,PRT.Finishedid) RecQuantity,
SUM(IsNull(PREMT.Moisture,0.00)) OVER (PARTITION BY PREMT.IndentId,PREMT.Orderdetailid,PREMT.FinishedId,PRT.Finishedid) Moisture,
PRT.IssueQuantity,
ROW_NUMBER() OVER(PARTITION BY PREMT.IndentId,PREMT.Orderdetailid,PREMT.FinishedId,PRT.Finishedid ORDER BY PREMT.AddedDate DESC) RowNo
from  PP_ProcessRecMaster PREM WITH (NOLOCK)   inner join PP_ProcessRecTran  PREMT WITH (NOLOCK)  on PREM.PRMid=PREMT.PRMid 
inner join PP_PROCESSRAWTRAN PRT WITH (NOLOCK)   ON PREMT.IssPrtId=PRT.PRTid
--Where PRT.Indentid=44
),
 ConsumeItem(PPID,ProcessId,CompanyId,OrderId,OrderDetailId,IFinishedId,OFinishedId,ConsmpQty,LossQty, RequiredQty,RowNo) 
AS 
(SELECT PP.PPID,PP.Process_ID ProcessId,PP.MasterCompanyid,PP.Order_ID as OrderId,PPC.OrderDetailId,PPC.IFinishedId,PPC.FinishedId,
SUM(IsNull(PPC.ConsmpQty,0.00)) OVER (PARTITION BY PP.PPID,PP.Process_ID,PP.MasterCompanyid,PP.Order_ID,PPC.OrderDetailId,PPC.IFinishedId,PPC.FinishedId) ConsmpQty,
SUM(IsNull(PPC.LossQty,0.00)) OVER (PARTITION BY PP.PPID,PP.Process_ID,PP.MasterCompanyid,PP.Order_ID,PPC.OrderDetailId,PPC.IFinishedId,PPC.FinishedId) LossQty,
SUM(IsNull(PPC.Qty+PPC.ExtraQty,0.00)) OVER (PARTITION BY PP.PPID,PP.Process_ID,PP.MasterCompanyid,PP.Order_ID,PPC.OrderDetailId,PPC.IFinishedId,PPC.FinishedId) RequiredQty,
ROW_NUMBER() OVER(PARTITION BY PP.PPID,PP.Process_ID,PP.MasterCompanyid,PP.Order_ID,PPC.OrderDetailId,PPC.IFinishedId,PPC.FinishedId ORDER BY PPC.OrderDetailId DESC) RowNo
FROM ProcessProgram PP WITH (NOLOCK)  INNER JOIN PP_Consumption PPC WITH (NOLOCK)  ON PP.PPID=PPC.PPID 
--Where  PPC.PPId=2
),
ReturnItem (ReturnId,IssueId,IndentId,PartyId,IFinishedId,OFinishedId,GodownId,ReturnQty,TagRemarks,ReturnDate,RowNo) 
AS 
(
Select IRRM.ID,PREMT.IssPrtId,IPRRD.INDENTID,IRRM.PartyID,PRT.Finishedid IFinishedId,IPRRD.FinishedID OFinishedId,IPRRD.GodownID,
SUM(IsNull(IPRRD.Qty,0.00)) OVER (PARTITION BY IPRRD.INDENTID,PRT.Finishedid,IPRRD.FinishedID) ReturnQty,
IPRRD.Remarks,Max(IPRRD.Date) OVER (PARTITION BY IPRRD.INDENTID,PRT.Finishedid,IPRRD.FinishedID) ReturnDate,
ROW_NUMBER() OVER(PARTITION BY IPRRD.INDENTID,PRT.Finishedid,IPRRD.FinishedID ORDER BY IPRRD.Date DESC) RowNo
from IndentRawReturnDetail IPRRD WITH (NOLOCK)  inner join  IndentRawReturnMaster IRRM WITH (NOLOCK) ON  IPRRD.ID=IRRM.ID 
inner join PP_ProcessRecTran PREMT WITH (NOLOCK) on IPRRD.PRTID=PREMT.PRTid
inner join PP_PROCESSRAWTRAN PRT WITH (NOLOCK) ON PREMT.IssPrtId=PRT.PRTid
) 
select 
emp.EMPNAME VendorName, VF.ITEM_NAME+' '+VF.QUALITYNAME+' '+VF.DESIGNNAME+' '+VF.COLORNAME+' '+VF.SHAPENAME+' '+VF.ShadeColorName+' '+CASE WHEN x.FLAGSIZE=0 THEN VF.SIZEFT    
ELSE CASE WHEN x.FLAGSIZE=1 THEN VF.SIZEMTR ELSE VF.SIZEINCH END END + ' '+CASE WHEN VF.SIZEID>0 THEN ST.TYPE ELSE '' END MaterialName, 
VF.QUALITYNAME, VF.COLORNAME,VF.SHAPENAME,VF.SHADECOLORNAME,VF.CATEGORY_NAME Category,
z.OrderId,z.OrderDetailId,z.ConsmpQty,z.LossQty,z.ProcessId,z.RequiredQty,x.IndentId,x.IndentNo,x.IFinishedId,x.OFinishedId,
x.IndentQty,x.ExtraQty,x.CancelQty,x.Quantity,x.IssueDate,x.ReqDate,x.PartyId,y.ReceiveDate,y.RecQuantity,y.Moisture,y.IssueId,y.IssueQuantity,
zz.ReturnId,zz.ReturnDate,IsNull(zz.ReturnQty,0.00) ReturnQty,zz.TagRemarks
from IndentItem x Left join ReceiveItem y on x.Indentid=y.Indentid and x.IFinishedId=y.IFinishedId and x.OFinishedId=y.OFinishedId and x.RowNo=y.RowNo --and x.OrderDetailId=y.OrderDetailId
inner join ConsumeItem z on x.PPNo=z.PPID and x.IFinishedId=z.IFinishedId  and  x.OFinishedId=z.OFinishedId and x.RowNo=z.RowNo
Left join ReturnItem zz on y.Indentid=zz.Indentid and  y.IFinishedid=zz.IFinishedid and y.OFinishedid=zz.OFinishedid and y.RowNo=zz.RowNo 
INNER JOIN V_FINISHEDITEMDETAIL VF ON x.OFinishedId=VF.ITEM_FINISHED_ID   
INNER JOIN EMPINFO emp WITH (NOLOCK)  ON x.PartyId=emp.EmpId    
LEFT JOIN SIZETYPE ST WITH (NOLOCK)  ON x.FLAGSIZE=ST.VAL    
Where z.OrderId=@OrderId and z.ProcessId=@ProcessId and x.RowNo=1";

                try
                {

                    var result = conn.Query<IndentRawMaterialModel>(sqlQuery, new { @OrderId = OrderId, @ProcessId = ProcessId, }).GroupBy(n => new { n.IndentId, n.IFinishedId, n.OFinishedId }).
                        Select(x => new IndentRawMaterialModel
                        {
                            IndentId = x.Key.IndentId,
                            IFinishedId = x.Key.IFinishedId,
                            OFinishedId = x.Key.OFinishedId,
                            Category = x.FirstOrDefault().Category,
                            VendorName = x.FirstOrDefault().VendorName,
                            MaterialName = x.FirstOrDefault().MaterialName,
                            QualityName = x.FirstOrDefault().QualityName,
                            ColorName = x.FirstOrDefault().ColorName,
                            ShadeName = x.FirstOrDefault().ShadeName,
                            PPNo = x.FirstOrDefault().PPNo,
                            ProcessId = x.FirstOrDefault().ProcessId,
                            CompanyId = x.FirstOrDefault().CompanyId,
                            OrderId = x.FirstOrDefault().OrderId,
                            OrderDetailId = x.FirstOrDefault().OrderDetailId,
                            IndentNo = x.FirstOrDefault().IndentNo,
                            PartyId = x.FirstOrDefault().PartyId,
                            IndentQty = x.Sum(y => y.IndentQty),
                            ExtraQty = x.Sum(y => y.ExtraQty),
                            CancelQty = x.Sum(y => y.CancelQty),
                            IssueId = x.FirstOrDefault().IssueId,
                            Quantity = x.Sum(q => q.Quantity),
                            IssueQuantity = x.Select(y => new { y.IssueId, y.IssueQuantity }).Distinct().Sum(p => p.IssueQuantity),
                            RecQuantity = x.Sum(q => q.RecQuantity),
                            ConsmpQty = x.Sum(y => y.ConsmpQty),
                            Moisture = x.Sum(y => y.Moisture),
                            LossQty = x.Sum(y => y.LossQty),
                            ReturnQty = x.Sum(q => q.ReturnQty),
                            RequiredQty = x.Sum(y => y.RequiredQty),
                            ReturnId = x.FirstOrDefault().ReturnId,
                            TagRemarks = x.FirstOrDefault().TagRemarks,
                            ReqDate = x.FirstOrDefault().ReqDate,
                            IssueDate = x.FirstOrDefault().IssueDate,
                            ReceiveDate = x.Max(y => y.ReceiveDate),
                            ReturnDate = x.Max(y => y.ReturnDate),

                        });





                    //var lst = result.Select(x => new IndentRawMaterialModel
                    //{
                    //    Category = x.Category != null ? x.Category : string.Empty,
                    //    VendorName = x.VendorName != null ? x.VendorName : string.Empty,
                    //    MaterialName = x.MaterialName != null ? x.MaterialName : string.Empty,
                    //    QualityName = x.QualityName != null ? x.QualityName : string.Empty,
                    //    ColorName = x.ColorName != null ? x.ColorName : string.Empty,
                    //    ShadeName = x.ShadeName != null ? x.ShadeName : string.Empty,
                    //    PPNo = x.PPNo != null ? x.PPNo : 0,
                    //    ProcessId = x.ProcessId != null ? x.ProcessId : 0,
                    //    CompanyId = x.CompanyId != null ? x.CompanyId : 0,
                    //    OrderId = x.OrderId != null ? x.OrderId : 0,
                    //    OrderDetailId = x.OrderDetailId != null ? x.OrderDetailId : 0,
                    //    IndentNo = x.IndentNo != null ? x.IndentNo : string.Empty,
                    //    PartyId = x.PartyId != null ? x.PartyId : 0,
                    //    IFinishedId = x.IFinishedId != null ? x.IFinishedId : 0,
                    //    OFinishedId = x.OFinishedId != null ? x.OFinishedId : 0,
                    //    IssueDate = x.IssueDate != null ? x.IssueDate.ToString("dd MMM yyyy") : string.Empty,
                    //    ReqDate = x.ReqDate != null ? x.ReqDate.ToString("dd MMM yyyy") : string.Empty,
                    //    IndentQty = x.IndentQty != null ? (decimal)x.IndentQty : 0.0m,
                    //    ExtraQty = x.ExtraQty != null ? (decimal)x.ExtraQty : 0.0m,
                    //    CancelQty = x.CancelQty != null ? (decimal)x.CancelQty : 0.0m,
                    //    Quantity = x.Quantity != null ? (decimal)x.Quantity : 0.0m,
                    //    IssueId = x.IssueId != null ? x.IssueId : 0,
                    //    IssueQuantity = x.IssueQuantity != null ? (decimal)x.IssueQuantity : 0.0m,
                    //    ReceiveDate = x.ReceiveDate != null ? x.ReceiveDate.ToString("dd MMM yyyy") : string.Empty,
                    //    RecQuantity = x.RecQuantity != null ? (decimal)x.RecQuantity : 0.0m,
                    //    ConsmpQty = x.ConsmpQty != null ? (decimal)x.ConsmpQty : 0.0m,
                    //    Moisture = x.Moisture != null ? (decimal)x.Moisture : 0.0m,
                    //    LossQty = x.LossQty != null ? (decimal)x.LossQty : 0.0m,
                    //    RequiredQty = x.RequiredQty != null ? (decimal)x.RequiredQty : 0.0m,
                    //    ReturnId = x.ReturnId != null ? x.ReturnId : 0,
                    //    ReturnQty = x.ReturnQty != null ? (decimal)x.ReturnQty : 0.0m,
                    //    TagRemarks = x.TagRemarks != null ? x.TagRemarks : string.Empty,
                    //    ReturnDate = x.ReturnDate != null ? x.ReturnDate.ToString("dd MMM yyyy") : string.Empty,
                    //});



                    return (result);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }



















    }
}
