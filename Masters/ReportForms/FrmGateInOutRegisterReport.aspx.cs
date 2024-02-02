using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ClosedXML.Excel;
using System.Text.RegularExpressions;

public partial class Masters_ReportForms_FrmGateInOutRegisterReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");           
            txtFromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
        }
    }
    protected void DDRegisterType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDRegisterType.SelectedValue == "2")
        {
            DDGateType.Enabled = false;
            DDGateType.SelectedIndex = 1;
            DDMaterialReturnType.SelectedIndex = 1;
            TRMaterialType.Visible = true;
        }
        else
        {
            DDGateType.Enabled = true;
            TRMaterialType.Visible = false;
            DDMaterialReturnType.SelectedIndex = 0;            
        }        
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        GateInOutRegisterReport();
        //switch (Session["varMasterCompanyIDForERP"].ToString())
        //{
        //    case "14":
        //        QCreport();
        //        break;
        //    default:
        //        QCreport_Other();
        //        break;
        //}
        ////
    }
    private static readonly Regex InvalidFileRegex = new Regex(string.Format("[{0}]", Regex.Escape(@"<>:""/\|?*")));
    public static string validateFilename(string filename)
    {
        return InvalidFileRegex.Replace(filename, string.Empty);
    }
    protected void GateInOutRegisterReport()
    {
        string where = "";
        lblmsg.Text = "";
        int RowNo = 0;
        if (DDMaterialReturnType.SelectedIndex > 0)
        {
            where = where + " and GOMR.MaterialType=" + DDMaterialReturnType.SelectedValue;
        }

        //********Proc
        SqlParameter[] param = new SqlParameter[6];
        param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
        param[1] = new SqlParameter("@RegisterType", DDRegisterType.SelectedValue);
        param[2] = new SqlParameter("@GateType", DDGateType.SelectedValue);
        param[3] = new SqlParameter("@fromdate", txtFromdate.Text);
        param[4] = new SqlParameter("@Todate", txttodate.Text);
        param[5] = new SqlParameter("@where", where);
        //******
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GateInOutRegisterExcelReport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Gate In Out Register");

            //************* 
            sht.Range("A1:L1").Merge();
            sht.Range("A1").Value = DDcompany.SelectedItem.Text;
            sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:L1").Style.Font.FontSize = 12;
            sht.Row(2).Height = 32.25;
            //
            sht.Range("A3:L3").Merge();
            sht.Range("A3").Value = DDGateType.SelectedItem.Text + " REPORT From " + txtFromdate.Text + " to " + txttodate.Text + "";
            sht.Range("A3:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A3:L3").Style.Font.FontSize = 12;

            //
            sht.Range("A5").Value = DDGateType.SelectedItem.Text + "Date";
            sht.Range("B5").Value = "GatePassNo";
            sht.Range("C5").Value = "PartyName";
            sht.Range("D5").Value = "Qty";
            sht.Range("E5").Value = "Unit";
            sht.Range("F5").Value = "VehicleNo";
            sht.Range("G5").Value = "Through";
            sht.Range("H5").Value = "MobileNo";
            sht.Range("I5:J5").Value = "MaterialDescription";
            sht.Range("I5:J5").Merge();
           // sht.Range("J5").Value = DDGateType.SelectedItem.Text + "Time";
            sht.Range("K5").Value = "MaterialType";
            sht.Range("A5:K5").Style.Font.Bold = true;

           // sht.Column(11).Hide();
           
            // values
            RowNo = 6;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + RowNo).Value = ds.Tables[0].Rows[i]["GateInOutDate"];
                sht.Range("B" + RowNo).Value = ds.Tables[0].Rows[i]["GatePassNo"];
                sht.Range("C" + RowNo).Value = ds.Tables[0].Rows[i]["PartyName"];
                sht.Range("D" + RowNo).Value = ds.Tables[0].Rows[i]["Qty"];
                sht.Range("E" + RowNo).Value = ds.Tables[0].Rows[i]["Unit"];
                sht.Range("F" + RowNo).Value = ds.Tables[0].Rows[i]["VehicleNo"];
                sht.Range("G" + RowNo).Value = ds.Tables[0].Rows[i]["Through"];
                sht.Range("H" + RowNo).Value = ds.Tables[0].Rows[i]["MobileNo"];
                sht.Range("I" + RowNo+":J" + RowNo).Value = ds.Tables[0].Rows[i]["MaterialDescription"];
                sht.Range("I" + RowNo + ":J" + RowNo).Merge();
                //sht.Range("J" + RowNo).Style.NumberFormat.Format = "@";
              //  sht.Range("J" + RowNo).Value = ds.Tables[0].Rows[i]["GateInOutTime"]; 
                sht.Range("K" + RowNo).Value = ds.Tables[0].Rows[i]["MaterialType"];

                RowNo = RowNo + 1;
               
            }
            sht.Columns(1, 20).AdjustToContents();

            using (var a = sht.Range("A5" + ":K" + RowNo))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            //*********************************
            string Fileextension = "xlsx";
            string filename = validateFilename("GateInOutRegister-" + DateTime.Now + "." + Fileextension);
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
            //*****
            //File.Delete(Path);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    
}
