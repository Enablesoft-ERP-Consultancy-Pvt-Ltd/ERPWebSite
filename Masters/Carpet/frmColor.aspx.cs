using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Carpet_frmColor : CustomPage
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
            string color = SqlHelper.ExecuteScalar(con, CommandType.Text, "select DISTINCT ps.Parameter_name from parameter_setting ps ,master_parameter mp where ps.Parameter_Id=mp.Parameter_Id  and  ps.parameter_id='3' And ps.Company_Id="+ Session["varCompanyId"] + "").ToString();
            lblcolorname.Text = color;
        }
        catch (Exception ex)
        {
            //UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmColor.aspx");
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
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
        Session["ReportPath"] = "Reports/RptColor.rpt";
        Session["CommanFormula"] = "";
    }

    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"SELECT ColorId as Sr_No,ColorName as " + lblcolorname.Text + " FROM Color where MasterCompanyId=" + Session["varCompanyId"] + " Order By ColorId";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            ds.Tables[0].Columns[1].ColumnName = "Color Name";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmColor.aspx");
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
            //CheckDuplicateDate();
            if (lblMessage.Visible == false)
            {
                SqlParameter[] _arrPara = new SqlParameter[5];
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
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmColor.aspx");
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
        string strsql = @"Select ColorId from Color Where ColorName='" + txtcolor.Text + "' and ColorId !=" + txtid.Text + " And MasterCompanyId=" + Session["varCompanyId"];
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Color Name AlReady Exists";
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
        string strsql = @"Select ColorId from Color Where ColorName='" + txtcolor.Text + "' and ColorId !=" + txtid.Text + " And MasterCompanyId=" + Session["varCompanyId"];
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Color AlReady Exists........";
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
        Session["ReportPath"] = "Reports/RptColor.rpt";
        Session["CommanFormula"] = "";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "addpriview();", true);
        //Report();
    }
    private void Report()
    {
        string qry = @" SELECT ColorName  FROM   Color where  MasterCompanyId=" + Session["varCompanyId"] + "  ORDER BY ColorName";
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
        Session["id"] = id;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from Color WHERE ColorId=" + id + " And MasterCompanyId=" + Session["varCompanyId"] + "");
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
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmColor.aspx");
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
            SqlParameter[] parparam = new SqlParameter[3];
            parparam[0] = new SqlParameter("@id", Session["id"].ToString());
            parparam[1] = new SqlParameter("@varCompanyId", Session["varCompanyId"].ToString());
            parparam[2] = new SqlParameter("@varuserid", Session["varuserid"].ToString());

            int id = SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Proc_DeleteColor", parparam);
            if (id > 0)
            {
               lblMessage.Visible = true;
                lblMessage.Text = "Record(s) has been deleted!";
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Value in Use...";

            }

          
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/FrmColor.aspx");
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
   
}