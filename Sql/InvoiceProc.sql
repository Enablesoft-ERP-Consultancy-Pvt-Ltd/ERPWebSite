CREATE PROCEDURE [dbo].[InvoiceProc]        
@Flag int=null,        
@InvoiceId int=null       
        
AS        
BEGIN        
    
SELECT   
  
CONVERT(varchar,GetDate(),105) as CreatedOn,  
InvoiceItem.INVOICEID,        
InvoiceItem.TINVOICENO,  
CONVERT(varchar,InvoiceItem.INVOICEDATE,105) as INVOICEDATE,  
InvoiceItem.TCONSIGNEE,        
InvoiceItem.TBUYEROCONSIGNEE,        
InvoiceItem.DESTINATIONADD FINALDESTINATION,        
InvoiceItem.PORTUNLOAD as PORTOFDISCHARGE,        
InvoiceItem.INSURANCE,        
InvoiceItem.FREIGHT,       
        
CAST(ISNULL(InvoiceItem.NETWT, 0.00) AS DECIMAL(16,2)) as NETWT,        
CAST(ISNULL(InvoiceItem.GROSSWT, 0.00) AS DECIMAL(16,2)) as GROSSWT,        
CAST(ISNULL(InvoiceItem.IGST, 0.00) AS DECIMAL(16,2)) as IGST,        
CAST(ISNULL(InvoiceItem.CGST, 0.00) AS DECIMAL(16,2)) as CGST,        
CAST(ISNULL(InvoiceItem.SGST, 0.00) AS DECIMAL(16,2)) as SGST,        
        
CAST(ISNULL(InvoiceItem.INRRate, 0.00) AS DECIMAL(16,2)) as INRRate,        
        
CAST(ISNULL(InvoiceItem.Ex1Rate, 0.00) AS DECIMAL(16,2)) as Ex1Rate,        
CAST(ISNULL(InvoiceItem.Ex2Rate, 0.00) AS DECIMAL(16,2)) as Ex2Rate,        
CAST(ISNULL(InvoiceItem.ExtraCharges, 0.00) AS DECIMAL(16,2)) as ExtraCharges,        
CAST(ISNULL(InvoiceItem.CBM, 0.00) AS DECIMAL(16,2)) as CBM,        
        
ISNULL(InvoiceItem.Composition, '') as Composition,        
        
InvoiceItem.descriptionofgoods as DescriptionOfGoods,        
InvoiceItem.LessAdvance,        
InvoiceItem.DiscountAmt,        
InvoiceItem.SUQty,        
InvoiceItem.PreferentialAgreement,        
InvoiceItem.PackingCharges,        
InvoiceItem.FlightNo,        
InvoiceItem.FlightDate,        
InvoiceItem.LeoDate,        
InvoiceItem.GstinType,        
        
InvoiceItem.SHIPINGID,        
InvoiceItem.RollMark,        
InvoiceItem.ShipToAddress,        
        
InvoiceItem.VesselName,        
InvoiceItem.Blno,        
InvoiceItem.SealNo,        
InvoiceItem.sbillno,        
InvoiceItem.Sbilldate,        
InvoiceItem.Bldt,        
InvoiceItem.ContainerNo,        
PRECARRIAGEBY.CARRIAGENAME,      
DM.CONSIGNEE,DM.CONSIGNEE_ADDRESS,DM.NOTIFYPARTY,DM.NOTIFYPARTY_ADDRESS,    
DM.RECEIVER,DM.RECEIVER_ADDRESS,    
DM.PAYINGAGENT,DM.PAYINGAGENT_ADDRESS,DM.BUYER_OTHERTHANCONSIGNEE,    
DM.OTHERTHANCONSIGNEE_ADDRESS,    
BYAIRSEA.TRANSMODENAME as VESSELFLIGHTNO,        
PORTLOAD.STATIONNAME as PORTOFLOADING,        
CI.COMPANYNAME, CI.COMPADDR1, CI.COMPADDR2, CI.COMPADDR3, CI.COMPTEL, CI.COMPFAX, CI.EMAIL, CI.GSTNO,        
RECEIPTAT.STATIONNAME as RECEIPTAT,PAYMENT.PAYMENTNAME as PAYMENTMODE,TERM.TERMNAME as PAYMENTTERMS,        
CUR.CURRENCYNAME as CURRENCY, CUR.CURRENCYTYPERS as CURRENCYTYPERS,CUR.CURRENCYTYPEPS as CURRENCYTYPEPS,        
B.BankName,B.Ifscode,ISNULL(B.Street+' '+B.City+' '+B.State+' '+B.Country, '') BankAddress,B.ADCode,        
ISNULL(B.ACNo, '') BankAccountNo,B.ContectPerson,B.SwiftCode,B.AccountType,U.UNITID, U.UNITNAME,        
CC.Mark,CC.NotifyByAir,CI.IECode,CI.FactoryAddress,CI.RollMarkHead,        
CI.ExpRef,CI.TinNo,CI.EDPNo,        
CI.lutarnno as LUTArnNo,LN.RexNo,LN.Registrationno,LN.RexIssueDate,WH.WHConsignee,WH.WHShipTo,WH.Warehousename,        
PM.PackingDATE,CC.NotifyBySea,        
isnull(CM.CountryName,'') as CustomerCountryName,        
SHP.AgentName,VPON.totorder,        
        
