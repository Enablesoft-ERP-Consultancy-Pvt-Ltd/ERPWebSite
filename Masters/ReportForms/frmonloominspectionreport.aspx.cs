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

public partial class Masters_ReportForms_frmonloominspectionreport : System.Web.UI.Page
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
                           select Val,Type from Sizetype";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 3, false, "--Plz Select--");

            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
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
        UtilityModule.ConditionalComboFill(ref DDLoomNo, @"select  PM.UID,PM.loomno+'/'+IM.ITEM_NAME as LoomNo from ProductionLoomMaster PM 
                                            inner join ITEM_MASTER IM on PM.Itemid=IM.ITEM_ID                                            
                                            Where  PM.CompanyId=" + DDcompany.SelectedValue + " and PM.UnitId=" + DDProdunit.SelectedValue + " order by case when ISNUMERIC(PM.loomno)=1 Then CONVERT(int,PM.loomno) Else 9999999 End,PM.loomno", true, "--Plz Select--");
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string Where = "";
        if (DDProdunit.SelectedIndex > 0)
        {
            Where = Where + " and PIM.units=" + DDProdunit.SelectedValue;
        }
        if (DDLoomNo.SelectedIndex > 0)
        {
            Where = Where + " and PIM.Loomid=" + DDLoomNo.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            Where = Where + " and vf.item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            Where = Where + " and vf.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            Where = Where + " and vf.DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            Where = Where + " and vf.Colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            Where = Where + " and vf.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            Where = Where + " and vf.Sizeid=" + DDSize.SelectedValue;
        }
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
        param[1] = new SqlParameter("@Where", Where);
        param[2] = new SqlParameter("@fromdate", txtfromdate.Text);
        param[3] = new SqlParameter("@Todate", txttodate.Text);
        //****
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETONLOOMINSPPECTIONREPORT", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/TempExcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/TempExcel/"));
            }
            //*************
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("ON LOOM INSPECTION");

            sht.Range("A1").Value = "User Name";
            sht.Range("B1").Value = "Stock No";
            sht.Range("C1").Value = "Scan Date";
            sht.Range("D1").Value = "Production Order Date";
            sht.Range("E1").Value = "Unit Name";
            sht.Range("F1").Value = "Loom No";
            sht.Range("G1").Value = "Folio No";
            sht.Range("H1").Value = "Quality";
            sht.Range("I1").Value = "Design";
            sht.Range("J1").Value = "Color";
            sht.Range("K1").Value = "Size";
            sht.Range("L1").Value = "Qty";
            sht.Range("M1").Value = "TArea";
            sht.Range("N1").Value = "EMP Name";


            //********Default Variables
            int Defectcnt = Convert.ToInt16(ds.Tables[0].Compute("Max(Defectcnt)", ""));
            Defectcnt = Defectcnt == 0 ? 1 : Defectcnt;
            int cell = 14;
            int FirstDefectcell = (cell + 1);
            int Remarkcell = 0;
            int Progresscell = 0;
            int Completedcell = 0;
            int Rownum = 2;
            //****************
            for (int i = 0; i < Defectcnt; i++)
            {
                cell = cell + 1;
                sht.Cell(1, cell).Value = "Defect";
            }
            cell += 1;
            Remarkcell = cell;
            sht.Cell(1, cell).Value = "Remark";
            cell += 1;
            Progresscell = cell;
            sht.Cell(1, cell).Value = "Progress";
            cell += 1;
            Completedcell = cell;
            sht.Cell(1, cell).Value = "Completed";

            sht.Range(sht.Cell(1, 1), sht.Cell(1, Completedcell)).Style.Font.FontSize = 10;
            sht.Range(sht.Cell(1, 1), sht.Cell(1, Completedcell)).Style.Font.Bold = true;

            //***********End of Headers
            string Defectsname = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + Rownum).SetValue(ds.Tables[0].Rows[i]["Username"]);
                sht.Range("B" + Rownum).SetValue(ds.Tables[0].Rows[i]["TstockNo"]);
                sht.Range("C" + Rownum).SetValue(ds.Tables[0].Rows[i]["scandate"]);  // = "Scan Date";
                sht.Range("D" + Rownum).SetValue(ds.Tables[0].Rows[i]["assigndate"]); // = "Production Order Date";
                sht.Range("E" + Rownum).SetValue(ds.Tables[0].Rows[i]["unitname"]); // = "Unit Name";
                sht.Range("F" + Rownum).SetValue(ds.Tables[0].Rows[i]["LoomNo"]); //= "Loom No";
                sht.Range("G" + Rownum).SetValue(ds.Tables[0].Rows[i]["issueorderid"]); //= "Folio No";
                sht.Range("H" + Rownum).SetValue(ds.Tables[0].Rows[i]["Qualityname"]); // = "Quality";
                sht.Range("I" + Rownum).SetValue(ds.Tables[0].Rows[i]["Designname"]); // = "Design";
                sht.Range("J" + Rownum).SetValue(ds.Tables[0].Rows[i]["colorname"]); // = "Color";
                sht.Range("K" + Rownum).SetValue(ds.Tables[0].Rows[i]["Size"]); // = "Size";
                sht.Range("L" + Rownum).SetValue(ds.Tables[0].Rows[i]["Qty"]); // = "Qty";
                sht.Range("M" + Rownum).SetValue(ds.Tables[0].Rows[i]["Area"]);// = "TArea";
                sht.Range("N" + Rownum).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]); // = "EMP Name";
                //**************Defects
                Defectsname = ds.Tables[0].Rows[i]["defects"].ToString().TrimStart(';');
                Defectsname = Defectsname.TrimEnd(';');
                var defects = Defectsname.Split(';');
                int Defectcellstart = FirstDefectcell;
                foreach (var item in defects)
                {
                    sht.Cell(Rownum, Defectcellstart).Value = item;
                    Defectcellstart += 1;
                }
                //****************
                sht.Cell(Rownum, Remarkcell).SetValue(ds.Tables[0].Rows[i]["Remark"]);
                sht.Cell(Rownum, Progresscell).SetValue(ds.Tables[0].Rows[i]["Progress"]);
                sht.Cell(Rownum, Completedcell).SetValue(ds.Tables[0].Rows[i]["Completed"]);

                Rownum = Rownum + 1;
            }
            //***********Borders
            using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(Rownum, Completedcell)))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*******************************************************************
            sht.Columns(1, 36).AdjustToContents();
            sht.Columns("C").Width = 9.44;

            string Path;
            string Fileextension = "xlsx";
            string filename = "OnLoomInspection-" + UtilityModule.validateFilename(System.DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension + "");
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "altrep", "alert('No records Found for this combination.');", true);
        }

    }
}