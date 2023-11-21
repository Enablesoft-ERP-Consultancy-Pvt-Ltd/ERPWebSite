--Select OrderId from OrderMaster where CustomerOrderNo='For testing'
Declare @OrderId int=204 , @ProcessId int=5;
Declare @OrderId int=204 , @ProcessId int=5;
WITH IndentItem(PPNo,ProcessId,OrderId,OrderDetailId,IndentNo,IndentId,PartyId,IFinishedId,OFinishedId,IssueDate,ReqDate,IndentQty,ExtraQty,CancelQty,Quantity,FLAGSIZE,RowNo) AS 
(SELECT ID.PPNo,IM.ProcessID,ID.ORDERID,ID.Orderdetailid,IM.IndentNo,IM.IndentId,IM.PartyId,Id.IFinishedId,Id.OFinishedId,IM.Date IssueDate,IM.ReqDate,
SUM(IsNull(ID.IndentQty,0.00)) OVER (PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId) IndentQty,
SUM(IsNull(ID.ExtraQty,0.00)) OVER (PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId) ExtraQty,
SUM(IsNull(ID.CancelQty,0.00)) OVER (PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId) CancelQty,
SUM(IsNull(ID.Quantity,0.00)) OVER (PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId) Quantity,Id.FLAGSIZE,
ROW_NUMBER() OVER(PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId ORDER BY IM.ReqDate DESC) RowNo
FROM  INDENTDETAIL ID WITH (NOLOCK)  Inner Join INDENTMASTER IM WITH (NOLOCK)   on IM.IndentId=ID.IndentId 
Where ID.ORDERID=204 and IM.ProcessID=5
)
Select * from IndentItem x Where x.RowNo=1

WITH IssueItem(IndentId,IssueId,PRTid,ProcessId,EmpId,GodownId,IFinishedId,IssueQuantity,IssueDate,RowNo) 
AS (
Select PRT.IndentId,Prm.PRMid IssueId,PRT.PRTid,PRM.ProcessId,PRM.EmpId,PRT.GodownId,PRT.FinishedId, 
SUM(IsNull(PRT.IssueQuantity,0.00)) OVER (PARTITION BY PRT.IndentId,PRT.FinishedId) IssueQuantity,PRM.Date as IssueDate ,
ROW_NUMBER() OVER(PARTITION BY PRT.IndentId,PRT.FinishedId ORDER BY PRT.IndentId DESC) RowNo
from PP_PROCESSRAWTRAN PRT inner join PP_ProcessRawMaster PRM   on PRM.PRMid=PRT.PRMid 
Where PRM.ProcessId=@ProcessId and  PRT.IndentId IN (Select distinct ID.IndentId FROM  INDENTDETAIL ID Where ID.ORDERID=@OrderId))
Select * from IssueItem x Where x.RowNo=1 order by x.IndentId,x.IFinishedId 
;WITH ReceiveItem(PRTid,PRMid,IssueId,IndentId,EmpId,GodownId,OFinishedId,ReceiveDate,RecQuantity,Moisture,RowNo) AS 
(
Select PREMT.PRTid,PREM.PRMid,PREMT.IssPrmID,PREMT.IndentId,PREM.EmpId,PREMT.GodownId,PREMT.FinishedId OFinishedId, 
PREMT.AddedDate as ReceiveDate,SUM(IsNull(PREMT.RecQuantity,0.00)) OVER (PARTITION BY PREMT.IndentId,PREMT.Orderdetailid,PREMT.FinishedId) RecQuantity,
SUM(IsNull(PREMT.Moisture,0.00)) OVER (PARTITION BY PREMT.IndentId,PREMT.Orderdetailid,PREMT.FinishedId) Moisture,
ROW_NUMBER() OVER(PARTITION BY PREMT.IndentId,PREMT.Orderdetailid,PREMT.FinishedId ORDER BY PREMT.AddedDate DESC) RowNo
from  PP_ProcessRecMaster PREM WITH (NOLOCK)   inner join PP_ProcessRecTran  PREMT WITH (NOLOCK)  on PREM.PRMid=PREMT.PRMid 
Where PREM.ProcessId=@ProcessId and PREM.OrderId=@OrderId
)Select * from ReceiveItem x Where x.RowNo=1 
order by x.OFinishedId 


