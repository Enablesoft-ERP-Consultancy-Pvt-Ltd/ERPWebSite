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

public partial class Masters_Order_Draft_Order_New : System.Web.UI.Page
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

            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "SELECT customerid,Customercode from customerinfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Customercode", true, "--SELECT--");
            TxtOrderDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtDeliveryDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            if (Session["Varcompanyno"].ToString() == "6")
            {
                //ChkEditOrder.Visible = false;
                ChkEditOrder.Enabled = true;
                tdlabinsp.Visible = true;
                BtnSave1.Visible = true;
                TxtLabel.Text = "Green Line Show Item Was Planned";
            }
            else
            {
                ChkEditOrder.Visible = true;
                ChkEditOrder.Checked = true;
            }
        }
        //show edit button
        if (Session["canedit"].ToString() == "0") //non authenticated person
        {
            ChkEditOrder.Enabled = false;
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
                CASE WHEN od.flagsize=1 THEN VF.SIZEMTR  when od.flagsize=2 then vf.sizeinch  ELSE VF.SIZEFT END+' '+case When Vf.sizeId>0 Then ST.Type Else '' End DESCRIPTION,od.Qtyrequired as Qty ,
                od.totalArea as Area,OD.Remarks as PPInstruction ,od.photo as photo
                from ordermaster om,orderdetail od,V_FINISHEDITEMDETAIL VF,SizeType St
                Where om.orderid=od.orderid and OD.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID and OM.OrderId=" + ddorderno.SelectedValue + " And Vf.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and OD.flagsize=St.val  Order By OD.OrderDetailId";

                ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    trdrig.Style.Add("display", "");
                }
                else
                {
                    trdrig.Style.Add("display", "none");
                }
                if (Session["Varcompanyno"].ToString() != "6")
                {
                    ChkEditOrder.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/Draft_Order_New.aspx");
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
                     And OM.OrderId Not in (Select Orderid From Order_Approval) And OM.customerid = " + DDCustomerCode.SelectedValue + @" And OM.companyid=" + DDCompanyName.SelectedValue;
        if (ChkEditOrder.Checked == true && Session["varcompanyno"].ToString() == "3")
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
                trgridcus.Style.Add("Display", "none");
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/Draft_Order_New.aspx");
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
            if (Session["varcompanyno"].ToString() != "6")
            {
                string CellValue = e.Row.Cells[1].Text;
                DataSet ds1 = null;
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                con.Open();
                string strsql = @"select * from ORDER_CONSUMPTION_DETAIL where orderdetailid=" + e.Row.Cells[1].Text + "";
                ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
                int n = ds1.Tables[0].Rows.Count;
                if (n == 0 && ds1.Tables[0].Rows.Count == 0)
                {
                    e.Row.BackColor = System.Drawing.Color.Green;
                    e.Row.Visible = true;
                }
                con.Close();
                DGOrderDetail.Columns[0].Visible = false;
            }
            else
            {
                DGOrderDetail.Columns[11].Visible = false;
                DGOrderDetail.Columns[12].Visible = false;
                DGOrderDetail.Columns[13].Visible = false;
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from OrderProductionPalanning where orderdetailid=" + e.Row.Cells[1].Text + "");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    e.Row.BackColor = System.Drawing.Color.Green;
                }
            }
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
        SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'ORDERDETAIL'," + Session["id"] + ",getdate(),'Update')");
        tr1.Style.Add("Display", "none");
        Txtremark.Text = "";
        refreshform();
    }
    protected void DGOrderDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        GridViewRow row = DGOrderDetail.Rows[index];
        Session["id"] = row.Cells[1].Text;
        if (ChkEditOrder.Checked == true)
        {
            DataSet ds = null;
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            string strsql = @"select Remarks from orderdetail where orderdetailid=" + Session["id"] + " ";
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            Txtremark.Text = ds.Tables[0].Rows[0][0].ToString();
            con.Close();
        }
        if (Session["val"].ToString() == "2")
        {
            Fill_Grid1();
            Session["val"] = "0";
        }
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            //ddorderno.SelectedIndex = 0;
            DDCustomerCode.SelectedIndex = 0;
            save_refresh();
            UtilityModule.ConditionalComboFill(ref ddorderno, "", true, "--Select");
            if (Session["varcompanyno"].ToString() == "3")
            {
                Fill_Grid();
                trgridcus.Style.Add("display", "none");
                trdrig.Style.Add("display", "none");
                tr1.Style.Add("display", "none");
            }
            else
            {

            }
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
            save_refresh();
        }
    }
    protected void BTNcon_Click(object sender, EventArgs e)
    {
        Session["val"] = "2";
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
                            where oc.finishedid=vf.item_finished_id and oc.ifinishedid=vf1.item_finished_id and oc.ofinishedid=vf2.item_finished_id and oc.processid=pm.process_name_id and orderdetailid=" + Session["id"] + " And Vf.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
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
            UtilityModule.MessageAlert(ex.Message, "Master/Order/Draft_Order_New.aspx");
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
        {   imgLogo.ImageUrl.DefaultIfEmpty();
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
            UtilityModule.MessageAlert(ex.Message, "Master/Order/Draft_Order_New.aspx");
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
    //protected void DGOrderDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    //protected void gv_cus_RowCreated(object sender, GridViewRowEventArgs e)
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
    private void fillLabel()
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT ITEM_MASTER.ITEM_ID as Sr_No, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME,dbo.ITEM_MASTER.ITEM_NAME as label FROM dbo.ITEM_MASTER Inner join 
        dbo.ITEM_CATEGORY_MASTER ON dbo.ITEM_MASTER.CATEGORY_ID = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID Where dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME='ACCESSORIES ITEM' And Item_Master.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            grdlabel.DataSource = ds;
            grdlabel.DataBind();
        }
    }
    private void fillinspection()
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select id Sr_No,DateName inspection from InspectionDateMaster");
        if (ds.Tables[0].Rows.Count > 0)
        {
            grdinspecctiondate.DataSource = ds;
            grdinspecctiondate.DataBind();
        }
    }
    protected void chklabel_CheckedChanged(object sender, EventArgs e)
    {
        chk_label_change();
    }
    private void chk_label_change()
    {
        if (chklabel.Checked == true)
        {
            tdlabel.Visible = true;
        }
        else
        {
            tdlabel.Visible = false;
        }
        fillLabel();
    }
    protected void chkinspectiondate_CheckedChanged(object sender, EventArgs e)
    {
        chkInspectiondate_change();
    }
    private void chkInspectiondate_change()
    {
        if (chkinspectiondate.Checked == true)
        {
            tdinspec.Visible = true;
        }
        else
        {
            tdinspec.Visible = false;
        }
        fillinspection();
    }
    protected void refreshdatename_Click(object sender, EventArgs e)
    {
        fillinspection();
    }
    protected void refreshitem11_Click(object sender, EventArgs e)
    {
        fillLabel();
    }
    protected void BtnSave1_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            ViewState["ID"] = 0;
            ViewState["PlanID"] = 0;
            string str = "";
            SqlParameter[] _arrPara = new SqlParameter[16];
            _arrPara[0] = new SqlParameter("@CompanyID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@CustomerCode", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@Orderid", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@Orderdetailid", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@LableId", SqlDbType.NVarChar, 550);
            _arrPara[5] = new SqlParameter("@Inspectiondate", SqlDbType.SmallDateTime);
            _arrPara[6] = new SqlParameter("@Innerpacking", SqlDbType.Float);
            _arrPara[7] = new SqlParameter("@Middlepacking", SqlDbType.NVarChar, 150);
            _arrPara[8] = new SqlParameter("@Masterpacking", SqlDbType.Float);
            _arrPara[9] = new SqlParameter("@TestingStatus", SqlDbType.NVarChar, 20);
            _arrPara[10] = new SqlParameter("@TestingRemark", SqlDbType.NVarChar, 250);
            _arrPara[11] = new SqlParameter("@PlanID", SqlDbType.Int);
            _arrPara[12] = new SqlParameter("@Carton_Bales", SqlDbType.NVarChar, 50);
            _arrPara[13] = new SqlParameter("@ItemRemark", SqlDbType.NVarChar, 250);
            _arrPara[14] = new SqlParameter("@ReqDate", SqlDbType.SmallDateTime);
            _arrPara[15] = new SqlParameter("@planingdate", SqlDbType.SmallDateTime);
            _arrPara[0].Value = Session["VarcompanyNo"].ToString();
            _arrPara[1].Value = DDCustomerCode.SelectedValue;
            _arrPara[2].Value = ddorderno.SelectedValue;
            if (chklabel.Checked == true)
            {
                str = "";
                for (int i = 0; i < grdlabel.Rows.Count; i++)
                {
                    if (((CheckBox)grdlabel.Rows[i].FindControl("Chkbox")).Checked == true)
                    {
                        if (str == "")
                        {
                            str = grdlabel.DataKeys[i].Value.ToString();
                        }
                        else
                        {
                            str = str + ',' + grdlabel.DataKeys[i].Value.ToString();
                        }
                    }
                }
            }
            _arrPara[4].Value = str;
            _arrPara[5].Value = DateTime.Now.ToString("dd-MMM-yyyy");
            _arrPara[6].Value = Txtinnerpacking.Text != "" ? Txtinnerpacking.Text : "0";
            _arrPara[7].Value = TxtMiddlepacking.Text != "" ? TxtMiddlepacking.Text : "0";
            _arrPara[8].Value = TxtMasterpacking.Text != "" ? TxtMasterpacking.Text : "0";
            _arrPara[9].Value = DDStatus.SelectedItem.Text;
            _arrPara[10].Value = TxtTestRemark.Text;
            _arrPara[11].Direction = ParameterDirection.InputOutput;
            _arrPara[11].Value = ViewState["PlanID"];
            _arrPara[12].Value = ddcarton.SelectedItem.Text;
            _arrPara[13].Value = txtitemremark.Text;
            _arrPara[14].Value = TxtDeliveryDate.Text;
            _arrPara[15].Value = DateTime.Now.Date;
            for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
            {
                if (((CheckBox)DGOrderDetail.Rows[i].FindControl("Chkbox")).Checked == true)
                {
                    _arrPara[3].Value = DGOrderDetail.DataKeys[i].Value;
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_OrderProductionPlanning", _arrPara);
                    if (chkinspectiondate.Checked == true)
                    {
                        SqlParameter[] _arrPara1 = new SqlParameter[5];
                        _arrPara1[0] = new SqlParameter("@dateid", SqlDbType.Int);
                        _arrPara1[1] = new SqlParameter("@date", SqlDbType.SmallDateTime);
                        _arrPara1[2] = new SqlParameter("@Orderid", SqlDbType.Int);
                        _arrPara1[3] = new SqlParameter("@Orderdetailid", SqlDbType.Int);
                        _arrPara1[4] = new SqlParameter("@Id", SqlDbType.Int);
                        for (int j = 0; j < grdinspecctiondate.Rows.Count; j++)
                        {
                            if (((CheckBox)grdinspecctiondate.Rows[j].FindControl("Chkbox")).Checked == true)
                            {
                                _arrPara1[0].Value = grdinspecctiondate.DataKeys[j].Value;
                                _arrPara1[1].Value = ((TextBox)grdinspecctiondate.Rows[j].FindControl("Txtdate")).Text;
                                _arrPara1[2].Value = ddorderno.SelectedValue;
                                _arrPara1[3].Value = _arrPara[3].Value;
                                _arrPara1[4].Direction = ParameterDirection.InputOutput;
                                _arrPara1[4].Value = ViewState["ID"];
                                if (((TextBox)grdinspecctiondate.Rows[j].FindControl("Txtdate")).Text != "")
                                {
                                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_InspectionDate", _arrPara1);
                                }
                                ViewState["ID"] = _arrPara1[4].Value;
                            }
                        }
                    }
                }
            }
            //ViewState["PlanID"] = _arrPara[11].Value;
            Tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Data Saved Successfully');", true);
            save_refresh();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/AddItemName.aspx");
            Logs.WriteErrorLog("Masters_Campany_ItemName|cmdSave_Click|" + ex.Message);
        }
    }
    private void save_refresh()
    {
        Fill_Grid();
        chklabel.Checked = false;
        chk_label_change();
        chkinspectiondate.Checked = false;
        chkInspectiondate_change();
        Txtinnerpacking.Text = "";
        TxtMiddlepacking.Text = "";
        TxtMasterpacking.Text = "";
        Txtremark.Text = "";
        txtitemremark.Text = "";
        TxtTestRemark.Text = "";
        ViewState["ID"] = 0;
    }
    protected void btnreport_Click(object sender, EventArgs e)
    {
        if (Session["varcompanyno"].ToString() == "6")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            Session["ReportPath"] = "Reports/RptProductionPlanning.rpt";
            string qry = @"select od.orderdetailid, Photo ,CATEGORY_NAME+' '+v.ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as Description,
            InnerPacking,MiddlePacking,MasterPacking,TestingStatus,TestingRemark,Carton_Bales,BUYERCODE,QtyRequired as qty,
            (SELECT * FROM [dbo].[F_Get_Label_For_Order](OD.ORDERDETAILID)) as itemname,(SELECT * FROM [dbo].[F_Get_INSPECTIONdATE_For_Order](OD.Orderdetailid )) as inspectiondate,
            od.DispatchDate as shipingdate,tm.transmodeName,sn.sku_no,CASE WHEN od.flagsize=1 THEN V.SIZEMTR  when od.flagsize=2 then v.sizeinch  ELSE V.SIZEFT END+' '+case When V.sizeId>0 Then ST.Type Else '' End size,
            om.CustomerOrderNo,om.LocalOrder,ci.CustomerCode,ItemRemark,replace(convert(varchar(11),ReqDate,106),' ','-') as reqdate,OD.Remarks as PPInstruction
            From ordermaster om inner join  orderdetail od On om.orderid=od.orderid inner join V_FinishedItemDetail v On od.Item_Finished_Id=v.Item_Finished_Id Left join 
            Transmode tm On tm.transmodeId=om.ByAirSea left outer join Sku_No sn On sn.finished_id=v.Item_Finished_Id 
            Left outer join OrderProductionPalanning op ON od.orderdetailid=op.orderdetailid  inner join customerinfo ci On ci.CustomerId=om.CustomerId inner join SizeType St on OD.flagsize=St.val
            Where OM.ORDERID=" + ddorderno.SelectedValue + " And V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            ds.Tables[0].Columns.Add("Image", typeof(System.Byte[]));
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                if (Convert.ToString(dr["Photo"]) != "")
                {
                    FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                    if (TheFile.Exists)
                    {
                        string img = dr["Photo"].ToString();
                        img = Server.MapPath(img);
                        Byte[] img_Byte = File.ReadAllBytes(img);
                        dr["Image"] = img_Byte;
                    }
                }
            }
            Session["dsFileName"] = "~\\ReportSchema\\RptProductionPlanning.xsd";
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = Session["ReportPath"];
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
        }
        else
        {
            Session["ReportPath"] = "Reports/Local_order.rpt";
            Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + ddorderno.SelectedValue + "";
        }
    }
    protected void Chkbox_checked(object sender, EventArgs e)
    {
        string order = "";
        int n = 0;
        int K = 0;
        if (ChkEditOrder.Checked == true && Session["varcompanyno"].ToString() == "6")
        {
            for ( int i = 0; i < DGOrderDetail.Rows.Count; i++)
            {
                if (((CheckBox)DGOrderDetail.Rows[i].FindControl("Chkbox")).Checked == true)
                {
                    order = DGOrderDetail.DataKeys[i].Value.ToString();
                    K = i;
                    //i = DGOrderDetail.Rows.Count + 1;
                    n = n + 1;
                }
            }
            if (n > 1)
            {
                ((CheckBox)DGOrderDetail.Rows[K].FindControl("Chkbox")).Checked = false;
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Select Only One CheckBox');", true);
                return;
            }
            if (order.ToString() != "")
            {
                string str = @"select InnerPacking,MiddlePacking,MasterPacking,TestingStatus,TestingRemark,VarUserID,Carton_Bales,ItemRemark,replace(convert(varchar(11),ReqDate,106),' ','-') as ReqDate from OrderProductionPalanning where orderid=" + ddorderno.SelectedValue + " and orderdetailid=" + order + @"
                               select labelid from LabelDetail where orderid=" + ddorderno.SelectedValue + " and orderdetailid=" + order + @"
                               select InspectionID,replace(convert(varchar(11),Date,106),' ','-') as Date from InspectionDateDetail where orderid=" + ddorderno.SelectedValue + " and orderdetailid=" + order + @"";
                DataSet ds5 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds5.Tables[0].Rows.Count > 0)
                {
                    Txtinnerpacking.Text = ds5.Tables[0].Rows[0]["InnerPacking"].ToString();
                    TxtMiddlepacking.Text = ds5.Tables[0].Rows[0]["MiddlePacking"].ToString();
                    TxtMasterpacking.Text = ds5.Tables[0].Rows[0]["MasterPacking"].ToString();
                    TxtTestRemark.Text = ds5.Tables[0].Rows[0]["TestingRemark"].ToString();
                    ddcarton.SelectedItem.Text = ds5.Tables[0].Rows[0]["Carton_Bales"].ToString();
                    DDStatus.SelectedItem.Text = ds5.Tables[0].Rows[0]["TestingStatus"].ToString();
                    Txtremark.Text = ds5.Tables[0].Rows[0]["ItemRemark"].ToString();
                    TxtDeliveryDate.Text = ds5.Tables[0].Rows[0]["ReqDate"].ToString();
                    txtitemremark.Text = ds5.Tables[0].Rows[0]["ItemRemark"].ToString();
                }
                if (ds5.Tables[1].Rows.Count > 0)
                {
                    chklabel.Checked = true;
                    chk_label_change();
                    for (int j = 0; j < ds5.Tables[1].Rows.Count; j++)
                    {
                        for (int k = 0; k < grdlabel.Rows.Count; k++)
                        {
                            if (ds5.Tables[1].Rows[j][0].ToString() == grdlabel.DataKeys[k].Value.ToString())
                            {
                                ((CheckBox)grdlabel.Rows[k].FindControl("Chkbox")).Checked = true;
                                //k = ds5.Tables[1].Rows.Count + 1;
                            }
                        }
                    }
                }
                else
                {
                    chklabel.Checked = false;
                    chk_label_change();
                }

                if (ds5.Tables[2].Rows.Count > 0)
                {
                    chkinspectiondate.Checked = true;
                    chkInspectiondate_change();
                    for (int j = 0; j < ds5.Tables[2].Rows.Count; j++)
                    {
                        for (int k = 0; k < grdinspecctiondate.Rows.Count; k++)
                        {
                            if (ds5.Tables[2].Rows[j]["InspectionID"].ToString() == grdinspecctiondate.DataKeys[k].Value.ToString())
                            {
                                ((CheckBox)grdinspecctiondate.Rows[k].FindControl("Chkbox")).Checked = true;
                                ((TextBox)grdinspecctiondate.Rows[k].FindControl("Txtdate")).Text = ds5.Tables[2].Rows[j]["Date"].ToString();
                                //k = ds5.Tables[1].Rows.Count + 1;
                            }
                        }
                    }
                }
                else
                {
                    chkinspectiondate.Checked = false;
                    chkInspectiondate_change();
                }
            }
            else
            {

                save_refresh();
            }
        }
        //else
        //{
        //    save_refresh();
        //}
    }
}