using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_process_EditProcessReceiveNew : System.Web.UI.Page
{
    static int MasterCompanyId;
    static int sum = 0;
    static int GVReceiveCount = 0;
    static string name = "";
    static string PenMode = "";
    static int checkboxtds = 0;
    static int checkboxdryweight = 0;
    static int sampleflag = 0;
    static int flagfixorweight = 0;
    static int FinalBazaar = 0;
    static string btnclickflag="";
    static int VendorEmpId = 0;
   static decimal TBToalWt = 0;
   static int ManualWtEntryflag = 0;
   StringBuilder sb = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varCompanyId"]);
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
           BtnSave.Attributes.Add("onclick", "getMessage()");
            string process;
            if (Session["varcompanyId"].ToString() == "9")
            {
                process = "Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where MasterCompanyId=" + Session["varCompanyId"] + @" and process_name_id in(1,16) Order By PROCESS_NAME";
            }
            else
            {
                process = "Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where MasterCompanyId=" + Session["varCompanyId"] + @" and Process_name_id=1 Order By PROCESS_NAME";
            }
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
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

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            if (Convert.ToInt32(Session["varCompanyId"]) == 7)
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
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedIndex = 1;                
                
            }
            if (DDProcessName.Items.Count > 0)
            {
                if (Convert.ToInt32(Session["varCompanyId"]) == 7)
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


                BindChallanNo();
                //ProcessSelectedChange();
                
            }

           // BindChallanNo();
            
            //DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + "");
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
            switch (Convert.ToInt16(Session["varcompanyId"]))
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

            GVCarpetReceive.Columns[15].Visible = false;
            GVCarpetReceive.Columns[16].Visible = false;
            GVCarpetReceive.Columns[17].Visible = false;

            GVCarpetReceiveEdit.Columns[15].Visible = false;
            GVCarpetReceiveEdit.Columns[16].Visible = false;
            GVCarpetReceiveEdit.Columns[17].Visible = false;
        }
        else
        {
            txtTotalWeight.Enabled = false;
            GVCarpetReceive.Columns[15].Visible = true;
            GVCarpetReceive.Columns[16].Visible = true;
            GVCarpetReceive.Columns[17].Visible = true;

            GVCarpetReceiveEdit.Columns[15].Visible = true;
            GVCarpetReceiveEdit.Columns[16].Visible = true;
            GVCarpetReceiveEdit.Columns[17].Visible = true;
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
            tdRemarks.Visible = true;
            //tdsampleflag.Visible = false;
            tdProcess.Visible = false;
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
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
    }
    protected void PenalityColumnShowStatus(int Status)
    {        
        for (int i = 0; i < GVCarpetReceiveEdit.Columns.Count; i++)
        {
            if (GVCarpetReceiveEdit.Columns[i].HeaderText == "Penality")
            {
                if (Status == 0)
                {
                    GVCarpetReceiveEdit.Columns[i].Visible = false;
                }
                else if(Status == 1)
                {
                    GVCarpetReceiveEdit.Columns[i].Visible = true;
                }
            }
        }
    }
    protected void TxtPrefix_TextChanged(object sender, EventArgs e)
    {
        TxtPostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix((TxtPrefix.Text).ToUpper()));
    }
    protected void BindChallanNo()
    {
        string str = "Select Process_Rec_Id, ChallanNo from PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PIM where PIM.CompanyId="+DDCompanyName.SelectedIndex+" and PIM.SampleFlag="+(chkboxSampleFlag.Checked==true ? 1 : 0)+"";

        if (DDEmployeeNamee.SelectedIndex > 0)
        {
            str = str + " and PIM.EmpId=" + DDEmployeeNamee.SelectedValue;
        }
        str = str + " order by Process_Rec_Id";

        UtilityModule.ConditionalComboFill(ref DDChallanNo, str, true, "--Select--");       
    }    
    protected void CreatePenEdit()
    {
        ViewState["CarpetReceivePenEdit"] = null;
        DataTable dtrecordsPenEdit = new DataTable();

        if (ViewState["CarpetReceivePenEdit"] == null)
        {
            //DataTable dtrecords = new DataTable();
            dtrecordsPenEdit.Columns.Add("WPenalityId", typeof(int));
            dtrecordsPenEdit.Columns.Add("PenalityId", typeof(int));
            dtrecordsPenEdit.Columns.Add("PenalityName", typeof(string));
            dtrecordsPenEdit.Columns.Add("Qty", typeof(int));
            dtrecordsPenEdit.Columns.Add("Rate", typeof(float));
            dtrecordsPenEdit.Columns.Add("Amount", typeof(float));
            dtrecordsPenEdit.Columns.Add("PenalityType", typeof(string));
            dtrecordsPenEdit.Columns.Add("Process_Rec_Id", typeof(int));
            dtrecordsPenEdit.Columns.Add("Process_Rec_Detail_Id", typeof(int));

            ViewState["CarpetReceivePenEdit"] = dtrecordsPenEdit;
        }
        else
        {
            dtrecordsPenEdit = (DataTable)ViewState["CarpetReceivePenEdit"];
        }

        string sql = "";

        sql = @"select WPenalityId,PM.PenalityId,PM.PenalityName, Qty,PM.Rate,Amount,PM.PenalityType,Process_Rec_Id,Process_Rec_Detail_Id from WeaverCarpetReceivePenality WCRP
                INNER JOIN PenalityMaster PM ON WCRP.PenalityId=PM.PenalityId and PM.PenalityWF='W' where Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " ";
        sql = sql + "Order By PenalityName Desc";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["CarpetReceivePenEdit"] = ds.Tables[0];
        }
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        GVCarpetReceiveEdit.EditIndex = -1;
        ChallanSelectedChange();       
       
        //CreatePenEdit();
        //ProcessSelectedChange();
    }
    private void ChallanSelectedChange()
    {
        if (DDProcessName.SelectedIndex > 0)
        {                       
                //ViewState["Process_Rec_Id"] = DDChallanNo.SelectedValue;
                //CreatePenEdit();

                UtilityModule.ConditionalComboFill(ref DDEmployeeNamee, "Select Distinct EI.EmpId,EI.EmpName from Empinfo EI,PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PRM WHERE PRM.EmpId=EI.EmpId ANd EI.MasterCompanyId=" + Session["varCompanyId"] + " and PRM.Process_Rec_Id=" + DDChallanNo.SelectedValue + " order by ei.empname ", true, "--Select--");
                if (DDEmployeeNamee.Items.Count > 0)
                {
                    DDEmployeeNamee.SelectedIndex = 1;
                    EmployeeSelectedChange();
                    ViewState["Process_Rec_Id"] = DDChallanNo.SelectedValue;
                    CreatePenEdit();
                }
                          
        }      
    }
    protected void chkboxSampleFlag_CheckedChanged(object sender, EventArgs e)
    {
        BindChallanNo();
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearAfterCompanyChange();
        ProcessSelectedChange();
        BindChallanNo();
        
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Process_Rec_Id"] = 0;
        //ProcessSelectedChange();
    }
    private void ProcessSelectedChange()
    {
        if (DDProcessName.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref DDEmployeeNamee, "Select Distinct EI.EmpId,EI.EmpName from Empinfo EI,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM WHERE PIM.EmpId=EI.EmpId ANd EI.MasterCompanyId=" + Session["varCompanyId"] + " order by ei.empname ", true, "--Select--");

            //if (DDEmployeeNamee.Items.Count > 0)
            //{                
            //    DDEmployeeNamee.SelectedValue =Convert.ToString(VendorEmpId);
            //}
        }
    }
    protected void DDEmployeeNamee_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmployeeSelectedChange();
        BindChallanNo();
    }
    private void EmployeeSelectedChange()
    {
        ViewState["Process_Rec_Id"] = 0;
        //Fill_Grid();
        //
        
        if (DDProcessName.SelectedIndex > 0 && DDEmployeeNamee.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref DDPONo, "Select Distinct PM.IssueOrderId,case When " + Session["varcompanyId"] + "=9 Then Om.localOrder+'/'+cast(PM.IssueOrderId as varchar(100)) ELse cast(PM.IssueOrderId as varchar(100)) End as IssueOrderid1 from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD,ordermaster OM  Where PM.ISSUEORDERID=PD.ISSUEORDERID And PD.orderid=OM.orderid And PD.PQty<>0 And PM.status='Pending' And Empid=" + DDEmployeeNamee.SelectedValue + " And Pm.CompanyId=" + DDCompanyName.SelectedValue + " order by PM.Issueorderid", true, "--Select--");
        }

        CategoryBind();
        fillorderdetail();
        fillCarpetReceive();
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
                        UtilityModule.ConditionalComboFill(ref DDCategoryName, "Select Distinct ICM.Category_Id,Category_Name from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PID,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM,ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,ITEM_CATEGORY_MASTER ICM inner join UserRights_Category UC on(ICM.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ")  where IM.CATEGORY_ID=ICM.CATEGORY_ID AND IPM.Item_Id=IM.Item_Id AND PID.Item_Finished_Id=IPM.Item_Finished_Id and PID.IssueOrderId=PIM.IssueOrderId and PIM.Empid=" + DDEmployeeNamee.SelectedValue + " and PIM.Status!='Canceled' And IM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
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
                    UtilityModule.ConditionalComboFill(ref DDCategoryName, "Select Distinct ICM.Category_Id,Category_Name from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PID,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM,ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,ITEM_CATEGORY_MASTER ICM inner join UserRights_Category UC on(ICM.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ") where IM.CATEGORY_ID=ICM.CATEGORY_ID AND IPM.Item_Id=IM.Item_Id AND PID.Item_Finished_Id=IPM.Item_Finished_Id and PID.IssueOrderId=PIM.IssueOrderId and PIM.Empid=" + DDEmployeeNamee.SelectedValue + " and PIM.Status!='Canceled' And IM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");

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
                        UtilityModule.ConditionalComboFill(ref DDCategoryName, "Select Distinct ICM.Category_Id,Category_Name from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PID,ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,ITEM_CATEGORY_MASTER ICM inner join UserRights_Category UC on(ICM.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ")  where IM.CATEGORY_ID=ICM.CATEGORY_ID AND IPM.Item_Id=IM.Item_Id AND PID.Item_Finished_Id=IPM.Item_Finished_Id and PID.IssueOrderId=" + DDPONo.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
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
                    UtilityModule.ConditionalComboFill(ref DDCategoryName, "Select Distinct ICM.Category_Id,Category_Name from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PID,ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,ITEM_CATEGORY_MASTER ICM inner join UserRights_Category UC on(ICM.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ") where IM.CATEGORY_ID=ICM.CATEGORY_ID AND IPM.Item_Id=IM.Item_Id AND PID.Item_Finished_Id=IPM.Item_Finished_Id and PID.IssueOrderId=" + DDPONo.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
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
            //UtilityModule.ConditionalComboFill(ref DDItemName, "Select Distinct Item_id, Item_Name from V_FinishedItemDetailNew VF,PROCESS_Receive_DETAIL_" + DDProcessName.SelectedValue + " PD,PROCESS_Receive_MASTER_" + DDProcessName.SelectedValue + " PIM Where VF.Item_Finished_Id=PD.Item_Finished_Id And VF.Category_Id=" + DDCategoryName.SelectedValue + " And PIM.Empid=" + DDEmployeeNamee.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
            UtilityModule.ConditionalComboFill(ref DDItemName, @"Select Distinct Item_id, Item_Name from V_FinishedItemDetailNew VF 
                INNER JOIN PROCESS_Receive_DETAIL_" + DDProcessName.SelectedValue + @" PD ON VF.Item_Finished_Id=PD.Item_Finished_Id
				INNER JOIN PROCESS_Receive_MASTER_" + DDProcessName.SelectedValue + @" PIM ON PD.Process_Rec_Id=PIM.Process_Rec_Id				
				Where  VF.Category_Id=" + DDCategoryName.SelectedValue + " and PIM.Process_Rec_Id=" + DDChallanNo.SelectedValue + @"
				And PIM.Empid=" + DDEmployeeNamee.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
            if (DDItemName.Items.Count > 0)
            {
                DDItemName.SelectedIndex = 1;
                DDitemchange();
                fillorderdetail();
            }
        }
        //else
        //{
        //    UtilityModule.ConditionalComboFill(ref DDItemName, "Select Distinct Item_id, Item_Name from V_FinishedItemDetail VF,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD Where VF.Item_Finished_Id=PD.Item_Finished_Id And VF.Category_Id=" + DDCategoryName.SelectedValue + " And PD.IssueOrderId=" + DDPONo.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
        //}
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
                TxtPrefix.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Item_Code from Item_Master Where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "").ToString();
                if (Convert.ToInt32(Session["varCompanyId"]) != 5)
                {
                    get_year = DateTime.Now.ToString("dd-MMM-yyyy");
                    lastTwoChars = get_year.Substring(get_year.Length - 2);
                }
                switch (Convert.ToInt32(Session["varcompanyId"]))
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
                   // qulitychk.Visible = true;
                    fillgrdquality();
                }
                else
                {
                   // qulitychk.Visible = false;
                }
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
    private void fillgrdquality()
    {
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select QCMaster.ID,SrNo,ParaName from QCParameter inner join QCMaster on QCParameter.ParaID=QCMaster.ParaID where CategoryID=" + DDCategoryName.SelectedValue + " and ItemID=" + DDItemName.SelectedValue + " and ProcessID=" + DDProcessName.SelectedValue + " order by SrNo");
        //grdqualitychk.DataSource = ds;
        //grdqualitychk.DataBind();
    }
    private void Item_SelectedIndexChange()
    {
        string str = "";

        str = @" select distinct VF.QualityId,VF.QualityName from PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PRM INNER JOIN PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PRD ON PRM.Process_Rec_Id=PRD.Process_Rec_Id INNER JOIN V_FinishedItemDetailNew VF ON PRD.Item_Finished_Id=VF.ITEM_FINISHED_ID
                where PRM.Process_Rec_Id=" + DDChallanNo.SelectedValue + " and PRM.Empid=" + DDEmployeeNamee.SelectedValue + " And VF.Item_Id=" + DDItemName.SelectedValue + "";

        UtilityModule.ConditionalComboFill(ref DDQualityName, str, true, "-----------Select------");
        if (DDQualityName.Items.Count > 0)
        {
            DDQualityName.SelectedIndex = 1;
            DDQualityNameChange();
            fillorderdetail();
        }
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
        btnclickflag = "";
        if (FinalBazaar == 1)
        {
            btnclickflag = "BtnSave";
            txtpwd.Focus();
            Popup(true);
        }
        else
        {
            CHECKVALIDCONTROL();
            ChkGVDDValidation();
            if (llMessageBox.Text == "")
            {
                ProcessIssue();               
                UpdateFullAreaRound();
                UpdateTotalWeightRemarks();               
                UpdateTDS();
                DryWeightSubmit();

                if (chkboxSampleFlag.Checked == false)
                {
                    UpdateRateComm();
                }
                FinalBazaarSubmit();
                 
            }
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
         //ChkGVDDValidation();
         //if (llMessageBox.Text == "")
         //{
             SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
             con.Open();

             DataTable dtrecords = new DataTable();
             if (ViewState["CarpetReceive"] != null)
             {
                 dtrecords = (DataTable)ViewState["CarpetReceive"];
             }

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
                     _arrpara[14] = new SqlParameter("@MastercompanyId", Session["varcompanyId"]);
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
                     ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + llMessageBox.Text + "');", true);
                     //ViewState["Process_Rec_Id"] = _arrpara[0].Value.ToString();
                     // ViewState["Process_Rec_Id"] = 0;
                     Tran.Commit();

                     TxtChallanNo.Text = _arrpara[5].Value.ToString();

                     ClearAfterSave();
                     //fillCarpetReceive();
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
        // }
       

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

    }
    private void ClearAfterCompanyChange()
    {

        DDEmployeeNamee.Focus();        
        GVCarpetReceive.DataSource = null;
        GVCarpetReceive.DataBind();

        GVCarpetReceiveEdit.DataSource = null;
        GVCarpetReceiveEdit.DataBind();

        GVPenalty.DataSource = null;
        GVPenalty.DataBind();

        dgorder.DataSource = null;
        dgorder.DataBind();

        ViewState["CarpetReceive"] = null;
        ViewState["CarpetReceivePen"] = null;
        ViewState["CarpetReceivePenEdit"] = null;

        chkboxSampleFlag.Checked = false;
        txtTharri.Text = "";
        txtLachhi.Text = "";
        txtTar.Text = "";
        txtMisc.Text = "";
        txtExtra.Text = "";
        txtLagat.Text = "";
        txtLoss.Text = "";

        txtTotalTharri.Text = "";
        txtTotalLachhi.Text = "";
        txtTotalTar.Text = "";
        txtTotalMisc.Text = "";
        txtTotalExtra.Text = "";
        txtTotalLagat.Text = "";

        txtTotalWeight.Text = "";
        txtCheckPcs.Text = "";
        txtCheckWeight.Text = "";
        CBTDS.Checked = false;

        txtTotalAmt.Text = "";
        txtTotalPen.Text = "";
        txtTotalComm.Text = "";
        txtTotalTDSAmt.Text = "";
        txtTotalNetAmt.Text = "";
        //TxtRecDate.Text = "";
        TxtRemarks.Text = "";        

    }
    private void CATEGORY_DEPENDS_CONTROLS()
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "select Distinct Item_id, Item_Name from Item_Master where Category_Id=" + DDCategoryName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
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
                       PIM.UnitId UNIT,PID.Item_Finished_Id as finishedid,Issue_Detail_Id,PIM.AssignDate,PID.width as ProdWidthFt,PID.Length as ProdLengthFt,PID.AfterKhapSizeOrder as finishing_Ft_Size,
