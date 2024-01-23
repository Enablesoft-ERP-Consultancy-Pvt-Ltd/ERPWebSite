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
using System.Text;
using System.Data.SqlTypes;
using System.Net;

public partial class Masters_Campany_frmCompanyInfo : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }        
        //newPreview.ImageUrl = "~/images/Logo/1_compney.gif";
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDState, "select stateid,statename from state_master", true, "--select state--");
            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "9"://for hafizia "9"
                    TRMobileState.Visible = true;
                    break;
                case "20"://for hafizia "20"
                    TRMobileState.Visible = true;
                    Label24.Text = "District Name";
                    Label25.Text = "District Code";
                    break;
                default:
                    TRMobileState.Visible = false;
                    break;
            }
            txtid.Text = "0";
            Fill_Grid();
            Fill_Combo();

        }
        lblerr.Visible = false;
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    private void Fill_Grid()
    {
        dgComapny.DataSource = Fill_Grid_Data();
        dgComapny.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string strsql = "select * from companyinfo where MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmCompanyInfo.aspx");
            Logs.WriteErrorLog("Masters_Campany_CompInfo|Fill_Grid_Data|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        return ds;
    }
    private void Fill_Combo()
    {
        #region on 29-Nov-2012
        //CommanFunction.FillCombo(CmbSignatory, "select SignatoryId,SignatoryName from Signatory");
        //CommanFunction.FillCombo(dropListBank, "select BankId,BankName from Bank ");
        //CommanFunction.FillCombo(dropDBK1Bank, "select BankId,BankName from Bank ");
        //CommanFunction.FillCombo(dropDBK2Bank, "select BankId,BankName from Bank");
        #endregion
        string str = @"select SignatoryId,SignatoryName from Signatory where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @"
                     select BankId,BankName from Bank where MasterCompanyid=" + Session["varMasterCompanyIDForERP"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        CommanFunction.FillComboWithDS(CmbSignatory, ds, 0);
        CommanFunction.FillComboWithDS(dropListBank, ds, 1);
        CommanFunction.FillComboWithDS(dropDBK1Bank, ds, 1);
        CommanFunction.FillComboWithDS(dropDBK2Bank, ds, 1);
        Session["ReportPath"] = "Reports/Company.rpt";
        Session["CommanFormula"] = "";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Validated();
        if (txtCompName.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[41];
                _arrPara[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@CompanyName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@CompAddr1", SqlDbType.NVarChar, 400);
                _arrPara[3] = new SqlParameter("@CompAddr2", SqlDbType.NVarChar, 200);
                _arrPara[4] = new SqlParameter("@CompAddr3", SqlDbType.NVarChar, 200);
                _arrPara[5] = new SqlParameter("@CompFax", SqlDbType.NVarChar, 50);
                _arrPara[6] = new SqlParameter("@CompTel", SqlDbType.NVarChar, 50);
                _arrPara[7] = new SqlParameter("@Bankid", SqlDbType.Int);
                _arrPara[8] = new SqlParameter("@RBIcode", SqlDbType.NVarChar, 50);
                _arrPara[9] = new SqlParameter("@IECode", SqlDbType.NVarChar, 50);
                _arrPara[10] = new SqlParameter("@PANNr", SqlDbType.NVarChar, 50);
                _arrPara[11] = new SqlParameter("@EDPNo", SqlDbType.NVarChar, 50);
                _arrPara[12] = new SqlParameter("@DbkBank1", SqlDbType.Int);
                _arrPara[13] = new SqlParameter("@DbkBank2", SqlDbType.Int);
                _arrPara[14] = new SqlParameter("@CurAcctNo", SqlDbType.NVarChar, 50);
                _arrPara[15] = new SqlParameter("@RollMarkHead", SqlDbType.NVarChar, 50);
                _arrPara[16] = new SqlParameter("@Sigantory", SqlDbType.Int);
                _arrPara[17] = new SqlParameter("@Dbk1ACNo", SqlDbType.NVarChar, 50);
                _arrPara[18] = new SqlParameter("@Dbk2ACNo", SqlDbType.NVarChar, 50);
                _arrPara[19] = new SqlParameter("@UPTTNo", SqlDbType.NVarChar, 50);
                _arrPara[20] = new SqlParameter("@CSTNo", SqlDbType.NVarChar, 50);
                _arrPara[21] = new SqlParameter("@RCMCNo", SqlDbType.NVarChar, 50);
                _arrPara[22] = new SqlParameter("@GSPNo", SqlDbType.NVarChar, 50);
                _arrPara[23] = new SqlParameter("@CoSyn", SqlDbType.NVarChar, 50);
                _arrPara[24] = new SqlParameter("@ExpRef", SqlDbType.NVarChar, 50);
                _arrPara[25] = new SqlParameter("@OpeningBalance", SqlDbType.Float);
                _arrPara[26] = new SqlParameter("@Instruction", SqlDbType.NVarChar, 1000);
                _arrPara[27] = new SqlParameter("@TinNo", SqlDbType.NVarChar, 50);
                _arrPara[28] = new SqlParameter("@Email", SqlDbType.NVarChar, 100);
                _arrPara[29] = new SqlParameter("@Declaration1", SqlDbType.NVarChar, 500);
                _arrPara[30] = new SqlParameter("@Declaration2", SqlDbType.NVarChar, 500);
                _arrPara[31] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[32] = new SqlParameter("@masterCompanyId", SqlDbType.Int);
                // _arrPara[31] = new SqlParameter("@Image1",SqlDbType.Image);
                _arrPara[33] = new SqlParameter("@GSTNo", SqlDbType.NVarChar,50);
                _arrPara[34] = new SqlParameter("@WebSiteName", SqlDbType.NVarChar, 200);
                _arrPara[35] = new SqlParameter("@FactoryAddress", SqlDbType.VarChar, 300);
                _arrPara[36] = new SqlParameter("@MobileNo", SqlDbType.VarChar, 20);
                _arrPara[37] = new SqlParameter("@StateId", SqlDbType.Int);
                _arrPara[38] = new SqlParameter("@LUTARNNO", SqlDbType.VarChar,50);
                _arrPara[39] = new SqlParameter("@LUTIssueDate", SqlDbType.DateTime);
                _arrPara[40] = new SqlParameter("@LUTExpiryDate", SqlDbType.DateTime);

                System.Data.SqlTypes.SqlDateTime getDate;
                //set DateTime null
                getDate = SqlDateTime.Null;

                _arrPara[0].Direction = ParameterDirection.InputOutput;

                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                _arrPara[1].Value = txtCompName.Text.ToUpper();
                _arrPara[2].Value = txtAddress1.Text.ToUpper().Trim();
                _arrPara[3].Value = txtAddress2.Text.ToUpper().Trim();
                _arrPara[4].Value = txtAddress3.Text.ToUpper().Trim();
                _arrPara[5].Value = txtCompFax.Text.ToUpper();
                _arrPara[6].Value = txtPhone.Text.ToUpper();
                _arrPara[7].Value = dropListBank.SelectedValue;
                _arrPara[8].Value = txtRBICode.Text.ToUpper();
                _arrPara[9].Value = txtIEcode.Text.ToUpper();
                _arrPara[10].Value = txtPAN.Text.ToUpper();
                _arrPara[11].Value = txtEDPCode.Text.ToUpper();
                _arrPara[12].Value = dropDBK1Bank.SelectedValue;
                _arrPara[13].Value = dropDBK2Bank.SelectedValue;
                _arrPara[14].Value = txtCACNo.Text.ToUpper();
                _arrPara[15].Value = txtRollMhead.Text.ToUpper();
                _arrPara[16].Value = CmbSignatory.SelectedIndex < 0 ? "0" : CmbSignatory.SelectedValue;
                _arrPara[17].Value = txtDBK1Ac.Text.ToUpper();
                _arrPara[18].Value = txtDBK2Ac.Text.ToUpper();
                _arrPara[19].Value = txtUPTTNo.Text.ToUpper();
                _arrPara[27].Value = txtUPTTNo.Text.ToUpper();
                _arrPara[20].Value = txtCSTNo.Text.ToUpper();
                _arrPara[23].Value = txtCoSynonyms.Text.ToUpper();
                _arrPara[24].Value = txtExpref.Text.ToUpper();
                _arrPara[26].Value = multitext.Text.ToUpper();
                _arrPara[28].Value = txtEmail.Text;
                _arrPara[29].Value = txtdecloration1.Text.ToUpper();
                _arrPara[30].Value = txtdecloration2.Text.ToUpper();
                _arrPara[31].Value = Session["varuserid"].ToString();
                _arrPara[32].Value = Session["varMasterCompanyIDForERP"].ToString();
                _arrPara[33].Value = txtGSTNo.Text;
                _arrPara[34].Value = txtWebSiteName.Text;
                _arrPara[35].Value = txtFactoryAddress.Text.ToUpper().Trim();
                _arrPara[36].Value = txtMobileNo.Text;
                _arrPara[37].Value = DDState.SelectedIndex != -1 ? DDState.SelectedValue : "0";
                _arrPara[38].Value = txtlutarnno.Text;
                _arrPara[39].Value = txtLUTIssueDate.Text == "" ? getDate : Convert.ToDateTime(txtLUTIssueDate.Text);
                _arrPara[40].Value = txtLUTExpiryDate.Text == "" ? getDate : Convert.ToDateTime(txtLUTExpiryDate.Text);   

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_COMPANY_INFO", _arrPara);
                txtid.Text = "0";
                txtCompName.Text = "";
                txtAddress1.Text = "";
                txtAddress2.Text = "";
                txtAddress3.Text = "";
                txtCompFax.Text = "";
                txtPhone.Text = "";
                dropListBank.SelectedIndex = -1;
                txtRBICode.Text = "";
                txtIEcode.Text = "";
                txtPAN.Text = "";
                txtEDPCode.Text = "";
                dropDBK1Bank.SelectedIndex = -1;
                dropDBK2Bank.SelectedIndex = -1;
                txtCSTNo.Text = "";
                txtRollMhead.Text = "";
                CmbSignatory.SelectedIndex = -1;
                txtDBK1Ac.Text = "";
                txtDBK2Ac.Text = "";
                txtUPTTNo.Text = "";
                txtCSTNo.Text = "";
                txtCoSynonyms.Text = "";
                txtExpref.Text = "";
                multitext.Text = "";
                txtEmail.Text = "";
                txtdecloration1.Text = "";
                txtdecloration2.Text = "";
                txtGSTNo.Text = "";
                txtWebSiteName.Text = "";
                txtFactoryAddress.Text = "";
                txtMobileNo.Text = "";
                DDState.SelectedIndex = -1;
                txtLUTIssueDate.Text = "";
                txtLUTExpiryDate.Text = "";
                string compid = _arrPara[0].Value.ToString();
                FileInfo TheFile = new FileInfo(Server.MapPath("~/images/Logo/") + compid + "_compney.gif");
                if (TheFile.Exists)
                {
                    File.Delete(MapPath("~/images/Logo/") + compid + "_compney.gif");
                }
                if (compneyImage.FileName != null && compneyImage.FileName != "")
                {
                    string imgCompney = Server.MapPath("~/Images/Logo/") + compid + "_company.gif";
                    compneyImage.SaveAs(imgCompney);
                }
                newPreview1.ImageUrl = "";
                lblerr.Visible = true;
                lblerr.Text = "Save Details............";
                Tran.Commit();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmCompanyInfo.aspx");
                Logs.WriteErrorLog("Masters_Campany_CompInfo|btnSave_Click|" + ex.Message);
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
                Fill_Grid();
            }

        }
        else
        {
            if (lblerr.Text == "Company Name already exists............")
            {
                lblerr.Visible = true;
                lblerr.Text = "Company Name already exists............";
            }
            else
            {
                lblerr.Visible = true;
                lblerr.Text = "Please Fill Details............";
            }

        }
        btnSave.Text = "Save";
    }
    protected void dgComapny_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["CompanyId"] = dgComapny.SelectedDataKey.Value.ToString();
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select Replace(convert(nvarchar(11),Companyinfo.LUTIssueDate,106),' ','-') As LUTIssueDate,Replace(convert(nvarchar(11),Companyinfo.LUTExpiryDate,106),' ','-') As LUTExpiryDate, * from Companyinfo WHERE CompanyId=" + ViewState["CompanyId"]);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["CompanyId"].ToString();
                txtCompName.Text = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                txtAddress1.Text = ds.Tables[0].Rows[0]["CompAddr1"].ToString();
                txtAddress2.Text = ds.Tables[0].Rows[0]["CompAddr2"].ToString();
                txtAddress3.Text = ds.Tables[0].Rows[0]["CompAddr3"].ToString();
                txtCompFax.Text = ds.Tables[0].Rows[0]["CompFax"].ToString();
                txtPhone.Text = ds.Tables[0].Rows[0]["CompTel"].ToString();
                dropListBank.SelectedValue = ds.Tables[0].Rows[0]["BankId"].ToString();
                txtRBICode.Text = ds.Tables[0].Rows[0]["RBICode"].ToString();
                txtIEcode.Text = ds.Tables[0].Rows[0]["IECode"].ToString();
                txtPAN.Text = ds.Tables[0].Rows[0]["PANNr"].ToString();
                txtEDPCode.Text = ds.Tables[0].Rows[0]["EDPNo"].ToString();
                dropDBK1Bank.SelectedValue = ds.Tables[0].Rows[0]["DbkBank1"].ToString();
                dropDBK2Bank.SelectedValue = ds.Tables[0].Rows[0]["DbkBank2"].ToString();
                txtCSTNo.Text = ds.Tables[0].Rows[0]["CurAcctNo"].ToString();
                txtRollMhead.Text = ds.Tables[0].Rows[0]["RollMarkHead"].ToString();
               // CmbSignatory.SelectedValue = ds.Tables[0].Rows[0]["Sigantory"].ToString();   
                

                if (CmbSignatory.Items.Count > 0)
                {
                    if (CmbSignatory.Items.FindByValue(ds.Tables[0].Rows[0]["Sigantory"].ToString()) != null)
                    {
                        CmbSignatory.SelectedValue = ds.Tables[0].Rows[0]["Sigantory"].ToString();
                    }                    
                }
                txtDBK1Ac.Text = ds.Tables[0].Rows[0]["Dbk1ACNo"].ToString();
                txtDBK2Ac.Text = ds.Tables[0].Rows[0]["Dbk2ACNo"].ToString();
                txtUPTTNo.Text = ds.Tables[0].Rows[0]["UPTTNo"].ToString();
                txtCSTNo.Text = ds.Tables[0].Rows[0]["CSTNo"].ToString();
                txtCoSynonyms.Text = ds.Tables[0].Rows[0]["CoSyn"].ToString();
                txtExpref.Text = ds.Tables[0].Rows[0]["ExpRef"].ToString();
                multitext.Text = ds.Tables[0].Rows[0]["Instruction"].ToString();
                txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                txtdecloration1.Text = ds.Tables[0].Rows[0]["Declaration1"].ToString();
                txtdecloration2.Text = ds.Tables[0].Rows[0]["Declaration2"].ToString();
                newPreview1.ImageUrl = "~/Images/Logo/" + txtid.Text + "_company.gif";
                // newPreview.ResolveUrl(Server.MapPath("~/images/Logo/" + txtid.Text + "_compney.gif"));
                txtGSTNo.Text = ds.Tables[0].Rows[0]["GSTNo"].ToString();
                txtWebSiteName.Text = ds.Tables[0].Rows[0]["WebSiteName"].ToString();
                txtFactoryAddress.Text = ds.Tables[0].Rows[0]["FactoryAddress"].ToString();
                txtlutarnno.Text = ds.Tables[0].Rows[0]["lutarnno"].ToString();
                txtLUTIssueDate.Text = ds.Tables[0].Rows[0]["LUTIssueDate"].ToString();
               txtLUTExpiryDate.Text = ds.Tables[0].Rows[0]["LUTExpiryDate"].ToString();

                if (Session["varCompanyNo"].ToString() == "9" || Session["varCompanyNo"].ToString() == "20")
                {
                    txtMobileNo.Text = ds.Tables[0].Rows[0]["MobileNo"].ToString();
                    DDState.SelectedValue = ds.Tables[0].Rows[0]["Stateid"].ToString();
                }
               
            }
            btnSave.Text = "Update";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmCompanyInfo.aspx");
            Logs.WriteErrorLog("Masters_Campany_CompInfo|dgComapny_SelectedIndexChanged|" + ex.Message);
        }
    }
    protected void dgComapny_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgComapny, "select$" + e.Row.RowIndex);
        }
    }
    protected void dgComapny_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgComapny.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }

    protected void B_Click(object sender, EventArgs e)
    {
        CommanFunction.FillCombo(dropListBank, "select BankId,BankName from Bank where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        CommanFunction.FillCombo(CmbSignatory, "select SignatoryId,SignatoryName from Signatory where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        CommanFunction.FillCombo(dropListBank, "select BankId,BankName from Bank where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
    }
    protected void x_Click(object sender, EventArgs e)
    {
        #region on 29-Nov-2012
        //CommanFunction.FillCombo(dropListBank, "select BankId,BankName from Bank ");
        //CommanFunction.FillCombo(dropDBK1Bank, "select BankId,BankName from Bank ");
        //CommanFunction.FillCombo(dropDBK2Bank, "select BankId,BankName from Bank");
        #endregion
        string str = "select BankId,BankName from Bank where MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        CommanFunction.FillComboWithDS(dropListBank, ds, 0);
        CommanFunction.FillComboWithDS(dropDBK1Bank, ds, 0);
        CommanFunction.FillComboWithDS(dropDBK2Bank, ds, 0);
    }
    private void Validated()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql;
            if (btnSave.Text == "Update")
            {
                strsql = "Select isnull(max(CompanyId),0) from CompanyInfo where CompanyId !=" + ViewState["CompanyId"].ToString() + " and CompanyName='" + txtCompName.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            else
            {
                strsql = "Select isnull(max(CompanyId),0) from CompanyInfo where CompanyName='" + txtCompName.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            con.Open();
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, strsql));
            if (id > 0)
            {
                lblerr.Visible = true;
                lblerr.Text = "Company Name already exists............";
                txtCompName.Text = "";
                txtCompName.Focus();
            }
            else
            {
                lblerr.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmCompanyInfo.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void btnupload_Click(object sender, EventArgs e)
    {
        // compneyImage.Equals=
    }
    protected void btnnew_Click(object sender, EventArgs e)
    {
        txtid.Text = "0";
        txtCompName.Text = "";
        txtAddress1.Text = "";
        txtAddress2.Text = "";
        txtAddress3.Text = "";
        txtCompFax.Text = "";
        txtPhone.Text = "";
        dropListBank.SelectedIndex = -1;
        txtRBICode.Text = "";
        txtIEcode.Text = "";
        txtPAN.Text = "";
        txtEDPCode.Text = "";
        dropDBK1Bank.SelectedIndex = -1;
        dropDBK2Bank.SelectedIndex = -1;
        txtCSTNo.Text = "";
        txtRollMhead.Text = "";
        CmbSignatory.SelectedIndex = -1;
        txtDBK1Ac.Text = "";
        txtDBK2Ac.Text = "";
        txtUPTTNo.Text = "";
        txtCSTNo.Text = "";
        txtCoSynonyms.Text = "";
        txtExpref.Text = "";
        multitext.Text = "";
        txtEmail.Text = "";
        txtdecloration1.Text = "";
        txtdecloration2.Text = "";
        newPreview1.ImageUrl = "";
        btnSave.Text = "Save";
        txtGSTNo.Text = "";
        txtWebSiteName.Text = "";
        txtFactoryAddress.Text = "";
        txtMobileNo.Text = "";
        DDState.SelectedIndex = -1;
        txtLUTIssueDate.Text = "";
        txtLUTExpiryDate.Text = "";
    }
    protected void rpt0_Click(object sender, EventArgs e)
    {
        report();
    }
    private void report()
    {
        string qry = @"SELECT CompanyName,CompAddr1,CompAddr2,CompAddr3,CompFax,CompTel,RBICode,IECode,PANNr,CSTNo,TinNo,SignatoryName 
                       FROM CompanyInfo INNER JOIN  Signatory ON CompanyInfo.Sigantory=Signatory.SignatoryId where CompanyInfo.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\CompanyReportNew.rpt";
            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\CompanyReportNew.xsd";
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
    protected void dgComapny_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        //if (e.Row.RowType == DataControlRowType.Header)
        //    e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        //if (e.Row.RowType == DataControlRowType.DataRow &&
        // e.Row.RowState == DataControlRowState.Alternate)
        // e.Row.CssClass = "alternate";
    }
    protected void dgComapny_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _array = new SqlParameter[4];
            _array[0] = new SqlParameter("@CompanyId", dgComapny.DataKeys[e.RowIndex].Value);
            _array[1] = new SqlParameter("@Message", SqlDbType.NVarChar, 100);
            _array[2] = new SqlParameter("@VarUserId", Session["varuserid"].ToString());
            _array[3] = new SqlParameter("@VarCompanyId", Session["varMasterCompanyIDForERP"].ToString());
            _array[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteCompany", _array);
            lblErr1.Visible = true;
            lblErr1.Text = _array[1].Value.ToString();
            if (lblErr1.Text == "Data Deleted Successfully....")
            {
                string imgCompney = Server.MapPath("~/images/Logo/") + ViewState["CompanyId"] + "_compney.gif";
                File.Delete(imgCompney);
            }
            Tran.Commit();
            Fill_Grid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmCompanyInfo.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    }
}
