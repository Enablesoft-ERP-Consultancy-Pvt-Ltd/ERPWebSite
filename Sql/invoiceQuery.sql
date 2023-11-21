SELECT I.INVOICEID,CI.COMPANYNAME,CI.COMPADDR1,CI.COMPADDR2,CI.COMPADDR3,CI.COMPTEL,CI.TINNO,CI.PANNr, I.TINVOICENO,
REPLACE(CONVERT(NVARCHAR(11),I.INVOICEDATE,106),' ','-') AS INVOICEDATE,CI.IECODE,I.TORDERNO,CI.RBICode
,I.OTHERREF,DM.CONSIGNEE,DM.CONSIGNEE_ADDRESS,DM.NOTIFYPARTY,DM.NOTIFYPARTY_ADDRESS,DM.RECEIVER,DM.RECEIVER_ADDRESS,
DM.PAYINGAGENT,DM.PAYINGAGENT_ADDRESS,DM.BUYER_OTHERTHANCONSIGNEE,DM.OTHERTHANCONSIGNEE_ADDRESS,C.CARRIAGENAME AS PRE_CARRIAGEBY
,GR.STATIONNAME AS RECEIPTBY,TM.TRANSMODENAME AS VESSEL_FLIGHTNO,GR1.STATIONNAME AS PORTLOAD,I.PORTUNLOAD,
I.DESTINATIONADD,I.COUNTRYOFORIGIN,I.COUNTRYOFFINALDEST,P.PAYMENTNAME AS DELIVERYTERM,I.DESCRIPTIONOFGOODS,I.NOOFROLLS,I.CONTENTS,
I.GROSSWT,I.NETWT,I.VOLUME,I.DESTCODE,DM.NOTIFYPARTY2,DM.NOTIFYPARTY2_ADDRESS,DM.CONSIGNEE_DT,DM.NOTIFYPARTY_DT,DM.NOTIFYPARTY2_DT,CC.CURRENCYTYPEPS,
B.BANKNAME AS COMPANYBANK,B.STREET AS BANKSTREET,B.CITY AS BANKCITY,B.STATE AS BANKSTATE,B.COUNTRY,I.SBILLNO,
I.SBILLDATE,I.SHIPMENTID,I.TRUCKNO,REPLACE(CONVERT(NVARCHAR(11),I.DISPATCHDATE,106),' ','-') AS DISPATCHDATE,I.DELVWK,I.SEALNO,I.FREIGHTTERMS,
CI.GSTNO,ISNULL(CI.PANNR,'') AS PANNO,DM.REC_GSTIN,DM.REC_STATE,DM.REC_STATECODE,DM.REC_PANNO,DM.REC_CINNO,
ISNULL(I.CGST,0) AS CGST,ISNULL(I.SGST,0) AS SGST,ISNULL(I.IGST,0) AS IGST,B.ACNO,B.IFSCODE,DM.INVOICE_RECEIVER,DM.INVOICE_RECEIVERADD,I.LUTARNNO
FROM INVOICE I 
INNER JOIN PACKING PM ON I.PACKINGID=PM.PACKINGID
INNER JOIN COMPANYINFO CI ON I.CONSIGNORID=CI.COMPANYID
INNER JOIN DESTINATIONMASTER DM ON I.DESTCODE=DM.DESTCODE
LEFT JOIN CARRIAGE C ON I.PRECARRIER=C.CARRIAGEID
LEFT JOIN GOODSRECEIPT GR ON I.RECEIPT=GR.GOODSRECEIPTID
LEFT JOIN TRANSMODE TM ON I.SHIPINGID=TM.TRANSMODEID
LEFT JOIN GOODSRECEIPT GR1 ON I.PORTLOAD=GR1.GOODSRECEIPTID
LEFT JOIN PAYMENT P ON I.DELTERMS=P.PAYMENTID
LEFT JOIN CURRENCYINFO CC ON PM.CURRENCYID=CC.CURRENCYID
LEFT JOIN BANK B ON CI.BANKID=B.BANKID


*******************************************************************

