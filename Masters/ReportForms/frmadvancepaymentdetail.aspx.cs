using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using ClosedXML.Excel;
using System.IO;

public partial class Masters_ReportForms_frmadvancepaymentdetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CI.CompanyId,CI.CompanyName From companyinfo  CI inner join Company_Authentication CA on CI.CompanyId=CA.CompanyId ANd CA.UserId=" + Session["varuserid"] + @" order by CompanyId
                           select PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME From process_Name_master PNM inner join UserRightsProcess UR on PNM.PROCESS_NAME_ID=UR.ProcessId and UR.Userid=" + Session["varuserid"] + @"
                           and PNM.processtype=1 order by PNM.PROCESS_NAME
                           SELECT DISTINCT  ICM.CATEGORY_ID,ICM.CATEGORY_NAME FROM ITEM_CATEGORY_MASTER ICM INNER JOIN CATEGORYSEPARATE CS ON ICM.CATEGORY_ID=CS.CATEGORYID AND CS.ID=0 ORDER BY CATEGORY_NAME";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDjobname, ds, 1, true, "--PLz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDcategory, ds, 2, true, "--PLz Select--");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDjobname_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDjobname.SelectedValue == "1")
        {
            chkEmpTransaction.Visible = true;
            chkpaymentdetails.Visible = true;
        }
        else
        {
            chkEmpTransaction.Checked = false;
            chkEmpTransaction.Visible = false;
            chkpaymentdetails.Visible = false;
        }
    }
    protected void chkExportForExcel_CheckedChanged(object sender, EventArgs e)
    {
        if (chkExportForExcel.Checked == true)
        {
            chkEmpTransaction.Checked = false;
            btnPreviewEmpTran.Visible = false;
            btnpreview.Visible = true;
            chkpaymentdetails.Checked = false;
        }
    }
    protected void chkEmpTransaction_CheckedChanged(object sender, EventArgs e)
    {
        if (chkEmpTransaction.Checked == true)
        {
            btnPreviewEmpTran.Visible = true;
            btnpreview.Visible = false;
            chkExportForExcel.Checked = false;
            chkpaymentdetails.Checked = false;
        }
        else
        {
            btnPreviewEmpTran.Visible = false;
            btnpreview.Visible = true;
        }
    }
    protected void chkpaymentdetails_CheckedChanged(object sender, EventArgs e)
    {
        if (chkpaymentdetails.Checked == true)
        {
            chkExportForExcel.Checked = false;
            chkEmpTransaction.Checked = false;
            btnPreviewEmpTran.Visible = true;
            btnpreview.Visible = false;
        }
        else
        {
            btnPreviewEmpTran.Visible = false;
            btnpreview.Visible = true;
        }
    }
    protected void btnPreviewEmpTran_Click(object sender, EventArgs e)
    {
        if (chkEmpTransaction.Checked == true)
        {
            EmpTransactionDetail();
        }
        else if (chkpaymentdetails.Checked == true)
        {
            getpaymentdetails();
            return;
        }
    }

    protected void btnpreview_Click(object sender, EventArgs e)
    {
        if (ChkForAdvanceDetail.Checked == true)
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@companyid", DDcompany.SelectedValue);
            param[1] = new SqlParameter("@PROCESSID", DDjobname.SelectedValue);
            param[2] = new SqlParameter("@Empcode", txtempcode.Text);
            param[3] = new SqlParameter("@FromDate", txtfromdate.Text);
            param[4] = new SqlParameter("@Todate", txttodate.Text);
            //***********
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETADVANCE_DATEWISE", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "Reports/RptAdvanceDetailDateWise.rpt";
                Session["dsFileName"] = "~\\ReportSchema\\RptAdvanceDetailDateWise.xsd";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No records found for this combination.')", true);
            }
            return;
        }
        if (chkExportForExcel.Checked == true)
        {
            txtpwd.Focus();
            Popup(true);
        }
        else
        {
            lblmsg.Text = "";
            try
            {
                string where = "";
                if (DDcategory.SelectedIndex > 0)
                {
                    where = where + " and vf.CATEGORY_ID=" + DDcategory.SelectedValue;
                }
                if (DDItemname.SelectedIndex > 0)
                {
                    where = where + " and vf.ITEM_ID=" + DDItemname.SelectedValue;
                }

                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@companyid", DDcompany.SelectedValue);
                param[1] = new SqlParameter("@PROCESSID", DDjobname.SelectedValue);
                param[2] = new SqlParameter("@Empcode", txtempcode.Text);
                param[3] = new SqlParameter("@FromDate", txtfromdate.Text);
                param[4] = new SqlParameter("@Todate", txttodate.Text);
                param[5] = new SqlParameter("@Where", where);
                //***********
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETADVANCEPAYMENTDETAIL", param);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["dsFileName"] = "~\\ReportSchema\\rptadvancepaymentdetail.xsd";
                    switch (Session["varcompanyId"].ToString())
                    {
                        case "22":
                            Session["rptFileName"] = "Reports/rptadvancepaymentdetail_Diamond.rpt";
                            break;
                        default:
                            Session["rptFileName"] = "Reports/rptadvancepaymentdetail.rpt";
                            break;
                    }

                    Session["GetDataset"] = ds;
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No records found for this combination.')", true);
                }

            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;
            }
        }


    }
    protected void getpaymentdetails()
    {
        lblmsg.Text = "";
        try
        {

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@companyid", DDcompany.SelectedValue);
            param[1] = new SqlParameter("@PROCESSID", DDjobname.SelectedValue);
            param[2] = new SqlParameter("@Empcode", txtempcode.Text);
            param[3] = new SqlParameter("@FromDate", txtfromdate.Text);
            param[4] = new SqlParameter("@Todate", txttodate.Text);


            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETEMPPAYMENTDETAIL", param);
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

                sht.Range("A1:G1").Merge();
                sht.Range("A1").Value = ds.Tables[0].Rows[0]["companyname"];
                sht.Range("A2:G2").Merge();
                sht.Range("A2").Value = "PAYMENT DETAILS ( " + DDjobname.SelectedItem.Text + ")";
                sht.Range("A3:G3").Merge();
                sht.Range("A3").Value = "From:" + txtfromdate.Text + " " + "To:" + txttodate.Text;
                //sht.Range("A2").Value = "Filter By :  " + FilterBy;
                //sht.Row(2).Height = 30;
                sht.Range("A1:G1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A3:G3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:G3").Style.Font.Bold = true;
                //*******Header
                sht.Range("A4").Value = "EMPLOYEE CODE";
                if (variable.VarLOOMBAZARBYPERCENTAGE == "1")
                {
                    sht.Range("B4").Value = "SQ. FT";
                }
                else
                {
                    sht.Range("B4").Value = "SQ.Yd";
                }
                sht.Range("C4").Value = "ADVANCE";
                sht.Range("D4").Value = "USE ADVANCE";
                sht.Range("E4").Value = "TOTAL PAYMENT";
                sht.Range("F4").Value = "PAY PAYMENT";
                sht.Range("G4").Value = "BALANCE PAYMENT";
                sht.Range("A4:G4").Style.Font.Bold = true;
                sht.Range("B4:G4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                row = 5;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["empcode"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["area"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["advance"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["useadvance"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["totalpayment"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["paypayment"]);

                    // sht.Range("G" + row).FormulaA1 = "=(E" + row + '-' +"("+ "$D$" + row + '+' + "$F$" + row+"))";
                    sht.Range("G" + row).FormulaA1 = "=(E" + row + '-' + "$F$" + row + ")";


                    row = row + 1;
                }
                sht.Columns(1, 10).AdjustToContents();
                using (var a = sht.Range("A1" + ":G" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("PAYMENTDETAIL_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No records found for this combination.')", true);
            }


        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void EmpTransactionDetail()
    {
        lblmsg.Text = "";
        try
        {

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@companyid", DDcompany.SelectedValue);
            param[1] = new SqlParameter("@PROCESSID", DDjobname.SelectedValue);
            param[2] = new SqlParameter("@Empcode", txtempcode.Text);
            param[3] = new SqlParameter("@FromDate", txtfromdate.Text);
            param[4] = new SqlParameter("@Todate", txttodate.Text);


            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETWEAVERMONTHLYTRANSACTION", param);

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

                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                sht.PageSetup.AdjustTo(49);
                sht.PageSetup.PaperSize = XLPaperSize.LetterPaper;




                sht.Columns("A").Width = 8.00;
                sht.Columns("B").Width = 20.22;
                sht.Columns("C").Width = 22.22;
                sht.Columns("D").Width = 20.33;
                sht.Columns("E").Width = 20.44;
                sht.Columns("F").Width = 20.33;


                sht.Range("A1:F1").Merge();
                sht.Range("A1").Value = "Diamond Export";
                sht.Range("A2:F2").Merge();
                sht.Range("A2").Value = "Weaver Transaction Detail";
                sht.Range("A3:F3").Merge();
                sht.Range("A3").Value = "From:" + ds.Tables[0].Rows[0]["FROMDATE"] + " " + "To:" + ds.Tables[0].Rows[0]["ToDATE"];
                sht.Range("A4:F4").Merge();
                sht.Range("A4").Value = "Job(Weaving)";
                //sht.Range("A2").Value = "Filter By :  " + FilterBy;
                //sht.Row(2).Height = 30;
                sht.Range("A1:F1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:F4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:F4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:F4").Style.Alignment.SetWrapText();
                sht.Range("A1:F4").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:F4").Style.Font.FontSize = 10;
                sht.Range("A1:F4").Style.Font.Bold = true;
                //*******Header
                sht.Range("A5").Value = "S.No";
                sht.Range("B5").Value = "Employee Code";
                sht.Range("C5").Value = "Employee Name";
                sht.Range("D5").Value = "Bazaar Date";
                sht.Range("E5").Value = "Material IssueDate";
                sht.Range("F5").Value = "Advance PaymentDate";

                sht.Range("A5:F5").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A5:F5").Style.Font.FontSize = 9;
                sht.Range("A5:F5").Style.Font.Bold = true;
                sht.Range("D5:F5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 6;
                int rowfrom = 6;
                int SrNo = 1;

                DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "EMPCODE", "EMPNAME", "RECEIVEDATE");
                DataView dvrecdate = new DataView(dtdistinct);
                dvrecdate.Sort = "empcode,receivedate";
                DataTable dtdistinct1 = dvrecdate.ToTable();
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dtdistinct1);
                DataView dvdate = new DataView(ds.Tables[0]);
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(SrNo + i);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["EMPCODE"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["EMPNAME"]);

                    //DataTable table = ds.Tables[0];

                    //table.DefaultView.RowFilter = "empcode = '" + ds1.Tables[0].Rows[i]["EMPCODE"] + "' and receivedate='" + ds1.Tables[0].Rows[i]["receivedate"] + "' and reporttype=0";
                    //DataRow row1 = table.DefaultView[0].Row;

                    DataRow[] foundRows;
                    foundRows = ds.Tables[0].Select("empcode = '" + ds1.Tables[0].Rows[i]["EMPCODE"] + "' and receivedate='" + ds1.Tables[0].Rows[i]["receivedate"] + "' and reporttype=0");
                    if (foundRows.Length > 0)
                    {
                        sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["receivedate"]);
                    }
                    foundRows = ds.Tables[0].Select("empcode = '" + ds1.Tables[0].Rows[i]["EMPCODE"] + "' and receivedate='" + ds1.Tables[0].Rows[i]["receivedate"] + "' and reporttype=1");
                    if (foundRows.Length > 0)
                    {
                        sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["receivedate"]);
                    }
                    foundRows = ds.Tables[0].Select("empcode = '" + ds1.Tables[0].Rows[i]["EMPCODE"] + "' and receivedate='" + ds1.Tables[0].Rows[i]["receivedate"] + "' and reporttype=2");
                    if (foundRows.Length > 0)
                    {
                        sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["receivedate"]);
                    }


                    row = row + 1;
                }

                //*************
                //sht.Columns(1, 20).AdjustToContents();
                //********************
                //***********BOrders
                using (var a = sht.Range("A1" + ":F" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("EmpTransactionDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                //Download File
                Response.Clear();
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
            lblmsg.Text = ex.Message;
        }


    }
    protected void MonthlyJobPaymentDetail()
    {
        lblmsg.Text = "";
        try
        {
            string where = "";
            if (DDcategory.SelectedIndex > 0)
            {
                where = where + " and vf.CATEGORY_ID=" + DDcategory.SelectedValue;
            }
            if (DDItemname.SelectedIndex > 0)
            {
                where = where + " and vf.ITEM_ID=" + DDItemname.SelectedValue;
            }
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@companyid", DDcompany.SelectedValue);
            param[1] = new SqlParameter("@PROCESSID", DDjobname.SelectedValue);
            param[2] = new SqlParameter("@Empcode", txtempcode.Text);
            param[3] = new SqlParameter("@FromDate", txtfromdate.Text);
            param[4] = new SqlParameter("@Todate", txttodate.Text);
            param[5] = new SqlParameter("@Where", where);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETADVANCEPAYMENTDETAIL", param);

            if (ds.Tables[1].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                sht.PageSetup.AdjustTo(49);
                sht.PageSetup.PaperSize = XLPaperSize.LetterPaper;

                //sht.PageSetup.Margins.Top = 0.21;
                //sht.PageSetup.Margins.Left = 0.47;
                //sht.PageSetup.Margins.Right = 0.36;
                //sht.PageSetup.Margins.Bottom = 0.19;
                //sht.PageSetup.Margins.Header = 0.20;
                //sht.PageSetup.Margins.Footer = 0.3;
                //sht.PageSetup.SetScaleHFWithDocument();
                //************


                sht.Columns("A").Width = 8.00;
                sht.Columns("B").Width = 20.22;
                sht.Columns("C").Width = 22.22;
                sht.Columns("D").Width = 20.33;
                sht.Columns("E").Width = 20.44;
                sht.Columns("F").Width = 20.33;


                sht.Range("A1:F1").Merge();
                sht.Range("A1").Value = ds.Tables[1].Rows[0]["COMPANYNAME"];
                sht.Range("A2:F2").Merge();
                sht.Range("A2").Value = "Advance Payment Detail";
                sht.Range("A3:F3").Merge();
                sht.Range("A3").Value = "From:" + ds.Tables[1].Rows[0]["FROMDATE"] + " " + "To:" + ds.Tables[1].Rows[0]["ToDATE"];
                sht.Range("A4:F4").Merge();
                sht.Range("A4").Value = "Job(" + ds.Tables[1].Rows[0]["JOBNAME"] + ")";
                //sht.Range("A2").Value = "Filter By :  " + FilterBy;
                //sht.Row(2).Height = 30;
                sht.Range("A1:F1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:F4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:F4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:F4").Style.Alignment.SetWrapText();
                sht.Range("A1:F4").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:F4").Style.Font.FontSize = 10;
                sht.Range("A1:F4").Style.Font.Bold = true;
                //*******Header
                sht.Range("A5").Value = "S.No";
                sht.Range("B5").Value = "Employee Code";
                sht.Range("C5").Value = "Employee Name";
                sht.Range("D5").Value = "Total Wages";
                sht.Range("E5").Value = "Total Advance";
                sht.Range("F5").Value = "Balance";

                sht.Range("A5:F5").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A5:F5").Style.Font.FontSize = 9;
                sht.Range("A5:F5").Style.Font.Bold = true;
                sht.Range("D5:F5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 6;
                int rowfrom = 6;
                int SrNo = 1;
                // DataView dv = new DataView(ds.Tables[0]);
                // dv.Sort = "EMPCODE";                
                // DataSet ds1 = new DataSet();
                // ds1.Tables.Add(dv.ToTable());

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {

                    sht.Range("A" + row + ":F" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":F" + row).Style.Font.FontSize = 9;

                    sht.Range("A" + row).SetValue(SrNo + i);
                    sht.Range("B" + row).SetValue(ds.Tables[1].Rows[i]["EMPCODE"]);
                    sht.Range("C" + row).SetValue(ds.Tables[1].Rows[i]["EMPNAME"]);
                    sht.Range("D" + row).SetValue(ds.Tables[1].Rows[i]["TOTALWAGES"]);
                    sht.Range("E" + row).SetValue(ds.Tables[1].Rows[i]["ADVANCEAMOUNT"]);
                    sht.Range("F" + row).SetValue(Convert.ToDecimal(ds.Tables[1].Rows[i]["TOTALWAGES"]) - Convert.ToDecimal(ds.Tables[1].Rows[i]["ADVANCEAMOUNT"]));


                    row = row + 1;
                }
                //GRAND TOAL
                //=SUM(D6:D29)
                sht.Range("D" + row).FormulaA1 = "=SUM(D" + rowfrom + ":D" + (row - 1) + ")";
                sht.Range("E" + row).FormulaA1 = "=SUM(E" + rowfrom + ":E" + (row - 1) + ")";
                sht.Range("F" + row).FormulaA1 = "=SUM(F" + rowfrom + ":F" + (row - 1) + ")";
                sht.Range("D" + row + ":F" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("D" + row + ":F" + row).Style.Font.FontSize = 9;
                sht.Range("D" + row + ":F" + row).Style.Font.Bold = true;

                //*************
                //sht.Columns(1, 20).AdjustToContents();
                //********************
                //***********BOrders
                using (var a = sht.Range("A1" + ":F" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("MonthlyJobPaymentDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                //Download File
                Response.Clear();
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
            lblmsg.Text = ex.Message;
        }

    }
    protected void MonthlyJobPaymentDetailDiamond()
    {
        lblmsg.Text = "";
        try
        {
            string where = "";
            if (DDcategory.SelectedIndex > 0)
            {
                where = where + " and vf.CATEGORY_ID=" + DDcategory.SelectedValue;
            }
            if (DDItemname.SelectedIndex > 0)
            {
                where = where + " and vf.ITEM_ID=" + DDItemname.SelectedValue;
            }
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@companyid", DDcompany.SelectedValue);
            param[1] = new SqlParameter("@PROCESSID", DDjobname.SelectedValue);
            param[2] = new SqlParameter("@Empcode", txtempcode.Text);
            param[3] = new SqlParameter("@FromDate", txtfromdate.Text);
            param[4] = new SqlParameter("@Todate", txttodate.Text);
            param[5] = new SqlParameter("@Where", where);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETADVANCEPAYMENTDETAIL", param);

            if (ds.Tables[1].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                sht.PageSetup.AdjustTo(49);
                sht.PageSetup.PaperSize = XLPaperSize.LetterPaper;

                //sht.PageSetup.Margins.Top = 0.21;
                //sht.PageSetup.Margins.Left = 0.47;
                //sht.PageSetup.Margins.Right = 0.36;
                //sht.PageSetup.Margins.Bottom = 0.19;
                //sht.PageSetup.Margins.Header = 0.20;
                //sht.PageSetup.Margins.Footer = 0.3;
                //sht.PageSetup.SetScaleHFWithDocument();
                //************


                sht.Columns("A").Width = 8.00;
                sht.Columns("B").Width = 20.22;
                sht.Columns("C").Width = 22.22;
                sht.Columns("D").Width = 20.33;
                sht.Columns("E").Width = 20.44;
                sht.Columns("F").Width = 20.33;
                sht.Columns("G").Width = 20.33;
                sht.Columns("H").Width = 20.33;


                sht.Range("A1:H1").Merge();
                sht.Range("A1").Value = ds.Tables[1].Rows[0]["COMPANYNAME"];
                sht.Range("A2:H2").Merge();
                sht.Range("A2").Value = "Advance Payment Detail";
                sht.Range("A3:H3").Merge();
                sht.Range("A3").Value = "From:" + ds.Tables[1].Rows[0]["FROMDATE"] + " " + "To:" + ds.Tables[1].Rows[0]["ToDATE"];
                sht.Range("A4:H4").Merge();
                sht.Range("A4").Value = "Job(" + ds.Tables[1].Rows[0]["JOBNAME"] + ")";
                //sht.Range("A2").Value = "Filter By :  " + FilterBy;
                //sht.Row(2).Height = 30;
                sht.Range("A1:F1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:F4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:F4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:F4").Style.Alignment.SetWrapText();
                sht.Range("A1:F4").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:F4").Style.Font.FontSize = 10;
                sht.Range("A1:F4").Style.Font.Bold = true;
                //*******Header
                sht.Range("A5").Value = "S.No";
                sht.Range("B5").Value = "Employee Code";
                sht.Range("C5").Value = "Employee Name";
                sht.Range("D5").Value = "Total Wages";
                sht.Range("E5").Value = "Advance";
                sht.Range("F5").Value = "Commission Amt";
                sht.Range("G5").Value = "Penality Amt";
                sht.Range("H5").Value = "Balance";

                sht.Range("A5:H5").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A5:H5").Style.Font.FontSize = 9;
                sht.Range("A5:H5").Style.Font.Bold = true;
                sht.Range("D5:H5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 6;
                int rowfrom = 6;
                int SrNo = 1;
                // DataView dv = new DataView(ds.Tables[0]);
                // dv.Sort = "EMPCODE";                
                // DataSet ds1 = new DataSet();
                // ds1.Tables.Add(dv.ToTable());
                decimal balanceamt = 0;
                decimal TotalWages = 0;

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {

                    sht.Range("A" + row + ":H" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":H" + row).Style.Font.FontSize = 9;
                    //sht.Range("F" + row).Style.Fill.BackgroundColor = XLColor.Red;



                    sht.Range("A" + row).SetValue(SrNo + i);
                    sht.Range("B" + row).SetValue(ds.Tables[1].Rows[i]["EMPCODE"]);
                    sht.Range("C" + row).SetValue(ds.Tables[1].Rows[i]["EMPNAME"]);
                    sht.Range("D" + row).SetValue(ds.Tables[1].Rows[i]["TOTALWAGES"]);
                    TotalWages = Convert.ToDecimal(ds.Tables[1].Rows[i]["TOTALWAGES"]);
                    if (TotalWages >= 15500)
                    {
                        sht.Range("D" + row).Style.Fill.BackgroundColor = XLColor.Red;
                    }
                    sht.Range("E" + row).SetValue(ds.Tables[1].Rows[i]["ADVANCEAMOUNT"]);
                    //balanceamt = Convert.ToDecimal(ds.Tables[1].Rows[i]["TOTALWAGES"]) - Convert.ToDecimal(ds.Tables[1].Rows[i]["ADVANCEAMOUNT"]);

                    sht.Range("F" + row).SetValue(Convert.ToDecimal(ds.Tables[1].Rows[i]["TotalCommAmt"]));
                    sht.Range("G" + row).SetValue(Convert.ToDecimal(ds.Tables[1].Rows[i]["TotalPenality"]));

                    sht.Range("H" + row).SetValue(Convert.ToDecimal(ds.Tables[1].Rows[i]["TOTALWAGES"]) + Convert.ToDecimal(ds.Tables[1].Rows[i]["TotalCommAmt"]) - Convert.ToDecimal(ds.Tables[1].Rows[i]["ADVANCEAMOUNT"]) - Convert.ToDecimal(ds.Tables[1].Rows[i]["TotalPenality"]));


                    row = row + 1;
                }
                //GRAND TOAL
                //=SUM(D6:D29)
                sht.Range("D" + row).FormulaA1 = "=SUM(D" + rowfrom + ":D" + (row - 1) + ")";
                sht.Range("E" + row).FormulaA1 = "=SUM(E" + rowfrom + ":E" + (row - 1) + ")";
                sht.Range("F" + row).FormulaA1 = "=SUM(F" + rowfrom + ":F" + (row - 1) + ")";
                sht.Range("G" + row).FormulaA1 = "=SUM(G" + rowfrom + ":G" + (row - 1) + ")";
                sht.Range("H" + row).FormulaA1 = "=SUM(H" + rowfrom + ":H" + (row - 1) + ")";
                sht.Range("D" + row + ":H" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("D" + row + ":H" + row).Style.Font.FontSize = 9;
                sht.Range("D" + row + ":H" + row).Style.Font.Bold = true;

                //*************
                //sht.Columns(1, 20).AdjustToContents();
                //********************
                //***********BOrders
                using (var a = sht.Range("A1" + ":H" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("MonthlyJobPaymentDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();

                //Download File
                Response.Clear();
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
            lblmsg.Text = ex.Message;
        }

    }

    protected void btnCheck_Click(object sender, EventArgs e)
    {
        if (variable.VarAdvancePaymentExportExcelpwd == txtpwd.Text)
        {
            switch (Session["varcompanyId"].ToString())
            {
                case "22":
                    MonthlyJobPaymentDetailDiamond();
                    Popup(false);
                    break;
                default:
                    MonthlyJobPaymentDetail();
                    Popup(false);
                    break;
            }
        }
        else
        {
            lblmsg.Visible = true;
            lblmsg.Text = "Please Enter Correct Password..";
        }
    }
    void Popup(bool isDisplay)
    {
        StringBuilder builder = new StringBuilder();
        if (isDisplay)
        {
            builder.Append("<script>");
            builder.Append("ShowPopup();</script>");
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", builder.ToString());
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ShowPopup", builder.ToString(), false);
        }
        else
        {
            builder.Append("<script>");
            builder.Append("HidePopup();</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HidePopup", builder.ToString(), false);
        }
    }
    protected void DDcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDItemname, "SELECT ITEM_ID,ITEM_NAME FROM ITEM_MASTER WHERE CATEGORY_ID=" + DDcategory.SelectedValue + " order by ITEM_NAME", true, "--Plz Select--");
    }

}