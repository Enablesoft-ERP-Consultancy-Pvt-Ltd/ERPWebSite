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

public partial class Masters_Recipe_FrmMakeRecipeForLatexing : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                    From Companyinfo CI(Nolock) 
                    JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                    Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By CI.CompanyName 
                    Select Distinct a.ProcessID, PNM.PROCESS_NAME 
                    From RecipeSlipGenerationMaster a(Nolock) 
                    JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = a.ProcessID 
                    Where a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @" And a.CompanyID = 1";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Plz Select--");
            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedIndex = 1;
                ProcessNameSelectedChanged();
            }

            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            hnissueid.Value = "0";
            DDSlipNo.Focus();
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessNameSelectedChanged();
    }
    protected void ProcessNameSelectedChanged()
    {
        hnissueid.Value = "0";
        string str = @"Select Distinct a.SlipNo, a.SlipNo SlipNo1
                    From RecipeSlipGenerationMaster a(Nolock) 
                    Where a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " And a.CompanyID = " + DDCompanyName.SelectedValue + @" And 
                    a.ProcessID = " + DDProcessName.SelectedValue + " Order By a.SlipNo";

        UtilityModule.ConditionalComboFill(ref DDSlipNo, str, true, "--Plz Select--");
    }
    protected void DDSlipNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        SlipNoSelectedChanged();
    }
    protected void SlipNoSelectedChanged()
    {
        hnissueid.Value = "0";
        string str = @"Select a.ID, RM.Name 
                From RecipeSlipGenerationMaster a(Nolock) 
                JOIN RecipeMaster RM(Nolock) ON RM.ID = a.RecipeMasterID 
                Where a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " And a.CompanyID = " + DDCompanyName.SelectedValue + @" And 
                a.ProcessID = " + DDProcessName.SelectedValue + " And a.SlipNo = " + DDSlipNo.SelectedValue + @" 
                Order By RM.Name";
        UtilityModule.ConditionalComboFill(ref DDRecipeName, str, true, "--Plz Select--");
    }
    protected void DDRecipeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        RecipeNameSelectedIndexChanged();
    }
    protected void Fillgrid()
    {
        //SqlParameter[] param = new SqlParameter[2];
        //param[0] = new SqlParameter("@SlipNo", DDSlipNo.SelectedValue);
        //param[1] = new SqlParameter("@RecipeMasterID", DDRecipeName.SelectedValue);

        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_RecipeRawMaterialIssueDetail", param);

        //DG.DataSource = ds.Tables[0];
        //DG.DataBind();

    }
    protected void RecipeNameSelectedIndexChanged()
    {
        hnissueid.Value = "0";
        string str = @"SELECT RSGC.Item_Finished_ID, 
        Replace(VF.CATEGORY_NAME + ' ' + VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.ShadeColorName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + VF.SizeMtr, '  ', '') [Description] 
        FROM RecipeSlipGenerationMaster RSGM(Nolock) 
        JOIN RecipeSlipGenerationConsumption RSGC(Nolock) ON RSGC.ID = RSGM.ID 
        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = RSGC.Item_Finished_ID 
        JOIN RecipeDetail RD(Nolock) ON RD.ReceipeNameID = RSGM.RecipeMasterID And RD.Item_Finished_ID = RSGC.Item_Finished_ID 
        Where RSGM.CompanyID = " + DDCompanyName.SelectedValue + " And RSGM.ProcessID = " + DDProcessName.SelectedValue + @" And 
        RSGM.SlipNo = " + DDSlipNo.SelectedValue + " And RSGM.ID = " + DDRecipeName.SelectedValue + @" 
        And VF.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By RD.ID 
        Select UnitID, UnitName From Unit(Nolock) Where MasterCompanyID = " + Session["varMasterCompanyIDForERP"];

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        
        UtilityModule.ConditionalComboFillWithDS(ref DDDescription, ds, 0, true, "Select Description");
        UtilityModule.ConditionalComboFillWithDS(ref DDUnit, ds, 1, true, "Select Unit");
        if (DDDescription.Items.Count > 0)
        {
            DDDescription.SelectedIndex = 1;
            DescriptionSelectedIndexChanged();
        }
    }
    protected void DDDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        DescriptionSelectedIndexChanged();
    }

    protected void DescriptionSelectedIndexChanged()
    {
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@SlipNo", DDSlipNo.SelectedValue);
        param[1] = new SqlParameter("@ID", DDRecipeName.SelectedValue);
        param[2] = new SqlParameter("@Item_Finished_ID", DDDescription.SelectedValue);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_DataMakeRecipeForLatexing", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DDUnit.SelectedValue = ds.Tables[0].Rows[0]["UnitID"].ToString();
            TxtConsmpQty.Text = ds.Tables[0].Rows[0]["TotalConsmpQty"].ToString();
            TxtRecQty.Text = ds.Tables[0].Rows[0]["RecQty"].ToString();
            TxtRecBalQty.Text = ds.Tables[0].Rows[0]["RecBalQty"].ToString();
            TxtIssueQty.Text = ds.Tables[0].Rows[0]["IssQty"].ToString();
            TxtBalQty.Text = ds.Tables[0].Rows[0]["IssBalQty"].ToString();
            TxtIssueQty.Focus();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //Select ID, CompanyID, ProcessID, SlipNo, RecipeMasterID, IssueNo, IssueDate, DateAdded, UserID, MasterCompanyID 
            //Select DetailID, MasterID, Item_Finished_ID, UnitID, Qty
            SqlParameter[] param = new SqlParameter[13];
            param[0] = new SqlParameter("@ID", SqlDbType.Int);
            param[0].Value = hnissueid.Value;
            param[0].Direction = ParameterDirection.InputOutput;
            param[1] = new SqlParameter("@CompanyID", DDCompanyName.SelectedValue);
            param[2] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
            param[3] = new SqlParameter("@SlipID", DDSlipNo.SelectedValue);
            param[4] = new SqlParameter("@RecipeMasterID", DDRecipeName.SelectedValue);
            param[5] = new SqlParameter("@IssueNo", txtissueno.Text);
            param[6] = new SqlParameter("@IssueDate", txtissuedate.Text);
            param[7] = new SqlParameter("@UserID", Session["varuserid"]);
            param[8] = new SqlParameter("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
            param[9] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[9].Direction = ParameterDirection.Output;
            param[10] = new SqlParameter("@Item_Finished_ID", DDDescription.SelectedValue);
            param[11] = new SqlParameter("@UnitID", DDUnit.SelectedValue);
            param[12] = new SqlParameter("@Qty", TxtQty.Text);

            ///**********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SaveRecipeMakeForLatexing", param);
            //*******************

            ViewState["ID"] = param[0].Value.ToString();
            txtissueno.Text = param[0].Value.ToString();
            hnissueid.Value = param[0].Value.ToString();
            TxtQty.Text = "";
            DDDescription.SelectedIndex = 0;
            DescriptionSelectedIndexChanged();
            DDDescription.Focus();
            Tran.Commit();
            if (param[9].Value.ToString() != "")
            {
                lblmessage.Text = param[9].Value.ToString();
            }
            else
            {
                lblmessage.Text = "DATA SAVED SUCCESSFULLY.";
                Fillgrid();
                FillissueGrid();
            }
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void FillissueGrid()
    {
        string str = @"Select b.DetailID gvdetail, Replace(VF.CATEGORY_NAME + ' / ' + VF.ITEM_NAME + ' / ' + VF.QUALITYNAME + ' / ' + VF.ShadeColorName + ' / ' + VF.DESIGNNAME + ' / ' + VF.COLORNAME + ' / ' + VF.SHAPENAME + ' / ' + 
            	CASE WHEN b.UnitID = 1 THEN VF.SIZEMTR ELSE CASE WHEN b.UnitID = 6 THEN VF.SIZEINCH ELSE SIZEFT END END, '/  ', '') ItemDescription, b.Qty 
                From RecipeMakeForLatexingMaster a(Nolock)
                JOIN RecipeMakeForLatexingDetail b(Nolock) ON b.MasterID = a.ID 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.Item_Finished_ID 
                Where a.ID = " + hnissueid.Value;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@Prmid", hnissueid.Value);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_WeftIssuereport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptWeftissueonLoom.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptWeftissueonLoom.xsd";
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
    protected void DDissueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnissueid.Value = DDissueno.SelectedValue;
        FillissueGrid();
    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}
        //SqlTransaction Tran = con.BeginTransaction();
        //try
        //{
        //    Label lblprmid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprmid");
        //    Label lblprtid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprtid");
        //    Label lblprorderid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprorderid");
        //    Label lblprocessid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprocessid");

        //    SqlParameter[] param = new SqlParameter[5];
        //    param[0] = new SqlParameter("@prmid", lblprmid.Text);
        //    param[1] = new SqlParameter("@prtid", lblprtid.Text);
        //    param[2] = new SqlParameter("@prorderid", lblprorderid.Text);
        //    param[3] = new SqlParameter("@Processid", lblprocessid.Text);
        //    param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
        //    param[4].Direction = ParameterDirection.Output;
        //    //****************
        //    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_deleteweftissue", param);
        //    lblmessage.Text = param[4].Value.ToString();
        //    Tran.Commit();
        //    FillissueGrid();
        //    //***************
        //}
        //catch (Exception ex)
        //{
        //    lblmessage.Text = ex.Message;
        //    Tran.Rollback();
        //}
        //finally
        //{
        //    con.Dispose();
        //    con.Close();
        //}
    }
}
