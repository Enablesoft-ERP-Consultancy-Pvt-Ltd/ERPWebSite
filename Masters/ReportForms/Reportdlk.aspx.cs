using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
public partial class Masters_ReportForms_Reportdlk : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        { 
            tdreporttype.Visible = true;
            string str=@"select empid,empname from empinfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by empname 
            Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by Companyname 
            select distinct customerid,Customercode + SPACE(5)+CompanyName from customerinfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
            select im.CATEGORY_ID,im.CATEGORY_NAME from ITEM_CATEGORY_MASTER im inner join UserRights_Category uc On im.CATEGORY_ID=uc.CategoryId where uc.userid=" + Session["Varuserid"] + " And im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            DataSet ds=SqlHelper.ExecuteDataset(str); 
            UtilityModule.ConditionalComboFillWithDS(ref dsuppl,ds,0 , true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds,1, true, "Select Comp Name");
            UtilityModule.ConditionalComboFillWithDS(ref ddcustomer, ds,2, true, "Select CustomerCode");
            UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, ds, 3, true, "-ALL-");
            UtilityModule.ConditionalComboFill(ref ddCatagory, "select Distinct im.CATEGORY_ID,im.CATEGORY_NAME from ITEM_CATEGORY_MASTER im inner join UserRights_Category uc On im.CATEGORY_ID=uc.CategoryId inner join V_FinishedItemDetail vd on im.CATEGORY_ID=vd.CATEGORY_ID inner join Orderdetail od On od.Item_Finished_Id=vd.ITEM_FINISHED_ID where uc.userid=" + Session["Varuserid"] + " And im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "-ALL-");
            UtilityModule.ConditionalComboFill(ref ddOrderno, "Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA,V_FinishedItemDetail vd,orderdetail od Where om.status=0 and od.orderid=om.orderid and vd.ITEM_FINISHED_ID=od.Item_Finished_Id and OM.Orderid=JA.Orderid And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "-ALL-");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName .Enabled = false;
            }

            if (ddcustomer.Items.Count > 0)
            {
                ddcustomer.SelectedIndex = 1;
                ddcustomer_change();
            }
            TxtFRDate.Text = DateTime.Now.AddMonths(-1).ToString("dd-MMM-yyyy");
            TxtTODate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            rdgarmentorder.Checked = true;
            trsupply.Visible = false;
        }
    }
    protected void ddcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddcustomer_change();
    }
    private void ddcustomer_change()
    {
        if (Rdpurchase.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddOrderno, @"Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo 
        from OrderMaster OM,Jobassigns JA,V_FinishedItemDetail vd,orderdetail od,PurchaseIndentIssue p
        Where om.status=0 and od.orderid=om.orderid and vd.ITEM_FINISHED_ID=od.Item_Finished_Id and p.orderid=om.orderid and OM.Orderid=JA.Orderid And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "And OM.CustomerId=" + ddcustomer.SelectedValue+"", true, "-Select Order No-");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddOrderno, "Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA Where om.status=0 and OM.Orderid=JA.Orderid And CustomerId=" + ddcustomer.SelectedValue, true, "-ALL-");
        }
    }
   
    protected void RDpurdelivRpt_CheckedChanged(object sender, EventArgs e)
    {
        Trcomp.Visible = true;
        trcustomer.Visible = false;
        trsupply.Visible = false;
        trfr.Visible = true;
        trto.Visible = false;
        LBLFRDATE.Text = "Stock At PH Month";
        btnExcelExport.Visible = true;
        BtnExport.Visible = true;
        pstatus.Visible = false;
        pindent.Visible = false;
        dpstatus.Visible = false;
        trgvdetail.Visible = false;
        trpptype.Visible = false;
        btnsybmit.Visible = true;
        btnpreview.Visible = true;
        BtnExport.Visible = true;
        btnExcelExport.Visible = true;
        btnPOExport.Visible = false;
        pstatus.Visible = false;
        trpusaseindent.Visible = false;
    }
    protected void RDindent_CheckedChanged(object sender, EventArgs e)
    {
        trsupply.Visible = false;
    }     
    protected void btnsybmit_Click(object sender, EventArgs e)
    {
        string qry = "", str = "";
        DataSet ds = new DataSet();
        if (rdgarmentorder.Checked == true || RDproductionRpt.Checked == true)
        {
            if (rdgarmentorder.Checked == true)
            {
                str = str + "Where om.OrderDate >= '" + TxtFRDate.Text + "' and om.OrderDate <='" + TxtTODate.Text + "'";
            }
            else if (RDproductionRpt.Checked == true)
            {
                str = str + "Where month(om.DispatchDate) = month('" + TxtFRDate.Text + "') ";
            }
            if (ddOrderno.SelectedIndex > 0)
            {
                str = str + "and om.orderid=" + ddOrderno.SelectedValue + "";
            }
            if (ddCatagory.SelectedIndex > 0)
            {
                str = str + "and vd.CATEGORY_ID=" + ddCatagory.SelectedValue + "";
            }
            if (DDStatus.SelectedValue!="3" )
            {
                str = str + " AND om.status=" + DDStatus.SelectedValue + "";
            }
            Session["ReportPath"] = "Reports/RptGarmentOrder.rpt";
            qry = @"select CATEGORY_NAME as Department,LocalOrder+'/'+CustomerOrderNo CustomerOrderNo,om.orderid,od.orderdetailid,CATEGORY_NAME +' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as Description ,Photo,om.status,case when ja.SupplierQty=0 then 'JW' else 'DP' end JWDP,od.QtyRequired as qty,
              Replace(convert(varchar(11),om.OrderDate,106),' ','-') as orderdate,replace(convert(varchar(11),om.DispatchDate,106),' ','-') as SRCdate,replace(convert(varchar(11),om.Custorderdate,106),' ','-') as Expdate,replace(convert(varchar(11),om.DueDate,106),' ','-') as Duedate,replace(convert(varchar(11),'" + TxtFRDate.Text + @"',106),' ','-') as frdate,replace(convert(varchar(11),'" + TxtTODate.Text + @"',106),' ','-') as Todate,'" + ddCatagory.SelectedItem + @"' as dept,
              (select case when FinalDate is null then '' else replace(convert(varchar(11),FinalDate,106),' ','-') end   from OrderProcessPlanning where orderid=om.orderid and processid=1) as purchasedate
              From ordermaster om inner join 
              Orderdetail od On om.orderid=od.orderid inner join 
              V_FinishedItemDetail vd On vd.ITEM_FINISHED_ID=od.Item_Finished_Id And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" inner join
              JobAssigns JA ON Ja.Orderid=om.OrderId and ja.ITEM_FINISHED_ID=od.Item_Finished_Id " + str + "";
            Session["dsFileName"] = "~\\ReportSchema\\RptGarmentOrder.xsd";
        }
        else if (RDPurchaseStatus.Checked == true)
        {
            Session["ReportPath"] = "Reports/RptPurchaseStatusDKL.rpt";
            if (ddCatagory.SelectedIndex > 0)
            {
                str = " and od.CATEGORY_ID=" + ddCatagory.SelectedValue + @"";
            }
             if(ddOrderno.SelectedIndex>0)
            {
                str = str + "and vc.orderid=" + ddOrderno.SelectedValue + ""; 
            }
             if (dsuppl.SelectedIndex > 0)
            {
                str = str + "  and vp.partyid="+dsuppl.SelectedValue+"";
            }
             if (DDStatus.SelectedValue != "3")
             {
                 str = str + " and vc.status=" + DDStatus.SelectedValue + @"";
             }
            qry = @"select vc.orderid,vc.Finishedid,vc.Discription,isnull(sum(vc.qty),0) qty,isnull(sum(vp.issueqty),0) as iqty,vc.orderno,isnull(sum(vp.RecQty),0) as RecQty,vc.finaldate 
            From View_DklConsumption vc left outer join  View_purchaseDetail vp On vc.orderid=vp.orderid and vc.finishedid=vp.finishedid inner join 
            V_Order_category od On vp.orderid=od.orderid 
            Where  vc.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And vp.pstatus='"+ddpstatus.SelectedValue+@"' "+str+@"   
            Group by vp.orderid,vc.Finishedid,vc.Discription,vc.orderno,vc.orderid,vc.finaldate";
            Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseStatusDKL.xsd";
        }
        else if (RDJwissue.Checked == true)
        {
            Session["ReportPath"] = "Reports/RptJWIndentStatusDKL.rpt";
            if (ddCatagory.SelectedIndex > 0)
            {
                str = " and Vc.CATEGORY_ID=" + ddCatagory.SelectedValue + @"";
            }
            if (ddOrderno.SelectedIndex > 0)
            {
                str = str + "and vi.orderid=" + ddOrderno.SelectedValue + "";
            }
            if (dsuppl.SelectedIndex > 0)
            {
                str = str + "  and vi.empid=" + dsuppl.SelectedValue + "";
            }
            if (DDStatus.SelectedValue != "3")
            {
                str = str + " and vi.status=" + DDStatus.SelectedValue + " ";
            }
            qry=@"select vi.orderid,orderno,Description,ofinishedid,SRC,indentqty,empname,empid ,vi.indentid,vr.Indentrecqty,status,vi.istatus
            From v_IndentDetail_DKl vi Inner join V_Order_category vc On vc.orderid=vi.orderid left Outer join 
            V_IndentReceive_Dkl vr On vi.indentid=vr.indentid and vi.ofinishedid=vr.Finishedid
            Where vi.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And vi.Istatus='"+ddistatus.SelectedValue+"' "+str+" ";
            Session["dsFileName"] = "~\\ReportSchema\\RptJWIndentStatusDKL.xsd";
        }
        else if (RDdpissue.Checked == true)
        {
            Session["ReportPath"] = "Reports/RptDPIssueStatusDKL.rpt";
            if (ddCatagory.SelectedIndex > 0)
            {
                str = " and Vd.CATEGORY_ID=" + ddCatagory.SelectedValue + @"";
            }
            if (ddOrderno.SelectedIndex > 0)
            {
                str = str + "and om.orderid=" + ddOrderno.SelectedValue + "";
            }
            if (dsuppl.SelectedIndex > 0)
            {
                str = str + "  and e.empid=" + dsuppl.SelectedValue + "";
            }
            if (DDStatus.SelectedValue != "3")
            {
                str = str + " and om.status=" + DDStatus.SelectedValue + " ";
            }
            qry = @"select om.orderid,pd.Item_Finished_Id as finishedid,om.localorder+om.customerorderno as orderno,e.empname,
            CATEGORY_NAME +'  '+ITEM_NAME+'  '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName as Description,
            isnull(sum(pd.Qty),0) as IssueQty,isnull(sum(prd.Qty),0) as RecQty
            From PROCESS_ISSUE_Master_9 pm Inner join PROCESS_ISSUE_DETAIL_9 pd ON pm.issueorderid=pd.issueorderid inner join 
            Ordermaster Om on om.orderid=pd.orderid inner join empinfo e ON e.empid=pm.empid inner join 
            V_FinishedItemDetail vd On vd.ITEM_FINISHED_ID=pd.Item_Finished_Id left outer join 
            PROCESS_RECEIVE_DETAIL_9 prd ON pm.IssueOrderId=prd.IssueOrderId and pd.Issue_Detail_Id=prd.Issue_Detail_Id and pd.Item_Finished_Id=prd.Item_Finished_Id
            where  e.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And pm.status='"+dpstatus1.SelectedValue+"' "+str+@"
            group by om.orderid,om.localorder+om.customerorderno,e.empname,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,pd.Item_Finished_Id";
            Session["dsFileName"] = "~\\ReportSchema\\RptDPIssueStatusDKL.xsd";
        }
        else if (Rdorderdetail.Checked == true)
        {
            str = str + "Where om.OrderDate >= '" + TxtFRDate.Text + "' and om.OrderDate <='" + TxtTODate.Text + "'";
            if (ddOrderno.SelectedIndex > 0)
            {
                str = str + "and om.orderid=" + ddOrderno.SelectedValue + "";
            }
            if (ddCatagory.SelectedIndex > 0)
            {
                str = str + "and om.orderid in(select orderid from v_Order_category where CATEGORY_ID=" + ddCatagory.SelectedValue + ")";
            }
            if (DDStatus.SelectedValue != "3")
            {
                str = str + " and om.status=" + DDStatus.SelectedValue + "";
            }
        qry = @"select distinct om.orderid,LocalOrder+'/'+CustomerOrderNo CustomerOrderNo,om.status,
        Case when sum(ja.SupplierQty)=0 then 'JW' else 'DP' end JWDP,sum(ja.OrderQty) as qty,
        Replace(convert(varchar(11),om.OrderDate,106),' ','-') as orderdate,replace(convert(varchar(11),om.DispatchDate,106),' ','-') as SRCdate,
        Replace(convert(varchar(11),om.Custorderdate,106),' ','-') as Expdate,replace(convert(varchar(11),om.DueDate,106),' ','-') as Duedate,
        Replace(convert(varchar(11),'" + TxtFRDate.Text + @"',106),' ','-') as frdate,
        Replace(convert(varchar(11),'" + TxtTODate.Text + @"',106),' ','-') as Todate,
        (select case when FinalDate is null then '' else replace(convert(varchar(11),FinalDate,106),' ','-') end From OrderProcessPlanning where orderid=om.orderid and processid=1) as purchasedate
        From ordermaster om inner join V_order_JobAssigns ja On om.orderid=ja.orderid   " + str + @"
        Group by LocalOrder,CustomerOrderNo,om.orderid,om.status,om.OrderDate,om.DispatchDate,om.Custorderdate,om.DueDate
        Order by orderid";
        }
        else if (RdVendorwise.Checked == true)
        {
            if (ddvendorstatus.SelectedValue == "0")
            {
                str = str + "Where im.DueDate >= '" + TxtFRDate.Text + "' and im.DueDate <='" + TxtTODate.Text + "'";
            }
            else
            {
                str = str + "Where reqdate >= '" + TxtFRDate.Text + "' and reqdate <='" + TxtTODate.Text + "'";
            }
            if (ddOrderno.SelectedIndex > 0)
            {
                str = str + "and om.orderid=" + ddOrderno.SelectedValue + "";
            }
            if (ddCatagory.SelectedIndex > 0)
            {
                str = str + "and om.orderid in(select orderid from v_Order_category where CATEGORY_ID=" + ddCatagory.SelectedValue + ")";
            }
            if (DDStatus.SelectedValue != "3")
            {
                str = str + " and om.status=" + DDStatus.SelectedValue + "";
            }
            if (dsuppl.SelectedIndex > 0)
            {
                str = str + " and EM.EMPID="+dsuppl.SelectedValue+"";
            }
            if (ddvendorstatus.SelectedValue == "0")
            {
                Session["ReportPath"] = "Reports/Rptvendorwisepurchase.rpt";
                qry = @"select EM.EMPID,em.empname,CATEGORY_NAME +'  '+ITEM_NAME+'  '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName as Description,replace(convert(varchar(11),im.Date,106),' ','-') as orderdate,
                replace(convert(varchar(11),im.DueDate,106),' ','-') as reqdate ,isnull((quantity),0)as orderqty,isnull(sum(QTY),0)as Recqty,
                om.localorder+'/'+Om.CustomerOrderNo as orderno,om.orderid,OM.STATUS ,
                CASE WHEN OM.STATUS=0 THEN 'Pending' when om.status=1 then 'Complete' else 'Cancel' End as stat,
                (select distinct CATEGORY_NAME from v_Order_category vc ,V_FinishedItemDetail vd1 where vc.CATEGORY_ID=vd1.CATEGORY_ID and orderid=om.orderid And vd1.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + ") as department,'"+ddvendorstatus.SelectedItem+@"' as vendorstatus
                From PurchaseIndentIssue im inner join PurchaseIndentIssueTran id ON im.PindentIssueid=id.PindentIssueid And im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" inner join
                Empinfo em ON em.empid=im.Partyid inner join V_FinishedItemDetail vd On id.Finishedid=vd.item_finished_id inner join
                Ordermaster om ON om.orderid=im.Orderid left outer join PurchaseReceiveDetail ppt On im.PIndentIssueId=ppt.PIndentIssueId and ppt.FinishedId=id.Finishedid left outer join
                PurchaseReceiveMaster ppm On ppm.PurchaseReceiveId=ppt.PurchaseReceiveId " + str + @"
                Group by im.Date,im.DueDate,em.empname,om.localorder,Om.CustomerOrderNo,om.orderid,quantity,EM.EMPID,OM.STATUS,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName
                ORDER BY ORDERID";
                Session["dsFileName"] = "~\\ReportSchema\\Rptvendorwisepurchase.xsd";
            }
            else
            {
                Session["ReportPath"] = "Reports/RptvendorwiseProduction.rpt";
                qry = @"select CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as Desciption,em.empname,im.indentid,im.indentno,replace(convert(varchar(11),im.date,106),' ','-') as orderdate,
                replace(convert(varchar(11),reqdate,106),' ','-') as reqdate ,Isnull(sum(Quantity),0) as orderqty,reqdate as reqdt,om.localorder+'/'+Om.CustomerOrderNo as orderno,om.orderid,0 as Recqty,'' as rdate ,'' recdate,vd.item_finished_id as finishedid,'" + ddvendorstatus.SelectedItem + @"' as vendorstatus,CASE WHEN OM.STATUS=0 THEN 'Pending' when om.status=1 then 'Complete' else 'Cancel' End as stat
                From IndentMaster im inner join IndentDetail id ON im.IndentID=id.IndentID And im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" inner join Empinfo em ON em.empid=im.partyid inner join 
                V_FinishedItemDetail vd On id.OFinishedId=vd.item_finished_id inner join
                Ordermaster om ON om.orderid=id.orderid "+str+@"
                group by CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,em.empname,im.indentid,im.indentno,im.date,reqdate,om.localorder,Om.CustomerOrderNo,om.orderid,vd.item_finished_id,om.status
                union 
                select CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as Desciption,em.empname,im.indentid,im.indentno,replace(convert(varchar(11),im.date,106),' ','-') as orderdate,
                replace(convert(varchar(11),reqdate,106),' ','-') as reqdate ,0 as orderqty,reqdate as reqdt,om.localorder+'/'+Om.CustomerOrderNo as orderno,om.orderid, sum(RecQuantity) Recqty ,ppm.Date as rdate,replace(convert(varchar(11),ppm.Date,106),' ','-') as recdate,vd.item_finished_id as finishedid,'" + ddvendorstatus.SelectedItem + @"' as vendorstatus,CASE WHEN OM.STATUS=0 THEN 'Pending' when om.status=1 then 'Complete' else 'Cancel' End as stat
                from PP_ProcessRecMaster ppm inner join PP_ProcessRecTran ppt On ppm.PRMid=ppt.PRMid inner join 
                V_FinishedItemDetail vd On vd.item_finished_id=ppt.Finishedid  inner join IndentMaster im On im.indentid=ppt.indentid And im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" 
                inner join IndentDetail id ON im.IndentID=id.IndentID and id.OFinishedId=vd.item_finished_id inner join
                Empinfo em ON em.empid=ppm.Empid inner join
                Ordermaster om ON om.orderid=id.orderid "+str+ @"
                group by CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName,em.empname,im.indentid,im.indentno,
                om.localorder,Om.CustomerOrderNo,om.orderid,ppm.Date,reqdate,im.date,vd.item_finished_id,om.status";
                Session["dsFileName"] = "~\\ReportSchema\\RptvendorwiseProduction.xsd";
            }

        }
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0 && RDPurchaseStatus.Checked==false && RDJwissue.Checked==false && RDdpissue.Checked==false && Rdorderdetail.Checked==false && RdVendorwise.Checked==false)
        {
            DGOrderDetail2.DataSource = ds;
            DGOrderDetail2.DataBind();
            trgrid2.Visible = true;
            ddCompName.Enabled = false;
            ddcustomer.Enabled = false;
            ddOrderno.Enabled = false;
            dsuppl.Enabled = false;
            ddCatagory.Enabled = false;
        }
        else if (Rdorderdetail.Checked == true)
        {
            gvdetailorder.DataSource = ds;
            gvdetailorder.DataBind();
            trgvdetail.Visible = true;
        }
        else if (ds.Tables[0].Rows.Count > 0)
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
    protected void DGOrderDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string qry = "";//, str = "";
        DataSet ds = new DataSet();
         string orderid=((Label)DGOrderDetail2.Rows[e.RowIndex].FindControl("lblorderid")).Text ;
         string Jwdp = ((Label)DGOrderDetail2.Rows[e.RowIndex].FindControl("lbljwdp")).Text;
         if (Jwdp == "JW")
         {
             Session["ReportPath"] = "Reports/RptOrderStatusDKL.rpt";
         }
         else if(Jwdp=="DP")
         {
             Session["ReportPath"] = "Reports/RptOrderStatusDPDKL.rpt";
         }
        //Order Detail
         qry = @"select CATEGORY_NAME as Department,LOCALORDER+'/'+CustomerOrderNo AS CustomerOrderNo,om.orderid,od.orderdetailid,CATEGORY_NAME +' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as Description ,Photo,om.status,od.QtyRequired as qty,
               Replace(convert(varchar(11),om.OrderDate,106),' ','-') as orderdate,replace(convert(varchar(11),om.DispatchDate,106),' ','-') as SRCdate,replace(convert(varchar(11),om.Custorderdate,106),' ','-') as Expdate,replace(convert(varchar(11),om.DueDate,106),' ','-') as Duedate,replace(convert(varchar(11),'" + TxtFRDate.Text + @"',106),' ','-') as frdate,replace(convert(varchar(11),'" + TxtTODate.Text + @"',106),' ','-') as Todate,'" + ddCatagory.SelectedItem + @"' as dept
               From ordermaster om inner join Orderdetail od On om.orderid=od.orderid inner join 
               V_FinishedItemDetail vd On vd.ITEM_FINISHED_ID=od.Item_Finished_Id where om.orderid=" + orderid + " And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
         ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
         SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
         if (Jwdp == "JW")
         {
             //Indent
             string str1 = @"select   MAX(im.date) as orderdate,MAX(reqdate) as reqdate ,isnull(sum(Quantity),0)as orderqty,isnull(sum(RecQuantity),0)as Recqty,
                           MAX(ppm.Date) as recdate,Om.localorder+'/'+Om.CustomerOrderNo as orderno,om.orderid
                           From IndentMaster im inner join IndentDetail id ON im.IndentID=id.IndentID inner join
                           V_FinishedItemDetail vd On id.OFinishedId=vd.item_finished_id And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" inner join Ordermaster om ON om.orderid=id.orderid left outer join
                           PP_ProcessRecTran ppt On im.IndentID=ppt.IndentID and ppt.Finishedid=id.OFinishedId left outer join
                           PP_ProcessRecMaster ppm On ppm.PRMid=ppt.PRMid Where om.orderid=" + orderid + " GROUP BY om.localorder,Om.CustomerOrderNo,om.orderid ";
             SqlDataAdapter sda = new SqlDataAdapter(str1, con);
             DataTable dt = new DataTable();
             sda.Fill(dt);
             ds.Tables.Add(dt);
             //Purchase Detail
             string str2 = @"select Om.localorder+'/'+Om.CustomerOrderNo as orderno,om.orderid,(Select ISNULL(SUM(quantity),0) from PurchaseIndentIssueTran pt,PurchaseIndentIssue pm where pm.PIndentIssueId=pt.PIndentIssueId and pm.orderid=om.orderid) AS ORDERQTY,ISNULL(SUM(ppt.QTY),0) AS RECQTY,MAX(PPM.ReceiveDate) AS RECDATE,MAX(ID.Delivery_Date) AS REQDATE,MAX(IM.Date) AS ORDERDATE,
                          (select isnull(sum(Qty),0) from OrderProcessPlanning where orderid=om.orderid and processid=1 ) as planingqty,op.Date,op.currentdate AS PLANDATE, OP.FinalDate,op.Appdate
                          From PurchaseIndentIssue im inner join PurchaseIndentIssueTran id ON im.PindentIssueid=id.PindentIssueid And im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" left Outer join
                          Ordermaster om ON om.orderid=IM.orderid left outer join OrderProcessPlanning op ON op.orderid=om.orderid  left outer join
                          PurchaseReceiveDetail ppt On im.PIndentIssueId=ppt.PIndentIssueId and ppt.PIndentIssueTranId=id.PIndentIssueTranId left outer join
                          PurchaseReceiveMaster ppm On ppm.PurchaseReceiveId=ppt.PurchaseReceiveId Where om.orderid="+orderid+@" and op.processid=1      
                          GROUP BY om.localorder, Om.CustomerOrderNo,om.orderid,op.Date,op.FinalDate,op.Appdate,op.currentdate";
             SqlDataAdapter sda1 = new SqlDataAdapter(str2, con);
             DataTable dt1 = new DataTable();
             sda1.Fill(dt1);
             ds.Tables.Add(dt1);
             //Production Detail
             string str3 = @"select CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as Desciption,em.empname,im.indentid,im.indentno,replace(convert(varchar(11),im.date,106),' ','-') as orderdate,
                    replace(convert(varchar(11),reqdate,106),' ','-') as reqdate ,isnull(sum(Quantity),0)as orderqty,isnull(sum(RecQuantity),0)as Recqty,
                    replace(convert(varchar(11),ppm.Date,106),' ','-') as recdate,replace(convert(varchar(11),'" + TxtFRDate.Text + @"',106),' ','-') as frdate,
                    replace(convert(varchar(11),'" + TxtTODate.Text + @"',106),' ','-') as Todate,ppm.Date as rdate,reqdate as reqdt,om.localorder+'/'+Om.CustomerOrderNo as orderno,om.orderid,vd.item_finished_id as finishedid
                    From IndentMaster im inner join
                    IndentDetail id ON im.IndentID=id.IndentID And im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" inner join
                    Empinfo em ON em.empid=im.partyid inner join 
                    V_FinishedItemDetail vd On id.OFinishedId=vd.item_finished_id inner join
                    Ordermaster om ON om.orderid=id.orderid left outer join
                    PP_ProcessRecTran ppt On im.IndentID=ppt.IndentID and ppt.Finishedid=id.OFinishedId left outer join
                    PP_ProcessRecMaster ppm On ppm.PRMid=ppt.PRMid Where om.Orderid=" + orderid + @"
                    Group by im.indentid,im.indentno,im.date,im.reqdate,em.empname,ppm.Date,om.localorder,Om.CustomerOrderNo,om.orderid,vd.item_finished_id,CATEGORY_NAME,ITEM_NAME,
                    QualityName,designName,ColorName,ShadeColorName,ShapeName 
                    Having isnull(sum(Quantity),0)>0";
             SqlDataAdapter sda2 = new SqlDataAdapter(str3, con);
             DataTable dt2 = new DataTable();
             sda2.Fill(dt2);
             ds.Tables.Add(dt2);
             // Purchase Detail
             string str4 = @"select CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as Desciption,em.empname,im.Challanno,replace(convert(varchar(11),im.Date,106),' ','-') as orderdate,
             replace(convert(varchar(11),im.DueDate,106),' ','-') as reqdate ,isnull((quantity),0)as orderqty,isnull(sum(QTY),0)as Recqty,
             replace(convert(varchar(11),ppm.ReceiveDate,106),' ','-') as recdate,ppm.ReceiveDate as rdate,im.DueDate as reqdt,om.localorder+'/'+Om.CustomerOrderNo as orderno,om.orderid,vd.item_finished_id
             From PurchaseIndentIssue im inner join PurchaseIndentIssueTran id ON im.PindentIssueid=id.PindentIssueid And im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" inner join
             Empinfo em ON em.empid=im.Partyid inner join V_FinishedItemDetail vd On id.Finishedid=vd.item_finished_id inner join
             Ordermaster om ON om.orderid=im.Orderid left outer join PurchaseReceiveDetail ppt On im.PIndentIssueId=ppt.PIndentIssueId and ppt.FinishedId=id.Finishedid left outer join
             PurchaseReceiveMaster ppm On ppm.PurchaseReceiveId=ppt.PurchaseReceiveId 
             Where om.orderid="+orderid+@" 
             Group by im.Challanno,im.Date,im.DueDate,em.empname,ppm.ReceiveDate,om.localorder,Om.CustomerOrderNo,om.orderid,vd.item_finished_id,CATEGORY_NAME,ITEM_NAME,QualityName,designName,
             ColorName,ShadeColorName,ShapeName,quantity
             Having isnull((quantity),0)>0";
             SqlDataAdapter sda3 = new SqlDataAdapter(str4, con);
             DataTable dt3 = new DataTable();
             sda3.Fill(dt3);
             ds.Tables.Add(dt3);
             //Purchase Summary
             string str5 = @"select CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' ' +ShapeName as Description,
                           (select sum(quantity)from PurchaseIndentIssueTran a where Finishedid=vd.ITEM_FINISHED_ID and Pindentissuetranid=pt.Pindentissuetranid and pm.orderid=" + orderid + @" )as orderqty,sum(QTY) as RecQty 
                           from PurchaseIndentIssue pm inner join PurchaseIndentIssueTran pt on pm.PindentIssueid=pt.PindentIssueid And pm.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" inner join 
                           dbo.V_FinishedItemDetail vd On vd.ITEM_FINISHED_ID=pt.Finishedid left outer join
                           dbo.PurchaseReceiveDetail prd On prd.PIndentIssueId=pm.PIndentIssueId and prd.Pindentissuetranid=pt.Pindentissuetranid and prd.FinishedId=pt.Finishedid
                           where pm.orderid=" + orderid + " group by CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName, vd.ITEM_FINISHED_ID,pm.orderid,pt.Pindentissuetranid";
             SqlDataAdapter sda4 = new SqlDataAdapter(str5, con);
             DataTable dt4 = new DataTable();
             sda4.Fill(dt4);
             ds.Tables.Add(dt4);
             //Production Summary
             string str6 = @"select omt.localorder+'/'+omt.CustomerOrderNo orderno, Item_Name+'  '+QualityName+'  '+Designname+'  '+ColorName+'  '+ShadeColorName As Description,isnull(Sum(Quantity),0) As orderqty,
                           isnull(sum(recquantity),0) as Recqty,omt.orderid
                           From IndentMaster OM inner join IndentDetail OD On OM.IndentId=OD.IndentId And om.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" inner join
                           ordermaster omt on omt.orderid=od.orderid inner join
                           V_FinishedItemDetail V on V.Item_Finished_Id=OD.OFinishedId left outer join
                           PP_ProcessRecTran pt on pt.indentid=OD.IndentId and pt.finishedid=OD.OFinishedId
                           where omt.orderid=" +orderid+" Group by omt.localorder,Item_Name,QualityName,Designname,ColorName,ShadeColorName,SizeMtr,SizeFt ,OD.OFinishedId,omt.CustomerOrderNo,omt.orderid";
             SqlDataAdapter sda5 = new SqlDataAdapter(str6, con);
             DataTable dt5 = new DataTable();
             sda5.Fill(dt5);
             ds.Tables.Add(dt5);
             string str7 = @"select CATEGORY_NAME+'  '+ITEM_NAME+'  '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName as Description ,Qty from OrderLocalConsumption oc,V_FinishedItemDetail v where oc.Finishedid=v.ITEM_FINISHED_ID and orderid=" + orderid + " And v.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
             SqlDataAdapter sda6 = new SqlDataAdapter(str7, con);
             DataTable dt6 = new DataTable();
             sda6.Fill(dt6);
             ds.Tables.Add(dt6);
             string str8 = @"Select 1  seq,'Garment Order Form' as Name,orderid,Remarks as garmentorder from ordermaster where orderid=" + orderid + @" Union
                           Select 2 seq,'Fabric Capture Form' as Name,orderid,Remark as localconsumption from OrderLocalConsumption where orderid=" + orderid + @" Union
                           Select 3 seq,'Order Planning Form- Purchase' as Name,orderid,planRemark as purchasePlan from OrderProcessPlanning where processid=1 and orderid=" + orderid + @" Union
                           Select 4 seq,'Order Planning Form- Production' as Name,orderid,planRemark as processplan from OrderProcessPlanning where processid=2 and orderid=" + orderid + @" Union
                           Select 5 seq,'Order Department Planning Form- Purchase' as Name,orderid,DepRemark as purchasedept from OrderProcessPlanning where processid=1 and orderid=" + orderid + @" Union
                           Select 6 seq,'Order Department Planning Form- Production' as Name,orderid,DepRemark as processdep from OrderProcessPlanning where processid=2 and orderid=" + orderid + @" Union
                           Select 7 seq,'Purchase Order Form' as Name,PindentIssueid as porder,Remarks as purchaseRemark from PurchaseIndentIssue where orderid=" + orderid + @" Union
                           Select 8 seq,'Purchase Receive Form' as Name,PM.PurchaseReceiveId as precorder,MRemark as purchaserec from PurchaseReceiveMaster pm inner join 
                           PurchaseReceivedetail pd On pm.PurchaseReceiveId=pd.PurchaseReceiveId Inner join PurchaseIndentIssue pii On pii.PindentIssueid=pd.PindentIssueid Where pii.orderid=" + orderid + @" Union
                           Select 9 seq,'JW Planning for Garments Form' as Name,IM.IndentID as jw,GRemark as Gremark from indentmaster im inner join indentdetail id On im.indentid=id.indentid where id.orderid=" + orderid + @" Union
                           Select 10 seq,'Indent Raw Issue form' as Name,PM.PRMid AS pRODORDER,RiRemark as PRemark from PP_ProcessRawMaster pm Inner join PP_ProcessRawtran pd On pm.PRMid=pd.PRMid inner join indentdetail id On pd.indentid=id.indentid  where id.orderid=" + orderid + @" Union
                           Select 11 seq,'Indent Raw Receive Form' as Name,PM.PRMid AS PRODRECORDER, RRRemark PRRECIVEremark from PP_ProcessRecMaster pm inner join PP_ProcessRectran pd On pm.PRMid=pd.PRMid Inner join indentdetail id On pd.indentid=id.indentid  where id.orderid=" + orderid + @" ";
             SqlDataAdapter sda7 = new SqlDataAdapter(str8, con);
             DataTable dt7 = new DataTable();
             sda7.Fill(dt7);
             ds.Tables.Add(dt7);
             string str9 = @"select om.orderid, om.localorder+'/'+customerorderno as OrderNo,IndentNo,empname,sum(IssueQuantity) as issueQty ,CATEGORY_NAME +'  '+ITEM_NAME+'  '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName as description
                           From V_INDENTDETAIL ID INNER JOIN PP_ProcessRawtran PRT On id.indentid=prt.indentid inner join ordermaster om On id.orderid=om.orderid inner join 
                           V_FinishedItemDetail vd On vd.item_finished_id=prt.Finishedid And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                           where om.orderid="+orderid+@"
                           Group by om.localorder,customerorderno,IndentNo,empname,om.orderid,vd.item_finished_id,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName
                           Order by orderid";
             SqlDataAdapter sda8 = new SqlDataAdapter(str9, con);
             DataTable dt8 = new DataTable();
             sda8.Fill(dt8);
             ds.Tables.Add(dt8);
             Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseOrderDKl.xsd";
         }
         else if (Jwdp == "DP")
         {
             string str2 = @"select replace(convert(varchar(11),max(AssignDate),106),' ','-') as orderdate,replace(convert(varchar(11),max(prm.ReceiveDate),106),' ','-') as RecDate,isnull(sum(pd.Qty),0) as dpqty,pd.orderid,isnull(sum(prd.Qty),0) as dprec
                         From PROCESS_ISSUE_MASTER_9 pi Inner join 
                         PROCESS_ISSUE_DETAIL_9 pd On pi.IssueOrderId=pd.IssueOrderId left outer join
                         PROCESS_RECEIVE_DETAIL_9 prd On prd.IssueOrderId=pi.IssueOrderId and pi.IssueOrderId=prd.IssueOrderId and pd.Issue_Detail_Id=prd.Issue_Detail_Id and pd.orderid=prd.orderid left outer join
                         PROCESS_RECEIVE_MASTER_9 prm On prm.Process_Rec_Id=prd.Process_Rec_Id and prd.orderid=pd.orderid
                         Where pd.orderid="+orderid+" Group by pd.orderid";
             SqlDataAdapter sda1 = new SqlDataAdapter(str2, con);
             DataTable dt1 = new DataTable();
             sda1.Fill(dt1);
             ds.Tables.Add(dt1);
            
            string str3 = @"Select CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as Desciption, em.empname,im.IssueOrderId,replace(convert(varchar(11),im.AssignDate,106),' ','-') as orderdate,
                          replace(convert(varchar(11),id.ReqByDate,106),' ','-') as reqdate ,isnull(sum(id.Qty),0)as orderqty,isnull(sum(ppt.Qty),0)as Recqty,
                          replace(convert(varchar(11),ppm.ReceiveDate,106),' ','-') as recdate,ppm.ReceiveDate as rdate,id.ReqByDate as reqdt,om.localorder+'/'+Om.CustomerOrderNo as orderno,om.orderid,vd.item_finished_id
                          From PROCESS_ISSUE_MASTER_9 im inner join PROCESS_ISSUE_DETAIL_9 id ON im.IssueOrderId=id.IssueOrderId inner join
                          Empinfo em ON em.empid=im.Empid inner join V_FinishedItemDetail vd On id.Item_Finished_Id=vd.item_finished_id And em.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" inner join
                          Ordermaster om ON om.orderid=id.Orderid left outer join PROCESS_RECEIVE_DETAIL_9 ppt On im.IssueOrderId=ppt.IssueOrderId and ppt.Item_Finished_Id=id.Item_Finished_Id and id.orderid=ppt.orderid left outer join
                          PROCESS_RECEIVE_MASTER_9 ppm On ppm.Process_Rec_Id=ppt.Process_Rec_Id 
                          Where om.orderid=" + orderid + " Group by im.IssueOrderId,im.AssignDate,id.ReqByDate,em.empname,ppm.ReceiveDate,om.localorder,Om.CustomerOrderNo,om.orderid,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,vd.item_finished_id";
            SqlDataAdapter sda2 = new SqlDataAdapter(str3, con);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            ds.Tables.Add(dt2);
            string str4 = @"select CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as Desciption,isnull(sum(pd.Qty),0) as dpqty,pd.orderid,isnull(sum(prd.Qty),0) as dprec
                         From PROCESS_ISSUE_MASTER_9 pi Inner join
                         PROCESS_ISSUE_DETAIL_9 pd On pi.IssueOrderId=pd.IssueOrderId Inner join 
                         V_FinishedItemDetail v On v.Item_Finished_Id=pd.Item_Finished_Id And v.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" left outer join
                         PROCESS_RECEIVE_DETAIL_9 prd On prd.IssueOrderId=pi.IssueOrderId and pi.IssueOrderId=prd.IssueOrderId and pd.Issue_Detail_Id=prd.Issue_Detail_Id and pd.orderid=prd.orderid and prd.Item_Finished_Id=v.Item_Finished_Id left outer join
                         PROCESS_RECEIVE_MASTER_9 prm On prm.Process_Rec_Id=prd.Process_Rec_Id and prd.orderid=pd.orderid  
                         Where pd.orderid="+orderid+" Group by pd.orderid,prd.Item_Finished_Id,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName ";
            SqlDataAdapter sda3 = new SqlDataAdapter(str4, con);
            DataTable dt3 = new DataTable();
            sda3.Fill(dt3);
            ds.Tables.Add(dt3);
            string str5 = @"Select 1  seq,'Garment Order Form' as Name,orderid,Remarks as garmentorder from ordermaster where orderid="+orderid+@" Union
                        Select 2 Seq,'DP Order Form'as Name,pim.IssueOrderId,remarks as remark from PROCESS_ISSUE_MASTER_9 pim Inner join PROCESS_ISSUE_Detail_9 pid On pim.IssueOrderId=pid.IssueOrderId where pid.orderid="+orderid+@" Union
                        Select 3 Seq,'DP Receive Form'as Name,pim.Process_Rec_Id,remarks as remark from PROCESS_RECEIVE_MASTER_9 pim Inner join PROCESS_RECEIVE_detail_9 pid On pim.Process_Rec_Id=pid.Process_Rec_Id where pid.orderid="+orderid+"";
            SqlDataAdapter sda4 = new SqlDataAdapter(str5, con);
            DataTable dt4 = new DataTable();
            sda4.Fill(dt4);
            ds.Tables.Add(dt4);
            Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseOrderDPDKl.xsd";
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderDetail2, "Select$" + e.Row.RowIndex);
        }

    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        trgrid2.Visible = false;
        ddCompName.Enabled = true;
        ddcustomer.Enabled = true;
        ddOrderno.Enabled = true;
        dsuppl.Enabled = true;
        ddCatagory.Enabled = true;
    }
    protected void BtnExport_Click(object sender, EventArgs e)
    {
        if (Rdorderdetail.Checked == false)
        {
            string qry = @"Select CATEGORY_NAME as Department,LocalOrder+'/'+CustomerOrderNo CustomerOrderNo,om.orderid,od.orderdetailid,CATEGORY_NAME +' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as Description ,Photo,om.status,case when ja.SupplierQty=0 then 'JW' else 'DP' end JWDP,od.QtyRequired as qty,
              Replace(convert(varchar(11),om.OrderDate,106),' ','-') as orderdate,replace(convert(varchar(11),om.DispatchDate,106),' ','-') as SRCdate,replace(convert(varchar(11),om.Custorderdate,106),' ','-') as Expdate,replace(convert(varchar(11),om.DueDate,106),' ','-') as Duedate,replace(convert(varchar(11),'" + TxtFRDate.Text + @"',106),' ','-') as frdate,replace(convert(varchar(11),'" + TxtTODate.Text + @"',106),' ','-') as Todate,'" + ddCatagory.SelectedItem + @"' as dept,
              (select case when FinalDate is null then '' else replace(convert(varchar(11),FinalDate,106),' ','-') end   from OrderProcessPlanning where orderid=om.orderid and processid=1) as purchasedate
              From ordermaster om inner join 
              Orderdetail od On om.orderid=od.orderid inner join 
              V_FinishedItemDetail vd On vd.ITEM_FINISHED_ID=od.Item_Finished_Id And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" inner join
              JobAssigns JA ON Ja.Orderid=om.OrderId and ja.ITEM_FINISHED_ID=od.Item_Finished_Id ";
            if (rdgarmentorder.Checked == true)
            {
                qry = qry + "Where om.OrderDate >= '" + TxtFRDate.Text + "' and om.OrderDate <='" + TxtTODate.Text + "'";
            }
            else if (RDproductionRpt.Checked == true)
            {
                qry = qry + "Where month(om.DispatchDate) = month('" + TxtFRDate.Text + "')";
            }
            if (ddOrderno.SelectedIndex > 0)
            {
                qry = qry + "and om.orderid=" + ddOrderno.SelectedValue + "";
            }
            if (ddCatagory.SelectedIndex > 0)
            {
                qry = qry + "and vd.CATEGORY_ID=" + ddCatagory.SelectedValue + "";
            }

            DataSet ds3 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            DGOrderDetail2.DataSource = ds3;
            DGOrderDetail2.DataBind();
            if (DGOrderDetail2.Rows.Count > 0)
            {
                DGOrderDetail2.Style.Add("font-size", "1em");
                Response.Clear();
                string attachment = "attachment; filename=OrderHistory.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.IO.StringWriter stringWrite = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                DGOrderDetail2.RenderControl(htmlWrite);
                Response.Write(stringWrite.ToString());
                Response.End();
                
            }
        }
        else if (gvdetailorder.Rows.Count > 0)
        {
            gvdetailorder.Style.Add("font-size", "1em");
            Response.Clear();
            string attachment = "attachment; filename=OrderHistory.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            gvdetailorder.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }
        else
        {
            
        }
    }

    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddCatagory.SelectedIndex == 0)
        {
            UtilityModule.ConditionalComboFill(ref ddOrderno, @"Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA,V_FinishedItemDetail vd,
            orderdetail od Where om.status=" + DDStatus.SelectedValue + " and od.orderid=om.orderid and vd.ITEM_FINISHED_ID=od.Item_Finished_Id and OM.Orderid=JA.Orderid And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "-ALL-");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddOrderno, @"Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA,V_FinishedItemDetail vd,
            orderdetail od Where om.status=" + DDStatus.SelectedValue + " and od.orderid=om.orderid and vd.ITEM_FINISHED_ID=od.Item_Finished_Id and OM.Orderid=JA.Orderid and vd.CATEGORY_ID=" + ddCatagory.SelectedValue + " And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "-ALL-");
        }
        if (Rdpurchase.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddOrderno, @"Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo 
            From OrderMaster OM,Jobassigns JA,V_FinishedItemDetail vd,orderdetail od,PurchaseIndentIssue p
            Where om.status=0 and od.orderid=om.orderid and vd.ITEM_FINISHED_ID=od.Item_Finished_Id and p.orderid=om.orderid and OM.Orderid=JA.Orderid And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "-Select Order No-");
        }
    }
    protected void RDRawMaterial_CheckedChanged(object sender, EventArgs e)
    {
        Trcomp.Visible = true;
        trcustomer.Visible = false;
        trsupply.Visible = false;
        trfr.Visible = true;
        LBLFRDATE.Text = "From Date";
        trto.Visible = true;
        btnExcelExport.Visible = true;
        BtnExport.Visible = true;
        pstatus.Visible = false;
        pindent.Visible = false;
        dpstatus.Visible = false;
        trgvdetail.Visible = false;
        trpptype.Visible = false;
        btnsybmit.Visible = true;
        btnpreview.Visible = true;
        BtnExport.Visible = true;
        btnExcelExport.Visible = true;
        btnPOExport.Visible = false;
        pstatus.Visible = false;
        trpusaseindent.Visible = false;
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        PostBackTrigger trigger = new PostBackTrigger();
        trigger.ControlID = btnExcelExport.ID;
        up1.Triggers.Add(trigger);
        PostBackTrigger trigger1 = new PostBackTrigger();
        trigger1.ControlID = BtnExport.ID;
        up1.Triggers.Add(trigger1);
        PostBackTrigger trigger2 = new PostBackTrigger();
        trigger2.ControlID = btnPOExport.ID;
        up1.Triggers.Add(trigger2);
    }
    protected void DDStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        if (DDStatus.SelectedValue != "3")
        {
             str = "om.status=" + DDStatus.SelectedValue + " and";
        }
        if (ddCatagory.SelectedIndex == 0)
        {
            UtilityModule.ConditionalComboFill(ref ddOrderno, @"Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA,V_FinishedItemDetail vd,
            orderdetail od Where "+str+" od.orderid=om.orderid and vd.ITEM_FINISHED_ID=od.Item_Finished_Id and OM.Orderid=JA.Orderid And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "-ALL-");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddOrderno, @"Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA,V_FinishedItemDetail vd,
            orderdetail od Where "+str+"  od.orderid=om.orderid and vd.ITEM_FINISHED_ID=od.Item_Finished_Id and OM.Orderid=JA.Orderid and vd.CATEGORY_ID=" + ddCatagory.SelectedValue + " And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "-ALL-");
        }
        if (Rdpurchase.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddOrderno, @"Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo 
            from OrderMaster OM,Jobassigns JA,V_FinishedItemDetail vd,orderdetail od,PurchaseIndentIssue p
            Where "+str+" od.orderid=om.orderid and vd.ITEM_FINISHED_ID=od.Item_Finished_Id and p.orderid=om.orderid and OM.Orderid=JA.Orderid And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "-Select Order No-");
        }
    }
    protected void btnExcelExport_Click(object sender, EventArgs e)
    {
        if (ddOrderno.SelectedIndex > 0)
        {
            string str = @"SELECT ID,Category_Name +'  '+VF.ITEM_NAME+'  '+QualityName+'  '+DesignName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName+'  '+
                           CASE WHEN OD.SizeUnit=1 Then SizeMtr Else SizeFt End  Description,U1.Unitname As Unit,Qty,Od.thanlength,od.remark FROM OrderLocalConsumption OD Inner JOIN V_FinishedItemDetail VF ON OD.FinishedId=VF.Item_Finished_Id And vf.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" 
                             INNER JOIN ITEM_PARAMETER_MASTER IPM ON OD.FinishedId=IPM.Item_Finished_Id INNER JOIN Unit U On OD.SizeUnit=U.UnitId inner Join Unit U1 on OD.UnitId=U1.UnitId  Where OrderId=" + ddOrderno.SelectedValue + "";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DGConsumption.DataSource = ds;
                DGConsumption.DataBind();
            }
            string str1 = @"select Distinct om.orderid,om.localorder,vf.CATEGORY_NAME,om.customerorderNO,Replace(convert(varchar(11),OrderDate,106),' ','-') as OrderDate ,Replace(convert(varchar(11),om.DispatchDate,106),' ','-') AS SRC,Replace(convert(varchar(11),DueDate,106),' ','-') as StoreDElDAte,(select  case when Date is not null then Replace(convert(varchar(11),Date,106),' ','-') else ' ' end from OrderProcessPlanning where orderid=om.orderid and processid=1) as DelDate 
            from ordermaster om inner join 
            orderdetail od On om.orderid=od.orderid inner join V_FinishedItemDetail VF On OD.Item_Finished_Id=VF.Item_Finished_Id And Vf.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
            Where om.orderid=" + ddOrderno.SelectedValue + "";
            SqlDataAdapter sda = new SqlDataAdapter(str1, ErpGlobal.DBCONNECTIONSTRING);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ds.Tables.Add(dt);
            if (ds.Tables[1].Rows.Count > 0)
            {
                DGConsumption.Style.Add("font-size", "1em");
                Response.Clear();
                string attachment = "attachment; filename=Internal Fabric Sheet.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.IO.StringWriter stringWrite = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                DGConsumption.RenderControl(htmlWrite);
                Response.Write(@"<TABLE><tr><td align=left style=font-weight:bold; colspan=3>Internal Fabric Procurement Sheet</td></tr><tr><td align=left colspan=3 style=font-weight:bold;>" + ddCompName.SelectedItem.Text + "</td></tr><tr align=center style=background-color:Silver;><td style=font-weight:bold;>General Detail</td></tr><tr><td></td><td style=font-weight:bold;>SR No.</td><td align=right>" + ds.Tables[1].Rows[0]["localorder"].ToString() + "</td></tr><tr><td></td><td style=font-weight:bold;>Category</td><td align=right>" + ds.Tables[1].Rows[0]["CATEGORY_NAME"].ToString() + "</td></tr><tr><td></td><td style=font-weight:bold;>Order Number</td><td align=right>" + ds.Tables[1].Rows[0]["customerorderNO"].ToString() + "</td></tr><tr><td></td><td style=font-weight:bold;>FAB ODER DATE</td><td align=right>" + ds.Tables[1].Rows[0]["OrderDate"].ToString() + "</td></tr><tr><td></td><td style=font-weight:bold;>Fab SRC</td><td align=right>" + ds.Tables[1].Rows[0]["SRC"].ToString() + "</td></tr><tr><td></td><td style=font-weight:bold;>Fab Cost</td><td align=right></td></tr><tr><td></td><td style=font-weight:bold;>Fab Del Date</td><td align=right>" + ds.Tables[1].Rows[0]["DelDate"] + "</td></tr><tr><td></td><td style=font-weight:bold;>Stock at Store</td><td align=right>" + ds.Tables[1].Rows[0]["StoreDElDAte"].ToString() + "</td></tr><tr><td></td><td style=font-weight:bold;>Garment Description</td><td></td></tr><tr><td></td><td style=font-weight:bold;>Consumption</td><td></td></tr><tr></tr><tr></tr></table>" + stringWrite.ToString() + "<table><tr><td align=right colspan=2 style=font-weight:bold;>Total</td><td>" + ds.Tables[0].Compute("sum(Qty)", " ").ToString() + "</td></tr><tr></tr><tr></tr><tr><td align=left colspan=3 style=font-weight:bold;>Signature of Category Head</td></tr></table>");
                Response.End();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Please Select Order No..');", true);
        }
    }
    protected void RDPurchaseStatus_CheckedChanged(object sender, EventArgs e)
    {
        Trcomp.Visible = false;
        trcustomer.Visible = false;
        trsupply.Visible = true;
        trfr.Visible = false;
        trto.Visible = false;
        btnExcelExport.Visible = false;
        BtnExport.Visible = false;
        pstatus.Visible = true;
        pindent.Visible = false;
        dpstatus.Visible = false;
        trgvdetail.Visible = false;
        trpptype.Visible = false;
        btnsybmit.Visible = true;
        btnpreview.Visible = true;
        BtnExport.Visible = true;
        btnExcelExport.Visible = true;
        btnPOExport.Visible = false;
        pstatus.Visible = false;
        trpusaseindent.Visible = false;
    }
    protected void ddOrderno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RDPurchaseStatus.Checked == true)
        {
            if (ddOrderno.SelectedIndex > 0 && RDPurchaseStatus.Checked==true)
            {
                UtilityModule.ConditionalComboFill(ref dsuppl, "select distinct empid,empname from empinfo e inner join PurchaseIndentIssue pii On e.empid=pii.partyid and pii.orderid=" + ddOrderno.SelectedValue + " And e.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by e.empname ", true, "--Select--");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref dsuppl, "select distinct empid,empname from empinfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by empname ", true, "--Select--");
            }
        }
        if (Rdpurchase.Visible == true && ddOrderno.SelectedIndex>0)
        {
            UtilityModule.ConditionalComboFill(ref ddpurchaseint, "select pindentissueid,cast(pindentissueid as varchar)+'  '+empname from purchaseindentissue p inner join empinfo e On p.partyid=e.empid where orderid="+ddOrderno.SelectedValue+"",true,"-select PO .No-");
        }
    }
    protected void RDJwissue_CheckedChanged(object sender, EventArgs e)
    {
        Trcomp.Visible = false;
        trcustomer.Visible = false;
        trsupply.Visible = true;
        trfr.Visible = false;
        trto.Visible = false;
        btnExcelExport.Visible = false;
        BtnExport.Visible = false;
        pstatus.Visible = false;
        pindent.Visible = true;
        dpstatus.Visible = false;
        trgvdetail.Visible = false;
        trpptype.Visible = false;
        btnsybmit.Visible = true;
        btnpreview.Visible = true;
        BtnExport.Visible = true;
        btnExcelExport.Visible = true;
        btnPOExport.Visible = false;
        pstatus.Visible = false;
        trpusaseindent.Visible = false;
    }
    protected void RDdpissue_CheckedChanged(object sender, EventArgs e)
    {
        Trcomp.Visible = false;
        trcustomer.Visible = false;
        trsupply.Visible = true;
        trfr.Visible = false;
        trto.Visible = false;
        btnExcelExport.Visible = false;
        BtnExport.Visible = false;
        pstatus.Visible = false;
        pindent.Visible = false;
        dpstatus.Visible = true;
        trgvdetail.Visible = false;
        trpptype.Visible = false;
        btnsybmit.Visible = true;
        btnpreview.Visible = true;
        BtnExport.Visible = true;
        btnExcelExport.Visible = true;
        btnPOExport.Visible = false;
        pstatus.Visible = false;
        trpusaseindent.Visible = false;
    }
    private void gvdetail()
    {
        string str1 = "",qry="";
         str1 = str1 + "Where om.OrderDate >= '" + TxtFRDate.Text + "' and om.OrderDate <='" + TxtTODate.Text + "' AND om.status=" + DDStatus.SelectedValue + "";
        if (ddOrderno.SelectedIndex > 0)
        {
            str1 = str1 + "and om.orderid=" + ddOrderno.SelectedValue + "";
        }
        if (ddCatagory.SelectedIndex > 0)
        {
            str1 = str1 + "and vd.CATEGORY_ID=" + ddCatagory.SelectedValue + "";
        }
        qry = @"select distinct om.orderid,LocalOrder+'/'+CustomerOrderNo CustomerOrderNo,om.status,
        case when sum(ja.SupplierQty)=0 then 'JW' else 'DP' end JWDP,isnull(sum(ja.OrderQty),0) as qty,
        Replace(convert(varchar(11),om.OrderDate,106),' ','-') as orderdate,replace(convert(varchar(11),om.DispatchDate,106),' ','-') as SRCdate,
        Replace(convert(varchar(11),om.Custorderdate,106),' ','-') as Expdate,replace(convert(varchar(11),om.DueDate,106),' ','-') as Duedate,
        Replace(convert(varchar(11),'" + TxtFRDate.Text + @"',106),' ','-') as frdate,
        Replace(convert(varchar(11),'" + TxtTODate.Text + @"',106),' ','-') as Todate,
        (select case when FinalDate is null then '' else replace(convert(varchar(11),FinalDate,106),' ','-') end From OrderProcessPlanning where orderid=om.orderid and processid=1) as purchasedate
        From ordermaster om inner join V_order_JobAssigns ja On om.orderid=ja.orderid  " + str1 + @"
        Group by LocalOrder,CustomerOrderNo,om.orderid,om.status,om.OrderDate,om.DispatchDate,om.Custorderdate,om.DueDate
        Order by orderid";
       DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
       gvdetailorder.DataSource = ds;
       gvdetailorder.DataBind();
       trgvdetail.Visible = true;
    }
    protected void RDorderdetail_CheckedChanged(object sender, EventArgs e)
    {
        Trcomp.Visible = true;
        trcustomer.Visible = false;
        trsupply.Visible = false;
        trfr.Visible = true;
        LBLFRDATE.Text = "From Date";
        trto.Visible = true;
        btnExcelExport.Visible = true;
        BtnExport.Visible = true;
        pstatus.Visible = false;
        pindent.Visible = false;
        dpstatus.Visible = false;
        trgvdetail.Visible = true;
        trpptype.Visible = false;
        btnsybmit.Visible = true;
        btnpreview.Visible = true;
        BtnExport.Visible = true;
        btnExcelExport.Visible = true;
        btnPOExport.Visible = false;
        pstatus.Visible = false;
        trpusaseindent.Visible = false;
        //gvdetail();
    }
    public string getdeptt(string Strval)
    {
        string val="";
        hndesc.Value = "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,   @"select Top(1) CATEGORY_NAME,CATEGORY_NAME+'  '+ITEM_NAME+'  '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName from ordermaster om Inner join orderdetail od On om.orderid=od.orderid Inner join 
        V_FinishedItemDetail vd On vd.ITEM_FINISHED_ID=od.ITEM_FINISHED_ID where om.orderid=" + Strval + " And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by orderdetailid");
        if (ds.Tables[0].Rows.Count > 0)
        {
            val = ds.Tables[0].Rows[0][0].ToString();
            hndesc.Value=ds.Tables[0].Rows[0][1].ToString();
        }
        return val;
    }
    public string getdiscription( string Strval)
    {
        string val = "";
        val = hndesc.Value;
        return val;
    }
    public string getRecPcs(string Strval,string Strval1)
    {
        string val = "",str="";
        if (Strval1 == "JW")
            str = "select isnull(sum(RecQuantity),0) from PP_ProcessRecTran where indentid in(select distinct indentid from indentdetail where  orderid=" + Strval + ")";
        else
            str = "select isnull(sum(Qty),0) from PROCESS_RECEIVE_DETAIL_9 where orderid="+Strval+"";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            val=ds.Tables[0].Rows[0][0].ToString();
            hnrecqty.Value = val;
        }
        return val;
    }
    public string getpendPcs(string Strval)
    {
        string val = "";
        val = Convert.ToString(Convert.ToDouble(Strval) - Convert.ToDouble(hnrecqty.Value));
        return val;
    }
    public string getordermtr(string Strval)
    {
        string val = "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(sum(qty),0) from  OrderLocalConsumption where orderid="+Strval+"");
        val = ds.Tables[0].Rows [0][0].ToString();
        hnordermtr.Value = val;
        return val;
    }
    public string getrecmtr(string Strval)
    {
        string val = "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(sum(QTY),0) from PurchaseIndentIssue Pii Inner join PurchaseReceiveDetail pit On pii.PindentIssueid=pit.PindentIssueid where orderid=" + Strval + " And pii.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
        val = ds.Tables[0].Rows[0][0].ToString();
        hnrecmtr.Value=ds.Tables[0].Rows[0][0].ToString();
        hnpstatus.Value="";
        return val;
    }
    public string getpendmtr()
    {
        string val = "";
        val = Convert.ToString(Convert.ToDouble(hnordermtr.Value) - Convert.ToDouble(hnrecmtr.Value));
        return val;
    }
    public string getpstatus(string Strval)
    {
        string val = "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Status from PurchaseIndentIssue where orderid="+Strval+"");
        if (ds.Tables[0].Rows.Count > 0)
        {
            val = ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            val="Pending";
        }
        return val;
    }
    public string getstatus(string strval)
    {
        string val = "";
        if (strval == "0")
            val = "Pending";
        else if (strval == "1")
            val = "Complete";
        else
            val = "Cancel";
        return val;
    }
    protected void gvdetailorder_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string sql = "";
        string orderid = ((Label)gvdetailorder.Rows[e.RowIndex].FindControl("lblorderid")).Text;
        Session["ReportPath"] = "Reports/RptPurchaseOrderrevised.rpt";
        sql = @"select distinct om.orderid,om.LocalOrder+'/'+om.CustomerOrderNo  As OrderNo,pii.PindentIssueid,replace(convert(varchar(11),pii.duedate,106),' ','-') as deliverydate,replace(convert(varchar(11),pt.Date,106),' ','-') as dateby,pt.remark as remark,replace(convert(varchar(11),pt.RemarkCurrentDate,106),' ','-') as RemarkDate from ordermaster om inner join 
                   PurchaseIndentIssue pii On pii.orderid=om.orderid left outer join PurchaseTracking pt On pt.PTrackId=pii.PindentIssueid inner join V_Order_category vo On vo.orderid=om.orderid
                   Where om.orderid=" +orderid+" And pii.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "";
        Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseOrderrevised.xsd";
       DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
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
    protected void gvdetailorder_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string qry = "";
        DataSet ds = new DataSet();
        string orderid = ((Label)gvdetailorder.Rows[e.RowIndex].FindControl("lblorderid")).Text;
        string Jwdp = ((Label)gvdetailorder.Rows[e.RowIndex].FindControl("lbljwdp")).Text;
        if (Jwdp == "JW")
        {
            Session["ReportPath"] = "Reports/RptOrderStatusDKL.rpt";
        }
        else if (Jwdp == "DP")
        {
            Session["ReportPath"] = "Reports/RptOrderStatusDPDKL.rpt";
        }
        //Order Detail
        qry = @"select CATEGORY_NAME as Department,LOCALORDER+'/'+CustomerOrderNo AS CustomerOrderNo,om.orderid,od.orderdetailid,CATEGORY_NAME +' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as Description ,Photo,om.status,od.QtyRequired as qty,
               Replace(convert(varchar(11),om.OrderDate,106),' ','-') as orderdate,replace(convert(varchar(11),om.DispatchDate,106),' ','-') as SRCdate,replace(convert(varchar(11),om.Custorderdate,106),' ','-') as Expdate,replace(convert(varchar(11),om.DueDate,106),' ','-') as Duedate,replace(convert(varchar(11),'" + TxtFRDate.Text + @"',106),' ','-') as frdate,replace(convert(varchar(11),'" + TxtTODate.Text + @"',106),' ','-') as Todate,'" + ddCatagory.SelectedItem + @"' as dept
               From ordermaster om inner join Orderdetail od On om.orderid=od.orderid inner join 
               V_FinishedItemDetail vd On vd.ITEM_FINISHED_ID=od.Item_Finished_Id where om.orderid=" + orderid + " And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "";
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (Jwdp == "JW")
        {
            //Indent
            string str1 = @"select   MAX(im.date) as orderdate,MAX(reqdate) as reqdate ,(select isnull(sum(Quantity),0) from IndentDetail where orderid=om.orderid ) as orderqty,isnull(sum(RecQuantity),0)as Recqty,
                           MAX(ppm.Date) as recdate,Om.localorder+'/'+Om.CustomerOrderNo as orderno,om.orderid
                           From IndentMaster im inner join IndentDetail id ON im.IndentID=id.IndentID inner join
                           V_FinishedItemDetail vd On id.OFinishedId=vd.item_finished_id inner join Ordermaster om ON om.orderid=id.orderid left outer join
                           PP_ProcessRecTran ppt On im.IndentID=ppt.IndentID and ppt.Finishedid=id.OFinishedId left outer join
                           PP_ProcessRecMaster ppm On ppm.PRMid=ppt.PRMid Where om.orderid=" + orderid + " And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " GROUP BY om.localorder,Om.CustomerOrderNo,om.orderid ";
            SqlDataAdapter sda = new SqlDataAdapter(str1, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ds.Tables.Add(dt);
            //Purchase Detail
            string str2 = @"select Om.localorder+'/'+Om.CustomerOrderNo as orderno,om.orderid,(Select ISNULL(SUM(quantity),0) from PurchaseIndentIssueTran pt,PurchaseIndentIssue pm where pm.PIndentIssueId=pt.PIndentIssueId and pm.orderid=om.orderid) AS ORDERQTY,ISNULL(SUM(ppt.QTY),0) AS RECQTY,MAX(PPM.ReceiveDate) AS RECDATE,MAX(ID.Delivery_Date) AS REQDATE,MAX(IM.Date) AS ORDERDATE,
                          (select isnull(sum(Qty),0) from OrderProcessPlanning where orderid=om.orderid and processid=1 ) as planingqty,op.Date,op.currentdate AS PLANDATE, OP.FinalDate,op.Appdate
                          From PurchaseIndentIssue im inner join PurchaseIndentIssueTran id ON im.PindentIssueid=id.PindentIssueid And im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" left Outer join
                          Ordermaster om ON om.orderid=IM.orderid left outer join OrderProcessPlanning op ON op.orderid=om.orderid  left outer join
                          PurchaseReceiveDetail ppt On im.PIndentIssueId=ppt.PIndentIssueId and ppt.PIndentIssueTranId=id.PIndentIssueTranId left outer join
                          PurchaseReceiveMaster ppm On ppm.PurchaseReceiveId=ppt.PurchaseReceiveId Where om.orderid=" + orderid + @" and op.processid=1      
                          GROUP BY om.localorder, Om.CustomerOrderNo,om.orderid,op.Date,op.FinalDate,op.Appdate,op.currentdate";
            SqlDataAdapter sda1 = new SqlDataAdapter(str2, con);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            ds.Tables.Add(dt1);
            string str3 = @"select CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as Desciption,em.empname,im.indentid,im.indentno,replace(convert(varchar(11),im.date,106),' ','-') as orderdate,
            replace(convert(varchar(11),reqdate,106),' ','-') as reqdate ,Isnull(sum(Quantity),0) as orderqty,replace(convert(varchar(11),'01-Jan-2013',106),' ','-') as frdate,replace(convert(varchar(11),'01-Feb-2013',106),' ','-') as Todate,
            reqdate as reqdt,om.localorder+'/'+Om.CustomerOrderNo as orderno,om.orderid,0 as Recqty,'' as rdate ,'' recdate,vd.item_finished_id as finishedid
            From IndentMaster im inner join IndentDetail id ON im.IndentID=id.IndentID inner join Empinfo em ON em.empid=im.partyid inner join 
            V_FinishedItemDetail vd On id.OFinishedId=vd.item_finished_id inner join
            Ordermaster om ON om.orderid=id.orderid where im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And  om.orderid="+orderid+@"
            group by CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,em.empname,im.indentid,im.indentno,im.date,reqdate,om.localorder,Om.CustomerOrderNo,om.orderid,vd.item_finished_id
            union 
            select CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as Desciption,em.empname,im.indentid,im.indentno,replace(convert(varchar(11),im.date,106),' ','-') as orderdate,
            replace(convert(varchar(11),reqdate,106),' ','-') as reqdate ,0 as orderqty,replace(convert(varchar(11),'01-Jan-2013',106),' ','-') as frdate,replace(convert(varchar(11),'01-Feb-2013',106),' ','-') as Todate,
            reqdate as reqdt,om.localorder+'/'+Om.CustomerOrderNo as orderno,om.orderid, sum(RecQuantity) Recqty ,ppm.Date as rdate,replace(convert(varchar(11),ppm.Date,106),' ','-') as recdate,vd.item_finished_id as finishedid
            from PP_ProcessRecMaster ppm inner join PP_ProcessRecTran ppt On ppm.PRMid=ppt.PRMid inner join 
            V_FinishedItemDetail vd On vd.item_finished_id=ppt.Finishedid  inner join IndentMaster im On im.indentid=ppt.indentid 
            inner join IndentDetail id ON im.IndentID=id.IndentID and id.OFinishedId=vd.item_finished_id inner join
            Empinfo em ON em.empid=ppm.Empid inner join
            Ordermaster om ON om.orderid=id.orderid
            where vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " ANd om.orderid="+orderid+@"
            group by CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName,em.empname,im.indentid,im.indentno,
            om.localorder,Om.CustomerOrderNo,om.orderid,ppm.Date,reqdate,im.date,vd.item_finished_id";
            SqlDataAdapter sda2 = new SqlDataAdapter(str3, con);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            ds.Tables.Add(dt2);
            // Purchase Detail
            string str4 = @"select CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as Desciption,em.empname,im.Challanno,replace(convert(varchar(11),im.Date,106),' ','-') as orderdate,
             replace(convert(varchar(11),im.DueDate,106),' ','-') as reqdate ,isnull((quantity),0)as orderqty,isnull(sum(QTY),0)as Recqty,
             replace(convert(varchar(11),ppm.ReceiveDate,106),' ','-') as recdate,ppm.ReceiveDate as rdate,im.DueDate as reqdt,om.localorder+'/'+Om.CustomerOrderNo as orderno,om.orderid,vd.item_finished_id
             From PurchaseIndentIssue im inner join PurchaseIndentIssueTran id ON im.PindentIssueid=id.PindentIssueid inner join
             Empinfo em ON em.empid=im.Partyid inner join V_FinishedItemDetail vd On id.Finishedid=vd.item_finished_id inner join
             Ordermaster om ON om.orderid=im.Orderid left outer join PurchaseReceiveDetail ppt On im.PIndentIssueId=ppt.PIndentIssueId and ppt.FinishedId=id.Finishedid left outer join
             PurchaseReceiveMaster ppm On ppm.PurchaseReceiveId=ppt.PurchaseReceiveId 
             Where vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And om.orderid=" + orderid + @" 
             Group by im.Challanno,im.Date,im.DueDate,em.empname,ppm.ReceiveDate,om.localorder,Om.CustomerOrderNo,om.orderid,vd.item_finished_id,CATEGORY_NAME,ITEM_NAME,QualityName,designName,
             ColorName,ShadeColorName,ShapeName,quantity
             Having isnull((quantity),0)>0";
            SqlDataAdapter sda3 = new SqlDataAdapter(str4, con);
            DataTable dt3 = new DataTable();
            sda3.Fill(dt3);
            ds.Tables.Add(dt3);
            //Purchase Summary
            string str5 = @"select CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' ' +ShapeName as Description,
                           (select sum(quantity)from PurchaseIndentIssueTran a where Finishedid=vd.ITEM_FINISHED_ID and Pindentissuetranid=pt.Pindentissuetranid and pm.orderid=" + orderid + @" )as orderqty,sum(QTY) as RecQty 
                           from PurchaseIndentIssue pm inner join PurchaseIndentIssueTran pt on pm.PindentIssueid=pt.PindentIssueid inner join 
                           dbo.V_FinishedItemDetail vd On vd.ITEM_FINISHED_ID=pt.Finishedid left outer join
                           dbo.PurchaseReceiveDetail prd On prd.PIndentIssueId=pm.PIndentIssueId and prd.Pindentissuetranid=pt.Pindentissuetranid and prd.FinishedId=pt.Finishedid
                           where pm.orderid=" + orderid + " And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " group by CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName, vd.ITEM_FINISHED_ID,pm.orderid,pt.Pindentissuetranid";
            SqlDataAdapter sda4 = new SqlDataAdapter(str5, con);
            DataTable dt4 = new DataTable();
            sda4.Fill(dt4);
            ds.Tables.Add(dt4);
            //Production Summary
            string str6 = @"select omt.localorder+'/'+omt.CustomerOrderNo orderno, Item_Name+'  '+QualityName+'  '+Designname+'  '+ColorName+'  '+ShadeColorName As Description,(select isnull(sum(Quantity),0) from IndentDetail where orderid=omt.orderid ) As orderqty,
                           isnull(sum(recquantity),0) as Recqty,omt.orderid
                           From IndentMaster OM inner join IndentDetail OD On OM.IndentId=OD.IndentId inner join
                           ordermaster omt on omt.orderid=od.orderid inner join
                           V_FinishedItemDetail V on V.Item_Finished_Id=OD.OFinishedId left outer join
                           PP_ProcessRecTran pt on pt.indentid=OD.IndentId and pt.finishedid=OD.OFinishedId
                           where omt.orderid=" + orderid + " And V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Group by omt.localorder,Item_Name,QualityName,Designname,ColorName,ShadeColorName,SizeMtr,SizeFt ,OD.OFinishedId,omt.CustomerOrderNo,omt.orderid";
            SqlDataAdapter sda5 = new SqlDataAdapter(str6, con);
            DataTable dt5 = new DataTable();
            sda5.Fill(dt5);
            ds.Tables.Add(dt5);
            string str7 = @"select CATEGORY_NAME+'  '+ITEM_NAME+'  '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName as Description ,Qty from OrderLocalConsumption oc,V_FinishedItemDetail v where oc.Finishedid=v.ITEM_FINISHED_ID and orderid=" + orderid + " And v.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "";
            SqlDataAdapter sda6 = new SqlDataAdapter(str7, con);
            DataTable dt6 = new DataTable();
            sda6.Fill(dt6);
            ds.Tables.Add(dt6);
            string str8 = @"Select 1  seq,'Garment Order/Order Change Status' as Name,orderid,Remarks as garmentorder from ordermaster where orderid=" + orderid + @" Union
                           Select 2 seq,'Fabric Capture Form' as Name,orderid,Remark as localconsumption from OrderLocalConsumption where orderid=" + orderid + @" Union
                           Select 3 seq,'Order Planning Form- Purchase' as Name,orderid,planRemark as purchasePlan from OrderProcessPlanning where processid=1 and orderid=" + orderid + @" Union
                           Select 4 seq,'Order Planning Form- Production' as Name,orderid,planRemark as processplan from OrderProcessPlanning where processid=2 and orderid=" + orderid + @" Union
                           Select 5 seq,'Order Department Planning Form- Purchase' as Name,orderid,DepRemark as purchasedept from OrderProcessPlanning where processid=1 and orderid=" + orderid + @" Union
                           Select 6 seq,'Order Department Planning Form- Production' as Name,orderid,DepRemark as processdep from OrderProcessPlanning where processid=2 and orderid=" + orderid + @" Union
                           Select 7 seq,'Purchase Order Form' as Name,PindentIssueid as porder,Remarks as purchaseRemark from PurchaseIndentIssue where orderid=" + orderid + @" Union
                           Select 8 seq,'Purchase Receive Form' as Name,PM.PurchaseReceiveId as precorder,MRemark as purchaserec from PurchaseReceiveMaster pm inner join 
                           PurchaseReceivedetail pd On pm.PurchaseReceiveId=pd.PurchaseReceiveId Inner join PurchaseIndentIssue pii On pii.PindentIssueid=pd.PindentIssueid Where pii.orderid=" + orderid + @" Union
                           Select 9 seq,'JW Planning for Garments Form' as Name,IM.IndentID as jw,GRemark as Gremark from indentmaster im inner join indentdetail id On im.indentid=id.indentid where id.orderid=" + orderid + @" Union
                           Select 10 seq,'Indent Raw Issue form' as Name,PM.PRMid AS pRODORDER,RiRemark as PRemark from PP_ProcessRawMaster pm Inner join PP_ProcessRawtran pd On pm.PRMid=pd.PRMid inner join indentdetail id On pd.indentid=id.indentid  where id.orderid=" + orderid + @" Union
                           Select 11 seq,'Indent Raw Receive Form' as Name,PM.PRMid AS PRODRECORDER, RRRemark PRRECIVEremark from PP_ProcessRecMaster pm inner join PP_ProcessRectran pd On pm.PRMid=pd.PRMid Inner join indentdetail id On pd.indentid=id.indentid  where id.orderid=" + orderid + @" ";
            SqlDataAdapter sda7 = new SqlDataAdapter(str8, con);
            DataTable dt7 = new DataTable();
            sda7.Fill(dt7);
            ds.Tables.Add(dt7);
            string str9 = @"select om.orderid, om.localorder+'/'+customerorderno as OrderNo,IndentNo,empname,sum(IssueQuantity) as issueQty ,CATEGORY_NAME +'  '+ITEM_NAME+'  '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName as description,replace(convert(varchar(11),ppm.Date,106),' ','-') as issuedate
                          From V_INDENTDETAIL ID INNER JOIN PP_ProcessRawtran PRT On id.indentid=prt.indentid inner join 
                          ordermaster om On id.orderid=om.orderid inner join 
                          V_FinishedItemDetail vd On vd.item_finished_id=prt.Finishedid inner join PP_ProcessRawMaster ppm On ppm.PRMid=prt.PRMid
                          where om.orderid=" + orderid + " And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                           Group by om.localorder,customerorderno,IndentNo,empname,om.orderid,vd.item_finished_id,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,ppm.Date
                           Order by orderid";
            SqlDataAdapter sda8 = new SqlDataAdapter(str9, con);
            DataTable dt8 = new DataTable();
            sda8.Fill(dt8);
            ds.Tables.Add(dt8);
            Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseOrderDKl.xsd";
        }
        else if (Jwdp == "DP")
        {
            string str2 = @"select replace(convert(varchar(11),max(AssignDate),106),' ','-') as orderdate,replace(convert(varchar(11),max(prm.ReceiveDate),106),' ','-') as RecDate,isnull(sum(pd.Qty),0) as dpqty,pd.orderid,isnull(sum(prd.Qty),0) as dprec
                         From PROCESS_ISSUE_MASTER_9 pi Inner join 
                         PROCESS_ISSUE_DETAIL_9 pd On pi.IssueOrderId=pd.IssueOrderId left outer join
                         PROCESS_RECEIVE_DETAIL_9 prd On prd.IssueOrderId=pi.IssueOrderId and pi.IssueOrderId=prd.IssueOrderId and pd.Issue_Detail_Id=prd.Issue_Detail_Id and pd.orderid=prd.orderid left outer join
                         PROCESS_RECEIVE_MASTER_9 prm On prm.Process_Rec_Id=prd.Process_Rec_Id and prd.orderid=pd.orderid
                         Where pd.orderid=" + orderid + " Group by pd.orderid";
            SqlDataAdapter sda1 = new SqlDataAdapter(str2, con);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            ds.Tables.Add(dt1);

            string str3 = @"Select CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as Desciption, em.empname,im.IssueOrderId,replace(convert(varchar(11),im.AssignDate,106),' ','-') as orderdate,
                          replace(convert(varchar(11),id.ReqByDate,106),' ','-') as reqdate ,isnull(sum(id.Qty),0)as orderqty,isnull(sum(ppt.Qty),0)as Recqty,
                          replace(convert(varchar(11),ppm.ReceiveDate,106),' ','-') as recdate,ppm.ReceiveDate as rdate,id.ReqByDate as reqdt,om.localorder+'/'+Om.CustomerOrderNo as orderno,om.orderid,vd.item_finished_id
                          From PROCESS_ISSUE_MASTER_9 im inner join PROCESS_ISSUE_DETAIL_9 id ON im.IssueOrderId=id.IssueOrderId inner join
                          Empinfo em ON em.empid=im.Empid inner join V_FinishedItemDetail vd On id.Item_Finished_Id=vd.item_finished_id inner join
                          Ordermaster om ON om.orderid=id.Orderid left outer join PROCESS_RECEIVE_DETAIL_9 ppt On im.IssueOrderId=ppt.IssueOrderId and ppt.Item_Finished_Id=id.Item_Finished_Id and id.orderid=ppt.orderid left outer join
                          PROCESS_RECEIVE_MASTER_9 ppm On ppm.Process_Rec_Id=ppt.Process_Rec_Id 
                          Where om.orderid=" + orderid + " And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Group by im.IssueOrderId,im.AssignDate,id.ReqByDate,em.empname,ppm.ReceiveDate,om.localorder,Om.CustomerOrderNo,om.orderid,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,vd.item_finished_id";
            SqlDataAdapter sda2 = new SqlDataAdapter(str3, con);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            ds.Tables.Add(dt2);
            string str4 = @"select CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as Desciption,isnull(sum(pd.Qty),0) as dpqty,pd.orderid,isnull(sum(prd.Qty),0) as dprec
                         From PROCESS_ISSUE_MASTER_9 pi Inner join
                         PROCESS_ISSUE_DETAIL_9 pd On pi.IssueOrderId=pd.IssueOrderId Inner join 
                         V_FinishedItemDetail v On v.Item_Finished_Id=pd.Item_Finished_Id left outer join
                         PROCESS_RECEIVE_DETAIL_9 prd On prd.IssueOrderId=pi.IssueOrderId and pi.IssueOrderId=prd.IssueOrderId and pd.Issue_Detail_Id=prd.Issue_Detail_Id and pd.orderid=prd.orderid and prd.Item_Finished_Id=v.Item_Finished_Id left outer join
                         PROCESS_RECEIVE_MASTER_9 prm On prm.Process_Rec_Id=prd.Process_Rec_Id and prd.orderid=pd.orderid  
                         Where pd.orderid=" + orderid + " And v.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Group by pd.orderid,prd.Item_Finished_Id,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName ";
            SqlDataAdapter sda3 = new SqlDataAdapter(str4, con);
            DataTable dt3 = new DataTable();
            sda3.Fill(dt3);
            ds.Tables.Add(dt3);
            string str5 = @"Select 1  seq,'Garment Order/Order Change Status' as Name,orderid,Remarks as garmentorder from ordermaster where orderid=" + orderid + @" Union
                        Select 2 Seq,'DP Order Form'as Name,pim.IssueOrderId,remarks as remark from PROCESS_ISSUE_MASTER_9 pim Inner join PROCESS_ISSUE_Detail_9 pid On pim.IssueOrderId=pid.IssueOrderId where pid.orderid=" + orderid + @" Union
                        Select 3 Seq,'DP Receive Form'as Name,pim.Process_Rec_Id,remarks as remark from PROCESS_RECEIVE_MASTER_9 pim Inner join PROCESS_RECEIVE_detail_9 pid On pim.Process_Rec_Id=pid.Process_Rec_Id where pid.orderid=" + orderid + "";
            SqlDataAdapter sda4 = new SqlDataAdapter(str5, con);
            DataTable dt4 = new DataTable();
            sda4.Fill(dt4);
            ds.Tables.Add(dt4);
            Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseOrderDPDKl.xsd";
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
    }
    protected void RdVendorwise_CheckedChanged(object sender, EventArgs e)
    {
        Trcomp.Visible = true;
        trcustomer.Visible = false;
        trsupply.Visible = true;
        trfr.Visible = true;
        LBLFRDATE.Text = "From Date";
        trto.Visible = true;
        btnExcelExport.Visible = true;
        BtnExport.Visible = true;
        pstatus.Visible = false;
        pindent.Visible = false;
        dpstatus.Visible = false;
        trgvdetail.Visible = false;
        trpptype.Visible = true;
        btnsybmit.Visible = true;
        btnpreview.Visible = true;
        BtnExport.Visible = true;
        btnExcelExport.Visible = true;
        btnPOExport.Visible = false;
        pstatus.Visible = false;
        trpusaseindent.Visible = false;
    }
    public string getvendor(string strval, string strval1)
    {
        string val = "-";
        if (strval == "DP")
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select e.empname from PROCESS_ISSUE_MASTER_9 pi inner join PROCESS_ISSUE_Detail_9 pd On  pi.IssueOrderId=pd.IssueOrderId inner join empinfo e On e.empid=pi.empid where e.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And  orderid="+strval1+" ");
            if (ds.Tables[0].Rows.Count > 0)
            {
                val=ds.Tables[0].Rows[0][0].ToString();
            }
        }
        return val;
    }
    public string getprodstatus(string strval)
    {
        string val = "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Distinct status,orderid from indentmaster im inner join indentdetail id On im.indentid=id.indentid where im.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And orderid="+strval+"");
        if (ds.Tables[0].Rows.Count > 0)
        {
            val=ds.Tables[0].Rows[0][0].ToString();
        }
        return val;
    }
    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
        //confirms that an HtmlForm control is rendered for the
        //specified ASP.NET server control at run time.
    }
    
    protected void btnPOExport_Click(object sender, EventArgs e)
    {
        if (ddOrderno.SelectedIndex > 0 && ddpurchaseint.SelectedIndex>0)
        {
            string str = @"SELECT Pindentissuetranid as ID,Category_Name +'  '+VF.ITEM_NAME+'  '+QualityName+'  '+DesignName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName+'  '+
            CASE WHEN OD.Unitid=1 Then SizeMtr Else SizeFt End  Description,U1.Unitname As Unit,quantity as Qty,ol.thanlength,ol.remark ,E.empname
            FROM PurchaseIndentIssue op Inner join PurchaseIndentIssueTran OD On op.PindentIssueid=od.PindentIssueid Inner JOIN 
            OrderLocalConsumption ol On ol.OrderId=op.OrderId and od.Finishedid=ol.Finishedid inner join EmpInfo e On E.empid=op.Partyid inner join
            V_FinishedItemDetail VF ON OD.FinishedId=VF.Item_Finished_Id And vf.MasterCompanyId="+Session["varcompanyno"]+@"
            INNER JOIN ITEM_PARAMETER_MASTER IPM ON OD.FinishedId=IPM.Item_Finished_Id INNER JOIN 
            Unit U On OD.Unitid=U.UnitId inner Join Unit U1 on OD.UnitId=U1.UnitId  
            Where op.OrderId=" + ddOrderno.SelectedValue + " and op.pindentissueid="+ddpurchaseint.SelectedValue+"";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DGOrderDetail3.DataSource = ds;
                DGOrderDetail3.DataBind();
            }
            string str1 = @"select Distinct om.orderid,om.localorder,vf.CATEGORY_NAME,om.customerorderNO,Replace(convert(varchar(11),OrderDate,106),' ','-') as OrderDate ,Replace(convert(varchar(11),om.DispatchDate,106),' ','-') AS SRC,Replace(convert(varchar(11),DueDate,106),' ','-') as StoreDElDAte,(select  case when Date is not null then Replace(convert(varchar(11),Date,106),' ','-') else ' ' end from OrderProcessPlanning where orderid=om.orderid and processid=1) as DelDate 
            from ordermaster om inner join 
            orderdetail od On om.orderid=od.orderid inner join V_FinishedItemDetail VF On OD.Item_Finished_Id=VF.Item_Finished_Id And Vf.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
            Where om.orderid=" + ddOrderno.SelectedValue + "";
            SqlDataAdapter sda = new SqlDataAdapter(str1, ErpGlobal.DBCONNECTIONSTRING);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ds.Tables.Add(dt);
            if (ds.Tables[1].Rows.Count > 0)
            {
                DGConsumption.Style.Add("font-size", "1em");
                Response.Clear();
                string attachment = "attachment; filename=Internal PO Sheet.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.IO.StringWriter stringWrite = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                DGOrderDetail3.RenderControl(htmlWrite);
                Response.Write(@"<TABLE><tr><td align=Center style=font-weight:bold;background-color:Silver; colspan=6>Order No:  " + ds.Tables[1].Rows[0]["CATEGORY_NAME"].ToString() + " /  " + ds.Tables[1].Rows[0]["localorder"].ToString() + " / " + ds.Tables[0].Rows[0]["empname"].ToString() +" / "+ddpurchaseint.SelectedValue+ @"</td></tr>
                <tr><td align=Center style=font-weight:bold;background-color:Silver; colspan=6>FABINDIA PURCHASE ORDER SHEET GARMENT FABRIC</td></tr><TR><TD></TD></TR>
                <tr><td align=Center style=font-weight:bold;background-color:Silver;width:75px>SOURCE</td><TD></TD><td align=Center style=font-weight:bold;background-color:Silver;>FABRIC DETAILS</td></tr>
                <tr><td style=font-weight:bold;>SR No.</td><td align=left>" + ds.Tables[1].Rows[0]["localorder"].ToString() + @"</td><td style=font-weight:bold;>FABRIC NAME</td><td align=left></td></tr>
                <tr><td style=font-weight:bold;>SRC</td><td></td><td style=font-weight:bold;>LOOM :</td><td align=left></td></tr>
                <tr><td style=font-weight:bold;>Category</td><td align=left>" + ds.Tables[1].Rows[0]["CATEGORY_NAME"].ToString() + @"</td><td style=font-weight:bold;>COMPOSITION %</td><td align=left></td></tr>
                <tr><td style=font-weight:bold;>SUPPLIER :</td><td align=left>" + ds.Tables[0].Rows[0]["empname"].ToString() + @"</td><td style=font-weight:bold;>WARP x WEFT (YARN COUNT) :</td><td align=left></td></tr>
                <tr><td align=Center style=font-weight:bold;background-color:Silver;>TIME LINES</td><TD></TD><td style=font-weight:bold;>REED x PICK :</td><td align=left></td></tr>
                <tr><td style=font-weight:bold;>FAB ODER DATE</td><td align=left>" + ds.Tables[1].Rows[0]["OrderDate"].ToString() + @"</td><td style=font-weight:bold;>WEIGHT(SPECIFY IF GSM OR GLM) :</td><td align=left></td><td style=font-weight:bold;>NO OF COLORS</td><td align=left></td></tr>
                <tr><td style=font-weight:bold;>Fab SRC</td><td align=left>" + ds.Tables[1].Rows[0]["SRC"].ToString() + @"</td><td style=font-weight:bold;>FINISHED WIDTH</td><td align=left></td></tr>
                <tr><td style=font-weight:bold;background-color:Silver;>Fab Cost</td><td align=left></td><td style=font-weight:bold;>FABRIC FINISHING :</td><td align=left></td></tr>
                <tr><td style=font-weight:bold;>Fab Del Date</td><td align=left>" + ds.Tables[1].Rows[0]["DelDate"] + @"</td><td style=font-weight:bold;>PRINT :</td><td align=left></td></tr>
                <tr><td style=font-weight:bold;>Stock at Store</td><td align=left>" + ds.Tables[1].Rows[0]["StoreDElDAte"].ToString() + @"</td><td style=font-weight:bold;>DYE :</td><td align=left></td></tr>
                <tr><td style=font-weight:bold;>CANCELLATION DATE</td><td></td><td align=Center style=font-weight:bold;background-color:Silver;>PRODUCT DETAILS</td></tr>
                <tr><td align=Center style=font-weight:bold;background-color:Silver;>PURCHASE PATTERN</td><TD></TD><td style=font-weight:bold;>THAAN LENGTH</td><td align=left></td></tr>
                <tr><td style=font-weight:bold;>DP/JW</td><TD></TD><td align=Center style=font-weight:bold;background-color:Silver;>QUALITY INSTRUCTIONS</td></tr>
                <tr><td align=Center style=font-weight:bold;background-color:Silver;>DETAILS FROM PSC</td><TD></TD><td style=font-weight:bold;>COLOLRFASTNESS TO WASHING/DRYCLEANING</td><td align=left></td></tr>
                <tr><td style=font-weight:bold;>PO/PROJECTION NO</td><td></td><td style=font-weight:bold;>RUBBING FASTNESS- WET</td><td align=left></td></tr>
                <tr><td style=font-weight:bold;>GARMENT PO NO</td><td align=left>" + ds.Tables[1].Rows[0]["customerorderNO"].ToString() + @"</td><td style=font-weight:bold;>RUBBING FASTNESS- DRY</td><td align=left></td></tr>
                <tr><td style=font-weight:bold;>GARMENT DETAILS</td><td align=left>" + ds.Tables[1].Rows[0]["CATEGORY_NAME"].ToString() + @"</td><td style=font-weight:bold;>RUBBING MAX SHRINKAGE ALLOWED- WARP</td><td align=left></td></tr>
                <tr><td style=font-weight:bold;>Garment Description</td><td></td><td style=font-weight:bold;>MAX SHRINKAGE ALLOWED-WEFT</td><td align=left></td></tr>
                <tr><td style=font-weight:bold;>PROMOTION/SEASON</td><td></td><td></td></tr>
                <tr><td style=font-weight:bold;>AOM NUMBER</td><td></td><td></td></tr>
                <tr></tr><tr></tr></table><table><tr><td>" + stringWrite.ToString() + "</td></tr><table><table><tr><td align=right colspan=2 style=font-weight:bold;>Total</td><td>" + ds.Tables[0].Compute("sum(Qty)", " ").ToString() + @"</td></tr><tr></tr>
                <tr></tr>
                <tr><td align=left colspan=3 style=font-weight:bold;>* PRICES ARE INCLUSIVE OF FREIGHT TO THE SRC AND ANY INCIDENTAL TAXES</td></tr>
                <tr><td colspan=3>INSTRUCTIONS</td></tr><tr><td colspan=3> 1. COLORFASTENSS TO DRYCLEANING IS ACCEPTABLE FOR FABRICS CONTAINING SILK.</td></tr>
                <tr><td colspan=6>2. THE PRODUCT SHOULD HAVE SUFFICIENT STRENGTH FOR SERVING ITS PURPOSE OF WEARING. IT INCLUDES TENSILE STRENGTH, TEAR STRENGTH AND SHEAR STRENGTH, AMONG OTHERS.</td></tr>
                <tr><td colspan=6>3. IF ANY RETURNS COME FROM STORES ON ACCOUNT OF QUALITY, SUPPLIER WILL BE LIABLE TO PAY FOR THE MRP+OTHER INCIDENTAL CHARGES WHICH MAY INCLUDE COMPLETE PRODUCT RECALL AND PENTALY FOR LOST OPPORTUNITY.</td></tr>
                <tr><td colspan=6>4. PRE PRODUCTION APPROVAL IS MANDAGORY FOR QUALITY/DESIGN/COLORWAY</td></tr>
                <tr><td colspan=6>5. NO SYNTHETIC FIBERS LIKE POLYESTER, NYLON TO BE USED IN THE BASE FABRIC, UNTIL SPECIFIED EXPLICITLY.</td></tr>
                <tr><td colspan=3>6. KINDLY SUBMIT 1 MTR. OF EACH DESIGN FOR COLOUR APPROVAL ALONG WITH TEXANLAB REPORT OF ANY ONE DESIGN FOR QUALITY PARAMETERS </td></tr>
                <tr><td colspan=6>7. PLEASE NOTE:- IF ANY DELAY IN PRODUCTION VENDOR NEEDS TO INTIMATE US WELL IN ADVANCE (AT LEAST 10DAYS). IF ANY DELAY IN DELIVERY WILL ATTRACT THE PENALTY OF 1% PER DAY BASIS DELIVERY DATE AT THE SRC</td></tr>
                <tr><td colspan=6>8. THERE SHOULD BE NO WEAVING DEFECTS IN THE FABRIC</td></tr></table>");
                Response.End();
            }
            //<tr align=center style=background-color:Silver;><td style=font-weight:bold;># PLEASE FILL THE INFORMATION IN UPPER CASE ONLY…</td></tr>
            //<tr align=center style=background-color:Silver;><td style=font-weight:bold;>*DO CHECK MANDATORY DIRECTIVES BELOW AND  INSTRUCTIONS IN CONTINUATION SHEETS…</td></tr>
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Please Select Order No..');", true);
        }
    }
    protected void Rdpurchaseorder_CheckedChanged(object sender, EventArgs e)
    {
        Trcomp.Visible = true;
        trcustomer.Visible = false;
        trsupply.Visible = false;
        trfr.Visible = false;
        LBLFRDATE.Text = "From Date";
        trto.Visible = false;
        btnExcelExport.Visible = true;
        BtnExport.Visible = true;
        pstatus.Visible = false;
        pindent.Visible = false;
        dpstatus.Visible = false;
        trgvdetail.Visible = false;
        trpptype.Visible = false;
        btnsybmit.Visible = false;
        btnpreview.Visible = false;
        BtnExport.Visible = false;
        btnExcelExport.Visible = false;
        btnPOExport.Visible = true;
        pstatus.Visible = true;
        trpusaseindent.Visible = true;
        UtilityModule.ConditionalComboFill(ref ddOrderno, @"Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo 
        from OrderMaster OM,Jobassigns JA,V_FinishedItemDetail vd,orderdetail od,PurchaseIndentIssue p
        Where om.status=0 and od.orderid=om.orderid and vd.ITEM_FINISHED_ID=od.Item_Finished_Id and p.orderid=om.orderid and OM.Orderid=JA.Orderid And vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "-Select Order No-");
    }
   
}