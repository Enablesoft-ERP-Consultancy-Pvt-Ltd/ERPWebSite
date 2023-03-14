using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ClosedXML.Excel;
using System.Text.RegularExpressions;

public partial class Masters_ReportForms_frmqcreport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName                                                      
                           select Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=cs.Categoryid and cs.id=0 and ICM.MasterCompanyid=" + Session["varcompanyid"] + @"
                           select Val,Type from Sizetype
                           select PROCESS_NAME_ID,PROCESS_NAME From PROCESS_NAME_MASTER Where MasterCompanyid=" + Session["varcompanyid"] + " and ProcessType=1 order by PROCESS_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 2, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDprocess, ds, 3, false, "--Plz Select--");
            txtFromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            switch (Session["varcompanyId"].ToString())
            {
                case "22":
                    if (Session["usertype"].ToString() == "1")
                    {
                        TRMonthlyBazaarReport.Visible = true;
                    }
                    else
                    {
                        TRMonthlyBazaarReport.Visible = false;
                    }
                    break;
                case "14":
                    TRMonthlyBazaarReport.Visible = false;
                    TRWithLoomNo.Visible = true;                   
                    break;
                default:
                    TRMonthlyBazaarReport.Visible = false;
                    TRWithLoomNo.Visible = false; 
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
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(DDQuality, DDDesign, DDColor, DDShape, Convert.ToInt16(ddItemName.SelectedValue));
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
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        switch (Session["varcompanyid"].ToString())
        {
            case "14":
                if (ChkWithLoomNo.Checked == true)
                {
                    QCreportWithLoomNoEastern();
                }
                else
                {
                    QCreport();
                }                
                break;
            case "22":
                if (ChkMonthlyBazaarReport.Checked == true)
                {
                    MonthlyBazaarReport();
                }
                else
                {
                    ProcessSummaryReport_Diamond();
                }
                break;
            case "16":
            case "28":
                 QCreport_OtherChampo();
                break;
            default:
                QCreport_Other();
                break;
        }
        //
    }
    private static readonly Regex InvalidFileRegex = new Regex(string.Format("[{0}]", Regex.Escape(@"<>:""/\|?*")));
    public static string validateFilename(string filename)
    {
        return InvalidFileRegex.Replace(filename, string.Empty);
    }
    protected void QCreport()
    {
        string where = "";
        lblmsg.Text = "";
        decimal defectperc = 0, cumperc = 0;
        int ii = 0;
        if (DDCategory.SelectedIndex > 0)
        {
            where = where + " and vf.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            where = where + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and vf.qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and vf.Designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            where = where + " and vf.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and vf.sizeid=" + DDSize.SelectedValue;
        }
        //********Proc
        SqlParameter[] param = new SqlParameter[6];
        param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
        param[1] = new SqlParameter("@Processid", DDprocess.SelectedValue);
        param[2] = new SqlParameter("@Process_name", DDprocess.SelectedItem.Text);
        param[3] = new SqlParameter("@fromdate", txtFromdate.Text);
        param[4] = new SqlParameter("@Todate", txttodate.Text);
        param[5] = new SqlParameter("@where", where);
        //******
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Qcreport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("QCreport-" + DDprocess.SelectedItem.Text);

            //*************
            sht.Range("A1:L1").Merge();
            sht.Range("A1").Value = "QC REPORT FOR " + DDprocess.SelectedItem.Text + " JOB From " + txtFromdate.Text + " to " + txttodate.Text + "";
            sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:L1").Style.Font.FontName = "Tahoma";
            sht.Range("A1:L1").Style.Font.FontSize = 12;
            sht.Range("A1:L1").Style.NumberFormat.Format = "@";
            //
            sht.Row(2).Height = 32.25;
            //
            sht.Range("A2:E2").Merge();
            sht.Range("A2:E2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:E2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:E2").Style.Font.FontName = "Tahoma";
            sht.Range("A2:E2").Style.Font.FontSize = 12;
            sht.Range("A2:E2").Style.NumberFormat.Format = "@";
            sht.Range("A2:E2").Style.Fill.SetBackgroundColor(XLColor.Orange);
            sht.Range("A2").Value = "Article Wise " + DDprocess.SelectedItem.Text + " Defects analysis";
            //            
            sht.Range("F2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("F2:J2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("F2:J2").Style.Font.FontName = "Tahoma";
            sht.Range("F2:J2").Style.Font.FontSize = 12;
            sht.Range("F2:J2").Style.NumberFormat.Format = "@";
            sht.Range("F2:J2").Style.Fill.SetBackgroundColor(XLColor.LightGreen);
            sht.Range("H2").Style.Alignment.WrapText = true;

            sht.Range("F2").Value = "Defect";
            sht.Range("G2").Value = "Defect Qty.";
            sht.Range("H2").Value = "Cumulative Qty.";
            sht.Range("I2").Value = "Defect %";
            sht.Range("J2").Value = "Cum %";
            var _With1 = sht.Range("F2:J2");
            _With1.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            _With1.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            _With1.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            _With1.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            // values
            ii = 3;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + ii).Value = ds.Tables[0].Rows[i]["Qualityname"];
                sht.Range("B" + ii).Value = ds.Tables[0].Rows[i]["Designname"];
                sht.Range("C" + ii).Value = ds.Tables[0].Rows[i]["Colorname"];
                sht.Range("D" + ii).Value = ds.Tables[0].Rows[i]["Sizemtr"];
                sht.Range("E" + ii).Value = ds.Tables[0].Rows[i]["Recqty"];
                //Defects
                ds.Tables[1].DefaultView.RowFilter = "Item_finished_id=" + ds.Tables[0].Rows[i]["Item_finished_id"] + "";
                DataView dv = ds.Tables[1].DefaultView;
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                    {
                        sht.Range("F" + ii).SetValue(ds1.Tables[0].Rows[j]["paraname"]);
                        sht.Range("G" + ii).SetValue(ds1.Tables[0].Rows[j]["Defectqty"]);
                        sht.Range("H" + ii).SetValue(ds1.Tables[0].Rows[j]["cumulativeqty"]);
                        defectperc = (Convert.ToDecimal(ds1.Tables[0].Rows[j]["Defectqty"]) * 100) / (Convert.ToDecimal(ds.Tables[0].Rows[i]["recqty"]));
                        cumperc = (Convert.ToDecimal(ds1.Tables[0].Rows[j]["cumulativeqty"]) * 100) / (Convert.ToDecimal(ds.Tables[0].Rows[i]["recqty"]));
                        sht.Range("I" + ii).SetValue(defectperc);
                        sht.Range("J" + ii).SetValue(cumperc);
                        sht.Range("I" + ii).Style.NumberFormat.Format = "0.00";
                        sht.Range("J" + ii).Style.NumberFormat.Format = "0.00";
                        ii = ii + 1;
                    }
                }
                else
                {
                    ii = ii + 1;
                }
                //
                sht.Columns(1, 20).AdjustToContents();
                //sht.Column("A").Width = 14.71;
                //sht.Column("B").Width = 8.86;
                //sht.Column("C").Width = 18.00;
                //sht.Column("D").Width = 9.29;
                //sht.Column("E").Width = 10.43;                
                sht.Column("G").Width = 10.57;
                sht.Column("H").Width = 12.00;
                sht.Column("I").Width = 10.14;
                sht.Column("J").Width = 9.00;
            }
            //*********************************
            string Fileextension = "xlsx";
            string filename = validateFilename("QCREPORT(" + DDprocess.SelectedItem.Text + ")-" + DateTime.Now + "." + Fileextension);
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
            //*****
            //File.Delete(Path);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void QCreport_Other()
    {
        string where = "";
        lblmsg.Text = "";

        if (DDCategory.SelectedIndex > 0)
        {
            where = where + " and vf.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            where = where + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and vf.qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and vf.Designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            where = where + " and vf.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and vf.sizeid=" + DDSize.SelectedValue;
        }
        //********Proc
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_QCREPORT_OTHER", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;
        cmd.Parameters.AddWithValue("@companyId", DDcompany.SelectedValue);
        cmd.Parameters.AddWithValue("@Processid", DDprocess.SelectedValue);
        cmd.Parameters.AddWithValue("@Process_name", DDprocess.SelectedItem.Text);
        cmd.Parameters.AddWithValue("@fromdate", txtFromdate.Text);
        cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
        cmd.Parameters.AddWithValue("@where", where);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varcompanyid"]);


        //SqlParameter[] param = new SqlParameter[6];
        //param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
        //param[1] = new SqlParameter("@Processid", DDprocess.SelectedValue);
        //param[2] = new SqlParameter("@Process_name", DDprocess.SelectedItem.Text);
        //param[3] = new SqlParameter("@fromdate", txtFromdate.Text);
        //param[4] = new SqlParameter("@Todate", txttodate.Text);
        //param[5] = new SqlParameter("@where", where);
        ////******
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_QCREPORT_OTHER", param);
        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("QCreport-" + DDprocess.SelectedItem.Text);
            //*************
            int row = 5;
            int DetailHstart = row;
            int Defectlabelrow = row - 1;
            int Defectcellstart = 0;
            int Firstdefectcell = 0;
            int lastdefectcell = 0;
            int Failedpcscell = 0;
            int Remarkcell = 0;

            sht.Range("A1:E1").Merge();
            sht.Range("A1").SetValue(DDcompany.SelectedItem.Text);
            sht.Range("A1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:E1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            sht.Range("F1:O1").Merge();
            sht.Range("F1").SetValue("Daily 100% " + DDprocess.SelectedItem.Text + " Checking Report");
            sht.Range("F1:O1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("F1:O1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            sht.Range("A1:O1").Style.Font.SetBold();
            sht.Range("A1:O1").Style.Font.FontSize = 14;

            sht.Range("P1:S1").Merge();
            sht.Range("P1").SetValue("Format No.:CHC/LCR/19");
            sht.Range("P2:S2").Merge();
            sht.Range("P2").SetValue("Version-02");
            sht.Range("P3:S3").Merge();
            sht.Range("P3").SetValue("Eff.Date :21.06.2018");

            sht.Range("A3:E3").Merge();
            string Datevalue = "";
            if (txtFromdate.Text == txttodate.Text)
            {
                Datevalue = txtFromdate.Text;
            }
            else
            {
                Datevalue = txtFromdate.Text + " To " + txttodate.Text;
            }
            sht.Range("A3").SetValue("Date :" + Datevalue);
            sht.Range("A3:E3").Style.Font.SetBold();

            sht.Range("A" + row).Value = "Sr No.";
            sht.Range("B" + row).Value = "Quality";
            sht.Range("C" + row).Value = "Style Name";
            sht.Range("D" + row).Value = "Colour";
            sht.Range("E" + row).Value = "Size";
            sht.Range("F" + row).Value = "Total Checked Pcs";
            sht.Range("G" + row).Value = "Total Passed Pcs";
            sht.Range("H" + row).Value = "Area";

            sht.Range("F" + row).Style.Alignment.SetWrapText();
            sht.Range("G" + row).Style.Alignment.SetWrapText();

            //***********DEFECTS COLUMN
            if (ds.Tables[1].Rows.Count > 0)
            {
                Defectcellstart = 8;
                Firstdefectcell = Defectcellstart + 1;
                DataTable dtdistinct = ds.Tables[1].DefaultView.ToTable(true, "PARANAME");
                DataView dv1 = new DataView(dtdistinct);
                DataTable dtdistinct1 = dv1.ToTable();
                foreach (DataRow dr in dtdistinct1.Rows)
                {
                    Defectcellstart += 1;
                    sht.Cell(row, Defectcellstart).Value = dr["PARANAME"];
                }
                lastdefectcell = Defectcellstart;
                sht.Cell(Defectlabelrow, Firstdefectcell).Value = "DEFECTS";

                sht.Range(sht.Cell(Defectlabelrow, Firstdefectcell), sht.Cell(Defectlabelrow, lastdefectcell)).Merge();
                sht.Range(sht.Cell(Defectlabelrow, Firstdefectcell), sht.Cell(Defectlabelrow, lastdefectcell)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range(sht.Cell(Defectlabelrow, Firstdefectcell), sht.Cell(Defectlabelrow, lastdefectcell)).Style.Fill.BackgroundColor = XLColor.Black;
                sht.Range(sht.Cell(Defectlabelrow, Firstdefectcell), sht.Cell(Defectlabelrow, lastdefectcell)).Style.Font.FontColor = XLColor.White;

            }
            else
            {
                Defectcellstart = 8;
            }
            //**********Failedpcs cell
            Defectcellstart += 1;
            Failedpcscell = Defectcellstart;
            sht.Cell(row, Failedpcscell).Value = "Failed Pcs";
            //*************
            //**********REMARK CELL
            Defectcellstart += 1;
            Remarkcell = Defectcellstart;
            sht.Cell(row, Remarkcell).Value = "Remark";
            //********
            sht.Range(sht.Cell(row, 1), sht.Cell(row, Remarkcell)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range(sht.Cell(row, 1), sht.Cell(row, Remarkcell)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //Borders
            using (var a = sht.Range(sht.Cell(row, 1), sht.Cell(row, Remarkcell)))
            {
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            //*********DETAILS
            row += 1;
            int Rejectedpcs = 0;
            int Checkedpcs = 0;
            int index = Firstdefectcell;
            int RejQty = 0;
            string paramname = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row).SetValue(i + 1);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Designname"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Colorname"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Sizeft"]);
                Checkedpcs = Convert.ToInt32(ds.Tables[0].Rows[i]["CHECKPCS"]);
                Rejectedpcs = Convert.ToInt32(ds.Tables[0].Rows[i]["REJECTPCS"]);
                sht.Range("F" + row).SetValue(Checkedpcs);
                sht.Range("G" + row).SetValue(Checkedpcs - Rejectedpcs);
                sht.Range("F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H" + row).SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["Area"]) * (Checkedpcs - Rejectedpcs));
                //**********DEFECTS VALUE
                index = Firstdefectcell;
                if (Firstdefectcell > 0)
                {
                    while (index <= lastdefectcell)
                    {
                        paramname = sht.Cell(DetailHstart, index).Value.ToString();
                        var rejqty = Convert.ToInt32(ds.Tables[1].Compute("Sum(Defectqty)", "ITEM_FINISHED_ID=" + ds.Tables[0].Rows[i]["ITEM_FINISHED_ID"] + " AND PARANAME='" + paramname + "'") == DBNull.Value ? "0" : ds.Tables[1].Compute("Sum(Defectqty)", "ITEM_FINISHED_ID=" + ds.Tables[0].Rows[i]["ITEM_FINISHED_ID"] + " AND PARANAME='" + paramname + "'"));
                        if (rejqty > 0)
                        {
                            sht.Cell(row, index).SetValue(rejqty);
                            sht.Cell(row, index).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        }

                        index += 1;
                    }
                }
                //**********
                sht.Cell(row, Failedpcscell).SetValue(Rejectedpcs);
                sht.Cell(row, Failedpcscell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell(row, Remarkcell).SetValue("");

                //Borders
                using (var a = sht.Range(sht.Cell(row, 1), sht.Cell(row, Remarkcell)))
                {
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                row += 1;
            }
            //Borders
            using (var a = sht.Range(sht.Cell(row, 1), sht.Cell((row + 3), 1)))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range(sht.Cell(row, Remarkcell), sht.Cell((row + 3), Remarkcell)))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }
            row += 3;
            //********Signature
            sht.Range("A" + row + ":D" + row).Merge();
            sht.Range("A" + row).SetValue("Signature of Supervisor");
            sht.Range("N" + row + ":Q" + row).Merge();
            sht.Range("N" + row).SetValue("Signature of Deptt. Head");
            sht.Range("A" + row + ":D" + row).Style.Font.SetBold();
            sht.Range("A" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("N" + row + ":Q" + row).Style.Font.SetBold();
            sht.Range("N" + row + ":Q" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            using (var a = sht.Range(sht.Cell(row, 1), sht.Cell(row, Remarkcell)))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*********Borders

            //*********************************
            sht.Columns(1, 40).AdjustToContents();
            sht.Columns("F").Width = 7;
            sht.Columns("G").Width = 7;
            if (Firstdefectcell > 0)
            {
                sht.Columns(Firstdefectcell, lastdefectcell).Width = 8.67;
                sht.Columns(Firstdefectcell, lastdefectcell).Style.Alignment.SetWrapText();
            }
            sht.Columns(Failedpcscell, Failedpcscell).Width = 7.22;
            sht.Columns(Failedpcscell, Failedpcscell).Style.Alignment.SetWrapText();

            string Fileextension = "xlsx";
            string filename = validateFilename("QCREPORT(" + DDprocess.SelectedItem.Text + ")-" + DateTime.Now + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            // Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
            //*****
            //File.Delete(Path);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void QCreport_OtherChampo()
    {
        string where = "";
        lblmsg.Text = "";

        if (DDCategory.SelectedIndex > 0)
        {
            where = where + " and vf.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            where = where + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and vf.qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and vf.Designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            where = where + " and vf.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and vf.sizeid=" + DDSize.SelectedValue;
        }
        //********Proc
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_QCREPORT_OTHERChampo", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;
        cmd.Parameters.AddWithValue("@companyId", DDcompany.SelectedValue);
        cmd.Parameters.AddWithValue("@Processid", DDprocess.SelectedValue);
        cmd.Parameters.AddWithValue("@Process_name", DDprocess.SelectedItem.Text);
        cmd.Parameters.AddWithValue("@fromdate", txtFromdate.Text);
        cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
        cmd.Parameters.AddWithValue("@where", where);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varcompanyid"]);


        //SqlParameter[] param = new SqlParameter[6];
        //param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
        //param[1] = new SqlParameter("@Processid", DDprocess.SelectedValue);
        //param[2] = new SqlParameter("@Process_name", DDprocess.SelectedItem.Text);
        //param[3] = new SqlParameter("@fromdate", txtFromdate.Text);
        //param[4] = new SqlParameter("@Todate", txttodate.Text);
        //param[5] = new SqlParameter("@where", where);
        ////******
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_QCREPORT_OTHER", param);
        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("QCreport-" + DDprocess.SelectedItem.Text);
            //*************
            int row = 5;
            int DetailHstart = row;
            int Defectlabelrow = row - 1;
            int Defectcellstart = 0;
            int Firstdefectcell = 0;
            int lastdefectcell = 0;
            int Failedpcscell = 0;
            int Remarkcell = 0;
            int CustomerCodecell = 0;
            int CustomerOrderNocell = 0;
            int ScanBycell = 0;

            sht.Range("A1:E1").Merge();
            sht.Range("A1").SetValue(DDcompany.SelectedItem.Text);
            sht.Range("A1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:E1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            sht.Range("F1:O1").Merge();
            sht.Range("F1").SetValue("Daily 100% " + DDprocess.SelectedItem.Text + " Checking Report");
            sht.Range("F1:O1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("F1:O1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            sht.Range("A1:O1").Style.Font.SetBold();
            sht.Range("A1:O1").Style.Font.FontSize = 14;

            sht.Range("P1:S1").Merge();
            sht.Range("P1").SetValue("Format No.:CHC/LCR/19");
            sht.Range("P2:S2").Merge();
            sht.Range("P2").SetValue("Version-02");
            sht.Range("P3:S3").Merge();
            sht.Range("P3").SetValue("Eff.Date :21.06.2018");

            sht.Range("A3:E3").Merge();
            string Datevalue = "";
            if (txtFromdate.Text == txttodate.Text)
            {
                Datevalue = txtFromdate.Text;
            }
            else
            {
                Datevalue = txtFromdate.Text + " To " + txttodate.Text;
            }
            sht.Range("A3").SetValue("Date :" + Datevalue);
            sht.Range("A3:E3").Style.Font.SetBold();

            sht.Range("A" + row).Value = "Sr No.";
            sht.Range("B" + row).Value = "Quality";
            sht.Range("C" + row).Value = "Style Name";
            sht.Range("D" + row).Value = "Colour";
            sht.Range("E" + row).Value = "Size";
            sht.Range("F" + row).Value = "Total Checked Pcs";
            sht.Range("G" + row).Value = "Total Passed Pcs";
            sht.Range("H" + row).Value = "Area";

            sht.Range("F" + row).Style.Alignment.SetWrapText();
            sht.Range("G" + row).Style.Alignment.SetWrapText();

            //***********DEFECTS COLUMN
            if (ds.Tables[1].Rows.Count > 0)
            {
                Defectcellstart = 8;
                Firstdefectcell = Defectcellstart + 1;
                DataTable dtdistinct = ds.Tables[1].DefaultView.ToTable(true, "PARANAME");
                DataView dv1 = new DataView(dtdistinct);
                DataTable dtdistinct1 = dv1.ToTable();
                foreach (DataRow dr in dtdistinct1.Rows)
                {
                    Defectcellstart += 1;
                    sht.Cell(row, Defectcellstart).Value = dr["PARANAME"];
                }
                lastdefectcell = Defectcellstart;
                sht.Cell(Defectlabelrow, Firstdefectcell).Value = "DEFECTS";

                sht.Range(sht.Cell(Defectlabelrow, Firstdefectcell), sht.Cell(Defectlabelrow, lastdefectcell)).Merge();
                sht.Range(sht.Cell(Defectlabelrow, Firstdefectcell), sht.Cell(Defectlabelrow, lastdefectcell)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range(sht.Cell(Defectlabelrow, Firstdefectcell), sht.Cell(Defectlabelrow, lastdefectcell)).Style.Fill.BackgroundColor = XLColor.Black;
                sht.Range(sht.Cell(Defectlabelrow, Firstdefectcell), sht.Cell(Defectlabelrow, lastdefectcell)).Style.Font.FontColor = XLColor.White;

            }
            else
            {
                Defectcellstart = 8;
            }
            //**********Failedpcs cell
            Defectcellstart += 1;
            Failedpcscell = Defectcellstart;
            sht.Cell(row, Failedpcscell).Value = "Failed Pcs";
            //*************
            //**********REMARK CELL
            Defectcellstart += 1;
            Remarkcell = Defectcellstart;
            sht.Cell(row, Remarkcell).Value = "Remark";

            //**********Customer CELL
            Defectcellstart += 1;
            CustomerCodecell = Defectcellstart;
            sht.Cell(row, CustomerCodecell).Value = "Customer Code";

            //**********Customer OrderNo CELL
            Defectcellstart += 1;
            CustomerOrderNocell = Defectcellstart;
            sht.Cell(row, CustomerOrderNocell).Value = "Customer OrderNo";

            //**********Scan By CELL
            Defectcellstart += 1;
            ScanBycell = Defectcellstart;
            sht.Cell(row, ScanBycell).Value = "Scan By";

            //********
            sht.Range(sht.Cell(row, 1), sht.Cell(row, ScanBycell)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range(sht.Cell(row, 1), sht.Cell(row, ScanBycell)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //Borders
            using (var a = sht.Range(sht.Cell(row, 1), sht.Cell(row, ScanBycell)))
            {
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            //*********DETAILS
            row += 1;
            int Rejectedpcs = 0;
            int Checkedpcs = 0;
            int index = Firstdefectcell;
            int RejQty = 0;
            string paramname = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row).SetValue(i + 1);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Designname"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Colorname"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Sizeft"]);
                Checkedpcs = Convert.ToInt32(ds.Tables[0].Rows[i]["CHECKPCS"]);
                Rejectedpcs = Convert.ToInt32(ds.Tables[0].Rows[i]["REJECTPCS"]);
                sht.Range("F" + row).SetValue(Checkedpcs);
                sht.Range("G" + row).SetValue(Checkedpcs - Rejectedpcs);
                sht.Range("F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H" + row).SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["Area"]) * (Checkedpcs - Rejectedpcs));
                //**********DEFECTS VALUE
                index = Firstdefectcell;
                if (Firstdefectcell > 0)
                {
                    while (index <= lastdefectcell)
                    {
                        paramname = sht.Cell(DetailHstart, index).Value.ToString();
                        var rejqty = Convert.ToInt32(ds.Tables[1].Compute("Sum(Defectqty)", "ITEM_FINISHED_ID=" + ds.Tables[0].Rows[i]["ITEM_FINISHED_ID"] + " AND PARANAME='" + paramname + "'") == DBNull.Value ? "0" : ds.Tables[1].Compute("Sum(Defectqty)", "ITEM_FINISHED_ID=" + ds.Tables[0].Rows[i]["ITEM_FINISHED_ID"] + " AND PARANAME='" + paramname + "'"));
                        if (rejqty > 0)
                        {
                            sht.Cell(row, index).SetValue(rejqty);
                            sht.Cell(row, index).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        }

                        index += 1;
                    }
                }
                //**********
                sht.Cell(row, Failedpcscell).SetValue(Rejectedpcs);
                sht.Cell(row, Failedpcscell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell(row, Remarkcell).SetValue("");

                sht.Cell(row, CustomerCodecell).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                sht.Cell(row, CustomerCodecell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Cell(row, CustomerOrderNocell).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                sht.Cell(row, CustomerOrderNocell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                sht.Cell(row, ScanBycell).SetValue(ds.Tables[0].Rows[i]["UserName"]);
                sht.Cell(row, ScanBycell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                

                //Borders
                using (var a = sht.Range(sht.Cell(row, 1), sht.Cell(row, ScanBycell)))
                {
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                row += 1;
            }
            //Borders
            using (var a = sht.Range(sht.Cell(row, 1), sht.Cell((row + 3), 1)))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range(sht.Cell(row, ScanBycell), sht.Cell((row + 3), ScanBycell)))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }
            row += 3;
            //********Signature
            sht.Range("A" + row + ":D" + row).Merge();
            sht.Range("A" + row).SetValue("Signature of Supervisor");
            sht.Range("N" + row + ":Q" + row).Merge();
            sht.Range("N" + row).SetValue("Signature of Deptt. Head");
            sht.Range("A" + row + ":D" + row).Style.Font.SetBold();
            sht.Range("A" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("N" + row + ":Q" + row).Style.Font.SetBold();
            sht.Range("N" + row + ":Q" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            using (var a = sht.Range(sht.Cell(row, 1), sht.Cell(row, ScanBycell)))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*********Borders

            //*********************************
            sht.Columns(1, 40).AdjustToContents();
            sht.Columns("F").Width = 7;
            sht.Columns("G").Width = 7;
            if (Firstdefectcell > 0)
            {
                sht.Columns(Firstdefectcell, lastdefectcell).Width = 8.67;
                sht.Columns(Firstdefectcell, lastdefectcell).Style.Alignment.SetWrapText();
            }
            sht.Columns(Failedpcscell, Failedpcscell).Width = 7.22;
            sht.Columns(Failedpcscell, Failedpcscell).Style.Alignment.SetWrapText();

            string Fileextension = "xlsx";
            string filename = validateFilename("QCREPORT(" + DDprocess.SelectedItem.Text + ")-" + DateTime.Now + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            // Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
            //*****
            //File.Delete(Path);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void MonthlyBazaarReport()
    {
        string where = "";
        lblmsg.Text = "";

        if (DDCategory.SelectedIndex > 0)
        {
            where = where + " and vf.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            where = where + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and vf.qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and vf.Designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            where = where + " and vf.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and vf.sizeid=" + DDSize.SelectedValue;
        }
        //********Proc
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_MONTHLY_BAZAAR_REPORT", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;
        cmd.Parameters.AddWithValue("@companyId", DDcompany.SelectedValue);
        cmd.Parameters.AddWithValue("@Processid", DDprocess.SelectedValue);
        cmd.Parameters.AddWithValue("@Process_name", DDprocess.SelectedItem.Text);
        cmd.Parameters.AddWithValue("@fromdate", txtFromdate.Text);
        cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
        cmd.Parameters.AddWithValue("@where", where);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varcompanyid"]);


        //SqlParameter[] param = new SqlParameter[6];
        //param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
        //param[1] = new SqlParameter("@Processid", DDprocess.SelectedValue);
        //param[2] = new SqlParameter("@Process_name", DDprocess.SelectedItem.Text);
        //param[3] = new SqlParameter("@fromdate", txtFromdate.Text);
        //param[4] = new SqlParameter("@Todate", txttodate.Text);
        //param[5] = new SqlParameter("@where", where);
        ////******
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_QCREPORT_OTHER", param);
        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("MonthlyBazaarReport-" + DDprocess.SelectedItem.Text);
            //*************

            sht.Column("A").Width = 6.22;
            sht.Column("B").Width = 30.22;
            sht.Column("C").Width = 18.11;
            sht.Column("D").Width = 8.33;
            sht.Column("E").Width = 6.44;
            sht.Column("F").Width = 6.44;
            sht.Column("G").Width = 6.44;
            sht.Column("H").Width = 6.44;
            sht.Column("I").Width = 6.44;
            sht.Column("J").Width = 6.44;
            sht.Column("K").Width = 6.44;
            sht.Column("L").Width = 6.44;
            sht.Column("M").Width = 6.44;
            sht.Column("N").Width = 6.44;
            sht.Column("O").Width = 6.44;
            sht.Column("P").Width = 6.44;
            sht.Column("Q").Width = 6.44;
            sht.Column("R").Width = 6.44;
            sht.Column("S").Width = 6.44;
            sht.Column("T").Width = 6.44;
            sht.Column("U").Width = 6.44;
            sht.Column("V").Width = 6.44;
            sht.Column("W").Width = 6.44;
            sht.Column("X").Width = 6.44;
            sht.Column("Y").Width = 6.44;
            sht.Column("Z").Width = 6.44;
            sht.Column("AA").Width = 6.44;
            sht.Column("AB").Width = 6.44;
            sht.Column("AC").Width = 6.44;
            sht.Column("AD").Width = 6.44;
            sht.Column("AE").Width = 6.44;
            sht.Column("AF").Width = 6.44;
            sht.Column("AG").Width = 6.44;
            sht.Column("AH").Width = 6.44;
            sht.Column("AI").Width = 6.44;
            sht.Column("AJ").Width = 12.44;

            int row = 5;
            //int DetailHstart = row;
            //int Defectlabelrow = row - 1;
            //int Defectcellstart = 0;
            //int Firstdefectcell = 0;
            //int lastdefectcell = 0;
            //int Failedpcscell = 0;
            //int Remarkcell = 0;

            sht.Range("A1:AJ1").Merge();
            sht.Range("A1").SetValue(DDcompany.SelectedItem.Text);
            sht.Range("A1:AJ1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:AJ1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:AJ1").Style.Font.SetBold();
            sht.Range("A1:AJ1").Style.Font.FontSize = 14;

            sht.Range("A2:AJ2").Merge();
            sht.Range("A2").SetValue(DDprocess.SelectedItem.Text);
            sht.Range("A2:AJ2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:AJ2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:AJ2").Style.Font.SetBold();
            sht.Range("A2:AJ2").Style.Font.FontSize = 14;

            sht.Range("A3:AJ3").Merge();
            string Datevalue = "";
            if (txtFromdate.Text == txttodate.Text)
            {
                Datevalue = txtFromdate.Text;
            }
            else
            {
                Datevalue = txtFromdate.Text + " To " + txttodate.Text;
            }
            sht.Range("A3").SetValue("Date :" + Datevalue);
            sht.Range("A3:AJ3").Style.Font.SetBold();
            sht.Range("A3:AJ3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A3:AJ3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            sht.Range("A5:AJ5").Style.Font.SetBold();
            sht.Range("A5:AJ5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A5:AJ5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            sht.Range("A5").SetValue("Sr No");
            sht.Range("B5").SetValue("Style Name");
            sht.Range("C5").SetValue("Colour");
            sht.Range("D5").SetValue("Size");
            sht.Range("E5").SetValue("1");
            sht.Range("F5").SetValue("2");
            sht.Range("G5").SetValue("3");
            sht.Range("H5").SetValue("4");
            sht.Range("I5").SetValue("5");
            sht.Range("J5").SetValue("6");
            sht.Range("K5").SetValue("7");
            sht.Range("L5").SetValue("8");
            sht.Range("M5").SetValue("9");
            sht.Range("N5").SetValue("10");
            sht.Range("O5").SetValue("11");
            sht.Range("P5").SetValue("12");
            sht.Range("Q5").SetValue("13");
            sht.Range("R5").SetValue("14");
            sht.Range("S5").SetValue("15");
            sht.Range("T5").SetValue("16");
            sht.Range("U5").SetValue("17");
            sht.Range("V5").SetValue("18");
            sht.Range("W5").SetValue("19");
            sht.Range("X5").SetValue("20");
            sht.Range("Y5").SetValue("21");
            sht.Range("Z5").SetValue("22");
            sht.Range("AA5").SetValue("23");
            sht.Range("AB5").SetValue("24");
            sht.Range("AC5").SetValue("25");
            sht.Range("AD5").SetValue("26");
            sht.Range("AE5").SetValue("27");
            sht.Range("AF5").SetValue("28");
            sht.Range("AG5").SetValue("29");
            sht.Range("AH5").SetValue("30");
            sht.Range("AI5").SetValue("31");
            sht.Range("AJ5").SetValue("Total");



            // sht.Range("F" + row).Style.Alignment.SetWrapText();
            //sht.Range("G" + row).Style.Alignment.SetWrapText();

            //***********DEFECTS COLUMN

            row = row + 1;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                using (var a = sht.Range("A" + row + ":AJ" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                if (ds.Tables[0].Rows[i]["ColorId"].ToString() == "0" && ds.Tables[0].Rows[i]["DesignId"].ToString() == "0" && ds.Tables[0].Rows[i]["Type"].ToString() == "2")
                {
                    row = row + 1;
                    sht.Range("A" + row + ":AJ" + row).Style.Font.SetBold();
                }

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["SRNo"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Designname"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Colorname"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Size"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["1"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["2"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["3"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["4"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["5"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["6"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["7"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["8"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["9"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["10"]);
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["11"]);
                sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["12"]);
                sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["13"]);
                sht.Range("R" + row).SetValue(ds.Tables[0].Rows[i]["14"]);
                sht.Range("S" + row).SetValue(ds.Tables[0].Rows[i]["15"]);
                sht.Range("T" + row).SetValue(ds.Tables[0].Rows[i]["16"]);
                sht.Range("U" + row).SetValue(ds.Tables[0].Rows[i]["17"]);
                sht.Range("V" + row).SetValue(ds.Tables[0].Rows[i]["18"]);
                sht.Range("W" + row).SetValue(ds.Tables[0].Rows[i]["19"]);
                sht.Range("X" + row).SetValue(ds.Tables[0].Rows[i]["20"]);
                sht.Range("Y" + row).SetValue(ds.Tables[0].Rows[i]["21"]);
                sht.Range("Z" + row).SetValue(ds.Tables[0].Rows[i]["22"]);
                sht.Range("AA" + row).SetValue(ds.Tables[0].Rows[i]["23"]);
                sht.Range("AB" + row).SetValue(ds.Tables[0].Rows[i]["24"]);
                sht.Range("AC" + row).SetValue(ds.Tables[0].Rows[i]["25"]);
                sht.Range("AD" + row).SetValue(ds.Tables[0].Rows[i]["26"]);
                sht.Range("AE" + row).SetValue(ds.Tables[0].Rows[i]["27"]);
                sht.Range("AF" + row).SetValue(ds.Tables[0].Rows[i]["28"]);
                sht.Range("AG" + row).SetValue(ds.Tables[0].Rows[i]["29"]);
                sht.Range("AH" + row).SetValue(ds.Tables[0].Rows[i]["30"]);
                sht.Range("AI" + row).SetValue(ds.Tables[0].Rows[i]["31"]);
                sht.Range("AJ" + row).SetValue(ds.Tables[0].Rows[i]["Total"]);
                sht.Range("AJ" + row).Style.Font.SetBold();



                row = row + 1;
            }
            using (var a = sht.Range("A" + row + ":AJ" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = validateFilename("MONTHLYBAZAARREPORT(" + DDprocess.SelectedItem.Text + ")-" + DateTime.Now + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            // Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
            //*****
            //File.Delete(Path);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }

    protected void ProcessSummaryReport_Diamond()
    {
        string where = "";
        lblmsg.Text = "";

        if (DDCategory.SelectedIndex > 0)
        {
            where = where + " and vf.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            where = where + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and vf.qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and vf.Designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            where = where + " and vf.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and vf.sizeid=" + DDSize.SelectedValue;
        }
        //********Proc
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_ProcessSummaryReport_Diamond", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;
        cmd.Parameters.AddWithValue("@companyId", DDcompany.SelectedValue);
        cmd.Parameters.AddWithValue("@Processid", DDprocess.SelectedValue);
        cmd.Parameters.AddWithValue("@Process_name", DDprocess.SelectedItem.Text);
        cmd.Parameters.AddWithValue("@fromdate", txtFromdate.Text);
        cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
        cmd.Parameters.AddWithValue("@where", where);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varcompanyid"]);


        //SqlParameter[] param = new SqlParameter[6];
        //param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
        //param[1] = new SqlParameter("@Processid", DDprocess.SelectedValue);
        //param[2] = new SqlParameter("@Process_name", DDprocess.SelectedItem.Text);
        //param[3] = new SqlParameter("@fromdate", txtFromdate.Text);
        //param[4] = new SqlParameter("@Todate", txttodate.Text);
        //param[5] = new SqlParameter("@where", where);
        ////******
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_QCREPORT_OTHER", param);
        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("SummaryReport-" + DDprocess.SelectedItem.Text);
            //*************
            int row = 5;
            int DetailHstart = row;
            int Defectlabelrow = row - 1;           
            int Firstdefectcell = 0;           
            int Remarkcell = 0;

            sht.Range("A1:E1").Merge();
            sht.Range("A1").SetValue(DDcompany.SelectedItem.Text);
            sht.Range("A1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:E1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:E1").Style.Font.SetBold();

            sht.Range("F1:O1").Merge();
            sht.Range("F1").SetValue(DDprocess.SelectedItem.Text + " Checking Report");
            sht.Range("F1:O1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("F1:O1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("F1:O1").Style.Font.SetBold();

            //sht.Range("A1:O1").Style.Font.SetBold();
            //sht.Range("A1:O1").Style.Font.FontSize = 14;

            //sht.Range("P1:S1").Merge();
            //sht.Range("P1").SetValue("Format No.:CHC/LCR/19");
            //sht.Range("P2:S2").Merge();
            //sht.Range("P2").SetValue("Version-02");
            //sht.Range("P3:S3").Merge();
            //sht.Range("P3").SetValue("Eff.Date :21.06.2018");

            sht.Range("A3:E3").Merge();
            string Datevalue = "";
            if (txtFromdate.Text == txttodate.Text)
            {
                Datevalue = txtFromdate.Text;
            }
            else
            {
                Datevalue = txtFromdate.Text + " To " + txttodate.Text;
            }
            sht.Range("A3").SetValue("Date :" + Datevalue);
            sht.Range("A3:E3").Style.Font.SetBold();

            sht.Range("A" + row).Value = "Sr No.";
            sht.Range("B" + row).Value = "Quality";
            sht.Range("C" + row).Value = "Style Name";
            sht.Range("D" + row).Value = "Colour";
            sht.Range("E" + row).Value = "Size";
            sht.Range("F" + row).Value = "Total Checked Pcs";
            sht.Range("G" + row).Value = "Total Passed Pcs";
            sht.Range("H" + row).Value = "Area";
            sht.Range("I" + row).Value = "Failed Pcs";
            sht.Range("J" + row).Value = "Remarks";
            sht.Range("K" + row).Value = "Fail(%)";
            sht.Range("L" + row).Value = "KPI(%)";

            sht.Range("F" + row).Style.Alignment.SetWrapText();
            sht.Range("G" + row).Style.Alignment.SetWrapText();
            sht.Range("H" + row).Style.Alignment.SetWrapText();
            sht.Range("I" + row).Style.Alignment.SetWrapText();
            sht.Range("J" + row).Style.Alignment.SetWrapText();
            sht.Range("K" + row).Style.Alignment.SetWrapText();
            sht.Range("L" + row).Style.Alignment.SetWrapText();

            using (var a = sht.Range("A" + row + ":L" + row))
            {
                a.Style.Font.SetBold();
            }

            using (var a = sht.Range("A" + row + ":L" + row))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }  

            if (ds.Tables[0].Rows.Count > 0)
            {
                //*********DETAILS
                row += 1;
                int Rejectedpcs = 0;
                int Checkedpcs = 0;
                int index = Firstdefectcell;
                int RejQty = 0;
                string paramname = "";
                decimal failper = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A" + row).SetValue(i + 1);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Designname"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Colorname"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Sizeft"]);
                    Checkedpcs = Convert.ToInt32(ds.Tables[0].Rows[i]["CHECKEDPCS"]);
                    Rejectedpcs = Convert.ToInt32(ds.Tables[0].Rows[i]["REJECTPCS"]);
                    sht.Range("F" + row).SetValue(Checkedpcs);
                    sht.Range("G" + row).SetValue(Checkedpcs - Rejectedpcs);
                    sht.Range("F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("H" + row).SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["Area"]) * (Checkedpcs - Rejectedpcs));
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["REJECTPCS"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["PenalityRemarks"]);
                    sht.Range("J" + row).Style.Alignment.SetWrapText();
                    failper = Convert.ToDecimal(ds.Tables[0].Rows[i]["REJECTPCS"]) / Convert.ToDecimal(ds.Tables[0].Rows[i]["CHECKEDPCS"]) * 100;
                    sht.Range("K" + row).SetValue(Math.Round(failper,2)+"%");
                    sht.Range("L" + row).SetValue(Math.Round(100 - failper, 2) + "%");

                    //Borders
                    using (var a = sht.Range("A" + row + ":L" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }                   

                    row += 1;
                }

            }  
            //Borders
            using (var a = sht.Range(sht.Cell(row, 1), sht.Cell((row + 3), 1)))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            //using (var a = sht.Range(sht.Cell(row, Remarkcell), sht.Cell((row + 3), Remarkcell)))
            //{
            //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //}
            row += 3;
            //********Signature
            sht.Range("A" + row + ":D" + row).Merge();
            sht.Range("A" + row).SetValue("Signature of Supervisor");
            sht.Range("N" + row + ":Q" + row).Merge();
            sht.Range("N" + row).SetValue("Signature of Deptt. Head");
            sht.Range("A" + row + ":D" + row).Style.Font.SetBold();
            sht.Range("A" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("N" + row + ":Q" + row).Style.Font.SetBold();
            sht.Range("N" + row + ":Q" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //using (var a = sht.Range(sht.Cell(row, 1), sht.Cell(row, Remarkcell)))
            //{
            //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //}

            using (var a = sht.Range("A" + row + ":L" + row))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }    
            //*********Borders

            //*********************************
            sht.Columns(1, 40).AdjustToContents();
            sht.Columns("F").Width = 10;
            sht.Columns("G").Width = 10;
            sht.Columns("H").Width = 11.00;
            sht.Columns("I").Width = 8;
            sht.Columns("J").Width = 30;          

            string Fileextension = "xlsx";
            string filename = validateFilename("ProcessSummaryReportDiamond(" + DDprocess.SelectedItem.Text + ")-" + DateTime.Now + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            // Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
            //*****
            //File.Delete(Path);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }

    protected void QCreportWithLoomNoEastern()
    {
        string where = "";
        lblmsg.Text = "";
        decimal defectperc = 0, cumperc = 0;
        int ii = 0;
        if (DDCategory.SelectedIndex > 0)
        {
            where = where + " and vf.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            where = where + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and vf.qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and vf.Designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            where = where + " and vf.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and vf.sizeid=" + DDSize.SelectedValue;
        }
        //********Proc
        SqlParameter[] param = new SqlParameter[6];
        param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
        param[1] = new SqlParameter("@Processid", DDprocess.SelectedValue);
        param[2] = new SqlParameter("@Process_name", DDprocess.SelectedItem.Text);
        param[3] = new SqlParameter("@fromdate", txtFromdate.Text);
        param[4] = new SqlParameter("@Todate", txttodate.Text);
        param[5] = new SqlParameter("@where", where);
        //******
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_QcreportDefectWithLoomNo", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("QCreportWithLoomNo-" + DDprocess.SelectedItem.Text);

            //*************
            sht.Range("A1:L1").Merge();
            sht.Range("A1").Value = "QC REPORT FOR " + DDprocess.SelectedItem.Text + " JOB From " + txtFromdate.Text + " to " + txttodate.Text + "";
            sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:L1").Style.Font.FontName = "Tahoma";
            sht.Range("A1:L1").Style.Font.FontSize = 12;
            sht.Range("A1:L1").Style.NumberFormat.Format = "@";
            //
            sht.Row(2).Height = 32.25;
            //
            sht.Range("A2:E2").Merge();
            sht.Range("A2:E2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:E2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:E2").Style.Font.FontName = "Tahoma";
            sht.Range("A2:E2").Style.Font.FontSize = 12;
            sht.Range("A2:E2").Style.NumberFormat.Format = "@";
            sht.Range("A2:E2").Style.Fill.SetBackgroundColor(XLColor.Orange);
            sht.Range("A2").Value = "Article Wise " + DDprocess.SelectedItem.Text + " Defects analysis";
            //            
            sht.Range("F2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("F2:H2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("F2:H2").Style.Font.FontName = "Tahoma";
            sht.Range("F2:H2").Style.Font.FontSize = 12;
            sht.Range("F2:H2").Style.NumberFormat.Format = "@";
            sht.Range("F2:H2").Style.Fill.SetBackgroundColor(XLColor.LightGreen);
            sht.Range("H2").Style.Alignment.WrapText = true;

            sht.Range("F2").Value = "Defect";
            sht.Range("G2").Value = "Defect Qty.";
            sht.Range("H2").Value = "Loom No.";
            //sht.Range("I2").Value = "Defect %";
            //sht.Range("J2").Value = "Cum %";
            var _With1 = sht.Range("F2:H2");
            _With1.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            _With1.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            _With1.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            _With1.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            // values
            ii = 3;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + ii).Value = ds.Tables[0].Rows[i]["Qualityname"];
                sht.Range("B" + ii).Value = ds.Tables[0].Rows[i]["Designname"];
                sht.Range("C" + ii).Value = ds.Tables[0].Rows[i]["Colorname"];
                sht.Range("D" + ii).Value = ds.Tables[0].Rows[i]["Sizemtr"];
                sht.Range("E" + ii).Value = ds.Tables[0].Rows[i]["Recqty"];
                //Defects
                ds.Tables[1].DefaultView.RowFilter = "Item_finished_id=" + ds.Tables[0].Rows[i]["Item_finished_id"] + "";
                DataView dv = ds.Tables[1].DefaultView;
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                    {
                        sht.Range("F" + ii).SetValue(ds1.Tables[0].Rows[j]["paraname"]);
                        sht.Range("G" + ii).SetValue(ds1.Tables[0].Rows[j]["Defectqty"]);
                        sht.Range("H" + ii).SetValue(ds1.Tables[0].Rows[j]["LoomNo"]);                     
                        
                        //sht.Range("H" + ii).SetValue(ds1.Tables[0].Rows[j]["cumulativeqty"]);
                        //defectperc = (Convert.ToDecimal(ds1.Tables[0].Rows[j]["Defectqty"]) * 100) / (Convert.ToDecimal(ds.Tables[0].Rows[i]["recqty"]));
                        //cumperc = (Convert.ToDecimal(ds1.Tables[0].Rows[j]["cumulativeqty"]) * 100) / (Convert.ToDecimal(ds.Tables[0].Rows[i]["recqty"]));
                        //sht.Range("I" + ii).SetValue(defectperc);
                        //sht.Range("J" + ii).SetValue(cumperc);
                        //sht.Range("I" + ii).Style.NumberFormat.Format = "0.00";
                        //sht.Range("J" + ii).Style.NumberFormat.Format = "0.00";
                        ii = ii + 1;
                    }
                }
                else
                {
                    ii = ii + 1;
                }
                //
                sht.Columns(1, 20).AdjustToContents();
                //sht.Column("A").Width = 14.71;
                //sht.Column("B").Width = 8.86;
                //sht.Column("C").Width = 18.00;
                //sht.Column("D").Width = 9.29;
                //sht.Column("E").Width = 10.43;                
                sht.Column("G").Width = 10.57;
                sht.Column("H").Width = 12.00;
                sht.Column("I").Width = 10.14;
                sht.Column("J").Width = 9.00;
            }
            //*********************************
            string Fileextension = "xlsx";
            string filename = validateFilename("QCREPORTWITHLOOMNO(" + DDprocess.SelectedItem.Text + ")-" + DateTime.Now + "." + Fileextension);
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
            //*****
            //File.Delete(Path);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
}
