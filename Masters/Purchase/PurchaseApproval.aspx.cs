using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Purchase_PurchaseApproval : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["IndentNo"] = null;
            ViewState["PIApprovalId"] = 0;
            CommanFunction.FillCombo(DDCompanyName, "select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.USERID=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + " Order by Companyname");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
                CompanySelectedIndexChanged();
            }

            TxtApprovalDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedIndexChanged();
    }
    private void CompanySelectedIndexChanged()
    {
        CHkPindentNo.Items.Clear();
        UtilityModule.ConditionalComboFill(ref DDDepartment, "Select DepartmentId,DepartmentName from Department Where masterCompanyId=" + Session["varCompanyId"] + " order by DepartmentName", true, "--Select Department--");
    }
    protected void DDDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditonalChkBoxListFill(ref CHkPindentNo, "Select PIndentId,replace(Str(PIndentNo)+' / '+replace(convert(varchar(11),date,106),' ','-'),'  ','')  from PurchaseIndentMaster where FlagApproval=0 and DepartmentId=" + DDDepartment.SelectedValue + " and CompanyId="+DDCompanyName.SelectedValue+" And masterCompanyId=" + Session["varCompanyId"] + " Order BY PIndentNo");
        
    }
    protected void CHkPindentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["IndentNo"] = null;
        ViewState["IndentNo"] = CHkPindentNo.SelectedValue;
        foreach (ListItem li in CHkPindentNo.Items)
        {
            if (li.Selected == true )
            {
                if (li.Value == ViewState["IndentNo"])
                    ViewState["IndentNo"] = li.Value;
                else
                    li.Selected = false;
            }
            else
                li.Selected = false;
        }
        ViewState["IndentNo"] = ViewState["IndentNo"] != null ? ViewState["IndentNo"] : 0;
        fill_grid(ViewState["IndentNo"].ToString());
    }
    private void fill_grid(string PindentNo)
    {
        DGPIndentDetail.DataSource = Fill_Grid_Data(PindentNo);
        DGPIndentDetail.DataBind();
    }
    private DataSet Fill_Grid_Data(string PindentNo)
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = @"select Distinct PID.PIndentDetailId,PIndentNo,D.DepartmentName,EmpName PartyName,Category_Name+'  '+Item_Name+'  '+ QualityName+'  '+isnull(DesignName,'')+isnull(ColorName,'')+'  '+isnull(ShadeColorName,'')+'  '+isnull(ShapeName,'')  ItemDescription,SizeMtr,SizeFt,Qty,UnitName,pid.itemremark as iremark,isnull(pid.rate ,0) as rate
                            From PurchaseIndentMaster PIM inner Join PurchaseIndentDetail PID  on PIM.PIndentId=PID.PIndentId inner join 
                            V_Companyinfo VC on PIM.CompanyId=VC.CompanyId inner join V_Employeeinfo VE ON VE.EmpId=PIM.PartyId inner join
                            Department D on D.DepartmentId=PIM.DepartmentId inner join V_ItemDetail VI on VI.Item_Finished_Id=PID.FinishedId Left outer Join 
                            Unit on Unit.UnitId=PID.UnitId 
                            Where PIM.PIndentId in (select * from Split('" + PindentNo + "',',')) And PIM.masterCompanyId=" + Session["varCompanyId"];
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchaseApproval.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        return ds;
    }
    protected void DGPIndentDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGPIndentDetail.PageIndex = e.NewPageIndex;
        fill_grid(ViewState["IndentNo"].ToString());
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        if (ViewState["IndentNo"] != null)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

            try
            {
                con.Open();
                SqlParameter[] _arrPara = new SqlParameter[9];
                _arrPara[0] = new SqlParameter("@PIApprovalId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@PIndentNo", SqlDbType.NVarChar,4000);
                _arrPara[2] = new SqlParameter("@ApprovalDate", SqlDbType.DateTime);
                _arrPara[3] = new SqlParameter("@userId", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@ApprovedBy", SqlDbType.NVarChar,200);
                _arrPara[6] = new SqlParameter("@Qty", SqlDbType.Float);
                _arrPara[7] = new SqlParameter("@Pindentdetailid", SqlDbType.Int);
                _arrPara[0].Direction = ParameterDirection.InputOutput;
                _arrPara[0].Value =ViewState["PIApprovalId"];
                _arrPara[1].Value = ViewState["IndentNo"];
                _arrPara[2].Value = TxtApprovalDate.Text;
                _arrPara[3].Value = Session["varuserid"].ToString();
                _arrPara[4].Value = Session["varCompanyId"].ToString();
                _arrPara[5].Value = TxtApprovedBy.Text;
                for (int i = 0; i < DGPIndentDetail.Rows.Count; i++)
                {
                    _arrPara[6].Value = ((TextBox)DGPIndentDetail.Rows[i].FindControl("TxtQty")).Text;
                    _arrPara[7].Value = DGPIndentDetail.DataKeys[i].Value.ToString();
                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_PIndentApproval", _arrPara);
                }
                LblErr.Text = "Data saved..............";
                UtilityModule.ConditonalChkBoxListFill(ref CHkPindentNo, "Select PIndentId,PIndentNo from PurchaseIndentMaster PIM where FlagApproval=0 and DepartmentId=" + DDDepartment.SelectedValue + " and PartyId=" + DDPartyName.SelectedValue + " And masterCompanyId=" + Session["varCompanyId"] + " Order BY PIM.PIndentNo");
                BtnPreview.Enabled = true;
                Session["ReportPath"] = "Reports/PGenrateIndentApproval.rpt";
                Session["CommanFormula"] = "{V_PurchaseIndentApprovalReport.PIApprovalId} =" +_arrPara[0].Value + "";
                ViewState["PIApprovalId"] = _arrPara[0].Value;
                fill_grid("0");
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Purchase/PurchaseApproval.aspx");
                LblErr.Text = ex.Message;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/main.aspx");
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        Session["ReportPath"] = "Reports/PGenrateIndentApproval.rpt";
        Session["CommanFormula"] = "";
        //OpenNewWidow("../../ReportViewer.aspx");
        Report();
    }
    private void Report()
    {
        string qry = @" SELECT V_Companyinfo.CompanyName,V_Companyinfo.CompanyAddress,V_Companyinfo.CompanyPhoneNo,V_Companyinfo.CompanyFaxNo,V_Companyinfo.TinNo,V_EmployeeInfo.EmpName,V_EmployeeInfo.EmpAddress,V_EmployeeInfo.EmpPhoneNo,V_EmployeeInfo.EmpMobile,V_PurchaseIndentApprovalReport.ItemDescription,V_PurchaseIndentApprovalReport.PIndentNo,V_PurchaseIndentApprovalReport.Date,V_PurchaseIndentApprovalReport.Qty,V_PurchaseIndentApprovalReport.UnitName,V_PurchaseIndentApprovalReport.PIApprovalId,V_PurchaseIndentApprovalReport.ApprovedBy,V_PurchaseIndentApprovalReport.remark,V_PurchaseIndentApprovalReport.itemremark,V_Companyinfo.GSTIN,V_EmployeeInfo.EMPGSTIN
                     FROM V_Companyinfo INNER JOIN (V_EmployeeInfo INNER JOIN V_PurchaseIndentApprovalReport ON V_EmployeeInfo.EmpId=V_PurchaseIndentApprovalReport.PartyID) ON V_Companyinfo.CompanyId=V_PurchaseIndentApprovalReport.CompanyId And V_PurchaseIndentApprovalReport.MasterCompanyId=" + Session["varCompanyId"] + @"
                     Where V_PurchaseIndentApprovalReport.PIApprovalId=" + Convert.ToInt32(ViewState["PIApprovalId"])+"";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\PGenrateIndentApprovalNEW.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\PGenrateIndentApprovalNEW.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    public void OpenNewWidow(string url)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
    }
    //protected void DGPIndentDetail_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //Add CSS class on header row.
    //    if (e.Row.RowType == DataControlRowType.Header)
    //        e.Row.CssClass = "header";
    //    //Add CSS class on normal row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Normal)
    //        e.Row.CssClass = "normal";
    //    //Add CSS class on alternate row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Alternate)
    //        e.Row.CssClass = "alternate";
    //}
   
}