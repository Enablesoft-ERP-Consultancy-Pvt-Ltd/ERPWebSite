Declare @OrderId int=204 , @ProcessId int=1;
With IssueItem(IssueId,DetailId,IssueNo,ProcessId,EmpId,OrderId,Finishedid,MaterialId,Rate,IssueQuantity,
AssignDate,RequestDate,FlagSize,RowNo)
AS(Select HFOM.ISSUEORDERID,HFOD.IssueDetailId,HFOM.CHALLANNO,EMP1.ProcessId,EMP1.EmpID,HFOD.OrderID,
HFOD.Order_FinishedID,HFOD.OrderDetailDetail_FinishedID,HFOD.Rate,
SUM(IsNull((HFOD.Qty-HFOD.CancelQty),0.00)) OVER (PARTITION BY HFOD.OrderID,HFOD.Order_FinishedID,HFOM.ISSUEORDERID,HFOD.OrderDetailDetail_FinishedID) IssueQuantity,
HFOM.ASSIGNDATE,HFOD.ReqByDate,HFOD.Exportsizeflag,
ROW_NUMBER() OVER(PARTITION BY  HFOD.OrderID,HFOD.Order_FinishedID,HFOM.ISSUEORDERID,HFOD.OrderDetailDetail_FinishedID ORDER BY HFOM.ISSUEORDERID) RowNo
From HomeFurnishingOrderMaster HFOM(NoLock) 
Inner JOIN HomeFurnishingOrderDetail HFOD(NoLock) 
ON HFOM.IssueOrderId=HFOD.IssueOrderId
Inner JOIN Employee_HomeFurnishingOrderMaster EMP1(nolock) ON 
EMP1.IssueDetailID =HFOD.IssueDetailId 
),
ReceiveItem(IssueId,ReceiveId,ProcessId,OrderId,Finishedid,MaterialId,
Rate,RecQuantity,ReceiveDate,RowNo)
As(Select HFRD.IssueOrderId, HFRD.ProcessRecDetailId, HFRM.ProcessID,HFRD.OrderId,
HFRD.Order_FinishedID,HFRD.OrderDetailDetail_FinishedID,
AVG(IsNull(HFRD.Rate,0.00)) OVER (PARTITION BY  HFRD.OrderId,HFRD.Order_FinishedID,HFRD.OrderDetailDetail_FinishedID,HFRD.ISSUEORDERID ) Rate,
SUM(IsNull(HFRD.Qty,0.00)) OVER (PARTITION BY  HFRD.OrderId,HFRD.Order_FinishedID,HFRD.OrderDetailDetail_FinishedID,
HFRD.ISSUEORDERID ) recQuantity,
Max(HFRM.ReceiveDate) OVER (PARTITION BY  HFRD.OrderId,HFRD.Order_FinishedID,HFRD.OrderDetailDetail_FinishedID,HFRD.ISSUEORDERID ) Rate,
ROW_NUMBER() OVER(PARTITION BY  HFRD.OrderId,HFRD.Order_FinishedID,HFRD.OrderDetailDetail_FinishedID,HFRD.ISSUEORDERID ORDER BY HFRD.ISSUEORDERID) RowNo
From HomeFurnishingReceiveMaster HFRM(NoLock)   
Inner JOIN HomeFurnishingReceiveDetail HFRD(NoLock) ON HFRM.ProcessRecId=HFRD.ProcessRecId 
)
Select 
emp.EMPNAME VendorName,ISNULL(D.DesignName, '') DesignName, 
VF.ITEM_NAME+' '+VF.QUALITYNAME+' '+VF.DESIGNNAME+' '+VF.COLORNAME+' '+VF.SHAPENAME+' '+VF.ShadeColorName+' '+CASE WHEN x.FLAGSIZE=0 THEN VF.SIZEFT    
ELSE CASE WHEN x.FLAGSIZE=1 THEN VF.SIZEMTR ELSE VF.SIZEINCH END END + ' '+CASE WHEN VF.SIZEID>0 THEN ST.TYPE ELSE '' END MaterialName, 
x.IssueId,x.IssueNo,x.ProcessId,x.EmpId,x.OrderId,x.Finishedid,x.MaterialId,
x.AssignDate,x.RequestDate,y.ReceiveDate,x.Rate,x.IssueQuantity,
IsNUll(y.RecQuantity,0.00) RecQuantity
from IssueItem x 
Left Join ReceiveItem y on  x.OrderId=y.OrderId and x.Finishedid=y.Finishedid 
and x.MaterialId=y.MaterialId and x.IssueId=y.IssueId and x.RowNo=y.RowNo
INNER JOIN V_FINISHEDITEMDETAIL VF ON x.MaterialId=VF.ITEM_FINISHED_ID  
LEFT JOIN SIZETYPE ST WITH (NOLOCK)  ON x.FLAGSIZE=ST.VAL  
INNER JOIN EMPINFO emp WITH (NOLOCK)  ON x.EmpId=emp.EmpId    
inner join ITEM_PARAMETER_MASTER IPM WITH(Nolock) on x.Finishedid=IPM.ITEM_FINISHED_ID
Left JOIN Design D WITH (Nolock) ON IPM.DESIGN_ID =D.DesignId 

Where  x.OrderId= @OrderId  and x.RowNo=1 and  x.ProcessId=@ProcessId 
Order BY  x.IssueId