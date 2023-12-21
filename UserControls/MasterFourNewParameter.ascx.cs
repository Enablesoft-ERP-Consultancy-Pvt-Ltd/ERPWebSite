using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class UserControls_MasterFourNewParameter : System.Web.UI.UserControl
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
            TxtType.Text = Request.QueryString["Type"];
            if (TxtType.Text == "")
            {
                TxtType.Text = "9";
            }
            lablechange();
            fill_grid();
            txtName.Focus();
            
        }
        lblMessage.Visible = false;
    }
    public void lablechange()
    {
        string Str = @"Select ps.Parameter_Name 
                    From parameter_setting ps(nolock)
                    JOIN master_parameter mp(nolock) ON mp.Parameter_Id = ps.Parameter_Id 
                    Where ps.company_id = " + Session["varCompanyId"] + " And ps.parameter_id = " + TxtType.Text;

        string Name = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str).ToString();

        lblcolorname.Text = Name;
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
        try
        {
            string strsql = "Select ID Sr_No, [Name] " + lblcolorname.Text + @" 
            From ContentDescriptionPatternFitSize(Nolock) 
            Where MasterCompanyID = " + Session["varCompanyId"] + " And [TYPE] = " + TxtType.Text + " Order By [Name]";

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/UserControls/MasterFourNewParameter");
        }
        return ds;
    }
    private void ClearAll()
    {
        txtid.Text = "0";
        txtName.Text = "";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            if (lblMessage.Visible == false)
            {
                SqlParameter[] _arrPara = new SqlParameter[8];
                _arrPara[0] = new SqlParameter("@ID", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@Name", SqlDbType.NVarChar, 100);
                _arrPara[2] = new SqlParameter("@Code", SqlDbType.VarChar, 10);
                _arrPara[3] = new SqlParameter("@Type", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@SaveUpdateFlag", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@UserID", SqlDbType.Int);
                _arrPara[6] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
                _arrPara[7] = new SqlParameter("@Msg", SqlDbType.NVarChar, 100);

                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                _arrPara[1].Value = txtName.Text.ToUpper();
                _arrPara[2].Value = "";
                _arrPara[3].Value = TxtType.Text;
                _arrPara[4].Value = btnSave.Text == "Update" ? 1 : 0;
                _arrPara[5].Value = Session["varuserid"].ToString();
                _arrPara[6].Value = Session["varCompanyId"].ToString();
                _arrPara[7].Value = 0;
                _arrPara[7].Direction = ParameterDirection.Output;


                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveForMasterParameter", _arrPara);


                if (_arrPara[7].Value.ToString() != "")
                {
                    Tran.Rollback();
                    lblMessage.Text = _arrPara[7].Value.ToString();
                }
                else
                {
                    Tran.Commit();
                    ClearAll();
                    lblMessage.Visible = true;
                    lblMessage.Text = "Save Details........";
                    fill_grid();
                }
            }
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
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
        ViewState["id"] = id;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
        @"Select * From ContentDescriptionPatternFitSize(Nolock) Where ID = " + id);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["ID"].ToString();
                txtName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/UserControls/MasterFourNewParameter");
        }
        btnSave.Text = "Update";
    }
    protected void gdcolor_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gdcolor, "Select$" + e.Row.RowIndex);
        }
    }

    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = @"Select a.ID, a.[Name], a.Code, a.[Type], a.DateAdded, a.UserID, a.MasterCompanyID, PS.Parameter_Name 
        From ContentDescriptionPatternFitSize a(Nolock) 
        JOIN Parameter_Setting PS(Nolock) ON PS.Parameter_Id = a.[Type] 
        Where Type = " + TxtType.Text + " Order By a.[Name]";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptfilename"] = "~\\Reports\\RptFourNewParameter.rpt";
            Session["Getdataset"] = ds;
            Session["dsFilename"] = "~\\ReportSchema\\RptFourNewParameter.xsd";
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
    protected void BtnNew_Click(object sender, EventArgs e)
    {

    }
}