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

public partial class Masters_ReportForms_FrmDepartmentDetailReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                        From Companyinfo CI(nolock)
                        JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @"  
                        Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName 
                        Select Distinct a.DepartmentID, D.DepartmentName 
                        From ProcessIssueToDepartmentMaster a(Nolock) 
                        JOIN Department D(Nolock) ON D.DepartmentId = a.DepartmentID 
                        Where a.MastercompanyId = " + Session["varMasterCompanyIDForERP"] + @" 
                        Order By D.DepartmentName 
                        Select Distinct CI.CustomerId, CI.CustomerCode 
                        From ProcessIssueToDepartmentMaster a(Nolock) 
                        JOIN ProcessIssueToDepartmentDetail b(Nolock) ON b.IssueOrderID = a.IssueOrderID 
                        JOIN OrderMaster OM(Nolock) ON OM.OrderID = b.OrderID 
                        JOIN CustomerInfo CI(Nolock) ON CI.CustomerId = OM.CustomerId 
                        Where a.CompanyID = " + Session["CurrentWorkingCompanyID"] + @" And a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @" 
                        Order By CI.CustomerCode ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDDepartmentName, ds, 1, true, "---Plz Select---");
            UtilityModule.ConditionalComboFillWithDS(ref DDCustCode, ds, 2, true, "---Plz Select---");
            txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
            RDDepartmentRawIssueDetail.Checked = true;
        }
    }
    protected void DDCustCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = @"Select Distinct OM.OrderID, OM.CustomerOrderNo 
                    From ProcessIssueToDepartmentMaster a(Nolock) 
                    JOIN ProcessIssueToDepartmentDetail b(Nolock) ON b.IssueOrderID = a.IssueOrderID 
                    JOIN OrderMaster OM(Nolock) ON OM.OrderID = b.OrderID And OM.CustomerID = " + DDCustCode.SelectedValue + @" 
                    Where a.CompanyID = " + DDCompany.SelectedValue + @" And a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @" 
                    Order By OM.CustomerOrderNo";
        Str = Str + @" Select Distinct a.DepartmentID, D.DepartmentName 
                    From ProcessIssueToDepartmentMaster a(Nolock)";
        if (DDOrderNo.SelectedIndex > 0)
        {
            Str = Str + @" JOIN ProcessIssueToDepartmentDetail b(Nolock) ON b.IssueOrderID = a.IssueOrderID And b.OrderID = " + DDOrderNo.SelectedValue + "";
        }

        Str = Str + @" JOIN Department D(Nolock) ON D.DepartmentId = a.DepartmentID 
                    Where a.CompanyID = " + DDCompany.SelectedValue + @" And a.MastercompanyId = " + Session["varMasterCompanyIDForERP"] + @" 
                    Order By D.DepartmentName ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref DDOrderNo, ds, 0, true, "--Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDDepartmentName, ds, 1, true, "---Plz Select---");
    }
    protected void DDDepartmentName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillIssueNo();
    }

    protected void FillIssueNo()
    {
        string str = "";

        str = @"Select Distinct a.IssueOrderID, a.IssueNo 
            From ProcessIssueToDepartmentMaster a(Nolock) 
            JOIN ProcessIssueToDepartmentDetail b(Nolock) ON b.IssueOrderID = a.IssueOrderID ";
        if (DDOrderNo.Items.Count > 0 && DDOrderNo.SelectedIndex > 0)
        {
            str = str + @" And OM.OrderId = " + DDOrderNo.SelectedValue;
        }
        if (DDCustCode.Items.Count > 0 && DDCustCode.SelectedIndex > 0)
        {
            str = str + @"  JOIN OrderMaster OM(Nolock) ON OM.OrderID = b.OrderID And OM.CustomerId = " + DDCustCode.SelectedValue;
        }
        str = str + @" Where a.CompanyID = " + DDCompany.SelectedValue + " And a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"];
        if (DDDepartmentName.Items.Count > 0 && DDDepartmentName.SelectedIndex > 0)
        {
            str = str + @" And a.DepartmentID = " + DDDepartmentName.SelectedValue;
        }
        str = str + @" Order By a.IssueOrderID Desc";
        UtilityModule.ConditionalComboFill(ref DDIssueNo, str, true, "---Plz Select---");
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";

        if (RDDepartmentRawIssueDetail.Checked == true)
        {
            DepartmentRawIssueDetail();
            return;
        }
    }
    protected void DepartmentRawIssueDetail()
    {
        lblmsg.Text = "";
        try
        {
            int ChkDateFlag = 0;
            if (ChkselectDate.Checked == true)
            {
                ChkDateFlag = 1;
            }
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetDepartmentRawMaterialDetailReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@CustomerID", DDCustCode.SelectedIndex > 0 ? DDCustCode.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@OrderID", DDOrderNo.SelectedIndex > 0 ? DDOrderNo.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@DepartmentID", DDDepartmentName.SelectedIndex > 0 ? DDDepartmentName.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@IssueOrderID", DDIssueNo.SelectedIndex > 0 ? DDIssueNo.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@ChkDateFlag", ChkDateFlag);
            cmd.Parameters.AddWithValue("@FromDate", txtfromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", txttodate.Text);
            cmd.Parameters.AddWithValue("@TranType", 0);
            cmd.Parameters.AddWithValue("@ReportType", 1);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            con.Close();
            con.Dispose();
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

                sht.Range("A1").Value = "DEPARTMENT RAWMATERIAL REPORT";
                sht.Range("A1:L1").Style.Font.FontName = "Calibri";
                sht.Range("A1:L1").Style.Font.Bold = true;
                sht.Range("A1:L1").Style.Font.FontSize = 12;
                sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:L1").Merge();

                //*******Header
                sht.Range("A2").Value = "ISSUE DATE";
                sht.Range("B2").Value = "ISSUE CHALLAN NO";
                sht.Range("C2").Value = "BUYER CODE";
                sht.Range("D2").Value = "BUYER ORDER NO";
                sht.Range("E2").Value = "FOLIO NO";
                sht.Range("F2").Value = "YARN ITEM NAME";
                sht.Range("G2").Value = "YARN QUALITY";
                sht.Range("H2").Value = "SHADE COLOR";
                sht.Range("I2").Value = "LOT NO";
                sht.Range("J2").Value = "TAG NO";
                sht.Range("K2").Value = "CONE TYPE";
                sht.Range("L2").Value = "ISSUE QTY";

                sht.Range("A2:L2").Style.Font.FontName = "Calibri";
                sht.Range("A2:L2").Style.Font.FontSize = 11;
                sht.Range("A2:L2").Style.Font.Bold = true;

                row = 3;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":L" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":L" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Date"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["IssueChallanNo"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["BuyerCode"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["BuyerOrderNo"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["FolioNo"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ItemName"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["LotNo"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["TagNo"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["CONETYPE"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);

                    row = row + 1;
                }

                //*************
                sht.Columns(1, 30).AdjustToContents();

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "L")))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("DepartmentRawMaterialReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            lblmsg.Text = ex.Message;
        }
    }
}