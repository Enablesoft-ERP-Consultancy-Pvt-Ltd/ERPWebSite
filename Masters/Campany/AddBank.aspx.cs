using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Campany_frmBank1 : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["BankId"] = "0";
            txtBankName.Focus();
            Fill_Grid();
            UtilityModule.ConditonalChkBoxListFill(ref chkBankCategory, "select CategoryId,CategoryName from Bankcategory");
            UtilityModule.ConditionalComboFill(ref DDcurrency, "select CurrencyId,CurrencyName from currencyinfo order by CurrencyName", true, "--Plz Select Currency--");

            //SqlhelperEnum.FillCheckBoxlist(AllEnums.MasterTables.BankCategory, chkBankCategory, pID: "CategoryId", pName: "CategoryName");
            //SqlhelperEnum.FillDropDown(AllEnums.MasterTables.Currencyinfo, DDcurrency, pID: "CurrencyId", pName: "Currencyname", Selecttext: "--Plz Select Currency--", pFillBlank: true);
        }
        lbblerr.Visible = false;
    }
    private void Fill_Grid()
    {
        dgBank.DataSource = Fill_Grid_Data();
        dgBank.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "select * from Bank Where MasterCompanyId=" + Session["varCompanyId"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_Campany_frmBank|Fill_Grid_Data|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmBank1.apx");
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
    protected void dgBank_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgBank, "Select$" + e.Row.RowIndex);
        }
    }
    protected void dgBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["BankId"] = dgBank.SelectedDataKey.Value;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string str = "select * from Bank where BankId=" + ViewState["BankId"] + @"
                     select CategoryId from BankCategory_Detail Where BankId=" + ViewState["BankId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtBankName.Text = ds.Tables[0].Rows[0]["BankName"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["Street"].ToString();
                txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                txtPhn.Text = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
                txtAcc.Text = ds.Tables[0].Rows[0]["ACNo"].ToString();
                TxtAdCode.Text = ds.Tables[0].Rows[0]["ADICode"].ToString();
                txtState.Text = ds.Tables[0].Rows[0]["State"].ToString();
                txtFaxNo.Text = ds.Tables[0].Rows[0]["FaxNo"].ToString();
                txtCode.Text = ds.Tables[0].Rows[0]["SwiftCode"].ToString();
                txtCountry.Text = ds.Tables[0].Rows[0]["Country"].ToString();
                txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                txtMisc.Text = ds.Tables[0].Rows[0]["ContectPerson"].ToString();
                txtiban.Text = ds.Tables[0].Rows[0]["Iban"].ToString();
                txtbic.Text = ds.Tables[0].Rows[0]["Bic"].ToString();
                txtifscode.Text = ds.Tables[0].Rows[0]["Ifscode"].ToString();
                txtacode.Text = ds.Tables[0].Rows[0]["Adcode"].ToString();
                txtbranchcode.Text = ds.Tables[0].Rows[0]["Branchcode"].ToString();
                txtmicrcode.Text = ds.Tables[0].Rows[0]["Micrcode"].ToString();
                DDcurrency.SelectedValue = ds.Tables[0].Rows[0]["CurrencyId"].ToString();
                DDActype.SelectedValue = ds.Tables[0].Rows[0]["AccountType"].ToString();
                txtaccountname.Text = ds.Tables[0].Rows[0]["accountname"].ToString();
                txtPostcode.Text = ds.Tables[0].Rows[0]["postcode"].ToString();
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    for (int j = 0; j < chkBankCategory.Items.Count; j++)
                    {
                        if (chkBankCategory.Items[j].Value == ds.Tables[1].Rows[i]["CategoryId"].ToString())
                        {
                            chkBankCategory.Items[j].Selected = true;
                        }
                    }
                }
            }
            BtnSave.Text = "Update";
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_Campany_CompInfo|dgbank_SelectedIndexChanged|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmBank1.apx");
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
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        //Validated();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[27];
            _arrPara[0] = new SqlParameter("@BankId", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@BankName", SqlDbType.NVarChar, 50);
            _arrPara[2] = new SqlParameter("@Street", SqlDbType.NVarChar, 250);
            _arrPara[3] = new SqlParameter("@City", SqlDbType.NVarChar, 50);
            _arrPara[4] = new SqlParameter("@State", SqlDbType.NVarChar, 50);
            _arrPara[5] = new SqlParameter("@Country", SqlDbType.NVarChar, 50);
            _arrPara[6] = new SqlParameter("@ACNo", SqlDbType.NVarChar, 50);
            _arrPara[7] = new SqlParameter("@SwiftCode", SqlDbType.NVarChar, 50);
            _arrPara[8] = new SqlParameter("@PhoneNo", SqlDbType.NVarChar, 50);
            _arrPara[9] = new SqlParameter("@FaxNo", SqlDbType.NVarChar, 50);
            _arrPara[10] = new SqlParameter("@Email", SqlDbType.NVarChar, 50);
            _arrPara[11] = new SqlParameter("@ContectPerson", SqlDbType.NVarChar, 50);
            _arrPara[12] = new SqlParameter("@ADICode", SqlDbType.NVarChar, 50);
            _arrPara[13] = new SqlParameter("@varuserid", SqlDbType.Int);
            _arrPara[14] = new SqlParameter("@companyid", SqlDbType.Int);
            _arrPara[15] = new SqlParameter("@Iban", SqlDbType.VarChar, 50);
            _arrPara[16] = new SqlParameter("@Bic", SqlDbType.VarChar, 50);
            _arrPara[17] = new SqlParameter("@Ifscode", SqlDbType.VarChar, 50);
            _arrPara[18] = new SqlParameter("@Adcode", SqlDbType.VarChar, 50);
            _arrPara[19] = new SqlParameter("@Branchcode", SqlDbType.VarChar, 50);
            _arrPara[20] = new SqlParameter("@Micrcode", SqlDbType.VarChar, 50);
            _arrPara[21] = new SqlParameter("@CurrencyId", SqlDbType.VarChar, 50);
            _arrPara[22] = new SqlParameter("@BankCategoryId", SqlDbType.NVarChar, 20);
            _arrPara[23] = new SqlParameter("@AccountType", SqlDbType.Int);
            _arrPara[24] = new SqlParameter("@Msgflag", SqlDbType.VarChar,30);
            _arrPara[25] = new SqlParameter("@AccountName", SqlDbType.VarChar, 100);
            _arrPara[26] = new SqlParameter("@Postcode", SqlDbType.VarChar, 20);


            _arrPara[0].Value = Convert.ToInt32(ViewState["BankId"]);
            _arrPara[1].Value = txtBankName.Text.ToUpper();
            _arrPara[2].Value = txtAddress.Text.ToUpper();
            _arrPara[3].Value = txtCity.Text.ToUpper();
            _arrPara[4].Value = txtState.Text.ToUpper();
            _arrPara[5].Value = txtCountry.Text.ToUpper();
            _arrPara[6].Value = txtAcc.Text.ToUpper();
            _arrPara[7].Value = txtCode.Text.ToUpper();
            _arrPara[8].Value = txtPhn.Text.ToUpper();
            _arrPara[9].Value = txtFaxNo.Text.ToUpper();
            _arrPara[10].Value = txtEmail.Text.ToUpper();
            _arrPara[11].Value = txtMisc.Text.ToUpper();
            _arrPara[12].Value = TxtAdCode.Text.ToUpper();
            _arrPara[13].Value = Session["varuserid"].ToString();
            _arrPara[14].Value = Session["varCompanyId"].ToString();
            _arrPara[15].Value = txtiban.Text.ToUpper();
            _arrPara[16].Value = txtbic.Text.ToUpper();
            _arrPara[17].Value = txtifscode.Text.ToUpper();
            _arrPara[18].Value = txtacode.Text.ToUpper();
            _arrPara[19].Value = txtbranchcode.Text.ToUpper();
            _arrPara[20].Value = txtmicrcode.Text.ToUpper();
            _arrPara[21].Value = DDcurrency.SelectedIndex <= 0 ? "0" : DDcurrency.SelectedValue;
            //Check Categories
            String str = "";
            for (int i = 0; i < chkBankCategory.Items.Count; i++)
            {
                if (chkBankCategory.Items[i].Selected)
                {
                    if (str == "")
                    {
                        str = chkBankCategory.Items[i].Value;
                    }
                    else
                    {
                        str = str + "," + chkBankCategory.Items[i].Value;
                    }
                }
            }
            _arrPara[22].Value = str;
            _arrPara[23].Value = DDActype.SelectedValue;
            _arrPara[24].Direction = ParameterDirection.Output;
            _arrPara[25].Value = txtaccountname.Text;
            _arrPara[26].Value = txtPostcode.Text;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_BANk", _arrPara);
            lbblerr.Visible = true;
            lbblerr.Text = _arrPara[24].Value.ToString();
            Tran.Commit();
            Clear_After_Save();
        }
        catch (Exception ex)
        {
            lbblerr.Visible = true;
            lbblerr.Text = ex.Message;
            Tran.Rollback();
            Logs.WriteErrorLog("Masters_Campany_frmBank|BtnSave_Click|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmBank1.apx");
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
            if (Request.QueryString["id"] != null)
            {
                if (Request.QueryString["id"] == "1")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "onload", "onSuccess();", true);
                }
            }
        }
    }
    private void Clear_After_Save()
    {
        Fill_Grid();
        ViewState["BankId"] = "0";
        BtnSave.Text = "Save";
        txtBankName.Text = "";
        txtAddress.Text = "";
        txtCity.Text = "";
        txtPhn.Text = "";
        txtAcc.Text = "";
        TxtAdCode.Text = "";
        txtState.Text = "";
        txtFaxNo.Text = "";
        txtCode.Text = "";
        txtCountry.Text = "";
        txtEmail.Text = "";
        txtMisc.Text = "";
        txtiban.Text = "";
        txtbic.Text = "";
        txtifscode.Text = "";
        txtacode.Text = "";
        txtbranchcode.Text = "";
        txtmicrcode.Text = "";
        txtaccountname.Text = "";
        txtPostcode.Text = "";
        DDcurrency.SelectedIndex = 0;
        UtilityModule.ConditonalChkBoxListFill(ref chkBankCategory, "select CategoryId,CategoryName from Bankcategory");
        //SqlhelperEnum.FillCheckBoxlist(AllEnums.MasterTables.BankCategory, chkBankCategory, pID: "CategoryId", pName: "CategoryName");

    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        string qry = "SELECT BankId,BankName,City,State,Country,ACNo,SwiftCode,PhoneNo FROM   Bank";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\BankReportNew.rpt";
            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\BankReportNew.xsd";
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
    private void Validated()
    {
        try
        {
            string strsql;
            if (BtnSave.Text == "Update")
            {
                strsql = "select BankName from bank where BankId!='" + ViewState["BankId"].ToString() + "' and  BankName='" + txtBankName.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                strsql = "select BankName from bank where BankName='" + txtBankName.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
            }
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lbblerr.Visible = true;
                lbblerr.Text = "Bank already exists............";
                txtBankName.Text = "";
                txtBankName.Focus();
            }
            else
            {
                lbblerr.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmBank1.apx");
        }
    }
   protected void dgBank_RowCreated(object sender, GridViewRowEventArgs e)
   {
        //Add CSS class on header row.
      // if (e.Row.RowType == DataControlRowType.Header)
           // e.Row.CssClass = "header";
        //Add CSS class on normal row.
      if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        //Add CSS class on alternate row.
      // if (e.Row.RowType == DataControlRowType.DataRow &&
                 // e.Row.RowState == DataControlRowState.Alternate)
           // e.Row.CssClass = "alternate";
    }
    protected void dgBank_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
            _array[0] = new SqlParameter("@BankId", dgBank.DataKeys[e.RowIndex].Value);
            _array[1] = new SqlParameter("@Message", SqlDbType.NVarChar, 100);
            _array[2] = new SqlParameter("@VarUserId", Session["varuserid"].ToString());
            _array[3] = new SqlParameter("@VarCompanyId", Session["varCompanyId"].ToString());
            _array[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteBank", _array);
            Tran.Commit();
            Clear_After_Save();
            lbblerr.Visible = true;
            lbblerr.Text = _array[1].Value.ToString();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmBank1.apx");
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