using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_ReportForms_frmUpdate_YarnWarpingMottelingRate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select PROCESS_NAME_ID,Process_name from PROCESS_NAME_MASTER WHere Process_name in('YARN OPENING','WEFT DEPARTMENT', 'WARPING COTTON','WARPING WOOL','YARN OPENING+MOTTELING') order by Process_Name";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            //UtilityModule.ConditionalComboFillWithDS(ref DDunits, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDjob, ds, 0, true, "--Plz Select Process--");
            txtFromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            RDOrderRate.Checked = true;
            switch (Session["varcompanyId"].ToString())
            {
                case "8":
                    break;
                default:
                    RDRecRate.Visible = true;
                    break;
            }
        }

    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (DDjob.SelectedIndex > 0)
        {

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("Pro_Update_Yarn_Warping_Motteling_Rate", con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 500;

                cmd.Parameters.AddWithValue("@ProcessId", DDjob.SelectedValue);
                cmd.Parameters.AddWithValue("@FromDate", txtFromdate.Text);
                cmd.Parameters.AddWithValue("@ToDate", txtToDate.Text);
                cmd.Parameters.AddWithValue("@Rateoption", RDOrderRate.Checked == true ? 0 : 1);
                cmd.Parameters.AddWithValue("@mastercompanyid", Session["varcompanyId"].ToString());
                cmd.Parameters.AddWithValue("@Userid", Session["varuserid"].ToString());
                cmd.ExecuteNonQuery();
                Tran.Commit();
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('Rates Updated Successfully!');", true); 

            }
            catch (Exception ex)
            {
                Tran.Rollback();
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('" + ex.Message + "');", true);
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please Select Job Name.');", true);
        }
    }
}