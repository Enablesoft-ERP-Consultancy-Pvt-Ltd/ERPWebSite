using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using System.Text;

public partial class Masters_Hissab_FrmProcessHissabPayment : System.Web.UI.Page
{
    static int rowindex = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            string Str = @"select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + @" order by CompanyName 
                           Select Process_Name_Id,Process_Name from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order By Process_Name ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--SELECT--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--SELECT--");

            if (Session["canedit"].ToString() == "1")
            {
                Chkedit.Visible = true;
            }

            switch (Session["varcompanyid"].ToString())
            {
                case "41":
                    TRApprovalAmt.Visible = true;
                    BindGridApprovalAmt();

                    break;
                default:
                    TRApprovalAmt.Visible = false;
                    GVApprovalAmt.DataSource = null;
                    GVApprovalAmt.DataBind();
                    break;
            }



            Date.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            lblErr.Text = "";
            Bname.Visible = false;
            chqNo.Visible = false;            
            Clear();
        }
    }
    protected void BindGridApprovalAmt()
    {
         DataSet ds = new DataSet();

         #region Where Condition
         string Where = "";
         string filterby = "";
         //string filterby = "From : " + TxtFRDate.Text + "  To : " + TxtTODate.Text;
         //Where = Where + " and Prm.Receivedate>='" + TxtFRDate.Text + "' and PRM.Receivedate<='" + TxtTODate.Text + "'";
         //if (DDProcessName.SelectedIndex > 0)
         //{
            // Where = Where + " And CII.Customerid=" + ddcustomer.SelectedValue;
             //filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
         //}        

         #endregion

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GET_HISSABAPPROVALAMT_FORGRID", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedValue);
        cmd.Parameters.AddWithValue("@Where", Where);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyNo"]);
        cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);         

        // DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        if (ds.Tables[0].Rows.Count > 0)
        {
            GVApprovalAmt.DataSource = ds.Tables[0];
            GVApprovalAmt.DataBind();
        }
    }
    protected void chkByFolio_CheckedChanged(object sender, EventArgs e)
    {
        chkcommpay.Checked = false;
        if (chkByFolio.Checked == true)
        {
            // tdApprovalNo.Visible = false;
            //tdFolioNo.Visible = true;
            Label3.Text = "Folio No";

            DDEmployerName_SelectedIndexChanged(sender, new EventArgs());
            tdCrAmt.Visible = false;
            tdDrAmt.Visible = false;
            tdBalAmt.Visible = false;
            chkcommpay.Visible = true;
            ProcessNameSelectedIndexChanged();

        }
        else
        {
            // tdApprovalNo.Visible = true;
            // tdFolioNo.Visible = false;          
            Label3.Text = "Approval No";

            DDEmployerName_SelectedIndexChanged(sender, new EventArgs());
            //DDApprvNo.Items.Clear();
            tdCrAmt.Visible = true;
            tdDrAmt.Visible = true;
            tdBalAmt.Visible = true;
            DDVoucherNo.Items.Clear();
            tdVoucherNo.Visible = false;
            dgPayment.Columns[5].Visible = false;
            BtnUpdate.Visible = false;
            BtnSave.Visible = true;
            tdFolioAmt.Visible = false;
            tdpaidfolioamt.Visible = false;
            tdbalfolioamt.Visible = false;
            txtFolioTotalAmt.Text = "";
            chkcommpay.Visible = false;
            ProcessNameSelectedIndexChanged();

        }
    }
    protected void Chkedit_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkedit.Checked == true && DDProcessName.SelectedValue == "1" && chkByFolio.Checked == true)
        {
            tdVoucherNo.Visible = true;
            BtnSave.Visible = false;
            BtnUpdate.Visible = true;
        }
        else
        {
            tdVoucherNo.Visible = false;
            BtnSave.Visible = true;
            BtnUpdate.Visible = false;
            ClearEditFalse();
        }

    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Clear();
        ProcessNameSelectedIndexChanged();
    }
    private void ProcessNameSelectedIndexChanged()
    {
        string Str;
        //if (DDProcessName.SelectedIndex > 0)
        //{
        //    Str = "Select Distinct E.EmpId,E.EmpName From EmpInfo E,ProcessHissabApproved PHA Where E.Empid=PHA.Empid And PHA.ProcessId=" + DDProcessName.SelectedValue + " And E.MasterCompanyId=" + Session["varCompanyId"] + " order by Empname";
        //    UtilityModule.ConditionalComboFill(ref DDEmployerName, Str, true, "--SELECT--");
        //}

        DDApprvNo.Items.Clear();
        if (DDProcessName.SelectedValue == "1")
        {
            chkByFolio.Visible = true;
        }
        else
        {
            chkByFolio.Visible = false;
            chkByFolio.Checked = false;
        }

        if (chkByFolio.Checked == true)
        {
            if (DDProcessName.SelectedIndex > 0)
            {
                Str = "Select Distinct PM.EmpId,E.EmpName + case when isnull(e.empcode,'')<>'' then ' ['+ E.empcode+']' else '' end empname From PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM,EMpInfo E Where CompanyId=" + DDCompanyName.SelectedValue + " And PM.EmpId=E.EmpId And E.MasterCompanyId=" + Session["varCompanyId"] + " Order By EmpName";
                UtilityModule.ConditionalComboFill(ref DDEmployerName, Str, true, "--SELECT--");
            }
        }
        else
        {
            if (DDProcessName.SelectedIndex > 0)
            {
                Str = "Select Distinct E.EmpId,E.EmpName + case when isnull(e.empcode,'')<>'' then ' ['+ E.empcode+']' else '' end EMpname From EmpInfo E,ProcessHissabApproved PHA Where E.Empid=PHA.Empid And PHA.ProcessId=" + DDProcessName.SelectedValue + " And E.MasterCompanyId=" + Session["varCompanyId"] + " order by Empname";
                UtilityModule.ConditionalComboFill(ref DDEmployerName, Str, true, "--SELECT--");
            }
        }
    }
    protected void DDEmployerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Clear();
        //        string Str = "Select Id,cast(AppvNo As Nvarchar)+' / '+replace(convert(varchar(11),AppDate,106), ' ','-') As AppvNo  from ProcessHissabApproved Where CompanyId=" + DDCompanyName.SelectedValue + @" And 
        //        Processid=" + DDProcessName.SelectedValue + " And EmpId=" + DDEmployerName.SelectedValue + "  And MasterCompanyId=" + Session["varCompanyId"] + " Order by AppDate";
        string Str;
        if (Chkedit.Checked == true)
        {
            if (chkByFolio.Checked == true)
            {
                Str = @"SELECT distinct PIM.IssueOrderId as orderNo,PIM.Issueorderid as orderid FROM PROCESS_ISSUE_MASTER_1  PIM 
                        Inner join ProcessHissabPayment PHP ON PIM.IssueOrderId=PHP.FolioNo                       
                        Where PHP.CompanyId=" + DDCompanyName.SelectedValue + @" And PHP.Processid=1 And PHP.PartyId=" + DDEmployerName.SelectedValue + "  And PHP.MasterCompanyId=" + Session["varCompanyId"] + " and PHP.ByFolioStatus=1";
            }
            else
            {
                Str = @"select Distinct Id,cast(AppvNo As Nvarchar)+' / '+replace(convert(varchar(11),AppDate,106), ' ','-')
                    As AppvNo from ProcessHissabApproved PH Inner join ProcessHissabPayment PHP   on  PH.id=PHP.ApprovalNo
                    Where PH.CompanyId=" + DDCompanyName.SelectedValue + @" And PH.Processid=" + DDProcessName.SelectedValue + " And PH.EmpId=" + DDEmployerName.SelectedValue + "  And PH.MasterCompanyId=" + Session["varCompanyId"] + " and PHP.ByFolioStatus=0";
            }
        }
        else
        {
            if (chkByFolio.Checked == true)
            {
                Str = @"SELECT PIM.IssueOrderId as orderNo,PIM.Issueorderid as orderid FROM PROCESS_ISSUE_MASTER_1  PIM INNER JOIN EMPINFO EI ON PIM.EMPID=EI.EMPID
                        Where PIM.FOLIOSTATUS=0 and Pim.empid=" + DDEmployerName.SelectedValue + " and PIM.Companyid=" + DDCompanyName.SelectedValue + "";
            }
            else
            {
                Str = @"select Id,cast(AppvNo As Nvarchar)+' / '+replace(convert(varchar(11),AppDate,106), ' ','-')
                     As AppvNo from ProcessHissabApproved PH   Where PH.CompanyId=" + DDCompanyName.SelectedValue + @" And 
                     PH.Processid=" + DDProcessName.SelectedValue + " And PH.EmpId=" + DDEmployerName.SelectedValue + "  And PH.MasterCompanyId=" + Session["varCompanyId"] + " group by Id,AppvNo,AppDate having IsNull(Round(Sum(NetAmt),0),0)>dbo.F_GetHissabpayment(id)";
            }
        }

        UtilityModule.ConditionalComboFill(ref DDApprvNo, Str, true, "--SELECT--");

        if (DDApprvNo.SelectedIndex <= 0)
        {
            DDVoucherNo.Items.Clear();
            dgPayment.DataSource = null;
            dgPayment.DataBind();
        }

    }
    protected void DDApprvNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str2;
        if (chkByFolio.Checked == true && DDApprvNo.SelectedIndex > 0)
        {
            tdFolioAmt.Visible = true;
            tdpaidfolioamt.Visible = true;
            tdbalfolioamt.Visible = true;

            Str2 = @"select ISNULL(round(sum(AMOUNT-PENALITYAMT),2),0) as FolioNetAmount,ISNULL(round(sum(commamt),2),0) as Commamt from V_WeavercarpetreceiveDetail  Where CompanyId=" + DDCompanyName.SelectedValue + @"
                    And EmpId=" + DDEmployerName.SelectedValue + "  And ISSUEORDERID=" + DDApprvNo.SelectedValue + "";
            Str2 = Str2 + @" select isnull(sum(case when COMMPAYMENTFLAG=0 then CrAmt+DrAmt else 0 end),0) as Weavingamt,isnull(Sum( case when COMMPAYMENTFLAG=1 then  CrAmt+DrAmt else 0 end),0) as Commamt
                            From Processhissabpayment where Partyid=" + DDEmployerName.SelectedValue + " and  FolioNo=" + DDApprvNo.SelectedValue + "";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str2);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (chkcommpay.Checked == true)
                {
                    txtFolioTotalAmt.Text = ds.Tables[0].Rows[0]["Commamt"].ToString();
                    txtpaidfolioamt.Text = ds.Tables[1].Rows[0]["Commamt"].ToString();
                }
                else
                {
                    txtFolioTotalAmt.Text = ds.Tables[0].Rows[0]["FolioNetAmount"].ToString();
                    txtpaidfolioamt.Text = ds.Tables[1].Rows[0]["Weavingamt"].ToString();
                }
                txtbalfolioamt.Text = Convert.ToString(Convert.ToDecimal(txtFolioTotalAmt.Text == "" ? "0" : txtFolioTotalAmt.Text) - Convert.ToDecimal(txtpaidfolioamt.Text == "" ? "0" : txtpaidfolioamt.Text));

            }
        }
        else
        {
            txtFolioTotalAmt.Text = "";
            tdFolioAmt.Visible = false;
            tdpaidfolioamt.Visible = false;
            tdbalfolioamt.Visible = false;
        }

        string Str;
        if (Chkedit.Checked == true && DDProcessName.SelectedValue == "1" && chkByFolio.Checked == true)
        {
            tdVoucherNo.Visible = true;
            BtnSave.Visible = false;
            BtnUpdate.Visible = true;
            Str = @"SELECT SrNo,VoucherNo from  processhissabpayment PHP  Where PHP.CompanyId=" + DDCompanyName.SelectedValue + @" And PHP.Processid=1 
                    And PHP.PartyId=" + DDEmployerName.SelectedValue + "  And PHP.MasterCompanyId=" + Session["varCompanyId"] + " and PHP.ByFolioStatus=1 And PHP.FolioNo=" + DDApprvNo.SelectedValue + "";

            UtilityModule.ConditionalComboFill(ref DDVoucherNo, Str, true, "--SELECT--");
        }
        Clear();
        Fill_Grid();
    }
    protected void DDVoucherNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strsql = @"Select SrNo,VoucherNo,CrDr,ChqCash,isnull(Amount,0) as Amount, BankId,isnull(ChqNo,0) as ChqNo,isnull(Narration,'') as Narration,
                            isnull(PaymentTransferMode,0) as PaymentTransferMode from processhissabpayment where SrNo=" + DDVoucherNo.SelectedValue + "";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);

        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtVocNo.Text = ds.Tables[0].Rows[0]["VoucherNo"].ToString();
            DDTrType.SelectedItem.Text = ds.Tables[0].Rows[0]["CrDr"].ToString();
            TxtAmt.Text = ds.Tables[0].Rows[0]["Amount"].ToString();
            PayThrough.SelectedValue = ds.Tables[0].Rows[0]["ChqCash"].ToString();
            PayThrough_SelectedIndexChanged(sender, new EventArgs());
            DDBankName.SelectedValue = ds.Tables[0].Rows[0]["BankId"].ToString();
            TxtChkNo.Text = ds.Tables[0].Rows[0]["ChqNo"].ToString();
            TxtNarration.Text = ds.Tables[0].Rows[0]["Narration"].ToString();
            if (TDPaymentTransferMode.Visible == true)
            {
                DDPaymentTransferMode.SelectedValue = ds.Tables[0].Rows[0]["PaymentTransferMode"].ToString();
            }
        }
        else
        {
            TxtVocNo.Text = "";
            DDTrType.SelectedIndex = 0;
            TxtAmt.Text = "";
            PayThrough.SelectedIndex = 0;
            PayThrough_SelectedIndexChanged(sender, new EventArgs());
            // DDBankName.SelectedIndex = 0;
            TxtChkNo.Text = "";
            TxtNarration.Text = "";
            if (TDPaymentTransferMode.Visible == true)
            {
                DDPaymentTransferMode.SelectedIndex = 0;
            }
        }
    }
    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        if (Chkedit.Checked == true && DDProcessName.SelectedValue == "1" && chkByFolio.Checked == true && DDVoucherNo.SelectedIndex > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {

                SqlParameter[] _arrpara1 = new SqlParameter[16];
                _arrpara1[0] = new SqlParameter("@SrNo", SqlDbType.Int);
                _arrpara1[1] = new SqlParameter("@CrDr", SqlDbType.NVarChar);
                _arrpara1[2] = new SqlParameter("@Amount", SqlDbType.Float);
                _arrpara1[3] = new SqlParameter("@ChkCash", SqlDbType.Int);
                _arrpara1[4] = new SqlParameter("@BankId", SqlDbType.Int);
                _arrpara1[5] = new SqlParameter("@ChqNo", SqlDbType.VarChar,50);
                _arrpara1[6] = new SqlParameter("@CrAmt", SqlDbType.Float);
                _arrpara1[7] = new SqlParameter("@DrAmt", SqlDbType.Float);
                _arrpara1[8] = new SqlParameter("@UId", SqlDbType.Int);
                _arrpara1[9] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                _arrpara1[10] = new SqlParameter("@Narration", SqlDbType.NVarChar);
                _arrpara1[11] = new SqlParameter("@PayMonth", SqlDbType.SmallDateTime);
                _arrpara1[12] = new SqlParameter("@AdvanceStatus", SqlDbType.Int);
                _arrpara1[13] = new SqlParameter("@FolioNo", SqlDbType.Int);
                _arrpara1[14] = new SqlParameter("@ByFolioStatus", SqlDbType.Int);
                _arrpara1[15] = new SqlParameter("@PaymentTransferMode", SqlDbType.Int);

                _arrpara1[0].Value = DDVoucherNo.SelectedValue;

                _arrpara1[1].Value = DDTrType.SelectedItem.Text;
                _arrpara1[2].Value = TxtAmt.Text;
                _arrpara1[3].Value = PayThrough.SelectedValue;
                _arrpara1[4].Value = PayThrough.SelectedValue == "1" ? DDBankName.SelectedValue : "0";
                _arrpara1[5].Value = PayThrough.SelectedValue == "1" ? TxtChkNo.Text : "0";
                _arrpara1[6].Value = DDTrType.SelectedValue == "1" ? TxtAmt.Text : "0";
                _arrpara1[7].Value = DDTrType.SelectedValue == "0" ? TxtAmt.Text : "0";
                _arrpara1[8].Value = Session["varuserid"];
                _arrpara1[9].Value = Session["varCompanyId"];
                _arrpara1[10].Value = TxtNarration.Text.Trim().ToString();
                _arrpara1[11].Value = System.DateTime.Now.ToString("dd-MMM-yyyy");
                _arrpara1[12].Value = ChkAdvanceAmt.Checked == true ? 1 : 0;
                if (chkByFolio.Checked == true)
                {
                    _arrpara1[13].Value = DDApprvNo.SelectedValue;
                }
                else
                {
                    _arrpara1[13].Value = 0;
                }
                _arrpara1[14].Value = chkByFolio.Checked == true ? 1 : 0;
                _arrpara1[15].Value = TDPaymentTransferMode.Visible == false ? "0" : (DDPaymentTransferMode.SelectedIndex > 0 ? DDPaymentTransferMode.SelectedValue : "0");

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateHissabPayment", _arrpara1);
                Tran.Commit();
                ChkAdvanceAmt_CheckedChanged(sender, e);
                TxtVocNo.Text = "";
                TxtAmt.Text = "";
                DDVoucherNo.SelectedIndex = 0;
                PayThrough.SelectedValue = "0";
                // DDBankName.SelectedIndex = 0;
                TxtChkNo.Text = "";
                TxtNarration.Text = "";

                if (TDPaymentTransferMode.Visible == true)
                {
                    DDPaymentTransferMode.SelectedIndex = 0;
                }

                Fill_Grid();
                lblErr.Text = "Data successfully updated !";
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblErr.Visible = true;
                lblErr.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        string str;
        DataSet ds;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {

            if (chkByFolio.Checked == false)
            {
                //if (ChkAdvanceAmt.Checked == true)
                //{
                //    str = "select Round(Advance,2) As Advance From V_AvailableAdvance Where EmpId=" + DDEmployerName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " And CompanyId=" + DDCompanyName.SelectedValue + "";
                //    ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        lblAdvance.Text = ds.Tables[0].Rows[0]["Advance"].ToString();
                //    }
                //    else
                //    {
                //        lblAdvance.Text = "0";
                //    }
                //    if (Convert.ToDouble(TxtAmt.Text) > Convert.ToDouble(lblAdvance.Text))
                //    {
                //        MessageSave("Amount Can not be greater than Advance Amount..");
                //        Tran.Commit();
                //        return;
                //    }
                //}
                str = "Select Round(IsNull(Sum(CrAmount-DrAmt),0),0) BalAmount From View_ProcessHissabApprovedPaymentAmt Where CompanyId=" + DDCompanyName.SelectedValue + " And Processid=" + DDProcessName.SelectedValue + " And EmpId=" + DDEmployerName.SelectedValue + " And Id=" + DDApprvNo.SelectedValue + " and Id!=0";
                ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToDouble(TxtAmt.Text) > Convert.ToDouble(ds.Tables[0].Rows[0]["BalAmount"]))
                    {
                        MessageSave("Amount Can not be greater than Balance Amount....");
                        TxtBalAmt.Text = ds.Tables[0].Rows[0]["BalAmount"].ToString();
                        Tran.Commit();
                        return;
                    }
                }
            }

            SqlParameter[] _arrpara1 = new SqlParameter[25];
            _arrpara1[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _arrpara1[1] = new SqlParameter("@ProcessID", SqlDbType.Int);
            _arrpara1[2] = new SqlParameter("@PartyId", SqlDbType.Int);
            _arrpara1[3] = new SqlParameter("@ApprovalNo", SqlDbType.Int);
            _arrpara1[4] = new SqlParameter("@Date", SqlDbType.SmallDateTime);
            _arrpara1[5] = new SqlParameter("@VoucherNo", SqlDbType.NVarChar);
            _arrpara1[6] = new SqlParameter("@CrDr", SqlDbType.NVarChar);
            _arrpara1[7] = new SqlParameter("@Amount", SqlDbType.Float);
            _arrpara1[8] = new SqlParameter("@ChkCash", SqlDbType.Int);
            _arrpara1[9] = new SqlParameter("@BankId", SqlDbType.Int);
            _arrpara1[10] = new SqlParameter("@ChqNo", SqlDbType.VarChar,50);
            _arrpara1[11] = new SqlParameter("@CrAmt", SqlDbType.Float);
            _arrpara1[12] = new SqlParameter("@DrAmt", SqlDbType.Float);
            _arrpara1[13] = new SqlParameter("@UId", SqlDbType.Int);
            _arrpara1[14] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _arrpara1[15] = new SqlParameter("@Types", SqlDbType.Int);
            _arrpara1[16] = new SqlParameter("@Narration", SqlDbType.NVarChar);
            _arrpara1[17] = new SqlParameter("@PayMonth", SqlDbType.SmallDateTime);
            _arrpara1[18] = new SqlParameter("@AdvanceStatus", SqlDbType.Int);
            _arrpara1[19] = new SqlParameter("@FolioNo", SqlDbType.Int);
            _arrpara1[20] = new SqlParameter("@ByFolioStatus", SqlDbType.Int);
            _arrpara1[21] = new SqlParameter("@COMMPAYMENTFLAG", SqlDbType.Int);
            _arrpara1[22] = new SqlParameter("@USEADVAMT", TDuseadvanceamt.Visible == false ? "0" : (txtuseadvamt.Text == "" ? "0" : txtuseadvamt.Text));
            _arrpara1[23] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            _arrpara1[23].Direction = ParameterDirection.Output;
            _arrpara1[24] = new SqlParameter("@PaymentTransferMode", SqlDbType.Int);

            _arrpara1[0].Value = DDCompanyName.SelectedValue;
            _arrpara1[1].Value = DDProcessName.SelectedValue;
            _arrpara1[2].Value = DDEmployerName.SelectedValue;
            if (chkByFolio.Checked == false)
            {
                _arrpara1[3].Value = DDApprvNo.SelectedValue;
            }
            else
            {
                _arrpara1[3].Value = 0;
            }
            _arrpara1[4].Value = Date.Text;
            _arrpara1[5].Value = "";
            _arrpara1[5].Direction = ParameterDirection.InputOutput;
            _arrpara1[6].Value = DDTrType.SelectedItem.Text;
            _arrpara1[7].Value = TxtAmt.Text;
            _arrpara1[8].Value = PayThrough.SelectedValue;
            _arrpara1[9].Value = PayThrough.SelectedValue == "1" ? DDBankName.SelectedValue : "0";
            _arrpara1[10].Value = PayThrough.SelectedValue == "1" ? TxtChkNo.Text : "0";
            _arrpara1[11].Value = DDTrType.SelectedValue == "1" ? TxtAmt.Text : "0";
            _arrpara1[12].Value = DDTrType.SelectedValue == "0" ? TxtAmt.Text : "0";
            _arrpara1[13].Value = Session["varuserid"];
            _arrpara1[14].Value = Session["varCompanyId"];
            _arrpara1[15].Value = "1";
            _arrpara1[16].Value = TxtNarration.Text.Trim().ToString();
            _arrpara1[17].Value = System.DateTime.Now.ToString("dd-MMM-yyyy");
            _arrpara1[18].Value = ChkAdvanceAmt.Checked == true ? 1 : 0;
            if (chkByFolio.Checked == true)
            {
                _arrpara1[19].Value = DDApprvNo.SelectedValue;
            }
            else
            {
                _arrpara1[19].Value = 0;
            }
            _arrpara1[20].Value = chkByFolio.Checked == true ? 1 : 0;
            _arrpara1[21].Value = chkcommpay.Checked == true ? 1 : 0;
            _arrpara1[24].Value = TDPaymentTransferMode.Visible == false ? "0" : (DDPaymentTransferMode.SelectedIndex > 0 ? DDPaymentTransferMode.SelectedValue : "0"); 

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_HissabPayment", _arrpara1);
            Tran.Commit();
            if (_arrpara1[23].Value.ToString() != "")
            {
                lblErr.Text = _arrpara1[23].Value.ToString();
            }
            else
            {
                TxtVocNo.Text = "";
                TxtAmt.Text = "";
                lblErr.Text = "Data successfully saved !";

            }
            ChkAdvanceAmt_CheckedChanged(sender, e);
            Fill_Grid();

            if (Session["varcompanyid"].ToString() == "41")
            {
                BindGridApprovalAmt();
            }

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblErr.Visible = true;
            lblErr.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void PayThrough_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PayThrough.SelectedValue == "1")
        {
            Bname.Visible = true;
            chqNo.Visible = true;
            UtilityModule.ConditionalComboFill(ref DDBankName, "Select BankId,BankName from Bank Where MasterCompanyId=" + Session["varCompanyId"] + " order by BankName", true, "--SELECT--");

            if (Session["varCompanyNo"].ToString() == "41")
            {
                TDPaymentTransferMode.Visible = true;
                UtilityModule.ConditionalComboFill(ref DDPaymentTransferMode, "Select PaymentTransferModeId,PaymentTransferMode from PaymentTransferMode Where MasterCompanyId=" + Session["varCompanyId"] + " order by PaymentTransferMode", true, "--SELECT--");
            }
        }
        else
        {
            Bname.Visible = false;
            chqNo.Visible = false;
            TDPaymentTransferMode.Visible = false;
        }
    }
    private void Fill_Grid()
    {
        string ColumnName = "";
        string Where = "";

        if (chkByFolio.Checked == true)
        {
            ColumnName = "FolioNo";
            Where = "ByFolioStatus=1 ";
            if (chkcommpay.Checked == true)
            {
                Where = Where + " and COMMPAYMENTFLAG=1";
            }
            else
            {
                Where = Where + " and COMMPAYMENTFLAG=0";
            }
        }
        else
        {
            ColumnName = "ApprovalNo";
            Where = "ByFolioStatus=0 ";
        }

        string view = "";
        if (Session["VarCompanyNo"].ToString() == "41")
        {
            view = "View_ProcessHissabApprovedPaymentAmtNew";
        }
        else
        {
            view = "View_ProcessHissabApprovedPaymentAmt";
        }

        string strsql = "";

        if (Session["varCompanyNo"].ToString() == "41")
        {
            strsql = @"Select SrNo,case when PHP.ByFolioStatus=1 then PHP.FolioNo else PHP.ApprovalNo end as ApprovalNo,CONVERT(CHAR(12),Date, 106)as Date,PHP.VoucherNo,Round(PHP.Amount,0) Amount,
                            Case When PHP.chqCash=0 Then 'Cash' Else 'Bank' End chqCash,Crdr,isnull(PHA.Tds,0) as TdsPercentage,isnull(round((PHP.Amount*PHA.Tds/100),2),0) as TdsAmt,
                            isnull(round((PHP.Amount-(PHP.Amount*PHA.Tds/100)),2),0) as AmountAfterTds 
                        from processhissabpayment PHP(NoLock) JOIN ProcessHissabApproved PHA(NoLock) ON PHP.ApprovalNo=PHA.AppvNo
                        where PHP.CompanyID=" + DDCompanyName.SelectedValue + " and PHP.ProcessId=" + DDProcessName.SelectedValue + " and PHP.PartyId=" + DDEmployerName.SelectedValue + " And " + ColumnName + "=" + DDApprvNo.SelectedValue + "  And " + Where + " and PHP.MasterCompanyId=" + Session["varCompanyId"] + @"
                Select Round(IsNull(Sum(DrAmt),0),0) DrAmt,Round(IsNull(Sum(CrAmount),0),0) CrAmount,Round(IsNull(Sum(CrAmount-DrAmt),0),0) BalAmount From " + view + @" Where CompanyId=" + DDCompanyName.SelectedValue + " And Processid=" + DDProcessName.SelectedValue + " And EmpId=" + DDEmployerName.SelectedValue + " And Id=" + DDApprvNo.SelectedValue + " and Id!=0";
        }
        else
        {
            strsql = @"Select SrNo,case when ByFolioStatus=1 then FolioNo else ApprovalNo end as ApprovalNo,CONVERT(CHAR(12),Date, 106)as Date,VoucherNo,Round(Amount,0) Amount,
                        Case When chqCash=0 Then 'Cash' Else 'Bank' End chqCash,Crdr,0 as TdsAmt,0 as AmountAfterTds
                        from processhissabpayment where CompanyID=" + DDCompanyName.SelectedValue + " and ProcessId=" + DDProcessName.SelectedValue + " and PartyId=" + DDEmployerName.SelectedValue + " And " + ColumnName + "=" + DDApprvNo.SelectedValue + "  And " + Where + " and MasterCompanyId=" + Session["varCompanyId"] + @"
                Select Round(IsNull(Sum(DrAmt),0),0) DrAmt,Round(IsNull(Sum(CrAmount),0),0) CrAmount,Round(IsNull(Sum(CrAmount-DrAmt),0),0) BalAmount From " + view + @" Where CompanyId=" + DDCompanyName.SelectedValue + " And Processid=" + DDProcessName.SelectedValue + " And EmpId=" + DDEmployerName.SelectedValue + " And Id=" + DDApprvNo.SelectedValue + " and Id!=0";
        }
       
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);

        dgPayment.DataSource = ds.Tables[0];
        dgPayment.DataBind();
        if (ds.Tables[1].Rows.Count > 0)
        {
            TxtCrAmt.Text = ds.Tables[1].Rows[0]["CrAmount"].ToString();
            TxtDrAmt.Text = ds.Tables[1].Rows[0]["DrAmt"].ToString();
            TxtBalAmt.Text = ds.Tables[1].Rows[0]["BalAmount"].ToString();
        }
    }
    protected void Preview_Click(object sender, EventArgs e)
    {
        DataSet DS = null;
        DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from V_HissabPayment where ApprovalNo=" + DDApprvNo.SelectedItem.Text + " And ProcessId=" + DDProcessName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "");

        if (DS.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptProcessHissabPayment.rpt";
            Session["GetDataset"] = DS;
            Session["dsFileName"] = "~\\ReportSchema\\RptHissabPayment.xsd";
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
    protected void dgPayment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgPayment.PageIndex = e.NewPageIndex;
    }
    protected void Rowdelete(int rowindex, object sender = null)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int key = Convert.ToInt32(dgPayment.DataKeys[rowindex].Value);
            SqlParameter[] param = new SqlParameter[9];
            param[0] = new SqlParameter("@srno", key);
            param[1] = new SqlParameter("@companyid", DDCompanyName.SelectedValue);
            param[2] = new SqlParameter("@processid", DDProcessName.SelectedValue);
            param[3] = new SqlParameter("@partyid", DDEmployerName.SelectedValue);
            param[4] = new SqlParameter("@approvalNo", DDApprvNo.SelectedValue);
            param[5] = new SqlParameter("@mastercompanyid", Session["varcompanyId"]);
            param[6] = new SqlParameter("@userid", Session["varuserid"]);
            param[7] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[7].Direction = ParameterDirection.Output;
            param[8] = new SqlParameter("@voucherno", dgPayment.Rows[rowindex].Cells[2].Text);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEPROCESSHISSABPAYMENT", param);
            lblErr.Text = param[7].Value.ToString();
            Tran.Commit();

            //            string del = @"Delete from processhissabpayment Where SrNo=" + key + " And CompanyID=" + DDCompanyName.SelectedValue + " And ProcessId=" + DDProcessName.SelectedValue + " and PartyId=" + DDEmployerName.SelectedValue + @" 
            //                           Select * From ProcessHissabPayment Where CompanyID=" + DDCompanyName.SelectedValue + " And ProcessId=" + DDProcessName.SelectedValue + " and PartyId=" + DDEmployerName.SelectedValue + " And ApprovalNo=" + DDApprvNo.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];

            //            DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, del);

            //            if (chkByFolio.Checked == false)
            //            {
            //                if (Ds.Tables[0].Rows.Count == 0)
            //                {
            //                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Update ProcessHissabApproved Set Status=0 Where CompanyID=" + DDCompanyName.SelectedValue + " And ProcessId=" + DDProcessName.SelectedValue + " And EmpId=" + DDEmployerName.SelectedValue + " And AppvNo=" + DDApprvNo.SelectedValue);
            //                }
            //                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete From  AdvanceAmount where Hissab_VoucherNo=" + dgPayment.Rows[rowindex].Cells[2].Text + ""); // 2 fro voucherno
            //            }

            //            Tran.Commit();
            if (sender != null)
            {
                ChkAdvanceAmt_CheckedChanged(sender, new EventArgs());
            }
            Fill_Grid();
            //lblErr.Text = "Record Deleted Successfully !";
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblErr.Visible = true;
            lblErr.Text = ex.Message;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    private void Clear()
    {
        TxtBalAmt.Text = "";
        TxtCrAmt.Text = "";
        TxtDrAmt.Text = "";
        TxtAmt.Text = "";
        TxtChkNo.Text = "";
        TxtNarration.Text = "";
        TxtVocNo.Text = "";
        if (TDPaymentTransferMode.Visible == true)
        {
            DDPaymentTransferMode.SelectedIndex = 0;
        }
    }
    private void ClearEditFalse()
    {
        DDProcessName.SelectedIndex = 0;
        DDEmployerName.Items.Clear();
        chkByFolio.Checked = false;
        chkByFolio.Visible = false;
        DDApprvNo.Items.Clear();
        TxtBalAmt.Text = "";
        TxtCrAmt.Text = "";
        TxtDrAmt.Text = "";
        TxtAmt.Text = "";
        TxtChkNo.Text = "";
        TxtNarration.Text = "";
        TxtVocNo.Text = "";
        dgPayment.DataSource = null;
        dgPayment.DataBind();

        if (TDPaymentTransferMode.Visible == true)
        {
            DDPaymentTransferMode.SelectedIndex = 0;
        }
    }
    protected void dgPayment_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int key = Convert.ToInt32(dgPayment.DataKeys[e.RowIndex].Value);
        string str = "";

        if (Session["VarCompanyNo"].ToString() == "41")
        {
            str = @"Select CI.CompanyName,CI.CompAddr1,CI.CompAddr2,CI.CompAddr3,EI.EmpName,EI.EmpCode,EI.Address+' '+EI.Address2+' '+EI.Address3 as EmpAddress,EI.EmailAdd,EI.Mobile,
                        PNM.PROCESS_NAME+case when PHP.Commpaymentflag=1 Then '  COMMISSION ' else '' end as Process_name,
                        PHP.ApprovalNo,PHP.Date,PHP.VoucherNo,PHP.CrDr,PHP.Amount,ChqCash,B.BankName,ChqNo as ChqNoUPCNo,SrNo,PHP.Narration,PHP.FolioNo,
                        EI.Purchase_EmpBankDetails as EmpBankDetail,PTM.PaymentTransferMode,isnull(PHA.Tds,0) as TdsPercentage		
                        From ProcessHissabPayment PHP(NoLock) JOIN CompanyInfo CI(NoLock) ON PHP.CompanyId=CI.CompanyId
                        JOIN EmpInfo EI(NoLock) ON PHP.PartyId=EI.EmpId
                        JOIN PROCESS_NAME_MASTER PNM(NoLock) ON PHP.ProcessId=PNM.PROCESS_NAME_ID
                        Left JOIN Bank B(NoLock) ON PHP.BankId=B.BankId
                        Left JOIN PaymentTransferMode PTM(NoLock) ON PHP.PaymentTransferMode=PTM.PaymentTransferModeId
                        JOIN ProcessHissabApproved PHA ON PHP.ApprovalNo=PHA.AppvNo
                        Where PHP.SrNo=" + key;
        }
        else if (Session["VarCompanyNo"].ToString() == "42")
        {
            str = @"Select CI.CompanyName,CI.CompAddr1,CI.CompAddr2,CI.CompAddr3,EI.EmpName,EI.EmpCode,EI.Address+' '+EI.Address2+' '+EI.Address3 as EmpAddress,EI.EmailAdd,EI.Mobile,EI.PanNo,
                        PNM.PROCESS_NAME+case when PHP.Commpaymentflag=1 Then '  COMMISSION ' else '' end as Process_name,
                        PHP.ApprovalNo,PHP.Date,PHP.VoucherNo,PHP.CrDr,PHP.Amount,ChqCash,B.BankName,ChqNo as ChqNoUPCNo,SrNo,PHP.Narration,PHP.FolioNo,
                        EI.Purchase_EmpBankDetails as EmpBankDetail,PTM.PaymentTransferMode,
                        Case When PHP.ProcessId=1 Then (Select Distinct isnull(PIM.ChallanNo,PIM.ISSUEORDERID) from ProcessHissabApproved PHA JOIN ProcessHissabApprovedDetail PHAD ON PHA.ID=PHAD.ID
                        JOIN Process_hissab PH ON PHAD.HissabId=PH.HissabNo JOIN Process_Issue_Master_1 PIM ON PH.ProcessOrderNo=PIM.ISSUEORDERID Where PHA.AppvNo=PHP.ApprovalNo )
                        Else '' End as FolioChallanNo		
                        From ProcessHissabPayment PHP(NoLock) JOIN CompanyInfo CI(NoLock) ON PHP.CompanyId=CI.CompanyId
                        JOIN EmpInfo EI(NoLock) ON PHP.PartyId=EI.EmpId
                        JOIN PROCESS_NAME_MASTER PNM(NoLock) ON PHP.ProcessId=PNM.PROCESS_NAME_ID
                        Left JOIN Bank B(NoLock) ON PHP.BankId=B.BankId
                        Left JOIN PaymentTransferMode PTM(NoLock) ON PHP.PaymentTransferMode=PTM.PaymentTransferModeId
                        JOIN ProcessHissabApproved PHA ON PHP.ApprovalNo=PHA.AppvNo
                        Where PHP.SrNo=" + key;
        }
        else
        {
            str = @"Select CI.CompanyName,PNM.PROCESS_NAME+case when PHP.Commpaymentflag=1 Then '  COMMISSION ' else '' end as Process_name,EI.EmpName,
                    ApprovalNo,Date,VoucherNo,CrDr,Amount,ChqCash,B.BankName,ChqNo,SrNo,Narration,FolioNo From CompanyInfo CI,Process_Name_Master PNM,EmpInfo EI,ProcessHissabPayment PHP 
                    Left Outer Join Bank B ON PHP.BankId=B.BankId Where PHP.CompanyId=CI.CompanyId And PHP.ProcessId=PNM.PROCESS_NAME_ID And PHP.PartyId=EI.EmpId And SrNo=" + key;
        }      

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

////        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select CI.CompanyName,PNM.PROCESS_NAME+case when PHP.Commpaymentflag=1 Then '  COMMISSION ' else '' end as Process_name,EI.EmpName,
////        ApprovalNo,Date,VoucherNo,CrDr,Amount,ChqCash,B.BankName,ChqNo,SrNo,Narration,FolioNo From CompanyInfo CI,Process_Name_Master PNM,EmpInfo EI,ProcessHissabPayment PHP 
////        Left Outer Join Bank B ON PHP.BankId=B.BankId Where PHP.CompanyId=CI.CompanyId And PHP.ProcessId=PNM.PROCESS_NAME_ID And PHP.PartyId=EI.EmpId And SrNo=" + key);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Session["VarCompanyNo"].ToString() == "41")
            {
                Session["rptFileName"] = "~\\Reports\\RptPaymentVoucherMohdRazi.rpt"; 
            }
            else if (Session["VarCompanyNo"].ToString() == "42")
            {
                Session["rptFileName"] = "~\\Reports\\RptPaymentVoucherVikramMirzapur.rpt";
            }
            else
            {
                Session["rptFileName"] = "reports/RptPaymentVoucher.rpt";
            }
            
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptPaymentVoucher.xsd";
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
    protected void ChkAdvanceAmt_CheckedChanged(object sender, EventArgs e)
    {

        if (DDEmployerName.SelectedIndex > 0)
        {
            TDuseadvanceamt.Visible = false;
            txtuseadvamt.Text = "";

            if (ChkAdvanceAmt.Checked == true)
            {
                lblAdvance.Visible = true;
                TDuseadvanceamt.Visible = true;

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Round(Advance,2) As Advance From V_AvailableAdvance Where EmpId=" + DDEmployerName.SelectedValue + " And MasterCompanyId=" + Session["VarCompanyId"] + " And CompanyId=" + DDCompanyName.SelectedValue + "");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblAdvance.Text = ds.Tables[0].Rows[0]["Advance"].ToString();
                }
                else
                {
                    lblAdvance.Text = "0";
                }
            }
            else
            {
                lblAdvance.Text = "";
                lblAdvance.Visible = false; ;
            }

        }
    }
    private void MessageSave(string msg)
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('");
        stb.Append(msg);
        stb.Append("');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
    protected void DDTrType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDTrType.SelectedValue == "1")
        {
            ChkAdvanceAmt.Checked = false;
            ChkAdvanceAmt.Visible = false;
            lblAdvance.Visible = false;
        }
        else
        {
            lblAdvance.Visible = true;
            ChkAdvanceAmt.Visible = true;
        }
    }


    protected void chkcommpay_CheckedChanged(object sender, EventArgs e)
    {
        DDApprvNo_SelectedIndexChanged(sender, new EventArgs());
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
    protected void txtpwd_TextChanged(object sender, EventArgs e)
    {
        lblErr.Text = "";
        if (dgPayment.Rows.Count > 0)
        {

            if (variable.VarPAYMENTDEL_PWD == txtpwd.Text)
            {
                Rowdelete(rowindex, sender);
                Popup(false);
            }
            else
            {
                lblErr.Text = "Please Enter Correct Password..";
            }

        }
    }
    protected void lnkdel_Click(object sender, EventArgs e)
    {
        Popup(true);
        txtpwd.Focus();
        LinkButton lnkdel = sender as LinkButton;
        GridViewRow gvr = lnkdel.NamingContainer as GridViewRow;
        rowindex = gvr.RowIndex;
    }
    protected void dgPayment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgPayment, "Select$" + e.Row.RowIndex);


            for (int i = 0; i < dgPayment.Columns.Count; i++)
            {
                if (Session["VarCompanyNo"].ToString()=="41")
                {
                    if (dgPayment.Columns[i].HeaderText== "TDSAmt" || dgPayment.Columns[i].HeaderText== "Amount")
                    {
                        dgPayment.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (dgPayment.Columns[i].HeaderText== "TDSAmt" || dgPayment.Columns[i].HeaderText== "Amount")
                    {
                        dgPayment.Columns[i].Visible = false;
                    }
                }

                if (chkByFolio.Checked == true && DDApprvNo.SelectedIndex > 0)
                {
                    if (dgPayment.Columns[i].HeaderText== "Credit/Debit")
                    {
                        dgPayment.Columns[i].Visible = true;
                    }

                }
                else
                {
                    if (dgPayment.Columns[i].HeaderText.ToUpper() == "Credit/Debit")
                    {
                        dgPayment.Columns[i].Visible = false;
                    }
                }

                if (dgPayment.Columns[i].HeaderText.ToUpper() == "DEL")
                {
                    if (Session["varCompanyNo"].ToString() == "42")
                    {
                        if (Session["usertype"].ToString() == "1")
                        {
                            dgPayment.Columns[i].Visible = true;
                        }
                        else
                        {
                            dgPayment.Columns[i].Visible = false;
                        }
                    }
                }
            }

           

            //if (chkByFolio.Checked == true && DDApprvNo.SelectedIndex > 0)
            //{                
            //    dgPayment.Columns[5].Visible = true;
            //}
            //else
            //{
            //    dgPayment.Columns[5].Visible = false;
            //}
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

    protected void GVApprovalAmt_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVApprovalAmt.PageIndex = e.NewPageIndex;
        BindGridApprovalAmt();
    }
    protected void GVApprovalAmt_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVApprovalAmt, "Select$" + e.Row.RowIndex);

        }
    }
    protected void GVApprovalAmt_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        int id =Convert.ToInt32(GVApprovalAmt.SelectedDataKey.Value.ToString());
        //hnSizeId.Value = id;
        
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GET_HISSABAPPROVALAMT_FORAUTOFILL", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", "");
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyNo"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
            cmd.Parameters.AddWithValue("@Id", id);

            // DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                DDProcessName.SelectedValue = ds.Tables[0].Rows[0]["ProcessId"].ToString();
                ProcessNameSelectedIndexChanged();
                DDEmployerName.SelectedValue = ds.Tables[0].Rows[0]["EmpId"].ToString();
                DDEmployerName_SelectedIndexChanged(sender, new EventArgs());
                DDApprvNo.SelectedValue = ds.Tables[0].Rows[0]["Id"].ToString();
                DDApprvNo_SelectedIndexChanged(sender, new EventArgs());
               
            }
        
    }
}