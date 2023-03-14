using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Hissab_FrmProcessHissabByIssueNo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            string Str = @"Select CI.CompanyId, CI.CompanyName 
                From CompanyInfo CI(Nolock)
                JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                Where CI.CompanyID = " + Session["CurrentWorkingCompanyID"].ToString() + " And CI.MasterCompanyid = " + Session["varCompanyId"] + @" 
                Order By CI.CompanyName 
                Select PNM.PROCESS_NAME_ID, PNM.PROCESS_NAME 
                From PROCESS_NAME_MASTER PNM(Nolock)
                JOIN UserRightsProcess URP(Nolock) ON URP.ProcessId = PNM.PROCESS_NAME_ID And URP.Userid = " + Session["varuserid"] + @" 
                Where PNM.MasterCompanyID = " + Session["varCompanyId"];
            if (Session["varCompanyId"].ToString() == "16")
            {
                Str = Str + " And PNM.Process_Name_ID In (145, 150)";
            }
            
            Str = Str + " Order By PNM.PROCESS_NAME";

            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, Ds, 0, true, "--SELECT--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, Ds, 1, true, "--SELECT--");

            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            CheckForEditSelectedChanges();
            ViewState["Hissab_No"] = 0;
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessNameSelectedIndexChanged();
    }
    private void ProcessNameSelectedIndexChanged()
    {
        if (DDProcessName.SelectedIndex > 0)
        {
            string Str = "";
            if (ChkForEdit.Checked == true)
            {
                Str = @"Select Distinct EI.EmpId, EI.EmpName + case when isnull(ei.empcode, '') <> '' then ' [' + ei.empcode + ']' else '' end EmpName 
                From ProcessHissabIssueNoWise PH(Nolock) 
                JOIN Empinfo EI(Nolock) ON EI.EmpID = PH.EmpID And EI.Blacklist = 0 
                Where PH.CompanyId = " + DDCompanyName.SelectedValue + @" And PH.MasterCompanyId=" + Session["varcompanyId"] + @" And 
                PH.ProcessId = " + DDProcessName.SelectedValue + " Order By EmpName";
            }
            else
            {
                if (DDProcessName.SelectedItem.Text == "WASHING BY WEIGHT")
                {
                    Str = @"Select Distinct EI.EmpId, EI.EmpName + case when isnull(ei.empcode, '') <> '' then ' [' + ei.empcode + ']' else '' end EmpName 
                        From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" a(Nolock) 
                        JOIN Empinfo EI(Nolock) ON EI.EmpID = a.EmpID And EI.BlackList = 0 
                        Where a.CompanyID = " + DDCompanyName.SelectedValue + @" And a.Status = 'Complete' And a.EmpID > 0 
                        UNION
                        Select Distinct EI.EmpId, EI.EmpName + case when isnull(ei.empcode, '') <> '' then ' [' + ei.empcode + ']' else '' end EmpName 
                        From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" a(Nolock) 
                        JOIN Employee_ProcessOrderNo EPO(Nolock) ON EPO.IssueOrderID = a.IssueOrderID And EPO.ProcessID = " + DDProcessName.SelectedValue + @" 
                        JOIN Empinfo EI(Nolock) ON EI.EmpID = EPO.EmpID And EI.BlackList = 0 
                        Where a.CompanyID = " + DDCompanyName.SelectedValue + @" And a.Status = 'Complete' And a.EmpID = 0 ";
                }
                else if (DDProcessName.SelectedItem.Text == "TASSEL MAKING" || DDProcessName.SelectedItem.Text == "POM-POM MAKING" || DDProcessName.SelectedItem.Text == "BRAIDING")
                {
                    Str = @"Select Distinct EI.EmpId, EI.EmpName + case when isnull(ei.empcode, '') <> '' then ' [' + ei.empcode + ']' else '' end EmpName 
                        From ProcessIssueToTasselMakingMaster a(Nolock) 
                        JOIN Empinfo EI(Nolock) ON EI.EmpID = a.EmpID And EI.BlackList = 0 
                        Where a.CompanyID = " + DDCompanyName.SelectedValue + @" And a.Status = 'Complete'";
                }
            }
            UtilityModule.ConditionalComboFill(ref DDEmployerName, Str, true, "--SELECT--");
        }
        ViewState["Hissab_No"] = 0;
    }
    protected void DDEmployerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmployerNameSelectedIndexChanged();
    }
    private void EmployerNameSelectedIndexChanged()
    {
        if (DDProcessName.SelectedIndex > 0)
        {
            if (ChkForEdit.Checked == true)
            {
                UtilityModule.ConditionalComboFill(ref DDSlipNo, @"Select Distinct HissabNo, HissabNo HissabNo1 
                    From ProcessHissabIssueNoWise(Nolock) Where CompanyId = " + DDCompanyName.SelectedValue + " And ProcessID = " + DDProcessName.SelectedValue + @" And 
                    Empid = " + DDEmployerName.SelectedValue + " order by HissabNo1", true, "--SELECT--");
            }
            else
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@CompanyID", DDCompanyName.SelectedValue);
                param[1] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
                param[2] = new SqlParameter("@EmpID", DDEmployerName.SelectedValue);
                param[3] = new SqlParameter("@ProcessType", 1);
                param[4] = new SqlParameter("@PageName", "FrmProcessHissabByIssueNo");

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetIssueRecNoForProcessHissab", param);

                UtilityModule.ConditionalComboFillWithDS(ref DDPOOrderNo, ds, 0, true, "--SELECT--");
            }
        }
        ViewState["Hissab_No"] = 0;
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand("[Pro_ProcessHissabByIssueNo]", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@CompanyId", DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@ProcessID", DDProcessName.SelectedValue);
            cmd.Parameters.AddWithValue("@EmpId", DDEmployerName.SelectedValue);
            cmd.Parameters.AddWithValue("@IssueOrderID", DDPOOrderNo.SelectedValue);
            cmd.Parameters.AddWithValue("@ProcessTypeID", 1);
            cmd.Parameters.AddWithValue("@Date", TxtDate.Text);
            cmd.Parameters.AddWithValue("@HissabNo", ViewState["Hissab_No"]);
            cmd.Parameters["@HissabNo"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@FromDate", TxtDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtDate.Text);
            cmd.Parameters.AddWithValue("@UserID", Session["varuserId"]);
            cmd.Parameters.AddWithValue("@MasterCompanyID", Session["varCompanyId"]);
            cmd.Parameters.Add(new SqlParameter("@Msg", SqlDbType.VarChar, 300));
            cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            if (cmd.Parameters["@Msg"].Value.ToString() != "")
            {
                lblMessage.Text = "";
                lblMessage.Visible = true;
                lblMessage.Text = cmd.Parameters["@Msg"].Value.ToString();
                return;
            }

            ViewState["Hissab_No"] = cmd.Parameters["@HissabNo"].Value.ToString();
            Tran.Commit();
            lblMessage.Visible = true;
            lblMessage.Text = "Data Inserted Successfully !";
            TxtHissabNo.Text = ViewState["Hissab_No"].ToString();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void ChkForEdit_CheckedChanged(object sender, EventArgs e)
    {
        EditSelectedChange();
    }
    private void EditSelectedChange()
    {
        CheckForEditSelectedChanges();
        ProcessNameSelectedIndexChanged();
    }
    private void CheckForEditSelectedChanges()
    {
        if (ChkForEdit.Checked == true)
        {
            TDSlipNoForEdit.Visible = true;
            TDDDSlipNo.Visible = true;
            TDPoOrderNo.Visible = false;
            BtnDelete.Visible = true;
            BtnSave.Visible = false;
        }
        else
        {
            TDPoOrderNo.Visible = true;
            TDSlipNoForEdit.Visible = false;
            TDDDSlipNo.Visible = false;
            BtnDelete.Visible = false;
            BtnSave.Visible = true;
            TxtSlipNo.Text = "";
            DDSlipNo.Items.Clear();
            if (DDEmployerName.Items.Count > 0)
            {
                DDEmployerName.SelectedIndex = 0;
            }
        }
    }
    protected void TxtSlipNo_TextChanged(object sender, EventArgs e)
    {
        lblMessage.Visible = false;
        if (TxtSlipNo.Text != "")
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
                @"Select CompanyId, ProcessID, EmpId, IssueOrderID, HissabNo, replace(convert(varchar(11), Date, 106), ' ', '-') Date, ChallanNo 
                From ProcessHissabIssueNoWise(Nolock) 
                Where CommPaymentFlag = 0 And CompanyID = " + DDCompanyName.SelectedValue + " And HissabNo = " + TxtSlipNo.Text + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                DDProcessName.SelectedValue = Ds.Tables[0].Rows[0]["ProcessID"].ToString();
                ProcessNameSelectedIndexChanged();
                DDEmployerName.SelectedValue = Ds.Tables[0].Rows[0]["EmpId"].ToString();
                EmployerNameSelectedIndexChanged();
                TxtHissabNo.Text = "";
                DDSlipNo.SelectedValue = Ds.Tables[0].Rows[0]["HissabNo"].ToString();
                SlipNoSelectedChanges();
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Pls Enter Proper Slip No";
            }
        }
        else
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Pls. Enter Proper Slip No";
        }
    }
    protected void DDSlipNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        SlipNoSelectedChanges();
    }
    private void SlipNoSelectedChanges()
    {
        ViewState["Hissab_No"] = DDSlipNo.SelectedValue;
        if (DDSlipNo.SelectedIndex > 0)
        {
            TxtHissabNo.Text = DDSlipNo.Text;
            TxtDate.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select IsNull(replace(convert(varchar(11),Date,106), ' ', '-'), '')  Date From ProcessHissabIssueNoWise(Nolock) Where HissabNo = " + DDSlipNo.SelectedValue + "").ToString();
        }
    }
    protected void BtnPriview_Click(object sender, EventArgs e)
    {
        //Session["ReportPath"] = "Reports/RptProcessHissabSummary.rpt";
        //Session["CommanFormula"] = "{VIEW_PROCESS_HISSAB.HissabNo}= " + ViewState["Hissab_No"].ToString() + "";
        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
    }
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@Hissab_No", ViewState["Hissab_No"]);
            param[1] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
            param[2] = new SqlParameter("@PageName", "FrmProcessHissabByIssueNo");
            param[3] = new SqlParameter("@ProcessTypeID", 1);
            param[4] = new SqlParameter("@userid", Session["varuserid"]);
            param[5] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[6].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteProcessHissabByIssueNo", param);
            if (param[6].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + param[6].Value.ToString() + "');", true);
                Tran.Rollback();
            }
            else
            {

                Tran.Commit();
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Slip successfully deleted!');", true);
                ChkForEdit.Checked = false;
                TDSlipNoForEdit.Visible = false;
                TDDDSlipNo.Visible = false;
                TxtSlipNo.Text = "";
                TxtHissabNo.Text = "";
                DDSlipNo.Items.Clear();
                ViewState["Hissab_No"] = 0;
            }
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }


    protected void btnprintvoucher_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@ID", ViewState["Hissab_No"]);
        param[1] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[2] = new SqlParameter("@PageName", "FrmProcessHissabByIssueNo");
        param[3] = new SqlParameter("@ProcessTypeID", 1);
        param[4] = new SqlParameter("@MasterCompanyID", Session["varcompanyid"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetVoucherDetail", param);

        switch (Session["varcompanyNo"].ToString())
        {
            case "16":
                if (DDProcessName.SelectedValue == "1")
                {
                    Session["rptFileName"] = "Reports/rptvoucher.rpt";
                }
                else
                {
                    Session["rptFileName"] = "Reports/rptvoucher_otherjob.rpt";
                }
                break;
            default:
                Session["rptFileName"] = "Reports/rptvoucher.rpt";
                break;
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptvoucher.xsd";
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
}