PID.Area as Finishing_Ft_Area,V.Finishing_Mt_Size,PID.Orderid,PID.Rate,PID.Comm,PIM.FlagFixOrWeight From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PIM,
                       PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PID,V_FinishedItemDetailNew V Where PIM.IssueOrderId=PID.IssueOrderId And 
                       pid.item_finished_id=V.Item_Finished_Id And PIM.Empid=" + DDEmployeeNamee.SelectedValue + " and PIM.Status='Pending' And V.MasterCompanyId=" + Session["varCompanyId"] + " And PIM.CompanyId=" + DDCompanyName.SelectedValue + @" Group By PID.IssueOrderId, CATEGORY_NAME,Item_Name,QualityName,Designname,
                       ColorName,ShadeColorName,ShapeName,ProdSizeMtr,ProdSizeFt,Sizeinch,UnitId,CATEGORY_ID,V.ITEM_ID,QualityId,ColorId,DesignId,SizeId,ShapeId,ShadecolorId,PID.Item_Finished_Id,PIM.AssignDate,
                        PID.width,PID.Length,PID.AfterKhapSizeOrder,PID.Area,V.Finishing_Mt_Size,PID.Orderid,PID.Rate,PID.Comm,PIM.FlagFixOrWeight,Issue_Detail_Id Having isnull(sum(pid.PQty),0)>0";

