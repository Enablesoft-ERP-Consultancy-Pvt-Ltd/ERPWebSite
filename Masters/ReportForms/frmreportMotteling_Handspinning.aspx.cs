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
public partial class Masters_ReportForms_frmreportMotteling_Handspinning : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.COmpanyid And CA.Userid=" + Session["varuserid"] + @"
                           SELECT PROCESS_NAME_ID,PROCESS_NAME FROM PROCESS_NAME_MASTER WHERE PROCESS_NAME IN('YARN OPENING+MOTTELING','HAND SPINNING','MOTTELING', 'HANK MAKING') ORDER BY PROCESS_NAME 
                           SELECT ICM.CATEGORY_ID,ICM.CATEGORY_NAME FROM ITEM_CATEGORY_MASTER ICM INNER JOIN CATEGORYSEPARATE CS ON ICM.CATEGORY_ID=CS.CATEGORYID AND CS.ID=1 ORDER BY CATEGORY_NAME
                           select customerid,CustomerCode+'  '+companyname as customer from customerinfo WHere mastercompanyid=" + Session["varcompanyid"] + " order by customer";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessname, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds, 3, true, "--Plz Select--");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        if (RDissue.Checked == true)
        {
            IssueDetail();
        }
        else if (RDReceive.Checked == true)
        {
            ReceiveDetail();
        }
        else if (RDissuerecdetail.Checked == true)
        {
            Issue_ReceiveDetail();
        }
    }
    protected void IssueDetail()
    {
        lblmsg.Text = "";
        try
        {
            string Where = "";
            if (DDcustcode.SelectedIndex > 0)
            {
                Where = Where + " and customerid=" + DDcustcode.SelectedValue;
            }
            if (DDorderno.SelectedIndex > 0)
            {
                Where = Where + " and orderid=" + DDorderno.SelectedValue;
            }
            if (DDPartyname.SelectedIndex > 0)
            {
                Where = Where + " and EI.empid=" + DDPartyname.SelectedValue;
            }
            if (Ddemplocation.SelectedIndex > 0)
            {
                Where = Where + " and EI.EmployeeType=" + Ddemplocation.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Where = Where + " and vf.Item_id=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                Where = Where + " and vf.Qualityid=" + DDQuality.SelectedValue;
            }
            if (DDShadecolor.SelectedIndex > 0)
            {
                Where = Where + " and vf.ShadecolorId=" + DDShadecolor.SelectedValue;
            }

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@companyid", DDcompany.SelectedValue);
            param[1] = new SqlParameter("@Process_Name", DDProcessname.SelectedItem.Text);
            param[2] = new SqlParameter("@Fromdate", txtfromdate.Text);
            param[3] = new SqlParameter("@ToDate", txttodate.Text);
            param[4] = new SqlParameter("@Dateflag", chkdate.Checked == true ? "1" : "0");
            param[5] = new SqlParameter("@Where", Where);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_MOTTELING_HANDSPINNING_ISSUEREPORT", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\rptmotteling_handspinningissuedetail.rpt";
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptmotteling_handspinningissuedetail.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No records fetched..');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void ReceiveDetail()
    {
        lblmsg.Text = "";
        try
        {
            string Where = "";
            if (DDcustcode.SelectedIndex > 0)
            {
                Where = Where + " and OM.customerid=" + DDcustcode.SelectedValue;
            }
            if (DDorderno.SelectedIndex > 0)
            {
                Where = Where + " and OM.orderid=" + DDorderno.SelectedValue;
            }
            if (DDPartyname.SelectedIndex > 0)
            {
                Where = Where + " and EI.empid=" + DDPartyname.SelectedValue;
            }
            if (Ddemplocation.SelectedIndex > 0)
            {
                Where = Where + " and EI.EmployeeType=" + Ddemplocation.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Where = Where + " and vf.Item_id=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                Where = Where + " and vf.Qualityid=" + DDQuality.SelectedValue;
            }
            if (DDShadecolor.SelectedIndex > 0)
            {
                Where = Where + " and vf.ShadecolorId=" + DDShadecolor.SelectedValue;
            }
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@companyid", DDcompany.SelectedValue);
            param[1] = new SqlParameter("@Process_Name", DDProcessname.SelectedItem.Text);
            param[2] = new SqlParameter("@Fromdate", txtfromdate.Text);
            param[3] = new SqlParameter("@ToDate", txttodate.Text);
            param[4] = new SqlParameter("@Dateflag", chkdate.Checked == true ? "1" : "0");
            param[5] = new SqlParameter("@Where", Where);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_MOTTELING_HANDSPINNING_RECEIVEREPORT", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Session["VarCompanyNo"].ToString() == "36")
                {
                    Session["rptFileName"] = "~\\Reports\\rptmotteling_handspinningReceivedetailWithRateAmount.rpt";
                }
                else if (Session["VarCompanyNo"].ToString() == "46")
                {
                    Session["rptFileName"] = "~\\Reports\\rptmotteling_handspinningReceivedetailWithRateAmountNeman.rpt";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\rptmotteling_handspinningReceivedetail.rpt";
                }
               
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptmotteling_handspinningReceivedetail.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No records fetched..');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void Issue_ReceiveDetail()
    {
        lblmsg.Text = "";
        try
        {
            string Where = "", Filterby = "";
            if (DDcustcode.SelectedIndex > 0)
            {
                Where = Where + " and customerid=" + DDcustcode.SelectedValue;
                Filterby = Filterby + " CustomerName = " + DDcustcode.SelectedItem.Text + "";
            }
            if (DDorderno.SelectedIndex > 0)
            {
                Where = Where + " and orderid=" + DDorderno.SelectedValue;
                Filterby = Filterby + " OrderNo = " + DDorderno.SelectedItem.Text + "";
            }
            if (DDPartyname.SelectedIndex > 0)
            {
                Where = Where + " and EI.empid=" + DDPartyname.SelectedValue;
                Filterby = Filterby + " EMPNAME = " + DDPartyname.SelectedItem.Text + "";
            }
            if (Ddemplocation.SelectedIndex > 0)
            {
                Where = Where + " and EI.EmployeeType=" + Ddemplocation.SelectedValue;
                Filterby = Filterby + " EMPLOYEE TYPE = " + Ddemplocation.SelectedItem.Text + "";
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Where = Where + " and vf.Item_id=" + ddItemName.SelectedValue;
                Filterby = Filterby + " ITEM NAME = " + ddItemName.SelectedItem.Text + "";
            }
            if (DDQuality.SelectedIndex > 0)
            {
                Where = Where + " and vf.Qualityid=" + DDQuality.SelectedValue;
                Filterby = Filterby + " QUALITY NAME = " + DDQuality.SelectedItem.Text + "";
            }
            if (DDShadecolor.SelectedIndex > 0)
            {
                Where = Where + " and vf.ShadecolorId=" + DDShadecolor.SelectedValue;
                Filterby = Filterby + " SHADECOLOR = " + DDShadecolor.SelectedItem.Text + "";
            }
            if (chkdate.Checked == true)
            {
                Filterby = Filterby + " FROM DATE> = " + txtfromdate.Text + " and TODATE<=" + txttodate.Text + "";
            }
            string Status = "";

            if (DDStatus.SelectedIndex > 0)
            {
                Status = DDStatus.SelectedItem.Text;
            }

            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@companyid", DDcompany.SelectedValue);
            param[1] = new SqlParameter("@Process_Name", DDProcessname.SelectedItem.Text);
            param[2] = new SqlParameter("@Fromdate", txtfromdate.Text);
            param[3] = new SqlParameter("@ToDate", txttodate.Text);
            param[4] = new SqlParameter("@Dateflag", chkdate.Checked == true ? "1" : "0");
            param[5] = new SqlParameter("@Where", Where);
            param[6] = new SqlParameter("@Status", Status);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_MOTTELING_HANDSPINNING_ISSUERECEIVEREPORT", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (chkexcelexport.Checked == true)
                {
                    if (Session["VarCompanyNo"].ToString() == "21")
                    {
                        IssueReceivedetail_ExcelKaysons(ds, Filterby);
                    }
                    else
                    {
                        IssueReceivedetail_Excel(ds, Filterby);
                    }
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\rptmotteling_handspinningissueReceivedetail.rpt";
                    Session["Getdataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\rptmotteling_handspinningissueReceivedetail.xsd";
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No records fetched..');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void IssueReceivedetail_Excel(DataSet ds, string Filterby = "")
    {
        if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
        }
        string Path = "";
        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("Issue Receive Detail");
        //**************
        sht.Range("A1:M1").Merge();
        sht.Range("A1:M1").Style.Font.FontSize = 11;
        sht.Range("A1:M1").Style.Font.Bold = true;
        sht.Range("A1:M1").Style.NumberFormat.Format = "@";
        sht.Range("A1:M1").Style.Alignment.WrapText = true;
        sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A1:M1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("A1").Value = DDcompany.SelectedItem.Text + " (" + DDProcessname.SelectedItem.Text + " ISSUE RECEIVE REPORT) " + Filterby;
        sht.Row(1).Height = 24.00;
        //***
        sht.Range("A2:M2").Style.Font.Bold = true;
        sht.Range("F2:M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

        sht.Range("A2").Value = "Party Name";
        sht.Range("B2").Value = "Issue No.";
        sht.Range("C2").Value = "Issue Date";
        sht.Range("D2").Value = "Item Name";
        sht.Range("E2").Value = "Color Name";
        sht.Range("F2").Value = "Issue Qty";
        sht.Range("G2").Value = "Rec Qty";
        sht.Range("H2").Value = "Return Qty";
        sht.Range("I2").Value = "Loss Qty";
        sht.Range("J2").Value = "Pend. Qty";
        sht.Range("K2").Value = "Cone Type";
        sht.Range("L2").Value = "Ply Type";
        sht.Range("M2").Value = "Transport Type";

        DataView dv = ds.Tables[0].DefaultView;
        dv.Sort = "issuedate,Id";
        DataSet ds1 = new DataSet();
        ds1.Tables.Add(dv.ToTable());
        int row = 3;
        Decimal Bal = 0;
        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            using (var a = sht.Range("A" + row + ":M" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            sht.Range("F" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("F" + row + ":J" + row).Style.NumberFormat.Format = "#,##0.000";


            sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Empname"]);
            sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["issueno"]);
            sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["issuedate"]);
            sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["ItemName"].ToString());
            sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Shadecolorname"]);
            sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["issueqty"]);
            sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["recqty"]);
            sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["retqty"]);
            sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["lossqty"]);
            Bal = Convert.ToDecimal(ds1.Tables[0].Rows[i]["issueqty"]) - (Convert.ToDecimal(ds1.Tables[0].Rows[i]["recqty"]) + Convert.ToDecimal(ds1.Tables[0].Rows[i]["retqty"]) + Convert.ToDecimal(ds1.Tables[0].Rows[i]["lossqty"]));
            sht.Range("J" + row).SetValue(Bal);
            sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["ConeType"]);
            sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["PlyType"]);
            sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["TransportType"]);
            row = row + 1;
        }
        using (var a = sht.Range("F" + row + ":M" + row))
        {
            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        }
        //********Grand Total
        var issued = sht.Evaluate("SUM(F3:F" + (row - 1) + ")");
        var Received = sht.Evaluate("SUM(G3:G" + (row - 1) + ")");
        var Balanced = sht.Evaluate("SUM(J3:J" + (row - 1) + ")");
        var Lossqty = sht.Evaluate("SUM(I3:I" + (row - 1) + ")");
        var Retnqty = sht.Evaluate("SUM(H3:H" + (row - 1) + ")");


        sht.Range("F" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        sht.Range("F" + row + ":J" + row).Style.Font.Bold = true;
        sht.Range("F" + row + ":J" + row).Style.NumberFormat.Format = "#,##0.000";

        sht.Range("F" + row).Value = issued;
        sht.Range("G" + row).Value = Received;
        sht.Range("H" + row).Value = Retnqty;
        sht.Range("I" + row).Value = Lossqty;
        sht.Range("J" + row).Value = Balanced;

        //********************
        sht.Columns(1, 30).AdjustToContents();
        //********************
        string Fileextension = "xlsx";
        string filename = UtilityModule.validateFilename("" + DDProcessname.SelectedItem.Text + "_Issue Receive" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
        Path = Server.MapPath("~/Tempexcel/" + filename);
        xapp.SaveAs(Path);
        xapp.Dispose();
        //Download File
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Clear();
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        Response.WriteFile(Path);
        // File.Delete(Path);
        Response.End();

    }
    protected void DDProcessname_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        switch (DDProcessname.SelectedItem.Text.ToUpper())
        {
            case "HAND SPINNING":
                str = @"SELECT Distinct EI.EMPID,EI.EMPNAME+CASE WHEN ISNULL(EI.EMPCODE,'')<>'' THEN ' ['+EI.EMPCODE+']'  ELSE '' END EMPNAME FROM HANDSPINNINGISSUEMASTER HIM INNER JOIN EMPINFO EI ON HIM.EMPID=EI.EMPID 
                       Where HIm.Processid=" + DDProcessname.SelectedValue + " order by EmpName";
                break;
            default:
                str = @"SELECT Distinct EI.EMPID,EI.EMPNAME+CASE WHEN ISNULL(EI.EMPCODE,'')<>'' THEN ' ['+EI.EMPCODE+']'  ELSE '' END EMPNAME 
                       FROM MOTTELINGISSUEMASTER MIM INNER JOIN EMPINFO EI ON MIM.EMPID=EI.EMPID WHERE MIM.processid=" + DDProcessname.SelectedValue + " order by EmpName";
                break;
        }
        UtilityModule.ConditionalComboFill(ref DDPartyname, str, true, "--Plz Select--");
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddItemName, "SELECT ITEM_ID,ITEM_NAME FROM ITEM_MASTER WHERE CATEGORY_ID=" + DDCategory.SelectedValue + " order by ITEM_NAME", true, "--Plz Select--");
        Fillshadecolor();
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + ddItemName.SelectedValue + " ORDER BY QUALITYNAME", true, "--Plz Select--");
        Fillshadecolor();
    }
    protected void Fillshadecolor()
    {
        string str = @"select Distinct ShadecolorId,ShadeColorName from V_finisheditemdetail Where ShadecolorId>0 and ShadeColorName<>''";
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and QualityId=" + DDQuality.SelectedValue;
        }
        str = str + " order by ShadeColorName";
        UtilityModule.ConditionalComboFill(ref DDShadecolor, str, true, "--Plz Select--");
    }

    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillshadecolor();
    }
    protected void DDcustcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDorderno, @"select orderid,LocalOrder+' '+CustomerOrderNo as orderno from ordermaster where CompanyId=" + DDcompany.SelectedValue + " and customerid=" + DDcustcode.SelectedValue + @"
                                            and status=0 order by orderno", true, "--Plz Select--");
    }
    protected void IssueReceivedetail_ExcelKaysons(DataSet ds, string Filterby = "")
    {
        if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
        }
        string Path = "";
        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("Issue Receive Detail");
        //**************
        sht.Range("A1:L1").Merge();
        sht.Range("A1:L1").Style.Font.FontSize = 11;
        sht.Range("A1:L1").Style.Font.Bold = true;
        sht.Range("A1:L1").Style.NumberFormat.Format = "@";
        sht.Range("A1:L1").Style.Alignment.WrapText = true;
        sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A1:L1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("A1").Value = DDcompany.SelectedItem.Text + " (" + DDProcessname.SelectedItem.Text + " ISSUE RECEIVE REPORT) " + Filterby;
        sht.Row(1).Height = 24.00;
        //***
        sht.Range("A2:L2").Style.Font.Bold = true;
        sht.Range("H2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

        sht.Range("A2").Value = "Party Name";
        sht.Range("B2").Value = "Issue No.";
        sht.Range("C2").Value = "Issue Date";
        sht.Range("D2").Value = "Item Name";
        sht.Range("E2").Value = "Color Name";

        sht.Range("F2").Value = "RecLot No";
        sht.Range("G2").Value = "RecTag No";

        sht.Range("H2").Value = "Issue Qty";
        sht.Range("I2").Value = "Rec Qty";
        sht.Range("J2").Value = "Return Qty";
        sht.Range("K2").Value = "Loss Qty";
        sht.Range("L2").Value = "Pend. Qty";

        DataView dv = ds.Tables[0].DefaultView;
        dv.Sort = "issuedate,Id";
        DataSet ds1 = new DataSet();
        ds1.Tables.Add(dv.ToTable());
        int row = 3;
        Decimal Bal = 0;
        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            using (var a = sht.Range("A" + row + ":L" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            sht.Range("H" + row + ":L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("H" + row + ":L" + row).Style.NumberFormat.Format = "#,##0.000";


            sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Empname"]);
            sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["issueno"]);
            sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["issuedate"]);
            sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["ItemName"].ToString());
            sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Shadecolorname"]);
            sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["RecLotNo"]);
            sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["RecTagNo"]);


            sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["issueqty"]);
            sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["recqty"]);
            sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["retqty"]);
            sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["lossqty"]);
            Bal = Convert.ToDecimal(ds1.Tables[0].Rows[i]["issueqty"]) - (Convert.ToDecimal(ds1.Tables[0].Rows[i]["recqty"]) + Convert.ToDecimal(ds1.Tables[0].Rows[i]["retqty"]) + Convert.ToDecimal(ds1.Tables[0].Rows[i]["lossqty"]));
            sht.Range("L" + row).SetValue(Bal);

            row = row + 1;
        }
        using (var a = sht.Range("H" + row + ":L" + row))
        {
            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        }
        //********Grand Total
        var issued = sht.Evaluate("SUM(H3:H" + (row - 1) + ")");
        var Received = sht.Evaluate("SUM(I3:I" + (row - 1) + ")");
        var Balanced = sht.Evaluate("SUM(L3:L" + (row - 1) + ")");
        var Lossqty = sht.Evaluate("SUM(K3:K" + (row - 1) + ")");
        var Retnqty = sht.Evaluate("SUM(J3:J" + (row - 1) + ")");


        sht.Range("H" + row + ":L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        sht.Range("H" + row + ":L" + row).Style.Font.Bold = true;
        sht.Range("H" + row + ":L" + row).Style.NumberFormat.Format = "#,##0.000";

        sht.Range("H" + row).Value = issued;
        sht.Range("I" + row).Value = Received;
        sht.Range("J" + row).Value = Retnqty;
        sht.Range("K" + row).Value = Lossqty;
        sht.Range("L" + row).Value = Balanced;



        //********************
        sht.Columns(1, 30).AdjustToContents();
        //********************


        string Fileextension = "xlsx";
        string filename = UtilityModule.validateFilename("" + DDProcessname.SelectedItem.Text + "_Issue Receive" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
        Path = Server.MapPath("~/Tempexcel/" + filename);
        xapp.SaveAs(Path);
        xapp.Dispose();
        //Download File
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Clear();
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        Response.WriteFile(Path);
        // File.Delete(Path);
        Response.End();

    }
}