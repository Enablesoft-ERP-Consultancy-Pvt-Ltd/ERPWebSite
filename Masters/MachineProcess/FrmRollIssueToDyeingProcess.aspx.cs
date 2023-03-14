using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_MachineProcess_FrmRollIssueToDyeingProcess : System.Web.UI.Page
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
            Where PNM.MasterCompanyid = " + Session["varcompanyid"] + @" Order By PNM.Process_Name_ID 

            Select EI.EmpId, EI.EmpName 
            From Empinfo EI(Nolock)
            JOIN EMPPROCESS EP(Nolock) ON EP.EmpId = EI.EmpId";

            if (Convert.ToInt32(Session["varcompanyid"]) == 21)
            {
                str = str + " And EP.ProcessId = 5 ";
            }
            else
            {
                str = str + " And EP.ProcessId = 5 ";
            }

            str = str + " Where EI.MasterCompanyID = " + Session["varcompanyid"] + " Order By EI.EmpName ";

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
                    DDProcessName.SelectedValue = "5";
                }
                else
                {
                    DDProcessName.SelectedIndex = 4;
                }

                DDProcessName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 2, true, "--Plz Select--");

            FillDGGrid();
            TxtIssueDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            HnIssueID.Value = "0";
        }
    }
    private void FillDGGrid()
    {
//        string str = @"Select a.RollReceiveToNextID, a.RollReceiveToNextDetailID, a.UnitID, a.MainRollFinishedID, a.SubSubRollFinishedID, a.OrderID, a.Item_Finished_ID, 
//        U.UnitName, OM.CustomerOrderNo OrderNo, 
//        VF.ITEM_NAME + ' / ' + VF.QualityName + ' / ' + VF.DesignName + ' / ' + VF.ColorName + ' / ' + VF.ShapeName + ' / ' + 
//        Case When a.UnitID = 1 Then VF.SizeMtr Else Case When a.UnitID = 2 Then VF.SizeFt Else VF.SizeInch End End + Case WHen VF.ShadeColorName <> '' Then ' / ' + VF.ShadeColorName Else '' End ItemDescription, 
//        a.Qty - a.RejectQty Qty 
//        From RollReceiveToNextProcessDetail a(Nolock) 
//        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = a.SubSubRollFinishedID 
//        JOIN OrderMaster OM(Nolock) ON OM.OrderId = a.OrderID 
//        JOIN Unit U ON U.UnitId = a.UnitID 
//        LEFT JOIN RollIssueOtherProcessDetail VRINPD(Nolock) ON VRINPD.RollReceiveToNextDetailID = a.RollReceiveToNextDetailID
//        Where VRINPD.RollReceiveToNextDetailID Is Null 
//        Order By a.RollReceiveToNextDetailID ";


        string str = @"select RRD.RollReceiveOtherProcessId, RRD.RollReceiveOtherProcessDetailId,RRD.RollReceiveToNextID, RRD.RollReceiveToNextDetailID, RRD.UnitID, 
                RRD.SubSubRollFinishedID, RRD.OrderID, RRD.Item_Finished_ID, U.UnitName, OM.CustomerOrderNo OrderNo, 
                VF.ITEM_NAME + ' / ' + VF.QualityName + ' / ' + VF.DesignName + ' / ' + VF.ColorName + ' / ' + VF.ShapeName + ' / ' + 
                Case When RRD.UnitID = 1 Then VF.SizeMtr Else Case When RRD.UnitID = 2 Then VF.SizeFt Else VF.SizeInch End End + Case WHen VF.ShadeColorName <> '' Then ' / ' + VF.ShadeColorName Else '' End ItemDescription, 
                RRD.Qty - RRD.RejectQty Qty,
                isnull((Select distinct MaterialReceiveInPcsDetailID from RollReceiveToNextProcessDetail RRNPD 
				Where RRNPD.RollReceiveToNextID=RRD.RollReceiveToNextId and RRNPD.RollReceiveToNextDetailID=RRD.RollReceiveToNextDetailID),0) as MaterialReceiveInPcsDetailID
                from RollReceiveOtherProcessMatser RRM(NoLock) JOIN RollReceiveOtherProcessDetail RRD(NoLock) ON RRM.RollReceiveOtherProcessId=RRD.RollReceiveOtherProcessId
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = RRD.SubSubRollFinishedID 
                JOIN OrderMaster OM(Nolock) ON OM.OrderId = RRD.OrderID 
                JOIN Unit U ON U.UnitId = RRD.UnitID 
                LEFT JOIN RollIssueDyeingProcessDetail VRINPD(Nolock) ON VRINPD.RollReceiveToNextDetailID = RRD.RollReceiveToNextDetailID
                Where VRINPD.RollReceiveToNextDetailID Is Null and RRM.ProcessId=17
                Order By RRD.RollReceiveToNextDetailID";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtTotalQty.Text = ds.Tables[0].Compute("Sum(Qty)", "").ToString();

        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        string DetailData = "";
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));

            if (Chkboxitem.Checked == true)
            {
                Label LblRollReceiveToNextID = ((Label)DG.Rows[i].FindControl("LblRollReceiveToNextID"));
                Label LblRollReceiveToNextDetailID = ((Label)DG.Rows[i].FindControl("LblRollReceiveToNextDetailID"));
                Label LblUnitID = ((Label)DG.Rows[i].FindControl("LblUnitID"));
                Label lblRollReceiveOtherProcessId = ((Label)DG.Rows[i].FindControl("lblRollReceiveOtherProcessId"));
                Label lblRollReceiveOtherProcessDetailId = ((Label)DG.Rows[i].FindControl("lblRollReceiveOtherProcessDetailId"));
                Label LblMaterialReceiveInPcsDetailID = ((Label)DG.Rows[i].FindControl("LblMaterialReceiveInPcsDetailID"));
                if (DetailData == "")
                {
                    DetailData = LblRollReceiveToNextID.Text + "|" + LblRollReceiveToNextDetailID.Text + "|" + LblUnitID.Text + "|" + lblRollReceiveOtherProcessId.Text + "|" + lblRollReceiveOtherProcessDetailId.Text + "|" + LblMaterialReceiveInPcsDetailID.Text + "~";
                }
                else
                {
                    DetailData = DetailData + LblRollReceiveToNextID.Text + "|" + LblRollReceiveToNextDetailID.Text + "|" + LblUnitID.Text + "|" + lblRollReceiveOtherProcessId.Text + "|" + lblRollReceiveOtherProcessDetailId.Text + "|" + LblMaterialReceiveInPcsDetailID.Text + "~";
                }
            }
        }
        if (DetailData == "")
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
            SqlParameter[] arr = new SqlParameter[15];
            arr[0] = new SqlParameter("@RollIssueDyeingProcessID", SqlDbType.Int);
            arr[1] = new SqlParameter("@CompanyID", SqlDbType.Int);
            arr[2] = new SqlParameter("@ProcessID", SqlDbType.Int);
            arr[3] = new SqlParameter("@EmpID", SqlDbType.Int);
            arr[4] = new SqlParameter("@IssueNo", SqlDbType.NVarChar, 50);
            arr[5] = new SqlParameter("@IssueDate", SqlDbType.DateTime);
            arr[6] = new SqlParameter("@DetailData", SqlDbType.NVarChar);
            arr[7] = new SqlParameter("@UserID", SqlDbType.Int);
            arr[8] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            arr[9] = new SqlParameter("@Msg", SqlDbType.VarChar, 200);
            arr[10] = new SqlParameter("@EwayBillNo", SqlDbType.VarChar, 30);
            arr[11] = new SqlParameter("@VehicleNo", SqlDbType.VarChar, 15);
            arr[12] = new SqlParameter("@HSNCode", SqlDbType.VarChar, 20);
            arr[13] = new SqlParameter("@Remarks", SqlDbType.VarChar, 500);
            arr[14] = new SqlParameter("@Amount", SqlDbType.Float);

            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = HnIssueID.Value;
            arr[1].Value = ddCompName.SelectedValue;
            arr[2].Value = DDProcessName.SelectedValue;
            arr[3].Value = DDEmployeeName.SelectedValue;
            arr[4].Direction = ParameterDirection.InputOutput;
            arr[4].Value = TxtIssueNo.Text;
            arr[5].Value = TxtIssueDate.Text;
            arr[6].Value = DetailData;
            arr[7].Value = Session["varuserid"];
            arr[8].Value = Session["varCompanyId"];
            arr[9].Direction = ParameterDirection.Output;
            arr[10].Value = txtEwayBillNo.Text;
            arr[11].Value = txtVehicleNo.Text;
            arr[12].Value = txtHSNCode.Text;
            arr[13].Value = txtRemarks.Text;
            arr[14].Value = txtAmount.Text;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[Pro_SaveRollIssueDyeingProcess]", arr);

            if (arr[9].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + arr[8].Value.ToString() + "');", true);
                tran.Rollback();
            }
            else
            {
                HnIssueID.Value = arr[0].Value.ToString();
                TxtIssueNo.Text = Convert.ToString(arr[4].Value);
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

    private void fill_grid()
    {
//        string str = @"Select a.RollIssueOtherProcessID, b.RollIssueOtherProcessDetailID, b.RollReceiveToNextDetailID, b.Qty, U.UnitName, 
//            VF.ITEM_NAME + ' / ' + VF.QualityName + ' / ' + VF.DesignName + ' / ' + VF.ColorName + ' / ' + VF.ShapeName + ' / ' + 
//            Case When b.UnitID = 1 Then VF.SizeMtr Else Case When b.UnitID = 2 Then VF.SizeFt Else VF.SizeInch End End + 
//            Case WHen VF.ShadeColorName <> '' Then ' / ' + VF.ShadeColorName Else '' End ItemDescription 
//            From RollIssueOtherProcessMatser a(Nolock)
//            JOIN RollIssueOtherProcessDetail b(Nolock) ON b.RollIssueOtherProcessID = a.RollIssueOtherProcessID 
//            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.SubSubRollFinishedID 
//            JOIN Unit U(Nolock) ON U.UnitID = b.UnitID 
//            Where a.RollIssueOtherProcessID = " + HnIssueID.Value + @" 
//            Order By b.RollIssueOtherProcessDetailID ";

        string str = @"Select a.RollIssueDyeingProcessID, b.RollIssueDyeingProcessDetailID, b.RollReceiveToNextDetailID, b.Qty, U.UnitName, 
            VF.ITEM_NAME + ' / ' + VF.QualityName + ' / ' + VF.DesignName + ' / ' + VF.ColorName + ' / ' + VF.ShapeName + ' / ' + 
            Case When b.UnitID = 1 Then VF.SizeMtr Else Case When b.UnitID = 2 Then VF.SizeFt Else VF.SizeInch End End + 
            Case WHen VF.ShadeColorName <> '' Then ' / ' + VF.ShadeColorName Else '' End ItemDescription 
            From RollIssueDyeingProcessMatser a(Nolock)
            JOIN RollIssueDyeingProcessDetail b(Nolock) ON b.RollIssueDyeingProcessID = a.RollIssueDyeingProcessID 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.SubSubRollFinishedID 
            JOIN Unit U(Nolock) ON U.UnitID = b.UnitID 
            Where a.RollIssueDyeingProcessID = " + HnIssueID.Value + @" 
            Order By b.RollIssueDyeingProcessDetailID";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
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
            //Label LblRollIssueToNextID = (Label)gvdetail.Rows[e.RowIndex].FindControl("LblRollIssueToNextID");
            //Label LblRollIssueToNextDetailID = (Label)gvdetail.Rows[e.RowIndex].FindControl("LblRollIssueToNextDetailID");

            Label lblRollIssueDyeingProcessID = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblRollIssueDyeingProcessID");
            Label lblRollIssueDyeingProcessDetailID = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblRollIssueDyeingProcessDetailID");

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@RollIssueDyeingProcessID", lblRollIssueDyeingProcessID.Text);
            param[1] = new SqlParameter("@RollIssueDyeingProcessDetailId", lblRollIssueDyeingProcessDetailID.Text);
            param[2] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
            param[3] = new SqlParameter("@UserID", Session["VarUserId"]);
            param[4] = new SqlParameter("@MasterCompanyID", Session["VarCompanyId"]);
            param[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[5].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteRollIssueDyeingProcess", param);
            lblmessage.Text = param[5].Value.ToString();
            Tran.Commit();
            fill_grid();
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
        DDIssueNo.Items.Clear();
        TxtIssueNo.Text = "";
        TxtIssueDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
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
        HnIssueID.Value = "0";
        TxtIssueNo.Text = "";
        string str = @"Select Distinct EI.EmpID, EI.EmpName 
            From RollIssueDyeingProcessMatser a(Nolock)
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


//        string Str = @"Select b.RollReceiveToNextDetailID RollNo, CI.CompanyName, CI.CompAddr1 + ', ' + CI.CompAddr2 + ', ' + CI.CompAddr3 CompanyAddress, 
//                        CI.CompTel, CI.GSTNo, PNM.PROCESS_NAME ProcessName, a.IssueNo, 
//                        REPLACE(CONVERT(NVARCHAR(11), a.IssueDate, 106), ' ', '-') IssueDate, U1.UnitName, 
//                        VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + 
//                        Case When b.UnitID = 1 Then VF.SizeMtr Else Case When b.UnitID = 2 Then VF.SizeFt Else VF.SizeInch End End + ' ' + VF.ShadeColorName ItemDescription, 
//                        OM.CustomerOrderNo OrderNo, b.Qty OrderQty 
//                        From RollIssueDyeingProcessMatser a(Nolock)
//                        JOIN RollIssueDyeingProcessDetail b(Nolock) ON b.RollIssueDyeingProcessID = a.RollIssueDyeingProcessID 
//                        JOIN CompanyInfo CI(Nolock) ON CI.CompanyId = a.CompanyID 
//                        JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = a.ProcessID 
//                        JOIN Unit U1(Nolock) ON U1.UnitId = b.UnitID 
//                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.SubSubRollFinishedID 
//                        JOIN OrderMaster OM(Nolock) ON OM.OrderId = b.OrderID 
//                        Where a.RollIssueDyeingProcessID = " + HnIssueID.Value + @" 
//                        Order By b.RollIssueDyeingProcessDetailID ";

//       DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@RollIssueDyeingProcessID", HnIssueID.Value);
        param[1] = new SqlParameter("@MasterCompanyId", Session["VarCompanyNo"]);
        param[2] = new SqlParameter("@UserId", Session["VarUserId"]);
        //************      

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_RollIssueDyeingReport", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptRollIssueToDyeingProcess.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRollIssueToDyeingProcess.xsd";
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
        string str = @"Select a.RollIssueDyeingProcessID, a.IssueNo, REPLACE(CONVERT(NVARCHAR(11), a.IssueDate, 106), ' ', '-') IssueDate,a.EwayBillNo,a.VehicleNo,a.HSNCode,a.Remarks,
            isnull(b.Amount,0) as Amount 
            From RollIssueDyeingProcessMatser a(Nolock) JOIN RollIssueDyeingProcessDetail b(NoLock) ON a.RollIssueDyeingProcessID=b.RollIssueDyeingProcessID
            Where a.MasterCompanyID = " + Session["VarCompanyId"] + " And a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + @" 
            And a.RollIssueDyeingProcessID = " + DDIssueNo.SelectedValue + @"
            Order By a.RollIssueDyeingProcessID Desc";

        DataSet ds = SqlHelper.ExecuteDataset(str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtIssueNo.Text = ds.Tables[0].Rows[0]["IssueNo"].ToString();
            TxtIssueDate.Text = ds.Tables[0].Rows[0]["IssueDate"].ToString();
            HnIssueID.Value = ds.Tables[0].Rows[0]["RollIssueDyeingProcessID"].ToString();

            txtEwayBillNo.Text = ds.Tables[0].Rows[0]["EWayBillNo"].ToString();
            txtVehicleNo.Text = ds.Tables[0].Rows[0]["VehicleNo"].ToString();
            txtHSNCode.Text = ds.Tables[0].Rows[0]["HSNCode"].ToString();
            txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
            txtAmount.Text = ds.Tables[0].Rows[0]["Amount"].ToString();
        }
        fill_grid();
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChKForEdit.Checked == true)
        {
            string str = @"Select RollIssueDyeingProcessID, IssueNo 
            From RollIssueDyeingProcessMatser(Nolock) 
            Where MasterCompanyID = " + Session["VarCompanyId"] + " And CompanyID = " + ddCompName.SelectedValue + " And ProcessID = " + DDProcessName.SelectedValue + @" 
            And EmpID = " + DDEmployeeName.SelectedValue + " Order By RollIssueDyeingProcessID Desc ";

            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref DDIssueNo, ds, 0, true, "-Select Issue No-");
        }
    }
}
