using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ClosedXML.Excel;
using System.Text.RegularExpressions;
using System.Text;

public partial class Masters_ReportForms_FrmPendingBazaarPcsIssToFinishingProcess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName
                            Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where ProcessType=1 and Process_Name<>'WEAVING' and MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By PROCESS_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Select--");
            txtFromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        BazaarPcsIssFinishingProcessReport_CI();       
    }
    protected void BazaarPcsIssFinishingProcessReport_CI()
    {
        lblmsg.Text = "";
        try
        {
            string str = "", FilterBy = "";
            #region
            //if (ddItemName.SelectedIndex > 0)
            //{
            //    str = str + " and Vf.Item_id=" + ddItemName.SelectedValue;
            //    FilterBy = FilterBy + ", Item Name -" + ddItemName.SelectedItem.Text;
            //}
            //if (DDQuality.SelectedIndex > 0)
            //{
            //    str = str + " and Vf.Qualityid=" + DDQuality.SelectedValue;
            //    FilterBy = FilterBy + ", Quality -" + DDQuality.SelectedItem.Text;
            //}
            //if (DDDesign.SelectedIndex > 0)
            //{
            //    str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
            //    FilterBy = FilterBy + ", Design -" + DDDesign.SelectedItem.Text;
            //}
            //if (DDColor.SelectedIndex > 0)
            //{
            //    str = str + " and vf.Colorid=" + DDColor.SelectedValue;
            //    FilterBy = FilterBy + ", Color -" + DDColor.SelectedItem.Text;
            //}
            //if (DDSize.SelectedIndex > 0)
            //{
            //    str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
            //    FilterBy = FilterBy + ", Size -" + DDSize.SelectedItem.Text;
            //}
            //if (ChkForDate.Checked == true)
            //{
            //    str = str + " and PM.ReceiveDate>='" + TxtFromDate.Text + "' and PM.ReceiveDate<='" + TxtToDate.Text + "'";
            //    FilterBy = FilterBy + ", From -" + TxtFromDate.Text + " To - " + TxtToDate.Text;
            //}

            #endregion

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PendingBazaarPcsIssToFinishingProcessReport_CI", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 500;

            cmd.Parameters.AddWithValue("@Companyid", DDcompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Processid", DDProcessName.SelectedValue);            
            cmd.Parameters.AddWithValue("@FromDate", txtFromdate.Text);
            cmd.Parameters.AddWithValue("@ToDate", txttodate.Text);            
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyNo"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
            //cmd.Parameters.AddWithValue("@Where", str);           
            

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************
            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptPendingBazaarPcsIssToFinishingProcess_CI.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptPendingBazaarPcsIssToFinishingProcess_CI.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
    private static readonly Regex InvalidFileRegex = new Regex(string.Format("[{0}]", Regex.Escape(@"<>:""/\|?*")));
    public static string validateFilename(string filename)
    {
        return InvalidFileRegex.Replace(filename, string.Empty);
    }
    
    
}
