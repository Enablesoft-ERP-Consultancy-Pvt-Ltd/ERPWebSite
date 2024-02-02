using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Purchase_PurchaseUnApproval : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["IndentNo"] = null;
            ViewState["PIApprovalId"] = 0;

            CommanFunction.FillCombo(DDCompanyName, "select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.USERID=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Companyname");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFill(ref DDDepartment, "Select Distinct D.DepartmentId,DepartmentName from Department D,PurchaseIndentMaster PM Where PM.Departmentid=D.Departmentid And  D.masterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And FlagApproval=1 order by DepartmentName", true, "--Select Department--");
            //            TxtApprovalDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDvendorName, "Select Distinct E.Empid,EmpName   from PurchaseIndentMaster PM,Empinfo E where PM.Partyid=E.EmpId And  FlagApproval=1 and PM.DepartmentId=" + DDDepartment.SelectedValue + " And PM.masterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order BY EmpName", true, "--Select Vendor--");

    }

    private void fill_grid()
    {
        DGPIndentDetail.DataSource = Fill_Grid_Data();
        DGPIndentDetail.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql;
            strsql = @"select Distinct PIM.PIndentId,PIndentNo+Space(2)+'/'+Replace(Convert(nvarchar(11),Date,106),' ','-') As PIndentNo
                            From PurchaseIndentMaster PIM     Where  PIM.masterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And PIM.EmpId=" + DDpartyName.SelectedValue + " And PIM.PartyId=" + DDvendorName.SelectedValue + " And DepartmentId=" + DDDepartment.SelectedValue + " And FlagApproval=1";
            if (TxtPindentNo.Text != "")
            {
                strsql = strsql + " And PindentNo='" + TxtPindentNo.Text + "' Order by PindentNo";
            }
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
        fill_grid();
    }

    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/main.aspx");
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

    protected void DDvendorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDpartyName, "Select Distinct E.Empid,EmpName    from PurchaseIndentMaster PM,Empinfo E where PM.EmpId=E.EmpId And  FlagApproval=1 and PM.PartyId=" + DDvendorName.SelectedValue + " And PM.masterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And PM.DepartMentId=" + DDDepartment.SelectedValue + " Order BY EmpName", true, "--Select Vendor--");
    }
    protected void DDpartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LblErr.Text = "";
        fill_grid();
    }
    protected void DGPIndentDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int PIndentId;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            LblErr.Text = "";
            PIndentId = Convert.ToInt16(DGPIndentDetail.DataKeys[e.RowIndex].Value);
            SqlParameter[] _array = new SqlParameter[2];
            _array[0] = new SqlParameter("@PindentId", SqlDbType.Int);
            _array[1] = new SqlParameter("@Message", SqlDbType.NVarChar, 250);

            _array[0].Value = PIndentId;
            _array[1].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_PIndentUnApprove", _array);
            LblErr.Text = _array[1].Value.ToString();
            Tran.Commit();
            fill_grid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            LblErr.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void TxtPindentNo_TextChanged(object sender, EventArgs e)
    {
        string str;
        LblErr.Text = "";
        DGPIndentDetail.DataSource = null;
        DGPIndentDetail.DataBind();
        str = "select Companyid,Departmentid,PartyId,EmpId From PurchaseindentMaster Where CompanyID = " + DDCompanyName.SelectedValue + " And Pindentno='" + TxtPindentNo.Text + "' And FlagApproval=1";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DDDepartment.SelectedValue = ds.Tables[0].Rows[0]["Departmentid"].ToString();
            DDDepartment_SelectedIndexChanged(sender, e);
            DDvendorName.SelectedValue = ds.Tables[0].Rows[0]["PartyId"].ToString();
            DDvendorName_SelectedIndexChanged(sender, e);
            DDpartyName.SelectedValue = ds.Tables[0].Rows[0]["Empid"].ToString();
            fill_grid();
        }
        else
        {
            LblErr.Text = "Please Enter Valid IndentNo....";
        }
    }
}