SELECT I.INVOICEID, I.TINVOICENO, REPLACE(CONVERT(NVARCHAR(11), I.INVOICEDATE, 106), ' ', '-') INVOICEDATE, CI.COMPANYNAME, CI.COMPADDR1, CI.COMPADDR2, CI.COMPADDR3,       
CI.COMPTEL, CI.COMPFAX, CI.EMAIL, CI.GSTNO, I.TCONSIGNEE,       
I.TBUYEROCONSIGNEE, I.DESTINATIONADD FINALDESTINATION, ISNULL(PRECARRIAGEBY.CARRIAGENAME, '') PRECARRIAGEBY, ISNULL(BYAIRSEA.TRANSMODENAME, '') VESSELFLIGHTNO,       
ISNULL(PORTLOAD.STATIONNAME, '') PORTOFLOADING, ISNULL(I.PORTUNLOAD, '') PORTOFDISCHARGE, ISNULL(RECEIPTAT.STATIONNAME, '')  RECEIPTAT, ISNULL(PAYMENT.PAYMENTNAME, '') PAYMENTMODE,       
ISNULL(TERM.TERMNAME, '') PAYMENTTERMS, ISNULL(CUR.CURRENCYNAME, '') CURRENCY, ISNULL(CUR.CURRENCYTYPERS, '') CURRENCYTYPERS, ISNULL(CUR.CURRENCYTYPEPS, '') CURRENCYTYPEPS,       
VF.ITEM_NAME, PD.DESIGN, PD.COLOR, PD.WIDTH, PD.LENGTH, SUM(PD.PCS) PCS, PD.PRICE, SUM(PD.AREA) AREA, Round(SUM(CASE WHEN PD.CALTYPEAMT = 1 THEN PD.PRICE*PD.PCS ELSE PD.AREA*PD.PRICE  END),2) AMOUNT,       
SUM(TOTALROLL) NOOFROLLS, ISNULL(I.INSURANCE, 0) INSURANCE, ISNULL(I.FREIGHT, 0) FREIGHT, ISNULL(I.NETWT, 0) NETWT, ISNULL(I.GROSSWT, 0) GROSSWT,       
CASE WHEN U.UNITID = 1 THEN 'SQ MTR.' ELSE 'SQ. FT' END UNIT, CASE WHEN U.UNITID = 1 THEN 'CM' ELSE U.UNITNAME END SIZEUNIT, U.UNITID, ISNULL(SUM(PD.CBM), 0) CBM,       
ISNULL(I.IGST, 0) IGST, ISNULL(I.CGST, 0) CGST, ISNULL(I.SGST, 0) SGST, ISNULL(I.INRRate, 0) INRRate, ISNULL(I.descriptionofgoods, '') DescriptionOfGoods,       
CC.Mark, ISNULL(B.BankName, '') BankName, ISNULL(B.Street+' '+B.City+' '+B.State+' '+B.Country, '') BankAddress, ISNULL(B.ACNo, '') BankAccountNo, ISNULL(B.Ifscode, '') BankIfscCode,       
ISNULL(case when B.AccountType = 1 then 'Current' else case when B.AccountType = 2 then 'Saving' else case when B.AccountType = 3 then 'DrawBack' else '' end end end, '') AccountType,       
ISNULL(B.ContectPerson, '') ContactPerson, isnull(OM.CustomerOrderNo, '') CustomerOrderNo, ISNULL(CC.NotifyByAir, '') NotifyByAir, isnull(PD.RollFrom, 0) RollFrom,       
ISNULL(PD.RollTo, 0) RollTo, VF.ITEM_ID, PD.Id, isnull(VF.HSNCode, '') HSNCode, ISNULL(CI.IECode, '') IECode, ISNULL(CI.FactoryAddress, '') FactoryAddress,       
ISNULL(B.SwiftCode, '') SwiftCode, ISNULL(CI.RollMarkHead, '') RollMarkHead, isnull(SKU.Sku_no, '') SKUNO,skudesc, isnull(PD.SinglePcsNetWt, 0) SinglePcsNetWt,       
isnull(PD.SinglePcsGrossWt, 0) SinglePcsGrossWt, isnull(SUM(PD.TotalRoll)* isnull(PD.SinglePcsNetWt, 0), 0) TotalPcsNetWt, isnull(SUM(PD.TotalRoll)*ISNULL(PD.SinglePcsGrossWt, 0), 0) TotalPcsGrossWt,       
--isnull(PD.RatePerPcs, 0) RatePerPcs,      
Round((SUM(CASE WHEN PD.CALTYPEAMT = 1 THEN PD.PRICE/Pcs*PCS ELSE AREA/PCS*PD.PRICE  END)), 2) RatePerPcs,      
 VF.DesignName, isnull(CI.ExpRef, '') ExportRef, isnull(CI.TinNo, '') CompTinNo, ISNULL(OM.LocalOrder, '') LocalOrderNo,       
