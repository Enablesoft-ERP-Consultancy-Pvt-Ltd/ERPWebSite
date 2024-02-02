using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_process_ProcessReceive : System.Web.UI.Page
{
    static int MasterCompanyId;
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
                process = "Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" and process_name_id in(1,16,35) Order By PROCESS_NAME";
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

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

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
                TxtPRemarks.Visible = false;
            }
            else
            {
                TxtPRemarks.Visible = true;
            }
            TxtRecDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtPostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix(TxtPrefix.Text));
            ViewState["Process_Rec_Id"] = 0;
            TxtRecDate.Enabled = true;
            ViewState["VarGatePassNo"] = 0;
            ParameteLabel();
            switch (VarProdCode)
            {
                case 0:
                    TDTextProductCode.Visible = false;
                    break;
                case 1:
                    TDTextProductCode.Visible = true;
                    break;
            }

            if (variable.VarBAZARQAPEROSONNAME == "1")
            {
                TDQaname.Visible = true;
            }
            if (DDProcessName.Items.Count > 0)
            {
                if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 7)
                {
                    DDProcessName.SelectedValue = "9";
                    ViewState["Process_Rec_Id"] = 0;
                    TxtRecDate.Enabled = true;
                    ProcessSelectedChange();
                    BtnPreview.Visible = false;
                    btnqcchkpreview.Visible = false;
                    BtnGatePass.Visible = false;
                }
                else
                {
                    if (DDProcessName.Items.FindByValue("1") != null)
                    {
                        DDProcessName.SelectedValue = "1";
                        DDProcessName_SelectedIndexChanged(sender, new EventArgs());
                    }
                }
                // UtilityModule.ConditionalComboFill(ref DDEmployeeNamee, "Select Distinct EI.EmpId,EI.EmpName from Empinfo EI,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM WHERE PIM.EmpId=EI.EmpId And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by EI.EmpName", true, "--Select--");
            }
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                {
                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Insert into TEMP_PROCESS_ISSUE_MASTER_NEW SELECT Distinct PM.Companyid,OM.Customerid,PD.Orderid," + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " ProcessId,PM.Empid,PM.IssueOrderid,PM.ChallanNO FROM PROCESS_ISSUE_MASTER_" + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " PM,PROCESS_ISSUE_DETAIL_" + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " PD,OrderMaster OM Where PM.IssueOrderid=PD.IssueOrderid And PD.Orderid=OM.Orderid And PQty<>0");
                }
            }
            btnqcchkpreview.Enabled = false;
            visible_comp();
            //
            switch (Convert.ToInt16(Session["varMasterCompanyIDForERP"]))
            {
                case 9:
                    btnqcchkpreview.Visible = false;
                    BtnGatePass.Visible = false;
                    chkForSlip.Visible = true;
                    chkForSlip.Checked = true;
                    tdSrNo.Visible = true;
                    tdFolioNo.Visible = true;
                    Label4.Text = "Folio No";
                    Label26.Text = "Folio No";
                    TxtChallanNo.Enabled = false;
                    ChkForRejectPcsSlip.Visible = true;
                    break;
                case 16:
                    TDLagat.Visible = true;
                    break;
                case 30:
                    TDactualW.Visible = false;
                    TDactualL.Visible = false;
                    break;
                case 39:
                    TDPartyChallanNo.Visible = true;
                    break;

            }

            if (variable.VarCompanyWiseChallanNoGenerated == "1")
            {
                TxtChallanNo.Enabled = false;
                TxtChallanNo.Text = "";
            }

        }
    }
    private void visible_comp()
    {
        if (hncomp.Value == "7")
        {
            TDArea.Visible = false;
            TDleng.Visible = false;
            Tdpenality.Visible = false;
            TDrate.Visible = false;
            TDweigth.Visible = false;
            TDwwidth.Visible = false;
            TDComm.Visible = false;
            trprifix.Visible = false;
            tdcalname.Visible = false;
        }
    }
    private void ParameteLabel()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
    }
    protected void TxtPrefix_TextChanged(object sender, EventArgs e)
    {
        TxtPostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix((TxtPrefix.Text).ToUpper()));
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Process_Rec_Id"] = 0;
        TxtRecDate.Enabled = true;
        if (TDQaname.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDQaname, "select Ei.EmpId,EI.EmpName From Empinfo Ei inner join Department D on Ei.Departmentid=D.DepartmentId and D.DepartmentName='QC Department' order by Ei.EmpName", true, "--Plz Select--");
        }
        ProcessSelectedChange();
    }
    private void ProcessSelectedChange()
    {
        if (DDProcessName.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref DDEmployeeNamee, "Select Distinct EI.EmpId,EI.EmpName from Empinfo EI,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PIM 
                WHERE PIM.EmpId=EI.EmpId And PIM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By ei.empname ", true, "--Select--");
        }
    }
    protected void DDEmployeeNamee_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmployeeSelectedChange();
    }
    private void EmployeeSelectedChange()
    {
        ViewState["Process_Rec_Id"] = 0;
        TxtRecDate.Enabled = true;
        Fill_Grid();
        //*******************
        string str = "";
        if (DDProcessName.SelectedIndex > 0 && DDEmployeeNamee.SelectedIndex > 0)
        {
            if (variable.VarLoomNoGenerated == "1")
            {
                str = @"Select Distinct PM.IssueOrderId,case When " + Session["varMasterCompanyIDForERP"] + @"=9 Then Om.localOrder+'/'+cast(PM.IssueOrderId as varchar(100)) 
                    ELse cast(PM.ChallanNo as varchar(100)) End as IssueOrderid1 
                    from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM 
                    Inner join PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD on PM.issueorderid=Pd.issueorderid 
                    inner join ordermaster OM on PD.orderid=Om.orderid 
                    left join LoomstockNo ls on Pm.issueorderid=Ls.issueorderid AND ls.ProcessID = " + DDProcessName.SelectedValue + @" 
                    Where Ls.issueorderid is null and  PD.PQty<>0 And PM.status='Pending' And PM.Empid=" + DDEmployeeNamee.SelectedValue + " And Pm.CompanyId=" + DDCompanyName.SelectedValue + " order by PM.Issueorderid";
            }
            else
            {
                str = @"Select Distinct PM.IssueOrderId,case When " + Session["varMasterCompanyIDForERP"] + @"=9 Then Om.localOrder+'/'+cast(PM.IssueOrderId as varchar(100)) 
                   ELse cast(PM.ChallanNo as varchar(100)) End as IssueOrderid1 from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM Inner join
                   PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD on PM.issueorderid=Pd.issueorderid inner join ordermaster OM on PD.orderid=Om.orderid
                   Where PD.PQty<>0 And PM.status='Pending' And PM.Empid=" + DDEmployeeNamee.SelectedValue + " And Pm.CompanyId=" + DDCompanyName.SelectedValue + " order by PM.Issueorderid";
            }
            UtilityModule.ConditionalComboFill(ref DDPONo, str, true, "--Select--");
        }
        //*******************
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
                    //DDunit.SelectedValue = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Unitid From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " Where IssueOrderId=" + DDPONo.SelectedValue).ToString();
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
            UtilityModule.ConditionalComboFill(ref DDItemName, "Select Distinct Item_id, Item_Name from V_FinishedItemDetailNew VF,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD Where VF.Item_Finished_Id=PD.Item_Finished_Id And VF.Category_Id=" + DDCategoryName.SelectedValue + " And PD.IssueOrderId=" + DDPONo.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDItemName, "Select Distinct Item_id, Item_Name from V_FinishedItemDetail VF,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD Where VF.Item_Finished_Id=PD.Item_Finished_Id And VF.Category_Id=" + DDCategoryName.SelectedValue + " And PD.IssueOrderId=" + DDPONo.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
        }
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDitemchange();
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
                    case 16:
                        TxtPrefix.Text = TxtPrefix.Text.Replace(" ", "");
                        break;
                    case 30:
                        TxtPrefix.Text = (TxtPrefix.Text + lastTwoChars + "-").Replace(" ", "");
                        break;
                    case 31:
                        TxtPrefix.Text = variable.VarSTOCKNOPREFIX;
                        break;
                    default:
                        TxtPrefix.Text = (TxtPrefix.Text + "-" + lastTwoChars).Replace(" ", "");
                        break;

                }

                int VARQCTYPE = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VARQCTYPE From MasterSetting"));
                if (VARQCTYPE == 1)
                {
                    qulitychk.Visible = true;
                    fillgrdquality();
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
    private void fillgrdquality()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select QCMaster.ID,SrNo,ParaName from QCParameter inner join QCMaster on QCParameter.ParaID=QCMaster.ParaID where CategoryID=" + DDCategoryName.SelectedValue + " and ItemID=" + DDItemName.SelectedValue + " and ProcessID=" + DDProcessName.SelectedValue + " order by SrNo");
        grdqualitychk.DataSource = ds;
        grdqualitychk.DataBind();
    }
    private void Item_SelectedIndexChange()
    {
        string str = "";

        if (variable.VarNewQualitySize == "1")
        {
            str = "Select Issue_Detail_Id,QDCS+ space(3) + Case When " + DDunit.SelectedValue + "=6 Then ISNULL(Sizeinch,'') Else case When " + DDunit.SelectedValue + "=1 Then ISNULL(ProdSizeMtr,'') Else ISNULL(ProdSizeFt,'') ENd End  from ViewFindFinishedidItemidQDCSSNew,PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + " PIM Where PIM.PQty>0 And PIM.Item_Finished_Id=FinishedId  And ITEM_ID=" + DDItemName.SelectedValue + " and IssueOrderId=" + DDPONo.SelectedValue;
        }
        else
        {
            str = "Select Issue_Detail_Id,QDCS+ space(3) + Case When " + DDunit.SelectedValue + "=6 Then ISNULL(Sizeinch,'') Else case When " + DDunit.SelectedValue + "=1 Then ISNULL(ProdSizeMtr,'') Else ISNULL(ProdSizeFt,'') ENd End  from ViewFindFinishedidItemidQDCSS,PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + " PIM Where PIM.PQty>0 And PIM.Item_Finished_Id=FinishedId  And ITEM_ID=" + DDItemName.SelectedValue + " and IssueOrderId=" + DDPONo.SelectedValue;
        }

        UtilityModule.ConditionalComboFill(ref DDDescription, str, true, "-----------Select------");
    }
    protected void DDDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        Get_Detail();
    }
    private void Get_Detail()
    {
        // DataSet DS = null;

        try
        {
            SqlParameter[] param = new SqlParameter[13];
            param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
            param[1] = new SqlParameter("@Issue_Detail_id", DDDescription.SelectedValue);
            param[2] = new SqlParameter("@Pqty", SqlDbType.Int);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@Length", SqlDbType.VarChar, 50);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@Width", SqlDbType.VarChar, 50);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@Rate", SqlDbType.Float);
            param[5].Direction = ParameterDirection.Output;
            param[6] = new SqlParameter("@Issueqty", SqlDbType.Int);
            param[6].Direction = ParameterDirection.Output;
            param[7] = new SqlParameter("@Area", SqlDbType.Float);
            param[7].Direction = ParameterDirection.Output;
            param[8] = new SqlParameter("@Commission", SqlDbType.Float);
            param[8].Direction = ParameterDirection.Output;
            param[9] = new SqlParameter("@Varnewqualitysize", variable.VarNewQualitySize);
            param[10] = new SqlParameter("@Shapeid", SqlDbType.Int);
            param[10].Direction = ParameterDirection.Output;
            param[11] = new SqlParameter("@QualityGrmPerMeterMinus", SqlDbType.Float);
            param[11].Direction = ParameterDirection.Output;
            param[12] = new SqlParameter("@QualityGrmPerMeterPlus", SqlDbType.Float);
            param[12].Direction = ParameterDirection.Output;
            //********************
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetProductionIssueDetail", param);
            TxtPQty.Text = param[2].Value.ToString();
            Txtlength.Text = param[3].Value.ToString();
            TxtWidth.Text = param[4].Value.ToString();
            TxtRate.Text = param[5].Value.ToString();
            TxtIssuQty.Text = param[6].Value.ToString();
            TxtArea.Text = param[7].Value.ToString();
            TxtCommission.Text = param[8].Value.ToString();
            hnQualityGrmPerMeterMinus.Value = param[11].Value.ToString();
            hnQualityGrmPerMeterPlus.Value = param[12].Value.ToString();
            if (DDcaltype.SelectedIndex > 1)
            {
                if (Convert.ToInt32(DDunit.SelectedValue) == 1)
                {
                    TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(Txtlength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(param[10].Value)));
                }
                if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
                {
                    TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(Txtlength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(param[10].Value), UnitId: Convert.ToInt16(DDunit.SelectedValue)));
                }
            }
            #region
            //            string strsql = "";
            //            if (variable.VarNewQualitySize == "1")
            //            {
            //                 strsql = "Select Qty-Isnull(CancelQty,0) As Qty,IsNull(PQty,0)-IsNull(RejectQty,0)-Isnull(CancelQty,0) PQty,Length,Width,Area,Rate,Comm,ShapeId From PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,
            //                            V_FinishedItemDetailNew VF Where PD.Item_Finished_Id=VF.Item_Finished_Id And Issue_Detail_Id=" + DDDescription.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            //            }
            //            else
            //            {
            //                 strsql = "Select Qty-Isnull(CancelQty,0) As Qty,IsNull(PQty,0)-IsNull(RejectQty,0)-Isnull(CancelQty,0) PQty,Length,Width,Area,Rate,Comm,ShapeId From PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,
            //                            V_FinishedItemDetail VF Where PD.Item_Finished_Id=VF.Item_Finished_Id And Issue_Detail_Id=" + DDDescription.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            //            }
            //            DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            //            if (DS.Tables[0].Rows.Count > 0)
            //            {
            //                TxtPQty.Text = DS.Tables[0].Rows[0]["PQty"].ToString();
            //                Txtlength.Text = DS.Tables[0].Rows[0]["Length"].ToString();
            //                TxtWidth.Text = DS.Tables[0].Rows[0]["Width"].ToString();
            //                TxtRate.Text = DS.Tables[0].Rows[0]["Rate"].ToString();
            //                TxtIssuQty.Text = DS.Tables[0].Rows[0]["Qty"].ToString();
            //                TxtArea.Text = DS.Tables[0].Rows[0]["Area"].ToString();
            //                TxtCommission.Text = DS.Tables[0].Rows[0]["Comm"].ToString();
            //                if (DDcaltype.SelectedIndex > 1)
            //                {
            //                    if (Convert.ToInt32(DDunit.SelectedValue) == 1)
            //                    {
            //                        TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(Txtlength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(DS.Tables[0].Rows[0]["ShapeId"])));
            //                    }
            //                    if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
            //                    {
            //                        TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(Txtlength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(DS.Tables[0].Rows[0]["ShapeId"]), UnitId: Convert.ToInt16(DDunit.SelectedValue)));
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                TxtPQty.Text = "";
            //                Txtlength.Text = "";
            //                TxtWidth.Text = "";
            //                TxtArea.Text = "";
            //                TxtRate.Text = "";
            //                TxtIssuQty.Text = "";
            //                TxtRecQty.Text = "";
            //                TxtCommission.Text = "";
            //            }
            #endregion
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessReceive.aspx");
            llMessageBox.Visible = true;
            llMessageBox.Text = ex.Message;
        }

    }
    protected void Txtlength_TextChanged(object sender, EventArgs e)
    {
        Check_Length_Width_Format();
    }
    protected void TxtWidth_TextChanged(object sender, EventArgs e)
    {
        Check_Length_Width_Format();
    }
    private void Check_Length_Width_Format()
    {
        int FootLength = 0;
        int FootWidth = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        string Str = "";
        if (Txtlength.Text != "")
        {
            if (Convert.ToInt32(DDunit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(Txtlength.Text));
                Txtlength.Text = Str;
                FootLength = Convert.ToInt32(Str.Split('.')[0]);
                FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootLengthInch > 11)
                {
                    llMessageBox.Text = "Inch value must be less than 12";
                    Txtlength.Text = "";
                    Txtlength.Focus();
                }
            }
        }
        if (TxtWidth.Text != "")
        {
            if (Convert.ToInt32(DDunit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(TxtWidth.Text));
                TxtWidth.Text = Str;
                FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootWidthInch > 11)
                {
                    llMessageBox.Text = "Inch value must be less than 12";
                    TxtWidth.Text = "";
                    TxtWidth.Focus();
                }
            }
        }
        if (Txtlength.Text != "" && TxtWidth.Text != "")
        {
            int Shape = 0;
            if (DDDescription.SelectedIndex > 0)
            {
                if (variable.VarNewQualitySize == "1")
                {
                    Shape = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetailNew VF 
                    Where PD.Item_Finished_Id=VF.Item_Finished_Id And PD.Issue_Detail_Id=" + DDDescription.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + ""));
                }
                else
                {
                    Shape = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF 
                    Where PD.Item_Finished_Id=VF.Item_Finished_Id And PD.Issue_Detail_Id=" + DDDescription.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + ""));
                }
            }
            if (Convert.ToInt32(DDunit.SelectedValue) == 1)
            {
                TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(Txtlength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Shape));
            }
            if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
            {
                TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(Txtlength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(DDcaltype.SelectedValue), Shape, UnitId: Convert.ToInt16(DDunit.SelectedValue)));
            }
        }
    }
    private void CheckWeightRange()
    {
        double QualityPerMeterMinus = 0;
        double QualityPerMeterPlus = 0;
        if (Convert.ToInt32(DDunit.SelectedValue) == 1)
        {
            QualityPerMeterMinus = Convert.ToDouble(UtilityModule.DecimalvalueUptoWithoutRounding(Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtRecQty.Text) * Convert.ToDouble(hnQualityGrmPerMeterMinus.Value), 4));
            QualityPerMeterPlus = Convert.ToDouble(UtilityModule.DecimalvalueUptoWithoutRounding(Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtRecQty.Text) * Convert.ToDouble(hnQualityGrmPerMeterPlus.Value), 4));
            if (Convert.ToDecimal(TxtWeight.Text) < Convert.ToDecimal(QualityPerMeterMinus) || Convert.ToDouble(TxtWeight.Text) > Convert.ToDouble(QualityPerMeterPlus))
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "consmp", "alert('Actual weight are go to Minus or Plus are you want to save it?')", true);
            }
        }
        if (Convert.ToInt32(DDunit.SelectedValue) == 2 || Convert.ToInt16(DDunit.SelectedValue) == 6)
        {
            QualityPerMeterMinus = Convert.ToDouble(UtilityModule.DecimalvalueUptoWithoutRounding(Convert.ToDouble(UtilityModule.DecimalvalueUptoWithoutRounding(Math.Round(Convert.ToDouble(TxtArea.Text) / 10.76, 5), 4)) * Convert.ToDouble(TxtRecQty.Text) * Convert.ToDouble(hnQualityGrmPerMeterMinus.Value), 4));
            QualityPerMeterPlus = Convert.ToDouble(UtilityModule.DecimalvalueUptoWithoutRounding(Convert.ToDouble(UtilityModule.DecimalvalueUptoWithoutRounding(Math.Round(Convert.ToDouble(TxtArea.Text) / 10.76, 5), 4)) * Convert.ToDouble(TxtRecQty.Text) * Convert.ToDouble(hnQualityGrmPerMeterPlus.Value), 4));
            if (Convert.ToDouble(TxtWeight.Text) < Convert.ToDouble(QualityPerMeterMinus) || Convert.ToDouble(TxtWeight.Text) > Convert.ToDouble(QualityPerMeterPlus))
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "consmp", "alert('Actual weight are go to Minus or Plus are you want to save it?')", true);
            }
        }

    }
    protected void TxtWeight_TextChanged(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"].ToString() == "9")
        {
            CheckWeightRange();
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {

        CHECKVALIDCONTROL();

        //if (Session["varMasterCompanyIDForERP"].ToString() != "9")
        //{
            //************check Pending qty
            TxtRecQty_TextChanged(sender, new EventArgs());
        //}


        //************

        if (Session["varMasterCompanyIDForERP"].ToString() == "30")
        {
            if (TxtRate.Text == "0" || TxtRate.Text == "" || TxtWeight.Text == "0" || TxtWeight.Text == "")
            {
                llMessageBox.Visible = true;
                llMessageBox.Text = "Please add rate or weight";
            }
        }

        if (llMessageBox.Text == "")
        {
            ProcessIssue();
            Fill_Grid();
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();

            string str = @"Delete TEMP_PROCESS_RECEIVE_MASTER
                             Delete TEMP_PROCESS_RECEIVE_DETAIL";
            str = str + " Insert into TEMP_PROCESS_RECEIVE_MASTER Select Process_Rec_Id,EmpId,ReceiveDate,UnitId,UserId,ChallanNo,Companyid,Remarks,CalType," + DDProcessName.SelectedValue + "  from PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " Where Process_Rec_Id=" + ViewState["Process_Rec_Id"] + "";
            str = str + " Insert into TEMP_PROCESS_RECEIVE_DETAIL Select Process_Rec_Detail_Id, Process_Rec_Id, Item_Finished_Id, Length, Width, Area, Rate, Amount, Qty, Weight, Comm, CommAmt, IssueOrderId, Issue_Detail_Id, OrderId, Penality, PRemarks, QualityType, GatePassNo, FlagFixOrWeight, TDSPercentage, Warp_10cm, Weft_10cm, straightness, Design, OBA, Date_Stamp, StockNoRemarks, WyPly, Cyply from PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + " Where Process_Rec_Id=" + ViewState["Process_Rec_Id"] + "";
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, str);


            for (int i = 0; i < grdqualitychk.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)grdqualitychk.Rows[i].FindControl("CheckBox1");
                TextBox txtqcreason = (TextBox)grdqualitychk.Rows[i].FindControl("txtqcreason");
                chk.Checked = false;
                txtqcreason.Text = "";
            }
            btnqcchkpreview.Enabled = true;
            if (ddStockQualityType.SelectedValue == "3")
            {
                BtnGatePass.Enabled = true;
            }
            else
            {
                BtnPreview.Enabled = true;
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
        if (UtilityModule.VALIDDROPDOWNLIST(DDPONo) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCategoryName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDItemName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDDescription) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDunit) == false)
        {
            goto a;
        }

        if (UtilityModule.VALIDTEXTBOX(TxtRecQty) == false)
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
    //*********************************************Process Issue**************************************************************************
    private void ProcessIssue()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[36];
            _arrpara[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@Empid", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@ReceiveDate", SqlDbType.DateTime);
            _arrpara[3] = new SqlParameter("@Unitid", SqlDbType.Int);
            _arrpara[4] = new SqlParameter("@Userid", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 250);
            _arrpara[6] = new SqlParameter("@Companyid", SqlDbType.Int);
            _arrpara[7] = new SqlParameter("@Remarks", SqlDbType.NVarChar);
            _arrpara[8] = new SqlParameter("@Process_Rec_Detail_Id", SqlDbType.Int);
            _arrpara[9] = new SqlParameter("@Item_Finished_id", SqlDbType.Int);
            _arrpara[10] = new SqlParameter("@Length", SqlDbType.NVarChar);
            _arrpara[11] = new SqlParameter("@Width", SqlDbType.NVarChar);
            _arrpara[12] = new SqlParameter("@Area", SqlDbType.Float);
            _arrpara[13] = new SqlParameter("@Rate", SqlDbType.Float);
            _arrpara[14] = new SqlParameter("@Amount", SqlDbType.Float);
            _arrpara[15] = new SqlParameter("@Qty", SqlDbType.Int);
            _arrpara[16] = new SqlParameter("@Weight", SqlDbType.Float);
            _arrpara[17] = new SqlParameter("@Comm", SqlDbType.Float);
            _arrpara[18] = new SqlParameter("@CommAmt", SqlDbType.Float);
            _arrpara[19] = new SqlParameter("@IssueOrderid", SqlDbType.Int);
            _arrpara[20] = new SqlParameter("@Issue_Detail_Id", SqlDbType.Int);
            _arrpara[21] = new SqlParameter("@Orderid", SqlDbType.Int);
            _arrpara[22] = new SqlParameter("@Penality", SqlDbType.Float);
            _arrpara[23] = new SqlParameter("@QualityType", SqlDbType.Int);
            _arrpara[24] = new SqlParameter("@PRemark", SqlDbType.NVarChar);
            _arrpara[25] = new SqlParameter("@CalType", SqlDbType.Int);
            _arrpara[26] = new SqlParameter("@FlagFixOrWeight", SqlDbType.Int);
            _arrpara[27] = new SqlParameter("@TDSPercentage", SqlDbType.Float);
            _arrpara[28] = new SqlParameter("@ProcessId", SqlDbType.Int);
            _arrpara[29] = new SqlParameter("@GatePassNo", SqlDbType.Int);
            _arrpara[30] = new SqlParameter("@MastercompanyId", SqlDbType.Int);
            _arrpara[31] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            _arrpara[32] = new SqlParameter("@ActualLength", SqlDbType.VarChar, 10);
            _arrpara[33] = new SqlParameter("@ActualWidth", SqlDbType.VarChar, 10);
            _arrpara[34] = new SqlParameter("@QAPersonname", SqlDbType.VarChar, 50);
            _arrpara[35] = new SqlParameter("@PartyChallanNo", SqlDbType.VarChar, 50);

            int num;
            //if (Convert.ToInt32(ViewState["Process_Rec_Id"]) == 0 && Convert.ToInt32(DDProcessName.SelectedIndex) > 0)
            //{
            //    ViewState["Process_Rec_Id"] = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(Process_Rec_Id),0)+1 from MasterSetting"));
            //    num = 1;
            //}
            //else
            //{
            //    num = 0;
            //}
            if (ViewState["Process_Rec_Id"] == null)
            {
                ViewState["Process_Rec_Id"] = 0;
            }
            _arrpara[0].Direction = ParameterDirection.InputOutput;
            _arrpara[0].Value = ViewState["Process_Rec_Id"];
            _arrpara[1].Value = DDEmployeeNamee.SelectedValue;
            _arrpara[2].Value = TxtRecDate.Text;
            _arrpara[3].Value = DDunit.SelectedValue;
            _arrpara[4].Value = Session["varuserid"];
            _arrpara[5].Direction = ParameterDirection.InputOutput;
            _arrpara[5].Value = TxtChallanNo.Text;
            _arrpara[6].Value = DDCompanyName.SelectedValue;
            _arrpara[7].Value = (TxtRemarks.Text == "" ? "" : TxtRemarks.Text);
            // int VarRecDetailId = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Isnull(Max(Process_Rec_Detail_Id),0)+1 from PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue));
            _arrpara[8].Direction = ParameterDirection.Output;
            _arrpara[8].Value = 0;
            int VarFinishedid = 0;
            int VarOrderid = 0;
            int VarFlagFixOrWeight = 0;
            DataSet DsNew = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select Item_Finished_id,OrderId,isnull(FlagFixOrWeight,0) As FlagFixOrWeight from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD Where PM.IssueOrderid=PD.IssueOrderid And PM.IssueOrderid=" + DDPONo.SelectedValue + " And PD.Issue_Detail_Id=" + DDDescription.SelectedValue);
            if (DsNew.Tables[0].Rows.Count > 0)
            {
                VarFinishedid = Convert.ToInt32(DsNew.Tables[0].Rows[0]["Item_Finished_id"]);
                VarOrderid = Convert.ToInt32(DsNew.Tables[0].Rows[0]["OrderId"]);
                VarFlagFixOrWeight = Convert.ToInt32(DsNew.Tables[0].Rows[0]["FlagFixOrWeight"]);
                _arrpara[9].Value = VarFinishedid;
                _arrpara[21].Value = VarOrderid;
                _arrpara[26].Value = VarFlagFixOrWeight;
            }
            if (Session["varMasterCompanyIDForERP"].ToString() == "9")
            {
                _arrpara[10].Value = Convert.ToDouble(Txtlength.Text);
                _arrpara[11].Value = Convert.ToDouble(TxtWidth.Text);
            }
            else
            {
                _arrpara[10].Value = string.Format("{0:#0.00}", Convert.ToDouble(Txtlength.Text));
                _arrpara[11].Value = string.Format("{0:#0.00}", Convert.ToDouble(TxtWidth.Text));
            }

            _arrpara[12].Value = TxtArea.Text;
            _arrpara[13].Value = TxtRate.Text;
            if (ddStockQualityType.SelectedValue == "1")
            {
                if (DDcaltype.SelectedValue == "0" || DDcaltype.SelectedValue == "2" || DDcaltype.SelectedValue == "3" || DDcaltype.SelectedValue == "4")
                {
                    _arrpara[14].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtRate.Text) * Convert.ToDouble(TxtRecQty.Text)));
                    _arrpara[18].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtCommission.Text) * Convert.ToDouble(TxtRecQty.Text)));
                }
                if (DDcaltype.SelectedValue == "1")
                {
                    _arrpara[14].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtRate.Text) * Convert.ToDouble(TxtRecQty.Text)));
                    _arrpara[18].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtCommission.Text) * Convert.ToDouble(TxtRecQty.Text)));
                }
            }
            else
            {
                _arrpara[14].Value = 0.00;
                _arrpara[18].Value = 0.00;
            }
            _arrpara[15].Value = TxtRecQty.Text;
            _arrpara[16].Value = (TxtWeight.Text == "" ? "0" : TxtWeight.Text);
            _arrpara[17].Value = TxtCommission.Text;
            _arrpara[19].Value = DDPONo.SelectedValue;
            _arrpara[20].Value = DDDescription.SelectedValue;
            _arrpara[22].Value = (TxtPEnality.Text == "" ? "0" : TxtPEnality.Text);
            _arrpara[23].Value = ddStockQualityType.SelectedValue;
            _arrpara[24].Value = (TxtPRemarks.Text == "" ? "" : TxtPRemarks.Text);
            _arrpara[25].Value = DDcaltype.SelectedValue;
            _arrpara[27].Value = 0;
            _arrpara[28].Value = DDProcessName.SelectedValue;
            _arrpara[29].Value = 0;
            _arrpara[30].Value = Session["varMasterCompanyIDForERP"];
            _arrpara[31].Direction = ParameterDirection.Output;
            _arrpara[32].Value = txtactualL.Text.Trim();
            _arrpara[33].Value = txtactualW.Text.Trim();
            _arrpara[34].Value = TDQaname.Visible == false ? "" : (DDQaname.SelectedIndex > 0 ? DDQaname.SelectedItem.Text : "");
            _arrpara[35].Value = TDPartyChallanNo.Visible==true ? txtPartyChallanNo.Text : "";
            #region
            //            DataSet DsNew1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"Select IsNull(Sum(TDS),0) TDS From Empinfo EI,TDS_MASTER TM 
            //                    Where EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And  EI.TypeId=TM.TypeID And EI.Empid=" + _arrpara[1].Value + @" And FromDate<='" + _arrpara[2].Value + "' And (EndDate is Null Or EndDate>'" + _arrpara[2].Value + "')");
            //            if (DsNew1.Tables[0].Rows.Count > 0)
            //            {
            //                _arrpara[27].Value = Convert.ToDouble(DsNew1.Tables[0].Rows[0]["TDS"]);
            //            }
            //if (num == 1)
            //{
            //    string str = @"Update MasterSetting Set Process_Rec_Id =" + _arrpara[0].Value;
            //    str = str + @"Insert Into PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + "(Process_Rec_Id,Empid,ReceiveDate,Unitid,Userid,ChallanNo,Companyid,Remarks,CalType) Values (" + _arrpara[0].Value + "," + _arrpara[1].Value + ",'" + _arrpara[2].Value + "'," + _arrpara[3].Value + "," + _arrpara[4].Value + ",'" + _arrpara[5].Value + "'," + _arrpara[6].Value + ",'" + _arrpara[7].Value + "'," + _arrpara[25].Value + ")";
            //    //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            //    // str = @"Insert Into PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + "(Process_Rec_Id,Empid,ReceiveDate,Unitid,Userid,ChallanNo,Companyid,Remarks,CalType) Values (" + _arrpara[0].Value + "," + _arrpara[1].Value + ",'" + _arrpara[2].Value + "'," + _arrpara[3].Value + "," + _arrpara[4].Value + ",'" + _arrpara[5].Value + "'," + _arrpara[6].Value + ",'" + _arrpara[7].Value + "'," + _arrpara[25].Value + ")";
            //    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            //    TxtChallanNo.Text = TxtChallanNo.Text == "" ? ViewState["Process_Rec_Id"].ToString() : TxtChallanNo.Text;
            //}
            //string str1 = @"Insert Into PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + "(Process_Rec_Detail_Id,Process_Rec_Id,Item_Finished_id,Length,Width,Area,Rate,Amount,Qty,Weight,Comm,CommAmt,IssueOrderid,Issue_Detail_Id,Orderid,Penality,PRemarks,QualityType,GatePassNo,FlagFixOrWeight,TDSPercentage) values(" + _arrpara[8].Value + "," + _arrpara[0].Value + "," + _arrpara[9].Value + ",'" + _arrpara[10].Value + "','" + _arrpara[11].Value + "'," + _arrpara[12].Value + "," + _arrpara[13].Value + "," + _arrpara[14].Value + "," + _arrpara[15].Value + "," + _arrpara[16].Value + "," + _arrpara[17].Value + "," + _arrpara[18].Value + "," + _arrpara[19].Value + "," + _arrpara[20].Value + "," + _arrpara[21].Value + "," + _arrpara[22].Value + ",'" + _arrpara[24].Value + "'," + _arrpara[23].Value + ",0," + _arrpara[26].Value + "," + _arrpara[27].Value + ")";
            //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);
            //if (ddStockQualityType.SelectedValue == "3")
            //{
            //    if (Convert.ToInt32(ViewState["VarGatePassNo"]) == 0)
            //    {
            //        ViewState["VarGatePassNo"] = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select IsNull(Max(GatePassNo),0)+1 from PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + "");
            //    }
            //    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Update PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + " Set GatePassNo=" + ViewState["VarGatePassNo"] + " Where Process_Rec_Detail_Id=" + _arrpara[8].Value + "");
            //    str1 = @"Update Process_Issue_Detail_" + DDProcessName.SelectedValue + " Set RejectQty=RejectQty+" + _arrpara[15].Value + " Where  IssueOrderid=" + DDPONo.SelectedValue + " And Issue_Detail_Id=" + DDDescription.SelectedValue;
            //}
            //else
            //{
            //    str1 = @"Update Process_Issue_Detail_" + DDProcessName.SelectedValue + " Set PQty=PQty-" + _arrpara[15].Value + " Where  IssueOrderid=" + DDPONo.SelectedValue + " And Issue_Detail_Id=" + DDDescription.SelectedValue;
            //}
            #endregion

            string tablename = "PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + "";
            ViewState["tablename"] = "PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + "";
            //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_FirstProcessReceive]", _arrpara);
            if (_arrpara[31].Value.ToString() != "")
            {
                llMessageBox.Visible = true;
                llMessageBox.Text = _arrpara[31].Value.ToString();
                Tran.Commit();
            }
            else
            {
                UtilityModule.Insert_Into_Carpet_NumberAndProcess_StockDetailwithProc(VarFinishedid, VarOrderid, Convert.ToInt32(TxtRecQty.Text), TxtPrefix.Text, Convert.ToInt32(TxtPostfix.Text), Convert.ToInt32(DDCompanyName.SelectedValue), Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[8].Value), TxtRecDate.Text, Tran, Convert.ToInt32(DDProcessName.SelectedValue), Convert.ToInt32(_arrpara[20].Value), Convert.ToInt32(_arrpara[4].Value));
                llMessageBox.Visible = true;
                QCSAVE(Tran, Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[8].Value), tablename);
                ViewState["PurchaseReceiveDetailId"] = _arrpara[8].Value;
                llMessageBox.Text = "Data Successfully Saved.......";
                ViewState["Process_Rec_Id"] = _arrpara[0].Value.ToString();
                TxtRecDate.Enabled = false;
                Tran.Commit();
                //TxtChallanNo.Text = TxtChallanNo.Text == "" ? ViewState["Process_Rec_Id"].ToString() : TxtChallanNo.Text;
                TxtChallanNo.Text = _arrpara[5].Value.ToString();
                TxtPostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix((TxtPrefix.Text).ToUpper()));
                // Item_SelectedIndexChange();
                if (DDDescription.Items.Count == 0)
                {
                    DDItemName.SelectedIndex = 0;
                }
                ClearAfterSave();
                if (DDDescription.Items.Count > 0)
                {
                    Get_Detail();
                    TxtRecQty.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessReceive.aspx");
            Tran.Rollback();
            ViewState["Process_Rec_Id"] = 0;
            TxtRecDate.Enabled = true;
            llMessageBox.Visible = true;
            llMessageBox.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void QCSAVE(SqlTransaction Tran, int ReceiveId, int ReceiveDetailId, string tablename)
    {

        //*SQL TABLE TYPE
        DataTable dt = new DataTable();
        dt.Columns.Add("QCID", typeof(int));
        dt.Columns.Add("Qcvalue", typeof(string));
        dt.Columns.Add("Reason", typeof(string));

        //**************
        for (int i = 0; i < grdqualitychk.Rows.Count; i++)
        {
            CheckBox chk = (CheckBox)grdqualitychk.Rows[i].FindControl("CheckBox1");
            TextBox txtqcreason = (TextBox)grdqualitychk.Rows[i].FindControl("txtqcreason");
            DataRow dr = dt.NewRow();
            dr["Qcid"] = grdqualitychk.DataKeys[i].Value.ToString();
            dr["Qcvalue"] = chk.Checked == true ? "1" : "0";
            dr["Reason"] = txtqcreason.Text.Trim();
            dt.Rows.Add(dr);
        }
        #region
        //for (int i = 0; i < grdqualitychk.Rows.Count; i++)
        //{
        //    CheckBox chk = (CheckBox)grdqualitychk.Rows[i].FindControl("CheckBox1");
        //    TextBox txtqcreason = (TextBox)grdqualitychk.Rows[i].FindControl("txtqcreason");
        //    if (chk.Checked)
        //    {
        //        if (checkpara == "")
        //        {
        //            checkpara = grdqualitychk.DataKeys[i].Value.ToString();
        //            Reason = "'" + txtqcreason.Text + "'";
        //        }
        //        else
        //        {
        //            checkpara = checkpara + "," + grdqualitychk.DataKeys[i].Value.ToString();
        //            Reason = Reason + "," + "'" + txtqcreason.Text + "'";
        //        }
        //    }
        //    else
        //        if (noncheck == "")
        //        {
        //            noncheck = grdqualitychk.DataKeys[i].Value.ToString();
        //            NoReason = "'" + txtqcreason.Text + "'";
        //        }
        //        else
        //        {
        //            noncheck = noncheck + "," + grdqualitychk.DataKeys[i].Value.ToString();
        //            NoReason = NoReason + "," + "'" + txtqcreason.Text + "'";
        //        }
        //}

        //SqlParameter[] _arrpara1 = new SqlParameter[7];
        //_arrpara1[0] = new SqlParameter("@ReceiveId", SqlDbType.Int);
        //_arrpara1[1] = new SqlParameter("@ReceiveDetailId", SqlDbType.Int);
        //_arrpara1[2] = new SqlParameter("@checkpara", SqlDbType.NVarChar, 50);
        //_arrpara1[3] = new SqlParameter("@noncheck", SqlDbType.NVarChar, 50);
        //_arrpara1[4] = new SqlParameter("@TableName", SqlDbType.NVarChar, 50);
        //_arrpara1[5] = new SqlParameter("@Reason", SqlDbType.NVarChar, 1000);
        //_arrpara1[6] = new SqlParameter("@Noreason", SqlDbType.NVarChar, 1000);
        //_arrpara1[0].Value = ReceiveId;
        //_arrpara1[1].Value = ReceiveDetailId;
        //_arrpara1[2].Value = checkpara;
        //_arrpara1[3].Value = noncheck;
        //_arrpara1[4].Value = tablename;
        //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PurchaseReceivequalitychk", _arrpara1);
        //**************
        #endregion
        SqlParameter[] _arrpara1 = new SqlParameter[4];
        _arrpara1[0] = new SqlParameter("@ReceiveId", ReceiveId);
        _arrpara1[1] = new SqlParameter("@ReceiveDetailId", ReceiveDetailId);
        _arrpara1[2] = new SqlParameter("@TableName", tablename);
        _arrpara1[3] = new SqlParameter("@dt", dt);
        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_SAVEQCDETAIL]", _arrpara1);
    }
    private void ClearAfterSave()
    {
        TxtIssuQty.Text = "";
        TxtPQty.Text = "";
        Txtlength.Text = "";
        TxtWidth.Text = "";
        txtactualL.Text = "";
        txtactualW.Text = "";
        TxtArea.Text = "";
        TxtRate.Text = "";
        TxtRecQty.Text = "";
        TxtWeight.Text = "";
        TxtPEnality.Text = "";
        TxtPRemarks.Text = "";
        TxtCommission.Text = "";
        DDDescription.Focus();
        fillorderdetail();
    }
    private void Fill_Grid()
    {
        DGRec.DataSource = GetDetail();
        DGRec.DataBind();
    }
    //*********************************************************FIll Gride*********************************************************
    private DataSet GetDetail()
    {
        DataSet DS = null;
        string sqlstr = "";
        string view = "ViewFindFinishedidItemidQDCSS";
        if (variable.VarNewQualitySize == "1")
        {
            view = "ViewFindFinishedidItemidQDCSSNew";
        }

        sqlstr = @"Select Process_Rec_Detail_Id as Sr_No,ICM.Category_Name as Category,IM.Item_Name as Item,IPM.QDCS + Space(5) + 
                        Case When PM.Unitid=1 Then SizeMtr Else case When PM.unitid=6 Then Sizeinch else Sizeft End End Description,Length,Width,Qty,Rate,Qty*Area Area,Amount,Weight,Penality,
                        case When " + Session["varMasterCompanyIDForERP"] + @"=9 Then '' Else [dbo].[F_GetstockNo_RecDetailWise](PD.Process_Rec_Id,PD.Process_Rec_Detail_Id) End TStockNo
                        From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD," + view + @" IPM,
                        Item_Master IM,ITEM_CATEGORY_MASTER ICM Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=IPM.Finishedid And 
                        IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
                        PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And PM.CompanyId=" + DDCompanyName.SelectedValue + " Order By Process_Rec_Detail_Id Desc";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessReceive.aspx");
            llMessageBox.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return DS;
    }
    protected void DGRec_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGRec, "Select$" + e.Row.RowIndex);
        }
        if (Session["varcompanyno"].ToString() == "7")
        {
            for (int i = 0; i < DGRec.Rows.Count; i++)
            {
                DGRec.Columns[3].Visible = false;
                DGRec.Columns[4].Visible = false;
                DGRec.Columns[6].Visible = false;
                DGRec.Columns[7].Visible = false;
                DGRec.Columns[8].Visible = false;
                DGRec.Columns[9].Visible = false;
                DGRec.Columns[10].Visible = false;
                DGRec.Columns[11].Visible = false;
            }
        }
    }
    protected void TxtRecQty_TextChanged(object sender, EventArgs e)
    {
        llMessageBox.Visible = false;
        if (TxtRecQty.Text != "")
        {
            //if (Session["varMasterCompanyIDForERP"].ToString() != "9")
            //{
                //GET PQTY
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
                param[1] = new SqlParameter("@issue_detail_id", DDDescription.SelectedValue);
                param[2] = new SqlParameter("@PqtyExtra", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetProductionIssueDetail", param);

                int Pqty = 0;
                Pqty = Convert.ToInt16(param[2].Value);

                if (Pqty < Convert.ToInt32(TxtRecQty.Text) && Convert.ToInt32(TxtRecQty.Text) > 0)
                {
                    TxtRecQty.Text = "";
                    llMessageBox.Visible = true;
                    llMessageBox.Text = "RecQty must less than or equals to PQty...................";
                    TxtRecQty.Focus();
                }
                else
                {
                    llMessageBox.Text = "";
                    TxtWeight.Focus();
                }
            //}


        }
        else
        {
            llMessageBox.Text = "RecQty must less than or equals to PQty...................";
        }
    }
    protected void TxtProductCode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds;
        string Str;
        llMessageBox.Text = "";
        if (TxtProductCode.Text != "")
        {
            Str = "Select * from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD,ITEM_PARAMETER_MASTER IPM Where PD.Item_Finished_id=IPM.Item_Finished_id And ProductCode='" + TxtProductCode.Text + "' And IPM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count == 0)
            {
                llMessageBox.Text = "ITEM CODE DOES NOT BELONGS TO THAT PO. NO....";
                DDCategoryName.SelectedIndex = 0;
                CATEGORY_DEPENDS_CONTROLS();
                TxtProductCode.Text = "";
                TxtProductCode.Focus();
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDCategoryName, "Select Distinct ICM.Category_Id,Category_Name from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PID,ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,ITEM_CATEGORY_MASTER ICM inner join UserRights_Category UC on(ICM.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"] + ") where IM.CATEGORY_ID=ICM.CATEGORY_ID AND IPM.Item_Id=IM.Item_Id AND PID.Item_Finished_Id=IPM.Item_Finished_Id and PID.IssueOrderId=" + DDPONo.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
                Str = "Select Issue_Detail_Id,IPM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,CategorySeparate CS,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD Where IM.Category_Id=CS.CategoryId And IPM.ITEM_ID=IM.ITEM_ID  And Id=0 and PD.Item_Finished_id=IPM.Item_Finished_id And ProductCode='" + TxtProductCode.Text + "' And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DDCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                    CATEGORY_DEPENDS_CONTROLS();
                    DDItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                    if (DDunit.SelectedValue == "2")
                    {
                        if (variable.VarNewQualitySize == "1")
                        {
                            Str = "select Issue_Detail_Id,QDCS+ space(3) +SizeFt from ViewFindFinishedidItemidQDCSSNew,PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + " PIM where PIM.Item_Finished_Id=FinishedId and IssueOrderId=" + DDPONo.SelectedValue;
                        }
                        else
                        {
                            Str = "select Issue_Detail_Id,QDCS+ space(3) +SizeFt from ViewFindFinishedidItemidQDCSS,PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + " PIM where PIM.Item_Finished_Id=FinishedId and IssueOrderId=" + DDPONo.SelectedValue;
                        }
                    }
                    else if (DDunit.SelectedValue == "1")
                    {
                        if (variable.VarNewQualitySize == "1")
                        {
                            Str = "select Issue_Detail_Id,QDCS+ space(3) +SizeMtr from ViewFindFinishedidItemidQDCSSNew,PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + " PIM where PIM.Item_Finished_Id=FinishedId and IssueOrderId=" + DDPONo.SelectedValue;
                        }
                        else
                        {
                            Str = "select Issue_Detail_Id,QDCS+ space(3) +SizeMtr from ViewFindFinishedidItemidQDCSS,PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + " PIM where PIM.Item_Finished_Id=FinishedId and IssueOrderId=" + DDPONo.SelectedValue;
                        }
                    }
                    UtilityModule.ConditionalComboFill(ref DDDescription, Str, true, "-----------Select------");
                    DDDescription.SelectedValue = ds.Tables[0].Rows[0]["Issue_Detail_Id"].ToString();
                    Get_Detail();
                }
                else
                {
                    llMessageBox.Text = "ITEM CODE DOES NOT EXISTS....";
                    DDCategoryName.SelectedIndex = 0;
                    CATEGORY_DEPENDS_CONTROLS();
                    TxtProductCode.Text = "";
                    TxtProductCode.Focus();
                }
            }
        }
        else
        {
            DDCategoryName.SelectedIndex = 0;
            CATEGORY_DEPENDS_CONTROLS();
        }
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
    protected void ddStockQualityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddStockQualityType.SelectedValue == "3")
        {
            TxtPrefix.Text = "REJECT-";
        }
        else
        {
            if (DDItemName.SelectedIndex > 0)
            {
                TxtPrefix.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Item_Code from Item_Master Where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "").ToString();
                string get_year = DateTime.Now.ToString("dd-MMM-yyyy");
                string lastTwoChars = get_year.Substring(get_year.Length - 2);
                TxtPrefix.Text = (TxtPrefix.Text + "-" + lastTwoChars).Replace(" ", "");
            }
            else
            {
                TxtPrefix.Text = "";
            }
        }
        TxtPostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix((TxtPrefix.Text).ToUpper()));
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        Report();
    }
    private void Report()
    {
        string qry = "";
        DataSet ds;
        if (Session["varMasterCompanyIDForERP"].ToString() == "8")//for Anisha Carpet
        {
            if (variable.VarNewQualitySize == "1")
            {
                qry = @"Select CI.CompanyName,CI.CompAddr1,CI.CompAddr2,EI.EmpName,EI.Address As EmpAddress,EI.EmpId,PD.Qty,IM.Item_Name,
                    Colorname ,Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Size,
                    PM.ReceiveDate,PD.IssueOrderid,U.UnitName,PM.UnitId,PM.ChallanNo,
                    " + DDProcessName.SelectedValue + @" PROCESSID,Penality,PRemarks From 
                    PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetailNew vf,
                    Item_Master IM,CompanyInfo CI,EmpInfo EI,Unit U Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=vf.Item_Finished_id And 
                    IM.Item_Id=vf.Item_Id And  PM.Companyid=CI.CompanyId And PM.Empid=EI.EmpId And PM.UnitId=U.UnitId And QualityType<>3 and 
                    PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And PM.CompanyId=" + DDCompanyName.SelectedValue;
            }
            else
            {
                qry = @"Select CI.CompanyName,CI.CompAddr1,CI.CompAddr2,EI.EmpName,EI.Address As EmpAddress,EI.EmpId,PD.Qty,IM.Item_Name,
                    Colorname ,Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Size,
                    PM.ReceiveDate,PD.IssueOrderid,U.UnitName,PM.UnitId,PM.ChallanNo,
                    " + DDProcessName.SelectedValue + @" PROCESSID,Penality,PRemarks From 
                    PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail vf,
                    Item_Master IM,CompanyInfo CI,EmpInfo EI,Unit U Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=vf.Item_Finished_id And 
                    IM.Item_Id=vf.Item_Id And  PM.Companyid=CI.CompanyId And PM.Empid=EI.EmpId And PM.UnitId=U.UnitId And QualityType<>3 and 
                    PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And PM.CompanyId=" + DDCompanyName.SelectedValue;
            }
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        }
        else if (Session["varMasterCompanyIDForERP"].ToString() == "9")
        {
            if (chkForSlip.Checked == true)
            {
                if (variable.VarNewQualitySize == "1")
                {
                    qry = @"Select CI.CompanyName,CI.CompAddr1,CI.CompAddr2,
                 Sum((PD.Area*PD.Qty)) Area,Sum(PD.Qty) As Qty,Sum(PD.Weight) As Weight,EI.EmpName,EI.Address,Vf.DesignName,VF.ColorName,case When PM.unitId=1 Then vf.Sizemtr Else Case When PM.UnitId=2 Then vf.Sizeft Else case When PM.UnitId=6 Then  Sizeinch Else Sizeft End End End As Size,
                 OM.LocalOrder,PM.ChallanNo,PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,Sum(PD.Amount) As Amount,PD.IssueOrderid,
                 U.UnitName,PM.UnitId," + DDProcessName.SelectedValue + " PROCESSID,Penality,PRemarks From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,OrderMaster OM,
                 CompanyInfo CI,EmpInfo EI,Unit U,V_finishedItemDetailNew vf Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.OrderId=OM.OrderId And PD.Item_Finished_Id=vf.Item_Finished_id And 
                 PM.Companyid=CI.CompanyId And PM.Empid=EI.EmpId And PM.UnitId=U.UnitId And QualityType<>3 and 
                 PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + "  And PM.CompanyId=" + DDCompanyName.SelectedValue + @"
                 group by CI.CompanyName,CI.CompAddr1,CI.CompAddr2,EI.EmpName,EI.Address,Vf.DesignName,VF.ColorName,
                 U.UnitName,PM.UnitId,Penality,PRemarks,OM.LocalOrder,PM.ChallanNo,PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,sizemtr,sizeft,Sizeinch,PD.IssueOrderId";
                }
                else
                {
                    qry = @"Select CI.CompanyName,CI.CompAddr1,CI.CompAddr2,
                 Sum((PD.Area*PD.Qty)) Area,Sum(PD.Qty) As Qty,Sum(PD.Weight) As Weight,EI.EmpName,EI.Address,Vf.DesignName,VF.ColorName,case When PM.unitId=1 Then vf.Sizemtr Else Case When PM.UnitId=2 Then vf.Sizeft Else case When PM.UnitId=6 Then  Sizeinch Else Sizeft End End End As Size,
                 OM.LocalOrder,PM.ChallanNo,PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,Sum(PD.Amount) As Amount,PD.IssueOrderid,
                 U.UnitName,PM.UnitId," + DDProcessName.SelectedValue + " PROCESSID,Penality,PRemarks From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,OrderMaster OM,
                 CompanyInfo CI,EmpInfo EI,Unit U,V_finishedItemDetail vf Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.OrderId=OM.OrderId And PD.Item_Finished_Id=vf.Item_Finished_id And 
                 PM.Companyid=CI.CompanyId And PM.Empid=EI.EmpId And PM.UnitId=U.UnitId And QualityType<>3 and 
                 PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + "  And PM.CompanyId=" + DDCompanyName.SelectedValue + @"
                 group by CI.CompanyName,CI.CompAddr1,CI.CompAddr2,EI.EmpName,EI.Address,Vf.DesignName,VF.ColorName,
                 U.UnitName,PM.UnitId,Penality,PRemarks,OM.LocalOrder,PM.ChallanNo,PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,sizemtr,sizeft,Sizeinch,PD.IssueOrderId";
                }

                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            }
            else if (ChkForRejectPcsSlip.Checked == true)
            {
                qry = @"Select CI.CompanyName,CI.CompAddr1,CI.CompAddr2,
                 Sum((PD.Area*PD.Qty)) Area,Sum(PD.Qty) As Qty,Sum(PD.Weight) As Weight,EI.EmpName,EI.Address,Vf.DesignName,VF.ColorName,
				 case When PM.unitId=1 Then vf.Sizemtr Else Case When PM.UnitId=2 Then vf.Sizeft Else case When PM.UnitId=6 Then  Sizeinch Else Sizeft End End End As Size,
                 OM.LocalOrder,PM.ChallanNo,PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,Sum(PD.Amount) As Amount,PD.IssueOrderid,
                 U.UnitName,PM.UnitId," + DDProcessName.SelectedValue + @" PROCESSID,Penality,PRemarks,CI.Gstno,EI.Gstno 
                 From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM JOIN PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD ON PM.Process_Rec_Id=PD.Process_Rec_Id
				 JOIN OrderMaster OM ON PD.OrderId=OM.OrderId
				 JOIN CompanyInfo CI ON PM.Companyid=CI.CompanyId
				 JOIN V_finishedItemDetail vf  ON PD.Item_Finished_Id=vf.Item_Finished_id
				 JOIN EmpInfo EI ON PM.Empid=EI.EmpId
				 JOIN Unit U ON PM.UnitId=U.UnitId
                  Where PD.QualityType=3 and PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + "  And PM.CompanyId=" + DDCompanyName.SelectedValue + @"
                 group by CI.CompanyName,CI.CompAddr1,CI.CompAddr2,EI.EmpName,EI.Address,Vf.DesignName,VF.ColorName,
                 U.UnitName,PM.UnitId,Penality,PRemarks,OM.LocalOrder,PM.ChallanNo,PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,sizemtr,sizeft,sizeinch,PD.IssueOrderId,CI.Gstno,EI.Gstno";

                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            }
            else
            {
                SqlParameter[] array = new SqlParameter[3];
                array[0] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
                array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
                array[2] = new SqlParameter("@CompanyId", SqlDbType.Int);

                array[0].Value = DDPONo.SelectedValue;
                array[1].Value = DDProcessName.SelectedValue;
                array[2].Value = DDCompanyName.SelectedValue;

                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ProductionRecSummary", array);

            }
        }
        else
        {
            if (variable.VarNewQualitySize == "1")
            {
                qry = @"Select ICM.Category_Name,(PD.Area*PD.Qty) Area,PD.Qty,PD.Weight,CI.CompanyName,CI.CompAddr1,CI.CompAddr2,IM.Item_Name,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Description,
        PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,PD.Amount,PD.IssueOrderid,EI.EmpName,EI.Address,U.UnitName,PM.UnitId,PM.ChallanNo,
        (Select * from [dbo].[Get_StockNoRec_Detail_Wise](PD.Process_Rec_Id,PD.Process_Rec_Detail_Id)) StockNo," + DDProcessName.SelectedValue + @" PROCESSID,Penality,PRemarks,CI.Gstno,EI.Gstno as Empgstno,Upper('" + DDProcessName.SelectedItem.Text + @"') as ProcessName,isnull(pd.Qapersonname,'') as Qaname,
        " + Session["varMasterCompanyIDForERP"] + @" as MasterCompanyId,isnull(NU.UserName,'') as UserName,isnull(PM.PartyChallanNo,'') as PartyChallanNo
        From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM(NoLock),PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD(NoLock),ViewFindFinishedidItemidQDCSSNew IPM(NoLock),
        Item_Master IM(NoLock),ITEM_CATEGORY_MASTER ICM(NoLock),CompanyInfo CI(NoLock),EmpInfo EI(NoLock),Unit U(NoLock),NewUserDetail NU(NoLock) 
        Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=IPM.Finishedid And 
        IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And PM.Companyid=CI.CompanyId And PM.Empid=EI.EmpId And PM.UnitId=U.UnitId And QualityType<>3 and 
        NU.UserId=PM.USERID and PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And PM.CompanyId=" + DDCompanyName.SelectedValue;
            }
            else
            {
                qry = @"Select ICM.Category_Name,(PD.Area*PD.Qty) Area,PD.Qty,PD.Weight,CI.CompanyName,CI.CompAddr1,CI.CompAddr2,IM.Item_Name,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Description,
        PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,PD.Amount,PD.IssueOrderid,EI.EmpName,EI.Address,U.UnitName,PM.UnitId,PM.ChallanNo,
        (Select * from [dbo].[Get_StockNoRec_Detail_Wise](PD.Process_Rec_Id,PD.Process_Rec_Detail_Id)) StockNo," + DDProcessName.SelectedValue + @" PROCESSID,Penality,PRemarks,CI.Gstno,EI.Gstno as Empgstno,Upper('" + DDProcessName.SelectedItem.Text + @"') as ProcessName,isnull(pd.Qapersonname,'') as Qaname, 
        " + Session["varMasterCompanyIDForERP"] + @" as MasterCompanyId,isnull(NU.UserName,'') as UserName,isnull(PM.PartyChallanNo,'') as PartyChallanNo 
        From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM(NoLock),PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD(NoLock),ViewFindFinishedidItemidQDCSS IPM(NoLock),
        Item_Master IM(NoLock),ITEM_CATEGORY_MASTER ICM(NoLock),CompanyInfo CI(NoLock),EmpInfo EI(NoLock),Unit U(NoLock) ,NewUserDetail NU(NoLock) 
        Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=IPM.Finishedid And 
        IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And PM.Companyid=CI.CompanyId And PM.Empid=EI.EmpId And PM.UnitId=U.UnitId And QualityType<>3 and 
        NU.UserId=PM.USERID and  PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And PM.CompanyId=" + DDCompanyName.SelectedValue;
            }
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "8")//Anisa Carpet
            {
                Session["rptFileName"] = "~\\Reports\\RptBazarSlip.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptBazarSlip.xsd";
            }
            else if (Session["varMasterCompanyIDForERP"].ToString() == "9")
            {
                if (chkForSlip.Checked == true)
                {
                    Session["rptFileName"] = "~\\Reports\\RptBazarSlipforHafizia.rpt";
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\RptBazarSlipforHafizia.xsd";
                }
                else if (ChkForRejectPcsSlip.Checked == true)
                {
                    Session["rptFileName"] = "~\\Reports\\RptBazarRejectPcsSlipforHafizia.rpt";
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\RptBazarRejectPcsSlipforHafizia.xsd";
                }
                else
                {
                    //
                    Session["rptFileName"] = "~\\Reports\\RptProductionSummaryCrossTab.rpt";
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\RptProductionSummaryCrossTab.xsd";
                }
            }           
            else
            {
                Session["rptFileName"] = "~\\Reports\\WCarpetRecvNew.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\WCarpetRecvNew.xsd";
            }
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    private void Report1()
    {
        string qry = "";
        if (variable.VarNewQualitySize == "1")
        {
            qry = @"Select ICM.Category_Name,(PD.Area*PD.Qty) Area,PD.Qty,PD.Weight,CI.CompanyName,CI.CompAddr1,CI.CompAddr2,IM.Item_Name,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Description,
        Replace(Convert(varchar(11),PM.ReceiveDate,106),' ','-') as ReceiveDate,PD.Length,PD.Width,PD.Rate,PD.Amount,PD.IssueOrderid,EI.EmpName,EI.Address,U.UnitName,PM.UnitId,PM.ChallanNo,
        (Select * from [dbo].[Get_StockNoRec_Detail_Wise](PD.Process_Rec_Id,PD.Process_Rec_Detail_Id)) StockNo,1 PROCESSID,PD.GatePassNo 
         ,PD.IssueOrderid as FolioChallanNo
        From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,ViewFindFinishedidItemidQDCSSNew IPM,
        Item_Master IM,ITEM_CATEGORY_MASTER ICM,CompanyInfo CI,EmpInfo EI,Unit U Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=IPM.Finishedid And 
        IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And PM.Companyid=CI.CompanyId And PM.Empid=EI.EmpId And PM.UnitId=U.UnitId And QualityType=3 and 
        PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And PM.CompanyId=" + DDCompanyName.SelectedValue;
        }
        else
        {

            qry = @"Select ICM.Category_Name,(PD.Area*PD.Qty) Area,PD.Qty,PD.Weight,CI.CompanyName,CI.CompAddr1,CI.CompAddr2,IM.Item_Name,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Description,
       Replace(Convert(varchar(11),PM.ReceiveDate,106),' ','-') as ReceiveDate ,PD.Length,PD.Width,PD.Rate,PD.Amount,PD.IssueOrderid,EI.EmpName,EI.Address,U.UnitName,PM.UnitId,PM.ChallanNo,
        (Select * from [dbo].[Get_StockNoRec_Detail_Wise](PD.Process_Rec_Id,PD.Process_Rec_Detail_Id)) StockNo,1 PROCESSID,PD.GatePassNo 
        ,(Select ChallanNo from Process_Issue_master_1 Where IssueOrderId=PD.IssueOrderid) as FolioChallanNo
        From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,ViewFindFinishedidItemidQDCSS IPM,
        Item_Master IM,ITEM_CATEGORY_MASTER ICM,CompanyInfo CI,EmpInfo EI,Unit U Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=IPM.Finishedid And 
        IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And PM.Companyid=CI.CompanyId And PM.Empid=EI.EmpId And PM.UnitId=U.UnitId And QualityType=3 and 
        PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And PM.CompanyId=" + DDCompanyName.SelectedValue;
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Session["VarCompanyNo"].ToString() == "30")
            {
                Session["rptFileName"] = "~\\Reports\\RptBazarGatePassNewSamara.rpt";
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\RptBazarGatePassNew.rpt";
            }
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptBazarGatePassNew.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    protected void BtnGatePass_Click(object sender, EventArgs e)
    {
        Report1();
    }
    protected void TxtLocalOrderNo_TextChanged(object sender, EventArgs e)
    {
        if (TxtLocalOrderNo.Text != "")
        {
            UtilityModule.ConditionalComboFill(ref DDFolioNo, @"Select Distinct PM.IssueOrderId,cast(PM.IssueOrderId as varchar(100)) as IssueOrderid1 
            from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,ordermaster OM  
            Where PM.ISSUEORDERID=PD.ISSUEORDERID And PD.orderid=OM.orderid And PD.PQty<>0 And PM.status='Pending' And PM.CompanyID = + " + Session["CurrentWorkingCompanyID"] + @"
            And OM.LocalOrder='" + TxtLocalOrderNo.Text + @"'  
            order by PM.Issueorderid", true, "--Select--");
        }
    }
    protected void DDFolioNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtPOrderNo.Text = DDFolioNo.SelectedValue;
        TxtPOrderNo_TextChanged(sender, new EventArgs());

    }
    protected void TxtPOrderNo_TextChanged(object sender, EventArgs e)
    {
        TxtChallanNo.Text = "";
        if (TxtPOrderNo.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                con.Open();
                ////DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select CompanyId,ProcessId,EmpId,IssueOrderId from TEMP_PROCESS_ISSUE_MASTER_NEW Where ISSUEORDERID=" + TxtPOrderNo.Text + "");

                DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, @"Select CompanyId,ProcessId,EmpId,IssueOrderId 
                    From TEMP_PROCESS_ISSUE_MASTER_NEW 
                    Where CompanyId = " + Session["CurrentWorkingCompanyID"] + " And ChallanNo='" + TxtPOrderNo.Text + "'");
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    DDProcessName.SelectedValue = Ds.Tables[0].Rows[0]["ProcessId"].ToString();
                    ProcessSelectedChange();
                    if (DDEmployeeNamee.Items.Count > 0)
                    {
                        DDEmployeeNamee.SelectedValue = Ds.Tables[0].Rows[0]["EmpId"].ToString();
                        EmployeeSelectedChange();
                    }
                    if (DDPONo.Items.Count > 0)
                    {
                        DDPONo.SelectedValue = Ds.Tables[0].Rows[0]["IssueOrderId"].ToString();
                        PoNoSelectedChange();
                    }
                    DDCategoryName.Focus();
                }
                else
                {
                    llMessageBox.Visible = true;
                    llMessageBox.Text = "Pls Enter Proper POrderNo";
                    TxtPOrderNo.Text = "";
                    TxtPOrderNo.Focus();
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Process/ProcessReceive.aspx");
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void TxtPEnality_TextChanged(object sender, EventArgs e)
    {
        TxtWeight.Focus();
    }
    protected void TxtPostfix_TextChanged(object sender, EventArgs e)
    {
        llMessageBox.Text = "";
        string TStockNo = TxtPrefix.Text + TxtPostfix.Text;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select TStockNo from CarpetNumber Where TStockNo='" + TStockNo + "' And CompanyId=" + DDCompanyName.SelectedValue + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            llMessageBox.Text = "Stock No AllReady Exits....";
            TxtPostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix((TxtPrefix.Text).ToUpper()));
        }
    }
    protected void DGRec_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillchkbox();
    }
    public void fillchkbox()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @"Select QcmasterID from QCDETAIL where RecieveDetailID=" + DGRec.SelectedDataKey.Value + " and Qcvalue='1' and RefName='PurchaseReceiveDetail'";
        SqlDataAdapter sda = new SqlDataAdapter(qry, con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            for (int j = 0; j < grdqualitychk.Rows.Count; j++)
            {
                //if (((Label)grdqualitychk.Rows[j].FindControl("Label1")).Text == ds.Tables[0].Rows[i]["QcmasterID"].ToString())
                if (Convert.ToString(grdqualitychk.DataKeys[j].Value) == ds.Tables[0].Rows[i]["QcmasterID"].ToString())
                {
                    CheckBox chk = (CheckBox)grdqualitychk.Rows[j].FindControl("CheckBox1");
                    chk.Checked = true;
                }
            }
        }

    }
    protected void btnqcchkpreview_Click(object sender, EventArgs e)
    {
        reportQcheck();
    }
    private void reportQcheck()
    {

        DataSet ds = new DataSet();
        int Issueorderid = 0;
        #region
        //        string view = "ViewFindFinishedidItemidQDCSS";
        //        if (variable.VarNewQualitySize == "1")
        //        {
        //            view = "ViewFindFinishedidItemidQDCSSNew";
        //        }

        //        qry = @"Select ICM.Category_Name,PD.Qty*PD.Area Area,PD.Qty,PD.Weight,CI.CompanyName,CI.CompAddr1,CI.CompAddr2,IM.Item_Name,IPM.QDCS + Space(5) + Width+'x'+Length Description,
        //                PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,PD.Amount,PD.IssueOrderid,EI.EmpName,EI.Address,U.UnitName,'' ShortName,
        //                PM.UnitId,PM.ChallanNo,(Select * from [dbo].[Get_StockNoRec_Detail_Wise](PD.Process_Rec_Id,PD.Process_Rec_Detail_Id)) StockNo,PM.Process_Rec_Id,PD.Process_Rec_Detail_Id,
        //                PD.ActualWidth,PD.ActualLength,
        //                dbo.F_GETQCValue('PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + "'," + DDProcessName.SelectedValue + @",PM.Process_rec_id,PD.Process_Rec_Detail_Id) as QCVALUE,
        //                dbo.F_GETQCParameterValue('PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + "'," + DDProcessName.SelectedValue + @",PM.Process_rec_id) as QCPARAMETER
        //                From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD," + view + @" IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM,CompanyInfo CI,
        //                EmpInfo EI,Unit U Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And 
        //                PM.Companyid=CI.CompanyId And PM.EmpId=EI.EmpId And PM.UnitId=U.UnitId And IM.Category_Id=ICM.Category_Id And PD.QualityType<>3 And PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"];


        //        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        #endregion
        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@Processrecid", ViewState["Process_Rec_Id"]);
        param[2] = new SqlParameter("@issueorderid", Issueorderid);
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetQcreportBazar", param);
        #region MyRegion

        //        DataTable mytable = new DataTable();
        //        mytable.Columns.Add("PrtID", typeof(int));
        //        mytable.Columns.Add("SName", typeof(string));
        //        mytable.Columns.Add("QCValue", typeof(string));

        //        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
        //        {
        //            string str = @"Select SName, Case when QCValue='1' then 'YES' else 'NO' END QCValue from QCdetail QCD inner join QCMaster QCM ON 
        //                         QCD.QCMasterID=QCM.ID Inner Join QCParameter QCP ON QCM.ParaID=QCP.ParaID
        //                         Where RefName= 'PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + "' And ProcessId=" + DDProcessName.SelectedValue + " And QCD.RecieveID=" + ViewState["Process_Rec_Id"] + " And QCD.RecieveDetailID=" + ds.Tables[0].Rows[j]["Process_Rec_Detail_Id"];
        //            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //            SqlDataAdapter sda = new SqlDataAdapter(str, con);
        //            DataTable dt = new DataTable();
        //            sda.Fill(dt);
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                if (SName == "" && QCValue == "")
        //                {
        //                    SName = dt.Rows[i]["SName"].ToString();
        //                    QCValue = dt.Rows[i]["QCValue"].ToString();
        //                }
        //                else
        //                {
        //                    SName = SName + ' ' + dt.Rows[i]["SName"].ToString();
        //                    QCValue = QCValue + ' ' + dt.Rows[i]["QCValue"].ToString();
        //                }
        //            }
        //            mytable.Rows.Add(ds.Tables[0].Rows[j]["Process_Rec_Detail_Id"], SName, QCValue);
        //            SName = "";
        //            QCValue = "";
        //        }
        //        ds.Tables.Add(mytable);

        #endregion
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "Reports/WCarpetRecvQCNew.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\WCarpetRecvQCNew.xsd";
            Session["GetDataset"] = ds;
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
    //protected void DGRec_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //Add CSS class on header row.
    //    if (e.Row.RowType == DataControlRowType.Header)
    //        e.Row.CssClass = "header";

    //    //Add CSS class on normal row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Normal)
    //        e.Row.CssClass = "normal";

    //    //Add CSS class on alternate row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Alternate)
    //        e.Row.CssClass = "alternate";
    //}
    //protected void grdqualitychk_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //Add CSS class on header row.
    //    if (e.Row.RowType == DataControlRowType.Header)
    //        e.Row.CssClass = "header";

    //    //Add CSS class on normal row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Normal)
    //        e.Row.CssClass = "normal";

    //    //Add CSS class on alternate row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Alternate)
    //        e.Row.CssClass = "alternate";
    //}
    private void fillorderdetail()
    {
        string sql = "";
        string View = "V_FinishedItemDetail";
        if (variable.VarNewQualitySize == "1")
        {
            View = "V_FinishedItemDetailNew";
        }

        //        sql = @"Select CATEGORY_NAME+'  '+ Item_Name+'  '+QualityName+'  '+Designname+'  '+ColorName+'  '+ShapeName+'  '+Case When UnitId=1 Then SizeMtr Else Case When UnitId=2 Then  SizeFt Else Case When UnitId=6 Then SizeInch End End End+'  '+ShadeColorName As Description,
        //                       IsNull(Sum(PID.Qty),0)-Isnull(Sum(Pid.CancelQty),0) As Qty,isnull(sum(pid.PQty),0)-Isnull(Sum(CancelQty),0) as issueqqty,CATEGORY_ID,v.ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId,
        //                       PIM.UnitId UNIT,PID.Item_Finished_Id as finishedid,Issue_Detail_Id From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PIM,
        //                       PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PID," + View + @" V Where PIM.IssueOrderId=PID.IssueOrderId And 
        //                       pid.item_finished_id=V.Item_Finished_Id And pid.issueorderid=" + DDPONo.SelectedValue + " And V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And PIM.CompanyId=" + DDCompanyName.SelectedValue + @" Group By CATEGORY_NAME,Item_Name,QualityName,Designname,
        //                       ColorName,ShadeColorName,ShapeName,SizeMtr,SizeFt,Sizeinch,UnitId,CATEGORY_ID,V.ITEM_ID,QualityId,ColorId,DesignId,SizeId,ShapeId,ShadecolorId,PID.Item_Finished_Id,Issue_Detail_Id Having isnull(sum(pid.PQty),0)>0";

        sql = @"select CATEGORY_NAME+'  '+ Item_Name+'  '+QualityName+'  '+Designname+'  '+ColorName+'  '+ShapeName+'  '+Case When UnitId=1 Then SizeMtr Else Case When UnitId=2 Then  SizeFt Else Case When UnitId=6 Then SizeInch End End End+'  '+ShadeColorName As Description,
                IsNull(Sum(PID.Qty),0)-Isnull(Sum(Pid.CancelQty),0) As Qty,isnull(sum(pid.PQty),0)-Isnull(Sum(RejectQty),0)-Isnull(Sum(CancelQty),0) as issueqqty,CATEGORY_ID,v.ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId,
                PIM.UnitId UNIT,PID.Item_Finished_Id as finishedid,Issue_Detail_Id,
                ISNULL(case When PIM.Unitid=1 Then Max(VC.IQTY)*1.196 Else MAx(VC.IQTY) ENd,0) as Lagat,UnitId,PID.OrderId,isnull(OM.LocalOrder,'') as LocalOrder
                From Process_issue_Master_" + DDProcessName.SelectedValue + " PIM inner Join Process_Issue_Detail_" + DDProcessName.SelectedValue + @" PID on PIM.IssueOrderId=PID.IssueOrderId
                inner join " + View + @" v on PID.Item_finished_id=V.ITEM_FINISHED_ID
                Left Join V_Weaverconsumption  VC on PIM.IssueOrderId=VC.issueorderid
                JOIN OrderMaster OM ON PID.OrderId=OM.OrderId
                Where PIM.IssueOrderId=" + DDPONo.SelectedValue + " and V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and PIM.Companyid=" + DDCompanyName.SelectedValue + @"
                Group By CATEGORY_NAME,Item_Name,QualityName,Designname,
                ColorName,ShadeColorName,ShapeName,SizeMtr,SizeFt,Sizeinch,UnitId,CATEGORY_ID,V.ITEM_ID,QualityId,ColorId,DesignId,
                SizeId,ShapeId,ShadecolorId,PID.Item_Finished_Id,Issue_Detail_Id,PID.OrderId,OM.LocalOrder 
                Having isnull(sum(pid.PQty),0)>0";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            dgorder.DataSource = ds;
            dgorder.DataBind();
            dgorder.Visible = true;
            lblLagat.Text = "Lagat  " + ds.Tables[0].Rows[0]["Lagat"].ToString() + " / " + (ds.Tables[0].Rows[0]["UnitId"].ToString() == "1" ? "SQ METER" : "SQ YARD") + "";
        }
        else
        {
            dgorder.Visible = false;
            lblLagat.Text = "";
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
    protected void dgorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        int r = Convert.ToInt32(dgorder.SelectedIndex.ToString());
        string lblbalnce = ((Label)dgorder.Rows[r].FindControl("lblbalnce")).Text;
        if (Convert.ToInt32(lblbalnce) > 0 && lblbalnce != "")
        {
            string category = ((Label)dgorder.Rows[r].FindControl("lblcategoryid")).Text;
            string Item = ((Label)dgorder.Rows[r].FindControl("lblitem_id")).Text;
            string Issue_Detail_Id = ((Label)dgorder.Rows[r].FindControl("Issue_Detail_Id")).Text;

            DDCategoryName.SelectedValue = category;
            ddcategorychange();
            DDItemName.SelectedValue = Item;
            DDitemchange();
            DDDescription.SelectedValue = Issue_Detail_Id;
            Get_Detail();
            TxtRecQty.Text = lblbalnce;
            llMessageBox.Text = "";
            llMessageBox.Visible = false;
        }
        else
        {
            llMessageBox.Text = "No Pending Qty";
            llMessageBox.Visible = true;
        }
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
    protected void ChkForRejectPcsSlip_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForRejectPcsSlip.Checked == true)
        {
            chkForSlip.Checked = false;
        }
        else
        {
            chkForSlip.Checked = true;
        }
    }
}