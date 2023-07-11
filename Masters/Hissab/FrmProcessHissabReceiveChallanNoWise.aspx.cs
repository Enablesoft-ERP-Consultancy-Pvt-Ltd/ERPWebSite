using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Hissab_FrmProcessHissabReceiveChallanNoWise : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            string Str = "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + @" order by CompanyName
                        Select [Year] YearID, [Year] From YearData(Nolock) Order By [Year] Desc 
                         Delete TEMP_HISSAB_WISE_CONSUMPTION Where Userid=" + Session["varuserid"] + " And MasterCompanyId=" + Session["varCompanyId"];

            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, Ds, 0, true, "--SELECT--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
                CompanyNameSelectedIndexChanged();
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDYear, Ds, 1, false, "");

            //TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            //TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TDRadioButton.Visible = false;
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            CheckForEditSelectedChanges();
            ViewState["Hissab_No"] = 0;
            switch (Convert.ToInt16(Session["varcompanyId"]))
            {
                
                case 43: //for Carpet International
                    TDPoOrderNo.Visible = true;
                    TDsrno.Visible = false;
                    TDAdditionAmt.Visible = false;
                    TDDeductionAmt.Visible = false;                   
                    TDBonusAmt.Visible = false;
                    break;
                default:
                    TDPoOrderNo.Visible = true;
                    TDsrno.Visible = false;
                    TDAdditionAmt.Visible = false;
                    TDDeductionAmt.Visible = false;                   
                    TDBonusAmt.Visible = false;
                    break;
            }
        }
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanyNameSelectedIndexChanged();
    }

    private void CompanyNameSelectedIndexChanged()
    {
        string Str = "";
        if (ChkForEdit.Checked == true)
        {
            Str = @"Select Distinct PNM.Process_Name_Id, PNM.Process_Name 
            from PROCESS_HISSAB PH(Nolock) 
            JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.Process_Name_Id = PH.ProcessId And PNM.MasterCompanyId = " + Session["varCompanyId"] + @" 
            Where PH.CompanyID = " + DDCompanyName.SelectedValue + " And PH.YearID = " + DDYear.SelectedValue + " Order By PNM.Process_Name";
        }
        else
        {
            Str = "Select Process_Name_Id, Process_Name from PROCESS_NAME_MASTER(Nolock) Where MasterCompanyId=" + Session["varCompanyId"] + " Order By Process_Name";
            
        }
        UtilityModule.ConditionalComboFill(ref DDProcessName, Str, true, "--SELECT--");
        ViewState["Hissab_No"] = 0;
    }

    protected void FillSrno()
    {
        if (TDsrno.Visible == true)
        {
            if (DDPOOrderNo.SelectedIndex > 0)
            {
                UtilityModule.ConditionalComboFill(ref DDsrno, @"select Distinct OM.orderid,OM.LocalOrder From OrderMaster OM(NoLock) inner join Process_Issue_Detail_" + DDProcessName.SelectedValue + @" PID(NoLock) on OM.orderid=PID.orderid and PID.Issueorderid=" + DDPOOrderNo.SelectedValue + @"
                                                            Where OM.CompanyId=" + DDCompanyName.SelectedValue + " and OM.Status=0 order by OM.OrderId", true, "--Plz Select--");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDsrno, "select orderid,LocalOrder From OrderMaster(NoLock) Where CompanyId=" + DDCompanyName.SelectedValue + " and Status=0 order by OrderId", true, "--Plz Select--");
            }
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DGDetail.DataSource = null;
        DGDetail.DataBind();
        ProcessNameSelectedIndexChanged();
        FillSrno();
        ShowButton();
    }
    private void ProcessNameSelectedIndexChanged()
    {
        if (DDProcessName.SelectedIndex > 0)
        {
            string Str = "";
            if (ChkForEdit.Checked == true)
            {
                Str = @"Select Distinct EI.EmpId,EI.EmpName +case when isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as EmpName 
                From PROCESS_HISSAB PH(NoLock) 
                JOIN EmpInfo EI(NoLock) ON EI.EmpID = PH.EmpId And EI.Blacklist = 0 And EI.MasterCompanyId=" + Session["varcompanyId"] + @" 
                Where PH.CompanyId=" + DDCompanyName.SelectedValue + " And PH.CommPaymentFlag = 0  And PH.ProcessId=" + DDProcessName.SelectedValue + @" 
                And PH.YearID = " + DDYear.SelectedValue + @" Order By EmpName";
            }
            else
            {
                if (variable.VarFinishingNewModuleWise == "1")
                {
                    switch (Session["varcompanyId"].ToString())
                    {
                        case "28":
                        case "22":
                        case "43":
                            Str = @"SELECT EI.EMPID,EI.EMPNAME + CASE WHEN EI.EMPCODE<>'' THEN ' ['+ISNULL(EI.EMPCODE,'')+']' ELSE '' END EMPNAME FROM PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM(NoLock) INNER JOIN EMPINFO EI(NoLock) ON PIM.EMPID=EI.EMPID WHERE PIM.Companyid=" + DDCompanyName.SelectedValue + @" and  EI.Blacklist=0
                                UNION
                                SELECT EI.EMPID,EI.EMPNAME + CASE WHEN EI.EMPCODE<>'' THEN ' ['+ISNULL(EI.EMPCODE,'')+']' ELSE '' END EMPNAME FROM EMPLOYEE_PROCESSORDERNO EMP(NoLock) INNER JOIN PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM(NoLock)  ON EMP.ISSUEORDERID=PIM.ISSUEORDERID AND EMP.PROCESSID=" + DDProcessName.SelectedValue + @"
                                INNER JOIN EMPINFO EI(NoLock) ON EMP.EMPID=EI.EMPID WHere PIm.companyid=" + DDCompanyName.SelectedValue + " and EI.Blacklist=0 ORDER BY EMPNAME";

                            break;
                        default:
                            if (DDProcessName.SelectedValue == "1")
                            {
                                Str = "Select Distinct PM.EmpId,EI.EmpName +case when isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as EmpName From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM(NoLock),EMpInfo EI Where CompanyId=" + DDCompanyName.SelectedValue + " And PM.EmpId=EI.EmpId And EI.MasterCompanyId=" + Session["varCompanyId"] + " and EI.Blacklist=0 Order By EmpName";
                            }
                            else
                            {
                                Str = @"select Distinct EI.EmpId,EI.EmpName +case when isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as EmpName From Employee_ProcessOrderNo EMP(NoLock) inner join PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PIM(NoLock) on EMP.IssueOrderId=PIM.IssueOrderId
                            inner join EmpInfo EI(NoLock) on EMP.Empid=EI.EmpId Where  EMP.ProcessId=" + DDProcessName.SelectedValue + " and PIM.Companyid= " + DDCompanyName.SelectedValue + " and EI.Blacklist=0 order by Empname";
                            }
                            break;
                    }
                }
                else
                {
                    Str = "Select Distinct PM.EmpId,EI.EmpName +case when isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as EmpName From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM(NoLock),EMpInfo EI(NoLock) Where CompanyId=" + DDCompanyName.SelectedValue + " And PM.EmpId=EI.EmpId And EI.MasterCompanyId=" + Session["varCompanyId"] + " and EI.Blacklist=0 Order By EmpName";
                }

            }
            UtilityModule.ConditionalComboFill(ref DDEmployerName, Str, true, "--SELECT--");
           
        }
        ViewState["Hissab_No"] = 0;
        ShowButton();
    }
    protected void DDEmployerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DGDetail.DataSource = null;
        DGDetail.DataBind();
        EmployerNameSelectedIndexChanged();
        ShowButton();
    }
    private void EmployerNameSelectedIndexChanged()
    {
        if (DDProcessName.SelectedIndex > 0 && ChkForEdit.Checked == true)
        {
            
                UtilityModule.ConditionalComboFill(ref DDSlipNo, @"Select Distinct HissabNo, HissabNo HissabNo1 
                From PROCESS_HISSAB(NoLock) 
                Where CommPaymentFlag = 0 And CompanyId=" + DDCompanyName.SelectedValue + @" And YearID = " + DDYear.SelectedValue + @" And 
                ProcessID=" + DDProcessName.SelectedValue + " And Empid=" + DDEmployerName.SelectedValue + " Order by HissabNo1", true, "--SELECT--");
            
        }
        if (DDProcessName.SelectedIndex > 0)
        {
            if (DDProcessName.SelectedItem.Text == "WEAVING" && Session["VarCompanyId"].ToString() == "43")
            {
                TDPoOrderNo.Visible = false;
                FillReceiveChallanNo();
            }
            else
            {

                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("PRO_BindProductionOrderNoForHissab", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 3000;

                cmd.Parameters.AddWithValue("@CompanyId", DDCompanyName.SelectedValue);
                cmd.Parameters.AddWithValue("@ProcessId", DDProcessName.SelectedValue);
                cmd.Parameters.AddWithValue("@Empid", DDEmployerName.SelectedValue);
                cmd.Parameters.AddWithValue("@Mastercompanyid", Session["VarCompanyNo"]);
                cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);

                DataSet ds = new DataSet();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                cmd.ExecuteNonQuery();
                ad.Fill(ds);
                //*************

                con.Close();
                con.Dispose();

                UtilityModule.ConditionalComboFillWithDS(ref DDPOOrderNo, ds, 0, true, "--Plz Select--");


                ////UtilityModule.ConditionalComboFill(ref DDPOOrderNo, str, true, "--Plz Select--");
            }
        }
        ViewState["Hissab_No"] = 0;
    }
    private void ShowButton()
    {        
            if (DDCompanyName.SelectedIndex > 0 && DDProcessName.SelectedIndex > 0 && DDEmployerName.SelectedIndex > 0 && ChkForEdit.Checked == false)
            {
                BtnShowData.Visible = true;
                //if (Convert.ToInt16(Session["varcompanyId"]) == 16)
                //{
                //    BtnShowData.Visible = false;
                //}
            }
            else
            {
                BtnShowData.Visible = false;
            }
        
        
    }
    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGDetail, "select$" + e.Row.RowIndex);

            for (int i = 0; i < DGDetail.Columns.Count; i++)
            {
                if (DGDetail.Columns[i].HeaderText == "Bonus Amt" || DGDetail.Columns[i].HeaderText == "Material Amt")
                {
                    if (Session["varcompanyId"].ToString() == "42")
                    {
                        DGDetail.Columns[i].Visible = true;
                    }
                    else
                    {
                        DGDetail.Columns[i].Visible = false;
                    }
                }               

            }

        }
    }
    protected void BtnShowData_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        selectall.Visible = true;      

        if (Session["VarCompanyNo"].ToString() != "9")
        {
            TxtHissabNo.Text = "";
        }

        //if (Session["VarCompanyNo"].ToString() == "42" && DDPOOrderNo.SelectedIndex <= 0 && DDProcessName.SelectedItem.Text.ToUpper()=="WEAVING")
        //{
        //    DGDetail.DataSource = null;
        //    DGDetail.DataBind();

        //    lblMessage.Visible = true;
        //    lblMessage.Text = "Please Select Folio No for hissab";
        //    return;
        //}
        
        ShowDataInGrid();
        
    }
    private void ShowDataInGrid()
    {
        string Str = "";
        try
        {

            if (DDCompanyName.SelectedIndex > 0 && DDProcessName.SelectedIndex > 0 && DDEmployerName.SelectedIndex > 0)
            {
                if (ChkForEdit.Checked == false)
                {
                    ////                    Str = @"SELECT CN.STOCKNO SR_NO,CN.TSTOCKNO STOCKNO,VF.CATEGORY_NAME CATEGORY,VF.ITEM_NAME  ITEM,
                    ////                    VF.QUALITYNAME+' / '+VF.DesignName+' / '+VF.ColorName+' / '+VF.ShapeName+' / '+Case When PM.UnitID=1 Then VF.SizeMtr Else case When UnitId=2 Then VF.SizeFt Else Case When UnitId=6 Then Sizeinch Else Sizeft End End End DESCRIPTION,1 QTY,
                    ////                    PD.Area,PD.Rate,Round(PD.Amount/PD.Qty,2) Amount,isnull(Round(PD.Area*[dbo].[Get_Process_Total_Consmp](PD.Issue_Detail_Id,PM.UnitId, " + DDProcessName.SelectedValue + @"),3),0) ReqWeight,
                    ////                    Round(PD.Weight/PD.Qty,3) Weight,Round(PD.Penality/PD.Qty,2) Penality,'' PRemark,1 Flag,Pm.Unitid,PM.Caltype,CN.Item_finished_id,Round(PD.commamt/PD.Qty,2) commAmount,isnull(PD.comm,0) as comm FROM PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PM,
                    ////                    PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PD,PROCESS_STOCK_DETAIL PSD,CARPETNUMBER CN,V_FinishedItemDetail VF 
                    ////                    WHERE PM.Process_Rec_ID=PD.Process_Rec_ID AND PD.PROCESS_REC_DETAIL_ID=PSD.ReceiveDetailId And CN.StockNo=PSD.StockNo 
                    ////                    And PD.Item_Finished_Id=VF.Item_Finished_Id And PD.QualityType<>3   And PSD.HissabFlag=0 AND PM.COMPANYID=" + DDCompanyName.SelectedValue + " And PSD.ToProcessId=" + DDProcessName.SelectedValue + @" AND 
                    ////                    PM.EMPID=" + DDEmployerName.SelectedValue + " AND PM.ReceiveDate>='" + TxtFromDate.Text.ToString() + "' and PM.ReceiveDate<='" + TxtToDate.Text.ToString() + "' And VF.MasterCompanyId=" + Session["varCompanyId"];
                    ////                    if (DDPOOrderNo.SelectedIndex > 0)
                    ////                    {
                    ////                        Str = Str + " And PD.IssueOrderid=" + DDPOOrderNo.SelectedValue + "";
                    ////                    }
                    ////                    if (DDsrno.SelectedIndex > 0)
                    ////                    {
                    ////                        Str = Str + " And PD.orderid=" + DDsrno.SelectedValue + "";
                    ////                    }

                    //SqlParameter[] param = new SqlParameter[9];
                    //param[0] = new SqlParameter("@CompanyId", DDCompanyName.SelectedValue);
                    //param[1] = new SqlParameter("@Toprocessid", DDProcessName.SelectedValue);
                    //param[2] = new SqlParameter("@Empid", DDEmployerName.SelectedValue);
                    //param[3] = new SqlParameter("@FromDate", TxtFromDate.Text);
                    //param[4] = new SqlParameter("@TODate", TxtToDate.Text);
                    //param[5] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
                    //param[6] = new SqlParameter("@Issueorderid", DDPOOrderNo.SelectedIndex > 0 ? DDPOOrderNo.SelectedValue : "0");
                    //param[7] = new SqlParameter("@orderid", DDsrno.SelectedIndex > 0 ? DDsrno.SelectedValue : "0");

                    //DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GETHISSABDATA", param);

                    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("Pro_GETHISSABDATA", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300;

                    cmd.Parameters.AddWithValue("@CompanyId", DDCompanyName.SelectedValue);
                    cmd.Parameters.AddWithValue("@Toprocessid", DDProcessName.SelectedValue);
                    cmd.Parameters.AddWithValue("@Empid", DDEmployerName.SelectedValue);
                    cmd.Parameters.AddWithValue("@FromDate", System.DateTime.Now.ToString("dd-MMM-yyyy"));
                    cmd.Parameters.AddWithValue("@TODate", System.DateTime.Now.ToString("dd-MMM-yyyy"));
                    cmd.Parameters.AddWithValue("@Mastercompanyid", Session["varcompanyid"]);
                    cmd.Parameters.AddWithValue("@Issueorderid", DDPOOrderNo.SelectedIndex > 0 ? DDPOOrderNo.SelectedValue : "0");
                    cmd.Parameters.AddWithValue("@orderid", DDsrno.SelectedIndex > 0 ? DDsrno.SelectedValue : "0");
                    //cmd.Parameters.AddWithValue("@Item_Finished_Id", DDItemDescription.SelectedIndex > 0 ? DDItemDescription.SelectedValue : "0");
                    cmd.Parameters.AddWithValue("@Process_Rec_ID", DDReceiveChallanNo.SelectedIndex > 0 ? DDReceiveChallanNo.SelectedValue : "0");                    


                    DataSet Ds = new DataSet();
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    cmd.ExecuteNonQuery();
                    ad.Fill(Ds);

                    //if (Session["VarCompanyNo"].ToString() == "9")
                    //{
                    //    if (Ds.Tables[1].Rows.Count > 0)
                    //    {
                    //        UtilityModule.ConditionalComboFillWithDS(ref DDItemDescription, Ds, 1, true, "--SELECT--");                            
                    //    }
                    //}

                    DGDetail.DataSource = Ds.Tables[0];
                    DGDetail.DataBind();

                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        txttotalpcs.Text = Ds.Tables[0].Compute("Sum(Qty)", "").ToString();
                        txttotalarea.Text = Math.Round(Convert.ToDouble(Ds.Tables[0].Compute("Sum(Area)", "")), 3).ToString();                       
                        txtamount.Text = Math.Round(Convert.ToDouble(Ds.Tables[0].Compute("Sum(TAmount)", "")), 2).ToString(); 
                        txttotalcommission.Text = Math.Round(Convert.ToDouble(Ds.Tables[0].Compute("Sum(commamount)", "")), 2).ToString();
                        txttotalpenality.Text = Math.Round(Convert.ToDouble(Ds.Tables[0].Compute("Sum(Penality)", "")), 2).ToString();
                        
                        if (Session["VarCompanyNo"].ToString() == "42")
                        {
                            txtTotalBonusAmt.Text = Math.Round(Convert.ToDouble(Ds.Tables[0].Compute("Sum(BonusAmt)", "")), 2).ToString();
                            txtAdditionAmt.Text = Math.Round(Convert.ToDouble(Ds.Tables[0].Compute("Sum(MaterialAmt)", "")), 2).ToString();

                            FillMaterialDeductionCharge();
                        }
                        else
                        {
                            txtTotalBonusAmt.Text = "0";                          
                        }
                    }
                    else
                    {
                        txttotalpcs.Text = "";
                        txttotalarea.Text = "";
                        txtamount.Text = "";
                        txttotalcommission.Text = "";
                        txttotalpenality.Text = "";
                        txtAdditionAmt.Text = "";
                        txtDeductionAmt.Text = "";
                        txtTotalBonusAmt.Text = "";
                        txtMaterialDeductionAmt.Text = "";
                        ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    }
                }

            }
        }
        catch (Exception ex)
        {
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;            
        }
    }
    private void ForCheckAllRows()
    {
        for (int i = 0; i < DGDetail.Rows.Count; i++)
        {
            GridViewRow row = DGDetail.Rows[i];
            if (Convert.ToInt32(DGDetail.Rows[i].Cells[12].Text) == 1)
            {
                ((CheckBox)row.FindControl("Chkbox")).Checked = true;
            }
            else
            {
                ((CheckBox)row.FindControl("Chkbox")).Checked = false;
            }
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SaveOther();
    }

    private void SaveOther()
    {
        int VarEditVarible = 0;
        string Str = "";
        ////*************CHECK DATE
        //if (Convert.ToDateTime(TxtFromDate.Text) > Convert.ToDateTime(TxtDate.Text) || Convert.ToDateTime(TxtToDate.Text) > Convert.ToDateTime(TxtDate.Text))
        //{

        //    ScriptManager.RegisterStartupScript(Page, GetType(), "date", "alert('Slip Date can not be less than From and To Date.');", true);
        //    return;
        //}
        //*************
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //**********
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("FinishedId", typeof(int));
        dtrecords.Columns.Add("StockNo", typeof(int));
        dtrecords.Columns.Add("Qty", typeof(int));
        dtrecords.Columns.Add("Area", typeof(float));
        dtrecords.Columns.Add("Rate", typeof(float));
        dtrecords.Columns.Add("Amount", typeof(float));
        dtrecords.Columns.Add("ReqWeight", typeof(float));
        dtrecords.Columns.Add("Weight", typeof(float));
        dtrecords.Columns.Add("Penality", typeof(float));
        dtrecords.Columns.Add("PRemark", typeof(string));
        dtrecords.Columns.Add("UnitId", typeof(int));
        dtrecords.Columns.Add("Commamount", typeof(float));
        dtrecords.Columns.Add("comm", typeof(float));
        dtrecords.Columns.Add("IssueOrderID", typeof(int));
        dtrecords.Columns.Add("CalType", typeof(int));
        dtrecords.Columns.Add("Bonus", typeof(float));
        dtrecords.Columns.Add("BonusAmt", typeof(float));
        dtrecords.Columns.Add("MaterialAmtCarpetNoWise", typeof(float));
        dtrecords.Columns.Add("FRRate2", typeof(float));
        //****

        for (int i = 0; i < DGDetail.Rows.Count; i++)
        {
            if (((CheckBox)DGDetail.Rows[i].FindControl("Chkbox")).Checked == true)
            {
                TextBox TxtRate = (TextBox)DGDetail.Rows[i].FindControl("TxtRate");
                TextBox TxtWeight = (TextBox)DGDetail.Rows[i].FindControl("TxtWeight");
                TextBox TxtPenality = (TextBox)DGDetail.Rows[i].FindControl("TxtPenality");
                TextBox TxtPRemark = (TextBox)DGDetail.Rows[i].FindControl("TxtPRemark");
                Label Txtunitid = (Label)DGDetail.Rows[i].FindControl("lblunitid");
                Label TxtCalType = (Label)DGDetail.Rows[i].FindControl("lblcaltype");
                Label lblfinishedid = (Label)DGDetail.Rows[i].FindControl("lblfinishedid");
                Label lblreqweight = (Label)DGDetail.Rows[i].FindControl("lblreqweight");
                Label lblarea = (Label)DGDetail.Rows[i].FindControl("lblarea");
                Label lblqty = (Label)DGDetail.Rows[i].FindControl("lblqty");
                Label lblamount = (Label)DGDetail.Rows[i].FindControl("lblamount");
                Label lblcommamount = (Label)DGDetail.Rows[i].FindControl("lblcommamount");
                Label lblcommrate = (Label)DGDetail.Rows[i].FindControl("lblcommrate");
                Label lblIssueOrderID = (Label)DGDetail.Rows[i].FindControl("lblIssueOrderID");
                Label lblBonus = (Label)DGDetail.Rows[i].FindControl("lblBonus");
                TextBox txtBonusAmt = (TextBox)DGDetail.Rows[i].FindControl("txtBonusAmt");
                TextBox txtMaterialAmt = (TextBox)DGDetail.Rows[i].FindControl("txtMaterialAmt");
                Label lblFRRate2 = (Label)DGDetail.Rows[i].FindControl("lblFRRate2");
                

                string VarRate = TxtRate.Text == "" ? "0" : TxtRate.Text;
                string VarWeight = TxtWeight.Text == "" ? "0" : TxtWeight.Text;
                string VarPenality = TxtPenality.Text == "" ? "0" : TxtPenality.Text;
                string VarPRemark = TxtPRemark.Text == "" ? " " : TxtPRemark.Text;
                string Varunitid = Txtunitid.Text == "" ? " " : Txtunitid.Text;
                string VarCaltype = TxtCalType.Text == "" ? " " : TxtCalType.Text;
                string VarBonusAmt = txtBonusAmt.Text == "" ? "0" : txtBonusAmt.Text;
                string VarMaterialAmtCarpetNoWise = txtMaterialAmt.Text == "" ? "0" : txtMaterialAmt.Text;
                string VarFRRate2 = lblFRRate2.Text == "" ? "0" : lblFRRate2.Text;
                

                //Str = DGDetail.DataKeys[i].Value + "," + DGDetail.Rows[i].Cells[4].Text + "," + DGDetail.Rows[i].Cells[5].Text + "," + VarRate + "," + DGDetail.Rows[i].Cells[8].Text + "," + VarWeight + "," + VarPenality + "," + VarPRemark + "," + Varunitid + "," + VarCaltype;
                Str = "" + VarRate + "," + lblreqweight.Text + "," + VarWeight + "," + VarPenality + "," + VarPRemark + "," + Varunitid + "," + VarCaltype + "," + VarBonusAmt + "," + VarMaterialAmtCarpetNoWise + "," + VarFRRate2;
                DataRow dr = dtrecords.NewRow();
                Decimal amount;
                amount = Convert.ToDecimal(lblamount.Text);

                dr["FinishedId"] = lblfinishedid.Text;
                dr["StockNo"] = DGDetail.DataKeys[i].Value;
                dr["Qty"] = lblqty.Text;
                dr["Area"] = lblarea.Text;
                dr["Rate"] = VarRate;
                dr["Amount"] = amount;
                dr["ReqWeight"] = lblreqweight.Text;
                dr["Weight"] = VarWeight;
                dr["Penality"] = VarPenality;
                dr["PRemark"] = VarPRemark;
                dr["UnitId"] = Varunitid;
                dr["commamount"] = lblcommamount.Text;
                dr["comm"] = lblcommrate.Text;
                dr["IssueOrderID"] = lblIssueOrderID.Text;
                dr["CalType"] = VarCaltype;
                dr["Bonus"] = lblBonus.Text;
                dr["BonusAmt"] = VarBonusAmt;
                dr["MaterialAmtCarpetNoWise"] = VarMaterialAmtCarpetNoWise;
                dr["FRRate2"] = VarFRRate2;
                dtrecords.Rows.Add(dr);
            }

        }
        if (dtrecords.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("[PRO_PROCESS_HISSAB]", con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;

                cmd.Parameters.AddWithValue("@CompanyId", DDCompanyName.SelectedValue);
                cmd.Parameters.AddWithValue("@ProcessID", DDProcessName.SelectedValue);
                cmd.Parameters.AddWithValue("@EmpId", DDEmployerName.SelectedValue);
                cmd.Parameters.AddWithValue("@ProcessOrderNo", 0);
                cmd.Parameters.AddWithValue("@HissabNo", SqlDbType.Int);
                cmd.Parameters["@HissabNo"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Date", TxtDate.Text);
                cmd.Parameters.AddWithValue("@ChallanNo", "");
                cmd.Parameters.AddWithValue("@VarHissabNo", ViewState["Hissab_No"]);
                cmd.Parameters.AddWithValue("@UnitId", 0);

                if (ChkForEdit.Checked == true)
                {
                    VarEditVarible = VarEditVarible + 1;
                    cmd.Parameters.AddWithValue("@ID", VarEditVarible);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ID", 0);
                }
                cmd.Parameters.AddWithValue("@dtrecords", dtrecords);
                cmd.Parameters.AddWithValue("@FromDate", System.DateTime.Now.ToString("dd-MMM-yyyy"));
                cmd.Parameters.AddWithValue("@ToDate", System.DateTime.Now.ToString("dd-MMM-yyyy"));
                cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
                cmd.Parameters.AddWithValue("@AdditionAmt", txtAdditionAmt.Text == "" ? "0" : txtAdditionAmt.Text);
                cmd.Parameters.AddWithValue("@DeductionAmt", txtDeductionAmt.Text == "" ? "0" : txtDeductionAmt.Text);
                cmd.Parameters.Add(new SqlParameter("@MSG", SqlDbType.VarChar, 300));
                cmd.Parameters["@MSG"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@MaterialDeductionAmt", TDMaterialDeductionAmt.Visible == true ? txtMaterialDeductionAmt.Text == "" ? "0" : txtMaterialDeductionAmt.Text : "0");
                cmd.Parameters.AddWithValue("@ProcessHissabGST", TDGST.Visible == true ? txtGST.Text == "" ? "0" : txtGST.Text : "0");
                cmd.Parameters.AddWithValue("@Process_Rec_Id", DDReceiveChallanNo.SelectedIndex>0 ? DDReceiveChallanNo.SelectedValue : "0");

                cmd.ExecuteNonQuery();

                if (cmd.Parameters["@MSG"].Value.ToString() != "")
                {
                    lblMessage.Text = "";
                    lblMessage.Visible = true;
                    lblMessage.Text = cmd.Parameters["@MSG"].Value.ToString();
                    return;
                }

                ViewState["Hissab_No"] = cmd.Parameters["@HissabNo"].Value.ToString();
                Tran.Commit();
                lblMessage.Visible = true;                
                lblMessage.Text = "Data Inserted Successfully !";
                ShowDataInGrid();
                TxtHissabNo.Text = ViewState["Hissab_No"].ToString();
                ChkForAllSelect.Checked = false;
                txtAdditionAmt.Text = "";
                txtDeductionAmt.Text = "";
                txtMaterialDeductionAmt.Text = "";
                txtGST.Text = "";
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void ChkForEdit_CheckedChanged(object sender, EventArgs e)
    {
        EditSelectedChange();
    }
    private void EditSelectedChange()
    {
        CheckForEditSelectedChanges();
        ProcessNameSelectedIndexChanged();
    }
    private void CheckForEditSelectedChanges()
    {
        if (ChkForEdit.Checked == true)
        {
            TDSlipNoForEdit.Visible = true;
            TDDDSlipNo.Visible = true;
            BtnDelete.Visible = true;
            TDPoOrderNo.Visible = false;           
            TDDDYear.Visible = true;
        }
        else
        {
            TDPoOrderNo.Visible = true;
            BtnDelete.Visible = false;
            TDSlipNoForEdit.Visible = false;
            TDDDSlipNo.Visible = false;
            TxtSlipNo.Text = "";
            DDSlipNo.Items.Clear();
            if (DDEmployerName.Items.Count > 0)
            {
                DDEmployerName.SelectedIndex = 0;
            }
           
            TDDDYear.Visible = false;
        }
        //ButtonBtnSaveAllProcessWise();
        ////CompanyNameSelectedIndexChanged();
    }
    protected void TxtSlipNo_TextChanged(object sender, EventArgs e)
    {
        lblMessage.Visible = false;
        if (TxtSlipNo.Text != "")
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, 
                @"Select CompanyId,ProcessID,EmpId,ProcessOrderNo,HissabNo,replace(convert(varchar(11),Date,106), ' ','-') as Date,ChallanNo 
                    From PROCESS_HISSAB Where CommPaymentFlag=0 And CompanyID = " + DDCompanyName.SelectedValue + " And HissabNo=" + TxtSlipNo.Text + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                //CompanyId,ProcessID,EmpId,ProcessOrderNo,HissabNo,Date,Finishedid,StockNo,Qty,Area,Rate,Amount,Weight,Penality,PRemarks,ChallanNo,UnitId

                //DDCompanyName.SelectedValue = Ds.Tables[0].Rows[0]["CompanyId"].ToString();
                //CompanyNameSelectedIndexChanged();
                DDProcessName.SelectedValue = Ds.Tables[0].Rows[0]["ProcessID"].ToString();
                ProcessNameSelectedIndexChanged();
                DDEmployerName.SelectedValue = Ds.Tables[0].Rows[0]["EmpId"].ToString();
                EmployerNameSelectedIndexChanged();
                TxtHissabNo.Text = "";
                DDSlipNo.SelectedValue = Ds.Tables[0].Rows[0]["HissabNo"].ToString();
                SlipNoSelectedChanges();
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Pls Enter Proper Slip No";
            }
        }
        else
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Pls. Enter Proper Slip No";
        }
    }
    protected void DDSlipNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        SlipNoSelectedChanges();
    }
    private void SlipNoSelectedChanges()
    {
        ViewState["Hissab_No"] = DDSlipNo.SelectedValue;
        if (DDSlipNo.SelectedIndex > 0)
        {
            TxtHissabNo.Text = DDSlipNo.Text;
            TxtDate.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select IsNull(replace(convert(varchar(11),Date,106), ' ','-'),'') as Date From PROCESS_HISSAB Where HissabNo=" + DDSlipNo.SelectedValue + "").ToString();
        }
        ShowDataInGrid();
    }
    protected void BtnPriview_Click(object sender, EventArgs e)
    {
        if (Session["varCompanyNo"].ToString() == "43")
        {
            ReceiveChallanNowiseReport();
        }
        else if (chkstocknowise.Checked == true)
        {
            StockNowiseReport();
        }
        else
        {

            switch (Session["varcompanyNo"].ToString())
            {
                case "30":
                    Session["ReportPath"] = "Reports/RptProcessHissabSummarySamara.rpt";
                    break;
                case "27":
                    Session["ReportPath"] = "Reports/RptProcessHissabSummaryWithIssueOrderId.rpt";
                    break;
                case "9":
                    Session["ReportPath"] = "Reports/RptProcessHissabSummaryHafizia.rpt";
                    break;
                case "38":
                    Session["ReportPath"] = "Reports/RptProcessHissabSummaryVikramKhamaria.rpt";
                    break;
                case "42":
                    if (DDProcessName.SelectedValue == "1")
                    {
                        Session["ReportPath"] = "Reports/RptProcessHissabSummaryVikramMirzapur.rpt";
                    }
                    else
                    {
                        Session["ReportPath"] = "Reports/RptProcessHissabSummaryVikramMirzapurForNextProcess.rpt";
                    }
                    break;               
                default:
                    Session["ReportPath"] = "Reports/RptProcessHissabSummary.rpt";
                    break;
            }
            //Session["ReportPath"] = "Reports/RptProcessHissabSummary.rpt";

            Session["CommanFormula"] = "{VIEW_PROCESS_HISSAB.HissabNo}= " + ViewState["Hissab_No"];
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
        }
    }
    protected void StockNowiseReport()
    {
        ////string str = "select * from VIEW_PROCESS_HISSAB_StockNo Where HissabNo=" + ViewState["Hissab_No"];
       /// DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        string str = @"select * from VIEW_PROCESS_HISSAB_StockNo Where HissabNo=" + ViewState["Hissab_No"];
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand(str, con);
        //cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        if (ds.Tables[0].Rows.Count > 0)
        {
            switch (Session["varcompanyNo"].ToString())
            {
                case "16":
                    if (DDProcessName.SelectedValue == "1")
                    {
                        Session["rptFileName"] = "Reports/rptprocesshissabstocknowise.rpt";
                    }
                    else if (ChkForRateWise.Checked == true)
                    {
                        Session["rptFileName"] = "Reports/rptProcesshissabstocknowise_otherjob_WithRate.rpt";
                    }
                    else
                    {
                        Session["rptFileName"] = "Reports/rptProcesshissabstocknowise_otherjob.rpt";
                    }
                    break;
                case "28":
                    if (DDProcessName.SelectedValue == "1")
                    {
                        Session["rptFileName"] = "Reports/rptprocesshissabstocknowise.rpt";
                    }
                    else if (ChkForRateWise.Checked == true)
                    {
                            Session["rptFileName"] = "Reports/rptProcesshissabstocknowise_otherjob_WithRateWithPenality.rpt";
                    }
                    else
                    {
                        Session["rptFileName"] = "Reports/rptProcesshissabstocknowise_otherjob.rpt";
                    }
                    break;
                default:
                    Session["rptFileName"] = "Reports/rptprocesshissabstocknowise.rpt";
                    break;
            }

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptprocesshissabstocknowise.xsd";
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
    protected void DGDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGDetail.PageIndex = e.NewPageIndex;
        ShowDataInGrid();
    }
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@Hissab_No", ViewState["Hissab_No"]);
            param[1] = new SqlParameter("@processid", DDProcessName.SelectedValue);
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEPROCESSHISSAB", param);
            if (param[4].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('" + param[4].Value.ToString() + "');", true);
                Tran.Rollback();
            }
            else
            {

                Tran.Commit();
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Slip successfully deleted!');", true);
                ChkForEdit.Checked = false;
                BtnDelete.Visible = false;
                TDSlipNoForEdit.Visible = false;
                TDDDSlipNo.Visible = false;
                TxtSlipNo.Text = "";
                TxtHissabNo.Text = "";
                DDSlipNo.Items.Clear();
                ViewState["Hissab_No"] = 0;
                ShowButton();
            }
            //            string Str = "Update Process_Stock_Detail Set HissabFlag=0 Where StockNo in (Select StockNo From PROCESS_HISSAB Where HissabNo in (" + ViewState["Hissab_No"] + ")) And ToProcessId=" + DDProcessName.SelectedValue + @"
            //                          Delete PROCESS_HISSAB Where HissabNo in (" + ViewState["Hissab_No"] + @")
            //                          Exec Pro_Updatestatus " + Session["varcompanyId"] + "," + Session["Varuserid"] + ",'PROCESS_HISSAB'," + ViewState["Hissab_No"] + ",'Hissab Slip deleted..'";


            //            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, Str);



        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }


    protected void DDPOOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select Replace(convert(nvarchar(11),isnull(MIN(PRM.ReceiveDate),Getdate()),106),' ','-') As FromDate,Replace(convert(nvarchar(11),
                    isnull(MAX(PRM.ReceiveDate),getdate()),106),' ','-') as ToDate
                    From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PRM(NoLock) inner Join PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PRD(NoLock) on PRM.Process_Rec_Id=PRD.Process_Rec_Id
                    Where PRD.IssueOrderId=" + DDPOOrderNo.SelectedValue + " and PRM.EmpId=" + DDEmployerName.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //TxtFromDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["FromDate"]);
        //TxtToDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["Todate"]);
        FillSrno();
        //FillReceiveChallanNo();
        
    }
    private void FillReceiveChallanNo()
    {
//        string str = @"select Distinct PRM.PROCESS_REC_ID,PRM.CHALLANNO
//                    From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PRM(NoLock) inner Join PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PRD(NoLock) on PRM.Process_Rec_Id=PRD.Process_Rec_Id
//                    Where PRD.IssueOrderId=" + DDPOOrderNo.SelectedValue + "";
//        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        string str = @"select Distinct PRM.PROCESS_REC_ID,PRM.CHALLANNO
                    From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + @" PRM(NoLock) 
                    inner Join PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PRD(NoLock) on PRM.Process_Rec_Id=PRD.Process_Rec_Id
                    inner join Process_Stock_Detail psd(NoLock)  on PRD.Process_Rec_Detail_Id=Psd.ReceiveDetailId and psd.ToProcessId=" + DDProcessName.SelectedValue + @" And Psd.HissabFlag=0 
                    JOIN Employee_ProcessReceiveNo EPO(nolock) ON EPO.ProcessID = " + DDProcessName.SelectedValue + @" And EPO.Process_Rec_id = PRM.PROCESS_REC_ID And EPO.Process_Rec_Detail_id = PRD.PROCESS_REC_DETAIL_ID AND EPO.Empid = " + DDEmployerName.SelectedValue+@"
                    Where PRM.CompanyId=" + DDCompanyName.SelectedValue + " Order by PRM.PROCESS_REC_ID desc";

        UtilityModule.ConditionalComboFill(ref DDReceiveChallanNo, str, true, "--Select--");
    }
    protected void DDReceiveChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }
    protected void DDsrno_SelectedIndexChanged(object sender, EventArgs e)
    {
//        if (Session["VarCompanyNo"].ToString() == "9")
//        {
//            string str = @"select Replace(convert(nvarchar(11),isnull(MIN(PRM.ReceiveDate),Getdate()),106),' ','-') As FromDate,Replace(convert(nvarchar(11),
//                    isnull(MAX(PRM.ReceiveDate),getdate()),106),' ','-') as ToDate
//                    From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PRM(NoLock) inner Join PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PRD(NoLock) on PRM.Process_Rec_Id=PRD.Process_Rec_Id
//                    Where PRD.OrderID=" + DDsrno.SelectedValue + " and PRM.EmpId=" + DDEmployerName.SelectedValue;
//            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
//            //TxtFromDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["FromDate"]);
//            //TxtToDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["Todate"]);      
//        }        

       
    }
    protected void btnprintvoucher_Click(object sender, EventArgs e)
    {
        string view = "V_Printvoucher";
        switch (Session["varcompanyNo"].ToString())
        {
            case "16":
                if (DDProcessName.SelectedValue == "1")
                {
                    Session["rptFileName"] = "Reports/rptvoucher.rpt";
                }
                else
                {
                    view = "V_PRINTVOUCHER_OTHERJOB";
                    Session["rptFileName"] = "Reports/rptvoucher_otherjob.rpt";
                }
                break;
            case "42":
                if (DDProcessName.SelectedValue == "1")
                {
                    view = "V_PRINTVOUCHER_FOLIOWISE";
                    Session["rptFileName"] = "Reports/RptVoucher_VikramMirzapur.rpt";
                }
                else
                {
                    view = "V_PRINTVOUCHER_FORNEXRPROCESS";
                    Session["rptFileName"] = "Reports/RptVoucher_VikramMirzapurForNextProcess.rpt";
                }                
                break;
            default:
                Session["rptFileName"] = "Reports/rptvoucher.rpt";
                break;
        }
        string str = "select * from " + view + " Where SlipNo=" + ViewState["Hissab_No"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptvoucher.xsd";
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
    protected void btnsearchemp_Click(object sender, EventArgs e)
    {
        if (txtWeaverIdNoscan.Text != "")
        {
            string str = "select empid   From empinfo where empcode='" + txtWeaverIdNoscan.Text + "'";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (DDEmployerName.Items.FindByValue(ds.Tables[0].Rows[0]["empid"].ToString()) != null)
                {
                    DDEmployerName.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
                    DDEmployerName_SelectedIndexChanged(sender, new EventArgs());

                }
                txtWeaverIdNoscan.Text = "";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('No Employee found on this Employee code.')", true);
                txtWeaverIdNoscan.Focus();
            }
        }
    }
    //protected void TxtToDate_TextChanged(object sender, EventArgs e)
    //{
    //    TxtDate.Text = TxtToDate.Text;
    //}
    
    protected void FillMaterialDeductionCharge()
    {
        if (Session["VarCompanyNo"].ToString() == "42" && DDPOOrderNo.SelectedIndex > 0 && DDProcessName.SelectedItem.Text.ToUpper() == "WEAVING")
        { 

            string str = "";
            str = "Select isnull(sum(AA.AdvanceAmt),0) as AdvanceAmt From AdvanceAmountByFolioNoWise AA(NoLock) Where AA.PaymentType=2 And AA.CompanyID = " + DDCompanyName.SelectedValue + @" 
                And AA.EmpId = " + DDEmployerName.SelectedValue + @" and AA.ProcessId=" + DDProcessName.SelectedValue + " and AA.IssueOrderId=" + DDPOOrderNo.SelectedValue + " ";

            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                txtMaterialDeductionAmt.Text = Ds.Tables[0].Rows[0]["AdvanceAmt"].ToString();
            }
        }       
    }
    protected void DDYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDProcessName.SelectedIndex = 0;
    }

    protected void ReceiveChallanNowiseReport()
    {
        SqlParameter[] array = new SqlParameter[4];
        array[0] = new SqlParameter("@HissabNo", ViewState["Hissab_No"]);
        array[1] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
        array[2] = new SqlParameter("@MasterCompanyId", Session["VarCompanyNo"]);
        array[3] = new SqlParameter("@UserId", Session["VarUserId"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HissabReportReceiveChallanNoWise", array);
        
        if (ds.Tables[0].Rows.Count > 0)
        {
            switch (Session["varcompanyNo"].ToString())
            {
                case "43":
                    Session["rptFileName"] = "Reports/RptProcessHissabReceiveChallanNoWise.rpt";
                    break;
                default:                    
                    break;
            }

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptProcessHissabReceiveChallanNoWise.xsd";
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