VF.designId, isnull(VPON.totorder, '') PackingBuyerOrderNo, ISNULL(PD.StyleNo, '') StyleNo, isnull(CI.EDPNo, '') ADCode, isnull(SHP.AgentName, '') ShippingAgentName,       
isnull(I.VesselName, '') VesselName, isnull(I.SealNo, '') SealNo, isnull(I.sbillno, '') ShippingBillNo, isnull(REPLACE(CONVERT(NVARCHAR(11), I.Sbilldate, 106), ' ', '-'), '') ShippingBillDate,       
isnull(I.Blno, '') AWBNo, IsNULL(REPLACE(CONVERT(NVARCHAR(11), I.Bldt, 106), ' ', '-'), '') AWBBillDate, isnull(I.ContainerNo, '') ContainerNo,       
isnull(sum(I.CBM), 0) TotalCBM, VF.SizeFT, sum(VF.AreaFT*PD.PCS) AreaFT, sum(VF.AreaMtr*PD.PCS) AreaMtr,       
Round((SUM(CASE WHEN PD.CALTYPEAMT = 1 THEN PD.PRICE*PCS ELSE AREA*PD.PRICE  END) / sum(VF.AreaFT*PD.PCS)), 4) RatePerSqFt,       
Round((SUM(CASE WHEN PD.CALTYPEAMT = 1 THEN PD.PRICE*PCS ELSE AREA*PD.PRICE  END) / sum(VF.AreaMtr*PD.PCS)), 4) RatePerSqMtr,      
isnull(CI.lutarnno,'') as LUTArnNo,isnull(LN.RexNo,'') as RexNo,LN.Registrationno,isnull(REPLACE(CONVERT(NVARCHAR(11), LN.RexIssueDate, 106), ' ', '-'), '') as RexIssueDate ,      
isnull(I.SUQty,'') as SUQty,isnull(I.PreferentialAgreement,'') as PreferentialAgreement,isnull(I.PackingCharges,0) as PackingCharges,isnull(I.FlightNo,'') as FlightNo,      
isnull(REPLACE(CONVERT(NVARCHAR(11), I.FlightDate, 106), ' ', '-'), '') FlightDate,isnull(REPLACE(CONVERT(NVARCHAR(11), I.LeoDate, 106), ' ', '-'), '') LeoDate,isnull(I.GstinType,'') as GstinType,      
isnull(IPO.Description,'') as Composition,isnull(IPO.HS_CODE,'') as FinishedIdHSCode,isnull(PD.Quality,'') as Quality,isnull(I.LessAdvance,0) as LessAdvance,isnull(I.DiscountAmt,0) as DiscountAmt,      
isnull(I.ExtraCharges,0) as ExtraCharges,isnull(B.ADCode,'') as BankADCode,VF.SizeMtr,      
CASE WHEN Month(I.INVOICEDATE) BETWEEN 4 AND 12 THEN CONVERT(VARCHAR(4),YEAR(I.INVOICEDATE)) + '-' + CONVERT(VARCHAR(4),YEAR(I.INVOICEDATE) + 1)      
     WHEN Month(I.INVOICEDATE) BETWEEN 1 AND 3  THEN CONVERT(VARCHAR(4),YEAR(I.INVOICEDATE) - 1) + '-' + CONVERT(VARCHAR(4),YEAR(I.INVOICEDATE) )End as FinancialYear ,  
  isnull(WH.WHConsignee,'') AS BILLTO,isnull(WH.WHShipTo,'') AS SHIPTO,isnull(WH.Warehousename,'') as Warehousename ,isnull(I.Ex1Rate,0) as Ex1Rate,isnull(I.Ex2Rate,0) as Ex2Rate,  
  isnull(I.Composition,'') as InvoiceComposition,REPLACE(CONVERT(NVARCHAR(11), I.INVOICEDATE, 105), ' ', '-') as INVOICEDATE1,isnull(PD.RPcs,0) as PcsPerRoll,  
  isnull(I.RollMark,'') as InvoiceRollMark,isnull(PD.L,'') as BaleLength,isnull(PD.W,'') as BaleWidth,isnull(PD.H,'') as BaleHeight,  
  isnull(I.ShipToAddress,'') as InvoiceShipToAddress,isnull(PD.PurchaseCode,'') as DPCItem,  
  REPLACE(CONVERT(NVARCHAR(11), PM.PackingDATE, 106), ' ', '-') PackingDATE  
  , Case When I.SHIPINGID=1 Then CC.NotifyByAir else case when I.SHIPINGID=2 then CC.NotifyBySea else '' End End as NotifyParty  
  ,isnull(SKU.Composition,'') as FinishedIdComposition,isnull(CM.CountryName,'') as CustomerCountryName,isnull(PD.RugId,'') as RugID  
  --,min(PD.Rollfrom) as MinRollno,Max(PD.RollTo) as Maxrollno  
