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

public partial class Masters_ReportForms_FrmDirectPackCarpetDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {

            string str = @"Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varCompanyId"] + @" Order by Companyname";
            DataSet ds = SqlHelper.ExecuteDataset(str);
            CommanFunction.FillComboWithDS(DDCompany, ds, 0);
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }

    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_GetDirectPackCarpetReportDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@CompanyId", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Fromdate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
            cmd.Parameters.AddWithValue("@UserId", Session["varUserId"]);


            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            if(ds.Tables[0].Rows.Count>0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("DirectPackStockDetail");
                //**********************
                sht.Range("A1:J1").Merge();
                sht.Range("A1").Value = "From"+" "+txtfromdate.Text+" "+"To"+" " +txttodate.Text;
                sht.Range("A1:J1").Style.Font.Bold = true;
                sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:J1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1:J1").Style.Font.FontSize = 13;
                sht.Row(1).Height = 30.00;

                sht.Range("A2").Value = "DATE";
                sht.Range("B2").Value = "BARCODE";
                sht.Range("C2").Value = "CATEGORY";
                sht.Range("D2").Value = "ITEM NAME";
                sht.Range("E2").Value = "QUALITY";
                sht.Range("F2").Value = "DESIGN";
                sht.Range("G2").Value = "COLOR";
                sht.Range("H2").Value = "SHAPE";
                sht.Range("I2").Value = "SIZE";
                sht.Range("J2").Value = "QTY";
                sht.Range("A2:J2").Style.Font.Bold = true;
                sht.Range("A2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:J2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A2:J2").Style.Font.FontSize = 13;
                sht.Row(1).Height = 25.50;
                //**********************
                int row = 3;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":J" + row).Style.NumberFormat.Format = "@";
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Date"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["TStockNo"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CATEGORY_NAME"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);

                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ShapeName"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["SizeMtr"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);

                    row = row + 1;
                }
                //************************                
                sht.Columns(1, 10).AdjustToContents();
                //******SAVE FILE
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("DirectPackCarpetDetail" + DateTime.Now + "." + Fileextension);
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


            con.Close();
            con.Dispose();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
        }


//        string str = @"select PM.TInvoiceNo,vf.QualityName,vf.designName,vf.ColorName,PD.Width+'x'+PD.Length as Size,
//                        sum(Pd.pcs) as pcs,Sum(PD.area) as Area,'" + txtfromdate.Text + "' as Fromdate,'" + txttodate.Text + @"' as Todate
//                        From Packing PM inner join PackingInformation PD on PM.PackingId=PD.PackingId
//                        inner join V_FinishedItemDetail vf on PD.FinishedId=vf.ITEM_FINISHED_ID
//                        Where PM.ConsignorId = " + Session["CurrentWorkingCompanyID"] + " And PM.PackingDate>='" + txtfromdate.Text + "' and PM.PackingDate<='" + txttodate.Text + @"'
//                        group by PM.TInvoiceNo,vf.QualityName,vf.designName,vf.ColorName,PD.Width,PD.Length";

//        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

//        if (ds.Tables[0].Rows.Count > 0)
//        {
//            Session["rptFileName"] = "~\\Reports\\RptPackingoutDetail.rpt";
//            Session["Getdataset"] = ds;
//            Session["dsFileName"] = "~\\ReportSchema\\RptPackingoutDetail.xsd";
//            StringBuilder stb = new StringBuilder();
//            stb.Append("<script>");
//            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
//            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
//        }
//        else
//        {
//            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No Records Found..')", true);
//        }

    }
}