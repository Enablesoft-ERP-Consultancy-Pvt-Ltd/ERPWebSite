using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Hissab_PurchaseBillVoucher : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            Session["voucherno"] = "";
            CommanFunction.FillCombo(DDCompanyName, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFill(ref DDPartyName, "Select empid,empName from empinfo where empid in(select partyid  from PurchaseHissab where CompanyID = " + DDCompanyName.SelectedValue + " billstatus=1) And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by empname", true, "--Select--");
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDPartyName.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref ddbillno, "select phissabid,billno from PurchaseHissab where billstatus=1 and partyid=" + DDPartyName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "-Select-");
            LblErrorMessage.Visible = false;
            TxtvoucherNo.Text = "";
        }
    }

    protected void ddbillno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddbillno.SelectedIndex > 0)
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(sum(payAmount),0)from PurchaseBillDetail  where phissabid=" + ddbillno.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
            DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(sum(payment),0) from PurchaseVoucher where phissabid=" + ddbillno.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
            TxtPayment.Text = Convert.ToString(Convert.ToDouble(ds.Tables[0].Rows[0][0].ToString()) - Convert.ToDouble(ds1.Tables[0].Rows[0][0].ToString()));
            Txtamount.Text = TxtPayment.Text;
            fillgrid();
        }
    }

    protected void Txttds_TextChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(TxtPayment.Text) > 0)
        {
            if (Txttds.Text == "")
            {
                Txttds.Text = "0";
            }
            Txtamount.Text = Convert.ToString(Convert.ToDouble(TxtPayment.Text) - Convert.ToDouble(Txttds.Text));
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            Session["phissabid"] = 0;

            SqlParameter[] _arrPara = new SqlParameter[16];
            _arrPara[0] = new SqlParameter("@pvoucherid", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@phissabid", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@companyid", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@userid", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@partyid", SqlDbType.Int);
            _arrPara[6] = new SqlParameter("@paydate", SqlDbType.SmallDateTime);
            _arrPara[7] = new SqlParameter("@cr_dr", SqlDbType.Int);
            _arrPara[8] = new SqlParameter("@payment", SqlDbType.Float);
            _arrPara[9] = new SqlParameter("@tds", SqlDbType.Float);
            _arrPara[10] = new SqlParameter("@amount", SqlDbType.Float);
            _arrPara[11] = new SqlParameter("@paymentby", SqlDbType.Int);
            _arrPara[12] = new SqlParameter("@bankname", SqlDbType.NVarChar);
            _arrPara[13] = new SqlParameter("@chequeno", SqlDbType.NVarChar);
            _arrPara[14] = new SqlParameter("@narration", SqlDbType.NVarChar);
            _arrPara[0].Direction = ParameterDirection.InputOutput;
            _arrPara[0].Value = 0;
            _arrPara[1].Value = ddbillno.SelectedValue;
            _arrPara[2].Value = DDCompanyName.SelectedValue;
            _arrPara[3].Value = Session["varMasterCompanyIDForERP"];
            _arrPara[4].Value = Session["varuserid"];
            _arrPara[5].Value = DDPartyName.SelectedValue;
            _arrPara[6].Value = TxtDate.Text;
            _arrPara[7].Value = DDcr.SelectedValue;
            _arrPara[8].Value = TxtPayment.Text != "" ? Convert.ToDouble(TxtPayment.Text) : 0;
            _arrPara[9].Value = Txttds.Text != "" ? Convert.ToDouble(Txttds.Text) : 0;
            _arrPara[10].Value = Txtamount.Text != "" ? Convert.ToDouble(Txtamount.Text) : 0; ;
            _arrPara[11].Value = ddpaymentby.SelectedValue;
            _arrPara[12].Value = Txtbank.Text;
            _arrPara[13].Value = Txtcheque.Text;
            _arrPara[14].Value = Txtnarration.Text;
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_PurchaseVoucher", _arrPara);
            TxtvoucherNo.Text = Convert.ToString(_arrPara[0].Value);
            Session["voucherno"] = Convert.ToString(_arrPara[0].Value);
            LblErrorMessage.Text = "Data Saved";
            refresh();
            report();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Hissab/PurchaseBillVoucher.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void ddpaymentby_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddpaymentby.SelectedValue == "1")
        {
            tdbank.Visible = true;
            tdcheque.Visible = true;
            tdnarr.Visible = true;
        }
        else
        {
            tdbank.Visible = false;
            tdcheque.Visible = false;
            tdnarr.Visible = false;
        }
    }
    private void refresh()
    {
        DDPartyName.SelectedIndex = 0;
        ddbillno.SelectedIndex = 0;
        TxtPayment.Text = "";
        Txttds.Text = "";
        Txtamount.Text = "";
        Txtbank.Text = "";
        Txtcheque.Text = "";
        Txtnarration.Text = "";
        DGVOUCHERDetail.Visible = false;
        btnprivew.Enabled = true;
    }

    private void fillgrid()
    {
        try
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select pvoucherid as vouchorno,replace(convert(varchar(11),paydate,106),' ','-') as date,pv.amount,ph.billno from PurchaseVoucher pv,PurchaseHissab ph
                                               where ph.phissabid=pv.phissabid and pv.partyid=" + DDPartyName.SelectedValue + " and pv.phissabid=" + ddbillno.SelectedValue + " And PV.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
            DGVOUCHERDetail.DataSource = ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                DGVOUCHERDetail.DataBind();
                DGVOUCHERDetail.Visible = true;

            }
            else
            {
                DGVOUCHERDetail.Visible = false;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Hissab/PurchaseBillVoucher.aspx");
        }
    }
    protected void DGVOUCHERDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        int n = DGVOUCHERDetail.SelectedIndex;
        Session["voucherno"] = DGVOUCHERDetail.Rows[n].Cells[0].Text;
        report();
        btnprivew.Enabled = true;
    }
    private void report()
    {
        Session["ReportPath"] = "Reports/Rpt_Purchase_Vouchor.rpt";
        Session["CommanFormula"] = "{PurchaseVoucher.pvoucherid} =" + Session["voucherno"].ToString() + "";
    }
    protected void DGVOUCHERDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void DGVOUCHERDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
        e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGVOUCHERDetail, "Select$" + e.Row.RowIndex);
    }
    protected void DGVOUCHERDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
}