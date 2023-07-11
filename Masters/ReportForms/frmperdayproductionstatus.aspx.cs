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
public partial class Masters_ReportForms_frmperdayproductionstatus : System.Web.UI.Page
{
    public static string Export = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName                           
                           select UnitsId,UnitName from Units order by UnitName
                           select Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=cs.Categoryid and cs.id=0 and ICM.MasterCompanyid=" + Session["varcompanyid"] + @"
                           select Val,Type from Sizetype
                           Select CustomerID, CustomerCode From CustomerInfo Where MasterCompanyid = " + Session["varcompanyid"] + " Order By CustomerCode ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 3, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerOrderNo, ds, 4, true, "--Plz Select--");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            //Export
            switch (variable.ReportWithpdf)
            {
                case "1":
                    chkexport.Visible = true;
                    Export = "N";
                    break;
                default:
                    Export = "Y";
                    chkexport.Visible = false;
                    break;
            }

            switch (Session["varCompanyNo"].ToString())
            {
                case "21":
                    trQCDefects.Visible = true;
                    break;
                default:
                    trQCDefects.Visible = false;
                    break;
            }
        }
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
    }
    private void ddlcategorycange()
    {
        DDQuality.Items.Clear();
        DDDesign.Items.Clear();
        DDColor.Items.Clear();
        DDShape.Items.Clear();
        DDSize.Items.Clear();
        TRDDQuality.Visible = false;
        TRDDDesign.Visible = false;
        TRDDColor.Visible = false;
        TRDDShape.Visible = false;
        TRDDSize.Visible = false;
        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME 
                      FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on 
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        TRDDQuality.Visible = true;
                        break;
                    case "2":
                        TRDDDesign.Visible = true;
                        break;
                    case "3":
                        TRDDColor.Visible = true;
                        break;
                    case "6":
                        //TDIColorShade.Visible = true;
                        break;
                    case "4":
                        TRDDShape.Visible = true;
                        break;
                    case "5":
                        TRDDSize.Visible = true;
                        break;
                    case "10":
                        //TDIcolor.Visible = true;
                        break;
                }
            }
        }

        string stritem = "select distinct IM.Item_Id,IM.Item_Name from  Item_Parameter_Master IPM  inner Join Item_Master IM on IM.Item_Id=IPM.Item_Id inner join Item_Category_Master ICM on ICM.Category_Id=IM.Category_Id where  IM.Category_Id=" + DDCategory.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " order by IM.item_name";
        UtilityModule.ConditionalComboFill(ref ddItemName, stritem, true, "---Select Item----");
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(DDQuality, DDDesign, DDColor, DDShape, Convert.ToInt16(ddItemName.SelectedValue));
    }
    private void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, int Itemid)
    {
        string Str = @"SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + Itemid + " And MasterCompanyId=" + Session["varCompanyId"] + @" Order By QUALITYNAME
                     SELECT DESIGNID,DESIGNNAME from DESIGN Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By DESIGNNAME
                     SELECT COLORID,COLORNAME FROM COLOR Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By COLORNAME
                     SELECT SHAPEID,SHAPENAME FROM SHAPE Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By SHAPENAME
                     SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR Where  MasterCompanyId=" + Session["varCompanyId"] + " Order By SHADECOLORNAME";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref Quality, ds, 0, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Design, ds, 1, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Color, ds, 2, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Shape, ds, 3, true, "--SELECT--");
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void FillSize()
    {
        string size = "";
        string str = "";

        switch (DDsizetype.SelectedValue)
        {
            case "1":
                size = "Sizemtr";
                break;
            case "0":
                size = "Sizeft";
                break;
            case "2":
                size = "Sizeinch";
                break;
            default:
                size = "Sizeft";
                break;
        }
        //size Query

        str = "Select Distinct S.Sizeid,S." + size + " As  " + size + @" From Size S 
                 Where shapeid=" + DDShape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " order by " + size + "";

        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--SELECT--");
        //

    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void DDProdunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDLoomNo, @"select  PM.UID,PM.loomno+'/'+IM.ITEM_NAME as LoomNo from ProductionLoomMaster PM 
                                            inner join ITEM_MASTER IM on PM.Itemid=IM.ITEM_ID                                            
                                            Where  PM.CompanyId=" + DDcompany.SelectedValue + " and PM.UnitId=" + DDProdunit.SelectedValue + " order by case when ISNUMERIC(PM.loomno)=1 Then CONVERT(int,replace(loomno, '.', '')) Else 9999999 End,PM.loomno", true, "--Plz Select--");
    }
    protected void QCDefectReport()
    {
        string str = "", FilterBy = "";
        // str = " Where PRM.companyId=" + DDcompany.SelectedValue;
        str = str + " And PRM.ReceiveDate>='" + txtfromdate.Text + "' and PRM.ReceiveDate<='" + txttodate.Text + "'";
        if (DDProdunit.SelectedIndex > 0)
        {
            str = str + " and PIM.units=" + DDProdunit.SelectedValue;
            FilterBy = FilterBy + " and UnitName -" + DDProdunit.SelectedItem.Text;
        }
        if (DDLoomNo.SelectedIndex > 0)
        {
            str = str + " and PIM.Loomid=" + DDLoomNo.SelectedValue;
            FilterBy = FilterBy + " and LoomNo -" + DDLoomNo.SelectedItem.Text;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf.item_id=" + ddItemName.SelectedValue;
            FilterBy = FilterBy + " and ItemName -" + ddItemName.SelectedItem.Text;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.QualityId=" + DDQuality.SelectedValue;
            FilterBy = FilterBy + " and QualityName -" + DDQuality.SelectedItem.Text;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
            FilterBy = FilterBy + " and DesignName -" + DDDesign.SelectedItem.Text;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and vf.Colorid=" + DDColor.SelectedValue;
            FilterBy = FilterBy + " and colorname -" + DDColor.SelectedItem.Text;
        }
        if (DDShape.SelectedIndex > 0)
        {
            str = str + " and vf.shapeid=" + DDShape.SelectedValue;
            FilterBy = FilterBy + " and ShapeName -" + DDShape.SelectedItem.Text;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
            FilterBy = FilterBy + " and Size -" + DDSize.SelectedItem.Text;
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_PERDAYPRODUCTION_QCDEFECTREPORT", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@CompanyId", DDcompany.SelectedValue);
        cmd.Parameters.AddWithValue("@where", str);
        cmd.Parameters.AddWithValue("@reportype", 0);
        cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
        cmd.Parameters.AddWithValue("@toDate", txttodate.Text);
        cmd.Parameters.AddWithValue("@WithDetail", chkexportwithdetail.Checked == true ? "1" : "0");

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("PerDayProductionDefects_");

            //*************
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:L1").Merge();
            sht.Range("A1:L1").Style.Font.FontSize = 10;
            sht.Range("A1:L1").Style.Font.Bold = true;
            sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:L1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:L1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("Per Day Production Defects From : " + txtfromdate.Text + " To : " + txttodate.Text + " " + FilterBy);

            sht.Range("A2:L2").Style.Font.FontSize = 10;
            sht.Range("A2:L2").Style.Font.Bold = true;

            sht.Range("A2").Value = "Bazar Date";
            sht.Range("B2").Value = "Unit Name";
            sht.Range("C2").Value = "Loom No";
            sht.Range("D2").Value = "Folio No";
            sht.Range("E2").Value = "Quality";
            sht.Range("F2").Value = "Design";
            sht.Range("G2").Value = "Color";
            sht.Range("H2").Value = "Size";
            sht.Range("I2").Value = "Qty";
            sht.Range("J2").Value = "Area";
            sht.Range("K2").Value = "Weight";
            sht.Range("L2").Value = "Defects";

            int row = 3;

            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "Receivedate,issueorderid";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Receivedate"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Unitname"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["FolioChallanNo"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["QualityName"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Designname"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["colorname"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);
                sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Area"]);
                sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Weight"]);
                sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["Defect"]);

                row = row + 1;
            }
            ds.Dispose();
            ds1.Dispose();
            //*************************************************
            using (var a = sht.Range("A1" + ":L" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            //*************************************************
            String Path;
            sht.Columns(1, 28).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PerDayProductionDefects_" + DateTime.Now + "." + Fileextension);
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
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        if (ChkForQcDefects.Checked == true)
        {
            QCDefectReport();
        }
        else
        {
            string str = "", FilterBy = "";
            str = " Where PRM.companyId=" + DDcompany.SelectedValue;
            str = str + " And PRM.ReceiveDate>='" + txtfromdate.Text + "' and PRM.ReceiveDate<='" + txttodate.Text + "'";
            if (DDProdunit.SelectedIndex > 0)
            {
                str = str + " and PIM.units=" + DDProdunit.SelectedValue;
                FilterBy = FilterBy + " and UnitName -" + DDProdunit.SelectedItem.Text;
            }
            if (DDLoomNo.SelectedIndex > 0)
            {
                str = str + " and PIM.Loomid=" + DDLoomNo.SelectedValue;
                FilterBy = FilterBy + " and LoomNo -" + DDLoomNo.SelectedItem.Text;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + " and vf.item_id=" + ddItemName.SelectedValue;
                FilterBy = FilterBy + " and ItemName -" + ddItemName.SelectedItem.Text;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and vf.QualityId=" + DDQuality.SelectedValue;
                FilterBy = FilterBy + " and QualityName -" + DDQuality.SelectedItem.Text;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
                FilterBy = FilterBy + " and DesignName -" + DDDesign.SelectedItem.Text;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " and vf.Colorid=" + DDColor.SelectedValue;
                FilterBy = FilterBy + " and colorname -" + DDColor.SelectedItem.Text;
            }
            if (DDShape.SelectedIndex > 0)
            {
                str = str + " and vf.shapeid=" + DDShape.SelectedValue;
                FilterBy = FilterBy + " and ShapeName -" + DDShape.SelectedItem.Text;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
                FilterBy = FilterBy + " and Size -" + DDSize.SelectedItem.Text;
            }

            ////****************
            //SqlParameter[] arr = new SqlParameter[5];
            //arr[0] = new SqlParameter("@where", str);
            //arr[1] = new SqlParameter("@reportype", 0);
            //arr[2] = new SqlParameter("@fromDate", txtfromdate.Text);
            //arr[3] = new SqlParameter("@toDate", txttodate.Text);
            //arr[4] = new SqlParameter("@WithDetail", chkexportwithdetail.Checked == true ? "1" : "0");

            ////***************
            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_perdayproductionstatus", arr);

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_perdayproductionstatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@where", str);
            cmd.Parameters.AddWithValue("@reportype", 0);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@toDate", txttodate.Text);
            cmd.Parameters.AddWithValue("@WithDetail", chkexportwithdetail.Checked == true ? "1" : "0");
            cmd.Parameters.AddWithValue("@CustomerID", TrCustomerCode.Visible == true ? DDCustomerOrderNo.SelectedIndex <= 0 ? "0" : DDCustomerOrderNo.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@OrderID", TrOrderNo.Visible == true ? DDOrderNo.SelectedIndex <= 0 ? "0" : DDOrderNo.SelectedValue : "0");

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (chkexportwithdetail.Checked == true)
                {
                    if (Session["varCompanyId"].ToString() == "22")
                    {
                        PerdayproductionwithdetailDiamond(ds, FilterBy);
                        return;                        
                    }
                    else if (Session["varCompanyId"].ToString() == "44")
                    {
                        Perdayproductionwithdetailagni(ds, FilterBy);
                        return;
                    }
                    else
                    {
                        Perdayproductionwithdetail(ds, FilterBy);
                        return;
                    }
                }
                if (chksummary.Checked) //New
                {
                    Session["rptFileName"] = "~\\Reports\\rptperdayproductionstatussummarynew.rpt";
                    Session["Getdataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\rptperdayproductionstatussummarynew.xsd";
                }
                else
                {
                    if (Session["varCompanyId"].ToString() == "14")
                    {
                        PerdayproductionSummary(ds, FilterBy);
                        return;
                        // Session["rptFileName"] = "~\\Reports\\rptperdayproductionstatussummaryEHI.rpt";
                    }
                    else if (Session["varCompanyId"].ToString() == "16" || Session["varCompanyId"].ToString() == "28")
                    {
                        Session["rptFileName"] = "~\\Reports\\rptperdayproductionstatussummaryChampo.rpt";
                    }
                    else if (Session["varCompanyId"].ToString() == "44")
                    {
                        Session["rptFileName"] = "~\\Reports\\rptperdayproductionstatussummaryagni.rpt";
                    }
                    else
                    {
                        Session["rptFileName"] = "~\\Reports\\rptperdayproductionstatussummary.rpt";
                    }


                    Session["Getdataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\rptperdayproductionstatussummary.xsd";
                }
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                if (chkexport.Checked == true)
                {
                    Export = "Y";
                }
                else
                {
                    switch (variable.ReportWithpdf)
                    {
                        case "1":
                            Export = "N";
                            break;
                        default:
                            Export = "Y";
                            break;
                    }
                }
                stb.Append("window.open('../../ViewReport.aspx?Export=" + Export + "', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
            }
        }


    }

    private void Perdayproductionwithdetailagni(DataSet ds, String FilterBy)
    {
        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("Perdayproductionstatus_");

        //*************
        //***********
        sht.Row(1).Height = 24;
        sht.Range("A1:X1").Merge();
        sht.Range("A1:X1").Style.Font.FontSize = 10;
        sht.Range("A1:X1").Style.Font.Bold = true;
        sht.Range("A1:X1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A1:X1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("A1:X1").Style.Alignment.WrapText = true;
        //************
        sht.Range("A1").SetValue("Per Day Production From : " + txtfromdate.Text + " To : " + txttodate.Text + " " + FilterBy);

        sht.Range("A2:AA2").Style.Font.FontSize = 10;
        sht.Range("A2:AA2").Style.Font.Bold = true;

        sht.Range("A2").Value = "Bazar Date";
        sht.Range("B2").Value = "Carpet No.";
        sht.Range("C2").Value = "Unit Name";
        sht.Range("D2").Value = "Loom No";
        sht.Range("E2").Value = "Folio No";
        sht.Range("F2").Value = "Quality";
        sht.Range("G2").Value = "Design";
        sht.Range("H2").Value = "Color";
        sht.Range("I2").Value = "Size";
        sht.Range("J2").Value = "Qty";
        sht.Range("K2").Value = "Area";
        sht.Column(4).Hide();
        sht.Range("L2").Value = "Rate";
        sht.Range("M2").Value = "Amount";
        sht.Range("N2").Value = "Weight";
        sht.Range("O2").Value = "Emp Name";
        sht.Range("P2").Value = "Status";

        sht.Range("Q2").Value = "Defects";
        sht.Range("R2").Value = "User Name";
        sht.Range("S2").Value = "Remove Defects";
        sht.Range("T2").Value = "Inspected By";
        sht.Range("U2").Value = "Inspection Date";
        sht.Range("V2").Value = "Penality Amount";
         sht.Range("W2").Value = "Commission Amount";
         sht.Range("X2").Value = "Party ChallanNo";
         sht.Range("Y2").Value = "CustomerOrderNo";

        //if (Session["varCompanyId"].ToString() == "16" || Session["varCompanyId"].ToString() == "28")
        //{
        //    sht.Range("X2").Value = "Party ChallanNo";
        //}
        //else
        //{
        //    sht.Column(24).Hide();
        //}
        //if (Session["varCompanyId"].ToString() == "21")
        //{
            //sht.Range("Y2").Value = "QC Comment";
        //}
        //else
        //{
        //    sht.Column(25).Hide();
        //}

        //if (Session["varCompanyId"].ToString() == "14")
        //{
        //    sht.Range("Z2").Value = "Actual Width";
        //    sht.Range("AA2").Value = "Actual Length";
        //}
        //else
        //{
        //    sht.Column(26).Hide();
        //    sht.Column(27).Hide();
        //}


        int row = 3;

        DataView dv = ds.Tables[0].DefaultView;
        dv.Sort = "Receivedate,issueorderid";
        DataSet ds1 = new DataSet();
        ds1.Tables.Add(dv.ToTable());
        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Receivedate"]);
            sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Tstockno"]);
            sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Unitname"]);
            sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
            sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["FolioChallanNo"]);
            sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["QualityName"]);
            sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Designname"]);
            sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["colorname"]);
            sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
            sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);
            sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Area"]);
            sht.Column(4).Hide();
            sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);
            sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["Amount"]);
            sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["Weight"]);
            sht.Range("O" + row).SetValue(ds1.Tables[0].Rows[i]["Empname"]);
            sht.Range("P" + row).SetValue(ds1.Tables[0].Rows[i]["Stockstatus"]);
            //if (Session["varCompanyId"].ToString() == "21" && Session["usertype"].ToString() != "1")
            //{
            //    sht.Column(12).Hide();
            //    sht.Column(13).Hide();
            //}
            //else
            //{
           
            //}
         
            //if (Session["varCompanyId"].ToString() == "21" && Session["usertype"].ToString() != "1")
            //{
            //    sht.Column(15).Hide();
            //    sht.Range("O" + row).SetValue("");
            //    sht.Column(16).Hide();
            //    sht.Range("P" + row).SetValue("");
            //}
            //else
            //{
            //    sht.Range("O" + row).SetValue(ds1.Tables[0].Rows[i]["Empname"]);
            //    sht.Range("P" + row).SetValue(ds1.Tables[0].Rows[i]["Stockstatus"]);
            //}

            sht.Range("Q" + row).SetValue(ds1.Tables[0].Rows[i]["Defect"]);
            sht.Range("R" + row).SetValue(ds1.Tables[0].Rows[i]["username"]);
            sht.Range("S" + row).SetValue(ds1.Tables[0].Rows[i]["QCRemoveVALUE"]);
            sht.Range("T" + row).SetValue(ds1.Tables[0].Rows[i]["QCRemove_UserID"]);
            sht.Range("U" + row).SetValue(ds1.Tables[0].Rows[i]["QCRemove_Date"]);
            sht.Range("V" + row).SetValue(ds1.Tables[0].Rows[i]["Penality"]);
            sht.Range("W" + row).SetValue(ds1.Tables[0].Rows[i]["CommAmt"]);
            sht.Range("X" + row).SetValue(ds1.Tables[0].Rows[i]["PartyChallanNo"]);
            sht.Range("Y" + row).SetValue(ds1.Tables[0].Rows[i]["customerorderno"]);
            //if (Session["varCompanyId"].ToString() == "21" && Session["usertype"].ToString() != "1")
            //{
            //    sht.Column(22).Hide();
            //    sht.Column(23).Hide();
            //    sht.Range("V" + row).SetValue("");
            //    sht.Range("W" + row).SetValue("");
            //}
            //else
            //{
            //    sht.Range("V" + row).SetValue(ds1.Tables[0].Rows[i]["Penality"]);
            //    sht.Range("W" + row).SetValue(ds1.Tables[0].Rows[i]["CommAmt"]);
            //}

            //if (Session["varCompanyId"].ToString() == "16" || Session["varCompanyId"].ToString() == "28")
            //{
            //    sht.Range("X" + row).SetValue(ds1.Tables[0].Rows[i]["PartyChallanNo"]);
            //}
            //else
            //{
            //    sht.Column(24).Hide();
            //    sht.Range("X" + row).SetValue("");
            //}

            //if (Session["varCompanyId"].ToString() == "21")
            //{
            //    sht.Range("Y" + row).SetValue("");
            //}
            //else
            //{
            //    sht.Column(25).Hide();
            //    sht.Range("Y" + row).SetValue("");
            //}

            //if (Session["varCompanyId"].ToString() == "14")
            //{
            //    sht.Range("Z" + row).SetValue(ds1.Tables[0].Rows[i]["ActualWidth"]);
            //    sht.Range("AA" + row).SetValue(ds1.Tables[0].Rows[i]["ActualLength"]);
            //}
            //else
            //{
            //    sht.Column(26).Hide();
            //    sht.Column(27).Hide();
            //    sht.Range("Z" + row).SetValue("");
            //    sht.Range("AA" + row).SetValue("");
            //}


            row = row + 1;
        }
        ds.Dispose();
        ds1.Dispose();
        //*************************************************
        using (var a = sht.Range("A1" + ":AA" + row))
        {
            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        }

        //*************************************************
        String Path;
        sht.Columns(1, 29).AdjustToContents();
        string Fileextension = "xlsx";
        string filename = UtilityModule.validateFilename("Perdayproductionstatus_" + DateTime.Now + "." + Fileextension);
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
    private void Perdayproductionwithdetail(DataSet ds, String FilterBy)
    {
        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("Perdayproductionstatus_");

        //*************
        //***********
        sht.Row(1).Height = 24;
        sht.Range("A1:X1").Merge();
        sht.Range("A1:X1").Style.Font.FontSize = 10;
        sht.Range("A1:X1").Style.Font.Bold = true;
        sht.Range("A1:X1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A1:X1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("A1:X1").Style.Alignment.WrapText = true;
        //************
        sht.Range("A1").SetValue(ds.Tables[0].Rows[0]["CompanyName"] + " Per Day Production From : " + txtfromdate.Text + " To : " + txttodate.Text + " " + FilterBy);

        sht.Range("A2:AA2").Style.Font.FontSize = 10;
        sht.Range("A2:AA2").Style.Font.Bold = true;

        sht.Range("A2").Value = "Bazar Date";
        sht.Range("B2").Value = "Carpet No.";
        sht.Range("C2").Value = "Unit Name";

        if (Session["varCompanyId"].ToString() == "45" && Session["varSubCompanyId"].ToString() == "451")
        {
            sht.Column(4).Hide();
        }
        else
        {
            sht.Range("D2").Value = "Loom No";
        }

        sht.Range("E2").Value = "Folio No";
        sht.Range("F2").Value = "Quality";
        sht.Range("G2").Value = "Design";
        sht.Range("H2").Value = "Color";
        sht.Range("I2").Value = "Size";
        sht.Range("J2").Value = "Qty";
        sht.Range("K2").Value = "Area";
        if (Session["varCompanyId"].ToString() == "21" && Session["usertype"].ToString() != "1")
        {
            sht.Column(12).Hide();
            sht.Column(13).Hide();
        }
        else
        {
            sht.Range("L2").Value = "Rate";
            sht.Range("M2").Value = "Amount";
        }
        sht.Range("N2").Value = "Weight";
        if (Session["varCompanyId"].ToString() == "21" && Session["usertype"].ToString() != "1")
        {
            sht.Column(15).Hide();
            sht.Column(16).Hide();
        }
        else
        {
            sht.Range("O2").Value = "Emp Name";
            sht.Range("P2").Value = "Status";
        }
        sht.Range("Q2").Value = "Defects";
        sht.Range("R2").Value = "User Name";
        sht.Range("S2").Value = "Remove Defects";
        sht.Range("T2").Value = "Inspected By";

        if (Session["varCompanyId"].ToString() == "45")
        {
            sht.Column(21).Hide();
        }
        else
        {   
            sht.Range("U2").Value = "Inspection Date";
        }

        if (Session["varCompanyId"].ToString() == "21" && Session["usertype"].ToString() != "1")
        {
            sht.Column(22).Hide();
            sht.Column(23).Hide();
        }
        else if (Session["varCompanyId"].ToString() == "45")
        {
            sht.Column(22).Hide();
            sht.Column(23).Hide();
        }
        else
        {
            sht.Range("W2").Value = "Commission Amount";
            sht.Range("V2").Value = "Penality Amount";
        }

        if (Session["varCompanyId"].ToString() == "16" || Session["varCompanyId"].ToString() == "28")
        {
            sht.Range("X2").Value = "Party ChallanNo"; 
        }
        else
        {
            sht.Column(24).Hide();
        }
        if (Session["varCompanyId"].ToString() == "21")
        {
            sht.Range("Y2").Value = "QC Comment"; 
        }
        else
        {
            sht.Column(25).Hide();
        }

        if (Session["varCompanyId"].ToString() == "14")
        {
            sht.Range("Z2").Value = "Actual Width";
            sht.Range("AA2").Value = "Actual Length";            
        }
        else
        {
            sht.Column(26).Hide();
            sht.Column(27).Hide();
        }

        sht.Range("AB2").Value = "Remarks";     
        int row = 3;

        DataView dv = ds.Tables[0].DefaultView;
        dv.Sort = "Receivedate,issueorderid";
        DataSet ds1 = new DataSet();
        ds1.Tables.Add(dv.ToTable());
        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Receivedate"]);
            sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Tstockno"]);
            sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Unitname"]);

            if (Session["varCompanyId"].ToString() == "45" && Session["varSubCompanyId"].ToString() == "451")
            {
                sht.Column(4).Hide();
            }
            else
            {
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
            }

            sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["FolioChallanNo"]);
            sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["QualityName"]);
            sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Designname"]);
            sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["colorname"]);
            sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
            sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);
            sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Area"]);
            if (Session["varCompanyId"].ToString() == "21" && Session["usertype"].ToString() != "1")
            {
                sht.Column(12).Hide();
                sht.Column(13).Hide();
            }
            else
            {
                sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);
                sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["Amount"]);
            }
            sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["Weight"]);
            if (Session["varCompanyId"].ToString() == "21" && Session["usertype"].ToString() != "1")
            {
                sht.Column(15).Hide();
                sht.Range("O" + row).SetValue("");
                sht.Column(16).Hide();
                sht.Range("P" + row).SetValue("");
            }
            else
            {
                sht.Range("O" + row).SetValue(ds1.Tables[0].Rows[i]["Empname"]);
                sht.Range("P" + row).SetValue(ds1.Tables[0].Rows[i]["Stockstatus"]);
            }
            
            sht.Range("Q" + row).SetValue(ds1.Tables[0].Rows[i]["Defect"]);
            sht.Range("R" + row).SetValue(ds1.Tables[0].Rows[i]["username"]);
            sht.Range("S" + row).SetValue(ds1.Tables[0].Rows[i]["QCRemoveVALUE"]);
            sht.Range("T" + row).SetValue(ds1.Tables[0].Rows[i]["QCRemove_UserID"]);

            if (Session["varCompanyId"].ToString() == "45")
            {
                sht.Column(21).Hide();
            }
            else
            {   
                sht.Range("U" + row).SetValue(ds1.Tables[0].Rows[i]["QCRemove_Date"]);
            }

            
            if (Session["varCompanyId"].ToString() == "21" && Session["usertype"].ToString() != "1")
            {
                sht.Column(22).Hide();
                sht.Column(23).Hide();
                sht.Range("V" + row).SetValue("");
                sht.Range("W" + row).SetValue("");
            }
            else if (Session["varCompanyId"].ToString() == "45")
            {
                sht.Column(22).Hide();
                sht.Column(23).Hide();
                sht.Range("V" + row).SetValue("");
                sht.Range("W" + row).SetValue("");
            }
            else
            {
                sht.Range("V" + row).SetValue(ds1.Tables[0].Rows[i]["Penality"]);
                sht.Range("W" + row).SetValue(ds1.Tables[0].Rows[i]["CommAmt"]);
            }

            if (Session["varCompanyId"].ToString() == "16" || Session["varCompanyId"].ToString() == "28")
            {
                sht.Range("X" + row).SetValue(ds1.Tables[0].Rows[i]["PartyChallanNo"]);                  
            }
            else
            {
                sht.Column(24).Hide();
                sht.Range("X" + row).SetValue("");
            }

            if (Session["varCompanyId"].ToString() == "21")
            {
                sht.Range("Y" + row).SetValue("");
            }
            else
            {
                sht.Column(25).Hide();
                sht.Range("Y" + row).SetValue("");    
            }

            if (Session["varCompanyId"].ToString() == "14")
            {
                sht.Range("Z" + row).SetValue(ds1.Tables[0].Rows[i]["ActualWidth"]);
                sht.Range("AA" + row).SetValue(ds1.Tables[0].Rows[i]["ActualLength"]);
            }
            else
            {
                sht.Column(26).Hide();
                sht.Column(27).Hide();
                sht.Range("Z" + row).SetValue("");
                sht.Range("AA" + row).SetValue("");
            }
            sht.Range("AB" + row).SetValue(ds1.Tables[0].Rows[i]["REMARKS"]);
            row = row + 1;
        }
        ds.Dispose();
        ds1.Dispose();
        //*************************************************
        using (var a = sht.Range("A1" + ":AB" + row))
        {
            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        }

        //*************************************************
        String Path;
        sht.Columns(1, 29).AdjustToContents();
        string Fileextension = "xlsx";
        string filename = UtilityModule.validateFilename("Perdayproductionstatus_" + DateTime.Now + "." + Fileextension);
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

    private void PerdayproductionSummary(DataSet ds, String FilterBy)
    {
        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("PerdayproductionstatusSummary_");

        //*************
        //***********
        sht.Row(1).Height = 24;
        sht.Range("A1:O1").Merge();
        sht.Range("A1:O1").Style.Font.FontSize = 10;
        sht.Range("A1:O1").Style.Font.Bold = true;
        sht.Range("A1:O1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A1:O1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("A1:O1").Style.Alignment.WrapText = true;
        //************
        sht.Range("A1").SetValue("Per Day Production From : " + txtfromdate.Text + " To : " + txttodate.Text + " " + FilterBy);

        sht.Range("A2:O2").Style.Font.FontSize = 10;
        sht.Range("A2:O2").Style.Font.Bold = true;

        sht.Range("A2").Value = "Bazar Date";
        sht.Range("B2").Value = "Carpet No.";
        sht.Range("C2").Value = "Unit Name";
        sht.Range("D2").Value = "Loom No";
        sht.Range("E2").Value = "Folio No";
        sht.Range("F2").Value = "Quality";
        sht.Range("G2").Value = "Design";
        sht.Range("H2").Value = "Color";
        sht.Range("I2").Value = "Size";
        sht.Range("J2").Value = "Qty";
        sht.Range("K2").Value = "Total Area";
        sht.Range("L2").Value = "Total Weight";
        sht.Range("M2").Value = "Total Amount";
        sht.Range("N2").Value = "Emp Name";
        int row = 3;
        int Rowfrom = 3;

        DataView dv = ds.Tables[0].DefaultView;
        dv.Sort = "Receivedate";
        DataSet ds1 = new DataSet();
        ds1.Tables.Add(dv.ToTable());
        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Receivedate"]);
            sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["CarpetNo"]);
            sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Unitname"]);
            sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
            sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["FolioChallanNo"]);
            sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["QualityName"]);
            sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Designname"]);
            sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["colorname"]);
            sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
            sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["RecQty"]);
            sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Area"]);
            sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["Weight"]);
            sht.Range("M" + row).SetValue(Convert.ToDouble(ds1.Tables[0].Rows[i]["RecQty"]) * Convert.ToDouble(ds1.Tables[0].Rows[i]["Rate"]));
            sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["EmpName"]);

            row = row + 1;
        }
        //TOTAL

        sht.Range("I" + row).Value = "Total";
        sht.Range("J" + row).FormulaA1 = "=SUM(J" + Rowfrom + ":$J$" + (row - 1) + ")";
        sht.Range("K" + row).FormulaA1 = "=SUM(K" + Rowfrom + ":$K$" + (row - 1) + ")";
        sht.Range("L" + row).FormulaA1 = "=SUM(L" + Rowfrom + ":$L$" + (row - 1) + ")";
        sht.Range("M" + row).FormulaA1 = "=SUM(M" + Rowfrom + ":$M$" + (row - 1) + ")";
        sht.Range("I" + row + ":N" + row).Style.Font.SetBold();
        row = row + 1;

        ds.Dispose();
        ds1.Dispose();
        //*************************************************
        using (var a = sht.Range("A1" + ":O" + row))
        {
            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        }

        //*************************************************
        String Path;
        sht.Columns(1, 26).AdjustToContents();
        string Fileextension = "xlsx";
        string filename = UtilityModule.validateFilename("PerdayproductionstatusSummary_" + DateTime.Now + "." + Fileextension);
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

    private void PerdayproductionwithdetailDiamond(DataSet ds, String FilterBy)
    {
        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("Perdayproductionstatus_");

        //*************
        //***********
        sht.Row(1).Height = 24;
        sht.Range("A1:Y1").Merge();
        sht.Range("A1:Y1").Style.Font.FontSize = 10;
        sht.Range("A1:Y1").Style.Font.Bold = true;
        sht.Range("A1:Y1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A1:Y1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("A1:Y1").Style.Alignment.WrapText = true;
        //************
        sht.Range("A1").SetValue("Per Day Production From : " + txtfromdate.Text + " To : " + txttodate.Text + " " + FilterBy);

        sht.Range("A2:Y2").Style.Font.FontSize = 10;
        sht.Range("A2:Y2").Style.Font.Bold = true;

        sht.Range("A2").Value = "Bazar Date";
        sht.Range("B2").Value = "Carpet No.";
        sht.Range("C2").Value = "Unit Name";
        sht.Range("D2").Value = "Loom No";
        sht.Range("E2").Value = "Folio No";
        sht.Range("F2").Value = "Quality";
        sht.Range("G2").Value = "Design";
        sht.Range("H2").Value = "Color";
        sht.Range("I2").Value = "Size";
        sht.Range("J2").Value = "Qty";
        sht.Range("K2").Value = "Area";
        sht.Range("L2").Value = "Rate";
        sht.Range("M2").Value = "Amount";

        sht.Range("N2").Value = "Weight";

        sht.Range("O2").Value = "Emp Name";

        sht.Range("P2").Value = "Status";
        sht.Range("Q2").Value = "Defects";
        sht.Range("R2").Value = "User Name";
        sht.Range("S2").Value = "Remove Defects";
        sht.Range("T2").Value = "Remove By";
        sht.Range("U2").Value = "Remove Date";

        sht.Range("V2").Value = "Penality Amount";
        sht.Range("W2").Value = "Commission Amount";
        sht.Range("X2").Value = "Penality Remark";
        sht.Range("Y2").Value = "Grade";


        int row = 3;

        DataView dv = ds.Tables[0].DefaultView;
        dv.Sort = "Receivedate,issueorderid";
        DataSet ds1 = new DataSet();
        ds1.Tables.Add(dv.ToTable());
        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Receivedate"]);
            sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Tstockno"]);
            sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Unitname"]);
            sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
            sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["FolioChallanNo"]);
            sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["QualityName"]);
            sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Designname"]);
            sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["colorname"]);
            sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
            sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);
            sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Area"]);

            sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);
            sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["Amount"]);

            sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["Weight"]);

            sht.Range("O" + row).SetValue(ds1.Tables[0].Rows[i]["Empname"]);

            sht.Range("P" + row).SetValue(ds1.Tables[0].Rows[i]["Stockstatus"]);
            sht.Range("Q" + row).SetValue(ds1.Tables[0].Rows[i]["Defect"]);
            sht.Range("R" + row).SetValue(ds1.Tables[0].Rows[i]["username"]);
            sht.Range("S" + row).SetValue(ds1.Tables[0].Rows[i]["QCRemoveVALUE"]);
            sht.Range("T" + row).SetValue(ds1.Tables[0].Rows[i]["QCRemove_UserID"]);
            sht.Range("U" + row).SetValue(ds1.Tables[0].Rows[i]["QCRemove_Date"]);

            sht.Range("V" + row).SetValue(ds1.Tables[0].Rows[i]["Penality"]);
            sht.Range("W" + row).SetValue(ds1.Tables[0].Rows[i]["CommAmt"]);
            sht.Range("X" + row).SetValue(ds1.Tables[0].Rows[i]["PenalityRemark"]);
            sht.Range("X" + row).Style.Alignment.SetWrapText();
            sht.Range("Y" + row).SetValue(ds1.Tables[0].Rows[i]["CarpetGradeName"]);


            row = row + 1;
        }
        ds.Dispose();
        ds1.Dispose();
        //*************************************************
        using (var a = sht.Range("A1" + ":Y" + row))
        {
            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        }

        //*************************************************
        String Path;
        sht.Columns(1, 28).AdjustToContents();
        string Fileextension = "xlsx";
        string filename = UtilityModule.validateFilename("PerdayproductionstatusDiamond_" + DateTime.Now + "." + Fileextension);
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
    protected void DDCustomerOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDOrderNo, @"Select OrderID, CustomerOrderNo 
            From OrderMaster(Nolock) 
            Where CompanyID = " + DDcompany.SelectedValue + @" And 
            CustomerID = " + DDCustomerOrderNo.SelectedValue + " Order By CustomerOrderNo ", true, "Select");
    }
}