using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_FrmPNMRawReceiveDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {

            txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[6];
        param[0] = new SqlParameter("@CompanyID", 1);
        param[1] = new SqlParameter("@ProcessID", 5);
        param[3] = new SqlParameter("@FromDate", txtfromDate.Text);
        param[4] = new SqlParameter("@ToDate", txttodate.Text);
        param[5] = new SqlParameter("@Type", 1);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetProcessDirectRawDetail", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGIndentRawIssueDetail.DataSource = ds.Tables[0];
            DGIndentRawIssueDetail.DataBind();
        }
    }

    protected void lnkbtnToOpenPRMDetail_Click(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;
        GridViewRow grv = lnk.NamingContainer as GridViewRow;
        int lblPRMID = Convert.ToInt16(((Label)DGIndentRawIssueDetail.Rows[grv.RowIndex].FindControl("lblPRMID")).Text);

        string qry = @"Select Distinct '' EmpName, '' Address, '' PhoneNo, '' Mobile, '' EmailAdd, vc.companyid, Vc.CompanyName, BM.BranchAddress CompanyAddress, 
            BM.PhoneNo ComPanyPhoneNo, CompanyFaxNo, VC.TinNo, IPM.Category_Name, IPM.Item_Name, IPM.QualityName, IPM.DesignName, IPM.ColorName, IPM.ShapeName, 
            IPM.ShadeColorName, IPM.SizeMtr, IPM.SizeFt, PRM.CHALLANNO + ' / ' + PRM.ChampoChallanNo + ' / ' + PRT.ChampoIndentNo ChallanNo, 
            PNM.ShortName, 0 Indentid, GatepassNo, PRT.PRMid, PRTid, PRT.QTY IssueQuantity, 
            prt.LotNo, GM.GodownNAme, PRT.FinishedId, 0 Finish_Type_Id, '' IndentNo, PRM.PROCESSID, u.unitname, PRM.Date, 0 orderid, '' localorder, 
            '' customerorderno,'' reqdate,prt.remark,replace(convert(varchar(11),prm.date,106),' ','-') Issuedate, 0 O_FINISHED_TYPE_ID, prt.GoDownID, 
            0 CanQty,0 flagsize, PRM.MASTERCOMPANYID, PNM.Process_Name, '' Buyercode, Prt.TagNo, BM.GSTNo GSTIN, '' EmpGStno, ISNULL(PRT.BINNO,0) BinNo, 0 ManualRate, 
            0 GSTType, 0 CGST, 0 SGST, 0 IGST, isnull(IPM.HSNCode,'') HSNCode, '' VehicleNo, '' DriverName, '' EWayBillNo, 0 PurchaseRate, 0 PurchaseAmt, 
            0 CGSTAmt, 0 SGSTAmt, 0 IGSTAmt , isnull(NU.UserName,'') as UserName, PRM.ChampoChallanNo, PRT.ChampoIndentNo 
            From PP_ProcessDirectRawMaster PRM(Nolock)   
            JOIN PP_ProcessDirectRawTran PRT(Nolock) on PRT.PRMid = PRM.PRMid  
            JOIN V_FinishedItemDetail IPM(Nolock) on IPM.Item_Finished_Id = PRT.FinishedId  
            JOIN BRANCHMASTER BM(Nolock) ON BM.ID = PRM.BranchID   
            JOIN GodownMaster GM(Nolock) on GM.GodownId=PRT.GodownId   
            JOIN V_CompanyInfo VC(Nolock) on Prm.CompanyId=VC.CompanyId   
            JOIN Process_Name_Master PNM(Nolock) on PRM.ProcessId=PNM.Process_Name_ID   
            JOIN unit u(Nolock) on u.unitid=prt.unitid   
            JOIN NewUserDetail NU(NoLock) ON PRM.UserId=NU.UserId 
            Where PRM.PRMID = " + lblPRMID;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\IndentRawDirectIssueToPNM.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\IndentRawDirectIssueToPNM.xsd";
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