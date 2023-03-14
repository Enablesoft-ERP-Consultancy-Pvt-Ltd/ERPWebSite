using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Masters_AddSeaportAirport : System.Web.UI.Page
{
    public static int SeaPortId = 0;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
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
        string strsql = "select SeaPortId,SeaPortName,CountryName from seaport sp inner join Countrymaster cm on cm.countryid=sp.countryid";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        DGSeaport.DataSource = null;
        DGSeaport.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGSeaport.DataSource = ds.Tables[0];
            DGSeaport.DataBind();
        }


    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Validate();
        if (ddcountry.Text != "" && txtseaport.Text != "")
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
                _arrPara[0] = new SqlParameter("@SeaPortId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@SeaPortName", SqlDbType.VarChar, 50);
                _arrPara[2] = new SqlParameter("@CountryId", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@userid", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 250);


                _arrPara[0].Direction = ParameterDirection.InputOutput;
                _arrPara[0].Value = SeaPortId;
                _arrPara[1].Value = txtseaport.Text;
                _arrPara[2].Value = ddcountry.SelectedValue;
                _arrPara[3].Value = Session["varuserid"].ToString();
                _arrPara[4].Value = Session["varCompanyId"].ToString();
                _arrPara[5].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_seaport", _arrPara);
                Tran.Commit();
                lblerr.Text = _arrPara[5].Value.ToString();
                SeaPortId = 0;
                txtseaport.Text = "";
                ddcountry.Enabled = true;
                fill_grid();
                
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
                strsql = "select SeaPortName from seaport where SeaPortName='" + ViewState["id"].ToString() + "' and SeaPortName='" + txtseaport.Text + "' And  masterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                strsql = "select *  from seaport where CountryName='" + txtseaport.Text + "' And masterCompanyId=" + Session["varCompanyId"];
            }
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblerr.Visible = true;
                lblerr.Text = "SeaPortName already exists............";
                txtseaport.Text = "";
                ddcountry.Focus();
            }
            else
            {
                lblerr.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddSeaport.aspx");
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
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select SeaPortId from customerinfo where SeaPortId=" + SeaPortId + ""));
            if (id <= 0)
            {
                SqlHelper.ExecuteScalar(Tran, CommandType.Text, "delete  from seaport where SeaPortId=" + SeaPortId + "");
                DataSet dt = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(Tran, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'seaport'," + SeaPortId + ",getdate(),'Delete')");
                btnsave.Text = "Save";
                txtseaport.Text = "";
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
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddSeaport.aspx");
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
    protected void DGSeaport_RowCreated(object sender, GridViewRowEventArgs e)
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
           // e.Row.CssClass = "alternate";
        
    }
    protected void DGSeaport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGSeaport, "select$" + e.Row.RowIndex);
        }

    }

    protected void DGSeaport_SelectedIndexChanged(object sender, EventArgs e)
    {
        
            ddcountry.Enabled = false;
            SeaPortId = (int)DGSeaport.SelectedDataKey.Value;

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select SeaPortName,CountryId from seaport where SeaPortId=" + SeaPortId);
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtseaport.Text = ds.Tables[0].Rows[0]["SeaPortName"].ToString();
                    ddcountry.SelectedValue = ds.Tables[0].Rows[0]["CountryId"].ToString();

                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddSeaport.aspx");
                Logs.WriteErrorLog("Masters_Campany_AddSeaport|Fill_Grid_Data|" + ex.Message);
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