using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class frmColor : CustomPage
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
           // lablechange();
            fill_grid();            
            txtcolor.Focus();
         }
        lblMessage.Visible = false;
    }
    public void lablechange()
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string color=SqlHelper.ExecuteScalar(con, CommandType.Text, "select DISTINCT ps.Parameter_name from parameter_setting ps ,master_parameter mp where ps.Parameter_Id=mp.Parameter_Id And  ps.company_id='1' and ps.user_id='1' and  ps.parameter_id='3'").ToString();
            lblcolorname.Text = color;
        }
        catch (Exception ex)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Data base errer..................";
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmDepartment.aspx");
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
    
    private void fill_grid()
    {
        gdcolor.DataSource = Fill_Grid_Data();
        gdcolor.DataBind();
        Session["ReportPath"]="Color.rpt";
        Session["CommanFormula"] = "";       
    }

    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"SELECT ColorId as Sr_No,ColorName as " + lblcolorname.Text + " FROM Color Where MasterCompanyId=" + Session["varCompanyId"] + " Order By ColorId";            
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);                   
            ds.Tables[0].Columns["ColorName"].ColumnName = "Color Name";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmDepartment.aspx");
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

    private void Store_Data()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            CheckDuplicateDate();
            if (lblMessage.Visible == false)
            {
                SqlParameter[] _arrPara = new SqlParameter[4];
                _arrPara[0] = new SqlParameter("@ColorId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@ColorName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@varCompanyId", SqlDbType.Int);

                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                _arrPara[1].Value = txtcolor.Text.ToUpper();
                _arrPara[2].Value = Session["varuserid"].ToString();
                _arrPara[3].Value = Session["varCompanyId"].ToString();
                con.Open();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_Color", _arrPara);
                ClearAll();
                lblMessage.Visible = true;
                lblMessage.Text = "Save Details........";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmDepartment.aspx");
            Logs.WriteErrorLog("Masters_Carpet_FrmColor|cmdSave_Click|" + ex.Message);
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
        fill_grid();
        
    }

    private void CheckDuplicateDate()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string strsql = @"Select * from Color Where ColorName='" + txtcolor.Text + "' and ColorId !="+txtid.Text + " And MasterCompanyId= " + Session["varCompanyId"];
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Color Name AlReady Exits";
            txtcolor.Text = "";
            txtcolor.Focus();
        }
        else
        {
            lblMessage.Visible = false;
        }
    }
    private void CheckDuplicateData()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string strsql = @"Select * from Color Where ColorName='" + txtcolor.Text + "' and ColorId !=" + txtid.Text + " And MasterCompanyId=" + Session["varCompanyId"];
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Color AlReady Exits........";
            txtcolor.Text = "";
            txtcolor.Focus();
        }
        else
        {
            lblMessage.Visible = false;
        }
    }
    private void ClearAll()
    {
       txtid.Text = "0";
       txtcolor.Text = "";                   
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtcolor.Text != "")
        {
            CheckDuplicateData();
            if (lblMessage.Visible == false)
            {
                Store_Data();
            }
            txtcolor.Text = "";
            btnSave.Text = "Save";
            btndelete.Visible = false;
        }
        else
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Fill Details........";
        }
    }

    protected void btnrpt_Click(object sender, EventArgs e)
    { 

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        ClearAll();
    }
    
    protected void gdcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
      string id = gdcolor.SelectedDataKey.Value.ToString();
      //Session["id"] = id;
      ViewState["id"] = id;
      SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
      DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from Color WHERE ColorId=" + id);
      try
       {
        if (ds.Tables[0].Rows.Count == 1)
         {
           txtid.Text = ds.Tables[0].Rows[0]["ColorId"].ToString();
           txtcolor.Text = ds.Tables[0].Rows[0]["ColorName"].ToString();            
         }
       }
    catch (Exception ex)
    {
        UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmDepartment.aspx");
    }
    finally
    {
        if (con.State == ConnectionState.Open)
        {
            con.Close();
            con.Dispose();
        }
    }
      btnSave.Text = "Update";
      btndelete.Visible = true;
    }
    protected void gdcolor_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdcolor, "Select$" + e.Row.RowIndex);
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        btnSave.Text = "Save";
        txtid.Text = "0";
        txtcolor.Text = "";
        btndelete.Visible = false;
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select COLOR_ID from ITEM_PARAMETER_MASTER where MasterCompanyId=" + Session["varCompanyId"] + " And COLOR_ID=" + ViewState["id"].ToString()));
            if (id <= 0)
            {
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "delete  from Color where ColorId=" + ViewState["id"].ToString());
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varCompanyId"].ToString() + "," + Session["varuserid"].ToString() + ",'Color'," + ViewState["id"].ToString() + ",getdate(),'Delete')");
                lblMessage.Visible = true;
                lblMessage.Text = "Value Deleted....";
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Value in Use...";
                
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmDepartment.aspx");
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
        btnSave.Text = "Save";
        txtcolor.Text = "";
        txtid.Text = "0";

    }
    protected void gdcolor_RowCreated(object sender, GridViewRowEventArgs e)
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