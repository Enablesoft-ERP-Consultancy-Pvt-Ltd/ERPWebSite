using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ClosedXML.Excel;
using System.IO;
public partial class Masters_Purchase_Purchase_Matarial_report1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        string Qry=string.Empty;
        if (!IsPostBack)
        {
            if (Session["varCompanyId"].ToString() == "6")
            {
                RDpurchasedetail.Visible = false;
            }
            if (Session["varCompanyId"].ToString() == "44")
            {
                Qry = @"Select Distinct EmpId,EmpName From Empinfo EI,PurchaseIndentIssue PII Where EI.EmpId=Partyid  And EI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By EmpName
            select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.USERID=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order by Companyname            
            select distinct ci.customerid,ci.Customercode From customerinfo ci 
            Inner join OrderMaster om on om.customerid=ci.customerid And ci.MasterCompanyId=" + Session["varCompanyId"] + @"
            select GoDownID,GodownName from GodownMaster  order by GodownName";
            }
            else {

                Qry = @"Select Distinct EmpId,EmpName From Empinfo EI,PurchaseIndentIssue PII Where EI.EmpId=Partyid  And EI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By EmpName
            select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.USERID=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order by Companyname            
            select distinct ci.customerid,ci.Customercode+'/'+CompanyName From customerinfo ci 
            Inner join OrderMaster om on om.customerid=ci.customerid And ci.MasterCompanyId=" + Session["varCompanyId"] + @"
            select GoDownID,GodownName from GodownMaster  order by GodownName";
            
            
            }
            //Inner join Jobassigns JA ON OM.Orderid=JA.Orderid";
            DataSet ds = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref dsuppl, ds, 0, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 1, true, "Select Comp Name");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddcustomer, ds, 2, true, "Select CustomerCode");
            UtilityModule.ConditionalComboFillWithDS(ref DDgodown, ds, 3, true, "--Select godown--");
            //ddCompName.SelectedIndex = 1;
            TxtFRDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtTODate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtAsOnDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            rdrawmaterial.Checked = true;
            RawmaterialdetailCheckedChange();
            switch (Session["varcompanyid"].ToString())
            {
                case "12":
                    rdrawmaterial.Visible = false;
                    rdrawmaterial.Checked = false;
                    break;
                case "9":
                    RDPurchaseMaterialReceive.Visible = true;
                    RDPurchaseMaterialIssueReceive.Visible = true;
                    break;
                case "16":
                    rdrawmaterial.Visible = false;
                    RDindent.Visible = false;
                    RdSupply.Visible = false;
                    RDDebitNote.Visible = false;
                    RDPurchaseMaterialReceive.Visible = false;
                    rdrawmaterial.Checked = false;
                    RDpurdelivRpt.Checked = true;
                    RDPurchaseMaterialRecPending.Visible = true;
                    RDPurchaseOrderReceiveBuyerCode.Visible = true;
                    RDCustomerOrderWisePODetail.Visible = true;
                    break;
                case "28":
                    rdrawmaterial.Visible = false;
                    RDindent.Visible = false;
                    RdSupply.Visible = false;
                    RDDebitNote.Visible = false;
                    RDPurchaseMaterialReceive.Visible = false;
                    rdrawmaterial.Checked = false;
                    RDpurdelivRpt.Checked = true;
                    RDPurchaseMaterialRecPending.Visible = true;
                    RDPurchaseOrderReceiveBuyerCode.Visible = true;
                    RDCustomerOrderWisePODetail.Visible = true;
                    break;
                case "44":
                    rdrawmaterial.Visible = false;
                    RDindent.Visible = false;
                    RdSupply.Visible = false;
                    RDDebitNote.Visible = false;
                    RDPurchaseMaterialReceive.Visible = false;
                    rdrawmaterial.Checked = false;
                    RDpurdelivRpt.Checked = false;
                    RDPurchaseMaterialRecPending.Visible = false;
                    RDPurchaseOrderReceiveBuyerCode.Visible = false;
                    RDCustomerOrderWisePODetail.Visible = false;
                    RDSupplyorder.Visible = true;
                    RDPurchaseReceive.Visible = true;
                    RDpurdelivRpt.Visible = false;
                    RDpurchasedetail.Visible = false;
                    break;
                //case "38":
                //    rdrawmaterial.Visible = false;
                //    RDindent.Visible = false;
                //    RdSupply.Visible = false;
                //    RDDebitNote.Visible = false;
                //    break;
            }
        }
        if (RDSupplyorder.Checked == true)
        {
            TDExcelExport.Visible = true;
        }
        else
        {
            TDExcelExport.Visible = false;

            if (Session["VarCompanyNo"].ToString() != "22")
            {
                chkexcelexport.Checked = false;
            }
            
        }
    }
    protected void ddcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RDSupplyorder.Checked == true)
        {
            if (Session["varcompanyid"].ToString() == "44")
            {
                UtilityModule.ConditionalComboFill(ref ddOrderno, "Select Distinct OM.OrderId,CustomerOrderNo+ ' / ' +LocalOrder from OrderMaster OM Where OM.Status=0 And CustomerId=" + ddcustomer.SelectedValue + " And om.CompanyId=" + ddCompName.SelectedValue, true, "Select OrderNo.");

            }
            else
            {
                UtilityModule.ConditionalComboFill(ref ddOrderno, "select Distinct OM.OrderId,CustomerOrderNo+ ' / ' +LocalOrder from purchaseindentissue  PII inner join Ordermaster OM on PII.orderid=Om.orderid where OM.customerid=" + ddcustomer.SelectedValue + " and OM.CompanyId=" + ddCompName.SelectedValue + "", true, "Select OrderNo.");
            }
        }
        else
        {
            //UtilityModule.ConditionalComboFill(ref ddOrderno, "Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA Where OM.Orderid=JA.Orderid and OM.Status=0 And CustomerId=" + ddcustomer.SelectedValue + " And om.CompanyId=" + ddCompName.SelectedValue, true, "Select OrderNo.");
            UtilityModule.ConditionalComboFill(ref ddOrderno, "Select Distinct OM.OrderId,CustomerOrderNo+ ' / ' +LocalOrder from OrderMaster OM Where OM.Status=0 And CustomerId=" + ddcustomer.SelectedValue + " And om.CompanyId=" + ddCompName.SelectedValue, true, "Select OrderNo.");
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string qry = "";
        DataSet ds = new DataSet();
        if (RDpurdelivRpt.Checked == true)
        {
            if (Session["varcompanyid"].ToString() == "6")
            {
                Session["ReportPath"] = "Reports/rptpurchaseorder_empwise1NEW.rpt";

                qry = @"SELECT vp.empname,vp.item_description,vp.colour,Sum(vp.qty) qty,Sum(vp.recqty) recqty,vp.pindentissueid,vp.deliverydate,vp.recdate,
            vp.qualityname,vp.orderdate,replace(Convert(varchar(11),'" + TxtFRDate.Text + "',106),' ','-') as frdt,replace(Convert(varchar(11),'" + TxtTODate.Text + @"',106),' ','-') as todt,Sum(vp.canqty) canqty,Status,Challanno
            FROM v_purchase_DELIVERYSTATUS vp  where vp.orderdate >= '" + TxtFRDate.Text + "' and vp.orderdate <='" + TxtTODate.Text + "' And vp.MasterCompanyId=" + Session["varCompanyId"];
                if (ddCompName.SelectedIndex > 0)
                {
                    qry = qry + " And vp.CompanyId=" + ddCompName.SelectedValue + "";
                }
                if (dsuppl.SelectedIndex > 0)
                {
                    qry = qry + " And vp.EmpId=" + dsuppl.SelectedValue + "";
                }
                qry = qry + " Group By vp.empname,vp.item_description,vp.colour,vp.pindentissueid,vp.deliverydate,vp.recdate,vp.qualityname,vp.orderdate,Status,Challanno ORDER BY vp.empname";

                Session["dsFileName"] = "~\\ReportSchema\\rptpurchaseorder_empwiseNEW1.xsd";
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            }
            else
            {
                ////For Carpet CompanyWise
                qry = @"select *,case When " + ddCompName.SelectedIndex + ">0 Then '" + ddCompName.SelectedItem.Text + "' Else 'ALL' End CompanyName,'" + TxtFRDate.Text + "' as FromDate,'" + TxtTODate.Text + "' as ToDate from V_PurchaseDeliveryStatusReport Where OrderDate>='" + TxtFRDate.Text + "' And OrderDate<='" + TxtTODate.Text + "' And MasterCompanyId=" + Session["varcompanyId"] + "";
                if (ddCompName.SelectedIndex > 0)
                {
                    qry = qry + " And CompanyId=" + ddCompName.SelectedValue + "";
                }
                if (dsuppl.SelectedIndex > 0)
                {
                    qry = qry + " And EmpId=" + dsuppl.SelectedValue + "";
                }
                Session["ReportPath"] = "Reports/RptPurchaseDeliveryStatus.rpt";
                Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseDeliveryStatus.xsd";
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            }
        }
        else if (RDindent.Checked == true)
        {
            Session["ReportPath"] = "Reports/rptindentregister.rpt";
            qry = @"SELECT vp.empname,vp.item_description,vp.qty,vp.pindentissueid,vp.deliverydate,Om.CustomerOrderNo,om.LocalOrder,vp.qualityname,vp.orderdate,t.termname,p.paymentname,u.username ,replace(Convert(varchar(11),'" + TxtFRDate.Text + "',106),' ','-') as frdt,replace(Convert(varchar(11),'" + TxtTODate.Text + @"',106),' ','-') as todt,vp.rate,Challanno ,LotNo 
                FROM v_purchase_emp_order_wise1 vp left outer JOIN OrderMaster om ON vp.orderid=om.OrderId left outer join term t on t.termid=vp.deliverytermid left outer join payment p on p.paymentid=vp.payementtermid left outer join newuserdetail u on u.userid=vp.userid
                where vp.orderdate >= '" + TxtFRDate.Text + "' and vp.orderdate <='" + TxtTODate.Text + "' And vp.MasterCompanyId=" + Session["varCompanyId"] + "";
            if (ddCatagory.Visible == true && ddCatagory.SelectedIndex > 0)
            {
                qry = qry + " and vp.category_id=" + ddCatagory.SelectedValue + "";
            }
            if (dditemname.Visible == true && dditemname.SelectedIndex > 0)
            {
                qry = qry + " and vp.item_id=" + dditemname.SelectedValue + "";
            }
            if (dquality.Visible == true && dquality.SelectedIndex > 0)
            {
                qry = qry + " and vp.qualityid=" + dquality.SelectedValue + "";
            }
            if (dddesign.Visible == true && dddesign.SelectedIndex > 0)
            {
                qry = qry + " and vp.designid=" + dddesign.SelectedValue + "";
            }
            if (ddcolor.Visible == true && ddcolor.SelectedIndex > 0)
            {
                qry = qry + " and vp.colorid=" + ddcolor.SelectedValue + "";
            }
            if (ddshape.Visible == true && ddshape.SelectedIndex > 0)
            {
                qry = qry + " and vp.shapeid=" + ddshape.SelectedValue + "";
            }
            if (ddsize.Visible == true && ddsize.SelectedIndex > 0)
            {
                qry = qry + " and vp.sizeid=" + ddsize.SelectedValue + "";
            }
            if (ddlshade.Visible == true && ddlshade.SelectedIndex > 0)
            {
                qry = qry + " and vp.shadecolorid=" + ddshape.SelectedValue + "";
            }
            Session["dsFileName"] = "~\\ReportSchema\\rptindentregister.xsd";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        }
        else if (RDpurchasedetail.Checked == true)
        {
            Session["ReportPath"] = "~\\Reports\\RptPurchaseDetailListNew.rpt";
            qry = @"Select *,'" + TxtFRDate.Text + "' FromDate,'" + TxtTODate.Text + "' ToDate From View_Purchase_Challan_Bill_Detail Where CompanyId=" + ddCompName.SelectedValue + " And PHDate>='" + TxtFRDate.Text + "' And PHDate<='" + TxtTODate.Text + "'";

            if (ddcustomer.SelectedIndex > 0)
            {
                qry = qry + " And CustomerId=" + ddcustomer.SelectedValue + "";
            }
            if (ddOrderno.SelectedIndex > 0)
            {
                qry = qry + " And OrderId=" + ddOrderno.SelectedValue;
            }
            if (dsuppl.SelectedIndex > 0)
            {
                qry = qry + " And EmpId=" + dsuppl.SelectedValue;
            }
            Session["dsFileName"] = "~\\ReportSchema\\Rptpurchasedetaillist.xsd";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        }
        else if (rdrawmaterial.Checked == true)
        {
            Session["ReportPath"] = "~\\Reports\\Rptpurchaserawmaterial.rpt";
            if (ddOrderno.SelectedIndex > 0)
            {
                qry = @" Select CI.CustomerName,CI.CompanyName,CI.CustomerCode,OM.OrderId,OM.CustomerId,OM.CompanyId,OM.OrderDate,OM.DispatchDate,OM.CustomerOrderNo,OM.LocalOrder,OM.SailingDate,OM.DueDate,OM.ProdReqDate,
                  OD.Item_Finished_Id,OD.QtyRequired,OD.UnitRate,OD.Amount,OD.CurrencyId,OD.QtyRequired*OD.TotalArea TArea,OD.CancelQty,OD.HoldQty,OD.DispatchDate,OD.TAG_FLAG,OD.OrderCalType,OD.OrderUnitId,
                  VF.CATEGORY_NAME,VF.ITEM_NAME,VF.QualityName,VF.designName,VF.ColorName,VF.ShadeColorName,VF.ShapeName,VF.SizeMtr,VF.SizeFt,U.UnitName
                  From OrderMaster OM,CustomerInfo CI,OrderDetail OD,V_FinishedItemDetail VF,Unit U
                  Where OM.CustomerId=CI.CustomerId And OM.Orderid=OD.Orderid And OD.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID And OD.OrderUnitId=U.UnitId And OM.Orderid=" + ddOrderno.SelectedValue + " And CI.MasterCompanyId=" + Session["varCompanyId"];
                Session["dsFileName"] = "~\\ReportSchema\\RptpurchaseRawmaterial.xsd";
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                //                string str = @"Select VS.Orderid,VS.Category_name,VS.ITEM_NAME,VS.DSC,VS.ShadeColorName,VS.finishedid,VS.masterconsmpQty as mstconsumption,VS.orderconsmpQty as totQty,
                //                isnull(os.AssignQTY,0) as  assign ,dbo.F_PurchaseRate(VS.Finishedid,isnull(Lotno,'')) As rate,Isnull(LotNo,'') As LotNo 
                //                 From V_StockWithFinishedid VS left Outer Join
                //                OrderStockAssign os on os.orderid=VS.orderid and VS.finishedid=os.FinishedID And AssignQty>0 Where VS.orderid=" + ddOrderno.SelectedValue + " And VS.Finishedid=31";
                string str = @"Select VS.Orderid,VS.Category_name,VS.ITEM_NAME,VS.DSC,VS.ShadeColorName,VS.finishedid,0 as mstconsumption,Sum(VS.orderconsmpQty) as totQty,0 As assign,0 As rate,'' LotNo
                             From V_StockWithFinishedid VS Where VS.orderid=" + ddOrderno.SelectedValue + @"
                             group by VS.Orderid,VS.Category_name,VS.ITEM_NAME,VS.DSC,VS.ShadeColorName,VS.finishedid
                             Union
                             Select Orderid,VF.Category_name,VF.ITEM_NAME,VF.QualityName+' '+VF.designName+' '+VF.ColorName+' '+VF.ShapeName+' '+
                             VF.SizeMtr As DSC,VF.ShadeColorName,os.finishedid,0 as mstconsumption,0 as totQty,
                             isnull(Sum(os.AssignQTY),0) as  assign ,dbo.F_PurchaseRate(Finishedid,isnull(Lotno,'')) As rate,Isnull(LotNo,'') As LotNo 
                             From OrderStockAssign os,V_FinishedItemDetail VF Where os.FinishedID=VF.ITEM_FINISHED_ID And orderid=" + ddOrderno.SelectedValue + @" 
                             group by Orderid,VF.Category_name,VF.ITEM_NAME,VF.CATEGORY_NAME,VF.ITEM_NAME,VF.QualityName,VF.designName,VF.ColorName,VF.ShapeName,
                             VF.SizeMtr,VF.ShadeColorName,Finishedid,lotNo";
                SqlDataAdapter sda = new SqlDataAdapter(str, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                ds.Tables.Add(dt);
            }
        }
        else if (RdSupply.Checked == true)
        {
            Session["ReportPath"] = "Reports/RptSupplyLedger.rpt";
            qry = @"select * from SupplierLedger where date >= '" + TxtFRDate.Text + "' and date <='" + TxtTODate.Text + "' And MasterCompanyId=" + Session["varCompanyId"];
            if (ddCompName.SelectedIndex > 0)
            {
                qry = qry + " And CompanyId=" + ddCompName.SelectedValue;
            }
            if (dsuppl.SelectedIndex > 0)
            {
                qry = qry + " And Empid=" + dsuppl.SelectedValue;
            }
            Session["dsFileName"] = "~\\ReportSchema\\RptSupplyLedger.xsd";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        }
        else if (RDSupplyorder.Checked == true)
        {
            if (chkpurchaseordervendorwiselotbillno.Checked == true)
            {
                if (Session["varcompanyid"].ToString() == "44")
                {
                    PurchaseordervendorwiseLotBillDetailagni();
                }
                else
                {

                    PurchaseordervendorwiseLotBillDetail();
                }
                return;
            }
            else if (chkpurchasesumm.Checked == true)
            {
                qry = @"select vs.*,'" + TxtFRDate.Text + "' FromDate,'" + TxtTODate.Text + @"' as ToDate from V_purchasesummary vs inner join v_finisheditemdetail vf on vs.finishedid=vf.item_finished_id WHere OrderDate>='" + TxtFRDate.Text + "'  and Orderdate<='" + TxtTODate.Text + "'";
                if (ddCompName.SelectedIndex > 0)
                {
                    qry = qry + " And vs.companyid=" + ddCompName.SelectedValue;
                }
                if (ddcustomer.SelectedIndex > 0)
                {
                    qry = qry + " And vs.Customerid=" + ddcustomer.SelectedValue;
                }
                if (ddOrderno.SelectedIndex > 0)
                {
                    qry = qry + " And vs.orderid=" + ddOrderno.SelectedValue;
                }
                if (dsuppl.SelectedIndex > 0)
                {
                    qry = qry + " And vs.Empid=" + dsuppl.SelectedValue;
                }
                if (DDPONo.SelectedIndex > 0)
                {
                    qry = qry + " And vs.pindentissueid=" + DDPONo.SelectedValue;
                }
                if (ddCatagory.SelectedIndex > 0)
                {
                    qry = qry + " And vf.CATEGORY_ID=" + ddCatagory.SelectedValue;
                }
                if (dditemname.SelectedIndex > 0)
                {
                    qry = qry + " And vf.item_id=" + dditemname.SelectedValue;
                }
                if (dquality.SelectedIndex > 0)
                {
                    qry = qry + " And vf.qualityid=" + dquality.SelectedValue;
                }
                if (dddesign.SelectedIndex > 0)
                {
                    qry = qry + " And vf.designid=" + dddesign.SelectedValue;
                }
                if (ddcolor.SelectedIndex > 0)
                {
                    qry = qry + " And vf.colorid=" + ddcolor.SelectedValue;
                }
                if (ddshape.SelectedIndex > 0)
                {
                    qry = qry + " And vf.shapeid=" + ddshape.SelectedValue;
                }
                if (ddsize.SelectedIndex > 0)
                {
                    qry = qry + " And vf.sizeid=" + ddsize.SelectedValue;
                }
                if (ddlshade.SelectedIndex > 0)
                {
                    qry = qry + " And vf.shadecolorid=" + ddlshade.SelectedValue;
                }
                qry = qry + "   order by pindentissueid";
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);

                Session["ReportPath"] = "Reports/RptpurchaseVendorSummary.rpt";
                Session["dsFileName"] = "~\\ReportSchema\\RptpurchaseVendorSummary.xsd";
            }
            else
            {

                string filterby = "";

                if (Session["varcompanyid"].ToString() == "27" || Session["varcompanyid"].ToString() == "14")
                {
                    qry = @"SELECT vo.empname,vo.pindentissueid,vo.finishedid,vo.item_description as description,qty as orderqty,vo.orderdate,
                    REPLACE(CONVERT(NVARCHAR(11),vo.recdate,106),' ','-')+'/ '+vo.ReceiveNo as recdate,vo.BILLNO+'/ '+vo.BILLNO1 as recchalan,vo.recqty,
                    '" + TxtFRDate.Text + "' FromDate,'" + TxtTODate.Text + @"' as ToDate,vo.CanQty OrdercanQty,vo.ReturnDate,vo.ReturnChallan,vo.qtyreturn,vo.PO,
                    vo.Localorder,vo.customercode,vo.deliverydate,vo.rate,vo.LshortPercentage,vo.pindentissuetranid,vo.Category_name,vo.Status,Vo.colour,
                    vo.Recqty_beforeRetnqty, IsNull(vo.GATEINNO, '') GateInNo, vo.OrderID, '' BranchName, '' UserName 
                    From v_purchase_emp_order_wise2 vo inner join V_FinishedItemDetail vf on vo.finishedid=vf.ITEM_FINISHED_ID
                    where 1 =1 ";
                }
                else
                {
                    qry = @"SELECT vo.empname,vo.pindentissueid,vo.finishedid,vo.item_description as description,qty as orderqty,vo.orderdate,vo.recdate,vo.recqty,
                    '" + TxtFRDate.Text + "' FromDate,'" + TxtTODate.Text + @"' as ToDate,vo.CanQty OrdercanQty,vo.ReturnDate,vo.ReturnChallan,vo.qtyreturn,vo.PO,
                    vo.Localorder,vo.customercode,vo.deliverydate,vo.rate,vo.LshortPercentage,vo.pindentissuetranid,vo.Category_name,vo.Status,Vo.colour,
                    vo.Recqty_beforeRetnqty, IsNull(vo.GATEINNO, '') GateInNo, vo.OrderID, vo.CustomerOrderNo,DATEDIFF(DAY,getdate() ,vo.deliverydate ) as DayDifference,
                    Case When VO.GstType=1 Then VO.SGST else Case When VO.GSTType=2 Then VO.IGST Else 0 End End As SGSTIGST,vo.requestby,vo.requestfor,
                    vo.BranchName, vo.UserName 
                    From v_purchase_emp_order_wise vo(Nolock) inner join V_FinishedItemDetail vf(Nolock) on vo.finishedid=vf.ITEM_FINISHED_ID 
                    Where 1 =1 ";
                }


                ////                qry = @"SELECT vo.empname,vo.pindentissueid,vo.finishedid,vo.item_description as description,qty as orderqty,vo.orderdate,vo.recdate,vo.recqty,
                ////                    '" + TxtFRDate.Text + "' FromDate,'" + TxtTODate.Text + @"' as ToDate,vo.CanQty OrdercanQty,vo.ReturnDate,vo.ReturnChallan,vo.qtyreturn,vo.PO,
                ////                    vo.Localorder,vo.customercode,vo.deliverydate,vo.rate,vo.LshortPercentage,vo.pindentissuetranid,vo.Category_name,vo.Status,Vo.colour,
                ////                    vo.Recqty_beforeRetnqty, IsNull(vo.GATEINNO, '') GateInNo, vo.OrderID, vo.CustomerOrderNo 
                ////                    From v_purchase_emp_order_wise vo inner join V_FinishedItemDetail vf on vo.finishedid=vf.ITEM_FINISHED_ID
                ////                    where 1 =1 ";
                if (ChkForDate.Checked == true)
                {
                    qry = qry + " And vo.orderdate >= '" + TxtFRDate.Text + "' and vo.orderdate <='" + TxtTODate.Text + "'";
                    filterby = filterby + " From : " + TxtFRDate.Text + "  TO : " + TxtTODate.Text;
                }
                if (ddCompName.SelectedIndex > 0)
                {
                    qry = qry + " And vo.CompanyId=" + ddCompName.SelectedValue;
                    filterby = filterby + " Company : " + ddCompName.SelectedItem.Text;
                }
                if (ddcustomer.SelectedIndex > 0)
                {
                    qry = qry + " And vo.Customerid=" + ddcustomer.SelectedValue;
                    filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
                }
                if (ddOrderno.SelectedIndex > 0)
                {
                    qry = qry + " And vo.orderid=" + ddOrderno.SelectedValue;
                    filterby = filterby + " Order No : " + ddOrderno.SelectedItem.Text;
                }
                if (dsuppl.SelectedIndex > 0)
                {
                    qry = qry + " And vo.Empid=" + dsuppl.SelectedValue;
                    filterby = filterby + " Supp Name : " + dsuppl.SelectedItem.Text;
                }
                if (DDPONo.SelectedIndex > 0)
                {
                    qry = qry + " And vo.pindentissueid=" + DDPONo.SelectedValue;
                    filterby = filterby + " PO No : " + DDPONo.SelectedItem.Text;
                }
                if (ddCatagory.SelectedIndex > 0)
                {
                    qry = qry + " And vf.CATEGORY_ID=" + ddCatagory.SelectedValue;
                    filterby = filterby + " Category : " + ddCatagory.SelectedItem.Text;
                }
                if (dditemname.SelectedIndex > 0)
                {
                    qry = qry + " And vf.item_id=" + dditemname.SelectedValue;
                    filterby = filterby + " Item : " + dditemname.SelectedItem.Text;
                }
                if (dquality.SelectedIndex > 0)
                {
                    qry = qry + " And vf.qualityid=" + dquality.SelectedValue;
                    filterby = filterby + " Quality : " + dquality.SelectedItem.Text;
                }
                if (dddesign.SelectedIndex > 0)
                {
                    qry = qry + " And vf.designid=" + dddesign.SelectedValue;
                    filterby = filterby + " design : " + dddesign.SelectedItem.Text;
                }
                if (ddcolor.SelectedIndex > 0)
                {
                    qry = qry + " And vf.colorid=" + ddcolor.SelectedValue;
                    filterby = filterby + " Color : " + ddcolor.SelectedItem.Text;
                }
                if (ddshape.SelectedIndex > 0)
                {
                    qry = qry + " And vf.shapeid=" + ddshape.SelectedValue;
                    filterby = filterby + " shape : " + ddshape.SelectedItem.Text;
                }
                if (ddsize.SelectedIndex > 0)
                {
                    qry = qry + " And vf.sizeid=" + ddsize.SelectedValue;
                }
                if (ddlshade.SelectedIndex > 0)
                {
                    qry = qry + " And vf.shadecolorid=" + ddlshade.SelectedValue;
                    filterby = filterby + " Shadecolor : " + ddlshade.SelectedItem.Text;
                }
                if (DDStatus.SelectedIndex > 0)
                {
                    if (DDStatus.SelectedValue == "1")
                    {
                        qry = qry + " And vo.Status = 'COMPLETE'";
                    }
                    else if (DDStatus.SelectedValue == "2")
                    {
                        qry = qry + " And vo.Status = 'PENDING'";
                    }
                }
                filterby = filterby + " Status : " + DDStatus.SelectedItem.Text;
                qry = qry + "   order by vo.pindentissueid, vo.OrderID";

                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);

                if (chkexcelexport.Checked == true)
                {
                    if (Session["VarCompanyNo"].ToString() == "22")
                    {
                        PurchaseordervendorwisexcelexportDiamond(ds, filterby);
                        return;
                    }
                    else
                    {
                        Purchaseordervendorwisexcelexport(ds, filterby);
                        return;
                    }
                }
                else
                {
                    switch (Session["varcompanyid"].ToString())
                    {
                        case "6":
                        case "12":
                            Session["ReportPath"] = "Reports/RptpurchaseVendorArtindia.rpt";
                            break;
                        case "14":
                            Session["ReportPath"] = "Reports/RptpurchaseVendornew.rpt";
                            break;
                        default:
                            Session["ReportPath"] = "Reports/RptpurchaseVendor.rpt";
                            break;
                    }
                    Session["dsFileName"] = "~\\ReportSchema\\RptpurchaseVendor.xsd";
                }
            }
        }
        else if (RDDebitNote.Checked == true)
        {
            string datefrom = "";
            string dateto = "";
            if (ChkForDate.Checked == true)
            {
                datefrom = TxtFRDate.Text;
                dateto = TxtTODate.Text;
            }

            qry = "select *,'" + datefrom + "' FromDate,'" + dateto + @"' as ToDate from V_PurchaseDebitNoteDetail Where companyId=" + ddCompName.SelectedValue + " and DebitAmt>0";
            if (dsuppl.SelectedIndex > 0)
            {
                qry = qry + " and Empid=" + dsuppl.SelectedValue;
            }
            if (DDPONo.SelectedIndex > 0)
            {
                qry = qry + " and PurchaseReceiveId=" + DDPONo.SelectedValue;
            }
            if (ChkForDate.Checked == true)
            {
                qry = qry + " And Date>='" + TxtFRDate.Text + "' And Date<='" + TxtTODate.Text + "'";
            }
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            Session["ReportPath"] = "Reports/RptPurchasedebitNote.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptPurchasedebitNote.xsd";
        }
        else if (RDPurchaseReceive.Checked == true)
        {
            if (chkpurchasedetailbychallan.Checked == true)
            {
                if (Session["varCompanyNo"].ToString() == "14")
                {

                    PurchaseReceiveDetailByChallanNoEastern();
                    return;
                }
                else
                {
                    PurchaseReceiveDetailByChallanNo();
                    return;
                }
            }
            else if (chkexcelexport.Checked == true)
            {
                PurchaseReceiveDetailWithGSTAmt();
                return;
            }
            else
            {
                if (Session["VarCompanyNo"].ToString() == "43")
                {
                    PurchaseReceiveDetailCI();
                    return;
                }
                else
                {
                    PurchaseReceiveDetail();
                    return;
                }

            }
        }
        else if (RDPurchaseMaterialReceive.Checked == true)
        {
            if (ChkFinalAbbaReport.Checked == true)
            {
                PurchaseMaterialReceiveIssueHafiziaAbbaReport();
                return;
            }
            else
            {
                PurchaseMaterialReceive();
                return;
            }
        }
        else if (RDPurchaseMaterialRecPending.Checked == true)
        {
            PurchaseMaterialRecPendingAsOnDate();
            return;
        }
        else if (RDPurchaseOrderReceiveBuyerCode.Checked == true)
        {
            PurchaseOrderReceiveOrderWise();
            return;
        }
        else if (RDPurchaseMaterialIssueReceive.Checked == true)
        {
            PurchaseOrderIssRecHafizia();
            return;
        }
        else if (RDCustomerOrderWisePODetail.Checked == true)
        {
            CustomerOrderWisePODetail();
            return;
        }
        else if (RDPurchaseOrderRecPendingDetail.Checked == true)
        {
            string filterby = "";
            qry = @"SELECT vo.empname,vo.pindentissueid,vo.finishedid,vo.item_description as description,qty as orderqty,vo.orderdate,vo.recdate,vo.recqty,
                    '" + TxtFRDate.Text + "' FromDate,'" + TxtTODate.Text + @"' as ToDate,vo.CanQty OrdercanQty,vo.ReturnDate,vo.ReturnChallan,vo.qtyreturn,vo.PO,
                    vo.Localorder,vo.customercode,vo.deliverydate,vo.rate,vo.LshortPercentage,vo.pindentissuetranid,vo.Category_name,vo.Status,Vo.colour,
                    vo.Recqty_beforeRetnqty, IsNull(vo.GATEINNO, '') GateInNo, vo.OrderID, vo.CustomerOrderNo,DATEDIFF(DAY,getdate() ,vo.deliverydate ) as DayDifference,
                    vo.requestby,vo.requestfor,vo.BranchName, vo.UserName 
                    From v_purchase_emp_order_wise vo(Nolock) 
                    inner join V_FinishedItemDetail vf(Nolock) on vo.finishedid=vf.ITEM_FINISHED_ID 
                    Where 1 =1 ";

            if (ChkForDate.Checked == true)
            {
                qry = qry + " And vo.orderdate >= '" + TxtFRDate.Text + "' and vo.orderdate <='" + TxtTODate.Text + "'";
                filterby = filterby + " From : " + TxtFRDate.Text + "  TO : " + TxtTODate.Text;
            }
            if (ddCompName.SelectedIndex > 0)
            {
                qry = qry + " And vo.CompanyId=" + ddCompName.SelectedValue;
                filterby = filterby + " Company : " + ddCompName.SelectedItem.Text;
            }
            if (ddcustomer.SelectedIndex > 0)
            {
                qry = qry + " And vo.Customerid=" + ddcustomer.SelectedValue;
                filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
            }
            if (ddOrderno.SelectedIndex > 0)
            {
                qry = qry + " And vo.orderid=" + ddOrderno.SelectedValue;
                filterby = filterby + " Order No : " + ddOrderno.SelectedItem.Text;
            }
            if (dsuppl.SelectedIndex > 0)
            {
                qry = qry + " And vo.Empid=" + dsuppl.SelectedValue;
                filterby = filterby + " Supp Name : " + dsuppl.SelectedItem.Text;
            }
            if (DDPONo.SelectedIndex > 0)
            {
                qry = qry + " And vo.pindentissueid=" + DDPONo.SelectedValue;
                filterby = filterby + " PO No : " + DDPONo.SelectedItem.Text;
            }
            if (ddCatagory.SelectedIndex > 0)
            {
                qry = qry + " And vf.CATEGORY_ID=" + ddCatagory.SelectedValue;
                filterby = filterby + " Category : " + ddCatagory.SelectedItem.Text;
            }
            if (dditemname.SelectedIndex > 0)
            {
                qry = qry + " And vf.item_id=" + dditemname.SelectedValue;
                filterby = filterby + " Item : " + dditemname.SelectedItem.Text;
            }
            if (dquality.SelectedIndex > 0)
            {
                qry = qry + " And vf.qualityid=" + dquality.SelectedValue;
                filterby = filterby + " Quality : " + dquality.SelectedItem.Text;
            }
            if (dddesign.SelectedIndex > 0)
            {
                qry = qry + " And vf.designid=" + dddesign.SelectedValue;
                filterby = filterby + " design : " + dddesign.SelectedItem.Text;
            }
            if (ddcolor.SelectedIndex > 0)
            {
                qry = qry + " And vf.colorid=" + ddcolor.SelectedValue;
                filterby = filterby + " Color : " + ddcolor.SelectedItem.Text;
            }
            if (ddshape.SelectedIndex > 0)
            {
                qry = qry + " And vf.shapeid=" + ddshape.SelectedValue;
                filterby = filterby + " shape : " + ddshape.SelectedItem.Text;
            }
            if (ddsize.SelectedIndex > 0)
            {
                qry = qry + " And vf.sizeid=" + ddsize.SelectedValue;
            }
            if (ddlshade.SelectedIndex > 0)
            {
                qry = qry + " And vf.shadecolorid=" + ddlshade.SelectedValue;
                filterby = filterby + " Shadecolor : " + ddlshade.SelectedItem.Text;
            }
            //if (DDStatus.SelectedIndex > 0)
            //{
            //    if (DDStatus.SelectedValue == "1")
            //    {
            //        qry = qry + " And vo.Status = 'COMPLETE'";
            //    }
            //    else if (DDStatus.SelectedValue == "2")
            //    {
            //        qry = qry + " And vo.Status = 'PENDING'";
            //    }
            //}
            filterby = filterby + " Status : " + DDStatus.SelectedItem.Text;
            qry = qry + "   order by vo.pindentissueid, vo.OrderID";

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            
            Session["ReportPath"] = "Reports/RptPurchaseOrderRecPendingDetail.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseOrderRecPendingDetail.xsd";
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
    protected void PurchaseReceiveDetail()
    {
        #region Where Condition
        string Where = "";
        string filterby = "From : " + TxtFRDate.Text + "  To : " + TxtTODate.Text;
        Where = Where + " and Prm.Receivedate>='" + TxtFRDate.Text + "' and PRM.Receivedate<='" + TxtTODate.Text + "'";
        if (ddcustomer.SelectedIndex > 0)
        {
            Where = Where + " And VP.Customerid=" + ddcustomer.SelectedValue;
            filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
        }
        if (ddOrderno.SelectedIndex > 0)
        {
            Where = Where + " And VP.orderid=" + ddOrderno.SelectedValue;
            filterby = filterby + " Order No : " + ddOrderno.SelectedItem.Text;
        }
        if (dsuppl.SelectedIndex > 0)
        {
            Where = Where + " And EI.Empid=" + dsuppl.SelectedValue;
            filterby = filterby + " Supp Name : " + dsuppl.SelectedItem.Text;
        }
        if (DDPONo.SelectedIndex > 0)
        {
            Where = Where + " And PRD.pindentissueid=" + DDPONo.SelectedValue;
            filterby = filterby + " PO No : " + DDPONo.SelectedItem.Text;
        }
        if (TRRecChallanNo.Visible == true)
        {
            if (txtRecChallanNo.Text != "0" && txtRecChallanNo.Text != "")
            {
                Where = Where + " And PRM.BillNo1='" + txtRecChallanNo.Text + "'";
                filterby = filterby + " BillNo : '" + txtRecChallanNo.Text + "'";
            }
        }

        if (ddCatagory.SelectedIndex > 0)
        {
            Where = Where + " And vf.CATEGORY_ID=" + ddCatagory.SelectedValue;
            filterby = filterby + " Category : " + ddCatagory.SelectedItem.Text;
        }
        if (dditemname.SelectedIndex > 0)
        {
            Where = Where + " And vf.item_id=" + dditemname.SelectedValue;
            filterby = filterby + " Item : " + dditemname.SelectedItem.Text;
        }
        if (dquality.SelectedIndex > 0)
        {
            Where = Where + " And vf.qualityid=" + dquality.SelectedValue;
            filterby = filterby + " Quality : " + dquality.SelectedItem.Text;
        }
        if (dddesign.SelectedIndex > 0)
        {
            Where = Where + " And vf.designid=" + dddesign.SelectedValue;
            filterby = filterby + " design : " + dddesign.SelectedItem.Text;
        }
        if (ddcolor.SelectedIndex > 0)
        {
            Where = Where + " And vf.colorid=" + ddcolor.SelectedValue;
            filterby = filterby + " Color : " + ddcolor.SelectedItem.Text;
        }
        if (ddshape.SelectedIndex > 0)
        {
            Where = Where + " And vf.shapeid=" + ddshape.SelectedValue;
            filterby = filterby + " shape : " + ddshape.SelectedItem.Text;
        }
        if (ddsize.SelectedIndex > 0)
        {
            Where = Where + " And vf.sizeid=" + ddsize.SelectedValue;
        }
        if (ddlshade.SelectedIndex > 0)
        {
            Where = Where + " And vf.shadecolorid=" + ddlshade.SelectedValue;
            filterby = filterby + " Shadecolor : " + ddlshade.SelectedItem.Text;
        }
        if (DDgodown.SelectedIndex > 0)
        {
            Where = Where + " And gm.godownid=" + DDgodown.SelectedValue;
            filterby = filterby + " Godown : " + DDgodown.SelectedItem.Text;
        }

        #endregion
         SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlCommand cmd = new SqlCommand("PRO_GETPURCHASERECEIVEDETAILS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@CompanyId", ddCompName.SelectedValue);           
            cmd.Parameters.AddWithValue("@Where",Where);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            sda.Fill(dt);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                Decimal TQty = 0, TAmount = 0;
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;
                //***********
                sht.Range("A1:K1").Merge();
                sht.Range("A1").Value = "Purchase Received Details " + "For - " + ddCompName.SelectedItem.Text;
                sht.Range("A2:K2").Merge();
                sht.Range("A2").Value = "Filter By :  " + filterby;
                sht.Row(2).Height = 30;
                sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:K2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A1:K2").Style.Font.Bold = true;
                //***********Filter By Item_Name
                row = 3;
                DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "Item_Name");
                foreach (DataRow dr in dtdistinct.Rows)
                {
                    DataView dv = new DataView(ds.Tables[0]);
                    dv.RowFilter = "Item_Name='" + dr["Item_Name"] + "'";
                    dv.Sort = "Receivedate";
                    DataSet ds1 = new DataSet();
                    ds1.Tables.Add(dv.ToTable());
                    //******Headers
                    sht.Range("A" + row).SetValue(dr["Item_Name"]);
                    sht.Range("A" + row).Style.Font.SetBold();
                    sht.Range("A" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    row = row + 1;
                    sht.Range("A" + row).Value = "Vendor Name";
                    sht.Range("B" + row).Value = "Quality";
                    sht.Range("C" + row).Value = "Rec.Date";

                    sht.Range("D" + row).Value = "Vendor Lot No.";
                    sht.Range("E" + row).Value = "Internal Lot No.";
                    sht.Range("F" + row).Value = "Godown Name";

                    sht.Range("G" + row).Value = "Rec.Qty";
                    sht.Range("H" + row).Value = "Rate";
                    sht.Range("I" + row).Value = "Amount";
                    sht.Range("J" + row).Value = "Gate In No";
                    sht.Range("K" + row).Value = "Moisture(%)";
                    sht.Range("G" + row + ":I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("A" + row + ":K" + row).Style.Font.SetBold();
                    //******
                    row = row + 1;
                    int Rowfrom = 0;
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        if (Rowfrom == 0)
                        {
                            Rowfrom = row;
                        }
                        sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["empname"]);
                        sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["QUALITYNAME"]);
                        sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["RECDATE"]);
                        sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["VendorLotno"]);
                        sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
                        sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["godownName"]);
                        sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["ACTUALRECQTY"]);
                        TQty = TQty + Convert.ToDecimal(ds1.Tables[0].Rows[i]["ACTUALRECQTY"]);
                        sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);
                        sht.Range("I" + row).FormulaA1 = "=G" + row + '*' + ("$H$" + row + "");
                        TAmount = TAmount + (Convert.ToDecimal(ds1.Tables[0].Rows[i]["ACTUALRECQTY"]) * Convert.ToDecimal(ds1.Tables[0].Rows[i]["Rate"]));
                        sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["GateInNo"]);
                        sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["ReceiveMoisture"]);

                        row = row + 1;
                    }
                    //TOTAL

                    sht.Range("F" + row).Value = "Total";
                    sht.Range("G" + row).FormulaA1 = "=SUM(G" + Rowfrom + ":$G$" + (row - 1) + ")";
                    sht.Range("I" + row).FormulaA1 = "=SUM(I" + Rowfrom + ":$I$" + (row - 1) + ")";
                    sht.Range("G" + row + ":I" + row).Style.Font.SetBold();
                    row = row + 2;
                    ds1.Dispose();
                }
                //*************GRAND TOTAL
                sht.Range("G" + row + ":I" + row).Style.Font.SetBold();
                sht.Range("F" + row).Value = "Grand Total";
                sht.Range("G" + row).SetValue(TQty);
                sht.Range("I" + row).SetValue(TAmount);
                //*************
                sht.Columns(1, 11).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("PurchaseReceiveDetails_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "purchaserec", "alert('No Record Found!');", true);

            }
        }
        catch (Exception ex)
        {
            Label1.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

        
        //SqlParameter[] param = new SqlParameter[2];
        //param[0] = new SqlParameter("@CompanyId", ddCompName.SelectedValue);
        //param[1] = new SqlParameter("@Where", Where);

        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETPURCHASERECEIVEDETAILS", param);
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
        //    {
        //        Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
        //    }
        //    string Path = "";
        //    Decimal TQty = 0, TAmount = 0;
        //    var xapp = new XLWorkbook();
        //    var sht = xapp.Worksheets.Add("sheet1");
        //    int row = 0;
        //    //***********
        //    sht.Range("A1:K1").Merge();
        //    sht.Range("A1").Value = "Purchase Received Details " + "For - " + ddCompName.SelectedItem.Text;
        //    sht.Range("A2:K2").Merge();
        //    sht.Range("A2").Value = "Filter By :  " + filterby;
        //    sht.Row(2).Height = 30;
        //    sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    sht.Range("A2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    sht.Range("A2:K2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
        //    sht.Range("A1:K2").Style.Font.Bold = true;
        //    //***********Filter By Item_Name
        //    row = 3;
        //    DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "Item_Name");
        //    foreach (DataRow dr in dtdistinct.Rows)
        //    {
        //        DataView dv = new DataView(ds.Tables[0]);
        //        dv.RowFilter = "Item_Name='" + dr["Item_Name"] + "'";
        //        dv.Sort = "Receivedate";
        //        DataSet ds1 = new DataSet();
        //        ds1.Tables.Add(dv.ToTable());
        //        //******Headers
        //        sht.Range("A" + row).SetValue(dr["Item_Name"]);
        //        sht.Range("A" + row).Style.Font.SetBold();
        //        sht.Range("A" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        //        row = row + 1;
        //        sht.Range("A" + row).Value = "Vendor Name";
        //        sht.Range("B" + row).Value = "Quality";
        //        sht.Range("C" + row).Value = "Rec.Date";

        //        sht.Range("D" + row).Value = "Vendor Lot No.";
        //        sht.Range("E" + row).Value = "Internal Lot No.";
        //        sht.Range("F" + row).Value = "Godown Name";

        //        sht.Range("G" + row).Value = "Rec.Qty";
        //        sht.Range("H" + row).Value = "Rate";
        //        sht.Range("I" + row).Value = "Amount";
        //        sht.Range("J" + row).Value = "Gate In No";
        //        sht.Range("K" + row).Value = "Moisture(%)";
        //        sht.Range("G" + row + ":I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        //        sht.Range("A" + row + ":K" + row).Style.Font.SetBold();
        //        //******
        //        row = row + 1;
        //        int Rowfrom = 0;
        //        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        //        {
        //            if (Rowfrom == 0)
        //            {
        //                Rowfrom = row;
        //            }
        //            sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["empname"]);
        //            sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["QUALITYNAME"]);
        //            sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["RECDATE"]);
        //            sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["VendorLotno"]);
        //            sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
        //            sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["godownName"]);
        //            sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["ACTUALRECQTY"]);
        //            TQty = TQty + Convert.ToDecimal(ds1.Tables[0].Rows[i]["ACTUALRECQTY"]);
        //            sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);
        //            sht.Range("I" + row).FormulaA1 = "=G" + row + '*' + ("$H$" + row + "");
        //            TAmount = TAmount + (Convert.ToDecimal(ds1.Tables[0].Rows[i]["ACTUALRECQTY"]) * Convert.ToDecimal(ds1.Tables[0].Rows[i]["Rate"]));
        //            sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["GateInNo"]);
        //            sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["ReceiveMoisture"]);

        //            row = row + 1;
        //        }
        //        //TOTAL

        //        sht.Range("F" + row).Value = "Total";
        //        sht.Range("G" + row).FormulaA1 = "=SUM(G" + Rowfrom + ":$G$" + (row - 1) + ")";
        //        sht.Range("I" + row).FormulaA1 = "=SUM(I" + Rowfrom + ":$I$" + (row - 1) + ")";
        //        sht.Range("G" + row + ":I" + row).Style.Font.SetBold();
        //        row = row + 2;
        //        ds1.Dispose();
        //    }
        //    //*************GRAND TOTAL
        //    sht.Range("G" + row + ":I" + row).Style.Font.SetBold();
        //    sht.Range("F" + row).Value = "Grand Total";
        //    sht.Range("G" + row).SetValue(TQty);
        //    sht.Range("I" + row).SetValue(TAmount);
        //    //*************
        //    sht.Columns(1, 11).AdjustToContents();
        //    //********************
        //    string Fileextension = "xlsx";
        //    string filename = UtilityModule.validateFilename("PurchaseReceiveDetails_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
        //    Path = Server.MapPath("~/Tempexcel/" + filename);
        //    xapp.SaveAs(Path);
        //    xapp.Dispose();
        //    //Download File
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    // Response.Clear();
        //    Response.ContentType = "application/vnd.ms-excel";
        //    Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        //    Response.WriteFile(Path);
        //    // File.Delete(Path);
        //    Response.End();
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(Page, GetType(), "purchaserec", "alert('No Record Found!');", true);

        //}


    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
    }
    private void ddlcategorycange()
    {
        try
        {
            UtilityModule.ConditionalComboFill(ref dditemname, "select distinct item_id,item_name  from ITEM_MASTER where category_id=" + ddCatagory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Item");
            ql.Visible = false;
            clr.Visible = false;
            dsn.Visible = false;
            shp.Visible = false;
            sz.Visible = false;
            shd.Visible = false;
            string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                          " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                          " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCatagory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    switch (dr["PARAMETER_ID"].ToString())
                    {
                        case "1":
                            ql.Visible = true;
                            break;
                        case "2":
                            dsn.Visible = true;
                            UtilityModule.ConditionalComboFill(ref dddesign, "select distinct designId, designName from Design Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "Slect Design");
                            break;
                        case "3":
                            clr.Visible = true;
                            UtilityModule.ConditionalComboFill(ref ddcolor, "select distinct colorid, colorname from color Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Color");
                            break;
                        case "4":
                            shp.Visible = true;
                            UtilityModule.ConditionalComboFill(ref ddshape, "select distinct ShapeId, ShapeName from shape Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Shape");
                            break;
                        case "5":
                            sz.Visible = true;
                            break;
                        case "6":
                            shd.Visible = true;
                            UtilityModule.ConditionalComboFill(ref ddlshade, "select distinct ShadecolorId, ShadeColorName from ShadeColor Where MasterCompanyId=" + Session["varCompanyId"] + " Order By ShadeColorName", true, "Select ShadeColor");
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchageIndentIssue.aspx");
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillitemchange();
    }
    private void fillitemchange()
    {
        UtilityModule.ConditionalComboFill(ref dquality, "select distinct qualityid, qualityname from quality where item_id=" + dditemname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By qualityname", true, "Select Item");
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillShapeSelectedChange();
    }
    private void FillShapeSelectedChange()
    {
        string Str = "SizeFt";

        UtilityModule.ConditionalComboFill(ref ddsize, "select distinct SizeId, " + Str + @" from Size Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Size--");
    }
    protected void RDpurdelivRpt_CheckedChanged(object sender, EventArgs e)
    {
        TrStatus.Visible = false;
        PurDelivCheckedChange();
        TRPurchaseSumm.Visible = false;
        Trgodown.Visible = false;
        TRFinalAbbaReport.Visible = false;
        TRASOnDate.Visible = false;
    }
    private void PurDelivCheckedChange()
    {
        trChkForDate.Visible = false;
        trPurchaseIndentChallanNo.Visible = false;
        Tr3.Visible = false;
        shp.Visible = false;
        // Trcomp.Visible = false;
        trcustomer.Visible = false;
        trorder.Visible = false;
        trsupply.Visible = true;
        trfr.Visible = true;
        trto.Visible = true;
        TRRecChallanNo.Visible = false;
        txtRecChallanNo.Text = "";
        TRPurchaseDetailByChallan.Visible = false;
        TRLotBillDetail.Visible = false;
        TRFinalAbbaReport.Visible = false;
    }
    protected void RDindent_CheckedChanged(object sender, EventArgs e)
    {
        TrStatus.Visible = false;
        PurIndentCheckedChange();
        TRPurchaseSumm.Visible = false;
        Trgodown.Visible = false;
        TRFinalAbbaReport.Visible = false;
    }
    private void PurIndentCheckedChange()
    {
        TRFinalAbbaReport.Visible = false;
        trChkForDate.Visible = false;
        trPurchaseIndentChallanNo.Visible = false;
        Tr3.Visible = true;
        TrItemName.Visible = true;
        Trcomp.Visible = false;
        trcustomer.Visible = false;
        trorder.Visible = false;
        trsupply.Visible = false;
        trfr.Visible = true;
        trto.Visible = true;
        TRRecChallanNo.Visible = false;
        txtRecChallanNo.Text = "";
        TRPurchaseDetailByChallan.Visible = false;
        TRLotBillDetail.Visible = false;
        UtilityModule.ConditionalComboFill(ref ddCatagory, @"Select distinct icm.CATEGORY_ID,icm.CATEGORY_NAME from ITEM_CATEGORY_MASTER icm inner join ITEM_MASTER im on 
        icm.CATEGORY_ID=im.CATEGORY_ID inner join ITEM_PARAMETER_MASTER ipm on ipm.item_id=im.item_id  where ipm.item_finished_id in(select finishedid from PurchaseIndentIssueTran ) And ipm.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Category");
    }
    protected void RDpurchasedetail_CheckedChanged(object sender, EventArgs e)
    {
        TrStatus.Visible = false;
        TRPurchaseSumm.Visible = false;
        PurchasedetailCheckedChange();
        Trgodown.Visible = false;
        TRFinalAbbaReport.Visible = false;
        TRASOnDate.Visible = false;
    }
    private void PurchasedetailCheckedChange()
    {
        trChkForDate.Visible = false;
        trPurchaseIndentChallanNo.Visible = false;
        Tr3.Visible = false;
        TrItemName.Visible = false;
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        Trcomp.Visible = true;
        trcustomer.Visible = true;
        trorder.Visible = true;
        trfr.Visible = true;
        trto.Visible = true;
        trsupply.Visible = true;
        TRRecChallanNo.Visible = false;
        txtRecChallanNo.Text = "";
        TRPurchaseDetailByChallan.Visible = false;
        TRLotBillDetail.Visible = false;
        TRFinalAbbaReport.Visible = false;
    }
    protected void RDRawMaterial_CheckedChanged(object sender, EventArgs e)
    {
        TrStatus.Visible = false;
        RawmaterialdetailCheckedChange();
        TRPurchaseSumm.Visible = false;
        trChkForDate.Visible = false;
        Trgodown.Visible = false;
        TRFinalAbbaReport.Visible = false;
        TRASOnDate.Visible = false;
    }
    private void RawmaterialdetailCheckedChange()
    {
        trPurchaseIndentChallanNo.Visible = false;
        trsupply.Visible = false;
        trfr.Visible = false;
        trto.Visible = false;
        trsupply.Visible = false;
        Tr3.Visible = false;
        TrItemName.Visible = false;
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        Trcomp.Visible = true;
        trcustomer.Visible = true;
        trorder.Visible = true;
        trsupply.Visible = false;
        trfr.Visible = false;
        trto.Visible = false;
        trsupply.Visible = false;
        TRRecChallanNo.Visible = false;
        txtRecChallanNo.Text = "";
        TRPurchaseDetailByChallan.Visible = false;
        TRLotBillDetail.Visible = false;
        TRFinalAbbaReport.Visible = false;
    }
    protected void RDpurchasesupply_CheckedChanged(object sender, EventArgs e)
    {
        TrStatus.Visible = false;
        Trgodown.Visible = false;
        trChkForDate.Visible = false;
        trsupply.Visible = true;
        trfr.Visible = true;
        trto.Visible = true;
        Tr3.Visible = false;
        TrItemName.Visible = false;
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        //Trcomp.Visible = false;
        trcustomer.Visible = false;
        trorder.Visible = false;
        trPurchaseIndentChallanNo.Visible = false;
        TRPurchaseSumm.Visible = false;
        TRRecChallanNo.Visible = false;
        txtRecChallanNo.Text = "";
        TRPurchaseDetailByChallan.Visible = false;
        TRLotBillDetail.Visible = false;
        TRFinalAbbaReport.Visible = false;
        TRASOnDate.Visible = false;
    }
    protected void RDSupplyorder_CheckedChanged(object sender, EventArgs e)
    {
        TrStatus.Visible = true;
        Trgodown.Visible = false;
        trChkForDate.Visible = true;
        Tr3.Visible = true;
        Fill_Category();
        TrItemName.Visible = true;
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        Trcomp.Visible = true;
        trcustomer.Visible = false;
        trorder.Visible = false;
        trfr.Visible = true;
        trto.Visible = true;
        trsupply.Visible = true;
        trPurchaseIndentChallanNo.Visible = true;
        lblPoNo.Text = "PO.No.";
        TRPurchaseSumm.Visible = true;
        trcustomer.Visible = true;
        trorder.Visible = true;
        TRRecChallanNo.Visible = false;
        txtRecChallanNo.Text = "";
        TRPurchaseDetailByChallan.Visible = false;
        TRLotBillDetail.Visible = true;
        TRFinalAbbaReport.Visible = false;
        TRASOnDate.Visible = false;

        if (Session["varcompanyid"].ToString() == "44")
        {
            UtilityModule.ConditionalComboFill(ref ddcustomer, @"select Distinct CI.CustomerId,CI.CustomerCode as Customercode from Ordermaster OM
                                                          inner join customerinfo CI on ci.CustomerId=OM.CustomerId order by Customercode", true, "Select CustomerCode");
        }
        else
        { 
          UtilityModule.ConditionalComboFill(ref ddcustomer, @"select Distinct CI.CustomerId,CI.CustomerCode+'/'+CI.CompanyName as Customercode from Ordermaster OM
                                                          inner join customerinfo CI on ci.CustomerId=OM.CustomerId order by Customercode", true, "Select CustomerCode");
        
        }
    }
    protected void dsuppl_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        if (RDSupplyorder.Checked == true || RDPurchaseOrderRecPendingDetail.Checked==true)
        {
            str = "select distinct pii.PIndentIssueId,pii.ChallanNo from PurchaseIndentIssue PII left Join OrderMaster OM on Pii.orderid=Om.orderid  Where pii.PartyId=" + dsuppl.SelectedValue + " And pii.CompanyId=" + ddCompName.SelectedValue + "";
            if (ddcustomer.SelectedIndex > 0)
            {
                str = str + " and Om.customerid=" + ddcustomer.SelectedValue;
            }
            if (ddOrderno.SelectedIndex > 0)
            {
                str = str + " and Om.orderid=" + ddOrderno.SelectedValue;
            }
            UtilityModule.ConditionalComboFill(ref DDPONo, str, true, "--Select P.O.No--");
        }
        else if (RDDebitNote.Checked == true)
        {
            str = "select PurchaseReceiveId,BillNo from PurchaseReceiveMaster where CompanyId=" + ddCompName.SelectedValue;
            if (dsuppl.SelectedIndex > 0)
            {
                str = str + " And Partyid=" + dsuppl.SelectedValue;
            }
            str = str + " order by PurchaseReceiveId desc";
            UtilityModule.ConditionalComboFill(ref DDPONo, str, true, "--Plz Select--");
        }
        else if (RDPurchaseReceive.Checked == true)
        {
            if (Session["VarCompanyNo"].ToString() == "14" || Session["VarCompanyNo"].ToString() == "43")
            {
                str = "select distinct pii.PIndentIssueId,pii.ChallanNo from PurchaseIndentIssue PII left Join OrderMaster OM on Pii.orderid=Om.orderid  Where pii.PartyId=" + dsuppl.SelectedValue + " And pii.CompanyId=" + ddCompName.SelectedValue + "";
                if (ddcustomer.SelectedIndex > 0)
                {
                    str = str + " and Om.customerid=" + ddcustomer.SelectedValue;
                }
                if (ddOrderno.SelectedIndex > 0)
                {
                    str = str + " and Om.orderid=" + ddOrderno.SelectedValue;
                }
                UtilityModule.ConditionalComboFill(ref DDPONo, str, true, "--Select P.O.No--");
            }
        }
    }
    protected void ddOrderno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RDSupplyorder.Checked == true)
        {
            string str = "select Distinct Ei.empid,Ei.empname from  purchaseindentissue pii inner join empinfo Ei on pii.partyid=ei.empid where companyid= " + ddCompName.SelectedValue;
            if (ddOrderno.SelectedIndex > 0)
            {
                str = str + " and orderid=" + ddOrderno.SelectedValue;
            }
            str = str + " order by ei.empname";
            UtilityModule.ConditionalComboFill(ref dsuppl, str, true, "--Select--");
        }
    }
    protected void DDPONo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str;
        str = "select REPLACE(convert(nvarchar(11),date,106),' ','-') as Date from PurchaseIndentIssue  Where PindentIssueid=" + DDPONo.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtFRDate.Text = ds.Tables[0].Rows[0]["Date"].ToString();
            TxtTODate.Text = ds.Tables[0].Rows[0]["Date"].ToString();
        }
        else
        {
            TxtFRDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtTODate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }

    }
    protected void ChkForDate_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForDate.Checked == true)
        {
            trfr.Visible = true;
            trto.Visible = true;
        }
        else
        {
            trto.Visible = false;
            trfr.Visible = false;
        }
    }
    protected void RDDebitNote_CheckedChanged(object sender, EventArgs e)
    {
        TrStatus.Visible = false;
        Trgodown.Visible = false;
        trChkForDate.Visible = true;
        trcustomer.Visible = false;
        trorder.Visible = false;
        TrItemName.Visible = false;
        Tr3.Visible = false;
        ql.Visible = false;
        dsn.Visible = false;
        clr.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        trfr.Visible = false;
        trto.Visible = false;
        TRPurchaseSumm.Visible = false;
        //Visibility True
        Trcomp.Visible = true;
        trsupply.Visible = true;
        trPurchaseIndentChallanNo.Visible = true;
        lblPoNo.Text = "Rec. ChallanNo.";
        TRRecChallanNo.Visible = false;
        txtRecChallanNo.Text = "";
        TRPurchaseDetailByChallan.Visible = false;
        TRLotBillDetail.Visible = false;
        TRFinalAbbaReport.Visible = false;
        TRASOnDate.Visible = false;
    }
    protected void Purchaseordervendorwisexcelexport(DataSet ds, string FilterBy)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 0;
            //***********
            sht.Range("A1:P1").Merge();
            sht.Range("A1").Value = "PURCHASE VENDOR WISE";
            sht.Range("A2:P2").Merge();
            sht.Range("A2").Value = "Filter By :  " + FilterBy;
            sht.Range("A1:P1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:R2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:T2").Style.Font.Bold = true;
            //**********
            sht.Range("A3").Value = "Category";
            sht.Range("B3").Value = "Po No";
            sht.Range("C3").Value = "Po Status";
            sht.Range("D3").Value = "Po Date";
            sht.Range("E3").Value = "Supp. Name";
            sht.Range("F3").Value = "Item Name";
            sht.Range("G3").Value = "Rate";
            sht.Range("H3").Value = "PO Qty";
            sht.Range("G3:H3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("I3").Value = "Delv Date";
            sht.Range("J3").Value = "CanQty/Date";
            sht.Range("K3").Value = "RecDate/ChallanNo";
            sht.Range("L3").Value = "Rec Qty";
            sht.Range("L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("M3").Value = "Ret Date";
            sht.Range("N3").Value = "Ret Qty";
            sht.Range("O3").Value = "Pending Qty";
            sht.Range("N3:O3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3:T3").Style.Font.SetBold();
            sht.Range("P3").Value = "Gate In NO";
            if (Session["varCompanyNo"].ToString() == "27" || Session["varCompanyNo"].ToString() == "14" || Session["varCompanyNo"].ToString() == "22")
            {
                sht.Range("Q3").Value = "";
                sht.Column(17).Hide();
            }
            else
            {
                sht.Range("Q3").Value = "Customer OrderNo";
            }

            if (Session["varCompanyNo"].ToString() == "22")
            {
                sht.Range("R3").Value = "Late By";
            }
            else
            {
                sht.Range("R3").Value = "";
                sht.Column(18).Hide();
            }
            sht.Range("S3").Value = "Branch Name";
            sht.Range("T3").Value = "User Name";

            using (var a = sht.Range("A3:T3"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*******
            row = 4;
            DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "Pindentissueid", "finishedid", "OrderID");
            foreach (DataRow dr in dtdistinct.Rows)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "Pindentissueid=" + dr["Pindentissueid"] + " and Finishedid=" + dr["finishedid"] + " and OrderID=" + dr["OrderID"];
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Category_name"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Po"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Status"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Orderdate"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Empname"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Description"].ToString() + ds1.Tables[0].Rows[i]["Colour"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Orderqty"]);
                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Deliverydate"]);
                    sht.Range("J" + row).SetValue("");
                    sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Recdate"]);
                    sht.Range("L" + row).SetValue(Convert.ToDecimal(ds1.Tables[0].Rows[i]["Recqty"]) + Convert.ToDecimal(ds1.Tables[0].Rows[i]["Qtyreturn"]));
                    sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["Returnchallan"]);
                    sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["Qtyreturn"]);
                    //sht.Range("O" + row).SetValue("");
                    if (i == 0)
                    {
                        sht.Range("O" + row).FormulaA1 = "=H" + row + '+' + "$N$" + row + '-' + "$L$" + row;
                    }
                    else
                    {
                        sht.Range("O" + row).FormulaA1 = "=O" + (row - 1) + '+' + "$N$" + row + '-' + "$L$" + row;
                    }
                    sht.Range("P" + row).SetValue(ds1.Tables[0].Rows[i]["GateInNo"]);

                    if (Session["varCompanyNo"].ToString() == "27" || Session["varCompanyNo"].ToString() == "14" || Session["varCompanyNo"].ToString() == "22")
                    {
                        sht.Range("Q" + row).SetValue("");
                    }
                    else
                    {
                        sht.Range("Q" + row).SetValue(ds1.Tables[0].Rows[i]["CustomerOrderNo"]);
                    }

                    if (Session["varCompanyNo"].ToString() == "22")
                    {
                        sht.Range("R" + row).SetValue(ds1.Tables[0].Rows[i]["DayDifference"]);
                    }
                    else
                    {
                        sht.Range("R" + row).SetValue("");
                    }
                    sht.Range("S" + row).SetValue(ds1.Tables[0].Rows[i]["BranchName"]);
                    sht.Range("T" + row).SetValue(ds1.Tables[0].Rows[i]["UserName"]);

                    row = row + 1;
                }
                ds1.Dispose();
            }
            //*************
            sht.Columns(1, 30).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PurchaseOrderVendorwise_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altP", "alert('No records found...')", true);
        }
    }
    protected void RDPurchaseReceive_CheckedChanged(object sender, EventArgs e)
    {
        TrStatus.Visible = false;
        Tr3.Visible = true;
        Fill_Category();
        TrItemName.Visible = true;
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        Trcomp.Visible = true;
        trcustomer.Visible = false;
        trorder.Visible = false;
        trfr.Visible = true;
        trto.Visible = true;
        trsupply.Visible = true;
        trPurchaseIndentChallanNo.Visible = true;
        lblPoNo.Text = "PO.No.";
        TRPurchaseSumm.Visible = true;
        trcustomer.Visible = true;
        trorder.Visible = true;
        TRRecChallanNo.Visible = true;
        TRPurchaseDetailByChallan.Visible = true;
        TRLotBillDetail.Visible = false;
        Trgodown.Visible = true;
        TRFinalAbbaReport.Visible = false;
        TRASOnDate.Visible = false;

        if (Session["VarCompanyNo"].ToString() == "22")
        {
            if (RDPurchaseReceive.Checked == true)
            {
                TDExcelExport.Visible = true;
            }
            else
            {
                TDExcelExport.Visible = false;
            }
        }
        
    }
    protected void RDPurchaseMaterialReceive_CheckedChanged(object sender, EventArgs e)
    {
        TrStatus.Visible = false;
        Tr3.Visible = true;
        Trgodown.Visible = false;
        Fill_Category();
        TrItemName.Visible = true;
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        Trcomp.Visible = true;
        trcustomer.Visible = false;
        trorder.Visible = false;
        trfr.Visible = true;
        trto.Visible = true;
        trsupply.Visible = true;
        trPurchaseIndentChallanNo.Visible = true;
        lblPoNo.Text = "PO.No.";
        TRPurchaseSumm.Visible = true;
        trcustomer.Visible = true;
        trorder.Visible = true;
        TRRecChallanNo.Visible = false;
        txtRecChallanNo.Text = "";
        TRPurchaseDetailByChallan.Visible = false;
        TRLotBillDetail.Visible = false;
        TRFinalAbbaReport.Visible = true;
        TRPurchaseSumm.Visible = false;
        TRASOnDate.Visible = false;

    }
    protected void PurchaseMaterialReceive()
    {
        #region Where Condition
        string Where = "";
        string filterby = "From : " + TxtFRDate.Text + "  To : " + TxtTODate.Text;
        Where = Where + " and PRM.Receivedate>='" + TxtFRDate.Text + "' and PRM.Receivedate<='" + TxtTODate.Text + "'";
        if (ddcustomer.SelectedIndex > 0)
        {
            Where = Where + " And VP.Customerid=" + ddcustomer.SelectedValue;
            filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
        }
        if (ddOrderno.SelectedIndex > 0)
        {
            Where = Where + " And VP.orderid=" + ddOrderno.SelectedValue;
            filterby = filterby + " Order No : " + ddOrderno.SelectedItem.Text;
        }
        if (dsuppl.SelectedIndex > 0)
        {
            Where = Where + " And EI.Empid=" + dsuppl.SelectedValue;
            filterby = filterby + " Supp Name : " + dsuppl.SelectedItem.Text;
        }
        if (DDPONo.SelectedIndex > 0)
        {
            Where = Where + " And PRD.pindentissueid=" + DDPONo.SelectedValue;
            filterby = filterby + " PO No : " + DDPONo.SelectedItem.Text;
        }
        if (ddCatagory.SelectedIndex > 0)
        {
            Where = Where + " And vf.CATEGORY_ID=" + ddCatagory.SelectedValue;
            filterby = filterby + " Category : " + ddCatagory.SelectedItem.Text;
        }
        if (dditemname.SelectedIndex > 0)
        {
            Where = Where + " And vf.item_id=" + dditemname.SelectedValue;
            filterby = filterby + " Item : " + dditemname.SelectedItem.Text;
        }
        if (dquality.SelectedIndex > 0)
        {
            Where = Where + " And vf.qualityid=" + dquality.SelectedValue;
            filterby = filterby + " Quality : " + dquality.SelectedItem.Text;
        }
        if (dddesign.SelectedIndex > 0)
        {
            Where = Where + " And vf.designid=" + dddesign.SelectedValue;
            filterby = filterby + " design : " + dddesign.SelectedItem.Text;
        }
        if (ddcolor.SelectedIndex > 0)
        {
            Where = Where + " And vf.colorid=" + ddcolor.SelectedValue;
            filterby = filterby + " Color : " + ddcolor.SelectedItem.Text;
        }
        if (ddshape.SelectedIndex > 0)
        {
            Where = Where + " And vf.shapeid=" + ddshape.SelectedValue;
            filterby = filterby + " shape : " + ddshape.SelectedItem.Text;
        }
        if (ddsize.SelectedIndex > 0)
        {
            Where = Where + " And vf.sizeid=" + ddsize.SelectedValue;
        }
        if (ddlshade.SelectedIndex > 0)
        {
            Where = Where + " And vf.shadecolorid=" + ddlshade.SelectedValue;
            filterby = filterby + " Shadecolor : " + ddlshade.SelectedItem.Text;
        }
        #endregion
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@CompanyId", ddCompName.SelectedValue);
        param[1] = new SqlParameter("@Where", Where);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETPURCHASERECEIVEDETAILS_HAFIZIA", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            Decimal TQty = 0, TAmount = 0, TBillQty = 0, TBillAmt = 0, TTransportAmt = 0, TUnloadingAmt = 0, TTransportPerKG = 0;
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 0;
            //***********
            sht.Range("A1:X1").Merge();
            sht.Range("A1").Value = "Purchase Material Received " + "For - " + ddCompName.SelectedItem.Text;
            sht.Range("A2:X2").Merge();
            sht.Range("A2").Value = "Filter By :  " + filterby;
            sht.Row(2).Height = 30;
            sht.Range("A1:X1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:X2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:X2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A1:X2").Style.Font.Bold = true;
            //***********Filter By Item_Name
            row = 3;

            //******Headers
            //sht.Range("A" + row).SetValue(dr["EMPNAME"]);
            //sht.Range("A" + row).Style.Font.SetBold();
            //sht.Range("A" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //row = row + 1;

            sht.Range("A" + row).Value = "Year";
            sht.Range("B" + row).Value = "Financial Year";
            sht.Range("C" + row).Value = "Rec.Date";
            sht.Range("D" + row).Value = "Supplier Name";
            sht.Range("E" + row).Value = "Address";
            sht.Range("F" + row).Value = "Yarn Detail ";
            sht.Range("G" + row).Value = "Yarn Count/Ply";
            sht.Range("H" + row).Value = "";
            sht.Range("I" + row).Value = "Shade Color";

            sht.Range("J" + row).Value = "Lot No";
            sht.Range("K" + row).Value = "YarnShape (CONE/HANK)";

            sht.Range("L" + row).Value = "HSN Code";
            sht.Range("M" + row).Value = "Bill No";
            sht.Range("N" + row).Value = "Bill Date";
            sht.Range("O" + row).Value = "Bill Qty";
            sht.Range("P" + row).Value = "Bag Qty";
            sht.Range("Q" + row).Value = "Receive Qty";
            sht.Range("R" + row).Value = "Bill Amount";
            //sht.Range("S" + row).Value = "ReceiveQty Amount";
            sht.Range("S" + row).Value = "Transport Builty Amount";
            sht.Range("T" + row).Value = "Unloading Expenses";
            sht.Range("U" + row).Value = "Yarn Rate/per KG";
            sht.Range("V" + row).Value = "Transport Per Kg";

            sht.Range("W" + row).Value = "Total";

            sht.Range("X" + row).Value = "GST%";
            ////sht.Range("W" + row).Value = "IGST%";
            //sht.Range("V" + row).Value = "Yarn Rate/per KG";

            ////sht.Range("W" + row).Value = "Yarn Rate/per KG";
            sht.Range("Y" + row).Value = "GST Value Per KG.";
            ////sht.Range("Z" + row).Value = "IGST % Per KG.";
            //sht.Range("X" + row).Value = "Transport Per Kg";
            sht.Range("Z" + row).Value = "Total Value Per Kg";

            sht.Column(8).Hide();

            sht.Range("AA" + row).Value = "";

            sht.Range("O" + row + ":AA" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A" + row + ":AB" + row).Style.Font.SetBold();
            using (var a = sht.Range("A" + row + ":AB" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //******

            row = row + 1;
            int Rowfrom = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                decimal GST = 0, IGST = 0;
                if (Rowfrom == 0)
                {
                    Rowfrom = row;
                }
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Year"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["FinancialYear"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["RECDATE"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Address"] + " " + ds.Tables[0].Rows[i]["Address2"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                //sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["designName"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["LotNo"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["YarnShape"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Hscode"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["BillNo1"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["BillDate"]);
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["BillQty"]);
                TBillQty = TBillQty + Convert.ToDecimal(ds.Tables[0].Rows[i]["BillQty"]);
                sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["Baleno"]);
                sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["ACTUALRECQTY"]);
                TQty = TQty + Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"]);
                sht.Range("R" + row).SetValue(ds.Tables[0].Rows[i]["BillAmt"]);
                TBillAmt = TBillAmt + (Convert.ToDecimal(ds.Tables[0].Rows[i]["BillQty"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]));
                //sht.Range("S" + row).SetValue(ds.Tables[0].Rows[i]["TotalAmt"]);
                sht.Range("S" + row).SetValue(ds.Tables[0].Rows[i]["TransportBuiltyAmt"]);
                TTransportAmt = TTransportAmt + (Convert.ToDecimal(ds.Tables[0].Rows[i]["TransportBuiltyAmt"]));
                sht.Range("T" + row).SetValue(ds.Tables[0].Rows[i]["UnloadingExpenses"]);
                TUnloadingAmt = TUnloadingAmt + (Convert.ToDecimal(ds.Tables[0].Rows[i]["UnloadingExpenses"]));
                sht.Range("U" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                if (Session["varcompanyid"].ToString() == "9")
                {
                    if ((ds.Tables[0].Rows[i]["TransportBuiltyAmt"].ToString() != "0") && (ds.Tables[0].Rows[i]["BillQty"].ToString() != "0"))
                    {
                        sht.Range("V" + row).SetValue(Math.Round((Convert.ToDecimal(ds.Tables[0].Rows[i]["TransportBuiltyAmt"]) / Convert.ToDecimal(ds.Tables[0].Rows[i]["BillQty"])), 2));
                        TTransportPerKG = Math.Round((Convert.ToDecimal(ds.Tables[0].Rows[i]["TransportBuiltyAmt"]) / Convert.ToDecimal(ds.Tables[0].Rows[i]["BillQty"])), 2);
                    }
                    else
                    {
                        sht.Range("V" + row).SetValue("0");
                        TTransportPerKG = 0;

                    }
                }
                else
                {

                    sht.Range("V" + row).SetValue(Math.Round((Convert.ToDecimal(ds.Tables[0].Rows[i]["TransportBuiltyAmt"]) / Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"])), 2));
                    TTransportPerKG = Math.Round((Convert.ToDecimal(ds.Tables[0].Rows[i]["TransportBuiltyAmt"]) / Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"])), 2);
                }
                sht.Range("W" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[i]["UnloadingExpenses"]) + Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) + TTransportPerKG);

                if (ds.Tables[0].Rows[i]["SGST"].ToString() != "0")
                {
                    sht.Range("X" + row).SetValue(ds.Tables[0].Rows[i]["SGST"]);
                }
                else if (ds.Tables[0].Rows[i]["IGST"].ToString() != "0")
                {
                    sht.Range("X" + row).SetValue(ds.Tables[0].Rows[i]["IGST"]);
                }
                else
                {
                    sht.Range("X" + row).SetValue(0);
                }



                //sht.Range("W" + row).SetValue(ds.Tables[0].Rows[i]["IGST"]);
                if (ds.Tables[0].Rows[i]["SGST"].ToString() != "0")
                {
                    GST = Math.Round((Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) * Convert.ToDecimal(Convert.ToDecimal(ds.Tables[0].Rows[i]["SGST"]) / 100)), 2);
                }
                if (ds.Tables[0].Rows[i]["IGST"].ToString() != "0")
                {
                    IGST = Math.Round((Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) * Convert.ToDecimal(Convert.ToDecimal(ds.Tables[0].Rows[i]["IGST"]) / 100)), 2);
                }

                ////sht.Range("X" + row).FormulaA1 = "=D" + row + '*' + ("$E$" + row + "");
                TAmount = TAmount + (Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]));
                ////sht.Range("Y" + row).SetValue((Convert.ToDecimal(ds1.Tables[0].Rows[i]["ACTUALRECQTY"]) * Convert.ToDecimal(ds1.Tables[0].Rows[i]["Rate"]) + GST + IGST + Convert.ToDecimal(ds1.Tables[0].Rows[i]["TransportBuiltyAmt"]) + Convert.ToDecimal(ds1.Tables[0].Rows[i]["UnloadingExpenses"])) / Convert.ToDecimal(ds1.Tables[0].Rows[i]["ACTUALRECQTY"]));
                //sht.Range("Y" + row).SetValue(Math.Round((GST) / Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"]),2));
                //sht.Range("Z" + row).SetValue(Math.Round((IGST) / Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"]),2));
                //sht.Range("AA" + row).SetValue(Math.Round((Convert.ToDecimal(ds.Tables[0].Rows[i]["TransportBuiltyAmt"]) + Convert.ToDecimal(ds.Tables[0].Rows[i]["UnloadingExpenses"])) / Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"]),2));
                //sht.Range("AB" + row).SetValue(Math.Round((Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) + GST + IGST + Convert.ToDecimal(ds.Tables[0].Rows[i]["TransportBuiltyAmt"]) + Convert.ToDecimal(ds.Tables[0].Rows[i]["UnloadingExpenses"])) / Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"]), 2));

                if (ds.Tables[0].Rows[i]["SGST"].ToString() != "0")
                {
                    sht.Range("Y" + row).SetValue(Math.Round((Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["SGST"]) / 100), 2));
                }
                else if (ds.Tables[0].Rows[i]["IGST"].ToString() != "0")
                {
                    sht.Range("Y" + row).SetValue(Math.Round((Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["IGST"]) / 100), 2));
                }
                else
                {
                    sht.Range("Y" + row).SetValue(0);
                }

                // sht.Range("Z" + row).SetValue(Math.Round((Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["IGST"])/100), 2));
                // sht.Range("X" + row).SetValue(Math.Round((Convert.ToDecimal(ds.Tables[0].Rows[i]["TransportBuiltyAmt"]) / Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"])), 2));

                if (ds.Tables[0].Rows[i]["ACTUALRECQTY"].ToString() != "0")
                {
                    sht.Range("Z" + row).SetValue(Math.Round(((Convert.ToDecimal(ds.Tables[0].Rows[i]["TransportBuiltyAmt"]) / Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"])) + (Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["SGST"]) / 100) + (Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["IGST"]) / 100) + Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) + Convert.ToDecimal(ds.Tables[0].Rows[i]["UnloadingExpenses"])), 2));
                }
                else
                {
                    sht.Range("Z" + row).SetValue(0);
                }

                //sht.Range("Z" + row).SetValue(Math.Round(((Convert.ToDecimal(ds.Tables[0].Rows[i]["TransportBuiltyAmt"]) / Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"])) + (Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["SGST"]) / 100) + (Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["IGST"]) / 100) + Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) + Convert.ToDecimal(ds.Tables[0].Rows[i]["UnloadingExpenses"])), 2));

                using (var a = sht.Range("A" + row + ":AB" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;
            }
            ////TOTAL
            //sht.Range("N" + row).Value = "Total";
            //sht.Range("O" + row).FormulaA1 = "=SUM(O" + Rowfrom + ":$O$" + (row - 1) + ")";
            ////sht.Range("P" + row).FormulaA1 = "=SUM(P" + Rowfrom + ":$P$" + (row - 1) + ")";
            //sht.Range("Q" + row).FormulaA1 = "=SUM(Q" + Rowfrom + ":$Q$" + (row - 1) + ")";
            //sht.Range("R" + row).FormulaA1 = "=SUM(R" + Rowfrom + ":$R$" + (row - 1) + ")";
            //sht.Range("S" + row).FormulaA1 = "=SUM(S" + Rowfrom + ":$S$" + (row - 1) + ")";
            //sht.Range("T" + row).FormulaA1 = "=SUM(T" + Rowfrom + ":$T$" + (row - 1) + ")";
            //sht.Range("U" + row).FormulaA1 = "=SUM(U" + Rowfrom + ":$U$" + (row - 1) + ")";
            ////sht.Range("F" + row).FormulaA1 = "=SUM(F" + Rowfrom + ":$F$" + (row - 1) + ")";
            //sht.Range("N" + row + ":U" + row).Style.Font.SetBold();
            ////sht.Range("O" + row ).Style.Font.SetBold();
            ////sht.Range("Q" + row).Style.Font.SetBold();                                

            //using (var a = sht.Range("L" + row + ":AC" + row))
            //{
            //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;                    
            //}                

            row = row + 2;
            ds.Dispose();

            ////*************GRAND TOTAL
            //sht.Range("N" + row + ":U" + row).Style.Font.SetBold();
            //sht.Range("N" + row).Value = "Grand Total";
            //sht.Range("O" + row).SetValue(TBillQty);
            //sht.Range("Q" + row).SetValue(TQty);
            //sht.Range("R" + row).SetValue(TBillAmt);
            //sht.Range("S" + row).SetValue(TAmount);
            //sht.Range("T" + row).SetValue(TTransportAmt);
            //sht.Range("U" + row).SetValue(TUnloadingAmt);
            ////*************
            sht.Columns(1, 30).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PurchaseMaterialReceive_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "purchaserec", "alert('No Record Found!');", true);

        }
    }
    protected void PurchaseReceiveDetailByChallanNo()
    {
        #region Where Condition
        string Where = "";
        string filterby = "From : " + TxtFRDate.Text + "  To : " + TxtTODate.Text;
        Where = Where + " and Prm.Receivedate>='" + TxtFRDate.Text + "' and PRM.Receivedate<='" + TxtTODate.Text + "'";
        if (ddcustomer.SelectedIndex > 0)
        {
            Where = Where + " And VP.Customerid=" + ddcustomer.SelectedValue;
            filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
        }
        if (ddOrderno.SelectedIndex > 0)
        {
            Where = Where + " And VP.orderid=" + ddOrderno.SelectedValue;
            filterby = filterby + " Order No : " + ddOrderno.SelectedItem.Text;
        }
        if (dsuppl.SelectedIndex > 0)
        {
            Where = Where + " And EI.Empid=" + dsuppl.SelectedValue;
            filterby = filterby + " Supp Name : " + dsuppl.SelectedItem.Text;
        }
        if (DDPONo.SelectedIndex > 0)
        {
            Where = Where + " And PRD.pindentissueid=" + DDPONo.SelectedValue;
            filterby = filterby + " PO No : " + DDPONo.SelectedItem.Text;
        }
        if (TRRecChallanNo.Visible == true)
        {
            if (txtRecChallanNo.Text != "0" && txtRecChallanNo.Text != "")
            {
                Where = Where + " And PRM.BillNo1='" + txtRecChallanNo.Text + "'";
                filterby = filterby + " BillNo : '" + txtRecChallanNo.Text + "'";
            }
        }
        if (ddCatagory.SelectedIndex > 0)
        {
            Where = Where + " And vf.CATEGORY_ID=" + ddCatagory.SelectedValue;
            filterby = filterby + " Category : " + ddCatagory.SelectedItem.Text;
        }
        if (dditemname.SelectedIndex > 0)
        {
            Where = Where + " And vf.item_id=" + dditemname.SelectedValue;
            filterby = filterby + " Item : " + dditemname.SelectedItem.Text;
        }
        if (dquality.SelectedIndex > 0)
        {
            Where = Where + " And vf.qualityid=" + dquality.SelectedValue;
            filterby = filterby + " Quality : " + dquality.SelectedItem.Text;
        }
        if (dddesign.SelectedIndex > 0)
        {
            Where = Where + " And vf.designid=" + dddesign.SelectedValue;
            filterby = filterby + " design : " + dddesign.SelectedItem.Text;
        }
        if (ddcolor.SelectedIndex > 0)
        {
            Where = Where + " And vf.colorid=" + ddcolor.SelectedValue;
            filterby = filterby + " Color : " + ddcolor.SelectedItem.Text;
        }
        if (ddshape.SelectedIndex > 0)
        {
            Where = Where + " And vf.shapeid=" + ddshape.SelectedValue;
            filterby = filterby + " shape : " + ddshape.SelectedItem.Text;
        }
        if (ddsize.SelectedIndex > 0)
        {
            Where = Where + " And vf.sizeid=" + ddsize.SelectedValue;
        }
        if (ddlshade.SelectedIndex > 0)
        {
            Where = Where + " And vf.shadecolorid=" + ddlshade.SelectedValue;
            filterby = filterby + " Shadecolor : " + ddlshade.SelectedItem.Text;
        }
        #endregion
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@CompanyId", ddCompName.SelectedValue);
        param[1] = new SqlParameter("@Where", Where);
        param[2] = new SqlParameter("@MasterCompanyId", Session["VarCompanyNo"]);
        param[3] = new SqlParameter("@UserID", Session["VarUserId"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETPURCHASERECEIVEDETAILSBYCHALLANNO", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            Decimal TQty = 0, TAmount = 0;
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 0;
            //***********
            sht.Range("A1:I1").Merge();
            sht.Range("A1").Value = "Purchase Received Details " + "For - " + ddCompName.SelectedItem.Text;
            sht.Range("A2:I2").Merge();
            sht.Range("A2").Value = "Filter By :  " + filterby;
            sht.Row(2).Height = 30;
            sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:I2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A1:I2").Style.Font.Bold = true;
            //***********Filter By Item_Name
            row = 3;
            DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "Item_Name");
            foreach (DataRow dr in dtdistinct.Rows)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "Item_Name='" + dr["Item_Name"] + "'";
                dv.Sort = "Receivedate";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());
                //******Headers
                sht.Range("A" + row).SetValue(dr["Item_Name"]);
                sht.Range("A" + row).Style.Font.SetBold();
                sht.Range("A" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                row = row + 1;
                sht.Range("A" + row).Value = "Vendor Name";
                sht.Range("B" + row).Value = "Quality";
                sht.Range("C" + row).Value = "Rec.Date";
                sht.Range("D" + row).Value = "PO No";
                sht.Range("E" + row).Value = "Bill No";

                sht.Range("F" + row).Value = "Vendor Lot No.";
                sht.Range("G" + row).Value = "Internal Lot No.";

                sht.Range("H" + row).Value = "Rec.Qty";
                sht.Range("I" + row).Value = "Rate";
                sht.Range("J" + row).Value = "Amount";
                sht.Range("K" + row).Value = "Godown Name";
                if (Session["VarCompanyNo"].ToString() == "21")
                {
                    sht.Range("L" + row).Value = "GateInNo";
                }
                else
                {
                    sht.Range("L" + row).Value = "";
                }

                sht.Range("H" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A" + row + ":K" + row).Style.Font.SetBold();
                //******
                row = row + 1;
                int Rowfrom = 0;
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    if (Rowfrom == 0)
                    {
                        Rowfrom = row;
                    }
                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["empname"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["QUALITYNAME"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["RECDATE"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["PONo"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["BillNo1"]);

                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["VendorLotno"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["ACTUALRECQTY"]);
                    TQty = TQty + Convert.ToDecimal(ds1.Tables[0].Rows[i]["ACTUALRECQTY"]);
                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);
                    sht.Range("J" + row).FormulaA1 = "=H" + row + '*' + ("$I$" + row + "");
                    TAmount = TAmount + (Convert.ToDecimal(ds1.Tables[0].Rows[i]["ACTUALRECQTY"]) * Convert.ToDecimal(ds1.Tables[0].Rows[i]["Rate"]));
                    sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["GodownName"]);

                    if (Session["VarCompanyNo"].ToString() == "21")
                    {
                        sht.Range("L" + row).Value = ds1.Tables[0].Rows[i]["GateInNo"];
                    }
                    else
                    {
                        sht.Range("L" + row).Value = "";
                    }
                    row = row + 1;
                }
                //TOTAL

                sht.Range("G" + row).Value = "Total";
                sht.Range("H" + row).FormulaA1 = "=SUM(H" + Rowfrom + ":$H$" + (row - 1) + ")";
                sht.Range("J" + row).FormulaA1 = "=SUM(J" + Rowfrom + ":$J$" + (row - 1) + ")";
                sht.Range("H" + row + ":J" + row).Style.Font.SetBold();
                row = row + 2;
                ds1.Dispose();
            }
            //*************GRAND TOTAL
            sht.Range("H" + row + ":J" + row).Style.Font.SetBold();
            sht.Range("G" + row).Value = "Grand Total";
            sht.Range("H" + row).SetValue(TQty);
            sht.Range("J" + row).SetValue(TAmount);
            //*************
            sht.Columns(1, 12).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PurchaseReceiveDetailsByChallan_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();


        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "purchaserec", "alert('No Record Found!');", true);

        }


    }
    protected void PurchaseordervendorwiseLotBillDetailagni()
    {

        #region Where Condition
        string Where = "";
        string filterby = "From : " + TxtFRDate.Text + "  To : " + TxtTODate.Text;
        Where = Where + " and vo.orderdate>='" + TxtFRDate.Text + "' and vo.orderdate<='" + TxtTODate.Text + "'";
        if (ddCompName.SelectedIndex > 0)
        {
            Where = Where + " And vo.companyid=" + ddCompName.SelectedValue;
            filterby = filterby + " Company : " + ddCompName.SelectedItem.Text;
        }
        if (ddcustomer.SelectedIndex > 0)
        {
            Where = Where + " And vo.Customerid=" + ddcustomer.SelectedValue;
            filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
        }
        if (ddOrderno.SelectedIndex > 0)
        {
            Where = Where + " And vo.orderid=" + ddOrderno.SelectedValue;
            filterby = filterby + " Order No : " + ddOrderno.SelectedItem.Text;
        }
        if (dsuppl.SelectedIndex > 0)
        {
            Where = Where + " And vo.Empid=" + dsuppl.SelectedValue;
            filterby = filterby + " Supp Name : " + dsuppl.SelectedItem.Text;
        }
        if (DDPONo.SelectedIndex > 0)
        {
            Where = Where + " And vo.pindentissueid=" + DDPONo.SelectedValue;
            filterby = filterby + " PO No : " + DDPONo.SelectedItem.Text;
        }
        if (ddCatagory.SelectedIndex > 0)
        {
            Where = Where + " And vf.CATEGORY_ID=" + ddCatagory.SelectedValue;
            filterby = filterby + " Category : " + ddCatagory.SelectedItem.Text;
        }
        if (dditemname.SelectedIndex > 0)
        {
            Where = Where + " And vf.item_id=" + dditemname.SelectedValue;
            filterby = filterby + " Item : " + dditemname.SelectedItem.Text;
        }
        if (dquality.SelectedIndex > 0)
        {
            Where = Where + " And vf.qualityid=" + dquality.SelectedValue;
            filterby = filterby + " Quality : " + dquality.SelectedItem.Text;
        }
        if (dddesign.SelectedIndex > 0)
        {
            Where = Where + " And vf.designid=" + dddesign.SelectedValue;
            filterby = filterby + " design : " + dddesign.SelectedItem.Text;
        }
        if (ddcolor.SelectedIndex > 0)
        {
            Where = Where + " And vf.colorid=" + ddcolor.SelectedValue;
            filterby = filterby + " Color : " + ddcolor.SelectedItem.Text;
        }
        if (ddshape.SelectedIndex > 0)
        {
            Where = Where + " And vf.shapeid=" + ddshape.SelectedValue;
            filterby = filterby + " shape : " + ddshape.SelectedItem.Text;
        }
        if (ddsize.SelectedIndex > 0)
        {
            Where = Where + " And vf.sizeid=" + ddsize.SelectedValue;
        }
        if (ddlshade.SelectedIndex > 0)
        {
            Where = Where + " And vf.shadecolorid=" + ddlshade.SelectedValue;
            filterby = filterby + " Shadecolor : " + ddlshade.SelectedItem.Text;
        }
        #endregion
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@FromDate", TxtFRDate.Text);
        param[1] = new SqlParameter("@ToDate", TxtTODate.Text);
        param[2] = new SqlParameter("@Where", Where);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETPURCHASEORDERVENDORWISELOTBILLDETAIL", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 0;
            //***********
            sht.Range("A1:O1").Merge();
            sht.Range("A1").Value = "PURCHASE VENDOR WISE BILL NO";
            sht.Range("A2:O2").Merge();
            sht.Range("A2").Value = "Filter By :  " + filterby;
            sht.Range("A1:O1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:O2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:O2").Style.Font.Bold = true;
            //**********
            sht.Range("A3").Value = "Category";
            sht.Range("B3").Value = "Po No";
            sht.Range("C3").Value = "Po Status";
            sht.Range("D3").Value = "Po Date";
            sht.Range("E3").Value = "Supp. Name";
            sht.Range("F3").Value = "Item Name";
            sht.Range("G3").Value = "Rate";
            sht.Range("H3").Value = "PO Qty";
            sht.Range("G3:H3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("I3").Value = "Delv Date";
            sht.Range("J3").Value = "CanQty/Date";
            sht.Range("K3").Value = "RecDate";

            sht.Range("L3").Value = "ChallanNo";

            sht.Range("M3").Value = "LotNo";
            sht.Range("N3").Value = "Bill No";

            sht.Range("O3").Value = "Rec Qty";
            sht.Range("O3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("P3").Value = "Ret Date";
            sht.Range("Q3").Value = "Ret Qty";
            sht.Range("R3").Value = "Pending Qty";
            sht.Range("S3").Value = "Receive Remark";
            sht.Range("T3").Value = "Order Remark";
            sht.Range("U3").Value = "Order No.";
            sht.Range("V3").Value = "Customer Code";

            //if (Session["VarCompanyNo"].ToString() == "21")
            //{
            //    sht.Range("U3").Value = "GateIn No";
            //}
            //else
            //{
            //    sht.Range("U3").Value = "";
            //}

            sht.Range("Q3:V3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3:V3").Style.Font.SetBold();
            using (var a = sht.Range("A3:V3"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*******
            row = 4;
            DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "Pindentissueid", "finishedid");
            foreach (DataRow dr in dtdistinct.Rows)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "Pindentissueid=" + dr["Pindentissueid"] + " and Finishedid=" + dr["finishedid"];
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {

                    using (var a = sht.Range("A" + row + ":V" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Category_name"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Po"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Status"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Orderdate"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Empname"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Description"].ToString() + ds1.Tables[0].Rows[i]["Colour"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Orderqty"]);
                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Deliverydate"]);
                    sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Ordercanqty"]);
                    sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Recdate"]);

                    sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["ChallanNo"]);

                    sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["LotNo"]);
                    sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["BillNo1"]);

                    sht.Range("O" + row).SetValue(Convert.ToDecimal(ds1.Tables[0].Rows[i]["Recqty"]) + Convert.ToDecimal(ds1.Tables[0].Rows[i]["Qtyreturn"]));
                    sht.Range("P" + row).SetValue(ds1.Tables[0].Rows[i]["Returnchallan"]);
                    sht.Range("Q" + row).SetValue(ds1.Tables[0].Rows[i]["Qtyreturn"]);
                    //sht.Range("O" + row).SetValue("");
                    if (i == 0)
                    {
                        sht.Range("R" + row).FormulaA1 = "=H" + row + '+' + "$Q$" + row + '-' + "$O$" + row;
                    }
                    else
                    {
                        sht.Range("R" + row).FormulaA1 = "=R" + (row - 1) + '+' + "$Q$" + row + '-' + "$O$" + row;
                    }
                    sht.Range("S" + row).SetValue(ds1.Tables[0].Rows[i]["PurchaseReceiveItemRemark"]);
                    sht.Range("T" + row).SetValue(ds1.Tables[0].Rows[i]["PurchaseOrderMasterRemark"]);
                    sht.Range("U" + row).SetValue(ds1.Tables[0].Rows[i]["ORDERNO"]);
                    sht.Range("V" + row).SetValue(ds1.Tables[0].Rows[i]["customercode"]);

                    if (Session["VarCompanyNo"].ToString() == "21")
                    {
                        sht.Range("U" + row).Value = ds1.Tables[0].Rows[i]["GateInNo"];
                    }
                    else
                    {
                        sht.Range("U" + row).Value = "";
                    }

                    row = row + 1;
                }
                ds1.Dispose();
            }
            //*************
            sht.Columns(1, 32).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PurchaseOrderVendorwiseLotBillDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altP", "alert('No records found...')", true);
        }
    }
    protected void PurchaseordervendorwiseLotBillDetail()
    {

        #region Where Condition
        string Where = "";
        string filterby = "From : " + TxtFRDate.Text + "  To : " + TxtTODate.Text;
        Where = Where + " and vo.orderdate>='" + TxtFRDate.Text + "' and vo.orderdate<='" + TxtTODate.Text + "'";
        if (ddCompName.SelectedIndex > 0)
        {
            Where = Where + " And vo.companyid=" + ddCompName.SelectedValue;
            filterby = filterby + " Company : " + ddCompName.SelectedItem.Text;
        }
        if (ddcustomer.SelectedIndex > 0)
        {
            Where = Where + " And vo.Customerid=" + ddcustomer.SelectedValue;
            filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
        }
        if (ddOrderno.SelectedIndex > 0)
        {
            Where = Where + " And vo.orderid=" + ddOrderno.SelectedValue;
            filterby = filterby + " Order No : " + ddOrderno.SelectedItem.Text;
        }
        if (dsuppl.SelectedIndex > 0)
        {
            Where = Where + " And vo.Empid=" + dsuppl.SelectedValue;
            filterby = filterby + " Supp Name : " + dsuppl.SelectedItem.Text;
        }
        if (DDPONo.SelectedIndex > 0)
        {
            Where = Where + " And vo.pindentissueid=" + DDPONo.SelectedValue;
            filterby = filterby + " PO No : " + DDPONo.SelectedItem.Text;
        }
        if (ddCatagory.SelectedIndex > 0)
        {
            Where = Where + " And vf.CATEGORY_ID=" + ddCatagory.SelectedValue;
            filterby = filterby + " Category : " + ddCatagory.SelectedItem.Text;
        }
        if (dditemname.SelectedIndex > 0)
        {
            Where = Where + " And vf.item_id=" + dditemname.SelectedValue;
            filterby = filterby + " Item : " + dditemname.SelectedItem.Text;
        }
        if (dquality.SelectedIndex > 0)
        {
            Where = Where + " And vf.qualityid=" + dquality.SelectedValue;
            filterby = filterby + " Quality : " + dquality.SelectedItem.Text;
        }
        if (dddesign.SelectedIndex > 0)
        {
            Where = Where + " And vf.designid=" + dddesign.SelectedValue;
            filterby = filterby + " design : " + dddesign.SelectedItem.Text;
        }
        if (ddcolor.SelectedIndex > 0)
        {
            Where = Where + " And vf.colorid=" + ddcolor.SelectedValue;
            filterby = filterby + " Color : " + ddcolor.SelectedItem.Text;
        }
        if (ddshape.SelectedIndex > 0)
        {
            Where = Where + " And vf.shapeid=" + ddshape.SelectedValue;
            filterby = filterby + " shape : " + ddshape.SelectedItem.Text;
        }
        if (ddsize.SelectedIndex > 0)
        {
            Where = Where + " And vf.sizeid=" + ddsize.SelectedValue;
        }
        if (ddlshade.SelectedIndex > 0)
        {
            Where = Where + " And vf.shadecolorid=" + ddlshade.SelectedValue;
            filterby = filterby + " Shadecolor : " + ddlshade.SelectedItem.Text;
        }
        #endregion
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@FromDate", TxtFRDate.Text);
        param[1] = new SqlParameter("@ToDate", TxtTODate.Text);
        param[2] = new SqlParameter("@Where", Where);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETPURCHASEORDERVENDORWISELOTBILLDETAIL", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 0;
            //***********
            sht.Range("A1:O1").Merge();
            sht.Range("A1").Value = "PURCHASE VENDOR WISE BILL NO";
            sht.Range("A2:O2").Merge();
            sht.Range("A2").Value = "Filter By :  " + filterby;
            sht.Range("A1:O1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:O2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:O2").Style.Font.Bold = true;
            //**********
            sht.Range("A3").Value = "Category";
            sht.Range("B3").Value = "Po No";
            sht.Range("C3").Value = "Po Status";
            sht.Range("D3").Value = "Po Date";
            sht.Range("E3").Value = "Supp. Name";
            sht.Range("F3").Value = "Item Name";
            sht.Range("G3").Value = "Rate";
            sht.Range("H3").Value = "PO Qty";
            sht.Range("G3:H3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("I3").Value = "Delv Date";
            sht.Range("J3").Value = "CanQty/Date";
            sht.Range("K3").Value = "RecDate";

            sht.Range("L3").Value = "ChallanNo";

            sht.Range("M3").Value = "LotNo";
            sht.Range("N3").Value = "Bill No";

            sht.Range("O3").Value = "Rec Qty";
            sht.Range("O3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("P3").Value = "Ret Date";
            sht.Range("Q3").Value = "Ret Qty";
            sht.Range("R3").Value = "Pending Qty";
            sht.Range("S3").Value = "Receive Remark";
            sht.Range("T3").Value = "Order Remark";

            if (Session["VarCompanyNo"].ToString() == "21")
            {
                sht.Range("U3").Value = "GateIn No";
            }
            else
            {
                sht.Range("U3").Value = "";
            }

            sht.Range("Q3:T3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3:T3").Style.Font.SetBold();
            using (var a = sht.Range("A3:T3"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*******
            row = 4;
            DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "Pindentissueid", "finishedid");
            foreach (DataRow dr in dtdistinct.Rows)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "Pindentissueid=" + dr["Pindentissueid"] + " and Finishedid=" + dr["finishedid"];
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {

                    using (var a = sht.Range("A" + row + ":T" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Category_name"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Po"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Status"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Orderdate"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Empname"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Description"].ToString() + ds1.Tables[0].Rows[i]["Colour"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Orderqty"]);
                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Deliverydate"]);
                    sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Ordercanqty"]);
                    sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Recdate"]);

                    sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["ChallanNo"]);

                    sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["LotNo"]);
                    sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["BillNo1"]);

                    sht.Range("O" + row).SetValue(Convert.ToDecimal(ds1.Tables[0].Rows[i]["Recqty"]) + Convert.ToDecimal(ds1.Tables[0].Rows[i]["Qtyreturn"]));
                    sht.Range("P" + row).SetValue(ds1.Tables[0].Rows[i]["Returnchallan"]);
                    sht.Range("Q" + row).SetValue(ds1.Tables[0].Rows[i]["Qtyreturn"]);
                    //sht.Range("O" + row).SetValue("");
                    if (i == 0)
                    {
                        sht.Range("R" + row).FormulaA1 = "=H" + row + '+' + "$Q$" + row + '-' + "$O$" + row;
                    }
                    else
                    {
                        sht.Range("R" + row).FormulaA1 = "=R" + (row - 1) + '+' + "$Q$" + row + '-' + "$O$" + row;
                    }
                    sht.Range("S" + row).SetValue(ds1.Tables[0].Rows[i]["PurchaseReceiveItemRemark"]);
                    sht.Range("T" + row).SetValue(ds1.Tables[0].Rows[i]["PurchaseOrderMasterRemark"]);

                    if (Session["VarCompanyNo"].ToString() == "21")
                    {
                        sht.Range("U" + row).Value = ds1.Tables[0].Rows[i]["GateInNo"];
                    }
                    else
                    {
                        sht.Range("U" + row).Value = "";
                    }

                    row = row + 1;
                }
                ds1.Dispose();
            }
            //*************
            sht.Columns(1, 32).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PurchaseOrderVendorwiseLotBillDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altP", "alert('No records found...')", true);
        }
    }
    protected void PurchaseMaterialReceiveIssueHafiziaAbbaReport()
    {
        #region Where Condition
        string Where = "";
        string filterby = "From : " + TxtFRDate.Text + "  To : " + TxtTODate.Text;
        Where = Where + " and PRM.Receivedate>='" + TxtFRDate.Text + "' and PRM.Receivedate<='" + TxtTODate.Text + "'";
        if (ddcustomer.SelectedIndex > 0)
        {
            Where = Where + " And VP.Customerid=" + ddcustomer.SelectedValue;
            filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
        }
        if (ddOrderno.SelectedIndex > 0)
        {
            Where = Where + " And VP.orderid=" + ddOrderno.SelectedValue;
            filterby = filterby + " Order No : " + ddOrderno.SelectedItem.Text;
        }
        if (dsuppl.SelectedIndex > 0)
        {
            Where = Where + " And EI.Empid=" + dsuppl.SelectedValue;
            filterby = filterby + " Supp Name : " + dsuppl.SelectedItem.Text;
        }
        if (DDPONo.SelectedIndex > 0)
        {
            Where = Where + " And PRD.pindentissueid=" + DDPONo.SelectedValue;
            filterby = filterby + " PO No : " + DDPONo.SelectedItem.Text;
        }
        if (ddCatagory.SelectedIndex > 0)
        {
            Where = Where + " And vf.CATEGORY_ID=" + ddCatagory.SelectedValue;
            filterby = filterby + " Category : " + ddCatagory.SelectedItem.Text;
        }
        if (dditemname.SelectedIndex > 0)
        {
            Where = Where + " And vf.item_id=" + dditemname.SelectedValue;
            filterby = filterby + " Item : " + dditemname.SelectedItem.Text;
        }
        if (dquality.SelectedIndex > 0)
        {
            Where = Where + " And vf.qualityid=" + dquality.SelectedValue;
            filterby = filterby + " Quality : " + dquality.SelectedItem.Text;
        }
        if (dddesign.SelectedIndex > 0)
        {
            Where = Where + " And vf.designid=" + dddesign.SelectedValue;
            filterby = filterby + " design : " + dddesign.SelectedItem.Text;
        }
        if (ddcolor.SelectedIndex > 0)
        {
            Where = Where + " And vf.colorid=" + ddcolor.SelectedValue;
            filterby = filterby + " Color : " + ddcolor.SelectedItem.Text;
        }
        if (ddshape.SelectedIndex > 0)
        {
            Where = Where + " And vf.shapeid=" + ddshape.SelectedValue;
            filterby = filterby + " shape : " + ddshape.SelectedItem.Text;
        }
        if (ddsize.SelectedIndex > 0)
        {
            Where = Where + " And vf.sizeid=" + ddsize.SelectedValue;
        }
        if (ddlshade.SelectedIndex > 0)
        {
            Where = Where + " And vf.shadecolorid=" + ddlshade.SelectedValue;
            filterby = filterby + " Shadecolor : " + ddlshade.SelectedItem.Text;
        }
        #endregion
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@CompanyId", ddCompName.SelectedValue);
        param[1] = new SqlParameter("@FromDate", TxtFRDate.Text);
        param[2] = new SqlParameter("@ToDate", TxtTODate.Text);
        param[3] = new SqlParameter("@Where", Where);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETPURCHASERECEIVEDETAILS_HAFIZIA_FINAL_REPORT", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            Decimal TQty = 0, TAmount = 0, TBillQty = 0, TBillAmt = 0, TTransportAmt = 0, TUnloadingAmt = 0, ActualRecQty = 0;
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 0;
            //***********
            sht.Range("A1:N1").Merge();
            //sht.Range("A1").Value = "Purchase Material Received " + "For - " + ddCompName.SelectedItem.Text;
            sht.Range("A1").Value = "Final Report Purchase " + "For - " + ddCompName.SelectedItem.Text;
            sht.Range("A2:N2").Merge();
            sht.Range("A2").Value = "Filter By :  " + filterby;
            sht.Row(2).Height = 30;
            sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:N2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A1:N2").Style.Font.Bold = true;
            //***********Show Data
            row = 3;

            sht.Range("A" + row).Value = "Yarn Detail";
            sht.Range("B" + row).Value = "Yarn Count/Ply";
            sht.Range("C" + row).Value = "";
            sht.Range("D" + row).Value = "Shade Color";
            sht.Range("E" + row).Value = "Lot No";
            sht.Range("F" + row).Value = "OP Stock ";
            sht.Range("G" + row).Value = "Issued";
            sht.Range("H" + row).Value = "Closing Bal";
            sht.Range("I" + row).Value = "Company";

            sht.Range("J" + row).Value = "Receive Date";
            sht.Range("K" + row).Value = "Rate & Freight";

            sht.Range("L" + row).Value = "Transport Name";
            sht.Range("M" + row).Value = "Rack No";
            sht.Range("N" + row).Value = "10 Mtr Weight";
            sht.Range("O" + row).Value = "Return Qty";
            sht.Range("P" + row).Value = "Remark";

            sht.Column(3).Hide();


            //sht.Range("O" + row + ":AA" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A" + row + ":P" + row).Style.Font.SetBold();
            using (var a = sht.Range("A" + row + ":P" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //******
            row = row + 1;
            int Rowfrom = 0;
            string TempQuality = "";

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                if (i == 0)
                {
                    TempQuality = ds.Tables[0].Rows[i]["QUALITYNAME"].ToString();
                }

                if (TempQuality != ds.Tables[0].Rows[i]["QUALITYNAME"].ToString())
                {
                    sht.Range("A" + row).Value = "";
                    sht.Range("A" + row + ":P" + row).Merge();
                    sht.Row(row).Height = 12;
                    sht.Range("A" + row + ":P" + row).Style.Fill.BackgroundColor = XLColor.Black;
                    sht.Range("A" + row + ":P" + row).Style.Font.Bold = true;
                    TempQuality = ds.Tables[0].Rows[i]["QUALITYNAME"].ToString();
                    row = row + 1;
                }
                
                decimal GST = 0, IGST = 0;
                if (Rowfrom == 0)
                {
                    Rowfrom = row;
                }

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                //sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["designName"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["LotNo"]);
                ////ActualRecQty = Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"]) - Convert.ToDecimal(ds.Tables[0].Rows[i]["ProcessIndentReturnQty"]);
                ActualRecQty = Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"]);
                sht.Range("F" + row).SetValue(ActualRecQty);

                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["IssQty"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["CurrentStock"]);
                ////sht.Range("H" + row).SetValue(Convert.ToDecimal(ActualRecQty) - Convert.ToDecimal(ds.Tables[0].Rows[i]["IssQty"]));
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["RECDATE"]);
                if (ds.Tables[0].Rows[i]["SGST"].ToString() != "0")
                {
                    GST = (Convert.ToDecimal(ActualRecQty) * Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) * Convert.ToDecimal(Convert.ToDecimal(ds.Tables[0].Rows[i]["SGST"]) / 100));
                }
                if (ds.Tables[0].Rows[i]["IGST"].ToString() != "0")
                {
                    IGST = (Convert.ToDecimal(ActualRecQty) * Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) * Convert.ToDecimal(Convert.ToDecimal(ds.Tables[0].Rows[i]["IGST"]) / 100));
                }
                if (Session["varcompanyid"].ToString() == "9")
                {

                    sht.Range("K" + row).SetValue(Math.Round(((Convert.ToDecimal(ds.Tables[0].Rows[i]["TransportBuiltyAmt"]) / Convert.ToDecimal(ActualRecQty)) + (Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["SGST"]) / 100) + (Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["IGST"]) / 100) + Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) + Convert.ToDecimal(ds.Tables[0].Rows[i]["UnloadingExpenses"])), 2));
                }
                else
                {
                    sht.Range("K" + row).SetValue(Math.Round((Convert.ToDecimal(ActualRecQty) * Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]) + GST + IGST + Convert.ToDecimal(ds.Tables[0].Rows[i]["TransportBuiltyAmt"]) + Convert.ToDecimal(ds.Tables[0].Rows[i]["UnloadingExpenses"])) / Convert.ToDecimal(ActualRecQty), 2, MidpointRounding.AwayFromZero));
                }

                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["TransportName"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["BinNo"]);

                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["FiftyMWait"]);
                sht.Range("O" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[i]["RETURNQTY"]) + Convert.ToDecimal(ds.Tables[0].Rows[i]["ProcessIndentReturnQty"]));
                sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["ItemRemark"]);

                using (var a = sht.Range("A" + row + ":P" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }                

                
                row = row + 1;
            }

            row = row + 1;
            ds.Dispose();

            ////*************GRAND TOTAL
            //sht.Range("N" + row + ":U" + row).Style.Font.SetBold();
            //sht.Range("N" + row).Value = "Grand Total";
            //sht.Range("O" + row).SetValue(TBillQty);
            //sht.Range("Q" + row).SetValue(TQty);
            //sht.Range("R" + row).SetValue(TBillAmt);
            //sht.Range("S" + row).SetValue(TAmount);
            //sht.Range("T" + row).SetValue(TTransportAmt);
            //sht.Range("U" + row).SetValue(TUnloadingAmt);
            ////*************
            sht.Columns(1, 30).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PurchaseMaterialReceiveIssueFinalReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "purchaserec", "alert('No Record Found!');", true);

        }
    }

    protected void RDPurchaseMaterialRecPending_CheckedChanged(object sender, EventArgs e)
    {
        if (RDPurchaseMaterialRecPending.Checked == true)
        {
            PurDelivCheckedChange();
            trcustomer.Visible = true;
            trorder.Visible = true;
            trsupply.Visible = false;
            TrStatus.Visible = false;
            Fill_Category();
            TRPurchaseSumm.Visible = false;
            Trgodown.Visible = false;
            TRFinalAbbaReport.Visible = false;
            trChkForDate.Visible = false;
            trfr.Visible = false;
            trto.Visible = false;

            Tr3.Visible = true;
            TrItemName.Visible = true;
            ql.Visible = true;
            TRASOnDate.Visible = true;
        }
    }

    private void Fill_Category()
    {
        UtilityModule.ConditionalComboFill(ref ddCatagory, "select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER  Where MasterCompanyid=" + Session["varcompanyid"] + " order by CATEGORY_NAME", true, "--Plz Select--");
    }
    protected void PurchaseMaterialRecPendingAsOnDate()
    {
        DataSet ds = new DataSet();
        #region Where Condition
        string Where = "";
        string filterby = "";
        //string filterby = "From : " + TxtFRDate.Text + "  To : " + TxtTODate.Text;
        //Where = Where + " and Prm.Receivedate>='" + TxtFRDate.Text + "' and PRM.Receivedate<='" + TxtTODate.Text + "'";
        if (ddcustomer.SelectedIndex > 0)
        {
            Where = Where + " And CII.Customerid=" + ddcustomer.SelectedValue;
            filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
        }
        if (ddOrderno.SelectedIndex > 0)
        {
            Where = Where + " And OM.orderid=" + ddOrderno.SelectedValue;
            filterby = filterby + " Order No : " + ddOrderno.SelectedItem.Text;
        }

        if (Tr3.Visible == true && ddCatagory.SelectedIndex > 0)
        {
            Where = Where + " And VF.CATEGORY_ID = " + ddCatagory.SelectedValue;
            filterby = filterby + " Category Name : " + ddCatagory.SelectedItem.Text;
        }
        if (TrItemName.Visible == true && dditemname.SelectedIndex > 0)
        {
            Where = Where + " And VF.ITEM_ID = " + dditemname.SelectedValue;
            filterby = filterby + " Item Name : " + dditemname.SelectedItem.Text;
        }
        if (ql.Visible == true && dquality.SelectedIndex > 0)
        {
            Where = Where + " And VF.QualityId = " + dquality.SelectedValue;
            filterby = filterby + " Quality Name : " + dquality.SelectedItem.Text;
        }
        if (shd.Visible == true && ddlshade.SelectedIndex > 0)
        {
            Where = Where + " And VF.ShadecolorId = " + ddlshade.SelectedValue;
            filterby = filterby + " Shade Color Name : " + ddlshade.SelectedItem.Text;
        }

        #endregion

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GETPURCHASEMATERIALBALACEASONDATE", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", ddCompName.SelectedValue);
        cmd.Parameters.AddWithValue("@Where", Where);
        cmd.Parameters.AddWithValue("@AsOnDate", txtAsOnDate.Text);

        // DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            Decimal TQty = 0, TAmount = 0;
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 0;
            //***********
            sht.Range("A1:I1").Merge();
            sht.Range("A1").Value = "Purchase Material Rec Pending " + "For - " + ddCompName.SelectedItem.Text;
            sht.Range("A2:I2").Merge();
            //sht.Range("A2").Value = "Filter By :  " + filterby;
            //sht.Row(2).Height = 30;
            sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:I2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A1:I2").Style.Font.Bold = true;
            //***********Filter By Item_Name
            row = 3;



            sht.Column("A").Width = 20.22;
            sht.Column("B").Width = 20.22;
            sht.Column("C").Width = 25.22;
            sht.Column("D").Width = 15.22;
            sht.Column("E").Width = 15.22;
            sht.Column("F").Width = 20.22;
            sht.Column("G").Width = 20.22;
            sht.Column("H").Width = 20.22;
            sht.Column("I").Width = 20.22;
            sht.Column("J").Width = 15.22;
            sht.Column("K").Width = 15.22;
            sht.Column("L").Width = 20.22;

            //******Headers          
            sht.Range("A" + row).Value = "Customer Code";
            sht.Range("B" + row).Value = "Customer OrderNo";

            sht.Range("D" + row).Value = "PO No";
            sht.Range("E" + row).Value = "PO Date";
            sht.Range("F" + row).Value = "Due Date";


            sht.Range("H" + row).Value = "Vendor Name";
            sht.Range("I" + row).Value = "Category";
            sht.Range("J" + row).Value = "Item Name";
            sht.Range("K" + row).Value = "Quality";
            sht.Range("L" + row).Value = "ShadeColor";

            sht.Range("M" + row).Value = "Order Qty";

            sht.Range("N" + row).Value = "Rec.Qty";
            sht.Range("O" + row).Value = "Pending Qty";
            sht.Range("M" + row + ":O" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A" + row + ":O" + row).Style.Font.SetBold();

            if (Session["varCompanyId"].ToString() == "16" || Session["varCompanyId"].ToString() == "28")
            {
                sht.Range("C" + row).Value = "OrderNo (Manual Filled by User)";
                sht.Range("G" + row).Value = "Pending Days";
            }
            else
            {
                sht.Column(3).Hide();
                sht.Column(7).Hide();
                sht.Range("C" + row).Value = "";
                sht.Range("G" + row).Value = "";
            }

            row = row + 1;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                sht.Range("A" + row).Style.Alignment.SetWrapText();
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                sht.Range("B" + row).Style.Alignment.SetWrapText();

                if (Session["varCompanyId"].ToString() == "16" || Session["varCompanyId"].ToString() == "28")
                {
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ManualOrderNo"]);
                    sht.Range("C" + row).Style.Alignment.SetWrapText();
                }
                else
                {
                    sht.Range("C" + row).SetValue("");
                    sht.Range("C" + row).Style.Alignment.SetWrapText();
                }


                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ChallanNo"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Date"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["DueDate"]);
                if (Session["varCompanyId"].ToString() == "16" || Session["varCompanyId"].ToString() == "28")
                {
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Difference"]);

                }
                else
                {
                    sht.Range("G" + row).SetValue("");
                }

                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                sht.Range("H" + row).Style.Alignment.SetWrapText();
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["CATEGORY_NAME"]);
                sht.Range("I" + row).Style.Alignment.SetWrapText();
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
                sht.Range("J" + row).Style.Alignment.SetWrapText();
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("K" + row).Style.Alignment.SetWrapText();

                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                sht.Range("L" + row).Style.Alignment.SetWrapText();

                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["IssueQty"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["RecQty"]);
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["PendingQty"]);

                row = row + 1;
            }
            ////*************
            //sht.Columns(1, 14).AdjustToContents();
            ////********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PurchaseMaterialBalanceAsOnDate_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();

        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "purchaserec", "alert('No Record Found!');", true);

        }


    }
    protected void RDPurchaseOrderReceiveBuyerCode_CheckedChanged(object sender, EventArgs e)
    {
        TrStatus.Visible = true;
        Trgodown.Visible = false;
        trChkForDate.Visible = true;
        Tr3.Visible = false;
        Fill_Category();
        TrItemName.Visible = false;
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        Trcomp.Visible = true;
        trcustomer.Visible = false;
        trorder.Visible = false;
        trfr.Visible = true;
        trto.Visible = true;
        trsupply.Visible = true;
        trPurchaseIndentChallanNo.Visible = false;
        lblPoNo.Text = "PO.No.";
        TRPurchaseSumm.Visible = false;
        trcustomer.Visible = true;
        trorder.Visible = true;
        TRRecChallanNo.Visible = false;
        txtRecChallanNo.Text = "";
        TRPurchaseDetailByChallan.Visible = false;
        TRLotBillDetail.Visible = false;
        TRFinalAbbaReport.Visible = false;
        TRASOnDate.Visible = false;

        if (Session["varcompanyid"].ToString() == "44")
        {
            UtilityModule.ConditionalComboFill(ref ddcustomer, @"select Distinct CI.CustomerId,CI.CustomerCode as Customercode from Ordermaster OM
                                                          inner join customerinfo CI on ci.CustomerId=OM.CustomerId order by Customercode", true, "Select CustomerCode");
        }
        else { 
          UtilityModule.ConditionalComboFill(ref ddcustomer, @"select Distinct CI.CustomerId,CI.CustomerCode+'/'+CI.CompanyName as Customercode from Ordermaster OM
                                                          inner join customerinfo CI on ci.CustomerId=OM.CustomerId order by Customercode", true, "Select CustomerCode");
        
        }

    }
    protected void PurchaseOrderReceiveOrderWise()
    {

        #region Where Condition
        string Where = "";
        string Where2 = "";
        //string filterby = "From : " + TxtFRDate.Text + "  To : " + TxtTODate.Text;
        //Where = Where + " and vo.orderdate>='" + TxtFRDate.Text + "' and vo.orderdate<='" + TxtTODate.Text + "'";

        if (ddcustomer.SelectedIndex > 0)
        {
            Where = Where + " And OM.Customerid=" + ddcustomer.SelectedValue;
            // filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
        }
        if (ddOrderno.SelectedIndex > 0)
        {
            Where = Where + " And OM.orderid=" + ddOrderno.SelectedValue;
            //filterby = filterby + " Order No : " + ddOrderno.SelectedItem.Text;
        }
        if (ChkForDate.Checked == true)
        {
            Where = Where + " and OM.orderdate>='" + TxtFRDate.Text + "' and OM.orderdate<='" + TxtTODate.Text + "'";
        }

        Where2 = Where2 + " Where IsNull(PIID.OrderID, 0) > 0";
        if (dsuppl.SelectedIndex > 0)
        {
            Where2 = Where2 + " and PII.Partyid=" + dsuppl.SelectedValue;
            //filterby = filterby + " Supp Name : " + dsuppl.SelectedItem.Text;
        }
        if (DDStatus.SelectedIndex > 0)
        {
            if (DDStatus.SelectedValue == "1")
            {
                Where2 = Where2 + " And PII.Status = 'COMPLETE'";
            }
            else if (DDStatus.SelectedValue == "2")
            {
                Where2 = Where2 + " And PII.Status = 'PENDING'";
            }
        }


        #endregion
        //SqlParameter[] param = new SqlParameter[3];
        //param[0] = new SqlParameter("@FromDate", TxtFRDate.Text);
        //param[1] = new SqlParameter("@ToDate", TxtTODate.Text);
        //param[2] = new SqlParameter("@Where", Where);

        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETPURCHASEORDERVENDORWISELOTBILLDETAIL", param);

        DataSet ds = new DataSet();

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GETPURCHASEORDER_RECEIVE_BUYERCODE_ORDERWISE", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", ddCompName.SelectedValue);
        cmd.Parameters.AddWithValue("@Where", Where);
        cmd.Parameters.AddWithValue("@Where2", Where2);

        // DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();


        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 0;
            //***********
            sht.Range("A1:Q1").Merge();
            sht.Range("A1").Value = "CUSTOMER ORDER WISE PURCHASE DETAIL";
            sht.Range("A2:Q2").Merge();
            //sht.Range("A2").Value = "Filter By :  " + filterby;
            sht.Range("A1:Q2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:Q2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:Q2").Style.Font.Bold = true;
            //**********

            sht.Range("A3").Value = "Customer Code";
            sht.Range("B3").Value = "Customer OrderNo";
            sht.Range("C3").Value = "Item Description";
            sht.Range("D3").Value = "Required Qty";
            sht.Range("E3").Value = "Vendor Name";
            sht.Range("F3").Value = "Contact Person";
            sht.Range("G3").Value = "MobileNo";
            sht.Range("H3").Value = "Purchase OrderNo";
            sht.Range("I3").Value = "Order Date";
            sht.Range("J3").Value = "Due Date  ";
            sht.Range("K3").Value = "Days In Hand";
            sht.Range("L3").Value = "PO Qty";
            sht.Range("M3").Value = "Received Qty";
            sht.Range("N3").Value = "LotNo";
            sht.Range("O3").Value = "Pending Qty";
            sht.Range("P3").Value = "Status";
            sht.Range("Q3").Value = "Order Remark";

            // sht.Range("P3:R3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3:Q3").Style.Font.SetBold();
            using (var a = sht.Range("A3:Q3"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*******
            row = 4;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"] + " " + ds.Tables[0].Rows[i]["QualityName"] + " " + ds.Tables[0].Rows[i]["DesignName"] + " " + ds.Tables[0].Rows[i]["ColorName"] + " " + ds.Tables[0].Rows[i]["ShadeColorName"] + " " + ds.Tables[0].Rows[i]["ShapeName"] + " " + ds.Tables[0].Rows[i]["SizeFt"]);
                sht.Range("C" + row).Style.Alignment.SetWrapText();
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ConsumptionQty"]);

                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ContactPerson"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Mobile"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["PurchaseOrderNo"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["PODate"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["PODueDate"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["DaysInHand"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["PurchaseQty"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["PurchaseReceiveQty"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["LotNo"]);
                sht.Range("O" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[i]["PurchaseQty"]) - Convert.ToDecimal(ds.Tables[0].Rows[i]["PurchaseReceiveQty"]));
                sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["Status"]);
                sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["PurchaseOrderMasterRemark"]);

                row = row + 1;
            }

            //*************
            sht.Columns(1, 31).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PurchaseOrderIssueReceiveOrderWise_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altP", "alert('No records found...')", true);
        }
    }
    protected void RDPurchaseMaterialIssueReceive_CheckedChanged(object sender, EventArgs e)
    {
        if (RDPurchaseMaterialIssueReceive.Checked == true)
        {
            TrStatus.Visible = false;
            Tr3.Visible = true;
            Trgodown.Visible = false;
            Fill_Category();
            TrItemName.Visible = true;
            ql.Visible = false;
            clr.Visible = false;
            dsn.Visible = false;
            shp.Visible = false;
            sz.Visible = false;
            shd.Visible = false;
            Trcomp.Visible = true;
            trcustomer.Visible = false;
            trorder.Visible = false;
            trfr.Visible = true;
            trto.Visible = true;
            trsupply.Visible = true;
            trPurchaseIndentChallanNo.Visible = false;
            lblPoNo.Text = "PO.No.";
            TRPurchaseSumm.Visible = false;
            trcustomer.Visible = false;
            trorder.Visible = false;
            TRRecChallanNo.Visible = false;
            txtRecChallanNo.Text = "";
            TRPurchaseDetailByChallan.Visible = false;
            TRLotBillDetail.Visible = false;
            TRFinalAbbaReport.Visible = false;
            TRPurchaseSumm.Visible = false;
            TRASOnDate.Visible = false;
        }
    }
    protected void PurchaseOrderIssRecHafizia()
    {
        DataSet ds = new DataSet();
        try
        {
            #region Where Condition
            string Where = "";
            string filterby = "From : " + TxtFRDate.Text + "  To : " + TxtTODate.Text;
            Where = Where + " and PII.Date>='" + TxtFRDate.Text + "' and PII.Date<='" + TxtTODate.Text + "'";

            if (dsuppl.SelectedIndex > 0)
            {
                Where = Where + " And EI.Empid=" + dsuppl.SelectedValue;
                filterby = filterby + " Supp Name : " + dsuppl.SelectedItem.Text;
            }
            if (ddCatagory.SelectedIndex > 0)
            {
                Where = Where + " And vf.CATEGORY_ID=" + ddCatagory.SelectedValue;
                filterby = filterby + " Category : " + ddCatagory.SelectedItem.Text;
            }
            if (dditemname.SelectedIndex > 0)
            {
                Where = Where + " And vf.item_id=" + dditemname.SelectedValue;
                filterby = filterby + " Item : " + dditemname.SelectedItem.Text;
            }
            if (dquality.SelectedIndex > 0)
            {
                Where = Where + " And vf.qualityid=" + dquality.SelectedValue;
                filterby = filterby + " Quality : " + dquality.SelectedItem.Text;
            }
            if (dddesign.SelectedIndex > 0)
            {
                Where = Where + " And vf.designid=" + dddesign.SelectedValue;
                filterby = filterby + " design : " + dddesign.SelectedItem.Text;
            }
            if (ddcolor.SelectedIndex > 0)
            {
                Where = Where + " And vf.colorid=" + ddcolor.SelectedValue;
                filterby = filterby + " Color : " + ddcolor.SelectedItem.Text;
            }
            if (ddshape.SelectedIndex > 0)
            {
                Where = Where + " And vf.shapeid=" + ddshape.SelectedValue;
                filterby = filterby + " shape : " + ddshape.SelectedItem.Text;
            }
            if (ddsize.SelectedIndex > 0)
            {
                Where = Where + " And vf.sizeid=" + ddsize.SelectedValue;
            }
            if (ddlshade.SelectedIndex > 0)
            {
                Where = Where + " And vf.shadecolorid=" + ddlshade.SelectedValue;
                filterby = filterby + " Shadecolor : " + ddlshade.SelectedItem.Text;
            }
            #endregion
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_PurchaseMaterialIssRecHafiziaReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@CompanyId", ddCompName.SelectedValue);
            cmd.Parameters.AddWithValue("@FromDate", TxtFRDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtTODate.Text);
            cmd.Parameters.AddWithValue("@Where", Where);
            //cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //SqlParameter[] param = new SqlParameter[1];
            //param[0] = new SqlParameter("@issueorderid", DDFolioNo.SelectedValue);

            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_WEAVERFOLIO_HAFIZIA_HINDI_REPORT", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("PurchaseMaterialIssRec");
                //int row = 0;

                sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                sht.PageSetup.AdjustTo(85);
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;

                //
                sht.PageSetup.Margins.Top = 0.41;
                sht.PageSetup.Margins.Left = 0.20;
                sht.PageSetup.Margins.Right = 0.10;
                sht.PageSetup.Margins.Bottom = 0.19;
                sht.PageSetup.Margins.Header = 1.50;
                sht.PageSetup.Margins.Footer = 0.3;
                sht.PageSetup.SetScaleHFWithDocument();

                sht.Column("A").Width = 24.11;
                sht.Column("B").Width = 17.11;
                sht.Column("C").Width = 21.22;
                sht.Column("D").Width = 18.22;
                sht.Column("E").Width = 20.33;
                sht.Column("F").Width = 24.33;
                sht.Column("G").Width = 12.67;
                sht.Column("H").Width = 18.33;

                //sht.ColumnWidth = 5.15;

                //sht.Row(1).Height = 30;
                //sht.Row(2).Height = 30;
                //sht.Row(3).Height = 30;
                //sht.Row(4).Height = 30;
                //sht.Row(5).Height = 30;
                //sht.Row(6).Height = 30;
                //sht.Row(7).Height = 30;
                //sht.Row(8).Height = 30;
                //sht.Row(9).Height = 30;
                //sht.Row(10).Height = 3;
                //sht.Row(11).Height = 18;
                //sht.Row(12).Height = 18;
                //sht.Row(13).Height = 18;
                //sht.Row(14).Height = 18;
                //sht.Row(15).Height = 18;
                //sht.Row(16).Height = 18;
                //sht.Row(17).Height = 18;
                //sht.Row(18).Height = 18;
                //sht.Row(19).Height = 20;

                sht.Range("A1:N1").Merge();
                sht.Range("A1").Value = "PURCHASE ORDER ISSUE RECEIVE";

                sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:N1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A1:N1").Style.Alignment.SetWrapText();
                sht.Range("A1:N1").Style.Font.FontName = "Calibri";
                sht.Range("A1:N1").Style.Font.FontSize = 18;
                sht.Range("A1:N1").Style.Font.Bold = true;
                //*******Header

                //sht.Range("A2").Value = ds.Tables[0].Rows[0]["CompanyName"];
                //sht.Range("A2:H2").Style.Font.FontName = "Calibri";
                //sht.Range("A2:H2").Style.Font.FontSize = 14;
                //sht.Range("A2:H2").Style.Font.SetBold();
                //sht.Range("A2:H2").Merge();
                //sht.Range("A2:H2").Style.Alignment.SetWrapText();
                //sht.Range("A2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //sht.Range("A2:H2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                //sht.Range("I2").Value = "Weaver Name & Address";
                //sht.Range("I2:K2").Style.Font.FontName = "Calibri";
                //sht.Range("I2:K2").Style.Font.FontSize = 11;
                //sht.Range("I2:K2").Style.Font.SetBold();
                ////sht.Range("A2").Merge();
                //sht.Range("I2:K2").Merge();
                //sht.Range("I2:K2").Style.Alignment.SetWrapText();
                //sht.Range("I2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Range("I2:K2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                //sht.Range("I2:K2").Style.Fill.BackgroundColor = XLColor.LightGray;                      

                sht.Row(2).Height = 51.6;

                sht.Range("A2").Value = "PURCHASE ORDER DATE";
                sht.Range("A2:A2").Style.Font.FontName = "Calibri";
                sht.Range("A2:A2").Style.Font.FontSize = 14;
                sht.Range("A2:A2").Style.Font.SetBold();
                sht.Range("A2:A2").Merge();
                sht.Range("A2:A2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:A2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A2:A2").Style.Alignment.SetWrapText();
                //sht.Range("A2:A2").Style.Fill.BackgroundColor = XLColor.LightGray;

                sht.Range("B2").Value = "SR NO.";
                sht.Range("B2:B2").Style.Font.FontName = "Calibri";
                sht.Range("B2:B2").Style.Font.FontSize = 14;
                sht.Range("B2:B2").Style.Font.SetBold();
                sht.Range("B2:B2").Merge();
                sht.Range("B2:B2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B2:B2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B2:B2").Style.Alignment.SetWrapText();
                //sht.Range("B2:B2").Style.Fill.BackgroundColor = XLColor.LightGray;

                sht.Range("C2").Value = "खरीदारी क्वालिटी";
                sht.Range("C2:E2").Style.Font.FontName = "Calibri";
                sht.Range("C2:E2").Style.Font.FontSize = 14;
                sht.Range("C2:E2").Style.Font.SetBold();
                sht.Range("C2:E2").Merge();
                sht.Range("C2:E2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C2:E2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("C2:E2").Style.Alignment.SetWrapText();
                //sht.Range("B2:B2").Style.Fill.BackgroundColor = XLColor.LightGray;

                sht.Range("F2").Value = "COMPANY";
                sht.Range("F2:F2").Style.Font.FontName = "Calibri";
                sht.Range("F2:F2").Style.Font.FontSize = 14;
                sht.Range("F2:F2").Style.Font.SetBold();
                sht.Range("F2:F2").Merge();
                sht.Range("F2:F2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("F2:F2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("F2:F2").Style.Alignment.SetWrapText();
                //sht.Range("B2:B2").Style.Fill.BackgroundColor = XLColor.LightGray;

                sht.Range("G2").Value = "ORDER QTY";
                sht.Range("G2:G2").Style.Font.FontName = "Calibri";
                sht.Range("G2:G2").Style.Font.FontSize = 14;
                sht.Range("G2:G2").Style.Font.SetBold();
                sht.Range("G2:G2").Merge();
                sht.Range("G2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("G2:G2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("G2:G2").Style.Alignment.SetWrapText();
                //sht.Range("B2:B2").Style.Fill.BackgroundColor = XLColor.LightGray;               



                int row = 2;
                int column = 6;
                int noofrows = 0;
                int i = 0;
                int Dynamiccol = 7;
                int Dynamiccolstart = Dynamiccol + 1;
                int Dynamiccolend;
                int Totalcol;
                decimal Area = 0;
                decimal TotalArea = 0;

                DataTable dtdistinct = ds.Tables[1].DefaultView.ToTable(true, "ReceiveDate");
                noofrows = dtdistinct.Rows.Count;

                for (i = 0; i < noofrows; i++)
                {
                    Dynamiccol = Dynamiccol + 1;
                    sht.Range(sht.Cell(row, Dynamiccol), sht.Cell(row, Dynamiccol)).Merge();

                    sht.Cell(row, Dynamiccol).Value = dtdistinct.Rows[i]["ReceiveDate"].ToString();
                    sht.Cell(row, Dynamiccol).Style.Font.Bold = true;
                    sht.Cell(row, Dynamiccol).Style.Font.FontName = "Calibri";
                    sht.Cell(row, Dynamiccol).Style.Font.FontSize = 14;
                    sht.Cell(row, Dynamiccol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell(row, Dynamiccol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Cell(row, Dynamiccol).Style.Alignment.SetWrapText();
                    sht.Cell(row, Dynamiccol).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Column(Dynamiccol).Width = 18.33;

                    //Dynamiccol = Dynamiccol + 1;
                }
                //Dynamiccol = Dynamiccol + 1;
                Dynamiccolend = Dynamiccol;
                Totalcol = Dynamiccolend + 1;

                sht.Range(sht.Cell(row, Totalcol), sht.Cell(row, Totalcol)).Merge();
                sht.Cell(row, Totalcol).Value = "PENDING QTY ";
                sht.Cell(row, Totalcol).Style.Font.FontName = "Calibri";
                sht.Cell(row, Totalcol).Style.Font.FontSize = 14;
                sht.Cell(row, Totalcol).Style.Font.Bold = true;
                sht.Cell(row, Totalcol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell(row, Totalcol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Column(Totalcol).Width = 19.67;

                // Dynamiccol = Dynamiccol + 1;

                Dynamiccolend = Dynamiccol;
                Totalcol = Dynamiccolend + 2;

                sht.Range(sht.Cell(row, Totalcol), sht.Cell(row, Totalcol)).Merge();
                sht.Cell(row, Totalcol).Value = "PURCHASE STATUS ";
                sht.Cell(row, Totalcol).Style.Font.FontName = "Calibri";
                sht.Cell(row, Totalcol).Style.Font.FontSize = 14;
                sht.Cell(row, Totalcol).Style.Font.Bold = true;
                sht.Cell(row, Totalcol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell(row, Totalcol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Column(Totalcol).Width = 24.56;

                //for (i = noofrows + 1; i <= 10; i++)
                //{
                //    Dynamiccol = Dynamiccol + 1;
                //    sht.Range(sht.Cell(row, Dynamiccol), sht.Cell(row, Dynamiccol + 1)).Merge();
                //    sht.Cell(row, Dynamiccol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //    sht.Cell(row, Dynamiccol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                //    Dynamiccol = Dynamiccol + 1;
                //}
                row = row + 2;

                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[j]["Date"]);
                    sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 14;
                    //sht.Range("A" + row + ":A" + row).Style.Font.SetBold();
                    sht.Range("A" + row + ":A" + row).Merge();
                    sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A" + row + ":A" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[j]["PurchaseOrderNo"]);
                    sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Calibri";
                    sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 14;
                    //sht.Range("B" + row + ":B" + row).Style.Font.SetBold();
                    sht.Range("B" + row + ":B" + row).Merge();
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[j]["Item_Name"]);
                    sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Calibri";
                    sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 14;
                    //sht.Range("C" + row + ":C" + row).Style.Font.SetBold();
                    sht.Range("C" + row + ":C" + row).Merge();
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetWrapText();
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[j]["QualityName"]);
                    sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Calibri";
                    sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 14;
                    //sht.Range("D" + row + ":D" + row).Style.Font.SetBold();
                    sht.Range("D" + row + ":D" + row).Merge();
                    sht.Range("D" + row + ":D" + row).Style.Alignment.SetWrapText();
                    sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("D" + row + ":D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[j]["ShadeColorName"]);
                    sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Calibri";
                    sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 14;
                    //sht.Range("E" + row + ":E" + row).Style.Font.SetBold();
                    sht.Range("E" + row + ":E" + row).Merge();
                    sht.Range("E" + row + ":E" + row).Style.Alignment.SetWrapText();
                    sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("E" + row + ":E" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[j]["VendorName"]);
                    sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Calibri";
                    sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 14;
                    //sht.Range("F" + row + ":F" + row).Style.Font.SetBold();
                    sht.Range("F" + row + ":F" + row).Merge();
                    sht.Range("F" + row + ":F" + row).Style.Alignment.SetWrapText();
                    sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("F" + row + ":F" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[j]["PurchaseQty"]);
                    sht.Range("G" + row + ":G" + row).Style.Font.FontName = "Calibri";
                    sht.Range("G" + row + ":G" + row).Style.Font.FontSize = 14;
                    //sht.Range("G" + row + ":G" + row).Style.Font.SetBold();
                    sht.Range("G" + row + ":G" + row).Merge();
                    sht.Range("G" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("G" + row + ":G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    //Area = Convert.ToDecimal(ds.Tables[0].Rows[j]["Area"]);
                    // TotalArea = TotalArea + Area;

                    decimal TotalSumOneRow = 0;
                    for (int k = Dynamiccolstart; k <= Dynamiccolend; k = k + 1)
                    {
                        var Date = sht.Cell(2, k).Value;
                        decimal RecQty = 0;

                        DataRow[] foundRows;
                        foundRows = ds.Tables[1].Select("ReceiveDate='" + Date + "'  and FinishedId='" + ds.Tables[0].Rows[j]["FinishedId"] + "' and PIndentIssueId='" + ds.Tables[0].Rows[j]["PINDENTISSUEID"] + "' ");

                        if (foundRows.Length > 0)
                        {
                            RecQty = Convert.ToDecimal(ds.Tables[1].Compute("sum(PurchaseReceiveQty)", "ReceiveDate='" + Date + "' and FinishedId='" + ds.Tables[0].Rows[j]["FinishedId"] + "' and PIndentIssueId='" + ds.Tables[0].Rows[j]["PINDENTISSUEID"] + "' "));
                        }
                        //IssRecConQty = IssQty + RecQty + ConsQty;
                        TotalSumOneRow = TotalSumOneRow + RecQty;
                        sht.Range(sht.Cell(row, k), sht.Cell(row, k)).Merge();
                        sht.Cell(row, k).Value = RecQty;
                        sht.Cell(row, k).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Cell(row, k).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Cell(row, k).Style.Font.FontName = "Calibri";
                        sht.Cell(row, k).Style.Font.FontSize = 14;
                        sht.Column(Dynamiccol).Width = 18.33;

                    }

                    decimal BalanceQty = 0;
                    BalanceQty = Convert.ToDecimal(ds.Tables[0].Rows[j]["PurchaseQty"]) - TotalSumOneRow;

                    Totalcol = Dynamiccolend + 1;
                    sht.Range(sht.Cell(row, Totalcol), sht.Cell(row, Totalcol)).Merge();
                    sht.Cell(row, Totalcol).Value = BalanceQty;
                    sht.Cell(row, Totalcol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell(row, Totalcol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Cell(row, Totalcol).Style.Font.FontName = "Calibri";
                    sht.Cell(row, Totalcol).Style.Font.FontSize = 14;
                    sht.Column(Dynamiccol).Width = 19.67;

                    if (BalanceQty == 0)
                    {
                        Totalcol = Dynamiccolend + 2;
                        sht.Range(sht.Cell(row, Totalcol), sht.Cell(row, Totalcol)).Merge();
                        sht.Cell(row, Totalcol).Value = "Complete";
                        sht.Cell(row, Totalcol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Cell(row, Totalcol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Cell(row, Totalcol).Style.Font.FontName = "Calibri";
                        sht.Cell(row, Totalcol).Style.Font.FontSize = 14;
                        sht.Column(Dynamiccol).Width = 24.56;
                    }
                    else
                    {
                        Totalcol = Dynamiccolend + 2;
                        sht.Range(sht.Cell(row, Totalcol), sht.Cell(row, Totalcol)).Merge();
                        sht.Cell(row, Totalcol).Value = "Pending";
                        sht.Cell(row, Totalcol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Cell(row, Totalcol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Cell(row, Totalcol).Style.Font.FontName = "Calibri";
                        sht.Cell(row, Totalcol).Style.Font.FontSize = 14;
                        sht.Column(Dynamiccol).Width = 24.56;
                    }

                    //for (i = Totalcol + 2; i <= 28; i = i + 2)
                    //{
                    //    sht.Range(sht.Cell(row, i), sht.Cell(row, i + 1)).Merge();
                    //    sht.Cell(row, i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //    sht.Cell(row, i).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    //}
                    row = row + 1;

                    sht.Range("A" + row).SetValue("");
                    sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 14;
                    sht.Range("A" + row + ":A" + row).Style.Font.SetBold();
                    sht.Range("A" + row + ":A" + row).Merge();
                    sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A" + row + ":A" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A" + row + ":A" + row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Row(row).Height = 9.8;

                    sht.Range("B" + row).SetValue("");
                    sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Calibri";
                    sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 20;
                    sht.Range("B" + row + ":B" + row).Style.Font.SetBold();
                    sht.Range("B" + row + ":B" + row).Merge();
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("B" + row + ":B" + row).Style.Fill.BackgroundColor = XLColor.LightGray;


                    sht.Range("C" + row).SetValue("");
                    sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Calibri";
                    sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 20;
                    sht.Range("C" + row + ":C" + row).Style.Font.SetBold();
                    sht.Range("C" + row + ":C" + row).Merge();
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C" + row + ":C" + row).Style.Fill.BackgroundColor = XLColor.LightGray;


                    sht.Range("D" + row).SetValue("");
                    sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Calibri";
                    sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 20;
                    sht.Range("D" + row + ":D" + row).Style.Font.SetBold();
                    sht.Range("D" + row + ":D" + row).Merge();
                    sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("D" + row + ":D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("D" + row + ":D" + row).Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("E" + row).SetValue("");
                    sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Calibri";
                    sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 20;
                    sht.Range("E" + row + ":E" + row).Style.Font.SetBold();
                    sht.Range("E" + row + ":E" + row).Merge();
                    sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("E" + row + ":E" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("E" + row + ":E" + row).Style.Fill.BackgroundColor = XLColor.LightGray;


                    sht.Range("F" + row).SetValue("");
                    sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Calibri";
                    sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 20;
                    sht.Range("F" + row + ":F" + row).Style.Font.SetBold();
                    sht.Range("F" + row + ":F" + row).Merge();
                    sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("F" + row + ":F" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("F" + row + ":F" + row).Style.Fill.BackgroundColor = XLColor.LightGray;


                    sht.Range("G" + row).SetValue("");
                    sht.Range("G" + row + ":G" + row).Style.Font.FontName = "Calibri";
                    sht.Range("G" + row + ":G" + row).Style.Font.FontSize = 20;
                    sht.Range("G" + row + ":G" + row).Style.Font.SetBold();
                    sht.Range("G" + row + ":G" + row).Merge();
                    sht.Range("G" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("G" + row + ":G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("G" + row + ":G" + row).Style.Fill.BackgroundColor = XLColor.LightGray;

                    for (int m = Dynamiccolstart; m <= Dynamiccolend; m = m + 1)
                    {
                        sht.Range(sht.Cell(row, m), sht.Cell(row, m)).Merge();
                        sht.Cell(row, m).Value = "";
                        sht.Cell(row, m).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Cell(row, m).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        sht.Cell(row, m).Style.Fill.BackgroundColor = XLColor.LightGray;

                    }

                    Totalcol = Dynamiccolend + 1;
                    sht.Range(sht.Cell(row, Totalcol), sht.Cell(row, Totalcol)).Merge();
                    sht.Cell(row, Totalcol).Value = "";
                    sht.Cell(row, Totalcol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell(row, Totalcol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Cell(row, Totalcol).Style.Font.FontName = "Calibri";
                    sht.Cell(row, Totalcol).Style.Font.FontSize = 14;
                    sht.Cell(row, Totalcol).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Column(Dynamiccol).Width = 19.67;


                    Totalcol = Dynamiccolend + 2;
                    sht.Range(sht.Cell(row, Totalcol), sht.Cell(row, Totalcol)).Merge();
                    sht.Cell(row, Totalcol).Value = "";
                    sht.Cell(row, Totalcol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell(row, Totalcol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Cell(row, Totalcol).Style.Font.FontName = "Calibri";
                    sht.Cell(row, Totalcol).Style.Font.FontSize = 14;
                    sht.Cell(row, Totalcol).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Column(Dynamiccol).Width = 24.56;

                    row = row + 1;

                }


                ////////*************
                //// sht.Columns(1, 20).AdjustToContents();
                ////********************
                ////***********BOrders
                //using (var a = sht.Range("A1" + ":N" + (row - 1)))

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, Totalcol)))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("PurchaseOrderIssRecHafizia_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
            }
        }
        catch (Exception ex)
        {
            //lblmsg.Text = ex.Message;
        }
    }
    protected void RDCustomerOrderWisePODetail_CheckedChanged(object sender, EventArgs e)
    {
        TrStatus.Visible = false;
        Trgodown.Visible = false;
        trChkForDate.Visible = true;
        Tr3.Visible = false;
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        Trcomp.Visible = true;
        trcustomer.Visible = true;
        trorder.Visible = true;
        trfr.Visible = true;
        trto.Visible = true;
        trsupply.Visible = true;
        trPurchaseIndentChallanNo.Visible = true;
        lblPoNo.Text = "PO.No.";
        TRPurchaseSumm.Visible = false;
        TRRecChallanNo.Visible = false;
        txtRecChallanNo.Text = "";
        TRPurchaseDetailByChallan.Visible = false;
        TRLotBillDetail.Visible = false;
        TRFinalAbbaReport.Visible = false;
        TRASOnDate.Visible = false;
        if (Session["varcompanyid"].ToString() == "44")
        {
            UtilityModule.ConditionalComboFill(ref ddcustomer, @"select Distinct CI.CustomerId,CI.CustomerCode as Customercode from Ordermaster OM
                                                          inner join customerinfo CI on ci.CustomerId=OM.CustomerId order by Customercode", true, "Select CustomerCode");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddcustomer, @"select Distinct CI.CustomerId,CI.CustomerCode+'/'+CI.CompanyName as Customercode from Ordermaster OM
                                                          inner join customerinfo CI on ci.CustomerId=OM.CustomerId order by Customercode", true, "Select CustomerCode");
        
        }
    }
    protected void RDPurchaseOrderRecPendingDetail_CheckedChanged(object sender, EventArgs e)
    {
        TrStatus.Visible = false;
        Trgodown.Visible = false;
        trChkForDate.Visible = true;
        Tr3.Visible = true;
        Fill_Category();
        TrItemName.Visible = true;
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        Trcomp.Visible = true;
        trcustomer.Visible = false;
        trorder.Visible = false;
        trfr.Visible = true;
        trto.Visible = true;
        trsupply.Visible = true;
        trPurchaseIndentChallanNo.Visible = true;
        lblPoNo.Text = "PO.No.";
        TRPurchaseSumm.Visible = false;
        trcustomer.Visible = true;
        trorder.Visible = true;
        TRRecChallanNo.Visible = false;
        txtRecChallanNo.Text = "";
        TRPurchaseDetailByChallan.Visible = false;
        TRLotBillDetail.Visible = false;
        TRFinalAbbaReport.Visible = false;
        TRASOnDate.Visible = false;

        if (Session["varcompanyid"].ToString() == "44")
        {
            UtilityModule.ConditionalComboFill(ref ddcustomer, @"select Distinct CI.CustomerId,CI.CustomerCode as Customercode from Ordermaster OM
                                                          inner join customerinfo CI on ci.CustomerId=OM.CustomerId order by Customercode", true, "Select CustomerCode");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddcustomer, @"select Distinct CI.CustomerId,CI.CustomerCode+'/'+CI.CompanyName as Customercode from Ordermaster OM
                                                          inner join customerinfo CI on ci.CustomerId=OM.CustomerId order by Customercode", true, "Select CustomerCode");

        }
    }
    protected void CustomerOrderWisePODetail()
    {
        DataSet ds = new DataSet();

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_GetCustomerOrderWisePODetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@CompanyID", ddCompName.SelectedValue);
        cmd.Parameters.AddWithValue("@CustomerID", ddcustomer.SelectedValue);
        cmd.Parameters.AddWithValue("@OrderID", ddOrderno.SelectedValue);
        cmd.Parameters.AddWithValue("@PartyID", dsuppl.SelectedValue);
        cmd.Parameters.AddWithValue("@PindentIssueID", DDPONo.SelectedValue);
        cmd.Parameters.AddWithValue("@DateFlag", ChkForDate.Checked == true ? 1 : 0);
        cmd.Parameters.AddWithValue("@FromDate", TxtFRDate.Text);
        cmd.Parameters.AddWithValue("@ToDate", TxtTODate.Text);

        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);

        con.Close();
        con.Dispose();

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 0;

            sht.Range("A1:L1").Merge();
            sht.Range("A1").Value = "CUSTOMER ORDER WISE PURCHASE ORDER DETAIL";
            sht.Range("A2:L2").Merge();

            sht.Range("A1:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:L2").Style.Font.Bold = true;

            sht.Range("A3").Value = "Company Name";
            sht.Range("B3").Value = "Supplier Name";
            sht.Range("C3").Value = "Customer Code";
            sht.Range("D3").Value = "Order No";
            sht.Range("E3").Value = "Purchase Order No";
            sht.Range("F3").Value = "Date";
            sht.Range("G3").Value = "Item Description";
            sht.Range("H3").Value = "Unit Name";
            sht.Range("I3").Value = "Qty";
            sht.Range("J3").Value = "Rate";
            sht.Range("K3").Value = "Amount";
            sht.Range("L3").Value = "Net Amount";

            sht.Range("A3:L3").Style.Font.SetBold();

            row = 4;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CompanyName"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["PartyName"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ChallanNo"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Date"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["ItemDescription"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["UnitName"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["Amount"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["NetAmount"]);

                row = row + 1;
            }

            row = row - 1;
            using (var a = sht.Range("A3:L" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            sht.Columns(1, 15).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("CustomerOrderWisePODetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altP", "alert('No records found...')", true);
        }
    }

    protected void PurchaseReceiveDetailByChallanNoEastern()
    {
        #region Where Condition
        string Where = "";
        string filterby = "From : " + TxtFRDate.Text + "  To : " + TxtTODate.Text;
        Where = Where + " and Prm.Receivedate>='" + TxtFRDate.Text + "' and PRM.Receivedate<='" + TxtTODate.Text + "'";
        if (ddcustomer.SelectedIndex > 0)
        {
            Where = Where + " And VP.Customerid=" + ddcustomer.SelectedValue;
            filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
        }
        if (ddOrderno.SelectedIndex > 0)
        {
            Where = Where + " And VP.orderid=" + ddOrderno.SelectedValue;
            filterby = filterby + " Order No : " + ddOrderno.SelectedItem.Text;
        }
        if (dsuppl.SelectedIndex > 0)
        {
            Where = Where + " And EI.Empid=" + dsuppl.SelectedValue;
            filterby = filterby + " Supp Name : " + dsuppl.SelectedItem.Text;
        }
        if (DDPONo.SelectedIndex > 0)
        {
            Where = Where + " And PRD.pindentissueid=" + DDPONo.SelectedValue;
            filterby = filterby + " PO No : " + DDPONo.SelectedItem.Text;
        }
        if (TRRecChallanNo.Visible == true)
        {
            if (txtRecChallanNo.Text != "0" && txtRecChallanNo.Text != "")
            {
                Where = Where + " And PRM.BillNo1='" + txtRecChallanNo.Text + "'";
                filterby = filterby + " BillNo : '" + txtRecChallanNo.Text + "'";
            }
        }
        if (ddCatagory.SelectedIndex > 0)
        {
            Where = Where + " And vf.CATEGORY_ID=" + ddCatagory.SelectedValue;
            filterby = filterby + " Category : " + ddCatagory.SelectedItem.Text;
        }
        if (dditemname.SelectedIndex > 0)
        {
            Where = Where + " And vf.item_id=" + dditemname.SelectedValue;
            filterby = filterby + " Item : " + dditemname.SelectedItem.Text;
        }
        if (dquality.SelectedIndex > 0)
        {
            Where = Where + " And vf.qualityid=" + dquality.SelectedValue;
            filterby = filterby + " Quality : " + dquality.SelectedItem.Text;
        }
        if (dddesign.SelectedIndex > 0)
        {
            Where = Where + " And vf.designid=" + dddesign.SelectedValue;
            filterby = filterby + " design : " + dddesign.SelectedItem.Text;
        }
        if (ddcolor.SelectedIndex > 0)
        {
            Where = Where + " And vf.colorid=" + ddcolor.SelectedValue;
            filterby = filterby + " Color : " + ddcolor.SelectedItem.Text;
        }
        if (ddshape.SelectedIndex > 0)
        {
            Where = Where + " And vf.shapeid=" + ddshape.SelectedValue;
            filterby = filterby + " shape : " + ddshape.SelectedItem.Text;
        }
        if (ddsize.SelectedIndex > 0)
        {
            Where = Where + " And vf.sizeid=" + ddsize.SelectedValue;
        }
        if (ddlshade.SelectedIndex > 0)
        {
            Where = Where + " And vf.shadecolorid=" + ddlshade.SelectedValue;
            filterby = filterby + " Shadecolor : " + ddlshade.SelectedItem.Text;
        }
        #endregion
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@CompanyId", ddCompName.SelectedValue);
        param[1] = new SqlParameter("@Where", Where);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETPURCHASERECEIVEDETAILSBYCHALLANNO_EASTERN", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            Decimal TQty = 0, TAmount = 0;
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 0;
            //***********
            sht.Range("A1:J1").Merge();
            sht.Range("A1").Value = "Purchase Received Details " + "For - " + ddCompName.SelectedItem.Text;
            sht.Range("A2:J2").Merge();
            sht.Range("A2").Value = "Filter By :  " + filterby;
            sht.Row(2).Height = 30;
            sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:J2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A1:J2").Style.Font.Bold = true;
            //***********Filter By Item_Name

            sht.Range("A3").Value = "Vendor Name";
            sht.Range("B3").Value = "Quality";
            sht.Range("C3").Value = "Item Name";
            sht.Range("D3").Value = "Rec.Date";
            sht.Range("E3").Value = "PO No";
            sht.Range("F3").Value = "Bill No";

            sht.Range("G3").Value = "Vendor Lot No.";
            sht.Range("H3").Value = "Internal Lot No.";

            sht.Range("I3").Value = "Rec.Qty";
            sht.Range("J3").Value = "Rate";
            sht.Range("K3").Value = "Amount";
            sht.Range("L3").Value = "Godown Name";
            sht.Range("M3").Value = "Weight";
            sht.Range("I3:K3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3:M3").Style.Font.SetBold();
            //******

            row = 4;

            int Rowfrom = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                //if (Rowfrom == 0)
                //{
                //    Rowfrom = row;
                //}
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["RECDATE"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["PONo"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["BillNo1"]);

                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["VendorLotno"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Lotno"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["ACTUALRECQTY"]);
                TQty = TQty + Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                sht.Range("K" + row).FormulaA1 = "=I" + row + '*' + ("$J$" + row + "");
                TAmount = TAmount + (Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]));
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["GodownName"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["QtyWeight"]);
                row = row + 1;
            }

            //*************GRAND TOTAL
            sht.Range("I" + row + ":K" + row).Style.Font.SetBold();
            sht.Range("H" + row).Value = "Grand Total";
            sht.Range("I" + row).SetValue(TQty);
            sht.Range("K" + row).SetValue(TAmount);
            //*************
            sht.Columns(1, 12).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PurchaseReceiveDetailsByChallanEastern_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();


        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "purchaserec", "alert('No Record Found!');", true);

        }


    }

    protected void PurchaseordervendorwisexcelexportDiamond(DataSet ds, string FilterBy)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 0;
            //***********
            sht.Range("A1:X1").Merge();
            sht.Range("A1").Value = "PURCHASE VENDOR WISE";
            sht.Range("A2:X2").Merge();
            sht.Range("A2").Value = "Filter By :  " + FilterBy;
            sht.Range("A1:X1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:X2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:X2").Style.Font.Bold = true;
            //**********
            sht.Range("A3").Value = "Category";
            sht.Range("B3").Value = "Po No";
            sht.Range("C3").Value = "Po Status";
            sht.Range("D3").Value = "Po Date";
            sht.Range("E3").Value = "Supp. Name";
            sht.Range("F3").Value = "Item Name";
            sht.Range("G3").Value = "Rate";
            sht.Range("H3").Value = "PO Qty";

            sht.Range("I3").Value = "GST/IGST (%)";
            sht.Range("J3").Value = "NET AMOUNT";

            sht.Range("G3:J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("K3").Value = "Delv Date";
            sht.Range("L3").Value = "Net Rate";
            sht.Range("M3").Value = "RecDate/ChallanNo";
            sht.Range("N3").Value = "Rec Qty";
            sht.Range("N3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("O3").Value = "Ret Date";
            sht.Range("P3").Value = "Ret Qty";
            sht.Range("Q3").Value = "Pending Qty";
            sht.Range("P3:Q3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3:X3").Style.Font.SetBold();
            sht.Range("R3").Value = "Gate In NO";           
           sht.Range("S3").Value = "";
           sht.Column(17).Hide();
           sht.Range("T3").Value = "Late By";
           sht.Range("U3").Value = "RequestedBy";
           sht.Range("U3:V3").Merge();
           sht.Range("W3").Value = "RequestedFor";
           sht.Range("W3:X3").Merge();
            using (var a = sht.Range("A3:X3"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*******
            row = 4;
            DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "Pindentissueid", "finishedid", "OrderID");
            foreach (DataRow dr in dtdistinct.Rows)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "Pindentissueid=" + dr["Pindentissueid"] + " and Finishedid=" + dr["finishedid"] + " and OrderID=" + dr["OrderID"];
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Category_name"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Po"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Status"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Orderdate"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Empname"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Description"].ToString() + ds1.Tables[0].Rows[i]["Colour"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Orderqty"]);

                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["SGSTIGST"]);
                    decimal SGSTIGSTValue = 0;
                    SGSTIGSTValue = ((Convert.ToDecimal(ds1.Tables[0].Rows[i]["Orderqty"]) * Convert.ToDecimal(ds1.Tables[0].Rows[i]["Rate"])) * Convert.ToDecimal(ds1.Tables[0].Rows[i]["SGSTIGST"]) / 100);
                    decimal NetAmount = 0;
                    NetAmount = (Convert.ToDecimal(ds1.Tables[0].Rows[i]["Orderqty"]) * Convert.ToDecimal(ds1.Tables[0].Rows[i]["Rate"])) + SGSTIGSTValue;
                    sht.Range("J" + row).SetValue(Math.Round(NetAmount,2));

                    sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Deliverydate"]);
                    sht.Range("L" + row).FormulaA1 = "=J" + row + '/' + "$H$"+row;
                    sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["Recdate"]);
                    sht.Range("N" + row).SetValue(Convert.ToDecimal(ds1.Tables[0].Rows[i]["Recqty"]) + Convert.ToDecimal(ds1.Tables[0].Rows[i]["Qtyreturn"]));
                    sht.Range("O" + row).SetValue(ds1.Tables[0].Rows[i]["Returnchallan"]);
                    sht.Range("P" + row).SetValue(ds1.Tables[0].Rows[i]["Qtyreturn"]);
                    //sht.Range("O" + row).SetValue("");
                    if (i == 0)
                    {
                        sht.Range("Q" + row).FormulaA1 = "=H" + row + '+' + "$P$" + row + '-' + "$N$" + row;
                    }
                    else
                    {
                        sht.Range("Q" + row).FormulaA1 = "=Q" + (row - 1) + '+' + "$P$" + row + '-' + "$N$" + row;
                    }
                    sht.Range("R" + row).SetValue(ds1.Tables[0].Rows[i]["GateInNo"]);
                    sht.Range("S" + row).SetValue("");
                    sht.Range("T" + row).SetValue(ds1.Tables[0].Rows[i]["DayDifference"]);
                    sht.Range("U" + row).SetValue(ds1.Tables[0].Rows[i]["requestby"]);
                    sht.Range("U" + row+":V"+row).Merge();
                    sht.Range("W" + row).SetValue(ds1.Tables[0].Rows[i]["requestfor"]);
                    sht.Range("W" + row + ":X" + row).Merge();

                    row = row + 1;
                }
                ds1.Dispose();
            }
            //*************
            sht.Columns(1, 30).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PurchaseOrderVendorwise_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altP", "alert('No records found...')", true);
        }
    }

    protected void PurchaseReceiveDetailWithGSTAmt()
    {
        #region Where Condition
        string Where = "";
        string filterby = "From : " + TxtFRDate.Text + "  To : " + TxtTODate.Text;
        Where = Where + " and Prm.Receivedate>='" + TxtFRDate.Text + "' and PRM.Receivedate<='" + TxtTODate.Text + "'";
        if (ddcustomer.SelectedIndex > 0)
        {
            Where = Where + " And VP.Customerid=" + ddcustomer.SelectedValue;
            filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
        }
        if (ddOrderno.SelectedIndex > 0)
        {
            Where = Where + " And VP.orderid=" + ddOrderno.SelectedValue;
            filterby = filterby + " Order No : " + ddOrderno.SelectedItem.Text;
        }
        if (dsuppl.SelectedIndex > 0)
        {
            Where = Where + " And EI.Empid=" + dsuppl.SelectedValue;
            filterby = filterby + " Supp Name : " + dsuppl.SelectedItem.Text;
        }
        if (DDPONo.SelectedIndex > 0)
        {
            Where = Where + " And PRD.pindentissueid=" + DDPONo.SelectedValue;
            filterby = filterby + " PO No : " + DDPONo.SelectedItem.Text;
        }
        if (TRRecChallanNo.Visible == true)
        {
            if (txtRecChallanNo.Text != "0" && txtRecChallanNo.Text != "")
            {
                Where = Where + " And PRM.BillNo1='" + txtRecChallanNo.Text + "'";
                filterby = filterby + " BillNo : '" + txtRecChallanNo.Text + "'";
            }
        }

        if (ddCatagory.SelectedIndex > 0)
        {
            Where = Where + " And vf.CATEGORY_ID=" + ddCatagory.SelectedValue;
            filterby = filterby + " Category : " + ddCatagory.SelectedItem.Text;
        }
        if (dditemname.SelectedIndex > 0)
        {
            Where = Where + " And vf.item_id=" + dditemname.SelectedValue;
            filterby = filterby + " Item : " + dditemname.SelectedItem.Text;
        }
        if (dquality.SelectedIndex > 0)
        {
            Where = Where + " And vf.qualityid=" + dquality.SelectedValue;
            filterby = filterby + " Quality : " + dquality.SelectedItem.Text;
        }
        if (dddesign.SelectedIndex > 0)
        {
            Where = Where + " And vf.designid=" + dddesign.SelectedValue;
            filterby = filterby + " design : " + dddesign.SelectedItem.Text;
        }
        if (ddcolor.SelectedIndex > 0)
        {
            Where = Where + " And vf.colorid=" + ddcolor.SelectedValue;
            filterby = filterby + " Color : " + ddcolor.SelectedItem.Text;
        }
        if (ddshape.SelectedIndex > 0)
        {
            Where = Where + " And vf.shapeid=" + ddshape.SelectedValue;
            filterby = filterby + " shape : " + ddshape.SelectedItem.Text;
        }
        if (ddsize.SelectedIndex > 0)
        {
            Where = Where + " And vf.sizeid=" + ddsize.SelectedValue;
        }
        if (ddlshade.SelectedIndex > 0)
        {
            Where = Where + " And vf.shadecolorid=" + ddlshade.SelectedValue;
            filterby = filterby + " Shadecolor : " + ddlshade.SelectedItem.Text;
        }
        if (DDgodown.SelectedIndex > 0)
        {
            Where = Where + " And gm.godownid=" + DDgodown.SelectedValue;
            filterby = filterby + " Godown : " + DDgodown.SelectedItem.Text;
        }

        #endregion
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlCommand cmd = new SqlCommand("PRO_GETPURCHASERECEIVEDETAILSWITHGSTAMTREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@CompanyId", ddCompName.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", Where);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            sda.Fill(dt);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                Decimal TQty = 0, TAmount = 0;
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;
                //***********
                sht.Range("A1:T1").Merge();
                sht.Range("A1").Value = "Purchase Received Details " + "For - " + ddCompName.SelectedItem.Text;
                sht.Range("A2:T2").Merge();
                sht.Range("A2").Value = "Filter By :  " + filterby;
                sht.Row(2).Height = 30;
                sht.Range("A1:T1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:T2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:T2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A1:T2").Style.Font.Bold = true;
                //***********Filter By Item_Name
                row = 3;

                //******Headers 
                sht.Range("A" + row).Value = "REC DATE";
                sht.Range("B" + row).Value = "PO NO";
                sht.Range("C" + row).Value = "VENDOR NAME";

                sht.Range("D" + row).Value = "ITEM NAME";
                sht.Range("E" + row).Value = "Quality";
                sht.Range("F" + row).Value = "SHADE";

                sht.Range("G" + row).Value = "REC QTY";
                sht.Range("H" + row).Value = "BALE NO";
                sht.Range("I" + row).Value = "REC LOT NO.";
                sht.Range("J" + row).Value = "REC TAG NO.";
                sht.Range("K" + row).Value = "GODOWN NAME";

                sht.Range("L" + row).Value = "BILL NO.";
                sht.Range("M" + row).Value = "BILL DATE";
                sht.Range("N" + row).Value = "INWARD NO";
                sht.Range("O" + row).Value = "RATE";
                sht.Range("P" + row).Value = "GST%";
                sht.Range("Q" + row).Value = "PUR. AMOUNT";
                sht.Range("R" + row).Value = "GST AMOUNT";
                sht.Range("S" + row).Value = "NET AMOUNT";
                sht.Range("T" + row).Value = "NET RATE"; 

                sht.Range("P" + row + ":T" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A" + row + ":T" + row).Style.Font.SetBold();

                row = row + 1;
                int Rowfrom = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (Rowfrom == 0)
                    {
                        Rowfrom = row;
                    }
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["RECDATE"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["PURCHASEORDER"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["RECQTY"]);

                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Baleno"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["LotNo"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["TagNo"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["GodownName"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["PartyChallanNo"]);
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["BillDate"]);
                    sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["InwardsNo"]);
                    sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                    sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["GSTPercentage"]);
                    sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["Amount"]);                   

                    sht.Range("R" + row).FormulaA1 = ("=Round(Q" + row + '*' + ("$P$" + row + "") + '/' + 100+ ",2)");
                    //sht.Range("R" + row).Style.NumberFormat.Format = "#,##0.00";
                    sht.Range("S" + row).FormulaA1 = ("=Round(Q" + row + '+' + ("$R$" + row + "")+ ",2)");
                    sht.Range("T" + row).FormulaA1 = ("=Round(S" + row + '/' + ("$G$" + row + "") + ",2)");

                    //TQty = TQty + Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"]);
                    //sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);

                    //sht.Range("I" + row).FormulaA1 = "=G" + row + '*' + ("$H$" + row + "");
                    //TAmount = TAmount + (Convert.ToDecimal(ds1.Tables[0].Rows[i]["ACTUALRECQTY"]) * Convert.ToDecimal(ds1.Tables[0].Rows[i]["Rate"]));
                   

                    row = row + 1;
                }
                row = row + 1;
                ////TOTAL

                //sht.Range("F" + row).Value = "Total";
                //sht.Range("G" + row).FormulaA1 = "=SUM(G" + Rowfrom + ":$G$" + (row - 1) + ")";
                //sht.Range("I" + row).FormulaA1 = "=SUM(I" + Rowfrom + ":$I$" + (row - 1) + ")";
                //sht.Range("G" + row + ":I" + row).Style.Font.SetBold();
                //row = row + 2;
                //ds.Dispose();

                ////*************GRAND TOTAL
                //sht.Range("G" + row + ":I" + row).Style.Font.SetBold();
                //sht.Range("F" + row).Value = "Grand Total";
                //sht.Range("G" + row).SetValue(TQty);
                //sht.Range("I" + row).SetValue(TAmount);
                ////*************
                sht.Columns(1, 18).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("PurchaseReceiveDetailsWithGSTAmt_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "purchaserec", "alert('No Record Found!');", true);

            }
        }
        catch (Exception ex)
        {
            Label1.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void PurchaseReceiveDetailCI()
    {
        #region Where Condition
        string Where = "";
        string filterby = "From : " + TxtFRDate.Text + "  To : " + TxtTODate.Text;
        Where = Where + " and Prm.Receivedate>='" + TxtFRDate.Text + "' and PRM.Receivedate<='" + TxtTODate.Text + "'";
        if (ddcustomer.SelectedIndex > 0)
        {
            Where = Where + " And VP.Customerid=" + ddcustomer.SelectedValue;
            filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
        }
        if (ddOrderno.SelectedIndex > 0)
        {
            Where = Where + " And VP.orderid=" + ddOrderno.SelectedValue;
            filterby = filterby + " Order No : " + ddOrderno.SelectedItem.Text;
        }
        if (dsuppl.SelectedIndex > 0)
        {
            Where = Where + " And EI.Empid=" + dsuppl.SelectedValue;
            filterby = filterby + " Supp Name : " + dsuppl.SelectedItem.Text;
        }
        if (DDPONo.SelectedIndex > 0)
        {
            Where = Where + " And PRD.pindentissueid=" + DDPONo.SelectedValue;
            filterby = filterby + " PO No : " + DDPONo.SelectedItem.Text;
        }
        if (TRRecChallanNo.Visible == true)
        {
            if (txtRecChallanNo.Text != "0" && txtRecChallanNo.Text != "")
            {
                Where = Where + " And PRM.BillNo1='" + txtRecChallanNo.Text + "'";
                filterby = filterby + " BillNo : '" + txtRecChallanNo.Text + "'";
            }
        }

        if (ddCatagory.SelectedIndex > 0)
        {
            Where = Where + " And vf.CATEGORY_ID=" + ddCatagory.SelectedValue;
            filterby = filterby + " Category : " + ddCatagory.SelectedItem.Text;
        }
        if (dditemname.SelectedIndex > 0)
        {
            Where = Where + " And vf.item_id=" + dditemname.SelectedValue;
            filterby = filterby + " Item : " + dditemname.SelectedItem.Text;
        }
        if (dquality.SelectedIndex > 0)
        {
            Where = Where + " And vf.qualityid=" + dquality.SelectedValue;
            filterby = filterby + " Quality : " + dquality.SelectedItem.Text;
        }
        if (dddesign.SelectedIndex > 0)
        {
            Where = Where + " And vf.designid=" + dddesign.SelectedValue;
            filterby = filterby + " design : " + dddesign.SelectedItem.Text;
        }
        if (ddcolor.SelectedIndex > 0)
        {
            Where = Where + " And vf.colorid=" + ddcolor.SelectedValue;
            filterby = filterby + " Color : " + ddcolor.SelectedItem.Text;
        }
        if (ddshape.SelectedIndex > 0)
        {
            Where = Where + " And vf.shapeid=" + ddshape.SelectedValue;
            filterby = filterby + " shape : " + ddshape.SelectedItem.Text;
        }
        if (ddsize.SelectedIndex > 0)
        {
            Where = Where + " And vf.sizeid=" + ddsize.SelectedValue;
        }
        if (ddlshade.SelectedIndex > 0)
        {
            Where = Where + " And vf.shadecolorid=" + ddlshade.SelectedValue;
            filterby = filterby + " Shadecolor : " + ddlshade.SelectedItem.Text;
        }
        if (DDgodown.SelectedIndex > 0)
        {
            Where = Where + " And gm.godownid=" + DDgodown.SelectedValue;
            filterby = filterby + " Godown : " + DDgodown.SelectedItem.Text;
        }

        #endregion
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlCommand cmd = new SqlCommand("PRO_GETPURCHASERECEIVEDETAILS_CI", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@CompanyId", ddCompName.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", Where);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            sda.Fill(dt);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                Decimal TQty = 0, TAmount = 0;
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;
                //***********
                sht.Range("A1:M1").Merge();
                sht.Range("A1").Value = "Purchase Received Details " + "For - " + ddCompName.SelectedItem.Text;
                sht.Range("A2:M2").Merge();
                sht.Range("A2").Value = "Filter By :  " + filterby;
                sht.Row(2).Height = 30;
                sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:M2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A1:M2").Style.Font.Bold = true;
                //***********Filter By Item_Name
                row = 3;

                sht.Range("A" + row).Value = "PO NO";
                sht.Range("B" + row).Value = "Rec Date";
                sht.Range("C" + row).Value = "Rec ChallanNo";

                sht.Range("D" + row).Value = "Vendor Name";
                sht.Range("E" + row).Value = "Quality";
                sht.Range("F" + row).Value = "UCN No";

                sht.Range("G" + row).Value = "Lot No";
                sht.Range("H" + row).Value = "Rec Qty";
                sht.Range("I" + row).Value = "Rate";
                sht.Range("J" + row).Value = "Amount";
                sht.Range("K" + row).Value = "GateIn No";
                sht.Range("L" + row).Value = "Godown Name";
                sht.Range("M" + row).Value = "Moisture(%)";

                sht.Range("H" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A" + row + ":M" + row).Style.Font.SetBold();

                row = row+1;
                int Rowfrom = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (Rowfrom == 0)
                    {
                        Rowfrom = row;
                    }
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["PurchaseOrderChallanNo"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["RECDATE"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ReceiveBillChallanNo"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["TagNo"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Lotno"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ACTUALRECQTY"]);
                    TQty = TQty + Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                    sht.Range("J" + row).FormulaA1 = "=H" + row + '*' + ("$I$" + row + "");
                    TAmount = TAmount + (Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["Rate"]));
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["GateInNo"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["godownName"]);
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["ReceiveMoisture"]);

                    //sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    //sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                    //sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["RECDATE"]);
                    //sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["VendorLotno"]);
                    //sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Lotno"]);
                    //sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["godownName"]);
                    //sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["ACTUALRECQTY"]);
                    //TQty = TQty + Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"]);
                    //sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                    //sht.Range("I" + row).FormulaA1 = "=G" + row + '*' + ("$H$" + row + "");
                    //TAmount = TAmount + (Convert.ToDecimal(ds.Tables[0].Rows[i]["ACTUALRECQTY"]) * Convert.ToDecimal(ds1.Tables[0].Rows[i]["Rate"]));
                    //sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["GateInNo"]);
                    //sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["ReceiveMoisture"]);

                    row = row + 1;
                }
                //sht.Range("F" + row).Value = "Total";
                //sht.Range("H" + row).FormulaA1 = "=SUM(H" + Rowfrom + ":$H$" + (row - 1) + ")";
                //sht.Range("J" + row).FormulaA1 = "=SUM(J" + Rowfrom + ":$J$" + (row - 1) + ")";
                //sht.Range("G" + row + ":J" + row).Style.Font.SetBold();
                //row = row + 2;

                //DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "Item_Name");
                //foreach (DataRow dr in dtdistinct.Rows)
                //{
                //    DataView dv = new DataView(ds.Tables[0]);
                //    dv.RowFilter = "Item_Name='" + dr["Item_Name"] + "'";
                //    dv.Sort = "Receivedate";
                //    DataSet ds1 = new DataSet();
                //    ds1.Tables.Add(dv.ToTable());
                //    //******Headers
                //    sht.Range("A" + row).SetValue(dr["Item_Name"]);
                //    sht.Range("A" + row).Style.Font.SetBold();
                //    sht.Range("A" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //    row = row + 1;
                //    sht.Range("A" + row).Value = "Vendor Name";
                //    sht.Range("B" + row).Value = "Quality";
                //    sht.Range("C" + row).Value = "Rec.Date";

                //    sht.Range("D" + row).Value = "Vendor Lot No.";
                //    sht.Range("E" + row).Value = "Internal Lot No.";
                //    sht.Range("F" + row).Value = "Godown Name";

                //    sht.Range("G" + row).Value = "Rec.Qty";
                //    sht.Range("H" + row).Value = "Rate";
                //    sht.Range("I" + row).Value = "Amount";
                //    sht.Range("J" + row).Value = "Gate In No";
                //    sht.Range("K" + row).Value = "Moisture(%)";
                //    sht.Range("G" + row + ":I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //    sht.Range("A" + row + ":K" + row).Style.Font.SetBold();
                //    //******
                //    row = row + 1;
                //    int Rowfrom = 0;
                //    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                //    {
                //        if (Rowfrom == 0)
                //        {
                //            Rowfrom = row;
                //        }
                //        sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["empname"]);
                //        sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["QUALITYNAME"]);
                //        sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["RECDATE"]);
                //        sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["VendorLotno"]);
                //        sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
                //        sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["godownName"]);
                //        sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["ACTUALRECQTY"]);
                //        TQty = TQty + Convert.ToDecimal(ds1.Tables[0].Rows[i]["ACTUALRECQTY"]);
                //        sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);
                //        sht.Range("I" + row).FormulaA1 = "=G" + row + '*' + ("$H$" + row + "");
                //        TAmount = TAmount + (Convert.ToDecimal(ds1.Tables[0].Rows[i]["ACTUALRECQTY"]) * Convert.ToDecimal(ds1.Tables[0].Rows[i]["Rate"]));
                //        sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["GateInNo"]);
                //        sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["ReceiveMoisture"]);

                //        row = row + 1;
                //    }
                //    //TOTAL

                //    sht.Range("F" + row).Value = "Total";
                //    sht.Range("G" + row).FormulaA1 = "=SUM(G" + Rowfrom + ":$G$" + (row - 1) + ")";
                //    sht.Range("I" + row).FormulaA1 = "=SUM(I" + Rowfrom + ":$I$" + (row - 1) + ")";
                //    sht.Range("G" + row + ":I" + row).Style.Font.SetBold();
                //    row = row + 2;
                //    ds1.Dispose();
                //}
                //*************GRAND TOTAL
                sht.Range("G" + row + ":M" + row).Style.Font.SetBold();
                sht.Range("F" + row).Value = "Grand Total";
                sht.Range("H" + row).SetValue(TQty);
                sht.Range("J" + row).SetValue(TAmount);
                //*************
                sht.Columns(1, 20).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("PurchaseReceiveDetails_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "purchaserec", "alert('No Record Found!');", true);

            }
        }
        catch (Exception ex)
        {
            Label1.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
           
    }
}