--WITH IndentItem(PPNo,ProcessId,OrderId,OrderDetailId,IndentNo,IndentId,PartyId,IFinishedId,OFinishedId,IssueDate,ReqDate,IndentQty,ExtraQty,CancelQty,Quantity,FLAGSIZE,RowNo) AS 
--(SELECT ID.PPNo,IM.ProcessID,ID.ORDERID,ID.Orderdetailid,IM.IndentNo,IM.IndentId,IM.PartyId,Id.IFinishedId,Id.OFinishedId,IM.Date IssueDate,IM.ReqDate,
--SUM(IsNull(ID.IndentQty,0.00)) OVER (PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId) IndentQty,
--SUM(IsNull(ID.ExtraQty,0.00)) OVER (PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId) ExtraQty,
--SUM(IsNull(ID.CancelQty,0.00)) OVER (PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId) CancelQty,
--SUM(IsNull(ID.Quantity,0.00)) OVER (PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId) Quantity,Id.FLAGSIZE,
--ROW_NUMBER() OVER(PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId ORDER BY IM.ReqDate DESC) RowNo
--FROM  INDENTDETAIL ID WITH (NOLOCK)  Inner Join INDENTMASTER IM WITH (NOLOCK)   on IM.IndentId=ID.IndentId 
--Where ID.ORDERID=@OrderId and IM.ProcessID=@ProcessId
--)
--Select * from IndentItem x Where x.RowNo=1
--order by x.IFinishedId ,x.OFinishedId
-- WITH ConsumeItem(PPID,ProcessId,CompanyId,OrderId,OrderDetailId,IFinishedId,OFinishedId,ConsmpQty,LossQty, RequiredQty,RowNo) 
--AS 
--(SELECT PP.PPID,PP.Process_ID ProcessId,PP.MasterCompanyid,PP.Order_ID as OrderId,PPC.OrderDetailId,PPC.IFinishedId,PPC.FinishedId,
--SUM(IsNull(PPC.ConsmpQty,0.00)) OVER (PARTITION BY PP.PPID,PP.Process_ID,PP.MasterCompanyid,PP.Order_ID,PPC.OrderDetailId,PPC.IFinishedId,PPC.FinishedId) ConsmpQty,
--SUM(IsNull(PPC.LossQty,0.00)) OVER (PARTITION BY PP.PPID,PP.Process_ID,PP.MasterCompanyid,PP.Order_ID,PPC.OrderDetailId,PPC.IFinishedId,PPC.FinishedId) LossQty,
--SUM(IsNull(PPC.Qty+PPC.ExtraQty,0.00)) OVER (PARTITION BY PP.PPID,PP.Process_ID,PP.MasterCompanyid,PP.Order_ID,PPC.OrderDetailId,PPC.IFinishedId,PPC.FinishedId) RequiredQty,
--ROW_NUMBER() OVER(PARTITION BY PP.PPID,PP.Process_ID,PP.MasterCompanyid,PP.Order_ID,PPC.OrderDetailId,PPC.IFinishedId,PPC.FinishedId ORDER BY PPC.OrderDetailId DESC) RowNo
--FROM ProcessProgram PP WITH (NOLOCK)  INNER JOIN PP_Consumption PPC WITH (NOLOCK)  ON PP.PPID=PPC.PPID 
--Where PP.Order_ID=@OrderId and PP.Process_ID=@ProcessId
--)
--Select * from ConsumeItem x Where x.RowNo=1
--order by x.IFinishedId ,x.OFinishedId


















