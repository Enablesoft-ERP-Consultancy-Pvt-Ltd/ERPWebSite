using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Hissab_PurchaseBillDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            CommanFunction.FillCombo(DDCompanyName, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    private void fillgrid()
    {
        try
        {
            DataSet ds = new DataSet();
            if (DDbillName.SelectedValue == "1")
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select empname as party,partyid as partyid,phissabid,billno,Amount,replace(convert(varchar(11),Date,106), ' ','-') as date from PurchaseHissab ph,empinfo e where ph.partyid=e.empid  and phissabid not in(select phissabid from PurchaseBillDetail where BILLStatus=1) and ph.companyid=" + DDCompanyName.SelectedValue + " And E.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
            else if (DDbillName.SelectedValue == "2")
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select empname as party,partyid as partyid,phissabid,billno,Amount,replace(convert(varchar(11),Date,106), ' ','-') as date from PurchaseHissab ph,empinfo e where ph.partyid=e.empid and billstatus=1 and phissabid not in(select phissabid from PurchaseBillDetail where BILLStatus=0) and ph.companyid=" + DDCompanyName.SelectedValue + " And E.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
            DGbillnoDetail.DataSource = ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                DGbillnoDetail.DataBind();
                DGbillnoDetail.Visible = true;
                visible();
                LblErrorMessage.Visible = false;
            }
            else
            {
                DGbillnoDetail.Visible = false;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Hissab/PurchaseBillDetail.aspx");
        }
    }
    public string getgiven(string strVal, string strval1)
    {
        string val = "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(sum(payamount),0) from PurchaseBillDetail where phissabid=" + strVal + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
        val = Convert.ToString(Convert.ToDouble(strval1) - Convert.ToDouble(ds.Tables[0].Rows[0][0].ToString()));
        hnbal.Value = val;
        return val;
    }
    public string getamt()
    {
        string val = "";
        val = hnbal.Value;
        return val;
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[12];

            _arrPara[0] = new SqlParameter("@pbillid", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@phissabid", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@companyid", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@userid", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@partyid", SqlDbType.Int);
            _arrPara[6] = new SqlParameter("@billno", SqlDbType.NVarChar);
            _arrPara[7] = new SqlParameter("@date", SqlDbType.SmallDateTime);
            _arrPara[8] = new SqlParameter("@payamount", SqlDbType.Float);
            _arrPara[9] = new SqlParameter("@billstatus", SqlDbType.Int);
            _arrPara[10] = new SqlParameter("@balance", SqlDbType.Float);

            for (int i = 0; i < DGbillnoDetail.Rows.Count; i++)
            {
                if (((CheckBox)DGbillnoDetail.Rows[i].FindControl("Chkbox")).Checked == true)
                {
                    _arrPara[0].Value = 0;
                    _arrPara[1].Value = Convert.ToInt32(((Label)DGbillnoDetail.Rows[i].FindControl("lblphissabidid")).Text);
                    _arrPara[2].Value = DDCompanyName.SelectedValue;
                    _arrPara[3].Value = Session["varMasterCompanyIDForERP"];
                    _arrPara[4].Value = Session["varuserid"];
                    _arrPara[5].Value = ((Label)DGbillnoDetail.Rows[i].FindControl("lblpartyid")).Text;
                    _arrPara[6].Value = DGbillnoDetail.Rows[i].Cells[2].Text;
                    _arrPara[7].Value = TxtDate.Text;
                    _arrPara[8].Value = ((TextBox)DGbillnoDetail.Rows[i].FindControl("TXTamt")).Text;
                    _arrPara[9].Value = 0;
                    _arrPara[10].Value = Convert.ToDouble(((Label)DGbillnoDetail.Rows[i].FindControl("lblbalnce")).Text);
                    SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_PurchaseBillDetail", _arrPara);
                    LblErrorMessage.Text = "Data Saved";
                    LblErrorMessage.Visible = true;
                    refresh();
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Hissab/PurchaseBillDetail.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
    }
    private void refresh()
    {
        DDbillName.SelectedIndex = 0;
        DGbillnoDetail.Visible = false;
        DGchallanDETAIL.Visible = false;
    }

    protected void DDbillName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDbillName.SelectedIndex > 0)
        {
            fillgrid();
        }
        else
        {
            DGbillnoDetail.Visible = false;
            DGchallanDETAIL.Visible = false;
        }
    }
    protected void DGbillnoDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        hnrow.Value = Convert.ToString(index);
        if (hnbutt.Value == "1")
        {
            int n = Convert.ToInt32(hnrow.Value);
            hnhissabid.Value = ((Label)DGbillnoDetail.Rows[n].FindControl("lblphissabidid")).Text;
            hnbill.Value = ((Label)DGbillnoDetail.Rows[n].FindControl("lblbillno")).Text;
            hnpartyid.Value = ((Label)DGbillnoDetail.Rows[n].FindControl("lblpartyid")).Text;
            fillchallan_grid();
            hnbutt.Value = "0";
        }
    }
    protected void DGbillnoDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
        e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGbillnoDetail, "Select$" + e.Row.RowIndex);
    }
    protected void DGbillnoDetail_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    private void fillchallan_grid()
    {
        try
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select billno as challanno,sum(prd.rate*prd.qty) Amt,replace(convert(varchar(11),receiveDate,106), ' ','-') as date
                     from PurchaseReceiveDetail prd inner join 
                    PurchaseReceiveMaster prm on prd.purchasereceiveid=prm.purchasereceiveid                                          
                    where prm.billno in(select challanno from Bill_ChallanDetail where billid=" + hnhissabid.Value + " ) and prm.partyid=" + hnpartyid.Value + " And prm.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  group by billno,billno1,receiveDate");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DGchallanDETAIL.DataSource = ds;
                DGchallanDETAIL.DataBind();
                DGchallanDETAIL.Visible = true;
            }
            else
            {
                DGchallanDETAIL.Visible = false;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Hissab/PurchaseBillDetail.aspx");
        }
    }
    protected void BTNdetail_Click(object sender, EventArgs e)
    {
        hnbutt.Value = "1";
    }
    private void visible()
    {
        if (DDbillName.SelectedValue == "2")
        {
            for (int i = 0; i < DGbillnoDetail.Rows.Count; i++)
            {
                ((TextBox)DGbillnoDetail.Rows[i].FindControl("TXTamt")).Enabled = false;
                ((CheckBox)DGbillnoDetail.Rows[i].FindControl("Chkbox")).Enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < DGbillnoDetail.Rows.Count; i++)
            {
                ((TextBox)DGbillnoDetail.Rows[i].FindControl("TXTamt")).Enabled = true;
                ((CheckBox)DGbillnoDetail.Rows[i].FindControl("Chkbox")).Enabled = true;
            }
        }
    }
    protected void DGbillnoDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGchallanDETAIL_RowCreated(object sender, GridViewRowEventArgs e)
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