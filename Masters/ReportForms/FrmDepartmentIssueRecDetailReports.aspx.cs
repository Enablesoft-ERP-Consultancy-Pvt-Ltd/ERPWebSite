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

public partial class Masters_ReportForms_FrmDepartmentIssueRecDetailReports : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {

            string Str = @"Select Distinct CI.CompanyId, CI.Companyname 
            From Companyinfo CI(Nolock)
            JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
            Where CI.MastercompanyId = " + Session["varCompanyId"] + @" Order by CI.Companyname 
            Select Distinct a.DepartmentID, D.DepartmentName 
            From ProcessIssueToDepartmentMaster a(Nolock) 
            JOIN Department D(Nolock) ON D.DepartmentId = a.DepartmentID 
            Where a.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And a.MasterCompanyID = " + Session["varCompanyId"] + @" Order By D.DepartmentName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDDepartmentName, ds, 1, true, " Select ");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                CompanySelectedChange();
            }
            imgLogo.ImageUrl.DefaultIfEmpty();
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varCompanyId"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
            LblCompanyName.Text = Session["varCompanyName"].ToString();
            LblUserName.Text = Session["varusername"].ToString();
            TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            RDDepartmentIssueRecDetail.Checked = true;
        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedChange();
    }
    private void CompanySelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDCustCode, "select CustomerId,CustomerCode From CustomerInfo Where MasterCompanyId=" + Session["varCompanyId"] + " Order By CustomerCode", true, "--Select--");
    }

    protected void DDCustCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillorderno();
    }
    protected void fillorderno()
    {
        string str = @"Select Distinct OM.OrderId, OM.LocalOrder+ ' / ' + OM.CustomerOrderNo 
            From ProcessIssueToDepartmentMaster a(Nolock) 
            JOIN ProcessIssueToDepartmentDetail b(Nolock) ON b.IssueOrderID = a.IssueOrderID 
            JOIN OrderMaster OM(Nolock) ON OM.OrderId = b.OrderID And OM.CustomerId = " + DDCustCode.SelectedValue + @" 
            Where a.CompanyID = " + DDCompany.SelectedValue + " And a.MasterCompanyID = " + Session["varCompanyId"] + " Order By OM.OrderId Desc ";

        UtilityModule.ConditionalComboFill(ref DDOrderNo, str, true, "--Select--");
    }

    protected void DDDepartmentName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillIssueNo();
    }
    private void FillIssueNo()
    {
        string Str = @"Select Distinct a.IssueOrderID, a.IssueNo 
        From ProcessIssueToDepartmentMaster a(Nolock) 
        JOIN ProcessIssueToDepartmentDetail b(Nolock) ON b.IssueOrderID = a.IssueOrderID";
        if (DDOrderNo.Items.Count > 0 && DDOrderNo.SelectedIndex > 0)
        {
            Str = Str + " And b.OrderID = " + DDOrderNo.SelectedValue;
        }
        if (DDCustCode.Items.Count > 0 && DDCustCode.SelectedIndex > 0)
        {
            Str = Str + " JOIN OrderMaster OM(Nolock) ON OM.OrderId = b.OrderID And OM.CustomerId = " + DDCustCode.SelectedValue;
        }

        if (DDDepartmentName.Items.Count > 0 && DDDepartmentName.SelectedIndex > 0)
        {
            Str = Str + " And a.DepartmentID = " + DDDepartmentName.SelectedValue;
        }
        Str = Str + " Where a.CompanyID = " + DDCompany.SelectedValue + " And a.MasterCompanyID = " + Session["varCompanyId"];

        Str = Str + " Order By a.IssueOrderID Desc ";
        UtilityModule.ConditionalComboFill(ref DDIssueNo, Str, true, "--Select--");
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        if (RDDepartmentIssueRecDetail.Checked == true)
        {
            DepartmentIssRecDetail();
        }
        else if (RDDepartmentRawMaterialIssueReport.Checked == true)
        {
            DepartmentRawMaterialIssueReport();
            return;
        }
    }
    protected void DepartmentRawMaterialIssueReport()
    {
        lblMessage.Text = "";
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetDepartmentRawMaterialIssueReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@CustomerID", DDCustCode.SelectedValue);
            cmd.Parameters.AddWithValue("@OrderID", DDOrderNo.SelectedValue);
            cmd.Parameters.AddWithValue("@DepartmentID", DDDepartmentName.SelectedValue);
            cmd.Parameters.AddWithValue("@IssueOrderID", DDIssueNo.SelectedValue);
            cmd.Parameters.AddWithValue("@DateFlag", ChkselectDate.Checked == true ? 1 : 0);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);

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

                //*******Header
                sht.Range("A1").Value = "Issue Challan Date";
                sht.Range("B1").Value = "Issue Challan No";
                sht.Range("C1").Value = "Buyer Code";
                sht.Range("D1").Value = "Buyer Order No";
                sht.Range("E1").Value = "Folio No";
                sht.Range("F1").Value = "Yarn Quality";
                sht.Range("G1").Value = "Shade No";
                sht.Range("H1").Value = "Physical Form of Yarn";
                sht.Range("I1").Value = "Issued Qty";

                sht.Range("A1:M1").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:M1").Style.Font.FontSize = 10;
                sht.Range("A1:M1").Style.Font.Bold = true;
                //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 2;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":I" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":I" + row).Style.Font.FontSize = 9;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Date"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["ChallanNo"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ProrderId"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["CONETYPE"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["ISSUEQTY"]);

                    row = row + 1;
                }

                //*************
                sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("DepartmentRawMaterialIssuereport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            lblMessage.Text = ex.Message;
        }
    }
    protected void DepartmentIssRecDetail()
    {
        lblMessage.Text = "";
        try
        {
            string str = "", CustomerID = "0", OrderID = "0", DepartmentID = "0", IssueOrderID = "0";

            if (DDCustCode.Items.Count > 0 && DDCustCode.SelectedIndex > 0)
            {
                CustomerID = DDCustCode.SelectedValue;
            }
            if (DDOrderNo.Items.Count > 0 && DDOrderNo.SelectedIndex > 0)
            {
                OrderID = DDOrderNo.SelectedValue;
            }
            if (DDDepartmentName.Items.Count > 0 && DDDepartmentName.SelectedIndex > 0)
            {
                DepartmentID = DDDepartmentName.SelectedValue;
            }
            if (DDIssueNo.Items.Count > 0 && DDIssueNo.SelectedIndex > 0)
            {
                IssueOrderID = DDIssueNo.SelectedValue;
            }

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("Pro_GetDepartmentIssueRecDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@CompanyID", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@OrderID", OrderID);
            cmd.Parameters.AddWithValue("@DepartmentID", DepartmentID);
            cmd.Parameters.AddWithValue("@IssueOrderID", IssueOrderID);
            cmd.Parameters.AddWithValue("@DateFlag", ChkselectDate.Checked == true ? 1 : 0);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************
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
                sht.Range("A1").Value = "Department Issue Receive Detail";
                sht.Range("A1:R1").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:R1").Style.Font.FontSize = 12;
                sht.Range("A1:R1").Style.Font.Bold = true;
                sht.Range("A1:R1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:R1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A1:R1").Merge();

                //*******Header
                sht.Range("A2").Value = "Customer Code";
                sht.Range("B2").Value = "Order No";
                sht.Range("C2").Value = "Department Issue No";
                sht.Range("D2").Value = "Issue Date";
                sht.Range("E2").Value = "Required Date";
                sht.Range("F2").Value = "Process Name";
                sht.Range("G2").Value = "Department Name";
                sht.Range("H2").Value = "Item Name";
                sht.Range("I2").Value = "Quality";
                sht.Range("J2").Value = "Design";
                sht.Range("K2").Value = "Color";
                sht.Range("L2").Value = "Size";
                sht.Range("M2").Value = "Department Order Qty";
                sht.Range("N2").Value = "Department Iss Qty";
                sht.Range("O2").Value = "Department Iss Due Qty";
                sht.Range("P2").Value = "Department Bazar Qty";
                sht.Range("Q2").Value = "Department Bazar Bal";
                sht.Range("R2").Value = "Remark";

                sht.Range("A2:R2").Style.Font.Bold = true;

                row = 3;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":R" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":R" + row).Style.Font.FontSize = 9;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["DepartmentIssueNo"]);
                    sht.Range("D" + row).SetValue(Convert.ToDateTime(ds.Tables[0].Rows[i]["IssueDate"]).ToString("dd-MMM-yyyy"));
                    sht.Range("E" + row).SetValue(Convert.ToDateTime(ds.Tables[0].Rows[i]["RequiredDate"]).ToString("dd-MMM-yyyy"));
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ProcessName"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["DepartmentName"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ItemName"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Size"]);
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["DepartmentOrderQty"]);
                    sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["IssueQty"]);
                    sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["BalToIssDepartmentQty"]);
                    sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["DepartmentRecQty"]);
                    sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["BalToRecDepartmentQty"]);
                    sht.Range("R" + row).SetValue(ds.Tables[0].Rows[i]["Remark"]);

                    row = row + 1;
                }

                //*************
                sht.Columns(1, 18).AdjustToContents();
                //********************
                //***********BOrders
                using (var a = sht.Range("A1" + ":R" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("DepartmentIssRecDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/main.aspx");
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
}