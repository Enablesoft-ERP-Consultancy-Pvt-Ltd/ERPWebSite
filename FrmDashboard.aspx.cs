using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.IO;
using ClosedXML.Excel;

public partial class FrmDashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void ProductionSummary()
    {
        //String str = "";
        //str = str + " and cast(OM.Dateadded as date)>='" + txtfromdate.Text + "' and cast(Om.Dateadded as date)<='" + txttodate.Text + "'";
        //if (DDCustCode.SelectedIndex > 0)
        //{
        //    str = str + " and OM.customerid=" + DDCustCode.SelectedValue;
        //}
        //if (DDOrderNo.SelectedIndex > 0)
        //{
        //    str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
        //}
      
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_DashboardProductionData", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", Session["CurrentWorkingCompanyID"]);
        //cmd.Parameters.AddWithValue("@Where", str);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@FromDate", txtfromdate.Text);
        cmd.Parameters.AddWithValue("@ToDate", txttodate.Text);

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        con.Close();
        con.Dispose(); 


        //DataTable dt = new DataTable();
        //dt.Load(cmd.ExecuteReader());
        ////*************
        //DataSet ds = new DataSet();
        //ds.Tables.Add(dt);
        //con.Close();
        //con.Dispose();       


        //SqlParameter[] param = new SqlParameter[3];
        //param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        //param[1] = new SqlParameter("@Where", str);
        //param[2] = new SqlParameter("@userid", Session["varuserid"]);
        //***********
        // DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_DashboardProductionData", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Dashboard");
            int row = 0;
            //**************

            sht.Range("A1").Value = "PNM DASHBOARD :FROM " + ds.Tables[0].Rows[0]["FromDate"] + " TO " + ds.Tables[0].Rows[0]["ToDate"] + "";/// ds.Tables[0].Rows[0]["CompanyName"];
            sht.Range("A1:L1").Style.Font.FontName = "Calibri";
            sht.Range("A1:L1").Style.Font.Bold = true;
            sht.Range("A1:L1").Style.Font.FontSize = 12;
            sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:L1").Merge();


            sht.Range("A2").Value = "YARN STORE";
            sht.Range("A2:L2").Style.Font.FontName = "Calibri";
            sht.Range("A2:L2").Style.Font.Bold = true;
            sht.Range("A2:L2").Style.Font.FontSize = 11;
            sht.Range("A2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:L2").Merge();


            //*******************YARN DETAIL
            //Headers
            sht.Range("A3").Value = "DATE";
            sht.Range("B3").Value = "DAY";
            sht.Range("C3").Value = "RCV QTY";
            sht.Range("D3").Value = "OPENING ISSUE";
            sht.Range("E3").Value = "OPENING RCV";
            sht.Range("F3").Value = "ISSUE TO PRODUCTION";
            //***********      

            sht.Range("C3:F3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3:L3").Style.Font.FontName = "Calibri";
            sht.Range("A3:L3").Style.Font.Bold = true;
            sht.Range("A3:L3").Style.Font.FontSize = 11;
            sht.Range("A3:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A3:F3").Style.Fill.BackgroundColor = XLColor.BabyBlue;

            row = 4;            
            for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
            {

                sht.Range("A" + row + ":F" + row).Style.Font.FontName = "Calibri";
                sht.Range("A" + row + ":F" + row).Style.Font.FontSize = 10;
                sht.Range("A" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A" + row + ":F" + row).Style.Fill.BackgroundColor = XLColor.BabyBlue;

                sht.Range("A" + row).SetValue(ds.Tables[2].Rows[i]["FromDate"] + " - " + ds.Tables[2].Rows[i]["ToDate"]); //"CurrentDate";
                sht.Range("B" + row).SetValue("1"); //"BUYER CODE";
                sht.Range("C" + row).SetValue(ds.Tables[2].Rows[i]["GateInRecQty"]); //"GateInRecQty";
                sht.Range("D" + row).SetValue(ds.Tables[2].Rows[i]["YarnOpeningIssue"]); // = "YarnOpeningIssue";
                sht.Range("E" + row).SetValue(ds.Tables[2].Rows[i]["YarnOpeningReceive"]);  //"YarnOpeningReceive";
                sht.Range("F" + row).SetValue(ds.Tables[2].Rows[i]["WeaverToIssueQty"]);

                row = row + 1;
            }

            row = row + 2;           

            sht.Range("A" + row).Value = "PRODUCTION";
            sht.Range("A" + row + ":L" + row).Style.Font.FontName = "Calibri";
            sht.Range("A" + row + ":L" + row).Style.Font.Bold = true;
            sht.Range("A" + row + ":L" + row).Style.Font.FontSize = 11;
            sht.Range("A" + row + ":L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A" + row + ":L" + row).Merge();

            row = row + 1;    

            //*******************Production DETAIL
            //Headers
            sht.Range("C" + row).Value = "ITEM NAME";
            sht.Range("D" + row).Value = "ORDR ISSUE TO WEAVER(AREA)";
            sht.Range("E" + row).Value = "BAZAR (AREA)";
            sht.Range("F" + row).Value = "BAZAR REJECTION (AREA)";           

            //***********       

            sht.Range("A" + row + ":F" + row).Style.Font.FontName = "Calibri";
            sht.Range("A" + row + ":F" + row).Style.Font.Bold = true;
            sht.Range("A" + row + ":F" + row).Style.Font.FontSize = 11;
            sht.Range("A" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A" + row + ":F" + row).Style.Fill.BackgroundColor = XLColor.BabyBlue;

            row = row + 1;
            int rowfrom = 0;

            if (rowfrom == 0)
            {
                rowfrom = row;
            }
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            { 
                sht.Range("A" + row + ":F" + row).Style.Font.FontName = "Calibri";
                sht.Range("A" + row + ":F" + row).Style.Font.FontSize = 10;
                sht.Range("A" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A" + row + ":F" + row).Style.Fill.BackgroundColor = XLColor.BabyBlue;

                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[j]["ITEM_NAME"]); //"ITEM_NAME";
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[j]["WeaverIssueArea"]); // = "WeaverIssueArea";
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[j]["BazaarArea"]);  //"BazaarArea";
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[j]["BazaarRejectedArea"]);
               
                row = row + 1;
            }

            sht.Range("D" + row).FormulaA1 = "=SUM(D" + rowfrom + ":D" + (row - 1) + ")";
            sht.Range("E" + row).FormulaA1 = "=SUM(E" + rowfrom + ":E" + (row - 1) + ")";
            sht.Range("F" + row).FormulaA1 = "=SUM(F" + rowfrom + ":F" + (row - 1) + ")";
            sht.Range("D" + row + ":F" + row).Style.Font.FontName = "Calibri";
            sht.Range("D" + row + ":F" + row).Style.Font.FontSize = 10;
            sht.Range("D" + row + ":F" + row).Style.Font.Bold = true;

            row = row + 2;

            sht.Range("A" + row).Value = "FINISHING";
            sht.Range("A" + row + ":L" + row).Style.Font.FontName = "Calibri";
            sht.Range("A" + row + ":L" + row).Style.Font.Bold = true;
            sht.Range("A" + row + ":L" + row).Style.Font.FontSize = 11;
            sht.Range("A" + row + ":L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A" + row + ":L" + row).Merge();

            row = row + 1;

            //*******************Finishing DETAIL
            //Headers
            sht.Range("C" + row).Value = "LATEXING";
            sht.Range("D" + row).Value = "PATTI FOLDING";
            sht.Range("E" + row).Value = "THIRD BACKING";
            sht.Range("F" + row).Value = "BINDING";
            sht.Range("G" + row).Value = "CLIPPING";
            sht.Range("H" + row).Value = "EMBOSSING";
            sht.Range("I" + row).Value = "DRY WASH";
            sht.Range("J" + row).Value = "STREACHING 1ST";
            sht.Range("K" + row).Value = "FINAL FINISHING";
            sht.Range("L" + row).Value = "DISPATCH";
            //***********        


           // sht.Range("D3:F3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A" + row + ":L" + row).Style.Font.FontName = "Calibri";
            sht.Range("A" + row + ":L" + row).Style.Font.Bold = true;
            sht.Range("A" + row + ":L" + row).Style.Font.FontSize = 11;
            sht.Range("A" + row + ":L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A" + row + ":L" + row).Style.Fill.BackgroundColor = XLColor.BabyBlue;

            row = row + 1;
            int rowfrom2 = 0;
            if (rowfrom2 == 0)
            {
                rowfrom2 = row;
            }           
            if (ds.Tables[1].Rows.Count > 0)
            {
                for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
                {
                    sht.Range("A" + row + ":L" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":L" + row).Style.Font.FontSize = 10;
                    sht.Range("A" + row + ":L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("A" + row + ":L" + row).Style.Fill.BackgroundColor = XLColor.BabyBlue;

                    sht.Range("C" + row).SetValue(ds.Tables[1].Rows[k]["LATEXING"]); //"BUYER CODE";
                    sht.Range("D" + row).SetValue(ds.Tables[1].Rows[k]["PATTI FOLDING"]); // = "Local OrderNo";
                    sht.Range("E" + row).SetValue(ds.Tables[1].Rows[k]["THIRD BACKING"]);  //"DELvdate";
                    sht.Range("F" + row).SetValue(ds.Tables[1].Rows[k]["BINDING"]);
                    sht.Range("G" + row).SetValue(ds.Tables[1].Rows[k]["CLIPPING SINGLE"]);
                    sht.Range("H" + row).SetValue(ds.Tables[1].Rows[k]["EMBOSSING"]);
                    sht.Range("I" + row).SetValue(ds.Tables[1].Rows[k]["DRY WASH"]);
                    sht.Range("J" + row).SetValue(ds.Tables[1].Rows[k]["STREACHING 1ST"]);
                    sht.Range("K" + row).SetValue(ds.Tables[1].Rows[k]["FINAL FINISHING"]);
                    sht.Range("L" + row).SetValue(ds.Tables[1].Rows[k]["DISPATCH"]);

                    row = row + 1;
                }              
          
            }


            //********************
            sht.Columns(1, 20).AdjustToContents();
            //******************
            sht.Row(1).Height = 28.80;
            sht.Range("A1:H1").Style.Alignment.WrapText = true;

            using (var a = sht.Range("A1" + ":L" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = "";          
            filename = UtilityModule.validateFilename("DashboardProduction_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);          

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
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
        }

    }
    protected void BtnDownloadDashboardData_Click(object sender, EventArgs e)
    {
        ProductionSummary();
    }
}