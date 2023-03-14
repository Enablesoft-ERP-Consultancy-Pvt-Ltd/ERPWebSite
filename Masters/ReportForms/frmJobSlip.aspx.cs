using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;


public partial class Masters_ReportForms_frmJobSlip : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            txtdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
           // SqlhelperEnum.FillDropDown(AllEnums.MasterTables.PROCESS_NAME_MASTER, DDjob, pWhere: "MasterCompanyid=" + Session["varcompanyId"] + "", pID: "Process_Name_Id", pName: "Process_Name", pFillBlank: true, Selecttext: "--Plz Select Job--");
            UtilityModule.ConditionalComboFill(ref DDjob, "select PROCESS_NAME_ID,Process_name from PROCESS_NAME_MASTER order by Process_Name", true, "--Plz Select Process--");
        }
    }
    protected void btnprint_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] array = new SqlParameter[5];
            array[0] = new SqlParameter("@processid", SqlDbType.Int);
            array[1] = new SqlParameter("@strEmpid", SqlDbType.VarChar, 50);
            array[2] = new SqlParameter("@ProcessName", SqlDbType.VarChar, 50);
            array[3] = new SqlParameter("@Date", SqlDbType.SmallDateTime);

            array[0].Value = DDjob.SelectedValue;
            array[1].Value = txtIdNo.Text;
            array[2].Value = DDjob.SelectedItem.Text;
            array[3].Value = txtdate.Text;

            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_RptJobSlip", array);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptJobSlip.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptJobSlip.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Error", "alert('" + ex.Message + "');", true);
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }

    }
}