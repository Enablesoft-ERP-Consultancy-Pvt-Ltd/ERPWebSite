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
using System.Text;

public partial class Masters_Order_Draft_order : System.Web.UI.Page
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
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select * From PROCESS_NAME_MASTER PNM,Process_UserType PUT,UserType UT,
            NewUserDetail NUD Where PNM.PROCESS_NAME_ID=PUT.PRocessID And PUT.ID=UT.ID And ApprovalFlag=1 and UT.ID=NUD.UserType And VarUserId=" + Session["varuserid"] + " And PNM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                BtnForApprovalOrder.Visible = true;
                ChkForApprovalOrder.Visible = true;
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
            con.Open();
            if (ddorderno.SelectedIndex > 0)
            {
                string strsql = @"select OD.OrderDetailId as Sr_No,od.ourcode,od.buyercode,VF.CATEGORY_NAME CATEGORY,VF.ITEM_NAME ITEMNAME,VF.QUALITYNAME+SPACE(2)+VF.DESIGNNAME+SPACE(2)+VF.COLORNAME+SPACE(2)+SHAPENAME+SPACE(2)+
                CASE WHEN od.orderUnitId=1 THEN VF.SIZEMTR ELSE VF.SIZEFT END DESCRIPTION,od.Qtyrequired as Qty ,od.totalArea as Area,OD.Remarks as PPInstruction ,od.photo as photo,vf.item_finished_id
                from ordermaster om,orderdetail od,V_FINISHEDITEMDETAIL VF
                where om.orderid=od.orderid and OD.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID and OM.OrderId=" + ddorderno.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By OD.OrderDetailId";
                ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    trdrig.Style.Add("display", "");
                }
                else
                {
                    trdrig.Style.Add("display", "none");
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/Draft_order.aspx");
            LblErrorMessage.Text = ex.Message;
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
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerCodeSelectedIndexChanged();
    }
    private void CustomerCodeSelectedIndexChanged()
    {
        string Str = @"SELECT distinct OM.OrderId,OM.LocalOrder+ ' / ' +OM.CustomerOrderNo FROM ORDERMASTER OM , orderdetail OD WHERE OD.ORDERID=OM.ORDERid AND od.tag_flag=1 
                     And OM.customerid = " + DDCustomerCode.SelectedValue + @" And OM.companyid=" + DDCompanyName.SelectedValue;
        if (ChkForApprovalOrder.Checked == true)
        {
            ChkEditOrder.Checked = true;
            Str = Str + " And OM.OrderId in (Select Orderid From Order_Approval)";
        }
        else
        {
            Str = Str + " And OM.OrderId Not in (Select Orderid From Order_Approval)";
        }
        if (ChkEditOrder.Checked == true)
        {
            Str = Str + " And pro_flag =1";
        }
        else
        {
            Str = Str + " And pro_flag <>1";
        }
        UtilityModule.ConditionalComboFill(ref ddorderno, Str, true, "--Select");
        trdrig.Style.Add("display", "none");
        trgridcus.Style.Add("display", "none");
    }
    protected void ddorderno_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            if (ddorderno.SelectedIndex > 0)
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
                Session["ReportPath"] = "Reports/Local_order.rpt";
                Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + ddorderno.SelectedValue + "";
                trgridcus.Style.Add("Display", "none");
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/Draft_order.aspx");
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DGOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
        e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderDetail, "Select$" + e.Row.RowIndex);
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string CellValue = e.Row.Cells[0].Text;
            DataSet ds1 = null;
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            string strsql = @"select * from ORDER_CONSUMPTION_DETAIL where orderdetailid=" + e.Row.Cells[0].Text + "";
            ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            int n = ds1.Tables[0].Rows.Count;
            if (n == 0 && ds1.Tables[0].Rows.Count == 0)
            {
                e.Row.BackColor = System.Drawing.Color.Green;
                e.Row.Visible = true;
            }
            con.Close();
        }
    }
    protected void BTNadd_Click(object sender, EventArgs e)
    {
        tr1.Style.Add("display", "");
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        string str;
        if (ChkEditOrder.Checked == true)
            str = @"UPDATE orderDETAIL Set Remarks='" + Txtremark.Text + "',pro_flag=1,UPDATE_FLAG=1 where orderdetailid=" + Session["id"] + "";
        else
            str = @"UPDATE orderDETAIL Set Remarks='" + Txtremark.Text + "',pro_flag=1,UPDATE_FLAG=0 where orderdetailid=" + Session["id"] + "";
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
        SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'orderDETAIL'," + Session["id"] + ",getdate(),'Update')");
        tr1.Style.Add("Display", "none");
        Txtremark.Text = "";
        refreshform();
    }
    protected void DGOrderDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        GridViewRow row = DGOrderDetail.Rows[index];
        Session["id"] = row.Cells[0].Text;
        //if (ChkEditOrder.Checked == true)
        //{
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strsql = @"select Remarks from orderdetail where orderdetailid=" + Session["id"] + " ";
        ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        Txtremark.Text = ds.Tables[0].Rows[0][0].ToString();
        con.Close();
        //}
        if (Session["val"].ToString() == "2")
        {
            Fill_Grid1();
            Session["val"] = "0";
        }
        if (Session["val"].ToString() == "3")
        {
            int index1 = Convert.ToInt32(e.CommandArgument);
            GridViewRow row1 = (DGOrderDetail.Rows[index1]);
            //int row2=Convert.ToInt32(row1);
            //string finishedid=row1.Cells[13].Text;
            string finishedid = ((Label)DGOrderDetail.Rows[index1].FindControl("item_finished_id")).Text;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../Carpet/DefineBomAndConsumption.aspx?finishedid=" + finishedid + "&ZZZ=1', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            //string finished = ((Label)DGOrderDetail.[e.CommandArgument].FindControl("item_finished_id")).Text;
        }
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        BtnForApprovalOrder.Visible = false;
        if (ChkEditOrder.Checked == true)
        {
            //ddorderno.SelectedIndex = 0;
            DDCustomerCode.SelectedIndex = 0;
            UtilityModule.ConditionalComboFill(ref ddorderno, "", true, "--Select");
            Fill_Grid();
            trgridcus.Style.Add("display", "none");
            trdrig.Style.Add("display", "none");
            tr1.Style.Add("display", "none");
        }
        else
        {
            if (ddorderno.Items.Count > 0)
            {
                ddorderno.SelectedIndex = 0;
            }
            UtilityModule.ConditionalComboFill(ref ddorderno, "", true, "--Select");
            DDCustomerCode.SelectedIndex = 0;
            Fill_Grid();
            tr1.Style.Add("Display", "none");
            trgridcus.Style.Add("display", "none");
            trdrig.Style.Add("display", "none");
        }
    }
    protected void BTNcon_Click(object sender, EventArgs e)
    {
        Session["val"] = "2";
    }
    protected void BTNaddcon_Click(object sender, EventArgs e)
    {
        Session["val"] = "3";
    }
    private DataSet fill_detail1()
    {
        DataSet ds2 = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string strsql = @"select VF.CATEGORY_NAME +','+VF.ITEM_NAME+','+VF.QUALITYNAME+','+VF.DESIGNNAME+SPACE(2)+VF.COLORNAME+SPACE(2)+vf.SHAPENAME+SPACE(2)
                            Item,VF1.CATEGORY_NAME +','+VF1.ITEM_NAME+','+VF1.QUALITYNAME+','+VF1.DESIGNNAME+SPACE(2)+VF1.COLORNAME+SPACE(2)+vf1.SHAPENAME+SPACE(2)
                            input_Item,VF2.CATEGORY_NAME +','+VF2.ITEM_NAME+','+VF2.QUALITYNAME+','+VF2.DESIGNNAME+SPACE(2)+VF2.COLORNAME+SPACE(2)+vf2.SHAPENAME+SPACE(2)
                            output_item,pm.process_name,oc.iqty input_qty,oc.iloss input_loss,oc.irate as input_rate,oc.oqty as output_qnt,oc.orate as output_rate 
                            from V_FinishedItemDetail vf ,ORDER_CONSUMPTION_DETAIL oc,V_FinishedItemDetail vf1,V_FinishedItemDetail vf2,process_name_master pm
                            where oc.finishedid=vf.item_finished_id and oc.ifinishedid=vf1.item_finished_id and oc.ofinishedid=vf2.item_finished_id and oc.processid=pm.process_name_id and orderdetailid=" + Session["id"] + " ANd VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds2 = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                trgridcus.Style.Add("display", "");
            }
            else
                trgridcus.Style.Add("display", "none");
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/Draft_order.aspx");
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        return ds2;
    }
    private void Fill_Grid1()
    {
        gv_cus.DataSource = fill_detail1();
        gv_cus.DataBind();
    }
    protected void refreshitem_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        DataSet ds4 = new DataSet();
        con.Open();
        SqlHelper.ExecuteNonQuery(con, CommandType.Text, "Delete Order_Consumption_Detail Where ORDERDETAILID=" + Session["id"]);
        string str = @"select orderdetailid,orderid,item_finished_id from orderdetail where orderdetailid='" + Session["id"] + "' ";
        ds4 = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
        if (ds4.Tables[0].Rows.Count > 0)
        {
            UtilityModule.ORDER_CONSUMPTION_DEFINE(Convert.ToInt32(ds4.Tables[0].Rows[0]["item_finished_id"]), Convert.ToInt32(ds4.Tables[0].Rows[0]["orderid"]), Convert.ToInt32(ds4.Tables[0].Rows[0]["orderdetailid"]), 0, 0);
        }
        Fill_Grid();
        Fill_Grid1();
        trdrig.Style.Add("display", "");
        trgridcus.Style.Add("display", "");
    }
    private void refreshform()
    {
        Session["OrderId"] = "0";
        Fill_Grid();
    }
    //********************Function to check For Existing Finished Item Id*********************************************************
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
    protected void BtnOrderDetailWithConsumption_Click(object sender, EventArgs e)
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlParameter para = new SqlParameter("@OrderId", SqlDbType.Int);
        para.Value = ddorderno.SelectedValue;
        ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "PRO_OrderConsumptionDetailNew", para);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptOrderConsumptionDetailNew.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptOrderConsumptionDetailNew.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
    }
    protected void BtnForApprovalOrder_Click(object sender, EventArgs e)
    {
        //(OrderId,ProcessId,UserId,MasterCompanyId,Date,Remarks)
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[6];
            _arrPara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@UserId", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@Date", SqlDbType.SmallDateTime);
            _arrPara[5] = new SqlParameter("@Remarks", SqlDbType.NVarChar, 50);
            _arrPara[0].Value = ddorderno.SelectedValue;
            _arrPara[1].Value = 10;
            _arrPara[2].Value = Session["varuserid"].ToString();
            _arrPara[3].Value = Session["varMasterCompanyIDForERP"].ToString();
            _arrPara[4].Value = DateTime.Now.ToString("dd-MMM-yyyy");
            _arrPara[5].Value = "";
            con.Open();
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_Order_Approval", _arrPara);
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Successfully Approvaled Order........";
            CustomerCodeSelectedIndexChanged();
            Session["OrderId"] = "0";
            Fill_Grid();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/Draft_order.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
            Logs.WriteErrorLog("Masters_Carpet_FrmColor|cmdSave_Click|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (con != null)
            {
                con.Dispose();
            }
        }
    }
    protected void ChkForApprovalOrder_CheckedChanged(object sender, EventArgs e)
    {
        CustomerCodeSelectedIndexChanged();
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
    protected void gv_cus_RowCreated(object sender, GridViewRowEventArgs e)
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
