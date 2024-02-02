using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class UserControls_MasterDyerColorRate : System.Web.UI.UserControl
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
            BindGrid();
            DataSet ds = new DataSet();
            string str = @"select EI.EmpId,EI.EmpName from EmpInfo EI INNER JOIN EmpProcess EP ON EI.EmpId=EP.EmpId where EP.ProcessId=5
                            select ShadecolorId,ShadeColorName from ShadeColor order by ShadeColorName";
            ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref DDDyerName, ds, 0, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDShadeColor, ds, 1, true, "--Select--");

            txtEffectiveDate.Attributes.Add("readonly", "readonly");
            txtEffectiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            //txtcolor.Focus();
        }
        lblMessage.Visible = false;
    }
    protected void DDDyerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDDyerName.SelectedIndex > 0)
        {
            BindGrid();
        }       
    }
    protected void DDShadeColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDShadeColor.SelectedIndex > 0)
        {
            BindGrid();
        }        
    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDDyerName) == false)
        {
            goto a;
        }       
        if (UtilityModule.VALIDDROPDOWNLIST(DDShadeColor) == false)
        {
            goto a;
        }        
        if (UtilityModule.VALIDTEXTBOX(txtRate) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtEffectiveDate) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        lblMessage.Visible = true;
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
    }

    private void SubmitData()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[6];
            if (ViewState["Id"] == null)
            {
                ViewState["Id"] = 0;
            }
            _arrpara[0] = new SqlParameter("@Id", SqlDbType.Int);
            _arrpara[0].Direction = ParameterDirection.InputOutput;
            _arrpara[0].Value = ViewState["Id"];
            _arrpara[1] = new SqlParameter("@DyerShadeColorId", DDShadeColor.SelectedValue);
            _arrpara[2] = new SqlParameter("@DyerId", DDDyerName.SelectedValue);           
            _arrpara[3] = new SqlParameter("@Rate", txtRate.Text == "" ? "0" : txtRate.Text);          
            _arrpara[4] = new SqlParameter("@EffectiveDate", txtEffectiveDate.Text);
            _arrpara[5] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);
            _arrpara[5].Direction = ParameterDirection.Output;           

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_SaveDyerColorRate]", _arrpara);

            lblMessage.Visible = true;
            lblMessage.Text = _arrpara[5].Value.ToString();
            //llMessageBox.Text = "Data Successfully Saved.......";

            ViewState["Id"] = 0;
            Tran.Commit();
            ClearAfterSave();
            BindGrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            ViewState["RateId"] = 0;
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (lblMessage.Text == "")
        {
            SubmitData();
        }
    }
    private void ClearAfterSave()
    {
        //lblMessage.Visible = false;

        DDDyerName.SelectedIndex = -1;
        DDShadeColor.SelectedIndex = -1;
        txtRate.Text = "";
        txtEffectiveDate.Text = "";       
    }
    protected void BindGrid()
    {
        string where = "";
       
        if (DDDyerName.SelectedIndex > 0)
        {
            where = where + " and DCR.DyerId=" + DDDyerName.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            where = where + " and DCR.DyerShadeColorId=" + DDShadeColor.SelectedValue;
        }      
       
            where = where + " and ToDate Is Null"; 

        where = where + @" Order by EI.EmpName";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@Id", "0");
            param[1] = new SqlParameter("@Where", where);

            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetDyerColorRate", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gdDyerColorRate.DataSource = ds.Tables[0];
                gdDyerColorRate.DataBind();
            }
            else
            {
                gdDyerColorRate.DataSource = null;
                gdDyerColorRate.DataBind();
            }

            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblMessage.Text = ex.Message;
            con.Close();
        }
    }
    protected void gdDyerColorRate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] =Page.ClientScript.GetPostBackClientHyperlink(this.gdDyerColorRate, "select$" + e.Row.RowIndex);
            

        }
    }
    protected void gdDyerColorRate_SelectedIndexChanged(object sender, EventArgs e)
    {
        int r = Convert.ToInt32(gdDyerColorRate.SelectedIndex.ToString());

        lblMessage.Text = "";

        btnSave.Text = "Save";

        string id = gdDyerColorRate.SelectedDataKey.Value.ToString();
        hnId.Value = id;

        ViewState["Id"] = id;

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Id", id);

            //**********
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetDyerColorRate", param);

            if (ds.Tables[0].Rows.Count == 1)
            {
                DDDyerName.SelectedValue = ds.Tables[0].Rows[0]["DyerId"].ToString();               
                DDShadeColor.SelectedValue = ds.Tables[0].Rows[0]["DyerShadeColorId"].ToString();
                txtRate.Text = ds.Tables[0].Rows[0]["Rate"].ToString();               
                txtEffectiveDate.Text = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[0]["EffectiveDate"].ToString()).ToString("dd-MMM-yyyy"));                
            }
            //BindGrid();            

            Tran.Commit();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        btnSave.Text = "Update";
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