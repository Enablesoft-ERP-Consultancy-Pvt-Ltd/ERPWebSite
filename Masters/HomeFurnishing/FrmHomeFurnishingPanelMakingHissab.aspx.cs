using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_HomeFurnishing_FrmHomeFurnishingPanelMakingHissab : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            string Str = @"Select CI.CompanyId, CI.CompanyName 
                    From CompanyInfo CI(Nolock) 
                    JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                    Where CI.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" Order By CI.CompanyName 
                    Select Distinct a.ProcessID, PNM.PROCESS_NAME 
                    From HomeFurnishingMakingReceiveMaster a(Nolock)
                    JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = a.ProcessID 
                    Where a.CompanyID = " + Session["CurrentWorkingCompanyID"] + @"
                    Order By PNM.PROCESS_NAME ";

            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, Ds, 0, true, "--SELECT--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, Ds, 1, true, "--SELECT--");

            TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            CheckForEditSelectedChanges();
            ViewState["Hissab_No"] = 0;
        }
    }

    private void CheckForEditSelectedChanges()
    {
        if (ChkForEdit.Checked == true)
        {
            TDSlipNoForEdit.Visible = true;
            TDDDSlipNo.Visible = true;
        }
        else
        {
            TDSlipNoForEdit.Visible = false;
            TDDDSlipNo.Visible = false;
            TxtSlipNo.Text = "";
            DDSlipNo.Items.Clear();
            if (DDEmployerName.Items.Count > 0)
            {
                DDEmployerName.SelectedIndex = 0;
            }
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
                Str = @" Select Distinct EI.EmpId, EI.EmpName + case when isnull(ei.empcode, '') <> '' then ' [' + ei.empcode + ']' else '' end EmpName 
                From HomeFurnishingMakingHissab PH(Nolock) 
                JOIN EMpInfo EI(Nolock) ON EI.EmpID = PH.EmpID And EI.Blacklist = 0 
                Where PH.CompanyId = " + DDCompanyName.SelectedValue + " AND EI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" And 
                PH.ProcessId = " + DDProcessName.SelectedValue + " Order By EmpName";
            }
            else
            {
                Str = @" SELECT Distinct EI.EMPID, EI.EMPNAME + CASE WHEN EI.EMPCODE <> '' THEN ' [' + ISNULL(EI.EMPCODE, '') + ']' ELSE '' END EMPNAME 
                FROM HomeFurnishingMakingReceiveMaster a(Nolock) 
                JOIN Employee_HomeFurnishingMakingReceiveMaster EMP(Nolock) ON EMP.ProcessRecID = a.ProcessRecId And EMP.ProcessID = a.ProcessID 
                JOIN EMPINFO EI(Nolock) ON EI.EMPID = EMP.EMPID And EI.Blacklist = 0 
                WHere a.CompanyID = " + DDCompanyName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + " ORDER BY EMPNAME";
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
        if (DDProcessName.SelectedIndex > 0 && ChkForEdit.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDSlipNo, @"Select Distinct HissabNo, HissabNo HissabNo1 
            From HomeFurnishingMakingHissab a(Nolock) 
            Where a.CompanyId = " + DDCompanyName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + @" And 
            a.EmpID = " + DDEmployerName.SelectedValue + " Order By HissabNo1", true, "--SELECT--");
        }
        ViewState["Hissab_No"] = 0;
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        int VarEditVarible = 0;

        //*************CHECK DATE
        if (Convert.ToDateTime(TxtFromDate.Text) > Convert.ToDateTime(TxtDate.Text) || Convert.ToDateTime(TxtToDate.Text) > Convert.ToDateTime(TxtDate.Text))
        {

            ScriptManager.RegisterStartupScript(Page, GetType(), "date", "alert('Slip Date can not be less than From and To Date.');", true);
            return;
        }
        //*************
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //**********

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand("[PRO_SAVE_HOMEFURNISHINGMAKINGHISSAB]", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@CompanyID", DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@ProcessID", DDProcessName.SelectedValue);
            cmd.Parameters.AddWithValue("@EmpID", DDEmployerName.SelectedValue);
            cmd.Parameters.Add("@HissabNo", SqlDbType.Int);
            cmd.Parameters["@HissabNo"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters["@HissabNo"].Value = ViewState["Hissab_No"];
            cmd.Parameters.AddWithValue("@Date", TxtDate.Text);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);
            cmd.Parameters.AddWithValue("@UserID", Session["varuserid"]);
            cmd.Parameters.AddWithValue("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
            cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 250);
            cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            if (cmd.Parameters["@Msg"].Value.ToString() == "Data Inserted Successfully !")
            {
                ViewState["Hissab_No"] = cmd.Parameters["@HissabNo"].Value.ToString();
                Tran.Commit();
                TxtHissabNo.Text = ViewState["Hissab_No"].ToString();
                btnprintvoucher.Visible = true;
            }
            else
            {
                Tran.Rollback();
            }

            lblMessage.Visible = true;
            lblMessage.Text = cmd.Parameters["@Msg"].Value.ToString();
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
    protected void TxtSlipNo_TextChanged(object sender, EventArgs e)
    {
        lblMessage.Visible = false;
        if (TxtSlipNo.Text != "")
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
                @"Select Distinct CompanyId, ProcessID, EmpId, HissabNo, replace(convert(varchar(11), Date, 106), ' ', '-') Date 
                From HomeFurnishingMakingHissab(Nolock) 
                Where CompanyID = " + DDCompanyName.SelectedValue + " And HissabNo = " + TxtSlipNo.Text + "");
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
        btnprintvoucher.Visible = false;
        ViewState["Hissab_No"] = DDSlipNo.SelectedValue;
        if (DDSlipNo.SelectedIndex > 0)
        {
            TxtHissabNo.Text = DDSlipNo.Text;
            TxtDate.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select IsNull(replace(convert(varchar(11), Date, 106), ' ', '-'), '') Date 
            From HomeFurnishingMakingHissab(Nolock) Where HissabNo = " + DDSlipNo.SelectedValue).ToString();
            btnprintvoucher.Visible = true;
        }
    }
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@Hissab_No", ViewState["Hissab_No"]);
            param[1] = new SqlParameter("@processid", DDProcessName.SelectedValue);
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEPROCESSHISSAB", param);
            if (param[4].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + param[4].Value.ToString() + "');", true);
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

    protected void btnsearchemp_Click(object sender, EventArgs e)
    {
        if (txtWeaverIdNoscan.Text != "")
        {
            string str = "Select EmpID From EmpInfo(Nolock) Where EmpCode = '" + txtWeaverIdNoscan.Text + "'";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (DDEmployerName.Items.FindByValue(ds.Tables[0].Rows[0]["empid"].ToString()) != null)
                {
                    DDEmployerName.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
                    DDEmployerName_SelectedIndexChanged(sender, new EventArgs());
                }
                txtWeaverIdNoscan.Text = "";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('No Employee found on this Employee code.')", true);
                txtWeaverIdNoscan.Focus();
            }
        }
    }
    protected void TxtToDate_TextChanged(object sender, EventArgs e)
    {
        TxtDate.Text = TxtToDate.Text;
    }
    protected void BtnPriview_Click(object sender, EventArgs e)
    {
        ReportType(1);
    }
    protected void btnprintvoucher_Click(object sender, EventArgs e)
    {
        ReportType(2);
    }
    protected void ReportType(int ReportType)
    {
        SqlParameter[] array = new SqlParameter[7];
        array[0] = new SqlParameter("@CompanyID", SqlDbType.Int);
        array[1] = new SqlParameter("@ProcessID", SqlDbType.Int);
        array[2] = new SqlParameter("@EmpID", SqlDbType.Int);
        array[3] = new SqlParameter("@FromDate", SqlDbType.DateTime);
        array[4] = new SqlParameter("@ToDate", SqlDbType.DateTime);
        array[5] = new SqlParameter("@ReportType", SqlDbType.Int);
        array[6] = new SqlParameter("@SlipNo", SqlDbType.Int);

        array[0].Value = DDCompanyName.SelectedValue;
        array[1].Value = DDProcessName.SelectedValue;
        array[2].Value = DDEmployerName.SelectedValue;
        array[3].Value = TxtFromDate.Text;
        array[4].Value = TxtToDate.Text;
        array[5].Value = ReportType;
        array[6].Value = ViewState["Hissab_No"];

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_HomeFurnishingMakingHissabForReport", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["GetDataset"] = ds;
            if (ReportType == 1)
            {
                Session["rptFileName"] = "~\\Reports\\RptHomeFurnishingMakingReportForHissab.rpt";
                Session["dsFileName"] = "~\\ReportSchema\\RptHomeFurnishingMakingReportForHissab.xsd";
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\rptvoucher_otherjob.rpt";
                Session["dsFileName"] = "~\\ReportSchema\\rptvoucher.xsd";
            }

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