using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using ClosedXML.Excel;

public partial class Masters_ReportForms_FrmBomAndConsumptionDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select DISTINCT Category_Id,Category_Name from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And Id=0 And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by Category_Name
            Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By PROCESS_NAME";
            DataSet ds = SqlHelper.ExecuteDataset(str);
            CommanFunction.FillComboWithDS(ddCategoryName, ds, 0);
            UtilityModule.ConditionalComboFillWithDS(ref ddProcessName, ds, 1, true, "--Select--");
            if (ddCategoryName.Items.Count > 0)
            {
                CATEGORY_DEPENDS_CONTROLS();
            }
            imgLogo.ImageUrl.DefaultIfEmpty();
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varMasterCompanyIDForERP"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
            LblCompanyName.Text = Session["varCompanyName"].ToString();
            LblUserName.Text = Session["varusername"].ToString();
        }
    }

    protected void ddcategoryname_SelectedIndexChanged(object sender, EventArgs e)
    {
        CATEGORY_DEPENDS_CONTROLS();
    }
    private void CATEGORY_DEPENDS_CONTROLS()
    {
        Quality.Visible = false;
        Design.Visible = false;
        Color.Visible = false;
        Shape.Visible = false;
        Size.Visible = false;
        Shade.Visible = false;
        UtilityModule.ConditionalComboFill(ref ddItemName, "Select ITEM_ID,ITEM_NAME from ITEM_MASTER Where CATEGORY_ID=" + ddCategoryName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By ITEM_NAME", true, "SELECT--");
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from ITEM_CATEGORY_PARAMETERS Where CATEGORY_ID=" + ddCategoryName.SelectedValue + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in Ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        Quality.Visible = true;
                        break;
                    case "2":
                        Design.Visible = true;
                        break;
                    case "3":
                        Color.Visible = true;
                        break;
                    case "4":
                        Shape.Visible = true;
                        break;
                    case "5":
                        Size.Visible = true;
                        break;
                    case "6":
                        Shade.Visible = true;
                        break;
                }
            }
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(ddQuality, ddDesign, ddColor, ddShape, ddShade, Convert.ToInt32(ddItemName.SelectedValue), 0);
    }
    private void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Shade, int Itemid, int Type_Flag, System.Web.UI.HtmlControls.HtmlTableCell tdItem = null, System.Web.UI.HtmlControls.HtmlTableCell tdQuality = null, System.Web.UI.HtmlControls.HtmlTableCell tdDesign = null)
    {
        string Str = @" SELECT Distinct VF.QUALITYID, VF.QUALITYNAME 
            FROM V_FinishedItemDetail VF(Nolock) 
            JOIN PROCESSCONSUMPTIONMASTER PCM(Nolock) ON PCM.FINISHEDID = VF.ITEM_FINISHED_ID 
            WHERE VF.ITEM_ID = " + Itemid + " And VF.MasterCompanyId = " + Session["varMasterCompanyIDForERP"];
        if (ddProcessName.SelectedIndex > 0)
        {
            Str = Str + " And PCM.ProcessID = " + ddProcessName.SelectedValue;
        }
        Str = Str + " Order By VF.QUALITYNAME";

        Str = Str + @" SELECT Distinct VF.DesignID, VF.DESIGNNAME
            FROM V_FinishedItemDetail VF(Nolock) 
            JOIN PROCESSCONSUMPTIONMASTER PCM(Nolock) ON PCM.FINISHEDID = VF.ITEM_FINISHED_ID 
            WHERE VF.ITEM_ID = " + Itemid + " And VF.MasterCompanyId = " + Session["varMasterCompanyIDForERP"];
        if (ddProcessName.SelectedIndex > 0)
        {
            Str = Str + " And PCM.ProcessID = " + ddProcessName.SelectedValue;
        }
        Str = Str + " Order By VF.DESIGNNAME";

        Str = Str + @" SELECT Distinct VF.ColorID, VF.COLORNAME 
            FROM V_FinishedItemDetail VF(Nolock) 
            JOIN PROCESSCONSUMPTIONMASTER PCM(Nolock) ON PCM.FINISHEDID = VF.ITEM_FINISHED_ID 
            WHERE VF.ITEM_ID = " + Itemid + " And VF.MasterCompanyId = " + Session["varMasterCompanyIDForERP"];
        if (ddProcessName.SelectedIndex > 0)
        {
            Str = Str + " And PCM.ProcessID = " + ddProcessName.SelectedValue;
        }
        Str = Str + " Order By VF.COLORNAME";

        Str = Str + @" SELECT Distinct VF.SHAPEID, VF.SHAPENAME 
            FROM V_FinishedItemDetail VF(Nolock) 
            JOIN PROCESSCONSUMPTIONMASTER PCM(Nolock) ON PCM.FINISHEDID = VF.ITEM_FINISHED_ID 
            WHERE VF.ITEM_ID = " + Itemid + " And VF.MasterCompanyId = " + Session["varMasterCompanyIDForERP"];
        if (ddProcessName.SelectedIndex > 0)
        {
            Str = Str + " And PCM.ProcessID = " + ddProcessName.SelectedValue;
        }
        Str = Str + " Order By VF.SHAPENAME";

        Str = Str + @" SELECT Distinct VF.SHADECOLORID, VF.SHADECOLORNAME 
            FROM V_FinishedItemDetail VF(Nolock) 
            JOIN PROCESSCONSUMPTIONMASTER PCM(Nolock) ON PCM.FINISHEDID = VF.ITEM_FINISHED_ID 
            WHERE VF.ITEM_ID = " + Itemid + " And VF.MasterCompanyId = " + Session["varMasterCompanyIDForERP"];
        if (ddProcessName.SelectedIndex > 0)
        {
            Str = Str + " And PCM.ProcessID = " + ddProcessName.SelectedValue;
        }
        Str = Str + " Order By VF.SHADECOLORNAME";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref Quality, ds, 0, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Design, ds, 1, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Color, ds, 2, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Shape, ds, 3, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Shade, ds, 4, true, "--SELECT--");
    }
    protected void ddQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = @" SELECT Distinct VF.DesignID, VF.DESIGNNAME
            FROM V_FinishedItemDetail VF(Nolock) 
            JOIN PROCESSCONSUMPTIONMASTER PCM(Nolock) ON PCM.FINISHEDID = VF.ITEM_FINISHED_ID 
            WHERE VF.ITEM_ID = " + ddItemName.SelectedValue + " And VF.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + " And VF.QualityID = " + ddQuality.SelectedValue;
        if (ddProcessName.SelectedIndex > 0)
        {
            Str = Str + " And PCM.ProcessID = " + ddProcessName.SelectedValue;
        }
        Str = Str + " Order By VF.DESIGNNAME";

        Str = Str + @" SELECT Distinct VF.ColorID, VF.COLORNAME 
            FROM V_FinishedItemDetail VF(Nolock) 
            JOIN PROCESSCONSUMPTIONMASTER PCM(Nolock) ON PCM.FINISHEDID = VF.ITEM_FINISHED_ID 
            WHERE VF.ITEM_ID = " + ddItemName.SelectedValue + " And VF.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + " And VF.QualityID = " + ddQuality.SelectedValue;
        if (ddProcessName.SelectedIndex > 0)
        {
            Str = Str + " And PCM.ProcessID = " + ddProcessName.SelectedValue;
        }
        Str = Str + " Order By VF.COLORNAME";

        Str = Str + @" SELECT Distinct VF.SHAPEID, VF.SHAPENAME 
            FROM V_FinishedItemDetail VF(Nolock) 
            JOIN PROCESSCONSUMPTIONMASTER PCM(Nolock) ON PCM.FINISHEDID = VF.ITEM_FINISHED_ID 
            WHERE VF.ITEM_ID = " + ddItemName.SelectedValue + " And VF.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + " And VF.QualityID = " + ddQuality.SelectedValue;
        if (ddProcessName.SelectedIndex > 0)
        {
            Str = Str + " And PCM.ProcessID = " + ddProcessName.SelectedValue;
        }
        Str = Str + " Order By VF.SHAPENAME";

        Str = Str + @" SELECT Distinct VF.SHADECOLORID, VF.SHADECOLORNAME 
            FROM V_FinishedItemDetail VF(Nolock) 
            JOIN PROCESSCONSUMPTIONMASTER PCM(Nolock) ON PCM.FINISHEDID = VF.ITEM_FINISHED_ID 
            WHERE VF.ITEM_ID = " + ddItemName.SelectedValue + " And VF.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + " And VF.QualityID = " + ddQuality.SelectedValue;
        if (ddProcessName.SelectedIndex > 0)
        {
            Str = Str + " And PCM.ProcessID = " + ddProcessName.SelectedValue;
        }
        Str = Str + " Order By VF.SHADECOLORNAME";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref ddDesign, ds, 0, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref ddColor, ds, 1, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref ddShape, ds, 2, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref ddShade, ds, 3, true, "--SELECT--");
    }
    protected void ddDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = @" SELECT Distinct VF.ColorID, VF.COLORNAME 
            FROM V_FinishedItemDetail VF(Nolock) 
            JOIN PROCESSCONSUMPTIONMASTER PCM(Nolock) ON PCM.FINISHEDID = VF.ITEM_FINISHED_ID 
            WHERE VF.ITEM_ID = " + ddItemName.SelectedValue + " And VF.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + " And VF.QualityID = " + ddQuality.SelectedValue + @"
            And VF.DesignID = " + ddDesign.SelectedValue;
        if (ddProcessName.SelectedIndex > 0)
        {
            Str = Str + " And PCM.ProcessID = " + ddProcessName.SelectedValue;
        }
        Str = Str + " Order By VF.COLORNAME";

        Str = Str + @" SELECT Distinct VF.SHAPEID, VF.SHAPENAME 
            FROM V_FinishedItemDetail VF(Nolock) 
            JOIN PROCESSCONSUMPTIONMASTER PCM(Nolock) ON PCM.FINISHEDID = VF.ITEM_FINISHED_ID 
            WHERE VF.ITEM_ID = " + ddItemName.SelectedValue + " And VF.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + " And VF.QualityID = " + ddQuality.SelectedValue + @"
            And VF.DesignID = " + ddDesign.SelectedValue;
        if (ddProcessName.SelectedIndex > 0)
        {
            Str = Str + " And PCM.ProcessID = " + ddProcessName.SelectedValue;
        }
        Str = Str + " Order By VF.SHAPENAME";

        Str = Str + @" SELECT Distinct VF.SHADECOLORID, VF.SHADECOLORNAME 
            FROM V_FinishedItemDetail VF(Nolock) 
            JOIN PROCESSCONSUMPTIONMASTER PCM(Nolock) ON PCM.FINISHEDID = VF.ITEM_FINISHED_ID 
            WHERE VF.ITEM_ID = " + ddItemName.SelectedValue + " And VF.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + " And VF.QualityID = " + ddQuality.SelectedValue + @"
            And VF.DesignID = " + ddDesign.SelectedValue;
        if (ddProcessName.SelectedIndex > 0)
        {
            Str = Str + " And PCM.ProcessID = " + ddProcessName.SelectedValue;
        }
        Str = Str + " Order By VF.SHADECOLORNAME";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref ddColor, ds, 0, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref ddShape, ds, 1, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref ddShade, ds, 2, true, "--SELECT--");
    }
    protected void ddColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = @" SELECT Distinct VF.SHAPEID, VF.SHAPENAME 
            FROM V_FinishedItemDetail VF(Nolock) 
            JOIN PROCESSCONSUMPTIONMASTER PCM(Nolock) ON PCM.FINISHEDID = VF.ITEM_FINISHED_ID 
            WHERE VF.ITEM_ID = " + ddItemName.SelectedValue + " And VF.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + " And VF.QualityID = " + ddQuality.SelectedValue + @"
            And VF.DesignID = " + ddDesign.SelectedValue + " And VF.ColorID = " + ddColor.SelectedValue;
        if (ddProcessName.SelectedIndex > 0)
        {
            Str = Str + " And PCM.ProcessID = " + ddProcessName.SelectedValue;
        }
        Str = Str + " Order By VF.SHAPENAME";

        Str = Str + @" SELECT Distinct VF.SHADECOLORID, VF.SHADECOLORNAME 
            FROM V_FinishedItemDetail VF(Nolock) 
            JOIN PROCESSCONSUMPTIONMASTER PCM(Nolock) ON PCM.FINISHEDID = VF.ITEM_FINISHED_ID 
            WHERE VF.ITEM_ID = " + ddItemName.SelectedValue + " And VF.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + " And VF.QualityID = " + ddQuality.SelectedValue + @"
            And VF.DesignID = " + ddDesign.SelectedValue + " And VF.ColorID = " + ddColor.SelectedValue;
        if (ddProcessName.SelectedIndex > 0)
        {
            Str = Str + " And PCM.ProcessID = " + ddProcessName.SelectedValue;
        }
        Str = Str + " Order By VF.SHADECOLORNAME";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref ddShape, ds, 0, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref ddShade, ds, 1, true, "--SELECT--");
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void FillSize()
    {
        string size = "";
        string str = "";

        switch ("0")
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

        str = "Select Distinct VF.Sizeid, VF." + size + " + Space(2) + isnull(SizeNameAToC,'') As  " + size + @" 
            From V_FinishedItemDetail VF(Nolock) 
            JOIN PROCESSCONSUMPTIONMASTER PCM(Nolock) ON PCM.FINISHEDID = VF.ITEM_FINISHED_ID 
            Left Outer Join CustomerSize CS on VF.SizeId = CS.SizeId 
            Where VF.shapeid=" + ddShape.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        if (ddQuality.SelectedIndex > 0)
        {
            str = str + " And VF.QualityID = " + ddQuality.SelectedValue;
        }
        if (ddDesign.SelectedIndex > 0)
        {
            str = str + " And VF.DesignID = " + ddDesign.SelectedValue;
        }
        if (ddColor.SelectedIndex > 0)
        {
            str = str + " And VF.ColorID = " + ddColor.SelectedValue;
        }

        str = str + " order by " + size + "";

        UtilityModule.ConditionalComboFill(ref ddSize, str, true, "--SELECT--");
    }
    protected void ddProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        Report();
    }
    private void Report()
    {
        DataSet ds = new DataSet();

        string STR = "";

        if (ddQuality.SelectedIndex > 0)
        {
            STR = STR + @" AND VF.QualityId=" + ddQuality.SelectedValue + "";
        }
        if (ddDesign.SelectedIndex > 0)
        {
            STR = STR + @" AND VF.designId=" + ddDesign.SelectedValue + "";

        }
        if (ddColor.SelectedIndex > 0)
        {
            STR = STR + @" AND VF.ColorId=" + ddColor.SelectedValue + "";
        }
        if (ddShape.SelectedIndex > 0)
        {
            STR = STR + @" AND VF.ShapeId=" + ddShape.SelectedValue + "";
        }
        if (ddSize.SelectedIndex > 0)
        {
            STR = STR + @" AND VF.SizeId=" + ddSize.SelectedValue + "";
        }
        if (ddProcessName.SelectedIndex > 0)
        {
            STR = STR + @" AND PCM.PROCESSID=" + ddProcessName.SelectedValue + "";
        }
        ////STR = STR + " order by PCD.PCMDID";


        SqlParameter[] array = new SqlParameter[4];
        array[0] = new SqlParameter("@CategoryId", ddCategoryName.SelectedValue);
        array[1] = new SqlParameter("@ItemId", ddItemName.SelectedValue);
        array[2] = new SqlParameter("@where", STR);
        array[3] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_DefineBombAndConsumptionExcelReport", array);

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

            sht.Range("A1").Value = "DEFINE BOMB AND CONSUMPTION DETAILS";
            sht.Range("A1:M1").Style.Font.Bold = true;
            sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:M1").Merge();

            //Headers
            sht.Range("A3").Value = "PROCESS NAME";
            sht.Range("B3").Value = "CATEGORY";
            sht.Range("C3").Value = "ITEM NAME";
            sht.Range("D3").Value = "QUALITY NAME";
            sht.Range("E3").Value = "DESIGN NAME";
            sht.Range("F3").Value = "COLOR NAME";
            sht.Range("G3").Value = "SHAPE NAME";
            sht.Range("H3").Value = "SIZE NAME";

            sht.Range("I3").Value = "ITEM NAME";
            sht.Range("J3").Value = "QUALITY NAME";
            sht.Range("K3").Value = "SHADE COLORNAME";
            sht.Range("L3").Value = "CONSUMPTION QTY";


            //sht.Range("I1:R1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3:L3").Style.Font.Bold = true;

            row = 4;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Process_Name"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Category_Name"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["ShapeName"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["FinishingSize"]);

                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["InputOutputItemName"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["InputOutPutQualityName"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["InputOutPutShadeColorName"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["InputOutPutConsumptionQty"]);

                //sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["JobIssueULLNo"]);
                //sht.Range("L" + row).Style.NumberFormat.Format = "@";                

                row = row + 1;

            }
            sht.Range("I" + row + ":R" + row).Style.Font.Bold = true;
            sht.Columns(1, 20).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("DefineBombAndConsumptionReport:" + "_" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm") + "." + Fileextension);
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
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/main.aspx");
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }

}
