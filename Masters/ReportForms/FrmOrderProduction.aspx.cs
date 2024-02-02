using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_FrmOrderProduction : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string Qry = @"select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.USERID=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by Companyname
            select distinct ci.customerid,ci.Customercode From customerinfo ci 
            Inner join OrderMaster om on om.customerid=ci.customerid And ci.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" inner join ORDER_CONSUMPTION_DETAIL ocd on ocd.orderid=om.orderid
            Inner join Jobassigns JA ON OM.Orderid=JA.Orderid";
            DataSet ds = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "Select Comp Name");
            UtilityModule.ConditionalComboFillWithDS(ref ddcustomer, ds, 1, true, "Select CustomerCode");
            ddCompName.SelectedIndex = 1;
            rdrawmaterial.Checked = true;
        }
    }
    protected void ddcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddOrderno, "Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA Where OM.Orderid=JA.Orderid And CustomerId=" + ddcustomer.SelectedValue + " And om.CompanyId=" + ddCompName.SelectedValue, true, "Select OrderNo.");
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        Session["ReportPath"] = "Reports/RptOrderProductionReport.rpt";
        string str1 = @"Select c.CustomerCode+' /'+LocalOrder+'/'+CustomerOrderNo as BuyerCode ,CATEGORY_NAME+'  '+ITEM_NAME+' '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName as item,case when od.OrderUnitId=1 Then Sizemtr when od.OrderUnitId=2 then Sizeft else sizeinch end as size,sum(od.QtyRequired) as Qty,om.orderid,od.Item_Finished_Id,cm.CompanyName,cm.CompAddr1,cm.CompAddr2,cm.CompAddr3,cm.CompFax,cm.CompTel
        From ordermaster om inner join orderdetail od On om.orderid=od.orderid  inner join customerinfo c On om.CustomerId=c.CustomerId Inner join 
        V_FinishedItemDetail v On v.ITEM_FINISHED_ID=od.Item_Finished_Id inner join companyinfo cm On cm.CompanyId=om.CompanyId 
        Where om.orderid=" + ddOrderno.SelectedValue + @"
        Group by CustomerCode,LocalOrder,CustomerOrderNo,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,Sizemtr,Sizeft,sizeinch,om.orderid,od.Item_Finished_Id,OrderUnitId,cm.CompanyName,CompAddr1,CompAddr2,CompAddr3,CompFax,CompTel";
        SqlDataAdapter sda = new SqlDataAdapter(str1, con);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        ds.Tables.Add(dt);
        string str2 = @"select ItemDescription,Sum(Qty) as goodsrec,Finishedid,orderid from V_PurchaseReceiveReport
        Where orderid=" + ddOrderno.SelectedValue + " Group by ItemDescription,Finishedid,orderid";
        SqlDataAdapter sda1 = new SqlDataAdapter(str2, con);
        DataTable dt1 = new DataTable();
        sda1.Fill(dt1);
        ds.Tables.Add(dt1);
        string str3 = @"select PROCESS_NAME,sum(issueqty) as issueqty,sum(Recqty) as RecQty,vp.Item_Finished_Id,orderid,CATEGORY_NAME+'  '+ITEM_NAME+'  '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName+'  '+SizeMtr as Description
        From View_production_Issue_Rec vp Inner join V_FinishedItemDetail vd On vp.Item_Finished_Id=vd.Item_Finished_Id 
        Where orderid=" + ddOrderno.SelectedValue + @"
        Group by PROCESS_NAME,vp.Item_Finished_Id,orderid,CATEGORY_NAME,iTEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,SizeMtr";
        SqlDataAdapter sda2 = new SqlDataAdapter(str3, con);
        DataTable dt2 = new DataTable();
        sda2.Fill(dt2);
        ds.Tables.Add(dt2);
        string str4 = @"select orderid,finishedid,CATEGORY_NAME+'  '+ITEM_NAME+'  '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName+'  '+SizeMtr as Description ,sum(TotalPcs) as PackQty
        From PackingInformation p inner join V_FinishedItemDetail vd On p.finishedid=vd.item_finished_id where orderid=" + ddOrderno.SelectedValue + @"
        Group by orderid,finishedid,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,SizeMtr";
        SqlDataAdapter sda3 = new SqlDataAdapter(str4, con);
        DataTable dt3 = new DataTable();
        sda3.Fill(dt3);
        ds.Tables.Add(dt3);
        Session["dsFileName"] = "~\\ReportSchema\\RptOrderProductionReport.xsd";
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