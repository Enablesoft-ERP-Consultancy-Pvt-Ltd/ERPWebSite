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

public partial class ERPmaster : System.Web.UI.MasterPage
{
    DataSet ds5;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Session["varMasterCompanyIDForERP"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            if (Session["varMasterCompanyIDForERP"].ToString() == "20")
            {
                trheader.Visible = false;
            }
            imgLogo.ImageUrl.DefaultIfEmpty();
            if (File.Exists(Server.MapPath("~/Images/Logo/" + Session["varMasterCompanyIDForERP"] + "_company.gif")))
            {
                imgLogo.ImageUrl = "~/Images/Logo/" + Session["varMasterCompanyIDForERP"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
            }
            LblCompanyName.Text = Session["varCompanyName"].ToString();
            LblUserName.Text = Session["varusername"].ToString();
            LblFrmName.Text = Page.Title.ToUpper().ToString();
            switch (Session["varcompanyNo"].ToString())
            {
                case "7":
                    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from UserRightsProcess where processid in(2,3) and userid=" + Session["VAruserid"] + "");
                    if (Session["varuserid"].ToString() != "1")
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FillGridplanedGF();
                            FillGridDiliveryOrderprodissue();
                            FillGridDiliveryOrderprod();
                            FillGridDiliveryOrderproddp();
                            FillGridJW();
                            FillGridToplan();
                            FillGridConsumption();
                            FillDpordertomake();
                        }
                        else
                        {
                            FillGridplaned();
                            FillGridplanedconf();
                            FillGridDiliveryOrder();
                            FillISQ();
                            FillRemarkAlert();
                        }
                    }
                    else
                    {
                        FillRemarkAlert();
                        FillGridplanedGF();
                        FillGridDiliveryOrderprodissue();
                        FillGridDiliveryOrderprod();
                        FillGridDiliveryOrderproddp();
                        FillGridJW();
                        FillGridToplan();
                        FillGridConsumption();
                        //FillDpordertomake();
                        FillGridplaned();
                        FillGridplanedconf();
                        FillGridDiliveryOrder();
                        FillISQ();
                    }
                    break;
                case "6":
                case "12":
                    Fillartalert();
                    ds5 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from alertname order by Alertid");
                    ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from UserRights where menuid=36 and userid=" + Session["varuserid"] + "");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        planningAll();
                        DpLatedelorderAll();
                    }
                    ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from UserRights where menuid=76 and userid=" + Session["varuserid"] + "");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        PurchaseAll();
                    }
                    ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from UserRights where menuid=40 and userid=" + Session["varuserid"] + "");
                    {
                        DPorderAll();
                        DpreciveAll();
                        DpProductionorderAll();
                    }
                    break;
                case "16":
                    //FillGridDelayCustomerOrder();
                    break;
                case "27":
                    //FillGridDelayCustomerOrder();
                    //FillGridDelayPurchaseOrder();
                    //FillGridDelayDyeingIndent();
                    //FillGridDelayProductionOrder();
                    break;
            }
        }
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void FillGridplaned()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @" SELECT Distinct LocalOrder+'/'+CustomerOrderNo + space(7) + Replace(convert(nvarchar(11),OrderDate,106),' ',' -') As OrderNo,OP.OrderId As OrderId From OrderProcessPlanning OP,OrderMaster OM,Process_Name_Master PNM 
                     Where OP.OrderId=OM.OrderId And om.status=0 And OP.PROCESSID=PNM.PROCESS_NAME_ID and op.processid in(select Distinct processid from UserRightsProcess where userid=" + Session["varuserid"] + ") and FinalDate is null and processid=1";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGAlertplaned.DataSource = ds;
            DGAlertplaned.DataBind();
            tdplaning.Visible = true;
        }
    }
    protected void FillGridplanedconf()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @"select Distinct LocalOrder+'/'+CustomerOrderNo + space(7) + Replace(convert(nvarchar(11),OrderDate,106),' ',' -')+'/ '+isnull(op.Purhasername,' ') As OrderNo,om.orderid 
               from OrderProcessPlanning OP,Process_Name_Master PNM,V_Order_category vc,UserRights_Category uc,V_OrderConsumptionAndPurchaseQty as ocp,OrderMaster OM LEFT OUTER JOIN PurchaseIndentIssue PII ON PII.ORDERID=OM.ORDERID and PII.STATUS='Pending'
               Where OP.OrderId=OM.OrderId  And OP.PROCESSID=PNM.PROCESS_NAME_ID and om.orderid=vc.orderid and vc.CATEGORY_ID=uc.CategoryId and ocp.orderid=op.orderid and 
               FinalStatus=1 and om.status=0 and processid=1 and om.status=0  and
               op.orderid in(select distinct orderid from JobAssigns where PreProdAssignedQty<>0 and supplierqty=0) and uc.userid=" + Session["varuserid"] + @"
               Group by om.orderid,ocp.finishedid,LocalOrder,CustomerOrderNo,OrderDate,op.Purhasername
               Having  sum(consumption)>sum(issue)";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGAlertolanedconf.DataSource = ds;
            DGAlertolanedconf.DataBind();
            tdplaningconformation.Visible = true;
        }
    }
    protected void FillGridplanedGF()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @" SELECT Distinct LocalOrder+'/'+CustomerOrderNo + space(7) + Replace(convert(nvarchar(11),OrderDate,106),' ',' -') As OrderNo,OP.OrderId As OrderId
                     From OrderProcessPlanning OP,OrderMaster OM,Process_Name_Master PNM, UserRights_Category uc,V_FinishedItemDetail v,orderdetail od,JobAssigns j
                     Where OP.OrderId=OM.OrderId And OP.PROCESSID=PNM.PROCESS_NAME_ID and od.orderid=om.orderid and od.Item_Finished_Id=v.Item_Finished_Id and v.CATEGORY_ID=uc.CategoryId and om.status=0  and j.orderid=om.orderid and j.SupplierQty=0 and uc.userid=" + Session["varuserid"] + @" and
                     FinalDate is not null and processid=1 and isnull(FinalStatus,0)<>1";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGAlertplanedGF.DataSource = ds;
            DGAlertplanedGF.DataBind();
            tdplaninggf.Visible = true;
        }
    }
    protected void FillGridDiliveryOrder()
    {
        string fromdt = DateTime.Now.Date.ToString("dd-MMM-yyyy");
        string todate = DateTime.Now.Date.AddDays(7).ToString("dd-MMM-yyyy");
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @" select Distinct 'PO.No-'+Challanno+', '+e.empname +', '+ LocalOrder+'/'+CustomerOrderNo+'/ '+isnull(op.Purhasername,' ')  As OrderNo,om.OrderId As OrderId 
               From PurchaseIndentIssue p left outer join ordermaster om  on p.orderid=om.orderid inner join 
               Empinfo e On e.empid=p.Partyid inner join Orderdetail od On om.orderid=od.orderid Inner join 
               V_FinishedItemDetail vd On  vd.ITEM_FINISHED_ID=od.Item_Finished_Id inner join 
               UserRights_Category uc On vd.CATEGORY_ID=uc.CategoryId inner join OrderProcessPlanning OP On op.orderid=om.orderid and op.processid=1
               Where  om.status=0   and uc.userid=" + Session["varuserid"] + "  and p.duedate>='" + fromdt + "' and p.duedate<='" + todate + "'";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGAlertdelvorder.DataSource = ds;
            DGAlertdelvorder.DataBind();
            Trdelvorder.Visible = true;
        }
    }
    protected void FillGridDiliveryOrderprodissue()
    {
        string fromdt = DateTime.Now.Date.ToString("dd-MMM-yyyy");
        string todate = DateTime.Now.Date.AddDays(7).ToString("dd-MMM-yyyy");
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @" select distinct  om.orderid as orderid ,'I.No-'+V.IndentNo+' '+V.empname+' '+LocalOrder+'/'+CustomerOrderNo   As OrderNo ,isnull(sum(pQty),0) as pqty,isnull(sum(op.IssueQuantity),0) as issueqty
        from  V_IndentDetail V left outer join v_indentissueqty pr on V.indentid=pr.IndentId  inner join OrderMaster Om On om.orderid=v.orderid inner join 
        v_Order_category voc On  voc.orderid=om.orderid inner join UserRights_Category uc On uc.CategoryId=voc.CATEGORY_ID inner join OrderAssignQty op On op.orderid=om.orderid
        where    om.status=0 and v.status='Pending' and uc.userid=" + Session["varuserid"] + @" group by om.orderid,V.IndentNo,V.empname,LocalOrder,CustomerOrderNo
        Having isnull(sum(pQty),0)> sum(op.IssueQuantity)";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DgalertindentprodIss.DataSource = ds;
            DgalertindentprodIss.DataBind();
            TrAlertindentprodIss.Visible = true;
        }
    }
    protected void FillGridDiliveryOrderprod()
    {
        string fromdt = DateTime.Now.Date.ToString("dd-MMM-yyyy");
        string todate = DateTime.Now.Date.AddDays(7).ToString("dd-MMM-yyyy");
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @" Select distinct om.orderid as orderid,IndentNo+', '+e.empname+', '+LocalOrder+'/'+CustomerOrderNo  As OrderNo 
                     From indentmaster im Inner join Indentdetail id On im.IndentID=id.IndentID inner join  Ordermaster om ON om.orderid=id.orderid inner join Orderdetail od On od.orderid=om.orderid inner join 
                     V_FinishedItemDetail v On v.item_finished_id=od.Item_Finished_Id inner join UserRights_Category uc On uc.CategoryId=v.CATEGORY_ID Inner join
                     empInfo e On e.empid=im.PartyId Where om.status=0  and uc.userid=" + Session["varuserid"] + " and  ReqDate>='" + fromdt + "' and ReqDate<='" + todate + "'";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Dgalertindentprod.DataSource = ds;
            Dgalertindentprod.DataBind();
            TrAlertindentprod.Visible = true;
        }
    }
    protected void FillGridDiliveryOrderproddp()
    {
        string fromdt = DateTime.Now.Date.ToString("dd-MMM-yyyy");
        string todate = DateTime.Now.Date.AddDays(7).ToString("dd-MMM-yyyy");
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @" Select distinct om.orderid as orderid ,cast(im.IssueOrderid as varchar)+', '+LocalOrder+'/'+CustomerOrderNo+'  '+e.empname  As OrderNo 
                     From PROCESS_ISSUE_Master_9 im inner join PROCESS_ISSUE_DETAIL_9 id On im.IssueOrderid=id.IssueOrderid  inner join Ordermaster om ON om.orderid=id.orderid inner join Orderdetail od On od.orderid=om.orderid inner join 
                     V_FinishedItemDetail v On v.item_finished_id=od.Item_Finished_Id Inner join empinfo e On e.empid=im.Empid inner join UserRights_Category uc On uc.CategoryId=v.CATEGORY_ID
                     Where om.status=0  and uc.userid=" + Session["varuserid"] + " and  ReqByDate>='" + fromdt + "' and ReqByDate<='" + todate + "'";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DgalertindentprodDP.DataSource = ds;
            DgalertindentprodDP.DataBind();
            TrAlertindentprodDP.Visible = true;
        }
    }
    protected void FillGridJW()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @"Select distinct  om.orderid as orderid ,LocalOrder+'/'+CustomerOrderNo   As OrderNo  
                     From ordermaster om inner join orderdetail od On om.orderid=od.orderid left outer join 
                     V_IndentDetail i On i.orderid=om.orderid left outer join IndentDetail id On id.indentid=i.IndentId inner join 
                     V_FinishedItemDetail v On v.item_finished_id=od.Item_Finished_Id inner join 
                     UserRights_Category uc On uc.CategoryId=v.CATEGORY_ID Inner join 
                     OrderProcessPlanning op On op.orderid=om.orderid inner join 
                     PurchaseIndentIssue pii On pii.Orderid=om.orderid inner join
                     v_purchase_receive_report prr On prr.PIndentIssueId=pii.PIndentIssueId Inner join 
                     OrderAssignQty oap On oap.orderid=om.orderid
                     Where om.status=0 and isnull(op.FinalStatus,0)=1 and i.status='Pending' and uc.userid=" + Session["varuserid"] + @"
                     Group by om.orderid ,LocalOrder,CustomerOrderNo
                     Having isnull(sum(Quantity),0)< sum(od.QtyRequired) and isnull(sum(pQty),0)>isnull(sum(IssueQuantity),0)";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DgJW.DataSource = ds;
            DgJW.DataBind();
            TrJWorder.Visible = true;
        }
    }
    protected void FillGridToplan()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @" select distinct om.orderid,LocalOrder+'/'+CustomerOrderNo + space(7) + Replace(convert(nvarchar(11),OrderDate,106),' ',' -') As OrderNo 
               From ordermaster Om Inner join OrderLocalConsumption oc On om.orderid=oc.orderid  inner join Orderdetail od On od.orderid=om.orderid inner join 
               V_FinishedItemDetail v On v.item_finished_id=od.Item_Finished_Id inner join UserRights_Category uc On uc.CategoryId=v.CATEGORY_ID inner join
               JobAssigns j on j.orderid=om.orderid 
               Where om.status=0 and SupplierQty=0 and uc.userid=" + Session["varuserid"] + "  and om.orderid  not in(select distinct orderid from OrderProcessPlanning)";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            dgtoplan.DataSource = ds;
            dgtoplan.DataBind();
            Trtoplan.Visible = true;
        }
    }
    protected void FillGridConsumption()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @" select distinct om.orderid,LocalOrder+'/'+CustomerOrderNo + space(7) + Replace(convert(nvarchar(11),OrderDate,106),' ',' -') As OrderNo 
                     From ordermaster om inner join orderdetail od On om.orderid=od.orderid  inner join 
                     V_FinishedItemDetail v On v.item_finished_id=od.Item_Finished_Id inner join UserRights_Category uc On uc.CategoryId=v.CATEGORY_ID inner join JobAssigns j On j.orderid=om.orderid
                     Where om.status=0 and j.SupplierQty=0 and uc.userid =" + Session["varuserid"] + "  and om.orderid not in(select orderid from OrderLocalConsumption )";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Dgtoconsump.DataSource = ds;
            Dgtoconsump.DataBind();
            Trconsump.Visible = true;
        }
    }
    protected void FillDpordertomake()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @" SELECT Distinct LocalOrder+'/'+CustomerOrderNo + space(7) + Replace(convert(nvarchar(11),OrderDate,106),' ',' -') As OrderNo,Om.OrderId As OrderId
                     From OrderMaster OM inner join 
                     JobAssigns j On Om.orderid=j.orderid inner join 
                     orderdetail od On od.orderid=om.orderid and j.ITEM_FINISHED_ID =od.Item_Finished_Id left outer join 
                     PROCESS_ISSUE_DETAIL_9 pi On pi.orderid=om.orderid and pi.Item_Finished_Id=od.Item_Finished_Id inner join 
                     V_FinishedItemDetail v On v.item_finished_id=od.Item_Finished_Id inner join UserRights_Category uc On uc.CategoryId=v.CATEGORY_ID
                     Where om.status=0 and j.SupplierQty<>0 and uc.userid =" + Session["varuserid"] + @"
                     group by LocalOrder,CustomerOrderNo,OrderDate,Om.OrderId,j.Item_Finished_Id
                     Having isnull(sum(SupplierQty),0)>isnull(sum(Qty),0)";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Dgdpordertomake.DataSource = ds;
            Dgdpordertomake.DataBind();
            Trdporder.Visible = true;
        }
    }
    protected void FillISQ()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @" select distinct  CATEGORY_NAME+'  '+ITEM_NAME+'  '+QualityName+'  '+designName+'  '+ColorName+'  '+ShadeColorName+' '+ShapeName as isq
                     From ReOrderQty r left outer join stock s On s.ITEM_FINISHED_ID=r.Item_Finished_id Inner join
                     V_FinishedItemDetail vd On vd.ITEM_FINISHED_ID=r.ITEM_FINISHED_ID inner join 
                     UserRights_Category uc On uc.CategoryId=vd.CATEGORY_ID inner join 
                     ITEM_PARAMETER_MASTER ip On ip.ITEM_FINISHED_ID=vd.ITEM_FINISHED_ID
                     Where isnull(MinStockQty,0)>isnull(Qtyinhand,0) and userid=" + Session["varuserid"] + " and ip.status=1";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GVisq.DataSource = ds;
            GVisq.DataBind();
            Trisq.Visible = true;
        }
    }
    protected void FillRemarkAlert()
    {
        string todate = DateTime.Now.Date.AddDays(-15).ToString("dd-MMM-yyyy");
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @" select  'P.O No '+cast(isnull(pii.PindentIssueid,' ') as varchar)+'  '+om.localorder+' / '+om.customerorderno +' '+isnull(Purhasername,'')  as remark
        from ordermaster Om inner join OrderProcessPlanning op On op.orderid=om.orderid Left outer join purchaseindentissue pii On om.orderid=pii.orderid   
        where pii.PindentIssueid   not in(select distinct PTrackId from PurchaseTracking where RemarkCurrentDate>'" + todate + @"' )
        and pii.status='Pending' and om.status=0 and ProcessId=1";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Dgremark.DataSource = ds;
            Dgremark.DataBind();
            Trremark.Visible = true;
        }
    }
    protected void Fillartalert()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @" select distinct om.orderid, LocalOrder+' /'+CustomerOrderNo orderNo from ordermaster om, orderdetail od 
        Where om.orderid=od.orderid and ( Statusqty <> 0 Or Statuslabel<>0 OR StatusInspectionDate<>0)";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            dgalertart.DataSource = ds;
            dgalertart.DataBind();
            Trartalert.Visible = true;
        }
    }
    protected void dgalertart_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string orderid = ((Label)dgalertart.Rows[e.RowIndex].FindControl("lblOrderNo")).Text;
        Session["ReportPath"] = "Reports/RptAlertArtIndia.rpt";
        Session["CommanFormula"] = "{AlertArtIndia.Orderid}=" + orderid + "";
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("window.open('ReportViewer.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, fullscreen=No, resizable = yes');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
    private void planningAll()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @" select Distinct Top(5) om.Orderid,localorder+' / '+customerorderno orderno from orderdetail od inner join ordermaster om On od.orderid=om.orderid where TAG_FLAG=0 order by om.orderid desc";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblordertag.Text = ds5.Tables[0].Rows[0][1].ToString();
            GvTag.DataSource = ds;
            GvTag.DataBind();
            trorder.Visible = true;
        }
    }
    private void PurchaseAll()
    {
        string Fromdate = DateTime.Now.Date.ToString("dd-MMM-yyyy");
        string todate = DateTime.Now.Date.AddDays(7).ToString("dd-MMM-yyyy");
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @" select distinct top(5) Pindentissueid , 'P.O ' +cast(Pindentissueid as varchar)+'  '+e.empname as poNo 
        From PurchaseIndentIssue pii inner join empinfo e On pii.partyid=e.empid 
        Where pindentissueid not in (select distinct pindentissueid from purchasereceivedetail) 
         Order by Pindentissueid desc";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        //and duedate>='" + Fromdate + "' and duedate<='" + todate + "'
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblpurchase.Text = ds5.Tables[0].Rows[1][1].ToString();
            gvpurchase.DataSource = ds;
            gvpurchase.DataBind();
            trpurchase.Visible = true;
        }
    }
    private void DPorderAll()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @" select distinct Top(5) om.orderid,localorder+' / '+customerorderno orderno  from OrderMaster Om  inner join JobAssigns j On om.orderid=j.orderid 
        Where j.SupplierQty<>0 and Om.orderid  not in(select distinct orderid from PROCESS_ISSUE_DETAIL_9 ) 
        order by orderid desc";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lbldporder11.Text = ds5.Tables[0].Rows[2][1].ToString();
            gvdporder.DataSource = ds;
            gvdporder.DataBind();
            trdporder11.Visible = true;
        }
    }
    private void DpreciveAll()
    {
        string Fromdate = DateTime.Now.Date.ToString("dd-MMM-yyyy");
        string todate = DateTime.Now.Date.AddDays(7).ToString("dd-MMM-yyyy");
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @" select Distinct top(5) 'Supply Ord.No '+ cast(pim.issueorderid as varchar)+' ' +empname as supplyorderno,pim.issueorderid as supplyorder from PROCESS_ISSUE_Master_9 pim inner join 
        PROCESS_ISSUE_DETAIL_9 pid On pim.issueorderid=pid.issueorderid Inner join 
        Empinfo e On e.EmpId = pim.Empid
        Where  ReqByDate>='" + Fromdate + "' and ReqByDate<='" + todate + "' Order by  pim.issueorderid desc";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lbldprec.Text = ds5.Tables[0].Rows[3][1].ToString();
            Gvdprec.DataSource = ds;
            Gvdprec.DataBind();
            trdpreceive.Visible = true;
        }
    }
    private void DpLatedelorderAll()
    {
        string Fromdate = DateTime.Now.Date.ToString("dd-MMM-yyyy");
        string todate = DateTime.Now.Date.AddDays(7).ToString("dd-MMM-yyyy");
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @"select distinct top(5) om.localorder+'/'+om.customerorderno as orderno,om.orderid from ordermaster om inner join 
        orderdetail od On om.orderid=od.orderid left outer join PackingInformation p On om.orderid=p.orderid
        Where om.Dispatchdate>='" + Fromdate + "' and om.Dispatchdate<='" + todate + @"' Group by om.localorder,om.customerorderno,om.orderid
        Having isnull(sum(QtyRequired),0)>isnull(sum(totalpcs),0)";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lbllatedelorder.Text = ds5.Tables[0].Rows[4][1].ToString();
            Gvlatedelorder.DataSource = ds;
            Gvlatedelorder.DataBind();
            trlatedeldate.Visible = true;
        }
    }
    private void DpProductionorderAll()
    {
        string Fromdate = DateTime.Now.Date.ToString("dd-MMM-yyyy");
        string todate = DateTime.Now.Date.AddDays(7).ToString("dd-MMM-yyyy");
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @"select localorder+'/'+customerorderno as orderno,orderid from ordermaster 
        where orderid in(select orderid from JobAssigns where isnull(prod_status,0)<>1 and PreProdAssignedQty<>0) order by orderid desc";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblproductionorder.Text = ds5.Tables[0].Rows[5][1].ToString();
            gvproductionorder.DataSource = ds;
            gvproductionorder.DataBind();
            trproductionorder.Visible = true;
        }
    }
    private void FillGridDelayCustomerOrder()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //        string qry = @" select Distinct Top 10 OrderId,CustomerId,CustomerOrderNo,LocalOrder,DispatchDate,Replace(Convert(varchar(11), DispatchDate,106),' ','-') as DispatchDate2 
        //                        from V_DelayCustomerOrder 
        //                        Where OrderQty > DispatchQty And DispatchDate < GetDate() order by DispatchDate desc";

        string qry = @"select Top 10 CI.CustomerCode, OM.OrderID, OM.CustomerID, OM.CustomerOrderNo, OM.LocalOrder, OM.DispatchDate, 
            Replace(Convert(Varchar(11), DispatchDate, 106), ' ', '-') DispatchDate2 
            From OrderMaster OM(Nolock) 
            JOIN CustomerInfo CI(Nolock) ON OM.CustomerID=CI.CustomerID 
            Where OM.[Status] <> 1 And DispatchDate < DATEADD(dd, DATEDIFF(dd, 0, getdate()), 0) order by OM.DispatchDate desc";
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GVDelayCustomerOrder.DataSource = ds;
            GVDelayCustomerOrder.DataBind();
            TRDelayOrder.Visible = true;
        }
    }
