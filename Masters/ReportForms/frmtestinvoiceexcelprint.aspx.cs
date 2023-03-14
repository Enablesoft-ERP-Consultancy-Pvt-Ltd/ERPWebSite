using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using ClosedXML.Excel;

using System.IO;

public partial class Masters_ReportForms_frmtestinvoiceexcelprint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    //protected void Exporttoexcel_1()
    //{

    //    int i = 0, Pcs = 0;
    //    Double Area = 0, Amount = 0;
    //    if (!Directory.Exists(Server.MapPath("~/InvoiceExcel/")))
    //    {
    //        Directory.CreateDirectory(Server.MapPath("~/InvoiceExcel/"));
    //    }
    //    Excel.Application xapp = new Microsoft.Office.Interop.Excel.Application();
    //    Excel.Workbook xbk;
    //    Excel.Worksheet sht;
    //    object misvalue = System.Reflection.Missing.Value;
    //    xbk = xapp.Workbooks.Add(misvalue);
    //    sht = (Excel.Worksheet)xbk.Worksheets.get_Item(1);
    //    //************set cell width
    //    ((Range)sht.Columns["A", Type.Missing]).ColumnWidth = 14.71;
    //    ((Range)sht.Columns["B", Type.Missing]).ColumnWidth = 8.86;
    //    ((Range)sht.Columns["C", Type.Missing]).ColumnWidth = 18.00;
    //    ((Range)sht.Columns["D", Type.Missing]).ColumnWidth = 9.29;
    //    ((Range)sht.Columns["E", Type.Missing]).ColumnWidth = 10.43;
    //    ((Range)sht.Columns["F", Type.Missing]).ColumnWidth = 15.86;
    //    ((Range)sht.Columns["G", Type.Missing]).ColumnWidth = 12.86;
    //    ((Range)sht.Columns["H", Type.Missing]).ColumnWidth = 18.29;
    //    ((Range)sht.Columns["I", Type.Missing]).ColumnWidth = 10.71;
    //    ((Range)sht.Columns["J", Type.Missing]).ColumnWidth = 12.29;
    //    ((Range)sht.Columns["K", Type.Missing]).ColumnWidth = 23.14;
    //    //************
    //    ((Range)sht.Rows[1, Type.Missing]).RowHeight = 15.75;

    //    //*****Styles
    //    ExcelStyle HStyle = sht.Application.ActiveWorkbook.Styles.Add("Hstyle");
    //    var With = HStyle;
    //    With.Font.Size = "12";
    //    With.Font.Name = "Tahoma";
    //    With.NumberFormat = "@";
    //    With.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
    //    With.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
    //    With.Font.Bold = true;
    //    //**********Normal
    //    ExcelStyle NormalStyle = sht.Application.ActiveWorkbook.Styles.Add("NormalStyle");
    //    var With1 = NormalStyle;
    //    With1.Font.Size = "12";
    //    With1.Font.Name = "Tahoma";
    //    With1.NumberFormat = "@";
    //    //***************Normal Bold
    //    ExcelStyle NormalStyleWithBold = sht.Application.ActiveWorkbook.Styles.Add("NormalStyleWithBold");
    //    var With2 = NormalStyleWithBold;
    //    With2.Font.Size = "12";
    //    With2.Font.Name = "Tahoma";
    //    With2.NumberFormat = "@";
    //    With2.Font.Bold = true;
    //    //***************Normal Bold centre
    //    ExcelStyle NormalStyleWithBold_Centre = sht.Application.ActiveWorkbook.Styles.Add("NormalStyleWithBold_Centre");
    //    var With3 = NormalStyleWithBold_Centre;
    //    With3.Font.Size = "12";
    //    With3.Font.Name = "Tahoma";
    //    With3.NumberFormat = "@";
    //    With3.Font.Bold = true;
    //    With3.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
    //    //***************Normal  centre
    //    ExcelStyle NormalStyleWith_Centre = sht.Application.ActiveWorkbook.Styles.Add("NormalStyleWith_Centre");
    //    var With4 = NormalStyleWith_Centre;
    //    With4.Font.Size = "12";
    //    With4.Font.Name = "Tahoma";
    //    With4.NumberFormat = "@";
    //    With4.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
    //    //***********Header        
    //    sht.get_Range("A1").Value = "INVOICE";
    //    sht.get_Range("A1:K1").Style = HStyle;
    //    sht.get_Range("A1:K1").MergeCells = true;

    //    string str = @"select * From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=1";
    //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        //********Exporter
    //        sht.get_Range("A2").Value = "Exporter";
    //        sht.get_Range("A2:F2").Style = NormalStyle;
    //        sht.get_Range("A2:F2").MergeCells = true;

    //        //CompanyName
    //        sht.get_Range("A3").Value = ds.Tables[0].Rows[0]["companyName"];
    //        sht.get_Range("A3:F3").Style = NormalStyleWithBold;
    //        sht.get_Range("A3:F3").MergeCells = true;
    //        //Address
    //        sht.get_Range("A4").Value = ds.Tables[0].Rows[0]["compaddr1"];
    //        sht.get_Range("A4:F4").Style = NormalStyle;
    //        sht.get_Range("A4:F4").MergeCells = true;
    //        //address2
    //        sht.get_Range("A5").Value = ds.Tables[0].Rows[0]["compaddr2"] + "," + ds.Tables[0].Rows[0]["compaddr3"];
    //        sht.get_Range("A5:F5").Style = NormalStyle;
    //        sht.get_Range("A5:F5").MergeCells = true;
    //        //TiN No
    //        sht.get_Range("A6").Value = "TIN#" + ds.Tables[0].Rows[0]["TinNo"];
    //        sht.get_Range("A6:F6").Style = NormalStyleWithBold;
    //        sht.get_Range("A6:F6").MergeCells = true;
    //        //**********INvoiceNodate
    //        sht.get_Range("G2").Value = "Invoice No./Date";
    //        sht.get_Range("G2:K2").Style = NormalStyle;
    //        sht.get_Range("G2:K2").MergeCells = true;
    //        //value
    //        sht.get_Range("G3").Value = ds.Tables[0].Rows[0]["TInvoiceNo"] + " Dated " + ds.Tables[0].Rows[0]["InvoiceDate"];
    //        sht.get_Range("G3:K3").Style = NormalStyle;
    //        sht.get_Range("G3:K3").MergeCells = true;
    //        sht.get_Range("G3", "K3").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
    //        //*************IE Code
    //        sht.get_Range("G4").Value = "IE Code No.";
    //        sht.get_Range("G4:I4").Style = NormalStyle;
    //        sht.get_Range("G4:I4").MergeCells = true;
    //        //value
    //        sht.get_Range("G5").Value = ds.Tables[0].Rows[0]["IEcode"];
    //        sht.get_Range("G5:I5").Style = NormalStyle;
    //        sht.get_Range("G5:I5").MergeCells = true;
    //        sht.get_Range("G5", "I5").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
    //        //***********GRI Form No
    //        sht.get_Range("J4").Value = "GRI Form No.";
    //        sht.get_Range("J4:K4").Style = NormalStyle;
    //        sht.get_Range("J4:K4").MergeCells = true;
    //        // value
    //        sht.get_Range("J5").Value = "";
    //        sht.get_Range("J5:K5").Style = NormalStyle;
    //        sht.get_Range("J5:K5").MergeCells = true;
    //        sht.get_Range("J5", "K5").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
    //        //*************Buyer's Order No and Date
    //        sht.get_Range("G6").Value = "Buyer's Order No. / Date";
    //        sht.get_Range("G6:K6").Style = NormalStyle;
    //        sht.get_Range("G6:K6").MergeCells = true;
    //        //value
    //        sht.get_Range("G7").Value = ds.Tables[0].Rows[0]["TorderNo"];
    //        sht.get_Range("G7:K7").Style = NormalStyle;
    //        sht.get_Range("G7:K7").MergeCells = true;
    //        sht.get_Range("G7", "K7").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
    //        //**************Other Reference(s)
    //        sht.get_Range("G8").Value = "Other Reference(s)";
    //        sht.get_Range("G9").Value = "CONSIGNMENT NO.:" + ds.Tables[0].Rows[0]["otherref"];
    //        sht.get_Range("G10").Value = "SUPPLIER NO.:";
    //        sht.get_Range("G8:G9:G10").Style = NormalStyle;
    //        sht.get_Range("G8:K8").MergeCells = true;
    //        sht.get_Range("G9:K9").MergeCells = true;
    //        sht.get_Range("G10:K10").MergeCells = true;
    //        //*************Consignee
    //        sht.get_Range("A11").Value = "Consignee";
    //        sht.get_Range("C11").Value = ds.Tables[0].Rows[0]["consignee_dt"];
    //        sht.get_Range("A11:B11").Style = NormalStyle;
    //        sht.get_Range("C11:F11").Style = NormalStyleWithBold;
    //        sht.get_Range("A11:B11").MergeCells = true;
    //        sht.get_Range("C11:F11").MergeCells = true;
    //        //value
    //        sht.get_Range("A12").Value = ds.Tables[0].Rows[0]["consignee"];
    //        sht.get_Range("A12:F12").Style = NormalStyleWithBold;
    //        sht.get_Range("A12:F12").MergeCells = true;
    //        //**
    //        sht.get_Range("A13").Value = ds.Tables[0].Rows[0]["consignee_address"];
    //        sht.get_Range("A13:F16").Style = NormalStyle;
    //        sht.get_Range("A13", "F16").Cells.WrapText = true;
    //        sht.get_Range("A13", "F16").Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
    //        sht.get_Range("A13:F16").MergeCells = true;
    //        //***********Notify
    //        sht.get_Range("A17").Value = "Notify Party";
    //        sht.get_Range("C17").Value = ds.Tables[0].Rows[0]["Notifyparty_dt"];
    //        sht.get_Range("A17:B17").Style = NormalStyle;
    //        sht.get_Range("A17:B17").Cells.Font.Underline = true;
    //        sht.get_Range("C17:F17").Style = NormalStyleWithBold;
    //        sht.get_Range("A17:B17").MergeCells = true;
    //        sht.get_Range("C17:F17").MergeCells = true;
    //        //value
    //        sht.get_Range("A18").Value = ds.Tables[0].Rows[0]["Notifyparty"];
    //        sht.get_Range("A18:F18").Style = NormalStyleWithBold;
    //        sht.get_Range("A18:F18").MergeCells = true;

    //        sht.get_Range("A19").Value = ds.Tables[0].Rows[0]["notifyparty_address"];
    //        sht.get_Range("A19:F23").Style = NormalStyle;
    //        sht.get_Range("A19", "F23").Cells.WrapText = true;
    //        sht.get_Range("A19", "F23").Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
    //        sht.get_Range("A19:F23").MergeCells = true;
    //        //***********Receiver address
    //        sht.get_Range("G11").Value = "Receiver Address";
    //        sht.get_Range("G11:I11").Style = NormalStyle;
    //        sht.get_Range("G11:I11").MergeCells = true;
    //        //values
    //        sht.get_Range("J11").Value = ds.Tables[0].Rows[0]["Notifyparty_dt"];
    //        sht.get_Range("J11:K11").Style = NormalStyleWithBold;
    //        sht.get_Range("J11:K11").MergeCells = true;
    //        //****** 1.
    //        sht.get_Range("G12").Value = "1. " + ds.Tables[0].Rows[0]["Receiver"];
    //        sht.get_Range("G12:K12").Style = NormalStyleWithBold;
    //        sht.get_Range("G12:K12").MergeCells = true;
    //        //*
    //        sht.get_Range("G13").Value = ds.Tables[0].Rows[0]["Receiver_address"];
    //        sht.get_Range("G13:K15").Style = NormalStyle;
    //        sht.get_Range("G13:K15").Cells.WrapText = true;
    //        sht.get_Range("G13:K15").Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
    //        sht.get_Range("G13:K15").MergeCells = true;
    //        //****** 2.
    //        sht.get_Range("G16").Value = "2.Paying Agent : " + ds.Tables[0].Rows[0]["payingagent"];
    //        sht.get_Range("G16:K16").Style = NormalStyleWithBold;
    //        sht.get_Range("G16:K16").MergeCells = true;
    //        //*
    //        sht.get_Range("I17").Value = ds.Tables[0].Rows[0]["Receiver_address"];
    //        sht.get_Range("I17:K19").Style = NormalStyle;
    //        sht.get_Range("I17:K19").Cells.WrapText = true;
    //        sht.get_Range("I17:K19").Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
    //        sht.get_Range("I17:K19").MergeCells = true;
    //        //*******3.
    //        sht.get_Range("G20").Value = "Buyer (If other than Consignee)";
    //        sht.get_Range("G20:K20").Style = NormalStyle;
    //        sht.get_Range("G20:K20").MergeCells = true;

    //        sht.get_Range("G21").Value = "3." + ds.Tables[0].Rows[0]["buyer_otherthanconsignee"];
    //        sht.get_Range("G21:K21").Style = NormalStyleWithBold;
    //        sht.get_Range("G21:K21").MergeCells = true;
    //        //*
    //        sht.get_Range("G22").Value = ds.Tables[0].Rows[0]["otherthanconsignee_address"];
    //        sht.get_Range("G22:K24").Style = NormalStyle;
    //        sht.get_Range("G22:K24").Cells.WrapText = true;
    //        sht.get_Range("G22:K24").Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
    //        sht.get_Range("G22:K24").MergeCells = true;
    //        //***********Pre-carriage By
    //        sht.get_Range("A24").Value = "Pre-Carriage By";
    //        sht.get_Range("A24:C24").Style = NormalStyle;
    //        sht.get_Range("A24:C24").MergeCells = true;
    //        //value
    //        sht.get_Range("A25").Value = ds.Tables[0].Rows[0]["Pre_carriageby"];
    //        sht.get_Range("A25:C25").Style = NormalStyle;
    //        sht.get_Range("A25:C25").MergeCells = true;
    //        sht.get_Range("A25:C25").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
    //        //************Place of Receipt by Pre-Carrier
    //        sht.get_Range("D24").Value = "Place of Receipt by Pre-Carrier";
    //        sht.get_Range("D24:F24").Style = NormalStyle;
    //        sht.get_Range("D24:F24").MergeCells = true;
    //        //value
    //        sht.get_Range("D25").Value = ds.Tables[0].Rows[0]["receiptby"];
    //        sht.get_Range("D25:F25").Style = NormalStyle;
    //        sht.get_Range("D25:F25").MergeCells = true;
    //        sht.get_Range("D25:F25").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
    //        //************Vessel/Flight No
    //        sht.get_Range("A26").Value = "Vessel/Flight No";
    //        sht.get_Range("A26:C26").Style = NormalStyle;
    //        sht.get_Range("A26:C26").MergeCells = true;
    //        //value
    //        sht.get_Range("A27").Value = ds.Tables[0].Rows[0]["vessel_flightno"];
    //        sht.get_Range("A27:C27").Style = NormalStyle;
    //        sht.get_Range("A27:C27").MergeCells = true;
    //        sht.get_Range("A27:C27").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
    //        //************Port of Loading
    //        sht.get_Range("D26").Value = "Port of Loading";
    //        sht.get_Range("D26:F26").Style = NormalStyle;
    //        sht.get_Range("D26:F26").MergeCells = true;
    //        //value
    //        sht.get_Range("D27").Value = ds.Tables[0].Rows[0]["portload"];
    //        sht.get_Range("D27:F27").Style = NormalStyle;
    //        sht.get_Range("D27:F27").MergeCells = true;
    //        sht.get_Range("D27:F27").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
    //        //************Port of Discharge
    //        sht.get_Range("A28").Value = "Port of Discharge";
    //        sht.get_Range("A28:C28").Style = NormalStyle;
    //        sht.get_Range("A28:C28").MergeCells = true;
    //        //value
    //        sht.get_Range("A29").Value = ds.Tables[0].Rows[0]["portunload"];
    //        sht.get_Range("A29:C29").Style = NormalStyle;
    //        sht.get_Range("A29:C29").MergeCells = true;
    //        sht.get_Range("A29:C29").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
    //        //************Final Destination
    //        sht.get_Range("D28").Value = "Final Destination";
    //        sht.get_Range("D28:F28").Style = NormalStyle;
    //        sht.get_Range("D28:F28").MergeCells = true;
    //        //value
    //        sht.get_Range("D29").Value = ds.Tables[0].Rows[0]["Destinationadd"];
    //        sht.get_Range("D29:F29").Style = NormalStyle;
    //        sht.get_Range("D29:F29").MergeCells = true;
    //        sht.get_Range("D29:F29").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
    //        //************Country of Origin of Goods
    //        sht.get_Range("G25").Value = "Country of Origin of Goods";
    //        sht.get_Range("G25:I25").Style = NormalStyle;
    //        sht.get_Range("G25:I25").MergeCells = true;
    //        //value
    //        sht.get_Range("G26").Value = ds.Tables[0].Rows[0]["countryoforigin"];
    //        sht.get_Range("G26:I26").Style = NormalStyle;
    //        sht.get_Range("G26:I26").MergeCells = true;
    //        sht.get_Range("G26:I26").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
    //        //************Country of Final Destination
    //        sht.get_Range("J25").Value = "Country of Final Destination";
    //        sht.get_Range("J25:K25").Style = NormalStyle;
    //        sht.get_Range("J25:K25").MergeCells = true;
    //        //value
    //        sht.get_Range("J26").Value = ds.Tables[0].Rows[0]["countryoforigin"];
    //        sht.get_Range("J26:K26").Style = NormalStyle;
    //        sht.get_Range("J26:K26").MergeCells = true;
    //        sht.get_Range("J26:K26").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
    //        //***********Terms of Delivery and Payment
    //        sht.get_Range("G27").Value = "Terms of Delivery and Payment";
    //        sht.get_Range("G27:K27").Style = NormalStyle;
    //        sht.get_Range("G27:K27").MergeCells = true;
    //        //value
    //        sht.get_Range("H28").Value = ds.Tables[0].Rows[0]["Deliveryterm"];
    //        sht.get_Range("H28:K29").Style = NormalStyle;
    //        sht.get_Range("H28:K29").Cells.WrapText = true;
    //        sht.get_Range("H28:K29").Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
    //        sht.get_Range("H28:K29").MergeCells = true;
    //        //*****************Nos
    //        sht.get_Range("A30").Value = "Nos.";
    //        sht.get_Range("B30").Value = "No and kind of Packages";
    //        sht.get_Range("B30:C30").MergeCells = true;
    //        sht.get_Range("E30").Value = "Description of Goods";
    //        sht.get_Range("E30:H30").MergeCells = true;
    //        sht.get_Range("I30").Value = "Quantity";
    //        sht.get_Range("J30").Value = "Rate";
    //        sht.get_Range("k30").Value = "Amount";
    //        sht.get_Range("A30:K30").Style = NormalStyle;
    //        sht.get_Range("K30").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
    //        //values
    //        sht.get_Range("A31:H31").MergeCells = true;
    //        sht.get_Range("A32:H36").Style = NormalStyle;
    //        sht.get_Range("A32").Value = ds.Tables[0].Compute("Min(MinrollNo)", "") + "/" + ds.Tables[0].Compute("Max(Maxrollno)", "");
    //        sht.get_Range("B32").Value = ds.Tables[0].Rows[0]["Noofrolls"] + " Packages";
    //        sht.get_Range("B32:C32").MergeCells = true;
    //        sht.get_Range("D32").Value = ds.Tables[0].Rows[0]["descriptionofgoods"];
    //        sht.get_Range("D32:H36").Cells.WrapText = true;
    //        sht.get_Range("D32:H36").Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
    //        sht.get_Range("D32:H36").MergeCells = true;
    //        sht.get_Range("A37:H37").MergeCells = true;

    //        //**********************Details
    //        //********Hader
    //        sht.get_Range("A38").Value = "P.O.NO.";
    //        sht.get_Range("A38:K38").Style = NormalStyleWithBold_Centre;
    //        sht.get_Range("B38").Value = "ARICLE NAME";
    //        sht.get_Range("B38:C38").MergeCells = true;
    //        sht.get_Range("D38").Value = "ART.NO.";
    //        sht.get_Range("D38:E38").MergeCells = true;
    //        sht.get_Range("F38").Value = "COLOR";
    //        sht.get_Range("G38").Value = "SIZE(CM)";
    //        sht.get_Range("H38").Value = "AREA SQ.MTR.";
    //        sht.get_Range("I38").Value = "QTY";
    //        sht.get_Range("J38").Value = "FCA/" + ds.Tables[0].Rows[0]["CIF"];
    //        sht.get_Range("K38").Value = "FCA/" + ds.Tables[0].Rows[0]["CIF"];
    //        sht.get_Range("K38").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
    //        //***********generate Loop
    //        i = 39;
    //        for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
    //        {
    //            sht.get_Range("A" + i, "K" + i).Style = NormalStyleWith_Centre;
    //            //pono
    //            sht.get_Range("A" + i).Value = ds.Tables[0].Rows[ii]["Pono"];
    //            //Article Name
    //            sht.get_Range("B" + i).Value = ds.Tables[0].Rows[ii]["Design"];
    //            sht.get_Range("B" + i, "C" + i).MergeCells = true;
    //            //Art No.
    //            sht.get_Range("D" + i).Value = ds.Tables[0].Rows[ii]["articleno"];
    //            sht.get_Range("D" + i, "E" + i).MergeCells = true;
    //            //Colour
    //            sht.get_Range("F" + i).Value = ds.Tables[0].Rows[ii]["Color"];
    //            //Size
    //            sht.get_Range("G" + i).Value = ds.Tables[0].Rows[ii]["width"] + "x" + ds.Tables[0].Rows[ii]["Length"];
    //            //Area
    //            sht.get_Range("H" + i).Value = ds.Tables[0].Rows[ii]["Area"];
    //            Area = Area + Convert.ToDouble(ds.Tables[0].Rows[ii]["Area"]);
    //            //QTY
    //            sht.get_Range("I" + i).Value = ds.Tables[0].Rows[ii]["Pcs"];
    //            Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[ii]["Pcs"]);
    //            //
    //            var _with27 = sht.get_Range("I" + i);

    //            _with27.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //            _with27.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //            //rate
    //            sht.get_Range("J" + i).Value = ds.Tables[0].Rows[ii]["Price"];
    //            var _with28 = sht.get_Range("J" + i);

    //            _with28.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //            _with28.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //            //Amount
    //            sht.get_Range("K" + i).Value = ds.Tables[0].Rows[ii]["amount"];
    //            Amount = Amount + Convert.ToDouble(ds.Tables[0].Rows[ii]["amount"]);
    //            //
    //            sht.get_Range("K" + i).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
    //            sht.get_Range("H" + i + ":J" + i + ":K" + i).NumberFormat = "#,###0.00";
    //            sht.get_Range("I" + i).NumberFormat = "@";
    //            i = i + 1;
    //        }
    //        //
    //        var _with29 = sht.get_Range("I" + i + ":I" + (i + 2));

    //        _with29.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        _with29.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        //
    //        var _with30 = sht.get_Range("J" + i + ":J" + (i + 2));

    //        _with30.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        _with30.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        //End Of loop
    //        i = i + 2;
    //        //Total
    //        sht.get_Range("G" + i, "I" + i).Style = NormalStyleWithBold_Centre;
    //        sht.get_Range("G" + i).Value = "Total:";
    //        sht.get_Range("H" + i).Value = Area;
    //        sht.get_Range("I" + i).Value = Pcs;
    //        sht.get_Range("A" + i + ":F" + i).MergeCells = true;
    //        sht.get_Range("H" + i).NumberFormat = "#,###0.00";
    //        //border
    //        var _with23 = sht.get_Range("H" + i);
    //        _with23.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = 2;
    //        _with23.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        //
    //        var _with24 = sht.get_Range("I" + i);
    //        _with24.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = 2;
    //        _with24.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        _with24.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        _with24.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        //Notify Party
    //        i = i + 1;
    //        sht.get_Range("A" + i).Value = "Notify Party:";
    //        sht.get_Range("A" + i).Style = NormalStyle;
    //        sht.get_Range("A" + i).Font.Underline = true;
    //        //
    //        sht.get_Range("B" + i).Value = ds.Tables[0].Rows[0]["Notifyparty2_dt"];
    //        sht.get_Range("B" + i + ":C" + i).Style = NormalStyleWithBold;
    //        sht.get_Range("B" + i + ":C" + i).MergeCells = true;
    //        //border
    //        //
    //        var _with31 = sht.get_Range("I" + i + ":I" + (i + 4));
    //        _with31.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        _with31.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        //
    //        var _with32 = sht.get_Range("J" + i + ":J" + (i + 4));
    //        _with32.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        _with32.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        //
    //        i = i + 1;
    //        sht.get_Range("B" + i).Value = ds.Tables[0].Rows[0]["Notifyparty2_address"];
    //        sht.get_Range("B" + i + ":E" + (i + 3)).Style = NormalStyle;
    //        sht.get_Range("B" + i + ":E" + (i + 3)).Cells.WrapText = true;
    //        sht.get_Range("B" + i + ":E" + (i + 3)).Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
    //        sht.get_Range("B" + i + ":E" + (i + 3)).MergeCells = true;
    //        //border
    //        var _with25 = sht.get_Range("A" + (i - 1) + ":E" + (i + 3));
    //        _with25.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = 2;
    //        _with25.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        _with25.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        _with25.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        //**********Article Loop          
    //        //
    //        i = i + 5;
    //        sht.get_Range("A" + i).Value = "ARTICLE NAME";
    //        sht.get_Range("A" + i + ":B" + i).MergeCells = true;
    //        sht.get_Range("A" + i + ":B" + i).Style = NormalStyleWithBold;
    //        //
    //        sht.get_Range("D" + i).Value = "CONTENT";
    //        sht.get_Range("D" + i + ":F" + i).MergeCells = true;
    //        sht.get_Range("D" + i + ":F" + i).Style = NormalStyleWithBold;
    //        //
    //        sht.get_Range("G" + i).Value = "RATE/SQM";
    //        sht.get_Range("G" + i).Style = NormalStyleWithBold;
    //        //Border
    //        var _with33 = sht.get_Range("I" + (i - 1) + ":I" + i);
    //        _with33.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        _with33.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        //
    //        var _with34 = sht.get_Range("J" + (i - 1) + ":J" + i);
    //        _with34.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        _with34.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        //********
    //        i = i + 1;
    //        str = "Select Design as ArticleNo,contents,Round(sum(amount)/sum(Area),2) as Rate From V_PackingDetailsIkea Where PackingId=1 group by Design,contents";
    //        DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
    //        if (ds1.Tables[0].Rows.Count > 0)
    //        {
    //            for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
    //            {
    //                sht.get_Range("A" + i).Value = ds1.Tables[0].Rows[j]["articleno"];
    //                sht.get_Range("A" + i + ":B" + i).MergeCells = true;
    //                sht.get_Range("A" + i + ":B" + i).Style = NormalStyle;
    //                //
    //                sht.get_Range("D" + i).Value = ds1.Tables[0].Rows[j]["contents"];
    //                sht.get_Range("D" + i + ":F" + i).MergeCells = true;
    //                sht.get_Range("D" + i + ":F" + i).Style = NormalStyle;
    //                //rate
    //                sht.get_Range("G" + i).Value = ds1.Tables[0].Rows[j]["rate"];
    //                sht.get_Range("G" + i).Style = NormalStyle;
    //                sht.get_Range("G" + i).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
    //                //
    //                var _with35 = sht.get_Range("I" + (i) + ":I" + i);
    //                _with35.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //                _with35.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //                //
    //                var _with36 = sht.get_Range("J" + (i) + ":J" + i);
    //                _with36.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //                _with36.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;

    //                i = i + 1;
    //            }
    //        }

    //        //*******amount in words
    //        sht.get_Range("A" + i).Value = "Amount in Words";
    //        sht.get_Range("A" + i + ":B" + i).MergeCells = true;
    //        sht.get_Range("A" + i + ":B" + i).Style = NormalStyle;

    //        string amountinwords = "";
    //        // amountinwords = ChangeNumbersToWords.NumberToWords(Convert.ToInt32(Amount));
    //        sht.get_Range("C" + i).Value = amountinwords;
    //        sht.get_Range("C" + i + ":I" + (i + 1)).Style = NormalStyleWithBold;
    //        sht.get_Range("C" + i + ":I" + (i + 1)).Cells.WrapText = true;
    //        sht.get_Range("C" + i + ":I" + (i + 1)).Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
    //        sht.get_Range("C" + i + ":I" + (i + 1)).MergeCells = true;
    //        //*****Total
    //        sht.get_Range("J" + i).Value = "Total";
    //        sht.get_Range("J" + i).Style = NormalStyleWith_Centre;
    //        //Amount
    //        sht.get_Range("K" + i).Value = Amount;
    //        sht.get_Range("K" + i).Style = NormalStyleWithBold;
    //        sht.get_Range("K" + i).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
    //        sht.get_Range("K" + i).NumberFormat = "#,###0.00";
    //        //
    //        var _with37 = sht.get_Range("K" + i);
    //        _with37.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        _with37.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        _with37.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = 2;
    //        _with37.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        //
    //        //border
    //        var _with26 = sht.get_Range("A" + i + ":K" + i);
    //        _with26.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = 2;
    //        //value
    //        //**********Total
    //        i = i + 2;
    //        //i = 63;
    //        sht.get_Range("A" + i).Value = "TOTAL ROLLS   :";
    //        sht.get_Range("A" + (i + 1)).Value = "TOTAL PCS     :";
    //        sht.get_Range("A" + (i + 2)).Value = "TOTAL GR. WT  :";
    //        sht.get_Range("A" + (i + 3)).Value = "TOTAL NT. WT  :";
    //        sht.get_Range("A" + (i + 4)).Value = "TOTAL SQ.MTR  :";
    //        sht.get_Range("A" + (i + 5)).Value = "TOTAL VOLUME  :";
    //        sht.get_Range("A" + i + ":A" + (i + 5)).Style = NormalStyle;
    //        sht.get_Range("A" + i + ":A" + (i + 5)).Font.Name = "Courier New";
    //        //value
    //        sht.get_Range("C" + i + ":C" + (i + 5)).Style = NormalStyle;
    //        sht.get_Range("C" + i).Value = ds.Tables[0].Rows[0]["Noofrolls"];
    //        sht.get_Range("C" + (i + 1)).Value = Pcs;
    //        sht.get_Range("C" + (i + 2)).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["grosswt"]) + " Kgs.";
    //        sht.get_Range("C" + (i + 3)).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["netwt"]) + " Kgs.";
    //        sht.get_Range("C" + (i + 4)).Value = String.Format("{0:#,0.00}", Area) + " Sq.Mtr";
    //        sht.get_Range("C" + (i + 5)).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["Volume"]) + " CBM";
    //        //******Central Excise
    //        sht.get_Range("F" + i).Value = "Central Excise No. AADFE8796LEM001";
    //        sht.get_Range("F" + (i + 1)).Value = "Range: Bhadohi";
    //        sht.get_Range("F" + (i + 2)).Value = "Division: Allahabad-I";
    //        sht.get_Range("F" + (i + 3)).Value = "Commissionerate: Allahabad";
    //        sht.get_Range("F" + (i + 4)).Value = "Tariff Heading# 570201";
    //        sht.get_Range("F" + i + ":H" + (i + 4)).Style = NormalStyleWithBold;
    //        sht.get_Range("F" + i + ":H" + i).MergeCells = true;
    //        sht.get_Range("F" + (i + 1) + ":H" + (i + 1)).MergeCells = true;
    //        sht.get_Range("F" + (i + 2) + ":H" + (i + 2)).MergeCells = true;
    //        sht.get_Range("F" + (i + 3) + ":H" + (i + 3)).MergeCells = true;
    //        sht.get_Range("F" + (i + 4) + ":H" + (i + 4)).MergeCells = true;
    //        //***********Sig and date
    //        i = i + 6;
    //        //i = 70;
    //        sht.get_Range("A" + i + ":H" + i).MergeCells = true;
    //        sht.get_Range("I" + i).Value = "Signature/Date";
    //        sht.get_Range("I" + i).Style = NormalStyle;
    //        sht.get_Range("K" + i).Value = "For " + ds.Tables[0].Rows[0]["companyname"];
    //        sht.get_Range("J" + i + ":K" + i).Style = NormalStyleWithBold;
    //        sht.get_Range("J" + i + ":K" + i).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
    //        sht.get_Range("J" + i + ":K" + i).Font.Size = 9;
    //        //sht.get_Range("J" + i + ":K" + i).MergeCells = true;
    //        var _with38 = sht.get_Range("I" + i + ":K" + (i + 3));
    //        _with38.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = 2;
    //        _with38.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        //**************Declaration
    //        i = i + 1;
    //        //i = 71;
    //        sht.get_Range("A" + i).Value = "Declaration";
    //        sht.get_Range("A" + i + ":B" + i).MergeCells = true;
    //        sht.get_Range("A" + i + ":B" + i).Style = NormalStyleWithBold_Centre;
    //        //value
    //        i = i + 1;
    //        sht.get_Range("A" + i).Value = "We declare that this invoice shows the actual price of the";
    //        sht.get_Range("A" + i + ":E" + i).MergeCells = true;
    //        sht.get_Range("A" + i + ":E" + i).Style = NormalStyle;
    //        i = i + 1;
    //        sht.get_Range("A" + i).Value = "goods described and that all particulars are true and correct";
    //        sht.get_Range("A" + i + ":E" + i).MergeCells = true;
    //        sht.get_Range("A" + i + ":E" + i).Style = NormalStyle;
    //        //**********
    //        sht.get_Range("I" + i).Value = System.DateTime.Now.ToString("dd/MMM/yyyy");
    //        sht.get_Range("I" + i).Style = NormalStyle;
    //        sht.get_Range("I" + i).Font.Size = 11;
    //        sht.get_Range("I" + i).NumberFormat = "dd/MMM/yyyy";
    //        sht.get_Range("K" + i).Value = "Auth Sign";
    //        sht.get_Range("K" + i).Style = NormalStyleWithBold_Centre;
    //        //
    //        var _with39 = sht.get_Range("I" + i);

    //        _with39.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        //
    //        #region Borders
    //        //*****************Set Borders
    //        var _with1 = sht.get_Range("A2:K" + i);
    //        _with1.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = 2;
    //        _with1.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        _with1.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        _with1.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        //************************
    //        var _with2 = sht.get_Range("A10:K10");
    //        _with2.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        //
    //        var _with3 = sht.get_Range("G2:K3");
    //        _with3.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        _with3.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        _with3.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        //
    //        var _with4 = sht.get_Range("G4:I5");
    //        _with4.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        _with4.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        _with4.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        //
    //        var _with5 = sht.get_Range("J4:K5");
    //        _with5.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        //
    //        var _with6 = sht.get_Range("G6:K7");
    //        _with6.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        _with6.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        _with6.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        //
    //        var _with7 = sht.get_Range("G8:K10");
    //        _with7.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        _with7.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        _with7.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        //
    //        var _with8 = sht.get_Range("A11:F23");
    //        _with8.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        _with8.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        _with8.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        //
    //        var _with9 = sht.get_Range("A24:C25");
    //        _with9.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        _with9.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        _with9.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        //
    //        var _with10 = sht.get_Range("D24:F25");
    //        _with10.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        _with10.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        //
    //        var _with11 = sht.get_Range("A26:C27");
    //        _with11.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        _with11.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        _with11.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        //
    //        var _with12 = sht.get_Range("D26:F27");
    //        _with12.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        _with12.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        //
    //        var _with13 = sht.get_Range("A28:C29");
    //        _with13.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        _with13.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        _with13.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        //
    //        var _with14 = sht.get_Range("D28:F29");
    //        _with14.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        _with14.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        //
    //        var _with15 = sht.get_Range("G25:I26");
    //        _with15.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        _with15.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        _with15.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        _with15.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = 2;
    //        //
    //        var _with16 = sht.get_Range("J25:K26");
    //        _with16.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = 2;
    //        _with16.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        _with16.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        //
    //        var _with17 = sht.get_Range("G27:K29");
    //        _with17.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        _with17.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        _with17.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        //
    //        var _with18 = sht.get_Range("I30:I37");
    //        _with18.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        _with18.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        //
    //        var _with19 = sht.get_Range("J30:J37");
    //        _with19.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        _with19.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        //
    //        var _with20 = sht.get_Range("A38:K38");
    //        _with20.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = 2;
    //        _with20.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
    //        //
    //        var _with21 = sht.get_Range("I38:I38");
    //        _with21.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        _with21.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        //
    //        var _with22 = sht.get_Range("J38:J38");
    //        _with22.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //        _with22.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //        #endregion
    //        //  go to dynamic list
    //    }
    //    else
    //    {

    //    }
    //    //******************
    //    string Fileextension = "xls";
    //    string filename = "InvoiceExcel" + DateTime.Now.Ticks + "." + Fileextension + "";
    //    #region
    //    //switch (xapp.Version.ToString())
    //    //{
    //    //    case "11.0":
    //    //        Fileextension = "xls";
    //    //        break;
    //    //    case "12.0":
    //    //        Fileextension = "xlsx";
    //    //        break;
    //    //    default:
    //    //        Fileextension = "xlsx";
    //    //        break;
    //    //}
    //    #endregion

    //    string Path = Server.MapPath("~/InvoiceExcel/" + filename);

    //    xbk.SaveAs(Path, Excel.XlFileFormat.xlWorkbookNormal, misvalue, misvalue, misvalue, misvalue, Excel.XlSaveAsAccessMode.xlExclusive, misvalue, misvalue, misvalue, misvalue, misvalue);
    //    xbk.Close(true, misvalue, misvalue);
    //    xapp.Quit();

    //    Marshal.ReleaseComObject(sht);
    //    Marshal.ReleaseComObject(xbk);
    //    Marshal.ReleaseComObject(xapp);
    //    //Download File
    //    Response.Clear();
    //    Response.ContentType = "application/vnd.ms-excel";
    //    Response.AddHeader("content-disposition", "attachment;filename=" + filename);
    //    Response.WriteFile(Path);
    //    Response.End();
    //    //*****
    //    File.Delete(Path);
    //}

    protected void btnprint_Click(object sender, EventArgs e)
    {      
        int i = 0, Pcs = 0;
        Decimal Area = 0, Amount = 0;

        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("Invoice1");
        //************set cell width
        sht.Column("A").Width = 14.71;
        sht.Column("B").Width = 8.86;
        sht.Column("C").Width = 18.00;
        sht.Column("D").Width = 9.29;
        sht.Column("E").Width = 10.43;
        sht.Column("F").Width = 15.86;
        sht.Column("G").Width = 12.86;
        sht.Column("H").Width = 18.29;
        sht.Column("I").Width = 10.71;
        sht.Column("J").Width = 12.29;
        sht.Column("K").Width = 23.14;
        //************
        sht.Row(1).Height = 15.75;
        //*****Header
        sht.Cell("A1").Value = "INVOICE";
        sht.Range("A1:K1").Style.Font.FontName = "Tahoma";
        sht.Range("A1:K1").Style.Font.FontSize = 12;
        sht.Range("A1:K1").Style.Font.Bold = true;
        sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A1:K1").Merge();
        //********Exporter
        sht.Range("A2").Value = "Exporter";
        sht.Range("A2:F2").Style.Font.FontName = "Tahoma";
        sht.Range("A2:F2").Style.Font.FontSize = 12;
        sht.Range("A2:F2").Merge();
        //*****************
        string str = @"select * From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=1";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //CompanyName
            sht.Range("A3").Value = ds.Tables[0].Rows[0]["companyName"];
            sht.Range("A3:F3").Style.Font.FontName = "Tahoma";
            sht.Range("A3:F3").Style.Font.FontSize = 12;
            sht.Range("A3:F3").Style.Font.Bold = true;
            sht.Range("A3:F3").Merge();
            //Address
            sht.Range("A4").Value = ds.Tables[0].Rows[0]["compaddr1"];
            sht.Range("A4:F4").Style.Font.FontName = "Tahoma";
            sht.Range("A4:F4").Style.Font.FontSize = 12;
            sht.Range("A4:F4").Merge();
            //address2
            sht.Range("A5").Value = ds.Tables[0].Rows[0]["compaddr2"] + "," + ds.Tables[0].Rows[0]["compaddr3"];
            sht.Range("A5:F5").Style.Font.FontName = "Tahoma";
            sht.Range("A5:F5").Style.Font.FontSize = 12;
            sht.Range("A5:F5").Merge();
            //TiN No
            sht.Range("A6").Value = "TIN#" + ds.Tables[0].Rows[0]["TinNo"];
            sht.Range("A6:F6").Style.Font.FontName = "Tahoma";
            sht.Range("A6:F6").Style.Font.FontSize = 12;
            sht.Range("A6:F6").Style.Font.Bold = true;
            sht.Range("A6:F6").Merge();
            //**********INvoiceNodate
            sht.Range("G2").Value = "Invoice No./Date";
            sht.Range("G2:K2").Style.Font.FontName = "Tahoma";
            sht.Range("G2:K2").Style.Font.FontSize = 12;
            sht.Range("G2:K2").Merge();
            //value
            sht.Range("G3").Value = ds.Tables[0].Rows[0]["TInvoiceNo"] + " Dated " + ds.Tables[0].Rows[0]["InvoiceDate"];
            sht.Range("G3:K3").Style.Font.FontName = "Tahoma";
            sht.Range("G3:K3").Style.Font.FontSize = 12;
            sht.Range("G3:K3").Merge();
            sht.Range("G3", "K3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //*************IE Code
            sht.Range("G4").Value = "IE Code No.";
            sht.Range("G4:I4").Style.Font.FontName = "Tahoma";
            sht.Range("G4:I4").Style.Font.FontSize = 12;
            sht.Range("G4:I4").Merge();
            //value
            sht.Range("G5").Value = ds.Tables[0].Rows[0]["IEcode"];
            sht.Range("G5:I5").Style.Font.FontName = "Tahoma";
            sht.Range("G5:I5").Style.Font.FontSize = 12;
            sht.Range("G5:I5").Merge();
            sht.Range("G5", "I5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //***********GRI Form No
            sht.Range("J4").Value = "GRI Form No.";
            sht.Range("J4:K4").Style.Font.FontName = "Tahoma";
            sht.Range("J4:K4").Style.Font.FontSize = 12;
            sht.Range("J4:K4").Merge();
            // value
            sht.Range("J5").Value = "";
            sht.Range("J5:K5").Style.Font.FontName = "Tahoma";
            sht.Range("J5:K5").Style.Font.FontSize = 12;
            sht.Range("J5:K5").Merge();
            sht.Range("J5", "K5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //*************Buyer's Order No and Date
            sht.Range("G6").Value = "Buyer's Order No. / Date";
            sht.Range("G6:K7").Style.Font.FontName = "Tahoma";
            sht.Range("G6:K7").Style.Font.FontSize = 12;
            sht.Range("G6:K6").Merge();
            //value
            sht.Range("G7").Value = ds.Tables[0].Rows[0]["TorderNo"];
            sht.Range("G7:K7").Style.Font.FontName = "Tahoma";
            sht.Range("G7:K7").Style.Font.FontSize = 12;
            sht.Range("G7:K7").Merge();
            sht.Range("G7", "K7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //**************Other Reference(s)
            sht.Range("G8").Value = "Other Reference(s)";
            sht.Range("G9").Value = "CONSIGNMENT NO.:" + ds.Tables[0].Rows[0]["otherref"];
            sht.Range("G10").Value = "SUPPLIER NO.:";
            sht.Range("G8:G10").Style.Font.FontName = "Tahoma";
            sht.Range("G8:G10").Style.Font.FontSize = 12;
            sht.Range("G8:K8").Merge();
            sht.Range("G9:K9").Merge();
            sht.Range("G10:K10").Merge();
            //*************Consignee
            sht.Range("A11").Value = "Consignee";
            sht.Range("C11").Value = ds.Tables[0].Rows[0]["consignee_dt"];
            sht.Range("A11:B11").Style.Font.FontName = "Tahoma";
            sht.Range("A11:B11").Style.Font.FontSize = 12;

            sht.Range("C11:F11").Style.Font.FontName = "Tahoma";
            sht.Range("C11:F11").Style.Font.FontSize = 12;
            sht.Range("C11:F11").Style.Font.Bold = true;

            sht.Range("A11:B11").Merge();
            sht.Range("C11:F11").Merge();
            //value
            sht.Range("A12").Value = ds.Tables[0].Rows[0]["consignee"];
            sht.Range("A12:F12").Style.Font.FontName = "Tahoma";
            sht.Range("A12:F12").Style.Font.FontSize = 12;
            sht.Range("A12:F12").Style.Font.Bold = true;
            sht.Range("A12:F12").Merge();
            //**
            sht.Range("A13").Value = ds.Tables[0].Rows[0]["consignee_address"];
            sht.Range("A13:F16").Style.Font.FontName = "Tahoma";
            sht.Range("A13:F16").Style.Font.FontSize = 12;
            sht.Range("A13", "F16").Style.Alignment.WrapText = true;
            sht.Range("A13", "F16").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A13:F16").Merge();
            //***********Notify
            sht.Range("A17").Value = "Notify Party";
            sht.Range("C17").Value = ds.Tables[0].Rows[0]["Notifyparty_dt"];
            sht.Range("A17:B17").Style.Font.FontName = "Tahoma";
            sht.Range("A17:B17").Style.Font.FontSize = 12;

            sht.Range("A17:B17").Style.Font.SetUnderline();
            sht.Range("C17:F17").Style.Font.FontName = "Tahoma";
            sht.Range("C17:F17").Style.Font.FontSize = 12;
            sht.Range("C17:F17").Style.Font.Bold = true;

            sht.Range("A17:B17").Merge();
            sht.Range("C17:F17").Merge();
            //value
            sht.Range("A18").Value = ds.Tables[0].Rows[0]["Notifyparty"];
            sht.Range("A18:F18").Style.Font.FontName = "Tahoma";
            sht.Range("A18:F18").Style.Font.FontSize = 12;
            sht.Range("A18:F18").Style.Font.Bold = true;
            sht.Range("A18:F18").Merge();
            //
            sht.Range("A19").Value = ds.Tables[0].Rows[0]["notifyparty_address"];
            sht.Range("A19:F23").Style.Font.FontName = "Tahoma";
            sht.Range("A19:F23").Style.Font.FontSize = 12;
            sht.Range("A19", "F23").Style.Alignment.WrapText = true;
            sht.Range("A19", "F23").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A19:F23").Merge();
            //***********Receiver address
            sht.Range("G11").Value = "Receiver Address";
            sht.Range("G11:I11").Style.Font.FontName = "Tahoma";
            sht.Range("G11:I11").Style.Font.FontSize = 12;
            sht.Range("G11:I11").Merge();
            //values
            sht.Range("J11").Value = ds.Tables[0].Rows[0]["Notifyparty_dt"];
            sht.Range("J11:K11").Style.Font.FontName = "Tahoma";
            sht.Range("J11:K11").Style.Font.FontSize = 12;
            sht.Range("J11:K11").Style.Font.Bold = true;
            sht.Range("J11:K11").Merge();
            //****** 1.
            sht.Range("G12").Value = "1. " + ds.Tables[0].Rows[0]["Receiver"];
            sht.Range("G12:K12").Style.Font.FontName = "Tahoma";
            sht.Range("G12:K12").Style.Font.FontSize = 12;
            sht.Range("G12:K12").Style.Font.Bold = true;
            sht.Range("G12:K12").Merge();
            //*
            sht.Range("G13").Value = ds.Tables[0].Rows[0]["Receiver_address"];
            sht.Range("G13:K15").Style.Font.FontName = "Tahoma";
            sht.Range("G13:K15").Style.Font.FontSize = 12;
            sht.Range("G13:K15").Style.Alignment.WrapText = true;
            sht.Range("G13:K15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("G13:K15").Merge();
            //****** 2.
            sht.Range("G16").Value = "2.Paying Agent : " + ds.Tables[0].Rows[0]["payingagent"];
            sht.Range("G16:K16").Style.Font.FontName = "Tahoma";
            sht.Range("G16:K16").Style.Font.FontSize = 12;
            sht.Range("G16:K16").Style.Font.Bold = true;
            sht.Range("G16:K16").Merge();
            //*
            sht.Range("I17").Value = ds.Tables[0].Rows[0]["Receiver_address"];
            sht.Range("I17:K19").Style.Font.FontName = "Tahoma";
            sht.Range("I17:K19").Style.Font.FontSize = 12;
            sht.Range("I17:K19").Style.Alignment.WrapText = true;
            sht.Range("I17:K19").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("I17:K19").Merge();
            //*******3.
            sht.Range("G20").Value = "Buyer (If other than Consignee)";
            sht.Range("G20:K20").Style.Font.FontName = "Tahoma";
            sht.Range("G20:K20").Style.Font.FontSize = 12;
            sht.Range("G20:K20").Merge();

            sht.Range("G21").Value = "3." + ds.Tables[0].Rows[0]["buyer_otherthanconsignee"];
            sht.Range("G21:K21").Style.Font.FontName = "Tahoma";
            sht.Range("G21:K21").Style.Font.FontSize = 12;
            sht.Range("G21:K21").Merge();
            //*
            sht.Range("G22").Value = ds.Tables[0].Rows[0]["otherthanconsignee_address"];
            sht.Range("G22:K24").Style.Font.FontName = "Tahoma";
            sht.Range("G22:K24").Style.Font.FontSize = 12;
            sht.Range("G22:K24").Style.Alignment.WrapText = true;
            sht.Range("G22:K24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("G22:K24").Merge();
            //***********Pre-carriage By
            sht.Range("A24").Value = "Pre-Carriage By";
            sht.Range("A24:C24").Style.Font.FontName = "Tahoma";
            sht.Range("A24:C24").Style.Font.FontSize = 12;
            sht.Range("A24:C24").Merge();
            //value
            sht.Range("A25").Value = ds.Tables[0].Rows[0]["Pre_carriageby"];
            sht.Range("A25:C25").Style.Font.FontName = "Tahoma";
            sht.Range("A25:C25").Style.Font.FontSize = 12;
            sht.Range("A25:C25").Merge();
            sht.Range("A25:C25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //************Place of Receipt by Pre-Carrier
            sht.Range("D24").Value = "Place of Receipt by Pre-Carrier";
            sht.Range("D24:F24").Style.Font.FontName = "Tahoma";
            sht.Range("D24:F24").Style.Font.FontSize = 12;
            sht.Range("D24:F24").Merge();
            //value
            sht.Range("D25").Value = ds.Tables[0].Rows[0]["receiptby"];
            sht.Range("D25:F25").Style.Font.FontName = "Tahoma";
            sht.Range("D25:F25").Style.Font.FontSize = 12;
            sht.Range("D25:F25").Merge();
            sht.Range("D25:F25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //************Vessel/Flight No
            sht.Range("A26").Value = "Vessel/Flight No";
            sht.Range("A26:C26").Style.Font.FontName = "Tahoma";
            sht.Range("A26:C26").Style.Font.FontSize = 12;
            sht.Range("A26:C26").Merge();
            //value
            sht.Range("A27").Value = ds.Tables[0].Rows[0]["vessel_flightno"];
            sht.Range("A27:C27").Style.Font.FontName = "Tahoma";
            sht.Range("A27:C27").Style.Font.FontSize = 12;
            sht.Range("A27:C27").Merge();
            sht.Range("A27:C27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //************Port of Loading
            sht.Range("D26").Value = "Port of Loading";
            sht.Range("D26:F26").Style.Font.FontName = "Tahoma";
            sht.Range("D26:F26").Style.Font.FontSize = 12;
            sht.Range("D26:F26").Merge();
            //value
            sht.Range("D27").Value = ds.Tables[0].Rows[0]["portload"];
            sht.Range("D27:F27").Style.Font.FontName = "Tahoma";
            sht.Range("D27:F27").Style.Font.FontSize = 12;
            sht.Range("D27:F27").Merge();
            sht.Range("D27:F27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //************Port of Discharge
            sht.Range("A28").Value = "Port of Discharge";
            sht.Range("A28:C28").Style.Font.FontName = "Tahoma";
            sht.Range("A28:C28").Style.Font.FontSize = 12;
            sht.Range("A28:C28").Merge();
            //value
            sht.Range("A29").Value = ds.Tables[0].Rows[0]["portunload"];
            sht.Range("A29:C29").Style.Font.FontName = "Tahoma";
            sht.Range("A29:C29").Style.Font.FontSize = 12;
            sht.Range("A29:C29").Merge();
            sht.Range("A29:C29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //************Final Destination
            sht.Range("D28").Value = "Final Destination";
            sht.Range("D28:F28").Style.Font.FontName = "Tahoma";
            sht.Range("D28:F28").Style.Font.FontSize = 12;
            sht.Range("D28:F28").Merge();
            //value
            sht.Range("D29").Value = ds.Tables[0].Rows[0]["Destinationadd"];
            sht.Range("D29:F29").Style.Font.FontName = "Tahoma";
            sht.Range("D29:F29").Style.Font.FontSize = 12;
            sht.Range("D29:F29").Merge();
            sht.Range("D29:F29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //************Country of Origin of Goods
            sht.Range("G25").Value = "Country of Origin of Goods";
            sht.Range("G25:I25").Style.Font.FontName = "Tahoma";
            sht.Range("G25:I25").Style.Font.FontSize = 12;
            sht.Range("G25:I25").Merge();
            //value
            sht.Range("G26").Value = ds.Tables[0].Rows[0]["countryoforigin"];
            sht.Range("G26:I26").Style.Font.FontName = "Tahoma";
            sht.Range("G26:I26").Style.Font.FontSize = 12;
            sht.Range("G26:I26").Merge();
            sht.Range("G26:I26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //************Country of Final Destination
            sht.Range("J25").Value = "Country of Final Destination";
            sht.Range("J25:K25").Style.Font.FontName = "Tahoma";
            sht.Range("J25:K25").Style.Font.FontSize = 12;
            sht.Range("J25:K25").Merge();
            //value
            sht.Range("J26").Value = ds.Tables[0].Rows[0]["countryoforigin"];
            sht.Range("J26:K26").Style.Font.FontName = "Tahoma";
            sht.Range("J26:K26").Style.Font.FontSize = 12;
            sht.Range("J26:K26").Style.NumberFormat.Format = "@";
            sht.Range("J26:K26").Merge();
            sht.Range("J26:K26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //***********Terms of Delivery and Payment
            sht.Range("G27").Value = "Terms of Delivery and Payment";
            sht.Range("G27:K27").Style.Font.FontName = "Tahoma";
            sht.Range("G27:K27").Style.Font.FontSize = 12;
            sht.Range("G27:K27").Merge();
            //value
            sht.Range("H28").Value = ds.Tables[0].Rows[0]["Deliveryterm"];
            sht.Range("H28:K29").Style.Font.FontName = "Tahoma";
            sht.Range("H28:K29").Style.Font.FontSize = 12;
            sht.Range("H28:K29").Style.NumberFormat.Format = "@";
            sht.Range("H28:K29").Style.Alignment.WrapText = true;
            sht.Range("H28:K29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("H28:K29").Merge();
            //*****************Nos
            sht.Range("A30").Value = "Nos.";
            sht.Range("B30").Value = "No and kind of Packages";
            sht.Range("B30:C30").Merge();
            sht.Range("E30").Value = "Description of Goods";
            sht.Range("E30:H30").Merge();
            sht.Range("I30").Value = "Quantity";
            sht.Range("J30").Value = "Rate";
            sht.Range("k30").Value = "Amount";
            sht.Range("A30:K30").Style.Font.FontName = "Tahoma";
            sht.Range("A30:K30").Style.Font.FontSize = 12;
            sht.Range("A30:K30").Style.NumberFormat.Format = "@";
            sht.Range("K30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            //values
            sht.Range("A31:H31").Merge();
            sht.Range("A32:H36").Style.Font.FontName = "Tahoma";
            sht.Range("A32:H36").Style.Font.FontSize = 12;
            sht.Range("A32:H36").Style.NumberFormat.Format = "@";

            sht.Range("A32").Value = ds.Tables[0].Compute("Min(MinrollNo)", "") + "/" + ds.Tables[0].Compute("Max(Maxrollno)", "");

            sht.Range("B32").Value = ds.Tables[0].Rows[0]["Noofrolls"] + " Packages";
            sht.Range("B32:C32").Merge();
            sht.Range("D32").Value = ds.Tables[0].Rows[0]["descriptionofgoods"];
            sht.Range("D32:H36").Style.Alignment.WrapText = true;
            sht.Range("D32:H36").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("D32:H36").Merge();
            sht.Range("A37:H37").Merge();
            //********Details
            sht.Range("A38").Value = "P.O.NO.";
            sht.Range("A38:K38").Style.Font.FontName = "Tahoma";
            sht.Range("A38:K38").Style.Font.FontSize = 12;
            sht.Range("A38:K38").Style.Font.Bold = true;
            sht.Range("A38:K38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            sht.Range("B38").Value = "ARICLE NAME";
            sht.Range("B38:C38").Merge();
            sht.Range("D38").Value = "ART.NO.";
            sht.Range("D38:E38").Merge();
            sht.Range("F38").Value = "COLOR";
            sht.Range("G38").Value = "SIZE(CM)";
            sht.Range("H38").Value = "AREA SQ.MTR.";
            sht.Range("I38").Value = "QTY";
            sht.Range("J38").Value = "FCA/" + ds.Tables[0].Rows[0]["CIF"];
            sht.Range("K38").Value = "FCA/" + ds.Tables[0].Rows[0]["CIF"];
            sht.Range("K38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            //***********generate Loop
            i = 39;
            for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
            {
                sht.Range("A" + i + ":K" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":K" + i).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //pono
                sht.Range("A" + i).Value = ds.Tables[0].Rows[ii]["Pono"];
                //Article Name
                sht.Range("B" + i).Value = ds.Tables[0].Rows[ii]["Design"];
                sht.Range("B" + i, "C" + i).Merge();
                //Art No.
                sht.Range("D" + i).Value = ds.Tables[0].Rows[ii]["articleno"];
                sht.Range("D" + i, "E" + i).Merge();
                //Colour
                sht.Range("F" + i).Value = ds.Tables[0].Rows[ii]["Color"];
                //Size
                sht.Range("G" + i).Value = ds.Tables[0].Rows[ii]["width"] + "x" + ds.Tables[0].Rows[ii]["Length"];
                //Area
                sht.Range("H" + i).Value = ds.Tables[0].Rows[ii]["Area"];
                Area = Decimal.Parse(Area.ToString()) + Decimal.Parse(ds.Tables[0].Rows[ii]["Area"].ToString());
                //QTY
                sht.Range("I" + i).Value = ds.Tables[0].Rows[ii]["Pcs"];
                Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[ii]["Pcs"]);
                //
                var _with27 = sht.Range("I" + i);
                _with27.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with27.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //rate
                sht.Range("J" + i).Value = ds.Tables[0].Rows[ii]["Price"];

                var _with28 = sht.Range("J" + i);
                _with28.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with28.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //Amount
                sht.Range("K" + i).Value = ds.Tables[0].Rows[ii]["amount"];
                Amount = Decimal.Parse(Amount.ToString()) + Decimal.Parse(ds.Tables[0].Rows[ii]["amount"].ToString());
                //
                sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("H" + i + ":K" + i).Style.NumberFormat.Format = "#,###0.00";
                sht.Range("I" + i).Style.NumberFormat.Format = "@";
                //border
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                i = i + 1;
            }
            var _with29 = sht.Range("I" + i + ":I" + (i + 2));
            _with29.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            _with29.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("A" + i + ":A" + (i + 2)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("K" + i + ":K" + (i + 2)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //
            var _with30 = sht.Range("J" + i + ":J" + (i + 2));
            _with30.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            _with30.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //End Of loop
            i = i + 2;
            //Total            
            sht.Range("G" + i + ":I" + i).Style.Font.FontName = "Tahoma";
            sht.Range("G" + i + ":I" + i).Style.Font.FontSize = 12;
            sht.Range("G" + i + ":I" + i).Style.NumberFormat.Format = "@";
            sht.Range("G" + i + ":I" + i).Style.Font.Bold = true;
            sht.Range("G" + i + ":I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            sht.Range("G" + i).Value = "Total:";
            sht.Range("H" + i).Value = Area;
            sht.Range("I" + i).Value = Pcs;
            sht.Range("A" + i + ":F" + i).Merge();
            sht.Range("H" + i).Style.NumberFormat.Format = "#,###0.00";
            //border
            var _with23 = sht.Range("H" + i);
            _with23.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            _with23.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //
            var _with24 = sht.Range("I" + i);
            _with24.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            _with24.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            _with24.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            _with24.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //Notify Party
            i = i + 1;
            sht.Range("A" + i).Value = "Notify Party:";
            sht.Range("A" + i).Style.Font.FontName = "Tahoma";
            sht.Range("A" + i).Style.Font.FontSize = 12;
            sht.Range("A" + i).Style.NumberFormat.Format = "@";
            sht.Range("A" + i).Style.Font.SetUnderline();
            //
            sht.Range("B" + i).Value = ds.Tables[0].Rows[0]["Notifyparty2_dt"];
            sht.Range("B" + i + ":C" + i).Style.Font.FontName = "Tahoma";
            sht.Range("B" + i + ":C" + i).Style.Font.FontSize = 12;
            sht.Range("B" + i + ":C" + i).Style.NumberFormat.Format = "@";
            sht.Range("B" + i + ":C" + i).Style.Font.Bold = true;
            sht.Range("B" + i + ":C" + i).Merge();
            var _with31 = sht.Range("I" + i + ":I" + (i + 4));
            _with31.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            _with31.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("A" + i + ":A" + (i + 4)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("K" + i + ":K" + (i + 4)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //
            var _with32 = sht.Range("J" + i + ":J" + (i + 4));
            _with32.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            _with32.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //
            i = i + 1;
            sht.Range("B" + i).Value = ds.Tables[0].Rows[0]["Notifyparty2_address"];
            sht.Range("B" + i + ":E" + (i + 3)).Style.Font.FontName = "Tahoma";
            sht.Range("B" + i + ":E" + (i + 3)).Style.Font.FontSize = 12;
            sht.Range("B" + i + ":E" + (i + 3)).Style.NumberFormat.Format = "@";
            sht.Range("B" + i + ":E" + (i + 3)).Style.Alignment.WrapText = true;
            sht.Range("B" + i + ":E" + (i + 3)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("B" + i + ":E" + (i + 3)).Merge();
            //border
            var _with25 = sht.Range("A" + (i - 1) + ":E" + (i - 1));
            _with25.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("E" + (i - 1) + ":E" + (i + 3)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + (i + 3) + ":E" + (i + 3)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //**********Article Loop          
            //
            i = i + 5;
            sht.Range("A" + i).Value = "ARTICLE NAME";
            sht.Range("A" + i + ":B" + i).Merge();
            sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Tahoma";
            sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 12;
            sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";
            sht.Range("A" + i + ":B" + i).Style.Font.Bold = true;
            //
            sht.Range("D" + i).Value = "CONTENT";
            sht.Range("D" + i + ":F" + i).Merge();
            sht.Range("D" + i + ":F" + i).Style.Font.FontName = "Tahoma";
            sht.Range("D" + i + ":F" + i).Style.Font.FontSize = 12;
            sht.Range("D" + i + ":F" + i).Style.NumberFormat.Format = "@";
            sht.Range("D" + i + ":F" + i).Style.Font.Bold = true;
            //
            sht.Range("G" + i).Value = "RATE/SQM";
            sht.Range("G" + i).Style.Font.FontName = "Tahoma";
            sht.Range("G" + i).Style.Font.FontSize = 12;
            sht.Range("G" + i).Style.NumberFormat.Format = "@";
            sht.Range("G" + i).Style.Font.Bold = true;
            //Border
            var _with33 = sht.Range("I" + (i - 1) + ":I" + i);
            _with33.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            _with33.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + (i - 1) + ":A" + (i)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("K" + (i - 1) + ":K" + (i)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //
            var _with34 = sht.Range("J" + (i - 1) + ":J" + i);
            _with34.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            _with34.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            //********
            i = i + 1;
            sht.Range("A" + i + ":A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("K" + i + ":K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            str = "Select Design as ArticleNo,contents,Round(sum(amount)/sum(Area),2) as Rate From V_PackingDetailsIkea Where PackingId=1 group by Design,contents";
            DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                {
                    sht.Range("A" + i).Value = ds1.Tables[0].Rows[j]["articleno"];
                    sht.Range("A" + i + ":B" + i).Merge();
                    sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 12;
                    sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";
                    //
                    sht.Range("D" + i).Value = ds1.Tables[0].Rows[j]["contents"];
                    sht.Range("D" + i + ":F" + i).Merge();
                    sht.Range("D" + i + ":F" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("D" + i + ":F" + i).Style.Font.FontSize = 12;
                    sht.Range("D" + i + ":F" + i).Style.NumberFormat.Format = "@";
                    //rate
                    sht.Range("G" + i).Value = ds1.Tables[0].Rows[j]["rate"];
                    sht.Range("G" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("G" + i).Style.Font.FontSize = 12;
                    sht.Range("G" + i).Style.NumberFormat.Format = "@";
                    sht.Range("G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //
                    var _with35 = sht.Range("I" + (i) + ":I" + i);
                    _with35.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    _with35.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //
                    var _with36 = sht.Range("J" + (i) + ":J" + i);
                    _with36.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    _with36.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    sht.Range("A" + i + ":A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("K" + i + ":K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    i = i + 1;
                }

            }
            //*******amount in words
            sht.Range("A" + i).Value = "Amount in Words";
            sht.Range("A" + i + ":B" + i).Merge();
            sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Tahoma";
            sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 12;
            sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";

            string amountinwords = "";

            amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(Amount));
            string val = "", paise = "";
            if (Amount.ToString().IndexOf('.') > 0)
            {
                val = Amount.ToString().Split('.')[1];
                if (Convert.ToInt16(val)>0)
                {
                    paise = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val));    
                }                
            }
            amountinwords = ds.Tables[0].Rows[0]["CIF"] + " " + amountinwords + " " + ds.Tables[0].Rows[0]["Currencytypeps"] + " " + paise + " Only";
            sht.Range("C" + i).Value = amountinwords.ToUpper();
            sht.Range("C" + i + ":I" + (i + 1)).Style.Font.FontName = "Tahoma";
            sht.Range("C" + i + ":I" + (i + 1)).Style.Font.FontSize = 12;
            sht.Range("C" + i + ":I" + (i + 1)).Style.NumberFormat.Format = "@";
            sht.Range("C" + i + ":I" + (i + 1)).Style.Font.Bold = true;
            sht.Range("C" + i + ":I" + (i + 1)).Style.Alignment.WrapText = true;
            sht.Range("C" + i + ":I" + (i + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("C" + i + ":I" + (i + 1)).Merge();
            //*****Total
            sht.Range("J" + i).Value = "Total";
            sht.Range("J" + i).Style.Font.FontName = "Tahoma";
            sht.Range("J" + i).Style.Font.FontSize = 12;
            sht.Range("J" + i).Style.NumberFormat.Format = "@";
            sht.Range("J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //Amount
            sht.Range("K" + i).Value = Amount;
            sht.Range("K" + i).Style.Font.FontName = "Tahoma";
            sht.Range("K" + i).Style.Font.FontSize = 12;
            sht.Range("K" + i).Style.NumberFormat.Format = "@";
            sht.Range("K" + i).Style.Font.Bold = true;
            sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("K" + i).Style.NumberFormat.Format = "#,###0.00";
            //
            sht.Range("A" + i + ":A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("K" + i + ":K" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

            var _with37 = sht.Range("K" + i);
            _with37.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            _with37.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            _with37.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            _with37.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //
            //border
            var _with26 = sht.Range("A" + i + ":K" + i);
            _with26.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //value
            //**********Total
            i = i + 2;
            //i = 63;
            sht.Range("A" + i).Value = "TOTAL ROLLS   :";
            sht.Range("A" + (i + 1)).Value = "TOTAL PCS     :";
            sht.Range("A" + (i + 2)).Value = "TOTAL GR. WT  :";
            sht.Range("A" + (i + 3)).Value = "TOTAL NT. WT  :";
            sht.Range("A" + (i + 4)).Value = "TOTAL SQ.MTR  :";
            sht.Range("A" + (i + 5)).Value = "TOTAL VOLUME  :";
            sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontSize = 12;
            sht.Range("A" + i + ":A" + (i + 5)).Style.NumberFormat.Format = "@";
            sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontName = "Courier New";
            //value            
            sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontName = "Tahoma";
            sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontSize = 12;
            sht.Range("C" + i + ":C" + (i + 5)).Style.NumberFormat.Format = "@";
            sht.Range("C" + i).Value = ds.Tables[0].Rows[0]["Noofrolls"];
            sht.Range("C" + (i + 1)).Value = Pcs;
            sht.Range("C" + (i + 2)).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["grosswt"]) + " Kgs.";
            sht.Range("C" + (i + 3)).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["netwt"]) + " Kgs.";
            sht.Range("C" + (i + 4)).Value = String.Format("{0:#,0.00}", Area) + " Sq.Mtr";
            sht.Range("C" + (i + 5)).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["Volume"]) + " CBM";
            //******Central Excise
            sht.Range("F" + i).Value = "Central Excise No. AADFE8796LEM001";
            sht.Range("F" + (i + 1)).Value = "Range: Bhadohi";
            sht.Range("F" + (i + 2)).Value = "Division: Allahabad-I";
            sht.Range("F" + (i + 3)).Value = "Commissionerate: Allahabad";
            sht.Range("F" + (i + 4)).Value = "Tariff Heading# 570201";
            sht.Range("F" + i + ":H" + (i + 4)).Style.Font.FontName = "Tahoma";
            sht.Range("F" + i + ":H" + (i + 4)).Style.Font.FontSize = 12;
            sht.Range("F" + i + ":H" + (i + 4)).Style.NumberFormat.Format = "@";
            sht.Range("F" + i + ":H" + (i + 4)).Style.Font.Bold = true;
            sht.Range("F" + i + ":H" + i).Merge();
            sht.Range("F" + (i + 1) + ":H" + (i + 1)).Merge();
            sht.Range("F" + (i + 2) + ":H" + (i + 2)).Merge();
            sht.Range("F" + (i + 3) + ":H" + (i + 3)).Merge();
            sht.Range("F" + (i + 4) + ":H" + (i + 4)).Merge();
            sht.Range("A" + i + ":A" + (i + 5)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("K" + i + ":K" + (i + 5)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //***********Sig and date
            i = i + 6;
            //i = 70;
            sht.Range("A" + i + ":H" + i).Merge();
            sht.Range("I" + i).Value = "Signature/Date";
            sht.Range("I" + i).Style.Font.FontName = "Tahoma";
            sht.Range("I" + i).Style.Font.FontSize = 12;
            sht.Range("I" + i).Style.NumberFormat.Format = "@";

            sht.Range("K" + i).Value = "For " + ds.Tables[0].Rows[0]["companyname"];
            sht.Range("J" + i + ":K" + i).Style.Font.FontName = "Tahoma";
            sht.Range("J" + i + ":K" + i).Style.Font.FontSize = 9;
            sht.Range("J" + i + ":K" + i).Style.NumberFormat.Format = "@";
            sht.Range("J" + i + ":K" + i).Style.Font.Bold = true;
            sht.Range("J" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            //sht.Range("J" + i + ":K" + i).Merge();
            var _with38 = sht.Range("I" + i + ":K" + i);
            _with38.Style.Border.TopBorder = XLBorderStyleValues.Thin;

            sht.Range("I" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

            sht.Range("A" + i + ":A" + (i + 3)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("K" + i + ":K" + (i + 3)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //**************Declaration
            i = i + 1;
            sht.Range("I" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //i = 71;
            sht.Range("A" + i).Value = "Declaration";
            sht.Range("A" + i + ":B" + i).Merge();
            sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Tahoma";
            sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 12;
            sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";
            sht.Range("A" + i + ":B" + i).Style.Font.Bold = true;
            sht.Range("A" + i + ":B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //value
            i = i + 1;
            sht.Range("I" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i).Value = "We declare that this invoice shows the actual price of the";
            sht.Range("A" + i + ":E" + i).Merge();
            sht.Range("A" + i + ":E" + i).Style.Font.FontName = "Tahoma";
            sht.Range("A" + i + ":E" + i).Style.Font.FontSize = 12;
            sht.Range("A" + i + ":E" + i).Style.NumberFormat.Format = "@";
            i = i + 1;
            sht.Range("A" + i).Value = "goods described and that all particulars are true and correct";
            sht.Range("A" + i + ":E" + i).Merge();
            sht.Range("A" + i + ":E" + i).Style.Font.FontName = "Tahoma";
            sht.Range("A" + i + ":E" + i).Style.Font.FontSize = 12;
            sht.Range("A" + i + ":E" + i).Style.NumberFormat.Format = "@";

            //**********
            sht.Range("I" + i).Value = System.DateTime.Now.ToString("dd/MMM/yyyy");
            sht.Range("I" + i).Style.Font.FontName = "Tahoma";
            sht.Range("I" + i).Style.Font.FontSize = 11;
            sht.Range("I" + i).Style.NumberFormat.Format = "@";
            sht.Range("I" + i).Style.NumberFormat.Format = "dd/MMM/yyyy";
            sht.Range("K" + i).Value = "Auth Sign";
            sht.Range("K" + i).Style.Font.FontName = "Tahoma";
            sht.Range("K" + i).Style.Font.FontSize = 12;
            sht.Range("K" + i).Style.NumberFormat.Format = "@";
            sht.Range("K" + i).Style.Font.Bold = true;
            sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //
            var _with39 = sht.Range("I" + i);
            _with39.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + i + ":K" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //

            //*****************Set Borders
            //var _with1 = sht.Range("A2:K" + i);
            //_with1.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //_with1.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //_with1.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //_with1.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //************************
            var _with2 = sht.Range("A10:K10");
            _with2.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //
            var _with3 = sht.Range("G2:K3");
            _with3.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            _with3.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            var _with41 = sht.Range("G2:K2");
            _with41.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            var _with40 = sht.Range("G3:K3");
            _with40.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //
            var _with4 = sht.Range("G4:I5");
            _with4.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            _with4.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            var _with42 = sht.Range("G4:I4");
            _with42.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            var _with43 = sht.Range("G5:I5");
            _with43.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            //
            var _with5 = sht.Range("J4:K4");
            _with5.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            var _with44 = sht.Range("J4:K5");
            _with44.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            var _with45 = sht.Range("J5:K5");
            _with45.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //
            var _with6 = sht.Range("G6:K7");
            _with6.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            _with6.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            var _with46 = sht.Range("G7:K7");
            _with46.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //
            var _with7 = sht.Range("G8:K10");
            _with7.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            _with7.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            var _with47 = sht.Range("G11:K24");
            _with47.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("G11:I11").Style.Border.RightBorder = XLBorderStyleValues.None;

            //
            var _with8 = sht.Range("A23:F23");
            _with8.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            var _with48 = sht.Range("A11:F23");
            _with48.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            sht.Range("A11:B11").Style.Border.RightBorder = XLBorderStyleValues.None;
            sht.Range("A17:B17").Style.Border.RightBorder = XLBorderStyleValues.None;
            sht.Range("G11:H11").Style.Border.RightBorder = XLBorderStyleValues.None;
            sht.Range("G17:H19").Style.Border.RightBorder = XLBorderStyleValues.None;

            //
            var _with9 = sht.Range("A24:C25");
            _with9.Style.Border.LeftBorder = XLBorderStyleValues.Thin;            
            _with9.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //sht.Range("A24:C24").Style.Border.BottomBorder = XLBorderStyleValues.None;

            sht.Range("A25:C25").Style.Border.BottomBorder = XLBorderStyleValues.Thin;            
            
            sht.Range("G25:I25").Style.Border.BottomBorder = XLBorderStyleValues.None;

            sht.Range("J25:K25").Style.Border.BottomBorder = XLBorderStyleValues.None;
            //
            var _with10 = sht.Range("D24:F25");            
            _with10.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            _with10.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("D24:F24").Style.Border.BottomBorder = XLBorderStyleValues.None;
            //
            var _with11 = sht.Range("A26:C27");
            _with11.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            _with11.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            _with11.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("A26:C26").Style.Border.BottomBorder = XLBorderStyleValues.None;
            //
            var _with12 = sht.Range("D26:F27");
            _with12.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            _with12.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("D26:F26").Style.Border.BottomBorder = XLBorderStyleValues.None;
            //
            var _with13 = sht.Range("A28:C29");
            _with13.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            _with13.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("A28:C28").Style.Border.BottomBorder = XLBorderStyleValues.None;
            sht.Range("A29:C29").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //
            var _with14 = sht.Range("D28:F29");
            _with14.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            _with14.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("D28:G28").Style.Border.BottomBorder = XLBorderStyleValues.None;
            sht.Range("G28").Style.Border.BottomBorder = XLBorderStyleValues.None;
            
            //
            var _with15 = sht.Range("G25:I26");
            _with15.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            _with15.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("G25:I25").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("G26:I26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //
            var _with16 = sht.Range("J25:K26");
            _with16.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("J25:K25").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("J26:K26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //
            var _with17 = sht.Range("G27:K29");
            _with17.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            _with17.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("G29:K29").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("G28:G29").Style.Border.RightBorder = XLBorderStyleValues.None;
            sht.Range("H28:K29").Style.Border.LeftBorder = XLBorderStyleValues.None;
            //
            var _with18 = sht.Range("I30:I37");
            _with18.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            _with18.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //
            var _with19 = sht.Range("J30:J37");
            _with19.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            _with19.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //
            var _with20 = sht.Range("A38:K38");
            _with20.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            _with20.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //
            var _with21 = sht.Range("I38:I38");
            _with21.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            _with21.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //
            var _with22 = sht.Range("J38:J38");
            _with22.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            _with22.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            //
            sht.Range("A30:A37").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("K30:K37").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A38:A38").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("K38:K38").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A1:K1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A2:A23").Style.Border.LeftBorder = XLBorderStyleValues.Thin;

        }
        else
        {

        }
        string Path = Server.MapPath("~/InvoiceExcel/123.xlsx");
        xapp.SaveAs(Path);
        //Download File
        //Response.Clear();
        //Response.ContentType = "application/vnd.ms-excel";
        //Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        //Response.WriteFile(Path);
        //Response.End();

    }
    protected void Exporttoexcel_1()
    {
        //lblmsg.Text = "Wait For Excel File to Download";
        int i = 0, Pcs = 0;
        Decimal Area = 0, Amount = 0;
        if (!Directory.Exists(Server.MapPath("~/InvoiceExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/InvoiceExcel/"));
        }
        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("Invoice1");
        try
        {
            //*****Styles                    
            #region

            //***************Normal Bold
            //ExcelStyle NormalStyleWithBold = sht.Application.ActiveWorkbook.Styles.Add("NormalStyleWithBold");
            //var With2 = NormalStyleWithBold;
            //With2.Font.Size = "12";
            //With2.Font.Name = "Tahoma";
            //With2.NumberFormat = "@";
            //With2.Font.Bold = true;
            ////***************Normal Bold centre
            //ExcelStyle NormalStyleWithBold_Centre = sht.Application.ActiveWorkbook.Styles.Add("NormalStyleWithBold_Centre");
            //var With3 = NormalStyleWithBold_Centre;
            //With3.Font.Size = "12";
            //With3.Font.Name = "Tahoma";
            //With3.NumberFormat = "@";
            //With3.Font.Bold = true;
            //With3.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            ////***************Normal  centre
            //ExcelStyle NormalStyleWith_Centre = sht.Application.ActiveWorkbook.Styles.Add("NormalStyleWith_Centre");
            //var With4 = NormalStyleWith_Centre;
            //With4.Font.Size = "12";
            //With4.Font.Name = "Tahoma";
            //With4.NumberFormat = "@";
            //With4.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            ////***********Header        


            //string str = @"select * From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue;
            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            //if (ds.Tables[0].Rows.Count > 0)
            //{






            //    //**********************Details
            //    //********Hader





            //    //border
            //    //




            //  go to dynamic list
            //}
            //else
            //{

            //}
            #endregion
            //******************

            string Fileextension = "xlsx";
            string filename = "InvoiceExcel123." + Fileextension;//" + DateTime.Now.Ticks + "." + Fileextension + "";
            #region
            //switch (xapp.Version.ToString())
            //{
            //    case "11.0":
            //        Fileextension = "xls";
            //        break;
            //    case "12.0":
            //        Fileextension = "xlsx";
            //        break;
            //    default:
            //        Fileextension = "xlsx";
            //        break;
            //}
            #endregion

            string Path = Server.MapPath("~/InvoiceExcel/" + filename);
            xapp.SaveAs(Path);
            //Download File
            //Response.Clear();
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            //Response.WriteFile(Path);
            //Response.End();
            ////*****
            //File.Delete(Path);
        }
        catch (Exception ex)
        {
            xapp.Dispose();
        }
        //lblmsg.Text = "Invoice Excel Format downloaded successfully.";
    }
}
