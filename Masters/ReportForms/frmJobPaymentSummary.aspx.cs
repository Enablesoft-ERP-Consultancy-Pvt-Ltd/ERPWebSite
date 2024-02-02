using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using ClosedXML.Excel;


public partial class Masters_ReportForms_frmJobPaymentSummary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            //SqlhelperEnum.FillDropDown(AllEnums.MasterTables.PROCESS_NAME_MASTER, DDjob, pWhere: "MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + "", pID: "Process_name_Id", pName: "Process_name", pFillBlank: true, Selecttext: "--Plz select Job");
            UtilityModule.ConditionalComboFill(ref DDjob, "select PROCESS_NAME_ID,Process_name from PROCESS_NAME_MASTER  order by Process_Name", true, "--Plz Select Process--");
            UtilityModule.ConditionalComboFill(ref DDyear, "select Year,Year From YearData", false, "");
            DDMonth.SelectedValue = DateTime.Now.Month.ToString();
            DDyear.SelectedValue = DateTime.Now.Year.ToString();



            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "22":
                    if (Session["usertype"].ToString() == "1")
                    {
                        TDForWeaverWiseDetail.Visible = true;
                    }
                    else
                    {
                        TDForWeaverWiseDetail.Visible = false;
                    }
                    
                    break;
                default :
                    TDForWeaverWiseDetail.Visible = false;
                    break;
            }
        }
    }
    protected void btnprint_Click(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_ForJobPaymentSummary", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@ProcessId", DDjob.SelectedValue);
        cmd.Parameters.AddWithValue("@Month", DDMonth.SelectedItem.Text);
        cmd.Parameters.AddWithValue("@Year", DDyear.SelectedItem.Text);
        cmd.Parameters.AddWithValue("@EmpCode", txtIdNo.Text);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
        cmd.Parameters.AddWithValue("@CompanyId", 1);
        cmd.Parameters.AddWithValue("@ChkForWeaverWiseDetail", ChkForWeaverWiseDetail.Checked==true ? 1 : 0);

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ChkForWeaverWiseDetail.Checked == true)
            {
                WeaverWiseDetailReportInExcel(ds);
            }
            else
            {
                // Table 1 For Show image in crystal Report
                ds.Tables[1].Columns.Add("Image", typeof(System.Byte[]));
                foreach (DataRow dr in ds.Tables[1].Rows)
                {

                    if (Convert.ToString(dr["Photo"]) != "")
                    {
                        FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                        if (TheFile.Exists)
                        {
                            string img = dr["Photo"].ToString();
                            img = Server.MapPath(img);
                            Byte[] img_Byte = File.ReadAllBytes(img);
                            dr["Image"] = img_Byte;
                        }
                    }
                }
                Session["dsFileName"] = "~\\ReportSchema\\RptJobPaymentSummary.xsd";
                Session["rptFileName"] = "Reports/RptJobPaymentSummary.rpt";

                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }

        
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}
        //SqlTransaction Tran = con.BeginTransaction();
        //try
        //{
        //    lblErrorMsg.Text = "";
        //    SqlParameter[] _array = new SqlParameter[8];
        //    _array[0] = new SqlParameter("@ProcessId", SqlDbType.Int);
        //    _array[1] = new SqlParameter("@Month", SqlDbType.VarChar, 10);
        //    _array[2] = new SqlParameter("@Year", SqlDbType.VarChar, 10);
        //    _array[3] = new SqlParameter("@EmpCode", SqlDbType.VarChar, 20);
        //    _array[4] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        //    _array[5] = new SqlParameter("@CompanyId", SqlDbType.Int);



        //    _array[0].Value = DDjob.SelectedValue;// DDProcessName.SelectedValue;
        //    _array[1].Value = DDMonth.SelectedItem.Text;
        //    _array[2].Value = DDyear.SelectedItem.Text;
        //    _array[3].Value = txtIdNo.Text;
        //    _array[4].Value = Session["varMasterCompanyIDForERP"];
        //    _array[5].Value = 1;// DDCompany.SelectedValue;


        //    DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "[Pro_ForJobPaymentSummary]", _array);

        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        // Table 1 For Show image in crystal Report
        //        ds.Tables[1].Columns.Add("Image", typeof(System.Byte[]));
        //        foreach (DataRow dr in ds.Tables[1].Rows)
        //        {

        //            if (Convert.ToString(dr["Photo"]) != "")
        //            {
        //                FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
        //                if (TheFile.Exists)
        //                {
        //                    string img = dr["Photo"].ToString();
        //                    img = Server.MapPath(img);
        //                    Byte[] img_Byte = File.ReadAllBytes(img);
        //                    dr["Image"] = img_Byte;
        //                }
        //            }
        //        }
        //        Session["dsFileName"] = "~\\ReportSchema\\RptJobPaymentSummary.xsd";
        //        Session["rptFileName"] = "Reports/RptJobPaymentSummary.rpt";

        //        Session["GetDataset"] = ds;
        //        StringBuilder stb = new StringBuilder();
        //        stb.Append("<script>");
        //        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
        //        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        //    }
        //    else
        //    {

        //        ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);

        //    }
        //    Tran.Commit();
        //}
        //catch (Exception ex)
        //{
        //    lblErrorMsg.Text = ex.Message;
        //    Tran.Rollback();
        //}
        //finally
        //{
        //    con.Dispose();
        //    con.Close();
        //}
    }

    private void WeaverWiseDetailReportInExcel(DataSet ds)
    {
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

            //sht.Range("A1:N1").Merge();
            //sht.Range("A1").Value = ds.Tables[0].Rows[0]["ProcessName"] + " " + "WEAVER WISE LIST FORMAT";
            ////sht.Range("A2:X2").Merge();
            ////sht.Range("A2").Value = "Filter By :  " + FilterBy;
            ////sht.Row(2).Height = 30;
            //sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //sht.Range("A2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //sht.Range("A2:N2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            //sht.Range("A2:N2").Style.Alignment.SetWrapText();
            //sht.Range("A1:N2").Style.Font.FontName = "Arial";
            //sht.Range("A1:N2").Style.Font.FontSize = 10;
            //sht.Range("A1:N2").Style.Font.Bold = true;

            //*******Header
            sht.Range("A1").Value = "PROCESS NAME";
            sht.Range("B1").Value = "EMP CODE";
            sht.Range("C1").Value = "EMP NAME";
            sht.Range("D1").Value = "ARTICLE";
            sht.Range("E1").Value = "COLOR";
            sht.Range("F1").Value = "SIZE";
            sht.Range("G1").Value = "QTY";
            sht.Range("H1").Value = "RATE";
            sht.Range("I1").Value = "AMOUNT"; 

            sht.Range("A1:I1").Style.Font.FontName = "Arial";
            sht.Range("A1:I1").Style.Font.FontSize = 9;
            sht.Range("A1:I1").Style.Font.Bold = true;
            sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:I1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A1:I1").Style.Alignment.SetWrapText();


            //DataView dv = new DataView(ds.Tables[0]);
            //dv.Sort = "FOLIONO";
            //DataSet ds1 = new DataSet();
            //ds1.Tables.Add(dv.ToTable());

            int srno = 0;
            row = 2;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":I" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":I" + row).Style.Font.FontSize = 8;

                srno = srno + 1;

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Process_Name"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["EmpCode"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Size"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Amount"]);               


                row = row + 1;

            }
            //*************
            sht.Columns(1, 26).AdjustToContents();

            //sht.Columns("K").Width = 13.43;

            using (var a = sht.Range("A1" + ":I" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("WeaverWiseDetailReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
    }
}