//    private void FillGridDelayPurchaseOrder()
//    {
//        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
//        string qry = @" Select distinct Top 10 CI.CompanyName, EI.EmpName,VDPO.ChallanNo,VDPO.DueDate,Replace(Convert(varchar(11),VDPO.DueDate,106),' ','-') as DueDate2 
//                        From V_DelayPurchaseOrder VDPO JOIN EmpInfo EI ON VDPO.PartyId=EI.EmpID 
//                        JOIN CompanyInfo CI ON VDPO.CompanyId=CI.CompanyId
//                        Where IssQty > RecQty And DueDate < DATEADD(dd, DATEDIFF(dd, 0, getdate()), 0) Order by VDPO.DueDate desc";
//        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
//        if (ds.Tables[0].Rows.Count > 0)
//        {
//            GVDelayPurchaseOrder.DataSource = ds;
//            GVDelayPurchaseOrder.DataBind();
//            TRDelayPurchaseOrder.Visible = true;
//        }
//    }
//    private void FillGridDelayDyeingIndent()
//    {
//        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
//        string qry = @" select distinct Top 10 VDDI.IndentId, CI.CompanyName, EI.EmpName,VDDI.IndentNo,VDDI.ReqDate,Replace(Convert(varchar(11),VDDI.ReqDate,106),' ','-') as DueDate2 
//                        from V_DelayDyeingIndent VDDI JOIN EmpInfo EI ON VDDI.PartyId=EI.EmpID 
//                        JOIN CompanyInfo CI ON VDDI.CompanyId=CI.CompanyId
//                        Where IssQty > RecQty And ReqDate < DATEADD(dd, DATEDIFF(dd, 0, getdate()), 0) order by VDDI.ReqDate desc";
//        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
//        if (ds.Tables[0].Rows.Count > 0)
//        {
//            GVDelayDyeingIndent.DataSource = ds;
//            GVDelayDyeingIndent.DataBind();
//            TRDelayDyeingIndent.Visible = true;
//        }
//    }
//    private void FillGridDelayProductionOrder()
//    {
//        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
//        string qry = @"select distinct Top 10 VDPO.IssueOrderID, CI.CompanyName, EI.EmpName,VDPO.ReqByDate,Replace(Convert(varchar(11),VDPO.ReqByDate,106),' ','-') as DueDate 
//                        from V_DelayProductionOrderNo VDPO JOIN EmpInfo EI ON VDPO.EmpID=EI.EmpID 
//                        JOIN CompanyInfo CI ON VDPO.CompanyId=CI.CompanyId
//                        Where VDPO.IssQty > VDPO.RecQty And VDPO.ReqByDate < DATEADD(dd, DATEDIFF(dd, 0, getdate()), 0) order by VDPO.ReqByDate desc";
//        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, qry);
//        if (ds.Tables[0].Rows.Count > 0)
//        {
//            GVDelayProductionOrder.DataSource = ds;
//            GVDelayProductionOrder.DataBind();
//            TRDelayProductionOrder.Visible = true;
//        }
//    } 
   
}