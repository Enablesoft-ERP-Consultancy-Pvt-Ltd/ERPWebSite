using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Hissab_FrmUpdateFinisherJobRate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            string Str = "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by CompanyName
                         Delete TEMP_HISSAB_WISE_CONSUMPTION Where Userid=" + Session["varuserid"] + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);


            Str = "Select Process_Name_Id,Process_Name from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and ProcessType=1 Order By Process_Name";
            UtilityModule.ConditionalComboFill(ref DDProcessName, Str, true, "--SELECT--");
            ViewState["Hissab_No"] = 0;


            TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            // TDRadioButton.Visible = false;
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtDate.Enabled = false;
            CheckForEditSelectedChanges();
            ViewState["Hissab_No"] = 0;
            switch (Convert.ToInt16(Session["varMasterCompanyIDForERP"]))
            {
                case 9: //for Hafizia
                    TDPoOrderNo.Visible = true;
                    //TDsrno.Visible = true;
                    TDQuality.Visible = false;
                    break;
                case 20: //for MaltiRugs
                    TDPoOrderNo.Visible = false;
                    //TDsrno.Visible = true;
                    TDQuality.Visible = true;
                    break;
                default:
                    TDPoOrderNo.Visible = false;
                    // TDsrno.Visible = false;
                    TDQuality.Visible = false;
                    break;
            }
        }
    }
    protected void GridVoucherColumnShowStatus()
    {
        int chkboxExists = 0;
        if (ChkForExist.Checked == true)
        {
            chkboxExists = 1;
        }
        else
        {
            chkboxExists = 0;
        }

        for (int i = 0; i < DGDetail.Columns.Count; i++)
        {
            if (DGDetail.Columns[i].HeaderText == "VoucherNo" || DGDetail.Columns[i].HeaderText == "VoucherDate")
            {

                if (chkboxExists == 0)
                {
                    DGDetail.Columns[i].Visible = false;
                }
                else if (chkboxExists == 1)
                {
                    DGDetail.Columns[i].Visible = true;
                }
            }
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DGDetail.DataSource = null;
        DGDetail.DataBind();
        txttotalpcs.Text = "";
        txttotalarea.Text = "";
        txtamount.Text = "";
        txttotalpenality.Text = "";
        txtTotalPaidAmt.Text = "";
        ProcessNameSelectedIndexChanged();
        //FillSrno();
    }
    private void ProcessNameSelectedIndexChanged()
    {
        if (DDProcessName.SelectedIndex > 0)
        {
            if (TDQuality.Visible == true)
            {
                string Str2 = "";
                Str2 = "Select Distinct VF.QualityId,VF.ITEM_NAME+' '+VF.QualityName as QualityName From PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + " PRD INNER JOIN V_FinishedItemDetailNew VF ON PRD.Item_Finished_Id=VF.ITEM_FINISHED_ID Where VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By VF.QualityId";

                UtilityModule.ConditionalComboFill(ref DDQuality, Str2, true, "--SELECT--");
            }

            string Str = "";
            Str = "Select Distinct PM.EmpId,EI.EmpName From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM INNER JOIN EmpInfo EI ON PM.Empid=EI.EmpId Where EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By EI.EmpName";

            UtilityModule.ConditionalComboFill(ref DDEmployerName, Str, true, "--SELECT--");
            FillReceiptNo();

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
        FillReceiptNo();
        ViewState["Hissab_No"] = 0;
    }
    private void FillReceiptNo()
    {
        string sql = "";
        if (DDProcessName.SelectedIndex > 0)
        {
            sql = @"select Distinct PRM.Process_Rec_Id ,Convert(NvarChar,PRM.Process_Rec_Id )+ Space(5) +  Convert(NvarChar ,PRM.ReceiveDate,106)   from Process_Receive_Master_" + DDProcessName.SelectedValue + " PRM INNER JOIN PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + " PRD ON PRM.Process_Rec_Id=PRD.Process_Rec_Id Where PRM.ReceiveDate>='" + TxtFromDate.Text + "'  And PRM.ReceiveDate<='" + TxtToDate.Text + "'";
        }
        if (DDEmployerName.SelectedIndex > 0)
        {
            sql = sql + " and PRM.EmpId=" + DDEmployerName.SelectedValue;
        }
        sql = sql + " Order by PRM.Process_Rec_Id";
        UtilityModule.ConditionalComboFill(ref DDSlipNo, sql, true, "--SELECT--");
    }
    private void ShowButton()
    {
        BtnShowData.Visible = true;
    }
    protected void BtnShowData_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        ShowDataInGrid();
    }
    //private void ShowDataInGrid()
    private DataSet ShowDataInGrid()
    {
        DataSet Ds = new DataSet();
        string Str = "";
        try
        {
            if (DDProcessName.SelectedIndex > 0)
            {
                string where = "";

                if (DDEmployerName.SelectedIndex > 0)
                {
                    where = where + " and PRM.EmpId=" + DDEmployerName.SelectedValue;
                }
                if (DDSlipNo.SelectedIndex > 0)
                {
                    where = where + " and PRM.Process_Rec_Id=" + DDSlipNo.SelectedValue;
                }
                if (TDQuality.Visible == true)
                {
                    if (DDQuality.SelectedIndex > 0)
                    {
                        where = where + " and VF.QualityId=" + DDQuality.SelectedValue;
                    }
                }
                if (ChkForVoucher.Checked == false)
                {
                    if (DDReportType.SelectedValue == "1")
                    {
                        where = where + " And  (PRD.Rate>0 Or PRD.FRRateId>0)";
                    }
                    else if (DDReportType.SelectedValue == "2")
                    {
                        where = where + " And  (PRD.Rate=0 Or PRD.FRRateId=0)";
                    }
                }
                if (ChkForExist.Checked == true)
                {
                    where = where + " And  PRM.VoucherNo Is Not Null And PRM.VoucherDate Is Not Null";
                }
                else if (ChkForExist.Checked == false)
                {
                    where = where + " And  PRM.VoucherNo Is Null And PRM.VoucherDate Is Null";
                }

                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@ReportType", DDReportType.SelectedValue);
                param[1] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
                param[2] = new SqlParameter("@Empid", DDEmployerName.SelectedValue);
                param[3] = new SqlParameter("@FromDate", TxtFromDate.Text);
                param[4] = new SqlParameter("@TODate", TxtToDate.Text);
                param[5] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
                param[6] = new SqlParameter("@SlipNo", DDSlipNo.SelectedValue);
                param[7] = new SqlParameter("@Where", where);


                Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetFinisherReceiveDataForUpdateRate", param);

                DGDetail.DataSource = "";
                DGDetail.DataBind();

                if (Ds != null && Ds.Tables.Count == 2 && Ds.Tables[1].Rows.Count > 0)
                {
                    if (Ds.Tables[1].Rows.Count > 0)
                    {
                        DGDetail.DataSource = Ds.Tables[1];
                        DGDetail.DataBind();

                        GridVoucherColumnShowStatus();

                        txttotalpcs.Text = Ds.Tables[1].Compute("Sum(Qty)", "").ToString();
                        txttotalarea.Text = Math.Round(Convert.ToDouble(Ds.Tables[1].Compute("Sum(TotalArea)", "")), 4).ToString();
                        txtamount.Text = Math.Round(Convert.ToDouble(Ds.Tables[1].Compute("Sum(Amount)", "")), 2).ToString();
                        txttotalpenality.Text = Math.Round(Convert.ToDouble(Ds.Tables[1].Compute("Sum(Penality)", "")), 2).ToString();
                        txtTotalPaidAmt.Text = Math.Round(Convert.ToDouble(txtamount.Text) - Convert.ToDouble(txttotalpenality.Text), 2).ToString();
                    }
                    else
                    {
                        txttotalpcs.Text = "";
                        txttotalarea.Text = "";
                        txtamount.Text = "";
                        txttotalpenality.Text = "";
                        txtTotalPaidAmt.Text = "";
                        ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No records found for this combination.');", true);
                    }
                }
                else
                {
                    txttotalpcs.Text = "";
                    txttotalarea.Text = "";
                    txtamount.Text = "";
                    txttotalpenality.Text = "";
                    txtTotalPaidAmt.Text = "";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No records found for this combination.');", true);
                }

            }

        }
        catch (Exception ex)
        {
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
        }
        return Ds;
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        int VarEditVarible = 0;
        string Str = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        //**********
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("Item_Finished_Id", typeof(int));
        dtrecords.Columns.Add("Process_Rec_Id", typeof(int));
        dtrecords.Columns.Add("Qty", typeof(int));
        dtrecords.Columns.Add("RPcs", typeof(int));
        dtrecords.Columns.Add("Area", typeof(float));
        dtrecords.Columns.Add("Rate", typeof(float));
        dtrecords.Columns.Add("Rate2", typeof(float));
        dtrecords.Columns.Add("RateId", typeof(int));
        dtrecords.Columns.Add("Amount", typeof(float));
        dtrecords.Columns.Add("Penality", typeof(float));
        dtrecords.Columns.Add("TDSPercentage", typeof(float));
        dtrecords.Columns.Add("TDSAmt", typeof(float));
        dtrecords.Columns.Add("Process_Rec_Detail_Id", typeof(string));
        dtrecords.Columns.Add("CustomerId", typeof(int));
        dtrecords.Columns.Add("hnRate", typeof(float));
        dtrecords.Columns.Add("hnRateId", typeof(int));
        dtrecords.Columns.Add("hnRate2", typeof(float));


        //****

        for (int i = 0; i < DGDetail.Rows.Count; i++)
        {
            Label lblItemFinishedId = (Label)DGDetail.Rows[i].FindControl("lblItemFinishedId");
            Label lblProcessRecId = (Label)DGDetail.Rows[i].FindControl("lblRecpNo");
            Label lblQty = (Label)DGDetail.Rows[i].FindControl("lblQty");
            Label lblRPcs = (Label)DGDetail.Rows[i].FindControl("lblRPcs");
            Label lblArea = (Label)DGDetail.Rows[i].FindControl("lblArea");
            Label lblRate = (Label)DGDetail.Rows[i].FindControl("lblRate");
            Label lblRate2 = (Label)DGDetail.Rows[i].FindControl("lblRate2");
            Label lblFRRateId = (Label)DGDetail.Rows[i].FindControl("lblFRRateId");
            Label lblAmount = (Label)DGDetail.Rows[i].FindControl("lblAmount");
            Label lblPenality = (Label)DGDetail.Rows[i].FindControl("lblPenality");
            Label lblTDSPercentage = (Label)DGDetail.Rows[i].FindControl("lblTDSPercentage");
            Label lblTDSAmt = (Label)DGDetail.Rows[i].FindControl("lblTDSAmt");
            Label lblProcessRecDetailId = (Label)DGDetail.Rows[i].FindControl("lblProcessRecDetailId");
            Label lblCustomerId = (Label)DGDetail.Rows[i].FindControl("lblCustomerId");
            Label hnRate = (Label)DGDetail.Rows[i].FindControl("hnRate");
            Label hnRateId = (Label)DGDetail.Rows[i].FindControl("hnRateId");
            Label hnRate2 = (Label)DGDetail.Rows[i].FindControl("hnRate2");


            DataRow dr = dtrecords.NewRow();

            dr["Item_Finished_Id"] = lblItemFinishedId.Text;
            dr["Process_Rec_Id"] = lblProcessRecId.Text;
            dr["Qty"] = lblQty.Text;
            dr["RPcs"] = lblRPcs.Text;
            dr["Area"] = lblArea.Text;
            dr["Rate"] = lblRate.Text;
            dr["Rate2"] = lblRate2.Text == "" ? "0" : lblRate2.Text;
            dr["RateId"] = lblFRRateId.Text == "" ? "0" : lblFRRateId.Text;
            dr["Amount"] = lblAmount.Text == "" ? "0" : lblAmount.Text;
            dr["Penality"] = lblPenality.Text == "" ? "0" : lblPenality.Text;
            dr["TDSPercentage"] = lblTDSPercentage.Text == "" ? "0" : lblTDSPercentage.Text;
            dr["TDSAmt"] = lblTDSAmt.Text == "" ? "0" : lblTDSAmt.Text;
            dr["Process_Rec_Detail_Id"] = lblProcessRecDetailId.Text;
            dr["CustomerId"] = lblCustomerId.Text;
            dr["hnRate"] = hnRate.Text == "" ? "0" : hnRate.Text;
            dr["hnRateId"] = hnRateId.Text == "" ? "0" : hnRateId.Text;
            dr["hnRate2"] = hnRate2.Text == "" ? "0" : hnRate2.Text;

            dtrecords.Rows.Add(dr);

        }
        if (dtrecords.Rows.Count > 0)
        {

            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                int chkboxVoucher = 0;
                if (ChkForVoucher.Checked == true)
                {
                    chkboxVoucher = 1;
                }
                else
                {
                    chkboxVoucher = 0;
                }

                int chkboxExists = 0;
                if (ChkForExist.Checked == true)
                {
                    chkboxExists = 1;
                }
                else
                {
                    chkboxExists = 0;
                }

                SqlParameter[] _arrpara1 = new SqlParameter[11];
                _arrpara1[0] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
                _arrpara1[1] = new SqlParameter("@EmpId", DDEmployerName.SelectedValue);
                _arrpara1[2] = new SqlParameter("@Date", TxtDate.Text);
                _arrpara1[3] = new SqlParameter("@SlipNo", DDSlipNo.SelectedValue);
                _arrpara1[4] = new SqlParameter("@FromDate", TxtFromDate.Text);
                _arrpara1[5] = new SqlParameter("@ToDate", TxtToDate.Text);
                _arrpara1[6] = new SqlParameter("@MastercompanyId", Session["varMasterCompanyIDForERP"]);
                _arrpara1[7] = new SqlParameter("@dtrecords", dtrecords);
                _arrpara1[8] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 100);
                _arrpara1[8].Direction = ParameterDirection.Output;
                _arrpara1[9] = new SqlParameter("@ChkForVoucher", chkboxVoucher);
                _arrpara1[10] = new SqlParameter("@ChkForExists", chkboxExists);

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_FinisherProcessHissabUpdateRate", _arrpara1);
                Tran.Commit();
                lblMessage.Visible = true;
                lblMessage.Text = _arrpara1[8].Value.ToString();

                ClearAfterSave();

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
    private void ClearAfterSave()
    {

        DGDetail.DataSource = "";
        DGDetail.DataBind();
        ChkForVoucher.Checked = false;

        txttotalpcs.Text = "";
        txttotalarea.Text = "";
        txtamount.Text = "";
        txttotalpenality.Text = "";
        txtTotalPaidAmt.Text = "";


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
            TDDDSlipNo.Visible = true;
            TDPoOrderNo.Visible = false;
        }
        else
        {
            TDPoOrderNo.Visible = true;
            TDDDSlipNo.Visible = true;
            DDSlipNo.Items.Clear();
            if (DDEmployerName.Items.Count > 0)
            {
                DDEmployerName.SelectedIndex = 0;
            }

        }
    }
    protected void TxtSlipNo_TextChanged(object sender, EventArgs e)
    {
        lblMessage.Visible = false;
    }
    protected void DDSlipNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowButton();
    }
    private void SlipNoSelectedChanges()
    {
        ViewState["Hissab_No"] = DDSlipNo.SelectedValue;
        if (DDSlipNo.SelectedIndex > 0)
        {
            TxtDate.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select IsNull(replace(convert(varchar(11),Date,106), ' ','-'),'') as Date From PROCESS_HISSAB Where HissabNo=" + DDSlipNo.SelectedValue + "").ToString();
        }
        ShowDataInGrid();
    }
    protected void DDReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FillReceiptNo();
    }
    protected void test(object sender)
    {
        lblMessage.Text = "";
        TextBox txt = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
        Label lblSrNo = (Label)gvRow.FindControl("lblSrNo");
        Label lblRecpNo = (Label)gvRow.FindControl("lblRecpNo");
        Label lblShapeName = (Label)gvRow.FindControl("lblShapeName");
        TextBox txtSize = (TextBox)gvRow.FindControl("txtSize");
        Label lblQty = (Label)gvRow.FindControl("lblQty");
        Label lblRPcs = (Label)gvRow.FindControl("lblRPcs");

        Label lblArea = (Label)gvRow.FindControl("lblArea");
        Label lblRate = (Label)gvRow.FindControl("lblRate");

        Label lblRate2 = (Label)gvRow.FindControl("lblRate2");
        Label lblAmount = (Label)gvRow.FindControl("lblAmount");
        Label lblPenality = (Label)gvRow.FindControl("lblPenality");

        Label lblPaidAmt = (Label)gvRow.FindControl("lblPaidAmt");
        Label lblReceiveDate = (Label)gvRow.FindControl("lblReceiveDate");
        Label lblTDSAmt = (Label)gvRow.FindControl("lblTDSAmt");

        Label lblQualityId = (Label)gvRow.FindControl("lblQualityId");
        Label lblDesignId = (Label)gvRow.FindControl("lblDesignId");
        Label lblColorId = (Label)gvRow.FindControl("lblColorId");
        Label lblSizeId = (Label)gvRow.FindControl("lblSizeId");
        Label lblShapeId = (Label)gvRow.FindControl("lblShapeId");
        Label lblFRRateId = (Label)gvRow.FindControl("lblFRRateId");

        Label lblCalOptionId = (Label)gvRow.FindControl("lblCalOptionId");
        Label lblCustomerId = (Label)gvRow.FindControl("lblCustomerId");
        Label lblItemFinishedId = (Label)gvRow.FindControl("lblItemFinishedId");
        Label lblTDSPercentage = (Label)gvRow.FindControl("lblTDSPercentage");
        Label lblProcessRecDetailId = (Label)gvRow.FindControl("lblProcessRecDetailId");
        Label lblEmpId = (Label)gvRow.FindControl("lblEmpId");

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _param = new SqlParameter[31];

            _param[0] = new SqlParameter("@ReportType", DDReportType.SelectedValue);
            _param[1] = new SqlParameter("@Empid", DDEmployerName.SelectedValue);
            _param[2] = new SqlParameter("@FromDate", TxtFromDate.Text);
            _param[3] = new SqlParameter("@TODate", TxtToDate.Text);
            _param[4] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
            _param[5] = new SqlParameter("@MastercompanyId", Session["varMasterCompanyIDForERP"]);
            _param[6] = new SqlParameter("@Userid", Session["varuserid"]);
            _param[7] = new SqlParameter("@Process_Rec_Id", lblRecpNo.Text);
            _param[8] = new SqlParameter("@SrNo", lblSrNo.Text);

            _param[9] = new SqlParameter("@ShapeName", lblShapeName.Text);
            _param[10] = new SqlParameter("@Size", txtSize.Text);
            _param[11] = new SqlParameter("@Qty", lblQty.Text);
            _param[12] = new SqlParameter("@RPcs", lblRPcs.Text);
            _param[13] = new SqlParameter("@Area", Convert.ToDouble(lblArea.Text));
            _param[13].Direction = ParameterDirection.InputOutput;
            _param[14] = new SqlParameter("@Rate", Convert.ToDouble(lblRate.Text));
            _param[15] = new SqlParameter("@Rate2", Convert.ToDouble(lblRate2.Text));
            _param[16] = new SqlParameter("@Amount", Convert.ToDouble(lblAmount.Text));
            _param[16].Direction = ParameterDirection.InputOutput;
            _param[17] = new SqlParameter("@PenAmt", Convert.ToDouble(lblPenality.Text));
            _param[17].Direction = ParameterDirection.InputOutput;
            _param[18] = new SqlParameter("@PaidAmt", Convert.ToDouble(lblPaidAmt.Text));
            _param[18].Direction = ParameterDirection.InputOutput;
            _param[19] = new SqlParameter("@ReceiveDate", lblReceiveDate.Text);
            _param[20] = new SqlParameter("@TDSAmt", Convert.ToDouble(lblTDSAmt.Text));
            _param[20].Direction = ParameterDirection.InputOutput;
            _param[21] = new SqlParameter("@RateId", lblFRRateId.Text);
            _param[22] = new SqlParameter("@FCalOptionId", lblCalOptionId.Text);
            _param[23] = new SqlParameter("@CustomerId", lblCustomerId.Text);
            _param[24] = new SqlParameter("@ItemFinishedId", lblItemFinishedId.Text);
            _param[25] = new SqlParameter("@TDSPercentage", Convert.ToDouble(lblTDSPercentage.Text));
            _param[26] = new SqlParameter("@ProcessRecDetailId", lblProcessRecDetailId.Text);
            _param[27] = new SqlParameter("@RTotalArea", SqlDbType.Float);
            _param[27].Direction = ParameterDirection.InputOutput;
            _param[27].Value = 0;
            _param[28] = new SqlParameter("@hnArea", SqlDbType.Float);
            _param[28].Direction = ParameterDirection.InputOutput;
            _param[28].Value = 0;
            _param[29] = new SqlParameter("@hnAmount", SqlDbType.Float);
            _param[29].Direction = ParameterDirection.InputOutput;
            _param[29].Value = 0;
            _param[30] = new SqlParameter("@hnPenality", SqlDbType.Float);
            _param[30].Direction = ParameterDirection.InputOutput;
            _param[30].Value = 0;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_CalculateFinisherAreaForHissab]", _param);


            //lblMessage.Visible = true;
            //lblMessage.Text = _param[27].Value.ToString();

            lblArea.Text = _param[13].Value.ToString();
            lblAmount.Text = _param[16].Value.ToString();
            lblPenality.Text = _param[17].Value.ToString();
            lblPaidAmt.Text = _param[18].Value.ToString();
            lblTDSAmt.Text = _param[20].Value.ToString();

            txttotalarea.Text = Math.Round((Convert.ToDouble(txttotalarea.Text) - Convert.ToDouble(_param[28].Value.ToString()) * Convert.ToUInt32(lblQty.Text)) + Convert.ToDouble(_param[27].Value.ToString()), 4).ToString();
            txtamount.Text = Math.Round((Convert.ToDouble(txtamount.Text) - Convert.ToDouble(_param[29].Value.ToString())) + Convert.ToDouble(_param[16].Value.ToString()), 2).ToString();
            txttotalpenality.Text = Math.Round((Convert.ToDouble(txttotalpenality.Text) - Convert.ToDouble(_param[30].Value.ToString())) + Convert.ToDouble(_param[17].Value.ToString()), 2).ToString();
            txtTotalPaidAmt.Text = Math.Round(Convert.ToDouble(txtamount.Text) - Convert.ToDouble(txttotalpenality.Text), 2).ToString();

            Tran.Commit();

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmUpdateFinisherJobRate.aspx");
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
    protected void txtSize_TextChanged(object sender, EventArgs e)
    {
        test(sender);
    }
    public SortDirection dir
    {
        get
        {
            if (ViewState["dirState"] == null)
            {
                ViewState["dirState"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["dirState"];
        }
        set
        {
            ViewState["dirState"] = value;
        }

    }
    protected void DGDetail_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataSet ddd = new DataSet();
        ddd = ShowDataInGrid();
        DataTable dt = ddd.Tables[1];
        string sortingDirection = string.Empty;
        if (dir == SortDirection.Ascending)
        {
            dir = SortDirection.Descending;
            sortingDirection = "Desc";
        }
        else
        {
            dir = SortDirection.Ascending;
            sortingDirection = "Asc";
        }
        DataView sortedView = new DataView(dt);
        sortedView.Sort = e.SortExpression + " " + sortingDirection;
        //Session["objects"] = sortedView;
        DGDetail.DataSource = sortedView;
        DGDetail.DataBind();
    }

    protected void BtnPriview_Click(object sender, EventArgs e)
    {

        //if (chkstocknowise.Checked == true)
        //{
        //    StockNowiseReport();
        //}
        //else
        //{
        //    Session["ReportPath"] = "Reports/RptProcessHissabSummary.rpt";
        //    Session["CommanFormula"] = "{VIEW_PROCESS_HISSAB.HissabNo}= " + ViewState["Hissab_No"].ToString() + "";
        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
        //}
    }
    protected void StockNowiseReport()
    {
        string str = "select * from VIEW_PROCESS_HISSAB_StockNo Where HissabNo=" + ViewState["Hissab_No"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "Reports/rptprocesshissabstocknowise.rpt";
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
    protected void ChkForExist_CheckedChanged(object sender, EventArgs e)
    {
        DGDetail.DataSource = "";
        DGDetail.DataBind();
    }
    protected void ChkForVoucher_CheckedChanged(object sender, EventArgs e)
    {
        //If ChkVoucher.Checked = True Then
        //    DtpVoucherDate.Enabled = True
        //Else
        //    DtpVoucherDate.Enabled = False
        //End If

        if (ChkForVoucher.Checked == true)
        {
            TxtDate.Enabled = true;
        }
        else
        {
            TxtDate.Enabled = false;
        }
    }

    protected void DDPOOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select Replace(convert(nvarchar(11),isnull(MIN(PRM.ReceiveDate),Getdate()),106),' ','-') As FromDate,Replace(convert(nvarchar(11),
                    isnull(MAX(PRM.ReceiveDate),getdate()),106),' ','-') as ToDate
                    From PROCESS_RECEIVE_MASTER_" + DDProcessName.SelectedValue + " PRM inner Join PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PRD on PRM.Process_Rec_Id=PRD.Process_Rec_Id
                    Where PRD.IssueOrderId=" + DDPOOrderNo.SelectedValue + " and PRM.EmpId=" + DDEmployerName.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        TxtFromDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["FromDate"]);
        TxtToDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["Todate"]);
        //FillSrno();
    }
    protected void btnprintvoucher_Click(object sender, EventArgs e)
    {
        string str = "select * from V_Printvoucher Where SlipNo=" + ViewState["Hissab_No"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "Reports/rptvoucher.rpt";
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
    protected void BtnSaveFinishingConsumption_Click(object sender, EventArgs e)
    {
        string Str = "";

        if (DDProcessName.SelectedIndex > 0 && (DGDetail.Rows.Count>0))
        {
            string where = "";

            if (DDEmployerName.SelectedIndex > 0)
            {
                where = where + " and PRM.EmpId=" + DDEmployerName.SelectedValue;
            }
            if (DDSlipNo.SelectedIndex > 0)
            {
                where = where + " and PRM.Process_Rec_Id=" + DDSlipNo.SelectedValue;
            }
            if (TDQuality.Visible == true)
            {
                if (DDQuality.SelectedIndex > 0)
                {
                    where = where + " and VF.QualityId=" + DDQuality.SelectedValue;
                }
            }
            if (ChkForVoucher.Checked == false)
            {
                if (DDReportType.SelectedValue == "1")
                {
                    where = where + " And  (PRD.Rate>0 Or PRD.FRRateId>0)";
                }
                else if (DDReportType.SelectedValue == "2")
                {
                    where = where + " And  (PRD.Rate=0 Or PRD.FRRateId=0)";
                }
            }
            if (ChkForExist.Checked == true)
            {
                where = where + " And  PRM.VoucherNo Is Not Null And PRM.VoucherDate Is Not Null";
            }
            else if (ChkForExist.Checked == false)
            {
                where = where + " And  PRM.VoucherNo Is Null And PRM.VoucherDate Is Null";
            }

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {

                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@ReportType", DDReportType.SelectedValue);
                param[1] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
                param[2] = new SqlParameter("@Empid", DDEmployerName.SelectedValue);
                param[3] = new SqlParameter("@FromDate", TxtFromDate.Text);
                param[4] = new SqlParameter("@TODate", TxtToDate.Text);
                param[5] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
                param[6] = new SqlParameter("@SlipNo", DDSlipNo.SelectedValue);
                param[7] = new SqlParameter("@Where", where);
                param[8] = new SqlParameter("@UserId", Session["VarUserid"]);
                param[9] = new SqlParameter("@msg", SqlDbType.NVarChar, 200);
                param[9].Direction = ParameterDirection.Output;


                //Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_SaveFinishing_Process_Consumption", param);

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveFinishing_Process_Consumption", param);

                lblMessage.Visible = true;
                lblMessage.Text = param[9].Value.ToString();
                Tran.Commit();

            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Hissab/FrmUpdateFinisherJobRate.aspx");
                Tran.Rollback();
                //ViewState["Process_Rec_Id"] = 0;
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

}