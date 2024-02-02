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
public partial class Masters_ReportForms_frmwastagereport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName                                                      
                           select Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=cs.Categoryid and cs.id=0 and ICM.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @"
                           select Val,Type from Sizetype
                           select Distinct U.UnitsId,U.UnitName From Units U inner join PROCESS_ISSUE_MASTER_1 PIM on U.UnitsId=PIM.Units Where U.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by U.UnitName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 2, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDunitName, ds, 3, true, "--Plz Select--");
            txtFromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
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
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(DDQuality, DDDesign, DDColor, DDShape, Convert.ToInt16(ddItemName.SelectedValue));
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
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void DDunitName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDloomno, "select Distinct PLM.UID,PLM.LoomNo,case when ISNUMERIC(PLM.loomno)=1 Then CONVERT(int,PLM.loomno) Else 9999999 End as Loomno1 From ProductionLoommaster PLM inner join PROCESS_ISSUE_MASTER_1 PIM on PLM.uid=PIM.LoomId Where PLM.UnitId=" + DDunitName.SelectedValue + " order by PLM.LoomNo,Loomno1", true, "--Plz Select--");
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string where = "", rpttilte = "Filter By ";
        lblmsg.Text = "";
        where = where + " and PIM.CompanyId=" + DDcompany.SelectedValue;
        rpttilte = rpttilte + " Company-" + DDcompany.SelectedItem.Text;
        if (DDunitName.SelectedIndex > 0)
        {
            where = where + " and PIM.Units=" + DDunitName.SelectedValue;
            rpttilte = rpttilte + " ,UNIT-" + DDunitName.SelectedItem.Text;
        }
        if (DDloomno.SelectedIndex > 0)
        {
            where = where + " and PIM.Loomid=" + DDloomno.SelectedValue;
            rpttilte = rpttilte + " " + ",LoomNo.-" + DDloomno.SelectedItem.Text;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            where = where + " and vf.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            where = where + " and vf.Item_id=" + ddItemName.SelectedValue;
            rpttilte = rpttilte + " ," + ddItemName.SelectedItem.Text;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and vf.qualityid=" + DDQuality.SelectedValue;
            rpttilte = rpttilte + " ," + DDQuality.SelectedItem.Text;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and vf.Designid=" + DDDesign.SelectedValue;
            rpttilte = rpttilte + " ," + DDDesign.SelectedItem.Text;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and vf.colorid=" + DDColor.SelectedValue;
            rpttilte = rpttilte + " ," + DDColor.SelectedItem.Text;
        }
        if (DDShape.SelectedIndex > 0)
        {
            where = where + " and vf.shapeid=" + DDShape.SelectedValue;
            rpttilte = rpttilte + " ," + DDShape.SelectedItem.Text;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and vf.sizeid=" + DDSize.SelectedValue;
            rpttilte = rpttilte + " ," + DDSize.SelectedItem.Text;
        }
        //********Proc
        SqlParameter[] param = new SqlParameter[6];
        param[0] = new SqlParameter("@fromdate", txtFromdate.Text);
        param[1] = new SqlParameter("@Todate", txttodate.Text);
        param[2] = new SqlParameter("@where", where);
        //******
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Wastagereport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Wastage Report");
            //***************************
            sht.Range("A1:M1").Merge();
            sht.Range("A1:M1").Style.Font.FontName = "Tahoma";
            sht.Range("A1:M1").Style.Font.FontSize = 10;
            sht.Range("A1:M1").Style.Font.Bold = true;
            sht.Range("A1:M1").Style.NumberFormat.Format = "@";
            sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:M1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1").Value = ds.Tables[0].Rows[0]["CompanyName"] + " (Wastage Report)  From " + txtFromdate.Text + " To " + txttodate.Text;
            sht.Row(1).Height = 24.00;
            //Title
            sht.Range("A2:M2").Merge();
            sht.Range("A2:M2").Style.Font.FontName = "Tahoma";
            sht.Range("A2:M2").Style.Font.FontSize = 10;
            sht.Range("A2:M2").Style.Font.Bold = true;
            sht.Range("A2:M2").Style.NumberFormat.Format = "@";
            sht.Range("A2:M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:M2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:M2").Style.Alignment.WrapText = true;
            sht.Range("A2").Value = rpttilte;
            sht.Row(1).Height = 20.00;
            //Headers        
            sht.Range("A3:M3").Style.Font.FontName = "Tahoma";
            sht.Range("A3:M3").Style.Font.FontSize = 10;
            sht.Range("A3:M3").Style.Font.Bold = true;
            sht.Range("A3:M3").Style.NumberFormat.Format = "@";
            sht.Range("G3:M3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3").Value = "UNIT NAME";
            sht.Range("B3").Value = "LOOM NO";
            sht.Range("C3").Value = "QUALITY";
            sht.Range("D3").Value = "DESIGN";
            sht.Range("E3").Value = "COLOR";
            sht.Range("F3").Value = "SIZE";
            sht.Range("G3").Value = "ORDER QTY";
            sht.Range("H3").Value = "REC. QTY";
            sht.Range("I3").Value = "REC. WEIGHT(Kgs)";
            sht.Range("J3").Value = "TOTAL MATERIAL WT(Kgs)";
            sht.Range("K3").Value = "WASTAGE(Kgs)";
            sht.Range("L3").Value = "FOLIO NO";
            sht.Range("M3").Value = "FOLIO STATUS";
            int i;
            decimal wastage = 0, Totalwastage = 0;

            //Excel row
            i = 4;
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                sht.Range("A" + i + ":M" + i).Style.Font.FontName = "Tahoma";
                sht.Range("A" + i + ":M" + i).Style.Font.FontSize = 10;
                sht.Range("A" + i + ":M" + i).Style.NumberFormat.Format = "@";

                sht.Range("A" + i).SetValue(ds.Tables[0].Rows[j]["Unitname"]);
                sht.Range("B" + i).SetValue(ds.Tables[0].Rows[j]["LoomNo"]);
                sht.Range("C" + i).SetValue(ds.Tables[0].Rows[j]["QualityName"]);
                sht.Range("D" + i).SetValue(ds.Tables[0].Rows[j]["DesignName"]);
                sht.Range("E" + i).SetValue(ds.Tables[0].Rows[j]["colorname"]);
                sht.Range("F" + i).SetValue(ds.Tables[0].Rows[j]["Size"]);
                sht.Range("G" + i).SetValue(Convert.ToInt32(ds.Tables[0].Rows[j]["orderqty"]));
                sht.Range("H" + i).SetValue(Convert.ToInt32(ds.Tables[0].Rows[j]["Recqty"]));
                sht.Range("I" + i).SetValue(Convert.ToDecimal(String.Format("{0:#,0.000}", ds.Tables[0].Rows[j]["recweight"])));
                sht.Range("J" + i).SetValue(Convert.ToDecimal(String.Format("{0:#,0.000}", ds.Tables[0].Rows[j]["materialwt"])));
                //*******wastage
                wastage = Convert.ToDecimal(ds.Tables[0].Rows[j]["materialwt"]) - Convert.ToDecimal(ds.Tables[0].Rows[j]["recweight"]);
                Totalwastage = Totalwastage + wastage;
                //*******
                sht.Range("K" + i).SetValue(Convert.ToDecimal(String.Format("{0:#,0.000}", wastage)));
                sht.Range("L" + i).SetValue(ds.Tables[0].Rows[j]["IssueOrderID"]);
                sht.Range("M" + i).SetValue(ds.Tables[0].Rows[j]["FolioStatus"]);
                sht.Range("G" + i + ":M" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                i = i + 1;

            }
            //Grand Total
            sht.Range("G" + i + ":L" + i).Style.Font.FontName = "Tahoma";
            sht.Range("G" + i + ":L" + i).Style.Font.FontSize = 10;
            sht.Range("G" + i + ":L" + i).Style.NumberFormat.Format = "@";
            sht.Range("G" + i + ":L" + i).Style.Font.Bold = true;
            sht.Range("G" + i + ":L" + i).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("G" + i).SetValue(ds.Tables[0].Compute("sum(orderqty)", ""));
            sht.Range("H" + i).SetValue(ds.Tables[0].Compute("sum(Recqty)", ""));
            sht.Range("I" + i).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Compute("sum(recweight)", "")));
            sht.Range("J" + i).SetValue(String.Format("{0:#,0.000}", ds.Tables[0].Compute("sum(materialwt)", "")));
            sht.Range("K" + i).SetValue(String.Format("{0:#,0.000}", Totalwastage));
            //
            sht.Columns(1, 30).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("WastageReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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

        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }

    }
}