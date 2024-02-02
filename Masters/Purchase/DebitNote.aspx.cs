using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Purchase_DebitNote : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["DebitNoteid"] = 0;
            ViewState["DebitNoteDetailid"] = 0;
            DataSet ds = SqlHelper.ExecuteDataset(@"Select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA 
            Where CI.CompanyId=CA.CompanyId And CA.USERID=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by Companyname");

            CommanFunction.FillComboWithDS(DDCompany, ds, 0);

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                CompanySelectedIndexChanged();
            }
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedIndexChanged();
    }
    private void CompanySelectedIndexChanged()
    {
        UtilityModule.ConditionalComboFill(ref DDPartyName, "Select EmpId,EmpName from Empinfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDReceiveNo, "select PurchaseReceiveId,GateInNo from PurchaseReceiveMaster where PartyId=" + DDPartyName.SelectedValue + "  and CompanyId=" + DDCompany.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
    }
    protected void DDReceiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_Gride();
    }
    private void fill_Gride()
    {
        DGItemDetail.DataSource = GetStock();
        DGItemDetail.DataBind();
    }
    private DataSet GetStock()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string strsql = @"select PRD.PurchaseReceiveDetailId,Item_Name,isnull(QualityName,'')+'/'+isnull(DesignName,'')+'/'+isnull(ColorName,'') +'/'+isnull(ShadeColorName,'')+'/'+isnull(ShapeName,'')+'/'+Isnull(SizeFt,'') ItemDescription,LotNo,GodownName,QTY,isnull(ReturnQty,0) ReturnQty from PurchaseReceiveDetail PRD 
                            Inner join V_FinishedItemDetail IPM on IPM.Item_Finished_Id=PRD.FinishedId Left Outer Join PDebitNotedetail DND on DND.PurchaseReceiveDetailId=PRD.PurchaseReceiveDetailId
                            Inner join GodownMaster G on G.GodownId=PRD.GodownId where PRD.PurchaseReceiveId=" + DDReceiveNo.SelectedValue + " And IPM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/DebitNote.aspx");
            //Logs.WriteErrorLog("error");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return ds;
    }
    protected void DGItemDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            //  e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGItemDetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            int n = DGItemDetail.Rows.Count;
            for (int i = 0; i < n; i++)
            {
                GridViewRow row = DGItemDetail.Rows[0];
                string Id = row.Cells[0].Text;
                string LotNo = row.Cells[4].Text;
                string ReturnQty = ((TextBox)row.Cells[6].FindControl("TxtReturnQty")).Text;
                string TotalQty = row.Cells[5].Text;
                SqlParameter[] _arrPara = new SqlParameter[11];
                _arrPara[0] = new SqlParameter("@DebitNoteid", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@DebitNo", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@Date", SqlDbType.DateTime);
                _arrPara[3] = new SqlParameter("@Userid", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@VarCompanyId", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@DebitNoteDetailid", SqlDbType.Int);
                _arrPara[6] = new SqlParameter("@PurchaseReceiveDetailId", SqlDbType.Int);
                _arrPara[7] = new SqlParameter("@PurchaseReceiveId", SqlDbType.Int);
                _arrPara[8] = new SqlParameter("@TotalQty", SqlDbType.Float);
                _arrPara[9] = new SqlParameter("@ReturnQty", SqlDbType.Float);
                _arrPara[10] = new SqlParameter("@LotNo", SqlDbType.NVarChar, 50);
                _arrPara[0].Direction = ParameterDirection.InputOutput;
                _arrPara[0].Value = ViewState["DebitNoteid"];
                _arrPara[1].Direction = ParameterDirection.InputOutput;
                _arrPara[1].Value = TxtDebtNoteNo.Text;
                _arrPara[2].Value = TxtDate.Text;
                _arrPara[3].Value = Session["varuserid"].ToString();
                _arrPara[4].Value = Session["varMasterCompanyIDForERP"].ToString();
                _arrPara[5].Value = ViewState["DebitNoteDetailid"];
                _arrPara[6].Value = Id;
                _arrPara[7].Value = DDReceiveNo.SelectedValue;
                _arrPara[8].Value = TotalQty;
                _arrPara[9].Value = ReturnQty;
                _arrPara[10].Value = LotNo;
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_PDebitNote", _arrPara);
                TxtDebtNoteNo.Text = _arrPara[1].Value.ToString();
                ViewState["DebitNoteid"] = _arrPara[0].Value.ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/DebitNote.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("../../main.aspx");
    }
    protected void DGItemDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
