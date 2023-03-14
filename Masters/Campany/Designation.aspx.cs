using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Designation : CustomPage
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
        Label2.Visible = false;
    }
    private void Fill_Grid()
    {
        gdDesignation.DataSource = Fill_Grid_Data();
        gdDesignation.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        try
        {
            string str = "select DesignationId SrNo,DesignationName,Description from Designation Where MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(str);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/Designation.aspx");
        }
        return ds;
    }
    protected void gdDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtid.Text = gdDesignation.SelectedRow.Cells[0].Text;
        //Session["id"] = txtid.Text;
        ViewState["id"] = txtid.Text;
        txtDesignationname.Text = gdDesignation.SelectedRow.Cells[1].Text.Replace("&nbsp;", "");
        txtDescription.Text = gdDesignation.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
        btnsave.Text = "Update";
        btndelete.Visible = true;
    }
    protected void gdDesignation_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdDesignation, "select$" + e.Row.RowIndex);
        }
    }
    protected void gdDesignation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdDesignation.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Validated();
        if (txtDesignationname.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrpara = new SqlParameter[5];
                _arrpara[0] = new SqlParameter("@DesignationId", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@DesignationName", SqlDbType.NVarChar, 50);
                _arrpara[2] = new SqlParameter("@Description", SqlDbType.NVarChar, 50);
                _arrpara[3] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrpara[4] = new SqlParameter("@CompanyId", SqlDbType.Int);
                if (btnsave.Text == "Update")
                {
                    _arrpara[0].Value = ViewState["id"];
                }
                else
                {
                    _arrpara[0].Value = Convert.ToInt32(txtid.Text);
                }
                _arrpara[1].Value = txtDesignationname.Text.ToUpper();
                _arrpara[2].Value = txtDescription.Text.ToUpper();
                _arrpara[3].Value = Session["varuserid"].ToString();
                _arrpara[4].Value = Session["varCompanyId"].ToString();
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DESIGNATION", _arrpara);
                Tran.Commit();
                Label2.Visible = true;
                Label2.Text = "Save Details............";
                Fill_Grid();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                Label2.Visible = true;
                Label2.Text = ex.Message;
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/Designation.aspx");
            }
            finally
            {
                con.Close();
            }
            txtid.Text = "0";
            txtDesignationname.Text = "";
            txtDescription.Text = "";
            btnsave.Text = "Save";
            btndelete.Visible = false;
        }
        else
        {
            if (Label2.Text == "Design already exists............")
            {
                Label2.Visible = true;
                Label2.Text = "Design already exists............";
            }
            else
            {
                Label2.Visible = true;
                Label2.Text = "Please Fill Details........";
            }
        }
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        txtid.Text = "0";
        txtDesignationname.Text = "";
        txtDescription.Text = "";
    }
    private void Validated()
    {
        try
        {
            string strsql;
            if (btnsave.Text == "Update")
            {
                strsql = "select DesignationName from Designation where DesignationId !='" + ViewState["id"].ToString() + "' and DesignationName='" + txtDesignationname.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                strsql = "Select DesignationName from Designation where DesignationName='" + txtDesignationname.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
            }
            DataSet ds = SqlHelper.ExecuteDataset(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Label2.Visible = true;
                Label2.Text = "Design already exists............";
                txtDesignationname.Text = "";
                txtDesignationname.Focus();
            }
            else
            {
                Label2.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/Designation.aspx");
        }
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlHelper.ExecuteScalar(con, CommandType.Text, "delete  from Designation where DesignationId=" + ViewState["id"].ToString());
            Label2.Visible = true;
            Label2.Text = "Value Deleted.........";
        }
        catch (Exception ex)
        {
            Label2.Visible = true;
            Label2.Text = ex.Message;
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/Designation.aspx");
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
        txtDescription.Text = "";
        txtDesignationname.Text = "";
        txtid.Text = "0";
    }
    protected void gdDesignation_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        //if (e.Row.RowType == DataControlRowType.Header)
            //e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        //if (e.Row.RowType == DataControlRowType.DataRow &&
                 // e.Row.RowState == DataControlRowState.Alternate)
           // e.Row.CssClass = "alternate";
    }
}