//            sql = @"Select row_number() over (order by issue_Detail_Id) as srno, PID.IssueOrderId ,Item_Name as Item, QualityName as Quality,Designname as Design,ColorName as Color,ShapeName as Shape,
//                        CATEGORY_NAME+'  '+ Item_Name+'  '+QualityName+'  '+Designname+'  '+ColorName+'  '+ShapeName+'  '+Case When UnitId=1 Then ProdSizeMtr Else Case When UnitId=2 Then  ProdSizeFt Else Case When UnitId=6 Then SizeInch End End End+'  '+ShadeColorName As Description,
//                        Case When UnitId=1 Then ProdSizeMtr Else Case When UnitId=2 Then  ProdSizeFt Else Case When UnitId=6 Then SizeInch End End End as Size,
//                       IsNull(Sum(PID.Qty),0)-Isnull(Sum(Pid.CancelQty),0) As Qty,isnull(sum(pid.PQty),0)-Isnull(Sum(CancelQty),0) as issueqqty,CATEGORY_ID,v.ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId,
//                       PIM.UnitId UNIT,PID.Item_Finished_Id as finishedid,Issue_Detail_Id,PIM.AssignDate,V.ProdWidthFt,V.ProdLengthFt,
//                        V.finishing_Ft_Size, V.Finishing_Ft_Area,V.Finishing_Mt_Size,PID.Orderid,PID.Rate,PID.Comm,PIM.FlagFixOrWeight From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PIM,
//                       PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PID,V_FinishedItemDetailNew V Where PIM.IssueOrderId=PID.IssueOrderId And 
//                       pid.item_finished_id=V.Item_Finished_Id And PIM.Empid=" + DDEmployeeNamee.SelectedValue + " and PIM.Status='Pending' And V.MasterCompanyId=" + Session["varCompanyId"] + " And PIM.CompanyId=" + DDCompanyName.SelectedValue + @" Group By PID.IssueOrderId, CATEGORY_NAME,Item_Name,QualityName,Designname,
//                       ColorName,ShadeColorName,ShapeName,ProdSizeMtr,ProdSizeFt,Sizeinch,UnitId,CATEGORY_ID,V.ITEM_ID,QualityId,ColorId,DesignId,SizeId,ShapeId,ShadecolorId,PID.Item_Finished_Id,PIM.AssignDate,
//                        V.ProdWidthFt,V.ProdLengthFt,V.finishing_Ft_Size, V.Finishing_Ft_Area,V.Finishing_Mt_Size,PID.Orderid,PID.Rate,PID.Comm,PIM.FlagFixOrWeight,Issue_Detail_Id Having isnull(sum(pid.PQty),0)>0";
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
            //ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    decimal TotalArea3 = 0;
    double VarLoss = 0;
    double OtherMat = 0;
    private void FillTharThari()
    {
        for (int i = 0; i < GVCarpetReceiveEdit.Rows.Count; i++)
        {
            Label lblTotalArea = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblTotalArea"));

            TotalArea3 += Convert.ToDecimal(lblTotalArea.Text);
        }

        if (sampleflag > 0)
        {
            if (flagfixorweight > 0)
            {
                VarLoss = Convert.ToDouble(string.Format("{0:#0.000}", Convert.ToDouble(TotalArea3) * Convert.ToDouble(txtLoss.Text == "" ? "0" : txtLoss.Text)));
                OtherMat = Convert.ToDouble(string.Format("{0:#0.000}", (Convert.ToDouble(TotalArea3) * Convert.ToDouble(txtTharri.Text == "" ? "0" : txtTharri.Text) + Convert.ToDouble(TotalArea3) * Convert.ToDouble(txtLachhi.Text == "" ? "0" : txtLachhi.Text) + Convert.ToDouble(TotalArea3) * Convert.ToDouble(txtTar.Text == "" ? "0" : txtTar.Text) + Convert.ToDouble(TotalArea3) * Convert.ToDouble(txtMisc.Text == "" ? "0" : txtMisc.Text) + Convert.ToDouble(TotalArea3) * Convert.ToDouble(txtExtra.Text == "" ? "0" : txtExtra.Text))));

                if (Convert.ToDouble(txtTotalWeight.Text == "" ? "0" : txtTotalWeight.Text) > 0)
                {
                    txtTotalTharri.Text = Convert.ToString(string.Format("{0:#0.000}", Convert.ToDouble(TotalArea3) * Convert.ToDouble(txtTharri.Text== "" ? "0" :txtTharri.Text)));
                    txtTotalLachhi.Text = Convert.ToString(string.Format("{0:#0.000}", Convert.ToDouble(TotalArea3) * Convert.ToDouble(txtLachhi.Text== "" ? "0" :txtLachhi.Text)));
                    txtTotalTar.Text = Convert.ToString(string.Format("{0:#0.000}", Convert.ToDouble(TotalArea3) * Convert.ToDouble(txtTar.Text== "" ? "0" :txtTar.Text)));
                    txtTotalMisc.Text = Convert.ToString(string.Format("{0:#0.000}", Convert.ToDouble(TotalArea3) * Convert.ToDouble(txtMisc.Text== "" ? "0" :txtMisc.Text)));
                    txtTotalExtra.Text = Convert.ToString(string.Format("{0:#0.000}", Convert.ToDouble(TotalArea3) * Convert.ToDouble(txtExtra.Text== "" ? "0" :txtExtra.Text)));
                    txtTotalLagat.Text = Convert.ToString(string.Format("{0:#0.000}", Convert.ToDouble(txtTotalWeight.Text == "" ? "0" : txtTotalWeight.Text) + VarLoss - OtherMat));
                }
            }
            else if(flagfixorweight==0)
            {
                if (Convert.ToDouble(txtTotalWeight.Text == "" ? "0" : txtTotalWeight.Text) > 0)
                {
                    txtTotalTharri.Text = Convert.ToString(string.Format("{0:#0.000}", Convert.ToDouble(TotalArea3) * Convert.ToDouble(txtTharri.Text== "" ? "0" :txtTharri.Text)));
                    txtTotalLachhi.Text = Convert.ToString(string.Format("{0:#0.000}", Convert.ToDouble(TotalArea3) * Convert.ToDouble(txtLachhi.Text== "" ? "0" :txtLachhi.Text)));
                    txtTotalTar.Text = Convert.ToString(string.Format("{0:#0.000}", Convert.ToDouble(TotalArea3) * Convert.ToDouble(txtTar.Text== "" ? "0" :txtTar.Text)));
                    txtTotalMisc.Text = Convert.ToString(string.Format("{0:#0.000}", Convert.ToDouble(TotalArea3) * Convert.ToDouble(txtMisc.Text== "" ? "0" :txtMisc.Text)));
                    txtTotalExtra.Text = Convert.ToString(string.Format("{0:#0.000}", Convert.ToDouble(TotalArea3) * Convert.ToDouble(txtExtra.Text== "" ? "0" :txtExtra.Text)));
                    txtTotalLagat.Text = Convert.ToString(string.Format("{0:#0.000}", Convert.ToDouble(TotalArea3) * Convert.ToDouble(txtLagat.Text == "" ? "0" : txtLagat.Text) + VarLoss));
                    //txtTotalLoss.Text = Convert.ToString(string.Format("{0:#0.000}", Convert.ToDouble(TotalArea3) * Convert.ToDouble(txtLoss.Text)));
                }
               
            }
           
        }
        else
        {
            chkboxSampleFlag.Checked = false;
            txtTharri.Text = "0";
            txtLachhi.Text = "0";
            txtTar.Text = "0";
            txtMisc.Text = "0";
            txtExtra.Text = "0";
            txtLagat.Text = "0";
            txtLoss.Text = "0";

            txtTotalTharri.Text="0";
            txtTotalLachhi.Text="0";
            txtTotalTar.Text="0";
            txtTotalMisc.Text="0";
            txtTotalExtra.Text="0";
            txtTotalLagat.Text="0";
            //txtTotalLoss.Text = "0";

            txtTharri.ReadOnly = true;
            txtLachhi.ReadOnly = true;
            txtTar.ReadOnly = true;
            txtMisc.ReadOnly = true;
            txtExtra.ReadOnly = true;
            txtLagat.ReadOnly = true;
            txtLoss.ReadOnly = true;
        }
        
    }   
    private void fillCarpetReceive()
    {
        if (DDChallanNo.SelectedIndex > 0)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _param = new SqlParameter[11];

                _param[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
                _param[1] = new SqlParameter("@EmpId", SqlDbType.Int);
                _param[2] = new SqlParameter("@MastercompanyId", SqlDbType.Int);
                _param[3] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);
                _param[4] = new SqlParameter("@ProcessId", SqlDbType.Int);
                _param[5] = new SqlParameter("@UserId", SqlDbType.Int);
                _param[6] = new SqlParameter("@CompanyId", SqlDbType.Int);

                _param[0].Value = DDChallanNo.SelectedValue;
                _param[1].Value = DDEmployeeNamee.SelectedValue;
                _param[2].Value = Session["varCompanyId"];
                _param[3].Direction = ParameterDirection.Output;
                _param[4].Value = DDProcessName.SelectedValue;
                _param[5].Value = Session["varuserid"];
                _param[6].Value = DDCompanyName.SelectedValue;


                //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_GetCarpetReceiveData]", _param);
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetCarpetReceiveData", _param);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    GVCarpetReceiveEdit.DataSource = ds.Tables[0];
                    GVCarpetReceiveEdit.DataBind();
                    GVCarpetReceiveEdit.Visible = true;


                    if (Convert.ToDecimal(lblTotalTDS.Text) > 0)
                    {
                        CBTDS.Checked = true;
                    }
                    else
                    {
                        CBTDS.Checked = false;
                    }


                    txtTotalWeight.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["TotalWeight"].ToString());
                    txtCheckPcs.Text = ds.Tables[0].Rows[0]["CheckPcs"].ToString();
                    txtCheckWeight.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["CheckWeight"].ToString());
                    TxtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                    TxtRecDate.Text = ds.Tables[0].Rows[0]["ReceivedDate"].ToString();
                    sampleflag = Convert.ToInt32(ds.Tables[0].Rows[0]["SampleFlag"].ToString());
                    flagfixorweight = Convert.ToInt32(ds.Tables[0].Rows[0]["FlagFixOrWeight"].ToString());
                    FinalBazaar = Convert.ToInt32(ds.Tables[0].Rows[0]["FinalBazaarFlag"].ToString());
                    VendorEmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"].ToString());
                    ManualWtEntryflag = Convert.ToInt32(ds.Tables[0].Rows[0]["ManualWtEntry"].ToString());
                    //txtTharri.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["Tharri"].ToString());
                    //txtTar.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["tar"].ToString());
                    //txtLachhi.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["CYarn"].ToString());
                    //txtMisc.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["Misc"].ToString());
                    //txtLoss.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["Loss"].ToString());
                    //txtExtra.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["Extra"].ToString());
                    //txtLagat.Text= string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["Wool"].ToString());

                    if (FinalBazaar == 1)
                    {
                        chkFinalBazaar.Checked = true;
                        //BtnFinalBazaar.Visible = false;
                        //BtnDryWeight.Visible = false;                
                        //BtnSave.Visible = false;
                        chkFinalBazaar.Enabled = false;
                        //BtnCancelChallan.Visible = false;                       
                    }
                    else
                    {
                        chkFinalBazaar.Checked = false;
                        chkFinalBazaar.Enabled = true;
                        FillTharThari();                        
                    }

                    if (ManualWtEntryflag == 1)
                    {
                        chkboxManualWeight.Checked = true;
                        txtTotalWeight.Enabled = true;

                        GVCarpetReceive.Columns[15].Visible = false;
                        GVCarpetReceive.Columns[16].Visible = false;
                        GVCarpetReceive.Columns[17].Visible = false;

                        GVCarpetReceiveEdit.Columns[15].Visible = false;
                        GVCarpetReceiveEdit.Columns[16].Visible = false;
                        GVCarpetReceiveEdit.Columns[17].Visible = false;
                       
                    }
                    else
                    {
                        chkboxManualWeight.Checked = false;
                        txtTotalWeight.Enabled = false;

                        GVCarpetReceive.Columns[15].Visible = true;
                        GVCarpetReceive.Columns[16].Visible = true;
                        GVCarpetReceive.Columns[17].Visible = true;

                        GVCarpetReceiveEdit.Columns[15].Visible = true;
                        GVCarpetReceiveEdit.Columns[16].Visible = true;
                        GVCarpetReceiveEdit.Columns[17].Visible = true;
                    }

                    if (flagfixorweight == 1)
                    {
                        RBLConsumption.SelectedValue = "1";
                    }
                    else if (flagfixorweight == 0)
                    {
                        RBLConsumption.SelectedValue = "0";
                    }
                    if (sampleflag > 0)
                    {
                        chkboxSampleFlag.Checked = true;

                        txtTharri.ReadOnly = false;
                        txtLachhi.ReadOnly = false;
                        txtTar.ReadOnly = false;
                        txtMisc.ReadOnly = false;
                        txtExtra.ReadOnly = false;
                        txtLagat.ReadOnly = false;
                        txtLoss.ReadOnly = false;

                        txtTharri.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["Tharri"].ToString());
                        txtLachhi.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["CYarn"].ToString());
                        txtTar.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["Tar"].ToString());
                        txtMisc.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["Misc"].ToString());
                        txtExtra.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["Extra"].ToString());
                        txtLagat.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["Wool"].ToString());
                        txtLoss.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["Loss"].ToString());
                    }
                    else if (FinalBazaar == 1)
                    {
                        txtTharri.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["Tharri"].ToString());
                        txtLachhi.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["CYarn"].ToString());
                        txtTar.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["Tar"].ToString());
                        txtMisc.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["Misc"].ToString());
                        txtExtra.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["Extra"].ToString());
                        txtLagat.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["Wool"].ToString());
                        txtLoss.Text = string.Format("{0:#0.000}", ds.Tables[0].Rows[0]["Loss"].ToString());

                        txtTharri.ReadOnly = true;
                        txtLachhi.ReadOnly = true;
                        txtTar.ReadOnly = true;
                        txtMisc.ReadOnly = true;
                        txtExtra.ReadOnly = true;
                        txtLagat.ReadOnly = true;
                        txtLoss.ReadOnly = true;
                    }
                    else
                    {
                        chkboxSampleFlag.Checked = false;
                        txtTharri.Text = "0";
                        txtLachhi.Text = "0";
                        txtTar.Text = "0";
                        txtMisc.Text = "0";
                        txtExtra.Text = "0";
                        txtLagat.Text = "0";
                        txtLoss.Text = "0";

                        txtTharri.ReadOnly = true;
                        txtLachhi.ReadOnly = true;
                        txtTar.ReadOnly = true;
                        txtMisc.ReadOnly = true;
                        txtExtra.ReadOnly = true;
                        txtLagat.ReadOnly = true;
                        txtLoss.ReadOnly = true;
                    }

                    //*************************Fill Total Lagat****************************

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        txtTotalLagat.Text = ds.Tables[1].Rows[0]["Wool"].ToString();
                        txtTotalTharri.Text = ds.Tables[1].Rows[0]["Tharri"].ToString();
                        txtTotalTar.Text = ds.Tables[1].Rows[0]["Tar"].ToString();
                        txtTotalLachhi.Text = ds.Tables[1].Rows[0]["Cotton"].ToString();
                        txtTotalMisc.Text = ds.Tables[1].Rows[0]["Misc"].ToString();
                        txtTotalExtra.Text = ds.Tables[1].Rows[0]["Extra"].ToString();
                    }
                    else
                    {
                        txtTotalLagat.Text = "0";
                        txtTotalTharri.Text = "0";
                        txtTotalTar.Text = "0";
                        txtTotalLachhi.Text = "0";
                        txtTotalMisc.Text = "0";
                        txtTotalExtra.Text = "0";

                    }
                }
                else
                {
                    GVCarpetReceiveEdit.DataSource = null;
                    GVCarpetReceiveEdit.DataBind();
                    //GVCarpetReceiveEdit.Visible = false;
                }


            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceiveNew.aspx");
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
        else
        {
            ClearAfterSave();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
       
    }
    private void fillDryWeight()
    {
        string sql = "";
        if (variable.VarNewQualitySize == "1")
        {
            sql = @"select Distinct  PRM.Process_Rec_Id,PRM.ChallanNo,EI.EmpId,EI.EmpName,replace(Convert(nvarchar(11),PRM.ReceiveDate,106),' ','-')ReceiveDate,ITEM_NAME, ITEM_ID ,sum(PRD.qty) as TotalQty,PRM.TotalWeight,PRM.CheckPcs,PRM.CheckWeight,
                    PRM.DryWeight,PRM.DeductionInPer from PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PRM INNER JOIN PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PRD ON PRM.Process_Rec_Id=PRD.Process_Rec_Id
                    INNER JOIN EmpInfo EI ON PRM.EmpId=EI.EmpId
                    INNER JOIN V_FinishedItemDetailNew VFI ON PRD.Item_Finished_Id=VFI.ITEM_FINISHED_ID
                    where PRM.Status<>'Canceled' ";
        }
        if (ChkboxDryWeight.Checked==false)
        {
            sql = sql + "And (PRM.TotalWeight=0  Or PRM.CheckWeight=0) And PRM.FinalBazaarFlag=0 And PRM.ReceiveDate='" + txtDryWeightDate.Text + "'";            
        }
        else if (ChkboxDryWeight.Checked == true)
        {
            sql = sql + "And CheckWeight>0 And TotalWeight>0";            
        }
        if (DDDryType.SelectedIndex >0 && ChkboxDryWeight.Checked == true )
        {
            sql = sql + "And DryWeight=0 And PRM.ReceiveDate<='" + txtDryWeightDate.Text + "' ";
        }
        else if (DDDryType.SelectedIndex == 0 && ChkboxDryWeight.Checked == true)
        {
            sql = sql + " And DryWeight>0 And PRM.ReceiveDate='" + txtDryWeightDate.Text + "'";
        }
        sql = sql + @"group by PRM.Process_Rec_Id,PRM.ChallanNo,EI.EmpId,EI.EmpName,PRM.ReceiveDate,ITEM_NAME, ITEM_ID ,PRM.TotalWeight,PRM.CheckPcs,PRM.CheckWeight,
                    PRM.DryWeight,PRM.DeductionInPer Order By PRM.ChallanNo,EI.EmpName";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GVDryWeight.DataSource = ds;
            GVDryWeight.DataBind();
            GVDryWeight.Visible = true;
        }
        else
        {
            GVDryWeight.Visible = false;
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
        ////****************sql Table Type 

        //if (ViewState["Process_Rec_Id"] == "0")
        //{
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
                        dr["FlagFixOrWeight"] = lblFlagFixOrWeight;
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
        //}
        //else
        //{

        //}


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

        if (Convert.ToDouble(TxtKhapWidth.Text) > 0 && Convert.ToDouble(TxtKhapLength.Text) > 0)
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
                lblArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(BZL, BZW, Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(lblShapeId.Text), UnitId: Convert.ToInt16(DDunit.SelectedValue), RoundFullAreaValue:Convert.ToDouble (chkboxRoundFullArea.Checked==true ? "1" : "0.7853")));
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

        DataTable dt3 = new DataTable();
        dt3 = (DataTable)ViewState["CarpetReceive"];

        //int sum = 0;
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
                    row["Penality"] = lblPenality.Text;

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

            Label lblTotalArea = ((Label)GVCarpetReceive.Rows[i].FindControl("lblTotalArea"));
            lblTotalArea.Text = Convert.ToString(Convert.ToDecimal(lblArea.Text) * Convert.ToDecimal(txtReqQty.Text == "" ? "0" : txtReqQty.Text));

            Label lblFinalWt = ((Label)GVCarpetReceive.Rows[i].FindControl("lblFinalWt"));
            TotalWeight += Convert.ToDecimal(lblFinalWt.Text);

        }

        Label lblGrandTQty = (Label)GVCarpetReceive.FooterRow.FindControl("lblGrandTQty");
        lblGrandTQty.Text = TotalQty2.ToString();
        Label lblGrandTArea = (Label)GVCarpetReceive.FooterRow.FindControl("lblGrandTArea");
        lblGrandTArea.Text = string.Format("{0:#0.0000}", TotalArea2).ToString();

        if (TBToalWt == 0)
        {
            TBToalWt = Convert.ToDecimal(txtTotalWeight.Text);
        }

        txtTotalWeight.Text = Convert.ToString(Convert.ToDecimal(TBToalWt) + TotalWeight);
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
                    }
                    dt3.AcceptChanges();
                    row.SetModified();
                    ViewState["CarpetReceive"] = dt3;
                }
            }
        }

        CalQtyTotalArea();

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

            if (PenMode=="Edit" && ViewState["CarpetReceivePenEdit"] != null)
            {
                DataTable dt = (DataTable)ViewState["CarpetReceivePenEdit"];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (ViewState["ProcessRecDetailID"].ToString() == dt.Rows[i]["Process_Rec_Detail_Id"].ToString() && lblPenalityId.Text == dt.Rows[i]["PenalityId"].ToString())
                    {
                        chkoutItem.Checked = true;
                        txtQty.Text = dt.Rows[i]["Qty"].ToString();
                        txtAmt.Text = dt.Rows[i]["Amount"].ToString();

                        //DataView dv = new DataView(dt);
                        //dv.RowFilter = "SrNo =" + ViewState["SrNo"].ToString();
                        //GVPenalty.DataSource = dv;
                        //GVPenalty.DataBind();

                    }
                }
            }

            if (PenMode=="Add" && ViewState["CarpetReceivePen"]!= null)
            {
                DataTable dt = (DataTable)ViewState["CarpetReceivePen"];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (ViewState["SrNo"].ToString() == dt.Rows[i]["SrNo"].ToString() && lblPenalityId.Text == dt.Rows[i]["PenalityId"].ToString())
                    {
                        chkoutItem.Checked = true;
                        txtQty.Text = dt.Rows[i]["Qty"].ToString();
                        txtAmt.Text = dt.Rows[i]["Amt"].ToString();

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

            if (PenMode=="Edit" && ViewState["CarpetReceivePenEdit"] != null)
            {
                DataTable dt4 = new DataTable();
                dt4 = (DataTable)ViewState["CarpetReceivePenEdit"];

                int m = 0;
                for (int n = 0; n < dt4.Rows.Count; n++)
                {
                    if (dt4.Rows[n]["Process_Rec_Detail_Id"].ToString() == ViewState["ProcessRecDetailID"].ToString() && dt4.Rows[n]["PenalityId"].ToString() == lblPenalityId.Text)
                    {
                        m = 1;

                        dt4.Rows.RemoveAt(n);
                        dt4.AcceptChanges();
                        break;
                    }
                }
            }

            if (PenMode=="Add" && ViewState["CarpetReceivePen"] != null)
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

            if (PenMode=="Edit" && ViewState["CarpetReceivePenEdit"]!= null)
            {
                if (Convert.ToInt32(txtQty.Text) > Convert.ToInt32(ViewState["RQtyEdit"]))
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
                            double TAmt = Convert.ToDouble(ViewState["AreaEdit"]) * Convert.ToDouble(txtQty.Text) * Convert.ToDouble(lblRate.Text);
                            // lblAmt.Text = Convert.ToString(TAmt);
                            txtAmt.Text = Convert.ToString(TAmt);
                        }
                    }
                }
            }
            else if(PenMode=="Add")
            {

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
            if (PenMode=="Edit" && ViewState["CarpetReceivePenEdit"] != null)
            {
                DataTable dt4 = new DataTable();
                dt4 = (DataTable)ViewState["CarpetReceivePenEdit"];

                int m = 0;
                for (int n = 0; n < dt4.Rows.Count; n++)
                {
                    if (dt4.Rows[n]["Process_Rec_Detail_Id"].ToString() == ViewState["ProcessRecDetailID"].ToString() && dt4.Rows[n]["PenalityId"].ToString() == lblPenalityId)
                    {
                        m = 1;

                        dt4.Rows[n]["Amount"] = txtAmt.Text;

                        dt4.AcceptChanges();
                        ViewState["CarpetReceivePenEdit"] = dt4;
                    }
                }
            }


            if (PenMode == "Add" && ViewState["CarpetReceivePen"] != null)
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
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = gvRow.RowIndex;
        Label lnk1 = (Label)gvRow.FindControl("lblText");
        PenMode = lnk1.Text;

        if (PenMode=="Edit" && ViewState["CarpetReceivePenEdit"].ToString() != "")
        {
            ViewState["RQtyEdit"] = "";
            ViewState["AreaEdit"] = "";
            ViewState["ProcessRecDetailID"] = "";

            // Show ModalPopUpExtender.
            ModalPopupExtender1.Show();

            //GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            //int index = gvRow.RowIndex;
            LinkButton lnk = (LinkButton)gvRow.FindControl("popup");
            string myScript = lnk.Text;

            Label lblProcessRecDetailId = (Label)gvRow.FindControl("lblProcessRecDetailId");
            Label lblQualityId = (Label)gvRow.FindControl("lblQualityId");
            TextBox txtReqQtyEdit = (TextBox)gvRow.FindControl("txtReqQtyEdit");
            Label lblArea = (Label)gvRow.FindControl("lblArea");
            Label lblItemId = (Label)gvRow.FindControl("lblItemId");

            int Qualityid = Convert.ToInt32(lblQualityId.Text);
            int ItemId = Convert.ToInt32(lblItemId.Text);
            ViewState["RQtyEdit"] = txtReqQtyEdit.Text;
            ViewState["AreaEdit"] = lblArea.Text;
            ViewState["ProcessRecDetailID"] = lblProcessRecDetailId.Text;
            FillPenalityGrid(ItemId);
        }
        else if(PenMode=="Add")
        {
            ViewState["RQty"] = "";
            ViewState["Area"] = "";
            ViewState["SrNo"] = "";

            // Show ModalPopUpExtender.
            ModalPopupExtender1.Show();

            //GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            //int index = gvRow.RowIndex;
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
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        ModalPopupExtender1.Hide();
        name = "";

        if (PenMode=="Edit" && ViewState["CarpetReceivePenEdit"] != null)
        {
            DataTable dtrecordsPen = new DataTable();

            dtrecordsPen = (DataTable)ViewState["CarpetReceivePenEdit"];

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
                        if (dtrecordsPen.Rows[n]["Process_Rec_Detail_Id"].ToString() == ViewState["ProcessRecDetailID"].ToString() && dtrecordsPen.Rows[n]["PenalityId"].ToString() == lblPenalityId.Text)
                        {
                            m = 1;
                            dtrecordsPen.Rows.RemoveAt(n);
                            dtrecordsPen.AcceptChanges();
                            break;
                        }
                    }

                    DataRow dr = dtrecordsPen.NewRow();
                    dr["WPenalityId"] = 0;
                    dr["PenalityId"] = lblPenalityId.Text;
                    dr["PenalityName"] = lblPenalityName.Text;
                    dr["Qty"] = txtQty.Text;
                    dr["Rate"] = lblRate.Text == "" ? "0" : lblRate.Text;
                    dr["Amount"] = txtAmt.Text == "" ? "0" : txtAmt.Text;
                    dr["PenalityType"] = lblPenalityType.Text;
                    dr["Process_Rec_Id"] = ViewState["Process_Rec_Id"];
                    dr["Process_Rec_Detail_Id"] = ViewState["ProcessRecDetailID"];

                    dtrecordsPen.Rows.Add(dr);
                    ViewState["CarpetReceivePenEdit"] = dtrecordsPen;
                    name += lblPenalityName.Text + " " + txtQty.Text + "-" + txtAmt.Text + ",";
                    //name += lblPenalityId.Text + "_" + lblPenalityName.Text + " " + txtQty.Text + "-" + txtAmt.Text + ",";
                }
            }

            for (int i = 0; i < GVCarpetReceiveEdit.Rows.Count; i++)
            {
                Label lblPenalityName = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblPenalityName"));
                Label lblProcessRecDetailId = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblProcessRecDetailId"));

                if (lblProcessRecDetailId.Text == ViewState["ProcessRecDetailID"].ToString())
                {
                    lblPenalityName.Text = name;
                    lblPenalityName.ToolTip = name;
                }
            }
        }
        else if(PenMode=="Add")
        {
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
                    //name += lblPenalityId.Text + "_" + lblPenalityName.Text + " " + txtQty.Text + "-" + txtAmt.Text + ",";
                    name += lblPenalityName.Text + " " + txtQty.Text + "-" + txtAmt.Text + ",";
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
        }
    }
    protected void GVCarpetReceive_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int Issuedqty = 0;
        string txtReqQty = ((TextBox)GVCarpetReceive.Rows[e.RowIndex].FindControl("txtReqQty")).Text;

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

        txtTotalWeight.Text = Convert.ToString(Convert.ToDecimal(txtTotalWeight.Text) - Convert.ToDecimal(lblFinalWt));
        BQty(Convert.ToInt32(Issue_Detail_Id), 0);
    }
    protected void UpdateWidthLength(object sender)
    {
        string HKStrWL = "";
        double BZW = 0, BZL = 0;
        TextBox txt3 = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txt3.Parent.Parent;
        Label lblProcessRecDetailId = (Label)gvRow.FindControl("lblProcessRecDetailId");
        Label lblShape = (Label)gvRow.FindControl("lblShape");
        Label lblKhapWidth = (Label)gvRow.FindControl("lblKhapWidth");
        Label lblKhapLength = (Label)gvRow.FindControl("lblKhapLength");
        Label lblAfterKhapSize = (Label)gvRow.FindControl("lblAfterKhapSize");

        TextBox txtAfterKhapSizeEdit = (TextBox)gvRow.FindControl("txtAfterKhapSizeEdit");

        Label lblArea = (Label)gvRow.FindControl("lblArea");
        Label lblShapeId = (Label)gvRow.FindControl("lblShapeId");

        TextBox txtWidthEdit = (TextBox)gvRow.FindControl("txtWidthEdit");
        TextBox txtLengthEdit = (TextBox)gvRow.FindControl("txtLengthEdit");
        //TextBox TxtAfterKhapSize = (TextBox)gvRow.FindControl("TxtAfterKhapSize");

        Label hnKhapWidth = (Label)gvRow.FindControl("hnKhapWidth");
        Label hnKhapLength = (Label)gvRow.FindControl("hnKhapLength");
        Label hnAfterKhapSize = (Label)gvRow.FindControl("hnAfterKhapSize");

        if (chkboxSampleFlag.Checked == true)
        {
            hnAfterKhapSize.Text = txtAfterKhapSizeEdit.Text;            
        } 

        Label lblQty = (Label)gvRow.FindControl("lblQty");
        TextBox txtReqQty = (TextBox)gvRow.FindControl("txtReqQtyEdit");     

        Label Item = (Label)gvRow.FindControl("lblItemId");
        Label lblQualityid = (Label)gvRow.FindControl("lblQualityId");
        Label IssueDetailId = (Label)gvRow.FindControl("Issue_Detail_Id");
        TextBox txtReqQtyEdit = (TextBox)gvRow.FindControl("txtReqQtyEdit");

        TextBox txtRecWtEdit = (TextBox)gvRow.FindControl("txtRecWtEdit");
        Label lblActualWtEdit = (Label)gvRow.FindControl("lblActualWtEdit");       
        Label lblFinalWtEdit = (Label)gvRow.FindControl("lblFinalWtEdit");
        Label lblTotalLagatEdit = (Label)gvRow.FindControl("lblTotalLagatEdit");

        if (Convert.ToDouble(txtWidthEdit.Text) > 0 && Convert.ToDouble(txtLengthEdit.Text) > 0)
        {
            //lblAfterKhapSize.Text = Convert.ToString(UtilityModule.Calculate_Process_Receive_Area(Convert.ToDouble(txtWidthEdit.Text), Convert.ToDouble(txtLengthEdit.Text), txtAfterKhapSizeEdit.Text, Convert.ToInt32(DDunit.SelectedValue), Convert.ToDouble(hnKhapWidth.Text), Convert.ToDouble(hnKhapLength.Text), hnAfterKhapSize.Text));
            txtAfterKhapSizeEdit.Text = Convert.ToString(UtilityModule.Calculate_Process_Receive_Area(Convert.ToDouble(txtWidthEdit.Text), Convert.ToDouble(txtLengthEdit.Text), txtAfterKhapSizeEdit.Text, Convert.ToInt32(DDunit.SelectedValue), Convert.ToDouble(hnKhapWidth.Text), Convert.ToDouble(hnKhapLength.Text), hnAfterKhapSize.Text));
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Plz Enter Proper Size!');", true);
        }

        if (DDcaltype.SelectedIndex >= 0)
        {
           // HKStrWL = string.Format("{0:#0.00}", lblAfterKhapSize.Text);
            HKStrWL = string.Format("{0:#0.00}", txtAfterKhapSizeEdit.Text);
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
            lblActualWtEdit.Text = Convert.ToString(Math.Round(Convert.ToDouble(lblTotalLagatEdit.Text) * Convert.ToDouble(lblArea.Text) * Convert.ToInt32(txtReqQtyEdit.Text), 3));
        }
        else
        {
            lblActualWtEdit.Text = "0";
        }

        if (chkboxSampleFlag.Checked == false)
        {
            if (Convert.ToDouble(txtRecWtEdit.Text) > Convert.ToDouble(lblActualWtEdit.Text))
            {
                lblFinalWtEdit.Text = lblActualWtEdit.Text;
            }
            else
            {
                lblFinalWtEdit.Text = txtRecWtEdit.Text;
            }
        }
        else
        {
            lblFinalWtEdit.Text = txtRecWtEdit.Text;
        }
        ////''*******End Calculate Actual Weight
        
    }
    decimal TotalQtyEdit = 0;
    decimal TotalAreaEdit = 0;
    decimal TotalWeightEdit = 0;
    protected void CalQtyTotalAreaEdit()
    {
        for (int i = 0; i < GVCarpetReceiveEdit.Rows.Count; i++)
        {                      
            Label lblQty = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("hnlblQty"));
            TotalQtyEdit += Convert.ToDecimal(lblQty.Text == "" ? "0" : lblQty.Text);
            Label lblArea = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblArea"));
            TotalAreaEdit += Convert.ToDecimal(lblArea.Text) * Convert.ToDecimal(lblQty.Text == "" ? "0" : lblQty.Text);

            Label lblTotalArea = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblTotalArea"));
            lblTotalArea.Text = Convert.ToString(Convert.ToDecimal(lblArea.Text) * Convert.ToDecimal(lblQty.Text == "" ? "0" : lblQty.Text));

            Label lblTotalAmt = (Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblTotalAmt");
            Label lblRate = (Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblRate");            

            TextBox txtRateEdit = (TextBox)GVCarpetReceiveEdit.Rows[i].FindControl("txtRateEdit");
            if (txtRateEdit!= null)
            {
                lblTotalAmt.Text = Convert.ToString(Convert.ToDecimal(lblTotalArea.Text) * Convert.ToDecimal(txtRateEdit.Text));
            }
            else
            {
               lblTotalAmt.Text = Convert.ToString(Convert.ToDecimal(lblTotalArea.Text) * Convert.ToDecimal(lblRate.Text));
            }

            Label lblFinalWtEdit = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblFinalWtEdit"));
            TotalWeightEdit += Convert.ToDecimal(lblFinalWtEdit.Text);
        }

        Label lblGrandTQty = (Label)GVCarpetReceiveEdit.FooterRow.FindControl("lblGrandTQty");
        lblGrandTQty.Text = TotalQtyEdit.ToString();
        Label lblGrandTArea = (Label)GVCarpetReceiveEdit.FooterRow.FindControl("lblGrandTArea");
        lblGrandTArea.Text = string.Format("{0:#0.0000}", TotalAreaEdit).ToString();

        if (chkboxManualWeight.Checked == false)
        {
            txtTotalWeight.Text = Convert.ToString(TotalWeightEdit);
        }

        
    }
    public Boolean BQtyEdit(int IssueDetailId, int rowindex)
    {
        llMessageBox.Visible = false;
        llMessageBox.Text = "";

        int Balqty = 0;
        int Issuedqty = 0;
        int Count2 = dgorder.Rows.Count;
        int Count3 = GVCarpetReceiveEdit.Rows.Count;

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

        if (GVCarpetReceiveEdit.Rows.Count > 0)
        {
            //for (int k = 0; k < Count3; k++)
            //{
            string Issue_Detail_Id = ((Label)(GVCarpetReceiveEdit.Rows[rowindex].FindControl("Issue_Detail_Id"))).Text;
            string lblQtyEdit = ((Label)(GVCarpetReceiveEdit.Rows[rowindex].FindControl("lblQtyEdit"))).Text;
            string txtReqQtyEdit = ((TextBox)(GVCarpetReceiveEdit.Rows[rowindex].FindControl("txtReqQtyEdit"))).Text;

            Issuedqty = Convert.ToInt32(txtReqQtyEdit);
            Balqty = Balqty + Convert.ToInt32(lblQtyEdit);

            //if (IssueDetailId == Convert.ToInt32(Issue_Detail_Id))
            //{
            //    Issuedqty += Convert.ToInt32(lblQty);
            //}
            //}
        }
        if (Issuedqty > Balqty)
        {
            llMessageBox.Visible = true;
            llMessageBox.Text = "Receive Qty Cannot greater than order qty!!";
            TextBox RecQty = (TextBox)GVCarpetReceiveEdit.Rows[rowindex].FindControl("txtReqQtyEdit");
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
    protected void txtWidthEdit_TextChanged(object sender, EventArgs e)
    {
        UpdateWidthLength(sender);
        CalQtyTotalAreaEdit();
    }
    protected void txtLengthEdit_TextChanged(object sender, EventArgs e)
    {
        UpdateWidthLength(sender);
        CalQtyTotalAreaEdit();
    }
    protected void txtAfterKhapSizeEdit_TextChanged(object sender, EventArgs e)
    {
        UpdateWidthLength(sender);
        CalQtyTotalAreaEdit();
    }
    protected void txtRateEdit_TextChanged(object sender, EventArgs e)
    {
        TextBox txt2 = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txt2.Parent.Parent;
        txt2.Focus();

        Label lblProcessRecDetailId = (Label)gvRow.FindControl("lblProcessRecDetailId");
        Label lblShape = (Label)gvRow.FindControl("lblShape");

        TextBox txtReqQtyEdit = (TextBox)gvRow.FindControl("txtReqQtyEdit");

        TextBox txtRecWtEdit = (TextBox)gvRow.FindControl("txtRecWtEdit");
        Label lblArea = (Label)gvRow.FindControl("lblArea");       

        TextBox txtRateEdit = (TextBox)gvRow.FindControl("txtRateEdit");
        TextBox txtCommEdit = (TextBox)gvRow.FindControl("txtCommEdit");
        Label lblTotalAmt = (Label)gvRow.FindControl("lblTotalAmt");       

        if (DDcaltype.SelectedValue == "0" || DDcaltype.SelectedValue == "2" || DDcaltype.SelectedValue == "3" || DDcaltype.SelectedValue == "4")
        {  
            lblTotalAmt.Text =Convert.ToString(Math.Round(Convert.ToDecimal(lblArea.Text) * Convert.ToInt32(txtReqQtyEdit.Text) * Convert.ToDecimal(txtRateEdit.Text),4));
        }
        if (DDcaltype.SelectedValue == "1")
        {
            lblTotalAmt.Text = Convert.ToString(Math.Round(Convert.ToInt32(txtReqQtyEdit.Text) * Convert.ToDecimal(txtRateEdit.Text), 2)); 
        }       
        CalQtyTotalAreaEdit();
    }
    //protected void txtCommEdit_TextChanged(object sender, EventArgs e)
    //{
    //    TextBox txt = (TextBox)sender;
    //    GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
    //    txt.Focus();

    //    Label lblProcessRecDetailId = (Label)gvRow.FindControl("lblProcessRecDetailId");
    //    Label lblShape = (Label)gvRow.FindControl("lblShape");

    //    TextBox txtReqQtyEdit = (TextBox)gvRow.FindControl("txtReqQtyEdit");

    //    TextBox txtRecWtEdit = (TextBox)gvRow.FindControl("txtRecWtEdit");
    //    Label lblArea = (Label)gvRow.FindControl("lblArea");

    //    TextBox txtRateEdit = (TextBox)gvRow.FindControl("txtRateEdit");
    //    TextBox txtCommEdit = (TextBox)gvRow.FindControl("txtCommEdit");
    //    Label lblTotalAmt = (Label)gvRow.FindControl("lblTotalAmt");
    //    Label lblTotalComm = (Label)gvRow.FindControl("lblTotalComm");

    //    if (DDcaltype.SelectedValue == "0" || DDcaltype.SelectedValue == "2" || DDcaltype.SelectedValue == "3" || DDcaltype.SelectedValue == "4")
    //    {
    //        lblTotalAmt.Text = Convert.ToString(Math.Round(Convert.ToDecimal(lblArea.Text) * Convert.ToInt32(txtReqQtyEdit.Text) * Convert.ToDecimal(txtRateEdit.Text), 2));
    //        lblTotalComm.Text = Convert.ToString(Math.Round(Convert.ToDecimal(lblArea.Text) * Convert.ToInt32(txtReqQtyEdit.Text) * Convert.ToDecimal(txtCommEdit.Text), 2));

    //    }
    //    if (DDcaltype.SelectedValue == "1")
    //    {
    //        lblTotalAmt.Text = Convert.ToString(Math.Round(Convert.ToInt32(txtReqQtyEdit.Text) * Convert.ToDecimal(txtRateEdit.Text), 2));
    //        lblTotalComm.Text = Convert.ToString(Math.Round(Convert.ToInt32(txtReqQtyEdit.Text) * Convert.ToDecimal(txtCommEdit.Text), 2));

    //    }
    //    CalQtyTotalAreaEdit();
        
    //}
    protected void txtRecWtEdit_TextChanged(object sender, EventArgs e)
    {
        TextBox txt1 = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txt1.Parent.Parent;
        txt1.Focus();

        //Label lblSrNo = (Label)gvRow.FindControl("lblSrNo");
        //Label lblShape = (Label)gvRow.FindControl("lblShape");

        TextBox txtRecWtEdit = (TextBox)gvRow.FindControl("txtRecWtEdit");
        TextBox txtReqQtyEdit = (TextBox)gvRow.FindControl("txtReqQtyEdit");

       
        Label lblArea = (Label)gvRow.FindControl("lblArea");
        Label lblActualWtEdit = (Label)gvRow.FindControl("lblActualWtEdit");       
        Label lblFinalWtEdit = (Label)gvRow.FindControl("lblFinalWtEdit");
        Label lblTotalLagatEdit = (Label)gvRow.FindControl("lblTotalLagatEdit");
        Label lblFinishedId = (Label)gvRow.FindControl("lblFinishedId");

        ////''*******Start Calculate Actual Weight
        if (chkboxSampleFlag.Checked == false)
        {
            lblActualWtEdit.Text = Convert.ToString(Math.Round(Convert.ToDouble(lblTotalLagatEdit.Text == "" ? "0" : lblTotalLagatEdit.Text) * Convert.ToDouble(lblArea.Text) * Convert.ToInt32(txtReqQtyEdit.Text == "" ? "0" : txtReqQtyEdit.Text), 3));
        }
        else
        {
            lblActualWtEdit.Text = "0";
        }

        if (chkboxSampleFlag.Checked == false)
        {
            if (Convert.ToDouble(txtRecWtEdit.Text) > Convert.ToDouble(lblActualWtEdit.Text))
            {
                lblFinalWtEdit.Text = lblActualWtEdit.Text;
            }
            else
            {
                lblFinalWtEdit.Text = txtRecWtEdit.Text;
            }

        }
        else
        {
            lblFinalWtEdit.Text = txtRecWtEdit.Text;
        }
        ////''*******End Calculate Actual Weight       

        CalQtyTotalAreaEdit();
    }
    protected void txtReqQtyEdit_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
        txt.Focus();

        Label lblProcessRecDetailId = (Label)gvRow.FindControl("lblProcessRecDetailId");
        Label lblShape = (Label)gvRow.FindControl("lblShape");

        TextBox txtReqQtyEdit = (TextBox)gvRow.FindControl("txtReqQtyEdit");
        Label hnlblQty = (Label)gvRow.FindControl("hnlblQty");


        Label Item = (Label)gvRow.FindControl("lblItemId");
        Label lblQualityid = (Label)gvRow.FindControl("lblQualityId");
        Label IssueDetailId = (Label)gvRow.FindControl("Issue_Detail_Id");
        //Label hnBalQty = (Label)gvRow.FindControl("hnBalQty");  

        TextBox txtRecWtEdit = (TextBox)gvRow.FindControl("txtRecWtEdit");
        Label lblArea = (Label)gvRow.FindControl("lblArea");
        Label lblActualWtEdit = (Label)gvRow.FindControl("lblActualWtEdit");
        Label lblFinalWtEdit = (Label)gvRow.FindControl("lblFinalWtEdit");
        Label lblTotalLagatEdit = (Label)gvRow.FindControl("lblTotalLagatEdit");
        Label lblFinishedId = (Label)gvRow.FindControl("lblFinishedId");

      

        if (BQtyEdit(Convert.ToInt32(IssueDetailId.Text), gvRow.RowIndex) == true)
        {
           
        }

        ////''*******Start Calculate Actual Weight
        if (chkboxSampleFlag.Checked == false)
        {
            lblActualWtEdit.Text = Convert.ToString(Math.Round(Convert.ToDouble(lblTotalLagatEdit.Text == "" ? "0" : lblTotalLagatEdit.Text) * Convert.ToDouble(lblArea.Text) * Convert.ToInt32(txtReqQtyEdit.Text == "" ? "0" : txtReqQtyEdit.Text), 3));
        }
        else
        {
            lblActualWtEdit.Text = "0";
        }

        if (chkboxSampleFlag.Checked == false)
        {
            if (Convert.ToDouble(txtRecWtEdit.Text) > Convert.ToDouble(lblActualWtEdit.Text))
            {
                lblFinalWtEdit.Text = lblActualWtEdit.Text;
            }
            else
            {
                lblFinalWtEdit.Text = txtRecWtEdit.Text;
            }

        }
        else
        {
            lblFinalWtEdit.Text = txtRecWtEdit.Text;
        }
        ////''*******End Calculate Actual Weight  

        CalQtyTotalAreaEdit();

    }
    protected void DDJobNameEdit_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddJobNameEdit = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddJobNameEdit.Parent.Parent;
        int idx = row.RowIndex;
        ddJobNameEdit.Focus();

        DropDownList DDJobNameEdit = (DropDownList)row.FindControl("DDJobNameEdit");
        DropDownList DDFinisherNameEdit = (DropDownList)row.FindControl("DDFinisherNameEdit");

        Label lblJobNameEdit = (Label)row.FindControl("lblJobNameEdit");
        Label lblFinisherNameEdit = (Label)row.FindControl("lblFinisherNameEdit");
        Label lblSrNo = (Label)row.FindControl("lblSrNo");

        if (DDJobNameEdit.SelectedIndex > 0)
        {
            lblJobNameEdit.Text = DDJobNameEdit.SelectedValue;
            UtilityModule.ConditionalComboFill(ref DDFinisherNameEdit, "select EI.EmpId,EI.EmpName from EmpProcess EP INNER JOIN EmpInfo EI ON EP.EmpId=EI.EmpId  where Processid=" + DDJobNameEdit.SelectedValue, true, "--Select--");
        }      

    }
    protected void DDFinisherNameEdit_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddFinisherNameEdit = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddFinisherNameEdit.Parent.Parent;
        int idx = row.RowIndex;
        //ddFinisherName.Focus();

        DropDownList DDJobNameEdit = (DropDownList)row.FindControl("DDJobNameEdit");
        DropDownList DDFinisherNameEdit = (DropDownList)row.FindControl("DDFinisherNameEdit");

        Label lblJobNameEdit = (Label)row.FindControl("lblJobNameEdit");
        Label lblFinisherNameEdit = (Label)row.FindControl("lblFinisherNameEdit");
        Label lblProcessRecDetailId = (Label)row.FindControl("lblProcessRecDetailId");

        if (DDJobNameEdit.SelectedIndex > 0)
        {
            lblJobNameEdit.Text = DDJobNameEdit.SelectedValue;

            //UtilityModule.ConditionalComboFill(ref DDFinisherName, "select EI.EmpId,EI.EmpName from EmpProcess EP INNER JOIN EmpInfo EI ON EP.EmpId=EI.EmpId  where Processid=" + DDJobName.SelectedValue, true, "--Select--");
            lblFinisherNameEdit.Text = DDFinisherNameEdit.SelectedValue;
        }
       
    }
    protected void DDType_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    decimal TotalTDS = 0;
    decimal TotalPenAmt = 0;
    decimal TotalComm = 0;
    decimal TotalGrandAmt = 0;
    decimal TotalNetAmt = 0;
    decimal GrandTotalComm = 0;
    decimal TotalLagat = 0;
    decimal TotalRecWt= 0;
    decimal TotalActualWt = 0;
    decimal TotalFinalWt = 0;
    protected void GVCarpetReceiveEdit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            

            Label lblQty = (Label)e.Row.FindControl("hnlblQty");
            TotalQty += Convert.ToDecimal(lblQty.Text == null ? "0" : lblQty.Text);
            Label lblArea = (Label)e.Row.FindControl("lblArea");
            TotalArea += Convert.ToDecimal(lblArea.Text) * Convert.ToDecimal(lblQty.Text);
           
            Label lblTotalAmt = (Label)e.Row.FindControl("lblTotalAmt");

            Label lblTotalArea = (Label)e.Row.FindControl("lblTotalArea");
            lblTotalArea.Text = Convert.ToString(Convert.ToDecimal(lblArea.Text) * Convert.ToDecimal(lblQty.Text));
          

            Label lblTDSAmt = (Label)e.Row.FindControl("lblTDSAmt");
            TotalTDS += Convert.ToDecimal(lblTDSAmt.Text);
            lblTotalTDS.Text =Convert.ToString(TotalTDS);

            Label lblPenAmt = (Label)e.Row.FindControl("lblPenAmt");
            TotalPenAmt += Convert.ToDecimal(lblPenAmt.Text);            

            Label lblLagat = (Label)e.Row.FindControl("lblLagat");
            TotalLagat += Convert.ToDecimal(lblLagat.Text);
            if (e.Row.RowType == DataControlRowType.DataRow && GVCarpetReceiveEdit.EditIndex == e.Row.RowIndex)
            {
                TextBox txtRateEdit = (TextBox)e.Row.FindControl("txtRateEdit");
                lblTotalAmt.Text = Convert.ToString(Convert.ToDecimal(lblTotalArea.Text) * Convert.ToDecimal(txtRateEdit.Text));
                TotalGrandAmt += (Convert.ToDecimal(lblTotalArea.Text) * Convert.ToDecimal(txtRateEdit.Text));
                TextBox txtCommEdit = (TextBox)e.Row.FindControl("txtCommEdit");
                TotalComm += Convert.ToDecimal(txtCommEdit.Text);
                GrandTotalComm += (Convert.ToDecimal(lblArea.Text) * Convert.ToDecimal(txtCommEdit.Text) * Convert.ToDecimal(lblQty.Text));
                Label lblRecWtEdit = (Label)e.Row.FindControl("lblRecWtEdit");
                TotalRecWt += Convert.ToDecimal(lblRecWtEdit.Text);
            }
            else
            {
                Label lblRate = (Label)e.Row.FindControl("lblRate");
                lblTotalAmt.Text = Convert.ToString(Convert.ToDecimal(lblTotalArea.Text) * Convert.ToDecimal(lblRate.Text));
                TotalGrandAmt += (Convert.ToDecimal(lblTotalArea.Text) * Convert.ToDecimal(lblRate.Text));
                Label lblComm = (Label)e.Row.FindControl("lblComm");
                TotalComm += Convert.ToDecimal(lblComm.Text);
                GrandTotalComm += (Convert.ToDecimal(lblArea.Text) * Convert.ToDecimal(lblComm.Text) * Convert.ToDecimal(lblQty.Text));
                Label lblRecWt = (Label)e.Row.FindControl("lblRecWt");
                TotalRecWt += Convert.ToDecimal(lblRecWt.Text);
            }

            Label lblActualWtEdit = (Label)e.Row.FindControl("lblActualWtEdit");
            TotalActualWt += Convert.ToDecimal(lblActualWtEdit.Text);

            Label lblFinalWtEdit = (Label)e.Row.FindControl("lblFinalWtEdit");
            TotalFinalWt += Convert.ToDecimal(lblFinalWtEdit.Text); 
            
            //Label lblActualWtEdit = (Label)e.Row.FindControl("lblActualWtEdit");
            //Label lblRecWtEdit = (Label)e.Row.FindControl("lblRecWtEdit");
            //Label lblRecWt = (Label)e.Row.FindControl("lblRecWt");
            //Label lblFinalWtEdit = (Label)e.Row.FindControl("lblFinalWtEdit");
            //Label lblTotalLagatEdit = (Label)e.Row.FindControl("lblTotalLagatEdit");
            //Label lblFinishedId = (Label)e.Row.FindControl("lblFinishedId");
            //Label lblTypeId = (Label)e.Row.FindControl("lblTypeId");

            //lblTotalLagatEdit.Text =Convert.ToString(UtilityModule.BAZAAR_CONSUMPTION_FOR_ACTUAL_WEIGHT(Convert.ToInt32(lblFinishedId.Text), Convert.ToInt32(DDunit.SelectedValue), TypeId: Convert.ToInt32(lblTypeId.Text), effectivedate: TxtRecDate.Text));
           
        }
        if (e.Row.RowType == DataControlRowType.DataRow && GVCarpetReceiveEdit.EditIndex == e.Row.RowIndex)
        {            

            Label lblProcessRecDetailId = (Label)e.Row.FindControl("lblProcessRecDetailId");
            Label lblJobName = (Label)e.Row.FindControl("lblJobName");
            Label lblJobNameEdit = (Label)e.Row.FindControl("lblJobNameEdit");
            Label lblFinisherName = (Label)e.Row.FindControl("lblFinisherName");
            Label lblFinisherNameEdit = (Label)e.Row.FindControl("lblFinisherNameEdit");
            Label lblTypeEdit = (Label)e.Row.FindControl("lblTypeEdit");
            DropDownList DDJobNameEdit = ((DropDownList)e.Row.FindControl("DDJobNameEdit"));
            DropDownList DDFinisherNameEdit = ((DropDownList)e.Row.FindControl("DDFinisherNameEdit"));
            DropDownList DDType = ((DropDownList)e.Row.FindControl("DDType"));

            UtilityModule.ConditionalComboFill(ref DDJobNameEdit, "select PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER where ProcessType=1 and PROCESS_NAME_ID!=1 order by PROCESS_NAME_ID", true, "--Select--");
            
            //if (lblJobName.Text!=null || lblJobName.Text!="")
            //{
            // Get the data from DB and bind the dropdownlist
            DDJobNameEdit.SelectedValue = lblJobNameEdit.Text;
            UtilityModule.ConditionalComboFill(ref DDFinisherNameEdit, "select EI.EmpId,EI.EmpName from EmpProcess EP INNER JOIN EmpInfo EI ON EP.EmpId=EI.EmpId  where Processid=" + DDJobNameEdit.SelectedValue, true, "--Select--");
            DDFinisherNameEdit.SelectedValue = lblFinisherNameEdit.Text;
            UtilityModule.ConditionalComboFill(ref DDType, "select ID,Type from TDSType", true, "");
            DDType.SelectedValue = lblTypeEdit.Text;
            // }

            TextBox txtRecWtEdit = (TextBox)e.Row.FindControl("txtRecWtEdit");
            Label lblActualWtEdit = (Label)e.Row.FindControl("lblActualWtEdit");
            Label lblRecWtEdit = (Label)e.Row.FindControl("lblRecWtEdit");           
            Label lblFinalWtEdit = (Label)e.Row.FindControl("lblFinalWtEdit");
            Label lblTotalLagatEdit = (Label)e.Row.FindControl("lblTotalLagatEdit");
            Label lblFinishedId = (Label)e.Row.FindControl("lblFinishedId");
            Label lblArea = (Label)e.Row.FindControl("lblArea");
            TextBox txtReqQtyEdit = (TextBox)e.Row.FindControl("txtReqQtyEdit");

            TextBox txtAfterKhapSizeEdit = (TextBox)e.Row.FindControl("txtAfterKhapSizeEdit");
            TextBox txtRateEdit = (TextBox)e.Row.FindControl("txtRateEdit");
            TextBox txtCommEdit = (TextBox)e.Row.FindControl("txtCommEdit");
            TextBox txtfinishingSizeEdit = (TextBox)e.Row.FindControl("txtfinishingSizeEdit");

            lblTotalLagatEdit.Text = UtilityModule.BAZAAR_CONSUMPTION_FOR_ACTUAL_WEIGHT(Convert.ToInt32(lblFinishedId.Text), Convert.ToInt32(DDunit.SelectedValue), TypeId: Convert.ToInt32(DDType.SelectedValue), effectivedate: TxtRecDate.Text).ToString();

            if (chkboxSampleFlag.Checked == false)
            {
                lblActualWtEdit.Text = Convert.ToString(Math.Round(Convert.ToDouble(lblTotalLagatEdit.Text == "" ? "0" : lblTotalLagatEdit.Text) * Convert.ToDouble(lblArea.Text) * Convert.ToInt32(txtReqQtyEdit.Text == "" ? "0" : txtReqQtyEdit.Text), 3));
            }
            else
            {
                lblActualWtEdit.Text = "0";
            }

            if (chkboxSampleFlag.Checked == false)
            {
                if (Convert.ToDouble(txtRecWtEdit.Text) > Convert.ToDouble(lblActualWtEdit.Text))
                {
                    lblFinalWtEdit.Text = lblActualWtEdit.Text;
                }
                else
                {
                    lblFinalWtEdit.Text = txtRecWtEdit.Text;
                }
            }
            else
            {
                lblFinalWtEdit.Text = txtRecWtEdit.Text;
            }
           

            if (chkboxSampleFlag.Checked == true)
            {
                txtAfterKhapSizeEdit.Enabled = true;
                txtRateEdit.Enabled=true;
                txtCommEdit.Enabled=true;
                txtfinishingSizeEdit.Enabled = true;
            }
            else
            {
                txtAfterKhapSizeEdit.Enabled = false;
                txtRateEdit.Enabled = false;
                txtCommEdit.Enabled = false;
                txtfinishingSizeEdit.Enabled = false;
            }

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblGrandTQty = (Label)e.Row.FindControl("lblGrandTQty");
            lblGrandTQty.Text = TotalQty.ToString();
            Label lblGrandTArea = (Label)e.Row.FindControl("lblGrandTArea");
            lblGrandTArea.Text = TotalArea.ToString();
            Label lblTotalPenAmt = (Label)e.Row.FindControl("lblTotalPenAmt");
            lblTotalPenAmt.Text = TotalPenAmt.ToString();
            Label lblTotalTDS = (Label)e.Row.FindControl("lblTotalTDS");
            lblTotalTDS.Text = TotalTDS.ToString();
            Label lblTotalComm = (Label)e.Row.FindControl("lblTotalComm");
            lblTotalComm.Text = TotalComm.ToString();
            Label lblGrandTotalAmt = (Label)e.Row.FindControl("lblGrandTotalAmt");
            lblGrandTotalAmt.Text = TotalGrandAmt.ToString();
            Label lblTotalLagat2 = (Label)e.Row.FindControl("lblTotalLagat");
            lblTotalLagat2.Text = TotalLagat.ToString();

            lblTotalLagat.Text = lblTotalLagat2.Text;

            Label lblGrandTWt = (Label)e.Row.FindControl("lblGrandTWt");
            lblGrandTWt.Text =  TotalRecWt.ToString();
            Label lblGrandTActualWt = (Label)e.Row.FindControl("lblGrandTActualWt");
            lblGrandTActualWt.Text =TotalActualWt.ToString();
            Label lblGrandTFinalWt = (Label)e.Row.FindControl("lblGrandTFinalWt");
            lblGrandTFinalWt.Text =TotalFinalWt.ToString();

            TotalNetAmt = TotalGrandAmt - TotalPenAmt - GrandTotalComm - TotalTDS;
            txtTotalAmt.Text = Convert.ToString(string.Format("{0:#0.00}", TotalGrandAmt));
            txtTotalPen.Text = Convert.ToString(string.Format("{0:#0.00}",TotalPenAmt));
            txtTotalComm.Text = Convert.ToString(string.Format("{0:#0.00}", GrandTotalComm));
            txtTotalTDSAmt.Text = Convert.ToString(string.Format("{0:#0.00}",TotalTDS));
            txtTotalNetAmt.Text =Convert.ToString(string.Format("{0:#0.00}",TotalNetAmt));       

        }
    }
    protected void GVCarpetReceiveEdit_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GVCarpetReceiveEdit.EditIndex = e.NewEditIndex;
        fillCarpetReceive();
        PenalityColumnShowStatus(1);
        //for (int i = 0; i < GVCarpetReceiveEdit.Columns.Count; i++)
        //{            
        //        if (GVCarpetReceiveEdit.Columns[i].HeaderText == "Penality")
        //        {
        //            GVCarpetReceiveEdit.Columns[i].Visible = true;
        //        } 
            
        //}
    }
    protected void GVCarpetReceiveEdit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GVCarpetReceiveEdit.EditIndex = -1;
        fillCarpetReceive();
        PenalityColumnShowStatus(0);
    }
    protected void GVCarpetReceiveEdit_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DataTable dtrecordsPenSave = new DataTable();
        if (ViewState["CarpetReceivePenEdit"] != null)
        {
            dtrecordsPenSave = (DataTable)ViewState["CarpetReceivePenEdit"];
        }
        else if (ViewState["CarpetReceivePenEdit"] == null)
        {
            dtrecordsPenSave.Columns.Add("WPenalityId", typeof(int));
            dtrecordsPenSave.Columns.Add("PenalityId", typeof(int));
            dtrecordsPenSave.Columns.Add("PenalityName", typeof(string));
            dtrecordsPenSave.Columns.Add("Qty", typeof(int));
            dtrecordsPenSave.Columns.Add("Rate", typeof(float));
            dtrecordsPenSave.Columns.Add("Amount", typeof(float));
            dtrecordsPenSave.Columns.Add("PenalityType", typeof(string));
            dtrecordsPenSave.Columns.Add("Process_Rec_Id", typeof(int));
            dtrecordsPenSave.Columns.Add("Process_Rec_Detail_Id", typeof(int));

            DataRow dr = dtrecordsPenSave.NewRow();
            dr["WPenalityId"] = 0;
            dr["PenalityId"] = 0;
            dr["PenalityName"] = "";
            dr["Qty"] = 0;
            dr["Rate"] = 0;
            dr["Amount"] = 0;
            dr["PenalityType"] = null;
            dr["Process_Rec_Id"] = 0;
            dr["Process_Rec_Detail_Id"] = 0;

            dtrecordsPenSave.Rows.Add(dr);
            ViewState["CarpetReceivePenEdit"] = dtrecordsPenSave;
        }

        string lblProcessRecDetailId = ((Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblProcessRecDetailId")).Text;

        TextBox txtWidthEdit = (TextBox)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("txtWidthEdit");
        TextBox txtLengthEdit = (TextBox)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("txtLengthEdit");
        Label lblAfterKhapSize = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblAfterKhapSize");       
        TextBox txtReqQtyEdit = (TextBox)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("txtReqQtyEdit");
        Label lblArea = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblArea");
        Label lblRate = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblRate");        
        Label lblTDSAmt = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblTDSAmt");
        DropDownList DDJobNameEdit = (DropDownList)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("DDJobNameEdit");
        DropDownList DDFinisherNameEdit = (DropDownList)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("DDFinisherNameEdit");
        DropDownList DDType = (DropDownList)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("DDType");
        Label lblPenalityName = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblPenalityName");
        Label lblComm = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblComm");
        Label Issue_Detail_Id = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("Issue_Detail_Id");
        Label lblFinishedId = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblFinishedId");
        Label lblIssueOrderId = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblIssueOrderId");
        Label hnFinisherJobID = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("hnFinisherJobID");
        Label hnFinisherNameID = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("hnFinisherNameID");
        Label lblOrderId = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblOrderId");
        Label lblFlagFixOrWeight = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblFlagFixOrWeight");
        Label lblfinishingSize = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblfinishingSize");
        Label hnlblQty = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("hnlblQty");

        TextBox txtRecWtEdit = (TextBox)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("txtRecWtEdit");
        Label lblActualWtEdit = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblActualWtEdit");
        Label lblRecWtEdit = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblRecWtEdit");
        Label lblFinalWtEdit = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblFinalWtEdit");
        Label lblTotalLagatEdit = (Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblTotalLagatEdit");

        TextBox txtAfterKhapSizeEdit = (TextBox)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("txtAfterKhapSizeEdit");
        TextBox txtRateEdit = (TextBox)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("txtRateEdit");
        TextBox txtCommEdit = (TextBox)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("txtCommEdit");
        TextBox txtfinishingSizeEdit = (TextBox)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("txtfinishingSizeEdit");

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _param = new SqlParameter[45];

            _param[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
            _param[0].Direction = ParameterDirection.InputOutput;
            _param[0].Value = ViewState["Process_Rec_Id"];
            _param[1] = new SqlParameter("@Empid", DDEmployeeNamee.SelectedValue);
            _param[2] = new SqlParameter("@ReceiveDate", TxtRecDate.Text);
            _param[3] = new SqlParameter("@Unitid", DDunit.SelectedValue);
            _param[4] = new SqlParameter("@Userid", Session["varuserid"]);

            _param[5] = new SqlParameter("@Companyid", DDCompanyName.SelectedValue);
            _param[6] = new SqlParameter("@Remarks", TxtRemarks.Text == "" ? "" : TxtRemarks.Text);

            _param[7] = new SqlParameter("@CalType", DDcaltype.SelectedValue);

            _param[8] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);

            _param[9] = new SqlParameter("@MastercompanyId", Session["varcompanyId"]);

            _param[10] = new SqlParameter("@dtrecordsPen", dtrecordsPenSave);
            _param[11] = new SqlParameter("@TotalWeight", txtTotalWeight.Text == "" ? "0" : txtTotalWeight.Text);
            _param[12] = new SqlParameter("@CheckPcs", txtCheckPcs.Text == "" ? "0" : txtCheckPcs.Text);
            _param[13] = new SqlParameter("@CheckWeight", txtCheckWeight.Text == "" ? "0" : txtCheckWeight.Text);

            _param[14] = new SqlParameter("@Process_Rec_Detail_Id", lblProcessRecDetailId);
            _param[15] = new SqlParameter("@Width", txtWidthEdit.Text);
            _param[16] = new SqlParameter("@Length", txtLengthEdit.Text);
            //_param[17] = new SqlParameter("@CalcSize", lblAfterKhapSize.Text);
            _param[17] = new SqlParameter("@CalcSize", txtAfterKhapSizeEdit.Text);
            _param[18] = new SqlParameter("@Qty", txtReqQtyEdit.Text);
            _param[19] = new SqlParameter("@Area", lblArea.Text);
           // _param[20] = new SqlParameter("@Rate", lblRate.Text);
            _param[20] = new SqlParameter("@Rate", txtRateEdit.Text);
            if (DDcaltype.SelectedValue == "0" || DDcaltype.SelectedValue == "2" || DDcaltype.SelectedValue == "3" || DDcaltype.SelectedValue == "4")
            {
                //_param[21] = new SqlParameter("@Amount", Convert.ToDecimal(lblArea.Text) * Convert.ToInt32(txtReqQtyEdit.Text) * Convert.ToDecimal(lblRate.Text));
                //_param[22] = new SqlParameter("@CommAmt", Convert.ToDecimal(lblArea.Text) * Convert.ToInt32(txtReqQtyEdit.Text) * Convert.ToDecimal(lblComm.Text));

                _param[21] = new SqlParameter("@Amount", Convert.ToDecimal(lblArea.Text) * Convert.ToInt32(txtReqQtyEdit.Text) * Convert.ToDecimal(txtRateEdit.Text));
                _param[22] = new SqlParameter("@CommAmt", Convert.ToDecimal(lblArea.Text) * Convert.ToInt32(txtReqQtyEdit.Text) * Convert.ToDecimal(txtCommEdit.Text));

            }
            if (DDcaltype.SelectedValue == "1")
            {
                //_param[21] = new SqlParameter("@Amount", Convert.ToInt32(txtReqQtyEdit.Text) * Convert.ToDecimal(lblRate.Text));
                //_param[22] = new SqlParameter("@CommAmt", Convert.ToInt32(txtReqQtyEdit.Text) * Convert.ToDecimal(lblComm.Text));

                _param[21] = new SqlParameter("@Amount", Convert.ToInt32(txtReqQtyEdit.Text) * Convert.ToDecimal(txtRateEdit.Text));
                _param[22] = new SqlParameter("@CommAmt", Convert.ToInt32(txtReqQtyEdit.Text) * Convert.ToDecimal(txtCommEdit.Text));
            }
            _param[23] = new SqlParameter("@FinisherJobId", DDJobNameEdit.SelectedValue);
            _param[24] = new SqlParameter("@FinisherNameId", DDFinisherNameEdit.SelectedValue);
            _param[25] = new SqlParameter("@QualityType", 1);
            _param[26] = new SqlParameter("@PRemarks", lblPenalityName.Text);
            //_param[13] = new SqlParameter("@TDSAmt", lblTDSAmt.Text);

            _param[27] = new SqlParameter("@Msgflag", SqlDbType.NVarChar, 200);
            _param[27].Direction = ParameterDirection.Output;
            _param[28] = new SqlParameter("@Issue_Detail_Id", Issue_Detail_Id.Text);
            _param[29] = new SqlParameter("@FinishedId", lblFinishedId.Text);
            _param[30] = new SqlParameter("@IssueOrderId", lblIssueOrderId.Text);
            _param[31] = new SqlParameter("@OldFinisherJobId", hnFinisherJobID.Text);
            _param[32] = new SqlParameter("@OldFinisherNameId", hnFinisherNameID.Text);
            //_param[33] = new SqlParameter("@Comm", lblComm.Text);
            _param[33] = new SqlParameter("@Comm", txtCommEdit.Text);
            _param[34] = new SqlParameter("@OrderId", lblOrderId.Text);
            _param[35] = new SqlParameter("@FlagFixOrWeight", lblFlagFixOrWeight.Text);
           // _param[36] = new SqlParameter("@FinishingSize", lblfinishingSize.Text);
            _param[36] = new SqlParameter("@FinishingSize", txtfinishingSizeEdit.Text);
            _param[37] = new SqlParameter("@OldQty", hnlblQty.Text);
            _param[38] = new SqlParameter("@TDSType", DDType.SelectedValue);
            if(CBTDS.Checked==true)
            {
                checkboxtds=1;
            }
            else
            {
                checkboxtds=0;
            }
            _param[39] = new SqlParameter("@CheckBoxTds", checkboxtds);
            _param[40] = new SqlParameter("@ChallanNo", DDChallanNo.SelectedItem.Text);

            _param[41] = new SqlParameter("@Weight", txtRecWtEdit.Text);
            _param[42] = new SqlParameter("@ActualWt", lblActualWtEdit.Text);
            _param[43] = new SqlParameter("@FinalWt", lblFinalWtEdit.Text);
            int chkboxsampleflag = 0;
            if (chkboxSampleFlag.Checked == true)
            {
                chkboxsampleflag = 1;
            }
            else
            {
                chkboxsampleflag = 0;
            }
            _param[44] = new SqlParameter("@Sampleflag", chkboxsampleflag);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_UpdateFirstProcessReceiveNew]", _param);

            llMessageBox.Visible = true;
            llMessageBox.Text = _param[27].Value.ToString();           
            Tran.Commit();
            GVCarpetReceiveEdit.EditIndex = -1;

            //if (_param[27].Value.ToString() != "")
            //{
            //    llMessageBox.Visible = true;
            //    llMessageBox.Text = _param[27].Value.ToString();
            //}

            //for (int i = 0; i < GVCarpetReceiveEdit.Columns.Count; i++)
            //{
            //    if (GVCarpetReceiveEdit.Columns[i].HeaderText == "Penality")
            //    {
            //        GVCarpetReceiveEdit.Columns[i].Visible = false;
            //    }
            //}
            PenalityColumnShowStatus(0);
            fillCarpetReceive();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceiveNew.aspx");
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
    protected void GVCarpetReceiveEdit_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int Issuedqty = 0;
        string lblQty = ((Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblQty")).Text;

        string lblProcessRecDetailId = ((Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblProcessRecDetailId")).Text;
        string lblItemId = ((Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblItemId")).Text;
        string lblQualityId = ((Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblQualityId")).Text;
        string Issue_Detail_Id = ((Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("Issue_Detail_Id")).Text;
        string lblJobNameDel = ((Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblJobNameDel")).Text;
        string lblFinisherNameDel = ((Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblFinisherNameDel")).Text;
        string lblIssueOrderId = ((Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblIssueOrderId")).Text;
        string lblProcessRecId = ((Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblProcessRecId")).Text;
        string lblFinalWtEdit = ((Label)GVCarpetReceiveEdit.Rows[e.RowIndex].FindControl("lblFinalWtEdit")).Text; 

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _param = new SqlParameter[8];

            _param[0] = new SqlParameter("@ProcessId", SqlDbType.Int);
            _param[1] = new SqlParameter("@ReceiveDetailId", SqlDbType.Int);
            _param[2] = new SqlParameter("@IssueDetailId", SqlDbType.Int);
            _param[3] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
            _param[4] = new SqlParameter("@RowCount", SqlDbType.Int);
            _param[5] = new SqlParameter("@FinishnerJobId", SqlDbType.Int);
            _param[6] = new SqlParameter("@ProcessRecId", SqlDbType.Int);

            _param[0].Value = DDProcessName.SelectedValue;
            _param[1].Value = lblProcessRecDetailId;
            _param[2].Value = Issue_Detail_Id;
            _param[3].Value = lblIssueOrderId;
            _param[4].Value = GVCarpetReceiveEdit.Rows.Count;
            _param[5].Value = lblJobNameDel;
            _param[6].Value = lblProcessRecId;
            _param[7] = new SqlParameter("@msg", SqlDbType.NVarChar, 200);
            _param[7].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_DeleteProcessReceiveDetail]", _param);

            llMessageBox.Visible = true;
            llMessageBox.Text = _param[7].Value.ToString();
            
            Tran.Commit();

            txtTotalWeight.Text = Convert.ToString(Convert.ToDecimal(txtTotalWeight.Text) - Convert.ToDecimal(lblFinalWtEdit));

            UpdateTotalWeightRemarks();
            if (chkFinalBazaar.Checked == true)
            {
                FinalBazaarSubmit();
            }
            else
            {
                fillCarpetReceive();
            }
           
            fillorderdetail();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceiveNew.aspx");
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
    protected void UpdateTDS()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _param = new SqlParameter[6];

            _param[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
            _param[1] = new SqlParameter("@ReceiveDate", SqlDbType.DateTime);
            _param[2] = new SqlParameter("@ProcessId", SqlDbType.Int);
            _param[3] = new SqlParameter("@MastercompanyId", SqlDbType.Int);
            _param[4] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 100);
            _param[5] = new SqlParameter("@CheckBoxTds", SqlDbType.Int);


            _param[0].Value = ViewState["Process_Rec_Id"];
            _param[1].Value = TxtRecDate.Text;
            _param[2].Value = DDProcessName.SelectedValue;
            _param[3].Value = Session["varcompanyId"];
            if (CBTDS.Checked == true)
            {
                checkboxtds = 1;
            }
            else
            {
                checkboxtds = 0;
            }
            _param[5].Value = checkboxtds;

            // _param[7] = new SqlParameter("@msg", SqlDbType.NVarChar, 200);
            _param[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_UpdateTDSInProcessReceiveDetail]", _param);

            llMessageBox.Visible = true;
            llMessageBox.Text = _param[4].Value.ToString();

            Tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + llMessageBox.Text + "');", true);
            //fillCarpetReceive();

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceiveNew.aspx");
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
    //protected void BtnUpdateTDS_Click(object sender,EventArgs e)
    //{
    //    btnclickflag = "";
    //    if (FinalBazaar == 1)
    //    {
    //        btnclickflag = "BtnUpdateTDS";
    //        txtpwd.Focus();
    //        Popup(true);
            
    //    }       
    //    else
    //    {
    //        UpdateTDS();
    //    }        
        
    //}
    protected void DryWeightSubmit()
    {
        txtDryWeightDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        if (txtTotalWeight.Text != "" && txtCheckPcs.Text != "" && txtCheckWeight.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _param = new SqlParameter[16];

                _param[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
                _param[1] = new SqlParameter("@ChallanNo", SqlDbType.Int);
                _param[2] = new SqlParameter("@TotalWeight", SqlDbType.Float);
                _param[3] = new SqlParameter("@CheckPcs", SqlDbType.Float);
                _param[4] = new SqlParameter("@CheckWeight", SqlDbType.Float);
                _param[5] = new SqlParameter("@MastercompanyId", SqlDbType.Int);
                _param[6] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);
                _param[7] = new SqlParameter("@Remarks", SqlDbType.VarChar, 500);
                _param[8] = new SqlParameter("@ProcessId", SqlDbType.Int);

                _param[9] = new SqlParameter("@Wool", SqlDbType.Float);
                _param[10] = new SqlParameter("@Tharri", SqlDbType.Float);
                _param[11] = new SqlParameter("@CYarn", SqlDbType.Float);
                _param[12] = new SqlParameter("@Tar", SqlDbType.Float);
                _param[13] = new SqlParameter("@Misc", SqlDbType.Float);
                _param[14] = new SqlParameter("@Extra", SqlDbType.Float);
                _param[15] = new SqlParameter("@Loss", SqlDbType.Float);

                _param[0].Value = ViewState["Process_Rec_Id"];
                _param[1].Value = DDChallanNo.SelectedValue;
                _param[2].Value = txtTotalWeight.Text;
                _param[3].Value = txtCheckPcs.Text;
                _param[4].Value = txtCheckWeight.Text;
                _param[5].Value = Session["varcompanyId"];
                _param[6].Direction = ParameterDirection.Output;
                _param[7].Value = TxtRemarks.Text;
                _param[8].Value = DDProcessName.SelectedValue;

                _param[9].Value = txtLagat.Text == "" ? "0" : txtLagat.Text;
                _param[10].Value = txtTharri.Text == "" ? "0" : txtTharri.Text;
                _param[11].Value = txtLachhi.Text == "" ? "0" : txtLachhi.Text;
                _param[12].Value = txtTar.Text == "" ? "0" : txtTar.Text;
                _param[13].Value = txtMisc.Text == "" ? "0" : txtMisc.Text;
                _param[14].Value = txtExtra.Text == "" ? "0" : txtExtra.Text;
                _param[15].Value = txtLoss.Text == "" ? "0" : txtLoss.Text;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_UpdateRemarksPcsWeightInProcessReceiveMaster]", _param);

                llMessageBox.Visible = true;
                llMessageBox.Text = _param[6].Value.ToString();
                Tran.Commit();
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + llMessageBox.Text + "');", true);

                if (chkFinalBazaar.Checked == false)
                {
                    fillCarpetReceive();
                }


                if (chkFinalBazaar.Checked == true)
                {
                    if (llMessageBox.Text == "Plz Enter First Dry Weight !!")
                    {
                        ModalPopupExtender2.Show();
                    }
                }

            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceiveNew.aspx");
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
    protected void BtnDryWeight_Click(object sender, EventArgs e)
    {
        ModalPopupExtender2.Show();
        txtDryWeightDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");


        //btnclickflag = "";
        //if (FinalBazaar == 1)
        //{
        //    btnclickflag = "BtnDryWeight";
        //    txtpwd.Focus();
        //    Popup(true);
        //}
        //else
        //{
        //    //DryWeightSubmit();
        //    ModalPopupExtender2.Show();
        //    txtDryWeightDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        //}            
        
    }
    protected void GVDryWeight_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
                if (ChkboxDryWeight.Checked == true)
                {         
                    GVDryWeight.Columns[4].Visible = false;
                    GVDryWeight.Columns[5].Visible = true;
                    GVDryWeight.Columns[6].Visible = false;
                    GVDryWeight.Columns[7].Visible = true;
                    GVDryWeight.Columns[8].Visible = false;
                    GVDryWeight.Columns[9].Visible = true;
                    GVDryWeight.Columns[10].Visible = false;
                    GVDryWeight.Columns[11].Visible = true;
                    GVDryWeight.Columns[12].Visible = true;
                   
                }                
                else if (ChkboxDryWeight.Checked == false)                
                {
                    GVDryWeight.Columns[4].Visible = true;
                    GVDryWeight.Columns[5].Visible = false;
                    GVDryWeight.Columns[6].Visible = true;
                    GVDryWeight.Columns[7].Visible = false;
                    GVDryWeight.Columns[8].Visible = true;
                    GVDryWeight.Columns[9].Visible = false;
                    GVDryWeight.Columns[10].Visible = true;
                    GVDryWeight.Columns[11].Visible = false;
                    GVDryWeight.Columns[12].Visible = false;
                }
            
        }

    }
    protected void BtnSaveDryWeight_Click(object sender, EventArgs e)
    {
        if (ChkboxDryWeight.Checked == true)
        {
            DataTable dtrecordsDryWeight2 = new DataTable();
            if (ViewState["DryWeightEdit2"] != null)
            {
                dtrecordsDryWeight2 = (DataTable)ViewState["DryWeightEdit2"];
            }
            else if (ViewState["DryWeightEdit2"] == null)
            {
                dtrecordsDryWeight2.Columns.Add("DryWeight", typeof(float));
                dtrecordsDryWeight2.Columns.Add("DeductionInPer", typeof(float));                
                dtrecordsDryWeight2.Columns.Add("ChallanNo", typeof(int));
                dtrecordsDryWeight2.Columns.Add("Process_Rec_Id", typeof(int));
                dtrecordsDryWeight2.Columns.Add("EmpId", typeof(int));
                dtrecordsDryWeight2.Columns.Add("ItemId", typeof(int));
            }

            for (int i = 0; i < GVDryWeight.Rows.Count; i++)
            {
                Label lblChallanNo = ((Label)GVDryWeight.Rows[i].FindControl("lblChallanNo"));
                Label lblProcessRecId = ((Label)GVDryWeight.Rows[i].FindControl("lblProcessRecId"));
                Label lblEmpId = ((Label)GVDryWeight.Rows[i].FindControl("lblEmpId"));
                Label lblReceiveDate = ((Label)GVDryWeight.Rows[i].FindControl("lblReceiveDate"));
                Label lblItemId = ((Label)GVDryWeight.Rows[i].FindControl("lblItemId"));
                Label lblTotalWeight = ((Label)GVDryWeight.Rows[i].FindControl("lblTotalWeight"));
                Label lblCheckPcs = ((Label)GVDryWeight.Rows[i].FindControl("lblCheckPcs"));
                Label lblCheckWeight = ((Label)GVDryWeight.Rows[i].FindControl("lblCheckWeight"));
                TextBox txtDriedWeight = ((TextBox)GVDryWeight.Rows[i].FindControl("txtDriedWeight"));
                Label lblDeductionInPer = ((Label)GVDryWeight.Rows[i].FindControl("lblDeductionInPer"));
                TextBox txtTotalWeight = ((TextBox)GVDryWeight.Rows[i].FindControl("txtTotalWeight"));
                TextBox txtCheckPcs = ((TextBox)GVDryWeight.Rows[i].FindControl("txtCheckPcs"));
                TextBox txtCheckWeight = ((TextBox)GVDryWeight.Rows[i].FindControl("txtCheckWeight"));

                if (Convert.ToDecimal(lblTotalWeight.Text) > 0 && Convert.ToInt32(lblCheckPcs.Text) > 0 && Convert.ToDecimal(lblCheckWeight.Text) > 0)
                {
                    DataRow dr = dtrecordsDryWeight2.NewRow();

                    dr["DryWeight"] = txtDriedWeight.Text == "" ? "0" : txtDriedWeight.Text;
                    dr["DeductionInPer"] = lblDeductionInPer.Text == "" ? "0" : lblDeductionInPer.Text;
                    dr["ChallanNo"] = lblChallanNo.Text;
                    dr["Process_Rec_Id"] = lblProcessRecId.Text;
                    dr["EmpId"] = lblEmpId.Text;
                    dr["ItemId"] = lblItemId.Text;

                    dtrecordsDryWeight2.Rows.Add(dr);
                    ViewState["DryWeightEdit2"] = dtrecordsDryWeight2;
                }
            }

                if (dtrecordsDryWeight2.Rows.Count > 0)
                {
                    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlTransaction Tran = con.BeginTransaction();
                    try
                    {
                        SqlParameter[] _param = new SqlParameter[5];

                        //_param[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
                        //_param[1] = new SqlParameter("@ChallanNo", SqlDbType.Int);
                        ////_param[2] = new SqlParameter("@TotalWeight", SqlDbType.Float);
                        ////_param[3] = new SqlParameter("@CheckPcs", SqlDbType.Float);
                        ////_param[4] = new SqlParameter("@CheckWeight", SqlDbType.Float);
                        //_param[2] = new SqlParameter("@DryWeight", SqlDbType.Float);
                        //_param[3] = new SqlParameter("@DeductionInPer", SqlDbType.Float);
                        //_param[4] = new SqlParameter("@EmpId", SqlDbType.Int);
                        //_param[5] = new SqlParameter("@QualityTypeId", SqlDbType.Int);
                        _param[0] = new SqlParameter("@MastercompanyId", SqlDbType.Int);
                        _param[1] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);
                        _param[2] = new SqlParameter("@CheckBoxDry", SqlDbType.Int);
                        _param[3] = new SqlParameter("@ProcessId", SqlDbType.Int);


                        //_param[0].Value = ViewState["Process_Rec_Id"];
                        //_param[1].Value = lblChallanNo.Text;

                        if (ChkboxDryWeight.Checked == true)
                        {
                            checkboxdryweight = 1;
                            //_param[2].Value = lblTotalWeight.Text;
                            //_param[3].Value = lblCheckPcs.Text;
                            //_param[4].Value = lblCheckWeight.Text;
                        }
                        else
                        {
                            checkboxdryweight = 0;
                            //_param[2].Value = txtTotalWeight.Text;
                            //_param[3].Value = txtCheckPcs.Text;
                            //_param[4].Value = txtCheckWeight.Text;
                        }

                        //_param[2].Value = txtDriedWeight.Text;
                        //_param[3].Value = lblDeductionInPer.Text;
                        //_param[4].Value = lblEmpId.Text;
                        //_param[5].Value = lblItemId.Text;
                        _param[0].Value = Session["varcompanyId"];

                        _param[1].Direction = ParameterDirection.Output;
                        _param[2].Value = checkboxdryweight;
                        _param[3].Value = DDProcessName.SelectedValue;
                        _param[4] = new SqlParameter("@dtrecords", dtrecordsDryWeight2);


                        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_UpdateDryWeightInProcessReceiveMaster]", _param);

                        llMessageBox.Visible = true;
                        llMessageBox.Text = _param[1].Value.ToString();
                        Tran.Commit();
                        fillCarpetReceive();

                        DDDryType.Items.Clear();
                        DDDryType.Enabled = false;
                        GVDryWeight.DataSource = "";
                        GVDryWeight.DataBind();
                        ChkboxDryWeight.Checked = false;

                    }
                    catch (Exception ex)
                    {
                        UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceiveNew.aspx");
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
                //else
                //{
                //    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                //}

               
                        
        }
        else if (ChkboxDryWeight.Checked == false)
        {
            DataTable dtrecordsDryWeight = new DataTable();
            if (ViewState["DryWeightEdit"] != null)
            {
                dtrecordsDryWeight = (DataTable)ViewState["DryWeightEdit"];
            }
            else if (ViewState["DryWeightEdit"] == null)
            {
                dtrecordsDryWeight.Columns.Add("TotalWeight", typeof(float));
                dtrecordsDryWeight.Columns.Add("CheckPcs", typeof(float));
                dtrecordsDryWeight.Columns.Add("CheckWeight", typeof(float));
                dtrecordsDryWeight.Columns.Add("ChallanNo", typeof(int));
                dtrecordsDryWeight.Columns.Add("Process_Rec_Id", typeof(int));
                dtrecordsDryWeight.Columns.Add("EmpId", typeof(int));
                dtrecordsDryWeight.Columns.Add("ItemId", typeof(int));
            }

            for (int i = 0; i < GVDryWeight.Rows.Count; i++)
            {
                Label lblChallanNo = ((Label)GVDryWeight.Rows[i].FindControl("lblChallanNo"));
                Label lblProcessRecId = ((Label)GVDryWeight.Rows[i].FindControl("lblProcessRecId"));
                Label lblEmpId = ((Label)GVDryWeight.Rows[i].FindControl("lblEmpId"));
                Label lblReceiveDate = ((Label)GVDryWeight.Rows[i].FindControl("lblReceiveDate"));
                Label lblItemId = ((Label)GVDryWeight.Rows[i].FindControl("lblItemId"));
                Label lblTotalWeight = ((Label)GVDryWeight.Rows[i].FindControl("lblTotalWeight"));
                Label lblCheckPcs = ((Label)GVDryWeight.Rows[i].FindControl("lblCheckPcs"));
                Label lblCheckWeight = ((Label)GVDryWeight.Rows[i].FindControl("lblCheckWeight"));
                TextBox txtDriedWeight = ((TextBox)GVDryWeight.Rows[i].FindControl("txtDriedWeight"));
                Label lblDeductionInPer = ((Label)GVDryWeight.Rows[i].FindControl("lblDeductionInPer"));
                TextBox txtTotalWeight = ((TextBox)GVDryWeight.Rows[i].FindControl("txtTotalWeight"));
                TextBox txtCheckPcs = ((TextBox)GVDryWeight.Rows[i].FindControl("txtCheckPcs"));
                TextBox txtCheckWeight = ((TextBox)GVDryWeight.Rows[i].FindControl("txtCheckWeight"));

                if (Convert.ToDecimal(txtTotalWeight.Text) > 0 && Convert.ToInt32(txtCheckPcs.Text) > 0 && Convert.ToDecimal(txtCheckWeight.Text) > 0)
                {
                    DataRow dr = dtrecordsDryWeight.NewRow();

                    dr["TotalWeight"] = txtTotalWeight.Text == "" ? "0" : txtTotalWeight.Text;
                    dr["CheckPcs"] = txtCheckPcs.Text == "" ? "0" : txtCheckPcs.Text;
                    dr["CheckWeight"] = txtCheckWeight.Text == "" ? "0" : txtCheckWeight.Text;
                    dr["ChallanNo"] = lblChallanNo.Text;
                    dr["Process_Rec_Id"] = lblProcessRecId.Text;
                    dr["EmpId"] = lblEmpId.Text;
                    dr["ItemId"] = lblItemId.Text;

                    dtrecordsDryWeight.Rows.Add(dr);
                    ViewState["DryWeightEdit"] = dtrecordsDryWeight;
                }
            }

                if (dtrecordsDryWeight.Rows.Count > 0)
                {
                    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlTransaction Tran = con.BeginTransaction();
                    try
                    {
                        SqlParameter[] _param = new SqlParameter[5];

                        _param[0] = new SqlParameter("@MastercompanyId", SqlDbType.Int);
                        _param[1] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);
                        _param[2] = new SqlParameter("@CheckBoxDry", SqlDbType.Int);
                        _param[3] = new SqlParameter("@ProcessId", SqlDbType.Int);

                        if (ChkboxDryWeight.Checked == true)
                        {
                            checkboxdryweight = 1;
                            //_param[2].Value = lblTotalWeight.Text;
                            //_param[3].Value = lblCheckPcs.Text;
                            //_param[4].Value = lblCheckWeight.Text;
                        }
                        else
                        {
                            checkboxdryweight = 0;
                            //_param[2].Value = txtTotalWeight.Text;
                            //_param[3].Value = txtCheckPcs.Text;
                            //_param[4].Value = txtCheckWeight.Text;
                        }

                        _param[0].Value = Session["varcompanyId"];
                        _param[1].Direction = ParameterDirection.Output;
                        _param[2].Value = checkboxdryweight;
                        _param[3].Value = DDProcessName.SelectedValue;
                        _param[4] = new SqlParameter("@dtrecords", dtrecordsDryWeight);

                        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_UpdateTotalWeightInProcessReceiveMaster]", _param);

                        llMessageBox.Visible = true;
                        llMessageBox.Text = _param[1].Value.ToString();
                        Tran.Commit();
                        fillCarpetReceive();

                        DDDryType.Items.Clear();
                        DDDryType.Enabled = false;
                        GVDryWeight.DataSource = "";
                        GVDryWeight.DataBind();
                        ChkboxDryWeight.Checked = false;

                    }
                    catch (Exception ex)
                    {
                        UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceiveNew.aspx");
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
    protected void btnShow_Click(object sender, EventArgs e)
    {
        fillDryWeight();        
        ModalPopupExtender2.Show();        
    }
    protected void ChkboxDryWeight_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkboxDryWeight.Checked == true)
        {
            DDDryType.Enabled = true;
            DDDryType.Items.Clear();
            

            DDDryType.Items.Insert(0,"Already Filled");
            DDDryType.Items.Insert(1,"Not Filled");
            DDDryType.SelectedIndex = 1;
            ModalPopupExtender2.Show();
        }
        else
        {
            DDDryType.Items.Clear();
            DDDryType.Enabled = false;
            GVDryWeight.DataSource = "";
            GVDryWeight.DataBind();
        }
    }
    protected void txtDriedWeight_TextChanged(object sender, EventArgs e)
    {
        Double VarBal, Deduction, TotalDedc;
        string qry="";
        int relaxation=0;
        if (ChkboxDryWeight.Checked == true && GVDryWeight.Rows.Count > 0)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
            txt.Focus();            

            TextBox txtDriedWeight = (TextBox)gvRow.FindControl("txtDriedWeight");

            Label lblTotalWeight = (Label)gvRow.FindControl("lblTotalWeight");
            Label lblCheckPcs = (Label)gvRow.FindControl("lblCheckPcs");
            Label lblCheckWeight = (Label)gvRow.FindControl("lblCheckWeight");
             Label lblItemId = (Label)gvRow.FindControl("lblItemId");
             Label lblDeductionInPer = (Label)gvRow.FindControl("lblDeductionInPer");

             if (Convert.ToDecimal(txtDriedWeight.Text == "" ? "0" : txtDriedWeight.Text) > 0)
             {
                 qry = @"Select  Top 1 isnull(RelaxationInPer,0) as RelaxationInPer From Quality1 Q1 INNER JOIN Quality Q ON Q1.QualityId=Q.QualityId where Q.Item_Id=" + lblItemId.Text;

                 DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
                 if (ds.Tables[0].Rows.Count > 0)
                 {
                     relaxation = Convert.ToInt32(ds.Tables[0].Rows[0]["RelaxationInPer"].ToString());
                 }


                 VarBal = Convert.ToDouble(lblCheckWeight.Text) - Convert.ToDouble(txtDriedWeight.Text);

                 Deduction = (VarBal / Convert.ToDouble(lblCheckWeight.Text)) * 100;
                 TotalDedc = Deduction - relaxation;

                 if (TotalDedc < 0)
                 {
                     lblDeductionInPer.Text = "0";
                 }
                 else
                 {
                     lblDeductionInPer.Text = Convert.ToString(string.Format("{0:#0.000}", Deduction - relaxation));
                 }

             }
             else
             {
                 lblDeductionInPer.Text = "0";
             }
        }
        ModalPopupExtender2.Show();
    }
    protected void CancelChallanSubmit()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _param = new SqlParameter[9];

            _param[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
            _param[1] = new SqlParameter("@ChallanNo", SqlDbType.Int);
            _param[2] = new SqlParameter("@MastercompanyId", SqlDbType.Int);
            _param[3] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);
            _param[4] = new SqlParameter("@ProcessId", SqlDbType.Int);
            _param[5] = new SqlParameter("@UserId", SqlDbType.Int);

            _param[0].Value = ViewState["Process_Rec_Id"];
            _param[1].Value = DDChallanNo.SelectedValue;
            _param[2].Value = Session["varcompanyId"];
            _param[3].Direction = ParameterDirection.Output;
            _param[4].Value = DDProcessName.SelectedValue;
            _param[5].Value = Session["varuserid"];

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_DeleteCancelChallan]", _param);

            llMessageBox.Visible = true;
            llMessageBox.Text = _param[3].Value.ToString();
            Tran.Commit();

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceiveNew.aspx");
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
    protected void BtnCancelChallan_Click(object sender, EventArgs e)
    {
        btnclickflag = "";
        btnclickflag = "BtnCancelChallan";
        txtpwd.Focus();
        Popup(true);
        //CancelChallanSubmit();


        //if (FinalBazaar == 1)
        //{
        //    btnclickflag = "BtnCancelChallan";
        //    txtpwd.Focus();
        //    Popup(true);
        //}
        //else
        //{           
        //    CancelChallanSubmit();
        //} 
        
    }
    protected void txtTharri_TextChanged(object sender, EventArgs e)
    {
        FillTharThari();
    }
    protected void txtLachhi_TextChanged(object sender, EventArgs e)
    {
        FillTharThari();
    }
    protected void txtTar_TextChanged(object sender, EventArgs e)
    {
        FillTharThari();
    }
    protected void txtMisc_TextChanged(object sender, EventArgs e)
    {
        FillTharThari();
    }
    protected void txtExtra_TextChanged(object sender, EventArgs e)
    {
        FillTharThari();
    }
    protected void txtLagat_TextChanged(object sender, EventArgs e)
    {
        FillTharThari();
    }
    protected void txtLoss_TextChanged(object sender, EventArgs e)
    {
        FillTharThari();
    }
    protected void BtnWeaver_Click(object sender, EventArgs e)
    {
        if (DDChallanNo.SelectedIndex > 0)
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
        SqlParameter[] array = new SqlParameter[7];
        array[0] = new SqlParameter("@ChallanNo", SqlDbType.Int);
        array[1] = new SqlParameter("@EmpId", SqlDbType.Int);
        array[2] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        array[3] = new SqlParameter("@CompanyId", SqlDbType.Int);
        array[4] = new SqlParameter("@SampleFlag", SqlDbType.Int);
        array[5] = new SqlParameter("@ManualWtEntry", SqlDbType.Int); 
       
        array[0].Value = Convert.ToInt32(DDChallanNo.SelectedValue == "" ? "0" : DDChallanNo.SelectedValue);
        array[1].Value = DDEmployeeNamee.SelectedValue;
        array[2].Value = Session["varcompanyId"];
        array[3].Value = DDCompanyName.SelectedValue;
        array[4].Value = chkboxSampleFlag.Checked == true ? "1" : "0";
        array[5].Value = chkboxManualWeight.Checked == true ? "1" : "0";               

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

        //int chkboxManualWtflag = 0;
        //if (chkboxManualWeight.Checked == true)
        //{
        //    chkboxManualWtflag = 1;
        //}
        //else
        //{
        //    chkboxManualWtflag = 0;
        //}
        //array[5].Value = chkboxManualWtflag;
      

        //array[1].Value = 4;
        //array[2].Value = 20;
        //array[3].Value = 1;  

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
        if (DDChallanNo.SelectedIndex > 0)
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
        SqlParameter[] array = new SqlParameter[4];
        array[0] = new SqlParameter("@ChallanNo", SqlDbType.Int);
        array[1] = new SqlParameter("@EmpId", SqlDbType.Int);
        array[2] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        array[3] = new SqlParameter("@CompanyId", SqlDbType.Int);
        array[0].Value = Convert.ToInt32(DDChallanNo.SelectedValue == "" ? "0" : DDChallanNo.SelectedValue);
        array[1].Value = DDEmployeeNamee.SelectedValue;
        array[2].Value = Session["varcompanyId"];
        array[3].Value = DDCompanyName.SelectedValue;

        //array[1].Value = 4;
        //array[2].Value = 20;
        //array[3].Value = 1; 

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
    //protected void chkFinalBazaar_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chkFinalBazaar.Checked == true)
    //    {
    //        //BtnFinalBazaar.Visible = true;
    //    }
    //    else
    //    {
    //        //BtnFinalBazaar.Visible = false;
    //    }
    //}
    protected void FinalBazaarSubmit()
    {
        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "ConfirmSave();", true);        
        
        if (chkFinalBazaar.Checked == true)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _param = new SqlParameter[11];

                _param[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
                _param[1] = new SqlParameter("@ChallanNo", SqlDbType.Int);
                _param[2] = new SqlParameter("@ProcessId", SqlDbType.Int);
                _param[3] = new SqlParameter("@MastercompanyId", SqlDbType.Int);
                _param[4] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);               
                _param[5] = new SqlParameter("@UserId", SqlDbType.Int);
                _param[6] = new SqlParameter("@ReceiveDate", SqlDbType.DateTime);
                _param[7] = new SqlParameter("@Remarks", SqlDbType.VarChar, 500);
                _param[8] = new SqlParameter("@FlagFixOrWeight", SqlDbType.Int);
                _param[9] = new SqlParameter("@TotalLagat", SqlDbType.Float);
                _param[10] = new SqlParameter("@Sampleflag", SqlDbType.Int);               


                _param[0].Value = ViewState["Process_Rec_Id"];
                _param[1].Value = DDChallanNo.SelectedValue;
                _param[2].Value = DDProcessName.SelectedValue;
                _param[3].Value = Session["varcompanyId"];
                _param[4].Direction = ParameterDirection.Output;
                _param[4].Value = "";
                _param[5].Value = Session["varuserid"];
                _param[6].Value = TxtRecDate.Text;
                _param[7].Value = TxtRemarks.Text;
                _param[8].Value = Convert.ToInt32(RBLConsumption.SelectedValue);
                _param[9].Value = Convert.ToDouble(lblTotalLagat.Text);
                int chkboxsampleflag = 0;
                if (chkboxSampleFlag.Checked == true)
                {
                    chkboxsampleflag = 1;
                }
                else
                {
                    chkboxsampleflag = 0;
                }
                _param[10].Value = chkboxsampleflag;                

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_UpdateFinalBazaar]", _param);

                llMessageBox.Visible = true;
                llMessageBox.Text = _param[4].Value.ToString();
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('"+llMessageBox.Text+"');", true);

                //sb.AppendLine(llMessageBox.Text);
                //llMessageBox.Text=(sb.ToString());

                Tran.Commit();
                fillCarpetReceive();               

            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceiveNew.aspx");
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
    //protected void BtnFinalBazaar_Click(object sender, EventArgs e)
    //{
    //    btnclickflag = "";
    //    if (FinalBazaar == 1)
    //    {
    //        btnclickflag = "BtnFinalBazaar";
    //        txtpwd.Focus();
    //        Popup(true);
    //        //FinalBazaarSubmit();
    //    }
    //    else
    //    {

    //        if (chkboxSampleFlag.Checked == false)
    //        {
    //            UpdateRateComm();
    //        } 
    //        FinalBazaarSubmit();
            
    //    } 
    //}
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        if (MySession.ProductionEditPwd == txtpwd.Text)
        {
            ////if (btnclickflag == "BtnUpdateTDS")
            ////{
            ////    UpdateTDS();
            ////}
            ////else if (btnclickflag == "BtnFinalBazaar")
            ////{
            ////    if (chkboxSampleFlag.Checked == false)
            ////    {
            ////        UpdateRateComm();
            ////    } 
            ////    FinalBazaarSubmit();               
            ////}

            //if (btnclickflag == "BtnDryWeight")
            //{
            //    //DryWeightSubmit();
            //    ModalPopupExtender2.Show();
            //    txtDryWeightDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            //}
            if (btnclickflag == "BtnSave")
            {
                CHECKVALIDCONTROL();
                if (llMessageBox.Text == "")
                {
                    ProcessIssue();
                    UpdateFullAreaRound();
                    UpdateTotalWeightRemarks();                   
                    UpdateTDS();
                    DryWeightSubmit();
                    if (chkboxSampleFlag.Checked == false)
                    {
                        UpdateRateComm();
                    }                    
                    FinalBazaarSubmit();
                    UpdateGSTRATE();
                }
            }
            else if (btnclickflag == "BtnCancelChallan")
            {
                CancelChallanSubmit();
            }            
            Popup(false);
        }
        else
        {
            llMessageBox.Visible = true;
            llMessageBox.Text = "Please Enter Correct Password..";
        }
    }
    void Popup(bool isDisplay)
    {
        StringBuilder builder = new StringBuilder();
        if (isDisplay)
        {
            builder.Append("<script>");
            builder.Append("ShowPopup();</script>");
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", builder.ToString());
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ShowPopup", builder.ToString(), false);
        }
        else
        {
            builder.Append("<script>");
            builder.Append("HidePopup();</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HidePopup", builder.ToString(), false);
        }
    }
    protected void UpdateFullAreaRound()
    {
        string HKStrWL = "";
        double BZW = 0, BZL = 0;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
     
        //****************sql Table Type 
        DataTable dtrecordsFullArea = new DataTable();
        dtrecordsFullArea.Columns.Add("Process_Rec_Id", typeof(int));
        dtrecordsFullArea.Columns.Add("process_rec_detail_id", typeof(int));
        dtrecordsFullArea.Columns.Add("TDSType", typeof(int));
        dtrecordsFullArea.Columns.Add("FinisherJobId", typeof(int));
        dtrecordsFullArea.Columns.Add("FinisherNameId", typeof(int));
        dtrecordsFullArea.Columns.Add("FinishedId", typeof(int));
        dtrecordsFullArea.Columns.Add("IssueOrderId", typeof(int));
        dtrecordsFullArea.Columns.Add("Area", typeof(float));
        dtrecordsFullArea.Columns.Add("Amount", typeof(float));
        dtrecordsFullArea.Columns.Add("CommAmt", typeof(float));

        for (int i = 0; i < GVCarpetReceiveEdit.Rows.Count; i++)
        {
            Label lblQty = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblQty"));
            Label lblShapeId = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblShapeId"));
            //TextBox txtqty = ((TextBox)DGIndent.Rows[i].FindControl("txtQty"));
            //if ((chkoutItem.Checked == true) && (Convert.ToDouble(txtqty.Text == "" ? "0" : txtqty.Text) > 0))
            if (Convert.ToInt32(lblQty.Text) > 0 && lblShapeId.Text=="2")
            {
                DataRow dr = dtrecordsFullArea.NewRow();
                //***********
                Label lblProcessRecId = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblProcessRecId"));
                Label lblProcessRecDetailId = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblProcessRecDetailId"));
                Label lblType = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblTypeId"));
                Label lblJobNameDel = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblJobNameDel"));
                Label lblFinisherNameDel = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblFinisherNameDel"));
                Label lblFinishedId = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblFinishedId"));
                Label lblIssueOrderId = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblIssueOrderId"));
                Label lblAfterKhapSize = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblAfterKhapSize"));
                Label lblRate = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblRate"));
                Label lblComm = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblComm"));
               

                dr["Process_Rec_Id"] = lblProcessRecId.Text;
                dr["process_rec_detail_id"] = Convert.ToInt32(lblProcessRecDetailId.Text);
                dr["TDSType"] =Convert.ToInt32 (lblType.Text);
                dr["FinisherJobId"] = Convert.ToInt32(lblJobNameDel.Text);
                dr["FinisherNameId"] = Convert.ToInt32(lblFinisherNameDel.Text);
                dr["FinishedId"] = lblFinishedId.Text;
                dr["IssueOrderId"] = lblIssueOrderId.Text;

                if (DDcaltype.SelectedIndex >= 0)
                {
                    HKStrWL = string.Format("{0:#0.00}", lblAfterKhapSize.Text);
                    BZW = Convert.ToDouble(HKStrWL.Split('x')[0]);
                    BZL = Convert.ToDouble(HKStrWL.Split('x')[1]);
                    if (Convert.ToInt32(DDunit.SelectedValue) == 1)
                    {
                        dr["Area"] = Convert.ToString(UtilityModule.Calculate_Area_Mtr(BZL, BZW, Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(lblShapeId.Text)));
                    }
                    if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
                    {
                        dr["Area"] = Convert.ToString(UtilityModule.Calculate_Area_Ft(BZL, BZW, Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(lblShapeId.Text), UnitId: Convert.ToInt16(DDunit.SelectedValue), RoundFullAreaValue: Convert.ToDouble(chkboxRoundFullArea.Checked == true ? "1" : "0.7853")));
                    }
                }

                if (DDcaltype.SelectedValue == "0" || DDcaltype.SelectedValue == "2" || DDcaltype.SelectedValue == "3" || DDcaltype.SelectedValue == "4")
                {
                    dr["Amount"] = Convert.ToDecimal(dr["Area"]) * Convert.ToInt32(lblQty.Text) * Convert.ToDecimal(lblRate.Text);
                    dr["CommAmt"] = Convert.ToDecimal(dr["Area"]) * Convert.ToInt32(lblQty.Text) * Convert.ToDecimal(lblComm.Text);
                }
                if (DDcaltype.SelectedValue == "1")
                {
                    dr["Amount"] = Convert.ToInt32(lblQty.Text) * Convert.ToDecimal(lblRate.Text);
                    dr["CommAmt"] = Convert.ToInt32(lblQty.Text) * Convert.ToDecimal(lblComm.Text);
                }

                //************** 
                dtrecordsFullArea.Rows.Add(dr);
            }
        }
        //********************
        if (dtrecordsFullArea.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[8];

                param[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
                param[1] = new SqlParameter("@MastercompanyId", SqlDbType.Int);
                param[2] = new SqlParameter("@ProcessId", SqlDbType.Int);
                param[3] = new SqlParameter("@ReceiveDate", SqlDbType.DateTime);
                param[4] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);

                if (CBTDS.Checked == true)
                {
                    checkboxtds = 1;
                }
                else
                {
                    checkboxtds = 0;
                }

                param[0].Value = ViewState["Process_Rec_Id"];
                param[1].Value = Session["varcompanyId"];
                param[2].Value = DDProcessName.SelectedValue;
                param[3].Value = TxtRecDate.Text;
                param[4].Direction = ParameterDirection.Output;
                param[5] = new SqlParameter("@dtrecordsRoundArea", dtrecordsFullArea);               
                param[6] = new SqlParameter("@CheckBoxTds", checkboxtds);               

                //**********
                //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveIndentNew2", param);
                int rowscount;
                rowscount = SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateFirstProcessRoundAreaFull", param);
                Tran.Commit();
                
                llMessageBox.Text = param[4].Value.ToString();
                llMessageBox.Visible = true;
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + llMessageBox.Text + "');", true);
               // fillCarpetReceive();

            }
            catch (Exception ex)
            {
                llMessageBox.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        else
        {
          //  ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('No Round Size records found.');", true);
        }
    }
    protected void UpdateRateComm()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //****************sql Table Type 
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("Process_Rec_Id", typeof(int));
        dtrecords.Columns.Add("Process_Rec_Detail_Id", typeof(int));
        dtrecords.Columns.Add("Item_Finished_Id", typeof(int));
        dtrecords.Columns.Add("FinisherJobId", typeof(int));
        dtrecords.Columns.Add("FinisherNameId", typeof(int));
        dtrecords.Columns.Add("Area", typeof(float));
        dtrecords.Columns.Add("Qty", typeof(int));
        dtrecords.Columns.Add("PenAmt", typeof(float));
        dtrecords.Columns.Add("Comm", typeof(float));
        dtrecords.Columns.Add("TypeId", typeof(int));
        dtrecords.Columns.Add("UnitId", typeof(int));
        dtrecords.Columns.Add("Process_ID", typeof(int));
        dtrecords.Columns.Add("effectivedate", typeof(DateTime));
        dtrecords.Columns.Add("mastercompanyid", typeof(int));
        dtrecords.Columns.Add("OrderId", typeof(int));
        dtrecords.Columns.Add("FinishingSize", typeof(string));
        dtrecords.Columns.Add("Width", typeof(string));
        dtrecords.Columns.Add("Length", typeof(string));

        for (int i = 0; i < GVCarpetReceiveEdit.Rows.Count; i++)
        {
            //CheckBox chkoutItem = ((CheckBox)DGOrderdetail.Rows[i].FindControl("Chkboxitem"));
            ////TextBox txtqty = ((TextBox)DGIndent.Rows[i].FindControl("txtQty"));
            ////if ((chkoutItem.Checked == true) && (Convert.ToDouble(txtqty.Text == "" ? "0" : txtqty.Text) > 0))
            //if (chkoutItem.Checked == true)
            //{                
            //}

            DataRow dr = dtrecords.NewRow();
            //***********
            Label lblFinishedId = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblFinishedId"));
            Label hnFinisherJobID = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("hnFinisherJobID"));
            Label hnFinisherNameID = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("hnFinisherNameID"));
            Label lblProcessRecDetailId = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblProcessRecDetailId"));
            Label lblProcessRecId = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblProcessRecId"));
            Label lblArea = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblArea"));
            Label lblQty = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblQty"));
            Label lblPenAmt = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblPenAmt"));
            Label lblTypeId = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblTypeId"));
            Label lblComm = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblComm"));
            Label lblOrderId = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblOrderId"));
            Label lblfinishingSize = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblfinishingSize"));
            Label lblKhapWidth = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblKhapWidth"));
            Label lblKhapLength = ((Label)GVCarpetReceiveEdit.Rows[i].FindControl("lblKhapLength"));

            //**************           

            dr["Process_Rec_Id"] = lblProcessRecId.Text;
            dr["Process_Rec_Detail_Id"] = lblProcessRecDetailId.Text;
            dr["Item_Finished_Id"] = lblFinishedId.Text;
            dr["FinisherJobId"] = hnFinisherJobID.Text;
            dr["FinisherNameId"] = hnFinisherNameID.Text;
            dr["Area"] = lblArea.Text;
            dr["Qty"] = Convert.ToInt32(lblQty.Text);
            dr["PenAmt"] = lblPenAmt.Text;
            dr["Comm"] = lblComm.Text;
            dr["TypeId"] = lblTypeId.Text;
            dr["UnitId"] = Convert.ToInt32(DDunit.SelectedValue);
            dr["Process_ID"] = Convert.ToInt32(DDProcessName.SelectedValue);
            dr["effectivedate"] = TxtRecDate.Text == "" ? System.DateTime.Now.ToString("dd-MMM-yyyy") : TxtRecDate.Text;
            dr["mastercompanyid"] = HttpContext.Current.Session["varcompanyid"];
            dr["OrderId"] = lblOrderId.Text;
            dr["FinishingSize"] = lblfinishingSize.Text;
            dr["Width"] = lblKhapWidth.Text;
            dr["Length"] = lblKhapLength.Text;

            dtrecords.Rows.Add(dr);
        }
        //********************
        if (dtrecords.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 100);
                param[0].Direction = ParameterDirection.Output;
                param[1] = new SqlParameter("@dtrecords", dtrecords);
                param[2] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
                
                if (CBTDS.Checked == true)
                {
                    checkboxtds = 1;
                }
                else
                {
                    checkboxtds = 0;
                }
                param[3] = new SqlParameter("@CheckBoxTds", checkboxtds);
                param[4] = new SqlParameter("@UserId", Session["varuserid"]);             

                //**********               
                int rowscount;
                rowscount = SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_GET_UPDATE_CONSUMPTION_RATE_COMMISSION_BAZAARTABLE", param);
                Tran.Commit();
                //if (rowscount == -1)
                //{
                //    lblMessage.Visible = true;
                //    lblMessage.Text = "";
                //}
                llMessageBox.Text = param[0].Value.ToString();               
                llMessageBox.Visible = true;                

            }
            catch (Exception ex)
            {
                llMessageBox.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        else
        {
           // ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
        }
    }
    protected void UpdateTotalWeightRemarks()
    {      
        if (txtTotalWeight.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _param = new SqlParameter[11];

                _param[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
                _param[1] = new SqlParameter("@ChallanNo", SqlDbType.Int);
                _param[2] = new SqlParameter("@TotalWeight", SqlDbType.Float);
                _param[3] = new SqlParameter("@CheckPcs", SqlDbType.Float);
                _param[4] = new SqlParameter("@CheckWeight", SqlDbType.Float);
                _param[5] = new SqlParameter("@MastercompanyId", SqlDbType.Int);
                _param[6] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);
                _param[7] = new SqlParameter("@Remarks", SqlDbType.VarChar, 500);
                _param[8] = new SqlParameter("@ProcessId", SqlDbType.Int);
                _param[9] = new SqlParameter("@ManualWtEntry", SqlDbType.Int);
                _param[10] = new SqlParameter("@ReceiveDate", SqlDbType.DateTime);

                _param[0].Value = ViewState["Process_Rec_Id"];
                _param[1].Value = DDChallanNo.SelectedValue;
                _param[2].Value = txtTotalWeight.Text=="" ? "0" : txtTotalWeight.Text;
                _param[3].Value = txtCheckPcs.Text == "" ? "0" : txtCheckPcs.Text;
                _param[4].Value = txtCheckWeight.Text=="" ? "0" : txtCheckWeight.Text;
                _param[5].Value = Session["varcompanyId"];
                _param[6].Direction = ParameterDirection.Output;
                _param[7].Value = TxtRemarks.Text;
                _param[8].Value = DDProcessName.SelectedValue;

                int chkboxManualWtEntryflag = 0;
                if (chkboxManualWeight.Checked == true)
                {
                    chkboxManualWtEntryflag = 1;
                }
                else
                {
                    chkboxManualWtEntryflag = 0;
                }
                _param[9] = new SqlParameter("@ManualWtEntry", chkboxManualWtEntryflag);
                _param[10].Value = TxtRecDate.Text;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_UpdateTotalWtRemarksInProcessReceiveMaster]", _param);

                llMessageBox.Visible = true;
                if (llMessageBox.Text == _param[6].Value.ToString())
                {
                    llMessageBox.Text = _param[6].Value.ToString();
                }
                else
                {
                    llMessageBox.Text = llMessageBox.Text;
                }
               
                //ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + llMessageBox.Text + "');", true);

                sb.AppendLine(llMessageBox.Text);
               
                Tran.Commit();
                //fillCarpetReceive();
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceiveNew.aspx");
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

    protected void UpdateGSTRATE()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _param = new SqlParameter[6];

            _param[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
            _param[1] = new SqlParameter("@ReceiveDate", SqlDbType.DateTime);
            _param[2] = new SqlParameter("@ProcessId", SqlDbType.Int);
            _param[3] = new SqlParameter("@MastercompanyId", SqlDbType.Int);
            _param[4] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 100);
            _param[5] = new SqlParameter("@Empid", SqlDbType.Int);


            _param[0].Value = ViewState["Process_Rec_Id"];
            _param[1].Value = TxtRecDate.Text;
            _param[2].Value = DDProcessName.SelectedValue;
            _param[3].Value = Session["varcompanyId"]; 
            _param[4].Direction = ParameterDirection.Output;
            _param[5].Value = DDEmployeeNamee.SelectedValue;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_UpdateGSTRateInProcessReceiveDetail]", _param);

            llMessageBox.Visible = true;
            llMessageBox.Text = _param[4].Value.ToString();

            Tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + llMessageBox.Text + "');", true);
            //fillCarpetReceive();

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceiveNew.aspx");
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