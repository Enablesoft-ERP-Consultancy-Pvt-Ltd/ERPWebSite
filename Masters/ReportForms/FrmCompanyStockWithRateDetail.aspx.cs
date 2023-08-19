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
public partial class Masters_ReportForms_FrmCompanyStockWithRateDetail : System.Web.UI.Page
{
    DataSet DS = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA 
                    Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                    Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"] + @"
                    Select ICM.CATEGORY_ID, ICM.CATEGORY_NAME 
                    From ITEM_CATEGORY_MASTER ICM(nolock)
                    JOIN CategorySeparate CS(nolock) ON CS.Categoryid = ICM.CATEGORY_ID 
                    Where CS.id = 1 And ICM.MasterCompanyid = " + Session["varCompanyId"] + @" 
                    Order By ICM.CATEGORY_NAME ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 1, false, "");

            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, " Select ");
            FillGodownMaster();
        }
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDCategory.SelectedItem.Text.Trim() != "ALL")
        {
            string str = @"Select ITEM_ID,ITEM_NAME from ITEM_MASTER(nolock) " + "WHERE CATEGORY_ID = " + DDCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyid"] + @" order by ITEM_NAME
                Select * From ITEM_CATEGORY_PARAMETERS(nolock) Where CATEGORY_ID = " + DDCategory.SelectedValue;

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref ddItemName, ds, 0, true, "ALL");
            if (ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    switch (dr["Parameter_Id"].ToString())
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
                        case "4":
                            TRDDShape.Visible = true;
                            break;
                        case "5":
                            TRDDSize.Visible = true;
                            break;
                        case "6":
                            TRDDShadeColor.Visible = true;
                            break;
                    }
                }
            }
            else
            {
                TRDDDesign.Visible = true;
                TRDDColor.Visible = true;
                TRDDShadeColor.Visible = true;
                TRDDShape.Visible = true;
                UtilityModule.ConditionalComboFillWithDS(ref DDDesign, ds, 0, true, "ALL");
                UtilityModule.ConditionalComboFillWithDS(ref DDColor, ds, 1, true, "ALL");
                UtilityModule.ConditionalComboFillWithDS(ref DDShape, ds, 3, true, "ALL");
                UtilityModule.ConditionalComboFillWithDS(ref DDShadeColor, ds, 2, true, "ALL");
            }
        }
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        TRDDQuality.Visible = true;
        if (ddItemName.SelectedItem.Text.Trim() != "ALL")
        {
            UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality(Nolock) Where Item_Id=" + ddItemName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyid"] + " order by QualityName", true, "ALL");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality(Nolock) Where MasterCompanyId=" + Session["varCompanyid"] + "  order by QualityName", true, "ALL");
        }
        DDQuality_SelectedIndexChanged(sender, e);
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        string qry = @"select DESIGNID,DESIGNNAME from DESIGN(Nolock) Where MasterCompanyId=" + Session["varCompanyid"] + @" ORDER BY DESIGNNAME
        select COLORID,COLORNAME from color(Nolock) Where MasterCompanyId=" + Session["varCompanyid"] + @" ORDER BY COLORNAME
        SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR(Nolock) Where MasterCompanyId=" + Session["varCompanyid"] + @" ORDER BY SHADECOLORNAME
        SELECT SHAPEID, SHAPENAME FROM SHAPE(Nolock) Where MasterCompanyId=" + Session["varCompanyid"] + " ORDER BY SHAPENAME";
        DataSet ds = SqlHelper.ExecuteDataset(qry);

        if (TRDDDesign.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref DDDesign, ds, 0, true, "ALL");
        }
        if (TRDDColor.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref DDColor, ds, 1, true, "ALL");
        }
        if (TRDDShape.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref DDShape, ds, 3, true, "ALL");
        }
        if (TRDDShadeColor.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref DDShadeColor, ds, 2, true, "ALL");
        }
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TRDDSize.Visible == true)
        {
            if (DDShape.SelectedItem.Text.Trim() != "ALL")
            {
                UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SIZEID, SIZEft AS SIZENAME FROM SIZE(Nolock) WHERE SHAPEID=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyid"] + " ORDER BY SIZEID", true, "ALL");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SIZEID, SIZEft AS SIZENAME FROM SIZE(Nolock) Where MasterCompanyId=" + Session["varCompanyid"] + " ORDER BY SIZEID", true, "ALL");
            }
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlCommand cmd = new SqlCommand("Proc_Get_StockWithPurchaseDyingRate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@CompanyID", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);
            //if (DDGodownName.SelectedIndex > 0)
            //{
            //    cmd.Parameters.AddWithValue("@GodownID", DDGodownName.SelectedValue);
            //}
            //else
            //{
            //    cmd.Parameters.AddWithValue("@GodownID", 0);
            //}

            cmd.Parameters.AddWithValue("@GodownID", DDGodownName.SelectedValue);
            cmd.Parameters.AddWithValue("@CategoryID", DDCategory.SelectedValue);
            cmd.Parameters.AddWithValue("@ItemID", ddItemName.SelectedValue);
            cmd.Parameters.AddWithValue("@QualityID", DDQuality.SelectedValue);

            if (TRDDDesign.Visible == true)
            {
                cmd.Parameters.AddWithValue("@DesignID", DDDesign.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@DesignID", 0);
            }

            if (TRDDColor.Visible == true)
            {
                cmd.Parameters.AddWithValue("@ColorID", DDColor.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ColorID", 0);
            }

            if (TRDDShape.Visible == true)
            {
                cmd.Parameters.AddWithValue("@ShapeID", DDShape.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ShapeID", 0);
            }

            if (TRDDSize.Visible == true)
            {
                cmd.Parameters.AddWithValue("@SizeID", DDSize.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@SizeID", 0);
            }

            if (TRDDShadeColor.Visible == true)
            {
                cmd.Parameters.AddWithValue("@ShadeColorID", DDShadeColor.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ShadeColorID", 0);
            }

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);

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

                sht.Range("A1:Y1").Merge();
                sht.Range("A1").SetValue("STOCK WITH RATE AMOUNT DETAIL REPORT");
                sht.Range("A1:Y1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:Y1").Style.Font.SetBold();

                sht.Range("A2").Value = "COMPANY NAME";
                sht.Range("B2").Value = "GODOWN NAME";
                sht.Range("C2").Value = "CATEGORY NAME";
                sht.Range("D2").Value = "ITEM NAME";
                sht.Range("E2").Value = "QUALITY NAME";
                sht.Range("F2").Value = "SHADECOLOR NAME";
                sht.Range("G2").Value = "LOT NO";
                sht.Range("H2").Value = "TAG NO";
                sht.Range("I2").Value = "BIN NO";
                sht.Range("J2").Value = "HAND SPINNING RATE";
                sht.Range("K2").Value = "MOTTELING RATE";
                sht.Range("L2").Value = "RAPIER RATE";
                sht.Range("M2").Value = "YARN OPENING RATE";
                sht.Range("N2").Value = "SAMPLING DYEING RATE";
                sht.Range("O2").Value = "PURCHASE RATE";
                sht.Range("P2").Value = "DYEING RATE";
                sht.Range("Q2").Value = "STOCK QTY";
                sht.Range("R2").Value = "HAND SPINNING AMOUNT";
                sht.Range("S2").Value = "MOTTELING AMOUNT";
                sht.Range("T2").Value = "RAPIER AMOUNT";
                sht.Range("U2").Value = "YARN OPENING AMOUNT";
                sht.Range("V2").Value = "SAMPLING DYEING AMOUNT";
                sht.Range("W2").Value = "PURCHASE AMOUNT";
                sht.Range("X2").Value = "DYEING AMOUNT";
                sht.Range("Y2").Value = "TOTAL AMOUNT";

                sht.Range("A2:Y2").Style.Font.Bold = true;

                row = 3;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CompanyName"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["GodownName"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CATEGORY_Name"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["LotNo"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["TagNo"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["BINNO"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["HandSpinningRate"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["MottelingRate"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["RapierRate"]);
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["YarnOpeningRate"]);
                    sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["SamplingDyeingRate"]);
                    sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["PurchaseRate"]);
                    sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["DyeingRate"]);
                    sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["StockQty"]);
                    sht.Range("R" + row).SetValue(ds.Tables[0].Rows[i]["HandSpinningAmount"]);
                    sht.Range("S" + row).SetValue(ds.Tables[0].Rows[i]["MottelingAmount"]);
                    sht.Range("T" + row).SetValue(ds.Tables[0].Rows[i]["RapierAmount"]);
                    sht.Range("U" + row).SetValue(ds.Tables[0].Rows[i]["YarnOpeningAmount"]);
                    sht.Range("V" + row).SetValue(ds.Tables[0].Rows[i]["SamplingDyeingAmount"]);
                    sht.Range("W" + row).SetValue(ds.Tables[0].Rows[i]["PurchaseAmount"]);
                    sht.Range("X" + row).SetValue(ds.Tables[0].Rows[i]["DyeingAmount"]);
                    sht.Range("Y" + row).SetValue(ds.Tables[0].Rows[i]["TotalAmount"]);

                    row = row + 1;
                }
                using (var a = sht.Range("A2:Y" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                sht.Columns(1, 20).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("STOCKWITHRATEAMOUNTDETAILREPORT" + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
                Path = Server.MapPath("~/Tempexcel/" + filename);
                xapp.SaveAs(Path);
                xapp.Dispose();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altrpt", "alert('No records found for this combination.')", true);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/main.aspx");
    }
    protected void DDBranchName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGodownMaster();
    }
    private void FillGodownMaster()
    {
        UtilityModule.ConditionalComboFill(ref DDGodownName, @"Select GodownID, GodownName 
        From GodownMaster(Nolock) Where BranchID = " + DDBranchName.SelectedValue + " And MasterCompanyID = " + Session["varCompanyId"] + @" 
        Order By GodownName ", true, "--Plz Select--");
    }
}

