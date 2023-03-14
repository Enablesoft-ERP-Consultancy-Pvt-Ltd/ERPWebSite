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

public partial class Masters_Recipe_FrmRecipeRawMaterialIssue : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                    From Companyinfo CI(Nolock) 
                    JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                    Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By CI.CompanyName 
                    Select Distinct a.ProcessID, PNM.PROCESS_NAME 
                    From RecipeSlipGenerationMaster a(Nolock) 
                    JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = a.ProcessID 
                    Where a.MasterCompanyID = " + Session["varCompanyId"] + @" And a.CompanyID = 1";

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
                DDProcessName.SelectedValue = "1";
                ProcessNameSelectedChanged();
            }

            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
            }
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessNameSelectedChanged();
    }
    protected void ProcessNameSelectedChanged()
    {
        string str = @"Select Distinct a.SlipNo, a.SlipNo SlipNo1
                    From RecipeSlipGenerationMaster a(Nolock) 
                    Where a.MasterCompanyID = " + Session["varCompanyId"] + " And a.CompanyID = " + DDCompanyName.SelectedValue + @" And 
                    a.ProcessID = " + DDProcessName.SelectedValue + " Order By a.SlipNo";
        if (chkEdit.Checked == true)
        {
            str = @"Select Distinct a.SlipNo, a.SlipNo SlipNo1
                    From RecipeSlipGenerationMaster a(Nolock) 
                    JOIN ProcessRawMaster PRM(Nolock) ON PRM.RecipeSlipGenerationMasterSlipNo = a.SlipNo 
                    Where PRM.TypeFlag = 0 And a.MasterCompanyID = " + Session["varCompanyId"] + " And a.CompanyID = " + DDCompanyName.SelectedValue + @" And 
                    a.ProcessID = " + DDProcessName.SelectedValue + " Order By a.SlipNo";
        }

        UtilityModule.ConditionalComboFill(ref DDSlipNo, str, true, "--Plz Select--");
    }
    protected void DDSlipNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        SlipNoSelectedChanged();
    }
    protected void SlipNoSelectedChanged()
    {
        string str = @"Select a.ID, RM.Name 
                From RecipeSlipGenerationMaster a(Nolock) 
                JOIN RecipeMaster RM(Nolock) ON RM.ID = a.RecipeMasterID 
                Where a.MasterCompanyID = " + Session["varCompanyId"] + " And a.CompanyID = " + DDCompanyName.SelectedValue + @" And 
                a.ProcessID = " + DDProcessName.SelectedValue + " And a.SlipNo = " + DDSlipNo.SelectedValue + @" 
                Order By RM.Name";

        if (chkEdit.Checked == true)
        {
            str = @"Select Distinct a.ID, RM.Name 
                From RecipeSlipGenerationMaster a(Nolock) 
                JOIN ProcessRawMaster PRM(Nolock) ON PRM.RecipeSlipGenerationMasterID = a.ID 
                JOIN RecipeMaster RM(Nolock) ON RM.ID = a.RecipeMasterID 
                Where PRM.TypeFlag = 0 And a.MasterCompanyID = " + Session["varCompanyId"] + " And a.CompanyID = " + DDCompanyName.SelectedValue + @" And 
                a.ProcessID = " + DDProcessName.SelectedValue + " And a.SlipNo = " + DDSlipNo.SelectedValue + @" 
                Order By RM.Name";
        }
        UtilityModule.ConditionalComboFill(ref DDRecipeName, str, true, "--Plz Select--");
    }
    protected void DDRecipeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillgrid();
        if (chkEdit.Checked == true)
        {
            string str = @"Select PRM.PRMid, PRM.ChalanNo 
                From ProcessRawMaster PRM(Nolock) 
                Where PRM.TypeFlag = 0 And PRM.MasterCompanyId = " + Session["varCompanyId"] + " And IsNull(RecipeSlipGenerationMasterSlipNo, 0) = " + DDSlipNo.SelectedValue + @" And 
                RecipeSlipGenerationMasterID = " + DDRecipeName.SelectedValue + @" 
                Order By PRM.ChalanNo ";
            UtilityModule.ConditionalComboFill(ref DDissueno, str, true, "--Plz Select--");
        }
    }
    protected void Fillgrid()
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@SlipNo", DDSlipNo.SelectedValue);
        param[1] = new SqlParameter("@RecipeMasterID", DDRecipeName.SelectedValue);
        
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_RecipeRawMaterialIssueDetail", param);

        DG.DataSource = ds.Tables[0];
        DG.DataBind();

    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            for (int i = 0; i < DG.Columns.Count; i++)
            {
                if (variable.VarBINNOWISE == "1")
                {
                    if (DG.Columns[i].HeaderText == "BinNo")
                    {
                        DG.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (DG.Columns[i].HeaderText == "BinNo")
                    {
                        DG.Columns[i].Visible = false;
                    }
                }
            }
            DropDownList DDGodown = ((DropDownList)e.Row.FindControl("DDGodown"));
            string str = @"select GoDownID,GodownName from GodownMaster order by GodownName
                           select godownid From Modulewisegodown Where ModuleName='" + Page.Title + "'";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDGodown, ds, 0, true, "--Plz Select--");

            if (hngodownid.Value == "0")
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (DDGodown.Items.FindByValue(ds.Tables[1].Rows[0]["godownid"].ToString()) != null)
                    {
                        DDGodown.SelectedValue = ds.Tables[1].Rows[0]["godownid"].ToString();
                    }
                }
                else
                {
                    if (DDGodown.Items.Count > 0)
                    {
                        DDGodown.SelectedIndex = 1;
                    }
                }
            }
            else
            {
                if (DDGodown.Items.FindByValue(hngodownid.Value) != null)
                {
                    DDGodown.SelectedValue = hngodownid.Value;
                }
            }
            DDgodown_SelectedIndexChanged(DDGodown, new EventArgs());
            ds.Dispose();
        }
    }
    protected void DDgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlgodown = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlgodown.Parent.Parent;
        Label Item_Finished_ID = ((Label)row.FindControl("lblItem_Finished_ID"));

        if (variable.VarBINNOWISE == "1")
        {
            DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));
            string str = "select Distinct S.BinNo,S.BinNo From Stock S(Nolock) Where S.companyid = " + DDCompanyName.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Item_Finished_ID.Text + " and S.Qtyinhand>0";
            UtilityModule.ConditionalComboFill(ref DDBinNo, str, true, "--Plz Select--");
        }
        else
        {
            int index = row.RowIndex;
            DropDownList ddLotno = ((DropDownList)row.FindControl("DDLotNo"));
            string str = "select Distinct S.Lotno,S.Lotno from Stock S Where S.companyid=" + DDCompanyName.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Item_Finished_ID.Text + " and S.Qtyinhand>0";
            UtilityModule.ConditionalComboFill(ref ddLotno, str, true, "--Plz Select--");

            if (ddLotno.Items.Count > 0)
            {
                ddLotno.SelectedIndex = 1;
                DDLotno_SelectedIndexChanged(ddLotno, e);
            }
        }
    }
    protected void DDBinNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList DDBinNo = (DropDownList)sender;
        GridViewRow row = (GridViewRow)DDBinNo.Parent.Parent;
        Label Item_Finished_ID = ((Label)row.FindControl("lblItem_Finished_ID"));
        DropDownList ddlgodown = ((DropDownList)row.FindControl("DDGodown"));

        DropDownList ddLotno = ((DropDownList)row.FindControl("DDLotNo"));
        string str = "select Distinct S.Lotno,S.Lotno from Stock S Where S.companyid=" + DDCompanyName.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Item_Finished_ID.Text + " and S.Qtyinhand>0";
        if (variable.VarBINNOWISE == "1")
        {
            str = str + " and BinNo='" + DDBinNo.SelectedItem.Text + "'";
        }
        UtilityModule.ConditionalComboFill(ref ddLotno, str, true, "--Plz Select--");

        if (ddLotno.Items.Count > 0)
        {
            ddLotno.SelectedIndex = 1;
            DDLotno_SelectedIndexChanged(ddLotno, e);
        }
    }
    protected void DDLotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlLotno = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlLotno.Parent.Parent;
        int index = row.RowIndex;

        Label Item_Finished_ID = (Label)row.FindControl("lblItem_Finished_ID");
        DropDownList DDTagNo = ((DropDownList)row.FindControl("DDTagNo"));
        DropDownList ddlgodown = ((DropDownList)row.FindControl("DDgodown"));
        DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));

        string str = "select Distinct S.TagNo,S.Tagno from Stock S Where S.companyid=" + DDCompanyName.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Item_Finished_ID.Text + " and S.Lotno='" + ddlLotno.Text + "' and S.Qtyinhand>0";
        if (variable.VarBINNOWISE == "1")
        {
            str = str + " and BinNo='" + DDBinNo.SelectedItem.Text + "'";
        }
        UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "--Plz Select--");

        if (DDTagNo.Items.Count > 0)
        {
            DDTagNo.SelectedIndex = 1;
            DDTagno_SelectedIndexChanged(DDTagNo, e);
        }
    }
    protected void DDTagno_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddTagno = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddTagno.Parent.Parent;
        int index = row.RowIndex;
        int Item_Finished_ID = Convert.ToInt32(((Label)row.FindControl("lblItem_Finished_ID")).Text);
        Label lblstockqty = ((Label)row.FindControl("lblstockqty"));
        DropDownList ddgodown = ((DropDownList)row.FindControl("DDgodown"));
        DropDownList ddlotno = ((DropDownList)row.FindControl("DDLotNo"));
        DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));

        Double StockQty = UtilityModule.getstockQty(DDCompanyName.SelectedValue, ddgodown.SelectedValue, ddlotno.Text, Item_Finished_ID, ddTagno.Text, BinNo: (variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : ""));
        lblstockqty.Text = StockQty.ToString();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        string DetailData = "", VarBinNo = "";

        for (int i = 0; i < DG.Rows.Count; i++)
        {
            VarBinNo = "";
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtissueqty = ((TextBox)DG.Rows[i].FindControl("txtissueqty"));
            DropDownList DDGodown = ((DropDownList)DG.Rows[i].FindControl("DDGodown"));
            DropDownList DDLotNo = ((DropDownList)DG.Rows[i].FindControl("DDLotNo"));
            DropDownList DDTagNo = ((DropDownList)DG.Rows[i].FindControl("DDTagNo"));
            DropDownList DDBinNo = ((DropDownList)DG.Rows[i].FindControl("DDBinNo"));

            if (Chkboxitem.Checked == true && (txtissueqty.Text != "") && DDGodown.SelectedIndex > 0 && DDLotNo.SelectedIndex > 0 && DDTagNo.SelectedIndex > 0)
            {
                if (variable.VarBINNOWISE == "1")
                {
                    VarBinNo = DDBinNo.SelectedItem.Text;
                }
                else
                {
                    VarBinNo = "";
                }
                Label Item_Finished_ID = ((Label)DG.Rows[i].FindControl("lblItem_Finished_ID"));
                Label lblUnitID = ((Label)DG.Rows[i].FindControl("lblUnitID"));
                string Lotno = DDLotNo.Text;
                string TagNo = DDTagNo.Text;
                Label lblconsmpqty = ((Label)DG.Rows[i].FindControl("lblconsmpqty"));

                DetailData = DetailData + Item_Finished_ID.Text + '|' + lblUnitID.Text + '|' + DDGodown.SelectedValue + '|' + Lotno + '|' + TagNo + '|' + txtissueqty.Text + '|';
                DetailData = DetailData + lblconsmpqty.Text + '|' + VarBinNo + '~';
            }
        }
        
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        if (DetailData != "")
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[13];
                param[0] = new SqlParameter("@PrmId", SqlDbType.Int);
                param[0].Value = 0;
                param[0].Direction = ParameterDirection.InputOutput;
                param[1] = new SqlParameter("@CompanyID", DDCompanyName.SelectedValue);
                param[2] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
                param[3] = new SqlParameter("@SlipID", DDSlipNo.SelectedValue);
                param[4] = new SqlParameter("@RecipeMasterID", DDRecipeName.SelectedValue);
                param[5] = new SqlParameter("@IssueNo", txtissueno.Text);
                param[6] = new SqlParameter("@IssueDate", txtissuedate.Text);
                param[7] = new SqlParameter("@EWayBillNo", TxtEWayBillNo.Text);
                param[8] = new SqlParameter("@TranType", 0);
                param[9] = new SqlParameter("@DetailData", DetailData);
                param[10] = new SqlParameter("@UserID", Session["varuserid"]);
                param[11] = new SqlParameter("@MasterCompanyID", Session["varcompanyid"]);
                param[12] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[12].Direction = ParameterDirection.Output;
                
                ///**********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SaveRecipeSlipGenerationRawMaterialIssue", param);
                //*******************
                ViewState["reportid"] = param[0].Value.ToString();
                txtissueno.Text = param[0].Value.ToString();
                hnissueid.Value = param[0].Value.ToString();
                Tran.Commit();
                if (param[12].Value.ToString() != "")
                {
                    lblmessage.Text = param[12].Value.ToString();
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
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
        }
    }
    protected void FillissueGrid()
    {
        string str = @"select dbo.F_getItemDescription(PT.Finishedid,PT.flagsize) as ItemDescription,
                    PT.Lotno,PT.TagNo,PT.IssueQuantity,PM.chalanNo,Replace(CONVERT(nvarchar(11),PM.date,106),' ','-') as IssueDate,
                    PM.prmid,PT.Prtid,PM.prorderid,PM.processid, isnull(PM.EWayBillNo,'') as EWayBillNo
                    From ProcessRawMaster PM 
                    join ProcessRawTran PT on PM.PRMid=PT.PRMid 
                    Where PM.TypeFlag = 0 And PM.BeamType=0 And PM.prmid=" + hnissueid.Value;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
        if (chkEdit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtissueno.Text = ds.Tables[0].Rows[0]["chalanno"].ToString();
                txtissuedate.Text = ds.Tables[0].Rows[0]["issuedate"].ToString();
                TxtEWayBillNo.Text = ds.Tables[0].Rows[0]["EWayBillNo"].ToString();
            }
            else
            {
                txtissueno.Text = "";
                txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@Prmid", hnissueid.Value);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_RECIPE_RAWMATERIAL_ISSUE", param);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\RptRecipeRawMaterialIssue.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRecipeRawMaterialIssue.xsd";
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
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
        TDIssueNo.Visible = false;
        DG.DataSource = null;
        DG.DataBind();
        gvdetail.DataSource = null;
        gvdetail.DataBind();
        txtissueno.Text = "";
        txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

        if (chkEdit.Checked == true)
        {
            TDIssueNo.Visible = true;
            DDissueno.SelectedIndex = -1;
        }
    }
    protected void DDissueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnissueid.Value = DDissueno.SelectedValue;
        FillissueGrid();
    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblprmid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprmid");
            Label lblprtid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprtid");
            Label lblprorderid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprorderid");
            Label lblprocessid = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprocessid");

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@prmid", lblprmid.Text);
            param[1] = new SqlParameter("@prtid", lblprtid.Text);
            param[2] = new SqlParameter("@prorderid", lblprorderid.Text);
            param[3] = new SqlParameter("@Processid", lblprocessid.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_deleteweftissue", param);
            lblmessage.Text = param[4].Value.ToString();
            Tran.Commit();
            FillissueGrid();
            //***************
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
}
