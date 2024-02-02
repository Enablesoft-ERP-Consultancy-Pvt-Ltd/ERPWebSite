using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Packing_FrmInvoiceCheckList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");

        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDCompany, "select CI.CompanyId,CI.CompanyName From Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "", true, "--Select---");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
            fill_grid();
        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDCustomerName, "SELECT customerid,CompanyName + SPACE(5)+Customercode from customerinfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "---Select---");
    }
    protected void gvforinvice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
        //    e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        //    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvforinvice, "Select$" + e.Row.RowIndex);
        //}
    }
    protected void gvforinvice_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void fill_grid()
    {
        string str = "select ID,Name from invoicechecklist";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        gvforinvice.DataSource = ds;
        gvforinvice.DataBind();
    }
    protected void Btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[6];
            _arrpara[0] = new SqlParameter("@Id", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@Checklistid", SqlDbType.VarChar, 100);
            _arrpara[2] = new SqlParameter("@Noofcopies", SqlDbType.VarChar, 100);
            _arrpara[3] = new SqlParameter("@Customerid", SqlDbType.Int);
            _arrpara[4] = new SqlParameter("@Companyid", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@Userid", SqlDbType.Int);

            string str = null;
            string Qty = null;
            _arrpara[0].Value = 0;

            _arrpara[3].Value = DDCustomerName.SelectedValue;
            _arrpara[4].Value = DDCompany.SelectedValue;
            _arrpara[5].Value = Session["varuserid"].ToString();
            for (int i = 0; i < gvforinvice.Rows.Count; i++)
            {
                if (((CheckBox)gvforinvice.Rows[i].FindControl("Chkbox")).Checked == true)
                {
                    _arrpara[1].Value = ((Label)gvforinvice.Rows[i].FindControl("lblid")).Text;
                    _arrpara[2].Value = ((TextBox)gvforinvice.Rows[i].FindControl("txtnoofcopies")).Text;
                    str = str == null ? _arrpara[1].Value.ToString() : str + "," + _arrpara[1].Value;
                    Qty = Qty == null ? _arrpara[2].Value.ToString() : Qty + "," + _arrpara[2].Value;

                }
            }
            _arrpara[1].Value = str;
            _arrpara[2].Value = Qty;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_CheckListInvoice", _arrpara);
            Tran.Commit();
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Save", "alert('Data Saved Successfully....');", true);
            fill_grid();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void Fillcustomerchecklistdetail()
    {
        string str = "select CheckListId,Noofcopies from allinformationchecklist where Customerid=" + DDCustomerName.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < gvforinvice.Rows.Count; i++)
            {
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    CheckBox chk = ((CheckBox)gvforinvice.Rows[i].FindControl("chkbox"));
                    Label checkid = ((Label)gvforinvice.Rows[i].FindControl("lblid"));
                    TextBox TxtNoofcopies = ((TextBox)gvforinvice.Rows[i].FindControl("txtnoofcopies"));
                    if (checkid.Text == ds.Tables[0].Rows[j]["CheckListId"].ToString())
                    {
                        chk.Checked = true;
                        TxtNoofcopies.Text = ds.Tables[0].Rows[j]["Noofcopies"].ToString();
                    }
                }
            }
        }
    }
    protected void DDCustomerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillcustomerchecklistdetail();
    }
}