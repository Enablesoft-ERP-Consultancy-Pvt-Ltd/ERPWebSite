using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using ClosedXML.Excel;
public partial class Masters_ReportForms_frmhold_rejecteddetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CI.CompanyId,CI.CompanyName From companyinfo  CI inner join Company_Authentication CA on CI.CompanyId=CA.CompanyId ANd CA.UserId=" + Session["varuserid"] + @" order by CompanyId
                           select PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME From process_Name_master PNM inner join UserRightsProcess UR on PNM.PROCESS_NAME_ID=UR.ProcessId and UR.Userid=" + Session["varuserid"] + @"
                           and PNM.processtype=1 order by PNM.PROCESS_NAME";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDjobname, ds, 1, true, "--PLz Select--");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }

    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        string filterby = "", Path;
        try
        {
            if (DDjobname.SelectedIndex > 0)
            {
                filterby = filterby + "," + "JOB : " + DDjobname.SelectedItem.Text;
            }
            filterby = filterby + ",From : " + txtfromdate.Text + " To : " + txttodate.Text;

            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@Companyid", DDcompany.SelectedValue);
            param[1] = new SqlParameter("@Processid", DDjobname.SelectedValue);
            param[2] = new SqlParameter("@FromDate", txtfromdate.Text);
            param[3] = new SqlParameter("@ToDate", txttodate.Text);
            //***************
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETHOLD_REJECTEDPCDDETAIL", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("Hold_Reject");

                sht.Range("A1:F1").Merge();
                sht.Range("A1").SetValue("Hold/Rejected Pcs Detail");
                sht.Range("A1:F1").Style.Font.Bold = true;
                sht.Range("A1:F1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Row(2).Height = 26.25;
                sht.Range("A2:F2").Merge();
                sht.Range("A2").SetValue(filterby.TrimStart(','));
                sht.Range("A2:F2").Style.Font.Bold = true;
                sht.Range("A2:F2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:F2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A2:F2").Style.Alignment.WrapText = true;

                //Headings
                sht.Range("A3:F3").Style.Font.Bold = true;
                sht.Range("A3").SetValue("Receive Date");
                sht.Range("B3").SetValue("Stock No");
                sht.Range("C3").SetValue("Employee Name");
                sht.Range("D3").SetValue("ItemDescription");
                sht.Range("E3").SetValue("Status");
                sht.Range("F3").SetValue("Penality Remarks");
                //***************
                int row = 4;
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "Stockno";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());

                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Receivedate"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Tstockno"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["EmpName"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["ItemDescription"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Status"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["PRemarks"]);

                    row = row + 1;
                }
                ds1.Dispose();
                ds.Dispose();
                sht.Columns(1, 9).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("Hold/Rejected Pcs_" + DateTime.Now + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                //Download File
                Response.ClearContent();
                Response.ClearHeaders();
                // Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                // File.Delete(Path);
                Response.End();

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No records found for this combination.')", true);
            }
        }

        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}