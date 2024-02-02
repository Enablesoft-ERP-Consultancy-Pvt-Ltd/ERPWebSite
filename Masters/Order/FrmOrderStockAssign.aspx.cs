using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Order_FrmOrderStockAssign : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        LblMessage.Text = "";
        if (!IsPostBack)
        {
            ChkEdit.Checked = false;
            CommanFunction.FillCombo(DDCompany, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                CompanySelectedIndexChange();
            }
        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedIndexChange();
    }
    private void CompanySelectedIndexChange()
    {
        UtilityModule.ConditionalComboFill(ref DDcustomer, " Select customerid,customercode from customerinfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by customercode", true, "--Select--");
    }

    protected void DDcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        EditChanged();
    }
    protected void DDOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
        OrderSelectedChange();
    }
    private void OrderSelectedChange()
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_OrderConsmpStockIssAssignQty", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@OrderId", DDOrder.SelectedValue);
        cmd.Parameters.AddWithValue("@VarUserid", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@VarMasterCompanyId", Session["varMasterCompanyIDForERP"]);
        cmd.Parameters.AddWithValue("@CompanyId", DDCompany.SelectedValue);

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        GVOrderStockAssign.DataSource = ds.Tables[1];
        GVOrderStockAssign.DataBind();
        DGForAssign.DataSource = ds.Tables[0];
        DGForAssign.DataBind();
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        if (DGForAssign.Rows.Count > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {                
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete from OrderStockAssign Where Orderid=" + DDOrder.SelectedValue);
                for (int i = 0; i < DGForAssign.Rows.Count; i++)
                {
                    TextBox txt = (TextBox)DGForAssign.Rows[i].FindControl("TxtAssignqty");
                    if (txt.Text != "" && Convert.ToDouble(txt.Text) > 0 && Convert.ToDouble(txt.Text) != 0)
                    {
                        string sAssignQty = txt.Text.Trim();
                        Label LotNo = (Label)DGForAssign.Rows[i].FindControl("lblLotNo");
                        string LotNumber = LotNo.Text;

                        Label TagNo = (Label)DGForAssign.Rows[i].FindControl("lblTagNo");
                        string TagNumber = TagNo.Text;

                        SqlParameter[] parparam = new SqlParameter[8];
                        parparam[0] = new SqlParameter("@OrderID", DDOrder.SelectedValue);
                        parparam[1] = new SqlParameter("@FinishedID", DGForAssign.DataKeys[i].Value);
                        parparam[2] = new SqlParameter("@AssignQTY", sAssignQty);
                        parparam[3] = new SqlParameter("@Userid", Session["varuserId"].ToString().Trim());
                        parparam[4] = new SqlParameter("@MasterCompanyID", Session["varMasterCompanyIDForERP"].ToString().Trim());
                        parparam[5] = new SqlParameter("@LotNo", LotNumber);
                        parparam[6] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
                        parparam[7] = new SqlParameter("@TagNo", TagNumber);

                        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_OrderStock_Assign", parparam);
                        Tran.Commit();
                    }
                }
                OrderSelectedChange();
                LblMessage.Text = "Record(s) has been saved successfully!";
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmOrderStockaassign");
                LblMessage.Text = ex.Message.ToString();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void BtnPrev_Click(object sender, EventArgs e)
    {
        SqlParameter[] _arrpara = new SqlParameter[4];
        _arrpara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
        _arrpara[1] = new SqlParameter("@VarUserid", SqlDbType.Int);
        _arrpara[2] = new SqlParameter("@VarMasterCompanyId", SqlDbType.Int);
        _arrpara[3] = new SqlParameter("@CompanyId", SqlDbType.Int);

        _arrpara[0].Value = DDOrder.SelectedValue;
        _arrpara[1].Value = Session["varuserid"];
        _arrpara[2].Value = Session["varMasterCompanyIDForERP"];
        _arrpara[3].Value = DDCompany.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_OrderConsmpStockIssAssignQtyForReport", _arrpara);

        Session["ReportPath"] = "reports/Rpt_OrderStockAssignReport.rpt";
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\OrderStockAssign.xsd";
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
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Main.aspx");
    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmOrderStockAssign.aspx");
    }
    protected void ChkEdit_CheckedChanged(object sender, EventArgs e)
    {
        EditChanged();
    }
    private void EditChanged()
    {
        string str = @"Select Distinct Orderid, CustomerOrderNo from OrderMaster Where CustomerID=" + DDcustomer.SelectedValue + " And CompanyId=" + DDCompany.SelectedValue;
        if (ChkEdit.Checked == true)
        {
            str = str + " And Orderid IN (Select OrderID From OrderStockAssign)";
        }
        else
        {
            str = str + " And Orderid not IN (Select OrderID From OrderStockAssign)";
        }
        str = str + " Order by CustomerOrderNo";
        UtilityModule.ConditionalComboFill(ref DDOrder, str, true, "--Select--");
    }
    protected void TextAssign_Changed(object sender, EventArgs e)
    {
        int RowIndex = ((GridViewRow)((TextBox)sender).NamingContainer).RowIndex;
        Double StockQty = Convert.ToDouble(DGForAssign.Rows[RowIndex].Cells[7].Text);
        Double AssignQty = Convert.ToDouble(((TextBox)DGForAssign.Rows[RowIndex].FindControl("TxtAssignqty")).Text);
        if (AssignQty > StockQty)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('Assign Qty Can not be greater than Stock Qty..');", true);
            ((TextBox)DGForAssign.Rows[RowIndex].FindControl("TxtAssignqty")).Text = "0";
        }
    }
    //protected void GVOrderStockAssign_RowCreated(object sender, GridViewRowEventArgs e)
    //{

    //    //if (e.Row.RowType == DataControlRowType.Header)
    //    //    e.Row.CssClass = "header";

    //    ////Add CSS class on normal row.
    //    //if (e.Row.RowType == DataControlRowType.DataRow &&
    //    //          e.Row.RowState == DataControlRowState.Normal)
    //    //    e.Row.CssClass = "normal";

    //    ////Add CSS class on alternate row.
    //    //if (e.Row.RowType == DataControlRowType.DataRow &&
    //    //          e.Row.RowState == DataControlRowState.Alternate)
    //    //    e.Row.CssClass = "alternate";
    //}

    //protected void DGForAssign_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //if (e.Row.RowType == DataControlRowType.Header)
    //    //    e.Row.CssClass = "header";

    //    ////Add CSS class on normal row.
    //    //if (e.Row.RowType == DataControlRowType.DataRow &&
    //    //          e.Row.RowState == DataControlRowState.Normal)
    //    //    e.Row.CssClass = "normal";

    //    ////Add CSS class on alternate row.
    //    //if (e.Row.RowType == DataControlRowType.DataRow &&
    //    //          e.Row.RowState == DataControlRowState.Alternate)
    //    //    e.Row.CssClass = "alternate";

    //}
    protected void BtnPreviewLotWise_Click(object sender, EventArgs e)
    {
        if (TxtLotNo.Text == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Please enter lot no');", true);
            return;
        }

        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@LotNo", TxtLotNo.Text);
        param[1] = new SqlParameter("@UserID", Session["varMasterCompanyIDForERP"]);
        param[2] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetOrderStockAssignQtyLotNoWise", param);

        Session["ReportPath"] = "reports/RptOrderStockAssignQtyLotNoWise.rpt";
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptOrderStockAssignQtyLotNoWise.xsd";
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
    protected void GVOrderStockAssign_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVOrderStockAssign, "Select$" + e.Row.RowIndex);           
        }
    }
    protected void GVOrderStockAssign_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["varCompanyNo"].ToString() == "9")
        {
            int rowindex = GVOrderStockAssign.SelectedRow.RowIndex;
            Label lblIFinishedId = ((Label)GVOrderStockAssign.Rows[rowindex].FindControl("lblIFinishedid"));

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_OrderConsmpStockIssAssignQtyByIFinishedId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@OrderId", DDOrder.SelectedValue);
            cmd.Parameters.AddWithValue("@VarUserid", Session["varuserid"]);
            cmd.Parameters.AddWithValue("@VarMasterCompanyId", Session["varMasterCompanyIDForERP"]);
            cmd.Parameters.AddWithValue("@CompanyId", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@IFinishedId", lblIFinishedId.Text);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                DGForAssign.DataSource = ds.Tables[0];
                DGForAssign.DataBind();
            }
            else
            {
                DGForAssign.DataSource = null;
                DGForAssign.DataBind();
            }      
            
        }        
    }
}