using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class UserControls_MasterGST : System.Web.UI.UserControl
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
            string Str = @"Select Distinct CI.CompanyId,CompanyName from Companyinfo CI(nolock),Company_Authentication CA(nolock) 
            Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order by CompanyName 
            Select ID, BranchName 
            From BRANCHMASTER BM(nolock) 
            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"] + @" 
            
            Select Process_Name_Id,Process_Name from PROCESS_NAME_MASTER order by Process_Name";

            DataSet DSQ = SqlHelper.ExecuteDataset(Str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, DSQ, 0, true, "Select Comp Name");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, DSQ, 1, false, "");

            if (DDBranchName.Items.Count > 0)
            {
                DDBranchName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, DSQ, 2, true, "--Plz Select--");
            
            txtEffectiveDate.Attributes.Add("readonly", "readonly");
            txtEffectiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            //txtcolor.Focus();
        }
        lblMessage.Visible = false;
    }

    private void BindCategoryMaster()
    {
        if (Session["varcompanyId"] != null)
        {
            if (Session["varcompanyId"].ToString() == "44")
            {
                UtilityModule.ConditionalComboFill(ref DDCategoryName, "select Category_Id,Category_Name from ITEM_CATEGORY_MASTER ICM INNER JOIN CategorySeparate CS ON ICM.CATEGORY_ID=CS.Categoryid   order by Category_Name", true, "--Plz Select--");
            }
            else
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
        }
    }
    private void BindItemName()
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "select ITEM_ID,ITEM_NAME from ITEM_MASTER IM where IM.Category_Id=" + DDCategoryName.SelectedValue + " and IM.MasterCompanyid=" + Session["varCompanyId"] + @" Order by IM.Item_Name", true, "--Plz Select--");

    }
    private void BindQuality()
    {
        UtilityModule.ConditionalComboFill(ref DDQuality, "select QualityId,QualityName from Quality where Item_Id=" + DDItemName.SelectedValue + " and MasterCompanyid=" + Session["varCompanyId"] + @" Order by QualityName", true, "--Plz Select--");
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
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompanyName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDProcessName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCategoryName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtCGSTRate) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtSGSTRate) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtIGSTRate) == false)
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
            SqlParameter[] _arrpara = new SqlParameter[14];
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
            _arrpara[5] = new SqlParameter("@CGSTRate", txtCGSTRate.Text == "" ? "0" : txtCGSTRate.Text);
            _arrpara[6] = new SqlParameter("@SGSTRate", txtSGSTRate.Text == "" ? "0" : txtSGSTRate.Text);
            _arrpara[7] = new SqlParameter("@IGSTRate", txtIGSTRate.Text == "" ? "0" : txtIGSTRate.Text);
            _arrpara[8] = new SqlParameter("@EffectiveDate", txtEffectiveDate.Text);
            _arrpara[9] = new SqlParameter("@MasterCompanyId", Session["varcompanyId"]);
            _arrpara[10] = new SqlParameter("@UserId", Session["varuserid"]);
            _arrpara[11] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);
            _arrpara[11].Direction = ParameterDirection.Output;
            _arrpara[12] = new SqlParameter("@CompanyID", DDCompanyName.SelectedValue);
            _arrpara[13] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_SaveGSTMasterRate]", _arrpara);

            lblMessage.Visible = true;
            lblMessage.Text = _arrpara[11].Value.ToString();
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
        txtCGSTRate.Text = "";
        txtSGSTRate.Text = "";
        txtIGSTRate.Text = "";
        txtEffectiveDate.Text = "";   
        btnSave.Text = "Save";
    }
    protected void BindGrid()
    {
        string where = "";
        where = where + " and GMR.BranchID = " + DDBranchName.SelectedValue;

        if (DDCompanyName.SelectedIndex > 0)
        {
            where = where + " and GMR.CompanyID = " + DDCompanyName.SelectedValue;
        }
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
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetGSTMasterRate", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gdGSTRate.DataSource = ds.Tables[0];
                gdGSTRate.DataBind();
            }
            else
            {
                gdGSTRate.DataSource = null;
                gdGSTRate.DataBind();
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
    protected void gdGSTRate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gdGSTRate, "select$" + e.Row.RowIndex);
        }
    }
    protected void gdGSTRate_SelectedIndexChanged(object sender, EventArgs e)
    {
        int r = Convert.ToInt32(gdGSTRate.SelectedIndex.ToString());

        lblMessage.Text = "";

        btnSave.Text = "Save";

        string id = gdGSTRate.SelectedDataKey.Value.ToString();
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
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetGSTMasterRate", param);

            if (ds.Tables[0].Rows.Count == 1)
            {
                DDProcessName.SelectedValue = ds.Tables[0].Rows[0]["ProcessId"].ToString();

                BindCategoryMaster();
                DDCategoryName.SelectedValue = ds.Tables[0].Rows[0]["Categoryid"].ToString();

                BindItemName();
                DDItemName.SelectedValue = ds.Tables[0].Rows[0]["ItemId"].ToString();

                BindQuality();
                DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
               
                txtCGSTRate.Text = ds.Tables[0].Rows[0]["CGSTRate"].ToString();
                txtSGSTRate.Text = ds.Tables[0].Rows[0]["SGSTRate"].ToString();
                txtIGSTRate.Text = ds.Tables[0].Rows[0]["IGSTRate"].ToString();
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