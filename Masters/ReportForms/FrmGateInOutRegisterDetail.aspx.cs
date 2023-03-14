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

public partial class Masters_ReportForms_FrmGateInOutRegisterDetail : System.Web.UI.Page
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
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@CompanyId", DDcompany.SelectedValue);
        param[1] = new SqlParameter("@GateTypeInOut", DDGateTypeInOut.SelectedValue);
        param[2] = new SqlParameter("@fromdate", txtfromdate.Text);
        param[3] = new SqlParameter("@Todate", txttodate.Text);
        //*************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetGateInOutRegisterReport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/TempExcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/TempExcel/"));
            }
            //*********************
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("GateInOutRegisterReport");            

            //****************
            //*************
            sht.Range("A1:L1").Merge();
            sht.Range("A1:L1").Style.Font.FontSize = 11;
            sht.Range("A1:L1").Style.Font.Bold = true;
            sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:L1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1").SetValue(DDGateTypeInOut.SelectedItem.Text+" Register Between  " + txtfromdate.Text + "---------" + txttodate.Text + "");
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
            sht.Range("B3:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Row(3).Height = 18.00;

            if (DDGateTypeInOut.SelectedValue == "1")
            {
                sht.Range("A3").SetValue("GateInDate");
                sht.Range("G3").SetValue("Challan No");
                sht.Range("K3").SetValue("InTime");                
            }
            else if (DDGateTypeInOut.SelectedValue == "2")
            {
                sht.Range("A3").SetValue("GateOutDate");
                sht.Range("G3").SetValue("GPNo");
                sht.Range("K3").SetValue("OutTime");                
            }
            
            sht.Range("B3").SetValue("Company Name");
            sht.Range("C3").SetValue("Party Name");
            sht.Range("D3").SetValue("Qty");
            sht.Range("E3").SetValue("Unit");
            sht.Range("F3").SetValue("Material Description");            
           
            sht.Range("H3").SetValue("Vehicle No");
            sht.Range("I3").SetValue("Through");
            sht.Range("J3").SetValue("Mobile No");
            sht.Range("L3").SetValue("Remarks");            
                       
            using (var a = sht.Range("A3:L3"))
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
                sht.Range("A" + Row + ":L" + Row).Style.Font.FontSize = 11;

                if (DDGateTypeInOut.SelectedValue == "1")
                {
                    sht.Range("G" + Row).SetValue(ds.Tables[0].Rows[i]["ChallanNo"]);
                    sht.Range("K" + Row).SetValue(ds.Tables[0].Rows[i]["InTime"]);
                }
                else if (DDGateTypeInOut.SelectedValue == "2")
                {
                    sht.Range("G" + Row).SetValue(ds.Tables[0].Rows[i]["GPNo"]);
                    sht.Range("K" + Row).SetValue(ds.Tables[0].Rows[i]["OutTime"]);                   
                }

                sht.Range("A" + Row).SetValue(ds.Tables[0].Rows[i]["GateInOutDate"]);
                sht.Range("B" + Row).SetValue(ds.Tables[0].Rows[i]["CompanyName"]);
                sht.Range("C" + Row).SetValue(ds.Tables[0].Rows[i]["PartyName"]);
                sht.Range("D" + Row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                sht.Range("E" + Row).SetValue(ds.Tables[0].Rows[i]["Unit"]);
                sht.Range("F" + Row).SetValue(ds.Tables[0].Rows[i]["MaterialDescription"]);
                
                sht.Range("H" + Row).SetValue(ds.Tables[0].Rows[i]["VehicleNo"]);
                sht.Range("I" + Row).SetValue(ds.Tables[0].Rows[i]["Through"]);
                sht.Range("J" + Row).SetValue(ds.Tables[0].Rows[i]["MobileNo"]);
                sht.Range("L" + Row).SetValue(ds.Tables[0].Rows[i]["Remarks"]);                

                using (var a = sht.Range("A" + Row + ":L" + Row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                
                Row = Row + 1;
            }
            //**********Total
            var TotalQty = sht.Evaluate("SUM(D4:D" + (Row - 1) + ")");           
            //*************
            sht.Columns(1, 13).AdjustToContents();
            sht.Range("C" + Row).SetValue("Total");
            sht.Range("C" + Row +":D" + Row).Style.Font.Bold = true;
            sht.Range("D" + Row).SetValue(TotalQty);

            using (var a = sht.Range("A" + Row + ":L" + Row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //**************Save
            //******SAVE FILE
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("GateInOutRegister-" + DateTime.Now + "." + Fileextension);            
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