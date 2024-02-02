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

public partial class Masters_ReportForms_FrmBazaarReportLotTagNoWise : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select GoDownID,GodownName From Godownmaster order by GodownName
                        select ShadecolorId,ShadeColorName From ShadeColor order by ShadeColorName
                        Select Distinct CI.CompanyId, CI.CompanyName 
                        From Companyinfo CI(nolock)
                        JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @"  
                        Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            //UtilityModule.ConditionalComboFillWithDS(ref DDgodown, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDshadeno, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDcompanyName, ds, 2, true, "--Plz Select--");
            if (DDcompanyName.Items.Count > 0)
            {
                DDcompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompanyName.Enabled = false;
            }
            TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtFromDate.Attributes.Add("readonly", "readonly");
            TxtToDate.Attributes.Add("readonly", "readonly");
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {

        SqlParameter[] array = new SqlParameter[3];
        array[0] = new SqlParameter("@FromDate", SqlDbType.DateTime);
        array[1] = new SqlParameter("@ToDate", SqlDbType.DateTime);
        array[2] = new SqlParameter("@Where", SqlDbType.VarChar, 3000);

        //string sQry = "  Vf.mastercompanyId=" + Session["varMasterCompanyIDForERP"] + "";

        string sQry = "";

        if (txtlotno.Text != "")
        {
            sQry = sQry + " AND TRM.LotNo = '" + txtlotno.Text + "'";
        }
        if (txttagno.Text != "")
        {
            sQry = sQry + " AND TRM.TagNo = '" + txttagno.Text + "'";
        }
        if (DDcompanyName.SelectedIndex > 0)
        {
            sQry = sQry + " AND TB.companyid =" + DDcompanyName.SelectedValue;
        }
        if (DDshadeno.SelectedIndex > 0)
        {
            sQry = sQry + " and VF2.ShadeColorId=" + DDshadeno.SelectedValue + "";
            //sQry = sQry + " and vf.ShadeColorName='" + DDshadeno.SelectedItem.Text + "'";
        }
        array[0].Value = TxtFromDate.Text;
        array[1].Value = TxtToDate.Text;
        array[2].Value = sQry;

        DataSet DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETBAZAARDETAILLOTTAGNOWISE", array);

        // DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);
        if (DS.Tables[0].Rows.Count > 0)
        {

            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("BazaarReportWithLotTagNoWise");
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:H1").Merge();
            sht.Range("A1:H1").Style.Font.FontSize = 10;
            sht.Range("A1:H1").Style.Font.Bold = true;
            sht.Range("A1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:H1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:H1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("FROM - " + TxtFromDate.Text + " " + "To " + TxtToDate.Text);
            //Detail headers                
            sht.Range("A2:H2").Style.Font.FontSize = 10;
            sht.Range("A2:H2").Style.Font.Bold = true;
            using (var a = sht.Range("A2:H2"))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            sht.Range("A2").Value = "RECEIVE DATE";
            sht.Range("B2").Value = "ITEM NAME";
            sht.Range("C2").Value = "QUALITY";
            sht.Range("D2").Value = "DESGIN";
            sht.Range("E2").Value = "COLOR";
            sht.Range("F2").Value = "SIZE";
            sht.Range("G2").Value = "TSTOCKNO";

            ////********
            //DataView dv = DS.Tables[0].DefaultView;
            //dv.Sort = "Date,ECISNO";
            //DataSet ds1 = new DataSet();
            //ds1.Tables.Add(dv.ToTable());
            ////
            int row = 3;
            for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":H" + row).Style.Font.FontSize = 10;
                using (var a = sht.Range("A" + row + ":H" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                sht.Range("A" + row).SetValue(DS.Tables[0].Rows[i]["ReceiveDate"]);
                sht.Range("B" + row).SetValue(DS.Tables[0].Rows[i]["Item_Name"]);
                sht.Range("C" + row).SetValue(DS.Tables[0].Rows[i]["QualityName"]);
                sht.Range("D" + row).SetValue(DS.Tables[0].Rows[i]["DesignName"]);
                sht.Range("E" + row).SetValue(DS.Tables[0].Rows[i]["ColorName"]);
                sht.Range("F" + row).SetValue(DS.Tables[0].Rows[i]["SizeMtr"]);
                sht.Range("G" + row).SetValue(DS.Tables[0].Rows[i]["TStockNo"]);


                row = row + 1;
            }
            DS.Dispose();

            ////**************grand Total
            //var Total = sht.Evaluate("SUM(J3:J" + (row - 1) + ")");
            //sht.Range("J" + row).Value = Total;
            //sht.Range("J" + row).Style.Font.Bold = true;

            //var ToalPD = sht.Evaluate("SUM(K3:K" + (row - 1) + ")");
            //sht.Range("K" + row).Value = ToalPD;
            //sht.Range("K" + row).Style.Font.Bold = true;
            ////************** Save
            String Path;
            sht.Columns(1, 20).AdjustToContents();

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("BAZAARREPORTLOTTAGNOWISE_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
}