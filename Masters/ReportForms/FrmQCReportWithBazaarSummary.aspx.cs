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

public partial class Masters_ReportForms_FrmQCReportWithBazaarSummary : System.Web.UI.Page
{
    public static string Export = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName                           
                           select UnitsId,UnitName from Units order by UnitName
                           select Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=cs.Categoryid and cs.id=0 and ICM.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @"
                           select Val,Type from Sizetype
                           select PROCESS_NAME_ID,PROCESS_NAME From PROCESS_NAME_MASTER Where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " and ProcessType=1 order by PROCESS_NAME";


            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 3, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDprocess, ds, 4, false, "--Plz Select--");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            ////Export
            //switch (variable.ReportWithpdf)
            //{
            //    case "1":
            //        chkexport.Visible = true;
            //        Export = "N";
            //        break;
            //    default:
            //        Export = "Y";
            //        chkexport.Visible = false;
            //        break;
            //}
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
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
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

        string stritem = "select distinct IM.Item_Id,IM.Item_Name from  Item_Parameter_Master IPM  inner Join Item_Master IM on IM.Item_Id=IPM.Item_Id inner join Item_Category_Master ICM on ICM.Category_Id=IM.Category_Id where  IM.Category_Id=" + DDCategory.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by IM.item_name";
        UtilityModule.ConditionalComboFill(ref ddItemName, stritem, true, "---Select Item----");
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(DDQuality, DDDesign, DDColor, DDShape, Convert.ToInt16(ddItemName.SelectedValue));
    }
    private void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, int Itemid)
    {
        string Str = @"SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + Itemid + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By QUALITYNAME
                     SELECT DESIGNID,DESIGNNAME from DESIGN Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By DESIGNNAME
                     SELECT COLORID,COLORNAME FROM COLOR Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By COLORNAME
                     SELECT SHAPEID,SHAPENAME FROM SHAPE Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By SHAPENAME
                     SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By SHADECOLORNAME";

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
                 Where shapeid=" + DDShape.SelectedValue + " And S.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by " + size + "";

        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--SELECT--");
        //

    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void DDProdunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDprocess.SelectedValue == "1")
        {

            UtilityModule.ConditionalComboFill(ref DDLoomNo, @"select  PM.UID,PM.loomno+'/'+IM.ITEM_NAME as LoomNo from ProductionLoomMaster PM 
                                            inner join ITEM_MASTER IM on PM.Itemid=IM.ITEM_ID                                            
                                            Where  PM.CompanyId=" + DDcompany.SelectedValue + " and PM.UnitId=" + DDProdunit.SelectedValue + " order by case when ISNUMERIC(PM.loomno)=1 Then CONVERT(int,PM.loomno) Else 9999999 End,PM.loomno", true, "--Plz Select--");
        }
        else
        {
            DDLoomNo.Items.Clear();
        }

    }
    protected void DDprocess_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDprocess.SelectedValue == "1")
        {
            TRLoom.Visible = true;
        }

        else
        {
            TRLoom.Visible = false;
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        if (chkexportwithdetail.Checked == true)
        {
            string str = "", FilterBy = "";
            str = "  and PRM.companyId=" + DDcompany.SelectedValue;
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
                if (Session["varMasterCompanyIDForERP"].ToString() == "21" && ddItemName.SelectedItem.Text == "MACHINE WOVEN")
                {
                    str = str + " and vf.item_id in(3,427,463)";
                }
                else
                {
                    str = str + " and vf.item_id=" + ddItemName.SelectedValue;
                    FilterBy = FilterBy + " and ItemName -" + ddItemName.SelectedItem.Text;
                }

                //str = str + " and vf.item_id=" + ddItemName.SelectedValue;
                //FilterBy = FilterBy + " and ItemName -" + ddItemName.SelectedItem.Text;
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
            SqlCommand cmd = new SqlCommand("PRO_QCREPORT_DETAIL_WITHBAZAARSUMMARY", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@where", str);
            cmd.Parameters.AddWithValue("@reportype", 0);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@toDate", txttodate.Text);
            cmd.Parameters.AddWithValue("@ProcessID", DDprocess.SelectedValue);
            //cmd.Parameters.AddWithValue("@WithDetail", chkexportwithdetail.Checked == true ? "1" : "0");

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                //if (chkexportwithdetail.Checked == true)
                //{
                QCReportDetailWithBazaarSummary(ds, FilterBy);
                return;
                // }          

            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
            }      
        }
        else
        {
            string str = "", FilterBy = "";
            str = "  and PRM.companyId=" + DDcompany.SelectedValue;
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
                if (Session["varMasterCompanyIDForERP"].ToString() == "21" && ddItemName.SelectedItem.Text == "MACHINE WOVEN")
                {
                    str = str + " and vf.item_id in(3,427,463)";
                    //str = str + " and vf.item_id=" + ddItemName.SelectedValue;
                }
                else
                {
                    str = str + " and vf.item_id=" + ddItemName.SelectedValue;
                    FilterBy = FilterBy + " and ItemName -" + ddItemName.SelectedItem.Text;
                }

                //str = str + " and vf.item_id=" + ddItemName.SelectedValue;
                //FilterBy = FilterBy + " and ItemName -" + ddItemName.SelectedItem.Text;
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
            SqlCommand cmd = new SqlCommand("PRO_QCREPORT_WITHBAZAARSUMMARY", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@where", str);
            cmd.Parameters.AddWithValue("@reportype", 0);
            cmd.Parameters.AddWithValue("@fromDate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@toDate", txttodate.Text);
            cmd.Parameters.AddWithValue("@ProcessID", DDprocess.SelectedValue);
            //cmd.Parameters.AddWithValue("@WithDetail", chkexportwithdetail.Checked == true ? "1" : "0");

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                //if (chkexportwithdetail.Checked == true)
                //{
                QCReportWithBazaarSummary(ds, FilterBy);
                return;
                // }          

            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
            }      
        }

    }

    private void QCReportWithBazaarSummary(DataSet ds, String FilterBy)
    {
        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("QCReportWithReceiveSummary_");

        sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
        sht.PageSetup.AdjustTo(83);
        sht.PageSetup.PaperSize = XLPaperSize.A4Paper;


        sht.PageSetup.Margins.Top = 1.21;
        sht.PageSetup.Margins.Left = 0.47;
        sht.PageSetup.Margins.Right = 0.36;
        sht.PageSetup.Margins.Bottom = 0.19;
        sht.PageSetup.Margins.Header = 1.20;
        sht.PageSetup.Margins.Footer = 0.3;
        sht.PageSetup.SetScaleHFWithDocument();


        //Export to excel
        GridView GridView1 = new GridView();
        GridView1.AllowPaging = false;

        GridView1.DataSource = ds;
        GridView1.DataBind();
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
         "attachment;filename=QCReportWithReceiveDetail" + DateTime.Now + ".xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        int columncount = GridView1.Rows[0].Cells.Count;

        ////Change the Header Row back to white color
        //GridView1.HeaderRow.Style.Add("background-color", "#FFFFFF");
        ////Applying stlye to gridview header cells

        if (GridView1.Rows.Count > 0)
        {

            for (int i = 0; i < GridView1.HeaderRow.Cells.Count; i++)
            {
                //GridView1.HeaderRow.Cells[i].Style.Add("background-color", "#df5015");
                GridView1.HeaderRow.Cells[i].Style.Add("width", "120px");
            }


            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                ////GridView1.Rows[i].Cells[1].Style.Add("NumberFormat", "d-mmm-yy");
                ////GridView1.Rows[i].Cells[1].Style.Add("FormatString", "{0:dd.MM.yyyy}");

                string strTemp = GridView1.Rows[i].Cells[2].Text;
                if (strTemp == "01-Jan-1900 00:00:00")
                {
                    GridView1.Rows[i].Cells[2].Text = "";
                }
                else
                {
                    GridView1.Rows[i].Cells[2].Text = Convert.ToDateTime(strTemp).ToString("dd-MM-yyyy");
                }

                if (GridView1.Rows[i].Cells[columncount - 1].Text != "")
                {
                    if (GridView1.Rows[i].Cells[columncount - 1].Text == "Repaired" || GridView1.Rows[i].Cells[columncount - 1].Text == "Rejected")
                    {
                        GridView1.Rows[i].Cells[columncount - 5].Text = GridView1.Rows[i].Cells[columncount - 1].Text;
                    }
                }

                if (GridView1.Rows[i].Cells[columncount - 2].Text != "")
                {                    
                    GridView1.Rows[i].Cells[columncount - 4].Text = GridView1.Rows[i].Cells[columncount - 2].Text;                    
                }

                //Apply text style to each Row
                GridView1.Rows[i].Attributes.Add("class", "textmode");
                GridView1.Rows[1].Style.Add("height", "80px");
            }

            string StrHeaderText = "";
            for (int i = 0; i < GridView1.HeaderRow.Cells.Count; i++)
            {
                if (GridView1.HeaderRow.Cells[i].Text == "RowType" || GridView1.HeaderRow.Cells[i].Text == "Status" || GridView1.HeaderRow.Cells[i].Text == "LastRowType")
                {
                    StrHeaderText = GridView1.HeaderRow.Cells[i].Text;

                    GridView1.HeaderRow.Cells[i].Visible = false;
                    //GridView1.Columns[i].Visible = false;
                }
            }

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                GridView1.Rows[i].Cells[columncount - 1].Visible = false;
                GridView1.Rows[i].Cells[columncount - 2].Visible = false;
                GridView1.Rows[i].Cells[columncount - 3].Visible = false;
            }
        }

        GridView1.RenderControl(hw);

        //style to format numbers to string
        string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        Response.Write(style);
        Response.Output.Write(sw.ToString());
        Response.Flush();
        Response.End();

        #region
            ////*************
            ////***********
            //sht.Row(1).Height = 24;
            //sht.Range("A1:W1").Merge();
            //sht.Range("A1:W1").Style.Font.FontSize = 10;
            //sht.Range("A1:W1").Style.Font.Bold = true;
            //sht.Range("A1:W1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //sht.Range("A1:W1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //sht.Range("A1:W1").Style.Alignment.WrapText = true;
            ////************
            //sht.Range("A1").SetValue("QC Report With Bazaar Summary From : " + txtfromdate.Text + " To : " + txttodate.Text + " " + FilterBy);

            //sht.Range("A2:W2").Style.Font.FontSize = 10;
            //sht.Range("A2:W2").Style.Font.Bold = true;

            //int row = 2;
            //int noofrows2 = 0;
            //int i2 = 0;

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    sht.Range("A" + row).Value = "Week";
            //    sht.Range("A" + row + ":A" + row).Style.Font.FontName = "Calibri";
            //    sht.Range("A" + row + ":A" + row).Style.Font.FontSize = 11;
            //    sht.Range("A" + row + ":A" + row).Style.Font.SetBold();
            //    //sht.Range("A" + row).Merge();
            //    sht.Range("A" + row + ":A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //    sht.Range("A" + row + ":A" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //    //sht.Range("A" + row + ":A" + row).Style.Fill.BackgroundColor = XLColor.LightGray;

            //    sht.Range("B" + row).Value = "Bazar Date";
            //    sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Calibri";
            //    sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 11;
            //    sht.Range("B" + row + ":B" + row).Style.Font.SetBold();
            //    //sht.Range("A" + row).Merge();
            //    sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //    sht.Range("B" + row + ":B" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //    //sht.Range("B" + row + ":B" + row).Style.Fill.BackgroundColor = XLColor.LightGray;

            //    int DynamiccolMain = 2;
            //    int DynamiccolstartMain = DynamiccolMain + 1;
            //    int DynamiccolendMain = 0;

            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        int row3 = row;
            //        int noofrows3 = 0;
            //        int i3 = 0;
            //        int Dynamiccol3 = 2;
            //        int Dynamiccolstart3 = Dynamiccol3 + 1;
            //        int Dynamiccolend3;
            //        int Totalcol3;
            //        string DesignName = "";

            //        DataTable dtdistinctdesign = ds.Tables[0].DefaultView.ToTable(true, "QualityName","DesignName","ColorName","Size","Item_Finished_Id");
            //        noofrows3 = dtdistinctdesign.Rows.Count;

            //        for (i3 = 0; i3 < noofrows3; i3++)
            //        {
            //            DesignName = "";
            //            if (i3 < noofrows3)
            //            {
            //                DesignName = ds.Tables[0].Rows[i3]["QualityName"].ToString() + " / " + ds.Tables[0].Rows[i3]["DesignName"].ToString() + " / " + ds.Tables[0].Rows[i3]["ColorName"].ToString() + " / " + ds.Tables[0].Rows[i3]["Size"].ToString();
            //            }
            //            Dynamiccol3 = Dynamiccol3 + 1;
            //            //sht.Range(sht.Cell(row3, Dynamiccol3), sht.Cell(row3, Dynamiccol3 + 1)).Merge();

            //            sht.Cell(row3, Dynamiccol3).Value = DesignName;
            //            sht.Cell(row3, Dynamiccol3).Style.Font.Bold = true;
            //            sht.Cell(row3, Dynamiccol3).Style.Font.FontName = "Calibri";
            //            sht.Cell(row3, Dynamiccol3).Style.Font.FontSize = 11;
            //            sht.Cell(row3, Dynamiccol3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //            sht.Cell(row3, Dynamiccol3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //            sht.Cell(row3, Dynamiccol3).Style.Alignment.WrapText = true;
            //            ////sht.Cell(row2, Dynamiccol2).Value = dtdistinct2.Rows[i2]["ReceiveDate"].ToString();
            //            //Dynamiccol3 = Dynamiccol3 + 1;

            //            //sht.Range("AA5:AB6").Merge();

            //        }
            //        Dynamiccolend3 = Dynamiccol3;

            //        Totalcol3 = Dynamiccolend3 + 1;

            //        //sht.Range(sht.Cell(row3, Totalcol3), sht.Cell(row3, Totalcol3 + 1)).Merge();

            //        sht.Cell(row3, Totalcol3).Value = "TOTAL";
            //        sht.Cell(row3, Totalcol3).Style.Font.Bold = true;
            //        sht.Cell(row3, Totalcol3).Style.Font.FontName = "Calibri";
            //        sht.Cell(row3, Totalcol3).Style.Font.FontSize = 11;
            //        sht.Cell(row3, Totalcol3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //        sht.Cell(row3, Totalcol3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //        sht.Cell(row3, Totalcol3).Style.Alignment.WrapText = true;

            //        Totalcol3 = Dynamiccolend3 + 1;

            //        //sht.Range(sht.Cell(row3, Totalcol3), sht.Cell(row3, Totalcol3 + 1)).Merge();

            //        sht.Cell(row3, Totalcol3).Value = "REPAIR QTY";
            //        sht.Cell(row3, Totalcol3).Style.Font.Bold = true;
            //        sht.Cell(row3, Totalcol3).Style.Font.FontName = "Calibri";
            //        sht.Cell(row3, Totalcol3).Style.Font.FontSize = 11;
            //        sht.Cell(row3, Totalcol3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //        sht.Cell(row3, Totalcol3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //        sht.Cell(row3, Totalcol3).Style.Alignment.WrapText = true;

            //        Totalcol3 = Dynamiccolend3 + 1;

            //        //sht.Range(sht.Cell(row3, Totalcol3), sht.Cell(row3, Totalcol3 + 1)).Merge();

            //        sht.Cell(row3, Totalcol3).Value = "DEFECT NAME & DATE";
            //        sht.Cell(row3, Totalcol3).Style.Font.Bold = true;
            //        sht.Cell(row3, Totalcol3).Style.Font.FontName = "Calibri";
            //        sht.Cell(row3, Totalcol3).Style.Font.FontSize = 11;
            //        sht.Cell(row3, Totalcol3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //        sht.Cell(row3, Totalcol3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //        sht.Cell(row3, Totalcol3).Style.Alignment.WrapText = true;

            //        Totalcol3 = Dynamiccolend3 + 1;

            //        //sht.Range(sht.Cell(row3, Totalcol3), sht.Cell(row3, Totalcol3 + 1)).Merge();

            //        sht.Cell(row3, Totalcol3).Value = "REMOVE DEFECT & DATE";
            //        sht.Cell(row3, Totalcol3).Style.Font.Bold = true;
            //        sht.Cell(row3, Totalcol3).Style.Font.FontName = "Calibri";
            //        sht.Cell(row3, Totalcol3).Style.Font.FontSize = 11;
            //        sht.Cell(row3, Totalcol3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //        sht.Cell(row3, Totalcol3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //        sht.Cell(row3, Totalcol3).Style.Alignment.WrapText = true;

            //        Totalcol3 = Dynamiccolend3 + 1;

            //        //sht.Range(sht.Cell(row3, Totalcol3), sht.Cell(row3, Totalcol3 + 1)).Merge();

            //        sht.Cell(row3, Totalcol3).Value = "DEFECT STATUS";
            //        sht.Cell(row3, Totalcol3).Style.Font.Bold = true;
            //        sht.Cell(row3, Totalcol3).Style.Font.FontName = "Calibri";
            //        sht.Cell(row3, Totalcol3).Style.Font.FontSize = 11;
            //        sht.Cell(row3, Totalcol3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //        sht.Cell(row3, Totalcol3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            //        sht.Cell(row3, Totalcol3).Style.Alignment.WrapText = true;

            //        DynamiccolendMain = Totalcol3;
            //    }
            //}
            //row = row + 1;        

            ////ds1.Dispose();

            //ds.Dispose();
            ////*************************************************
            //using (var a = sht.Range("A1" + ":W" + row))
            //{
            //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //}

            ////*************************************************
            //String Path;
            ////sht.Columns(1, 36).AdjustToContents();
            //string Fileextension = "xlsx";
            //string filename = UtilityModule.validateFilename("QCReportWithBazaarSummary_" + DateTime.Now + "." + Fileextension);
            //Path = Server.MapPath("~/Tempexcel/" + filename);
            //xapp.SaveAs(Path);
            //xapp.Dispose();
            ////Download File
            //Response.ClearContent();
            //Response.ClearHeaders();
            //// Response.Clear();
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            //Response.WriteFile(Path);
            //// File.Delete(Path);
            //Response.End();
        #endregion

       
    }
    private void QCReportDetailWithBazaarSummary(DataSet ds, String FilterBy)
    {
        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("QCReportDetailWithRecSummary_");

        sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
        sht.PageSetup.AdjustTo(83);
        sht.PageSetup.PaperSize = XLPaperSize.A4Paper;


        sht.PageSetup.Margins.Top = 1.21;
        sht.PageSetup.Margins.Left = 0.47;
        sht.PageSetup.Margins.Right = 0.36;
        sht.PageSetup.Margins.Bottom = 0.19;
        sht.PageSetup.Margins.Header = 1.20;
        sht.PageSetup.Margins.Footer = 0.3;
        sht.PageSetup.SetScaleHFWithDocument();


        //Export to excel
        GridView GridView1 = new GridView();
        GridView1.AllowPaging = false;

        GridView1.DataSource = ds;
        GridView1.DataBind();
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
         "attachment;filename=QCReportDetailWithRecSummary" + DateTime.Now + ".xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        int columncount = GridView1.Rows[0].Cells.Count;

        ////Change the Header Row back to white color
        //GridView1.HeaderRow.Style.Add("background-color", "#FFFFFF");
        ////Applying stlye to gridview header cells

        if (GridView1.Rows.Count > 0)
        {

            for (int i = 0; i < GridView1.HeaderRow.Cells.Count; i++)
            {
                //GridView1.HeaderRow.Cells[i].Style.Add("background-color", "#df5015");
                GridView1.HeaderRow.Cells[i].Style.Add("width", "120px");
            }


            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                ////GridView1.Rows[i].Cells[1].Style.Add("NumberFormat", "d-mmm-yy");
                ////GridView1.Rows[i].Cells[1].Style.Add("FormatString", "{0:dd.MM.yyyy}");

                string strTemp = GridView1.Rows[i].Cells[2].Text;
                if (strTemp == "01-Jan-1900 00:00:00")
                {
                    GridView1.Rows[i].Cells[2].Text = "";
                }
                else
                {
                    GridView1.Rows[i].Cells[2].Text = Convert.ToDateTime(strTemp).ToString("dd-MM-yyyy");
                }

                if (GridView1.Rows[i].Cells[columncount - 1].Text != "")
                {
                    if (GridView1.Rows[i].Cells[columncount - 1].Text == "Repaired" || GridView1.Rows[i].Cells[columncount - 1].Text == "Rejected")
                    {
                        GridView1.Rows[i].Cells[columncount - 8].Text = GridView1.Rows[i].Cells[columncount - 1].Text;
                    }
                }

                if (GridView1.Rows[i].Cells[columncount - 4].Text != "")
                {
                    GridView1.Rows[i].Cells[columncount - 7].Text = GridView1.Rows[i].Cells[columncount - 4].Text;
                }

                if (GridView1.Rows[i].Cells[columncount - 3].Text != "")
                {
                    GridView1.Rows[i].Cells[columncount - 6].Text = GridView1.Rows[i].Cells[columncount - 3].Text;
                }

                if (GridView1.Rows[i].Cells[columncount - 2].Text != "")
                {
                    GridView1.Rows[i].Cells[columncount - 5].Text = GridView1.Rows[i].Cells[columncount - 2].Text;
                }

                //Apply text style to each Row
                GridView1.Rows[i].Attributes.Add("class", "textmode");
                GridView1.Rows[1].Style.Add("height", "80px");
            }

            string StrHeaderText = "";
            for (int i = 0; i < GridView1.HeaderRow.Cells.Count; i++)
            {
                if (GridView1.HeaderRow.Cells[i].Text == "RowType" || GridView1.HeaderRow.Cells[i].Text == "Status" || GridView1.HeaderRow.Cells[i].Text == "LastRowType" || GridView1.HeaderRow.Cells[i].Text == "Defect_SrNo")
                {
                    StrHeaderText = GridView1.HeaderRow.Cells[i].Text;

                    GridView1.HeaderRow.Cells[i].Visible = false;
                    //GridView1.Columns[i].Visible = false;
                }
            }

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                GridView1.Rows[i].Cells[columncount - 1].Visible = false;
                GridView1.Rows[i].Cells[columncount - 2].Visible = false;
                GridView1.Rows[i].Cells[columncount - 3].Visible = false;
                GridView1.Rows[i].Cells[columncount - 4].Visible = false;
            }
        }

        GridView1.RenderControl(hw);

        //style to format numbers to string
        string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        Response.Write(style);
        Response.Output.Write(sw.ToString());
        Response.Flush();
        Response.End();       


    }
    
}