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

public partial class Masters_Loom_FrmMasterPCIssRecOnFolio : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CI.CompanyId,CompanyName 
                        From CompanyInfo CI(Nolock) 
                        inner Join Company_Authentication CA(Nolock) on CA.CompanyId = CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + @" order by CompanyName 
                        Select Unitsid,Unitname from Units(Nolock) order by unitname
                        Select EI.EmpId,EI.Empcode+' ['+EI.Empname+']' Empname 
                        From EmpInfo EI(Nolock) 
                        inner join Department D(Nolock) on EI.Departmentid = D.DepartmentId And D.DepartmentName = 'PRODUCTION' And EI.Status = 'P' 
                        And EI.Blacklist = 0 order by Empname";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDunitname, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 2, true, "--Plz Select--");
            TxtIssRecDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDIssRecType_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtIssRecNo.Text = "";

        string Str = @"Select EI.EmpId,EI.Empcode+' ['+EI.Empname+']' Empname 
                        From EmpInfo EI(Nolock) 
                        inner join Department D(Nolock) on EI.Departmentid = D.DepartmentId And D.DepartmentName = 'PRODUCTION' And EI.Status = 'P' 
                        And EI.Blacklist = 0 order by Empname";

        if (DDIssRecType.SelectedValue == "1")
        {
            Str = @"Select Distinct EI.EmpId, EI.Empcode + ' [' + EI.Empname + ']' Empname 
            From ProcessIssueAttachMasterPC a(Nolock) 
            JOIN EmpInfo EI(Nolock) ON EI.EmpID = a.EmpID 
            Where a.IssRecFlag = 0 And a.CompanyID = " + DDcompany.SelectedValue + " And a.Units = " + DDunitname.SelectedValue + @" Order By Empname ";
        }

        UtilityModule.ConditionalComboFill(ref DDEmployeeName, Str, true, "--Plz Select--");
    }
    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_EmployeeName();
    }
    protected void Fill_EmployeeName()
    {
        TxtIssRecNo.Text = "";
        string Str = @"Select Distinct a.IssueOrderId, a.CHALLANNO 
            From PROCESS_ISSUE_MASTER_1 a(Nolock) 
            JOIN Employee_ProcessOrderNo EPO(Nolock) ON EPO.IssueOrderId = a.IssueOrderId And EPO.ProcessId = 1 And EPO.EmpID = " + DDEmployeeName.SelectedValue + @" 
            Where a.Companyid = " + DDcompany.SelectedValue + " And Units = " + DDunitname.SelectedValue + " Order By a.IssueOrderId Desc";

        if (DDIssRecType.SelectedValue == "1")
        {
            Str = @"Select Distinct a.IssueOrderID, PIM.CHALLANNO 
            From ProcessIssueAttachMasterPC a(Nolock)
            JOIN PROCESS_ISSUE_MASTER_1 PIM(Nolock) ON PIM.IssueOrderId = a.IssueOrderID 
            Where a.IssRecFlag = 0 And a.CompanyID = " + DDcompany.SelectedValue + " And a.Units = " + DDunitname.SelectedValue + @" And 
                a.EmpID = " + DDEmployeeName.SelectedValue + @" Order By a.IssueOrderID Desc ";
        }

        UtilityModule.ConditionalComboFill(ref DDFolioNo, Str, true, "--Plz Select--");
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[12];
            param[0] = new SqlParameter("@CompanyID", DDcompany.SelectedValue);
            param[1] = new SqlParameter("@Units", DDunitname.SelectedValue);
            param[2] = new SqlParameter("@EmpID", DDEmployeeName.SelectedValue);
            param[3] = new SqlParameter("@IssueOrderID", DDFolioNo.SelectedValue);
            param[4] = new SqlParameter("@ProcessID", 1);
            param[5] = new SqlParameter("@IssRecFlag", DDIssRecType.SelectedValue);
            param[6] = new SqlParameter("@TstockNo", txtstockno.Text);
            param[7] = new SqlParameter("@MasterCompanyID", Session["varcompanyid"]);
            param[8] = new SqlParameter("@UserID", Session["varuserid"]);
            param[9] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[9].Direction = ParameterDirection.Output;
            param[10] = new SqlParameter("@IssRecNo", TxtIssRecNo.Text == "" ? "0" : TxtIssRecNo.Text);
            param[10].Direction = ParameterDirection.InputOutput;
            param[11] = new SqlParameter("@IssRecDate", TxtIssRecDate.Text);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveMasterPCAttachToFolio", param);
            Tran.Commit();
            lblmsg.Text = param[9].Value.ToString();
            TxtIssRecNo.Text = param[10].Value.ToString();
            FillGrid();
            Refreshcontrol();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void Refreshcontrol()
    {
        txtstockno.Text = "";
        txtstockno.Focus();
    }
    protected void FillGrid()
    {
        string str = @"Select a.IssRecNo, VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + VF.SizeFt + ' ' + VF.ShadeColorName [Description], 
                a.TStockNo, a.IssueOrderID, a.StockNo 
                From ProcessIssueAttachMasterPC a(Nolock) 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = a.ITEM_FINISHED_ID 
                Where a.IssRecFlag = " + DDIssRecType.SelectedValue + " And IssueOrderID = " + DDFolioNo.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
    }

    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        }
    }

    protected void lbDelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            LinkButton lb = (LinkButton)sender;
            GridViewRow grv = (GridViewRow)lb.NamingContainer;
            string IssueOrderID = ((Label)DG.Rows[grv.RowIndex].FindControl("lblIssueOrderID")).Text;
            string StockNo = ((Label)DG.Rows[grv.RowIndex].FindControl("lblStockNo")).Text;

            SqlParameter[] param = new SqlParameter[7];

            param[0] = new SqlParameter("@StockNo", StockNo);
            param[1] = new SqlParameter("@IssueOrderID", IssueOrderID);
            param[2] = new SqlParameter("@ProcessID", 1);
            param[3] = new SqlParameter("@IssRecFlag", DDIssRecType.SelectedValue);
            param[4] = new SqlParameter("@MasterCompanyID", Session["varcompanyid"]);
            param[5] = new SqlParameter("@UserID", Session["varuserid"]);

            param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[6].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteMasterPCAttachToFolio", param);
            Tran.Commit();
            lblmsg.Text = param[6].Value.ToString();
            FillGrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void txtstockno_TextChanged(object sender, EventArgs e)
    {
        BtnSave.Focus();
    }
    protected void DDunitname_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtIssRecNo.Text = "";
    }
    protected void DDFolioNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtIssRecNo.Text = "";
        FillGrid();
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        string Str = @"Select CI.CompanyName, CI.CompAddr1 + CI.CompAddr2 + CI.CompAddr3 CompanyAddress, CI.CompTel PhoneNumber, 
                U.UnitName, EI.EmpName, EI.EmpCode, a.IssueOrderID, PNM.PROCESS_NAME ProcessName, a.IssRecNo, a.IssRecDate, 
                Case When a.IssRecFlag = 0 Then 'Issue' Else 'Receive' End IssRecFlag, 
                VF.ITEM_NAME ItemName, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName, VF.SizeFt, VF.ShadeColorName, 
                StockNo, TStockNo 
                From ProcessIssueAttachMasterPC a(Nolock) 
                JOIN CompanyInfo CI(Nolock) ON CI.CompanyId = a.CompanyID 
                JOIN Units U(Nolock) ON U.UnitsId = a.Units 
                JOIN Empinfo EI(Nolock) ON EI.EmpID = a.EmpID 
                JOIN Process_Name_Master PNM(Nolock) ON PNM.PROCESS_NAME_ID = a.ProcessID 
                JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = a.Item_Finished_ID 
                Where a.TypeFlag = 1 And a.IssRecFlag = " + DDIssRecType.SelectedValue + " And a.IssueOrderID = " + DDFolioNo.SelectedValue;

        if (TxtIssRecNo.Text != "")
        {
            Str = Str + @" And IssRecNo = " + TxtIssRecNo.Text;
        }

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptMasterPCIssRecOnFolio.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptMasterPCIssRecOnFolio.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
}