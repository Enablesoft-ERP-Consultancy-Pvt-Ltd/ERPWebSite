using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

public partial class Masters_ReportForms_frmorderFolioforAnisa : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
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

            SqlParameter[] _array = new SqlParameter[7];
            _array[0] = new SqlParameter("@Companyid", SqlDbType.Int);
            _array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
            _array[2] = new SqlParameter("@EmpId", SqlDbType.Int);
            _array[3] = new SqlParameter("@IssueOrderid", SqlDbType.Int);

            _array[0].Value = Session["CurrentWorkingCompanyID"];// DDCompany.SelectedValue;
            _array[1].Value = 1;// DDProcessName.SelectedValue;
            _array[2].Value = 0;// DDEmpName.SelectedValue;
            _array[3].Value = txtFolioNo.Text;//DDChallanNo.SelectedValue;

            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_ForOrderFolio", _array);

            if (ds.Tables[0].Rows.Count > 0)
            {
                // Table 3 For Show image in crystal Report
                ds.Tables[3].Columns.Add("Image", typeof(System.Byte[]));
                foreach (DataRow dr in ds.Tables[3].Rows)
                {

                    if (Convert.ToString(dr["Photo"]) != "")
                    {
                        FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                        if (TheFile.Exists)
                        {
                            string img = dr["Photo"].ToString();
                            img = Server.MapPath(img);
                            Byte[] img_Byte = File.ReadAllBytes(img);
                            dr["Image"] = img_Byte;
                        }
                    }
                }
                Session["dsFileName"] = "~\\ReportSchema\\RptProcessOrderFolio.xsd";
                Session["rptFileName"] = "Reports/RptProcessOrderFolio.rpt";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void txtFolioNo_TextChanged(object sender, EventArgs e)
    {
        btnprint_Click(sender, e);
    }
}