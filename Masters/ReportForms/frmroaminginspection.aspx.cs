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
            param[1] = new SqlParameter("@processid", DDprocessname.SelectedValue);
            param[2] = new SqlParameter("@fromdate", txtfromdate.Text);
            param[3] = new SqlParameter("@Todate", txttodate.Text);
            param[4] = new SqlParameter("@Unitsid", DDunit.SelectedValue);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETROAMINGINSPECTIONREPORT", param);
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
                sht.Range("A2:L2").Merge();
                sht.Range("A2").SetValue("ROAMING INSPECTION REPORT (" + DDprocessname.SelectedItem.Text + ")");
                sht.Range("A3:L3").Merge();
                sht.Range("A3").SetValue("From :" + txtfromdate.Text + "  To : " + txttodate.Text);
                sht.Range("A2:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:L3").Style.Font.SetBold();

                //Headers
                sht.Range("A4").Value = "USER NAME";
                sht.Range("B4").Value = "STOCK NO";
                sht.Range("C4").Value = "SCAN DATE";
                sht.Range("D4").Value = "QUALITY";
                sht.Range("E4").Value = "DESIGN";
                sht.Range("F4").Value = "COLOR";
                sht.Range("G4").Value = "SIZE";
                sht.Range("H4").Value = "QTY";
                sht.Range("I4").Value = "EMPNAME";
                sht.Range("J4").Value = "DEFECT";
                sht.Range("K4").Value = "REMARK";
                sht.Range("L4").Value = "STATUS";

                sht.Range("A4:L4").Style.Font.Bold = true;

                row = 5;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["USerName"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["TstockNo"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["scandate"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Designname"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["colorname"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Sizeft"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Defect"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["Remark"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Status"]);

                    row = row + 1;

                }
                sht.Range("G" + row).SetValue("TOTAL");
                sht.Range("H" + row).FormulaA1 = "=SUM(H5:H" + (row - 1) + ")";
                sht.Range("G" + row + ":H" + row + "").Style.Font.Bold = true;
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
                string filename = UtilityModule.validateFilename("RoamingInspectionReport(" + DDprocessname.SelectedItem.Text + ")" + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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