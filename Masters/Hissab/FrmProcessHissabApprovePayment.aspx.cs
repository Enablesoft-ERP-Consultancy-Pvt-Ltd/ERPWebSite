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

public partial class Masters_Hissab_FrmProcessHissabApprovePayment : System.Web.UI.Page
{
    static int rowindex = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            string Str = @"select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by CompanyName 
                           Select Process_Name_Id,Process_Name from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By Process_Name ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--SELECT--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--SELECT--");

            //if (Session["canedit"].ToString() == "1")
            //{
            //    Chkedit.Visible = true;
            //}

            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "41":
                    TRApprovalAmt.Visible = true;
                    //BindGridApprovalAmt();

                    break;
                default:
                    TRApprovalAmt.Visible = false;
                    GVApprovalAmt.DataSource = null;
                    GVApprovalAmt.DataBind();
                    break;
            }



            TxtDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            lblErr.Text = "";                     
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
         if (DDProcessName.SelectedIndex > 0)
         {
             Where = Where + " And PH.ProcessId=" + DDProcessName.SelectedValue;
             //filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
         }
         if (DDEmployerName.SelectedIndex > 0)
         {
             Where = Where + " And PH.EmpId=" + DDEmployerName.SelectedValue;
             //filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
         }

         #endregion

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GET_HISSAB_PENDING_APPROVEAMT_FORGRID", con);
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
    
    protected void Chkedit_CheckedChanged(object sender, EventArgs e)
    {
        //if (Chkedit.Checked == true && DDProcessName.SelectedValue == "1" && chkByFolio.Checked == true)
        //{
        //    tdVoucherNo.Visible = true;
        //    BtnSave.Visible = false;
        //    BtnUpdate.Visible = true;
        //}
        //else
        //{
        //    tdVoucherNo.Visible = false;
        //    BtnSave.Visible = true;
        //    BtnUpdate.Visible = false;
        //    ClearEditFalse();
        //}

    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Clear();
        ProcessNameSelectedIndexChanged();
        //BindGridApprovalAmt();
    }
    private void ProcessNameSelectedIndexChanged()
    {
        string Str;
        //if (DDProcessName.SelectedIndex > 0)
        //{
        //    Str = "Select Distinct E.EmpId,E.EmpName From EmpInfo E,ProcessHissabApproved PHA Where E.Empid=PHA.Empid And PHA.ProcessId=" + DDProcessName.SelectedValue + " And E.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Empname";
        //    UtilityModule.ConditionalComboFill(ref DDEmployerName, Str, true, "--SELECT--");
        //}

        if (DDProcessName.SelectedIndex > 0)
        {
            Str = "Select Distinct E.EmpId,E.EmpName + case when isnull(e.empcode,'')<>'' then ' ['+ E.empcode+']' else '' end EMpname From EmpInfo E,ProcessHissabApproved PHA Where E.Empid=PHA.Empid And PHA.ProcessId=" + DDProcessName.SelectedValue + " And E.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Empname";
            UtilityModule.ConditionalComboFill(ref DDEmployerName, Str, true, "--SELECT--");
        }
       
    }
    protected void DDEmployerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Clear();      
        string Str;
        if (Chkedit.Checked == true)
        {
            Str = @"select Distinct Id,cast(AppvNo As Nvarchar)+' / '+replace(convert(varchar(11),AppDate,106), ' ','-')
                    As AppvNo from ProcessHissabApproved PH Inner join ProcessHissabPayment PHP   on  PH.id=PHP.ApprovalNo
                    Where PH.CompanyId=" + DDCompanyName.SelectedValue + @" And PH.Processid=" + DDProcessName.SelectedValue + " And PH.EmpId=" + DDEmployerName.SelectedValue + "  And PH.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and PHP.ByFolioStatus=0";

            
        }
        else
        {
            Str = @"select Id,cast(AppvNo As Nvarchar)+' / '+replace(convert(varchar(11),AppDate,106), ' ','-')
                     As AppvNo from ProcessHissabApproved PH   Where PH.CompanyId=" + DDCompanyName.SelectedValue + @" And 
                     PH.Processid=" + DDProcessName.SelectedValue + " And PH.EmpId=" + DDEmployerName.SelectedValue + "  And PH.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " group by Id,AppvNo,AppDate having IsNull(Round(Sum(NetAmt),0),0)>dbo.F_GetHissabApprovePayment(id)";

           
        }

        BindGridApprovalAmt();

       // UtilityModule.ConditionalComboFill(ref DDApprvNo, Str, true, "--SELECT--");

        //if (DDApprvNo.SelectedIndex <= 0)
        //{
        //    DDVoucherNo.Items.Clear();
        //    dgPayment.DataSource = null;
        //    dgPayment.DataBind();
        //}

    }
   
   
    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        //if (Chkedit.Checked == true && DDProcessName.SelectedValue == "1" && chkByFolio.Checked == true && DDVoucherNo.SelectedIndex > 0)
        //{
        //    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //    con.Open();
        //    SqlTransaction Tran = con.BeginTransaction();
        //    try
        //    {

        //        SqlParameter[] _arrpara1 = new SqlParameter[16];
        //        _arrpara1[0] = new SqlParameter("@SrNo", SqlDbType.Int);
        //        _arrpara1[1] = new SqlParameter("@CrDr", SqlDbType.NVarChar);
        //        _arrpara1[2] = new SqlParameter("@Amount", SqlDbType.Float);
        //        _arrpara1[3] = new SqlParameter("@ChkCash", SqlDbType.Int);
        //        _arrpara1[4] = new SqlParameter("@BankId", SqlDbType.Int);
        //        _arrpara1[5] = new SqlParameter("@ChqNo", SqlDbType.VarChar,50);
        //        _arrpara1[6] = new SqlParameter("@CrAmt", SqlDbType.Float);
        //        _arrpara1[7] = new SqlParameter("@DrAmt", SqlDbType.Float);
        //        _arrpara1[8] = new SqlParameter("@UId", SqlDbType.Int);
        //        _arrpara1[9] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        //        _arrpara1[10] = new SqlParameter("@Narration", SqlDbType.NVarChar);
        //        _arrpara1[11] = new SqlParameter("@PayMonth", SqlDbType.SmallDateTime);
        //        _arrpara1[12] = new SqlParameter("@AdvanceStatus", SqlDbType.Int);
        //        _arrpara1[13] = new SqlParameter("@FolioNo", SqlDbType.Int);
        //        _arrpara1[14] = new SqlParameter("@ByFolioStatus", SqlDbType.Int);
        //        _arrpara1[15] = new SqlParameter("@PaymentTransferMode", SqlDbType.Int);

        //        _arrpara1[0].Value = DDVoucherNo.SelectedValue;

        //        _arrpara1[1].Value = DDTrType.SelectedItem.Text;
        //        _arrpara1[2].Value = TxtAmt.Text;
        //        _arrpara1[3].Value = PayThrough.SelectedValue;
        //        _arrpara1[4].Value = PayThrough.SelectedValue == "1" ? DDBankName.SelectedValue : "0";
        //        _arrpara1[5].Value = PayThrough.SelectedValue == "1" ? TxtChkNo.Text : "0";
        //        _arrpara1[6].Value = DDTrType.SelectedValue == "1" ? TxtAmt.Text : "0";
        //        _arrpara1[7].Value = DDTrType.SelectedValue == "0" ? TxtAmt.Text : "0";
        //        _arrpara1[8].Value = Session["varuserid"];
        //        _arrpara1[9].Value = Session["varMasterCompanyIDForERP"];
        //        _arrpara1[10].Value = TxtNarration.Text.Trim().ToString();
        //        _arrpara1[11].Value = System.DateTime.Now.ToString("dd-MMM-yyyy");
        //        _arrpara1[12].Value = ChkAdvanceAmt.Checked == true ? 1 : 0;
        //        if (chkByFolio.Checked == true)
        //        {
        //            _arrpara1[13].Value = DDApprvNo.SelectedValue;
        //        }
        //        else
        //        {
        //            _arrpara1[13].Value = 0;
        //        }
        //        _arrpara1[14].Value = chkByFolio.Checked == true ? 1 : 0;
        //        _arrpara1[15].Value = TDPaymentTransferMode.Visible == false ? "0" : (DDPaymentTransferMode.SelectedIndex > 0 ? DDPaymentTransferMode.SelectedValue : "0");

        //        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateHissabPayment", _arrpara1);
        //        Tran.Commit();
        //        ChkAdvanceAmt_CheckedChanged(sender, e);
        //        TxtVocNo.Text = "";
        //        TxtAmt.Text = "";
        //        DDVoucherNo.SelectedIndex = 0;
        //        PayThrough.SelectedValue = "0";
        //        // DDBankName.SelectedIndex = 0;
        //        TxtChkNo.Text = "";
        //        TxtNarration.Text = "";

        //        if (TDPaymentTransferMode.Visible == true)
        //        {
        //            DDPaymentTransferMode.SelectedIndex = 0;
        //        }

        //        Fill_Grid();
        //        lblErr.Text = "Data successfully updated !";
        //    }
        //    catch (Exception ex)
        //    {
        //        Tran.Rollback();
        //        lblErr.Visible = true;
        //        lblErr.Text = ex.Message;
        //    }
        //    finally
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //}

    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        string status = "";
        string ApproveAmountDetailData = "";
        for (int i = 0; i < GVApprovalAmt.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)GVApprovalAmt.Rows[i].FindControl("Chkboxitem"));
            TextBox txtPendingApproveAmt = ((TextBox)GVApprovalAmt.Rows[i].FindControl("txtPendingApproveAmt"));

            if (Chkboxitem.Checked == true)   // Change when Updated Completed
            {
                status = "1";
            }
            if (txtPendingApproveAmt.Text == "" && Chkboxitem.Checked == true)   // Change when Updated Completed
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Approve Amount can not be blank');", true);
                txtPendingApproveAmt.Focus();
                return;
            }
            if (Chkboxitem.Checked == true && (Convert.ToDecimal(txtPendingApproveAmt.Text) <= 0))   // Change when Updated Completed
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Pending Approve Amount always greater then zero');", true);
                txtPendingApproveAmt.Focus();
                return;
            }

            if (Chkboxitem.Checked == true)
            {
                Label lblApproveId = ((Label)GVApprovalAmt.Rows[i].FindControl("lblApproveId"));
                Label lblProcessId = ((Label)GVApprovalAmt.Rows[i].FindControl("lblProcessId"));
                Label lblApproveBillAmt = ((Label)GVApprovalAmt.Rows[i].FindControl("lblApproveBillAmt"));
                Label lblPartyId = ((Label)GVApprovalAmt.Rows[i].FindControl("lblPartyId"));
                //TextBox txtPendingApproveAmt = ((TextBox)GVApprovalAmt.Rows[i].FindControl("txtPendingApproveAmt"));
                

                if (ApproveAmountDetailData == "")
                {
                    ApproveAmountDetailData = lblApproveId.Text + "|" + lblProcessId.Text + "|" + lblPartyId.Text + "|" + lblApproveBillAmt .Text+ "|"+ txtPendingApproveAmt.Text + "~";
                }
                else
                {
                    ApproveAmountDetailData = ApproveAmountDetailData + lblApproveId.Text + "|" + lblProcessId.Text + "|" + lblPartyId.Text +"|"+ lblApproveBillAmt .Text+ "|" + txtPendingApproveAmt.Text + "~";
                }
            }
        }
        if (ApproveAmountDetailData == "" && status=="")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box');", true);
            return;
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[10];
            arr[0] = new SqlParameter("@ProcessHissabApprovePaymentId", SqlDbType.Int);
            arr[1] = new SqlParameter("@CompanyID", SqlDbType.Int);
            arr[2] = new SqlParameter("@ProcessID", SqlDbType.Int);
            arr[3] = new SqlParameter("@PartyId", SqlDbType.Int);
            arr[4] = new SqlParameter("@Date", SqlDbType.DateTime);
            arr[5] = new SqlParameter("@ApprovePaymentNo", SqlDbType.VarChar,100);          
            arr[6] = new SqlParameter("@ApproveAmountDetailData", SqlDbType.NVarChar);
            arr[7] = new SqlParameter("@UserID", SqlDbType.Int);
            arr[8] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            arr[9] = new SqlParameter("@Msg", SqlDbType.VarChar, 200);

            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = 0;
            arr[1].Value = DDCompanyName.SelectedValue;
            arr[2].Value = DDProcessName.SelectedValue;
            arr[3].Value = DDEmployerName.SelectedValue;
            //arr[4].Value = DDIssueNo.SelectedValue;
            arr[4].Value = TxtDate.Text;
            arr[5].Value = "";
            arr[6].Value = ApproveAmountDetailData;           
            arr[7].Value = Session["varuserid"];
            arr[8].Value = Session["varCompanyNo"];
            arr[9].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[Pro_SaveProcessHissabApprovePayment]", arr);

            if (arr[9].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + arr[9].Value.ToString() + "');", true);
                tran.Rollback();
            }
            else
            {
               //HnReceiveID.Value = arr[0].Value.ToString();
                //TxtReceiveNo.Text = Convert.ToString(arr[5].Value);
                tran.Commit();
            }
            BindGridApprovalAmt();
            //FillDGGrid();
            //fill_grid();
            //btnPreview.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + ex.Message + "');", true);
            tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

        //string str;
        //DataSet ds;
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //con.Open();
        //SqlTransaction Tran = con.BeginTransaction();
        //try
        //{

        //    if (chkByFolio.Checked == false)
        //    {
        //        //if (ChkAdvanceAmt.Checked == true)
        //        //{
        //        //    str = "select Round(Advance,2) As Advance From V_AvailableAdvance Where EmpId=" + DDEmployerName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And CompanyId=" + DDCompanyName.SelectedValue + "";
        //        //    ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
        //        //    if (ds.Tables[0].Rows.Count > 0)
        //        //    {
        //        //        lblAdvance.Text = ds.Tables[0].Rows[0]["Advance"].ToString();
        //        //    }
        //        //    else
        //        //    {
        //        //        lblAdvance.Text = "0";
        //        //    }
        //        //    if (Convert.ToDouble(TxtAmt.Text) > Convert.ToDouble(lblAdvance.Text))
        //        //    {
        //        //        MessageSave("Amount Can not be greater than Advance Amount..");
        //        //        Tran.Commit();
        //        //        return;
        //        //    }
        //        //}
        //        str = "Select Round(IsNull(Sum(CrAmount-DrAmt),0),0) BalAmount From View_ProcessHissabApprovedPaymentAmt Where CompanyId=" + DDCompanyName.SelectedValue + " And Processid=" + DDProcessName.SelectedValue + " And EmpId=" + DDEmployerName.SelectedValue + " And Id=" + DDApprvNo.SelectedValue + " and Id!=0";
        //        ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            if (Convert.ToDouble(TxtAmt.Text) > Convert.ToDouble(ds.Tables[0].Rows[0]["BalAmount"]))
        //            {
        //                MessageSave("Amount Can not be greater than Balance Amount....");
        //                TxtBalAmt.Text = ds.Tables[0].Rows[0]["BalAmount"].ToString();
        //                Tran.Commit();
        //                return;
        //            }
        //        }
        //    }

        //    SqlParameter[] _arrpara1 = new SqlParameter[25];
        //    _arrpara1[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
        //    _arrpara1[1] = new SqlParameter("@ProcessID", SqlDbType.Int);
        //    _arrpara1[2] = new SqlParameter("@PartyId", SqlDbType.Int);
        //    _arrpara1[3] = new SqlParameter("@ApprovalNo", SqlDbType.Int);
        //    _arrpara1[4] = new SqlParameter("@Date", SqlDbType.SmallDateTime);
        //    _arrpara1[5] = new SqlParameter("@VoucherNo", SqlDbType.NVarChar);
        //    _arrpara1[6] = new SqlParameter("@CrDr", SqlDbType.NVarChar);
        //    _arrpara1[7] = new SqlParameter("@Amount", SqlDbType.Float);
        //    _arrpara1[8] = new SqlParameter("@ChkCash", SqlDbType.Int);
        //    _arrpara1[9] = new SqlParameter("@BankId", SqlDbType.Int);
        //    _arrpara1[10] = new SqlParameter("@ChqNo", SqlDbType.VarChar,50);
        //    _arrpara1[11] = new SqlParameter("@CrAmt", SqlDbType.Float);
        //    _arrpara1[12] = new SqlParameter("@DrAmt", SqlDbType.Float);
        //    _arrpara1[13] = new SqlParameter("@UId", SqlDbType.Int);
        //    _arrpara1[14] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        //    _arrpara1[15] = new SqlParameter("@Types", SqlDbType.Int);
        //    _arrpara1[16] = new SqlParameter("@Narration", SqlDbType.NVarChar);
        //    _arrpara1[17] = new SqlParameter("@PayMonth", SqlDbType.SmallDateTime);
        //    _arrpara1[18] = new SqlParameter("@AdvanceStatus", SqlDbType.Int);
        //    _arrpara1[19] = new SqlParameter("@FolioNo", SqlDbType.Int);
        //    _arrpara1[20] = new SqlParameter("@ByFolioStatus", SqlDbType.Int);
        //    _arrpara1[21] = new SqlParameter("@COMMPAYMENTFLAG", SqlDbType.Int);
        //    _arrpara1[22] = new SqlParameter("@USEADVAMT", TDuseadvanceamt.Visible == false ? "0" : (txtuseadvamt.Text == "" ? "0" : txtuseadvamt.Text));
        //    _arrpara1[23] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
        //    _arrpara1[23].Direction = ParameterDirection.Output;
        //    _arrpara1[24] = new SqlParameter("@PaymentTransferMode", SqlDbType.Int);

        //    _arrpara1[0].Value = DDCompanyName.SelectedValue;
        //    _arrpara1[1].Value = DDProcessName.SelectedValue;
        //    _arrpara1[2].Value = DDEmployerName.SelectedValue;
        //    if (chkByFolio.Checked == false)
        //    {
        //        _arrpara1[3].Value = DDApprvNo.SelectedValue;
        //    }
        //    else
        //    {
        //        _arrpara1[3].Value = 0;
        //    }
        //    _arrpara1[4].Value = Date.Text;
        //    _arrpara1[5].Value = "";
        //    _arrpara1[5].Direction = ParameterDirection.InputOutput;
        //    _arrpara1[6].Value = DDTrType.SelectedItem.Text;
        //    _arrpara1[7].Value = TxtAmt.Text;
        //    _arrpara1[8].Value = PayThrough.SelectedValue;
        //    _arrpara1[9].Value = PayThrough.SelectedValue == "1" ? DDBankName.SelectedValue : "0";
        //    _arrpara1[10].Value = PayThrough.SelectedValue == "1" ? TxtChkNo.Text : "0";
        //    _arrpara1[11].Value = DDTrType.SelectedValue == "1" ? TxtAmt.Text : "0";
        //    _arrpara1[12].Value = DDTrType.SelectedValue == "0" ? TxtAmt.Text : "0";
        //    _arrpara1[13].Value = Session["varuserid"];
        //    _arrpara1[14].Value = Session["varMasterCompanyIDForERP"];
        //    _arrpara1[15].Value = "1";
        //    _arrpara1[16].Value = TxtNarration.Text.Trim().ToString();
        //    _arrpara1[17].Value = System.DateTime.Now.ToString("dd-MMM-yyyy");
        //    _arrpara1[18].Value = ChkAdvanceAmt.Checked == true ? 1 : 0;
        //    if (chkByFolio.Checked == true)
        //    {
        //        _arrpara1[19].Value = DDApprvNo.SelectedValue;
        //    }
        //    else
        //    {
        //        _arrpara1[19].Value = 0;
        //    }
        //    _arrpara1[20].Value = chkByFolio.Checked == true ? 1 : 0;
        //    _arrpara1[21].Value = chkcommpay.Checked == true ? 1 : 0;
        //    _arrpara1[24].Value = TDPaymentTransferMode.Visible == false ? "0" : (DDPaymentTransferMode.SelectedIndex > 0 ? DDPaymentTransferMode.SelectedValue : "0"); 

        //    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_HissabPayment", _arrpara1);
        //    Tran.Commit();
        //    if (_arrpara1[23].Value.ToString() != "")
        //    {
        //        lblErr.Text = _arrpara1[23].Value.ToString();
        //    }
        //    else
        //    {
        //        TxtVocNo.Text = "";
        //        TxtAmt.Text = "";
        //        lblErr.Text = "Data successfully saved !";

        //    }
        //    ChkAdvanceAmt_CheckedChanged(sender, e);
        //    Fill_Grid();

        //}
        //catch (Exception ex)
        //{
        //    Tran.Rollback();
        //    lblErr.Visible = true;
        //    lblErr.Text = ex.Message;
        //}
        //finally
        //{
        //    con.Close();
        //    con.Dispose();
        //}
    }
    
    
    protected void Preview_Click(object sender, EventArgs e)
    {
        //DataSet DS = null;
        //DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from V_HissabPayment where ApprovalNo=" + DDApprvNo.SelectedItem.Text + " And ProcessId=" + DDProcessName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");

        //if (DS.Tables[0].Rows.Count > 0)
        //{
        //    Session["rptFileName"] = "~\\Reports\\RptProcessHissabPayment.rpt";
        //    Session["GetDataset"] = DS;
        //    Session["dsFileName"] = "~\\ReportSchema\\RptHissabPayment.xsd";
        //    StringBuilder stb = new StringBuilder();
        //    stb.Append("<script>");
        //    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
        //    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        //}
    }
  
    //protected void Rowdelete(int rowindex, object sender = null)
    //{
    //    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //    con.Open();
    //    SqlTransaction Tran = con.BeginTransaction();
    //    try
    //    {
    //        int key = Convert.ToInt32(dgPayment.DataKeys[rowindex].Value);
    //        SqlParameter[] param = new SqlParameter[9];
    //        param[0] = new SqlParameter("@srno", key);
    //        param[1] = new SqlParameter("@companyid", DDCompanyName.SelectedValue);
    //        param[2] = new SqlParameter("@processid", DDProcessName.SelectedValue);
    //        param[3] = new SqlParameter("@partyid", DDEmployerName.SelectedValue);
    //        param[4] = new SqlParameter("@approvalNo", DDApprvNo.SelectedValue);
    //        param[5] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
    //        param[6] = new SqlParameter("@userid", Session["varuserid"]);
    //        param[7] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
    //        param[7].Direction = ParameterDirection.Output;
    //        param[8] = new SqlParameter("@voucherno", dgPayment.Rows[rowindex].Cells[2].Text);

    //        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEPROCESSHISSABPAYMENT", param);
    //        lblErr.Text = param[7].Value.ToString();
    //        Tran.Commit();

    //        //            string del = @"Delete from processhissabpayment Where SrNo=" + key + " And CompanyID=" + DDCompanyName.SelectedValue + " And ProcessId=" + DDProcessName.SelectedValue + " and PartyId=" + DDEmployerName.SelectedValue + @" 
    //        //                           Select * From ProcessHissabPayment Where CompanyID=" + DDCompanyName.SelectedValue + " And ProcessId=" + DDProcessName.SelectedValue + " and PartyId=" + DDEmployerName.SelectedValue + " And ApprovalNo=" + DDApprvNo.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

    //        //            DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, del);

    //        //            if (chkByFolio.Checked == false)
    //        //            {
    //        //                if (Ds.Tables[0].Rows.Count == 0)
    //        //                {
    //        //                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Update ProcessHissabApproved Set Status=0 Where CompanyID=" + DDCompanyName.SelectedValue + " And ProcessId=" + DDProcessName.SelectedValue + " And EmpId=" + DDEmployerName.SelectedValue + " And AppvNo=" + DDApprvNo.SelectedValue);
    //        //                }
    //        //                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete From  AdvanceAmount where Hissab_VoucherNo=" + dgPayment.Rows[rowindex].Cells[2].Text + ""); // 2 fro voucherno
    //        //            }

    //        //            Tran.Commit();
    //        if (sender != null)
    //        {
    //            ChkAdvanceAmt_CheckedChanged(sender, new EventArgs());
    //        }
    //        Fill_Grid();
    //        //lblErr.Text = "Record Deleted Successfully !";
    //    }
    //    catch (Exception ex)
    //    {
    //        Tran.Rollback();
    //        lblErr.Visible = true;
    //        lblErr.Text = ex.Message;
    //    }
    //    finally
    //    {
    //        if (con.State == ConnectionState.Open)
    //        {
    //            con.Close();
    //            con.Dispose();
    //        }
    //    }
    //}
    private void Clear()
    {
        //TxtBalAmt.Text = "";
        //TxtCrAmt.Text = "";
        //TxtDrAmt.Text = "";
        //TxtAmt.Text = "";
        //TxtChkNo.Text = "";
        //TxtNarration.Text = "";
        //TxtVocNo.Text = "";
        //if (TDPaymentTransferMode.Visible == true)
        //{
        //    DDPaymentTransferMode.SelectedIndex = 0;
        //}
    }
    private void ClearEditFalse()
    {
        //DDProcessName.SelectedIndex = 0;
        //DDEmployerName.Items.Clear();
        //chkByFolio.Checked = false;
        //chkByFolio.Visible = false;
        //DDApprvNo.Items.Clear();
        //TxtBalAmt.Text = "";
        //TxtCrAmt.Text = "";
        //TxtDrAmt.Text = "";
        //TxtAmt.Text = "";
        //TxtChkNo.Text = "";
        //TxtNarration.Text = "";
        //TxtVocNo.Text = "";
        //dgPayment.DataSource = null;
        //dgPayment.DataBind();

        //if (TDPaymentTransferMode.Visible == true)
        //{
        //    DDPaymentTransferMode.SelectedIndex = 0;
        //}
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
    


    protected void chkcommpay_CheckedChanged(object sender, EventArgs e)
    {
       // DDApprvNo_SelectedIndexChanged(sender, new EventArgs());
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
        //lblErr.Text = "";
        //if (dgPayment.Rows.Count > 0)
        //{

        //    if (variable.VarPAYMENTDEL_PWD == txtpwd.Text)
        //    {
        //        Rowdelete(rowindex, sender);
        //        Popup(false);
        //    }
        //    else
        //    {
        //        lblErr.Text = "Please Enter Correct Password..";
        //    }

        //}
    }
    protected void lnkdel_Click(object sender, EventArgs e)
    {
        //Popup(true);
        //txtpwd.Focus();
        //LinkButton lnkdel = sender as LinkButton;
        //GridViewRow gvr = lnkdel.NamingContainer as GridViewRow;
        //rowindex = gvr.RowIndex;
    }
    protected void dgPayment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
        //    e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        //    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgPayment, "Select$" + e.Row.RowIndex);

        //    if (chkByFolio.Checked == true && DDApprvNo.SelectedIndex > 0)
        //    {
        //        dgPayment.Columns[5].Visible = true;
        //    }
        //    else
        //    {
        //        dgPayment.Columns[5].Visible = false;
        //    }
        //}
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
    //protected void GVApprovalAmt_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //DataSet ds = new DataSet();
    //    //int id =Convert.ToInt32(GVApprovalAmt.SelectedDataKey.Value.ToString());
    //    ////hnSizeId.Value = id;
        
    //    //    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //    //    if (con.State == ConnectionState.Closed)
    //    //    {
    //    //        con.Open();
    //    //    }
    //    //    SqlCommand cmd = new SqlCommand("PRO_GET_HISSABAPPROVALAMT_FORAUTOFILL", con);
    //    //    cmd.CommandType = CommandType.StoredProcedure;
    //    //    cmd.CommandTimeout = 300;

    //    //    cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedValue);
    //    //    cmd.Parameters.AddWithValue("@Where", "");
    //    //    cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyNo"]);
    //    //    cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
    //    //    cmd.Parameters.AddWithValue("@Id", id);

    //    //    // DataSet ds = new DataSet();
    //    //    SqlDataAdapter ad = new SqlDataAdapter(cmd);
    //    //    cmd.ExecuteNonQuery();
    //    //    ad.Fill(ds);
    //    //    //*************

    //    //    con.Close();
    //    //    con.Dispose();

    //    //    if (ds.Tables[0].Rows.Count > 0)
    //    //    {
    //    //        DDProcessName.SelectedValue = ds.Tables[0].Rows[0]["ProcessId"].ToString();
    //    //        ProcessNameSelectedIndexChanged();
    //    //        DDEmployerName.SelectedValue = ds.Tables[0].Rows[0]["EmpId"].ToString();
    //    //        DDEmployerName_SelectedIndexChanged(sender, new EventArgs());
    //    //        DDApprvNo.SelectedValue = ds.Tables[0].Rows[0]["Id"].ToString();
    //    //        DDApprvNo_SelectedIndexChanged(sender, new EventArgs());
               
    //    //    }
        
    //}
}