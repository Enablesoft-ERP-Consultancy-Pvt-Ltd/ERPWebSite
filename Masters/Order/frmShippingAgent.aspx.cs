using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Order_frmShippingAgent : CustomPage
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
        }
    }
    private void Fill_Grid()
    {
        gdShippingMaster.DataSource = Fill_Grid_Data();
        gdShippingMaster.DataBind();
    }

    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string str = "select AgentId,AgentName,Address,ContectPerson,PhoneNo,Mobile,Fax, Email from shipp Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Agentid";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/frmShippingAgent.aspx");
            //
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
    protected void gdShippingMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gdShippingMaster.SelectedDataKey.Value.ToString();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from Shipp where AgentId=" + id);
        try
        {
            txtid.Text = ds.Tables[0].Rows[0]["AgentId"].ToString();
            txtCompanyName.Text = ds.Tables[0].Rows[0]["AgentName"].ToString();
            txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
            txtContactPerson.Text = ds.Tables[0].Rows[0]["ContectPerson"].ToString();
            txtPhone.Text = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
            txtMobile.Text = ds.Tables[0].Rows[0]["Mobile"].ToString();
            txtFax.Text = ds.Tables[0].Rows[0]["Fax"].ToString();
            txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/frmShippingAgent.aspx");
            //
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
    protected void gdShippingMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdShippingMaster, "select$" + e.Row.RowIndex);
        }

    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[8];
            _arrpara[0] = new SqlParameter("@Agentid", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@AgentName", SqlDbType.NVarChar, 50);
            _arrpara[2] = new SqlParameter("@Address", SqlDbType.NVarChar, 50);
            _arrpara[3] = new SqlParameter("@ContectPerson", SqlDbType.NVarChar, 50);
            _arrpara[4] = new SqlParameter("@PhoneNo", SqlDbType.NVarChar, 50);
            _arrpara[5] = new SqlParameter("@Mobile", SqlDbType.NVarChar, 50);
            _arrpara[6] = new SqlParameter("@Fax", SqlDbType.NVarChar, 50);
            _arrpara[7] = new SqlParameter("@Email", SqlDbType.NVarChar, 50);

            _arrpara[0].Value = Convert.ToInt32(txtid.Text);
            _arrpara[1].Value = txtCompanyName.Text;
            _arrpara[2].Value = txtAddress.Text;
            _arrpara[3].Value = txtContactPerson.Text;
            _arrpara[4].Value = txtPhone.Text;
            _arrpara[5].Value = txtMobile.Text;
            _arrpara[6].Value = txtFax.Text;
            _arrpara[7].Value = txtEmail.Text;
            con.Open();
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_SHIPPING", _arrpara);
            Fill_Grid();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/frmShippingAgent.aspx");
            Label1.Text = ex.Message;
        }

        finally
        {
            con.Close();
        }
        txtid.Text = "0";
        txtCompanyName.Text = "";
        txtAddress.Text = "";
        txtContactPerson.Text = "";
        txtPhone.Text = "";
        txtMobile.Text = "";
        txtEmail.Text = "";
        txtFax.Text = "";
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        txtid.Text = "0";
        txtCompanyName.Text = "";
        txtAddress.Text = "";
        txtContactPerson.Text = "";
        txtPhone.Text = "";
        txtMobile.Text = "";
        txtEmail.Text = "";
        txtFax.Text = "";
    }
    protected void gdShippingMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdShippingMaster.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void gdShippingMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
}