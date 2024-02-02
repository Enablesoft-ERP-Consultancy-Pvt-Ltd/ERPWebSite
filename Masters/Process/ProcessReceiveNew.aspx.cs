using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_process_ProcessReceiveNew : System.Web.UI.Page
{
    static int MasterCompanyId;
    static int sum = 0;
    static int GVReceiveCount = 0;
    static string name = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        MasterCompanyId = Convert.ToInt16(Session["varMasterCompanyIDForERP"]);
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
            string process;
            if (Session["varMasterCompanyIDForERP"].ToString() == "9")
            {
                process = "Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" and process_name_id in(1,16) Order By PROCESS_NAME";
            }
            else
            {
                process = "Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" and Process_name_id=1 Order By PROCESS_NAME";
            }
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName
                        " + process + @"
                        Select Unitid,UnitName from Unit Where Unitid in (1,2,7,4,6)
                        Select VarCompanyNo,VarProdCode From MasterSetting 
                        DELETE TEMP_PROCESS_ISSUE_MASTER_NEW ";

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDunit, ds, 2, true, "-Select-");
            hncomp.Value = ds.Tables[3].Rows[0]["VarCompanyNo"].ToString();
            int VarProdCode = Convert.ToInt32(ds.Tables[3].Rows[0]["VarProdCode"]);

            if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 7)
            {
                if (DDunit.Items.Count > 0)
                {
                    DDunit.SelectedValue = "4";
                }
                //TxtPRemarks.Visible = false;
            }
            else
            {
                //TxtPRemarks.Visible = true;
            }
            TxtRecDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtPostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix(TxtPrefix.Text));
            ViewState["Process_Rec_Id"] = 0;
            ViewState["VarGatePassNo"] = 0;
            ParameteLabel();
            //switch (VarProdCode)
            //{
            //    case 0:
            //        TDTextProductCode.Visible = false;
            //        break;
            //    case 1:
            //        TDTextProductCode.Visible = true;
            //        break;
            //}
            lblChallanNo.Text = "";

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            if (DDProcessName.Items.Count > 0)
            {
                if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 7)
                {
                    DDProcessName.SelectedValue = "9";
                    ViewState["Process_Rec_Id"] = 0;
                    ProcessSelectedChange();
                    //BtnPreview.Visible = false;
                    //btnqcchkpreview.Visible = false;
                    //BtnGatePass.Visible = false;
                }
                else
                {
                    if (DDProcessName.Items.FindByValue("1") != null)
                    {
                        DDProcessName.SelectedValue = "1";
                    }
                }
                UtilityModule.ConditionalComboFill(ref DDEmployeeNamee, "Select Distinct EI.EmpId,EI.EmpName from Empinfo EI,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM WHERE PIM.EmpId=EI.EmpId And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by EI.EmpName", true, "--Select--");
            }
            //DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
            //if (Ds.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            //    {
            //        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Insert into TEMP_PROCESS_ISSUE_MASTER_NEW SELECT Distinct PM.Companyid,OM.Customerid,PD.Orderid," + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " ProcessId,PM.Empid,PM.IssueOrderid FROM PROCESS_ISSUE_MASTER_" + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " PM,PROCESS_ISSUE_DETAIL_" + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " PD,OrderMaster OM Where PM.IssueOrderid=PD.IssueOrderid And PD.Orderid=OM.Orderid And PQty<>0");
            //    }
            //}


            // btnqcchkpreview.Enabled = false;
            visible_comp();
            //
            switch (Convert.ToInt16(Session["varMasterCompanyIDForERP"]))
            {
                case 9:
                    //btnqcchkpreview.Visible = false;
                    //BtnGatePass.Visible = false;
                    chkForSlip.Visible = true;
                    chkForSlip.Checked = true;
                    break;
            }

            Enable_TotalWeight();
        }
    }
    private void Enable_TotalWeight()
    {
        if (chkboxManualWeight.Checked == true)
        {
            txtTotalWeight.Enabled = true;

            GVCarpetReceive.Columns[14].Visible = false;
            GVCarpetReceive.Columns[15].Visible = false;
            GVCarpetReceive.Columns[16].Visible = false;
        }
        else
        {
            txtTotalWeight.Enabled = false;
            GVCarpetReceive.Columns[14].Visible = true;
            GVCarpetReceive.Columns[15].Visible = true;
            GVCarpetReceive.Columns[16].Visible = true;
        }
    }
    protected void chkboxManualWeight_CheckedChanged(object sender, EventArgs e)
    {
        Enable_TotalWeight();
    }
    private void visible_comp()
    {
        if (hncomp.Value == "20")
        {
            trprifix.Visible = false;
            TDDDPONO.Visible = false;
            trUnitCalType.Visible = false;
            tdRemarks.Visible = false;
        }

        if (hncomp.Value == "7")
        {
            //TDArea.Visible = false;
            //TDleng.Visible = false;
            //Tdpenality.Visible = false;
            //TDrate.Visible = false;
            //TDweigth.Visible = false;
            //TDwwidth.Visible = false;
            //TDComm.Visible = false;
            trprifix.Visible = false;
            //tdcalname.Visible = false;

        }
    }
    private void ParameteLabel()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
    }
    private void ClearAfterCompanyChange()
    {
        DDEmployeeNamee.Focus();
        GVCarpetReceive.DataSource = null;
        GVCarpetReceive.DataBind();

        GVPenalty.DataSource = null;
        GVPenalty.DataBind();

        dgorder.DataSource = null;
        dgorder.DataBind();

        ViewState["CarpetReceive"] = null;
        ViewState["CarpetReceivePen"] = null;

        chkboxSampleFlag.Checked = false;
        chkboxManualWeight.Checked = false;

        txtTotalWeight.Text = "";
        txtCheckPcs.Text = "";
        txtCheckWeight.Text = "";

        //TxtRecDate.Text = "";
        TxtRemarks.Text = "";

    }
    protected void TxtPrefix_TextChanged(object sender, EventArgs e)
    {
        TxtPostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix((TxtPrefix.Text).ToUpper()));
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearAfterCompanyChange();
        ProcessSelectedChange();
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearAfterCompanyChange();
        ViewState["Process_Rec_Id"] = 0;
        ProcessSelectedChange();
    }
    private void ProcessSelectedChange()
    {
        if (DDProcessName.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref DDEmployeeNamee, "Select Distinct EI.EmpId,EI.EmpName from Empinfo EI,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM WHERE PIM.EmpId=EI.EmpId ANd EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ei.empname ", true, "--Select--");
        }
    }
    protected void DDEmployeeNamee_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ClearAfterCompanyChange();
        EmployeeSelectedChange();
    }
    private void EmployeeSelectedChange()
    {
        string sql = "";
        sql = sql + "Select Distinct PM.IssueOrderId,case When " + Session["varMasterCompanyIDForERP"] + "=9 Then Om.localOrder+'/'+cast(PM.IssueOrderId as varchar(100)) ELse cast(PM.IssueOrderId as varchar(100)) End as IssueOrderid1 from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD,ordermaster OM  Where PM.ISSUEORDERID=PD.ISSUEORDERID And PD.orderid=OM.orderid And PD.PQty<>0 And PM.status='Pending' And Empid=" + DDEmployeeNamee.SelectedValue + " And Pm.CompanyId=" + DDCompanyName.SelectedValue + "";
        if (chkboxSampleFlag.Checked == true)
        {
            sql = sql + " and PM.SampleNumber!='' ";

        }
        else
        {
            sql = sql + " and PM.SampleNumber='' ";
        }
        sql = sql + "order by PM.Issueorderid";

        ViewState["Process_Rec_Id"] = 0;
        //Fill_Grid();

        CategoryBind();
        if (DDProcessName.SelectedIndex > 0 && DDEmployeeNamee.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref DDPONo, sql, true, "--Select--");
        }
        fillorderdetail();
    }
    private void CategoryBind()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            if (DDProcessName.SelectedIndex > 0 && DDEmployeeNamee.SelectedIndex > 0)
            {

                if (Convert.ToInt32(ViewState["Process_Rec_Id"]) != 0)
                {
                    string Str = "Select IssueOrderid from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " Where EmpId=" + DDEmployeeNamee.SelectedValue + " and Status!='Canceled' And UnitId in (Select UnitId from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PIM,
                        PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PRD Where PIM.IssueOrderId=PRD.IssueOrderId And Process_Rec_Id=" + ViewState["Process_Rec_Id"] + ")";
                    DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        DDunit.SelectedValue = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Unitid From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " Where EmpId=" + DDEmployeeNamee.SelectedValue + " and Status!='Canceled'").ToString();
                        UtilityModule.ConditionalComboFill(ref DDCategoryName, "Select Distinct ICM.Category_Id,Category_Name from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PID,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM,ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,ITEM_CATEGORY_MASTER ICM inner join UserRights_Category UC on(ICM.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ")  where IM.CATEGORY_ID=ICM.CATEGORY_ID AND IPM.Item_Id=IM.Item_Id AND PID.Item_Finished_Id=IPM.Item_Finished_Id and PID.IssueOrderId=PIM.IssueOrderId and PIM.Empid=" + DDEmployeeNamee.SelectedValue + " and PIM.Status!='Canceled' And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
                        if (DDCategoryName.Items.Count > 0)
                        {
                            DDCategoryName.SelectedIndex = 1;
                            ddcategorychange();
                        }
                    }
                    else
                    {
                        DDPONo.SelectedIndex = 0;
                        llMessageBox.Visible = true;
                        llMessageBox.Text = "Pls Select Same Unit Order No.";
                    }
                }
                else
                {
                    DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Unitid,CalType From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " Where EmpId=" + DDEmployeeNamee.SelectedValue + " and Status!='Canceled'");
                    ////DDunit.SelectedValue = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Unitid From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " Where IssueOrderId=" + DDPONo.SelectedValue).ToString();
                    DDunit.SelectedValue = ds2.Tables[0].Rows[0]["Unitid"].ToString();
                    DDcaltype.SelectedValue = ds2.Tables[0].Rows[0]["CalType"].ToString();
                    UtilityModule.ConditionalComboFill(ref DDCategoryName, "Select Distinct ICM.Category_Id,Category_Name from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PID,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM,ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,ITEM_CATEGORY_MASTER ICM inner join UserRights_Category UC on(ICM.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ") where IM.CATEGORY_ID=ICM.CATEGORY_ID AND IPM.Item_Id=IM.Item_Id AND PID.Item_Finished_Id=IPM.Item_Finished_Id and PID.IssueOrderId=PIM.IssueOrderId and PIM.Empid=" + DDEmployeeNamee.SelectedValue + " and PIM.Status!='Canceled' And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");

                    if (DDCategoryName.Items.Count > 0)
                    {
                        DDCategoryName.SelectedIndex = 1;
                        ddcategorychange();
                    }
                }


            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessReceive.aspx");
            Tran.Rollback();
            llMessageBox.Visible = false;
            llMessageBox.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DDPONo_SelectedIndexChanged(object sender, EventArgs e)
    {
        PoNoSelectedChange();
    }
    private void PoNoSelectedChange()
    {

        llMessageBox.Visible = false;
        if (Convert.ToInt32(ViewState["Process_Rec_Id"]) != 0)
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select PIM.IssueOrderid From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PID,PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PRM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + " PRD Where PIM.IssueOrderId=PID.IssueOrderId And PRM.Process_Rec_Id=PRD.Process_Rec_Id And PID.Issue_Detail_Id=PRD.Issue_Detail_Id And PIM.CalType=PRM.CalType And PRM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + "");
            if (Ds.Tables[0].Rows.Count == 0)
            {
                llMessageBox.Text = "Pls Select Same CalType PoNo.";
                DDPONo.SelectedIndex = 0;
            }
        }
        fillorderdetail();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            if (DDProcessName.SelectedIndex > 0 && DDPONo.SelectedIndex > 0)
            {
                if (Convert.ToInt32(ViewState["Process_Rec_Id"]) != 0)
                {
                    string Str = "Select IssueOrderid from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " Where IssueOrderId=" + DDPONo.SelectedValue + " And UnitId in (Select UnitId from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PIM,
                        PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PRD Where PIM.IssueOrderId=PRD.IssueOrderId And Process_Rec_Id=" + ViewState["Process_Rec_Id"] + ")";
                    DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        DDunit.SelectedValue = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Unitid From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " Where IssueOrderId=" + DDPONo.SelectedValue).ToString();
                        UtilityModule.ConditionalComboFill(ref DDCategoryName, "Select Distinct ICM.Category_Id,Category_Name from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PID,ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,ITEM_CATEGORY_MASTER ICM inner join UserRights_Category UC on(ICM.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ")  where IM.CATEGORY_ID=ICM.CATEGORY_ID AND IPM.Item_Id=IM.Item_Id AND PID.Item_Finished_Id=IPM.Item_Finished_Id and PID.IssueOrderId=" + DDPONo.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
                    }
                    else
                    {
                        DDPONo.SelectedIndex = 0;
                        llMessageBox.Visible = true;
                        llMessageBox.Text = "Pls Select Same Unit Order No.";
                    }
                }
                else
                {
                    DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Unitid,CalType From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " Where IssueOrderId=" + DDPONo.SelectedValue + "");
                    ////DDunit.SelectedValue = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Unitid From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " Where IssueOrderId=" + DDPONo.SelectedValue).ToString();
                    DDunit.SelectedValue = ds2.Tables[0].Rows[0]["Unitid"].ToString();
                    DDcaltype.SelectedValue = ds2.Tables[0].Rows[0]["CalType"].ToString();
                    UtilityModule.ConditionalComboFill(ref DDCategoryName, "Select Distinct ICM.Category_Id,Category_Name from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PID,ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,ITEM_CATEGORY_MASTER ICM inner join UserRights_Category UC on(ICM.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ") where IM.CATEGORY_ID=ICM.CATEGORY_ID AND IPM.Item_Id=IM.Item_Id AND PID.Item_Finished_Id=IPM.Item_Finished_Id and PID.IssueOrderId=" + DDPONo.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
                }
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessReceive.aspx");
            Tran.Rollback();
            llMessageBox.Visible = false;
            llMessageBox.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DDCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddcategorychange();
    }
    private void ddcategorychange()
    {
        if (variable.VarNewQualitySize == "1")
        {
            UtilityModule.ConditionalComboFill(ref DDItemName, "Select Distinct Item_id, Item_Name from V_FinishedItemDetailNew VF,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM Where VF.Item_Finished_Id=PD.Item_Finished_Id And VF.Category_Id=" + DDCategoryName.SelectedValue + " And PIM.Empid=" + DDEmployeeNamee.SelectedValue + " and PIM.Status!='Canceled' And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDItemName, "Select Distinct Item_id, Item_Name from V_FinishedItemDetail VF,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD Where VF.Item_Finished_Id=PD.Item_Finished_Id And VF.Category_Id=" + DDCategoryName.SelectedValue + " And PD.IssueOrderId=" + DDPONo.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
        }
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDitemchange();
        fillorderdetail();
    }
    private void DDitemchange()
    {
        llMessageBox.Visible = false;
        string get_year = "";
        string lastTwoChars = "";
        try
        {
            if (DDItemName.SelectedIndex > 0)
            {
                TxtPrefix.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Item_Code from Item_Master Where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "").ToString();
                if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) != 5)
                {
                    get_year = DateTime.Now.ToString("dd-MMM-yyyy");
                    lastTwoChars = get_year.Substring(get_year.Length - 2);
                }
                switch (Convert.ToInt32(Session["varMasterCompanyIDForERP"]))
                {
                    case 4:
                    case 15:
                    case 16:
                        TxtPrefix.Text = TxtPrefix.Text.Replace(" ", "");
                        break;
                    default:
                        TxtPrefix.Text = (TxtPrefix.Text + "-" + lastTwoChars).Replace(" ", "");
                        break;

                }

                int VARQCTYPE = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VARQCTYPE From MasterSetting"));
                if (VARQCTYPE == 1)
                {
                    qulitychk.Visible = true;

                }
                else
                { qulitychk.Visible = false; }
            }
            else
            {
                TxtPrefix.Text = "";
            }
            TxtPostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix((TxtPrefix.Text).ToUpper()));
            Item_SelectedIndexChange();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessReceive.aspx");
            llMessageBox.Visible = true;
            llMessageBox.Text = ex.Message;
        }
    }
    private void Item_SelectedIndexChange()
    {
        string str = "";

        str = @" select Distinct Q.Qualityid,Qualityname From Quality Q,PROCESS_ISSUE_Detail_1 PID , PROCESS_ISSUE_Master_1 PIM ,ViewFindFinishedidItemidQDCSSNew VQDCSSNew
                where PIM.ISSUEORDERID=PID.ISSUEORDERID and VQDCSSNew.qualityid=q.qualityid and PIM.Empid=" + DDEmployeeNamee.SelectedValue + " And Q.Item_Id=" + DDItemName.SelectedValue + " and PIM.Status='Pending'";

        UtilityModule.ConditionalComboFill(ref DDQualityName, str, true, "-----------Select------");
    }
    protected void DDQualityName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDQualityNameChange();
        fillorderdetail();
    }
    private void DDQualityNameChange()
    {
        string str = "";
        string str1 = "";

        str = @" select Distinct D.designId,D.designName From Design D,PROCESS_ISSUE_Detail_1 PID , PROCESS_ISSUE_Master_1 PIM ,ViewFindFinishedidItemidQDCSSNew VQDCSSNew
                where PIM.ISSUEORDERID=PID.ISSUEORDERID And PID.Item_Finished_Id=FinishedId 
                and VQDCSSNew.Designid=D.designId and PIM.Empid=" + DDEmployeeNamee.SelectedValue + " And VQDCSSNew.Qualityid=" + DDQualityName.SelectedValue + " and PIM.Status='Pending'";

        UtilityModule.ConditionalComboFill(ref DDDesignName, str, true, "-----------Select------");

        str1 = @" Select distinct SizeId, case When " + DDunit.SelectedValue + "=6 Then ISNULL(Sizeinch,'') Else case When " + DDunit.SelectedValue + "=1 Then ISNULL(ProdSizeMtr,'') Else ISNULL(ProdSizeFt,'') ENd End as Size from ViewFindFinishedidItemidQDCSSNew VQDCSSNew,PROCESS_ISSUE_Detail_1 PID , PROCESS_ISSUE_Master_1 PIM Where PID.PQty>0 And PID.Item_Finished_Id=FinishedId  And ITEM_ID=" + DDItemName.SelectedValue + " and VQDCSSNew.Qualityid=" + DDQualityName.SelectedValue + "  and PIM.IssueOrderId=PID.IssueOrderId and PIM.Empid=" + DDEmployeeNamee.SelectedValue + " and PIM.Status='Pending'";

        UtilityModule.ConditionalComboFill(ref DDSize, str1, true, "-----------Select------");
    }
    protected void DDDesignName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDDesignNameChange();
        fillorderdetail();
    }
    private void DDDesignNameChange()
    {
        string str = "";

        str = @" select Distinct C.ColorId,C.ColorName From Color C,PROCESS_ISSUE_Detail_1 PID , PROCESS_ISSUE_Master_1 PIM ,ViewFindFinishedidItemidQDCSSNew VQDCSSNew
                 where PIM.ISSUEORDERID=PID.ISSUEORDERID And PID.Item_Finished_Id=FinishedId 
                 and VQDCSSNew.colorid=C.ColorId and PIM.Empid=" + DDEmployeeNamee.SelectedValue + " And VQDCSSNew.Designid=" + DDDesignName.SelectedValue + " and PIM.Status='Pending'";


        UtilityModule.ConditionalComboFill(ref DDColorName, str, true, "-----------Select------");
    }
    protected void DDColorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDColorNameChange();
        fillorderdetail();
    }
    private void DDColorNameChange()
    {

    }
    protected void DDSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDSizeChange();
        fillorderdetail();
    }
    private void DDSizeChange()
    {

    }
    protected void DDDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (llMessageBox.Text == "")
        {
            ProcessIssue();
        }
    }
    private void CHECKVALIDCONTROL()
    {
        llMessageBox.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompanyName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDProcessName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDEmployeeNamee) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtRecDate) == false)
        {
            goto a;
        }

        else
        {
            goto B;
        }
    a:
        llMessageBox.Visible = true;
        UtilityModule.SHOWMSG(llMessageBox);
    B: ;
    }
    private void ChkGVDDValidation()
    {
        llMessageBox.Text = "";
        int count = GVCarpetReceive.Rows.Count;

        if (GVCarpetReceive.Rows.Count > 0)
        {
            for (int k = 0; k < count; k++)
            {
                int JobId = ((DropDownList)(GVCarpetReceive.Rows[k].FindControl("DDJobName"))).SelectedIndex;
                //int JobId = ((DropDownList)(GVCarpetReceive.Rows[k].FindControl("DDJobName"))).SelectedIndex;
                int FinisherId = ((DropDownList)(GVCarpetReceive.Rows[k].FindControl("DDFinisherName"))).SelectedIndex;
                int Qty = Convert.ToInt32(((TextBox)(GVCarpetReceive.Rows[k].FindControl("txtReqQty"))).Text);
                if (Qty <= 0 || Qty == 0)
                {
                    llMessageBox.Visible = true;
                    llMessageBox.Text = "Please fill qty";
                }
                else if (JobId <= 0 || FinisherId <= 0)
                {
                    llMessageBox.Visible = true;
                    llMessageBox.Text = "Please select job name and finisher name";
                }
            }
        }
    }
    //*********************************************Process Issue**************************************************************************
    private void ProcessIssue()
    {
        string HKStrWL = "";
        string RoundFullArea = "";
        double BZW = 0, BZL = 0;
        ChkGVDDValidation();
        if (llMessageBox.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();

            DataTable dtrecords = new DataTable();
            if (ViewState["CarpetReceive"] != null)
            {
                dtrecords = (DataTable)ViewState["CarpetReceive"];
            }

            DataTable dt6 = new DataTable();
            dt6 = (DataTable)ViewState["CarpetReceive"];

            if (dtrecords.Rows.Count > 0)
            {
                for (int i = 0; i < dtrecords.Rows.Count; i++)
                {
                    string SrNo = dtrecords.Rows[i]["SrNo"].ToString();
                    int ShapeId = Convert.ToInt32(dtrecords.Rows[i]["ShapeId"].ToString());
                    float Area = float.Parse(dtrecords.Rows[i]["Area"].ToString());
                    int Qty = Convert.ToInt32(dtrecords.Rows[i]["Qty"].ToString());
                    string AfterKhapSize = dtrecords.Rows[i]["AfterKhapSize"].ToString();
                    float KhapWidth = float.Parse(dtrecords.Rows[i]["KhapWidth"].ToString());
                    float KhapLength = float.Parse(dtrecords.Rows[i]["KhapLength"].ToString());
                    if (ShapeId == 2)
                    {
                        if (DDcaltype.SelectedIndex >= 0)
                        {
                            HKStrWL = string.Format("{0:#0.00}", AfterKhapSize);
                            BZW = Convert.ToDouble(HKStrWL.Split('x')[0]);
                            BZL = Convert.ToDouble(HKStrWL.Split('x')[1]);
                            if (Convert.ToInt32(DDunit.SelectedValue) == 1)
                            {
                                RoundFullArea = Convert.ToString(UtilityModule.Calculate_Area_Mtr(BZL, BZW, Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(ShapeId)));
                            }
                            if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
                            {
                                RoundFullArea = Convert.ToString(UtilityModule.Calculate_Area_Ft(BZL, BZW, Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(ShapeId), UnitId: Convert.ToInt16(DDunit.SelectedValue), RoundFullAreaValue: Convert.ToDouble(chkboxRoundFullArea.Checked == true ? "1" : "0.7853")));
                            }
                        }

                        if (dt6.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt6.Rows)
                            {
                                if (row["SrNo"].ToString() == SrNo)
                                {

                                    row["Area"] = RoundFullArea;


                                    if (DDcaltype.SelectedValue == "0" || DDcaltype.SelectedValue == "2" || DDcaltype.SelectedValue == "3" || DDcaltype.SelectedValue == "4")
                                    {
                                        row["Amount"] = Convert.ToDecimal(RoundFullArea) * Convert.ToInt32(Qty) * Convert.ToDecimal(row["Rate"].ToString());
                                        row["CommAmt"] = Convert.ToDecimal(RoundFullArea) * Convert.ToInt32(Qty) * Convert.ToDecimal(row["Comm"].ToString());
                                    }
                                    if (DDcaltype.SelectedValue == "1")
                                    {
                                        row["Amount"] = Convert.ToInt32(Qty) * Convert.ToDecimal(row["Rate"].ToString());
                                        row["CommAmt"] = Convert.ToInt32(Qty) * Convert.ToDecimal(row["Comm"].ToString());
                                    }

                                }
                                dt6.AcceptChanges();
                                row.SetModified();
                                ViewState["CarpetReceive"] = dt6;

                            }
                        }
                    }


                }
            }

            dtrecords = (DataTable)ViewState["CarpetReceive"];

            DataTable dtrecordsPenSave = new DataTable();
            if (ViewState["CarpetReceivePen"] != null)
            {
                dtrecordsPenSave = (DataTable)ViewState["CarpetReceivePen"];
            }
            else if (ViewState["CarpetReceivePen"] == null)
            {
                dtrecordsPenSave.Columns.Add("SrNo", typeof(int));
                dtrecordsPenSave.Columns.Add("PenalityId", typeof(int));
                dtrecordsPenSave.Columns.Add("PenalityName", typeof(string));
                dtrecordsPenSave.Columns.Add("Qty", typeof(int));
                dtrecordsPenSave.Columns.Add("Rate", typeof(float));
                dtrecordsPenSave.Columns.Add("Amt", typeof(float));
                dtrecordsPenSave.Columns.Add("PenalityType", typeof(string));

                DataRow dr = dtrecordsPenSave.NewRow();
                dr["SrNo"] = 0;
                dr["PenalityId"] = 0;
                dr["PenalityName"] = "";
                dr["Qty"] = 0;
                dr["Rate"] = 0;
                dr["Amt"] = 0;
                dr["PenalityType"] = null;

                dtrecordsPenSave.Rows.Add(dr);
                ViewState["CarpetReceivePen"] = dtrecordsPenSave;
            }


            if (dtrecords.Rows.Count > 0)
            {
                SqlTransaction Tran = con.BeginTransaction();
                try
                {

                    SqlParameter[] _arrpara = new SqlParameter[23];
                    if (ViewState["Process_Rec_Id"] == null)
                    {
                        ViewState["Process_Rec_Id"] = 0;
                    }
                    _arrpara[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
                    _arrpara[0].Direction = ParameterDirection.InputOutput;
                    _arrpara[0].Value = ViewState["Process_Rec_Id"];
                    _arrpara[1] = new SqlParameter("@Empid", DDEmployeeNamee.SelectedValue);
                    _arrpara[2] = new SqlParameter("@ReceiveDate", TxtRecDate.Text);
                    _arrpara[3] = new SqlParameter("@Unitid", DDunit.SelectedValue);
                    _arrpara[4] = new SqlParameter("@Userid", Session["varuserid"]);
                    _arrpara[5] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 250);
                    _arrpara[5].Direction = ParameterDirection.InputOutput;
                    _arrpara[5].Value = TxtChallanNo.Text;
                    _arrpara[6] = new SqlParameter("@Companyid", DDCompanyName.SelectedValue);
                    _arrpara[7] = new SqlParameter("@Remarks", TxtRemarks.Text == "" ? "" : TxtRemarks.Text);
                    _arrpara[8] = new SqlParameter("@process_rec_detail_id", SqlDbType.Int);
                    _arrpara[8].Direction = ParameterDirection.Output;
                    _arrpara[8].Value = 0;
                    _arrpara[9] = new SqlParameter("@QualityType", 1);
                    _arrpara[10] = new SqlParameter("@CalType", DDcaltype.SelectedValue);
                    _arrpara[11] = new SqlParameter("@TDSPercentage", 0);
                    _arrpara[12] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
                    _arrpara[13] = new SqlParameter("@GatePassNo", 0);
                    _arrpara[14] = new SqlParameter("@MastercompanyId", Session["varMasterCompanyIDForERP"]);
                    _arrpara[15] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 100);
                    _arrpara[15].Direction = ParameterDirection.Output;
                    _arrpara[16] = new SqlParameter("@dtrecords", dtrecords);
                    _arrpara[17] = new SqlParameter("@dtrecordsPen", dtrecordsPenSave);
                    _arrpara[18] = new SqlParameter("@TotalWeight", txtTotalWeight.Text == "" ? "0.0" : txtTotalWeight.Text);
                    _arrpara[19] = new SqlParameter("@CheckPcs", txtCheckPcs.Text == "" ? "0" : txtCheckPcs.Text);
                    _arrpara[20] = new SqlParameter("@CheckWeight", txtCheckWeight.Text == "" ? "0.0" : txtCheckWeight.Text);

                    int chkboxsampleflag = 0;
                    if (chkboxSampleFlag.Checked == true)
                    {
                        chkboxsampleflag = 1;
                    }
                    else
                    {
                        chkboxsampleflag = 0;
                    }
                    _arrpara[21] = new SqlParameter("@Sampleflag", chkboxsampleflag);
                    //_arrpara[21] = new SqlParameter("@TDSType", 1);

                    int chkboxManualWtEntryflag = 0;
                    if (chkboxManualWeight.Checked == true)
                    {
                        chkboxManualWtEntryflag = 1;
                    }
                    else
                    {
                        chkboxManualWtEntryflag = 0;
                    }
                    _arrpara[22] = new SqlParameter("@ManualWtEntry", chkboxManualWtEntryflag);


                    string tablename = "PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + "";
                    ViewState["tablename"] = "PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + "";

                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_FirstProcessReceiveNew]", _arrpara);

                    llMessageBox.Visible = true;
                    llMessageBox.Text = _arrpara[15].Value.ToString();
                    //QCSAVE(Tran, Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[8].Value), tablename);
                    ViewState["PurchaseReceiveDetailId"] = _arrpara[8].Value;
                    llMessageBox.Text = "Data Successfully Saved.......";
                    //ViewState["Process_Rec_Id"] = _arrpara[0].Value.ToString();
                    ViewState["Process_Rec_Id"] = 0;
                    lblChallanNo.Text = _arrpara[5].Value.ToString();
                    lblProcessRecID.Text = _arrpara[0].Value.ToString();
                    Tran.Commit();

                    TxtChallanNo.Text = _arrpara[5].Value.ToString();

                    ClearAfterSave();
                    //if (DDDescription.Items.Count > 0)
                    //{
                    //    Get_Detail();
                    //    TxtRecQty.Focus();
                    //}
                }
                catch (Exception ex)
                {
                    UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessReceive.aspx");
                    Tran.Rollback();
                    ViewState["Process_Rec_Id"] = 0;
                    llMessageBox.Visible = true;
                    llMessageBox.Text = ex.Message;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }
    }
    private void ClearAfterSave()
    {

        DDEmployeeNamee.Focus();
        fillorderdetail();
        GVCarpetReceive.DataSource = null;
        GVCarpetReceive.DataBind();

        GVPenalty.DataSource = null;
        GVPenalty.DataBind();

        ViewState["CarpetReceive"] = null;
        ViewState["CarpetReceivePen"] = null;

        txtTotalWeight.Text = "";
        txtCheckPcs.Text = "";
        txtCheckWeight.Text = "";

    }
    private void CATEGORY_DEPENDS_CONTROLS()
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "select Distinct Item_id, Item_Name from Item_Master where Category_Id=" + DDCategoryName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER Where ProductCode Like  '" + prefixText + "%' And MasterCompanyId=" + MasterCompanyId;
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(strQuery, con);
        da.Fill(ds);
        count = ds.Tables[0].Rows.Count;
        con.Close();
        List<string> ProductCode = new List<string>();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            ProductCode.Add(ds.Tables[0].Rows[i][0].ToString());
        }
        con.Close();
        return ProductCode.ToArray();
    }
    protected void TxtPostfix_TextChanged(object sender, EventArgs e)
    {
        llMessageBox.Text = "";
        string TStockNo = TxtPrefix.Text + TxtPostfix.Text;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from CarpetNumber Where TStockNo='" + TStockNo + "' And CompanyId=" + DDCompanyName.SelectedValue + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            llMessageBox.Text = "Stock No AllReady Exits....";
            TxtPostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix((TxtPrefix.Text).ToUpper()));
        }
    }
    private void fillorderdetail()
    {
        string sql = "";
        if (variable.VarNewQualitySize == "1")
        {
            sql = @"Select row_number() over (order by issue_Detail_Id) as srno, PID.IssueOrderId ,Item_Name as Item, QualityName as Quality,Designname as Design,ColorName as Color,ShapeName as Shape,
                        Item_Name+'  '+QualityName+'  '+Designname+'  '+ColorName+'  '+ShapeName+'  '+Case When UnitId=1 Then ProdSizeMtr Else Case When UnitId=2 Then  ProdSizeFt Else Case When UnitId=6 Then SizeInch End End End+'  '+ShadeColorName As Description,
                        PID.width+'x'+PID.Length as Size,
                       
                       IsNull(Sum(PID.Qty),0)-Isnull(Sum(Pid.CancelQty),0) As Qty,isnull(sum(pid.PQty),0)-Isnull(Sum(CancelQty),0) as issueqqty,CATEGORY_ID,v.ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId,
                       PIM.UnitId UNIT,PID.Item_Finished_Id as finishedid,Issue_Detail_Id,PIM.AssignDate,PID.width as ProdWidthFt,PID.Length as ProdLengthFt,PID.AfterKhapSizeOrder as finishing_Ft_Size,PID.Area as Finishing_Ft_Area,
                       V.Finishing_Mt_Size,PID.Orderid,PID.Rate,PID.Comm,PIM.FlagFixOrWeight,PID.Area From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PIM,
                       PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PID,V_FinishedItemDetailNew V Where PIM.IssueOrderId=PID.IssueOrderId And 
                       pid.item_finished_id=V.Item_Finished_Id And PIM.Empid=" + DDEmployeeNamee.SelectedValue + " and PIM.Status='Pending' And V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And PIM.CompanyId=" + DDCompanyName.SelectedValue + @" Group By PID.IssueOrderId, CATEGORY_NAME,Item_Name,QualityName,Designname,
                       ColorName,ShadeColorName,ShapeName,ProdSizeMtr,ProdSizeFt,Sizeinch,UnitId,CATEGORY_ID,V.ITEM_ID,QualityId,ColorId,DesignId,SizeId,ShapeId,ShadecolorId,PID.Item_Finished_Id,PIM.AssignDate,
                        PID.width,PID.Length,PID.AfterKhapSizeOrder,V.Finishing_Mt_Size,PID.Orderid,PID.Rate,PID.Comm,PIM.FlagFixOrWeight,PID.Area,PIM.SampleNumber,Issue_Detail_Id Having isnull(sum(pid.PQty),0)>0";

            //            sql = @"Select row_number() over (order by issue_Detail_Id) as srno, PID.IssueOrderId ,Item_Name as Item, QualityName as Quality,Designname as Design,ColorName as Color,ShapeName as Shape,
            //                        CATEGORY_NAME+'  '+ Item_Name+'  '+QualityName+'  '+Designname+'  '+ColorName+'  '+ShapeName+'  '+Case When UnitId=1 Then ProdSizeMtr Else Case When UnitId=2 Then  ProdSizeFt Else Case When UnitId=6 Then SizeInch End End End+'  '+ShadeColorName As Description,
            //                        PID.width+'x'+PID.Length as Size
            //                        --Case When UnitId=1 Then ProdSizeMtr Else Case When UnitId=2 Then  ProdSizeFt Else Case When UnitId=6 Then SizeInch End End End as Size,
            //                       IsNull(Sum(PID.Qty),0)-Isnull(Sum(Pid.CancelQty),0) As Qty,isnull(sum(pid.PQty),0)-Isnull(Sum(CancelQty),0) as issueqqty,CATEGORY_ID,v.ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId,
            //                       PIM.UnitId UNIT,PID.Item_Finished_Id as finishedid,Issue_Detail_Id,PIM.AssignDate,V.ProdWidthFt,V.ProdLengthFt,
            //                        V.finishing_Ft_Size, V.Finishing_Ft_Area,V.Finishing_Mt_Size,PID.Orderid,PID.Rate,PID.Comm,PIM.FlagFixOrWeight,PID.Area From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PIM,
            //                       PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PID,V_FinishedItemDetailNew V Where PIM.IssueOrderId=PID.IssueOrderId And 
            //                       pid.item_finished_id=V.Item_Finished_Id And PIM.Empid=" + DDEmployeeNamee.SelectedValue + " and PIM.Status='Pending' And V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And PIM.CompanyId=" + DDCompanyName.SelectedValue + @" Group By PID.IssueOrderId, CATEGORY_NAME,Item_Name,QualityName,Designname,
            //                       ColorName,ShadeColorName,ShapeName,ProdSizeMtr,ProdSizeFt,Sizeinch,UnitId,CATEGORY_ID,V.ITEM_ID,QualityId,ColorId,DesignId,SizeId,ShapeId,ShadecolorId,PID.Item_Finished_Id,PIM.AssignDate,
            //                        V.ProdWidthFt,V.ProdLengthFt,V.finishing_Ft_Size, V.Finishing_Ft_Area,V.Finishing_Mt_Size,PID.Orderid,PID.Rate,PID.Comm,PIM.FlagFixOrWeight,PID.Area,Issue_Detail_Id Having isnull(sum(pid.PQty),0)>0";
        }
        if (DDPONo.SelectedIndex > 0)
        {
            sql = sql + " And pid.issueorderid=" + DDPONo.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            sql = sql + " And v.ITEM_ID=" + DDItemName.SelectedValue;
        }
        if (DDQualityName.SelectedIndex > 0)
        {
            sql = sql + " And QualityId=" + DDQualityName.SelectedValue;
        }
        if (DDDesignName.SelectedIndex > 0)
        {
            sql = sql + " And designId=" + DDDesignName.SelectedValue;
        }
        if (DDColorName.SelectedIndex > 0)
        {
            sql = sql + " And ColorId=" + DDColorName.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            sql = sql + " And SizeId=" + DDSize.SelectedValue;
        }
        if (chkboxSampleFlag.Checked == true)
        {
            sql = sql + " and PIM.SampleNumber!='' ";
        }
        else
        {
            sql = sql + " and PIM.SampleNumber='' ";
        }
        sql = sql + "order by PID.IssueOrderId asc";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            dgorder.DataSource = ds;
            dgorder.DataBind();
            dgorder.Visible = true;
        }
        else
        {
            dgorder.Visible = false;
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    public string getgiven(string strval, string strval1, string strval2)
    {
        string val = "0";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(sum(qty),0) from PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + " where Issue_Detail_Id=" + strval2 + "  and item_finished_id=" + strval1 + "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            val = Convert.ToString(Convert.ToInt32(strval) - Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()));
        }
        return val;
    }
    private void ChkItemFinishedId(int FinishedId)
    {
        int count = GVCarpetReceive.Rows.Count;
        string ItemId = "";
        string Qualityid = "";
        string Designid = "";

        if (GVCarpetReceive.Rows.Count > 0)
        {
            for (int k = 0; k < count; k++)
            {
                DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Item_id, Qualityid,DesignId from ViewFindFinishedidItemidQDCSSNew Where FinishedId=" + FinishedId + " ");
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    Qualityid = Ds.Tables[0].Rows[0]["QualityId"].ToString();
                    Designid = Ds.Tables[0].Rows[0]["DesignId"].ToString();
                    ItemId = Ds.Tables[0].Rows[0]["Item_id"].ToString();
                }

                string GLQualityId = ((Label)(GVCarpetReceive.Rows[k].FindControl("lblQualityid"))).Text;
                string GLItemId = ((Label)(GVCarpetReceive.Rows[k].FindControl("lblItemId"))).Text;
                if (GLQualityId != Qualityid || GLItemId != ItemId)
                {
                    llMessageBox.Visible = true;
                    llMessageBox.Text = "Sorry You Can't Receive More Than One Quality";
                }
            }
        }
    }
    public Boolean BQty(int IssueDetailId, int rowindex)
    {
        llMessageBox.Visible = false;
        llMessageBox.Text = "";

        int Balqty = 0;
        int Issuedqty = 0;
        int Count2 = dgorder.Rows.Count;
        int Count3 = GVCarpetReceive.Rows.Count;

        if (dgorder.Rows.Count > 0)
        {
            for (int k = 0; k < Count2; k++)
            {
                string GLQualityId = ((Label)(dgorder.Rows[k].FindControl("lblQualityid"))).Text;
                string GLItemId = ((Label)(dgorder.Rows[k].FindControl("lblitem_id"))).Text;
                string IssueDetailId2 = ((Label)(dgorder.Rows[k].FindControl("Issue_Detail_Id"))).Text;
                string lblbalnce2 = ((Label)(dgorder.Rows[k].FindControl("lblbalnce"))).Text;
                string hnBalanceQty = ((Label)(dgorder.Rows[k].FindControl("hnBalanceQty"))).Text;

                if (IssueDetailId == Convert.ToInt32(IssueDetailId2))
                {
                    lblbalnce2 = Convert.ToString(Convert.ToInt32(hnBalanceQty) - sum);
                    Balqty = Convert.ToInt32(hnBalanceQty);
                }
            }
        }
        if (GVCarpetReceive.Rows.Count > 0)
        {
            for (int k = 0; k < Count3; k++)
            {
                string Issue_Detail_Id = ((Label)(GVCarpetReceive.Rows[k].FindControl("Issue_Detail_Id"))).Text;
                string txtReqQty = ((TextBox)(GVCarpetReceive.Rows[k].FindControl("txtReqQty"))).Text;

                if (IssueDetailId == Convert.ToInt32(Issue_Detail_Id))
                {
                    Issuedqty += Convert.ToInt32(txtReqQty);
                }
            }
        }
        if (Issuedqty > Balqty)
        {
            llMessageBox.Visible = true;
            llMessageBox.Text = "Receive Qty Cannot greater than order qty!!";
            TextBox RecQty = (TextBox)GVCarpetReceive.Rows[rowindex].FindControl("txtReqQty");
            RecQty.Text = "";
            return false;
        }
        else
        {
            if (dgorder.Rows.Count > 0)
            {
                for (int k = 0; k < Count2; k++)
                {
                    string IssueDetailId2 = ((Label)(dgorder.Rows[k].FindControl("Issue_Detail_Id"))).Text;
                    Label lblbalnce2 = ((Label)(dgorder.Rows[k].FindControl("lblbalnce")));

                    if (IssueDetailId == Convert.ToInt32(IssueDetailId2))
                    {
                        lblbalnce2.Text = Convert.ToString(Balqty - Issuedqty);
                    }
                }
            }
        }
        return true;
    }
    protected void dgorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        //****************sql Table Type 

        tbsample.ActiveTabIndex = 1;

        llMessageBox.Visible = false;
        llMessageBox.Text = "";
        int max = 0;
        int srno;
        int TypeId = 1;

        DataTable dtrecords = new DataTable();

        if (ViewState["CarpetReceive"] == null)
        {
            //dtrecords = new DataTable();
            dtrecords.Columns.Add("SrNo", typeof(int));
            dtrecords.Columns.Add("ItemName", typeof(string));
            dtrecords.Columns.Add("Quality", typeof(string));
            dtrecords.Columns.Add("Design", typeof(string));
            dtrecords.Columns.Add("Color", typeof(string));
            dtrecords.Columns.Add("Shape", typeof(string));
            dtrecords.Columns.Add("Size", typeof(string));
            dtrecords.Columns.Add("KhapWidth", typeof(string));
            dtrecords.Columns.Add("KhapLength", typeof(string));
            dtrecords.Columns.Add("AfterKhapSize", typeof(string));
            dtrecords.Columns.Add("Qty", typeof(int));
            dtrecords.Columns.Add("Area", typeof(float));
            dtrecords.Columns.Add("JobName", typeof(string));
            dtrecords.Columns.Add("FinisherName", typeof(string));
            dtrecords.Columns.Add("Type", typeof(int));
            dtrecords.Columns.Add("Penality", typeof(string));
            dtrecords.Columns.Add("FinishingSize", typeof(string));
            dtrecords.Columns.Add("WOrderId", typeof(int));
            dtrecords.Columns.Add("ItemId", typeof(int));
            dtrecords.Columns.Add("QualityId", typeof(int));
            dtrecords.Columns.Add("ShapeId", typeof(int));
            dtrecords.Columns.Add("Issue_Detail_Id", typeof(int));
            dtrecords.Columns.Add("hnKhapWidth", typeof(float));
            dtrecords.Columns.Add("hnKhapLength", typeof(float));
            dtrecords.Columns.Add("hnAfterKhapSize", typeof(string));
            dtrecords.Columns.Add("hnArea", typeof(float));
            dtrecords.Columns.Add("BalQty", typeof(int));
            dtrecords.Columns.Add("FinishedId", typeof(int));
            dtrecords.Columns.Add("Rate", typeof(float));
            dtrecords.Columns.Add("Amount", typeof(float));
            dtrecords.Columns.Add("Comm", typeof(float));
            dtrecords.Columns.Add("CommAmt", typeof(float));
            dtrecords.Columns.Add("IssueOrderId", typeof(int));
            dtrecords.Columns.Add("OrderID", typeof(int));
            dtrecords.Columns.Add("FlagFixOrWeight", typeof(int));
            dtrecords.Columns.Add("Description", typeof(string));
            dtrecords.Columns.Add("RecWt", typeof(float));
            dtrecords.Columns.Add("ActualWt", typeof(float));
            dtrecords.Columns.Add("FinalWt", typeof(float));
            dtrecords.Columns.Add("TotalLagat", typeof(float));

            ViewState["CarpetReceive"] = dtrecords;
        }
        else
        {
            dtrecords = (DataTable)ViewState["CarpetReceive"];
        }


        int r = Convert.ToInt32(dgorder.SelectedIndex.ToString());

        if (r >= 0)
        {
            string lblbalnce = ((Label)dgorder.Rows[r].FindControl("lblbalnce")).Text;
            if (Convert.ToInt32(lblbalnce) > 0 && lblbalnce != "")
            {
                string category = ((Label)dgorder.Rows[r].FindControl("lblcategoryid")).Text;
                string Item = ((Label)dgorder.Rows[r].FindControl("lblitem_id")).Text;
                string Issue_Detail_Id = ((Label)dgorder.Rows[r].FindControl("Issue_Detail_Id")).Text;
                string lblFinishedId = ((Label)dgorder.Rows[r].FindControl("lblFinishedId")).Text;
                string lblQualityid = ((Label)dgorder.Rows[r].FindControl("lblQualityid")).Text;

                string lblItemName = ((Label)dgorder.Rows[r].FindControl("lblItemName")).Text;
                string lblQuality = ((Label)dgorder.Rows[r].FindControl("lblQuality")).Text;
                string lblDesign = ((Label)dgorder.Rows[r].FindControl("lblDesign")).Text;
                string lblColor = ((Label)dgorder.Rows[r].FindControl("lblColor")).Text;
                string lblShape = ((Label)dgorder.Rows[r].FindControl("lblShape")).Text;
                string lblSize = ((Label)dgorder.Rows[r].FindControl("lblSize")).Text;
                string lblQty = ((Label)dgorder.Rows[r].FindControl("lblQty")).Text;

                string lblProdWidthFt = ((Label)dgorder.Rows[r].FindControl("lblProdWidthFt")).Text;
                string lblProdLengthFt = ((Label)dgorder.Rows[r].FindControl("lblProdLengthFt")).Text;
                string lblFinishingFtSize = ((Label)dgorder.Rows[r].FindControl("lblFinishingFtSize")).Text;
                string lblFinishingFtArea = ((Label)dgorder.Rows[r].FindControl("lblFinishingFtArea")).Text;
                string lblFinishingMtSize = ((Label)dgorder.Rows[r].FindControl("lblFinishingMtSize")).Text;
                string lblshapeid = ((Label)dgorder.Rows[r].FindControl("lblshapeid")).Text;
                string IssueDetailId = ((Label)dgorder.Rows[r].FindControl("Issue_Detail_Id")).Text;
                Label lblBalanceQty = (Label)dgorder.Rows[r].FindControl("lblbalnce");
                string lblIssueOrderId = ((Label)dgorder.Rows[r].FindControl("lblIssueOrderId")).Text;

                string lblRate = ((Label)dgorder.Rows[r].FindControl("lblRate")).Text;
                string lblComm = ((Label)dgorder.Rows[r].FindControl("lblComm")).Text;
                string lblOrderid = ((Label)dgorder.Rows[r].FindControl("lblOrderid")).Text;
                string lblFlagFixOrWeight = ((Label)dgorder.Rows[r].FindControl("lblFlagFixOrWeight")).Text;
                string lblDescription = ((Label)dgorder.Rows[r].FindControl("lblDescription")).Text;


                //int count2 = dtrecords.Rows.Count;
                //if (dtrecords.Rows.Count > 0)
                //{
                //    for (int k = 0; k < count2; k++)
                //    {
                //        llMessageBox.Visible = false;
                //        llMessageBox.Text = "";
                //        string issuedetailid =dtrecords.Rows[k]["Issue_Detail_Id"].ToString();
                //        if (IssueDetailId == issuedetailid)
                //        {
                //            sum = Convert.ToInt32(dtrecords.Compute("SUM(Qty)", "ItemId=" + Item + " and QualityId=" + lblQualityid + " and Issue_Detail_Id=" + IssueDetailId));                            
                //        }
                //    } 
                //}

                ChkItemFinishedId(Convert.ToInt32(lblFinishedId));

                if (Convert.ToInt32(lblbalnce) == 0)
                {
                    //ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Receive Qty Cannot greater than order qty!');", true);
                    llMessageBox.Visible = true;
                    llMessageBox.Text = "Receive Qty Cannot greater than order qty!!";
                }
                else
                {
                    if (dtrecords.Rows.Count > 0)
                    {
                        max = (int)dtrecords.Compute("MAX(SrNo)", "");
                    }

                    if (max == 0)
                    {
                        srno = 1;
                    }
                    else
                    {
                        srno = max + 1;
                    }


                    if (llMessageBox.Text == "")
                    {

                        //**************
                        DataRow dr = dtrecords.NewRow();
                        dr["SrNo"] = srno;
                        dr["ItemName"] = lblItemName;
                        dr["Quality"] = lblQuality;
                        dr["Design"] = lblDesign;
                        dr["Color"] = lblColor;
                        dr["Shape"] = lblShape;
                        dr["Size"] = lblSize;
                        dr["KhapWidth"] = lblProdWidthFt;
                        dr["KhapLength"] = lblProdLengthFt;
                        dr["AfterKhapSize"] = lblFinishingFtSize;
                        //dr["Qty"] = Convert.ToInt32(lblbalnce);
                        dr["Qty"] = 0;
                        dr["Area"] = lblFinishingFtArea;
                        dr["JobName"] = "";
                        dr["FinisherName"] = "";
                        dr["Type"] = 1;
                        dr["Penality"] = null;
                        dr["FinishingSize"] = lblFinishingMtSize;
                        dr["WOrderId"] = lblIssueOrderId;
                        dr["ItemId"] = Item;
                        dr["QualityId"] = lblQualityid;
                        dr["ShapeId"] = lblshapeid;
                        dr["Issue_Detail_Id"] = IssueDetailId;
                        dr["hnKhapWidth"] = lblProdWidthFt;
                        dr["hnKhapLength"] = lblProdLengthFt;
                        dr["hnAfterKhapSize"] = lblFinishingFtSize;
                        dr["hnArea"] = lblFinishingFtArea;
                        dr["BalQty"] = Convert.ToInt32(lblbalnce) - sum;

                        dr["FinishedId"] = lblFinishedId;
                        dr["Rate"] = lblRate;
                        dr["Amount"] = Convert.ToDecimal(lblFinishingFtArea) * Convert.ToInt32(lblbalnce) * Convert.ToDecimal(lblRate);
                        dr["Comm"] = lblComm;
                        if (DDcaltype.SelectedValue == "0" || DDcaltype.SelectedValue == "2" || DDcaltype.SelectedValue == "3" || DDcaltype.SelectedValue == "4")
                        {
                            dr["Amount"] = Convert.ToDecimal(lblFinishingFtArea) * Convert.ToInt32(lblbalnce) * Convert.ToDecimal(lblRate);
                            dr["CommAmt"] = Convert.ToDecimal(lblFinishingFtArea) * Convert.ToInt32(lblbalnce) * Convert.ToDecimal(lblComm);
                        }
                        if (DDcaltype.SelectedValue == "1")
                        {
                            dr["Amount"] = Convert.ToInt32(lblbalnce) * Convert.ToDecimal(lblRate);
                            dr["CommAmt"] = Convert.ToInt32(lblbalnce) * Convert.ToDecimal(lblComm);
                        }

                        dr["IssueOrderId"] = lblIssueOrderId;
                        dr["OrderID"] = lblOrderid;
                        //dr["FlagFixOrWeight"] = lblFlagFixOrWeight;
                        dr["FlagFixOrWeight"] = RBLConsumption.SelectedValue;
                        dr["Description"] = lblDescription;
                        dr["RecWt"] = "0";
                        dr["ActualWt"] = "0";
                        dr["FinalWt"] = "0";
                        dr["TotalLagat"] = UtilityModule.BAZAAR_CONSUMPTION_FOR_ACTUAL_WEIGHT(Convert.ToInt32(lblFinishedId), Convert.ToInt32(DDunit.SelectedValue), TypeId: TypeId, effectivedate: TxtRecDate.Text).ToString();


                        dtrecords.Rows.Add(dr);
                        ViewState["CarpetReceive"] = dtrecords;

                        //lblBalanceQty.Text = "0";

                    }

                }
            }
            else
            {
                llMessageBox.Text = "No Pending Qty";
                llMessageBox.Visible = true;
            }
        }
        DataTable dt5 = new DataTable();
        dt5 = (DataTable)ViewState["CarpetReceive"];

        GVReceiveCount = dt5.Rows.Count;

        GVCarpetReceive.DataSource = dt5;
        GVCarpetReceive.DataBind();

    }
    protected void dgorder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgorder, "Select$" + e.Row.RowIndex);
        }
    }
    decimal TotalQty = 0;
    decimal TotalArea = 0;
    protected void GVCarpetReceive_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int GDCount = GVReceiveCount - 1;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.FindControl("txtKhapWidth").Focus();
            TextBox txtReqQty = (TextBox)e.Row.FindControl("txtReqQty");
            Label lblQty = (Label)e.Row.FindControl("lblQty");
            TotalQty += Convert.ToDecimal(lblQty.Text);
            Label lblArea = (Label)e.Row.FindControl("lblArea");
            TotalArea += Convert.ToDecimal(lblArea.Text) * Convert.ToDecimal(lblQty.Text);

            Label lblTotalArea = (Label)e.Row.FindControl("lblTotalArea");
            lblTotalArea.Text = Convert.ToString(Convert.ToDecimal(lblArea.Text) * Convert.ToDecimal(txtReqQty.Text));

            Label lblPenalityName = (Label)e.Row.FindControl("lblPenalityName");

            Label lblSrNo = (Label)e.Row.FindControl("lblSrNo");
            Label lblJobName = (Label)e.Row.FindControl("lblJobName");
            Label lblFinisherName = (Label)e.Row.FindControl("lblFinisherName");
            DropDownList DDJobName = ((DropDownList)e.Row.FindControl("DDJobName"));
            DropDownList DDFinisherName = ((DropDownList)e.Row.FindControl("DDFinisherName"));

            UtilityModule.ConditionalComboFill(ref DDJobName, "select PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER where ProcessType=1 and PROCESS_NAME_ID!=1 order by PROCESS_NAME_ID", true, "--Select--");

            if (DDJobName.SelectedIndex > 0)
            {
                UtilityModule.ConditionalComboFill(ref DDFinisherName, "select EI.EmpId,EI.EmpName from EmpProcess EP INNER JOIN EmpInfo EI ON EP.EmpId=EI.EmpId  where Processid=" + DDJobName.SelectedValue, true, "--Select--");
            }

            //if (lblJobName.Text!=null || lblJobName.Text!="")
            //{
            // Get the data from DB and bind the dropdownlist
            DDJobName.SelectedValue = lblJobName.Text;
            UtilityModule.ConditionalComboFill(ref DDFinisherName, "select EI.EmpId,EI.EmpName from EmpProcess EP INNER JOIN EmpInfo EI ON EP.EmpId=EI.EmpId  where Processid=" + DDJobName.SelectedValue, true, "--Select--");
            DDFinisherName.SelectedValue = lblFinisherName.Text;
            // }           


            TextBox txtRecWt = (TextBox)e.Row.FindControl("txtRecWt");
            Label lblActualWt = (Label)e.Row.FindControl("lblActualWt");
            Label lblRecWt = (Label)e.Row.FindControl("lblRecWt");
            Label lblFinalWt = (Label)e.Row.FindControl("lblFinalWt");
            Label lblTotalLagat = (Label)e.Row.FindControl("lblTotalLagat");
            Label lblFinishedId = (Label)e.Row.FindControl("lblFinishedId");
            if (chkboxSampleFlag.Checked == false)
            {
                lblActualWt.Text = Convert.ToString(Math.Round(Convert.ToDouble(lblTotalLagat.Text) * Convert.ToDouble(lblArea.Text) * Convert.ToInt32(txtReqQty.Text), 3));
            }
            else
            {
                lblActualWt.Text = "0";
            }

            if (chkboxSampleFlag.Checked == false)
            {
                if (Convert.ToDouble(txtRecWt.Text) > Convert.ToDouble(lblActualWt.Text))
                {
                    lblFinalWt.Text = lblActualWt.Text;
                }
                else
                {
                    lblFinalWt.Text = txtRecWt.Text;
                }
            }
            else
            {
                lblFinalWt.Text = txtRecWt.Text;
            }


            if (e.Row.RowIndex == GDCount)
            {
                if (e.Row.RowIndex > 0)
                {
                    //string jobid = ((DropDownList)GVCarpetReceive.Rows[e.Row.RowIndex - 1].FindControl("DDJobName")).SelectedValue;
                    //string FinId = ((DropDownList)GVCarpetReceive.Rows[e.Row.RowIndex - 1].FindControl("DDFinisherName")).SelectedValue;

                    string jobid = ((Label)GVCarpetReceive.Rows[e.Row.RowIndex - 1].FindControl("lblJobName")).Text;
                    string FinId = ((Label)GVCarpetReceive.Rows[e.Row.RowIndex - 1].FindControl("lblFinisherName")).Text;
                    DDJobName.SelectedValue = jobid;
                    UtilityModule.ConditionalComboFill(ref DDFinisherName, "select EI.EmpId,EI.EmpName from EmpProcess EP INNER JOIN EmpInfo EI ON EP.EmpId=EI.EmpId  where Processid=" + DDJobName.SelectedValue, true, "--Select--");
                    DDFinisherName.SelectedValue = FinId;

                    DataTable dt3 = new DataTable();
                    dt3 = (DataTable)ViewState["CarpetReceive"];

                    if (dt3.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt3.Rows)
                        {
                            if (row["SrNo"].ToString() == lblSrNo.Text)
                            {
                                row["JobName"] = DDJobName.SelectedIndex > 0 ? DDJobName.SelectedValue : "0";
                                UtilityModule.ConditionalComboFill(ref DDFinisherName, "select EI.EmpId,EI.EmpName from EmpProcess EP INNER JOIN EmpInfo EI ON EP.EmpId=EI.EmpId  where Processid=" + DDJobName.SelectedValue, true, "--Select--");
                                DDFinisherName.SelectedValue = FinId;
                                row["FinisherName"] = DDFinisherName.SelectedIndex > 0 ? DDFinisherName.SelectedValue : "0";
                                lblPenalityName.Text = row["Penality"].ToString();
                            }
                            dt3.AcceptChanges();
                            row.SetModified();
                            ViewState["CarpetReceive"] = dt3;
                        }
                    }
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblGrandTQty = (Label)e.Row.FindControl("lblGrandTQty");
            lblGrandTQty.Text = TotalQty.ToString();
            Label lblGrandTArea = (Label)e.Row.FindControl("lblGrandTArea");
            lblGrandTArea.Text = TotalArea.ToString();
        }
    }
    protected void test(object sender)
    {
        string HKStrWL = "";
        double BZW = 0, BZL = 0;
        TextBox txt = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
        Label lblSrNo = (Label)gvRow.FindControl("lblSrNo");
        Label lblShape = (Label)gvRow.FindControl("lblShape");
        Label lblKhapWidth = (Label)gvRow.FindControl("lblKhapWidth");
        Label lblKhapLength = (Label)gvRow.FindControl("lblKhapLength");
        Label lblAfterKhapSize = (Label)gvRow.FindControl("lblAfterKhapSize");

        Label lblArea = (Label)gvRow.FindControl("lblArea");
        Label lblShapeId = (Label)gvRow.FindControl("lblShapeId");

        TextBox TxtKhapWidth = (TextBox)gvRow.FindControl("TxtKhapWidth");
        TextBox TxtKhapLength = (TextBox)gvRow.FindControl("TxtKhapLength");
        TextBox TxtAfterKhapSize = (TextBox)gvRow.FindControl("TxtAfterKhapSize");

        Label hnKhapWidth = (Label)gvRow.FindControl("hnKhapWidth");
        Label hnKhapLength = (Label)gvRow.FindControl("hnKhapLength");
        Label hnAfterKhapSize = (Label)gvRow.FindControl("hnAfterKhapSize");

        TextBox txtReqQty = (TextBox)gvRow.FindControl("txtReqQty");
        DropDownList DDJobName = (DropDownList)gvRow.FindControl("DDJobName");
        DropDownList DDFinisherName = (DropDownList)gvRow.FindControl("DDFinisherName");
        Label lblPenality = (Label)gvRow.FindControl("lblPenality");
        Label lblPenalityName = (Label)gvRow.FindControl("lblPenalityName");
        Label lblJobName = (Label)gvRow.FindControl("lblJobName");
        Label lblFinisherName = (Label)gvRow.FindControl("lblFinisherName");

        Label Item = (Label)gvRow.FindControl("lblItemId");
        Label lblQualityid = (Label)gvRow.FindControl("lblQualityId");
        Label IssueDetailId = (Label)gvRow.FindControl("Issue_Detail_Id");

        TextBox txtRecWt = (TextBox)gvRow.FindControl("txtRecWt");
        Label lblActualWt = (Label)gvRow.FindControl("lblActualWt");
        Label lblRecWt = (Label)gvRow.FindControl("lblRecWt");
        Label lblFinalWt = (Label)gvRow.FindControl("lblFinalWt");
        Label lblTotalLagat = (Label)gvRow.FindControl("lblTotalLagat");

        if (Convert.ToDouble(TxtKhapWidth.Text == "" ? "0" : TxtKhapWidth.Text) > 0 && Convert.ToDouble(TxtKhapLength.Text == "" ? "0" : TxtKhapLength.Text) > 0)
        {
            lblAfterKhapSize.Text = Convert.ToString(UtilityModule.Calculate_Process_Receive_Area(Convert.ToDouble(TxtKhapWidth.Text), Convert.ToDouble(TxtKhapLength.Text), lblAfterKhapSize.Text, Convert.ToInt32(DDunit.SelectedValue), Convert.ToDouble(hnKhapWidth.Text), Convert.ToDouble(hnKhapLength.Text), hnAfterKhapSize.Text));

        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Plz Enter Proper Size!');", true);
        }

        if (DDcaltype.SelectedIndex >= 0)
        {
            HKStrWL = string.Format("{0:#0.00}", lblAfterKhapSize.Text);
            BZW = Convert.ToDouble(HKStrWL.Split('x')[0]);
            BZL = Convert.ToDouble(HKStrWL.Split('x')[1]);
            if (Convert.ToInt32(DDunit.SelectedValue) == 1)
            {
                lblArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(BZL, BZW, Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(lblShapeId.Text)));
            }
            if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
            {
                lblArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(BZL, BZW, Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(lblShapeId.Text), UnitId: Convert.ToInt16(DDunit.SelectedValue), RoundFullAreaValue: Convert.ToDouble(chkboxRoundFullArea.Checked == true ? "1" : "0.7853")));
            }
        }

        ////''*******Start Calculate Actual Weight
        if (chkboxSampleFlag.Checked == false)
        {
            lblActualWt.Text = Convert.ToString(Math.Round(Convert.ToDouble(lblTotalLagat.Text) * Convert.ToDouble(lblArea.Text) * Convert.ToInt32(txtReqQty.Text), 3));
        }
        else
        {
            lblActualWt.Text = "0";
        }

        if (chkboxSampleFlag.Checked == false)
        {
            if (Convert.ToDouble(txtRecWt.Text) > Convert.ToDouble(lblActualWt.Text))
            {
                lblFinalWt.Text = lblActualWt.Text;
            }
            else
            {
                lblFinalWt.Text = txtRecWt.Text;
            }

        }
        else
        {
            lblFinalWt.Text = txtRecWt.Text;
        }
        ////''*******End Calculate Actual Weight
        lblPenalityName.Text = "";

        DataTable dt3 = new DataTable();
        dt3 = (DataTable)ViewState["CarpetReceive"];

        int sum = 0;
        sum = Convert.ToInt32(dt3.Compute("SUM(Qty)", "ItemId=" + Item.Text + " and QualityId=" + lblQualityid.Text + " and Issue_Detail_Id=" + IssueDetailId.Text));

        if (dt3.Rows.Count > 0)
        {
            foreach (DataRow row in dt3.Rows)
            {
                if (row["SrNo"].ToString() == lblSrNo.Text)
                {
                    row["KhapWidth"] = string.Format("{0:#0.00}", TxtKhapWidth.Text); ;
                    row["KhapLength"] = string.Format("{0:#0.00}", TxtKhapLength.Text);
                    row["AfterKhapSize"] = lblAfterKhapSize.Text;
                    row["Area"] = lblArea.Text;
                    row["Qty"] = txtReqQty.Text == "" ? "0" : txtReqQty.Text;
                    row["JobName"] = DDJobName.SelectedIndex > 0 ? DDJobName.SelectedValue : "0";
                    row["FinisherName"] = DDFinisherName.SelectedIndex > 0 ? DDFinisherName.SelectedValue : "0";
                    row["Penality"] = "";
                    //row["Penality"] = lblPenality.Text;

                    if (DDcaltype.SelectedValue == "0" || DDcaltype.SelectedValue == "2" || DDcaltype.SelectedValue == "3" || DDcaltype.SelectedValue == "4")
                    {
                        row["Amount"] = Convert.ToDecimal(lblArea.Text) * Convert.ToInt32(txtReqQty.Text) * Convert.ToDecimal(row["Rate"].ToString());
                        row["CommAmt"] = Convert.ToDecimal(lblArea.Text) * Convert.ToInt32(txtReqQty.Text) * Convert.ToDecimal(row["Comm"].ToString());
                    }
                    if (DDcaltype.SelectedValue == "1")
                    {
                        row["Amount"] = Convert.ToInt32(txtReqQty.Text) * Convert.ToDecimal(row["Rate"].ToString());
                        row["CommAmt"] = Convert.ToInt32(txtReqQty.Text) * Convert.ToDecimal(row["Comm"].ToString());
                    }
                    if (DDJobName.SelectedIndex > 0)
                    {
                        lblJobName.Text = row["JobName"].ToString();
                    }
                    if (DDFinisherName.SelectedIndex > 0)
                    {
                        lblFinisherName.Text = row["FinisherName"].ToString();
                    }

                    row["RecWt"] = txtRecWt.Text == "" ? "0" : txtRecWt.Text;
                    row["ActualWt"] = lblActualWt.Text == "" ? "0" : lblActualWt.Text;
                    row["FinalWt"] = lblFinalWt.Text == "" ? "0" : lblFinalWt.Text;
                }
                dt3.AcceptChanges();
                row.SetModified();
                ViewState["CarpetReceive"] = dt3;

                CarpetPenalityDel(Convert.ToInt32(lblSrNo.Text));
            }
        }
    }
    decimal TotalQty2 = 0;
    decimal TotalArea2 = 0;
    decimal TotalWeight = 0;
    protected void CalQtyTotalArea()
    {

        for (int i = 0; i < GVCarpetReceive.Rows.Count; i++)
        {
            TextBox txtReqQty = ((TextBox)GVCarpetReceive.Rows[i].FindControl("txtReqQty"));
            TotalQty2 += Convert.ToDecimal(txtReqQty.Text == "" ? "0" : txtReqQty.Text);
            Label lblArea = ((Label)GVCarpetReceive.Rows[i].FindControl("lblArea"));
            TotalArea2 += Convert.ToDecimal(lblArea.Text) * Convert.ToDecimal(txtReqQty.Text == "" ? "0" : txtReqQty.Text);

            Label lblFinalWt = ((Label)GVCarpetReceive.Rows[i].FindControl("lblFinalWt"));
            TotalWeight += Convert.ToDecimal(lblFinalWt.Text);

            Label lblTotalArea = ((Label)GVCarpetReceive.Rows[i].FindControl("lblTotalArea"));
            lblTotalArea.Text = Convert.ToString(Convert.ToDecimal(lblArea.Text) * Convert.ToDecimal(txtReqQty.Text == "" ? "0" : txtReqQty.Text));

        }

        Label lblGrandTQty = (Label)GVCarpetReceive.FooterRow.FindControl("lblGrandTQty");
        lblGrandTQty.Text = TotalQty2.ToString();
        Label lblGrandTArea = (Label)GVCarpetReceive.FooterRow.FindControl("lblGrandTArea");
        lblGrandTArea.Text = string.Format("{0:#0.000}", TotalArea2).ToString();

        txtTotalWeight.Text = Convert.ToString(TotalWeight);
    }
    protected void txtKhapWidth_TextChanged(object sender, EventArgs e)
    {
        test(sender);
        CalQtyTotalArea();
    }
    protected void txtKhapLength_TextChanged(object sender, EventArgs e)
    {
        test(sender);
        CalQtyTotalArea();
    }
    protected void txtRecWt_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
        txt.Focus();

        Label lblSrNo = (Label)gvRow.FindControl("lblSrNo");
        Label lblShape = (Label)gvRow.FindControl("lblShape");

        TextBox txtRecWt = (TextBox)gvRow.FindControl("txtRecWt");
        TextBox txtReqQty = (TextBox)gvRow.FindControl("txtReqQty");

        Label hnBalQty = (Label)gvRow.FindControl("hnBalQty");
        Label lblArea = (Label)gvRow.FindControl("lblArea");
        Label lblActualWt = (Label)gvRow.FindControl("lblActualWt");
        Label lblRecWt = (Label)gvRow.FindControl("lblRecWt");
        Label lblFinalWt = (Label)gvRow.FindControl("lblFinalWt");
        Label lblTotalLagat = (Label)gvRow.FindControl("lblTotalLagat");
        Label lblFinishedId = (Label)gvRow.FindControl("lblFinishedId");

        ////''*******Start Calculate Actual Weight
        if (chkboxSampleFlag.Checked == false)
        {
            lblActualWt.Text = Convert.ToString(Math.Round(Convert.ToDouble(lblTotalLagat.Text) * Convert.ToDouble(lblArea.Text) * Convert.ToInt32(txtReqQty.Text), 3));
        }
        else
        {
            lblActualWt.Text = "0";
        }

        if (chkboxSampleFlag.Checked == false)
        {
            if (Convert.ToDouble(txtRecWt.Text) > Convert.ToDouble(lblActualWt.Text))
            {
                lblFinalWt.Text = lblActualWt.Text;
            }
            else
            {
                lblFinalWt.Text = txtRecWt.Text;
            }

        }
        else
        {
            lblFinalWt.Text = txtRecWt.Text;
        }
        ////''*******End Calculate Actual Weight

        DataTable dt3 = new DataTable();
        dt3 = (DataTable)ViewState["CarpetReceive"];

        // int sum = 0;
        //sum = Convert.ToInt32(dt3.Compute("SUM(Qty)", "ItemId=" + Item.Text + " and QualityId=" + lblQualityid.Text + " and Issue_Detail_Id=" + IssueDetailId.Text));


        if (dt3.Rows.Count > 0)
        {
            foreach (DataRow row in dt3.Rows)
            {
                if (row["SrNo"].ToString() == lblSrNo.Text)
                {
                    row["RecWt"] = txtRecWt.Text == "" ? "0" : txtRecWt.Text;
                    row["ActualWt"] = lblActualWt.Text == "" ? "0" : lblActualWt.Text;
                    row["FinalWt"] = lblFinalWt.Text == "" ? "0" : lblFinalWt.Text;

                }
                dt3.AcceptChanges();
                row.SetModified();
                ViewState["CarpetReceive"] = dt3;
            }
        }

        CalQtyTotalArea();


    }
    protected void txtReqQty_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
        txt.Focus();

        Label lblSrNo = (Label)gvRow.FindControl("lblSrNo");
        Label lblShape = (Label)gvRow.FindControl("lblShape");

        TextBox txtReqQty = (TextBox)gvRow.FindControl("txtReqQty");

        Label Item = (Label)gvRow.FindControl("lblItemId");
        Label lblQualityid = (Label)gvRow.FindControl("lblQualityId");
        Label IssueDetailId = (Label)gvRow.FindControl("Issue_Detail_Id");
        Label hnBalQty = (Label)gvRow.FindControl("hnBalQty");
        Label lblArea = (Label)gvRow.FindControl("lblArea");
        Label lblPenalityName = (Label)gvRow.FindControl("lblPenalityName");
        lblPenalityName.Text = "";

        TextBox txtRecWt = (TextBox)gvRow.FindControl("txtRecWt");
        Label lblActualWt = (Label)gvRow.FindControl("lblActualWt");
        Label lblRecWt = (Label)gvRow.FindControl("lblRecWt");
        Label lblFinalWt = (Label)gvRow.FindControl("lblFinalWt");
        Label lblTotalLagat = (Label)gvRow.FindControl("lblTotalLagat");

        ////''*******Start Calculate Actual Weight
        if (chkboxSampleFlag.Checked == false)
        {
            lblActualWt.Text = Convert.ToString(Math.Round(Convert.ToDouble(lblTotalLagat.Text) * Convert.ToDouble(lblArea.Text) * Convert.ToInt32(txtReqQty.Text), 3));
        }
        else
        {
            lblActualWt.Text = "0";
        }

        if (chkboxSampleFlag.Checked == false)
        {
            if (Convert.ToDouble(txtRecWt.Text) > Convert.ToDouble(lblActualWt.Text))
            {
                lblFinalWt.Text = lblActualWt.Text;
            }
            else
            {
                lblFinalWt.Text = txtRecWt.Text;
            }

        }
        else
        {
            lblFinalWt.Text = txtRecWt.Text;
        }
        ////''*******End Calculate Actual Weight

        DataTable dt3 = new DataTable();
        dt3 = (DataTable)ViewState["CarpetReceive"];

        // int sum = 0;
        //sum = Convert.ToInt32(dt3.Compute("SUM(Qty)", "ItemId=" + Item.Text + " and QualityId=" + lblQualityid.Text + " and Issue_Detail_Id=" + IssueDetailId.Text));

        if (BQty(Convert.ToInt32(IssueDetailId.Text), gvRow.RowIndex) == true)
        {
            if (dt3.Rows.Count > 0)
            {
                foreach (DataRow row in dt3.Rows)
                {
                    if (row["SrNo"].ToString() == lblSrNo.Text)
                    {
                        row["Qty"] = txtReqQty.Text == "" ? "0" : txtReqQty.Text;
                        row["RecWt"] = txtRecWt.Text == "" ? "0" : txtRecWt.Text;
                        row["ActualWt"] = lblActualWt.Text == "" ? "0" : lblActualWt.Text;
                        row["FinalWt"] = lblFinalWt.Text == "" ? "0" : lblFinalWt.Text;

                        if (DDcaltype.SelectedValue == "0" || DDcaltype.SelectedValue == "2" || DDcaltype.SelectedValue == "3" || DDcaltype.SelectedValue == "4")
                        {
                            row["Amount"] = Convert.ToDecimal(lblArea.Text) * Convert.ToInt32(txtReqQty.Text) * Convert.ToDecimal(row["Rate"].ToString());
                            row["CommAmt"] = Convert.ToDecimal(lblArea.Text) * Convert.ToInt32(txtReqQty.Text) * Convert.ToDecimal(row["Comm"].ToString());
                        }
                        if (DDcaltype.SelectedValue == "1")
                        {
                            row["Amount"] = Convert.ToInt32(txtReqQty.Text) * Convert.ToDecimal(row["Rate"].ToString());
                            row["CommAmt"] = Convert.ToInt32(txtReqQty.Text) * Convert.ToDecimal(row["Comm"].ToString());
                        }
                    }
                    dt3.AcceptChanges();
                    row.SetModified();
                    ViewState["CarpetReceive"] = dt3;
                }
            }
        }

        CalQtyTotalArea();
        CarpetPenalityDel(Convert.ToInt32(lblSrNo.Text));

    }
    protected void DDJobName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddJobName = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddJobName.Parent.Parent;
        int idx = row.RowIndex;
        ddJobName.Focus();

        DropDownList DDJobName = (DropDownList)row.FindControl("DDJobName");
        DropDownList DDFinisherName = (DropDownList)row.FindControl("DDFinisherName");

        Label lblJobName = (Label)row.FindControl("lblJobName");
        Label lblFinisherName = (Label)row.FindControl("lblFinisherName");
        Label lblSrNo = (Label)row.FindControl("lblSrNo");

        if (DDJobName.SelectedIndex > 0)
        {
            lblJobName.Text = DDJobName.SelectedValue;
            UtilityModule.ConditionalComboFill(ref DDFinisherName, "select EI.EmpId,EI.EmpName from EmpProcess EP INNER JOIN EmpInfo EI ON EP.EmpId=EI.EmpId  where Processid=" + DDJobName.SelectedValue, true, "--Select--");
        }

        DataTable dt3 = new DataTable();
        dt3 = (DataTable)ViewState["CarpetReceive"];

        if (dt3.Rows.Count > 0)
        {
            foreach (DataRow row2 in dt3.Rows)
            {
                if (row2["SrNo"].ToString() == lblSrNo.Text)
                {
                    row2["JobName"] = DDJobName.SelectedIndex > 0 ? DDJobName.SelectedValue : "0";
                    row2["FinisherName"] = DDFinisherName.SelectedIndex > 0 ? DDFinisherName.SelectedValue : "0";


                    if (DDJobName.SelectedIndex > 0)
                    {
                        lblJobName.Text = row2["JobName"].ToString();
                    }
                    if (DDFinisherName.SelectedIndex > 0)
                    {
                        lblFinisherName.Text = row2["FinisherName"].ToString();
                    }
                }
                dt3.AcceptChanges();
                row2.SetModified();
                ViewState["CarpetReceive"] = dt3;
            }
        }

    }
    protected void DDFinisherName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddFinisherName = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddFinisherName.Parent.Parent;
        int idx = row.RowIndex;
        //ddFinisherName.Focus();

        DropDownList DDJobName = (DropDownList)row.FindControl("DDJobName");
        DropDownList DDFinisherName = (DropDownList)row.FindControl("DDFinisherName");

        Label lblJobName = (Label)row.FindControl("lblJobName");
        Label lblFinisherName = (Label)row.FindControl("lblFinisherName");
        Label lblSrNo = (Label)row.FindControl("lblSrNo");

        if (DDJobName.SelectedIndex > 0)
        {
            lblJobName.Text = DDJobName.SelectedValue;

            //UtilityModule.ConditionalComboFill(ref DDFinisherName, "select EI.EmpId,EI.EmpName from EmpProcess EP INNER JOIN EmpInfo EI ON EP.EmpId=EI.EmpId  where Processid=" + DDJobName.SelectedValue, true, "--Select--");
            lblFinisherName.Text = DDFinisherName.SelectedValue;
        }

        DataTable dt3 = new DataTable();
        dt3 = (DataTable)ViewState["CarpetReceive"];

        if (dt3.Rows.Count > 0)
        {
            foreach (DataRow row2 in dt3.Rows)
            {
                if (row2["SrNo"].ToString() == lblSrNo.Text)
                {
                    row2["JobName"] = DDJobName.SelectedIndex > 0 ? DDJobName.SelectedValue : "0";
                    row2["FinisherName"] = DDFinisherName.SelectedIndex > 0 ? DDFinisherName.SelectedValue : "0";


                    if (DDJobName.SelectedIndex > 0)
                    {
                        lblJobName.Text = row2["JobName"].ToString();
                    }
                    if (DDFinisherName.SelectedIndex > 0)
                    {
                        lblFinisherName.Text = row2["FinisherName"].ToString();
                    }
                }
                dt3.AcceptChanges();
                row2.SetModified();
                ViewState["CarpetReceive"] = dt3;
            }
        }
    }
    protected void GVPenalty_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkoutItem = ((CheckBox)e.Row.FindControl("Chkboxitem"));
            TextBox txtQty = ((TextBox)e.Row.FindControl("txtQty"));
            TextBox txtAmt = ((TextBox)e.Row.FindControl("txtAmt"));
            //Label lblAmt = ((Label)e.Row.FindControl("lblAmt"));

            Label lblRate = ((Label)e.Row.FindControl("lblRate"));
            Label lblPenalityName = ((Label)e.Row.FindControl("lblPenalityName"));
            Label lblPenalityId = ((Label)e.Row.FindControl("lblPenalityId"));
            Label lblPenalityType = ((Label)e.Row.FindControl("lblPenalityType"));

            if (lblPenalityId.Text == "0")
            {
                //GVPenalty.Rows[currentRow.RowIndex].Enabled = false;
                txtQty.Enabled = false;
                txtQty.Text = "0";
                txtAmt.Enabled = true;
                //txtAmt.Text = "0";
                //cb.Checked = false;
            }
            else
            {
                //GVPenalty.Rows[currentRow.RowIndex].Enabled = true;
                txtAmt.Enabled = false;
            }

            if (ViewState["CarpetReceivePen"] != null)
            {
                DataTable dt = (DataTable)ViewState["CarpetReceivePen"];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (ViewState["SrNo"].ToString() == dt.Rows[i]["SrNo"].ToString() && lblPenalityId.Text == dt.Rows[i]["PenalityId"].ToString())
                    {
                        chkoutItem.Checked = true;
                        txtQty.Text = dt.Rows[i]["Qty"].ToString();
                        txtAmt.Text = dt.Rows[i]["Amt"].ToString();

                        //DataView dv = new DataView(dt);
                        //dv.RowFilter = "SrNo =" + ViewState["SrNo"].ToString();
                        //GVPenalty.DataSource = dv;
                        //GVPenalty.DataBind();

                    }
                }
            }
        }

    }
    protected void Chkboxitem_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow gr = (GridViewRow)chk.Parent.Parent;
        CheckBox cb = (CheckBox)gr.FindControl("Chkboxitem");

        TextBox txtQty = (TextBox)gr.FindControl("txtQty");
        TextBox txtAmt = (TextBox)gr.FindControl("txtAmt");
        //Label lblAmt = (Label)gr.FindControl("lblAmt");
        Label lblPenalityType = (Label)gr.FindControl("lblPenalityType");
        Label lblPenalityId = (Label)gr.FindControl("lblPenalityId");
        Label lblRate = (Label)gr.FindControl("lblRate");

        if (cb.Checked == false)
        {
            txtQty.Text = "0";
            txtAmt.Text = "0";

            if (ViewState["CarpetReceivePen"] != null)
            {
                DataTable dt4 = new DataTable();
                dt4 = (DataTable)ViewState["CarpetReceivePen"];

                int m = 0;
                for (int n = 0; n < dt4.Rows.Count; n++)
                {
                    if (dt4.Rows[n]["SrNo"].ToString() == ViewState["SrNo"].ToString() && dt4.Rows[n]["PenalityId"].ToString() == lblPenalityId.Text)
                    {
                        m = 1;

                        dt4.Rows.RemoveAt(n);
                        dt4.AcceptChanges();
                        break;
                    }
                }
            }
        }

        ModalPopupExtender1.Show();
    }
    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        if (GVPenalty.Rows.Count > 0)
        {
            GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
            //int id =Convert.ToInt32(currentRow);

            String lblPenalityId = ((Label)currentRow.FindControl("lblPenalityId")).Text;

            TextBox txtQty = (TextBox)currentRow.FindControl("txtQty");
            // Label lblAmt = (Label)currentRow.FindControl("lblAmt");
            TextBox txtAmt = (TextBox)currentRow.FindControl("txtAmt");
            Label lblPenalityType = (Label)currentRow.FindControl("lblPenalityType");
            Label lblRate = (Label)currentRow.FindControl("lblRate");

            CheckBox cb = (CheckBox)currentRow.FindControl("Chkboxitem");

            if (lblPenalityId == "0")
            {
                //GVPenalty.Rows[currentRow.RowIndex].Enabled = false;
                txtQty.Enabled = false;
                txtQty.Text = "0";
                txtAmt.Enabled = true;
                //txtAmt.Text = "0";
                //cb.Checked = false;
            }
            else
            {
                //GVPenalty.Rows[currentRow.RowIndex].Enabled = true;
                txtAmt.Enabled = false;
            }

            if (cb.Checked == false)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Plz Check First Penality !!');", true);
                txtQty.Text = "0";
            }
            if (Convert.ToInt32(txtQty.Text) > Convert.ToInt32(ViewState["RQty"]))
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Sorry You Can't Enter More Than Receive Quantity !!');", true);
                txtQty.Text = "0";
                //lblAmt.Text = "0";
                txtAmt.Text = "0";
            }
            else
            {
                if (Convert.ToInt32(lblPenalityId) > 0)
                {
                    if (lblPenalityType.Text == "A")
                    {
                        double TAmt = Convert.ToDouble(txtQty.Text) * Convert.ToDouble(lblRate.Text);
                        //lblAmt.Text = Convert.ToString(TAmt);
                        txtAmt.Text = Convert.ToString(TAmt);
                    }
                    else
                    {
                        double TAmt = Convert.ToDouble(ViewState["Area"]) * Convert.ToDouble(txtQty.Text) * Convert.ToDouble(lblRate.Text);
                        // lblAmt.Text = Convert.ToString(TAmt);
                        txtAmt.Text = Convert.ToString(TAmt);
                    }
                }
            }

            ModalPopupExtender1.Show();
        }
    }
    protected void txtAmt_TextChanged(object sender, EventArgs e)
    {
        GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        //int id =Convert.ToInt32(currentRow);

        String lblPenalityId = ((Label)currentRow.FindControl("lblPenalityId")).Text;

        TextBox txtQty = (TextBox)currentRow.FindControl("txtQty");
        // Label lblAmt = (Label)currentRow.FindControl("lblAmt");
        TextBox txtAmt = (TextBox)currentRow.FindControl("txtAmt");
        Label lblPenalityType = (Label)currentRow.FindControl("lblPenalityType");
        Label lblRate = (Label)currentRow.FindControl("lblRate");

        CheckBox cb = (CheckBox)currentRow.FindControl("Chkboxitem");

        if (cb.Checked == true)
        {
            if (ViewState["CarpetReceivePen"] != null)
            {
                DataTable dt4 = new DataTable();
                dt4 = (DataTable)ViewState["CarpetReceivePen"];

                int m = 0;
                for (int n = 0; n < dt4.Rows.Count; n++)
                {
                    if (dt4.Rows[n]["SrNo"].ToString() == ViewState["SrNo"].ToString() && dt4.Rows[n]["PenalityId"].ToString() == lblPenalityId)
                    {
                        m = 1;
                        dt4.Rows[n]["Amt"] = txtAmt.Text;

                        dt4.AcceptChanges();
                        ViewState["CarpetReceivePen"] = dt4;
                    }
                }
            }
        }
        ModalPopupExtender1.Show();
    }
    protected void FillPenalityGrid(int ItemId)
    {
        string sql = "";
        if (variable.VarNewQualitySize == "1")
        {
            sql = @"select PenalityId,PenalityName,rate,PenalityType,QualityId from PenalityMaster where (Qualityid=" + ItemId + " or Qualityid=0) and PenalityWF='W' And PenalityId<>-1 ";
        }
        sql = sql + "Order By PenalityName Desc";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GVPenalty.DataSource = ds;
            GVPenalty.DataBind();
            GVPenalty.Visible = true;
        }
        else
        {
            GVPenalty.Visible = false;
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void popup_Click(object sender, EventArgs e)
    {
        ViewState["RQty"] = "";
        ViewState["Area"] = "";
        ViewState["SrNo"] = "";

        // Show ModalPopUpExtender.
        ModalPopupExtender1.Show();

        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = gvRow.RowIndex;
        LinkButton lnk = (LinkButton)gvRow.FindControl("popup");
        string myScript = lnk.Text;

        Label lblSrNo = (Label)gvRow.FindControl("lblSrNo");
        Label lblQualityId = (Label)gvRow.FindControl("lblQualityId");
        TextBox txtReqQty = (TextBox)gvRow.FindControl("txtReqQty");
        Label lblArea = (Label)gvRow.FindControl("lblArea");
        Label lblItemId = (Label)gvRow.FindControl("lblItemId");

        int Qualityid = Convert.ToInt32(lblQualityId.Text);
        int ItemId = Convert.ToInt32(lblItemId.Text);
        ViewState["RQty"] = txtReqQty.Text;
        ViewState["Area"] = lblArea.Text;
        ViewState["SrNo"] = lblSrNo.Text;
        FillPenalityGrid(ItemId);
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        ModalPopupExtender1.Hide();
        name = "";

        DataTable dtrecordsPen = new DataTable();

        if (ViewState["CarpetReceivePen"] == null)
        {
            //DataTable dtrecords = new DataTable();
            dtrecordsPen.Columns.Add("SrNo", typeof(int));
            dtrecordsPen.Columns.Add("PenalityId", typeof(int));
            dtrecordsPen.Columns.Add("PenalityName", typeof(string));
            dtrecordsPen.Columns.Add("Qty", typeof(int));
            dtrecordsPen.Columns.Add("Rate", typeof(float));
            dtrecordsPen.Columns.Add("Amt", typeof(float));
            dtrecordsPen.Columns.Add("PenalityType", typeof(string));

            ViewState["CarpetReceivePen"] = dtrecordsPen;
        }
        else
        {
            dtrecordsPen = (DataTable)ViewState["CarpetReceivePen"];
        }

        for (int i = 0; i < GVPenalty.Rows.Count; i++)
        {
            CheckBox chkoutItem = ((CheckBox)GVPenalty.Rows[i].FindControl("Chkboxitem"));
            TextBox txtQty = ((TextBox)GVPenalty.Rows[i].FindControl("txtQty"));
            TextBox txtAmt = ((TextBox)GVPenalty.Rows[i].FindControl("txtAmt"));
            //Label lblAmt = ((Label)GVPenalty.Rows[i].FindControl("lblAmt"));

            if ((chkoutItem.Checked == true) && (Convert.ToDouble(txtQty.Text == "" ? "0" : txtQty.Text) >= 0) && Convert.ToDouble(txtAmt.Text == "" ? "0" : txtAmt.Text) > 0)
            {
                Label lblRate = ((Label)GVPenalty.Rows[i].FindControl("lblRate"));
                Label lblPenalityName = ((Label)GVPenalty.Rows[i].FindControl("lblPenalityName"));
                Label lblPenalityId = ((Label)GVPenalty.Rows[i].FindControl("lblPenalityId"));
                Label lblPenalityType = ((Label)GVPenalty.Rows[i].FindControl("lblPenalityType"));

                int m = 0;
                for (int n = 0; n < dtrecordsPen.Rows.Count; n++)
                {
                    if (dtrecordsPen.Rows[n]["SrNo"].ToString() == ViewState["SrNo"].ToString() && dtrecordsPen.Rows[n]["PenalityId"].ToString() == lblPenalityId.Text)
                    {
                        m = 1;
                        dtrecordsPen.Rows.RemoveAt(n);
                        dtrecordsPen.AcceptChanges();
                        break;
                    }
                }

                DataRow dr = dtrecordsPen.NewRow();
                dr["SrNo"] = ViewState["SrNo"];
                dr["PenalityId"] = lblPenalityId.Text;
                dr["PenalityName"] = lblPenalityName.Text;
                dr["Qty"] = txtQty.Text;
                dr["Rate"] = lblRate.Text == "" ? "0" : lblRate.Text;
                dr["Amt"] = txtAmt.Text == "" ? "0" : txtAmt.Text;
                dr["PenalityType"] = lblPenalityType.Text;

                dtrecordsPen.Rows.Add(dr);
                ViewState["CarpetReceivePen"] = dtrecordsPen;
                name += lblPenalityName.Text + " " + txtQty.Text + "-" + txtAmt.Text + ",";
                //name += lblPenalityId.Text + "_" + lblPenalityName.Text + " " + txtQty.Text + "-" + txtAmt.Text + ",";
            }
        }

        DataTable dt3 = new DataTable();
        dt3 = (DataTable)ViewState["CarpetReceive"];

        if (dt3.Rows.Count > 0)
        {
            foreach (DataRow row2 in dt3.Rows)
            {
                if (row2["SrNo"].ToString() == ViewState["SrNo"].ToString())
                {
                    row2["Penality"] = name;
                }
                dt3.AcceptChanges();
                row2.SetModified();
                ViewState["CarpetReceive"] = dt3;
            }
        }

        for (int i = 0; i < GVCarpetReceive.Rows.Count; i++)
        {
            Label lblPenalityName = ((Label)GVCarpetReceive.Rows[i].FindControl("lblPenalityName"));
            Label lblSrNo = ((Label)GVCarpetReceive.Rows[i].FindControl("lblSrNo"));

            if (lblSrNo.Text == ViewState["SrNo"].ToString())
            {
                lblPenalityName.Text = name;
                lblPenalityName.ToolTip = name;
            }
        }

        //GVCarpetReceive.DataSource = ViewState["CarpetReceive"];
        //GVCarpetReceive.DataBind();        

    }
    protected void CarpetPenalityDel(int SrNo)
    {
        if (ViewState["CarpetReceivePen"] != null)
        {
            DataTable dtrecordsPen = (DataTable)ViewState["CarpetReceivePen"];

            DataView dv = new DataView(dtrecordsPen);
            dv.RowFilter = "SrNo not in('" + SrNo + "')";
            ViewState["CarpetReceivePen"] = dv.ToTable();
        }
    }

    protected void GVCarpetReceive_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int Issuedqty = 0;
        string txtReqQty = ((TextBox)GVCarpetReceive.Rows[e.RowIndex].FindControl("txtReqQty")).Text;

        string lblSrNo = ((Label)GVCarpetReceive.Rows[e.RowIndex].FindControl("lblSrNo")).Text;
        string lblItemId = ((Label)GVCarpetReceive.Rows[e.RowIndex].FindControl("lblItemId")).Text;
        string lblQualityId = ((Label)GVCarpetReceive.Rows[e.RowIndex].FindControl("lblQualityId")).Text;
        string Issue_Detail_Id = ((Label)GVCarpetReceive.Rows[e.RowIndex].FindControl("Issue_Detail_Id")).Text;
        string hnBalQty = ((Label)GVCarpetReceive.Rows[e.RowIndex].FindControl("hnBalQty")).Text;
        string lblFinalWt = ((Label)GVCarpetReceive.Rows[e.RowIndex].FindControl("lblFinalWt")).Text;
        int rowindex = e.RowIndex;

        DataTable dt3 = new DataTable();
        dt3 = (DataTable)ViewState["CarpetReceive"];

        if (dt3.Rows.Count > 0)
        {
            dt3.Rows.Remove(dt3.Rows[rowindex]);
            dt3.AcceptChanges();
            ViewState["CarpetReceive"] = dt3;
        }

        if (dt3.Rows.Count <= 0)
        {
            ViewState["CarpetReceivePen"] = null;
        }

        GVCarpetReceive.DataSource = ViewState["CarpetReceive"];
        GVCarpetReceive.DataBind();

        txtTotalWeight.Text = Convert.ToString(Convert.ToDouble(txtTotalWeight.Text == "" ? "0" : txtTotalWeight.Text) - Convert.ToDouble(lblFinalWt));

        CarpetPenalityDel(Convert.ToInt32(lblSrNo));

        BQty(Convert.ToInt32(Issue_Detail_Id), 0);
    }
    protected void BtnWeaver_Click(object sender, EventArgs e)
    {
        if (lblChallanNo.Text != null || lblChallanNo.Text != "")
        {
            Report3();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Challan no not showing!');", true);
        }
    }
    private void Report3()
    {
        DataSet ds = new DataSet();
        // string qry = "";
        // string str = "";
        SqlParameter[] array = new SqlParameter[6];
        array[0] = new SqlParameter("@ChallanNo", SqlDbType.Int);
        array[1] = new SqlParameter("@EmpId", SqlDbType.Int);
        array[2] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        array[3] = new SqlParameter("@CompanyId", SqlDbType.Int);
        array[4] = new SqlParameter("@SampleFlag", SqlDbType.Int);
        array[5] = new SqlParameter("@ManualWtEntry", SqlDbType.Int);
        array[0].Value = Convert.ToInt32(lblProcessRecID.Text == "" ? "0" : lblProcessRecID.Text);
        array[1].Value = DDEmployeeNamee.SelectedValue;
        array[2].Value = Session["varMasterCompanyIDForERP"];
        array[3].Value = DDCompanyName.SelectedValue;

        int chkboxsampleflag = 0;
        if (chkboxSampleFlag.Checked == true)
        {
            chkboxsampleflag = 1;
        }
        else
        {
            chkboxsampleflag = 0;
        }
        array[4].Value = chkboxsampleflag;

        int chkboxManualWtflag = 0;
        if (chkboxManualWeight.Checked == true)
        {
            chkboxManualWtflag = 1;
        }
        else
        {
            chkboxManualWtflag = 0;
        }
        array[5].Value = chkboxManualWtflag;

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForWeaverBazaarReport", array);


        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\WCarpetBazaar.rpt";

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\WCarpetBazaar.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    protected void BtnFinisher_Click(object sender, EventArgs e)
    {
        if (lblChallanNo.Text != null || lblChallanNo.Text != "")
        {
            Report4();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Challan no not showing!');", true);
        }

    }
    private void Report4()
    {
        DataSet ds = new DataSet();
        // string qry = "";
        // string str = "";
        SqlParameter[] array = new SqlParameter[5];
        array[0] = new SqlParameter("@ChallanNo", SqlDbType.Int);
        array[1] = new SqlParameter("@EmpId", SqlDbType.Int);
        array[2] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        array[3] = new SqlParameter("@CompanyId", SqlDbType.Int);
        // array[4] = new SqlParameter("@SampleFlag", SqlDbType.Int);
        array[0].Value = Convert.ToInt32(lblProcessRecID.Text == "" ? "0" : lblProcessRecID.Text);
        array[1].Value = DDEmployeeNamee.SelectedValue;
        array[2].Value = Session["varMasterCompanyIDForERP"];
        array[3].Value = DDCompanyName.SelectedValue;

        //int chkboxsampleflag = 0;
        //if (chkboxSampleFlag.Checked == true)
        //{
        //    chkboxsampleflag = 1;
        //}
        //else
        //{
        //    chkboxsampleflag = 0;
        //}

        //array[4].Value = chkboxsampleflag;


        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForFinisherBazaarReport", array);


        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\FinisherCarpetBazaar.rpt";


            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\FinisherCarpetBazaar.xsd";
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
}