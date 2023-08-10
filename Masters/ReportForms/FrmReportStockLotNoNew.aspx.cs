using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using ClosedXML.Excel;
using AjaxControlToolkit.HTMLEditor.ToolbarButton;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Net;
using System.Web.Services.Description;




public partial class Masters_ReportForms_FrmReportStockLotNoNew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["VarcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
       
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {

        ////********For Diamond Export Only*********************////

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_GetStockNoTransactionDetailLotNoWiseDiamond", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@CompanyId", Session["CurrentWorkingCompanyID"]);
        cmd.Parameters.AddWithValue("@Lotno", txtLotno.Text);
        cmd.Parameters.AddWithValue("@TagNo", txtTagNo.Text);
        cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyId"]);        

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptStockNoTransactionDetailByLotNoTagNoDiamond.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptStockNoTransactionDetailByLotNoTagNoDiamond.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

            //Session["GetDataset"] = ds;
            //Session["rptFileName"] = "~\\Reports\\rptorderweavingconsumptiondetail.rpt";
            //Session["dsFileName"] = "~\\ReportSchema\\rptorderweavingconsumptiondetail.xsd";
            //StringBuilder stb = new StringBuilder();
            //stb.Append("<script>");
            //stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            //ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }       

    }   
    
}