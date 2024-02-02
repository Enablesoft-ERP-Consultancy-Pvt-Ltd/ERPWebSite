using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_AddDepartment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtid.Text = "0";
            UtilityModule.NewChkBoxListFill(ref ChkForBranch, @"Select BM.ID, BM.BranchName 
                            From BRANCHMASTER BM(nolock) 
                            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " Order By BM.BranchName ");
            fill_grid();
            txtDepartment.Focus();
        }
        lblMessage.Visible = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtDepartment.Text != "")
        {
            CheckDuplicateData();
            if (lblMessage.Visible == false)
            {
                Store_Data();
            }
        }
        else
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Fill Details........";
        }
    }
    private void CheckDuplicateData()
    {
        DataSet ds = null;
        string strsql = @"Select DepartmentName from Department Where DepartmentName='" + txtDepartment.Text + "' and DepartmentId !=" + txtid.Text + "  And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Department AlReady Exits........";
            txtDepartment.Text = "";
            txtDepartment.Focus();
        }
        else
        {
            lblMessage.Visible = false;
        }
    }
    private void Store_Data()
    {
        string str5 = null;  //// For Branch 
        try
        {
            for (int j = 0; j < ChkForBranch.Items.Count; j++)
            {
                str5 = ChkForBranch.Items[j].Value;

                if (ChkForBranch.Items[j].Selected)
                {
                    str5 = str5 + ",1";
                }
                else
                {
                    str5 = str5 + ",0";
                }
                str5 = str5 + "|";
            }

            CheckDuplicateData();
            if (lblMessage.Visible == false)
            {
                SqlParameter[] _arrPara = new SqlParameter[6];
                _arrPara[0] = new SqlParameter("@DeartmentId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@DepartmentName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@ShowOrNotInHR", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@BranchIDS", SqlDbType.NVarChar, 500);

                _arrPara[0].Value = Convert.ToInt32(txtid.Text);
                _arrPara[1].Value = txtDepartment.Text.ToUpper();
                _arrPara[2].Value = Session["varuserid"].ToString();
                _arrPara[3].Value = Session["varMasterCompanyIDForERP"].ToString();
                _arrPara[4].Value = ChkShowOrNotInHR.Checked == true ? 1 : 0;
                _arrPara[5].Value = str5;

                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_Department", _arrPara);
                ClearAll();
                lblMessage.Visible = true;
                lblMessage.Text = "Save Details.............";
            }
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_Carpet_AddDepartment|cmdSave_Click|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddDepartment.aspx");
        }
        fill_grid();
    }
    private void fill_grid()
    {
        DgDepartment.DataSource = Fill_Grid_Data();
        DgDepartment.DataBind();
        Session["ReportPath"] = "";
        Session["CommanFormula"] = "";
    }

    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        try
        {
            string strsql = @"SELECT DepartmentId,DepartmentName FROM Department Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By DepartmentName";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error:- " + ex.ToString();
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddDepartment.aspx");
        }
        return ds;

    }
    private void ClearAll()
    {
        txtid.Text = "0";
        txtDepartment.Text = "";
        btnSave.Text = "Save";
        ChkShowOrNotInHR.Checked = false;

        for (int c = 0; c < ChkForBranch.Items.Count; c++)
        {
            ChkForBranch.Items[c].Selected = false;
        }
    }
    protected void DgDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChkShowOrNotInHR.Checked = false;
        string id = DgDepartment.SelectedDataKey.Value.ToString();
        ViewState["id"] = id;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select DepartmentId,DepartmentName, Isnull(ShowOrNotInHR, 0) ShowOrNotInHR 
            From Department 
            WHERE DepartmentId=" + id + @"
            Select DepartmentId, BranchID From DepartmentBranch Where DepartmentId=" + id);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtid.Text = ds.Tables[0].Rows[0]["DepartmentId"].ToString();
                txtDepartment.Text = ds.Tables[0].Rows[0]["DepartmentName"].ToString();
                if (ds.Tables[0].Rows[0]["ShowOrNotInHR"].ToString() == "1")
                {
                    ChkShowOrNotInHR.Checked = true;
                }
            }
            
            for (int c = 0; c < ChkForBranch.Items.Count; c++)
            {
                ChkForBranch.Items[c].Selected = false;
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                {
                    for (int i = 0; i < ChkForBranch.Items.Count; i++)
                    {
                        if (ChkForBranch.Items[i].Value == ds.Tables[1].Rows[j]["BranchID"].ToString())
                        {
                            ChkForBranch.Items[i].Selected = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddDepartment.aspx");
        }
        btnSave.Text = "Update";
    }
    protected void DgDepartment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DgDepartment, "Select$" + e.Row.RowIndex);
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        txtid.Text = "0";
        txtDepartment.Text = "";
        btnSave.Text = "Save";
    }
    protected void DgDepartment_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        //if (e.Row.RowType == DataControlRowType.Header)
        // e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        // if (e.Row.RowType == DataControlRowType.DataRow &&
        //  e.Row.RowState == DataControlRowState.Alternate)
        // e.Row.CssClass = "alternate";
    }
}
