using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class UserControls_MasterTCS : System.Web.UI.UserControl
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
            //BindGrid();
            BindProcessName();

            txtEffectiveDate.Attributes.Add("readonly", "readonly");
            txtEffectiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            //txtcolor.Focus();
        }
        lblMessage.Visible = false;
    }
    private void BindProcessName()
    {
        UtilityModule.ConditionalComboFill(ref DDProcessName, "select Process_Name_Id,Process_Name from PROCESS_NAME_MASTER Where Process_Name='PURCHASE' order by Process_Name", true, "--Plz Select--");
    }
    private void BindCategoryMaster()
    {
        if (DDProcessName.SelectedValue == "9" || DDProcessName.SelectedValue == "5")
        {
            UtilityModule.ConditionalComboFill(ref DDCategoryName, "select Category_Id,Category_Name from ITEM_CATEGORY_MASTER ICM INNER JOIN CategorySeparate CS ON ICM.CATEGORY_ID=CS.Categoryid where CS.id=1  order by Category_Name", true, "--Plz Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDCategoryName, "select Category_Id,Category_Name from ITEM_CATEGORY_MASTER ICM INNER JOIN CategorySeparate CS ON ICM.CATEGORY_ID=CS.Categoryid where CS.id=0 order by Category_Name", true, "--Plz Select--");
        }        
    }
    private void BindItemName()
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "select ITEM_ID,ITEM_NAME from ITEM_MASTER IM where IM.Category_Id=" + DDCategoryName.SelectedValue + " and IM.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" Order by IM.Item_Name", true, "--Plz Select--");

    }
    private void BindQuality()
    {
        UtilityModule.ConditionalComboFill(ref DDQuality, "select QualityId,QualityName from Quality where Item_Id=" + DDItemName.SelectedValue + " and MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" Order by QualityName", true, "--Plz Select--");
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDProcessName.SelectedIndex > 0)
        {
            BindCategoryMaster();
            //BindGrid();
            txtEffectiveDate.Text = "";
        }
        else
        {
            DDItemName.Items.Clear();
            DDQuality.Items.Clear();
        }
        BindGrid();
    }
    protected void DDCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDCategoryName.SelectedIndex > 0)
        {
            BindItemName();
            //BindGrid();
            txtEffectiveDate.Text = "";
        }
        else
        {
            DDItemName.Items.Clear();
            DDQuality.Items.Clear();
        }
        BindGrid();
    }

    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDItemName.SelectedIndex > 0)
        {
            BindQuality();
            //BindGrid();
            txtEffectiveDate.Text = "";
        }
        else
        {
            DDQuality.Items.Clear();
        }
        BindGrid();
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
        //if (DDQuality.SelectedIndex > 0)
        //{
        //    BindGrid();
        //}
    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDProcessName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCategoryName) == false)
        {
            goto a;
        }
        //if (UtilityModule.VALIDDROPDOWNLIST(DDItemName) == false)
        //{
        //    goto a;
        //}
        //if (UtilityModule.VALIDDROPDOWNLIST(DDQuality) == false)
        //{
        //    goto a;
        //}
        if (UtilityModule.VALIDTEXTBOX(txtTCSRate) == false)
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
            SqlParameter[] _arrpara = new SqlParameter[10];
            if (ViewState["Id"] == null)
            {
                ViewState["Id"] = 0;
            }
            _arrpara[0] = new SqlParameter("@Id", SqlDbType.Int);
            _arrpara[0].Direction = ParameterDirection.InputOutput;
            _arrpara[0].Value = ViewState["Id"];

            _arrpara[1] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
            _arrpara[2] = new SqlParameter("@CategoryId", DDCategoryName.SelectedValue);            
            _arrpara[3] = new SqlParameter("@ItemId", DDItemName.SelectedIndex > 0 ? DDItemName.SelectedValue : "0");
            _arrpara[4] = new SqlParameter("@QualityId", DDQuality.SelectedIndex > 0 ? DDQuality.SelectedValue : "0");
            _arrpara[5] = new SqlParameter("@TCSRate", txtTCSRate.Text == "" ? "0" : txtTCSRate.Text);           
            _arrpara[6] = new SqlParameter("@EffectiveDate", txtEffectiveDate.Text);
            _arrpara[7] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
            _arrpara[8] = new SqlParameter("@UserId", Session["varuserid"]);
            _arrpara[9] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);
            _arrpara[9].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_SaveTCSMasterRate]", _arrpara);

            lblMessage.Visible = true;
            lblMessage.Text = _arrpara[9].Value.ToString();
            //llMessageBox.Text = "Data Successfully Saved.......";            

            ViewState["Id"] = 0;
            Tran.Commit();
            ClearAfterSave();
            BindGrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            ViewState["Id"] = 0;
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
        txtTCSRate.Text = "";       
        txtEffectiveDate.Text = "";   
        btnSave.Text = "Save";
    }
    protected void BindGrid()
    {
        string where = "";

        if (DDProcessName.SelectedIndex > 0)
        {
            where = where + " and GMR.ProcessId=" + DDProcessName.SelectedValue;
        }
        if (DDCategoryName.SelectedIndex > 0)
        {
            where = where + " and GMR.CategoryId=" + DDCategoryName.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            where = where + " and GMR.ItemId=" + DDItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and GMR.QualityId=" + DDQuality.SelectedValue;
        }
        where = where + " and ToDate Is Null";

        where = where + @" Order by PNM.PROCESS_NAME";

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
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetTCSMasterRate", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gdTCSRate.DataSource = ds.Tables[0];
                gdTCSRate.DataBind();
            }
            else
            {
                gdTCSRate.DataSource = null;
                gdTCSRate.DataBind();
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
    protected void gdTCSRate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gdTCSRate, "select$" + e.Row.RowIndex);
        }
    }
    protected void gdTCSRate_SelectedIndexChanged(object sender, EventArgs e)
    {
        int r = Convert.ToInt32(gdTCSRate.SelectedIndex.ToString());

        lblMessage.Text = "";

        btnSave.Text = "Save";

        string id = gdTCSRate.SelectedDataKey.Value.ToString();
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
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetTCSMasterRate", param);

            if (ds.Tables[0].Rows.Count == 1)
            {
                BindProcessName();
                DDProcessName.SelectedValue = ds.Tables[0].Rows[0]["ProcessId"].ToString();

                BindCategoryMaster();
                DDCategoryName.SelectedValue = ds.Tables[0].Rows[0]["Categoryid"].ToString();

                BindItemName();
                DDItemName.SelectedValue = ds.Tables[0].Rows[0]["ItemId"].ToString();

                BindQuality();
                DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
               
                txtTCSRate.Text = ds.Tables[0].Rows[0]["TCSRate"].ToString();              
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
    //    string qry = @" SELECT ColorName  FROM   Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  ORDER BY ColorName";
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