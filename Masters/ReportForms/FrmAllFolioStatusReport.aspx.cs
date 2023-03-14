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

public partial class Masters_ReportForms_FrmAllFolioStatusReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDCompany, "select CI.CompanyId,CI.CompanyName From Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "", false, "");
           
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {        
        lblErrmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }        
        try
        {

            SqlCommand cmd = new SqlCommand("Pro_CompanyWiseTotalProductionOrder", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@FromDate", txtfromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", txttodate.Text);
            cmd.Parameters.AddWithValue("@ChkDateFlag", ChkselectDate.Checked == true ? 1 : 0);
            cmd.Parameters.AddWithValue("@UserId", Session["varuserid"]);
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyId"]);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            //***********
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

                sht.Range("A1").Value = DDCompany.SelectedItem.Text;
                sht.Range("A1:D1").Style.Font.Bold = true;
                sht.Range("A1:D1").Style.Font.FontSize = 15;
                sht.Range("A1:D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:D1").Merge();

                string RawTwo = "";
                if (ChkselectDate.Checked == true)
                {
                    RawTwo = RawTwo + " TOTAL FOLIO DETAILS FROM DATE : " + txtfromDate.Text + " TO DATE : " + txttodate.Text;
                }
                else
                {
                    RawTwo = "";
                }               

                sht.Range("A2").Value = RawTwo;
                sht.Range("A2:D2").Style.Font.Bold = true;
                sht.Range("A2:D2").Style.Font.FontSize = 12;
                sht.Range("A2:D2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:D2").Merge();

                ////*******Header
                sht.Range("A3").Value = "RUNNING FOLIO";
                sht.Range("B3").Value = "COMPLETE FOLIO";
                sht.Range("C3").Value = "PENDING FOLIO";

                sht.Column("D3").Width = 33.59;

                sht.Range("A3:D3").Style.Font.FontName = "Calibri";
                sht.Range("A3:D3").Style.Font.FontSize = 9;
                sht.Range("A3:D3").Style.Font.Bold = true;

                ////sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 4;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["RunningFolio"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CompleteFolio"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["PendingFolio"]);                    
                   
                    row = row + 1;
                }

                //*************
                sht.Columns(1, 20).AdjustToContents();

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "D")))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("TotalFolioDetails_" + DateTime.Now + "." + Fileextension);
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
        catch (Exception ex)
        {
            lblErrmsg.Text = ex.Message;
            //Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
}