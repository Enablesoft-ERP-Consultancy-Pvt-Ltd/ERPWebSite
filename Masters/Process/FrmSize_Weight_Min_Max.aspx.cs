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

public partial class Masters_Process_FrmSize_Weight_Min_Max : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varcompanyNo"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = null;
            DataSet ds = null;
            str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                    From Companyinfo CI(nolock)
                    JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @"  
                    Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By CompanyName 

                    select  UnitsId,UnitName from Units with(nolock) Where Mastercompanyid = " + Session["varcompanyid"] + @" 
                    select ITEM_ID,ITEM_NAME from ITEM_MASTER IM with(nolock) Inner Join CategorySeparate CS with(nolock) on 
                    cs.Categoryid=IM.CATEGORY_ID and Cs.id=0 And IM.Mastercompanyid = " + Session["varcompanyid"] + @" 
                    select  ShapeId,ShapeName from Shape with(nolock) 
                    select PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER With(nolock) Where MasterCompanyid = " + Session["varcompanyid"] + @" 
                    select ICm.CATEGORY_ID,ICM.CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=Cs.Categoryid and cs.id=0 order by CATEGORY_NAME 
                    select val,Type From Sizetype";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDUnitName, ds, 1, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDArticleName, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDShape, ds, 3, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref ddJob, ds, 4, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 5, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizeType, ds, 6, false, "--Plz Select--");

            if (DDsizeType.Items.FindByValue(variable.VarDefaultSizeId) != null)
            {
                DDsizeType.SelectedValue = variable.VarDefaultSizeId;
            }
            if (DDShape.Items.Count > 0)
            {
                DDShape.SelectedIndex = 0;
            }
            ds.Dispose();
            switch (Session["varcompanyId"].ToString())
            {
                case "8":
                    //Divuniname.Visible = true;
                    break;
                default:
                    divQuality.Visible = true;
                    divDesign.Visible = true;
                    Divuniname.Visible = true;
                    divCategory.Visible = true;
                    if (DDCategory.Items.Count > 0)
                    {
                        DDCategory.SelectedIndex = 1;
                        DDCategory_SelectedIndexChanged(DDCategory, new EventArgs());
                    }
                    break;
            }
            
            
        }
    }
   
    protected void DDArticleName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (divQuality.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddquality, @"select Distinct vf.Qualityid,vf.Qualityname from V_FinishedItemDetail vf with(nolock)
                                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + " And QualityId<>0 order by vf.Qualityname", true, "--Plz Select--");
        }
        UtilityModule.ConditionalComboFill(ref DDColor, @"select Distinct vf.ColorId,vf.ColorName from V_FinishedItemDetail vf with(nolock)
                                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + " And ColorId<>0 order by ColorName", true, "--Plz Select--");
        fillGrid();
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        string size = "SizeMtr";
        switch (DDsizeType.SelectedValue)
        {
            case "0":
                size = "sizeft";
                break;
            case "1":
                size = "SizeMtr";
                break;
            case "2":
                size = "Sizeinch";
                break;
            default:
                break;
        }
        string str = @"select Distinct SizeId," + size + @" as size from V_FinishedItemDetail vf with(nolock)
                       inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + @"
                       And ShapeId= " + DDShape.SelectedValue + " and Sizeid<>0";
        if (ddquality.SelectedIndex > 0)
        {
            str = str + " and  vf.QualityId=" + ddquality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and  vf.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and  vf.colorid=" + DDColor.SelectedValue;
        }
        str = str + " order by size";
        UtilityModule.ConditionalComboFill(ref ddSize, str, true, "--Plz Select--");
    }
    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDShape_SelectedIndexChanged(sender, e);
        fillGrid();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int finishedid = 0;
            finishedid = UtilityModule.getItemFinishedId(DDArticleName, ddquality, DDDesign, DDColor, DDShape, ddSize, TxtProductCode, Tran, ddlshade, "", Convert.ToInt32(Session["varCompanyId"]));
            SqlParameter[] param = new SqlParameter[13];
            param[0] = new SqlParameter("@CompanyId", DDCompanyName.SelectedValue);
            param[1] = new SqlParameter("@UnitsId", Divuniname.Visible == true ? DDUnitName.SelectedValue : "0");
            param[2] = new SqlParameter("@ProcessId", ddJob.SelectedValue);
            param[3] = new SqlParameter("@Finishedid", finishedid);
            param[4] = new SqlParameter("@WidthMin", txtWidthMin.Text);
            param[5] = new SqlParameter("@WidthMax", txtWidthMax.Text);
            param[6] = new SqlParameter("@LengthMin", txtLengthMin.Text);
            param[7] = new SqlParameter("@LengthMax", txtLengthMax.Text);
            param[8] = new SqlParameter("@WeightMin", txtWeightMin.Text);
            param[9] = new SqlParameter("@WeightMax", txtWeightMax.Text);
            param[10] = new SqlParameter("@SizeUnitId", DDsizeType.SelectedValue);
            param[11] = new SqlParameter("@UserId", Session["varuserid"]);
            param[12] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);  
            //Save data
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEWidhtLengthWeightMinMax", param);
            //

            Tran.Commit();
            lblMessage.Text = "Size and Weight Added Successfully....";
            txtWidthMin.Text = "";
            txtWidthMax.Text = "";
            txtLengthMin.Text = "";
            txtLengthMax.Text = "";
            txtWeightMin.Text = "";
            txtWeightMax.Text = "";
            fillGrid();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }

    }
    protected void fillGrid()
    {
        DataSet ds = null;
        string str = @"Select VF.item_id,VF.ITEM_NAME,VF.QualityName,VF.designName,PNM.Process_name as ProcessName,vf.ColorName as Colour,Vf.shapeName,
                        case when WLM.SizeUnitId=1 then vf.SizeMtr else vf.sizeft end as Size,WLM.AddDate,WLM.WidthMin,WLM.WidthMax,WLM.LengthMin,WLM.LengthMax,WLM.WeightMin,WLM.WeightMax
                        from WidthLengthWeightMinMaxMaster WLM JOIN V_FinishedItemDetail VF ON WLM.FinishedId=VF.Item_Finished_id
                        inner join PROCESS_NAME_MASTER PNM on PNM.PROCESS_NAME_ID=WLM.ProcessId 
                        Where WLM.MasterCompanyId=" + Session["varcompanyId"] + " And WLM.companyId=" + DDCompanyName.SelectedValue;
        if (Divuniname.Visible == true)
        {
            if (DDUnitName.SelectedIndex >= 0)
            {
                str = str + "  And WLM.UnitsId= " + DDUnitName.SelectedValue;
            }
        }
        if (ddJob.SelectedIndex > 0)
        {
            str = str + "  And WLM.ProcessId= " + ddJob.SelectedValue;
        }
        if (DDArticleName.SelectedIndex > 0)
        {
            str = str + "  And vf.Item_Id= " + DDArticleName.SelectedValue;
        }
        if (ddquality.SelectedIndex > 0)
        {
            str = str + "  And vf.QualityId= " + ddquality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + "  And vf.DesignId= " + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + "  And vf.ColorId= " + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            str = str + "  And vf.ShapeId= " + DDShape.SelectedValue;
        }
        if (ddSize.SelectedIndex > 0)
        {
            str = str + "  And vf.SizeId= " + ddSize.SelectedValue;
        }

        str = str + " order by WLM.id desc";
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //var strSort = "Articles asc";
        //var dv = ds.Tables[0].DefaultView;
        //dv.Sort = strSort;
        //var newDS = new DataSet();
        //var newDT = dv.ToTable();
        ////newDS.Tables.Add(newDT);        
        //ds.Tables[0].DefaultView.Sort = "Articles asc";
        //ds.Tables[0].DefaultView.Sort = "Date desc";
        DGWidthLengthWeight.DataSource = ds;
        DGWidthLengthWeight.DataBind();
    }
    protected void ddJob_SelectedIndexChanged(object sender, EventArgs e)
    {
        //switch (ddJob.SelectedItem.Text.ToUpper())
        //{
        //    case "WEAVING":
        //        divratetype.Visible = true;
        //        break;
        //    case "PANEL MAKING":
        //        divratetype.Visible = true;
        //        break;
        //    case "FILLER MAKING":
        //        divratetype.Visible = true;
        //        break;
        //    default:
        //        divratetype.Visible = false;
        //        break;
        //}
        fillGrid();
    }
    protected void DDUnitName_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillGrid();
    }

    protected void ddquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDDesign, @"select Distinct designId,designName from V_FinishedItemDetail vf with(nolock)
                                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + " and QualityId=" + ddquality.SelectedValue + " And Designid<>0 order by designName", true, "--Plz Select--");
        fillGrid();
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDColor, @"select Distinct Colorid,ColorName from V_FinishedItemDetail vf with(nolock)
                                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + " and QualityId=" + ddquality.SelectedValue + " and DesignId=" + DDDesign.SelectedValue + " And Colorid<>0 order by ColorName", true, "--Plz Select--");
        if (DDColor.Items.Count > 0)
        {
            DDColor_SelectedIndexChanged(sender, new EventArgs());
        }
        fillGrid();
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
    }

    private void ddlcategorycange()
    {
        divQuality.Visible = false;
        divDesign.Visible = false;
        divColor.Visible = false;
        divShape.Visible = false;
        divSize.Visible = false;
        divshade.Visible = false;

        UtilityModule.ConditionalComboFill(ref DDArticleName, @"select IM.ITEM_ID,IM.ITEM_NAME From Item_Master IM inner join CategorySeparate CS on IM.CATEGORY_ID=CS.Categoryid
                                                            and CS.id=0 and cs.categoryid=" + DDCategory.SelectedValue + " and IM.MasterCompanyid=" + Session["varcompanyId"] + " order by IM.ITEM_NAME", true, "--Plz Select--");

        string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                      " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                      " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        divQuality.Visible = true;
                        break;
                    case "2":
                        divDesign.Visible = true;
                        break;
                    case "3":
                        divColor.Visible = true;
                        break;
                    case "4":
                        divShape.Visible = true;
                        break;
                    case "5":
                        divSize.Visible = true;
                        break;
                    case "6":
                        divshade.Visible = false;
                        UtilityModule.ConditionalComboFill(ref ddlshade, @"Select Distinct SC.ShadeColorID, SC.ShadeColorName 
                                From ShadeColor SC(nolock)
                                Order By ShadeColorName", true, "--Plz Select--");
                        break;
                }
            }
        }
    }
    protected void DDsizeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDShape_SelectedIndexChanged(sender, new EventArgs());
        fillGrid();
    }  
   
    
    protected void DGWidthLengthWeight_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVFinisherJobRate, "select$" + e.Row.RowIndex);

            //for (int i = 0; i < DGRateDetail.Columns.Count; i++)
            //{
            //    if (DGRateDetail.Columns[i].HeaderText == "Rate Location")
            //    {
            //        if (variable.VarDEFINEPROCESSRATE_LOCATIONWISE == "1")
            //        {
            //            DGRateDetail.Columns[i].Visible = true;
            //        }
            //        else
            //        {
            //            DGRateDetail.Columns[i].Visible = false;
            //        }
            //    }

            //    if (DGRateDetail.Columns[i].HeaderText == "Emp Name")
            //    {
            //        if (Session["varCompanyId"].ToString() == "27")
            //        {
            //            DGRateDetail.Columns[i].Visible = true;
            //        }
            //        else
            //        {
            //            DGRateDetail.Columns[i].Visible = false;
            //        }
            //    }
            //}
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        try
        {
            string str = "";
            if (Divuniname.Visible == true)
            {
                if (DDUnitName.SelectedIndex >= 0)
                {
                    str = str + "  And WLM.Unitsid= " + DDUnitName.SelectedValue;
                }
            }
            if (ddJob.SelectedIndex > 0)
            {
                str = str + "  And WLM.ProcessId= " + ddJob.SelectedValue;
            }
            if (DDArticleName.SelectedIndex > 0)
            {
                str = str + "  And vf.Item_Id= " + DDArticleName.SelectedValue;
            }
            if (ddquality.SelectedIndex > 0)
            {
                str = str + "  And vf.QualityId= " + ddquality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + "  And vf.DesignId= " + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + "  And vf.ColorId= " + DDColor.SelectedValue;
            }
            if (DDShape.SelectedIndex > 0)
            {
                str = str + "  And vf.ShapeId= " + DDShape.SelectedValue;
            }
            if (ddSize.SelectedIndex > 0)
            {
                str = str + "  And vf.SizeId= " + ddSize.SelectedValue;
            }
           
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@companyid", DDCompanyName.SelectedValue);
            param[1] = new SqlParameter("@where", str);
            param[2] = new SqlParameter("@CurrentWidthLengthWeight", chkcurrentwidthlengthweight.Checked == true ? "1" : "0");

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_PRINTWidthLengthWeightMinMaxReport", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (chkcurrentwidthlengthweight.Checked == true)
                {
                    ProcessJobRateInExcel(ds);
                }


                //if (Session["VarCompanyNo"].ToString() == "16" || Session["VarCompanyNo"].ToString() == "28")
                //{
                //    if (chkcurrentwidthlengthweight.Checked == true)
                //    {
                //        ProcessJobRateInExcel(ds);
                //    }
                //    else
                //    {
                //        Session["rptFileName"] = "~\\Reports\\rptjobrate.rpt";
                //        Session["GetDataset"] = ds;
                //        Session["dsFileName"] = "~\\ReportSchema\\rptjobrate.xsd";
                //    }

                //}
                //else
                //{
                //    Session["rptFileName"] = "~\\Reports\\rptjobrate.rpt";
                //    Session["GetDataset"] = ds;
                //    Session["dsFileName"] = "~\\ReportSchema\\rptjobrate.xsd";
                //}

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
    private void ProcessJobRateInExcel(DataSet ds)
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

            sht.Range("A1:N1").Merge();
            sht.Range("A1").Value = ds.Tables[0].Rows[0]["ProcessName"] + " " + "MIN MAX WIDTH LENGTH WEIGHT LIST FORMAT";
            //sht.Range("A2:X2").Merge();
            //sht.Range("A2").Value = "Filter By :  " + FilterBy;
            //sht.Row(2).Height = 30;
            sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:N2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A2:N2").Style.Alignment.SetWrapText();
            sht.Range("A1:N2").Style.Font.FontName = "Arial";
            sht.Range("A1:N2").Style.Font.FontSize = 10;
            sht.Range("A1:N2").Style.Font.Bold = true;

            //*******Header
            sht.Range("A3").Value = "SR NO.";
            sht.Range("B3").Value = "ITEM";
            sht.Range("C3").Value = "QUALITY";
            sht.Range("D3").Value = "DESIGN";
            sht.Range("E3").Value = "COLOR";
            sht.Range("F3").Value = "SHAPE";
            sht.Range("G3").Value = "SIZE";
            sht.Range("H3").Value = "MIN WIDTH";
            sht.Range("I3").Value = "MAX WIDTH";
            sht.Range("J3").Value = "MIN LENGTH";
            sht.Range("K3").Value = "MAX LENGTH";
            sht.Range("L3").Value = "MIN WEIGHT";
            sht.Range("M3").Value = "MAX WEIGHT";
            sht.Range("N3").Value = "DATE";


            sht.Range("A3:N3").Style.Font.FontName = "Arial";
            sht.Range("A3:N3").Style.Font.FontSize = 9;
            sht.Range("A3:N3").Style.Font.Bold = true;
            sht.Range("S3:N3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3:N3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A3:N3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A3:N3").Style.Alignment.SetWrapText();


            //DataView dv = new DataView(ds.Tables[0]);
            //dv.Sort = "FOLIONO";
            //DataSet ds1 = new DataSet();
            //ds1.Tables.Add(dv.ToTable());

            int srno = 0;
            row = 4;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":N" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":N" + row).Style.Font.FontSize = 8;

                srno = srno + 1;

                sht.Range("A" + row).SetValue(srno);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Colour"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ShapeName"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Size"]);


                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["WidthMin"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["WidthMax"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["LengthMin"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["LengthMax"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["WeightMin"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["WeightMax"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["Date"]);


                row = row + 1;

            }
            //*************
            sht.Columns(1, 26).AdjustToContents();

            //sht.Columns("K").Width = 13.43;

            using (var a = sht.Range("A1" + ":N" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("ProcessWiseMinMaxWidthLengthWeigh_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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