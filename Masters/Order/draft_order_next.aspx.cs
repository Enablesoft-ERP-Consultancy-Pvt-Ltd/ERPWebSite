using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO.Compression;
using CrystalDecisions.CrystalReports;
using System.Configuration;

public partial class Masters_Order_draft_order_next : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            Session["OrderId"] = 0;
            Session["OrderDetailId"] = 0;
            Session["val"] = 0;
            hncode.Value = "";
            logo();
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName", true, "--SELECT--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "SELECT customerid,CompanyName + SPACE(5)+Customercode from customerinfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName", true, "--SELECT--");
            TxtOrderDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtDeliveryDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    private void CHECKVALIDCONTROL()
    {
        LblErrorMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompanyName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerCode) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        LblErrorMessage.Visible = true;
        UtilityModule.SHOWMSG(LblErrorMessage);
    B: ;
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    private void logo()
    {
        if (File.Exists(Server.MapPath("~/Images/Logo/" + Session["varMasterCompanyIDForERP"] + "_company.gif")))
        {
            imgLogo.ImageUrl.DefaultIfEmpty();
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varMasterCompanyIDForERP"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
        }
        LblCompanyName.Text = Session["varCompanyName"].ToString();
        LblUserName.Text = Session["varusername"].ToString();
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddorderno, "SELECT distinct om.orderid,(OM.CUSTOMERORDERNO) FROM ORDERMASTER OM , orderdetail OD WHERE OD.ORDERID=OM.ORDERid  and pro_flag =1  and om.customerid = " + DDCustomerCode.SelectedValue + " and om.companyid=" + DDCompanyName.SelectedValue + "", true, "--Select");
    }
    protected void ddorderno_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string strsql = @"SELECT OM.Orderid,orderUnitId,replace(convert(varchar(11),OrderDate,106), ' ','-') as OrderDate,replace(convert(varchar(11),OM.DispatchDate,106), ' ','-') as DeliveryDate,customerorderno,localorder From ORDERMASTER OM,ORDERDETAIL OD where OM.OrderId=OD.OrderId And OM.OrderId=" + ddorderno.SelectedValue;
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Textlocalorder.Text = ds.Tables[0].Rows[0]["localorder"].ToString();
                TxtOrderDate.Text = ds.Tables[0].Rows[0]["OrderDate"].ToString();
                TxtDeliveryDate.Text = ds.Tables[0].Rows[0]["DeliveryDate"].ToString();
            }
            Fill_Grid();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/Draft_Order_Next.aspx");
            LblErrorMessage.Visible = true;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
    private void Fill_Grid()
    {
        DGOrderDetail.DataSource = GetDetail();
        DGOrderDetail.DataBind();
    }
    private DataSet GetDetail()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string id;
            if (ddorderno.SelectedItem == null)
                id = "0";
            else
                id = ddorderno.SelectedValue;
            con.Open();
            string strsql = @"select OD.OrderDetailId as Sr_No,od.ourcode,od.buyercode,VF.CATEGORY_NAME CATEGORY,VF.ITEM_NAME ITEMNAME,VF.QUALITYNAME+SPACE(2)+VF.DESIGNNAME+SPACE(2)+VF.COLORNAME+SPACE(2)+SHAPENAME+SPACE(2)+
            CASE WHEN oD.orderUnitId=1 THEN VF.SIZEMTR ELSE VF.SIZEFT END DESCRIPTION,od.remarks as Remark,od.Qtyrequired as Qty ,od.totalArea as Area,finishinginstructions as PPInstruction 
            from ordermaster om,orderdetail od,V_FINISHEDITEMDETAIL VF
            where om.orderid=od.orderid and OD.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID and OM.Orderid='" + id + "' And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            hntot.Value = Convert.ToString(ds.Tables[0].Rows.Count);
            if (ds.Tables[0].Rows.Count > 0)
            {
                trdrig.Style.Add("display", "");
                lblgreen.Visible = true;
            }
            else
            {
                trdrig.Style.Add("display", "none");
                lblgreen.Visible = false;
            }
            int n = ds.Tables[0].Rows.Count;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/Draft_Order_Next.aspx");
            LblErrorMessage.Visible = true;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        return ds;
    }
    private void refreshform()
    {
        //DDCustOrderNo.SelectedIndex = 0;
        Session["OrderId"] = "0";
        Fill_Grid();
    }
    protected void DGOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
        e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderDetail, "Select$" + e.Row.RowIndex);
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            try
            {
                string CellValue = e.Row.Cells[1].Text;
                DataSet ds1 = null;
                string strsql = @"select update_flag from ORDERDETAIL where orderdetailid=" + e.Row.Cells[1].Text + "";
                ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
                int n = ds1.Tables[0].Rows.Count;
                if (ds1.Tables[0].Rows[0][0].ToString() == "1")
                {
                    e.Row.BackColor = System.Drawing.Color.Green;
                    e.Row.Visible = true;
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Order/Draft_Order_Next.aspx");
                LblErrorMessage.Visible = true;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
    }
    protected void BTNadd_Click(object sender, EventArgs e)
    {
        hncode.Value = "1";
        //btnreport.Visible = true;
    }
    protected void BTNaddNew_Click(object sender, EventArgs e)
    {
        tr1.Style.Add("display", "");

    }
    protected void DGOrderDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int n = 0;
        if (hntot.Value != "")
            n = Convert.ToInt32(hntot.Value);
        int index = Convert.ToInt32(e.CommandArgument);
        GridViewRow row = DGOrderDetail.Rows[index];
        bool isChecked = ((CheckBox)row.FindControl("Chkbox")).Checked;

        if (isChecked == true)
        {
            for (int i = 0; i < n; i++)
            {
                GridViewRow row1 = DGOrderDetail.Rows[i];
                if (row == row1)
                {
                    ((CheckBox)row1.FindControl("Chkbox")).Checked = true;
                }
                else
                {
                    ((CheckBox)row1.FindControl("Chkbox")).Checked = false;
                }
            }
            Session["id"] = row.Cells[1].Text;
            if (hncode.Value == "1")
            {
                Session["ReportPath"] = "Reports/Local_order.rpt";
                Session["CommanFormula"] = "{NewOrderReport.orderdetailid}=" + Session["id"] + "";
            }
        }
        DataSet ds1 = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strsql = @"select Remarks from orderdetail where orderdetailid=" + Session["id"] + " ";
        ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        Txtremark.Text = ds1.Tables[0].Rows[0][0].ToString();
        con.Close();
        Txtremark.Focus();
    }
    protected void DGOrderDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        trdrig.Style.Add("display", "");
        hncode.Value = "1";



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
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        string str;
        str = @"UPDATE orderDETAIL Set Remarks='" + Txtremark.Text + "',pro_flag=1,UPDATE_FLAG=0 where orderdetailid=" + Session["id"] + "";
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
        SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'orderDETAIL'," + Session["id"] + ",getdate(),'Update')");
        tr1.Style.Add("Display", "none");
        Txtremark.Text = "";
        refreshform();
    }
}