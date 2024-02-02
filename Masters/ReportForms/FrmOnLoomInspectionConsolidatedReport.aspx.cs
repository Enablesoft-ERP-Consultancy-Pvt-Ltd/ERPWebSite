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
using ClosedXML.Excel;

public partial class Masters_Campany_FrmOnLoomInspectionConsolidatedReport : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtDate.Text = "";
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        if (RDConsolidated.Checked == true)
        {
            ConsolidatedReport();
        }
        else if (RDFolioMaterialLotNoTrack.Checked == true)
        {
            FolioMaterialLotNoTrackReport();
        }
    }
    protected void ConsolidatedReport()
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@Date", txtDate.Text);

        //****
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETONLOOMINSPPECTION_CONSOLIDATED_REPORT", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/TempExcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/TempExcel/"));
            }
            //*************
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("ON LOOM INSPECTION CONSOLIDATED");
            int row = 0;
            string Path = "";

            sht.Range("A1:N1").Merge();
            sht.Range("A1").Value = "CONSOLIDATED PRODUCTION DETAILS" + " " + txtDate.Text;
            sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:N1").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A1:N1").Style.Font.FontSize = 10;
            sht.Range("A1:N1").Style.Font.Bold = true;

            sht.Range("A2").Value = "QUALITY";
            sht.Range("B2").Value = "DESIGN";
            sht.Range("C2").Value = "COLOR";
            sht.Range("D2").Value = "SIZE";
            sht.Range("E2").Value = "NO.OF LOOMS";
            sht.Range("F2").Value = "WORK IN CM";
            sht.Range("G2").Value = "WORK IN M2";
            sht.Range("H2").Value = "LOOM PRODUCTIVITY";
            sht.Range("I2").Value = "NO.OF WEAVERS";
            sht.Range("J2").Value = "WEAVER PRODUCTIVITY";
            sht.Range("K2").Value = "WORK IN INCH";

            sht.Range("A2:K2").Style.Font.Bold = true;

            row = 2;

            DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "QualityName");
            DataView dv1 = new DataView(dtdistinct);
            DataTable dtdistinct1 = dv1.ToTable();

            foreach (DataRow dr in dtdistinct1.Rows)
            {
                decimal TotalNoOfLoom = 0;
                decimal TotalWorkInCM = 0;
                decimal TotalWorkInM2 = 0;
                decimal TotalNoOfWeaver = 0;
                decimal TotalWorkInInch = 0;
                DataTable dtdistinctItemDetails = ds.Tables[0];
                DataView dv2 = new DataView(dtdistinctItemDetails);
                dv2.RowFilter = "QualityName='" + dr["QualityName"] + "'";
                DataTable dtdistinctItemDetails2 = dv2.ToTable();

                row = row + 1;

                foreach (DataRow dr2 in dtdistinctItemDetails2.Rows)
                {
                    sht.Range("A" + row).SetValue(dr2["QualityName"]);
                    sht.Range("B" + row).SetValue(dr2["DesignName"]);
                    sht.Range("C" + row).SetValue(dr2["ColorName"]);
                    sht.Range("D" + row).SetValue(dr2["Size"]);
                    sht.Range("E" + row).SetValue(dr2["NoOfLooms"]);
                    sht.Range("K" + row).SetValue(dr2["WorkInInch"]);

                    decimal WorkInCm = 0;
                    WorkInCm = (Convert.ToDecimal(dr2["WorkInInch"]) * Convert.ToDecimal(2.54));
                    sht.Range("F" + row).SetValue(WorkInCm);
                    sht.Range("F" + row).Style.NumberFormat.Format = "#,###0.00";
                    decimal WorkInMtr = 0;
                    WorkInMtr = Convert.ToDecimal(dr2["Width"]) * WorkInCm / 10000;
                    sht.Range("G" + row).SetValue(Math.Round(WorkInMtr, 2));
                    sht.Range("H" + row).SetValue(Math.Round(WorkInMtr / Convert.ToDecimal(dr2["NoOfLooms"]), 5));
                    sht.Range("I" + row).SetValue(dr2["NoOfWeavers"]);
                    sht.Range("J" + row).SetValue(Math.Round(WorkInMtr / Convert.ToDecimal(dr2["NoOfWeavers"]), 2));

                    //TotalNoOfLoom = Convert.ToDecimal(ds.Tables[0].Compute("sum(NoOfLooms)", "QualityName='" + dr2["QualityName"] + "' "));
                    TotalNoOfLoom = TotalNoOfLoom + Convert.ToDecimal(dr2["NoOfLooms"]);
                    TotalWorkInCM = TotalWorkInCM+WorkInCm;
                    TotalWorkInM2 = TotalWorkInM2 + WorkInMtr;
                    TotalNoOfWeaver = TotalNoOfWeaver + Convert.ToDecimal(dr2["NoOfWeavers"]);
                    TotalWorkInInch = TotalWorkInInch + Convert.ToDecimal(dr2["WorkInInch"]);

                    row = row + 1;
                }
               
                sht.Range("D" + row).SetValue("Total");
                sht.Range("D" + row, "K" + row).Style.Font.Bold = true;
                sht.Range("E" + row).SetValue(TotalNoOfLoom);               
                sht.Range("F" + row).SetValue(TotalWorkInCM);
                sht.Range("F" + row).Style.NumberFormat.Format = "#,###0.00";
                sht.Range("G" + row).SetValue(TotalWorkInM2);
                sht.Range("G" + row).Style.NumberFormat.Format = "#,###0.00";
                sht.Range("I" + row).SetValue(TotalNoOfWeaver);
                sht.Range("K" + row).SetValue(TotalWorkInInch);

                row = row + 1;
            }


            #region
            ////for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            ////{
            ////    //sht.Range("A" + row + ":J" + row).Style.Font.FontName = "Arial Unicode MS";
            ////    //sht.Range("A" + row + ":J" + row).Style.Font.FontSize = 9;

            ////    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
            ////    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
            ////    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
            ////    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Size"]);
            ////    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["NoOfLooms"]);
            ////    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["WorkInInch"]);

            ////    decimal WorkInCm = 0;
            ////    WorkInCm = (Convert.ToDecimal(ds.Tables[0].Rows[i]["WorkInInch"]) * Convert.ToDecimal(2.54));
            ////    sht.Range("F" + row).SetValue(WorkInCm);
            ////    decimal WorkInMtr = 0;
            ////    WorkInMtr = Convert.ToDecimal(ds.Tables[0].Rows[i]["Width"]) * WorkInCm / 10000;
            ////    sht.Range("G" + row).SetValue(Math.Round(WorkInMtr, 5));
            ////    sht.Range("H" + row).SetValue(Math.Round(WorkInMtr / Convert.ToDecimal(ds.Tables[0].Rows[i]["NoOfLooms"]), 5));
            ////    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["NoOfWeavers"]);
            ////    sht.Range("J" + row).SetValue(Math.Round(WorkInMtr / Convert.ToDecimal(ds.Tables[0].Rows[i]["NoOfWeavers"]), 5));

            ////    row = row + 1;

            ////}
            #endregion
           
            row = row + 1;

            //*************
            sht.Columns(1, 20).AdjustToContents();
            //********************
            //***********BOrders
            using (var a = sht.Range("A1" + ":K" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("OnLoomInspectionConsolidatedReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "altrep", "alert('No records Found for this combination.');", true);
        }
    }
    protected void FolioMaterialLotNoTrackReport()
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@FromDate", txtfromDate.Text);
        param[1] = new SqlParameter("@ToDate", txttodate.Text);

        //****
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETFOLIOMATERIALISSUE_LOTNOTRACK_REPORT", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/TempExcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/TempExcel/"));
            }
            //*************
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("FOLIO MATERIAL LOTNO TRACK");
            int row = 0;
            string Path = "";

            sht.Range("A1:N1").Merge();
            sht.Range("A1").Value = "FOLIO MATERIAL LOTNO TRACK REPORT";
            sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:N1").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A1:N1").Style.Font.FontSize = 10;
            sht.Range("A1:N1").Style.Font.Bold = true;

            sht.Range("A2:N2").Merge();
            sht.Range("A2").Value = "FROM" + " " + txtfromDate.Text + " " + "TO" + " " + txttodate.Text;
            sht.Range("A2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:N2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A2:N2").Style.Font.FontSize = 10;
            sht.Range("A2:N2").Style.Font.Bold = true;

            sht.Range("A3").Value = "DATE";
            sht.Range("B3").Value = "FOLIO NO";
            sht.Range("C3").Value = "ITEM NAME";
            sht.Range("D3").Value = "SHADE COLOR";
            sht.Range("E3").Value = "LOT NO";

            sht.Range("A3:F3").Style.Font.Bold = true;

            row = 4;

            DataTable dtdistinctDateIssueOrderId = ds.Tables[0].DefaultView.ToTable(true, "AssignDate", "IssueOrderId");
            DataView dv1 = new DataView(dtdistinctDateIssueOrderId);
            dv1.Sort = "AssignDate";
            DataTable dtdistinctDateIssueOrderId2 = dv1.ToTable();
            foreach (DataRow dr in dtdistinctDateIssueOrderId2.Rows)
            {
                sht.Range("A" + row).SetValue(dr["AssignDate"]);
                sht.Range("B" + row).SetValue(dr["IssueOrderId"]);

                sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;
                row = row + 1;

                DataTable dtdistinctItemName = ds.Tables[0].DefaultView.ToTable(true, "AssignDate", "IssueOrderId", "Item_Name");
                DataView dv2 = new DataView(dtdistinctItemName);
                dv2.RowFilter = "AssignDate='" + dr["AssignDate"] + "' and IssueOrderId='" + dr["IssueOrderId"] + "' ";
                DataTable dtdistinctItemName2 = dv2.ToTable();
                foreach (DataRow dr2 in dtdistinctItemName2.Rows)
                {
                    sht.Range("C" + row).SetValue(dr2["Item_Name"]);

                    row = row + 1;

                    DataTable dtdistinctItemDetails = ds.Tables[0];
                    DataView dv3 = new DataView(dtdistinctItemDetails);
                    dv3.RowFilter = "AssignDate='" + dr["AssignDate"] + "' and IssueOrderId='" + dr["IssueOrderId"] + "' and Item_Name='" + dr2["Item_Name"] + "'";
                    DataTable dtdistinctItemDetails2 = dv3.ToTable();
                    foreach (DataRow dr3 in dtdistinctItemDetails2.Rows)
                    {
                        sht.Range("D" + row).SetValue(dr3["ShadeColorName"]);
                        sht.Range("E" + row).SetValue(dr3["LotNo"]);

                        row = row + 1;
                    }

                    row = row + 1;

                }

                row = row + 1;

            }

            row = row + 1;

            //*************
            sht.Columns(1, 20).AdjustToContents();
            //********************
            //***********BOrders
            using (var a = sht.Range("A1" + ":J" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("FolioMaterialLotNoTrackReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "altrep", "alert('No records Found for this combination.');", true);
        }
    }
}