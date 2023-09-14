using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using ClosedXML.Excel;
using AjaxControlToolkit.HTMLEditor.ToolbarButton;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Net;
using System.Web.Services.Description;
//using DocumentFormat.OpenXml.Drawing.Charts;

public partial class Masters_ReportForms_frmorderReportsnew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varCompanyId"] + @" Order by Companyname 
                           select Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=CS.Categoryid and Cs.id=0 order by CATEGORY_NAME
                           select Unitid,Unitname From Unit WHere Unitid in(1,2)
                           SELECT PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME FROM PROCESS_NAME_MASTER PNM INNER JOIN USERRIGHTSPROCESS URP ON PNM.PROCESS_NAME_ID=URP.PROCESSID WHERE URP.USERID=" + Session["varuserId"] + " and PNm.Process_name_id<>1 order by PROCESS_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizeunit, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDjobname, ds, 3, true, "--Plz Select--");

            string str2 = @"Select OrderCategoryId,OrderCategory From OrderCategory";

            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);

            UtilityModule.ConditionalComboFillWithDS(ref DDOrderType, ds2, 0, true, "--ALL--");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                CompanySelectedChange();
            }

            if (DDsizeunit.Items.FindByValue(variable.VarDefaultProductionunit) != null)
            {
                DDsizeunit.SelectedValue = variable.VarDefaultProductionunit;
            }
            RDOrderConsumption.Checked = true;
            //***********Month Year
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (variable.VarTAGGINGWITHINTERNALPRODUCTION == "1")
            {
                TDinternalfoliodetail.Visible = true;
            }

            if (Session["varcompanyId"].ToString() == "9")
            {
                TDProcessWiseReport.Visible = true;
                TDBazaarCompleteStatus.Visible = true;
                TROrderConsumptionWithIndentIssRec.Visible = true;
                TROrderConsumptionSummaryWithWeaverIssRec.Visible = true;
            }
            if (Session["varcompanyId"].ToString() == "16" || Session["varcompanyId"].ToString() == "28")
            {
                TROrderSummaryWithAllProcess.Visible = true;
                TROrderDispatchSummary.Visible = true;
                TROrderDetailWIP.Visible = true;
                UtilityModule.ConditionalComboFill(ref DDEmployeeName, @"Select Distinct EI.EmpID, EI.EmpName 
                        From Empinfo EI
                        JOIN PROCESS_ISSUE_Master_1 PIM ON PIM.EmpID = EI.EmpID 
                        Order By EI.EmpName ", true, "--Select--");
            }
            if (Session["varcompanyId"].ToString() == "40")
            {
                TROrderConsumptionRecMaterialPending.Visible = true;
            }
            if (Session["VarCompanyNo"].ToString() == "30")
            {
                TROrderShippedInvoiceWiseDetail.Visible = true;
            }
            if (Session["VarCompanyNo"].ToString() == "44")
            {
                RDOrderConsumption.Visible = true;
                RDCancelPo.Visible = false;
                RDVendorWiseDetail.Visible = false;
                RDProductionSummary.Visible = false;
                RDOrderstatus.Visible = false;
                
                RDFinishingStatus.Visible = false;
                RDCustomeropenorderstatus.Visible = true;
                RDFolioWiseDetail.Visible = false;
                RDDesignWiseConsmpDetail.Visible = false;
                RDProductionReport.Visible = false;
                RDPOSTATUS.Visible = true;
                RDPOSTATUSAGNI.Visible = true;
                RDPOSTATUSInHouse.Visible = false;
                RDPOSTATUSOutSide.Visible = false;
                TDinternalfoliodetail.Visible = false;
                TDProcessWiseReport.Visible = false;
                TD1.Visible = false;
                TrProcessname.Visible = true;
                TDCustomerOrderInvoiceStatus.Visible = false;
                TRProcessDetails.Visible = true;
                RDCustomerOrderInternalOC.Visible = false;


                RDOrderConsumption.Text = "PO CONSUMPTION";
                RDPOSTATUS.Text = "ORDER DETAIL";
                RDPOSTATUSAGNI.Text = "PO STATUS";
            }
            else
            {
                RDOrderConsumption.Text = "Order Consumption Detail";
            }
            
            if (Session["VarCompanyNo"].ToString() == "38")
            {
                //RDVendorWiseDetail.Visible = false;
                //RDProductionSummary.Visible = false;
                //RDFinishingStatus.Visible = false;
                //RDCustomeropenorderstatus.Visible = false;
                //RDFolioWiseDetail.Visible = false;
                //RDPOSTATUSInHouse.Visible = false;
                //RDPOSTATUSOutSide.Visible = false;
                TDinternalfoliodetail.Visible = false;
                //TD1.Visible = false;
            }


            if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
            {
                TRCustomerCode.Visible = false;
            }
        }
    }
    private void CompanySelectedChange()
    {
        string str = string.Empty;
        //if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
        //{
        //    string str = @"Select OrderId,LocalOrder+ ' / ' +CustomerOrderNo From OrderMaster where CompanyId=" + DDCompany.SelectedValue + " Order By CustomerOrderNo";
        //    if (Session["varCompanyId"].ToString() == "16")
        //    {
                str = @"Select OrderId, CustomerOrderNo From OrderMaster where CompanyId=" + DDCompany.SelectedValue + " Order By CustomerOrderNo";
        //    }
            UtilityModule.ConditionalComboFill(ref DDOrderNo, str, true, "--Select--");
        //}
        //UtilityModule.ConditionalComboFill(ref DDCustCode, "Select CustomerId,CustomerCode From CustomerInfo Where MasterCompanyId=" + Session["varCompanyId"] + " Order By CustomerCode", true, "--Select--");
    }
    protected void DDCustCode_SelectedIndexChanged(object sender, EventArgs e)
    {
//        string Str = @"Select OrderId, case when " + variable.VarORDERNODROPDOWNWITHLOCALORDER + @" = 1 THen LocalOrder + ' / ' + CustomerOrderNo Else customerorderno End OrderNo 
//            From OrderMaster 
//            Where CustomerId = " + DDCustCode.SelectedValue + " And CompanyId = " + DDCompany.SelectedValue;
//        if (DDorderstatus.SelectedIndex > 0)
//        {
//            Str = Str + " And Status = " + DDorderstatus.SelectedValue;
//        }
//        Str = Str + " Order By OrderNo";

//        UtilityModule.ConditionalComboFill(ref DDOrderNo, Str, true, "--Select--");
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        if (RDOrderConsumption.Checked == true)
        {
            if (variable.Carpetcompany == "1")
            {
                if (Session["varCompanyNo"].ToString() == "44")
                {
                    orderconsumptiondetail_Carpetcompany_agni();
                }
                else
                {
                    orderconsumptiondetail_Carpetcompany();
                }

                //orderconsumptiondetail_Carpetcompany();
            }
            else
            {
                orderconsumptiondetail();
            }
        }
        else if (RDVendorWiseDetail.Checked == true)
        {
            if (Session["varcompanyId"].ToString() == "9")
            {
                if (chkpaidunpaid.Checked == true)
                {
                    PaidunpaidDetailHafizia();
                }
                else
                {
                    Vendorwisedetail();
                }
            }
            else
            {
                if (chkpaidunpaid.Checked == true)
                {
                    PaidunpaidDetail();
                }
                else
                {
                    Vendorwisedetail();
                }
            }

        }
        else if (RDProductionSummary.Checked == true)
        {
            Productionsummary();
        }
        else if (RDCancelPo.Checked == true)
        {
            cancelpodetail();
        }
        else if (RDOrderstatus.Checked == true)
        {
            Orderstatus();
        }
        else if (RDCustomerOrderInvoiceStatus.Checked == true)
        {
            CustomerOrderInvoiceStatus();
        }
        else if (RDFinishingStatus.Checked == true)
        {
            FinishingStatus();
        }
        else if (RDCustomeropenorderstatus.Checked == true)
        {
            if (Session["varCompanyNo"].ToString() == "44")
            {
                Customeropenorderstatus_AGNI();
               
            }
            else
            {

                Customeropenorderstatus();
            }
        }
        else if (RDFolioWiseDetail.Checked == true)
        {
            if (ChkForFolioWiseConsReporType2.Checked == true)
            {
                FolioWiseConsumptionDetailReportType2();
            }
            else if (ChkForFolioWiseConReportType2Summary.Checked == true)
            {
                FolioWiseConsumptionSummaryReportType2();
            }
            else
            {
                FolioWiseDetail();
            }

        }
        else if (RDDesignWiseConsmpDetail.Checked == true)
        {
            DesignWiseConsmpDetail();
        }
        else if (RDProductionReport.Checked == true)
        {
            ProductionReport();
        }
        else if (RDPOSTATUS.Checked == true && ChkWithOrderValue.Checked == true)
        {
            if (Session["varCompanyNo"].ToString() == "30")
            {
                PoStatusWithOrderValue();
            }
            else
            {
                PoStatus();
            }
        }
        else if (RDPOSTATUS.Checked == true || RDPOSTATUSInHouse.Checked == true || RDPOSTATUSOutSide.Checked == true)
        {
            if (Session["varCompanyNo"].ToString() == "4")
            {
                POOrderstatusDeepakRugs();
            }
            else
            {
                if (ChkForPOStatusReportType2.Checked == true)
                {
                    PoStatusReportType2();
                }
                else
                {
                    PoStatus();
                }
            }
        }
        else if (RDPOSTATUSAGNI.Checked == true)
        {

            if (chkforpodetail.Checked)
            {
                PodetailAGNI();
            
            }
            else
            {
                PoStatusAGNI();
            }
        
        }
        else if (RDinternalfoliodetail.Checked == true)
        {
            Interfoliodetail();
        }
        else if (RDProcessWiseReport.Checked == true)
        {
            if (ChkForDate.Checked == true)
            {
                ProcesswisedetailHafiziaAllProcess();
            }
            else if (ChkForRecQty.Checked == true)
            {
                OrderWiseProcessdetailHafiziaRecQty();
            }
            else
            {
                Processwisedetail();
            }

        }
        else if (RDInternalprodstockno.Checked == true)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@orderid", DDOrderNo.SelectedValue);
                param[1] = new SqlParameter("@mastercompanyid", Session["varcompanyid"].ToString());

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETINTERNALPRODSTOCKNO", param);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var xapp = new XLWorkbook();
                    var sht = xapp.Worksheets.Add("sheet1");
                    //************

                    sht.Range("A1:J1").Merge();
                    sht.Range("A1").SetValue("INTERNAL PROD. STOCK NO. DETAIL");
                    sht.Range("A1:J1").Style.Font.Bold = true;
                    sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    sht.Range("A2:J2").Merge();
                    sht.Range("A2").SetValue("CUSTOMER CODE : " + ds.Tables[0].Rows[0]["customercode"].ToString());
                    sht.Range("A2:J2").Style.Font.Bold = true;
                    sht.Range("A2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    sht.Range("A3:J3").Merge();
                    sht.Range("A3").SetValue("ORDER NO : " + ds.Tables[0].Rows[0]["customerorderno"].ToString());
                    sht.Range("A3:J3").Style.Font.Bold = true;
                    sht.Range("A3:J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    //Headings
                    sht.Range("A4:J4").Style.Font.Bold = true;
                    sht.Range("A4").SetValue("Item Name");
                    sht.Range("B4").SetValue("Quality");
                    sht.Range("C4").SetValue("Desingn");
                    sht.Range("D4").SetValue("Color");
                    sht.Range("E4").SetValue("Shape Name");
                    sht.Range("F4").SetValue("Size");
                    sht.Range("G4").SetValue("Stock No.");
                    sht.Range("H4").SetValue("OnLoom Status");
                    sht.Range("I4").SetValue("Bazar Status");
                    sht.Range("J4").SetValue("BUCKET ISSUE");

                    int row = 5;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                        sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Quality"]);
                        sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Design"]);
                        sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["color"]);
                        sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["shapename"]);
                        sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["size"]);
                        sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Tstockno"]);
                        sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["OnLoomStatus"]);
                        sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Bazarstatus"]);
                        sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Materialstatus"]);

                        row = row + 1;
                    }


                    sht.Columns(1, 13).AdjustToContents();
                    string Fileextension = "xlsx";
                    string filename = UtilityModule.validateFilename("InternalTaggingStockno_" + DateTime.Now + "." + Fileextension);
                    String Path = Server.MapPath("~/Tempexcel/" + filename);
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
                    ScriptManager.RegisterStartupScript(Page, GetType(), "altrep", "alert('No records found...');", true);
                }


            }
            catch (Exception ex)
            {

            }
        }
        else if (RDCustomerOrderInternalOC.Checked == true)
        {
            CustomerOrderInternalOCReport();
        }
        else if (RDBazaarCompleteStatus.Checked == true)
        {
            BazaarCompleteStatus();
        }
        else if (RDOrderSummaryWithAllProcess.Checked == true)
        {
            OrderSummaryDataWithAllProcess();
        }
        else if (RDOrderDispatchSummary.Checked == true)
        {
            OrderDispatchSummary();
        }
        else if (RDOrderConsumptionWithIndentIssRec.Checked == true)
        {
            OrderConsumptionWithIndentIssRec();

        }
        else if (RDOrderDetailWIP.Checked == true)
        {
            OrderDetailWIP();
        }
        else if (RDOrderConsumptionSummaryWithWeaverIssRec.Checked == true)
        {
            OrderConsumptionSummaryWithWeaverIssRec();
        }
        else if (RDOrderConsumptionRecMaterialPending.Checked == true)
        {
            OrderConsumptionRecMaterialPendingDetail();
        }
        else if (RDOrderShippedInvoiceWiseDetail.Checked == true)
        {
            OrderShippedWithInvoiceDetail();
        }
        else if (RDProcessDetails.Checked == true)
        {
            OrderProcesswise();
        }
    }
    protected void Productionsummary()
    {
        try
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_GetOrderLagat_Weaver_Dyer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@orderid", DDOrderNo.SelectedValue);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("Image", typeof(System.Byte[]));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DataView dv = new DataView(ds.Tables[2]);
                    dv.RowFilter = "Item_id=" + dr["item_id"] + " ANd QualityId=" + dr["qualityid"] + " and DesignId=" + dr["designid"] + " and colorid=" + dr["colorid"] + "";
                    DataTable dt = dv.ToTable();
                    foreach (DataRow dr2 in dt.Rows)
                    {

                        if (Convert.ToString(dr2["Photo"]) != "")
                        {
                            FileInfo TheFile = new FileInfo(Server.MapPath(dr2["photo"].ToString()));
                            if (TheFile.Exists)
                            {
                                string img = dr2["Photo"].ToString();
                                img = Server.MapPath(img);
                                Byte[] img_Byte = File.ReadAllBytes(img);
                                dr["Image"] = img_Byte;
                                break;
                            }
                        }
                    }
                }

                Session["dsFileName"] = "~\\ReportSchema\\RptorderLagatweaverDyer.xsd";
                Session["rptFileName"] = "Reports/RptorderLagatweaverDyer.rpt";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {

        }

    }

    private void orderconsumptiondetail()
    {
        SqlParameter[] param = new SqlParameter[1];
        param[0] = new SqlParameter("@orderid", DDOrderNo.SelectedValue);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getorderConsumptionDetail", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["GetDataset"] = ds;
            Session["rptFileName"] = "~\\Reports\\RptorderProcessconsumptionDetail.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptorderProcessconsumptionDetail.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
        ds.Dispose();
    }
    private void orderconsumptiondetail_Carpetcompany()
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@orderid", DDOrderNo.SelectedValue);
        param[1] = new SqlParameter("@MasterCompanyId", Session["VarcompanyNo"]);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getorderWeavingConsumption", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["GetDataset"] = ds;
            Session["rptFileName"] = "~\\Reports\\rptorderweavingconsumptiondetail.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\rptorderweavingconsumptiondetail.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
        ds.Dispose();
    }
    private void orderconsumptiondetail_Carpetcompany_agni()
    {
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@orderid", DDOrderNo.SelectedValue);
        param[1] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
        param[2] = new SqlParameter("@jobid", DDjobname.SelectedValue);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_OrderCosting", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["GetDataset"] = ds;
            Session["rptFileName"] = "~\\Reports\\RptOrderPOConsumption.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptOrderCosting.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
        ds.Dispose();
    }
    private void Vendorwisedetail()
    {



        //SqlParameter[] param = new SqlParameter[2];
        //param[0] = new SqlParameter("@localOrderNo", txtlocalOrderNo.Text);
        //param[1] = new SqlParameter("@userid", Session["varuserid"]);

        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_getVendorWiseDetail", param);

        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("pro_getVendorWiseDetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@localOrderNo", txtlocalOrderNo.Text);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);

        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["GetDataset"] = ds;
            if (Session["varcompanyid"].ToString() == "9")
            {
                Session["rptFileName"] = "~\\Reports\\RptvendorwisedetailforHaf.rpt";
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\Rptvendorwisedetail.rpt";
            }
            Session["dsFileName"] = "~\\ReportSchema\\Rptvendorwisedetail.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }
        ds.Dispose();
    }
    private void FolioWiseDetail()
    {
        int Rowcount = 0;
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@companyId", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@processid", 1);
        param[2] = new SqlParameter("orderid", DDOrderNo.SelectedValue);
        param[3] = new SqlParameter("LocalOrder", txtlocalOrderNo.Text);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETFOLIOWISEDETAIL", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("FolioWiseDetail");
            int row = 0;
            //*******************
            sht.Range("A1").Value = "Cust.Code/Order No.";
            sht.Range("B1").Value = "Folio No.";
            sht.Range("C1").Value = "Design/Quality";
            sht.Range("D1").Value = "Count";
            sht.Range("E1").Value = "Order(Area)";
            sht.Range("F1").Value = "Shade No.";
            sht.Range("G1").Value = "Std Cons";
            sht.Range("H1").Value = "Yarn Detail";
            sht.Range("I1").Value = "Weaver Issue Qty";
            sht.Range("J1").Value = "Weaver Return Qty";
            sht.Range("K1").Value = "Company Stock";
            sht.Range("L1").Value = "Dyer Indent Bal";
            sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:H1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("I1:L1").Style.Alignment.WrapText = true;
            sht.Range("A1:L1").Style.Font.SetBold();
            sht.Columns("I:L").Width = 10.11;
            //*************
            DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "orderid", "Issueorderid", "DESIGN_QUALITY");
            row = 2;
            int rowfrom = 0;
            foreach (DataRow dr in dtdistinct.Rows)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "orderid=" + dr["orderid"] + " and Issueorderid=" + dr["Issueorderid"] + " and Design_Quality='" + dr["DESIGN_QUALITY"] + "'";
                DataSet ds1 = new DataSet();

                ds1.Tables.Add(dv.ToTable());
                Rowcount = ds1.Tables[0].Rows.Count;
                rowfrom = row;
                for (int i = 0; i < Rowcount; i++)
                {
                    if (i == 0)
                    {
                        sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Customercode"] + "/" + ds1.Tables[0].Rows[i]["CustomerorderNo"]);
                        sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Issueorderid"]);
                        sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Design_Quality"]);
                    }

                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Quality"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["OrderArea"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["ShadecolorName"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["IQty"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["YarnDetail"]);
                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["IssueQty"]);
                    sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["RecQty"]);
                    sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["CompanyStock"]);
                    sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["BalQty"]);
                    sht.Range("G" + row + ":L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    row = row + 1;
                }
                sht.Range("A" + row).Value = "Total";
                sht.Range("G" + row).FormulaA1 = "=SUM(G" + rowfrom + ":G" + (row - 1) + ")";
                sht.Range("H" + row).FormulaA1 = "=SUM(H" + rowfrom + ":H" + (row - 1) + ")";
                sht.Range("I" + row).FormulaA1 = "=SUM(I" + rowfrom + ":I" + (row - 1) + ")";
                sht.Range("J" + row).FormulaA1 = "=SUM(J" + rowfrom + ":J" + (row - 1) + ")";
                sht.Range("K" + row).FormulaA1 = "=SUM(K" + rowfrom + ":K" + (row - 1) + ")";
                sht.Range("L" + row).FormulaA1 = "=SUM(L" + rowfrom + ":L" + (row - 1) + ")";
                sht.Range("A" + row + ":L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row + ":L" + row).Style.Font.SetBold();
                row = row + 1;
            }
            //*************
            using (var a = sht.Range("A1" + ":L" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*************
            sht.Columns(1, 8).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("FolioWiseDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "Foliowise", "alert('No Record Found!');", true);
        }
    }
    private void DesignWiseConsmpDetail()
    {
        int Rowcount = 0;
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@companyId", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@processid", 5); //Dyeing
        param[2] = new SqlParameter("orderid", DDOrderNo.SelectedValue);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETDESIGNWISECONSMPDETAIL", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("DesignWiseDetail");
            int row = 0;
            //*******************
            sht.Range("A1").Value = "Cust.Code/Order No.";
            sht.Range("B1").Value = "Design";
            sht.Range("C1").Value = "Count";
            sht.Range("D1").Value = "Order(Area)";
            sht.Range("E1").Value = "Shade No.";
            sht.Range("F1").Value = "Std Cons";
            sht.Range("G1").Value = "Yarn Detail";
            sht.Range("H1").Value = "Dyeing Issue Qty";
            sht.Range("I1").Value = "Dyeing Rec Qty";
            sht.Range("J1").Value = "Indent Bal";
            sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:H1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("J1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("G1:J1").Style.Alignment.WrapText = true;
            sht.Range("A1:L1").Style.Font.SetBold();
            sht.Columns("G:J").Width = 10.11;
            //*************
            DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "orderid", "DESIGN");
            row = 2;
            int rowfrom = 0;
            foreach (DataRow dr in dtdistinct.Rows)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "orderid=" + dr["orderid"] + "  and Design='" + dr["DESIGN"] + "'";
                DataSet ds1 = new DataSet();

                ds1.Tables.Add(dv.ToTable());
                Rowcount = ds1.Tables[0].Rows.Count;
                rowfrom = row;
                for (int i = 0; i < Rowcount; i++)
                {
                    if (i == 0)
                    {
                        sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Customercode"] + "/" + ds1.Tables[0].Rows[i]["CustomerorderNo"]);
                        sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Design"]);
                    }

                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Quality"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["OrderArea"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["ShadecolorName"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["IQty"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["YarnDetail"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["IssueQty"]);
                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["RecQty"]);
                    sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["BalQty"]);
                    sht.Range("F" + row + ":L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    row = row + 1;
                }
                sht.Range("A" + row).Value = "Total";
                //sht.Range("F" + row).FormulaA1 = "=SUM(F" + rowfrom + ":F" + (row - 1) + ")";
                sht.Range("G" + row).FormulaA1 = "=SUM(G" + rowfrom + ":G" + (row - 1) + ")";
                sht.Range("H" + row).FormulaA1 = "=SUM(H" + rowfrom + ":H" + (row - 1) + ")";
                sht.Range("I" + row).FormulaA1 = "=SUM(I" + rowfrom + ":I" + (row - 1) + ")";
                sht.Range("J" + row).FormulaA1 = "=SUM(J" + rowfrom + ":J" + (row - 1) + ")";
                sht.Range("A" + row + ":L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row + ":L" + row).Style.Font.SetBold();
                row = row + 1;
            }
            //*************
            using (var a = sht.Range("A1" + ":J" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*************
            sht.Columns(1, 5).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("DesignWiseDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "Foliowise", "alert('No Record Found!');", true);
        }
    }
    private void PaidunpaidDetail()
    {
        int TOQTY = 0, TOBal = 0, TWQTY = 0, TRQTY = 0, TBAL = 0;
        int TWOQTY = 0, TWRQTY = 0, TWBal = 0, TPaid = 0, TUpaid = 0;
        Decimal TWOArea = 0, TWRArea = 0, TPArea = 0, TUparea = 0;


        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@localOrderNo", txtlocalOrderNo.Text);
        param[1] = new SqlParameter("@userid", Session["varuserid"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_getVendorWisepaymentReports", param);

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
            //**************
            //*******************BUYER ORDER DETAIL
            sht.Range("A1:J1").Merge();
            sht.Range("A1").Value = ds.Tables[0].Rows[0]["companyname"];
            sht.Range("A2:J2").Merge();
            sht.Range("A2").Value = "(PAYMENT REPORTS)";
            sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:J2").Style.Font.Bold = true;
            //Headers
            sht.Range("A3").Value = "Quality";
            sht.Range("B3").Value = "Design";
            sht.Range("C3").Value = "Color";
            sht.Range("D3").Value = "Shape";
            sht.Range("E3").Value = "Size";
            sht.Range("A3:E3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            sht.Range("F3").Value = "Order Qty.";
            sht.Range("G3").Value = "Order Bal.";
            sht.Range("H3").Value = "WQty.";
            sht.Range("I3").Value = "RecQty.";
            sht.Range("J3").Value = "Balance";
            sht.Range("F3:J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3:J3").Style.Font.Bold = true;
            //
            sht.Range("A4:J4").Merge();
            sht.Range("A4").Value = "Customer Code :" + ds.Tables[0].Rows[0]["customercode"] + "       Customer Order No : " + ds.Tables[0].Rows[0]["customerorderNo"] + "       SR No : " + ds.Tables[0].Rows[0]["Localorder"] + "";
            sht.Range("A4:J4").Style.Font.FontSize = 12;
            sht.Range("A4:J4").Style.Font.Bold = true;
            //*********Details
            row = 5;
            int Rowcount = ds.Tables[0].Rows.Count;
            int Orderbal = 0, Balance = 0;

            for (int i = 0; i < Rowcount; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Designname"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Colorname"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Shapename"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Export_Format"]);

                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Orderqty"]);
                TOQTY = TOQTY + Convert.ToInt32(ds.Tables[0].Rows[i]["Orderqty"]);
                Orderbal = Convert.ToInt32(ds.Tables[0].Rows[i]["Orderqty"]) - Convert.ToInt32(ds.Tables[0].Rows[i]["Worderqty"]);
                sht.Range("G" + row).SetValue(Orderbal);
                TOBal = TOBal + Orderbal;
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Worderqty"]);
                TWQTY = TWQTY + Convert.ToInt32(ds.Tables[0].Rows[i]["Worderqty"]);

                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Recqty"]);
                TRQTY = TRQTY + Convert.ToInt32(ds.Tables[0].Rows[i]["Recqty"]);

                if (Convert.ToInt32(ds.Tables[0].Rows[i]["Worderqty"]) - Convert.ToInt32(ds.Tables[0].Rows[i]["RecQty"]) < 0)
                {
                    Balance = 0;
                }
                else
                {
                    Balance = Convert.ToInt32(ds.Tables[0].Rows[i]["Worderqty"]) - Convert.ToInt32(ds.Tables[0].Rows[i]["RecQty"]);
                }
                sht.Range("J" + row).SetValue(Balance);
                TBAL = TBAL + Balance;
                row = row + 1;
            }
            //TOtal
            sht.Range("F" + row).SetValue(TOQTY);
            sht.Range("G" + row).SetValue(TOBal);
            sht.Range("H" + row).SetValue(TWQTY);
            sht.Range("I" + row).SetValue(TRQTY);
            sht.Range("J" + row).SetValue(TBAL);
            sht.Range("F" + row + ":J" + row).Style.Font.Bold = true;

            sht.Range("A3:J3").Style.Fill.BackgroundColor = XLColor.Yellow;
            sht.Range("F" + row + ":J" + row).Style.Fill.BackgroundColor = XLColor.AshGrey;

            using (var a = sht.Range("A3" + ":J" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //**********************
            row = row + 2;
            sht.Range("A" + row + ":J" + row).Merge();
            sht.Range("A" + row).Value = "(JOB WISE DETAIL)";
            sht.Range("A" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;
            row = row + 1;
            int Orderlastrow = row;
            //JOb Headers

            sht.Range("A" + row).Value = "WorderNo.";
            sht.Range("B" + row).Value = "Emp./Contractor Name";
            sht.Range("C" + row).Value = "Process";

            sht.Range("D" + row).Value = "Quality";
            sht.Range("E" + row).Value = "Design";
            sht.Range("F" + row).Value = "Color";
            sht.Range("G" + row).Value = "Shape";
            sht.Range("H" + row).Value = "Size";

            sht.Range("I" + row).Value = "WOqty.";
            sht.Range("J" + row).Value = "WOArea";
            sht.Range("K" + row).Value = "RecQty.";
            sht.Range("L" + row).Value = "RecArea";
            sht.Range("M" + row).Value = "Balance";

            sht.Range("N" + row).Value = "Paid Qty.";
            sht.Range("O" + row).Value = "Paid Area";
            sht.Range("P" + row).Value = "Unpaid Qty.";
            sht.Range("Q" + row).Value = "Unpaid Area";

            sht.Range("I3:Q3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A" + row + ":Q" + row).Style.Fill.BackgroundColor = XLColor.Yellow;
            sht.Range("A" + row + ":Q" + row).Style.Font.Bold = true;
            //
            row = row + 1;

            int Unpaidqty = 0;
            Decimal Unpaidarea = 0;

            DataTable dtdistinct = ds.Tables[1].DefaultView.ToTable(true, "SeqNo", "ProcessName");

            foreach (DataRow dr in dtdistinct.Rows)
            {
                DataView dv = new DataView(ds.Tables[1]);
                dv.RowFilter = "SeqNo=" + dr["SeqNo"] + " and ProcessName='" + dr["ProcessName"] + "'";
                DataSet ds1 = new DataSet();

                ds1.Tables.Add(dv.ToTable());
                Rowcount = ds1.Tables[0].Rows.Count;

                TWOQTY = 0; TWRQTY = 0; TWBal = 0; TPaid = 0; TUpaid = 0;
                TWOArea = 0; TWRArea = 0; TPArea = 0; TUparea = 0;

                for (int i = 0; i < Rowcount; i++)
                {

                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Issueorderid"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["EMpname"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["ProcessName"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Designname"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Colorname"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Shape"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["WOQTY"]);
                    TWOQTY = TWOQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["WOQTY"]);
                    sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["WOArea"]);
                    TWOArea = TWOArea + Convert.ToDecimal(ds1.Tables[0].Rows[i]["WOArea"]);
                    sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["WRQTY"]);
                    TWRQTY = TWRQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["WRQTY"]);
                    sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["WRArea"]);
                    TWRArea = TWRArea + Convert.ToDecimal(ds1.Tables[0].Rows[i]["WRArea"]);
                    if (Convert.ToInt32(ds1.Tables[0].Rows[i]["WOQTY"]) - Convert.ToInt32(ds1.Tables[0].Rows[i]["WRQTY"]) < 0)
                    {
                        Balance = 0;
                    }
                    else
                    {
                        Balance = Convert.ToInt32(ds1.Tables[0].Rows[i]["WOQTY"]) - Convert.ToInt32(ds1.Tables[0].Rows[i]["WRQTY"]);
                    }
                    sht.Range("M" + row).SetValue(Balance);
                    TWBal = TWBal + Balance;
                    sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["Paidqty"]);
                    TPaid = TPaid + Convert.ToInt32(ds1.Tables[0].Rows[i]["Paidqty"]);
                    sht.Range("O" + row).SetValue(ds1.Tables[0].Rows[i]["PaidArea"]);
                    TPArea = TPArea + Convert.ToDecimal(ds1.Tables[0].Rows[i]["Paidarea"]);
                    sht.Range("P" + row).SetValue(Unpaidqty);
                    TUpaid = TUpaid + Unpaidqty;
                    Unpaidarea = Convert.ToDecimal(ds1.Tables[0].Rows[i]["WRArea"]) - Convert.ToDecimal(ds1.Tables[0].Rows[i]["Paidarea"]);
                    sht.Range("Q" + row).SetValue(Unpaidarea);
                    TUparea = TUparea + Unpaidarea;
                    row = row + 1;
                    //**************************

                }
                //***************Total
                sht.Range("I" + row).SetValue(TWOQTY);
                sht.Range("J" + row).SetValue(TWOArea);
                sht.Range("K" + row).SetValue(TWRQTY);
                sht.Range("L" + row).SetValue(TWRArea);
                sht.Range("M" + row).SetValue(TWBal);
                sht.Range("N" + row).SetValue(TPaid);
                sht.Range("O" + row).SetValue(TPArea);
                sht.Range("P" + row).SetValue(TUpaid);
                sht.Range("Q" + row).SetValue(TUparea);
                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row).Value = dr["Processname"] + " " + "Total : ";
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;

                sht.Range("A" + row + ":Q" + row).Style.Fill.BackgroundColor = XLColor.AshGrey;
                sht.Range("I" + row + ":Q" + row).Style.Font.Bold = true;
                row = row + 1;
            }

            using (var a = sht.Range("A" + Orderlastrow + ":Q" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*************
            sht.Columns(1, 30).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("Paidunpaid_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "o2", "alert('No Record Found!');", true);
        }
        ds.Dispose();

    }

    protected void GetProcessDetails()
    { 
        
    }
    protected void Orderstatus()
    {
        SqlParameter[] param = new SqlParameter[6];
        param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@customerid", DDCustCode.SelectedIndex > 0 ? DDCustCode.SelectedValue : "0");
        param[2] = new SqlParameter("@orderid", DDOrderNo.SelectedIndex > 0 ? DDOrderNo.SelectedValue : "0");
        param[3] = new SqlParameter("@Where", "");
        param[4] = new SqlParameter("@Sizeunit", DDsizeunit.SelectedValue);
        param[5] = new SqlParameter("@OrderType", TROrderType.Visible == true ? (DDOrderType.SelectedIndex > 0 ? DDOrderType.SelectedValue : "0") : "0");
        //***********
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getorderstatus", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["GetDataset"] = ds;
            Session["rptFileName"] = "~\\Reports\\Rptorderstatus.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\Rptorderstatus.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "ostatus", "alert('No Record Found!');", true);
        }

    }
    protected void FinishingStatus()
    {
        String str = "";
        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " and OM.customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.qualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf.designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and vf.sizeid=" + DDSize.SelectedValue;
        }
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@Where", str);
        param[2] = new SqlParameter("@Processid", DDjobname.SelectedIndex > 0 ? DDjobname.SelectedValue : "0");
        param[3] = new SqlParameter("@Excelexport", chkexcelexport.Checked == true ? "1" : "0");
        //***********
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetFinishingStockStatus", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (chkexcelexport.Checked == true)
            {
                FinishingStatusexcelexport(ds);
            }
            else
            {
                Session["GetDataset"] = ds;
                Session["rptFileName"] = "~\\Reports\\RptfinishingStatus.rpt";
                Session["dsFileName"] = "~\\ReportSchema\\RptfinishingStatus.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
        }

    }
    protected void FinishingStatusexcelexport(DataSet ds)
    {
        try
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            //************
            sht.Range("A1:J1").Merge();
            sht.Range("A1").SetValue("ORDER WISE FINISHING STATUS");
            sht.Range("A1:J1").Style.Font.Bold = true;
            sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //JOB NAME
            sht.Range("H2:J2").Merge();
            sht.Range("H2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("H2:J2").Style.Font.Bold = true;


            sht.Range("A3").SetValue("Cust Code");
            sht.Range("B3").SetValue("Order No.");
            sht.Range("C3").SetValue("Stock No");
            sht.Range("D3").SetValue("Quality");
            sht.Range("E3").SetValue("Design");
            sht.Range("F3").SetValue("Color");
            sht.Range("G3").SetValue("Size");
            sht.Range("H3").SetValue("Folio No.");
            sht.Range("I3").SetValue("ODate");
            sht.Range("J3").SetValue("RDate");

            sht.Range("A3:J3").Style.Font.Bold = true;

            int row = 4;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (i == 0)
                {
                    sht.Range("H2").SetValue(ds.Tables[0].Rows[0]["JobName"]);
                }

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Custcode"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["OrderNo"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Tstockno"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Quality"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Design"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["colorname"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["size"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ONo"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Issuedate"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Receivedate"]);


                row = row + 1;
            }
            using (var a = sht.Range("A1" + ":J" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            //********************
            sht.Columns(1, 50).AdjustToContents();
            //******************
            String Path = "";
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("FINISHINGSTATUS_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
    protected void PoStatus()
    {
        String str = "";
        str = str + " and cast(OM.Dateadded as date)>='" + txtfromdate.Text + "' and cast(Om.Dateadded as date)<='" + txttodate.Text + "'";
        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " and OM.customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.qualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf.designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and vf.sizeid=" + DDSize.SelectedValue;
        }
        if (txtlocalOrderNo.Text != "")
        {
            str = str + " and OM.LocalOrder='" + txtlocalOrderNo.Text + "'";
        }
        if (DDorderstatus.SelectedIndex > 0)
        {
            str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        }
        //else if (DDorderstatus.SelectedIndex == 2)
        //{
        //    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        //}
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("pro_getPoorderstatus", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@Where", str);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@status", DDorderstatus.SelectedValue);
        DataTable dt = new DataTable();
        dt.Load(cmd.ExecuteReader());
        //*************
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        con.Close();
        con.Dispose();
        //SqlParameter[] param = new SqlParameter[3];
        //param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        //param[1] = new SqlParameter("@Where", str);
        //param[2] = new SqlParameter("@userid", Session["varuserid"]);
        //***********
        // DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_getPoorderstatus", param);

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
            //**************
            //*******************BUYER ORDER DETAIL
            //Headers
            sht.Range("A1").Value = "BUYER CODE";
            sht.Range("B1").Value = "PO STATUS";
            sht.Range("C1").Value = "PO TYPE";
            sht.Range("D1").Value = "LPO";
            sht.Range("E1").Value = "BPO";
            sht.Range("F1").Value = "ORD DT";
            sht.Range("G1").Value = "ORD ENTRY DATE";
            sht.Range("H1").Value = "DELV DT";
            sht.Range("I1").Value = "DISP DT";
            sht.Range("J1").Value = "PLANNED PROD START DT";
            sht.Range("K1").Value = "PLANNED PROD END DT";
            sht.Range("L1").Value = "HOLIDAYS";
            sht.Range("M1").Value = "AVL. DAYS";
            sht.Range("N1").Value = "PLANNED FIN.COM DT";
            sht.Range("O1").Value = "ORD UNIT";
            sht.Range("P1").Value = "PO AREA";
            sht.Range("Q1").Value = "PO AREA SQ YD";
            sht.Range("R1").Value = "ITEM NAME";
            //***********
            sht.Range("S1").Value = "QUALITY";
            sht.Range("T1").Value = "DESIGN";

            if (Session["varCompanyNo"].ToString() == "16")
            {
                sht.Range("U1").Value = "TOTAL YARN REQ";
            }
            else
            {
                sht.Column(21).Hide();
                sht.Range("U1").Value = "";
            }

            sht.Range("V1").Value = "SHAPE";
            sht.Range("W1").Value = "COLOR";
            sht.Range("X1").Value = "OSIZE";
            sht.Range("Y1").Value = "MAP SIZE";

            sht.Range("Z1").Value = "ORD QTY";
            sht.Range("AA1").Value = "TAG PCS";
            sht.Range("AB1").Value = "TOT ISS QTY";
            sht.Range("AC1").Value = "TOT ISS DUE";
            sht.Range("AD1").Value = "TOT BAZAR QTY";
            sht.Range("AE1").Value = "TOT BAZAR DUE";
            //***
            sht.Range("AF1").Value = "IN HS PLAN QTY";
            sht.Range("AG1").Value = "IN HS ISS";
            sht.Range("AH1").Value = "IN HS ISS DUE";
            sht.Range("AI1").Value = "IN HS BZR QTY";
            sht.Range("AJ1").Value = "IN HS BZR BAL";
            //**
            sht.Range("AK1").Value = "OS PLAN QTY";
            sht.Range("AL1").Value = "OS ISS";
            sht.Range("AM1").Value = "OS ISS DUE";
            sht.Range("AN1").Value = "OS BZR QTY";
            sht.Range("AO1").Value = "OS BZR BAL";
            //**
            sht.Range("AP1").Value = "UNDER FINISHING";
            sht.Range("AQ1").Value = "FINISHED QTY";
            sht.Range("AR1").Value = "DISP QTY";
            sht.Range("AS1").Value = "DISP DUE";
            sht.Range("AT1").Value = "PACKED_TO_OTHER";
            sht.Range("AU1").Value = "PACKED_FROM_OTHER";
            if (Session["varCompanyNo"].ToString() != "44")
            {
                sht.Range("AV1").Value = "CHAMPO BUYER CODE ORDER NO DELIVERY DATE";
            }


            sht.Range("Z1:AU1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("Q1:R1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A1:AV1").Style.Font.Bold = true;

            row = 2;
            int ORDQTY = 0, ISSQTY = 0, ISSDUE = 0, Bazarqty = 0, BazarDue = 0, DispQty = 0, DispDue = 0, PackedfromOther = 0, PackedtoOther = 0, Packedqty = 0,
                avldays = 0, Inhsplanqty = 0, inhsissue = 0, inhsissuedue = 0, inhsbazarqty = 0, inhsbazardue = 0,
                osplanqty = 0, osissue = 0, osissuedue = 0, osbazarqty = 0, osbazardue = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["customercode"]); //"BUYER CODE";
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Postatus"]); // = "PO TYPE";
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Potype"].ToString());  //"LPO";
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["LPO"].ToString());  //"BPO";
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["BPO"]);  //"ORD DT";
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ORDDT"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["orderentrydate"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["DELvdate"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Dispdt"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Planprodstartdt"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["planprodenddt"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Holidays"]);
                avldays = Convert.ToInt32(ds.Tables[0].Rows[i]["Daysdiff"]) - Convert.ToInt32(ds.Tables[0].Rows[i]["Holidays"]);
                sht.Range("M" + row).SetValue(avldays);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["Planfincomdt"]);
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["ORDUNIT"]);
                sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["POAREA"]);
                sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["POAREASQYD"]);
                sht.Range("R" + row).SetValue(ds.Tables[0].Rows[i]["ITEMNAME"]);
                sht.Range("S" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                sht.Range("T" + row).SetValue(ds.Tables[0].Rows[i]["DESIGNNAME"]);

                if (Session["VarCompanyNo"].ToString() == "16")
                {
                    sht.Range("U" + row).SetValue(ds.Tables[0].Rows[i]["ConsumptionQty"]);
                }
                else
                {
                    sht.Range("U" + row).SetValue("");
                }

                sht.Range("V" + row).SetValue(ds.Tables[0].Rows[i]["SHAPENAME"]);
                sht.Range("W" + row).SetValue(ds.Tables[0].Rows[i]["COLORNAME"]);
                sht.Range("X" + row).SetValue(ds.Tables[0].Rows[i]["OSiZE"]);
                sht.Range("Y" + row).SetValue(ds.Tables[0].Rows[i]["MAPSIZE"]);
                //***********
                ORDQTY = Convert.ToInt32(ds.Tables[0].Rows[i]["ORDQTY"]);
                sht.Range("Z" + row).SetValue(ORDQTY);// = "ORD QTY";
                sht.Range("AA" + row).SetValue(0);// = "TAG PCS";
                ISSQTY = Convert.ToInt32(ds.Tables[0].Rows[i]["ISSQTY"]);
                sht.Range("AB" + row).SetValue(ISSQTY);// = "ISS QTY";
                ISSDUE = ORDQTY - ISSQTY;
                sht.Range("AC" + row).SetValue((ISSDUE < 0 ? 0 : ISSDUE));// = "ISS DUE";
                Bazarqty = Convert.ToInt32(ds.Tables[0].Rows[i]["Bazarqty"]);
                sht.Range("AD" + row).SetValue(Bazarqty);// = "BAZAR QTY";
                BazarDue = ORDQTY - Bazarqty;
                sht.Range("AE" + row).SetValue((BazarDue < 0 ? 0 : BazarDue));// = "BAZAR DUE";
                //******In hs and Os Detail
                Inhsplanqty = Convert.ToInt32(ds.Tables[0].Rows[i]["Inhsplanqty"]);
                inhsissue = Convert.ToInt32(ds.Tables[0].Rows[i]["Inhsissue"]);
                inhsbazarqty = Convert.ToInt32(ds.Tables[0].Rows[i]["Inhsbazar"]);
                inhsissuedue = Inhsplanqty - inhsissue;// Inhsplanqty - inhsissue;
                inhsbazardue = Inhsplanqty - inhsbazarqty;// Inhsplanqty - inhsbazarqty;

                sht.Range("AF" + row).SetValue(Inhsplanqty);
                sht.Range("AG" + row).SetValue(inhsissue);
                sht.Range("AH" + row).SetValue(inhsissuedue);
                sht.Range("AI" + row).SetValue(inhsbazarqty);
                sht.Range("AJ" + row).SetValue(inhsbazardue);
                //*******OS 
                osplanqty = Convert.ToInt32(ds.Tables[0].Rows[i]["osplanqty"]);
                osissue = Convert.ToInt32(ds.Tables[0].Rows[i]["osissue"]);
                osbazarqty = Convert.ToInt32(ds.Tables[0].Rows[i]["osbazar"]);
                osissuedue = osplanqty - osissue;
                osbazardue = osplanqty - osbazarqty;

                sht.Range("AK" + row).SetValue(osplanqty);
                sht.Range("AL" + row).SetValue(osissue);
                sht.Range("AM" + row).SetValue(osissuedue);
                sht.Range("AN" + row).SetValue(osbazarqty);
                sht.Range("AO" + row).SetValue(osbazardue);

                //******
                sht.Range("AP" + row).SetValue(ds.Tables[0].Rows[i]["Underfinishing"]); // "UNDER FINISHING";
                sht.Range("AQ" + row).SetValue(ds.Tables[0].Rows[i]["finishedqty"]);  //"FINISHED QTY";
                Packedqty = Convert.ToInt32(ds.Tables[0].Rows[i]["Packedqty"]);
                PackedfromOther = Convert.ToInt32(ds.Tables[0].Rows[i]["PACKED_From_OTHER"]);
                PackedtoOther = Convert.ToInt32(ds.Tables[0].Rows[i]["PACKED_TO_OTHER"]);
                DispQty = Packedqty + PackedfromOther;
                DispDue = ORDQTY - DispQty;
                sht.Range("AR" + row).SetValue(DispQty);// = "DISP QTY";
                sht.Range("AS" + row).SetValue((DispDue < 0 ? 0 : DispDue)); // = "DISP DUE";
                sht.Range("AT" + row).SetValue(PackedtoOther); // = "PACKED_TO_OTHER";
                sht.Range("AU" + row).SetValue(PackedfromOther); //= "PACKED FROM OTHER";
                if (Session["varCompanyNo"].ToString() != "44")
                {
                    sht.Range("AV" + row).SetValue(ds.Tables[0].Rows[i]["Remarks"]);
                }

                row = row + 1;
            }
            //********************
            sht.Columns(1, 50).AdjustToContents();
            //******************
            sht.Row(1).Height = 28.80;
            sht.Column("K").Width = 13.78;
            sht.Column("L").Width = 13.78;
            sht.Column("O").Width = 13.78;
            sht.Range("A1:AU1").Style.Alignment.WrapText = true;

            //if (RDPOSTATUSInHouse.Checked == true || RDPOSTATUSOutSide.Checked == true) OR if (RDPOSTATUS.Checked == false) ARE SAME
            if (RDPOSTATUS.Checked == false)
            {
                sht.Column(2).Delete();
                sht.Column(2).Delete();
                sht.Column(2).Delete();
                sht.Column(3).Delete();
                sht.Column(3).Delete();
                sht.Column(4).Delete();
                sht.Column(4).Delete();
                sht.Column(4).Delete();
                sht.Column(4).Delete();
                sht.Column(4).Delete();
                sht.Column(4).Delete();
                sht.Column(4).Delete();
                sht.Column(4).Delete();
                sht.Column(4).Delete();
                sht.Column(5).Delete();
                sht.Column(6).Delete();
                sht.Column(8).Delete();

                sht.Column(9).Delete();
                sht.Column(9).Delete();
                sht.Column(9).Delete();
                sht.Column(9).Delete();
                sht.Column(9).Delete();
            }

            if (RDPOSTATUSOutSide.Checked == true)
            {
                sht.Column(9).Delete();
                sht.Column(9).Delete();
                sht.Column(9).Delete();
                sht.Column(9).Delete();
                sht.Column(9).Delete();
            }

            if (RDPOSTATUS.Checked == false)
            {
                sht.Column(14).Delete();
                sht.Column(14).Delete();
                sht.Column(14).Delete();
                sht.Column(14).Delete();
                sht.Column(14).Delete();
                sht.Column(14).Delete();
                sht.Column(14).Delete();
                sht.Column(14).Delete();
                sht.Column(14).Delete();
                sht.Column(14).Delete();
                sht.Column(14).Delete();
            }

            string Fileextension = "xlsx";
            string filename = "";
            if (RDPOSTATUS.Checked == true)
            {
                filename = UtilityModule.validateFilename("POSTATUS_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            }
            else if (RDPOSTATUSInHouse.Checked == true)
            {
                filename = UtilityModule.validateFilename("RDPOSTATUSINHOUSE_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            }
            if (RDPOSTATUSOutSide.Checked == true)
            {
                filename = UtilityModule.validateFilename("RDPOSTATUSOUTSIDE_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            }
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
        }

    }
    protected void PoStatusAGNI()
    {
       
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //SqlCommand cmd = new SqlCommand("PRO_POSTATUSREPORT", con);
        //cmd.CommandType = CommandType.StoredProcedure;
        //cmd.CommandTimeout = 300;

        //cmd.Parameters.AddWithValue("@CUSTORDER", DDOrderNo.SelectedValue);
        //cmd.Parameters.AddWithValue("@CUSTOMERID", DDCustCode.SelectedValue);
       
        //DataTable dt = new DataTable();
        //dt.Load(cmd.ExecuteReader());
        ////*************
        //DataSet ds = new DataSet();
        //ds.Tables.Add(dt);
        //con.Close();
        //con.Dispose();
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@CUSTORDER", DDOrderNo.SelectedValue);
        param[1] = new SqlParameter("@CUSTOMERID", DDCustCode.SelectedValue);


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_POSTATUSREPORT", param);

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
            //**************
            //*******************BUYER ORDER DETAIL
            //Headers

            sht.Range("G1").Value = "CUSTOMER ORDER STATUS";
            sht.Range("G1:K1").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("G1:K1").Style.Font.FontSize = 11;
            sht.Range("G1:K1").Style.Font.Bold = true;
            sht.Range("G1:K1").Merge();

            sht.Range("A2").Value = "Customer Code";
            sht.Range("A2:B2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A2:B2").Style.Font.FontSize = 11;
            sht.Range("A2:B2").Style.Font.Bold = true;
            sht.Range("A2:B2").Merge();

            sht.Range("C2").Value = DDCustCode.SelectedItem.Text;
            sht.Range("C2:C2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("C2:C2").Style.Font.FontSize = 11;
            sht.Range("C2:D2").Style.Font.Bold = true;
            sht.Range("C2:C2").Merge();


            sht.Range("A3").Value = "Order No";
            sht.Range("A3:B3").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A3:B3").Style.Font.FontSize = 11;
            sht.Range("A3:B3").Style.Font.Bold = true;
            sht.Range("A3:B3").Merge();

            sht.Range("C3").Value = DDOrderNo.SelectedItem.Text;
            sht.Range("C3:C3").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("C3:C3").Style.Font.FontSize = 11;
            sht.Range("C3:C3").Style.Font.Bold = true;
            sht.Range("C3:C3").Merge();


            if (ds != null)
            {
               
                    //sht.Range("G1").Value = "CUSTOMER ORDER STATUS";
                    //sht.Range("G1:H1").Style.Font.FontName = "Arial Unicode MS";
                    //sht.Range("G1:H1").Style.Font.FontSize = 12;
                    sht.Range("G1:H1").Style.Font.Bold = true;
                    //sht.Range("G1:H1").Merge();
                    sht.Range("A5:K5").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("A5:K5").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    sht.Range("A5:K5").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    sht.Range("A5:K5").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    sht.Range("A5").Value = "Category";
                    sht.Range("B5").Value = "Technique";
                    sht.Range("C5").Value = "Quality";
                    sht.Range("D5").Value = "Design";
                    sht.Range("E5").Value = "Color";
                    sht.Range("F5").Value = "Shape";
                    sht.Range("G5").Value = "Size";
                    sht.Range("H5").Value = "Unit";
                    sht.Range("I5").Value = "Order Qty.";
                    sht.Range("J5").Value = "Extra Qty.";
                    sht.Range("K5").Value = "Filler";
                    sht.Range("A5:K5").Style.Font.Bold = true;
                    row = 6;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CATEGORY_NAME"]); //"BUYER CODE";
                            sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]); // = "PO TYPE";
                            sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"].ToString());  //"LPO";
                            sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["DESIGNNAME"].ToString());  //"BPO";
                            sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["COLORNAME"]);  //"ORD DT";
                            sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["SHAPENAME"]);
                            sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["SIZE"]);
                            sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["UNIT"]);
                            sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["QTYREQUIRED"]);
                            sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["EXTRAQTY"]);
                            sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["FILLER"]);
                            sht.Range("A" + row + ":K" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":K" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":K" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":K" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                            row = row + 1;
                        }
                    }

                /////////////////////////////////Purchase////////////////////////////////////////////////////////////////////
                    row += 1;
                    sht.Range("A" + row).Value = "Purchase";
                    sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 11;
                    sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;
                    sht.Range("A" + row + ":B" + row).Merge();
                    row += 3;
                    sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;
                    //sht.Range("G1:H1").Merge();
                    sht.Range("A" + row + ":T" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row + ":T" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row + ":T" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row + ":T" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    sht.Range("A"+row).Value = "Category";
                    sht.Range("B" + row).Value = "Po No";
                    sht.Range("C" + row).Value = "Po Status";
                    sht.Range("D" + row).Value = "Po Date";
                    sht.Range("E" + row).Value = "Supp. Name";
                    sht.Range("F" + row).Value = "Item Name";
                    sht.Range("G" + row).Value = "Rate";
                    sht.Range("H" + row).Value = "PO Qty";
                    sht.Range("I" + row).Value = "Delv Date";
                    sht.Range("J" + row).Value = "CanQty/Date";
                    sht.Range("K" + row).Value = "RecDate";
                    sht.Range("L" + row).Value = "ChallanNo";
                    sht.Range("M" + row).Value = "LotNo";
                    sht.Range("N" + row).Value = "Bill No";

                    sht.Range("O" + row).Value = "Rec Qty";
                    sht.Range("P" + row).Value = "Ret Date";
                    sht.Range("Q" + row).Value = "Ret Qty";
                    sht.Range("R" + row).Value = "Pending Qty";
                    sht.Range("S" + row).Value = "Receive Remark";
                    sht.Range("T" + row).Value = "Order Remark";
                    sht.Range("A" + row + ":T" + row).Style.Font.Bold = true;
                    row +=1;
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            sht.Range("A" + row).SetValue(ds.Tables[1].Rows[i]["CATEGORY_NAME"]); //"BUYER CODE";
                            sht.Range("B" + row).SetValue(ds.Tables[1].Rows[i]["PO"]); // = "PO TYPE";
                            sht.Range("C" + row).SetValue(ds.Tables[1].Rows[i]["STATUS"].ToString());  //"LPO";
                            sht.Range("D" + row).SetValue(ds.Tables[1].Rows[i]["ORDERDATE"].ToString());  //"BPO";
                            sht.Range("E" + row).SetValue(ds.Tables[1].Rows[i]["EMPNAME"]);  //"ORD DT";
                            sht.Range("F" + row).SetValue(ds.Tables[1].Rows[i]["DESCRIPTION"]);
                            sht.Range("G" + row).SetValue(ds.Tables[1].Rows[i]["RATE"]);
                            sht.Range("H" + row).SetValue(ds.Tables[1].Rows[i]["ORDERQTY"]);
                            sht.Range("I" + row).SetValue(ds.Tables[1].Rows[i]["DELIVERYDATE"]);
                            sht.Range("J" + row).SetValue(ds.Tables[1].Rows[i]["ORDERCANQTY"]);
                            sht.Range("K" + row).SetValue(ds.Tables[1].Rows[i]["RECDATE"]);
                            sht.Range("L" + row).SetValue(ds.Tables[1].Rows[i]["CHALLANNO"]);
                            sht.Range("M" + row).SetValue(ds.Tables[1].Rows[i]["LOTNO"]);
                            sht.Range("N" + row).SetValue(ds.Tables[1].Rows[i]["BILLNO1"]);
                            sht.Range("O" + row).SetValue(ds.Tables[1].Rows[i]["RECQTY"]);
                            sht.Range("P" + row).SetValue(ds.Tables[1].Rows[i]["RETURNDATE"]);
                            sht.Range("Q" + row).SetValue(ds.Tables[1].Rows[i]["QTYRETURN"]);
                            sht.Range("R" + row).SetValue(ds.Tables[1].Rows[i]["PENDINGQTY"]);
                            sht.Range("S" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASERECEIVEITEMREMARK"]);
                            sht.Range("T" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASEORDERMASTERREMARK"]);

                          
                            sht.Range("A" + row + ":T" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":T" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":T" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":T" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                            row = row + 1;
                        }
                    }

                   
                    row += 1;
                    //sht.Range("A" + row).Value = "Process Program";
                    //sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial Unicode MS";
                    //sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 11;
                    //sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;
                    //sht.Range("A" + row + ":B" + row).Merge();
                    //row += 1;
                 List<int> pidpp = new List<int>();
                 pidpp = ds.Tables[2].AsEnumerable().Select(a => a.Field<Int32>("PROCESS_ID")).Distinct().ToList();

                 foreach (var item in pidpp)
                 {

                     DataTable dtpp = new DataTable();

                     var query = (from gift in ds.Tables[2].AsEnumerable()
                                  where gift.Field<int>("PROCESS_ID") == item
                                  //  fr.price,vf.ITEM_NAME,vf.QualityName,vf.ShapeName,vf.ColorName
                                  select new

                                  {




                                      processname = gift.Field<string>("PROCESS_NAME"),
                                      shadecolor = gift.Field<string>("SHADECOLORNAME"),
                                      dyername = gift.Field<string>("DYERNAME"),
                                      // shapename = gift.Field<string>("shapename"),
                                      qualityname = gift.Field<string>("QUALITYNAME"),
                                      consmpqty = gift.Field<double>("CONSMPQTY"),
                                      indentqty = gift.Field<double>("INDENTQTY"),

                                      lossqty = gift.Field<double>("LOSSQTY"),
                                      
                                      UNIT = gift.Field<string>("UNITNAME"),
                                      reqQTY = gift.Field<double>("REQQTY"),
                                      RECQTY = gift.Field<double>("recQty"),
                                      pendingqty = gift.Field<double>("PENDINGQTY"),
                                      balqty = gift.Field<double>("BALQTY"),


                                  });

                     row += 1;
                     sht.Range("A" + row).Value = query.FirstOrDefault().processname.ToUpper(); ;
                     sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial Unicode MS";
                     sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 11;
                     sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;
                     sht.Range("A" + row + ":B" + row).Merge();
                     row += 1;
                     sht.Range("A" + row + ":L" + row).Style.Font.Bold = true;
                     //sht.Range("G1:H1").Merge();
                     sht.Range("A" + row + ":L" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                     sht.Range("A" + row + ":L" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                     sht.Range("A" + row + ":L" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                     sht.Range("A" + row + ":L" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                     sht.Range("A" + row).Value = "Quality";
                     sht.Range("B" + row).Value = "Shade No";
                     sht.Range("C" + row).Value = "Consumption (per pc.)";
                     sht.Range("D" + row).Value = "Unit";
                     sht.Range("E" + row).Value = "Reqd. Qty.";
                     sht.Range("F" + row).Value = "Issued Qty.";
                     sht.Range("G" + row).Value = "Rcvd";
                     sht.Range("H" + row).Value = "Loss Qty.";
                     sht.Range("I" + row).Value = "Pending Qty";
                     sht.Range("J" + row).Value = "Reqd Bal Qty";
                     sht.Range("K" + row).Value = "JobWorker Name";
                   //  sht.Range("L" + row).Value = "Process Name";
                     //sht.Range("M" + row).Value = "LotNo";
                     //sht.Range("N" + row).Value = "Bill No";

                     //sht.Range("O" + row).Value = "Rec Qty";
                     //sht.Range("P" + row).Value = "Ret Date";
                     //sht.Range("Q" + row).Value = "Ret Qty";
                     //sht.Range("R" + row).Value = "Pending Qty";
                     //sht.Range("S" + row).Value = "Receive Remark";
                     //sht.Range("T" + row).Value = "Order Remark";
                     sht.Range("A" + row + ":K" + row).Style.Font.Bold = true;
                     row += 1;
                     if (query.Count() > 0)
                     {
                         int pendingqty = 0, balqty = 0, qty = 0, recqty = 0, orderqty = 0, sumissueqty = 0, sumrecqty = 0, sumpendinqty = 0;

                         foreach (var data in query)
                         {
                             sht.Range("A" + row).SetValue(data.qualityname); //"BUYER CODE";
                             sht.Range("B" + row).SetValue(data.shadecolor); // = "PO TYPE";
                             sht.Range("C" + row).SetValue(data.consmpqty);  //"LPO";
                             sht.Range("D" + row).SetValue(data.UNIT);  //"BPO";
                             sht.Range("E" + row).SetValue(data.reqQTY);  //"ORD DT";
                             sht.Range("F" + row).SetValue(data.indentqty);
                             sht.Range("G" + row).SetValue(data.RECQTY);
                             sht.Range("H" + row).SetValue(data.lossqty);
                             sht.Range("I" + row).SetValue(data.pendingqty);
                             sht.Range("J" + row).SetValue(data.balqty);
                             sht.Range("K" + row).SetValue(data.dyername);
                            // sht.Range("L" + row).SetValue(ds.Tables[2].Rows[i]["PROCESS_NAME"]);
                             //sht.Range("M" + row).SetValue(ds.Tables[1].Rows[i]["LOTNO"]);
                             //sht.Range("N" + row).SetValue(ds.Tables[1].Rows[i]["BILLNO1"]);
                             //sht.Range("O" + row).SetValue(ds.Tables[1].Rows[i]["RECQTY"]);
                             //sht.Range("P" + row).SetValue(ds.Tables[1].Rows[i]["RETURNDATE"]);
                             //sht.Range("Q" + row).SetValue(ds.Tables[1].Rows[i]["QTYRETURN"]);
                             //sht.Range("R" + row).SetValue(ds.Tables[1].Rows[i]["PENDINGQTY"]);
                             //sht.Range("S" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASERECEIVEITEMREMARK"]);
                             //sht.Range("T" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASEORDERMASTERREMARK"]);


                             sht.Range("A" + row + ":K" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                             sht.Range("A" + row + ":K" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                             sht.Range("A" + row + ":K" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                             sht.Range("A" + row + ":K" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                             row = row + 1;
                         }
                     }
                 }
                    row += 1;

                 List<int> pidcutting = new List<int>();
                 pidcutting = ds.Tables[3].AsEnumerable().Select(a => a.Field<Int32>("PROCESS_NAME_ID")).Distinct().ToList();

                 foreach (var item in pidcutting)
                  {
                      DataTable dtcutting = new DataTable();

                      var query = (from gift in ds.Tables[3].AsEnumerable()
                                   where gift.Field<int>("PROCESS_NAME_ID") == item
                                   //  fr.price,vf.ITEM_NAME,vf.QualityName,vf.ShapeName,vf.ColorName
                                   select new
                                   {
                                       processname = gift.Field<string>("process_name"),
                                       itemname = gift.Field<string>("item_name"),
                                       colorname = gift.Field<string>("colorname"),
                                      // shapename = gift.Field<string>("shapename"),
                                       qualityname = gift.Field<string>("qualityname"),
                                       cateogoryname = gift.Field<string>("category_name"),
                                       designname = gift.Field<string>("designname"),

                                       size = gift.Field<string>("size"),
                                      // width = gift.Field<string>("WIDTH"),
                                       //height = gift.Field<Double?>("heightinch"),
                                       UNIT = gift.Field<string>("UNITNAME"),
                                       QTY = gift.Field<int>("qty"),
                                       RECQTY = gift.Field<int>("recQty"),
                                       ISSUEQTY = gift.Field<int>("QtyOrder"),


                                   });



                      row += 1;
                      sht.Range("A" + row).Value = query.FirstOrDefault().processname.ToUpper();
                      sht.Range("A" + row + ":C" + row).Style.Font.FontName = "Arial Unicode MS";
                      sht.Range("A" + row + ":C" + row).Style.Font.FontSize = 11;
                      sht.Range("A" + row + ":C" + row).Style.Font.Bold = true;
                      sht.Range("A" + row + ":C" + row).Merge();
                      row += 1;
                      sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;
                      //sht.Range("G1:H1").Merge();
                      sht.Range("A" + row + ":J" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                      sht.Range("A" + row + ":J" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                      sht.Range("A" + row + ":J" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                      sht.Range("A" + row + ":J" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                      sht.Range("A" + row).Value = "Components";
                      sht.Range("B" + row).Value = "Design";
                      sht.Range("C" + row).Value = "Color";
                      sht.Range("D" + row).Value = "Size";
                      sht.Range("E" + row).Value = "Unit";
                      sht.Range("F" + row).Value = "Order Qty.";
                      sht.Range("G" + row).Value = "Issued";
                      sht.Range("H" + row).Value = "Rcvd";
                      sht.Range("I" + row).Value = "Pending Qty";
                      sht.Range("J" + row).Value = "Reqd Bal Qty";
                      // sht.Range("K" + row).Value = "JOBWORKER NAME";
                      //sht.Range("L" + row).Value = "ChallanNo";
                      //sht.Range("M" + row).Value = "LotNo";
                      //sht.Range("N" + row).Value = "Bill No";

                      //sht.Range("O" + row).Value = "Rec Qty";
                      //sht.Range("P" + row).Value = "Ret Date";
                      //sht.Range("Q" + row).Value = "Ret Qty";
                      //sht.Range("R" + row).Value = "Pending Qty";
                      //sht.Range("S" + row).Value = "Receive Remark";
                      //sht.Range("T" + row).Value = "Order Remark";
                      sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;
                      row += 1;
                      if (query.Count() > 0)
                      {
                          Int32 pendingqty = 0, balqty = 0, qty = 0, recqty = 0, orderqty = 0, sumissueqty = 0, sumrecqty = 0, sumpendinqty = 0;

                          foreach (var data in query)
                          {
                              qty = Convert.ToInt32(data.QTY);
                              recqty = Convert.ToInt16(data.RECQTY);
                              orderqty = Convert.ToInt16(data.ISSUEQTY);
                              pendingqty = qty - recqty;
                              balqty = orderqty - qty;

                              sht.Range("A" + row).SetValue(data.qualityname); //"BUYER CODE";
                              sht.Range("B" + row).SetValue(data.designname); // = "PO TYPE";
                              sht.Range("C" + row).SetValue(data.colorname);  //"LPO";
                              sht.Range("D" + row).SetValue(data.size);  //"BPO";
                              sht.Range("E" + row).SetValue(data.UNIT);  //"ORD DT";
                              sht.Range("F" + row).SetValue(data.ISSUEQTY);
                              sht.Range("G" + row).SetValue(data.QTY);
                              sht.Range("H" + row).SetValue(data.RECQTY);
                              sht.Range("I" + row).SetValue(pendingqty);
                              sht.Range("J" + row).SetValue(balqty);
                              //sht.Range("K" + row).SetValue(ds.Tables[3].Rows[i]["DYERNAME"]);
                              //sht.Range("L" + row).SetValue(ds.Tables[1].Rows[i]["CHALLANNO"]);
                              //sht.Range("M" + row).SetValue(ds.Tables[1].Rows[i]["LOTNO"]);
                              //sht.Range("N" + row).SetValue(ds.Tables[1].Rows[i]["BILLNO1"]);
                              //sht.Range("O" + row).SetValue(ds.Tables[1].Rows[i]["RECQTY"]);
                              //sht.Range("P" + row).SetValue(ds.Tables[1].Rows[i]["RETURNDATE"]);
                              //sht.Range("Q" + row).SetValue(ds.Tables[1].Rows[i]["QTYRETURN"]);
                              //sht.Range("R" + row).SetValue(ds.Tables[1].Rows[i]["PENDINGQTY"]);
                              //sht.Range("S" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASERECEIVEITEMREMARK"]);
                              //sht.Range("T" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASEORDERMASTERREMARK"]);


                              sht.Range("A" + row + ":J" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                              sht.Range("A" + row + ":J" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                              sht.Range("A" + row + ":J" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                              sht.Range("A" + row + ":J" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                              row = row + 1;
                          }
                      }
                  }
                    row += 1;
                    sht.Range("A" + row).Value = "Raw Material Issue for Cutting";
                    sht.Range("A" + row + ":D" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":D" + row).Style.Font.FontSize = 11;
                    sht.Range("A" + row + ":D" + row).Style.Font.Bold = true;
                    sht.Range("A" + row + ":D" + row).Merge();
                    row += 1;
                    sht.Range("A" + row + ":H" + row).Style.Font.Bold = true;
                    //sht.Range("G1:H1").Merge();
                    sht.Range("A" + row + ":H" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row + ":H" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row + ":H" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row + ":H" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row).Value = "Quality";
                    sht.Range("B" + row).Value = "Shade Color";
                    sht.Range("C" + row).Value = "Consumption (per pc.)";
                    sht.Range("D" + row).Value = "Unit";
                    sht.Range("E" + row).Value = "Reqd. Qty.";
                    sht.Range("F" + row).Value = "Issue Qty";
                    sht.Range("G" + row).Value = "Bal to Issue";
                    sht.Range("H" + row).Value = "Jobworker Name";
                    //sht.Range("I" + row).Value = "Pending Qty";
                    //sht.Range("J" + row).Value = "Reqd Bal Qty";
                    // sht.Range("K" + row).Value = "JOBWORKER NAME";
                    //sht.Range("L" + row).Value = "ChallanNo";
                    //sht.Range("M" + row).Value = "LotNo";
                    //sht.Range("N" + row).Value = "Bill No";

                    //sht.Range("O" + row).Value = "Rec Qty";
                    //sht.Range("P" + row).Value = "Ret Date";
                    //sht.Range("Q" + row).Value = "Ret Qty";
                    //sht.Range("R" + row).Value = "Pending Qty";
                    //sht.Range("S" + row).Value = "Receive Remark";
                    //sht.Range("T" + row).Value = "Order Remark";
                    sht.Range("A" + row + ":H" + row).Style.Font.Bold = true;
                    row += 1;
                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        int pendingqty = 0, balqty = 0, qty = 0, recqty = 0, orderqty = 0;

                        for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                        {
                            qty = Convert.ToInt16(ds.Tables[4].Rows[i]["REQQTY"]);
                            recqty = Convert.ToInt16(ds.Tables[4].Rows[i]["ISSQTY"]);
                           // orderqty = Convert.ToInt16(ds.Tables[3].Rows[i]["qtyorder"]);
                            pendingqty = qty - recqty;
                          //  balqty = orderqty - qty;

                            sht.Range("A" + row).SetValue(ds.Tables[4].Rows[i]["QUALITYNAME"]); //"BUYER CODE";
                            sht.Range("B" + row).SetValue(ds.Tables[4].Rows[i]["SHADECOLORNAME"]); // = "PO TYPE";
                            sht.Range("C" + row).SetValue(ds.Tables[4].Rows[i]["CONSMPQTY"].ToString());  //"LPO";
                            sht.Range("D" + row).SetValue(ds.Tables[4].Rows[i]["UNIT"].ToString());  //"BPO";
                            sht.Range("E" + row).SetValue(ds.Tables[4].Rows[i]["REQQTY"]);  //"ORD DT";
                            sht.Range("F" + row).SetValue(ds.Tables[4].Rows[i]["ISSQTY"]);
                            sht.Range("G" + row).SetValue(pendingqty);
                            sht.Range("H" + row).SetValue("");
                            //sht.Range("I" + row).SetValue(pendingqty);
                            //sht.Range("J" + row).SetValue(balqty);
                            //sht.Range("K" + row).SetValue(ds.Tables[3].Rows[i]["DYERNAME"]);
                            //sht.Range("L" + row).SetValue(ds.Tables[1].Rows[i]["CHALLANNO"]);
                            //sht.Range("M" + row).SetValue(ds.Tables[1].Rows[i]["LOTNO"]);
                            //sht.Range("N" + row).SetValue(ds.Tables[1].Rows[i]["BILLNO1"]);
                            //sht.Range("O" + row).SetValue(ds.Tables[1].Rows[i]["RECQTY"]);
                            //sht.Range("P" + row).SetValue(ds.Tables[1].Rows[i]["RETURNDATE"]);
                            //sht.Range("Q" + row).SetValue(ds.Tables[1].Rows[i]["QTYRETURN"]);
                            //sht.Range("R" + row).SetValue(ds.Tables[1].Rows[i]["PENDINGQTY"]);
                            //sht.Range("S" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASERECEIVEITEMREMARK"]);
                            //sht.Range("T" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASEORDERMASTERREMARK"]);


                            sht.Range("A" + row + ":H" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":H" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":H" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":H" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                            row = row + 1;
                        }
                    }

                    row += 1;
                    sht.Range("A" + row).Value = "Raw Material Issue for Stitching";
                    sht.Range("A" + row + ":D" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":D" + row).Style.Font.FontSize = 11;
                    sht.Range("A" + row + ":D" + row).Style.Font.Bold = true;
                    sht.Range("A" + row + ":D" + row).Merge();
                    row += 1;
                    sht.Range("A" + row + ":H" + row).Style.Font.Bold = true;
                    //sht.Range("G1:H1").Merge();
                    sht.Range("A" + row + ":H" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row + ":H" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row + ":H" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row + ":H" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row).Value = "Quality";
                    sht.Range("B" + row).Value = "Shade Color";
                    sht.Range("C" + row).Value = "Consumption (per pc.)";
                    sht.Range("D" + row).Value = "Unit";
                    sht.Range("E" + row).Value = "Reqd. Qty.";
                    sht.Range("F" + row).Value = "Issue Qty";
                    sht.Range("G" + row).Value = "Bal to Issue";
                    sht.Range("H" + row).Value = "Jobworker Name";
                    //sht.Range("I" + row).Value = "Pending Qty";
                    //sht.Range("J" + row).Value = "Reqd Bal Qty";
                    // sht.Range("K" + row).Value = "JOBWORKER NAME";
                    //sht.Range("L" + row).Value = "ChallanNo";
                    //sht.Range("M" + row).Value = "LotNo";
                    //sht.Range("N" + row).Value = "Bill No";

                    //sht.Range("O" + row).Value = "Rec Qty";
                    //sht.Range("P" + row).Value = "Ret Date";
                    //sht.Range("Q" + row).Value = "Ret Qty";
                    //sht.Range("R" + row).Value = "Pending Qty";
                    //sht.Range("S" + row).Value = "Receive Remark";
                    //sht.Range("T" + row).Value = "Order Remark";
                    sht.Range("A" + row + ":H" + row).Style.Font.Bold = true;
                    row += 1;
                    if (ds.Tables[6].Rows.Count > 0)
                    {
                        int pendingqty = 0, balqty = 0, qty = 0, recqty = 0, orderqty = 0;

                        for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                        {
                            qty = Convert.ToInt16(ds.Tables[6].Rows[i]["REQQTY"]);
                            recqty = Convert.ToInt16(ds.Tables[6].Rows[i]["ISSQTY"]);
                            // orderqty = Convert.ToInt16(ds.Tables[3].Rows[i]["qtyorder"]);
                            pendingqty = qty - recqty;
                            //  balqty = orderqty - qty;

                            sht.Range("A" + row).SetValue(ds.Tables[6].Rows[i]["QUALITYNAME"]); //"BUYER CODE";
                            sht.Range("B" + row).SetValue(ds.Tables[6].Rows[i]["SHADECOLORNAME"]); // = "PO TYPE";
                            sht.Range("C" + row).SetValue(ds.Tables[6].Rows[i]["CONSMPQTY"].ToString());  //"LPO";
                            sht.Range("D" + row).SetValue(ds.Tables[6].Rows[i]["UNIT"].ToString());  //"BPO";
                            sht.Range("E" + row).SetValue(ds.Tables[6].Rows[i]["REQQTY"]);  //"ORD DT";
                            sht.Range("F" + row).SetValue(ds.Tables[6].Rows[i]["ISSQTY"]);
                            sht.Range("G" + row).SetValue(pendingqty);
                            sht.Range("H" + row).SetValue("");
                            //sht.Range("I" + row).SetValue(pendingqty);
                            //sht.Range("J" + row).SetValue(balqty);
                            //sht.Range("K" + row).SetValue(ds.Tables[3].Rows[i]["DYERNAME"]);
                            //sht.Range("L" + row).SetValue(ds.Tables[1].Rows[i]["CHALLANNO"]);
                            //sht.Range("M" + row).SetValue(ds.Tables[1].Rows[i]["LOTNO"]);
                            //sht.Range("N" + row).SetValue(ds.Tables[1].Rows[i]["BILLNO1"]);
                            //sht.Range("O" + row).SetValue(ds.Tables[1].Rows[i]["RECQTY"]);
                            //sht.Range("P" + row).SetValue(ds.Tables[1].Rows[i]["RETURNDATE"]);
                            //sht.Range("Q" + row).SetValue(ds.Tables[1].Rows[i]["QTYRETURN"]);
                            //sht.Range("R" + row).SetValue(ds.Tables[1].Rows[i]["PENDINGQTY"]);
                            //sht.Range("S" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASERECEIVEITEMREMARK"]);
                            //sht.Range("T" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASEORDERMASTERREMARK"]);


                            sht.Range("A" + row + ":H" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":H" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":H" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":H" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                            row = row + 1;
                        }
                    }

                    row += 1;
                    sht.Range("A" + row).Value = "Raw Material Issue for Packing";
                    sht.Range("A" + row + ":D" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":D" + row).Style.Font.FontSize = 11;
                    sht.Range("A" + row + ":D" + row).Style.Font.Bold = true;
                    sht.Range("A" + row + ":D" + row).Merge();
                    row += 1;
                    sht.Range("A" + row + ":H" + row).Style.Font.Bold = true;
                    //sht.Range("G1:H1").Merge();
                    sht.Range("A" + row + ":H" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row + ":H" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row + ":H" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row + ":H" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row).Value = "Quality";
                    sht.Range("B" + row).Value = "Shade Color";
                    sht.Range("C" + row).Value = "Consumption (per pc.)";
                    sht.Range("D" + row).Value = "Unit";
                    sht.Range("E" + row).Value = "Reqd. Qty.";
                    sht.Range("F" + row).Value = "Issue Qty";
                    sht.Range("G" + row).Value = "Bal to Issue";
                    sht.Range("H" + row).Value = "Jobworker Name";
                    //sht.Range("I" + row).Value = "Pending Qty";
                    //sht.Range("J" + row).Value = "Reqd Bal Qty";
                    // sht.Range("K" + row).Value = "JOBWORKER NAME";
                    //sht.Range("L" + row).Value = "ChallanNo";
                    //sht.Range("M" + row).Value = "LotNo";
                    //sht.Range("N" + row).Value = "Bill No";

                    //sht.Range("O" + row).Value = "Rec Qty";
                    //sht.Range("P" + row).Value = "Ret Date";
                    //sht.Range("Q" + row).Value = "Ret Qty";
                    //sht.Range("R" + row).Value = "Pending Qty";
                    //sht.Range("S" + row).Value = "Receive Remark";
                    //sht.Range("T" + row).Value = "Order Remark";
                    sht.Range("A" + row + ":H" + row).Style.Font.Bold = true;
                    row += 1;
                    if (ds.Tables[7].Rows.Count > 0)
                    {
                        int pendingqty = 0, balqty = 0, qty = 0, recqty = 0, orderqty = 0;

                        for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                        {
                            qty = Convert.ToInt16(ds.Tables[7].Rows[i]["REQQTY"]);
                            recqty = Convert.ToInt16(ds.Tables[7].Rows[i]["ISSQTY"]);
                            // orderqty = Convert.ToInt16(ds.Tables[3].Rows[i]["qtyorder"]);
                            pendingqty = qty - recqty;
                            //  balqty = orderqty - qty;

                            sht.Range("A" + row).SetValue(ds.Tables[7].Rows[i]["QUALITYNAME"]); //"BUYER CODE";
                            sht.Range("B" + row).SetValue(ds.Tables[7].Rows[i]["SHADECOLORNAME"]); // = "PO TYPE";
                            sht.Range("C" + row).SetValue(ds.Tables[7].Rows[i]["CONSMPQTY"].ToString());  //"LPO";
                            sht.Range("D" + row).SetValue(ds.Tables[7].Rows[i]["UNIT"].ToString());  //"BPO";
                            sht.Range("E" + row).SetValue(ds.Tables[7].Rows[i]["REQQTY"]);  //"ORD DT";
                            sht.Range("F" + row).SetValue(ds.Tables[7].Rows[i]["ISSQTY"]);
                            sht.Range("G" + row).SetValue(pendingqty);
                            sht.Range("H" + row).SetValue("");
                            //sht.Range("I" + row).SetValue(pendingqty);
                            //sht.Range("J" + row).SetValue(balqty);
                            //sht.Range("K" + row).SetValue(ds.Tables[3].Rows[i]["DYERNAME"]);
                            //sht.Range("L" + row).SetValue(ds.Tables[1].Rows[i]["CHALLANNO"]);
                            //sht.Range("M" + row).SetValue(ds.Tables[1].Rows[i]["LOTNO"]);
                            //sht.Range("N" + row).SetValue(ds.Tables[1].Rows[i]["BILLNO1"]);
                            //sht.Range("O" + row).SetValue(ds.Tables[1].Rows[i]["RECQTY"]);
                            //sht.Range("P" + row).SetValue(ds.Tables[1].Rows[i]["RETURNDATE"]);
                            //sht.Range("Q" + row).SetValue(ds.Tables[1].Rows[i]["QTYRETURN"]);
                            //sht.Range("R" + row).SetValue(ds.Tables[1].Rows[i]["PENDINGQTY"]);
                            //sht.Range("S" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASERECEIVEITEMREMARK"]);
                            //sht.Range("T" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASEORDERMASTERREMARK"]);


                            sht.Range("A" + row + ":H" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":H" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":H" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":H" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                            row = row + 1;
                        }
                    }




                    List<int> pid = new List<int>();
                  pid=  ds.Tables[5].AsEnumerable().Select(a => a.Field<Int32>("processid")).Distinct().ToList();
                    
                  foreach (var item in pid)
                  {

                      DataTable dtfinal = new DataTable();

                      var query = (from gift in ds.Tables[5].AsEnumerable()
                                   where gift.Field<int>("processid") == item 
                                   //  fr.price,vf.ITEM_NAME,vf.QualityName,vf.ShapeName,vf.ColorName
                                   select new
                                   {
                                       processname=gift.Field<string>("process_name"),
                                       itemname = gift.Field<string>("item_name"),
                                       colorname = gift.Field<string>("colorname"),
                                       shapename = gift.Field<string>("shapename"),
                                       qualityname = gift.Field<string>("qualityname"),
                                       cateogoryname = gift.Field<string>("category_name"),
                                       designname = gift.Field<string>("designname"),

                                       length = gift.Field<string>("LENGTH"),
                                       width = gift.Field<string>("WIDTH"),
                                       //height = gift.Field<Double?>("heightinch"),
                                       UNIT = gift.Field<string>("UNITNAME"),
                                       RECQTY = gift.Field<int>("RECEIVEQTY"),
                                       ISSUEQTY = gift.Field<int>("ISSUEQTY"),
                                       REQQTY= gift.Field<int>("ORDERQTY"),


                                   });


                      row += 1;
                      sht.Range("A" + row).Value = (query.FirstOrDefault().processname.ToUpper() == "CUTTING") ? "Purchase" : query.FirstOrDefault().processname;
                      sht.Range("A" + row + ":D" + row).Style.Font.FontName = "Arial Unicode MS";
                      sht.Range("A" + row + ":D" + row).Style.Font.FontSize = 11;
                      sht.Range("A" + row + ":D" + row).Style.Font.Bold = true;
                      sht.Range("A" + row + ":D" + row).Merge();
                      row += 3;
                      sht.Range("A" + row + ":M" + row).Style.Font.Bold = true;
                      //sht.Range("G1:H1").Merge();
                      sht.Range("A" + row + ":M" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                      sht.Range("A" + row + ":M" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                      sht.Range("A" + row + ":M" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                      sht.Range("A" + row + ":M" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                      sht.Range("A" + row).Value = "Category";
                      sht.Range("B" + row).Value = "Technique";
                      sht.Range("C" + row).Value = "Quality";
                      sht.Range("D" + row).Value = "Design";
                      sht.Range("E" + row).Value = "Color";
                      sht.Range("F" + row).Value = "Shape";
                      sht.Range("G" + row).Value = "Size";
                      sht.Range("H" + row).Value = "Unit";
                    sht.Range("I" + row).Value = "Req Qty";
                    sht.Range("J" + row).Value = "Issue Qty";
                      sht.Range("K" + row).Value = "Recv  Qty";
                       sht.Range("L" + row).Value = "Pending Qty";
                      sht.Range("M" + row).Value = "Req Bal to Issue";
                      //sht.Range("M" + row).Value = "LotNo";
                      //sht.Range("N" + row).Value = "Bill No";

                      //sht.Range("O" + row).Value = "Rec Qty";
                      //sht.Range("P" + row).Value = "Ret Date";
                      //sht.Range("Q" + row).Value = "Ret Qty";
                      //sht.Range("R" + row).Value = "Pending Qty";
                      //sht.Range("S" + row).Value = "Receive Remark";
                      //sht.Range("T" + row).Value = "Order Remark";
                      sht.Range("A" + row + ":M" + row).Style.Font.Bold = true;
                      row += 1;
                      if (query.Count() > 0)
                      {
                          int pendingqty = 0, balqty = 0, qty = 0, recqty = 0, orderqty = 0, sumissueqty = 0, sumrecqty = 0, sumpendinqty = 0,sumissuebal = 0;

                          foreach (var data in query)
                          {
                              //qty = Convert.ToInt16(ds.Tables[3].Rows[i]["QTY"]);
                              //recqty = Convert.ToInt16(ds.Tables[3].Rows[i]["RECQTY"]);
                              //orderqty = Convert.ToInt16(ds.Tables[3].Rows[i]["qtyorder"]);
                              //pendingqty = qty - recqty;
                              //balqty = orderqty - qty;
                              sumissueqty += Convert.ToInt16(data.ISSUEQTY);
                              sumrecqty += Convert.ToInt16(data.RECQTY);
                              sumpendinqty += Convert.ToInt16(data.ISSUEQTY) - Convert.ToInt16(data.RECQTY);
                            sumissuebal += Convert.ToInt16(data.REQQTY) - Convert.ToInt16(data.ISSUEQTY);
                            sht.Range("A" + row).SetValue(data.cateogoryname); //"BUYER CODE";
                              sht.Range("B" + row).SetValue(data.itemname); // = "PO TYPE";
                              sht.Range("C" + row).SetValue(data.qualityname);  //"LPO";
                              sht.Range("D" + row).SetValue(data.designname);  //"BPO";
                              sht.Range("E" + row).SetValue(data.colorname);  //"ORD DT";
                              sht.Range("F" + row).SetValue(data.shapename);
                              sht.Range("G" + row).SetValue(data.width + "x" + data.length);
                              sht.Range("H" + row).SetValue(data.UNIT);
                              sht.Range("I" + row).SetValue(data.REQQTY);
                              sht.Range("J" + row).SetValue(data.ISSUEQTY);
                              sht.Range("K" + row).SetValue(data.RECQTY);
                              sht.Range("L" + row).SetValue(Convert.ToInt16(data.ISSUEQTY) - Convert.ToInt16(data.RECQTY));
                            sht.Range("M" + row).SetValue(Convert.ToInt16(data.REQQTY) - Convert.ToInt16(data.ISSUEQTY));
                            //sht.Range("L" + row).SetValue(ds.Tables[1].Rows[i]["CHALLANNO"]);
                            //sht.Range("M" + row).SetValue(ds.Tables[1].Rows[i]["LOTNO"]);
                            //sht.Range("N" + row).SetValue(ds.Tables[1].Rows[i]["BILLNO1"]);
                            //sht.Range("O" + row).SetValue(ds.Tables[1].Rows[i]["RECQTY"]);
                            //sht.Range("P" + row).SetValue(ds.Tables[1].Rows[i]["RETURNDATE"]);
                            //sht.Range("Q" + row).SetValue(ds.Tables[1].Rows[i]["QTYRETURN"]);
                            //sht.Range("R" + row).SetValue(ds.Tables[1].Rows[i]["PENDINGQTY"]);
                            //sht.Range("S" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASERECEIVEITEMREMARK"]);
                            //sht.Range("T" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASEORDERMASTERREMARK"]);


                            sht.Range("A" + row + ":M" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                              sht.Range("A" + row + ":M" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                              sht.Range("A" + row + ":M" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                              sht.Range("A" + row + ":M" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                              row = row + 1;
                          }

                        //  row += 1;
                          sht.Range("H" + row).Value = "Total";
                          sht.Range("J" + row).Value = sumissueqty;
                          sht.Range("K" + row).Value = sumrecqty;
                          sht.Range("L" + row).Value = sumpendinqty;
                          sht.Range("M" + row).Value = sumissuebal;
                          sht.Range("H" + row + ":M" + row).Style.Font.FontName = "Arial Unicode MS";
                          sht.Range("H" + row + ":M" + row).Style.Font.FontSize = 11;
                          sht.Range("H" + row + ":M" + row).Style.Font.Bold = true;
                         // sht.Range("H" + row + ":J" + row).Merge();
                          sht.Range("H" + row + ":M" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                          sht.Range("H" + row + ":M" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                          sht.Range("H" + row + ":M" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                          sht.Range("H" + row + ":M" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                      }
                  
                  }
                    





            }

            string Fileextension = "xlsx";
            string filename = "";
           
           filename = UtilityModule.validateFilename("POSTATUS_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
          
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
        }

    }
    protected void PodetailAGNI()
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //SqlCommand cmd = new SqlCommand("PRO_POSTATUSREPORT", con);
        //cmd.CommandType = CommandType.StoredProcedure;
        //cmd.CommandTimeout = 300;

        //cmd.Parameters.AddWithValue("@CUSTORDER", DDOrderNo.SelectedValue);
        //cmd.Parameters.AddWithValue("@CUSTOMERID", DDCustCode.SelectedValue);

        //DataTable dt = new DataTable();
        //dt.Load(cmd.ExecuteReader());
        ////*************
        //DataSet ds = new DataSet();
        //ds.Tables.Add(dt);
        //con.Close();
        //con.Dispose();
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@CUSTORDER", DDOrderNo.SelectedValue);
        param[1] = new SqlParameter("@CUSTOMERID", DDCustCode.SelectedValue);


        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_PODETAILREPORT", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 0,rowinner=0;
            //**************
            //*******************BUYER ORDER DETAIL
            //Headers

            sht.Range("G1").Value = "CUSTOMER ORDER STATUS DETAIL";
            sht.Range("G1:K1").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("G1:K1").Style.Font.FontSize = 11;
            sht.Range("G1:K1").Style.Font.Bold = true;
            sht.Range("G1:K1").Merge();

            sht.Range("A2").Value = "Customer Code";
            sht.Range("A2:B2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A2:B2").Style.Font.FontSize = 11;
            sht.Range("A2:B2").Style.Font.Bold = true;
            sht.Range("A2:B2").Merge();

            sht.Range("C2").Value = DDCustCode.SelectedItem.Text;
            sht.Range("C2:C2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("C2:C2").Style.Font.FontSize = 11;
            sht.Range("C2:D2").Style.Font.Bold = true;
            sht.Range("C2:C2").Merge();


            sht.Range("A3").Value = "Order No";
            sht.Range("A3:B3").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A3:B3").Style.Font.FontSize = 11;
            sht.Range("A3:B3").Style.Font.Bold = true;
            sht.Range("A3:B3").Merge();

            sht.Range("C3").Value = DDOrderNo.SelectedItem.Text;
            sht.Range("C3:C3").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("C3:C3").Style.Font.FontSize = 11;
            sht.Range("C3:C3").Style.Font.Bold = true;
            sht.Range("C3:C3").Merge();


            if (ds != null)
            {

                //sht.Range("G1").Value = "CUSTOMER ORDER STATUS";
                //sht.Range("G1:H1").Style.Font.FontName = "Arial Unicode MS";
                //sht.Range("G1:H1").Style.Font.FontSize = 12;
                sht.Range("G1:H1").Style.Font.Bold = true;
                //sht.Range("G1:H1").Merge();
                sht.Range("A5:K5").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A5:K5").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A5:K5").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A5:K5").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A5").Value = "Category";
                sht.Range("B5").Value = "Technique";
                sht.Range("C5").Value = "Quality";
                sht.Range("D5").Value = "Design";
                sht.Range("E5").Value = "Color";
                sht.Range("F5").Value = "Shape";
                sht.Range("G5").Value = "Size";
                sht.Range("H5").Value = "Unit";
                sht.Range("I5").Value = "Order Qty.";
                sht.Range("J5").Value = "Extra Qty.";
                sht.Range("K5").Value = "Filler";
                sht.Range("A5:K5").Style.Font.Bold = true;
                row = 6;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CATEGORY_NAME"]); //"BUYER CODE";
                        sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]); // = "PO TYPE";
                        sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"].ToString());  //"LPO";
                        sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["DESIGNNAME"].ToString());  //"BPO";
                        sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["COLORNAME"]);  //"ORD DT";
                        sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["SHAPENAME"]);
                        sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["SIZE"]);
                        sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["UNIT"]);
                        sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["QTYREQUIRED"]);
                        sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["EXTRAQTY"]);
                        sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["FILLER"]);
                        sht.Range("A" + row + ":K" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + row + ":K" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + row + ":K" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + row + ":K" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                        row = row + 1;
                    }
                }

                /////////////////////////////////Purchase////////////////////////////////////////////////////////////////////
                row += 1;
                sht.Range("A" + row).Value = "Purchase";
                sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 11;
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;
                sht.Range("A" + row + ":B" + row).Merge();
                row += 3;
                sht.Range("A" + row + ":W" + row).Style.Font.Bold = true;
                //sht.Range("G1:H1").Merge();
                sht.Range("A" + row + ":W" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":W" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":W" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":W" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row).Value = "Category";
                sht.Range("B" + row).Value = "Po No";
                sht.Range("C" + row).Value = "Po Status";
                sht.Range("D" + row).Value = "Po Date";
                sht.Range("E" + row).Value = "Supp. Name";
                sht.Range("F" + row).Value = "Item Name";
                sht.Range("G" + row).Value = "Rate";
                sht.Range("H" + row).Value = "Req Qty";
                sht.Range("I" + row).Value = "Issued";
                sht.Range("J" + row).Value = "Req Balance";
                sht.Range("K" + row).Value = "PO Qty";
                sht.Range("L" + row).Value = "Delv Date";
                sht.Range("M" + row).Value = "CanQty/Date";
                sht.Range("N" + row).Value = "RecDate";
                sht.Range("O" + row).Value = "ChallanNo";
                sht.Range("P" + row).Value = "LotNo";
                sht.Range("Q" + row).Value = "Bill No";

                sht.Range("R" + row).Value = "Rec Qty";
                sht.Range("S" + row).Value = "Ret Date";
                sht.Range("T" + row).Value = "Ret Qty";
                sht.Range("U" + row).Value = "Pending Qty";
                sht.Range("V" + row).Value = "Receive Remark";
                sht.Range("W" + row).Value = "Order Remark";
                sht.Range("A" + row + ":W" + row).Style.Font.Bold = true;
                row += 1;
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        decimal reqbal = 0;
                        sht.Range("A" + row).SetValue(ds.Tables[1].Rows[i]["CATEGORY_NAME"]); //"BUYER CODE";
                        sht.Range("B" + row).SetValue(ds.Tables[1].Rows[i]["PO"]); // = "PO TYPE";
                        sht.Range("C" + row).SetValue(ds.Tables[1].Rows[i]["STATUS"].ToString());  //"LPO";
                        sht.Range("D" + row).SetValue(ds.Tables[1].Rows[i]["ORDERDATE"].ToString());  //"BPO";
                        sht.Range("E" + row).SetValue(ds.Tables[1].Rows[i]["EMPNAME"]);  //"ORD DT";
                        sht.Range("F" + row).SetValue(ds.Tables[1].Rows[i]["DESCRIPTION"]);
                        sht.Range("G" + row).SetValue(ds.Tables[1].Rows[i]["RATE"]);

                        sht.Range("H" + row).SetValue(ds.Tables[1].Rows[i]["consumptionqty"]);
                        sht.Range("I" + row).SetValue(ds.Tables[1].Rows[i]["ORDERQTY"]);
                        reqbal = Convert.ToDecimal(ds.Tables[1].Rows[i]["consumptionqty"]) - Convert.ToDecimal(ds.Tables[1].Rows[i]["ORDERQTY"]);
                        sht.Range("J" + row).SetValue(reqbal);

                        sht.Range("K" + row).SetValue(ds.Tables[1].Rows[i]["ORDERQTY"]);
                        sht.Range("L" + row).SetValue(ds.Tables[1].Rows[i]["DELIVERYDATE"]);
                        sht.Range("M" + row).SetValue(ds.Tables[1].Rows[i]["ORDERCANQTY"]);
                        sht.Range("N" + row).SetValue(ds.Tables[1].Rows[i]["RECDATE"]);
                        sht.Range("O" + row).SetValue(ds.Tables[1].Rows[i]["CHALLANNO"]);
                        sht.Range("P" + row).SetValue(ds.Tables[1].Rows[i]["LOTNO"]);
                        sht.Range("Q" + row).SetValue(ds.Tables[1].Rows[i]["BILLNO1"]);
                        sht.Range("R" + row).SetValue(ds.Tables[1].Rows[i]["RECQTY"]);
                        sht.Range("S" + row).SetValue(ds.Tables[1].Rows[i]["RETURNDATE"]);
                        sht.Range("T" + row).SetValue(ds.Tables[1].Rows[i]["QTYRETURN"]);
                        sht.Range("U" + row).SetValue(ds.Tables[1].Rows[i]["PENDINGQTY"]);
                        sht.Range("V" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASERECEIVEITEMREMARK"]);
                        sht.Range("W" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASEORDERMASTERREMARK"]);


                        sht.Range("A" + row + ":W" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + row + ":W" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + row + ":W" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + row + ":W" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                        row = row + 1;
                    }
                }


                row += 1;
                //sht.Range("A" + row).Value = "Process Program";
                //sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial Unicode MS";
                //sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 11;
                //sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;
                //sht.Range("A" + row + ":B" + row).Merge();
                //row += 1;
                List<int> pidpp = new List<int>();
                pidpp = ds.Tables[2].AsEnumerable().Select(a => a.Field<Int32>("PROCESS_ID")).Distinct().ToList();

                foreach (var item in pidpp)
                {

                    DataTable dtpp = new DataTable();

                    var query = (from gift in ds.Tables[2].AsEnumerable()
                                 where gift.Field<int>("PROCESS_ID") == item
                                 //  fr.price,vf.ITEM_NAME,vf.QualityName,vf.ShapeName,vf.ColorName
                                 select new

                                 {

                                     indentid = gift.Field<int?>("INDENTID"),
                                     indentdate = gift.Field<DateTime?>("INDENTDATE"),
                                     challanno = gift.Field<string>("ChallanNo"),
                                     recchallanno = gift.Field<string>("recChallanNo"),
                                     date = gift.Field<DateTime?>("date"),
                                     recdate = gift.Field<DateTime?>("recDate"),
                                     
                                     processname = gift.Field<string>("PROCESS_NAME"),
                                     shadecolor = gift.Field<string>("SHADECOLORNAME"),
                                     dyername = gift.Field<string>("DYERNAME"),
                                     // shapename = gift.Field<string>("shapename"),
                                     qualityname = gift.Field<string>("QUALITYNAME"),
                                     consmpqty = gift.Field<double>("CONSMPQTY"),
                                     indentqty = gift.Field<double>("INDENTQTY"),

                                     lossqty = gift.Field<double>("LOSSQTY"),

                                     UNIT = gift.Field<string>("UNITNAME"),
                                     reqQTY = gift.Field<double>("REQQTY"),
                                     RECQTY = gift.Field<double>("recQty"),
                                     pendingqty = gift.Field<double>("PENDINGQTY"),
                                     balqty = gift.Field<double>("BALQTY"),


                                 });

                    row += 1;
                    sht.Range("A" + row).Value = query.FirstOrDefault().processname.ToUpper(); ;
                    sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 11;
                    sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;
                    sht.Range("A" + row + ":B" + row).Merge();
                    row += 1;
                    sht.Range("A" + row + ":L" + row).Style.Font.Bold = true;
                    //sht.Range("G1:H1").Merge();
                    sht.Range("A" + row + ":L" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row + ":L" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row + ":L" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row + ":L" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row).Value = "Party Name";
                    sht.Range("B" + row).Value = "Indent Date";
                    sht.Range("C" + row).Value = "Indent No.";
                    sht.Range("D" + row).Value = "RM Issue Ch No.";
                    sht.Range("E" + row).Value = "RM Issue Date.";
                    sht.Range("F" + row).Value = "Rec Date";
                    sht.Range("G" + row).Value = "Rec Challan No.";
                    sht.Range("H" + row).Value = "Quality";
                    sht.Range("I" + row).Value = "Shade No";
                    sht.Range("J" + row).Value = "Consumption (per pc.)";
                    sht.Range("K" + row).Value = "Unit";
                    sht.Range("L" + row).Value = "Reqd. Qty.";
                    sht.Range("M" + row).Value = "Issued Qty.";
                    sht.Range("N" + row).Value = "Rcvd";
                    sht.Range("O" + row).Value = "Loss Qty.";
                    sht.Range("P" + row).Value = "Pending Qty";
                    sht.Range("Q" + row).Value = "Reqd Bal Qty";
                   // sht.Range("R" + row).Value = "JobWorker Name";
                    //  sht.Range("L" + row).Value = "Process Name";
                    //sht.Range("M" + row).Value = "LotNo";
                    //sht.Range("N" + row).Value = "Bill No";

                    //sht.Range("O" + row).Value = "Rec Qty";
                    //sht.Range("P" + row).Value = "Ret Date";
                    //sht.Range("Q" + row).Value = "Ret Qty";
                    //sht.Range("R" + row).Value = "Pending Qty";
                    //sht.Range("S" + row).Value = "Receive Remark";
                    //sht.Range("T" + row).Value = "Order Remark";
                    sht.Range("A" + row + ":R" + row).Style.Font.Bold = true;
                    row += 1;
                    if (query.Count() > 0)
                    {
                        int pendingqty = 0, balqty = 0, qty = 0, recqty = 0, orderqty = 0, sumissueqty = 0, sumrecqty = 0, sumpendinqty = 0;

                        foreach (var data in query)
                        {
                            sht.Range("A" + row).SetValue(data.dyername);
                            sht.Range("D" + row).SetValue(data.indentdate); //"BUYER CODE";
                            sht.Range("E" + row).SetValue(data.indentid); //"BUYER CODE";
                            sht.Range("F" + row).SetValue(data.recdate); //"BUYER CODE";
                            sht.Range("G" + row).SetValue(data.recchallanno); //"BUYER CODE";
                            sht.Range("H" + row).SetValue(data.qualityname); //"BUYER CODE";
                            sht.Range("I" + row).SetValue(data.shadecolor); // = "PO TYPE";
                            sht.Range("J" + row).SetValue(data.consmpqty);  //"LPO";
                            sht.Range("K" + row).SetValue(data.UNIT);  //"BPO";
                            sht.Range("L" + row).SetValue(data.reqQTY);  //"ORD DT";
                            sht.Range("M" + row).SetValue(data.indentqty);
                            sht.Range("N" + row).SetValue(data.RECQTY);
                            sht.Range("O" + row).SetValue(data.lossqty);
                            sht.Range("P" + row).SetValue(data.pendingqty);
                            sht.Range("Q" + row).SetValue(data.balqty);
                            
                            // sht.Range("L" + row).SetValue(ds.Tables[2].Rows[i]["PROCESS_NAME"]);
                            //sht.Rage("M" + row).SetValue(ds.Tables[1].Rows[i]["LOTNO"]);
                            //sht.Range("N" + row).SetValue(ds.Tables[1].Rows[i]["BILLNO1"]);
                            //sht.Range("O" + row).SetValue(ds.Tables[1].Rows[i]["RECQTY"]);
                            //sht.Range("P" + row).SetValue(ds.Tables[1].Rows[i]["RETURNDATE"]);
                            //sht.Range("Q" + row).SetValue(ds.Tables[1].Rows[i]["QTYRETURN"]);
                            //sht.Range("R" + row).SetValue(ds.Tables[1].Rows[i]["PENDINGQTY"]);
                            //sht.Range("S" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASERECEIVEITEMREMARK"]);
                            //sht.Range("T" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASEORDERMASTERREMARK"]);


                            sht.Range("A" + row + ":Q" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":Q" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":Q" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":Q" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                            row = row + 1;
                        }
                    }
                }
                row += 1;

                row += 1;
                sht.Range("A" + row).Value = "Home Furnishing Details";
                sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 11;
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;
                sht.Range("A" + row + ":B" + row).Merge();
                row += 3;
                sht.Range("A" + row + ":W" + row).Style.Font.Bold = true;
                //sht.Range("G1:H1").Merge();
                sht.Range("A" + row + ":W" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":W" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":W" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":W" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                       List<int> pihm = new List<int>();
                       List<string> pifin = new List<string>();
                       pihm = ds.Tables[3].AsEnumerable().Select(a => a.Field<Int32>("PROCESS_NAME_ID")).Distinct().ToList();
                      

                foreach (var item in pihm)
                {

                    DataTable dtpp = new DataTable();
                    DataTable dtf = new DataTable();
                    var query = (from gift in ds.Tables[3].AsEnumerable()
                                 where gift.Field<int>("PROCESS_NAME_ID") == item
                                 //  fr.price,vf.ITEM_NAME,vf.QualityName,vf.ShapeName,vf.ColorName
                                 select new
                                 {
                                     AssignDate = gift.Field<DateTime?>("AssignDate"),
                                      ReceiveDate = gift.Field<DateTime?>("ReceiveDate"),
                                     
                                     CustomerCode = gift.Field<string>("CustomerCode"),
                                     CustomerOrderNo = gift.Field<string>("CustomerOrderNo"),
                                     IssueOrderID = gift.Field<string>("IssueOrderID"),
                                     IssueDetailId = gift.Field<int>("IssueDetailId"),
                                     Order_FinishedID = gift.Field<int>("Order_FinishedID"),
                                     processname = gift.Field<string>("PROCESS_NAME"),
                                     challanno = gift.Field<string>("challanno"),
                                     QualityName = gift.Field<string>("QualityName"),
                                     DesignName = gift.Field<string>("DesignName"),
                                     // shapename = gift.Field<string>("shapename"),
                                     ColorName = gift.Field<string>("ColorName"),
                                     width = gift.Field<string>("width"),
                                     lenght = gift.Field<string>("length"),
                                     IssueQty = gift.Field<int>("IssueQty"),
                                     RecQty = gift.Field<int>("RecQty"),
                                     EMPNAME = gift.Field<string>("EMPNAME"),
                                     Checkedby = gift.Field<string>("Checkedby"),
                                     Rate = gift.Field<double>("Rate"),
                                     Amount = gift.Field<double>("Amount"),                               
                                     UserName = gift.Field<string>("UserName"),
                                     OrderQty = gift.Field<int>("ORDERQTY"),
                                     ProcessRecDetailId = gift.Field<int>("ProcessRecDetailId"),

                                 });


                    sht.Range("A" + row).Value = query.FirstOrDefault().processname.ToUpper(); ;
                    sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 11;
                    sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;
                    sht.Range("A" + row + ":B" + row).Merge();
                    row += 1;
                    sht.Range("A" + row + ":L" + row).Style.Font.Bold = true;

                    sht.Range("A" + row).Value = "Issue Date";
                    //sht.Range("B" + row).Value = "Customer Code";
                    //sht.Range("C" + row).Value = "Customer OrderNo";
                    sht.Range("B" + row).Value = "Issue ChallanNo";
                    sht.Range("C" + row).Value = "Rec Date";
                    sht.Range("D" + row).Value = "Rec ChallanNo";
                    sht.Range("E" + row).Value = "Job Name";
                    sht.Range("F" + row).Value = "Quality";
                    sht.Range("G" + row).Value = "Design";
                    sht.Range("H" + row).Value = "Color";
                    sht.Range("I" + row).Value = "Size";
                    sht.Range("J" + row).Value = "Issue Qty";
                    sht.Range("K" + row).Value = "Rec Qty";
                    sht.Range("L" + row).Value = "Pending Qty";
                    sht.Range("M" + row).Value = "Emp Name";
                    sht.Range("N" + row).Value = "Checked By";
                    sht.Range("O" + row).Value = "Rate";
                    sht.Range("P" + row).Value = "Amount";
                    sht.Range("Q" + row).Value = "User Name";
                    rowinner = row;
                    pifin = ds.Tables[3].AsEnumerable().Where(a => a.Field<Int32>("PROCESS_NAME_ID") == item).Select(a => a.Field<string>("QualityName")).Distinct().ToList();
                    int count = 0;
                    foreach (var fin in pifin)
                    {
                        var queryfin = (from gift in ds.Tables[3].AsEnumerable()
                                        where gift.Field<int>("PROCESS_NAME_ID") == item && gift.Field<string>("QualityName") == fin 
                                     //  fr.price,vf.ITEM_NAME,vf.QualityName,vf.ShapeName,vf.ColorName
                                     select new
                                     {
                                         //AssignDate = gift.Field<DateTime?>("AssignDate"),
                                         //CustomerCode = gift.Field<string>("CustomerCode"),
                                         //CustomerOrderNo = gift.Field<string>("CustomerOrderNo"),
                                         //IssueOrderID = gift.Field<string>("IssueOrderID"),
                                         IssueDetailId = gift.Field<int>("IssueDetailId"),
                                         ProcessRecDetailId = gift.Field<int>("ProcessRecDetailId"),
                                         Order_FinishedID = gift.Field<int>("Order_FinishedID"),
                                         //processname = gift.Field<string>("PROCESS_NAME"),
                                         //challanno = gift.Field<string>("challanno"),
                                         QualityName = gift.Field<string>("QualityName"),
                                      //   DesignName = gift.Field<string>("DesignName"),
                                        
                                         //ColorName = gift.Field<string>("ColorName"),
                                         //width = gift.Field<string>("width"),
                                         //lenght = gift.Field<string>("length"),
                                         IssueQty = gift.Field<int>("IssueQty"),
                                         RecQty = gift.Field<int>("RecQty"),
                                         OrderQty = gift.Field<int>("ORDERQTY"),
                                         //EMPNAME = gift.Field<string>("EMPNAME"),
                                         //Checkedby = gift.Field<string>("Checkedby"),
                                         //Rate = gift.Field<double>("Rate"),
                                         //Amount = gift.Field<double>("Amount"),
                                         //UserName = gift.Field<string>("UserName"),

                                     });
                        if (count == 0)
                        {
                            sht.Range("U" + row).Value = "Order Summary";
                            sht.Range("U" + row + ":Z" + row).Style.Font.FontName = "Arial Unicode MS";
                            sht.Range("U" + row + ":Z" + row).Style.Font.FontSize = 11;
                            sht.Range("U" + row + ":Z" + row).Style.Font.Bold = true;
                            sht.Range("U" + row + ":Z" + row).Merge();
                            rowinner += 1;
                            sht.Range("U" + row).Value = "Quality";
                            sht.Range("V" + rowinner).Value = "Qty Order";
                            //sht.Range("C" + row).Value = "Customer OrderNo";
                            sht.Range("W" + rowinner).Value = "Qty Issued";
                            sht.Range("X" + rowinner).Value = "Qty Rec.";
                            sht.Range("Y" + rowinner).Value = "Qty Pending";
                        }
                        rowinner += 1;
                        if (queryfin.Count() > 0)
                        {
                            int ISSQTY = 0,RECQTY=0,PENDINGQTY=0;
                          //  int pendingqty = 0, balqty = 0, qty = 0, recqty = 0, orderqty = 0, sumrecqty = 0, sumpendinqty = 0;
                           // double sumissueqty = 0;
                           // string issuechallan = string.Empty;

                            //foreach (var data in queryfin)
                            //{
                            ISSQTY = queryfin.GroupBy(a => a.IssueDetailId).Sum(a => a.FirstOrDefault().IssueQty);
                            RECQTY = queryfin.GroupBy(a => a.ProcessRecDetailId).Sum(a => a.FirstOrDefault().RecQty);
                            PENDINGQTY = ISSQTY - RECQTY;
                            sht.Range("U" + rowinner).SetValue(queryfin.FirstOrDefault().QualityName); //"BUYER CODE";
                            sht.Range("V" + rowinner).SetValue(query.GroupBy(a=>a.Order_FinishedID).Sum(a=>a.FirstOrDefault().OrderQty)); //"BUYER CODE";
                            sht.Range("W" + rowinner).SetValue(ISSQTY); //"BUYER CODE";
                            sht.Range("X" + rowinner).SetValue(RECQTY); //"BUYER CODE";
                            sht.Range("Y" + rowinner).SetValue(PENDINGQTY); //"BUYER CODE";


                            //}
                        }
                        rowinner++;
                        count++;
                    }
                    //sht.Range("R" + row).Value = "Rec Qty";
                    //sht.Range("S" + row).Value = "Ret Date";
                    //sht.Range("T" + row).Value = "Ret Qty";
                    //sht.Range("U" + row).Value = "Pending Qty";
                    //sht.Range("V" + row).Value = "Receive Remark";
                    //sht.Range("W" + row).Value = "Order Remark";
                    sht.Range("A" + row + ":W" + row).Style.Font.Bold = true;
                    row += 1;
                    if (query.Count() > 0)
                    {
                        int pendingqty = 0, balqty = 0, qty = 0, recqty = 0, orderqty = 0,  sumrecqty = 0, sumpendinqty = 0;
                        double sumissueqty = 0;
                        string issuechallan = string.Empty;

                        foreach (var data in query)
                        {
                            decimal reqbal = 0;

                            if (issuechallan == data.IssueDetailId.ToString())
                            {
                                if (sumissueqty == 0)
                                {
                                    sumissueqty = data.IssueQty;
                                }
                                else
                                {
                                    sumissueqty = sumissueqty - Convert.ToDouble(data.RecQty);
                                }
                              
                            
                            }
                            else {

                                sumissueqty = data.IssueQty - Convert.ToDouble(data.RecQty);
                            }
                            sht.Range("A" + row).SetValue(data.AssignDate); //"BUYER CODE";
                            //sht.Range("B" + row).SetValue(data.CustomerCode); // = "PO TYPE";
                            //sht.Range("C" + row).SetValue(data.CustomerOrderNo.ToString());  //"LPO";
                            sht.Range("B" + row).SetValue(data.IssueOrderID.ToString());  //"BPO";
                            sht.Range("C" + row).SetValue(data.ReceiveDate);  //"BPO";
                            sht.Range("D" + row).SetValue(data.challanno.ToString());  //"BPO";
                            sht.Range("E" + row).SetValue(data.processname);  //"ORD DT";
                            sht.Range("F" + row).SetValue(data.QualityName);
                            sht.Range("G" + row).SetValue(data.DesignName);

                            sht.Range("H" + row).SetValue(data.ColorName);
                            sht.Range("I" + row).SetValue(data.width.ToString() + 'x' + data.lenght);
                            //  reqbal = Convert.ToDecimal(data.consumptionqty"]) - Convert.ToDecimal(data.ORDERQTY"]);
                            // sht.Range("J" + row).SetValue(reqbal);

                            sht.Range("J" + row).SetValue(data.IssueQty);
                            sht.Range("K" + row).SetValue(data.RecQty);
                            sht.Range("L" + row).SetValue(sumissueqty);
                            sht.Range("M" + row).SetValue(data.EMPNAME);
                            sht.Range("N" + row).SetValue(data.Checkedby);
                            sht.Range("O" + row).SetValue(data.Rate);
                            sht.Range("P" + row).SetValue(data.Amount);
                            sht.Range("Q" + row).SetValue(data.UserName);
                            //sht.Range("S" + row).SetValue(ds.Tables[3].Rows[i]["RETURNDATE"]);
                            //sht.Range("T" + row).SetValue(ds.Tables[3].Rows[i]["QTYRETURN"]);
                            //sht.Range("U" + row).SetValue(ds.Tables[3].Rows[i]["PENDINGQTY"]);
                            //sht.Range("V" + row).SetValue(ds.Tables[3].Rows[i]["PURCHASERECEIVEITEMREMARK"]);
                            //sht.Range("W" + row).SetValue(ds.Tables[3].Rows[i]["PURCHASEORDERMASTERREMARK"]);


                            sht.Range("A" + row + ":W" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":W" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":W" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":W" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            if (issuechallan != data.IssueDetailId.ToString())
                            {
                                issuechallan = data.IssueDetailId.ToString();
                            }
                          //  sumissueqty = sumissueqty - Convert.ToDouble(data.RecQty);
                            row = row + 1;
                        }
                    }
                   // count++;
                }
                row += 1;



                //List<int> pidcutting = new List<int>();
                //pidcutting = ds.Tables[3].AsEnumerable().Select(a => a.Field<Int32>("PROCESS_NAME_ID")).Distinct().ToList();

                //foreach (var item in pidcutting)
                //{
                //    DataTable dtcutting = new DataTable();

                //    var query = (from gift in ds.Tables[3].AsEnumerable()
                //                 where gift.Field<int>("PROCESS_NAME_ID") == item
                //                 //  fr.price,vf.ITEM_NAME,vf.QualityName,vf.ShapeName,vf.ColorName
                //                 select new
                //                 {
                //                     processname = gift.Field<string>("process_name"),
                //                     itemname = gift.Field<string>("item_name"),
                //                     colorname = gift.Field<string>("colorname"),
                //                     // shapename = gift.Field<string>("shapename"),
                //                     qualityname = gift.Field<string>("qualityname"),
                //                     cateogoryname = gift.Field<string>("category_name"),
                //                     designname = gift.Field<string>("designname"),

                //                     size = gift.Field<string>("size"),
                //                     // width = gift.Field<string>("WIDTH"),
                //                     //height = gift.Field<Double?>("heightinch"),
                //                     UNIT = gift.Field<string>("UNITNAME"),
                //                     QTY = gift.Field<int>("qty"),
                //                     RECQTY = gift.Field<int>("recQty"),
                //                     ISSUEQTY = gift.Field<int>("QtyOrder"),


                //                 });



                //    row += 1;
                //    sht.Range("A" + row).Value = query.FirstOrDefault().processname.ToUpper();
                //    sht.Range("A" + row + ":C" + row).Style.Font.FontName = "Arial Unicode MS";
                //    sht.Range("A" + row + ":C" + row).Style.Font.FontSize = 11;
                //    sht.Range("A" + row + ":C" + row).Style.Font.Bold = true;
                //    sht.Range("A" + row + ":C" + row).Merge();
                //    row += 1;
                //    sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;
                //    //sht.Range("G1:H1").Merge();
                //    sht.Range("A" + row + ":J" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //    sht.Range("A" + row + ":J" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //    sht.Range("A" + row + ":J" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //    sht.Range("A" + row + ":J" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //    sht.Range("A" + row).Value = "Components";
                //    sht.Range("B" + row).Value = "Design";
                //    sht.Range("C" + row).Value = "Color";
                //    sht.Range("D" + row).Value = "Size";
                //    sht.Range("E" + row).Value = "Unit";
                //    sht.Range("F" + row).Value = "Order Qty.";
                //    sht.Range("G" + row).Value = "Issued";
                //    sht.Range("H" + row).Value = "Rcvd";
                //    sht.Range("I" + row).Value = "Pending Qty";
                //    sht.Range("J" + row).Value = "Reqd Bal Qty";
                //    // sht.Range("K" + row).Value = "JOBWORKER NAME";
                //    //sht.Range("L" + row).Value = "ChallanNo";
                //    //sht.Range("M" + row).Value = "LotNo";
                //    //sht.Range("N" + row).Value = "Bill No";

                //    //sht.Range("O" + row).Value = "Rec Qty";
                //    //sht.Range("P" + row).Value = "Ret Date";
                //    //sht.Range("Q" + row).Value = "Ret Qty";
                //    //sht.Range("R" + row).Value = "Pending Qty";
                //    //sht.Range("S" + row).Value = "Receive Remark";
                //    //sht.Range("T" + row).Value = "Order Remark";
                //    sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;
                //    row += 1;
                //    if (query.Count() > 0)
                //    {
                //        Int32 pendingqty = 0, balqty = 0, qty = 0, recqty = 0, orderqty = 0, sumissueqty = 0, sumrecqty = 0, sumpendinqty = 0;

                //        foreach (var data in query)
                //        {
                //            qty = Convert.ToInt32(data.QTY);
                //            recqty = Convert.ToInt16(data.RECQTY);
                //            orderqty = Convert.ToInt16(data.ISSUEQTY);
                //            pendingqty = qty - recqty;
                //            balqty = orderqty - qty;

                //            sht.Range("A" + row).SetValue(data.qualityname); //"BUYER CODE";
                //            sht.Range("B" + row).SetValue(data.designname); // = "PO TYPE";
                //            sht.Range("C" + row).SetValue(data.colorname);  //"LPO";
                //            sht.Range("D" + row).SetValue(data.size);  //"BPO";
                //            sht.Range("E" + row).SetValue(data.UNIT);  //"ORD DT";
                //            sht.Range("F" + row).SetValue(data.ISSUEQTY);
                //            sht.Range("G" + row).SetValue(data.QTY);
                //            sht.Range("H" + row).SetValue(data.RECQTY);
                //            sht.Range("I" + row).SetValue(pendingqty);
                //            sht.Range("J" + row).SetValue(balqty);
                //            //sht.Range("K" + row).SetValue(ds.Tables[3].Rows[i]["DYERNAME"]);
                //            //sht.Range("L" + row).SetValue(ds.Tables[1].Rows[i]["CHALLANNO"]);
                //            //sht.Range("M" + row).SetValue(ds.Tables[1].Rows[i]["LOTNO"]);
                //            //sht.Range("N" + row).SetValue(ds.Tables[1].Rows[i]["BILLNO1"]);
                //            //sht.Range("O" + row).SetValue(ds.Tables[1].Rows[i]["RECQTY"]);
                //            //sht.Range("P" + row).SetValue(ds.Tables[1].Rows[i]["RETURNDATE"]);
                //            //sht.Range("Q" + row).SetValue(ds.Tables[1].Rows[i]["QTYRETURN"]);
                //            //sht.Range("R" + row).SetValue(ds.Tables[1].Rows[i]["PENDINGQTY"]);
                //            //sht.Range("S" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASERECEIVEITEMREMARK"]);
                //            //sht.Range("T" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASEORDERMASTERREMARK"]);


                //            sht.Range("A" + row + ":J" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //            sht.Range("A" + row + ":J" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //            sht.Range("A" + row + ":J" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //            sht.Range("A" + row + ":J" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                //            row = row + 1;
                //        }
                //    }
                //}
                row += 1;
                sht.Range("A" + row).Value = "Raw Material Issue for Cutting";
                sht.Range("A" + row + ":D" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A" + row + ":D" + row).Style.Font.FontSize = 11;
                sht.Range("A" + row + ":D" + row).Style.Font.Bold = true;
                sht.Range("A" + row + ":D" + row).Merge();
                row += 1;
                sht.Range("A" + row + ":H" + row).Style.Font.Bold = true;
                //sht.Range("G1:H1").Merge();
                sht.Range("A" + row + ":H" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":H" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":H" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":H" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row).Value = "Quality";
                sht.Range("B" + row).Value = "Shade Color";
                sht.Range("C" + row).Value = "Consumption (per pc.)";
                sht.Range("D" + row).Value = "Unit";
                sht.Range("E" + row).Value = "Reqd. Qty.";
                sht.Range("F" + row).Value = "Issue Qty";
                sht.Range("G" + row).Value = "Bal to Issue";
                sht.Range("H" + row).Value = "Jobworker Name";
                //sht.Range("I" + row).Value = "Pending Qty";
                //sht.Range("J" + row).Value = "Reqd Bal Qty";
                // sht.Range("K" + row).Value = "JOBWORKER NAME";
                //sht.Range("L" + row).Value = "ChallanNo";
                //sht.Range("M" + row).Value = "LotNo";
                //sht.Range("N" + row).Value = "Bill No";

                //sht.Range("O" + row).Value = "Rec Qty";
                //sht.Range("P" + row).Value = "Ret Date";
                //sht.Range("Q" + row).Value = "Ret Qty";
                //sht.Range("R" + row).Value = "Pending Qty";
                //sht.Range("S" + row).Value = "Receive Remark";
                //sht.Range("T" + row).Value = "Order Remark";
                sht.Range("A" + row + ":H" + row).Style.Font.Bold = true;
                row += 1;
                if (ds.Tables[4].Rows.Count > 0)
                {
                    int pendingqty = 0, balqty = 0, qty = 0, recqty = 0, orderqty = 0;

                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        qty = Convert.ToInt16(ds.Tables[4].Rows[i]["REQQTY"]);
                        recqty = Convert.ToInt16(ds.Tables[4].Rows[i]["ISSQTY"]);
                        // orderqty = Convert.ToInt16(ds.Tables[3].Rows[i]["qtyorder"]);
                        pendingqty = qty - recqty;
                        //  balqty = orderqty - qty;

                        sht.Range("A" + row).SetValue(ds.Tables[4].Rows[i]["QUALITYNAME"]); //"BUYER CODE";
                        sht.Range("B" + row).SetValue(ds.Tables[4].Rows[i]["SHADECOLORNAME"]); // = "PO TYPE";
                        sht.Range("C" + row).SetValue(ds.Tables[4].Rows[i]["CONSMPQTY"].ToString());  //"LPO";
                        sht.Range("D" + row).SetValue(ds.Tables[4].Rows[i]["UNIT"].ToString());  //"BPO";
                        sht.Range("E" + row).SetValue(ds.Tables[4].Rows[i]["REQQTY"]);  //"ORD DT";
                        sht.Range("F" + row).SetValue(ds.Tables[4].Rows[i]["ISSQTY"]);
                        sht.Range("G" + row).SetValue(pendingqty);
                        sht.Range("H" + row).SetValue("");
                        //sht.Range("I" + row).SetValue(pendingqty);
                        //sht.Range("J" + row).SetValue(balqty);
                        //sht.Range("K" + row).SetValue(ds.Tables[3].Rows[i]["DYERNAME"]);
                        //sht.Range("L" + row).SetValue(ds.Tables[1].Rows[i]["CHALLANNO"]);
                        //sht.Range("M" + row).SetValue(ds.Tables[1].Rows[i]["LOTNO"]);
                        //sht.Range("N" + row).SetValue(ds.Tables[1].Rows[i]["BILLNO1"]);
                        //sht.Range("O" + row).SetValue(ds.Tables[1].Rows[i]["RECQTY"]);
                        //sht.Range("P" + row).SetValue(ds.Tables[1].Rows[i]["RETURNDATE"]);
                        //sht.Range("Q" + row).SetValue(ds.Tables[1].Rows[i]["QTYRETURN"]);
                        //sht.Range("R" + row).SetValue(ds.Tables[1].Rows[i]["PENDINGQTY"]);
                        //sht.Range("S" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASERECEIVEITEMREMARK"]);
                        //sht.Range("T" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASEORDERMASTERREMARK"]);


                        sht.Range("A" + row + ":H" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + row + ":H" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + row + ":H" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + row + ":H" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                        row = row + 1;
                    }
                }

                row += 1;
                sht.Range("A" + row).Value = "Raw Material Issue for Stitching";
                sht.Range("A" + row + ":D" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A" + row + ":D" + row).Style.Font.FontSize = 11;
                sht.Range("A" + row + ":D" + row).Style.Font.Bold = true;
                sht.Range("A" + row + ":D" + row).Merge();
                row += 1;
                sht.Range("A" + row + ":H" + row).Style.Font.Bold = true;
                //sht.Range("G1:H1").Merge();
                sht.Range("A" + row + ":H" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":H" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":H" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":H" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row).Value = "Quality";
                sht.Range("B" + row).Value = "Shade Color";
                sht.Range("C" + row).Value = "Consumption (per pc.)";
                sht.Range("D" + row).Value = "Unit";
                sht.Range("E" + row).Value = "Reqd. Qty.";
                sht.Range("F" + row).Value = "Issue Qty";
                sht.Range("G" + row).Value = "Bal to Issue";
                sht.Range("H" + row).Value = "Jobworker Name";
                //sht.Range("I" + row).Value = "Pending Qty";
                //sht.Range("J" + row).Value = "Reqd Bal Qty";
                // sht.Range("K" + row).Value = "JOBWORKER NAME";
                //sht.Range("L" + row).Value = "ChallanNo";
                //sht.Range("M" + row).Value = "LotNo";
                //sht.Range("N" + row).Value = "Bill No";

                //sht.Range("O" + row).Value = "Rec Qty";
                //sht.Range("P" + row).Value = "Ret Date";
                //sht.Range("Q" + row).Value = "Ret Qty";
                //sht.Range("R" + row).Value = "Pending Qty";
                //sht.Range("S" + row).Value = "Receive Remark";
                //sht.Range("T" + row).Value = "Order Remark";
                sht.Range("A" + row + ":H" + row).Style.Font.Bold = true;
                row += 1;
                if (ds.Tables[6].Rows.Count > 0)
                {
                    int pendingqty = 0, balqty = 0, qty = 0, recqty = 0, orderqty = 0;

                    for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                    {
                        qty = Convert.ToInt16(ds.Tables[6].Rows[i]["REQQTY"]);
                        recqty = Convert.ToInt16(ds.Tables[6].Rows[i]["ISSQTY"]);
                        // orderqty = Convert.ToInt16(ds.Tables[3].Rows[i]["qtyorder"]);
                        pendingqty = qty - recqty;
                        //  balqty = orderqty - qty;

                        sht.Range("A" + row).SetValue(ds.Tables[6].Rows[i]["QUALITYNAME"]); //"BUYER CODE";
                        sht.Range("B" + row).SetValue(ds.Tables[6].Rows[i]["SHADECOLORNAME"]); // = "PO TYPE";
                        sht.Range("C" + row).SetValue(ds.Tables[6].Rows[i]["CONSMPQTY"].ToString());  //"LPO";
                        sht.Range("D" + row).SetValue(ds.Tables[6].Rows[i]["UNIT"].ToString());  //"BPO";
                        sht.Range("E" + row).SetValue(ds.Tables[6].Rows[i]["REQQTY"]);  //"ORD DT";
                        sht.Range("F" + row).SetValue(ds.Tables[6].Rows[i]["ISSQTY"]);
                        sht.Range("G" + row).SetValue(pendingqty);
                        sht.Range("H" + row).SetValue("");
                        //sht.Range("I" + row).SetValue(pendingqty);
                        //sht.Range("J" + row).SetValue(balqty);
                        //sht.Range("K" + row).SetValue(ds.Tables[3].Rows[i]["DYERNAME"]);
                        //sht.Range("L" + row).SetValue(ds.Tables[1].Rows[i]["CHALLANNO"]);
                        //sht.Range("M" + row).SetValue(ds.Tables[1].Rows[i]["LOTNO"]);
                        //sht.Range("N" + row).SetValue(ds.Tables[1].Rows[i]["BILLNO1"]);
                        //sht.Range("O" + row).SetValue(ds.Tables[1].Rows[i]["RECQTY"]);
                        //sht.Range("P" + row).SetValue(ds.Tables[1].Rows[i]["RETURNDATE"]);
                        //sht.Range("Q" + row).SetValue(ds.Tables[1].Rows[i]["QTYRETURN"]);
                        //sht.Range("R" + row).SetValue(ds.Tables[1].Rows[i]["PENDINGQTY"]);
                        //sht.Range("S" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASERECEIVEITEMREMARK"]);
                        //sht.Range("T" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASEORDERMASTERREMARK"]);


                        sht.Range("A" + row + ":H" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + row + ":H" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + row + ":H" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + row + ":H" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                        row = row + 1;
                    }
                }

                row += 1;
                sht.Range("A" + row).Value = "Raw Material Issue for Packing";
                sht.Range("A" + row + ":D" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A" + row + ":D" + row).Style.Font.FontSize = 11;
                sht.Range("A" + row + ":D" + row).Style.Font.Bold = true;
                sht.Range("A" + row + ":D" + row).Merge();
                row += 1;
                sht.Range("A" + row + ":H" + row).Style.Font.Bold = true;
                //sht.Range("G1:H1").Merge();
                sht.Range("A" + row + ":H" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":H" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":H" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":H" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row).Value = "Quality";
                sht.Range("B" + row).Value = "Shade Color";
                sht.Range("C" + row).Value = "Consumption (per pc.)";
                sht.Range("D" + row).Value = "Unit";
                sht.Range("E" + row).Value = "Reqd. Qty.";
                sht.Range("F" + row).Value = "Issue Qty";
                sht.Range("G" + row).Value = "Bal to Issue";
                sht.Range("H" + row).Value = "Jobworker Name";
                //sht.Range("I" + row).Value = "Pending Qty";
                //sht.Range("J" + row).Value = "Reqd Bal Qty";
                // sht.Range("K" + row).Value = "JOBWORKER NAME";
                //sht.Range("L" + row).Value = "ChallanNo";
                //sht.Range("M" + row).Value = "LotNo";
                //sht.Range("N" + row).Value = "Bill No";

                //sht.Range("O" + row).Value = "Rec Qty";
                //sht.Range("P" + row).Value = "Ret Date";
                //sht.Range("Q" + row).Value = "Ret Qty";
                //sht.Range("R" + row).Value = "Pending Qty";
                //sht.Range("S" + row).Value = "Receive Remark";
                //sht.Range("T" + row).Value = "Order Remark";
                sht.Range("A" + row + ":H" + row).Style.Font.Bold = true;
                row += 1;
                if (ds.Tables[7].Rows.Count > 0)
                {
                    int pendingqty = 0, balqty = 0, qty = 0, recqty = 0, orderqty = 0;

                    for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                    {
                        qty = Convert.ToInt16(ds.Tables[7].Rows[i]["REQQTY"]);
                        recqty = Convert.ToInt16(ds.Tables[7].Rows[i]["ISSQTY"]);
                        // orderqty = Convert.ToInt16(ds.Tables[3].Rows[i]["qtyorder"]);
                        pendingqty = qty - recqty;
                        //  balqty = orderqty - qty;

                        sht.Range("A" + row).SetValue(ds.Tables[7].Rows[i]["QUALITYNAME"]); //"BUYER CODE";
                        sht.Range("B" + row).SetValue(ds.Tables[7].Rows[i]["SHADECOLORNAME"]); // = "PO TYPE";
                        sht.Range("C" + row).SetValue(ds.Tables[7].Rows[i]["CONSMPQTY"].ToString());  //"LPO";
                        sht.Range("D" + row).SetValue(ds.Tables[7].Rows[i]["UNIT"].ToString());  //"BPO";
                        sht.Range("E" + row).SetValue(ds.Tables[7].Rows[i]["REQQTY"]);  //"ORD DT";
                        sht.Range("F" + row).SetValue(ds.Tables[7].Rows[i]["ISSQTY"]);
                        sht.Range("G" + row).SetValue(pendingqty);
                        sht.Range("H" + row).SetValue("");
                        //sht.Range("I" + row).SetValue(pendingqty);
                        //sht.Range("J" + row).SetValue(balqty);
                        //sht.Range("K" + row).SetValue(ds.Tables[3].Rows[i]["DYERNAME"]);
                        //sht.Range("L" + row).SetValue(ds.Tables[1].Rows[i]["CHALLANNO"]);
                        //sht.Range("M" + row).SetValue(ds.Tables[1].Rows[i]["LOTNO"]);
                        //sht.Range("N" + row).SetValue(ds.Tables[1].Rows[i]["BILLNO1"]);
                        //sht.Range("O" + row).SetValue(ds.Tables[1].Rows[i]["RECQTY"]);
                        //sht.Range("P" + row).SetValue(ds.Tables[1].Rows[i]["RETURNDATE"]);
                        //sht.Range("Q" + row).SetValue(ds.Tables[1].Rows[i]["QTYRETURN"]);
                        //sht.Range("R" + row).SetValue(ds.Tables[1].Rows[i]["PENDINGQTY"]);
                        //sht.Range("S" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASERECEIVEITEMREMARK"]);
                        //sht.Range("T" + row).SetValue(ds.Tables[1].Rows[i]["PURCHASEORDERMASTERREMARK"]);


                        sht.Range("A" + row + ":H" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + row + ":H" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + row + ":H" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + row + ":H" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                        row = row + 1;
                    }
                }




                List<int> pid = new List<int>();
                pid = ds.Tables[5].AsEnumerable().Select(a => a.Field<Int32>("processid")).Distinct().ToList();

                foreach (var item in pid)
                {

                    DataTable dtfinal = new DataTable();

                    var query = (from gift in ds.Tables[5].AsEnumerable()
                                 where gift.Field<int>("processid") == item
                                 //  fr.price,vf.ITEM_NAME,vf.QualityName,vf.ShapeName,vf.ColorName
                                 select new
                                 {
                                     processname = gift.Field<string>("process_name"),
                                     itemname = gift.Field<string>("item_name"),
                                     colorname = gift.Field<string>("colorname"),
                                     shapename = gift.Field<string>("shapename"),
                                     qualityname = gift.Field<string>("qualityname"),
                                     cateogoryname = gift.Field<string>("category_name"),
                                     designname = gift.Field<string>("designname"),

                                     length = gift.Field<string>("LENGTH"),
                                     width = gift.Field<string>("WIDTH"),
                                     //height = gift.Field<Double?>("heightinch"),
                                     UNIT = gift.Field<string>("UNITNAME"),
                                     RECQTY = gift.Field<int>("RECEIVEQTY"),
                                     ISSUEQTY = gift.Field<int>("ISSUEQTY"),
                                     Checkedby = gift.Field<string>("Checkedby"),
                                     Rate = gift.Field<double>("Rate"),
                                     Amount = gift.Field<double>("Amount"),
                                     UserName = gift.Field<string>("UserName"),
                                     OrderQty = gift.Field<int>("ORDERQTY"),
                                     RECDETAILID = gift.Field<int>("RECDETAILID"),
                                     ISSUEDETAILID = gift.Field<int>("ISSUEDETAILID"),
                                     ISSUECHALLAN = gift.Field<string>("ISSUECHALLAN"),
                                     RECHALLAN = gift.Field<string>("RECHALLAN"),
                                     PARTYCHALLAN = gift.Field<string>("PARTYCHALLAN"),
                                     WEIGHT = gift.Field<double>("WEIGHT"),
                                     ASSIGNDATE = gift.Field<DateTime>("ASSIGNDATE"),
                                     RECEIVEDATE = gift.Field<DateTime>("RECEIVEDATE"),
                                     EMPNAME = gift.Field<string>("EMPNAME"),


                                 });


                    row += 1;
                    sht.Range("A" + row).Value = (query.FirstOrDefault().processname.ToUpper() == "CUTTING") ? "Purchase" : query.FirstOrDefault().processname;
                    sht.Range("A" + row + ":D" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":D" + row).Style.Font.FontSize = 11;
                    sht.Range("A" + row + ":D" + row).Style.Font.Bold = true;
                    sht.Range("A" + row + ":D" + row).Merge();
                    row += 3;
                    sht.Range("A" + row + ":K" + row).Style.Font.Bold = true;
                    //sht.Range("G1:H1").Merge();
                    sht.Range("A" + row + ":K" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row + ":K" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row + ":K" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    sht.Range("A" + row + ":K" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                    sht.Range("A" + row).Value = "Vendor Name";
                    sht.Range("B" + row).Value = "Issue Date";
                    //sht.Range("B" + row).Value = "Customer Code";
                    //sht.Range("C" + row).Value = "Customer OrderNo";
                    sht.Range("C" + row).Value = "Issue ChallanNo";
                    sht.Range("D" + row).Value = "Rec Date";
                    sht.Range("E" + row).Value = "Rec ChallanNo";
                    sht.Range("F" + row).Value = "Party ChallanNo";
                    sht.Range("G" + row).Value = "Job Name";
                    sht.Range("H" + row).Value = "Order Desc.";
                   // sht.Range("F" + row).Value = "Design";
                    sht.Range("I" + row).Value = "Color";
                    sht.Range("J" + row).Value = "Size";
                    sht.Range("K" + row).Value = "Issue Qty";
                    sht.Range("L" + row).Value = "Rec Qty";
                    sht.Range("M" + row).Value = "Pending Qty";
                    sht.Range("N" + row).Value = "Weight";
                    sht.Range("O" + row).Value = "Checked By";
                    sht.Range("P" + row).Value = "Rate";
                    sht.Range("Q" + row).Value = "Amount";
                    sht.Range("R" + row).Value = "User Name";
                    sht.Range("A" + row + ":K" + row).Style.Font.Bold = true;
                    row += 1;
                    rowinner = row;
                    pifin = ds.Tables[5].AsEnumerable().Where(a => a.Field<Int32>("PROCESSID") == item).Select(a => a.Field<string>("QualityName")).Distinct().ToList();
                    int count = 0;
                    foreach (var fin in pifin)
                    {
                        var queryfin = (from gift in ds.Tables[5].AsEnumerable()
                                        where gift.Field<int>("PROCESSID") == item && gift.Field<string>("QualityName") == fin
                                        //  fr.price,vf.ITEM_NAME,vf.QualityName,vf.ShapeName,vf.ColorName
                                        select new
                                        {
                                            //AssignDate = gift.Field<DateTime?>("AssignDate"),
                                            //CustomerCode = gift.Field<string>("CustomerCode"),
                                            //CustomerOrderNo = gift.Field<string>("CustomerOrderNo"),
                                            //IssueOrderID = gift.Field<string>("IssueOrderID"),
                                            IssueDetailId = gift.Field<int>("ISSUEDETAILID"),
                                            ProcessRecDetailId = gift.Field<int>("RECDETAILID"),
                                           // Order_FinishedID = gift.Field<int>("Order_FinishedID"),
                                            //processname = gift.Field<string>("PROCESS_NAME"),
                                            //challanno = gift.Field<string>("challanno"),
                                            QualityName = gift.Field<string>("QualityName"),
                                            //   DesignName = gift.Field<string>("DesignName"),

                                            //ColorName = gift.Field<string>("ColorName"),
                                            //width = gift.Field<string>("width"),
                                            //lenght = gift.Field<string>("length"),
                                            IssueQty = gift.Field<int>("IssueQty"),
                                            RecQty = gift.Field<int>("RECEIVEQTY"),
                                            OrderQty = gift.Field<int>("ORDERQTY"),
                                            //EMPNAME = gift.Field<string>("EMPNAME"),
                                            //Checkedby = gift.Field<string>("Checkedby"),
                                            //Rate = gift.Field<double>("Rate"),
                                            //Amount = gift.Field<double>("Amount"),
                                            //UserName = gift.Field<string>("UserName"),

                                        });
                        if (count == 0)
                        {
                            sht.Range("U" + row).Value = "Order Summary";
                            sht.Range("U" + row + ":Z" + row).Style.Font.FontName = "Arial Unicode MS";
                            sht.Range("U" + row + ":Z" + row).Style.Font.FontSize = 11;
                            sht.Range("U" + row + ":Z" + row).Style.Font.Bold = true;
                            sht.Range("U" + row + ":Z" + row).Merge();
                            rowinner += 1;
                            sht.Range("U" + row).Value = "Quality";
                            sht.Range("V" + rowinner).Value = "Qty Order";
                            //sht.Range("C" + row).Value = "Customer OrderNo";
                            sht.Range("W" + rowinner).Value = "Qty Issued";
                            sht.Range("X" + rowinner).Value = "Qty Rec.";
                            sht.Range("Y" + rowinner).Value = "Qty Pending";
                        }
                        rowinner += 1;
                        if (queryfin.Count() > 0)
                        {
                            int ISSQTY = 0, RECQTY = 0, PENDINGQTY = 0;
                            //  int pendingqty = 0, balqty = 0, qty = 0, recqty = 0, orderqty = 0, sumrecqty = 0, sumpendinqty = 0;
                            // double sumissueqty = 0;
                            // string issuechallan = string.Empty;

                            //foreach (var data in queryfin)
                            //{
                            ISSQTY = queryfin.GroupBy(a => a.IssueDetailId).Sum(a => a.FirstOrDefault().IssueQty);
                            RECQTY = queryfin.GroupBy(a => a.ProcessRecDetailId).Sum(a => a.FirstOrDefault().RecQty);
                            PENDINGQTY = ISSQTY - RECQTY;
                            sht.Range("U" + rowinner).SetValue(queryfin.FirstOrDefault().QualityName); //"BUYER CODE";
                            sht.Range("V" + rowinner).SetValue(query.GroupBy(a => a.qualityname).Sum(a => a.FirstOrDefault().OrderQty)); //"BUYER CODE";
                            sht.Range("W" + rowinner).SetValue(ISSQTY); //"BUYER CODE";
                            sht.Range("X" + rowinner).SetValue(RECQTY); //"BUYER CODE";
                            sht.Range("Y" + rowinner).SetValue(PENDINGQTY); //"BUYER CODE";


                            //}
                        }
                        rowinner++;
                        count++;
                    }

                    row += 1;
                    if (query.Count() > 0)
                    {
                        int pendingqty = 0, balqty = 0, qty = 0, recqty = 0, orderqty = 0, sumrecqty = 0, sumpendinqty = 0;
                        double sumissueqty = 0;
                        string issuechallan = string.Empty;

                        foreach (var data in query)
                        {
                            decimal reqbal = 0;

                            if (issuechallan == data.ISSUEDETAILID.ToString())
                            {
                                if (sumissueqty == 0)
                                {
                                    sumissueqty = data.ISSUEQTY;
                                }
                                else
                                {
                                    sumissueqty = sumissueqty - Convert.ToDouble(data.RECQTY);
                                }


                            }
                            else
                            {

                                sumissueqty = data.ISSUEQTY - Convert.ToDouble(data.RECQTY);
                            }
                            sht.Range("A" + row).SetValue(data.EMPNAME); //"BUYER CODE";
                            sht.Range("B" + row).SetValue(data.ASSIGNDATE); //"BUYER CODE";
                            //sht.Range("B" + row).SetValue(data.CustomerCode); // = "PO TYPE";
                            //sht.Range("C" + row).SetValue(data.CustomerOrderNo.ToString());  //"LPO";
                            sht.Range("C" + row).SetValue(data.ISSUECHALLAN.ToString());  //"BPO";
                            sht.Range("D" + row).SetValue(data.RECEIVEDATE);  //"BPO";
                            sht.Range("E" + row).SetValue(data.RECHALLAN.ToString());  //"BPO";
                            sht.Range("F" + row).SetValue("");  //"BPO";
                            sht.Range("G" + row).SetValue(data.processname);  //"ORD DT";
                            sht.Range("H" + row).SetValue(data.qualityname.ToString() + " " + data.designname);
                         //   sht.Range("F" + row).SetValue(data.designname);

                            sht.Range("I" + row).SetValue(data.colorname);
                            sht.Range("J" + row).SetValue(data.width.ToString() + 'x' + data.length);
                            //  reqbal = Convert.ToDecimal(data.consumptionqty"]) - Convert.ToDecimal(data.ORDERQTY"]);
                            // sht.Range("J" + row).SetValue(reqbal);

                            sht.Range("K" + row).SetValue(data.ISSUEQTY);
                            sht.Range("L" + row).SetValue(data.RECQTY);
                            sht.Range("M" + row).SetValue(sumissueqty);
                            sht.Range("N" + row).SetValue(data.WEIGHT);
                            sht.Range("O" + row).SetValue(data.Checkedby);
                            sht.Range("P" + row).SetValue(data.Rate);
                            sht.Range("Q" + row).SetValue(data.Amount);
                            sht.Range("R" + row).SetValue(data.UserName);
                            //sht.Range("S" + row).SetValue(ds.Tables[3].Rows[i]["RETURNDATE"]);
                            //sht.Range("T" + row).SetValue(ds.Tables[3].Rows[i]["QTYRETURN"]);
                            //sht.Range("U" + row).SetValue(ds.Tables[3].Rows[i]["PENDINGQTY"]);
                            //sht.Range("V" + row).SetValue(ds.Tables[3].Rows[i]["PURCHASERECEIVEITEMREMARK"]);
                            //sht.Range("W" + row).SetValue(ds.Tables[3].Rows[i]["PURCHASEORDERMASTERREMARK"]);


                            sht.Range("A" + row + ":W" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":W" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":W" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":W" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            if (issuechallan != data.ISSUEDETAILID.ToString())
                            {
                                issuechallan = data.ISSUEDETAILID.ToString();
                            }
                            //  sumissueqty = sumissueqty - Convert.ToDouble(data.RecQty);
                            row = row + 1;
                        }
                    }

                }


                row += 1;
                sht.Range("A" + row).Value = "Tassle Details";
                sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 11;
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;
                sht.Range("A" + row + ":B" + row).Merge();
                row += 3;
                sht.Range("A" + row + ":W" + row).Style.Font.Bold = true;
                //sht.Range("G1:H1").Merge();
                sht.Range("A" + row + ":W" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":W" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":W" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":W" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                List<int> pitsl = new List<int>();
               // List<string> pitsl = new List<string>();
                pitsl = ds.Tables[8].AsEnumerable().Select(a => a.Field<Int32>("PROCESS_NAME_ID")).Distinct().ToList();


                foreach (var item in pitsl)
                {

                    DataTable dtpp = new DataTable();
                  //  DataTable dtf = new DataTable();
                    var query = (from gift in ds.Tables[8].AsEnumerable()
                                 where gift.Field<int>("PROCESS_NAME_ID") == item
                                 //  fr.price,vf.ITEM_NAME,vf.QualityName,vf.ShapeName,vf.ColorName
                                 select new
                                 {
                                     AssignDate = gift.Field<DateTime?>("IssueDate"),
                                     ReceiveDate = gift.Field<DateTime?>("ReceiveDate"),

                                     //CustomerCode = gift.Field<string>("CustomerCode"),
                                     //CustomerOrderNo = gift.Field<string>("CustomerOrderNo"),
                                     issuechallan = gift.Field<string>("issuechallan"),
                                     IssueDetailId = gift.Field<int>("IssueDetailId"),
                                  //   Order_FinishedID = gift.Field<int>("Order_FinishedID"),
                                     processname = gift.Field<string>("PROCESS_NAME"),
                                     challanno = gift.Field<string>("challanno"),
                                     QualityName = gift.Field<string>("QualityName"),
                                     DesignName = gift.Field<string>("DesignName"),
                                     // shapename = gift.Field<string>("shapename"),
                                     ColorName = gift.Field<string>("ColorName"),
                                     //width = gift.Field<string>("width"),
                                     //lenght = gift.Field<string>("length"),
                                     IssueQty = gift.Field<int>("IssueQty"),
                                     RecQty = gift.Field<int>("RecQty"),
                                     EMPNAME = gift.Field<string>("EMPNAME"),
                                     Checkedby = gift.Field<string>("Checkedby"),
                                     Rate = gift.Field<double>("Rate"),
                                     Amount = gift.Field<double>("Amount"),
                                     UserName = gift.Field<string>("UserName"),
                                     OrderQty = gift.Field<int>("ORDERQTY"),
                                     ProcessRecDetailId = gift.Field<int>("ProcessRecDetailId"),

                                 });

                    if (query.Count() > 0)
                    {
                        sht.Range("A" + row).Value = query.FirstOrDefault().processname.ToUpper(); ;
                        sht.Range("A" + row + ":B" + row).Style.Font.FontName = "Arial Unicode MS";
                        sht.Range("A" + row + ":B" + row).Style.Font.FontSize = 11;
                        sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;
                        sht.Range("A" + row + ":B" + row).Merge();
                    }
                    row += 1;
                    sht.Range("A" + row + ":L" + row).Style.Font.Bold = true;

                    sht.Range("A" + row).Value = "Vendor Name";
                    sht.Range("B" + row).Value = "Issue Date";
                    //sht.Range("B" + row).Value = "Customer Code";
                    //sht.Range("C" + row).Value = "Customer OrderNo";
                    sht.Range("C" + row).Value = "Issue ChallanNo";
                    sht.Range("D" + row).Value = "Rec Date";
                    sht.Range("E" + row).Value = "Rec ChallanNo";
                    sht.Range("F" + row).Value = "Party ChallanNo";
                    sht.Range("G" + row).Value = "Job Name";
                    sht.Range("H" + row).Value = "Order Desc.";
                    sht.Range("J" + row).Value = "Issue Qty";
                    sht.Range("K" + row).Value = "Rec Qty";
                    sht.Range("L" + row).Value = "Pending Qty";
                    sht.Range("M" + row).Value = "Rec Weight";
                    //sht.Range("N" + row).Value = "Checked By";
                    //sht.Range("O" + row).Value = "Rate";
                    //sht.Range("P" + row).Value = "Amount";
                    //sht.Range("Q" + row).Value = "User Name";
                    //rowinner = row;
                    //pifin = ds.Tables[3].AsEnumerable().Where(a => a.Field<Int32>("PROCESS_NAME_ID") == item).Select(a => a.Field<string>("QualityName")).Distinct().ToList();
                    //int count = 0;
                    //foreach (var fin in pifin)
                    //{
                    //    var queryfin = (from gift in ds.Tables[3].AsEnumerable()
                    //                    where gift.Field<int>("PROCESS_NAME_ID") == item && gift.Field<string>("QualityName") == fin
                    //                    //  fr.price,vf.ITEM_NAME,vf.QualityName,vf.ShapeName,vf.ColorName
                    //                    select new
                    //                    {
                    //                        //AssignDate = gift.Field<DateTime?>("AssignDate"),
                    //                        //CustomerCode = gift.Field<string>("CustomerCode"),
                    //                        //CustomerOrderNo = gift.Field<string>("CustomerOrderNo"),
                    //                        //IssueOrderID = gift.Field<string>("IssueOrderID"),
                    //                        IssueDetailId = gift.Field<int>("IssueDetailId"),
                    //                        ProcessRecDetailId = gift.Field<int>("ProcessRecDetailId"),
                    //                        Order_FinishedID = gift.Field<int>("Order_FinishedID"),
                    //                        //processname = gift.Field<string>("PROCESS_NAME"),
                    //                        //challanno = gift.Field<string>("challanno"),
                    //                        QualityName = gift.Field<string>("QualityName"),
                    //                        //   DesignName = gift.Field<string>("DesignName"),

                    //                        //ColorName = gift.Field<string>("ColorName"),
                    //                        //width = gift.Field<string>("width"),
                    //                        //lenght = gift.Field<string>("length"),
                    //                        IssueQty = gift.Field<int>("IssueQty"),
                    //                        RecQty = gift.Field<int>("RecQty"),
                    //                        OrderQty = gift.Field<int>("ORDERQTY"),
                    //                        //EMPNAME = gift.Field<string>("EMPNAME"),
                    //                        //Checkedby = gift.Field<string>("Checkedby"),
                    //                        //Rate = gift.Field<double>("Rate"),
                    //                        //Amount = gift.Field<double>("Amount"),
                    //                        //UserName = gift.Field<string>("UserName"),

                    //                    });
                    //    if (count == 0)
                    //    {
                    //        sht.Range("U" + row).Value = "Order Summary";
                    //        sht.Range("U" + row + ":Z" + row).Style.Font.FontName = "Arial Unicode MS";
                    //        sht.Range("U" + row + ":Z" + row).Style.Font.FontSize = 11;
                    //        sht.Range("U" + row + ":Z" + row).Style.Font.Bold = true;
                    //        sht.Range("U" + row + ":Z" + row).Merge();
                    //        rowinner += 1;
                    //        sht.Range("U" + row).Value = "Quality";
                    //        sht.Range("V" + rowinner).Value = "Qty Order";
                    //        //sht.Range("C" + row).Value = "Customer OrderNo";
                    //        sht.Range("W" + rowinner).Value = "Qty Issued";
                    //        sht.Range("X" + rowinner).Value = "Qty Rec.";
                    //        sht.Range("Y" + rowinner).Value = "Qty Pending";
                    //    }
                    //    rowinner += 1;
                    //    if (queryfin.Count() > 0)
                    //    {
                    //        int ISSQTY = 0, RECQTY = 0, PENDINGQTY = 0;
                    //        //  int pendingqty = 0, balqty = 0, qty = 0, recqty = 0, orderqty = 0, sumrecqty = 0, sumpendinqty = 0;
                    //        // double sumissueqty = 0;
                    //        // string issuechallan = string.Empty;

                    //        //foreach (var data in queryfin)
                    //        //{
                    //        ISSQTY = queryfin.GroupBy(a => a.IssueDetailId).Sum(a => a.FirstOrDefault().IssueQty);
                    //        RECQTY = queryfin.GroupBy(a => a.ProcessRecDetailId).Sum(a => a.FirstOrDefault().RecQty);
                    //        PENDINGQTY = ISSQTY - RECQTY;
                    //        sht.Range("U" + rowinner).SetValue(queryfin.FirstOrDefault().QualityName); //"BUYER CODE";
                    //        sht.Range("V" + rowinner).SetValue(query.GroupBy(a => a.Order_FinishedID).Sum(a => a.FirstOrDefault().OrderQty)); //"BUYER CODE";
                    //        sht.Range("W" + rowinner).SetValue(ISSQTY); //"BUYER CODE";
                    //        sht.Range("X" + rowinner).SetValue(RECQTY); //"BUYER CODE";
                    //        sht.Range("Y" + rowinner).SetValue(PENDINGQTY); //"BUYER CODE";


                    //        //}
                    //    }
                    //    rowinner++;
                    //    count++;
                    //}
                    //sht.Range("R" + row).Value = "Rec Qty";
                    //sht.Range("S" + row).Value = "Ret Date";
                    //sht.Range("T" + row).Value = "Ret Qty";
                    //sht.Range("U" + row).Value = "Pending Qty";
                    //sht.Range("V" + row).Value = "Receive Remark";
                    //sht.Range("W" + row).Value = "Order Remark";
                    sht.Range("A" + row + ":W" + row).Style.Font.Bold = true;
                    row += 1;
                    if (query.Count() > 0)
                    {
                        int pendingqty = 0, balqty = 0, qty = 0, recqty = 0, orderqty = 0, sumrecqty = 0, sumpendinqty = 0;
                        double sumissueqty = 0;
                        string issuechallan = string.Empty;

                        foreach (var data in query)
                        {
                            decimal reqbal = 0;

                            if (issuechallan == data.IssueDetailId.ToString())
                            {
                                if (sumissueqty == 0)
                                {
                                    sumissueqty = data.IssueQty;
                                }
                                else
                                {
                                    sumissueqty = sumissueqty - Convert.ToDouble(data.RecQty);
                                }


                            }
                            else
                            {

                                sumissueqty = data.IssueQty - Convert.ToDouble(data.RecQty);
                            }
                            sht.Range("A" + row).SetValue(data.EMPNAME); //"BUYER CODE";
                            sht.Range("B" + row).SetValue(data.AssignDate); //"BUYER CODE";
                            //sht.Range("B" + row).SetValue(data.CustomerCode); // = "PO TYPE";
                            //sht.Range("C" + row).SetValue(data.CustomerOrderNo.ToString());  //"LPO";
                            sht.Range("C" + row).SetValue(data.issuechallan.ToString());  //"BPO";
                            sht.Range("D" + row).SetValue(data.ReceiveDate);  //"BPO";
                            sht.Range("E" + row).SetValue(data.challanno.ToString());  //"BPO";
                            sht.Range("F" + row).SetValue("");  //"BPO";
                            sht.Range("G" + row).SetValue(data.processname);  //"ORD DT";
                            sht.Range("H" + row).SetValue(data.QualityName.ToString() + " " + data.DesignName);
                            sht.Range("J" + row).SetValue(data.IssueQty);
                            sht.Range("K" + row).SetValue(data.RecQty);
                            sht.Range("L" + row).SetValue(sumissueqty);
                            //sht.Range("M" + row).SetValue(data.EMPNAME);
                            //sht.Range("N" + row).SetValue(data.Checkedby);
                            //sht.Range("O" + row).SetValue(data.Rate);
                            //sht.Range("P" + row).SetValue(data.Amount);
                            //sht.Range("Q" + row).SetValue(data.UserName);
                            //sht.Range("S" + row).SetValue(ds.Tables[3].Rows[i]["RETURNDATE"]);
                            //sht.Range("T" + row).SetValue(ds.Tables[3].Rows[i]["QTYRETURN"]);
                            //sht.Range("U" + row).SetValue(ds.Tables[3].Rows[i]["PENDINGQTY"]);
                            //sht.Range("V" + row).SetValue(ds.Tables[3].Rows[i]["PURCHASERECEIVEITEMREMARK"]);
                            //sht.Range("W" + row).SetValue(ds.Tables[3].Rows[i]["PURCHASEORDERMASTERREMARK"]);


                            sht.Range("A" + row + ":W" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":W" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":W" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            sht.Range("A" + row + ":W" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            if (issuechallan != data.IssueDetailId.ToString())
                            {
                                issuechallan = data.IssueDetailId.ToString();
                            }
                            //  sumissueqty = sumissueqty - Convert.ToDouble(data.RecQty);
                            row = row + 1;
                        }
                    }
                    // count++;
                }
                row += 1;
                sht.Range("A" + row + ":W" + row).Style.Font.Bold = true;
                //sht.Range("G1:H1").Merge();
                sht.Range("A" + row + ":W" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":W" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":W" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":W" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row).Value = "Vendor Name";
                sht.Range("B" + row).Value = "Issue Date";
                sht.Range("C" + row).Value = "Issue Challan No.";
                sht.Range("D" + row).Value = "Po Number";
                sht.Range("E" + row).Value = "RM Desc";
                sht.Range("F" + row).Value = "Issue Qty";
               
                sht.Range("A" + row + ":W" + row).Style.Font.Bold = true;
                row += 1;
                if (ds.Tables[9].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[9].Rows.Count; i++)
                    {
                        decimal reqbal = 0;
                        sht.Range("A" + row).SetValue(ds.Tables[9].Rows[i]["empname"]); //"BUYER CODE";
                        sht.Range("B" + row).SetValue(ds.Tables[9].Rows[i]["Date"]); // = "PO TYPE";
                        sht.Range("C" + row).SetValue(ds.Tables[9].Rows[i]["chalanno"].ToString());  //"LPO";
                        sht.Range("D" + row).SetValue(ds.Tables[9].Rows[i]["foliochallanno"].ToString());  //"BPO";
                        sht.Range("E" + row).SetValue(ds.Tables[9].Rows[i]["Description"]);  //"ORD DT";
                        sht.Range("F" + row).SetValue(ds.Tables[9].Rows[i]["Issueqty"]);
                        

                        sht.Range("A" + row + ":W" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + row + ":W" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + row + ":W" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        sht.Range("A" + row + ":W" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                        row = row + 1;
                    }
                }







            }

            string Fileextension = "xlsx";
            string filename = "";

            filename = UtilityModule.validateFilename("PODETAIL_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);

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
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
        }

    }
    protected void Interfoliodetail()
    {
        String str = "", FilterBy = "";
        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " and OM.customerid=" + DDCustCode.SelectedValue;
            FilterBy = FilterBy + "," + "Custcode -" + DDCustCode.SelectedItem.Text;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
            FilterBy = FilterBy + "," + "OrderNo -" + DDOrderNo.SelectedItem.Text;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf.Item_id=" + ddItemName.SelectedValue;
            FilterBy = FilterBy + "," + "ItemName -" + ddItemName.SelectedItem.Text;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.qualityId=" + DDQuality.SelectedValue;
            FilterBy = FilterBy + "," + "Quality -" + DDQuality.SelectedItem.Text;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf.designid=" + DDDesign.SelectedValue;
            FilterBy = FilterBy + "," + "Design -" + DDDesign.SelectedItem.Text;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and vf.colorid=" + DDColor.SelectedValue;
            FilterBy = FilterBy + "," + "Color -" + DDColor.SelectedItem.Text;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and vf.sizeid=" + DDSize.SelectedValue;
            FilterBy = FilterBy + "," + "Size -" + DDSize.SelectedItem.Text;
        }
        if (txtlocalOrderNo.Text != "")
        {
            str = str + " and OM.LocalOrder='" + txtlocalOrderNo.Text + "'";
            FilterBy = FilterBy + "," + "Localorder -" + txtlocalOrderNo.Text;
        }
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@Where", str);
        //***********
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETORDERWISEINTERFOLIODETAIL", param);
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

            sht.Range("A1:J1").Merge();
            sht.Range("A1").Value = "ORDER WISE INTERNAL FOLIO DETAILS";
            sht.Range("A2:J2").Merge();
            sht.Range("A2").Value = "Filter By :  " + FilterBy;
            sht.Row(2).Height = 30;
            sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:J2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A2:J2").Style.Alignment.SetWrapText();
            sht.Range("A1:J2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A1:J2").Style.Font.FontSize = 10;
            sht.Range("A1:J2").Style.Font.Bold = true;
            //*******Header
            sht.Range("A3").Value = "Customer code";
            sht.Range("B3").Value = "Order No.";
            sht.Range("C3").Value = "Quality Name";
            sht.Range("D3").Value = "Design Name";
            sht.Range("E3").Value = "Color Name";
            sht.Range("F3").Value = "Size";
            sht.Range("G3").Value = "CustOQty.";
            sht.Range("H3").Value = "WOQty.";
            sht.Range("I3").Value = "Rec. Qty";
            sht.Range("J3").Value = "Rec Bal. Qty";

            sht.Range("A3:J3").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A3:J3").Style.Font.FontSize = 9;
            sht.Range("A3:J3").Style.Font.Bold = true;
            sht.Range("G3:J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            row = 4;
            int rowfrom = 4;
            DataView dv = new DataView(ds.Tables[0]);
            dv.Sort = "CUSTOMERORDERNO";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());

            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":J" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A" + row + ":J" + row).Style.Font.FontSize = 9;

                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["customercode"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["customerorderno"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Designname"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["colorname"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["size"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["OQty"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["WOQty"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Recqty"]);
                sht.Range("J" + row).FormulaA1 = "=H" + row + '-' + ("$I$" + row + "");

                row = row + 1;
            }
            //GRAND TOAL
            //=SUM(L4:L929)
            sht.Range("G" + row).FormulaA1 = "=SUM(G" + rowfrom + ":G" + (row - 1) + ")";
            sht.Range("H" + row).FormulaA1 = "=SUM(H" + rowfrom + ":H" + (row - 1) + ")";
            sht.Range("I" + row).FormulaA1 = "=SUM(I" + rowfrom + ":I" + (row - 1) + ")";
            sht.Range("J" + row).FormulaA1 = "=SUM(J" + rowfrom + ":J" + (row - 1) + ")";
            sht.Range("G" + row + ":J" + row).Style.Font.FontName = "Arial Unicode MS";
            sht.Range("G" + row + ":J" + row).Style.Font.FontSize = 9;
            sht.Range("G" + row + ":J" + row).Style.Font.Bold = true;

            //*************
            sht.Columns(1, 20).AdjustToContents();
            //********************
            //***********BOrders
            using (var a = sht.Range("A1" + ":J" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("OrderwiseInternalFolioDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "foliodetail", "alert('No Record Found!');", true);
        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedChange();
    }
    protected void cancelpodetail()
    {

        string Where = "";
        Where = "Where PIM.Companyid=" + DDCompany.SelectedValue + "";
        if (DDCustCode.SelectedIndex > 0)
        {
            Where = Where + " and OM.customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            Where = Where + " and OM.orderid=" + DDOrderNo.SelectedValue + "";
        }
        if (txtlocalOrderNo.Text != "")
        {
            Where = Where + " and OM.LOcalorder='" + txtlocalOrderNo.Text + "'";
        }

        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@Where", Where);
        param[1] = new SqlParameter("@MastercompanyId", Session["varcompanyId"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetCancelPodetail", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["GetDataset"] = ds;
            Session["rptFileName"] = "~\\Reports\\RptcancelpoDetail.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptcancelpoDetail.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
        ds.Dispose();
    }
    protected void Customeropenorderstatus()
    {
        SqlParameter[] param = new SqlParameter[6];
        param[0] = new SqlParameter("@companyid", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@Customerid", DDCustCode.SelectedIndex > 0 ? DDCustCode.SelectedValue : "0");
        param[2] = new SqlParameter("@orderid", DDOrderNo.SelectedIndex > 0 ? DDOrderNo.SelectedValue : "0");
        param[3] = new SqlParameter("@fromdate", txtfromdate.Text);
        param[4] = new SqlParameter("@Todate", txttodate.Text);
        //***********
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_CUSTOMEROPENORDERSTATUS", param);
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
            //**************
            //*******************BUYER ORDER DETAIL
            //Headers
            sht.Range("A1").Value = "BUYER NAME";
            sht.Range("B1").Value = "ORDER#";
            sht.Range("C1").Value = "ITEM";
            sht.Range("D1").Value = "QTY";
            sht.Range("E1").Value = "SHIP DATE/EX FACTORY";
            sht.Range("F1").Value = "SQ YRD AREA";
            sht.Range("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("F1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A1:E1").Style.Font.Bold = true;
            //
            row = 2;
            int TQTY = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["BuyerName"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerorderNo"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Item"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["DispatchDate"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["SqYrdArea"]);
                row = row + 1;
            }
            row = row - 1;

            sht.Range("A1" + ":F" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("A1" + ":F" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A1" + ":F" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A1" + ":F" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            sht.Columns(1, 6).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("Customer Open Order Status_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn6", "alert('No Record Found!');", true);
        }

    }
    protected void Customeropenorderstatus_AGNI()
    {
        SqlParameter[] param = new SqlParameter[6];
        param[0] = new SqlParameter("@companyid", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@Customerid", DDCustCode.SelectedIndex > 0 ? DDCustCode.SelectedValue : "0");
        param[2] = new SqlParameter("@orderid", DDOrderNo.SelectedIndex > 0 ? DDOrderNo.SelectedValue : "0");
        param[3] = new SqlParameter("@fromdate", txtfromdate.Text);
        param[4] = new SqlParameter("@Todate", txttodate.Text);
        //***********
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_CUSTOMEROPENORDERSTATUS", param);
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
            sht.Range("A1:E1").Value = ds.Tables[0].Rows[0]["CompanyName"].ToString();
            sht.Range("A1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:E1").Style.Font.FontSize = 12;
            sht.Range("A1:E1").Style.Font.Bold = true;
            sht.Range("A1:E1").Merge();
            sht.Range("A2:E2").Value = ds.Tables[0].Rows[0]["COMPADDR1"].ToString();
            sht.Range("A2:E2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:E2").Style.Font.FontSize = 12;
            sht.Range("A2:E2").Style.Font.Bold = true;
            sht.Range("A2:E2").Merge();
            sht.Range("A3:E3").Value ="GSTNo.-"+ ds.Tables[0].Rows[0]["GSTNO"].ToString();
            sht.Range("A3:E3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A3:E3").Style.Font.FontSize = 12;
            sht.Range("A3:E3").Style.Font.Bold = true;
            sht.Range("A3:E3").Merge();
                   
            sht.Range("A4").Value = "BUYER CODER";
            sht.Range("B4").Value = "ORDER#";
            sht.Range("C4").Value = "ITEM";
            sht.Range("D4").Value = "QTY";
            sht.Range("E4").Value = "SHIP DATE/EX FACTORY";
         //   sht.Range("F1").Value = "SQ YRD AREA";
            sht.Range("D4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("F4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A4:E4").Style.Font.Bold = true;
            //
            row = 5;
            int TQTY = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["BuyerName"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerorderNo"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Item"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["DispatchDate"]);
             // sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["SqYrdArea"]);
                row = row + 1;
            }
            row = row - 1;

            sht.Range("A1" + ":F" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("A1" + ":F" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A1" + ":F" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A1" + ":F" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            sht.Columns(1, 6).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("Customer Open Order Status_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn6", "alert('No Record Found!');", true);
        }

    }

    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select IM.ITEM_ID,IM.ITEM_NAME From Item_Master IM Where IM.category_id=" + DDCategory.SelectedValue + " order by ITEM_NAME";
        if (DDCustCode.SelectedIndex > 0 || DDOrderNo.SelectedIndex > 0)
        {
            str = @"SELECT DISTINCT VF.ITEM_ID,VF.ITEM_NAME FROM ORDERMASTER OM INNER JOIN  ORDERDETAIL OD ON OM.ORDERID=OD.ORDERID
                    INNER JOIN V_FINISHEDITEMDETAIL VF ON OD.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID WHere vf.CATEGORY_ID=" + DDCategory.SelectedValue;
            if (DDCustCode.SelectedIndex > 0)
            {
                str = str + " and Om.customerid=" + DDCustCode.SelectedValue;
            }
            if (DDOrderNo.SelectedIndex > 0)
            {
                str = str + " and Om.orderid=" + DDOrderNo.SelectedValue;
            }
            str = str + " ORDER BY ITEM_NAME";
        }
        UtilityModule.ConditionalComboFill(ref ddItemName, str, true, "--Plz Select--");

        if (RDProductionReport.Checked == false)
        {
            Fillcombo();
        }

        if (RDPOSTATUS.Checked == true)
        {
            FillQuality();
            FillDesign();
        }
    }
    protected void Fillcombo()
    {
        Trquality.Visible = false;
        Trdesign.Visible = false;
        Trcolor.Visible = false;
        Trsize.Visible = false;
        Trshadecolor.Visible = false;
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
                        Trquality.Visible = true;
                        break;
                    case "2":
                        Trdesign.Visible = true;
                        break;
                    case "3":
                        Trcolor.Visible = true;
                        break;
                    case "5":
                        Trsize.Visible = true;
                        break;
                    case "6":
                        Trshadecolor.Visible = true;
                        break;
                }
            }
        }

    }
    protected void FillQuality()
    {
        string str = "";
        if (DDCustCode.SelectedIndex > 0 || DDOrderNo.SelectedIndex > 0)
        {
            str = @"SELECT DISTINCT VF.QualityId,VF.QualityName FROM ORDERMASTER OM INNER JOIN  ORDERDETAIL OD ON OM.ORDERID=OD.ORDERID
                  INNER JOIN V_FINISHEDITEMDETAIL VF ON OD.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID WHere 1=1";
            if (DDCategory.SelectedIndex > 0)
            {
                str = str + " and vf.CATEGORY_ID=" + DDCategory.SelectedValue;
            }
            if (DDCustCode.SelectedIndex > 0)
            {
                str = str + " and Om.customerid=" + DDCustCode.SelectedValue;
            }
            if (DDOrderNo.SelectedIndex > 0)
            {
                str = str + " and Om.orderid=" + DDOrderNo.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + "  and vf.Item_id=" + ddItemName.SelectedValue;
            }
            str = str + " ORDER BY QualityName";
        }
        else
        {
            str = @"select Distinct Q.QualityId,Q.QualityName From ITEM_MASTER IM inner Join Quality Q on Q.Item_Id=IM.ITEM_ID inner Join CategorySeparate cs on IM.CATEGORY_ID=cs.Categoryid where 1=1";
            if (DDCategory.SelectedIndex > 0)
            {
                str = str + "  and IM.Category_id=" + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + "  and IM.Item_id=" + ddItemName.SelectedValue;
            }
            str = str + "  order by QualityName";
        }
        UtilityModule.ConditionalComboFill(ref DDQuality, str, true, "---Plz Select---");
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQuality();
    }
    protected void FillDesign()
    {
        string str = "";
        if (DDCustCode.SelectedIndex > 0 || DDOrderNo.SelectedIndex > 0)
        {
            str = @"SELECT DISTINCT VF.designId,VF.designName FROM ORDERMASTER OM INNER JOIN  ORDERDETAIL OD ON OM.ORDERID=OD.ORDERID
                  INNER JOIN V_FINISHEDITEMDETAIL VF ON OD.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID WHere 1=1";
            if (DDCategory.SelectedIndex > 0)
            {
                str = str + " and vf.CATEGORY_ID=" + DDCategory.SelectedValue;
            }
            if (DDCustCode.SelectedIndex > 0)
            {
                str = str + " and Om.customerid=" + DDCustCode.SelectedValue;
            }
            if (DDOrderNo.SelectedIndex > 0)
            {
                str = str + " and Om.orderid=" + DDOrderNo.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + "  and vf.Item_id=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
            }
            str = str + " ORDER BY designName";
        }
        else
        {
            str = @"select Distinct Vf.designId,vf.designName From V_finishedItemDetail Vf Where Vf.designId>0";
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + "  and vf.Item_id=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
            }

            str = str + "  order by designName";
        }
        UtilityModule.ConditionalComboFill(ref DDDesign, str, true, "---Plz Select---");
    }
    protected void FillShade()
    {
        string str = @"select Distinct ShadecolorId,ShadeColorName From V_finishedItemDetail Vf Where Vf.ShadecolorId>0";
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + "  and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
        }

        str = str + "  order by ShadeColorName";
        UtilityModule.ConditionalComboFill(ref DDshade, str, true, "---Plz Select---");
    }
    protected void FillColor()
    {
        string str = "";
        if (DDCustCode.SelectedIndex > 0 || DDOrderNo.SelectedIndex > 0)
        {
            str = @"SELECT DISTINCT VF.ColorId,VF.ColorName FROM ORDERMASTER OM INNER JOIN  ORDERDETAIL OD ON OM.ORDERID=OD.ORDERID
                  INNER JOIN V_FINISHEDITEMDETAIL VF ON OD.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID WHere 1=1";
            if (DDCategory.SelectedIndex > 0)
            {
                str = str + " and vf.CATEGORY_ID=" + DDCategory.SelectedValue;
            }
            if (DDCustCode.SelectedIndex > 0)
            {
                str = str + " and Om.customerid=" + DDCustCode.SelectedValue;
            }
            if (DDOrderNo.SelectedIndex > 0)
            {
                str = str + " and Om.orderid=" + DDOrderNo.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + "  and vf.Item_id=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + "  and vf.DesignId=" + DDDesign.SelectedValue;
            }
            str = str + " ORDER BY ColorName";
        }
        else
        {
            str = @"select Distinct Vf.Colorid,vf.Colorname From V_finishedItemDetail Vf Where Vf.Colorid>0";
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + "  and vf.Item_id=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + "  and vf.DesignId=" + DDDesign.SelectedValue;
            }
            str = str + "  order by Colorname";
        }
        UtilityModule.ConditionalComboFill(ref DDColor, str, true, "---Plz Select---");
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {

        FillDesign();
        FillSize();
        if (Trshadecolor.Visible == true)
        {
            FillShade();
        }
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillColor();
    }
    protected void FillSize()
    {
        string size = "ProdSizeFt";
        if (Chkmtrsize.Checked == true)
        {
            size = "ProdSizemtr";
        }
        string str = "";
        if (DDCustCode.SelectedIndex > 0 || DDOrderNo.SelectedIndex > 0)
        {
            str = @"SELECT DISTINCT Vf.Sizeid,vf." + size + @" as Size FROM ORDERMASTER OM INNER JOIN  ORDERDETAIL OD ON OM.ORDERID=OD.ORDERID
                  INNER JOIN V_FINISHEDITEMDETAIL VF ON OD.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID WHere 1=1";
            if (DDCategory.SelectedIndex > 0)
            {
                str = str + " and vf.CATEGORY_ID=" + DDCategory.SelectedValue;
            }
            if (DDCustCode.SelectedIndex > 0)
            {
                str = str + " and Om.customerid=" + DDCustCode.SelectedValue;
            }
            if (DDOrderNo.SelectedIndex > 0)
            {
                str = str + " and Om.orderid=" + DDOrderNo.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + "  and vf.Item_id=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + "  and vf.Colorid=" + DDColor.SelectedValue;
            }
            str = str + " ORDER BY Size";
        }
        else
        {

            str = @"select Distinct  Vf.Sizeid,vf." + size + " as Size  From V_finishedItemDetail Vf Where Vf.Sizeid>0";
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + "  and vf.Item_id=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + "  and vf.DesignId=" + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + "  and vf.Colorid=" + DDColor.SelectedValue;
            }

            str = str + "  order by Size";
        }
        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "---Plz Select---");
    }
    protected void Chkmtrsize_CheckedChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void ProductionReport()
    {
        string str = "";
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + "  and VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + "  and VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        //str = str + " group by VF.CATEGORY_NAME,VF.ITEM_NAME";
        //str = str + " order by VF.ITEM_NAME";

        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@companyid", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@Where", str);
        param[2] = new SqlParameter("@fromdate", txtfromdate.Text);
        param[3] = new SqlParameter("@Todate", txttodate.Text);
        //***********
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "GetWeavingAreaProductionReportSummary", param);
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
            //**************
            //*******************Production Report Summary
            //Headers
            sht.Range("A3").Value = "Production Report";
            sht.Range("B3").Value = txtfromdate.Text;
            sht.Range("C3").Value = "To";
            sht.Range("D3").Value = txttodate.Text;
            sht.Range("A3:D3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            sht.Range("A4").Value = "Quality";
            sht.Range("B4").Value = "Order(sqrd)";
            sht.Range("C4").Value = "Issue(Sqrd)";
            sht.Range("D4").Value = "Received(Sqrd)";
            sht.Range("A3:D4").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("A3:D4").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A3:D4").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A3:D4").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            sht.Range("A3:D3").Style.Font.Bold = true;
            //
            row = 5;
            int rowfrom = 5;
            double TotalOrderSqYd = 0;
            double TotalIssueSqYd = 0;
            double TotalReceiveSqYd = 0;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);

                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["OrderSqYd"]);
                TotalOrderSqYd += Convert.ToDouble(ds.Tables[0].Rows[i]["OrderSqYd"]);

                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["IssueSqYd"]);
                TotalIssueSqYd += Convert.ToDouble(ds.Tables[0].Rows[i]["IssueSqYd"]);

                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ReceiveSqYd"]);
                TotalReceiveSqYd += Convert.ToDouble(ds.Tables[0].Rows[i]["ReceiveSqYd"]);

                sht.Range("A" + row + ":D" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":D" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":D" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                sht.Range("A" + row + ":D" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                row = row + 1;

            }

            sht.Range("A" + row).Value = "Total";
            sht.Range("B" + row).SetValue(TotalOrderSqYd);
            sht.Range("C" + row).SetValue(TotalIssueSqYd);
            sht.Range("D" + row).SetValue(TotalReceiveSqYd);

            //sht.Range("B" + row).FormulaA1 = "=SUM(B" + rowfrom + ":B" + (row - 1) + ")";
            //sht.Range("C" + row).FormulaA1 = "=SUM(C" + rowfrom + ":C" + (row - 1) + ")";
            //sht.Range("D" + row).FormulaA1 = "=SUM(D" + rowfrom + ":D" + (row - 1) + ")";

            sht.Range("A" + row + ":D" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + row + ":D" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + row + ":D" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + row + ":D" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            sht.Range("A" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A" + row + ":D" + row).Style.Font.SetBold();

            sht.Columns(1, 15).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("Weaving Area Production Report_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn6", "alert('No Record Found!');", true);
        }

    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDCustCode, "Select c.CustomerId,CustomerCode From ordermaster o join  CustomerInfo c on o.customerid=c.CustomerId Where MasterCompanyId=" + Session["varCompanyId"] + " and o.orderid="+DDOrderNo.SelectedValue+" Order By CustomerCode", true, "");

       // UtilityModule.ConditionalComboFill(ref DDOrderNo, Str, true, "--Select--");

        if (RDPOSTATUS.Checked == true)
        {

            string str = @"select Replace(CONVERT(nvarchar(11),Dateadded,106),' ','-') as Dateadded From Ordermaster Where orderid=" + DDOrderNo.SelectedValue;
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtfromdate.Text = ds.Tables[0].Rows[0]["Dateadded"].ToString();
                txttodate.Text = ds.Tables[0].Rows[0]["Dateadded"].ToString();
            }
            else
            {
                txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
                txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            }

        }
    }
    private void PaidunpaidDetailHafizia()
    {
        int TOQTY = 0, TOBal = 0, TWQTY = 0, TRQTY = 0, TBAL = 0;
        int TWOQTY = 0, TWRQTY = 0, TWBal = 0, TPaid = 0, TUpaid = 0;
        Decimal TWOArea = 0, TWRArea = 0, TPArea = 0, TUparea = 0;


        //SqlParameter[] param = new SqlParameter[2];
        //param[0] = new SqlParameter("@localOrderNo", txtlocalOrderNo.Text);
        //param[1] = new SqlParameter("@userid", Session["varuserid"]);

        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_getVendorWisepaymentReports_Hafizia", param);

        //********Proc
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("pro_getVendorWisepaymentReports_Hafizia", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;
        cmd.Parameters.AddWithValue("@localOrderNo", txtlocalOrderNo.Text);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);

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
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 0;
            //**************
            //*******************BUYER ORDER DETAIL
            sht.Range("A1:J1").Merge();
            sht.Range("A1").Value = ds.Tables[0].Rows[0]["companyname"];
            sht.Range("A2:J2").Merge();
            sht.Range("A2").Value = "(PAYMENT REPORTS)";
            sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:J2").Style.Font.Bold = true;
            //Headers
            sht.Range("A3").Value = "Quality";
            sht.Range("B3").Value = "Design";
            sht.Range("C3").Value = "Color";
            sht.Range("D3").Value = "Shape";
            sht.Range("E3").Value = "Size";
            sht.Range("A3:E3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            sht.Range("F3").Value = "Order Qty.";
            sht.Range("G3").Value = "Order Bal.";
            sht.Range("H3").Value = "WQty.";
            sht.Range("I3").Value = "RecQty.";
            sht.Range("J3").Value = "Balance";
            sht.Range("F3:J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3:J3").Style.Font.Bold = true;
            //
            sht.Range("A4:J4").Merge();
            sht.Range("A4").Value = "Customer Code :" + ds.Tables[0].Rows[0]["customercode"] + "       Customer Order No : " + ds.Tables[0].Rows[0]["customerorderNo"] + "       SR No : " + ds.Tables[0].Rows[0]["Localorder"] + "";
            sht.Range("A4:J4").Style.Font.FontSize = 12;
            sht.Range("A4:J4").Style.Font.Bold = true;
            //*********Details
            row = 5;
            int Rowcount = ds.Tables[0].Rows.Count;
            int Orderbal = 0, Balance = 0;

            for (int i = 0; i < Rowcount; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Designname"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Colorname"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Shapename"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Export_Format"]);

                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Orderqty"]);
                TOQTY = TOQTY + Convert.ToInt32(ds.Tables[0].Rows[i]["Orderqty"]);
                Orderbal = Convert.ToInt32(ds.Tables[0].Rows[i]["Orderqty"]) - Convert.ToInt32(ds.Tables[0].Rows[i]["Worderqty"]);
                sht.Range("G" + row).SetValue(Orderbal);
                TOBal = TOBal + Orderbal;
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Worderqty"]);
                TWQTY = TWQTY + Convert.ToInt32(ds.Tables[0].Rows[i]["Worderqty"]);

                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Recqty"]);
                TRQTY = TRQTY + Convert.ToInt32(ds.Tables[0].Rows[i]["Recqty"]);

                if (Convert.ToInt32(ds.Tables[0].Rows[i]["Worderqty"]) - Convert.ToInt32(ds.Tables[0].Rows[i]["RecQty"]) < 0)
                {
                    Balance = 0;
                }
                else
                {
                    Balance = Convert.ToInt32(ds.Tables[0].Rows[i]["Worderqty"]) - Convert.ToInt32(ds.Tables[0].Rows[i]["RecQty"]);
                }
                sht.Range("J" + row).SetValue(Balance);
                TBAL = TBAL + Balance;
                row = row + 1;
            }
            //TOtal
            sht.Range("F" + row).SetValue(TOQTY);
            sht.Range("G" + row).SetValue(TOBal);
            sht.Range("H" + row).SetValue(TWQTY);
            sht.Range("I" + row).SetValue(TRQTY);
            sht.Range("J" + row).SetValue(TBAL);
            sht.Range("F" + row + ":J" + row).Style.Font.Bold = true;

            sht.Range("A3:J3").Style.Fill.BackgroundColor = XLColor.Yellow;
            sht.Range("F" + row + ":J" + row).Style.Fill.BackgroundColor = XLColor.AshGrey;

            using (var a = sht.Range("A3" + ":J" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //**********************
            row = row + 2;
            sht.Range("A" + row + ":J" + row).Merge();
            sht.Range("A" + row).Value = "(JOB WISE DETAIL)";
            sht.Range("A" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;
            row = row + 1;
            int Orderlastrow = row;
            //JOb Headers

            // sht.Range("A" + row).Value = "WorderNo.";
            sht.Range("A" + row).Value = "Emp./Contractor Name";
            sht.Range("B" + row).Value = "Process";

            sht.Range("C" + row).Value = "Quality";
            sht.Range("D" + row).Value = "Design";
            sht.Range("E" + row).Value = "Color";
            sht.Range("F" + row).Value = "Shape";
            sht.Range("G" + row).Value = "Size";

            sht.Range("H" + row).Value = "WOqty.";
            sht.Range("I" + row).Value = "WOArea";
            sht.Range("J" + row).Value = "RecQty.";
            sht.Range("K" + row).Value = "RecArea";
            sht.Range("L" + row).Value = "Balance";

            sht.Range("M" + row).Value = "Paid Qty.";
            sht.Range("N" + row).Value = "Paid Area";
            sht.Range("O" + row).Value = "Unpaid Qty.";
            sht.Range("P" + row).Value = "Unpaid Area";

            sht.Range("H3:P3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A" + row + ":P" + row).Style.Fill.BackgroundColor = XLColor.Yellow;
            sht.Range("A" + row + ":P" + row).Style.Font.Bold = true;
            //
            row = row + 1;

            int Unpaidqty = 0;
            Decimal Unpaidarea = 0;

            DataTable dtdistinct = ds.Tables[1].DefaultView.ToTable(true, "SeqNo", "ProcessName");

            foreach (DataRow dr in dtdistinct.Rows)
            {
                DataView dv = new DataView(ds.Tables[1]);
                dv.RowFilter = "SeqNo=" + dr["SeqNo"] + " and ProcessName='" + dr["ProcessName"] + "'";
                DataSet ds1 = new DataSet();

                ds1.Tables.Add(dv.ToTable());
                Rowcount = ds1.Tables[0].Rows.Count;

                TWOQTY = 0; TWRQTY = 0; TWBal = 0; TPaid = 0; TUpaid = 0;
                TWOArea = 0; TWRArea = 0; TPArea = 0; TUparea = 0;

                for (int i = 0; i < Rowcount; i++)
                {

                    //sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Issueorderid"]);
                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["EMpname"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["ProcessName"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Designname"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Colorname"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Shape"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["WOQTY"]);
                    TWOQTY = TWOQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["WOQTY"]);
                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["WOArea"]);
                    TWOArea = TWOArea + Convert.ToDecimal(ds1.Tables[0].Rows[i]["WOArea"]);
                    sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["WRQTY"]);
                    TWRQTY = TWRQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["WRQTY"]);
                    sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["WRArea"]);
                    TWRArea = TWRArea + Convert.ToDecimal(ds1.Tables[0].Rows[i]["WRArea"]);
                    if (Convert.ToInt32(ds1.Tables[0].Rows[i]["WOQTY"]) - Convert.ToInt32(ds1.Tables[0].Rows[i]["WRQTY"]) < 0)
                    {
                        Balance = 0;
                    }
                    else
                    {
                        Balance = Convert.ToInt32(ds1.Tables[0].Rows[i]["WOQTY"]) - Convert.ToInt32(ds1.Tables[0].Rows[i]["WRQTY"]);
                    }
                    sht.Range("L" + row).SetValue(Balance);
                    TWBal = TWBal + Balance;
                    sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["Paidqty"]);
                    TPaid = TPaid + Convert.ToInt32(ds1.Tables[0].Rows[i]["Paidqty"]);
                    sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["PaidArea"]);
                    TPArea = TPArea + Convert.ToDecimal(ds1.Tables[0].Rows[i]["Paidarea"]);
                    Unpaidqty = Convert.ToInt32(ds1.Tables[0].Rows[i]["WRQTY"]) - Convert.ToInt32(ds1.Tables[0].Rows[i]["Paidqty"]);
                    sht.Range("O" + row).SetValue(Unpaidqty);
                    TUpaid = TUpaid + Unpaidqty;
                    Unpaidarea = Convert.ToDecimal(ds1.Tables[0].Rows[i]["WRArea"]) - Convert.ToDecimal(ds1.Tables[0].Rows[i]["Paidarea"]);
                    sht.Range("P" + row).SetValue(Unpaidarea);
                    TUparea = TUparea + Unpaidarea;
                    row = row + 1;
                    //**************************

                }
                //***************Total
                sht.Range("H" + row).SetValue(TWOQTY);
                sht.Range("I" + row).SetValue(TWOArea);
                sht.Range("J" + row).SetValue(TWRQTY);
                sht.Range("K" + row).SetValue(TWRArea);
                sht.Range("L" + row).SetValue(TWBal);
                sht.Range("M" + row).SetValue(TPaid);
                sht.Range("N" + row).SetValue(TPArea);
                sht.Range("O" + row).SetValue(TUpaid);
                sht.Range("P" + row).SetValue(TUparea);
                sht.Range("A" + row + ":B" + row).Merge();
                sht.Range("A" + row).Value = dr["Processname"] + " " + "Total : ";
                sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;

                sht.Range("A" + row + ":P" + row).Style.Fill.BackgroundColor = XLColor.AshGrey;
                sht.Range("H" + row + ":P" + row).Style.Font.Bold = true;
                row = row + 1;



                //sht.Range("I" + row).SetValue(TWOQTY);
                //sht.Range("J" + row).SetValue(TWOArea);
                //sht.Range("K" + row).SetValue(TWRQTY);
                //sht.Range("L" + row).SetValue(TWRArea);
                //sht.Range("M" + row).SetValue(TWBal);
                //sht.Range("N" + row).SetValue(TPaid);
                //sht.Range("O" + row).SetValue(TPArea);
                //sht.Range("P" + row).SetValue(TUpaid);
                //sht.Range("Q" + row).SetValue(TUparea);
                //sht.Range("A" + row + ":B" + row).Merge();
                //sht.Range("A" + row).Value = dr["Processname"] + " " + "Total : ";
                //sht.Range("A" + row + ":B" + row).Style.Font.Bold = true;

                //sht.Range("A" + row + ":Q" + row).Style.Fill.BackgroundColor = XLColor.AshGrey;
                //sht.Range("I" + row + ":Q" + row).Style.Font.Bold = true;
                //row = row + 1;
            }

            using (var a = sht.Range("A" + Orderlastrow + ":P" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*************
            sht.Columns(1, 30).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("Paidunpaid_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "o2", "alert('No Record Found!');", true);
        }
        ds.Dispose();

    }
    private void ProcesswisedetailHafiziaAllProcess()
    {
        int TOQTY = 0, TOBal = 0, TWQTY = 0, TRQTY = 0, TBAL = 0;
        int TWOQTY = 0, TWRQTY = 0, TWBal = 0, TPaid = 0, TUpaid = 0;
        Decimal TWOArea = 0, TWRArea = 0, TPArea = 0, TUparea = 0;


        //SqlParameter[] param = new SqlParameter[4];
        //param[0] = new SqlParameter("@localOrderNo", txtlocalOrderNo.Text=="" ?"0" : txtlocalOrderNo.Text);
        //param[1] = new SqlParameter("@userid", Session["varuserid"]);
        //param[2] = new SqlParameter("@FromDate", txtfromdate.Text);
        //param[3] = new SqlParameter("@ToDate", txttodate.Text);

        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_getProcessWiseDetailHafAllProcess", param);


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("pro_getProcessWiseDetailHafAllProcess", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@localOrderNo", txtlocalOrderNo.Text == "" ? "0" : txtlocalOrderNo.Text);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@FromDate", txtfromdate.Text);
        cmd.Parameters.AddWithValue("@ToDate", txttodate.Text);
        //cmd.Parameters.AddWithValue("@ChkForDate", ChkForDate.Checked == true ? "1" : "0");

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

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
            int Rowcount = 0;
            int Orderbal = 0, Balance = 0;
            //**************
            //*******************BUYER ORDER DETAIL
            sht.Range("A1:J1").Merge();
            sht.Range("A1").Value = "";
            sht.Range("A2:J2").Merge();
            sht.Range("A2").Value = "(ALL PROCESS REPORTS)";
            sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:J2").Style.Font.Bold = true;

            sht.Range("A3:J3").Merge();
            sht.Range("A3").Value = "Date From:" + txtfromdate.Text + " To:" + txttodate.Text + "";

            //**********************
            row = 4;
            sht.Range("A" + row + ":J" + row).Merge();
            sht.Range("A" + row).Value = "(JOB WISE DETAIL)";
            sht.Range("A" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;
            row = row + 1;
            int Orderlastrow = row;
            //JOb Headers

            // sht.Range("A" + row).Value = "WorderNo.";
            sht.Range("A" + row).Value = "OrderDate";
            sht.Range("B" + row).Value = "OrderNo";
            sht.Range("C" + row).Value = "Process";

            sht.Range("D" + row).Value = "Quality";
            sht.Range("E" + row).Value = "Design";
            sht.Range("F" + row).Value = "Color";
            sht.Range("G" + row).Value = "Shape";
            sht.Range("H" + row).Value = "Size";

            sht.Range("I" + row).Value = "OrderQty";
            sht.Range("J" + row).Value = "WOqty";
            sht.Range("K" + row).Value = "RecQty";
            sht.Range("L" + row).Value = "Balance";

            sht.Range("I3:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A" + row + ":L" + row).Style.Fill.BackgroundColor = XLColor.Yellow;
            sht.Range("A" + row + ":L" + row).Style.Font.Bold = true;
            //
            row = row + 1;

            int Unpaidqty = 0;
            Decimal Unpaidarea = 0;

            //DataTable dtdistinct2 = ds.Tables[0].DefaultView.ToTable(true, "OrderId");

            DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "OrderId", "SeqNo", "ProcessName");

            foreach (DataRow dr in dtdistinct.Rows)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "OrderId=" + dr["OrderId"] + " and SeqNo=" + dr["SeqNo"] + " and ProcessName='" + dr["ProcessName"] + "'";
                DataSet ds1 = new DataSet();

                ds1.Tables.Add(dv.ToTable());
                Rowcount = ds1.Tables[0].Rows.Count;

                TWOQTY = 0; TWRQTY = 0; TWBal = 0; TPaid = 0; TUpaid = 0;
                TWOArea = 0; TWRArea = 0; TPArea = 0; TUparea = 0; TOQTY = 0;

                for (int i = 0; i < Rowcount; i++)
                {

                    //sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Issueorderid"]);
                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["OrderDate"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["LocalOrderNo"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["ProcessName"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Designname"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Colorname"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Shape"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["OrderQty"]);
                    TOQTY = TOQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["OrderQty"]);

                    sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["WOQTY"]);
                    TWOQTY = TWOQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["WOQTY"]);

                    sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["WRQTY"]);
                    TWRQTY = TWRQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["WRQTY"]);
                    if (Convert.ToInt32(ds1.Tables[0].Rows[i]["WOQTY"]) - Convert.ToInt32(ds1.Tables[0].Rows[i]["WRQTY"]) < 0)
                    {
                        Balance = 0;
                    }
                    else
                    {
                        Balance = Convert.ToInt32(ds1.Tables[0].Rows[i]["WOQTY"]) - Convert.ToInt32(ds1.Tables[0].Rows[i]["WRQTY"]);
                    }
                    sht.Range("L" + row).SetValue(Balance);
                    TWBal = TWBal + Balance;
                    row = row + 1;
                    //**************************

                }
                //***************Total
                sht.Range("I" + row).SetValue(TOQTY);
                sht.Range("J" + row).SetValue(TWOQTY);
                sht.Range("K" + row).SetValue(TWRQTY);
                sht.Range("L" + row).SetValue(TWBal);
                sht.Range("A" + row + ":C" + row).Merge();
                sht.Range("A" + row).Value = dr["Processname"] + " " + "Total : ";
                sht.Range("A" + row + ":C" + row).Style.Font.Bold = true;

                sht.Range("A" + row + ":L" + row).Style.Fill.BackgroundColor = XLColor.AshGrey;
                sht.Range("H" + row + ":L" + row).Style.Font.Bold = true;
                row = row + 1;

            }

            using (var a = sht.Range("A" + Orderlastrow + ":L" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*************
            sht.Columns(1, 30).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("ProcessWiseDetailAllProcess_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "o2", "alert('No Record Found!');", true);
        }
        ds.Dispose();

    }
    private void OrderWiseProcessdetailHafiziaRecQty()
    {
        int TOQTY = 0, TOBal = 0, TWQTY = 0, TRQTY = 0, TBAL = 0;
        int TWOQTY = 0, TWRQTY = 0, TWBal = 0, TGIQTY = 0, TBIQTY = 0, TKIQTY = 0, TKQTY = 0, TEIQTY = 0, TEQTY = 0, TMIQTY = 0,
            TSIQTY = 0, TSQTY = 0, TLIQTY = 0, TLQTY = 0, TRIQTY = 0, TREQTY = 0, TPIQTY = 0, TPQTY = 0,
            TSQty2=0,TREMQTY=0,TSIPQTY=0,TPIPQTY=0,TMKHQTY=0,TSSQTY=0,TRDISQTY=0,THSQTY=0,TRPQTY=0,TRFSQTY=0,TRL2QTY=0,TRWQTY=0
            ,TROFQTY=0,TRTQTY=0,TRCQTY=0,TRS3QTY=0,TRZZQTY=0,TRTAQTY=0,TRHSQTY=0,TRREQTY=0,TRSTIQTY=0,TRRLQTY=0,TRP2QTY=0,TRSQTY=0
            ,TRFINQTY=0,TRREHIQTY=0,TRRFIQTY=0;


        //SqlParameter[] param = new SqlParameter[4];
        //param[0] = new SqlParameter("@localOrderNo", txtlocalOrderNo.Text=="" ?"0" : txtlocalOrderNo.Text);
        //param[1] = new SqlParameter("@userid", Session["varuserid"]);
        //param[2] = new SqlParameter("@FromDate", txtfromdate.Text);
        //param[3] = new SqlParameter("@ToDate", txttodate.Text);

        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_getProcessOrderWiseDetailHafizia", param);


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("pro_getProcessOrderWiseDetailHafizia", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        //cmd.Parameters.AddWithValue("@localOrderNo", txtlocalOrderNo.Text == "" ? "0" : txtlocalOrderNo.Text);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@FromDate", txtfromdate.Text);
        cmd.Parameters.AddWithValue("@ToDate", txttodate.Text);
        //cmd.Parameters.AddWithValue("@ChkForDate", ChkForDate.Checked == true ? "1" : "0");

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

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
            int Rowcount = 0;
            int Orderbal = 0, Balance = 0;
            //**************
            //*******************BUYER ORDER DETAIL
            sht.Range("A1:J1").Merge();
            sht.Range("A1").Value = "";
            sht.Range("A2:J2").Merge();
            sht.Range("A2").Value = "(REC QTY PROCESS REPORTS)";
            sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:J2").Style.Font.Bold = true;

            sht.Range("A3:J3").Merge();
            sht.Range("A3").Value = "Date From:" + txtfromdate.Text + " To:" + txttodate.Text + "";

            //**********************
            row = 4;
            sht.Range("A" + row + ":J" + row).Merge();
            sht.Range("A" + row).Value = "(ORDER WISE DETAIL)";
            sht.Range("A" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A" + row + ":J" + row).Style.Font.Bold = true;
             row = row + 1;
            int Orderlastrow = row;
            //JOb Headers

            //sht.Range("A" + row).Value = "OrderDate";
            //sht.Range("C" + row).Value = "Process";

            sht.Range("A" + row).Value = "OrderNo";
            sht.Range("B" + row).Value = "Quality";
            sht.Range("C" + row).Value = "Design";
            sht.Range("D" + row).Value = "Color";
            sht.Range("E" + row).Value = "Shape";
            sht.Range("F" + row).Value = "Size";

            sht.Range("G" + row).Value = "OrderQty";
            sht.Range("H" + row).Value = "IssueQty";
            sht.Range("I" + row).Value = "Order Bal";
            sht.Range("J" + row).Value = "RecQty";
            sht.Range("K" + row).Value = "Balance";
            sht.Range("L" + row).Value = "BACKING";
            sht.Range("M" + row).Value = "EMBRODING-INTERNAL PROCESS";
            sht.Range("N" + row).Value = "LABEL-INTERNAL PROCESS";
            sht.Range("O" + row).Value = "LACE";
            sht.Range("P" + row).Value = "GODOWN-INTERNAL PROCESS";
            sht.Range("Q" + row).Value = "REPAIRING";
            sht.Range("R" + row).Value = "KNOTTING";
            sht.Range("S" + row).Value = "BINDING";
            sht.Range("T" + row).Value = "NEWADH";
            sht.Range("U" + row).Value = "ROPPING";
            sht.Range("V" + row).Value = "PACKING";
            sht.Range("W" + row).Value = "STITCHING";
            sht.Range("X" + row).Value = "KONE SILAI";
            sht.Range("Y" + row).Value = "LABEL";
            sht.Range("Z" + row).Value = "STRETCHING (TEDA)";
            sht.Range("AA" + row).Value = "STITTCHING-2";
            sht.Range("AB" + row).Value = "EMBROIDERY";
            sht.Range("AC" + row).Value = "STITCHING-INTERNAL PROCESS";
            sht.Range("AD" + row).Value = "PACKING-INTERNAL PROCESS";
            sht.Range("AE" + row).Value = "MARKEEN KHOLI SILAI";
            sht.Range("AF" + row).Value = "TAANKI SILAI";
            sht.Range("AG" + row).Value = "DISPATCH";
            sht.Range("AH" + row).Value = "HAND STIT-MUNNA";
            sht.Range("AI" + row).Value = "PRINTING";
            sht.Range("AJ" + row).Value = "FILLER STITCHING";
            sht.Range("AK" + row).Value = "LABLE-2";
            sht.Range("AL" + row).Value = "WASHING";
            sht.Range("AM" + row).Value = "OUTDOOR FINISHING";
            sht.Range("AN" + row).Value = "TUFTING ";
            sht.Range("AO" + row).Value = "CHHOTI";
            sht.Range("AP" + row).Value = "STITCHING-3";
            sht.Range("AQ" + row).Value = "ZIGZAG STITCH";
            sht.Range("AR" + row).Value = "TASSEL";
            sht.Range("AS" + row).Value = "HAND STITCH-2";
            sht.Range("AT" + row).Value = "RE-PACKING";
            sht.Range("AU" + row).Value = "RE-STITCHING";
            sht.Range("AV" + row).Value = "RE-LABEL";
            sht.Range("AW" + row).Value = "RE-PACKING-2";
            sht.Range("AX" + row).Value = "SERGING";
            sht.Range("AY" + row).Value = "FINISHING";
            sht.Range("AZ" + row).Value = "RE-MARKEEN KHOLI SILAI";
            sht.Range("BA" + row).Value = "RE-FINISHING";         



            sht.Range("G3:BA3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A" + row + ":BA" + row).Style.Fill.BackgroundColor = XLColor.Yellow;
            sht.Range("A" + row + ":BA" + row).Style.Font.Bold = true;
            //
            row = row + 1;

            DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "OrderId");

            foreach (DataRow dr in dtdistinct.Rows)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "OrderId=" + dr["OrderId"] + "";
                DataSet ds1 = new DataSet();

                ds1.Tables.Add(dv.ToTable());
                Rowcount = ds1.Tables[0].Rows.Count;

                TWOQTY = 0; TWRQTY = 0; TWBal = 0;
                TOQTY = 0; TOBal = 0;
                TGIQTY = 0; TBIQTY = 0; TKIQTY = 0; TKQTY = 0; TEIQTY = 0; TEQTY = 0; TMIQTY = 0;
                TSIQTY = 0; TSQTY = 0; TLIQTY = 0; TLQTY = 0; TRIQTY = 0; TREQTY = 0; TPIQTY = 0; TPQTY = 0;
                
                TSQty2=0;TREMQTY=0;TSIPQTY=0;TPIPQTY=0;TMKHQTY=0;TSSQTY=0;TRDISQTY=0;THSQTY=0;TRPQTY=0;TRFSQTY=0;TRL2QTY=0;TRWQTY=0;
                TROFQTY=0;TRTQTY=0;TRCQTY=0;TRS3QTY=0;TRZZQTY=0;TRTAQTY=0;TRHSQTY=0;TRREQTY=0;TRSTIQTY=0;TRRLQTY=0;TRP2QTY=0;TRSQTY=0;
                TRFINQTY = 0; TRREHIQTY = 0; TRRFIQTY = 0;

                for (int i = 0; i < Rowcount; i++)
                {

                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["LocalOrder"]);

                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Designname"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Colorname"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["SHAPENAME"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);

                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Qtyrequired"]);
                    TOQTY = TOQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["Qtyrequired"]);
                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["IssuedQty"]);
                    TWOQTY = TWOQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["IssuedQty"]);

                    if (Convert.ToInt32(ds1.Tables[0].Rows[i]["Qtyrequired"]) - Convert.ToInt32(ds1.Tables[0].Rows[i]["IssuedQty"]) < 0)
                    {
                        Orderbal = 0;
                    }
                    else
                    {
                        Orderbal = Convert.ToInt32(ds1.Tables[0].Rows[i]["Qtyrequired"]) - Convert.ToInt32(ds1.Tables[0].Rows[i]["IssuedQty"]);
                    }
                    sht.Range("I" + row).SetValue(Orderbal);
                    TOBal = TOBal + Orderbal;

                    sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["RecJobWork"]);
                    TWRQTY = TWRQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecJobWork"]);
                    if (Convert.ToInt32(ds1.Tables[0].Rows[i]["IssuedQty"]) - Convert.ToInt32(ds1.Tables[0].Rows[i]["RecJobWork"]) < 0)
                    {
                        Balance = 0;
                    }
                    else
                    {
                        Balance = Convert.ToInt32(ds1.Tables[0].Rows[i]["IssuedQty"]) - Convert.ToInt32(ds1.Tables[0].Rows[i]["RecJobWork"]);
                    }
                    sht.Range("K" + row).SetValue(Balance);
                    TWBal = TWBal + Balance;

                    sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["RecBACKING"]);
                    TGIQTY = TGIQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecBACKING"]);
                    sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["RecEMBRODINGINTERNALPROCESS"]);
                    TBIQTY = TBIQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecEMBRODINGINTERNALPROCESS"]);
                    sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["RecLABELINTERNALPROCESS"]);
                    TKIQTY = TKIQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecLABELINTERNALPROCESS"]);
                    sht.Range("O" + row).SetValue(ds1.Tables[0].Rows[i]["RecLACE"]);
                    TKQTY = TKQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecLACE"]);
                    sht.Range("P" + row).SetValue(ds1.Tables[0].Rows[i]["RecGODOWNINTERNALPROCESS"]);
                    TEIQTY = TEIQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecGODOWNINTERNALPROCESS"]);
                    sht.Range("Q" + row).SetValue(ds1.Tables[0].Rows[i]["RecREPAIRING"]);
                    TEQTY = TEQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecREPAIRING"]);
                    sht.Range("R" + row).SetValue(ds1.Tables[0].Rows[i]["RecKNOTTING"]);
                    TMIQTY = TMIQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecKNOTTING"]);
                    sht.Range("S" + row).SetValue(ds1.Tables[0].Rows[i]["RecBINDING"]);
                    TSIQTY = TSIQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecBINDING"]);
                    sht.Range("T" + row).SetValue(ds1.Tables[0].Rows[i]["RecNEWADH"]);
                    TSQTY = TSQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecNEWADH"]);
                    sht.Range("U" + row).SetValue(ds1.Tables[0].Rows[i]["RecROPPING"]);
                    TLIQTY = TLIQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecROPPING"]);
                    sht.Range("V" + row).SetValue(ds1.Tables[0].Rows[i]["RecPACKING"]);
                    TLQTY = TLQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecPACKING"]);
                    sht.Range("W" + row).SetValue(ds1.Tables[0].Rows[i]["RecSTITCHING"]);
                    TRIQTY = TRIQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecSTITCHING"]);

                    sht.Range("X" + row).SetValue(ds1.Tables[0].Rows[i]["RecKONESILAI"]);
                    TREQTY = TREQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecKONESILAI"]);
                    sht.Range("Y" + row).SetValue(ds1.Tables[0].Rows[i]["RecLABEL"]);
                    TPIQTY = TPIQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecLABEL"]);
                    sht.Range("Z" + row).SetValue(ds1.Tables[0].Rows[i]["RecSTRETCHINGTEDA"]);
                    TPQTY = TPQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecSTRETCHINGTEDA"]);
                    

                    sht.Range("AA" + row).SetValue(ds1.Tables[0].Rows[i]["RecSTITTCHING2"]);
                    TSQty2 = TSQty2 + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecSTITTCHING2"]);
                    
                    sht.Range("AB" + row).SetValue(ds1.Tables[0].Rows[i]["RecEMBROIDERY"]);
                    TREMQTY = TREMQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecEMBROIDERY"]);
                    
                    sht.Range("AC" + row).SetValue(ds1.Tables[0].Rows[i]["RecSTITCHINGINTERNALPROCESS"]);
                    TSIPQTY = TSIPQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecSTITCHINGINTERNALPROCESS"]);
                   
                    sht.Range("AD" + row).SetValue(ds1.Tables[0].Rows[i]["RecPACKINGINTERNALPROCESS"]);
                    TPIPQTY = TPIPQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecPACKINGINTERNALPROCESS"]);
                  
                    sht.Range("AE" + row).SetValue(ds1.Tables[0].Rows[i]["RecMARKEENKHOLISILAI"]);
                    TMKHQTY = TMKHQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecMARKEENKHOLISILAI"]);
                   
                    sht.Range("AF" + row).SetValue(ds1.Tables[0].Rows[i]["RecSTAANKISILAI"]);
                    TSSQTY = TSSQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecSTAANKISILAI"]);
                   
                    sht.Range("AG" + row).SetValue(ds1.Tables[0].Rows[i]["RecDISPATCH"]);
                    TRDISQTY = TRDISQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecDISPATCH"]);
                   
                    sht.Range("AH" + row).SetValue(ds1.Tables[0].Rows[i]["RecHANDSTITMUNNA"]);
                    THSQTY = THSQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecHANDSTITMUNNA"]);
                    
                    sht.Range("AI" + row).SetValue(ds1.Tables[0].Rows[i]["RecPRINTING"]);
                    TRPQTY = TRPQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecPRINTING"]);
                   
                    sht.Range("AJ" + row).SetValue(ds1.Tables[0].Rows[i]["RecFILLERSTITCHING"]);
                    TRFSQTY = TRFSQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecFILLERSTITCHING"]);
                   
                    sht.Range("AK" + row).SetValue(ds1.Tables[0].Rows[i]["RecLABLE2"]);
                    TRL2QTY = TRL2QTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecLABLE2"]);
                    
                    sht.Range("AL" + row).SetValue(ds1.Tables[0].Rows[i]["RecWASHING"]);
                    TRWQTY = TRWQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecWASHING"]);
                   
                    sht.Range("AM" + row).SetValue(ds1.Tables[0].Rows[i]["RecOUTDOORFINISHING"]);
                    TROFQTY = TROFQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecOUTDOORFINISHING"]);
                   
                    sht.Range("AN" + row).SetValue(ds1.Tables[0].Rows[i]["RecTUFTING"]);
                    TRTQTY = TRTQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecTUFTING"]);
                   
                    sht.Range("AO" + row).SetValue(ds1.Tables[0].Rows[i]["RecCHHOTI"]);
                    TRCQTY = TRCQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecCHHOTI"]);
                    
                    sht.Range("AP" + row).SetValue(ds1.Tables[0].Rows[i]["RecSTITCHING3"]);
                    TRS3QTY = TRS3QTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecSTITCHING3"]);
                  
                    sht.Range("AQ" + row).SetValue(ds1.Tables[0].Rows[i]["RecZIGZAGSTITCH"]);
                    TRZZQTY = TRZZQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecZIGZAGSTITCH"]);
                    
                    sht.Range("AR" + row).SetValue(ds1.Tables[0].Rows[i]["RecTASSEL"]);
                    TRTAQTY = TRTAQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecTASSEL"]);
                    
                    sht.Range("AS" + row).SetValue(ds1.Tables[0].Rows[i]["RecHANDSTITCH2"]);
                    TRHSQTY = TRHSQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecHANDSTITCH2"]);
                    
                    sht.Range("AT" + row).SetValue(ds1.Tables[0].Rows[i]["RecREPACKING"]);
                    TRREQTY = TRREQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecREPACKING"]);
                    
                    sht.Range("AU" + row).SetValue(ds1.Tables[0].Rows[i]["RecRESTITCHING"]);
                    TRSTIQTY = TRSTIQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecRESTITCHING"]);
                    
                    sht.Range("AV" + row).SetValue(ds1.Tables[0].Rows[i]["RecRELABEL"]);
                    TRRLQTY = TRRLQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecRELABEL"]);
                    
                    sht.Range("AW" + row).SetValue(ds1.Tables[0].Rows[i]["RecREPACKING2"]);
                    TRP2QTY = TRP2QTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecREPACKING2"]);
                   
                    sht.Range("AX" + row).SetValue(ds1.Tables[0].Rows[i]["RecSERGING"]);
                    TRSQTY = TRSQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecSERGING"]);
                 
                    sht.Range("AY" + row).SetValue(ds1.Tables[0].Rows[i]["RecFINISHING"]);
                    TRFINQTY = TRFINQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecFINISHING"]);
                   
                    sht.Range("AZ" + row).SetValue(ds1.Tables[0].Rows[i]["RecREMARKEENKHOLISILAI"]);
                    TRREHIQTY = TRREHIQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecREMARKEENKHOLISILAI"]);
                  
                    sht.Range("BA" + row).SetValue(ds1.Tables[0].Rows[i]["RecREFINISHING"]);
                    TRRFIQTY = TRRFIQTY + Convert.ToInt32(ds1.Tables[0].Rows[i]["RecREFINISHING"]);
                    row = row + 1;

                    //**************************

                }
                //***************Total
                sht.Range("G" + row).SetValue(TOQTY);
                sht.Range("H" + row).SetValue(TWOQTY);
                sht.Range("I" + row).SetValue(TOBal);
                sht.Range("J" + row).SetValue(TWRQTY);
                sht.Range("K" + row).SetValue(TWBal);

                sht.Range("L" + row).SetValue(TGIQTY);
                sht.Range("M" + row).SetValue(TBIQTY);
                sht.Range("N" + row).SetValue(TKIQTY);
                sht.Range("O" + row).SetValue(TKQTY);
                sht.Range("P" + row).SetValue(TEIQTY);
                sht.Range("Q" + row).SetValue(TEQTY);
                sht.Range("R" + row).SetValue(TMIQTY);
                sht.Range("S" + row).SetValue(TSIQTY);
                sht.Range("T" + row).SetValue(TSQTY);
                sht.Range("U" + row).SetValue(TLIQTY);
                sht.Range("V" + row).SetValue(TLQTY);
                sht.Range("W" + row).SetValue(TRIQTY);
                sht.Range("X" + row).SetValue(TREQTY);
                sht.Range("Y" + row).SetValue(TPIQTY);
                sht.Range("Z" + row).SetValue(TPQTY);

                sht.Range("AA" + row).SetValue(TSQty2);
                sht.Range("AB" + row).SetValue(TREMQTY);
                sht.Range("AC" + row).SetValue(TSIPQTY);
                sht.Range("AD" + row).SetValue(TPIPQTY);
                sht.Range("AE" + row).SetValue(TMKHQTY);
                sht.Range("AF" + row).SetValue(TSSQTY);
                sht.Range("AG" + row).SetValue(TRDISQTY);
                sht.Range("AH" + row).SetValue(THSQTY);
                sht.Range("AI" + row).SetValue(TRPQTY);
                sht.Range("AJ" + row).SetValue(TRFSQTY);
                sht.Range("AK" + row).SetValue(TRL2QTY);
                sht.Range("AL" + row).SetValue(TRWQTY);
                sht.Range("AM" + row).SetValue(TROFQTY);
                sht.Range("AN" + row).SetValue(TRTQTY);
                sht.Range("AO" + row).SetValue(TRCQTY);
                sht.Range("AP" + row).SetValue(TRS3QTY);
                sht.Range("AQ" + row).SetValue(TRZZQTY);
                sht.Range("AR" + row).SetValue(TRTAQTY);
                sht.Range("AS" + row).SetValue(TRHSQTY);
                sht.Range("AT" + row).SetValue(TRREQTY);
                sht.Range("AU" + row).SetValue(TRSTIQTY);
                sht.Range("AV" + row).SetValue(TRRLQTY);
                sht.Range("AW" + row).SetValue(TRP2QTY);
                sht.Range("AX" + row).SetValue(TRSQTY);
                sht.Range("AY" + row).SetValue(TRFINQTY);
                sht.Range("AZ" + row).SetValue(TRREHIQTY);
                sht.Range("BA" + row).SetValue(TRRFIQTY);


                sht.Range("A" + row + ":C" + row).Merge();
                sht.Range("A" + row).Value = "OrderWise Total : ";
                //sht.Range("A" + row).Value = dr["Processname"] + " " + "Total : ";
                sht.Range("A" + row + ":C" + row).Style.Font.Bold = true;

                sht.Range("A" + row + ":BA" + row).Style.Fill.BackgroundColor = XLColor.AshGrey;
                sht.Range("G" + row + ":BA" + row).Style.Font.Bold = true;
                row = row + 1;

            }


            using (var a = sht.Range("A" + Orderlastrow + ":BA" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*************
            sht.Columns(1, 30).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("OrderWiseProcessDetailHafiziaRecQty_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "o2", "alert('No Record Found!');", true);
        }
        ds.Dispose();

    }
    private void Processwisedetail()
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@localOrderNo", txtlocalOrderNo.Text);
        param[1] = new SqlParameter("@userid", Session["varuserid"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_getProcessWiseDetailHaf", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["GetDataset"] = ds;
            if (Session["varcompanyid"].ToString() == "9")
            {
                Session["rptFileName"] = "~\\Reports\\RptprocesswisedetailforHaf.rpt";
            }
            //else
            //{
            //    Session["rptFileName"] = "~\\Reports\\Rptvendorwisedetail.rpt";
            //}
            Session["dsFileName"] = "~\\ReportSchema\\RptprocesswisedetailforHaf.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }
        ds.Dispose();
    }
    private void OrderProcesswise()
    {
        try
        {
            string url = "../../excel.aspx?DDCompany=" + DDCompany.SelectedValue + "&DDCustCode=" + DDCustCode.SelectedValue
                 + "&DDOrderNo=" + DDOrderNo.SelectedValue + "&DDCategory=" + DDCategory.SelectedValue + "&ddItemName=" + ddItemName.SelectedValue
                 + "&DDQuality=" + DDQuality.SelectedValue + "&DDDesign=" + DDDesign.SelectedValue + "&DDColor=" + DDColor.SelectedValue
                 + "&name=ProcessDetail&ac=pwd&DDSize=" + DDSize.SelectedValue + "&mastercompanyid=" + Session["VarCompanyId"].ToString();
            
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('"+ url + "');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

            //DataSet ds = SqlHelperNew.ExecuteDataset("PRO_TEMP_JITE",
            //    Convert.ToInt32(DDCompany.SelectedValue == "" ? "0" : DDCompany.SelectedValue),
            //    Convert.ToInt32(DDCustCode.SelectedValue == "" ? "0" : DDCustCode.SelectedValue), 
            //    Convert.ToInt32(DDOrderNo.SelectedValue == "" ? "0" : DDOrderNo.SelectedValue),
            //    Convert.ToInt32(DDCategory.SelectedValue == "" ? "0" : DDCategory.SelectedValue),
            //    Convert.ToInt32(ddItemName.SelectedValue =="" ?"0": ddItemName.SelectedValue),
            //    Convert.ToInt32(DDQuality.SelectedValue == "" ? "0" : DDQuality.SelectedValue),
            //    Convert.ToInt32(DDDesign.SelectedValue == "" ? "0" : DDDesign.SelectedValue),
            //    Convert.ToInt32(DDColor.SelectedValue == "" ? "0" : DDColor.SelectedValue), 1, "GET");

        }
        catch (Exception ex)
        {
            throw new Exception("ServiceRequest|" + ex.Message);
        }


    }
    protected void ChkForDate_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForDate.Checked == true)
        {
            ChkForRecQty.Checked = false;
            TRMonthyear.Visible = true;
        }
        else
        {
            ChkForRecQty.Checked = false;
            TRMonthyear.Visible = false;
        }
    }
    protected void ChkForRecQty_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForRecQty.Checked == true)
        {
            ChkForDate.Checked = false;
            TRMonthyear.Visible = true;
        }
        else
        {
            ChkForDate.Checked = false;
            TRMonthyear.Visible = false;
        }
    }
    protected void CustomerOrderInvoiceStatus()
    {
        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@customerid", DDCustCode.SelectedIndex > 0 ? DDCustCode.SelectedValue : "0");
        param[2] = new SqlParameter("@orderid", DDOrderNo.SelectedIndex > 0 ? DDOrderNo.SelectedValue : "0");
        param[3] = new SqlParameter("@Where", "");
        param[4] = new SqlParameter("@Sizeunit", DDsizeunit.SelectedValue);
        //***********
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETCUSTOMERORDERINVOICESTATUS", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["GetDataset"] = ds;
            Session["rptFileName"] = "~\\Reports\\RptCustomerOrderInvoiceStatus.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptCustomerOrderInvoiceStatus.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "ostatus", "alert('No Record Found!');", true);
        }

    }
    protected void POOrderstatusDeepakRugs()
    {
        String str = "";
        str = str + " and cast(OM.OrderDate as date)>='" + txtfromdate.Text + "' and cast(Om.OrderDate as date)<='" + txttodate.Text + "'";
        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " and OM.customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.qualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf.designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and vf.sizeid=" + DDSize.SelectedValue;
        }
        if (txtlocalOrderNo.Text != "")
        {
            str = str + " and OM.LocalOrder='" + txtlocalOrderNo.Text + "'";
        }
        //if (DDorderstatus.SelectedIndex == 1)
        //{
        //    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        //}
        //else if (DDorderstatus.SelectedIndex == 2)
        //{
        //    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        //}
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@Where", str);

        //param[1] = new SqlParameter("@customerid", DDCustCode.SelectedIndex > 0 ? DDCustCode.SelectedValue : "0");
        //param[2] = new SqlParameter("@orderid", DDOrderNo.SelectedIndex > 0 ? DDOrderNo.SelectedValue : "0");
        //param[3] = new SqlParameter("@Where", str);
        //param[4] = new SqlParameter("@Sizeunit", DDsizeunit.SelectedValue);
        //***********
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETPOORDERSTATUSDEEPAKRUGS", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["GetDataset"] = ds;
            Session["rptFileName"] = "~\\Reports\\RptPOOrderStatusDeepak.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptPOOrderStatusDeepak.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "ostatus", "alert('No Record Found!');", true);
        }

    }
    private void CustomerOrderInternalOCReport()
    {

        SqlParameter[] param = new SqlParameter[1];
        param[0] = new SqlParameter("@orderid", DDOrderNo.SelectedValue);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetCustomerOrderInternalOCReport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["GetDataset"] = ds;
            Session["rptFileName"] = "~\\Reports\\RptLocalOC.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptLocalOC.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
        ds.Dispose();
    }
    protected void BazaarCompleteStatus()
    {
        String str = "";

        if (ChkForDate.Checked == true)
        {
            str = str + " and PRM.ReceiveDate>='" + txtfromdate.Text + "' and PRM.ReceiveDate<='" + txttodate.Text + "'";
        }
        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " and OM.customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.qualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf.designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and vf.sizeid=" + DDSize.SelectedValue;
        }
        if (txtlocalOrderNo.Text != "")
        {
            str = str + " and OM.LocalOrder='" + txtlocalOrderNo.Text + "'";
        }
        //if (DDorderstatus.SelectedIndex == 1)
        //{
        //    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        //}
        //else if (DDorderstatus.SelectedIndex == 2)
        //{
        //    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        //}
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GETBAZAARCOMPLETESTATUSHAFIZIA", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@Where", str);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        //cmd.Parameters.AddWithValue("@status", DDorderstatus.SelectedValue);
        DataTable dt = new DataTable();
        dt.Load(cmd.ExecuteReader());
        //*************
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        con.Close();
        con.Dispose();
        //SqlParameter[] param = new SqlParameter[3];
        //param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        //param[1] = new SqlParameter("@Where", str);
        //param[2] = new SqlParameter("@userid", Session["varuserid"]);
        //***********
        // DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_getPoorderstatus", param);

        if (ds.Tables[0].Rows.Count > 0)
        {

            try
            {
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                //************
                sht.Range("A1:J1").Merge();
                sht.Range("A1").SetValue("BAZAAR COMPLETE STATUS");
                sht.Range("A1:J1").Style.Font.Bold = true;
                sht.Range("A1:J1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //JOB NAME
                sht.Range("H2:J2").Merge();
                sht.Range("H2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H2:J2").Style.Font.Bold = true;


                sht.Range("A3").SetValue("Local OrderNo");
                sht.Range("B3").SetValue("Item Name");
                sht.Range("C3").SetValue("Quality");
                sht.Range("D3").SetValue("Design");
                sht.Range("E3").SetValue("Color");
                sht.Range("F3").SetValue("Shape");
                sht.Range("G3").SetValue("Size");
                sht.Range("H3").SetValue("Last Rec Pcs Date");

                sht.Range("A3:H3").Style.Font.Bold = true;

                int row = 4;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //if (i == 0)
                    //{
                    //    sht.Range("H2").SetValue(ds.Tables[0].Rows[0]["JobName"]);
                    //}

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["LocalOrder"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ShapeName"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["size"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ReceiveDate"]);


                    row = row + 1;
                }
                using (var a = sht.Range("A1" + ":H" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }
                //********************
                sht.Columns(1, 50).AdjustToContents();
                //******************
                String Path = "";
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("BAZAARCOMPLETESTATUS_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }


            //if (chkexcelexport.Checked == true)
            //{
            //    FinishingStatusexcelexport(ds);
            //}
            //else
            //{
            //    Session["GetDataset"] = ds;
            //    Session["rptFileName"] = "~\\Reports\\RptfinishingStatus.rpt";
            //    Session["dsFileName"] = "~\\ReportSchema\\RptfinishingStatus.xsd";
            //    StringBuilder stb = new StringBuilder();
            //    stb.Append("<script>");
            //    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            //    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            //}
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
        }

    }
    protected void PoStatusWithOrderValue()
    {
        String str = "";
        str = str + " and cast(OM.Dateadded as date)>='" + txtfromdate.Text + "' and cast(Om.Dateadded as date)<='" + txttodate.Text + "'";
        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " and OM.customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.qualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf.designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and vf.sizeid=" + DDSize.SelectedValue;
        }
        if (txtlocalOrderNo.Text != "")
        {
            str = str + " and OM.LocalOrder='" + txtlocalOrderNo.Text + "'";
        }
        if (DDorderstatus.SelectedIndex > 0)
        {
            str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        }
        //else if (DDorderstatus.SelectedIndex == 2)
        //{
        //    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        //}
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GETPOORDERWITHVALUESTATUS", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@Where", str);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@status", DDorderstatus.SelectedValue);
        DataTable dt = new DataTable();
        dt.Load(cmd.ExecuteReader());
        //*************
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        con.Close();
        con.Dispose();
        //SqlParameter[] param = new SqlParameter[3];
        //param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        //param[1] = new SqlParameter("@Where", str);
        //param[2] = new SqlParameter("@userid", Session["varuserid"]);
        //***********
        // DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_getPoorderstatus", param);

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
            //**************

            sht.Range("A1:N1").Merge();
            sht.Range("A1").SetValue(DDCompany.SelectedItem.Text);
            sht.Range("A1:N1").Style.Font.Bold = true;
            sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:N1").Style.Font.FontName = "Calibri";
            sht.Range("A1:N1").Style.Font.FontSize = 12;

            sht.Range("A2:N2").Merge();
            sht.Range("A2").SetValue("ORDER REPORT FROM : " + txtfromdate.Text + " To " + txttodate.Text);
            sht.Range("A2:N2").Style.Font.Bold = true;
            sht.Range("A2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:N2").Style.Font.FontName = "Calibri";
            sht.Range("A1:N2").Style.Font.FontSize = 12;

            sht.Range("A3:N3").Merge();
            sht.Range("A3").SetValue("");
            sht.Range("A3:N2").Style.Font.Bold = true;
            sht.Range("A3:N3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);


            //*******************BUYER ORDER DETAIL
            //Headers
            sht.Range("A4").Value = "BUYER CODE";
            sht.Range("B4").Value = "PO STATUS";
            sht.Range("C4").Value = "PO TYPE";
            sht.Range("D4").Value = "LOCAL PO";
            sht.Range("E4").Value = "BUYER PO";
            sht.Range("F4").Value = "ORDER DATE";
            sht.Range("G4").Value = "ORDER ENTRY DATE";
            sht.Range("H4").Value = "DELIVERY DATE";
            sht.Range("I4").Value = "ORDER UNIT";
            sht.Range("J4").Value = "PO AREA";
            sht.Range("K4").Value = "PO AREA SQ YD";
            sht.Range("L4").Value = "CURRENCY";
            sht.Range("M4").Value = "ORDER VALUE";
            sht.Range("N4").Value = "ORDER VALUE INR";

            sht.Range("A4:N4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A4:N4").Style.Font.Bold = true;
            sht.Range("A4:N4").Style.Font.FontName = "Calibri";
            sht.Range("A4:N4").Style.Font.FontSize = 11;

            row = 5;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                //sht.Range("A" + row + ":N" + row).Style.Font.Bold = true;
                sht.Range("A" + row + ":N" + row).Style.Font.FontName = "Calibri";
                sht.Range("A" + row + ":N" + row).Style.Font.FontSize = 10;

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["customercode"]); //"BUYER CODE";
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Postatus"]); // = "PO TYPE";
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Potype"].ToString());  //"LPO";
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["LPO"].ToString());  //"BPO";
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["BPO"]);  //"ORD DT";
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ORDDT"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["ORDENTRYDATE"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["DELvdate"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["ORDUNIT"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["POAREA"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["POAREASQYD"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["CurrencyName"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["OrderValue"]);
                sht.Range("N" + row).SetValue("");

                row = row + 1;
            }
            //********************

            using (var a = sht.Range("A4" + ":N" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            sht.Columns(1, 50).AdjustToContents();
            //******************
            sht.Row(1).Height = 28.80;
            //sht.Column("J").Width = 13.78;
            //sht.Column("K").Width = 13.78;
            //sht.Column("N").Width = 13.78;
            sht.Range("A4:N4").Style.Alignment.WrapText = true;

            string Fileextension = "xlsx";
            string filename = "";
            filename = UtilityModule.validateFilename("POORDERWITHVALUESTATUS_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);


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
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
        }

    }

    protected void PoStatusReportType2()
    {
        String str = "";
        str = str + " and cast(OM.Dateadded as date)>='" + txtfromdate.Text + "' and cast(Om.Dateadded as date)<='" + txttodate.Text + "'";
        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " and OM.customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.qualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf.designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and vf.sizeid=" + DDSize.SelectedValue;
        }
        if (txtlocalOrderNo.Text != "")
        {
            str = str + " and OM.LocalOrder='" + txtlocalOrderNo.Text + "'";
        }
        if (DDorderstatus.SelectedIndex > 0)
        {
            str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        }
        //else if (DDorderstatus.SelectedIndex == 2)
        //{
        //    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        //}
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("pro_getPoorderstatus", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@Where", str);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@status", DDorderstatus.SelectedValue);
        cmd.Parameters.AddWithValue("@POReportType2", ChkForPOStatusReportType2.Checked == true ? "1" : "0");
        DataTable dt = new DataTable();
        dt.Load(cmd.ExecuteReader());
        //*************
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        con.Close();
        con.Dispose();
        //SqlParameter[] param = new SqlParameter[3];
        //param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        //param[1] = new SqlParameter("@Where", str);
        //param[2] = new SqlParameter("@userid", Session["varuserid"]);
        //***********
        // DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_getPoorderstatus", param);

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
            //**************

            sht.Range("A1:V1").Merge();
            sht.Range("A1").SetValue("REORT FROM : " + txtfromdate.Text + " To " + txttodate.Text);
            sht.Range("A1:V1").Style.Font.Bold = true;
            sht.Range("A1:V1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:V1").Style.Font.FontName = "Calibri";
            sht.Range("A1:V1").Style.Font.FontSize = 12;

            //*******************BUYER ORDER DETAIL
            //Headers
            sht.Range("A2").Value = "BUYER CODE";
            sht.Range("B2").Value = "ORDER NO";
            sht.Range("C2").Value = "PO TYPE";
            sht.Range("D2").Value = "ORD DT";
            sht.Range("E2").Value = "ORD ENTRY DATE";
            sht.Range("F2").Value = "DELV DT";
            sht.Range("G2").Value = "BPO";
            sht.Range("H2").Value = "PO AREA SQ YD";
            sht.Range("I2").Value = "ITEM NAME";
            //***********
            sht.Range("J2").Value = "QUALITY";
            sht.Range("K2").Value = "DESIGN";
            sht.Range("L2").Value = "COLOR";
            sht.Range("M2").Value = "OSIZE";
            sht.Range("N2").Value = "MAP SIZE";
            sht.Range("O2").Value = "ORD QTY";
            sht.Range("P2").Value = "TOT ISS QTY";
            sht.Range("Q2").Value = "TOT ISS DUE";
            sht.Range("R2").Value = "TOT BAZAR QTY";
            sht.Range("S2").Value = "TOT BAZAR DUE";
            sht.Range("T2").Value = "FINISHED QTY";
            sht.Range("U2").Value = "DISP QTY";
            sht.Range("V2").Value = "BALANCE IN PNM";

            sht.Range("O2:V2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            //sht.Range("P1:Q1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A2:V2").Style.Font.Bold = true;

            row = 3;
            int ORDQTY = 0, ISSQTY = 0, ISSDUE = 0, Bazarqty = 0, BazarDue = 0, DispQty = 0, DispDue = 0, PackedfromOther = 0, PackedtoOther = 0, Packedqty = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["customercode"]); //"BUYER CODE";
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["LPO"]); // = "Local OrderNo";
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Potype"].ToString());  //"Potype";
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ORDDT"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ORDENTRYDATE"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["DELvdate"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["BPO"]);  //"Buyer OrderNo";
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["POAREASQYD"]);

                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["DESIGNNAME"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["COLORNAME"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["OSiZE"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["MAPSIZE"]);
                ORDQTY = Convert.ToInt32(ds.Tables[0].Rows[i]["ORDQTY"]);
                sht.Range("O" + row).SetValue(ORDQTY);// = "ORD QTY";
                ISSQTY = Convert.ToInt32(ds.Tables[0].Rows[i]["ISSQTY"]);
                sht.Range("P" + row).SetValue(ISSQTY);// = "ISS QTY";
                ISSDUE = ORDQTY - ISSQTY;
                sht.Range("Q" + row).SetValue((ISSDUE < 0 ? 0 : ISSDUE));// = "ISS DUE";
                Bazarqty = Convert.ToInt32(ds.Tables[0].Rows[i]["Bazarqty"]);
                sht.Range("R" + row).SetValue(Bazarqty);// = "BAZAR QTY";
                BazarDue = ORDQTY - Bazarqty;
                sht.Range("S" + row).SetValue((BazarDue < 0 ? 0 : BazarDue));// = "BAZAR DUE";
                sht.Range("T" + row).SetValue(ds.Tables[0].Rows[i]["finishedqty"]);  //"FINISHED QTY";
                Packedqty = Convert.ToInt32(ds.Tables[0].Rows[i]["Packedqty"]);
                PackedfromOther = Convert.ToInt32(ds.Tables[0].Rows[i]["PACKED_From_OTHER"]);
                PackedtoOther = Convert.ToInt32(ds.Tables[0].Rows[i]["PACKED_TO_OTHER"]);
                DispQty = Packedqty + PackedfromOther;
                //DispDue = ORDQTY - DispQty;
                sht.Range("U" + row).SetValue(DispQty);// = "DISP QTY";
                sht.Range("V" + row).SetValue(ORDQTY - DispQty);

                row = row + 1;
            }
            //********************
            sht.Columns(1, 50).AdjustToContents();
            //******************
            sht.Row(1).Height = 28.80;
            sht.Range("A2:AT2").Style.Alignment.WrapText = true;

            string Fileextension = "xlsx";
            string filename = "";
            if (RDPOSTATUS.Checked == true)
            {
                filename = UtilityModule.validateFilename("POSTATUSTYPE2_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            }

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
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
        }

    }
    private void FolioWiseConsumptionDetailReportType2()
    {

        String str = "";
        str = str + " and cast(OM.Dateadded as date)>='" + txtfromdate.Text + "' and cast(Om.Dateadded as date)<='" + txttodate.Text + "'";
        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " and OM.customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
        }
        if (txtlocalOrderNo.Text != "")
        {
            str = str + " and OM.LocalOrder='" + txtlocalOrderNo.Text + "'";
        }
        if (DDEmployeeName.SelectedIndex > 0)
        {
            str = str + " And PIM.EmpID=" + DDEmployeeName.SelectedValue;
        }
        //if (DDorderstatus.SelectedIndex > 0)
        //{
        //    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        //}

        ////int Rowcount = 0;
        ////SqlParameter[] param = new SqlParameter[4];
        ////param[0] = new SqlParameter("@companyId", DDCompany.SelectedValue);
        ////param[1] = new SqlParameter("@processid", 1);
        ////param[2] = new SqlParameter("orderid", DDOrderNo.SelectedValue);
        ////param[3] = new SqlParameter("LocalOrder", txtlocalOrderNo.Text);
        ////DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETFOLIOWISEDETAIL", param);


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GETCUSTOMERORDER_FOLIOWISE_CONSUMPTIONDETAIL", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@processid", 1);
        cmd.Parameters.AddWithValue("@Where", str);
        cmd.Parameters.AddWithValue("@USERID", Session["VarUserId"]);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyId"]);

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************
        con.Close();
        con.Dispose();

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("YARNSTATUS");
            int row = 0;
            //*******************

            sht.Range("A1:M1").Merge();
            sht.Range("A1").SetValue("REORT FROM : " + txtfromdate.Text + " To " + txttodate.Text);
            sht.Range("A1:M1").Style.Font.Bold = true;
            sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:M1").Style.Font.FontName = "Calibri";
            sht.Range("A1:M1").Style.Font.FontSize = 12;

            sht.Range("A2").Value = "BUYER CODE";
            sht.Range("B2").Value = "ORDER NO";
            sht.Range("C2").Value = "LOCAL ORDER NO";
            sht.Range("D2").Value = "DELIVERY DATE";
            sht.Range("E2").Value = "FOLIO NO";

            sht.Range("F2").Value = "FOLIO DATE";

            sht.Range("G2").Value = "DESIGN";
            sht.Range("H2").Value = "COLOR";
            sht.Range("I2").Value = "RAW QUALITY";
            sht.Range("J2").Value = "SHADE NO";
            sht.Range("K2").Value = "CONS QTY";
            sht.Range("L2").Value = "ISSUE QTY";
            sht.Range("M2").Value = "BALANCE QTY";

            sht.Range("A2:M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:M2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("E2:H2").Style.Alignment.WrapText = true;
            sht.Range("A2:M2").Style.Font.SetBold();
            ////sht.Columns("I:L").Width = 10.11;
            //*************          
            row = 3;
            int rowfrom = 0;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Range("A" + row + ":L" + row).Style.Font.SetBold();                

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CUSTOMERCODE"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CUSTOMERORDERNO"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["LOCALORDER"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["DeliveryDate"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ISSUEORDERID"]);

                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["FolioDate"]);

                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["RawMaterialQUALITY"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["SHADECOLORNAME"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["YARNDETAIL"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["ISSUEQTY"]);
                sht.Range("M" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[i]["YARNDETAIL"]) - Convert.ToDecimal(ds.Tables[0].Rows[i]["ISSUEQTY"]));

                row = row + 1;
            }


            //*************
            using (var a = sht.Range("A1" + ":M" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*************
            sht.Columns(1, 12).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("FolioWiseDetailReportType2_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "Foliowise", "alert('No Record Found!');", true);
        }
    }
    protected void OrderSummaryDataWithAllProcess()
    {
        lblMessage.Text = "";
        try
        {
            //*****************
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_ORDERSUMMARYALLJOBWISE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;
            //********
            cmd.Parameters.AddWithValue("@companyId", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@customerid", DDCustCode.SelectedIndex > 0 ? DDCustCode.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@orderid", DDOrderNo.SelectedIndex > 0 ? DDOrderNo.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@Shipfrom", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Shipto", txttodate.Text);
            cmd.Parameters.AddWithValue("@mastercompanyid", Session["varcompanyId"]);
            DataTable dt1 = new DataTable();

            dt1.Load(cmd.ExecuteReader());
            //*************
            DataSet ds = new DataSet();
            ds.Tables.Add(dt1);

            ds.Tables[0].Columns.Remove("orderid");
            ds.Tables[0].Columns.Remove("Item_finished_id");
            ds.Tables[0].Columns.Remove("orderdateexcel");
            ds.Tables[0].Columns.Remove("Dispatchdateexcel");
            ds.Tables[0].Columns.Remove("UNIT");
            //Export to excel
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;

                GridView1.DataSource = ds;
                GridView1.DataBind();
                Response.Clear();
                Response.Buffer = true;

                string filename = "ORDERSUMMARYALLJOBWISE";
                filename = filename + "_From_" + txtfromdate.Text + "_To_ " + txttodate.Text;
                filename = filename + ".xls";

                ////Response.AddHeader("content-disposition",
                //// "attachment;filename=ORDERSUMMARYALLJOBWISE" + DateTime.Now + ".xls");
                Response.AddHeader("content-disposition",
                "attachment;filename=" + filename);
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
                lblMessage.Text = "Done.....";
                //*************
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altordersumm", "alert('No records found...');", true);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "";
        }
    }

    protected void OrderDispatchSummary()
    {
        String str = "";
        str = str + " and cast(OM.Dateadded as date)>='" + txtfromdate.Text + "' and cast(Om.Dateadded as date)<='" + txttodate.Text + "'";
        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " and OM.customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
        }

        //if (ddItemName.SelectedIndex > 0)
        //{
        //    str = str + " and vf.Item_id=" + ddItemName.SelectedValue;
        //}
        //if (DDQuality.SelectedIndex > 0)
        //{
        //    str = str + " and vf.qualityId=" + DDQuality.SelectedValue;
        //}
        //if (DDDesign.SelectedIndex > 0)
        //{
        //    str = str + " and vf.designid=" + DDDesign.SelectedValue;
        //}
        //if (DDColor.SelectedIndex > 0)
        //{
        //    str = str + " and vf.colorid=" + DDColor.SelectedValue;
        //}
        //if (DDSize.SelectedIndex > 0)
        //{
        //    str = str + " and vf.sizeid=" + DDSize.SelectedValue;
        //}
        //if (txtlocalOrderNo.Text != "")
        //{
        //    str = str + " and OM.LocalOrder='" + txtlocalOrderNo.Text + "'";
        //}
        //if (DDorderstatus.SelectedIndex > 0)
        //{
        //    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        //}
        ////else if (DDorderstatus.SelectedIndex == 2)
        ////{
        ////    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        ////}
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GETORDER_DISPATCH_SUMMARY", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@Where", str);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@status", DDorderstatus.SelectedValue);
        DataTable dt = new DataTable();
        dt.Load(cmd.ExecuteReader());
        //*************
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        con.Close();
        con.Dispose();
        //SqlParameter[] param = new SqlParameter[3];
        //param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        //param[1] = new SqlParameter("@Where", str);
        //param[2] = new SqlParameter("@userid", Session["varuserid"]);
        //***********
        // DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_getPoorderstatus", param);

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
            //**************

            sht.Range("A1:P1").Merge();
            sht.Range("A1").SetValue("REORT FROM : " + txtfromdate.Text + " To " + txttodate.Text);
            sht.Range("A1:P1").Style.Font.Bold = true;
            sht.Range("A1:P1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:P1").Style.Font.FontName = "Calibri";
            sht.Range("A1:P1").Style.Font.FontSize = 12;

            //*******************BUYER ORDER DETAIL
            //Headers
            sht.Range("A2").Value = "BUYER CODE";
            sht.Range("B2").Value = "ORDER NO";
            sht.Range("C2").Value = "DELIVERY DATE";
            sht.Range("D2").Value = "ITEM NAME";
            //***********
            sht.Range("E2").Value = "QUALITY";
            sht.Range("F2").Value = "DESIGN";
            sht.Range("G2").Value = "COLOR";
            sht.Range("H2").Value = "SIZE";
            sht.Range("I2").Value = "ORD QTY";
            sht.Range("J2").Value = "ORD AREA";
            sht.Range("K2").Value = "BAZAR PCS";
            sht.Range("L2").Value = "BAZAR AREA";
            sht.Range("M2").Value = "DELIVERY PCS";
            sht.Range("N2").Value = "DELIVERY AREA";
            sht.Range("O2").Value = "BALANCE PCS";
            sht.Range("P2").Value = "BALANCE AREA";


            sht.Range("I2:P2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            //sht.Range("P1:Q1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A2:P2").Style.Font.Bold = true;

            row = 3;
            int ORDQTY = 0, ISSQTY = 0, ISSDUE = 0, Bazarqty = 0, BazarDue = 0, DispQty = 0, DispDue = 0, PackedfromOther = 0, PackedtoOther = 0, Packedqty = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["customercode"]); //"BUYER CODE";
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["LPO"]); // = "Local OrderNo";
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["DELvdate"].ToString());  //"DELvdate";
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["DESIGNNAME"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["COLORNAME"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["MAPSIZE"]);
                ORDQTY = Convert.ToInt32(ds.Tables[0].Rows[i]["ORDQTY"]);
                sht.Range("I" + row).SetValue(ORDQTY);// = "ORD QTY";
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["POAREA"]);
                Bazarqty = Convert.ToInt32(ds.Tables[0].Rows[i]["BAZARQTY"]);
                sht.Range("K" + row).SetValue(Bazarqty);// = "BAZAR QTY";
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["BAZAARPOAREA"]);
                Packedqty = Convert.ToInt32(ds.Tables[0].Rows[i]["Packedqty"]);
                PackedfromOther = Convert.ToInt32(ds.Tables[0].Rows[i]["PACKED_From_OTHER"]);
                PackedtoOther = Convert.ToInt32(ds.Tables[0].Rows[i]["PACKED_TO_OTHER"]);
                DispQty = Packedqty + PackedfromOther;
                //DispDue = ORDQTY - DispQty;
                sht.Range("M" + row).SetValue(DispQty);// = "DISP QTY";
                sht.Range("N" + row).SetValue(Math.Round(DispQty * Convert.ToDecimal(ds.Tables[0].Rows[i]["SinglePcsPOArea"]), 3));// = "DISP Area";
                sht.Range("O" + row).SetValue(ORDQTY - DispQty); ////BalancePcs
                sht.Range("P" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[i]["POAREA"]) - Convert.ToDecimal(DispQty * Convert.ToDecimal(ds.Tables[0].Rows[i]["SinglePcsPOArea"]))); ////BalanceArea

                row = row + 1;
            }
            //********************
            sht.Columns(1, 50).AdjustToContents();
            //******************
            sht.Row(1).Height = 28.80;
            sht.Range("A2:H2").Style.Alignment.WrapText = true;

            string Fileextension = "xlsx";
            string filename = "";
            if (RDOrderDispatchSummary.Checked == true)
            {
                filename = UtilityModule.validateFilename("ORDERDISPATCHSUMMARY_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            }

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
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
        }

    }
    protected void OrderConsumptionWithIndentIssRec()
    {
        // String str = "";
        //// str = str + " and cast(OM.Dateadded as date)>='" + txtfromdate.Text + "' and cast(Om.Dateadded as date)<='" + txttodate.Text + "'";
        // if (DDCustCode.SelectedIndex > 0)
        // {
        //     str = str + " and OM.customerid=" + DDCustCode.SelectedValue;
        // }
        // if (DDOrderNo.SelectedIndex > 0)
        // {
        //     str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
        // }
        // if (txtlocalOrderNo.Text != "")
        // {
        //     str = str + " and OM.LocalOrder='" + txtlocalOrderNo.Text + "'";
        // }


        ////if (ddItemName.SelectedIndex > 0)
        ////{
        ////    str = str + " and vf.Item_id=" + ddItemName.SelectedValue;
        ////}
        ////if (DDQuality.SelectedIndex > 0)
        ////{
        ////    str = str + " and vf.qualityId=" + DDQuality.SelectedValue;
        ////}
        ////if (DDDesign.SelectedIndex > 0)
        ////{
        ////    str = str + " and vf.designid=" + DDDesign.SelectedValue;
        ////}
        ////if (DDColor.SelectedIndex > 0)
        ////{
        ////    str = str + " and vf.colorid=" + DDColor.SelectedValue;
        ////}
        ////if (DDSize.SelectedIndex > 0)
        ////{
        ////    str = str + " and vf.sizeid=" + DDSize.SelectedValue;
        ////}
        ////if (txtlocalOrderNo.Text != "")
        ////{
        ////    str = str + " and OM.LocalOrder='" + txtlocalOrderNo.Text + "'";
        ////}
        ////if (DDorderstatus.SelectedIndex > 0)
        ////{
        ////    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        ////}
        //////else if (DDorderstatus.SelectedIndex == 2)
        //////{
        //////    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        //////}
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GETORDERWEAVINGCONSUMPTION_WITHINDENTISSRECQTY_WEAVERRAWISSRECQTY", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@OrderId", DDOrderNo.SelectedValue);
        //cmd.Parameters.AddWithValue("@Where", str);
        cmd.Parameters.AddWithValue("@MASTERCOMPANYID", Session["VarCompanyNo"]);
        cmd.Parameters.AddWithValue("@UserID", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@LocalOrderNo", txtlocalOrderNo.Text);
        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        //SqlParameter[] param = new SqlParameter[3];
        //param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        //param[1] = new SqlParameter("@Where", str);
        //param[2] = new SqlParameter("@userid", Session["varuserid"]);
        //***********
        // DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_getPoorderstatus", param);

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
            //**************

            sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            sht.PageSetup.AdjustTo(60);
            sht.PageSetup.PaperSize = XLPaperSize.A4Paper;

            sht.PageSetup.Margins.Top = 1.21;
            sht.PageSetup.Margins.Left = 0.47;
            sht.PageSetup.Margins.Right = 0.36;
            sht.PageSetup.Margins.Bottom = 0.19;
            sht.PageSetup.Margins.Header = 1.20;
            sht.PageSetup.Margins.Footer = 0.3;
            sht.PageSetup.SetScaleHFWithDocument();

            sht.Column("A").Width = 33.11;
            sht.Column("B").Width = 11.67;
            sht.Column("C").Width = 9.11;
            sht.Column("D").Width = 14.22;
            sht.Column("E").Width = 15.22;
            sht.Column("F").Width = 12.89;
            sht.Column("G").Width = 13.67;
            sht.Column("H").Width = 15.89;
            sht.Column("I").Width = 13.67;
            sht.Column("J").Width = 13.67;
            sht.Column("K").Width = 13.67;
            sht.Column("L").Width = 13.67;
            sht.Column("M").Width = 30.33;


            sht.Range("A1:M1").Merge();
            sht.Range("A1").SetValue("SR NO : " + ds.Tables[1].Rows[0]["OrderNo"].ToString() + "");
            sht.Range("A1:M1").Style.Font.Bold = true;
            sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:M1").Style.Font.FontName = "Calibri";
            sht.Range("A1:M1").Style.Font.FontSize = 12;
            sht.Range("A1:M1").Style.Fill.BackgroundColor = XLColor.LightGray;


            row = 2;

            int noofrows2 = 0;
            int i2 = 0;
            string OrderQuality = "";
            string OrderDesign = "";
            string OrderColor = "";

            DataTable DtDistinctOrderDesignColor = ds.Tables[0].DefaultView.ToTable(true, "Quality", "Design", "Color");
            noofrows2 = DtDistinctOrderDesignColor.Rows.Count;

            for (i2 = 0; i2 < noofrows2; i2++)
            {
                OrderQuality = DtDistinctOrderDesignColor.Rows[i2]["Quality"].ToString();
                OrderDesign = DtDistinctOrderDesignColor.Rows[i2]["Design"].ToString();
                OrderColor = DtDistinctOrderDesignColor.Rows[i2]["Color"].ToString();

                sht.Range("A" + row).Value = "डिजाईन";
                sht.Range("A" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Style.Font.FontSize = 14;
                sht.Range("A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row).Style.Alignment.WrapText = true;
                sht.Range("A" + row).Merge();

                sht.Range("B" + row).SetValue(DtDistinctOrderDesignColor.Rows[i2]["Design"]);
                sht.Range("B" + row + ":D" + row).Style.Font.Bold = true;
                sht.Range("B" + row + ":D" + row).Style.Font.FontName = "Calibri";
                sht.Range("B" + row + ":D" + row).Style.Font.FontSize = 14;
                //sht.Range("B" + row + ":D" + row).Style.Font.FontColor = XLColor.White;
                sht.Range("B" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Range("A" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B" + row + ":D" + row).Style.Alignment.WrapText = true;
                //sht.Range("B" + row + ":D" + row).Style.Fill.BackgroundColor = XLColor.Black;
                sht.Range("B" + row + ":D" + row).Merge();


                sht.Range("E" + row).Value = "कलर";
                sht.Range("E" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("E" + row).Style.Font.Bold = true;
                sht.Range("E" + row).Style.Font.FontSize = 14;
                sht.Range("E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E" + row).Style.Alignment.WrapText = true;
                sht.Range("E" + row).Merge();

                sht.Range("F" + row).SetValue(DtDistinctOrderDesignColor.Rows[i2]["Color"]);
                sht.Range("F" + row + ":G" + row).Style.Font.Bold = true;
                sht.Range("F" + row + ":G" + row).Style.Font.FontName = "Calibri";
                sht.Range("F" + row + ":G" + row).Style.Font.FontSize = 14;
                sht.Range("F" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("F" + row + ":G" + row).Style.Alignment.WrapText = true;
                //sht.Range("B" + row + ":D" + row).Style.Fill.BackgroundColor = XLColor.Black;
                sht.Range("F" + row + ":G" + row).Merge();

                sht.Range("H" + row).Value = "Area M²";
                sht.Range("H" + row).Style.Font.FontName = "Calibri";
                sht.Range("H" + row).Style.Font.Bold = true;
                sht.Range("H" + row).Style.Font.FontSize = 14;
                sht.Range("H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H" + row).Style.Alignment.WrapText = true;
                sht.Range("H" + row).Merge();

                decimal OrderExportAreaMt = (Convert.ToDecimal(ds.Tables[0].Compute("sum(EXPORTMTRAREA)", "Quality='" + DtDistinctOrderDesignColor.Rows[i2]["Quality"] + "' and Design='" + DtDistinctOrderDesignColor.Rows[i2]["Design"] + "' and Color='" + DtDistinctOrderDesignColor.Rows[i2]["Color"] + "' ")));

                sht.Range("I" + row).SetValue(OrderExportAreaMt);
                sht.Range("I" + row + ":I" + row).Style.Font.Bold = true;
                sht.Range("I" + row + ":I" + row).Style.Font.FontName = "Calibri";
                sht.Range("I" + row + ":I" + row).Style.Font.FontSize = 14;
                sht.Range("I" + row + ":I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("I" + row + ":I" + row).Style.Alignment.WrapText = true;
                //sht.Range("B" + row + ":D" + row).Style.Fill.BackgroundColor = XLColor.Black;
                sht.Range("I" + row + ":I" + row).Merge();

                row = row + 1;

                sht.Range("A" + row).Value = "क्वालिटी";
                sht.Range("A" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A" + row).Style.Font.Bold = true;
                sht.Range("A" + row).Style.Font.FontSize = 14;
                sht.Range("A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A" + row).Style.Alignment.WrapText = true;
                sht.Range("A" + row).Merge();

                sht.Range("B" + row).Value = "प्रोसेस";
                sht.Range("B" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("B" + row).Style.Font.Bold = true;
                sht.Range("B" + row).Style.Font.FontSize = 14;
                sht.Range("B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B" + row).Style.Alignment.WrapText = true;
                sht.Range("B" + row).Merge();

                sht.Range("C" + row).Value = "ताना/बाना";
                sht.Range("C" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("C" + row).Style.Font.Bold = true;
                sht.Range("C" + row).Style.Font.FontSize = 14;
                sht.Range("C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("C" + row).Style.Alignment.WrapText = true;
                sht.Range("C" + row).Merge();

                sht.Range("D" + row).Value = "काउंट";
                sht.Range("D" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("D" + row).Style.Font.Bold = true;
                sht.Range("D" + row).Style.Font.FontSize = 14;
                sht.Range("D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("D" + row).Style.Alignment.WrapText = true;
                sht.Range("D" + row).Merge();

                sht.Range("E" + row).Value = "कलर ";
                sht.Range("E" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("E" + row).Style.Font.Bold = true;
                sht.Range("E" + row).Style.Font.FontSize = 14;
                sht.Range("E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("E" + row).Style.Alignment.WrapText = true;
                sht.Range("E" + row).Merge();

                sht.Range("F" + row).Value = "दरी की लागत ";
                sht.Range("F" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("F" + row).Style.Font.Bold = true;
                sht.Range("F" + row).Style.Font.FontSize = 14;
                sht.Range("F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("F" + row).Style.Alignment.WrapText = true;
                sht.Range("F" + row).Merge();

                sht.Range("G" + row).Value = "डाई को भेजा गया ";
                sht.Range("G" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("G" + row).Style.Font.Bold = true;
                sht.Range("G" + row).Style.Font.FontSize = 14;
                sht.Range("G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("G" + row).Style.Alignment.WrapText = true;
                sht.Range("G" + row).Merge();

                sht.Range("H" + row).Value = "डाई से प्राप्त किया";
                sht.Range("H" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("H" + row).Style.Font.Bold = true;
                sht.Range("H" + row).Style.Font.FontSize = 14;
                sht.Range("H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("H" + row).Style.Alignment.WrapText = true;
                sht.Range("H" + row).Merge();

                sht.Range("I" + row).Value = "डाई हाउस में बैलेंस";
                sht.Range("I" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("I" + row).Style.Font.Bold = true;
                sht.Range("I" + row).Style.Font.FontSize = 14;
                sht.Range("I" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("I" + row).Style.Alignment.WrapText = true;
                sht.Range("I" + row).Merge();

                sht.Range("J" + row).Value = "एजेंट को दिया";
                sht.Range("J" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("J" + row).Style.Font.Bold = true;
                sht.Range("J" + row).Style.Font.FontSize = 14;
                sht.Range("J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("J" + row).Style.Alignment.WrapText = true;
                sht.Range("J" + row).Merge();

                sht.Range("K" + row).Value = "एजेंट से वापसी";
                sht.Range("K" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("K" + row).Style.Font.Bold = true;
                sht.Range("K" + row).Style.Font.FontSize = 14;
                sht.Range("K" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("K" + row).Style.Alignment.WrapText = true;
                sht.Range("K" + row).Merge();

                sht.Range("L" + row).Value = "टोटल बैलेंस";
                sht.Range("L" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("L" + row).Style.Font.Bold = true;
                sht.Range("L" + row).Style.Font.FontSize = 14;
                sht.Range("L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("L" + row).Style.Alignment.WrapText = true;
                sht.Range("L" + row).Merge();

                sht.Range("M" + row).Value = "विवरण";
                sht.Range("M" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("M" + row).Style.Font.Bold = true;
                sht.Range("M" + row).Style.Font.FontSize = 14;
                sht.Range("M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("M" + row).Style.Alignment.WrapText = true;
                sht.Range("M" + row).Merge();

                sht.Row(row).Height = 60;

                sht.Range("A" + row + ":M" + row).Style.Font.Bold = true;
                sht.Range("A" + row + ":M" + row).Style.Font.FontName = "Calibri";
                sht.Range("A" + row + ":M" + row).Style.Font.FontSize = 11;
                sht.Range("A" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row + ":M" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A" + row + ":M" + row).Style.Alignment.WrapText = true;

                row = row + 1;

                string OItemName = "";
                string OQualityName = "";
                string IShadeColor = "";
                string OShadeColorName = "";
                decimal IssueQty = 0;
                decimal IndentQty = 0;
                decimal IndentRecQty = 0;
                decimal WeaverIssueQty = 0;
                decimal WeaverRecQty = 0;

                DataRow[] foundRows;
                foundRows = ds.Tables[1].Select("OrderQuality='" + DtDistinctOrderDesignColor.Rows[i2]["Quality"] + "' and OrderDesign='" + DtDistinctOrderDesignColor.Rows[i2]["Design"] + "' and OrderColor='" + DtDistinctOrderDesignColor.Rows[i2]["Color"] + "'");

                if (foundRows.Length > 0)
                {
                    foreach (DataRow row3 in foundRows)
                    {
                        OItemName = row3["OItemName"].ToString();
                        OQualityName = row3["QualityName"].ToString();
                        IShadeColor = row3["IShadeColor"].ToString();
                        OShadeColorName = row3["OShadeColor"].ToString();
                        IssueQty = Convert.ToDecimal(row3["Qty"].ToString());
                        IndentQty = Convert.ToDecimal(row3["IndentQty"].ToString());
                        IndentRecQty = Convert.ToDecimal(row3["IndentRecQty"].ToString());
                        WeaverIssueQty = Convert.ToDecimal(row3["WeaverIssueQty"].ToString());
                        WeaverRecQty = Convert.ToDecimal(row3["WeaverReceiveQty"].ToString());

                        sht.Range("A" + row).SetValue(OItemName);
                        sht.Range("B" + row).SetValue("");
                        sht.Range("C" + row).SetValue("");
                        sht.Range("D" + row).SetValue(OQualityName);
                        //sht.Range("C" + row).SetValue(IShadeColor);
                        sht.Range("E" + row).SetValue(OShadeColorName);
                        sht.Range("F" + row).SetValue(IssueQty);

                        decimal TotalLagat = (Convert.ToDecimal(ds.Tables[1].Compute("sum(Qty)", "OItemName='" + row3["OItemName"] + "' and QualityName='" + row3["QualityName"] + "' and OShadeColor='" + row3["OShadeColor"] + "' ")));
                        decimal IndentQtyAfterFormula = (Convert.ToDecimal(IssueQty) * IndentQty) / TotalLagat;
                        sht.Range("G" + row).SetValue(Math.Round(IndentQtyAfterFormula, 3));

                        decimal IndentRecQtyAfterFormula = (Convert.ToDecimal(IssueQty) * IndentRecQty) / TotalLagat;
                        sht.Range("H" + row).SetValue(Math.Round(IndentRecQtyAfterFormula, 3));

                        sht.Range("I" + row).SetValue(Math.Round(IndentQtyAfterFormula - IndentRecQtyAfterFormula, 3));

                        decimal WeaverIssueQtyAfterFormula = (Convert.ToDecimal(IssueQty) * WeaverIssueQty) / TotalLagat;
                        sht.Range("J" + row).SetValue(Math.Round(WeaverIssueQtyAfterFormula, 3));

                        decimal WeaverReceiveQtyAfterFormula = (Convert.ToDecimal(IssueQty) * WeaverRecQty) / TotalLagat;
                        sht.Range("K" + row).SetValue(Math.Round(WeaverReceiveQtyAfterFormula, 3));

                        sht.Range("L" + row).SetValue(Math.Round(IndentRecQtyAfterFormula - WeaverIssueQtyAfterFormula + WeaverReceiveQtyAfterFormula, 3));

                        ////sht.Range("G" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Compute("sum(Qty)", "OItemName='" + row3["OItemName"] + "' and QualityName='" + row3["QualityName"] + "' and IFinishedId='" + row3["IFinishedId"] + "' and FinishedId='" + row3["FinishedId"] + "' ")));


                        sht.Range("A" + row + ":M" + row).Style.Font.Bold = true;
                        sht.Range("A" + row + ":M" + row).Style.Font.FontName = "Calibri";
                        sht.Range("A" + row + ":M" + row).Style.Font.FontSize = 11;
                        sht.Range("A" + row + ":M" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("A" + row + ":M" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                        sht.Range("A" + row + ":M" + row).Style.Alignment.WrapText = true;

                        row = row + 1;

                        //break;
                    }
                }

                using (var a = sht.Range("A2" + ":M" + (row - 1)))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                row = row + 1;
            }

            row = row + 1;

            ////********************
            //sht.Columns(1, 50).AdjustToContents();
            //******************
            sht.Row(1).Height = 28.80;
            //sht.Range("A2:H2").Style.Alignment.WrapText = true;

            string Fileextension = "xlsx";
            string filename = "";

            filename = UtilityModule.validateFilename("ORDERCONSUMPTIONWITHINDENTISSREC_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);


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
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
        }

    }
    private void FolioWiseConsumptionSummaryReportType2()
    {

        String str = "";
        str = str + " and cast(OM.Dateadded as date)>='" + txtfromdate.Text + "' and cast(Om.Dateadded as date)<='" + txttodate.Text + "'";
        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " and OM.customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
        }
        if (txtlocalOrderNo.Text != "")
        {
            str = str + " and OM.LocalOrder='" + txtlocalOrderNo.Text + "'";
        }
        //if (DDorderstatus.SelectedIndex > 0)
        //{
        //    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        //}

        ////int Rowcount = 0;
        ////SqlParameter[] param = new SqlParameter[4];
        ////param[0] = new SqlParameter("@companyId", DDCompany.SelectedValue);
        ////param[1] = new SqlParameter("@processid", 1);
        ////param[2] = new SqlParameter("orderid", DDOrderNo.SelectedValue);
        ////param[3] = new SqlParameter("LocalOrder", txtlocalOrderNo.Text);
        ////DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETFOLIOWISEDETAIL", param);


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GETCUSTOMERORDER_FOLIOWISE_CONSUMPTION_SUMMARY", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@processid", 1);
        cmd.Parameters.AddWithValue("@Where", str);
        cmd.Parameters.AddWithValue("@USERID", Session["VarUserId"]);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyId"]);
        cmd.Parameters.AddWithValue("@EmpId", DDEmployeeName.SelectedValue);

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************
        con.Close();
        con.Dispose();

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("YARNSTATUS");
            int row = 0;
            //*******************

            sht.Range("A1:H1").Merge();
            sht.Range("A1").SetValue("REORT FROM : " + txtfromdate.Text + " To " + txttodate.Text);
            sht.Range("A1:H1").Style.Font.Bold = true;
            sht.Range("A1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:H1").Style.Font.FontName = "Calibri";
            sht.Range("A1:H1").Style.Font.FontSize = 12;

            sht.Range("A2").Value = "FOLIO NO";
            sht.Range("B2").Value = "FOLIO DATE";
            sht.Range("C2").Value = "WEAVER NAME";

            sht.Range("D2").Value = "RAW QUALITY";
            sht.Range("E2").Value = "CONS QTY";
            sht.Range("F2").Value = "ISSUE QTY";
            sht.Range("G2").Value = "BALANCE QTY";

            //sht.Range("A2").Value = "BUYER CODE";
            //sht.Range("B2").Value = "ORDER NO";
            //sht.Range("C2").Value = "DELIVERY DATE";
            //sht.Range("D2").Value = "FOLIO NO";
            //sht.Range("E2").Value = "DESIGN";
            //sht.Range("F2").Value = "COLOR";
            //sht.Range("G2").Value = "RAW QUALITY";
            //sht.Range("H2").Value = "SHADE NO";
            //sht.Range("I2").Value = "CONS QTY";
            //sht.Range("J2").Value = "ISSUE QTY";
            //sht.Range("K2").Value = "BALANCE QTY";

            sht.Range("A2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:G2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("B2:D2").Style.Alignment.WrapText = true;
            sht.Range("A2:G2").Style.Font.SetBold();
            ////sht.Columns("I:L").Width = 10.11;
            //*************          
            row = 3;
            int rowfrom = 0;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Range("A" + row + ":L" + row).Style.Font.SetBold();

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["ISSUEORDERID"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["AssignDate"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);

                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["RawMaterialQUALITY"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["YARNDETAIL"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ISSUEQTY"]);
                sht.Range("G" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[i]["YARNDETAIL"]) - Convert.ToDecimal(ds.Tables[0].Rows[i]["ISSUEQTY"]));

                //sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CUSTOMERCODE"]);
                //sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CUSTOMERORDERNO"]);
                //sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["DeliveryDate"]);
                //sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ISSUEORDERID"]);
                //sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                //sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                //sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["RawMaterialQUALITY"]);
                //sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["SHADECOLORNAME"]);
                //sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["YARNDETAIL"]);
                //sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["ISSUEQTY"]);
                //sht.Range("K" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[i]["YARNDETAIL"]) - Convert.ToDecimal(ds.Tables[0].Rows[i]["ISSUEQTY"]));

                row = row + 1;
            }


            //*************
            using (var a = sht.Range("A1" + ":G" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*************
            sht.Columns(1, 12).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("FolioWiseSummaryReportType2_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "Foliowise", "alert('No Record Found!');", true);
        }
    }

    protected void OrderDetailWIP()
    {
        //String str = "";
        //str = str + " and cast(OM.OrderDate as date)>='" + txtfromdate.Text + "' and cast(Om.OrderDate as date)<='" + txttodate.Text + "'";
        //if (DDCustCode.SelectedIndex > 0)
        //{
        //    str = str + " and OM.customerid=" + DDCustCode.SelectedValue;
        //}
        //if (DDOrderNo.SelectedIndex > 0)
        //{
        //    str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
        //}

        ////if (ddItemName.SelectedIndex > 0)
        ////{
        ////    str = str + " and vf.Item_id=" + ddItemName.SelectedValue;
        ////}
        ////if (DDQuality.SelectedIndex > 0)
        ////{
        ////    str = str + " and vf.qualityId=" + DDQuality.SelectedValue;
        ////}
        ////if (DDDesign.SelectedIndex > 0)
        ////{
        ////    str = str + " and vf.designid=" + DDDesign.SelectedValue;
        ////}
        ////if (DDColor.SelectedIndex > 0)
        ////{
        ////    str = str + " and vf.colorid=" + DDColor.SelectedValue;
        ////}
        ////if (DDSize.SelectedIndex > 0)
        ////{
        ////    str = str + " and vf.sizeid=" + DDSize.SelectedValue;
        ////}
        ////if (txtlocalOrderNo.Text != "")
        ////{
        ////    str = str + " and OM.LocalOrder='" + txtlocalOrderNo.Text + "'";
        ////}
        ////if (DDorderstatus.SelectedIndex > 0)
        ////{
        ////    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        ////}
        //////else if (DDorderstatus.SelectedIndex == 2)
        //////{
        //////    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        //////}
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_ORDERDETAILWIP", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@companyId", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@customerid", DDCustCode.SelectedIndex > 0 ? DDCustCode.SelectedValue : "0");
        cmd.Parameters.AddWithValue("@orderid", DDOrderNo.SelectedIndex > 0 ? DDOrderNo.SelectedValue : "0");
        cmd.Parameters.AddWithValue("@Shipfrom", txtfromdate.Text);
        cmd.Parameters.AddWithValue("@Shipto", txttodate.Text);
        cmd.Parameters.AddWithValue("@mastercompanyid", Session["varcompanyId"]);


        DataTable dt = new DataTable();
        dt.Load(cmd.ExecuteReader());
        //*************
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        con.Close();
        con.Dispose();
        //SqlParameter[] param = new SqlParameter[3];
        //param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        //param[1] = new SqlParameter("@Where", str);
        //param[2] = new SqlParameter("@userid", Session["varuserid"]);
        //***********
        // DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_getPoorderstatus", param);

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
            //**************

            sht.Range("A1:P1").Merge();
            sht.Range("A1").SetValue("REPORT FROM : " + txtfromdate.Text + " To " + txttodate.Text);
            sht.Range("A1:P1").Style.Font.Bold = true;
            sht.Range("A1:P1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:P1").Style.Font.FontName = "Calibri";
            sht.Range("A1:P1").Style.Font.FontSize = 12;

            sht.Column("A").Width = 13.11;
            sht.Column("B").Width = 14.67;
            sht.Column("C").Width = 21.11;
            sht.Column("D").Width = 12.22;
            sht.Column("E").Width = 14.22;
            sht.Column("F").Width = 12.89;
            sht.Column("G").Width = 14.67;
            sht.Column("H").Width = 20.89;
            sht.Column("I").Width = 20.67;
            sht.Column("J").Width = 15.67;
            sht.Column("K").Width = 11.67;
            sht.Column("L").Width = 12.67;
            sht.Column("M").Width = 1.33;
            sht.Column("N").Width = 15.33;
            sht.Column("O").Width = 16.33;
            sht.Column("P").Width = 11.33;
            sht.Column("Q").Width = 17.33;
            sht.Column("R").Width = 26.33;
            sht.Column("S").Width = 16.33;
            sht.Column("T").Width = 16.33;
            sht.Column("U").Width = 16.33;
            sht.Column("V").Width = 16.33;

            //*******************BUYER ORDER DETAIL
            //Headers
            sht.Range("A2").Value = "ORDER TYPE";
            sht.Range("B2").Value = "ORDER STATUS";
            sht.Range("C2").Value = "CUSTOMER NAME";
            sht.Range("D2").Value = "LO ORDERNO";
            sht.Range("E2").Value = "BUYER ORDERNO";
            //***********
            sht.Range("F2").Value = "ORDER DATE";
            sht.Range("G2").Value = "DELIVERY DATE";
            sht.Range("H2").Value = "QUALITY";
            sht.Range("I2").Value = "DESIGN";
            sht.Range("J2").Value = "COLOR";
            sht.Range("K2").Value = "OSIZE";
            sht.Range("L2").Value = "MAP SIZE";
            sht.Range("M2").Value = "";
            sht.Range("N2").Value = "PO AREA(SQYD)";
            sht.Range("O2").Value = "CARPET NUMBER";
            sht.Range("P2").Value = "BZDATE";
            sht.Range("Q2").Value = "PROCESS NAME";
            sht.Range("R2").Value = "EMP NAME";
            sht.Range("S2").Value = "ISSUE CHALLANNO";
            sht.Range("T2").Value = "ISSUE DATE";
            sht.Range("U2").Value = "REC DATE";
            sht.Range("V2").Value = "STATUS";

            sht.Column(13).Hide();


            //sht.Range("I2:P2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            //sht.Range("P1:Q1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A2:V2").Style.Font.Bold = true;

            row = 3;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":V" + row).Style.Alignment.WrapText = true;

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["OrderType"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["OrderStatus"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerName"].ToString());
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["LocalOrder"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["OrderDate"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["DispatchDate"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["OrderSize"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["MapSize"]);
                sht.Range("M" + row).SetValue("");
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["POAreaSQYD"]);
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["TStockNo"]);
                sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["BazaarReceiveDate"]);
                sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["Process_Name"]);
                sht.Range("R" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                sht.Range("S" + row).SetValue(ds.Tables[0].Rows[i]["IssueOrderId"]);
                sht.Range("T" + row).SetValue(ds.Tables[0].Rows[i]["AssignDate"]);
                sht.Range("U" + row).SetValue(ds.Tables[0].Rows[i]["ReceiveDate"]);
                sht.Range("V" + row).SetValue(ds.Tables[0].Rows[i]["Status"]);

                row = row + 1;
            }
            ////********************
            //sht.Columns(1, 50).AdjustToContents();
            ////******************
            sht.Row(1).Height = 28.80;
            sht.Range("A2:R2").Style.Alignment.WrapText = true;

            string Fileextension = "xlsx";
            string filename = "";
            filename = UtilityModule.validateFilename("ORDERDETAILWIP_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);

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
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
        }

    }
    protected void OrderConsumptionSummaryWithWeaverIssRec()
    {
        #region

        // String str = "";
        //// str = str + " and cast(OM.Dateadded as date)>='" + txtfromdate.Text + "' and cast(Om.Dateadded as date)<='" + txttodate.Text + "'";
        // if (DDCustCode.SelectedIndex > 0)
        // {
        //     str = str + " and OM.customerid=" + DDCustCode.SelectedValue;
        // }
        // if (DDOrderNo.SelectedIndex > 0)
        // {
        //     str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
        // }
        // if (txtlocalOrderNo.Text != "")
        // {
        //     str = str + " and OM.LocalOrder='" + txtlocalOrderNo.Text + "'";
        // }


        ////if (ddItemName.SelectedIndex > 0)
        ////{
        ////    str = str + " and vf.Item_id=" + ddItemName.SelectedValue;
        ////}
        ////if (DDQuality.SelectedIndex > 0)
        ////{
        ////    str = str + " and vf.qualityId=" + DDQuality.SelectedValue;
        ////}
        ////if (DDDesign.SelectedIndex > 0)
        ////{
        ////    str = str + " and vf.designid=" + DDDesign.SelectedValue;
        ////}
        ////if (DDColor.SelectedIndex > 0)
        ////{
        ////    str = str + " and vf.colorid=" + DDColor.SelectedValue;
        ////}
        ////if (DDSize.SelectedIndex > 0)
        ////{
        ////    str = str + " and vf.sizeid=" + DDSize.SelectedValue;
        ////}
        ////if (txtlocalOrderNo.Text != "")
        ////{
        ////    str = str + " and OM.LocalOrder='" + txtlocalOrderNo.Text + "'";
        ////}
        ////if (DDorderstatus.SelectedIndex > 0)
        ////{
        ////    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        ////}
        //////else if (DDorderstatus.SelectedIndex == 2)
        //////{
        //////    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        //////}

        #endregion
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GETORDERCONSUMPTION_SUMMARY_WITHINDENTRECQTY_WEAVERRAWISSRECQTY", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@OrderId", DDOrderNo.SelectedValue);
        //cmd.Parameters.AddWithValue("@Where", str);
        cmd.Parameters.AddWithValue("@MASTERCOMPANYID", Session["VarCompanyNo"]);
        cmd.Parameters.AddWithValue("@UserID", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@LocalOrderNo", txtlocalOrderNo.Text);
        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        //SqlParameter[] param = new SqlParameter[3];
        //param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        //param[1] = new SqlParameter("@Where", str);
        //param[2] = new SqlParameter("@userid", Session["varuserid"]);
        //***********
        // DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_getPoorderstatus", param);

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
            //**************

            sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            sht.PageSetup.AdjustTo(60);
            sht.PageSetup.PaperSize = XLPaperSize.A4Paper;

            sht.PageSetup.Margins.Top = 0.21;
            sht.PageSetup.Margins.Left = 0.47;
            sht.PageSetup.Margins.Right = 0.36;
            sht.PageSetup.Margins.Bottom = 0.19;
            sht.PageSetup.Margins.Header = 0.20;
            sht.PageSetup.Margins.Footer = 0.3;
            sht.PageSetup.SetScaleHFWithDocument();

            sht.Column("A").Width = 27.11;
            sht.Column("B").Width = 12.78;
            sht.Column("C").Width = 11.78;
            sht.Column("D").Width = 15.56;
            sht.Column("E").Width = 14.89;
            sht.Column("F").Width = 12.22;
            sht.Column("G").Width = 13.67;
            sht.Column("H").Width = 12.89;
            sht.Column("I").Width = 13.67;
            sht.Column("J").Width = 12.56;
            sht.Column("K").Width = 10.33;
            sht.Column("L").Width = 25.67;
            //sht.Column("M").Width = 30.33;


            sht.Range("A1:L1").Merge();
            sht.Range("A1").SetValue("SR NO : " + ds.Tables[0].Rows[0]["OrderNo"].ToString() + "");
            sht.Range("A1:L1").Style.Font.Bold = true;
            sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:L1").Style.Font.FontName = "Calibri";
            sht.Range("A1:L1").Style.Font.FontSize = 14;
            sht.Range("A1:L1").Style.Fill.BackgroundColor = XLColor.LightGray;

            sht.Range("A2").Value = "क्वालिटी";
            sht.Range("A2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A2").Style.Font.Bold = true;
            sht.Range("A2").Style.Font.FontSize = 14;
            sht.Range("A2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A2").Style.Alignment.WrapText = true;
            sht.Range("A2").Merge();

            sht.Range("B2").Value = "प्रोसेस";
            sht.Range("B2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("B2").Style.Font.Bold = true;
            sht.Range("B2").Style.Font.FontSize = 14;
            sht.Range("B2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("B2").Style.Alignment.WrapText = true;
            sht.Range("B2").Merge();

            sht.Range("C2").Value = "ताना/बाना";
            sht.Range("C2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("C2").Style.Font.Bold = true;
            sht.Range("C2").Style.Font.FontSize = 14;
            sht.Range("C2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("C2").Style.Alignment.WrapText = true;
            sht.Range("C2").Merge();

            sht.Range("D2").Value = "काउंट";
            sht.Range("D2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("D2").Style.Font.Bold = true;
            sht.Range("D2").Style.Font.FontSize = 14;
            sht.Range("D2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("D2").Style.Alignment.WrapText = true;
            sht.Range("D2").Merge();

            sht.Range("E2").Value = "कलर ";
            sht.Range("E2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("E2").Style.Font.Bold = true;
            sht.Range("E2").Style.Font.FontSize = 14;
            sht.Range("E2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("E2").Style.Alignment.WrapText = true;
            sht.Range("E2").Merge();

            sht.Range("F2").Value = "दरी की लागत ";
            sht.Range("F2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("F2").Style.Font.Bold = true;
            sht.Range("F2").Style.Font.FontSize = 14;
            sht.Range("F2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("F2").Style.Alignment.WrapText = true;
            sht.Range("F2").Merge();

            sht.Range("G2").Value = "";
            sht.Range("G2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("G2").Style.Font.Bold = true;
            sht.Range("G2").Style.Font.FontSize = 14;
            sht.Range("G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("G2").Style.Alignment.WrapText = true;
            sht.Range("G2").Merge();

            sht.Range("H2").Value = "डाई से प्राप्त किया";
            sht.Range("H2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("H2").Style.Font.Bold = true;
            sht.Range("H2").Style.Font.FontSize = 14;
            sht.Range("H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("H2").Style.Alignment.WrapText = true;
            sht.Range("H2").Merge();

            sht.Range("I2").Value = "एजेंट को दिया";
            sht.Range("I2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("I2").Style.Font.Bold = true;
            sht.Range("I2").Style.Font.FontSize = 14;
            sht.Range("I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("I2").Style.Alignment.WrapText = true;
            sht.Range("I2").Merge();

            sht.Range("J2").Value = "एजेंट से वापसी";
            sht.Range("J2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("J2").Style.Font.Bold = true;
            sht.Range("J2").Style.Font.FontSize = 14;
            sht.Range("J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("J2").Style.Alignment.WrapText = true;
            sht.Range("J2").Merge();

            sht.Range("K2").Value = "टोटल बैलेंस";
            sht.Range("K2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("K2").Style.Font.Bold = true;
            sht.Range("K2").Style.Font.FontSize = 14;
            sht.Range("K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("K2").Style.Alignment.WrapText = true;
            sht.Range("K2").Merge();

            sht.Range("L2").Value = "विवरण";
            sht.Range("L2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("L2").Style.Font.Bold = true;
            sht.Range("L2").Style.Font.FontSize = 14;
            sht.Range("L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("L2").Style.Alignment.WrapText = true;
            sht.Range("L2").Merge();

            sht.Row(2).Height = 40;

            sht.Range("A2:L2").Style.Font.Bold = true;
            sht.Range("A2:L2").Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A2:L2").Style.Font.FontSize = 14;
            sht.Range("A2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:L2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A2:L2").Style.Alignment.WrapText = true;

            row = 3;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["OItemName"]);
                sht.Range("B" + row).SetValue("");
                sht.Range("C" + row).SetValue("");
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["OQualityName"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["OShadeColor"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                sht.Range("G" + row).SetValue("");
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["IndentRecQty"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["WeaverIssueQty"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["WeaverReceiveQty"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["BalanceQty"]);
                sht.Range("L" + row).SetValue("");

                sht.Range("A" + row + ":L" + row).Style.Font.Bold = true;
                sht.Range("A" + row + ":L" + row).Style.Font.FontName = "Calibri";
                sht.Range("A" + row + ":L" + row).Style.Font.FontSize = 11;
                sht.Range("A" + row + ":L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row + ":L" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A" + row + ":L" + row).Style.Alignment.WrapText = true;

                row = row + 1;
            }

            using (var a = sht.Range("A2" + ":L" + (row - 1)))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            row = row + 1;



            ////********************
            //sht.Columns(1, 50).AdjustToContents();
            //******************
            sht.Row(1).Height = 28.80;
            //sht.Range("A2:H2").Style.Alignment.WrapText = true;

            string Fileextension = "xlsx";
            string filename = "";

            filename = UtilityModule.validateFilename("ORDERCONSUMPTIONSUMMARYWITHWEAVERISSREC_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);

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
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
        }

    }

    protected void OrderConsumptionRecMaterialPendingDetail()
    {
        String str = "";

        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " and OM.customerid=" + DDCustCode.SelectedValue;
        }
        //if (DDOrderNo.SelectedIndex > 0)
        //{
        //    str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
        //}        
        if (txtlocalOrderNo.Text != "")
        {
            str = str + " and OM.LocalOrder='" + txtlocalOrderNo.Text + "'";
        }
        //if (DDorderstatus.SelectedIndex == 1)
        //{
        //    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        //}
        //else if (DDorderstatus.SelectedIndex == 2)
        //{
        //    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        //}

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GETOrderConsumptionRecMaterialPendingDetail", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@OrderId", DDOrderNo.SelectedValue);
        cmd.Parameters.AddWithValue("@Where", str);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        ////cmd.Parameters.AddWithValue("@status", DDorderstatus.SelectedValue);
        //DataTable dt = new DataTable();
        //dt.Load(cmd.ExecuteReader());
        ////*************
        //DataSet ds = new DataSet();
        //ds.Tables.Add(dt);
        //con.Close();
        //con.Dispose();


        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        con.Close();
        con.Dispose();

        //SqlParameter[] param = new SqlParameter[3];
        //param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        //param[1] = new SqlParameter("@Where", str);
        //param[2] = new SqlParameter("@userid", Session["varuserid"]);
        //***********
        // DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_getPoorderstatus", param);

        if (ds.Tables[0].Rows.Count > 0)
        {

            try
            {
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                //************
                sht.Range("A1:H1").Merge();
                sht.Range("A1").SetValue(ds.Tables[0].Rows[0]["CompanyName"]);
                sht.Range("A1:H1").Style.Font.Bold = true;
                sht.Range("A1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:H1").Style.Font.FontName = "Calibri";
                sht.Range("A1:H1").Style.Font.FontSize = 12;

                sht.Range("A2:H2").Merge();
                sht.Range("A2").SetValue(ds.Tables[0].Rows[0]["CompAddr1"] + " " + ds.Tables[0].Rows[0]["CompAddr2"] + " " + ds.Tables[0].Rows[0]["CompAddr3"]);
                sht.Range("A2:H2").Style.Font.Bold = true;
                sht.Range("A2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:H2").Style.Font.FontName = "Calibri";
                sht.Range("A2:H2").Style.Font.FontSize = 12;

                ////JOB NAME
                //sht.Range("H2:J2").Merge();
                //sht.Range("H2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Range("H2:J2").Style.Font.Bold = true;

                sht.Range("A3:H3").Merge();
                sht.Range("A3").SetValue("");
                sht.Range("A3:H3").Style.Font.Bold = true;
                sht.Range("A3:H3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A3:H3").Style.Font.FontName = "Calibri";
                sht.Range("A3:H3").Style.Font.FontSize = 12;

                sht.Range("A4").SetValue("CUSTOMER CODE");
                sht.Range("B4").SetValue("ORDER NO");
                sht.Range("C4").SetValue("ITEM NAME");
                sht.Range("D4").SetValue("QUALITY");
                sht.Range("E4").SetValue("SHADE COLOR");
                sht.Range("F4").SetValue("CONSUMPTION QTY");
                sht.Range("G4").SetValue("REC QTY");
                sht.Range("H4").SetValue("PENDING QTY");

                sht.Range("A4:J4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A4:J4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A4:J4").Style.Alignment.WrapText = true;
                sht.Range("A4:J4").Style.Font.SetBold();
                sht.Range("A4:J4").Style.Font.FontName = "Calibri";
                sht.Range("A4:J4").Style.Font.FontSize = 12;

                int row = 5;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //if (i == 0)
                    //{
                    //    sht.Range("H2").SetValue(ds.Tables[0].Rows[0]["JobName"]);
                    //}

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["OrderConsumptionQty"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["GateInQty"]);
                    sht.Range("H" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[i]["OrderConsumptionQty"]) - Convert.ToDecimal(ds.Tables[0].Rows[i]["GateInQty"]));


                    row = row + 1;
                }
                using (var a = sht.Range("A1" + ":H" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }
                //********************
                sht.Columns(1, 50).AdjustToContents();
                //******************
                String Path = "";
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("OrderConsumptionRecPendingQtyReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }


            //if (chkexcelexport.Checked == true)
            //{
            //    FinishingStatusexcelexport(ds);
            //}
            //else
            //{
            //    Session["GetDataset"] = ds;
            //    Session["rptFileName"] = "~\\Reports\\RptfinishingStatus.rpt";
            //    Session["dsFileName"] = "~\\ReportSchema\\RptfinishingStatus.xsd";
            //    StringBuilder stb = new StringBuilder();
            //    stb.Append("<script>");
            //    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            //    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            //}
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
        }

    }

    protected void OrderShippedWithInvoiceDetail()
    {
        String str = "";
        if (ChkForDate.Checked == true)
        {
            str = str + " and cast(OM.OrderDate as date)>='" + txtfromdate.Text + "' and cast(Om.OrderDate as date)<='" + txttodate.Text + "'";
        }
        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " and OM.customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.qualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf.designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and vf.sizeid=" + DDSize.SelectedValue;
        }
        //if (txtlocalOrderNo.Text != "")
        //{
        //    str = str + " and OM.LocalOrder='" + txtlocalOrderNo.Text + "'";
        //}
        //if (DDorderstatus.SelectedIndex > 0)
        //{
        //    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        //}
        ////else if (DDorderstatus.SelectedIndex == 2)
        ////{
        ////    str = str + " and OM.Status=" + DDorderstatus.SelectedValue;
        ////}
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GETCUSTOMERORDERSHIPPEDWITHINVOICEDETAIL", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@Where", str);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyNo"]);
        //cmd.Parameters.AddWithValue("@status", DDorderstatus.SelectedValue);
        DataTable dt = new DataTable();
        dt.Load(cmd.ExecuteReader());
        //*************
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        con.Close();
        con.Dispose();

        ////SqlParameter[] param = new SqlParameter[3];
        ////param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
        ////param[1] = new SqlParameter("@Where", str);
        ////param[2] = new SqlParameter("@userid", Session["varuserid"]);
        ////***********
        //// DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETCUSTOMERORDERSHIPPEDWITHINVOICEDETAIL", param);

        ds.Tables[0].Columns.Remove("orderid");
        ds.Tables[0].Columns.Remove("Item_finished_id");
        //ds.Tables[0].Columns.Remove("orderdateexcel");
        //ds.Tables[0].Columns.Remove("Dispatchdateexcel");
        //ds.Tables[0].Columns.Remove("UNIT");
        ////Export to excel
        if (ds.Tables[0].Rows.Count > 0)
        {
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;

            GridView1.DataSource = ds;
            GridView1.DataBind();
            Response.Clear();
            Response.Buffer = true;

            string filename = "INVOICEWISESHIPMENTDETAILS";
            filename = filename + "_Date_" + DateTime.Now + "";
            filename = filename + ".xls";

            ////Response.AddHeader("content-disposition",
            //// "attachment;filename=ORDERSUMMARYALLJOBWISE" + DateTime.Now + ".xls");
            Response.AddHeader("content-disposition",
            "attachment;filename=" + filename);
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
            lblMessage.Text = "Done.....";
            //*************
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altordersumm", "alert('No records found...');", true);
        }

    }

}