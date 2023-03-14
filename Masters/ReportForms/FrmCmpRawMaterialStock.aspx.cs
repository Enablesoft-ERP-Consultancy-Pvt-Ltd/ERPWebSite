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
public partial class Masters_ReportForms_FrmCmpRawMaterialStock : System.Web.UI.Page
{
    DataSet DS = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (RDOneDayReport.Checked == true)
        {
            TRDate.Visible = true;
        }
        else
        {
            TRDate.Visible = false;
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varCompanyId"] + @" Order by Companyname 
            Select customerid,customercode+'/'+Companyname from customerinfo Where MasterCompanyId=" + Session["varCompanyId"] + @" order by customercode            
            SELECT DISTINCT  LOTNO AS TEXTLOTNO,LOTNO FROM STOCK ORDER BY LOTNO
            select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " order by CATEGORY_NAME";
            DataSet ds = SqlHelper.ExecuteDataset(str);
            CommanFunction.FillComboWithDS(DDCompany, ds, 0);
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDcustomer, ds, 1, true, "ALL");           
            UtilityModule.ConditionalComboFillWithDS(ref DDLotNo, ds, 2, true, "ALL");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 3, true, "ALL");

            UtilityModule.ConditionalComboFill(ref ddItemName, "Select ITEM_ID,ITEM_NAME from ITEM_MASTER " + "WHERE CATEGORY_ID = " + DDCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by ITEM_NAME", true, "ALL");

            string str2 = "";
            if (Session["VarCompanyNo"].ToString() == "22")
            {
                 str2 = @"select GM.GODOWNID,GM.GODOWNNAME from GODOWNMASTER GM(NoLock) JOIN  Godown_Authentication GA(NoLock) ON GM.GoDownID=GA.GodownID 
                             Where GM.MasterCompanyId=" + Session["varCompanyId"] + @" and GA.UserId=" + Session["VarUserId"] + " ORDER BY GM.GODOWNNAME";
            }
            else
            {
                str2 = @"select GM.GODOWNID,GM.GODOWNNAME from GODOWNMASTER GM(NoLock) Where GM.MasterCompanyId=" + Session["varCompanyId"] + @"  ORDER BY GM.GODOWNNAME";
            }          

            DataSet ds2 = SqlHelper.ExecuteDataset(str2);
            UtilityModule.ConditionalComboFillWithDS(ref DDGudown, ds2, 0, true, "ALL");

            if (Session["VarCompanyNo"].ToString() == "38")
            {
                string str3 = "";
                str3 = "Select Process_Name_Id,Process_Name From Process_Name_Master Where ProcessType=1";

                UtilityModule.ConditionalComboFill(ref DDProcessName, str3, true, "---Select---");
            }
            
            
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtfromdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtstockupto.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            switch (Session["varcompanyId"].ToString())
            {
                case "16":
                    RDOneDayReport.Visible = false;
                    RDMonthlystock.Visible = false;
                    break;
                case "20":  //for ChampaDyeing
                    ChkForLedgerTransaction.Visible = true;
                    break;
                case "9": 
                    RDRawMaterialOrderAssignIssueStock.Visible = true;
                    break;
                case "38":
                    ChkForLedgerTransaction.Visible = false;
                    RDFinishedStockProcessWise.Visible = true;
                    break;
                //case "14":
                //    trwithvalue.Visible = true;
                //    break;
                case "44":
                    ChkForLedgerTransaction.Visible = false;
                    RDTotalStock.Visible = false;
                    RDOneDayReport.Visible = false;
                    RdShadewisestock.Visible = false;
                    RDrawmaterialopeningreport.Visible = false;
                    RDMonthlystock.Visible = false;
                    RDpurchaseissue_receivedetail.Visible = false;
                    RDPurchaseMaterialasondate.Visible = false;
                    RDRawMaterialStockGodownWise.Visible = false;
                    RDfinishedstockasondate.Visible = false;
                    RDRawMaterialStockAsOnDate.Visible = true;
                    break;  
                default:
                    ChkForLedgerTransaction.Visible = false;
                    break;
            }
        }
    }
    public void FinishedStock()
    {
        lblMessage.Text = "";
        DataSet DS = new DataSet();
        String sQry = " ", Size = "Sizeft";
        if (chkformtr.Checked == true)
        {
            Size = "Sizemtr";
        }
//        sQry = @"SELECT isnull(c.CustomerCode,'Direct Stock') AS BuyersCode,o.LocalOrder AS BuyerOrderNo,v.Designname AS DesignName,v.QualityName AS Quality,v.ColorName AS Color,v." + Size + @" AS Size, 
//                count(cr.Item_finished_ID) AS StockQTY,dbo.F_GetFinishedstock_orderwise(Cr.item_finished_id,cr.orderid,cr.pack,CR.Confirm,isnull(cr.DirectStockRemark,'')) as StockNo,CI.companyname,ci.compaddr1,ci.gstno,
//                isnull(cr.DirectStockRemark,'') as DirectStockRemark                
//                from carpetnumber cr inner join companyinfo ci on cr.companyid=ci.companyid left join ordermaster o on cr.orderid=o.orderid left join customerinfo c on 
//                o.customerid= c.customerid left join v_finishedItemDetail v on cr.Item_Finished_Id = v.Item_Finished_ID WHERE cr.companyid=" + DDCompany.SelectedValue;
        sQry = @"SELECT isnull(c.CustomerCode,'Direct Stock') AS BuyersCode,o.LocalOrder AS BuyerOrderNo,v.CATEGORY_NAME,v.ITEM_NAME,v.Designname AS DesignName,v.QualityName AS Quality,v.ColorName AS Color,v." + Size + @" AS Size, 
                count(cr.Item_finished_ID) AS StockQTY,";
        if (Session["varcompanyId"].ToString() == "44")
        {
            sQry = sQry + "dbo.F_GetFinishedstock_orderwise_agni(Cr.item_finished_id,cr.orderid,cr.pack,CR.Confirm,isnull(cr.DirectStockRemark,'')) as StockNo";
        }
        else
        {
            sQry = sQry + "dbo.F_GetFinishedstock_orderwise(Cr.item_finished_id,cr.orderid,cr.pack,CR.Confirm,isnull(cr.DirectStockRemark,'')) as StockNo";
        }


        sQry = sQry + ",CI.companyname,ci.compaddr1,ci.gstno,isnull(cr.DirectStockRemark,'') as DirectStockRemark from carpetnumber cr inner join companyinfo ci on cr.companyid=ci.companyid left join ordermaster o on cr.orderid=o.orderid left join customerinfo c on o.customerid= c.customerid left join v_finishedItemDetail v on cr.Item_Finished_Id = v.Item_Finished_ID WHERE cr.companyid=" + DDCompany.SelectedValue;
        if (chkallstockno.Checked == false)
        {
            sQry = sQry + " and CR.PACK=0";
        }
        if (chkunconfirmcarpet.Checked == true)
        {
            sQry = sQry + " and isnull(CR.Confirm,0)=0";
        }
        if (DDcustomer.SelectedIndex > 0)
        {
            sQry = sQry + " AND o.customerid= " + DDcustomer.SelectedValue;
        }
        if (DDOrder.SelectedIndex > 0)
        {
            sQry = sQry + " AND cr.orderid= " + DDOrder.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.QUALITYID = " + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.DESIGNID = " + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.COLORID = " + DDColor.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SHADECOLORID = " + DDSize.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SHAPEID = " + DDShape.SelectedValue;

        }
        if (DDSize.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SIZEID = " + DDSize.SelectedValue;
        }
        sQry = sQry + @" group by CI.companyname,ci.compaddr1,ci.gstno,c.CustomerCode,o.LocalOrder,v.Designname,v.QualityName,v.ColorName,v." + Size + ", Cr.Item_Finished_ID,cr.orderid,cr.pack,CR.Confirm,cr.DirectStockRemark,v.CATEGORY_NAME,v.ITEM_NAME Order By c.CustomerCode";
        DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);
        if (DS.Tables[0].Rows.Count > 0)
        {
            if (Session["varCompanyId"].ToString() == "36")
            {
                if (ChkExportExcel.Checked == true)
                {
                    FinishedStockExcelExport(DS);
                }
                else
                {
                    Session["rptFileName"] = "reports/CarpetStockBalanceReportPrasad.rpt";
                }
            }
            else if (Session["varCompanyId"].ToString() == "44")
            {
                Session["rptFileName"] = "reports/CarpetStockBalanceReportagni.rpt";
            }
            else
            {
                Session["rptFileName"] = "reports/CarpetStockBalanceReport.rpt";
            }

            Session["GetDataset"] = DS;
            Session["dsFileName"] = "~\\ReportSchema\\CarpetStockBalance.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }

    protected void Rawmaterialstocksummary_Excel()
    {
        lblMessage.Text = "";
        int Row;
        DataSet DS = new DataSet();
        String sQry = " ";
        string shadecolor = "";
        string FilterBy = "Filter By-" + DDCompany.SelectedItem.Text;
        try
        {

            if (RDundyed.Checked == true)
            {
                shadecolor = "Undyed";
            }
            else if (RDDyed.Checked == true)
            {
                shadecolor = "Dyed";
            }
            switch (Session["varcompanyNo"].ToString())
            {
                case "14":
                    sQry = @"SELECT g.godownname,Round(Sum(s.qtyinhand),3) qtyinhand,v.category_name,v.item_name,v.qualityname,'" + shadecolor + @"' AS Description
                     from stock s,godownmaster g,v_finishedItemDetail v Where s.godownid=g.godownid And s.Item_Finished_ID=v.Item_Finished_ID And s.companyid = " + DDCompany.SelectedValue + "  And V.MasterCompanyId=" + Session["varCompanyId"] + " and Round(S.qtyinhand,3)>0";
                    break;
                default:
                    sQry = @"select  g.godownname,Round(Sum(case when St.trantype=1 Then St.quantity else 0 End)-Sum(case when St.trantype=0 Then St.quantity else 0 End),3) qtyinhand,
                                        v.category_name,v.item_name,v.qualityname,'" + shadecolor + @"' AS Description
                                        From stock s inner join stockTran St on s.StockID=st.Stockid
                                        inner join GodownMaster g on s.Godownid=g.GoDownID
                                        inner join V_FinishedItemDetail v on s.ITEM_FINISHED_ID=v.ITEM_FINISHED_ID
                                        Where s.companyid = " + DDCompany.SelectedValue + "  And V.MasterCompanyId=" + Session["varCompanyId"] + "";


                    if (TDstockupto.Visible == true)
                    {
                        sQry = sQry + " and St.TranDate<='" + txtstockupto.Text + "'";
                        FilterBy = FilterBy + ",Stock Up to - " + txtstockupto.Text;
                    }
                    break;
            }
            if (RDundyed.Checked == true)
            {
                sQry = sQry + " and v.Shadecolorname like '%Undyed%'";
                FilterBy = FilterBy + ",Shade - " + shadecolor;
            }
            else if (RDDyed.Checked == true)
            {
                sQry = sQry + " and v.Shadecolorname not like '%Undyed%'";
                FilterBy = FilterBy + ",Shade - " + shadecolor;
            }
            if (DDGudown.SelectedIndex > 0)
            {
                sQry = sQry + "AND g.godownid =" + DDGudown.SelectedValue;
                FilterBy = FilterBy + ",Godown - " + DDGudown.SelectedItem.Text;
            }
            if (DDCategory.SelectedIndex > 0)
            {
                sQry = sQry + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;

            }
            if (ddItemName.SelectedIndex > 0)
            {
                sQry = sQry + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
                FilterBy = FilterBy + ",Item - " + ddItemName.SelectedItem.Text;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                sQry = sQry + " AND v.QUALITYID = " + DDQuality.SelectedValue;
                FilterBy = FilterBy + ",Quality - " + DDQuality.SelectedItem.Text;

            }
            if (DDDesign.SelectedIndex > 0)
            {
                sQry = sQry + " AND v.DESIGNID = " + DDDesign.SelectedValue;

            }
            if (DDColor.SelectedIndex > 0)
            {
                sQry = sQry + " AND v.COLORID = " + DDColor.SelectedValue;
            }
            if (DDShadeColor.SelectedIndex > 0)
            {
                sQry = sQry + " AND v.SHADECOLORID = " + DDShadeColor.SelectedValue;
                FilterBy = FilterBy + ",Shadecolor - " + DDShadeColor.SelectedItem.Text;

            }
            if (DDSize.SelectedIndex > 0)
            {
                sQry = sQry + " AND v.Sizeid = " + DDSize.SelectedValue;

            }
            if (DDLotNo.SelectedIndex > 0)
            {
                sQry = sQry + " AND s.lotno = " + DDLotNo.SelectedValue;
                FilterBy = FilterBy + ",Lotno - " + DDLotNo.SelectedItem.Text;
            }
            if (txtLotno.Text != "")
            {
                sQry = sQry + " AND s.lotno = '" + txtLotno.Text + "'";
                FilterBy = FilterBy + ",Lotno - " + txtLotno.Text;
            }
            if (txtTagno.Text != "")
            {
                sQry = sQry + " AND s.Tagno = '" + txtTagno.Text + "'";
                FilterBy = FilterBy + ",Tagno - " + txtTagno.Text;
            }

            sQry = sQry + " group by g.godownname,v.category_name,v.item_name,v.qualityname order by qualityname";

            DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);
            if (DS.Tables[0].Rows.Count > 0)
            {
                string Path = "";

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("Stock Summary");
                //*************
                sht.Range("A1:D1").Merge();
                sht.Range("A1:D1").Style.Font.FontSize = 11;
                sht.Range("A1:D1").Style.Font.Bold = true;
                sht.Range("A1:D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:D1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1").SetValue("STOCK SUMMARY(" + shadecolor + ")");
                sht.Row(1).Height = 21.75;
                //
                sht.Range("A2:D2").Merge();
                sht.Range("A2:D2").Style.Font.FontSize = 11;
                sht.Range("A2:D2").Style.Font.Bold = true;
                sht.Range("A2:D2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:D2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A2").SetValue(FilterBy.TrimStart(','));
                sht.Row(2).Height = 21.75;
                //Header
                sht.Range("A3:D3").Style.Font.FontSize = 11;
                sht.Range("A3:D3").Style.Font.Bold = true;
                sht.Range("A3:D3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("D3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Row(3).Height = 18.00;
                //
                sht.Range("A3").SetValue("Item Name");
                sht.Range("B3").SetValue("Colour");
                sht.Range("C3").SetValue("Godown Name");
                sht.Range("D3").SetValue("Stock Qty(Kgs.)");
                Row = 4;
                Decimal Tqty = 0;
                for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + Row + ":D" + Row).Style.Font.FontSize = 11;

                    sht.Range("A" + Row).SetValue(DS.Tables[0].Rows[i]["Qualityname"]);
                    sht.Range("B" + Row).SetValue(DS.Tables[0].Rows[i]["Description"]);
                    sht.Range("C" + Row).SetValue(DS.Tables[0].Rows[i]["Godownname"]);
                    sht.Range("D" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("D" + Row).Style.NumberFormat.Format = "#,##0.000";
                    sht.Range("D" + Row).SetValue(Convert.ToDecimal(DS.Tables[0].Rows[i]["qtyinhand"]));
                    Tqty = Tqty + Convert.ToDecimal(DS.Tables[0].Rows[i]["qtyinhand"]);
                    Row = Row + 1;
                }
                // Total
                sht.Range("D" + Row).Style.Font.FontSize = 11;
                sht.Range("D" + Row).Style.Font.Bold = true;
                sht.Range("D" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("D" + Row).Style.NumberFormat.Format = "#,##0.000";
                sht.Range("D" + Row).SetValue(Tqty);
                //**********
                sht.Columns(1, 10).AdjustToContents();
                //**************Save
                //******SAVE FILE
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("StockSummary(" + shadecolor + ")" + DateTime.Now + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
    public void RawMaterialStock()
    {
        lblMessage.Text = "";
        DataSet DS = new DataSet();
        String sQry = " ";

        if (chkstocksummary.Checked == true)
        {
            Rawmaterialstocksummary_Excel();
            return;
        }
        else
        {
            switch (Session["varcompanyId"].ToString())
            {
                case "14":
                case "16":
                case "28":
                case "44":
                case "21":
                    if (variable.VarBINNOWISE == "1")
                    {
                        Rawmaterialstock_Excel_BinWise();
                    }
                    else
                    {
                        if (Session["varcompanyId"].ToString() == "14")
                        {
                            if (chkwithval.Checked)
                            {
                                Rawmaterialstock_Excel_eastrn();
                            }
                            else
                            { Rawmaterialstock_Excel(); }



                        }
                        else if (Session["varcompanyId"].ToString() == "44")
                        {
                            Rawmaterialstock_Excel_AGNI();

                        }   
                        else { Rawmaterialstock_Excel(); }
                    }

                    break;
                default:
                    if (ChkForTotalStock.Checked == true)
                    {
                        Rawmaterialstock_Excel();
                    }
                    else
                    {
                        sQry = @"SELECT g.godownname,Round(Sum(s.qtyinhand),3) qtyinhand,s.lotno,v.category_name,v.item_name,v.qualityname,designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeMtr AS Description,S.TagNo,ci.companyname,ci.compaddr1,ci.gstno,CI.MasterCompanyid
                                From Stock S JOIN companyinfo ci ON S.Companyid=CI.CompanyId
                                JOIN GodownMaster g ON S.Godownid=g.GoDownID
                                JOIN Godown_Authentication GA ON g.GoDownID=GA.Godownid and GA.Userid=" + Session["varUserId"]+@"
                                JOIN v_finishedItemDetail v ON s.Item_Finished_ID=v.Item_Finished_ID
                                Where s.companyid = " + DDCompany.SelectedValue + "  And V.MasterCompanyId=" + Session["varCompanyId"];
                        if (DDGudown.SelectedIndex > 0)
                        {
                            sQry = sQry + "AND g.godownid =" + DDGudown.SelectedValue;
                        }
                        if (DDCategory.SelectedIndex > 0)
                        {
                            sQry = sQry + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;

                        }
                        if (ddItemName.SelectedIndex > 0)
                        {
                            sQry = sQry + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
                        }
                        if (DDQuality.SelectedIndex > 0)
                        {
                            sQry = sQry + " AND v.QUALITYID = " + DDQuality.SelectedValue;

                        }
                        if (DDDesign.SelectedIndex > 0)
                        {
                            sQry = sQry + " AND v.DESIGNID = " + DDDesign.SelectedValue;

                        }
                        if (DDColor.SelectedIndex > 0)
                        {
                            sQry = sQry + " AND v.COLORID = " + DDColor.SelectedValue;
                        }
                        if (DDShadeColor.SelectedIndex > 0)
                        {
                            sQry = sQry + " AND v.SHADECOLORID = " + DDShadeColor.SelectedValue;

                        }
                        if (DDSize.SelectedIndex > 0)
                        {
                            sQry = sQry + " AND v.SHAPEID = " + DDSize.SelectedValue;

                        }
                        if (DDLotNo.SelectedIndex > 0)
                        {
                            sQry = sQry + " AND s.lotno = " + DDLotNo.SelectedValue;
                        }
                        if (txtLotno.Text != "")
                        {
                            sQry = sQry + " AND s.lotno = '" + txtLotno.Text + "'";
                        }
                        if (txtTagno.Text != "")
                        {
                            sQry = sQry + " AND s.Tagno = '" + txtTagno.Text + "'";

                        }
                        sQry = sQry + " group by ci.companyname,ci.compaddr1,ci.gstno,g.godownname,s.lotno,v.category_name,v.item_name,v.qualityname,designName,ColorName,ShadeColorName,ShapeName,SizeMtr,S.TagNo,CI.MasterCompanyid";

                        sQry = sQry + " Order by v.item_name,v.qualityname,designName,ColorName,ShadeColorName";

                        DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);
                        if (DS.Tables[0].Rows.Count > 0)
                        {
                            if (chkstocksummary.Checked == true)
                            {
                                Session["rptFileName"] = "reports/rptstockreportsummary.rpt";
                            }
                            else
                            {
                                Session["rptFileName"] = "reports/StockReport.rpt";
                            }
                            Session["GetDataset"] = DS;
                            Session["dsFileName"] = "~\\ReportSchema\\StockReport.xsd";
                            StringBuilder stb = new StringBuilder();
                            stb.Append("<script>");
                            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                        }
                        
                    }
                    break;
                    
            }
        }

    }
    protected void Rawmaterialstock_Excel()
    {
        string sQry, filterby = "Filter By-" + DDCompany.SelectedItem.Text, Path;
        string shadecolor = "";
        //DataSet DS;

        if (RDundyed.Checked == true)
        {
            shadecolor = "Undyed";
        }
        else if (RDDyed.Checked == true)
        {
            shadecolor = "Dyed";
        }

        sQry = @"SELECT g.godownname,Round(Sum(s.qtyinhand),3) qtyinhand,s.lotno,v.category_name,v.item_name,v.qualityname,designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeMtr AS Description,S.TagNo
        from stock s,godownmaster g,v_finishedItemDetail v Where s.godownid=g.godownid And s.Item_Finished_ID=v.Item_Finished_ID And s.companyid = " + DDCompany.SelectedValue + "  And V.MasterCompanyId=" + Session["varCompanyId"];

        if (DDGudown.SelectedIndex > 0)
        {
            sQry = sQry + "AND g.godownid =" + DDGudown.SelectedValue;
            filterby = filterby + " Godown-" + DDGudown.SelectedItem.Text;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;

        }
        if (ddItemName.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
            filterby = filterby + ",Item-" + ddItemName.SelectedItem.Text;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.QUALITYID = " + DDQuality.SelectedValue;
            filterby = filterby + ",Quality-" + DDQuality.SelectedItem.Text;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.DESIGNID = " + DDDesign.SelectedValue;
            filterby = filterby + ",Design-" + DDDesign.SelectedItem.Text;

        }
        if (DDColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.COLORID = " + DDColor.SelectedValue;
            filterby = filterby + ",Color-" + DDColor.SelectedItem.Text;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SHADECOLORID = " + DDShadeColor.SelectedValue;
            filterby = filterby + ",ShadeColor-" + DDShadeColor.SelectedItem.Text;

        }
        if (TDundyed_dyed.Visible == true)
        {
            if (RDundyed.Checked == true)
            {
                sQry = sQry + " and v.Shadecolorname like '%Undyed%'";
                filterby = filterby + ",Shade - " + shadecolor;
            }
            else if (RDDyed.Checked == true)
            {
                sQry = sQry + " and v.Shadecolorname not like '%Undyed%'";
                filterby = filterby + ",Shade - " + shadecolor;
            }
        }
        if (DDSize.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.Sizeid = " + DDSize.SelectedValue;

        }
        if (DDLotNo.SelectedIndex > 0)
        {
            sQry = sQry + " AND s.lotno = " + DDLotNo.SelectedValue;
            filterby = filterby + ",LotNo-" + DDLotNo.SelectedItem.Text;
        }
        if (txtLotno.Text != "")
        {
            sQry = sQry + " AND s.lotno = '" + txtLotno.Text + "'";
            filterby = filterby + ",LotNo-" + txtLotno.Text;
        }
        if (txtTagno.Text != "")
        {
            sQry = sQry + " AND s.TagNo = '" + txtTagno.Text + "'";
            filterby = filterby + ",TagNo-" + txtTagno.Text;
        }
        filterby = filterby + ",Date - " + DateTime.Now.ToString("dd-MMM-yyyy");
        sQry = sQry + " group by g.godownname,s.lotno,v.category_name,v.item_name,v.qualityname,designName,ColorName,ShadeColorName,ShapeName,SizeMtr,S.TagNo";

        //DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);       

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand(sQry, con);
        //cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        DataSet DS = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(DS);
        //*************

        con.Close();
        con.Dispose();


        if (DS.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Raw Material Stock");
            //************

            sht.Range("A1:G1").Merge();
            sht.Range("A1").SetValue("STOCK REPORT");
            sht.Range("A1:G1").Style.Font.Bold = true;
            sht.Range("A1:G1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //Filter by 
            sht.Row(2).Height = 26.25;
            sht.Range("A2:G2").Merge();
            sht.Range("A2").SetValue(filterby.TrimStart(','));
            sht.Range("A2:G2").Style.Font.Bold = true;
            sht.Range("A2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:G2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:G2").Style.Alignment.WrapText = true;
            //Headings
            sht.Range("A3:G3").Style.Font.Bold = true;
            sht.Range("G3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3").SetValue("Item Name");
            sht.Range("B3").SetValue("Quality");
            sht.Range("C3").SetValue("Description");
            sht.Range("D3").SetValue("Lot No.");
            sht.Range("E3").SetValue("Tag No.");
            sht.Range("F3").SetValue("Godown");
            if (Session["varcompanyNo"].ToString() == "44")
            {
                sht.Range("G3").SetValue("Stock Qty");

            }
            else
            {
                sht.Range("G3").SetValue("Stock Qty(Kgs.)");
            }
            //***************
            int row = 4;
            DataView dv = DS.Tables[0].DefaultView;
            switch (Session["varcompanyNo"].ToString())
            {
                case "14":
                    dv.RowFilter = "Qtyinhand>0";
                    break;
                default:
                    dv.RowFilter = "Qtyinhand<>0";
                    break;
            }
            dv.Sort = "Item_Name,Qualityname,Description,Lotno,TagNo";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());

            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Item_Name"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Description"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["TagNo"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["godownname"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Qtyinhand"]);
                sht.Range("G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("G" + row).Style.NumberFormat.Format = "#,##0.000";
                row = row + 1;
            }
            ds1.Dispose();
            DS.Dispose();
            //grand Total:
            var sum = sht.Evaluate("SUM(G4:G" + (row - 1) + ")");
            sht.Range("G" + row).Value = sum;
            sht.Range("G" + row).Style.Font.Bold = true;
            sht.Range("G" + row).Style.NumberFormat.Format = "#,##0.000";
            //**********

            sht.Columns(1, 8).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("StockReport_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }
    }
    protected void Rawmaterialstock_Excel_AGNI()
    {
        string sQry, filterby = "Filter By-" + DDCompany.SelectedItem.Text, Path;
        string shadecolor = "";
        //DataSet DS;

        if (RDundyed.Checked == true)
        {
            shadecolor = "Undyed";
        }
        else if (RDDyed.Checked == true)
        {
            shadecolor = "Dyed";
        }

        sQry = @"SELECT g.godownname,Round(Sum(s.qtyinhand),3) qtyinhand,s.lotno,v.category_name,v.item_name,v.qualityname,designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeMtr AS Description,S.TagNo,ci.CompanyName,ci.CompAddr1,ci.GSTNo
        from stock s join godownmaster g on s.godownid=g.godownid join v_finishedItemDetail v on s.Item_Finished_ID=v.Item_Finished_ID  join companyinfo ci on s.Companyid=ci.CompanyId Where s.godownid=g.godownid And s.Item_Finished_ID=v.Item_Finished_ID And s.companyid = " + DDCompany.SelectedValue + "  And V.MasterCompanyId=" + Session["varCompanyId"];

        if (DDGudown.SelectedIndex > 0)
        {
            sQry = sQry + "AND g.godownid =" + DDGudown.SelectedValue;
            filterby = filterby + " Godown-" + DDGudown.SelectedItem.Text;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;

        }
        if (ddItemName.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
            filterby = filterby + ",Item-" + ddItemName.SelectedItem.Text;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.QUALITYID = " + DDQuality.SelectedValue;
            filterby = filterby + ",Quality-" + DDQuality.SelectedItem.Text;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.DESIGNID = " + DDDesign.SelectedValue;
            filterby = filterby + ",Design-" + DDDesign.SelectedItem.Text;

        }
        if (DDColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.COLORID = " + DDColor.SelectedValue;
            filterby = filterby + ",Color-" + DDColor.SelectedItem.Text;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SHADECOLORID = " + DDShadeColor.SelectedValue;
            filterby = filterby + ",ShadeColor-" + DDShadeColor.SelectedItem.Text;

        }
        if (TDundyed_dyed.Visible == true)
        {
            if (RDundyed.Checked == true)
            {
                sQry = sQry + " and v.Shadecolorname like '%Undyed%'";
                filterby = filterby + ",Shade - " + shadecolor;
            }
            else if (RDDyed.Checked == true)
            {
                sQry = sQry + " and v.Shadecolorname not like '%Undyed%'";
                filterby = filterby + ",Shade - " + shadecolor;
            }
        }
        if (DDSize.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.Sizeid = " + DDSize.SelectedValue;

        }
        if (DDLotNo.SelectedIndex > 0)
        {
            sQry = sQry + " AND s.lotno = " + DDLotNo.SelectedValue;
            filterby = filterby + ",LotNo-" + DDLotNo.SelectedItem.Text;
        }
        if (txtLotno.Text != "")
        {
            sQry = sQry + " AND s.lotno = '" + txtLotno.Text + "'";
            filterby = filterby + ",LotNo-" + txtLotno.Text;
        }
        if (txtTagno.Text != "")
        {
            sQry = sQry + " AND s.TagNo = '" + txtTagno.Text + "'";
            filterby = filterby + ",TagNo-" + txtTagno.Text;
        }
        filterby = filterby + ",Date - " + DateTime.Now.ToString("dd-MMM-yyyy");
        sQry = sQry + " group by g.godownname,s.lotno,v.category_name,v.item_name,v.qualityname,designName,ColorName,ShadeColorName,ShapeName,SizeMtr,S.TagNo,ci.CompanyName,ci.CompAddr1,ci.GSTNo";

        //DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);       

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand(sQry, con);
        //cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        DataSet DS = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(DS);
        //*************

        con.Close();
        con.Dispose();


        if (DS.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Raw Material Stock");
            //************

            sht.Range("A1:H1").Merge();
            sht.Range("A1").SetValue(DS.Tables[0].Rows[0]["CompanyName"].ToString());
            sht.Range("A1:H1").Style.Font.Bold = true;
            sht.Range("A1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //Filter by 
            sht.Row(2).Height = 26.25;
            sht.Range("A2:H2").Merge();
            sht.Range("A2").SetValue(DS.Tables[0].Rows[0]["CompAddr1"].ToString());
            sht.Range("A2:H2").Style.Font.Bold = true;
            sht.Range("A2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:H2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:H2").Style.Alignment.WrapText = true;
            sht.Row(3).Height = 26.25;
            sht.Range("A3:H3").Merge();
            sht.Range("A3").SetValue("GST No."+DS.Tables[0].Rows[0]["GSTNO"].ToString());
            sht.Range("A3:H3").Style.Font.Bold = true;
            sht.Range("A3:H3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A3:H3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A3:H3").Style.Alignment.WrapText = true;
            sht.Row(4).Height = 26.25;
            sht.Range("A4:H4").Merge();
            sht.Range("A4").SetValue("Stock Report " + ", Print Date - " + DateTime.Now.ToString("dd-MMM-yyyy"));
            sht.Range("A4:H4").Style.Font.Bold = true;
            sht.Range("A4:H4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A4:H4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A4:H4").Style.Alignment.WrapText = true;
            //Headings
            sht.Range("A5:H5").Style.Font.Bold = true;
            sht.Range("H5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A5").SetValue("Category Name");
            sht.Range("B5").SetValue("Item Name");
            sht.Range("C5").SetValue("Quality");
            sht.Range("D5").SetValue("Description");
            sht.Range("E5").SetValue("Lot No.");
            sht.Range("F5").SetValue("Tag No.");
            sht.Range("G5").SetValue("Godown");
            if (Session["varcompanyNo"].ToString() == "44")
            {
                sht.Range("H5").SetValue("Stock Qty");

            }
            else
            {
                sht.Range("H5").SetValue("Stock Qty(Kgs.)");
            }
            //***************
            int row = 6;
            DataView dv = DS.Tables[0].DefaultView;
            switch (Session["varcompanyNo"].ToString())
            {
                case "14":
                    dv.RowFilter = "Qtyinhand>0";
                    break;
                default:
                    dv.RowFilter = "Qtyinhand<>0";
                    break;
            }
            dv.Sort = "Item_Name,Qualityname,Description,Lotno,TagNo";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());

            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["category_name"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Item_Name"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Description"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["TagNo"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["godownname"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Qtyinhand"]);
                sht.Range("H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("H" + row).Style.NumberFormat.Format = "#,##0.000";
                row = row + 1;
            }
            ds1.Dispose();
            DS.Dispose();
            //grand Total:
            var sum = sht.Evaluate("SUM(H4:H" + (row - 1) + ")");
            sht.Range("H" + row).Value = sum;
            sht.Range("H" + row).Style.Font.Bold = true;
            sht.Range("H" + row).Style.NumberFormat.Format = "#,##0.000";
            //**********

            sht.Columns(1, 8).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("StockReport_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }
    }
    protected void Rawmaterialstock_Excel_eastrn()
    {
        string sQry, filterby = "Filter By-" + DDCompany.SelectedItem.Text, Path;
        string shadecolor = "";
        //DataSet DS;

        if (RDundyed.Checked == true)
        {
            shadecolor = "Undyed";
        }
        else if (RDDyed.Checked == true)
        {
            shadecolor = "Dyed";
        }

        //        sQry = @"SELECT g.godownname,Round(Sum(s.qtyinhand),3) qtyinhand,s.lotno,v.category_name,v.item_name,v.qualityname,designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeMtr AS Description,S.TagNo,Case when V.MasterCompanyId in(16,14) Then Case When vp.rate is null Then isnull((Select Max(Rate) From PurchaseReceiveDetail PRD(Nolock) Where PRD.LotNo = s.LotNo),0) Else isnull(vp.rate,0) End Else isnull(vp.rate,0) End Rate,v.UnitType,
        //        isnull((select Top (1) isnull(PPRT.Rate,0) as DyeingRate from PP_ProcessRecMaster PPRM JOIN PP_ProcessRecTran PPRT ON PPRM.PRMid=PPRT.PRMID
        //                Where PPRT.Finishedid=s.ITEM_FINISHED_ID Order by PPRM.Date desc),0) as LastDyeingRate
        //        from stock s 
        //        inner join V_FinishedItemDetail v on s.ITEM_FINISHED_ID=v.ITEM_FINISHED_ID  
        //        inner join GodownMaster g on s.godownid=g.godownid 
        //        left JOIN V_PURCHASELOTVENDORDETAIL VP ON S.ITEM_FINISHED_ID=VP.FINISHEDID AND S.LOTNO=VP.LOTNO AND S.TAGNO=VP.TAGNO 
        //        Where s.companyid = " + DDCompany.SelectedValue + "  And V.MasterCompanyId=" + Session["varCompanyId"];

        sQry = @"SELECT g.godownname,Round(Sum(s.qtyinhand),3) qtyinhand,s.lotno,v.category_name,v.item_name,v.qualityname,designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeMtr AS Description,S.TagNo,
        isnull(Case when V.MasterCompanyId in(14) Then Case When IsNull(vp.rate, 0) = 0 
            Then Case When isnull((Select Max(Rate) From PurchaseReceiveDetail PRD2(Nolock) Where PRD2.FinishedId=S.ITEM_FINISHED_ID and PRD2.LotNo = s.LotNo),0)=0 
            Then Case When isnull((Select Max(Rate) From PurchaseReceiveDetail PRD3(Nolock) Where PRD3.FinishedId=S.ITEM_FINISHED_ID),0)=0
			Then Case When isnull((Select Max(Rate) From PurchaseReceiveDetail PRD3(Nolock) JOIN V_FinishedItemDetail VF2 ON PRD3.FinishedId=VF2.ITEM_FINISHED_ID Where VF2.ITEM_ID=V.ITEM_ID and VF2.QualityId=V.QualityId and PRD3.LotNo = s.LotNo),0)=0 
			Then Case When isnull((Select Max(Rate) From PurchaseReceiveDetail PRD4(Nolock) Where PRD4.LotNo=S.LotNo and PRD4.LotNo!='Without Lot No'),0)=0
			Then isnull((Select Max(Rate) From PurchaseReceiveDetail PRD3(Nolock) JOIN V_FinishedItemDetail VF2 ON PRD3.FinishedId=VF2.ITEM_FINISHED_ID Where VF2.ITEM_ID=V.ITEM_ID and VF2.QualityId=V.QualityId),0)
			ELSE isnull((Select Max(Rate) From PurchaseReceiveDetail PRD4(Nolock) Where PRD4.LotNo=S.LotNo and PRD4.LotNo!='Without Lot No'),0) END
			ELSE isnull((Select Max(Rate) From PurchaseReceiveDetail PRD3(Nolock) JOIN V_FinishedItemDetail VF2 ON PRD3.FinishedId=VF2.ITEM_FINISHED_ID Where VF2.ITEM_ID=V.ITEM_ID and VF2.QualityId=V.QualityId and PRD3.LotNo = s.LotNo),0) END
			Else isnull((Select Max(Rate) From PurchaseReceiveDetail PRD3(Nolock) Where PRD3.FinishedId=S.ITEM_FINISHED_ID),0) End 
			Else isnull((Select Max(Rate) From PurchaseReceiveDetail PRD2(Nolock) Where PRD2.FinishedId=S.ITEM_FINISHED_ID and PRD2.LotNo = s.LotNo),0) End
            Else isnull(vp.rate,0)  End
            Else isnull(vp.rate,0) End,0) as Rate,v.UnitType,
            Case When V.MasterCompanyId in(14) Then isnull((select Top (1) isnull(PPRT.Rate,0) as DyeingRate from PP_ProcessRecMaster PPRM JOIN PP_ProcessRecTran PPRT ON PPRM.PRMid=PPRT.PRMID
                Where PPRT.Finishedid=s.ITEM_FINISHED_ID and PPRT.Lotno=S.LotNo and PPRT.TagNo=S.TagNo Order by PPRM.Date desc),0)
				Else isnull((select Top (1) isnull(PPRT.Rate,0) as DyeingRate from PP_ProcessRecMaster PPRM JOIN PP_ProcessRecTran PPRT ON PPRM.PRMid=PPRT.PRMID
                Where PPRT.Finishedid=s.ITEM_FINISHED_ID Order by PPRM.Date desc),0) End as LastDyeingRate        
        from stock s 
        inner join V_FinishedItemDetail v on s.ITEM_FINISHED_ID=v.ITEM_FINISHED_ID  
        inner join GodownMaster g on s.godownid=g.godownid 
        left JOIN V_PURCHASELOTVENDORDETAIL VP ON S.ITEM_FINISHED_ID=VP.FINISHEDID AND S.LOTNO=VP.LOTNO AND S.TAGNO=VP.TAGNO 
        Where s.companyid = " + DDCompany.SelectedValue + "  And V.MasterCompanyId=" + Session["varCompanyId"];

        if (DDGudown.SelectedIndex > 0)
        {
            sQry = sQry + "AND g.godownid =" + DDGudown.SelectedValue;
            filterby = filterby + " Godown-" + DDGudown.SelectedItem.Text;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;

        }
        if (ddItemName.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
            filterby = filterby + ",Item-" + ddItemName.SelectedItem.Text;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.QUALITYID = " + DDQuality.SelectedValue;
            filterby = filterby + ",Quality-" + DDQuality.SelectedItem.Text;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.DESIGNID = " + DDDesign.SelectedValue;
            filterby = filterby + ",Design-" + DDDesign.SelectedItem.Text;

        }
        if (DDColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.COLORID = " + DDColor.SelectedValue;
            filterby = filterby + ",Color-" + DDColor.SelectedItem.Text;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SHADECOLORID = " + DDShadeColor.SelectedValue;
            filterby = filterby + ",ShadeColor-" + DDShadeColor.SelectedItem.Text;

        }
        if (TDundyed_dyed.Visible == true)
        {
            if (RDundyed.Checked == true)
            {
                sQry = sQry + " and v.Shadecolorname like '%Undyed%'";
                filterby = filterby + ",Shade - " + shadecolor;
            }
            else if (RDDyed.Checked == true)
            {
                sQry = sQry + " and v.Shadecolorname not like '%Undyed%'";
                filterby = filterby + ",Shade - " + shadecolor;
            }
        }
        if (DDSize.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.Sizeid = " + DDSize.SelectedValue;

        }
        if (DDLotNo.SelectedIndex > 0)
        {
            sQry = sQry + " AND s.lotno = " + DDLotNo.SelectedValue;
            filterby = filterby + ",LotNo-" + DDLotNo.SelectedItem.Text;
        }
        if (txtLotno.Text != "")
        {
            sQry = sQry + " AND s.lotno = '" + txtLotno.Text + "'";
            filterby = filterby + ",LotNo-" + txtLotno.Text;
        }
        if (txtTagno.Text != "")
        {
            sQry = sQry + " AND s.TagNo = '" + txtTagno.Text + "'";
            filterby = filterby + ",TagNo-" + txtTagno.Text;
        }
        filterby = filterby + ",Date - " + DateTime.Now.ToString("dd-MMM-yyyy");
        sQry = sQry + " group by g.godownname,s.lotno,v.category_name,v.item_name,v.qualityname,designName,ColorName,ShadeColorName,ShapeName,SizeMtr,S.TagNo,v.MasterCompanyId,vp.rate,v.UnitType,s.ITEM_FINISHED_ID,V.ITEM_ID,V.QualityId";

        //DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);       

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand(sQry, con);
        //cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        DataSet DS = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(DS);
        //*************

        con.Close();
        con.Dispose();


        if (DS.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Raw Material Stock");
            //************

            sht.Range("A1:M1").Merge();
            sht.Range("A1").SetValue("STOCK REPORT");
            sht.Range("A1:M1").Style.Font.Bold = true;
            sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //Filter by 
            sht.Row(2).Height = 26.25;
            sht.Range("A2:M2").Merge();
            sht.Range("A2").SetValue(filterby.TrimStart(','));
            sht.Range("A2:M2").Style.Font.Bold = true;
            sht.Range("A2:M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:M2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:M2").Style.Alignment.WrapText = true;
            //Headings
            sht.Range("A3:M3").Style.Font.Bold = true;
            sht.Range("J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3").SetValue("Item Name");
            sht.Range("B3").SetValue("Quality");
            sht.Range("C3").SetValue("Description");
            sht.Range("D3").SetValue("Lot No.");
            sht.Range("E3").SetValue("Tag No.");
            sht.Range("F3").SetValue("Godown");
            sht.Range("G3").SetValue("Stock Qty(Kgs.)");
            sht.Range("H3").SetValue("Unit");
            sht.Range("I3").SetValue("Rate/Pcs");
            sht.Range("J3").SetValue("Item Amount in Value");
            sht.Range("K3").SetValue("Dyeing Rate");
            sht.Range("L3").SetValue("Dyeing Amt");
            sht.Range("M3").SetValue("Total Amt");
            //***************
            int row = 4;
            DataView dv = DS.Tables[0].DefaultView;
            switch (Session["varcompanyNo"].ToString())
            {
                case "14":
                    dv.RowFilter = "Qtyinhand>0";
                    break;
                default:
                    dv.RowFilter = "Qtyinhand<>0";
                    break;
            }
            dv.Sort = "Item_Name,Qualityname,Description,Lotno,TagNo";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            decimal TOTALVALUE = 0, RATE = 0, QTY = 0,TotalDyeingAmt=0, TotalDyeingAndPurchaseAmt=0;
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Item_Name"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Description"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["TagNo"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["godownname"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Qtyinhand"]);
                sht.Range("G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("G" + row).Style.NumberFormat.Format = "#,##0.000";
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["UnitType"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);
                sht.Range("I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("I" + row).Style.NumberFormat.Format = "#,##0.000";
                QTY = Convert.ToDecimal(ds1.Tables[0].Rows[i]["Qtyinhand"]);
                RATE = Convert.ToDecimal(ds1.Tables[0].Rows[i]["Rate"]);
                TOTALVALUE = QTY * RATE;
                sht.Range("J" + row).SetValue(TOTALVALUE);
                sht.Range("J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("J" + row).Style.NumberFormat.Format = "#,##0.000";

                sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["LastDyeingRate"]);
                sht.Range("K" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("K" + row).Style.NumberFormat.Format = "#,##0.000";

                TotalDyeingAmt = Convert.ToDecimal(ds1.Tables[0].Rows[i]["Qtyinhand"]) * Convert.ToDecimal(ds1.Tables[0].Rows[i]["LastDyeingRate"]);
                sht.Range("L" + row).SetValue(TotalDyeingAmt);
                sht.Range("L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("L" + row).Style.NumberFormat.Format = "#,##0.000";
                
                TotalDyeingAndPurchaseAmt = TOTALVALUE + TotalDyeingAmt;
                sht.Range("M" + row).SetValue(TotalDyeingAndPurchaseAmt);
                sht.Range("M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("M" + row).Style.NumberFormat.Format = "#,##0.000";

                row = row + 1;
            }
            ds1.Dispose();
            DS.Dispose();
            //grand Total:
            var sum = sht.Evaluate("SUM(G4:G" + (row - 1) + ")");
            sht.Range("G" + row).Value = sum;
            sht.Range("G" + row).Style.Font.Bold = true;
            sht.Range("G" + row).Style.NumberFormat.Format = "#,##0.000";
            var sumval = sht.Evaluate("SUM(J4:J" + (row - 1) + ")");
            sht.Range("J" + row).Value = sumval;
            sht.Range("J" + row).Style.Font.Bold = true;
            sht.Range("J" + row).Style.NumberFormat.Format = "#,##0.000";

            sht.Range("L" + row).FormulaA1 = "SUM(L4:L" + (row - 1) + ")";
            sht.Range("L" + row).Style.Font.Bold = true;
            sht.Range("L" + row).Style.NumberFormat.Format = "#,##0.000";

            sht.Range("M" + row).FormulaA1 = "SUM(M4:M" + (row - 1) + ")";
            sht.Range("M" + row).Style.Font.Bold = true;
            sht.Range("M" + row).Style.NumberFormat.Format = "#,##0.000";
            //**********

            sht.Columns(1, 15).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("StockReport_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }
    }
    protected void Rawmaterialstock_Excel_BinWise()
    {
        string sQry, VarStr = "", filterby = "Filter By-", Path;
        string shadecolor = "";
        DataSet DS = new DataSet();

        if (RDundyed.Checked == true)
        {
            shadecolor = "Undyed";
        }
        else if (RDDyed.Checked == true)
        {
            shadecolor = "Dyed";
        }
        if (chkwithpurchasedetail.Checked == false)
        {
            sQry = @"SELECT g.godownname,Round(Sum(s.qtyinhand),3) qtyinhand,s.lotno,v.category_name,v.item_name,v.qualityname,
                designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeMtr AS Description,S.TagNo,isnull(s.BinNo,'') as BinNo,
                '' as empname,'' as vendorlotno,'' as Rate
                from stock s 
                inner join V_FinishedItemDetail v on s.ITEM_FINISHED_ID=v.ITEM_FINISHED_ID
                inner join GodownMaster g on s.godownid=g.godownid         
                Where s.companyid = " + DDCompany.SelectedValue + "  And V.MasterCompanyId=" + Session["varCompanyId"];
        }
        else
        {
            sQry = @"SELECT g.godownname,Round(Sum(s.qtyinhand),3) qtyinhand,s.lotno,v.category_name,v.item_name,v.qualityname,
                designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+SizeMtr AS Description,S.TagNo,isnull(s.BinNo,'') as BinNo,
                vp.empname,vp.VENDORLOTNO,
                Case when V.MasterCompanyId=16 Then Case When vp.rate is null Then (Select Max(Rate) 
                            From PurchaseReceiveDetail PRD(Nolock) Where PRD.LotNo = s.LotNo) Else vp.rate End Else vp.rate End Rate
                from stock s 
                inner join V_FinishedItemDetail v on s.ITEM_FINISHED_ID=v.ITEM_FINISHED_ID
                inner join GodownMaster g on s.godownid=g.godownid 
                LEFT JOIN V_PURCHASELOTVENDORDETAIL VP ON S.ITEM_FINISHED_ID=VP.FINISHEDID AND S.LOTNO=VP.LOTNO AND S.TAGNO=VP.TAGNO 
                Where s.companyid = " + DDCompany.SelectedValue + "  And V.MasterCompanyId=" + Session["varCompanyId"];
        }
        if (DDGudown.SelectedIndex > 0)
        {
            sQry = sQry + " AND g.godownid =" + DDGudown.SelectedValue;
            filterby = filterby + " Godown-" + DDGudown.SelectedItem.Text;
            VarStr = VarStr + " And s.GodownID = " + DDGudown.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;
            VarStr = VarStr + " And V.CATEGORY_ID = " + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
            filterby = filterby + ",Item-" + ddItemName.SelectedItem.Text;
            VarStr = VarStr + " And V.ITEM_ID = " + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.QUALITYID = " + DDQuality.SelectedValue;
            filterby = filterby + ",Quality-" + DDQuality.SelectedItem.Text;
            VarStr = VarStr + " And V.QUALITYID = " + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.DESIGNID = " + DDDesign.SelectedValue;
            filterby = filterby + ",Design-" + DDDesign.SelectedItem.Text;
            VarStr = VarStr + " And V.DESIGNID = " + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.COLORID = " + DDColor.SelectedValue;
            filterby = filterby + ",Color-" + DDColor.SelectedItem.Text;
            VarStr = VarStr + " And V.COLORID = " + DDColor.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SHADECOLORID = " + DDShadeColor.SelectedValue;
            filterby = filterby + ",ShadeColor-" + DDShadeColor.SelectedItem.Text;
            VarStr = VarStr + " And V.SHADECOLORID = " + DDShadeColor.SelectedValue;

        }
        if (TDundyed_dyed.Visible == true)
        {
            if (DDShadeColor.SelectedIndex <= 0)
            {
                if (RDundyed.Checked == true)
                {
                    sQry = sQry + " and v.Shadecolorname like '%Undyed%'";
                    filterby = filterby + ",Shade - " + shadecolor;
                    VarStr = VarStr + " And V.Shadecolorname like '%Undyed%'";
                }
                else if (RDDyed.Checked == true)
                {
                    sQry = sQry + " and v.Shadecolorname not like '%Undyed%'";
                    filterby = filterby + ",Shade - " + shadecolor;
                    VarStr = VarStr + " And V.Shadecolorname not like '%Undyed%'";
                }
            }
        }
        if (DDSize.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.Sizeid = " + DDSize.SelectedValue;
            VarStr = VarStr + " And V.Sizeid = " + DDSize.SelectedValue;

        }
        if (DDLotNo.SelectedIndex > 0)
        {
            sQry = sQry + " AND s.lotno = '" + DDLotNo.SelectedValue + "'";
            filterby = filterby + ",LotNo-" + DDLotNo.SelectedItem.Text;
            VarStr = VarStr + " And S.lotno = '" + DDLotNo.SelectedValue + "'";
        }
        if (txtLotno.Text != "")
        {
            if (chkwithpurchasedetail.Checked == true)
            {
                sQry = sQry + " AND (s.lotno = '" + txtLotno.Text + "' or Vp.vendorLotno='" + txtLotno.Text + "')";
                filterby = filterby + ",LotNo-" + txtLotno.Text;
            }
            else
            {
                sQry = sQry + " AND s.lotno = '" + txtLotno.Text + "'";
                filterby = filterby + ",LotNo-" + txtLotno.Text;
            }
        }
        if (txtTagno.Text != "")
        {
            sQry = sQry + " AND s.TagNo = '" + txtTagno.Text + "'";
            filterby = filterby + ",TagNo-" + txtTagno.Text;
            VarStr = VarStr + " And S.TagNo = '" + txtTagno.Text + "'";
        }
        if (chkwithpurchasedetail.Checked == true)
        {
            sQry = sQry + " group by g.godownname,s.lotno,v.category_name,v.item_name,v.qualityname,designName,ColorName,ShadeColorName,ShapeName,SizeMtr,S.TagNo,S.BinNo, vp.empname,vp.VENDORLOTNO,vp.rate, V.MasterCompanyId";

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_GetRawMaterialDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@CompanyID", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@MasterCompanyID", Session["varCompanyId"]);
            cmd.Parameters.AddWithValue("@Where", VarStr);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(DS);

            con.Close();
            con.Dispose();
        }
        else
        {
            sQry = sQry + " group by g.godownname,s.lotno,v.category_name,v.item_name,v.qualityname,designName,ColorName,ShadeColorName,ShapeName,SizeMtr,S.TagNo,S.BinNo";
            DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);
        }

        
        if (DS.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Raw Material Stock");
            //************

            sht.Range("A1:K1").Merge();
            sht.Range("A1").SetValue("STOCK REPORT");
            sht.Range("A1:K1").Style.Font.Bold = true;
            sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //Filter by 
            sht.Row(2).Height = 26.25;
            sht.Range("A2:K2").Merge();
            sht.Range("A2").SetValue(filterby.TrimStart(','));
            sht.Range("A2:K2").Style.Font.Bold = true;
            sht.Range("A2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:K2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:K2").Style.Alignment.WrapText = true;
            //Headings
            sht.Range("A3:K3").Style.Font.Bold = true;
            sht.Range("H3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("K3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3").SetValue("Item Name");
            sht.Range("B3").SetValue("Quality");
            sht.Range("C3").SetValue("Description");
            sht.Range("D3").SetValue("Lot No.");
            sht.Range("E3").SetValue("Tag No.");
            sht.Range("F3").SetValue("Bin No.");
            sht.Range("G3").SetValue("Godown");
            sht.Range("H3").SetValue("Stock Qty(Kgs.)");
            if (chkwithpurchasedetail.Checked == true)
            {
                sht.Range("I3").SetValue("Vendor Name");
                sht.Range("J3").SetValue("Vendor Lot No");
                sht.Range("K3").SetValue("Rate");
            }
            //***************
            int row = 4;
            DataView dv = DS.Tables[0].DefaultView;
            switch (Session["varcompanyNo"].ToString())
            {
                case "14":
                    dv.RowFilter = "Qtyinhand>0";
                    break;
                default:
                    dv.RowFilter = "Qtyinhand<>0";
                    break;
            }
            dv.Sort = "Item_Name,Qualityname,Description,Lotno,TagNo";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());

            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Item_Name"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Description"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["TagNo"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["BinNo"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["godownname"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Qtyinhand"]);
                sht.Range("H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("H" + row).Style.NumberFormat.Format = "#,##0.000";
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Empname"]);
                sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["vendorlotno"]);
                sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);

                row = row + 1;
            }
            ds1.Dispose();
            DS.Dispose();
            //grand Total:
            var sum = sht.Evaluate("SUM(H4:H" + (row - 1) + ")");
            sht.Range("H" + row).Value = sum;
            sht.Range("H" + row).Style.Font.Bold = true;
            sht.Range("H" + row).Style.NumberFormat.Format = "#,##0.000";
            //**********

            sht.Columns(1, 11).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("StockReport_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }
    }
    public void TotalStock()
    {
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Delete TEMP_PROCESS_ISSUE_MASTER");
        DataSet DsNew = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * From Process_Name_Master Where MasterCompanyId=" + Session["varCompanyId"] + "");
        if (DsNew.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < DsNew.Tables[0].Rows.Count; i++)
            {
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Insert into TEMP_PROCESS_ISSUE_MASTER (IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight,PROCESSID) Select IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight," + DsNew.Tables[0].Rows[i]["Process_Name_ID"] + " From PROCESS_ISSUE_MASTER_" + DsNew.Tables[0].Rows[i]["Process_Name_ID"]);
            }
        }
        string str = @"Select DISTINCT '' as Name,V.ITEM_FINISHED_ID, v.category_name as RawMaterialName,v.item_name as SubRawName ,v.qualityname as Quality, v.shadecolorname as Colour,
        s.lotno as LotNo, s.qtyinhand as TotalQuantity,0 RecQty,0 as Status,1 as AssumedPro_ID from stock s,v_finisheditemdetail v Where s.item_finished_id=v.item_finished_id 
        And S.Companyid=" + DDCompany.SelectedValue + " And V.MasterCompanyId=" + Session["varCompanyId"];

        string str1 = @" Select e.empName,V.ITEM_FINISHED_ID, v.item_name as RawMaterialName,v.item_name as SubRawName ,v.qualityname as Quality,v.shadecolorname as Colour,
        v.LotNo,0 balqty,V.RecQty,1 Status ,1 as AssumedPro_ID From V_DyreQtyBalance V,empinfo e, V_FinishedItemDetail V1
        Where V.empid =e.empid AND V.Item_finished_ID=V1.ITEM_FINISHED_ID  And V1.MasterCompanyId=" + Session["varCompanyId"];

        string str2 = @"SELECT e.EmpName as ContractorName,V.ITEM_FINISHED_ID, v.category_name as RawMaterialName,v.item_name as SubRawName ,v.qualityname as Quality,v.shadecolorname as Colour,
        PRawTran.LotNo,Sum(Case When TranType=0 Then PRawTran.IssueQuantity Else 0 End)-Sum(Case When TranType=1 Then PRawTran.IssueQuantity Else 0 End) IssueQuantity,0 RecQty,2 as Status,PRawM.ProcessID 
        From ProcessRawMaster PRawM,ProcessRawTran PRawTran,EmpInfo e,v_finisheditemdetail v 
        Where PRawM.TypeFlag = 0 And PRawM.PRMid=PRawTran.PRMid And PRawM.empid=e.empid And PRawTran.finishedid=v.item_finished_id
        And PRawM.Companyid=" + DDCompany.SelectedValue + " And V.MasterCompanyId=" + Session["varCompanyId"];

        string str3 = @"Select e.EmpName as ContractorName,V.ITEM_FINISHED_ID,v.category_name as RawMaterialName,v.item_name as SubRawName,v.qualityname as Quality,v.shadecolorname as Colour,
        '' LotNo,0 IssueQuantity,Sum(TConsmp+TLoss) TConsmp,2 as Status,PRC.ProcessID  From PROCESS_RECEIVE_CONSUMPTION PRC,V_FinishedItemDetail V,TEMP_PROCESS_ISSUE_MASTER TPM,EmpInfo E 
        Where V.ITEM_FINISHED_ID=PRC.IFinishedid And PRC.IssueOrderId=TPM.IssueOrderId And TPM.EmpId=E.EmpId And TPM.Companyid=" + DDCompany.SelectedValue + " And V.MasterCompanyId=" + Session["varCompanyId"];

        if (DDGudown.SelectedIndex > 0)
        {
            str = str + " And S.GODOWNID =" + DDGudown.SelectedValue;
            str1 = str1 + " And V.GODOWNID =" + DDGudown.SelectedValue;
            str2 = str2 + " And PRawTran.GODOWNID =" + DDGudown.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;
            str1 = str1 + " AND V1.CATEGORY_ID = " + DDCategory.SelectedValue;
            str2 = str2 + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;
            str3 = str3 + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
            str1 = str1 + " AND V1.ITEM_ID = " + ddItemName.SelectedValue;
            str2 = str2 + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
            str3 = str3 + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " AND v.QUALITYID = " + DDQuality.SelectedValue;
            str1 = str1 + " AND V1.QUALITYID = " + DDQuality.SelectedValue;
            str2 = str2 + " AND v.QUALITYID = " + DDQuality.SelectedValue;
            str3 = str3 + " AND v.QUALITYID = " + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " AND v.DESIGNID = " + DDDesign.SelectedValue;
            str1 = str1 + " AND V1.DESIGNID = " + DDDesign.SelectedValue;
            str2 = str2 + " AND v.DESIGNID = " + DDDesign.SelectedValue;
            str3 = str3 + "  AND v.DESIGNID = " + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " AND v.COLORID = " + DDColor.SelectedValue;
            str1 = str1 + " AND V1.COLORID = " + DDColor.SelectedValue;
            str2 = str2 + " AND v.COLORID = " + DDColor.SelectedValue;
            str3 = str3 + "  AND v.COLORID = " + DDColor.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " AND v.SIZEID = " + DDSize.SelectedValue;
            str1 = str1 + " AND V1.SIZEID = " + DDSize.SelectedValue;
            str2 = str2 + " AND v.SIZEID = " + DDSize.SelectedValue;
            str3 = str3 + " AND v.SIZEID = " + DDSize.SelectedValue;
        }
        if (DDLotNo.SelectedIndex > 0)
        {
            str = str + " AND s.LOTNO = '" + DDLotNo.SelectedItem.Text + "'";
            str1 = str1 + " AND V.LotNo = '" + DDLotNo.SelectedItem.Text + "'";
            str2 = str2 + " AND PRawTran.LotNo = '" + DDLotNo.SelectedItem.Text + "'";
            //str3 = str3 + " AND PRawTran.LotNo = '" + DDLotNo.SelectedItem.Text + "'";
        }
        str1 = str1 + @" Group By e.empName,v.item_name,v.item_name,v.qualityname,v.shadecolorname,v.LotNo,V.RecQty,V.ITEM_FINISHED_ID";

        //Group By e.EmpName,v.category_name,v.item_name,v.qualityname,v.shadecolorname,PRawTran.LotNo,V.ITEM_FINISHED_ID

        str2 = str2 + " Group BY e.EmpName,v.category_name,v.item_name,v.qualityname,v.shadecolorname,PRawTran.LotNo,PRawM.ProcessID,V.ITEM_FINISHED_ID";
        str3 = str3 + " Group BY e.EmpName,v.category_name,v.item_name,v.qualityname,v.shadecolorname,PRC.ProcessID,V.ITEM_FINISHED_ID";

        DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str + " UNION " + str1);
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

        SqlDataAdapter sda = new SqlDataAdapter(str2 + " UNION " + str3, ErpGlobal.DBCONNECTIONSTRING);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        DS.Tables.Add(dt);
        if (DS.Tables[0].Rows.Count > 0 || DS.Tables[1].Rows.Count > 0)
        {
            Session["rptFileName"] = "reports/Rpt_CompanyRawMaterialStock.rpt";
            Session["GetDataset"] = DS;
            Session["dsFileName"] = "~\\ReportSchema\\RawMaterialStocksDetails.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    public void OneDayIssueReport()
    {
        String sQry = "", str = "", str1 = "", str2 = "", str3 = "";
        DataSet DS = new DataSet();


        if (DDCategory.SelectedIndex > 0 && DDCategory.SelectedItem.Text != "ALL")
        {
            sQry = sQry + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;

        }
        if (ddItemName.SelectedIndex > 0 && ddItemName.SelectedItem.Text != "ALL")
        {
            sQry = sQry + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0 && DDQuality.SelectedItem.Text != "ALL")
        {
            sQry = sQry + " AND v.QUALITYID = " + DDQuality.SelectedValue;

        }
        if (DDDesign.SelectedIndex > 0 && DDDesign.SelectedItem.Text != "ALL")
        {
            sQry = sQry + " AND v.DESIGNID = " + DDDesign.SelectedValue;

        }
        if (DDColor.SelectedIndex > 0 && DDColor.SelectedItem.Text != "ALL")
        {
            sQry = sQry + " AND v.COLORID = " + DDColor.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && DDShadeColor.SelectedItem.Text != "ALL")
        {
            sQry = sQry + " AND v.SHADECOLORID = " + DDShadeColor.SelectedValue;

        }
        if (DDShape.SelectedIndex > 0 && DDShape.SelectedItem.Text != "ALL")
        {
            sQry = sQry + " AND v.SHAPEID = " + DDShape.SelectedValue;

        }
        if (DDSize.SelectedIndex > 0 && DDSize.SelectedItem.Text != "ALL")
        {
            sQry = sQry + " AND v.SHAPEID = " + DDSize.SelectedValue;

        }
        str = "select PIS.date,PISTran.quantity as issueqty, 0 as recQty,PISTran.LotNo,PIS.ChallanNo as GatePassNo, '' as GateInNo, v.Item_Name,v.colorName,v.QualityName," +
                " 'Issue to purchase' as IssueToDetail, '' as RecFromDetail from PurchaseIndentIssue PIS " +
                " left join PurchaseindentissueTran PISTran on PIS.PindentIssueid = PISTran.PindentIssueid " +
                " left join v_finisheditemDetail v on PISTran.finishedid = v.item_Finished_ID " +
                 "where PIS.MasterCompanyId=" + Session["varCompanyId"] + " And  replace(convert(varchar(11),PIS.Date,106), ' ','-') = '" + TxtDate.Text.Trim() + "'" + sQry;

        str1 = "select PRM.Receivedate,0 as IssueQty,PRT.qty as Recqty,PRT.LotNo,'' as GatePassNo, PRM.BillNo as GateInNo , v.Item_Name,v.colorName,v.QualityName, " +
                " '' as IssueToDetail, 'Receive from purchase' as RecFromDetail from PurchaseReceiveMaster PRM " +
                " left join PurchasereceiveDetail PRT on PRM.PurchaseReceiveid = PRT.PurchaseReceiveid " +
                " left join v_finisheditemDetail v on PRT.finishedid = v.item_Finished_ID " +
                " where PRM.MasterCompanyId=" + Session["varCompanyId"] + " And  replace(convert(varchar(11),PRM.Receivedate,106), ' ','-') = '" + TxtDate.Text.Trim() + "'" + sQry;
        str2 = "select PRawM.date,PRawT.Issuequantity, 0 as RecQty ,PRawT.LotNo,PRawM.ChallanNo as GatePassNo, '' as GateInNo, v.Item_Name,v.colorName,v.QualityName, " +
                " 'Issue to Dyer' as IssueToDetail, '' as RecFromDetail from pp_processrawmaster PRawM " +
                " Inner join pp_processrawtran PRawT on PRawM.Prmid = PRawT.Prmid " +
                " inner join v_finisheditemDetail v on PRawT.finishedid = v.item_Finished_ID " +
                " where PRawM.MasterCompanyId=" + Session["varCompanyId"] + " And  replace(convert(varchar(11),PRawM.Date,106), ' ','-') = '" + TxtDate.Text.Trim() + "'" + sQry;

        str3 = "select PRecM.date,0 as IssueQty,PRecT.Recquantity ,PRecT.LotNo,'' as GatePassNo, PRecM.ChallanNo as GateInNo, v.Item_Name,v.colorName,v.QualityName, " +
                " '' as IssueToDetail, 'Receive from Dyer' as RecFromDetail from pp_processrecmaster PRecM " +
                " Inner join pp_processrectran PRecT on PRecM.Prmid = PRecT.Prmid " +
                " inner join v_finisheditemDetail v on PRecT.finishedid = v.item_Finished_ID  " +
                " where V.MasterCompanyId=" + Session["varCompanyid"] + " And  replace(convert(varchar(11),PRecM.Date,106), ' ','-') = '" + TxtDate.Text.Trim() + "'" + sQry;

        sQry = str + " UNION " + str1 + " UNION " + str2 + " UNION " + str3;
        Session["ReportPath"] = "reports/RptDayIssueTransaction.rpt";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            DS = SqlHelper.ExecuteDataset(Tran, CommandType.Text, sQry.Trim());
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = DS;
            Session["dsFileName"] = "~\\ReportSchema\\DayIssueTransaction.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
            // UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmProcessDetailIssueReceive.aspx");
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DDcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = @"select orderid,customerorderNo+' #' +localorder as Localorder from ordermaster where customerid= " + DDcustomer.SelectedValue + " order by localorder";
        if (Session["varcompanyId"].ToString() == "16" || Session["varcompanyId"].ToString() == "28")
        {
            Str = @"select orderid,customerorderNo+' #' +localorder as Localorder from ordermaster where Status = 0 And customerid= " + DDcustomer.SelectedValue + " order by localorder";
        }
        UtilityModule.ConditionalComboFill(ref DDOrder, Str, true, "ALL");
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDCategory.SelectedItem.Text.Trim() != "ALL")
        {
            UtilityModule.ConditionalComboFill(ref ddItemName, "Select ITEM_ID,ITEM_NAME from ITEM_MASTER " + "WHERE CATEGORY_ID = " + DDCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyid"] + " order by ITEM_NAME", true, "ALL");
        }
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        TRDDQuality.Visible = true;
        if (ddItemName.SelectedItem.Text.Trim() != "ALL")
        {
            UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality Where Item_Id=" + ddItemName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyid"] + " order by QualityName", true, "ALL");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDQuality, "SELECT QualityId,QualityName from Quality Where MasterCompanyId=" + Session["varCompanyid"] + "  order by QualityName", true, "ALL");
        }
        DDQuality_SelectedIndexChanged(sender, e);
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {

        TRDDDesign.Visible = false;
        TRDDColor.Visible = false;
        TRDDShadeColor.Visible = false;
        TRDDShape.Visible = false;

        if (RDOneDayReport.Checked == true)
        {
            TRDate.Visible = true;
        }
        else
        {

            TRDate.Visible = false;

        }
        string qry = @"select DESIGNID,DESIGNNAME from DESIGN Where MasterCompanyId=" + Session["varCompanyid"] + @" ORDER BY DESIGNNAME
        select COLORID,COLORNAME from color Where MasterCompanyId=" + Session["varCompanyid"] + @" ORDER BY COLORNAME
        SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR Where MasterCompanyId=" + Session["varCompanyid"] + @" ORDER BY SHADECOLORNAME
        SELECT SHAPEID, SHAPENAME FROM SHAPE Where MasterCompanyId=" + Session["varCompanyid"] + " ORDER BY SHAPENAME";
        DataSet ds = SqlHelper.ExecuteDataset(qry);

        string str = @"select PARAMETER_ID from ITEM_CATEGORY_PARAMETERS where category_id=" + DDCategory.SelectedValue;
        DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds1.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                switch (dr["Parameter_Id"].ToString())
                {
                    case "2":
                        TRDDDesign.Visible = true;
                        UtilityModule.ConditionalComboFillWithDS(ref DDDesign, ds, 0, true, "ALL");
                        break;
                    case "3":
                        TRDDColor.Visible = true;
                        UtilityModule.ConditionalComboFillWithDS(ref DDColor, ds, 1, true, "ALL");
                        break;
                    case "4":
                        TRDDShape.Visible = true;
                        UtilityModule.ConditionalComboFillWithDS(ref DDShape, ds, 3, true, "ALL");
                        break;
                    case "5":
                        TRDDSize.Visible = true;
                        break;
                    case "6":
                        TRDDShadeColor.Visible = true;
                        UtilityModule.ConditionalComboFillWithDS(ref DDShadeColor, ds, 2, true, "ALL");
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
    public void RawMaterialOpening()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] array = new SqlParameter[3];
            array[0] = new SqlParameter("@Fromdate", SqlDbType.DateTime);
            array[1] = new SqlParameter("@Todate", SqlDbType.DateTime);
            array[2] = new SqlParameter("@Where", SqlDbType.VarChar, 8000);
            string sQry = " S.CompanyId=" + DDCompany.SelectedValue + " And V.mastercompanyId=" + Session["varcompanyId"];

            if (DDCategory.SelectedIndex > 0 && DDCategory.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0 && ddItemName.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0 && DDQuality.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND v.QUALITYID = " + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0 && DDDesign.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND v.DESIGNID = " + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0 && DDColor.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND v.COLORID = " + DDColor.SelectedValue;
            }
            if (DDShadeColor.SelectedIndex > 0 && DDShadeColor.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND v.SHADECOLORID = " + DDShadeColor.SelectedValue;
            }
            if (DDShape.SelectedIndex > 0 && DDShape.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND v.SHAPEID = " + DDShape.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0 && DDSize.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND v.sizeid = " + DDSize.SelectedValue;
            }
            if (DDGudown.SelectedIndex > 0)
            {
                sQry = sQry + " AND S.godownid = " + DDGudown.SelectedValue;
            }
            if (txtLotno.Text != "")
            {
                sQry = sQry + " AND S.LotNo = '" + txtLotno.Text + "'";
            }
            if (txtTagno.Text != "")
            {
                sQry = sQry + " AND S.TagNo = '" + txtTagno.Text + "'";
            }
            if (TDundyed_dyed.Visible == true)
            {
                if (RDundyed.Checked == true)
                {
                    sQry = sQry + " and v.Shadecolorname like '%Undyed%'";
                }
                else if (RDDyed.Checked == true)
                {
                    sQry = sQry + " and v.Shadecolorname not like '%Undyed%'";
                }
            }
            array[0].Value = txtfromdate.Text;
            array[1].Value = txttodate.Text;
            array[2].Value = sQry;

            DS = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "pro_rawmaterialopeningclosing", array);
            if (DS.Tables[0].Rows.Count > 0)
            {
                if (ChkExportExcel.Checked == true)
                {
                    RawMaterialOpeningClosingReportForExcel(DS);
                }
                else
                {
                    Session["rptFileName"] = "reports/StockReportopening.rpt";
                    Session["GetDataset"] = DS;
                    Session["dsFileName"] = "~\\ReportSchema\\StockReportopening.xsd";
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx?Export=Y', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
            // UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmProcessDetailIssueReceive.aspx");
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    public void RawMaterialOpeningClosingReportForExcel(DataSet DS)
    {
        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("RawMaterialOpeningClosingReport");

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
        sht.Range("A1").SetValue(DS.Tables[0].Rows[0]["CompanyName"]);

        sht.Row(1).Height = 24;
        sht.Range("A2:L2").Merge();
        sht.Range("A2:L2").Style.Font.FontSize = 10;
        sht.Range("A2:L2").Style.Font.Bold = true;
        sht.Range("A2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A2:L2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("A2:L2").Style.Alignment.WrapText = true;
        sht.Range("A2").SetValue(DS.Tables[0].Rows[0]["Compaddr1"]);

        sht.Row(1).Height = 24;
        sht.Range("A3:L3").Merge();
        sht.Range("A3:L3").Style.Font.FontSize = 10;
        sht.Range("A3:L3").Style.Font.Bold = true;
        sht.Range("A3:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A3:L3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("A3:L3").Style.Alignment.WrapText = true;
        sht.Range("A3").SetValue(DS.Tables[0].Rows[0]["gstno"]);

        sht.Row(1).Height = 24;
        sht.Range("A4:L4").Merge();
        sht.Range("A4:L4").Style.Font.FontSize = 10;
        sht.Range("A4:L4").Style.Font.Bold = true;
        sht.Range("A4:L4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A4:L4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("A4:L4").Style.Alignment.WrapText = true;
        sht.Range("A4").SetValue("STOCK REPORT FROM : " + txtfromdate.Text + " To : " + txttodate.Text + "");
        
        sht.Range("A5:L5").Style.Font.FontSize = 10;
        sht.Range("A5:L5").Style.Font.Bold = true;

        sht.Range("A5").Value = "Category Name";
        sht.Range("B5").Value = "Item Name";
        sht.Range("C5").Value = "Quality";
        sht.Range("D5").Value = "Design";
        sht.Range("E5").Value = "Color";
        sht.Range("F5").Value = "Size";
        sht.Range("G5").Value = "Shade Color";
        sht.Range("H5").Value = "Unit";
        sht.Range("I5").Value = "Opening Stock";
        sht.Range("J5").Value = "Inw.Qty";
        sht.Range("K5").Value = "Out.Qty";
        sht.Range("L5").Value = "Clos.Bal";

        int row = 6;
        
        for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
        {
            sht.Range("A" + row).SetValue(DS.Tables[0].Rows[i]["CATEGORY_NAME"]);
            sht.Range("B" + row).SetValue(DS.Tables[0].Rows[i]["Item_Name"]);
            sht.Range("C" + row).SetValue(DS.Tables[0].Rows[i]["QualityName"]);
            sht.Range("D" + row).SetValue(DS.Tables[0].Rows[i]["designName"]);
            sht.Range("E" + row).SetValue(DS.Tables[0].Rows[i]["ColorName"]);
            sht.Range("F" + row).SetValue(DS.Tables[0].Rows[i]["sizemtr"]);
            sht.Range("G" + row).SetValue(DS.Tables[0].Rows[i]["ShadeColorName"]);
            sht.Range("H" + row).SetValue(DS.Tables[0].Rows[i]["unitName"]);
            sht.Range("I" + row).SetValue(DS.Tables[0].Rows[i]["Opening"]);
            sht.Range("J" + row).SetValue(DS.Tables[0].Rows[i]["InwardQty"]);
            sht.Range("K" + row).SetValue(DS.Tables[0].Rows[i]["OutwardQty"]);
            decimal ClsBal = Convert.ToDecimal(DS.Tables[0].Rows[i]["Opening"]) + Convert.ToDecimal(DS.Tables[0].Rows[i]["InwardQty"]) - Convert.ToDecimal(DS.Tables[0].Rows[i]["OutwardQty"]);
            sht.Range("L" + row).SetValue(ClsBal);

            row = row + 1;
        }

        sht.Range("G" + row).Value = "Total";
        sht.Range("I" + row).FormulaA1 = "=SUM(I6" + ":$I$" + (row - 1) + ")";
        sht.Range("J" + row).FormulaA1 = "=SUM(J6" + ":$J$" + (row - 1) + ")";
        sht.Range("K" + row).FormulaA1 = "=SUM(K6" + ":$K$" + (row - 1) + ")";
        sht.Range("L" + row).FormulaA1 = "=SUM(L6" + ":$L$" + (row - 1) + ")";
        sht.Range("G" + row + ":L" + row).Style.Font.SetBold();

        DS.Dispose();
        using (var a = sht.Range("A1" + ":L" + row))
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
        string filename = UtilityModule.validateFilename("RawMaterialOpeningClosingReport_" + DateTime.Now + "." + Fileextension);
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
    public void RawMaterialStockDetail()
    {
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}
        //SqlTransaction Tran = con.BeginTransaction();
        try
        {

            string sQry = " S.CompanyId=" + DDCompany.SelectedValue + " And Vf.mastercompanyId=" + Session["varcompanyId"] + "";

            if (DDCategory.SelectedIndex > 0 && DDCategory.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND vf.CATEGORY_ID = " + DDCategory.SelectedValue;

            }
            if (ddItemName.SelectedIndex > 0 && ddItemName.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND vf.ITEM_ID = " + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0 && DDQuality.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND vf.QUALITYID = " + DDQuality.SelectedValue;

            }
            if (DDDesign.SelectedIndex > 0 && DDDesign.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND vf.DESIGNID = " + DDDesign.SelectedValue;

            }
            if (DDColor.SelectedIndex > 0 && DDColor.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND vf.COLORID = " + DDColor.SelectedValue;
            }
            if (DDShadeColor.SelectedIndex > 0 && DDShadeColor.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND vf.SHADECOLORID = " + DDShadeColor.SelectedValue;

            }
            if (DDShape.SelectedIndex > 0 && DDShape.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND vf.SHAPEID = " + DDShape.SelectedValue;

            }
            if (DDSize.SelectedIndex > 0 && DDSize.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND vf.sizeid = " + DDSize.SelectedValue;

            }
            if (DDGudown.SelectedIndex > 0)
            {
                sQry = sQry + " AND S.godownid = " + DDGudown.SelectedValue;
            }
            if (txtLotno.Text != "")
            {
                sQry = sQry + " AND S.LotNo ='" + txtLotno.Text + "'";
            }
            if (txtTagno.Text != "")
            {
                sQry = sQry + " AND S.TagNo ='" + txtTagno.Text + "'";
            }
            if (TDundyed_dyed.Visible == true)
            {
                if (RDundyed.Checked == true)
                {
                    sQry = sQry + " and vf.Shadecolorname like '%Undyed%'";

                }
                else if (RDDyed.Checked == true)
                {
                    sQry = sQry + " and vf.Shadecolorname not like '%Undyed%'";

                }
            }

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_GetStockTranDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Fromdate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
            cmd.Parameters.AddWithValue("@Where", sQry);
            cmd.Parameters.AddWithValue("@CompanyName", DDCompany.SelectedItem.Text);

            // DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(DS);
            //*************

            con.Close();
            con.Dispose();
            //***********
            if (DS.Tables[0].Rows.Count > 0)
            {
                //RptStockTranDetailLotWiseTagWise
                if (chkLotwiseTagwise.Checked == true)
                {
                    if (Session["varcompanyId"].ToString() == "21")
                    {
                        StockTranDetailWithLotTag_Excel(DS);
                        return;
                    }
                    else
                    {
                        Session["rptFileName"] = "reports/RptStockTranDetailLotWiseTagWise.rpt";
                    }

                }
                else
                {
                    Session["rptFileName"] = "reports/RptStockTranDetail.rpt";
                }
                Session["GetDataset"] = DS;
                Session["dsFileName"] = "~\\ReportSchema\\RptStockTranDetail.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }




            //SqlParameter[] array = new SqlParameter[4];
            //array[0] = new SqlParameter("@Fromdate", txtfromdate.Text);
            //array[1] = new SqlParameter("@Todate", txttodate.Text);
            //array[2] = new SqlParameter("@Where", sQry);
            //array[3] = new SqlParameter("@CompanyName", DDCompany.SelectedItem.Text);

            //DS = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetStockTranDetail", array);

            //// DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);
            //if (DS.Tables[0].Rows.Count > 0)
            //{
            //    //RptStockTranDetailLotWiseTagWise
            //    if (chkLotwiseTagwise.Checked == true)
            //    {
            //        if (Session["varcompanyId"].ToString() == "21")
            //        {
            //            StockTranDetailWithLotTag_Excel(DS);
            //            return;
            //        }
            //        else
            //        {
            //            Session["rptFileName"] = "reports/RptStockTranDetailLotWiseTagWise.rpt";
            //        }

            //    }
            //    else
            //    {
            //        Session["rptFileName"] = "reports/RptStockTranDetail.rpt";
            //    }
            //    Session["GetDataset"] = DS;
            //    Session["dsFileName"] = "~\\ReportSchema\\RptStockTranDetail.xsd";
            //    StringBuilder stb = new StringBuilder();
            //    stb.Append("<script>");
            //    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            //    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            //}
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
            // UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmProcessDetailIssueReceive.aspx");
            // Tran.Rollback();
        }

    }

    private void StockTranDetailWithLotTag_Excel(DataSet DS)
    {
        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("StockTranDetail");

        //*************
        //***********
        sht.Row(1).Height = 24;
        sht.Range("A1:I1").Merge();
        sht.Range("A1:I1").Style.Font.FontSize = 10;
        sht.Range("A1:I1").Style.Font.Bold = true;
        sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A1:I1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("A1:I1").Style.Alignment.WrapText = true;
        //************
        sht.Range("A1").SetValue("Stock Transaction From : " + txtfromdate.Text + " To : " + txttodate.Text + "");

        sht.Range("A2:I2").Style.Font.FontSize = 10;
        sht.Range("A2:I2").Style.Font.Bold = true;

        sht.Range("A2").Value = "Tran Date";
        sht.Range("B2").Value = "Item Name";
        sht.Range("C2").Value = "Color Name";
        sht.Range("D2").Value = "Lot No";
        sht.Range("E2").Value = "Tag No";
        sht.Range("F2").Value = "Qty";
        sht.Range("G2").Value = "Godown Name";
        sht.Range("H2").Value = "TranDetail";
        sht.Range("I2").Value = "Type";

        int row = 3;

        DataView dv = DS.Tables[0].DefaultView;
        dv.Sort = "Item_Finished_id,LotNo,TagNo,godownName,TranDate";
        DataSet ds1 = new DataSet();
        ds1.Tables.Add(dv.ToTable());

        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["TranDate"]);
            sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Item_Name"] + " " + ds1.Tables[0].Rows[i]["QualityName"]);
            sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Shadecolorname"] + " " + ds1.Tables[0].Rows[i]["colorname"]);
            sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
            sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Tagno"]);
            sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Quantity"]);
            sht.Range("F" + row).Style.NumberFormat.Format = "@";
            sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["GodownName"]);
            sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Transactiondetail"]);
            sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["TranType"].ToString() == "1" ? "Receive" : "Issue");

            row = row + 1;
        }
        DS.Dispose();
        ds1.Dispose();
        //*************************************************
        using (var a = sht.Range("A1" + ":I" + row))
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
        string filename = UtilityModule.validateFilename("StockTranDetail_" + DateTime.Now + "." + Fileextension);
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
    protected void Monthlystockflow()
    {

        //        string str = @"Select vf.QualityName,Round(Sum(case When ST.trantype=1 Then St.quantity Else 0 End),3) as Recqty,
        //                        Round(Sum(case When ST.trantype=0 Then St.quantity Else 0 End),3) as Issqty,'" + DDCompany.SelectedItem.Text + "' as Companyname,'" + txtfromdate.Text + "' as FromDate,'" + txttodate.Text + @"' as Todate
        //                        From stock S inner join Stocktran ST on S.StockID=ST.Stockid
        //                        inner join V_FinishedItemDetail vf on S.ITEM_FINISHED_ID=vf.ITEM_FINISHED_ID Where S.companyid=" + DDCompany.SelectedValue + " and ST.tranDate>='" + txtfromdate.Text + "' and ST.TranDate<='" + txttodate.Text + @"'
        //                        group by vf.QualityName ";
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@companyid", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@Companyname", DDCompany.SelectedItem.Text);
        param[2] = new SqlParameter("@fromdate", txtfromdate.Text);
        param[3] = new SqlParameter("@Todate", txttodate.Text);
        //*******
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_getmonthlystockflow", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "reports/RptMonthlystockflow.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptMonthlystockflow.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opmonth", "alert('No Record Found!');", true);
        }

    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (RDRawMaterialStock.Checked == true)
        {
            RawMaterialStock();
        }
        else if (RDTotalStock.Checked == true)
        {
            TotalStock();
        }
        else if (RDOneDayReport.Checked == true)
        {
            OneDayIssueReport();
        }
        else if (RDFinishedStock.Checked == true)
        {
            if (Session["VarCompanyNo"].ToString() == "38")
            {
                if (ChkExportExcel.Checked == true)
                {
                    FinishedStockWithAreaExcelReport();
                }
                else if (ChkWithBuyerOrderNo.Checked == true)
                {
                    FinishedStockWithBuyerOrderNoAreaExcelReport();
                }
                else
                {
                    FinishedStock();
                }
            }
            else if (ChkWithoutStockNo.Checked == true)
            {
                FinishedStockWithoutStockNo();
            }
            else
            {
                FinishedStock();
            }
            
        }
        else if (RDrawmaterialopeningreport.Checked == true)
        {
            if (chkStkDetail.Checked == true)
            {
                RawMaterialStockDetail();
            }
            else
            {
                if (Session["VarCompanyNo"].ToString() == "30")
                {
                    RawMaterialOpeningWithLotNo();
                }
                else
                {
                    RawMaterialOpening();
                }                
            }
        }
        else if (RDMonthlystock.Checked == true)
        {
            Monthlystockflow();
        }
        else if (RDpurchaseissue_receivedetail.Checked == true)
        {
            PurchaseIssue_ReceiveDetail();
        }
        else if (RDPurchaseMaterialasondate.Checked == true)
        {
            if (ChkExportExcel.Checked == true)
            {
                StockInHandAsOnDate_Excel();
            }
            else
            {
                PurchaseMaterialasondate();
            }
        }
        else if (RDfinishedstockasondate.Checked == true)
        {
            FinishedstockasonDate();
        }
        else if (RdShadewisestock.Checked == true)
        {
            if (Session["varCompanyNo"].ToString() == "22")
            {
                if (ChkForIssueRegister.Checked == true)
                {
                    IssueRegisterReportForDiamondExport();
                }
                else
                {
                    shadewisestockDiamondExport();
                }
            }
            else
            {
                shadewisestock();
            }
        }
        else if (RDStockTransferDetail.Checked == true)
        {
            StockTransferDetail();
        }
        else if (RDRawMaterialStockGodownWise.Checked == true)
        {
            if (ChkForLedgerTransaction.Checked == true)
            {
                RawMaterialStockTransactionDetails();
            }
            else
            {
                RawMaterialStockGodownWise();
            }
        }
        else if (RDRawMaterialStockAsOnDate.Checked == true)
        {
            if (chkwithpurchasedetail.Checked == true)
            {
                RawMaterialStockAsOnDateWiseWithPurchaseRate();
            }
            else
            {
                if (ChkExportExcel.Checked == true)
                {
                    RawMaterialStockAsOnDateWiseExcelReport();
                }
                else
                {
                    RawMaterialStockAsOnDateWise();
                }
            }
            
        }
        else if (RDRawMaterialOrderAssignIssueStock.Checked == true)
        {
            RawMaterialOrderAssignIssueStock();
        }
        else if (RDFinishedStockProcessWise.Checked== true)
        {
            FinishedStockProcessWiseWithBuyerOrderNoAreaExcelReport();
        }
    }
    protected void RawMaterialOrderAssignIssueStock()
    {
        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_GetOrderStockAssignWithIssueDetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        string CustomerID = "0", OrderID = "0", CategoryID = "0", ItemID = "0", QualityID = "0", DesignID = "0", ColorID = "0", ShapeID  = "0", SizeID = "0", 
            ShadeColorID = "0", LotNo = "";
        if (TRDDCustName.Visible == true && DDcustomer.SelectedIndex > 0)
        {
            CustomerID = DDcustomer.SelectedValue;
        }
        if (TRDDOrder.Visible == true && DDOrder.SelectedIndex > 0)
        {
            OrderID = DDOrder.SelectedValue;
        }
        if (TRcategory.Visible == true && DDCategory.SelectedIndex > 0)
        {
            CategoryID = DDCategory.SelectedValue;
        }
        if (TRddItemName.Visible == true && ddItemName.SelectedIndex > 0)
        {
            ItemID = ddItemName.SelectedValue;
        }
        if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
        {
            QualityID = DDQuality.SelectedValue;
        }
        if (TRDDDesign.Visible == true && DDDesign.SelectedIndex > 0)
        {
            DesignID = DDDesign.SelectedValue;
        }
        if (TRDDColor.Visible == true && DDColor.SelectedIndex > 0)
        {
            ColorID = DDColor.SelectedValue;
        }
        if (TRDDShape.Visible == true && DDShape.SelectedIndex > 0)
        {
            ShapeID = DDShape.SelectedValue;
        }
        if (TRDDSize.Visible == true && DDSize.SelectedIndex > 0)
        {
            SizeID = DDSize.SelectedValue;
        }
        if (TRDDShadeColor.Visible == true && DDShadeColor.SelectedIndex > 0)
        {
            ShadeColorID = DDShadeColor.SelectedValue;
        }
        if (TRDDLotNo.Visible == true && DDLotNo.SelectedIndex > 0)
        {
            LotNo = DDLotNo.SelectedItem.Text;
        }

        cmd.Parameters.AddWithValue("@CompanyID", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
        cmd.Parameters.AddWithValue("@OrderID", OrderID);
        cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
        cmd.Parameters.AddWithValue("@ItemID", ItemID);
        cmd.Parameters.AddWithValue("@QualityID", QualityID);
        cmd.Parameters.AddWithValue("@DesignID", DesignID);
        cmd.Parameters.AddWithValue("@ColorID", ColorID);
        cmd.Parameters.AddWithValue("@ShapeID", ShapeID);
        cmd.Parameters.AddWithValue("@SizeID", SizeID);
        cmd.Parameters.AddWithValue("@ShadeColorID", ShadeColorID);
        cmd.Parameters.AddWithValue("@LotNo", LotNo);
        cmd.Parameters.AddWithValue("@TagNo", "");
                
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();
        //***********     
        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Stock Assign Issue Detail");
            //************

            sht.Range("A1:I1").Merge();
            sht.Range("A1").SetValue("Stock Assign Issue Detail");
            sht.Range("A1:I1").Style.Font.Bold = true;
            sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            
            //Headings
            sht.Range("A2:I2").Style.Font.Bold = true;
            sht.Range("E2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            sht.Range("A2").SetValue("Item Name");
            sht.Range("B2").SetValue("Quality");
            sht.Range("C2").SetValue("Shade Color");
            sht.Range("D2").SetValue("Lot No");
            sht.Range("E2").SetValue("PurchaseDirectStock");
            sht.Range("F2").SetValue("OrderAssignQty");
            sht.Range("G2").SetValue("IndentIssQty");
            sht.Range("H2").SetValue("ProcessIssQty");
            sht.Range("I2").SetValue("StockQty");

            using (var a = sht.Range("A2:I2"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            //***************
            int row = 3;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                using (var a = sht.Range("A" + row + ":I" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"].ToString());
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"].ToString());
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"].ToString());
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["LotNo"].ToString());
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["PurchaseDirectStock"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["OrderAssignQty"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["IndentIssQty"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ProcessIssQty"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["StockQty"]);

                row = row + 1;
            }

            sht.Columns(1, 12).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("StockAssignIssueDetail_" + DateTime.Now + "." + Fileextension);
            string Path = "";
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "altshade", "alert('No records found for this combination.')", true);
        }
    }

    protected void StockTransferDetail()
    {
        string str = @" Select CIF.CompanyName FCompanyName, CIT.CompanyName TCompanyName, REPLACE(CONVERT(VARCHAR(11), a.TransferDate, 106), ' ', '-') TransferDate, a.ChallanNo, 
            Replace(VF.ITEM_NAME + ', ' + VF.QualityName + ', ' + VF.designName + ', ' + VF.ColorName + ', ' + VF.ShapeName + ', ' + VF.SizeMtr + ', ' + VF.ShadeColorName, ', , ', '') Descriptions, 
            B.Qty, U.UnitName, GMF.GodownName FGodownName, 
            GMT.GodownName TGodownName, b.FLotNo, b.TLotNo, b.FTagNo, b.TTagNo, b.FBINNO, b.TBINNO,
            isnull(NUD.UserName,'') as UserName,REPLACE(CONVERT(VARCHAR(11), b.DATEADDED, 106), ' ', '-') as AddedDate,convert(char(5), b.DATEADDED, 108) as Addedtime, 
            VF.ITEM_NAME, VF.QualityName, VF.designName, VF.ColorName, VF.ShapeName, VF.SizeMtr, VF.ShadeColorName,isnull(b.Remarks,'') as Remarks 
            FROM Stock_TransferMaster a(Nolock) 
            JOIN CompanyInfo CIF(Nolock) ON CIF.CompanyId = a.FCompanyId 
            JOIN CompanyInfo CIT(Nolock) ON CIT.CompanyId = a.TCompanyId  
            JOIN Stock_TransferDetail b(Nolock) ON b.TransferId = a.TransferId 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.Item_Finishedid 
            JOIN GodownMaster GMF(Nolock) ON GMF.GoDownID = b.FGodownId 
            JOIN GodownMaster GMT(Nolock) ON GMT.GoDownID = b.TGodownId 
            JOIN Unit U(Nolock) ON U.UnitId = b.unitid 
            JOIN NewUserDetail NUD(NoLock) ON B.UserId=NUD.UserID
            Where a.TransferDate >= '" + txtfromdate.Text + "' AND a.TransferDate <= '" + txttodate.Text + @"'
            Order By a.TransferDate, a.TransferId ";
        //*********
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //ds.Tables[0].Columns.Remove("ID");
            //ds.Tables[0].Columns.Remove("DetailID");

            if (Session["VarCompanyNo"].ToString() != "21")
            {
                ds.Tables[0].Columns.Remove("UserName");
                ds.Tables[0].Columns.Remove("AddedDate");
                ds.Tables[0].Columns.Remove("Addedtime");
            }

            //Export to excel
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;

            GridView1.DataSource = ds;
            GridView1.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
             "attachment;filename=StockTransferDetail" + DateTime.Now + ".xls");
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
            //*************
        }
    }

    protected void FinishedstockasonDate()
    {
        lblMessage.Text = "";
        try
        {
            string Filterby = "", Where = "";
            Filterby = "Company - " + DDCompany.SelectedItem.Text;
            if (DDcustomer.SelectedIndex > 0)
            {
                Where = Where + " and OM.customerid=" + DDcustomer.SelectedValue;
                Filterby = Filterby + " , Customer - " + DDcustomer.SelectedItem.Text;
            }
            if (DDOrder.SelectedIndex > 0)
            {
                Where = Where + " and OM.orderid=" + DDOrder.SelectedValue;
                Filterby = Filterby + " , Order No. - " + DDOrder.SelectedItem.Text;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Where = Where + " and vf.Item_id=" + ddItemName.SelectedValue;
                Filterby = Filterby + " , ItemName - " + ddItemName.SelectedItem.Text;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                Where = Where + " and vf.QualityId=" + DDQuality.SelectedValue;
                Filterby = Filterby + " , Quality - " + DDQuality.SelectedItem.Text;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                Where = Where + " and vf.DesignId=" + DDDesign.SelectedValue;
                Filterby = Filterby + " , Design - " + DDDesign.SelectedItem.Text;
            }
            if (DDColor.SelectedIndex > 0)
            {
                Where = Where + " and vf.Colorid=" + DDColor.SelectedValue;
                Filterby = Filterby + " , Color - " + DDColor.SelectedItem.Text;
            }
            if (DDShape.SelectedIndex > 0)
            {
                Where = Where + " and vf.Shapeid=" + DDShape.SelectedValue;
                Filterby = Filterby + " , Shape - " + DDShape.SelectedItem.Text;
            }
            if (DDSize.SelectedIndex > 0)
            {
                Where = Where + " and vf.Sizeid=" + DDSize.SelectedValue;
                Filterby = Filterby + " , Size - " + DDSize.SelectedItem.Text;
            }

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@companyid", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@AsonDate", txtstockupto.Text);
            param[2] = new SqlParameter("@Where", Where);
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETSTOCKASONDATE", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                Decimal TQty = 0, TAmount = 0;
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;

                sht.Range("A1:C1").Merge();
                sht.Range("A1").Value = "Carpet Stock as on " + txtstockupto.Text + "";
                sht.Range("A2:C2").Merge();
                sht.Range("A2").Value = "Filter By :  " + Filterby;
                sht.Row(2).Height = 30;
                sht.Range("A1:C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:C2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:C2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:C2").Style.Alignment.SetWrapText();
                sht.Range("A1:C2").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:C2").Style.Font.FontSize = 10;
                sht.Range("A1:C2").Style.Font.Bold = true;
                //*******Header
                sht.Range("A3").Value = "Quality";
                sht.Range("B3").Value = "Pcs";
                sht.Range("C3").Value = "Area In Sq.yd";
                sht.Range("A3:C3").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A3:C3").Style.Font.FontSize = 11;
                sht.Range("A3:C3").Style.Font.Bold = true;
                sht.Range("B3:C3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 4;
                DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "Item_Name");
                foreach (DataRow dr in dtdistinct.Rows)
                {
                    DataView dv = new DataView(ds.Tables[0]);
                    dv.RowFilter = "Item_Name='" + dr["Item_Name"] + "'";
                    dv.Sort = "Item_name";
                    DataSet ds1 = new DataSet();
                    ds1.Tables.Add(dv.ToTable());

                    sht.Range("A" + row).Value = dr["Item_name"];
                    sht.Range("A" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row).Style.Font.FontSize = 10;
                    sht.Range("A" + row).Style.Font.Bold = true;

                    row = row + 1;
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {

                        sht.Range("A" + row + ":C" + row).Style.Font.FontName = "Arial Unicode MS";
                        sht.Range("A" + row + ":C" + row).Style.Font.FontSize = 10;

                        sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["QualityName"]);
                        sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Pcs"]);
                        sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Area"]);
                        row = row + 1;
                    }
                }
                //*************
                //*************GRAND TOTAL
                sht.Range("B" + row + ":C" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("B" + row + ":C" + row).Style.Font.FontSize = 10;
                sht.Range("B" + row + ":C" + row).Style.Font.Bold = true;

                sht.Range("B" + row).SetValue(ds.Tables[0].Compute("sum(pcs)", ""));
                sht.Range("C" + row).SetValue(ds.Tables[0].Compute("sum(Area)", ""));
                //***********BOrders
                using (var a = sht.Range("A1" + ":C" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }


                //*************

                sht.Columns(1, 10).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("CarpetstockasonDate_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "purchaserec", "alert('No Record Found!');", true);
            }


        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }


    private void StockInHandAsOnDate_Excel()
    {
        DataSet ds = new DataSet();
        string str = "";
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            str = str + " and vf.Shadecolorid=" + DDShadeColor.SelectedValue;
        }
        if (DDLotNo.SelectedIndex > 0)
        {
            str = str + " and S.LotNo='" + DDLotNo.SelectedItem.Text + "'";
        }
        if (DDGudown.SelectedIndex > 0)
        {
            str = str + " and S.GodownId=" + DDGudown.SelectedValue;
        }

        //SqlParameter[] param = new SqlParameter[3];
        //param[0] = new SqlParameter("@companyid", DDCompany.SelectedValue);
        //param[1] = new SqlParameter("@Date", txtfromdate.Text);
        //param[2] = new SqlParameter("@where", str);

        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_STOCKQTYASONDATE", param);


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_STOCKQTYASONDATE", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@companyid", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@Date", txtfromdate.Text);
        cmd.Parameters.AddWithValue("@Where", str);

        //DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************
        con.Close();
        con.Dispose();


        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("StockInHand");

            //*************
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:F1").Merge();
            sht.Range("A1:F1").Style.Font.FontSize = 10;
            sht.Range("A1:F1").Style.Font.Bold = true;
            sht.Range("A1:F1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:F1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:F1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("Stock As On Date : " + txtfromdate.Text + "");

            sht.Range("A2:F2").Style.Font.FontSize = 10;
            sht.Range("A2:F2").Style.Font.Bold = true;

            //sht.Range("A2").Value = "Tran Date";
            sht.Range("A2").Value = "Item Name";
            sht.Range("B2").Value = "Quality Name";
            sht.Range("C2").Value = "Shade Color";
            sht.Range("D2").Value = "Lot No";
            sht.Range("E2").Value = "Godown Name";
            sht.Range("F2").Value = "Stock Qty";

            int row = 3;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Shadecolorname"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Lotno"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["GodownName"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["StockQty"]);
                sht.Range("F" + row).Style.NumberFormat.Format = "@";


                row = row + 1;
            }

            //DataTable dtdistinct = DS.Tables[0].DefaultView.ToTable(true, "FinishedId", "Item_Name", "QualityName", "ShadeColorName", "GodownName", "LotNo", "BinNo", "StockQty");
            //DataView dv = new DataView(dtdistinct);
            //dv.Sort = "Item_Name, QualityName,ShadeColorName,FinishedId,GodownName,BinNo,LotNo";
            //DataSet ds1 = new DataSet();
            //ds1.Tables.Add(dv.ToTable());

            //for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            //{

            //    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Item_Name"]);
            //    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["QualityName"]);
            //    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Shadecolorname"]);
            //    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
            //    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["GodownName"]);
            //    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["StockQty"]);
            //    sht.Range("F" + row).Style.NumberFormat.Format = "@";


            //    row = row + 1;
            //}
            //DS.Dispose();
            //ds1.Dispose();
            ////*************************************************
            using (var a = sht.Range("A1" + ":F" + row))
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
            string filename = UtilityModule.validateFilename("StockAsOnDate_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "o", "alert('No Record Found!');", true);
        }
    }

    protected void PurchaseMaterialasondate()
    {
        string str = "";
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            str = str + " and vf.Shadecolorid=" + DDShadeColor.SelectedValue;
        }
        if (DDLotNo.SelectedIndex > 0)
        {
            str = str + " and PRD.LotNo='" + DDLotNo.SelectedItem.Text + "'";
        }
        if (DDGudown.SelectedIndex > 0)
        {
            str = str + " and PRD.GodownId=" + DDGudown.SelectedValue;
        }
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@companyid", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@Date", txtfromdate.Text);
        param[2] = new SqlParameter("@where", str);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_purchasematerialstockasondate", param);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "reports/Rptpurchasematerialasondate.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\Rptpurchasematerialasondate.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "o", "alert('No Record Found!');", true);
        }
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/main.aspx");
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        TRDDSize.Visible = true;

        if (DDShape.SelectedItem.Text.Trim() != "ALL")
        {
            UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SIZEID, SIZEft AS SIZENAME FROM SIZE WHERE SHAPEID=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyid"] + " ORDER BY SIZEID", true, "ALL");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDSize, "SELECT SIZEID, SIZEft AS SIZENAME FROM SIZE Where MasterCompanyId=" + Session["varCompanyid"] + " ORDER BY SIZEID", true, "ALL");
        }
    }
    protected void RDFinishedStock_CheckedChanged(object sender, EventArgs e)
    {
        if (RDFinishedStock.Checked == true)
        {
            TRDDGodow.Visible = true;
            TRDDCompany.Visible = true;
            TRDDCustName.Visible = true;
            TRDate.Visible = false;
            TRDDOrder.Visible = true;
            TRDDLotNo.Visible = true;
            TRCheckStockDetail.Visible = false;
        }
    }
    protected void RDRawMaterialStock_CheckedChanged(object sender, EventArgs e)
    {
        TRDDGodow.Visible = true;
        TRDDCompany.Visible = true;
        TRDDCustName.Visible = true;
        TRDate.Visible = false;
        TRDDOrder.Visible = true;
        TRDDLotNo.Visible = false;
        TRCheckStockDetail.Visible = false;

    }
    protected void RDOneDayReport_CheckedChanged(object sender, EventArgs e)
    {
        TRDate.Visible = true;
        TRDDCompany.Visible = false;
        TRDDGodow.Visible = false;
        TRDDCustName.Visible = false;
        TRDDOrder.Visible = false;
        TRDDLotNo.Visible = false;
        TRCheckStockDetail.Visible = false;
    }
    protected void RDTotalStock_CheckedChanged(object sender, EventArgs e)
    {
        TRDDGodow.Visible = true;
        TRDDCompany.Visible = true;
        TRDDCustName.Visible = true;
        TRDate.Visible = false;
        TRDDOrder.Visible = false;
        TRDDLotNo.Visible = true;
        TRCheckStockDetail.Visible = false;
    }


    protected void RDrawmaterialopeningreport_CheckedChanged(object sender, EventArgs e)
    {
        TRDDGodow.Visible = true;
        TRDDCompany.Visible = true;
        TRDDCustName.Visible = false;
        TRDate.Visible = false;
        TRDDOrder.Visible = false;
        TRDDLotNo.Visible = false;
        trforfromandtodate.Visible = true;
        TRCheckStockDetail.Visible = true;
    }
    protected void RDRawMaterialStockGodownWise_CheckedChanged(object sender, EventArgs e)
    {
        chkformtr.Visible = false;
        chkallstockno.Visible = false;
        TRDDGodow.Visible = true;
        TRDDCompany.Visible = true;
        TRDDCustName.Visible = false;
        TRDate.Visible = false;
        TRDDOrder.Visible = false;
        TRDDLotNo.Visible = false;
        TRCheckStockDetail.Visible = false;
        chkunconfirmcarpet.Visible = false;
        Trwithpurchasedetail.Visible = false;
        chkstocksummary.Visible = false;
        trforfromandtodate.Visible = false;
        TRLotNoTagNo.Visible = true;
        TDundyed_dyed.Visible = true;
        TDstockupto.Visible = true;
        TDForLedgerDetail.Visible = false;
        TDExportExcel.Visible = false;

        if (RDRawMaterialStockGodownWise.Checked == true)
        {
            TDForLedgerDetail.Visible = true;
            UtilityModule.ConditionalComboFill(ref DDCompany, "Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varCompanyId"] + @" Order by Companyname", true, "ALL");
            UtilityModule.ConditionalComboFill(ref DDCategory, "select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " and CATEGORY_NAME='RAW MATERIAL' order by CATEGORY_NAME", true, "ALL");
            if (DDCategory.Items.Count > 0)
            {
                DDCategory.SelectedIndex = 1;
                DDCategory.Enabled = false;
            }
            DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();

            UtilityModule.ConditionalComboFill(ref ddItemName, "Select ITEM_ID,ITEM_NAME from ITEM_MASTER " + "WHERE CATEGORY_ID = " + DDCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by ITEM_NAME", true, "ALL");
        }
        else
        {
            DDCategory.Enabled = true;
            DDCategory.SelectedIndex = 0;
            TDForLedgerDetail.Visible = false;
        }


    }
    public void PurchaseIssue_ReceiveDetail()
    {
        try
        {
            string rpttitle = DDCompany.SelectedItem.Text + " ";

            //SqlParameter[] array = new SqlParameter[4];
            //array[0] = new SqlParameter("@Fromdate", SqlDbType.DateTime);
            //array[1] = new SqlParameter("@Todate", SqlDbType.DateTime);
            //array[2] = new SqlParameter("@Where", SqlDbType.VarChar, 8000);
            //array[3] = new SqlParameter("@CompanyName", SqlDbType.VarChar, 50);

            string sQry = " S.CompanyId=" + DDCompany.SelectedValue + " And Vf.mastercompanyId=" + Session["varcompanyId"] + "";

            if (DDCategory.SelectedIndex > 0 && DDCategory.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND vf.CATEGORY_ID = " + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0 && ddItemName.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND vf.ITEM_ID = " + ddItemName.SelectedValue;

            }
            if (DDQuality.SelectedIndex > 0 && DDQuality.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND vf.QUALITYID = " + DDQuality.SelectedValue;
                rpttitle = rpttitle + "  Quality-" + DDQuality.SelectedItem.Text;
            }
            if (DDDesign.SelectedIndex > 0 && DDDesign.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND vf.DESIGNID = " + DDDesign.SelectedValue;

            }
            if (DDColor.SelectedIndex > 0 && DDColor.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND vf.COLORID = " + DDColor.SelectedValue;
            }
            if (DDShadeColor.SelectedIndex > 0 && DDShadeColor.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND vf.SHADECOLORID = " + DDShadeColor.SelectedValue;
                rpttitle = rpttitle + "  Color-" + DDShadeColor.SelectedItem.Text;
            }
            if (DDShape.SelectedIndex > 0 && DDShape.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND vf.SHAPEID = " + DDShape.SelectedValue;

            }
            if (DDSize.SelectedIndex > 0 && DDSize.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND vf.sizeid = " + DDSize.SelectedValue;

            }
            if (DDGudown.SelectedIndex > 0)
            {
                sQry = sQry + " AND S.godownid = " + DDGudown.SelectedValue;
                rpttitle = rpttitle + "  Godown-" + DDGudown.SelectedItem.Text;
            }
            if (txtLotno.Text != "")
            {
                sQry = sQry + " AND S.LotNo ='" + txtLotno.Text + "'";
                rpttitle = rpttitle + "  LotNo-" + txtLotno.Text;
            }
            if (txtTagno.Text != "")
            {
                sQry = sQry + " AND S.TagNo ='" + txtTagno.Text + "'";
                rpttitle = rpttitle + "  Tag No-" + txtTagno.Text;
            }
            rpttitle = rpttitle + "  From Date-" + txtfromdate.Text + " To-" + txttodate.Text;
            rpttitle = "(Stock Transaction Detail) " + rpttitle;

            //array[0].Value = txtfromdate.Text;
            //array[1].Value = txttodate.Text;
            //array[2].Value = sQry;
            //array[3].Value = DDCompany.SelectedItem.Text;

            //DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetPurchaseissue_ReceiveDetail", array);

           
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_GetPurchaseissue_ReceiveDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@Fromdate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
            cmd.Parameters.AddWithValue("@Where", sQry);
            cmd.Parameters.AddWithValue("@CompanyName", DDCompany.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@userid",Convert.ToInt32(Session["VarUserId"] ) );   


            //DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(DS);
            //*************

            con.Close();
            con.Dispose();

            if (chkpurchaseissuerecsummary.Checked == true)
            {
                PurchaseIssue_receiveExcel_Summary(DS, rpttitle);
            }
            else
            {
                if (Session["varcompanyId"].ToString() == "22")
                {
                    PurchaseIssue_receiveExcelDiamond(DS, rpttitle);
                }
                else
                {
                    PurchaseIssue_receiveExcel(DS, rpttitle);
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
            // UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmProcessDetailIssueReceive.aspx");

        }

    }
    protected void PurchaseIssue_receiveExcel(DataSet DS, string rpttitle = "")
    {
        // DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);
        if (DS.Tables[0].Rows.Count > 0)
        {
            int ii;
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("TransactionDetail");
            //**********************
            sht.Range("A1:J1").Merge();
            sht.Range("A1").Value = rpttitle;
            sht.Range("A1:J1").Style.Font.Bold = true;
            sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:J1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:J1").Style.Font.FontSize = 13;
            sht.Row(1).Height = 30.00;

            sht.Range("A2").Value = "TRNS DATE";
            sht.Range("B2").Value = "ITEMNAME";
            sht.Range("C2").Value = "LOTNO";
            sht.Range("D2").Value = "COLOR";
            sht.Range("E2").Value = "TAGNO";
            sht.Range("F2").Value = "REC FROM";
            sht.Range("G2").Value = "REC QTY";
            sht.Range("H2").Value = "ISSUE QTY";
            sht.Range("I2").Value = "ISSUE DETAILS";
            sht.Range("J2").Value = "VOUCHER NO";
            sht.Range("A2:J2").Style.Font.Bold = true;
            sht.Range("A2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:J2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:J2").Style.Font.FontSize = 13;
            sht.Row(1).Height = 25.50;
            //**********************
            // DS.Tables[0].DefaultView.Sort = "order by Trandate ASC,Item_Finished_id ASC";
            DataView dv = DS.Tables[0].DefaultView;
            dv.Sort = "Trandate ASC,Item_Finished_id ASC";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            //*****excel row
            ii = 3;
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + ii + ":J" + ii).Style.NumberFormat.Format = "@";
                sht.Range("A" + ii).SetValue(ds1.Tables[0].Rows[i]["TranDate"]);
                sht.Range("B" + ii).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                // sht.Range("B" + ii).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);                    
                sht.Range("C" + ii).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
                sht.Range("D" + ii).SetValue(ds1.Tables[0].Rows[i]["Shadecolorname"]);
                sht.Range("E" + ii).SetValue(ds1.Tables[0].Rows[i]["Tagno"]);
                sht.Range("F" + ii).SetValue(ds1.Tables[0].Rows[i]["Recfrom"]);
                sht.Range("G" + ii).SetValue(ds1.Tables[0].Rows[i]["Recqty"]);
                sht.Range("G" + ii).SetValue(Convert.ToDouble(ds1.Tables[0].Rows[i]["Recqty"]));
                sht.Range("H" + ii).SetValue(Convert.ToDouble(ds1.Tables[0].Rows[i]["Issueqty"]));
                sht.Range("I" + ii).SetValue(ds1.Tables[0].Rows[i]["Issuedetail"]);
                sht.Range("J" + ii).SetValue(ds1.Tables[0].Rows[i]["voucherno"]);

                ii = ii + 1;
            }
            //************************                
            sht.Columns(1, 10).AdjustToContents();
            //******SAVE FILE
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("StockTransactionDetail" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void PurchaseIssue_receiveExcel_Summary(DataSet DS, string rpttitle = "")
    {
        // DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);
        if (DS.Tables[0].Rows.Count > 0)
        {
            int ii;
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("TransactionDetail");
            //**********************
            sht.Range("A1:E1").Merge();
            sht.Range("A1").Value = "STOCK TRANSACTION DETAIL";
            sht.Range("A1:E1").Style.Font.Bold = true;
            sht.Range("A1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:E1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            sht.Range("A2:B2").Merge();
            sht.Range("A2").SetValue(System.DateTime.Now.ToString());


            sht.Range("C2:E2").Merge();
            sht.Range("C2").Value = txtfromdate.Text + " To " + txttodate.Text;


            sht.Range("A3").Value = "TRNS DATE";
            sht.Range("B3").Value = "ITEMNAME";
            sht.Range("C3").Value = "COLOR";
            sht.Range("D3").Value = "REC QTY";
            sht.Range("E3").Value = "ISSUE QTY";
            sht.Range("A3:E3").Style.Font.Bold = true;
            sht.Range("A3:E3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A3:E3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Row(1).Height = 25.50;
            //**********************
            ii = 4;
            int rfrom = 0;
            rfrom = ii;
            DataTable dtdistinct = DS.Tables[0].DefaultView.ToTable(true, "TranDate", "TranDate1", "Item_Name", "QualityName", "Shadecolorname");
            DataView dv = new DataView(dtdistinct);
            dv.Sort = "TranDate,Item_Name,QualityName,shadecolorname";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + ii).SetValue(ds1.Tables[0].Rows[i]["TranDate"]);
                sht.Range("B" + ii).SetValue(ds1.Tables[0].Rows[i]["Item_Name"] + "  " + ds1.Tables[0].Rows[i]["Qualityname"]);
                sht.Range("C" + ii).SetValue(ds1.Tables[0].Rows[i]["Shadecolorname"]);

                var Recqty = DS.Tables[0].Compute("sum(Recqty)", "TranDate1='" + ds1.Tables[0].Rows[i]["TranDate1"] + "' and Item_Name='" + ds1.Tables[0].Rows[i]["Item_Name"] + "' and  QualityName='" + ds1.Tables[0].Rows[i]["Qualityname"] + "' and  Shadecolorname='" + ds1.Tables[0].Rows[i]["Shadecolorname"] + "'");
                var issqty = DS.Tables[0].Compute("sum(issueqty)", "TranDate1='" + ds1.Tables[0].Rows[i]["TranDate1"] + "' and Item_Name='" + ds1.Tables[0].Rows[i]["Item_Name"] + "' and  QualityName='" + ds1.Tables[0].Rows[i]["Qualityname"] + "' and  Shadecolorname='" + ds1.Tables[0].Rows[i]["Shadecolorname"] + "'");
                sht.Range("D" + ii).SetValue(Recqty);
                sht.Range("E" + ii).SetValue(issqty);

                ii = ii + 1;
            }
            sht.Range("C" + ii).Value = "Total";
            sht.Range("D" + ii).FormulaA1 = "=SUM(D" + rfrom + ":$D$" + (ii - 1) + ")";
            sht.Range("E" + ii).FormulaA1 = "=SUM(E" + rfrom + ":$E$" + (ii - 1) + ")";
            sht.Range("C" + ii + ":E" + ii).Style.Font.SetBold();

            using (var a = sht.Range("A1:E" + ii))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            }
            //************************                
            sht.Columns(1, 10).AdjustToContents();
            //******SAVE FILE
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("StockTransactionDetailSummary" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void chkstocksummary_CheckedChanged(object sender, EventArgs e)
    {
        TDstockupto.Visible = false;
        Trwithpurchasedetail.Visible = false;
        chkwithpurchasedetail.Checked = false;
        if (chkstocksummary.Checked == true)
        {
            TRDDShadeColor.Visible = false;
            DDShadeColor.SelectedIndex = -1;
            TDundyed_dyed.Visible = true;
            switch (Session["varcompanyNo"].ToString())
            {
                case "14":
                    break;
                default:
                    TDstockupto.Visible = true;
                    break;
            }
            TDstockupto.Visible = true;
        }
        else
        {
            TRDDShadeColor.Visible = true;
            TDundyed_dyed.Visible = false;
            Trwithpurchasedetail.Visible = true;
        }

        if (Session["varcompanyid"].ToString() != "14")
        {
            TDundyed_dyed.Visible = true;
        }
    }
    protected void txtLotno_TextChanged(object sender, EventArgs e)
    {
        string str = @"select Replace(convert(nvarchar(11),Min(Prm.receivedate),106),' ','-') as Receivedate From Purchasereceivemaster PRM inner join PurchaseReceiveDetail PRD on PRM.PurchaseReceiveId=PRd.PurchaseReceiveId
                     Where Prd.LotNo='" + txtLotno.Text + "'";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (ds.Tables[0].Rows[0]["Receivedate"].ToString() != "")
        {
            txtfromdate.Text = ds.Tables[0].Rows[0]["Receivedate"].ToString();
        }
        else
        {
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }

    }
    protected void shadewisestock()
    {
        string sQry = "", filterby = "";
        if (DDGudown.SelectedIndex > 0)
        {
            sQry = sQry + "AND S.godownid =" + DDGudown.SelectedValue;
            filterby = filterby + " Godown-" + DDGudown.SelectedItem.Text;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            sQry = sQry + " AND vf.CATEGORY_ID = " + DDCategory.SelectedValue;

        }
        if (ddItemName.SelectedIndex > 0)
        {
            sQry = sQry + " AND vf.ITEM_ID = " + ddItemName.SelectedValue;
            filterby = filterby + ",Item-" + ddItemName.SelectedItem.Text;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            sQry = sQry + " AND vf.QUALITYID = " + DDQuality.SelectedValue;
            filterby = filterby + ",Quality-" + DDQuality.SelectedItem.Text;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            sQry = sQry + " AND vf.DESIGNID = " + DDDesign.SelectedValue;
            filterby = filterby + ",Design-" + DDDesign.SelectedItem.Text;

        }
        if (DDColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND vf.COLORID = " + DDColor.SelectedValue;
            filterby = filterby + ",Color-" + DDColor.SelectedItem.Text;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND vf.SHADECOLORID = " + DDShadeColor.SelectedValue;
            filterby = filterby + ",ShadeColor-" + DDShadeColor.SelectedItem.Text;

        }
        if (DDSize.SelectedIndex > 0)
        {
            sQry = sQry + " AND vf.Sizeid = " + DDSize.SelectedValue;

        }
        if (DDLotNo.SelectedIndex > 0)
        {
            sQry = sQry + " AND s.lotno = " + DDLotNo.SelectedValue;
            filterby = filterby + ",LotNo-" + DDLotNo.SelectedItem.Text;
        }
        if (txtLotno.Text != "")
        {
            sQry = sQry + " AND s.lotno = '" + txtLotno.Text + "'";
            filterby = filterby + ",LotNo-" + txtLotno.Text;
        }
        if (txtTagno.Text != "")
        {
            sQry = sQry + " AND s.TagNo = '" + txtTagno.Text + "'";
            filterby = filterby + ",TagNo-" + txtTagno.Text;
        }
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@FromDate", txtfromdate.Text);
        param[2] = new SqlParameter("@ToDate", txttodate.Text);
        param[3] = new SqlParameter("@Where", sQry);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETSHADEWISESTOCK", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Raw Material Stock");
            //************

            sht.Range("A1:E1").Merge();
            sht.Range("A1").SetValue("SHADE WISE REPORT");
            sht.Range("A1:E1").Style.Font.Bold = true;
            sht.Range("A1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //Filter by 
            sht.Range("A2:E2").Merge();
            sht.Range("A2").SetValue("From : " + txtfromdate.Text + " TO : " + txttodate.Text);
            sht.Range("A2:E2").Style.Font.Bold = true;
            sht.Range("A2:E2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:E2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:E2").Style.Alignment.WrapText = true;
            //Headings
            sht.Range("A3:E3").Style.Font.Bold = true;
            sht.Range("D3:E3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3").SetValue("Item Name");
            sht.Range("B3").SetValue("Quality");
            sht.Range("C3").SetValue("Shade No.");
            sht.Range("D3").SetValue("Issue Qty");
            sht.Range("E3").SetValue("Closing Stock");
            using (var a = sht.Range("A3:E3"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //***************
            int row = 4;
            DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "Item_Name", "QualityName");
            foreach (DataRow dr in dtdistinct.Rows)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "Item_Name='" + dr["Item_Name"] + "' and QualityName='" + dr["QualityName"] + "'";
                dv.Sort = "Item_name,QualityName";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());

                sht.Range("A" + row).Value = dr["Item_name"];
                sht.Range("B" + row).Value = dr["QualityName"];
                using (var a = sht.Range("A" + row + ":E" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //row = row + 1;
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    using (var a = sht.Range("C" + row + ":E" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["shadecolorname"].ToString());
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Issueqty"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["CurrentstockQty"]);
                    row = row + 1;
                }
            }
            DS.Dispose();

            sht.Columns(1, 8).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("Shadewisereport_" + DateTime.Now + "." + Fileextension);
            string Path = "";
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "altshade", "alert('No records found for this combination.')", true);
        }
    }

    protected void PurchaseIssue_receiveExcelDiamond(DataSet DS, string rpttitle = "")
    {
        // DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);
        if (DS.Tables[0].Rows.Count > 0)
        {
            int ii;
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("TransactionDetail");
            //**********************
            sht.Range("A1:K1").Merge();
            sht.Range("A1").Value = rpttitle;
            sht.Range("A1:K1").Style.Font.Bold = true;
            sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:K1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:K1").Style.Font.FontSize = 13;
            sht.Row(1).Height = 30.00;

            sht.Range("A2").Value = "TRNS DATE";
            sht.Range("B2").Value = "ITEMNAME";
            sht.Range("C2").Value = "LOTNO";
            sht.Range("D2").Value = "COLOR";
            sht.Range("E2").Value = "TAGNO";
            sht.Range("F2").Value = "REC QTY";
            sht.Range("G2").Value = "ISSUE QTY";
            sht.Range("H2").Value = "ISSUE DETAILS";
            sht.Range("I2").Value = "ARTICLE";
            sht.Range("J2").Value = "COLOR";
            sht.Range("K2").Value = "SIZE";

            sht.Range("L2").Value = "VOUCHER NO";
            sht.Range("M2").Value = "STOCK NO";
            sht.Range("A2:M2").Style.Font.Bold = true;
            sht.Range("A2:M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:M2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:M2").Style.Font.FontSize = 13;
            sht.Row(1).Height = 25.50;
            //**********************
            // DS.Tables[0].DefaultView.Sort = "order by Trandate ASC,Item_Finished_id ASC";
            DataView dv = DS.Tables[0].DefaultView;
            dv.Sort = "Trandate ASC,Item_Finished_id ASC";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            //*****excel row
            ii = 3;
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + ii + ":J" + ii).Style.NumberFormat.Format = "@";
                sht.Range("A" + ii).SetValue(ds1.Tables[0].Rows[i]["TranDate"]);
                sht.Range("B" + ii).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                // sht.Range("B" + ii).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);                    
                sht.Range("C" + ii).SetValue(ds1.Tables[0].Rows[i]["Lotno"]);
                sht.Range("D" + ii).SetValue(ds1.Tables[0].Rows[i]["Shadecolorname"]);
                sht.Range("E" + ii).SetValue(ds1.Tables[0].Rows[i]["Tagno"]);
                sht.Range("F" + ii).SetValue(Convert.ToDouble(ds1.Tables[0].Rows[i]["Recqty"]));
                //sht.Range("G" + ii).SetValue(ds1.Tables[0].Rows[i]["Recqty"]);
                sht.Range("G" + ii).SetValue(Convert.ToDouble(ds1.Tables[0].Rows[i]["Issueqty"]));
                sht.Range("H" + ii).SetValue(ds1.Tables[0].Rows[i]["Issuedetail"]);
                sht.Range("I" + ii).SetValue(ds1.Tables[0].Rows[i]["CarpetDesignName"]);
                sht.Range("J" + ii).SetValue(ds1.Tables[0].Rows[i]["CarpetColorName"]);
                sht.Range("K" + ii).SetValue(ds1.Tables[0].Rows[i]["CarpetSize"]);

                sht.Range("L" + ii).SetValue(ds1.Tables[0].Rows[i]["voucherno"]);
                sht.Range("M" + ii).SetValue(ds1.Tables[0].Rows[i]["TStockNo"]);

                ii = ii + 1;
            }
            //************************                
            sht.Columns(1, 14).AdjustToContents();
            //******SAVE FILE
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("StockTransactionDetail" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }

    protected void RawMaterialStockGodownWise()
    {
        //int Row;        
        //String sQry = " ";
        //string shadecolor = "";        
        //string FilterBy = "Filter By-" + DDCompany.SelectedItem.Text;

        lblMessage.Text = "";
        string str = "";
        try
        {
            //if (RDundyed.Checked == true)
            //{
            //    shadecolor = "Undyed";
            //}
            //else if (RDDyed.Checked == true)
            //{
            //    shadecolor = "Dyed";
            //}

            if (TDstockupto.Visible == true)
            {
                str = str + " and St.TranDate<='" + txtstockupto.Text + "'";
            }
            if (RDundyed.Checked == true)
            {
                str = str + " and v.Shadecolorname like '%Undyed%'";
            }
            else if (RDDyed.Checked == true)
            {
                str = str + " and v.Shadecolorname not like '%Undyed%'";
            }
            if (DDGudown.SelectedIndex > 0)
            {
                str = str + "AND g.godownid =" + DDGudown.SelectedValue;
            }
            if (DDCompany.SelectedIndex > 0)
            {
                str = str + " AND s.companyid = " + DDCompany.SelectedValue;
            }
            if (DDCategory.SelectedIndex > 0)
            {
                str = str + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " AND v.QUALITYID = " + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " AND v.DESIGNID = " + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " AND v.COLORID = " + DDColor.SelectedValue;
            }
            if (DDShadeColor.SelectedIndex > 0)
            {
                str = str + " AND v.SHADECOLORID = " + DDShadeColor.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " AND v.Sizeid = " + DDSize.SelectedValue;
            }
            if (DDLotNo.SelectedIndex > 0)
            {
                str = str + " AND s.lotno = " + DDLotNo.SelectedValue;
            }
            if (txtLotno.Text != "")
            {
                str = str + " AND s.lotno = '" + txtLotno.Text + "'";
            }
            if (txtTagno.Text != "")
            {
                str = str + " AND s.Tagno = '" + txtTagno.Text + "'";
            }



            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@userid", Session["varuserid"]);
            param[1] = new SqlParameter("@MasterCompanyId", Session["varCompanyId"]);
            param[2] = new SqlParameter("@where", str);
            param[3] = new SqlParameter("@StockUpToDate", txtstockupto.Text);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetRawMaterialStockGodownWiseReport", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "Reports/RptRawMaterialStockGodownWise.rpt";
                Session["dsFileName"] = "~\\ReportSchema\\RptRawMaterialStockGodownWise.xsd";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
    //protected void FinishedStockExcelExport()
    protected void FinishedStockExcelExport(DataSet DS)
    {
        //// DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);
        if (DS.Tables[0].Rows.Count > 0)
        {
            // int ii;
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("FinsihedStockDetail");
            //**********************
            sht.Range("A1:H1").Merge();
            sht.Range("A1").Value = "Finished Stock Report";
            sht.Range("A1:H1").Style.Font.Bold = true;
            sht.Range("A1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:H1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:H1").Style.Font.FontSize = 13;
            sht.Row(1).Height = 30.00;

            sht.Range("A2").Value = "BUYER CODE";
            sht.Range("B2").Value = "STOCK NO";
            sht.Range("C2").Value = "QUALITY";
            sht.Range("D2").Value = "DESIGN";
            sht.Range("E2").Value = "COLOR";
            sht.Range("F2").Value = "SIZE";
            sht.Range("G2").Value = "QTY";

            sht.Range("A2:H2").Style.Font.Bold = true;
            sht.Range("A2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:H2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:H2").Style.Font.FontSize = 13;
            sht.Row(1).Height = 25.50;
            //**********************
            //// DS.Tables[0].DefaultView.Sort = "order by Trandate ASC,Item_Finished_id ASC";
            DataView dv = DS.Tables[0].DefaultView;
            dv.Sort = "BuyersCode ASC,Quality ASC";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            ////*****excel row
            int row = 3;
            decimal TStockQty = 0;
            for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
            {
                //sht.Range("A" + row + ":J" + row).Style.NumberFormat.Format = "@";
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["BuyersCode"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["StockNo"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Quality"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["DesignName"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Color"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["StockQty"]);
                TStockQty = TStockQty + Convert.ToDecimal(ds1.Tables[0].Rows[i]["StockQty"]);
                row = row + 1;
            }

            // Total
            sht.Range("F" + row).Style.Font.FontSize = 11;
            sht.Range("F" + row).Style.Font.Bold = true;
            sht.Range("F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("F" + row).SetValue("Total");

            sht.Range("G" + row).Style.Font.FontSize = 11;
            sht.Range("G" + row).Style.Font.Bold = true;
            sht.Range("G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            //sht.Range("G" + row).Style.NumberFormat.Format = "#,##0.000";
            sht.Range("G" + row).SetValue(TStockQty);

            //************************                
            sht.Columns(1, 12).AdjustToContents();
            //******SAVE FILE
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("FinishedStockDetailReport" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void ChkForLedgerTransaction_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForLedgerTransaction.Checked == true)
        {
            TDstockupto.Visible = false;
            trforfromandtodate.Visible = true;
            TDundyed_dyed.Visible = false;
        }
        else
        {
            TDstockupto.Visible = true;
            trforfromandtodate.Visible = false;
            TDundyed_dyed.Visible = true;
        }
    }
    protected void RawMaterialStockTransactionDetails()
    {
        //int Row;        
        //String sQry = " ";
        //string shadecolor = "";        
        //string FilterBy = "Filter By-" + DDCompany.SelectedItem.Text;

        lblMessage.Text = "";
        string str = "";
        try
        {
            if (DDCompany.SelectedIndex > 0)
            {

                if (trforfromandtodate.Visible == true)
                {
                    str = str + " and St.TranDate>='" + txtfromdate.Text + "' and St.TranDate<='" + txttodate.Text + "'";
                }
                //if (RDundyed.Checked == true)
                //{
                //    str = str + " and v.Shadecolorname like '%Undyed%'";
                //}
                //else if (RDDyed.Checked == true)
                //{
                //    str = str + " and v.Shadecolorname not like '%Undyed%'";
                //}
                if (DDGudown.SelectedIndex > 0)
                {
                    str = str + "AND g.godownid =" + DDGudown.SelectedValue;
                }

                if (DDCategory.SelectedIndex > 0)
                {
                    str = str + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;
                }
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
                }
                if (DDQuality.SelectedIndex > 0)
                {
                    str = str + " AND v.QUALITYID = " + DDQuality.SelectedValue;
                }
                if (DDDesign.SelectedIndex > 0)
                {
                    str = str + " AND v.DESIGNID = " + DDDesign.SelectedValue;
                }
                if (DDColor.SelectedIndex > 0)
                {
                    str = str + " AND v.COLORID = " + DDColor.SelectedValue;
                }
                if (DDShadeColor.SelectedIndex > 0)
                {
                    str = str + " AND v.SHADECOLORID = " + DDShadeColor.SelectedValue;
                }
                if (DDSize.SelectedIndex > 0)
                {
                    str = str + " AND v.Sizeid = " + DDSize.SelectedValue;
                }
                if (DDLotNo.SelectedIndex > 0)
                {
                    str = str + " AND s.lotno = " + DDLotNo.SelectedValue;
                }
                if (txtLotno.Text != "")
                {
                    str = str + " AND s.lotno = '" + txtLotno.Text + "'";
                }
                if (txtTagno.Text != "")
                {
                    str = str + " AND s.Tagno = '" + txtTagno.Text + "'";
                }



                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@userid", Session["varuserid"]);
                param[1] = new SqlParameter("@MasterCompanyId", Session["varCompanyId"]);
                param[2] = new SqlParameter("@where", str);
                param[3] = new SqlParameter("@FromDate", txtfromdate.Text);
                param[4] = new SqlParameter("@ToDate", txttodate.Text);
                param[5] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);


                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetRawMaterialStockLedgerReport", param);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["rptFileName"] = "Reports/RptRawMaterialStockLedger.rpt";
                    Session["dsFileName"] = "~\\ReportSchema\\RptRawMaterialStockLedger.xsd";
                    Session["GetDataset"] = ds;
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('Please Select Company Name..')", true);
            }


        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }

    protected void shadewisestockDiamondExport()
    {
        DataSet ds = new DataSet();
        string sQry = "", filterby = "";
        if (DDGudown.SelectedIndex > 0)
        {
            sQry = sQry + "AND S.godownid =" + DDGudown.SelectedValue;
            filterby = filterby + " Godown-" + DDGudown.SelectedItem.Text;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            sQry = sQry + " AND vf.CATEGORY_ID = " + DDCategory.SelectedValue;

        }
        if (ddItemName.SelectedIndex > 0)
        {
            sQry = sQry + " AND vf.ITEM_ID = " + ddItemName.SelectedValue;
            filterby = filterby + ",Item-" + ddItemName.SelectedItem.Text;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            sQry = sQry + " AND vf.QUALITYID = " + DDQuality.SelectedValue;
            filterby = filterby + ",Quality-" + DDQuality.SelectedItem.Text;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            sQry = sQry + " AND vf.DESIGNID = " + DDDesign.SelectedValue;
            filterby = filterby + ",Design-" + DDDesign.SelectedItem.Text;

        }
        if (DDColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND vf.COLORID = " + DDColor.SelectedValue;
            filterby = filterby + ",Color-" + DDColor.SelectedItem.Text;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND vf.SHADECOLORID = " + DDShadeColor.SelectedValue;
            filterby = filterby + ",ShadeColor-" + DDShadeColor.SelectedItem.Text;

        }
        if (DDSize.SelectedIndex > 0)
        {
            sQry = sQry + " AND vf.Sizeid = " + DDSize.SelectedValue;

        }
        if (DDLotNo.SelectedIndex > 0)
        {
            sQry = sQry + " AND s.lotno = " + DDLotNo.SelectedValue;
            filterby = filterby + ",LotNo-" + DDLotNo.SelectedItem.Text;
        }
        if (txtLotno.Text != "")
        {
            sQry = sQry + " AND s.lotno = '" + txtLotno.Text + "'";
            filterby = filterby + ",LotNo-" + txtLotno.Text;
        }
        if (txtTagno.Text != "")
        {
            sQry = sQry + " AND s.TagNo = '" + txtTagno.Text + "'";
            filterby = filterby + ",TagNo-" + txtTagno.Text;
        }
        //SqlParameter[] param = new SqlParameter[4];
        //param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        //param[1] = new SqlParameter("@FromDate", txtfromdate.Text);
        //param[2] = new SqlParameter("@ToDate", txttodate.Text);
        //param[3] = new SqlParameter("@Where", sQry);

        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETSHADEWISESTOCKDiamondExport", param);


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GETSHADEWISESTOCKDiamondExport", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@Fromdate", txtfromdate.Text);
        cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
        cmd.Parameters.AddWithValue("@Where", sQry);
        cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);

        // DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();
        //***********     
        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Raw Material Stock");
            //************

            sht.Range("A1:J1").Merge();
            sht.Range("A1").SetValue("SHADE WISE REPORT");
            sht.Range("A1:J1").Style.Font.Bold = true;
            sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //Filter by 
            sht.Range("A2:J2").Merge();
            sht.Range("A2").SetValue("From : " + txtfromdate.Text + " TO : " + txttodate.Text);
            sht.Range("A2:J2").Style.Font.Bold = true;
            sht.Range("A2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:J2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:J2").Style.Alignment.WrapText = true;
            //Headings
            sht.Range("A3:J3").Style.Font.Bold = true;
            sht.Range("D3:J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3").SetValue("ITEM NAME");
            sht.Range("B3").SetValue("QUALITY");
            sht.Range("C3").SetValue("SHADE NO");
            sht.Range("D3").SetValue("LOTNO");
            sht.Range("E3").SetValue("TAGNO");

            sht.Range("F3").SetValue("OPENING QTY (KG)");
            sht.Range("G3").SetValue("ISSUE QTY (KG)");
            sht.Range("H3").SetValue("RECEIVE QTY (KG)");
            sht.Range("I3").SetValue("CLOSING STOCK (KG)");
            sht.Range("J3").SetValue("CATEGORY NAME");
            sht.Range("J3:J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            using (var a = sht.Range("A3:J3"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //***************
            int row = 4;
            DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "Item_Name", "QualityName");
            foreach (DataRow dr in dtdistinct.Rows)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "Item_Name='" + dr["Item_Name"] + "' and QualityName='" + dr["QualityName"] + "'";
                dv.Sort = "Item_name,QualityName";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());

                //sht.Range("A" + row).Value = dr["Item_name"];
                //sht.Range("B" + row).Value = dr["QualityName"];
                //using (var a = sht.Range("A" + row + ":E" + row))
                //{
                //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //}
                ////row = row + 1;
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    using (var a = sht.Range("A" + row + ":J" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Item_name"].ToString());
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["QualityName"].ToString());
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["shadecolorname"].ToString());
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["LotNo"].ToString());
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["TagNo"].ToString());
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["BeforeToDateSTOCKQTY"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Issueqty"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["ReceiveQTY"]);
                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["CurrentstockQty"]);
                    sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Category_Name"]);
                    row = row + 1;
                }
            }
            DS.Dispose();

            sht.Columns(1, 12).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("ShadewisereportDiamond_" + DateTime.Now + "." + Fileextension);
            string Path = "";
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "altshade", "alert('No records found for this combination.')", true);
        }
    }
    protected void IssueRegisterReportForDiamondExport()
    {
        DataSet ds = new DataSet();
        string sQry = "", filterby = "";
        //if (DDGudown.SelectedIndex > 0)
        //{
        //    sQry = sQry + "AND S.godownid =" + DDGudown.SelectedValue;
        //    filterby = filterby + " Godown-" + DDGudown.SelectedItem.Text;
        //}
        //if (DDCategory.SelectedIndex > 0)
        //{
        //    sQry = sQry + " AND vf.CATEGORY_ID = " + DDCategory.SelectedValue;

        //}
        //if (ddItemName.SelectedIndex > 0)
        //{
        //    sQry = sQry + " AND vf.ITEM_ID = " + ddItemName.SelectedValue;
        //    filterby = filterby + ",Item-" + ddItemName.SelectedItem.Text;
        //}
        //if (DDQuality.SelectedIndex > 0)
        //{
        //    sQry = sQry + " AND vf.QUALITYID = " + DDQuality.SelectedValue;
        //    filterby = filterby + ",Quality-" + DDQuality.SelectedItem.Text;
        //}
        //if (DDDesign.SelectedIndex > 0)
        //{
        //    sQry = sQry + " AND vf.DESIGNID = " + DDDesign.SelectedValue;
        //    filterby = filterby + ",Design-" + DDDesign.SelectedItem.Text;

        //}
        //if (DDColor.SelectedIndex > 0)
        //{
        //    sQry = sQry + " AND vf.COLORID = " + DDColor.SelectedValue;
        //    filterby = filterby + ",Color-" + DDColor.SelectedItem.Text;
        //}
        //if (DDShadeColor.SelectedIndex > 0)
        //{
        //    sQry = sQry + " AND vf.SHADECOLORID = " + DDShadeColor.SelectedValue;
        //    filterby = filterby + ",ShadeColor-" + DDShadeColor.SelectedItem.Text;

        //}
        //if (DDSize.SelectedIndex > 0)
        //{
        //    sQry = sQry + " AND vf.Sizeid = " + DDSize.SelectedValue;

        //}
        //if (DDLotNo.SelectedIndex > 0)
        //{
        //    sQry = sQry + " AND s.lotno = " + DDLotNo.SelectedValue;
        //    filterby = filterby + ",LotNo-" + DDLotNo.SelectedItem.Text;
        //}
        //if (txtLotno.Text != "")
        //{
        //    sQry = sQry + " AND s.lotno = '" + txtLotno.Text + "'";
        //    filterby = filterby + ",LotNo-" + txtLotno.Text;
        //}
        //if (txtTagno.Text != "")
        //{
        //    sQry = sQry + " AND s.TagNo = '" + txtTagno.Text + "'";
        //    filterby = filterby + ",TagNo-" + txtTagno.Text;
        //}

        //SqlParameter[] param = new SqlParameter[4];
        //param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        //param[1] = new SqlParameter("@FromDate", txtfromdate.Text);
        //param[2] = new SqlParameter("@ToDate", txttodate.Text);
        //param[3] = new SqlParameter("@Where", sQry);

        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GetIssueRegisterReportDiamondExport", param);


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GetIssueRegisterReportDiamondExport", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Fromdate", txtfromdate.Text);
        cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
        cmd.Parameters.AddWithValue("@Where", sQry);
        cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);

        // DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();
        //***********           

        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("ISSUE REGISTER");
            //************

            sht.Range("A1:M1").Merge();
            sht.Range("A1").SetValue(ds.Tables[0].Rows[0]["CompanyName"].ToString() + " " + "WOOL ISSUE REPORT");
            sht.Range("A1:M1").Style.Font.Bold = true;
            sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //Filter by 
            sht.Range("A2:M2").Merge();
            sht.Range("A2").SetValue("From : " + txtfromdate.Text + " TO : " + txttodate.Text);
            sht.Range("A2:M2").Style.Font.Bold = true;
            sht.Range("A2:M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:M2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:M2").Style.Alignment.WrapText = true;
            //Headings
            sht.Range("A3:M3").Style.Font.Bold = true;
            sht.Range("J3:M3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3").SetValue("DATE");
            sht.Range("B3").SetValue("FOLIO NO");
            sht.Range("C3").SetValue("EMP NAME");
            sht.Range("D3").SetValue("LOOM NO");
            sht.Range("E3").SetValue("ITEM NAME");
            sht.Range("F3").SetValue("QUALITY");
            sht.Range("G3").SetValue("DESIGN");
            sht.Range("H3").SetValue("COLOR");
            sht.Range("I3").SetValue("SIZE");
            sht.Range("J3").SetValue("WO QTY");
            sht.Range("K3").SetValue("BOM WT");
            sht.Range("L3").SetValue("ISSUE WT");
            sht.Range("M3").SetValue("DIFFERENCE");


            using (var a = sht.Range("A3:M3"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //***************
            int row = 4;
            DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "MaterialIssueDate", "ITEM_NAME", "QualityName", "DesignName", "ColorName", "SizeFt");
            foreach (DataRow dr in dtdistinct.Rows)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "MaterialIssueDate='" + dr["MaterialIssueDate"] + "' and ITEM_NAME='" + dr["ITEM_NAME"] + "' and QualityName='" + dr["QualityName"] + "' and DesignName='" + dr["DesignName"] + "' and ColorName='" + dr["ColorName"] + "' and SizeFt='" + dr["SizeFt"] + "'";
                dv.Sort = "Item_name,QualityName,DesignName,ColorName,SizeFt";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());

                //sht.Range("A" + row).Value = dr["Item_name"];
                //sht.Range("B" + row).Value = dr["QualityName"];
                //using (var a = sht.Range("A" + row + ":E" + row))
                //{
                //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //}
                ////row = row + 1;

                var issqty = 0;
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    using (var a = sht.Range("A" + row + ":M" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["MaterialIssueDate"].ToString());
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["IssueOrderId"].ToString());
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["EMPNAME"].ToString());
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["LOOMNo"].ToString());
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["ITEM_NAME"].ToString());
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["DesignName"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["ColorName"]);
                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Width"] + "X" + ds1.Tables[0].Rows[i]["Length"]);
                    sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);
                    sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["ConsumptionQTY"]);
                    sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["WeaverIssueQty"]);
                    sht.Range("M" + row).SetValue(Math.Round(Convert.ToDecimal(ds1.Tables[0].Rows[i]["ConsumptionQTY"]) - Convert.ToDecimal(ds1.Tables[0].Rows[i]["WeaverIssueQty"]), 3));

                    if (issqty == 0)
                    {
                        issqty = Convert.ToInt32(ds1.Tables[0].Compute("sum(Qty)", "MaterialIssueDate='" + ds1.Tables[0].Rows[i]["MaterialIssueDate"] + "' and Item_Name='" + ds1.Tables[0].Rows[i]["Item_Name"] + "' and  QualityName='" + ds1.Tables[0].Rows[i]["Qualityname"] + "' and  DesignName='" + ds1.Tables[0].Rows[i]["DesignName"] + "' and  ColorName='" + ds1.Tables[0].Rows[i]["ColorName"] + "' and  SizeFt='" + ds1.Tables[0].Rows[i]["SizeFt"] + "'"));
                    }

                    row = row + 1;
                }

                using (var a = sht.Range("A" + row + ":M" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                sht.Range("J" + row).SetValue(issqty);
                sht.Range("J" + row).Style.Font.SetBold();
                row = row + 2;

            }
            DS.Dispose();

            sht.Columns(1, 26).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("IssueRegisterReportDiamond_" + DateTime.Now + "." + Fileextension);
            string Path = "";
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "altshade", "alert('No records found for this combination.')", true);
        }
    }

    public void RawMaterialOpeningWithLotNo()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] array = new SqlParameter[3];
            array[0] = new SqlParameter("@Fromdate", SqlDbType.DateTime);
            array[1] = new SqlParameter("@Todate", SqlDbType.DateTime);
            array[2] = new SqlParameter("@Where", SqlDbType.VarChar, 8000);
            string sQry = " S.CompanyId=" + DDCompany.SelectedValue + " And V.mastercompanyId=" + Session["varcompanyId"];

            if (DDCategory.SelectedIndex > 0 && DDCategory.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0 && ddItemName.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0 && DDQuality.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND v.QUALITYID = " + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0 && DDDesign.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND v.DESIGNID = " + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0 && DDColor.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND v.COLORID = " + DDColor.SelectedValue;
            }
            if (DDShadeColor.SelectedIndex > 0 && DDShadeColor.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND v.SHADECOLORID = " + DDShadeColor.SelectedValue;
            }
            if (DDShape.SelectedIndex > 0 && DDShape.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND v.SHAPEID = " + DDShape.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0 && DDSize.SelectedItem.Text != "ALL")
            {
                sQry = sQry + " AND v.sizeid = " + DDSize.SelectedValue;
            }
            if (DDGudown.SelectedIndex > 0)
            {
                sQry = sQry + " AND S.godownid = " + DDGudown.SelectedValue;
            }
            if (txtLotno.Text != "")
            {
                sQry = sQry + " AND S.LotNo = '" + txtLotno.Text + "'";
            }
            if (txtTagno.Text != "")
            {
                sQry = sQry + " AND S.TagNo = '" + txtTagno.Text + "'";
            }
            if (TDundyed_dyed.Visible == true)
            {
                if (RDundyed.Checked == true)
                {
                    sQry = sQry + " and v.Shadecolorname like '%Undyed%'";
                }
                else if (RDDyed.Checked == true)
                {
                    sQry = sQry + " and v.Shadecolorname not like '%Undyed%'";
                }
            }
            array[0].Value = txtfromdate.Text;
            array[1].Value = txttodate.Text;
            array[2].Value = sQry;

            DS = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "PRO_RawMaterialOpeningClosingWithLotNo", array);
            if (DS.Tables[0].Rows.Count > 0)
            {
                if (ChkExportExcel.Checked == true)
                {
                    RawMaterialOpeningClosingReportForExcelWithLotNo(DS);
                }
                else
                {

                    Session["rptFileName"] = "reports/RptStockReportOpeningWithLotNo.rpt";
                    Session["GetDataset"] = DS;
                    Session["dsFileName"] = "~\\ReportSchema\\RptStockReportOpeningWithLotNo.xsd";
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx?Export=Y', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                }
               
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
            // UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmProcessDetailIssueReceive.aspx");
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void RawMaterialStockAsOnDateWise()
    {
        //int Row;        
        //String sQry = " ";
        //string shadecolor = "";        
        //string FilterBy = "Filter By-" + DDCompany.SelectedItem.Text;

        lblMessage.Text = "";
        string str = "";
        try
        {
            ////if (RDundyed.Checked == true)
            ////{
            ////    shadecolor = "Undyed";
            ////}
            ////else if (RDDyed.Checked == true)
            ////{
            ////    shadecolor = "Dyed";
            ////}

            //if (TDstockupto.Visible == true)
            //{
            //    str = str + " and St.TranDate<='" + txtstockupto.Text + "'";
            //}
            //if (RDundyed.Checked == true)
            //{
            //    str = str + " and v.Shadecolorname like '%Undyed%'";
            //}
            //else if (RDDyed.Checked == true)
            //{
            //    str = str + " and v.Shadecolorname not like '%Undyed%'";
            //}
            if (DDGudown.SelectedIndex > 0)
            {
                str = str + "AND g.godownid =" + DDGudown.SelectedValue;
            }
            if (DDCompany.SelectedIndex > 0)
            {
                str = str + " AND s.companyid = " + DDCompany.SelectedValue;
            }
            if (DDCategory.SelectedIndex > 0)
            {
                str = str + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " AND v.QUALITYID = " + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " AND v.DESIGNID = " + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " AND v.COLORID = " + DDColor.SelectedValue;
            }
            if (DDShadeColor.SelectedIndex > 0)
            {
                str = str + " AND v.SHADECOLORID = " + DDShadeColor.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " AND v.Sizeid = " + DDSize.SelectedValue;
            }
            if (DDLotNo.SelectedIndex > 0)
            {
                str = str + " AND s.lotno = " + DDLotNo.SelectedValue;
            }
            if (txtLotno.Text != "")
            {
                str = str + " AND s.lotno = '" + txtLotno.Text + "'";
            }
            if (txtTagno.Text != "")
            {
                str = str + " AND s.Tagno = '" + txtTagno.Text + "'";
            }



            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@userid", Session["varuserid"]);
            param[1] = new SqlParameter("@MasterCompanyId", Session["varCompanyId"]);
            param[2] = new SqlParameter("@where", str);
            param[3] = new SqlParameter("@StockUpToDate", txtstockupto.Text);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetRawMaterialStockAsOnDateReport", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "Reports/RptRawMaterialStockAsOnDateWise.rpt";
                Session["dsFileName"] = "~\\ReportSchema\\RptRawMaterialStockAsOnDateWise.xsd";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
    public void RawMaterialOpeningClosingReportForExcelWithLotNo(DataSet DS)
    {
        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("RawMaterialOpenCloseWithLotNo");

        //*************
        //***********
        sht.Row(1).Height = 24;
        sht.Range("A1:M1").Merge();
        sht.Range("A1:M1").Style.Font.FontSize = 10;
        sht.Range("A1:M1").Style.Font.Bold = true;
        sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A1:M1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("A1:M1").Style.Alignment.WrapText = true;
        //************
        sht.Range("A1").SetValue(DS.Tables[0].Rows[0]["CompanyName"]);

        sht.Row(1).Height = 24;
        sht.Range("A2:M2").Merge();
        sht.Range("A2:M2").Style.Font.FontSize = 10;
        sht.Range("A2:M2").Style.Font.Bold = true;
        sht.Range("A2:M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A2:M2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("A2:M2").Style.Alignment.WrapText = true;
        sht.Range("A2").SetValue(DS.Tables[0].Rows[0]["Compaddr1"]);

        sht.Row(1).Height = 24;
        sht.Range("A3:M3").Merge();
        sht.Range("A3:M3").Style.Font.FontSize = 10;
        sht.Range("A3:M3").Style.Font.Bold = true;
        sht.Range("A3:M3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A3:M3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("A3:M3").Style.Alignment.WrapText = true;
        sht.Range("A3").SetValue(DS.Tables[0].Rows[0]["gstno"]);

        sht.Row(1).Height = 24;
        sht.Range("A4:M4").Merge();
        sht.Range("A4:M4").Style.Font.FontSize = 10;
        sht.Range("A4:M4").Style.Font.Bold = true;
        sht.Range("A4:M4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A4:M4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        sht.Range("A4:M4").Style.Alignment.WrapText = true;
        sht.Range("A4").SetValue("STOCK REPORT FROM : " + txtfromdate.Text + " To : " + txttodate.Text + "");

        sht.Range("A5:M5").Style.Font.FontSize = 10;
        sht.Range("A5:M5").Style.Font.Bold = true;

        sht.Range("A5").Value = "Category Name";
        sht.Range("B5").Value = "Item Name";
        sht.Range("C5").Value = "Quality";
        sht.Range("D5").Value = "Design";
        sht.Range("E5").Value = "Color";
        sht.Range("F5").Value = "Size";
        sht.Range("G5").Value = "Shade Color";

        sht.Range("H5").Value = "LotNo";

        sht.Range("I5").Value = "Unit";
        sht.Range("J5").Value = "Opening Stock";
        sht.Range("K5").Value = "Inw.Qty";
        sht.Range("L5").Value = "Out.Qty";
        sht.Range("M5").Value = "Clos.Bal";

        int row = 6;

        for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
        {
            sht.Range("A" + row).SetValue(DS.Tables[0].Rows[i]["CATEGORY_NAME"]);
            sht.Range("B" + row).SetValue(DS.Tables[0].Rows[i]["Item_Name"]);
            sht.Range("C" + row).SetValue(DS.Tables[0].Rows[i]["QualityName"]);
            sht.Range("D" + row).SetValue(DS.Tables[0].Rows[i]["designName"]);
            sht.Range("E" + row).SetValue(DS.Tables[0].Rows[i]["ColorName"]);
            sht.Range("F" + row).SetValue(DS.Tables[0].Rows[i]["sizemtr"]);
            sht.Range("G" + row).SetValue(DS.Tables[0].Rows[i]["ShadeColorName"]);
            sht.Range("H" + row).SetValue(DS.Tables[0].Rows[i]["Lotno"]);

            sht.Range("I" + row).SetValue(DS.Tables[0].Rows[i]["unitName"]);
            sht.Range("J" + row).SetValue(DS.Tables[0].Rows[i]["Opening"]);
            sht.Range("K" + row).SetValue(DS.Tables[0].Rows[i]["InwardQty"]);
            sht.Range("L" + row).SetValue(DS.Tables[0].Rows[i]["OutwardQty"]);
            decimal ClsBal = Convert.ToDecimal(DS.Tables[0].Rows[i]["Opening"]) + Convert.ToDecimal(DS.Tables[0].Rows[i]["InwardQty"]) - Convert.ToDecimal(DS.Tables[0].Rows[i]["OutwardQty"]);
            sht.Range("M" + row).SetValue(ClsBal);

            row = row + 1;
        }

        sht.Range("H" + row).Value = "Total";
        sht.Range("J" + row).FormulaA1 = "=SUM(J6" + ":$J$" + (row - 1) + ")";
        sht.Range("K" + row).FormulaA1 = "=SUM(K6" + ":$K$" + (row - 1) + ")";
        sht.Range("L" + row).FormulaA1 = "=SUM(L6" + ":$L$" + (row - 1) + ")";
        sht.Range("M" + row).FormulaA1 = "=SUM(M6" + ":$M$" + (row - 1) + ")";
        sht.Range("H" + row + ":M" + row).Style.Font.SetBold();

        DS.Dispose();
        using (var a = sht.Range("A1" + ":M" + row))
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
        string filename = UtilityModule.validateFilename("RawMaterialOpeningClosingReportWithLotNo_" + DateTime.Now + "." + Fileextension);
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


    public void FinishedStockWithAreaExcelReport()
    {
        lblMessage.Text = "";
        //DataSet DS = new DataSet();
        String sQry = " ", Size = "Sizeft", Path = "", Area = "Actualfullareasqyd";
        if (chkformtr.Checked == true)
        {
            Size = "Sizemtr";
            Area = "AreaMtr";
        }
        sQry = @"SELECT isnull(c.CustomerCode,'Direct Stock') AS BuyersCode,V.ITEM_NAME,v.Designname AS DesignName,v.QualityName AS Quality,v.ColorName AS Color,v." + Size + @" AS Size, 
                count(cr.Item_finished_ID) AS StockQTY,round(Cast(v." + Area + @" as decimal(28,3)),2,3) AS Area,count(cr.Item_finished_ID)*round(Cast(v." + Area + @" as decimal(28,3)),2,3) as TotalArea,
                CI.companyname,ci.compaddr1,ci.gstno,isnull(cr.DirectStockRemark,'') as DirectStockRemark                
                from carpetnumber cr inner join companyinfo ci on cr.companyid=ci.companyid left join ordermaster o on cr.orderid=o.orderid left join customerinfo c on 
                o.customerid= c.customerid left join v_finishedItemDetail v on cr.Item_Finished_Id = v.Item_Finished_ID WHERE cr.companyid=" + DDCompany.SelectedValue;
        if (chkallstockno.Checked == false)
        {
            sQry = sQry + " and CR.PACK=0";
        }
        if (chkunconfirmcarpet.Checked == true)
        {
            sQry = sQry + " and isnull(CR.Confirm,0)=0";
        }
        if (DDcustomer.SelectedIndex > 0)
        {
            sQry = sQry + " AND o.customerid= " + DDcustomer.SelectedValue;
        }
        if (DDOrder.SelectedIndex > 0)
        {
            sQry = sQry + " AND cr.orderid= " + DDOrder.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.QUALITYID = " + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.DESIGNID = " + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.COLORID = " + DDColor.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SHADECOLORID = " + DDSize.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SHAPEID = " + DDShape.SelectedValue;

        }
        if (DDSize.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SIZEID = " + DDSize.SelectedValue;
        }
        sQry = sQry + @" group by CI.companyname,ci.compaddr1,ci.gstno,c.CustomerCode,V.ITEM_NAME,v.Designname,v.QualityName,v.ColorName,v." + Size + ",v." + Area + ", Cr.Item_Finished_ID,cr.pack,CR.Confirm,cr.DirectStockRemark Order By c.CustomerCode";
        
        
        
        //DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand(sQry, con);
        //cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        DataSet DS = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(DS);
        //*************

        con.Close();
        con.Dispose();


        if (DS.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Finished Stock");
            //************

            sht.Range("A1:I1").Merge();
            sht.Range("A1").SetValue("FINISHED STOCK REPORT");
            sht.Range("A1:I1").Style.Font.Bold = true;
            sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //Filter by 
            sht.Row(2).Height = 26.25;
            sht.Range("A2:I2").Merge();
            //sht.Range("A2").SetValue(filterby.TrimStart(','));
            sht.Range("A2").SetValue("");
            sht.Range("A2:I2").Style.Font.Bold = true;
            sht.Range("A2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:I2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:I2").Style.Alignment.WrapText = true;

            ////Headings
            sht.Range("A3:I3").Style.Font.Bold = true;
            sht.Range("H3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("I3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            sht.Range("A3").SetValue("Buyer Code");
            sht.Range("B3").SetValue("");
            sht.Range("C3").SetValue("Item Name");
            sht.Range("D3").SetValue("Quality Name");
            sht.Range("E3").SetValue("Design Name");
            sht.Range("F3").SetValue("Color");
            sht.Range("G3").SetValue("Size");
            //sht.Range("G3").SetValue("Area");
            sht.Range("H3").SetValue("Qty");
            sht.Range("I3").SetValue("TotalArea");

            sht.Column(2).Hide();

            //***************
            int row = 4;
            DataView dv = DS.Tables[0].DefaultView;
            //switch (Session["varcompanyNo"].ToString())
            //{
            //    case "14":
            //        dv.RowFilter = "Qtyinhand>0";
            //        break;
            //    default:
            //        dv.RowFilter = "Qtyinhand<>0";
            //        break;
            //}
            dv.Sort = "BuyersCode,Item_Name,Quality";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());

            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["BuyersCode"]);
                ////sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["BuyerOrderNo"]);
                sht.Range("B" + row).SetValue("");
                
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Item_Name"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Quality"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["DesignName"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Color"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
                //sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Area"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["StockQty"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["TotalArea"]);

                //sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Area"]);
                //sht.Range("G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //sht.Range("G" + row).Style.NumberFormat.Format = "#,##0.000";
                row = row + 1;
            }
            ds1.Dispose();
            DS.Dispose();

            sht.Range("F" + row).Value = "TOTAL";
            sht.Range("F" + row).Style.Font.Bold = true;
            //grand Total:
            var sumArea = sht.Evaluate("SUM(H4:H" + (row - 1) + ")");
            sht.Range("H" + row).Value = sumArea;
            sht.Range("H" + row).Style.Font.Bold = true;
            sht.Range("H" + row).Style.NumberFormat.Format = "#,##0.00";

            var SumQty = sht.Evaluate("SUM(I4:I" + (row - 1) + ")");
            sht.Range("I" + row).Value = SumQty;
            sht.Range("I" + row).Style.Font.Bold = true;
            //sht.Range("I" + row).Style.NumberFormat.Format = "#,##0.00";
            ////**********

            sht.Columns(1, 12).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("FinishedStockReport_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }

        //if (DS.Tables[0].Rows.Count > 0)
        //{
        //    if (Session["varCompanyId"].ToString() == "36")
        //    {
        //        if (ChkExportExcel.Checked == true)
        //        {
        //            FinishedStockExcelExport(DS);
        //        }
        //        else
        //        {
        //            Session["rptFileName"] = "reports/CarpetStockBalanceReportPrasad.rpt";
        //        }
        //    }
        //    else
        //    {
        //        Session["rptFileName"] = "reports/CarpetStockBalanceReport.rpt";
        //    }

        //    Session["GetDataset"] = DS;
        //    Session["dsFileName"] = "~\\ReportSchema\\CarpetStockBalance.xsd";
        //    StringBuilder stb = new StringBuilder();
        //    stb.Append("<script>");
        //    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
        //    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        //}
    }
    protected void RawMaterialStockAsOnDateWiseWithPurchaseRate()
    {
        //int Row;        
        //String sQry = " ";
        //string shadecolor = "";        
        //string FilterBy = "Filter By-" + DDCompany.SelectedItem.Text;

        lblMessage.Text = "";
        string str = "", Path; 
        try
        {
            ////if (RDundyed.Checked == true)
            ////{
            ////    shadecolor = "Undyed";
            ////}
            ////else if (RDDyed.Checked == true)
            ////{
            ////    shadecolor = "Dyed";
            ////}

            //if (TDstockupto.Visible == true)
            //{
            //    str = str + " and St.TranDate<='" + txtstockupto.Text + "'";
            //}
            //if (RDundyed.Checked == true)
            //{
            //    str = str + " and v.Shadecolorname like '%Undyed%'";
            //}
            //else if (RDDyed.Checked == true)
            //{
            //    str = str + " and v.Shadecolorname not like '%Undyed%'";
            //}
            if (DDGudown.SelectedIndex > 0)
            {
                str = str + "AND g.godownid =" + DDGudown.SelectedValue;
            }
            if (DDCompany.SelectedIndex > 0)
            {
                str = str + " AND s.companyid = " + DDCompany.SelectedValue;
            }
            if (DDCategory.SelectedIndex > 0)
            {
                str = str + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " AND v.QUALITYID = " + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " AND v.DESIGNID = " + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " AND v.COLORID = " + DDColor.SelectedValue;
            }
            if (DDShadeColor.SelectedIndex > 0)
            {
                str = str + " AND v.SHADECOLORID = " + DDShadeColor.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " AND v.Sizeid = " + DDSize.SelectedValue;
            }
            if (DDLotNo.SelectedIndex > 0)
            {
                str = str + " AND s.lotno = " + DDLotNo.SelectedValue;
            }

            if (txtLotno.Text != "")
            {
                str = str + " AND s.lotno = '" + txtLotno.Text + "'";
            }
            if (txtTagno.Text != "")
            {
                str = str + " AND s.Tagno = '" + txtTagno.Text + "'";
            }



            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@userid", Session["varuserid"]);
            param[1] = new SqlParameter("@MasterCompanyId", Session["varCompanyId"]);
            param[2] = new SqlParameter("@where", str);
            param[3] = new SqlParameter("@StockUpToDate", txtstockupto.Text);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetRawMaterialStockAsOnDateReportWithPurchaseRate", param);

            //DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("Raw Material Stock");
                //************

                sht.Range("A1:K1").Merge();
                sht.Range("A1").SetValue("STOCK REPORT");
                sht.Range("A1:K1").Style.Font.Bold = true;
                sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //Filter by 
                sht.Row(2).Height = 26.25;
                sht.Range("A2:K2").Merge();
                ////sht.Range("A2").SetValue(filterby.TrimStart(','));
                sht.Range("A2").SetValue("");
                sht.Range("A2:K2").Style.Font.Bold = true;
                sht.Range("A2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:K2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A2:K2").Style.Alignment.WrapText = true;
                //Headings
                sht.Range("A3:K3").Style.Font.Bold = true;
                sht.Range("H3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("K3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A3").SetValue("Item Name");
                sht.Range("B3").SetValue("Quality");
                sht.Range("C3").SetValue("Description");
                sht.Range("D3").SetValue("Godown");
                sht.Range("E3").SetValue("Stock Qty(Kgs.)");
                sht.Range("F3").SetValue("Rate");
                sht.Range("G3").SetValue("Amount");
                //sht.Range("H3").SetValue("Stock Qty(Kgs.)");
                //if (chkwithpurchasedetail.Checked == true)
                //{
                //    sht.Range("I3").SetValue("Vendor Name");
                //    sht.Range("J3").SetValue("Vendor Lot No");
                //    sht.Range("K3").SetValue("Rate");
                //}
                ////***************
                int row = 4;
                DataView dv = ds.Tables[0].DefaultView;
                switch (Session["varcompanyNo"].ToString())
                {
                    case "14":
                        dv.RowFilter = "Qtyinhand>0";
                        break;
                    default:
                        dv.RowFilter = "Qtyinhand<>0";
                        break;
                }
                dv.Sort = "Item_Name,Qualityname,Description,GodownName";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());

                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Item_Name"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Description"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["godownname"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Qtyinhand"]);
                    sht.Range("E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("E" + row).Style.NumberFormat.Format = "#,##0.000";
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);
                    sht.Range("G" + row).SetValue(Math.Round(Convert.ToDouble(ds1.Tables[0].Rows[i]["Qtyinhand"]) * Convert.ToDouble(ds1.Tables[0].Rows[i]["Rate"]), 2));

              
                    //sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["godownname"]);
                    //sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Qtyinhand"]);
                    //sht.Range("H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //sht.Range("H" + row).Style.NumberFormat.Format = "#,##0.000";
                    //sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Empname"]);
                    //sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["vendorlotno"]);
                    //sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Rate"]);

                    row = row + 1;
                }
                ds1.Dispose();
                ds.Dispose();
                //grand Total:
                var sum = sht.Evaluate("SUM(E4:E" + (row - 1) + ")");
                sht.Range("E" + row).Value = sum;
                sht.Range("E" + row).Style.Font.Bold = true;
                sht.Range("E" + row).Style.NumberFormat.Format = "#,##0.000";
                //**********
               
                var sumAmt = sht.Evaluate("SUM(G4:G" + (row - 1) + ")");
                sht.Range("G" + row).Value = sumAmt;
                sht.Range("G" + row).Style.Font.Bold = true;
                sht.Range("G" + row).Style.NumberFormat.Format = "#,##0.000";

                sht.Columns(1, 11).AdjustToContents();
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("StockReportNew_" + DateTime.Now + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
            }
            
            
            
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    Session["rptFileName"] = "Reports/RptRawMaterialStockAsOnDateWise.rpt";
            //    Session["dsFileName"] = "~\\ReportSchema\\RptRawMaterialStockAsOnDateWise.xsd";
            //    Session["GetDataset"] = ds;
            //    StringBuilder stb = new StringBuilder();
            //    stb.Append("<script>");
            //    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            //    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
            //}

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }

    public void FinishedStockWithBuyerOrderNoAreaExcelReport()
    {
        lblMessage.Text = "";
        //DataSet DS = new DataSet();
        String sQry = " ", Size = "Sizeft", Path = "", Area = "Actualfullareasqyd";
        if (chkformtr.Checked == true)
        {
            Size = "Sizemtr";
            Area = "AreaMtr";
        }
        sQry = @"SELECT isnull(c.CustomerCode,'Direct Stock') AS BuyersCode,V.ITEM_NAME,v.Designname AS DesignName,v.QualityName AS Quality,v.ColorName AS Color,v." + Size + @" AS Size, 
                count(cr.Item_finished_ID) AS StockQTY,round(Cast(v." + Area + @" as decimal(28,3)),2,3) AS Area,count(cr.Item_finished_ID)*round(Cast(v." + Area + @" as decimal(28,3)),2,3) as TotalArea,
                CI.companyname,ci.compaddr1,ci.gstno,isnull(cr.DirectStockRemark,'') as DirectStockRemark,o.CustomerOrderNo AS BuyerOrderNo                
                from carpetnumber cr inner join companyinfo ci on cr.companyid=ci.companyid left join ordermaster o on cr.orderid=o.orderid left join customerinfo c on 
                o.customerid= c.customerid left join v_finishedItemDetail v on cr.Item_Finished_Id = v.Item_Finished_ID WHERE cr.companyid=" + DDCompany.SelectedValue;
        if (chkallstockno.Checked == false)
        {
            sQry = sQry + " and CR.PACK=0";
        }
        if (chkunconfirmcarpet.Checked == true)
        {
            sQry = sQry + " and isnull(CR.Confirm,0)=0";
        }
        if (DDcustomer.SelectedIndex > 0)
        {
            sQry = sQry + " AND o.customerid= " + DDcustomer.SelectedValue;
        }
        if (DDOrder.SelectedIndex > 0)
        {
            sQry = sQry + " AND cr.orderid= " + DDOrder.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.QUALITYID = " + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.DESIGNID = " + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.COLORID = " + DDColor.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SHADECOLORID = " + DDSize.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SHAPEID = " + DDShape.SelectedValue;

        }
        if (DDSize.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SIZEID = " + DDSize.SelectedValue;
        }
        sQry = sQry + @" group by CI.companyname,ci.compaddr1,ci.gstno,c.CustomerCode,V.ITEM_NAME,v.Designname,v.QualityName,v.ColorName,v." + Size + ",v." + Area + ", Cr.Item_Finished_ID,cr.pack,CR.Confirm,cr.DirectStockRemark,o.CustomerOrderNo Order By c.CustomerCode";



        //DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand(sQry, con);
        //cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        DataSet DS = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(DS);
        //*************

        con.Close();
        con.Dispose();


        if (DS.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Finished Stock");
            //************

            sht.Range("A1:I1").Merge();
            sht.Range("A1").SetValue("FINISHED STOCK REPORT");
            sht.Range("A1:I1").Style.Font.Bold = true;
            sht.Range("A1:I1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //Filter by 
            sht.Row(2).Height = 26.25;
            sht.Range("A2:I2").Merge();
            //sht.Range("A2").SetValue(filterby.TrimStart(','));
            sht.Range("A2").SetValue("");
            sht.Range("A2:I2").Style.Font.Bold = true;
            sht.Range("A2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:I2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:I2").Style.Alignment.WrapText = true;

            ////Headings
            sht.Range("A3:I3").Style.Font.Bold = true;
            sht.Range("H3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("I3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            sht.Range("A3").SetValue("Buyer Code");
            sht.Range("B3").SetValue("Buyer OrderNo");
            sht.Range("C3").SetValue("Item Name");
            sht.Range("D3").SetValue("Quality Name");
            sht.Range("E3").SetValue("Design Name");
            sht.Range("F3").SetValue("Color");
            sht.Range("G3").SetValue("Size");
            //sht.Range("G3").SetValue("Area");
            sht.Range("H3").SetValue("Qty");
            sht.Range("I3").SetValue("TotalArea");

            //sht.Column(2).Hide();

            //***************
            int row = 4;
            DataView dv = DS.Tables[0].DefaultView;
            //switch (Session["varcompanyNo"].ToString())
            //{
            //    case "14":
            //        dv.RowFilter = "Qtyinhand>0";
            //        break;
            //    default:
            //        dv.RowFilter = "Qtyinhand<>0";
            //        break;
            //}
            dv.Sort = "BuyersCode,Item_Name,Quality";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());

            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["BuyersCode"]);
                ////sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["BuyerOrderNo"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["BuyerOrderNo"]);

                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Item_Name"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Quality"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["DesignName"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Color"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
                //sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Area"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["StockQty"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["TotalArea"]);

                //sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Area"]);
                //sht.Range("G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //sht.Range("G" + row).Style.NumberFormat.Format = "#,##0.000";
                row = row + 1;
            }
            ds1.Dispose();
            DS.Dispose();

            sht.Range("F" + row).Value = "TOTAL";
            sht.Range("F" + row).Style.Font.Bold = true;
            //grand Total:
            var sumArea = sht.Evaluate("SUM(H4:H" + (row - 1) + ")");
            sht.Range("H" + row).Value = sumArea;
            sht.Range("H" + row).Style.Font.Bold = true;
            sht.Range("H" + row).Style.NumberFormat.Format = "#,##0.00";

            var SumQty = sht.Evaluate("SUM(I4:I" + (row - 1) + ")");
            sht.Range("I" + row).Value = SumQty;
            sht.Range("I" + row).Style.Font.Bold = true;
            //sht.Range("I" + row).Style.NumberFormat.Format = "#,##0.00";
            ////**********

            sht.Columns(1, 12).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("FinishedStockWithBuyerOrderNoReport_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }
       
    }

    protected void RawMaterialStockAsOnDateWiseExcelReport()
    {
        DataSet ds = new DataSet();
        string sQry = "", filterby = "";
        if (DDGudown.SelectedIndex > 0)
        {
            sQry = sQry + "AND g.godownid =" + DDGudown.SelectedValue;
        }
        if (DDCompany.SelectedIndex > 0)
        {
            sQry = sQry + " AND s.companyid = " + DDCompany.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.QUALITYID = " + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.DESIGNID = " + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.COLORID = " + DDColor.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SHADECOLORID = " + DDShadeColor.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.Sizeid = " + DDSize.SelectedValue;
        }
        if (DDLotNo.SelectedIndex > 0)
        {
            sQry = sQry + " AND s.lotno = " + DDLotNo.SelectedValue;
        }
        if (txtLotno.Text != "")
        {
            sQry = sQry + " AND s.lotno = '" + txtLotno.Text + "'";
        }
        if (txtTagno.Text != "")
        {
            sQry = sQry + " AND s.Tagno = '" + txtTagno.Text + "'";
        }

        //SqlParameter[] param = new SqlParameter[4];
        //param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        //param[1] = new SqlParameter("@FromDate", txtfromdate.Text);
        //param[2] = new SqlParameter("@ToDate", txttodate.Text);
        //param[3] = new SqlParameter("@Where", sQry);

        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETSHADEWISESTOCKDiamondExport", param);


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_GetRawMaterialStockAsOnDateReport", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
        cmd.Parameters.AddWithValue("@where", sQry);
        cmd.Parameters.AddWithValue("@StockUpToDate", txtstockupto.Text);     

        // DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();
        //***********     
        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Raw Material Stock");
            //************

            sht.Range("A1:G1").Merge();
            sht.Range("A1").SetValue("STOCK REPORT");
            sht.Range("A1:G1").Style.Font.Bold = true;
            sht.Range("A1:G1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //Filter by 
            sht.Range("A2:G2").Merge();
            sht.Range("A2").SetValue("Stock Up To : " + txtstockupto.Text);
            sht.Range("A2:G2").Style.Font.Bold = true;
            sht.Range("A2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:G2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:G2").Style.Alignment.WrapText = true;
            //Headings
            sht.Range("A3:G3").Style.Font.Bold = true;
            sht.Range("D3:G3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3").SetValue("ITEM NAME");
            sht.Range("B3").SetValue("QUALITY");
            sht.Range("C3").SetValue("DESCRIPTION");
            sht.Range("D3").SetValue("LOTNO");
            sht.Range("E3").SetValue("TAGNO");
            sht.Range("F3").SetValue("GODOWN");
            sht.Range("G3").SetValue("STOCK QTY");
            //sht.Range("H3").SetValue("RECEIVE QTY (KG)");
            //sht.Range("I3").SetValue("CLOSING STOCK (KG)");
            using (var a = sht.Range("A3:G3"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //***************
            int row = 4;
            DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "Item_Name", "QualityName");
            foreach (DataRow dr in dtdistinct.Rows)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "Item_Name='" + dr["Item_Name"] + "' and QualityName='" + dr["QualityName"] + "' and QtyinHand<>0";
                dv.Sort = "Item_name,QualityName";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());

                //sht.Range("A" + row).Value = dr["Item_name"];
                //sht.Range("B" + row).Value = dr["QualityName"];
                //using (var a = sht.Range("A" + row + ":E" + row))
                //{
                //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //}
                ////row = row + 1;
                decimal SumQtyinhand = 0;
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    using (var a = sht.Range("A" + row + ":G" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Item_name"].ToString());
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["QualityName"].ToString());
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Description"].ToString());
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["LotNo"].ToString());
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["TagNo"].ToString());
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["GodownName"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["QtyinHand"]);
                    //sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["ReceiveQTY"]);
                    //sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["CurrentstockQty"]);

                    SumQtyinhand = SumQtyinhand + Convert.ToDecimal(ds1.Tables[0].Rows[i]["QtyinHand"]);
                    row = row + 1;
                }
                               
                sht.Range("F" + row).Value = "Quality Wise Total";
                sht.Range("F" + row).Style.Font.Bold = true;
                ////grand Total:
               //// var sumArea = sht.Evaluate("SUM(H4:H" + (row - 1) + ")");
                sht.Range("G" + row).Value = SumQtyinhand;
                sht.Range("G" + row).Style.Font.Bold = true;
                sht.Range("G" + row).Style.NumberFormat.Format = "#,##0.00";

                row = row + 1;
            }
            DS.Dispose();

            sht.Columns(1, 12).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("RawMaterialStockAsOnDateWiseExcelReport_" + DateTime.Now + "." + Fileextension);
            string Path = "";
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "altshade", "alert('No records found for this combination.')", true);
        }
    }

    public void FinishedStockWithoutStockNo()
    {
        lblMessage.Text = "";
        DataSet DS = new DataSet();
        String sQry = " ", Size = "Sizeft";
        if (chkformtr.Checked == true)
        {
            Size = "Sizemtr";
        }
        sQry = @"SELECT isnull(c.CustomerCode,'Direct Stock') AS BuyersCode,o.LocalOrder AS BuyerOrderNo,v.Designname AS DesignName,v.QualityName AS Quality,v.ColorName AS Color,v." + Size + @" AS Size, 
                count(cr.Item_finished_ID) AS StockQTY,'' as StockNo,
                --dbo.F_GetFinishedstock_orderwise(Cr.item_finished_id,cr.orderid,cr.pack,CR.Confirm,isnull(cr.DirectStockRemark,'')) as StockNo,
                CI.companyname,ci.compaddr1,ci.gstno,
                isnull(cr.DirectStockRemark,'') as DirectStockRemark                
                from carpetnumber cr inner join companyinfo ci on cr.companyid=ci.companyid left join ordermaster o on cr.orderid=o.orderid left join customerinfo c on 
                o.customerid= c.customerid left join v_finishedItemDetail v on cr.Item_Finished_Id = v.Item_Finished_ID WHERE cr.companyid=" + DDCompany.SelectedValue;
        if (chkallstockno.Checked == false)
        {
            sQry = sQry + " and CR.PACK=0";
        }
        if (chkunconfirmcarpet.Checked == true)
        {
            sQry = sQry + " and isnull(CR.Confirm,0)=0";
        }
        if (DDcustomer.SelectedIndex > 0)
        {
            sQry = sQry + " AND o.customerid= " + DDcustomer.SelectedValue;
        }
        if (DDOrder.SelectedIndex > 0)
        {
            sQry = sQry + " AND cr.orderid= " + DDOrder.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.QUALITYID = " + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.DESIGNID = " + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.COLORID = " + DDColor.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SHADECOLORID = " + DDSize.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SHAPEID = " + DDShape.SelectedValue;

        }
        if (DDSize.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SIZEID = " + DDSize.SelectedValue;
        }
        sQry = sQry + @" group by CI.companyname,ci.compaddr1,ci.gstno,c.CustomerCode,o.LocalOrder,v.Designname,v.QualityName,v.ColorName,v." + Size + @", Cr.Item_Finished_ID,
                        cr.DirectStockRemark Order By c.CustomerCode";
        DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);
        if (DS.Tables[0].Rows.Count > 0)
        {
           
            Session["rptFileName"] = "reports/CarpetStockBalanceReportWithoutStockNo.rpt";           

            Session["GetDataset"] = DS;
            Session["dsFileName"] = "~\\ReportSchema\\CarpetStockBalance.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }

    public void FinishedStockProcessWiseWithBuyerOrderNoAreaExcelReport()
    {
        lblMessage.Text = "";
        //DataSet DS = new DataSet();
        String sQry = " ", Size = "Sizeft", Path = "", Area = "Actualfullareasqyd";
        if (chkformtr.Checked == true)
        {
            Size = "Sizemtr";
            Area = "AreaMtr";
        }
        sQry = @"SELECT isnull(c.CustomerCode,'Direct Stock') AS BuyersCode,V.ITEM_NAME,v.Designname AS DesignName,v.QualityName AS Quality,v.ColorName AS Color,v." + Size + @" AS Size, 
                count(cr.Item_finished_ID) AS StockQTY,round(Cast(v." + Area + @" as decimal(28,3)),2,3) AS Area,count(cr.Item_finished_ID)*round(Cast(v." + Area + @" as decimal(28,3)),2,3) as TotalArea,
                CI.companyname,ci.compaddr1,ci.gstno,isnull(cr.DirectStockRemark,'') as DirectStockRemark,o.CustomerOrderNo AS BuyerOrderNo,V.ShapeName                
                from carpetnumber cr(NoLock) Join Process_Stock_Detail PSD(NoLock) On cr.StockNo=PSD.StockNo 
                inner join companyinfo ci(NoLock) on cr.companyid=ci.companyid 
                left join ordermaster o(NoLock) on cr.orderid=o.orderid 
                left join customerinfo c(NoLock) on o.customerid= c.customerid 
                left join v_finishedItemDetail v(NoLock) on cr.Item_Finished_Id = v.Item_Finished_ID WHERE isnull(PSD.ReceiveDetailId,0)>0 and cr.companyid=" + DDCompany.SelectedValue;
        if (chkallstockno.Checked == false)
        {
            sQry = sQry + " and CR.PACK=0";
        }
        if (chkunconfirmcarpet.Checked == true)
        {
            sQry = sQry + " and isnull(CR.Confirm,0)=0";
        }
        if (DDcustomer.SelectedIndex > 0)
        {
            sQry = sQry + " AND o.customerid= " + DDcustomer.SelectedValue;
        }
        if (DDOrder.SelectedIndex > 0)
        {
            sQry = sQry + " AND cr.orderid= " + DDOrder.SelectedValue;
        }
        if (DDProcessName.SelectedIndex > 0)
        {
            sQry = sQry + " AND PSD.ToProcessId= " + DDProcessName.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.CATEGORY_ID = " + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.ITEM_ID = " + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.QUALITYID = " + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.DESIGNID = " + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.COLORID = " + DDColor.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SHADECOLORID = " + DDSize.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SHAPEID = " + DDShape.SelectedValue;

        }
        if (DDSize.SelectedIndex > 0)
        {
            sQry = sQry + " AND v.SIZEID = " + DDSize.SelectedValue;
        }
        sQry = sQry + @" group by CI.companyname,ci.compaddr1,ci.gstno,c.CustomerCode,V.ITEM_NAME,v.Designname,v.QualityName,v.ColorName,v." + Size + ",v." + Area + ", Cr.Item_Finished_ID,cr.pack,CR.Confirm,cr.DirectStockRemark,o.CustomerOrderNo,V.ShapeName Order By c.CustomerCode";



        //DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand(sQry, con);
        //cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        DataSet DS = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(DS);
        //*************

        con.Close();
        con.Dispose();


        if (DS.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Finished Stock");
            //************

            sht.Range("A1:J1").Merge();
            sht.Range("A1").SetValue("FINISHED STOCK REPORT");
            sht.Range("A1:J1").Style.Font.Bold = true;
            sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //Filter by 
            sht.Row(2).Height = 26.25;
            sht.Range("A2:J2").Merge();
            //sht.Range("A2").SetValue(filterby.TrimStart(','));
            sht.Range("A2").SetValue("");
            sht.Range("A2:J2").Style.Font.Bold = true;
            sht.Range("A2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:J2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2:J2").Style.Alignment.WrapText = true;

            ////Headings
            sht.Range("A3:J3").Style.Font.Bold = true;
            sht.Range("I3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            sht.Range("A3").SetValue("Buyer Code");
            sht.Range("B3").SetValue("Buyer OrderNo");
            sht.Range("C3").SetValue("Item Name");
            sht.Range("D3").SetValue("Quality Name");
            sht.Range("E3").SetValue("Design Name");
            sht.Range("F3").SetValue("Color");
            sht.Range("G3").SetValue("Size");
            //sht.Range("G3").SetValue("Area");
            sht.Range("H3").SetValue("Shape");
            sht.Range("I3").SetValue("Qty");
            sht.Range("J3").SetValue("TotalArea");

            //sht.Column(2).Hide();

            //***************
            int row = 4;
            DataView dv = DS.Tables[0].DefaultView;
            //switch (Session["varcompanyNo"].ToString())
            //{
            //    case "14":
            //        dv.RowFilter = "Qtyinhand>0";
            //        break;
            //    default:
            //        dv.RowFilter = "Qtyinhand<>0";
            //        break;
            //}
            dv.Sort = "BuyersCode,Item_Name,Quality";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());

            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["BuyersCode"]);
                ////sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["BuyerOrderNo"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["BuyerOrderNo"]);

                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Item_Name"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Quality"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["DesignName"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Color"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
                //sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Area"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["ShapeName"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["StockQty"]);
                sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["TotalArea"]);

                //sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Area"]);
                //sht.Range("G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //sht.Range("G" + row).Style.NumberFormat.Format = "#,##0.000";
                row = row + 1;
            }
            ds1.Dispose();
            DS.Dispose();

            sht.Range("F" + row).Value = "TOTAL";
            sht.Range("F" + row).Style.Font.Bold = true;
            //grand Total:
            var sumArea = sht.Evaluate("SUM(I4:I" + (row - 1) + ")");
            sht.Range("I" + row).Value = sumArea;
            sht.Range("I" + row).Style.Font.Bold = true;
            sht.Range("I" + row).Style.NumberFormat.Format = "#,##0.00";

            var SumQty = sht.Evaluate("SUM(J4:J" + (row - 1) + ")");
            sht.Range("J" + row).Value = SumQty;
            sht.Range("J" + row).Style.Font.Bold = true;
            //sht.Range("I" + row).Style.NumberFormat.Format = "#,##0.00";
            ////**********

            sht.Columns(1, 12).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("FinishedStockProcessWiseWithBuyerOrderNoReport_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }

    }
}

