using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class Masters_Carpet_FrmAlert : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            if (Request.QueryString["ZZZ"] != "1")
            {
                zzz.Style.Add("display", "none");
            }
            if (Request.QueryString["VarFlag"] == "1")
            {
                trVarFlag1.Visible = true;
                Fill_GridForProduction();
            }
            if (Request.QueryString["VarFlag"] == "2")
            {
                trVarFlag2.Visible = true;
                Fill_GridForOrderProcessProgram();
            }
            if (Request.QueryString["VarFlag"] == "3")
            {
                trVarFlag3.Visible = true;
                Fill_GridForPurchaseApproval();
            }
            if (Request.QueryString["VarFlag"] == "4")
            {
                trVarFlag4.Visible = true;
                Fill_GridForPurchaseOrder();
            }
            if (Request.QueryString["VarFlag"] == "5")
            {
                trVarFlag5.Visible = true;
                Fill_GridForPlanning();
            }
            if (Request.QueryString["VarFlag"] == "6")
            {
                trVarFlag6.Visible = true;
                UtilityModule.ConditionalComboFill(ref DDUserName, "Select UserId,UserName+' -- '+LoginName from NewUserDetail Where Companyid=" + Session["varMasterCompanyIDForERP"], true, "----Select ----");
                Fill_GridToDoManagment();
                logo();
                BtnRefresh.Visible = true;
            }
            if (Request.QueryString["VarFlag"] == "7")
            {
                trVarFlag7.Visible = true;
                Fill_GridToDoSampling();
                logo();
                BtnRefresh.Visible = true;
            }
        }
    }
    private void Fill_GridToDoSampling()
    {
        string str1 = @"Select SrNo,Case When PriorityLevel=0 Then 'Normal' Else Case When PriorityLevel=1 Then 'Urgent' Else 'Top Urgent' End End PriorityLevel,BuyerCode,DNo,DName,Technique,Size,
        RawMaterialComposition,QualityOfRaw,WashUnWash,PileHeight,PlWt,TWt,replace(convert(varchar(11),DispDate,106), ' ','-') DispDate,StatusOfDispatch,CourierNo,TrackingNo,ColorNoS,Remark,UserId,MasterCompanyID 
        From SamplingToDo Where MasterCompanyID=" + Session["VarcompanyNo"] + "  Order By BuyerCode";
        DataSet Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str1);
        DGToDoSampling.DataSource = Ds1.Tables[0];
        DGToDoSampling.DataBind();
    }
    private void Fill_GridToDoManagment()
    {
        string Str = @"Select SrNo,ND.UserName,WorkToDo,Remark,Case When PriorityLevel=0 Then 'Normal' Else Case When PriorityLevel=1 Then 'Urgent' Else 'Top Urgent' End End PriorityLevel,replace(convert(varchar(11),DueDate,106), ' ','-') DueDate,
        DATEDIFF(day, DueDate, GETDATE()) LateByDays,JobStatus From ManagmentToDo MTD,NewUserDetail ND Where MTD.Userid=ND.Userid And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Distinct UserId From FormName F,UserRights UR Where F.MEnuID=UR.MenuID And NavigateURL='../Masters/Campany/FrmTodoManagement.aspx' And Userid=" + Session["varuserid"]);
        if (Ds.Tables[0].Rows.Count == 0)
        {
            Str = Str + " And MTD.UserID=" + Session["varuserid"];
            divusername.Visible = false;
        }
        if (DDUserName.SelectedIndex > 0)
        {
            Str = Str + " And MTD.UserID=" + DDUserName.SelectedValue;
        }
        DataSet Ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        DGToDoManagment.DataSource = Ds1.Tables[0];
        DGToDoManagment.DataBind();
    }
    private void Fill_GridForProduction()
    {
        DataSet ds = null;
        int VarProcessId = Convert.ToInt32(Request.QueryString["ProcessID"]);
        string Str = "";
        if (VarProcessId == 1)
        {
            Str = @"Select CI.CustomerCode,OM.LocalOrder+' / '+OM.CustomerOrderNo OrderNo,replace(convert(varchar(11),OM.OrderDate,106), ' ','-') OrderDate,
            replace(convert(varchar(11),OM.ProdReqDate,106), ' ','-') ProdReqDate,VF.ITEM_NAME ItemName,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName Shape,Case When OM.OrderUnitId=1 Then VF.SizeMtr Else SizeFt End Size,PreProdAssignedQty+ProdDemageQty-((IsNull(Sum(PD.Qty),0))-IsNull(Sum(PD.RejectQty),0)-IsNull(Sum(PD.CancellQty),0)) Qty
            From OrderMaster OM,customerinfo CI,V_FinishedItemDetail VF,JobAssigns JA LEFT OUTER JOIN PROCESS_ISSUE_DETAIL_" + VarProcessId + @" PD ON PD.OrderId=JA.OrderId And 
            PD.Item_Finished_Id=JA.Item_Finished_Id Where OM.CustomerId=CI.CustomerId And OM.OrderId=JA.OrderId And JA.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
            Group By CI.Customercode,OM.LocalOrder,OM.CustomerOrderNo,VF.ITEM_NAME,VF.QualityName,VF.designName,VF.ColorName,VF.ShapeName,
            OM.OrderUnitId,VF.SizeMtr,SizeFt,JA.Item_Finished_Id,PreProdAssignedQty,OM.OrderId,ProdDemageQty,OM.OrderDate,OM.ProdReqDate
            Having PreProdAssignedQty+ProdDemageQty>((IsNull(Sum(PD.Qty),0))-IsNull(Sum(PD.RejectQty),0)-IsNull(Sum(PD.CancellQty),0)) Order By CI.Customercode,OM.OrderId";
        }
        else if (VarProcessId == 9)
        {
            Str = @"Select OM.OrderID,CI.CustomerCode,OM.LocalOrder+' / '+OM.CustomerOrderNo OrderNo,replace(convert(varchar(11),OM.OrderDate,106), ' ','-') OrderDate,
        replace(convert(varchar(11),OM.ProdReqDate,106), ' ','-') ProdReqDate,VF.ITEM_NAME ItemName,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName Shape,
        Case When OM.OrderUnitId=1 Then VF.SizeMtr Else SizeFt End Size,SupplierQty-((IsNull(Sum(PD.Qty),0))-IsNull(Sum(PD.RejectQty),0)-IsNull(Sum(PD.CancellQty),0)) Qty
        From OrderMaster OM,customerinfo CI,V_FinishedItemDetail VF,JobAssigns JA LEFT OUTER JOIN PROCESS_ISSUE_DETAIL_" + VarProcessId + @" PD ON PD.OrderId=JA.OrderId And 
        PD.Item_Finished_Id=JA.Item_Finished_Id Where OM.CustomerId=CI.CustomerId And OM.OrderId=JA.OrderId And JA.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID And 
        SupplierQty>0 And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Group By CI.Customercode,OM.LocalOrder,OM.CustomerOrderNo,VF.ITEM_NAME,VF.QualityName,VF.designName,VF.ColorName,VF.ShapeName,
        OM.OrderUnitId,VF.SizeMtr,SizeFt,JA.Item_Finished_Id,OM.OrderId,SupplierQty,OM.OrderDate,OM.ProdReqDate
        Having SupplierQty>((IsNull(Sum(PD.Qty),0))-IsNull(Sum(PD.RejectQty),0)-IsNull(Sum(PD.CancellQty),0)) Order By CI.Customercode,OM.OrderId";
        }
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        DGForProduction.DataSource = ds;
        DGForProduction.DataBind();

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select Distinct CI.CompanyName,EI.EmpName,PIM.SupplyOrderNo IssueOrderId,replace(convert(varchar(11),PIM.AssignDate,106), ' ','-') AssignDate,
        replace(convert(varchar(11),PID.ReqByDate,106), ' ','-') ReqDate From PROCESS_ISSUE_MASTER_" + VarProcessId + " PIM,PROCESS_ISSUE_DETAIL_" + VarProcessId + @" PID,CompanyInfo CI,EmpInfo EI
        Where PIM.IssueOrderId=PID.IssueOrderId And PIM.Companyid=CI.CompanyId And PIM.Empid=EI.Empid And PQty>0 And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By EI.EmpName");
        DGForProductionPending.DataSource = ds;
        DGForProductionPending.DataBind();
    }
    private void Fill_GridForOrderProcessProgram()
    {
        DataSet ds = null;
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select CustomerCode,OrderNo,OrderDate From View_OrderProcessProgram Order By CustomerCode,OrderId");
        DGOrderProcessProgram.DataSource = ds;
        DGOrderProcessProgram.DataBind();
    }
    private void Fill_GridForPurchaseApproval()
    {
        DataSet ds = null;
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select CI.CompanyName,D.DepartmentName,EI.EmpName PartyName,PIM.PIndentNo,replace(convert(varchar(11),PIM.Date,106), ' ','-') Date From PurchaseIndentMaster PIM,CompanyInfo CI,EmpInfo EI,Department D Where PIM.CompanyId=CI.CompanyId And PIM.PartyId=EI.EmpId And PIM.DepartmentId=D.DepartmentId And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And PIndentId not in (Select PIndentNo From PurchaseIndentApproval)");
        DGPurchaseApproval.DataSource = ds;
        DGPurchaseApproval.DataBind();
    }
    private void Fill_GridForPurchaseOrder()
    {
        DataSet ds = null;
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select CI.CompanyName,EI.EmpName PartyName,PIM.PIndentNo,replace(convert(varchar(11),PIM.Date,106), ' ','-') Date,VPI.ApprovalDate,ApprovedBy,ApprovalNo 
             From CompanyInfo CI,EmpInfo EI,PurchaseIndentMaster PIM,View_PurchaseIndentQty VPI Left Outer Join View_PurchaseIndentIssueQty VPII ON 
             VPI.PIndentId=VPII.IndentID And VPI.FinishedId=VPII.FinishedID Where PIM.CompanyId=CI.CompanyId And PIM.PartyId=EI.EmpId And VPI.PIndentId=PIM.PIndentId And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
             Group By CI.CompanyName,EI.EmpName,PIM.PIndentNo,PIM.Date,VPI.ApprovalDate,IndentQty,VPI.ApprovedBy,VPI.ApprovalNo  Having IndentQty>Sum(IsNull(IssQty,0))");
        DGPurchaseOrder.DataSource = ds;
        DGPurchaseOrder.DataBind();

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT Distinct PII.PIndentIssueID,CI.CompanyName,EI.EmpName,PII.ChallanNo,
        replace(convert(varchar(11),PII.Date,106), ' ','-') Date,replace(convert(varchar(11),PII.DueDate,106), ' ','-') DueDate
        FROM PurchaseIndentIssue PII INNER JOIN View_PurchaseOrderQty VPO ON PII.PIndentIssueID=VPO.PIndentIssueID INNER JOIN
        EmpInfo EI ON PII.PartyID=EI.EmpId INNER JOIN CompanyInfo CI ON PII.CompanyID=CI.CompanyId And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" LEFT OUTER JOIN 
        View_PurchaseRecQty VPR ON VPO.PIndentIssueID=VPR.PIndentIssueID AND VPO.FinishedID=VPR.FinishedId Group By 
        PII.PIndentIssueID,CI.CompanyName,EI.EmpName,PII.ChallanNo,PII.Date,PII.DueDate,VPO.PIssQty Having VPO.PIssQty>SUM(Isnull(VPR.PRecQty,0)) Order By EI.EmpName");
        DGPurchaseOrderPending.DataSource = ds;
        DGPurchaseOrderPending.DataBind();
    }
    private void Fill_GridForPlanning()
    {
        DataSet ds = null;
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select Distinct om.Orderid,localorder+' / '+customerorderno orderno ,replace(convert(varchar(11),orderdate,106),' ','-' ) as OrderDate from orderdetail od inner join ordermaster om On od.orderid=om.orderid where TAG_FLAG=0 or tag_flag is null order by om.orderid desc");
        Dgplaning.DataSource = ds;
        Dgplaning.DataBind();
    }
    protected void DGToDoManagment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGToDoManagment, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGForProduction_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGForProduction, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGForProductionPending_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGForProductionPending, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGOrderProcessProgram_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderProcessProgram, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGPurchaseApproval_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGPurchaseApproval, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGPurchaseOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGPurchaseOrder, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGPurchaseOrderPending_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGPurchaseOrderPending, "Select$" + e.Row.RowIndex);
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        Response.Clear();
        string attachment = "attachment; filename=OrderHistory.xls";
        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        if (Request.QueryString["VarFlag"] == "1")
        {
            DGForProduction.Style.Add("font-size", "1em");
            DGForProduction.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString() + "<Table><tr></tr><tr></tr><tr></tr><tr></tr></table>");

            System.IO.StringWriter stringWrite1 = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite1 = new HtmlTextWriter(stringWrite1);

            DGForProductionPending.Style.Add("font-size", "1em");
            DGForProductionPending.RenderControl(htmlWrite1);
            Response.Write(stringWrite1.ToString());
        }
        if (Request.QueryString["VarFlag"] == "2")
        {
            DGOrderProcessProgram.Style.Add("font-size", "1em");
            DGOrderProcessProgram.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
        }
        if (Request.QueryString["VarFlag"] == "3")
        {
            DGPurchaseApproval.Style.Add("font-size", "1em");
            DGPurchaseApproval.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
        }
        if (Request.QueryString["VarFlag"] == "4")
        {
            DGPurchaseOrder.Style.Add("font-size", "1em");
            DGPurchaseOrder.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString() + "<Table><tr></tr><tr></tr><tr></tr><tr></tr></table>");

            System.IO.StringWriter stringWrite1 = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite1 = new HtmlTextWriter(stringWrite1);

            DGPurchaseOrderPending.Style.Add("font-size", "1em");
            DGPurchaseOrderPending.RenderControl(htmlWrite1);
            Response.Write(stringWrite1.ToString());
        }

        if (Request.QueryString["VarFlag"] == "5")
        {
            Dgplaning.Style.Add("font-size", "1em");
            Dgplaning.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
        }
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        // Confirms that an HtmlForm control is rendered for the
        // specified ASP.NET server control at run time.
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        PostBackTrigger trigger = new PostBackTrigger();
        trigger.ControlID = BtnPreview.ID;
        UpdatePanel1.Triggers.Add(trigger);
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
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void BtnRefresh_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["VarFlag"] == "6")
        {
            Fill_GridToDoManagment();
        }
        if (Request.QueryString["VarFlag"] == "7")
        {
            Fill_GridToDoSampling();
        }
    }
    protected void DGToDoManagment_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGToDoManagment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, " Select Userid From ManagmentToDo Where SrNo=" + DGToDoManagment.DataKeys[e.RowIndex].Value + " And Userid=" + Session["varuserid"]);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update ManagmentToDo Set JobStatus='DONE' Where SrNo=" + DGToDoManagment.DataKeys[e.RowIndex].Value);
            Fill_GridToDoManagment();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Only authenticated persons can update job status...');", true);
        }
    }
    protected void DDUserName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_GridToDoManagment();
    }
    protected void DGToDoSampling_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGToDoSampling_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGToDoSampling, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGToDoSampling_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGToDoSampling.EditIndex = -1;
        Fill_GridToDoSampling();
    }
    protected void DGToDoSampling_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGToDoSampling.EditIndex = e.NewEditIndex;
        Fill_GridToDoSampling();
    }
    protected void DGToDoSampling_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int VarSrNo = Convert.ToInt32(DGToDoSampling.DataKeys[e.RowIndex].Value);
        string VarCourierNo = ((TextBox)DGToDoSampling.Rows[e.RowIndex].FindControl("TxtDGCourierNo")).Text;
        string VarTrackingNo = ((TextBox)DGToDoSampling.Rows[e.RowIndex].FindControl("TxtDGTrackingNo")).Text;
        SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update SamplingToDo Set CourierNo='" + VarCourierNo + "',TrackingNo='" + VarTrackingNo + "' Where SrNo=" + VarSrNo);
        DGToDoSampling.EditIndex = -1;
        Fill_GridToDoSampling();
    }
    protected void DGToDoSampling_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGToDoSampling.PageIndex = e.NewPageIndex;
        Fill_GridToDoSampling();
    }
}