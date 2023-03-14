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
    static int Gridindex = -1;
    static int VarProcess_Rec_Detail_Id = 0;
    static string btnclickflag = "";
    static int rowindex = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
            string process;
            if (Session["varcompanyId"].ToString() == "9")
            {
                process = "Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where MasterCompanyId=" + Session["varCompanyId"] + @" and process_name_id in(1,16,35) Order By PROCESS_NAME";
            }
            else
            {
                process = "Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where MasterCompanyId=" + Session["varCompanyId"] + @" and Process_name_id=1 Order By PROCESS_NAME";
            }
            string str = @"select Distinct CI.CompanyId,CI.Companyname From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order by Companyname
                        " + process + @"
                        Select Unitid,UnitName from Unit Where Unitid in (1,2,7,4,6)
                        Select VarCompanyNo,VarProdCode From MasterSetting 
                        DELETE TEMP_PROCESS_ISSUE_MASTER_NEW  
                        Select * from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"];
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
            
            DDProcessName.Focus();
            if (variable.VarBAZARQAPEROSONNAME == "1")
            {
                TDQaname.Visible = true;
            }
            if (DDProcessName.Items.Count > 0)
            {
                if (Convert.ToInt32(Session["varcompanyno"]) == 7)
                {
                    DDProcessName.SelectedValue = "9";
                    ViewState["Process_Rec_Id"] = 0;
                    TxtRecDate.Enabled = true;
                    ProcessSelectedChange();
                    BtnPreview.Visible = false;
                    BtnGatePass.Visible = false;
                }
                else
                {
                    DDProcessName.SelectedIndex = 1;
                    DDProcessName_SelectedIndexChanged(sender, new EventArgs());
                }
                //UtilityModule.ConditionalComboFill(ref DDEmployeeNamee, "Select Distinct EI.EmpId,EI.EmpName from Empinfo EI,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM WHERE PIM.EmpId=EI.EmpId And EI.MasterCompanyId=" + Session["varCompanyId"] + "  order by EI.EmpName", true, "--Select--");
                if (Convert.ToInt32(Session["varcompanyno"]) == 7)
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
            }
            TxtRecDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtPostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix(TxtPrefix.Text));
            ViewState["Process_Rec_Id"] = 0;

            ChkEditChanged();
            ParameteLabel();
            //Comment on 15-jan-2015
            //if (ds.Tables[4].Rows.Count > 0)
            //{
            //    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
            //    {
            //        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Insert into TEMP_PROCESS_ISSUE_MASTER_NEW SELECT PM.Companyid,OM.Customerid,PD.Orderid," + ds.Tables[4].Rows[i]["Process_Name_Id"] + " ProcessId,PM.Empid,PM.IssueOrderid FROM PROCESS_ISSUE_MASTER_" + ds.Tables[4].Rows[i]["Process_Name_Id"] + " PM,PROCESS_ISSUE_DETAIL_" + ds.Tables[4].Rows[i]["Process_Name_Id"] + " PD,OrderMaster OM Where PM.IssueOrderid=PD.IssueOrderid And PD.Orderid=OM.Orderid");
            //    }
            //}
            switch (Convert.ToInt16(Session["varcompanyId"]))
            {
                case 9:
                    btnqcchkpreview.Visible = false;
                    BtnPreviewWithConsmp.Visible = false;
                    BtnGatePass.Visible = false;
                    chkForSlip.Visible = true;
                    chkForSlip.Checked = true;
                    ChkForRejectPcsSlip.Visible = true;
                    //BtnCurrentConsumption.Visible = false;
                    Label4.Text = "Folio No";
                    Label9.Text = "Folio No";
                    break;
                case 30:
                    TDactualW.Visible = false;
                    TDactualL.Visible = false;
                    break;

            }
            visible_comp();

        }

        // TxtPOrderNo.Focus();
        //controls visibility


    }
    protected void TxtPrefix_TextChanged(object sender, EventArgs e)
    {
        TxtPostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix((TxtPrefix.Text).ToUpper()));
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
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
            UtilityModule.ConditionalComboFill(ref DDEmployeeNamee, "Select Distinct EI.EmpId,EI.EmpName from Empinfo EI,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM WHERE PIM.EmpId=EI.EmpId And EI.MasterCompanyId=" + Session["varCompanyId"] + " order by EI.EmpName", true, "--Select--");
        }
    }
    protected void DDEmployeeNamee_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmployeeSelectedChange();
    }
    private void EmployeeSelectedChange()
    {
        ViewState["Process_Rec_Id"] = "0";
        TxtRecDate.Enabled = true;
        string Str = "";
        if (DDProcessName.SelectedIndex > 0 && DDEmployeeNamee.SelectedIndex > 0)
        {
            if (variable.VarLoomNoGenerated == "1")
            {
                Str = @"Select Distinct PM.IssueOrderId,case When " + Session["varcompanyId"] + @"=9 Then Om.localOrder+'/'+cast(PM.IssueOrderId as varchar(100)) 
                    ELse cast(PM.ChallanNo as varchar(100)) End as IssueOrderid1 
                    from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM 
                    Inner join PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD on PM.issueorderid=Pd.issueorderid 
                    inner join ordermaster OM on PD.orderid=Om.orderid 
                    left join LoomstockNo ls on Pm.issueorderid=Ls.issueorderid And ls.processID = " + DDProcessName.SelectedValue + @"
                    Where Ls.issueorderid is null And PM.Empid=" + DDEmployeeNamee.SelectedValue + " And Pm.CompanyId=" + DDCompanyName.SelectedValue;
            }
            else
            {
                Str = @"Select Distinct PM.IssueOrderId,case When " + Session["varcompanyId"] + @"=9 Then Om.localOrder+'/'+cast(PM.IssueOrderId as varchar(100)) 
                   ELse cast(PM.ChallanNo as varchar(100)) End as IssueOrderid1 from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM Inner join
                   PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD on PM.issueorderid=Pd.issueorderid inner join ordermaster OM on PD.orderid=Om.orderid
                    Where PM.Empid=" + DDEmployeeNamee.SelectedValue + " and Om.orderid=PD.orderid And PM.CompanyId=" + DDCompanyName.SelectedValue;
            }

            if (TxtPOrderNo.Text != "")
            {
                Str = Str + " And PM.ChallanNo='" + TxtPOrderNo.Text + "'";
            }
            Str = Str + " Order by IssueOrderId";
            UtilityModule.ConditionalComboFill(ref DDPONo, Str, true, "--Select--");
        }
    }
    protected void DDPONo_SelectedIndexChanged(object sender, EventArgs e)
    {
        PoNoSelectedChange();
    }
    private void PoNoSelectedChange()
    {
        string Str = "";
        llMessageBox.Visible = false;
        fillorderdetail();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            if (DDProcessName.SelectedIndex > 0 && DDPONo.SelectedIndex > 0)
            {
                Str = "";

                Str = "Select Distinct PM.Process_Rec_Id,Cast(PM.ChallanNo As NVarchar) + ' / ' + Replace(Convert(Varchar(11),ReceiveDate,106), ' ','-') From PROCESS_Receive_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_Receive_DETAIL_" + DDProcessName.SelectedValue + @" PD 
                        Where PM.Process_Rec_Id=PD.Process_Rec_Id And EmpId=" + DDEmployeeNamee.SelectedValue;

                Str = Str + " And PD.IssueOrderId=" + DDPONo.SelectedValue + "";

                ViewState["Process_Rec_Id"] = 0;
                TxtRecDate.Enabled = true;

                UtilityModule.ConditionalComboFill(ref DDReceiveNo, Str, true, "--Select--");

                if (Convert.ToInt32(ViewState["Process_Rec_Id"]) != 0)
                {
                    Str = "Select IssueOrderId from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " Where IssueOrderId=" + DDPONo.SelectedValue + " And UnitId in (Select UnitId from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PIM,
                           PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PRD Where PIM.IssueOrderId=PRD.IssueOrderId And Process_Rec_Id=" + ViewState["Process_Rec_Id"] + ")";
                    DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        DDunit.SelectedValue = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Unitid From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " Where IssueOrderId=" + DDPONo.SelectedValue).ToString();
                        UtilityModule.ConditionalComboFill(ref DDCategoryName, "Select Distinct ICM.Category_Id,Category_Name from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PID,ITEM_CATEGORY_MASTER ICM,ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM where IM.CATEGORY_ID=ICM.CATEGORY_ID AND IPM.Item_Id=IM.Item_Id AND PID.Item_Finished_Id=IPM.Item_Finished_Id and PID.IssueOrderId=" + DDPONo.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
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
                    if (Session["varcompanyno"].ToString() != "7")
                    {
                        DDunit.SelectedValue = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Unitid From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " Where IssueOrderId=" + DDPONo.SelectedValue).ToString();
                    }
                    UtilityModule.ConditionalComboFill(ref DDCategoryName, "Select Distinct ICM.Category_Id,Category_Name from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PID,ITEM_CATEGORY_MASTER ICM,ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM where IM.CATEGORY_ID=ICM.CATEGORY_ID AND IPM.Item_Id=IM.Item_Id AND PID.Item_Finished_Id=IPM.Item_Finished_Id and PID.IssueOrderId=" + DDPONo.SelectedValue + " And IPM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select--");
                }
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceive.aspx");
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
        UtilityModule.ConditionalComboFill(ref DDItemName, "select Distinct IM.Item_Id,Item_Name from Item_Master IM ,ITEM_PARAMETER_MASTER IPM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PID where PID.IssueOrderId=" + DDPONo.SelectedValue + " and PID.Item_Finished_Id=IPM.Item_Finished_Id and IM.Item_Id=IPM.Item_Id And IM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "---Select----");
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
                TxtPrefix.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Item_Code from Item_Master Where Item_Id=" + DDItemName.SelectedValue + " and mastercompanyId=" + Session["varcompanyId"] + "").ToString();
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
                    case 30:
                        TxtPrefix.Text = (TxtPrefix.Text + lastTwoChars + "-").Replace(" ", "");
                        break;
                    default:
                        TxtPrefix.Text = (TxtPrefix.Text + "-" + lastTwoChars).Replace(" ", "");
                        break;

                }
                //TxtPrefix.Text = (TxtPrefix.Text + "-" + lastTwoChars).Replace(" ", "");
                //// int VARQCTYPE = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VARQCTYPE From MasterSetting"));
                if (variable.VarQctype == "1")
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
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceive.aspx");
            llMessageBox.Visible = true;
            llMessageBox.Text = ex.Message;
        }
    }
    private void fillgrdquality()
    {
        Trupdateqcdetail.Visible = false;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select QCMaster.ID,SrNo,ParaName from QCParameter inner join QCMaster on QCParameter.ParaID=QCMaster.ParaID where CategoryID=" + DDCategoryName.SelectedValue + " and ItemID=" + DDItemName.SelectedValue + " and ProcessID=" + DDProcessName.SelectedValue + " order by SrNo");
        grdqualitychk.DataSource = ds;
        grdqualitychk.DataBind();
    }
    private void Item_SelectedIndexChange()
    {
        string str = "";
        if (Session["varcompanyno"].ToString() == "7")
        {
            if (variable.VarNewQualitySize == "1")
            {
                str = "Select Issue_Detail_Id,QDCS+ space(3) + Case When " + DDunit.SelectedValue + "=2 Then ISNULL(ProdSizeFt,'') Else ISNULL(ProdSizeMtr,'') End  from ViewFindFinishedidItemidQDCSSNew,PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + " PIM Where PIM.Item_Finished_Id=FinishedId  And ITEM_ID=" + DDItemName.SelectedValue + " and IssueOrderId=" + DDPONo.SelectedValue;
            }
            else
            {
                str = "Select Issue_Detail_Id,QDCS+ space(3) + Case When " + DDunit.SelectedValue + "=2 Then ISNULL(ProdSizeFt,'') Else ISNULL(ProdSizeMtr,'') End  from ViewFindFinishedidItemidQDCSS,PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + " PIM Where PIM.Item_Finished_Id=FinishedId  And ITEM_ID=" + DDItemName.SelectedValue + " and IssueOrderId=" + DDPONo.SelectedValue;
            }
        }
        else if (hncomp.Value == "6")
        {
            if (variable.VarNewQualitySize == "1")
            {
                str = "select Issue_Detail_Id,QDCS+ space(3) +ProdSizeMtr from ViewFindFinishedidItemidQDCSSNew,PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + " PIM Where PIM.Item_Finished_Id=FinishedId and IssueOrderId=" + DDPONo.SelectedValue;
            }
            else
            {
                str = "select Issue_Detail_Id,QDCS+ space(3) +ProdSizeMtr from ViewFindFinishedidItemidQDCSS,PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + " PIM Where PIM.Item_Finished_Id=FinishedId and IssueOrderId=" + DDPONo.SelectedValue;
            }
        }
        else
        {
            if (variable.VarNewQualitySize == "1")
            {
                str = "select Issue_Detail_Id,QDCS+ space(3) +case When " + DDunit.SelectedValue + "=6 Then Sizeinch Else case When " + DDunit.SelectedValue + "=1 then isnull(Prodsizemtr,'') Else isnull(ProdSizeft,'') End End  from ViewFindFinishedidItemidQDCSSNew,PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + " PIM where PIM.Item_Finished_Id=FinishedId and IssueOrderId=" + DDPONo.SelectedValue;
            }
            else
            {
                str = "select Issue_Detail_Id,QDCS+ space(3) +case When " + DDunit.SelectedValue + "=6 Then Sizeinch Else case When " + DDunit.SelectedValue + "=1 then isnull(Prodsizemtr,'') Else isnull(ProdSizeft,'') End End  from ViewFindFinishedidItemidQDCSS,PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + " PIM where PIM.Item_Finished_Id=FinishedId and IssueOrderId=" + DDPONo.SelectedValue;
            }
        }
        if (BtnSave.Text == "")
        {
            str = str + " And PIM.PQty>0";
        }
        UtilityModule.ConditionalComboFill(ref DDDescription, str, true, "-----------Select-------");
    }
    protected void DDDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        Get_Detail();
    }
    private void Get_Detail()
    {
        //  DataSet DS = null;
        //  SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
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
            //con.Open();
            //string strsql = "select Qty-CancelQty Qty,PQty-CancelQty PQty,Length,Width,Area,Rate,Comm from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " where Issue_Detail_Id=" + DDDescription.SelectedValue;
            //DS = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            //TxtPQty.Text = DS.Tables[0].Rows[0]["PQty"].ToString();
            //Txtlength.Text = DS.Tables[0].Rows[0]["Length"].ToString();
            //TxtWidth.Text = DS.Tables[0].Rows[0]["Width"].ToString();
            //TxtArea.Text = DS.Tables[0].Rows[0]["Area"].ToString();
            //TxtRate.Text = DS.Tables[0].Rows[0]["Rate"].ToString();
            //TxtIssuQty.Text = DS.Tables[0].Rows[0]["Qty"].ToString();
            //TxtCommission.Text = DS.Tables[0].Rows[0]["Comm"].ToString();
        }
        catch (Exception ex) { UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceive.aspx"); }

    }
    private void QCSAVE(SqlTransaction Tran, int ReceiveId, int ReceiveDetailId, string tablename)
    {
        #region
        //string checkpara = "";
        //string noncheck = "";

        //for (int i = 0; i < grdqualitychk.Rows.Count; i++)
        //{
        //    CheckBox chk = (CheckBox)grdqualitychk.Rows[i].FindControl("CheckBox1");
        //    if (chk.Checked)
        //    {
        //        if (checkpara == "")
        //        {
        //            checkpara = grdqualitychk.DataKeys[i].Value.ToString();
        //        }
        //        else
        //        {
        //            checkpara = checkpara + "," + grdqualitychk.DataKeys[i].Value.ToString();
        //        }
        //    }
        //    else
        //        if (noncheck == "")
        //        {
        //            noncheck = grdqualitychk.DataKeys[i].Value.ToString();
        //        }
        //        else
        //        {
        //            noncheck = noncheck + "," + grdqualitychk.DataKeys[i].Value.ToString();
        //        }
        //}
        //SqlParameter[] _arrpara1 = new SqlParameter[5];
        //_arrpara1[0] = new SqlParameter("@ReceiveId", SqlDbType.Int);
        //_arrpara1[1] = new SqlParameter("@ReceiveDetailId", SqlDbType.Int);
        //_arrpara1[2] = new SqlParameter("@checkpara", SqlDbType.NVarChar, 50);
        //_arrpara1[3] = new SqlParameter("@noncheck", SqlDbType.NVarChar, 50);
        //_arrpara1[4] = new SqlParameter("@TableName", SqlDbType.NVarChar, 50);
        //_arrpara1[0].Value = ReceiveId;
        //_arrpara1[1].Value = ReceiveDetailId;
        //_arrpara1[2].Value = checkpara;
        //_arrpara1[3].Value = noncheck;
        //_arrpara1[4].Value = tablename;
        //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PurchaseReceivequalitychk", _arrpara1);
        #endregion
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
        SqlParameter[] _arrpara1 = new SqlParameter[4];
        _arrpara1[0] = new SqlParameter("@ReceiveId", ReceiveId);
        _arrpara1[1] = new SqlParameter("@ReceiveDetailId", ReceiveDetailId);
        _arrpara1[2] = new SqlParameter("@TableName", tablename);
        _arrpara1[3] = new SqlParameter("@dt", dt);
        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_SAVEQCDETAIL]", _arrpara1);
    }
    protected void TxtPOrderNo_TextChanged(object sender, EventArgs e)
    {
        if (TxtPOrderNo.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                con.Open();
                DataSet Ds;
                //DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select CompanyId,ProcessId,EmpId,IssueOrderId from TEMP_PROCESS_ISSUE_MASTER_NEW Where ISSUEORDERID=" + TxtPOrderNo.Text + "");
                if (Session["varcompanyid"].ToString() == "9")
                {
                    Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, @"select distinct  Companyid,Empid,processid,IssueOrderId  from  (select *,1 As processid from PROCESS_ISSUE_MASTER_1
                                                                           Union all 
                                                    select *,16 as Processid from PROCESS_ISSUE_MASTER_16
                                                    Union all 
                                                    select *,35 as Processid from PROCESS_ISSUE_MASTER_35) as ProcessIssueMaster Where Companyid = " + DDCompanyName.SelectedValue + " And ChallanNo='" + TxtPOrderNo.Text + "'");
                }
                else
                {
                    ////Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "select CompanyId,1 as ProcessId,EmpId,issueOrderId from PROCESS_ISSUE_MASTER_1 Where IssueOrderId=" + TxtPOrderNo.Text + "");

                    Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "select CompanyId,1 as ProcessId,EmpId,issueOrderId from PROCESS_ISSUE_MASTER_1 Where Companyid = " + DDCompanyName.SelectedValue + " And ChallanNo='" + TxtPOrderNo.Text + "'");
                }
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
                    if (DDReceiveNo.Items.Count > 0)
                    {
                        DDReceiveNo.SelectedIndex = 1;
                        ViewState["Process_Rec_Id"] = DDReceiveNo.SelectedValue;
                    }
                    Fill_Grid();
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
                UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceive.aspx");
                llMessageBox.Text = ex.Message;
                llMessageBox.Visible = true;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
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
                Where PD.Item_Finished_Id=VF.Item_Finished_Id And PD.Issue_Detail_Id=" + DDDescription.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
                }
                else
                {
                    Shape = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PD,V_FinishedItemDetail VF 
                Where PD.Item_Finished_Id=VF.Item_Finished_Id And PD.Issue_Detail_Id=" + DDDescription.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
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
        if (UtilityModule.VALIDDROPDOWNLIST(DDReceiveNo) == false)
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
        if (Session["varcompanyId"].ToString() == "9")
        {
            CheckWeightRange();
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();

        //if (Session["VarCompanyId"].ToString() != "9")
        //{
            TxtRecQty_TextChanged(sender, new EventArgs());
        //}

        if (Session["varCompanyId"].ToString() == "30")
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
            for (int i = 0; i < grdqualitychk.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)grdqualitychk.Rows[i].FindControl("CheckBox1");
                TextBox txtqcreason = (TextBox)grdqualitychk.Rows[i].FindControl("txtqcreason");
                txtqcreason.Text = "";
                chk.Checked = false;
            }
        }
    }
    //****************************************************UpdateData**********************************************************************
    private void Update()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[23];

            _arrpara[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@Process_Rec_Detail_Id", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@Length", SqlDbType.NVarChar);
            _arrpara[3] = new SqlParameter("@Width", SqlDbType.NVarChar);
            _arrpara[4] = new SqlParameter("@Area", SqlDbType.Float);
            _arrpara[5] = new SqlParameter("@Rate", SqlDbType.Float);
            _arrpara[6] = new SqlParameter("@Amount", SqlDbType.Float);
            _arrpara[7] = new SqlParameter("@Qty", SqlDbType.Int);
            _arrpara[8] = new SqlParameter("@Weight", SqlDbType.Float);
            _arrpara[9] = new SqlParameter("@Penality", SqlDbType.Float);
            _arrpara[10] = new SqlParameter("@Comm", SqlDbType.Float);
            _arrpara[11] = new SqlParameter("@CommAmt", SqlDbType.Float);

            if (ChkEdit.Checked == true)
            {
                _arrpara[0].Value = DDReceiveNo.SelectedValue;
            }
            else
            {
                _arrpara[0].Value = ViewState["Process_Rec_Id"];
            }
            _arrpara[1].Value = DGRec.SelectedValue;
            _arrpara[2].Value = Txtlength.Text == "" ? "0" : string.Format("{0:#0.00}", Convert.ToDouble(Txtlength.Text));
            _arrpara[3].Value = TxtWidth.Text == "" ? "0" : string.Format("{0:#0.00}", Convert.ToDouble(TxtWidth.Text));
            _arrpara[4].Value = TxtArea.Text;
            _arrpara[5].Value = TxtRate.Text;
            int VarCalType = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select CalType from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " Where IssueOrderid=" + DDPONo.SelectedValue));
            if (VarCalType == 0)
            {
                _arrpara[6].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtRate.Text) * Convert.ToDouble(TxtRecQty.Text)));
                _arrpara[11].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtCommission.Text == "" ? "0" : TxtCommission.Text) * Convert.ToDouble(TxtRecQty.Text)));
            }
            if (VarCalType == 1)
            {
                _arrpara[6].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtRate.Text) * Convert.ToDouble(TxtRecQty.Text)));
                _arrpara[11].Value = String.Format("{0:#0.00}", (Convert.ToDouble(TxtCommission.Text == "" ? "0" : TxtCommission.Text) * Convert.ToDouble(TxtRecQty.Text)));
            }
            _arrpara[7].Value = TxtRecQty.Text;
            _arrpara[8].Value = (TxtWeight.Text == "" ? "0" : TxtWeight.Text);
            _arrpara[9].Value = (TxtPEnality.Text == "" ? "0" : TxtPEnality.Text);
            _arrpara[10].Value = (TxtCommission.Text == "" ? "0" : TxtCommission.Text);

            string str1 = @"Update PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + " set Length=" + _arrpara[2].Value + ",Width=" + _arrpara[3].Value + ",Area=" + _arrpara[4].Value + ",Rate=" + _arrpara[5].Value + ",Amount=" + _arrpara[6].Value + ",Qty=" + _arrpara[7].Value + ",Weight=" + _arrpara[8].Value + ",Penality=" + _arrpara[9].Value + ",Comm=" + _arrpara[10].Value + ",CommAmt=" + _arrpara[11].Value + " where Process_Rec_Id=" + _arrpara[0].Value + " and Process_Rec_Detail_Id=" + _arrpara[1].Value;
            string tablename = "PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + "";
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);

            //---------------------Update Process_Issue_Detail Table----------------------

            str1 = @"Update Process_Issue_Detail_" + DDProcessName.SelectedValue + " Set PQty=" + TxtPQty.Text + "-" + _arrpara[7].Value + " Where  IssueOrderid=" + DDPONo.SelectedValue + " And Issue_Detail_Id=" + DDDescription.SelectedValue;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);
            int VarFinishedid = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Item_Finished_id from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " Where Issue_Detail_Id=" + DDDescription.SelectedValue));
            int VarOrderid = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Orderid from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " Where IssueOrderid=" + DDPONo.SelectedValue + " And Issue_Detail_Id=" + DDDescription.SelectedValue));
            int n = Convert.ToInt32(Session["RQty"]) - Convert.ToInt32(TxtRecQty.Text);
            int m = Convert.ToInt32(TxtRecQty.Text);
            if (n > 0)
            {
                str1 = "Delete from carpetnumber where Process_Rec_Id=" + _arrpara[0].Value + " and Process_Rec_Detail_Id=" + _arrpara[1].Value + " and StockNo not in (select top(" + m + ") StockNo from CarpetNumber)";
                //str1="Delete Top (" + n + ")  from carpetnumber where Process_Rec_Id=" + _arrpara[0].Value + " and Process_Rec_Detail_Id=" + _arrpara[1].Value;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);
            }
            else if (n < 0)
            {
                n = Convert.ToInt32(TxtRecQty.Text) - Convert.ToInt32(Session["RQty"]);
                //88ManojUtilityModule.Insert_Into_Carpet_NumberwithProc(VarFinishedid, VarOrderid, n, TxtPrefix.Text, Convert.ToInt32(TxtPostfix.Text), Convert.ToInt32(DDCompanyName.SelectedValue), Convert.ToInt32(ViewState["Process_Rec_Id"]), Convert.ToInt32(DGRec.SelectedValue), TxtRecDate.Text, Tran, Convert.ToInt32(DDProcessName.SelectedValue));
            }
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete PROCESS_RECEIVE_CONSUMPTION Where Process_Rec_Detail_Id=" + _arrpara[1].Value);

            llMessageBox.Visible = true;
            QCSAVE(Tran, Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[8].Value), tablename);
            llMessageBox.Text = "Data Successfully Saved.......";
            Tran.Commit();
            TxtPostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix((TxtPrefix.Text).ToUpper()));
            ClearAfterSave();
            BtnSave.Visible = true;
            BtnUpdate.Visible = false;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceive.aspx");
            Tran.Rollback();
            llMessageBox.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    //*********************************************Process Issue**************************************************************************
    private void ProcessIssue()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[35];

            _arrpara[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@Empid", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@ReceiveDate", SqlDbType.DateTime);
            _arrpara[3] = new SqlParameter("@Unitid", SqlDbType.Int);
            _arrpara[4] = new SqlParameter("@Userid", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 250);
            _arrpara[6] = new SqlParameter("@Companyid", SqlDbType.Int);
            _arrpara[7] = new SqlParameter("@Remarks", SqlDbType.VarChar, 250);

            _arrpara[8] = new SqlParameter("@Process_Rec_Detail_Id", SqlDbType.Int);
            _arrpara[9] = new SqlParameter("@Item_Finished_id", SqlDbType.Int);
            _arrpara[10] = new SqlParameter("@Length", SqlDbType.VarChar, 250);
            _arrpara[11] = new SqlParameter("@Width", SqlDbType.VarChar, 250);
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
            _arrpara[24] = new SqlParameter("@PRemark", SqlDbType.VarChar, 250);
            _arrpara[25] = new SqlParameter("@CalType", SqlDbType.Int);
            _arrpara[26] = new SqlParameter("@FlagFixOrWeight", SqlDbType.Int);
            _arrpara[27] = new SqlParameter("@TDSPercentage", SqlDbType.Float);
            _arrpara[28] = new SqlParameter("@ProcessId", SqlDbType.Int);
            _arrpara[29] = new SqlParameter("@GatePassNo", SqlDbType.Int);
            _arrpara[30] = new SqlParameter("@Mastercompanyid", SqlDbType.Int);
            _arrpara[31] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            _arrpara[32] = new SqlParameter("@ActualLength", SqlDbType.VarChar, 10);
            _arrpara[33] = new SqlParameter("@ActualWidth", SqlDbType.VarChar, 10);
            _arrpara[34] = new SqlParameter("@Qapersonname", SqlDbType.VarChar, 50);

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
            _arrpara[4].Value = 0;
            _arrpara[5].Direction = ParameterDirection.InputOutput;
            _arrpara[5].Value = (TxtChallanNo.Text == "" ? "" : TxtChallanNo.Text);
            _arrpara[6].Value = DDCompanyName.SelectedValue;
            _arrpara[7].Value = (TxtRemarks.Text == "" ? "" : TxtRemarks.Text);

            //int VarRecDetailId = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Isnull(Max(Process_Rec_Detail_Id),0)+1 from PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue));
            _arrpara[8].Direction = ParameterDirection.Output;
            _arrpara[8].Value = 0;
            int VarFinishedid = 0;
            int VarOrderid = 0;
            int VarFlagFixOrWeight = 0;
            DataSet DsNew = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select Item_Finished_id,OrderId,FlagFixOrWeight from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD Where PM.IssueOrderid=PD.IssueOrderid And PM.IssueOrderid=" + DDPONo.SelectedValue + " And PD.Issue_Detail_Id=" + DDDescription.SelectedValue);
            if (DsNew.Tables[0].Rows.Count > 0)
            {
                VarFinishedid = Convert.ToInt32(DsNew.Tables[0].Rows[0]["Item_Finished_id"]);
                VarOrderid = Convert.ToInt32(DsNew.Tables[0].Rows[0]["OrderId"]);
                VarFlagFixOrWeight = Convert.ToInt32(DsNew.Tables[0].Rows[0]["FlagFixOrWeight"]);

                _arrpara[9].Value = VarFinishedid;
                _arrpara[21].Value = VarOrderid;
                _arrpara[26].Value = VarFlagFixOrWeight;
            }
            _arrpara[9].Value = VarFinishedid;
            if (Session["varcompanyid"].ToString() == "9")
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
            _arrpara[30].Value = Session["varcompanyId"];
            _arrpara[31].Direction = ParameterDirection.Output;
            _arrpara[32].Value = txtactualL.Text.Trim();
            _arrpara[33].Value = txtactualW.Text.Trim();
            _arrpara[34].Value = TDQaname.Visible == false ? "" : (DDQaname.SelectedIndex > 0 ? DDQaname.SelectedItem.Text : "");
            #region
            //            DataSet DsNew1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"Select IsNull(Sum(TDS),0) TDS From Empinfo EI,TDS_MASTER TM 
            //                    Where EI.MasterCompanyId=" + Session["varCompanyId"] + " And  EI.TypeId=TM.TypeID And EI.Empid=" + _arrpara[1].Value + @" And FromDate<='" + _arrpara[2].Value + "' And (EndDate is Null Or EndDate>'" + _arrpara[2].Value + "')");
            //            if (DsNew1.Tables[0].Rows.Count > 0)
            //            {
            //                _arrpara[27].Value = Convert.ToDouble(DsNew1.Tables[0].Rows[0]["TDS"]);
            //            }
            //            if (num == 1)
            //            {
            //                //Select Process_Rec_Id,Empid,ReceiveDate,Unitid,Userid,ChallanNo,Companyid,Remarks from PROCESS_RECEIVE_MASTER_1
            //                //Select Process_Rec_Detail_Id,Process_Rec_Id,Item_Finished_id,Length,Width,Area,Rate,Amount,Qty,Weight,Comm,CommAmt,IssueOrderid,Issue_Detail_Id,Orderid from PROCESS_RECEIVE_DETAIL_1

            //                string str = @"Update MasterSetting Set Process_Rec_Id =" + _arrpara[0].Value;
            //                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            //                str = @"Insert Into PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + "(Process_Rec_Id,Empid,ReceiveDate,Unitid,Userid,ChallanNo,Companyid,Remarks,CalType) Values (" + _arrpara[0].Value + "," + _arrpara[1].Value + ",'" + _arrpara[2].Value + "'," + _arrpara[3].Value + "," + _arrpara[4].Value + ",'" + _arrpara[5].Value + "'," + _arrpara[6].Value + ",'" + _arrpara[7].Value + "'," + _arrpara[25].Value + ")";
            //                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            //            }
            //            string str1 = @"Insert Into PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + "(Process_Rec_Detail_Id,Process_Rec_Id,Item_Finished_id,Length,Width,Area,Rate,Amount,Qty,Weight,Comm,CommAmt,IssueOrderid,Issue_Detail_Id,Orderid,Penality,PRemarks,QualityType,GatePassNo,FlagFixOrWeight,TDSPercentage) values(" + _arrpara[8].Value + "," + _arrpara[0].Value + "," + _arrpara[9].Value + ",'" + _arrpara[10].Value + "','" + _arrpara[11].Value + "'," + _arrpara[12].Value + "," + _arrpara[13].Value + "," + _arrpara[14].Value + "," + _arrpara[15].Value + "," + _arrpara[16].Value + "," + _arrpara[17].Value + "," + _arrpara[18].Value + "," + _arrpara[19].Value + "," + _arrpara[20].Value + "," + _arrpara[21].Value + "," + _arrpara[22].Value + ",'" + _arrpara[24].Value + "'," + _arrpara[23].Value + ",0," + _arrpara[26].Value + "," + _arrpara[27].Value + ")";
            //            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);

            //---------------------Update Process_Issue_Detail Table----------------------
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
            //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_FirstProcessReceive]", _arrpara);
            if (_arrpara[31].Value.ToString() != "")
            {
                llMessageBox.Visible = true;
                llMessageBox.Text = _arrpara[31].Value.ToString();
            }
            else
            {

                UtilityModule.Insert_Into_Carpet_NumberAndProcess_StockDetailwithProc(VarFinishedid, VarOrderid, Convert.ToInt32(TxtRecQty.Text), TxtPrefix.Text, Convert.ToInt32(TxtPostfix.Text), Convert.ToInt32(DDCompanyName.SelectedValue), Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[8].Value), TxtRecDate.Text, Tran, Convert.ToInt32(DDProcessName.SelectedValue), Convert.ToInt32(_arrpara[20].Value), Convert.ToInt32(_arrpara[4].Value));
                UtilityModule.PROCESS_RECEIVE_CONSUMPTION(Convert.ToInt32(_arrpara[8].Value), Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[9].Value), Convert.ToInt32(DDProcessName.SelectedValue), (Convert.ToDouble(_arrpara[12].Value) * Convert.ToDouble(_arrpara[15].Value)), Convert.ToDouble(_arrpara[16].Value), Convert.ToInt32(DDunit.SelectedValue), Convert.ToInt32(_arrpara[20].Value), Convert.ToInt32(_arrpara[19].Value), Tran, VarFlagFixOrWeight, Convert.ToInt32(_arrpara[15].Value), Convert.ToInt32(_arrpara[25].Value));

                llMessageBox.Visible = true;
                QCSAVE(Tran, Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[8].Value), tablename);
                llMessageBox.Text = "Data Successfully Saved.......";
                ViewState["Process_Rec_Id"] = _arrpara[0].Value.ToString();
                TxtRecDate.Enabled = false;
                Tran.Commit();
                TxtPostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix((TxtPrefix.Text).ToUpper()));
                ClearAfterSave();

            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceive.aspx");
            Tran.Rollback();
            llMessageBox.Visible = true;
            llMessageBox.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void ClearAfterSave()
    {
        DDDescription.SelectedIndex = 0;
        TxtIssuQty.Text = "";
        TxtPQty.Text = "";
        Txtlength.Text = "";
        TxtWidth.Text = "";
        TxtArea.Text = "";
        TxtRate.Text = "";
        TxtRecQty.Text = "";
        TxtWeight.Text = "";
        TxtPEnality.Text = "";
        TxtCommission.Text = "";
        fillorderdetail();
        DDDescription.Focus();
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
        string View = "ViewFindFinishedidItemidQDCSS";
        if (variable.VarNewQualitySize == "1")
        {
            View = "ViewFindFinishedidItemidQDCSSNew";
        }


        sqlstr = @"Select Process_Rec_Detail_Id as Sr_No,ICM.Category_Name as Category,IM.Item_Name as Item,IPM.QDCS + Space(5) + 
                        Case When PM.Unitid=1 Then SizeMtr Else case when PM.unitid=6 Then Sizeinch Else SizeFt End End Description,Length,Width,Qty,Rate,Qty*Area Area,Amount,Weight,Penality,
                        case When " + Session["varcompanyId"] + @"=9 Then '' Else [dbo].[F_GetstockNo_RecDetailWise](PD.Process_Rec_Id,PD.Process_Rec_Detail_Id) End TStockNo,PD.Premarks,PD.Comm,
                        PD.ActualLength,PD.ActualWidth,IM.Item_id,ICM.Category_id,isnull(PD.Qapersonname,'') as QaName
                        From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD," + View + @" IPM,
                        Item_Master IM,ITEM_CATEGORY_MASTER ICM Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=IPM.Finishedid And 
                        IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
                        PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " And PD.issueorderid=" + DDPONo.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
            //llMessageBox.Visible = false;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceive.aspx");
            llMessageBox.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

        return DS;
    }
    protected void DGRec_SelectedIndexChanged(object sender, EventArgs e)
    {
        //fill_Detail();
    }
    private void fill_Detail()
    {
        DataSet DS1 = null;
        DataSet DS = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string strsql = @"Select * From Process_Stock_Detail Where StockNo not in (Select StockNo From Process_Stock_Detail Where FromProcessId=0 And 
                    ReceiveDetailId=" + DGRec.SelectedValue + ") And ReceiveDetailId=" + DGRec.SelectedValue;
            DS1 = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (DS1.Tables[0].Rows.Count > 0)
            {
                llMessageBox.Visible = true;
                llMessageBox.Text = "Used in another process...";
            }
            else
            {
                strsql = @"SELECT replace(convert(varchar(11),PRM.ReceiveDate,106), ' ','-') as ReceiveDate,PID.Issue_Detail_Id, PRM.UnitId, PRM.ChallanNo, PRM.Remarks, PRD.Length, PRD.Width, PRD.Area, PRD.Rate, PRD.Qty AS RQty, PRD.Weight, PRD.IssueOrderId, PRD.Penality, (PID.PQty+PRD.Qty)PQty, PID.Qty AS IQty, PRD.Item_Finished_Id, IM.ITEM_ID, IM.CATEGORY_ID,PRD.Comm
                            FROM ITEM_MASTER IM INNER JOIN	ITEM_PARAMETER_MASTER IPM ON IM.ITEM_ID = IPM.ITEM_ID INNER JOIN PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @"  PRD 
                            INNER JOIN PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PRM ON PRD.Process_Rec_Id = PRM.Process_Rec_Id INNER JOIN 
                            PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PID ON PRD.Issue_Detail_Id = PID.Issue_Detail_Id ON IPM.ITEM_FINISHED_ID = PRD.Item_Finished_Id 
                            Where PRD.Process_Rec_Detail_Id=" + DGRec.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
                DS = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
                if (DS.Tables[0].Rows.Count > 0)
                {
                    BtnSave.Visible = false;
                    BtnUpdate.Visible = true;
                    TxtRecDate.Text = DS.Tables[0].Rows[0]["ReceiveDate"].ToString();
                    DDunit.SelectedValue = DS.Tables[0].Rows[0]["UnitId"].ToString();
                    DDCategoryName.SelectedValue = DS.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                    ddcategorychange();
                    DDItemName.SelectedValue = DS.Tables[0].Rows[0]["ITEM_ID"].ToString();
                    Item_SelectedIndexChange();
                    DDDescription.SelectedValue = DS.Tables[0].Rows[0]["Issue_Detail_Id"].ToString();
                    TxtPQty.Text = DS.Tables[0].Rows[0]["PQty"].ToString();
                    TxtCommission.Text = DS.Tables[0].Rows[0]["Comm"].ToString();
                    Txtlength.Text = DS.Tables[0].Rows[0]["Length"].ToString();
                    TxtWidth.Text = DS.Tables[0].Rows[0]["Width"].ToString();
                    TxtArea.Text = DS.Tables[0].Rows[0]["Area"].ToString();
                    TxtRate.Text = DS.Tables[0].Rows[0]["Rate"].ToString();
                    TxtIssuQty.Text = DS.Tables[0].Rows[0]["IQty"].ToString();
                    TxtRecQty.Text = DS.Tables[0].Rows[0]["RQty"].ToString();
                    Session["RQty"] = TxtRecQty.Text;
                    TxtWeight.Text = DS.Tables[0].Rows[0]["Weight"].ToString();
                    TxtPEnality.Text = DS.Tables[0].Rows[0]["Penality"].ToString();
                    TxtRemarks.Text = DS.Tables[0].Rows[0]["Remarks"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceive.aspx");
            llMessageBox.Visible = true;
            llMessageBox.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void DGRec_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGRec, "Select$" + e.Row.RowIndex);
        }
    }
    protected void TxtRecQty_TextChanged(object sender, EventArgs e)
    {
        llMessageBox.Visible = false;
        if (TxtRecQty.Text != "")
        {
            //if (Session["VarCompanyId"].ToString() != "9")
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
    protected void DDReceiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Process_Rec_Id"] = DDReceiveNo.SelectedValue;
        if (Session["varcompanyno"].ToString() != "7")
        {
            DDcaltype.SelectedValue = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select CalType From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + "  Where Process_Rec_Id=" + DDReceiveNo.SelectedValue).ToString();
        }
        Fill_Grid();
    }
    protected void ChkEdit_CheckedChanged(object sender, EventArgs e)
    {
        ChkEditChanged();
    }
    private void ChkEditChanged()
    {
        if (ChkEdit.Checked == true)
        {
            DDReceiveNo.Visible = true;
        }
        else
        {
            DDReceiveNo.Visible = false;
        }
    }
    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        if (llMessageBox.Text == "")
        {
            Update();
            Fill_Grid();
        }
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
                TxtPrefix.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Item_Code from Item_Master Where Item_Id=" + DDItemName.SelectedValue).ToString() + " And MasterCompanyId=" + Session["varCompanyId"];
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
        if (Session["varcompanyId"].ToString() == "9")
        {
            if (chkForSlip.Checked == true)
            {
                if (variable.VarNewQualitySize == "1")
                {
                    qry = @"Select CI.CompanyName,CI.CompAddr1,CI.CompAddr2,
                 Sum((PD.Area*PD.Qty)) Area,Sum(PD.Qty) As Qty,Sum(PD.Weight) As Weight,EI.EmpName,EI.Address,Vf.DesignName,VF.ColorName,case When PM.unitId=1 Then vf.Sizemtr Else Case When PM.UnitId=2 Then vf.Sizeft Else case When PM.UnitId=6 Then  Sizeinch Else Sizeft End End End As Size,
                 OM.LocalOrder,PM.ChallanNo,PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,Sum(PD.Amount) As Amount,PD.IssueOrderid,
                 U.UnitName,PM.UnitId," + DDProcessName.SelectedValue + " PROCESSID,Penality,PRemarks,CI.Gstno,EI.Gstno as EmpGstno From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,OrderMaster OM,
                 CompanyInfo CI,EmpInfo EI,Unit U,V_finishedItemDetailNew vf Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.OrderId=OM.OrderId And PD.Item_Finished_Id=vf.Item_Finished_id And 
                 PM.Companyid=CI.CompanyId And PM.Empid=EI.EmpId And PM.UnitId=U.UnitId And QualityType<>3 and 
                 PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + "  And PM.CompanyId=" + DDCompanyName.SelectedValue + @"
                 group by CI.CompanyName,CI.CompAddr1,CI.CompAddr2,EI.EmpName,EI.Address,Vf.DesignName,VF.ColorName,
                 U.UnitName,PM.UnitId,Penality,PRemarks,OM.LocalOrder,PM.ChallanNo,PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,sizemtr,sizeft,sizeinch,PD.IssueOrderId,CI.Gstno,EI.Gstno";
                }
                else
                {
                    qry = @"Select CI.CompanyName,CI.CompAddr1,CI.CompAddr2,
                 Sum((PD.Area*PD.Qty)) Area,Sum(PD.Qty) As Qty,Sum(PD.Weight) As Weight,EI.EmpName,EI.Address,Vf.DesignName,VF.ColorName,case When PM.unitId=1 Then vf.Sizemtr Else Case When PM.UnitId=2 Then vf.Sizeft Else case When PM.UnitId=6 Then  Sizeinch Else Sizeft End End End As Size,
                 OM.LocalOrder,PM.ChallanNo,PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,Sum(PD.Amount) As Amount,PD.IssueOrderid,
                 U.UnitName,PM.UnitId," + DDProcessName.SelectedValue + " PROCESSID,Penality,PRemarks,CI.Gstno,EI.Gstno From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,OrderMaster OM,
                 CompanyInfo CI,EmpInfo EI,Unit U,V_finishedItemDetail vf Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.OrderId=OM.OrderId And PD.Item_Finished_Id=vf.Item_Finished_id And 
                 PM.Companyid=CI.CompanyId And PM.Empid=EI.EmpId And PM.UnitId=U.UnitId And QualityType<>3 and 
                 PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + "  And PM.CompanyId=" + DDCompanyName.SelectedValue + @"
                 group by CI.CompanyName,CI.CompAddr1,CI.CompAddr2,EI.EmpName,EI.Address,Vf.DesignName,VF.ColorName,
                 U.UnitName,PM.UnitId,Penality,PRemarks,OM.LocalOrder,PM.ChallanNo,PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,sizemtr,sizeft,sizeinch,PD.IssueOrderId,CI.Gstno,EI.Gstno";
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
                    (Select * from [dbo].[Get_StockNoRec_Detail_Wise](PD.Process_Rec_Id,PD.Process_Rec_Detail_Id)) StockNo," + DDProcessName.SelectedValue + @" PROCESSID,Penality,PRemarks,CI.Gstno,EI.Gstno as Empgstno,Upper('" + DDProcessName.SelectedItem.Text + @"') as ProcessName,isnull(pd.Qapersonname,'') as Qaname 
                    ," + Session["varCompanyId"] + @" as MasterCompanyId,isnull(NU.UserName,'') as UserName,isnull(PM.PartyChallanNo,'') as PartyChallanNo
                    From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM(NoLock),PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD(NoLock),ViewFindFinishedidItemidQDCSSNew IPM(NoLock),
                    Item_Master IM(NoLock),ITEM_CATEGORY_MASTER ICM(NoLock),CompanyInfo CI(NoLock),EmpInfo EI(NoLock),Unit U(NoLock),NewUserDetail NU(NoLock) 
                    Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=IPM.Finishedid And 
                    IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And PM.Companyid=CI.CompanyId And PM.Empid=EI.EmpId And PM.UnitId=U.UnitId And QualityType<>3 
                    and  NU.UserId=PM.USERID and PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " And IM.MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                qry = @"Select ICM.Category_Name,(PD.Area*PD.Qty) Area,PD.Qty,PD.Weight,CI.CompanyName,CI.CompAddr1,CI.CompAddr2,IM.Item_Name,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Description,
                    PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,PD.Amount,PD.IssueOrderid,EI.EmpName,EI.Address,U.UnitName,PM.UnitId,PM.ChallanNo,
                    (Select * from [dbo].[Get_StockNoRec_Detail_Wise](PD.Process_Rec_Id,PD.Process_Rec_Detail_Id)) StockNo," + DDProcessName.SelectedValue + @" PROCESSID,Penality,PRemarks,CI.Gstno,EI.Gstno as Empgstno,Upper('" + DDProcessName.SelectedItem.Text + @"') as ProcessName,isnull(pd.Qapersonname,'') as Qaname 
                    ," + Session["varCompanyId"] + @" as MasterCompanyId,isnull(NU.UserName,'') as UserName,isnull(PM.PartyChallanNo,'') as PartyChallanNo
                    From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM(NoLock),PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD(NoLock),ViewFindFinishedidItemidQDCSS IPM(NoLock),
                    Item_Master IM(NoLock),ITEM_CATEGORY_MASTER ICM(NoLock),CompanyInfo CI(NoLock),EmpInfo EI(NoLock),Unit U(NoLock) ,NewUserDetail NU(NoLock) 
                    Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=IPM.Finishedid And 
                    IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And PM.Companyid=CI.CompanyId And PM.Empid=EI.EmpId And PM.UnitId=U.UnitId And QualityType<>3 
                    and NU.UserId=PM.USERID and PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " and PD.issueorderid=" + DDPONo.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
            }
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Session["varcompanyId"].ToString() == "9")
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
                (Select * from [dbo].[Get_StockNoRec_Detail_Wise](PD.Process_Rec_Id,PD.Process_Rec_Detail_Id)) StockNo," + DDProcessName.SelectedValue + @" PROCESSID,PD.GatePassNo 
                 ,PD.IssueOrderid as FolioChallanNo
                From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,ViewFindFinishedidItemidQDCSSNew IPM,
                Item_Master IM,ITEM_CATEGORY_MASTER ICM,CompanyInfo CI,EmpInfo EI,Unit U Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=IPM.Finishedid And 
                IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And PM.Companyid=CI.CompanyId And PM.Empid=EI.EmpId And PM.UnitId=U.UnitId And QualityType=3 and 
                PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " And IM.MasterCompanyId=" + Session["varCompanyId"];
        }
        else
        {
            qry = @"Select ICM.Category_Name,(PD.Area*PD.Qty) Area,PD.Qty,PD.Weight,CI.CompanyName,CI.CompAddr1,CI.CompAddr2,IM.Item_Name,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Description,
                 Replace(Convert(varchar(11),PM.ReceiveDate,106),' ','-') as ReceiveDate,PD.Length,PD.Width,PD.Rate,PD.Amount,PD.IssueOrderid,EI.EmpName,EI.Address,U.UnitName,PM.UnitId,PM.ChallanNo,
                (Select * from [dbo].[Get_StockNoRec_Detail_Wise](PD.Process_Rec_Id,PD.Process_Rec_Detail_Id)) StockNo," + DDProcessName.SelectedValue + @" PROCESSID,PD.GatePassNo 
                ,(Select ChallanNo from Process_Issue_master_1 Where IssueOrderId=PD.IssueOrderid) as FolioChallanNo
                From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,ViewFindFinishedidItemidQDCSS IPM,
                Item_Master IM,ITEM_CATEGORY_MASTER ICM,CompanyInfo CI,EmpInfo EI,Unit U Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=IPM.Finishedid And 
                IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And PM.Companyid=CI.CompanyId And PM.Empid=EI.EmpId And PM.UnitId=U.UnitId And QualityType=3 and 
                PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " And IM.MasterCompanyId=" + Session["varCompanyId"];
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
    protected void DeleteRow(int VarProcess_Rec_Detail_Id)
    {
        llMessageBox.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //int VarProcess_Rec_Detail_Id = Convert.ToInt32(DGRec.DataKeys[e.RowIndex].Value);
            SqlParameter[] _array = new SqlParameter[5];
            _array[0] = new SqlParameter("@ProcessId", SqlDbType.Int);
            _array[1] = new SqlParameter("@ReceiveDetailId", SqlDbType.Int);
            _array[2] = new SqlParameter("@RowCount", SqlDbType.Int);
            _array[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            _array[3].Direction = ParameterDirection.Output;
            _array[4] = new SqlParameter("@UserID", SqlDbType.Int);
            _array[0].Value = DDProcessName.SelectedValue;
            _array[1].Value = VarProcess_Rec_Detail_Id;
            _array[2].Value = DGRec.Rows.Count;
            _array[4].Value = Session["varuserid"];
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteProductionReceiveDetail", _array);
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('" + _array[3].Value.ToString() + "');", true);

            #region
            //if (DGRec.Rows.Count == 1)
            //{
            //    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "DELETE PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " Where Process_Rec_ID in (Select Process_Rec_ID from PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + " Where Process_Rec_Detail_Id=" + VarProcess_Rec_Detail_Id + ")");
            //}
            //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "UPDATE PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " Set PQty=PQty+PRD.Qty From PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + " PRD Where PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + ".Issue_Detail_Id=PRD.Issue_Detail_Id And Process_Rec_Detail_Id=" + VarProcess_Rec_Detail_Id + "");
            //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "DELETE PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + " Where Process_Rec_Detail_Id=" + VarProcess_Rec_Detail_Id + "");
            //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "DELETE Process_Stock_Detail Where ReceiveDetailId=" + VarProcess_Rec_Detail_Id + "");
            //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "DELETE CarpetNumber Where Process_Rec_Detail_Id=" + VarProcess_Rec_Detail_Id + "");
            //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "DELETE PROCESS_RECEIVE_CONSUMPTION Where Process_Rec_Detail_Id=" + VarProcess_Rec_Detail_Id + "");
            //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "DELETE QCDetail Where RefName='PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + "' and RecieveDetailID=" + VarProcess_Rec_Detail_Id + "");
            #endregion

            Tran.Commit();
            if (DGRec.Rows.Count == 1)
            {
                EmployeeSelectedChange();
            }
            Fill_Grid();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceive.aspx");
            Tran.Rollback();
            llMessageBox.Visible = true;
            llMessageBox.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DGRec_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        VarProcess_Rec_Detail_Id = Convert.ToInt32(DGRec.DataKeys[e.RowIndex].Value);
        btnclickflag = "";

        btnclickflag = "BtnDeleteRow";
        txtpwd.Focus();
        Popup(true);        
    }
    protected void TxtPostfix_TextChanged(object sender, EventArgs e)
    {
        llMessageBox.Text = "";
        string TStockNo = TxtPrefix.Text + TxtPostfix.Text;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from CarpetNumber Where TStockNo='" + TStockNo + "'");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            llMessageBox.Text = "Stock No AllReady Exits....";
            TxtPostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix((TxtPrefix.Text).ToUpper()));
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
        string view = "V_FinishedItemDetail";
        if (variable.VarNewQualitySize == "1")
        {
            view = "V_FinishedItemDetailNew";
        }

        sql = @"Select CATEGORY_NAME+'  '+ Item_Name+'  '+QualityName+'  '+Designname+'  '+ColorName+'  '+ShapeName+'  '+Case When UnitId=1 Then SizeMtr Else SizeFt End+'  '+ShadeColorName As Description,
                       IsNull(Sum(PID.Qty),0)-Isnull(Sum(CancelQty),0) As Qty,isnull(sum(pid.PQty),0)-Isnull(Sum(RejectQty),0)-isnull(Sum(CancelQty),0) as issueqqty,CATEGORY_ID,v.ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId,
                       PIM.UnitId UNIT,PID.Item_Finished_Id as finishedid,Issue_Detail_Id From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PIM,
                       PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + @" PID," + view + @" V Where PIM.IssueOrderId=PID.IssueOrderId And 
                       pid.item_finished_id=V.Item_Finished_Id And pid.issueorderid=" + DDPONo.SelectedValue + " and PIM.empid=" + DDEmployeeNamee.SelectedValue + " And V.MasterCompanyId=" + Session["varCompanyId"] + @" Group By CATEGORY_NAME,Item_Name,QualityName,Designname,
                       ColorName,ShadeColorName,ShapeName,SizeMtr,SizeFt,UnitId,CATEGORY_ID,V.ITEM_ID,QualityId,ColorId,DesignId,SizeId,ShapeId,ShadecolorId,PID.Item_Finished_Id,Issue_Detail_Id Having isnull(sum(pid.PQty),0)>0";


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
    //protected void dgorder_RowCreated(object sender, GridViewRowEventArgs e)
    //{
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
    protected void DGRec_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGRec, "Select$" + e.Row.RowIndex);
            //*****************
            for (int i = 0; i < DGRec.Columns.Count; i++)
            {
                if (DGRec.Columns[i].HeaderText == "QCCHECK")
                {
                    if (variable.VarQctype == "1")
                    {
                        DGRec.Columns[i].Visible = true;
                    }
                    else
                    {
                        DGRec.Columns[i].Visible = false;
                    }
                }
            }
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
            trprifix.Visible = false;
            tdcalname.Visible = false;
            tdcaltype.Visible = false;
            TDcomm.Visible = false;
        }
    }
    private void ParameteLabel()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
    }
    protected void BtnCurrentConsumption_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            if (DGRec.Rows.Count > 0)
            {
                for (int i = 0; i < DGRec.Rows.Count; i++)
                {
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete PROCESS_RECEIVE_CONSUMPTION Where Processid=" + DDProcessName.SelectedValue + " And Process_Rec_Detail_Id=" + DGRec.DataKeys[i].Value);

                    string Str = @"Select PM.UnitId,PM.CalType,PD.Process_Rec_Detail_Id,PD.Process_Rec_Id,PD.Item_Finished_Id,PD.Qty*PD.Area Area,PD.Qty,PD.Weight,
                                 PD.IssueOrderId,PD.Issue_Detail_Id,PD.OrderId,PD.FlagFixOrWeight From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                                 PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + " PD Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Process_Rec_Detail_Id=" + DGRec.DataKeys[i].Value;

                    DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                    //{
                    //    UtilityModule.PROCESS_RECEIVE_CONSUMPTION(Convert.ToInt32(Ds.Tables[0].Rows[0]["Process_Rec_Detail_Id"]), Convert.ToInt32(Ds.Tables[0].Rows[0]["Process_Rec_Id"]), Convert.ToInt32(Ds.Tables[0].Rows[0]["Item_Finished_Id"]), Convert.ToInt32(DDProcessName.SelectedValue), Convert.ToDouble(Ds.Tables[0].Rows[0]["Area"]), Convert.ToDouble(Ds.Tables[0].Rows[0]["Weight"]), Convert.ToInt32(Ds.Tables[0].Rows[0]["UnitId"]), Convert.ToInt32(Ds.Tables[0].Rows[0]["Issue_Detail_Id"]), Convert.ToInt32(Ds.Tables[0].Rows[0]["IssueOrderId"]), Tran, Convert.ToInt32(Ds.Tables[0].Rows[0]["FlagFixOrWeight"]), Convert.ToInt32(Ds.Tables[0].Rows[0]["Qty"]), Convert.ToInt32(Ds.Tables[0].Rows[0]["CalType"]));
                    //}
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        //**************************Current consumption
                        SqlParameter[] param = new SqlParameter[12];
                        param[0] = new SqlParameter("@Process_Rec_Id", Ds.Tables[0].Rows[0]["Process_Rec_Id"]);
                        param[1] = new SqlParameter("@Process_Rec_Detail_Id", Ds.Tables[0].Rows[0]["Process_Rec_Detail_Id"]);
                        param[2] = new SqlParameter("@Finishedid", Ds.Tables[0].Rows[0]["Item_Finished_Id"]);
                        param[3] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
                        param[4] = new SqlParameter("@Area", Ds.Tables[0].Rows[0]["Area"]);
                        param[5] = new SqlParameter("@Weight", Ds.Tables[0].Rows[0]["Weight"]);
                        param[6] = new SqlParameter("@UnitID", Ds.Tables[0].Rows[0]["Unitid"]);
                        param[7] = new SqlParameter("@Issue_Order_Id", Ds.Tables[0].Rows[0]["Issueorderid"]);
                        param[8] = new SqlParameter("@Issue_Detail_Id", Ds.Tables[0].Rows[0]["Issue_Detail_id"]);
                        param[9] = new SqlParameter("@FlagFixOrWeight", Ds.Tables[0].Rows[0]["FlagFixOrWeight"]);
                        param[10] = new SqlParameter("@Qty", Ds.Tables[0].Rows[0]["Qty"]);
                        param[11] = new SqlParameter("@CalType", Ds.Tables[0].Rows[0]["CalType"]);

                        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_Process_ReceiveConsumption]", param);
                    }
                    //**************************
                }
                Tran.Commit();
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Consumption successfully changed...');", true);
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessReceive.aspx");
            Tran.Rollback();
            llMessageBox.Visible = true;
            llMessageBox.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void BtnPreviewWithConsmp_Click(object sender, EventArgs e)
    {
        string qry = "";
        if (variable.VarNewQualitySize == "1")
        {
            qry = @"Select ICM.Category_Name,(PD.Area*PD.Qty) Area,PD.Qty,PD.Weight,CI.CompanyName,CI.CompAddr1,CI.CompAddr2,IM.Item_Name,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Description,
        PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,PD.Amount,PD.IssueOrderid,EI.EmpName,EI.Address,U.UnitName,PM.UnitId,PM.ChallanNo,
        (Select * from [dbo].[Get_StockNoRec_Detail_Wise](PD.Process_Rec_Id,PD.Process_Rec_Detail_Id)) StockNo," + DDProcessName.SelectedValue + @" PROCESSID,PD.Process_Rec_Detail_Id,PD.Process_Rec_Id From 
        PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,ViewFindFinishedidItemidQDCSSNew IPM,
        Item_Master IM,ITEM_CATEGORY_MASTER ICM,CompanyInfo CI,EmpInfo EI,Unit U Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=IPM.Finishedid And 
        IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And PM.Companyid=CI.CompanyId And PM.Empid=EI.EmpId And PM.UnitId=U.UnitId And QualityType<>3 and 
        PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " And IM.MasterCompanyId=" + Session["varCompanyId"];
        }
        else
        {
            qry = @"Select ICM.Category_Name,(PD.Area*PD.Qty) Area,PD.Qty,PD.Weight,CI.CompanyName,CI.CompAddr1,CI.CompAddr2,IM.Item_Name,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Description,
        PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,PD.Amount,PD.IssueOrderid,EI.EmpName,EI.Address,U.UnitName,PM.UnitId,PM.ChallanNo,
        (Select * from [dbo].[Get_StockNoRec_Detail_Wise](PD.Process_Rec_Id,PD.Process_Rec_Detail_Id)) StockNo," + DDProcessName.SelectedValue + @" PROCESSID,PD.Process_Rec_Detail_Id,PD.Process_Rec_Id From 
        PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,ViewFindFinishedidItemidQDCSS IPM,
        Item_Master IM,ITEM_CATEGORY_MASTER ICM,CompanyInfo CI,EmpInfo EI,Unit U Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=IPM.Finishedid And 
        IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And PM.Companyid=CI.CompanyId And PM.Empid=EI.EmpId And PM.UnitId=U.UnitId And QualityType<>3 and 
        PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " And IM.MasterCompanyId=" + Session["varCompanyId"];
        }

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);

        if (variable.VarNewQualitySize == "1")
        {
            qry = @"Select ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,SizeMtr,Area Area,Weight,IFinishedid,
                Round(Qty,3) MTConmp,Round(Qty*Area,3) ConsmpNew,TConsmp,Rate,Loss,IssueOrderId,TLoss,RecQty,Process_Rec_Detail_Id,Process_Rec_Id 
                From PROCESS_RECEIVE_CONSUMPTION PRC,V_FinishedItemDetailNew VF Where PRC.IFinishedid=VF.Item_Finished_id And PRC.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        }
        else
        {
            qry = @"Select ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,SizeMtr,Area Area,Weight,IFinishedid,
                Round(Qty,3) MTConmp,Round(Qty*Area,3) ConsmpNew,TConsmp,Rate,Loss,IssueOrderId,TLoss,RecQty,Process_Rec_Detail_Id,Process_Rec_Id 
                From PROCESS_RECEIVE_CONSUMPTION PRC,V_FinishedItemDetail VF Where PRC.IFinishedid=VF.Item_Finished_id And PRC.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        }
        SqlDataAdapter adp = new SqlDataAdapter(qry, ErpGlobal.DBCONNECTIONSTRING);
        DataTable dt2 = new DataTable();
        adp.Fill(dt2);
        ds.Tables.Add(dt2);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\WCarpetRecvWithConsumptionNew.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\WCarpetRecvNew.xsd";
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

    protected void btnqcchkpreview_Click(object sender, EventArgs e)
    {
        reportQcheck();
    }
    private void reportQcheck()
    {

        DataSet ds = new DataSet();
        int Issueorderid = Convert.ToInt32(DDPONo.SelectedValue);

        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@Processrecid", ViewState["Process_Rec_Id"]);
        param[2] = new SqlParameter("@issueorderid", Issueorderid);
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetQcreportBazar", param);
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
    //    private void reportQcheck()
    //    {
    //        string SName = "";
    //        string QCValue = "";
    //        string qry = "";
    //        DataSet ds = new DataSet();
    //        string view = "ViewFindFinishedidItemidQDCSS";
    //        if (variable.VarNewQualitySize == "1")
    //        {
    //            view = "ViewFindFinishedidItemidQDCSS";
    //        }

    //        qry = @"Select ICM.Category_Name,PD.Qty*PD.Area Area,PD.Qty,PD.Weight,CI.CompanyName,CI.CompAddr1,CI.CompAddr2,IM.Item_Name,IPM.QDCS + Space(5) + Width+'x'+Length Description,
    //                PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,PD.Amount,PD.IssueOrderid,EI.EmpName,EI.Address,U.UnitName,(Select ShortName From PROCESS_NAME_MASTER Where PROCESS_NAME_ID=" + DDProcessName.SelectedValue + @") ShortName,
    //                PM.UnitId,PM.ChallanNo,(Select * from [dbo].[Get_StockNoRec_Detail_Wise](PD.Process_Rec_Id,PD.Process_Rec_Detail_Id)) StockNo,PM.Process_Rec_Id,PD.Process_Rec_Detail_Id,PD.ActualWidth,PD.ActualLength,
    //                dbo.F_GETQCValue('PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + "'," + DDProcessName.SelectedValue + @",PM.Process_rec_id,PD.Process_Rec_Detail_Id) as QCVALUE,
    //                dbo.F_GETQCParameterValue('PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + "'," + DDProcessName.SelectedValue + @",PM.Process_rec_id) as QCPARAMETER
    //                From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD," + view + @" IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM,CompanyInfo CI,
    //                EmpInfo EI,Unit U Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And 
    //                PM.Companyid=CI.CompanyId And PM.EmpId=EI.EmpId And PM.UnitId=U.UnitId And IM.Category_Id=ICM.Category_Id And PD.QualityType<>3 And PM.Process_Rec_Id=" + ViewState["Process_Rec_Id"] + " and PD.issueorderid="+DDPONo.SelectedValue +"";

    //        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
    //        #region
    //        //      DataTable mytable = new DataTable();
    //        //        mytable.Columns.Add("PrtID", typeof(int));
    //        //        mytable.Columns.Add("SName", typeof(string));
    //        //        mytable.Columns.Add("QCValue", typeof(string));

    //        //        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
    //        //        {
    //        //            string str = @"Select SName, Case when QCValue='1' then 'YES' else 'NO' END QCValue from QCdetail QCD inner join QCMaster QCM ON 
    //        //                         QCD.QCMasterID=QCM.ID Inner Join QCParameter QCP ON QCM.ParaID=QCP.ParaID
    //        //                         Where RefName= 'PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + "' And ProcessId=" + DDProcessName.SelectedValue + " And QCD.RecieveID=" + ViewState["Process_Rec_Id"] + " And QCD.RecieveDetailID=" + ds.Tables[0].Rows[j]["Process_Rec_Detail_Id"];
    //        //            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //        //            SqlDataAdapter sda = new SqlDataAdapter(str, con);
    //        //            DataTable dt = new DataTable();
    //        //            sda.Fill(dt);
    //        //            for (int i = 0; i < dt.Rows.Count; i++)
    //        //            {
    //        //                if (SName == "" && QCValue == "")
    //        //                {
    //        //                    SName = dt.Rows[i]["SName"].ToString();
    //        //                    QCValue = dt.Rows[i]["QCValue"].ToString();
    //        //                }
    //        //                else
    //        //                {
    //        //                    SName = SName + ' ' + dt.Rows[i]["SName"].ToString();
    //        //                    QCValue = QCValue + ' ' + dt.Rows[i]["QCValue"].ToString();
    //        //                }
    //        //            }
    //        //            mytable.Rows.Add(ds.Tables[0].Rows[j]["Process_Rec_Detail_Id"], SName, QCValue);
    //        //            SName = "";
    //        //            QCValue = "";
    //        //        }
    //        //        ds.Tables.Add(mytable);
    //        #endregion
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            Session["rptFileName"] = "Reports/WCarpetRecvQCNew.rpt";
    //            Session["dsFileName"] = "~\\ReportSchema\\WCarpetRecvQCNew.xsd";
    //            Session["GetDataset"] = ds;
    //            StringBuilder stb = new StringBuilder();
    //            stb.Append("<script>");
    //            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
    //            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    //        }
    //        else
    //        {
    //            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
    //        }
    //    }    
    protected void DGRec_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGRec.EditIndex = e.NewEditIndex;
        Fill_Grid();
    }
    protected void DGRec_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGRec.EditIndex = -1;
        Fill_Grid();
    }
    protected void DGRec_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        btnclickflag = "";
        btnclickflag = "BtnUpdate";
        Popup(true);
        txtpwd.Focus();
        rowindex = e.RowIndex;
    }
    protected void Updatedetails(int rowindex)
    {
        llMessageBox.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int Processrecdetailid = Convert.ToInt32(DGRec.DataKeys[rowindex].Value);
            TextBox txtweightedit = (TextBox)DGRec.Rows[rowindex].FindControl("txtweightedit");
            TextBox txtpenalityedit = (TextBox)DGRec.Rows[rowindex].FindControl("txtpenalityedit");
            TextBox txtpremarksedit = (TextBox)DGRec.Rows[rowindex].FindControl("txtpremarksedit");
            TextBox txtrateedit = (TextBox)DGRec.Rows[rowindex].FindControl("txtrateedit");
            TextBox txtcommedit = (TextBox)DGRec.Rows[rowindex].FindControl("txtcommedit");
            TextBox txtgridactualL = (TextBox)DGRec.Rows[rowindex].FindControl("txtgridactualL");
            TextBox txtgridactualW = (TextBox)DGRec.Rows[rowindex].FindControl("txtgridactualW");
            TextBox txtqanameedit = (TextBox)DGRec.Rows[rowindex].FindControl("txtqanameedit");
            //*************
            SqlParameter[] param = new SqlParameter[13];
            param[0] = new SqlParameter("@Processrecdetailid", Processrecdetailid);
            param[1] = new SqlParameter("@weight", txtweightedit.Text == "" ? "0" : txtweightedit.Text);
            param[2] = new SqlParameter("@Penality", txtpenalityedit.Text == "" ? "0" : txtpenalityedit.Text);
            param[3] = new SqlParameter("@PRemarks", txtpremarksedit.Text);
            param[4] = new SqlParameter("@Processid", DDProcessName.SelectedValue);
            param[5] = new SqlParameter("@userid", Session["varuserid"]);
            param[6] = new SqlParameter("@MastercompanyId", Session["varcompanyId"]);
            param[7] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[7].Direction = ParameterDirection.Output;
            param[8] = new SqlParameter("@Rate", txtrateedit.Text == "" ? "0" : txtrateedit.Text);
            param[9] = new SqlParameter("@Comm", txtcommedit.Text == "" ? "0" : txtcommedit.Text);
            param[10] = new SqlParameter("@actualLength", txtgridactualL.Text.Trim());
            param[11] = new SqlParameter("@actualWidth", txtgridactualW.Text.Trim());
            param[12] = new SqlParameter("@QaName", txtqanameedit.Text.Trim());
           
            //**************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_updateBazarDetail", param);
            llMessageBox.Visible = true;
            llMessageBox.Text = param[7].Value.ToString();
            Tran.Commit();
            DGRec.EditIndex = -1;
            Fill_Grid();
        }
        catch (Exception ex)
        {
            llMessageBox.Visible = true;
            llMessageBox.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void lnkqccheck(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;

        if (lnk != null)
        {
            GridViewRow grv = lnk.NamingContainer as GridViewRow;
            Gridindex = grv.RowIndex;
            int Processrecdetailid = Convert.ToInt32(DGRec.DataKeys[grv.RowIndex].Value);
            Label lblitemidgrid = (Label)grv.FindControl("lblitemidgrid");
            Label lblcategoryidgrid = (Label)grv.FindControl("lblcategoryidgrid");
            //***********Fill Grid quality
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select QCMaster.ID,SrNo,ParaName from QCParameter inner join QCMaster on QCParameter.ParaID=QCMaster.ParaID where CategoryID=" + lblcategoryidgrid.Text + " and ItemID=" + lblitemidgrid.Text + " and ProcessID=" + DDProcessName.SelectedValue + " order by SrNo");
            grdqualitychk.DataSource = ds;
            grdqualitychk.DataBind();
            DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select QCmasterid,QCvalue,isnull(Reason,'') as Reason From Qcdetail  Where RecieveID=" + DDReceiveNo.SelectedValue + " and RecieveDetailID=" + Processrecdetailid + " and RefName='PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + "'");
            int Qcmasterid = 0;
            if (ds1.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < grdqualitychk.Rows.Count; i++)
                {
                    Qcmasterid = Convert.ToInt32(grdqualitychk.DataKeys[i].Value);
                    for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                    {
                        if (Qcmasterid == Convert.ToInt32(ds1.Tables[0].Rows[j]["Qcmasterid"]))
                        {
                            CheckBox chk = (CheckBox)grdqualitychk.Rows[i].FindControl("CheckBox1");
                            chk.Checked = Convert.ToBoolean(ds1.Tables[0].Rows[j]["Qcvalue"]);
                            TextBox txtqcreason = (TextBox)grdqualitychk.Rows[i].FindControl("txtqcreason");
                            txtqcreason.Text = ds1.Tables[0].Rows[j]["reason"].ToString();
                        }
                    }
                }
            }
            //***********
            if (grdqualitychk.Rows.Count > 0)
            {
                Trupdateqcdetail.Visible = true;
            }
            else
            {
                Trupdateqcdetail.Visible = false;
            }
            //***********
        }
    }

    protected void lnkupdateqcdetail_Click(object sender, EventArgs e)
    {
        if (Gridindex >= 0)
        {
            llMessageBox.Text = "";
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();

            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                int Processrecdetailid = Convert.ToInt32(DGRec.DataKeys[Gridindex].Value);
                QCSAVE(Tran, Convert.ToInt32(DDReceiveNo.SelectedValue), Processrecdetailid, "Process_receive_detail_" + DDProcessName.SelectedValue);
                Tran.Commit();
                llMessageBox.Visible = true;
                llMessageBox.Text = "QC Detail updated successfully....";

            }
            catch (Exception ex)
            {
                Tran.Rollback();
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

    protected void btnCheck_Click(object sender, EventArgs e)
    {
        if (MySession.ProductionEditPwd == txtpwd.Text)
        {
            if (btnclickflag == "BtnUpdate")
            {              
                Updatedetails(rowindex);
            }           
            else if (btnclickflag == "BtnDeleteRow")
            {
                DeleteRow(VarProcess_Rec_Detail_Id);
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