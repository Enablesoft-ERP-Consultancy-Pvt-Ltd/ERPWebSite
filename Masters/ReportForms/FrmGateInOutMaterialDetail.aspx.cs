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

public partial class Masters_ReportForms_FrmGateInOutMaterialDetail : System.Web.UI.Page
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
        report();       
    }
    private void report()
    {
        DataSet ds = new DataSet(); 

        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@CompanyId", DDcompany.SelectedValue);
        param[1] = new SqlParameter("@GateType", DDGateTypeInOut.SelectedValue);
        param[2] = new SqlParameter("@fromdate", txtfromdate.Text);
        param[3] = new SqlParameter("@Todate", txttodate.Text);
        //*************
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetGateInOutMaterialReport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/TempExcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/TempExcel/"));
            }
            //*********************
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("GateInOutMaterialReport");

            //****************
            //*************
            sht.Range("A1:M1").Merge();
            sht.Range("A1:M1").Style.Font.FontSize = 11;
            sht.Range("A1:M1").Style.Font.Bold = true;
            sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:M1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1").SetValue(DDGateTypeInOut.SelectedItem.Text + " Material Between  " + txtfromdate.Text + "---------" + txttodate.Text + "");
            sht.Row(1).Height = 21.75;
            sht.Range("A2:M2").Merge();
            sht.Range("A2:M2").Style.Font.FontSize = 11;
            sht.Range("A2:M2").Style.Font.Bold = true;
            sht.Range("A2:M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:M2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2").SetValue(DDcompany.SelectedItem.Text);
            //*********
            sht.Range("A3:M3").Style.Font.FontSize = 11;
            sht.Range("A3:M3").Style.Font.Bold = true;
            sht.Range("A3:M3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("B3:M3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Row(3).Height = 18.00;

            if (DDGateTypeInOut.SelectedValue == "1")
            {
                sht.Range("A3").SetValue("GateInDate");
                sht.Range("H3").SetValue("GatePassIn No");
                sht.Range("L3").SetValue("InTime");
            }
            else if (DDGateTypeInOut.SelectedValue == "2")
            {
                sht.Range("A3").SetValue("GateOutDate");
                sht.Range("H3").SetValue("GatePassOut No");
                sht.Range("L3").SetValue("OutTime");                
            }

            sht.Range("B3").SetValue("MaterialType");
            sht.Range("C3").SetValue("Company Name");
            sht.Range("D3").SetValue("Party Name");
            sht.Range("E3").SetValue("Qty");
            sht.Range("F3").SetValue("Unit");
            sht.Range("G3").SetValue("Material Description");

            sht.Range("I3").SetValue("Vehicle No");
            sht.Range("J3").SetValue("Through");
            sht.Range("K3").SetValue("Mobile No");
            sht.Range("M3").SetValue("Remarks");

            using (var a = sht.Range("A3:M3"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //**************************************************

            int Row = 4;
            int rowcount = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rowcount; i++)
            {
                sht.Range("A" + Row + ":M" + Row).Style.Font.FontSize = 11;

                //if (DDGateTypeInOut.SelectedValue == "1")
                //{                    
                //    sht.Range("L" + Row).SetValue(ds.Tables[0].Rows[i]["GateInOutTime"]);
                //}
                //else if (DDGateTypeInOut.SelectedValue == "2")
                //{
                //    sht.Range("H" + Row).SetValue(ds.Tables[0].Rows[i]["GatePassNo"]);                    
                //}

                sht.Range("A" + Row).SetValue(ds.Tables[0].Rows[i]["GateMaterialDate"]);
                sht.Range("B" + Row).SetValue(ds.Tables[0].Rows[i]["MaterialType"]);
                sht.Range("C" + Row).SetValue(ds.Tables[0].Rows[i]["CompanyName"]);
                sht.Range("D" + Row).SetValue(ds.Tables[0].Rows[i]["PartyName"]);
                sht.Range("E" + Row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                sht.Range("F" + Row).SetValue(ds.Tables[0].Rows[i]["Unit"]);
                sht.Range("G" + Row).SetValue(ds.Tables[0].Rows[i]["MaterialDescription"]);
                sht.Range("H" + Row).SetValue(ds.Tables[0].Rows[i]["GatePassNo"]);

                sht.Range("I" + Row).SetValue(ds.Tables[0].Rows[i]["VehicleNo"]);
                sht.Range("J" + Row).SetValue(ds.Tables[0].Rows[i]["Through"]);
                sht.Range("K" + Row).SetValue(ds.Tables[0].Rows[i]["MobileNo"]);
                sht.Range("L" + Row).SetValue(ds.Tables[0].Rows[i]["GateInOutTime"]);
                sht.Range("M" + Row).SetValue(ds.Tables[0].Rows[i]["Remarks"]);

                using (var a = sht.Range("A" + Row + ":M" + Row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                Row = Row + 1;
            }
            //**********Total
            var TotalQty = sht.Evaluate("SUM(E4:E" + (Row - 1) + ")");
            //*************
            sht.Columns(1, 13).AdjustToContents();
            sht.Range("D" + Row).SetValue("Total");
            sht.Range("D" + Row + ":E" + Row).Style.Font.Bold = true;
            sht.Range("E" + Row).SetValue(TotalQty);           

            using (var a = sht.Range("A" + Row + ":M" + Row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //**************Save
            //******SAVE FILE
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("GateInOutMaterial-" + DateTime.Now + "." + Fileextension);
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
}