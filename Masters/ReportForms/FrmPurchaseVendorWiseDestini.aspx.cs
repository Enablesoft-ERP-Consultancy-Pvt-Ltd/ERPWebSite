using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO.Compression;
using CrystalDecisions.CrystalReports;
using System.Configuration;
using System.Text;
public partial class Masters_ReportForms_FrmPurchaseVendorWiseDestini : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str1 = @"select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by CompanyName
                            select Distinct g.GoDownID,g.GodownName from GodownMaster g inner join PP_ProcessRawTran prm On g.GoDownID=prm.GoDownID
                            select Distinct PROCESS_NAME_ID,PROCESS_NAME from process_name_master p inner join PP_ProcessRawMaster prm On p.PROCESS_NAME_ID=prm.Processid
                            select Distinct ft.id,ft.FINISHED_TYPE_NAME from FINISHED_TYPE ft inner join V_IndentRawIssue vr On vr.O_FINISHED_TYPE_ID =ft.Id ";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str1);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "-Select Company-");
            UtilityModule.ConditionalComboFillWithDS(ref ddgodown, ds, 1, true, "-ALL-");
            UtilityModule.ConditionalComboFillWithDS(ref ddprocess, ds, 2, true, "-ALL-");
            UtilityModule.ConditionalComboFillWithDS(ref ddfinishtype, ds, 3, true, "-Select Finish Type-");
            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }
            fillvendor();
            rdpurchasevendor.Checked = true;
            RDvendorwise();
        }
    }
    private void fillvendor()
    {
        UtilityModule.ConditionalComboFill(ref ddvendor, "Select Empid,Empname from Empinfo where MAstercompanyid=" + Session["varcompanyno"] + " order by empname ", true, "-Select Vendor-");
    }
    private void fillcustomer()
    {
        UtilityModule.ConditionalComboFill(ref ddcustomername, "Select Customerid,customercode from customerinfo order by customercode", true, "-ALL-");
    }
    private void fillcaterory()
    {
        if (RDDebitNote.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDProdCode, "select Distinct IPM.ITEM_FINISHED_ID,IPM.ProductCode from item_Parameter_Master IPM , DebitNote D Where IPM.ITEM_FINISHED_ID=D.FinishedID", true, "-ALL-");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddCatagory, "Select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER order by CATEGORY_NAME", true, "-ALL-");
        }
    }
    private void RDvendorwise()
    {
        Trcomp.Visible = true;
        trvendor.Visible = true;
        trfr.Visible = true;
        trto.Visible = true;
        trgodown.Visible = false;
        trprocess.Visible = false;
        trfinishtype.Visible = false;
        trcategory.Visible = false;
        tritem.Visible = false;
        trquality.Visible = false;
        trcustomercode.Visible = false;
        trProdCode.Visible = false;
        trOrder.Visible = false;
        trInvno.Visible = false;
        tdsubmit.Visible = true;
        tdExport.Visible = false;
    }
    private void RDMatrialIssue()
    {
        Trcomp.Visible = true;
        trvendor.Visible = false;
        trfr.Visible = true;
        trto.Visible = true;
        trgodown.Visible = true;
        trprocess.Visible = true;
        trfinishtype.Visible = true;
        trcategory.Visible = false;
        tritem.Visible = false;
        trquality.Visible = false;
        trcustomercode.Visible = false;
        trProdCode.Visible = false;
        trOrder.Visible = false;
        trInvno.Visible = false;
        tdsubmit.Visible = true;
        tdExport.Visible = false;
    }
    private void Rdrawmaterialstock()
    {
        Trcomp.Visible = true;
        trvendor.Visible = true;
        trfr.Visible = false;
        trto.Visible = false;
        trgodown.Visible = false;
        trprocess.Visible = false;
        trfinishtype.Visible = false;
        trcategory.Visible = true;
        tritem.Visible = true;
        trquality.Visible = true;
        trcustomercode.Visible = false;
        trProdCode.Visible = false;
        trOrder.Visible = false;
        trInvno.Visible = false;
        fillcaterory();
        tdsubmit.Visible = true;
        tdExport.Visible = false;
    }
    private void RDSticker()
    {
        Trcomp.Visible = true;
        trvendor.Visible = false;
        trfr.Visible = false;
        trto.Visible = false;
        trgodown.Visible = false;
        trprocess.Visible = false;
        trfinishtype.Visible = false;
        trcategory.Visible = false;
        tritem.Visible = false;
        trquality.Visible = false;
        trcustomercode.Visible = true;
        trProdCode.Visible = false;
        trOrder.Visible = false;
        trInvno.Visible = false;
        fillcustomer();
        tdsubmit.Visible = true;
        tdExport.Visible = false;
    }
    private void RDGlassStockStatement()
    {
        Trcomp.Visible = true;
        trvendor.Visible = false;
        trfr.Visible = true;
        trto.Visible = true;
        trgodown.Visible = false;
        trprocess.Visible = false;
        trfinishtype.Visible = false;
        trcategory.Visible = false;
        tritem.Visible = false;
        trquality.Visible = false;
        trcustomercode.Visible = false;
        trProdCode.Visible = true;
        trOrder.Visible = false;
        trInvno.Visible = false;
        tdsubmit.Visible = true;
        tdExport.Visible = false;
        UtilityModule.ConditionalComboFill(ref DDProdCode, "SELECT DISTINCT ITEM_FINISHED_ID, ProductCode FROM ITEM_PARAMETER_MASTER WHERE ProductCode <> '' ORDER BY ProductCode", true, "-ALL-");
    }
    private void RDPurchaseRateSummary()
    {
        Trcomp.Visible = true;
        trvendor.Visible = false;
        trfr.Visible = false;
        trto.Visible = false;
        trgodown.Visible = false;
        trprocess.Visible = false;
        trfinishtype.Visible = false;
        trcategory.Visible = false;
        tritem.Visible = false;
        trquality.Visible = false;
        trcustomercode.Visible = false;
        trProdCode.Visible = false;
        trOrder.Visible = true;
        tdsubmit.Visible = true;
        tdExport.Visible = false;
        trInvno.Visible = false;
        UtilityModule.ConditionalComboFill(ref DDOrderNo, "SELECT DISTINCT OM.orderid,OM.LocalOrder +'/' + OM.CustomerOrderNo from ordermaster OM, INDENTDETAIL ID Where OM.orderid=ID.orderid Order BY OM.LocalOrder +'/' + OM.CustomerOrderNo", true, "-ALL-");
    }
    private void DespatchDetail()
    {
        Trcomp.Visible = true;
        trvendor.Visible = true;
        trfr.Visible = false;
        trto.Visible = false;
        trgodown.Visible = false;
        trprocess.Visible = false;
        trfinishtype.Visible = false;
        trcategory.Visible = false;
        tritem.Visible = false;
        trquality.Visible = false;
        trcustomercode.Visible = false;
        trProdCode.Visible = false;
        trOrder.Visible = false;
        trInvno.Visible = true;
        tdsubmit.Visible = true;
        tdExport.Visible = false;
        UtilityModule.ConditionalComboFill(ref DDInvNo, "select Distinct Pinfo.packingid,Pinfo.TPackingNo from packinginformation Pinfo , Invoice I Where Pinfo.packingid=I.packingid AND I.Status=1 And Pinfo.Isfinal=1 Order By TPackingNo", true, "-ALL-");

    }

    private void RejectMaterial()
    {
        Trcomp.Visible = true;
        trvendor.Visible = true;
        trfr.Visible = true;
        trto.Visible = true;
        trgodown.Visible = false;
        trprocess.Visible = false;
        trfinishtype.Visible = false;
        trcategory.Visible = false;
        tritem.Visible = false;
        trquality.Visible = false;
        trcustomercode.Visible = false;
        trProdCode.Visible = false;
        trOrder.Visible = false;
        trInvno.Visible = false;
        tdsubmit.Visible = true;
        tdExport.Visible = false;
    }
    private void DebitNote()
    {
        ddfinishtype.Items.Clear();
        Trcomp.Visible = true;
        trvendor.Visible = true;
        trfr.Visible = false;
        trto.Visible = false;
        trgodown.Visible = false;
        trprocess.Visible = false;
        trfinishtype.Visible = true;
        trcategory.Visible = false;
        tritem.Visible = false;
        trquality.Visible = false;
        trcustomercode.Visible = false;
        trProdCode.Visible = true;
        trOrder.Visible = false;
        trInvno.Visible = false;
        fillcaterory();
        tdsubmit.Visible = true;
        tdExport.Visible = false;
    }
    private void BuyerOrderDetail()
    {
        ddfinishtype.Items.Clear();
        Trcomp.Visible = true;
        trvendor.Visible = false;
        trfr.Visible = false;
        trto.Visible = false;
        trgodown.Visible = false;
        trprocess.Visible = false;
        trfinishtype.Visible = true;
        trcategory.Visible = false;
        tritem.Visible = false;
        trquality.Visible = false;
        trcustomercode.Visible = true;
        trProdCode.Visible = true;
        trOrder.Visible = true;
        trInvno.Visible = true;
        fillcaterory();
        tdsubmit.Visible = true;
        tdExport.Visible = false;
        UtilityModule.ConditionalComboFill(ref ddcustomername, "Select Customerid,customercode from customerinfo order by customercode", true, "-Select Customer Name -");
    }
    private void CostComparision()
    {
        Trcomp.Visible = true;
        trvendor.Visible = false;
        trfr.Visible = false;
        trto.Visible = false;
        trgodown.Visible = false;
        trprocess.Visible = false;
        trfinishtype.Visible = true;
        trcategory.Visible = false;
        tritem.Visible = false;
        trquality.Visible = false;
        trcustomercode.Visible = true;
        trProdCode.Visible = true;
        trOrder.Visible = false;
        trInvno.Visible = true;
        fillcaterory();
        tdsubmit.Visible = true;
        tdExport.Visible = false;
        UtilityModule.ConditionalComboFill(ref ddcustomername, "Select Customerid,customercode from customerinfo order by customercode", true, "-Select Customer Name -");
        
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref dditemname, "Select ITEM_ID,ITEM_NAME from ITEM_MASTER where CATEGORY_ID=" + ddCatagory.SelectedValue + " order by ITEM_NAME", true, "-ALL-");
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RDDebitNote.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref dquality, "Select QualityId,QualityName from Quality where Item_Id=" + dditemname.SelectedValue + " order by QualityName", true, "-ALL-");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref dquality, "Select QualityId,QualityName from Quality where Item_Id=" + dditemname.SelectedValue + " order by QualityName", true, "-ALL-");
        }
    }
    protected void btnsybmit_Click(object sender, EventArgs e)
    {
        string Str = "";
        DataSet ds;
        if (rdpurchasevendor.Checked == true)
        {
            Str = @"select CustomerOrderNo +'  '+LocalOrder as fileno,pit.PindentIssueid,pit.Finishedid,Orderqty,RECQTY,Date,pii.DueDate ,ProductCode,e.empname
            From PurchaseIndentIssue pii inner join ordermaster om On om.orderid=pii.orderid inner join PurchaseIndentIssueTran pit On
            pit.PindentIssueid=pii.PindentIssueid inner join V_PendingPurchaseReceive vpr On vpr.PindentIssueid=pii.PindentIssueid and vpr.finishedid=pit.Finishedid inner join
            V_FinishedItemDetail vd On vd.ITEM_FINISHED_ID=pit.finishedid INNER JOIN EMPINFO E On e.empid=pii.partyid
            Where OM.STATUS=0 AND Partyid=" + ddvendor.SelectedValue;
            if (TxtFRDate.Text != "" && TxtTODate.Text != "")
            {
                Str = Str + " and Date>='" + TxtFRDate.Text + "' and date<='" + TxtTODate.Text + "'";
            }
            Str = Str + "  Order by pit.PindentIssueid";
            Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseVendorWise.xsd";
            Session["rptFileName"] = "Reports/RptPurchaseVendorWise.rpt";
        }
        else if (RDrawmaterial.Checked == true)
        {
//            Str = @"select Category_Name,Item_Name,QualityName,DesignName,ColorName,ShapeName,ShadeColorName,SizeMtr,SizeFt,ChallanNo,IssueQuantity ,
//            GodownNAme,IndentNo,localorder,customerorderno,FINISHED_TYPE_NAME,Issuedate,'" + ddgodown.SelectedItem.Text + "' as godown,'" + TxtFRDate.Text + "' as frdate,'" + TxtTODate.Text + @"' as todate
//            From V_IndentRawIssue vr inner join FINISHED_TYPE ft On vr.O_FINISHED_TYPE_ID =ft.Id
//            where companyid=" + ddCompName.SelectedValue + "";
            Str = @"select Category_Name,Item_Name,QualityName,DesignName,ColorName,ShapeName,ShadeColorName,SizeMtr,SizeFt,ChallanNo,IssueQuantity ,
            GodownNAme,IndentNo,localorder,customerorderno,PROCESS_NAME AS FINISHED_TYPE_NAME,Issuedate,'" + ddgodown.SelectedItem.Text + "' as godown,'" + TxtFRDate.Text + "' as frdate,'" + TxtTODate.Text + @"' as todate
            From V_IndentRawIssue_Destini_1 vr inner join Process_Name_Master PNM On vr.ProcessID =PNM.PROCESS_NAME_ID 
            where companyid=" + ddCompName.SelectedValue + "";
            if (ddgodown.SelectedIndex > 0)
            {
                Str = Str + " and godownid=" + ddgodown.SelectedValue + "";
            }
            if (ddprocess.SelectedIndex > 0)
            {
                Str = Str + " and processid=" + ddprocess.SelectedValue + "";
            }
            if (ddfinishtype.SelectedIndex > 0)
            {
                Str = Str + " and  ft.id=" + ddfinishtype.SelectedValue + "";
            }
            if (TxtFRDate.Text != "" && TxtTODate.Text != "")
            {
                Str = Str + " and Issuedate>='" + TxtFRDate.Text + "' and Issuedate<='" + TxtTODate.Text + "'";
            }
            Str = Str + " Order By QualityName";
            Session["dsFileName"] = "~\\ReportSchema\\RptMaterialIssueReport.xsd";
            Session["rptFileName"] = "Reports/RptMaterialIssueReport.rpt";
        }
        else if (RDrawmaterialrec.Checked == true)
        {
            Str = @"select Distinct CATEGORY_NAME,ITEM_NAME,QualityName,GodownName,EmpName,DESCRIPTION,RecQuantity,challanno,indentno,Date,ProductCode,CustomerOrderNo,
            LocalOrder,'" + ddgodown.SelectedItem.Text + "' as godown,'" + TxtFRDate.Text + "' as frdt,'" + TxtTODate.Text + @"' as todt
            From V_IndentRawRec vr inner join ordermaster om On Om.orderid=vr.orderid 
            Where vr.CompanyId=" + ddCompName.SelectedValue + " ";
            if (ddgodown.SelectedIndex > 0)
            {
                Str = Str + " and godownid=" + ddgodown.SelectedValue + "";
            }
            if (ddprocess.SelectedIndex > 0)
            {
                Str = Str + " and processid=" + ddprocess.SelectedValue + "";
            }
            if (ddfinishtype.SelectedIndex > 0)
            {
                Str = Str + " and  finish_type=" + ddfinishtype.SelectedValue + "";
            }
            if (TxtFRDate.Text != "" && TxtTODate.Text != "")
            {
                Str = Str + " and Date>='" + TxtFRDate.Text + "' and Date<='" + TxtTODate.Text + "'";
            }
            Str = Str + "  Order By QualityName";
            Session["dsFileName"] = "~\\ReportSchema\\RptMaterialRecReport.xsd";
            Session["rptFileName"] = "Reports/RptMaterialRecReport.rpt";
        }
        else if (RDrawmaterialstock.Checked == true)
        {
//            Str = @"select distinct cd.orderid,pd.finishedid,issueqty,issueqty-(consumptionqty*packqty) as balqty,recqty-(consumptionqty*packqty) as ihqty,Retqty,fillno,EmpName,QualityName,description, PindentIssueid,partyid,Date,DueDate
//            from V_ConsumptionDestini cd Inner join V_PurchaseDestini1 pd On cd.orderid=pd.orderid and cd.FinishedId=pd.conFINISHEDID
            Str = @"select distinct cd.orderid,pd.finishedid,issueqty,issueqty-(consumptionqty*packqty) as balqty,recqty-(consumptionqty*packqty) as ihqty,Retqty,fillno,EmpName,QualityName,description, PindentIssueid,partyid,Date,DueDate
            from V_ConsumptionDestini cd right outer join V_PurchaseDestini1 pd On cd.orderid=pd.orderid and cd.FinishedId=pd.conFINISHEDID
            where companyid=" + Session["varcompanyno"].ToString() + @"";
            if (ddCatagory.SelectedIndex > 0)
            {
                Str = Str + " and pd.CATEGORY_ID=" + ddCatagory.SelectedValue + "";
            }
            if (dditemname.SelectedIndex > 0)
            {
                Str = Str + " and pd.ITEM_ID=" + dditemname.SelectedValue + "";
            }
            if (dquality.SelectedIndex > 0)
            {
                Str = Str + " and pd.QualityId=" + dquality.SelectedValue + "";
            }
            if (ddvendor.SelectedIndex > 0)
            {
                Str = Str + " and pd.partyid=" + ddvendor.SelectedValue + "";
            }
            Str = Str + "   order by QualityName";
            Session["dsFileName"] = "~\\ReportSchema\\RptMaterialStock.xsd";
            Session["rptFileName"] = "Reports/RptMaterialStock.rpt";
        }
        else if (RDSTICKER.Checked == true)
        {
            Str = "select distinct om.LocalOrder +' / '+ om.CustomerOrderNo,BUYERCODE,QtyRequired,'" + ddcustomername.SelectedItem.Text + "' as customercode from ordermaster Om , orderdetail od Where  om.OrderId=od.OrderId and status=0";
            if (ddcustomername.SelectedIndex > 0)
            {
                Str = Str + " and om.CustomerId=" + ddcustomername.SelectedValue + "";
            }
            Str = Str + " order by om.LocalOrder +' / '+ om.CustomerOrderNo";
            Session["dsFileName"] = "~\\ReportSchema\\RptSticker.xsd";
            Session["rptFileName"] = "Reports/RptSticker.rpt";
        }
        else if (RDremovalorder.Checked == true)
        {
            Str = @"select om.OrderId,LocalOrder+' / '+CustomerOrderNo as Orderno,isnull(sum(QtyRequired),0) as orderqty ,isnull(sum(totalpcs),0) as dispactchQty,om.DispatchDate,'" + ddcustomername.SelectedItem.Text + @"' as customercode
            From ordermaster om Inner join orderdetail od  ON om.orderid=od.orderid left outer join   
            PackingInformation p On  p.orderid=om.orderid and p.isfinal=1 AND P.FinishedId=OD.Item_Finished_Id";
            if (ddcustomername.SelectedIndex > 0)
            {
                Str = Str + " Where om.CustomerId=" + ddcustomername.SelectedValue + "";
            }
            Str = Str + " group by om.orderid,LocalOrder,CustomerOrderNo,om.DispatchDate order by LocalOrder+' / '+CustomerOrderNo";
            Session["dsFileName"] = "~\\ReportSchema\\RptRemovalOrder.xsd";
            Session["rptFileName"] = "Reports/RptRemovalOrder.rpt";
        }
        else if (Rdpurchasedetail.Checked == true)
        {
            Str = @"select LocalOrder+' / '+CustomerOrderNo as Orderno, pii.pindentissueid,pii.DueDate,sum(quantity) as orderedqty,rate,Finishedid,QualityName,[dbo].[GET_PurchaseRec](prt.Finishedid,pii.PindentIssueid) as RecQty from purchaseindentissue pii inner join 
            purchaseindentissuetran prt On pii.pindentissueid=prt.pindentissueid inner join 
            V_FinishedItemDetail vd On vd.ITEM_FINISHED_ID=prt.Finishedid inner join ordermaster om On Om.orderid=pii.orderid 
            Where pii.companyid=" + ddCompName.SelectedValue + "";
            if (ddcustomername.SelectedIndex > 0)
            {
                Str = Str + " and om.CustomerId=" + ddcustomername.SelectedValue + "";
            }
            if (ddvendor.SelectedIndex > 0)
            {
                Str = Str + " and partyid=" + ddvendor.SelectedValue + "";
            }
            if (ddCatagory.SelectedIndex > 0)
            {
                Str = Str + " and vd.CATEGORY_ID=" + ddCatagory.SelectedValue + "";
            }
            if (dditemname.SelectedIndex > 0)
            {
                Str = Str + " and vd.ITEM_ID=" + dditemname.SelectedValue + "";
            }
            if (dquality.SelectedIndex > 0)
            {
                Str = Str + " and vd.QualityId=" + dquality.SelectedValue + "";
            }
            Str = Str + @" group by pii.pindentissueid,pii.DueDate,rate,Finishedid,QualityName,LocalOrder,CustomerOrderNo
                            order by LocalOrder+' / '+CustomerOrderNo , QualityName";
            Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseOrderStatus.xsd";
            Session["rptFileName"] = "Reports/RptPurchaseOrderStatus.rpt";
        }
        else if (RDremovalorderitem.Checked == true)
        {
            Str = @"select om.OrderId,LocalOrder+' / '+CustomerOrderNo as Orderno,isnull(sum(QtyRequired),0) as orderqty ,
            isnull(sum(totalpcs),0) as dispactchQty,om.DispatchDate,'" + ddcustomername.SelectedItem.Text + @"' as customercode,
            od.Item_Finished_Id,CATEGORY_NAME,ITEM_NAME,QualityName
            From ordermaster om Inner join orderdetail od  ON om.orderid=od.orderid inner join
            V_FinishedItemDetail vd On vd.Item_Finished_Id=od.Item_Finished_Id left outer join   
            PackingInformation p On  p.orderid=om.orderid and p.FinishedId=od.Item_Finished_Id and p.isfinal=1
            Where om.companyid=" + ddCompName.SelectedValue + "";
            if (ddcustomername.SelectedIndex > 0)
            {
                Str = Str + " and om.CustomerId=" + ddcustomername.SelectedValue + "";
            }
            if (ddCatagory.SelectedIndex > 0)
            {
                Str = Str + " and vd.CATEGORY_ID=" + ddCatagory.SelectedValue + "";
            }
            if (dditemname.SelectedIndex > 0)
            {
                Str = Str + " and vd.ITEM_ID=" + dditemname.SelectedValue + "";
            }
            if (dquality.SelectedIndex > 0)
            {
                Str = Str + " and vd.QualityId=" + dquality.SelectedValue + "";
            }
            Str = Str + " Group by om.orderid,LocalOrder,CustomerOrderNo,om.DispatchDate,od.Item_Finished_Id,CATEGORY_NAME,ITEM_NAME,QualityName order by LocalOrder+' / '+CustomerOrderNo";
            Session["dsFileName"] = "~\\ReportSchema\\RptRemovalOrderItem.xsd";
            Session["rptFileName"] = "Reports/RptRemovalOrderItem.rpt";
        }
        else if (RDGlassstockstmt.Checked == true)
        {
            Str = @"SELECT  IPM.ProductCode,MI.Remarks, ISNULL(VS.StockQty,0) StockQty, isnull(Sum(PRD.Qty),0) PurQty, V.SalesQty,[dbo].[F_PurchaseRate_Recent](IPM.ITEM_FINISHED_ID,'" + TxtFRDate.Text + "', '" + TxtTODate.Text + @"') RATE
                    FROM View_StockWithFinishedid VS, PurchaseReceiveDetail PRD, MAIN_ITEM_IMAGE MI, ITEM_PARAMETER_MASTER IPM, V_TotalSalesQty_RawMaterial V,PurchaseReceiveMaster PRM
                    WHERE PRM.PURCHASERECEIVEID=PRD.PURCHASERECEIVEID AND VS.ITEM_FINISHED_ID=PRD.FINISHEDID AND IPM.ITEM_FINISHED_ID=PRD.FINISHEDID AND PRD.FINISHEDID=MI.FINISHEDID AND IPM.ITEM_FINISHED_ID= V.Ifinishedid  ";
            if (TxtFRDate.Text != "" && TxtTODate.Text != "")
            {
                Str = Str + " and PRM.RECEIVEDATE>='" + TxtFRDate.Text + "' and PRM.RECEIVEDATE<='" + TxtTODate.Text + "'";
            }
            if (DDProdCode.SelectedIndex > 0)
            {
                Str = Str + " AND PRD.FINISHEDID=" + DDProdCode.SelectedValue + "";
            }
            Str = Str + "  GROUP BY IPM.ProductCode,MI.Remarks,VS.StockQty,V.SalesQty,PRD.RATE,IPM.ITEM_FINISHED_ID order by IPM.ProductCode";
            Session["dsFileName"] = "~\\ReportSchema\\RptGlassStocksStatement.xsd";
            Session["rptFileName"] = "Reports/RptGlassStocksStatement.rpt";
        }

        else if (RDPurchaseRate.Checked == true)
        {
            Str = @"select Distinct PII.Date ReceiveDate, (OM.LocalOrder +'/' + OM.CustomerOrderNo) as PINo , IPM.ProductCode,FT.FINISHED_TYPE_NAME,
                [dbo].[F_RawConsumptionQTY_OrderWise](PII.OrderID,PIT.Finishedid) OrderQTY, PIT.Rate PP,isnull(PIT.Weight,0) Weight, IPM.Description,PIT.Remark,E.EmpName
                from  Item_Parameter_Master IPM, OrderMaster OM,FINISHED_TYPE FT,EmpInfo E,PurchaseIndentIssueTran PIT
                inner join PurchaseIndentIssue PII on PIT.PindentIssueid=PII.PindentIssueid
                where IPM.ITEM_FINISHED_ID=PIT.Finishedid And OM.OrderID=PII.OrderID And FT.Id=PIT.FINISHED_TYPE_ID  And E.Empid=PII.PartyID";

            if (DDOrderNo.SelectedIndex > 0)
            {
                Str = Str + " AND PIT.finishedid in (select Distinct Ifinishedid from Order_Consumption_Detail where orderid=" + DDOrderNo.SelectedValue + ") ";
            }
            Str = Str + @"  and PII.Date in (Select max(p.DATE) from PurchaseIndentIssue P, PurchaseIndentIssueTran PT 
                            Where PT.PIndentIssueId=P.PIndentIssueId group by P.PartyID,PT.Finishedid,PT.finished_type_Id ) Order By  PII.Date desc";
            Session["dsFileName"] = "~\\ReportSchema\\RptViewPurchaseRate.xsd";
            Session["rptFileName"] = "Reports/RptViewPurchaseRate.rpt";
        }
        else if (RDDespatchDetail.Checked == true)
        {
            Str = @"SELECT Distinct P.InvoiceNo,OM.localorder+'/'+OM.CustomerOrderNo localorder,P.ShippDate,IPM.ProductCode,FT.FINISHED_TYPE_NAME,V.SalesQty,Isnull(ID.Rate,0) Rate,Pinfo.TPackingNo,E.EmpName
                     FROM packinginformation Pinfo,packing P,indentDETAIL ID,indentmaster IM, FINISHED_TYPE FT, Item_parameter_Master IPM, ordermaster OM, VIEW_TotalPackedQty_1 V, EmpInfo e
                    WHERE Pinfo.PackingID=P.PackingID  AND ID.INDENTID=IM.INDENTID AND FT.id=ID.O_FINISHED_TYPE_ID 
                    AND IPM.ITEM_FINISHED_ID=ID.IFinishedId AND V.PackingID=Pinfo.PackingID AND V.OrderID=Pinfo.Orderid AND V.Ifinishedid=ID.Ifinishedid
                    AND OM.OrderID= Pinfo.OrderID AND E.EmpID=IM.PartyID AND V.ISFINAL=1";

            if (DDInvNo.SelectedIndex > 0)
            {
                Str = Str + " AND P.packingid=" + DDInvNo.SelectedValue + "";
            }
            if (ddvendor.SelectedIndex > 0)
            {
                Str = Str + " AND IM.PartyID=" + ddvendor.SelectedValue + "";
            }
            Str = Str + @" Group by p.InvoiceNo,OM.localorder,P.ShippDate,IPM.ProductCode,FT.FINISHED_TYPE_NAME,V.SalesQty,ID.Rate,OM.CustomerOrderNo,Pinfo.TPackingNo,E.EmpName
                            having ID.Rate >0";
            Str = Str + @"  union 
                    SELECT Distinct P.InvoiceNo,OM.localorder+'/'+OM.CustomerOrderNo localorder,P.ShippDate,IPM.ProductCode,FT.FINISHED_TYPE_NAME,V.SalesQty,Isnull(ID.Rate,0) Rate,Pinfo.TPackingNo,E.EmpName
                     FROM packinginformation Pinfo,packing P,purchaseindentissuetran ID,purchaseindentissue IM, FINISHED_TYPE FT, Item_parameter_Master IPM, ordermaster OM, VIEW_TotalPackedQty_1 V, EmpInfo e
                    WHERE Pinfo.PackingID=P.PackingID  AND ID.PindentIssueid=IM.PindentIssueid AND FT.id=ID.FINISHED_TYPE_ID 
                    AND IPM.ITEM_FINISHED_ID=ID.FinishedId AND V.PackingID=Pinfo.PackingID AND V.OrderID=Pinfo.Orderid AND V.Ifinishedid=ID.finishedid
                    AND OM.OrderID= Pinfo.OrderID AND E.EmpID=IM.PartyID AND V.ISFINAL=1 ";

            if (DDInvNo.SelectedIndex > 0)
            {
                Str = Str + " AND P.packingid=" + DDInvNo.SelectedValue + "";
            }
            if (ddvendor.SelectedIndex > 0)
            {
                Str = Str + " AND IM.PartyID=" + ddvendor.SelectedValue + "";
            }
            Str = Str + @" Group by p.InvoiceNo,OM.localorder,P.ShippDate,IPM.ProductCode,FT.FINISHED_TYPE_NAME,V.SalesQty,ID.Rate,OM.CustomerOrderNo,Pinfo.TPackingNo,E.EmpName
                            having ID.Rate >0    order by IPM.ProductCode";
            Session["dsFileName"] = "~\\ReportSchema\\Rpt_DespatchDetail.xsd";
            Session["rptFileName"] = "Reports/Rpt_DespatchDetail.rpt";
        }
        else if (RDDespdetwithpo.Checked == true)
        {
            //            Str = @"SELECT Distinct P.InvoiceNo,OM.localorder+'/'+OM.CustomerOrderNo localorder,P.ShippDate,IPM.ProductCode,FT.FINISHED_TYPE_NAME,V.SalesQty,Isnull(ID.Rate,0) Rate,Pinfo.TPackingNo,E.EmpName,IM.IndentNo
            //                     FROM packinginformation Pinfo,packing P,indentDETAIL ID,indentmaster IM, FINISHED_TYPE FT, Item_parameter_Master IPM, ordermaster OM, VIEW_TotalPackedQty_1 V, EmpInfo e
            //                    WHERE Pinfo.PackingID=P.PackingID  AND ID.INDENTID=IM.INDENTID AND FT.id=ID.O_FINISHED_TYPE_ID 
            //                    AND IPM.ITEM_FINISHED_ID=ID.IFinishedId AND V.PackingID=Pinfo.PackingID AND V.OrderID=Pinfo.Orderid AND V.Ifinishedid=ID.Ifinishedid
            //                    AND OM.OrderID= Pinfo.OrderID AND E.EmpID=IM.PartyID AND V.ISFINAL=1";
            Str = @"SELECT Distinct P.InvoiceNo,OM.localorder+'/'+OM.CustomerOrderNo localorder,P.ShippDate,IPM.ProductCode,FT.FINISHED_TYPE_NAME,V.SalesQty,Isnull(ID.Rate,0) Rate,
                    Pinfo.TPackingNo,E.EmpName,(select * from [dbo].[F_GetPurchaseChallanNo](ID.Finishedid,IM.PartyID )) as IndentNO
                    FROM packinginformation Pinfo,packing P,purchaseindentissuetran ID,purchaseindentissue IM, FINISHED_TYPE FT, Item_parameter_Master IPM, ordermaster OM,
                     VIEW_TotalPackedQty_1 V, EmpInfo e
                    WHERE Pinfo.PackingID=P.PackingID  AND ID.PindentIssueid=IM.PindentIssueid AND FT.id=ID.FINISHED_TYPE_ID 
                    AND IPM.ITEM_FINISHED_ID=ID.FinishedId AND V.PackingID=Pinfo.PackingID AND V.OrderID=Pinfo.Orderid AND V.Ifinishedid=ID.finishedid
                    AND OM.OrderID= Pinfo.OrderID AND E.EmpID=IM.PartyID AND V.ISFINAL=1  AND ID.Rate>0";

            if (DDInvNo.SelectedIndex > 0)
            {
                Str = Str + " AND P.packingid=" + DDInvNo.SelectedValue + "";
            }
            if (ddvendor.SelectedIndex > 0)
            {
                Str = Str + " AND IM.PartyID=" + ddvendor.SelectedValue + "";
            }
            //            Str = Str + @" Group by p.InvoiceNo,OM.localorder,P.ShippDate,IPM.ProductCode,FT.FINISHED_TYPE_NAME,V.SalesQty,ID.Rate,OM.CustomerOrderNo,Pinfo.TPackingNo,E.EmpName,IM.IndentNo
            //                            having ID.Rate >0";
            //            Str = Str + @"  union 
            //                    SELECT Distinct P.InvoiceNo,OM.localorder+'/'+OM.CustomerOrderNo localorder,P.ShippDate,IPM.ProductCode,FT.FINISHED_TYPE_NAME,V.SalesQty,Isnull(ID.Rate,0) Rate,Pinfo.TPackingNo,E.EmpName, IM.ChallanNo
            //                     FROM packinginformation Pinfo,packing P,purchaseindentissuetran ID,purchaseindentissue IM, FINISHED_TYPE FT, Item_parameter_Master IPM, ordermaster OM, VIEW_TotalPackedQty_1 V, EmpInfo e
            //                    WHERE Pinfo.PackingID=P.PackingID  AND ID.PindentIssueid=IM.PindentIssueid AND FT.id=ID.FINISHED_TYPE_ID 
            //                    AND IPM.ITEM_FINISHED_ID=ID.FinishedId AND V.PackingID=Pinfo.PackingID AND V.OrderID=Pinfo.Orderid AND V.Ifinishedid=ID.finishedid
            //                    AND OM.OrderID= Pinfo.OrderID AND E.EmpID=IM.PartyID AND V.ISFINAL=1 ";
            Str = Str + @"Union
                        SELECT Distinct P.InvoiceNo,OM.localorder+'/'+OM.CustomerOrderNo localorder,P.ShippDate,IPM.ProductCode,FT.FINISHED_TYPE_NAME,V.SalesQty,Isnull(ID.Rate,0) Rate
                        ,Pinfo.TPackingNo,E.EmpName,(select * from [dbo].[F_GetIndentNo](ID.IFinishedid,IM.PartyID )) as IndentNO
                        FROM packinginformation Pinfo,packing P,indentDETAIL ID,indentmaster IM, FINISHED_TYPE FT, Item_parameter_Master IPM, ordermaster OM, VIEW_TotalPackedQty_1 V, EmpInfo e
                        WHERE Pinfo.PackingID=P.PackingID  AND ID.INDENTID=IM.INDENTID AND FT.id=ID.O_FINISHED_TYPE_ID 
                        AND IPM.ITEM_FINISHED_ID=ID.IFinishedId AND V.PackingID=Pinfo.PackingID AND V.OrderID=Pinfo.Orderid AND V.Ifinishedid=ID.Ifinishedid
                        AND OM.OrderID= Pinfo.OrderID AND E.EmpID=IM.PartyID AND V.ISFINAL=1  AND ID.Rate>0";

            if (DDInvNo.SelectedIndex > 0)
            {
                Str = Str + " AND P.packingid=" + DDInvNo.SelectedValue + "";
            }
            if (ddvendor.SelectedIndex > 0)
            {
                Str = Str + " AND IM.PartyID=" + ddvendor.SelectedValue + "";
            }
            //            Str = Str + @" Group by p.InvoiceNo,OM.localorder,P.ShippDate,IPM.ProductCode,FT.FINISHED_TYPE_NAME,V.SalesQty,ID.Rate,OM.CustomerOrderNo,Pinfo.TPackingNo,E.EmpName, IM.ChallanNo
            //                            having ID.Rate >0    order by IPM.ProductCode";
            Str = Str + @"  order by IPM.ProductCode";
            Session["dsFileName"] = "~\\ReportSchema\\Rpt_DespatchDetailWithPO.xsd";
            Session["rptFileName"] = "Reports/Rpt_DespatchDetailWithPO.rpt";
        }

        else if (Rdorderstatus.Checked == true)
        {
            Str = @"select od.Item_Finished_Id,om.orderid, LocalOrder,CustomerOrderNo,BUYERCODE,QualityName,CRBCode as barcode,CATEGORY_NAME +'  '+ITEM_NAME+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName+'  ' as description,
            isnull(sum(QtyRequired),0) orderedqty ,isnull(sum(SalesQty),0) as dispachqty,om.DispatchDate,[dbo].[GET_CMB](od.Item_Finished_Id) as CBM,
            UnitRate,Weight as netweight,Length,Width,Height,PCS,om.Remarks,OD.HTSCODE
            From ordermaster om Inner join orderdetail od ON om.orderid=od.orderid Inner join 
            V_FinishedItemDetail vd On vd.ITEM_FINISHED_ID=od.Item_Finished_Id left outer join 
            V_SalesQty vs On vs.orderid=om.orderid and vs.finishedid=od.Item_Finished_Id left Outer join 
            PACKINGCOST pc On pc.Finishedid=od.ITEM_FINISHED_ID and PackingType=3 where om.companyid=" + ddCompName.SelectedValue + "";
            if (ddcustomername.SelectedIndex > 0)
            {
                Str = Str + " and om.customerid=" + ddcustomername.SelectedValue + "";
            }
            if (TxtFRDate.Text != "" && TxtTODate.Text != "")
            {
                Str = Str + " and om.OrderDate>='" + TxtFRDate.Text + "' and om.OrderDate<='" + TxtTODate.Text + "'";
            }
            Str = Str + " group by LocalOrder,CustomerOrderNo,BUYERCODE,CRBCode,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,om.DispatchDate,od.Item_Finished_Id,om.orderid,UnitRate,Weight,Length,Width,Height,PCS,om.Remarks,OD.HTSCODE";
            Session["dsFileName"] = "~\\ReportSchema\\Rpt_OrderStatusCustomerWise.xsd";
            Session["rptFileName"] = "Reports/Rpt_OrderStatusCustomerWise.rpt";
        }
        else if (RDFinishingRateDetail.Checked == true)
        {
            Str = @"SELECT ID.OrderID,(OM.LocalOrder +'/' + OM.CustomerOrderNo) OrderNO,IPM.PRODUCTCODE, ID.IFINISHEDID,ID.O_FINISHED_TYPE_ID fINISHID,FT.FINISHED_TYPE_NAME,ISNULL(MAX(ID.RATE),0) RATE,[dbo].[F_RawConsumptionQTY_OrderWise](ID.OrderID,ID.IFinishedid) OrderQTY,
                    [dbo].[F_PurchaseRate_Recent](ID.IFinishedid ,'', '') PP
                    FROM INDENTMASTER IM, INDENTDETAIL ID, Item_Parameter_Master IPM, Finished_Type FT, OrderMaster OM
                    WHERE IM.INDENTID =ID.INDENTID  AND IPM.ITEM_FINISHED_ID=ID.IFINISHEDID AND  FT.ID=ID.O_FINISHED_TYPE_ID AND OM.OrderID=ID.OrderID  ";
            if (DDOrderNo.SelectedIndex > 0)
            {
                Str = Str + " and ID.ORDERID=" + DDOrderNo.SelectedValue + "";
            }

            Str = Str + @" GROUP BY ID.ORDERID, FT.FINISHED_TYPE_NAME,ID.IFINISHEDID,ID.O_FINISHED_TYPE_ID,OM.LocalOrder,OM.CustomerOrderNo,IPM.PRODUCTCODE 
                            having ISNULL(MAX(ID.RATE),0) > 0";
            Session["dsFileName"] = "~\\ReportSchema\\RPTfinishingRateDetail_Destini.xsd";
            Session["rptFileName"] = "Reports/RPTfinishingRateDetail_Destini.rpt";
        }
        else if (RDRejMaterial.Checked == true)
        {
            Str = @"Select E.EmpName ,PRM.Date,PRM.ChallanNo,PRT.ReturnQty,PRT.Remark,IPM.ProductCode+'    '+ FT.FINISHED_TYPE_NAME AS ItemCode,G.GodownName
                    from PP_ProcessRecMaster PRM,PP_ProcessRecTran PRT, Item_Parameter_Master IPM, Finished_Type FT, EmpInfo E, GodownMaster G
                    WHERE PRM.PRMID=PRT.PRMID AND IPM.Item_Finished_ID=prt.finishedid  AND FT.ID=PRT.Finish_Type AND E.EmpID=PRM.EMPID 
					AND G.GodownID=PRT.GodownID AND PRT.ReturnQty>0 ";
            if (ddvendor.SelectedIndex > 0)
            {
                Str = Str + " AND prm.empid=" + ddvendor.SelectedValue;
            }
            if (TxtFRDate.Text != "" && TxtTODate.Text != "")
            {
                Str = Str + " and PRM.Date>='" + TxtFRDate.Text + "' and PRM.date<='" + TxtTODate.Text + "'";
            }
            Str = Str + "  Order by IPM.ProductCode+'    '+ FT.FINISHED_TYPE_NAME  ";
            Session["dsFileName"] = "~\\ReportSchema\\RptRejectMaterial.xsd";
            Session["rptFileName"] = "Reports/RptRejectMaterial.rpt";
        }
        else if (RDDebitNote.Checked == true)
        {
            Str = @"Select Distinct D.ID,IPM.ProductCode + ' '+FT.FINISHED_TYPE_NAME AS ItemCode,OM.CustomerOrderNo+ ' /' +OM.LocalOrder as CustomerOrder,Qty,Rate,Amount,Date,'" + ddvendor.SelectedItem + @"',D.Remarks,
                    case When DebitType =0 then 'Purchase' else 'Finishing' end As DebitnoteType , D.ChallanNo,c.CompanyName,c.CompAddr1 , c.CompAddr2,c.CompAddr3,c.CompTel,
                    c.CompFax,c.TinNo,c.Email
					From debitnote D, item_Parameter_Master IPM, Finished_Type FT, OrderMaster OM ,companyinfo c
                    Where IPM.ITEM_FINISHED_ID=D.FinishedID AND D.Finished_Type=FT.ID AND OM.OrderID=D.OrderID AND D.CompanyID=c.Companyid ";
            if (ddvendor.SelectedIndex > 0)
            {
                Str = Str + " AND PartyID=" + ddvendor.SelectedValue;
            }
            if (DDProdCode.SelectedIndex > 0)
            {
                Str = Str + " AND FinishedID=" + DDProdCode.SelectedValue;
            }
            if (ddfinishtype.SelectedIndex > 0)
            {
                Str = Str + " AND Finished_Type=" + ddfinishtype.SelectedValue;
            }
            Str = Str + " Order BY IPM.ProductCode + ' '+FT.FINISHED_TYPE_NAME";
            Session["dsFileName"] = "~\\ReportSchema\\RptDebitNote.xsd";
            Session["rptFileName"] = "Reports/RptDebitNote.rpt";
        }
        else if (RDBuyerOrderDetail.Checked == true)
        {
            Str = @"Select   OM.localorder,OM.localorder+'/'+OM.CustomerOrderNo AS PINO , P.InvoiceNo, I.InvoiceDate, P.ShippDate,OD.BuyerCode,OD.CRBCode,IPM.ProductCode,
                    FT.FINISHED_TYPE_NAME,(OD.QtyRequired - [dbo].[F_TotalDespQty_BeforeGivenDate_OrderWise](Pinfo.ID,OD.OrderID,OD.ITEM_FINISHED_ID,Pinfo.FINISHED_TYPE_ID,I.InvoiceDate)) OrderQty
                     ,Isnull(sum(Pinfo.PCS),0) DespQty,Isnull( pINFO.pRICE,0) Price  from packing P, Invoice I ,FINISHED_TYPE FT, Item_parameter_Master IPM, OrderMaster OM,OrderDetail OD
                    Left Outer join packinginformation Pinfo On Pinfo.OrderID=OD.OrderID AND pINFO.fINISHEDID=OD.ITEM_FINISHED_ID Where  OM.OrderID=OD.ORderID 
                    AND  Pinfo.PackingID=P.PackingID AND P.PackingID=I.PackingID AND IPM.ITEM_FINISHED_ID=OD.ITEM_FINISHED_ID AND FT.id=Pinfo.FINISHED_TYPE_ID  ";                 
                   
            if (DDOrderNo.SelectedIndex > 0)
            {
                Str = Str + " AND  OM.OrderID=" + DDOrderNo.SelectedValue;
            }
            if (DDProdCode.SelectedIndex > 0)
            {
                Str = Str + " AND Pinfo.FinishedID=" + DDProdCode.SelectedValue;
            }
            if (ddfinishtype.SelectedIndex > 0)
            {
                Str = Str + " AND Pinfo.FINISHED_TYPE_ID=" + ddfinishtype.SelectedValue;
            }
            if (DDInvNo.SelectedIndex > 0)
            {
                Str = Str + " AND Pinfo.PackingID=" + DDInvNo.SelectedValue;
            }
            Str = Str + @"   group by OM.localorder,OM.CustomerOrderNo , P.InvoiceNo, I.InvoiceDate, P.ShippDate,OD.BuyerCode,OD.CRBCode,IPM.ProductCode,
                         FT.FINISHED_TYPE_NAME,OD.QtyRequired, OD.OrderID,Pinfo.FINISHED_TYPE_ID,OD.ITEM_FINISHED_ID,Pinfo.ID,pINFO.pRICE
                         Order BY pinfo.id ";
            Session["dsFileName"] = "~\\ReportSchema\\RptBuyerOrderDetail.xsd";
            Session["rptFileName"] = "Reports/RptBuyerOrderDetail.rpt";
        }
        else if (RDCostComparision.Checked == true)
        {
             Str = @"select distinct PAckingid,IndentID,OrderID,IFinishedId,O_Finished_Type_ID,Quantity,Rate,ProductCode,FINISHED_TYPE_NAME,Process_Name,TPackingNo,OrderNo
                          from  V_IndentPurchaseDetail_PL  where  rate>0";
           if (DDInvNo.SelectedIndex > 0)
           {
               Str = Str + " And PackingID=" + DDInvNo.SelectedValue;
           }
           Session["dsFileName"] = "~\\ReportSchema\\RptCostComparision.xsd";
           Session["rptFileName"] = "Reports/RptCostComparision.rpt";
        }
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseVendorWise.xsd";
            //Session["rptFileName"] = "Reports/RptPurchaseVendorWise.rpt";
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void rdpurchasevendor_CheckedChanged(object sender, EventArgs e)
    {
        RDvendorwise();
    }
    protected void RDrawmaterial_CheckedChanged(object sender, EventArgs e)
    {
        RDMatrialIssue();
    }
    protected void RDrawmaterialrec_CheckedChanged(object sender, EventArgs e)
    {
        RDMatrialIssue();
    }
    protected void RDrawmaterialstock_CheckedChanged(object sender, EventArgs e)
    {
        Rdrawmaterialstock();
    }
    protected void RDJwissue_CheckedChanged(object sender, EventArgs e)
    {
        RDSticker();
    }
    protected void RDremovalorder_CheckedChanged(object sender, EventArgs e)
    {
        RDSticker();
    }
    protected void Rdpurchasedetail_CheckedChanged(object sender, EventArgs e)
    {
        RDSticker();
        trvendor.Visible = true;
        trcategory.Visible = true;
        tritem.Visible = true;
        trquality.Visible = true;
        tdsubmit.Visible = true;
        tdExport.Visible = false;
        fillcaterory();
    }
    protected void RDremovalorderitem_CheckedChanged(object sender, EventArgs e)
    {
        RDSticker();
        trvendor.Visible = false;
        trcategory.Visible = true;
        tritem.Visible = true;
        trquality.Visible = true;
        tdsubmit.Visible = true;
        tdExport.Visible = false;
        fillcaterory();
    }
    protected void RDGlassstockstmt_CheckedChanged(object sender, EventArgs e)
    {
        RDGlassStockStatement();
    }
    protected void RDPurchaseRate_CheckedChanged(object sender, EventArgs e)
    {
        RDPurchaseRateSummary();
    }
    protected void RDDespatchDetail_CheckedChanged(object sender, EventArgs e)
    {
        DespatchDetail();
    }
    protected void Rdorderstatus_CheckedChanged(object sender, EventArgs e)
    {
        RDSticker();
        trfr.Visible = true;
        trto.Visible = true;
    }
    protected void RdOrderStock_CheckedChanged(object sender, EventArgs e)
    {
        Trcomp.Visible = true;
        trvendor.Visible = false;
        trfr.Visible = false;
        trto.Visible = false;
        trgodown.Visible = false;
        trprocess.Visible = false;
        trfinishtype.Visible = false;
        trcategory.Visible = false;
        tritem.Visible = false;
        trquality.Visible = false;
        trcustomercode.Visible = true;
        trProdCode.Visible = false;
        trOrder.Visible = true;
        trInvno.Visible = false;
        tdsubmit.Visible = false;
        tdExport.Visible = true;
        UtilityModule.ConditionalComboFill(ref ddcustomername, "Select Customerid,customercode from customerinfo order by customercode", true, "-Select Customer Name -");
    }
    protected void ddcustomername_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RdOrderStock.Checked == true || RDBuyerOrderDetail.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDOrderNo, "SELECT DISTINCT orderid,LocalOrder +'/' + CustomerOrderNo from ordermaster where customerid=" + ddcustomername.SelectedValue + " Order BY LocalOrder +'/' + CustomerOrderNo", true, "-Select Order No-");
        }
        if (RDCostComparision.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDOrderNo, "SELECT DISTINCT orderid,LocalOrder +'/' + CustomerOrderNo from ordermaster where customerid=" + ddcustomername.SelectedValue + " Order BY LocalOrder +'/' + CustomerOrderNo", true, "-Select Order No-");

            string str = @"select Distinct Pinfo.PackingID, Pinfo.TPackingNo From PackingInformation Pinfo, OrderMaster OM, Invoice I
                            Where OM.OrderID=Pinfo.OrderID AND i.PackingID=Pinfo.PackingID AND I.Status=1 ";
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    str = str + "  AND Pinfo.OrderID=" + DDOrderNo.SelectedValue;
            //}
            if (DDProdCode.SelectedIndex > 0)
            {
                str = str + "  AND Pinfo.FinishedID=" + DDProdCode.SelectedValue;
            }
            str = str + "   Order By Pinfo.TPackingNo";
            UtilityModule.ConditionalComboFill(ref DDInvNo, str, true, "--Select--");

            str = @"select Distinct Pinfo.FinishedID, IPM.ProductCode From PackingInformation Pinfo, p  ppp IPM, Invoice I
                      Where IPM.ITEM_FINISHED_ID=Pinfo.FinishedID AND I.PackingID=Pinfo.PAckingID AND I.Status=1  Order By IPM.ProductCode";
            UtilityModule.ConditionalComboFill(ref DDProdCode, str, true, "--Select--");
        }

    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string str = "";
        if (RdOrderStock.Checked == true)
        {

            if (DDOrderNo.SelectedIndex > 0)
            {
                //string str1 = "";
                //DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Distinct Processid,process_name from PP_ProcessRawMaster pr inner join process_name_master p On p.process_name_id=pr.Processid  ");
                //if (ds1.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                //    {
                //        str1 = str1 + ",[dbo].[GET_ProcessStock] (ifinishedid,I_FINISHED_TYPE_ID," + ds1.Tables[0].Rows[i][0].ToString() + ") as " + ds1.Tables[0].Rows[i][1].ToString() + "";
                //    }
                //}
//                str = @"select Item as ItemCode, Description,
//            [dbo].[GET_ProcessRejQty] (ifinishedid,I_FINISHED_TYPE_ID) as REJQTY,[dbo].[GET_Stock] (ifinishedid,I_FINISHED_TYPE_ID,getdate())RecQty " + str1 + @",0 as total ,sum(ReqQty) as OrderReqQty,
//            (Select sum(ReqQty) from V_OrderStockStatus where ifinishedid=v.ifinishedid and I_FINISHED_TYPE_ID=v.I_FINISHED_TYPE_ID ) as ToTOrderReq
//            from V_OrderStockStatus v 
//            where  orderid=" + DDOrderNo.SelectedValue + @"
//            group by v.orderid,finishedid,I_FINISHED_TYPE_ID,ifinishedid,description,Item
//            Order By Item";

                str = @"Select ItemCode, Description,REJQTY, RecQty,ETCHING,FINISH_PROCESS,ASSMBL, (FINISH_PROCESS+ETCHING+ASSMBL + RecQty) as total,OrderReqQty,
                        ( FINISH_PROCESS+ETCHING+ASSMBL + RecQty) - OrderReqQty As Bal ,ToTOrderReq, (FINISH_PROCESS+ETCHING+ASSMBL + RecQty)-ToTOrderReq AS Balance
                        From V_Order_Wise_Stock where  orderid=" + DDOrderNo.SelectedValue + @"
                        Order By ItemCode";

                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                con.Open();
//                DataSet Ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select * from SysObjects Where Name='VIEW_TotalOrder_Final'");
//                if (Ds1.Tables[0].Rows.Count > 0)
//                {
//                    SqlHelper.ExecuteDataset(con, CommandType.Text, "DROP VIEW VIEW_TotalOrder_Final");
//                }
//                str = @"Create View VIEW_TotalOrder_Final AS 
//                        Select ItemCode, Description,REJQTY,(RecQty - (FINISH_PROCESS+ETCHING+ASSMBL) ) RecQty,FINISH_PROCESS,ETCHING,ASSMBL, (FINISH_PROCESS+ETCHING+ASSMBL) + (RecQty - (FINISH_PROCESS+ETCHING+ASSMBL) ) as total,OrderReqQty,
//                        ( (FINISH_PROCESS+ETCHING+ASSMBL) + (RecQty - (FINISH_PROCESS+ETCHING+ASSMBL) )) - OrderReqQty As Bal ,ToTOrderReq,
//                       (ToTOrderReq - OrderReqQty) -(( (FINISH_PROCESS+ETCHING+ASSMBL) + (RecQty - (FINISH_PROCESS+ETCHING+ASSMBL) )) - OrderReqQty) AS Balance
//                        From V_Order_Wise_Stock where  orderid=" + DDOrderNo.SelectedValue + " " ;
//                SqlHelper.ExecuteDataset(con, CommandType.Text, str);
//                str = @"select Distinct ItemCode,Description,sum(REJQTY) REJQTY, sum(RecQty) RecQty, sum(FINISH_PROCESS) FINISH_PROCESS,
//                         sum(ETCHING) ETCHING,sum(ASSMBL) ASSMBL, sum(total) total,OrderReqQty,sum(total) -OrderReqQty Bal,
//                        ToTOrderReq,sum(total)-ToTOrderReq Balance from VIEW_TotalOrder_Final
//                        group by OrderReqQty,ToTOrderReq,ItemCode,Description Order By ItemCode";
                
            }

            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Select Order No);", true);
            }
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            DGConsumption.DataSource = ds;
            DGConsumption.DataBind();
            if (DGConsumption.Rows.Count > 0)
            {
                DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select LocalOrder,CustomerOrderNo,replace(convert(varchar(11),DispatchDate,106),' ','-') as dispachdate from ordermaster where orderid=" + DDOrderNo.SelectedValue + "");
                DGConsumption.Style.Add("font-size", "1em");
                Response.Clear();
                string attachment = "attachment; filename=Production Order Wise.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.ContentType = "application/ms-excel";
                Response.Charset = "UTF-8";
                System.IO.StringWriter stringWrite = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                DGConsumption.GridLines = GridLines.Vertical;
                DGConsumption.RenderControl(htmlWrite);
                Response.Write(@"<table><tr><td colspan=3 align=left>Production Of Items :Performa Invoice No -: " + ds2.Tables[0].Rows[0]["LocalOrder"].ToString() + "</td></tr><tr><td align=right >Ship Date :</td><td align=left>" + ds2.Tables[0].Rows[0]["dispachdate"].ToString() + "</td><td>P.O Number :</td><td  align=left >" + ds2.Tables[0].Rows[0]["CustomerOrderNo"].ToString() + "</td></tr></table>" + stringWrite.ToString());
                Response.End();
            }
        }
    }
    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
        //confirms that an HtmlForm control is rendered for the
        //specified ASP.NET server control at run time.
    }
    protected void gdDesign_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gdDesign_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    protected void RDFinishingRateDetail_CheckedChanged(object sender, EventArgs e)
    {
        RDPurchaseRateSummary();
    }

    protected void RDDespdetwithpo_CheckedChanged(object sender, EventArgs e)
    {
        DespatchDetail();
    }
    protected void RDRejMaterial_CheckedChanged(object sender, EventArgs e)
    {
        RejectMaterial();
    }
    protected void RDDebitNote_CheckedChanged(object sender, EventArgs e)
    {
        DebitNote();
    }
    protected void DDProdCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RDDebitNote.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddfinishtype, "Select Distinct Finished_Type, FT.FINISHED_TYPE_NAME   From debitnote D, Finished_Type FT Where D.Finished_Type=FT.ID AND FinishedID=" + DDProdCode.SelectedValue + "  Order By FINISHED_TYPE_NAME", true, "-Select-");
        }
        else if (RDBuyerOrderDetail.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddfinishtype, "Select Distinct Finished_Type_id, FT.FINISHED_TYPE_NAME   From PackingInformation Pinfo, Finished_Type FT Where Pinfo.Finished_Type_id=FT.ID AND FinishedID=" + DDProdCode.SelectedValue + "    Order By FINISHED_TYPE_NAME", true, "-Select-");
        }
        else if (RDCostComparision.Checked == true)
        {
            string str = @"select Distinct Pinfo.FinishedID, IPM.ProductCode From PackingInformation Pinfo, Item_Parameter_Master IPM, Invoice I
                      Where IPM.ITEM_FINISHED_ID=Pinfo.FinishedID AND I.PackingID=Pinfo.PAckingID AND I.Status=1 ";
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    str = str + "  AND Pinfo.OrderID=" + DDOrderNo.SelectedValue;
            //}
            if (DDProdCode.SelectedIndex > 0)
            {
                str = str + "  AND Pinfo.FinishedID=" + DDProdCode.SelectedValue;
            }
            str = str + "   Order By Pinfo.TPackingNo";
            UtilityModule.ConditionalComboFill(ref DDInvNo, str, true, "--Select--");
        }
    }
    protected void RDBuyerOrderDetail_CheckedChanged(object sender, EventArgs e)
    {
        BuyerOrderDetail();
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RDBuyerOrderDetail.Checked == true)
        {
            string Str = @"select Distinct Pinfo.FinishedID, IPM.ProductCode From PackingInformation Pinfo, Item_Parameter_Master IPM, Invoice I
                      Where IPM.ITEM_FINISHED_ID=Pinfo.FinishedID AND I.PackingID=Pinfo.PAckingID AND I.Status=1";
            if (DDOrderNo.SelectedIndex > 0)
            {
                Str = Str + "  AND OrderID=" + DDOrderNo.SelectedValue;
            }
            Str = Str + "  Order By IPM.ProductCode";
            UtilityModule.ConditionalComboFill(ref DDProdCode, Str, true, "--Select--");
        }
        if (RDCostComparision.Checked == true)
        {
            string Str = @"select Distinct Pinfo.FinishedID, IPM.ProductCode From PackingInformation Pinfo, Item_Parameter_Master IPM, Invoice I
                      Where IPM.ITEM_FINISHED_ID=Pinfo.FinishedID AND I.PackingID=Pinfo.PAckingID AND I.Status=1";
            if (DDOrderNo.SelectedIndex > 0)
            {
                Str = Str + "  AND OrderID=" + DDOrderNo.SelectedValue;
            }
            Str = Str + "  Order By IPM.ProductCode";
            UtilityModule.ConditionalComboFill(ref DDProdCode, Str, true, "--Select--");
        }
    }
    protected void ddfinishtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RDBuyerOrderDetail.Checked == true)
        {
            string str = @"select Distinct Pinfo.PackingID, Pinfo.TPackingNo From PackingInformation Pinfo, OrderMaster OM
                            Where OM.OrderID=Pinfo.OrderID ";
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    str = str + "  AND Pinfo.OrderID=" + DDOrderNo.SelectedValue;
            //}
            if (DDProdCode.SelectedIndex > 0)
            {
                str = str + "  AND Pinfo.FinishedID=" + DDProdCode.SelectedValue;
            }
            str = str + "   Order By Pinfo.TPackingNo";
            UtilityModule.ConditionalComboFill(ref DDInvNo, str, true, "--Select--");
        }
    }
    protected void RDCostComparision_CheckedChanged(object sender, EventArgs e)
    {
        CostComparision();
    }
}
