using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_reportdlk1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            if (Request.QueryString["Type"] == "u")
                rdorderRevised.Checked = true;
            else if (Request.QueryString["Type"] == "p")
                RDProjectionOrder.Checked = true;
            UtilityModule.ConditionalComboFill(ref ddCatagory, @"select im.CATEGORY_ID,im.CATEGORY_NAME from ITEM_CATEGORY_MASTER im inner join UserRights_Category uc On im.CATEGORY_ID=uc.CategoryId where uc.userid=" + Session["Varuserid"] + " And im.MasterCompanyId=" + Session["varCompanyId"] + "", true, "-Select-");
        }

    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo CustomerOrderNo 
                    From OrderMaster OM,Jobassigns JA,V_FinishedItemDetail vd,orderdetail od 
                    Where om.status=0 and od.orderid=om.orderid and vd.ITEM_FINISHED_ID=od.Item_Finished_Id and OM.Orderid=JA.Orderid and 
                    vd.CATEGORY_ID=" + ddCatagory.SelectedValue + " And vd.MasterCompanyId=" + Session["varCompanyId"] + " Order By CustomerOrderNo";
        if (Session["varCompanyId"].ToString() == "16" || Session["varCompanyId"].ToString() == "28")
        {
            str = @"Select Distinct OM.OrderId, CustomerOrderNo 
                From OrderMaster OM,Jobassigns JA,V_FinishedItemDetail vd,orderdetail od 
                Where om.status=0 and od.orderid=om.orderid and vd.ITEM_FINISHED_ID=od.Item_Finished_Id and OM.Orderid=JA.Orderid and 
                vd.CATEGORY_ID=" + ddCatagory.SelectedValue + " And vd.MasterCompanyId=" + Session["varCompanyId"] + " Order By CustomerOrderNo";
        }
        UtilityModule.ConditionalComboFill(ref ddOrderno, str, true, "-ALL-");
    }
    protected void btnsybmit_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        string sql = "";
        if (rdorderRevised.Checked == true)
        {
            Session["ReportPath"] = "Reports/RptPurchaseOrderrevised.rpt";
            sql = @"select distinct om.orderid,om.LocalOrder+'/'+om.CustomerOrderNo  As OrderNo,pii.PindentIssueid,replace(convert(varchar(11),pii.duedate,106),' ','-') as deliverydate,replace(convert(varchar(11),pt.Date,106),' ','-') as dateby,pt.remark as remark,replace(convert(varchar(11),pt.RemarkCurrentDate,106),' ','-') as Remarkdate from ordermaster om inner join 
                   PurchaseIndentIssue pii On pii.orderid=om.orderid left outer join PurchaseTracking pt On pt.PTrackId=pii.PindentIssueid inner join V_Order_category vo On vo.orderid=om.orderid
                   Where om.status=0 And pii.MasterCompanyId=" + Session["varCompanyId"];
            if (ddCatagory.SelectedIndex > 0)
            {
                sql = sql + "and CATEGORY_ID=" + ddCatagory.SelectedValue + "";
            }
            if (ddOrderno.SelectedIndex > 0)
            {
                sql = sql + "and om.orderid=" + ddOrderno.SelectedValue + "";
            }
            Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseOrderrevised.xsd";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        }
        else if (RDProjectionOrder.Checked == true)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            Session["ReportPath"] = "Reports/RptProjectionOrder.rpt";
            sql = @"Select om.Orderid,om.Localorder+'/'+om.CustomerOrderNo OrderNo,vf.CATEGORY_NAME+' '+Vf.ITEM_NAME+' '+Vf.QualityName+' '+Vf.designName+' '+Vf.ColorName+' '+Vf.ShadeColorName
                ShapeName,isnull(sum(od.Qty),0)as QtyProjected,isnull(sum(pit.QTY),0) as purchaseQty,vf.ITEM_FINISHED_ID from ordermaster om Left Outer join 
                OrderLocalConsumption od On om.orderid=od.orderid inner join 
                V_FinishedItemDetail vf On od.Finishedid=vf.ITEM_FINISHED_ID Left Outer join
                PurchaseIndentIssue Pii On Pii.orderid=om.orderid left Outer join
                PurchaseReceiveDetail Pit On pii.PindentIssueid=pit.PindentIssueid and pit.Finishedid=Vf.ITEM_FINISHED_ID inner join
                orderdetail odd On om.orderid=odd.orderid inner join V_FinishedItemDetail vf1 On odd.Item_Finished_Id=vf1.ITEM_FINISHED_ID
                Where om.Status=0 And vf.MasterCompanyId=" + Session["varCompanyId"];
            if (ddCatagory.SelectedIndex > 0)
            {
                sql = sql + "and Vf1.CATEGORY_ID=" + ddCatagory.SelectedValue + "";
            }
            if (ddOrderno.SelectedIndex > 0)
            {
                sql = sql + "and om.orderid=" + ddOrderno.SelectedValue + "";
            }
            sql = sql + "Group By om.Orderid,om.Localorder,om.CustomerOrderNo,vf.CATEGORY_NAME,Vf.ITEM_NAME,Vf.QualityName,Vf.designName,Vf.ColorName,Vf.ShadeColorName,vf.ITEM_FINISHED_ID";
            SqlDataAdapter sda = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ds.Tables.Add(dt);
            string sql1 = @"select om.Orderid,om.Localorder+'/'+om.CustomerOrderNo OrderNo,isnull(sum(Qty),0) as Assignqty,oa.FromOrderid,Item_Finished_id from ordermaster om  left outer join 
            OrderAssignQtyOrder oa On om.orderid=oa.ToOrderid And oa.MasterCompanyId=" + Session["varCompanyId"] + "  Group by om.Orderid,om.Localorder,om.CustomerOrderNo,oa.FromOrderid,Item_Finished_id";
            SqlDataAdapter sda1 = new SqlDataAdapter(sql1, con);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            ds.Tables.Add(dt1);
            Session["dsFileName"] = "~\\ReportSchema\\RptProjectionOrderRpt.xsd";
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
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