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

public partial class Masters_ReportForms_FrmOnLoomInspectionEmpWiseReport : System.Web.UI.Page
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
                           select EI.EmpId,EI.EmpName + case When isnull(Ei.empcode,'')='' then '' else ' ['+EI.empcode+']' end as Empname  From EmpInfo  EI inner Join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentName='PRODUCTION' and Ei.MastercompanyId=" + Session["varcompanyId"] + @" order by EmpName";


            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 3, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDEmpName, ds, 4, true, "--Plz Select--");

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
    protected void DDEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        str = @"select distinct PLM.Uid,PLM.LoomNo from process_issue_master_1 PIM INNER JOIN ProductionLoomMaster PLM ON PIM.LoomId=PLM.UID
                INNER JOIN employee_processorderno EPO ON PIM.IssueOrderId=EPO.IssueOrderId
                INNER JOIN EmpInfo EI ON EPO.EmpId=EI.EmpId
                where PIM.Status='Pending' and PLM.CompanyId=" + DDcompany.SelectedValue + " and PLM.UnitId=" + DDProdunit.SelectedValue + "";

        if (DDEmpName.SelectedIndex > 0)
        {
            str = str + " and EI.EmpId=" + DDEmpName.SelectedValue;
        }
        str = str + " order by PLM.loomno";

        UtilityModule.ConditionalComboFill(ref DDLoomNo, str, true, "--SELECT--");
    }
    //    protected void DDProdunit_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        UtilityModule.ConditionalComboFill(ref DDLoomNo, @"select  PM.UID,PM.loomno+'/'+IM.ITEM_NAME as LoomNo from ProductionLoomMaster PM 
    //                                            inner join ITEM_MASTER IM on PM.Itemid=IM.ITEM_ID                                            
    //                                            Where  PM.CompanyId=" + DDcompany.SelectedValue + " and PM.UnitId=" + DDProdunit.SelectedValue + " order by case when ISNUMERIC(PM.loomno)=1 Then CONVERT(int,PM.loomno) Else 9999999 End,PM.loomno", true, "--Plz Select--");
    //    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string Where = "";
        if (DDProdunit.SelectedIndex > 0)
        {
            Where = Where + " and PIM.units=" + DDProdunit.SelectedValue;
        }
        if (DDEmpName.SelectedIndex > 0)
        {
            Where = Where + " and EI.EmpId=" + DDEmpName.SelectedValue;
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
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETONLOOMINSPPECTION_EMPWISE_REPORT", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/TempExcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/TempExcel/"));
            }
            //*************
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("ON LOOM INSPECTION EMP WISE");
            int row = 0;
            string Path = "";


            sht.Range("A1:R1").Merge();
            sht.Range("A1").Value = "DAILY PRODUCTION MONITORING";
            sht.Range("A1:R1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:R1").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A1:R1").Style.Font.FontSize = 10;
            sht.Range("A1:R1").Style.Font.Bold = true;

            sht.Range("A2").Value = "Sr.No";
            sht.Range("B2").Value = "Date";
            sht.Range("C2").Value = "Folio No";
            sht.Range("D2").Value = "Folio IssueDate";
            sht.Range("E2").Value = "Stock No";
            sht.Range("F2").Value = "Emp Name";
            sht.Range("G2").Value = "Emp Code";
            sht.Range("H2").Value = "Loom No";
            sht.Range("I2").Value = "Quality";
            sht.Range("J2").Value = "Design";
            sht.Range("K2").Value = "Color";
            sht.Range("L2").Value = "SizeCM";
            sht.Range("M2").Value = "SizeFt";
            sht.Range("N2").Value = "SizeINCH";
            sht.Range("O2").Value = "Production CM";
            sht.Range("P2").Value = "Production FT";
            sht.Range("Q2").Value = "Production INCH";
            sht.Range("R2").Value = "Production Wages CM";
            sht.Range("S2").Value = "Production Wages FT";
            sht.Range("T2").Value = "Production Wages INCH";
            sht.Range("U2").Value = "StockNo Cummulative Production";
            sht.Range("V2").Value = "COMPLETIONSTATUS";
            sht.Range("W2").Value = "StockNo Cummulative Wage";

            sht.Range("A2:W2").Style.Font.Bold = true;

            row = 3;
            int rowfrom = 3;
            string stockno = "";
            int srno = 0;

            DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "Date", "TStockNo");
            DataView dv1 = new DataView(dtdistinct);
            dv1.Sort = "Date";
            DataTable dtdistinct1 = dv1.ToTable();
            decimal Grandtotal = 0;
            foreach (DataRow dr in dtdistinct1.Rows)
            {
                if (stockno != dr["TStockNo"].ToString())
                {
                    stockno = dr["TStockNo"].ToString();
                    Grandtotal = 0;
                }

                DataView dvitemdesc = new DataView(ds.Tables[0]);
                dvitemdesc.RowFilter = "Date='" + dr["Date"] + "' and TStockNo='" + dr["TStockNo"] + "'";
                DataSet dsitemdesc = new DataSet();
                dsitemdesc.Tables.Add(dvitemdesc.ToTable());
                DataTable dtdistinctitemdesc = dsitemdesc.Tables[0];

                decimal total = 0;
                int mergerowfrom = 0;
                decimal MergeProductionWages = 0;
                decimal EmpShare = 0;
                decimal TotalProductionWages = 0;
                foreach (DataRow dritemdesc in dtdistinctitemdesc.Rows)
                {
                    if (mergerowfrom == 0)
                    {
                        mergerowfrom = row;
                    }

                    sht.Range("B" + row).SetValue(String.Format("{0:d/MMM/yyyy}", dritemdesc["Date"]));
                    sht.Range("C" + row).SetValue(dritemdesc["Issueorderid"]);
                    sht.Range("D" + row).SetValue(dritemdesc["AssignDate"]);
                    sht.Range("E" + row).SetValue(dritemdesc["TStockNo"]);
                    sht.Range("F" + row).SetValue(dritemdesc["EmpName"]);
                    sht.Range("G" + row).SetValue(dritemdesc["EmpCode"]);
                    sht.Range("H" + row).SetValue(dritemdesc["LoomNo"]);

                    sht.Range("I" + row).SetValue(dritemdesc["QualityName"]);
                    sht.Range("J" + row).SetValue(dritemdesc["designName"]);
                    sht.Range("K" + row).SetValue(dritemdesc["ColorName"]);
                    sht.Range("L" + row).SetValue(dritemdesc["SIZE"]);
                    sht.Range("M" + row).SetValue(dritemdesc["SIZEFT"]);
                    sht.Range("N" + row).SetValue(dritemdesc["SIZEINCH"]);
                    sht.Range("O" + row).SetValue(dritemdesc["ProductionCM"]);
                    sht.Range("P" + row).SetValue(dritemdesc["ProductionFT"]);
                    sht.Range("Q" + row).SetValue(dritemdesc["ProductionINCH"]);
                    sht.Range("R" + row).SetValue(dritemdesc["ProductionWagesCM"]);
                    sht.Range("S" + row).SetValue(dritemdesc["ProductionWagesFT"]);
                    sht.Range("T" + row).SetValue(dritemdesc["ProductionWagesINCH"]);

                    //sht.Range("Q" + row).SetValue(dritemdesc["StockNoCummulativeTotal"]);
                    sht.Range("V" + row).SetValue(dritemdesc["COMPLETIONSTATUS"]);
                    //sht.Range("N" + row).SetValue(0);

                    total = Convert.ToDecimal(dritemdesc["StockNoCummulativeTotal"]);

                    MergeProductionWages = Convert.ToDecimal(dritemdesc["ProductionWagesCM"]) + Convert.ToDecimal(dritemdesc["ProductionWagesFT"]) + Convert.ToDecimal(dritemdesc["ProductionWagesINCH"]);
                    EmpShare = Convert.ToDecimal(dritemdesc["ProductionCM"]) + Convert.ToDecimal(dritemdesc["ProductionFT"]) + Convert.ToDecimal(dritemdesc["ProductionINCH"]);
                    row = row + 1;
                }
                //Grandtotal = Grandtotal + total;
                TotalProductionWages = MergeProductionWages / EmpShare * total;
                sht.Range("U" + (row - 1)).SetValue(total);
                sht.Range("W" + (row - 1)).SetValue(TotalProductionWages);
                sht.Range("W" + (row - 1)).Style.NumberFormat.Format = "#,###0.00";

                //sht.Range("W" + (row - 1)).SetValue(String.Format("{0:#,0.00}", TotalProductionWages));

               
                row = row + 1;
                srno = srno + 1;

                //sht.Range("A2:U2").Merge();
                sht.Range("A" + mergerowfrom + ":A" + (row - 2)).Merge();
                sht.Range("A" + mergerowfrom).SetValue(srno);
                sht.Range("A" + mergerowfrom + ":A" + (row - 2)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + mergerowfrom + ":A" + (row - 2)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            }
            sht.Range("L" + row).SetValue("Grand Total");
            sht.Range("O" + row).FormulaA1 = "=SUM(O" + rowfrom + ":O" + (row - 1) + ")";
            sht.Range("P" + row).FormulaA1 = "=SUM(P" + rowfrom + ":P" + (row - 1) + ")";
            sht.Range("Q" + row).FormulaA1 = "=SUM(Q" + rowfrom + ":Q" + (row - 1) + ")";
            sht.Range("R" + row).FormulaA1 = "=SUM(R" + rowfrom + ":R" + (row - 1) + ")";
            sht.Range("S" + row).FormulaA1 = "=SUM(S" + rowfrom + ":S" + (row - 1) + ")";
            sht.Range("T" + row).FormulaA1 = "=SUM(T" + rowfrom + ":T" + (row - 1) + ")";
            sht.Range("U" + row).FormulaA1 = "=SUM(U" + rowfrom + ":U" + (row - 1) + ")";
            sht.Range("W" + row).FormulaA1 = "=SUM(W" + rowfrom + ":W" + (row - 1) + ")";

            //*************
            sht.Columns(1, 26).AdjustToContents();
            //********************
            //***********BOrders
            using (var a = sht.Range("A1" + ":W" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("OnLoomInspectionEmpWise_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "altrep", "alert('No records Found for this combination.');", true);
        }

    }
}