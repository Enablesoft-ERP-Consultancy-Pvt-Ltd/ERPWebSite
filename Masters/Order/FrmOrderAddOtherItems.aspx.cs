using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;
public partial class Masters_Order_FrmOrderAddOtherItems : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            VarID.Value = "0";
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + " order by CompanyName", true, "--SELECT--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
                CompanySelectedChange();
            }
            UtilityModule.ConditionalComboFill(ref DDCurrency, "SELECT CurrencyId,CurrencyName from CurrencyInfo Where MasterCompanyId=" + Session["varCompanyId"] + " order by CurrencyName", true, "-SELECT-");
            DescriptionWithValueChange();
        }
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedChange();
    }
    private void CompanySelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDCustomerCode, @"SELECT Distinct CI.Customerid,CI.CompanyName + SPACE(5)+CI.Customercode From OrderMaster OM,OrderDetail OD,Customerinfo CI Where OM.OrderId=OD.OrderId And OM.CustomerID=CI.CustomerID And OM.CompanyId=" + DDCompanyName.SelectedValue + " And CI.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
        if (DDCustomerCode.Items.Count > 0)
        {
            DDCustomerCode.SelectedIndex = 1;
            CustomerCodeSelectedChange();
        }
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerCodeSelectedChange();
    }
    private void CustomerCodeSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDOrderNo, "Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo From OrderMaster OM,OrderDetail OD Where OM.OrderId=OD.OrderId And OM.CompanyId=" + DDCompanyName.SelectedValue + " And OM.Customerid=" + DDCustomerCode.SelectedValue, true, "--SELECT--");
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        OrderNoSelectedChange();

    }
    private void OrderNoSelectedChange()
    {
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Distinct CurrencyId,OrderCalType from OrderDetail Where OrderId=" + DDOrderNo.SelectedValue);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            DDCurrency.SelectedValue = Ds.Tables[0].Rows[0]["CurrencyId"].ToString();
            DDCalType.SelectedValue = Ds.Tables[0].Rows[0]["OrderCalType"].ToString();
            CalTypeSelectChange();
            Report_Type();
        }
        FillShowGrid();
    }
    private void FillShowGrid()
    {
        string Str = @"Select ID,GroupName,BuyerCode,OurCode,Description,Unit,Qty,Rate,Amount From OrderExtraItemDetail Where Type=0 And OrderId=" + DDOrderNo.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + @"
                Union Select ID,GroupName,BuyerCode,OurCode,DescriptionValueText Description,Unit,Qty,Rate,DescriptionValue Amount From OrderExtraItemDetail Where Type=1 And OrderId=" + DDOrderNo.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        DGOrderDetail.DataSource = Ds;
        DGOrderDetail.DataBind();
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[33];
            _arrpara[0] = new SqlParameter("@ID", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@OrderId", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@CalType", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@CurrencyId", SqlDbType.Int);
            _arrpara[4] = new SqlParameter("@GroupName", SqlDbType.NVarChar);
            _arrpara[5] = new SqlParameter("@BuyerCode", SqlDbType.NVarChar);
            _arrpara[6] = new SqlParameter("@OurCode", SqlDbType.NVarChar);
            _arrpara[7] = new SqlParameter("@Description", SqlDbType.NVarChar);
            _arrpara[8] = new SqlParameter("@Unit", SqlDbType.NVarChar);
            _arrpara[9] = new SqlParameter("@Qty", SqlDbType.Float);
            _arrpara[10] = new SqlParameter("@Rate", SqlDbType.Float);
            _arrpara[11] = new SqlParameter("@Area", SqlDbType.Float);
            _arrpara[12] = new SqlParameter("@Amount", SqlDbType.Float);
            _arrpara[13] = new SqlParameter("@Type", SqlDbType.Int);
            _arrpara[14] = new SqlParameter("@DescriptionValueText", SqlDbType.NVarChar);
            _arrpara[15] = new SqlParameter("@DescriptionValue", SqlDbType.Float);
            _arrpara[16] = new SqlParameter("@VarUser", SqlDbType.Int);
            _arrpara[17] = new SqlParameter("@MasterCompany", SqlDbType.Int);


            _arrpara[0].Value = VarID.Value;
            _arrpara[1].Value = DDOrderNo.SelectedValue;
            _arrpara[2].Value = DDCalType.SelectedValue;
            _arrpara[3].Value = DDCurrency.SelectedValue;
            _arrpara[4].Value = TxtGroupName.Text;
            _arrpara[5].Value = TxtBuyerCode.Text;
            _arrpara[6].Value = TxtOurCode.Text;
            _arrpara[7].Value = TxtDescription.Text;
            _arrpara[8].Value = TxtUnit.Text == "" ? "" : TxtUnit.Text;
            _arrpara[9].Value = TxtQty.Text == "" ? "0" : TxtQty.Text;
            _arrpara[10].Value = TxtRate.Text == "" ? "0" : TxtRate.Text;
            _arrpara[11].Value = TDArea.Visible == true ? TxtArea.Text : "0";
            _arrpara[12].Value = Convert.ToInt32(DDCalType.SelectedValue) == 0 ? Convert.ToDouble(_arrpara[9].Value) * Convert.ToDouble(_arrpara[10].Value) * Convert.ToDouble(_arrpara[11].Value) : Convert.ToDouble(_arrpara[9].Value) * Convert.ToDouble(_arrpara[10].Value);
            _arrpara[13].Value = ChkForAddDescriptionWithValue.Checked == true ? 1 : 0;
            _arrpara[14].Value = TxtDescriptionText.Text;
            _arrpara[15].Value = TxtDescriptionWithValue.Text == "" ? "0" : TxtDescriptionWithValue.Text;
            _arrpara[16].Value = Session["varuserid"];
            _arrpara[17].Value = Session["VarcompanyNo"];

            con.Open();
            SqlTransaction tran = con.BeginTransaction();

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_OrderExtraItemAdd", _arrpara);
            tran.Commit();
            VarID.Value = "0";
            BtnSave.Text = "Save";
            FillShowGrid();
            AfterSaveClear();
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void AfterSaveClear()
    {
        TxtBuyerCode.Text = "";
        TxtOurCode.Text = "";
        TxtDescription.Text = "";
        //TxtUnit.Text = "";
        TxtQty.Text = "";
        TxtRate.Text = "";
        TxtAmount.Text = "";
        TxtDescriptionText.Text = "";
        TxtDescriptionWithValue.Text = "";
    }
    protected void DGOrderDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = null;
        try
        {
            string sql = @"Select ID,OrderId,CalType,CurrencyId,GroupName,BuyerCode,OurCode,Description,Unit,Qty,Rate,Amount,Type,DescriptionValueText,DescriptionValue 
            From OrderExtraItemDetail Where OrderId=" + DDOrderNo.SelectedValue + " And Id=" + DGOrderDetail.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                VarID.Value = DGOrderDetail.SelectedValue.ToString();
                DDCalType.SelectedValue = ds.Tables[0].Rows[0]["CalType"].ToString();
                DDCurrency.SelectedValue = ds.Tables[0].Rows[0]["CurrencyId"].ToString();
                ChkForAddDescriptionWithValue.Checked = false;
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["Type"]) == 1)
                {
                    ChkForAddDescriptionWithValue.Checked = true;
                    DescriptionWithValueChange();
                    TxtDescriptionText.Text = ds.Tables[0].Rows[0]["DescriptionValueText"].ToString();
                    TxtDescriptionWithValue.Text = ds.Tables[0].Rows[0]["DescriptionValue"].ToString();
                }
                else
                {
                    TxtGroupName.Text = ds.Tables[0].Rows[0]["GroupName"].ToString();
                    TxtBuyerCode.Text = ds.Tables[0].Rows[0]["BuyerCode"].ToString();
                    TxtOurCode.Text = ds.Tables[0].Rows[0]["OurCode"].ToString();
                    TxtDescription.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                    TxtUnit.Text = ds.Tables[0].Rows[0]["Unit"].ToString();
                    TxtQty.Text = ds.Tables[0].Rows[0]["Qty"].ToString();
                    TxtRate.Text = ds.Tables[0].Rows[0]["Rate"].ToString();
                    TxtAmount.Text = ds.Tables[0].Rows[0]["Amount"].ToString();
                }
                BtnSave.Text = "Update";
                LblErrorMessage.Visible = false;
            }
        }
        catch (Exception ex)
        {
            VarID.Value = "0";
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
        }
    }
    protected void DGOrderDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        LblErrorMessage.Text = "";
        int VarDetailId = Convert.ToInt32(DGOrderDetail.DataKeys[e.RowIndex].Value);
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "Delete OrderExtraItemDetail Where Id=" + VarDetailId + " And OrderId=" + DDOrderNo.SelectedValue);
            tran.Commit();
            FillShowGrid();
        }
        catch (Exception ex)
        {
            tran.Rollback();
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void Gvchklist_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGOrderDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderDetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void ChkForAddDescriptionWithValue_CheckedChanged(object sender, EventArgs e)
    {
        DescriptionWithValueChange();
    }
    private void DescriptionWithValueChange()
    {
        TxtGroupName.Text = "";
        TxtBuyerCode.Text = "";
        TxtOurCode.Text = "";
        TxtDescription.Text = "";
        TxtUnit.Text = "";
        TxtQty.Text = "";
        TxtRate.Text = "";
        TxtArea.Text = "";
        TxtAmount.Text = "";
        TxtDescriptionText.Text = "";
        TxtDescriptionText.Text = "";
        if (ChkForAddDescriptionWithValue.Checked == true)
        {
            Table1.Visible = false;
            Table2.Visible = false;
            Table3.Visible = true;
        }
        else
        {
            Table1.Visible = true;
            Table2.Visible = true;
            Table3.Visible = false;
        }
    }
    protected void DDCalType_SelectedIndexChanged(object sender, EventArgs e)
    {
        CalTypeSelectChange();
    }
    private void CalTypeSelectChange()
    {
        TDArea.Visible = false;
        TxtQty.Text = "";
        TxtArea.Text = "";
        if (DDCalType.SelectedValue == "0")
        {
            TDArea.Visible = true;
        }
    }
    private void Report_Type()
    {
        Session["ReportPath"] = "Reports/RptPerFormaInvoiceAddExtraItem.rpt";
        Session["CommanFormula"] = "{VIEW_PERFORMAINVOICE.Orderid}=" + DDOrderNo.SelectedValue;

    }
}