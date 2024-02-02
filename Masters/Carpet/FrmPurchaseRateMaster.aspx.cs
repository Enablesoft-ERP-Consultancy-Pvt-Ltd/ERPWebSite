using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ClosedXML.Excel;
using System.IO;

public partial class Masters_Process_FrmPurchaseRateMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = null;
            DataSet ds = null;
            str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                    From Companyinfo CI(nolock)
                    JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @"  
                    Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName                     
                                        
                    select PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER With(nolock) Where MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" and PROCESS_NAME='PURCHASE' Order By PROCESS_NAME 
                    Select ID, BranchName 
                                From BRANCHMASTER BM(nolock) 
                                JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                                Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @" ";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }   
            UtilityModule.ConditionalComboFillWithDS(ref ddJob, ds, 1, true, "--Plz Select--");           
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 2, false, "");

            if (DDBranchName.Items.Count > 0)
            {
                DDBranchName.Enabled = false;
            }
            ds.Dispose();

            BindCategoryMaster();
            if (DDCategory.Items.Count > 0)
            {
                DDCategory.SelectedIndex = 1;
                DDCategory_SelectedIndexChanged(DDCategory, new EventArgs());
            }

            BindPurchaseVendor();

            txtEffectiveDate.Attributes.Add("readonly", "readonly");
            txtEffectiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            
        }
    }
    private void BindCategoryMaster()
    {        
            UtilityModule.ConditionalComboFill(ref DDCategory, "select Category_Id,Category_Name from ITEM_CATEGORY_MASTER ICM INNER JOIN CategorySeparate CS ON ICM.CATEGORY_ID=CS.Categoryid where CS.id=1  order by Category_Name", true, "--Plz Select--");
        
        
    }
    protected void BindPurchaseVendor()
    {
        string Str = "select distinct EI.empid,EI.empname from empinfo EI inner join Department DM on EI.Departmentid=DM.Departmentid Where EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and EI.blacklist=0";
        if (Session["varCompanyno"].ToString() != "6")
        {
            Str = Str + "  AND DM.Departmentname='PURCHASE'";
        }
        Str = Str + "  Order By empname ";
        UtilityModule.ConditionalComboFill(ref DDPurchaseVendor, Str, true, "--Select Party--");
    }

    private void BindItemName()
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "select ITEM_ID,ITEM_NAME from ITEM_MASTER IM where IM.Category_Id=" + DDCategory.SelectedValue + " and IM.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" Order by IM.Item_Name", true, "--Plz Select--");

    }
    private void BindQuality()
    {
        UtilityModule.ConditionalComboFill(ref ddquality, "select QualityId,QualityName from Quality where Item_Id=" + DDItemName.SelectedValue + " and MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" Order by QualityName", true, "--Plz Select--");
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
            ddquality.Items.Clear();
        } 
        fillGrid();
    }    
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

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

            _arrpara[1] = new SqlParameter("@CompanyID", DDCompanyName.SelectedValue);
            _arrpara[2] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);
            _arrpara[3] = new SqlParameter("@ProcessId", ddJob.SelectedValue);          
            _arrpara[4] = new SqlParameter("@CategoryId", DDCategory.SelectedValue);
            _arrpara[5] = new SqlParameter("@ItemId", DDItemName.SelectedIndex > 0 ? DDItemName.SelectedValue : "0");
            _arrpara[6] = new SqlParameter("@QualityId", ddquality.SelectedIndex > 0 ? ddquality.SelectedValue : "0");
            _arrpara[7] = new SqlParameter("@EmpId", DDPurchaseVendor.SelectedIndex > 0 ? DDPurchaseVendor.SelectedValue : "0");
            _arrpara[8] = new SqlParameter("@PurchaseRate", txtrate.Text == "" ? "0" : txtrate.Text);          
            _arrpara[9] = new SqlParameter("@EffectiveDate", txtEffectiveDate.Text);
            _arrpara[10] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
            _arrpara[11] = new SqlParameter("@UserId", Session["varuserid"]);
            _arrpara[12] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);
            _arrpara[12].Direction = ParameterDirection.Output;


            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_SavePurchaseRateMaster]", _arrpara);

            lblMessage.Visible = true;
            lblMessage.Text = _arrpara[12].Value.ToString();
            //llMessageBox.Text = "Data Successfully Saved.......";            

            ViewState["Id"] = 0;
            Tran.Commit();
            ClearAfterSave();
            fillGrid();
           
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
            con.Dispose();
            con.Close();
        }
    }
    private void ClearAfterSave()
    {
        ////lblMessage.Visible = false;
        //DDItemName.SelectedIndex = -1;
      ddquality.SelectedIndex = -1;
      txtrate.Text = "";
        txtEffectiveDate.Text = "";        
        btnsave.Text = "Save";
    }
    protected void fillGrid()
    {

        string where = "";
        where = where + " and PRM.BranchID = " + DDBranchName.SelectedValue;

        if (DDCompanyName.SelectedIndex > 0)
        {
            where = where + " and PRM.CompanyID = " + DDCompanyName.SelectedValue;
        }
        if (ddJob.SelectedIndex > 0)
        {
            where = where + " and PRM.ProcessId=" + ddJob.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            where = where + " and PRM.CategoryId=" + DDCategory.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            where = where + " and PRM.ItemId=" + DDItemName.SelectedValue;
        }
        if (ddquality.SelectedIndex > 0)
        {
            where = where + " and PRM.QualityId=" + ddquality.SelectedValue;
        }
        if (DDPurchaseVendor.SelectedIndex > 0)
        {
            where = where + " and PRM.EmpId=" + DDPurchaseVendor.SelectedValue;
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
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_BindPurchaseRateMaster", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DGRateDetail.DataSource = ds.Tables[0];
                DGRateDetail.DataBind();
            }
            else
            {
                DGRateDetail.DataSource = null;
                DGRateDetail.DataBind();
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

    protected void ddJob_SelectedIndexChanged(object sender, EventArgs e)
    {        
        fillGrid();
    }   

    protected void ddquality_SelectedIndexChanged(object sender, EventArgs e)
    {       
        fillGrid();
    }    
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDCategory.SelectedIndex > 0)
        {
            BindItemName();
            //BindGrid();
            txtEffectiveDate.Text = "";
        }
        else
        {
            DDItemName.Items.Clear();
            ddquality.Items.Clear();            
        }
        fillGrid();
    }
    
    protected void DDWeavingEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillGrid();
    }
    protected void DGRateDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGRateDetail, "select$" + e.Row.RowIndex);

            //for (int i = 0; i < DGRateDetail.Columns.Count; i++)
            //{
            //    if (DGRateDetail.Columns[i].HeaderText == "Rate Location")
            //    {
            //        if (variable.VarDEFINEPROCESSRATE_LOCATIONWISE == "1")
            //        {
            //            DGRateDetail.Columns[i].Visible = true;
            //        }
            //        else
            //        {
            //            DGRateDetail.Columns[i].Visible = false;
            //        }
            //    }
            //    if (DGRateDetail.Columns[i].HeaderText == "Bonus" || DGRateDetail.Columns[i].HeaderText == "Finisher Rate" || DGRateDetail.Columns[i].HeaderText == "Order Type")
            //    {
            //        if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 42)
            //        {
            //            DGRateDetail.Columns[i].Visible = true;
            //        }
            //        else
            //        {
            //            DGRateDetail.Columns[i].Visible = false;
            //        }
            //    }

            //    //if (DGRateDetail.Columns[i].HeaderText == "Emp Name")
            //    //{
            //    //    if (Session["varMasterCompanyIDForERP"].ToString() == "27")
            //    //    {
            //    //        DGRateDetail.Columns[i].Visible = true;
            //    //    }
            //    //    else
            //    //    {
            //    //        DGRateDetail.Columns[i].Visible = false;
            //    //    }
            //    //}
            //}
        }
    }
    protected void DGRateDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        int r = Convert.ToInt32(DGRateDetail.SelectedIndex.ToString());

        lblMessage.Text = "";


        btnsave.Text = "Save";

        string id = DGRateDetail.SelectedDataKey.Value.ToString();
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
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_BindPurchaseRateMaster", param);

            if (ds.Tables[0].Rows.Count == 1)
            {
                ddJob.SelectedValue = ds.Tables[0].Rows[0]["ProcessId"].ToString();

                BindCategoryMaster();
                DDCategory.SelectedValue = ds.Tables[0].Rows[0]["Categoryid"].ToString();

                BindItemName();
                DDItemName.SelectedValue = ds.Tables[0].Rows[0]["ItemId"].ToString();

                BindQuality();
                ddquality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();


                DDPurchaseVendor.SelectedValue = ds.Tables[0].Rows[0]["EmpId"].ToString();

                txtrate.Text = ds.Tables[0].Rows[0]["PurchaseRate"].ToString();
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
        btnsave.Text = "Update";
    }

    protected void btnpreview_Click(object sender, EventArgs e)
    {

        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@FromDate", txtFromDate.Text);
        param[1] = new SqlParameter("@ToDate", txtToDate.Text);
        param[2] = new SqlParameter("@userid", Session["varuserid"]);
        param[3] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
        param[4] = new SqlParameter("@CompanyId", DDCompanyName.SelectedValue);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetPurchaseRateReportBetweenDate", param);
        if (ds.Tables[0].Rows.Count > 0)
        {            
            Session["rptFileName"] = "Reports/RptPurchaseRateListBetweenDate.rpt";           

            Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseRateListBetweenDate.xsd";
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
        }
    }   
}