(Select        
PD.Id,        
PD.RollNo,        
PD.Rollfrom,        
PD.RollTo,        
PD.RPcs,        
PD.TotalPcs,        
PD.TotalRoll,        
PD.OrderId,        
PD.RugId,        
PD.DESIGN,        
PD.COLOR,        
PD.WIDTH,        
PD.LENGTH,        
PD.PCS,        
CAST(PD.PRICE AS DECIMAL(16,2)) as PRICE,        
CAST(PD.AREA AS DECIMAL(16,2)) AS AREA,        
CAST(PD.CBM AS DECIMAL(16,2)) AS CBM,        
PD.CALTYPEAMT,        
        
        
PD.PurchaseCode,PD.L,PD.W,PD.H,PD.Quality,        
CAST(PD.SinglePcsNetWt AS DECIMAL(16,2)) AS SinglePcsNetWt,        
CAST(PD.SinglePcsGrossWt AS DECIMAL(16,2)) AS SinglePcsGrossWt,        
PD.StyleNo,        
OM.OrderId,        
OM.CustomerOrderNo,        
OM.LocalOrder,        
VF.ITEM_NAME,VF.DesignName,VF.designId, VF.SizeFT,VF.SizeMtr,VF.ITEM_ID, VF.HSNCode,        
CAST(VF.AreaFT AS DECIMAL(16,2)) AS AreaFT,        
CAST(VF.AreaMtr AS DECIMAL(16,2)) AS AreaMtr,        
SKU.Sku_no as SKUNO,SKU.skudesc,SKU.Composition,        
IPO.Description,        
IPO.HS_CODE as FinishedIdHSCode,        
CAST(PD.PCS*PD.PRICE AS DECIMAL(16,2)) as TotalAmount        
from PACKINGINFORMATION PD(NoLock)        
INNER JOIN OrderMaster OM(NoLock) ON PD.OrderId = OM.OrderId        
INNER JOIN V_FINISHEDITEMDETAIL VF(NoLock) ON PD.FINISHEDID = VF.ITEM_FINISHED_ID        
LEFT JOIN sku_no SKU(NoLock) ON PD.FinishedId = SKU.finished_id        
LEFT JOIN Item_Parameter_Other IPO(NoLock) ON PD.FINISHEDID=IPO.Item_Finished_Id        
where PD.PACKINGID = PM.PackingId --multi        
ORDER BY PD.RollNo        
FOR XML PATH('RollItem'),TYPE,Root('Rolls')        
)        
        
FROM INVOICE InvoiceItem(NoLock) INNER JOIN PACKING PM(NoLock) ON InvoiceItem.PACKINGID =PM.PACKINGID        
LEFT JOIN CARRIAGE PRECARRIAGEBY(NoLock) ON InvoiceItem.PRECARRIER = PRECARRIAGEBY.CARRIAGEID        
LEFT JOIN TRANSMODE BYAIRSEA(NoLock) ON InvoiceItem.SHIPINGID = BYAIRSEA.TRANSMODEID        
LEFT JOIN GOODSRECEIPT PORTLOAD(NoLock) ON InvoiceItem.PORTLOAD = PORTLOAD.GOODSRECEIPTID        
LEFT JOIN PAYMENT(NoLock) ON InvoiceItem.DELTERMS = PAYMENT.PAYMENTID        
LEFT JOIN TERM(NoLock) ON InvoiceItem.CREDITID = TERM.TERMID        
LEFT JOIN CURRENCYINFO CUR(NoLock) ON InvoiceItem.currencytype = CUR.CURRENCYID        
LEFT JOIN WareHouseMaster WH(NoLock) ON InvoiceItem.cosigneeId =WH.CustomerId        
LEFT JOIN GOODSRECEIPT RECEIPTAT(NoLock) ON InvoiceItem.RECEIPT = RECEIPTAT.GOODSRECEIPTID        
LEFT JOIN COMPANYINFO CI(NoLock) ON InvoiceItem.CONSIGNORID = CI.COMPANYID        
LEFT JOIN Addlegalinformation LN(NoLock) ON CI.CompanyId=LN.CompanyId        
LEFT JOIN Bank B(NoLock) ON CI.BankId = B.BankId        
LEFT JOIN DESTINATIONMASTER DM ON InvoiceItem.DESTCODE=DM.DESTCODE       
LEFT JOIN CUSTOMERINFO CC(NoLock) ON PM.CONSIGNEEID = CC.CUSTOMERID        
LEFT JOIN CountryMaster CM ON CC.Country=CM.CountryId        
LEFT JOIN CustomerShippingAgent CSA(NoLock) ON CC.CustomerId = CSA.CustomerId        
LEFT JOIN Shipp SHP(NoLock) ON CSA.ShippingAgentId = SHP.AgentId        
/********************************************************************************************************************/        
LEFT JOIN UNIT U(NoLock) ON PM.UNITID = U.UNITID        
INNER JOIN V_PackingOrderNo VPON(NoLock) ON PM.PackingId = VPON.Packingid        
Where InvoiceItem.InvoiceId=@InvoiceId        
ORDER BY InvoiceItem.InvoiceId    
FOR XML PATH ('InvoiceItem'),ROOT('InvoiceList') , ELEMENTS XSINIL;       
  
    
    
  
        
END  