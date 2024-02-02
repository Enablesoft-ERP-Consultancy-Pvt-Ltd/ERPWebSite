using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_HomeFurnishing_FrmHomeFurnishingProductionHissab : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            string Str = @"Select CI.CompanyId, CI.CompanyName 
                    From CompanyInfo CI(Nolock) 
                    JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                    Where CI.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" Order By CI.CompanyName ";

            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, Ds, 0, true, "--SELECT--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TDRadioButton.Visible = false;
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            CheckForEditSelectedChanges();
            ViewState["Hissab_No"] = 0;
            //    if (Convert.ToInt16(Session["varMasterCompanyIDForERP"]) == 16)
            //    {
            //        BtnSaveAllOneTime.Visible = true;
            //        BtnSaveAllProcessWise.Visible = true;
            //    }
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
            From HomeFurnishingOrderHissab PH(Nolock) 
            JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.Process_Name_Id = PH.ProcessId 
            Where PH.CompanyID = " + DDCompanyName.SelectedValue + " And PH.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By PNM.Process_Name";
        }
        else
        {
            Str = @"Select Distinct PNM.PROCESS_NAME_ID, PNM.PROCESS_NAME 
            From HomeFurnishingReceiveMaster HFRM(Nolock) 
            JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = HFRM.ProcessID 
            JOIN UserRightsProcess URP(Nolock) ON URP.ProcessId = PNM.PROCESS_NAME_ID And URP.Userid = " + Session["varuserid"] + @" 
            Order by PNM.PROCESS_NAME";
        }
        UtilityModule.ConditionalComboFill(ref DDProcessName, Str, true, "--SELECT--");
        ViewState["Hissab_No"] = 0;
    }

    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessNameSelectedIndexChanged();
    }
    private void ProcessNameSelectedIndexChanged()
    {
        if (DDProcessName.SelectedIndex > 0)
        {
            string Str = "";
            if (ChkForEdit.Checked == true)
            {
                Str = @"Select Distinct EI.EmpId, EI.EmpName + Case When IsNull(EI.EmpCode, '') <> '' Then ' [' + EI.EmpCode + ']' Else '' End EmpName 
                From HomeFurnishingOrderHissab PH(Nolock) 
                JOIN EmpInfo EI(Nolock) ON EI.EmpID = PH.EmpID And EI.Blacklist = 0 
                Where PH.CommPaymentFlag = 0 And PH.CompanyID = " + DDCompanyName.SelectedValue + " And PH.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" And 
                PH.ProcessId = " + DDProcessName.SelectedValue + " Order By EmpName";
            }
            else
            {
                Str = @"SELECT DISTINCT EI.EMPID, EI.EMPNAME + CASE WHEN EI.EMPCODE <> '' THEN ' [' + ISNULL(EI.EMPCODE, '') + ']' ELSE '' END EMPNAME 
                FROM HomeFurnishingReceiveMaster PIM(Nolock) 
                JOIN Employee_HomeFurnishingReceiveMaster EMP(Nolock) ON EMP.ProcessRecID = PIM.ProcessRecID AND EMP.PROCESSID = PIM.ProcessID 
                JOIN EMPINFO EI(Nolock) ON EI.EMPID = EMP.EMPID And EI.Blacklist = 0 
                Where PIM.CompanyID = " + DDCompanyName.SelectedValue + " And PIM.ProcessID = " + DDProcessName.SelectedValue + @" ORDER BY EMPNAME";
            }
            UtilityModule.ConditionalComboFill(ref DDEmployerName, Str, true, "--SELECT--");
        }
        ViewState["Hissab_No"] = 0;
    }
    protected void DDEmployerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmployerNameSelectedIndexChanged();
    }
    private void EmployerNameSelectedIndexChanged()
    {
        if (DDProcessName.SelectedIndex > 0 && ChkForEdit.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDSlipNo, @"Select Distinct HissabNo, HissabNo HissabNo1 
            From HomeFurnishingOrderHissab(Nolock) 
            Where CommPaymentFlag = 0 And CompanyId = " + DDCompanyName.SelectedValue + " And ProcessID = " + DDProcessName.SelectedValue + @" And 
            EmpID = " + DDEmployerName.SelectedValue + " Order by HissabNo1", true, "--SELECT--");
        }
        ViewState["Hissab_No"] = 0;
    }
    protected void BtnShowData_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        selectall.Visible = true;
        TxtHissabNo.Text = "";
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
            BtnSaveAllProcessWise.Visible = false;
        }
        else
        {
            BtnDelete.Visible = false;
            TDSlipNoForEdit.Visible = false;
            TDDDSlipNo.Visible = false;
            TxtSlipNo.Text = "";
            DDSlipNo.Items.Clear();
            if (DDEmployerName.Items.Count > 0)
            {
                DDEmployerName.SelectedIndex = 0;
            }
        }
        ButtonBtnSaveAllProcessWise();
        CompanyNameSelectedIndexChanged();
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
    }
    protected void BtnPriview_Click(object sender, EventArgs e)
    {
        if (chkstocknowise.Checked == true)
        {
            StockNowiseReport();
        }
        else
        {
            Session["ReportPath"] = "Reports/RptHomeFurnishingProcessHissabSummary.rpt";

            Session["CommanFormula"] = "{VIEW_PROCESS_HISSAB.HissabNo}= " + ViewState["Hissab_No"].ToString() + "";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
        }
    }

    protected void StockNowiseReport()
    {
        string str = @"SELECT DISTINCT PH.ID,CI.COMPANYNAME,PH.COMPANYID,PH.PROCESSID,EI.EMPNAME+CASE WHEN ISNULL(EI.EMPCODE,'')='' THEN '' ELSE ' ['+EI.EMPCODE+']' END AS EMPNAME,PH.EMPID,
        PH.PROCESSORDERNO,PH.HISSABNO,PH.DATE, PH.FINISHEDID,PH.STOCKNO,PH.QTY,PH.AREA,PH.RATE,PH.AMOUNT,PH.WEIGHT,PH.PENALITY,PH.PREMARKS,
        PH.CHALLANNO,PH.UNITID,ISNULL(PH.TDS,0) AS TDS,PH.REQWEIGHT,PH.COMMPAYMENTFLAG,PH.FROMDATE,PH.TODATE,
        PH.COMMAMOUNT,CN.TSTOCKNO,
        PNM.PROCESS_NAME,VF.QUALITYNAME,VF.DESIGNNAME,VF.COLORNAME,VF.SHAPENAME,
        Case When MS.VARCOMPANYNO = 9 Then CASE WHEN PNM.PROCESS_NAME='WEAVING' THEN DBO.F_GETSTOCKACTUALSIZE(CN.STOCKNO,EI.MASTERCOMPANYID) ELSE  
        CASE WHEN PH.UNITID=1 THEN VF.SIZEMTR WHEN PH.UNITID=6 THEN VF.SIZEINCH ELSE VF.SIZEFT End End
        Else 
        CASE WHEN PNM.PROCESS_NAME='WEAVING' THEN CASE WHEN PH.UNITID=1 THEN VF.SIZEMTR WHEN PH.UNITID=6 THEN VF.SIZEINCH ELSE VF.SIZEFT End ELSE
        CASE WHEN MS.VARFINISHINGUNIT=1 THEN (CASE WHEN MS.VARFINISHINGSIZE=1 THEN VF.SIZEMTR
            WHEN MS.VARFINISHINGUNIT=2 THEN VF.PRODSIZEMTR ELSE DBO.F_GETSTOCKACTUALSIZE(CN.STOCKNO,EI.MASTERCOMPANYID) END) 
        ELSE  CASE WHEN MS.VARFINISHINGSIZE=1 THEN VF.SIZEFT
            WHEN MS.VARFINISHINGUNIT=2 THEN VF.PRODSIZEFT ELSE DBO.F_GETSTOCKACTUALSIZE(CN.STOCKNO,EI.MASTERCOMPANYID) end end end End as SIZE, 
        PH.COMMRATE,VF.ITEM_ID,VF.QUALITYID,VF.DESIGNID,VF.COLORID,VF.SHAPEID,VF.SIZEID,PNM.PROCESS_NAME_ID,PNM.SEQNO,OM.LOCALORDER,
        VARDECPLACE=CASE WHEN PNM.PROCESS_NAME='WEAVING' THEN (CASE WHEN PH.UNITID=1 THEN ROUNDMTRFLAG ELSE ROUNDFTFLAG END)   
        ELSE (CASE WHEN MS.VARFINISHINGUNIT=1 THEN MS.ROUNDMTRFLAG ELSE MS.ROUNDFTFLAG END) END,
        '' RECEIVEDATE,VF.SIZEFT,
        (SELECT DISTINCT COUNT(EMP.EMPID) FROM Employee_HomeFurnishingReceiveMaster EMP  WHERE EMP.PROCESSID=PH.PROCESSID 
        AND EMP.ProcessRecDetailID = CN.Process_Rec_Detail_ID) AS NOOFEMPLOYEES,
        Case When MS.VARCOMPANYNO = 16 Then 
			        Case When PH.ProcessID in (2, 73) Then 
				        Case When PH.UNITID = 2 Then 'FT' Else 'Mtr' End 
				        Else Case When PH.ProcessID in (107) Then 
					        Case When PH.UNITID = 2 Then 'Sq YD' Else 'Sq Mtr' End
						        Else 
							        Case When PH.CalType = 1 Then 'PCS' 
								        Else Case When PH.UNITID = 2 Then 'Sq YD' Else 'Sq Mtr' End 
							        END
					        End
				        END
			        Else Case When MS.VARCOMPANYNO = 28 Then 
			        Case When PH.ProcessID in (4, 8) Then 
				        Case When PH.UNITID = 2 Then 'FT' Else 'Mtr' End 
				        Else Case When PH.CalType = 1 Then 'PCS' 
					        Else Case When PH.UNITID = 2 Then 'Sq YD' Else 'Sq Mtr' End 
				        END
			        End
			        Else Case When PH.RATE = PH.AMOUNT Then 'PCS' 
					        Else Case When PH.UNITID = 2 Then 'Sq YD' Else 'Sq Mtr' End 
			        End
        End End UnitName 
        FROM HomeFurnishingOrderHissab PH(Nolock) 
        JOIN HomeFurnishingStockNo CN(Nolock) ON PH.STOCKNO = CN.STOCKNO 
        INNER JOIN  COMPANYINFO CI(Nolock) ON PH.COMPANYID=CI.COMPANYID 
        INNER JOIN V_FINISHEDITEMDETAIL VF(Nolock) ON PH.FINISHEDID=VF.ITEM_FINISHED_ID
        INNER JOIN EMPINFO EI(Nolock) ON PH.EMPID=EI.EMPID
        INNER JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PH.PROCESSID = PNM.PROCESS_NAME_ID
        INNER JOIN MASTERSETTING MS(Nolock) ON VF.MASTERCOMPANYID=MS.VARCOMPANYNO
        LEFT JOIN ORDERMASTER OM(Nolock) ON CN.ORDERID=OM.ORDERID
        Where PH.HissabNo=" + ViewState["Hissab_No"];

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand(str, con);
        cmd.CommandTimeout = 300;

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
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
            param[3] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteHomeFurnishingOrderHissab", param);
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
            }
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
    protected void btnprintvoucher_Click(object sender, EventArgs e)
    {
        Session["rptFileName"] = "Reports/rptvoucher.rpt";

        string str = @"SELECT CI.COMPANYID,CI.COMPANYNAME,CI.COMPADDR1,CI.COMPADDR2,CI.COMPADDR3,CI.COMPTEL,CI.GSTNO,
            PH.HISSABNO AS SLIPNO,EI.EMPID,EI.EMPNAME + CASE WHEN ISNULL(EI.EMPCODE,'')='' THEN '' ELSE ' ['+EI.EMPCODE+']' END AS EMPNAME,PNM.PROCESS_NAME,PH.FROMDATE,
            PH.TODATE,ROUND(SUM(AMOUNT-ISNULL(PENALITY,0)),2) AS AMOUNT, IsNull(PH.TDS, 0) TDS, ROUND(SUM(AMOUNT-ISNULL(PENALITY,0))*IsNull(PH.TDS, 0)*0.01, 2) AS TDSAMOUNT, 
            PH.ID, 0 AdvanceAmount, 0 BonusAmt, 0 AdditionAmt, 0 DeductionAmt 
            FROM HomeFurnishingOrderHissab PH (Nolock) 
            JOIN COMPANYINFO CI (Nolock) ON PH.COMPANYID=CI.COMPANYID
            JOIN EMPINFO EI(Nolock) ON PH.EMPID=EI.EMPID
            JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PH.PROCESSID=PNM.PROCESS_NAME_ID
            Where PH.HISSABNO = " + ViewState["Hissab_No"] + @"
            GROUP BY CI.COMPANYID,CI.COMPANYNAME,CI.COMPADDR1,CI.COMPADDR2,CI.COMPADDR3,CI.COMPTEL,CI.GSTNO,
            PH.HISSABNO,PH.ID,EI.EMPID,EI.EMPNAME,EI.EMPCODE,PNM.PROCESS_NAME,PH.FROMDATE,PH.TODATE,PH.TDS ";            

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
    protected void TxtToDate_TextChanged(object sender, EventArgs e)
    {
        TxtDate.Text = TxtToDate.Text;
    }
    protected void ButtonBtnSaveAllProcessWise()
    {
        if (Convert.ToInt16(Session["varMasterCompanyIDForERP"]) == 16 && Convert.ToInt16(Session["varuserid"]) == 57 && ChkForEdit.Checked == false)
        {
            BtnSaveAllProcessWise.Visible = true;
        }
        else
        {
            BtnSaveAllProcessWise.Visible = false;
        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        if (Convert.ToDateTime(TxtFromDate.Text) > Convert.ToDateTime(TxtDate.Text) || Convert.ToDateTime(TxtToDate.Text) > Convert.ToDateTime(TxtDate.Text))
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "date", "alert('Slip Date can not be less than From and To Date.');", true);
            return;
        }
        //*************
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand("[Pro_SaveHomeFurnishingOrderHissab]", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

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
            cmd.Parameters.AddWithValue("@ID", 0);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);
            cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
            cmd.Parameters.AddWithValue("@AdditionAmt", 0);
            cmd.Parameters.AddWithValue("@DeductionAmt", 0);
            cmd.Parameters.AddWithValue("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);

            cmd.ExecuteNonQuery();
            ViewState["Hissab_No"] = cmd.Parameters["@HissabNo"].Value.ToString();
            Tran.Commit();

            lblMessage.Visible = true;
            ScriptManager.RegisterStartupScript(Page, GetType(), "date", "alert('Data Inserted Successfully ');", true);
            lblMessage.Text = "Data Inserted Successfully !";
            TxtHissabNo.Text = ViewState["Hissab_No"].ToString();
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
    protected void BtnSaveAllOneTime_Click(object sender, EventArgs e)
    {
        SaveChampo();
    }
    private void SaveChampo()
    {
        string Str = "";
        //*************CHECK DATE
        if (Convert.ToDateTime(TxtFromDate.Text) > Convert.ToDateTime(TxtDate.Text) || Convert.ToDateTime(TxtToDate.Text) > Convert.ToDateTime(TxtDate.Text))
        {

            ScriptManager.RegisterStartupScript(Page, GetType(), "date", "alert('Slip Date can not be less than From and To Date.');", true);
            return;
        }
        //*************
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand("[PRO_PROCESS_HISSAB_NEW]", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

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
            cmd.Parameters.AddWithValue("@ID", 0);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);
            cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
            //cmd.Parameters.AddWithValue("@AdditionAmt", txtAdditionAmt.Text == "" ? "0" : txtAdditionAmt.Text);
            //cmd.Parameters.AddWithValue("@DeductionAmt", txtDeductionAmt.Text == "" ? "0" : txtDeductionAmt.Text);
            cmd.Parameters.AddWithValue("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);

            cmd.ExecuteNonQuery();
            ViewState["Hissab_No"] = cmd.Parameters["@HissabNo"].Value.ToString();
            Tran.Commit();

            lblMessage.Visible = true;
            lblMessage.Text = "Data Inserted Successfully !";
            //ShowDataInGrid();
            TxtHissabNo.Text = ViewState["Hissab_No"].ToString();
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
    protected void BtnSaveAllProcessWise_Click(object sender, EventArgs e)
    {
        if (Convert.ToDateTime(TxtFromDate.Text) > Convert.ToDateTime(TxtDate.Text) || Convert.ToDateTime(TxtToDate.Text) > Convert.ToDateTime(TxtDate.Text))
        {

            ScriptManager.RegisterStartupScript(Page, GetType(), "date", "alert('Slip Date can not be less than From and To Date.');", true);
            return;
        }
        //*************
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand("[PRO_PROCESS_HISSAB_NEW]", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

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
            cmd.Parameters.AddWithValue("@ID", 0);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);
            cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
            //cmd.Parameters.AddWithValue("@AdditionAmt", txtAdditionAmt.Text == "" ? "0" : txtAdditionAmt.Text);
            //cmd.Parameters.AddWithValue("@DeductionAmt", txtDeductionAmt.Text == "" ? "0" : txtDeductionAmt.Text);
            cmd.Parameters.AddWithValue("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);

            cmd.ExecuteNonQuery();
            ViewState["Hissab_No"] = cmd.Parameters["@HissabNo"].Value.ToString();
            Tran.Commit();

            lblMessage.Visible = true;
            ScriptManager.RegisterStartupScript(Page, GetType(), "date", "alert('Data Inserted Successfully ');", true);
            lblMessage.Text = "Data Inserted Successfully !";
            TxtHissabNo.Text = ViewState["Hissab_No"].ToString();
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