using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Data.SqlTypes;
public partial class Masters_Purchase_FrmPurchaseReceiveStockEntry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            string Qry = @"select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA 
            Where CI.CompanyId=CA.CompanyId And CA.USERID=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by CompanyId 
            Select ID, BranchName 
            From BRANCHMASTER BM(nolock) 
            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"];

            DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, DSQ, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, DSQ, 1, false, "");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
                CompanySelectedIndexChanged();
            }

            DDPartyName.Focus();
        }
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedIndexChanged();
    }
    private void CompanySelectedIndexChanged()
    {
        UtilityModule.ConditionalComboFill(ref DDPartyName, @"Select Distinct EI.EmpId, EI.EmpName 
            From PurchaseReceiveMaster a(nolock) 
            JOIN EmpInfo EI(Nolock) ON EI.Empid = a.Partyid 
            JOIN PurchaseReceiveDetail b(nolock) ON b.PurchaseReceiveId = a.PurchaseReceiveId 
            Where b.StockUpdateFlag = 0 And a.CompanyID = " + DDCompanyName.SelectedValue + " And IsNull(a.BranchID, 0) = " + DDBranchName.SelectedValue + @" And 
            a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " Order By EI.EmpName ", true, "--Select Employee--");
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select Distinct b.PIndentIssueID, c.ChallanNo 
        From PurchaseReceiveMaster a(nolock) 
        JOIN PurchaseReceiveDetail b(nolock) ON b.PurchaseReceiveId = a.PurchaseReceiveId 
        JOIN PurchaseIndentIssue c(nolock) ON c.PIndentIssueID = b.PIndentIssueID 
        Where b.StockUpdateFlag = 0 And a.CompanyID = " + DDCompanyName.SelectedValue + " And a.BranchID = " + DDBranchName.SelectedValue + " And a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @" And 
        a.PartyID = " + DDPartyName.SelectedValue + " Order By b.PIndentIssueID Desc", true, "--SELECT--");
    }

    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChallanNoSelectedChange();
    }
    private void ChallanNoSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref ddlrecchalanno, @"Select Distinct a.PurchaseReceiveId, a.Receiveno + ' / ' + a.BillNo ChallanNo 
        From PurchaseReceiveMaster a(nolock) 
        JOIN PurchaseReceiveDetail b(nolock) ON b.PurchaseReceiveId = a.PurchaseReceiveId And b.PIndentIssueId = " + DDChallanNo.SelectedValue + @" 
        Where b.StockUpdateFlag = 0 And a.CompanyID = " + DDCompanyName.SelectedValue + " And a.BranchID = " + DDBranchName.SelectedValue + @" And 
        a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " And a.PartyID = " + DDPartyName.SelectedValue + " Order By Receiveno + ' / ' + BillNo", true, "--SELECT--");
    }
    protected void ddlrecchalanno_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_grid();
    }

    private void fill_grid()
    {
        DGPurchaseReceiveDetail.DataSource = Fill_Grid_Data();
        DGPurchaseReceiveDetail.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        string strsql;
        DataSet ds = null;
        try
        {
            strsql = @"Select PRD.PurchaseReceiveDetailId, Replace(Replace(FID.Item_Name + ' / ' + Isnull(FID.QualityName, '') + ' / ' + isnull(FID.DesignName, '') + ' / ' + 
                    isnull(FID.ColorName, '') + ' / ' + isnull(FID.ShadeColorName, '') + ' / ' + isnull(FID.ShapeName, '') + ' / ' + 
                    isnull(FID.SizeFt, ''), '/  / ', ''), '  / ', '') ItemDescription, Prd.Qty 
                    From  PurchaseReceiveMaster prm(Nolock) 
                    join PurchaseReceiveDetail PRD(Nolock) on prd.purchasereceiveid = prm.purchasereceiveid 
                    Inner join V_FinishedItemDetail FID(Nolock) on FID.Item_Finished_Id = PRD.FinishedId 
                    Where prm.PurchaseReceiveId = " + ddlrecchalanno.SelectedValue + " And prm.MasterCompanyId = " + Session["varMasterCompanyIDForERP"];

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/FrmPurchaseReceiveStockEntry.aspx");
        }
        return ds;
    }
    public void OpenNewWidow(string url)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[5];
            _arrpara[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@PurchaseReceiveId", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@UserId", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _arrpara[4] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            _arrpara[0].Value = DDCompanyName.SelectedValue;
            _arrpara[1].Value = ddlrecchalanno.SelectedValue;
            _arrpara[2].Value = Session["varuserid"];
            _arrpara[3].Value = Session["varMasterCompanyIDForERP"];
            _arrpara[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SavePurchaseReceiveStockEntry", _arrpara);
            Tran.Commit();
            if (_arrpara[4].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('" + _arrpara[4].Value + "');", true);
            }
            ChallanNoSelectedChange();
            DGPurchaseReceiveDetail.DataSource = null;
            DGPurchaseReceiveDetail.DataBind();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/FrmPurchaseReceiveStockEntry.aspx");
            Tran.Rollback();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('" + ex.Message + "');", true);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DGPurchaseReceiveDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGPurchaseReceiveDetail, "Select$" + e.Row.RowIndex);
        }
    }
}