using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Hissab_purchase_hisab : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            CommanFunction.FillCombo(DDCompanyName, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + " order by CompanyName");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFill(ref DDPartyName, @"Select Distinct EI.EmpId,EI.EmpName from EmpInfo EI,PurchaseReceiveMaster PII Where EI.Empid=PII.Partyid And Challan_status=0 And 
                PII.CompanyID = " + DDCompanyName.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"] + @" Union 
                Select Distinct EI.EmpId,EI.EmpName from EmpInfo EI,PurchaseHissab PH Where EI.Empid=PH.Partyid And billstatus=0 And PH.CompanyID = " + DDCompanyName.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"] + " Order By EI.EmpName", true, "--Select Employee--");
            TDBillNo.Visible = false;
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtBillDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewState["phissabid"] = 0;
            if (Session["VarCompanyNo"].ToString() == "42")
            {
                Textpamt.ReadOnly = true;
                TDDeductionAmt.Visible = true;
            }
        }
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["phissabid"] = 0;
        PartyNameSelectedChange();
        fillgrid();
    }
    private void PartyNameSelectedChange()
    {
        if (DDPartyName.SelectedIndex > 0 && ChkEditOrder.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDBillNo, "Select PhissabId,BillNo From PurchaseHissab Where billstatus=0 And CompanyId=" + DDCompanyName.SelectedValue + " And PartyID=" + DDPartyName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Bill No--");
        }
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            TDBillNo.Visible = true;
            PartyNameSelectedChange();
            BtnPreview.Visible = true;
            if (Session["VarCompanyNo"].ToString() == "44")
            {
                BtnPreview.Text = "Payment Voucher";
                BtnPreview.Width = 150;
            }
            else {

                BtnPreview.Text = "Preview";
            }
            
            if (Session["VarCompanyNo"].ToString() == "16")
            {
                BtnSave.Visible = false;
            }
            
        }
        else
        {
            TDBillNo.Visible = false;
            BtnPreview.Visible = false;
        }
        fillgrid();
        Txttotamt.Text = "";
        Textbillno.Text = "";
        Textpamt.Text = "";
        txtremark.Text = "";
    }
    protected void DDBillNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["phissabid"] = DDBillNo.SelectedValue;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select BillNo,Amount,replace(convert(varchar(11),Date,106), ' ','-') Date,Remark,BillAmt,replace(convert(varchar(11),BillDate,106), ' ','-') BillDate,DebitAmt,isnull(DeductionAmt,0) as DeductionAmt From PurchaseHissab Where Phissabid=" + DDBillNo.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            Textbillno.Text = Ds.Tables[0].Rows[0]["BillNo"].ToString();
            TxtBillAmount.Text = Ds.Tables[0].Rows[0]["BillAmt"].ToString();
            TxtBillDate.Text = Ds.Tables[0].Rows[0]["BillDate"].ToString();
            Textpamt.Text = Ds.Tables[0].Rows[0]["Amount"].ToString();
            TxtDate.Text = Ds.Tables[0].Rows[0]["Date"].ToString();
            txtremark.Text = Ds.Tables[0].Rows[0]["Remark"].ToString();
            txtDebitAmt.Text = Ds.Tables[0].Rows[0]["DebitAmt"].ToString();
            txtDeductionAmt.Text = Ds.Tables[0].Rows[0]["DeductionAmt"].ToString();
        }
        fillgrid();
    }
    private void fillgrid()
    {
        double tot = 0;
        double DebitAmt = 0;
        //        string Str = @"Select prm.purchasereceiveid,billno as challanno,isnull(Round(sum(((Qty-isnull(ReturnQty,0))*Rate+(Qty-isnull(ReturnQty,0))*Rate*vat/100+(Qty-isnull(ReturnQty,0))*Rate*cst/100)-Penalty),0),0) total,0 Flag
        //                    From PurchaseReceiveDetail prd inner join PurchaseReceiveMaster prm on prd.purchasereceiveid=prm.purchasereceiveid left outer join V_PurchaseReturnQty V On prm.PurchaseReceiveid=V.PurchaseReceiveId                                          
        //                    Where prm.partyid=" + DDPartyName.SelectedValue + " and prm.companyid=" + DDCompanyName.SelectedValue + @" And 
        //                    Challan_status=0 And Prm.MasterCompanyId=" + Session["varCompanyId"] + @" And prm.PurchaseReceiveId not in (Select purchasereceiveid From Bill_ChallanDetail) 
        //                    Group By prm.purchasereceiveid,billno";
        string Str = @"select PurchaseReceiveId,Challanno,isnull(Round(Sum(total),0),0) + Freight Total, 0 flag,
                dbo.F_DebitAmtForPurchase(CompanyId,PartyId,PurchaseReceiveId,MasterCompanyId) DebitAmt 
                From V_PurchaseHissabDetail(nolock) Where partyId=" + DDPartyName.SelectedValue + " And CompanyId=" + DDCompanyName.SelectedValue + @" And
                Challan_Status=0 And MasterCompanyId=" + Session["varCompanyId"] + @" And 
                PurchaseReceiveId not in (select PurchaseReceiveId From Bill_ChallanDetail(nolock)) 
                group by PurchaseReceiveId,ChallanNo,CompanyId,MasterCompanyId,PartyId, Freight";
        if (ChkEditOrder.Checked == true && DDBillNo.SelectedIndex > 0)
        {
            Str = Str + @" Union Select purchasereceiveid,challanno,isnull(Round(sum(total),0),0) + Freight total,1 Flag,
                    dbo.F_DebitAmtForPurchase(CompanyId,PartyId,PurchaseReceiveId,MasterCompanyId) DebitAmt
                    From V_PurchaseHissabDetail(nolock)
                    Where purchasereceiveid in (Select purchasereceiveid From Bill_ChallanDetail(nolock) Where billid=" + DDBillNo.SelectedValue + @") And 
                    MasterCompanyId=" + Session["varCompanyId"] + @" 
                    Group By purchasereceiveid,Challanno,CompanyId,MasterCompanyId,Partyid, Freight";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        DGChallanDetail.DataSource = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGChallanDetail.DataBind();
            if (ChkEditOrder.Checked == true)
            {
                for (int i = 0; i < DGChallanDetail.Rows.Count; i++)
                {
                    if (Convert.ToInt32(DGChallanDetail.Rows[i].Cells[3].Text) == 1)
                    {
                        ((CheckBox)DGChallanDetail.Rows[i].FindControl("Chkbox")).Checked = true;
                        tot = tot + Convert.ToDouble(DGChallanDetail.Rows[i].Cells[2].Text);
                        DebitAmt = DebitAmt + Convert.ToDouble(DGChallanDetail.Rows[i].Cells[4].Text); //Debit Amount
                    }
                }
            }
            Txttotamt.Text = tot.ToString();
            txtDebitAmt.Text = DebitAmt.ToString();
            DGChallanDetail.Visible = true;
        }
        else
        {
            DGChallanDetail.Visible = false;
        }
    }
    protected void Chkchallan_CheckedChanged(object sender, EventArgs e)
    {
        fillAmt();
    }
    protected void txtDeductionAmt_TextChanged(object sender, EventArgs e)
    {
        fillAmt();
    }
    private void fillAmt()
    {
        double tot = 0;
        double DebitAmt = 0;
        for (int i = 0; i < DGChallanDetail.Rows.Count; i++)
        {
            if (((CheckBox)DGChallanDetail.Rows[i].FindControl("Chkbox")).Checked == true)
            {
                tot = tot + Convert.ToDouble(DGChallanDetail.Rows[i].Cells[2].Text);
                DebitAmt = DebitAmt + Convert.ToDouble(DGChallanDetail.Rows[i].Cells[4].Text);//DebitAmount
            }
        }
        Txttotamt.Text = tot.ToString();
        Textpamt.Text = Txttotamt.Text;
        txtDebitAmt.Text = DebitAmt.ToString();

        if (Session["VarCompanyNo"].ToString() == "42")
        {
            Textpamt.Text = (Convert.ToDouble(Textpamt.Text) - Convert.ToDouble(txtDeductionAmt.Text)).ToString();
        }
        else
        {

            Textpamt.Text = (Convert.ToDouble(Textpamt.Text) - Convert.ToDouble(txtDebitAmt.Text)).ToString();
        }

    }
    private void CheckDuplicateBillNo()
    {
        string Str = "Select * From PurchaseHissab Where BillNo='" + Textbillno.Text + "' And PartyID=" + DDPartyName.SelectedValue + " And PhissabId<>" + ViewState["phissabid"] + " And MasterCompanyId=" + Session["varCompanyId"];
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            LblErrorMessage.Text = "Duplicate bill no exists..";
            Textbillno.Text = "";
            Textbillno.Focus();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Duplicate bill no exists..!');", true);
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        CheckDuplicateBillNo();
        if (LblErrorMessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[15];

                _arrPara[0] = new SqlParameter("@phissabid", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@companyid", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@userid", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@billno", SqlDbType.NVarChar);
                _arrPara[5] = new SqlParameter("@partyid", SqlDbType.Int);
                _arrPara[6] = new SqlParameter("@amount", SqlDbType.Float);
                _arrPara[7] = new SqlParameter("@date", SqlDbType.SmallDateTime);
                _arrPara[8] = new SqlParameter("@remark", SqlDbType.NVarChar);
                _arrPara[9] = new SqlParameter("@purchasereceiveid", SqlDbType.NVarChar);
                _arrPara[10] = new SqlParameter("@BillAmount", SqlDbType.Float);
                _arrPara[11] = new SqlParameter("@BillDate", SqlDbType.SmallDateTime);
                _arrPara[12] = new SqlParameter("@DebitAmt", SqlDbType.Float);
                _arrPara[13] = new SqlParameter("@MSG", SqlDbType.VarChar, 300);
                _arrPara[14] = new SqlParameter("@DeductionAmt", SqlDbType.Float); 


                _arrPara[0].Direction = ParameterDirection.InputOutput;
                _arrPara[0].Value = ViewState["phissabid"];
                _arrPara[1].Value = DDCompanyName.SelectedValue;
                _arrPara[2].Value = Session["varCompanyId"];
                _arrPara[3].Value = Session["varuserid"];
                _arrPara[4].Value = Textbillno.Text;
                _arrPara[5].Value = DDPartyName.SelectedValue;
                _arrPara[6].Value = Textpamt.Text != "" ? Convert.ToDouble(Textpamt.Text) : 0;
                _arrPara[7].Value = TxtDate.Text;
                _arrPara[8].Value = txtremark.Text;
                for (int i = 0; i < DGChallanDetail.Rows.Count; i++)
                {
                    if (((CheckBox)DGChallanDetail.Rows[i].FindControl("Chkbox")).Checked == true)
                    {
                        if (_arrPara[9].Value == null)
                        {
                            _arrPara[9].Value = ((Label)DGChallanDetail.Rows[i].FindControl("lblpreceiveid")).Text;
                        }
                        else
                        {
                            _arrPara[9].Value = _arrPara[9].Value.ToString() + ',' + ((Label)DGChallanDetail.Rows[i].FindControl("lblpreceiveid")).Text;
                        }
                    }
                }
                _arrPara[10].Value = TxtBillAmount.Text != "" ? Convert.ToDouble(TxtBillAmount.Text) : 0;
                _arrPara[11].Value = TxtBillDate.Text;
                _arrPara[12].Value = txtDebitAmt.Text != "" ? txtDebitAmt.Text : "0";
                _arrPara[13].Direction = ParameterDirection.Output;
                _arrPara[14].Value = txtDeductionAmt.Text != "" ? txtDeductionAmt.Text : "0";

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PurchaseHissab", _arrPara);
                ViewState["phissabid"] = _arrPara[0].Value;
                Tran.Commit();
                refresh();
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = _arrPara[13].Value.ToString();
                //LblErrorMessage.Text = "Data Saved Successfully";
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Hissab/purchase_hisab.aspx");
                Tran.Rollback();
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = ex.Message;

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    private void refresh()
    {
        txtremark.Text = "";
        Txttotamt.Text = "";
        Textbillno.Text = "";
        Textpamt.Text = "";
        txtDebitAmt.Text = "0";
        TxtBillAmount.Text = "";
        txtDeductionAmt.Text = "";
        fillgrid();
    }
    protected void DGChallanDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
        e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGChallanDetail, "Select$" + e.Row.RowIndex);
    }
    //protected void DGChallanDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    //protected void DGBILLDETAIL_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void BtnPurchasePreview_Click(object sender, EventArgs e)
    {
        string str = "";
        string PurchaseReceiveID = "";
        for (int i = 0; i < DGChallanDetail.Rows.Count; i++)
        {
            if (((CheckBox)DGChallanDetail.Rows[i].FindControl("Chkbox")).Checked == true)
            {
                if (PurchaseReceiveID == "")
                {
                    PurchaseReceiveID = ((Label)DGChallanDetail.Rows[i].FindControl("lblpreceiveid")).Text;
                }
                else
                {
                    PurchaseReceiveID = PurchaseReceiveID + ',' + ((Label)DGChallanDetail.Rows[i].FindControl("lblpreceiveid")).Text;
                }
            }
        }

        if (PurchaseReceiveID == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('No Records found')", true);
            return;
        }
        str = @"SELECT vo.empname,vo.pindentissueid,vo.finishedid,vo.item_description as description,
            qty as orderqty,vo.orderdate,vo.recdate,vo.recqty,'' FromDate,'' as ToDate,
            vo.CanQty OrdercanQty,vo.ReturnDate,vo.ReturnChallan,vo.qtyreturn,vo.PO,vo.Localorder,vo.customercode,vo.deliverydate,
            vo.rate,vo.LshortPercentage,vo.pindentissuetranid,vo.Category_name,vo.Status,Vo.colour,vo.Recqty_beforeRetnqty, IsNull(vo.GATEINNO, '') GateInNo, vo.PURCHASERECEIVEID 
            From v_purchase_emp_order_wise vo(Nolock)
            inner join V_FinishedItemDetail vf(Nolock) ON vo.finishedid=vf.ITEM_FINISHED_ID 
            where vo.CompanyId = " + DDCompanyName.SelectedValue + " And vo.Empid = " + DDPartyName.SelectedValue;

        if (PurchaseReceiveID != "")
        {
            str = str + " And vo.pindentissueid in (Select pindentissueid From PurchaseReceiveDetail(Nolock) Where PurchasereceiveID in (" + PurchaseReceiveID + "))";
        }
        str = str + @" order by vo.pindentissueid ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["Getdataset"] = ds;
            Session["rptFilename"] = "Reports/RptpurchaseVendor.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptpurchaseVendor.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('No Records found')", true);
        }
    }

    protected void Preview_Click(object sender, EventArgs e)
    {
       
            DataSet ds = null;
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@Phissabid", ViewState["phissabid"]);           
            param[1] = new SqlParameter("@MasterCompanyId", Session["VarCompanyNo"]);
            param[2] = new SqlParameter("@UserId", Session["VarUserId"]);

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetPurchaseHissabReport", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Session["VarCompanyNo"].ToString() == "44")
                {
                    Session["rptFileName"] = "~\\Reports\\RptPurchaseHissabReport_agni.rpt";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\RptPurchaseHissabReport.rpt";
                }
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseHissabReport.xsd";
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