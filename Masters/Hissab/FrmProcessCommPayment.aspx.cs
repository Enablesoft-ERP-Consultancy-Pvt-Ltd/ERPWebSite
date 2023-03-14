using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Hissab_FrmProcessCommPayment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            string Str = "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + " order by CompanyName";
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, Ds, 0, true, "--SELECT--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
                CompanyNameSelectedIndexChanged();
            }
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            CheckForEditSelectedChanges();
            ViewState["Hissab_No"] = 0;
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
            Str = "Select Distinct Process_Name_Id,Process_Name from PROCESS_NAME_MASTER PNM,PROCESS_HISSAB PH Where PNM.Process_Name_Id=PH.ProcessId  And PH.CommPaymentFlag=1 And PNM.MasterCompanyId=" + Session["varCompanyId"] + " Order By Process_Name";
        }
        else
        {
            Str = "Select Process_Name_Id,Process_Name from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order By Process_Name";
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
                Str = "Select Distinct PM.EmpId,EI.EmpName From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM,EMpInfo EI,PROCESS_HISSAB PH Where PM.CompanyId=" + DDCompanyName.SelectedValue + " And PM.EmpId=EI.EmpId And PM.EmpId=PH.EmpID And PH.CommPaymentFlag=1 AND EI.MasterCompanyId=" + Session["varCompanyId"] + " Order By EI.EmpName";
            }
            else
            {
                Str = "Select Distinct PM.EmpId,EI.EmpName From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM,EMpInfo EI Where CompanyId=" + DDCompanyName.SelectedValue + " And PM.EmpId=EI.EmpId And EI.MasterCompanyId=" + Session["varCompanyId"] + " Order By EI.EmpName";
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
        string Str = @"Select Distinct IssueOrderID,isnull(ChallanNo,IssueOrderID) as ChallanNo From View_ProcessIssueReceiveDetailNew Where CompanyId=" + DDCompanyName.SelectedValue + " And ProcessID=" + DDProcessName.SelectedValue + " And Empid=" + DDEmployerName.SelectedValue;
        if (ChkForEdit.Checked == true)
        {
            Str = Str + " And IssueOrderID in (Select ProcessOrderNo from PROCESS_HISSAB Where CommPaymentFlag=1)";
        }
        else
        {
            Str = Str + @" And IssueOrderID not in (Select Distinct IssueOrderID From View_ProcessIssueReceiveDetailNew Where ProcessId=" + DDProcessName.SelectedValue + @"
                       Group By IssueOrderID,Item_Finished_id Having Sum(Qty)>Sum(RecQty)) And IssueOrderID not in (Select ProcessOrderNo from PROCESS_HISSAB Where CommPaymentFlag=1)";

        }
        Str = Str + @" Select Distinct HissabNo,HissabNo from PROCESS_HISSAB Where CommPaymentFlag=1 And CompanyId=" + DDCompanyName.SelectedValue + " And ProcessID=" + DDProcessName.SelectedValue + " And Empid=" + DDEmployerName.SelectedValue;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref DDProcessOrderNo, Ds, 0, true, "--SELECT--");
        if (DDProcessName.SelectedIndex > 0 && ChkForEdit.Checked == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref DDSlipNo, Ds, 1, true, "--SELECT--");
        }
        ViewState["Hissab_No"] = 0;
    }
    protected void DDProcessOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    private void Fill_Grid()
    {

////        string Str = @"Select ITEM_NAME Item ,QualityName+' '+designName+' '+ColorName+' '+ShapeName+' '+Case When PM.UnitId=1 Then ProdSizeMtr Else ProdSizeFt End+' '+ShadeColorName Description,
////                       PD.Item_Finished_id Sr_No,Sum(Qty) Qty,ROUND(SUM(QTY*AREA),4) as Area,Comm Rate,ROUND(Sum(CommAmt),2) Amount,UnitId,TDSPercentage,0 Flag,1 as commpaymentflag,Unitid
////                       From View_Process_Receive_Master PM,View_Process_Receive_Detail PD,V_FinishedItemDetail VF 
////                       Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=VF.Item_Finished_id  and PM.Processid=" + DDProcessName.SelectedValue;


        string Str = @"Select ITEM_NAME Item ,QualityName+' '+designName+' '+ColorName+' '+ShapeName+' '+Case When PM.UnitId=1 Then ProdSizeMtr Else ProdSizeFt End+' '+ShadeColorName Description,
                       PD.Item_Finished_id Sr_No,Sum(Qty) Qty,ROUND(SUM(QTY*AREA),4) as Area,Comm Rate,ROUND(Sum(CommAmt),2) Amount,UnitId,TDSPercentage,0 Flag,1 as commpaymentflag,Unitid
                       From View_Process_Receive_Master PM(NoLock) 
                        JOIN View_Process_Receive_Detail PD(NoLock) ON PM.PROCESS_REC_ID=PD.PROCESS_REC_ID and PM.PROCESSID=PD.PROCESS_NAME_ID
                        JOIN V_FinishedItemDetail VF(NoLock) ON PD.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID 
                       Where PM.Processid=" + DDProcessName.SelectedValue;
        if (ChkForEdit.Checked == true)
        {
            Str = Str + " And PD.IssueOrderId in (Select ProcessOrderNo from PROCESS_HISSAB Where CommPaymentFlag=1 And HissabNo=" + DDSlipNo.SelectedValue + ")";
            Str = Str + "  And PD.Process_Rec_Detail_Id in(select Process_Rec_Detail_Id From View_Process_Receive_Detail Where IssueOrderId=" + DDProcessOrderNo.SelectedValue + " And  QualityType<>3 and PROCESS_NAME_ID=PM.PROCESSID)";
        }
        else
        {
            Str = Str + " And PD.IssueOrderId=" + DDProcessOrderNo.SelectedValue;
            Str = Str + " And PD.Process_Rec_Detail_Id in(select Process_Rec_Detail_Id From View_Process_Receive_Detail Where IssueOrderId=" + DDProcessOrderNo.SelectedValue + " And  QualityType<>3 and PROCESS_NAME_ID=PM.PROCESSID)";
        }
        Str = Str + " Group By ITEM_NAME,QualityName,designName,ColorName,ShapeName,PM.UnitId,ProdSizeMtr,ProdSizeFt,ShadeColorName,PD.Item_Finished_id,Comm,UnitId,TDSPercentage";

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        DGDetail.DataSource = Ds;
        DGDetail.DataBind();
    }
    protected void DDSlipNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        SlipNoSelectedChange();
    }
    private void SlipNoSelectedChange()
    {
        string Str = "Select ProcessOrderNo,HissabNo,ChallanNo,replace(convert(varchar(11),Date,106), ' ','-') as Date from PROCESS_HISSAB Where CommPaymentFlag=1 And HissabNo=" + DDSlipNo.SelectedValue;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            ViewState["Hissab_No"] = Ds.Tables[0].Rows[0]["HissabNo"];
            TxtHissabNo.Text = Ds.Tables[0].Rows[0]["ChallanNo"].ToString();
            TxtDate.Text = Ds.Tables[0].Rows[0]["Date"].ToString();
            DDProcessOrderNo.SelectedValue = Ds.Tables[0].Rows[0]["ProcessOrderNo"].ToString();
        }
        Fill_Grid();
    }
    protected void TxtSlipNo_TextChanged(object sender, EventArgs e)
    {
        lblMessage.Visible = false;
        if (TxtSlipNo.Text != "")
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select CompanyId,ProcessID,EmpId,ProcessOrderNo,HissabNo,
                    replace(convert(varchar(11),Date,106), ' ','-') as Date,ChallanNo 
                    From PROCESS_HISSAB Where CommPaymentFlag=1 And CompanyId = " + DDCompanyName.SelectedValue + " And HissabNo=" + TxtSlipNo.Text + "");

            if (Ds.Tables[0].Rows.Count > 0)
            {
                //DDCompanyName.SelectedValue = Ds.Tables[0].Rows[0]["CompanyId"].ToString();
                //CompanyNameSelectedIndexChanged();
                DDProcessName.SelectedValue = Ds.Tables[0].Rows[0]["ProcessID"].ToString();
                ProcessNameSelectedIndexChanged();
                DDEmployerName.SelectedValue = Ds.Tables[0].Rows[0]["EmpId"].ToString();
                EmployerNameSelectedIndexChanged();
                TxtHissabNo.Text = "";
                DDSlipNo.SelectedValue = Ds.Tables[0].Rows[0]["HissabNo"].ToString();
                SlipNoSelectedChange();
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
            TxtSlipNo.Focus();
            DDProcessOrderNo.Enabled = false;
        }
        else
        {
            DDProcessOrderNo.Enabled = true;
            TDSlipNoForEdit.Visible = false;
            TDDDSlipNo.Visible = false;
            TxtSlipNo.Text = "";
        }
        CompanyNameSelectedIndexChanged();
        DDSlipNo.Items.Clear();
        DDEmployerName.Items.Clear();
        DDProcessOrderNo.Items.Clear();
        DGDetail.DataSource = null;
        DGDetail.DataBind();
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        int VarID = 0;

        DataTable dt = new DataTable();
        dt.Columns.Add("Item_Finished_id", typeof(int));
        dt.Columns.Add("Qty", typeof(int));
        dt.Columns.Add("Rate", typeof(float));
        dt.Columns.Add("Area", typeof(float));
        dt.Columns.Add("Amount", typeof(float));
        dt.Columns.Add("unitid", typeof(int));
        dt.Columns.Add("Tdspercentage", typeof(int));
        dt.Columns.Add("commpaymentflag", typeof(int));

        for (int i = 0; i < DGDetail.Rows.Count; i++)
        {

            DataRow dr = dt.NewRow();
            Label lblitemfinishedid = (Label)DGDetail.Rows[i].FindControl("lblitemfinishedid");
            Label lblqty = (Label)DGDetail.Rows[i].FindControl("lblqty");
            Label lblrate = (Label)DGDetail.Rows[i].FindControl("lblrate");
            Label lblarea = (Label)DGDetail.Rows[i].FindControl("lblarea");
            Label lblamount = (Label)DGDetail.Rows[i].FindControl("lblamount");
            Label lblunitid = (Label)DGDetail.Rows[i].FindControl("lblunitid");
            Label lbltdspercentage = (Label)DGDetail.Rows[i].FindControl("lbltdspercentage");
            Label lblcommpaymentflag = (Label)DGDetail.Rows[i].FindControl("lblcommpaymentflag");

            dr["Item_Finished_id"] = lblitemfinishedid.Text;
            dr["Qty"] = lblqty.Text;
            dr["Rate"] = lblrate.Text;
            dr["Area"] = lblarea.Text;
            dr["Amount"] = lblamount.Text;
            dr["unitid"] = lblunitid.Text;
            dr["Tdspercentage"] = lbltdspercentage.Text == "" ? "0" : lbltdspercentage.Text;
            dr["commpaymentflag"] = lblcommpaymentflag.Text;

            dt.Rows.Add(dr);
        }
        if (dt.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('No records found in Data Grid !!')", true);
            return;
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {


            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter("@HissabNo", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = 0;
            param[1] = new SqlParameter("@Companyid", DDCompanyName.SelectedValue);
            param[2] = new SqlParameter("@Empid", DDEmployerName.SelectedValue);
            param[3] = new SqlParameter("@processid", DDProcessName.SelectedValue);
            param[4] = new SqlParameter("@Processorderno", DDProcessOrderNo.SelectedValue);
            param[5] = new SqlParameter("@date", TxtDate.Text);
            param[6] = new SqlParameter("@dt", dt);
            param[7] = new SqlParameter("@userid", Session["varuserid"]);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PROCESSCOMMHISSAB", param);

            #region comment
            //            string Str = @"Select Isnull(Max(HissabNo),0)+1 HissabNo from PROCESS_HISSAB 
            //                           Select PD.Item_Finished_id,Sum(Qty) Qty,Sum(Area*Qty) Area,Comm Rate,Sum(CommAmt) Amount,UnitId,TDSPercentage TDS 
            //                           From View_Process_Receive_Master PM,View_Process_Receive_Detail PD Where PM.Process_Rec_Id=PD.Process_Rec_Id AND PM.Processid=PD.process_name_id And QualityType<>3 And 
            //                           PD.IssueOrderId=" + DDProcessOrderNo.SelectedValue + @" and PM.Processid=" + DDProcessName.SelectedValue + @" Group By PD.Item_Finished_id,Comm,UnitId,TDSPercentage
            //                           Select Isnull(Max(ID),0)+1  ID from PROCESS_HISSAB";

            //            DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
            //            int VarHissabNo = Convert.ToInt32(Ds.Tables[0].Rows[0]["HissabNo"]);
            //            VarID = Convert.ToInt32(Ds.Tables[2].Rows[0]["ID"]);

            //            if (Ds.Tables[1].Rows.Count > 0)
            //            {
            //                for (int i = 0; i < Ds.Tables[1].Rows.Count; i++)
            //                {
            //                    string StrNew = @"Insert into PROCESS_HISSAB(ID,CompanyId,ProcessID,EmpId,ProcessOrderNo,HissabNo,Date,Finishedid,StockNo,Qty,Area,Rate,Amount,Weight,Penality,PRemarks,ChallanNo,UnitId,TDS,ReqWeight,CommPaymentFlag) 
            //                          Select " + VarID + " ID," + DDCompanyName.SelectedValue + " CompanyId," + DDProcessName.SelectedValue + @" ProcessId,
            //                          " + DDEmployerName.SelectedValue + " EmpID,PD.IssueOrderId ProcessOrderNo," + VarHissabNo + " ,'" + TxtDate.Text + @"' Date,
            //                          PD.Item_Finished_id,'' StockNo,Sum(Qty) Qty,Sum(Area*Qty) Area,Comm Rate,Sum(CommAmt) Amount,0 Weight,0 Penality,'' PRemarks,'" + VarHissabNo + @"' ChallanNo,
            //                          UnitId,TDSPercentage TDS,0 ReqWeight,1 CommPaymentFlag From View_Process_Receive_Master PM,View_Process_Receive_Detail PD 
            //                          Where PM.Process_Rec_Id=PD.Process_Rec_Id and PM.processid=PD.Process_name_id And QualityType<>3 And PD.Item_Finished_id=" + Ds.Tables[1].Rows[i]["Item_Finished_id"] + @" And 
            //                          PD.IssueOrderId=" + DDProcessOrderNo.SelectedValue + @" and PM.Processid=" + DDProcessName.SelectedValue + " Group By PD.Item_Finished_id,Comm,UnitId,TDSPercentage,PD.IssueOrderId";
            //                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, StrNew);
            //                    VarID = VarID + 1;
            //                }
            //            }
            #endregion
            Tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Data successfully saved!');", true);
            EmployerNameSelectedIndexChanged();
            ViewState["Hissab_No"] = param[0].Value.ToString();
            TxtHissabNo.Text = param[0].Value.ToString();
            DGDetail.DataSource = null;
            DGDetail.DataBind();

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
    protected void BtnPriview_Click(object sender, EventArgs e)
    {
        Session["ReportPath"] = "Reports/RptProcessHissabSummary.rpt";
        Session["CommanFormula"] = "{VIEW_PROCESS_HISSAB.HissabNo}= " + ViewState["Hissab_No"].ToString() + "";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
    }
}