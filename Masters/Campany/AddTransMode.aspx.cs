using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_Default : System.Web.UI.Page
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
        lbl.Visible = false;
    }
    private void Fill_Grid()
    {
        gvtransmode.DataSource = Fill_Grid_Data();
        gvtransmode.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "select transmodeId as Sr_No,TransmodeName from TransMode Where MasterCompanyId=" + Session["varCompanyId"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_Campany_Default|Fill_Grid_Data|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddTransMode.aspx");
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
    protected void btnsave_Click(object sender, EventArgs e)
    {
         Validated();
        if (txtname.Text != "")
        {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
         try
        {
            SqlParameter[] _arrPara = new SqlParameter[4];
            _arrPara[0] = new SqlParameter("@transmodeId", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@transmodename", SqlDbType.NVarChar, 50);
            _arrPara[2] = new SqlParameter("@varCompanyId", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@varuserid", SqlDbType.Int);
            if (btnsave.Text == "Update")
            {
                _arrPara[0].Value =ViewState["id"];
            }
            else
            {
                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
            }
            _arrPara[1].Value = txtname.Text.ToUpper();
            _arrPara[2].Value = Session["varCompanyId"].ToString();
            _arrPara[3].Value = Session["varuserid"].ToString();
            con.Open();
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_Transmode", _arrPara);
           txtid.Text = "0";
            txtname.Text = "";
       }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_Campany_Default|btnSave_Click|" + ex.Message);
            ClientScript.RegisterStartupScript(this.GetType(), "onload", "onError();", true);
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddTransMode.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
         Fill_Grid();
         lbl.Visible = true;
         lbl.Text = "Save Details.......";
        }
        else
        {
        }
        btnsave.Text = "Save";
        btndelete.Visible = false;
    }
    protected void gvtransmode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gvtransmode.SelectedDataKey.Value.ToString();
        //Session["id"] = id;
        ViewState["id"] = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from TransMode where transmodeId=" + id);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["transmodeId"].ToString();
                txtname.Text = ds.Tables[0].Rows[0]["transmodename"].ToString();
             }
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_Campany_Default|Fill_Grid_Data|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddTransMode.aspx");
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
    protected void gvtransmode_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvtransmode, "select$" + e.Row.RowIndex);
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
                    strsql = "select transmodename from TransMode where transmodeId!='" + ViewState["id"].ToString() + "' and  transmodename='" + txtname.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
                }
                else
                {
                    strsql = "select transmodename from TransMode where transmodename='" + txtname.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
                }
                con.Open();
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lbl.Visible = true;
                    lbl.Text = "Agent already exists............";
                    txtname.Text = "";
                    txtname.Focus();
                }
                else
                {
                    //  Label1.Text = "";
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddTransMode.aspx");
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
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select ByAirSea from customerinfo where  MasterCompanyId=" + Session["varCompanyId"] + " And ByAirSea=" + ViewState["id"].ToString()));
            if (id <= 0)
            {
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from TransMode where transmodeId=" + ViewState["id"].ToString());
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'TransMode'," + ViewState["id"].ToString() + ",getdate(),'Delete')");
                lbl.Visible = true;
                lbl.Text = "Deleted Value............";
            }
            else
            {
                lbl.Visible = true;
                lbl.Text = "Value in Use...";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddTransMode.aspx");
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
        txtname.Text = "";
        txtid.Text = "0";
    }
    protected void gvtransmode_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        //if (e.Row.RowType == DataControlRowType.Header)
         //   e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
       // if (e.Row.RowType == DataControlRowType.DataRow &&
                //  e.Row.RowState == DataControlRowState.Alternate)
           // e.Row.CssClass = "alternate";
    }
}
    
