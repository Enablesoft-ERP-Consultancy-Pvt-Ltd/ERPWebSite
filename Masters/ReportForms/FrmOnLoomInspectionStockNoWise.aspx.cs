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
using System.IO;

public partial class Masters_Campany_FrmOnLoomInspectionStockNoWise : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtStockNo.Text = "";
            txtStockNo.Focus();
        }
    }

    protected void txtStockNo_TextChanged(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@TStockNo", txtStockNo.Text);
        param[1] = new SqlParameter("@CompanyID", Session["CurrentWorkingCompanyID"]);

        //****
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETONLOOMINSPPECTION_STOCKNOWISE_REPORT", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/TempExcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/TempExcel/"));
            }
            //*************
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("ON LOOM INSPECTION STOCKNO WISE");
            int row = 0;
            string Path = "";

            sht.Range("A1:N1").Merge();
            sht.Range("A1").Value = "STOCK NO WISE WEAVING DETAILS";
            sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:N1").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A1:N1").Style.Font.FontSize = 10;
            sht.Range("A1:N1").Style.Font.Bold = true;

            row = 2;

            DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "TStockNo", "QualityName", "designName", "ColorName", "SIZE", "SizeInch", "SizeFt", "AssignDate");
            DataView dv1 = new DataView(dtdistinct);
            dv1.Sort = "TStockNo";
            DataTable dtdistinct1 = dv1.ToTable();
            foreach (DataRow dr in dtdistinct1.Rows)
            {
                sht.Range("A" + row).SetValue(dr["TStockNo"]);
                sht.Range("B" + row).SetValue(dr["QualityName"]);
                sht.Range("C" + row).SetValue(dr["designName"]);
                sht.Range("D" + row).SetValue(dr["ColorName"]);
                sht.Range("E" + row).SetValue(dr["SIZE"]);
                sht.Range("F" + row).SetValue(dr["SizeInch"]);
                sht.Range("G" + row).SetValue(dr["SizeFt"]);
                sht.Range("H" + row).SetValue(dr["AssignDate"]);

                sht.Range("A" + row + ":N" + row).Style.Font.Bold = true;

                row = row + 1;

                sht.Range("B" + row).SetValue("Date");
                sht.Range("C" + row).SetValue("EmpName");
                sht.Range("D" + row).SetValue("Weaving CM");
                sht.Range("E" + row).SetValue("Weaving FT");
                sht.Range("F" + row).SetValue("Weaving INCH");

                sht.Range("G" + row).SetValue("Production Wages CM");
                sht.Range("H" + row).SetValue("Production Wages FT");
                sht.Range("I" + row).SetValue("Production Wages INCH");

                sht.Range("B" + row + ":I" + row).Style.Font.Bold = true;

                row = row + 1;

                DataTable dtdistinctDate = ds.Tables[0].DefaultView.ToTable(true, "Date");
                DataView dv2 = new DataView(dtdistinctDate);
                dv2.Sort = "Date";
                DataTable dtdistinctDate2 = dv2.ToTable();
                foreach (DataRow drDate in dtdistinctDate2.Rows)
                {
                    DataView dvitemdesc = new DataView(ds.Tables[0]);
                    dvitemdesc.RowFilter = "Date='" + drDate["Date"] + "' ";
                    DataSet dsitemdesc = new DataSet();
                    dsitemdesc.Tables.Add(dvitemdesc.ToTable());
                    DataTable dtdistinctitemdesc = dsitemdesc.Tables[0];

                    foreach (DataRow dritemdesc in dtdistinctitemdesc.Rows)
                    {
                        sht.Range("B" + row).SetValue(String.Format("{0:d/MMM/yyyy}", dritemdesc["Date"]));
                        sht.Range("C" + row).SetValue(dritemdesc["EmpName"]);
                        sht.Range("D" + row).SetValue(dritemdesc["ProductionCM"]);
                        sht.Range("E" + row).SetValue(dritemdesc["ProductionFT"]);
                        sht.Range("F" + row).SetValue(dritemdesc["ProductionINCH"]);
                        sht.Range("G" + row).SetValue(dritemdesc["ProductionWagesCM"]);
                        sht.Range("H" + row).SetValue(dritemdesc["ProductionWagesFt"]);
                        sht.Range("I" + row).SetValue(dritemdesc["ProductionWagesINCH"]);

                        row = row + 1;
                    }
                    row = row + 1;
                }

            }

            //*************
            sht.Columns(1, 20).AdjustToContents();
            //********************
            //***********BOrders
            using (var a = sht.Range("A1" + ":N" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("OnLoomInspectionStockNoWise_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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