using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_RawMaterial_FrmAnyItemIssue : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select CI.CompanyId,CompanyName 
                        From CompanyInfo CI 
                        JOIN Company_Authentication CA on CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + @" And 
                        CA.MasterCompanyid=" + Session["varCompanyId"] + @" order by CompanyName 
                        Select D.Departmentid, D.DepartmentName 
                        From Department D Order By D.DepartmentName                         
                        Select Distinct EI.EmpId, EI.EmpName + case when isnull(EI.empcode, '') <> '' Then ' [' + EI.empcode + ']' Else '' End EmpName 
                        From EmpInfo EI 
                        JOIN Department D ON D.DepartmentId = EI.departmentId Order By EmpName 
                        ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDDepartmentName, ds, 1, true, "Select Department");
            UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 2, true, "Select Employee");

            TxtIssueDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            ViewState["ID"] = "0";
        }
    }
    protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    {
        string str = @"Select Empid 
                From EmpInfo EI 
                Where EmpCode = '" + txtWeaverIdNoscan.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (DDEmployeeName.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) != null)
            {
                DDEmployeeName.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
                DDEmployeeName_SelectedIndexChanged(sender, new EventArgs());
                txtWeaverIdNoscan.Text = "";
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('Employee Code does not exists')", true);
            txtWeaverIdNoscan.Focus();
        }
    }
    protected void DDDepartmentName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @" Select Distinct EI.EmpId, EI.EmpName + CASE WHEN EI.EMPCODE <> '' THEN ' [' + EI.EMPCODE + ']' ELSE '' END EMPNAME 
                From empinfo EI 
                JOIN Department D ON D.DepartmentId = EI.departmentId ";
                if (Chkedit .Checked ==true)
                {
                    str = str + " JOIN AnyItemIssueMasterDetail a ON a.DepartmentID = D.DepartmentId And a.EmpID = EI.EmpID ";
                }
                str = str + " Where Isnull(EI.Blacklist, 0) = 0 And EI.departmentId = " + DDDepartmentName.SelectedValue + " Order By EMPNAME";
        UtilityModule.ConditionalComboFill(ref DDEmployeeName, str, true, "Select Employee");
        ViewState["ID"] = "0";
    }
    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["ID"] = "0";
        TxtIssueNo.Text = "";
        FillIssueNo();
    }
    protected void DDIssuedNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["ID"] = DDIssuedNo.SelectedValue;
        FillGrid();
    }
    protected void Chkedit_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkedit.Checked == true)
        {
            TDIssuedNo.Visible = true;
        }
        else
        {
            TDIssuedNo.Visible = false;
        }
        FillIssueNo();
    }
    protected void TxtStockIDScan_TextChanged(object sender, EventArgs e)
    {
        ViewState["STOCKID"] = "0";
        string Str = "Select CompanyID, StockID, Round(QtyInHand, 3) QtyInHand From Stock(Nolock) Where CompanyID = " + DDcompany.SelectedValue + " And StockID = '" + TxtStockIDScan.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["QtyInHand"]) < 1)
            {
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('Stock Not available');", true);
                return;
            }
            ViewState["STOCKID"] = ds.Tables[0].Rows[0]["StockID"].ToString();
            TxtStockIDScan.Text = "";
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[26];

            param[0] = new SqlParameter("@ID", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = ViewState["ID"] == null ? 0 : ViewState["ID"];
            param[1] = new SqlParameter("@CompanyId", DDcompany.SelectedValue);
            param[2] = new SqlParameter("@DepartmentID", DDDepartmentName.SelectedValue);
            param[3] = new SqlParameter("@EmpID", DDEmployeeName.SelectedIndex > 0 ? DDEmployeeName.SelectedValue : "0");
            param[4] = new SqlParameter("IssueNo", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.InputOutput;
            param[4].Value = TxtIssueNo.Text;
            param[5] = new SqlParameter("@IssueDate", TxtIssueDate.Text);
            param[6] = new SqlParameter("@StockID", ViewState["STOCKID"]);
            param[7] = new SqlParameter("@Remarks", (TxtRemark.Text).Trim());
            param[8] = new SqlParameter("@UserID", Session["varuserid"]);
            param[9] = new SqlParameter("@MasterCompanyID", Session["varCompanyId"]);
            param[10] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[10].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_SaveAnyItemIssue]", param);

            ViewState["ID"] = param[0].Value.ToString();
            TxtIssueNo.Text = param[0].Value.ToString();
            Tran.Commit();

            if (param[10].Value.ToString() == "")
            {
                lblmessage.Text = "Data Saved successfully..";
                FillGrid();
            }
            else
            {
                lblmessage.Text = param[10].Value.ToString();
            }
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmessage.Text = ex.Message;
        }
    }
    protected void FillGrid()
    {
        string str = @"Select a.ID, a.DetailID, a.IssueNo, REPLACE(CONVERT(nvarchar(11), a.IssueDate, 106), ' ', '-') IssueDate, a.Remark, 
                    Replace(VF.Category_Name + ' / ' + VF.Item_Name + ' / ' + VF.QualityName + ' / ' + VF.DesignName + ' / ' + VF.ColorName + ' / ' + VF.ShapeName + ' / ' + VF.SizeFt + ' / ' + VF.ShadeColorName, ' /  / ', '') ItemDescription, 
                    GM.GodownName, S.LotNo, S.TagNo, 1 Qty  
                    From AnyItemIssueMasterDetail a(Nolock)
                    JOIN Stock S(Nolock) ON S.StockID = a.StockID 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.Item_Finished_ID = a.Item_Finished_ID 
                    JOIN GodownMaster GM ON GM.GodownID = S.GodownID 
                    Where a.ID = " + ViewState["ID"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
        if (Chkedit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                TxtIssueNo.Text = ds.Tables[0].Rows[0]["IssueNo"].ToString();
                TxtIssueDate.Text = ds.Tables[0].Rows[0]["IssueDate"].ToString();
                TxtRemark.Text = ds.Tables[0].Rows[0]["Remark"].ToString();
            }
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string str = @"select  * from V_AnyItemIssueMasterDetail Where id=" + ViewState["ID"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptAnyItemIssueMasterDetail.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptAnyItemIssueMasterDetail.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }
    }
    protected void FillIssueNo()
    {
        string str = @"Select Distinct ID, IssueNo + '/' + REPLACE(CONVERT(nvarchar(11), IssueDate, 106), ' ', '-') IssueNo 
        From AnyItemIssueMasterDetail (nolock)
        Where Companyid = " + DDcompany.SelectedValue + " And DepartmentID = " + DDDepartmentName.SelectedValue + " And EmpID = " + DDEmployeeName.SelectedValue + @" 
        Order By ID Desc";

        UtilityModule.ConditionalComboFill(ref DDIssuedNo, str, true, "--Plz Select--");
    }
    protected void DG_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            string masterid = ((Label)DG.Rows[e.RowIndex].FindControl("lblid")).Text;
            string Detailid = ((Label)DG.Rows[e.RowIndex].FindControl("lbldetailid")).Text;
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@Id", masterid);
            param[1] = new SqlParameter("@Detailid", Detailid);
            param[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            //************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteAnyItemIssue", param);
            //Pro_DeleteYarnOpeningIssue
            Tran.Commit();
            lblmessage.Text = param[2].Value.ToString();
            FillGrid();

        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
    }    
}