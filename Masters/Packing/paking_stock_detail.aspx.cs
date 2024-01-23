using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Packing_paking_stock_detail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["PorderpackId"] = 0;
            ViewState["PorderpackdetailId"] = 0;
            string Qry = @"select distinct ci.customerid,ci.Customercode + SPACE(5)+CI.CompanyName 
            from customerinfo ci 
            inner join OrderMaster om on om.customerid=ci.customerid And Ci.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" 
            inner join ORDER_CONSUMPTION_DETAIL ocd on ocd.orderid=om.orderid
            inner join Jobassigns JA ON OM.Orderid=JA.Orderid
            select Distinct CI.CompanyId,CI.Companyname From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Companyname";
            DataSet ds = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddcustomercode, ds, 0, true, "Select CustomerCode");
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 1, true, "Select Comp Name");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            ddcustomercode.Focus();
            imgLogo.ImageUrl.DefaultIfEmpty();
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varMasterCompanyIDForERP"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
            LblCompanyName.Text = Session["varCompanyName"].ToString();
            LblUserName.Text = Session["varusername"].ToString();
        }
    }
    protected void ddcustomercode_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddorderno, @"Select Distinct OM.OrderId,LocalOrder + ' / ' + CustomerOrderNo 
            From OrderMaster OM, Jobassigns JA Where OM.Orderid = JA.Orderid And OM.CompanyID = " + ddCompName.SelectedValue + " And OM.CustomerId = " + ddcustomercode.SelectedValue, true, "Select OrderNo.");
    }
    protected void ddorderno_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
        //data();

    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    private void fillgrid()
    {
        string str = @"select pod.pid pid,pod.detailid as pdetail,vf.productcode ,pod.length,width,pod.height,ply,gsm,gsm2, pod.qty,pod.pqty,(isnull(pod.qty,0)-isnull(pod.pqty,0)) receiveqty, case when pod.packingtype=1 then 'Inner' when pod.packingtype=2 then 'Middle' else 'Master' end as packingtype 
         from PurchaseOrderMasterPacking pom left outer join
              PurchaseOrdeDetailPacking pod on pod.pid=pom.pid left outer join
              ordermaster om on om.orderid=pom.orderid  inner join
              V_FinishedItemDetail vf on item_finished_id=finishedid             
              where pom.orderid=" + ddorderno.SelectedValue + "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGSHOWDATA.DataSource = ds;
            DGSHOWDATA.DataBind();
            btnpriview.Visible = true;
            DGSHOWDATA.Visible = true;
            Session["ReportPath"] = "Reports/Rpt_cartain_stockNEW.rpt";
            Session["CommanFormula"] = "{v_cartainstock.orderid}=" + ddorderno.SelectedValue + "";
        }
        else
        {
            btnpriview.Visible = false;
            DGSHOWDATA.Visible = false;
        }
    }
    protected void DGSHOWDATA_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
        e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGSHOWDATA, "Select$" + e.Row.RowIndex);
    }
    protected void DGSHOWDATA_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Session["pid"] = ((Label)DGSHOWDATA.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("lbldetailid")).Text;
        Session["ReportPath"] = "Reports/Rpt_cartain_stock_detailNEW.rpt";
        Session["CommanFormula"] = "{PakingProcessReceiveDetail.pdetailid}=" + Session["pid"] + "";
    }
    protected void btnpriview_Click(object sender, EventArgs e)
    {
        Report();
    }
    private void Report()
    {
        string qry = "";
        if (Convert.ToString(Session["ReportPath"]) == "Reports/Rpt_cartain_stockNEW.rpt")
        {
            qry = @"  SELECT v_cartainstock.productcode,v_cartainstock.length,v_cartainstock.width,v_cartainstock.height,v_cartainstock.ply,v_cartainstock.qty,v_cartainstock.pqty,
                v_cartainstock.receiveqty,CompanyInfo.CompanyName,CompanyInfo.CompAddr1,CompanyInfo.CompAddr2,CompanyInfo.CompAddr3,CompanyInfo.CompFax,CompanyInfo.CompTel,
                CompanyInfo.TinNo,OrderMaster.CustomerOrderNo,OrderMaster.localorder
                FROM  v_cartainstock INNER JOIN CompanyInfo ON v_cartainstock.companyid=CompanyInfo.CompanyId INNER JOIN OrderMaster ON v_cartainstock.orderid=OrderMaster.OrderId
                where v_cartainstock.orderid=" + ddorderno.SelectedValue + " And CompanyInfo.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            Session["dsFileName"] = "~\\ReportSchema\\Rpt_cartain_stockNEW.xsd";
        }
        else
        {
            qry = @"SELECT PakingProcessReceiveDetail.Length,PakingProcessReceiveDetail.Width,PakingProcessReceiveDetail.height,PakingProcessReceiveDetail.Ply,PakingProcessReceiveDetail.Qty,PackingProcessReceiveMaster.ReceiveDate,CompanyInfo.CompanyName,
                CompanyInfo.CompAddr1,CompanyInfo.CompAddr2,CompanyInfo.CompAddr3,CompanyInfo.CompFax,CompanyInfo.CompTel,CompanyInfo.TinNo
                FROM PakingProcessReceiveDetail INNER JOIN PackingProcessReceiveMaster ON PakingProcessReceiveDetail.PackingReceiveId=PackingProcessReceiveMaster.PackingReceiveId
                INNER JOIN CompanyInfo ON PackingProcessReceiveMaster.CompanyId=CompanyInfo.CompanyId
                where PakingProcessReceiveDetail.pdetailid=" + Session["pid"] + " And CompanyInfo.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            Session["dsFileName"] = "~\\ReportSchema\\Rpt_cartain_stock_detailNEW.xsd";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Session["rptFileName"] = "~\\Reports\\Rpt_cartain_stockNEW.rpt";
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
    }
    protected void DGSHOWDATA_RowCreated(object sender, GridViewRowEventArgs e)
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