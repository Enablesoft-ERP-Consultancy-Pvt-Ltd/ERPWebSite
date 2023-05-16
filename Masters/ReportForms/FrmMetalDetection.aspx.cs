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
public partial class Masters_ReportForms_frmroaminginspection : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.COmpanyid And CA.Userid=" + Session["varuserid"] + @"
                           SELECT PROCESS_NAME_ID,PROCESS_NAME FROM  PROCESS_NAME_MASTER PNM WHERE PROCESS_NAME NOT LIKE 'AQL%' ORDER BY PROCESS_NAME
                           select UnitsId,UnitName From Units order by UnitName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDprocessname, ds, 1, true, "--Plz Select Process--");
            UtilityModule.ConditionalComboFillWithDS(ref DDunit, ds, 2, true, "--Plz Select Unit Name--");
            ds.Dispose();
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void btngetdata_Click(object sender, EventArgs e)
    {
        RoamingInspection();
    }
    protected void RoamingInspection()
    {
        lblmsg.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@companyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@fromdate", txtfromdate.Text);
            param[2] = new SqlParameter("@Todate", txttodate.Text);
            param[3] = new SqlParameter("@Unitsid", DDunit.SelectedValue);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETMETALDETECTIONREPORT", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                sht.Range("A1:L1").Merge();
                sht.Range("A1").SetValue(DDCompany.SelectedItem.Text);
                sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:J2").Merge();
                sht.Range("A2").SetValue("Metal Detection Record for Unpacked Product");
                sht.Range("A3:L3").Merge();
                sht.Range("A3").SetValue("From :" + txtfromdate.Text + "  To : " + txttodate.Text);
                sht.Range("A2:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:L3").Style.Font.SetBold();

                //Headers
                sht.Range("A4").Value = "Date";
                sht.Range("B4").Value = "Stock No.";
                sht.Range("C4").Value = "Article Name/Design No.";
                sht.Range("D4").Value = "Color";
                sht.Range("E4").Value = "Size";
                sht.Range("F4").Value = "Quantity";
                sht.Range("G4").Value = "Result";
                sht.Range("H4").Value = "Defects";
                sht.Range("I4").Value = "Scan By";
                sht.Range("J4").Value = "Remark";
                sht.Range("K4").Value = "Customer Code";
                sht.Range("L4").Value = "Customer Order No";

                sht.Range("A4:L4").Style.Font.Bold = true;

                row = 5;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["ScanDate"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["TstockNo"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Designname"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["colorname"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Sizeft"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Status"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Defects"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Username"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Remark"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);

                    row = row + 1;

                }
                sht.Range("E" + row).SetValue("TOTAL");
                sht.Range("F" + row).FormulaA1 = "=SUM(F5:F" + (row - 1) + ")";
                sht.Range("E" + row + ":F" + row + "").Style.Font.Bold = true;
                using (var a = sht.Range("A4:L" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                sht.Columns(1, 20).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("MetalDetectionReport(" + DDprocessname.SelectedItem.Text + ")" + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altrpt", "alert('No records found for this combination.')", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = "";
        }
    }
}
