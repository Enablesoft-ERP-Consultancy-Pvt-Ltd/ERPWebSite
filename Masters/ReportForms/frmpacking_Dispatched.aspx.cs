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

public partial class Masters_ReportForms_frmpacking_Dispatched : System.Web.UI.Page
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
                           select Val,Type from Sizetype";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 2, false, "--Plz Select--");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            if (Session["varCompanyNo"].ToString() == "21")
            {
                ChkForWithStockNo.Visible = true;
                TRDispatchWithRawDetail.Visible = true;
                TRDispatchWithFinishingProcessRawDetail.Visible = true;
            }

            if (Session["varCompanyNo"].ToString() == "14")
            {
                ChkForWithStockNo.Visible = true;
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
        if (RDpacking.Checked == true)
        {
            if (ChkForWithStockNo.Checked == true)
            {
                PackingWithStockNoReport();
            }
            else
            {
                Packingreport();
            }            
        }
        if (Rdtobedispatch.Checked == true)
        {
            if (Session["varCompanyId"].ToString() == "21")
            {
                TObeDispatchedKaysons();
            }
            else
            {
                TObeDispatched();
            }
            
        }
        else if (RDdispatch.Checked == true)
        {
            if (ChkForWithStockNo.Checked == true)
            {
                DispatchedWithStockNoReport();
            }
            else
            {
                Dispatchedreport();
            }           
        }
        else if (RDDispatchWithRawDetail.Checked== true)
        {           
            DispatchedWithRawDetailReport();                       
        }
        else if (RDDispatchWithFinishingProcessRawDetail.Checked == true)
        {
            DispatchedWithFinishingProcessRawDetailReport();
        }

    }
    protected void Dispatchedreport()
    {
        string where = "", Filterby = "";
        lblmsg.Text = "";

        if (ddItemName.SelectedIndex > 0)
        {
            where = where + " and DPD.Itemid=" + ddItemName.SelectedValue;
            Filterby = "ItemName-" + ddItemName.SelectedItem.Text;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and Q.qualityid=" + DDQuality.SelectedValue;
            Filterby = Filterby + " Quality-" + DDQuality.SelectedItem.Text;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and D.Designid=" + DDDesign.SelectedValue;
            Filterby = Filterby + " Design-" + DDDesign.SelectedItem.Text;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and C.colorid=" + DDColor.SelectedValue;
            Filterby = Filterby + " Color-" + DDColor.SelectedItem.Text;
        }
        if (DDShape.SelectedIndex > 0)
        {
            where = where + " and DPD.shapeid=" + DDShape.SelectedValue;
            Filterby = Filterby + " Shape-" + DDShape.SelectedItem.Text;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and DPD.sizeid=" + DDSize.SelectedValue;
            Filterby = Filterby + " Size-" + DDSize.SelectedItem.Text;
        }
        if (DDpacktype.SelectedIndex > 0)
        {
            where = where + " and DPD.Packtypeid=" + DDpacktype.SelectedValue;
            Filterby = Filterby + " Packtype-" + DDpacktype.SelectedItem.Text;
        }
        if (DDarticleno.SelectedIndex > 0)
        {
            where = where + " and DPD.ArticleNo='" + DDarticleno.SelectedValue+"'";
            Filterby = Filterby + " ArticleNo-" + DDarticleno.SelectedItem.Text;
        }
        Filterby = Filterby + " From- " + txtfromdate.Text + " To- " + txttodate.Text;
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
        param[1] = new SqlParameter("@where", where);
        param[2] = new SqlParameter("@Fromdate", txtfromdate.Text);
        param[3] = new SqlParameter("@Todate", txttodate.Text);
        //*************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetDispatchedReport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Dispatched");
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:K1").Merge();
            sht.Range("A1:K1").Style.Font.FontSize = 10;
            sht.Range("A1:K1").Style.Font.Bold = true;
            sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:K1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:K1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("DISPATCHED - " + Filterby);
            //Detail headers                
            sht.Range("A2:K2").Style.Font.FontSize = 10;
            sht.Range("A2:K2").Style.Font.Bold = true;
            sht.Range("J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            using (var a = sht.Range("A2:K2"))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            sht.Range("A2").Value = "DATE";
            sht.Range("B2").Value = "BATCH NO";
            sht.Range("C2").Value = "ECISNO";
            sht.Range("D2").Value = "DEST";
            sht.Range("E2").Value = "ARTDESC";
            sht.Range("F2").Value = "ARTICLENO";
            sht.Range("G2").Value = "PACKTYPE";
            sht.Range("H2").Value = "DTSTAMP";
            sht.Range("I2").Value = "PONO";
            sht.Range("J2").Value = "TOTAL";
            sht.Range("K2").Value = "PD";

            //********
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "Date,ECISNO";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            //
            int row = 3;
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":J" + row).Style.Font.FontSize = 10;
                sht.Range("I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                using (var a = sht.Range("A" + row + ":J" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Date"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["BatchNo"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Ecisno"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Dest"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Articledesc"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Articleno"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["packingtype"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["dtstamp"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["PONo"]);
                sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);
                sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["PD"]);

                row = row + 1;
            }
            ds.Dispose();
            ds1.Dispose();

            //**************grand Total
            var Total = sht.Evaluate("SUM(J3:J" + (row - 1) + ")");
            sht.Range("J" + row).Value = Total;
            sht.Range("J" + row).Style.Font.Bold = true;

            var ToalPD = sht.Evaluate("SUM(K3:K" + (row - 1) + ")");
            sht.Range("K" + row).Value = ToalPD;
            sht.Range("K" + row).Style.Font.Bold = true;
            //************** Save
            String Path;
            sht.Columns(1, 20).AdjustToContents();

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("DISPATCHEDREPORT_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "al2", "alert('No records found')", true);
        }
    }
    protected void DispatchedWithStockNoReport()
    {
        string where = "", Filterby = "";
        lblmsg.Text = "";

        if (ddItemName.SelectedIndex > 0)
        {
            where = where + " and DPD.Itemid=" + ddItemName.SelectedValue;
            Filterby = "ItemName-" + ddItemName.SelectedItem.Text;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and Q.qualityid=" + DDQuality.SelectedValue;
            Filterby = Filterby + " Quality-" + DDQuality.SelectedItem.Text;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and D.Designid=" + DDDesign.SelectedValue;
            Filterby = Filterby + " Design-" + DDDesign.SelectedItem.Text;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and C.colorid=" + DDColor.SelectedValue;
            Filterby = Filterby + " Color-" + DDColor.SelectedItem.Text;
        }
        if (DDShape.SelectedIndex > 0)
        {
            where = where + " and DPD.shapeid=" + DDShape.SelectedValue;
            Filterby = Filterby + " Shape-" + DDShape.SelectedItem.Text;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and DPD.sizeid=" + DDSize.SelectedValue;
            Filterby = Filterby + " Size-" + DDSize.SelectedItem.Text;
        }
        if (DDpacktype.SelectedIndex > 0)
        {
            where = where + " and DPD.Packtypeid=" + DDpacktype.SelectedValue;
            Filterby = Filterby + " Packtype-" + DDpacktype.SelectedItem.Text;
        }
        if (DDarticleno.SelectedIndex > 0)
        {
            where = where + " and DPD.ArticleNo='" + DDarticleno.SelectedValue + "'";
            Filterby = Filterby + " ArticleNo-" + DDarticleno.SelectedItem.Text;
        }
        Filterby = Filterby + " From- " + txtfromdate.Text + " To- " + txttodate.Text;

        DataSet ds = new DataSet();

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_GetDispatchedReportWithStockNo", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@companyId", DDcompany.SelectedValue);
        cmd.Parameters.AddWithValue("@where", where);
        cmd.Parameters.AddWithValue("@Fromdate", txtfromdate.Text);
        cmd.Parameters.AddWithValue("@Todate", txttodate.Text);      
        
        // DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        ////SqlParameter[] param = new SqlParameter[4];
        ////param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
        ////param[1] = new SqlParameter("@where", where);
        ////param[2] = new SqlParameter("@Fromdate", txtfromdate.Text);
        ////param[3] = new SqlParameter("@Todate", txttodate.Text);
        //////*************
        ////DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetDispatchedReportWithStockNo", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("DispatchedWithStockNo");
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:K1").Merge();
            sht.Range("A1:K1").Style.Font.FontSize = 10;
            sht.Range("A1:K1").Style.Font.Bold = true;
            sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:K1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:K1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("DISPATCHED - " + Filterby);
            //Detail headers                
            sht.Range("A2:K2").Style.Font.FontSize = 10;
            sht.Range("A2:K2").Style.Font.Bold = true;
            //sht.Range("J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            using (var a = sht.Range("A2:K2"))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            sht.Range("A2").Value = "DATE";
            sht.Range("B2").Value = "BATCH NO";
            sht.Range("C2").Value = "ECISNO";
            sht.Range("D2").Value = "DEST";
            sht.Range("E2").Value = "CARPET NO";
            sht.Range("F2").Value = "ARTDESC";
            sht.Range("G2").Value = "ARTICLENO";
            sht.Range("H2").Value = "PACKTYPE";
            sht.Range("I2").Value = "DTSTAMP";
            sht.Range("J2").Value = "PONO";
            sht.Range("K2").Value = "Qty";
            //sht.Range("K2").Value = "PD";

            //********
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "Date,ECISNO";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            //
            int row = 3;
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":J" + row).Style.Font.FontSize = 10;
                sht.Range("I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                using (var a = sht.Range("A" + row + ":J" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Date"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["BatchNo"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Ecisno"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Dest"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["TStockNo"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Articledesc"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Articleno"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["packingtype"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["dtstamp"]);
                sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["PONo"]);
                sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);                

                row = row + 1;
            }
            ds.Dispose();
            ds1.Dispose();

            //**************grand Total
            var Total = sht.Evaluate("SUM(K3:K" + (row - 1) + ")");
            sht.Range("K" + row).Value = Total;
            sht.Range("K" + row).Style.Font.Bold = true;

            //var ToalPD = sht.Evaluate("SUM(K3:K" + (row - 1) + ")");
            //sht.Range("K" + row).Value = ToalPD;
            //sht.Range("K" + row).Style.Font.Bold = true;
            //************** Save
            String Path;
            sht.Columns(1, 20).AdjustToContents();

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("DISPATCHEDWITHSTOCKNOREPORT_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "al2", "alert('No records found')", true);
        }
    }
    protected void PackingWithStockNoReport()
    {
        string where = "", Filterby = "";
        lblmsg.Text = "";

        if (ddItemName.SelectedIndex > 0)
        {
            where = where + " and vf.Item_id=" + ddItemName.SelectedValue;
            Filterby = "ItemName-" + ddItemName.SelectedItem.Text;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and vf.qualityid=" + DDQuality.SelectedValue;
            Filterby = Filterby + " Quality-" + DDQuality.SelectedItem.Text;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and vf.Designid=" + DDDesign.SelectedValue;
            Filterby = Filterby + " Design-" + DDDesign.SelectedItem.Text;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and vf.colorid=" + DDColor.SelectedValue;
            Filterby = Filterby + " Color-" + DDColor.SelectedItem.Text;
        }
        if (DDShape.SelectedIndex > 0)
        {
            where = where + " and vf.shapeid=" + DDShape.SelectedValue;
            Filterby = Filterby + " Shape-" + DDShape.SelectedItem.Text;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and vf.sizeid=" + DDSize.SelectedValue;
            Filterby = Filterby + " Size-" + DDSize.SelectedItem.Text;
        }
        if (DDpacktype.SelectedIndex > 0)
        {
            where = where + " and PRD.PackingTypeid=" + DDpacktype.SelectedValue;
            Filterby = Filterby + " Packtype-" + DDpacktype.SelectedItem.Text;
        }
        if (DDarticleno.SelectedIndex > 0)
        {
            where = where + " and PRD.ArticleNo='" + DDarticleno.SelectedValue + "'";
            Filterby = Filterby + " ArticleNo-" + DDarticleno.SelectedItem.Text;
        }
        Filterby = Filterby + " From- " + txtfromdate.Text + " To- " + txttodate.Text;

        DataSet ds = new DataSet();

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_GetpackingReportWithStockNo", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@companyId", DDcompany.SelectedValue);
        cmd.Parameters.AddWithValue("@where", where);
        cmd.Parameters.AddWithValue("@Fromdate", txtfromdate.Text);
        cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
        cmd.Parameters.AddWithValue("@Batchno", txtbatchno.Text);

        // DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        ////SqlParameter[] param = new SqlParameter[4];
        ////param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
        ////param[1] = new SqlParameter("@where", where);
        ////param[2] = new SqlParameter("@Fromdate", txtfromdate.Text);
        ////param[3] = new SqlParameter("@Todate", txttodate.Text);
        //////*************
        ////DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetDispatchedReportWithStockNo", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("PackingWithStockNo");
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:H1").Merge();
            sht.Range("A1:H1").Style.Font.FontSize = 10;
            sht.Range("A1:H1").Style.Font.Bold = true;
            sht.Range("A1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:H1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:H1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("PACKING - " + Filterby);
            //Detail headers                
            sht.Range("A2:H2").Style.Font.FontSize = 10;
            sht.Range("A2:H2").Style.Font.Bold = true;
            //sht.Range("J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            using (var a = sht.Range("A2:H2"))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            sht.Range("A2").Value = "RECEIVE DATE";
            sht.Range("B2").Value = "BATCH NO";
            sht.Range("C2").Value = "CARPET NO";
            sht.Range("D2").Value = "ARTDESC";
            sht.Range("E2").Value = "ARTICLENO";
            sht.Range("F2").Value = "PACKTYPE";
            sht.Range("G2").Value = "DTSTAMP";
            sht.Range("H2").Value = "Qty";           
           

            //********
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "ReceiveDate";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            //
            int row = 3;
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":H" + row).Style.Font.FontSize = 10;
                //sht.Range("I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                using (var a = sht.Range("A" + row + ":H" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["ReceiveDate"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["BatchNo"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["TStockNo"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Articledesc"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Articleno"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["PackingType"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Date_Stamp"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);
               

                row = row + 1;
            }
            ds.Dispose();
            ds1.Dispose();

            //**************grand Total
            var Total = sht.Evaluate("SUM(H3:H" + (row - 1) + ")");
            sht.Range("H" + row).Value = Total;
            sht.Range("H" + row).Style.Font.Bold = true;

            //var ToalPD = sht.Evaluate("SUM(K3:K" + (row - 1) + ")");
            //sht.Range("K" + row).Value = ToalPD;
            //sht.Range("K" + row).Style.Font.Bold = true;
            //************** Save
            String Path;
            sht.Columns(1, 20).AdjustToContents();

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PACKINGWITHSTOCKNOREPORT_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "al2", "alert('No records found')", true);
        }
    }
    protected void DDSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Trpacktype.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDpacktype, "select ID,Packingtype From packingtype order by PackingType", true, "--Plz Select--");
        }
    }
    protected void DDpacktype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Trarticleno.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDarticleno, "select Distinct ArticleNo,ArticleNo as articleno1 From Packingarticle Where QualityId=" + DDQuality.SelectedValue + " and Designid=" + DDDesign.SelectedValue + " and Colorid=" + DDColor.SelectedValue + " and sizeid=" + DDSize.SelectedValue + " and Packingtypeid=" + DDpacktype.SelectedValue + " order by articleno1", true, "--Plz Select--");
        }
    }
    protected void TObeDispatched()
    {
        string where = "";

        if (ddItemName.SelectedIndex > 0)
        {
            where = where + " and PLD.Itemid=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and PLD.qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and PLD.Designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and PLD.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            where = where + " and PLD.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and PLD.sizeid=" + DDSize.SelectedValue;
        }
        if (DDpacktype.SelectedIndex > 0)
        {
            where = where + " and PLD.Packtypeid=" + DDpacktype.SelectedValue;
        }
        if (DDarticleno.SelectedIndex > 0)
        {
            where = where + " and PLD.ArticleNo='" + DDarticleno.SelectedValue+"'";
        }


        DataSet ds = new DataSet();

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_GetTobedispatchReport", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@Where", where);        
        cmd.Parameters.AddWithValue("@Fromdate", txtfromdate.Text);
        cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
        cmd.Parameters.AddWithValue("@BatchNo", txtbatchno.Text);
        cmd.Parameters.AddWithValue("@Tobedispatched", Rdtobedispatch.Checked == true ? 1 : 0);     

        // DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();


        //SqlParameter[] param = new SqlParameter[5];
        //param[0] = new SqlParameter("@Where", where);
        //param[1] = new SqlParameter("@FromDate", txtfromdate.Text);
        //param[2] = new SqlParameter("@Todate", txttodate.Text);
        //param[3] = new SqlParameter("@Batchno", txtbatchno.Text);
        //param[4] = new SqlParameter("@Tobedispatched", Rdtobedispatch.Checked == true ? 1 : 0);
        ////*************
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetTobedispatchReport", param);
        ////Export to excel


        if (ds.Tables.Count > 0)
        {
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;

            GridView1.DataSource = ds;
            GridView1.DataBind();
            lblmsg.Text = "Wait....";
            Response.Clear();
            Response.Buffer = true;
            String Filename;
            if (Rdtobedispatch.Checked == true)
            {
                Filename = "TobeDispatched up to- " + txtfromdate.Text + " " + DateTime.Now + ".xls";
            }
            else
            {
                Filename = "PackedReport-" + DateTime.Now + ".xls";
            }
            Response.AddHeader("content-disposition",
             "attachment;filename=" + Filename);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                //Apply text style to each Row
                GridView1.Rows[i].Attributes.Add("class", "textmode");
            }
            GridView1.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            lblmsg.Text = "Done.....";
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt4", "alert('No Record found')", true);
        }
    }
    protected void Packingreport()
    {
        string where = "";
        lblmsg.Text = "";
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
        if (DDpacktype.SelectedIndex > 0)
        {
            where = where + " and PRD.PackingTypeid=" + DDpacktype.SelectedValue;
        }
        if (DDarticleno.SelectedIndex > 0)
        {
            where = where + " and PRD.ArticleNo= '"+ DDarticleno.SelectedValue+"'";
        }
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@Where", where);
        param[1] = new SqlParameter("@FromDate", txtfromdate.Text);
        param[2] = new SqlParameter("@Todate", txttodate.Text);
        param[3] = new SqlParameter("@Batchno", txtbatchno.Text);
        //*************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetpackingReport", param);
        //Export to excel
        if (ds.Tables.Count > 0)
        {
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;

            GridView1.DataSource = ds;
            GridView1.DataBind();
            lblmsg.Text = "Wait....";
            Response.Clear();
            Response.Buffer = true;
            String Filename;

            Filename = "PackingReport-From-" + txtfromdate.Text + " To-" + txttodate.Text + " " + DateTime.Now + ".xls";

            Response.AddHeader("content-disposition",
             "attachment;filename=" + Filename);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                //Apply text style to each Row
                GridView1.Rows[i].Attributes.Add("class", "textmode");
            }
            GridView1.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            lblmsg.Text = "Done.....";
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt3", "alert('No Record found')", true);
        }
    }

    protected void DispatchedWithRawDetailReport()
    {
        string where = "", Filterby = "";
        lblmsg.Text = "";

        if (ddItemName.SelectedIndex > 0)
        {
            where = where + " and DPD.Itemid=" + ddItemName.SelectedValue;
            Filterby = "ItemName-" + ddItemName.SelectedItem.Text;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and Q.qualityid=" + DDQuality.SelectedValue;
            Filterby = Filterby + " Quality-" + DDQuality.SelectedItem.Text;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and D.Designid=" + DDDesign.SelectedValue;
            Filterby = Filterby + " Design-" + DDDesign.SelectedItem.Text;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and C.colorid=" + DDColor.SelectedValue;
            Filterby = Filterby + " Color-" + DDColor.SelectedItem.Text;
        }
        if (DDShape.SelectedIndex > 0)
        {
            where = where + " and DPD.shapeid=" + DDShape.SelectedValue;
            Filterby = Filterby + " Shape-" + DDShape.SelectedItem.Text;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and DPD.sizeid=" + DDSize.SelectedValue;
            Filterby = Filterby + " Size-" + DDSize.SelectedItem.Text;
        }
        if (DDpacktype.SelectedIndex > 0)
        {
            where = where + " and DPD.Packtypeid=" + DDpacktype.SelectedValue;
            Filterby = Filterby + " Packtype-" + DDpacktype.SelectedItem.Text;
        }
        if (DDarticleno.SelectedIndex > 0)
        {
            where = where + " and DPD.ArticleNo='" + DDarticleno.SelectedValue + "'";
            Filterby = Filterby + " ArticleNo-" + DDarticleno.SelectedItem.Text;
        }
        Filterby = Filterby + " From- " + txtfromdate.Text + " To- " + txttodate.Text;

        DataSet ds = new DataSet();

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_GetDispatchedReportWithRawDetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@companyId", DDcompany.SelectedValue);
        cmd.Parameters.AddWithValue("@where", where);
        cmd.Parameters.AddWithValue("@Fromdate", txtfromdate.Text);
        cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
        cmd.Parameters.AddWithValue("@BatchNo", txtbatchno.Text);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyNo"]);
        cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);

        // DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        ////SqlParameter[] param = new SqlParameter[4];
        ////param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
        ////param[1] = new SqlParameter("@where", where);
        ////param[2] = new SqlParameter("@Fromdate", txtfromdate.Text);
        ////param[3] = new SqlParameter("@Todate", txttodate.Text);
        //////*************
        ////DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetDispatchedReportWithStockNo", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("DispatchedWithRawDetail");
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:P1").Merge();
            sht.Range("A1:P1").Style.Font.FontSize = 10;
            sht.Range("A1:P1").Style.Font.Bold = true;
            sht.Range("A1:P1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:P1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:P1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("DISPATCHED - " + Filterby);
            //Detail headers                
            sht.Range("A2:P2").Style.Font.FontSize = 10;
            sht.Range("A2:P2").Style.Font.Bold = true;
            //sht.Range("J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            using (var a = sht.Range("A2:P2"))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            sht.Range("A2").Value = "DATE";
            sht.Range("B2").Value = "BATCH NO";
            sht.Range("C2").Value = "ECISNO";
            sht.Range("D2").Value = "DEST";
            sht.Range("E2").Value = "ARTDESC";
            sht.Range("F2").Value = "ARTICLENO";
            sht.Range("G2").Value = "PACKTYPE";
            sht.Range("H2").Value = "DTSTAMP";
            sht.Range("I2").Value = "PONO";
            sht.Range("J2").Value = "Qty";
            sht.Range("K2").Value = "IIEM NAME";
            sht.Range("L2").Value = "QUALITY";
            sht.Range("M2").Value = "SHADECOLOR";
            sht.Range("N2").Value = "LOTNO";
            sht.Range("O2").Value = "TAGNO";
            sht.Range("P2").Value = "CONSUMP QTY";
            //sht.Range("K2").Value = "PD";

            //********
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "Date,ECISNO";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            //
            int row = 3;
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":P" + row).Style.Font.FontSize = 10;
                sht.Range("I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                using (var a = sht.Range("A" + row + ":P" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Date"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["BatchNo"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Ecisno"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Dest"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Articledesc"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Articleno"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["packingtype"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["dtstamp"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["PONo"]);
                sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);               

                row = row + 1;

                //DataTable dtdistinct8 = ds.Tables[1].Select("SRNo='" + ds1.Tables[0].Rows[i]["SRNo"] + "' ");

                //DataView dv2 = ds.Tables[0].DefaultView;
                //dv.Sort = "Date,ECISNO";
                //DataSet ds2 = new DataSet();
                //ds1.Tables.Add(dv.ToTable());

                DataRow[] foundRows;
                foundRows = ds.Tables[1].Select("SRNo='" + ds1.Tables[0].Rows[i]["SRNo"] + "' ");
                if (foundRows.Length > 0)
                {
                    foreach (DataRow row4 in foundRows)
                    {
                        sht.Range("K" + row).SetValue(row4["ItemName"].ToString());
                        sht.Range("L" + row).SetValue(row4["QualityName"].ToString());
                        sht.Range("M" + row).SetValue(row4["ShadeColorName"].ToString());
                        sht.Range("N" + row).SetValue(row4["LotNo"].ToString());
                        sht.Range("O" + row).SetValue(row4["TagNo"].ToString());
                        sht.Range("P" + row).SetValue(row4["TotalPcsLagat"]);
                        row = row + 1;
                    }
                }

                row = row + 1;

            }
            ds.Dispose();
            ds1.Dispose();

            ////**************grand Total
            //var Total = sht.Evaluate("SUM(K3:K" + (row - 1) + ")");
            //sht.Range("K" + row).Value = Total;
            //sht.Range("K" + row).Style.Font.Bold = true;

            //var ToalPD = sht.Evaluate("SUM(K3:K" + (row - 1) + ")");
            //sht.Range("K" + row).Value = ToalPD;
            //sht.Range("K" + row).Style.Font.Bold = true;
            //************** Save
            String Path;
            sht.Columns(1, 20).AdjustToContents();

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("DISPATCHEDREPORTWITHRAWDETAIL_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "al2", "alert('No records found')", true);
        }
    }
    protected void DispatchedWithFinishingProcessRawDetailReport()
    {
        string where = "", Filterby = "";
        lblmsg.Text = "";

        if (ddItemName.SelectedIndex > 0)
        {
            where = where + " and DPD.Itemid=" + ddItemName.SelectedValue;
            Filterby = "ItemName-" + ddItemName.SelectedItem.Text;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and Q.qualityid=" + DDQuality.SelectedValue;
            Filterby = Filterby + " Quality-" + DDQuality.SelectedItem.Text;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and D.Designid=" + DDDesign.SelectedValue;
            Filterby = Filterby + " Design-" + DDDesign.SelectedItem.Text;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and C.colorid=" + DDColor.SelectedValue;
            Filterby = Filterby + " Color-" + DDColor.SelectedItem.Text;
        }
        if (DDShape.SelectedIndex > 0)
        {
            where = where + " and DPD.shapeid=" + DDShape.SelectedValue;
            Filterby = Filterby + " Shape-" + DDShape.SelectedItem.Text;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and DPD.sizeid=" + DDSize.SelectedValue;
            Filterby = Filterby + " Size-" + DDSize.SelectedItem.Text;
        }
        if (DDpacktype.SelectedIndex > 0)
        {
            where = where + " and DPD.Packtypeid=" + DDpacktype.SelectedValue;
            Filterby = Filterby + " Packtype-" + DDpacktype.SelectedItem.Text;
        }
        if (DDarticleno.SelectedIndex > 0)
        {
            where = where + " and DPD.ArticleNo='" + DDarticleno.SelectedValue + "'";
            Filterby = Filterby + " ArticleNo-" + DDarticleno.SelectedItem.Text;
        }
        Filterby = Filterby + " From- " + txtfromdate.Text + " To- " + txttodate.Text;

        DataSet ds = new DataSet();

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_GetDispatchedReportWithFinshingProcessRawDetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@companyId", DDcompany.SelectedValue);
        cmd.Parameters.AddWithValue("@where", where);
        cmd.Parameters.AddWithValue("@Fromdate", txtfromdate.Text);
        cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
        cmd.Parameters.AddWithValue("@BatchNo", txtbatchno.Text);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyNo"]);
        cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);

        // DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        ////SqlParameter[] param = new SqlParameter[4];
        ////param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
        ////param[1] = new SqlParameter("@where", where);
        ////param[2] = new SqlParameter("@Fromdate", txtfromdate.Text);
        ////param[3] = new SqlParameter("@Todate", txttodate.Text);
        //////*************
        ////DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetDispatchedReportWithStockNo", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("DispatchedWithRawDetail");
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:P1").Merge();
            sht.Range("A1:P1").Style.Font.FontSize = 10;
            sht.Range("A1:P1").Style.Font.Bold = true;
            sht.Range("A1:P1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:P1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:P1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("DISPATCHED - " + Filterby);
            //Detail headers                
            sht.Range("A2:P2").Style.Font.FontSize = 10;
            sht.Range("A2:P2").Style.Font.Bold = true;
            //sht.Range("J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            using (var a = sht.Range("A2:P2"))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            sht.Range("A2").Value = "DATE";
            sht.Range("B2").Value = "BATCH NO";
            sht.Range("C2").Value = "ECISNO";
            sht.Range("D2").Value = "DEST";
            sht.Range("E2").Value = "ARTDESC";
            sht.Range("F2").Value = "ARTICLENO";
            sht.Range("G2").Value = "PACKTYPE";
            sht.Range("H2").Value = "DTSTAMP";
            sht.Range("I2").Value = "PONO";
            sht.Range("J2").Value = "Qty";
            sht.Range("K2").Value = "IIEM NAME";
            sht.Range("L2").Value = "QUALITY";
            sht.Range("M2").Value = "SHADECOLOR";
            sht.Range("N2").Value = "LOTNO";
            sht.Range("O2").Value = "TAGNO";
            sht.Range("P2").Value = "CONSUMP QTY";
            //sht.Range("K2").Value = "PD";

            //********
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "Date,ECISNO";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            //
            int row = 3;
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":P" + row).Style.Font.FontSize = 10;
                sht.Range("I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                using (var a = sht.Range("A" + row + ":P" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Date"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["BatchNo"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Ecisno"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Dest"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Articledesc"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Articleno"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["packingtype"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["dtstamp"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["PONo"]);
                sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);

                row = row + 1;

                //DataTable dtdistinct8 = ds.Tables[1].Select("SRNo='" + ds1.Tables[0].Rows[i]["SRNo"] + "' ");

                //DataView dv2 = ds.Tables[0].DefaultView;
                //dv.Sort = "Date,ECISNO";
                //DataSet ds2 = new DataSet();
                //ds1.Tables.Add(dv.ToTable());

                DataRow[] foundRows;
                foundRows = ds.Tables[1].Select("SRNo='" + ds1.Tables[0].Rows[i]["SRNo"] + "' ");
                if (foundRows.Length > 0)
                {
                    foreach (DataRow row4 in foundRows)
                    {
                        sht.Range("K" + row).SetValue(row4["ItemName"].ToString());
                        sht.Range("L" + row).SetValue(row4["QualityName"].ToString());
                        sht.Range("M" + row).SetValue(row4["ShadeColorName"].ToString());
                        sht.Range("N" + row).SetValue(row4["LotNo"].ToString());
                        sht.Range("O" + row).SetValue(row4["TagNo"].ToString());
                        sht.Range("P" + row).SetValue(row4["TotalPcsLagat"]);
                        row = row + 1;
                    }
                }

                row = row + 1;

            }
            ds.Dispose();
            ds1.Dispose();

            ////**************grand Total
            //var Total = sht.Evaluate("SUM(K3:K" + (row - 1) + ")");
            //sht.Range("K" + row).Value = Total;
            //sht.Range("K" + row).Style.Font.Bold = true;

            //var ToalPD = sht.Evaluate("SUM(K3:K" + (row - 1) + ")");
            //sht.Range("K" + row).Value = ToalPD;
            //sht.Range("K" + row).Style.Font.Bold = true;
            //************** Save
            String Path;
            sht.Columns(1, 20).AdjustToContents();

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("DISPATCHEDREPORTWITHFINISHINGRAWDETAIL_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "al2", "alert('No records found')", true);
        }
    }

    protected void TObeDispatchedKaysons()
    {
        string where = "";

        if (ddItemName.SelectedIndex > 0)
        {
            where = where + " and PLD.Itemid=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and PLD.qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and PLD.Designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and PLD.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            where = where + " and PLD.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and PLD.sizeid=" + DDSize.SelectedValue;
        }
        if (DDpacktype.SelectedIndex > 0)
        {
            where = where + " and PLD.Packtypeid=" + DDpacktype.SelectedValue;
        }
        if (DDarticleno.SelectedIndex > 0)
        {
            where = where + " and PLD.ArticleNo='" + DDarticleno.SelectedValue + "'";
        }


        DataSet ds = new DataSet();

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_GetTobedispatchReport_Kaysons", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@Where", where);
        cmd.Parameters.AddWithValue("@Fromdate", txtfromdate.Text);
        cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
        cmd.Parameters.AddWithValue("@BatchNo", txtbatchno.Text);
        cmd.Parameters.AddWithValue("@Tobedispatched", Rdtobedispatch.Checked == true ? 1 : 0);

        // DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();


        //SqlParameter[] param = new SqlParameter[5];
        //param[0] = new SqlParameter("@Where", where);
        //param[1] = new SqlParameter("@FromDate", txtfromdate.Text);
        //param[2] = new SqlParameter("@Todate", txttodate.Text);
        //param[3] = new SqlParameter("@Batchno", txtbatchno.Text);
        //param[4] = new SqlParameter("@Tobedispatched", Rdtobedispatch.Checked == true ? 1 : 0);
        ////*************
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetTobedispatchReport", param);
        ////Export to excel


        if (ds.Tables.Count > 0)
        {
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;

            GridView1.DataSource = ds;
            GridView1.DataBind();
            lblmsg.Text = "Wait....";
            Response.Clear();
            Response.Buffer = true;
            String Filename;
            if (Rdtobedispatch.Checked == true)
            {
                Filename = "TobeDispatched up to- " + txtfromdate.Text + " " + DateTime.Now + ".xls";
            }
            else
            {
                Filename = "PackedReport-" + DateTime.Now + ".xls";
            }
            Response.AddHeader("content-disposition",
             "attachment;filename=" + Filename);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                //Apply text style to each Row
                GridView1.Rows[i].Attributes.Add("class", "textmode");
            }
            GridView1.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            lblmsg.Text = "Done.....";
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt4", "alert('No Record found')", true);
        }
    }
}