FROM PACKING PM(NoLock)       
INNER JOIN PACKINGINFORMATION PD(NoLock) ON PM.PACKINGID = PD.PACKINGID       
INNER JOIN V_FINISHEDITEMDETAIL VF(NoLock) ON PD.FINISHEDID = VF.ITEM_FINISHED_ID       
INNER JOIN INVOICE I(NoLock) ON PM.PACKINGID = I.PACKINGID       
INNER JOIN COMPANYINFO CI(NoLock) ON I.CONSIGNORID = CI.COMPANYID       
INNER JOIN Bank B(NoLock) ON CI.BankId = B.BankId       
INNER JOIN CUSTOMERINFO CC(NoLock) ON PM.CONSIGNEEID = CC.CUSTOMERID       
LEFT JOIN CustomerShippingAgent CSA(NoLock) ON CC.CustomerId = CSA.CustomerId   
LEFT JOIN Shipp SHP(NoLock) ON CSA.ShippingAgentId = SHP.AgentId       
LEFT JOIN CARRIAGE  PRECARRIAGEBY(NoLock) ON  I.PRECARRIER = PRECARRIAGEBY.CARRIAGEID       
LEFT JOIN TRANSMODE BYAIRSEA(NoLock) ON I.SHIPINGID = BYAIRSEA.TRANSMODEID       
LEFT JOIN GOODSRECEIPT PORTLOAD(NoLock) ON I.PORTLOAD = PORTLOAD.GOODSRECEIPTID       
LEFT JOIN PAYMENT(NoLock) ON I.DELTERMS = PAYMENT.PAYMENTID       
LEFT JOIN TERM(NoLock)  ON I.CREDITID = TERM.TERMID       
LEFT JOIN CURRENCYINFO CUR(NoLock) ON I.currencytype = CUR.CURRENCYID       
INNER JOIN UNIT U(NoLock) ON PM.UNITID = U.UNITID       
LEFT JOIN GOODSRECEIPT RECEIPTAT(NoLock) ON I.RECEIPT = RECEIPTAT.GOODSRECEIPTID       
LEFT JOIN OrderMaster OM(NoLock) ON PD.OrderId = OM.OrderId       
LEFT JOIN sku_no SKU(NoLock) ON PD.FinishedId = SKU.finished_id       
INNER JOIN V_PackingOrderNo VPON(NoLock) ON PM.PackingId = VPON.Packingid       
LEFT JOIN Addlegalinformation LN(NoLock) ON CI.CompanyId=LN.CompanyId      
LEFT JOIN WareHouseMaster WH(NoLock) ON  WH.CustomerId  =I.cosigneeId  
LEFT JOIN Item_Parameter_Other IPO(NoLock) ON PD.FINISHEDID=IPO.Item_Finished_Id   
LEFT JOIN CountryMaster CM ON CC.Country=CM.CountryId  
GROUP BY I.INVOICEID, I.TINVOICENO, I.INVOICEDATE, CI.COMPANYNAME, CI.COMPADDR1, CI.COMPADDR2, CI.COMPADDR3, CI.COMPTEL, CI.COMPFAX, CI.EMAIL, CI.GSTNO, I.TCONSIGNEE,       
I.TBUYEROCONSIGNEE, I.DESTINATIONADD, PRECARRIAGEBY.CARRIAGENAME, BYAIRSEA.TRANSMODENAME, PORTLOAD.STATIONNAME, I.PORTUNLOAD, PAYMENT.PAYMENTNAME, TERM.TERMNAME,       
CUR.CURRENCYNAME, CUR.CURRENCYTYPERS, CUR.CURRENCYTYPEPS, VF.ITEM_NAME, PD.DESIGN, PD.COLOR, PD.WIDTH, PD.LENGTH, PD.PRICE, I.INSURANCE, I.FREIGHT, I.NETWT, I.GROSSWT,       
U.UNITID, U.UNITNAME, RECEIPTAT.STATIONNAME, I.IGST, I.CGST, I.SGST, I.INRRate, I.descriptionofgoods, CC.Mark, B.BankName, B.Street, B.City, B.State, B.Country, B.ACNo,       
B.Ifscode, B.AccountType, B.ContectPerson, OM.CustomerOrderNo, CC.NotifyByAir, PD.RollFrom, PD.RollTo, VF.ITEM_ID, PD.Id, VF.HSNCode, CI.IECode, CI.FactoryAddress,       
B.SwiftCode, CI.RollMarkHead, SKU.Sku_no,skudesc, PD.SinglePcsNetWt, PD.SinglePcsGrossWt, PD.RatePerPcs, VF.DesignName, CI.ExpRef, CI.TinNo, OM.LocalOrder, VF.designId,       
VPON.totorder, PD.StyleNo, CI.EDPNo, SHP.AgentName, I.VesselName, I.SealNo, I.sbillno, I.Sbilldate, I.Blno, I.Bldt, I.ContainerNo, VF.SizeFT,CI.lutarnno,LN.RexNo,LN.RexIssueDate,      
I.SUQty,I.PreferentialAgreement,I.PackingCharges,I.FlightNo,I.FlightDate,I.LeoDate,I.GstinType,IPO.Description,IPO.HS_CODE,PD.Quality,I.LessAdvance,I.DiscountAmt,I.ExtraCharges,B.ADCode,      
VF.SizeMtr,WH.WHConsignee,WH.WHShipTo,WH.Warehousename,I.Ex1Rate,LN.Registrationno,I.Ex2Rate,I.Composition, PD.RPcs,I.RollMark,PD.L,PD.W,PD.H,  
I.ShipToAddress,PD.PurchaseCode,PM.PackingDATE,I.SHIPINGID,CC.NotifyBySea,SKU.Composition,CM.CountryName,PD.RugId 



