using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;

public partial class Masters_Campany_FrmCustomerUserWise : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref ddemp, @"select userid,username from Newuserdetail where companyid="+Session["varcompanyid"]+"", true, "-Select Employee-");
            fillgrid();
        }
    }
    private void fillgrid()
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select CustomerId,CustomerCode,CustomerName from customerinfo order by customercode");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGcustomer.DataSource = ds;
            DGcustomer.DataBind();
        }
    }
    protected void ddemp_SelectedIndexChanged(object sender, EventArgs e)
    {
        Refresh();
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Customerid from CustomerUserWise where userid=" + ddemp.SelectedValue + " and companyid="+Session["varcompanyid"]+" and mastercompanyid="+Session["varcompanyno"]+"");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                for (int i = 0; i < DGcustomer.Rows.Count; i++)
                {
                    if (DGcustomer.DataKeys[i].Value.ToString() == ds.Tables[0].Rows[j][0].ToString())
                    {
                        ((CheckBox)DGcustomer.Rows[i].FindControl("Chkbox")).Checked = true;
                    }

                }
            }
        }
    }
    protected void DGcustomer_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void BtnSave_Click(object sender, EventArgs e)
    {        
        string Str = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara1 = new SqlParameter[5];
            _arrpara1[0] = new SqlParameter("@Userid", SqlDbType.Int);
            _arrpara1[1] = new SqlParameter("@Customerid", SqlDbType.NVarChar, 1000);
            _arrpara1[2] = new SqlParameter("@MasterCompanyid", SqlDbType.Int);
            _arrpara1[3] = new SqlParameter("@Companyid", SqlDbType.Int);

            _arrpara1[0].Value = ddemp.SelectedValue;
            _arrpara1[2].Value = Session["varcompanyno"];
            _arrpara1[3].Value = Session["varcompanyid"];
            string str = "";
            for (int i = 0; i < DGcustomer.Rows.Count; i++)
            {
                if (((CheckBox)DGcustomer.Rows[i].FindControl("Chkbox")).Checked == true)
                {
                    if (str == "")
                    {
                        str = DGcustomer.DataKeys[i].Value.ToString();
                    }
                    else
                    {
                        str = str + "," + DGcustomer.DataKeys[i].Value.ToString();
                    }
                }
            }
            _arrpara1[1].Value = str;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_CustomerUserWise", _arrpara1);
            Tran.Commit();
            Refresh();
            ddemp.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void Refresh()
    {
        for (int i = 0; i < DGcustomer.Rows.Count; i++)
        {
            if (((CheckBox)DGcustomer.Rows[i].FindControl("Chkbox")).Checked == true)
            {
                ((CheckBox)DGcustomer.Rows[i].FindControl("Chkbox")).Checked = false;
            }
        }
        ChkForAllCustomer.Checked = false;
        
    }
}