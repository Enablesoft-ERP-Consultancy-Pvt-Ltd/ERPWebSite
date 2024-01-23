using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Order_frmOrderDelete : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref ddcompany, @"Select CI.CompanyId,CompanyName 
                            From CompanyInfo CI 
                            JOIN Company_Authentication CA on CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + @" And 
                            CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by CompanyName", false, "");
            if (ddcompany.Items.Count > 0)
            {
                ddcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddcompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFill(ref ddcustomercode, "select customerid,customercode from customerinfo where mastercompanyid=" + Session["varcompanyNO"] + " order by customercode", true, "--Plz select customer--");
        }
    }
    protected void btndel_Click(object sender, EventArgs e)
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
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@customerid", ddcustomercode.SelectedValue);
            param[1] = new SqlParameter("@OrderNo", txtorderNo.Text.Trim());
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;

            //Delete Order
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[pro_Delete_order]", param);
            Tran.Commit();
            txtorderNo.Text = "";
            ScriptManager.RegisterStartupScript(Page, GetType(), "alert", "alert('" + param[2].Value.ToString() + "');", true);
            //

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }

}