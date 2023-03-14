using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_MachineProcess_FrmRollReceiveToDyeingProcess : System.Web.UI.Page
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

            str = str + " Where EI.MasterCompanyID = " + Session["varcompanyid"] + @" Order By EI.EmpName 
                
             Select Distinct OM.CustomerID, CI.CustomerCode 
                        From OrderMaster OM(Nolock) 
                        JOIN CustomerInfo CI(Nolock) ON CI.CustomerID = OM.CustomerID 
                        JOIN JobAssigns JA(Nolock) ON JA.OrderId = OM.OrderID 
                        Where OM.CompanyId = " + Session["CurrentWorkingCompanyID"] + @" 
                        Order By CI.CustomerCode";

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
                    DDProcessName.SelectedIndex = 5;
                }
                DDProcessName_SelectedIndexChanged(sender, new EventArgs());
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds, 3, true, "--Plz Select--");

            TxtReceiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            HnReceiveID.Value = "0";
        }
    }

    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        str = @"Select Distinct EI.EmpID, EI.EmpName 
            From RollIssueOtherProcessMatser a(Nolock)
            JOIN Empinfo EI ON EI.EmpID = a.EmpID 
            Where a.MasterCompanyID = " + Session["VarCompanyId"] + " And a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + @" 
            Order By EI.EmpName ";

        DataSet ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 0, true, "Select Emp Name");

    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillOrderNO();
    }
    protected void FillOrderNO()
    {
        string str = @"Select Distinct OM.OrderId, OM.CustomerOrderNo 
        From OrderMaster OM
        JOIN OrderDetail OD(Nolock) ON OD.OrderID = OM.OrderID 
        Where OM.Status = 0 And OM.CompanyId = " + ddCompName.SelectedValue + " And OM.CustomerId = " + DDCustomerCode.SelectedValue + @" Order By OM.OrderId";
        UtilityModule.ConditionalComboFill(ref DDCustomerOrderNumber, str, true, "--Plz Select--");
    }
    protected void DDCustomerOrderNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillOrderDescription();
    }
    protected void FillOrderDescription()
    {
////        string str = @"Select OM.CustomerOrderNo, OD.Item_Finished_Id, VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + 
////        Case When OD.OrderUnitId = 1 Then VF.SizeMtr Else Case When OD.OrderUnitId = 2 Then VF.SizeFt Else VF.SizeInch End End [ItemDescription],
////        U.UnitName,OD.QtyRequired,isnull(sum(RRDP.RecQty),0) as DyedQty,(OD.QtyRequired-isnull(sum(RRDP.RecQty),0)) as PendingQty
////        From OrderMaster OM(nolock)
////        JOIN OrderDetail OD(nolock) ON OD.OrderID = OM.OrderID 
////        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OD.Item_Finished_Id
////        JOIN Unit U ON OD.OrderUnitId=U.UnitId
////        LEFT JOIN RollReceiveDyeingProcessDetail RRDP(NoLock) ON OD.OrderId=RRDP.OrderID and OD.Item_Finished_Id=RRDP.Item_Finished_ID
////        Where OM.OrderID = " + DDCustomerOrderNumber.SelectedValue + @" 
////        Group by OM.CustomerOrderNo, OD.Item_Finished_Id, VF.ITEM_NAME ,VF.QualityName,VF.DesignName , VF.ColorName, VF.ShapeName,OD.OrderUnitId,VF.SizeMtr,
////        VF.SizeFt,VF.SizeInch,U.UnitName,OD.QtyRequired
////        Having (OD.QtyRequired-isnull(sum(RRDP.RecQty),0))>0
////        Order By VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + 
////        Case When OD.OrderUnitId = 1 Then VF.SizeMtr Else Case When OD.OrderUnitId = 2 Then VF.SizeFt Else VF.SizeInch End End ";

////        UtilityModule.ConditionalComboFill(ref DDOrderDescription, str, true, "--Plz Select--");

         SqlParameter[] param = new SqlParameter[3];
         param[0] = new SqlParameter("@OrderId", DDCustomerOrderNumber.SelectedValue);
        param[1] = new SqlParameter("@MasterCompanyId", Session["VarCompanyNo"]);
        param[2] = new SqlParameter("@UserId", Session["VarUserId"]);
        //************      

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_RollCustomerOrderDetail", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOrderDetail.DataSource = ds.Tables[0];
            gvOrderDetail.DataBind();
        }

    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        string RollNoDetailData = "";
        string OrderNoDetailData = "";
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));

            if (Chkboxitem.Checked == true)
            {
                Label LblRollIssueDyeingProcessID = ((Label)DG.Rows[i].FindControl("LblRollIssueDyeingProcessID"));
                Label LblRollIssueDyeingProcessDetailID = ((Label)DG.Rows[i].FindControl("LblRollIssueDyeingProcessDetailID"));
                Label lblRollNoOrderID = ((Label)DG.Rows[i].FindControl("lblRollNoOrderID"));
                //TextBox TxtRejectPcs = ((TextBox)DG.Rows[i].FindControl("TxtRejectPcs"));
                if (RollNoDetailData == "")
                {
                    RollNoDetailData = LblRollIssueDyeingProcessID.Text + "|" + LblRollIssueDyeingProcessDetailID.Text + "|" + lblRollNoOrderID.Text + "~";
                }
                else
                {
                    RollNoDetailData = RollNoDetailData + LblRollIssueDyeingProcessID.Text + "|" + LblRollIssueDyeingProcessDetailID.Text + "|" + lblRollNoOrderID.Text + "~";
                }
            }
        }
        if (RollNoDetailData == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box');", true);
            return;
        }


        for (int i = 0; i < gvOrderDetail.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)gvOrderDetail.Rows[i].FindControl("Chkboxitem"));

            if (Chkboxitem.Checked == true)
            {
                Label lblItemFinishedId = ((Label)gvOrderDetail.Rows[i].FindControl("lblItemFinishedId"));
                Label lblOrderId = ((Label)gvOrderDetail.Rows[i].FindControl("lblOrderId"));
                TextBox txtRecQty = ((TextBox)gvOrderDetail.Rows[i].FindControl("txtRecQty"));
                TextBox txtDyedLotNo = ((TextBox)gvOrderDetail.Rows[i].FindControl("txtDyedLotNo"));
                TextBox txtRejectQty = ((TextBox)gvOrderDetail.Rows[i].FindControl("txtRejectQty"));

                if (OrderNoDetailData == "")
                {
                    OrderNoDetailData = lblItemFinishedId.Text + "|" + lblOrderId.Text + "|" + txtRecQty.Text + "|" + txtDyedLotNo.Text + "|" + txtRejectQty.Text + "~";
                }
                else
                {
                    OrderNoDetailData = OrderNoDetailData + lblItemFinishedId.Text + "|" + lblOrderId.Text + "|" + txtRecQty.Text + "|" + txtDyedLotNo.Text + "|" + txtRejectQty.Text + "~";
                }
            }
        }
        if (OrderNoDetailData == "")
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
            arr[0] = new SqlParameter("@RollReceiveDyeingProcessID", SqlDbType.Int);
            arr[1] = new SqlParameter("@CompanyID", SqlDbType.Int);
            arr[2] = new SqlParameter("@ProcessID", SqlDbType.Int);
            arr[3] = new SqlParameter("@EmpID", SqlDbType.Int);
            arr[4] = new SqlParameter("@RollIssueDyeingProcessID", SqlDbType.Int);
            arr[5] = new SqlParameter("@ReceiveNo", SqlDbType.NVarChar, 50);
            arr[6] = new SqlParameter("@ReceiveDate", SqlDbType.DateTime);
            arr[7] = new SqlParameter("@PartyChallanNo", SqlDbType.VarChar,150);
            arr[8] = new SqlParameter("@PartyBillNo", SqlDbType.VarChar, 150);
            arr[9] = new SqlParameter("@RollNoDetailData", SqlDbType.NVarChar);
            arr[10] = new SqlParameter("@OrderNoDetailData", SqlDbType.NVarChar);
            arr[11] = new SqlParameter("@UserID", SqlDbType.Int);
            arr[12] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            arr[13] = new SqlParameter("@Msg", SqlDbType.VarChar, 200);
            arr[14] = new SqlParameter("@Remarks", SqlDbType.VarChar, 500);

            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = HnReceiveID.Value;
            arr[1].Value = ddCompName.SelectedValue;
            arr[2].Value = DDProcessName.SelectedValue;
            arr[3].Value = DDEmployeeName.SelectedValue;
            arr[4].Value = DDIssueNo.SelectedValue;
            arr[5].Direction = ParameterDirection.InputOutput;
            arr[5].Value = TxtReceiveNo.Text;
            arr[6].Value = TxtReceiveDate.Text;
            arr[7].Value = txtPartyChallanNo.Text;
            arr[8].Value = txtPartyBillNo.Text;
            arr[9].Value = RollNoDetailData;
            arr[10].Value =OrderNoDetailData;
            arr[11].Value = Session["varuserid"];
            arr[12].Value = Session["varCompanyId"];
            arr[13].Direction = ParameterDirection.Output;
            arr[14].Value = txtRemarks.Text;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[Pro_SaveRollReceiveDyeingProcess]", arr);

            if (arr[13].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + arr[13].Value.ToString() + "');", true);
                tran.Rollback();
            }
            else
            {
                HnReceiveID.Value = arr[0].Value.ToString();
                TxtReceiveNo.Text = Convert.ToString(arr[5].Value);
                tran.Commit();
            }
            FillDGGrid();
            FillOrderDescription();
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
    protected void gvOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvOrderDetail, "Select$" + e.Row.RowIndex);
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
            Label LblRollReceiveDyeingProcessID = (Label)gvdetail.Rows[e.RowIndex].FindControl("LblRollReceiveDyeingProcessID");
            Label LblRollReceiveDyeingProcessDetailID = (Label)gvdetail.Rows[e.RowIndex].FindControl("LblRollReceiveDyeingProcessDetailID");

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@RollReceiveDyeingProcessID", LblRollReceiveDyeingProcessID.Text);
            param[1] = new SqlParameter("@RollReceiveDyeingProcessDetailID", LblRollReceiveDyeingProcessDetailID.Text);
            param[2] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
            param[3] = new SqlParameter("@UserID", Session["VarUserId"]);
            param[4] = new SqlParameter("@MasterCompanyID", Session["VarCompanyId"]);
            param[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[5].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteRollReceiveToDyeingProcess", param);
            lblmessage.Text = param[5].Value.ToString();
            Tran.Commit();
            fill_grid();
            FillOrderDescription();
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
            From RollReceiveDyeingProcessMatser a(Nolock)
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
        param[0] = new SqlParameter("@RollReceiveDyeingProcessID", HnReceiveID.Value);
        param[1] = new SqlParameter("@MasterCompanyId", Session["VarCompanyNo"]);
        param[2] = new SqlParameter("@UserId", Session["VarUserId"]);
        //************      

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_RollReceiveDyeingReport", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptRollReceiveToDyeingProcess.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRollReceiveToDyeingProcess.xsd";
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
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@RollIssueDyeingProcessID", DDIssueNo.SelectedValue);
        param[1] = new SqlParameter("@MasterCompanyId", Session["VarCompanyNo"]);
        param[2] = new SqlParameter("@UserId", Session["VarUserId"]);
        //************      

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_RollIssueDyeingDetail", param);

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

////        string str = @"Select Distinct a.RollIssueDyeingProcessID, a.IssueNo 
////            From RollIssueDyeingProcessMatser a(Nolock)
////            JOIN RollIssueDyeingProcessDetail b(Nolock) ON b.RollIssueDyeingProcessID = a.RollIssueDyeingProcessID 
////            LEFT JOIN RollReceiveDyeingProcessDetail c(Nolock) ON c.RollIssueDyeingProcessID = a.RollIssueDyeingProcessID And c.RollIssueDyeingProcessDetailID = b.RollIssueDyeingProcessDetailID 
////            Where c.RollIssueDyeingProcessID Is null And a.MasterCompanyID = " + Session["VarCompanyId"] + " And a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + @" 
////            And a.EmpID = " + DDEmployeeName.SelectedValue + " Order By a.RollIssueDyeingProcessID Desc ";

        string str = @"Select Distinct a.RollIssueDyeingProcessID, a.IssueNo 
            From RollIssueDyeingProcessMatser a(Nolock)
            JOIN RollIssueDyeingProcessDetail b(Nolock) ON b.RollIssueDyeingProcessID = a.RollIssueDyeingProcessID 
            LEFT JOIN RollReceiveDyeingProcessDetail c(Nolock) ON c.RollIssueDyeingProcessID = a.RollIssueDyeingProcessID And c.RollIssueDyeingProcessDetailID = b.RollIssueDyeingProcessDetailID 
            Where a.MasterCompanyID = " + Session["VarCompanyId"] + " And a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + @" 
            And a.EmpID = " + DDEmployeeName.SelectedValue + " Order By a.RollIssueDyeingProcessID Desc ";

        DataSet ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDIssueNo, ds, 0, true, "-Select Issue No-");

        if (ChKForEdit.Checked == true)
        {
            string str2= @" Select distinct a.RollReceiveDyeingProcessID, a.ReceiveNo 
                From RollReceiveDyeingProcessMatser a(Nolock) JOIN RollReceiveDyeingProcessDetail b(Nolock) ON a.RollReceiveDyeingProcessID=b.RollReceiveDyeingProcessID
                Where a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + " And a.EmpID = " + DDEmployeeName.SelectedValue + @"  
                Order By a.RollReceiveDyeingProcessID";

            DataSet ds2 = SqlHelper.ExecuteDataset(str2);
            UtilityModule.ConditionalComboFillWithDS(ref DDReceiveNo, ds2, 0, true, "-Select Issue No-");
        }
    }
    protected void DDReceive_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"Select a.RollReceiveDyeingProcessID, a.ReceiveNo, REPLACE(CONVERT(NVARCHAR(11), a.ReceiveDate, 106), ' ', '-') ReceiveDate,
            isnull(a.PartyChallanNo,'') as PartyChallanNo,isnull(a.PartyBillNo,'') as PartyBillNo 
            From RollReceiveDyeingProcessMatser a(Nolock) 
            Where a.MasterCompanyID = " + Session["VarCompanyId"] + " And a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + @" 
            And a.RollReceiveDyeingProcessID = " + DDReceiveNo.SelectedValue + @"
            Order By a.RollReceiveDyeingProcessID Desc";

        DataSet ds = SqlHelper.ExecuteDataset(str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtReceiveNo.Text = ds.Tables[0].Rows[0]["ReceiveNo"].ToString();
            TxtReceiveDate.Text = ds.Tables[0].Rows[0]["ReceiveDate"].ToString();
            HnReceiveID.Value = ds.Tables[0].Rows[0]["RollReceiveDyeingProcessID"].ToString();
            txtPartyChallanNo.Text = ds.Tables[0].Rows[0]["PartyChallanNo"].ToString();
            txtPartyBillNo.Text = ds.Tables[0].Rows[0]["PartyBillNo"].ToString();
        }
        FillDGGrid();
    }

    private void FillDGGrid()
    {
        string str = @"Select b.RollReceiveDyeingProcessID, b.RollReceiveDyeingProcessDetailID, b.RollReceiveToNextDetailID, U.UnitName, 
            VF.ITEM_NAME + ' / ' + VF.QualityName + ' / ' + VF.DesignName + ' / ' + VF.ColorName + ' / ' + VF.ShapeName + ' / ' + 
            Case When b.UnitID = 1 Then VF.SizeMtr Else Case When b.UnitID = 2 Then VF.SizeFt Else VF.SizeInch End End + 
            Case WHen VF.ShadeColorName <> '' Then ' / ' + VF.ShadeColorName Else '' End ItemDescription, 
            VF2.ITEM_NAME + ' / ' + VF2.QualityName + ' / ' + VF2.DesignName + ' / ' + VF2.ColorName + ' / ' + VF2.ShapeName + ' / ' + 
            Case When b.UnitID = 1 Then VF2.SizeMtr Else Case When b.UnitID = 2 Then VF2.SizeFt Else VF2.SizeInch End End + 
            Case WHen VF2.ShadeColorName <> '' Then ' / ' + VF2.ShadeColorName Else '' End OrderItemDescription,
			b.RecQty Qty,isnull(B.DyedLotNo,'') as DyedLotNo 
            From RollReceiveDyeingProcessMatser a(Nolock)
            JOIN RollReceiveDyeingProcessDetail b(Nolock) ON b.RollReceiveDyeingProcessID = a.RollReceiveDyeingProcessID 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.SubSubRollFinishedID 
            JOIN Unit U(Nolock) ON U.UnitID = b.UnitID
            JOIN V_FinishedItemDetail VF2(Nolock) ON VF2.ITEM_FINISHED_ID = b.Item_Finished_ID 
            Where a.RollReceiveDyeingProcessID = " + HnReceiveID.Value + @"  
            Order By b.RollReceiveDyeingProcessDetailID ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
    }
}
