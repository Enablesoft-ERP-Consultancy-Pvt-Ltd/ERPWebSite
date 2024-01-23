using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;
using System.Text;
public partial class Masters_ReportForms_FrmCustomerOrderReports : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            CommanFunction.FillCombo(DDCompany, "Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Companyname ");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                CompanySelectedChange();
            }
            RDDetailDeliveryStatusMaterialBal.Checked = true;
            TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) != 5)
            {
                RDOrderDeail.Visible = true;
                RDOrderSummary.Visible = true;
            }
            if (Session["varMasterCompanyIDForERP"].ToString() == "6")
            {
                RDComm.Visible = false;
                RDStockAssign.Visible = false;
                RDOrderDeail.Visible = false;
                RDOrderSummary.Visible = false;
            }
        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedChange();
    }
    private void CompanySelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDCustCode, "Select CustomerId,CustomerCode From CustomerInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By CustomerCode", true, "--Select--");
    }
    protected void DDCustCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"Select OrderId,LocalOrder+ ' / ' +CustomerOrderNo 
                From OrderMaster where CustomerId=" + DDCustCode.SelectedValue + " And CompanyId=" + DDCompany.SelectedValue + " Order By CustomerOrderNo";

        if (Session["varMasterCompanyIDForERP"].ToString() == "16" || Session["varMasterCompanyIDForERP"].ToString() == "28")
        {
            str = @"Select OrderId, CustomerOrderNo 
                From OrderMaster 
                Where Status = 0 And CustomerId=" + DDCustCode.SelectedValue + " And CompanyId=" + DDCompany.SelectedValue + " Order By CustomerOrderNo";
        }
        UtilityModule.ConditionalComboFill(ref DDOrderNo, str, true, "--Select--");
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDOrderNo.SelectedIndex > 0)
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select replace(convert(varchar(11),OrderDate,106), ' ','-') OrderDate From OrderMaster Where OrderId=" + DDOrderNo.SelectedValue);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                TxtFromDate.Text = Ds.Tables[0].Rows[0]["OrderDate"].ToString();
                TxtToDate.Text = Ds.Tables[0].Rows[0]["OrderDate"].ToString();
            }
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (lblMessage.Text == "")
        {
            int OrderNo = Convert.ToInt32(DDOrderNo.SelectedValue == "" ? "0" : DDOrderNo.SelectedValue);
            if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) != 5)
            {

                if (RDOrderDeail.Checked == true)
                {
                    Session["ReportPath"] = "Reports/RptOrderDetail.rpt";
                    Session["CommanFormula"] = "{V_RptOrderDetail.orderid}=" + OrderNo + "";
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ReportViewer.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    return;
                }
                else if (RDOrderSummary.Checked == true)
                {
                    Session["ReportPath"] = "Reports/RptOrderSummary.rpt";
                    Session["CommanFormula"] = "{V_RptOrderDetail.orderid}=" + OrderNo + "";
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ReportViewer.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    return;
                }
            }
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (RDBriefOrdersInHand.Checked == true)
                {
                    Session["rptFileName"] = "~\\Reports\\RptBriefOrderInHand.rpt";
                    BriefOrdersInHandAndAllOrders(tran);
                }
                else if (RDAllOrdersDetailDateToDate.Checked == true)
                {
                    Session["rptFileName"] = "~\\Reports\\RptAllOrdersDetailDateToDate.rpt";
                    BriefOrdersInHandAndAllOrdersDetailDateToDate(tran);
                }
                else if (RDDetailDeliveryStatusMaterialBal.Checked == true)
                {
                    DetailDeliveryStatusMaterialBal(tran);
                }
                else if (RD1.Checked == true)
                {
                    Session["rptFileName"] = "~\\Reports\\RptBalanceStockForCompanyDyerProcess.rpt";
                    RD123(tran);
                }
                else if (RDRawMaterialBalanceAgainstOrder.Checked == true)
                {
                    RawMaterialBalanceAgainstOrder(tran);
                }
                else if (RDComm.Checked == true)
                {
                    OrderWiseWeaverComm(tran);
                }
                else if (RDStockAssign.Checked == true)
                {
                    OrderWiseStockAssign(tran);
                }
                else if (RDOrderConsumption.Checked == true)
                {
                    orderconsumptiondetail(tran);
                }
                tran.Commit();
                DataSet ds = (DataSet)Session["GetDataset"];
                if (ds.Tables[0].Rows.Count > 0)
                {
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
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmProductionReports.aspx");
                tran.Rollback();
                lblMessage.Text = ex.Message;
                lblMessage.Visible = true;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    private void RawMaterialBalanceAgainstOrder(SqlTransaction tran)
    {
        SqlParameter[] _arrpara = new SqlParameter[1];
        _arrpara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
        _arrpara[0].Value = DDOrderNo.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, "PRO_ForRawMaterialAgainstOrder", _arrpara);
        Session["GetDataset"] = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptRawMaterialBalanceAgainstOrder.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptRawMaterialBalanceAgainstOrder.xsd";
        }
    }
    private void OrderWiseStockAssign(SqlTransaction Tran)
    {
        string str = @"select CustomerCode,CustomerorderNo,Item_Name,QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' ' As Description,Sum(AssignQty) As AssignQty,Finishedid,LotNo,[dbo].[F_PurchaseRate](Finishedid,lotNo) As Purchaserate From OrderStockAssign OD,V_FinishedItemDetail Vf,OrderMaster OM,Customerinfo Ci Where OD.Finishedid=Vf.Item_Finished_id
                       And OM.OrderId=OD.Orderid And OM.Customerid=CI.CustomerId And AssignQty>0 And Om.Orderid=" + DDOrderNo.SelectedValue + "  And OD.CompanyId=" + DDCompany.SelectedValue + " group by CustomerCode,CustomerorderNo,Item_Name,QualityName,designName,ColorName,ShadeColorName,Finishedid,LotNo";
        DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
        Session["GetDataset"] = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptOrderWiseStockAssignDetail.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptOrderWiseStockAssignDetail.xsd";
        }
    }
    private void orderconsumptiondetail(SqlTransaction Tran)
    {
        SqlParameter[] param = new SqlParameter[1];
        param[0] = new SqlParameter("@orderid", DDOrderNo.SelectedValue);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getorderConsumptionDetail", param);
        Session["GetDataset"] = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptorderProcessconsumptionDetail.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptorderProcessconsumptionDetail.xsd";
        }
    }
    private void RD123(SqlTransaction tran)
    {
        SqlParameter[] _arrpara = new SqlParameter[0];

        DataSet ds = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, "PRO_BalanceStockForCompanyDyerProcess", _arrpara);
        Session["GetDataset"] = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Session["rptFileName"] = "~\\Reports\\RptBriefOrderInHand.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptBalanceStockForCompanyDyerProcess.xsd";
        }
    }
    private void BriefOrdersInHandAndAllOrders(SqlTransaction tran)
    {
        string str = @" Select CI.CustomerName,CI.CompanyName,CI.CustomerCode,OM.OrderId,OM.OrderDate,OM.DispatchDate,OM.CustomerOrderNo,OM.LocalOrder,OM.SailingDate,
		OM.DueDate,OM.ProdReqDate,VF.CATEGORY_NAME,VF.ITEM_NAME,VF.QualityName,VF.designName,VF.ColorName,VF.ShadeColorName,VF.ShapeName,VF.SizeMtr,
		VF.SizeFt,U.UnitName,OD.Item_Finished_Id,Sum(OD.QtyRequired) QtyRequired,Sum(OD.QtyRequired*OD.TotalArea) TArea,OD.DispatchDate,OD.OrderUnitId,OM.InspectionDate,'" + TxtFromDate.Text + "' FromDate,'" + TxtToDate.Text + @"' ToDate
		From OrderMaster OM,CustomerInfo CI,OrderDetail OD,V_FinishedItemDetail VF,Unit U
		Where OM.CustomerId=CI.CustomerId And OM.Orderid=OD.Orderid And OD.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID And OD.OrderUnitId=U.UnitId 
		And OM.OrderDate>='" + TxtFromDate.Text + "' And OM.OrderDate<='" + TxtToDate.Text + "' And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

        string str1 = @" Select PD.Item_Finished_Id,Sum(PD.Qty) Qty,Sum(PD.PQty) PQty,PD.Orderid,Sum(PD.RejectQty) RejectQty
		From PROCESS_ISSUE_MASTER_1 PM,PROCESS_ISSUE_DETAIL_1 PD,OrderMaster OM,CustomerInfo CI Where PM.IssueOrderId=PD.IssueOrderId And PD.OrderId=OM.OrderId And OM.CustomerId=CI.CustomerId  And
        OM.OrderDate>='" + TxtFromDate.Text + "' And OM.OrderDate<='" + TxtToDate.Text + "' And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " And OM.CustomerId=" + DDCustCode.SelectedValue;
            str1 = str1 + " And OM.CustomerId=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " And OM.Orderid=" + DDOrderNo.SelectedValue;
            str1 = str1 + " And OM.Orderid=" + DDOrderNo.SelectedValue;
        }
        str = str + @" Group By CI.CustomerName,CI.CompanyName,CI.CustomerCode,OM.OrderId,OM.OrderDate,OM.DispatchDate,OM.CustomerOrderNo,OM.LocalOrder,OM.SailingDate,
		OM.DueDate,OM.ProdReqDate,VF.CATEGORY_NAME,VF.ITEM_NAME,VF.QualityName,VF.designName,VF.ColorName,VF.ShadeColorName,VF.ShapeName,VF.SizeMtr,
		VF.SizeFt,U.UnitName,OD.Item_Finished_Id,OD.DispatchDate,OD.OrderUnitId,OM.InspectionDate";

        str1 = str1 + " Group By PD.Item_Finished_Id,PD.Orderid";
        DataSet ds = SqlHelper.ExecuteDataset(tran, CommandType.Text, str + str1);
        Session["GetDataset"] = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["dsFileName"] = "~\\ReportSchema\\PRO_ForBriefOrdersAndDateToDate.xsd";
        }
    }
    private void BriefOrdersInHandAndAllOrdersDetailDateToDate(SqlTransaction tran)
    {
        SqlParameter[] _arrpara = new SqlParameter[4];
        //_arrpara[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
        _arrpara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
        _arrpara[1] = new SqlParameter("@FromDate", SqlDbType.SmallDateTime);
        _arrpara[2] = new SqlParameter("@ToDate", SqlDbType.SmallDateTime);

        //_arrpara[0].Value = DDCustCode.SelectedValue;
        _arrpara[0].Value = DDOrderNo.Items.Count > 0 ? DDOrderNo.SelectedValue : "0";
        _arrpara[1].Value = TxtFromDate.Text;
        _arrpara[2].Value = TxtToDate.Text;

        DataSet ds = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, "PRO_ForBriefOrdersAndDateToDate", _arrpara);
        Session["GetDataset"] = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Session["rptFileName"] = "~\\Reports\\RptBriefOrderInHand.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\PRO_ForBriefOrdersAndDateToDate.xsd";
        }
    }
    private void DetailDeliveryStatusMaterialBal(SqlTransaction tran)
    {
        SqlParameter[] _arrpara = new SqlParameter[1];
        _arrpara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
        _arrpara[0].Value = DDOrderNo.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, "PRO_ForDetailDeliveryStatusMaterialBal", _arrpara);
        Session["GetDataset"] = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptDetailDeliveryStatusMaterialBalAgainstOrder.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptDetailDeliveryStatusMaterialBalAgainstOrder.xsd";
        }
    }
    private void OrderWiseWeaverComm(SqlTransaction tran)
    {
        string str, str1;
        str = @"select CI.CompanyName,CustomerCode,CustomerOrderNo,sum(QtyRequired*TotalArea) As TotalArea,UnitName from OrderMaster OM,OrderDetail OD,CustomerInfo CM,CompanyInfo CI,Unit U Where OM.OrderId=OD.OrderId
                And OM.CustomerId=CM.Customerid and OM.CompanyId=CI.CompanyId And U.UnitId=OD.OrderUnitId And Ci.CompanyId=" + DDCompany.SelectedValue + " And Om.OrderId=" + DDOrderNo.SelectedValue + "  And CM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Group by CI.CompanyName,CustomerCode,CustomerOrderNo,UnitName";


        str1 = @" select E.EmpName,CI.Companyname,IssueOrderId,Sum(Area*Qty) As WeavedArea,Comm,Sum(CommAmt) As Amount,dbo.F_WeaverCommHissab(PD.issueOrderId,E.EmpID,CI.CompanyId,Comm,1) As Advance,
               dbo.F_WeaverRawMatBal(issueOrderId,E.EmpId,Ci.CompanyId,1) As MaterialBalance  from Process_Receive_master_1 PM,Process_Receive_Detail_1 PD,Empinfo E,Companyinfo CI Where PM.Process_Rec_Id=PD.Process_Rec_Id
               And PM.EmpId=E.EmpId And CI.CompanyId=PM.CompanyId And Pm.CompanyId=" + DDCompany.SelectedValue + " And OrderId=" + DDOrderNo.SelectedValue + " And E.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " group by E.EmpName,E.EmpID,CI.Companyname,CI.CompanyId,IssueOrderId,Comm";

        DataSet ds = SqlHelper.ExecuteDataset(tran, CommandType.Text, str + str1);
        Session["GetDataset"] = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Session["rptFileName"] = "~\\Reports\\RptOrderWise_WeaverComm.rpt";
            Session["rptFileName"] = "~\\Reports\\RptOrderWise_WeaverCommForPosh.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptOrderWise_WeaverComm.xsd";
        }
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }

}

