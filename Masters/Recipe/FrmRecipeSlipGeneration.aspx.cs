using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Recipe_FrmRecipeSlipGeneration : System.Web.UI.Page
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
                JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + " And CA.MasterCompanyid = " + Session["varCompanyId"] + @" 
                Order By CI.CompanyName
                Select ID, [Name] From RecipeMaster (Nolock) Where MasterCompanyID = 16 Order By [Name] ";

            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, Ds, 0, true, "--SELECT--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
                CompanyNameSelectedIndexChanged();
            }
            
            UtilityModule.ConditionalComboFillWithDS(ref DDRecipeName, Ds, 1, true, "--SELECT--");

            TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtSlipDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewState["Hissab_No"] = 0;
        }
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanyNameSelectedIndexChanged();
    }
    private void CompanyNameSelectedIndexChanged()
    {
        string Str = "";
        if (ChkForEdit.Checked == true)
        {
            Str = @"Select Distinct a.ProcessID, PNM.PROCESS_NAME 
                From RecipeSlipGenerationMaster a(Nolock) 
                JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = a.ProcessID 
                Where a.MasterCompanyID = " + Session["varCompanyId"] + " And a.CompanyID = " + DDCompanyName.SelectedValue + @" 
                Order By PNM.PROCESS_NAME ";
        }
        else
        {
            Str = @"Select Distinct PNM.Process_Name_Id, PNM.Process_Name 
                From PROCESS_NAME_MASTER PNM(Nolock)
                JOIN RecipeMaster RM(Nolock) ON RM.ProcessID = PNM.PROCESS_NAME_ID 
                Where PNM.MasterCompanyId = " + Session["varCompanyId"] + " Order By PNM.Process_Name";
        }
        UtilityModule.ConditionalComboFill(ref DDProcessName, Str, true, "--SELECT--");
        if (DDProcessName.Items.Count > 0)
        {
            DDProcessName.SelectedIndex = 1;
            ProcessNameSelectedIndexChanged();
        }

        ViewState["Hissab_No"] = 0;
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DGDetail.DataSource = null;
        DGDetail.DataBind();
        ProcessNameSelectedIndexChanged();
    }
    private void ProcessNameSelectedIndexChanged()
    {
        if (DDProcessName.SelectedIndex > 0)
        {
            if (ChkForEdit.Checked == true)
            {
                string Str = @"Select Distinct SlipNo, SlipNo SlipNo1 From RecipeSlipGenerationMaster(Nolock) Where MasterCompanyID = " + Session["varCompanyId"] + @" And 
                CompanyID = " + DDCompanyName.SelectedValue + " And ProcessID = " + DDProcessName.SelectedValue + " Order By SlipNo";
                UtilityModule.ConditionalComboFill(ref DDSlipNo, Str, true, "--SELECT--");
            }
            ShowButton();
        }
    }
    private void ShowButton()
    {
        if (DDCompanyName.SelectedIndex > 0 && DDProcessName.SelectedIndex > 0 && ChkForEdit.Checked == false)
        {
            BtnShowData.Visible = true;
        }
        else
        {
            BtnShowData.Visible = false;
        }
    }
    protected void BtnShowData_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        selectall.Visible = true;
        TxtSlipNo.Text = "";
        ShowDataInGrid();
    }
    private void ShowDataInGrid()
    {
        try
        {
            if (DDCompanyName.SelectedIndex > 0 && DDProcessName.SelectedIndex > 0)
            {
                if (ChkForEdit.Checked == false)
                {
                    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("PRO_GET_RecipeDataForSlipGeneration", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300;

                    cmd.Parameters.AddWithValue("@CompanyId", DDCompanyName.SelectedValue);
                    cmd.Parameters.AddWithValue("@Toprocessid", DDProcessName.SelectedValue);
                    cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
                    cmd.Parameters.AddWithValue("@TODate", TxtToDate.Text);
                    cmd.Parameters.AddWithValue("@Mastercompanyid", Session["varcompanyid"]);
                    cmd.Parameters.AddWithValue("@ReceipeID", DDRecipeName.SelectedValue);

                    DataSet Ds = new DataSet();
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    cmd.ExecuteNonQuery();
                    ad.Fill(Ds);
                    DGDetail.DataSource = Ds;
                    DGDetail.DataBind();
                    if (Ds.Tables[0].Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
        }
    }
    private void ForCheckAllRows()
    {
        for (int i = 0; i < DGDetail.Rows.Count; i++)
        {
            GridViewRow row = DGDetail.Rows[i];
            if (Convert.ToInt32(DGDetail.Rows[i].Cells[12].Text) == 1)
            {
                ((CheckBox)row.FindControl("Chkbox")).Checked = true;
            }
            else
            {
                ((CheckBox)row.FindControl("Chkbox")).Checked = false;
            }
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        string StockNo = "";
        //*************CHECK DATE
        if (Convert.ToDateTime(TxtFromDate.Text) > Convert.ToDateTime(TxtSlipDate.Text) || Convert.ToDateTime(TxtToDate.Text) > Convert.ToDateTime(TxtSlipDate.Text))
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

        for (int i = 0; i < DGDetail.Rows.Count; i++)
        {
            if (((CheckBox)DGDetail.Rows[i].FindControl("Chkbox")).Checked == true)
            {
                Label lblRecipeNameID = (Label)DGDetail.Rows[i].FindControl("lblRecipeNameID");
                Label lblarea = (Label)DGDetail.Rows[i].FindControl("lblarea");
                StockNo = StockNo + DGDetail.DataKeys[i].Value + '|' + lblRecipeNameID.Text + '|' + lblarea.Text + '~';
            }
        }
        if (StockNo == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "date", "alert('Pls select atleast one stock no');", true);
            return;
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara1 = new SqlParameter[10];
            _arrpara1[0] = new SqlParameter("@CompanyId", DDCompanyName.SelectedValue);
            _arrpara1[1] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
            _arrpara1[2] = new SqlParameter("@SlipID", 0);
            _arrpara1[2].Direction = ParameterDirection.Output;
            _arrpara1[3] = new SqlParameter("@SlipDate", TxtSlipDate.Text);
            _arrpara1[4] = new SqlParameter("@FromDate", TxtFromDate.Text);
            _arrpara1[5] = new SqlParameter("@ToDate", TxtToDate.Text);
            _arrpara1[6] = new SqlParameter("@StockNo", StockNo);
            _arrpara1[7] = new SqlParameter("@Userid", Session["varuserid"]);
            _arrpara1[8] = new SqlParameter("@MasterCompanyID", Session["varcompanyid"]);
            _arrpara1[9] = new SqlParameter("@Message", SqlDbType.NVarChar, 250);
            _arrpara1[9].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_RecipeSlipGeneration", _arrpara1);
            ViewState["Hissab_No"] = _arrpara1[2].Value;
            Tran.Commit();
            lblMessage.Visible = true;
            lblMessage.Text = _arrpara1[9].Value.ToString();
            ShowDataInGrid();
            TxtSlipNo.Text = ViewState["Hissab_No"].ToString();
            ChkForAllSelect.Checked = false;
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
        TxtSlipNo.Text = "";
        BtnDelete.Visible = false;
        TDSlipNoForEdit.Visible = false;
        TDDDSlipNo.Visible = false;
        DDSlipNo.Items.Clear();

        if (ChkForEdit.Checked == true)
        {
            TDSlipNoForEdit.Visible = true;
            TDDDSlipNo.Visible = true;
            BtnDelete.Visible = true;
        }
        CompanyNameSelectedIndexChanged();
    }
    protected void TxtSlipNoForEdit_TextChanged(object sender, EventArgs e)
    {
        lblMessage.Visible = false;
        if (TxtSlipNoForEdit.Text != "")
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
                @"Select Top 1 CompanyID, ProcessID, SlipNo, Replace(Convert(Varchar(11), SlipDate, 106), ' ', '-') SlipDate, 
                Replace(Convert(Varchar(11), FromDate, 106), ' ', '-') FromDate, Replace(Convert(Varchar(11), ToDate, 106), ' ', '-') ToDate 
                From RecipeSlipGenerationMaster(Nolock) Where MasterCompanyID = " + Session["varCompanyId"] + " And CompanyID = " + DDCompanyName.SelectedValue + " And SlipNo = " + TxtSlipNoForEdit.Text);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                DDProcessName.SelectedValue = Ds.Tables[0].Rows[0]["ProcessID"].ToString();
                ProcessNameSelectedIndexChanged();
                TxtSlipNo.Text = Ds.Tables[0].Rows[0]["SlipNo"].ToString();
                TxtSlipDate.Text = Ds.Tables[0].Rows[0]["SlipDate"].ToString();
                TxtFromDate.Text = Ds.Tables[0].Rows[0]["FromDate"].ToString();
                TxtToDate.Text = Ds.Tables[0].Rows[0]["ToDate"].ToString();
                DDSlipNo.SelectedValue = Ds.Tables[0].Rows[0]["SlipNo"].ToString();
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
            TxtSlipNo.Text = DDSlipNo.Text;
            //TxtSlipDate.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select IsNull(replace(convert(varchar(11),Date,106), ' ','-'),'') as Date From PROCESS_HISSAB Where HissabNo=" + DDSlipNo.SelectedValue + "").ToString();
        }
        ShowDataInGrid();
    }
    protected void BtnPriview_Click(object sender, EventArgs e)
    {
        SqlParameter[] array = new SqlParameter[1];
        array[0] = new SqlParameter("@SlipID", SqlDbType.Int);
        array[0].Value = ViewState["Hissab_No"];

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GET_RecipeSlipGenerationForReport", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "Reports/RptRecipeSlipGeneration.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRecipeSlipGeneration.xsd";
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
    protected void DGDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGDetail.PageIndex = e.NewPageIndex;
        ShowDataInGrid();
    }
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@SlipID", ViewState["Hissab_No"]);
            param[1] = new SqlParameter("@CompanyID", DDCompanyName.SelectedValue);
            param[2] = new SqlParameter("@Processid", DDProcessName.SelectedValue);
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            param[4] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            param[5] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[5].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteRecipeSlipGeneration", param);
            if (param[5].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + param[5].Value.ToString() + "');", true);
                Tran.Rollback();
            }
            else
            {

                Tran.Commit();
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Slip successfully deleted!');", true);
                ChkForEdit.Checked = false;
                BtnDelete.Visible = false;
                TDSlipNoForEdit.Visible = false;
                TDDDSlipNo.Visible = false;
                TxtSlipNo.Text = "";
                TxtSlipNo.Text = "";
                DDSlipNo.Items.Clear();
                ViewState["Hissab_No"] = 0;
                ShowButton();
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
    protected void TxtToDate_TextChanged(object sender, EventArgs e)
    {
        TxtSlipDate.Text = TxtToDate.Text;
    }

    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblRecipeNameID = (Label)e.Row.FindControl("lblRecipeNameID");
            string CellValue = lblRecipeNameID.Text;
            if (CellValue == "0")
            {
                e.Row.BackColor = System.Drawing.Color.Red;
            }
        }
    }
}