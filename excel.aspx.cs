using ClosedXML.Excel;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Wordprocessing;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class excel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        var result = new MemoryStream();
        var fileName = Request.QueryString["name"].Trim();
        if(Request.QueryString["ac"].Trim() == "pwd")
        {
            result = From_Dataset();
        }
        var timeStr = DateTime.Now.ToString("yyyy-MM-ddTHHmmss");
        fileName = fileName+"_"+ timeStr;
        result.WriteTo(HttpContext.Current.Response.OutputStream);
        HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename="+ fileName + ".xlsx");
        HttpContext.Current.Response.StatusCode = 200;
        HttpContext.Current.Response.End();
    }
        
    private MemoryStream From_Dataset()
    {
        var result = new MemoryStream();
        try
        {
            
            string DDCustCode = Request.QueryString["DDCustCode"].Trim();
            string DDCompany = Request.QueryString["DDCompany"].Trim();
            string DDOrderNo = Request.QueryString["DDOrderNo"].Trim();
            string DDCategory = Request.QueryString["DDCategory"].Trim();
            string ddItemName = Request.QueryString["ddItemName"].Trim();
            string DDQuality = Request.QueryString["DDQuality"].Trim();
            string DDDesign = Request.QueryString["DDDesign"].Trim();
            string DDColor = Request.QueryString["DDColor"].Trim();
            string Sizeid = Request.QueryString["DDSize"].Trim();
            string mastercompanyid = Request.QueryString["mastercompanyid"].Trim();

            DataSet ds = SqlHelperNew.ExecuteDataset("PRO_PROCESS_DETAIL_SUMMARY",
                Convert.ToInt32(DDCompany == "" ? "0" : DDCompany),
                Convert.ToInt32(DDCustCode == "" ? "0" : DDCustCode),
                Convert.ToInt32(DDOrderNo == "" ? "0" : DDOrderNo),
                Convert.ToInt32(DDCategory == "" ? "0" : DDCategory),
                Convert.ToInt32(ddItemName == "" ? "0" : ddItemName),
                Convert.ToInt32(DDQuality == "" ? "0" : DDQuality),
                Convert.ToInt32(DDDesign == "" ? "0" : DDDesign),
                Convert.ToInt32(DDColor == "" ? "0" : DDColor),
                Convert.ToInt32(Sizeid == "" ? "0" : Sizeid),
                Convert.ToInt32(mastercompanyid == "" ? "0" : mastercompanyid),
                1, "GET"); ;

        var memoryStream = new MemoryStream();
        int col = 1;
        int row = 1;
        using (ExcelPackage package = new ExcelPackage(memoryStream))
        {
            ExcelWorksheet worksheet;
            worksheet = package.Workbook.Worksheets.Add("sheet1");

            worksheet.Name = "sheet1";

            worksheet.Cells[row, col].Value = "S.No.";
                col++;
            foreach (DataColumn coln in ds.Tables[0].Columns)
            {
                worksheet.Cells[row, col].Value = coln.ColumnName;
                col++;
            }
            row++;
            int cnt = 1;
            col = 1;
            foreach (DataRow rw in ds.Tables[0].Rows)
            {
                worksheet.Cells[row, col].Value = cnt.ToString();
                cnt++;
                col++;
                foreach (DataColumn cl in ds.Tables[0].Columns)
                {
                    if (rw[cl.ColumnName] != DBNull.Value)
                        worksheet.Cells[row, col].Value = rw[cl.ColumnName].ToString();
                    col++;
                }
                row++;
                col = 1;
            }
            result = new MemoryStream(package.GetAsByteArray());
            //package.Save();
        }
        }
        catch(Exception ex)
        {
            throw ex;
        }
        return result;
    }
}