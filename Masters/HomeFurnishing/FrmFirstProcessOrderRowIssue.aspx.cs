using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_HomeFurnishing_FrmFirstProcessOrderRowIssue : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        DataSet DSQ = null; string Qry = "";
        if (!IsPostBack)
        {
            ViewState["PRMID"] = 0;
            Qry = @" Select Distinct CI.CompanyId,Companyname 
                From Companyinfo CI(Nolock) 
                JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By Companyname 

                Select PROCESS_NAME_ID, PROCESS_NAME From PROCESS_NAME_MASTER PNM(Nolock) 
                Where PROCESS_NAME in ('BATHMAT DYEING', 'STITCHING') And MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @" Order By Process_Name ";

            DSQ = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, DSQ, 0, true, "--Select--");
            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref ddProcessName, DSQ, 1, true, "--Select--");
            ddProcessName.SelectedIndex = 1;
            ddProcessName_SelectedIndexChanged(sender, new EventArgs());
            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            if (variable.Carpetcompany == "1")
            {
                TRempcodescan.Visible = true;
            }

            if (variable.VarCompanyWiseChallanNoGenerated == "1")
            {
                txtchalanno.Enabled = false;
            }
        }
    }
    protected void TxtPOrderNo_TextChanged(object sender, EventArgs e)
    {
        LblError.Text = "";
        String VarPOrderNo = TxtPOrderNo.Text == "" ? "0" : TxtPOrderNo.Text;

        string str = @" SELECT (Select Top 1 EmpID From Employee_ProcessOrderNo EPO(Nolock) 
                                                    Where EPO.IssueOrderId = HMOM.IssueOrderId And EPO.ProcessId = " + ddProcessName.SelectedValue + @") EMPID, 
                    HMOM.ISSUEORDERID, IsNull(HMOM.CHALLANNO, 0) CHALLANNO 
                    FROM PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" HMOM(Nolock)
                    Where HMOM.STATUS <> 'CANCELED' And HMOM.COMPANYID = " + ddCompName.SelectedValue + " And HMOM.ChallanNo='" + VarPOrderNo + "'";

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (Ds.Tables[0].Rows.Count > 0)
        {
            ProcessNameSelectedIndexChange();
            ddempname.SelectedValue = Ds.Tables[0].Rows[0]["EmpId"].ToString();
            EmpNameSelectedIndexChange();
            if (ddOrderNo.Items.FindByValue(Ds.Tables[0].Rows[0]["ISSUEOrderId"].ToString()) != null)
            {
                ddOrderNo.SelectedValue = Ds.Tables[0].Rows[0]["IssueOrderId"].ToString();
            }
            else
            {
                LblError.Text = "This Po No. does not exists or Sample Po.";
                LblError.Visible = true;
                return;

            }
            OrderNoSelectedIndexChange();
            if (DDChallanNo.Items.Count > 0)
            {
                DDChallanNo.SelectedIndex = 1;
                ChallanNoSelectedIndexChange();
            }
        }
        else
        {
            if (ddOrderNo.Items.Count > 0)
            {
                ddOrderNo.SelectedIndex = 0;
                fill_Grid_ShowConsmption();
            }
            TxtPOrderNo.Text = "";
            TxtPOrderNo.Focus();
        }
    }
    protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    {
        FillProcess_Employee(sender);
    }
    protected void FillProcess_Employee(object sender = null)
    {
        string str = @"SELECT Top(1) EMP.ProcessId, EI.EmpId 
                FROM Employee_PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" EMP(Nolock) 
                JOIN EMPINFO EI ON EMP.EMPID = EI.EMPID 
                WHERE EI.EMPCODE = '" + txtWeaverIdNoscan.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ddProcessName.Items.FindByValue(ds.Tables[0].Rows[0]["Processid"].ToString()) != null)
            {
                ddProcessName.SelectedValue = ds.Tables[0].Rows[0]["Processid"].ToString();
                if (sender != null)
                {
                    ddProcessName_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            if (ddempname.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) != null)
            {
                ddempname.SelectedValue = ds.Tables[0].Rows[0]["Empid"].ToString();
                if (sender != null)
                {
                    ddempname_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            ddOrderNo.Focus();
        }
        else
        {
            ddProcessName.SelectedIndex = -1;
            ddOrderNo.SelectedIndex = -1;
            ScriptManager.RegisterStartupScript(Page, GetType(), "fillemp", "alert('Please Enter correct Emp. Code or No entry from this employee')", true);

        }
    }
    protected void ddProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessNameSelectedIndexChange();
    }
    private void ProcessNameSelectedIndexChange()
    {
        ViewState["PRMID"] = 0;
        Fill_Grid();
        string str = @"Select Distinct EI.EmpID,  EI.EmpName + ' / ' + EI.EmpCode EmpName 
        From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" a(Nolock) 
        JOIN Employee_ProcessOrderNo EMP(Nolock) ON EMP.IssueOrderID = a.ISSUEORDERID And EMP.ProcessID = " + ddProcessName.SelectedValue + @"
        JOIN Empinfo EI(Nolock) ON EI.EmpID = EMP.EmpID 
        Where a.COMPANYID = " + ddCompName.SelectedValue + @" 
        Order By EmpName ";

        UtilityModule.ConditionalComboFill(ref ddempname, str, true, "--Select--");
    }
    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtchalanno.Text = "";
        EmpNameSelectedIndexChange();
    }
    private void EmpNameSelectedIndexChange()
    {
        ViewState["PRMID"] = 0;
        Fill_Grid();
         
        string str = @"Select Distinct a.ISSUEORDERID, a.CHALLANNO 
        From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" a(Nolock) 
        JOIN Employee_ProcessOrderNo EMP(Nolock) ON EMP.IssueOrderID = a.ISSUEORDERID And EMP.ProcessID = " + ddProcessName.SelectedValue + " And EMP.EmpID = " + ddempname.SelectedValue + @" 
        Where a.COMPANYID = " + ddCompName.SelectedValue + @" 
         And a.STATUS = 'PENDING' 
        Order By a.ISSUEORDERID Desc";
        UtilityModule.ConditionalComboFill(ref ddOrderNo, str, true, "Select order no");
    }
    protected void ddOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        OrderNoSelectedIndexChange();
    }
    private void OrderNoSelectedIndexChange()
    {
        string sp = string.Empty;
        if (ChKForEdit.Checked == true)
        {
            EditCheckedChanged();
        }

        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@ProcessID", ddProcessName.SelectedValue);
        param[1] = new SqlParameter("@ISSUEORDERID", ddOrderNo.SelectedValue);
        param[2] = new SqlParameter("@MASTERCOMPANYID", Session["varMasterCompanyIDForERP"]);
        if (Session["varMasterCompanyIDForERP"].ToString() == "44")
        {
            sp = "PRO_GetHomeFurnishingReceiveChallanNoagni";
        }
        else
        {
            sp = "PRO_GetHomeFurnishingReceiveChallanNonew";

        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, sp, param);
        UtilityModule.ConditionalComboFillWithDS(ref DDRecChallanNo, ds, 0, true, "--Select--");

        //fill_Grid_ShowConsmption();
    }

    protected void DDRecChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_Grid_ShowConsmption();
    }
    protected void ChKForEdit_CheckedChanged(object sender, EventArgs e)
    {
        EditCheckedChanged();

        if (Session["usertype"].ToString() != "1" && variable.VarOnlyPreviewButtonShowOnAllEditForm == "1")
        {
            btnsave.Visible = false;
            gvdetail.Columns[8].Visible = false;
        }
    }
    private void EditCheckedChanged()
    {
        Td7.Visible = false;
        if (ChKForEdit.Checked == true)
        {
            Td7.Visible = true;
            if (ddOrderNo.Items.Count > 0)
            {
                UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select PrmId, ChallanNo + ' / ' + REPLACE(CONVERT(NVARCHAR(11), Date, 106), ' ', '-') Challan 
                From FirstProcessOrderRowIssueMaster(Nolock) 
                Where TranType = 0 And CompanyID = " + ddCompName.SelectedValue + " And ProcessID = " + ddProcessName.SelectedValue + @" And 
                IssueOrderID = " + ddOrderNo.SelectedValue + " And MasterCompanyId = " + Session["varMasterCompanyIDForERP"], true, "Select Challan No");
            }
        }
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChallanNoSelectedIndexChange();
    }
    private void ChallanNoSelectedIndexChange()
    {
        ViewState["PRMID"] = 0;
        txtchalanno.Text = "";
        if (DDChallanNo.SelectedIndex > 0)
        {
            ViewState["PRMID"] = DDChallanNo.SelectedValue;

            string strsql2 = @"Select PRMID, ChallanNo  
            From FirstProcessOrderRowIssueMaster PRM(Nolock) 
            Where PRM.Prmid = " + DDChallanNo.SelectedValue + " And PRM.ProcessID = " + ddProcessName.SelectedValue + " And PRM.MasterCompanyId = " + Session["varMasterCompanyIDForERP"];
            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql2);

            if (ds2.Tables[0].Rows.Count > 0)
            {
                txtchalanno.Text = ds2.Tables[0].Rows[0]["ChallanNo"].ToString();
            }
        }
        Fill_Grid();
    }
    protected void DuplicateChallanNo()
    {
        LblError.Text = "";
        LblError.Visible = true;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            if (txtchalanno.Text != "")
            {
                string str = "Select ChallanNo From FirstProcessOrderRowIssueMaster Where ChallanNo<>'' And TranType=0 And ChallanNo='" + txtchalanno.Text + "' And PRMID<>" + ViewState["PRMID"] + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblError.Text = "Challan no. already exists.....";
                }
            }
        }
        catch (Exception ex)
        {
            LblError.Visible = true;
            LblError.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        LblError.Text = "";
        if (LblError.Text == "")
        {
            if (variable.VarMANYFOLIORAWISSUE_SINGLECHALAN == "0")
            {
                DuplicateChallanNo();
            }
        }
        string StrDetail = "";

        for (int i = 0; i < DG.Rows.Count; i++)
        {
            Label lblOrderID = (Label)DG.Rows[i].FindControl("LblOrderID");
            Label lblOrder_FinishedID = (Label)DG.Rows[i].FindControl("LblOrder_FinishedID");
            Label lblOrderDetailDetail_FinishedID = (Label)DG.Rows[i].FindControl("LblOrderDetailDetail_FinishedID");
            TextBox TxtQty = (TextBox)DG.Rows[i].FindControl("TxtQty");
            Label LblStockQty = (Label)DG.Rows[i].FindControl("LblStockQty");
            Label LblPendQty = (Label)DG.Rows[i].FindControl("LblPendQty");

            if (Convert.ToDouble(TxtQty.Text == "" ? "0" : TxtQty.Text) > 0 && Convert.ToDouble(LblStockQty.Text) >= Convert.ToDouble(TxtQty.Text == "" ? "0" : TxtQty.Text) && Convert.ToDouble(LblPendQty.Text) >= Convert.ToDouble(TxtQty.Text == "" ? "0" : TxtQty.Text))
            {
                if (StrDetail == "")
                {
                    StrDetail = lblOrderID.Text + '|' + lblOrder_FinishedID.Text + '|' + lblOrderDetailDetail_FinishedID.Text + '|' + TxtQty.Text + '~';
                }
                else
                {
                    StrDetail = StrDetail + lblOrderID.Text + '|' + lblOrder_FinishedID.Text + '|' + lblOrderDetailDetail_FinishedID.Text + '|' + TxtQty.Text + '~';
                }
            }
        }
        if (StrDetail == "")
        {
            LblError.Text = "Please check qty in gridview";
            LblError.Visible = true;
            return;
        }
        if (LblError.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] arr = new SqlParameter[36];

                arr[0] = new SqlParameter("@PRMID", SqlDbType.Int);
                arr[1] = new SqlParameter("@COMPANYID", SqlDbType.Int);
                arr[2] = new SqlParameter("@PROCESSID", SqlDbType.Int);
                arr[3] = new SqlParameter("@ISSUEORDERID", SqlDbType.Int);
                arr[4] = new SqlParameter("@ISSUEDATE", SqlDbType.DateTime);
                arr[5] = new SqlParameter("@CHALLANNO", SqlDbType.NVarChar, 150);
                arr[6] = new SqlParameter("@TRANTYPE", SqlDbType.Int);
                arr[7] = new SqlParameter("@DETAILDATA", SqlDbType.NVarChar, 4000);
                arr[8] = new SqlParameter("@USERID", SqlDbType.Int);
                arr[9] = new SqlParameter("@MASTERCOMPANYID", SqlDbType.Int);
                arr[10] = new SqlParameter("@REMARKS", SqlDbType.VarChar, 1000);
                arr[11] = new SqlParameter("@MSG", SqlDbType.NVarChar, 4000);

                arr[0].Value = ViewState["PRMID"];
                arr[0].Direction = ParameterDirection.InputOutput;
                arr[1].Value = ddCompName.SelectedValue;
                arr[2].Value = ddProcessName.SelectedValue;
                arr[3].Value = ddOrderNo.SelectedValue;
                arr[4].Value = txtdate.Text;
                arr[5].Value = txtchalanno.Text;
                arr[5].Direction = ParameterDirection.InputOutput;
                arr[6].Value = 0;
                arr[7].Value = StrDetail;
                arr[8].Value = Session["varuserid"].ToString();
                arr[9].Value = Session["varMasterCompanyIDForERP"].ToString();
                arr[10].Value = txtremarks.Text;
                arr[11].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_FirstProcessOrderRowIssue", arr);

                Tran.Commit();
                txtchalanno.Text = arr[5].Value.ToString();
                ViewState["PRMID"] = arr[0].Value;
                LblError.Visible = true;
                LblError.Text = arr[11].Value.ToString();
                Fill_Grid();
                fill_Grid_ShowConsmption();
                SaveReferece();
                btnsave.Text = "Save";
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                LblError.Visible = true;
                LblError.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    private void SaveReferece()
    {

    }
    private void Fill_Grid()
    {
        gvdetail.DataSource = fill_Data_grid();
        gvdetail.DataBind();
    }
    private DataSet fill_Data_grid()
    {
        DataSet ds = null;
        string strsql = "";
        try
        {
            if (Convert.ToInt32(ViewState["PRMID"]) != 0)
            {
                strsql = @"Select PrtId,CATEGORY_NAME,ITEM_NAME,QualityName+ Space(2)+DesignName+ Space(2)+ColorName+ Space(2)+ShapeName+ Space(2)+SizeFt+ Space(2)+ShadeColorName DESCRIPTION,
                            PT.Qty 
                            From FirstProcessOrderRowIssueTran PT
                            JOIN V_FinishedItemDetail VF ON VF.Item_Finished_id = PT.OrderDetailDetail_FinishedID And VF.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" 
                            Where PT.PrmID=" + ViewState["PRMID"];
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            }
        }
        catch (Exception ex)
        {
            LblError.Visible = true;
            LblError.Text = ex.Message;
            Logs.WriteErrorLog("Masters/HomeFurnishing/FrmFirstProcessOrderRowIssue|fill_Data_grid|" + ex.Message);
        }
        return ds;
    }
    protected void txtchalan_ontextchange(object sender, EventArgs e)
    {
        string ChalanNo = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
            @"Select IsNull(ChallanNo, 0) asd From FirstProcessOrderRowIssueMaster Where ChallanNo = '" + txtchalanno.Text + "' And MasterCompanyId = " + Session["varMasterCompanyIDForERP"]));
        if (ChalanNo != "")
        {
            txtchalanno.Text = "";
            txtchalanno.Focus();
            LblError.Visible = true;
            LblError.Text = "Challan No already exist";
        }
        else
        {
            LblError.Visible = false;
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
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Session.Remove("prmid");
        Session.Remove("finishedid");
        Session.Remove("inhand");
        Session.Remove("stocktranid");
        Session.Remove("stockid");
    }

    private void WayChallanFormatReport()
    {
        SqlParameter[] _array = new SqlParameter[3];
        _array[0] = new SqlParameter("@prmId", SqlDbType.Int);
        _array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
        _array[2] = new SqlParameter("@Trantype", SqlDbType.Int);

        _array[0].Value = ViewState["PRMID"];
        _array[1].Value = ddProcessName.SelectedValue;
        _array[2].Value = 0; //For Issue

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_RawMaterialIssuedDetail_Hafizia", _array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptRawMaterialIssueWayChallanHafizia.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRawMaterialIssueWayChallanHafizia.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    private void WayChallanFormatBackReport()
    {
        SqlParameter[] _array = new SqlParameter[3];
        _array[0] = new SqlParameter("@prmId", SqlDbType.Int);
        _array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
        _array[2] = new SqlParameter("@Trantype", SqlDbType.Int);

        _array[0].Value = ViewState["PRMID"];
        _array[1].Value = ddProcessName.SelectedValue;
        _array[2].Value = 0; //For Issue

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_RawMaterialIssuedWayChallanBackFormat_Hafizia", _array);

        if (ds.Tables[1].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptRawMaterialIssueWayChallanBackFormatHafizia.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRawMaterialIssueWayChallanBackFormatHafizia.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }

    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str=string.Empty;

        if (Session["varMasterCompanyIDForERP"].ToString() == "44")
        {
            str = @" Select PM.Date, PM.ChallanNo ChalanNo, PM.trantype, PT.Qty IssueQuantity, '' Lotno, '' GodownName, 
                (Select Distinct EII.EmpName + ', ' 
                From Employee_ProcessOrderNo EPO(Nolock) 
                JOIN Empinfo EII(Nolock) ON EII.EmpID = EPO.EmpID And EPO.IssueOrderId = PM.IssueOrderId And EPO.ProcessId = PM.Processid) EmpName, 
		                '' Address, CI.CompanyName, CI.CompAddr1, CI.CompAddr2, 
                CI.CompAddr3, CI.CompTel,vf.CATEGORY_NAME, vf.ITEM_NAME, vf.QualityName, vf.designName, 
                vf.ColorName, vf.ShadeColorName, vf.ShapeName, vf.SizeMtr, PNM.PROCESS_NAME, 
                PM.IssueOrderID Prorderid, '' empgstin, CI.GSTNo, '' TAGNO, '' BINNO, 
                (Select Distinct CII.CustomerCode + ', '
		                From FirstProcessOrderRowIssueTran PID(Nolock) 
		                JOIN OrderMaster OM(Nolock) ON OM.OrderiD = PID.OrderiD 
                        JOIN CustomerInfo CII(Nolock) ON CII.CustomerID = OM.CustomerID 
                        Where PID.PRMID = PM.PRMID For XML Path('')) OrderNo,(Select Distinct cast(OM.OrderId as varchar) + ', '
		                From FirstProcessOrderRowIssueTran PID(Nolock) 
		                JOIN OrderMaster OM(Nolock) ON OM.OrderiD = PID.OrderiD 
                        JOIN CustomerInfo CII(Nolock) ON CII.CustomerID = OM.CustomerID 
                        Where PID.PRMID = PM.PRMID For XML Path('')) customerOrderNo,PM.REMARKS 
                From FirstProcessOrderRowIssueMaster PM 
                join FirstProcessOrderRowIssueTran PT on PM.PRMid=PT.PRMid 
                join CompanyInfo ci on PM.Companyid=ci.CompanyId 
                join V_FinishedItemDetail vf on PT.OrderDetailDetail_FinishedID = vf.ITEM_FINISHED_ID 
                join PROCESS_NAME_MASTER PNM on PM.Processid=PNM.PROCESS_NAME_ID 
                Where PM.Prmid = " + ViewState["PRMID"];
        }
        else {
            str = @" Select PM.Date, PM.ChallanNo ChalanNo, PM.trantype, PT.Qty IssueQuantity, '' Lotno, '' GodownName, 
                (Select Distinct EII.EmpName + ', ' 
                From Employee_ProcessOrderNo EPO(Nolock) 
                JOIN Empinfo EII(Nolock) ON EII.EmpID = EPO.EmpID And EPO.IssueOrderId = PM.IssueOrderId And EPO.ProcessId = PM.Processid) EmpName, 
		                '' Address, CI.CompanyName, CI.CompAddr1, CI.CompAddr2, 
                CI.CompAddr3, CI.CompTel, vf.ITEM_NAME, vf.QualityName, vf.designName, 
                vf.ColorName, vf.ShadeColorName, vf.ShapeName, vf.SizeMtr, PNM.PROCESS_NAME, 
                PM.IssueOrderID Prorderid, '' empgstin, CI.GSTNo, '' TAGNO, '' BINNO, 
                (Select Distinct CII.CustomerCode + ', '
		                From FirstProcessOrderRowIssueTran PID(Nolock) 
		                JOIN OrderMaster OM(Nolock) ON OM.OrderiD = PID.OrderiD 
                        JOIN CustomerInfo CII(Nolock) ON CII.CustomerID = OM.CustomerID 
                        Where PID.PRMID = PM.PRMID For XML Path('')) OrderNo 
                From FirstProcessOrderRowIssueMaster PM 
                join FirstProcessOrderRowIssueTran PT on PM.PRMid=PT.PRMid 
                join CompanyInfo ci on PM.Companyid=ci.CompanyId 
                join V_FinishedItemDetail vf on PT.OrderDetailDetail_FinishedID = vf.ITEM_FINISHED_ID 
                join PROCESS_NAME_MASTER PNM on PM.Processid=PNM.PROCESS_NAME_ID 
                Where PM.Prmid = " + ViewState["PRMID"];
        
        
        }

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "44")
            {
                Session["rptFileName"] = "~\\Reports\\RptRawIssueRecDuplicateNew1agni.rpt";
                
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\RptRawIssueRecDuplicateNew1.rpt";
            
            }
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRawIssueRecDuplicateNew.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true); }
    }

    private void fill_Grid_ShowConsmption()
    {
        DataSet ds = null;
        string sp = string.Empty;
        try
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@ProcessID", ddProcessName.SelectedValue);
            param[1] = new SqlParameter("@ISSUEORDERID", ddOrderNo.SelectedValue);
            param[2] = new SqlParameter("@MASTERCOMPANYID", Session["varMasterCompanyIDForERP"]);
            param[3] = new SqlParameter("@RecChallanNo", DDRecChallanNo.SelectedValue);
            if (Session["varMasterCompanyIDForERP"].ToString() == "44")
            {
                sp = "PRO_FILLFirstProcessOrderRowIssueConsumptionagni";
            }
            else
            {
                sp = "PRO_FILLFirstProcessOrderRowIssueConsumptionnew";
            
            }
            
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_FILLFirstProcessOrderRowIssueConsumptionagni", param);

            DG.DataSource = ds;
            DG.DataBind();
        }
        catch (Exception ex)
        {
            LblError.Visible = true;
            LblError.Text = ex.Message;
            Logs.WriteErrorLog("Masters/HomeFurnishing/FrmFirstProcessOrderRowIssue|fill_Data_grid|" + ex.Message);
        }
    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarPrtID = Convert.ToInt32(gvdetail.DataKeys[e.RowIndex].Value);
            SqlParameter[] arr = new SqlParameter[6];

            arr[0] = new SqlParameter("@PrtID", SqlDbType.Int);
            arr[1] = new SqlParameter("@ProcessID", SqlDbType.Int);
            arr[2] = new SqlParameter("@TranType", SqlDbType.Int);
            arr[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            arr[4] = new SqlParameter("@UserID", Session["varuserid"]);
            arr[5] = new SqlParameter("@MastercompanyID", Session["varMasterCompanyIDForERP"]);

            arr[0].Value = VarPrtID;
            arr[1].Value = ddProcessName.SelectedValue;
            arr[2].Value = 0;
            arr[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_FirstProcessOrderRowIssueDelete", arr);
            if (arr[3].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altdel", "alert('" + arr[3].Value.ToString() + "');", true);
            }
            else
            {
                LblError.Text = "Row Item Deleted successfully.";
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            LblError.Visible = true;
            LblError.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        Fill_Grid();
    }

    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}