***************************************************************************************************************


select CI.Companyname,CI.CompAddr1,CI.CompAddr2,CI.CompAddr3,ci.TinNo,I.TinvoiceNo,I.InvoiceDate,Om.customerorderno,
CI.IECode,I.TConsignee,I.TBuyerOConsignee,I.DestinationAdd,CM.CarriageName as PreCarriage,
GR.StationName as ReceiptAt,GR1.StationName as PortLoad,i.portunload,P.PaymentName as Deliveryterm,
T.TermName as Terms,PD.RollFrom,PD.RollTo,vf.designName,vf.CATEGORY_NAME,vf.ITEM_NAME,
case when isnull(Pwd.Sizeflag,0)=0 Then vf.SizeFt When PWd.Sizeflag=1 Then vf.SizeMtr When Pwd.Sizeflag=2 Then vf.sizeinch End as Size,
isnull(St.Type,'FT') as Sizetype,vf.ColorName,pd.TotalRoll,Pd.TotalPcs,pwd.Netwtperpcs,pwd.Netwtfabric,pwd.NetwtBeads,
pwd.Totalnetwtfabric,pwd.totalnetwtbeads,pwd.Totalnetwt,Pwd.cartonwt,pwd.Totalgrosswt,I.InvoiceId,
gd.GoodsName,cc.CurrencyName,pd.price,Pd.CalTypeAmt,pd.Area,PD.cbm,pm.mastercompanyid
,PD.Barcode,Pd.Description,PD.L,PD.W,PD.H,I.blno,I.Bldt,I.VesselName,I.FircNo,
B.BankName,B.Adcode,B.Street,B.City,B.Country,B.PhoneNo,B.ACNo,B.SwiftCode,B.accountname,B.postcode
from Packing PM inner join PackingInformation PD on Pm.PackingId=Pd.PackingId
inner join Invoice I on PM.PackingId=I.PackingId
inner join CompanyInfo CI on I.consignorId=CI.CompanyId
left join carriage CM on I.PreCarrier=CM.CarriageId
left join GoodsReceipt GR on I.Receipt=GR.GoodsreceiptId
left join GoodsReceipt GR1 on I.PortLoad=GR1.GoodsreceiptId
left join Payment P on I.DelTerms=P.PaymentId
left join Term T on I.Terms=T.TermId
left join OrderMaster Om on PD.OrderId=OM.OrderId
inner join V_FinishedItemDetail vf on Pd.FinishedId=vf.ITEM_FINISHED_ID
left join PackingWeightDetail PWD on Pd.id=PWD.PackingDetailId
left join sizetype ST on Pwd.Sizeflag=ST.Val
left join GoodSdesc gd on i.goodsId=gd.GoodsId
left join CurrencyInfo CC on I.currencytype=CC.currencyid
left join bank B on I.bankid=B.bankid