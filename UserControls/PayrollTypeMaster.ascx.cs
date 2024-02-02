using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class UserControls_PayrollTypeMaster : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            Fill_Grid();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Save_Update();
    }
    protected void Save_Update()
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@payrolltypeid", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hngroupid.Value;
            param[1] = new SqlParameter("@payrolltypename", txtpayrolltype.Text.Trim());
            param[2] = new SqlParameter("@Userid", Session["varuserid"]);
            param[3] = new SqlParameter("@MastercompanyId", Session["varMasterCompanyIDForERP"]);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            //********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveHRPayrollTypeMaster", param);
            lblmsg.Text = param[4].Value.ToString();
            Tran.Commit();
            Fill_Grid();
            txtpayrolltype.Text = "";
            hngroupid.Value = "0";
            btnsave.Text = "Save";
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void lnkupdate(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int rowindex = row.RowIndex;


        Label lblpayrolltype = (Label)GDDetail.Rows[rowindex].FindControl("lblpayrolltype");
        Label lblpayrolltypeid = (Label)GDDetail.Rows[rowindex].FindControl("lblpayrolltypeid");
        hngroupid.Value = lblpayrolltypeid.Text;
        //**
        txtpayrolltype.Text = lblpayrolltype.Text;
        btnsave.Text = "Update";
    }
    protected void Fill_Grid()
    {
        string str = @"select payrolltypename,payrolltypeid From HR_Payrolltypemaster order by payrolltypename";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);


        GDDetail.DataSource = ds.Tables[0];
        GDDetail.DataBind();
    }

}