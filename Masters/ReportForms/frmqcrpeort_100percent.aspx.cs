﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ClosedXML.Excel;

public partial class Masters_ReportForms_frmqcrpeort_100percent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select CI.CompanyId,CI.CompanyName 
                        From Companyinfo CI,Company_Authentication CA 
                        Where CI.CompanyId=CA.COmpanyid And CA.Userid=" + Session["varuserid"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @"

                        SELECT PROCESS_NAME_ID,PROCESS_NAME FROM  PROCESS_NAME_MASTER PNM WHERE PROCESS_NAME NOT LIKE 'AQL%' ORDER BY PROCESS_NAME

                        Select Distinct OM.CustomerID, CI.CustomerCode 
                        From OrderMaster OM(Nolock) 
                        JOIN CustomerInfo CI(Nolock) ON CI.CustomerID = OM.CustomerID And CI.MasterCompanyid = " + Session["varCompanyId"] + @" 
                        Where OM.CompanyId = " + Session["CurrentWorkingCompanyID"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDprocessname, ds, 1, true, "--Plz Select Process--");

            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds, 2, true, "--Customer Code--");

            ds.Dispose();
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }

    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDOrderNo, @"Select OM.OrderID, OM.CustomerOrderNo  
                From OrderMaster OM(Nolock) 
                Where OM.CompanyId = " + DDCompany.SelectedValue + " And OM.CustomerId =  " + DDCustomerCode.SelectedValue + @"
                Order By OM.OrderID", true, "--Plz Select--");
    }

    protected void btngetdata1_Click(object sender, EventArgs e)
    {
        if (DDprocessname.SelectedItem.Text.Contains("100%"))
        {
            Qcreport_100percent();
        }
        else
        {
            Qcreport();
        }
    }
    protected void Qcreport()
    {
        lblmsg.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@companyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@processid", DDprocessname.SelectedValue);
            param[2] = new SqlParameter("@fromdate", txtfromdate.Text);
            param[3] = new SqlParameter("@Todate", txttodate.Text);
            param[4] = new SqlParameter("@CustomerID", DDCustomerCode.SelectedValue);

            if (DDOrderNo.Items.Count > 0)
            {
                param[5] = new SqlParameter("@OrderID", DDOrderNo.SelectedValue);
            }
            else
            {
                param[5] = new SqlParameter("@OrderID", 0);
            }

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETQCREPORT_100PERCENT", param);
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

                sht.Range("A1:K1").Merge();
                sht.Range("A1").SetValue("QC REPORT (" + DDprocessname.SelectedItem.Text + ")");
                sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:K2").Merge();
                sht.Range("A2").SetValue("From :" + txtfromdate.Text + "  To : " + txttodate.Text);
                sht.Range("A2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:K2").Style.Font.SetBold();

                //Headers
                sht.Range("A3").Value = "DATE";
                sht.Range("B3").Value = "EMP NAME";
                sht.Range("C3").Value = "STOCK NO.";
                sht.Range("D3").Value = "STYLE NAME";
                sht.Range("E3").Value = "COLOUR NAME";
                sht.Range("F3").Value = "SIZE";
                sht.Range("G3").Value = "QTY";
                sht.Range("H3").Value = "DEFECTS";
                sht.Range("I3").Value = "RESULT";
                sht.Range("J3").Value = "REMARK";
                sht.Range("K3").Value = "SCAN BY";

                sht.Range("A3:K3").Style.Font.Bold = true;

                row = 4;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Receivedate"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Tstockno"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["designname"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["colorname"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Sizeft"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["qty"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["defect"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["defect"].ToString() == "" ? "PASS" : "FAIL");
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["remark"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["scanby"]);

                    row = row + 1;

                }
                using (var a = sht.Range("A3:K" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                sht.Columns(1, 20).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("QCREPORT(" + DDprocessname.SelectedItem.Text + ")" + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altrpt", "alert('No records found for this combination.')", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void Qcreport_100percent()
    {
        lblmsg.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@companyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@processid", DDprocessname.SelectedValue);
            param[2] = new SqlParameter("@fromdate", txtfromdate.Text);
            param[3] = new SqlParameter("@Todate", txttodate.Text);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETQCREPORT_100PERCENT", param);
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

                sht.Range("A1:I1").Merge();
                sht.Range("A1").SetValue("QC REPORT (" + DDprocessname.SelectedItem.Text + ")");
                sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:I2").Merge();
                sht.Range("A2").SetValue("From :" + txtfromdate.Text + "  To : " + txttodate.Text);
                sht.Range("A2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:I2").Style.Font.SetBold();

                //Headers
                sht.Range("A3").Value = "DATE";
                sht.Range("B3").Value = "EMP NAME";
                sht.Range("C3").Value = "STOCK NO.";
                sht.Range("D3").Value = "STYLE NAME";
                sht.Range("E3").Value = "COLOUR NAME";
                sht.Range("F3").Value = "SIZE";
                sht.Range("G3").Value = "QTY";
                sht.Range("H3").Value = "REMARK";
                sht.Range("I3").Value = "SCAN BY";

                sht.Range("A3:K3").Style.Font.Bold = true;

                row = 4;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Receivedate"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Tstockno"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["designname"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["colorname"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Sizeft"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["qty"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["remark"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["scanby"]);

                    row = row + 1;

                }
                using (var a = sht.Range("A3:I" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                sht.Columns(1, 20).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("QCREPORT(" + DDprocessname.SelectedItem.Text + ")" + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altrpt", "alert('No records found for this combination.')", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void btngetdata_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlCommand cmd = new SqlCommand("PRO_GETQCREPORT_100PERCENT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@companyId", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@processid", DDprocessname.SelectedValue);
            cmd.Parameters.AddWithValue("@fromdate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
            cmd.Parameters.AddWithValue("@CustomerID", DDCustomerCode.SelectedValue);
            if (DDOrderNo.Items.Count > 0)
            {
                cmd.Parameters.AddWithValue("@OrderID", DDOrderNo.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@OrderID", 0);
            }

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);

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

                if (DDprocessname.SelectedItem.Text.Contains("100%"))
                {
                    row = 0;

                    sht.Range("A1:I1").Merge();
                    sht.Range("A1").SetValue("QC REPORT (" + DDprocessname.SelectedItem.Text + ")");
                    sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A2:I2").Merge();
                    sht.Range("A2").SetValue("From :" + txtfromdate.Text + "  To : " + txttodate.Text);
                    sht.Range("A2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A1:I2").Style.Font.SetBold();

                    //Headers
                    sht.Range("A3").Value = "DATE";
                    sht.Range("B3").Value = "EMP NAME";
                    sht.Range("C3").Value = "STOCK NO.";
                    sht.Range("D3").Value = "STYLE NAME";
                    sht.Range("E3").Value = "COLOUR NAME";
                    sht.Range("F3").Value = "SIZE";
                    sht.Range("G3").Value = "ACTUAL SIZE";
                    sht.Range("H3").Value = "QTY";
                    sht.Range("I3").Value = "REMARK";
                    sht.Range("J3").Value = "SCAN BY";
                    

                    sht.Range("A3:K3").Style.Font.Bold = true;

                    row = 4;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Receivedate"]);
                        sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                        sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Tstockno"]);
                        sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["designname"]);
                        sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["colorname"]);
                        sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Sizeft"]);
                        sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["ActualSize"]);
                        sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["qty"]);
                        sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["remark"]);
                        sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["scanby"]);

                        row = row + 1;
                    }
                    using (var a = sht.Range("A3:J" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                }
                else
                {
                    row = 0;

                    sht.Range("A1:N1").Merge();
                    sht.Range("A1").SetValue("QC REPORT (" + DDprocessname.SelectedItem.Text + ")");
                    sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A2:N2").Merge();
                    sht.Range("A2").SetValue("From :" + txtfromdate.Text + "  To : " + txttodate.Text);
                    sht.Range("A2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A1:N2").Style.Font.SetBold();

                    //Headers
                    sht.Range("A3").Value = "DATE";
                    sht.Range("B3").Value = "EMP NAME";
                    sht.Range("C3").Value = "STOCK NO.";
                    sht.Range("D3").Value = "STYLE NAME";
                    sht.Range("E3").Value = "COLOUR NAME";
                    sht.Range("F3").Value = "SIZE";
                    sht.Range("G3").Value = "ACTUAL SIZE";
                    sht.Range("H3").Value = "QTY";
                    sht.Range("I3").Value = "DEFECTS";
                    sht.Range("J3").Value = "RESULT";
                    sht.Range("K3").Value = "REMARK";
                    sht.Range("L3").Value = "SCAN BY";
                    sht.Range("M3").Value = "CUSTOMER CODE";
                    sht.Range("N3").Value = "ORDER NO";

                    sht.Range("A3:N3").Style.Font.Bold = true;

                    row = 4;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Receivedate"]);
                        sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                        sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Tstockno"]);
                        sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["designname"]);
                        sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["colorname"]);
                        sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Sizeft"]);
                        sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["ActualSize"]);
                        sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["qty"]);
                        sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["defect"]);
                        sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["defect"].ToString() == "" ? "PASS" : "FAIL");
                        sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["remark"]);
                        sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["scanby"]);
                        sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                        sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);

                        row = row + 1;
                    }
                    using (var a = sht.Range("A3:N" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    sht.Columns(1, 20).AdjustToContents();
                }
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("QCREPORT(" + DDprocessname.SelectedItem.Text + ")" + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altrpt", "alert('No records found for this combination.')", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
}