WITH IndentItem(PPNo,ProcessId,OrderId,OrderDetailId,IndentNo,IndentId,PartyId,IFinishedId,OFinishedId,IssueDate,ReqDate,IndentQty,ExtraQty,CancelQty,Quantity,FLAGSIZE,RowNo) AS 
(SELECT ID.PPNo,IM.ProcessID,ID.ORDERID,ID.Orderdetailid,IM.IndentNo,IM.IndentId,IM.PartyId,Id.IFinishedId,Id.OFinishedId,IM.Date IssueDate,IM.ReqDate,
SUM(IsNull(ID.IndentQty,0.00)) OVER (PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId) IndentQty,
SUM(IsNull(ID.ExtraQty,0.00)) OVER (PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId) ExtraQty,
SUM(IsNull(ID.CancelQty,0.00)) OVER (PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId) CancelQty,
SUM(IsNull(ID.Quantity,0.00)) OVER (PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId) Quantity,Id.FLAGSIZE,
ROW_NUMBER() OVER(PARTITION BY ID.PPNo,ID.IndentId,ID.Orderdetailid,Id.IFinishedId,Id.OFinishedId ORDER BY IM.ReqDate DESC) RowNo
FROM  INDENTDETAIL ID WITH (NOLOCK)  Inner Join INDENTMASTER IM WITH (NOLOCK)   on IM.IndentId=ID.IndentId 
Where ID.ORDERID=@OrderId and IM.ProcessID=@ProcessId
),
IssueItem(IndentId,IssueId,ProcessId,EmpId,GodownId,IFinishedId,IssueQuantity,IssueDate,RowNo) 
AS (
Select PRT.IndentId,Prm.PRMid IssueId,PRM.ProcessId,PRM.EmpId,PRT.GodownId,PRT.FinishedId, 
SUM(IsNull(PRT.IssueQuantity,0.00)) OVER (PARTITION BY PRT.IndentId,PRT.FinishedId) IssueQuantity,PRM.Date as IssueDate ,
ROW_NUMBER() OVER(PARTITION BY PRT.IndentId,PRT.FinishedId ORDER BY PRT.IndentId DESC) RowNo
from PP_PROCESSRAWTRAN PRT inner join PP_ProcessRawMaster PRM   on PRM.PRMid=PRT.PRMid 
Where PRM.ProcessId=@ProcessId
),
ReceiveItem(IssueId,IndentId,EmpId,GodownId,OFinishedId,ReceiveDate,RecQuantity,Moisture,RowNo) AS 
(
Select PREMT.IssPrmID,PREMT.IndentId,PREM.EmpId,PREMT.GodownId,PREMT.FinishedId OFinishedId, 
PREMT.AddedDate as ReceiveDate,SUM(IsNull(PREMT.RecQuantity,0.00)) OVER (PARTITION BY PREMT.IndentId,PREMT.Orderdetailid,PREMT.FinishedId) RecQuantity,
SUM(IsNull(PREMT.Moisture,0.00)) OVER (PARTITION BY PREMT.IndentId,PREMT.Orderdetailid,PREMT.FinishedId) Moisture,
ROW_NUMBER() OVER(PARTITION BY PREMT.IndentId,PREMT.Orderdetailid,PREMT.FinishedId ORDER BY PREMT.AddedDate DESC) RowNo
from  PP_ProcessRecMaster PREM WITH (NOLOCK)   inner join PP_ProcessRecTran  PREMT WITH (NOLOCK)  on PREM.PRMid=PREMT.PRMid 
Where PREM.ProcessId=@ProcessId
),
ConsumeItem(PPID,ProcessId,CompanyId,OrderId,OrderDetailId,IFinishedId,OFinishedId,ConsmpQty,LossQty, RequiredQty,RowNo) 
AS 
(SELECT PP.PPID,PP.Process_ID ProcessId,PP.MasterCompanyid,PP.Order_ID as OrderId,PPC.OrderDetailId,PPC.IFinishedId,PPC.FinishedId,
SUM(IsNull(PPC.ConsmpQty,0.00)) OVER (PARTITION BY PP.PPID,PP.Process_ID,PP.MasterCompanyid,PP.Order_ID,PPC.OrderDetailId,PPC.IFinishedId,PPC.FinishedId) ConsmpQty,
SUM(IsNull(PPC.LossQty,0.00)) OVER (PARTITION BY PP.PPID,PP.Process_ID,PP.MasterCompanyid,PP.Order_ID,PPC.OrderDetailId,PPC.IFinishedId,PPC.FinishedId) LossQty,
SUM(IsNull(PPC.Qty+PPC.ExtraQty,0.00)) OVER (PARTITION BY PP.PPID,PP.Process_ID,PP.MasterCompanyid,PP.Order_ID,PPC.OrderDetailId,PPC.IFinishedId,PPC.FinishedId) RequiredQty,
ROW_NUMBER() OVER(PARTITION BY PP.PPID,PP.Process_ID,PP.MasterCompanyid,PP.Order_ID,PPC.OrderDetailId,PPC.IFinishedId,PPC.FinishedId ORDER BY PPC.OrderDetailId DESC) RowNo
FROM ProcessProgram PP WITH (NOLOCK)  INNER JOIN PP_Consumption PPC WITH (NOLOCK)  ON PP.PPID=PPC.PPID 
Where PP.Order_ID=@OrderId and PP.Process_ID=@ProcessId
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
select IPM.ITEM_FINISHED_ID,ISNULL(D.DesignName, '') DesignName, 
emp.EMPNAME VendorName, VF.ITEM_NAME+' '+VF.QUALITYNAME+' '+VF.DESIGNNAME+' '+VF.COLORNAME+' '+VF.SHAPENAME+' '+VF.ShadeColorName+' '+CASE WHEN x.FLAGSIZE=0 THEN VF.SIZEFT    
ELSE CASE WHEN x.FLAGSIZE=1 THEN VF.SIZEMTR ELSE VF.SIZEINCH END END + ' '+CASE WHEN VF.SIZEID>0 THEN ST.TYPE ELSE '' END MaterialName, 
VF.QUALITYNAME, VF.COLORNAME,VF.SHAPENAME,VF.SHADECOLORNAME,VF.CATEGORY_NAME Category,
z.OrderId,z.OrderDetailId,z.ConsmpQty,z.LossQty,z.ProcessId,z.RequiredQty,x.IndentId,x.IndentNo,x.IFinishedId,x.OFinishedId,
x.IndentQty,x.ExtraQty,x.CancelQty,x.Quantity,x.IssueDate,x.ReqDate,x.PartyId,y.ReceiveDate,y.RecQuantity,y.Moisture,y.IssueId,p.IssueQuantity,
zz.ReturnId,zz.ReturnDate,IsNull(zz.ReturnQty,0.00) ReturnQty,zz.TagRemarks
from IndentItem x Left Join IssueItem p  On x.IndentId=p.IndentId and x.IFinishedId=p.IFinishedId and x.RowNo=p.RowNo 
Left join ReceiveItem y on p.Indentid=y.Indentid and p.IssueId=y.IssueId and  x.OFinishedId=y.OFinishedId  and x.RowNo=y.RowNo --and x.OrderDetailId=y.OrderDetailId
inner join ConsumeItem z on x.PPNo=z.PPID and x.IFinishedId=z.IFinishedId  and  x.OFinishedId=z.OFinishedId 
and x.RowNo=z.RowNo
Left join ReturnItem zz on y.Indentid=zz.Indentid and  y.OFinishedid=zz.OFinishedid and y.RowNo=zz.RowNo 
INNER JOIN V_FINISHEDITEMDETAIL VF ON x.OFinishedId=VF.ITEM_FINISHED_ID   
INNER JOIN EMPINFO emp WITH (NOLOCK)  ON x.PartyId=emp.EmpId    
LEFT JOIN SIZETYPE ST WITH (NOLOCK)  ON x.FLAGSIZE=ST.VAL
INNER JOIN OrderDetail ord WITH(Nolock)   ON z.OrderDetailId=ord.OrderDetailId
inner join ITEM_PARAMETER_MASTER IPM WITH(Nolock) on ord.Item_Finished_Id=IPM.ITEM_FINISHED_ID
Left JOIN Design D WITH (Nolock) ON IPM.DESIGN_ID =D.DesignId 
Where z.OrderId=@OrderId and z.ProcessId=@ProcessId and x.RowNo=1
Order By  x.IFinishedId,x.OFinishedId