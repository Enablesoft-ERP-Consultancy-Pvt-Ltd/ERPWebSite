using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_frmBranchMaster : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            TxtId.Text = "0";
            Fill_Combo();

            Fill_Grid();

        }
        lbl.Visible = false;
    }
    private void Fill_Grid()
    {
        dgBranch.DataSource = Fill_Grid_Data();
        dgBranch.DataBind();
    }
    private void Fill_Combo()
    {
        DataSet ds = null;
        string strsql = "select CompanyName,CompanyId from CompanyInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        cmbCompany.DataSource = ds;
        cmbCompany.DataTextField = "CompanyName";
        cmbCompany.DataValueField = "CompanyId";
        cmbCompany.DataBind();

        if (cmbCompany.Items.FindByValue(Session["CurrentWorkingCompanyID"].ToString()) != null)
        {
            cmbCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
            cmbCompany.Enabled = false;
        }
    }

    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        try
        {
            string sql = @"select BranchId as SrNo,BranchName,Address, PhoneNo,FaxNo,ContactPerson,
                        (select CompanyName from CompanyInfo where Branch.CompanyId=CompanyInfo.CompanyId)as CompanyName
                     from Branch where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                    And CompanyID = " + cmbCompany.SelectedValue + " order by BranchId ";
            ds = SqlHelper.ExecuteDataset(sql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmBranchMaster.aspx");
        }
        return ds;
    }
    private void Clear()
    {
        txtAddress.Text = "";
        txtBranchName.Text = "";
        txtCPerson.Text = "";
        txtFaxNo.Text = "";
        txtPhoneNo.Text = "";
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }
    protected void dgBranch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgBranch, "Select$" + e.Row.RowIndex);
        }
    }
    protected void dgBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = dgBranch.SelectedDataKey.Value.ToString();
        DataSet ds = SqlHelper.ExecuteDataset(@"SELECT * from Branch where BRANCHid=" + id);
        try
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                TxtId.Text = ds.Tables[0].Rows[0]["BRANCHid"].ToString();
                ViewState["id"] = TxtId.Text;
                txtBranchName.Text = ds.Tables[0].Rows[0]["BRANCHName"].ToString();
                cmbCompany.SelectedValue = ds.Tables[0].Rows[0]["companyid"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtPhoneNo.Text = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
                txtFaxNo.Text = ds.Tables[0].Rows[0]["FaxNo"].ToString();
                txtCPerson.Text = ds.Tables[0].Rows[0]["ContactPerson"].ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmBranchMaster.aspx");
            //Response.Write(ex.Message);
        }
        btnSave.Text = "Update";
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Validated();
        if (txtBranchName.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[9];
                _arrPara[0] = new SqlParameter("@BranchId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@BranchName", SqlDbType.NVarChar, 50);
                _arrPara[2] = new SqlParameter("@CompanyId", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@Address", SqlDbType.NVarChar, 150);
                _arrPara[4] = new SqlParameter("@PhoneNo", SqlDbType.NVarChar, 50);
                _arrPara[5] = new SqlParameter("@FaxNo", SqlDbType.NVarChar, 50);
                _arrPara[6] = new SqlParameter("@ContactPerson", SqlDbType.NVarChar, 50);
                _arrPara[7] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[8] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
                _arrPara[0].Value = TxtId.Text;
                _arrPara[1].Value = txtBranchName.Text.ToUpper();
                _arrPara[2].Value = cmbCompany.SelectedValue;
                _arrPara[3].Value = txtAddress.Text.ToUpper();
                _arrPara[4].Value = txtPhoneNo.Text;
                _arrPara[5].Value = txtFaxNo.Text;
                _arrPara[6].Value = txtCPerson.Text.ToUpper();
                _arrPara[7].Value = Session["varuserid"].ToString();
                _arrPara[8].Value = Session["varMasterCompanyIDForERP"].ToString();
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_BRANCH", _arrPara);
                Tran.Commit();
                lbl.Visible = true;
                lbl.Text = "Save Details............";
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lbl.Visible = true;
                lbl.Text = ex.Message;
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmBranchMaster.aspx");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                if (con != null)
                {
                    con.Dispose();
                }
            }

        }
        else
        {
            if (lbl.Text == "BranchName Name already exists............")
            {
                lbl.Visible = true;
                lbl.Text = "BranchName Name already exists............";
            }
            else
            {
                lbl.Visible = true;
                lbl.Text = "Please Fill Details........";
            }
        }
        Clear();
        Fill_Grid();
        btnSave.Text = "Save";
    }
    private void Validated()
    {
        try
        {
            string strsql;
            if (btnSave.Text == "Update")
            {
                strsql = "select BranchName from Branch where BranchId!='" + ViewState["id"].ToString() + "' and  BranchName='" + txtBranchName.Text + "' And  MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            else
            {
                strsql = "select BranchName from Branch where BranchName='" + txtBranchName.Text + "' And  MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            DataSet ds = SqlHelper.ExecuteDataset(strsql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lbl.Visible = true;
                lbl.Text = "BranchName Name already exists............";
                txtBranchName.Text = "";
                txtBranchName.Focus();
            }
            else
            {
                lbl.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/frmBranchMaster.aspx");
        }
    }
    protected void dgBranch_RowCreated(object sender, GridViewRowEventArgs e)
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
        // e.Row.RowState == DataControlRowState.Alternate)
        // e.Row.CssClass = "alternate";
    }
}