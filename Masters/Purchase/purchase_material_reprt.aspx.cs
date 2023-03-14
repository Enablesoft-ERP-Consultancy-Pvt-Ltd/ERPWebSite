using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Purchase_purchase_material_reprt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            if (Request.QueryString["Type"].ToString() == "4" || Request.QueryString["Type"].ToString() == "6")
            {
                tdsupply.Visible = true;
                Tdcomp.Visible = true;
                tdcustomer.Visible = false;
                tdorder.Visible = false;
            }
            else
            {
                tdorder.Visible = true;
                tdsupply.Visible = false;
                Tdcomp.Visible = true;
                tdcustomer.Visible = true;
            }

            string qry = @"select empid,empname from empinfo Where MasterCompanyId=" + Session["varCompanyId"] + @"
            select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.USERID=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order by Companyname
            select distinct ci.customerid,ci.Customercode from customerinfo ci 
            Inner join OrderMaster om on om.customerid=ci.customerid And ci.MasterCompanyId=" + Session["varCompanyId"] + @" inner join ORDER_CONSUMPTION_DETAIL ocd on ocd.orderid=om.orderid
            Inner join Jobassigns JA ON OM.Orderid=JA.Orderid";
            DataSet ds = SqlHelper.ExecuteDataset(qry);
            UtilityModule.ConditionalComboFillWithDS(ref dsuppl, ds, 0, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 1, true, "Select Comp Name");
            UtilityModule.ConditionalComboFillWithDS(ref ddcustomer, ds, 2, true, "Select CustomerCode");
            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }
        }
    }
    protected void ddcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddOrderno, "Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA Where OM.Orderid=JA.Orderid And CustomerId=" + ddcustomer.SelectedValue + " And OM.CompanyId=" + ddCompName.SelectedValue, true, "Select OrderNo.");
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["Type"].ToString() == "1")
        {
            Session["ReportPath"] = "Reports/rpt_rawmaterial_stockNEW.rpt";
            Session["CommanFormula"] = "{ordermaster.orderid}=" + ddOrderno.SelectedValue + "";
        }
        else if (Request.QueryString["Type"].ToString() == "2")
        {
            Session["ReportPath"] = "Reports/rptpurchaseorder_empwiseNEW.rpt";
            Session["CommanFormula"] = "{v_purchase_emp_order_wise.orderid}=" + ddOrderno.SelectedValue + "";
        }
        else if (Request.QueryString["Type"].ToString() == "3")
        {
            Session["ReportPath"] = "Reports/RptPurchaseHisabAgainstOrderNEW.rpt";
            Session["CommanFormula"] = "{v_purchase_emp_order_wise.orderid}=" + ddOrderno.SelectedValue + "";
        }
        else if (Request.QueryString["Type"].ToString() == "4")
        {
            Session["ReportPath"] = "Reports/RptPurchaseHisabSupplierWiseNEW.rpt";
            Session["CommanFormula"] = "{v_purchase_emp_order_wise.empid}=" + dsuppl.SelectedValue + "";
        }
        else if (Request.QueryString["Type"].ToString() == "5")
        {
            Session["ReportPath"] = "Reports/Rpt_Sup LedBuyerOrderNEW.rpt";
            Session["CommanFormula"] = "{v_purchaseHissab.orderid}=" + ddOrderno.SelectedValue + "";
        }
        else if (Request.QueryString["Type"].ToString() == "6")
        {
            Session["ReportPath"] = "Reports/Rpt_Sup LedBuyerAllOrderNEW.rpt";
            Session["CommanFormula"] = "{v_purchaseHissab.empid}=" + dsuppl.SelectedValue + "";
        }
        else if (Request.QueryString["Type"].ToString() == "7")
        {
            Session["ReportPath"] = "Reports/RptstockdetailDestiniNEW.rpt";
            Session["CommanFormula"] = "{v_orderstock.orderid}=" + ddOrderno.SelectedValue + "";
        }
        else if (Request.QueryString["Type"].ToString() == "8")
        {
            Session["ReportPath"] = "Reports/RptPurchaseOrderDetailNEW.rpt";
            Session["CommanFormula"] = "{V_CONSUMPTION_OrderWise.orderid}=" + ddOrderno.SelectedValue + "";
        }
        else if (Request.QueryString["Type"].ToString() == "9")
        {
            Session["ReportPath"] = "Reports/RptIndentDetailNEW.rpt";
        }        
        Report();
    }
    private void Report()
    {
        string qry = "";
        string str = "";
        string query = "";
        DataSet ds = new DataSet(); ;
        if (Convert.ToString(Session["ReportPath"]) == "Reports/rpt_rawmaterial_stockNEW.rpt")
        {
            qry = @"SELECT OrderMaster.CustomerOrderNo,OrderDetail.QtyRequired,V_FinishedItemDetail.CATEGORY_NAME,V_FinishedItemDetail.ITEM_NAME,V_FinishedItemDetail.QualityName,V_FinishedItemDetail.designName,
                  V_FinishedItemDetail.ColorName,V_FinishedItemDetail.ShadeColorName,V_FinishedItemDetail.ShapeName,V_FinishedItemDetail.SizeMtr,V_FinishedItemDetail.SizeFt,V_FinishedItemDetail.AreaFt,
                  V_FinishedItemDetail.AreaMtr,OrderMaster.OrderId,OrderMaster.LocalOrder,OrderDetail.OrderUnitId
                  FROM  OrderDetail INNER JOIN OrderMaster ON OrderDetail.OrderId=OrderMaster.OrderId INNER JOIN
                  V_FinishedItemDetail ON OrderDetail.Item_Finished_Id=V_FinishedItemDetail.ITEM_FINISHED_ID
                  Where ordermaster.orderid=" + ddOrderno.SelectedValue + " And V_FinishedItemDetail.MasterCompanyId=" + Session["varCompanyId"];
            str = @"SELECT V_Raw_Matrial_CONSUMPTION.item_description,V_Raw_Matrial_CONSUMPTION.qualityname,V_Raw_Matrial_CONSUMPTION.IQTY,V_Raw_Matrial_CONSUMPTION.IUNITID,V_Raw_Matrial_CONSUMPTION.Totqty,V_Raw_Matrial_CONSUMPTION.qty,V_Raw_Matrial_CONSUMPTION.prate,V_Raw_Matrial_CONSUMPTION.orderid
                  FROM  V_Raw_Matrial_CONSUMPTION Where orderid=" + ddOrderno.SelectedValue + "";
            Session["dsFileName"] = "~\\ReportSchema\\rpt_rawmaterial_stockNEW.xsd";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            SqlDataAdapter sda = new SqlDataAdapter(str, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ds.Tables.Add(dt);
            
        }
        else if (Convert.ToString(Session["ReportPath"]) == "Reports/rptpurchaseorder_empwiseNEW.rpt")
        {
            qry = @"SELECT v_purchase_emp_order_wise.empname,v_purchase_emp_order_wise.item_description,v_purchase_emp_order_wise.qty,v_purchase_emp_order_wise.recqty,v_purchase_emp_order_wise.pindentissueid,
                  v_purchase_emp_order_wise.deliverydate,isnull(v_purchase_emp_order_wise.recdate,Replace(Convert(nvarchar(11),getdate(),106),' ','-')) recdate,v_purchase_emp_order_wise.purchasereceiveid,OrderMaster.CustomerOrderNo,OrderMaster.LocalOrder,v_purchase_emp_order_wise.qualityname,v_purchase_emp_order_wise.orderdate,finishedid
                  FROM v_purchase_emp_order_wise INNER JOIN OrderMaster ON v_purchase_emp_order_wise.orderid=OrderMaster.OrderId
                  Where v_purchase_emp_order_wise.orderid=" + ddOrderno.SelectedValue + "";
            Session["dsFileName"] = "~\\ReportSchema\\rptpurchaseorder_empwiseNEW.xsd";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        }
        else if (Convert.ToString(Session["ReportPath"]) == "Reports/RptPurchaseHisabAgainstOrderNEW.rpt")
        {
            qry = @" SELECT v_purchase_emp_order_wise.empname,v_purchase_emp_order_wise.item_description,v_purchase_emp_order_wise.qualityname,v_purchase_emp_order_wise.colour,v_purchase_emp_order_wise.qty,
                  v_purchase_emp_order_wise.recqty,v_purchase_emp_order_wise.pindentissueid,v_purchase_emp_order_wise.billno,v_purchase_emp_order_wise.gateinno,v_purchase_emp_order_wise.rate,
                  v_purchase_emp_order_wise.purchasereceiveid,v_purchase_emp_order_wise.qtyreturn,v_purchase_emp_order_wise.billno1,OrderMaster.CustomerOrderNo,OrderMaster.LocalOrder,finishedid
                  FROM v_purchase_emp_order_wise INNER JOIN OrderMaster ON v_purchase_emp_order_wise.orderid=OrderMaster.OrderId
                  Where v_purchase_emp_order_wise.orderid=" + ddOrderno.SelectedValue + "";
            Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseHisabAgainstOrderNEW.xsd";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        }
        else if (Convert.ToString(Session["ReportPath"]) == "Reports/RptPurchaseHisabSupplierWiseNEW.rpt")
        {
            qry = @"SELECT v_purchase_emp_order_wise.empname,v_purchase_emp_order_wise.item_description,v_purchase_emp_order_wise.qualityname,v_purchase_emp_order_wise.colour,v_purchase_emp_order_wise.qty,
                  v_purchase_emp_order_wise.recqty,v_purchase_emp_order_wise.pindentissueid,v_purchase_emp_order_wise.billno,v_purchase_emp_order_wise.gateinno,v_purchase_emp_order_wise.rate,
                  v_purchase_emp_order_wise.purchasereceiveid,v_purchase_emp_order_wise.qtyreturn,v_purchase_emp_order_wise.billno1,OrderMaster.CustomerOrderNo,OrderMaster.LocalOrder,customerinfo.CustomerCode
                  FROM v_purchase_emp_order_wise INNER JOIN OrderMaster ON v_purchase_emp_order_wise.orderid=OrderMaster.OrderId INNER JOIN customerinfo ON OrderMaster.CustomerId=customerinfo.CustomerId
                  Where v_purchase_emp_order_wise.empid=" + dsuppl.SelectedValue + "";
            Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseHisabSupplierWiseNEW.xsd";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        }
        else if (Convert.ToString(Session["ReportPath"]) == "Reports/Rpt_SupLedBuyerOrderNEW.rpt")
        {
            qry = @"SELECT v_purchaseHissab.customercode,v_purchaseHissab.empname,v_purchaseHissab.billno,v_purchaseHissab.total,v_purchaseHissab.cridet,
                  v_purchaseHissab.tds,v_purchaseHissab.orderno FROM  v_purchaseHissab where v_purchaseHissab.orderid=" + ddOrderno.SelectedValue + "";
            Session["dsFileName"] = "~\\ReportSchema\\Rpt_SupLedBuyerOrderNEW.xsd";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        }
        else if (Convert.ToString(Session["ReportPath"]) == "Reports/RptPurchaseOrderDetailNEW.rpt")
        {
            qry = @" SELECT V_CONSUMPTION_OrderWise.ifinishedid,V_CONSUMPTION_OrderWise.description,V_CONSUMPTION_OrderWise.Totqty,Isnull(V_PurchaseDetail.pqty,0) pqty,
                    isnull(V_PurchaseDetail.pamt,0)pamt,V_CONSUMPTION_OrderWise.orderno,isnull(V_PurchaseDetail.rqty,0) rqty,isnull(V_PurchaseDetail.rrate,0)rrate
                  FROM V_CONSUMPTION_OrderWise LEFT OUTER JOIN V_PurchaseDetail ON V_CONSUMPTION_OrderWise.orderid=V_PurchaseDetail.orderid AND V_CONSUMPTION_OrderWise.ifinishedid=V_PurchaseDetail.finishedid
                  Where  V_CONSUMPTION_OrderWise.orderid=" + ddOrderno.SelectedValue + "";
            Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseOrderDetailNEW.xsd";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        }
        else if (Convert.ToString(Session["ReportPath"]) == "Reports/RptstockdetailDestiniNEW.rpt")
        {
            qry = @"SELECT v_orderstock.RECQTY,v_order_consumption_process.process_name,v_order_consumption_process.process_name_id,v_order_consumption_process.orderid,v_orderstock.orderid,v_stockindentdetail.Description,v_stockindentdetail.orderqty,v_stockindentdetail.issueindent
                  FROM   v_stockindentdetail INNER JOIN v_orderstock ON v_stockindentdetail.orderid=v_orderstock.orderid AND v_stockindentdetail.ofinishedid=v_orderstock.ofinishedid INNER JOIN v_order_consumption_process ON v_stockindentdetail.processid=v_order_consumption_process.process_name_id
                  AND v_orderstock.orderid=v_order_consumption_process.orderid AND v_orderstock.ofinishedid=v_order_consumption_process.ofinishedid AND v_orderstock.processid=v_order_consumption_process.process_name_id
                  Where v_orderstock.orderid=" + ddOrderno.SelectedValue + " ORDER BY v_order_consumption_process.process_name_id";
            str = @"SELECT OrderId,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShapeName,Size_Name,QtyRequired
                  FROM NewOrderReport Where orderid=" + ddOrderno.SelectedValue + "";
            query = @"SELECT VIEW_GENERATEINDENTQTY.ORDERID,VIEW_GENERATEINDENTQTY.PROCESSID,VIEW_GENERATEINDENTQTY.IQTY,v_process_stock.Description,v_process_stock.issueqty
                    FROM   v_process_stock INNER JOIN VIEW_GENERATEINDENTQTY ON v_process_stock.processid=VIEW_GENERATEINDENTQTY.PROCESSID AND v_process_stock.FINISHEDID=VIEW_GENERATEINDENTQTY.IFINISHEDID
                    Where v_process_stock.orderid=" + ddOrderno.SelectedValue + "";
            Session["dsFileName"] = "~\\ReportSchema\\RptstockdetailDestiniNEW.xsd";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            SqlDataAdapter sda = new SqlDataAdapter(str, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ds.Tables.Add(dt);
            SqlDataAdapter sda1 = new SqlDataAdapter(query, con);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            ds.Tables.Add(dt1);
            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        }
        else if (Convert.ToString(Session["ReportPath"]) == "Reports/RptIndentDetailNEW.rpt")
        {
//            qry = @"Select CATEGORY_NAME+'  '+ITEM_NAME+'  '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName+'  '+SizeMtr+'  '+SizeFt+'  '+FINISHED_TYPE_NAME as Description,e.empname as empname,im.indentid,IndentNo,Quantity Indentqty,v.ITEM_FINISHED_ID ,isnull(sum(recquantity),0) as RecQty,pm.GatepassNo,pm.ChallanNo,im.Date as JWDate,pm.Date as RecDate,pt.Godownid as recgodown,im.ProcessID,pnm.PROCESS_NAME,g.GodownName
//                   From indentmaster im inner join Indentdetail id On im.indentid=id.indentid inner join V_FinishedItemDetail v On v.ITEM_FINISHED_ID=id.ofinishedid inner join 
//                   FINISHED_TYPE vt On vt.Id=id.O_FINISHED_TYPE_ID left Outer join PP_ProcessRecTran pt On im.indentid=pt.indentid and pt.finishedid=id.ofinishedid Inner join
//                   Empinfo e On im.partyid=e.empid left outer join PP_ProcessRecMAster pm ON pm.PRMid=pt.PRMid inner join PROCESS_NAME_MASTER pnm On pnm.PROCESS_NAME_ID=im.ProcessID Inner join
//                   GodownMaster g On g.GoDownID=pt.Godownid
//                   Where id.orderid=" + ddOrderno.SelectedValue + @"
//                   Group by CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,SizeMtr,SizeFt,FINISHED_TYPE_NAME,im.indentid,IndentNo,Quantity,v.ITEM_FINISHED_ID,e.empname,pm.GatepassNo,pm.ChallanNo,im.Date,pm.Date,pt.Godownid,im.ProcessID ,pnm.PROCESS_NAME,g.GodownName";
            qry = @" Select Distinct Description,empname,indentid,IndentNo,Indentqty,ITEM_FINISHED_ID,RecQty,GatepassNo,ChallanNo,JWDate,RecDate,recgodown,ProcessID,PROCESS_NAME,GodownName
                      FROM V_IndentDetail_Destini  Where orderid=" + ddOrderno.SelectedValue;
            Session["dsFileName"] = "~\\ReportSchema\\RptIndentDetailNEW.xsd";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            // Session["rptFileName"] = "~\\Reports\\WCarpetRecvNew.rpt";
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            //Session["dsFileName"] = "~\\ReportSchema\\rpt_rawmaterial_stockNEW.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
   
}