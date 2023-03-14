using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Carpet_AddInspectionDate : System.Web.UI.Page
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
        LblError.Visible = false;
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        if (txtShape.Text != "")
        {
            CheckDuplicateData();
            if (LblError.Visible == false)
            {
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                try
                {
                    SqlParameter[] _arrPara = new SqlParameter[4];
                    _arrPara[0] = new SqlParameter("@Id", SqlDbType.Int);
                    _arrPara[1] = new SqlParameter("@DateName", SqlDbType.NVarChar, 50);
                    _arrPara[2] = new SqlParameter("@varuserid", SqlDbType.Int);
                    _arrPara[3] = new SqlParameter("@varCompanyId", SqlDbType.Int);

                    _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                    _arrPara[1].Value = txtShape.Text.ToUpper();
                    _arrPara[2].Value = Session["varuserid"].ToString();
                    _arrPara[3].Value = Session["varCompanyId"].ToString();
                    con.Open();
                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_InspectionDateName", _arrPara);
                    txtid.Text = "0";
                    txtShape.Text = "";
                    LblError.Visible = true;
                    LblError.Text = "Save Details.....";
                }
                catch (Exception ex)
                {
                    UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddShape.aspx");
                    Logs.WriteErrorLog("Masters_Campany_Shape|cmdSave_Click|" + ex.Message);
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
                Fill_Grid();
                btndelete.Visible = false;
                BtnSave.Text = "Save";
            }
        }
        else
        {
            LblError.Visible = true;
            LblError.Text = "Fill Details........";
        }
    }
    private void CheckDuplicateData()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string strsql = @"Select * from InspectionDateMaster Where DateName='" + txtShape.Text + "' and Id !=" + txtid.Text;
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            LblError.Visible = true;
            LblError.Text = "Shape AlReady Exits........";
            txtShape.Text = "";
            txtShape.Focus();
        }
        else
        {
            LblError.Visible = false;
        }
    }
    private void Fill_Grid()
    {
        gvShape.DataSource = Fill_Grid_Data();
        gvShape.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "select Id as Sr_No,DateName as datename from InspectionDateMaster order by Id";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddShape.aspx");
            Logs.WriteErrorLog("Masters_Campany_Shape|Fill_Grid_Data|" + ex.Message);
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
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from InspectionDateMaster where Id=" + Session["id"].ToString());
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'Shape'," + Session["id"].ToString() + ",getdate(),'Delete')");
                LblError.Visible = true;
                LblError.Text = "Value Deleted...";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddShape.aspx");
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
        txtShape.Text = "";
        txtid.Text = "0";
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        txtid.Text = "0";
        txtShape.Text = "";
        BtnSave.Text = "Save";
    }
    protected void gvShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gvShape.SelectedDataKey.Value.ToString();
        Session["id"] = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from InspectionDateMaster where Id=" + id);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["Id"].ToString();
                txtShape.Text = ds.Tables[0].Rows[0]["DateName"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddShape.aspx");
            Logs.WriteErrorLog("Masters_Campany_countrymaster|cmdSave_Click|" + ex.Message);
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
        btndelete.Visible = true;
        BtnSave.Text = "Update";
    }
    protected void gvShape_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvShape, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gvShape_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvShape.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void gvShape_RowCreated(object sender, GridViewRowEventArgs e)
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