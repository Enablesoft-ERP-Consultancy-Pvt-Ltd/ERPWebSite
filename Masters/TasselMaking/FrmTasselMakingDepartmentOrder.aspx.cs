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

public partial class Masters_TasselMaking_FrmTasselMakingDepartmentOrder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            TxtInOtherProcess.Text = "0";
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                        From Companyinfo CI(Nolock)
                        JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                        Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By CompanyName 
                        Select UnitId, UnitName From Unit(Nolock) Where Unitid in(1, 2, 6)";

            if (Convert.ToInt16(Request.QueryString["InOtherProcess"]) == 1)
            {
                TxtInOtherProcess.Text = "1";
                str = str + @" Select Distinct CI.CustomerId, CI.CustomerCode 
                        From OrderMaster OM(Nolock)
                        JOIN CustomerInfo CI(Nolock) ON CI.CustomerId = OM.CustomerId 
                        Where OM.Status = 0 And OM.CompanyID = " + Session["CurrentWorkingCompanyID"] + @"
                        Order By CI.CustomerCode 
                        SELECT PROCESS_NAME_ID, PROCESS_NAME From Process_Name_Master(Nolock) Where Process_Name in ('TUFTING') Order By PROCESS_NAME";
            }
            else
            {
                str = str + @" Select Distinct CI.CustomerId, CI.CustomerCode 
                        From OrderMaster OM(Nolock)
                        JOIN CustomerInfo CI(Nolock) ON CI.CustomerId = OM.CustomerId 
                        JOIN OrderDetail OD(Nolock) ON OD.OrderID = OM.OrderID 
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OD.Item_Finished_Id And VF.CUSHIONTYPEITEM = 1 
                        Where OM.Status = 0 And OM.CompanyID = " + Session["CurrentWorkingCompanyID"] + @"
                        Order By CI.CustomerCode 
                        SELECT PROCESS_NAME_ID, PROCESS_NAME From Process_Name_Master(Nolock) Where Process_Name in ('TASSEL MAKING', 'POM-POM MAKING', 'BRAIDING', 'LACE MACKING', 'FRINGES MAKING') Order By PROCESS_NAME ";
                        
            }
            str = str + @" Select ID, BranchName 
                        From BRANCHMASTER BM(nolock) 
                        JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                        Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];
                        
            str = str + @" Select DepartmentId, DepartmentName From Department (Nolock) Where MasterCompanyId = " + Session["varCompanyId"] + " Order By DepartmentName ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref ddunit, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds, 2, true, "--Plz Select--");

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 4, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 3, false, "");
            if (DDProcessName.Items.Count > 0)
            {
                ProcessSelectedChange();
            }
            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttargetdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            DDcaltype.SelectedIndex = 1;

            hnissueorderid.Value = "0";

            if (ddunit.Items.FindByValue(variable.VarDefaultProductionunit) != null)
            {
                ddunit.SelectedValue = variable.VarDefaultProductionunit;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDDepartmentName, ds, 5, true, "--Plz Select--");
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkEdit.Checked == true)
        {
            ProcessSelectedChange();
        }
    }
    protected void ProcessSelectedChange()
    {
        string str = @"Select a.DepartmentID, D.DepartmentName 
        From TasselMakingDepartmentOrderMaster a(Nolock)
        JOIN Department D(Nolock) ON D.DepartmentID = a.DepartmentID 
        Where a.CompanyID = " + DDcompany.SelectedValue + " And a.BranchID = " + DDBranchName.SelectedValue + @" And a.ProcessID = " + DDProcessName.SelectedValue;

        UtilityModule.ConditionalComboFill(ref DDDepartmentName, str, true, "--Plz select--");
    }

    protected void DDDepartmentName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_FolioNo();
    }

    private void Fill_FolioNo()
    {
        string str;
        if (chkEdit.Checked == true)
        {
            str = @"select Distinct PIM.IssueOrderId,PIM.IssueNo  
                From TasselMakingDepartmentOrderMaster PIM(Nolock) 
                Where PIM.CompanyId=" + DDcompany.SelectedValue + " And PIM.BranchID = " + DDBranchName.SelectedValue + @" And 
                PIM.ProcessID = " + DDProcessName.SelectedValue + " And PIM.DepartmentID = " + DDDepartmentName.SelectedValue;

            str = str + " and PIM.Status='Pending'";

            if (txtfolionoedit.Text != "")
            {
                str = str + " and PIM.IssueNo = '" + txtfolionoedit.Text + "'";
            }
            str = str + " order by PIM.IssueOrderId desc";

            UtilityModule.ConditionalComboFill(ref DDFolioNo, str, true, "--Plz Select--");
            if (DDFolioNo.Items.Count > 0)
            {
                DDFolioNo.SelectedIndex = 1;
                FolioNoSelectedChanged();
            }
        }
    }
    protected void DDFolioNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FolioNoSelectedChanged();
    }
    protected void FolioNoSelectedChanged()
    {
        hnissueorderid.Value = DDFolioNo.SelectedValue;
        FillGrid();
    }

    protected void ddunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillissueDetails();
    }
    protected void DDcaltype_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void DDcustcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"Select Distinct OM.OrderID, OM.CustomerOrderNo 
                    From OrderMaster OM(Nolock) 
                    Where OM.Status = 0 And OM.CompanyID = " + DDcompany.SelectedValue + " AND OM.CustomerId = " + DDcustcode.SelectedValue + @" 
                    Order By OM.CustomerOrderNo";
        UtilityModule.ConditionalComboFill(ref DDorderNo, str, true, "--Plz Select--");
    }

    protected void DDorderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillissueDetails();
    }
    protected void FillissueDetails()
    {
        hnissueorderid.Value = "0";
        SqlParameter[] array = new SqlParameter[5];
        array[0] = new SqlParameter("@OrderID", DDorderNo.SelectedValue);
        array[1] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
        array[2] = new SqlParameter("@UnitID", ddunit.SelectedValue);
        array[3] = new SqlParameter("@CalType", DDcaltype.SelectedValue);
        array[4] = new SqlParameter("@MasterCompanyID", Session["varcompanyId"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetProcessWiseOrderDetail", array);

        DG.DataSource = ds.Tables[0];
        DG.DataBind();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        string strOrderDetail = "";

        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtloomqty = ((TextBox)DG.Rows[i].FindControl("txtloomqty"));
            if (Chkboxitem.Checked == true && (txtloomqty.Text != "" && txtloomqty.Text != "0"))
            {
                Label lblitemfinishedid = ((Label)DG.Rows[i].FindControl("lblitemfinishedid"));
                Label lblOrderDetailID = ((Label)DG.Rows[i].FindControl("lblOrderDetailID"));
                TextBox txtrate = ((TextBox)DG.Rows[i].FindControl("txtrate"));
                if (strOrderDetail == "")
                {
                    strOrderDetail = lblitemfinishedid.Text + "|" + txtrate.Text + "|" + txtloomqty.Text + "|" + lblOrderDetailID.Text + "~";
                }
                else
                {
                    strOrderDetail = strOrderDetail + lblitemfinishedid.Text + "|" + txtrate.Text + "|" + txtloomqty.Text + "|" + lblOrderDetailID.Text + "~";
                }
            }
        }
        if (chkEdit.Checked == true)
        {
            hnissueorderid.Value = "0";
        }
        if (strOrderDetail != "")
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("PRO_SAVE_TASSEL_MAKING_DEPARTMENT_ORDER", con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30000;
                cmd.Parameters.Add("@ISSUEORDERID", SqlDbType.Int);
                cmd.Parameters["@ISSUEORDERID"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@ISSUEORDERID"].Value = hnissueorderid.Value;
                cmd.Parameters.AddWithValue("@COMPANYID", DDcompany.SelectedValue);
                cmd.Parameters.AddWithValue("@ProcessID", DDProcessName.SelectedValue);
                cmd.Parameters.AddWithValue("@DepartmentID", DDDepartmentName.SelectedValue);
                cmd.Parameters.Add("@ISSUENO", SqlDbType.VarChar, 100);
                cmd.Parameters["@ISSUENO"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@ISSUEDATE", txtissuedate.Text);
                cmd.Parameters.AddWithValue("@TARGETDATE", txttargetdate.Text);
                cmd.Parameters.AddWithValue("@OrderID", DDorderNo.SelectedValue);
                cmd.Parameters.AddWithValue("@UNITID", ddunit.SelectedValue);
                cmd.Parameters.AddWithValue("@CALTYPE", DDcaltype.SelectedValue);
                cmd.Parameters.AddWithValue("@DetailData", strOrderDetail);
                cmd.Parameters.AddWithValue("@USERID", Session["varuserid"]);
                cmd.Parameters.AddWithValue("@MASTERCOMPANYID", Session["varcompanyid"]);
                cmd.Parameters.AddWithValue("@REMARKS", TxtRemarks.Text.Trim());
                cmd.Parameters.Add("@MSG", SqlDbType.VarChar, 100);
                cmd.Parameters["@MSG"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);

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
                    txtfoliono.Text = cmd.Parameters["@ISSUENO"].Value.ToString(); //param[5].Value.ToString();
                    hnissueorderid.Value = cmd.Parameters["@ISSUEORDERID"].Value.ToString();// param[0].Value.ToString();
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
        DDorderNo.SelectedIndex = -1;
        DG.DataSource = null;
        DG.DataBind();
    }
    protected void FillGrid()
    {
        TxtRemarks.Text = "";
        string str = @"Select a.IssueOrderId, b.IssueDetailId, 
                VF.ITEM_NAME + ' ' + VF.QUALITYNAME + ' ' + VF.DESIGNNAME + ' ' + VF.COLORNAME + ' ' + VF.SHADECOLORNAME + ' ' + VF.SHAPENAME + ' ' + 
                                Case When a.UNITID = 1 Then VF.SizeMtr Else Case When a.UNITID = 6 Then VF.SizeInch Else VF.SizeFt End End ItemDescription, 
                b.Qty, b.Rate, b.Amount, REPLACE(CONVERT(NVARCHAR(11), a.IssueDate, 106), ' ', '-') assigndate, 
                REPLACE(CONVERT(NVARCHAR(11), a.RequiredDate, 106), ' ', '-') Reqbydate, a.IssueNo ChallanNo, a.unitid, a.caltype, b.Remark 
                From TasselMakingDepartmentOrderMaster a(Nolock) 
                JOIN TasselMakingDepartmentOrderDetail b(Nolock) ON b.IssueOrderID = a.IssueOrderID 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.Item_Finished_ID 
                Where a.ISSUEORDERID = " + hnissueorderid.Value + " And a.MasterCompanyId = " + Session["varCompanyId"] + " Order By b.IssueDetailId Desc";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGOrderdetail.DataSource = ds.Tables[0];
        DGOrderdetail.DataBind();
        if (chkEdit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtissuedate.Text = ds.Tables[0].Rows[0]["assigndate"].ToString();
                txttargetdate.Text = ds.Tables[0].Rows[0]["Reqbydate"].ToString();
                txtfoliono.Text = ds.Tables[0].Rows[0]["ChallanNo"].ToString();
                if (ddunit.Items.FindByValue(ds.Tables[0].Rows[0]["unitid"].ToString()) != null)
                {
                    ddunit.SelectedValue = ds.Tables[0].Rows[0]["unitid"].ToString();
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
        SqlParameter[] array = new SqlParameter[4];
        array[0] = new SqlParameter("@IssueOrderId", hnissueorderid.Value);
        array[1] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
        array[2] = new SqlParameter("@MasterCompanyId", Session["varcompanyId"]);
        array[3] = new SqlParameter("@ReportType", 5);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForProductionOrder", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptProductionOrderLoomWiseStockChampo.rpt";

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptProductionOrderLoomWise.xsd";

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
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
        EditSelectedChange();

        if (Session["usertype"].ToString() != "1" && variable.VarOnlyPreviewButtonShowOnAllEditForm == "1")
        {
            DGOrderdetail.Columns[10].Visible = false;
            DGOrderdetail.Columns[11].Visible = false;
        }
    }
    protected void EditSelectedChange()
    {
        txtfolionoedit.Text = "";
        ddunit.Enabled = true;
        if (chkEdit.Checked == true)
        {
            TDFolioNo.Visible = true;
            TDFolioNotext.Visible = true;
            hnissueorderid.Value = "0";
            ddunit.Enabled = false;
            //BtnUpdateConsumption.Visible = true;
        }
        else
        {
            BtnUpdateConsumption.Visible = false;
            btnsave.Visible = true;
            TDFolioNotext.Visible = false;
            TDFolioNo.Visible = false;
            hnissueorderid.Value = "0";
        }
        DDFolioNo.Items.Clear();
        txtfoliono.Text = "";
        DGOrderdetail.DataSource = null;
        DGOrderdetail.DataBind();

        DG.DataSource = null;
        DG.DataBind();
    }
    protected void GVDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox Chkboxitem = (CheckBox)e.Row.FindControl("Chkboxitem");
            Label lblactivestatus = (Label)e.Row.FindControl("lblactivestatus");
            if (lblactivestatus.Text == "1")
            {
                Chkboxitem.Checked = false;
            }
            else
            {
                Chkboxitem.Checked = true;
                e.Row.BackColor = System.Drawing.Color.Red;
            }
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
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@issueorderid", lblissueorderid.Text);
            param[1] = new SqlParameter("@IssueDetailId", lblissuedetailid.Text);
            param[2] = new SqlParameter("@PROCESSID", DDProcessName.SelectedValue);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@MastercompanyId", Session["varcompanyNo"]);
            param[5] = new SqlParameter("@Userid", Session["varuserid"]);
            //********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETE_TASSEL_MAKING_DEPARTMENT_ORDER", param);
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
    protected void DGOrderdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    TextBox txtqty = (TextBox)e.Row.FindControl("txtqty");
        //    if (Convert.ToInt16(Session["usertype"]) > 2)
        //    {
        //        DGOrderdetail.Columns[10].Visible = false;
        //        DGOrderdetail.Columns[11].Visible = false;
        //    }
        //}
    }
    protected void txtfolionoedit_TextChanged(object sender, EventArgs e)
    {
        string Str = @"Select IssueOrderID, DepartmentID, UnitID, CalType, IssueNo 
        From TasselMakingDepartmentOrderMaster(Nolock) 
        Where MasterCompanyID = " + Session["varCompanyId"] + " And CompanyID = " + DDcompany.SelectedValue + " And ProcessID = " + DDProcessName.SelectedValue + @" And 
        IssueNo = '" + txtfolionoedit.Text + "'";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DDDepartmentName.SelectedValue = ds.Tables[0].Rows[0]["DepartmentID"].ToString();
            DDDepartmentName_SelectedIndexChanged(sender, new EventArgs());
            DDFolioNo.SelectedValue = ds.Tables[0].Rows[0]["IssueOrderID"].ToString();
            FolioNoSelectedChanged();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Please check challan no !');", true);
        }
    }
    protected void BtnUpdateConsumption_Click(object sender, EventArgs e)
    {
        if (DDFolioNo.SelectedIndex > 0)
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
                param[0] = new SqlParameter("@IssueOrderID", DDFolioNo.SelectedValue);
                param[1] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
                param[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                param[2].Direction = ParameterDirection.Output;

                //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateConsumption_Tassel_Making_Order", param);
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
    protected void BtnComplete_Click(object sender, EventArgs e)
    {
        
        if (chkEdit.Checked == true && DDFolioNo.SelectedIndex > 0)
        {
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update TasselMakingDepartmentOrderMaster Set Status = 'Complete' Where IssueOrderID = " + hnissueorderid.Value);
        }

    }
}