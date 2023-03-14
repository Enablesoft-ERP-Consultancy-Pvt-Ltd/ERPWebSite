using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class UserControls_MasterWeaverRawSubItemRate : System.Web.UI.UserControl
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
            BindGrid();
            BindItemName();

            txtEffectiveDate.Attributes.Add("readonly", "readonly");
            txtEffectiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            //txtcolor.Focus();
        }
        lblMessage.Visible = false;
    }
    private void BindItemName()
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "select ITEM_ID,ITEM_NAME from ITEM_MASTER IM INNER JOIN CategorySeparate CS ON IM.CATEGORY_ID=CS.Categoryid where CS.id=1 and IM.MasterCompanyid=" + Session["varCompanyId"] + @" Order by IM.Item_Name", true, "--Plz Select--");

    }

    private void BindQuality()
    {
        UtilityModule.ConditionalComboFill(ref DDQuality, "select QualityId,QualityName from Quality where Item_Id=" + DDItemName.SelectedValue + " and MasterCompanyid=" + Session["varCompanyId"] + @" Order by QualityName", true, "--Plz Select--");
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDItemName.SelectedIndex > 0)
        {
            BindQuality();
            BindGrid();
            txtEffectiveDate.Text = "";
        }
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDQuality.SelectedIndex > 0)
        {
            BindGrid();
        }
    }   
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDItemName) == false)
        {
            goto a;
        }       
        if (UtilityModule.VALIDDROPDOWNLIST(DDQuality) == false)
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
            SqlParameter[] _arrpara = new SqlParameter[9];
            if (ViewState["Id"] == null)
            {
                ViewState["Id"] = 0;
            }
            _arrpara[0] = new SqlParameter("@Id", SqlDbType.Int);
            _arrpara[0].Direction = ParameterDirection.InputOutput;
            _arrpara[0].Value = ViewState["Id"];
            _arrpara[1] = new SqlParameter("@ItemId", DDItemName.SelectedValue);
            _arrpara[2] = new SqlParameter("@QualityId", DDQuality.SelectedValue);           
            _arrpara[3] = new SqlParameter("@Rate", txtRate.Text == "" ? "0" : txtRate.Text);          
            _arrpara[4] = new SqlParameter("@EffectiveDate", txtEffectiveDate.Text);
            _arrpara[5] = new SqlParameter("@MasterCompanyId", Session["varcompanyId"]);
            _arrpara[6] = new SqlParameter("@UserId", Session["varuserid"]);
            _arrpara[7] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);
            _arrpara[7].Direction = ParameterDirection.Output;
            _arrpara[8] = new SqlParameter("@Rate2", txtRate2.Text == "" ? "0" : txtRate2.Text);   

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_SaveWeaverRawSubItemRate]", _arrpara);

            lblMessage.Visible = true;
            lblMessage.Text = _arrpara[7].Value.ToString();
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
        ////lblMessage.Visible = false;
        //DDItemName.SelectedIndex = -1;
        DDQuality.SelectedIndex = -1;
        txtRate.Text = "";
        txtRate2.Text = "";
        //txtEffectiveDate.Text = "";   
        btnSave.Text = "Save";        
    }
    protected void BindGrid()
    {
        string where = "";
       
        if (DDItemName.SelectedIndex > 0)
        {
            where = where + " and WR.ItemId=" + DDItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and WR.QualityId=" + DDQuality.SelectedValue;
        }       
            where = where + " and ToDate Is Null";

            where = where + @" Order by Q.QualityName";           

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
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetWeaverRawSubItemRate", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gdWeaverRawSubItemRate.DataSource = ds.Tables[0];
                gdWeaverRawSubItemRate.DataBind();
            }
            else
            {
                gdWeaverRawSubItemRate.DataSource = null;
                gdWeaverRawSubItemRate.DataBind();                
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
    protected void gdWeaverRawSubItemRate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gdWeaverRawSubItemRate, "select$" + e.Row.RowIndex);
        }
    }
    protected void gdWeaverRawSubItemRate_SelectedIndexChanged(object sender, EventArgs e)
    {
        int r = Convert.ToInt32(gdWeaverRawSubItemRate.SelectedIndex.ToString());

        lblMessage.Text = "";

        btnSave.Text = "Save";

        string id = gdWeaverRawSubItemRate.SelectedDataKey.Value.ToString();
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
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetWeaverRawSubItemRate", param);

            if (ds.Tables[0].Rows.Count == 1)
            {
               DDItemName.SelectedValue = ds.Tables[0].Rows[0]["ItemId"].ToString();               
               DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
               BindQuality();
               txtRate.Text = ds.Tables[0].Rows[0]["Rate"].ToString();
               txtRate2.Text = ds.Tables[0].Rows[0]["Rate2"].ToString(); 
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

    //protected void btnrpt_Click(object sender, EventArgs e)
    //{
    //    Report();
    //}
    //private void Report()
    //{
    //    string qry = @" SELECT ColorName  FROM   Color Where MasterCompanyId=" + Session["varCompanyId"] + "  ORDER BY ColorName";
    //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        Session["rptFileName"] = "~\\Reports\\ColorNew.rpt";
    //        //Session["rptFileName"] = Session["ReportPath"];
    //        Session["GetDataset"] = ds;
    //        Session["dsFileName"] = "~\\ReportSchema\\ColorNew.xsd";
    //        StringBuilder stb = new StringBuilder();
    //        stb.Append("<script>");
    //        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
    //        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
    //    }
    //}
  
    //protected void btnpreview_Click(object sender, EventArgs e)
    //{
    //    string str = "select Colorid,ColorName from Color";
    //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        Session["rptfilename"] = "~\\Reports\\Rptcolornew.rpt";
    //        Session["Getdataset"] = ds;
    //        Session["dsFilename"] = "~\\ReportSchema\\Rptcolornew.xsd";
    //        StringBuilder stb = new StringBuilder();
    //        stb.Append("<script>");
    //        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
    //        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
    //    }

    //}
}