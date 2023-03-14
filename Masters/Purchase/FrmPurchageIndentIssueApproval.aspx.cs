using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Purchase_FrmPurchageIndentIssueApproval : CustomPage
{
    static string rowindexforPreview;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            lblMessage.Visible = false;
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            UtilityModule.ConditionalComboFill(ref DDPartyName, "Select Distinct EmpId,EmpName From EmpInfo EI,PurchaseIndentIssue PII Where PII.Partyid=EI.EmpId And EI.MasterCompanyId=" + Session["varCompanyId"] + " Order BY EmpName", true, "--Select Employee--");
            if (DDPartyName.Items.Count > 0)
            {
                DDChallanNo.Focus();
            }
        }
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        PartySelectedIndexChange();
    }
    private void PartySelectedIndexChange()
    {
        UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select PIndentIssueId,ChallanNo+'  /  '+Replace(Convert(VarChar(11),Date,106), ' ','-') From PurchaseIndentIssue Where PIndentIssueId Not in 
        (Select PIndentIssueId From Purchase_Approval) And PartyId=" + DDPartyName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By ChallanNo", true, "--Select Order No--");
        Fill_Grid();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (lblMessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[6];
                _arrPara[0] = new SqlParameter("@PindentIssueId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@UserId", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@Date", SqlDbType.SmallDateTime);
                _arrPara[5] = new SqlParameter("@Remarks", SqlDbType.NVarChar, 50);

                _arrPara[0].Value = DDChallanNo.SelectedValue;
                _arrPara[1].Value = 10;
                _arrPara[2].Value = Session["varuserid"];
                _arrPara[3].Value = Session["varCompanyId"];
                _arrPara[4].Value = TxtDate.Text;
                _arrPara[5].Value = txtRemarks.Text;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_Purchase_Order_Approval", _arrPara);
                Tran.Commit();
                ClearAll();
                lblMessage.Visible = true;
                lblMessage.Text = "DATA SAVED ........";
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Purchase/FrmPurchageIndentIssueApproval.aspx");
                lblMessage.Text = ex.Message;
                Tran.Rollback();
                Logs.WriteErrorLog("Masters_Carpet_FrmTDS_MASTER|cmdSave_Click|" + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                if (con != null)
                {
                    con.Dispose();
                }
            }
            Fill_Grid();
        }
    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (UtilityModule.VALIDTEXTBOX(TxtDate) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtRemarks) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        lblMessage.Visible = true;
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
    }
    private void Fill_Grid()
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select PII.PIndentIssueId Sr_No,ChallanNo+'  /  '+Replace(Convert(VarChar(11),PII.Date,106), ' ','-') ChallanNo,
        Replace(Convert(VarChar(11),PA.Date,106), ' ','-') Date,PA.Remarks From PurchaseIndentIssue PII,Purchase_Approval PA Where PII.PIndentIssueId=PA.PIndentIssueId And 
        PII.Partyid=" + DDPartyName.SelectedValue + " And PII.MasterCompanyId=" + Session["varCompanyId"] + "");
        DG.DataSource = ds;
        DG.DataBind();
    }
    private void ClearAll()
    {
        PartySelectedIndexChange();
        txtRemarks.Text = "";
        DDChallanNo.Focus();
    }

    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DG, "Select$" + e.Row.RowIndex);
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        ViewState["PIndentIssueId"] = 0;

        if (DG.Rows.Count > 0)
        {
            if (rowindexforPreview != "")
            {
                ViewState["PIndentIssueId"] = DG.DataKeys[Convert.ToInt32(rowindexforPreview)].Value;
                report();

                ViewState["VarCompanyNo"] = Session["Varcompanyno"];
                Report1();
            }
        }

    }
    private void report()
    {
        int VarCompanyNo = Convert.ToInt32(Session["varCompanyNo"].ToString());
        if (VarCompanyNo == 2)
        {
            Session["ReportPath"] = "Reports/PurchaseIndentIssDestini_1NEW.rpt";
            Session["CommanFormula"] = "{V_PIndentIssueReport_1.PIndentIssueId}=" + ViewState["PIndentIssueId"] + "";
        }
        else if (VarCompanyNo == 6)
        {
            Session["ReportPath"] = "Reports/PurchaseIndentIssArtNEW.rpt";
            Session["CommanFormula"] = "{V_PIndentIssueReport.PIndentIssueId}=" + ViewState["PIndentIssueId"] + "";
        }
        else
        {
            Session["ReportPath"] = "Reports/PurchaseIndentIssNEW.rpt";
            Session["CommanFormula"] = "{V_PIndentIssueReport.PIndentIssueId}=" + ViewState["PIndentIssueId"] + "";
        }
    }
    private void Report1()
    {
        string qry = "";
        if (Convert.ToInt32(ViewState["VarCompanyNo"]) != 2)
        {
            if (Convert.ToString(Session["ReportPath"]) == "Reports/PurchaseIndentIssNEW.rpt" || Convert.ToString(Session["ReportPath"]) == "Reports/PurchaseIndentIssArtNEW.rpt")
            {
                qry = @"SELECT V_Companyinfo.CompanyName,V_Companyinfo.CompanyAddress,V_Companyinfo.CompanyPhoneNo,V_Companyinfo.CompanyFaxNo, V_Companyinfo.TinNo,V_EmployeeInfo.EmpName,V_EmployeeInfo.EmpAddress,V_EmployeeInfo.EmpPhoneNo,V_EmployeeInfo.EmpMobile,V_EmployeeInfo.EmpFax,V_PIndentIssueReport.Challanno,V_PIndentIssueReport.Date,V_PIndentIssueReport.Destination, V_FinishedItemDetail.ITEM_NAME,V_FinishedItemDetail.QualityName,V_FinishedItemDetail.designName,V_FinishedItemDetail.ColorName,V_FinishedItemDetail.ShadeColorName,V_FinishedItemDetail.ShapeName, V_FinishedItemDetail.SizeMtr,V_PIndentIssueReport.FeightRate,V_PIndentIssueReport.Remarks,V_PIndentIssueReport.Quantity,V_PIndentIssueReport.LotNo,V_PIndentIssueReport.AgentName,V_PIndentIssueReport.PIndentIssueTranId, V_PIndentIssueReport.PindentIssueid,FINISHED_TYPE.FINISHED_TYPE_NAME,V_PIndentIssueReport.Amount,V_PIndentIssueReport.ExciseDuty,V_PIndentIssueReport.EduCess,V_PIndentIssueReport.CST,V_PIndentIssueReport.delivery_date,cm.customercode,em.empname,V_PIndentIssueReport.rate,cmm.customercode as custcode,omm.customerorderno as custorderno,omm.localorder as loc ,piid.itemremark,u.UnitName as UnitId
                       FROM V_PIndentIssueReport INNER JOIN 
                       V_Companyinfo ON V_PIndentIssueReport.Companyid=V_Companyinfo.CompanyId INNER JOIN 
                       V_EmployeeInfo ON V_PIndentIssueReport.Partyid=V_EmployeeInfo.EmpId INNER JOIN 
                       V_FinishedItemDetail ON V_PIndentIssueReport.FinishedId=V_FinishedItemDetail.ITEM_FINISHED_ID LEFT OUTER JOIN 
                       FINISHED_TYPE ON V_PIndentIssueReport.Finished_type_id=FINISHED_TYPE.ID left outer join 
                       Ordermaster om on om.orderid=V_PIndentIssueReport.orderid left outer join 
                       Customerinfo cm on cm.customerid=om.customerid left outer join 
                       PurchaseIndentMaster pid on pid.pindentid=V_PIndentIssueReport.indentid left outer join
                       PurchaseIndentDetail piid on piid.pindentid=pid.pindentid  left outer join 
                       Empinfo em on em.empid=pid.empid left outer join 
                       Customerinfo cmm on pid.customercode=cmm.customerid left outer join 
                       Ordermaster omm on pid.orderid=omm.orderid inner join 
                       unit u On u.unitid=V_PIndentIssueReport.unitid
                      Where V_PIndentIssueReport.PIndentIssueId=" + ViewState["PIndentIssueId"] + " And V_PIndentIssueReport.MasterCompanyId=" + Session["varCompanyId"];
                Session["dsFileName"] = "~\\ReportSchema\\PurchaseIndentIssNEW.xsd";
            }
            else
            {
                qry = @" SELECT V_Companyinfo.CompanyName,V_Companyinfo.CompanyAddress,V_Companyinfo.CompanyPhoneNo,V_Companyinfo.CompanyFaxNo,V_Companyinfo.TinNo,V_EmployeeInfo.EmpName,V_EmployeeInfo.EmpAddress,V_EmployeeInfo.EmpPhoneNo,V_EmployeeInfo.EmpMobile,V_EmployeeInfo.EmpFax,V_PIndentIssueReport.Challanno,V_PIndentIssueReport.Date,V_PIndentIssueReport.Destination,V_FinishedItemDetail.ITEM_NAME,V_FinishedItemDetail.QualityName,V_FinishedItemDetail.designName,V_FinishedItemDetail.ColorName,V_FinishedItemDetail.ShadeColorName,V_FinishedItemDetail.ShapeName,V_FinishedItemDetail.SizeMtr,V_PIndentIssueReport.FeightRate,V_PIndentIssueReport.Remarks,V_PIndentIssueReport.Quantity,V_PIndentIssueReport.LotNo,Payment.PaymentName,V_PIndentIssueReport.AgentName,Term.TermName,V_PIndentIssueReport.PIndentIssueTranId,V_PIndentIssueReport.PindentIssueid,FINISHED_TYPE.FINISHED_TYPE_NAME,V_PIndentIssueReport.Amount,V_PIndentIssueReport.ExciseDuty,V_PIndentIssueReport.EduCess,V_PIndentIssueReport.CST
                      FROM   V_PIndentIssueReport INNER JOIN V_Companyinfo ON V_PIndentIssueReport.Companyid=V_Companyinfo.CompanyId INNER JOIN V_EmployeeInfo ON V_PIndentIssueReport.Partyid=V_EmployeeInfo.EmpId INNER JOIN V_FinishedItemDetail ON V_PIndentIssueReport.FinishedId=V_FinishedItemDetail.ITEM_FINISHED_ID LEFT OUTER JOIN Payment ON V_PIndentIssueReport.PayementTermId=Payment.PaymentId LEFT OUTER JOIN Term ON V_PIndentIssueReport.DeliveryTermid=Term.TermId
                      LEFT OUTER JOIN FINISHED_TYPE ON V_PIndentIssueReport.Finished_type_id=FINISHED_TYPE.ID
                      Where V_PIndentIssueReport.PIndentIssueId=" + ViewState["PIndentIssueId"] + " And V_PIndentIssueReport.MasterCompanyId=" + Session["varCompanyId"];
                Session["dsFileName"] = "~\\ReportSchema\\PurchaseIndentIss_withoutratNew.xsd";
            }
        }
        else if (Convert.ToInt32(ViewState["VarCompanyNo"]) == 2)
        {
            qry = @"SELECT V_PIndentIssueReport_1.customerOrderNo,V_PIndentIssueReport_1.Challanno,V_PIndentIssueReport_1.Date,V_PIndentIssueReport_1.DueDate,V_PIndentIssueReport_1.Remarks,V_PIndentIssueReport_1.Rate,V_PIndentIssueReport_1.Quantity,V_PIndentIssueReport_1.Amount,V_PIndentIssueReport_1.unitid,V_PIndentIssueReport_1.InDescription,CompanyInfo.CompanyName,CompanyInfo.CompAddr1,CompanyInfo.CompAddr2,CompanyInfo.CompAddr3,CompanyInfo.CompFax,CompanyInfo.CompTel,CompanyInfo.TinNo,CompanyInfo.Email,EmpInfo.EmpName,EmpInfo.Mobile,V_PIndentIssueReport_1.weight,V_PIndentIssueReport_1.Delivery_Date,V_PIndentIssueReport_1.LocalOrder,OrderMaster.CustomerOrderNo
                  FROM   V_PIndentIssueReport_1 INNER JOIN CompanyInfo ON V_PIndentIssueReport_1.Companyid=CompanyInfo.CompanyId
                  INNER JOIN EmpInfo ON V_PIndentIssueReport_1.Partyid=EmpInfo.EmpId
                  INNER JOIN OrderMaster ON V_PIndentIssueReport_1.Orderid=OrderMaster.OrderId
                  Where V_PIndentIssueReport_1.PIndentIssueId=" + ViewState["PIndentIssueId"] + " And V_PIndentIssueReport_1.MasterCompanyId=" + Session["varCompanyId"];
            Session["dsFileName"] = "~\\ReportSchema\\PurchaseIndentIssDestini_1New.xsd";


        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = Session["ReportPath"];
            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            //Session["dsFileName"] = "~\\ReportSchema\\PurchaseIndentIss_withoutratNew.xsd";
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
    protected void DG_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Delete Purchase_Approval Where Processid=10 And PIndentIssueId=" + DG.DataKeys[e.RowIndex].Value);
            //}
            Fill_Grid();
            PartySelectedIndexChange();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/FrmPurchageIndentIssueApproval.aspx");
            lblMessage.Text = ex.Message;
        }
    }
    protected void DG_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    protected void Chk1_CheckedChanged(object sender, EventArgs e)
    {
        if ((sender as CheckBox).Checked == true)
        {
            rowindexforPreview = Convert.ToString(((sender as CheckBox).NamingContainer as GridViewRow).RowIndex);
        }
        else
        {
            rowindexforPreview = "";
        }

    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        rowindexforPreview = "";
    }
}