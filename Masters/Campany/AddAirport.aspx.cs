using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Masters_Campany_AddAirport : System.Web.UI.Page
{
    public static int AirPortId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            int Countryid = Convert.ToInt16(Request.QueryString["a"]);
            UtilityModule.ConditionalComboFill(ref ddcountry, "select CountryId,CountryName from CountryMaster Order by CountryName", true, "--Select--");
            if (ddcountry.Items.Count > 0)
            {
                ddcountry.SelectedValue = Countryid.ToString();
            }
            fill_grid();
        }

    }
    protected void fill_grid()
    {
        string strsql = "select AirPortId,AirPortName,CountryName from Airport ap inner join Countrymaster cm on cm.countryid=ap.countryid";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        DGAirport.DataSource = null;
        DGAirport.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGAirport.DataSource = ds.Tables[0];
            DGAirport.DataBind();
        }


    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
         Validate();
        if (ddcountry.Text != "" && txtAirport.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[6];
                _arrPara[0] = new SqlParameter("@AirPortId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@AirPortName", SqlDbType.VarChar, 50);
                _arrPara[2] = new SqlParameter("@CountryId", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@userid", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 250);


                _arrPara[0].Direction = ParameterDirection.InputOutput;
                _arrPara[0].Value = AirPortId;
                _arrPara[1].Value = txtAirport.Text;
                _arrPara[2].Value = ddcountry.SelectedValue;
                _arrPara[3].Value = Session["varuserid"].ToString();
                _arrPara[4].Value = Session["varMasterCompanyIDForERP"].ToString();
                _arrPara[5].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_Airport", _arrPara);
                Tran.Commit();
                lblerr.Text = _arrPara[5].Value.ToString();
                AirPortId = 0;
                txtAirport.Text = "";
                ddcountry.Enabled = true;
            }
            catch (Exception ex)
            {
                lblerr.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
            fill_grid();
        }
    }
    private void Validated()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql;
            if (btnsave.Text == "Update")
            {
                strsql = "select AirPortName from Airport where AirPortName='" + ViewState["id"].ToString() + "' and AirPortName='" + txtAirport.Text + "' And  masterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            else
            {
                strsql = "select *  from Airport where CountryName='" + txtAirport.Text + "' And masterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblerr.Visible = true;
                lblerr.Text = "AirPortName already exists............";
                txtAirport.Text = "";
                ddcountry.Focus();
            }
            else
            {
                lblerr.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddAirport.aspx");
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
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select AirPortId from customerinfo where AirPortId=" + AirPortId + ""));
            if (id <= 0)
            {
                SqlHelper.ExecuteScalar(Tran, CommandType.Text, "delete  from Airport where AirPortId=" + AirPortId + "");
                DataSet dt = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(Tran, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'Airport'," + AirPortId + ",getdate(),'Delete')");
                btnsave.Text = "Save";
                txtAirport.Text = "";
                lblerr.Visible = true;
                lblerr.Text = "Value Deleted..............";

            }
            else
            {
                lblerr.Visible = true;
                lblerr.Text = "Value in Use...";
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblerr.Visible = true;
            lblerr.Text = ex.Message;
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddAirport.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        fill_grid();
        btndelete.Visible = false;
    }
    protected void DGAirport_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        //if (e.Row.RowType == DataControlRowType.Header)
           // e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
       // if (e.Row.RowType == DataControlRowType.DataRow &&
                 // e.Row.RowState == DataControlRowState.Alternate)
            //e.Row.CssClass = "alternate";
    }
    protected void DGAirport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGAirport, "select$" + e.Row.RowIndex);
        }

    }
    protected void DGAirport_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddcountry.Enabled = false;
        AirPortId = (int)DGAirport.SelectedDataKey.Value;

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select AirPortName,CountryId from Airport where AirPortId=" + AirPortId);
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtAirport.Text = ds.Tables[0].Rows[0]["AirPortName"].ToString();
                ddcountry.SelectedValue = ds.Tables[0].Rows[0]["CountryId"].ToString();

            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddAirport.aspx");
            Logs.WriteErrorLog("Masters_Campany_AddAirport|Fill_Grid_Data|" + ex.Message);
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
}