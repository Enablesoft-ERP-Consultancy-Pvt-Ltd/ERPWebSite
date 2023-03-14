using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Carpet_FinishType : System.Web.UI.Page
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
            fill_grid();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        //if (btnsave.Text != "Update")
        //{
            CheckDuplicateDate();
       // }
        if(txtfinishtype.Text!="" && txtshortnsme.Text!="" )
        {
            lblerror.Visible=false;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[5];
            _arrPara[0] = new SqlParameter("@txtid", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@finishtype", SqlDbType.NVarChar);
            _arrPara[2] = new SqlParameter("@shortname", SqlDbType.NVarChar);
            _arrPara[3] = new SqlParameter("@varuserid", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@varCompanyId", SqlDbType.Int);
            _arrPara[0].Value = Convert.ToInt32(txtid.Text);
            _arrPara[1].Value = txtfinishtype.Text.ToUpper();
            _arrPara[2].Value =txtshortnsme.Text.ToUpper();
            _arrPara[3].Value = Session["varuserid"].ToString();
            _arrPara[4].Value = Session["varCompanyId"].ToString();

            con.Open();
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_finishtype", _arrPara);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FinishType.aspx");
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
        else
        {
        lblerror.Visible=true;
        }
       
       
        btnsave.Text = "Save";
        txtfinishtype.Text = "";
        txtshortnsme.Text = "";
        fill_grid();
    }


    private void CheckDuplicateDate()
    {
        DataSet ds = null;
        string strsql;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (btnsave.Text == "Update")
        {
             strsql = @"Select * from FINISHED_TYPE Where ( FINISHED_TYPE_NAME='" + txtfinishtype.Text + "' or SHORT_NAME ='" + txtshortnsme.Text + "' ) AND id <>" + Convert.ToInt32(txtid.Text);
        }
        else
        {
            strsql = @"Select * from FINISHED_TYPE Where  FINISHED_TYPE_NAME='" + txtfinishtype.Text + "' or SHORT_NAME ='" + txtshortnsme.Text+"'";
        }
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblMessage.Visible = true;
            lblMessage.Text = " Name AlReady Exits";
           
                txtfinishtype.Text = "";
                txtshortnsme.Text = "";
           
        }
        else
        {
            lblMessage.Visible = false;
        }
    }
    protected void gvfinishtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from FINISHED_TYPE WHERE Id=" + gvfinishtype.SelectedValue);
            txtid.Text = ds.Tables[0].Rows[0]["id"].ToString();
            txtfinishtype.Text = ds.Tables[0].Rows[0]["FINISHED_TYPE_NAME"].ToString();
            txtshortnsme.Text = ds.Tables[0].Rows[0]["SHORT_NAME"].ToString();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FinishType.aspx");
        }
        finally
        {
            
        }
        btnsave.Text = "Update";
        btndelete.Visible = true;
    }

    private void fill_grid()
    {
        gvfinishtype.DataSource = Fill_Grid_Data();
        gvfinishtype.DataBind();
    }

    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"SELECT * FROM FINISHED_TYPE ORDER BY ID DESC";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FinishType.aspx");
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
    protected void btndelete_Click(object sender, EventArgs e)
    {

    }
    protected void gvfinishtype_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvfinishtype, "Select$" + e.Row.RowIndex);
        }
    }
    protected void btnnew_Click(object sender, EventArgs e)
    {
        btnsave.Text = "Save";
        txtid.Text = "0";
        txtfinishtype.Text = "";
        txtshortnsme.Text = "";


    }
    protected void gvfinishtype_RowCreated(object sender, GridViewRowEventArgs e)
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