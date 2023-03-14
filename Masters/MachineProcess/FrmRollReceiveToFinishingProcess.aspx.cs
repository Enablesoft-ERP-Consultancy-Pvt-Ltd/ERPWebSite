using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_MachineProcess_FrmRollReceiveToFinishingProcess : System.Web.UI.Page
{
    string str = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            str = @"Select Distinct CI.CompanyId, CI.Companyname 
            From Companyinfo CI(nolock)
            JOIN Company_Authentication CA(nolock) ON CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + @" 
            Where CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CI.Companyname 

            Select PNM.PROCESS_NAME_ID, PNM.PROCESS_NAME 
            From PROCESS_NAME_MASTER PNM(nolock) 
            Where PNM.MasterCompanyid = " + Session["varcompanyid"] + @" and PNM.PROCESS_NAME='FINISHING' Order By PNM.Process_Name_ID 

             Select EI.EmpId, EI.EmpName 
            From Empinfo EI(Nolock)
            JOIN EMPPROCESS EP(Nolock) ON EP.EmpId = EI.EmpId";           

            if (Convert.ToInt32(Session["varcompanyid"]) == 21)
            {
                str = str + " And EP.ProcessId = 8 ";
            }
            else
            {
                str = str + " And EP.ProcessId = 8 ";
            }

            str = str + " Where EI.MasterCompanyID = " + Session["varcompanyid"] + @" Order By EI.EmpName ";

            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "Select Comp Name");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Plz Select--");

            if (DDProcessName.Items.Count > 0)
            {
                if (Convert.ToInt32(Session["varcompanyid"]) == 21)
                {
                    DDProcessName.SelectedValue = "8";
                }
                else
                {
                    DDProcessName.SelectedIndex = 8;
                }
                DDProcessName_SelectedIndexChanged(sender, new EventArgs());
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 2, true, "--Plz Select--");            

            TxtReceiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            HnReceiveID.Value = "0";

            fill_grid();
        }
    }

    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        str = @"Select Distinct EI.EmpID, EI.EmpName 
            From RollIssueFinishingProcessMaster a(Nolock)
            JOIN Empinfo EI ON EI.EmpID = a.EmpID 
            Where a.MasterCompanyID = " + Session["VarCompanyId"] + " And a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + @" 
            Order By EI.EmpName ";

        DataSet ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 0, true, "Select Emp Name");

    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        string RollNoDetailData = "";
       
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));

            if (Chkboxitem.Checked == true)
            {
                Label lblRollReceiveDyeingProcessID = ((Label)DG.Rows[i].FindControl("lblRollReceiveDyeingProcessID"));
                Label lblRollReceiveDyeingProcessDetailID = ((Label)DG.Rows[i].FindControl("lblRollReceiveDyeingProcessDetailID"));
                Label lblRollNoOrderID = ((Label)DG.Rows[i].FindControl("lblRollNoOrderID"));
                Label lblItem_Finished_ID = ((Label)DG.Rows[i].FindControl("lblItem_Finished_ID"));
                Label lblDyedLotNo = ((Label)DG.Rows[i].FindControl("lblDyedLotNo")); 
                TextBox txtRecQty = ((TextBox)DG.Rows[i].FindControl("txtRecQty"));
                TextBox txtRejectQty = ((TextBox)DG.Rows[i].FindControl("txtRejectQty"));
                
                if (RollNoDetailData == "")
                {
                    RollNoDetailData = lblRollReceiveDyeingProcessID.Text + "|" + lblRollReceiveDyeingProcessDetailID.Text + "|" + lblRollNoOrderID.Text + "|" + lblItem_Finished_ID.Text + "|" + lblDyedLotNo.Text + "|" + txtRecQty.Text + "|" + txtRejectQty.Text + "~";
                                        
                }
                else
                {
                    RollNoDetailData = RollNoDetailData + lblRollReceiveDyeingProcessID.Text + "|" + lblRollReceiveDyeingProcessDetailID.Text + "|" + lblRollNoOrderID.Text + "|" + lblItem_Finished_ID.Text + "|" + lblDyedLotNo.Text + "|" + txtRecQty.Text + "|" + txtRejectQty.Text + "~";
                }
            }
        }
        if (RollNoDetailData == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box');", true);
            return;
        }
        

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[11];
            arr[0] = new SqlParameter("@RollReceiveFinishingProcessID", SqlDbType.Int);
            arr[1] = new SqlParameter("@CompanyID", SqlDbType.Int);
            arr[2] = new SqlParameter("@ProcessID", SqlDbType.Int);
            arr[3] = new SqlParameter("@EmpID", SqlDbType.Int);
            arr[4] = new SqlParameter("@RollIssueFinishingProcessID", SqlDbType.Int);
            arr[5] = new SqlParameter("@ReceiveNo", SqlDbType.NVarChar, 50);
            arr[6] = new SqlParameter("@ReceiveDate", SqlDbType.DateTime);           
            arr[7] = new SqlParameter("@RollNoDetailData", SqlDbType.NVarChar);           
            arr[8] = new SqlParameter("@UserID", SqlDbType.Int);
            arr[9] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            arr[10] = new SqlParameter("@Msg", SqlDbType.VarChar, 200);

            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = HnReceiveID.Value;
            arr[1].Value = ddCompName.SelectedValue;
            arr[2].Value = DDProcessName.SelectedValue;
            arr[3].Value = DDEmployeeName.SelectedValue;
            //arr[4].Value = DDIssueNo.SelectedValue;
            arr[4].Value = 0;
            arr[5].Direction = ParameterDirection.InputOutput;
            arr[5].Value = TxtReceiveNo.Text;
            arr[6].Value = TxtReceiveDate.Text;           
            arr[7].Value = RollNoDetailData;            
            arr[8].Value = Session["varuserid"];
            arr[9].Value = Session["varCompanyId"];
            arr[10].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[Pro_SaveRollReceiveFinishingProcess]", arr);

            if (arr[10].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + arr[10].Value.ToString() + "');", true);
                tran.Rollback();
            }
            else
            {
                HnReceiveID.Value = arr[0].Value.ToString();
                TxtReceiveNo.Text = Convert.ToString(arr[5].Value);
                tran.Commit();
            }
            FillDGGrid();            
            fill_grid();
            btnPreview.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + ex.Message + "');", true);
            tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }    
    protected void gvdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvdetail, "Select$" + e.Row.RowIndex);
        }
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
            Label LblRollReceiveFinishingProcessID = (Label)gvdetail.Rows[e.RowIndex].FindControl("LblRollReceiveFinishingProcessID");
            Label LblRollReceiveFinishingProcessDetailID = (Label)gvdetail.Rows[e.RowIndex].FindControl("LblRollReceiveFinishingProcessDetailID");

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@RollReceiveFinishingProcessID", LblRollReceiveFinishingProcessID.Text);
            param[1] = new SqlParameter("@RollReceiveFinishingProcessDetailID", LblRollReceiveFinishingProcessDetailID.Text);
            param[2] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
            param[3] = new SqlParameter("@UserID", Session["VarUserId"]);
            param[4] = new SqlParameter("@MasterCompanyID", Session["VarCompanyId"]);
            param[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[5].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteRollReceiveToFinishingProcess", param);
            lblmessage.Text = param[5].Value.ToString();
            Tran.Commit();
            fill_grid();
            //FillOrderDescription();
            FillDGGrid();
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
    protected void ChKForEdit_CheckedChanged(object sender, EventArgs e)
    {
        DDReceiveNo.Items.Clear();
        DDIssueNo.Items.Clear();
        TxtReceiveNo.Text = "";
        TxtReceiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        Td3.Visible = false;
        if (ChKForEdit.Checked == true)
        {
            EditCheckedChanged();
            Td3.Visible = true;
            btnPreview.Visible = true;
        }
    }
    private void EditCheckedChanged()
    {
        HnReceiveID.Value = "0";
        TxtReceiveNo.Text = "";
        //string str = @"";

        string str = @"Select Distinct EI.EmpID, EI.EmpName 
            From RollReceiveFinishingProcessMaster a(Nolock)
            JOIN Empinfo EI ON EI.EmpID = a.EmpID 
            Where a.MasterCompanyID = " + Session["VarCompanyId"] + " And a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + @"
            Order By EI.EmpName ";

        DataSet ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 0, true, "-Select Employee-");
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        report();
    }
    private void report()
    {
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@RollReceiveFinishingProcessID", HnReceiveID.Value);
        param[1] = new SqlParameter("@MasterCompanyId", Session["VarCompanyNo"]);
        param[2] = new SqlParameter("@UserId", Session["VarUserId"]);
        //************      

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_RollReceiveFinishingReport", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptRollReceiveToFinishingProcess.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRollReceiveToFinishingProcess.xsd";
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
    protected void DDIssue_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_grid();
    }
    private void fill_grid()
    {
        SqlParameter[] param = new SqlParameter[2];
        //param[0] = new SqlParameter("@RollIssueFinishingProcessID", DDIssueNo.SelectedValue);
        param[0] = new SqlParameter("@MasterCompanyId", Session["VarCompanyNo"]);
        param[1] = new SqlParameter("@UserId", Session["VarUserId"]);
        //************      

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_RollReceiveDyeingDetail", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DG.DataSource = ds.Tables[0];
            DG.DataBind();
        }
        else
        {
            DG.DataSource = null;
            DG.DataBind();
        }
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //        string str = @"Select Distinct a.RollIssueFinishingProcessID, a.IssueNo 
        //            From RollIssueFinishingProcessMaster a(Nolock)
        //            JOIN RollIssueFinishingProcessDetail b(Nolock) ON b.RollIssueFinishingProcessID = a.RollIssueFinishingProcessID 
        //            LEFT JOIN RollReceiveFinishingProcessDetail c(Nolock) ON c.RollIssueFinishingProcessID = a.RollIssueFinishingProcessID And c.RollIssueFinishingProcessDetailID = b.RollIssueFinishingProcessDetailID 
        //            Where c.RollIssueFinishingProcessID Is null And a.MasterCompanyID = " + Session["VarCompanyId"] + " And a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + @" 
        //            And a.EmpID = " + DDEmployeeName.SelectedValue + " Order By a.RollIssueFinishingProcessID Desc ";

        //        DataSet ds = SqlHelper.ExecuteDataset(str);
        //        UtilityModule.ConditionalComboFillWithDS(ref DDIssueNo, ds, 0, true, "-Select Issue No-");

        if (ChKForEdit.Checked == true)
        {
            string str2 = @" Select distinct a.RollReceiveFinishingProcessID, a.ReceiveNo 
                From RollReceiveFinishingProcessMaster a(Nolock) JOIN RollReceiveFinishingProcessDetail b(Nolock) ON a.RollReceiveFinishingProcessID=b.RollReceiveFinishingProcessID
                Where a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + " And a.EmpID = " + DDEmployeeName.SelectedValue + @"  
                Order By a.RollReceiveFinishingProcessID";

            DataSet ds2 = SqlHelper.ExecuteDataset(str2);
            UtilityModule.ConditionalComboFillWithDS(ref DDReceiveNo, ds2, 0, true, "-Select Issue No-");
        }
    }
    protected void DDReceive_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"Select a.RollReceiveFinishingProcessID, a.ReceiveNo, REPLACE(CONVERT(NVARCHAR(11), a.ReceiveDate, 106), ' ', '-') ReceiveDate            
            From RollReceiveFinishingProcessMaster a(Nolock) 
            Where a.MasterCompanyID = " + Session["VarCompanyId"] + " And a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + @" 
            And a.RollReceiveFinishingProcessID = " + DDReceiveNo.SelectedValue + @"
            Order By a.RollReceiveFinishingProcessID Desc";

        DataSet ds = SqlHelper.ExecuteDataset(str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtReceiveNo.Text = ds.Tables[0].Rows[0]["ReceiveNo"].ToString();
            TxtReceiveDate.Text = ds.Tables[0].Rows[0]["ReceiveDate"].ToString();
            HnReceiveID.Value = ds.Tables[0].Rows[0]["RollReceiveFinishingProcessID"].ToString();
        }
        FillDGGrid();
    }

    private void FillDGGrid()
    {
        string str = @"Select b.RollReceiveFinishingProcessID, b.RollReceiveFinishingProcessDetailID, b.RollReceiveToNextDetailID, U.UnitName, 
            VF.ITEM_NAME + ' / ' + VF.QualityName + ' / ' + VF.DesignName + ' / ' + VF.ColorName + ' / ' + VF.ShapeName + ' / ' + 
            Case When b.UnitID = 1 Then VF.SizeMtr Else Case When b.UnitID = 2 Then VF.SizeFt Else VF.SizeInch End End + 
            Case WHen VF.ShadeColorName <> '' Then ' / ' + VF.ShadeColorName Else '' End ItemDescription, 
            VF2.ITEM_NAME + ' / ' + VF2.QualityName + ' / ' + VF2.DesignName + ' / ' + VF2.ColorName + ' / ' + VF2.ShapeName + ' / ' + 
            Case When b.UnitID = 1 Then VF2.SizeMtr Else Case When b.UnitID = 2 Then VF2.SizeFt Else VF2.SizeInch End End + 
            Case WHen VF2.ShadeColorName <> '' Then ' / ' + VF2.ShadeColorName Else '' End OrderItemDescription,
			b.RecQty Qty,isnull(B.DyedLotNo,'') as DyedLotNo 
            From RollReceiveFinishingProcessMaster a(Nolock)
            JOIN RollReceiveFinishingProcessDetail b(Nolock) ON b.RollReceiveFinishingProcessID = a.RollReceiveFinishingProcessID 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.SubSubRollFinishedID 
            JOIN Unit U(Nolock) ON U.UnitID = b.UnitID
            JOIN V_FinishedItemDetail VF2(Nolock) ON VF2.ITEM_FINISHED_ID = b.Item_Finished_ID 
            Where a.RollReceiveFinishingProcessID = " + HnReceiveID.Value + @"  
            Order By b.RollReceiveFinishingProcessDetailID ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
    }
}
