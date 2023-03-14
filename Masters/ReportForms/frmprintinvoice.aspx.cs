using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ClosedXML.Excel;
using System.IO;
using System.Text.RegularExpressions;

public partial class Masters_ReportForms_frmprintinvoice : System.Web.UI.Page
{
    public static string Export = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select Distinct CI.CompanyId,CI.Companyname From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + " Order by Companyname", false, "--Select--");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            CompanySelectedChange();
            FillInvoiceTypeOther();
            //switch (variable.ReportWithpdf)
            //{
            //    case "1":
            //        chkexport.Visible = true;
            //        Export = "N";
            //        break;
            //    default:
            //        chkexport.Visible = false;
            //        Export = "Y";
            //        break;
            //}
        }

    }
    private void CompanySelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDInvoiceYear, "Select Year,Session From Session Order By Year Desc", true, "--Select--");
        if (DDInvoiceYear.Items.Count > 0)
        {
            DDInvoiceYear.SelectedIndex = 1;
            FillInvoiceNo();
            // Fill_Grid();

            //PrintTypeSelectedChange();
        }
    }
    protected void FillInvoiceTypeOther()
    {
        DDPrintType.Items.Clear();
        DDPrintType.Items.Add(new ListItem("Type1", "1"));
        DDPrintType.Items.Add(new ListItem("Type2", "2"));
        DDPrintType.Items.Add(new ListItem("Type3", "3"));

    }
    protected void FillPackingType()
    {
        DDPrintType.Items.Clear();
        DDPrintType.Items.Add(new ListItem("Type1", "1"));
        DDPrintType.Items.Add(new ListItem("Type2", "2"));
        DDPrintType.Items.Add(new ListItem("Type3", "3"));
    }
    protected void InvoicePath()
    {
        string str = @"select * From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\Rptinvoicetype1_IK.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\Rptinvoicetype1_IK.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            if (chkexport.Checked == true)
            {
                Export = "Y";
            }
            else
            {
                switch (variable.ReportWithpdf)
                {
                    case "1":
                        Export = "N";
                        break;
                    default:
                        Export = "Y";
                        break;
                }
            }
            stb.Append("window.open('../../ViewReport.aspx?Export=" + Export + "', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            //stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            switch (DDDocumentType.SelectedValue)
            {
                case "1":
                    //InvoicePath();
                    switch (DDPrintType.SelectedValue)
                    {
                        case "1":
                            if (Session["VarCompanyNo"].ToString() == "21")
                            {
                             //   Exporttoexcel_Kaysons_1();
                                Exporttoexcel_Kaysons_new();
                            }
                            else
                            {
                                Exporttoexcel_1();
                            }
                            break;
                        case "2":
                            Exporttoexcel_2();
                            break;
                        case "3":
                            if (Session["VarCompanyNo"].ToString() == "21")
                            {
                                Exporttoexcel_Kaysons_4();
                            }
                            break;
                        default:
                            break;
                    }

                    break;
                case "2":
                    switch (DDPrintType.SelectedValue)
                    {
                        case "1":
                            if (Session["VarCompanyNo"].ToString() == "21")
                            {
                              //  Exporttoexcel_Packing_Kaysons();
                                Exporttoexcel_Packing_Kaysons_new();
                            }
                            else if (Session["VarCompanyNo"].ToString() == "14")
                            {
                                Exporttoexcel_Packing_Eastern_Ikea();
                            }
                            else
                            {
                                Exporttoexcel_Packing();
                            }

                            break;
                        case "2":
                            Exporttoexcel_Packing_type2();
                            break;
                        case "3":
                            if (Session["VarCompanyNo"].ToString() == "21")
                            {
                                Exporttoexcel_Packing_Kaysons_4();
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case "3":
                    break;
                case "4":
                    ShippingInstructionExcel();
                    break;
                case "5":
                    break;
                case "6":
                    break;
                case "7":
                    break;
                case "8":
                    break;
                case "9":
                    break;
                case "10":
                    break;
                case "11":
                    break;
                case "12":
                    SCD(Convert.ToInt32(DDinvoiceNo.SelectedValue));
                    break;
                case "13":
                    break;
                case "14":
                    Annexure1(Convert.ToInt32(DDinvoiceNo.SelectedValue));
                    break;
                case "15":
                    break;
                case "16":
                    break;
                case "17":
                    FormSdf(Convert.ToInt32(DDinvoiceNo.SelectedValue));
                    break;
                case "18":
                    Annexure_A(Convert.ToInt32(DDinvoiceNo.SelectedValue));
                    break;
                case "19":
                    //VDF(Convert.ToInt32(DDinvoiceNo.SelectedValue));
                    
                    if (Session["VarCompanyNo"].ToString() == "14")
                    {
                        vdfexcelEastern(Convert.ToInt32(DDinvoiceNo.SelectedValue));
                    }
                    else
                    {
                        vdfexcel(Convert.ToInt32(DDinvoiceNo.SelectedValue));
                    }
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }

    protected void Exporttoexcel_2()
    {
        lblmsg.Text = "Wait For Excel File to Download";
        if (!Directory.Exists(Server.MapPath("~/InvoiceExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/InvoiceExcel/"));
        }
        string Path = "";
        try
        {

            //******************
            string str = @"select * From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue;
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int Pcs = 0;
                Decimal Area = 0, Amount = 0;

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("Invoice1");

                //************set cell width
                //Page
                sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                sht.PageSetup.AdjustTo(83);
                //sht.PageSetup.FitToPages(1, 5);                
                sht.PageSetup.PaperSize = XLPaperSize.LetterPaper;
                sht.PageSetup.VerticalDpi = 300;
                sht.PageSetup.HorizontalDpi = 300;
                //
                sht.PageSetup.Margins.Top = 0.5;
                sht.PageSetup.Margins.Left = 0.26;
                sht.PageSetup.Margins.Right = 0.22;
                sht.PageSetup.Margins.Bottom = 0.51;
                sht.PageSetup.Margins.Header = 0.3;
                sht.PageSetup.Margins.Footer = 0.3;
                //sht.PageSetup.CenterHorizontally = true;
                //sht.PageSetup.CenterVertically = true;
                sht.PageSetup.SetScaleHFWithDocument();
                //************
                sht.Column("A").Width = 10.33;
                sht.Column("B").Width = 20.33;
                sht.Column("C").Width = 6.89;
                sht.Column("D").Width = 5.67;
                sht.Column("E").Width = 8.22;
                sht.Column("F").Width = 7.90;
                sht.Column("G").Width = 10.33;
                sht.Column("H").Width = 6.78;
                sht.Column("I").Width = 8.89;
                sht.Column("J").Width = 5.22;
                sht.Column("K").Width = 8.89;
                sht.Column("L").Width = 4.67;
                sht.Column("M").Width = 8.22;
                sht.Column("N").Width = 4.67;
                sht.Column("O").Width = 9.89;
                sht.Column("P").Width = 17.89;
                //************
                // sht.Row(1).Height = 15.75;
                //*****Header            
                sht.Range("A2:E2").Merge();
                sht.Range("A2").SetValue("Supplier Name:  " + ds.Tables[0].Rows[0]["companyName"]);
                sht.Range("A2:E2").Style.Font.FontName = "Calibri";
                sht.Range("A2:E2").Style.Font.FontSize = 11;
                sht.Range("A2:E2").Style.Font.Bold = true;
                //GSTIN
                sht.Range("M2:P2").Merge();
                sht.Range("M2").SetValue("GSTIN : " + ds.Tables[0].Rows[0]["GStno"]);
                sht.Range("M2:P2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("M2:P2").Style.Font.FontName = "Calibri";
                sht.Range("M2:P2").Style.Font.FontSize = 11;
                sht.Range("M2:P2").Style.Font.Bold = true;
                //Address
                sht.Range("A3:P3").Merge();
                sht.Range("A3").SetValue("Supplier Address : " + ds.Tables[0].Rows[0]["Compaddr1"] + "," + ds.Tables[0].Rows[0]["Compaddr2"] + "," + ds.Tables[0].Rows[0]["Compaddr3"]);
                sht.Range("A3:P3").Style.Font.FontName = "Calibri";
                sht.Range("A3:P3").Style.Font.FontSize = 11;
                //Tax INvoice
                sht.Range("A4:P4").Merge();
                sht.Range("A4").SetValue("TAX INVOICE");
                sht.Range("A4:P4").Style.Font.FontName = "Calibri";
                sht.Range("A4:P4").Style.Font.FontSize = 11;
                sht.Range("A4:P4").Style.Font.Bold = true;
                sht.Range("A4:P4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //Stype A5 to P9
                sht.Range("A5:P9").Style.Font.FontName = "Calibri";
                sht.Range("A5:P9").Style.Font.FontSize = 11;
                //Reverse charge
                sht.Range("A5:B5").Merge();
                sht.Range("A5").SetValue("Reverse Charge");
                sht.Range("C5:H5").Merge();
                sht.Range("C5").SetValue("NO");
                //Invoice No
                sht.Range("A6:B6").Merge();
                sht.Range("A6").SetValue("Invoice No.");
                sht.Range("C6:H6").Merge();
                sht.Range("C6").SetValue(ds.Tables[0].Rows[0]["TInvoiceNo"]);
                //Invoice Date
                sht.Range("A7:B7").Merge();
                sht.Range("A7").SetValue("Invoice Date");
                sht.Range("C7:H7").Merge();
                sht.Range("C7").SetValue(ds.Tables[0].Rows[0]["Invoicedate"]);
                //State & code
                sht.Range("A8:B8").Merge();
                sht.Range("A8").SetValue("State");
                sht.Range("C8:H8").Merge();
                sht.Range("C8").SetValue(":Uttar Pradesh");

                sht.Range("G8:H8").Merge();
                sht.Range("G8").Value = "State Code : 009";
                sht.Range("G8:H8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //PanNo
                sht.Range("A9:B9").Merge();
                sht.Range("A9").SetValue("PAN");
                sht.Range("C9:H9").Merge();
                sht.Range("C9").SetValue(":" + ds.Tables[0].Rows[0]["PanNo"]);
                //Transportaion mode
                sht.Range("I5:K5").Merge();
                sht.Range("I5").SetValue("Transportation Mode");
                sht.Range("L5:P5").Merge();
                sht.Range("L5").SetValue(ds.Tables[0].Rows[0]["Pre_Carriageby"]);
                sht.Range("L5:P5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //Vehicle No
                sht.Range("I6:K6").Merge();
                sht.Range("I6").SetValue("Vehicle No.");
                sht.Range("L6:P6").Merge();
                sht.Range("L6").SetValue(ds.Tables[0].Rows[0]["TruckNo"]);
                sht.Range("L6:P6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //Date of supply
                sht.Range("I7:K7").Merge();
                sht.Range("I7").SetValue("Date of Supply");
                sht.Range("L7:P7").Merge();
                sht.Range("L7").SetValue(ds.Tables[0].Rows[0]["Invoicedate"]);
                sht.Range("L7:P7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //Place of supply
                sht.Range("I8:K8").Merge();
                sht.Range("I8").SetValue("Place of Supply");
                sht.Range("L8:P8").Merge();
                sht.Range("L8").SetValue(ds.Tables[0].Rows[0]["Destinationadd"]);
                sht.Range("L8:P8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //style
                sht.Range("A10:P18").Style.Font.FontName = "Calibri";
                sht.Range("A10:P18").Style.Font.FontSize = 11;
                //Details of Receiver / Billed To
                sht.Range("A10:H10").Merge();
                sht.Range("A10").Value = "Details of Receiver / Billed To";
                sht.Range("A10:H10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //Name
                sht.Range("A11").Value = "Name";
                sht.Range("B11:H11").Merge();
                sht.Range("B11").SetValue(":" + ds.Tables[0].Rows[0]["Receiver"]);
                sht.Range("B11:H11").Style.Font.Bold = true;
                //address
                sht.Range("A12").Value = "Address";
                sht.Range("B12:H15").Merge();
                sht.Range("B12").SetValue(ds.Tables[0].Rows[0]["Receiver_address"]);
                sht.Range("B12:H15").Style.Alignment.WrapText = true;
                sht.Range("B12:H15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                //gstin
                sht.Range("A16").Value = "GSTIN";
                sht.Range("B16:H16").Merge();
                sht.Range("B16").SetValue(":" + ds.Tables[0].Rows[0]["Rec_gstin"]);
                //state
                sht.Range("A17").Value = "State";
                sht.Range("B17:F17").Merge();
                sht.Range("B17").SetValue(":" + ds.Tables[0].Rows[0]["Rec_state"]);

                sht.Range("G17:H17").Merge();
                sht.Range("G17").SetValue("State code : " + ds.Tables[0].Rows[0]["Rec_statecode"]);
                sht.Range("G17:H17").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //PAN
                sht.Range("A18").Value = "PAN";
                sht.Range("B18:F18").Merge();
                sht.Range("B18").SetValue(":" + ds.Tables[0].Rows[0]["Rec_PanNo"]);
                //CInNo
                sht.Range("G18:H18").Merge();
                sht.Range("G18").SetValue("CIN: " + ds.Tables[0].Rows[0]["Rec_CinNo"]);
                sht.Range("G18:H18").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //Details of Consignee / Shipped To
                sht.Range("I10:P10").Merge();
                sht.Range("I10").Value = "Details of Consignee / Shipped To";
                sht.Range("I10:P10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //Name
                sht.Range("I11").Value = "Name";
                sht.Range("J11:P11").Merge();
                sht.Range("J11").SetValue(":" + ds.Tables[0].Rows[0]["Receiver"]);
                sht.Range("J11:P11").Style.Font.Bold = true;
                //address
                sht.Range("I12").Value = "Address";
                sht.Range("J12:P15").Merge();
                sht.Range("J12").SetValue(ds.Tables[0].Rows[0]["Receiver_address"]);
                sht.Range("J12:P15").Style.Alignment.WrapText = true;
                sht.Range("J12:P15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                //gstin
                sht.Range("I16").Value = "GSTIN";
                sht.Range("J16:P16").Merge();
                sht.Range("J16").SetValue(":" + ds.Tables[0].Rows[0]["Rec_gstin"]);
                //state
                sht.Range("I17").Value = "State";
                sht.Range("J17:N17").Merge();
                sht.Range("J17").SetValue(":" + ds.Tables[0].Rows[0]["Rec_state"]);

                sht.Range("O17:P17").Merge();
                sht.Range("O17").SetValue("State code : " + ds.Tables[0].Rows[0]["Rec_statecode"]);
                sht.Range("O17:P17").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //********Detail Headers
                sht.Range("A19:P20").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A19:P20").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A19:P20").Style.Alignment.SetWrapText();

                sht.Range("A19:A20").Merge();
                sht.Range("A19").Value = "Article No.";
                sht.Range("B19:B20").Merge();
                sht.Range("B19").Value = "Name of the product/Service";
                sht.Range("C19:C20").Merge();
                sht.Range("C19").Value = "HSN ACS";
                sht.Range("D19:D20").Merge();
                sht.Range("D19").Value = "UOM";
                sht.Range("E19:E20").Merge();
                sht.Range("E19").Value = "QTY";
                sht.Range("F19:F20").Merge();
                sht.Range("F19").Value = "Rate";
                sht.Range("G19:G20").Merge();
                sht.Range("G19").Value = "Amount";
                sht.Range("H19:H20").Merge();
                sht.Range("H19").Value = "Discount";
                sht.Range("I19:I20").Merge();
                sht.Range("I19").Value = "Taxable value";
                //***GST %
                //cgst
                sht.Range("J19:K19").Merge();
                sht.Range("J19").Value = "CGST";
                sht.Range("J20").Value = "Rate";
                sht.Range("K20").Value = "Amount";
                //sgst
                sht.Range("L19:M19").Merge();
                sht.Range("L19").Value = "SGST";
                sht.Range("L20").Value = "Rate";
                sht.Range("M20").Value = "Amount";
                //Igst
                sht.Range("N19:O19").Merge();
                sht.Range("N19").Value = "IGST";
                sht.Range("N20").Value = "Rate";
                sht.Range("O20").Value = "Amount";
                //Total
                sht.Range("P19:P20").Merge();
                sht.Range("P19").Value = "Total";
                //Borders
                using (var a = sht.Range("A19:I20"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("J19:O19"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("J20:O20"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("P19:P20"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //
                int row = 21;
                int rowstart = 21;
                Decimal Cgstper, Sgstper, Igstper;
                Decimal Invamt = 0, Tcgst = 0, Tsgst = 0, TIgst = 0, Tinvamt = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":P" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    using (var a = sht.Range("A" + row + ":P" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["articleno"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Design"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Hsncode"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Unit"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Pcs"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Price"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Amount"]);
                    Invamt = Convert.ToDecimal(ds.Tables[0].Rows[i]["Amount"]);
                    sht.Range("I" + row).FormulaA1 = "=G" + row + '+' + ("$H$" + row + "");
                    sht.Range("I" + row).Style.NumberFormat.NumberFormatId = 2;

                    Cgstper = Convert.ToDecimal(ds.Tables[0].Rows[i]["cgst"]);
                    Sgstper = Convert.ToDecimal(ds.Tables[0].Rows[i]["Sgst"]);
                    Igstper = Convert.ToDecimal(ds.Tables[0].Rows[i]["Igst"]);

                    Tcgst = Math.Round((Invamt * Cgstper / 100), 2, MidpointRounding.AwayFromZero);
                    Tsgst = Math.Round((Invamt * Sgstper / 100), 2, MidpointRounding.AwayFromZero);
                    TIgst = Math.Round((Invamt * Igstper / 100), 2, MidpointRounding.AwayFromZero);
                    Tinvamt = Tinvamt + Invamt + Tcgst + Tsgst + TIgst;
                    //Cgst
                    sht.Range("J" + row).SetValue(Cgstper == 0 ? "" : Cgstper + "%");
                    sht.Range("K" + row).FormulaA1 = "=I" + row + '*' + ("$J$" + row);
                    sht.Range("K" + row).Style.NumberFormat.NumberFormatId = 2;
                    //Sgst
                    sht.Range("L" + row).SetValue(Sgstper == 0 ? "" : Sgstper + "%");
                    sht.Range("M" + row).FormulaA1 = "=I" + row + '*' + ("$L$" + row);
                    sht.Range("M" + row).Style.NumberFormat.NumberFormatId = 2;
                    //Igst
                    sht.Range("N" + row).SetValue(Igstper == 0 ? "" : Igstper + "%");
                    sht.Range("O" + row).FormulaA1 = "=I" + row + '*' + ("$N$" + row);
                    sht.Range("O" + row).Style.NumberFormat.NumberFormatId = 2;
                    //Total
                    sht.Range("P" + row).FormulaA1 = "=I" + row + '+' + "$K$" + row + '+' + "$M$" + row + '+' + "$O$" + row + "";
                    sht.Range("P" + row).Style.NumberFormat.NumberFormatId = 2;
                    row = row + 1;
                }
                int dsrowcnt = ds.Tables[0].Rows.Count;
                if (dsrowcnt < 10)
                {
                    dsrowcnt = 10 - Convert.ToInt16(ds.Tables[0].Rows.Count);
                }
                using (var a = sht.Range("A" + row + ":P" + (row + dsrowcnt)))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                row = row + dsrowcnt;
                int Totrownum = row;
                //total
                sht.Range("A" + Totrownum + ":D" + row).Merge();
                sht.Range("A" + Totrownum).Value = "Total";
                sht.Range("A" + Totrownum + ":P" + Totrownum).Style.Font.Bold = true;
                sht.Range("A" + Totrownum + ":P" + Totrownum).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E" + Totrownum).FormulaA1 = "=SUM(E" + rowstart + ":E" + (Totrownum - 1) + ")";
                sht.Range("G" + Totrownum).FormulaA1 = "=SUM(G" + rowstart + ":G" + (Totrownum - 1) + ")";
                sht.Range("I" + Totrownum).FormulaA1 = "=SUM(I" + rowstart + ":I" + (Totrownum - 1) + ")";
                sht.Range("K" + Totrownum).FormulaA1 = "=SUM(K" + rowstart + ":K" + (Totrownum - 1) + ")";
                sht.Range("M" + Totrownum).FormulaA1 = "=SUM(M" + rowstart + ":M" + (Totrownum - 1) + ")";
                sht.Range("O" + Totrownum).FormulaA1 = "=SUM(O" + rowstart + ":O" + (Totrownum - 1) + ")";
                sht.Range("P" + Totrownum).FormulaA1 = "=SUM(P" + rowstart + ":P" + (Totrownum - 1) + ")";
                sht.Range("E" + Totrownum + ":P" + Totrownum).Style.NumberFormat.NumberFormatId = 2;

                //Total amount in words
                row = row + 1;
                sht.Range("A" + row + ":I" + row).Merge();
                sht.Range("A" + row).Value = "Total invoice amount in words:";
                sht.Range("A" + row + ":I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row + ":I" + row).Style.Font.SetBold();

                using (var a = sht.Range("I" + row + ":I" + (row + 2)))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                //Total amt before tax
                sht.Range("J" + row + ":M" + row).Merge();
                sht.Range("J" + row).Value = "Total Amount Before Tax";
                sht.Range("N" + row).Value = ":";
                sht.Range("O" + row + ":P" + row).Merge();
                sht.Range("O" + row).FormulaA1 = "=I" + Totrownum;

                sht.Range("J" + row + ":P" + row).Style.Font.SetBold();
                sht.Range("N" + row + ":P" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //
                string amountinwords = "";


                string amt = Tinvamt.ToString();
                string val = "", paise = "";
                if (amt.IndexOf('.') > 0)
                {
                    val = amt.ToString().Split('.')[0];
                    amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val));
                }
                else
                {
                    amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(Tinvamt));
                }

                string Pointamt = string.Format("{0:0.00}", Tinvamt.ToString("0.00"));
                val = "";
                if (Pointamt.IndexOf('.') > 0)
                {
                    val = Pointamt.ToString().Split('.')[1];
                    if (Convert.ToInt32(val) > 0)
                    {
                        paise = ds.Tables[0].Rows[0]["Currencytypeps"] + " " + ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val)) + " ";
                    }
                }
                amountinwords = ds.Tables[0].Rows[0]["CIF"] + " " + amountinwords + " " + paise + "Only";

                sht.Range("A" + (row + 1) + ":I" + (row + 2)).Merge();
                sht.Range("A" + (row + 1)).SetValue(amountinwords);
                sht.Range("A" + (row + 1) + ":I" + (row + 2)).Style.Alignment.SetWrapText();
                sht.Range("A" + (row + 1) + ":I" + (row + 2)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A" + (row + 1) + ":I" + (row + 2)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //Gst
                row = row + 1;
                sht.Range("J" + row + ":M" + row).Merge();
                sht.Range("J" + row).Value = "Add : CGST ";
                sht.Range("N" + row).Value = ":";
                sht.Range("O" + row + ":P" + row).Merge();
                sht.Range("O" + row).FormulaA1 = "=K" + Totrownum;
                sht.Range("J" + row + ":P" + row).Style.Font.SetBold();
                sht.Range("J" + row + ":P" + row).Style.NumberFormat.NumberFormatId = 2;
                sht.Range("N" + row + ":P" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //sgst
                row = row + 1;
                sht.Range("J" + row + ":M" + row).Merge();
                sht.Range("J" + row).Value = "Add : SGST ";
                sht.Range("N" + row).Value = ":";
                sht.Range("O" + row + ":P" + row).Merge();
                sht.Range("O" + row).FormulaA1 = "=M" + Totrownum;

                sht.Range("J" + row + ":P" + row).Style.Font.SetBold();
                sht.Range("J" + row + ":P" + row).Style.NumberFormat.NumberFormatId = 2;
                sht.Range("N" + row + ":P" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //IGST
                row = row + 1;
                sht.Range("J" + row + ":M" + row).Merge();
                sht.Range("J" + row).Value = "Add : IGST ";
                sht.Range("N" + row).Value = ":";
                sht.Range("O" + row + ":P" + row).Merge();
                sht.Range("O" + row).FormulaA1 = "=O" + Totrownum;
                sht.Range("J" + row + ":P" + row).Style.Font.SetBold();
                sht.Range("J" + row + ":P" + row).Style.NumberFormat.NumberFormatId = 2;
                sht.Range("N" + row + ":P" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //Bandetails
                sht.Range("D" + row + ":E" + row).Merge();
                sht.Range("D" + row).Value = "Bank Details";
                sht.Range("D" + row + ":E" + row).Style.Font.SetBold();
                sht.Range("D" + row + ":E" + row).Style.Font.SetUnderline();
                sht.Range("D" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                using (var a = sht.Range("A" + row + ":I" + (row)))
                {
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("I" + row + ":I" + (row)))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("D" + row + ":E" + (row)))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                //Bank Name
                row = row + 1;
                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row).Value = "Bank Name";
                sht.Range("C" + row + ":I" + row).Merge();
                sht.Range("C" + row).Value = ds.Tables[0].Rows[0]["companyBank"];
                using (var a = sht.Range("I" + row + ":I" + (row)))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                //Bank ac No
                row = row + 1;
                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row).Value = "Bank Account No.";
                sht.Range("C" + row + ":I" + row).Merge();
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[0]["Acno"]);

                using (var a = sht.Range("I" + row + ":I" + (row)))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                sht.Range("J" + row + ":M" + row).Merge();
                sht.Range("J" + row).Value = "Tax Amount : GST ";
                sht.Range("N" + row).Value = ":";
                sht.Range("O" + row + ":P" + row).Merge();
                sht.Range("O" + row).FormulaA1 = "=K" + Totrownum + '+' + "$M$" + Totrownum + '+' + "$O$" + Totrownum;
                sht.Range("J" + row + ":P" + row).Style.Font.SetBold();
                sht.Range("J" + row + ":P" + row).Style.NumberFormat.NumberFormatId = 2;
                sht.Range("N" + row + ":P" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //Ifsc
                row = row + 1;
                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row).Value = "Bank Branch IFSC";
                sht.Range("C" + row + ":I" + row).Merge();
                sht.Range("C" + row).Value = ds.Tables[0].Rows[0]["Ifscode"];

                using (var a = sht.Range("I" + row + ":I" + (row)))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A" + row + ":P" + (row)))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Range("J" + row + ":M" + row).Merge();
                sht.Range("J" + row).Value = "Tax Amount After Tax ";
                sht.Range("N" + row).Value = ":";
                sht.Range("O" + row + ":P" + row).Merge();
                sht.Range("O" + row).FormulaA1 = "=I" + Totrownum + '+' + "$K$" + Totrownum + '+' + "$M$" + Totrownum + '+' + "$O$" + Totrownum;
                sht.Range("J" + row + ":P" + row).Style.Font.SetBold();
                sht.Range("J" + row + ":P" + row).Style.NumberFormat.NumberFormatId = 2;
                sht.Range("N" + row + ":P" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //Term And Conditions
                row = row + 1;
                sht.Range("C" + row + ":E" + row).Merge();
                sht.Range("C" + row).Value = "Term And Conditions";
                sht.Range("C" + row + ":E" + row).Style.Font.SetBold();
                sht.Range("C" + row + ":E" + row).Style.Font.SetUnderline();
                sht.Range("C" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                using (var a = sht.Range("C" + row + ":E" + (row)))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                sht.Range("A" + (row + 1) + ":I" + (row + 5)).Merge();
                sht.Range("A" + row + 1).Value = "";
                sht.Range("A" + (row + 1) + ":I" + (row + 5)).Style.Alignment.SetWrapText();
                sht.Range("A" + (row + 1) + ":I" + (row + 5)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("I" + (row) + ":I" + (row + 5)))
                {

                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                //Certified that the particular given above are true and correct
                row = row + 1;
                sht.Range("J" + row + ":P" + row).Merge();
                sht.Range("J" + row).Value = "Certified that the particular given above are true and correct";
                sht.Range("J" + row + ":P" + row).Style.Font.FontSize = 8;
                sht.Range("J" + row + ":P" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                using (var a = sht.Range("J" + row + ":P" + (row)))
                {
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                }

                //companyname
                row = row + 1;
                sht.Range("J" + row + ":P" + row).Merge();
                sht.Range("J" + row).Value = ds.Tables[0].Rows[0]["companyName"];
                sht.Range("J" + row + ":P" + row).Style.Font.SetBold();
                sht.Range("J" + row + ":P" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //Authorised sign
                row = row + 3;
                sht.Range("J" + row + ":P" + row).Merge();
                sht.Range("J" + row).Value = "Authorised Signatory";
                sht.Range("J" + row + ":P" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //Header borders
                using (var a = sht.Range("A2:P2"))
                {
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A2:A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("P2:P" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A3:P3"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A4:P4"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("H5:H9"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A9:P9"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A10:P10"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("H11:H18"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("A" + row + ":P" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                string Fileextension = "xlsx";
                string filename = "Invoice-" + validateFilename(ds.Tables[0].Rows[0]["TinvoiceNo"].ToString()) + "." + Fileextension + "";
                Path = Server.MapPath("~/InvoiceExcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                // Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
                lblmsg.Text = "Invoice Excel Format downloaded successfully.";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "AltR1", "alert('No Record Found to Export')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
    protected void FormSdf(int Invoiceid)
    {
        string str = "select * From V_InvoiceDetail where Invoiceid=" + Invoiceid;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptformsdftype1.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptformsdftype1.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void Annexure1(int Invoiceid)
    {
        string str = "select * From V_InvoiceDetail where Invoiceid=" + Invoiceid;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptannexuretype1.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptannexuretype1.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void Annexure_A(int Invoiceid)
    {
        string str = "select * From V_InvoiceDetail where Invoiceid=" + Invoiceid;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptannexure_A.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptannexure_A.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void SCD(int Invoiceid)
    {
        string str = "select * From V_SinglecountryDecl_Articlewise where Invoiceid=" + Invoiceid;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptSinglecountrydec.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptSinglecountrydec.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void VDF(int Invoiceid)
    {
        string str = @"select *,VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1) as Pcsperroll,VD.Pcs/(VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1)) as TotalRoll 
                        From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId 
                        Where VI.invoiceid=" + Invoiceid;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptvdf.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptvdf.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx?Export=Y', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void FillInvoiceNo()
    {
        string str = "select InvoiceId,TInvoiceNo From INVOICE Where consignorId=" + DDCompanyName.SelectedValue + " and InvoiceYear=" + DDInvoiceYear.SelectedValue + "  order by invoiceid desc,Tinvoiceno asc";
        UtilityModule.ConditionalComboFill(ref DDinvoiceNo, str, true, "--Plz Select--");

    }
    protected void DDInvoiceYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillInvoiceNo();
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillInvoiceNo();
    }
    protected void DDDocumentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDPrintType.Items.Clear();
        switch (DDDocumentType.SelectedValue)
        {
            case "1":
                FillInvoiceTypeOther();
                break;
            case "2":
                FillPackingType();
                break;
            case "12":
                DDPrintType.Items.Clear();
                DDPrintType.Items.Add(new ListItem("Type1", "1"));
                break;
            case "4":
                DDPrintType.Items.Clear();
                DDPrintType.Items.Add(new ListItem("Type1", "1"));
                break;
            case "14":
                DDPrintType.Items.Clear();
                DDPrintType.Items.Add(new ListItem("Type1", "1"));
                break;
            case "17":
                DDPrintType.Items.Clear();
                DDPrintType.Items.Add(new ListItem("Type1", "1"));
                break;
            case "18":
                DDPrintType.Items.Clear();
                DDPrintType.Items.Add(new ListItem("Type1", "1"));
                break;
            case "19":
                DDPrintType.Items.Clear();
                DDPrintType.Items.Add(new ListItem("Type1", "1"));
                break;
            default:
                break;
        }
    }

    protected void Exporttoexcel_1()
    {
        lblmsg.Text = "Wait For Excel File to Download";
        if (!Directory.Exists(Server.MapPath("~/InvoiceExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/InvoiceExcel/"));
        }
        string Path = "";
        try
        {

            //******************
            string str = @"select * From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue;
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int i, Pcs = 0;
                Decimal Area = 0, Amount = 0;

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("Invoice1");

                //************set cell width
                //Page
                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                sht.PageSetup.AdjustTo(63);
                //sht.PageSetup.FitToPages(1, 5);                
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                sht.PageSetup.VerticalDpi = 300;
                sht.PageSetup.HorizontalDpi = 300;
                //
                sht.PageSetup.Margins.Top = 0.01;
                sht.PageSetup.Margins.Left = 0;
                sht.PageSetup.Margins.Right = 0;
                sht.PageSetup.Margins.Bottom = 0.01;
                sht.PageSetup.Margins.Header = 0.1;
                sht.PageSetup.Margins.Footer = 0.1;
                sht.PageSetup.CenterHorizontally = true;
                sht.PageSetup.CenterVertically = true;
                sht.PageSetup.SetScaleHFWithDocument();
                //************
                sht.Column("A").Width = 14.71;
                sht.Column("B").Width = 8.86;
                sht.Column("C").Width = 18.00;
                sht.Column("D").Width = 9.29;
                sht.Column("E").Width = 10.43;
                sht.Column("F").Width = 15.86;
                sht.Column("G").Width = 15.86;
                sht.Column("H").Width = 12.86;
                sht.Column("I").Width = 18.29;
                sht.Column("J").Width = 10.71;
                sht.Column("K").Width = 12.29;
                sht.Column("L").Width = 23.14;
               
                //************
                sht.Row(1).Height = 15.75;
                //*****Header                
                sht.Cell("A1").Value = "INVOICE";
                sht.Range("A1:L1").Style.Font.FontName = "Tahoma";
                sht.Range("A1:L1").Style.Font.FontSize = 12;
                sht.Range("A1:L1").Style.Font.Bold = true;
                sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:L1").Merge();
                //***************
                sht.Cell("A2").Value = @"""SUPPLY MEANT FOR EXPORT UNDER LETTER OF UNDERTAKING NO-21/2017 Dt.07.07.2017. WITHOUT PAYMENT OF INTEGRATED TAX""";
                sht.Range("A2:L2").Style.Font.FontName = "Tahoma";
                sht.Range("A2:L2").Style.Font.FontSize = 12;
                sht.Range("A2:L2").Style.Font.Bold = true;
                sht.Range("A2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:L2").Merge();
                using (var a = sht.Range("A2:L2"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                }
                //********Exporter
                sht.Range("A3").Value = "Exporter";
                sht.Range("A3:F3").Style.Font.FontName = "Tahoma";
                sht.Range("A3:F3").Style.Font.FontSize = 12;
                sht.Range("A3:F3").Merge();
                //*****************

                //CompanyName
                sht.Range("A4").Value = ds.Tables[0].Rows[0]["companyName"];
                sht.Range("A4:F4").Style.Font.FontName = "Tahoma";
                sht.Range("A4:F4").Style.Font.FontSize = 12;
                sht.Range("A4:F4").Style.Font.Bold = true;
                sht.Range("A4:F4").Merge();
                //Address
                sht.Range("A5").Value = ds.Tables[0].Rows[0]["compaddr1"];
                sht.Range("A5:F5").Style.Font.FontName = "Tahoma";
                sht.Range("A5:F5").Style.Font.FontSize = 12;
                sht.Range("A5:F5").Merge();
                //address2
                sht.Range("A6").Value = ds.Tables[0].Rows[0]["compaddr2"] + "," + ds.Tables[0].Rows[0]["compaddr3"];
                sht.Range("A6:F6").Style.Font.FontName = "Tahoma";
                sht.Range("A6:F6").Style.Font.FontSize = 12;
                sht.Range("A6:F6").Merge();
                //TiN No
                sht.Range("A7").Value = "GSTIN#" + ds.Tables[0].Rows[0]["gstno"];
                sht.Range("A7:F7").Style.Font.FontName = "Tahoma";
                sht.Range("A7:F7").Style.Font.FontSize = 12;
                sht.Range("A7:F7").Style.Font.Bold = true;
                sht.Range("A7:F7").Merge();
                //**********INvoiceNodate
                sht.Range("G3").Value = "Invoice No./Date";
                sht.Range("G3:L3").Style.Font.FontName = "Tahoma";
                sht.Range("G3:L3").Style.Font.FontSize = 12;
                sht.Range("G3:L3").Merge();
                //value
                sht.Range("G4").Value = ds.Tables[0].Rows[0]["TInvoiceNo"] + " Dated " + ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("G4:L4").Style.Font.FontName = "Tahoma";
                sht.Range("G4:L4").Style.Font.FontSize = 12;
                sht.Range("G4:L4").Merge();
                sht.Range("G4", "L4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //*************IE Code
                sht.Range("G5").Value = "IE Code No.";
                sht.Range("G5:I5").Style.Font.FontName = "Tahoma";
                sht.Range("G5:I5").Style.Font.FontSize = 12;
                sht.Range("G5:I5").Merge();
                //value
                sht.Range("G6").Value = ds.Tables[0].Rows[0]["IEcode"];
                sht.Range("G6:I6").Style.Font.FontName = "Tahoma";
                sht.Range("G6:I6").Style.Font.FontSize = 12;
                sht.Range("G6:I6").Merge();
                sht.Range("G6", "I6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //***********GRI Form No
                sht.Range("J5").Value = "REX No.";
                sht.Range("J5:L5").Style.Font.FontName = "Tahoma";
                sht.Range("J5:L5").Style.Font.FontSize = 12;
                sht.Range("J5:L5").Merge();
                // value
                sht.Range("J6").Value = "INREX1510001883EC025";
                sht.Range("J6:L6").Style.Font.FontName = "Tahoma";
                sht.Range("J6:L6").Style.Font.FontSize = 12;
                sht.Range("J6:L6").Merge();
                sht.Range("J6", "L6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //*************Buyer's Order No and Date
                sht.Range("G7").Value = "Buyer's Order No. / Date";
                sht.Range("G7:L8").Style.Font.FontName = "Tahoma";
                sht.Range("G7:L8").Style.Font.FontSize = 12;
                sht.Range("G7:L8").Merge();
                //value
                sht.Range("G8").SetValue(ds.Tables[0].Rows[0]["TorderNo"]);
                sht.Range("G8:L8").Style.Font.FontName = "Tahoma";
                sht.Range("G8:L8").Style.Font.FontSize = 12;
                sht.Range("G8:L8").Merge();
                sht.Range("G8", "L8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //**************Other Reference(s)
                sht.Range("G9").Value = "Other Reference(s)";
                sht.Range("G10").Value = "CONSIGNMENT NO.:" + ds.Tables[0].Rows[0]["otherref"];
                sht.Range("G11").Value = "SUPPLIER NO.: 21899";
                sht.Range("G9:G11").Style.Font.FontName = "Tahoma";
                sht.Range("G9:G11").Style.Font.FontSize = 12;
                sht.Range("G9:L9").Merge();
                sht.Range("G10:L10").Merge();
                sht.Range("G11:L11").Merge();
                //*************Consignee
                sht.Range("A12").Value = "Consignee";
                sht.Range("C12").Value = ds.Tables[0].Rows[0]["consignee_dt"];
                sht.Range("A12:B12").Style.Font.FontName = "Tahoma";
                sht.Range("A12:B12").Style.Font.FontSize = 12;

                sht.Range("C12:F12").Style.Font.FontName = "Tahoma";
                sht.Range("C12:F12").Style.Font.FontSize = 12;
                sht.Range("C12:F12").Style.Font.Bold = true;

                sht.Range("A12:B12").Merge();
                sht.Range("C12:F12").Merge();
                //value
                sht.Range("A13").Value = ds.Tables[0].Rows[0]["consignee"];
                sht.Range("A13:F13").Style.Font.FontName = "Tahoma";
                sht.Range("A13:F13").Style.Font.FontSize = 12;
                sht.Range("A13:F13").Style.Font.Bold = true;
                sht.Range("A13:F13").Merge();
                //**
                sht.Range("A14").Value = ds.Tables[0].Rows[0]["consignee_address"];
                sht.Range("A14:F17").Style.Font.FontName = "Tahoma";
                sht.Range("A14:F17").Style.Font.FontSize = 12;
                sht.Range("A14", "F17").Style.Alignment.WrapText = true;
                sht.Range("A14", "F17").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A14:F17").Merge();
                //***********Notify
                sht.Range("A18").Value = "Notify Party";
                sht.Range("C18").Value = ds.Tables[0].Rows[0]["Notifyparty_dt"];
                sht.Range("A18:B18").Style.Font.FontName = "Tahoma";
                sht.Range("A18:B18").Style.Font.FontSize = 12;

                sht.Range("A18:B18").Style.Font.SetUnderline();
                sht.Range("C18:F18").Style.Font.FontName = "Tahoma";
                sht.Range("C18:F18").Style.Font.FontSize = 12;
                sht.Range("C18:F18").Style.Font.Bold = true;

                sht.Range("A18:B18").Merge();
                sht.Range("C18:F18").Merge();
                //value
                sht.Range("A19").Value = ds.Tables[0].Rows[0]["Notifyparty"];
                sht.Range("A19:F19").Style.Font.FontName = "Tahoma";
                sht.Range("A19:F19").Style.Font.FontSize = 12;
                sht.Range("A19:F19").Style.Font.Bold = true;
                sht.Range("A19:F19").Merge();
                //
                sht.Range("A20").Value = ds.Tables[0].Rows[0]["notifyparty_address"];
                sht.Range("A20:F24").Style.Font.FontName = "Tahoma";
                sht.Range("A20:F24").Style.Font.FontSize = 12;
                sht.Range("A20", "F24").Style.Alignment.WrapText = true;
                sht.Range("A20", "F24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A20:F24").Merge();
                //***********Receiver address
                sht.Range("G12").Value = "Receiver Address";
                sht.Range("G12:I12").Style.Font.FontName = "Tahoma";
                sht.Range("G12:I12").Style.Font.FontSize = 12;
                sht.Range("G12:I12").Merge();
                //values     2
                sht.Range("J12").Value = ds.Tables[0].Rows[0]["Destcode"];
                sht.Range("J12:L12").Style.Font.FontName = "Tahoma";
                sht.Range("J12:L12").Style.Font.FontSize = 12;
                sht.Range("J12:L12").Style.Font.Bold = true;
                sht.Range("J12:L12").Merge();
                //****** 1.
                sht.Range("G13").Value = "1. " + ds.Tables[0].Rows[0]["Receiver"];
                sht.Range("G13:L13").Style.Font.FontName = "Tahoma";
                sht.Range("G13:L13").Style.Font.FontSize = 12;
                sht.Range("G13:L13").Style.Font.Bold = true;
                sht.Range("G13:L13").Merge();
                //*
                sht.Range("G14").Value = ds.Tables[0].Rows[0]["Receiver_address"];
                sht.Range("G14:L16").Style.Font.FontName = "Tahoma";
                sht.Range("G14:L16").Style.Font.FontSize = 12;
                sht.Range("G14:L16").Style.Alignment.WrapText = true;
                sht.Range("G14:L16").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("G14:L16").Merge();
                //****** 2.
                sht.Range("G17").Value = "2.Paying Agent : " + ds.Tables[0].Rows[0]["payingagent"];
                sht.Range("G17:L17").Style.Font.FontName = "Tahoma";
                sht.Range("G17:L17").Style.Font.FontSize = 12;
                sht.Range("G17:L17").Style.Font.Bold = true;
                sht.Range("G17:L17").Merge();
                //*
                sht.Range("I18").Value = ds.Tables[0].Rows[0]["payingagent_address"];
                sht.Range("I18:L20").Style.Font.FontName = "Tahoma";
                sht.Range("I18:L20").Style.Font.FontSize = 12;
                sht.Range("I18:L20").Style.Alignment.WrapText = true;
                sht.Range("I18:L20").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("I18:L20").Merge();
                //*******3.
                sht.Range("G21").Value = "Buyer (If other than Consignee)";
                sht.Range("G21:L21").Style.Font.FontName = "Tahoma";
                sht.Range("G21:L21").Style.Font.FontSize = 12;
                sht.Range("G21:L21").Merge();

                sht.Range("G22").Value = "3." + ds.Tables[0].Rows[0]["buyer_otherthanconsignee"];
                sht.Range("G22:L22").Style.Font.FontName = "Tahoma";
                sht.Range("G22:L22").Style.Font.FontSize = 12;
                sht.Range("G22:L22").Merge();
                //*
                sht.Range("G23").Value = ds.Tables[0].Rows[0]["otherthanconsignee_address"];
                sht.Range("G23:L25").Style.Font.FontName = "Tahoma";
                sht.Range("G23:L25").Style.Font.FontSize = 12;
                sht.Range("G23:L25").Style.Alignment.WrapText = true;
                sht.Range("G23:L25").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("G23:L25").Merge();
                //***********Pre-carriage By
                sht.Range("A25").Value = "Pre-Carriage By";
                sht.Range("A25:C25").Style.Font.FontName = "Tahoma";
                sht.Range("A25:C25").Style.Font.FontSize = 12;
                sht.Range("A25:C25").Merge();
                //value
                sht.Range("A26").Value = ds.Tables[0].Rows[0]["Pre_carriageby"];
                sht.Range("A26:C26").Style.Font.FontName = "Tahoma";
                sht.Range("A26:C26").Style.Font.FontSize = 12;
                sht.Range("A26:C26").Merge();
                sht.Range("A26:C26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Place of Receipt by Pre-Carrier
                sht.Range("D25").Value = "Place of Receipt by Pre-Carrier";
                sht.Range("D25:F25").Style.Font.FontName = "Tahoma";
                sht.Range("D25:F25").Style.Font.FontSize = 12;
                sht.Range("D25:F25").Merge();
                //value
                sht.Range("D26").Value = ds.Tables[0].Rows[0]["receiptby"];
                sht.Range("D26:F26").Style.Font.FontName = "Tahoma";
                sht.Range("D26:F26").Style.Font.FontSize = 12;
                sht.Range("D26:F26").Merge();
                sht.Range("D26:F26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Vessel/Flight No
                sht.Range("A27").Value = "Vessel/Flight No";
                sht.Range("A27:C27").Style.Font.FontName = "Tahoma";
                sht.Range("A27:C27").Style.Font.FontSize = 12;
                sht.Range("A27:C27").Merge();
                //value
                sht.Range("A28").Value = ds.Tables[0].Rows[0]["vessel_flightno"];
                sht.Range("A28:C28").Style.Font.FontName = "Tahoma";
                sht.Range("A28:C28").Style.Font.FontSize = 12;
                sht.Range("A28:C28").Merge();
                sht.Range("A28:C28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Loading
                sht.Range("D27").Value = "Port of Loading";
                sht.Range("D27:F27").Style.Font.FontName = "Tahoma";
                sht.Range("D27:F27").Style.Font.FontSize = 12;
                sht.Range("D27:F27").Merge();
                //value
                sht.Range("D28").Value = ds.Tables[0].Rows[0]["portload"];
                sht.Range("D28:F28").Style.Font.FontName = "Tahoma";
                sht.Range("D28:F28").Style.Font.FontSize = 12;
                sht.Range("D28:F28").Merge();
                sht.Range("D28:F28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Discharge
                sht.Range("A29").Value = "Port of Discharge";
                sht.Range("A29:C29").Style.Font.FontName = "Tahoma";
                sht.Range("A29:C29").Style.Font.FontSize = 12;
                sht.Range("A29:C29").Merge();
                //value
                sht.Range("A30").Value = ds.Tables[0].Rows[0]["portunload"];
                sht.Range("A30:C30").Style.Font.FontName = "Tahoma";
                sht.Range("A30:C30").Style.Font.FontSize = 12;
                sht.Range("A30:C30").Merge();
                sht.Range("A30:C30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Final Destination
                sht.Range("D29").Value = "Final Destination";
                sht.Range("D29:F29").Style.Font.FontName = "Tahoma";
                sht.Range("D29:F29").Style.Font.FontSize = 12;
                sht.Range("D29:F29").Merge();
                //value
                sht.Range("D30").Value = ds.Tables[0].Rows[0]["Destinationadd"];
                sht.Range("D30:F30").Style.Font.FontName = "Tahoma";
                sht.Range("D30:F30").Style.Font.FontSize = 12;
                sht.Range("D30:F30").Merge();
                sht.Range("D30:F30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Origin of Goods
                sht.Range("G26").Value = "Country of Origin of Goods";
                sht.Range("G26:I26").Style.Font.FontName = "Tahoma";
                sht.Range("G26:I26").Style.Font.FontSize = 12;
                sht.Range("G26:I26").Merge();
                //value
                sht.Range("G27").Value = ds.Tables[0].Rows[0]["countryoforigin"];
                sht.Range("G27:I27").Style.Font.FontName = "Tahoma";
                sht.Range("G27:I27").Style.Font.FontSize = 12;
                sht.Range("G27:I27").Merge();
                sht.Range("G27:I27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Final Destination
                sht.Range("J26").Value = "Country of Final Destination";
                sht.Range("J26:L26").Style.Font.FontName = "Tahoma";
                sht.Range("J26:L26").Style.Font.FontSize = 12;
                sht.Range("J26:L26").Merge();
                //value
                sht.Range("J27").Value = ds.Tables[0].Rows[0]["countryoffinaldest"];
                sht.Range("J27:L27").Style.Font.FontName = "Tahoma";
                sht.Range("J27:L27").Style.Font.FontSize = 12;
                sht.Range("J27:L27").Style.NumberFormat.Format = "@";
                sht.Range("J27:L27").Merge();
                sht.Range("J27:L27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //***********Terms of Delivery and Payment
                sht.Range("G28").Value = "Terms of Delivery and Payment";
                sht.Range("G28:L28").Style.Font.FontName = "Tahoma";
                sht.Range("G28:L28").Style.Font.FontSize = 12;
                sht.Range("G28:L28").Merge();
                //value
                sht.Range("H29").Value = ds.Tables[0].Rows[0]["Deliveryterm"];
                sht.Range("H29:L30").Style.Font.FontName = "Tahoma";
                sht.Range("H29:L30").Style.Font.FontSize = 12;
                sht.Range("H29:L30").Style.NumberFormat.Format = "@";
                sht.Range("H29:L30").Style.Alignment.WrapText = true;
                sht.Range("H29:L30").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("H29:L30").Merge();
                //*****************Nos
                sht.Range("A31").Value = "Nos.";
                sht.Range("B31").Value = "No and kind of Packages";
                sht.Range("B31:C31").Merge();
                sht.Range("E31").Value = "Description of Goods";
                sht.Range("E31:I31").Merge();
                sht.Range("J31").Value = "Quantity";
                sht.Range("K31").Value = "Rate";
                sht.Range("L31").Value = "Amount";
                sht.Range("A31:L31").Style.Font.FontName = "Tahoma";
                sht.Range("A31:L31").Style.Font.FontSize = 12;
                sht.Range("A31:L31").Style.NumberFormat.Format = "@";
                sht.Range("L31").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //values
                sht.Range("A32:I32").Merge();
                sht.Range("A33:I37").Style.Font.FontName = "Tahoma";
                sht.Range("A33:I37").Style.Font.FontSize = 12;
                sht.Range("A33:I37").Style.NumberFormat.Format = "@";

                sht.Range("A33").SetValue(ds.Tables[0].Compute("Min(MinrollNo)", "") + "/" + ds.Tables[0].Compute("Max(Maxrollno)", ""));

                sht.Range("B33").Value = ds.Tables[0].Rows[0]["Noofrolls"] + " Packages";
                sht.Range("B33:C33").Merge();
                sht.Range("D33").Value = ds.Tables[0].Rows[0]["descriptionofgoods"];
                sht.Range("D33:I37").Style.Alignment.WrapText = true;
                sht.Range("D33:I37").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("D33:I37").Merge();
                sht.Range("A38:I38").Merge();
                //********Details
                sht.Range("A39").Value = "P.O.NO.";
                sht.Range("A39:L39").Style.Font.FontName = "Tahoma";
                sht.Range("A39:L39").Style.Font.FontSize = 12;
                sht.Range("A39:L39").Style.Font.Bold = true;
                sht.Range("A39:L39").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range("B39").Value = "ARTICLE NAME";
                sht.Range("B39:C39").Merge();
                sht.Range("D39").Value = "ART.NO.";
                sht.Range("D39:E39").Merge();
                sht.Range("F39").Value = "HSN CODE";
                sht.Range("G39").Value = "COLOR";
                sht.Range("H39").Value = "SIZE(CM)";
                sht.Range("I39").Value = "AREA SQ.MTR.";
                sht.Range("J39").Value = "QTY";
                sht.Range("K39").Value = "FCA/" + ds.Tables[0].Rows[0]["CIF"];
                sht.Range("L39").Value = "FCA/" + ds.Tables[0].Rows[0]["CIF"];
                sht.Range("L39").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                
                //***********generate Loop
                i = 40;
                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                {
                    sht.Range("A" + i + ":L" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("A" + i + ":L" + i).Style.Font.FontSize = 12;
                    sht.Range("A" + i + ":L" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //pono
                    sht.Range("A" + i).Value = ds.Tables[0].Rows[ii]["Pono"];
                    //Article Name
                    sht.Range("B" + i).Value = ds.Tables[0].Rows[ii]["Design"];
                    sht.Range("B" + i, "C" + i).Merge();
                    //Art No.
                    sht.Range("D" + i).Value = ds.Tables[0].Rows[ii]["articleno"];
                    sht.Range("D" + i, "E" + i).Merge();
                    //HSNCode
                    sht.Range("F" + i).Value = ds.Tables[0].Rows[ii]["QualityWiseHSNCode"];

                    //Colour
                    sht.Range("G" + i).Value = ds.Tables[0].Rows[ii]["Color"];
                    //Size
                    sht.Range("H" + i).Value = ds.Tables[0].Rows[ii]["width"] + "x" + ds.Tables[0].Rows[ii]["Length"];
                    //Area
                    sht.Range("I" + i).SetValue(ds.Tables[0].Rows[ii]["Area"]);
                    Area = Decimal.Parse(Area.ToString()) + Decimal.Parse(ds.Tables[0].Rows[ii]["Area"].ToString());
                    //QTY
                    sht.Range("J" + i).SetValue(ds.Tables[0].Rows[ii]["Pcs"]);
                    Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[ii]["Pcs"]);
                    //
                    var _with27 = sht.Range("J" + i);
                    _with27.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    _with27.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //rate
                    sht.Range("K" + i).SetValue(ds.Tables[0].Rows[ii]["Price"]);

                    var _with28 = sht.Range("K" + i);
                    _with28.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    _with28.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //Amount
                    sht.Range("L" + i).SetValue(ds.Tables[0].Rows[ii]["amount"]);
                    Amount = Amount + Convert.ToDecimal(ds.Tables[0].Rows[ii]["amount"]);
                    //
                    sht.Range("L" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    // sht.Range("H" + i + ":K" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("J" + i).Style.NumberFormat.Format = "@";
                    sht.Range("K" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("I" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("L" + i).Style.NumberFormat.Format = "#,###0.00";
                    //border
                    sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("L" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    i = i + 1;
                }
                var _with29 = sht.Range("J" + i + ":J" + (i + 2));
                _with29.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with29.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                sht.Range("A" + i + ":A" + (i + 2)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("L" + i + ":L" + (i + 2)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                var _with30 = sht.Range("K" + i + ":K" + (i + 2));
                _with30.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with30.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //End Of loop
                i = i + 2;
                //Total            
                sht.Range("H" + i + ":J" + i).Style.Font.FontName = "Tahoma";
                sht.Range("H" + i + ":J" + i).Style.Font.FontSize = 12;
                //sht.Range("G" + i + ":I" + i).Style.NumberFormat.Format = "@";
                sht.Range("H" + i).Style.NumberFormat.Format = "@";
                sht.Range("J" + i).Style.NumberFormat.Format = "@";
                sht.Range("H" + i + ":J" + i).Style.Font.Bold = true;
                sht.Range("H" + i + ":J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range("H" + i).Value = "Total:";
                sht.Range("I" + i).SetValue(Area);
                sht.Range("J" + i).SetValue(Pcs);
                sht.Range("A" + i + ":F" + i).Merge();
                sht.Range("I" + i).Style.NumberFormat.Format = "#,###0.00";
                //border
                var _with23 = sht.Range("I" + i);
                _with23.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                _with23.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with24 = sht.Range("J" + i);
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
                var _with31 = sht.Range("J" + i + ":J" + (i + 5));
                _with31.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with31.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                sht.Range("A" + i + ":A" + (i + 5)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("L" + i + ":L" + (i + 5)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                var _with32 = sht.Range("K" + i + ":K" + (i + 5));
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
                //***************
                i = i + 5;
                sht.Range("A" + i + ":H" + i).Merge();
                sht.Range("A" + i).SetValue("The exporter Eastern Home Industries of the product covered by this document Declares that, ");
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("L" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                using (var a = sht.Range("J" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("K" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                i = i + 1;
                sht.Range("A" + i + ":H" + i).Merge();
                sht.Range("A" + i).SetValue("except where otherwise clearly indicated, these products are of …(Indian)…. Preferential origin");
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("L" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                using (var a = sht.Range("J" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("L" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                i = i + 1;
                sht.Range("A" + i + ":H" + i).Merge();
                sht.Range("A" + i).SetValue("according to the rules of origin of the Generalised System of the Eouropeon Union and ");
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("L" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                using (var a = sht.Range("J" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("L" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                i = i + 1;
                sht.Range("A" + i + ":H" + i).Merge();
                sht.Range("A" + i).SetValue("that the origin criterion met is …\"P\"");
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("L" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                using (var a = sht.Range("J" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("L" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                //**********Article Loop          
                //
                i = i + 2;

                //i = i + 5;
                sht.Range("A" + i).Value = "ARTICLE NAME";
                sht.Range("A" + i + ":B" + i).Merge();
                sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":B" + i).Style.Font.Bold = true;

                //
                sht.Range("D" + i).Value = "ARTICLE NO";
                sht.Range("D" + i + ":E" + i).Merge();
                sht.Range("D" + i + ":E" + i).Style.Font.FontName = "Tahoma";
                sht.Range("D" + i + ":E" + i).Style.Font.FontSize = 12;
                sht.Range("D" + i + ":E" + i).Style.NumberFormat.Format = "@";
                sht.Range("D" + i + ":E" + i).Style.Font.Bold = true;

                //
                sht.Range("F" + i).Value = "CONTENT";
                sht.Range("F" + i + ":H" + i).Merge();
                sht.Range("F" + i + ":H" + i).Style.Font.FontName = "Tahoma";
                sht.Range("F" + i + ":H" + i).Style.Font.FontSize = 12;
                sht.Range("F" + i + ":H" + i).Style.NumberFormat.Format = "@";
                sht.Range("F" + i + ":H" + i).Style.Font.Bold = true;
                //
                sht.Range("I" + i).Value = "RATE/SQM";
                sht.Range("I" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i).Style.Font.FontSize = 12;
                sht.Range("I" + i).Style.NumberFormat.Format = "@";
                sht.Range("I" + i).Style.Font.Bold = true;
                //Border
                var _with33 = sht.Range("J" + (i - 1) + ":J" + i);
                _with33.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with33.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + (i - 1) + ":A" + (i)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("L" + (i - 1) + ":L" + (i)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                var _with34 = sht.Range("K" + (i - 1) + ":K" + i);
                _with34.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with34.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //********
                i = i + 1;
                sht.Range("A" + i + ":A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("L" + i + ":L" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                str = "Select Design as ArticleNo,ArticleNo as RealArticleNo,contents,Round(sum(amount)/sum(Area),2) as Rate From V_PackingDetailsIkea Where PackingId=" + DDinvoiceNo.SelectedValue + " group by Design,contents,ArticleNo";
                DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                    {
                        sht.Range("A" + i).SetValue(ds1.Tables[0].Rows[j]["articleno"]);
                        sht.Range("A" + i + ":B" + i).Merge();
                        sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Tahoma";
                        sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 12;
                        sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";
                        //
                        sht.Range("D" + i).SetValue(ds1.Tables[0].Rows[j]["RealArticleNo"]);
                        sht.Range("D" + i + ":E" + i).Merge();
                        sht.Range("D" + i + ":E" + i).Style.Font.FontName = "Tahoma";
                        sht.Range("D" + i + ":E" + i).Style.Font.FontSize = 12;
                        sht.Range("D" + i + ":E" + i).Style.NumberFormat.Format = "@";

                        //
                        sht.Range("F" + i).SetValue(ds1.Tables[0].Rows[j]["contents"]);
                        sht.Range("F" + i + ":H" + i).Merge();
                        sht.Range("F" + i + ":H" + i).Style.Font.FontName = "Tahoma";
                        sht.Range("F" + i + ":H" + i).Style.Font.FontSize = 12;
                        sht.Range("F" + i + ":H" + i).Style.NumberFormat.Format = "@";
                        //rate
                        sht.Range("I" + i).SetValue(ds1.Tables[0].Rows[j]["rate"]);
                        sht.Range("I" + i).Style.Font.FontName = "Tahoma";
                        sht.Range("I" + i).Style.Font.FontSize = 12;
                        sht.Range("I" + i).Style.NumberFormat.Format = "@";
                        sht.Range("I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                        //
                        var _with35 = sht.Range("J" + (i) + ":J" + i);
                        _with35.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        _with35.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        //
                        var _with36 = sht.Range("K" + (i) + ":K" + i);
                        _with36.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        _with36.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                        sht.Range("A" + i + ":A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        sht.Range("L" + i + ":L" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

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
                string amt = Amount.ToString();
                string val = "", paise = "";

                if (amt.IndexOf('.') > 0)
                {
                    val = amt.ToString().Split('.')[0];
                    amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val));
                }
                else
                {
                    amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(Amount));
                }

                string Pointamt = string.Format("{0:0.00}", Amount.ToString("0.00"));
                val = "";
                if (Pointamt.IndexOf('.') > 0)
                {
                    val = Pointamt.ToString().Split('.')[1];
                    if (Convert.ToInt32(val) > 0)
                    {
                        paise = ds.Tables[0].Rows[0]["Currencytypeps"] + " " + ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val)) + " ";
                    }
                }
                amountinwords = ds.Tables[0].Rows[0]["CIF"] + " " + amountinwords + " " + paise + "Only";
                sht.Range("C" + i).Value = amountinwords.ToUpper();
                sht.Range("C" + i + ":J" + (i + 1)).Style.Font.FontName = "Tahoma";
                sht.Range("C" + i + ":J" + (i + 1)).Style.Font.FontSize = 12;
                sht.Range("C" + i + ":J" + (i + 1)).Style.NumberFormat.Format = "@";
                sht.Range("C" + i + ":J" + (i + 1)).Style.Font.Bold = true;
                sht.Range("C" + i + ":J" + (i + 1)).Style.Alignment.WrapText = true;
                sht.Range("C" + i + ":J" + (i + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("C" + i + ":J" + (i + 1)).Merge();
                //*****Total
                sht.Range("K" + i).Value = "Total";
                sht.Range("K" + i).Style.Font.FontName = "Tahoma";
                sht.Range("K" + i).Style.Font.FontSize = 12;
                sht.Range("K" + i).Style.NumberFormat.Format = "@";
                sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //Amount
                sht.Range("L" + i).SetValue(Amount);
                sht.Range("L" + i).Style.Font.FontName = "Tahoma";
                sht.Range("L" + i).Style.Font.FontSize = 12;
                sht.Range("L" + i).Style.NumberFormat.Format = "@";
                sht.Range("L" + i).Style.Font.Bold = true;
                sht.Range("L" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("L" + i).Style.NumberFormat.Format = "#,###0.00";
                //
                sht.Range("A" + i + ":A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("L" + i + ":L" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                var _with37 = sht.Range("L" + i);
                _with37.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with37.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with37.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                _with37.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                //border
                var _with26 = sht.Range("A" + i + ":L" + i);
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
                sht.Range("C" + i).SetValue(ds.Tables[0].Rows[0]["Noofrolls"]);
                sht.Range("C" + (i + 1)).SetValue(Pcs);
                sht.Range("C" + (i + 2)).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["grosswt"]) + " Kgs.";
                sht.Range("C" + (i + 3)).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["netwt"]) + " Kgs.";
                sht.Range("C" + (i + 4)).Value = String.Format("{0:#,0.00}", Area) + " Sq.Mtr";
                sht.Range("C" + (i + 5)).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["Volume"]) + " CBM";
                //******Central Excise
                //sht.Range("F" + i).Value = "Central Excise No. AADFE8796LEM001";
                //sht.Range("F" + (i + 1)).Value = "Range: Bhadohi";
                //sht.Range("F" + (i + 2)).Value = "Division: Allahabad-I";
                //sht.Range("F" + (i + 3)).Value = "Commissionerate: Allahabad";
                //sht.Range("F" + (i + 4)).Value = "Tariff Heading# 570201";
                //sht.Range("F" + i + ":H" + (i + 4)).Style.Font.FontName = "Tahoma";
                //sht.Range("F" + i + ":H" + (i + 4)).Style.Font.FontSize = 12;
                //sht.Range("F" + i + ":H" + (i + 4)).Style.NumberFormat.Format = "@";
                //sht.Range("F" + i + ":H" + (i + 4)).Style.Font.Bold = true;
                //sht.Range("F" + i + ":H" + i).Merge();
                //sht.Range("F" + (i + 1) + ":H" + (i + 1)).Merge();
                //sht.Range("F" + (i + 2) + ":H" + (i + 2)).Merge();
                //sht.Range("F" + (i + 3) + ":H" + (i + 3)).Merge();
                //sht.Range("F" + (i + 4) + ":H" + (i + 4)).Merge();
                sht.Range("F" + i + ":L" + (i + 2)).Merge();
                sht.Range("F" + i).SetValue("HSN CODE# " + ds.Tables[0].Rows[0]["hsncode"]);
                sht.Range("F" + i + ":L" + (i + 2)).Style.Font.FontName = "Tahoma";
                sht.Range("F" + i + ":L" + (i + 2)).Style.Font.FontSize = 12;
                sht.Range("F" + i + ":L" + (i + 2)).Style.NumberFormat.Format = "@";
                sht.Range("F" + i + ":L" + (i + 2)).Style.Font.Bold = true;
                sht.Range("F" + i + ":L" + (i + 2)).Style.Alignment.WrapText = true;
                sht.Range("F" + i + ":L" + (i + 2)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                sht.Range("A" + i + ":A" + (i + 5)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("L" + i + ":L" + (i + 5)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //***********Sig and date
                i = i + 6;
                //i = 70;
                sht.Range("A" + i + ":H" + i).Merge();
                sht.Range("I" + i).Value = "Signature/Date";
                sht.Range("I" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i).Style.Font.FontSize = 12;
                sht.Range("I" + i).Style.NumberFormat.Format = "@";

                sht.Range("L" + i).Value = "For " + ds.Tables[0].Rows[0]["companyname"];
                sht.Range("J" + i + ":L" + i).Style.Font.FontName = "Tahoma";
                sht.Range("J" + i + ":L" + i).Style.Font.FontSize = 9;
                sht.Range("J" + i + ":L" + i).Style.NumberFormat.Format = "@";
                sht.Range("J" + i + ":L" + i).Style.Font.Bold = true;
                sht.Range("J" + i + ":L" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //sht.Range("J" + i + ":K" + i).Merge();
                var _with38 = sht.Range("I" + i + ":L" + i);
                _with38.Style.Border.TopBorder = XLBorderStyleValues.Thin;

                sht.Range("I" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                sht.Range("A" + i + ":A" + (i + 3)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("L" + i + ":L" + (i + 3)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
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
                sht.Range("I" + i + ":J" + i).Merge();
                sht.Range("I" + i + ":J" + i).Value = ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("I" + i + ":J" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i + ":J" + i).Style.Font.FontSize = 12;
                sht.Range("I" + i + ":J" + i).Style.NumberFormat.Format = "dd/MMM/yyyy";
                sht.Range("I" + i + ":J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("L" + i).Value = "Auth Sign";
                sht.Range("L" + i).Style.Font.FontName = "Tahoma";
                sht.Range("L" + i).Style.Font.FontSize = 12;
                sht.Range("L" + i).Style.NumberFormat.Format = "@";
                sht.Range("L" + i).Style.Font.Bold = true;
                sht.Range("L" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //
                var _with39 = sht.Range("I" + i);
                _with39.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i + ":L" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //

                //*****************Set Borders
                //var _with1 = sht.Range("A2:K" + i);
                //_with1.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //_with1.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //_with1.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //_with1.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //************************
                var _with2 = sht.Range("A11:L11");
                _with2.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with3 = sht.Range("G3:L4");
                _with3.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with3.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                var _with41 = sht.Range("G3:L3");
                _with41.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                var _with40 = sht.Range("G4:L4");
                _with40.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with4 = sht.Range("G5:I6");
                _with4.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with4.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                var _with42 = sht.Range("G5:I5");
                _with42.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                var _with43 = sht.Range("G6:I6");
                _with43.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                //
                var _with5 = sht.Range("J5:L5");
                _with5.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                var _with44 = sht.Range("J5:L6");
                _with44.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _with45 = sht.Range("J6:L6");
                _with45.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with6 = sht.Range("G7:L8");
                _with6.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with6.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                var _with46 = sht.Range("G8:L8");
                _with46.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with7 = sht.Range("G9:L11");
                _with7.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with7.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                var _with47 = sht.Range("G12:L25");
                _with47.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("G12:I12").Style.Border.RightBorder = XLBorderStyleValues.None;

                //
                var _with8 = sht.Range("A24:F24");
                _with8.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                var _with48 = sht.Range("A12:F24");
                _with48.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                sht.Range("A12:B12").Style.Border.RightBorder = XLBorderStyleValues.None;
                sht.Range("A18:B18").Style.Border.RightBorder = XLBorderStyleValues.None;
                sht.Range("G12:H12").Style.Border.RightBorder = XLBorderStyleValues.None;
                sht.Range("G18:H20").Style.Border.RightBorder = XLBorderStyleValues.None;

                //
                var _with9 = sht.Range("A25:C26");
                _with9.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with9.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //sht.Range("A24:C24").Style.Border.BottomBorder = XLBorderStyleValues.None;

                sht.Range("A26:C26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                sht.Range("G26:I26").Style.Border.BottomBorder = XLBorderStyleValues.None;

                sht.Range("J26:L26").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //
                var _with10 = sht.Range("D25:F26");
                _with10.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with10.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D25:F25").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //
                var _with11 = sht.Range("A27:C28");
                _with11.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with11.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with11.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A27:C27").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //
                var _with12 = sht.Range("D27:F28");
                _with12.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with12.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D27:F27").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //
                var _with13 = sht.Range("A29:C30");
                _with13.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with13.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A29:C29").Style.Border.BottomBorder = XLBorderStyleValues.None;
                sht.Range("A30:C30").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with14 = sht.Range("D29:F30");
                _with14.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with14.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D29:G29").Style.Border.BottomBorder = XLBorderStyleValues.None;
                sht.Range("G29").Style.Border.BottomBorder = XLBorderStyleValues.None;

                //
                var _with15 = sht.Range("G26:I27");
                _with15.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with15.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("G26:I26").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("G27:I27").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with16 = sht.Range("J26:L27");
                _with16.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("J26:L26").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("J27:L27").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with17 = sht.Range("G28:L30");
                _with17.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with17.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("G30:L30").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("G29:G30").Style.Border.RightBorder = XLBorderStyleValues.None;
                sht.Range("H29:L30").Style.Border.LeftBorder = XLBorderStyleValues.None;
                //
                var _with18 = sht.Range("I31:I38");
                _with18.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with18.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                var _with19 = sht.Range("J31:J38");
                _with19.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with19.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                var _with119 = sht.Range("K31:K38");
                _with119.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with119.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                var _with20 = sht.Range("A39:L39");
                _with20.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                _with20.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with21 = sht.Range("I39:I39");
                _with21.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with21.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                var _with22 = sht.Range("J39:J39");
                _with22.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with22.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                var _with222 = sht.Range("K39:K39");
                _with222.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with222.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                var _with223 = sht.Range("L39:L39");
                _with223.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with223.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //
                sht.Range("A31:A38").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("L31:L38").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A39:A39").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("L38:L38").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A2:L2").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A3:A24").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                string Fileextension = "xlsx";
                string filename = "Invoice-" + validateFilename(ds.Tables[0].Rows[0]["TinvoiceNo"].ToString()) + "." + Fileextension + "";
                Path = Server.MapPath("~/InvoiceExcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
                lblmsg.Text = "Invoice Excel Format downloaded successfully.";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "AltR1", "alert('No Record Found to Export')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
    protected void Exporttoexcel_Packing()  
    {
        lblmsg.Text = "Wait For Excel File to Download";
        int i = 0, Pcs = 0, TotalRolls = 0;
        Decimal Area = 0;
        string Path = "";
        if (!Directory.Exists(Server.MapPath("~/PackingExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/PackingExcel/"));
        }
        try
        {
            string str = @"select *,VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1) as Pcsperroll,VD.Pcs/(VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1)) as TotalRoll From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue + " order by MinrollNo";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("PackingList");
                //************set cell width
                //Page
                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                sht.PageSetup.AdjustTo(71);
                //sht.PageSetup.FitToPages(1, 5);                
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                sht.PageSetup.VerticalDpi = 300;
                sht.PageSetup.HorizontalDpi = 300;
                //
                sht.PageSetup.Margins.Top = 0.55;
                sht.PageSetup.Margins.Left = 0;
                sht.PageSetup.Margins.Right = 0;
                sht.PageSetup.Margins.Bottom = 0.53;
                sht.PageSetup.Margins.Header = 0.5;
                sht.PageSetup.Margins.Footer = 0.5;
                sht.PageSetup.CenterHorizontally = true;
                sht.PageSetup.SetScaleHFWithDocument();
                //************
                sht.Column("A").Width = 10.29;
                sht.Column("B").Width = 8.43;
                sht.Column("C").Width = 8.43;
                sht.Column("D").Width = 11.00;
                sht.Column("E").Width = 9.71;
                sht.Column("F").Width = 12.57;
                sht.Column("G").Width = 10.14;
                sht.Column("H").Width = 13.14;
                sht.Column("I").Width = 11.71;
                sht.Column("J").Width = 14.29;
                sht.Column("K").Width = 19.29;
                //************
                sht.Row(1).Height = 15.75;
                //********Header
                sht.Range("A1").Value = "PACKING LIST";
                sht.Range("A1:K1").Style.Font.FontName = "Tahoma";
                sht.Range("A1:K1").Style.Font.FontSize = 12;
                sht.Range("A1:K1").Style.Font.Bold = true;
                sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:K1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1:K1").Style.NumberFormat.Format = "@";
                sht.Range("A1:K1").Merge();
                //********Exporter
                sht.Range("A2").Value = "Exporter";
                sht.Range("A2:F2").Style.Font.FontSize = 12;
                sht.Range("A2:F2").Style.Font.FontName = "Tahoma";
                sht.Range("A2:F2").Style.NumberFormat.Format = "@";
                sht.Range("A2:F2").Merge();

                //CompanyName
                sht.Range("A3").Value = ds.Tables[0].Rows[0]["companyName"];
                sht.Range("A3:F3").Style.Font.FontSize = 12;
                sht.Range("A3:F3").Style.Font.FontName = "Tahoma";
                sht.Range("A3:F3").Style.NumberFormat.Format = "@";
                sht.Range("A3:F3").Style.Font.Bold = true;
                sht.Range("A3:F3").Merge();
                //Address
                sht.Range("A4").Value = ds.Tables[0].Rows[0]["compaddr1"];
                sht.Range("A4:F4").Style.Font.FontSize = 12;
                sht.Range("A4:F4").Style.Font.FontName = "Tahoma";
                sht.Range("A4:F4").Style.NumberFormat.Format = "@";
                sht.Range("A4:F4").Merge();
                //address2
                sht.Range("A5").Value = ds.Tables[0].Rows[0]["compaddr2"] + "," + ds.Tables[0].Rows[0]["compaddr3"];
                sht.Range("A5:F5").Style.Font.FontSize = 12;
                sht.Range("A5:F5").Style.Font.FontName = "Tahoma";
                sht.Range("A5:F5").Style.NumberFormat.Format = "@";
                sht.Range("A5:F5").Merge();
                //TiN No
                sht.Range("A6").Value = "GSTIN#" + ds.Tables[0].Rows[0]["gstno"];
                sht.Range("A6:F6").Style.Font.FontSize = 12;
                sht.Range("A6:F6").Style.Font.FontName = "Tahoma";
                sht.Range("A6:F6").Style.NumberFormat.Format = "@";
                sht.Range("A6:F6").Style.Font.Bold = true;
                sht.Range("A6:F6").Merge();
                //**********INvoiceNodate
                sht.Range("G2").Value = "Invoice No./Date";
                sht.Range("G2:K2").Style.Font.FontSize = 12;
                sht.Range("G2:K2").Style.Font.FontName = "Tahoma";
                sht.Range("G2:K2").Style.NumberFormat.Format = "@";

                sht.Range("G2:K2").Merge();
                //value
                sht.Range("G3").Value = ds.Tables[0].Rows[0]["TInvoiceNo"] + " Dated " + ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("G3:K3").Style.Font.FontSize = 12;
                sht.Range("G3:K3").Style.Font.FontName = "Tahoma";
                sht.Range("G3:K3").Style.NumberFormat.Format = "@";
                sht.Range("G3:K3").Merge();
                sht.Range("G3", "K3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //*************IE Code
                sht.Range("G4").Value = "IE Code No.";
                sht.Range("G4:I4").Style.Font.FontSize = 12;
                sht.Range("G4:I4").Style.Font.FontName = "Tahoma";
                sht.Range("G4:I4").Style.NumberFormat.Format = "@";
                sht.Range("G4:I4").Merge();
                //value
                sht.Range("G5").Value = ds.Tables[0].Rows[0]["IEcode"];
                sht.Range("G5:I5").Style.Font.FontSize = 12;
                sht.Range("G5:I5").Style.Font.FontName = "Tahoma";
                sht.Range("G5:I5").Style.NumberFormat.Format = "@";
                sht.Range("G5:I5").Merge();
                sht.Range("G5", "I5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //***********GRI Form No
                sht.Range("J4").Value = "GRI Form No.";
                sht.Range("J4:K4").Style.Font.FontSize = 12;
                sht.Range("J4:K4").Style.Font.FontName = "Tahoma";
                sht.Range("J4:K4").Style.NumberFormat.Format = "@";
                sht.Range("J4:K4").Merge();
                // value
                sht.Range("J5").Value = "";
                sht.Range("J5:K5").Style.Font.FontSize = 12;
                sht.Range("J5:K5").Style.Font.FontName = "Tahoma";
                sht.Range("J5:K5").Style.NumberFormat.Format = "@";
                sht.Range("J5:K5").Merge();
                sht.Range("J5", "K5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //*************Buyer's Order No and Date
                sht.Range("G6").Value = "Buyer's Order No. / Date";
                sht.Range("G6:K6").Style.Font.FontSize = 12;
                sht.Range("G6:K6").Style.Font.FontName = "Tahoma";
                sht.Range("G6:K6").Style.NumberFormat.Format = "@";
                sht.Range("G6:K6").Merge();
                //value
                sht.Range("G7").SetValue(ds.Tables[0].Rows[0]["TorderNo"]);
                sht.Range("G7:K7").Style.Font.FontSize = 12;
                sht.Range("G7:K7").Style.Font.FontName = "Tahoma";
                sht.Range("G7:K7").Style.NumberFormat.Format = "@";
                sht.Range("G7:K7").Merge();
                sht.Range("G7", "K7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //**************Other Reference(s)
                sht.Range("G8").Value = "Other Reference(s)";
                sht.Range("G9").Value = "CONSIGNMENT NO.:" + ds.Tables[0].Rows[0]["otherref"];
                sht.Range("G10").Value = "SUPPLIER NO.:";
                sht.Range("G8:G10").Style.Font.FontSize = 12;
                sht.Range("G8:G10").Style.Font.FontName = "Tahoma";
                sht.Range("G8:G10").Style.NumberFormat.Format = "@";
                sht.Range("G8:K8").Merge();
                sht.Range("G9:K9").Merge();
                sht.Range("G10:K10").Merge();
                //*************Consignee
                sht.Range("A11").Value = "Consignee";
                sht.Range("C11").Value = ds.Tables[0].Rows[0]["consignee_dt"];
                sht.Range("A11:B11").Style.Font.FontSize = 12;
                sht.Range("A11:B11").Style.Font.FontName = "Tahoma";
                sht.Range("A11:B11").Style.NumberFormat.Format = "@";


                sht.Range("C11:F11").Style.Font.FontSize = 12;
                sht.Range("C11:F11").Style.Font.FontName = "Tahoma";
                sht.Range("C11:F11").Style.NumberFormat.Format = "@";
                sht.Range("C11:F11").Style.Font.Bold = true;

                sht.Range("A11:B11").Merge();
                sht.Range("C11:F11").Merge();
                //value
                sht.Range("A12").Value = ds.Tables[0].Rows[0]["consignee"];
                sht.Range("A12:F12").Style.Font.FontSize = 12;
                sht.Range("A12:F12").Style.Font.FontName = "Tahoma";
                sht.Range("A12:F12").Style.NumberFormat.Format = "@";
                sht.Range("A12:F12").Style.Font.Bold = true;
                sht.Range("A12:F12").Merge();
                //**
                sht.Range("A13").Value = ds.Tables[0].Rows[0]["consignee_address"];
                sht.Range("A13:F16").Style.Font.FontSize = 12;
                sht.Range("A13:F16").Style.Font.FontName = "Tahoma";
                sht.Range("A13:F16").Style.NumberFormat.Format = "@";

                sht.Range("A13", "F16").Style.Alignment.WrapText = true;
                sht.Range("A13", "F16").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A13:F16").Merge();
                //***********Notify
                sht.Range("A17").Value = "Notify Party";
                sht.Range("C17").Value = ds.Tables[0].Rows[0]["Notifyparty_dt"];
                sht.Range("A17:B17").Style.Font.FontSize = 12;
                sht.Range("A17:B17").Style.Font.FontName = "Tahoma";
                sht.Range("A17:B17").Style.NumberFormat.Format = "@";

                sht.Range("A17:B17").Style.Font.Underline = XLFontUnderlineValues.Single;
                sht.Range("C17:F17").Style.Font.FontSize = 12;
                sht.Range("C17:F17").Style.Font.FontName = "Tahoma";
                sht.Range("C17:F17").Style.NumberFormat.Format = "@";
                sht.Range("C17:F17").Style.Font.Bold = true;

                sht.Range("A17:B17").Merge();
                sht.Range("C17:F17").Merge();
                //value
                sht.Range("A18").Value = ds.Tables[0].Rows[0]["Notifyparty"];
                sht.Range("A18:F18").Style.Font.FontSize = 12;
                sht.Range("A18:F18").Style.Font.FontName = "Tahoma";
                sht.Range("A18:F18").Style.NumberFormat.Format = "@";
                sht.Range("A18:F18").Style.Font.Bold = true;

                sht.Range("A18:F18").Merge();

                sht.Range("A19").Value = ds.Tables[0].Rows[0]["notifyparty_address"];
                sht.Range("A19:F23").Style.Font.FontSize = 12;
                sht.Range("A19:F23").Style.Font.FontName = "Tahoma";
                sht.Range("A19:F23").Style.NumberFormat.Format = "@";

                sht.Range("A19", "F23").Style.Alignment.WrapText = true;
                sht.Range("A19", "F23").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A19:F23").Merge();
                //***********Receiver address
                sht.Range("G11").Value = "Receiver Address";
                sht.Range("G11:I11").Style.Font.FontSize = 12;
                sht.Range("G11:I11").Style.Font.FontName = "Tahoma";
                sht.Range("G11:I11").Style.NumberFormat.Format = "@";
                sht.Range("G11:I11").Merge();
                //values
                sht.Range("J11").Value = ds.Tables[0].Rows[0]["Destcode"];
                sht.Range("J11:K11").Style.Font.FontSize = 12;
                sht.Range("J11:K11").Style.Font.FontName = "Tahoma";
                sht.Range("J11:K11").Style.NumberFormat.Format = "@";
                sht.Range("J11:K11").Style.Font.Bold = true;

                sht.Range("J11:K11").Merge();
                //****** 1.
                sht.Range("G12").Value = "1. " + ds.Tables[0].Rows[0]["Receiver"];
                sht.Range("G12:K12").Style.Font.FontSize = 12;
                sht.Range("G12:K12").Style.Font.FontName = "Tahoma";
                sht.Range("G12:K12").Style.NumberFormat.Format = "@";
                sht.Range("G12:K12").Style.Font.Bold = true;
                sht.Range("G12:K12").Merge();
                //*
                sht.Range("G13").Value = ds.Tables[0].Rows[0]["Receiver_address"];
                sht.Range("G13:K15").Style.Font.FontSize = 12;
                sht.Range("G13:K15").Style.Font.FontName = "Tahoma";
                sht.Range("G13:K15").Style.NumberFormat.Format = "@";

                sht.Range("G13:K15").Style.Alignment.WrapText = true;
                sht.Range("G13:K15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("G13:K15").Merge();
                //****** 2.
                sht.Range("G16").Value = "2.Paying Agent : " + ds.Tables[0].Rows[0]["payingagent"];
                sht.Range("G16:K16").Style.Font.FontSize = 12;
                sht.Range("G16:K16").Style.Font.FontName = "Tahoma";
                sht.Range("G16:K16").Style.NumberFormat.Format = "@";
                sht.Range("G16:K16").Style.Font.Bold = true;

                sht.Range("G16:K16").Merge();
                //*
                sht.Range("I17").Value = ds.Tables[0].Rows[0]["payingagent_address"];
                sht.Range("I17:K19").Style.Font.FontSize = 12;
                sht.Range("I17:K19").Style.Font.FontName = "Tahoma";
                sht.Range("I17:K19").Style.NumberFormat.Format = "@";

                sht.Range("I17:K19").Style.Alignment.WrapText = true;
                sht.Range("I17:K19").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("I17:K19").Merge();
                //*******3.
                sht.Range("G20").Value = "Buyer (If other than Consignee)";
                sht.Range("G20:K20").Style.Font.FontSize = 12;
                sht.Range("G20:K20").Style.Font.FontName = "Tahoma";
                sht.Range("G20:K20").Style.NumberFormat.Format = "@";
                sht.Range("G20:K20").Merge();

                sht.Range("G21").Value = "3." + ds.Tables[0].Rows[0]["buyer_otherthanconsignee"];
                sht.Range("G21:K21").Style.Font.FontSize = 12;
                sht.Range("G21:K21").Style.Font.FontName = "Tahoma";
                sht.Range("G21:K21").Style.NumberFormat.Format = "@";
                sht.Range("G21:K21").Style.Font.Bold = true;

                sht.Range("G21:K21").Merge();
                //*
                sht.Range("G22").Value = ds.Tables[0].Rows[0]["otherthanconsignee_address"];
                sht.Range("G22:K27").Style.Font.FontSize = 12;
                sht.Range("G22:K27").Style.Font.FontName = "Tahoma";
                sht.Range("G22:K27").Style.NumberFormat.Format = "@";
                sht.Range("G22:K27").Style.Alignment.WrapText = true;
                sht.Range("G22:K27").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("G22:K27").Merge();
                //***********Pre-carriage By
                sht.Range("A24").Value = "Pre-Carriage By";
                sht.Range("A24:C24").Style.Font.FontSize = 12;
                sht.Range("A24:C24").Style.Font.FontName = "Tahoma";
                sht.Range("A24:C24").Style.NumberFormat.Format = "@";
                sht.Range("A24:C24").Merge();
                //value
                sht.Range("A25").Value = ds.Tables[0].Rows[0]["Pre_carriageby"];
                sht.Range("A25:C25").Style.Font.FontSize = 12;
                sht.Range("A25:C25").Style.Font.FontName = "Tahoma";
                sht.Range("A25:C25").Style.NumberFormat.Format = "@";
                sht.Range("A25:C25").Merge();
                sht.Range("A25:C25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //************Place of Receipt by Pre-Carrier
                sht.Range("D24").Value = "Place of Receipt by Pre-Carrier";
                sht.Range("D24:F24").Style.Font.FontSize = 12;
                sht.Range("D24:F24").Style.Font.FontName = "Tahoma";
                sht.Range("D24:F24").Style.NumberFormat.Format = "@";
                sht.Range("D24:F24").Merge();
                //value
                sht.Range("D25").Value = ds.Tables[0].Rows[0]["receiptby"];
                sht.Range("D25:F25").Style.Font.FontSize = 12;
                sht.Range("D25:F25").Style.Font.FontName = "Tahoma";
                sht.Range("D25:F25").Style.NumberFormat.Format = "@";
                sht.Range("D25:F25").Merge();
                sht.Range("D25:F25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Vessel/Flight No
                sht.Range("A26").Value = "Vessel/Flight No";
                sht.Range("A26:C26").Style.Font.FontSize = 12;
                sht.Range("A26:C26").Style.Font.FontName = "Tahoma";
                sht.Range("A26:C26").Style.NumberFormat.Format = "@";
                sht.Range("A26:C26").Merge();
                //value
                sht.Range("A27").Value = ds.Tables[0].Rows[0]["vessel_flightno"];
                sht.Range("A27:C27").Style.Font.FontSize = 12;
                sht.Range("A27:C27").Style.Font.FontName = "Tahoma";
                sht.Range("A27:C27").Style.NumberFormat.Format = "@";
                sht.Range("A27:C27").Merge();
                sht.Range("A27:C27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Loading
                sht.Range("D26").Value = "Port of Loading";
                sht.Range("D26:F26").Style.Font.FontSize = 12;
                sht.Range("D26:F26").Style.Font.FontName = "Tahoma";
                sht.Range("D26:F26").Style.NumberFormat.Format = "@";
                sht.Range("D26:F26").Merge();
                //value
                sht.Range("D27").Value = ds.Tables[0].Rows[0]["portload"];
                sht.Range("D27:F27").Style.Font.FontSize = 12;
                sht.Range("D27:F27").Style.Font.FontName = "Tahoma";
                sht.Range("D27:F27").Style.NumberFormat.Format = "@";
                sht.Range("D27:F27").Merge();
                sht.Range("D27:F27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Discharge
                sht.Range("A28").Value = "Port of Discharge";
                sht.Range("A28:C28").Style.Font.FontSize = 12;
                sht.Range("A28:C28").Style.Font.FontName = "Tahoma";
                sht.Range("A28:C28").Style.NumberFormat.Format = "@";
                sht.Range("A28:C28").Merge();
                //value
                sht.Range("A29").Value = ds.Tables[0].Rows[0]["portunload"];
                sht.Range("A29:C29").Style.Font.FontSize = 12;
                sht.Range("A29:C29").Style.Font.FontName = "Tahoma";
                sht.Range("A29:C29").Style.NumberFormat.Format = "@";
                sht.Range("A29:C29").Merge();
                sht.Range("A29:C29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Final Destination
                sht.Range("D28").Value = "Final Destination";
                sht.Range("D28:F28").Style.Font.FontSize = 12;
                sht.Range("D28:F28").Style.Font.FontName = "Tahoma";
                sht.Range("D28:F28").Style.NumberFormat.Format = "@";
                sht.Range("D28:F28").Merge();
                //value
                sht.Range("D29").Value = ds.Tables[0].Rows[0]["Destinationadd"];
                sht.Range("D29:F29").Style.Font.FontSize = 12;
                sht.Range("D29:F29").Style.Font.FontName = "Tahoma";
                sht.Range("D29:F29").Style.NumberFormat.Format = "@";
                sht.Range("D29:F29").Merge();
                sht.Range("D29:F29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Origin of Goods
                sht.Range("G28").Value = "Country of Origin of Goods";
                sht.Range("G28:I28").Style.Font.FontSize = 12;
                sht.Range("G28:I28").Style.Font.FontName = "Tahoma";
                sht.Range("G28:I28").Style.NumberFormat.Format = "@";
                sht.Range("G28:I28").Merge();
                //value
                sht.Range("G29").Value = ds.Tables[0].Rows[0]["countryoforigin"];
                sht.Range("G29:I29").Style.Font.FontSize = 12;
                sht.Range("G29:I29").Style.Font.FontName = "Tahoma";
                sht.Range("G29:I29").Style.NumberFormat.Format = "@";
                sht.Range("G29:I29").Merge();
                sht.Range("G29:I29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Final Destination
                sht.Range("J28").Value = "Country of Final Destination";
                sht.Range("J28:K28").Style.Font.FontSize = 12;
                sht.Range("J28:K28").Style.Font.FontName = "Tahoma";
                sht.Range("J28:K28").Style.NumberFormat.Format = "@";
                sht.Range("J28:K28").Merge();
                //value
                sht.Range("J29").Value = ds.Tables[0].Rows[0]["countryoffinaldest"];
                sht.Range("J29:K29").Style.Font.FontSize = 12;
                sht.Range("J29:K29").Style.Font.FontName = "Tahoma";
                sht.Range("J29:K29").Style.NumberFormat.Format = "@";
                sht.Range("J29:K29").Merge();
                sht.Range("J29:K29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //*****************Nos
                sht.Range("A30").Value = "Nos.";
                sht.Range("B30").Value = "No and kind of Packages";
                sht.Range("B30:D30").Merge();

                sht.Range("E30").Value = "Description of Goods";
                sht.Range("E30:J30").Merge();
                //sht.Range("k30").Value = "Quantity";                
                sht.Range("A30:K30").Style.Font.FontSize = 12;
                sht.Range("A30:K30").Style.Font.FontName = "Tahoma";
                sht.Range("A30:K30").Style.NumberFormat.Format = "@";
                sht.Range("K30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //values
                sht.Range("A31:H31").Merge();
                sht.Range("A32:H36").Style.Font.FontSize = 12;
                sht.Range("A32:H36").Style.Font.FontName = "Tahoma";
                sht.Range("A32:H36").Style.NumberFormat.Format = "@";
                sht.Range("A32").Value = ds.Tables[0].Compute("Min(MinrollNo)", "") + "/" + ds.Tables[0].Compute("Max(Maxrollno)", "");
                sht.Range("B32").Value = ds.Tables[0].Rows[0]["Noofrolls"] + " Packages";
                sht.Range("B32:C32").Merge();
                sht.Range("E32").Value = ds.Tables[0].Rows[0]["descriptionofgoods"];
                sht.Range("E32:J36").Style.Alignment.WrapText = true;
                sht.Range("E32:J36").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("E32:J36").Merge();
                sht.Range("A37:J37").Merge();
                //            //**********************Details
                //********Hader
                sht.Range("A38").Value = "Roll No";
                sht.Range("A38:K38").Style.Font.FontSize = 12;
                sht.Range("A38:K38").Style.Font.FontName = "Tahoma";
                sht.Range("A38:K38").Style.NumberFormat.Format = "@";
                sht.Range("A38:K38").Style.Font.Bold = true;
                sht.Range("A38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B38").Value = "Quality";
                sht.Range("B38:C38").Merge();
                sht.Range("D38").Value = "ART.NO.";
                //sht.Range("D38:E38")..Merge();
                sht.Range("E38").Value = "COLOR";
                sht.Range("F38").Value = "SIZE(CM)";
                sht.Range("F38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("G38").Value = "Pcs/Roll";
                sht.Range("G38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H38").Value = "Total Rolls";
                sht.Range("H38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("I38").Value = "Total Pcs";
                sht.Range("J38").Value = "Area Sq. Mtr";
                sht.Range("K38").Value = "P.O.#";
                sht.Range("K38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A38:K38").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A38").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K38").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A38:K38").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //***********generate Loop
                i = 39;
                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                {
                    sht.Range("A" + i, "K" + i).Style.Font.FontSize = 12;
                    sht.Range("A" + i, "K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("A" + i, "I" + i).Style.NumberFormat.Format = "@";
                    sht.Range("K" + i).Style.Font.FontSize = 12;
                    sht.Range("K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("K" + i).Style.NumberFormat.Format = "@";
                    //pono
                    sht.Range("A" + i).Value = ds.Tables[0].Rows[ii]["Minrollno"] + "  " + ds.Tables[0].Rows[ii]["Maxrollno"];
                    sht.Range("A" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //Article Name
                    sht.Range("B" + i).Value = ds.Tables[0].Rows[ii]["Design"];
                    sht.Range("B" + i, "C" + i).Merge();
                    //Art No.
                    sht.Range("D" + i).Value = ds.Tables[0].Rows[ii]["articleno"];
                    //Colour
                    sht.Range("E" + i).Value = ds.Tables[0].Rows[ii]["Color"];
                    //Size
                    sht.Range("F" + i).Value = ds.Tables[0].Rows[ii]["width"] + "x" + ds.Tables[0].Rows[ii]["Length"];
                    sht.Range("F" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //pcs/roll
                    sht.Range("G" + i).SetValue(ds.Tables[0].Rows[ii]["pcsperroll"]);
                    sht.Range("G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("G" + i).Style.NumberFormat.Format = "@";
                    //Total rolls
                    sht.Range("H" + i).SetValue(ds.Tables[0].Rows[ii]["totalroll"]);
                    sht.Range("H" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("H" + i).Style.NumberFormat.Format = "@";
                    TotalRolls = TotalRolls + Convert.ToInt16(ds.Tables[0].Rows[ii]["totalroll"]);
                    //Qty
                    sht.Range("I" + i).SetValue(ds.Tables[0].Rows[ii]["Pcs"]);
                    sht.Range("I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[ii]["Pcs"]);
                    sht.Range("I" + i).Style.NumberFormat.Format = "@";
                    //Area
                    sht.Range("J" + i).SetValue(ds.Tables[0].Rows[ii]["Area"]);
                    sht.Range("J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                    Area = Decimal.Parse(Area.ToString()) + Decimal.Parse(ds.Tables[0].Rows[ii]["Area"].ToString());
                    //
                    //var _with27 = sht.Range("I" + i);

                    //_with27.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
                    //_with27.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
                    //PO
                    sht.Range("K" + i).Value = ds.Tables[0].Rows[ii]["Pono"];
                    //
                    sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //Borders
                    sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    i = i + 1;
                }
                //
                sht.Range("A" + i + ":A" + (i + 7)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 7)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //End Of loop
                i = i + 7;
                //*************** 

                //
                sht.Range("D" + i).Value = "Packing List Total:";
                sht.Range("D" + i, "F" + i).Style.Font.FontSize = 12;
                sht.Range("D" + i, "F" + i).Style.Font.FontName = "Tahoma";
                sht.Range("D" + i, "F" + i).Style.NumberFormat.Format = "@";
                sht.Range("D" + i, "F" + i).Style.Font.Bold = true;
                sht.Range("D" + i + ":F" + i).Merge();
                sht.Range("D" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //Total Rolls
                sht.Range("H" + i).Value = TotalRolls;
                sht.Range("H" + i).Style.Font.FontSize = 12;
                sht.Range("H" + i).Style.Font.FontName = "Tahoma";
                sht.Range("H" + i).Style.NumberFormat.Format = "@";
                sht.Range("H" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //Total Pcs
                sht.Range("I" + i).Value = Pcs;
                sht.Range("I" + i).Style.Font.FontSize = 12;
                sht.Range("I" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i).Style.NumberFormat.Format = "@";
                sht.Range("I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //Area
                sht.Range("J" + i).Value = Area;
                sht.Range("J" + i).Style.Font.FontSize = 12;
                sht.Range("J" + i).Style.Font.FontName = "Tahoma";
                sht.Range("J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                //
                var _with21 = sht.Range("A" + i + ":K" + i);
                _with21.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                _with21.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                sht.Range("A" + i + ":A" + (i + 10)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 10)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //**********Total
                i = i + 10;
                //i = 63;
                sht.Range("A" + i).Value = "TOTAL ROLLS";
                sht.Range("A" + (i + 1)).Value = "TOTAL PCS";
                sht.Range("A" + (i + 2)).Value = "TOTAL GR. WT";
                sht.Range("A" + (i + 3)).Value = "TOTAL NT. WT";
                sht.Range("A" + (i + 4)).Value = "TOTAL SQ.MTR";
                sht.Range("A" + (i + 5)).Value = "TOTAL VOLUME";
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":A" + (i + 5)).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontName = "Courier New";
                //value                
                sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontSize = 12;
                sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontName = "Tahoma";
                sht.Range("C" + i + ":C" + (i + 5)).Style.NumberFormat.Format = "@";


                sht.Range("C" + i).SetValue(ds.Tables[0].Rows[0]["Noofrolls"]);
                sht.Range("C" + (i + 1)).SetValue(Pcs);
                sht.Range("C" + (i + 2)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["grosswt"]) + " Kgs.");
                sht.Range("C" + (i + 3)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["netwt"]) + " Kgs.");
                sht.Range("C" + (i + 4)).SetValue(String.Format("{0:#,0.00}", Area) + " Sq.Mtr");
                sht.Range("C" + (i + 5)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["Volume"]) + " CBM");
                //
                sht.Range("A" + i + ":A" + (i + 5)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 5)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //***********Sig and date
                i = i + 6;
                //i = 70;
                sht.Range("A" + i + ":H" + i).Merge();
                sht.Range("I" + i).Value = "Signature/Date";
                sht.Range("I" + i).Style.Font.FontSize = 12;
                sht.Range("I" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i).Style.NumberFormat.Format = "@";

                sht.Range("K" + i).Value = "For " + ds.Tables[0].Rows[0]["companyname"];
                sht.Range("J" + i + ":K" + i).Style.Font.FontSize = 12;
                sht.Range("J" + i + ":K" + i).Style.Font.FontName = "Tahoma";
                sht.Range("J" + i + ":K" + i).Style.NumberFormat.Format = "@";
                sht.Range("J" + i + ":K" + i).Style.Font.Bold = true;
                sht.Range("J" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("J" + i + ":K" + i).Style.Font.FontSize = 9;
                //sht.Range("J" + i + ":K" + i).Merge();              
                sht.Range("I" + i + ":K" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("I" + i + ":I" + (i + 3)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 3)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //**************Declaration
                //
                sht.Range("A" + i + ":A" + (i + 2)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                i = i + 1;
                //i = 71;
                sht.Range("A" + i).Value = "Declaration";
                sht.Range("A" + i + ":B" + i).Merge();
                sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":B" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                //value
                i = i + 1;
                sht.Range("A" + i).Value = "We declare that this invoice shows the actual price of the";
                sht.Range("A" + i + ":F" + i).Merge();
                sht.Range("A" + i + ":F" + i).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":F" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":F" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                i = i + 1;
                sht.Range("A" + i).Value = "goods described and that all particulars are true and correct";
                sht.Range("A" + i + ":F" + i).Merge();
                sht.Range("A" + i + ":F" + i).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":F" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":F" + i).Style.NumberFormat.Format = "@";

                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //**********

                sht.Range("I" + i + ":J" + i).Merge();
                sht.Range("I" + i + ":J" + i).Value = ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("I" + i + ":J" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i + ":J" + i).Style.Font.FontSize = 12;
                sht.Range("I" + i + ":J" + i).Style.NumberFormat.Format = "dd/MMM/yyyy";
                sht.Range("I" + i + ":J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("K" + i).Value = "Auth Sign";
                sht.Range("K" + i).Style.Font.FontSize = 12;
                sht.Range("K" + i).Style.Font.FontName = "Tahoma";
                sht.Range("K" + i).Style.NumberFormat.Format = "@";
                sht.Range("K" + i).Style.Font.Bold = true;
                sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //
                var _with39 = sht.Range("I" + i);
                _with39.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //
                sht.Range("A2:A10").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A11:A23").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i + ":K" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A1:K1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //sht.Range("A2:A10").Style.Border.LeftBorder = XLBorderStyleValues.Thin;                
                //sht.Range("F2:F10").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //            //************************
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
                sht.Range("G28:K28").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A30:A37").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K30:K37").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //*******************Save
                string Fileextension = "xlsx";
                string filename = "PackingList -" + validateFilename(ds.Tables[0].Rows[0]["TinvoiceNo"].ToString()) + "." + Fileextension + "";
                Path = Server.MapPath("~/Packingexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
                //*****
                lblmsg.Text = "Packing Excel Format downloaded successfully.";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "AltR2", "alert('No Record Found to Export')", true);
            }

        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void Exporttoexcel_Packing_type2()
    {
        lblmsg.Text = "Wait For Excel File to Download";
        int i = 0, Pcs = 0, TotalRolls = 0;
        Decimal Area = 0;
        string Path = "";
        if (!Directory.Exists(Server.MapPath("~/PackingExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/PackingExcel/"));
        }
        try
        {
            string str = @"select *,VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1) as Pcsperroll,VD.Pcs/(VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1)) as TotalRoll From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue + " order by MinrollNo";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("PackingList");
                //************set cell width
                //Page
                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                sht.PageSetup.AdjustTo(71);
                //sht.PageSetup.FitToPages(1, 5);                
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                sht.PageSetup.VerticalDpi = 300;
                sht.PageSetup.HorizontalDpi = 300;
                //
                sht.PageSetup.Margins.Top = 0.55;
                sht.PageSetup.Margins.Left = 0;
                sht.PageSetup.Margins.Right = 0;
                sht.PageSetup.Margins.Bottom = 0.53;
                sht.PageSetup.Margins.Header = 0.5;
                sht.PageSetup.Margins.Footer = 0.5;
                sht.PageSetup.CenterHorizontally = true;
                sht.PageSetup.SetScaleHFWithDocument();
                //************
                sht.Column("A").Width = 10.29;
                sht.Column("B").Width = 8.43;
                sht.Column("C").Width = 8.43;
                sht.Column("D").Width = 11.00;
                sht.Column("E").Width = 9.71;
                sht.Column("F").Width = 12.57;
                sht.Column("G").Width = 10.14;
                sht.Column("H").Width = 13.14;
                sht.Column("I").Width = 11.71;
                sht.Column("J").Width = 14.29;
                sht.Column("K").Width = 19.29;
                //************
                sht.Row(1).Height = 15.75;
                //********Header
                sht.Range("A1").Value = "PACKING LIST";
                sht.Range("A1:K1").Style.Font.FontName = "Tahoma";
                sht.Range("A1:K1").Style.Font.FontSize = 12;
                sht.Range("A1:K1").Style.Font.Bold = true;
                sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:K1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1:K1").Style.NumberFormat.Format = "@";
                sht.Range("A1:K1").Merge();
                //********Exporter
                sht.Range("A2").Value = "Exporter";
                sht.Range("A2:F2").Style.Font.FontSize = 12;
                sht.Range("A2:F2").Style.Font.FontName = "Tahoma";
                sht.Range("A2:F2").Style.NumberFormat.Format = "@";
                sht.Range("A2:F2").Merge();

                //CompanyName
                sht.Range("A3").Value = ds.Tables[0].Rows[0]["companyName"];
                sht.Range("A3:F3").Style.Font.FontSize = 12;
                sht.Range("A3:F3").Style.Font.FontName = "Tahoma";
                sht.Range("A3:F3").Style.NumberFormat.Format = "@";
                sht.Range("A3:F3").Style.Font.Bold = true;
                sht.Range("A3:F3").Merge();
                //Address
                sht.Range("A4").Value = ds.Tables[0].Rows[0]["compaddr1"];
                sht.Range("A4:F4").Style.Font.FontSize = 12;
                sht.Range("A4:F4").Style.Font.FontName = "Tahoma";
                sht.Range("A4:F4").Style.NumberFormat.Format = "@";
                sht.Range("A4:F4").Merge();
                //address2
                sht.Range("A5").Value = ds.Tables[0].Rows[0]["compaddr2"] + "," + ds.Tables[0].Rows[0]["compaddr3"];
                sht.Range("A5:F5").Style.Font.FontSize = 12;
                sht.Range("A5:F5").Style.Font.FontName = "Tahoma";
                sht.Range("A5:F5").Style.NumberFormat.Format = "@";
                sht.Range("A5:F5").Merge();
                //TiN No
                sht.Range("A6").Value = "GSTIN#" + ds.Tables[0].Rows[0]["gstno"];
                sht.Range("A6:F6").Style.Font.FontSize = 12;
                sht.Range("A6:F6").Style.Font.FontName = "Tahoma";
                sht.Range("A6:F6").Style.NumberFormat.Format = "@";
                sht.Range("A6:F6").Style.Font.Bold = true;
                sht.Range("A6:F6").Merge();
                //**********INvoiceNodate
                sht.Range("G2").Value = "Invoice No./Date";
                sht.Range("G2:K2").Style.Font.FontSize = 12;
                sht.Range("G2:K2").Style.Font.FontName = "Tahoma";
                sht.Range("G2:K2").Style.NumberFormat.Format = "@";

                sht.Range("G2:K2").Merge();
                //value
                sht.Range("G3").Value = ds.Tables[0].Rows[0]["TInvoiceNo"] + " Dated " + ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("G3:K3").Style.Font.FontSize = 12;
                sht.Range("G3:K3").Style.Font.FontName = "Tahoma";
                sht.Range("G3:K3").Style.NumberFormat.Format = "@";
                sht.Range("G3:K3").Merge();
                sht.Range("G3", "K3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //*************IE Code
                sht.Range("G4").Value = "IE Code No.";
                sht.Range("G4:I4").Style.Font.FontSize = 12;
                sht.Range("G4:I4").Style.Font.FontName = "Tahoma";
                sht.Range("G4:I4").Style.NumberFormat.Format = "@";
                sht.Range("G4:I4").Merge();
                //value
                sht.Range("G5").Value = ds.Tables[0].Rows[0]["IEcode"];
                sht.Range("G5:I5").Style.Font.FontSize = 12;
                sht.Range("G5:I5").Style.Font.FontName = "Tahoma";
                sht.Range("G5:I5").Style.NumberFormat.Format = "@";
                sht.Range("G5:I5").Merge();
                sht.Range("G5", "I5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //***********GRI Form No
                sht.Range("J4").Value = "GRI Form No.";
                sht.Range("J4:K4").Style.Font.FontSize = 12;
                sht.Range("J4:K4").Style.Font.FontName = "Tahoma";
                sht.Range("J4:K4").Style.NumberFormat.Format = "@";
                sht.Range("J4:K4").Merge();
                // value
                sht.Range("J5").Value = "";
                sht.Range("J5:K5").Style.Font.FontSize = 12;
                sht.Range("J5:K5").Style.Font.FontName = "Tahoma";
                sht.Range("J5:K5").Style.NumberFormat.Format = "@";
                sht.Range("J5:K5").Merge();
                sht.Range("J5", "K5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //*************Buyer's Order No and Date
                sht.Range("G6").Value = "Buyer's Order No. / Date";
                sht.Range("G6:K6").Style.Font.FontSize = 12;
                sht.Range("G6:K6").Style.Font.FontName = "Tahoma";
                sht.Range("G6:K6").Style.NumberFormat.Format = "@";
                sht.Range("G6:K6").Merge();
                //value
                sht.Range("G7").SetValue(ds.Tables[0].Rows[0]["TorderNo"]);
                sht.Range("G7:K7").Style.Font.FontSize = 12;
                sht.Range("G7:K7").Style.Font.FontName = "Tahoma";
                sht.Range("G7:K7").Style.NumberFormat.Format = "@";
                sht.Range("G7:K7").Merge();
                sht.Range("G7", "K7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //**************Other Reference(s)
                sht.Range("G8").Value = "Other Reference(s)";
                sht.Range("G9").Value = "CONSIGNMENT NO.:" + ds.Tables[0].Rows[0]["otherref"];
                sht.Range("G10").Value = "SUPPLIER NO.:";
                sht.Range("G8:G10").Style.Font.FontSize = 12;
                sht.Range("G8:G10").Style.Font.FontName = "Tahoma";
                sht.Range("G8:G10").Style.NumberFormat.Format = "@";
                sht.Range("G8:K8").Merge();
                sht.Range("G9:K9").Merge();
                sht.Range("G10:K10").Merge();
                //*************Consignee
                sht.Range("A11").Value = "Consignee";
                sht.Range("C11").Value = ds.Tables[0].Rows[0]["destcode"] + "(Ship To)";
                sht.Range("A11:B11").Style.Font.FontSize = 12;
                sht.Range("A11:B11").Style.Font.FontName = "Tahoma";
                sht.Range("A11:B11").Style.NumberFormat.Format = "@";


                sht.Range("C11:F11").Style.Font.FontSize = 12;
                sht.Range("C11:F11").Style.Font.FontName = "Tahoma";
                sht.Range("C11:F11").Style.NumberFormat.Format = "@";
                sht.Range("C11:F11").Style.Font.Bold = true;

                sht.Range("A11:B11").Merge();
                sht.Range("C11:F11").Merge();
                //value
                sht.Range("A12").Value = ds.Tables[0].Rows[0]["Receiver"];
                sht.Range("A12:F12").Style.Font.FontSize = 12;
                sht.Range("A12:F12").Style.Font.FontName = "Tahoma";
                sht.Range("A12:F12").Style.NumberFormat.Format = "@";
                sht.Range("A12:F12").Style.Font.Bold = true;
                sht.Range("A12:F12").Merge();
                //**
                sht.Range("A13").Value = ds.Tables[0].Rows[0]["Receiver_address"];
                sht.Range("A13:F16").Style.Font.FontSize = 12;
                sht.Range("A13:F16").Style.Font.FontName = "Tahoma";
                sht.Range("A13:F16").Style.NumberFormat.Format = "@";

                sht.Range("A13", "F16").Style.Alignment.WrapText = true;
                sht.Range("A13", "F16").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A13:F16").Merge();
                //GSTIN
                sht.Range("A19:D19").Merge();
                sht.Range("A19").SetValue("GSTIN#" + ds.Tables[0].Rows[0]["Rec_gstin"]);
                sht.Range("A19:D19").Style.Font.FontSize = 12;
                sht.Range("A19:D19").Style.Font.FontName = "Tahoma";
                sht.Range("A19:D19").Style.Font.SetBold();
                //***********Receiver address
                sht.Range("G11").Value = "Receiver Address(Invoice receiver address)";
                sht.Range("G11:J11").Style.Font.FontSize = 12;
                sht.Range("G11:J11").Style.Font.FontName = "Tahoma";
                sht.Range("G11:J11").Style.NumberFormat.Format = "@";
                sht.Range("G11:J11").Merge();
                //values
                sht.Range("K11").Value = ds.Tables[0].Rows[0]["Destcode"];
                sht.Range("K11:K11").Style.Font.FontSize = 12;
                sht.Range("K11:K11").Style.Font.FontName = "Tahoma";
                sht.Range("K11:K11").Style.NumberFormat.Format = "@";
                sht.Range("K11:K11").Style.Font.Bold = true;


                //****** 1.
                sht.Range("G12").Value = "1. " + ds.Tables[0].Rows[0]["Invoice_receiver"];
                sht.Range("G12:K12").Style.Font.FontSize = 12;
                sht.Range("G12:K12").Style.Font.FontName = "Tahoma";
                sht.Range("G12:K12").Style.NumberFormat.Format = "@";
                sht.Range("G12:K12").Style.Font.Bold = true;
                sht.Range("G12:K12").Merge();
                //*
                sht.Range("G13").Value = ds.Tables[0].Rows[0]["Invoice_receiveradd"];
                sht.Range("G13:K15").Style.Font.FontSize = 12;
                sht.Range("G13:K15").Style.Font.FontName = "Tahoma";
                sht.Range("G13:K15").Style.NumberFormat.Format = "@";

                sht.Range("G13:K15").Style.Alignment.WrapText = true;
                sht.Range("G13:K15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("G13:K15").Merge();
                //Bill TO
                sht.Range("G18").Value = "Bill To :";
                sht.Range("G18:G19").Style.Font.FontSize = 12;
                sht.Range("G18:G19").Style.Font.FontName = "Tahoma";
                sht.Range("G19").Style.Font.SetBold();

                sht.Range("G19:K19").Merge();
                sht.Range("G19").Value = ds.Tables[0].Rows[0]["Receiver"];

                sht.Range("G20:K22").Merge();
                sht.Range("G20").Value = ds.Tables[0].Rows[0]["Receiver_address"];
                sht.Range("G20:K22").Style.Font.FontSize = 12;
                sht.Range("G20:K22").Style.Font.FontName = "Tahoma";
                sht.Range("G20:K22").Style.Alignment.WrapText = true;
                sht.Range("G20:K22").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //***********Pre-carriage By
                sht.Range("A20").Value = "Pre-Carriage By";
                sht.Range("A20:C20").Style.Font.FontSize = 12;
                sht.Range("A20:C20").Style.Font.FontName = "Tahoma";
                sht.Range("A20:C20").Style.NumberFormat.Format = "@";
                sht.Range("A20:C20").Merge();
                //value
                sht.Range("A21").Value = ds.Tables[0].Rows[0]["Pre_carriageby"];
                sht.Range("A21:C21").Style.Font.FontSize = 12;
                sht.Range("A21:C21").Style.Font.FontName = "Tahoma";
                sht.Range("A21:C21").Style.NumberFormat.Format = "@";
                sht.Range("A21:C21").Merge();
                sht.Range("A21:C21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //************Place of Receipt by Pre-Carrier
                sht.Range("D20").Value = "Place of Receipt by Pre-Carrier";
                sht.Range("D20:F20").Style.Font.FontSize = 12;
                sht.Range("D20:F20").Style.Font.FontName = "Tahoma";
                sht.Range("D20:F20").Style.NumberFormat.Format = "@";
                sht.Range("D20:F20").Merge();
                //value
                sht.Range("D21").Value = "";// ds.Tables[0].Rows[0]["receiptby"];
                sht.Range("D21:F21").Style.Font.FontSize = 12;
                sht.Range("D21:F21").Style.Font.FontName = "Tahoma";
                sht.Range("D21:F21").Style.NumberFormat.Format = "@";
                sht.Range("D21:F21").Merge();
                sht.Range("D21:F21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Vessel/Flight No
                sht.Range("A22").Value = "";
                sht.Range("A22:C22").Style.Font.FontSize = 12;
                sht.Range("A22:C22").Style.Font.FontName = "Tahoma";
                sht.Range("A22:C22").Style.NumberFormat.Format = "@";
                sht.Range("A22:C22").Merge();
                //value
                sht.Range("A23").Value = "";
                sht.Range("A23:C23").Style.Font.FontSize = 12;
                sht.Range("A23:C23").Style.Font.FontName = "Tahoma";
                sht.Range("A23:C23").Style.NumberFormat.Format = "@";
                sht.Range("A23:C23").Merge();
                sht.Range("A23:C23").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Loading
                sht.Range("D22").Value = "";
                sht.Range("D22:F22").Style.Font.FontSize = 12;
                sht.Range("D22:F22").Style.Font.FontName = "Tahoma";
                sht.Range("D22:F22").Style.NumberFormat.Format = "@";
                sht.Range("D22:F22").Merge();
                //value
                sht.Range("D23").Value = "";//ds.Tables[0].Rows[0]["portload"];
                sht.Range("D23:F23").Style.Font.FontSize = 12;
                sht.Range("D23:F23").Style.Font.FontName = "Tahoma";
                sht.Range("D23:F23").Style.NumberFormat.Format = "@";
                sht.Range("D23:F23").Merge();
                sht.Range("D23:F23").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Discharge
                sht.Range("A24").Value = "";// "Port of Discharge";
                sht.Range("A24:C24").Style.Font.FontSize = 12;
                sht.Range("A24:C24").Style.Font.FontName = "Tahoma";
                sht.Range("A24:C24").Style.NumberFormat.Format = "@";
                sht.Range("A24:C24").Merge();
                //value
                sht.Range("A25").Value = "";//ds.Tables[0].Rows[0]["portunload"];
                sht.Range("A25:C25").Style.Font.FontSize = 12;
                sht.Range("A25:C25").Style.Font.FontName = "Tahoma";
                sht.Range("A25:C25").Style.NumberFormat.Format = "@";
                sht.Range("A25:C25").Merge();
                sht.Range("A25:C25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Final Destination
                sht.Range("D24").Value = "Final Destination";
                sht.Range("D24:F24").Style.Font.FontSize = 12;
                sht.Range("D24:F24").Style.Font.FontName = "Tahoma";
                sht.Range("D24:F24").Style.NumberFormat.Format = "@";
                sht.Range("D24:F24").Merge();
                //value
                sht.Range("D25").Value = ds.Tables[0].Rows[0]["Destinationadd"];
                sht.Range("D25:F25").Style.Font.FontSize = 12;
                sht.Range("D25:F25").Style.Font.FontName = "Tahoma";
                sht.Range("D25:F25").Style.NumberFormat.Format = "@";
                sht.Range("D25:F25").Merge();
                sht.Range("D25:F25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Origin of Goods
                sht.Range("G24").Value = "Country of Origin of Goods";
                sht.Range("G24:I24").Style.Font.FontSize = 12;
                sht.Range("G24:I24").Style.Font.FontName = "Tahoma";
                sht.Range("G24:I24").Style.NumberFormat.Format = "@";
                sht.Range("G24:I24").Merge();
                //value
                sht.Range("G25").Value = ds.Tables[0].Rows[0]["countryoforigin"];
                sht.Range("G25:I25").Style.Font.FontSize = 12;
                sht.Range("G25:I25").Style.Font.FontName = "Tahoma";
                sht.Range("G25:I25").Style.NumberFormat.Format = "@";
                sht.Range("G25:I25").Merge();
                sht.Range("G25:I25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Final Destination
                sht.Range("J24").Value = "Country of Final Destination";
                sht.Range("J24:K24").Style.Font.FontSize = 12;
                sht.Range("J24:K24").Style.Font.FontName = "Tahoma";
                sht.Range("J24:K24").Style.NumberFormat.Format = "@";
                sht.Range("J24:K24").Merge();
                //value
                sht.Range("J25").Value = ds.Tables[0].Rows[0]["countryoffinaldest"];
                sht.Range("J25:K25").Style.Font.FontSize = 12;
                sht.Range("J25:K25").Style.Font.FontName = "Tahoma";
                sht.Range("J25:K25").Style.NumberFormat.Format = "@";
                sht.Range("J25:K25").Merge();
                sht.Range("J25:K25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //*****************Nos
                sht.Range("A26").Value = "Nos.";
                sht.Range("B26").Value = "No and kind of Packages";
                sht.Range("B26:D26").Merge();

                sht.Range("E26").Value = "Description of Goods";
                sht.Range("E26:J26").Merge();
                //sht.Range("k30").Value = "Quantity";                
                sht.Range("A26:K26").Style.Font.FontSize = 12;
                sht.Range("A26:K26").Style.Font.FontName = "Tahoma";
                sht.Range("A26:K26").Style.NumberFormat.Format = "@";
                sht.Range("K26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //values
                sht.Range("A27:H27").Merge();
                sht.Range("A28:H32").Style.Font.FontSize = 12;
                sht.Range("A28:H32").Style.Font.FontName = "Tahoma";
                sht.Range("A28:H32").Style.NumberFormat.Format = "@";
                sht.Range("A28").Value = ds.Tables[0].Compute("Min(MinrollNo)", "") + "/" + ds.Tables[0].Compute("Max(Maxrollno)", "");
                sht.Range("B28").Value = ds.Tables[0].Rows[0]["Noofrolls"] + " Packages";
                sht.Range("B28:D28").Merge();
                sht.Range("E28").Value = ds.Tables[0].Rows[0]["descriptionofgoods"];
                sht.Range("E28:J32").Style.Alignment.WrapText = true;
                sht.Range("E28:J32").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("E28:J32").Merge();
                sht.Range("A33:J33").Merge();
                //            //**********************Details
                //********Hader
                sht.Range("A34").Value = "Roll No";
                sht.Range("A34:K34").Style.Font.FontSize = 12;
                sht.Range("A34:K34").Style.Font.FontName = "Tahoma";
                sht.Range("A34:K34").Style.NumberFormat.Format = "@";
                sht.Range("A34:K34").Style.Font.Bold = true;
                sht.Range("A34").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B34").Value = "Quality";
                sht.Range("B34:C34").Merge();
                sht.Range("D34").Value = "ART.NO.";
                //sht.Range("D38:E38")..Merge();
                sht.Range("E34").Value = "COLOR";
                sht.Range("F34").Value = "SIZE(CM)";
                sht.Range("F34").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("G34").Value = "Pcs/Roll";
                sht.Range("G34").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H34").Value = "Total Rolls";
                sht.Range("H34").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("I34").Value = "Total Pcs";
                sht.Range("J34").Value = "Area Sq. Mtr";
                sht.Range("K34").Value = "P.O.#";
                sht.Range("K34").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A34:K34").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A34").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K34").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A34:K34").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //***********generate Loop
                i = 35;
                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                {
                    sht.Range("A" + i, "K" + i).Style.Font.FontSize = 12;
                    sht.Range("A" + i, "K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("A" + i, "I" + i).Style.NumberFormat.Format = "@";
                    sht.Range("K" + i).Style.Font.FontSize = 12;
                    sht.Range("K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("K" + i).Style.NumberFormat.Format = "@";
                    //pono
                    sht.Range("A" + i).Value = ds.Tables[0].Rows[ii]["Minrollno"] + "  " + ds.Tables[0].Rows[ii]["Maxrollno"];
                    sht.Range("A" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //Article Name
                    sht.Range("B" + i).Value = ds.Tables[0].Rows[ii]["Design"];
                    sht.Range("B" + i, "C" + i).Merge();
                    //Art No.
                    sht.Range("D" + i).Value = ds.Tables[0].Rows[ii]["articleno"];
                    //Colour
                    sht.Range("E" + i).Value = ds.Tables[0].Rows[ii]["Color"];
                    //Size
                    sht.Range("F" + i).Value = ds.Tables[0].Rows[ii]["width"] + "x" + ds.Tables[0].Rows[ii]["Length"];
                    sht.Range("F" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //pcs/roll
                    sht.Range("G" + i).SetValue(ds.Tables[0].Rows[ii]["pcsperroll"]);
                    sht.Range("G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("G" + i).Style.NumberFormat.Format = "@";
                    //Total rolls
                    sht.Range("H" + i).SetValue(ds.Tables[0].Rows[ii]["totalroll"]);
                    sht.Range("H" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("H" + i).Style.NumberFormat.Format = "@";
                    TotalRolls = TotalRolls + Convert.ToInt16(ds.Tables[0].Rows[ii]["totalroll"]);
                    //Qty
                    sht.Range("I" + i).SetValue(ds.Tables[0].Rows[ii]["Pcs"]);
                    sht.Range("I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[ii]["Pcs"]);
                    sht.Range("I" + i).Style.NumberFormat.Format = "@";
                    //Area
                    sht.Range("J" + i).SetValue(ds.Tables[0].Rows[ii]["Area"]);
                    sht.Range("J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                    Area = Decimal.Parse(Area.ToString()) + Decimal.Parse(ds.Tables[0].Rows[ii]["Area"].ToString());
                    //
                    //var _with27 = sht.Range("I" + i);

                    //_with27.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
                    //_with27.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
                    //PO
                    sht.Range("K" + i).Value = ds.Tables[0].Rows[ii]["Pono"];
                    //
                    sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //Borders
                    sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    i = i + 1;
                }
                //
                sht.Range("A" + i + ":A" + (i + 7)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 7)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //End Of loop
                i = i + 7;
                //*************** 

                //
                sht.Range("D" + i).Value = "Packing List Total:";
                sht.Range("D" + i, "F" + i).Style.Font.FontSize = 12;
                sht.Range("D" + i, "F" + i).Style.Font.FontName = "Tahoma";
                sht.Range("D" + i, "F" + i).Style.NumberFormat.Format = "@";
                sht.Range("D" + i, "F" + i).Style.Font.Bold = true;
                sht.Range("D" + i + ":F" + i).Merge();
                sht.Range("D" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //Total Rolls
                sht.Range("H" + i).Value = TotalRolls;
                sht.Range("H" + i).Style.Font.FontSize = 12;
                sht.Range("H" + i).Style.Font.FontName = "Tahoma";
                sht.Range("H" + i).Style.NumberFormat.Format = "@";
                sht.Range("H" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //Total Pcs
                sht.Range("I" + i).Value = Pcs;
                sht.Range("I" + i).Style.Font.FontSize = 12;
                sht.Range("I" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i).Style.NumberFormat.Format = "@";
                sht.Range("I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //Area
                sht.Range("J" + i).Value = Area;
                sht.Range("J" + i).Style.Font.FontSize = 12;
                sht.Range("J" + i).Style.Font.FontName = "Tahoma";
                sht.Range("J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                //
                var _with21 = sht.Range("A" + i + ":K" + i);
                _with21.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                _with21.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                sht.Range("A" + i + ":A" + (i + 10)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 10)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //**********Total
                i = i + 10;
                //i = 63;
                sht.Range("A" + i).Value = "TOTAL ROLLS";
                sht.Range("A" + (i + 1)).Value = "TOTAL PCS";
                sht.Range("A" + (i + 2)).Value = "TOTAL GR. WT";
                sht.Range("A" + (i + 3)).Value = "TOTAL NT. WT";
                sht.Range("A" + (i + 4)).Value = "TOTAL SQ.MTR";
                sht.Range("A" + (i + 5)).Value = "TOTAL VOLUME";
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":A" + (i + 5)).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontName = "Courier New";
                //value                
                sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontSize = 12;
                sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontName = "Tahoma";
                sht.Range("C" + i + ":C" + (i + 5)).Style.NumberFormat.Format = "@";


                sht.Range("C" + i).SetValue(ds.Tables[0].Rows[0]["Noofrolls"]);
                sht.Range("C" + (i + 1)).SetValue(Pcs);
                sht.Range("C" + (i + 2)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["grosswt"]) + " Kgs.");
                sht.Range("C" + (i + 3)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["netwt"]) + " Kgs.");
                sht.Range("C" + (i + 4)).SetValue(String.Format("{0:#,0.00}", Area) + " Sq.Mtr");
                sht.Range("C" + (i + 5)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["Volume"]) + " CBM");
                //
                sht.Range("A" + i + ":A" + (i + 5)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 5)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //***********Sig and date
                i = i + 6;
                //i = 70;
                sht.Range("A" + i + ":H" + i).Merge();
                sht.Range("I" + i).Value = "Signature/Date";
                sht.Range("I" + i).Style.Font.FontSize = 12;
                sht.Range("I" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i).Style.NumberFormat.Format = "@";

                sht.Range("K" + i).Value = "For " + ds.Tables[0].Rows[0]["companyname"];
                sht.Range("J" + i + ":K" + i).Style.Font.FontSize = 12;
                sht.Range("J" + i + ":K" + i).Style.Font.FontName = "Tahoma";
                sht.Range("J" + i + ":K" + i).Style.NumberFormat.Format = "@";
                sht.Range("J" + i + ":K" + i).Style.Font.Bold = true;
                sht.Range("J" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("J" + i + ":K" + i).Style.Font.FontSize = 9;
                //sht.Range("J" + i + ":K" + i).Merge();              
                sht.Range("I" + i + ":K" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("I" + i + ":I" + (i + 3)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 3)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //**************Declaration
                //
                sht.Range("A" + i + ":A" + (i + 2)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                i = i + 1;
                //i = 71;
                sht.Range("A" + i).Value = "Declaration";
                sht.Range("A" + i + ":B" + i).Merge();
                sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":B" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                //value
                i = i + 1;
                sht.Range("A" + i).Value = "We declare that this invoice shows the actual price of the";
                sht.Range("A" + i + ":F" + i).Merge();
                sht.Range("A" + i + ":F" + i).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":F" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":F" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                i = i + 1;
                sht.Range("A" + i).Value = "goods described and that all particulars are true and correct";
                sht.Range("A" + i + ":F" + i).Merge();
                sht.Range("A" + i + ":F" + i).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":F" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":F" + i).Style.NumberFormat.Format = "@";

                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //**********

                sht.Range("I" + i + ":J" + i).Merge();
                sht.Range("I" + i + ":J" + i).Value = ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("I" + i + ":J" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i + ":J" + i).Style.Font.FontSize = 12;
                sht.Range("I" + i + ":J" + i).Style.NumberFormat.Format = "dd/MMM/yyyy";
                sht.Range("I" + i + ":J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("K" + i).Value = "Auth Sign";
                sht.Range("K" + i).Style.Font.FontSize = 12;
                sht.Range("K" + i).Style.Font.FontName = "Tahoma";
                sht.Range("K" + i).Style.NumberFormat.Format = "@";
                sht.Range("K" + i).Style.Font.Bold = true;
                sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //
                var _with39 = sht.Range("I" + i);
                _with39.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //
                sht.Range("A2:A10").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A11:A23").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i + ":K" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A1:K1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //sht.Range("A2:A10").Style.Border.LeftBorder = XLBorderStyleValues.Thin;                
                //sht.Range("F2:F10").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //            //************************
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
                using (var a = sht.Range("K11:K25"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A21:F21"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A23:K23"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                sht.Range("G11:I11").Style.Border.RightBorder = XLBorderStyleValues.None;
                using (var a = sht.Range("F11:F23"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A25:K25"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A19:F19"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("C20:C23"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("I24:I25"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                var _with8 = sht.Range("A23:F23");
                _with8.Style.Border.BottomBorder = XLBorderStyleValues.Thin;


                sht.Range("A11:B11").Style.Border.RightBorder = XLBorderStyleValues.None;



                //
                var _with9 = sht.Range("A24:C25");
                _with9.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with9.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //sht.Range("A24:C24").Style.Border.BottomBorder = XLBorderStyleValues.None;


                //
                var _with10 = sht.Range("D24:F25");
                _with10.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with10.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D24:F24").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //
                //sht.Range("A26:C26").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //
                using (var a = sht.Range("A2" + ":A" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("K2" + ":K" + i))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                //*******************Save
                string Fileextension = "xlsx";
                string filename = "PackingList -" + validateFilename(ds.Tables[0].Rows[0]["TinvoiceNo"].ToString()) + "." + Fileextension + "";
                Path = Server.MapPath("~/Packingexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
                //*****
                lblmsg.Text = "Packing Excel Format downloaded successfully.";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "AltR2", "alert('No Record Found to Export')", true);
            }

        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void ShippingInstructionExcel()
    {
        lblmsg.Text = "Wait For Excel File to Download";
        int i = 0, Pcs = 0;
        Decimal Area = 0, Amount = 0;
        string Path = "";
        if (!Directory.Exists(Server.MapPath("~/TempExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/TempExcel/"));
        }
        try
        {
            string str = @"select *,VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1) as Pcsperroll,VD.Pcs/(VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1)) as TotalRoll From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue;
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("Shipment Instruction");
                //Page
                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                sht.PageSetup.AdjustTo(78);
                //sht.PageSetup.FitToPages(1, 5);                
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                sht.PageSetup.VerticalDpi = 300;
                sht.PageSetup.HorizontalDpi = 300;
                //
                sht.PageSetup.Margins.Top = 0.5;
                sht.PageSetup.Margins.Left = 0.2;
                sht.PageSetup.Margins.Right = 0.2;
                sht.PageSetup.Margins.Bottom = 0.5;
                sht.PageSetup.Margins.Header = 0.3;
                sht.PageSetup.Margins.Footer = 0.3;
                sht.PageSetup.CenterHorizontally = true;
                sht.PageSetup.SetScaleHFWithDocument();
                //*****Column Width
                sht.Column("A").Width = 11.00;
                sht.Column("B").Width = 10.43;
                sht.Column("C").Width = 16.14;
                sht.Column("D").Width = 12.29;
                sht.Column("E").Width = 14.14;
                sht.Column("F").Width = 9.86;
                sht.Column("G").Width = 14.29;
                sht.Column("H").Width = 9.14;
                sht.Column("I").Width = 10.14;
                sht.Column("J").Width = 13.14;
                //*End
                sht.Row(1).Height = 26.25;

                sht.Range("A1:J1").Merge();
                sht.Range("A1").Value = "ORDER SHEET";
                sht.Range("A1:J1").Style.Font.FontName = "Calibri";
                sht.Range("A1:J1").Style.Font.FontSize = 20;
                sht.Range("A1:J1").Style.Font.Bold = true;
                sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //****
                sht.Range("A7:I7").Merge();
                sht.Range("A7").Value = "(Export-Contract)";
                sht.Range("A7:I7").Style.Font.FontName = "Times New Roman";
                sht.Range("A7:I7").Style.Font.FontSize = 12;
                sht.Range("A7:I7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //Importer
                sht.Range("A10").Value = "IMPORTER:";
                sht.Range("A10:H10").Style.Font.FontName = "Times New Roman";
                sht.Range("A10:H10").Style.Font.FontSize = 12;
                //
                sht.Range("H10").Value = "EXPORTER:";
                //Consignee
                sht.Range("A12:F12").Merge();
                sht.Range("A12").Value = ds.Tables[0].Rows[0]["Consignee"];
                sht.Range("A12:F12").Style.Font.FontName = "Times New Roman";
                sht.Range("A12:F12").Style.Font.FontSize = 10;
                //address
                sht.Range("A13:F16").Merge();
                sht.Range("A13").Value = ds.Tables[0].Rows[0]["Consignee_address"];
                sht.Range("A13:F16").Style.Font.FontName = "Times New Roman";
                sht.Range("A13:F16").Style.Font.FontSize = 10;
                sht.Range("A13:F16").Style.Font.Bold = true;
                sht.Range("A13:F16").Style.Alignment.WrapText = true;
                sht.Range("A13:F16").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                //Exporter Details
                sht.Range("H12:J12").Merge();
                sht.Range("H12").Value = ds.Tables[0].Rows[0]["CompanyName"];
                //address
                sht.Range("H13:J13").Merge();
                sht.Range("H13").Value = ds.Tables[0].Rows[0]["compaddr1"];
                //
                sht.Range("H14:J14").Merge();
                sht.Range("H14").Value = ds.Tables[0].Rows[0]["compaddr2"] + "," + ds.Tables[0].Rows[0]["compaddr3"];

                sht.Range("H12:J14").Style.Font.FontName = "Calibri";
                sht.Range("H12:J14").Style.Font.FontSize = 11;
                //Header
                sht.Range("A17:I17").Merge();
                sht.Range("A17").Value = "SHIPMENT INSTRUCTION";
                sht.Range("A17:I17").Style.Font.FontName = "Times New Roman";
                sht.Range("A17:I17").Style.Font.FontSize = 12;
                sht.Range("A17:I17").Style.Font.Bold = true;
                sht.Range("A17:I17").Style.Font.Underline = XLFontUnderlineValues.Single;
                sht.Range("A17:I17").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //**********
                sht.Row(20).Height = 36.00;
                sht.Range("A20:I20").Merge();
                sht.Range("A20").Value = "Please Ship The Selected " + ds.Tables[0].Rows[0]["descriptionofgoods"];
                sht.Range("A20:I20").Style.Font.FontName = "Times New Roman";
                sht.Range("A20:I20").Style.Font.FontSize = 12;
                sht.Range("A20:I20").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                //Detail headers
                sht.Range("A21").Value = "ORDER NO";
                sht.Range("B21").Value = "ORDER DT";
                sht.Range("C21").Value = "ARTICLE NAME";
                sht.Range("D21").Value = "ART.NO.";
                sht.Range("E21").Value = "COLOR";
                sht.Range("F21").Value = "SIZE(" + ds.Tables[0].Rows[0]["Sizelabel"] + ")";
                sht.Range("G21").Value = "Area " + ds.Tables[0].Rows[0]["unit"];
                sht.Range("H21").Value = "QTY UNIT";
                sht.Range("I21").Value = "RATE/PC";
                sht.Range("J21").Value = "AMOUNT";

                sht.Range("A21:J21").Style.Font.FontName = "Tahoma";
                sht.Range("A21:J21").Style.Font.FontSize = 10;
                sht.Range("A21:J21").Style.Font.Bold = true;
                sht.Range("A21:J21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //Excel Row
                i = 22;
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    sht.Range("A" + i).Value = ds.Tables[0].Rows[j]["Pono"];
                    sht.Range("B" + i).Value = "";
                    sht.Range("C" + i).Value = ds.Tables[0].Rows[j]["Design"];
                    sht.Range("D" + i).Value = ds.Tables[0].Rows[j]["articleno"];
                    sht.Range("E" + i).Value = ds.Tables[0].Rows[j]["color"];
                    sht.Range("F" + i).Value = ds.Tables[0].Rows[j]["Width"] + "x" + ds.Tables[0].Rows[0]["Length"];
                    sht.Range("G" + i).Value = ds.Tables[0].Rows[j]["Area"];
                    Area = Area + Convert.ToDecimal(ds.Tables[0].Rows[j]["Area"]);
                    sht.Range("H" + i).Value = ds.Tables[0].Rows[j]["Pcs"];
                    Pcs = Pcs + Convert.ToInt32(ds.Tables[0].Rows[j]["Pcs"]);
                    sht.Range("I" + i).Value = ds.Tables[0].Rows[j]["Price"];
                    sht.Range("J" + i).Value = ds.Tables[0].Rows[j]["amount"];
                    Amount = Amount + Convert.ToDecimal(ds.Tables[0].Rows[j]["amount"]);
                    //
                    sht.Range("A" + i + ":J" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("A" + i + ":J" + i).Style.Font.FontSize = 10;
                    sht.Range("A" + i + ":J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    sht.Range("G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    sht.Range("G" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("I" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";

                    using (var a = sht.Range("A" + i + ":J" + i))
                    {
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }


                    i = i + 1;
                }
                var k = ds.Tables[0].Rows.Count;
                if (k < 8)
                {
                    using (var a = sht.Range("A" + i + ":J" + (i + 8 - k)))
                    {
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }
                    i = i + (8 - k);
                }
                //***********
                sht.Row(i).Height = 21.75;
                sht.Range("F" + i).Value = "Total:";
                sht.Range("G" + i).Value = Area;
                sht.Range("H" + i).Value = Pcs;
                sht.Range("J" + i).Value = Amount;


                sht.Range("F" + i + ":J" + i).Style.Font.FontName = "Tahoma";
                sht.Range("F" + i + ":J" + i).Style.Font.FontSize = 10;
                sht.Range("F" + i + ":J" + i).Style.Font.Bold = true;
                sht.Range("F" + i).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
                sht.Range("G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("G" + i).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
                sht.Range("G" + i).Style.NumberFormat.Format = "#,###0.00";
                sht.Range("H" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H" + i).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
                sht.Range("J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("J" + i).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
                sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                //************
                i = i + 1;
                sht.Range("A" + i + ":A" + (i + 2)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("J" + i + ":J" + (i + 2)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                i = i + 3;
                //
                sht.Range("A" + i + ":B" + i).Merge();
                sht.Range("A" + i).Value = "Delivery Time";
                sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Times New Roman";
                sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 11;
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("J" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                i = i + 1;
                sht.Range("A" + i + ":B" + i).Merge();
                sht.Range("A" + i).Value = "Shipment By Sea or Air";
                sht.Range("A" + i + ":D" + i).Style.Font.FontName = "Times New Roman";
                sht.Range("A" + i + ":D" + i).Style.Font.FontSize = 11;
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("J" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D" + i).Value = ds.Tables[0].Rows[0]["vessel_flightno"];
                //
                i = i + 1;
                sht.Range("A" + i + ":B" + i).Merge();
                sht.Range("A" + i).Value = "Port of Destination";
                sht.Range("A" + i + ":D" + i).Style.Font.FontName = "Times New Roman";
                sht.Range("A" + i + ":D" + i).Style.Font.FontSize = 11;
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("J" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D" + i).Value = ds.Tables[0].Rows[0]["portunload"];
                //
                i = i + 1;
                sht.Range("A" + i + ":B" + i).Merge();
                sht.Range("A" + i).Value = "Freight Paid or To-pay";
                sht.Range("A" + i + ":D" + i).Style.Font.FontName = "Times New Roman";
                sht.Range("A" + i + ":D" + i).Style.Font.FontSize = 11;
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("J" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D" + i).Value = ds.Tables[0].Rows[0]["Freightterms"];
                //
                i = i + 1;
                sht.Range("A" + i + ":B" + i).Merge();
                sht.Range("A" + i).Value = "Payment Terms";
                sht.Range("A" + i + ":I" + i).Style.Font.FontName = "Times New Roman";
                sht.Range("A" + i + ":I" + i).Style.Font.FontSize = 11;
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("J" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D" + i + ":I" + i).Merge();
                sht.Range("D" + i).Value = ds.Tables[0].Rows[0]["Deliveryterm"];
                //
                i = i + 1;
                sht.Range("A" + i + ":B" + i).Merge();
                sht.Range("A" + i).Value = "Notify Address";
                sht.Range("A" + i + ":I" + i).Style.Font.FontName = "Times New Roman";
                sht.Range("A" + i + ":I" + i).Style.Font.FontSize = 11;
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("J" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                sht.Range("D" + i + ":I" + (i + 2)).Merge();
                sht.Range("D" + i).Value = ds.Tables[0].Rows[0]["Notifyparty_address"];
                sht.Range("D" + i + ":I" + (i + 2)).Style.Font.FontName = "Times New Roman";
                sht.Range("D" + i + ":I" + (i + 2)).Style.Font.FontSize = 11;
                sht.Range("D" + i + ":I" + (i + 2)).Style.Font.Bold = true;
                sht.Range("D" + i + ":I" + (i + 2)).Style.Alignment.WrapText = true;
                sht.Range("D" + i + ":I" + (i + 2)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("J" + i + ":J" + (i + 2)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                i = i + 3;
                sht.Range("A" + i + ":B" + i).Merge();
                sht.Range("A" + i).Value = "Signature of Importer";
                sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Times New Roman";
                sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 11;
                sht.Range("A" + i + ":B" + i).Style.Font.Bold = true;

                //
                sht.Range("H" + i + ":I" + i).Merge();
                sht.Range("H" + i).Value = "Signature of Exporter";
                sht.Range("H" + i + ":I" + i).Style.Font.FontName = "Times New Roman";
                sht.Range("H" + i + ":I" + i).Style.Font.FontSize = 11;
                sht.Range("H" + i + ":I" + i).Style.Font.Bold = true;

                sht.Range("A" + i + ":A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("J" + i + ":J" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + (i + 1) + ":J" + (i + 1)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //*Borders
                using (var a = sht.Range("A1:J1"))
                {
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A1:A20"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("J1:J20"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A21:J21"))
                {
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                //*******************Save
                string Fileextension = "xlsx";
                string filename = "Ordersheet-" + validateFilename(ds.Tables[0].Rows[0]["TinvoiceNo"].ToString()) + "." + Fileextension + "";
                Path = Server.MapPath("~/Tempexcel/Ordersheet-" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
                //*****
                lblmsg.Text = "Packing Excel Format downloaded successfully.";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "AltR", "alert('No Record Found to Export')", true);
            }
            //***            

        }
        catch (Exception)
        {

            throw;
        }
    }
    //protected void Exporttoexcel_PackingType_1()
    //{

    //    try
    //    {

    #region
    //        //*****Styles
    //        ExcelStyle HStyle = sht.Application.ActiveWorkbook.Styles.Add("Hstyle");
    //        var With = HStyle;
    //        With.Font.Size = "12";
    //        With.Font.Name = "Tahoma";
    //        With.NumberFormat = "@";
    //        With.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
    //        With.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
    //        With.Font.Bold = true;
    //        //**********Normal
    //        ExcelStyle NormalStyle = sht.Application.ActiveWorkbook.Styles.Add("NormalStyle");
    //        var With1 = NormalStyle;
    //        With1.Font.Size = "12";
    //        With1.Font.Name = "Tahoma";
    //        With1.NumberFormat = "@";
    //        //***************Normal Bold
    //        ExcelStyle NormalStyleWithBold = sht.Application.ActiveWorkbook.Styles.Add("NormalStyleWithBold");
    //        var With2 = NormalStyleWithBold;
    //        With2.Font.Size = "12";
    //        With2.Font.Name = "Tahoma";
    //        With2.NumberFormat = "@";
    //        With2.Font.Bold = true;
    //        //***************Normal Bold centre
    //        ExcelStyle NormalStyleWithBold_Centre = sht.Application.ActiveWorkbook.Styles.Add("NormalStyleWithBold_Centre");
    //        var With3 = NormalStyleWithBold_Centre;
    //        With3.Font.Size = "12";
    //        With3.Font.Name = "Tahoma";
    //        With3.NumberFormat = "@";
    //        With3.Font.Bold = true;
    //        With3.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
    //        //***************Normal  centre
    //        ExcelStyle NormalStyleWith_Centre = sht.Application.ActiveWorkbook.Styles.Add("NormalStyleWith_Centre");
    //        var With4 = NormalStyleWith_Centre;
    //        With4.Font.Size = "12";
    //        With4.Font.Name = "Tahoma";
    //        With4.NumberFormat = "@";
    //        With4.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
    //        //***********Header        

    #endregion









    //            #endregion
    //            //  go to dynamic list
    //        }
    //        else
    //        {

    //        }
    //        //******************
    //        string Fileextension = "xls";
    //        string filename = "PackingExcel" + DateTime.Now.Ticks + "." + Fileextension + "";
    //        #region
    //        //switch (xapp.Version.ToString())
    //        //{
    //        //    case "11.0":
    //        //        Fileextension = "xls";
    //        //        break;
    //        //    case "12.0":
    //        //        Fileextension = "xlsx";
    //        //        break;
    //        //    default:
    //        //        Fileextension = "xlsx";
    //        //        break;
    //        //}
    //        #endregion

    //        string Path = Server.MapPath("~/PackingExcel/" + filename);

    //        xbk.SaveAs(Path, Excel.XlFileFormat.xlWorkbookNormal, misvalue, misvalue, misvalue, misvalue, Excel.XlSaveAsAccessMode.xlExclusive, misvalue, misvalue, misvalue, misvalue, misvalue);
    //        xbk.Close(true, misvalue, misvalue);
    //        xapp.Quit();
    //        Marshal.ReleaseComObject(sht);
    //        Marshal.ReleaseComObject(xbk);
    //        Marshal.ReleaseComObject(xapp);
    //        //Download File
    //        Response.ClearHeaders();
    //        Response.Clear();
    //        Response.ContentType = "application/vnd.ms-excel";
    //        Response.AddHeader("content-disposition", "attachment;filename=" + filename);
    //        Response.WriteFile(Path);
    //        Response.End();
    //        //*****
    //        File.Delete(Path);
    //    }
    //    catch (Exception ex)
    //    {
    //        xapp.Quit();
    //        Marshal.ReleaseComObject(sht);
    //        Marshal.ReleaseComObject(xbk);
    //        Marshal.ReleaseComObject(xapp);
    //        lblmsg.Text = ex.Message;
    //    }
    //    lblmsg.Text = "Packing Excel Format downloaded successfully.";
    //}
    private static readonly Regex InvalidFileRegex = new Regex(string.Format("[{0}]", Regex.Escape(@"<>:""/\|?*")));
    public static string validateFilename(string filename)
    {
        return InvalidFileRegex.Replace(filename, string.Empty);
    }
    protected void vdfexcel(int Invoiceid)
    {
        string str = "";
            str = @"select *,VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1) as Pcsperroll,VD.Pcs/(VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1)) as TotalRoll 
                        From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId 
                        Where VI.invoiceid=" + Invoiceid + " order by MinrollNo";       

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {

            XLWorkbook xapp = new XLWorkbook(Server.MapPath("~/VDFExcel/VDFExcel.xlsx"));
            IXLWorksheet sht = xapp.Worksheet(1);
            sht.Range("A5:M8").Style.Font.FontName = "Arial";
            sht.Range("A5:M8").Style.Font.FontSize = 10;
            sht.Range("A5:M8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            var a = sht.Cell(5, 1);
            a.RichText.AddText("EXPORTER NAME: ").SetFontName("Arial Black");
            a.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["companyname"])).SetFontName("Cambria");

            var b = sht.Cell(6, 1);
            b.RichText.AddText("GSTIN.# ").SetFontName("Arial Black");
            b.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["GSTNO"])).SetFontName("Cambria");

            var c = sht.Cell(7, 1);
            c.RichText.AddText("SHIPMENT ID: ").SetFontName("Arial Black");
            c.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["shipmentid"])).SetFontName("Cambria");

            var D = sht.Cell(8, 1);
            D.RichText.AddText("SUPPLIER No. ").SetFontName("Arial Black");
            D.RichText.AddText("21899").SetFontName("Cambria");

            var E = sht.Cell(5, 5);
            E.RichText.AddText("INVOICE No. ").SetFontName("Arial Black");
            E.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["TinvoiceNo"])).SetFontName("Cambria");

            var F = sht.Cell(6, 5);
            F.RichText.AddText("DATE : ").SetFontName("Arial Black");
            F.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["invoicedate"])).SetFontName("Cambria");

            var G = sht.Cell(7, 5);
            G.RichText.AddText("APLL CSGN NO ").SetFontName("Arial Black");
            G.RichText.AddText("ECIS NO " + Convert.ToString(ds.Tables[0].Rows[0]["Ecisno"])).SetFontName("Cambria");

            var H = sht.Cell(5, 10);
            H.RichText.AddText("CBM DECLARED : ").SetFontName("Arial Black");
            H.RichText.AddText(string.Format(Convert.ToString(ds.Tables[0].Rows[0]["Volume"]), "#,##0.000") + " CBM").SetFontName("Arial Black");

            var I = sht.Cell(6, 10);
            I.RichText.AddText("NET WEIGHT : ").SetFontName("Arial Black");
            I.RichText.AddText(string.Format(Convert.ToString(ds.Tables[0].Rows[0]["Netwt"]), "#,##0.000") + " KG").SetFontName("Arial Black");

            var J = sht.Cell(7, 10);
            J.RichText.AddText("GROSS WEIGHT : ").SetFontName("Arial Black");
            J.RichText.AddText(string.Format(Convert.ToString(ds.Tables[0].Rows[0]["grosswt"]), "#,##0.000") + " KG").SetFontName("Arial Black");
            sht.Range("A9:M11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            var K = sht.Cell(9, 1);
            K.RichText.AddText("DESTINATION CODE: ").SetFontName("Arial Black");
            K.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["destcode"])).SetFontName("Cambria");

            var L = sht.Cell(10, 1);
            L.RichText.AddText("DELIVERY WEEK: ").SetFontName("Arial Black");
            L.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["Delvwk"])).SetFontName("Arial Black");

            var M = sht.Cell(11, 1);
            M.RichText.AddText("SEAL NO : ").SetFontName("Cambria");
            M.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["sealno"])).SetFontName("Cambria");

            var N = sht.Cell(9, 5);
            N.RichText.AddText("TRUCK No. ").SetFontName("Arial Black");
            N.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["truckno"])).SetFontName("Arial Black");

            var O = sht.Cell(11, 5);
            O.RichText.AddText("DESP. DATE ").SetFontName("Arial Black");
            O.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["Dispatchdate"])).SetFontName("Arial Black");
            //**************Details
            int row = 17;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":M" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":M" + row).Style.Font.FontSize = 10;
                sht.Range("A" + row + ":M" + row).Style.Font.Bold = true;
                sht.Range("A" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                using (var ar = sht.Range("A" + row + ":M" + row))
                {
                    ar.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                    ar.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                    ar.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                    ar.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                }

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["PONo"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Articleno"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Pcs"] + " Pcs");
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Totalroll"]);
                sht.Range("E" + row).SetValue("PLT");
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["pcsperroll"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["MinrollNo"] + "-" + ds.Tables[0].Rows[i]["Maxrollno"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Pcs"] + " Pcs");
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Totalroll"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["pcsperroll"]);
                sht.Range("L" + row).SetValue("S2");

                row = row + 1;
            }
            //************           

            for (int i = 0; i <= 2; i++)
            {
                sht.Range("A" + row + ":M" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":M" + row).Style.Font.FontSize = 10;
                sht.Range("A" + row + ":M" + row).Style.Font.Bold = true;
                sht.Range("A" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                using (var ar = sht.Range("A" + row + ":M" + row))
                {
                    ar.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                    ar.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                    ar.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                    ar.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                }
                i = i + 1;
                row = row + 1;
            }
            //*******************
            row = row + 1;
            sht.Range("A" + row + ":C" + row).Merge();
            sht.Range("F" + row + ":J" + row).Merge();
            sht.Range("A" + row + ":M" + row).Style.Font.FontName = "Arial Black";
            sht.Range("A" + row + ":M" + row).Style.Font.FontSize = 10;
            sht.Range("A" + row + ":M" + row).Style.Font.Bold = true;
            sht.Range("A" + row).SetValue("VENDOR’S SIGNATURE & STAMP");
            sht.Range("F" + row).SetValue("APLL ACKNOWLEDGEMENT at W/H");
            //**********
            row = row + 1;
            sht.Range("G" + row + ":J" + row).Merge();
            sht.Range("G" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            var B1 = sht.Cell(row, 7);
            B1.RichText.AddText("· ").SetFontName("Symbol").SetFontSize(10).SetBold();
            B1.RichText.AddText("Received By :").SetFontName("Arial").SetFontSize(10).SetBold();
            //*******
            row = row + 1;
            sht.Range("G" + row + ":J" + row).Merge();
            sht.Range("G" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            var B2 = sht.Cell(row, 7);
            B2.RichText.AddText("· ").SetFontName("Symbol").SetFontSize(10).SetBold();
            B2.RichText.AddText("Condition of Cargo :").SetFontName("Arial").SetFontSize(10).SetBold();
            //**********
            row = row + 3;
            var a2 = sht.Cell(row, 1);
            sht.Range("A" + row).Style.Font.FontSize = 10;
            a2.RichText.AddText("Please").SetFontName("Arial Black");
            a2.RichText.AddText(" Ö").SetFontName("Symbol").SetBold();

            //**************
            row = row + 1;
            sht.Range("A" + row + ":A" + (row + 4)).Style.Font.FontName = "Arial";
            sht.Range("A" + row + ":A" + (row + 4)).Style.Font.FontSize = 7.5;
            sht.Range("A" + row + ":A" + (row + 4)).Style.Font.Bold = true;
            sht.Range("A" + row + ":A" + (row + 4)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            sht.Range("D" + row + ":J" + (row + 4)).Style.Font.FontName = "Arial";
            sht.Range("D" + row + ":J" + (row + 4)).Style.Font.FontSize = 7.5;
            sht.Range("D" + row + ":J" + (row + 4)).Style.Font.Bold = true;
            sht.Range("D" + row + ":J" + (row + 4)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            using (var range = sht.Range("A" + row + ":B" + (row + 4)))
            {
                range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
            }
            sht.Range("A" + row).SetValue("DEEC");
            sht.Range("D" + row + ":G" + row).Merge();
            sht.Range("H" + row + ":J" + row).Merge();
            sht.Range("D" + row).Value = "Truck Reporting Date n Time";

            using (var range = sht.Range("D" + row + ":J" + (row + 4)))
            {
                range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
            }
            row = row + 1;
            sht.Range("A" + row).SetValue("DEPB + EPCG");
            sht.Range("B" + row).Value = "Ö";
            sht.Range("B" + row).Style.Font.FontName = "Symbol";
            sht.Range("B" + row).Style.Font.SetFontColor(XLColor.Blue);
            sht.Range("B" + row).Style.Font.Bold = true;


            sht.Range("D" + row + ":G" + row).Merge();
            sht.Range("H" + row + ":J" + row).Merge();
            sht.Range("D" + row).Value = "Truck Gate-in Date n Time";

            row = row + 1;

            sht.Range("A" + row).SetValue("DRAWBACK");
            sht.Range("D" + row + ":G" + row).Merge();
            sht.Range("H" + row + ":J" + row).Merge();
            sht.Range("D" + row).Value = "Truck Offloading Start Time";

            row = row + 1;
            sht.Range("A" + row).SetValue("WHITE");
            sht.Range("D" + row + ":G" + row).Merge();
            sht.Range("H" + row + ":J" + row).Merge();
            sht.Range("D" + row).Value = "Truck Offloading End Time";

            row = row + 1;
            sht.Range("A" + row).SetValue("100% EOU");
            sht.Range("D" + row + ":G" + row).Merge();
            sht.Range("H" + row + ":J" + row).Merge();
            sht.Range("D" + row).Value = "Truck Gate-out Date n Time";
            //**********
            row = row + 2;
            sht.Range("A" + row + ":B" + row).Merge();
            sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial";
            sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 10;
            sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;
            sht.Range("A" + row).Value = "TERMS & CONDITIONS";
            //*******
            row = row + 1;
            //
            sht.Range("A" + row + ":M" + row).Merge();
            sht.Range("A" + row + ":M" + (row + 3)).Style.Font.FontName = "Times New Roman";
            sht.Range("A" + row + ":M" + (row + 3)).Style.Font.FontSize = 7;
            sht.Range("A" + row + ":M" + (row + 3)).Style.Font.Bold = false;
            sht.Range("A" + row).Value = "All transaction with APLL are subject to the terms and condition stipulated in the company’s cargo receipt (copies available on request from company) APLL may exclude or limit its liabilities and apply certain cases";
            //
            row = row + 1;
            sht.Range("A" + row + ":M" + row).Merge();
            sht.Range("A" + row).Value = "   1)    This Vendor Declaration form is applicable only if it is validated and signed by an authorized signatory of APLL warehouse or appointed warehouse operator in additions to that of shipper’s or it’s representative.";
            //
            row = row + 1;

            sht.Range("A" + row + ":M" + row).Merge();
            sht.Range("A" + row).Value = "   2)    This Vendor Declaration form should be completed and presented to APLL or it’s appointed warehouse operator for processing and validatio";
            //
            row = row + 1;
            sht.Range("A" + row + ":M" + row).Merge();
            sht.Range("A" + row).Value = "APLL reserves the right to verify and adjust shipper’s declared cargo measurement in accordance with the actual declare measurement";
            //Save
            string Path = "";
            string Fileextension = "xlsx";
            string filename = "Vdf-" + validateFilename(ds.Tables[0].Rows[0]["Tinvoiceno"] + "." + Fileextension + "");
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            Response.End();
            //*************
            lblmsg.Text = "VDF Excel Format downloaded successfully.";

        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }

    protected void Exporttoexcel_Kaysons_1()
    {
        lblmsg.Text = "Wait For Excel File to Download";
        if (!Directory.Exists(Server.MapPath("~/InvoiceExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/InvoiceExcel/"));
        }
        string Path = "";
        try
        {

            //******************
            string str = "";
            if (variable.VarGSTForInvoiceFormNew == "1")
            {
                str = @"select * From V_InvoiceDetail VI inner join V_PackingDetailsIkea_WithGST VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue;
            }
            else
            {
                str = @"select * From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue;
            }
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int i, Pcs = 0, TotalPcs = 0;
                Decimal Area = 0, Amount = 0, TotalAmount = 0, GSTAmount = 0, TotalGSTAmount = 0, TotalAmountWithGST = 0;

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("Invoice1");

                //************set cell width
                //Page
                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                sht.PageSetup.AdjustTo(63);
                //sht.PageSetup.FitToPages(1, 5);                
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                sht.PageSetup.VerticalDpi = 300;
                sht.PageSetup.HorizontalDpi = 300;
                //
                sht.PageSetup.Margins.Top = 0.01;
                sht.PageSetup.Margins.Left = 0;
                sht.PageSetup.Margins.Right = 0;
                sht.PageSetup.Margins.Bottom = 0.01;
                sht.PageSetup.Margins.Header = 0.1;
                sht.PageSetup.Margins.Footer = 0.1;
                sht.PageSetup.CenterHorizontally = true;
                sht.PageSetup.CenterVertically = true;
                sht.PageSetup.SetScaleHFWithDocument();
                //************
                sht.Column("A").Width = 25.71;
                sht.Column("B").Width = 8.86;
                sht.Column("C").Width = 18.00;
                sht.Column("D").Width = 15.29;
                sht.Column("E").Width = 10.43;
                sht.Column("F").Width = 15.86;
                sht.Column("G").Width = 12.86;
                sht.Column("H").Width = 18.29;
                sht.Column("I").Width = 20.71;
                sht.Column("J").Width = 20.29;
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
                //***************
                sht.Cell("A2").Value = @"""SUPPLY MEANT FOR EXPORT UNDER LETTER OF UNDERTAKING NO-21/2017 Dt.07.07.2017. WITHOUT PAYMENT OF INTEGRATED TAX""";
                sht.Range("A2:K2").Style.Font.FontName = "Tahoma";
                sht.Range("A2:K2").Style.Font.FontSize = 12;
                sht.Range("A2:K2").Style.Font.Bold = true;
                sht.Range("A2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:K2").Merge();
                using (var a = sht.Range("A2:K2"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                }
                //********Exporter
                sht.Range("A3").Value = "Exporter";
                sht.Range("A3:F3").Style.Font.FontName = "Tahoma";
                sht.Range("A3:F3").Style.Font.FontSize = 12;
                sht.Range("A3:F3").Merge();
                //*****************

                //CompanyName
                sht.Range("A4").Value = ds.Tables[0].Rows[0]["companyName"];
                sht.Range("A4:F4").Style.Font.FontName = "Tahoma";
                sht.Range("A4:F4").Style.Font.FontSize = 12;
                sht.Range("A4:F4").Style.Font.Bold = true;
                sht.Range("A4:F4").Merge();
                //Address
                sht.Range("A5").Value = ds.Tables[0].Rows[0]["compaddr1"];
                sht.Range("A5:F5").Style.Font.FontName = "Tahoma";
                sht.Range("A5:F5").Style.Font.FontSize = 12;
                sht.Range("A5:F5").Merge();
                //address2
                sht.Range("A6").Value = ds.Tables[0].Rows[0]["compaddr2"] + "," + ds.Tables[0].Rows[0]["compaddr3"];
                sht.Range("A6:F6").Style.Font.FontName = "Tahoma";
                sht.Range("A6:F6").Style.Font.FontSize = 12;
                sht.Range("A6:F6").Merge();
                //TiN No
                sht.Range("A7").Value = "GSTIN#" + ds.Tables[0].Rows[0]["gstno"];
                sht.Range("A7:F7").Style.Font.FontName = "Tahoma";
                sht.Range("A7:F7").Style.Font.FontSize = 12;
                sht.Range("A7:F7").Style.Font.Bold = true;
                sht.Range("A7:F7").Merge();
                //**********INvoiceNodate
                sht.Range("G3").Value = "Invoice No./Date";
                sht.Range("G3:K3").Style.Font.FontName = "Tahoma";
                sht.Range("G3:K3").Style.Font.FontSize = 12;
                sht.Range("G3:K3").Merge();
                //value
                sht.Range("G4").Value = ds.Tables[0].Rows[0]["TInvoiceNo"] + " Dated " + ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("G4:K4").Style.Font.FontName = "Tahoma";
                sht.Range("G4:K4").Style.Font.FontSize = 12;
                sht.Range("G4:K4").Merge();
                sht.Range("G4", "K4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //*************IE Code
                sht.Range("G5").Value = "IE Code No.";
                sht.Range("G5:I5").Style.Font.FontName = "Tahoma";
                sht.Range("G5:I5").Style.Font.FontSize = 12;
                sht.Range("G5:I5").Merge();
                //value
                sht.Range("G6").Value = ds.Tables[0].Rows[0]["IEcode"];
                sht.Range("G6:I6").Style.Font.FontName = "Tahoma";
                sht.Range("G6:I6").Style.Font.FontSize = 12;
                sht.Range("G6:I6").Merge();
                sht.Range("G6", "I6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //***********GRI Form No
                sht.Range("J5").Value = "REX No.";
                sht.Range("J5:K5").Style.Font.FontName = "Tahoma";
                sht.Range("J5:K5").Style.Font.FontSize = 12;
                sht.Range("J5:K5").Merge();
                // value
                sht.Range("J6").Value = "INREX1510001883EC025";
                sht.Range("J6:K6").Style.Font.FontName = "Tahoma";
                sht.Range("J6:K6").Style.Font.FontSize = 12;
                sht.Range("J6:K6").Merge();
                sht.Range("J6", "K6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //*************Buyer's Order No and Date
                sht.Range("G7").Value = "Buyer's Order No. / Date";
                sht.Range("G7:K8").Style.Font.FontName = "Tahoma";
                sht.Range("G7:K8").Style.Font.FontSize = 12;
                sht.Range("G7:K7").Merge();
                //value
                sht.Range("G8").SetValue(ds.Tables[0].Rows[0]["TorderNo"]);
                sht.Range("G8:K8").Style.Font.FontName = "Tahoma";
                sht.Range("G8:K8").Style.Font.FontSize = 12;
                sht.Range("G8:K8").Merge();
                sht.Range("G8", "K8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //**************Other Reference(s)
                sht.Range("G9").Value = "Other Reference(s)";
                sht.Range("G10").Value = "CONSIGNMENT NO.:" + ds.Tables[0].Rows[0]["otherref"];
                sht.Range("G11").Value = "SUPPLIER NO.: 21899";
                sht.Range("G9:G11").Style.Font.FontName = "Tahoma";
                sht.Range("G9:G11").Style.Font.FontSize = 12;
                sht.Range("G9:K9").Merge();
                sht.Range("G10:K10").Merge();
                sht.Range("G11:K11").Merge();
                //*************Consignee
                sht.Range("A12").Value = "Consignee";
                sht.Range("C12").Value = ds.Tables[0].Rows[0]["consignee_dt"];
                sht.Range("A12:B12").Style.Font.FontName = "Tahoma";
                sht.Range("A12:B12").Style.Font.FontSize = 12;

                sht.Range("C12:F12").Style.Font.FontName = "Tahoma";
                sht.Range("C12:F12").Style.Font.FontSize = 12;
                sht.Range("C12:F12").Style.Font.Bold = true;

                sht.Range("A12:B12").Merge();
                sht.Range("C12:F12").Merge();
                //value
                sht.Range("A13").Value = ds.Tables[0].Rows[0]["consignee"];
                sht.Range("A13:F13").Style.Font.FontName = "Tahoma";
                sht.Range("A13:F13").Style.Font.FontSize = 12;
                sht.Range("A13:F13").Style.Font.Bold = true;
                sht.Range("A13:F13").Merge();
                //**
                sht.Range("A14").Value = ds.Tables[0].Rows[0]["consignee_address"];
                sht.Range("A14:F17").Style.Font.FontName = "Tahoma";
                sht.Range("A14:F17").Style.Font.FontSize = 12;
                sht.Range("A14", "F17").Style.Alignment.WrapText = true;
                sht.Range("A14", "F17").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A14:F17").Merge();
                //***********Notify
                sht.Range("A18").Value = "Notify Party";
                sht.Range("C18").Value = ds.Tables[0].Rows[0]["Notifyparty_dt"];
                sht.Range("A18:B18").Style.Font.FontName = "Tahoma";
                sht.Range("A18:B18").Style.Font.FontSize = 12;

                sht.Range("A18:B18").Style.Font.SetUnderline();
                sht.Range("C18:F18").Style.Font.FontName = "Tahoma";
                sht.Range("C18:F18").Style.Font.FontSize = 12;
                sht.Range("C18:F18").Style.Font.Bold = true;

                sht.Range("A18:B18").Merge();
                sht.Range("C18:F18").Merge();
                //value
                sht.Range("A19").Value = ds.Tables[0].Rows[0]["Notifyparty"];
                sht.Range("A19:F19").Style.Font.FontName = "Tahoma";
                sht.Range("A19:F19").Style.Font.FontSize = 12;
                sht.Range("A19:F19").Style.Font.Bold = true;
                sht.Range("A19:F19").Merge();
                //
                sht.Range("A20").Value = ds.Tables[0].Rows[0]["notifyparty_address"];
                sht.Range("A20:F24").Style.Font.FontName = "Tahoma";
                sht.Range("A20:F24").Style.Font.FontSize = 12;
                sht.Range("A20", "F24").Style.Alignment.WrapText = true;
                sht.Range("A20", "F24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A20:F24").Merge();
                //***********Receiver address
                sht.Range("G12").Value = "Receiver Address";
                sht.Range("G12:I12").Style.Font.FontName = "Tahoma";
                sht.Range("G12:I12").Style.Font.FontSize = 12;
                sht.Range("G12:I12").Merge();
                //values     2
                sht.Range("J12").Value = ds.Tables[0].Rows[0]["Destcode"];
                sht.Range("J12:K12").Style.Font.FontName = "Tahoma";
                sht.Range("J12:K12").Style.Font.FontSize = 12;
                sht.Range("J12:K12").Style.Font.Bold = true;
                sht.Range("J12:K12").Merge();
                //****** 1.
                sht.Range("G13").Value = "1. " + ds.Tables[0].Rows[0]["Receiver"];
                sht.Range("G13:K13").Style.Font.FontName = "Tahoma";
                sht.Range("G13:K13").Style.Font.FontSize = 12;
                sht.Range("G13:K13").Style.Font.Bold = true;
                sht.Range("G13:K13").Merge();
                //*
                sht.Range("G14").Value = ds.Tables[0].Rows[0]["Receiver_address"];
                sht.Range("G14:K16").Style.Font.FontName = "Tahoma";
                sht.Range("G14:K16").Style.Font.FontSize = 12;
                sht.Range("G14:K16").Style.Alignment.WrapText = true;
                sht.Range("G14:K16").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("G14:K16").Merge();
                //****** 2.
                sht.Range("G17").Value = "2.Paying Agent : " + ds.Tables[0].Rows[0]["payingagent"];
                sht.Range("G17:K17").Style.Font.FontName = "Tahoma";
                sht.Range("G17:K17").Style.Font.FontSize = 12;
                sht.Range("G17:K17").Style.Font.Bold = true;
                sht.Range("G17:K17").Merge();
                //*
                sht.Range("I18").Value = ds.Tables[0].Rows[0]["payingagent_address"];
                sht.Range("I18:K20").Style.Font.FontName = "Tahoma";
                sht.Range("I18:K20").Style.Font.FontSize = 12;
                sht.Range("I18:K20").Style.Alignment.WrapText = true;
                sht.Range("I18:K20").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("I18:K20").Merge();
                //*******3.
                sht.Range("G21").Value = "Buyer (If other than Consignee)";
                sht.Range("G21:K21").Style.Font.FontName = "Tahoma";
                sht.Range("G21:K21").Style.Font.FontSize = 12;
                sht.Range("G21:K21").Merge();

                sht.Range("G22").Value = "3." + ds.Tables[0].Rows[0]["buyer_otherthanconsignee"];
                sht.Range("G22:K22").Style.Font.FontName = "Tahoma";
                sht.Range("G22:K22").Style.Font.FontSize = 12;
                sht.Range("G22:K22").Merge();
                //*
                sht.Range("G23").Value = ds.Tables[0].Rows[0]["otherthanconsignee_address"];
                sht.Range("G23:K25").Style.Font.FontName = "Tahoma";
                sht.Range("G23:K25").Style.Font.FontSize = 12;
                sht.Range("G23:K25").Style.Alignment.WrapText = true;
                sht.Range("G23:K25").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("G23:K25").Merge();
                //***********Pre-carriage By
                sht.Range("A25").Value = "Pre-Carriage By";
                sht.Range("A25:C25").Style.Font.FontName = "Tahoma";
                sht.Range("A25:C25").Style.Font.FontSize = 12;
                sht.Range("A25:C25").Merge();
                //value
                sht.Range("A26").Value = ds.Tables[0].Rows[0]["Pre_carriageby"];
                sht.Range("A26:C26").Style.Font.FontName = "Tahoma";
                sht.Range("A26:C26").Style.Font.FontSize = 12;
                sht.Range("A26:C26").Merge();
                sht.Range("A26:C26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Place of Receipt by Pre-Carrier
                sht.Range("D25").Value = "Place of Receipt by Pre-Carrier";
                sht.Range("D25:F25").Style.Font.FontName = "Tahoma";
                sht.Range("D25:F25").Style.Font.FontSize = 12;
                sht.Range("D25:F25").Merge();
                //value
                sht.Range("D26").Value = ds.Tables[0].Rows[0]["receiptby"];
                sht.Range("D26:F26").Style.Font.FontName = "Tahoma";
                sht.Range("D26:F26").Style.Font.FontSize = 12;
                sht.Range("D26:F26").Merge();
                sht.Range("D26:F26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Vessel/Flight No
                sht.Range("A27").Value = "Vessel/Flight No";
                sht.Range("A27:C27").Style.Font.FontName = "Tahoma";
                sht.Range("A27:C27").Style.Font.FontSize = 12;
                sht.Range("A27:C27").Merge();
                //value
                sht.Range("A28").Value = ds.Tables[0].Rows[0]["vessel_flightno"];
                sht.Range("A28:C28").Style.Font.FontName = "Tahoma";
                sht.Range("A28:C28").Style.Font.FontSize = 12;
                sht.Range("A28:C28").Merge();
                sht.Range("A28:C28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Loading
                sht.Range("D27").Value = "Port of Loading";
                sht.Range("D27:F27").Style.Font.FontName = "Tahoma";
                sht.Range("D27:F27").Style.Font.FontSize = 12;
                sht.Range("D27:F27").Merge();
                //value
                sht.Range("D28").Value = ds.Tables[0].Rows[0]["portload"];
                sht.Range("D28:F28").Style.Font.FontName = "Tahoma";
                sht.Range("D28:F28").Style.Font.FontSize = 12;
                sht.Range("D28:F28").Merge();
                sht.Range("D28:F28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Discharge
                sht.Range("A29").Value = "Port of Discharge";
                sht.Range("A29:C29").Style.Font.FontName = "Tahoma";
                sht.Range("A29:C29").Style.Font.FontSize = 12;
                sht.Range("A29:C29").Merge();
                //value
                sht.Range("A30").Value = ds.Tables[0].Rows[0]["portunload"];
                sht.Range("A30:C30").Style.Font.FontName = "Tahoma";
                sht.Range("A30:C30").Style.Font.FontSize = 12;
                sht.Range("A30:C30").Merge();
                sht.Range("A30:C30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Final Destination
                sht.Range("D29").Value = "Final Destination";
                sht.Range("D29:F29").Style.Font.FontName = "Tahoma";
                sht.Range("D29:F29").Style.Font.FontSize = 12;
                sht.Range("D29:F29").Merge();
                //value
                sht.Range("D30").Value = ds.Tables[0].Rows[0]["Destinationadd"];
                sht.Range("D30:F30").Style.Font.FontName = "Tahoma";
                sht.Range("D30:F30").Style.Font.FontSize = 12;
                sht.Range("D30:F30").Merge();
                sht.Range("D30:F30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Origin of Goods
                sht.Range("G26").Value = "Country of Origin of Goods";
                sht.Range("G26:I26").Style.Font.FontName = "Tahoma";
                sht.Range("G26:I26").Style.Font.FontSize = 12;
                sht.Range("G26:I26").Merge();
                //value
                sht.Range("G27").Value = ds.Tables[0].Rows[0]["countryoforigin"];
                sht.Range("G27:I27").Style.Font.FontName = "Tahoma";
                sht.Range("G27:I27").Style.Font.FontSize = 12;
                sht.Range("G27:I27").Merge();
                sht.Range("G27:I27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Final Destination
                sht.Range("J26").Value = "Country of Final Destination";
                sht.Range("J26:K26").Style.Font.FontName = "Tahoma";
                sht.Range("J26:K26").Style.Font.FontSize = 12;
                sht.Range("J26:K26").Merge();
                //value
                sht.Range("J27").Value = ds.Tables[0].Rows[0]["countryoffinaldest"];
                sht.Range("J27:K27").Style.Font.FontName = "Tahoma";
                sht.Range("J27:K27").Style.Font.FontSize = 12;
                sht.Range("J27:K27").Style.NumberFormat.Format = "@";
                sht.Range("J27:K27").Merge();
                sht.Range("J27:K27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //***********Terms of Delivery and Payment
                sht.Range("G28").Value = "Terms of Delivery and Payment";
                sht.Range("G28:K28").Style.Font.FontName = "Tahoma";
                sht.Range("G28:K28").Style.Font.FontSize = 12;
                sht.Range("G28:K28").Merge();
                //value
                sht.Range("H29").Value = ds.Tables[0].Rows[0]["Deliveryterm"];
                sht.Range("H29:K30").Style.Font.FontName = "Tahoma";
                sht.Range("H29:K30").Style.Font.FontSize = 12;
                sht.Range("H29:K30").Style.NumberFormat.Format = "@";
                sht.Range("H29:K30").Style.Alignment.WrapText = true;
                sht.Range("H29:K30").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("H29:K30").Merge();
                //*****************Nos
                sht.Range("A31").Value = "Nos.";
                sht.Range("B31").Value = "No and kind of Packages";
                sht.Range("B31:C31").Merge();
                sht.Range("E31").Value = "Description of Goods";
                sht.Range("E31:H31").Merge();
                sht.Range("I31").Value = "Quantity";
                sht.Range("J31").Value = "Rate";
                sht.Range("k31").Value = "Amount";
                sht.Range("A31:K31").Style.Font.FontName = "Tahoma";
                sht.Range("A31:K31").Style.Font.FontSize = 12;
                sht.Range("A31:K31").Style.NumberFormat.Format = "@";
                sht.Range("K31").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //values
                sht.Range("A32:H32").Merge();
                sht.Range("A33:H37").Style.Font.FontName = "Tahoma";
                sht.Range("A33:H37").Style.Font.FontSize = 12;
                sht.Range("A33:H37").Style.NumberFormat.Format = "@";

                sht.Range("A33").SetValue(ds.Tables[0].Compute("Min(MinrollNo)", "") + "/" + ds.Tables[0].Compute("Max(Maxrollno)", ""));

                sht.Range("B33").Value = ds.Tables[0].Rows[0]["Noofrolls"] + " Packages";
                sht.Range("B33:C33").Merge();
                sht.Range("D33").Value = ds.Tables[0].Rows[0]["descriptionofgoods"];
                sht.Range("D33:H37").Style.Alignment.WrapText = true;
                sht.Range("D33:H37").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("D33:H37").Merge();
                sht.Range("A38:H38").Merge();
                //********Details
                sht.Range("A39").Value = "P.O.NO.";
                sht.Range("A39:K39").Style.Font.FontName = "Tahoma";
                sht.Range("A39:K39").Style.Font.FontSize = 12;
                sht.Range("A39:K39").Style.Font.Bold = true;
                sht.Range("A39:K39").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range("B39").Value = "ARTICLE NAME";
                sht.Range("B39:C39").Merge();
                sht.Range("D39").Value = "ART.NO.";
                sht.Range("D39:E39").Merge();
                sht.Range("F39").Value = "COLOR";
                sht.Range("G39").Value = "SIZE(CM)";
                sht.Range("H39").Value = "AREA SQ.MTR.";
                sht.Range("I39").Value = "QTY";
                sht.Range("J39").Value = "FCA/" + ds.Tables[0].Rows[0]["CIF"];
                sht.Range("K39").Value = "FCA/" + ds.Tables[0].Rows[0]["CIF"];
                sht.Range("K39").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //***********generate Loop
                i = 40;
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
                    sht.Range("H" + i).SetValue(ds.Tables[0].Rows[ii]["Area"]);
                    Area = Decimal.Parse(Area.ToString()) + Decimal.Parse(ds.Tables[0].Rows[ii]["Area"].ToString());
                    //QTY
                    sht.Range("I" + i).SetValue(ds.Tables[0].Rows[ii]["Pcs"]);
                    Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[ii]["Pcs"]);
                    //
                    var _with27 = sht.Range("I" + i);
                    _with27.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    _with27.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //rate
                    sht.Range("J" + i).SetValue(ds.Tables[0].Rows[ii]["Price"]);

                    var _with28 = sht.Range("J" + i);
                    _with28.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    _with28.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //Amount
                    sht.Range("K" + i).SetValue(ds.Tables[0].Rows[ii]["amount"]);
                    Amount = Amount + Convert.ToDecimal(ds.Tables[0].Rows[ii]["amount"]);
                    //
                    sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    // sht.Range("H" + i + ":K" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("I" + i).Style.NumberFormat.Format = "@";
                    sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("H" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("K" + i).Style.NumberFormat.Format = "#,###0.00";
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
                //sht.Range("G" + i + ":I" + i).Style.NumberFormat.Format = "@";
                sht.Range("G" + i).Style.NumberFormat.Format = "@";
                sht.Range("I" + i).Style.NumberFormat.Format = "@";
                sht.Range("G" + i + ":I" + i).Style.Font.Bold = true;
                sht.Range("G" + i + ":I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range("G" + i).Value = "Total:";
                sht.Range("H" + i).SetValue(Area);
                sht.Range("I" + i).SetValue(Pcs);
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
                var _with31 = sht.Range("I" + i + ":I" + (i + 5));
                _with31.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with31.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                sht.Range("A" + i + ":A" + (i + 5)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 5)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                var _with32 = sht.Range("J" + i + ":J" + (i + 5));
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
                //***************
                i = i + 5;
                sht.Range("A" + i + ":G" + i).Merge();
                sht.Range("A" + i).SetValue("The exporter Eastern Home Industries of the product covered by this document Declares that, ");
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                using (var a = sht.Range("I" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("J" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                i = i + 1;
                sht.Range("A" + i + ":G" + i).Merge();
                sht.Range("A" + i).SetValue("except where otherwise clearly indicated, these products are of …(Indian)…. Preferential origin");
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                using (var a = sht.Range("I" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("J" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                i = i + 1;
                sht.Range("A" + i + ":G" + i).Merge();
                sht.Range("A" + i).SetValue("according to the rules of origin of the Generalised System of the Eouropeon Union and ");
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                using (var a = sht.Range("I" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("J" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                i = i + 1;
                sht.Range("A" + i + ":G" + i).Merge();
                sht.Range("A" + i).SetValue("that the origin criterion met is …\"P\"");
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                using (var a = sht.Range("I" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("J" + i))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                //**********Article Loop          
                //
                i = i + 2;

                //i = i + 5;
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
                str = "Select Design as ArticleNo,contents,Round(sum(amount)/sum(Area),2) as Rate From V_PackingDetailsIkea Where PackingId=" + DDinvoiceNo.SelectedValue + " group by Design,contents";
                DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                    {
                        sht.Range("A" + i).SetValue(ds1.Tables[0].Rows[j]["articleno"]);
                        sht.Range("A" + i + ":B" + i).Merge();
                        sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Tahoma";
                        sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 12;
                        sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";
                        //
                        sht.Range("D" + i).SetValue(ds1.Tables[0].Rows[j]["contents"]);
                        sht.Range("D" + i + ":F" + i).Merge();
                        sht.Range("D" + i + ":F" + i).Style.Font.FontName = "Tahoma";
                        sht.Range("D" + i + ":F" + i).Style.Font.FontSize = 12;
                        sht.Range("D" + i + ":F" + i).Style.NumberFormat.Format = "@";
                        //rate
                        sht.Range("G" + i).SetValue(ds1.Tables[0].Rows[j]["rate"]);
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
                string amt = Amount.ToString();
                string val = "", paise = "";

                if (amt.IndexOf('.') > 0)
                {
                    val = amt.ToString().Split('.')[0];
                    amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val));
                }
                else
                {
                    amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(Amount));
                }

                string Pointamt = string.Format("{0:0.00}", Amount.ToString("0.00"));
                val = "";
                if (Pointamt.IndexOf('.') > 0)
                {
                    val = Pointamt.ToString().Split('.')[1];
                    if (Convert.ToInt32(val) > 0)
                    {
                        paise = ds.Tables[0].Rows[0]["Currencytypeps"] + " " + ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val)) + " ";
                    }
                }
                amountinwords = ds.Tables[0].Rows[0]["CIF"] + " " + amountinwords + " " + paise + "Only";
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
                sht.Range("K" + i).SetValue(Amount);
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
                sht.Range("C" + i).SetValue(ds.Tables[0].Rows[0]["Noofrolls"]);
                sht.Range("C" + (i + 1)).SetValue(Pcs);
                sht.Range("C" + (i + 2)).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["grosswt"]) + " Kgs.";
                sht.Range("C" + (i + 3)).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["netwt"]) + " Kgs.";
                sht.Range("C" + (i + 4)).Value = String.Format("{0:#,0.00}", Area) + " Sq.Mtr";
                sht.Range("C" + (i + 5)).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["Volume"]) + " CBM";
                //******Central Excise
                //sht.Range("F" + i).Value = "Central Excise No. AADFE8796LEM001";
                //sht.Range("F" + (i + 1)).Value = "Range: Bhadohi";
                //sht.Range("F" + (i + 2)).Value = "Division: Allahabad-I";
                //sht.Range("F" + (i + 3)).Value = "Commissionerate: Allahabad";
                //sht.Range("F" + (i + 4)).Value = "Tariff Heading# 570201";
                //sht.Range("F" + i + ":H" + (i + 4)).Style.Font.FontName = "Tahoma";
                //sht.Range("F" + i + ":H" + (i + 4)).Style.Font.FontSize = 12;
                //sht.Range("F" + i + ":H" + (i + 4)).Style.NumberFormat.Format = "@";
                //sht.Range("F" + i + ":H" + (i + 4)).Style.Font.Bold = true;
                //sht.Range("F" + i + ":H" + i).Merge();
                //sht.Range("F" + (i + 1) + ":H" + (i + 1)).Merge();
                //sht.Range("F" + (i + 2) + ":H" + (i + 2)).Merge();
                //sht.Range("F" + (i + 3) + ":H" + (i + 3)).Merge();
                //sht.Range("F" + (i + 4) + ":H" + (i + 4)).Merge();
                sht.Range("F" + i + ":K" + (i + 2)).Merge();
                sht.Range("F" + i).SetValue("HSN CODE# " + ds.Tables[0].Rows[0]["hsncode"]);
                sht.Range("F" + i + ":K" + (i + 2)).Style.Font.FontName = "Tahoma";
                sht.Range("F" + i + ":K" + (i + 2)).Style.Font.FontSize = 12;
                sht.Range("F" + i + ":K" + (i + 2)).Style.NumberFormat.Format = "@";
                sht.Range("F" + i + ":K" + (i + 2)).Style.Font.Bold = true;
                sht.Range("F" + i + ":K" + (i + 2)).Style.Alignment.WrapText = true;
                sht.Range("F" + i + ":K" + (i + 2)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                sht.Range("A" + i + ":A" + (i + 5)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 5)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //*******************************GSTAmount

                //********Details
                i = i + 7;
                sht.Range("A" + i + ":K" + i).Merge();
                sht.Range("A" + i).Value = "Supply Meant for Export With Payment of Integrated Tax(" + ds.Tables[0].Rows[0]["GSTTypeName"].ToString() + ")";
                sht.Range("A" + i + ":K" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":K" + i).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":K" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                i = i + 1;
                sht.Range("A" + i).Value = "Description";
                sht.Range("A" + i + ":K" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":K" + i).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":K" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range("B" + i).Value = "HSN";
                sht.Range("C" + i).Value = "Qty PCS";
                sht.Range("D" + i).Value = "Rate" + Environment.NewLine + "FCA/" + ds.Tables[0].Rows[0]["CIF"];
                sht.Range("E" + i).Value = "Amount";
                sht.Range("F" + i).Value = "CGST";
                sht.Range("G" + i).Value = "SGST";
                sht.Range("H" + i).Value = "IGST";
                sht.Range("I" + i).Value = "GST Amount";
                sht.Range("J" + i).Value = "Total Amount" + Environment.NewLine + " Including GST";
                sht.Row(i).Height = 30;
                sht.Range("I" + i + ":J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                //***********generate Loop
                i = i + 1;
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    sht.Range("A" + i + ":K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("A" + i + ":K" + i).Style.Font.FontSize = 12;
                    sht.Range("A" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //pono
                    sht.Range("A" + i).Value = ds.Tables[0].Rows[j]["Description"].ToString();
                    //Article Name
                    sht.Range("B" + i).Value = ds.Tables[0].Rows[j]["Hsncode"];
                    //sht.Range("B" + i, "C" + i).Merge();
                    //Art No.
                    sht.Range("C" + i).Value = ds.Tables[0].Rows[j]["Pcs"];
                    TotalPcs = TotalPcs + Convert.ToInt16(ds.Tables[0].Rows[j]["Pcs"]);
                    //sht.Range("D" + i, "E" + i).Merge();
                    sht.Range("D" + i).Value = ds.Tables[0].Rows[j]["Price"];
                    sht.Range("E" + i).Value = ds.Tables[0].Rows[j]["amount"];
                    TotalAmount = TotalAmount + Convert.ToDecimal(ds.Tables[0].Rows[j]["amount"]);
                    sht.Range("F" + i).Value = ds.Tables[0].Rows[j]["CGST"];
                    sht.Range("G" + i).Value = ds.Tables[0].Rows[j]["SGST"];
                    sht.Range("H" + i).Value = ds.Tables[0].Rows[j]["IGST1"];

                    if (ds.Tables[0].Rows[j]["GSTType"].ToString() == "1")
                    {
                        GSTAmount = (Convert.ToDecimal(ds.Tables[0].Rows[j]["amount"]) / (Convert.ToDecimal(ds.Tables[0].Rows[j]["CGST"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["SGST"])) * 100);
                        TotalGSTAmount = TotalGSTAmount + GSTAmount;
                    }
                    else if (ds.Tables[0].Rows[j]["GSTType"].ToString() == "2")
                    {
                        GSTAmount = (Convert.ToDecimal(ds.Tables[0].Rows[j]["amount"]) / (Convert.ToDecimal(ds.Tables[0].Rows[j]["IGST1"])) * 100);
                        TotalGSTAmount = TotalGSTAmount + GSTAmount;
                    }
                    else
                    {
                        GSTAmount = 0;
                    }

                    sht.Range("I" + i).Value = GSTAmount;
                    sht.Range("J" + i).Value = Convert.ToDecimal(ds.Tables[0].Rows[j]["amount"]) + GSTAmount;
                    TotalAmountWithGST = TotalAmountWithGST + Convert.ToDecimal(ds.Tables[0].Rows[j]["amount"]) + GSTAmount;

                    var _with27 = sht.Range("I" + i);
                    _with27.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    _with27.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ////rate
                    //sht.Range("J" + i).SetValue(ds.Tables[0].Rows[ii]["Price"]);

                    var _with28 = sht.Range("J" + i);
                    _with28.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    _with28.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ////Amount
                    //sht.Range("K" + i).SetValue(ds.Tables[0].Rows[ii]["amount"]);
                    //Amount = Amount + Convert.ToDecimal(ds.Tables[0].Rows[ii]["amount"]);
                    //
                    sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    // sht.Range("H" + i + ":K" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("C" + i).Style.NumberFormat.Format = "@";
                    sht.Range("E" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("I" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                    //border
                    sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    i = i + 1;
                }

                i = i + 1;
                sht.Range("B" + i).Value = "Total:";
                sht.Range("C" + i).SetValue(TotalPcs);
                sht.Range("E" + i).SetValue(TotalAmount);
                sht.Range("I" + i).SetValue(TotalGSTAmount);
                sht.Range("J" + i).SetValue(TotalAmountWithGST);

                sht.Range("B" + i + ":J" + i).Style.Font.FontName = "Tahoma";
                sht.Range("B" + i + ":J" + i).Style.Font.FontSize = 12;
                sht.Range("B" + i + ":J" + i).Style.NumberFormat.Format = "@";
                sht.Range("B" + i + ":J" + i).Style.Font.Bold = true;

                //sht.Range("A" + i + ":F" + i).Merge();
                sht.Range("E" + i).Style.NumberFormat.Format = "#,###0.00";
                sht.Range("I" + i).Style.NumberFormat.Format = "#,###0.00";
                sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                ////border
                //var _with244 = sht.Range("C" + i);
                //_with244.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //_with244.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with245 = sht.Range("C" + i + ":J" + i);
                _with245.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                _with245.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with245.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with245.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                i = i + 2;
                sht.Range("A" + i + ":G" + i).Merge();
                sht.Range("A" + i).Value = "STATEMENT ON ORIGIN";
                sht.Range("A" + i + ":G" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":G" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":G" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                var _with466 = sht.Range("A" + i + ":K" + i);
                _with466.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with466.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                i = i + 1;
                sht.Range("A" + i + ":G" + i).Merge();
                sht.Range("A" + i).Value = "The Exporter INREX0694000965EC026 of the products covered by this document declares that, except where otherwise clearly indicated, these products are of INDIA preferential origin according to rules of origin of the Generalized system of preferences of the European Union and that the origin criterion met is 'P'";
                sht.Range("A" + i + ":G" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":G" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":G" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Justify);
                sht.Row(i).Height = 50;

                var _with467 = sht.Range("A" + i + ":K" + i);
                _with467.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with467.Style.Border.RightBorder = XLBorderStyleValues.Thin;


                i = i + 1;
                sht.Range("A" + i + ":C" + i).Merge();
                sht.Range("A" + i).Value = "R.B.I Code No. 02422282600008";
                sht.Range("A" + i + ":C" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":C" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":C" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":C" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                var _with468 = sht.Range("A" + i + ":K" + i);
                _with468.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with468.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                sht.Range("E" + i + ":G" + i).Merge();
                sht.Range("E" + i).Value = "Factory Address:";
                sht.Range("E" + i + ":G" + i).Style.Font.FontName = "Tahoma";
                sht.Range("E" + i + ":G" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("E" + i + ":G" + i).Style.Font.Bold = true;
                sht.Range("E" + i + ":G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                var _with469 = sht.Range("A" + i + ":K" + i);
                _with469.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with469.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                i = i + 1;
                sht.Range("A" + i + ":C" + i).Merge();
                sht.Range("A" + i).Value = "I.E.  Code  No.: 0694000965";
                sht.Range("A" + i + ":C" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":C" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":C" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":C" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("E" + i + ":G" + i).Merge();
                sht.Range("E" + i).Value = " AGRA-MATHURA ROAD, ARTONI" + Environment.NewLine + "AGRA-282 007 (INDIA)";
                sht.Range("E" + i + ":G" + i).Style.Font.FontName = "Tahoma";
                sht.Range("E" + i + ":G" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("E" + i + ":G" + i).Style.Font.Bold = true;
                sht.Range("E" + i + ":G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Row(i).Height = 30;

                var _with560 = sht.Range("A" + i + ":K" + i);
                _with560.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with560.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //***********Sig and date
                i = i + 2;
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
                sht.Range("I" + i + ":J" + i).Merge();
                sht.Range("I" + i + ":J" + i).Value = ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("I" + i + ":J" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i + ":J" + i).Style.Font.FontSize = 12;
                sht.Range("I" + i + ":J" + i).Style.NumberFormat.Format = "dd/MMM/yyyy";
                sht.Range("I" + i + ":J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

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
                var _with2 = sht.Range("A11:K11");
                _with2.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with3 = sht.Range("G3:K4");
                _with3.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with3.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                var _with41 = sht.Range("G3:K3");
                _with41.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                var _with40 = sht.Range("G4:K4");
                _with40.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with4 = sht.Range("G5:I6");
                _with4.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with4.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                var _with42 = sht.Range("G5:I5");
                _with42.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                var _with43 = sht.Range("G6:I6");
                _with43.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                //
                var _with5 = sht.Range("J5:K5");
                _with5.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                var _with44 = sht.Range("J5:K6");
                _with44.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _with45 = sht.Range("J6:K6");
                _with45.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with6 = sht.Range("G7:K8");
                _with6.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with6.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                var _with46 = sht.Range("G8:K8");
                _with46.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with7 = sht.Range("G9:K11");
                _with7.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with7.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                var _with47 = sht.Range("G12:K25");
                _with47.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("G12:I12").Style.Border.RightBorder = XLBorderStyleValues.None;

                //
                var _with8 = sht.Range("A24:F24");
                _with8.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                var _with48 = sht.Range("A12:F24");
                _with48.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                sht.Range("A12:B12").Style.Border.RightBorder = XLBorderStyleValues.None;
                sht.Range("A18:B18").Style.Border.RightBorder = XLBorderStyleValues.None;
                sht.Range("G12:H12").Style.Border.RightBorder = XLBorderStyleValues.None;
                sht.Range("G18:H20").Style.Border.RightBorder = XLBorderStyleValues.None;

                //
                var _with9 = sht.Range("A25:C26");
                _with9.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with9.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //sht.Range("A24:C24").Style.Border.BottomBorder = XLBorderStyleValues.None;

                sht.Range("A26:C26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                sht.Range("G26:I26").Style.Border.BottomBorder = XLBorderStyleValues.None;

                sht.Range("J26:K26").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //
                var _with10 = sht.Range("D25:F26");
                _with10.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with10.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D25:F25").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //
                var _with11 = sht.Range("A27:C28");
                _with11.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with11.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with11.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A27:C27").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //
                var _with12 = sht.Range("D27:F28");
                _with12.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with12.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D27:F27").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //
                var _with13 = sht.Range("A29:C30");
                _with13.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with13.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A29:C29").Style.Border.BottomBorder = XLBorderStyleValues.None;
                sht.Range("A30:C30").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with14 = sht.Range("D29:F30");
                _with14.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with14.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D29:G29").Style.Border.BottomBorder = XLBorderStyleValues.None;
                sht.Range("G29").Style.Border.BottomBorder = XLBorderStyleValues.None;

                //
                var _with15 = sht.Range("G26:I27");
                _with15.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with15.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("G26:I26").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("G27:I27").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with16 = sht.Range("J26:K27");
                _with16.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("J26:K26").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("J27:K27").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with17 = sht.Range("G28:K30");
                _with17.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with17.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("G30:K30").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("G29:G30").Style.Border.RightBorder = XLBorderStyleValues.None;
                sht.Range("H29:K30").Style.Border.LeftBorder = XLBorderStyleValues.None;
                //
                var _with18 = sht.Range("I31:I38");
                _with18.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with18.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                var _with19 = sht.Range("J31:J38");
                _with19.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with19.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                var _with20 = sht.Range("A39:K39");
                _with20.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                _with20.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with21 = sht.Range("I39:I39");
                _with21.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with21.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                var _with22 = sht.Range("J39:J39");
                _with22.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with22.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //
                sht.Range("A31:A38").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K31:K38").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A39:A39").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K38:K38").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A2:K2").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A3:A24").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                string Fileextension = "xlsx";
                string filename = "Invoice-" + validateFilename(ds.Tables[0].Rows[0]["TinvoiceNo"].ToString()) + "." + Fileextension + "";
                Path = Server.MapPath("~/InvoiceExcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
                lblmsg.Text = "Invoice Excel Format downloaded successfully.";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "AltR1", "alert('No Record Found to Export')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
    protected void Exporttoexcel_Kaysons_new()
    {
        lblmsg.Text = "Wait For Excel File to Download";
        if (!Directory.Exists(Server.MapPath("~/InvoiceExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/InvoiceExcel/"));
        }
        string Path = "";
        try
        {

            //******************
            string str = "";
            if (variable.VarGSTForInvoiceFormNew == "1")
            {
                str = @"select * From V_InvoiceDetail VI inner join V_PackingDetailsIkea_WithGST VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue;
            }
            else
            {
                str = @"select * From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue;
            }
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int i, Pcs = 0, TotalPcs = 0;
                Decimal Area = 0, Amount = 0, TotalAmount = 0, GSTAmount = 0, TotalGSTAmount = 0, TotalAmountWithGST = 0, netwt = 0;
                String ECIS = string.Empty, artno = string.Empty;

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("Invoice1");

                //************set cell width
                //Page
                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                sht.PageSetup.AdjustTo(95);
                sht.PageSetup.FitToPages(1, 1);
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                sht.PageSetup.VerticalDpi = 300;
                sht.PageSetup.HorizontalDpi = 300;
                //
                sht.PageSetup.Margins.Top = 0.1;
                sht.PageSetup.Margins.Left = 0.1;
                sht.PageSetup.Margins.Right = 0;
                sht.PageSetup.Margins.Bottom = 0.01;
                sht.PageSetup.Margins.Header = 0.1;
                sht.PageSetup.Margins.Footer = 0.1;
                //sht.pagesetup.centerhorizontally = true;
                //sht.pagesetup.centervertically = true;
                sht.PageSetup.SetScaleHFWithDocument();
                //************
                sht.Column("A").Width = 20.71;
                sht.Column("B").Width = 8.86;
                sht.Column("C").Width = 18.00;
                sht.Column("D").Width = 15.29;
                sht.Column("E").Width = 10.43;
                sht.Column("F").Width = 15.86;
                sht.Column("G").Width = 12.86;
                sht.Column("H").Width = 18.29;
                sht.Column("I").Width = 20.71;
                sht.Column("J").Width = 20.29;
                sht.Column("K").Width = 23.14;
                //************
                sht.Row(1).Height = 15.75;
                //*****Header                
                sht.Cell("A2").Value = "INVOICE";
                sht.Range("A2:K2").Style.Font.FontName = "Tahoma";
                sht.Range("A2:K2").Style.Font.FontSize = 15;
                sht.Range("A2:K2").Style.Font.Bold = true;
                sht.Range("A2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:K2").Merge();
                //***************
                //sht.Cell("A2").Value = @"""SUPPLY MEANT FOR EXPORT UNDER LETTER OF UNDERTAKING NO-21/2017 Dt.07.07.2017. WITHOUT PAYMENT OF INTEGRATED TAX""";
                //sht.Range("A2:K2").Style.Font.FontName = "Tahoma";
                //sht.Range("A2:K2").Style.Font.FontSize = 12;
                //sht.Range("A2:K2").Style.Font.Bold = true;
                //sht.Range("A2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Range("A2:K2").Merge();
                using (var a = sht.Range("A2:K2"))
                {
                    //a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //********Exporter
                sht.Range("A3").Value = "Exporter";
                sht.Range("A3:F3").Style.Font.FontName = "Tahoma";
                sht.Range("A3:F3").Style.Font.FontSize = 11;
                sht.Range("A3:F3").Merge();
                //*****************

                //CompanyName
                sht.Range("A4").Value = ds.Tables[0].Rows[0]["companyName"];
                sht.Range("A4:F4").Style.Font.FontName = "Tahoma";
                sht.Range("A4:F4").Style.Font.FontSize = 11;
                sht.Range("A4:F4").Style.Font.Bold = true;
                sht.Range("A4:F4").Merge();
                //Address
                sht.Range("A5").Value = ds.Tables[0].Rows[0]["compaddr1"];
                sht.Range("A5:F5").Style.Font.FontName = "Tahoma";
                sht.Range("A5:F5").Style.Font.FontSize = 11;
                sht.Range("A5:F5").Merge();
                //address2
                sht.Range("A6").Value = ds.Tables[0].Rows[0]["compaddr2"] + "," + ds.Tables[0].Rows[0]["compaddr3"];
                sht.Range("A6:F6").Style.Font.FontName = "Tahoma";
                sht.Range("A6:F6").Style.Font.FontSize = 11;
                sht.Range("A6:F6").Merge();
                //TiN No
                sht.Range("A7").Value = "GSTIN#" + ds.Tables[0].Rows[0]["gstno"];
                sht.Range("A7:F7").Style.Font.FontName = "Tahoma";
                sht.Range("A7:F7").Style.Font.FontSize = 11;
                sht.Range("A7:F7").Style.Font.Bold = true;
                sht.Range("A7:F7").Merge();

                sht.Range("A9").Value = "STATE:" + "Uttar Pradesh";
                sht.Range("A9:F9").Style.Font.FontName = "Tahoma";
                sht.Range("A9:F9").Style.Font.FontSize = 11;
                sht.Range("A9:F9").Style.Font.Bold = true;
                sht.Range("A9:F9").Merge();

                sht.Range("A11").Value = "PAN:" + ds.Tables[0].Rows[0]["panno"];
                sht.Range("A11:F11").Style.Font.FontName = "Tahoma";
                sht.Range("A11:F11").Style.Font.FontSize = 11;
                sht.Range("A11:F11").Style.Font.Bold = true;
                sht.Range("A11:F11").Merge();
                //**********INvoiceNodate
                sht.Range("G3").Value = "Invoice No.";
                sht.Range("G3:H3").Style.Font.FontName = "Tahoma";
                sht.Range("G3:H3").Style.Font.FontSize = 11;
                sht.Range("G3:H3").Merge();
                //value
                sht.Range("I3").Value = ds.Tables[0].Rows[0]["TInvoiceNo"];
                sht.Range("I3:I3").Style.Font.FontName = "Tahoma";
                sht.Range("I3:I3").Style.Font.FontSize = 11;
                sht.Range("I3:I3").Merge();
                sht.Range("I3:I3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("J3").Value = " Dated : " + ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("J3:K3").Style.Font.FontName = "Tahoma";
                sht.Range("J3:K3").Style.Font.FontSize = 11;
                sht.Range("J3:K3").Merge();
                //sht.Range("G3").Value = "Invoice No.";
                //sht.Range("G3:K3").Style.Font.FontName = "Tahoma";
                //sht.Range("G3:K3").Style.Font.FontSize = 11;
                //sht.Range("G3:K3").Merge();
                ////value
                //sht.Range("G4").Value = ds.Tables[0].Rows[0]["TInvoiceNo"] + " Dated " + ds.Tables[0].Rows[0]["InvoiceDate"];
                //sht.Range("G4:K4").Style.Font.FontName = "Tahoma";
                //sht.Range("G4:K4").Style.Font.FontSize = 11;
                //sht.Range("G4:K4").Merge();
                //sht.Range("G4", "K4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //*************IE Code
                sht.Range("G5").Value = "Buyer's Order No. : As Mentioned Below ";
                sht.Range("G5:I5").Style.Font.FontName = "Tahoma";
                sht.Range("G5:I5").Style.Font.FontSize = 11;
                sht.Range("G5:I5").Merge();
                //value
                sht.Range("G6").Value = ds.Tables[0].Rows[0]["TorderNo"];
                sht.Range("G6:I6").Style.Font.FontName = "Tahoma";
                sht.Range("G6:I6").Style.Font.FontSize = 11;
                sht.Range("G6:I6").Merge();
                sht.Range("G6", "I6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //***********GRI Form No
                //sht.Range("J5").Value = "REX No.";
                //sht.Range("J5:K5").Style.Font.FontName = "Tahoma";
                //sht.Range("J5:K5").Style.Font.FontSize = 11;
                //sht.Range("J5:K5").Merge();
                //// value
                //sht.Range("J6").Value = "INREX1510001883EC025";
                //sht.Range("J6:K6").Style.Font.FontName = "Tahoma";
                //sht.Range("J6:K6").Style.Font.FontSize = 11;
                //sht.Range("J6:K6").Merge();
                //sht.Range("J6", "K6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //*************Buyer's Order No and Date
                //sht.Range("G7").Value = "Buyer's Order No. / Date";
                //sht.Range("G7:K8").Style.Font.FontName = "Tahoma";
                //sht.Range("G7:K8").Style.Font.FontSize = 11;
                //sht.Range("G7:K7").Merge();
                //value
                //sht.Range("G8").SetValue(ds.Tables[0].Rows[0]["TorderNo"]);
                //sht.Range("G8:K8").Style.Font.FontName = "Tahoma";
                //sht.Range("G8:K8").Style.Font.FontSize = 11;
                //sht.Range("G8:K8").Merge();
                //sht.Range("G8", "K8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range("G8").Value = "SUPPLIER NO.: 21899";
                sht.Range("G8:I8").Style.Font.FontName = "Tahoma";
                sht.Range("G8:I8").Style.Font.FontSize = 11;
                sht.Range("G8:I8").Merge();
                //value
                //sht.Range("I8").Value = ds.Tables[0].Rows[0]["TInvoiceNo"];
                //sht.Range("I8:I8").Style.Font.FontName = "Tahoma";
                //sht.Range("I8:I8").Style.Font.FontSize = 11;
                //sht.Range("I8:I8").Merge();
                //sht.Range("I8:I8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("J8").Value = "DESTN. CODE :" + ds.Tables[0].Rows[0]["DestCode"];
                sht.Range("J8:K8").Style.Font.FontName = "Tahoma";
                sht.Range("J8:K8").Style.Font.FontSize = 11;
                sht.Range("J8:K8").Merge();
                //**************Other Reference(s)
                // sht.Range("G9").Value = "Other Reference(s)";
                sht.Range("G10").Value = "CONSIGNMENT NO.:" + ds.Tables[0].Rows[0]["otherref"];
                //  sht.Range("G11").Value = "SUPPLIER NO.: 21899";
                sht.Range("G9:G11").Style.Font.FontName = "Tahoma";
                sht.Range("G9:G11").Style.Font.FontSize = 11;
                sht.Range("G9:K9").Merge();
                sht.Range("G10:K10").Merge();
                sht.Range("G11:K11").Merge();
                //*************Consignee
                sht.Range("A12").Value = "Consignee";
                sht.Range("C12").Value = ds.Tables[0].Rows[0]["consignee_dt"];
                sht.Range("A12:B12").Style.Font.FontName = "Tahoma";
                sht.Range("A12:B12").Style.Font.FontSize = 11;

                sht.Range("C12:F12").Style.Font.FontName = "Tahoma";
                sht.Range("C12:F12").Style.Font.FontSize = 11;
                sht.Range("C12:F12").Style.Font.Bold = true;

                sht.Range("A12:B12").Merge();
                sht.Range("C12:F12").Merge();
                //value
                sht.Range("A13").Value = ds.Tables[0].Rows[0]["consignee"];
                sht.Range("A13:F13").Style.Font.FontName = "Tahoma";
                sht.Range("A13:F13").Style.Font.FontSize = 11;
                sht.Range("A13:F13").Style.Font.Bold = true;
                sht.Range("A13:F13").Merge();
                //**
                sht.Range("A14").Value = ds.Tables[0].Rows[0]["consignee_address"];
                sht.Range("A14:F17").Style.Font.FontName = "Tahoma";
                sht.Range("A14:F17").Style.Font.FontSize = 11;
                sht.Range("A14", "F17").Style.Alignment.WrapText = true;
                sht.Range("A14", "F17").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A14:F17").Merge();
                //***********Notify
                sht.Range("A18").Value = "Notify Party";
                sht.Range("C18").Value = ds.Tables[0].Rows[0]["Notifyparty_dt"];
                sht.Range("A18:B18").Style.Font.FontName = "Tahoma";
                sht.Range("A18:B18").Style.Font.FontSize = 11;

                sht.Range("A18:B18").Style.Font.SetUnderline();
                sht.Range("C18:F18").Style.Font.FontName = "Tahoma";
                sht.Range("C18:F18").Style.Font.FontSize = 11;
                sht.Range("C18:F18").Style.Font.Bold = true;

                sht.Range("A18:B18").Merge();
                sht.Range("C18:F18").Merge();
                //value
                sht.Range("A19").Value = ds.Tables[0].Rows[0]["Notifyparty"];
                sht.Range("A19:F19").Style.Font.FontName = "Tahoma";
                sht.Range("A19:F19").Style.Font.FontSize = 11;
                sht.Range("A19:F19").Style.Font.Bold = true;
                sht.Range("A19:F19").Merge();
                //
                sht.Range("A20").Value = ds.Tables[0].Rows[0]["notifyparty_address"];
                sht.Range("A20:F24").Style.Font.FontName = "Tahoma";
                sht.Range("A20:F24").Style.Font.FontSize = 11;
                sht.Range("A20", "F24").Style.Alignment.WrapText = true;
                sht.Range("A20", "F24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A20:F24").Merge();
                //***********Receiver address
                //sht.Range("G12").Value = "Receiver Address";
                //sht.Range("G12:I12").Style.Font.FontName = "Tahoma";
                //sht.Range("G12:I12").Style.Font.FontSize = 11;
                //sht.Range("G12:I12").Merge();
                ////values     2
                //sht.Range("J12").Value = ds.Tables[0].Rows[0]["Destcode"];
                //sht.Range("J12:K12").Style.Font.FontName = "Tahoma";
                //sht.Range("J12:K12").Style.Font.FontSize = 12;
                //sht.Range("J12:K12").Style.Font.Bold = true;
                //sht.Range("J12:K12").Merge();
                ////****** 1.
                //sht.Range("G13").Value = "1. " + ds.Tables[0].Rows[0]["Receiver"];
                //sht.Range("G13:K13").Style.Font.FontName = "Tahoma";
                //sht.Range("G13:K13").Style.Font.FontSize = 12;
                //sht.Range("G13:K13").Style.Font.Bold = true;
                //sht.Range("G13:K13").Merge();
                ////*
                //sht.Range("G14").Value = ds.Tables[0].Rows[0]["Receiver_address"];
                //sht.Range("G14:K16").Style.Font.FontName = "Tahoma";
                //sht.Range("G14:K16").Style.Font.FontSize = 12;
                //sht.Range("G14:K16").Style.Alignment.WrapText = true;
                //sht.Range("G14:K16").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                //sht.Range("G14:K16").Merge();
                sht.Range("G12").Value = "Buyer (If other than Consignee)";
                sht.Range("G12:K12").Style.Font.FontName = "Tahoma";
                sht.Range("G12:K12").Style.Font.FontSize = 11;
                sht.Range("G12:K12").Merge();

                sht.Range("G13").Value = "3." + ds.Tables[0].Rows[0]["buyer_otherthanconsignee"];
                sht.Range("G13:K13").Style.Font.FontName = "Tahoma";
                sht.Range("G13:K13").Style.Font.FontSize = 11;
                sht.Range("G13:K13").Merge();
                //*
                sht.Range("G14").Value = ds.Tables[0].Rows[0]["otherthanconsignee_address"];
                sht.Range("G14:K16").Style.Font.FontName = "Tahoma";
                sht.Range("G14:K16").Style.Font.FontSize = 11;
                sht.Range("G14:K16").Style.Alignment.WrapText = true;
                sht.Range("G14:K16").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("G14:K16").Merge();

                //****** 2.
                sht.Range("G18").Value = "2.Paying Agent : " + ds.Tables[0].Rows[0]["payingagent"];
                sht.Range("G18:K18").Style.Font.FontName = "Tahoma";
                sht.Range("G18:K18").Style.Font.FontSize = 11;
                sht.Range("G18:K18").Style.Font.Bold = true;
                sht.Range("G18:K18").Merge();
                //*
                sht.Range("I19").Value = ds.Tables[0].Rows[0]["payingagent_address"];
                sht.Range("I19:K21").Style.Font.FontName = "Tahoma";
                sht.Range("I19:K21").Style.Font.FontSize = 11;
                sht.Range("I19:K21").Style.Alignment.WrapText = true;
                sht.Range("I19:K21").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("I19:K21").Merge();
                //*******3.
                //sht.Range("G21").Value = "Buyer (If other than Consignee)";
                //sht.Range("G21:K21").Style.Font.FontName = "Tahoma";
                //sht.Range("G21:K21").Style.Font.FontSize = 11;
                //sht.Range("G21:K21").Merge();

                //sht.Range("G22").Value = "3." + ds.Tables[0].Rows[0]["buyer_otherthanconsignee"];
                //sht.Range("G22:K22").Style.Font.FontName = "Tahoma";
                //sht.Range("G22:K22").Style.Font.FontSize = 11;
                //sht.Range("G22:K22").Merge();
                ////*
                //sht.Range("G23").Value = ds.Tables[0].Rows[0]["otherthanconsignee_address"];
                //sht.Range("G23:K25").Style.Font.FontName = "Tahoma";
                //sht.Range("G23:K25").Style.Font.FontSize = 11;
                //sht.Range("G23:K25").Style.Alignment.WrapText = true;
                //sht.Range("G23:K25").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                //sht.Range("G23:K25").Merge();
                //***********Pre-carriage By
                sht.Range("A25").Value = "Pre-Carriage By";
                sht.Range("A25:C25").Style.Font.FontName = "Tahoma";
                sht.Range("A25:C25").Style.Font.FontSize = 11;
                sht.Range("A25:C25").Merge();
                //value
                sht.Range("A26").Value = ds.Tables[0].Rows[0]["Pre_carriageby"];
                sht.Range("A26:C26").Style.Font.FontName = "Tahoma";
                sht.Range("A26:C26").Style.Font.FontSize = 11;
                sht.Range("A26:C26").Merge();
                sht.Range("A26:C26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Place of Receipt by Pre-Carrier
                sht.Range("D25").Value = "Place of Receipt by Pre-Carrier";
                sht.Range("D25:F25").Style.Font.FontName = "Tahoma";
                sht.Range("D25:F25").Style.Font.FontSize = 11;
                sht.Range("D25:F25").Merge();
                //value
                sht.Range("D26").Value = ds.Tables[0].Rows[0]["receiptby"];
                sht.Range("D26:F26").Style.Font.FontName = "Tahoma";
                sht.Range("D26:F26").Style.Font.FontSize = 11;
                sht.Range("D26:F26").Merge();
                sht.Range("D26:F26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Vessel/Flight No
                sht.Range("A27").Value = "Vessel/Flight No";
                sht.Range("A27:C27").Style.Font.FontName = "Tahoma";
                sht.Range("A27:C27").Style.Font.FontSize = 11;
                sht.Range("A27:C27").Merge();
                //value
                sht.Range("A28").Value = ds.Tables[0].Rows[0]["vessel_flightno"];
                sht.Range("A28:C28").Style.Font.FontName = "Tahoma";
                sht.Range("A28:C28").Style.Font.FontSize = 11;
                sht.Range("A28:C28").Merge();
                sht.Range("A28:C28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Loading
                sht.Range("D27").Value = "Port of Loading";
                sht.Range("D27:F27").Style.Font.FontName = "Tahoma";
                sht.Range("D27:F27").Style.Font.FontSize = 11;
                sht.Range("D27:F27").Merge();
                //value
                sht.Range("D28").Value = ds.Tables[0].Rows[0]["portload"];
                sht.Range("D28:F28").Style.Font.FontName = "Tahoma";
                sht.Range("D28:F28").Style.Font.FontSize = 11;
                sht.Range("D28:F28").Merge();
                sht.Range("D28:F28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Discharge
                sht.Range("A29").Value = "Port of Discharge";
                sht.Range("A29:C29").Style.Font.FontName = "Tahoma";
                sht.Range("A29:C29").Style.Font.FontSize = 11;
                sht.Range("A29:C29").Merge();
                //value
                sht.Range("A30").Value = ds.Tables[0].Rows[0]["portunload"];
                sht.Range("A30:C30").Style.Font.FontName = "Tahoma";
                sht.Range("A30:C30").Style.Font.FontSize = 11;
                sht.Range("A30:C30").Merge();
                sht.Range("A30:C30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Final Destination
                sht.Range("D29").Value = "Final Destination";
                sht.Range("D29:F29").Style.Font.FontName = "Tahoma";
                sht.Range("D29:F29").Style.Font.FontSize = 11;
                sht.Range("D29:F29").Merge();
                //value
                sht.Range("D30").Value = ds.Tables[0].Rows[0]["Destinationadd"];
                sht.Range("D30:F30").Style.Font.FontName = "Tahoma";
                sht.Range("D30:F30").Style.Font.FontSize = 11;
                sht.Range("D30:F30").Merge();
                sht.Range("D30:F30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Origin of Goods
                sht.Range("G25").Value = "Country of Origin of Goods";
                sht.Range("G25:I25").Style.Font.FontName = "Tahoma";
                sht.Range("G25:I25").Style.Font.FontSize = 11;
                sht.Range("G25:I25").Merge();
                //value
                sht.Range("G26").Value = ds.Tables[0].Rows[0]["countryoforigin"];
                sht.Range("G26:I26").Style.Font.FontName = "Tahoma";
                sht.Range("G26:I26").Style.Font.FontSize = 11;
                sht.Range("G26:I26").Merge();
                sht.Range("G26:I26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Final Destination
                sht.Range("J25").Value = "Country of Final Destination";
                sht.Range("J25:K25").Style.Font.FontName = "Tahoma";
                sht.Range("J25:K25").Style.Font.FontSize = 11;
                sht.Range("J25:K25").Merge();
                //value
                sht.Range("J26").Value = ds.Tables[0].Rows[0]["countryoffinaldest"];
                sht.Range("J26:K26").Style.Font.FontName = "Tahoma";
                sht.Range("J26:K26").Style.Font.FontSize = 11;
                sht.Range("J26:K26").Style.NumberFormat.Format = "@";
                sht.Range("J26:K26").Merge();
                sht.Range("J26:K26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //***********Terms of Delivery and Payment
                sht.Range("G27").Value = "Terms of Delivery and Payment";
                sht.Range("G27:K27").Style.Font.FontName = "Tahoma";
                sht.Range("G27:K27").Style.Font.FontSize = 11;
                sht.Range("G27:K27").Merge();
                //value
                sht.Range("H28").Value = ds.Tables[0].Rows[0]["Deliveryterm"];
                sht.Range("H28:K29").Style.Font.FontName = "Tahoma";
                sht.Range("H28:K29").Style.Font.FontSize = 11;
                sht.Range("H28:K29").Style.NumberFormat.Format = "@";
                sht.Range("H28:K29").Style.Alignment.WrapText = true;
                sht.Range("H28:K29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("H28:K29").Merge();
                //*****************Nos
                string descriptionitem = string.Empty;
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    //descriptionitem += Convert.ToString(ds.Tables[0].Rows[j]["category"]) + "" + Convert.ToString(ds.Tables[0].Rows[j]["quality"]) + Convert.ToString(ds.Tables[0].Rows[j]["design"]) + "" + Convert.ToString(ds.Tables[0].Rows[j]["contents1"]) + Convert.ToString(ds.Tables[0].Rows[j]["articleno"])+"/n";
                    descriptionitem += Convert.ToString(ds.Tables[0].Rows[j]["description"]) + "-" + Convert.ToString(ds.Tables[0].Rows[j]["articleno"]) + ",";



                    j = j + 1;
                }
                sht.Range("A31").Value = "Marks Nos./ Container No";
                sht.Range("A31:B31").Merge();
                sht.Range("C31").Value = "No. of Packages";
                sht.Range("C31:D31").Merge();
                sht.Range("E31").Value = "Description of Goods";
                sht.Range("E31:K31").Merge();
                //sht.Range("I31").Value = "Quantity";
                //sht.Range("J31").Value = "Rate";
                //sht.Range("k31").Value = "Amount";
                sht.Range("A31:K31").Style.Font.FontName = "Tahoma";
                sht.Range("A31:K31").Style.Font.FontSize = 11;
                sht.Range("A31:K31").Style.NumberFormat.Format = "@";
                sht.Range("K31").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //values
                sht.Range("A32:H32").Merge();
                sht.Range("A33:H37").Style.Font.FontName = "Tahoma";
                sht.Range("A33:H37").Style.Font.FontSize = 11;
                sht.Range("A33:H37").Style.NumberFormat.Format = "@";

                sht.Range("A33").SetValue(ds.Tables[0].Compute("Min(MinrollNo)", "") + "/" + ds.Tables[0].Compute("Max(Maxrollno)", ""));
                sht.Range("A33:B33").Merge();
                sht.Range("C33").Value = ds.Tables[0].Rows[0]["Noofrolls"] + " Packages";
                sht.Range("C33:D33").Merge();
                sht.Range("E33").Value = descriptionitem;
                sht.Range("E33:K37").Style.Alignment.WrapText = true;
                sht.Range("E33:K37").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("E33:K37").Merge();
                sht.Range("E38:K38").Merge();
                //********Details
                sht.Range("A39").Value = "P.O.NO.";
                sht.Range("A39:K39").Style.Font.FontName = "Tahoma";
                sht.Range("A39:K39").Style.Font.FontSize = 11;
                sht.Range("A39:K39").Style.Font.Bold = true;
                sht.Range("A39:K39").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range("B39").Value = "ART.NO.";
                sht.Range("B39:C39").Merge();
                sht.Range("D39").Value = "DESCRIPTION, COLOUR & SIZE";
                sht.Range("D39:G39").Merge();
                //sht.Range("F39").Value = "COLOR";
                //sht.Range("G39").Value = "SIZE(CM)";
                sht.Range("H39").Value = "HSN";
                sht.Range("I39").Value = "QTY";
                sht.Range("J39").Value = "RATE FCA/" + ds.Tables[0].Rows[0]["CIF"];
                sht.Range("K39").Value = "AMOUNT FCA/" + ds.Tables[0].Rows[0]["CIF"];
                sht.Range("K39").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //***********generate Loop
                i = 40;
                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                {
                    sht.Range("A" + i + ":K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("A" + i + ":K" + i).Style.Font.FontSize = 11;
                    sht.Range("A" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //pono
                    sht.Range("A" + i).Value = ds.Tables[0].Rows[ii]["Pono"];
                    //Article Name
                    sht.Range("B" + i).Value = ds.Tables[0].Rows[ii]["articleno"];
                    sht.Range("B" + i, "C" + i).Merge();
                    //Art No.
                    sht.Range("D" + i).Value = Convert.ToString(ds.Tables[0].Rows[ii]["Design"]) + "," + Convert.ToString(ds.Tables[0].Rows[ii]["Color"]) + Convert.ToString(ds.Tables[0].Rows[ii]["width"]) + "x" + Convert.ToString(ds.Tables[0].Rows[ii]["Length"]);
                    sht.Range("D" + i, "G" + i).Merge();
                    //Colour
                    //sht.Range("F" + i).Value = ds.Tables[0].Rows[ii]["Color"];
                    ////Size
                    //sht.Range("G" + i).Value = ds.Tables[0].Rows[ii]["width"] + "x" + ds.Tables[0].Rows[ii]["Length"];
                    //Area
                    sht.Range("H" + i).SetValue(ds.Tables[0].Rows[ii]["hsncode"]);
                    Area = Decimal.Parse(Area.ToString()) + Decimal.Parse(ds.Tables[0].Rows[ii]["Area"].ToString());
                    //QTY
                    sht.Range("I" + i).SetValue(ds.Tables[0].Rows[ii]["Pcs"]);
                    Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[ii]["Pcs"]);
                    //
                    var _with27 = sht.Range("A" + i + ":K" + i);
                    _with27.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    _with27.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //rate
                    sht.Range("J" + i).SetValue(ds.Tables[0].Rows[ii]["Price"]);

                    //var _with28 = sht.Range("J" + i);
                    //_with28.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //_with28.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //Amount
                    sht.Range("K" + i).SetValue(ds.Tables[0].Rows[ii]["amount"]);
                    Amount = Amount + Convert.ToDecimal(ds.Tables[0].Rows[ii]["amount"]);
                    netwt = netwt + Convert.ToDecimal(ds.Tables[0].Rows[ii]["netwt"]);
                    ECIS = Convert.ToString(ds.Tables[0].Rows[0]["ecisno"]);
                    artno = Convert.ToString(ds.Tables[0].Rows[ii]["articleno"]);
                    //
                    sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    // sht.Range("H" + i + ":K" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("I" + i).Style.NumberFormat.Format = "@";
                    sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("H" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("K" + i).Style.NumberFormat.Format = "#,###0.00";
                    //border
                    sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    i = i + 1;
                }
                i = i + 5;
                sht.Range("A" + (i - 3) + ":A" + (i - 3)).SetValue("Supplier#17933");
                sht.Range("A" + (i - 2) + ":A" + (i - 2)).SetValue("ECIS#" + ECIS);
                sht.Range("A" + (i - 5) + ":A" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + (i - 5) + ":A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("C" + (i - 5) + ":C" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("G" + (i - 5) + ":G" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _withextra = sht.Range("H" + (i - 5) + ":K" + i);
                _withextra.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _withextra.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + (i) + ":K" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
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
                sht.Range("D" + (i + 1)).Value = "Total Sqmtr.:" + netwt;
                sht.Range("D" + (i + 1) + ":E" + (i + 1)).Merge();
                sht.Range("F" + (i + 1)).Value = "Total Wt.:" + netwt;
                sht.Range("F" + (i + 1) + ":G" + (i + 1)).Merge();
                sht.Range("E" + (i + 2)).Value = "For Art. No.:" + artno;
                sht.Range("E" + (i + 2) + ":F" + (i + 2)).Merge();
                i = i + 2;

                sht.Range("G" + i + ":K" + i).Style.Font.FontName = "Tahoma";
                sht.Range("G" + i + ":K" + i).Style.Font.FontSize = 11;
                //sht.Range("G" + i + ":I" + i).Style.NumberFormat.Format = "@";
                sht.Range("G" + i).Style.NumberFormat.Format = "@";
                sht.Range("I" + i).Style.NumberFormat.Format = "@";
                sht.Range("G" + i + ":K" + i).Style.Font.Bold = true;
                sht.Range("G" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range("H" + i).Value = "Total:";
                // sht.Range("H" + i).SetValue(Area);
                sht.Range("I" + i).SetValue(Pcs);
                sht.Range("K" + i).SetValue(Amount);
                //  sht.Range("A" + i + ":F" + i).Merge();
                sht.Range("H" + i).Style.NumberFormat.Format = "#,###0.00";
                //border
                var _withT = sht.Range("I" + i + ":K" + i);
                _withT.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                var _with23 = sht.Range("A" + i + ":K" + i);
                // _with23.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                _with23.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                i = i + 1;
                //
                //var _with24 = sht.Range("I" + i);
                //_with24.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //_with24.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //_with24.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with24.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //Notify Party

                //sht.Range("A" + i).Value = "Notify Party:";
                //sht.Range("A" + i).Style.Font.FontName = "Tahoma";
                //sht.Range("A" + i).Style.Font.FontSize = 11;
                //sht.Range("A" + i).Style.NumberFormat.Format = "@";
                //sht.Range("A" + i).Style.Font.SetUnderline();
                ////
                //sht.Range("B" + i).Value = ds.Tables[0].Rows[0]["Notifyparty2_dt"];
                //sht.Range("B" + i + ":C" + i).Style.Font.FontName = "Tahoma";
                //sht.Range("B" + i + ":C" + i).Style.Font.FontSize = 11;
                //sht.Range("B" + i + ":C" + i).Style.NumberFormat.Format = "@";
                //sht.Range("B" + i + ":C" + i).Style.Font.Bold = true;
                //sht.Range("B" + i + ":C" + i).Merge();
                //var _with31 = sht.Range("I" + i + ":I" + (i + 5));
                //_with31.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with31.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //sht.Range("A" + i + ":A" + (i + 5)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("K" + i + ":K" + (i + 5)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ////
                //var _with32 = sht.Range("J" + i + ":J" + (i + 5));
                //_with32.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with32.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ////
                //i = i + 1;
                //sht.Range("B" + i).Value = ds.Tables[0].Rows[0]["Notifyparty2_address"];
                //sht.Range("B" + i + ":E" + (i + 3)).Style.Font.FontName = "Tahoma";
                //sht.Range("B" + i + ":E" + (i + 3)).Style.Font.FontSize = 11;
                //sht.Range("B" + i + ":E" + (i + 3)).Style.NumberFormat.Format = "@";
                //sht.Range("B" + i + ":E" + (i + 3)).Style.Alignment.WrapText = true;
                //sht.Range("B" + i + ":E" + (i + 3)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                //sht.Range("B" + i + ":E" + (i + 3)).Merge();
                ////border
                //var _with25 = sht.Range("A" + (i - 1) + ":E" + (i - 1));
                //_with25.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //sht.Range("E" + (i - 1) + ":E" + (i + 3)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //sht.Range("A" + (i + 3) + ":E" + (i + 3)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ////***************
                //i = i + 5;
                //sht.Range("A" + i + ":G" + i).Merge();
                //sht.Range("A" + i).SetValue("The exporter Eastern Home Industries of the product covered by this document Declares that, ");
                //sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //using (var a = sht.Range("I" + i))
                //{
                //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //}
                //using (var a = sht.Range("J" + i))
                //{
                //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //}
                //i = i + 1;
                //sht.Range("A" + i + ":G" + i).Merge();
                //sht.Range("A" + i).SetValue("except where otherwise clearly indicated, these products are of …(Indian)…. Preferential origin");
                //sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //using (var a = sht.Range("I" + i))
                //{
                //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //}
                //using (var a = sht.Range("J" + i))
                //{
                //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //}
                //i = i + 1;
                //sht.Range("A" + i + ":G" + i).Merge();
                //sht.Range("A" + i).SetValue("according to the rules of origin of the Generalised System of the Eouropeon Union and ");
                //sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //using (var a = sht.Range("I" + i))
                //{
                //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //}
                //using (var a = sht.Range("J" + i))
                //{
                //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //}
                //i = i + 1;
                //sht.Range("A" + i + ":G" + i).Merge();
                //sht.Range("A" + i).SetValue("that the origin criterion met is …\"P\"");
                //sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //using (var a = sht.Range("I" + i))
                //{
                //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //}
                //using (var a = sht.Range("J" + i))
                //{
                //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //}
                ////**********Article Loop          
                ////
                //i = i + 2;

                ////i = i + 5;
                //sht.Range("A" + i).Value = "ARTICLE NAME";
                //sht.Range("A" + i + ":B" + i).Merge();
                //sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Tahoma";
                //sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 11;
                //sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";
                //sht.Range("A" + i + ":B" + i).Style.Font.Bold = true;
                ////
                //sht.Range("D" + i).Value = "CONTENT";
                //sht.Range("D" + i + ":F" + i).Merge();
                //sht.Range("D" + i + ":F" + i).Style.Font.FontName = "Tahoma";
                //sht.Range("D" + i + ":F" + i).Style.Font.FontSize = 11;
                //sht.Range("D" + i + ":F" + i).Style.NumberFormat.Format = "@";
                //sht.Range("D" + i + ":F" + i).Style.Font.Bold = true;
                ////
                //sht.Range("G" + i).Value = "RATE/SQM";
                //sht.Range("G" + i).Style.Font.FontName = "Tahoma";
                //sht.Range("G" + i).Style.Font.FontSize = 11;
                //sht.Range("G" + i).Style.NumberFormat.Format = "@";
                //sht.Range("G" + i).Style.Font.Bold = true;
                ////Border
                //var _with33 = sht.Range("I" + (i - 1) + ":I" + i);
                //_with33.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with33.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //sht.Range("A" + (i - 1) + ":A" + (i)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("K" + (i - 1) + ":K" + (i)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ////
                //var _with34 = sht.Range("J" + (i - 1) + ":J" + i);
                //_with34.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with34.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                ////********
                //i = i + 1;
                //sht.Range("A" + i + ":A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("K" + i + ":K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //str = "Select Design as ArticleNo,contents,Round(sum(amount)/sum(Area),2) as Rate From V_PackingDetailsIkea Where PackingId=" + DDinvoiceNo.SelectedValue + " group by Design,contents";
                //DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

                //if (ds1.Tables[0].Rows.Count > 0)
                //{
                //    for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                //    {
                //        sht.Range("A" + i).SetValue(ds1.Tables[0].Rows[j]["articleno"]);
                //        sht.Range("A" + i + ":B" + i).Merge();
                //        sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Tahoma";
                //        sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 11;
                //        sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";
                //        //
                //        sht.Range("D" + i).SetValue(ds1.Tables[0].Rows[j]["contents"]);
                //        sht.Range("D" + i + ":F" + i).Merge();
                //        sht.Range("D" + i + ":F" + i).Style.Font.FontName = "Tahoma";
                //        sht.Range("D" + i + ":F" + i).Style.Font.FontSize = 11;
                //        sht.Range("D" + i + ":F" + i).Style.NumberFormat.Format = "@";
                //        //rate
                //        sht.Range("G" + i).SetValue(ds1.Tables[0].Rows[j]["rate"]);
                //        sht.Range("G" + i).Style.Font.FontName = "Tahoma";
                //        sht.Range("G" + i).Style.Font.FontSize = 11;
                //        sht.Range("G" + i).Style.NumberFormat.Format = "@";
                //        sht.Range("G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //        //
                //        var _with35 = sht.Range("I" + (i) + ":I" + i);
                //        _with35.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //        _with35.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //        //
                //        var _with36 = sht.Range("J" + (i) + ":J" + i);
                //        _with36.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //        _with36.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //        sht.Range("A" + i + ":A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //        sht.Range("K" + i + ":K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //        i = i + 1;
                //    }

                //}
                //*******amount in words
                sht.Range("A" + i).Value = "Amount Chargeable in Words";
                sht.Range("A" + i + ":B" + i).Merge();
                sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 11;
                sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";

                string amountinwords = "";
                string amt = Amount.ToString();
                string val = "", paise = "";

                if (amt.IndexOf('.') > 0)
                {
                    val = amt.ToString().Split('.')[0];
                    amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val));
                }
                else
                {
                    amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(Amount));
                }

                string Pointamt = string.Format("{0:0.00}", Amount.ToString("0.00"));
                val = "";
                if (Pointamt.IndexOf('.') > 0)
                {
                    val = Pointamt.ToString().Split('.')[1];
                    if (Convert.ToInt32(val) > 0)
                    {
                        paise = ds.Tables[0].Rows[0]["Currencytypeps"] + " " + ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val)) + " ";
                    }
                }
                if (!string.IsNullOrEmpty(amountinwords))
                {
                    amountinwords = "TOTAL FCA VALUE IN INR:" + ds.Tables[0].Rows[0]["CIF"] + " " + amountinwords + " " + paise + "Only";
                }
                else
                { amountinwords = "TOTAL FCA VALUE IN INR:"; }
                sht.Range("C" + i).Value = amountinwords.ToUpper();
                sht.Range("C" + i + ":I" + (i + 1)).Style.Font.FontName = "Tahoma";
                sht.Range("C" + i + ":I" + (i + 1)).Style.Font.FontSize = 11;
                sht.Range("C" + i + ":I" + (i + 1)).Style.NumberFormat.Format = "@";
                sht.Range("C" + i + ":I" + (i + 1)).Style.Font.Bold = true;
                sht.Range("C" + i + ":I" + (i + 1)).Style.Alignment.WrapText = true;
                sht.Range("C" + i + ":I" + (i + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("C" + i + ":I" + (i + 1)).Merge();
                //*****Total
                //sht.Range("J" + i).Value = "Total";
                //sht.Range("J" + i).Style.Font.FontName = "Tahoma";
                //sht.Range("J" + i).Style.Font.FontSize = 11;
                //sht.Range("J" + i).Style.NumberFormat.Format = "@";
                //sht.Range("J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ////Amount
                //sht.Range("K" + i).SetValue(Amount);
                //sht.Range("K" + i).Style.Font.FontName = "Tahoma";
                //sht.Range("K" + i).Style.Font.FontSize = 11;
                //sht.Range("K" + i).Style.NumberFormat.Format = "@";
                //sht.Range("K" + i).Style.Font.Bold = true;
                //sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //sht.Range("K" + i).Style.NumberFormat.Format = "#,###0.00";
                //
                sht.Range("A" + i + ":A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //var _with37 = sht.Range("K" + i);
                //_with37.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with37.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //_with37.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //_with37.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                //border
                var _with26 = sht.Range("A" + i + ":K" + i);
                _with26.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //value
                //**********Total
                i = i + 2;
                //i = 63;
                sht.Range("A" + i).Value = "TOTAL PKGS :";
                sht.Range("A" + (i + 1)).Value = "TOTAL QUANTITY :";
                sht.Range("A" + (i + 2)).Value = "TOTAL GR. WT  :";
                sht.Range("A" + (i + 3)).Value = "TOTAL NT. WT  :";
                sht.Range("A" + (i + 4)).Value = "TOTAL SQ.MTR  :";
                sht.Range("A" + (i + 5)).Value = "VOLUME  :";
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontSize = 11;
                sht.Range("A" + i + ":A" + (i + 5)).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontName = "Courier New";
                //value            
                sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontName = "Tahoma";
                sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontSize = 11;
                sht.Range("C" + i + ":C" + (i + 5)).Style.NumberFormat.Format = "@";
                sht.Range("C" + i).SetValue(ds.Tables[0].Rows[0]["Noofrolls"].ToString() + " Pallets (Total " + Pcs + " pcs packed in " + ds.Tables[0].Rows[0]["Noofrolls"].ToString() + " Pallets)");
                sht.Range("C" + i + ":K" + i).Merge();
                sht.Range("C" + (i + 1)).SetValue(Pcs);
                sht.Range("C" + (i + 2)).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["grosswt"]) + " Kgs.";
                sht.Range("C" + (i + 3)).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["netwt"]) + " Kgs.";
                sht.Range("C" + (i + 4)).Value = String.Format("{0:#,0.00}", Area) + " Sq.Mtr";
                sht.Range("C" + (i + 5)).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["Volume"]) + " CBM";
                //******Central Excise
                //sht.Range("F" + i).Value = "Central Excise No. AADFE8796LEM001";
                //sht.Range("F" + (i + 1)).Value = "Range: Bhadohi";
                //sht.Range("F" + (i + 2)).Value = "Division: Allahabad-I";
                //sht.Range("F" + (i + 3)).Value = "Commissionerate: Allahabad";
                //sht.Range("F" + (i + 4)).Value = "Tariff Heading# 570201";
                //sht.Range("F" + i + ":H" + (i + 4)).Style.Font.FontName = "Tahoma";
                //sht.Range("F" + i + ":H" + (i + 4)).Style.Font.FontSize = 11;
                //sht.Range("F" + i + ":H" + (i + 4)).Style.NumberFormat.Format = "@";
                //sht.Range("F" + i + ":H" + (i + 4)).Style.Font.Bold = true;
                //sht.Range("F" + i + ":H" + i).Merge();
                //sht.Range("F" + (i + 1) + ":H" + (i + 1)).Merge();
                //sht.Range("F" + (i + 2) + ":H" + (i + 2)).Merge();
                //sht.Range("F" + (i + 3) + ":H" + (i + 3)).Merge();
                //sht.Range("F" + (i + 4) + ":H" + (i + 4)).Merge();
                //sht.Range("F" + i + ":K" + (i + 2)).Merge();
                //sht.Range("F" + i).SetValue("HSN CODE# " + ds.Tables[0].Rows[0]["hsncode"]);
                //sht.Range("F" + i + ":K" + (i + 2)).Style.Font.FontName = "Tahoma";
                //sht.Range("F" + i + ":K" + (i + 2)).Style.Font.FontSize = 11;
                //sht.Range("F" + i + ":K" + (i + 2)).Style.NumberFormat.Format = "@";
                //sht.Range("F" + i + ":K" + (i + 2)).Style.Font.Bold = true;
                //sht.Range("F" + i + ":K" + (i + 2)).Style.Alignment.WrapText = true;
                //sht.Range("F" + i + ":K" + (i + 2)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                sht.Range("A" + i + ":A" + (i + 7)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 7)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //*******************************GSTAmount

                //********Details
                i = i + 7;
                sht.Range("A" + i + ":K" + i).Merge();
                sht.Range("A" + i).Value = "Supply Meant for Export With Payment of Integrated Tax(" + ds.Tables[0].Rows[0]["GSTTypeName"].ToString() + ")";
                sht.Range("A" + i + ":K" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":K" + i).Style.Font.FontSize = 11;
                sht.Range("A" + i + ":K" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A" + i + ":K" + (i)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                i = i + 1;
                sht.Range("A" + i).Value = "Description";
                sht.Range("A" + i + ":D" + i).Merge();
                sht.Range("A" + i + ":K" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":K" + i).Style.Font.FontSize = 11;
                sht.Range("A" + i + ":K" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //sht.Range("B" + i).Value = "HSN";
                //sht.Range("C" + i).Value = "Qty PCS";
                //sht.Range("D" + i).Value = "Rate" + Environment.NewLine + "FCA/" + ds.Tables[0].Rows[0]["CIF"];
                sht.Range("E" + i).Value = "HSN";
                sht.Range("F" + i).Value = "Qty PCS";
                sht.Range("G" + i).Value = "Rate" + Environment.NewLine + "FCA/" + ds.Tables[0].Rows[0]["CIF"];
                sht.Range("H" + i).Value = "Amount";
                sht.Range("I" + i).Value = "GST Amount";
                sht.Range("J" + i).Value = "Total Amount" + Environment.NewLine + " Including GST";
                sht.Row(i).Height = 30;
                sht.Range("I" + i + ":J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                var _withINNER = sht.Range("A" + i + ":K" + i);
                _withINNER.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                _withINNER.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _withINNER.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _withINNER.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //***********generate Loop
                i = i + 1;
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    sht.Range("A" + i + ":K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("A" + i + ":K" + i).Style.Font.FontSize = 11;
                    sht.Range("A" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //pono
                    sht.Range("A" + i).Value = ds.Tables[0].Rows[j]["Description"].ToString();
                    sht.Range("A" + i + ":D" + i).Merge();
                    //Article Name
                    sht.Range("E" + i).Value = ds.Tables[0].Rows[j]["Hsncode"];
                    //sht.Range("B" + i, "C" + i).Merge();
                    //Art No.
                    sht.Range("F" + i).Value = ds.Tables[0].Rows[j]["Pcs"];
                    TotalPcs = TotalPcs + Convert.ToInt16(ds.Tables[0].Rows[j]["Pcs"]);
                    //sht.Range("D" + i, "E" + i).Merge();
                    sht.Range("G" + i).Value = ds.Tables[0].Rows[j]["Price"];
                    sht.Range("H" + i).Value = ds.Tables[0].Rows[j]["amount"];
                    TotalAmount = TotalAmount + Convert.ToDecimal(ds.Tables[0].Rows[j]["amount"]);
                    //sht.Range("F" + i).Value = ds.Tables[0].Rows[j]["CGST"];
                    //sht.Range("G" + i).Value = ds.Tables[0].Rows[j]["SGST"];
                    //sht.Range("H" + i).Value = ds.Tables[0].Rows[j]["IGST1"];

                    if (ds.Tables[0].Rows[j]["GSTType"].ToString() == "1")
                    {
                        GSTAmount = (Convert.ToDecimal(ds.Tables[0].Rows[j]["amount"]) / (Convert.ToDecimal(ds.Tables[0].Rows[j]["CGST"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["SGST"])) * 100);
                        TotalGSTAmount = TotalGSTAmount + GSTAmount;
                    }
                    else if (ds.Tables[0].Rows[j]["GSTType"].ToString() == "2")
                    {
                        GSTAmount = (Convert.ToDecimal(ds.Tables[0].Rows[j]["amount"]) / (Convert.ToDecimal(ds.Tables[0].Rows[j]["IGST1"])) * 100);
                        TotalGSTAmount = TotalGSTAmount + GSTAmount;
                    }
                    else
                    {
                        GSTAmount = 0;
                    }

                    sht.Range("I" + i).Value = GSTAmount;
                    sht.Range("J" + i).Value = Convert.ToDecimal(ds.Tables[0].Rows[j]["amount"]) + GSTAmount;
                    TotalAmountWithGST = TotalAmountWithGST + Convert.ToDecimal(ds.Tables[0].Rows[j]["amount"]) + GSTAmount;
                    var _withHEAD = sht.Range("A" + i + ":K" + i);
                    _withHEAD.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    _withHEAD.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    _withHEAD.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    _withHEAD.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //var _with27 = sht.Range("I" + i);
                    //_with27.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //_with27.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ////rate
                    //sht.Range("J" + i).SetValue(ds.Tables[0].Rows[ii]["Price"]);

                    //var _with28 = sht.Range("J" + i);
                    //_with28.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //_with28.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ////Amount
                    //sht.Range("K" + i).SetValue(ds.Tables[0].Rows[ii]["amount"]);
                    //Amount = Amount + Convert.ToDecimal(ds.Tables[0].Rows[ii]["amount"]);
                    //
                    sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    // sht.Range("H" + i + ":K" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("C" + i).Style.NumberFormat.Format = "@";
                    sht.Range("E" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("I" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                    //border
                    sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    i = i + 1;
                }
                sht.Range("A" + i + ":A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                i = i + 1;
                sht.Range("A" + i + ":A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("E" + i).Value = "Total:";
                sht.Range("F" + i).SetValue(TotalPcs);
                sht.Range("H" + i).SetValue(TotalAmount);
                sht.Range("I" + i).SetValue(TotalGSTAmount);
                sht.Range("J" + i).SetValue(TotalAmountWithGST);

                sht.Range("B" + i + ":J" + i).Style.Font.FontName = "Tahoma";
                sht.Range("B" + i + ":J" + i).Style.Font.FontSize = 11;
                sht.Range("B" + i + ":J" + i).Style.NumberFormat.Format = "@";
                sht.Range("B" + i + ":J" + i).Style.Font.Bold = true;

                //sht.Range("A" + i + ":F" + i).Merge();
                sht.Range("E" + i).Style.NumberFormat.Format = "#,###0.00";
                sht.Range("I" + i).Style.NumberFormat.Format = "#,###0.00";
                sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                ////border
                //var _with244 = sht.Range("C" + i);
                //_with244.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //_with244.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with245 = sht.Range("E" + i + ":J" + i);
                _with245.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                _with245.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with245.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with245.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                i = i + 2;
                sht.Range("A" + i + ":G" + i).Merge();
                sht.Range("A" + i).Value = "STATEMENT ON ORIGIN";
                sht.Range("A" + i + ":G" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":G" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":G" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //var _with466 = sht.Range("A" + i + ":A" + i);
                //_with466.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with466.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i + ":A" + (i + 2)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 2)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                sht.Range("G" + i + ":G" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("G" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i + ":G" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i + ":G" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;

                i = i + 1;
                sht.Range("A" + i + ":G" + i).Merge();
                sht.Range("A" + i).Value = "The Exporter INREX0694000965EC026 of the products covered by this document declares that, except where otherwise clearly indicated, these products are of INDIA preferential origin according to rules of origin of the Generalized system of preferences of the European Union and that the origin criterion met is 'P'";
                sht.Range("A" + i + ":G" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":G" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":G" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Justify);
                sht.Row(i).Height = 50;
                sht.Range("A" + i + ":G" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //var _with467 = sht.Range("A" + i + ":K" + i);
                //_with467.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with467.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                sht.Range("A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                i = i + 1;
                sht.Range("A" + i + ":C" + i).Merge();
                sht.Range("A" + i).Value = "R.B.I Code No. 02422282600008";
                sht.Range("A" + i + ":C" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":C" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":C" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":C" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //var _with468 = sht.Range("A" + i + ":K" + i);
                //_with468.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with468.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                sht.Range("E" + i + ":G" + i).Merge();
                sht.Range("E" + i).Value = "Factory Address:";
                sht.Range("E" + i + ":G" + i).Style.Font.FontName = "Tahoma";
                sht.Range("E" + i + ":G" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("E" + i + ":G" + i).Style.Font.Bold = true;
                sht.Range("E" + i + ":G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //var _with469 = sht.Range("A" + i + ":K" + i);
                //_with469.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with469.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                i = i + 1;
                sht.Range("A" + i + ":C" + i).Merge();
                sht.Range("A" + i).Value = "I.E.  Code  No.: 0694000965";
                sht.Range("A" + i + ":C" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":C" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":C" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":C" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("E" + i + ":G" + i).Merge();
                sht.Range("E" + i).Value = " AGRA-MATHURA ROAD, ARTONI" + Environment.NewLine + "AGRA-282 007 (INDIA)";
                sht.Range("E" + i + ":G" + i).Style.Font.FontName = "Tahoma";
                sht.Range("E" + i + ":G" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("E" + i + ":G" + i).Style.Font.Bold = true;
                sht.Range("E" + i + ":G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Row(i).Height = 30;
                sht.Range("A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //var _with560 = sht.Range("A" + i + ":K" + i);
                //_with560.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with560.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //***********Sig and date
                i = i + 2;
                //i = 70;
                sht.Range("A" + i + ":H" + i).Merge();
                sht.Range("I" + i).Value = "Signature/Date";
                sht.Range("I" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i).Style.Font.FontSize = 11;
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
                sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 11;
                sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":B" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //value
                i = i + 1;
                sht.Range("I" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i).Value = "We declare that this invoice shows the actual price of the";
                sht.Range("A" + i + ":E" + i).Merge();
                sht.Range("A" + i + ":E" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":E" + i).Style.Font.FontSize = 11;
                sht.Range("A" + i + ":E" + i).Style.NumberFormat.Format = "@";
                i = i + 1;
                sht.Range("A" + i).Value = "goods described and that all particulars are true and correct";
                sht.Range("A" + i + ":E" + i).Merge();
                sht.Range("A" + i + ":E" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":E" + i).Style.Font.FontSize = 11;
                sht.Range("A" + i + ":E" + i).Style.NumberFormat.Format = "@";

                //**********
                sht.Range("I" + i + ":J" + i).Merge();
                sht.Range("I" + i + ":J" + i).Value = ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("I" + i + ":J" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i + ":J" + i).Style.Font.FontSize = 11;
                sht.Range("I" + i + ":J" + i).Style.NumberFormat.Format = "dd/MMM/yyyy";
                sht.Range("I" + i + ":J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("K" + i).Value = "Auth Sign";
                sht.Range("K" + i).Style.Font.FontName = "Tahoma";
                sht.Range("K" + i).Style.Font.FontSize = 11;
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
                var _with2 = sht.Range("A11:K11");
                _with2.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with3 = sht.Range("G3:G30");
                //  _with3.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with3.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                var _with4 = sht.Range("K3:K30");
                //  _with3.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with4.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _with41 = sht.Range("G3:K3");
                _with41.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                var _with42 = sht.Range("G8:K8");
                _with42.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with42.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                // var _with40 = sht.Range("K4:K4");
                // _with40.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                // //
                // var _with4 = sht.Range("G5:I6");
                // //_with4.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                // _with4.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                // var _with42 = sht.Range("G5:I5");
                // _with42.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                // var _with43 = sht.Range("G6:I6");
                // _with43.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                // //
                // var _with5 = sht.Range("J5:K5");
                // _with5.Style.Border.TopBorder =     XLBorderStyleValues.Thin;
                // var _with44 = sht.Range("J5:K6");
                // _with44.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                // var _with45 = sht.Range("J6:K6");
                // _with45.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                // //
                // var _with6 = sht.Range("G7:K8");
                // _with6.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                // _with6.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                // var _with46 = sht.Range("G8:K8");
                // _with46.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                //var _with7 = sht.Range("G9:K11");
                //_with7.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //_with7.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //var _with47 = sht.Range("G12:K25");
                //_with47.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //sht.Range("G12:I12").Style.Border.RightBorder = XLBorderStyleValues.None;

                //
                var _with8 = sht.Range("A24:F24");
                _with8.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                var _with48 = sht.Range("A12:F24");
                _with48.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                sht.Range("A12:B12").Style.Border.RightBorder = XLBorderStyleValues.None;
                sht.Range("A18:B18").Style.Border.RightBorder = XLBorderStyleValues.None;
                sht.Range("G12:H12").Style.Border.RightBorder = XLBorderStyleValues.None;
                sht.Range("G18:H20").Style.Border.RightBorder = XLBorderStyleValues.None;

                //
                var _with9 = sht.Range("A25:C26");
                _with9.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with9.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //sht.Range("A24:C24").Style.Border.BottomBorder = XLBorderStyleValues.None;

                sht.Range("A26:C26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                sht.Range("G26:I26").Style.Border.BottomBorder = XLBorderStyleValues.None;

                sht.Range("J26:K26").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //
                var _with10 = sht.Range("D25:F26");
                _with10.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with10.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D25:F25").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //
                var _with11 = sht.Range("A27:C28");
                _with11.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with11.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with11.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A27:C27").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //
                var _with12 = sht.Range("D27:F28");
                _with12.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with12.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D27:F27").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //
                var _with13 = sht.Range("A29:C30");
                _with13.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with13.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A29:C29").Style.Border.BottomBorder = XLBorderStyleValues.None;
                sht.Range("A30:C30").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with14 = sht.Range("D29:F30");
                _with14.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with14.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D29:G29").Style.Border.BottomBorder = XLBorderStyleValues.None;
                sht.Range("G29").Style.Border.BottomBorder = XLBorderStyleValues.None;

                //
                //var _with15 = sht.Range("G25:K25");
                //_with15.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //_with15.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("G25:K25").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("G26:K26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                //var _with16 = sht.Range("J26:K27");
                //_with16.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //sht.Range("J26:K26").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //sht.Range("J27:K27").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with17 = sht.Range("G28:K30");
                _with17.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with17.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("G30:K30").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("G29:G30").Style.Border.RightBorder = XLBorderStyleValues.None;
                sht.Range("H29:K30").Style.Border.LeftBorder = XLBorderStyleValues.None;
                //
                var _with18 = sht.Range("A39:K39");
                _with18.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with18.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                //var _with19 = sht.Range("J31:J38");
                //_with19.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with19.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                var _with20 = sht.Range("A39:K39");
                _with20.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                _with20.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with21 = sht.Range("I39:I39");
                _with21.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with21.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                var _with22 = sht.Range("J39:J39");
                _with22.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with22.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //
                sht.Range("A31:A38").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K31:K38").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A39:A39").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K38:K38").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A2:K2").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A3:A24").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                string Fileextension = "xlsx";
                string filename = "Invoice-" + validateFilename(ds.Tables[0].Rows[0]["TinvoiceNo"].ToString()) + "." + Fileextension + "";
                Path = Server.MapPath("~/InvoiceExcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
                lblmsg.Text = "Invoice Excel Format downloaded successfully.";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "AltR1", "alert('No Record Found to Export')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }

    protected void Exporttoexcel_Kaysons_3()
    {
        lblmsg.Text = "Wait For Excel File to Download";
        if (!Directory.Exists(Server.MapPath("~/InvoiceExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/InvoiceExcel/"));
        }
        string Path = "";
        try
        {

            //******************
            string str = "";
            if (variable.VarGSTForInvoiceFormNew == "1")
            {
                str = @"select * From V_InvoiceDetail VI inner join V_PackingDetailsIkea_WithGST VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue;
            }
            else
            {
                str = @"select * From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue;
            }
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int i, Pcs = 0, TotalPcs = 0;
                Decimal Area = 0, Amount = 0, TotalAmount = 0, GSTAmount = 0, TotalGSTAmount = 0, TotalAmountWithGST = 0;

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("Invoice1");

                //************set cell width
                //Page
                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                sht.PageSetup.AdjustTo(63);
                sht.PageSetup.FitToPages(1, 1);
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                sht.PageSetup.VerticalDpi = 300;
                sht.PageSetup.HorizontalDpi = 300;

                //  sht.Row(1).Height = 15.75;
                //*****Header                
                sht.Cell("A1").Value = "INVOICE";
                sht.Range("A1:L1").Style.Font.FontName = "Tahoma";
                sht.Range("A1:L1").Style.Font.FontSize = 12;
                sht.Range("A1:L1").Style.Font.Bold = true;
                sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:L1").Merge();
                //***************
                // sht.Cell("A2").Value = @"""SUPPLY MEANT FOR EXPORT UNDER LETTER OF UNDERTAKING NO-21/2017 Dt.07.07.2017. WITHOUT PAYMENT OF INTEGRATED TAX""";
                sht.Range("A2:L2").Style.Font.FontName = "Tahoma";
                sht.Range("A2:L2").Style.Font.FontSize = 12;
                sht.Range("A2:L2").Style.Font.Bold = true;
                sht.Range("A2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:L2").Merge();
                using (var a = sht.Range("A2:L2"))
                {
                    //a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A2:L2"))
                {
                    //a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //********Exporter
                sht.Range("A3").Value = "Exporter";
                sht.Range("A3:C3").Style.Font.FontName = "Tahoma";
                sht.Range("A3:C3").Style.Font.FontSize = 10;
                //  sht.Range("A3:C3").Style.Alignment.WrapText = true;
                sht.Range("A3:C3").Merge();
                //*****************

                //CompanyName
                sht.Range("A4").Value = ds.Tables[0].Rows[0]["companyName"];
                sht.Range("A4:C4").Style.Font.FontName = "Tahoma";
                sht.Range("A4:C4").Style.Font.FontSize = 11;
                sht.Range("A4:C4").Style.Font.Bold = true;
                //  sht.Range("A3:C4").Style.Alignment.WrapText = true;
                sht.Range("A4:C4").Merge();
                //Address
                sht.Range("A5").Value = ds.Tables[0].Rows[0]["compaddr1"];
                sht.Range("A5:C5").Style.Font.FontName = "Tahoma";
                sht.Range("A5:C5").Style.Font.FontSize = 10;
                //  sht.Range("A3:C5").Style.Alignment.WrapText = true;
                sht.Range("A5:C5").Merge();
                //address2
                sht.Range("A6").Value = ds.Tables[0].Rows[0]["compaddr2"] + "," + ds.Tables[0].Rows[0]["compaddr3"];
                sht.Range("A6:C6").Style.Font.FontName = "Tahoma";
                sht.Range("A6:C6").Style.Font.FontSize = 10;
                //  sht.Range("A3:C6").Style.Alignment.WrapText = true;
                sht.Range("A6:C6").Merge();
                //TiN No
                sht.Range("A7").Value = "GSTIN: " + ds.Tables[0].Rows[0]["gstno"];
                sht.Range("A7:C7").Style.Font.FontName = "Taho  ma";
                sht.Range("A7:C7").Style.Font.FontSize = 10;
                sht.Range("A7:C7").Style.Font.Bold = true;
                sht.Range("A7:C7").Merge();

                sht.Range("A8").Value = "STATE : " + ds.Tables[0].Rows[0]["rec_state"];
                sht.Range("A8:C8").Style.Font.FontName = "Tahoma";
                sht.Range("A8:C8").Style.Font.FontSize = 10;
                sht.Range("A8:C8").Style.Font.Bold = true;
                sht.Range("A8:C8").Merge();

                sht.Range("A9").Value = "PAN NO. : " + ds.Tables[0].Rows[0]["PANNr"];
                sht.Range("A9:C9").Style.Font.FontName = "Tahoma";
                sht.Range("A9:C9").Style.Font.FontSize = 10;
                sht.Range("A9:C9").Style.Font.Bold = true;
                sht.Range("A9:C9").Merge();





                //**********INvoiceNodate
                sht.Range("F3").Value = "Invoice No. : " + ds.Tables[0].Rows[0]["TInvoiceNo"];
                sht.Range("F3:H3").Style.Font.FontName = "Tahoma";
                sht.Range("F3:H3").Style.Font.FontSize = 10;
                sht.Range("F3:H3").Merge();
                //value
                sht.Range("K3").Value = "Dated " + ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("K3:L3").Merge();
                sht.Range("F7").Value = "Buyer's Order No. / Date";
                sht.Range("F7:H8").Style.Font.FontName = "Tahoma";
                sht.Range("F7:H8").Style.Font.FontSize = 10;
                sht.Range("F7:H7").Merge();
                //value
                sht.Range("I8").SetValue(ds.Tables[0].Rows[0]["TorderNo"]);
                sht.Range("I8:K8").Style.Font.FontName = "Tahoma";
                sht.Range("I8:K8").Style.Font.FontSize = 10;
                sht.Range("I8:K8").Merge();
                sht.Range("I8", "K8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //**************Other Reference(s)
                //  sht.Range("G9").Value = "Other Reference(s)";
                sht.Range("F10:H10").Merge();
                sht.Range("F10").Value = "SUPPLIER NO.: 21899";
                sht.Range("F11").Value = "CONSIGNMENT NO.:" + ds.Tables[0].Rows[0]["otherref"];
                sht.Range("K10").Value = "DESTN. CODE";
                sht.Range("K10:L10").Merge();
                sht.Range("F10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("F10:H11").Style.Font.FontName = "Tahoma";
                sht.Range("F10:H11").Style.Font.FontSize = 10;
                // sht.Range("F10:H11").Merge();

                //Border for first section
                sht.Range("A2:A67").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("L2:L67").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("F3:L3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("F3:F11").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("k3").Style.Alignment.WrapText = true;
                sht.Range("k9").Style.Alignment.WrapText = true;
                sht.Range("A11:L11").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //sht.Range("F3:k3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                using (var a = sht.Range("F10:L10"))
                {
                    //a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A2:L2"))
                {
                    //a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                // sht.Range("D10:J10").Merge();
                // sht.Range("J10:K10").Merge();
                // sht.Range("G11:K11").Merge();
                //*************Consignee
                sht.Range("A12").Value = "Consignee";
                sht.Range("C12").Value = ds.Tables[0].Rows[0]["consignee_dt"];
                sht.Range("A12:B12").Style.Font.FontName = "Tahoma";
                sht.Range("A12:B12").Style.Font.FontSize = 10;

                sht.Range("C12:F12").Style.Font.FontName = "Tahoma";
                sht.Range("C12:F12").Style.Font.FontSize = 10;
                sht.Range("C12:F12").Style.Font.Bold = true;

                sht.Range("A12:B12").Merge();
                sht.Range("C12:F12").Merge();
                sht.Range("C12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //value
                sht.Range("A13").Value = ds.Tables[0].Rows[0]["consignee"];
                sht.Range("A13:F13").Style.Font.FontName = "Tahoma";
                sht.Range("A13:F13").Style.Font.FontSize = 10;
                sht.Range("A13:F13").Style.Font.Bold = true;
                sht.Range("A13:F13").Merge();
                //**
                sht.Range("A14").Value = ds.Tables[0].Rows[0]["consignee_address"];
                sht.Range("A14:F17").Style.Font.FontName = "Tahoma";
                sht.Range("A14:F17").Style.Font.FontSize = 10;
                sht.Range("A14", "F17").Style.Alignment.WrapText = true;
                sht.Range("A14", "F17").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A14:F17").Merge();
                //***********Notify
                sht.Range("A18").Value = "Notify Party";
                sht.Range("C18").Value = ds.Tables[0].Rows[0]["Notifyparty_dt"];
                sht.Range("A18:B18").Style.Font.FontName = "Tahoma";
                sht.Range("A18:B18").Style.Font.FontSize = 10;

                sht.Range("A18:B18").Style.Font.SetUnderline();
                sht.Range("C18:F18").Style.Font.FontName = "Tahoma";
                sht.Range("C18:F18").Style.Font.FontSize = 10;
                sht.Range("C18:F18").Style.Font.Bold = true;

                sht.Range("A18:B18").Merge();
                sht.Range("C18:F18").Merge();
                sht.Range("C18").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //value
                sht.Range("A19").Value = ds.Tables[0].Rows[0]["Notifyparty"];
                sht.Range("A19:F19").Style.Font.FontName = "Tahoma";
                sht.Range("A19:F19").Style.Font.FontSize = 10;
                sht.Range("A19:F19").Style.Font.Bold = true;
                sht.Range("A19:F19").Merge();
                //
                sht.Range("A20").Value = ds.Tables[0].Rows[0]["notifyparty_address"];
                sht.Range("A20:F24").Style.Font.FontName = "Tahoma";
                sht.Range("A20:F24").Style.Font.FontSize = 10;
                sht.Range("A20", "F24").Style.Alignment.WrapText = true;
                sht.Range("A20", "F24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A20:F24").Merge();

                sht.Range("H13").Value = "Buyer (If other than Consignee)";
                sht.Range("H13:L13").Style.Font.FontName = "Tahoma";
                sht.Range("H13:L13").Style.Font.FontSize = 10;
                sht.Range("H13:L13").Merge();

                sht.Range("H14").Value = "" + ds.Tables[0].Rows[0]["buyer_otherthanconsignee"];
                sht.Range("H14:L14").Style.Font.FontName = "Tahoma";
                sht.Range("H14:L14").Style.Font.FontSize = 10;
                sht.Range("H14:L14").Merge();
                //*
                sht.Range("H15").Value = ds.Tables[0].Rows[0]["otherthanconsignee_address"];
                sht.Range("H15:L17").Style.Font.FontName = "Tahoma";
                sht.Range("H15:L17").Style.Font.FontSize = 10;
                sht.Range("H15:L17").Style.Alignment.WrapText = true;
                sht.Range("H15:L17").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("H15:L17").Merge();

                sht.Range("H17").Value = "2.Paying Agent : " + ds.Tables[0].Rows[0]["payingagent"];
                sht.Range("H17:L17").Style.Font.FontName = "Tahoma";
                sht.Range("H17:L17").Style.Font.FontSize = 10;
                sht.Range("H17:L17").Style.Font.Bold = true;
                sht.Range("H17:L17").Merge();
                //*
                sht.Range("J18").Value = ds.Tables[0].Rows[0]["payingagent_address"];
                sht.Range("J18:L20").Style.Font.FontName = "Tahoma";
                sht.Range("J18:L20").Style.Font.FontSize = 10;
                sht.Range("J18:L20").Style.Alignment.WrapText = true;
                sht.Range("J18:L20").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("J18:L20").Merge();

                sht.Range("A25:L25").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("F12:F24").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //*******3.

                //***********Pre-carriage By
                sht.Range("A25").Value = "Pre-Carriage By";
                sht.Range("A25:C25").Style.Font.FontName = "Tahoma";
                sht.Range("A25:C25").Style.Font.FontSize = 10;
                sht.Range("A25:C25").Merge();
                //value
                sht.Range("A26").Value = ds.Tables[0].Rows[0]["Pre_carriageby"];
                sht.Range("A26:C26").Style.Font.FontName = "Tahoma";
                sht.Range("A26:C26").Style.Font.FontSize = 10;
                sht.Range("A26:C26").Merge();
                sht.Range("A26:C26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Place of Receipt by Pre-Carrier
                sht.Range("D25").Value = "Place of Receipt by Pre-Carrier";
                sht.Range("D25:F25").Style.Font.FontName = "Tahoma";
                sht.Range("D25:F25").Style.Font.FontSize = 10;
                sht.Range("D25:F25").Merge();
                //value
                sht.Range("D26").Value = ds.Tables[0].Rows[0]["receiptby"];
                sht.Range("D26:F26").Style.Font.FontName = "Tahoma";
                sht.Range("D26:F26").Style.Font.FontSize = 10;
                sht.Range("D26:F26").Merge();
                sht.Range("D26:F26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Vessel/Flight No
                sht.Range("A27").Value = "Vessel/Flight No";
                sht.Range("A27:C27").Style.Font.FontName = "Tahoma";
                sht.Range("A27:C27").Style.Font.FontSize = 10;
                sht.Range("A27:C27").Merge();
                //value
                sht.Range("A28").Value = ds.Tables[0].Rows[0]["vessel_flightno"];
                sht.Range("A28:C28").Style.Font.FontName = "Tahoma";
                sht.Range("A28:C28").Style.Font.FontSize = 10;
                sht.Range("A28:C28").Merge();
                sht.Range("A28:C28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Loading
                sht.Range("D27").Value = "Port of Loading";
                sht.Range("D27:F27").Style.Font.FontName = "Tahoma";
                sht.Range("D27:F27").Style.Font.FontSize = 10;
                sht.Range("D27:F27").Merge();
                //value
                sht.Range("D28").Value = ds.Tables[0].Rows[0]["portload"];
                sht.Range("D28:F28").Style.Font.FontName = "Tahoma";
                sht.Range("D28:F28").Style.Font.FontSize = 10;
                sht.Range("D28:F28").Merge();
                sht.Range("D28:F28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Discharge
                sht.Range("A29").Value = "Port of Discharge";
                sht.Range("A29:C29").Style.Font.FontName = "Tahoma";
                sht.Range("A29:C29").Style.Font.FontSize = 10;
                sht.Range("A29:C29").Merge();
                //value
                sht.Range("A30").Value = ds.Tables[0].Rows[0]["portunload"];
                sht.Range("A30:C30").Style.Font.FontName = "Tahoma";
                sht.Range("A30:C30").Style.Font.FontSize = 10;
                sht.Range("A30:C30").Merge();
                sht.Range("A30:C30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Final Destination
                sht.Range("D29").Value = "Final Destination";
                sht.Range("D29:F29").Style.Font.FontName = "Tahoma";
                sht.Range("D29:F29").Style.Font.FontSize = 10;
                sht.Range("D29:F29").Merge();
                //value
                sht.Range("D30").Value = ds.Tables[0].Rows[0]["Destinationadd"];
                sht.Range("D30:F30").Style.Font.FontName = "Tahoma";
                sht.Range("D30:F30").Style.Font.FontSize = 10;
                sht.Range("D30:F30").Merge();
                sht.Range("D30:F30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Origin of Goods
                sht.Range("G26").Value = "Country of Origin of Goods";
                sht.Range("G26:I26").Style.Font.FontName = "Tahoma";
                sht.Range("G26:I26").Style.Font.FontSize = 10;
                sht.Range("G26:I26").Merge();
                //value
                sht.Range("G27").Value = ds.Tables[0].Rows[0]["countryoforigin"];
                sht.Range("G27:I27").Style.Font.FontName = "Tahoma";
                sht.Range("G27:I27").Style.Font.FontSize = 10;
                sht.Range("G27:I27").Merge();
                sht.Range("G27:I27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Final Destination
                sht.Range("J26").Value = "Country of Final Destination";
                sht.Range("J26:L26").Style.Font.FontName = "Tahoma";
                sht.Range("J26:L26").Style.Font.FontSize = 10;
                sht.Range("J26:L26").Merge();
                //value
                sht.Range("J27").Value = ds.Tables[0].Rows[0]["countryoffinaldest"];
                sht.Range("J27:L27").Style.Font.FontName = "Tahoma";
                sht.Range("J27:L27").Style.Font.FontSize = 10;
                sht.Range("J27:L27").Style.NumberFormat.Format = "@";
                sht.Range("J27:L27").Merge();
                sht.Range("J27:L27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //***********Terms of Delivery and Payment
                sht.Range("G28").Value = "Terms of Delivery and Payment";
                sht.Range("G28:I28").Style.Font.FontName = "Tahoma";
                sht.Range("G28:I28").Style.Font.FontSize = 10;
                sht.Range("G28:I28").Merge();
                //value
                sht.Range("I29").Value = ds.Tables[0].Rows[0]["Deliveryterm"];
                sht.Range("G29").Value = "Terms of Payment";
                sht.Range("G29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("G30").Value = "Delivery WEEK :";

                sht.Range("G30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //sht.Range("J28").Value = "Pre Authenticated";
                //sht.Range("J29").Value = "For"+ ds.Tables[0].Rows[0]["companyName"];
                //sht.Range("J30").Value = "Auth. Sign.";
                sht.Range("H29:K30").Style.Font.FontName = "Tahoma";
                sht.Range("H29:K30").Style.Font.FontSize = 10;
                sht.Range("H29:K30").Style.NumberFormat.Format = "@";
                sht.Range("H29:K30").Style.Alignment.WrapText = true;
                sht.Range("H29:K30").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                sht.Range("A30:L30").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A26:F26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A28:F28").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("G27:L27").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("F25:F30").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("I25:I30").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //sht.Range("G29:I29").Merge();
                //sht.Range("G30:I30").Merge();
                //sht.Range("J28:K28").Merge();
                //sht.Range("J29:K29").Merge();
                //sht.Range("J30:K30").Merge();
                sht.Range("J28:K30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //*****************Nos
                sht.Range("A31:B31").Value = "Marks Nos./Container No.";
                sht.Range("A31:B31").Merge();
                sht.Range("D31").Value = "No and kind of Packages";
                sht.Range("D31:F31").Merge();
                sht.Range("H31").Value = "Description of Goods";
                sht.Range("H31:L31").Merge();
                //sht.Range("I31").Value = "Quantity";
                //sht.Range("J31").Value = "Rate";
                //sht.Range("k31").Value = "Amount";
                sht.Range("A31:L31").Style.Font.FontName = "Tahoma";
                sht.Range("A31:L31").Style.Font.FontSize = 10;
                sht.Range("A31:L31").Style.NumberFormat.Format = "@";
                sht.Range("L31").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //values
                sht.Range("A32:H32").Merge();
                sht.Range("A32").Value = "IKEA / FINAL DESTN.";
                sht.Range("A32").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A33:H37").Style.Font.FontName = "Tahoma";
                sht.Range("A33:H37").Style.Font.FontSize = 10;
                sht.Range("A33:H37").Style.NumberFormat.Format = "@";

                sht.Range("A33").SetValue("No." + ds.Tables[0].Compute("Min(MinrollNo)", "") + "To: " + ds.Tables[0].Compute("Max(Maxrollno)", ""));
                sht.Range("A33:B33").Merge();
                sht.Range("D33").Value = ds.Tables[0].Rows[0]["Noofrolls"] + " Pallets";
                sht.Range("D33:F33").Merge();
                sht.Range("H33").Value = ds.Tables[0].Rows[0]["descriptionofgoods"];
                sht.Range("H33:L35").Style.Alignment.WrapText = true;
                sht.Range("H33:L35").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("H33:L35").Merge();
                sht.Range("A35:L35").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //sht.Range("A38:L38").Merge();
                //********Details
                sht.Range("A37").Value = "P.O.NO.";
                sht.Range("A37:K37").Style.Font.FontName = "Tahoma";
                sht.Range("A37:K37").Style.Font.FontSize = 10;
                sht.Range("A37:K37").Style.Font.Bold = true;
                sht.Range("A37:K37").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A37:B37").Merge();
                sht.Range("C37").Value = "ART.NO.";
                sht.Range("C37:D37").Merge();
                sht.Range("E37").Value = "Description, Colour & Size";
                sht.Range("E37:H37").Merge();

                sht.Range("I37").Value = "HSN";
                //sht.Range("G39").Value = "SIZE(CM)";
                //sht.Range("H39").Value = "AREA SQ.MTR.";
                sht.Range("J37").Value = "QTY";
                sht.Range("K37").Value = "FCA/" + ds.Tables[0].Rows[0]["CIF"];
                sht.Range("L37").Value = "FCA/" + ds.Tables[0].Rows[0]["CIF"];
                sht.Range("L37").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("B36:B37").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D36:D37").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("H36:H37").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("I36:I37").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("J36:J37").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("K36:K37").Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //***********generate Loop
                i = 38;
                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                {
                    sht.Range("A" + i + ":K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("A" + i + ":K" + i).Style.Font.FontSize = 10;
                    sht.Range("A" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //pono
                    sht.Range("A" + i).Value = ds.Tables[0].Rows[ii]["Pono"];
                    sht.Range("A" + i + ":B" + i).Merge();
                    var _withPONO = sht.Range("B" + i);
                    _withPONO.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //Article Name
                    sht.Range("E" + i + ":H" + i).Merge();
                    sht.Range("E" + i).Value = ds.Tables[0].Rows[ii]["Design"] + "," + ds.Tables[0].Rows[ii]["Color"] + "&" + ds.Tables[0].Rows[ii]["width"] + "x" + ds.Tables[0].Rows[ii]["Length"];
                    var _withDESC = sht.Range("H" + i);
                    _withDESC.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //sht.Range("B" + i, "C" + i).Merge();
                    ////Art No.
                    sht.Range("C" + i).Value = ds.Tables[0].Rows[ii]["articleno"];
                    sht.Range("C" + i, "D" + i).Merge();
                    var _withART = sht.Range("D" + i);
                    _withART.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //HSN
                    sht.Range("I" + i).Value = ds.Tables[0].Rows[ii]["HSNCODE"];
                    var _withHSN = sht.Range("I" + i);
                    _withHSN.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //Size
                    // sht.Range("G" + i).Value = ds.Tables[0].Rows[ii]["width"] + "x" + ds.Tables[0].Rows[ii]["Length"];
                    //Area
                    //  sht.Range("H" + i).SetValue(ds.Tables[0].Rows[ii]["Area"]);
                    Area = Decimal.Parse(Area.ToString()) + Decimal.Parse(ds.Tables[0].Rows[ii]["Area"].ToString());
                    //QTY
                    sht.Range("J" + i).SetValue(ds.Tables[0].Rows[ii]["Pcs"]);
                    Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[ii]["Pcs"]);
                    //
                    var _withQTY = sht.Range("J" + i);
                    _withQTY.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //rate
                    sht.Range("K" + i).SetValue(ds.Tables[0].Rows[ii]["Price"]);
                    var _withRATE = sht.Range("K" + i);
                    _withRATE.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    //Amount
                    sht.Range("L" + i).SetValue(ds.Tables[0].Rows[ii]["amount"]);
                    Amount = Amount + Convert.ToDecimal(ds.Tables[0].Rows[ii]["amount"]);
                    var _withAMT = sht.Range("L" + i);
                    _withAMT.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //
                    sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("H" + i + ":K" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("I" + i).Style.NumberFormat.Format = "@";
                    sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("H" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("K" + i).Style.NumberFormat.Format = "#,###0.00";
                    //border
                    // sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //   sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    i = i + 1;
                }
                // sht.Range("B" + (i - 2) + ":C" + (i - 2)).Merge();

                sht.Range("A" + (i + 1)).SetValue("SUPPLIER#21899");
                //sht.Range("A" + (i + 1)).SetValue("SUPPLIER#21899");
                //sht.Range("F10").Value = "SUPPLIER NO.: 21899";
                sht.Range("A" + (i + 2)).Value = "ECIS#" + ds.Tables[0].Rows[0]["otherref"];
                var _withPONOlast = sht.Range("B" + i + ":B" + (i + 2));
                _withPONOlast.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _withDESClast = sht.Range("H" + i + ":H" + (i + 2));
                _withDESClast.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _withlast = sht.Range("A" + (i + 3) + ":L" + (i + 3));
                //   _with29.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _withlast.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                var _withARTlast = sht.Range("D" + i + ":D" + (i + 2));
                _withARTlast.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _withHSNlast = sht.Range("I" + i + ":I" + (i + 2));
                _withHSNlast.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _withQTYlast = sht.Range("J" + i + ":J" + (i + 2));
                _withQTYlast.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _withRATElast = sht.Range("K" + i + ":K" + (i + 2));
                _withRATElast.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _withAMTlast = sht.Range("L" + i + ":L" + (i + 2));
                _withAMTlast.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     //sht.Range("A" + i + ":A" + (i + 2)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     //sht.Range("K" + i + ":K" + (i + 2)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     ////
                //     //var _with30 = sht.Range("J" + i + ":J" + (i + 2));
                //     //_with30.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     //_with30.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     ////End Of loop
                i = i + 3;
                sht.Range("H" + i + ":J" + i).Style.Font.FontName = "Tahoma";
                sht.Range("H" + i + ":J" + i).Style.Font.FontSize = 10;
                //sht.Range("G" + i + ":I" + i).Style.NumberFormat.Format = "@";
                sht.Range("H" + i).Style.NumberFormat.Format = "@";
                sht.Range("J" + i).Style.NumberFormat.Format = "@";
                sht.Range("H" + i + ":J" + i).Style.Font.Bold = true;
                sht.Range("H" + i + ":J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range("H" + i).Value = "Total:";
                // sht.Range("H" + i).SetValue(Area);
                sht.Range("J" + i).SetValue(Pcs);
                sht.Range("l" + i).SetValue(Amount);
                sht.Range("A" + i + ":G" + i).Merge();
                sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                var _withHSNTOTAL = sht.Range("I" + (i - 1) + ":I" + (i));
                _withHSNTOTAL.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _withHSNTOTAL.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                var _withQTYTOTAL = sht.Range("J" + (i - 1) + ":J" + (i));
                _withQTYTOTAL.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _withRATETOTAL = sht.Range("K" + (i - 1) + ":K" + (i));
                _withRATETOTAL.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("J" + (i - 1) + ":L" + (i - 1)).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + (i + 1) + ":L" + (i + 1)).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("C" + i + ":D" + i).Merge();
                sht.Range("C" + i).Value = "TOTAL SQ.MTR  : ";
                sht.Range("E" + i).Value = Area;



                //AMOUNT IN WORDS
                i = i + 1;
                sht.Range("A" + i).Value = "Amount in Words";
                sht.Range("A" + i + ":B" + i).Merge();
                sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 10;
                sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";
                string amountinwords = "";
                string amt = Amount.ToString();
                string val = "", paise = "";

                if (amt.IndexOf('.') > 0)
                {
                    val = amt.ToString().Split('.')[0];
                    amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val));
                }
                else
                {
                    amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(Amount));
                }

                string Pointamt = string.Format("{0:0.00}", Amount.ToString("0.00"));
                val = "";
                if (Pointamt.IndexOf('.') > 0)
                {
                    val = Pointamt.ToString().Split('.')[1];
                    if (Convert.ToInt32(val) > 0)
                    {
                        paise = ds.Tables[0].Rows[0]["Currencytypeps"] + " " + ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val)) + " ";
                    }
                }
                amountinwords = ds.Tables[0].Rows[0]["CIF"] + " " + amountinwords + " " + paise + "Only";
                sht.Range("C" + i).Value = amountinwords.ToUpper();
                sht.Range("C" + i + ":K" + (i + 1)).Style.Font.FontName = "Tahoma";
                sht.Range("C" + i + ":K" + (i + 1)).Style.Font.FontSize = 10;
                sht.Range("C" + i + ":K" + (i + 1)).Style.NumberFormat.Format = "@";
                sht.Range("C" + i + ":K" + (i + 1)).Style.Font.Bold = true;
                sht.Range("C" + i + ":K" + (i + 1)).Style.Alignment.WrapText = true;
                sht.Range("C" + i + ":K" + (i + 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("C" + i + ":K" + (i + 1)).Merge();

                //     //********
                i = i + 1;
                //     sht.Range("A" + i + ":A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     sht.Range("K" + i + ":K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                str = "Select Design as ArticleNo,contents,Round(sum(amount)/sum(Area),2) as Rate From V_PackingDetailsIkea Where PackingId=" + DDinvoiceNo.SelectedValue + " group by Design,contents";
                DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                sht.Range("A" + i).Value = "TOTAL PKGS  :";
                sht.Range("A" + (i + 1)).Value = "TOTAL PCS     :";
                sht.Range("A" + (i + 2)).Value = "TOTAL GR. WT  :";
                sht.Range("A" + (i + 3)).Value = "TOTAL NT. WT  :";
                //  sht.Range("A" + (i + 4)).Value = "TOTAL SQ.MTR  :";
                sht.Range("A" + (i + 4)).Value = "TOTAL VOLUME  :";
                sht.Range("A" + i + ":A" + (i + 4)).Style.Font.FontSize = 10;
                sht.Range("A" + i + ":A" + (i + 4)).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":A" + (i + 4)).Style.Font.FontName = "Courier New";
                //value            
                sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontName = "Tahoma";
                sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontSize = 10;
                sht.Range("C" + i + ":C" + (i + 5)).Style.NumberFormat.Format = "@";
                sht.Range("C" + i).SetValue(ds.Tables[0].Rows[0]["Noofrolls"]);
                sht.Range("C" + (i + 1)).SetValue(Pcs);
                sht.Range("C" + (i + 2)).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["grosswt"]) + " Kgs.";
                sht.Range("C" + (i + 3)).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["netwt"]) + " Kgs.";
                //  sht.Range("C" + (i + 4)).Value = String.Format("{0:#,0.00}", Area) + " Sq.Mtr";
                sht.Range("C" + (i + 4)).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["Volume"]) + " CBM";
                //   sht.Range("A" + (i + 1) + ":L" + (i + 1)).Style.Border.TopBorder = XLBorderStyleValues.Thin;


                i = i + 6;
                sht.Range("A" + i + ":F" + i).Merge();
                sht.Range("A" + i).Value = "Supply Meant for Export With Payment of Integrated Tax(" + ds.Tables[0].Rows[0]["GSTTypeName"].ToString() + ")";
                sht.Range("A" + i + ":F" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":F" + i).Style.Font.FontSize = 10;
                sht.Range("A" + i + ":F" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":F" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A" + (i + 1) + ":L" + (i + 1)).Style.Border.TopBorder = XLBorderStyleValues.Thin;



                if (string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["LUTARNNO"])))
                {
                    i = i + 1;
                    sht.Range("A" + i + ":C" + i).Merge();
                    sht.Range("A" + i).Value = "Description";
                    sht.Range("A" + i + ":K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("A" + i + ":K" + i).Style.Font.FontSize = 10;
                    sht.Range("A" + i + ":K" + i).Style.Font.Bold = true;
                    sht.Range("A" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    sht.Range("D" + i).Value = "HSN";
                    sht.Range("E" + i).Value = "Qty PCS";
                    sht.Range("F" + i).Value = "Rate" + Environment.NewLine + "FCA/" + ds.Tables[0].Rows[0]["CIF"];
                    sht.Range("G" + i + ":H" + i).Merge();
                    sht.Range("G" + i).Value = "Amount";
                    //sht.Range("F" + i).Value = "CGST";
                    //sht.Range("G" + i).Value = "SGST";
                    sht.Range("I" + i).Value = "IGST";
                    sht.Range("J" + i).Value = "GST Amount";
                    sht.Range("K" + i + ":L" + i).Merge();
                    sht.Range("K" + i).Value = "Total Amount" + Environment.NewLine + " Including GST";
                    sht.Row(i).Height = 30;
                    sht.Range("I" + i + ":J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    var _withDESC = sht.Range("C" + i);
                    _withDESC.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    var _withHSN = sht.Range("D" + i);
                    _withHSN.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    var _withQTY = sht.Range("E" + i);
                    _withQTY.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    var _withRATE = sht.Range("F" + i);
                    _withRATE.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    var _withAMT = sht.Range("H" + i);
                    _withAMT.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    var _withIGST = sht.Range("I" + i);
                    _withIGST.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    var _withIGSTAMT = sht.Range("J" + i);
                    _withIGSTAMT.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + (i) + ":L" + (i)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    //***********generate Loop
                    i = i + 1;
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        sht.Range("A" + i + ":C" + i).Merge();
                        sht.Range("A" + i + ":K" + i).Style.Font.FontName = "Tahoma";
                        sht.Range("A" + i + ":K" + i).Style.Font.FontSize = 10;
                        sht.Range("A" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        //pono
                        sht.Range("A" + i).Value = ds.Tables[0].Rows[j]["Description"].ToString();
                        //Article Name
                        sht.Range("D" + i).Value = ds.Tables[0].Rows[j]["Hsncode"];
                        //sht.Range("B" + i, "C" + i).Merge();
                        //Art No.
                        sht.Range("E" + i).Value = ds.Tables[0].Rows[j]["Pcs"];
                        TotalPcs = TotalPcs + Convert.ToInt16(ds.Tables[0].Rows[j]["Pcs"]);
                        //sht.Range("D" + i, "E" + i).Merge();
                        sht.Range("F" + i).Value = ds.Tables[0].Rows[j]["Price"];
                        sht.Range("G" + i + ":H" + i).Merge();
                        sht.Range("G" + i).Value = ds.Tables[0].Rows[j]["amount"];

                        TotalAmount = TotalAmount + Convert.ToDecimal(ds.Tables[0].Rows[j]["amount"]);
                        //sht.Range("F" + i).Value = ds.Tables[0].Rows[j]["CGST"];
                        //sht.Range("G" + i).Value = ds.Tables[0].Rows[j]["SGST"];
                        sht.Range("I" + i).Value = ds.Tables[0].Rows[j]["IGST1"];

                        if (ds.Tables[0].Rows[j]["GSTType"].ToString() == "1")
                        {
                            GSTAmount = (Convert.ToDecimal(ds.Tables[0].Rows[j]["amount"]) / (Convert.ToDecimal(ds.Tables[0].Rows[j]["CGST"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["SGST"])) * 100);
                            TotalGSTAmount = TotalGSTAmount + GSTAmount;
                        }
                        else if (ds.Tables[0].Rows[j]["GSTType"].ToString() == "2")
                        {
                            GSTAmount = (Convert.ToDecimal(ds.Tables[0].Rows[j]["amount"]) / (Convert.ToDecimal(ds.Tables[0].Rows[j]["IGST1"])) * 100);
                            TotalGSTAmount = TotalGSTAmount + GSTAmount;
                        }
                        else
                        {
                            GSTAmount = 0;
                        }

                        sht.Range("J" + i).Value = GSTAmount;
                        sht.Range("K" + i + ":L" + i).Merge();
                        sht.Range("K" + i).Value = Convert.ToDecimal(ds.Tables[0].Rows[j]["amount"]) + GSTAmount;
                        TotalAmountWithGST = TotalAmountWithGST + Convert.ToDecimal(ds.Tables[0].Rows[j]["amount"]) + GSTAmount;

                        var _withDESCLOOP = sht.Range("C" + i);
                        _withDESCLOOP.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                        var _withHSNLOOP = sht.Range("D" + i);
                        _withHSNLOOP.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        var _withQTYLOOP = sht.Range("E" + i);
                        _withQTYLOOP.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        var _withRATELOOP = sht.Range("F" + i);
                        _withRATELOOP.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        var _withAMTLOOP = sht.Range("H" + i);
                        _withAMTLOOP.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        var _withIGSTLOOP = sht.Range("I" + i);
                        _withIGSTLOOP.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        var _withIGSTAMTLOOP = sht.Range("J" + i);
                        _withIGSTAMTLOOP.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + (i) + ":L" + (i)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                        // sht.Range("H" + i + ":K" + i).Style.NumberFormat.Format = "#,###0.00";
                        sht.Range("C" + i).Style.NumberFormat.Format = "@";
                        sht.Range("E" + i).Style.NumberFormat.Format = "#,###0.00";
                        sht.Range("I" + i).Style.NumberFormat.Format = "#,###0.00";
                        sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                        //border
                        //sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        //sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        i = i + 1;
                    }

                    i = i + 1;
                    sht.Range("D" + i).Value = "Total:";
                    sht.Range("E" + i).SetValue(TotalPcs);
                    sht.Range("G" + i + ":H" + i).Merge();
                    sht.Range("G" + i).SetValue(TotalAmount);

                    sht.Range("J" + i).SetValue(TotalGSTAmount);
                    sht.Range("K" + i + ":L" + i).Merge();
                    sht.Range("K" + i).SetValue(TotalAmountWithGST);

                    sht.Range("B" + i + ":J" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("B" + i + ":J" + i).Style.Font.FontSize = 10;
                    sht.Range("B" + i + ":J" + i).Style.NumberFormat.Format = "@";
                    sht.Range("B" + i + ":J" + i).Style.Font.Bold = true;

                    //sht.Range("A" + i + ":F" + i).Merge();
                    sht.Range("E" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("G" + i).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("K" + i).Style.NumberFormat.Format = "#,###0.00";
                    ////border
                    //var _with244 = sht.Range("C" + i);
                    //_with244.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    //_with244.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    //
                    var _with245 = sht.Range("C" + i + ":L" + i);
                    _with245.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    _with245.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    _with245.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    _with245.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                else
                {
                    sht.Range("G" + i + ":K" + i).Merge();
                    sht.Range("G" + i).Value = "ACKNOWLEDGEMENT FOR LUT ARN No." + ds.Tables[0].Rows[0]["LUTARNNO"].ToString() + "AA0904180038270 Dtd:" + DateTime.Now.ToShortDateString();
                    sht.Range("G" + i + ":K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("G" + i + ":K" + i).Style.Font.FontSize = 10;
                    sht.Range("G" + i + ":K" + i).Style.Font.Bold = true;
                    sht.Range("G" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);



                }
                i = i + 2;
                sht.Range("A" + i + ":G" + i).Merge();
                sht.Range("A" + i).Value = "STATEMENT ON ORIGIN";
                sht.Range("A" + i + ":G" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":G" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":G" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                var _withSO = sht.Range("A" + i + ":G" + i);
                _withSO.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                _withSO.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                i = i + 1;
                sht.Range("A" + i + ":H" + i).Merge();
                sht.Range("A" + i).Value = "The Exporter INREX0694000965EC026 of the products covered by this document declares that, except where otherwise clearly indicated, these products are of INDIA preferential origin according to rules of origin of the Generalized system of preferences of the European Union and that the origin criterion met is 'P'";
                sht.Range("A" + i + ":H" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":H" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":H" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":H" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Justify);
                sht.Row(i).Height = 55;
                var _withSTATEMENT = sht.Range("A" + i + ":H" + i);
                _withSTATEMENT.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                _withSTATEMENT.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //var _with467 = sht.Range("A" + i + ":K" + i);
                //_with467.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _withSTATEMENT.Style.Border.RightBorder = XLBorderStyleValues.Thin;


                i = i + 1;
                sht.Range("A" + i + ":C" + i).Merge();
                sht.Range("A" + i).Value = "R.B.I Code No. 02422282600008";
                sht.Range("A" + i + ":C" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":C" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":C" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":C" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);



                sht.Range("E" + i + ":G" + i).Merge();
                sht.Range("E" + i).Value = "Factory Address:";
                sht.Range("E" + i + ":G" + i).Style.Font.FontName = "Tahoma";
                sht.Range("E" + i + ":G" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("E" + i + ":G" + i).Style.Font.Bold = true;
                sht.Range("E" + i + ":G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);



                i = i + 1;
                sht.Range("A" + i + ":C" + i).Merge();
                sht.Range("A" + i).Value = "I.E.  Code  No.: 0694000965";
                sht.Range("A" + i + ":C" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":C" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":C" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":C" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("E" + i + ":G" + i).Merge();
                sht.Range("E" + i).Value = " AGRA-MATHURA ROAD, ARTONI" + Environment.NewLine + "AGRA-282 007 (INDIA)";
                sht.Range("E" + i + ":G" + i).Style.Font.FontName = "Tahoma";
                sht.Range("E" + i + ":G" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("E" + i + ":G" + i).Style.Font.Bold = true;
                sht.Range("E" + i + ":G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Row(i).Height = 40;

                //var _with560 = sht.Range("A" + i + ":K" + i);
                //_with560.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with560.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //***********Sig and date
                i = i + 2;
                //i = 70;
                sht.Range("A" + i + ":H" + i).Merge();
                sht.Range("I" + i).Value = "Signature";
                sht.Range("I" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i).Style.Font.FontSize = 10;
                sht.Range("I" + i).Style.NumberFormat.Format = "@";

                sht.Range("K" + i).Value = "For " + ds.Tables[0].Rows[0]["companyname"];
                sht.Range("J" + i + ":K" + i).Style.Font.FontName = "Tahoma";
                sht.Range("J" + i + ":K" + i).Style.Font.FontSize = 9;
                sht.Range("J" + i + ":K" + i).Style.NumberFormat.Format = "@";
                sht.Range("J" + i + ":K" + i).Style.Font.Bold = true;
                sht.Range("J" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("J" + i + ":K" + i).Merge();
                var _with38 = sht.Range("I" + i + ":K" + i);
                _with38.Style.Border.TopBorder = XLBorderStyleValues.Thin;

                sht.Range("I" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                sht.Range("A" + i + ":A" + (i + 3)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 3)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //**************Declaration
                i = i + 1;
                sht.Range("I" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //i = 71;
                sht.Range("A" + i).Value = "Declaration";
                sht.Range("A" + i + ":B" + i).Merge();
                sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 10;
                sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":B" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //value
                i = i + 1;
                sht.Range("I" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i).Value = "We declare that this invoice shows the actual price of the";
                sht.Range("A" + i + ":F" + i).Merge();
                sht.Range("A" + i + ":F" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":F" + i).Style.Font.FontSize = 10;
                sht.Range("A" + i + ":F" + i).Style.NumberFormat.Format = "@";
                i = i + 1;
                sht.Range("A" + i).Value = "goods described and that all particulars are true and correct";
                sht.Range("A" + i + ":F" + i).Merge();
                sht.Range("A" + i + ":F" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":F" + i).Style.Font.FontSize = 10;
                sht.Range("A" + i + ":F" + i).Style.NumberFormat.Format = "@";

                //**********
                //sht.Range("I" + i + ":J" + i).Merge();
                //sht.Range("I" + i + ":J" + i).Value = ds.Tables[0].Rows[0]["InvoiceDate"];
                //sht.Range("I" + i + ":J" + i).Style.Font.FontName = "Tahoma";
                //sht.Range("I" + i + ":J" + i).Style.Font.FontSize = 12;
                //sht.Range("I" + i + ":J" + i).Style.NumberFormat.Format = "dd/MMM/yyyy";
                //sht.Range("I" + i + ":J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("K" + i).Value = "Auth Sign";
                sht.Range("K" + i).Style.Font.FontName = "Tahoma";
                sht.Range("K" + i).Style.Font.FontSize = 10;
                sht.Range("K" + i).Style.NumberFormat.Format = "@";
                sht.Range("K" + i).Style.Font.Bold = true;
                sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //
                sht.Range("A" + (i) + ":L" + (i)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //

                //     //*****************Set Borders
                //     //var _with1 = sht.Range("A2:K" + i);
                //     //_with1.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //     //_with1.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //     //_with1.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     //_with1.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     //************************
                //     var _with2 = sht.Range("A11:K11");
                //     _with2.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //     //
                //     var _with3 = sht.Range("G3:K4");
                //     _with3.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     _with3.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     var _with41 = sht.Range("G3:K3");
                //     _with41.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //     var _with40 = sht.Range("G4:K4");
                //     _with40.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //     //
                //     //var _with4 = sht.Range("G5:I6");
                //     //_with4.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     //_with4.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     var _with42 = sht.Range("G5:I5");
                //     _with42.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //     var _with43 = sht.Range("G6:K6");
                //     _with43.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                //     //
                //     //var _with5 = sht.Range("J5:K5");
                //     //_with5.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //     //var _with44 = sht.Range("J5:K6");
                //     //_with44.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     //var _with45 = sht.Range("J6:K6");
                //     //_with45.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //     //
                //     var _with6 = sht.Range("G7:K8");
                //     _with6.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     _with6.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     var _with46 = sht.Range("G8:K8");
                //     _with46.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //     //
                //     var _with7 = sht.Range("G9:K11");
                //     _with7.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     _with7.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     var _with47 = sht.Range("G12:K25");
                //     _with47.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     sht.Range("G12:I12").Style.Border.RightBorder = XLBorderStyleValues.None;

                //     //
                //     var _with8 = sht.Range("A24:F24");
                //     _with8.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //     var _with48 = sht.Range("A12:F24");
                //     _with48.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //     sht.Range("A12:B12").Style.Border.RightBorder = XLBorderStyleValues.None;
                //     sht.Range("A18:B18").Style.Border.RightBorder = XLBorderStyleValues.None;
                //     sht.Range("G12:H12").Style.Border.RightBorder = XLBorderStyleValues.None;
                //     sht.Range("G18:H20").Style.Border.RightBorder = XLBorderStyleValues.None;

                //     //
                //     var _with9 = sht.Range("A25:C26");
                //     _with9.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     _with9.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     //sht.Range("A24:C24").Style.Border.BottomBorder = XLBorderStyleValues.None;

                //     sht.Range("A26:C26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                //     sht.Range("G26:I26").Style.Border.BottomBorder = XLBorderStyleValues.None;

                //     sht.Range("J26:K26").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //     //
                //     var _with10 = sht.Range("D25:F26");
                //     _with10.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //     _with10.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     sht.Range("D25:F25").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //     //
                //     var _with11 = sht.Range("A27:C28");
                //     _with11.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //     _with11.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     _with11.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     sht.Range("A27:C27").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //     //
                //     var _with12 = sht.Range("D27:F28");
                //     _with12.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //     _with12.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     sht.Range("D27:F27").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //     //
                //     var _with13 = sht.Range("A29:C30");
                //     _with13.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     _with13.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     sht.Range("A29:C29").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //     sht.Range("A30:C30").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //     //
                //     var _with14 = sht.Range("D29:F30");
                //     _with14.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //     _with14.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     sht.Range("D29:G29").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //     sht.Range("G29").Style.Border.BottomBorder = XLBorderStyleValues.None;

                //     //
                //     var _with15 = sht.Range("G26:I27");
                //     _with15.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     _with15.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     sht.Range("G26:I26").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //     sht.Range("G27:I27").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //     //
                //     var _with16 = sht.Range("J26:K27");
                //     _with16.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     sht.Range("J26:K26").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //     sht.Range("J27:K27").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //     //
                //     var _with17 = sht.Range("G28:K30");
                //     _with17.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     _with17.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     sht.Range("G30:K30").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //     sht.Range("G29:G30").Style.Border.RightBorder = XLBorderStyleValues.None;
                //     sht.Range("H29:K30").Style.Border.LeftBorder = XLBorderStyleValues.None;
                //     //
                //     var _with18 = sht.Range("I31:I38");
                //     _with18.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     _with18.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     //
                //     var _with19 = sht.Range("J31:J38");
                //     _with19.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     _with19.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     //
                //     var _with20 = sht.Range("A39:K39");
                //     _with20.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //     _with20.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //     //
                //     var _with21 = sht.Range("I39:I39");
                //     _with21.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     _with21.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     //
                //     var _with22 = sht.Range("J39:J39");
                //     _with22.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     _with22.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //     //
                //     sht.Range("A31:A38").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     sht.Range("K31:K38").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     sht.Range("A39:A39").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     sht.Range("K38:K38").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     sht.Range("A2:K2").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //     sht.Range("A3:A24").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //     sht.Range("F5:F6").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //     sht.Range("G4:K4").Merge();
                //     sht.Range("G15:K16").Merge();
                //     sht.Range("G21:K25").Merge();
                //  sht.Range("G26:I26").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //sht.Range("G25:K25").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                string Fileextension = "xlsx";
                string filename = "Invoice-" + validateFilename(ds.Tables[0].Rows[0]["TinvoiceNo"].ToString()) + "." + Fileextension + "";
                Path = Server.MapPath("~/InvoiceExcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
                lblmsg.Text = "Invoice Excel Format downloaded successfully.";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "AltR1", "alert('No Record Found to Export')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
    protected void Exporttoexcel_Packing_Kaysons()
    {
        lblmsg.Text = "Wait For Excel File to Download";
        int i = 0, Pcs = 0, TotalRolls = 0;
        Decimal Area = 0;
        string Path = "";
        if (!Directory.Exists(Server.MapPath("~/PackingExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/PackingExcel/"));
        }
        try
        {
            string str = @"select *,VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1) as Pcsperroll,VD.Pcs/(VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1)) as TotalRoll From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue + " order by MinrollNo";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("PackingList");
                //************set cell width
                //Page
                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                sht.PageSetup.AdjustTo(71);
                //sht.PageSetup.FitToPages(1, 5);                
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                sht.PageSetup.VerticalDpi = 300;
                sht.PageSetup.HorizontalDpi = 300;
                //
                sht.PageSetup.Margins.Top = 0.55;
                sht.PageSetup.Margins.Left = 0;
                sht.PageSetup.Margins.Right = 0;
                sht.PageSetup.Margins.Bottom = 0.53;
                sht.PageSetup.Margins.Header = 0.5;
                sht.PageSetup.Margins.Footer = 0.5;
                sht.PageSetup.CenterHorizontally = true;
                sht.PageSetup.SetScaleHFWithDocument();
                //************
                sht.Column("A").Width = 10.29;
                sht.Column("B").Width = 8.43;
                sht.Column("C").Width = 8.43;
                sht.Column("D").Width = 11.00;
                sht.Column("E").Width = 9.71;
                sht.Column("F").Width = 12.57;
                sht.Column("G").Width = 10.14;
                sht.Column("H").Width = 13.14;
                sht.Column("I").Width = 11.71;
                sht.Column("J").Width = 14.29;
                sht.Column("K").Width = 19.29;
                //************
                sht.Row(1).Height = 15.75;
                //********Header
                sht.Range("A1").Value = "PACKING LIST";
                sht.Range("A1:K1").Style.Font.FontName = "Tahoma";
                sht.Range("A1:K1").Style.Font.FontSize = 12;
                sht.Range("A1:K1").Style.Font.Bold = true;
                sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:K1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1:K1").Style.NumberFormat.Format = "@";
                sht.Range("A1:K1").Merge();
                //********Exporter
                sht.Range("A2").Value = "Exporter";
                sht.Range("A2:F2").Style.Font.FontSize = 12;
                sht.Range("A2:F2").Style.Font.FontName = "Tahoma";
                sht.Range("A2:F2").Style.NumberFormat.Format = "@";
                sht.Range("A2:F2").Merge();

                //CompanyName
                sht.Range("A3").Value = ds.Tables[0].Rows[0]["companyName"];
                sht.Range("A3:F3").Style.Font.FontSize = 12;
                sht.Range("A3:F3").Style.Font.FontName = "Tahoma";
                sht.Range("A3:F3").Style.NumberFormat.Format = "@";
                sht.Range("A3:F3").Style.Font.Bold = true;
                sht.Range("A3:F3").Merge();
                //Address
                sht.Range("A4").Value = ds.Tables[0].Rows[0]["compaddr1"];
                sht.Range("A4:F4").Style.Font.FontSize = 12;
                sht.Range("A4:F4").Style.Font.FontName = "Tahoma";
                sht.Range("A4:F4").Style.NumberFormat.Format = "@";
                sht.Range("A4:F4").Merge();
                //address2
                sht.Range("A5").Value = ds.Tables[0].Rows[0]["compaddr2"] + "," + ds.Tables[0].Rows[0]["compaddr3"];
                sht.Range("A5:F5").Style.Font.FontSize = 12;
                sht.Range("A5:F5").Style.Font.FontName = "Tahoma";
                sht.Range("A5:F5").Style.NumberFormat.Format = "@";
                sht.Range("A5:F5").Merge();
                //TiN No
                sht.Range("A6").Value = "GSTIN#" + ds.Tables[0].Rows[0]["gstno"];
                sht.Range("A6:F6").Style.Font.FontSize = 12;
                sht.Range("A6:F6").Style.Font.FontName = "Tahoma";
                sht.Range("A6:F6").Style.NumberFormat.Format = "@";
                sht.Range("A6:F6").Style.Font.Bold = true;
                sht.Range("A6:F6").Merge();
                //**********INvoiceNodate
                sht.Range("G2").Value = "Invoice No./Date";
                sht.Range("G2:K2").Style.Font.FontSize = 12;
                sht.Range("G2:K2").Style.Font.FontName = "Tahoma";
                sht.Range("G2:K2").Style.NumberFormat.Format = "@";

                sht.Range("G2:K2").Merge();
                //value
                sht.Range("G3").Value = ds.Tables[0].Rows[0]["TInvoiceNo"] + " Dated " + ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("G3:K3").Style.Font.FontSize = 12;
                sht.Range("G3:K3").Style.Font.FontName = "Tahoma";
                sht.Range("G3:K3").Style.NumberFormat.Format = "@";
                sht.Range("G3:K3").Merge();
                sht.Range("G3", "K3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //*************IE Code
                sht.Range("G4").Value = "IE Code No.";
                sht.Range("G4:I4").Style.Font.FontSize = 12;
                sht.Range("G4:I4").Style.Font.FontName = "Tahoma";
                sht.Range("G4:I4").Style.NumberFormat.Format = "@";
                sht.Range("G4:I4").Merge();
                //value
                sht.Range("G5").Value = ds.Tables[0].Rows[0]["IEcode"];
                sht.Range("G5:I5").Style.Font.FontSize = 12;
                sht.Range("G5:I5").Style.Font.FontName = "Tahoma";
                sht.Range("G5:I5").Style.NumberFormat.Format = "@";
                sht.Range("G5:I5").Merge();
                sht.Range("G5", "I5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //***********GRI Form No
                sht.Range("J4").Value = "GRI Form No.";
                sht.Range("J4:K4").Style.Font.FontSize = 12;
                sht.Range("J4:K4").Style.Font.FontName = "Tahoma";
                sht.Range("J4:K4").Style.NumberFormat.Format = "@";
                sht.Range("J4:K4").Merge();
                // value
                sht.Range("J5").Value = "";
                sht.Range("J5:K5").Style.Font.FontSize = 12;
                sht.Range("J5:K5").Style.Font.FontName = "Tahoma";
                sht.Range("J5:K5").Style.NumberFormat.Format = "@";
                sht.Range("J5:K5").Merge();
                sht.Range("J5", "K5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //*************Buyer's Order No and Date
                sht.Range("G6").Value = "Buyer's Order No. / Date";
                sht.Range("G6:K6").Style.Font.FontSize = 12;
                sht.Range("G6:K6").Style.Font.FontName = "Tahoma";
                sht.Range("G6:K6").Style.NumberFormat.Format = "@";
                sht.Range("G6:K6").Merge();
                //value
                sht.Range("G7").SetValue(ds.Tables[0].Rows[0]["TorderNo"]);
                sht.Range("G7:K7").Style.Font.FontSize = 12;
                sht.Range("G7:K7").Style.Font.FontName = "Tahoma";
                sht.Range("G7:K7").Style.NumberFormat.Format = "@";
                sht.Range("G7:K7").Merge();
                sht.Range("G7", "K7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //**************Other Reference(s)
                sht.Range("G8").Value = "Other Reference(s)";
                sht.Range("G9").Value = "CONSIGNMENT NO.:" + ds.Tables[0].Rows[0]["otherref"];
                sht.Range("G10").Value = "SUPPLIER NO.:";
                sht.Range("G8:G10").Style.Font.FontSize = 12;
                sht.Range("G8:G10").Style.Font.FontName = "Tahoma";
                sht.Range("G8:G10").Style.NumberFormat.Format = "@";
                sht.Range("G8:K8").Merge();
                sht.Range("G9:K9").Merge();
                sht.Range("G10:K10").Merge();
                //*************Consignee
                sht.Range("A11").Value = "Consignee";
                sht.Range("C11").Value = ds.Tables[0].Rows[0]["consignee_dt"];
                sht.Range("A11:B11").Style.Font.FontSize = 12;
                sht.Range("A11:B11").Style.Font.FontName = "Tahoma";
                sht.Range("A11:B11").Style.NumberFormat.Format = "@";


                sht.Range("C11:F11").Style.Font.FontSize = 12;
                sht.Range("C11:F11").Style.Font.FontName = "Tahoma";
                sht.Range("C11:F11").Style.NumberFormat.Format = "@";
                sht.Range("C11:F11").Style.Font.Bold = true;

                sht.Range("A11:B11").Merge();
                sht.Range("C11:F11").Merge();
                //value
                sht.Range("A12").Value = ds.Tables[0].Rows[0]["consignee"];
                sht.Range("A12:F12").Style.Font.FontSize = 12;
                sht.Range("A12:F12").Style.Font.FontName = "Tahoma";
                sht.Range("A12:F12").Style.NumberFormat.Format = "@";
                sht.Range("A12:F12").Style.Font.Bold = true;
                sht.Range("A12:F12").Merge();
                //**
                sht.Range("A13").Value = ds.Tables[0].Rows[0]["consignee_address"];
                sht.Range("A13:F16").Style.Font.FontSize = 12;
                sht.Range("A13:F16").Style.Font.FontName = "Tahoma";
                sht.Range("A13:F16").Style.NumberFormat.Format = "@";

                sht.Range("A13", "F16").Style.Alignment.WrapText = true;
                sht.Range("A13", "F16").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A13:F16").Merge();
                //***********Notify
                sht.Range("A17").Value = "Notify Party";
                sht.Range("C17").Value = ds.Tables[0].Rows[0]["Notifyparty_dt"];
                sht.Range("A17:B17").Style.Font.FontSize = 12;
                sht.Range("A17:B17").Style.Font.FontName = "Tahoma";
                sht.Range("A17:B17").Style.NumberFormat.Format = "@";

                sht.Range("A17:B17").Style.Font.Underline = XLFontUnderlineValues.Single;
                sht.Range("C17:F17").Style.Font.FontSize = 12;
                sht.Range("C17:F17").Style.Font.FontName = "Tahoma";
                sht.Range("C17:F17").Style.NumberFormat.Format = "@";
                sht.Range("C17:F17").Style.Font.Bold = true;

                sht.Range("A17:B17").Merge();
                sht.Range("C17:F17").Merge();
                //value
                sht.Range("A18").Value = ds.Tables[0].Rows[0]["Notifyparty"];
                sht.Range("A18:F18").Style.Font.FontSize = 12;
                sht.Range("A18:F18").Style.Font.FontName = "Tahoma";
                sht.Range("A18:F18").Style.NumberFormat.Format = "@";
                sht.Range("A18:F18").Style.Font.Bold = true;

                sht.Range("A18:F18").Merge();

                sht.Range("A19").Value = ds.Tables[0].Rows[0]["notifyparty_address"];
                sht.Range("A19:F23").Style.Font.FontSize = 12;
                sht.Range("A19:F23").Style.Font.FontName = "Tahoma";
                sht.Range("A19:F23").Style.NumberFormat.Format = "@";

                sht.Range("A19", "F23").Style.Alignment.WrapText = true;
                sht.Range("A19", "F23").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A19:F23").Merge();
                //***********Receiver address
                sht.Range("G11").Value = "Receiver Address";
                sht.Range("G11:I11").Style.Font.FontSize = 12;
                sht.Range("G11:I11").Style.Font.FontName = "Tahoma";
                sht.Range("G11:I11").Style.NumberFormat.Format = "@";
                sht.Range("G11:I11").Merge();
                //values
                sht.Range("J11").Value = ds.Tables[0].Rows[0]["Destcode"];
                sht.Range("J11:K11").Style.Font.FontSize = 12;
                sht.Range("J11:K11").Style.Font.FontName = "Tahoma";
                sht.Range("J11:K11").Style.NumberFormat.Format = "@";
                sht.Range("J11:K11").Style.Font.Bold = true;

                sht.Range("J11:K11").Merge();
                //****** 1.
                sht.Range("G12").Value = "1. " + ds.Tables[0].Rows[0]["Receiver"];
                sht.Range("G12:K12").Style.Font.FontSize = 12;
                sht.Range("G12:K12").Style.Font.FontName = "Tahoma";
                sht.Range("G12:K12").Style.NumberFormat.Format = "@";
                sht.Range("G12:K12").Style.Font.Bold = true;
                sht.Range("G12:K12").Merge();
                //*
                sht.Range("G13").Value = ds.Tables[0].Rows[0]["Receiver_address"];
                sht.Range("G13:K15").Style.Font.FontSize = 12;
                sht.Range("G13:K15").Style.Font.FontName = "Tahoma";
                sht.Range("G13:K15").Style.NumberFormat.Format = "@";

                sht.Range("G13:K15").Style.Alignment.WrapText = true;
                sht.Range("G13:K15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("G13:K15").Merge();
                //****** 2.
                sht.Range("G16").Value = "2.Paying Agent : " + ds.Tables[0].Rows[0]["payingagent"];
                sht.Range("G16:K16").Style.Font.FontSize = 12;
                sht.Range("G16:K16").Style.Font.FontName = "Tahoma";
                sht.Range("G16:K16").Style.NumberFormat.Format = "@";
                sht.Range("G16:K16").Style.Font.Bold = true;

                sht.Range("G16:K16").Merge();
                //*
                sht.Range("I17").Value = ds.Tables[0].Rows[0]["payingagent_address"];
                sht.Range("I17:K19").Style.Font.FontSize = 12;
                sht.Range("I17:K19").Style.Font.FontName = "Tahoma";
                sht.Range("I17:K19").Style.NumberFormat.Format = "@";

                sht.Range("I17:K19").Style.Alignment.WrapText = true;
                sht.Range("I17:K19").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("I17:K19").Merge();
                //*******3.
                sht.Range("G20").Value = "Buyer (If other than Consignee)";
                sht.Range("G20:K20").Style.Font.FontSize = 12;
                sht.Range("G20:K20").Style.Font.FontName = "Tahoma";
                sht.Range("G20:K20").Style.NumberFormat.Format = "@";
                sht.Range("G20:K20").Merge();

                sht.Range("G21").Value = "3." + ds.Tables[0].Rows[0]["buyer_otherthanconsignee"];
                sht.Range("G21:K21").Style.Font.FontSize = 12;
                sht.Range("G21:K21").Style.Font.FontName = "Tahoma";
                sht.Range("G21:K21").Style.NumberFormat.Format = "@";
                sht.Range("G21:K21").Style.Font.Bold = true;

                sht.Range("G21:K21").Merge();
                //*
                sht.Range("G22").Value = ds.Tables[0].Rows[0]["otherthanconsignee_address"];
                sht.Range("G22:K27").Style.Font.FontSize = 12;
                sht.Range("G22:K27").Style.Font.FontName = "Tahoma";
                sht.Range("G22:K27").Style.NumberFormat.Format = "@";
                sht.Range("G22:K27").Style.Alignment.WrapText = true;
                sht.Range("G22:K27").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("G22:K27").Merge();
                //***********Pre-carriage By
                sht.Range("A24").Value = "Pre-Carriage By";
                sht.Range("A24:C24").Style.Font.FontSize = 12;
                sht.Range("A24:C24").Style.Font.FontName = "Tahoma";
                sht.Range("A24:C24").Style.NumberFormat.Format = "@";
                sht.Range("A24:C24").Merge();
                //value
                sht.Range("A25").Value = ds.Tables[0].Rows[0]["Pre_carriageby"];
                sht.Range("A25:C25").Style.Font.FontSize = 12;
                sht.Range("A25:C25").Style.Font.FontName = "Tahoma";
                sht.Range("A25:C25").Style.NumberFormat.Format = "@";
                sht.Range("A25:C25").Merge();
                sht.Range("A25:C25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //************Place of Receipt by Pre-Carrier
                sht.Range("D24").Value = "Place of Receipt by Pre-Carrier";
                sht.Range("D24:F24").Style.Font.FontSize = 12;
                sht.Range("D24:F24").Style.Font.FontName = "Tahoma";
                sht.Range("D24:F24").Style.NumberFormat.Format = "@";
                sht.Range("D24:F24").Merge();
                //value
                sht.Range("D25").Value = ds.Tables[0].Rows[0]["receiptby"];
                sht.Range("D25:F25").Style.Font.FontSize = 12;
                sht.Range("D25:F25").Style.Font.FontName = "Tahoma";
                sht.Range("D25:F25").Style.NumberFormat.Format = "@";
                sht.Range("D25:F25").Merge();
                sht.Range("D25:F25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Vessel/Flight No
                sht.Range("A26").Value = "Vessel/Flight No";
                sht.Range("A26:C26").Style.Font.FontSize = 12;
                sht.Range("A26:C26").Style.Font.FontName = "Tahoma";
                sht.Range("A26:C26").Style.NumberFormat.Format = "@";
                sht.Range("A26:C26").Merge();
                //value
                sht.Range("A27").Value = ds.Tables[0].Rows[0]["vessel_flightno"];
                sht.Range("A27:C27").Style.Font.FontSize = 12;
                sht.Range("A27:C27").Style.Font.FontName = "Tahoma";
                sht.Range("A27:C27").Style.NumberFormat.Format = "@";
                sht.Range("A27:C27").Merge();
                sht.Range("A27:C27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Loading
                sht.Range("D26").Value = "Port of Loading";
                sht.Range("D26:F26").Style.Font.FontSize = 12;
                sht.Range("D26:F26").Style.Font.FontName = "Tahoma";
                sht.Range("D26:F26").Style.NumberFormat.Format = "@";
                sht.Range("D26:F26").Merge();
                //value
                sht.Range("D27").Value = ds.Tables[0].Rows[0]["portload"];
                sht.Range("D27:F27").Style.Font.FontSize = 12;
                sht.Range("D27:F27").Style.Font.FontName = "Tahoma";
                sht.Range("D27:F27").Style.NumberFormat.Format = "@";
                sht.Range("D27:F27").Merge();
                sht.Range("D27:F27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Discharge
                sht.Range("A28").Value = "Port of Discharge";
                sht.Range("A28:C28").Style.Font.FontSize = 12;
                sht.Range("A28:C28").Style.Font.FontName = "Tahoma";
                sht.Range("A28:C28").Style.NumberFormat.Format = "@";
                sht.Range("A28:C28").Merge();
                //value
                sht.Range("A29").Value = ds.Tables[0].Rows[0]["portunload"];
                sht.Range("A29:C29").Style.Font.FontSize = 12;
                sht.Range("A29:C29").Style.Font.FontName = "Tahoma";
                sht.Range("A29:C29").Style.NumberFormat.Format = "@";
                sht.Range("A29:C29").Merge();
                sht.Range("A29:C29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Final Destination
                sht.Range("D28").Value = "Final Destination";
                sht.Range("D28:F28").Style.Font.FontSize = 12;
                sht.Range("D28:F28").Style.Font.FontName = "Tahoma";
                sht.Range("D28:F28").Style.NumberFormat.Format = "@";
                sht.Range("D28:F28").Merge();
                //value
                sht.Range("D29").Value = ds.Tables[0].Rows[0]["Destinationadd"];
                sht.Range("D29:F29").Style.Font.FontSize = 12;
                sht.Range("D29:F29").Style.Font.FontName = "Tahoma";
                sht.Range("D29:F29").Style.NumberFormat.Format = "@";
                sht.Range("D29:F29").Merge();
                sht.Range("D29:F29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Origin of Goods
                sht.Range("G28").Value = "Country of Origin of Goods";
                sht.Range("G28:I28").Style.Font.FontSize = 12;
                sht.Range("G28:I28").Style.Font.FontName = "Tahoma";
                sht.Range("G28:I28").Style.NumberFormat.Format = "@";
                sht.Range("G28:I28").Merge();
                //value
                sht.Range("G29").Value = ds.Tables[0].Rows[0]["countryoforigin"];
                sht.Range("G29:I29").Style.Font.FontSize = 12;
                sht.Range("G29:I29").Style.Font.FontName = "Tahoma";
                sht.Range("G29:I29").Style.NumberFormat.Format = "@";
                sht.Range("G29:I29").Merge();
                sht.Range("G29:I29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Final Destination
                sht.Range("J28").Value = "Country of Final Destination";
                sht.Range("J28:K28").Style.Font.FontSize = 12;
                sht.Range("J28:K28").Style.Font.FontName = "Tahoma";
                sht.Range("J28:K28").Style.NumberFormat.Format = "@";
                sht.Range("J28:K28").Merge();
                //value
                sht.Range("J29").Value = ds.Tables[0].Rows[0]["countryoffinaldest"];
                sht.Range("J29:K29").Style.Font.FontSize = 12;
                sht.Range("J29:K29").Style.Font.FontName = "Tahoma";
                sht.Range("J29:K29").Style.NumberFormat.Format = "@";
                sht.Range("J29:K29").Merge();
                sht.Range("J29:K29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //*****************Nos
                sht.Range("A30").Value = "Nos.";
                sht.Range("B30").Value = "No and kind of Packages";
                sht.Range("B30:D30").Merge();

                sht.Range("E30").Value = "Description of Goods";
                sht.Range("E30:J30").Merge();
                //sht.Range("k30").Value = "Quantity";                
                sht.Range("A30:K30").Style.Font.FontSize = 12;
                sht.Range("A30:K30").Style.Font.FontName = "Tahoma";
                sht.Range("A30:K30").Style.NumberFormat.Format = "@";
                sht.Range("K30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //values
                sht.Range("A31:H31").Merge();
                sht.Range("A32:H36").Style.Font.FontSize = 12;
                sht.Range("A32:H36").Style.Font.FontName = "Tahoma";
                sht.Range("A32:H36").Style.NumberFormat.Format = "@";
                sht.Range("A32").Value = ds.Tables[0].Compute("Min(MinrollNo)", "") + "/" + ds.Tables[0].Compute("Max(Maxrollno)", "");
                sht.Range("B32").Value = ds.Tables[0].Rows[0]["Noofrolls"] + " Packages";
                sht.Range("B32:C32").Merge();
                sht.Range("E32").Value = ds.Tables[0].Rows[0]["descriptionofgoods"];
                sht.Range("E32:J36").Style.Alignment.WrapText = true;
                sht.Range("E32:J36").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("E32:J36").Merge();
                sht.Range("A37:J37").Merge();
                //            //**********************Details
                //********Hader
                sht.Range("A38").Value = "Roll No";
                sht.Range("A38:K38").Style.Font.FontSize = 12;
                sht.Range("A38:K38").Style.Font.FontName = "Tahoma";
                sht.Range("A38:K38").Style.NumberFormat.Format = "@";
                sht.Range("A38:K38").Style.Font.Bold = true;
                sht.Range("A38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B38").Value = "Quality";
                sht.Range("B38:C38").Merge();
                sht.Range("D38").Value = "ART.NO.";
                //sht.Range("D38:E38")..Merge();
                sht.Range("E38").Value = "COLOR";
                sht.Range("F38").Value = "SIZE(CM)";
                sht.Range("F38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("G38").Value = "Pcs/Roll";
                sht.Range("G38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H38").Value = "Total Rolls";
                sht.Range("H38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("I38").Value = "Total Pcs";
                sht.Range("J38").Value = "Area Sq. Mtr";
                sht.Range("K38").Value = "P.O.#";
                sht.Range("K38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A38:K38").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A38").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K38").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A38:K38").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //***********generate Loop
                i = 39;
                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                {
                    sht.Range("A" + i, "K" + i).Style.Font.FontSize = 12;
                    sht.Range("A" + i, "K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("A" + i, "I" + i).Style.NumberFormat.Format = "@";
                    sht.Range("K" + i).Style.Font.FontSize = 12;
                    sht.Range("K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("K" + i).Style.NumberFormat.Format = "@";
                    //pono
                    sht.Range("A" + i).Value = ds.Tables[0].Rows[ii]["Minrollno"] + "  " + ds.Tables[0].Rows[ii]["Maxrollno"];
                    sht.Range("A" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //Article Name
                    sht.Range("B" + i).Value = ds.Tables[0].Rows[ii]["Design"];
                    sht.Range("B" + i, "C" + i).Merge();
                    //Art No.
                    sht.Range("D" + i).Value = ds.Tables[0].Rows[ii]["articleno"];
                    //Colour
                    sht.Range("E" + i).Value = ds.Tables[0].Rows[ii]["Color"];
                    //Size
                    sht.Range("F" + i).Value = ds.Tables[0].Rows[ii]["width"] + "x" + ds.Tables[0].Rows[ii]["Length"];
                    sht.Range("F" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //pcs/roll
                    sht.Range("G" + i).SetValue(ds.Tables[0].Rows[ii]["pcsperroll"]);
                    sht.Range("G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("G" + i).Style.NumberFormat.Format = "@";
                    //Total rolls
                    sht.Range("H" + i).SetValue(ds.Tables[0].Rows[ii]["totalroll"]);
                    sht.Range("H" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("H" + i).Style.NumberFormat.Format = "@";
                    TotalRolls = TotalRolls + Convert.ToInt16(ds.Tables[0].Rows[ii]["totalroll"]);
                    //Qty
                    sht.Range("I" + i).SetValue(ds.Tables[0].Rows[ii]["Pcs"]);
                    sht.Range("I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[ii]["Pcs"]);
                    sht.Range("I" + i).Style.NumberFormat.Format = "@";
                    //Area
                    sht.Range("J" + i).SetValue(ds.Tables[0].Rows[ii]["Area"]);
                    sht.Range("J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                    Area = Decimal.Parse(Area.ToString()) + Decimal.Parse(ds.Tables[0].Rows[ii]["Area"].ToString());
                    //
                    //var _with27 = sht.Range("I" + i);

                    //_with27.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
                    //_with27.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
                    //PO
                    sht.Range("K" + i).Value = ds.Tables[0].Rows[ii]["Pono"];
                    //
                    sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //Borders
                    sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    i = i + 1;
                }
                //
                sht.Range("A" + i + ":A" + (i + 7)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 7)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //End Of loop
                i = i + 7;
                //*************** 

                //
                sht.Range("D" + i).Value = "Packing List Total:";
                sht.Range("D" + i, "F" + i).Style.Font.FontSize = 12;
                sht.Range("D" + i, "F" + i).Style.Font.FontName = "Tahoma";
                sht.Range("D" + i, "F" + i).Style.NumberFormat.Format = "@";
                sht.Range("D" + i, "F" + i).Style.Font.Bold = true;
                sht.Range("D" + i + ":F" + i).Merge();
                sht.Range("D" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //Total Rolls
                sht.Range("H" + i).Value = TotalRolls;
                sht.Range("H" + i).Style.Font.FontSize = 12;
                sht.Range("H" + i).Style.Font.FontName = "Tahoma";
                sht.Range("H" + i).Style.NumberFormat.Format = "@";
                sht.Range("H" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //Total Pcs
                sht.Range("I" + i).Value = Pcs;
                sht.Range("I" + i).Style.Font.FontSize = 12;
                sht.Range("I" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i).Style.NumberFormat.Format = "@";
                sht.Range("I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //Area
                sht.Range("J" + i).Value = Area;
                sht.Range("J" + i).Style.Font.FontSize = 12;
                sht.Range("J" + i).Style.Font.FontName = "Tahoma";
                sht.Range("J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                //
                var _with21 = sht.Range("A" + i + ":K" + i);
                _with21.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                _with21.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                sht.Range("A" + i + ":A" + (i + 10)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 10)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //*************** 
                i = i + 2;
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {

                    sht.Range("A" + i, "K" + i).Style.Font.FontSize = 12;
                    sht.Range("A" + i, "K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("A" + i, "I" + i).Style.NumberFormat.Format = "@";
                    sht.Range("K" + i).Style.Font.FontSize = 12;
                    sht.Range("K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("K" + i).Style.NumberFormat.Format = "@";

                    //Art No.
                    sht.Range("D" + i).Value = ds.Tables[0].Rows[j]["articleno"];

                    sht.Range("E" + i, "G" + i).Merge();
                    sht.Range("E" + i).Value = "(" + (Convert.ToDecimal(ds.Tables[0].Rows[j]["Pcs"]) / Convert.ToDecimal(ds.Tables[0].Rows[j]["pcsperroll"])) + " " + "Pallet x" + " " + ds.Tables[0].Rows[j]["pcsperroll"] + "Pcs/Pallet)";

                    ////pcs/roll
                    //sht.Range("G" + i).SetValue(ds.Tables[0].Rows[j]["pcsperroll"]);
                    //sht.Range("G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //sht.Range("G" + i).Style.NumberFormat.Format = "@";                  
                    ////Qty
                    //sht.Range("I" + i).SetValue(ds.Tables[0].Rows[j]["Pcs"]);
                    //sht.Range("I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[j]["Pcs"]);
                    //sht.Range("I" + i).Style.NumberFormat.Format = "@";                   
                    //Borders
                    sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    i = i + 1;
                }
                //
                sht.Range("A" + i + ":A" + (i + 7)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 7)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //*************** 

                //**********Total
                i = i + 10;
                //i = 63;
                sht.Range("A" + i).Value = "TOTAL ROLLS";
                sht.Range("A" + (i + 1)).Value = "TOTAL PCS";
                sht.Range("A" + (i + 2)).Value = "TOTAL GR. WT";
                sht.Range("A" + (i + 3)).Value = "TOTAL NT. WT";
                sht.Range("A" + (i + 4)).Value = "TOTAL SQ.MTR";
                sht.Range("A" + (i + 5)).Value = "TOTAL VOLUME";
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":A" + (i + 5)).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontName = "Courier New";
                //value                
                sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontSize = 12;
                sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontName = "Tahoma";
                sht.Range("C" + i + ":C" + (i + 5)).Style.NumberFormat.Format = "@";


                sht.Range("C" + i).SetValue(ds.Tables[0].Rows[0]["Noofrolls"]);
                sht.Range("C" + (i + 1)).SetValue(Pcs);
                sht.Range("C" + (i + 2)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["grosswt"]) + " Kgs.");
                sht.Range("C" + (i + 3)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["netwt"]) + " Kgs.");
                sht.Range("C" + (i + 4)).SetValue(String.Format("{0:#,0.00}", Area) + " Sq.Mtr");
                sht.Range("C" + (i + 5)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["Volume"]) + " CBM");
                //
                sht.Range("A" + i + ":A" + (i + 5)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 5)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //***********Sig and date
                i = i + 6;
                //i = 70;
                sht.Range("A" + i + ":H" + i).Merge();
                sht.Range("I" + i).Value = "Signature/Date";
                sht.Range("I" + i).Style.Font.FontSize = 12;
                sht.Range("I" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i).Style.NumberFormat.Format = "@";

                sht.Range("K" + i).Value = "For " + ds.Tables[0].Rows[0]["companyname"];
                sht.Range("J" + i + ":K" + i).Style.Font.FontSize = 12;
                sht.Range("J" + i + ":K" + i).Style.Font.FontName = "Tahoma";
                sht.Range("J" + i + ":K" + i).Style.NumberFormat.Format = "@";
                sht.Range("J" + i + ":K" + i).Style.Font.Bold = true;
                sht.Range("J" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("J" + i + ":K" + i).Style.Font.FontSize = 9;
                //sht.Range("J" + i + ":K" + i).Merge();              
                sht.Range("I" + i + ":K" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("I" + i + ":I" + (i + 3)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 3)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //**************Declaration
                //
                sht.Range("A" + i + ":A" + (i + 2)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                i = i + 1;
                //i = 71;
                sht.Range("A" + i).Value = "Declaration";
                sht.Range("A" + i + ":B" + i).Merge();
                sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":B" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                //value
                i = i + 1;
                sht.Range("A" + i).Value = "We declare that this invoice shows the actual price of the";
                sht.Range("A" + i + ":F" + i).Merge();
                sht.Range("A" + i + ":F" + i).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":F" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":F" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                i = i + 1;
                sht.Range("A" + i).Value = "goods described and that all particulars are true and correct";
                sht.Range("A" + i + ":F" + i).Merge();
                sht.Range("A" + i + ":F" + i).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":F" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":F" + i).Style.NumberFormat.Format = "@";

                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //**********

                sht.Range("I" + i + ":J" + i).Merge();
                sht.Range("I" + i + ":J" + i).Value = ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("I" + i + ":J" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i + ":J" + i).Style.Font.FontSize = 12;
                sht.Range("I" + i + ":J" + i).Style.NumberFormat.Format = "dd/MMM/yyyy";
                sht.Range("I" + i + ":J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("K" + i).Value = "Auth Sign";
                sht.Range("K" + i).Style.Font.FontSize = 12;
                sht.Range("K" + i).Style.Font.FontName = "Tahoma";
                sht.Range("K" + i).Style.NumberFormat.Format = "@";
                sht.Range("K" + i).Style.Font.Bold = true;
                sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //
                var _with39 = sht.Range("I" + i);
                _with39.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //
                sht.Range("A2:A10").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A11:A23").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i + ":K" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A1:K1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //sht.Range("A2:A10").Style.Border.LeftBorder = XLBorderStyleValues.Thin;                
                //sht.Range("F2:F10").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //            //************************
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
                // _with4.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //_with4.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
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

                //   sht.Range("G25:I25").Style.Border.BottomBorder = XLBorderStyleValues.None;

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
                sht.Range("G28:K28").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A30:A37").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K30:K37").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //*******************Save
                string Fileextension = "xlsx";
                string filename = "PackingList -" + validateFilename(ds.Tables[0].Rows[0]["TinvoiceNo"].ToString()) + "." + Fileextension + "";
                Path = Server.MapPath("~/Packingexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
                //*****
                lblmsg.Text = "Packing Excel Format downloaded successfully.";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "AltR2", "alert('No Record Found to Export')", true);
            }

        }
        catch (Exception)
        {

            throw;
        }
    }


    protected void Exporttoexcel_Packing_Kaysons_new()
    {
        lblmsg.Text = "Wait For Excel File to Download";
        int i = 0, Pcs = 0, TotalRolls = 0;
        Decimal Area = 0;
        string Path = "";
        if (!Directory.Exists(Server.MapPath("~/PackingExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/PackingExcel/"));
        }
        try
        {
            string str = @"select *,VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1) as Pcsperroll,VD.Pcs/(VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1)) as TotalRoll From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue + " order by MinrollNo";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                String ECIS = string.Empty, artno = string.Empty;
                Decimal TotalNetWT = 0, TotalGrossWT = 0;
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("PackingList");
                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                sht.PageSetup.AdjustTo(95);
                sht.PageSetup.FitToPages(1, 1);
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                sht.PageSetup.VerticalDpi = 300;
                sht.PageSetup.HorizontalDpi = 300;
                //
                sht.PageSetup.Margins.Top = 0.1;
                sht.PageSetup.Margins.Left = 0.1;
                sht.PageSetup.Margins.Right = 0;
                sht.PageSetup.Margins.Bottom = 0.01;
                sht.PageSetup.Margins.Header = 0.1;
                sht.PageSetup.Margins.Footer = 0.1;
                //sht.pagesetup.centerhorizontally = true;
                //sht.pagesetup.centervertically = true;
                sht.PageSetup.SetScaleHFWithDocument();
                //************
                sht.Column("A").Width = 20.71;
                sht.Column("B").Width = 8.86;
                sht.Column("C").Width = 18.00;
                sht.Column("D").Width = 15.29;
                sht.Column("E").Width = 10.43;
                sht.Column("F").Width = 15.86;
                sht.Column("G").Width = 12.86;
                sht.Column("H").Width = 18.29;
                sht.Column("I").Width = 20.71;
                sht.Column("J").Width = 20.29;
                sht.Column("K").Width = 23.14;
                //************
                sht.Row(1).Height = 15.75;
                //*****Header                
                sht.Cell("A2").Value = "PACKING LIST";
                sht.Range("A2:K2").Style.Font.FontName = "Arial";
                sht.Range("A2:K2").Style.Font.FontSize = 15;
                sht.Range("A2:K2").Style.Font.Bold = true;
                sht.Range("A2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:K2").Merge();
                //***************
                //sht.Cell("A2").Value = @"""SUPPLY MEANT FOR EXPORT UNDER LETTER OF UNDERTAKING NO-21/2017 Dt.07.07.2017. WITHOUT PAYMENT OF INTEGRATED TAX""";
                //sht.Range("A2:K2").Style.Font.FontName = "Arial";
                //sht.Range("A2:K2").Style.Font.FontSize = 12;
                //sht.Range("A2:K2").Style.Font.Bold = true;
                //sht.Range("A2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Range("A2:K2").Merge();
                using (var a = sht.Range("A2:K2"))
                {
                    //a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //********Exporter
                sht.Range("A3").Value = "Exporter";
                sht.Range("A3:F3").Style.Font.FontName = "Arial";
                sht.Range("A3:F3").Style.Font.FontSize = 12;
                sht.Range("A3:F3").Merge();
                //*****************

                //CompanyName
                sht.Range("A4").Value = ds.Tables[0].Rows[0]["companyName"];
                sht.Range("A4:F4").Style.Font.FontName = "Arial";
                sht.Range("A4:F4").Style.Font.FontSize = 12;
                sht.Range("A4:F4").Style.Font.Bold = true;
                sht.Range("A4:F4").Merge();
                //Address
                sht.Range("A5").Value = ds.Tables[0].Rows[0]["compaddr1"];
                sht.Range("A5:F5").Style.Font.FontName = "Arial";
                sht.Range("A5:F5").Style.Font.FontSize = 12;
                sht.Range("A5:F5").Merge();
                //address2
                sht.Range("A6").Value = ds.Tables[0].Rows[0]["compaddr2"] + "," + ds.Tables[0].Rows[0]["compaddr3"];
                sht.Range("A6:F6").Style.Font.FontName = "Arial";
                sht.Range("A6:F6").Style.Font.FontSize = 12;
                sht.Range("A6:F6").Merge();
                //TiN No
                sht.Range("A7").Value = "GSTIN#" + ds.Tables[0].Rows[0]["gstno"];
                sht.Range("A7:F7").Style.Font.FontName = "Arial";
                sht.Range("A7:F7").Style.Font.FontSize = 12;
                sht.Range("A7:F7").Style.Font.Bold = true;
                sht.Range("A7:F7").Merge();

                sht.Range("A9").Value = "STATE:" + "Uttar Pradesh";
                sht.Range("A9:F9").Style.Font.FontName = "Arial";
                sht.Range("A9:F9").Style.Font.FontSize = 12;
                sht.Range("A9:F9").Style.Font.Bold = true;
                sht.Range("A9:F9").Merge();

                sht.Range("A11").Value = "PAN:" + ds.Tables[0].Rows[0]["panno"];
                sht.Range("A11:F11").Style.Font.FontName = "Arial";
                sht.Range("A11:F11").Style.Font.FontSize = 12;
                sht.Range("A11:F11").Style.Font.Bold = true;
                sht.Range("A11:F11").Merge();
                //**********INvoiceNodate
                sht.Range("G3").Value = "Invoice No.";
                sht.Range("G3:H3").Style.Font.FontName = "Arial";
                sht.Range("G3:H3").Style.Font.FontSize = 12;
                sht.Range("G3:H3").Merge();
                //value
                sht.Range("I3").Value = ds.Tables[0].Rows[0]["TInvoiceNo"];
                sht.Range("I3:I3").Style.Font.FontName = "Arial";
                sht.Range("I3:I3").Style.Font.FontSize = 12;
                sht.Range("I3:I3").Merge();
                sht.Range("I3:I3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("J3").Value = " Dated : " + ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("J3:K3").Style.Font.FontName = "Arial";
                sht.Range("J3:K3").Style.Font.FontSize = 12;
                sht.Range("J3:K3").Merge();
                //sht.Range("G3").Value = "Invoice No.";
                //sht.Range("G3:K3").Style.Font.FontName = "Arial";
                //sht.Range("G3:K3").Style.Font.FontSize = 12;
                //sht.Range("G3:K3").Merge();
                ////value
                //sht.Range("G4").Value = ds.Tables[0].Rows[0]["TInvoiceNo"] + " Dated " + ds.Tables[0].Rows[0]["InvoiceDate"];
                //sht.Range("G4:K4").Style.Font.FontName = "Arial";
                //sht.Range("G4:K4").Style.Font.FontSize = 12;
                //sht.Range("G4:K4").Merge();
                //sht.Range("G4", "K4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //*************IE Code
                sht.Range("G5").Value = "Buyer's Order No. : As Mentioned Below ";
                sht.Range("G5:I5").Style.Font.FontName = "Arial";
                sht.Range("G5:I5").Style.Font.FontSize = 12;
                sht.Range("G5:I5").Merge();
                //value
                //sht.Range("G6").Value = ds.Tables[0].Rows[0]["TorderNo"];
                //sht.Range("G6:I6").Style.Font.FontName = "Arial";
                //sht.Range("G6:I6").Style.Font.FontSize = 12;
                //sht.Range("G6:I6").Merge();
                //sht.Range("G6", "I6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //***********GRI Form No
                //sht.Range("J5").Value = "REX No.";
                //sht.Range("J5:K5").Style.Font.FontName = "Arial";
                //sht.Range("J5:K5").Style.Font.FontSize = 12;
                //sht.Range("J5:K5").Merge();
                //// value
                //sht.Range("J6").Value = "INREX1510001883EC025";
                //sht.Range("J6:K6").Style.Font.FontName = "Arial";
                //sht.Range("J6:K6").Style.Font.FontSize = 12;
                //sht.Range("J6:K6").Merge();
                //sht.Range("J6", "K6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //*************Buyer's Order No and Date
                //sht.Range("G7").Value = "Buyer's Order No. / Date";
                //sht.Range("G7:K8").Style.Font.FontName = "Arial";
                //sht.Range("G7:K8").Style.Font.FontSize = 12;
                //sht.Range("G7:K7").Merge();
                //value
                //sht.Range("G8").SetValue(ds.Tables[0].Rows[0]["TorderNo"]);
                //sht.Range("G8:K8").Style.Font.FontName = "Arial";
                //sht.Range("G8:K8").Style.Font.FontSize = 12;
                //sht.Range("G8:K8").Merge();
                //sht.Range("G8", "K8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range("G8").Value = "SUPPLIER NO.: 21899";
                sht.Range("G8:I8").Style.Font.FontName = "Arial";
                sht.Range("G8:I8").Style.Font.FontSize = 12;
                sht.Range("G8:I8").Merge();
                //value
                //sht.Range("I8").Value = ds.Tables[0].Rows[0]["TInvoiceNo"];
                //sht.Range("I8:I8").Style.Font.FontName = "Arial";
                //sht.Range("I8:I8").Style.Font.FontSize = 12;
                //sht.Range("I8:I8").Merge();
                //sht.Range("I8:I8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("J8").Value = "DESTN. CODE :" + ds.Tables[0].Rows[0]["DestCode"];
                sht.Range("J8:K8").Style.Font.FontName = "Arial";
                sht.Range("J8:K8").Style.Font.FontSize = 12;
                sht.Range("J8:K8").Merge();
                //**************Other Reference(s)
                // sht.Range("G9").Value = "Other Reference(s)";
                sht.Range("G10").Value = "CONSIGNMENT NO.:" + ds.Tables[0].Rows[0]["otherref"];
                //  sht.Range("G11").Value = "SUPPLIER NO.: 21899";
                sht.Range("G9:G11").Style.Font.FontName = "Arial";
                sht.Range("G9:G11").Style.Font.FontSize = 12;
                sht.Range("G9:K9").Merge();
                sht.Range("G10:K10").Merge();
                sht.Range("G11:K11").Merge();
                //*************Consignee
                sht.Range("A12").Value = "Consignee";
                sht.Range("C12").Value = ds.Tables[0].Rows[0]["consignee_dt"];
                sht.Range("A12:B12").Style.Font.FontName = "Arial";
                sht.Range("A12:B12").Style.Font.FontSize = 12;

                sht.Range("C12:F12").Style.Font.FontName = "Arial";
                sht.Range("C12:F12").Style.Font.FontSize = 12;
                sht.Range("C12:F12").Style.Font.Bold = true;

                sht.Range("A12:B12").Merge();
                sht.Range("C12:F12").Merge();
                //value
                sht.Range("A13").Value = ds.Tables[0].Rows[0]["consignee"];
                sht.Range("A13:F13").Style.Font.FontName = "Arial";
                sht.Range("A13:F13").Style.Font.FontSize = 12;
                sht.Range("A13:F13").Style.Font.Bold = true;
                sht.Range("A13:F13").Merge();
                //**
                sht.Range("A14").Value = ds.Tables[0].Rows[0]["consignee_address"];
                sht.Range("A14:F17").Style.Font.FontName = "Arial";
                sht.Range("A14:F17").Style.Font.FontSize = 12;
                sht.Range("A14", "F17").Style.Alignment.WrapText = true;
                sht.Range("A14", "F17").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A14:F17").Merge();
                //***********Notify
                sht.Range("A18").Value = "Notify Party";
                sht.Range("C18").Value = ds.Tables[0].Rows[0]["Notifyparty_dt"];
                sht.Range("A18:B18").Style.Font.FontName = "Arial";
                sht.Range("A18:B18").Style.Font.FontSize = 12;

                sht.Range("A18:B18").Style.Font.SetUnderline();
                sht.Range("C18:F18").Style.Font.FontName = "Arial";
                sht.Range("C18:F18").Style.Font.FontSize = 12;
                sht.Range("C18:F18").Style.Font.Bold = true;

                sht.Range("A18:B18").Merge();
                sht.Range("C18:F18").Merge();
                //value
                sht.Range("A19").Value = ds.Tables[0].Rows[0]["Notifyparty"];
                sht.Range("A19:F19").Style.Font.FontName = "Arial";
                sht.Range("A19:F19").Style.Font.FontSize = 12;
                sht.Range("A19:F19").Style.Font.Bold = true;
                sht.Range("A19:F19").Merge();
                //
                sht.Range("A20").Value = ds.Tables[0].Rows[0]["notifyparty_address"];
                sht.Range("A20:F24").Style.Font.FontName = "Arial";
                sht.Range("A20:F24").Style.Font.FontSize = 12;
                sht.Range("A20", "F24").Style.Alignment.WrapText = true;
                sht.Range("A20", "F24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A20:F24").Merge();
                //***********Receiver address
                //sht.Range("G12").Value = "Receiver Address";
                //sht.Range("G12:I12").Style.Font.FontName = "Arial";
                //sht.Range("G12:I12").Style.Font.FontSize = 12;
                //sht.Range("G12:I12").Merge();
                ////values     2
                //sht.Range("J12").Value = ds.Tables[0].Rows[0]["Destcode"];
                //sht.Range("J12:K12").Style.Font.FontName = "Arial";
                //sht.Range("J12:K12").Style.Font.FontSize = 12;
                //sht.Range("J12:K12").Style.Font.Bold = true;
                //sht.Range("J12:K12").Merge();
                ////****** 1.
                //sht.Range("G13").Value = "1. " + ds.Tables[0].Rows[0]["Receiver"];
                //sht.Range("G13:K13").Style.Font.FontName = "Arial";
                //sht.Range("G13:K13").Style.Font.FontSize = 12;
                //sht.Range("G13:K13").Style.Font.Bold = true;
                //sht.Range("G13:K13").Merge();
                ////*
                //sht.Range("G14").Value = ds.Tables[0].Rows[0]["Receiver_address"];
                //sht.Range("G14:K16").Style.Font.FontName = "Arial";
                //sht.Range("G14:K16").Style.Font.FontSize = 12;
                //sht.Range("G14:K16").Style.Alignment.WrapText = true;
                //sht.Range("G14:K16").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                //sht.Range("G14:K16").Merge();
                sht.Range("G12").Value = "Buyer (If other than Consignee)";
                sht.Range("G12:K12").Style.Font.FontName = "Arial";
                sht.Range("G12:K12").Style.Font.FontSize = 12;
                sht.Range("G12:K12").Merge();

                sht.Range("G13").Value = "3." + ds.Tables[0].Rows[0]["buyer_otherthanconsignee"];
                sht.Range("G13:K13").Style.Font.FontName = "Arial";
                sht.Range("G13:K13").Style.Font.FontSize = 12;
                sht.Range("G13:K13").Merge();
                //*
                sht.Range("G14").Value = ds.Tables[0].Rows[0]["otherthanconsignee_address"];
                sht.Range("G14:K16").Style.Font.FontName = "Arial";
                sht.Range("G14:K16").Style.Font.FontSize = 12;
                sht.Range("G14:K16").Style.Alignment.WrapText = true;
                sht.Range("G14:K16").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("G14:K16").Merge();

                //****** 2.
                sht.Range("G18").Value = "2.Paying Agent : " + ds.Tables[0].Rows[0]["payingagent"];
                sht.Range("G18:K18").Style.Font.FontName = "Arial";
                sht.Range("G18:K18").Style.Font.FontSize = 12;
                sht.Range("G18:K18").Style.Font.Bold = true;
                sht.Range("G18:K18").Merge();
                //*
                sht.Range("I19").Value = ds.Tables[0].Rows[0]["payingagent_address"];
                sht.Range("I19:K21").Style.Font.FontName = "Arial";
                sht.Range("I19:K21").Style.Font.FontSize = 12;
                sht.Range("I19:K21").Style.Alignment.WrapText = true;
                sht.Range("I19:K21").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("I19:K21").Merge();
                //*******3.
                //sht.Range("G21").Value = "Buyer (If other than Consignee)";
                //sht.Range("G21:K21").Style.Font.FontName = "Arial";
                //sht.Range("G21:K21").Style.Font.FontSize = 12;
                //sht.Range("G21:K21").Merge();

                //sht.Range("G22").Value = "3." + ds.Tables[0].Rows[0]["buyer_otherthanconsignee"];
                //sht.Range("G22:K22").Style.Font.FontName = "Arial";
                //sht.Range("G22:K22").Style.Font.FontSize = 12;
                //sht.Range("G22:K22").Merge();
                ////*
                //sht.Range("G23").Value = ds.Tables[0].Rows[0]["otherthanconsignee_address"];
                //sht.Range("G23:K25").Style.Font.FontName = "Arial";
                //sht.Range("G23:K25").Style.Font.FontSize = 12;
                //sht.Range("G23:K25").Style.Alignment.WrapText = true;
                //sht.Range("G23:K25").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                //sht.Range("G23:K25").Merge();
                //***********Pre-carriage By
                sht.Range("A25").Value = "Pre-Carriage By";
                sht.Range("A25:C25").Style.Font.FontName = "Arial";
                sht.Range("A25:C25").Style.Font.FontSize = 12;
                sht.Range("A25:C25").Merge();
                //value
                sht.Range("A26").Value = ds.Tables[0].Rows[0]["Pre_carriageby"];
                sht.Range("A26:C26").Style.Font.FontName = "Arial";
                sht.Range("A26:C26").Style.Font.FontSize = 12;
                sht.Range("A26:C26").Merge();
                sht.Range("A26:C26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Place of Receipt by Pre-Carrier
                sht.Range("D25").Value = "Place of Receipt by Pre-Carrier";
                sht.Range("D25:F25").Style.Font.FontName = "Arial";
                sht.Range("D25:F25").Style.Font.FontSize = 12;
                sht.Range("D25:F25").Merge();
                //value
                sht.Range("D26").Value = ds.Tables[0].Rows[0]["receiptby"];
                sht.Range("D26:F26").Style.Font.FontName = "Arial";
                sht.Range("D26:F26").Style.Font.FontSize = 12;
                sht.Range("D26:F26").Merge();
                sht.Range("D26:F26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Vessel/Flight No
                sht.Range("A27").Value = "Vessel/Flight No";
                sht.Range("A27:C27").Style.Font.FontName = "Arial";
                sht.Range("A27:C27").Style.Font.FontSize = 12;
                sht.Range("A27:C27").Merge();
                //value
                sht.Range("A28").Value = ds.Tables[0].Rows[0]["vessel_flightno"];
                sht.Range("A28:C28").Style.Font.FontName = "Arial";
                sht.Range("A28:C28").Style.Font.FontSize = 12;
                sht.Range("A28:C28").Merge();
                sht.Range("A28:C28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Loading
                sht.Range("D27").Value = "Port of Loading";
                sht.Range("D27:F27").Style.Font.FontName = "Arial";
                sht.Range("D27:F27").Style.Font.FontSize = 12;
                sht.Range("D27:F27").Merge();
                //value
                sht.Range("D28").Value = ds.Tables[0].Rows[0]["portload"];
                sht.Range("D28:F28").Style.Font.FontName = "Arial";
                sht.Range("D28:F28").Style.Font.FontSize = 12;
                sht.Range("D28:F28").Merge();
                sht.Range("D28:F28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Discharge
                sht.Range("A29").Value = "Port of Discharge";
                sht.Range("A29:C29").Style.Font.FontName = "Arial";
                sht.Range("A29:C29").Style.Font.FontSize = 12;
                sht.Range("A29:C29").Merge();
                //value
                sht.Range("A30").Value = ds.Tables[0].Rows[0]["portunload"];
                sht.Range("A30:C30").Style.Font.FontName = "Arial";
                sht.Range("A30:C30").Style.Font.FontSize = 12;
                sht.Range("A30:C30").Merge();
                sht.Range("A30:C30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Final Destination
                sht.Range("D29").Value = "Final Destination";
                sht.Range("D29:F29").Style.Font.FontName = "Arial";
                sht.Range("D29:F29").Style.Font.FontSize = 12;
                sht.Range("D29:F29").Merge();
                //value
                sht.Range("D30").Value = ds.Tables[0].Rows[0]["Destinationadd"];
                sht.Range("D30:F30").Style.Font.FontName = "Arial";
                sht.Range("D30:F30").Style.Font.FontSize = 12;
                sht.Range("D30:F30").Merge();
                sht.Range("D30:F30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Origin of Goods
                sht.Range("G25").Value = "Country of Origin of Goods";
                sht.Range("G25:I25").Style.Font.FontName = "Arial";
                sht.Range("G25:I25").Style.Font.FontSize = 12;
                sht.Range("G25:I25").Merge();
                //value
                sht.Range("G26").Value = ds.Tables[0].Rows[0]["countryoforigin"];
                sht.Range("G26:I26").Style.Font.FontName = "Arial";
                sht.Range("G26:I26").Style.Font.FontSize = 12;
                sht.Range("G26:I26").Merge();
                sht.Range("G26:I26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Final Destination
                sht.Range("J25").Value = "Country of Final Destination";
                sht.Range("J25:K25").Style.Font.FontName = "Arial";
                sht.Range("J25:K25").Style.Font.FontSize = 12;
                sht.Range("J25:K25").Merge();
                //value
                sht.Range("J26").Value = ds.Tables[0].Rows[0]["countryoffinaldest"];
                sht.Range("J26:K26").Style.Font.FontName = "Arial";
                sht.Range("J26:K26").Style.Font.FontSize = 12;
                sht.Range("J26:K26").Style.NumberFormat.Format = "@";
                sht.Range("J26:K26").Merge();
                sht.Range("J26:K26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //***********Terms of Delivery and Payment
                sht.Range("G27").Value = "Terms of Delivery and Payment";
                sht.Range("G27:K27").Style.Font.FontName = "Arial";
                sht.Range("G27:K27").Style.Font.FontSize = 12;
                sht.Range("G27:K27").Merge();
                //value
                sht.Range("H28").Value = ds.Tables[0].Rows[0]["Deliveryterm"];
                sht.Range("H28:K29").Style.Font.FontName = "Arial";
                sht.Range("H28:K29").Style.Font.FontSize = 12;
                sht.Range("H28:K29").Style.NumberFormat.Format = "@";
                sht.Range("H28:K29").Style.Alignment.WrapText = true;
                sht.Range("H28:K29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("H28:K29").Merge();
                //*****************Nos
                string descriptionitem = string.Empty;
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    //descriptionitem += Convert.ToString(ds.Tables[0].Rows[j]["category"]) + "" + Convert.ToString(ds.Tables[0].Rows[j]["quality"]) + Convert.ToString(ds.Tables[0].Rows[j]["design"]) + "" + Convert.ToString(ds.Tables[0].Rows[j]["contents1"]) + Convert.ToString(ds.Tables[0].Rows[j]["articleno"])+"/n";
                    descriptionitem += Convert.ToString(ds.Tables[0].Rows[j]["descriptionofgoods"]) + "(" + Convert.ToString(ds.Tables[0].Rows[j]["contents1"]) + ")" + "-" + Convert.ToString(ds.Tables[0].Rows[j]["articleno"]) + ",";



                    j = j + 1;
                }
                sht.Range("A31").Value = "Marks Nos./ Container No";
                sht.Range("A31:B31").Merge();
                sht.Range("C31").Value = "No. of Packages";
                sht.Range("C31:D31").Merge();
                sht.Range("E31").Value = "Description of Goods";
                sht.Range("E31:K31").Merge();
                //sht.Range("I31").Value = "Quantity";
                //sht.Range("J31").Value = "Rate";
                //sht.Range("k31").Value = "Amount";
                sht.Range("A31:K31").Style.Font.FontName = "Arial";
                sht.Range("A31:K31").Style.Font.FontSize = 12;
                sht.Range("A31:K31").Style.NumberFormat.Format = "@";
                sht.Range("K31").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //values
                sht.Range("A32:H32").Merge();
                sht.Range("A33:H37").Style.Font.FontName = "Arial";
                sht.Range("A33:H37").Style.Font.FontSize = 12;
                sht.Range("A33:H37").Style.NumberFormat.Format = "@";

                sht.Range("A33").SetValue(ds.Tables[0].Compute("Min(MinrollNo)", "") + "/" + ds.Tables[0].Compute("Max(Maxrollno)", ""));
                sht.Range("A33:B33").Merge();
                sht.Range("C33").Value = ds.Tables[0].Rows[0]["Noofrolls"] + " Packages";
                sht.Range("C33:D33").Merge();
                sht.Range("E33").Value = descriptionitem;
                sht.Range("E33:K37").Style.Alignment.WrapText = true;
                sht.Range("E33:K37").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("E33:K37").Merge();
                sht.Range("E38:K38").Merge();
                //********Details
                sht.Range("A39").Value = "P.O.NO.";
                sht.Range("A39:A40").Merge();
                sht.Range("A39:A40").Style.Alignment.WrapText = true;
                sht.Range("A39:K39").Style.Font.FontName = "Arial";
                sht.Range("A39:K39").Style.Font.FontSize = 12;
                sht.Range("A39:K39").Style.Font.Bold = true;
                sht.Range("A39:K39").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range("B39").Value = "Pallet";
                sht.Range("B39:B40").Merge();
                sht.Range("B39:B40").Style.Alignment.WrapText = true;
                // sht.Range("B39:C39").Merge();
                sht.Range("C39").Value = "PCS/Pallet";
                sht.Range("C39:C40").Merge();
                sht.Range("C39:C40").Style.Alignment.WrapText = true;
                sht.Range("D39").Value = "Art. No.";
                sht.Range("D39:D40").Merge();
                sht.Range("D39:D40").Style.Alignment.WrapText = true;
                //sht.Range("B39:C39").Merge();
                sht.Range("E39").Value = "DESCRIPTION, COLOUR & SIZE";
                sht.Range("E39:F40").Merge();
                // sht.Range("A39:A40").Merge();
                sht.Range("E39:F40").Style.Alignment.WrapText = true;
                //sht.Range("F39").Value = "COLOR";
                //sht.Range("G39").Value = "SIZE(CM)";
                sht.Range("G39").Value = "QTY";
                sht.Range("G39:G40").Merge();
                sht.Range("G39:G40").Style.Alignment.WrapText = true;
                sht.Range("H39").Value = "TOTAL NT.WT";
                sht.Range("H39:H40").Merge();
                sht.Range("H39:H40").Style.Alignment.WrapText = true;
                sht.Range("I39").Value = "NT.WT";
                sht.Range("I39:I40").Merge();
                sht.Range("I39:I40").Style.Alignment.WrapText = true;
                sht.Range("J39").Value = "TOTAL GR.WT";
                sht.Range("J39:J40").Merge();
                sht.Range("J39:J40").Style.Alignment.WrapText = true;
                sht.Range("K39").Value = "GR.WT";
                sht.Range("K39:K40").Merge();
                sht.Range("K39:K40").Style.Alignment.WrapText = true;
                //sht.Range("K39").Value = "AMOUNT FCA/" + ds.Tables[0].Rows[0]["CIF"];
                sht.Range("K39").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //***********generate Loop
                i = 41;
                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                {
                    sht.Range("A" + i, "K" + (i + 1)).Style.Font.FontSize = 12;
                    sht.Range("A" + i, "K" + (i + 1)).Style.Font.FontName = "Arial";
                    sht.Range("A" + i, "I" + (i + 1)).Style.NumberFormat.Format = "@";
                    sht.Range("K" + (i + 1)).Style.Font.FontSize = 12;
                    sht.Range("K" + (i + 1)).Style.Font.FontName = "Arial";
                    sht.Range("K" + (i + 1)).Style.NumberFormat.Format = "@";
                    //pono
                    sht.Range("A" + i).Value = ds.Tables[0].Rows[ii]["Pono"];
                    sht.Range("A" + i + ":A" + (i + 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A" + i + ":A" + (i + 1)).Merge();
                    //Article Name
                    sht.Range("B" + i).Value = ds.Tables[0].Rows[ii]["Minrollno"] + "  " + ds.Tables[0].Rows[ii]["Maxrollno"] + "(" + ds.Tables[0].Rows[ii]["totalroll"] + " Pallet)";
                    sht.Range("B" + i + ":B" + (i + 1)).Merge();

                    //sht.Range("B" + i, "C" + i).Merge();
                    sht.Range("C" + i).Value = ds.Tables[0].Rows[ii]["pcsperroll"];
                    sht.Range("C" + i + ":C" + (i + 1)).Merge();
                    sht.Range("C" + i + ":C" + (i + 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //Art No.

                    sht.Range("D" + i).Value = ds.Tables[0].Rows[ii]["articleno"];
                    sht.Range("D" + i + ":D" + (i + 1)).Merge();
                    sht.Range("D" + i + ":D" + (i + 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //Colour
                    sht.Range("E" + i).Value = ds.Tables[0].Rows[ii]["design"] + "" + ds.Tables[0].Rows[ii]["Color"] + " " + ds.Tables[0].Rows[ii]["width"] + "x" + ds.Tables[0].Rows[ii]["Length"];
                    sht.Range("E" + i, "F" + (i + 1)).Merge();
                    //Size
                    sht.Range("G" + i).Value = ds.Tables[0].Rows[ii]["Pcs"];// ds.Tables[0].Rows[ii]["width"] + "x" + ds.Tables[0].Rows[ii]["Length"];
                    sht.Range("G" + i + ":G" + (i + 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("G" + i + ":G" + (i + 1)).Merge();
                    //pcs/roll
                    sht.Range("H" + i).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[ii]["netwt"]) * Convert.ToInt16(ds.Tables[0].Rows[ii]["totalroll"]));
                    sht.Range("H" + i + ":H" + (i + 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("H" + i + ":H" + (i + 1)).Style.NumberFormat.Format = "@";
                    sht.Range("H" + i + ":H" + (i + 1)).Merge();

                    TotalNetWT += TotalNetWT + Convert.ToDecimal(ds.Tables[0].Rows[ii]["netwt"]) * Convert.ToInt16(ds.Tables[0].Rows[ii]["totalroll"]);
                    //Total rolls
                    sht.Range("I" + i).SetValue(ds.Tables[0].Rows[ii]["netwt"]);
                    sht.Range("I" + i + ":I" + (i + 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("I" + i + ":I" + (i + 1)).Style.NumberFormat.Format = "@";
                    sht.Range("I" + i + ":I" + (i + 1)).Merge();
                    TotalRolls = TotalRolls + Convert.ToInt16(ds.Tables[0].Rows[ii]["totalroll"]);
                    //Qty
                    sht.Range("J" + i).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[ii]["grosswt"]) * Convert.ToInt16(ds.Tables[0].Rows[ii]["totalroll"]));
                    sht.Range("J" + i + ":J" + (i + 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[ii]["Pcs"]);
                    sht.Range("J" + i + ":J" + (i + 1)).Style.NumberFormat.Format = "@";
                    sht.Range("J" + i + ":J" + (i + 1)).Merge();
                    //Area
                    sht.Range("K" + i).SetValue(ds.Tables[0].Rows[ii]["grosswt"]);
                    sht.Range("K" + i + ":K" + (i + 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("K" + i + ":K" + (i + 1)).Style.NumberFormat.Format = "#,###0.00";
                    sht.Range("K" + i + ":K" + (i + 1)).Merge();
                    TotalGrossWT += TotalGrossWT + Convert.ToDecimal(ds.Tables[0].Rows[ii]["grosswt"]) * Convert.ToInt16(ds.Tables[0].Rows[ii]["totalroll"]);
                    //
                    //var _with27 = sht.Range("I" + i);

                    //_with27.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
                    //_with27.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
                    //PO
                    //sht.Range("K" + i).Value = ds.Tables[0].Rows[ii]["Pono"];
                    //
                    sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //Borders
                    sht.Range("A" + i + ":A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("K" + i + ":K" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ECIS = Convert.ToString(ds.Tables[0].Rows[0]["ecisno"]);
                    artno = Convert.ToString(ds.Tables[0].Rows[ii]["articleno"]);
                    i = i + 2;
                }
                //
                sht.Range("A" + i + ":A" + (i + 7)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 7)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //End Of loop
                i = i + 7;
                //*************** 
                sht.Range("A" + (i - 3) + ":A" + (i - 3)).SetValue("Supplier#17933");
                sht.Range("A" + (i - 2) + ":A" + (i - 2)).SetValue("ECIS#" + ECIS);
                //
                var _with21T = sht.Range("A" + i + ":K" + i);
                _with21T.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + (i + 1)).Value = "Total Sqmtr.:" + String.Format("{0:#,0.000}", Area);
                sht.Range("A" + (i + 1) + ":C" + (i + 1)).Merge();
                //sht.Range("C" + (i + 1)).Value = "Total Wt.:" + String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["netwt"]);
                //sht.Range("C" + (i + 1) + ":E" + (i + 1)).Merge();
                sht.Range("E" + (i + 2)).Value = "For Art. No.:" + artno;
                sht.Range("E" + (i + 2) + ":F" + (i + 2)).Merge();
                //_with21.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                i = i + 1;

                sht.Range("F" + (i + 1)).Value = "Total:";
                sht.Range("D" + i, "F" + (i + 1)).Style.Font.FontSize = 12;
                sht.Range("D" + i, "F" + (i + 1)).Style.Font.FontName = "Arial";
                sht.Range("D" + i, "F" + (i + 1)).Style.NumberFormat.Format = "@";
                sht.Range("D" + i, "F" + (i + 1)).Style.Font.Bold = true;
                sht.Range("D" + i + ":F" + i).Merge();
                sht.Range("D" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //Total Rolls
                sht.Range("G" + +(i + 1)).Value = TotalRolls;
                sht.Range("G" + (i + 1)).Style.Font.FontSize = 12;
                sht.Range("G" + (i + 1)).Style.Font.FontName = "Arial";
                sht.Range("G" + (i + 1)).Style.NumberFormat.Format = "@";
                sht.Range("G" + (i + 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //Total Pcs
                sht.Range("H" + (i + 1)).Value = TotalNetWT;
                sht.Range("H" + (i + 1)).Style.Font.FontSize = 12;
                sht.Range("H" + (i + 1)).Style.Font.FontName = "Arial";
                sht.Range("H" + (i + 1)).Style.NumberFormat.Format = "@";
                sht.Range("H" + (i + 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //Area
                sht.Range("J" + (i + 1)).Value = TotalGrossWT;
                sht.Range("J" + (i + 1)).Style.Font.FontSize = 12;
                sht.Range("J" + (i + 1)).Style.Font.FontName = "Arial";
                sht.Range("J" + (i + 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("J" + (i + 1)).Style.NumberFormat.Format = "#,###0.00";
                //
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                i = i + 1;
                var _with21 = sht.Range("A" + i + ":K" + i);
                // _with21.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                _with21.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                sht.Range("A" + i + ":A" + (i + 10)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 10)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //*************** 
                i = i + 2;
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {

                    sht.Range("A" + i + ":K" + i).Style.Font.FontSize = 12;
                    sht.Range("A" + i + ":K" + i).Style.Font.FontName = "Arial";
                    sht.Range("A" + i + ":I" + i).Style.NumberFormat.Format = "@";
                    sht.Range("K" + i).Style.Font.FontSize = 12;
                    sht.Range("K" + i).Style.Font.FontName = "Arial";
                    sht.Range("K" + i).Style.NumberFormat.Format = "@";

                    //Art No.
                    sht.Range("D" + i).Value = ds.Tables[0].Rows[j]["articleno"];

                    sht.Range("E" + i, "G" + i).Merge();
                    sht.Range("E" + i).Value = "(" + (Convert.ToDecimal(ds.Tables[0].Rows[j]["Pcs"]) / Convert.ToDecimal(ds.Tables[0].Rows[j]["pcsperroll"])) + " " + "Pallet x" + " " + ds.Tables[0].Rows[j]["pcsperroll"] + "Pcs/Pallet)";

                    ////pcs/roll
                    //sht.Range("G" + i).SetValue(ds.Tables[0].Rows[j]["pcsperroll"]);
                    //sht.Range("G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //sht.Range("G" + i).Style.NumberFormat.Format = "@";                  
                    ////Qty
                    //sht.Range("I" + i).SetValue(ds.Tables[0].Rows[j]["Pcs"]);
                    //sht.Range("I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[j]["Pcs"]);
                    //sht.Range("I" + i).Style.NumberFormat.Format = "@";                   
                    //Borders
                    sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    i = i + 1;
                }
                //
                sht.Range("A" + i + ":A" + (i + 7)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 7)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //*************** 
                i = i + 2;
                sht.Range("A" + i + ":G" + i).Merge();
                sht.Range("A" + i).Value = "STATEMENT ON ORIGIN";
                sht.Range("A" + i + ":G" + i).Style.Font.FontName = "Arial";
                sht.Range("A" + i + ":G" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":G" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //var _with466 = sht.Range("A" + i + ":A" + i);
                //_with466.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with466.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i + ":A" + (i + 2)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 2)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                sht.Range("G" + i + ":G" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("G" + i + ":G" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i + ":G" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i + ":G" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;

                i = i + 1;
                sht.Range("A" + i + ":G" + i).Merge();
                sht.Range("A" + i).Value = "The Exporter INREX0694000965EC026 of the products covered by this document declares that, except where otherwise clearly indicated, these products are of INDIA preferential origin according to rules of origin of the Generalized system of preferences of the European Union and that the origin criterion met is 'P'";
                sht.Range("A" + i + ":G" + i).Style.Font.FontName = "Arial";
                sht.Range("A" + i + ":G" + i).Style.Font.FontSize = 10;
                //sht.Range("A" + i + ":G" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":G" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Justify);
                sht.Row(i).Height = 50;
                sht.Range("A" + i + ":G" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //var _with467 = sht.Range("A" + i + ":K" + i);
                //_with467.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with467.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                sht.Range("A" + (i + 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + (i + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //**********Total
                i = i + 5;
                //i = 63;
                sht.Range("A" + i).Value = "TOTAL PKGS";
                sht.Range("A" + (i + 1)).Value = "TOTAL QUANTITY";
                sht.Range("A" + (i + 2)).Value = "TOTAL GR. WT";
                sht.Range("A" + (i + 3)).Value = "TOTAL NT. WT";
                sht.Range("A" + (i + 4)).Value = "TOTAL SQ.MTR";
                sht.Range("A" + (i + 5)).Value = "TOTAL VOLUME";
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontName = "Arial";
                sht.Range("A" + i + ":A" + (i + 5)).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontName = "Courier New";
                //value                
                sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontSize = 12;
                sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontName = "Arial";
                sht.Range("C" + i + ":C" + (i + 5)).Style.NumberFormat.Format = "@";


                sht.Range("C" + i).SetValue(ds.Tables[0].Rows[0]["Noofrolls"]);
                sht.Range("C" + (i + 1)).SetValue(Pcs);
                sht.Range("C" + (i + 2)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["grosswt"]) + " Kgs.");
                sht.Range("C" + (i + 3)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["netwt"]) + " Kgs.");
                sht.Range("C" + (i + 4)).SetValue(String.Format("{0:#,0.00}", Area) + " Sq.Mtr");
                sht.Range("C" + (i + 5)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["Volume"]) + " CBM");
                //
                sht.Range("A" + i + ":A" + (i + 5)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 5)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //***********Sig and date
                i = i + 6;
                //i = 70;
                sht.Range("A" + i + ":H" + i).Merge();
                sht.Range("I" + i).Value = "Signature/Date";
                sht.Range("I" + i).Style.Font.FontSize = 12;
                sht.Range("I" + i).Style.Font.FontName = "Arial";
                sht.Range("I" + i).Style.NumberFormat.Format = "@";

                sht.Range("K" + i).Value = "For " + ds.Tables[0].Rows[0]["companyname"];
                sht.Range("J" + i + ":K" + i).Style.Font.FontSize = 12;
                sht.Range("J" + i + ":K" + i).Style.Font.FontName = "Arial";
                sht.Range("J" + i + ":K" + i).Style.NumberFormat.Format = "@";
                sht.Range("J" + i + ":K" + i).Style.Font.Bold = true;
                sht.Range("J" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("J" + i + ":K" + i).Style.Font.FontSize = 9;
                //sht.Range("J" + i + ":K" + i).Merge();              
                sht.Range("I" + i + ":K" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("I" + i + ":I" + (i + 3)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 4)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //**************Declaration
                //
                sht.Range("A" + i + ":A" + (i + 3)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                i = i + 4;
                //i = 71;
                //sht.Range("A" + i).Value = "Declaration";
                //sht.Range("A" + i + ":B" + i).Merge();
                //sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 12;
                //sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Arial";
                //sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";
                //sht.Range("A" + i + ":B" + i).Style.Font.Bold = true;
                //sht.Range("A" + i + ":B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                //value
                //i = i + 1;
                //sht.Range("A" + i).Value = "We declare that this invoice shows the actual price of the";
                //sht.Range("A" + i + ":F" + i).Merge();
                //sht.Range("A" + i + ":F" + i).Style.Font.FontSize = 12;
                //sht.Range("A" + i + ":F" + i).Style.Font.FontName = "Arial";
                //sht.Range("A" + i + ":F" + i).Style.NumberFormat.Format = "@";
                //sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //   i = i + 1;
                //sht.Range("A" + i).Value = "goods described and that all particulars are true and correct";
                //sht.Range("A" + i + ":F" + i).Merge();
                //sht.Range("A" + i + ":F" + i).Style.Font.FontSize = 12;
                //sht.Range("A" + i + ":F" + i).Style.Font.FontName = "Arial";
                //sht.Range("A" + i + ":F" + i).Style.NumberFormat.Format = "@";

                //sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //**********

                sht.Range("I" + i + ":J" + i).Merge();
                sht.Range("I" + i + ":J" + i).Value = ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("I" + i + ":J" + i).Style.Font.FontName = "Arial";
                sht.Range("I" + i + ":J" + i).Style.Font.FontSize = 12;
                sht.Range("I" + i + ":J" + i).Style.NumberFormat.Format = "dd/MMM/yyyy";
                sht.Range("I" + i + ":J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("K" + i).Value = "Auth Sign";
                sht.Range("K" + i).Style.Font.FontSize = 12;
                sht.Range("K" + i).Style.Font.FontName = "Arial";
                sht.Range("K" + i).Style.NumberFormat.Format = "@";
                sht.Range("K" + i).Style.Font.Bold = true;
                sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //
                var _with39 = sht.Range("I" + i);
                _with39.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //
                //sht.Range("A2:A10").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("A11:A23").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i + ":K" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //sht.Range("A1:K1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //sht.Range("A2:A10").Style.Border.LeftBorder = XLBorderStyleValues.Thin;                
                //sht.Range("F2:F10").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //            //************************
                var _with2 = sht.Range("A11:K11");
                _with2.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with3 = sht.Range("G3:G30");
                //  _with3.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with3.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                var _with4 = sht.Range("K3:K30");
                //  _with3.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with4.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _with41 = sht.Range("G3:K3");
                _with41.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                var _with42 = sht.Range("G8:K8");
                _with42.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with42.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                // var _with40 = sht.Range("K4:K4");
                // _with40.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                // //
                // var _with4 = sht.Range("G5:I6");
                // //_with4.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                // _with4.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                // var _with42 = sht.Range("G5:I5");
                // _with42.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                // var _with43 = sht.Range("G6:I6");
                // _with43.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                // //
                // var _with5 = sht.Range("J5:K5");
                // _with5.Style.Border.TopBorder =     XLBorderStyleValues.Thin;
                // var _with44 = sht.Range("J5:K6");
                // _with44.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                // var _with45 = sht.Range("J6:K6");
                // _with45.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                // //
                // var _with6 = sht.Range("G7:K8");
                // _with6.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                // _with6.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                // var _with46 = sht.Range("G8:K8");
                // _with46.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                //var _with7 = sht.Range("G9:K11");
                //_with7.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //_with7.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //var _with47 = sht.Range("G12:K25");
                //_with47.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //sht.Range("G12:I12").Style.Border.RightBorder = XLBorderStyleValues.None;

                //
                var _with8 = sht.Range("A24:F24");
                _with8.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                var _with48 = sht.Range("A12:F24");
                _with48.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                sht.Range("A12:B12").Style.Border.RightBorder = XLBorderStyleValues.None;
                sht.Range("A18:B18").Style.Border.RightBorder = XLBorderStyleValues.None;
                sht.Range("G12:H12").Style.Border.RightBorder = XLBorderStyleValues.None;
                sht.Range("G18:H20").Style.Border.RightBorder = XLBorderStyleValues.None;

                //
                var _with9 = sht.Range("A25:C26");
                _with9.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with9.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //sht.Range("A24:C24").Style.Border.BottomBorder = XLBorderStyleValues.None;

                sht.Range("A26:C26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                sht.Range("G26:I26").Style.Border.BottomBorder = XLBorderStyleValues.None;

                sht.Range("J26:K26").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //
                var _with10 = sht.Range("D25:F26");
                _with10.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with10.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D25:F25").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //
                var _with11 = sht.Range("A27:C28");
                _with11.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with11.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with11.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A27:C27").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //
                var _with12 = sht.Range("D27:F28");
                _with12.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with12.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D27:F27").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //
                var _with13 = sht.Range("A29:C30");
                _with13.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _with13.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A29:C29").Style.Border.BottomBorder = XLBorderStyleValues.None;
                sht.Range("A30:C30").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with14 = sht.Range("D29:F30");
                _with14.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                _with14.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D29:G29").Style.Border.BottomBorder = XLBorderStyleValues.None;
                sht.Range("G29").Style.Border.BottomBorder = XLBorderStyleValues.None;

                //
                //var _with15 = sht.Range("G25:K25");
                //_with15.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //_with15.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("G25:K25").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("G26:K26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                //var _with16 = sht.Range("J26:K27");
                //_with16.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //sht.Range("J26:K26").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //sht.Range("J27:K27").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _with17 = sht.Range("G28:K30");
                _with17.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with17.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("G30:K30").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("G29:G30").Style.Border.RightBorder = XLBorderStyleValues.None;
                sht.Range("H29:K30").Style.Border.LeftBorder = XLBorderStyleValues.None;
                //
                var _with18 = sht.Range("A39:K39");
                _with18.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with18.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                //var _with19 = sht.Range("J31:J38");
                //_with19.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with19.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                var _with20 = sht.Range("A39:K40");
                _with20.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                _with20.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //
                var _withlr = sht.Range("I39:I40");
                _withlr.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _withlr.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                var _with22 = sht.Range("J39:J40");
                _with22.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                _with22.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //
                sht.Range("A31:A38").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K31:K40").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A39:A40").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K38:K38").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A2:K2").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A3:A24").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //*******************Save
                string Fileextension = "xlsx";
                string filename = "PackingList -" + validateFilename(ds.Tables[0].Rows[0]["TinvoiceNo"].ToString()) + "." + Fileextension + "";
                Path = Server.MapPath("~/Packingexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
                //*****
                lblmsg.Text = "Packing Excel Format downloaded successfully.";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "AltR2", "alert('No Record Found to Export')", true);
            }

        }
        catch (Exception)
        {

            throw;
        }
    }
    //protected void Exporttoexcel_Packing_Kaysons_3()
    //{
    //    lblmsg.Text = "Wait For Excel File to Download";
    //    int i = 0, Pcs = 0, TotalRolls = 0;
    //    Decimal Area = 0;
    //    string Path = "";
    //    if (!Directory.Exists(Server.MapPath("~/PackingExcel/")))
    //    {
    //        Directory.CreateDirectory(Server.MapPath("~/PackingExcel/"));
    //    }
    //    try
    //    {
    //        string str = @"select *,VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1) as Pcsperroll,VD.Pcs/(VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1)) as TotalRoll From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue + " order by MinrollNo";
    //        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {

    //            var xapp = new XLWorkbook();
    //            var sht = xapp.Worksheets.Add("PackingList");
    //            //************set cell width
    //            //Page
    //            sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
    //            sht.PageSetup.AdjustTo(71);
    //            //sht.PageSetup.FitToPages(1, 5);                
    //            sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
    //            sht.PageSetup.VerticalDpi = 300;
    //            sht.PageSetup.HorizontalDpi = 300;
    //            //
    //            sht.PageSetup.Margins.Top = 0.55;
    //            sht.PageSetup.Margins.Left = 0;
    //            sht.PageSetup.Margins.Right = 0;
    //            sht.PageSetup.Margins.Bottom = 0.53;
    //            sht.PageSetup.Margins.Header = 0.5;
    //            sht.PageSetup.Margins.Footer = 0.5;
    //            sht.PageSetup.CenterHorizontally = true;
    //            sht.PageSetup.SetScaleHFWithDocument();
    //            //************
    //            sht.Column("A").Width = 10.29;
    //            sht.Column("B").Width = 8.43;
    //            sht.Column("C").Width = 8.43;
    //            sht.Column("D").Width = 11.00;
    //            sht.Column("E").Width = 9.71;
    //            sht.Column("F").Width = 12.57;
    //            sht.Column("G").Width = 10.14;
    //            sht.Column("H").Width = 13.14;
    //            sht.Column("I").Width = 11.71;
    //            sht.Column("J").Width = 14.29;
    //            sht.Column("K").Width = 19.29;
    //            //************
    //            sht.Row(1).Height = 15.75;
    //            //********Header
    //            sht.Range("A1").Value = "PACKING LIST";
    //            sht.Range("A1:K1").Style.Font.FontName = "Tahoma";
    //            sht.Range("A1:K1").Style.Font.FontSize = 10;
    //            sht.Range("A1:K1").Style.Font.Bold = true;
    //            sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            sht.Range("A1:K1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
    //            sht.Range("A1:K1").Style.NumberFormat.Format = "@";
    //            sht.Range("A1:K1").Merge();
    //            //********Exporter
    //            sht.Range("A2").Value = "Exporter";
    //            sht.Range("A2:F2").Style.Font.FontSize = 10;
    //            sht.Range("A2:F2").Style.Font.FontName = "Tahoma";
    //            sht.Range("A2:F2").Style.NumberFormat.Format = "@";
    //            sht.Range("A2:F2").Merge();

    //            //CompanyName
    //            sht.Range("A3").Value = ds.Tables[0].Rows[0]["companyName"];
    //            sht.Range("A3:F3").Style.Font.FontSize = 10;
    //            sht.Range("A3:F3").Style.Font.FontName = "Tahoma";
    //            sht.Range("A3:F3").Style.NumberFormat.Format = "@";
    //            sht.Range("A3:F3").Style.Font.Bold = true;
    //            sht.Range("A3:F3").Merge();
    //            //Address
    //            sht.Range("A4").Value = ds.Tables[0].Rows[0]["compaddr1"];
    //            sht.Range("A4:F4").Style.Font.FontSize = 10;
    //            sht.Range("A4:F4").Style.Font.FontName = "Tahoma";
    //            sht.Range("A4:F4").Style.NumberFormat.Format = "@";
    //            sht.Range("A4:F4").Merge();
    //            //address2
    //            sht.Range("A5").Value = ds.Tables[0].Rows[0]["compaddr2"] + "," + ds.Tables[0].Rows[0]["compaddr3"];
    //            sht.Range("A5:F5").Style.Font.FontSize = 10;
    //            sht.Range("A5:F5").Style.Font.FontName = "Tahoma";
    //            sht.Range("A5:F5").Style.NumberFormat.Format = "@";
    //            sht.Range("A5:F5").Merge();
    //            //TiN No
    //            sht.Range("A6").Value = "GSTIN#" + ds.Tables[0].Rows[0]["gstno"];
    //            sht.Range("A6:F6").Style.Font.FontSize = 10;
    //            sht.Range("A6:F6").Style.Font.FontName = "Tahoma";
    //            sht.Range("A6:F6").Style.NumberFormat.Format = "@";
    //            sht.Range("A6:F6").Style.Font.Bold = true;
    //            sht.Range("A6:F6").Merge();

    //            sht.Range("A8").Value = "STATE" + ds.Tables[0].Rows[0]["rec_state"];
    //            sht.Range("A8:F8").Style.Font.FontName = "Tahoma";
    //            sht.Range("A8:F8").Style.Font.FontSize = 10;
    //            sht.Range("A8:F8").Style.Font.Bold = true;
    //            sht.Range("A8:F8").Merge();

    //            sht.Range("A9").Value = "PAN NO." + ds.Tables[0].Rows[0]["PANNr"];
    //            sht.Range("A9:F9").Style.Font.FontName = "Tahoma";
    //            sht.Range("A9:F9").Style.Font.FontSize = 10;
    //            sht.Range("A9:F9").Style.Font.Bold = true;
    //            sht.Range("A9:F9").Merge();
    //            //**********INvoiceNodate
    //            sht.Range("G2").Value = "Invoice No./Date";
    //            sht.Range("G2:K2").Style.Font.FontSize = 10;
    //            sht.Range("G2:K2").Style.Font.FontName = "Tahoma";
    //            sht.Range("G2:K2").Style.NumberFormat.Format = "@";

    //            sht.Range("G2:K2").Merge();
    //            //value
    //            sht.Range("G3").Value = ds.Tables[0].Rows[0]["TInvoiceNo"] + " Dated " + ds.Tables[0].Rows[0]["InvoiceDate"];
    //            sht.Range("G3:K3").Style.Font.FontSize = 10;
    //            sht.Range("G3:K3").Style.Font.FontName = "Tahoma";
    //            sht.Range("G3:K3").Style.NumberFormat.Format = "@";
    //            sht.Range("G3:K3").Merge();
    //            sht.Range("G3", "K3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            //*************IE Code
    //            //sht.Range("G4").Value = "IE Code No.";
    //            //sht.Range("G4:I4").Style.Font.FontSize = 12;
    //            //sht.Range("G4:I4").Style.Font.FontName = "Tahoma";
    //            //sht.Range("G4:I4").Style.NumberFormat.Format = "@";
    //            //sht.Range("G4:I4").Merge();
    //            ////value
    //            //sht.Range("G5").Value = ds.Tables[0].Rows[0]["IEcode"];
    //            //sht.Range("G5:I5").Style.Font.FontSize = 12;
    //            //sht.Range("G5:I5").Style.Font.FontName = "Tahoma";
    //            //sht.Range("G5:I5").Style.NumberFormat.Format = "@";
    //            //sht.Range("G5:I5").Merge();
    //            //sht.Range("G5", "I5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            ////***********GRI Form No
    //            //sht.Range("J4").Value = "GRI Form No.";
    //            //sht.Range("J4:K4").Style.Font.FontSize = 12;
    //            //sht.Range("J4:K4").Style.Font.FontName = "Tahoma";
    //            //sht.Range("J4:K4").Style.NumberFormat.Format = "@";
    //            //sht.Range("J4:K4").Merge();
    //            //// value
    //            //sht.Range("J5").Value = "";
    //            //sht.Range("J5:K5").Style.Font.FontSize = 12;
    //            //sht.Range("J5:K5").Style.Font.FontName = "Tahoma";
    //            //sht.Range("J5:K5").Style.NumberFormat.Format = "@";
    //            //sht.Range("J5:K5").Merge();
    //            //sht.Range("J5", "K5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            //*************Buyer's Order No and Date
    //            sht.Range("G6").Value = "Buyer's Order No.";
    //            sht.Range("G6:K6").Style.Font.FontSize = 10;
    //            sht.Range("G6:K6").Style.Font.FontName = "Tahoma";
    //            sht.Range("G6:K6").Style.NumberFormat.Format = "@";
    //            sht.Range("G6:K6").Merge();
    //            //value
    //            sht.Range("G7").SetValue(ds.Tables[0].Rows[0]["TorderNo"]);
    //            sht.Range("G7:K7").Style.Font.FontSize = 10;
    //            sht.Range("G7:K7").Style.Font.FontName = "Tahoma";
    //            sht.Range("G7:K7").Style.NumberFormat.Format = "@";
    //            sht.Range("G7:K7").Merge();
    //            sht.Range("G7", "K7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            //**************Other Reference(s)
    //            // sht.Range("G8").Value = "Other Reference(s)";

    //            sht.Range("G9").Value = "SUPPLIER NO.:";
    //            sht.Range("G10").Value = "CONSIGNMENT NO.:" + ds.Tables[0].Rows[0]["otherref"];
    //            sht.Range("J9").Value = "DESTN. CODE";
    //            sht.Range("J9:K9").Style.Font.FontSize = 10;
    //            sht.Range("J9:K9").Style.Font.FontName = "Tahoma";
    //            sht.Range("G8:G10").Style.Font.FontSize = 10;
    //            sht.Range("G8:G10").Style.Font.FontName = "Tahoma";
    //            sht.Range("G8:G10").Style.NumberFormat.Format = "@";
    //            sht.Range("G8:K8").Merge();
    //            sht.Range("G9:I9").Merge();
    //            sht.Range("G10:K10").Merge();
    //            sht.Range("J9:K9").Merge();
    //            //*************Consignee
    //            sht.Range("A11").Value = "Consignee";
    //            sht.Range("C11").Value = ds.Tables[0].Rows[0]["consignee_dt"];
    //            sht.Range("A11:B11").Style.Font.FontSize = 10;
    //            sht.Range("A11:B11").Style.Font.FontName = "Tahoma";
    //            sht.Range("A11:B11").Style.NumberFormat.Format = "@";


    //            sht.Range("C11:F11").Style.Font.FontSize = 10;
    //            sht.Range("C11:F11").Style.Font.FontName = "Tahoma";
    //            sht.Range("C11:F11").Style.NumberFormat.Format = "@";
    //            sht.Range("C11:F11").Style.Font.Bold = true;

    //            sht.Range("A11:B11").Merge();
    //            sht.Range("C11:F11").Merge();
    //            //value
    //            sht.Range("A12").Value = ds.Tables[0].Rows[0]["consignee"];
    //            sht.Range("A12:F12").Style.Font.FontSize = 10;
    //            sht.Range("A12:F12").Style.Font.FontName = "Tahoma";
    //            sht.Range("A12:F12").Style.NumberFormat.Format = "@";
    //            sht.Range("A12:F12").Style.Font.Bold = true;
    //            sht.Range("A12:F12").Merge();
    //            //**
    //            sht.Range("A13").Value = ds.Tables[0].Rows[0]["consignee_address"];
    //            sht.Range("A13:F16").Style.Font.FontSize = 10;
    //            sht.Range("A13:F16").Style.Font.FontName = "Tahoma";
    //            sht.Range("A13:F16").Style.NumberFormat.Format = "@";

    //            sht.Range("A13", "F16").Style.Alignment.WrapText = true;
    //            sht.Range("A13", "F16").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
    //            sht.Range("A13:F16").Merge();
    //            //***********Notify
    //            sht.Range("A17").Value = "Notify Party";
    //            sht.Range("C17").Value = ds.Tables[0].Rows[0]["Notifyparty_dt"];
    //            sht.Range("A17:B17").Style.Font.FontSize = 10;
    //            sht.Range("A17:B17").Style.Font.FontName = "Tahoma";
    //            sht.Range("A17:B17").Style.NumberFormat.Format = "@";

    //            sht.Range("A17:B17").Style.Font.Underline = XLFontUnderlineValues.Single;
    //            sht.Range("C17:F17").Style.Font.FontSize = 10;
    //            sht.Range("C17:F17").Style.Font.FontName = "Tahoma";
    //            sht.Range("C17:F17").Style.NumberFormat.Format = "@";
    //            sht.Range("C17:F17").Style.Font.Bold = true;

    //            sht.Range("A17:B17").Merge();
    //            sht.Range("C17:F17").Merge();
    //            //value
    //            sht.Range("A18").Value = ds.Tables[0].Rows[0]["Notifyparty"];
    //            sht.Range("A18:F18").Style.Font.FontSize = 10;
    //            sht.Range("A18:F18").Style.Font.FontName = "Tahoma";
    //            sht.Range("A18:F18").Style.NumberFormat.Format = "@";
    //            sht.Range("A18:F18").Style.Font.Bold = true;

    //            sht.Range("A18:F18").Merge();

    //            sht.Range("A19").Value = ds.Tables[0].Rows[0]["notifyparty_address"];
    //            sht.Range("A19:F23").Style.Font.FontSize = 10;
    //            sht.Range("A19:F23").Style.Font.FontName = "Tahoma";
    //            sht.Range("A19:F23").Style.NumberFormat.Format = "@";

    //            sht.Range("A19", "F23").Style.Alignment.WrapText = true;
    //            sht.Range("A19", "F23").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
    //            sht.Range("A19:F23").Merge();
    //            //***********Receiver address
    //            //sht.Range("G11").Value = "Receiver Address";
    //            //sht.Range("G11:I11").Style.Font.FontSize = 12;
    //            //sht.Range("G11:I11").Style.Font.FontName = "Tahoma";
    //            //sht.Range("G11:I11").Style.NumberFormat.Format = "@";
    //            //sht.Range("G11:I11").Merge();
    //            ////values
    //            //sht.Range("J11").Value = ds.Tables[0].Rows[0]["Destcode"];
    //            //sht.Range("J11:K11").Style.Font.FontSize = 12;
    //            //sht.Range("J11:K11").Style.Font.FontName = "Tahoma";
    //            //sht.Range("J11:K11").Style.NumberFormat.Format = "@";
    //            //sht.Range("J11:K11").Style.Font.Bold = true;

    //            //sht.Range("J11:K11").Merge();
    //            //****** 1.
    //            sht.Range("G12").Value = "1.Buyer (If other than Consignee) ";
    //            sht.Range("G12:K12").Style.Font.FontSize = 10;
    //            sht.Range("G12:K12").Style.Font.FontName = "Tahoma";
    //            sht.Range("G12:K12").Style.NumberFormat.Format = "@";
    //            sht.Range("G12:K12").Style.Font.Bold = true;
    //            sht.Range("G12:K12").Merge();
    //            //*
    //            sht.Range("G13").Value = ds.Tables[0].Rows[0]["buyer_otherthanconsignee"];
    //            sht.Range("G13:K15").Style.Font.FontSize = 10;
    //            sht.Range("G13:K15").Style.Font.FontName = "Tahoma";
    //            sht.Range("G13:K15").Style.NumberFormat.Format = "@";

    //            sht.Range("G13:K15").Style.Alignment.WrapText = true;
    //            sht.Range("G13:K15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
    //            sht.Range("G13:K15").Merge();
    //            //****** 2.
    //            sht.Range("G16").Value = "2.Paying Agent : " + ds.Tables[0].Rows[0]["payingagent"];
    //            sht.Range("G16:K16").Style.Font.FontSize = 10;
    //            sht.Range("G16:K16").Style.Font.FontName = "Tahoma";
    //            sht.Range("G16:K16").Style.NumberFormat.Format = "@";
    //            sht.Range("G16:K16").Style.Font.Bold = true;

    //            sht.Range("G16:K16").Merge();
    //            //*
    //            sht.Range("I17").Value = ds.Tables[0].Rows[0]["payingagent_address"];
    //            sht.Range("I17:K19").Style.Font.FontSize = 10;
    //            sht.Range("I17:K19").Style.Font.FontName = "Tahoma";
    //            sht.Range("I17:K19").Style.NumberFormat.Format = "@";

    //            sht.Range("I17:K19").Style.Alignment.WrapText = true;
    //            sht.Range("I17:K19").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
    //            sht.Range("I17:K19").Merge();
    //            //*******3.
    //            //sht.Range("G20").Value = "Buyer (If other than Consignee)";
    //            //sht.Range("G20:K20").Style.Font.FontSize = 12;
    //            //sht.Range("G20:K20").Style.Font.FontName = "Tahoma";
    //            //sht.Range("G20:K20").Style.NumberFormat.Format = "@";
    //            //sht.Range("G20:K20").Merge();

    //            //sht.Range("G21").Value = "3." + ds.Tables[0].Rows[0]["buyer_otherthanconsignee"];
    //            //sht.Range("G21:K21").Style.Font.FontSize = 12;
    //            //sht.Range("G21:K21").Style.Font.FontName = "Tahoma";
    //            //sht.Range("G21:K21").Style.NumberFormat.Format = "@";
    //            //sht.Range("G21:K21").Style.Font.Bold = true;

    //            //sht.Range("G21:K21").Merge();
    //            ////*
    //            //sht.Range("G22").Value = ds.Tables[0].Rows[0]["otherthanconsignee_address"];
    //            //sht.Range("G22:K27").Style.Font.FontSize = 12;
    //            //sht.Range("G22:K27").Style.Font.FontName = "Tahoma";
    //            //sht.Range("G22:K27").Style.NumberFormat.Format = "@";
    //            //sht.Range("G22:K27").Style.Alignment.WrapText = true;
    //            //sht.Range("G22:K27").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
    //            //sht.Range("G22:K27").Merge();
    //            //***********Pre-carriage By
    //            sht.Range("A24").Value = "Pre-Carriage By";
    //            sht.Range("A24:C24").Style.Font.FontSize = 10;
    //            sht.Range("A24:C24").Style.Font.FontName = "Tahoma";
    //            sht.Range("A24:C24").Style.NumberFormat.Format = "@";
    //            sht.Range("A24:C24").Merge();
    //            //value
    //            sht.Range("A25").Value = ds.Tables[0].Rows[0]["Pre_carriageby"];
    //            sht.Range("A25:C25").Style.Font.FontSize = 10;
    //            sht.Range("A25:C25").Style.Font.FontName = "Tahoma";
    //            sht.Range("A25:C25").Style.NumberFormat.Format = "@";
    //            sht.Range("A25:C25").Merge();
    //            sht.Range("A25:C25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

    //            //************Place of Receipt by Pre-Carrier
    //            sht.Range("D24").Value = "Place of Receipt by Pre-Carrier";
    //            sht.Range("D24:F24").Style.Font.FontSize = 10;
    //            sht.Range("D24:F24").Style.Font.FontName = "Tahoma";
    //            sht.Range("D24:F24").Style.NumberFormat.Format = "@";
    //            sht.Range("D24:F24").Merge();
    //            //value
    //            sht.Range("D25").Value = ds.Tables[0].Rows[0]["receiptby"];
    //            sht.Range("D25:F25").Style.Font.FontSize = 10;
    //            sht.Range("D25:F25").Style.Font.FontName = "Tahoma";
    //            sht.Range("D25:F25").Style.NumberFormat.Format = "@";
    //            sht.Range("D25:F25").Merge();
    //            sht.Range("D25:F25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            //************Vessel/Flight No
    //            sht.Range("A26").Value = "Vessel/Flight No";
    //            sht.Range("A26:C26").Style.Font.FontSize = 10;
    //            sht.Range("A26:C26").Style.Font.FontName = "Tahoma";
    //            sht.Range("A26:C26").Style.NumberFormat.Format = "@";
    //            sht.Range("A26:C26").Merge();
    //            //value
    //            sht.Range("A27").Value = ds.Tables[0].Rows[0]["vessel_flightno"];
    //            sht.Range("A27:C27").Style.Font.FontSize = 10;
    //            sht.Range("A27:C27").Style.Font.FontName = "Tahoma";
    //            sht.Range("A27:C27").Style.NumberFormat.Format = "@";
    //            sht.Range("A27:C27").Merge();
    //            sht.Range("A27:C27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            //************Port of Loading
    //            sht.Range("D26").Value = "Port of Loading";
    //            sht.Range("D26:F26").Style.Font.FontSize = 10;
    //            sht.Range("D26:F26").Style.Font.FontName = "Tahoma";
    //            sht.Range("D26:F26").Style.NumberFormat.Format = "@";
    //            sht.Range("D26:F26").Merge();
    //            //value
    //            sht.Range("D27").Value = ds.Tables[0].Rows[0]["portload"];
    //            sht.Range("D27:F27").Style.Font.FontSize = 10;
    //            sht.Range("D27:F27").Style.Font.FontName = "Tahoma";
    //            sht.Range("D27:F27").Style.NumberFormat.Format = "@";
    //            sht.Range("D27:F27").Merge();
    //            sht.Range("D27:F27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            //************Port of Discharge
    //            sht.Range("A28").Value = "Port of Discharge";
    //            sht.Range("A28:C28").Style.Font.FontSize = 10;
    //            sht.Range("A28:C28").Style.Font.FontName = "Tahoma";
    //            sht.Range("A28:C28").Style.NumberFormat.Format = "@";
    //            sht.Range("A28:C28").Merge();
    //            //value
    //            sht.Range("A29").Value = ds.Tables[0].Rows[0]["portunload"];
    //            sht.Range("A29:C29").Style.Font.FontSize = 10;
    //            sht.Range("A29:C29").Style.Font.FontName = "Tahoma";
    //            sht.Range("A29:C29").Style.NumberFormat.Format = "@";
    //            sht.Range("A29:C29").Merge();
    //            sht.Range("A29:C29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            //************Final Destination
    //            sht.Range("D28").Value = "Final Destination";
    //            sht.Range("D28:F28").Style.Font.FontSize = 10;
    //            sht.Range("D28:F28").Style.Font.FontName = "Tahoma";
    //            sht.Range("D28:F28").Style.NumberFormat.Format = "@";
    //            sht.Range("D28:F28").Merge();
    //            //value
    //            sht.Range("D29").Value = ds.Tables[0].Rows[0]["Destinationadd"];
    //            sht.Range("D29:F29").Style.Font.FontSize = 10;
    //            sht.Range("D29:F29").Style.Font.FontName = "Tahoma";
    //            sht.Range("D29:F29").Style.NumberFormat.Format = "@";
    //            sht.Range("D29:F29").Merge();
    //            sht.Range("D29:F29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            //************Country of Origin of Goods
    //            sht.Range("G24").Value = "Country of Origin of Goods";
    //            sht.Range("G24:I24").Style.Font.FontSize = 10;
    //            sht.Range("G24:I24").Style.Font.FontName = "Tahoma";
    //            sht.Range("G24:I24").Style.NumberFormat.Format = "@";
    //            sht.Range("G24:I24").Merge();
    //            //value
    //            sht.Range("G25").Value = ds.Tables[0].Rows[0]["countryoforigin"];
    //            sht.Range("G25:I25").Style.Font.FontSize = 10;
    //            sht.Range("G25:I25").Style.Font.FontName = "Tahoma";
    //            sht.Range("G25:I25").Style.NumberFormat.Format = "@";
    //            sht.Range("G25:I25").Merge();
    //            sht.Range("G25:I25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            //************Country of Final Destination
    //            sht.Range("J24").Value = "Country of Final Destination";
    //            sht.Range("J24:K24").Style.Font.FontSize = 10;
    //            sht.Range("J24:K24").Style.Font.FontName = "Tahoma";
    //            sht.Range("J24:K24").Style.NumberFormat.Format = "@";
    //            sht.Range("J24:K24").Merge();
    //            //value
    //            sht.Range("J25").Value = ds.Tables[0].Rows[0]["countryoffinaldest"];
    //            sht.Range("J25:K25").Style.Font.FontSize = 10;
    //            sht.Range("J25:K25").Style.Font.FontName = "Tahoma";
    //            sht.Range("J25:K25").Style.NumberFormat.Format = "@";
    //            sht.Range("J25:K25").Merge();
    //            sht.Range("J25:K25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

    //            sht.Range("G26").Value = "Terms of Delivery AND Payment";
    //            sht.Range("G26:k26").Style.Font.FontSize = 10;
    //            sht.Range("G26:k26").Style.Font.FontName = "Tahoma";
    //            sht.Range("G26:k26").Style.NumberFormat.Format = "@";
    //            sht.Range("G26:k26").Merge();

    //            sht.Range("H27").Value = "DELIVERY WEEk  :";
    //            sht.Range("H27").Style.Font.FontSize = 10;
    //            sht.Range("H27").Style.Font.FontName = "Tahoma";
    //            sht.Range("H27").Style.NumberFormat.Format = "@";
    //            //sht.Range("G26:k26").Merge();

    //            //*****************Nos
    //            sht.Range("A30").Value = "Marks Nos./Container No.";
    //            sht.Range("B30").Value = "No of Packages";
    //            sht.Range("B30:D30").Merge();

    //            sht.Range("E30").Value = "Description of Goods";
    //            sht.Range("E30:J30").Merge();
    //            //sht.Range("k30").Value = "Quantity";                
    //            sht.Range("A30:K30").Style.Font.FontSize = 10;
    //            sht.Range("A30:K30").Style.Font.FontName = "Tahoma";
    //            sht.Range("A30:K30").Style.NumberFormat.Format = "@";
    //            sht.Range("K30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    //            //values
    //            sht.Range("A31:H31").Merge();
    //            sht.Range("A31").Value = "IKEA/FINAL DESTN.";
    //            sht.Range("A32:H36").Style.Font.FontSize = 10;
    //            sht.Range("A32:H36").Style.Font.FontName = "Tahoma";
    //            sht.Range("A32:H36").Style.NumberFormat.Format = "@";
    //            sht.Range("A32").Value = "No. " + ds.Tables[0].Compute("Min(MinrollNo)", "") + " to " + ds.Tables[0].Compute("Max(Maxrollno)", "");
    //            sht.Range("B32").Value = ds.Tables[0].Rows[0]["Noofrolls"] + " Pallets";
    //            sht.Range("B32:C32").Merge();
    //            sht.Range("E32").Value = ds.Tables[0].Rows[0]["descriptionofgoods"];
    //            sht.Range("E32:J36").Style.Alignment.WrapText = true;
    //            sht.Range("E32:J36").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
    //            sht.Range("E32:J36").Merge();
    //            sht.Range("A37:J37").Merge();
    //            //            //**********************Details
    //            //********Hader
    //            sht.Range("A38").Value = "P.O.#";
    //            sht.Range("A38:K38").Style.Font.FontSize = 10;
    //            sht.Range("A38:K38").Style.Font.FontName = "Tahoma";
    //            sht.Range("A38:K38").Style.NumberFormat.Format = "@";
    //            sht.Range("A38:K38").Style.Font.Bold = true;
    //            sht.Range("A38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            sht.Range("B38").Value = "Description, Colour & Size";
    //            sht.Range("B38:D38").Merge();
    //            // sht.Range("D38").Value = "ART.NO.";
    //            //sht.Range("D38:E38")..Merge();
    //            sht.Range("E38").Value = "Total Pcs";
    //            sht.Range("F38").Value = "TOTAL NT. WT.";
    //            sht.Range("F38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            sht.Range("G38").Value = "NT WT/PALLET";
    //            sht.Range("G38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            sht.Range("H38").Value = "TOTAL GR.WT.";
    //            sht.Range("H38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            sht.Range("I38").Value = "GR WT/PALLET";
    //            //sht.Range("J38").Value = "Area Sq. Mtr";
    //            //sht.Range("K38").Value = "P.O.#";
    //            sht.Range("I38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            sht.Range("A38:I38").Style.Border.TopBorder = XLBorderStyleValues.Thin;
    //            sht.Range("A38").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            sht.Range("I38").Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            sht.Range("A38:I38").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //            //***********generate Loop
    //            i = 39;
    //            for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
    //            {
    //                sht.Range("A" + i, "K" + i).Style.Font.FontSize = 10;
    //                sht.Range("A" + i, "K" + i).Style.Font.FontName = "Tahoma";
    //                sht.Range("A" + i, "I" + i).Style.NumberFormat.Format = "@";
    //                sht.Range("K" + i).Style.Font.FontSize = 10;
    //                sht.Range("K" + i).Style.Font.FontName = "Tahoma";
    //                sht.Range("K" + i).Style.NumberFormat.Format = "@";
    //                //pono
    //                sht.Range("A" + i).Value = ds.Tables[0].Rows[ii]["Pono"];
    //                sht.Range("A" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                //Article Name
    //                sht.Range("B" + i).Value = ds.Tables[0].Rows[ii]["Design"] + "," + ds.Tables[0].Rows[ii]["Color"] + " , " + ds.Tables[0].Rows[ii]["width"] + "x" + ds.Tables[0].Rows[ii]["Length"];
    //                sht.Range("B" + i, "D" + i).Merge();
    //                //Art No.
    //                //  sht.Range("D" + i).Value = ds.Tables[0].Rows[ii]["articleno"];
    //                //Colour
    //                sht.Range("E" + i).Value = ds.Tables[0].Rows[ii]["Pcs"];
    //                //Size
    //                double netperpallet = Convert.ToDouble(ds.Tables[0].Rows[0]["netwt"]) / Convert.ToInt16(ds.Tables[0].Rows[ii]["Pcs"]);
    //                sht.Range("F" + i).Value = String.Format("{0:#,0.000}", netperpallet);

    //                //pcs/roll
    //                sht.Range("G" + i).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["netwt"]));
    //                //sht.Range("G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                //sht.Range("G" + i).Style.NumberFormat.Format = "@";
    //                //Total rolls
    //                double grossperpallet = Convert.ToDouble(ds.Tables[0].Rows[0]["grosswt"]) / Convert.ToInt16(ds.Tables[0].Rows[ii]["Pcs"]);
    //                sht.Range("H" + i).SetValue(String.Format("{0:#,0.000}", grossperpallet));
    //                //sht.Range("H" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                //sht.Range("H" + i).Style.NumberFormat.Format = "@";
    //                TotalRolls = TotalRolls + Convert.ToInt16(ds.Tables[0].Rows[ii]["totalroll"]);
    //                //Qty
    //                sht.Range("I" + i).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["grosswt"]));
    //                //  sht.Range("I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    //                Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[ii]["Pcs"]);
    //                //  sht.Range("I" + i).Style.NumberFormat.Format = "@";
    //                //Area
    //                //sht.Range("J" + i).SetValue(ds.Tables[0].Rows[ii]["Area"]);
    //                //sht.Range("J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    //                //sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
    //                Area = Decimal.Parse(Area.ToString()) + Decimal.Parse(ds.Tables[0].Rows[ii]["Area"].ToString());
    //                //
    //                //var _with27 = sht.Range("I" + i);

    //                //_with27.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
    //                //_with27.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
    //                //PO
    //                //   sht.Range("K" + i).Value = ds.Tables[0].Rows[ii]["Pono"];
    //                //
    //                // sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    //                //Borders
    //                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //                sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

    //                i = i + 1;
    //            }
    //            //
    //            sht.Range("A" + i + ":A" + (i + 7)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            sht.Range("K" + i + ":K" + (i + 7)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            //End Of loop
    //            i = i + 7;
    //            //*************** 

    //            //
    //            sht.Range("A" + i).Value = "TOTAL SQ.MTR";
    //            sht.Range("A" + i, "F" + i).Style.Font.FontSize = 10;
    //            sht.Range("A" + i, "F" + i).Style.Font.FontName = "Tahoma";
    //            sht.Range("A" + i, "F" + i).Style.NumberFormat.Format = "@";
    //            sht.Range("A" + i, "F" + i).Style.Font.Bold = true;
    //            // sht.Range("A" + i + ":D" + i).Merge();
    //            sht.Range("A" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

    //            sht.Range("B" + i).Value = String.Format("{0:#,0.00}", Area) + " Sq.Mtr";
    //            //  sht.Range("B" + i, "F" + i).Style.Font.FontSize = 12;
    //            //  sht.Range("B" + i, "F" + i).Style.Font.FontName = "Tahoma";
    //            //  sht.Range("B" + i, "F" + i).Style.NumberFormat.Format = "@";
    //            //  sht.Range("B" + i, "F" + i).Style.Font.Bold = true;
    //            ////  sht.Range("B" + i + ":D" + i).Merge();
    //            sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

    //            sht.Range("C" + i).Value = "Packing List Total:";
    //            sht.Range("C" + i, "F" + i).Style.Font.FontSize = 10;
    //            sht.Range("C" + i, "F" + i).Style.Font.FontName = "Tahoma";
    //            sht.Range("C" + i, "F" + i).Style.NumberFormat.Format = "@";
    //            sht.Range("C" + i, "F" + i).Style.Font.Bold = true;
    //            sht.Range("C" + i + ":D" + i).Merge();
    //            sht.Range("C" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    //            //Total Rolls
    //            //sht.Range("H" + i).Value = TotalRolls;
    //            //sht.Range("H" + i).Style.Font.FontSize = 12;
    //            //sht.Range("H" + i).Style.Font.FontName = "Tahoma";
    //            //sht.Range("H" + i).Style.NumberFormat.Format = "@";
    //            //sht.Range("H" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            ////Total Pcs
    //            sht.Range("E" + i).Value = Pcs;
    //            sht.Range("E" + i).Style.Font.FontSize = 10;
    //            sht.Range("E" + i).Style.Font.FontName = "Tahoma";
    //            sht.Range("E" + i).Style.NumberFormat.Format = "@";
    //            sht.Range("E" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    //            //Area
    //            sht.Range("H" + i).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["grosswt"]);
    //            sht.Range("H" + i).Style.Font.FontSize = 10;
    //            sht.Range("H" + i).Style.Font.FontName = "Tahoma";
    //            sht.Range("H" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    //            sht.Range("H" + i).Style.NumberFormat.Format = "#,###0.00";
    //            //
    //            var _with21 = sht.Range("A" + i + ":K" + i);
    //            _with21.Style.Border.TopBorder = XLBorderStyleValues.Thin;
    //            _with21.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            //
    //            sht.Range("A" + i + ":A" + (i + 10)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            sht.Range("K" + i + ":K" + (i + 10)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

    //            //*************** 
    //            i = i + 2;
    //            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
    //            {

    //                sht.Range("A" + i, "K" + i).Style.Font.FontSize = 10;
    //                sht.Range("A" + i, "K" + i).Style.Font.FontName = "Tahoma";
    //                sht.Range("A" + i, "I" + i).Style.NumberFormat.Format = "@";
    //                sht.Range("K" + i).Style.Font.FontSize = 10;
    //                sht.Range("K" + i).Style.Font.FontName = "Tahoma";
    //                sht.Range("K" + i).Style.NumberFormat.Format = "@";

    //                //Art No.
    //                sht.Range("D" + i).Value = ds.Tables[0].Rows[j]["articleno"];

    //                sht.Range("E" + i, "G" + i).Merge();
    //                sht.Range("E" + i).Value = "(" + (Convert.ToDecimal(ds.Tables[0].Rows[j]["Pcs"]) / Convert.ToDecimal(ds.Tables[0].Rows[j]["pcsperroll"])) + " " + "Pallet x" + " " + ds.Tables[0].Rows[j]["pcsperroll"] + "Pcs/Pallet)";

    //                ////pcs/roll
    //                //sht.Range("G" + i).SetValue(ds.Tables[0].Rows[j]["pcsperroll"]);
    //                //sht.Range("G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //                //sht.Range("G" + i).Style.NumberFormat.Format = "@";                  
    //                ////Qty
    //                //sht.Range("I" + i).SetValue(ds.Tables[0].Rows[j]["Pcs"]);
    //                //sht.Range("I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    //                //Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[j]["Pcs"]);
    //                //sht.Range("I" + i).Style.NumberFormat.Format = "@";                   
    //                //Borders
    //                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

    //                i = i + 1;
    //            }
    //            //
    //            sht.Range("A" + i + ":A" + (i + 7)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            sht.Range("K" + i + ":K" + (i + 7)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

    //            //*************** 

    //            //**********Total
    //            i = i + 10;
    //            //i = 63;
    //            sht.Range("A" + i).Value = "TOTAL PKGS";
    //            sht.Range("A" + (i + 1)).Value = "TOTAL PCS";
    //            sht.Range("A" + (i + 2)).Value = "TOTAL GR. WT";
    //            sht.Range("A" + (i + 3)).Value = "TOTAL NT. WT";
    //            //  sht.Range("A" + (i + 4)).Value = "TOTAL SQ.MTR";
    //            sht.Range("A" + (i + 4)).Value = "TOTAL VOLUME";
    //            sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontSize = 10;
    //            sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontName = "Tahoma";
    //            sht.Range("A" + i + ":A" + (i + 5)).Style.NumberFormat.Format = "@";
    //            sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontName = "Courier New";
    //            //value                
    //            sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontSize = 10;
    //            sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontName = "Tahoma";
    //            sht.Range("C" + i + ":C" + (i + 5)).Style.NumberFormat.Format = "@";


    //            sht.Range("C" + i).SetValue(ds.Tables[0].Rows[0]["Noofrolls"] + " Pallets(Total " + Pcs + " pcs packed in " + ds.Tables[0].Rows[0]["Noofrolls"] + ")");
    //            sht.Range("C" + (i + 1)).SetValue(Pcs);
    //            sht.Range("C" + (i + 2)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["grosswt"]) + " Kgs.");
    //            sht.Range("C" + (i + 3)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["netwt"]) + " Kgs.");
    //            // sht.Range("C" + (i + 4)).SetValue(String.Format("{0:#,0.00}", Area) + " Sq.Mtr");
    //            sht.Range("C" + (i + 4)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["Volume"]) + " CBM");
    //            //
    //            sht.Range("A" + i + ":A" + (i + 5)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            sht.Range("K" + i + ":K" + (i + 5)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            //***********Sig and date
    //            i = i + 6;
    //            //i = 70;
    //            sht.Range("A" + i + ":H" + i).Merge();
    //            sht.Range("I" + i).Value = "Signature/Date";
    //            sht.Range("I" + i).Style.Font.FontSize = 12;
    //            sht.Range("I" + i).Style.Font.FontName = "Tahoma";
    //            sht.Range("I" + i).Style.NumberFormat.Format = "@";

    //            sht.Range("K" + i).Value = "For " + ds.Tables[0].Rows[0]["companyname"];
    //            sht.Range("J" + i + ":K" + i).Style.Font.FontSize = 12;
    //            sht.Range("J" + i + ":K" + i).Style.Font.FontName = "Tahoma";
    //            sht.Range("J" + i + ":K" + i).Style.NumberFormat.Format = "@";
    //            sht.Range("J" + i + ":K" + i).Style.Font.Bold = true;
    //            sht.Range("J" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    //            sht.Range("J" + i + ":K" + i).Style.Font.FontSize = 9;
    //            //sht.Range("J" + i + ":K" + i).Merge();              
    //            sht.Range("I" + i + ":K" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
    //            sht.Range("I" + i + ":I" + (i + 3)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            sht.Range("K" + i + ":K" + (i + 3)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            //**************Declaration
    //            //
    //            sht.Range("A" + i + ":A" + (i + 2)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            i = i + 1;
    //            //i = 71;
    //            //sht.Range("A" + i).Value = "Declaration";
    //            //sht.Range("A" + i + ":B" + i).Merge();
    //            //sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 12;
    //            //sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Tahoma";
    //            //sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";
    //            //sht.Range("A" + i + ":B" + i).Style.Font.Bold = true;
    //            //sht.Range("A" + i + ":B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            //sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

    //            ////value
    //            //i = i + 1;
    //            //sht.Range("A" + i).Value = "We declare that this invoice shows the actual price of the";
    //            //sht.Range("A" + i + ":F" + i).Merge();
    //            //sht.Range("A" + i + ":F" + i).Style.Font.FontSize = 12;
    //            //sht.Range("A" + i + ":F" + i).Style.Font.FontName = "Tahoma";
    //            //sht.Range("A" + i + ":F" + i).Style.NumberFormat.Format = "@";
    //            //sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            //sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

    //            //i = i + 1;
    //            //sht.Range("A" + i).Value = "goods described and that all particulars are true and correct";
    //            //sht.Range("A" + i + ":F" + i).Merge();
    //            //sht.Range("A" + i + ":F" + i).Style.Font.FontSize = 12;
    //            //sht.Range("A" + i + ":F" + i).Style.Font.FontName = "Tahoma";
    //            //sht.Range("A" + i + ":F" + i).Style.NumberFormat.Format = "@";

    //            sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

    //            //**********

    //            sht.Range("I" + i + ":J" + i).Merge();
    //            sht.Range("I" + i + ":J" + i).Value = ds.Tables[0].Rows[0]["InvoiceDate"];
    //            sht.Range("I" + i + ":J" + i).Style.Font.FontName = "Tahoma";
    //            sht.Range("I" + i + ":J" + i).Style.Font.FontSize = 10;
    //            sht.Range("I" + i + ":J" + i).Style.NumberFormat.Format = "dd/MMM/yyyy";
    //            sht.Range("I" + i + ":J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

    //            sht.Range("K" + i).Value = "Auth Sign";
    //            sht.Range("K" + i).Style.Font.FontSize = 10;
    //            sht.Range("K" + i).Style.Font.FontName = "Tahoma";
    //            sht.Range("K" + i).Style.NumberFormat.Format = "@";
    //            sht.Range("K" + i).Style.Font.Bold = true;
    //            sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

    //            //
    //            var _with39 = sht.Range("I" + i);
    //            _with39.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            //
    //            sht.Range("A2:A10").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            sht.Range("A11:A23").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            sht.Range("A" + i + ":K" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //            sht.Range("A1:K1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //            //sht.Range("A2:A10").Style.Border.LeftBorder = XLBorderStyleValues.Thin;                
    //            //sht.Range("F2:F10").Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            //            //************************
    //            var _with2 = sht.Range("A10:K10");
    //            _with2.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //            //
    //            var _with3 = sht.Range("G2:K3");
    //            _with3.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            _with3.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            var _with41 = sht.Range("G2:K2");
    //            _with41.Style.Border.TopBorder = XLBorderStyleValues.Thin;
    //            var _with40 = sht.Range("G3:K3");
    //            _with40.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //            //
    //            var _with4 = sht.Range("G4:I5");
    //            _with4.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            _with4.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            var _with42 = sht.Range("G4:I4");
    //            _with42.Style.Border.TopBorder = XLBorderStyleValues.Thin;
    //            var _with43 = sht.Range("G5:I5");
    //            _with43.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

    //            //
    //            var _with5 = sht.Range("J4:K4");
    //            _with5.Style.Border.TopBorder = XLBorderStyleValues.Thin;
    //            var _with44 = sht.Range("J4:K5");
    //            _with44.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            var _with45 = sht.Range("J5:K5");
    //            _with45.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //            //
    //            var _with6 = sht.Range("G6:K7");
    //            _with6.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            _with6.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            var _with46 = sht.Range("G7:K7");
    //            _with46.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //            //
    //            var _with7 = sht.Range("G8:K10");
    //            _with7.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            _with7.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            var _with47 = sht.Range("G11:K24");
    //            _with47.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            sht.Range("G11:I11").Style.Border.RightBorder = XLBorderStyleValues.None;
    //            var _with8 = sht.Range("A23:F23");
    //            _with8.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //            var _with48 = sht.Range("A11:F23");
    //            _with48.Style.Border.RightBorder = XLBorderStyleValues.Thin;

    //            sht.Range("A11:B11").Style.Border.RightBorder = XLBorderStyleValues.None;
    //            sht.Range("A17:B17").Style.Border.RightBorder = XLBorderStyleValues.None;
    //            sht.Range("G11:H11").Style.Border.RightBorder = XLBorderStyleValues.None;
    //            sht.Range("G17:H19").Style.Border.RightBorder = XLBorderStyleValues.None;

    //            //
    //            var _with9 = sht.Range("A24:C25");
    //            _with9.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            _with9.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            //sht.Range("A24:C24").Style.Border.BottomBorder = XLBorderStyleValues.None;

    //            sht.Range("A25:C25").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

    //            sht.Range("G25:I25").Style.Border.BottomBorder = XLBorderStyleValues.None;

    //            sht.Range("J25:K25").Style.Border.BottomBorder = XLBorderStyleValues.None;
    //            //
    //            var _with10 = sht.Range("D24:F25");
    //            _with10.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //            _with10.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            sht.Range("D24:F24").Style.Border.BottomBorder = XLBorderStyleValues.None;
    //            //
    //            var _with11 = sht.Range("A26:C27");
    //            _with11.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //            _with11.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            _with11.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            sht.Range("A26:C26").Style.Border.BottomBorder = XLBorderStyleValues.None;
    //            //
    //            var _with12 = sht.Range("D26:F27");
    //            _with12.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //            _with12.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            sht.Range("D26:F26").Style.Border.BottomBorder = XLBorderStyleValues.None;
    //            //
    //            var _with13 = sht.Range("A28:C29");
    //            _with13.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            _with13.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            sht.Range("A28:C28").Style.Border.BottomBorder = XLBorderStyleValues.None;
    //            sht.Range("A29:C29").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //            //
    //            var _with14 = sht.Range("D28:F29");
    //            _with14.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //            _with14.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            sht.Range("D28:G28").Style.Border.BottomBorder = XLBorderStyleValues.None;
    //            sht.Range("G28").Style.Border.BottomBorder = XLBorderStyleValues.None;

    //            //
    //            var _with15 = sht.Range("G25:I26");
    //            _with15.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            _with15.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            sht.Range("G25:I25").Style.Border.TopBorder = XLBorderStyleValues.Thin;
    //            sht.Range("G26:I26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //            //
    //            var _with16 = sht.Range("J25:K26");
    //            _with16.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            sht.Range("J25:K25").Style.Border.TopBorder = XLBorderStyleValues.Thin;
    //            sht.Range("J26:K26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //            //
    //            var _with17 = sht.Range("G27:K29");
    //            _with17.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            _with17.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            sht.Range("G29:K29").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //            sht.Range("G28:G29").Style.Border.RightBorder = XLBorderStyleValues.None;
    //            sht.Range("H28:K29").Style.Border.LeftBorder = XLBorderStyleValues.None;
    //            sht.Range("G28:K28").Style.Border.TopBorder = XLBorderStyleValues.Thin;
    //            sht.Range("A30:A37").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //            sht.Range("K30:K37").Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //            //*******************Save
    //            string Fileextension = "xlsx";
    //            string filename = "PackingList -" + validateFilename(ds.Tables[0].Rows[0]["TinvoiceNo"].ToString()) + "." + Fileextension + "";
    //            Path = Server.MapPath("~/Packingexcel/" + filename);
    //            xapp.SaveAs(Path);
    //            xapp.Dispose();

    //            //Download File
    //            Response.Clear();
    //            Response.ContentType = "application/vnd.ms-excel";
    //            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
    //            Response.WriteFile(Path);
    //            Response.End();
    //            //*****
    //            lblmsg.Text = "Packing Excel Format downloaded successfully.";
    //        }
    //        else
    //        {
    //            ScriptManager.RegisterStartupScript(Page, GetType(), "AltR2", "alert('No Record Found to Export')", true);
    //        }

    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    protected void Exporttoexcel_Packing_Kaysons_3()
    {
        lblmsg.Text = "Wait For Excel File to Download";
        int i = 0, Pcs = 0, TotalRolls = 0;
        Decimal Area = 0;
        string Path = "";
        if (!Directory.Exists(Server.MapPath("~/PackingExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/PackingExcel/"));
        }
        try
        {
            string str = @"select *,VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1) as Pcsperroll,VD.Pcs/(VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1)) as TotalRoll From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue + " order by MinrollNo";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("PackingList");
                //************set cell width
                //Page
                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                sht.PageSetup.AdjustTo(70);
                sht.PageSetup.FitToPages(1, 1);
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                sht.PageSetup.VerticalDpi = 300;
                sht.PageSetup.HorizontalDpi = 300;

                //************
                //sht.Column("A").Width = 10.29;
                //sht.Column("B").Width = 8.43;
                //sht.Column("C").Width = 8.43;
                //sht.Column("D").Width = 11.00;
                //sht.Column("E").Width = 9.71;
                //sht.Column("F").Width = 12.57;
                //sht.Column("G").Width = 10.14;
                //sht.Column("H").Width = 13.14;
                //sht.Column("I").Width = 11.71;
                //sht.Column("J").Width = 14.29;
                //sht.Column("K").Width = 19.29;
                ////************
                //sht.Row(1).Height = 15.75;
                //********Header
                sht.Range("A1").Value = "PACKING LIST";
                sht.Range("A1:p1").Style.Font.FontName = "Tahoma";
                sht.Range("A1:P1").Style.Font.FontSize = 12;
                sht.Range("A1:P1").Style.Font.Bold = true;
                sht.Range("A1:P1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:P1").Merge();
                sht.Range("A2:P2").Style.Font.FontName = "Tahoma";
                sht.Range("A2:P2").Style.Font.FontSize = 12;
                sht.Range("A2:P2").Style.Font.Bold = true;
                sht.Range("A2:P2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:P2").Merge();
                using (var a = sht.Range("A2:P2"))
                {
                    //a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A2:P2"))
                {
                    //a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //********Exporter
                sht.Range("A3").Value = "Exporter";
                sht.Range("A3:C3").Style.Font.FontName = "Tahoma";
                sht.Range("A3:C3").Style.Font.FontSize = 10;
                //  sht.Range("A3:C3").Style.Alignment.WrapText = true;
                sht.Range("A3:C3").Merge();
                //*****************

                //CompanyName
                sht.Range("A4").Value = ds.Tables[0].Rows[0]["companyName"];
                sht.Range("A4:C4").Style.Font.FontName = "Tahoma";
                sht.Range("A4:C4").Style.Font.FontSize = 11;
                sht.Range("A4:C4").Style.Font.Bold = true;
                //  sht.Range("A3:C4").Style.Alignment.WrapText = true;
                sht.Range("A4:C4").Merge();
                //Address
                sht.Range("A5").Value = ds.Tables[0].Rows[0]["compaddr1"];
                sht.Range("A5:C5").Style.Font.FontName = "Tahoma";
                sht.Range("A5:C5").Style.Font.FontSize = 10;
                //  sht.Range("A3:C5").Style.Alignment.WrapText = true;
                sht.Range("A5:C5").Merge();
                //address2
                sht.Range("A6").Value = ds.Tables[0].Rows[0]["compaddr2"] + "," + ds.Tables[0].Rows[0]["compaddr3"];
                sht.Range("A6:C6").Style.Font.FontName = "Tahoma";
                sht.Range("A6:C6").Style.Font.FontSize = 10;
                //  sht.Range("A3:C6").Style.Alignment.WrapText = true;
                sht.Range("A6:C6").Merge();
                //TiN No
                sht.Range("A7").Value = "GSTIN: " + ds.Tables[0].Rows[0]["gstno"];
                sht.Range("A7:C7").Style.Font.FontName = "Taho  ma";
                sht.Range("A7:C7").Style.Font.FontSize = 10;
                sht.Range("A7:C7").Style.Font.Bold = true;
                sht.Range("A7:C7").Merge();

                sht.Range("A8").Value = "STATE : " + ds.Tables[0].Rows[0]["rec_state"];
                sht.Range("A8:C8").Style.Font.FontName = "Tahoma";
                sht.Range("A8:C8").Style.Font.FontSize = 10;
                sht.Range("A8:C8").Style.Font.Bold = true;
                sht.Range("A8:C8").Merge();

                sht.Range("A9").Value = "PAN NO. : " + ds.Tables[0].Rows[0]["PANNr"];
                sht.Range("A9:C9").Style.Font.FontName = "Tahoma";
                sht.Range("A9:C9").Style.Font.FontSize = 10;
                sht.Range("A9:C9").Style.Font.Bold = true;
                sht.Range("A9:C9").Merge();



                //**********INvoiceNodate
                sht.Range("I3").Value = "Invoice No. : " + ds.Tables[0].Rows[0]["TInvoiceNo"];
                sht.Range("I3:K3").Style.Font.FontName = "Tahoma";
                sht.Range("I3:K3").Style.Font.FontSize = 10;
                sht.Range("I3:K3").Merge();
                //value
                sht.Range("N3").Value = "Dated " + ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("N3:P3").Merge();
                sht.Range("I7").Value = "Buyer's Order No. / Date";
                sht.Range("I7:K8").Style.Font.FontName = "Tahoma";
                sht.Range("I7:K8").Style.Font.FontSize = 10;
                sht.Range("I7:K7").Merge();
                //value
                sht.Range("L8").SetValue(ds.Tables[0].Rows[0]["TorderNo"]);
                sht.Range("L8:N8").Style.Font.FontName = "Tahoma";
                sht.Range("L8:N8").Style.Font.FontSize = 10;
                sht.Range("L8:N8").Merge();
                sht.Range("L8", "N8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //**************Other Reference(s)
                // sht.Range("G8").Value = "Other Reference(s)";
                sht.Range("I10:K10").Merge();
                sht.Range("I10").Value = "SUPPLIER NO.: 21899";
                sht.Range("I11").Value = "CONSIGNMENT NO.:" + ds.Tables[0].Rows[0]["otherref"];
                sht.Range("N10").Value = "DESTN. CODE";
                sht.Range("N10:P10").Merge();
                sht.Range("I10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("I10:K11").Style.Font.FontName = "Tahoma";
                sht.Range("I10:K11").Style.Font.FontSize = 10;
                // sht.Range("F10:H11").Merge();

                //Border for first section
                sht.Range("A2:A63").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("P2:P63").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("I3:P3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("I3:I11").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("N3").Style.Alignment.WrapText = true;
                sht.Range("N9").Style.Alignment.WrapText = true;
                sht.Range("A11:P11").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //sht.Range("F3:k3").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                using (var a = sht.Range("I10:P10"))
                {
                    //a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A2:P2"))
                {
                    //a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                // sht.Range("D10:J10").Merge();
                // sht.Range("J10:K10").Merge();
                // sht.Range("G11:K11").Merge();
                //*************Consignee
                sht.Range("A12").Value = "Consignee";
                sht.Range("C12").Value = ds.Tables[0].Rows[0]["consignee_dt"];
                sht.Range("A12:B12").Style.Font.FontName = "Tahoma";
                sht.Range("A12:B12").Style.Font.FontSize = 10;

                sht.Range("C12:F12").Style.Font.FontName = "Tahoma";
                sht.Range("C12:F12").Style.Font.FontSize = 10;
                sht.Range("C12:F12").Style.Font.Bold = true;

                sht.Range("A12:B12").Merge();
                sht.Range("C12:F12").Merge();
                sht.Range("C12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //value
                sht.Range("A13").Value = ds.Tables[0].Rows[0]["consignee"];
                sht.Range("A13:F13").Style.Font.FontName = "Tahoma";
                sht.Range("A13:F13").Style.Font.FontSize = 10;
                sht.Range("A13:F13").Style.Font.Bold = true;
                sht.Range("A13:F13").Merge();
                //**
                sht.Range("A14").Value = ds.Tables[0].Rows[0]["consignee_address"];
                sht.Range("A14:F17").Style.Font.FontName = "Tahoma";
                sht.Range("A14:F17").Style.Font.FontSize = 10;
                sht.Range("A14", "F17").Style.Alignment.WrapText = true;
                sht.Range("A14", "F17").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A14:F17").Merge();
                //***********Notify
                sht.Range("A18").Value = "Notify Party";
                sht.Range("C18").Value = ds.Tables[0].Rows[0]["Notifyparty_dt"];
                sht.Range("A18:B18").Style.Font.FontName = "Tahoma";
                sht.Range("A18:B18").Style.Font.FontSize = 10;

                sht.Range("A18:B18").Style.Font.SetUnderline();
                sht.Range("C18:F18").Style.Font.FontName = "Tahoma";
                sht.Range("C18:F18").Style.Font.FontSize = 10;
                sht.Range("C18:F18").Style.Font.Bold = true;

                sht.Range("A18:B18").Merge();
                sht.Range("C18:F18").Merge();
                sht.Range("C18").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //value
                sht.Range("A19").Value = ds.Tables[0].Rows[0]["Notifyparty"];
                sht.Range("A19:F19").Style.Font.FontName = "Tahoma";
                sht.Range("A19:F19").Style.Font.FontSize = 10;
                sht.Range("A19:F19").Style.Font.Bold = true;
                sht.Range("A19:F19").Merge();
                //
                sht.Range("A20").Value = ds.Tables[0].Rows[0]["notifyparty_address"];
                sht.Range("A20:F24").Style.Font.FontName = "Tahoma";
                sht.Range("A20:F24").Style.Font.FontSize = 10;
                sht.Range("A20", "F24").Style.Alignment.WrapText = true;
                sht.Range("A20", "F24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A20:F24").Merge();

                sht.Range("I13").Value = "Buyer (If other than Consignee)";
                sht.Range("I13:K13").Style.Font.FontName = "Tahoma";
                sht.Range("I13:K13").Style.Font.FontSize = 10;
                sht.Range("I13:K13").Merge();

                sht.Range("I14").Value = "" + ds.Tables[0].Rows[0]["buyer_otherthanconsignee"];
                sht.Range("I14:K14").Style.Font.FontName = "Tahoma";
                sht.Range("I14:K14").Style.Font.FontSize = 10;
                sht.Range("I14:K14").Merge();
                //*
                sht.Range("I15").Value = ds.Tables[0].Rows[0]["otherthanconsignee_address"];
                sht.Range("I15:K17").Style.Font.FontName = "Tahoma";
                sht.Range("I15:K17").Style.Font.FontSize = 10;
                sht.Range("I15:K17").Style.Alignment.WrapText = true;
                sht.Range("I15:K17").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("I15:K17").Merge();

                sht.Range("I17").Value = "2.Paying Agent : " + ds.Tables[0].Rows[0]["payingagent"];
                sht.Range("I17:K17").Style.Font.FontName = "Tahoma";
                sht.Range("I17:K17").Style.Font.FontSize = 10;
                sht.Range("I17:K17").Style.Font.Bold = true;
                sht.Range("I17:K17").Merge();
                //*
                sht.Range("J18").Value = ds.Tables[0].Rows[0]["payingagent_address"];
                sht.Range("J18:L20").Style.Font.FontName = "Tahoma";
                sht.Range("J18:L20").Style.Font.FontSize = 10;
                sht.Range("J18:L20").Style.Alignment.WrapText = true;
                sht.Range("J18:L20").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("J18:L20").Merge();

                sht.Range("A25:P25").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("H12:H24").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //*******3.
                //  //***********Pre-carriage By
                sht.Range("A25").Value = "Pre-Carriage By";
                sht.Range("A25:C25").Style.Font.FontName = "Tahoma";
                sht.Range("A25:C25").Style.Font.FontSize = 10;
                sht.Range("A25:C25").Merge();
                //value
                sht.Range("A26").Value = ds.Tables[0].Rows[0]["Pre_carriageby"];
                sht.Range("A26:C26").Style.Font.FontName = "Tahoma";
                sht.Range("A26:C26").Style.Font.FontSize = 10;
                sht.Range("A26:C26").Merge();
                sht.Range("A26:C26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Place of Receipt by Pre-Carrier
                sht.Range("D25").Value = "Place of Receipt by Pre-Carrier";
                sht.Range("D25:F25").Style.Font.FontName = "Tahoma";
                sht.Range("D25:F25").Style.Font.FontSize = 10;
                sht.Range("D25:F25").Merge();
                //value
                sht.Range("D26").Value = ds.Tables[0].Rows[0]["receiptby"];
                sht.Range("D26:F26").Style.Font.FontName = "Tahoma";
                sht.Range("D26:F26").Style.Font.FontSize = 10;
                sht.Range("D26:F26").Merge();
                sht.Range("D26:F26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Vessel/Flight No
                sht.Range("A27").Value = "Vessel/Flight No";
                sht.Range("A27:C27").Style.Font.FontName = "Tahoma";
                sht.Range("A27:C27").Style.Font.FontSize = 10;
                sht.Range("A27:C27").Merge();
                //value
                sht.Range("A28").Value = ds.Tables[0].Rows[0]["vessel_flightno"];
                sht.Range("A28:C28").Style.Font.FontName = "Tahoma";
                sht.Range("A28:C28").Style.Font.FontSize = 10;
                sht.Range("A28:C28").Merge();
                sht.Range("A28:C28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Loading
                sht.Range("D27").Value = "Port of Loading";
                sht.Range("D27:F27").Style.Font.FontName = "Tahoma";
                sht.Range("D27:F27").Style.Font.FontSize = 10;
                sht.Range("D27:F27").Merge();
                //value
                sht.Range("D28").Value = ds.Tables[0].Rows[0]["portload"];
                sht.Range("D28:F28").Style.Font.FontName = "Tahoma";
                sht.Range("D28:F28").Style.Font.FontSize = 10;
                sht.Range("D28:F28").Merge();
                sht.Range("D28:F28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Discharge
                sht.Range("A29").Value = "Port of Discharge";
                sht.Range("A29:C29").Style.Font.FontName = "Tahoma";
                sht.Range("A29:C29").Style.Font.FontSize = 10;
                sht.Range("A29:C29").Merge();
                //value
                sht.Range("A30").Value = ds.Tables[0].Rows[0]["portunload"];
                sht.Range("A30:C30").Style.Font.FontName = "Tahoma";
                sht.Range("A30:C30").Style.Font.FontSize = 10;
                sht.Range("A30:C30").Merge();
                sht.Range("A30:C30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Final Destination
                sht.Range("D29").Value = "Final Destination";
                sht.Range("D29:F29").Style.Font.FontName = "Tahoma";
                sht.Range("D29:F29").Style.Font.FontSize = 10;
                sht.Range("D29:F29").Merge();
                //value
                sht.Range("D30").Value = ds.Tables[0].Rows[0]["Destinationadd"];
                sht.Range("D30:F30").Style.Font.FontName = "Tahoma";
                sht.Range("D30:F30").Style.Font.FontSize = 10;
                sht.Range("D30:F30").Merge();
                sht.Range("D30:F30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Origin of Goods
                sht.Range("I26").Value = "Country of Origin of Goods";
                sht.Range("I26:K26").Style.Font.FontName = "Tahoma";
                sht.Range("I26:K26").Style.Font.FontSize = 10;
                sht.Range("I26:K26").Merge();
                //value
                sht.Range("I27").Value = ds.Tables[0].Rows[0]["countryoforigin"];
                sht.Range("I27:K27").Style.Font.FontName = "Tahoma";
                sht.Range("I27:K27").Style.Font.FontSize = 10;
                sht.Range("I27:K27").Merge();
                sht.Range("I27:K27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Final Destination
                sht.Range("L26").Value = "Country of Final Destination";
                sht.Range("L26:N26").Style.Font.FontName = "Tahoma";
                sht.Range("L26:N26").Style.Font.FontSize = 10;
                sht.Range("L26:N26").Merge();
                //value
                sht.Range("L27").Value = ds.Tables[0].Rows[0]["countryoffinaldest"];
                sht.Range("L27:N27").Style.Font.FontName = "Tahoma";
                sht.Range("L27:N27").Style.Font.FontSize = 10;
                sht.Range("L27:N27").Style.NumberFormat.Format = "@";
                sht.Range("L27:N27").Merge();
                sht.Range("L27:N27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //***********Terms of Delivery and Payment
                sht.Range("I28").Value = "Terms of Delivery and Payment";
                sht.Range("I28:K28").Style.Font.FontName = "Tahoma";
                sht.Range("I28:K28").Style.Font.FontSize = 10;
                sht.Range("I28:K28").Merge();
                //value
                sht.Range("K29").Value = ds.Tables[0].Rows[0]["Deliveryterm"];
                sht.Range("I29").Value = "Terms of Payment";
                sht.Range("I29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("I30").Value = "Delivery WEEK :";

                // sht.Range("G30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //sht.Range("J28").Value = "Pre Authenticated";
                //sht.Range("J29").Value = "For"+ ds.Tables[0].Rows[0]["companyName"];
                //sht.Range("J30").Value = "Auth. Sign.";
                //sht.Range("H29:K30").Style.Font.FontName = "Tahoma";
                //sht.Range("H29:K30").Style.Font.FontSize = 10;
                //sht.Range("H29:K30").Style.NumberFormat.Format = "@";
                //                sht.Range("H29:K30").Style.Alignment.WrapText = true;
                //   sht.Range("H29:K30").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("H25:H31").Style.Border.RightBorder = XLBorderStyleValues.Thin;


                sht.Range("A26:H26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A28:H28").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("I27:P27").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                sht.Range("K25:K30").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A32:P32").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //sht.Range("G29:I29").Merge();
                //sht.Range("G30:I30").Merge();
                //sht.Range("J28:K28").Merge();
                //sht.Range("J29:K29").Merge();
                //sht.Range("J30:K30").Merge();
                // sht.Range("J28:K30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //*****************Nos
                sht.Range("A32:B32").Value = "Marks Nos./Container No.";
                sht.Range("A32:B32").Merge();
                sht.Range("D32").Value = "No and kind of Packages";
                sht.Range("D32:F32").Merge();
                sht.Range("H32").Value = "Description of Goods";
                sht.Range("H32:L32").Merge();
                //sht.Range("I31").Value = "Quantity";
                //sht.Range("J31").Value = "Rate";
                //sht.Range("k31").Value = "Amount";
                sht.Range("A32:L32").Style.Font.FontName = "Tahoma";
                sht.Range("A32:L32").Style.Font.FontSize = 10;
                sht.Range("A32:L32").Style.NumberFormat.Format = "@";
                sht.Range("L32").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //values
                sht.Range("A33:H33").Merge();
                sht.Range("A33").Value = "IKEA / FINAL DESTN.";
                sht.Range("A33").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A34:H38").Style.Font.FontName = "Tahoma";
                sht.Range("A34:H38").Style.Font.FontSize = 10;
                sht.Range("A34:H38").Style.NumberFormat.Format = "@";

                sht.Range("A34").SetValue("No." + ds.Tables[0].Compute("Min(MinrollNo)", "") + "To: " + ds.Tables[0].Compute("Max(Maxrollno)", ""));
                sht.Range("A34:B34").Merge();
                sht.Range("D34").Value = ds.Tables[0].Rows[0]["Noofrolls"] + " Pallets";
                sht.Range("D34:F34").Merge();
                sht.Range("H34").Value = ds.Tables[0].Rows[0]["descriptionofgoods"];
                sht.Range("H34:L36").Style.Alignment.WrapText = true;
                sht.Range("H34:L36").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("H34:L36").Merge();
                sht.Range("A36:P36").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //            //**********************Details
                //********Hader

                sht.Range("A37").Value = "P.O.#";
                sht.Range("A37:P37").Style.Font.FontSize = 10;
                sht.Range("A37:P37").Style.Font.FontName = "Tahoma";
                sht.Range("A37:P37").Style.NumberFormat.Format = "@";
                sht.Range("A37:P37").Style.Font.Bold = true;
                sht.Range("A37").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Range("B37").Value = "Pallet";
                sht.Range("B37:C37").Merge();

                sht.Range("D37").Value = "PCS/Pallet";
                sht.Range("E37").Value = "ART.NOS.";

                sht.Range("F37").Value = "Description, Colour & Size";
                sht.Range("F37:H37").Merge();
                // sht.Range("D38").Value = "ART.NO.";
                //sht.Range("D38:E38")..Merge();
                sht.Range("I37").Value = "Total Pcs";
                sht.Range("J37").Value = "TOTAL NT. WT.";
                sht.Range("J37:K37").Merge();
                sht.Range("J37").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("L37").Value = "NT WT/PALLET";
                sht.Range("L37:M37").Merge();
                sht.Range("L37").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("N37").Value = "TOTAL GR.WT.";

                sht.Range("N37").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("N37:O37").Merge();
                sht.Range("P37").Value = "GR WT/PALLET";
                sht.Range("P37").Style.Alignment.WrapText = true;
                //sht.Range("J38").Value = "Area Sq. Mtr";
                //sht.Range("K38").Value = "P.O.#";
                sht.Range("P37").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A37:A39").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("C37:C39").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("D37:D39").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("E37:E39").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("H37:H39").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("I37:I39").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("K37:K39").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("M37:M39").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("O37:O39").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //***********generate Loop
                i = 39;
                ViewState["lastpallet"] = 1;
                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                {
                    int lastvalue = 0, upto = 0;

                    sht.Range("A" + i, "P" + i).Style.Font.FontSize = 10;
                    sht.Range("A" + i, "P" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("A" + i, "I" + i).Style.NumberFormat.Format = "@";
                    sht.Range("K" + i).Style.Font.FontSize = 10;
                    sht.Range("K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("K" + i).Style.NumberFormat.Format = "@";
                    //pono
                    sht.Range("A" + i).Value = ds.Tables[0].Rows[ii]["Pono"];
                    sht.Range("A" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    var _withPONO = sht.Range("A" + i);
                    _withPONO.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //Article Name
                    int palletpcs = (Convert.ToInt16(ds.Tables[0].Rows[ii]["Pcs"]) / Convert.ToInt16(ds.Tables[0].Rows[ii]["pcsperroll"]));

                    if (ViewState["lastpallet"] != null)
                    {
                        lastvalue = Convert.ToInt16(ViewState["lastpallet"]);
                        if (ii == 0)
                        {
                            upto = palletpcs;
                        }
                        else { lastvalue = lastvalue + 1; upto = lastvalue + palletpcs; }


                    }
                    sht.Range("B" + i).Value = lastvalue + "   " + upto + "   " + palletpcs + "Pallets)";
                    sht.Range("B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("B" + i, "C" + i).Merge();
                    ViewState["lastpallet"] = upto;
                    var _withPALLETLOOP = sht.Range("C" + i);
                    _withPALLETLOOP.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    sht.Range("D" + i).Value = ds.Tables[0].Rows[ii]["pcsperroll"];
                    sht.Range("D" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    var _witHPPC = sht.Range("D" + i);
                    _witHPPC.Style.Border.RightBorder = XLBorderStyleValues.Thin;


                    sht.Range("E" + i).Value = ds.Tables[0].Rows[ii]["articleno"];
                    sht.Range("E" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    var _witHARTICAL = sht.Range("E" + i);
                    _withPONO.Style.Border.RightBorder = XLBorderStyleValues.Thin;



                    sht.Range("F" + i).Value = ds.Tables[0].Rows[ii]["Design"] + "," + ds.Tables[0].Rows[ii]["Color"] + " , " + ds.Tables[0].Rows[ii]["width"] + "x" + ds.Tables[0].Rows[ii]["Length"];
                    sht.Range("F" + i, "H" + (i + 1)).Merge();
                    sht.Range("F" + i).Style.Alignment.WrapText = true;
                    var _withDESC = sht.Range("H" + i);
                    _withDESC.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //Art No.
                    //  sht.Range("D" + i).Value = ds.Tables[0].Rows[ii]["articleno"];
                    //Colour
                    sht.Range("I" + i).Value = ds.Tables[0].Rows[ii]["Pcs"];
                    var _withPCS = sht.Range("I" + i);
                    _withPCS.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //Size
                    double netperpallet = Convert.ToDouble(ds.Tables[0].Rows[0]["netwt"]) / Convert.ToInt16(ds.Tables[0].Rows[ii]["Pcs"]);
                    sht.Range("J" + i).Value = String.Format("{0:#,0.000}", netperpallet);
                    sht.Range("J" + i, "K" + i).Merge();
                    var _withTOTNWT = sht.Range("K" + i);
                    _withTOTNWT.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //pcs/roll
                    sht.Range("L" + i).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["netwt"]));
                    sht.Range("L" + i, "M" + i).Merge();
                    var _withNWT = sht.Range("M" + i);
                    _withNWT.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //sht.Range("G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //sht.Range("G" + i).Style.NumberFormat.Format = "@";
                    //Total rolls
                    double grossperpallet = Convert.ToDouble(ds.Tables[0].Rows[0]["grosswt"]) / Convert.ToInt16(ds.Tables[0].Rows[ii]["Pcs"]);
                    sht.Range("N" + i).SetValue(String.Format("{0:#,0.000}", grossperpallet));
                    sht.Range("N" + i, "O" + i).Merge();
                    var _withgNWT = sht.Range("O" + i);
                    _withgNWT.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //sht.Range("H" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //sht.Range("H" + i).Style.NumberFormat.Format = "@";
                    TotalRolls = TotalRolls + Convert.ToInt16(ds.Tables[0].Rows[ii]["totalroll"]);
                    //Qty
                    sht.Range("P" + i).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["grosswt"]));
                    //  sht.Range("I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[ii]["Pcs"]);
                    //  sht.Range("I" + i).Style.NumberFormat.Format = "@";
                    //Area
                    //sht.Range("J" + i).SetValue(ds.Tables[0].Rows[ii]["Area"]);
                    //sht.Range("J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                    Area = Decimal.Parse(Area.ToString()) + Decimal.Parse(ds.Tables[0].Rows[ii]["Area"].ToString());
                    //
                    //var _with27 = sht.Range("I" + i);

                    //_with27.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
                    //_with27.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
                    //PO
                    //   sht.Range("K" + i).Value = ds.Tables[0].Rows[ii]["Pono"];
                    //
                    // sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //Borders
                    //sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //sht.Range("I" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    i = i + 1;
                }

                var _withPONOlast = sht.Range("A" + (i) + ":A" + (i + 1));
                _withPONOlast.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //var _withDESClast = sht.Range("H" + i);
                //_withDESClast.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _withlast = sht.Range("A" + (i + 2) + ":P" + (i + 2));
                //   _with29.Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                _withlast.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                var _withPALLET = sht.Range("C" + i);
                _withPALLET.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _withARTlast = sht.Range("D" + i);
                _withARTlast.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _withHSNlast = sht.Range("E" + (i));
                _withHSNlast.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _withQTYlast = sht.Range("H" + (i));
                _withQTYlast.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _withRATElast = sht.Range("I" + (i));
                _withRATElast.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _withAMTlast = sht.Range("K" + (i));
                _withAMTlast.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _withNTWT = sht.Range("M" + (i));
                _withNTWT.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _withGTWT = sht.Range("O" + (i));
                _withGTWT.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                //sht.Range("A" + i + ":A" + (i + 7)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("K" + i + ":K" + (i + 7)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //  ////End Of loop
                i = i + 2;
                //*************** 

                //
                sht.Range("A" + i).Value = "TOTAL SQ.MTR";
                sht.Range("A" + i, "F" + i).Style.Font.FontSize = 10;
                sht.Range("A" + i, "F" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i, "F" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i, "F" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":B" + i).Merge();
                sht.Range("A" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("C" + i).Value = String.Format("{0:#,0.00}", Area) + "";
                //  sht.Range("B" + i, "F" + i).Style.Font.FontSize = 12;
                //  sht.Range("B" + i, "F" + i).Style.Font.FontName = "Tahoma";
                //  sht.Range("B" + i, "F" + i).Style.NumberFormat.Format = "@";
                //  sht.Range("B" + i, "F" + i).Style.Font.Bold = true;
                ////  sht.Range("B" + i + ":D" + i).Merge();
                //sht.Range("C" + i).Style.Alignment.WrapText = true;

                sht.Range("H" + i).Value = "Total:";
                sht.Range("H" + i, "F" + i).Style.Font.FontSize = 10;
                sht.Range("H" + i, "F" + i).Style.Font.FontName = "Tahoma";
                sht.Range("H" + i, "F" + i).Style.NumberFormat.Format = "@";
                sht.Range("H" + i, "F" + i).Style.Font.Bold = true;
                //  sht.Range("D" + i + ":D" + i).Merge();
                //sht.Range("C" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //Total Rolls
                //sht.Range("H" + i).Value = TotalRolls;
                //sht.Range("H" + i).Style.Font.FontSize = 12;
                //sht.Range("H" + i).Style.Font.FontName = "Tahoma";
                //sht.Range("H" + i).Style.NumberFormat.Format = "@";
                //sht.Range("H" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ////Total Pcs
                sht.Range("I" + i).Value = Pcs;
                sht.Range("I" + i).Style.Font.FontSize = 10;
                sht.Range("I" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i).Style.NumberFormat.Format = "@";
                sht.Range("I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //Area
                sht.Range("P" + i).Value = String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["grosswt"]);
                sht.Range("P" + i).Style.Font.FontSize = 10;
                sht.Range("P" + i).Style.Font.FontName = "Tahoma";
                sht.Range("P" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("P" + i).Style.NumberFormat.Format = "#,###0.00";



                var _withHSNTOTAL = sht.Range("I" + (i - 1) + ":I" + (i));
                _withHSNTOTAL.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                _withHSNTOTAL.Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                var _withAMTTOTAL = sht.Range("K" + (i - 1) + ":K" + (i));
                _withAMTTOTAL.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _withQTYTOTAL = sht.Range("M" + (i - 1) + ":M" + (i));
                _withQTYTOTAL.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _withRATETOTAL = sht.Range("O" + (i - 1) + ":O" + (i));
                _withRATETOTAL.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                var _with21 = sht.Range("A" + i + ":P" + i);
                // _with21.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                _with21.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ////
                //  //sht.Range("A" + i + ":A" + (i + 10)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //  //sht.Range("K" + i + ":K" + (i + 10)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //  //*************** 
                i = i + 2;
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {

                    sht.Range("A" + i, "K" + i).Style.Font.FontSize = 10;
                    sht.Range("A" + i, "K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("A" + i, "I" + i).Style.NumberFormat.Format = "@";
                    sht.Range("K" + i).Style.Font.FontSize = 10;
                    sht.Range("K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("K" + i).Style.NumberFormat.Format = "@";

                    //Art No.
                    sht.Range("D" + i).Value = ds.Tables[0].Rows[j]["articleno"];

                    sht.Range("E" + i, "G" + i).Merge();
                    sht.Range("E" + i).Value = "(" + (Convert.ToDecimal(ds.Tables[0].Rows[j]["Pcs"]) / Convert.ToDecimal(ds.Tables[0].Rows[j]["pcsperroll"])) + " " + "Pallet x" + " " + ds.Tables[0].Rows[j]["pcsperroll"] + "Pcs/Pallet)";

                    ////pcs/roll
                    //sht.Range("G" + i).SetValue(ds.Tables[0].Rows[j]["pcsperroll"]);
                    //sht.Range("G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //sht.Range("G" + i).Style.NumberFormat.Format = "@";                  
                    ////Qty
                    //sht.Range("I" + i).SetValue(ds.Tables[0].Rows[j]["Pcs"]);
                    //sht.Range("I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[j]["Pcs"]);
                    //sht.Range("I" + i).Style.NumberFormat.Format = "@";                   
                    //Borders
                    //sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    i = i + 1;
                }
                //  ////
                //  //sht.Range("A" + i + ":A" + (i + 7)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //  //sht.Range("K" + i + ":K" + (i + 7)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //  ////*************** 

                //  ////**********Total
                i = i + 10;
                //i = 63;
                sht.Range("A" + i).Value = "TOTAL PKGS";
                sht.Range("A" + (i + 1)).Value = "TOTAL PCS";
                sht.Range("A" + (i + 2)).Value = "TOTAL GR. WT";
                sht.Range("A" + (i + 3)).Value = "TOTAL NT. WT";
                //  sht.Range("A" + (i + 4)).Value = "TOTAL SQ.MTR";
                sht.Range("A" + (i + 4)).Value = "TOTAL VOLUME";
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontSize = 10;
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":A" + (i + 5)).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontName = "Courier New";
                //value                
                sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontSize = 10;
                sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontName = "Tahoma";
                sht.Range("C" + i + ":C" + (i + 5)).Style.NumberFormat.Format = "@";


                sht.Range("C" + i).SetValue(ds.Tables[0].Rows[0]["Noofrolls"] + " Pallets(Total " + Pcs + " pcs packed in " + ds.Tables[0].Rows[0]["Noofrolls"] + ")");
                sht.Range("C" + (i + 1)).SetValue(Pcs);
                sht.Range("C" + (i + 2)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["grosswt"]) + " Kgs.");
                sht.Range("C" + (i + 3)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["netwt"]) + " Kgs.");
                // sht.Range("C" + (i + 4)).SetValue(String.Format("{0:#,0.00}", Area) + " Sq.Mtr");
                sht.Range("C" + (i + 4)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["Volume"]) + " CBM");
                //
                //sht.Range("A" + i + ":A" + (i + 5)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("K" + i + ":K" + (i + 5)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //***********Sig and date
                i = i + 4;
                //i = 70;
                sht.Range("A" + i + ":H" + i).Merge();
                sht.Range("N" + i).Value = "Signature/Date";
                sht.Range("N" + i + ":O" + i).Merge();
                sht.Range("N" + i).Style.Font.FontSize = 10;
                sht.Range("N" + i).Style.Font.FontName = "Tahoma";
                sht.Range("N" + i).Style.NumberFormat.Format = "@";

                sht.Range("N" + (i + 1)).Value = "For " + ds.Tables[0].Rows[0]["companyname"];
                sht.Range("N" + (i + 1) + ":O" + (i + 1)).Style.Font.FontSize = 10;
                sht.Range("N" + (i + 1) + ":O" + (i + 1)).Style.Font.FontName = "Tahoma";
                sht.Range("N" + (i + 1) + ":O" + (i + 1)).Style.NumberFormat.Format = "@";
                sht.Range("N" + (i + 1) + ":O" + (i + 1)).Style.Font.Bold = true;
                // sht.Range("N" + (i + 1) + ":O" + (i + 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("N" + (i + 1) + ":O" + (i + 1)).Style.Font.FontSize = 9;
                sht.Range("A" + (i + 3) + ":p" + (i + 3)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("N" + (i + 1) + ":P" + (i + 1)).Merge();
                sht.Range("N" + i + ":P" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("N" + i + ":N" + (i + 3)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //  sht.Range("O" + i + ":O" + (i + 3)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                // **************Declaration
                ////
                //sht.Range("A" + i + ":A" + (i + 2)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //i = i + 1;
                ////i = 71;
                ////sht.Range("A" + i).Value = "Declaration";
                ////sht.Range("A" + i + ":B" + i).Merge();
                ////sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 12;
                ////sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Tahoma";
                ////sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";
                ////sht.Range("A" + i + ":B" + i).Style.Font.Bold = true;
                ////sht.Range("A" + i + ":B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ////sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                //////value
                ////i = i + 1;
                ////sht.Range("A" + i).Value = "We declare that this invoice shows the actual price of the";
                ////sht.Range("A" + i + ":F" + i).Merge();
                ////sht.Range("A" + i + ":F" + i).Style.Font.FontSize = 12;
                ////sht.Range("A" + i + ":F" + i).Style.Font.FontName = "Tahoma";
                ////sht.Range("A" + i + ":F" + i).Style.NumberFormat.Format = "@";
                ////sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ////sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                ////i = i + 1;
                ////sht.Range("A" + i).Value = "goods described and that all particulars are true and correct";
                ////sht.Range("A" + i + ":F" + i).Merge();
                ////sht.Range("A" + i + ":F" + i).Style.Font.FontSize = 12;
                ////sht.Range("A" + i + ":F" + i).Style.Font.FontName = "Tahoma";
                ////sht.Range("A" + i + ":F" + i).Style.NumberFormat.Format = "@";

                //sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                ////**********

                //sht.Range("I" + i + ":J" + i).Merge();
                //sht.Range("I" + i + ":J" + i).Value = ds.Tables[0].Rows[0]["InvoiceDate"];
                //sht.Range("I" + i + ":J" + i).Style.Font.FontName = "Tahoma";
                //sht.Range("I" + i + ":J" + i).Style.Font.FontSize = 10;
                //sht.Range("I" + i + ":J" + i).Style.NumberFormat.Format = "dd/MMM/yyyy";
                //sht.Range("I" + i + ":J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //sht.Range("K" + i).Value = "Auth Sign";
                //sht.Range("K" + i).Style.Font.FontSize = 10;
                //sht.Range("K" + i).Style.Font.FontName = "Tahoma";
                //sht.Range("K" + i).Style.NumberFormat.Format = "@";
                //sht.Range("K" + i).Style.Font.Bold = true;
                //sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                ////
                //var _with39 = sht.Range("I" + i);
                //_with39.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ////
                //sht.Range("A2:A10").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("A11:A23").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("A" + i + ":K" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //sht.Range("A1:K1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ////sht.Range("A2:A10").Style.Border.LeftBorder = XLBorderStyleValues.Thin;                
                ////sht.Range("F2:F10").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ////            //************************
                //var _with2 = sht.Range("A10:K10");
                //_with2.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ////
                //var _with3 = sht.Range("G2:K3");
                //_with3.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //_with3.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //var _with41 = sht.Range("G2:K2");
                //_with41.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //var _with40 = sht.Range("G3:K3");
                //_with40.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ////
                //var _with4 = sht.Range("G4:I5");
                //_with4.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //_with4.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //var _with42 = sht.Range("G4:I4");
                //_with42.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //var _with43 = sht.Range("G5:I5");
                //_with43.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                ////
                //var _with5 = sht.Range("J4:K4");
                //_with5.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //var _with44 = sht.Range("J4:K5");
                //_with44.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //var _with45 = sht.Range("J5:K5");
                //_with45.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ////
                //var _with6 = sht.Range("G6:K7");
                //_with6.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //_with6.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //var _with46 = sht.Range("G7:K7");
                //_with46.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ////
                //var _with7 = sht.Range("G8:K10");
                //_with7.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //_with7.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //var _with47 = sht.Range("G11:K24");
                //_with47.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //sht.Range("G11:I11").Style.Border.RightBorder = XLBorderStyleValues.None;
                //var _with8 = sht.Range("A23:F23");
                //_with8.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //var _with48 = sht.Range("A11:F23");
                //_with48.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //sht.Range("A11:B11").Style.Border.RightBorder = XLBorderStyleValues.None;
                //sht.Range("A17:B17").Style.Border.RightBorder = XLBorderStyleValues.None;
                //sht.Range("G11:H11").Style.Border.RightBorder = XLBorderStyleValues.None;
                //sht.Range("G17:H19").Style.Border.RightBorder = XLBorderStyleValues.None;

                ////
                //var _with9 = sht.Range("A24:C25");
                //_with9.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with9.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ////sht.Range("A24:C24").Style.Border.BottomBorder = XLBorderStyleValues.None;

                //sht.Range("A25:C25").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                //sht.Range("G25:I25").Style.Border.BottomBorder = XLBorderStyleValues.None;

                //sht.Range("J25:K25").Style.Border.BottomBorder = XLBorderStyleValues.None;
                ////
                //var _with10 = sht.Range("D24:F25");
                //_with10.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //_with10.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //sht.Range("D24:F24").Style.Border.BottomBorder = XLBorderStyleValues.None;
                ////
                //var _with11 = sht.Range("A26:C27");
                //_with11.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //_with11.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //_with11.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("A26:C26").Style.Border.BottomBorder = XLBorderStyleValues.None;
                ////
                //var _with12 = sht.Range("D26:F27");
                //_with12.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //_with12.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //sht.Range("D26:F26").Style.Border.BottomBorder = XLBorderStyleValues.None;
                ////
                //var _with13 = sht.Range("A28:C29");
                //_with13.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //_with13.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("A28:C28").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //sht.Range("A29:C29").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ////
                //var _with14 = sht.Range("D28:F29");
                //_with14.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //_with14.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //sht.Range("D28:G28").Style.Border.BottomBorder = XLBorderStyleValues.None;
                //sht.Range("G28").Style.Border.BottomBorder = XLBorderStyleValues.None;

                ////
                //var _with15 = sht.Range("G25:I26");
                //_with15.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //_with15.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("G25:I25").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //sht.Range("G26:I26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ////
                //var _with16 = sht.Range("J25:K26");
                //_with16.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //sht.Range("J25:K25").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //sht.Range("J26:K26").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ////
                //var _with17 = sht.Range("G27:K29");
                //_with17.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //_with17.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //sht.Range("G29:K29").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //sht.Range("G28:G29").Style.Border.RightBorder = XLBorderStyleValues.None;
                //sht.Range("H28:K29").Style.Border.LeftBorder = XLBorderStyleValues.None;
                //sht.Range("G28:K28").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //sht.Range("A30:A37").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //sht.Range("K30:K37").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //*******************Save
                string Fileextension = "xlsx";
                string filename = "PackingList -" + validateFilename(ds.Tables[0].Rows[0]["TinvoiceNo"].ToString()) + "." + Fileextension + "";
                Path = Server.MapPath("~/Packingexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
                //*****
                lblmsg.Text = "Packing Excel Format downloaded successfully.";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "AltR2", "alert('No Record Found to Export')", true);
            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void Exporttoexcel_Kaysons_4()
    {
        lblmsg.Text = "Wait For Excel File to Download";
        if (!Directory.Exists(Server.MapPath("~/InvoiceExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/InvoiceExcel/"));
        }
        string Path = "";
        try
        {

            //******************
            string str = "";
            if (variable.VarGSTForInvoiceFormNew == "1")
            {
                str = @"select * From V_InvoiceDetail VI inner join V_PackingDetailsIkea_WithGST VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue;
            }
            else
            {
                str = @"select * From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue;
            }
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int Pcs = 0, TotalPcs = 0;
                Decimal Area = 0, Amount = 0, TotalAmount = 0, GSTAmount = 0, TotalGSTAmount = 0, TotalAmountWithGST = 0;

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("Invoice1");

                //************set cell width
                //Page
                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                sht.PageSetup.AdjustTo(105);
                sht.PageSetup.FitToPages(1, 1);
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                sht.PageSetup.VerticalDpi = 600;
                sht.PageSetup.HorizontalDpi = 600;
                //
                sht.PageSetup.Margins.Top = 0.2;
                sht.PageSetup.Margins.Left = 0.2;
                sht.PageSetup.Margins.Right = 0.2;
                sht.PageSetup.Margins.Bottom = 0.2;
                sht.PageSetup.Margins.Header = 0.2;
                sht.PageSetup.Margins.Footer = 0.2;
                sht.PageSetup.CenterHorizontally = true;
                //sht.PageSetup.CenterVertically = true;
                sht.PageSetup.SetScaleHFWithDocument();
                //************
                sht.Column("A").Width = 13.67;
                sht.Column("B").Width = 8.89;
                sht.Column("C").Width = 23.33;
                sht.Column("D").Width = 9.11;
                sht.Column("E").Width = 10.56;
                sht.Column("F").Width = 9.67;
                sht.Column("G").Width = 10.33;
                sht.Column("H").Width = 10.33;
                sht.Column("I").Width = 12.44;
                sht.Column("J").Width = 11.33;
              
                //************
                sht.Row(1).Height = 13.75;
                //*****Header                
                sht.Cell("A1").Value = "INVOICE";
                sht.Range("A1:J1").Style.Font.FontName = "Arial";
                sht.Range("A1:J1").Style.Font.FontSize = 11;
                sht.Range("A1:J1").Style.Font.Bold = true;
                sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:J1").Merge();

                using (var a = sht.Range("A1:J1"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //********Exporter
                sht.Range("A2").Value = "Exporter";
                sht.Range("A2:D2").Style.Font.FontName = "Arial";
                sht.Range("A2:D2").Style.Font.FontSize = 9;
                //sht.Range("A2:D2").Style.Font.Bold = true;
                sht.Range("A2:D2").Merge();
                sht.Range("A2:D2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
             
                //*****************

                //CompanyName
                sht.Range("A3").Value = ds.Tables[0].Rows[0]["companyName"];
                sht.Range("A3:D3").Style.Font.FontName = "Arial";
                sht.Range("A3:D3").Style.Font.FontSize = 9;
                sht.Range("A3:D3").Style.Font.Bold = true;
                sht.Range("A3:D3").Merge();
                sht.Range("A3:D3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //Address
                sht.Range("A4").Value = ds.Tables[0].Rows[0]["compaddr1"];
                sht.Range("A4:D4").Style.Font.FontName = "Arial";
                sht.Range("A4:D4").Style.Font.FontSize = 9;
                sht.Range("A4:D4").Merge();
                sht.Range("A4:D4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //address2
                sht.Range("A5").Value = ds.Tables[0].Rows[0]["compaddr2"] + "," + ds.Tables[0].Rows[0]["compaddr3"];
                sht.Range("A5:D5").Style.Font.FontName = "Arial";
                sht.Range("A5:D5").Style.Font.FontSize = 9;
                sht.Range("A5:D5").Merge();
                sht.Range("A5:D5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //TiN No
                sht.Range("A6").Value = "GSTIN#  " + ds.Tables[0].Rows[0]["gstno"];
                sht.Range("A6:D6").Style.Font.FontName = "Arial";
                sht.Range("A6:D6").Style.Font.FontSize = 8;
                sht.Range("A6:D6").Style.Font.Bold = true;
                sht.Range("A6:D6").Merge();
                sht.Range("A6:D6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //STATE 
                sht.Range("A7").Value = "STATE :   UTTAR PRADESH";
                sht.Range("A7:D7").Style.Font.FontName = "Arial";
                sht.Range("A7:D7").Style.Font.FontSize = 8;
                sht.Range("A7:D7").Style.Font.Bold = true;
                sht.Range("A7:D7").Merge();
                sht.Range("A7:D7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //PAN :
                sht.Range("A8").Value = "PAN :  " + ds.Tables[0].Rows[0]["Pannr"];
                sht.Range("A8:D8").Style.Font.FontName = "Arial";
                sht.Range("A8:D8").Style.Font.FontSize = 8;
                sht.Range("A8:D8").Style.Font.Bold = true;
                sht.Range("A8:D8").Merge();
                sht.Range("A8:D8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A8:J8"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("A2:A26"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("D2:D26"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("J2:J26"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }


                //**********Invoice No
                sht.Range("E2").Value = "Invoice No";
                sht.Range("E2:E2").Style.Font.FontName = "Arial";
                sht.Range("E2:E2").Style.Font.FontSize = 9;
                sht.Range("E2:E2").Merge();
                sht.Range("E2:E2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //value
                sht.Range("F2").Value = ds.Tables[0].Rows[0]["TInvoiceNo"];
                sht.Range("F2:G2").Style.Font.FontName = "Arial";
                sht.Range("F2:G2").Style.Font.FontSize = 9;
                sht.Range("F2:G2").Style.Font.Bold = true;
                sht.Range("F2:G2").Merge();
                sht.Range("F2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //**********InvoiceDate
                sht.Range("I2").Value = "DATED";
                sht.Range("I2:I2").Style.Font.FontName = "Arial";
                sht.Range("I2:I2").Style.Font.FontSize = 9;
                sht.Range("I2:I2").Merge();
                sht.Range("I2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //value
                //sht.Range("J2").Value = ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("J2").SetValue(ds.Tables[0].Rows[0]["Invoicedate"]);
                sht.Range("J2:J2").Style.Font.FontName = "Arial";
                sht.Range("J2:J2").Style.Font.FontSize = 9;
                sht.Range("J2:J2").Style.Font.Bold = true;
                sht.Range("J2:J2").Merge();
                sht.Range("J2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //sht.Range("J2").Value = DateTime.Now.ToShortDateString();

                using (var a = sht.Range("E2:J2"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //**********BuyerOrderNo
                sht.Range("E3").Value = "Buyer Order No.  :";
                sht.Range("E3:F3").Style.Font.FontName = "Arial";
                sht.Range("E3:F3").Style.Font.FontSize = 9;
                sht.Range("E3:F3").Merge();
                sht.Range("E3:F3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("G3").Value = " As Mentioned Below";
                sht.Range("G3:J3").Style.Font.FontName = "Arial";
                sht.Range("G3:J3").Style.Font.FontSize = 9;
                sht.Range("G3:J3").Style.Font.Bold = true;
                sht.Range("G3:J3").Merge();
                sht.Range("G3:J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //Value
                sht.Range("E4").Value = ds.Tables[0].Rows[0]["TorderNo"];
                sht.Range("E4:J5").Style.Font.FontName = "Arial";
                sht.Range("E4:J5").Style.Font.FontSize = 9;
                sht.Range("E4:J5").Merge();
                sht.Range("E4:J5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E4:J5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                
                using (var a = sht.Range("E5:J5"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //**********SupplierNo
                sht.Range("E6").Value = "Supplier No.";
                sht.Range("E6:E6").Style.Font.FontName = "Arial";
                sht.Range("E6:E6").Style.Font.FontSize = 9;
                sht.Range("E6:E6").Merge();
                sht.Range("E6:E6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //Value
                sht.Range("F6").Value = "17933";
                sht.Range("F6:G6").Style.Font.FontName = "Arial";
                sht.Range("F6:G6").Style.Font.FontSize = 9;
                sht.Range("F6:G6").Merge();
                sht.Range("F6:G6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("F6:G6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********DESTN. CODE
                sht.Range("H6").Value = "DESTN. CODE";
                sht.Range("H6:I6").Style.Font.FontName = "Arial";
                sht.Range("H6:I6").Style.Font.FontSize = 9;
                sht.Range("H6:I6").Merge();
                sht.Range("H6:I6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //Value
                //sht.Range("J6").Value = ds.Tables[0].Rows[0]["DestCode"];
                sht.Range("J6").SetValue(ds.Tables[0].Rows[0]["DestCode"]);
                //sht.Range("J6:J6").Style.NumberFormat.Format = "@";
                sht.Range("J6:J6").Style.Font.FontName = "Arial";
                sht.Range("J6:J6").Style.Font.FontSize = 9;
                sht.Range("J6:J6").Merge();
                sht.Range("J6:J6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("J6:J6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("E6:J6"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //**********CONSIGNMENT NO
                sht.Range("E7").Value = "CONSIGNMENT NO";
                sht.Range("E7:F8").Style.Font.FontName = "Arial";
                sht.Range("E7:F8").Style.Font.FontSize = 9;
                sht.Range("E7:F8").Style.Font.Bold = true;
                sht.Range("E7:F8").Merge();
                sht.Range("E7:F8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E7:F8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                //**********CONSIGNMENT NO Value
                sht.Range("G7").Value = ds.Tables[0].Rows[0]["otherref"];
                sht.Range("G7:J8").Style.Font.FontName = "Arial";
                sht.Range("G7:J8").Style.Font.FontSize = 9;
                sht.Range("G7:J8").Style.Font.Bold = true;
                sht.Range("G7:J8").Merge();
                sht.Range("G7:J8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("G7:J8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                //**********Consignee
                sht.Range("A9").Value = "Consignee";
                sht.Range("A9:A9").Style.Font.FontName = "Arial";
                sht.Range("A9:A9").Style.Font.FontSize = 9;
                sht.Range("A9:A9").Style.Font.Bold = false;
                sht.Range("A9:A9").Merge();
                sht.Range("A9:A9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A9:A9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Consignee Code Value
                sht.Range("B9").Value = ds.Tables[0].Rows[0]["consignee_dt"];
                sht.Range("B9:D9").Style.Font.FontName = "Arial";
                sht.Range("B9:D9").Style.Font.FontSize = 9;
                sht.Range("B9:D9").Style.Font.Bold = true;
                sht.Range("B9:D9").Merge();
                sht.Range("B9:D9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("B9:D9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Consignee Name Value
                sht.Range("A10").Value = ds.Tables[0].Rows[0]["consignee"];
                sht.Range("A10:D10").Style.Font.FontName = "Arial";
                sht.Range("A10:D10").Style.Font.FontSize = 9;
                sht.Range("A10:D10").Style.Font.Bold = true;
                sht.Range("A10:D10").Merge();
                sht.Range("A10:D10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A10:D10").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Consignee Address Value
                sht.Range("A11").Value = ds.Tables[0].Rows[0]["consignee_address"];
                sht.Range("A11:D13").Style.Font.FontName = "Arial";
                sht.Range("A11:D13").Style.Font.FontSize = 9;
                sht.Range("A11:D13").Style.Font.Bold = false;
                sht.Range("A11:D13").Merge();
                sht.Range("A11:D13").Style.Alignment.SetWrapText();
                sht.Range("A11:D13").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A11:D13").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Blank Row
                sht.Range("A14").Value = "";
                sht.Range("A14:D14").Style.Font.FontName = "Arial";
                sht.Range("A14:D14").Style.Font.FontSize = 9;
                sht.Range("A14:D14").Style.Font.Bold = false;
                sht.Range("A14:D14").Merge();
                sht.Range("A14:D14").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A14:D14").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Notify Party
                sht.Range("A15").Value = "Notify Party";
                sht.Range("A15:A15").Style.Font.FontName = "Arial";
                sht.Range("A15:A15").Style.Font.FontSize = 9;
                sht.Range("A15:A15").Style.Font.Bold = false;
                sht.Range("A15:A15").Merge();
                sht.Range("A15:A15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A15:A15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Notify Party Code Value
                sht.Range("B15").Value = ds.Tables[0].Rows[0]["Notifyparty_dt"];
                sht.Range("B15:D15").Style.Font.FontName = "Arial";
                sht.Range("B15:D15").Style.Font.FontSize = 9;
                sht.Range("B15:D15").Style.Font.Bold = true;
                sht.Range("B15:D15").Merge();
                sht.Range("B15:D15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("B15:D15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Notify Party Name Value
                sht.Range("A16").Value = ds.Tables[0].Rows[0]["Notifyparty"];
                sht.Range("A16:D16").Style.Font.FontName = "Arial";
                sht.Range("A16:D16").Style.Font.FontSize = 9;
                sht.Range("A16:D16").Style.Font.Bold = true;
                sht.Range("A16:D16").Merge();
                sht.Range("A16:D16").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A16:D16").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Notify Party Address Value
                sht.Range("A17").Value = ds.Tables[0].Rows[0]["notifyparty_address"];
                sht.Range("A17:D19").Style.Font.FontName = "Arial";
                sht.Range("A17:D19").Style.Font.FontSize = 9;
                sht.Range("A17:D19").Style.Font.Bold = false;
                sht.Range("A17:D19").Merge();
                sht.Range("A17:D19").Style.Alignment.SetWrapText();
                sht.Range("A17:D19").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A17:D19").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("A19:J19"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //**********Buyer  (If Other than Consignee)
                sht.Range("E9").Value = "Buyer  (If Other than Consignee)";
                sht.Range("E9:J9").Style.Font.FontName = "Arial";
                sht.Range("E9:J9").Style.Font.FontSize = 9;
                sht.Range("E9:J9").Style.Font.Bold = false;
                sht.Range("E9:J9").Merge();
                sht.Range("E9:J9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E9:J9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Buyer  (If Other than Consignee) Value
                sht.Range("E10").Value = "1." + ds.Tables[0].Rows[0]["buyer_otherthanconsignee"];
                sht.Range("E10:J10").Style.Font.FontName = "Arial";
                sht.Range("E10:J10").Style.Font.FontSize = 9;
                sht.Range("E10:J10").Style.Font.Bold = true;
                sht.Range("E10:J10").Merge();
                sht.Range("E10:J10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E10:J10").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Buyer  (If Other than Consignee) Value
                sht.Range("E11").Value = ds.Tables[0].Rows[0]["otherthanconsignee_address"];
                sht.Range("E11:J13").Style.Font.FontName = "Arial";
                sht.Range("E11:J13").Style.Font.FontSize = 9;
                sht.Range("E11:J13").Style.Font.Bold = false;
                sht.Range("E11:J13").Merge();
                sht.Range("E11:J13").Style.Alignment.SetWrapText();
                sht.Range("E11:J13").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E11:J13").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Blank Row
                sht.Range("E14").Value = "";
                sht.Range("E14:J14").Style.Font.FontName = "Arial";
                sht.Range("E14:J14").Style.Font.FontSize = 9;
                sht.Range("E14:J14").Style.Font.Bold = false;
                sht.Range("E14:J14").Merge();
                sht.Range("E14:J14").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E14:J14").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);


                //********** 2.       PAYING AGENT :
                sht.Range("E15").Value = " 2.PAYING AGENT :";
                sht.Range("E15:F15").Style.Font.FontName = "Arial";
                sht.Range("E15:F15").Style.Font.FontSize = 9;
                sht.Range("E15:F15").Style.Font.Bold = true;
                sht.Range("E15:F15").Merge();
                sht.Range("E15:F15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E15:F15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //********* 2.       PAYING AGENT Code : Value
                sht.Range("G15").Value = ds.Tables[0].Rows[0]["payingagent"];
                sht.Range("G15:J15").Style.Font.FontName = "Arial";
                sht.Range("G15:J15").Style.Font.FontSize = 9;
                sht.Range("G15:J15").Style.Font.Bold = true;
                sht.Range("G15:J15").Merge();
                sht.Range("G15:J15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("G15:J15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //********** 2.       PAYING AGENT Address: Value
                sht.Range("G16").Value = ds.Tables[0].Rows[0]["payingagent_address"];
                sht.Range("G16:J19").Style.Font.FontName = "Arial";
                sht.Range("G16:J19").Style.Font.FontSize = 9;
                sht.Range("G16:J19").Style.Font.Bold = false;
                sht.Range("G16:J19").Merge();
                sht.Range("G16:J19").Style.Alignment.SetWrapText();
                sht.Range("G16:J19").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("G16:J19").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);


                //**********Pre-Carriage By:
                sht.Range("A20").Value = "Pre-Carriage By";
                sht.Range("A20:B20").Style.Font.FontName = "Arial";
                sht.Range("A20:B20").Style.Font.FontSize = 9;
                sht.Range("A20:B20").Style.Font.Bold = false;
                sht.Range("A20:B20").Merge();
                sht.Range("A20:B20").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A20:B20").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Pre-Carriage By Value:
                sht.Range("A21").Value = ds.Tables[0].Rows[0]["pre_CarriageBy"];
                sht.Range("A21:B21").Style.Font.FontName = "Arial";
                sht.Range("A21:B21").Style.Font.FontSize = 9;
                sht.Range("A21:B21").Style.Font.Bold = true;
                sht.Range("A21:B21").Merge();
                sht.Range("A21:B21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A21:B21").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("B20:B26"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                //**********PLACE OF RECEIPT :
                sht.Range("C20").Value = "PLACE OF RECEIPT ";
                sht.Range("C20:D20").Style.Font.FontName = "Arial";
                sht.Range("C20:D20").Style.Font.FontSize = 9;
                sht.Range("C20:D20").Style.Font.Bold = false;
                sht.Range("C20:D20").Merge();
                sht.Range("C20:D20").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("C20:D20").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********PLACE OF RECEIPT  Value:
                sht.Range("C21").Value = ds.Tables[0].Rows[0]["receiptby"];
                sht.Range("C21:D21").Style.Font.FontName = "Arial";
                sht.Range("C21:D21").Style.Font.FontSize = 9;
                sht.Range("C21:D21").Style.Font.Bold = true;
                sht.Range("C21:D21").Merge();
                sht.Range("C21:D21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C21:D21").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("A21:D21"))
                {
                    a.Style.Border.BottomBorder= XLBorderStyleValues.Thin;
                }

                //**********VESSEL/FLIGHT NO.:
                sht.Range("A22").Value = "VESSEL/FLIGHT NO.";
                sht.Range("A22:B22").Style.Font.FontName = "Arial";
                sht.Range("A22:B22").Style.Font.FontSize = 9;
                sht.Range("A22:B22").Style.Font.Bold = false;
                sht.Range("A22:B22").Merge();
                sht.Range("A22:B22").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A22:B22").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********VESSEL/FLIGHT NO. Value:
                sht.Range("A23").Value = ds.Tables[0].Rows[0]["vessel_flightno"];
                sht.Range("A23:B23").Style.Font.FontName = "Arial";
                sht.Range("A23:B23").Style.Font.FontSize = 9;
                sht.Range("A23:B23").Style.Font.Bold = true;
                sht.Range("A23:B23").Merge();
                sht.Range("A23:B23").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A23:B23").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                //**********PORT OF LOADING  :
                sht.Range("C22").Value = "PORT OF LOADING  ";
                sht.Range("C22:D22").Style.Font.FontName = "Arial";
                sht.Range("C22:D22").Style.Font.FontSize = 9;
                sht.Range("C22:D22").Style.Font.Bold = false;
                sht.Range("C22:D22").Merge();
                sht.Range("C22:D22").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("C22:D22").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********PORT OF LOADING   Value:
                sht.Range("C23").Value = ds.Tables[0].Rows[0]["portload"];
                sht.Range("C23:D23").Style.Font.FontName = "Arial";
                sht.Range("C23:D23").Style.Font.FontSize = 9;
                sht.Range("C23:D23").Style.Font.Bold = true;
                sht.Range("C23:D23").Merge();
                sht.Range("C23:D23").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C23:D23").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("A23:D23"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //**********PORT OF DISCHARGE.:
                sht.Range("A24").Value = "PORT OF DISCHARGE.";
                sht.Range("A24:B24").Style.Font.FontName = "Arial";
                sht.Range("A24:B24").Style.Font.FontSize = 9;
                sht.Range("A24:B24").Style.Font.Bold = false;
                sht.Range("A24:B24").Merge();
                sht.Range("A24:B24").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A24:B24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********PORT OF DISCHARGE Value:
                sht.Range("A25").Value = ds.Tables[0].Rows[0]["portunload"];
                sht.Range("A25:B26").Style.Font.FontName = "Arial";
                sht.Range("A25:B26").Style.Font.FontSize = 9;
                sht.Range("A25:B26").Style.Font.Bold = true;
                sht.Range("A25:B26").Merge();
                sht.Range("A25:B26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A25:B26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                //**********FINAL DESTINATION  :
                sht.Range("C24").Value = "FINAL DESTINATION ";
                sht.Range("C24:D24").Style.Font.FontName = "Arial";
                sht.Range("C24:D24").Style.Font.FontSize = 9;
                sht.Range("C24:D24").Style.Font.Bold = false;
                sht.Range("C24:D24").Merge();
                sht.Range("C24:D24").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("C24:D24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********FINAL DESTINATION  Value:
                sht.Range("C25").Value = ds.Tables[0].Rows[0]["Destinationadd"];
                sht.Range("C25:D26").Style.Font.FontName = "Arial";
                sht.Range("C25:D26").Style.Font.FontSize = 9;
                sht.Range("C25:D26").Style.Font.Bold = true;
                sht.Range("C25:D26").Merge();
                sht.Range("C25:D26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C25:D26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("A26:D26"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //**********COUNTRY OF ORIGIN  :
                sht.Range("E20").Value = "COUNTRY OF ORIGIN ";
                sht.Range("E20:G20").Style.Font.FontName = "Arial";
                sht.Range("E20:G20").Style.Font.FontSize = 9;
                sht.Range("E20:G20").Style.Font.Bold = false;
                sht.Range("E20:G20").Merge();
                sht.Range("E20:G20").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E20:G20").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********COUNTRY OF ORIGIN  Value:
                sht.Range("E21").Value = ds.Tables[0].Rows[0]["countryoforigin"];
                sht.Range("E21:G21").Style.Font.FontName = "Arial";
                sht.Range("E21:G21").Style.Font.FontSize = 9;
                sht.Range("E21:G21").Style.Font.Bold = true;
                sht.Range("E21:G21").Merge();
                sht.Range("E21:G21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E21:G21").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("G20:G21"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                //**********COUNTRY OF FINAL DESTINATION :
                sht.Range("H20").Value = "COUNTRY OF FINAL DESTINATION ";
                sht.Range("H20:J20").Style.Font.FontName = "Arial";
                sht.Range("H20:J20").Style.Font.FontSize = 9;
                sht.Range("H20:J20").Style.Font.Bold = false;
                sht.Range("H20:J20").Merge();
                sht.Range("H20:J20").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("H20:J20").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********COUNTRY OF FINAL DESTINATION  Value:
                sht.Range("H21").Value = ds.Tables[0].Rows[0]["countryoffinaldest"];
                sht.Range("H21:J21").Style.Font.FontName = "Arial";
                sht.Range("H21:J21").Style.Font.FontSize = 9;
                sht.Range("H21:J21").Style.Font.Bold = true;
                sht.Range("H21:J21").Merge();
                sht.Range("H21:J21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H21:J21").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("E21:J21"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //**********Terms of Delivery and Payment  :
                sht.Range("E22").Value = "Terms of Delivery and Payment";
                sht.Range("E22:J22").Style.Font.FontName = "Arial";
                sht.Range("E22:J22").Style.Font.FontSize = 9;
                sht.Range("E22:J22").Style.Font.Bold = false;
                sht.Range("E22:J22").Merge();
                sht.Range("E22:J22").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E22:J22").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //***************BlankRow
                sht.Range("E23").Value = "";
                sht.Range("E23:J23").Style.Font.FontName = "Arial";
                sht.Range("E23:J23").Style.Font.FontSize = 9;
                sht.Range("E23:J23").Style.Font.Bold = false;
                sht.Range("E23:J23").Merge();
                sht.Range("E23:J23").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E23:J23").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //********** Terms of Payment :  :
                sht.Range("E24").Value = " Terms of Payment : " + " " + ds.Tables[0].Rows[0]["Deliveryterm"];
                sht.Range("E24:J24").Style.Font.FontName = "Arial";
                sht.Range("E24:J24").Style.Font.FontSize = 9;
                sht.Range("E24:J24").Style.Font.Bold = true;
                sht.Range("E24:J24").Merge();
                sht.Range("E24:J24").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E24:J24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //********** DELIVERY WEEK :  :
                sht.Range("E25").Value = " DELIVERY WEEK  :" + " " + ds.Tables[0].Rows[0]["DELVWK"];
                sht.Range("E25:J25").Style.Font.FontName = "Arial";
                sht.Range("E25:J25").Style.Font.FontSize = 9;
                sht.Range("E25:J25").Style.Font.Bold = true;
                sht.Range("E25:J25").Merge();
                sht.Range("E25:J25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E25:J25").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //***************BlankRow
                sht.Range("E26").Value = "";
                sht.Range("E26:J26").Style.Font.FontName = "Arial";
                sht.Range("E26:J26").Style.Font.FontSize = 9;
                sht.Range("E26:J26").Style.Font.Bold = false;
                sht.Range("E26:J26").Merge();
                sht.Range("E26:J26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E26:J26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("E26:J26"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //**********Marks Nos./ Container No.:
                sht.Range("A27").Value = "Marks Nos./ Container No.";
                sht.Range("A27:B27").Style.Font.FontName = "Arial";
                sht.Range("A27:B27").Style.Font.FontSize = 9;
                sht.Range("A27:B27").Style.Font.Bold = false;
                sht.Range("A27:B27").Merge();
                sht.Range("A27:B27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A27:B27").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Marks Nos./ Container No.:
                sht.Range("A28").Value = "IKEA/"+ds.Tables[0].Rows[0]["countryoffinaldest"];
                sht.Range("A28:B28").Style.Font.FontName = "Arial";
                sht.Range("A28:B28").Style.Font.FontSize = 9;
                sht.Range("A28:B28").Style.Font.Bold = false;
                sht.Range("A28:B28").Merge();
                sht.Range("A28:B28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A28:B28").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Marks Nos./ Container No Value.:
                sht.Range("A32").Value = "No" + " " + ds.Tables[0].Compute("Min(MinrollNo)", "") +" "+" to" +" "+ ds.Tables[0].Compute("Max(Maxrollno)", "");
                sht.Range("A32:B32").Style.Font.FontName = "Arial";
                sht.Range("A32:B32").Style.Font.FontSize = 9;
                sht.Range("A32:B32").Style.Font.Bold = true;
                sht.Range("A32:B32").Merge();
                sht.Range("A32:B32").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A32:B32").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********No. of Pkgs.:
                sht.Range("C27").Value = "No. of Pkgs.";
                sht.Range("C27:C27").Style.Font.FontName = "Arial";
                sht.Range("C27:C27").Style.Font.FontSize = 9;
                sht.Range("C27:C27").Style.Font.Bold = false;
                sht.Range("C27:C27").Merge();
                sht.Range("C27:C27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C27:C27").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                //**********No. of Pkgs Value.:
                sht.Range("C28").Value = ds.Tables[0].Rows[0]["Noofrolls"];
                sht.Range("C28:C28").Style.Font.FontName = "Arial";
                sht.Range("C28:C28").Style.Font.FontSize = 9;
                sht.Range("C28:C28").Style.Font.Bold = true;
                sht.Range("C28:C28").Merge();
                sht.Range("C28:C28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C28:C28").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                //**********No. of Pkgs Value.:
                sht.Range("C29").Value = "Pallets";
                sht.Range("C29:C29").Style.Font.FontName = "Arial";
                sht.Range("C29:C29").Style.Font.FontSize = 9;
                sht.Range("C29:C29").Style.Font.Bold = true;
                sht.Range("C29:C29").Merge();
                sht.Range("C29:C29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C29:C29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                //*********DESCRIPTION OF GOODS :
                sht.Range("D27").Value = "DESCRIPTION OF GOODS ";
                sht.Range("D27:J27").Style.Font.FontName = "Arial";
                sht.Range("D27:J27").Style.Font.FontSize = 9;
                sht.Range("D27:J27").Style.Font.Bold = true;
                sht.Range("D27:J27").Merge();
                sht.Range("D27:J27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("D27:J27").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //*********DESCRIPTION OF GOODS Value :
                sht.Range("D28").Value = ds.Tables[0].Rows[0]["descriptionofgoods"];
                sht.Range("D28:J30").Style.Font.FontName = "Arial";
                sht.Range("D28:J30").Style.Font.FontSize = 9;
                sht.Range("D28:J30").Style.Font.Bold = true;
                sht.Range("D28:J30").Merge();
                sht.Range("D28:J30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("D28:J30").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("A32:J32"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("A27:A32"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("J27:J32"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                ////********Details

                sht.Range("A33").Value = "P.O. NOS.";
                sht.Range("A33:A35").Style.Font.FontName = "Arial";
                sht.Range("A33:A35").Style.Font.FontSize = 9;
                sht.Range("A33:A35").Style.Font.Bold = true;
                sht.Range("A33:A35").Merge();
                sht.Range("A33:A35").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A33:A35").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A33:A35").Style.Alignment.SetWrapText();

                sht.Range("B33").Value = "ART. NO.";
                sht.Range("B33:B35").Style.Font.FontName = "Arial";
                sht.Range("B33:B35").Style.Font.FontSize = 9;
                sht.Range("B33:B35").Style.Font.Bold = true;
                sht.Range("B33:B35").Merge();
                sht.Range("B33:B35").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B33:B35").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B33:B35").Style.Alignment.SetWrapText();

                sht.Range("C33").Value = "DESCRIPTION, COLOUR & SIZE";
                sht.Range("C33:F35").Style.Font.FontName = "Arial";
                sht.Range("C33:F35").Style.Font.FontSize = 9;
                sht.Range("C33:F35").Style.Font.Bold = true;
                sht.Range("C33:F35").Merge();
                sht.Range("C33:F35").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C33:F35").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("C33:F35").Style.Alignment.SetWrapText();

                sht.Range("G33").Value = "HSN";
                sht.Range("G33:G35").Style.Font.FontName = "Arial";
                sht.Range("G33:G35").Style.Font.FontSize = 9;
                sht.Range("G33:G35").Style.Font.Bold = true;
                sht.Range("G33:G35").Merge();
                sht.Range("G33:G35").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("G33:G35").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("G33:G35").Style.Alignment.SetWrapText();

                sht.Range("H33").Value = "QUANTITY PCS.";
                sht.Range("H33:H35").Style.Font.FontName = "Arial";
                sht.Range("H33:H35").Style.Font.FontSize = 8;
                sht.Range("H33:H35").Style.Font.Bold = true;
                sht.Range("H33:H35").Merge();
                sht.Range("H33:H35").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H33:H35").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("H33:H35").Style.Alignment.SetWrapText();

                sht.Range("I33").Value = "RATE FCA/"+ ds.Tables[0].Rows[0]["CIF"];
                sht.Range("I33:I35").Style.Font.FontName = "Arial";
                sht.Range("I33:I35").Style.Font.FontSize = 9;
                sht.Range("I33:I35").Style.Font.Bold = true;
                sht.Range("I33:I35").Merge();
                sht.Range("I33:I35").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("I33:I35").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("I33:I35").Style.Alignment.SetWrapText();

                sht.Range("J33").Value = "AMOUNT FCA/" + ds.Tables[0].Rows[0]["CIF"];
                sht.Range("J33:J35").Style.Font.FontName = "Arial";
                sht.Range("J33:J35").Style.Font.FontSize = 9;
                sht.Range("J33:J35").Style.Font.Bold = true;
                sht.Range("J33:J35").Merge();
                sht.Range("J33:J35").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("J33:J35").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("J33:J35").Style.Alignment.SetWrapText();

                using (var a = sht.Range("A33:J35"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                int row = 36;
                int totalrowcount = 50;
                int rowcount = ds.Tables[0].Rows.Count;
                totalrowcount = totalrowcount - rowcount;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":J" + row).Style.Font.FontName = "Arial";
                    sht.Range("A" + row + ":J" + row).Style.Font.FontSize = 10;

                   // pono
                    sht.Range("A" + row).Value = ds.Tables[0].Rows[i]["Pono"];
                    sht.Range("B" + row).Value = ds.Tables[0].Rows[i]["articleno"];
                    sht.Range("C" + row + ":F" + row).Merge();
                    sht.Range("C" + row).Value = ds.Tables[0].Rows[i]["Quality"] + " " + ds.Tables[0].Rows[i]["Design"] + " " + ds.Tables[0].Rows[i]["Color"] + " " + ds.Tables[0].Rows[i]["Width"] + "X" + ds.Tables[0].Rows[i]["Length"];
                    sht.Range("G" + row).Value = ds.Tables[0].Rows[i]["HSNCode"];
                    sht.Range("H" + row).Value = ds.Tables[0].Rows[i]["Pcs"];
                    Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[i]["Pcs"]);
                    sht.Range("I" + row).Value = ds.Tables[0].Rows[i]["Price"];
                    sht.Range("J" + row).Value = ds.Tables[0].Rows[i]["amount"];
                    sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                    Amount = Amount + Convert.ToDecimal(ds.Tables[0].Rows[i]["amount"]);

                    Area = Convert.ToDecimal(Area.ToString()) + Convert.ToDecimal(ds.Tables[0].Rows[i]["Area"].ToString());

                    using (var a = sht.Range("A" + row + ":J" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    row = row + 1;
                }

                if (totalrowcount > 0)
                {
                    for (int j = row; j < totalrowcount - 1; j++)
                    {
                        sht.Range("A" + row + ":J" + row).Style.Font.FontName = "Arial";
                        sht.Range("A" + row + ":J" + row).Style.Font.FontSize = 10;
                        sht.Range("A" + row + ":J" + row).Style.Font.SetBold();
                        sht.Range("A" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        //sht.Range("A" + row + ":J" + row).Style.Alignment.SetWrapText();                     


                        sht.Range("A" + row).SetValue("");
                        sht.Range("B" + row).SetValue("");
                        sht.Range("C" + row).SetValue("");
                        sht.Range("D" + row).SetValue("");
                        sht.Range("E" + row).SetValue("");
                        sht.Range("F" + row).SetValue("");
                        sht.Range("G" + row).SetValue("");
                        sht.Range("H" + row).SetValue("");
                        sht.Range("I" + row).SetValue("");
                        sht.Range("J" + row).SetValue("");                       


                        using (var a = sht.Range("A" + row + ":J" + row))
                        {
                            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            //a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        }
                        row = row + 1;
                    }
                }
                

                sht.Range("A" + row + ":A" + row).Merge();
                sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":A" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "Supplier# 17933";
                sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row + ":J" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":A" + row).Merge();
                sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":A" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "ECIS #" + ds.Tables[0].Rows[0]["EcisNo"];
                sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row + ":J" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }


                row = row + 1;

                string CategoryName = "";
                string ArticleNo = "";
                decimal NetWeight = 0;
                string ArticleWithNetWt = "";

                for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                {
                    CategoryName = ds.Tables[0].Rows[m]["Category_Name"].ToString();
                    // if (CategoryName == "CARPET")
                    if (CategoryName == "CUSHION")
                    {
                        ArticleNo = ds.Tables[0].Rows[m]["ArticleNo"].ToString();
                        NetWeight = Convert.ToDecimal(ds.Tables[0].Rows[m]["ArticleNoWiseNetWt"])*Convert.ToDecimal(ds.Tables[0].Rows[m]["ArticleWiseNoOfRoll"]);

                        //ArticleWithNetWt = " Art. No. " + ArticleNo + " " + "Total Wt" + " " + NetWeight;
                        ArticleWithNetWt = ArticleWithNetWt + " Art. No. " + ArticleNo + " " + "Total Wt" + " " + NetWeight + Environment.NewLine;
                    }                   
                }

                sht.Range("A" + row + ":A" + row).Merge();
                sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":A" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "";
                sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
              
                sht.Range("B" + row + ":B" + row).Merge();
                sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Arial";
                sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 9;
                sht.Range("B" + row + ":B" + row).Style.Font.Bold = true;
                sht.Range("B" + row).Value = "";
                sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("C" + row + ":C" + row).Merge();
                sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("C" + row).Value = "";
                sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("D" + row + ":F" + (row+2)).Merge();
                sht.Range("D" + row + ":F" + (row + 2)).Style.Font.FontName = "Arial";
                sht.Range("D" + row + ":F" + (row + 2)).Style.Font.FontSize = 9;
                sht.Range("D" + row + ":F" + (row + 2)).Style.Font.Bold = true;
                sht.Range("D" + row).Value = ArticleWithNetWt;
                sht.Range("D" + row + ":F" + (row + 2)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("D" + row + ":F" + (row + 2)).Style.Alignment.SetWrapText();

                sht.Range("G" + row + ":G" + row).Merge();
                sht.Range("G" + row + ":G" + row).Style.Font.FontName = "Arial";
                sht.Range("G" + row + ":G" + row).Style.Font.FontSize = 9;
                sht.Range("G" + row + ":G" + row).Style.Font.Bold = true;
                sht.Range("G" + row).Value = "";
                sht.Range("G" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("H" + row + ":H" + row).Merge();
                sht.Range("H" + row + ":H" + row).Style.Font.FontName = "Arial";
                sht.Range("H" + row + ":H" + row).Style.Font.FontSize = 9;
                sht.Range("H" + row + ":H" + row).Style.Font.Bold = true;
                sht.Range("H" + row).Value = "";
                sht.Range("H" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("I" + row + ":I" + row).Merge();
                sht.Range("I" + row + ":I" + row).Style.Font.FontName = "Arial";
                sht.Range("I" + row + ":I" + row).Style.Font.FontSize = 9;
                sht.Range("I" + row + ":I" + row).Style.Font.Bold = true;
                sht.Range("I" + row).Value = "";
                sht.Range("I" + row + ":I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("J" + row + ":J" + row).Merge();
                sht.Range("J" + row + ":J" + row).Style.Font.FontName = "Arial";
                sht.Range("J" + row + ":J" + row).Style.Font.FontSize = 9;
                sht.Range("J" + row + ":J" + row).Style.Font.Bold = true;
                sht.Range("J" + row).Value = "";
                sht.Range("J" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row + ":J" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("C" + row + ":C" + row).Merge();
                sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("C" + row).Value = "Total Sqmtr." + Area;
                sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row + ":J" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                //sht.Range("C" + row + ":F" + row).Merge();
                //sht.Range("C" + row + ":F" + row).Style.Font.FontName = "Arial";
                //sht.Range("C" + row + ":F" + row).Style.Font.FontSize = 9;
                //sht.Range("C" + row + ":F" + row).Style.Font.Bold = true;
                //sht.Range("C" + row).Value = "For Art. No." + "    " ;
                //sht.Range("C" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("G" + row + ":G" + row).Merge();
                sht.Range("G" + row + ":G" + row).Style.Font.FontName = "Arial";
                sht.Range("G" + row + ":G" + row).Style.Font.FontSize = 9;
                sht.Range("G" + row + ":G" + row).Style.Font.Bold = true;
                sht.Range("G" + row).Value = "Total.";
                sht.Range("G" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("H" + row + ":H" + row).Merge();
                sht.Range("H" + row + ":H" + row).Style.Font.FontName = "Arial";
                sht.Range("H" + row + ":H" + row).Style.Font.FontSize = 9;
                sht.Range("H" + row + ":H" + row).Style.Font.Bold = true;
                sht.Range("H" + row).Value = Pcs;
                sht.Range("H" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("J" + row + ":J" + row).Merge();
                sht.Range("J" + row + ":J" + row).Style.Font.FontName = "Arial";
                sht.Range("J" + row + ":J" + row).Style.Font.FontSize = 9;
                sht.Range("J" + row + ":J" + row).Style.Font.Bold = true;
                sht.Range("J" + row).Value = Amount;
                sht.Range("J" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("J" + row).Style.NumberFormat.Format = "#,###0.00";

                using (var a = sht.Range("A" + row + ":J" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                ////Total Invoice Amt in Words                
                sht.Range("A" + row + ":J" + row).Merge();
                sht.Range("A" + row + ":J" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":J" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":J" + row).Style.Font.Bold = false;
                sht.Range("A" + row).Value = "Amount Chargeable (In words):-";
                sht.Range("A" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row + ":J" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "TOTAL FCA VALUE IN INR: ";
                sht.Range("A" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                Decimal Totalmt = 0;
                Totalmt = Convert.ToDecimal(Amount);
                string amountinwords = "";

                amountinwords = ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(Totalmt));

                string Pointamt = string.Format("{0:0.00}", Totalmt.ToString("0.00"));
                string val = "", paise = "";
                if (Pointamt.IndexOf('.') > 0)
                {
                    val = Pointamt.ToString().Split('.')[1];
                    if (Convert.ToInt32(val) > 0)
                    {
                        paise = "& Paise " + ChangeNumbersToWords.ConvertMyword(Convert.ToInt32(val)) + " ";
                    }
                }

                amountinwords = amountinwords + " " + paise + "Only";

                ////FOB INR Amt in Words              
                sht.Range("C" + row + ":J" + row).Merge();
                sht.Range("C" + row + ":J" + row).Style.Font.FontName = "Arial";
                sht.Range("C" + row + ":J" + row).Style.Font.FontSize = 9;                
                sht.Range("C" + row + ":J" + row).Style.Font.Bold = true;
                sht.Range("C" + row).Value = amountinwords.ToUpper();
                sht.Range("C" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                using (var a = sht.Range("A" + row + ":J" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                ////Blank Row
                row = row + 1;
                sht.Range("A" + row + ":J" + row).Merge();
                sht.Range("A" + row + ":J" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":J" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":J" + row).Style.Font.SetBold();
                sht.Range("A" + row).Value = "";
                sht.Range("A" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row + ":J" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = false;
                sht.Range("A" + row).Value = "TOTAL PKGS.        :";
                sht.Range("A" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("C" + row + ":E" + row).Merge();
                sht.Range("C" + row + ":E" + row).Style.Font.FontName = "Arial";
                sht.Range("C" + row + ":E" + row).Style.Font.FontSize = 9;
                sht.Range("C" + row + ":E" + row).Style.Font.Bold = true;
                sht.Range("C" + row).Value = ds.Tables[0].Rows[0]["NoOfRolls"] + " " + "Pallets(Total" + " " + Pcs + " " + "Pcs Packed in "+" " + ds.Tables[0].Rows[0]["NoOfRolls"]+" "+"Pallets)";
                sht.Range("C" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;                   
                }

                using (var a = sht.Range("J" + row))
                {                  
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = false;
                sht.Range("A" + row).Value = " TOTAL QUANTITY :";
                sht.Range("A" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("C" + row + ":C" + row).Merge();
                sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("C" + row).Value =  Pcs ;
                sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("D" + row + ":D" + row).Merge();
                sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 9;
                sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                sht.Range("D" + row).Value = "PCS.";
                sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("J" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = false;
                sht.Range("A" + row).Value = " TOTAL GR WT.     :";
                sht.Range("A" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("C" + row + ":C" + row).Merge();
                sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("C" + row).Value = ds.Tables[0].Rows[0]["GrossWt"];
                sht.Range("C" + row).Style.NumberFormat.Format = "#,###0.000";
                sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("D" + row + ":D" + row).Merge();
                sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 9;
                sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                sht.Range("D" + row).Value = "KGS.";
                sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("J" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = false;
                sht.Range("A" + row).Value = " TOTA  NT WT.       :";
                sht.Range("A" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("C" + row + ":C" + row).Merge();
                sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("C" + row).Value = ds.Tables[0].Rows[0]["NetWt"];
                sht.Range("C" + row).Style.NumberFormat.Format = "#,###0.000";
                sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("D" + row + ":D" + row).Merge();
                sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 9;
                sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                sht.Range("D" + row).Value = "KGS.";
                sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("J" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = false;
                sht.Range("A" + row).Value = " VOLUME               :";
                sht.Range("A" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("C" + row + ":C" + row).Merge();
                sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("C" + row).Value = ds.Tables[0].Rows[0]["Volume"];
                sht.Range("C" + row).Style.NumberFormat.Format = "#,###0.000";
                sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("D" + row + ":D" + row).Merge();
                sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 9;
                sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                sht.Range("D" + row).Value = "CBM.";
                sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("J" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }


                row = row + 1;

                sht.Range("A" + row + ":J" + row).Merge();
                sht.Range("A" + row + ":J" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":J" + row).Style.Font.FontSize = 10;
                sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "Supply Meant for Export With Payment of Integrated Tax (IGST)";
                sht.Range("A" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("J" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("A" + row + ":J" + row))
                {                   
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;
                sht.Row(row).Height = 44.3;

                sht.Range("A" + row + ":C" + row).Merge();
                sht.Range("A" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "DESCRIPTION";
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("D" + row + ":D" + row).Merge();
                sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 9;
                sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                sht.Range("D" + row).Value = "HSN";
                sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("D" + row + ":D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("E" + row + ":E" + row).Merge();
                sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 9;
                sht.Range("E" + row + ":E" + row).Style.Font.Bold = true;
                sht.Range("E" + row).Value = "QTY PCS";
                sht.Range("E" + row).Style.Alignment.SetWrapText();
                sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E" + row + ":E" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("F" + row + ":F" + row).Merge();
                sht.Range("F" + row + ":F" + row).Style.Font.FontName = "Arial";
                sht.Range("F" + row + ":F" + row).Style.Font.FontSize = 9;
                sht.Range("F" + row + ":F" + row).Style.Font.Bold = true;
                sht.Range("F" + row).Value = "Rate" + Environment.NewLine + "FCA/" + ds.Tables[0].Rows[0]["CIF"];
                sht.Range("F" + row).Style.Alignment.SetWrapText();
                sht.Range("F" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("F" + row + ":F" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("G" + row + ":G" + row).Merge();
                sht.Range("G" + row + ":G" + row).Style.Font.FontName = "Arial";
                sht.Range("G" + row + ":G" + row).Style.Font.FontSize = 9;
                sht.Range("G" + row + ":G" + row).Style.Font.Bold = true;
                sht.Range("G" + row).Value = "AMOUNT";
                sht.Range("G" + row).Style.Alignment.SetWrapText();
                sht.Range("G" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("G" + row + ":G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("H" + row + ":H" + row).Merge();
                sht.Range("H" + row + ":H" + row).Style.Font.FontName = "Arial";
                sht.Range("H" + row + ":H" + row).Style.Font.FontSize = 9;
                sht.Range("H" + row + ":H" + row).Style.Font.Bold = true;
                sht.Range("H" + row).Value = "IGST RATE %";
                sht.Range("H" + row).Style.Alignment.SetWrapText();
                sht.Range("H" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H" + row + ":H" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("I" + row + ":I" + row).Merge();
                sht.Range("I" + row + ":I" + row).Style.Font.FontName = "Arial";
                sht.Range("I" + row + ":I" + row).Style.Font.FontSize = 9;
                sht.Range("I" + row + ":I" + row).Style.Font.Bold = true;
                sht.Range("I" + row).Value = "IGST AMOUNT";
                sht.Range("I" + row).Style.Alignment.SetWrapText();
                sht.Range("I" + row + ":I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("I" + row + ":I" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("J" + row + ":J" + row).Merge();
                sht.Range("J" + row + ":J" + row).Style.Font.FontName = "Arial";
                sht.Range("J" + row + ":J" + row).Style.Font.FontSize = 9;
                sht.Range("J" + row + ":J" + row).Style.Font.Bold = true;
                sht.Range("J" + row).Value = "TOTAL AMOUNT (including IGST)";
                sht.Range("J" + row).Style.Alignment.SetWrapText();
                sht.Range("J" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("J" + row + ":J" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("A" + row + ":J" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder= XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    sht.Range("A" + row + ":J" + row).Style.Font.FontName = "Arial";
                    sht.Range("A" + row + ":J" + row).Style.Font.FontSize = 9;

                    sht.Range("A" + row + ":C" + row).Merge();
                    sht.Range("A" + row).Value = ds.Tables[0].Rows[k]["Description"];
                    sht.Range("A" + row).Style.Alignment.SetWrapText();
                    sht.Range("D" + row).Value = ds.Tables[0].Rows[k]["Hsncode"];
                    sht.Range("E" + row).Value = ds.Tables[0].Rows[k]["Pcs"];
                    TotalPcs = TotalPcs + Convert.ToInt16(ds.Tables[0].Rows[k]["Pcs"]);
                    sht.Range("F" + row).Value = ds.Tables[0].Rows[k]["Price"];
                    sht.Range("G" + row).Value = ds.Tables[0].Rows[k]["amount"];
                    TotalAmount = TotalAmount + Convert.ToDecimal(ds.Tables[0].Rows[k]["amount"]);
                    sht.Range("H" + row).Value = ds.Tables[0].Rows[k]["IGST1"];

                    if (ds.Tables[0].Rows[k]["GSTType"].ToString() == "1")
                    {
                        GSTAmount = (Convert.ToDecimal(ds.Tables[0].Rows[k]["amount"]) / (Convert.ToDecimal(ds.Tables[0].Rows[k]["CGST"]) + Convert.ToDecimal(ds.Tables[0].Rows[k]["SGST"])) * 100);
                        TotalGSTAmount = TotalGSTAmount + GSTAmount;
                    }
                    else if (ds.Tables[0].Rows[k]["GSTType"].ToString() == "2")
                    {
                        GSTAmount = (Convert.ToDecimal(ds.Tables[0].Rows[k]["amount"]) / (Convert.ToDecimal(ds.Tables[0].Rows[k]["IGST1"])) * 100);
                        TotalGSTAmount = TotalGSTAmount + GSTAmount;
                    }
                    else
                    {
                        GSTAmount = 0;
                    }

                    sht.Range("I" + row).Value = GSTAmount;
                    sht.Range("J" + row).Value = Convert.ToDecimal(ds.Tables[0].Rows[k]["amount"]) + GSTAmount;
                    TotalAmountWithGST = TotalAmountWithGST + Convert.ToDecimal(ds.Tables[0].Rows[k]["amount"]) + GSTAmount;                   

                    using (var a = sht.Range("A" + row + ":J" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    row = row + 1;
                }
              

                using (var a = sht.Range("A" + row + ":J" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                using (var a = sht.Range("A" + row + ":J" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":C" + row).Merge();
                sht.Range("A" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "";
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("D" + row + ":D" + row).Merge();
                sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 9;
                sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                sht.Range("D" + row).Value = "Total";
                sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("D" + row + ":D" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("E" + row + ":E" + row).Merge();
                sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 9;
                sht.Range("E" + row + ":E" + row).Style.Font.Bold = true;
                sht.Range("E" + row).Value = TotalPcs;
                sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E" + row + ":E" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("G" + row + ":G" + row).Merge();
                sht.Range("G" + row + ":G" + row).Style.Font.FontName = "Arial";
                sht.Range("G" + row + ":G" + row).Style.Font.FontSize = 9;
                sht.Range("G" + row + ":G" + row).Style.Font.Bold = true;
                sht.Range("G" + row).Value = TotalAmount;
                sht.Range("G" + row).Style.NumberFormat.Format = "#,###0.00";
                sht.Range("G" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("G" + row + ":G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("I" + row + ":I" + row).Merge();
                sht.Range("I" + row + ":I" + row).Style.Font.FontName = "Arial";
                sht.Range("I" + row + ":I" + row).Style.Font.FontSize = 9;
                sht.Range("I" + row + ":I" + row).Style.Font.Bold = true;
                sht.Range("I" + row).Value = TotalGSTAmount;
                sht.Range("I" + row).Style.NumberFormat.Format = "#,###0.00";
                sht.Range("I" + row + ":I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("I" + row + ":I" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("J" + row + ":J" + row).Merge();
                sht.Range("J" + row + ":J" + row).Style.Font.FontName = "Arial";
                sht.Range("J" + row + ":J" + row).Style.Font.FontSize = 9;
                sht.Range("J" + row + ":J" + row).Style.Font.Bold = true;
                sht.Range("J" + row).Value = TotalAmountWithGST;
                sht.Range("J" + row).Style.NumberFormat.Format = "#,###0.00";
                sht.Range("J" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("J" + row + ":J" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("A" + row + ":J" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                using (var a = sht.Range("A" + row ))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;                   
                }

                using (var a = sht.Range("J" + row))
                {
                    a.Style.Border.RightBorder= XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":G" + row).Merge();
                sht.Range("A" + row + ":G" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":G" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":G" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "STATEMENT OF ORIGIN";                
                sht.Range("A" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row + ":G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("G" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("J" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("A" + row + ":G" + row))
                { 
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Row(row).Height = 42;

                sht.Range("A" + row  + ":G" + row ).Merge();
                sht.Range("A" + row  + ":G" + row ).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":G" + row ).Style.Font.FontSize = 9;
                sht.Range("A" + row  + ":G" + row ).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "The Exporter INREX0694000965EC026 of the products covered by this document declares that, except where otherwise clearly indicated, these products are of INDIA preferential origin according to rules of origin of the Generalized system of preferences of the European Union and that the origin criterion met is P";
                sht.Range("A" + row + ":G" + row).Style.Alignment.SetWrapText();
                sht.Range("A" + row + ":G" + row ).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row  + ":G" + row ).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                sht.Range("H" + row  + ":J" + row ).Merge();
                sht.Range("H" + row + ":J" + row ).Style.Font.FontName = "Arial";
                sht.Range("H" + row + ":J" + row ).Style.Font.FontSize = 9;
                sht.Range("H" + row  + ":J" + row ).Style.Font.Bold = true;
                sht.Range("H" + row).Value = "";
                sht.Range("H" + row  + ":J" + row ).Style.Alignment.SetWrapText();
                sht.Range("H" + row + ":J" + row ).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H" + row  + ":J" + row ).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("A" + row + ":A" + row ))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("G" + row + ":G" + row ))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("J" + row + ":J" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A" + row  + ":G" + row ))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //using (var a = sht.Range("A" + (row + 3) + ":G" + (row + 3)))
                //{                    
                //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //}

              

                row = row + 1;

                //Blank Row
                sht.Range("A" + row + ":J" + row ).Merge();
                sht.Range("A" + row + ":J" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":J" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "";
                sht.Range("A" + row + ":J" + row).Style.Alignment.SetWrapText();
                sht.Range("A" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row + ":J" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }
              
                using (var a = sht.Range("J" + row ))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":C" + row).Merge();
                sht.Range("A" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "R.B.I Code No. "+ds.Tables[0].Rows[0]["RBICode"];
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetWrapText();
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                sht.Range("E" + row + ":G" + row).Merge();
                sht.Range("E" + row + ":G" + row).Style.Font.FontName = "Arial";
                sht.Range("E" + row + ":G" + row).Style.Font.FontSize = 9;
                sht.Range("E" + row + ":G" + row).Style.Font.Bold = true;
                sht.Range("E" + row).Value = "Factory Address:";
                sht.Range("E" + row + ":G" + row).Style.Alignment.SetWrapText();
                sht.Range("E" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E" + row + ":G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("G" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                sht.Range("H" + row + ":J" + row).Merge();
                sht.Range("H" + row + ":J" + row).Style.Font.FontName = "Arial";
                sht.Range("H" + row + ":J" + row).Style.Font.FontSize = 9;
                sht.Range("H" + row + ":J" + row).Style.Font.Bold = true;
                sht.Range("H" + row).Value = " Signature & Date";
                sht.Range("H" + row + ":J" + row).Style.Alignment.SetWrapText();
                sht.Range("H" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("H" + row + ":J" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("H" + row + ":J" + row ))
                {
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }


                row = row + 1;

                sht.Range("A" + row + ":C" + row).Merge();
                sht.Range("A" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = " I.E.  Code  No.: "+ds.Tables[0].Rows[0]["IECode"];
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetWrapText();
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                sht.Range("E" + row + ":G" + row).Merge();
                sht.Range("E" + row + ":G" + row).Style.Font.FontName = "Arial";
                sht.Range("E" + row + ":G" + row).Style.Font.FontSize = 9;
                sht.Range("E" + row + ":G" + row).Style.Font.Bold = true;
                sht.Range("E" + row).Value = "AGRA-MATHURA ROAD, ARTONI";
                sht.Range("E" + row + ":G" + row).Style.Alignment.SetWrapText();
                sht.Range("E" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E" + row + ":G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("G" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                sht.Range("H" + row + ":J" + row).Merge();
                sht.Range("H" + row + ":J" + row).Style.Font.FontName = "Arial";
                sht.Range("H" + row + ":J" + row).Style.Font.FontSize = 9;
                sht.Range("H" + row + ":J" + row).Style.Font.Bold = true;
                sht.Range("H" + row).Value = "";
                sht.Range("H" + row + ":J" + row).Style.Alignment.SetWrapText();
                sht.Range("H" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("H" + row + ":J" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("H" + row + ":J" + row))
                {                   
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":C" + row).Merge();
                sht.Range("A" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "DECLARATION:-";
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetWrapText();
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                sht.Range("E" + row + ":G" + row).Merge();
                sht.Range("E" + row + ":G" + row).Style.Font.FontName = "Arial";
                sht.Range("E" + row + ":G" + row).Style.Font.FontSize = 9;
                sht.Range("E" + row + ":G" + row).Style.Font.Bold = true;
                sht.Range("E" + row).Value = "AGRA-282 007 (INDIA)";
                sht.Range("E" + row + ":G" + row).Style.Alignment.SetWrapText();
                sht.Range("E" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E" + row + ":G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("G" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                sht.Range("H" + row + ":J" + row).Merge();
                sht.Range("H" + row + ":J" + row).Style.Font.FontName = "Arial";
                sht.Range("H" + row + ":J" + row).Style.Font.FontSize = 9;
                sht.Range("H" + row + ":J" + row).Style.Font.Bold = true;
                sht.Range("H" + row).Value = "";
                sht.Range("H" + row + ":J" + row).Style.Alignment.SetWrapText();
                sht.Range("H" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("H" + row + ":J" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("H" + row + ":J" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":C" + row).Merge();
                sht.Range("A" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "We declare that this invoice shows actual price of the good";
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetWrapText();
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                sht.Range("E" + row + ":G" + row).Merge();
                sht.Range("E" + row + ":G" + row).Style.Font.FontName = "Arial";
                sht.Range("E" + row + ":G" + row).Style.Font.FontSize = 9;
                sht.Range("E" + row + ":G" + row).Style.Font.Bold = true;
                sht.Range("E" + row).Value = "";
                sht.Range("E" + row + ":G" + row).Style.Alignment.SetWrapText();
                sht.Range("E" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E" + row + ":G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("G" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                sht.Range("H" + row + ":J" + row).Merge();
                sht.Range("H" + row + ":J" + row).Style.Font.FontName = "Arial";
                sht.Range("H" + row + ":J" + row).Style.Font.FontSize = 9;
                sht.Range("H" + row + ":J" + row).Style.Font.Bold = true;
                sht.Range("H" + row).Value = "";
                sht.Range("H" + row + ":J" + row).Style.Alignment.SetWrapText();
                sht.Range("H" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("H" + row + ":J" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("H" + row + ":J" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":C" + row).Merge();
                sht.Range("A" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "described and that all particulars are true and correct.";
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetWrapText();
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A" + row + ":C" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                sht.Range("E" + row + ":G" + row).Merge();
                sht.Range("E" + row + ":G" + row).Style.Font.FontName = "Arial";
                sht.Range("E" + row + ":G" + row).Style.Font.FontSize = 9;
                sht.Range("E" + row + ":G" + row).Style.Font.Bold = true;
                sht.Range("E" + row).Value = "";
                sht.Range("E" + row + ":G" + row).Style.Alignment.SetWrapText();
                sht.Range("E" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("E" + row + ":G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("G" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                sht.Range("H" + row + ":J" + row).Merge();
                sht.Range("H" + row + ":J" + row).Style.Font.FontName = "Arial";
                sht.Range("H" + row + ":J" + row).Style.Font.FontSize = 9;
                sht.Range("H" + row + ":J" + row).Style.Font.Bold = true;
                sht.Range("H" + row).Value = "";
                sht.Range("H" + row + ":J" + row).Style.Alignment.SetWrapText();
                sht.Range("H" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("H" + row + ":J" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("H" + row + ":J" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("A" + row + ":J" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }            


                string Fileextension = "xlsx";
                string filename = "Invoice-" + validateFilename(ds.Tables[0].Rows[0]["TinvoiceNo"].ToString()) + "." + Fileextension + "";
                Path = Server.MapPath("~/InvoiceExcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
                lblmsg.Text = "Invoice Excel Format downloaded successfully.";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "AltR1", "alert('No Record Found to Export')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }

    protected void Exporttoexcel_Packing_Kaysons_4()
    {
        lblmsg.Text = "Wait For Excel File to Download";
        if (!Directory.Exists(Server.MapPath("~/PackingExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/PackingExcel/"));
        }
        string Path = "";
        try
        {
           
            string str = @"select *,VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1) as Pcsperroll,VD.Pcs/(VD.Pcs/((VD.Maxrollno-VD.MinRollno)+1)) as TotalRoll From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue + " order by MinrollNo";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int Pcs = 0, TotalPcs = 0;
                Decimal Area = 0, Amount = 0, TotalAmount = 0, GSTAmount = 0, TotalGSTAmount = 0, TotalAmountWithGST = 0;

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("PackingList");

                //************set cell width
                //Page
                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                sht.PageSetup.AdjustTo(105);
                sht.PageSetup.FitToPages(1, 1);
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                sht.PageSetup.VerticalDpi = 600;
                sht.PageSetup.HorizontalDpi = 600;
                //
                sht.PageSetup.Margins.Top = 0.2;
                sht.PageSetup.Margins.Left = 0.2;
                sht.PageSetup.Margins.Right = 0.2;
                sht.PageSetup.Margins.Bottom = 0.2;
                sht.PageSetup.Margins.Header = 0.2;
                sht.PageSetup.Margins.Footer = 0.2;
                sht.PageSetup.CenterHorizontally = true;
                //sht.PageSetup.CenterVertically = true;
                sht.PageSetup.SetScaleHFWithDocument();
                //************
                sht.Column("A").Width = 13.67;
                sht.Column("B").Width = 12.93;
                sht.Column("C").Width = 9.67;
                sht.Column("D").Width = 9.89;
                sht.Column("E").Width = 9.67;
                sht.Column("F").Width = 9.78;
                sht.Column("G").Width = 7.56;
                sht.Column("H").Width = 9.89;
                sht.Column("I").Width = 6.11;
                sht.Column("J").Width = 8.67;
                sht.Column("K").Width = 7.56;
                sht.Column("L").Width = 8.33;
                sht.Column("M").Width = 8.11;

                //************
                sht.Row(1).Height = 13.75;
                //*****Header                
                sht.Cell("A1").Value = "PACKING LIST";
                sht.Range("A1:M1").Style.Font.FontName = "Arial";
                sht.Range("A1:M1").Style.Font.FontSize = 11;
                sht.Range("A1:M1").Style.Font.Bold = true;
                sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:M1").Merge();

                using (var a = sht.Range("A1:M1"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //********Exporter
                sht.Range("A2").Value = "Exporter";
                sht.Range("A2:E2").Style.Font.FontName = "Arial";
                sht.Range("A2:E2").Style.Font.FontSize = 9;
                //sht.Range("A2:D2").Style.Font.Bold = true;
                sht.Range("A2:E2").Merge();
                sht.Range("A2:E2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //*****************

                //CompanyName
                sht.Range("A3").Value = ds.Tables[0].Rows[0]["companyName"];
                sht.Range("A3:E3").Style.Font.FontName = "Arial";
                sht.Range("A3:E3").Style.Font.FontSize = 9;
                sht.Range("A3:E3").Style.Font.Bold = true;
                sht.Range("A3:E3").Merge();
                sht.Range("A3:E3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //Address
                sht.Range("A4").Value = ds.Tables[0].Rows[0]["compaddr1"];
                sht.Range("A4:E4").Style.Font.FontName = "Arial";
                sht.Range("A4:E4").Style.Font.FontSize = 9;
                sht.Range("A4:E4").Merge();
                sht.Range("A4:E4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //address2
                sht.Range("A5").Value = ds.Tables[0].Rows[0]["compaddr2"] + "," + ds.Tables[0].Rows[0]["compaddr3"];
                sht.Range("A5:E5").Style.Font.FontName = "Arial";
                sht.Range("A5:E5").Style.Font.FontSize = 9;
                sht.Range("A5:E5").Merge();
                sht.Range("A5:E5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //TiN No
                sht.Range("A6").Value = "GSTIN#  " + ds.Tables[0].Rows[0]["gstno"];
                sht.Range("A6:E6").Style.Font.FontName = "Arial";
                sht.Range("A6:E6").Style.Font.FontSize = 8;
                sht.Range("A6:E6").Style.Font.Bold = true;
                sht.Range("A6:E6").Merge();
                sht.Range("A6:E6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //STATE 
                sht.Range("A7").Value = "STATE :   UTTAR PRADESH";
                sht.Range("A7:E7").Style.Font.FontName = "Arial";
                sht.Range("A7:E7").Style.Font.FontSize = 8;
                sht.Range("A7:E7").Style.Font.Bold = true;
                sht.Range("A7:E7").Merge();
                sht.Range("A7:E7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //PAN :
                sht.Range("A8").Value = "PAN :  " + ds.Tables[0].Rows[0]["Pannr"];
                sht.Range("A8:E8").Style.Font.FontName = "Arial";
                sht.Range("A8:E8").Style.Font.FontSize = 8;
                sht.Range("A8:E8").Style.Font.Bold = true;
                sht.Range("A8:E8").Merge();
                sht.Range("A8:E8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A8:E8"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("A2:A26"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("E2:E26"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("M2:M26"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }


                //**********Invoice No
                sht.Range("F2").Value = "Invoice No";
                sht.Range("F2:F2").Style.Font.FontName = "Arial";
                sht.Range("F2:F2").Style.Font.FontSize = 9;
                sht.Range("F2:F2").Merge();
                sht.Range("F2:F2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //value
                sht.Range("G2").Value = ds.Tables[0].Rows[0]["TInvoiceNo"];
                sht.Range("G2:I2").Style.Font.FontName = "Arial";
                sht.Range("G2:I2").Style.Font.FontSize = 9;
                sht.Range("G2:I2").Style.Font.Bold = true;
                sht.Range("G2:I2").Merge();
                sht.Range("G2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //**********InvoiceDate
                sht.Range("J2").Value = "DATED";
                sht.Range("J2:J2").Style.Font.FontName = "Arial";
                sht.Range("J2:J2").Style.Font.FontSize = 9;
                sht.Range("J2:J2").Merge();
                sht.Range("J2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //value
                //sht.Range("J2").Value = ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("K2").SetValue(ds.Tables[0].Rows[0]["Invoicedate"]);
                sht.Range("K2:M2").Style.Font.FontName = "Arial";
                sht.Range("K2:M2").Style.Font.FontSize = 9;
                sht.Range("K2:M2").Style.Font.Bold = true;
                sht.Range("K2:M2").Merge();
                sht.Range("K2:M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //sht.Range("J2").Value = DateTime.Now.ToShortDateString();

                using (var a = sht.Range("F2:M2"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //**********BuyerOrderNo
                sht.Range("F3").Value = "Buyer Order No.  :";
                sht.Range("F3:G3").Style.Font.FontName = "Arial";
                sht.Range("F3:G3").Style.Font.FontSize = 9;
                sht.Range("F3:G3").Merge();
                sht.Range("F3:G3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("H3").Value = " As Mentioned Below";
                sht.Range("H3:M3").Style.Font.FontName = "Arial";
                sht.Range("H3:M3").Style.Font.FontSize = 9;
                sht.Range("H3:M3").Style.Font.Bold = true;
                sht.Range("H3:M3").Merge();
                sht.Range("H3:M3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //Value
                sht.Range("F4").Value = ds.Tables[0].Rows[0]["TorderNo"];
                sht.Range("F4:M5").Style.Font.FontName = "Arial";
                sht.Range("F4:M5").Style.Font.FontSize = 9;
                sht.Range("F4:M5").Merge();
                sht.Range("F4:M5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("F4:M5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("F5:M5"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //**********SupplierNo
                sht.Range("F6").Value = "Supplier No.";
                sht.Range("F6:F6").Style.Font.FontName = "Arial";
                sht.Range("F6:F6").Style.Font.FontSize = 9;
                sht.Range("F6:F6").Merge();
                sht.Range("F6:F6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //Value
                sht.Range("G6").Value = "17933";
                sht.Range("G6:H6").Style.Font.FontName = "Arial";
                sht.Range("G6:H6").Style.Font.FontSize = 9;
                sht.Range("G6:H6").Merge();
                sht.Range("G6:H6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("G6:H6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********DESTN. CODE
                sht.Range("I6").Value = "DESTN. CODE";
                sht.Range("I6:J6").Style.Font.FontName = "Arial";
                sht.Range("I6:J6").Style.Font.FontSize = 9;
                sht.Range("I6:J6").Merge();
                sht.Range("I6:J6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                //Value
                ////sht.Range("K6").Value = ds.Tables[0].Rows[0]["DestCode"];
                sht.Range("K6").SetValue(ds.Tables[0].Rows[0]["DestCode"]);
                //sht.Range("J6:J6").Style.NumberFormat.Format = "@";
                sht.Range("K6:M6").Style.Font.FontName = "Arial";
                sht.Range("K6:M6").Style.Font.FontSize = 9;
                sht.Range("K6:M6").Merge();
                sht.Range("K6:M6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("K6:M6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("F6:M6"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //**********CONSIGNMENT NO
                sht.Range("F7").Value = "CONSIGNMENT NO";
                sht.Range("F7:G8").Style.Font.FontName = "Arial";
                sht.Range("F7:G8").Style.Font.FontSize = 9;
                sht.Range("F7:G8").Style.Font.Bold = true;
                sht.Range("F7:G8").Merge();
                sht.Range("F7:G8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("F7:G8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                //**********CONSIGNMENT NO Value
                sht.Range("H7").Value = ds.Tables[0].Rows[0]["otherref"];
                sht.Range("H7:M8").Style.Font.FontName = "Arial";
                sht.Range("H7:M8").Style.Font.FontSize = 9;
                sht.Range("H7:M8").Style.Font.Bold = true;
                sht.Range("H7:M8").Merge();
                sht.Range("H7:M8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("H7:M8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                //**********Consignee
                sht.Range("A9").Value = "Consignee";
                sht.Range("A9:A9").Style.Font.FontName = "Arial";
                sht.Range("A9:A9").Style.Font.FontSize = 9;
                sht.Range("A9:A9").Style.Font.Bold = false;
                sht.Range("A9:A9").Merge();
                sht.Range("A9:A9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A9:A9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Consignee Code Value
                sht.Range("B9").Value = ds.Tables[0].Rows[0]["consignee_dt"];
                sht.Range("B9:E9").Style.Font.FontName = "Arial";
                sht.Range("B9:E9").Style.Font.FontSize = 9;
                sht.Range("B9:E9").Style.Font.Bold = true;
                sht.Range("B9:E9").Merge();
                sht.Range("B9:E9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("B9:E9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Consignee Name Value
                sht.Range("A10").Value = ds.Tables[0].Rows[0]["consignee"];
                sht.Range("A10:E10").Style.Font.FontName = "Arial";
                sht.Range("A10:E10").Style.Font.FontSize = 9;
                sht.Range("A10:E10").Style.Font.Bold = true;
                sht.Range("A10:E10").Merge();
                sht.Range("A10:E10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A10:E10").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Consignee Address Value
                sht.Range("A11").Value = ds.Tables[0].Rows[0]["consignee_address"];
                sht.Range("A11:E13").Style.Font.FontName = "Arial";
                sht.Range("A11:E13").Style.Font.FontSize = 9;
                sht.Range("A11:E13").Style.Font.Bold = false;
                sht.Range("A11:E13").Merge();
                sht.Range("A11:E13").Style.Alignment.SetWrapText();
                sht.Range("A11:E13").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A11:E13").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Blank Row
                sht.Range("A14").Value = "";
                sht.Range("A14:E14").Style.Font.FontName = "Arial";
                sht.Range("A14:E14").Style.Font.FontSize = 9;
                sht.Range("A14:E14").Style.Font.Bold = false;
                sht.Range("A14:E14").Merge();
                sht.Range("A14:E14").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A14:E14").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Notify Party
                sht.Range("A15").Value = "Notify Party";
                sht.Range("A15:A15").Style.Font.FontName = "Arial";
                sht.Range("A15:A15").Style.Font.FontSize = 9;
                sht.Range("A15:A15").Style.Font.Bold = false;
                sht.Range("A15:A15").Merge();
                sht.Range("A15:A15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A15:A15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Notify Party Code Value
                sht.Range("B15").Value = ds.Tables[0].Rows[0]["Notifyparty_dt"];
                sht.Range("B15:E15").Style.Font.FontName = "Arial";
                sht.Range("B15:E15").Style.Font.FontSize = 9;
                sht.Range("B15:E15").Style.Font.Bold = true;
                sht.Range("B15:E15").Merge();
                sht.Range("B15:E15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("B15:E15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Notify Party Name Value
                sht.Range("A16").Value = ds.Tables[0].Rows[0]["Notifyparty"];
                sht.Range("A16:E16").Style.Font.FontName = "Arial";
                sht.Range("A16:E16").Style.Font.FontSize = 9;
                sht.Range("A16:E16").Style.Font.Bold = true;
                sht.Range("A16:E16").Merge();
                sht.Range("A16:E16").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A16:E16").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Notify Party Address Value
                sht.Range("A17").Value = ds.Tables[0].Rows[0]["notifyparty_address"];
                sht.Range("A17:E19").Style.Font.FontName = "Arial";
                sht.Range("A17:E19").Style.Font.FontSize = 9;
                sht.Range("A17:E19").Style.Font.Bold = false;
                sht.Range("A17:E19").Merge();
                sht.Range("A17:E19").Style.Alignment.SetWrapText();
                sht.Range("A17:E19").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A17:E19").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("A19:M19"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //**********Buyer  (If Other than Consignee)
                sht.Range("F9").Value = "Buyer  (If Other than Consignee)";
                sht.Range("F9:M9").Style.Font.FontName = "Arial";
                sht.Range("F9:M9").Style.Font.FontSize = 9;
                sht.Range("F9:M9").Style.Font.Bold = false;
                sht.Range("F9:M9").Merge();
                sht.Range("F9:M9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("F9:M9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Buyer  (If Other than Consignee) Value
                sht.Range("F10").Value = "1." + ds.Tables[0].Rows[0]["buyer_otherthanconsignee"];
                sht.Range("F10:M10").Style.Font.FontName = "Arial";
                sht.Range("F10:M10").Style.Font.FontSize = 9;
                sht.Range("F10:M10").Style.Font.Bold = true;
                sht.Range("F10:M10").Merge();
                sht.Range("F10:M10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("F10:M10").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Buyer  (If Other than Consignee) Value
                sht.Range("F11").Value = ds.Tables[0].Rows[0]["otherthanconsignee_address"];
                sht.Range("F11:M13").Style.Font.FontName = "Arial";
                sht.Range("F11:M13").Style.Font.FontSize = 9;
                sht.Range("F11:M13").Style.Font.Bold = false;
                sht.Range("F11:M13").Merge();
                sht.Range("F11:M13").Style.Alignment.SetWrapText();
                sht.Range("F11:M13").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("F11:M13").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Blank Row
                sht.Range("F14").Value = "";
                sht.Range("F14:M14").Style.Font.FontName = "Arial";
                sht.Range("F14:M14").Style.Font.FontSize = 9;
                sht.Range("F14:M14").Style.Font.Bold = false;
                sht.Range("F14:M14").Merge();
                sht.Range("F14:M14").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("F14:M14").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);


                //********** 2.       PAYING AGENT :
                sht.Range("F15").Value = " 2.PAYING AGENT :";
                sht.Range("F15:G15").Style.Font.FontName = "Arial";
                sht.Range("F15:G15").Style.Font.FontSize = 9;
                sht.Range("F15:G15").Style.Font.Bold = true;
                sht.Range("F15:G15").Merge();
                sht.Range("F15:G15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("F15:G15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //********* 2.       PAYING AGENT Code : Value
                sht.Range("H15").Value = ds.Tables[0].Rows[0]["payingagent"];
                sht.Range("H15:M15").Style.Font.FontName = "Arial";
                sht.Range("H15:M15").Style.Font.FontSize = 9;
                sht.Range("H15:M15").Style.Font.Bold = true;
                sht.Range("H15:M15").Merge();
                sht.Range("H15:M15").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("H15:M15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //********** 2.       PAYING AGENT Address: Value
                sht.Range("H16").Value = ds.Tables[0].Rows[0]["payingagent_address"];
                sht.Range("H16:M19").Style.Font.FontName = "Arial";
                sht.Range("H16:M19").Style.Font.FontSize = 9;
                sht.Range("H16:M19").Style.Font.Bold = false;
                sht.Range("H16:M19").Merge();
                sht.Range("H16:M19").Style.Alignment.SetWrapText();
                sht.Range("H16:M19").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("H16:M19").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);


                //**********Pre-Carriage By:
                sht.Range("A20").Value = "Pre-Carriage By";
                sht.Range("A20:B20").Style.Font.FontName = "Arial";
                sht.Range("A20:B20").Style.Font.FontSize = 9;
                sht.Range("A20:B20").Style.Font.Bold = false;
                sht.Range("A20:B20").Merge();
                sht.Range("A20:B20").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A20:B20").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Pre-Carriage By Value:
                sht.Range("A21").Value = ds.Tables[0].Rows[0]["Pre_CarriageBy"];
                sht.Range("A21:B21").Style.Font.FontName = "Arial";
                sht.Range("A21:B21").Style.Font.FontSize = 9;
                sht.Range("A21:B21").Style.Font.Bold = true;
                sht.Range("A21:B21").Merge();
                sht.Range("A21:B21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A21:B21").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("B20:B26"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                //**********PLACE OF RECEIPT :
                sht.Range("C20").Value = "PLACE OF RECEIPT ";
                sht.Range("C20:E20").Style.Font.FontName = "Arial";
                sht.Range("C20:E20").Style.Font.FontSize = 9;
                sht.Range("C20:E20").Style.Font.Bold = false;
                sht.Range("C20:E20").Merge();
                sht.Range("C20:E20").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("C20:E20").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********PLACE OF RECEIPT  Value:
                sht.Range("C21").Value = ds.Tables[0].Rows[0]["receiptby"];
                sht.Range("C21:E21").Style.Font.FontName = "Arial";
                sht.Range("C21:E21").Style.Font.FontSize = 9;
                sht.Range("C21:E21").Style.Font.Bold = true;
                sht.Range("C21:E21").Merge();
                sht.Range("C21:E21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C21:E21").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("A21:E21"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //**********VESSEL/FLIGHT NO.:
                sht.Range("A22").Value = "VESSEL/FLIGHT NO.";
                sht.Range("A22:B22").Style.Font.FontName = "Arial";
                sht.Range("A22:B22").Style.Font.FontSize = 9;
                sht.Range("A22:B22").Style.Font.Bold = false;
                sht.Range("A22:B22").Merge();
                sht.Range("A22:B22").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A22:B22").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********VESSEL/FLIGHT NO. Value:
                sht.Range("A23").Value = ds.Tables[0].Rows[0]["vessel_flightno"];
                sht.Range("A23:B23").Style.Font.FontName = "Arial";
                sht.Range("A23:B23").Style.Font.FontSize = 9;
                sht.Range("A23:B23").Style.Font.Bold = true;
                sht.Range("A23:B23").Merge();
                sht.Range("A23:B23").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A23:B23").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                //**********PORT OF LOADING  :
                sht.Range("C22").Value = "PORT OF LOADING  ";
                sht.Range("C22:E22").Style.Font.FontName = "Arial";
                sht.Range("C22:E22").Style.Font.FontSize = 9;
                sht.Range("C22:E22").Style.Font.Bold = false;
                sht.Range("C22:E22").Merge();
                sht.Range("C22:E22").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("C22:E22").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********PORT OF LOADING   Value:
                sht.Range("C23").Value = ds.Tables[0].Rows[0]["portload"];
                sht.Range("C23:E23").Style.Font.FontName = "Arial";
                sht.Range("C23:E23").Style.Font.FontSize = 9;
                sht.Range("C23:E23").Style.Font.Bold = true;
                sht.Range("C23:E23").Merge();
                sht.Range("C23:E23").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C23:E23").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("A23:E23"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //**********PORT OF DISCHARGE.:
                sht.Range("A24").Value = "PORT OF DISCHARGE.";
                sht.Range("A24:B24").Style.Font.FontName = "Arial";
                sht.Range("A24:B24").Style.Font.FontSize = 9;
                sht.Range("A24:B24").Style.Font.Bold = false;
                sht.Range("A24:B24").Merge();
                sht.Range("A24:B24").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A24:B24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********PORT OF DISCHARGE Value:
                sht.Range("A25").Value = ds.Tables[0].Rows[0]["portunload"];
                sht.Range("A25:B26").Style.Font.FontName = "Arial";
                sht.Range("A25:B26").Style.Font.FontSize = 9;
                sht.Range("A25:B26").Style.Font.Bold = true;
                sht.Range("A25:B26").Merge();
                sht.Range("A25:B26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A25:B26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                //**********FINAL DESTINATION  :
                sht.Range("C24").Value = "FINAL DESTINATION ";
                sht.Range("C24:E24").Style.Font.FontName = "Arial";
                sht.Range("C24:E24").Style.Font.FontSize = 9;
                sht.Range("C24:E24").Style.Font.Bold = false;
                sht.Range("C24:E24").Merge();
                sht.Range("C24:E24").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("C24:E24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********FINAL DESTINATION  Value:
                sht.Range("C25").Value = ds.Tables[0].Rows[0]["Destinationadd"];
                sht.Range("C25:E26").Style.Font.FontName = "Arial";
                sht.Range("C25:E26").Style.Font.FontSize = 9;
                sht.Range("C25:E26").Style.Font.Bold = true;
                sht.Range("C25:E26").Merge();
                sht.Range("C25:E26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C25:E26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("A26:E26"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //**********COUNTRY OF ORIGIN  :
                sht.Range("F20").Value = "COUNTRY OF ORIGIN ";
                sht.Range("F20:I20").Style.Font.FontName = "Arial";
                sht.Range("F20:I20").Style.Font.FontSize = 9;
                sht.Range("F20:I20").Style.Font.Bold = false;
                sht.Range("F20:I20").Merge();
                sht.Range("F20:I20").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("F20:I20").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********COUNTRY OF ORIGIN  Value:
                sht.Range("F21").Value = ds.Tables[0].Rows[0]["countryoforigin"];
                sht.Range("F21:I21").Style.Font.FontName = "Arial";
                sht.Range("F21:I21").Style.Font.FontSize = 9;
                sht.Range("F21:I21").Style.Font.Bold = true;
                sht.Range("F21:I21").Merge();
                sht.Range("F21:I21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("F21:I21").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("I20:I21"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                //**********COUNTRY OF FINAL DESTINATION :
                sht.Range("J20").Value = "COUNTRY OF FINAL DESTINATION ";
                sht.Range("J20:M20").Style.Font.FontName = "Arial";
                sht.Range("J20:M20").Style.Font.FontSize = 9;
                sht.Range("J20:M20").Style.Font.Bold = false;
                sht.Range("J20:M20").Merge();
                sht.Range("J20:M20").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("J20:M20").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********COUNTRY OF FINAL DESTINATION  Value:
                sht.Range("J21").Value = ds.Tables[0].Rows[0]["countryoffinaldest"];
                sht.Range("J21:M21").Style.Font.FontName = "Arial";
                sht.Range("J21:M21").Style.Font.FontSize = 9;
                sht.Range("J21:M21").Style.Font.Bold = true;
                sht.Range("J21:M21").Merge();
                sht.Range("J21:M21").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("J21:M21").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("F21:M21"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //**********Terms of Delivery and Payment  :
                sht.Range("F22").Value = "Terms of Delivery and Payment";
                sht.Range("F22:M22").Style.Font.FontName = "Arial";
                sht.Range("F22:M22").Style.Font.FontSize = 9;
                sht.Range("F22:M22").Style.Font.Bold = false;
                sht.Range("F22:M22").Merge();
                sht.Range("F22:M22").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("F22:M22").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //***************BlankRow
                sht.Range("F23").Value = "";
                sht.Range("F23:M23").Style.Font.FontName = "Arial";
                sht.Range("F23:M23").Style.Font.FontSize = 9;
                sht.Range("F23:M23").Style.Font.Bold = false;
                sht.Range("F23:M23").Merge();
                sht.Range("F23:M23").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("F23:M23").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //********** Terms of Payment :  :
                sht.Range("F24").Value = " Terms of Payment : " + " " + ds.Tables[0].Rows[0]["Deliveryterm"];
                sht.Range("F24:M24").Style.Font.FontName = "Arial";
                sht.Range("F24:M24").Style.Font.FontSize = 9;
                sht.Range("F24:M24").Style.Font.Bold = true;
                sht.Range("F24:M24").Merge();
                sht.Range("F24:M24").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("F24:M24").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //********** DELIVERY WEEK :  :
                sht.Range("F25").Value = " DELIVERY WEEK  :" + " " + ds.Tables[0].Rows[0]["DELVWK"];
                sht.Range("F25:M25").Style.Font.FontName = "Arial";
                sht.Range("F25:M25").Style.Font.FontSize = 9;
                sht.Range("F25:M25").Style.Font.Bold = true;
                sht.Range("F25:M25").Merge();
                sht.Range("F25:M25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("F25:M25").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //***************BlankRow
                sht.Range("F26").Value = "";
                sht.Range("F26:M26").Style.Font.FontName = "Arial";
                sht.Range("F26:M26").Style.Font.FontSize = 9;
                sht.Range("F26:M26").Style.Font.Bold = false;
                sht.Range("F26:M26").Merge();
                sht.Range("F26:M26").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("F26:M26").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("F26:M26"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //**********Marks Nos./ Container No.:
                sht.Range("A27").Value = "Marks Nos./ Container No.";
                sht.Range("A27:B27").Style.Font.FontName = "Arial";
                sht.Range("A27:B27").Style.Font.FontSize = 9;
                sht.Range("A27:B27").Style.Font.Bold = false;
                sht.Range("A27:B27").Merge();
                sht.Range("A27:B27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A27:B27").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Marks Nos./ Container No.:
                sht.Range("A28").Value = "IKEA/" + ds.Tables[0].Rows[0]["countryoffinaldest"];
                sht.Range("A28:B28").Style.Font.FontName = "Arial";
                sht.Range("A28:B28").Style.Font.FontSize = 9;
                sht.Range("A28:B28").Style.Font.Bold = false;
                sht.Range("A28:B28").Merge();
                sht.Range("A28:B28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A28:B28").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********Marks Nos./ Container No Value.:
                sht.Range("A32").Value = "No" + " " + ds.Tables[0].Compute("Min(MinrollNo)", "") + " " + " to" + " " + ds.Tables[0].Compute("Max(Maxrollno)", "");
                sht.Range("A32:B32").Style.Font.FontName = "Arial";
                sht.Range("A32:B32").Style.Font.FontSize = 9;
                sht.Range("A32:B32").Style.Font.Bold = true;
                sht.Range("A32:B32").Merge();
                sht.Range("A32:B32").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A32:B32").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //**********No. of Pkgs.:
                sht.Range("C27").Value = "No. of Pkgs.";
                sht.Range("C27:E27").Style.Font.FontName = "Arial";
                sht.Range("C27:E27").Style.Font.FontSize = 9;
                sht.Range("C27:E27").Style.Font.Bold = false;
                sht.Range("C27:E27").Merge();
                sht.Range("C27:E27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C27:E27").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                //**********No. of Pkgs Value.:
                sht.Range("C28").Value = ds.Tables[0].Rows[0]["Noofrolls"];
                sht.Range("C28:E28").Style.Font.FontName = "Arial";
                sht.Range("C28:E28").Style.Font.FontSize = 9;
                sht.Range("C28:E28").Style.Font.Bold = true;
                sht.Range("C28:E28").Merge();
                sht.Range("C28:E28").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C28:E28").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                //**********No. of Pkgs Value.:
                sht.Range("C29").Value = "Pallets";
                sht.Range("C29:E29").Style.Font.FontName = "Arial";
                sht.Range("C29:E29").Style.Font.FontSize = 9;
                sht.Range("C29:E29").Style.Font.Bold = true;
                sht.Range("C29:E29").Merge();
                sht.Range("C29:E29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C29:E29").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                //*********DESCRIPTION OF GOODS :
                sht.Range("F27").Value = "DESCRIPTION OF GOODS ";
                sht.Range("F27:M27").Style.Font.FontName = "Arial";
                sht.Range("F27:M27").Style.Font.FontSize = 9;
                sht.Range("F27:M27").Style.Font.Bold = true;
                sht.Range("F27:M27").Merge();
                sht.Range("F27:M27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("F27:M27").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                //*********DESCRIPTION OF GOODS Value :
                sht.Range("F28").Value = ds.Tables[0].Rows[0]["descriptionofgoods"];
                sht.Range("F28:M30").Style.Font.FontName = "Arial";
                sht.Range("F28:M30").Style.Font.FontSize = 9;
                sht.Range("F28:M30").Style.Font.Bold = true;
                sht.Range("F28:M30").Merge();
                sht.Range("F28:M30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("F28:M30").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("A32:M32"))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("A27:A32"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("M27:M32"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                ////********Details

                sht.Range("A33").Value = "P.O. NOS";
                sht.Range("A33:A35").Style.Font.FontName = "Arial";
                sht.Range("A33:A35").Style.Font.FontSize = 9;
                sht.Range("A33:A35").Style.Font.Bold = true;
                sht.Range("A33:A35").Merge();
                sht.Range("A33:A35").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A33:A35").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A33:A35").Style.Alignment.SetWrapText();

                sht.Range("B33").Value = "PALLET";
                sht.Range("B33:B35").Style.Font.FontName = "Arial";
                sht.Range("B33:B35").Style.Font.FontSize = 9;
                sht.Range("B33:B35").Style.Font.Bold = true;
                sht.Range("B33:B35").Merge();
                sht.Range("B33:B35").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B33:B35").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B33:B35").Style.Alignment.SetWrapText();

                sht.Range("C33").Value = "PCS/"+Environment.NewLine+" Pallet";
                sht.Range("C33:C35").Style.Font.FontName = "Arial";
                sht.Range("C33:C35").Style.Font.FontSize = 9;
                sht.Range("C33:C35").Style.Font.Bold = true;
                sht.Range("C33:C35").Merge();
                sht.Range("C33:C35").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C33:C35").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("C33:C35").Style.Alignment.SetWrapText();

                sht.Range("D33").Value = "ART. NOS.";
                sht.Range("D33:D35").Style.Font.FontName = "Arial";
                sht.Range("D33:D35").Style.Font.FontSize = 9;
                sht.Range("D33:D35").Style.Font.Bold = true;
                sht.Range("D33:D35").Merge();
                sht.Range("D33:D35").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("D33:D35").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("D33:D35").Style.Alignment.SetWrapText();

                sht.Range("E33").Value = "DESCRIPTION, COLOUR & SIZE";
                sht.Range("E33:H35").Style.Font.FontName = "Arial";
                sht.Range("E33:H35").Style.Font.FontSize = 9;
                sht.Range("E33:H35").Style.Font.Bold = true;
                sht.Range("E33:H35").Merge();
                sht.Range("E33:H35").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E33:H35").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("E33:H35").Style.Alignment.SetWrapText();

                sht.Range("I33").Value = "QTY PCS";
                sht.Range("I33:I35").Style.Font.FontName = "Arial";
                sht.Range("I33:I35").Style.Font.FontSize = 9;
                sht.Range("I33:I35").Style.Font.Bold = true;
                sht.Range("I33:I35").Merge();
                sht.Range("I33:I35").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("I33:I35").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("I33:I35").Style.Alignment.SetWrapText();

                sht.Range("J33").Value = "TOTAL NT.WT";
                sht.Range("J33:J35").Style.Font.FontName = "Arial";
                sht.Range("J33:J35").Style.Font.FontSize = 9;
                sht.Range("J33:J35").Style.Font.Bold = true;
                sht.Range("J33:J35").Merge();
                sht.Range("J33:J35").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("J33:J35").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("J33:J35").Style.Alignment.SetWrapText();

                sht.Range("K33").Value = "NT.WT/"+Environment.NewLine+"Pallet";
                sht.Range("K33:K35").Style.Font.FontName = "Arial";
                sht.Range("K33:K35").Style.Font.FontSize = 9;
                sht.Range("K33:K35").Style.Font.Bold = true;
                sht.Range("K33:K35").Merge();
                sht.Range("K33:K35").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("K33:K35").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("K33:K35").Style.Alignment.SetWrapText();

                sht.Range("L33").Value = "TOTAL GR. WT.";
                sht.Range("L33:L35").Style.Font.FontName = "Arial";
                sht.Range("L33:L35").Style.Font.FontSize = 9;
                sht.Range("L33:L35").Style.Font.Bold = true;
                sht.Range("L33:L35").Merge();
                sht.Range("L33:L35").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("L33:L35").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("L33:L35").Style.Alignment.SetWrapText();

                sht.Range("M33").Value = "GR.WT/"+Environment.NewLine+"Pallet";
                sht.Range("M33:M35").Style.Font.FontName = "Arial";
                sht.Range("M33:M35").Style.Font.FontSize = 9;
                sht.Range("M33:M35").Style.Font.Bold = true;
                sht.Range("M33:M35").Merge();
                sht.Range("M33:M35").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("M33:M35").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("M33:M35").Style.Alignment.SetWrapText();

                using (var a = sht.Range("A33:M35"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                int row = 36;
                int totalrowcount = 50;
                int rowcount = ds.Tables[0].Rows.Count;
                totalrowcount = totalrowcount - rowcount;
                decimal ArticleRollWiseNetWt = 0, ArticleRollWiseGrossWt = 0;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":J" + row).Style.Font.FontName = "Arial";
                    sht.Range("A" + row + ":J" + row).Style.Font.FontSize = 10;

                    // pono
                    sht.Range("A" + row).Value = ds.Tables[0].Rows[i]["Pono"];
                    sht.Range("B" + row).Value = ds.Tables[0].Rows[i]["MinrollNo"] +" "+ "-" +" "+ds.Tables[0].Rows[i]["MaxrollNo"]+ " "+"("+ds.Tables[0].Rows[i]["TotalRoll"]+" "+"Pallet)";
                    sht.Range("C" + row).Value = ds.Tables[0].Rows[i]["PcsPerRoll"];
                    sht.Range("D" + row).Value = ds.Tables[0].Rows[i]["articleno"];
                    sht.Range("E" + row + ":H" + row).Merge();
                    sht.Range("E" + row).Value = ds.Tables[0].Rows[i]["Quality"] + " " + ds.Tables[0].Rows[i]["Design"] + " " + ds.Tables[0].Rows[i]["Color"] + " " + ds.Tables[0].Rows[i]["Width"] + "X" + ds.Tables[0].Rows[i]["Length"];
                    sht.Range("E" + row).Style.Alignment.SetWrapText();
                    sht.Range("I" + row).Value = ds.Tables[0].Rows[i]["Pcs"];
                    Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[i]["Pcs"]);
                    
                    sht.Range("J" + row).Value = Convert.ToDecimal(ds.Tables[0].Rows[i]["ArticleNoWiseNetWt"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalRoll"]);
                    ArticleRollWiseNetWt = ArticleRollWiseNetWt + Convert.ToDecimal(ds.Tables[0].Rows[i]["ArticleNoWiseNetWt"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalRoll"]);
                    sht.Range("J" + row).Style.NumberFormat.Format = "#,###0.000";

                    sht.Range("K" + row).Value = ds.Tables[0].Rows[i]["ArticleNoWiseNetWt"];
                    sht.Range("K" + row).Style.NumberFormat.Format = "#,###0.000";

                    sht.Range("L" + row).Value = Convert.ToDecimal(ds.Tables[0].Rows[i]["ArticleNoWiseGrossWt"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalRoll"]);
                    ArticleRollWiseGrossWt = ArticleRollWiseGrossWt + Convert.ToDecimal(ds.Tables[0].Rows[i]["ArticleNoWiseGrossWt"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalRoll"]);
                    sht.Range("L" + row).Style.NumberFormat.Format = "#,###0.000";

                    sht.Range("M" + row).Value = ds.Tables[0].Rows[i]["ArticleNoWiseGrossWt"];
                    sht.Range("M" + row).Style.NumberFormat.Format = "#,###0.000";

                    //sht.Range("G" + row).Value = ds.Tables[0].Rows[i]["HSNCode"];
                    //sht.Range("H" + row).Value = ds.Tables[0].Rows[i]["Pcs"];
                    //Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[i]["Pcs"]);
                    //sht.Range("I" + row).Value = ds.Tables[0].Rows[i]["Price"];
                    //sht.Range("J" + row).Value = ds.Tables[0].Rows[i]["amount"];
                    //sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                    //Amount = Amount + Convert.ToDecimal(ds.Tables[0].Rows[i]["amount"]);

                    Area = Convert.ToDecimal(Area.ToString()) + Convert.ToDecimal(ds.Tables[0].Rows[i]["Area"].ToString());

                    using (var a = sht.Range("A" + row + ":M" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    row = row + 1;
                }

                if (totalrowcount > 0)
                {
                    for (int j = row; j < totalrowcount - 1; j++)
                    {
                        sht.Range("A" + row + ":M" + row).Style.Font.FontName = "Arial";
                        sht.Range("A" + row + ":M" + row).Style.Font.FontSize = 10;
                        sht.Range("A" + row + ":M" + row).Style.Font.SetBold();
                        sht.Range("A" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                 
                        sht.Range("A" + row).SetValue("");
                        sht.Range("B" + row).SetValue("");
                        sht.Range("C" + row).SetValue("");
                        sht.Range("D" + row).SetValue("");
                        sht.Range("E" + row).SetValue("");
                        sht.Range("F" + row).SetValue("");
                        sht.Range("G" + row).SetValue("");
                        sht.Range("H" + row).SetValue("");
                        sht.Range("I" + row).SetValue("");
                        sht.Range("J" + row).SetValue("");
                        sht.Range("K" + row).SetValue("");
                        sht.Range("L" + row).SetValue("");
                        sht.Range("M" + row).SetValue("");


                        using (var a = sht.Range("A" + row + ":M" + row))
                        {
                            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            //a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        }
                        row = row + 1;
                    }
                }


                sht.Range("A" + row + ":A" + row).Merge();
                sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":A" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "Supplier# 17933";
                sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row + ":M" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":A" + row).Merge();
                sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":A" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "ECIS #" + ds.Tables[0].Rows[0]["EcisNo"];
                sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row + ":M" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                string CategoryName = "";
                string ArticleNo = "";
                decimal NetWeight = 0;
                string ArticleWithNetWt = "";

                for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                {
                    CategoryName = ds.Tables[0].Rows[m]["Category_Name"].ToString();
                    // if (CategoryName == "CARPET")
                    if (CategoryName == "CUSHION")
                    {
                        ArticleNo = ds.Tables[0].Rows[m]["ArticleNo"].ToString();
                        NetWeight = Convert.ToDecimal(ds.Tables[0].Rows[m]["ArticleNoWiseNetWt"]) * Convert.ToDecimal(ds.Tables[0].Rows[m]["ArticleWiseNoOfRoll"]);

                        //ArticleWithNetWt = " Art. No. " + ArticleNo + " " + "Total Wt" + " " + NetWeight;
                        ArticleWithNetWt = ArticleWithNetWt + " Art. No. " + ArticleNo + " " + "Total Wt" + " " + NetWeight + Environment.NewLine;
                    }
                }

                sht.Range("A" + row + ":A" + row).Merge();
                sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":A" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "";
                sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("B" + row + ":B" + row).Merge();
                sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Arial";
                sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 9;
                sht.Range("B" + row + ":B" + row).Style.Font.Bold = true;
                sht.Range("B" + row).Value = "";
                sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("C" + row + ":C" + row).Merge();
                sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("C" + row).Value = "";
                sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("D" + row + ":G" + (row + 2)).Merge();
                sht.Range("D" + row + ":G" + (row + 2)).Style.Font.FontName = "Arial";
                sht.Range("D" + row + ":G" + (row + 2)).Style.Font.FontSize = 9;
                sht.Range("D" + row + ":G" + (row + 2)).Style.Font.Bold = true;
                sht.Range("D" + row).Value = ArticleWithNetWt;
                sht.Range("D" + row + ":G" + (row + 2)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("D" + row + ":G" + (row + 2)).Style.Alignment.SetWrapText();

                //sht.Range("G" + row + ":G" + row).Merge();
                //sht.Range("G" + row + ":G" + row).Style.Font.FontName = "Arial";
                //sht.Range("G" + row + ":G" + row).Style.Font.FontSize = 9;
                //sht.Range("G" + row + ":G" + row).Style.Font.Bold = true;
                //sht.Range("G" + row).Value = "";
                //sht.Range("G" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("H" + row + ":H" + row).Merge();
                sht.Range("H" + row + ":H" + row).Style.Font.FontName = "Arial";
                sht.Range("H" + row + ":H" + row).Style.Font.FontSize = 9;
                sht.Range("H" + row + ":H" + row).Style.Font.Bold = true;
                sht.Range("H" + row).Value = "";
                sht.Range("H" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("I" + row + ":I" + row).Merge();
                sht.Range("I" + row + ":I" + row).Style.Font.FontName = "Arial";
                sht.Range("I" + row + ":I" + row).Style.Font.FontSize = 9;
                sht.Range("I" + row + ":I" + row).Style.Font.Bold = true;
                sht.Range("I" + row).Value = "";
                sht.Range("I" + row + ":I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("J" + row + ":J" + row).Merge();
                sht.Range("J" + row + ":J" + row).Style.Font.FontName = "Arial";
                sht.Range("J" + row + ":J" + row).Style.Font.FontSize = 9;
                sht.Range("J" + row + ":J" + row).Style.Font.Bold = true;
                sht.Range("J" + row).Value = "";
                sht.Range("J" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("K" + row + ":K" + row).Merge();
                sht.Range("K" + row + ":K" + row).Style.Font.FontName = "Arial";
                sht.Range("K" + row + ":K" + row).Style.Font.FontSize = 9;
                sht.Range("K" + row + ":K" + row).Style.Font.Bold = true;
                sht.Range("K" + row).Value = "";
                sht.Range("K" + row + ":K" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("L" + row + ":L" + row).Merge();
                sht.Range("L" + row + ":L" + row).Style.Font.FontName = "Arial";
                sht.Range("L" + row + ":L" + row).Style.Font.FontSize = 9;
                sht.Range("L" + row + ":L" + row).Style.Font.Bold = true;
                sht.Range("L" + row).Value = "";
                sht.Range("L" + row + ":L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("M" + row + ":M" + row).Merge();
                sht.Range("M" + row + ":M" + row).Style.Font.FontName = "Arial";
                sht.Range("M" + row + ":M" + row).Style.Font.FontSize = 9;
                sht.Range("M" + row + ":M" + row).Style.Font.Bold = true;
                sht.Range("M" + row).Value = "";
                sht.Range("M" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row + ":M" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "Total Sqmtr." + Area;
                sht.Range("A" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row + ":M" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                ////sht.Range("C" + row + ":F" + row).Merge();
                ////sht.Range("C" + row + ":F" + row).Style.Font.FontName = "Arial";
                ////sht.Range("C" + row + ":F" + row).Style.Font.FontSize = 9;
                ////sht.Range("C" + row + ":F" + row).Style.Font.Bold = true;
                ////sht.Range("C" + row).Value = "For Art. No." + "    " ;
                ////sht.Range("C" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("H" + row + ":H" + row).Merge();
                sht.Range("H" + row + ":H" + row).Style.Font.FontName = "Arial";
                sht.Range("H" + row + ":H" + row).Style.Font.FontSize = 9;
                sht.Range("H" + row + ":H" + row).Style.Font.Bold = true;
                sht.Range("H" + row).Value = "Total.";
                sht.Range("H" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("I" + row + ":I" + row).Merge();
                sht.Range("I" + row + ":I" + row).Style.Font.FontName = "Arial";
                sht.Range("I" + row + ":I" + row).Style.Font.FontSize = 9;
                sht.Range("I" + row + ":I" + row).Style.Font.Bold = true;
                sht.Range("I" + row).Value = Pcs;
                sht.Range("I" + row + ":I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("J" + row + ":J" + row).Merge();
                sht.Range("J" + row + ":J" + row).Style.Font.FontName = "Arial";
                sht.Range("J" + row + ":J" + row).Style.Font.FontSize = 9;
                sht.Range("J" + row + ":J" + row).Style.Font.Bold = true;
                sht.Range("J" + row).Value = ArticleRollWiseNetWt;
                sht.Range("J" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("J" + row).Style.NumberFormat.Format = "#,###0.000";

                sht.Range("L" + row + ":L" + row).Merge();
                sht.Range("L" + row + ":L" + row).Style.Font.FontName = "Arial";
                sht.Range("L" + row + ":L" + row).Style.Font.FontSize = 9;
                sht.Range("L" + row + ":L" + row).Style.Font.Bold = true;
                sht.Range("L" + row).Value = ArticleRollWiseGrossWt;
                sht.Range("L" + row + ":L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("L" + row).Style.NumberFormat.Format = "#,###0.000";

                using (var a = sht.Range("A" + row + ":M" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;
               
                string AllArticleNo = "";
                int NoOfPallet = 0;
                int NoOfPalletPcsPerRoll = 0;
                string ArticleWithPalletPcs = "";

                for (int n = 0; n < ds.Tables[0].Rows.Count; n++)
                {
                    AllArticleNo = ds.Tables[0].Rows[n]["ArticleNo"].ToString();
                    NoOfPallet = Convert.ToInt32(ds.Tables[0].Rows[n]["TotalRoll"]);
                    NoOfPalletPcsPerRoll = Convert.ToInt32(ds.Tables[0].Rows[n]["PcsPerRoll"]);                   

                    //ArticleWithNetWt = " Art. No. " + ArticleNo + " " + "Total Wt" + " " + NetWeight;
                    ArticleWithPalletPcs = ArticleWithPalletPcs + " Art. No. " + AllArticleNo + " " + "-" + " " + "(" + NoOfPallet + "Pallet X" + " " + NoOfPalletPcsPerRoll+" "+"Pcs/Pallet)" + Environment.NewLine;
                    
                }

                ////ArticleWithPalletPcs               
                sht.Range("A" + row + ":M" + (row + 9)).Merge();
                sht.Range("A" + row + ":M" + (row + 9)).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":M" + (row + 9)).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":M" + (row + 9)).Style.Font.Bold = false;
                sht.Range("A" + row).Value = ArticleWithPalletPcs;
                sht.Range("A" + row + ":M" + (row + 9)).Style.Alignment.SetWrapText();
                sht.Range("A" + row + ":M" + (row + 9)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row + ":M" + (row + 9)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("A" + row + ":M" + (row + 9)))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                row = row + 10;

                sht.Range("A" + row + ":I" + row).Merge();
                sht.Range("A" + row + ":I" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":I" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":I" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "STATEMENT OF ORIGIN";
                sht.Range("A" + row + ":I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row + ":I" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("J" + row + ":M" + row).Merge();
                sht.Range("J" + row + ":M" + row).Style.Font.FontName = "Arial";
                sht.Range("J" + row + ":M" + row).Style.Font.FontSize = 9;
                sht.Range("J" + row + ":M" + row).Style.Font.Bold = true;
                sht.Range("J" + row).Value = "";
                sht.Range("J" + row + ":M" + row).Style.Alignment.SetWrapText();
                sht.Range("J" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("J" + row + ":M" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("I" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("M" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("A" + row + ":I" + row))
                {
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Row(row).Height = 42;               

                sht.Range("A" + row + ":I" + row).Merge();
                sht.Range("A" + row + ":I" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":I" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":I" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Value = "The Exporter INREX0694000965EC026 of the products covered by this document declares that, except where otherwise clearly indicated, these products are of INDIA preferential origin according to rules of origin of the Generalized system of preferences of the European Union and that the origin criterion met is P";
                sht.Range("A" + row + ":I" + row).Style.Alignment.SetWrapText();
                sht.Range("A" + row + ":I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row + ":I" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                sht.Range("J" + row + ":M" + row).Merge();
                sht.Range("J" + row + ":M" + row).Style.Font.FontName = "Arial";
                sht.Range("J" + row + ":M" + row).Style.Font.FontSize = 9;
                sht.Range("J" + row + ":M" + row).Style.Font.Bold = true;
                sht.Range("J" + row).Value = "";
                sht.Range("J" + row + ":M" + row).Style.Alignment.SetWrapText();
                sht.Range("J" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("J" + row + ":M" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("A" + row + ":A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("I" + row + ":I" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("M" + row + ":M" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                using (var a = sht.Range("A" + row + ":I" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
             

                row = row + 1;
                ////Blank Rows               
                sht.Range("A" + row + ":M" + (row + 5)).Merge();
                sht.Range("A" + row + ":M" + (row + 5)).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":M" + (row + 5)).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":M" + (row + 5)).Style.Font.Bold = false;
                sht.Range("A" + row).Value ="";
                sht.Range("A" + row + ":M" + (row + 5)).Style.Alignment.SetWrapText();
                sht.Range("A" + row + ":M" + (row + 5)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row + ":M" + (row + 5)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                using (var a = sht.Range("A" + row + ":M" + (row + 5)))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                row = row + 6;

                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = false;
                sht.Range("A" + row).Value = "TOTAL PKGS.        :";
                sht.Range("A" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("C" + row + ":F" + row).Merge();
                sht.Range("C" + row + ":F" + row).Style.Font.FontName = "Arial";
                sht.Range("C" + row + ":F" + row).Style.Font.FontSize = 9;
                sht.Range("C" + row + ":F" + row).Style.Font.Bold = true;
                sht.Range("C" + row).Value = ds.Tables[0].Rows[0]["NoOfRolls"] + " " + "Pallets(Total" + " " + Pcs + " " + "Pcs Packed in " + " " + ds.Tables[0].Rows[0]["NoOfRolls"] + " " + "Pallets)";
                sht.Range("C" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("M" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = false;
                sht.Range("A" + row).Value = " TOTAL QUANTITY :";
                sht.Range("A" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("C" + row + ":C" + row).Merge();
                sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("C" + row).Value = Pcs;
                sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("D" + row + ":D" + row).Merge();
                sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 9;
                sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                sht.Range("D" + row).Value = "PCS.";
                sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("M" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = false;
                sht.Range("A" + row).Value = " TOTAL GR WT.     :";
                sht.Range("A" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("C" + row + ":C" + row).Merge();
                sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("C" + row).Value = ds.Tables[0].Rows[0]["GrossWt"];
                sht.Range("C" + row).Style.NumberFormat.Format = "#,###0.000";
                sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("D" + row + ":D" + row).Merge();
                sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 9;
                sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                sht.Range("D" + row).Value = "KGS.";
                sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("I" + row + ":M" + row).Merge();
                sht.Range("I" + row + ":M" + row).Style.Font.FontName = "Arial";
                sht.Range("I" + row + ":M" + row).Style.Font.FontSize = 9;
                sht.Range("I" + row + ":M" + row).Style.Font.Bold = true;
                sht.Range("I" + row).Value = " Signature & Date";
                sht.Range("I" + row + ":M" + row).Style.Alignment.SetWrapText();
                sht.Range("I" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("I" + row + ":M" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("I" + row + ":M" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("M" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = false;
                sht.Range("A" + row).Value = " TOTA  NT WT.       :";
                sht.Range("A" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("C" + row + ":C" + row).Merge();
                sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("C" + row).Value = ds.Tables[0].Rows[0]["NetWt"];
                sht.Range("C" + row).Style.NumberFormat.Format = "#,###0.000";
                sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("D" + row + ":D" + row).Merge();
                sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 9;
                sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                sht.Range("D" + row).Value = "KGS.";
                sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("I" + row + ":M" + row).Merge();
                sht.Range("I" + row + ":M" + row).Style.Font.FontName = "Arial";
                sht.Range("I" + row + ":M" + row).Style.Font.FontSize = 9;
                sht.Range("I" + row + ":M" + row).Style.Font.Bold = true;
                sht.Range("I" + row).Value = " ";
                sht.Range("I" + row + ":M" + row).Style.Alignment.SetWrapText();
                sht.Range("I" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("I" + row + ":M" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("I" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("M" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = false;
                sht.Range("A" + row).Value = " VOLUME               :";
                sht.Range("A" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("C" + row + ":C" + row).Merge();
                sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("C" + row).Value = ds.Tables[0].Rows[0]["Volume"];
                sht.Range("C" + row).Style.NumberFormat.Format = "#,###0.000";
                sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("D" + row + ":D" + row).Merge();
                sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 9;
                sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                sht.Range("D" + row).Value = "CBM.";
                sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                sht.Range("I" + row + ":M" + row).Merge();
                sht.Range("I" + row + ":M" + row).Style.Font.FontName = "Arial";
                sht.Range("I" + row + ":M" + row).Style.Font.FontSize = 9;
                sht.Range("I" + row + ":M" + row).Style.Font.Bold = true;
                sht.Range("I" + row).Value = " ";
                sht.Range("I" + row + ":M" + row).Style.Alignment.SetWrapText();
                sht.Range("I" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("I" + row + ":M" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("I" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("M" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }    
         
                row = row + 1;

                //Blank Rows
                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = false;
                sht.Range("A" + row).Value = "";
                sht.Range("A" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("C" + row + ":C" + row).Merge();
                sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("C" + row).Value = "";
                //sht.Range("C" + row).Style.NumberFormat.Format = "#,###0.000";
                sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("D" + row + ":D" + row).Merge();
                sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 9;
                sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                sht.Range("D" + row).Value = "";
                sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                sht.Range("I" + row + ":M" + row).Merge();
                sht.Range("I" + row + ":M" + row).Style.Font.FontName = "Arial";
                sht.Range("I" + row + ":M" + row).Style.Font.FontSize = 9;
                sht.Range("I" + row + ":M" + row).Style.Font.Bold = true;
                sht.Range("I" + row).Value = " ";
                sht.Range("I" + row + ":M" + row).Style.Alignment.SetWrapText();
                sht.Range("I" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("I" + row + ":M" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("I" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("M" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;

                //Blank Rows
                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 9;
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = false;
                sht.Range("A" + row).Value = "";
                sht.Range("A" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("C" + row + ":C" + row).Merge();
                sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 9;
                sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                sht.Range("C" + row).Value = "";
                //sht.Range("C" + row).Style.NumberFormat.Format = "#,###0.000";
                sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("D" + row + ":D" + row).Merge();
                sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 9;
                sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                sht.Range("D" + row).Value = "";
                sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                sht.Range("I" + row + ":M" + row).Merge();
                sht.Range("I" + row + ":M" + row).Style.Font.FontName = "Arial";
                sht.Range("I" + row + ":M" + row).Style.Font.FontSize = 9;
                sht.Range("I" + row + ":M" + row).Style.Font.Bold = true;
                sht.Range("I" + row).Value = " ";
                sht.Range("I" + row + ":M" + row).Style.Alignment.SetWrapText();
                sht.Range("I" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("I" + row + ":M" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                using (var a = sht.Range("I" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("A" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("M" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                using (var a = sht.Range("A" + row + ":M" + row))
                {                   
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

               // row = row + 1;  


                string Fileextension = "xlsx";
                string filename = "PackingList-" + validateFilename(ds.Tables[0].Rows[0]["TinvoiceNo"].ToString()) + "." + Fileextension + "";
                Path = Server.MapPath("~/Packingexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
                lblmsg.Text = "Packing Excel Format downloaded successfully.";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "AltR1", "alert('No Record Found to Export')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }

    protected void Exporttoexcel_Packing_Eastern_Ikea()
    {
        lblmsg.Text = "Wait For Excel File to Download";
        int i = 0, Pcs = 0, TotalRolls = 0;
        Decimal Area = 0;
        string Path = "";
        if (!Directory.Exists(Server.MapPath("~/PackingExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/PackingExcel/"));
        }
        try
        {
            string str = @"select *,VD.PcsRoll as Pcsperroll,VD.Pcs/VD.PcsRoll as TotalRoll From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId Where VI.invoiceid=" + DDinvoiceNo.SelectedValue + " order by MinrollNo";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("PackingList");
                //************set cell width
                //Page
                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                sht.PageSetup.AdjustTo(71);
                //sht.PageSetup.FitToPages(1, 5);                
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                sht.PageSetup.VerticalDpi = 300;
                sht.PageSetup.HorizontalDpi = 300;
                //
                sht.PageSetup.Margins.Top = 0.55;
                sht.PageSetup.Margins.Left = 0;
                sht.PageSetup.Margins.Right = 0;
                sht.PageSetup.Margins.Bottom = 0.53;
                sht.PageSetup.Margins.Header = 0.5;
                sht.PageSetup.Margins.Footer = 0.5;
                sht.PageSetup.CenterHorizontally = true;
                sht.PageSetup.SetScaleHFWithDocument();
                //************
                sht.Column("A").Width = 10.29;
                sht.Column("B").Width = 8.43;
                sht.Column("C").Width = 8.43;
                sht.Column("D").Width = 11.00;
                sht.Column("E").Width = 9.71;
                sht.Column("F").Width = 12.57;
                sht.Column("G").Width = 10.14;
                sht.Column("H").Width = 13.14;
                sht.Column("I").Width = 11.71;
                sht.Column("J").Width = 14.29;
                sht.Column("K").Width = 19.29;
                //************
                sht.Row(1).Height = 15.75;
                //********Header
                sht.Range("A1").Value = "PACKING LIST";
                sht.Range("A1:K1").Style.Font.FontName = "Tahoma";
                sht.Range("A1:K1").Style.Font.FontSize = 12;
                sht.Range("A1:K1").Style.Font.Bold = true;
                sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:K1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1:K1").Style.NumberFormat.Format = "@";
                sht.Range("A1:K1").Merge();
                //********Exporter
                sht.Range("A2").Value = "Exporter";
                sht.Range("A2:F2").Style.Font.FontSize = 12;
                sht.Range("A2:F2").Style.Font.FontName = "Tahoma";
                sht.Range("A2:F2").Style.NumberFormat.Format = "@";
                sht.Range("A2:F2").Merge();

                //CompanyName
                sht.Range("A3").Value = ds.Tables[0].Rows[0]["companyName"];
                sht.Range("A3:F3").Style.Font.FontSize = 12;
                sht.Range("A3:F3").Style.Font.FontName = "Tahoma";
                sht.Range("A3:F3").Style.NumberFormat.Format = "@";
                sht.Range("A3:F3").Style.Font.Bold = true;
                sht.Range("A3:F3").Merge();
                //Address
                sht.Range("A4").Value = ds.Tables[0].Rows[0]["compaddr1"];
                sht.Range("A4:F4").Style.Font.FontSize = 12;
                sht.Range("A4:F4").Style.Font.FontName = "Tahoma";
                sht.Range("A4:F4").Style.NumberFormat.Format = "@";
                sht.Range("A4:F4").Merge();
                //address2
                sht.Range("A5").Value = ds.Tables[0].Rows[0]["compaddr2"] + "," + ds.Tables[0].Rows[0]["compaddr3"];
                sht.Range("A5:F5").Style.Font.FontSize = 12;
                sht.Range("A5:F5").Style.Font.FontName = "Tahoma";
                sht.Range("A5:F5").Style.NumberFormat.Format = "@";
                sht.Range("A5:F5").Merge();
                //TiN No
                sht.Range("A6").Value = "GSTIN#" + ds.Tables[0].Rows[0]["gstno"];
                sht.Range("A6:F6").Style.Font.FontSize = 12;
                sht.Range("A6:F6").Style.Font.FontName = "Tahoma";
                sht.Range("A6:F6").Style.NumberFormat.Format = "@";
                sht.Range("A6:F6").Style.Font.Bold = true;
                sht.Range("A6:F6").Merge();
                //**********INvoiceNodate
                sht.Range("G2").Value = "Invoice No./Date";
                sht.Range("G2:K2").Style.Font.FontSize = 12;
                sht.Range("G2:K2").Style.Font.FontName = "Tahoma";
                sht.Range("G2:K2").Style.NumberFormat.Format = "@";

                sht.Range("G2:K2").Merge();
                //value
                sht.Range("G3").Value = ds.Tables[0].Rows[0]["TInvoiceNo"] + " Dated " + ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("G3:K3").Style.Font.FontSize = 12;
                sht.Range("G3:K3").Style.Font.FontName = "Tahoma";
                sht.Range("G3:K3").Style.NumberFormat.Format = "@";
                sht.Range("G3:K3").Merge();
                sht.Range("G3", "K3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //*************IE Code
                sht.Range("G4").Value = "IE Code No.";
                sht.Range("G4:I4").Style.Font.FontSize = 12;
                sht.Range("G4:I4").Style.Font.FontName = "Tahoma";
                sht.Range("G4:I4").Style.NumberFormat.Format = "@";
                sht.Range("G4:I4").Merge();
                //value
                sht.Range("G5").Value = ds.Tables[0].Rows[0]["IEcode"];
                sht.Range("G5:I5").Style.Font.FontSize = 12;
                sht.Range("G5:I5").Style.Font.FontName = "Tahoma";
                sht.Range("G5:I5").Style.NumberFormat.Format = "@";
                sht.Range("G5:I5").Merge();
                sht.Range("G5", "I5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //***********GRI Form No
                sht.Range("J4").Value = "GRI Form No.";
                sht.Range("J4:K4").Style.Font.FontSize = 12;
                sht.Range("J4:K4").Style.Font.FontName = "Tahoma";
                sht.Range("J4:K4").Style.NumberFormat.Format = "@";
                sht.Range("J4:K4").Merge();
                // value
                sht.Range("J5").Value = "";
                sht.Range("J5:K5").Style.Font.FontSize = 12;
                sht.Range("J5:K5").Style.Font.FontName = "Tahoma";
                sht.Range("J5:K5").Style.NumberFormat.Format = "@";
                sht.Range("J5:K5").Merge();
                sht.Range("J5", "K5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //*************Buyer's Order No and Date
                sht.Range("G6").Value = "Buyer's Order No. / Date";
                sht.Range("G6:K6").Style.Font.FontSize = 12;
                sht.Range("G6:K6").Style.Font.FontName = "Tahoma";
                sht.Range("G6:K6").Style.NumberFormat.Format = "@";
                sht.Range("G6:K6").Merge();
                //value
                sht.Range("G7").SetValue(ds.Tables[0].Rows[0]["TorderNo"]);
                sht.Range("G7:K7").Style.Font.FontSize = 12;
                sht.Range("G7:K7").Style.Font.FontName = "Tahoma";
                sht.Range("G7:K7").Style.NumberFormat.Format = "@";
                sht.Range("G7:K7").Merge();
                sht.Range("G7", "K7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //**************Other Reference(s)
                sht.Range("G8").Value = "Other Reference(s)";
                sht.Range("G9").Value = "CONSIGNMENT NO.:" + ds.Tables[0].Rows[0]["otherref"];
                sht.Range("G10").Value = "SUPPLIER NO.:";
                sht.Range("G8:G10").Style.Font.FontSize = 12;
                sht.Range("G8:G10").Style.Font.FontName = "Tahoma";
                sht.Range("G8:G10").Style.NumberFormat.Format = "@";
                sht.Range("G8:K8").Merge();
                sht.Range("G9:K9").Merge();
                sht.Range("G10:K10").Merge();
                //*************Consignee
                sht.Range("A11").Value = "Consignee";
                sht.Range("C11").Value = ds.Tables[0].Rows[0]["consignee_dt"];
                sht.Range("A11:B11").Style.Font.FontSize = 12;
                sht.Range("A11:B11").Style.Font.FontName = "Tahoma";
                sht.Range("A11:B11").Style.NumberFormat.Format = "@";


                sht.Range("C11:F11").Style.Font.FontSize = 12;
                sht.Range("C11:F11").Style.Font.FontName = "Tahoma";
                sht.Range("C11:F11").Style.NumberFormat.Format = "@";
                sht.Range("C11:F11").Style.Font.Bold = true;

                sht.Range("A11:B11").Merge();
                sht.Range("C11:F11").Merge();
                //value
                sht.Range("A12").Value = ds.Tables[0].Rows[0]["consignee"];
                sht.Range("A12:F12").Style.Font.FontSize = 12;
                sht.Range("A12:F12").Style.Font.FontName = "Tahoma";
                sht.Range("A12:F12").Style.NumberFormat.Format = "@";
                sht.Range("A12:F12").Style.Font.Bold = true;
                sht.Range("A12:F12").Merge();
                //**
                sht.Range("A13").Value = ds.Tables[0].Rows[0]["consignee_address"];
                sht.Range("A13:F16").Style.Font.FontSize = 12;
                sht.Range("A13:F16").Style.Font.FontName = "Tahoma";
                sht.Range("A13:F16").Style.NumberFormat.Format = "@";

                sht.Range("A13", "F16").Style.Alignment.WrapText = true;
                sht.Range("A13", "F16").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A13:F16").Merge();
                //***********Notify
                sht.Range("A17").Value = "Notify Party";
                sht.Range("C17").Value = ds.Tables[0].Rows[0]["Notifyparty_dt"];
                sht.Range("A17:B17").Style.Font.FontSize = 12;
                sht.Range("A17:B17").Style.Font.FontName = "Tahoma";
                sht.Range("A17:B17").Style.NumberFormat.Format = "@";

                sht.Range("A17:B17").Style.Font.Underline = XLFontUnderlineValues.Single;
                sht.Range("C17:F17").Style.Font.FontSize = 12;
                sht.Range("C17:F17").Style.Font.FontName = "Tahoma";
                sht.Range("C17:F17").Style.NumberFormat.Format = "@";
                sht.Range("C17:F17").Style.Font.Bold = true;

                sht.Range("A17:B17").Merge();
                sht.Range("C17:F17").Merge();
                //value
                sht.Range("A18").Value = ds.Tables[0].Rows[0]["Notifyparty"];
                sht.Range("A18:F18").Style.Font.FontSize = 12;
                sht.Range("A18:F18").Style.Font.FontName = "Tahoma";
                sht.Range("A18:F18").Style.NumberFormat.Format = "@";
                sht.Range("A18:F18").Style.Font.Bold = true;

                sht.Range("A18:F18").Merge();

                sht.Range("A19").Value = ds.Tables[0].Rows[0]["notifyparty_address"];
                sht.Range("A19:F23").Style.Font.FontSize = 12;
                sht.Range("A19:F23").Style.Font.FontName = "Tahoma";
                sht.Range("A19:F23").Style.NumberFormat.Format = "@";

                sht.Range("A19", "F23").Style.Alignment.WrapText = true;
                sht.Range("A19", "F23").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A19:F23").Merge();
                //***********Receiver address
                sht.Range("G11").Value = "Receiver Address";
                sht.Range("G11:I11").Style.Font.FontSize = 12;
                sht.Range("G11:I11").Style.Font.FontName = "Tahoma";
                sht.Range("G11:I11").Style.NumberFormat.Format = "@";
                sht.Range("G11:I11").Merge();
                //values
                sht.Range("J11").Value = ds.Tables[0].Rows[0]["Destcode"];
                sht.Range("J11:K11").Style.Font.FontSize = 12;
                sht.Range("J11:K11").Style.Font.FontName = "Tahoma";
                sht.Range("J11:K11").Style.NumberFormat.Format = "@";
                sht.Range("J11:K11").Style.Font.Bold = true;

                sht.Range("J11:K11").Merge();
                //****** 1.
                sht.Range("G12").Value = "1. " + ds.Tables[0].Rows[0]["Receiver"];
                sht.Range("G12:K12").Style.Font.FontSize = 12;
                sht.Range("G12:K12").Style.Font.FontName = "Tahoma";
                sht.Range("G12:K12").Style.NumberFormat.Format = "@";
                sht.Range("G12:K12").Style.Font.Bold = true;
                sht.Range("G12:K12").Merge();
                //*
                sht.Range("G13").Value = ds.Tables[0].Rows[0]["Receiver_address"];
                sht.Range("G13:K15").Style.Font.FontSize = 12;
                sht.Range("G13:K15").Style.Font.FontName = "Tahoma";
                sht.Range("G13:K15").Style.NumberFormat.Format = "@";

                sht.Range("G13:K15").Style.Alignment.WrapText = true;
                sht.Range("G13:K15").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("G13:K15").Merge();
                //****** 2.
                sht.Range("G16").Value = "2.Paying Agent : " + ds.Tables[0].Rows[0]["payingagent"];
                sht.Range("G16:K16").Style.Font.FontSize = 12;
                sht.Range("G16:K16").Style.Font.FontName = "Tahoma";
                sht.Range("G16:K16").Style.NumberFormat.Format = "@";
                sht.Range("G16:K16").Style.Font.Bold = true;

                sht.Range("G16:K16").Merge();
                //*
                sht.Range("I17").Value = ds.Tables[0].Rows[0]["payingagent_address"];
                sht.Range("I17:K19").Style.Font.FontSize = 12;
                sht.Range("I17:K19").Style.Font.FontName = "Tahoma";
                sht.Range("I17:K19").Style.NumberFormat.Format = "@";

                sht.Range("I17:K19").Style.Alignment.WrapText = true;
                sht.Range("I17:K19").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("I17:K19").Merge();
                //*******3.
                sht.Range("G20").Value = "Buyer (If other than Consignee)";
                sht.Range("G20:K20").Style.Font.FontSize = 12;
                sht.Range("G20:K20").Style.Font.FontName = "Tahoma";
                sht.Range("G20:K20").Style.NumberFormat.Format = "@";
                sht.Range("G20:K20").Merge();

                sht.Range("G21").Value = "3." + ds.Tables[0].Rows[0]["buyer_otherthanconsignee"];
                sht.Range("G21:K21").Style.Font.FontSize = 12;
                sht.Range("G21:K21").Style.Font.FontName = "Tahoma";
                sht.Range("G21:K21").Style.NumberFormat.Format = "@";
                sht.Range("G21:K21").Style.Font.Bold = true;

                sht.Range("G21:K21").Merge();
                //*
                sht.Range("G22").Value = ds.Tables[0].Rows[0]["otherthanconsignee_address"];
                sht.Range("G22:K27").Style.Font.FontSize = 12;
                sht.Range("G22:K27").Style.Font.FontName = "Tahoma";
                sht.Range("G22:K27").Style.NumberFormat.Format = "@";
                sht.Range("G22:K27").Style.Alignment.WrapText = true;
                sht.Range("G22:K27").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("G22:K27").Merge();
                //***********Pre-carriage By
                sht.Range("A24").Value = "Pre-Carriage By";
                sht.Range("A24:C24").Style.Font.FontSize = 12;
                sht.Range("A24:C24").Style.Font.FontName = "Tahoma";
                sht.Range("A24:C24").Style.NumberFormat.Format = "@";
                sht.Range("A24:C24").Merge();
                //value
                sht.Range("A25").Value = ds.Tables[0].Rows[0]["Pre_carriageby"];
                sht.Range("A25:C25").Style.Font.FontSize = 12;
                sht.Range("A25:C25").Style.Font.FontName = "Tahoma";
                sht.Range("A25:C25").Style.NumberFormat.Format = "@";
                sht.Range("A25:C25").Merge();
                sht.Range("A25:C25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //************Place of Receipt by Pre-Carrier
                sht.Range("D24").Value = "Place of Receipt by Pre-Carrier";
                sht.Range("D24:F24").Style.Font.FontSize = 12;
                sht.Range("D24:F24").Style.Font.FontName = "Tahoma";
                sht.Range("D24:F24").Style.NumberFormat.Format = "@";
                sht.Range("D24:F24").Merge();
                //value
                sht.Range("D25").Value = ds.Tables[0].Rows[0]["receiptby"];
                sht.Range("D25:F25").Style.Font.FontSize = 12;
                sht.Range("D25:F25").Style.Font.FontName = "Tahoma";
                sht.Range("D25:F25").Style.NumberFormat.Format = "@";
                sht.Range("D25:F25").Merge();
                sht.Range("D25:F25").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Vessel/Flight No
                sht.Range("A26").Value = "Vessel/Flight No";
                sht.Range("A26:C26").Style.Font.FontSize = 12;
                sht.Range("A26:C26").Style.Font.FontName = "Tahoma";
                sht.Range("A26:C26").Style.NumberFormat.Format = "@";
                sht.Range("A26:C26").Merge();
                //value
                sht.Range("A27").Value = ds.Tables[0].Rows[0]["vessel_flightno"];
                sht.Range("A27:C27").Style.Font.FontSize = 12;
                sht.Range("A27:C27").Style.Font.FontName = "Tahoma";
                sht.Range("A27:C27").Style.NumberFormat.Format = "@";
                sht.Range("A27:C27").Merge();
                sht.Range("A27:C27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Loading
                sht.Range("D26").Value = "Port of Loading";
                sht.Range("D26:F26").Style.Font.FontSize = 12;
                sht.Range("D26:F26").Style.Font.FontName = "Tahoma";
                sht.Range("D26:F26").Style.NumberFormat.Format = "@";
                sht.Range("D26:F26").Merge();
                //value
                sht.Range("D27").Value = ds.Tables[0].Rows[0]["portload"];
                sht.Range("D27:F27").Style.Font.FontSize = 12;
                sht.Range("D27:F27").Style.Font.FontName = "Tahoma";
                sht.Range("D27:F27").Style.NumberFormat.Format = "@";
                sht.Range("D27:F27").Merge();
                sht.Range("D27:F27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Port of Discharge
                sht.Range("A28").Value = "Port of Discharge";
                sht.Range("A28:C28").Style.Font.FontSize = 12;
                sht.Range("A28:C28").Style.Font.FontName = "Tahoma";
                sht.Range("A28:C28").Style.NumberFormat.Format = "@";
                sht.Range("A28:C28").Merge();
                //value
                sht.Range("A29").Value = ds.Tables[0].Rows[0]["portunload"];
                sht.Range("A29:C29").Style.Font.FontSize = 12;
                sht.Range("A29:C29").Style.Font.FontName = "Tahoma";
                sht.Range("A29:C29").Style.NumberFormat.Format = "@";
                sht.Range("A29:C29").Merge();
                sht.Range("A29:C29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Final Destination
                sht.Range("D28").Value = "Final Destination";
                sht.Range("D28:F28").Style.Font.FontSize = 12;
                sht.Range("D28:F28").Style.Font.FontName = "Tahoma";
                sht.Range("D28:F28").Style.NumberFormat.Format = "@";
                sht.Range("D28:F28").Merge();
                //value
                sht.Range("D29").Value = ds.Tables[0].Rows[0]["Destinationadd"];
                sht.Range("D29:F29").Style.Font.FontSize = 12;
                sht.Range("D29:F29").Style.Font.FontName = "Tahoma";
                sht.Range("D29:F29").Style.NumberFormat.Format = "@";
                sht.Range("D29:F29").Merge();
                sht.Range("D29:F29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Origin of Goods
                sht.Range("G28").Value = "Country of Origin of Goods";
                sht.Range("G28:I28").Style.Font.FontSize = 12;
                sht.Range("G28:I28").Style.Font.FontName = "Tahoma";
                sht.Range("G28:I28").Style.NumberFormat.Format = "@";
                sht.Range("G28:I28").Merge();
                //value
                sht.Range("G29").Value = ds.Tables[0].Rows[0]["countryoforigin"];
                sht.Range("G29:I29").Style.Font.FontSize = 12;
                sht.Range("G29:I29").Style.Font.FontName = "Tahoma";
                sht.Range("G29:I29").Style.NumberFormat.Format = "@";
                sht.Range("G29:I29").Merge();
                sht.Range("G29:I29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //************Country of Final Destination
                sht.Range("J28").Value = "Country of Final Destination";
                sht.Range("J28:K28").Style.Font.FontSize = 12;
                sht.Range("J28:K28").Style.Font.FontName = "Tahoma";
                sht.Range("J28:K28").Style.NumberFormat.Format = "@";
                sht.Range("J28:K28").Merge();
                //value
                sht.Range("J29").Value = ds.Tables[0].Rows[0]["countryoffinaldest"];
                sht.Range("J29:K29").Style.Font.FontSize = 12;
                sht.Range("J29:K29").Style.Font.FontName = "Tahoma";
                sht.Range("J29:K29").Style.NumberFormat.Format = "@";
                sht.Range("J29:K29").Merge();
                sht.Range("J29:K29").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //*****************Nos
                sht.Range("A30").Value = "Nos.";
                sht.Range("B30").Value = "No and kind of Packages";
                sht.Range("B30:D30").Merge();

                sht.Range("E30").Value = "Description of Goods";
                sht.Range("E30:J30").Merge();
                //sht.Range("k30").Value = "Quantity";                
                sht.Range("A30:K30").Style.Font.FontSize = 12;
                sht.Range("A30:K30").Style.Font.FontName = "Tahoma";
                sht.Range("A30:K30").Style.NumberFormat.Format = "@";
                sht.Range("K30").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //values
                sht.Range("A31:H31").Merge();
                sht.Range("A32:H36").Style.Font.FontSize = 12;
                sht.Range("A32:H36").Style.Font.FontName = "Tahoma";
                sht.Range("A32:H36").Style.NumberFormat.Format = "@";
                sht.Range("A32").Value = ds.Tables[0].Compute("Min(MinrollNo)", "") + "/" + ds.Tables[0].Compute("Max(Maxrollno)", "");
                sht.Range("B32").Value = ds.Tables[0].Rows[0]["Noofrolls"] + " Packages";
                sht.Range("B32:C32").Merge();
                sht.Range("E32").Value = ds.Tables[0].Rows[0]["descriptionofgoods"];
                sht.Range("E32:J36").Style.Alignment.WrapText = true;
                sht.Range("E32:J36").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("E32:J36").Merge();
                sht.Range("A37:J37").Merge();
                //            //**********************Details
                //********Hader
                sht.Range("A38").Value = "Roll No";
                sht.Range("A38:K38").Style.Font.FontSize = 12;
                sht.Range("A38:K38").Style.Font.FontName = "Tahoma";
                sht.Range("A38:K38").Style.NumberFormat.Format = "@";
                sht.Range("A38:K38").Style.Font.Bold = true;
                sht.Range("A38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B38").Value = "Quality";
                sht.Range("B38:C38").Merge();
                sht.Range("D38").Value = "ART.NO.";
                //sht.Range("D38:E38")..Merge();
                sht.Range("E38").Value = "COLOR";
                sht.Range("F38").Value = "SIZE(CM)";
                sht.Range("F38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("G38").Value = "Pcs/Roll";
                sht.Range("G38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H38").Value = "Total Rolls";
                sht.Range("H38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("I38").Value = "Total Pcs";
                sht.Range("J38").Value = "Area Sq. Mtr";
                sht.Range("K38").Value = "P.O.#";
                sht.Range("K38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A38:K38").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A38").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K38").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A38:K38").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //***********generate Loop
                i = 39;

                int MinRollNew = 0;
                    int MaxRollNew=0;
                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                {

                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        MinRollNew = 1;
                        MaxRollNew = Convert.ToInt32(ds.Tables[0].Rows[ii]["totalroll"]);
                    }
                    else if (ds.Tables[0].Rows.Count > 1)
                    {
                        if (MinRollNew == 0 && MaxRollNew == 0)
                        {
                            MinRollNew = 1;
                            MaxRollNew = Convert.ToInt32(ds.Tables[0].Rows[ii]["totalroll"]);
                        }
                        else
                        {
                            MinRollNew =MaxRollNew+1;
                            MaxRollNew = MinRollNew+Convert.ToInt32(ds.Tables[0].Rows[ii]["totalroll"])-1;
                        }
                    }
                   

                    sht.Range("A" + i, "K" + i).Style.Font.FontSize = 12;
                    sht.Range("A" + i, "K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("A" + i, "I" + i).Style.NumberFormat.Format = "@";
                    sht.Range("K" + i).Style.Font.FontSize = 12;
                    sht.Range("K" + i).Style.Font.FontName = "Tahoma";
                    sht.Range("K" + i).Style.NumberFormat.Format = "@";
                    ////pono
                    //sht.Range("A" + i).Value = ds.Tables[0].Rows[ii]["Minrollno"] + "  " + ds.Tables[0].Rows[ii]["Maxrollno"];

                    sht.Range("A" + i).Value = MinRollNew + "  " + MaxRollNew;
                    sht.Range("A" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //Article Name
                    sht.Range("B" + i).Value = ds.Tables[0].Rows[ii]["Design"];
                    sht.Range("B" + i, "C" + i).Merge();
                    //Art No.
                    sht.Range("D" + i).Value = ds.Tables[0].Rows[ii]["articleno"];
                    //Colour
                    sht.Range("E" + i).Value = ds.Tables[0].Rows[ii]["Color"];
                    //Size
                    sht.Range("F" + i).Value = ds.Tables[0].Rows[ii]["width"] + "x" + ds.Tables[0].Rows[ii]["Length"];
                    sht.Range("F" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //pcs/roll
                    sht.Range("G" + i).SetValue(ds.Tables[0].Rows[ii]["pcsperroll"]);
                    sht.Range("G" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("G" + i).Style.NumberFormat.Format = "@";
                    //Total rolls
                    sht.Range("H" + i).SetValue(ds.Tables[0].Rows[ii]["totalroll"]);
                    sht.Range("H" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("H" + i).Style.NumberFormat.Format = "@";
                    TotalRolls = TotalRolls + Convert.ToInt16(ds.Tables[0].Rows[ii]["totalroll"]);
                    //Qty
                    sht.Range("I" + i).SetValue(ds.Tables[0].Rows[ii]["Pcs"]);
                    sht.Range("I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    Pcs = Pcs + Convert.ToInt16(ds.Tables[0].Rows[ii]["Pcs"]);
                    sht.Range("I" + i).Style.NumberFormat.Format = "@";
                    //Area
                    sht.Range("J" + i).SetValue(ds.Tables[0].Rows[ii]["Area"]);
                    sht.Range("J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                    Area = Decimal.Parse(Area.ToString()) + Decimal.Parse(ds.Tables[0].Rows[ii]["Area"].ToString());
                    //
                    //var _with27 = sht.Range("I" + i);

                    //_with27.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
                    //_with27.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
                    //PO
                    sht.Range("K" + i).Value = ds.Tables[0].Rows[ii]["Pono"];
                    //
                    sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //Borders
                    sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    i = i + 1;
                }
                //
                sht.Range("A" + i + ":A" + (i + 7)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 7)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //End Of loop
                i = i + 7;
                //*************** 

                //
                sht.Range("D" + i).Value = "Packing List Total:";
                sht.Range("D" + i, "F" + i).Style.Font.FontSize = 12;
                sht.Range("D" + i, "F" + i).Style.Font.FontName = "Tahoma";
                sht.Range("D" + i, "F" + i).Style.NumberFormat.Format = "@";
                sht.Range("D" + i, "F" + i).Style.Font.Bold = true;
                sht.Range("D" + i + ":F" + i).Merge();
                sht.Range("D" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //Total Rolls
                sht.Range("H" + i).Value = TotalRolls;
                sht.Range("H" + i).Style.Font.FontSize = 12;
                sht.Range("H" + i).Style.Font.FontName = "Tahoma";
                sht.Range("H" + i).Style.NumberFormat.Format = "@";
                sht.Range("H" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //Total Pcs
                sht.Range("I" + i).Value = Pcs;
                sht.Range("I" + i).Style.Font.FontSize = 12;
                sht.Range("I" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i).Style.NumberFormat.Format = "@";
                sht.Range("I" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //Area
                sht.Range("J" + i).Value = Area;
                sht.Range("J" + i).Style.Font.FontSize = 12;
                sht.Range("J" + i).Style.Font.FontName = "Tahoma";
                sht.Range("J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("J" + i).Style.NumberFormat.Format = "#,###0.00";
                //
                var _with21 = sht.Range("A" + i + ":K" + i);
                _with21.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                _with21.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //
                sht.Range("A" + i + ":A" + (i + 10)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 10)).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //**********Total
                i = i + 10;
                //i = 63;
                sht.Range("A" + i).Value = "TOTAL ROLLS";
                sht.Range("A" + (i + 1)).Value = "TOTAL PCS";
                sht.Range("A" + (i + 2)).Value = "TOTAL GR. WT";
                sht.Range("A" + (i + 3)).Value = "TOTAL NT. WT";
                sht.Range("A" + (i + 4)).Value = "TOTAL SQ.MTR";
                sht.Range("A" + (i + 5)).Value = "TOTAL VOLUME";
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":A" + (i + 5)).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":A" + (i + 5)).Style.Font.FontName = "Courier New";
                //value                
                sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontSize = 12;
                sht.Range("C" + i + ":C" + (i + 5)).Style.Font.FontName = "Tahoma";
                sht.Range("C" + i + ":C" + (i + 5)).Style.NumberFormat.Format = "@";


                sht.Range("C" + i).SetValue(ds.Tables[0].Rows[0]["Noofrolls"]);
                sht.Range("C" + (i + 1)).SetValue(Pcs);
                sht.Range("C" + (i + 2)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["grosswt"]) + " Kgs.");
                sht.Range("C" + (i + 3)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["netwt"]) + " Kgs.");
                sht.Range("C" + (i + 4)).SetValue(String.Format("{0:#,0.00}", Area) + " Sq.Mtr");
                sht.Range("C" + (i + 5)).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Rows[0]["Volume"]) + " CBM");
                //
                sht.Range("A" + i + ":A" + (i + 5)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 5)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //***********Sig and date
                i = i + 6;
                //i = 70;
                sht.Range("A" + i + ":H" + i).Merge();
                sht.Range("I" + i).Value = "Signature/Date";
                sht.Range("I" + i).Style.Font.FontSize = 12;
                sht.Range("I" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i).Style.NumberFormat.Format = "@";

                sht.Range("K" + i).Value = "For " + ds.Tables[0].Rows[0]["companyname"];
                sht.Range("J" + i + ":K" + i).Style.Font.FontSize = 12;
                sht.Range("J" + i + ":K" + i).Style.Font.FontName = "Tahoma";
                sht.Range("J" + i + ":K" + i).Style.NumberFormat.Format = "@";
                sht.Range("J" + i + ":K" + i).Style.Font.Bold = true;
                sht.Range("J" + i + ":K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("J" + i + ":K" + i).Style.Font.FontSize = 9;
                //sht.Range("J" + i + ":K" + i).Merge();              
                sht.Range("I" + i + ":K" + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("I" + i + ":I" + (i + 3)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i + ":K" + (i + 3)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //**************Declaration
                //
                sht.Range("A" + i + ":A" + (i + 2)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                i = i + 1;
                //i = 71;
                sht.Range("A" + i).Value = "Declaration";
                sht.Range("A" + i + ":B" + i).Merge();
                sht.Range("A" + i + ":B" + i).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":B" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":B" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i + ":B" + i).Style.Font.Bold = true;
                sht.Range("A" + i + ":B" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                //value
                i = i + 1;
                sht.Range("A" + i).Value = "We declare that this invoice shows the actual price of the";
                sht.Range("A" + i + ":F" + i).Merge();
                sht.Range("A" + i + ":F" + i).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":F" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":F" + i).Style.NumberFormat.Format = "@";
                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                i = i + 1;
                sht.Range("A" + i).Value = "goods described and that all particulars are true and correct";
                sht.Range("A" + i + ":F" + i).Merge();
                sht.Range("A" + i + ":F" + i).Style.Font.FontSize = 12;
                sht.Range("A" + i + ":F" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":F" + i).Style.NumberFormat.Format = "@";

                sht.Range("A" + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K" + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //**********

                sht.Range("I" + i + ":J" + i).Merge();
                sht.Range("I" + i + ":J" + i).Value = ds.Tables[0].Rows[0]["InvoiceDate"];
                sht.Range("I" + i + ":J" + i).Style.Font.FontName = "Tahoma";
                sht.Range("I" + i + ":J" + i).Style.Font.FontSize = 12;
                sht.Range("I" + i + ":J" + i).Style.NumberFormat.Format = "dd/MMM/yyyy";
                sht.Range("I" + i + ":J" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Range("K" + i).Value = "Auth Sign";
                sht.Range("K" + i).Style.Font.FontSize = 12;
                sht.Range("K" + i).Style.Font.FontName = "Tahoma";
                sht.Range("K" + i).Style.NumberFormat.Format = "@";
                sht.Range("K" + i).Style.Font.Bold = true;
                sht.Range("K" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //
                var _with39 = sht.Range("I" + i);
                _with39.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //
                sht.Range("A2:A10").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A11:A23").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + i + ":K" + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A1:K1").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //sht.Range("A2:A10").Style.Border.LeftBorder = XLBorderStyleValues.Thin;                
                //sht.Range("F2:F10").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //            //************************
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
                sht.Range("G28:K28").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A30:A37").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("K30:K37").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //*******************Save
                string Fileextension = "xlsx";
                string filename = "PackingList -" + validateFilename(ds.Tables[0].Rows[0]["TinvoiceNo"].ToString()) + "." + Fileextension + "";
                Path = Server.MapPath("~/Packingexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
                //*****
                lblmsg.Text = "Packing Excel Format downloaded successfully.";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "AltR2", "alert('No Record Found to Export')", true);
            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void vdfexcelEastern(int Invoiceid)
    {
        string str = "";       
            str = @"select *,VD.PcsRoll as Pcsperroll,VD.Pcs/VD.PcsRoll as TotalRoll 
                        From V_InvoiceDetail VI inner join V_PackingDetailsIkea VD on VI.Invoiceid=VD.PackingId 
                        Where VI.invoiceid=" + Invoiceid + " order by MinrollNo";      
        

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {

            XLWorkbook xapp = new XLWorkbook(Server.MapPath("~/VDFExcel/VDFExcel.xlsx"));
            IXLWorksheet sht = xapp.Worksheet(1);
            sht.Range("A5:M8").Style.Font.FontName = "Arial";
            sht.Range("A5:M8").Style.Font.FontSize = 10;
            sht.Range("A5:M8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            var a = sht.Cell(5, 1);
            a.RichText.AddText("EXPORTER NAME: ").SetFontName("Arial Black");
            a.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["companyname"])).SetFontName("Cambria");

            var b = sht.Cell(6, 1);
            b.RichText.AddText("GSTIN.# ").SetFontName("Arial Black");
            b.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["GSTNO"])).SetFontName("Cambria");

            var c = sht.Cell(7, 1);
            c.RichText.AddText("SHIPMENT ID: ").SetFontName("Arial Black");
            c.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["shipmentid"])).SetFontName("Cambria");

            var D = sht.Cell(8, 1);
            D.RichText.AddText("SUPPLIER No. ").SetFontName("Arial Black");
            D.RichText.AddText("21899").SetFontName("Cambria");

            var E = sht.Cell(5, 5);
            E.RichText.AddText("INVOICE No. ").SetFontName("Arial Black");
            E.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["TinvoiceNo"])).SetFontName("Cambria");

            var F = sht.Cell(6, 5);
            F.RichText.AddText("DATE : ").SetFontName("Arial Black");
            F.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["invoicedate"])).SetFontName("Cambria");

            var G = sht.Cell(7, 5);
            G.RichText.AddText("APLL CSGN NO ").SetFontName("Arial Black");
            G.RichText.AddText("ECIS NO " + Convert.ToString(ds.Tables[0].Rows[0]["Ecisno"])).SetFontName("Cambria");

            var H = sht.Cell(5, 10);
            H.RichText.AddText("CBM DECLARED : ").SetFontName("Arial Black");
            H.RichText.AddText(string.Format(Convert.ToString(ds.Tables[0].Rows[0]["Volume"]), "#,##0.000") + " CBM").SetFontName("Arial Black");

            var I = sht.Cell(6, 10);
            I.RichText.AddText("NET WEIGHT : ").SetFontName("Arial Black");
            I.RichText.AddText(string.Format(Convert.ToString(ds.Tables[0].Rows[0]["Netwt"]), "#,##0.000") + " KG").SetFontName("Arial Black");

            var J = sht.Cell(7, 10);
            J.RichText.AddText("GROSS WEIGHT : ").SetFontName("Arial Black");
            J.RichText.AddText(string.Format(Convert.ToString(ds.Tables[0].Rows[0]["grosswt"]), "#,##0.000") + " KG").SetFontName("Arial Black");
            sht.Range("A9:M11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            var K = sht.Cell(9, 1);
            K.RichText.AddText("DESTINATION CODE: ").SetFontName("Arial Black");
            K.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["destcode"])).SetFontName("Cambria");

            var L = sht.Cell(10, 1);
            L.RichText.AddText("DELIVERY WEEK: ").SetFontName("Arial Black");
            L.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["Delvwk"])).SetFontName("Arial Black");

            var M = sht.Cell(11, 1);
            M.RichText.AddText("SEAL NO : ").SetFontName("Cambria");
            M.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["sealno"])).SetFontName("Cambria");

            var N = sht.Cell(9, 5);
            N.RichText.AddText("TRUCK No. ").SetFontName("Arial Black");
            N.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["truckno"])).SetFontName("Arial Black");

            var O = sht.Cell(11, 5);
            O.RichText.AddText("DESP. DATE ").SetFontName("Arial Black");
            O.RichText.AddText(Convert.ToString(ds.Tables[0].Rows[0]["Dispatchdate"])).SetFontName("Arial Black");
            //**************Details

            int row = 17;
            int MinRollNew = 0;
            int MaxRollNew = 0;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {              

                if (ds.Tables[0].Rows.Count == 1)
                {
                    MinRollNew = 1;
                    MaxRollNew = Convert.ToInt32(ds.Tables[0].Rows[i]["totalroll"]);
                }
                else if (ds.Tables[0].Rows.Count > 1)
                {
                    if (MinRollNew == 0 && MaxRollNew == 0)
                    {
                        MinRollNew = 1;
                        MaxRollNew = Convert.ToInt32(ds.Tables[0].Rows[i]["totalroll"]);
                    }
                    else
                    {
                        MinRollNew = MaxRollNew + 1;
                        MaxRollNew = MinRollNew + Convert.ToInt32(ds.Tables[0].Rows[i]["totalroll"]) - 1;
                    }
                }

                sht.Range("A" + row + ":M" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":M" + row).Style.Font.FontSize = 10;
                sht.Range("A" + row + ":M" + row).Style.Font.Bold = true;
                sht.Range("A" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                using (var ar = sht.Range("A" + row + ":M" + row))
                {
                    ar.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                    ar.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                    ar.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                    ar.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                }

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["PONo"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Articleno"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Pcs"] + " Pcs");
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Totalroll"]);
                sht.Range("E" + row).SetValue("PLT");
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["pcsperroll"]);
                //sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["MinrollNo"] + "-" + ds.Tables[0].Rows[i]["Maxrollno"]);
                sht.Range("G" + row).SetValue(MinRollNew + "-" + MaxRollNew);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Pcs"] + " Pcs");
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Totalroll"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["pcsperroll"]);
                sht.Range("L" + row).SetValue("S2");

                row = row + 1;
            }
            //************           

            for (int i = 0; i <= 2; i++)
            {
                sht.Range("A" + row + ":M" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":M" + row).Style.Font.FontSize = 10;
                sht.Range("A" + row + ":M" + row).Style.Font.Bold = true;
                sht.Range("A" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                using (var ar = sht.Range("A" + row + ":M" + row))
                {
                    ar.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                    ar.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                    ar.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                    ar.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                }
                i = i + 1;
                row = row + 1;
            }
            //*******************
            row = row + 1;
            sht.Range("A" + row + ":C" + row).Merge();
            sht.Range("F" + row + ":J" + row).Merge();
            sht.Range("A" + row + ":M" + row).Style.Font.FontName = "Arial Black";
            sht.Range("A" + row + ":M" + row).Style.Font.FontSize = 10;
            sht.Range("A" + row + ":M" + row).Style.Font.Bold = true;
            sht.Range("A" + row).SetValue("VENDOR’S SIGNATURE & STAMP");
            sht.Range("F" + row).SetValue("APLL ACKNOWLEDGEMENT at W/H");
            //**********
            row = row + 1;
            sht.Range("G" + row + ":J" + row).Merge();
            sht.Range("G" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            var B1 = sht.Cell(row, 7);
            B1.RichText.AddText("· ").SetFontName("Symbol").SetFontSize(10).SetBold();
            B1.RichText.AddText("Received By :").SetFontName("Arial").SetFontSize(10).SetBold();
            //*******
            row = row + 1;
            sht.Range("G" + row + ":J" + row).Merge();
            sht.Range("G" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            var B2 = sht.Cell(row, 7);
            B2.RichText.AddText("· ").SetFontName("Symbol").SetFontSize(10).SetBold();
            B2.RichText.AddText("Condition of Cargo :").SetFontName("Arial").SetFontSize(10).SetBold();
            //**********
            row = row + 3;
            var a2 = sht.Cell(row, 1);
            sht.Range("A" + row).Style.Font.FontSize = 10;
            a2.RichText.AddText("Please").SetFontName("Arial Black");
            a2.RichText.AddText(" Ö").SetFontName("Symbol").SetBold();

            //**************
            row = row + 1;
            sht.Range("A" + row + ":A" + (row + 4)).Style.Font.FontName = "Arial";
            sht.Range("A" + row + ":A" + (row + 4)).Style.Font.FontSize = 7.5;
            sht.Range("A" + row + ":A" + (row + 4)).Style.Font.Bold = true;
            sht.Range("A" + row + ":A" + (row + 4)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            sht.Range("D" + row + ":J" + (row + 4)).Style.Font.FontName = "Arial";
            sht.Range("D" + row + ":J" + (row + 4)).Style.Font.FontSize = 7.5;
            sht.Range("D" + row + ":J" + (row + 4)).Style.Font.Bold = true;
            sht.Range("D" + row + ":J" + (row + 4)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            using (var range = sht.Range("A" + row + ":B" + (row + 4)))
            {
                range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
            }
            sht.Range("A" + row).SetValue("DEEC");
            sht.Range("D" + row + ":G" + row).Merge();
            sht.Range("H" + row + ":J" + row).Merge();
            sht.Range("D" + row).Value = "Truck Reporting Date n Time";

            using (var range = sht.Range("D" + row + ":J" + (row + 4)))
            {
                range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                range.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                range.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
            }
            row = row + 1;
            sht.Range("A" + row).SetValue("DEPB + EPCG");
            sht.Range("B" + row).Value = "Ö";
            sht.Range("B" + row).Style.Font.FontName = "Symbol";
            sht.Range("B" + row).Style.Font.SetFontColor(XLColor.Blue);
            sht.Range("B" + row).Style.Font.Bold = true;


            sht.Range("D" + row + ":G" + row).Merge();
            sht.Range("H" + row + ":J" + row).Merge();
            sht.Range("D" + row).Value = "Truck Gate-in Date n Time";

            row = row + 1;

            sht.Range("A" + row).SetValue("DRAWBACK");
            sht.Range("D" + row + ":G" + row).Merge();
            sht.Range("H" + row + ":J" + row).Merge();
            sht.Range("D" + row).Value = "Truck Offloading Start Time";

            row = row + 1;
            sht.Range("A" + row).SetValue("WHITE");
            sht.Range("D" + row + ":G" + row).Merge();
            sht.Range("H" + row + ":J" + row).Merge();
            sht.Range("D" + row).Value = "Truck Offloading End Time";

            row = row + 1;
            sht.Range("A" + row).SetValue("100% EOU");
            sht.Range("D" + row + ":G" + row).Merge();
            sht.Range("H" + row + ":J" + row).Merge();
            sht.Range("D" + row).Value = "Truck Gate-out Date n Time";
            //**********
            row = row + 2;
            sht.Range("A" + row + ":B" + row).Merge();
            sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial";
            sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 10;
            sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;
            sht.Range("A" + row).Value = "TERMS & CONDITIONS";
            //*******
            row = row + 1;
            //
            sht.Range("A" + row + ":M" + row).Merge();
            sht.Range("A" + row + ":M" + (row + 3)).Style.Font.FontName = "Times New Roman";
            sht.Range("A" + row + ":M" + (row + 3)).Style.Font.FontSize = 7;
            sht.Range("A" + row + ":M" + (row + 3)).Style.Font.Bold = false;
            sht.Range("A" + row).Value = "All transaction with APLL are subject to the terms and condition stipulated in the company’s cargo receipt (copies available on request from company) APLL may exclude or limit its liabilities and apply certain cases";
            //
            row = row + 1;
            sht.Range("A" + row + ":M" + row).Merge();
            sht.Range("A" + row).Value = "   1)    This Vendor Declaration form is applicable only if it is validated and signed by an authorized signatory of APLL warehouse or appointed warehouse operator in additions to that of shipper’s or it’s representative.";
            //
            row = row + 1;

            sht.Range("A" + row + ":M" + row).Merge();
            sht.Range("A" + row).Value = "   2)    This Vendor Declaration form should be completed and presented to APLL or it’s appointed warehouse operator for processing and validatio";
            //
            row = row + 1;
            sht.Range("A" + row + ":M" + row).Merge();
            sht.Range("A" + row).Value = "APLL reserves the right to verify and adjust shipper’s declared cargo measurement in accordance with the actual declare measurement";
            //Save
            string Path = "";
            string Fileextension = "xlsx";
            string filename = "Vdf-" + validateFilename(ds.Tables[0].Rows[0]["Tinvoiceno"] + "." + Fileextension + "");
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            Response.End();
            //*************
            lblmsg.Text = "VDF Excel Format downloaded successfully.";

        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
}