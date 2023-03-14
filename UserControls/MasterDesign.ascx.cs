using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;


public partial class UserControls_MasterDesign : System.Web.UI.UserControl
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
            Fill_Grid();

        }
        Lblerr.Visible = false;
    }
    public void lablechange()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string design = SqlHelper.ExecuteScalar(con, CommandType.Text, "select DISTINCT ps.Parameter_name from parameter_setting ps ,master_parameter mp where ps.Parameter_Id=mp.Parameter_Id  and ps.user_id='1' and  ps.parameter_id='2' And Ps.Company_Id=" + Session["varCompanyId"] + "").ToString();
            lbldesignname.Text = design;
        }
        catch (Exception ex)
        {
            //UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Design.aspx");
            Lblerr.Visible = true;
            Lblerr.Text = "Data base errer..................";
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
    private void Fill_Grid()
    {
        gdDesign.DataSource = Fill_Grid_Data();
        gdDesign.DataBind();

        Session["ReportPath"] = "Reports/Design.rpt";
        Session["CommanFormula"] = "";
    }

    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "select DesignId as Sr_No,DesignName  as Designname,isnull(DesignCode,'') as DesignCode from Design where MasterCompanyId=" + Session["varCompanyId"];
            if (txtsearchdesign.Text != "")
            {
                strsql = strsql + " and Designname like '" + txtsearchdesign.Text + "%'";
            }
            strsql = strsql + " order by designname";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Design.aspx");
            Logs.WriteErrorLog("Masters_Campany_Design|Fill_Grid_Data|" + ex.Message);
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
    protected void gdDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gdDesign.SelectedDataKey.Value.ToString();
        //Session["id"] = id;
        ViewState["id"] = id;
        txtid.Text = "0";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select designId ,DesignName,isnull(DesignCode,'') as DesignCode from Design where DesignId=" + id + " And MasterCompanyId=" + Session["varCompanyId"] + "");
        try
        {

            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["DesignId"].ToString();
                txtDesign.Text = ds.Tables[0].Rows[0]["DesignName"].ToString();
                txtDesignCode.Text = ds.Tables[0].Rows[0]["DesignCode"].ToString();

            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Design.aspx");
            Logs.WriteErrorLog("Masters_Campany_Design|Fill_Grid_Data|" + ex.Message);
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
    protected void gdDesign_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdDesign.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void gdDesign_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gdDesign, "select$" + e.Row.RowIndex);

        }
    }
    private void CheckDuplicateData()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string strsql = @"Select * from Design Where designName='" + txtDesign.Text + "' and DesignId !=" + txtid.Text + " And MasterCompanyId=" + Session["varCompanyId"];
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Lblerr.Visible = true;
            Lblerr.Text = "Design AlReady Exits........";
            txtDesign.Text = "";
            txtDesign.Focus();
        }
        else
        {
            Lblerr.Visible = false;
        }
    }
    private void CheckDuplicateDesignCode()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string strsql = @"Select * from Design Where designCode='" + txtDesignCode.Text + "' and DesignId !=" + txtid.Text + " And MasterCompanyId=" + Session["varCompanyId"];
        con.Open();
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Lblerr.Visible = true;
            Lblerr.Text = "Design Code AlReady Exits........";
            txtDesignCode.Text = "";
            txtDesignCode.Focus();
        }
        else
        {
            Lblerr.Visible = false;
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (txtDesign.Text != "")
        {
            CheckDuplicateData();

            if (Session["varCompanyId"].ToString() == "21")
            {
                if (txtDesignCode.Text != "")
                {
                    CheckDuplicateDesignCode();
                }
            }

            if (Lblerr.Visible == false)
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
                    _arrPara[0] = new SqlParameter("@DesignId", SqlDbType.Int);
                    _arrPara[1] = new SqlParameter("@DesignName", SqlDbType.NVarChar, 50);
                    _arrPara[2] = new SqlParameter("@varuserid", SqlDbType.Int);
                    _arrPara[3] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                    _arrPara[4] = new SqlParameter("@DesignCode", SqlDbType.VarChar,10);



                    _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                    _arrPara[1].Value = txtDesign.Text.ToUpper();
                    _arrPara[2].Value = Session["varuserid"].ToString();
                    _arrPara[3].Value = Session["varCompanyId"].ToString();
                    _arrPara[4].Value = txtDesignCode.Text.ToUpper();

                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_Design", _arrPara);
                    Tran.Commit();
                    Lblerr.Visible = true;
                    Lblerr.Text = "Save Details....";
                    txtDesign.Text = "";
                    txtid.Text = "0";
                    txtDesignCode.Text = "";
                }
                catch (Exception ex)
                {
                    Tran.Rollback();
                    UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Design.aspx");
                    Lblerr.Visible = true;
                    Lblerr.Text = ex.Message;
                    Logs.WriteErrorLog("Masters_Campany_Design|cmdSave_Click|" + ex.Message);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", "onError();", true);
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
                btnsave.Text = "Save";
                btndelete.Visible = false;

            }
        }
        else
        {
            Lblerr.Visible = true;
            Lblerr.Text = "Fill Details....";
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Report();
    }
    private void Report()
    {
        string qry = @"  SELECT designName  FROM   Design where MasterCompanyid=" + Session["varCompanyId"] + "  ORDER BY designName";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\DesignNew.rpt";
            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\DesignNew.xsd";
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
    protected void btndelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            SqlParameter[] _array = new SqlParameter[5];
            _array[0] = new SqlParameter("@DesignId", ViewState["id"]);
            _array[1] = new SqlParameter("@MasterCompanyId", Session["varCompanyId"]);
            _array[2] = new SqlParameter("@UserId", Session["varuserid"]);
            _array[3] = new SqlParameter("@VarMsg", SqlDbType.NVarChar, 500);
            _array[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_DeleteDesign", _array);
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('" + _array[3].Value + "');", true);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/Design.aspx");

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
        txtDesign.Text = "";
        txtid.Text = "0";
        txtDesignCode.Text = "";
    }


    protected void txtsearchdesign_TextChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void gdDesign_Init(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
    }
}