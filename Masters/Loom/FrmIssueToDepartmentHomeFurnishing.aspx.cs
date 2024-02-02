using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;

public partial class Masters_Loom_FrmIssueToDepartmentHomeFurnishing : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName 
                    from Companyinfo CI(nolock)
                    JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                    Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName
                    Select Distinct CI.CustomerId, CI.CustomerCode 
                    From OrderMaster OM(Nolock)
                    JOIN CustomerInfo CI(Nolock) ON CI.CustomerId = OM.CustomerId 
                    JOIN OrderDetail OD(Nolock) ON OD.OrderID = OM.OrderID 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OD.Item_Finished_Id And VF.PoufTypeCategory = 1 
                    Where OM.CompanyID = " + Session["CurrentWorkingCompanyID"] + @" 
                    Order By CI.CustomerCode 
                    Select UnitId,UnitName From Unit Where Unitid in(1,2)
                    Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @"
                    Select DepartmentId, DepartmentName From Department (Nolock) Where MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + " Order By DepartmentName ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddunit, ds, 2, true, "--Plz Select--");

            if (ddunit.Items.Count > 0)
            {
                ddunit.SelectedValue = "2";
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 3, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDDepartmentName, ds, 4, true, "--Plz Select--");

            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttargetdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            hnissueorderid.Value = "0";
        }
    }

    protected void DDcustcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"Select Distinct OM.OrderID, OM.CustomerOrderNo 
            From OrderMaster OM(Nolock) 
            JOIN OrderDetail OD(Nolock) ON OD.OrderID = OM.OrderID 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OD.Item_Finished_Id And VF.PoufTypeCategory = 1 
            Where OM.Status = 0 And OM.CompanyID = " + DDcompany.SelectedValue + " AND OM.CustomerId = " + DDcustcode.SelectedValue + @" 
            Order By OM.CustomerOrderNo ";
        UtilityModule.ConditionalComboFill(ref DDorderNo, str, true, "--Plz Select--");
    }

    protected void DDorderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDorderNo.SelectedIndex > 0)
        {
            FillissueDetails();
        }
    }
    protected void FillissueDetails()
    {
        SqlParameter[] array = new SqlParameter[4];
        array[0] = new SqlParameter("@OrderID", SqlDbType.Int);
        array[1] = new SqlParameter("@UnitID", SqlDbType.Int);
        array[2] = new SqlParameter("@ProcessID", SqlDbType.Int);
        array[3] = new SqlParameter("@Type", SqlDbType.Int);

        array[0].Value = DDorderNo.SelectedValue;
        array[1].Value = ddunit.SelectedValue;
        array[2].Value = 1;
        array[3].Value = 1;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GET_DEPARTMENYISSUEORDERDETAIL", array);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DG.DataSource = ds.Tables[0];
            DG.DataBind();
        }
        else
        {
            DG.DataSource = ds.Tables[0];
            DG.DataBind();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        string Str = "";
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox TxtIssueQty = ((TextBox)DG.Rows[i].FindControl("TxtIssueQty"));
            if (Chkboxitem.Checked == true && (TxtIssueQty.Text != "" && TxtIssueQty.Text != "0"))
            {
                Label LblPendingQty = ((Label)DG.Rows[i].FindControl("LblPendingQty"));
                Label lblorderid = ((Label)DG.Rows[i].FindControl("lblorderid"));
                Label lblitemfinishedid = ((Label)DG.Rows[i].FindControl("lblitemfinishedid"));
                Label lblOrderDetailDetail_FinishedID = ((Label)DG.Rows[i].FindControl("lblOrderDetailDetail_FinishedID"));
                if (Convert.ToInt32(TxtIssueQty.Text) > Convert.ToInt32(LblPendingQty.Text))
                {
                    lblmessage.Text = "Qty can not be more than pending qty";
                    return;
                }
                else
                {
                    if (Str == "")
                    {
                        Str = lblorderid.Text + "|" + lblitemfinishedid.Text + "|" + lblOrderDetailDetail_FinishedID.Text + "|" + ddunit.SelectedValue + "|" + DDCalType.SelectedValue + "|" + TxtIssueQty.Text + "~";
                    }
                    else
                    {
                        Str = Str + lblorderid.Text + "|" + lblitemfinishedid.Text + "|" + lblOrderDetailDetail_FinishedID.Text + "|" + ddunit.SelectedValue + "|" + DDCalType.SelectedValue + "|" + TxtIssueQty.Text + "~";
                    }
                }
            }
        }
        if (Str != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("Pro_SaveIssueToDepartmentHomeFurnishing", con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30000;
                cmd.Parameters.Add("@issueorderid", SqlDbType.Int);
                cmd.Parameters["@issueorderid"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@issueorderid"].Value = hnissueorderid.Value;
                cmd.Parameters.AddWithValue("@Companyid", DDcompany.SelectedValue);
                cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);
                cmd.Parameters.AddWithValue("@ProcessID", 1);
                cmd.Parameters.AddWithValue("@DepartmentID", DDDepartmentName.SelectedValue);
                cmd.Parameters.Add("@IssueNo", SqlDbType.VarChar, 100);
                cmd.Parameters["@IssueNo"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Issuedate", txtissuedate.Text);
                cmd.Parameters.AddWithValue("@Targetdate", txttargetdate.Text);
                cmd.Parameters.AddWithValue("@DetailData", Str);
                cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
                cmd.Parameters.AddWithValue("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
                cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@OrderID", DDorderNo.SelectedValue);

                cmd.ExecuteNonQuery();
                if (cmd.Parameters["@msg"].Value.ToString() != "") //IF DATA NOT SAVED 
                {
                    lblmessage.Text = cmd.Parameters["@msg"].Value.ToString();
                    Tran.Rollback();
                }
                else
                {
                    lblmessage.Text = "Data Saved Successfully.";
                    Tran.Commit();
                    TxtIssueNo.Text = cmd.Parameters["@IssueNo"].Value.ToString(); //param[5].Value.ToString();
                    hnissueorderid.Value = cmd.Parameters["@issueorderid"].Value.ToString();// param[0].Value.ToString();
                    FillGrid();
                    Refreshcontrol();
                }
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblmessage.Text = ex.Message;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
        }
    }
    protected void Refreshcontrol()
    {
        DDcustcode.SelectedIndex = -1;
        DDorderNo.SelectedIndex = -1;
        DG.DataSource = null;
        DG.DataBind();
    }
    protected void FillGrid()
    {
        string str = @"Select PD.IssueDetailId Issue_Detail_Id, PM.issueorderid, VF.ITEM_NAME + ', ' + VF.QualityName + ', ' + VF.DesignName + ', ' + VF.ColorName + ', ' + VF.ShapeName + ', ' + 
            Case When " + ddunit.SelectedValue + " = 1 Then VF.SizeMtr Else Case When " + ddunit.SelectedValue + @" = 6 Then VF.SizeInch Else VF.SizeFt End End + ', ' + VF.ShadeColorName ItemDescription, 
            Round(Area * Qty, 4) Area, Rate, Qty, Amount, REPLACE(CONVERT(nvarchar(11), PM.IssueDate, 106), ' ', '-') AssignDate, REPLACE(CONVERT(nvarchar(11), PM.RequiredDate, 106), ' ', '-') ReqbyDate,
            PM.UnitID, PM.CalType, IsNull(PM.IssueNo, '') IssueNo 
            From ProcessIssueToHomeFurnishingDepartmentMaster PM(Nolock)
            JOIN ProcessIssueToHomeFurnishingDepartmentDetail PD(Nolock) ON PD.IssueOrderid = PM.IssueOrderid 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = PD.OrderDetailDetail_FinishedID 
            Where PM.IssueOrderid = " + hnissueorderid.Value + " And PM.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + " Order By PD.IssueDetailId Desc";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGOrderdetail.DataSource = ds.Tables[0];
        DGOrderdetail.DataBind();


        if (chkEdit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtissuedate.Text = ds.Tables[0].Rows[0]["assigndate"].ToString();
                txttargetdate.Text = ds.Tables[0].Rows[0]["Reqbydate"].ToString();
                TxtIssueNo.Text = ds.Tables[0].Rows[0]["IssueNo"].ToString();
                if (ddunit.Items.FindByValue(ds.Tables[0].Rows[0]["unitid"].ToString()) != null)
                {
                    ddunit.SelectedValue = ds.Tables[0].Rows[0]["unitid"].ToString();
                }
                if (DDCalType.Items.FindByValue(ds.Tables[0].Rows[0]["CalType"].ToString()) != null)
                {
                    DDCalType.SelectedValue = ds.Tables[0].Rows[0]["CalType"].ToString();
                }
            }
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        Report();
    }
    private void Report()
    {
        DataSet ds = new DataSet();

        SqlParameter[] array = new SqlParameter[4];
        array[0] = new SqlParameter("@IssueOrderId", hnissueorderid.Value);
        array[1] = new SqlParameter("@ProcessId", 1);
        array[2] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
        array[3] = new SqlParameter("@REPORTTYPE", 6);

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForProductionOrder", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWisechampo.rpt";
            Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWiseStockChampo.rpt";

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptProductionOrderLoomWise.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }

    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
        EditSelectedChange();

        if (Session["usertype"].ToString() != "1" && variable.VarOnlyPreviewButtonShowOnAllEditForm == "1")
        {
            btnsave.Visible = false;
            DGOrderdetail.Columns[10].Visible = false;
            DGOrderdetail.Columns[11].Visible = false;
        }
    }
    protected void EditSelectedChange()
    {
        TxtIssueNoEdit.Text = "";
        ddunit.Enabled = true;
        if (chkEdit.Checked == true)
        {
            TDIssueNo.Visible = true;
            TDIssueNoEdit.Visible = true;
            hnissueorderid.Value = "0";
            ddunit.Enabled = false;
            btnsave.Visible = false;
            BtnUpdateConsumption.Visible = true;
            ChkForDyeingConsumption.Visible = true;
        }
        else
        {
            btnsave.Visible = true;
            TDIssueNoEdit.Visible = false;
            TDIssueNo.Visible = false;
            hnissueorderid.Value = "0";
            BtnUpdateConsumption.Visible = false;
            ChkForDyeingConsumption.Visible = false;
        }
        DDIssueNo.Items.Clear();
        TxtIssueNo.Text = "";
        DGOrderdetail.DataSource = null;
        DGOrderdetail.DataBind();
        DG.DataSource = null;
        DG.DataBind();
    }

    protected void DDIssueNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnissueorderid.Value = DDIssueNo.SelectedValue;
        FillGrid();
    }
    protected void DGOrderdetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGOrderdetail.EditIndex = e.NewEditIndex;
        FillGrid();
    }
    protected void DGOrderdetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGOrderdetail.EditIndex = -1;
        FillGrid();
    }
    protected void DGOrderdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblissueorderid = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblissueorderid");
            Label lblissuedetailid = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblissuedetailid");
            Label lblhqty = (Label)DGOrderdetail.Rows[e.RowIndex].FindControl("lblhqty");
            TextBox txtqty = (TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtqty");
            TextBox txtrategrid = (TextBox)DGOrderdetail.Rows[e.RowIndex].FindControl("txtrategrid");

            SqlParameter[] param = new SqlParameter[10];
            param[0] = new SqlParameter("@issueorderid", lblissueorderid.Text);
            param[1] = new SqlParameter("@issuedetailid", lblissuedetailid.Text);
            param[2] = new SqlParameter("@qty", txtqty.Text == "" ? "0" : txtqty.Text);
            param[3] = new SqlParameter("@Hqty", lblhqty.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@Userid", Session["varuserid"]);
            param[6] = new SqlParameter("@rate", txtrategrid.Text == "" ? "0" : txtrategrid.Text);

            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateIssueToDepartmentHomeFurnishing", param);
            //*************
            lblmessage.Text = param[4].Value.ToString();
            Tran.Commit();
            DGOrderdetail.EditIndex = -1;
            FillGrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void lnkdelClick(object sender, EventArgs e)
    {
        lblmessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            LinkButton lnkdel = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)lnkdel.NamingContainer;

            Label lblissueorderid = (Label)gvr.FindControl("lblissueorderid");
            Label lblissuedetailid = (Label)gvr.FindControl("lblissuedetailid");
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@issueorderid", lblissueorderid.Text);
            param[1] = new SqlParameter("@IssueDetailId", lblissuedetailid.Text);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@MastercompanyId", Session["varcompanyNo"]);
            param[4] = new SqlParameter("@Userid", Session["varuserid"]);
            //********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteIssueToDepartmentHomeFurnishing", param);
            lblmessage.Text = param[2].Value.ToString();
            Tran.Commit();
            FillGrid();
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
    protected void ddunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        DG.DataSource = null;
        DG.DataBind();
    }
    protected void DGOrderdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtqty = (TextBox)e.Row.FindControl("txtqty");
            if (txtqty != null)
            {

            }
            if (Convert.ToInt16(Session["usertype"]) > 2)
            {
                DGOrderdetail.Columns[10].Visible = false;
                DGOrderdetail.Columns[11].Visible = false;
            }
        }
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < DG.Columns.Count; i++)
            {
                TextBox TxtIssueQty = (TextBox)e.Row.FindControl("TxtIssueQty");
                TxtIssueQty.Text = "";
            }
        }
    }
    protected void TxtIssueNoEdit_TextChanged(object sender, EventArgs e)
    {
        string str = @"Select PM.IssueOrderID, DepartmentID 
            From ProcessIssueToHomeFurnishingDepartmentMaster PM(Nolock)
            Where PM.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" And CompanyID = " + DDcompany.SelectedValue + @" And 
            BranchID = " + DDBranchName.SelectedValue + " And ProcessID = 1 And IssueNo = '" + TxtIssueNoEdit.Text + "'";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DDDepartmentName.SelectedValue = ds.Tables[0].Rows[0]["DepartmentID"].ToString();
            if (chkEdit.Checked == true)
            {
                DepartmentNameSelectedIndexChanged();
                DDIssueNo.SelectedValue = ds.Tables[0].Rows[0]["IssueOrderID"].ToString();
                hnissueorderid.Value = DDIssueNo.SelectedValue;
                FillGrid();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Please Check Issue No!');", true);
            DDDepartmentName.SelectedIndex = 0;
            DDIssueNo.Items.Clear();
            TxtIssueNoEdit.Text = "";
        }
    }
    protected void DDDepartmentName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DepartmentNameSelectedIndexChanged();
    }
    private void DepartmentNameSelectedIndexChanged()
    {
        if (chkEdit.Checked == true)
        {
            string str = @"Select IssueOrderID, IssueNo 
                From ProcessIssueToHomeFurnishingDepartmentMaster (Nolock) 
                Where CompanyID = " + DDcompany.SelectedValue + " And ProcessID = 1 And BranchID = " + DDBranchName.SelectedValue + @" And 
                DepartmentID = " + DDDepartmentName.SelectedValue + " And MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @" Order By IssueOrderID Desc ";

            UtilityModule.ConditionalComboFill(ref DDIssueNo, str, true, "--Plz Select--");

            DG.DataSource = null;
            DG.DataBind();
        }
    }
    protected void BtnUpdateConsumption_Click(object sender, EventArgs e)
    {
        if (DDIssueNo.SelectedIndex > 0)
        {

            lblmessage.Text = "";
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@IssueOrderID", DDIssueNo.SelectedValue);
                if (ChkForDyeingConsumption.Checked == true)
                {
                    param[1] = new SqlParameter("@ProcessID", 5);
                }
                else
                {
                    param[1] = new SqlParameter("@ProcessID", 1);
                }
                param[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                param[2].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateConsumptionIssueToDepartmentHomeFurnishing", param);
                if (param[2].Value.ToString() != "")
                {
                    lblmessage.Text = param[2].Value.ToString();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Consumption update sucessfully');", true);
                }
                Tran.Commit();
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Please select issue number');", true);
        }
    }
}