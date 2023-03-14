using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Masters_Campany_countrymaster : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtid.Text = "0";
            Fill_Grid();    
        }
        lblerr.Visible = false;
    }
    private void Fill_Grid()
    {
        gdCountry.DataSource = Fill_Grid_Data();
        gdCountry.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "select CountryId,CountryName,CountryCode from CountryMaster where masterCompanyId=" + Session["varCompanyId"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/CountryMaster.aspx");
            Logs.WriteErrorLog("Masters_Campany_CountryMaster|Fill_Grid_Data|" + ex.Message);
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
    protected void gdCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
       string id = gdCountry.SelectedDataKey.Value.ToString();
        //Session["id"] = id;
        ViewState["id"] = id;
        txtid.Text = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from CountryMaster where CountryId=" + id);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["CountryId"].ToString();
                txtCountry.Text = ds.Tables[0].Rows[0]["Countryname"].ToString();
                txtCountryCode.Text = ds.Tables[0].Rows[0]["CountryCode"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/CountryMaster.aspx");
            Logs.WriteErrorLog("Masters_Campany_Countrymaster|Fill_Grid_Data|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        btnsave.Text = "Update";
        btndelete.Visible = true;
    }
    protected void gdCountry_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdCountry, "select$" + e.Row.RowIndex);
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Validated();
        if (txtCountry.Text != "" && txtCountryCode.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[5];
                _arrPara[0] = new SqlParameter("@CountryId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@CountryName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@CountryCode", SqlDbType.NVarChar, 50);
                _arrPara[3] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@CompanyId", SqlDbType.Int);

                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                _arrPara[1].Value = txtCountry.Text.ToUpper();
                _arrPara[2].Value = txtCountryCode.Text.ToUpper();
                _arrPara[3].Value = Session["varuserid"].ToString();
                _arrPara[4].Value = Session["varCompanyId"].ToString();
                con.Open();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_Country", _arrPara);
                txtCountry.Text = "";
                txtCountryCode.Text = "";
            }
            catch (Exception ex)
            {
                lblerr.Text = ex.Message;
                Logs.WriteErrorLog("Masters_Campany_CountryMaster|cmdSave_Click|" + ex.Message);
                ClientScript.RegisterStartupScript(this.GetType(), "onload", "onError();", true);
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/CountryMaster.aspx");
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
            lblerr.Visible = true;
            lblerr.Text = "Save Details............";
        }
        else
            {
                if (lblerr.Text == "CountryName already exists............")
                {
                    lblerr.Visible = true;
                    lblerr.Text = "CountryName already exists............";
                }
                else
                {
                    lblerr.Visible = true;
                    lblerr.Text = "Please Fill Details............";
                }
            }
        btnsave.Text = "Save";
        btndelete.Visible = false;
        Fill_Grid();
        }
    protected void btnclose_click(object sender, EventArgs e)
    {
        
    }
    protected void rpt_Click(object sender, EventArgs e)
    {

    }
    private void Validated()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql;
            if (btnsave.Text == "Update")
            {
                strsql = "select CountryName from CountryMaster where CountryId!='" + ViewState["id"].ToString() + "' and CountryName='" + txtCountry.Text + "' And  masterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                strsql = "select CountryName from CountryMaster where CountryName='" + txtCountry.Text + "' And masterCompanyId=" + Session["varCompanyId"];
            }
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblerr.Visible = true;
                lblerr.Text = "CountryName already exists............";
                txtCountry.Text = "";
                txtCountry.Focus();
            }
            else
            {
                lblerr.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/CountryMaster.aspx");
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
        con.Open();
        try
        {
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Country from customerinfo where Country=" + ViewState["id"].ToString()));
            if (id <= 0)
            {
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from CountryMaster where CountryId=" + ViewState["id"].ToString());
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'CountryMaster'," + ViewState["id"].ToString() + ",getdate(),'Delete')");
                btnsave.Text = "Save";
                lblerr.Visible = true;
                lblerr.Text = "Value Deleted..............";
            }
            else
            {
                lblerr.Visible = true;
                lblerr.Text = "Value in Use...";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/CountryMaster.aspx");
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
        btnsave.Text = "Save";
        txtCountry.Text = "";
        txtCountryCode.Text = "";
    }
    protected void gdCountry_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
       // if (e.Row.RowType == DataControlRowType.Header)
          //  e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
      //  if (e.Row.RowType == DataControlRowType.DataRow &&
                 // e.Row.RowState == DataControlRowState.Alternate)
           // e.Row.CssClass = "alternate";
    }
}