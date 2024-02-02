using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class UserControls_MasterColor : System.Web.UI.UserControl
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
            lablechange();
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
            string color = SqlHelper.ExecuteScalar(con, CommandType.Text, "select DISTINCT ps.Parameter_name from parameter_setting ps ,master_parameter mp where ps.Parameter_Id=mp.Parameter_Id And  ps.company_id=" + Session["varMasterCompanyIDForERP"] + "  and  ps.parameter_id='3'").ToString();
            lblcolorname.Text = color;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Color.aspx");
            lblMessage.Visible = true;
            lblMessage.Text = "Data base errer..................";
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
        Session["ReportPath"] = "Reports/Color.rpt";
        Session["CommanFormula"] = "";
    }

    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"SELECT ColorId as Sr_No,ColorName as " + lblcolorname.Text + ",isnull(ColorCode,'') as ColorCode FROM Color Where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + "  Order By ColorId";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            ds.Tables[0].Columns[1].ColumnName = "Color Name";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Color.aspx");
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
                SqlParameter[] _arrPara = new SqlParameter[5];
                _arrPara[0] = new SqlParameter("@ColorId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@ColorName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@ColorCode", SqlDbType.VarChar,10);

                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                _arrPara[1].Value = txtcolor.Text.ToUpper();
                _arrPara[2].Value = Session["varuserid"].ToString();
                _arrPara[3].Value = Session["varMasterCompanyIDForERP"].ToString();
                _arrPara[4].Value = txtColorCode.Text.ToUpper();
                con.Open();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_Color", _arrPara);
                ClearAll();
                lblMessage.Visible = true;
                lblMessage.Text = "Save Details........";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Color.aspx");
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
        string strsql = @"Select * from Color Where ColorName='" + txtcolor.Text + "' and ColorId !=" + txtid.Text + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
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
        string strsql = @"Select * from Color Where ColorName='" + txtcolor.Text + "' and ColorId !=" + txtid.Text + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
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
    private void CheckDuplicateColorCode()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string strsql = @"Select * from Color Where ColorCode='" + txtColorCode.Text + "' and ColorId !=" + txtid.Text + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Color Code AlReady Exits........";
            txtColorCode.Text = "";
            txtColorCode.Focus();
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
        txtColorCode.Text = "";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtcolor.Text != "")
        {
            CheckDuplicateData();

            if (Session["varMasterCompanyIDForERP"].ToString() == "21")
            {
                if (txtColorCode.Text != "")
                {
                    CheckDuplicateColorCode();
                }
            }
            if (lblMessage.Visible == false)
            {
                Store_Data();
            }
            txtColorCode.Text = "";
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
        Report();
    }
    private void Report()
    {
        string qry = @" SELECT ColorName  FROM   Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  ORDER BY ColorName";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\ColorNew.rpt";
            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\ColorNew.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
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
                txtColorCode.Text = ds.Tables[0].Rows[0]["ColorCode"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Color.aspx");
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
            e.Row.Attributes["onclick"] =Page.ClientScript.GetPostBackClientHyperlink(this.gdcolor, "Select$" + e.Row.RowIndex);
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        btnSave.Text = "Save";
        txtid.Text = "0";
        txtcolor.Text = "";
        txtColorCode.Text = "";
        btndelete.Visible = false;
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            SqlParameter[] _array = new SqlParameter[5];
            _array[0] = new SqlParameter("@ColorID", ViewState["id"]);
            _array[1] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
            _array[2] = new SqlParameter("@UserId", Session["varuserid"]);
            _array[3] = new SqlParameter("@VarMsg", SqlDbType.NVarChar, 500);
            _array[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_Delete_Color", _array);
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('" + _array[3].Value + "');", true);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Color.aspx");
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
        txtColorCode.Text = "";

    }


    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = "select Colorid,ColorName from Color";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptfilename"] = "~\\Reports\\Rptcolornew.rpt";
            Session["Getdataset"] = ds;
            Session["dsFilename"] = "~\\ReportSchema\\Rptcolornew.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }

    }
}