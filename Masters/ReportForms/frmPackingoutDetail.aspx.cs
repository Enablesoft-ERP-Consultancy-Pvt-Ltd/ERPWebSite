using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using ClosedXML.Excel;


public partial class Masters_ReportForms_frmPackingoutDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");            

            switch (Session["varcompanyNo"].ToString())
            {
                case "38":
                    ChkForExcelReport.Visible = true;
                    break;
                default:
                    ChkForExcelReport.Visible = false;
                    break;
            }
        }

    }
    protected void PackingOutExcelReport(DataSet ds)
    {
        try
        {
//           string str = @"select PM.TInvoiceNo,vf.QualityName,vf.designName,vf.ColorName,PD.Width+'x'+PD.Length as Size,
//                        sum(Pd.pcs) as pcs,Sum(PD.area) as Area,'" + txtfromdate.Text + "' as Fromdate,'" + txttodate.Text + @"' as Todate
//                        From Packing PM(NoLock) inner join PackingInformation PD(NoLock) on PM.PackingId=PD.PackingId
//                        inner join V_FinishedItemDetail vf(NoLock) on PD.FinishedId=vf.ITEM_FINISHED_ID
//                        Where PM.ConsignorId = " + Session["CurrentWorkingCompanyID"] + " And PM.PackingDate>='" + txtfromdate.Text + "' and PM.PackingDate<='" + txttodate.Text + @"'
//                        group by PM.TInvoiceNo,vf.QualityName,vf.designName,vf.ColorName,PD.Width,PD.Length";
           

//            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
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

                sht.Range("A1:G1").Merge();
                sht.Range("A1").SetValue("PACKING OUT SUMMARY");
                sht.Range("A2:G2").Merge();
                sht.Range("A2").SetValue("FROM :" +txtfromdate.Text+ "TO :"+ txttodate.Text);
                sht.Range("A1:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:G2").Style.Font.SetBold();

                //Headers
                sht.Range("A3").Value = "INVOICE NO.";
                sht.Range("B3").Value = "QUALITY";
                sht.Range("C3").Value = "DESIGN";
                sht.Range("D3").Value = "COLOR";
                sht.Range("E3").Value = "SIZE";
                sht.Range("F3").Value = "PCS";
                sht.Range("G3").Value = "AREA";               

                sht.Range("A3:G3").Style.Font.Bold = true;

                row = 4;

                DataTable dtdistinctindent = ds.Tables[0].DefaultView.ToTable(true, "TInvoiceNo");
                DataView dvindetnNo = new DataView(dtdistinctindent);
                dvindetnNo.Sort = "TInvoiceNo asc";
                DataTable dtdistinct = dvindetnNo.ToTable();

                int rowfrom = 0, rowto = 0;
                string TPcsRow="",TAreaRow="";               
                foreach (DataRow dr in dtdistinct.Rows)
                {
                    DataView dvdetail = new DataView(ds.Tables[0]);
                    dvdetail.RowFilter = "TInvoiceNo='" + dr["TInvoiceNo"] + "' ";
                    dvdetail.Sort = "TInvoiceNo";
                    DataTable dt = dvdetail.ToTable();

                    rowfrom = row;                  
                    foreach (DataRow dr1 in dt.Rows)
                    {
                        sht.Range("A" + row).SetValue(dr1["TInvoiceNo"]);
                        sht.Range("B" + row).SetValue(dr1["QualityName"]);
                        sht.Range("C" + row).SetValue(dr1["DesignName"]);
                        sht.Range("D" + row).SetValue(dr1["ColorName"]);
                        sht.Range("E" + row).SetValue(dr1["Size"]);
                        sht.Range("F" + row).SetValue(dr1["Pcs"]);
                        sht.Range("G" + row).SetValue(dr1["Area"]); 

                        row = row + 1;
                    }

                    rowto = row - 1;
                    sht.Range("E" + row).SetValue("Total");
                    sht.Range("F" + row).FormulaA1 = "=SUM(F" + rowfrom + ":F" + rowto + ")";
                    sht.Range("G" + row).FormulaA1 = "=SUM(G" + rowfrom + ":G" + rowto + ")";                   
                    sht.Range("E" + row + ":G" + row).Style.Font.Bold = true;

                    TPcsRow = TPcsRow + "+" + "F" + row;
                    TAreaRow = TAreaRow + "+" + "G" + row;                   

                    row = row + 1;
                }

                TPcsRow = TPcsRow.TrimStart('+');
                TAreaRow = TAreaRow.TrimStart('+'); 


                sht.Range("E" + row).SetValue("G. Total");
                sht.Range("F" + row).FormulaA1 = "=SUM(" + TPcsRow + ")";
                sht.Range("G" + row).FormulaA1 = "=SUM(" + TAreaRow + ")";              

                sht.Range("E" + row + ":G" + row).Style.Font.Bold = true;

                using (var a = sht.Range("A3:G" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //*************
                sht.Columns(1, 15).AdjustToContents();
                string Fileextension = "xlsx";
                string name = "PackingOutSummary";
                //if (DDEmpName.SelectedIndex > 0)
                //{
                //    name = name + "-" + DDEmpName.SelectedItem.Text;
                //}
                string filename = UtilityModule.validateFilename("" + name + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altPacking", "alert('No records found for this combination.')", true);
            }
        }
        catch (Exception)
        {
            throw;
        }

    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = "";
        if (Session["VarCompanyNo"].ToString() == "36")
        {
            str = @"select PM.TInvoiceNo,vf.QualityName,vf.designName,vf.ColorName,PD.Width+'x'+PD.Length as Size,
                        sum(Pd.pcs) as pcs,Sum(PD.area) as Area,'" + txtfromdate.Text + "' as Fromdate,'" + txttodate.Text + @"' as Todate,dbo.F_GetstockNo(PD.ID) As StockNo
                        From Packing PM(NoLock) inner join PackingInformation PD(NoLock) on PM.PackingId=PD.PackingId
                        inner join V_FinishedItemDetail vf(NoLock) on PD.FinishedId=vf.ITEM_FINISHED_ID
                        Where PM.ConsignorId = " + Session["CurrentWorkingCompanyID"] + " And PM.PackingDate>='" + txtfromdate.Text + "' and PM.PackingDate<='" + txttodate.Text + @"'
                        group by PM.TInvoiceNo,vf.QualityName,vf.designName,vf.ColorName,PD.Width,PD.Length,PD.ID";
        }
        else
        {
             str = @"select PM.TInvoiceNo,vf.QualityName,vf.designName,vf.ColorName,PD.Width+'x'+PD.Length as Size,
                        sum(Pd.pcs) as pcs,Sum(PD.area) as Area,'" + txtfromdate.Text + "' as Fromdate,'" + txttodate.Text + @"' as Todate
                        From Packing PM(NoLock) inner join PackingInformation PD(NoLock) on PM.PackingId=PD.PackingId
                        inner join V_FinishedItemDetail vf(NoLock) on PD.FinishedId=vf.ITEM_FINISHED_ID
                        Where PM.ConsignorId = " + Session["CurrentWorkingCompanyID"] + " And PM.PackingDate>='" + txtfromdate.Text + "' and PM.PackingDate<='" + txttodate.Text + @"'
                        group by PM.TInvoiceNo,vf.QualityName,vf.designName,vf.ColorName,PD.Width,PD.Length";
        }  

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Session["VarCompanyNo"].ToString() == "36")
            {
                Session["rptFileName"] = "~\\Reports\\RptPackingoutDetailPrasad.rpt";
            }
            else
            {
                if (ChkForExcelReport.Checked == true)
                {
                    PackingOutExcelReport(ds);
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\RptPackingoutDetail.rpt";
                }
               
            }

            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptPackingoutDetail.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No Records Found..')", true);
        }

    }
}