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
public partial class Masters_ReportForms_FrmProcessDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by Companyname 
            Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By PROCESS_NAME";
            DataSet ds = SqlHelper.ExecuteDataset(str);
            CommanFunction.FillComboWithDS(DDCompany, ds, 0);
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Select--");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                CompanySelectedChange();
            }
            imgLogo.ImageUrl.DefaultIfEmpty();
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varMasterCompanyIDForERP"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
            LblCompanyName.Text = Session["varCompanyName"].ToString();
            LblUserName.Text = Session["varusername"].ToString();
            TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDEmpName, "Select Distinct EI.EmpId,EI.EmpName from Empinfo EI,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM WHERE PIM.EmpId=EI.EmpId And CompanyId=" + DDCompany.SelectedValue + " And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By EI.EmpName", true, "--Select--");
    }
    protected void DDEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmpSelectedChanged();
    }
    private void EmpSelectedChanged()
    {
        if (DDProcessName.SelectedIndex > 0 && DDEmpName.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref DDChallanNo, "Select Process_Rec_Id,Process_Rec_Id from PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " Where CompanyId=" + DDCompany.SelectedValue + " And EmpId=" + DDEmpName.SelectedValue + "", true, "--Select--");
        }
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = "";
        if (DDProcessName.SelectedIndex > 0 && DDEmpName.SelectedIndex > 0 && DDChallanNo.SelectedIndex > 0)
        {
            Str = "Select replace(convert(varchar(11),ReceiveDate,106), ' ','-') As Date From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " Where Process_Rec_Id=" + DDChallanNo.SelectedValue + "";
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                TxtFromDate.Text = Ds.Tables[0].Rows[0]["Date"].ToString();
                TxtToDate.Text = Ds.Tables[0].Rows[0]["Date"].ToString();
            }
        }
    }
    protected void ChkForProcessIssRecSummary_CheckedChanged(object sender, EventArgs e)
    {
        ProcessIssRecCheckedChanged();
    }
    private void ProcessIssRecCheckedChanged()
    {
        Label4.Text = "Rec Challan No";
        ChkSummary.Visible = true;
        EmpSelectedChanged();
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        string Str = "";
        CHECKVALIDCONTROL();
        if (lblMessage.Text == "")
        {

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            try
            {
                if (ChkForProcessLedger.Checked == true)
                {
                    Session["ReportPath"] = "Reports/RptProcessLadger.rpt";

                    Str = @"Select CompanyId,E.Empid,PHA.ProcessId,PNM.PROCESS_NAME,PHA.Appdate Date,E.EmpName,'Hissab/Slip No' AS Type,AppvNo HissabNo,NetAmt AS CreditBal,0 as DebitBal,'" + TxtFromDate.Text + "' FromDate,'" + TxtToDate.Text + @"' ToDate
                    From ProcessHissabApproved PHA inner join Empinfo E on PHA.EmpId=E.Empid Inner Join Process_Name_Master PNM ON PHA.ProcessId=PNM.PROCESS_NAME_ID 
                    Where PHA.HissabType=0 And CompanyId=" + DDCompany.SelectedValue + @" And 
                    PHA.Appdate>='" + TxtFromDate.Text + "' And PHA.Appdate<='" + TxtToDate.Text + "' And E.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

                    string Str1 = @" Union Select php.CompanyId,E.Empid,PHP.ProcessId,PNM.PROCESS_NAME,php.date,e.empname,Case When Chqcash=0 Then 'Cash' Else 'Cheque No.' + cast(ChqNo as varchar)+'  '+ Narration End As Typ,
                    ' ' As BillNo,0 As Hissab,Sum(PHP.Amount) As Debitbal,'" + TxtFromDate.Text + "' FromDate,'" + TxtToDate.Text + @"' ToDate From ProcessHissabPayment php inner join empinfo e on php.partyid=e.empid inner Join 
                    ProcessHissabApproved PHA on php.ApprovalNo=PHA.AppvNo Inner Join Process_Name_Master PNM ON PHA.ProcessId=PNM.PROCESS_NAME_ID 
                    Where PHA.HissabType=0 And php.CompanyId=" + DDCompany.SelectedValue + @" And 
                    PHA.Appdate>='" + TxtFromDate.Text + "' And PHA.Appdate<='" + TxtToDate.Text + "' And e.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                    if (DDProcessName.SelectedIndex > 0)
                    {
                        Str = Str + " And PHA.ProcessId=" + DDProcessName.SelectedValue;
                        Str1 = Str1 + " And PHP.ProcessId=" + DDProcessName.SelectedValue;
                    }
                    if (DDEmpName.SelectedIndex > 0)
                    {
                        Str = Str + " And E.Empid=" + DDEmpName.SelectedValue;
                        Str1 = Str1 + " And E.Empid=" + DDEmpName.SelectedValue;
                    }
                    Str1 = Str1 + " Group By Php.CompanyId,E.Empid,php.date,e.empname,Chqcash,ChqNo,Narration,PNM.PROCESS_NAME,PHP.ProcessId";
                    Str = Str + Str1;
                }
                else
                {
                    Str = "Select 0 StockNo,";
                    if (ChkSummary.Checked == true)
                    {
                        Session["ReportPath"] = "Reports/RptProcessSummaryNEW.rpt";
                        Str = Str + " '' TStockNo,";
                    }
                    else
                    {
                        Session["ReportPath"] = "Reports/RptProcessDetailNEW.rpt";
                        Str = Str + " (Select * From [dbo].[Get_StockNoNext_Receive_Detail_Wise](PD.Process_Rec_Detail_Id," + DDProcessName.SelectedValue + @",Issue_Detail_Id)) TStockNo,";
                    }
                    Str = Str + @" CI.CompanyName,PD.Area*PD.Qty Area,PD.Rate,PD.Amount,PD.Qty,'" + TxtFromDate.Text + "' FromDate,'" + TxtToDate.Text + "' ToDate,EI.EmpName,(Select PROCESS_NAME From PROCESS_NAME_MASTER Where PROCESS_NAME_ID=" + DDProcessName.SelectedValue + @") PROCESS_NAME,
                    PD.Length,PD.Width,VF.CATEGORY_NAME,VF.ITEM_NAME,VF.QualityName,VF.designName,VF.ColorName,VF.ShadeColorName,VF.ShapeName,
                    PM.UnitId,VF.QualityId,VF.designId,VF.SizeId,VF.ITEM_ID,PD.Penality,Comm,CommAmt,TDSPercentage,PM.Process_Rec_Id,PM.ReceiveDate,PM.ChallanNo,PD.IssueOrderId As FolioNo,U.UnitName,
                    (select isnull(sum(Qty),0) from PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PRD where  PRD.Process_Rec_Detail_Id=PD.Process_Rec_Detail_Id and PRD.QualityType=3) as RejectQty
                    From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF,CompanyInfo CI,EmpInfo EI,Unit U
                    Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Qty<>0 And PD.Item_Finished_id=VF.ITEM_FINISHED_ID  And PM.CompanyId=CI.CompanyId And PM.EmpId=EI.EmpId And PM.UnitId=U.UnitId And
                    PD.Qty<>0 And PD.QualityType<>3 And PM.CompanyId=" + DDCompany.SelectedValue + " And PM.ReceiveDate>='" + TxtFromDate.Text + "' And PM.ReceiveDate<='" + TxtToDate.Text + "' And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                    if (DDOrderNo.SelectedIndex > 0)
                    {
                        Str = Str + " And Pd.orderid=" + DDOrderNo.SelectedValue;
                    }

                    if (DDEmpName.SelectedIndex > 0)
                    {
                        Str = Str + " And PM.EmpId=" + DDEmpName.SelectedValue;
                    }
                    if (DDChallanNo.SelectedIndex > 0)
                    {
                        Str = Str + " And PM.Process_Rec_Id=" + DDChallanNo.SelectedValue + "";
                    }
                }
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["rptFileName"] = Session["ReportPath"];
                    if (ChkForProcessLedger.Checked == true)
                    {
                        Session["dsFileName"] = "~\\ReportSchema\\RptProcessLadger.xsd";
                    }
                    else
                    {
                        Session["dsFileName"] = "~\\ReportSchema\\RptProcessDetailNEW.xsd";
                    }
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
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmProcessDetail.aspx");
                lblMessage.Text = ex.Message;
                lblMessage.Visible = true;
            }
        }
    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDProcessName) == false)
        {
            goto a;
        }

        if (UtilityModule.VALIDTEXTBOX(TxtFromDate) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtFromDate) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/main.aspx");
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
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
        string str = "Select OrderId, LocalOrder+ ' / ' +CustomerOrderNo From OrderMaster where CustomerId=" + DDCustCode.SelectedValue + " And CompanyId=" + DDCompany.SelectedValue + " Order By CustomerOrderNo";

        if (Session["varMasterCompanyIDForERP"].ToString() == "16" || Session["varMasterCompanyIDForERP"].ToString() == "28")
        {
            str = "Select OrderId, CustomerOrderNo From OrderMaster where Status = 0 And CustomerId = " + DDCustCode.SelectedValue + " And CompanyId=" + DDCompany.SelectedValue + " Order By CustomerOrderNo";
        }
        UtilityModule.ConditionalComboFill(ref DDOrderNo, str, true, "--Select--");
    }
}