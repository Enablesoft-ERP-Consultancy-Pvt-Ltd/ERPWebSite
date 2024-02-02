using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


public partial class Masters_Process_FrmUpdateFolioConsumption : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
        }
    }

    protected void btnupdateconsmp_Click(object sender, EventArgs e)
    {
        UpdateConsumption(1);
    }
    protected void BtnUpdateFolioReceiveConsumption_Click(object sender, EventArgs e)
    {
        UpdateConsumption(2);
    }
    private void UpdateConsumption(int TypeFlag)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@ChallanNo", TxtFolioNo.Text);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@ChkForDyeingConsumption", ChkForDyeingConsumption.Checked == true ? "1" : "0");
            param[3] = new SqlParameter("@IssueDate", System.DateTime.Now.ToString("dd-MMM-yyyy"));
            param[4] = new SqlParameter("@MasterCompanyId", Session["varcompanyNo"]);
            param[5] = new SqlParameter("@TypeFlag", TypeFlag);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_updatecurrentconsmpLoomWise", param);

            Tran.Commit();
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('" + param[1].Value + "');", true);
            TxtFolioNo.Text = "";
            TxtFolioNo.Focus();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('" + ex.Message + "');", true);
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
}