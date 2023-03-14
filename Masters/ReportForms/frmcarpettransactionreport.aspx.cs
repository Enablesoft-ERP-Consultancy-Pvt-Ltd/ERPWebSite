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

public partial class Masters_ReportForms_frmcarpettransactionreport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDcompany, "select CI.CompanyId,ci.CompanyName From Companyinfo CI inner join Company_Authentication CA on CI.CompanyId=CA.CompanyId where CA.UserId=" + Session["varuserid"] + "", false, "");
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        try
        {
            SqlCommand cmd = new SqlCommand("Pro_GetcarpetTransactionreport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            int ReportType = 0;
            if (chkExportForExcel.Checked == true)
            {
                ReportType = 1;
            }
            cmd.Parameters.AddWithValue("@Companyid", DDcompany.SelectedValue);
            cmd.Parameters.AddWithValue("@fromdate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
            cmd.Parameters.AddWithValue("@ReportType", ReportType);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/TempExcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/TempExcel/"));
                }
                //*********************
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("CarpetTransactionReport");
                string Fileextension = "xlsx";
                string filename = "";
                
                if (chkExportForExcel.Checked == true)
                {                    
                    sht.Range("A1:L1").Merge();
                    sht.Range("A1:L1").Style.Font.FontSize = 11;
                    sht.Range("A1:L1").Style.Font.Bold = true;
                    sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A1:L1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A1").SetValue("CARPET TRANSACTION Excel  " + txtfromdate.Text + "---------" + txttodate.Text + "");
                    sht.Row(1).Height = 21.75;
                    sht.Range("A2:L2").Merge();
                    sht.Range("A2:L2").Style.Font.FontSize = 11;
                    sht.Range("A2:L2").Style.Font.Bold = true;
                    sht.Range("A2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A2:L2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A2").SetValue(DDcompany.SelectedItem.Text);
                    //*********
                    sht.Range("A3:L3").Style.Font.FontSize = 11;
                    sht.Range("A3:L3").Style.Font.Bold = true;
                    sht.Range("A3:L3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("B3:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Row(3).Height = 18.00;

                    sht.Range("A3").SetValue("CUSTOMER CODE");
                    sht.Range("B3").SetValue("PO NO");
                    sht.Range("C3").SetValue("ITEM NAME");
                    sht.Range("D3").SetValue("QUALITY NAME");
                    sht.Range("E3").SetValue("DESIGN NAME");
                    sht.Range("F3").SetValue("COLOR NAAME");
                    sht.Range("G3").SetValue("SIZE");
                    sht.Range("H3").SetValue("ORDER QTY");
                    sht.Range("I3").SetValue("STOCK QTY");
                    sht.Range("J3").SetValue("CUS_PO_RATE");
                    sht.Range("K3").SetValue("AREA");
                    sht.Range("L3").SetValue("TOTAL AMOUNT");
                    int Row = 4;
                    int rowcount = ds.Tables[0].Rows.Count;
                    for (int i = 0; i < rowcount; i++)
                    {
                        sht.Range("A" + Row + ":L" + Row).Style.Font.FontSize = 11;

                        sht.Range("A" + Row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                        sht.Range("B" + Row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                        sht.Range("C" + Row).SetValue(ds.Tables[0].Rows[i]["ItemName"]);
                        sht.Range("D" + Row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                        sht.Range("E" + Row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                        sht.Range("F" + Row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                        sht.Range("G" + Row).SetValue(ds.Tables[0].Rows[i]["SizeFt"]);
                        sht.Range("H" + Row).SetValue(ds.Tables[0].Rows[i]["OrderQty"]);
                        sht.Range("I" + Row).SetValue(ds.Tables[0].Rows[i]["Recpcs"]);
                        sht.Range("J" + Row).SetValue(ds.Tables[0].Rows[i]["UnitRate"]);
                        sht.Range("K" + Row).SetValue(ds.Tables[0].Rows[i]["RecArea"]);
                        sht.Range("L" + Row).SetValue(ds.Tables[0].Rows[i]["Amount"]);
                        Row = Row + 1;

                    }

                    var OrderQty = sht.Evaluate("SUM(H4:H" + (Row - 1) + ")");
                    var StockQty = sht.Evaluate("SUM(I4:I" + (Row - 1) + ")");
                    var Area = sht.Evaluate("SUM(K4:K" + (Row - 1) + ")");
                    var Amount = sht.Evaluate("SUM(L4:L" + (Row - 1) + ")");

                    sht.Columns(1, 10).AdjustToContents();
                    sht.Range("H" + Row).SetValue(OrderQty);
                    sht.Range("I" + Row).SetValue(StockQty);
                    sht.Range("K" + Row).SetValue(Area);
                    sht.Range("L" + Row).SetValue(Amount);

                    //******SAVE FILE

                    filename = UtilityModule.validateFilename("CarpetTransactionExcel-" + DateTime.Now + "." + Fileextension);
                }
                else
                {
                    //sht = xapp.Worksheets.Add("CarpetTransactionReport");
                    //****************
                    //*************
                    sht.Range("A1:K1").Merge();
                    sht.Range("A1:K1").Style.Font.FontSize = 11;
                    sht.Range("A1:K1").Style.Font.Bold = true;
                    sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A1:K1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A1").SetValue("CARPET TRANSACTION Between  " + txtfromdate.Text + "---------" + txttodate.Text + "");
                    sht.Row(1).Height = 21.75;
                    sht.Range("A2:K2").Merge();
                    sht.Range("A2:K2").Style.Font.FontSize = 11;
                    sht.Range("A2:K2").Style.Font.Bold = true;
                    sht.Range("A2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A2:K2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("A2").SetValue(DDcompany.SelectedItem.Text);
                    //*********
                    sht.Range("A3:K3").Style.Font.FontSize = 11;
                    sht.Range("A3:K3").Style.Font.Bold = true;
                    sht.Range("A3:K3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("B3:K3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Row(3).Height = 18.00;

                    sht.Range("A3").SetValue("QualityName");
                    sht.Range("B3").SetValue("Opn(Pcs)");
                    sht.Range("C3").SetValue("Opn(Area)yrd.");
                    sht.Range("D3").SetValue("Rec(Pcs)");
                    sht.Range("E3").SetValue("Rec(Area)yrd.");
                    sht.Range("F3").SetValue("Pur(Pcs)");
                    sht.Range("G3").SetValue("Pur(Area)yrd.");
                    sht.Range("H3").SetValue("Sold(Pcs)");
                    sht.Range("I3").SetValue("Sold(Area)yrd.");
                    sht.Range("J3").SetValue("Bal(Pcs)");
                    sht.Range("K3").SetValue("Bal(Area)yrd.");
                    int Row = 4;
                    int rowcount = ds.Tables[0].Rows.Count;
                    for (int i = 0; i < rowcount; i++)
                    {
                        sht.Range("A" + Row + ":K" + Row).Style.Font.FontSize = 11;

                        sht.Range("A" + Row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                        sht.Range("B" + Row).SetValue(ds.Tables[0].Rows[i]["Openpcs"]);
                        sht.Range("C" + Row).SetValue(ds.Tables[0].Rows[i]["Openarea"]);
                        sht.Range("D" + Row).SetValue(ds.Tables[0].Rows[i]["Recpcs"]);
                        sht.Range("E" + Row).SetValue(ds.Tables[0].Rows[i]["Recarea"]);
                        sht.Range("F" + Row).SetValue(ds.Tables[0].Rows[i]["Purpcs"]);
                        sht.Range("G" + Row).SetValue(ds.Tables[0].Rows[i]["Purarea"]);
                        sht.Range("H" + Row).SetValue(ds.Tables[0].Rows[i]["Soldpcs"]);
                        sht.Range("I" + Row).SetValue(ds.Tables[0].Rows[i]["Soldarea"]);
                        sht.Range("J" + Row).SetValue(ds.Tables[0].Rows[i]["Balpcs"]);
                        sht.Range("K" + Row).SetValue(ds.Tables[0].Rows[i]["Balarea"]);
                        Row = Row + 1;

                    }
                    //**********Total
                    var Openpcs = sht.Evaluate("SUM(B4:B" + (Row - 1) + ")");
                    var Openarea = sht.Evaluate("SUM(C4:C" + (Row - 1) + ")");
                    var Recpcs = sht.Evaluate("SUM(D4:D" + (Row - 1) + ")");
                    var Recarea = sht.Evaluate("SUM(E4:E" + (Row - 1) + ")");
                    var Purpcs = sht.Evaluate("SUM(F4:F" + (Row - 1) + ")");
                    var Purarea = sht.Evaluate("SUM(G4:G" + (Row - 1) + ")");
                    var Soldpcs = sht.Evaluate("SUM(H4:H" + (Row - 1) + ")");
                    var Soldarea = sht.Evaluate("SUM(I4:I" + (Row - 1) + ")");
                    var Balpcs = sht.Evaluate("SUM(J4:J" + (Row - 1) + ")");
                    var Balarea = sht.Evaluate("SUM(K4:K" + (Row - 1) + ")");
                    //*************
                    sht.Columns(1, 10).AdjustToContents();
                    sht.Range("B" + Row).SetValue(Openpcs);
                    sht.Range("C" + Row).SetValue(Openarea);
                    sht.Range("D" + Row).SetValue(Recpcs);
                    sht.Range("E" + Row).SetValue(Recarea);
                    sht.Range("F" + Row).SetValue(Purpcs);
                    sht.Range("G" + Row).SetValue(Purarea);
                    sht.Range("H" + Row).SetValue(Soldpcs);
                    sht.Range("I" + Row).SetValue(Soldarea);
                    sht.Range("J" + Row).SetValue(Balpcs);
                    sht.Range("K" + Row).SetValue(Balarea);
                    //**************Save
                    //******SAVE FILE
                    filename = UtilityModule.validateFilename("CarpetTransaction-" + DateTime.Now + "." + Fileextension);
                    
                }
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "al", "alert('No records found')", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "al", "alert(' " + ex.Message + "')", true);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
}
