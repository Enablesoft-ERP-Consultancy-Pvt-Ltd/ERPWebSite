using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Masters_Campany_Currencies : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtid.Text = "0";
            Fill_Grid();
            //int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
            if (Session["varCompanyNo"].ToString() == "3")
            {
                trPAYINSTRUCTION.Visible = true;
                trBENEFICIARYBANK.Visible = true;
                trRemarks.Visible = true;
            }
        }
        lblerr.Visible = false;
    }
    private void Fill_Grid()
    {
        dgcurrency.DataSource = Fill_Grid_data();
        dgcurrency.DataBind();
    }
    private DataSet Fill_Grid_data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string strsql = "Select CurrencyId,CurrencyName,CurrencyTypeRs,CurrencyTypePs,CurrentRateRefRs from  CurrencyInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            ds.Tables[0].Columns["CurrencyId"].ColumnName = "Sr.No";
            ds.Tables[0].Columns["CurrencyName"].ColumnName = "Curency Name";
            ds.Tables[0].Columns["CurrencyTypeRs"].ColumnName = "Rupees(Rs.)";
            ds.Tables[0].Columns["CurrencyTypePs"].ColumnName = "Paise(p)";
            ds.Tables[0].Columns["CurrentRateRefRs"].ColumnName = "Conversin Rate";
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Currency|Fill_Grid_Data|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCustomer.aspx");
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
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        Validated();
        if (TxtCurrencyName.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrpara = new SqlParameter[10];
                _arrpara[0] = new SqlParameter("@CurrencyId", SqlDbType.Int, 50);
                _arrpara[1] = new SqlParameter("@CurrencyName", SqlDbType.NVarChar, 50);
                _arrpara[2] = new SqlParameter("@CurrencyTypeRs", SqlDbType.NVarChar, 50);
                _arrpara[3] = new SqlParameter("@CurrencyTypePS", SqlDbType.NVarChar, 50);
                _arrpara[4] = new SqlParameter("@CurrentRateRefRs", SqlDbType.Float);
                _arrpara[5] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrpara[6] = new SqlParameter("@companyid", SqlDbType.Int);
                _arrpara[7] = new SqlParameter("@PAYINSTRUCTION", SqlDbType.NVarChar, 200);
                _arrpara[8] = new SqlParameter("@BENEFICIARY_BANK", SqlDbType.NVarChar, 200);
                _arrpara[9] = new SqlParameter("@REMARKS", SqlDbType.NVarChar, 200);

                _arrpara[0].Value = Convert.ToInt32(txtid.Text);
                _arrpara[1].Value = TxtCurrencyName.Text.ToUpper();
                _arrpara[2].Value = txtrupees.Text.ToUpper();
                _arrpara[3].Value = txtpaise.Text.ToUpper();
                _arrpara[4].Value = Convert.ToDouble(txtConversionRateAsPerRs.Text);
                _arrpara[5].Value = Session["varuserid"].ToString();
                _arrpara[6].Value = Session["varMasterCompanyIDForERP"].ToString();

                _arrpara[7].Value = trPAYINSTRUCTION.Visible == true ? TXTPAYMENTINSTRUCTION.Text : "";
                _arrpara[8].Value = trBENEFICIARYBANK.Visible == true ? TXTBENEFICIARYBANK.Text : "";
                _arrpara[9].Value = trRemarks.Visible == true ? TxtRemarks.Text : "";
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_CURRENCYINFO", _arrpara);
                lblerr.Visible = true;
                lblerr.Text = "Data Saved Successfully";
                BtnSave.Text = "Save";
                Tran.Commit();
                Fill_Grid();
                txtid.Text = "0";
                TxtCurrencyName.Text = "";
                txtrupees.Text = "";
                txtpaise.Text = "";
                txtConversionRateAsPerRs.Text = "";
                TXTPAYMENTINSTRUCTION.Text = "";
                TXTBENEFICIARYBANK.Text = "";
                TxtRemarks.Text = "";
                //AllEnums.MasterTables.Currencyinfo.RefreshTable();
            }
            catch (Exception ex)
            {
                lblerr.Visible = true;
                lblerr.Text = ex.Message; ;
                Tran.Rollback();
                Logs.WriteErrorLog("Currency|BtnSave_click|" + ex.Message);
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCustomer.aspx");
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
        }
        else
        {
            if (lblerr.Text == "Currency already exists............")
            {
                lblerr.Visible = true;
                lblerr.Text = "Currency already exists............";
            }
            else
            {
                lblerr.Visible = true;
                lblerr.Text = "Please fill details............";
            }
        }
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {

    }
    protected void dgcurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        BtnSave.Text = "Update";
        txtid.Text = dgcurrency.SelectedRow.Cells[0].Text;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select CurrencyId,currencyName,currencytypeRs,currencytypePs,currentRateRefRs,PAYINSTRUCTION,BENEFICIARY_BANK,REMARKS from CurrencyInfo where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And CurrencyId=" + dgcurrency.SelectedRow.Cells[0].Text);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            //ViewState["CurrencyId"] = txtid.Text;
            ViewState["CurrencyId"] = txtid.Text;
            TxtCurrencyName.Text = Ds.Tables[0].Rows[0]["currencyName"].ToString();
            txtrupees.Text = Ds.Tables[0].Rows[0]["currencytypeRs"].ToString();
            txtpaise.Text = Ds.Tables[0].Rows[0]["currencytypePs"].ToString();
            txtConversionRateAsPerRs.Text = Ds.Tables[0].Rows[0]["currentRateRefRs"].ToString();
            TXTPAYMENTINSTRUCTION.Text = Ds.Tables[0].Rows[0]["PAYINSTRUCTION"].ToString();
            TXTBENEFICIARYBANK.Text = Ds.Tables[0].Rows[0]["BENEFICIARY_BANK"].ToString();
            TxtRemarks.Text = Ds.Tables[0].Rows[0]["REMARKS"].ToString();
            btndelete.Visible = true;
        }

    }
    protected void dgcurrency_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgcurrency, "select$" + e.Row.RowIndex);
        }
    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        BtnSave.Text = "Save";
        txtid.Text = "0";
        TxtCurrencyName.Text = "";
        txtrupees.Text = "";
        txtpaise.Text = "";
        txtConversionRateAsPerRs.Text = "";
        TxtCurrencyName.Focus();
        lblerr.Text = "";
        TXTPAYMENTINSTRUCTION.Text = "";
        TXTBENEFICIARYBANK.Text = "";
        TxtRemarks.Text = "";
        btndelete.Visible = false;
    }
    private void Validated()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql;
            if (BtnSave.Text == "Update")
            {
                strsql = "Select isnull(max(CurrencyId),0) from CurrencyInfo where CurrencyId !=" + dgcurrency.SelectedValue + " and CurrencyName='" + TxtCurrencyName.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            else
            {
                strsql = "Select isnull(max(CurrencyId),0) from CurrencyInfo where CurrencyName='" + TxtCurrencyName.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            con.Open();
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, strsql));
            if (id > 0)
            {
                lblerr.Text = "Currency already exists............";
                TxtCurrencyName.Text = "";
                TxtCurrencyName.Focus();
            }
            else
            {
                lblerr.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCustomer.aspx");
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
    protected void btndelete_Click(object sender, EventArgs e)
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
            _array[0] = new SqlParameter("@CurrencyId", ViewState["CurrencyId"].ToString());
            _array[1] = new SqlParameter("@Message", SqlDbType.NVarChar, 100);
            _array[2] = new SqlParameter("@VarUserId", Session["varuserid"].ToString());
            _array[3] = new SqlParameter("@VarCompanyId", Session["varMasterCompanyIDForERP"].ToString());
            _array[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteCurrency", _array);
            lblerr.Visible = true;
            lblerr.Text = _array[1].Value.ToString();
            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblerr.Visible = true;
            lblerr.Text = ex.Message;
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddCustomer.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        Fill_Grid();
        btndelete.Visible = false;
        BtnSave.Text = "Save";
        txtid.Text = "0";
        TxtCurrencyName.Text = "";
        txtrupees.Text = "";
        txtpaise.Text = "";
        txtConversionRateAsPerRs.Text = "";
        TXTPAYMENTINSTRUCTION.Text = "";
        TXTBENEFICIARYBANK.Text = "";
        TxtRemarks.Text = "";
        TxtCurrencyName.Focus();
    }

}