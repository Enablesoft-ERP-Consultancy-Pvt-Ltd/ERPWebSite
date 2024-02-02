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

public partial class Masters_ReportForms_frmkitreport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                        From Companyinfo CI(nolock)
                        JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @"  
                        Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName 
                        Select EI.EmpId,EI.EmpName + case When isnull(Ei.empcode,'')='' then '' else ' ['+EI.empcode+']' end as Empname  
                        From EmpInfo  EI 
						Join EmpProcess EP on EP.EmpId = EI.EmpId and EP.ProcessId = 1 
                        Where EI.MastercompanyId = " + Session["varMasterCompanyIDForERP"] + @" order by EI.EmpName
                        select CATEGORY_ID,CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM 
                        Select CustomerId,CustomerCode From CustomerInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CustomerCode
                        SELECT DISTINCT U.UNITSID,U.UNITNAME  FROM UNITS U INNER JOIN UNITS_AUTHENTICATION UA ON  U.UnitsId=UA.UnitsId WHERE UA.USERID=" + Session["varuserid"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
          //  UtilityModule.ConditionalComboFillWithDS(ref DDWeaver, ds, 1, true, "---Plz Select---");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, "---Plz Select---");
            UtilityModule.ConditionalComboFillWithDS(ref DDCustCode, ds, 3, true, "---Plz Select---");
            UtilityModule.ConditionalComboFillWithDS(ref DDUnitname, ds, 4, true, "--ALL--");
            txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            //if (variable.VarLoomNoGenerated == "1")
            //{
            //    TDchkpbarcode.Visible = true;
            //}
            //else
            //{
            //    TDchkpbarcode.Visible = false;
            //}

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            //if (Session["varCompanyNo"].ToString() == "30")
            //{
            //    RDIssRecConsumpSummary.Text = "DateWise RawMaterial Summary";
            //    RDItemQualityWiseWeavingPaymentSummary.Text = "Production Summary";
            //    TDWeaverRawMaterialOnLoom.Visible = true;
            //}
            //else
            //{
            //    TDWeaverRawMaterialOnLoom.Visible = false;
            //}

            //if (Session["varCompanyNo"].ToString() == "22")
            //{
            //    TDWithTagNoTracking.Visible = true;
            //}
            //else
            //{
            //    TDWithTagNoTracking.Visible = false;
            //}
            //if (RDAll.Checked == true)
            //{
            //    Trunitname.Visible = true;
            //}
            //if (Session["varCompanyNo"].ToString() == "16")
            //{
            //    TDChampoPNMAmtDifference.Visible = true;
            //    TDChampoExternalWeaverConsumption.Visible = true;
            //    RDWeaverHissabReport.Visible = false;
            //    TDRDWeavingOrderRecBalWithAmountDetail.Visible = true;
            //    TDFolioWiseConsumptionReport.Visible = true;
            //    TDWeaverRawMaterialIssueReport.Visible = true;
            //    TDDesignWiseFolioMaterialIssueStatus.Visible = true;
            //}
            //else if (Session["varCompanyNo"].ToString() == "28")
            //{
            //    TDChampoPNMAmtDifference.Visible = false;
            //    TDChampoExternalWeaverConsumption.Visible = true;
            //    TDRDWeavingOrderRecBalWithAmountDetail.Visible = true;
            //    TDWeaverRawMaterialIssueReport.Visible = true;
            //}
            //else if (Session["varCompanyNo"].ToString() == "14")
            //{
            //    TDChampoPNMAmtDifference.Visible = false;
            //    TDChampoExternalWeaverConsumption.Visible = true;
            //    TDWeaverRawMaterialIssueReport.Visible = true;
            //}
            //else if (Session["varCompanyNo"].ToString() == "40")
            //{
            //    TDChampoPNMAmtDifference.Visible = false;
            //    TDChampoExternalWeaverConsumption.Visible = false;
            //    TDRDWeavingOrderRecBalWithAmountDetail.Visible = false;
            //    TDWeaverRawMaterialIssueReport.Visible = false;
            //    TDWeavingReceiveWithTDS.Visible = true;
            //}
            //else if (Session["varCompanyNo"].ToString() == "38")
            //{
            //    TDChampoPNMAmtDifference.Visible = false;
            //    TDChampoExternalWeaverConsumption.Visible = false;
            //    TDRDWeavingOrderRecBalWithAmountDetail.Visible = false;
            //    TDWeaverRawMaterialIssueReport.Visible = false;
            //    TDWeavingReceiveWithTDS.Visible = false;
            //    TDChkForWeavingPendingQtyWithArea.Visible = true;
            //    TDWeaverReceivePaymentSummary.Visible = true;
            //}
            //else if (Session["varMasterCompanyIDForERP"].ToString() == "41")
            //{
            //    TDInternalBucket.Visible = false;
            //    TDIssRecConsumpSummary.Visible = false;
            //    Td1.Visible = false;
            //    TDWeaverOrderStatus.Visible = false;
            //}
            //else
            //{
            //    TDChampoPNMAmtDifference.Visible = false;
            //    TDChampoExternalWeaverConsumption.Visible = false;
            //    TDRDWeavingOrderRecBalWithAmountDetail.Visible = false;
            //    TDWeaverRawMaterialIssueReport.Visible = false;
            //    TDWeavingReceiveWithTDS.Visible = false;
            //    TDInternalBucket.Visible = true;
            //    TDIssRecConsumpSummary.Visible = true;
            //    Td1.Visible = true;
            //    TDWeaverOrderStatus.Visible = true;                
            //}
           
           
        }
    }
    protected void DDReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (DDReportType.SelectedValue == "0")
        //{
        //    Label3.Text = "Approval No";
        //}
        //else
        //{
        //    Label3.Text = "Folio No";
        //}
        //if (DDWeaver.SelectedIndex > 0)
        //{
        //    FillFolioNo();
        //}
    }
    protected void DDWeaver_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillFolioNo();
        //DDReturnGatePassNo.Items.Clear();
    }

    protected void FillFolioNo()
    {
        string str = "";
//        if (RDWeaverHissabReport.Checked == true && DDReportType.SelectedValue == "1")
//        {
//            str = @"SELECT distinct PIM.IssueOrderId as orderNo,PIM.Issueorderid as orderid FROM PROCESS_ISSUE_MASTER_1  PIM 
//                        Inner join ProcessHissabPayment PHP ON PIM.IssueOrderId=PHP.FolioNo                       
//                        Where PHP.CompanyId=" + DDCompany.SelectedValue + @" And PHP.Processid=1 And PHP.PartyId=" + DDWeaver.SelectedValue + "  And PHP.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and PHP.ByFolioStatus=1";
//        }
//        else if (RDWeaverHissabReport.Checked == true && DDReportType.SelectedValue == "0")
//        {
//            str = @"select Distinct Id,cast(AppvNo As Nvarchar)+' / '+replace(convert(varchar(11),AppDate,106), ' ','-')
//                    As AppvNo from ProcessHissabApproved PH Inner join ProcessHissabPayment PHP on PH.id=PHP.ApprovalNo
//                    Where PH.CompanyId=" + DDCompany.SelectedValue + @" And PH.Processid=1 And PH.EmpId=" + DDWeaver.SelectedValue + "  And PH.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and PHP.ByFolioStatus=0";
//        }
//        else
//        {
//            str = @"select Distinct PIM.IssueOrderId,isnull(PIM.ChallanNo,Pim.IssueOrderId) as Issueorderid1 From PROCESS_ISSUE_MASTER_1 PIM inner join EmpInfo ei on pim.Empid=ei.EmpId
//                WHERE PIm.Companyid=" + DDCompany.SelectedValue + " and PIM.Empid=" + DDWeaver.SelectedValue;
//            if (DDProductionstatus.SelectedIndex > 0)
//            {
//                str = str + " and PIM.Status='" + DDProductionstatus.SelectedItem.Text + "'";
//            }
//            str = str + " UNION ALL ";
//            str = str + @" select Distinct pim.IssueOrderId,isnull(PIM.ChallanNo,pim.IssueOrderId) as Issueorderid1 From Process_issue_Master_1 pim inner join employee_processorderno emp on pim.issueorderid=emp.issueorderid and emp.ProcessId=1
//            and pim.Empid=0 Where PIm.Companyid=" + DDCompany.SelectedValue + " and EMP.Empid=" + DDWeaver.SelectedValue;

            //if (DDProductionstatus.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Status='" + DDProductionstatus.SelectedItem.Text + "'";
            //}
        //}
       // UtilityModule.ConditionalComboFill(ref DDFolioNo, str, true, "---Plz Select---");
    }
    protected void DDQtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQuality();
    }
    protected void FillQuality()
    {
        string str = @"select Distinct Q.QualityId,Q.QualityName From ITEM_MASTER IM inner Join Quality Q on Q.Item_Id=IM.ITEM_ID inner Join CategorySeparate cs on IM.CATEGORY_ID=cs.Categoryid where 1=1";
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + "  and IM.Category_id=" + DDCategory.SelectedValue;
        }
        if (DDQtype.SelectedIndex > 0)
        {
            str = str + "  and IM.Item_id=" + DDQtype.SelectedValue;
        }
        str = str + "  order by QualityName";
        UtilityModule.ConditionalComboFill(ref DDQuality, str, true, "---Plz Select---");
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {

        FillDesign();
        FillSize();
        //if (Trshadecolor.Visible == true)
        //{
        //    FillShade();
        //}
    }
    protected void FillDesign()
    {
        string str = @"select Distinct Vf.designId,vf.designName From V_finishedItemDetail Vf Where Vf.designId>0";
        if (DDQtype.SelectedIndex > 0)
        {
            str = str + "  and vf.Item_id=" + DDQtype.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
        }

        str = str + "  order by designName";
        UtilityModule.ConditionalComboFill(ref DDDesign, str, true, "---Plz Select---");
    }
    protected void FillShade()
    {
        string str = @"select Distinct ShadecolorId,ShadeColorName From V_finishedItemDetail Vf Where Vf.ShadecolorId>0";
        if (DDQtype.SelectedIndex > 0)
        {
            str = str + "  and vf.Item_id=" + DDQtype.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + "  and vf.QualityId=" + DDQuality.SelectedValue;
        }

        str = str + "  order by ShadeColorName";
       // UtilityModule.ConditionalComboFill(ref DDshade, str, true, "---Plz Select---");
    }
    protected void FillColor()
    {
        string str = @"select Distinct Vf.Colorid,vf.Colorname From V_finishedItemDetail Vf Where Vf.Colorid>0";
        if (DDQtype.SelectedIndex > 0)
        {
            str = str + "  and vf.Item_id=" + DDQtype.SelectedValue;
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
        UtilityModule.ConditionalComboFill(ref DDColor, str, true, "---Plz Select---");
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

        string str = @"select Distinct  Vf.Sizeid,vf." + size + " as Size  From V_finishedItemDetail Vf Where Vf.Sizeid>0";
        if (DDQtype.SelectedIndex > 0)
        {
            str = str + "  and vf.Item_id=" + DDQtype.SelectedValue;
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
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        if (RDkitPrep.Checked == true)
        {
            KIT_PREPARATION();
            return;
        }
        else if (RDKitIssue.Checked == true)
        {
            KIT_ISSUE();
            return;
        }
        else if (RDKitStock.Checked == true)
        {
            KIT_BALANCE();
            return;
        }
        else if (RDkitsummary.Checked == true)
        {
            KIT_SUMMARY();
            return;
        }
        //if (RDAll.Checked == true || RDOrder.Checked == true)
        //{
        //    if (chkpbarcode.Checked == true)
        //    {
        //        WeavingALL_LoomBarcodePending();
        //        return;
        //    }
        //    if (ChkWeavingReport.Checked == true)
        //    {
        //        WeavingFolioDetails();
        //        return;
        //    }
        //    if (ChkForWeavingPendingQtyWithExcelReport.Checked == true)
        //    {
        //        WeaverPendingQtyExcelReport();
        //        return;
        //    }
        //    WeavingALL_ORDER();
        //}
        //else if (RDReceive.Checked == true)
        //{
        //    if (ChkWeavingReport.Checked == true)
        //    {
        //        WeavingFolioBazaarDetails();
        //        return;
        //    }
        //    else if (ChkInternalExternalBazarSummary.Checked == true)
        //    {
        //        InternalExternalBazarSummary();
        //    }
        //    else if (ChkForInternalBazaarDetail.Checked == true)
        //    {
        //        InternalBazaarDetailFolioWiseChampoPanipat2();
        //    }
        //    else
        //    {
        //        WeavingReceive();
        //    }
        //}
        //else if (RDweaverrawbalance.Checked == true)
        //{
        //    if (chkforshadewise.Checked == true)
        //    {
        //        WeavingRawBalanceShadeWise();
        //    }
        //    else
        //    {
        //        WeavingRawBalance();
        //    }
        //}
        //else if (RDinternalFolioDetail.Checked == true)
        //{
        //    if (Session["varMasterCompanyIDForERP"].ToString() == "22")
        //    {
        //        InternalfolioDetailDiamondExport();
        //    }
        //    else
        //    {
        //        InternalfolioDetail();
        //    }
        //}
        //else if (RDWeaverHissabReport.Checked == true)
        //{
        //    if (ChkUnpaidApprovalNo.Checked == true && ChkselectDate.Checked == true)
        //    {
        //        WeaverUnpaidApprovalNo();
        //    }
        //    else
        //    {
        //        WeaverHissabDetail();
        //    }
        //}
        //else if (Rdweaverorderstatus.Checked == true)
        //{
        //    Weaverorderstatus();
        //}
        //else if (RdConsumptionreport.Checked == true)
        //{
        //    consumptionreport();
        //}
        //else if (RDInternalBucketDetail.Checked == true)
        //{
        //    InternalBucketDetail();
        //}
        //else if (RDIssRecConsumpSummary.Checked == true)
        //{
        //    IssRecConsumptionSummary();
        //}
        //else if (RDItemQualityWiseWeavingPaymentSummary.Checked == true)
        //{
        //    ItemQualityWiseWeavingPaymentSummary();
        //}
        //else if (RDCommissionDetail.Checked == true)
        //{
        //    Commissiondetail();
        //}
        //else if (RDBazaarReturnGatePassDetail.Checked == true)
        //{
        //    BazaarReturnGatePassDetail();           
        //}
        //else if (RDWeaverRawMaterialOnLoom.Checked == true)
        //{
        //    if (TRChkBazaarReceiveOnLoom.Visible == true && ChkBazaarReceiveOnLoom.Checked == true)
        //    {
        //        WeaverBazaarReceiveOnLoom();
        //    }
        //    else
        //    {
        //        WeaverRawMaterialOnLoom();
        //    } 
        //}
        //else if (RDWithTagNoTracking.Checked == true)
        //{
        //    WithTagNoTracking();
        //}
        //else if (RDChampoPNMAmtDifference.Checked == true)
        //{
        //    ChampoPNMAmtDifferenceReport();
        //    return;
        //}
        //else if (RDChampoExternalWeaverConsumption.Checked == true)
        //{
        //    if (TDChkForDepartmentRawDetail.Visible == true && ChkForDepartmentRawDetail.Checked == true)
        //    {
        //        DepartmentRawConsumptionDetail();
        //    }
        //    else
        //    {
        //        ChampoExternalWeaverConsumptionDetail();
        //    }
        //    return;
        //}
        //else if (RDWeavingOrderRecBalWithAmountDetail.Checked == true)
        //{
        //    WeavingOrderRecBalWithAmountDetail();
        //    return;
        //}
        //else if (RDFolioWiseConsumptionReport.Checked == true)
        //{
        //    FolioWiseConsumptionReport();
        //    return;
        //}
        //else if (RDWeaverRawMaterialIssueReport.Checked == true)
        //{
        //    WeaverRawMaterialIssueReport();
        //    return;
        //}
        //else if (RDDesignWiseFolioMaterialIssueStatus.Checked == true)
        //{
        //    DesignWiseFolioMaterialIssueStatus();
        //    return;
        //}
        //else if (RDWeavingReceiveWithTDS.Checked== true)
        //{
        //    WeavingReceiveWithTDS();
        //    return;
        //}
        //else if (RDWeaverRawMaterialIssRecWithConsumption.Checked == true)
        //{
        //    WeaverRawMaterialIssRecWithConsumptionReport();
        //    return;
        //}
        //else if (RDWeaverReceivePaymentSummary.Checked == true)
        //{
        //    WeavingReceivePaymentSummary();
        //    return;
        //}
    }
    protected void Commissiondetail()
    {
        lblmsg.Text = "";
        try
        {
            string str = "", FilterBy = "";

            //if (DDProductionstatus.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Status='" + DDProductionstatus.SelectedItem.Text + "'";
            //    FilterBy = FilterBy + ", Production Status -" + DDProductionstatus.SelectedItem.Text;
            //}
            //if (DDFoliotype.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Purchaseflag=" + DDFoliotype.SelectedValue;
            //    FilterBy = FilterBy + ", Folio Type -" + DDFoliotype.SelectedItem.Text;
            //}
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and PRD.Issueorderid=" + DDFolioNo.SelectedValue;
            //    FilterBy = FilterBy + ", Folio No. -" + DDFolioNo.SelectedItem.Text;
            //}
            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and Vf.Item_id=" + DDQtype.SelectedValue;
                FilterBy = FilterBy + ", Item Name -" + DDQtype.SelectedItem.Text;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and Vf.Qualityid=" + DDQuality.SelectedValue;
                FilterBy = FilterBy + ", Quality -" + DDQuality.SelectedItem.Text;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
                FilterBy = FilterBy + ", Design -" + DDDesign.SelectedItem.Text;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " and vf.Colorid=" + DDColor.SelectedValue;
                FilterBy = FilterBy + ", Color -" + DDColor.SelectedItem.Text;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
                FilterBy = FilterBy + ", Size -" + DDSize.SelectedItem.Text;
            }
            //if (DDWeaver.SelectedIndex > 0)
            //{
            //    FilterBy = FilterBy + ", Weaver -" + DDWeaver.SelectedItem.Text;
            //}
            if (ChkselectDate.Checked == true)
            {
                //str = str + " and PIM.Assigndate>='" + txtfromDate.Text + "' and PIM.AssignDate<='" + txttodate.Text + "'";                
                str = str + " and PRM.Receivedate>='" + txtfromDate.Text + "' and PRM.Receivedate<='" + txttodate.Text + "'";
                FilterBy = FilterBy + ", From -" + txtfromDate.Text + " To - " + txttodate.Text;
            }

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETWEAVERCOMMISSIONDETAILS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@empid", "0");
            cmd.Parameters.AddWithValue("@Where", str);

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


                sht.Range("A1").Value = "Commission Details";

                sht.Range("A2").Value = "Filter By :  " + FilterBy;
                sht.Row(2).Height = 30;

                //*******Header
                sht.Range("A4").Value = "Emp Name";
                sht.Range("B4").Value = "Folio No.";
                sht.Range("C4").Value = "Date";
                sht.Range("D4").Value = "Quality";
                sht.Range("E4").Value = "Design";
                sht.Range("F4").Value = "Area";
                sht.Range("G4").Value = "Weaving Rate";
                sht.Range("H4").Value = "Comm.Rate";
                sht.Range("I4").Value = "Comm Amount";
                sht.Range("J4").Value = "Lagat";
                sht.Range("K4").Value = "Average";

                int cellend = 11;
                int Dynamiccell = 0;
                int Dynamiccell_start = 0;
                int Dynamiccell_end = 0;

                if (ds.Tables[1].Rows.Count > 0)
                {
                    Dynamiccell = cellend;
                    Dynamiccell_start = Dynamiccell + 1;

                    DataTable dtdistinct = ds.Tables[1].DefaultView.ToTable(true, "item_name");
                    DataView dv1 = new DataView(dtdistinct);
                    DataTable dtdistinct1 = dv1.ToTable();

                    foreach (DataRow dr in dtdistinct1.Rows)
                    {
                        Dynamiccell = Dynamiccell + 1;
                        sht.Cell(4, Dynamiccell).Value = dr["Item_name"];
                    }
                    Dynamiccell_end = Dynamiccell;
                    sht.Range(sht.Cell(3, Dynamiccell_start), sht.Cell(3, Dynamiccell_end)).Merge();
                    sht.Cell(3, Dynamiccell_start).SetValue("M.Balance");
                    sht.Range(sht.Cell(3, Dynamiccell_start), sht.Cell(3, Dynamiccell_end)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range(sht.Cell(3, Dynamiccell_start), sht.Cell(3, Dynamiccell_end)).Style.Font.Bold = true;
                }
                int Lastcell = 0;
                if (Dynamiccell_end == 0)
                {
                    Lastcell = cellend;
                }
                else
                {
                    Lastcell = Dynamiccell_end;
                }


                sht.Range(sht.Cell(1, 1), sht.Cell(1, Lastcell)).Merge();
                sht.Range(sht.Cell(2, 1), sht.Cell(2, Lastcell)).Merge();

                sht.Range(sht.Cell(1, 1), sht.Cell(1, Lastcell)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range(sht.Cell(2, 1), sht.Cell(2, Lastcell)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range(sht.Cell(2, 1), sht.Cell(2, Lastcell)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range(sht.Cell(2, 1), sht.Cell(2, Lastcell)).Style.Alignment.SetWrapText();

                sht.Range(sht.Cell(1, 1), sht.Cell(2, Lastcell)).Style.Font.FontName = "Arial Unicode MS";
                sht.Range(sht.Cell(1, 1), sht.Cell(2, Lastcell)).Style.Font.FontSize = 10;
                sht.Range(sht.Cell(1, 1), sht.Cell(2, Lastcell)).Style.Font.Bold = true;



                sht.Range(sht.Cell(4, 1), sht.Cell(4, Lastcell)).Style.Font.FontName = "Arial Unicode MS";
                sht.Range(sht.Cell(4, 1), sht.Cell(4, Lastcell)).Style.Font.FontSize = 9;
                sht.Range(sht.Cell(4, 1), sht.Cell(4, Lastcell)).Style.Font.Bold = true;
                //sht.Range("N3:P3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 5;
                //DataView dv = new DataView(ds.Tables[0]);
                //dv.Sort = "Issueorderid";
                //DataSet ds1 = new DataSet();
                //ds1.Tables.Add(dv.ToTable());
                int lastissueorderid = 0;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":O" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":O" + row).Style.Font.FontSize = 9;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Empname"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["issueorderid"]);
                    sht.Range("C" + row).SetValue(Convert.ToDateTime(ds.Tables[0].Rows[i]["Fromdate"]).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Todate"]).ToString("dd-MMM-yyyy"));
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Qualityname"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Designname"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Area"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);

                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Comm"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Commamt"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["lagat"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["Avglagat"]);

                    if (Dynamiccell > 0)
                    {
                        if (lastissueorderid != Convert.ToInt32(ds.Tables[0].Rows[i]["issueorderid"]))
                        {
                            for (int k = Dynamiccell_start; k <= Dynamiccell_end; k++)
                            {
                                var Item_name = sht.Cell(4, k).Value;
                                if (Item_name != "")
                                {
                                    var MBalances = ds.Tables[1].Compute("sum(BALANCES)", "issueorderid=" + ds.Tables[0].Rows[i]["issueorderid"] + " and  Item_name='" + Item_name + "'");
                                    sht.Cell(row, k).SetValue(MBalances == DBNull.Value ? 0 : MBalances);
                                }

                            }
                            lastissueorderid = Convert.ToInt32(ds.Tables[0].Rows[i]["issueorderid"]);
                        }
                    }

                    row = row + 1;
                }

                //*************
                sht.Columns(1, 25).AdjustToContents();
                //********************
                //***********BOrders

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, Lastcell)))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("CommissionDetails_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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

                ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
    protected void WeaverHissabDetail()
    {
        try
        {
            string str = "";
            string Column = ""; string Column2 = "";
            //if (DDProductionstatus.SelectedIndex > 0)
            //{
            //    str = str + " and Status='" + DDProductionstatus.SelectedItem.Text + "'";
            //}
            //if (DDReportType.SelectedValue == "1")
            //{
            //    Column = "PHP.FolioNo";
            //    Column2 = "PHP.ByFolioStatus=1";

            //}
            //else
            //{
                Column = "PHP.ApprovalNo";
                Column2 = "PHP.ByFolioStatus=0";
           // }

            str = str + " and PHP.CompanyId=" + DDCompany.SelectedValue;

            //if (DDWeaver.SelectedIndex > 0)
            //{
            //    str = str + " and PHP.PartyId=" + DDWeaver.SelectedValue;
            //}
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and " + Column + "=" + DDFolioNo.SelectedValue;
            //}
            if (ChkselectDate.Checked == true)
            {
                str = str + " and PHP.DATE>='" + txtfromDate.Text + "' and PHP.DATE<='" + txttodate.Text + "'";
            }
            str = str + " and " + Column2 + "";
            //*****************
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@ReportType", 0);
            param[1] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[2] = new SqlParameter("@FromDate", txtfromDate.Text);
            param[3] = new SqlParameter("@ToDate", txttodate.Text);
            param[4] = new SqlParameter("@where", str);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetWeaverHissabPaymentDetails", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptWeaverHissabPayment.rpt";

                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptWeaverHissabPayment.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "RawBal", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void InternalfolioDetail()
    {
        lblmsg.Text = "";
        try
        {
            string str = "", FilterBy = "";
            //if (DDProductionstatus.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Status='" + DDProductionstatus.SelectedItem.Text + "'";
            //    FilterBy = FilterBy + ", Production Status -" + DDProductionstatus.SelectedItem.Text;
            //    if (DDProductionstatus.SelectedIndex == 1)
            //    {
            //        str = str + " and PID.Pqty>0";
            //    }
            //}
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Issueorderid=" + DDFolioNo.SelectedValue;
            //    FilterBy = FilterBy + ", Folio No. -" + DDFolioNo.SelectedItem.Text;
            //}
            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and Vf.Item_id=" + DDQtype.SelectedValue;
                FilterBy = FilterBy + ", Item Name -" + DDQtype.SelectedItem.Text;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and Vf.Qualityid=" + DDQuality.SelectedValue;
                FilterBy = FilterBy + ", Quality -" + DDQuality.SelectedItem.Text;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
                FilterBy = FilterBy + ", Design -" + DDDesign.SelectedItem.Text;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " and vf.Colorid=" + DDColor.SelectedValue;
                FilterBy = FilterBy + ", Color -" + DDColor.SelectedItem.Text;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
                FilterBy = FilterBy + ", Size -" + DDSize.SelectedItem.Text;
            }
            if (ChkselectDate.Checked == true)
            {
                str = str + " and PIM.Assigndate>='" + txtfromDate.Text + "' and PIM.AssignDate<='" + txttodate.Text + "'";
                FilterBy = FilterBy + ", From -" + txtfromDate.Text + " To - " + txttodate.Text;
            }
            //if (DDFoliotype.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Purchasefolio=" + DDFoliotype.SelectedValue;
            //    FilterBy = FilterBy + ", Folio Type -" + DDFoliotype.SelectedItem.Text;
            //}
            if (DDCustCode.SelectedIndex > 0)
            {
                str = str + " and OM.Customerid=" + DDCustCode.SelectedValue;
                FilterBy = FilterBy + ", Customer code -" + DDCustCode.SelectedItem.Text;
            }
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
            //    FilterBy = FilterBy + ", Order No. -" + DDOrderNo.SelectedItem.Text;
            //}
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@Processid", 1);
            param[2] = new SqlParameter("@where", str);
            param[3] = new SqlParameter("@EMpid", "0");

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETINTERNALFOLIODETAIL", param);

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

                sht.Range("A1:Q1").Merge();
                sht.Range("A1").Value = "INTERNAL FOLIO DETAILS";
                sht.Range("A2:Q2").Merge();
                sht.Range("A2").Value = "Filter By :  " + FilterBy;
                sht.Row(2).Height = 30;
                sht.Range("A1:Q1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:Q2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:Q2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:Q2").Style.Alignment.SetWrapText();
                sht.Range("A1:Q2").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:Q2").Style.Font.FontSize = 10;
                sht.Range("A1:Q2").Style.Font.Bold = true;
                //*******Header
                sht.Range("A3").Value = "Customer code";
                sht.Range("B3").Value = "Order No.";
                sht.Range("C3").Value = "Folio No.";
                sht.Range("D3").Value = "Assign Date";
                sht.Range("E3").Value = "Reqd Date";
                sht.Range("F3").Value = "Loom No.";
                sht.Range("G3").Value = "Emp. Code";

                sht.Range("H3").Value = "Emp. Name";
                sht.Range("I3").Value = "Emp. Type";
                sht.Range("J3").Value = "Quality Name";
                sht.Range("K3").Value = "Design Name";
                sht.Range("L3").Value = "Color Name";
                sht.Range("M3").Value = "Size";
                sht.Range("N3").Value = "WOQty.";
                sht.Range("O3").Value = "Rec. Qty";
                sht.Range("P3").Value = "Balance Qty";
                sht.Range("Q3").Value = "Pend.Stock No.";
                sht.Range("R3").Value = "Rate";

                sht.Range("A3:R3").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A3:R3").Style.Font.FontSize = 9;
                sht.Range("A3:R3").Style.Font.Bold = true;
                sht.Range("N3:P3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("R3:R3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 4;
                int rowfrom = 4;
                DataView dv = new DataView(ds.Tables[0]);
                dv.Sort = "Issueorderid";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());

                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":O" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":O" + row).Style.Font.FontSize = 9;

                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["customercode"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["customerorderno"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Issueorderid"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Assigndate"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Reqbydate"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Loomno"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["empcode"]);

                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["empname"]);
                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["emptype"]);
                    sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                    sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Designname"]);
                    sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["colorname"]);
                    sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["size"]);
                    sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);
                    sht.Range("O" + row).SetValue(Convert.ToDecimal(ds1.Tables[0].Rows[i]["Qty"]) - Convert.ToDecimal(ds1.Tables[0].Rows[i]["PQty"]));
                    sht.Range("P" + row).FormulaA1 = "=N" + row + '-' + ("$O$" + row + "");
                    sht.Range("Q" + row).SetValue(ds1.Tables[0].Rows[i]["Pendstockno"]);
                    sht.Range("R" + row).SetValue(ds1.Tables[0].Rows[i]["RATE"]);
                    row = row + 1;
                }
                //GRAND TOAL
                //=SUM(L4:L929)
                sht.Range("N" + row).FormulaA1 = "=SUM(N" + rowfrom + ":N" + (row - 1) + ")";
                sht.Range("O" + row).FormulaA1 = "=SUM(O" + rowfrom + ":O" + (row - 1) + ")";
                sht.Range("P" + row).FormulaA1 = "=SUM(P" + rowfrom + ":P" + (row - 1) + ")";
                sht.Range("N" + row + ":P" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("N" + row + ":P" + row).Style.Font.FontSize = 9;
                sht.Range("N" + row + ":P" + row).Style.Font.Bold = true;

                //*************
                sht.Columns(1, 20).AdjustToContents();
                //********************
                //***********BOrders
                using (var a = sht.Range("A1" + ":R" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("InternalFolioDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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

                ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
    protected void WeavingALL_ORDER()
    {
        try
        {
            string str = "select *,'" + txtfromDate.Text + "' as FromDate,'" + txttodate.Text + "' as Todate," + (ChkselectDate.Checked == true ? "1" : "0") + " as Dateflag,FolioType From V_WeavingOrderStatus Where CompanyId=" + DDCompany.SelectedValue;
                                  
            
            if (DDUnitname.SelectedIndex > 0)
            {
                str = str + " and Units=" + DDUnitname.SelectedValue;
            }
            //if (DDProductionstatus.SelectedIndex > 0)
            //{
            //    str = str + " and Status='" + DDProductionstatus.SelectedItem.Text + "'";
            //    if (DDProductionstatus.SelectedIndex == 1)
            //    {
            //        str = str + " and Pqty>0";
            //    }
            //}
            //if (DDWeaver.SelectedIndex > 0)
            //{
            //    str = str + " and Empid=" + DDWeaver.SelectedValue;
            //}
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and Issueorderid=" + DDFolioNo.SelectedValue;
            //}
            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and Item_id=" + DDQtype.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and Qualityid=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and DesignId=" + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " and Colorid=" + DDColor.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and Sizeid=" + DDSize.SelectedValue;
            }
            if (ChkselectDate.Checked == true)
            {
                str = str + " and Assigndate>='" + txtfromDate.Text + "' and AssignDate<='" + txttodate.Text + "'";
            }
            //if (DDFoliotype.SelectedIndex > 0)
            //{
            //    str = str + " and Purchasefolio=" + DDFoliotype.SelectedValue;
            //}
            if (DDCustCode.SelectedIndex > 0)
            {
                str = str + " and Customerid=" + DDCustCode.SelectedValue;
            }
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    str = str + " and orderid=" + DDOrderNo.SelectedValue;
            //}
            //if (DDproductiontype.SelectedIndex > 0)
            //{
            //    str = str + " and PRODUCTIONTYPE=" + DDproductiontype.SelectedValue;
            //}          

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(str, con);
            //cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();


            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //if (RDAll.Checked == true)
                //{
                //    if (Session["VarCompanyNo"].ToString() == "38" && ChkForWeavingPendingQtyWithArea.Checked == true)
                //    {
                //        Session["rptFileName"] = "~\\Reports\\RptWeaverSummaryPendingQtyWithPendingArea.rpt";
                //    }
                //    else
                //    {
                //        Session["rptFileName"] = "~\\Reports\\RptWeaversummary.rpt";
                //    }

                    
                //}
                //else if (RDOrder.Checked == true)
                //{
                //    //

                //    Session["rptFileName"] = "~\\Reports\\RptWeaverOrderDetail.rpt";
                //}
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptweavingOrderstatus.xsd";

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
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void WeavingALL_LoomBarcodePending()
    {
        try
        {
            string str = "";
            //if (DDProductionstatus.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Status='" + DDProductionstatus.SelectedItem.Text + "'";

            //}

            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Issueorderid=" + DDFolioNo.SelectedValue;
            //}
            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and VF.Item_id=" + DDQtype.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and VF.Qualityid=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and VF.DesignId=" + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " and VF.Colorid=" + DDColor.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and VF.Sizeid=" + DDSize.SelectedValue;
            }
            if (ChkselectDate.Checked == true)
            {
                str = str + " and PIM.Assigndate>='" + txtfromDate.Text + "' and PIM.AssignDate<='" + txttodate.Text + "'";
            }
            //if (DDFoliotype.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Purchasefolio=" + DDFoliotype.SelectedValue;
            //}
            if (DDCustCode.SelectedIndex > 0)
            {
                str = str + " and OM.customerid=" + DDCustCode.SelectedValue;
            }
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
            //}
            //if (DDproductiontype.SelectedIndex > 0)
            //{
            //    if (DDproductiontype.SelectedValue == "0")
            //    {
            //        str = str + " and PIM.EMPID>0";
            //    }
            //    if (DDproductiontype.SelectedValue == "1")
            //    {
            //        str = str + " and PIM.EMPID=0";
            //    }

            //}
            if (DDUnitname.SelectedIndex > 0)
            {
                str = str + " and PIM.Units=" + DDUnitname.SelectedValue;
            }
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@EMPID",  "0");
            param[2] = new SqlParameter("@WHERE", str);
            param[3] = new SqlParameter("@fromdate", txtfromDate.Text);
            param[4] = new SqlParameter("@Todate", txttodate.Text);
            param[5] = new SqlParameter("@Dateflag", ChkselectDate.Checked == true ? "1" : "0");

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETWEAVERPENDINGBARCODE", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\Rptweaverpendingbarcode.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\Rptweaverpendingbarcode.xsd";

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
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void InternalExternalBazarSummary()
    {
        try
        {
            string FilterBy = " From " + txtfromDate.Text + " To " + txttodate.Text;
            //if (DDproductiontype.SelectedIndex > 0)
            //{
            //    FilterBy = FilterBy + ", Production Type - " + DDproductiontype.SelectedItem.Text;
            //}
            //if (DDCustCode.SelectedIndex > 0)
            //{
            //    FilterBy = FilterBy + ", Cust. Code - " + DDCustCode.SelectedItem.Text;
            //}
            //if (DDOrderNo.Items.Count > 0 && DDOrderNo.SelectedIndex > 0)
            //{
            //    FilterBy = FilterBy + ", PO No. - " + DDOrderNo.SelectedItem.Text;
            //}

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@CompanyID", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@ProductionType",0);
            param[2] = new SqlParameter("@CustomerID", DDCustCode.SelectedValue);
            //if (DDOrderNo.Items.Count > 0)
            //{
            //    param[3] = new SqlParameter("@OrderID", DDOrderNo.SelectedValue);
            //}
            //else
            //{
                param[3] = new SqlParameter("@OrderID", 0);
           // }
            param[4] = new SqlParameter("@FromDate", txtfromDate.Text);
            param[5] = new SqlParameter("@ToDate", txttodate.Text);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetWeavingReceiveTypeOrderAndDateWise", param);

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

                sht.Range("A1:H1").Merge();
                sht.Range("A1").Value = "TOTAL BAZAR SUMMARY";
                sht.Range("A2:H2").Merge();
                sht.Range("A2").Value = "Filter By :  " + FilterBy;
                sht.Row(2).Height = 30;
                sht.Range("A1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:H2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:H2").Style.Alignment.SetWrapText();
                sht.Range("A1:H2").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:H2").Style.Font.FontSize = 10;
                sht.Range("A1:H2").Style.Font.Bold = true;
                //*******Header
                sht.Range("A3").Value = "BUYER";
                sht.Range("B3").Value = "PO";
                sht.Range("C3").Value = "ITEM";
                sht.Range("D3").Value = "QUALITY";
                sht.Range("E3").Value = "DESIGN";
                sht.Range("F3").Value = "COLOR";
                sht.Range("G3").Value = "SIZE";
                sht.Range("H3").Value = "QTY";

                sht.Range("A3:H3").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A3:H3").Style.Font.FontSize = 9;
                sht.Range("A3:H3").Style.Font.Bold = true;
                sht.Range("S3:H3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A3:H3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A3:H3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A3:H3").Style.Alignment.SetWrapText();

                row = 4;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":H" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":H" + row).Style.Font.FontSize = 9;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ItemName"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["SizeFt"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                    row = row + 1;

                }
                //*************
                sht.Columns(1, 26).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("TotalBazarSummary_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            lblmsg.Text = ex.Message;
        }
    }
    protected void KIT_PREPARATION()
    {
        try
        {

            //string str = "select *,'" + txtfromDate.Text + "' as FromDate,'" + txttodate.Text + "' as Todate," + (ChkselectDate.Checked == true ? "1" : "0") + " as Dateflag From V_WeavercarpetreceiveDetail Where CompanyId=" + DDCompany.SelectedValue;
            string str = "select  '" + txtfromDate.Text + "' as FromDate,'" + txttodate.Text + "' as Todate," + (ChkselectDate.Checked == true ? "1" : "0") + " as Dateflag,pm.PRMid ,Replace(CONVERT(nvarchar(11),PM.date,106),' ','-') as IssueDate,pm.kitno, VF.DesignName,vf.ColorName, VF.SizeMtr,PM.qty,(select sum(issuequantity) from ProcessRawTran_KM where PRMid=pm.PRMid)as wt ,nd.UserName from ProcessRawMaster_KM PM JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = PM.Finishedid left join NewUserDetail nd on pm.UserId= nd.UserId Where pm.CompanyId=" + DDCompany.SelectedValue;
            if (DDUnitname.SelectedIndex > 0)
            {
                str = str + "  and units=" + DDUnitname.SelectedValue;
            }
           
            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and Item_id=" + DDQtype.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and Qualityid=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and DesignId=" + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " and Colorid=" + DDColor.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and Sizeid=" + DDSize.SelectedValue;
            }
            if (ChkselectDate.Checked == true)
            {
                str = str + " and Date>='" + txtfromDate.Text + "' and Date<='" + txttodate.Text + "'";
            }
         
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
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
                    sht.Range("A1:H3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A1:H3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A1:H3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("A1:H1").Merge();
                    sht.Range("A1").Value = "DIAMOND EXPORTS";
                    sht.Range("A2:H2").Merge();
                    sht.Range("A2").Value = "WEAVING KIT  PREPARED FROM";
                    sht.Range("A2:H2").Style.Alignment.SetWrapText();
                    sht.Range("A1:H2").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A1:H2").Style.Font.FontSize = 10;
                    sht.Range("A1:H2").Style.Font.Bold = true;
                    sht.Range("A3").Value = "WEAVING KIT  PREPARED FROM";
                   
                    sht.Range("A3:H3").Style.Alignment.SetWrapText();
                    sht.Range("A3:H3").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A3:H3").Style.Font.FontSize = 10;
                    sht.Range("A3:H3").Style.Font.Bold = true;
                    sht.Range("A3").Value = ds.Tables[0].Rows[0]["FromDate"].ToString()+" to "+ds.Tables[0].Rows[0]["ToDate"].ToString();
                    sht.Range("A3:H3").Merge();
                    sht.Range("A3:H3").Style.Alignment.SetWrapText();
                    sht.Range("A3:H3").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A3:H3").Style.Font.FontSize = 10;
                    sht.Range("A3:H3").Style.Font.Bold = true;
                    //*******Header
                    sht.Range("A4").Value = "DATE";
                    sht.Range("B4").Value = "KIT NO.";
                    sht.Range("C4").Value = "Design Name";
                    sht.Range("D4").Value = "Color Name";
                    sht.Range("E4").Value = "Size";
                    sht.Range("F4").Value = "PC QTY";
                    sht.Range("G4").Value = "KIT WT";
                    sht.Range("H4").Value = "USER NAME";
                    sht.Range("A4:H4").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A4:H4").Style.Font.FontSize = 9;
                    sht.Range("A4:H4").Style.Font.Bold = true;
                    sht.Range("S4:H4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("A3:H4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A4:H4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("A4:H4").Style.Alignment.SetWrapText();
                    using (var a = sht.Range("A1:H4"))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                 

                    row = 5;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sht.Range("A" + row + ":X" + row).Style.Font.FontName = "Arial Unicode MS";
                        sht.Range("A" + row + ":X" + row).Style.Font.FontSize = 9;

                        sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["IssueDate"]);
                        sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["kitno"]);
                        sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                        sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                        sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["SizeMtr"]);
                        sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["qty"]);
                        sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["wt"]);
                        sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["username"]);
                       
                        using (var a = sht.Range("A"+row+":H"+row))
                        {
                            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        }
                        row = row + 1;

                    }
                    //*************
                    sht.Columns(1, 26).AdjustToContents();

                    sht.Columns("K").Width = 13.43;


                    string Fileextension = "xlsx";
                    string filename = UtilityModule.validateFilename("kitprep_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void KIT_ISSUE()
    {
        try
        {

            //string str = "select *,'" + txtfromDate.Text + "' as FromDate,'" + txttodate.Text + "' as Todate," + (ChkselectDate.Checked == true ? "1" : "0") + " as Dateflag From V_WeavercarpetreceiveDetail Where CompanyId=" + DDCompany.SelectedValue;
            string str = "select  '" + txtfromDate.Text + "' as FromDate,'" + txttodate.Text + "' as Todate," + (ChkselectDate.Checked == true ? "1" : "0") + " as Dateflag,                        Replace(CONVERT(nvarchar(11),pk.date,106),' ','-') as issuedate,pk.kitno,pk.qty as kitpcs,pm.Prorderid,pim.tanalotno,pim.LoomId, EI.EMPCODE, EI.EMPNAME,VF.DESIGNNAME,VF.COLORNAME,VF.SizeMtr AS SIZE ,SUM(PID.QTY) AS QTY from ProcessRawMaster_km pk join ProcessRawMaster pm on pk.PRMid=pm.kitid join PROCESS_ISSUE_MASTER_1 pim on pm.Prorderid=pim.IssueOrderId JOIN PROCESS_ISSUE_DETAIL_1 PID ON PIM.ISSUEORDERID=PID.ISSUEORDERID INNER JOIN V_FINISHEDITEMDETAIL VF ON pK.finishedid =VF.ITEM_FINISHED_ID  CROSS APPLY (SELECT * FROM DBO.F_GETEMPLOYEEANDEMPTYPE(1,PIM.ISSUEORDERID)) AS EI Where pm.CompanyId=" + DDCompany.SelectedValue + " ";
            if (DDUnitname.SelectedIndex > 0)
            {
                str = str + "  and units=" + DDUnitname.SelectedValue;
            }
           
            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and Item_id=" + DDQtype.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and Qualityid=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and DesignId=" + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " and Colorid=" + DDColor.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and Sizeid=" + DDSize.SelectedValue;
            }
            if (ChkselectDate.Checked == true)
            {
                str = str + " and pk.Date>='" + txtfromDate.Text + "' and pk.Date<='" + txttodate.Text + "'";
            }
            str = str + " GROUP BY pk.date,pk.kitno,pm.Prorderid,pim.tanalotno,EI.EMPCODE,EI.EMPNAME,VF.DESIGNNAME,VF.COLORNAME,VF.SizeMtr,pk.qty,pim.LoomId ";
          
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
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
                    sht.Range("A1:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A1:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A1:L3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    
                    sht.Range("A1").Value = "DIAMOND EXPORTS";
                    sht.Range("A1:L1").Merge();
                    
                    sht.Range("A2").Value = "WEAVING KIT  ISSUE FROM";
                    sht.Range("A2:L2").Merge();
                    sht.Range("A2:L2").Style.Alignment.SetWrapText();
                    sht.Range("A1:L2").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A1:L2").Style.Font.FontSize = 10;
                    sht.Range("A1:L2").Style.Font.Bold = true;
                    sht.Range("A3").Value = "WEAVING KIT  ISSUE FROM";

                    sht.Range("A3:L3").Style.Alignment.SetWrapText();
                    sht.Range("A3:L3").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A3:L3").Style.Font.FontSize = 10;
                    sht.Range("A3:L3").Style.Font.Bold = true;
                    sht.Range("A3").Value = ds.Tables[0].Rows[0]["FromDate"].ToString() + " to " + ds.Tables[0].Rows[0]["ToDate"].ToString();
                    sht.Range("A3:L3").Merge();
                    sht.Range("A3:L3").Style.Alignment.SetWrapText();
                    sht.Range("A3:L3").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A3:L3").Style.Font.FontSize = 10;
                    sht.Range("A3:L3").Style.Font.Bold = true;
                    //*******Header
                    sht.Range("A4").Value = "DATE";
                    sht.Range("B4").Value = "KIT NO.";
                    sht.Range("C4").Value = "KIT PCS";
                    sht.Range("D4").Value = "Issue Folio No.";
                    sht.Range("E4").Value = "Emp Code";
                    sht.Range("F4").Value = "Emp Name";
                    sht.Range("G4").Value = "Design Name";
                    sht.Range("H4").Value = "Color Name";
                    sht.Range("I4").Value = "Size";
                    sht.Range("J4").Value = "WOQty";
                    sht.Range("K4").Value = "Cotton LotNo.";
                    sht.Range("L4").Value = "Loom No.";
                    sht.Range("A4:L4").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A4:L4").Style.Font.FontSize = 9;
                    sht.Range("A4:L4").Style.Font.Bold = true;
                    sht.Range("S4:L4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("A3:L4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("L4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A4:L4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("A4:L4").Style.Alignment.SetWrapText();
                    using (var a = sht.Range("A1:L4"))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                  

                    row = 5;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sht.Range("A" + row + ":X" + row).Style.Font.FontName = "Arial Unicode MS";
                        sht.Range("A" + row + ":X" + row).Style.Font.FontSize = 9;

                        sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["IssueDate"]);
                        sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["kitno"]);
                        sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["kitpcs"]);
                        sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Prorderid"]);
                        sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["EMPCODE"]);
                        sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                        sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["DESIGNNAME"]);
                        sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["COLORNAME"]);
                        sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["SIZE"]);
                        sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["QTY"]);
                        sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["tanalotno"]);
                        sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["LoomId"]);
                        sht.Range("L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        using (var a = sht.Range("A" + row + ":L" + row))
                        {
                            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        }
                        row = row + 1;

                    }
                    //*************
                    sht.Columns(1, 26).AdjustToContents();

                    sht.Columns("L").Width = 30.43;


                    string Fileextension = "xlsx";
                    string filename = UtilityModule.validateFilename("kitfolio_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void KIT_BALANCE()
    {
        try
        {

            //string str = "select *,'" + txtfromDate.Text + "' as FromDate,'" + txttodate.Text + "' as Todate," + (ChkselectDate.Checked == true ? "1" : "0") + " as Dateflag From V_WeavercarpetreceiveDetail Where CompanyId=" + DDCompany.SelectedValue;
            string str = "select   Replace(CONVERT(nvarchar(11),pk.date,106),' ','-') as issuedate,PK.kitno,VF.DESIGNNAME,VF.COLORNAME,VF.SizeMtr AS SIZE,pk.qty as kitpcs,US.UserName from ProcessRawMaster_km pk  left JOIN ProcessRawMaster pm on pk.PRMid=pm.kitid INNER JOIN V_FINISHEDITEMDETAIL VF ON pK.finishedid =VF.ITEM_FINISHED_ID LEFT JOIN NewUserDetail US ON US.UserId=PK.UserId  WHERE PM.kitno IS   NULL and pk.CompanyId=" + DDCompany.SelectedValue + " ";
            //if (DDUnitname.SelectedIndex > 0)
            //{
            //    str = str + "  and units=" + DDUnitname.SelectedValue;
            //}

            //if (DDQtype.SelectedIndex > 0)
            //{
            //    str = str + " and Item_id=" + DDQtype.SelectedValue;
            //}
            //if (DDQuality.SelectedIndex > 0)
            //{
            //    str = str + " and Qualityid=" + DDQuality.SelectedValue;
            //}
            //if (DDDesign.SelectedIndex > 0)
            //{
            //    str = str + " and DesignId=" + DDDesign.SelectedValue;
            //}
            //if (DDColor.SelectedIndex > 0)
            //{
            //    str = str + " and Colorid=" + DDColor.SelectedValue;
            //}
            //if (DDSize.SelectedIndex > 0)
            //{
            //    str = str + " and Sizeid=" + DDSize.SelectedValue;
            //}
            //if (ChkselectDate.Checked == true)
            //{
            //    str = str + " and pk.Date>='" + txtfromDate.Text + "' and pk.Date<='" + txttodate.Text + "'";
            //}
          //  str = str + " GROUP BY pk.date,pk.kitno,pm.Prorderid,pim.tanalotno,EI.EMPCODE, EI.EMPNAME,VF.DESIGNNAME,VF.COLORNAME,VF.SizeMtr,pk.qty ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
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
                    sht.Range("A2:G3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A2:G3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A2:G3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                    sht.Range("A2").Value = "DIAMOND EXPORTS";
                    sht.Range("A2:G2").Merge();
                    sht.Range("A2:G3").Style.Alignment.SetWrapText();
                    sht.Range("A2:G3").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A2:G3").Style.Font.FontSize = 10;
                    sht.Range("A2:G3").Style.Font.Bold = true;
                    //sht.Range("A2").Value = "WEAVING KIT  ISSUE FROM";
                    //sht.Range("A2:K2").Merge();
                    //sht.Range("A2:K2").Style.Alignment.SetWrapText();
                    //sht.Range("A1:K2").Style.Font.FontName = "Arial Unicode MS";
                    //sht.Range("A1:K2").Style.Font.FontSize = 10;
                    //sht.Range("A1:K2").Style.Font.Bold = true;
                    //sht.Range("A3").Value = "WEAVING KIT  BALANCE AS ON DATE";

                    //sht.Range("A3:K3").Style.Alignment.SetWrapText();
                    //sht.Range("A3:K3").Style.Font.FontName = "Arial Unicode MS";
                    //sht.Range("A3:K3").Style.Font.FontSize = 10;
                    //sht.Range("A3:K3").Style.Font.Bold = true;
                    sht.Range("A3").Value = "WEAVING KIT  BALANCE AS ON DATE"+DateTime.Today.ToShortDateString();
                    sht.Range("A3:G3").Merge();
                    sht.Range("A3:G3").Style.Alignment.SetWrapText();
                    sht.Range("A3:G3").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A3:G3").Style.Font.FontSize = 10;
                    sht.Range("A3:G3").Style.Font.Bold = true;
                    //*******Header
                    sht.Range("A4").Value = "DATE";
                    sht.Range("B4").Value = "KIT NO.";
                    sht.Range("C4").Value = "Design Name";
                    sht.Range("D4").Value = "Color Name";
                    sht.Range("E4").Value = "Size";
                    sht.Range("F4").Value = "PC Qty";
                    sht.Range("G4").Value = "User Name";
                  
                    sht.Range("A4:G4").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A4:G4").Style.Font.FontSize = 9;
                    sht.Range("A4:G4").Style.Font.Bold = true;
                    sht.Range("S4:G4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("A3:G4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A4:G4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("A4:G4").Style.Alignment.SetWrapText();
                    using (var a = sht.Range("A2:G4"))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }



                    row = 5;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sht.Range("A" + row + ":X" + row).Style.Font.FontName = "Arial Unicode MS";
                        sht.Range("A" + row + ":X" + row).Style.Font.FontSize = 9;

                        sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["IssueDate"]);
                        sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["kitno"]);
                        sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["DESIGNNAME"]);
                        sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["COLORNAME"]);
                        sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["SIZE"]);
                        sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["kitpcs"]);
                        sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["USERNAME"]);
                        //sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["COLORNAME"]);
                        //sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["SIZE"]);
                        //sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["QTY"]);
                        //sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["tanalotno"]);

                        using (var a = sht.Range("A" + row + ":G" + row))
                        {
                            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        }
                        row = row + 1;

                    }
                    //*************
                    sht.Columns(1, 26).AdjustToContents();

                   // sht.Columns("K").Width = 30.43;


                    string Fileextension = "xlsx";
                    string filename = UtilityModule.validateFilename("kitbalance_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void KIT_SUMMARY()
    {
        try
        {

            //string str = "select *,'" + txtfromDate.Text + "' as FromDate,'" + txttodate.Text + "' as Todate," + (ChkselectDate.Checked == true ? "1" : "0") + " as Dateflag From V_WeavercarpetreceiveDetail Where CompanyId=" + DDCompany.SelectedValue;
            string str = "select   VF.DESIGNNAME,VF.COLORNAME,VF.SizeMtr AS SIZE,COUNT(1) as pcs from ProcessRawMaster_km pk  left JOIN ProcessRawMaster pm on pk.PRMid=pm.kitid INNER JOIN V_FINISHEDITEMDETAIL VF ON pK.finishedid =VF.ITEM_FINISHED_ID LEFT JOIN NewUserDetail US ON US.UserId=PK.UserId  WHERE PM.kitno IS   NULL and pk.CompanyId=" + DDCompany.SelectedValue + " GROUP BY DESIGNNAME,COLORNAME,SizeMtr ";
            //if (DDUnitname.SelectedIndex > 0)
            //{
            //    str = str + "  and units=" + DDUnitname.SelectedValue;
            //}

            //if (DDQtype.SelectedIndex > 0)
            //{
            //    str = str + " and Item_id=" + DDQtype.SelectedValue;
            //}
            //if (DDQuality.SelectedIndex > 0)
            //{
            //    str = str + " and Qualityid=" + DDQuality.SelectedValue;
            //}
            //if (DDDesign.SelectedIndex > 0)
            //{
            //    str = str + " and DesignId=" + DDDesign.SelectedValue;
            //}
            //if (DDColor.SelectedIndex > 0)
            //{
            //    str = str + " and Colorid=" + DDColor.SelectedValue;
            //}
            //if (DDSize.SelectedIndex > 0)
            //{
            //    str = str + " and Sizeid=" + DDSize.SelectedValue;
            //}
            //if (ChkselectDate.Checked == true)
            //{
            //    str = str + " and pk.Date>='" + txtfromDate.Text + "' and pk.Date<='" + txttodate.Text + "'";
            //}
            //  str = str + " GROUP BY pk.date,pk.kitno,pm.Prorderid,pim.tanalotno,EI.EMPCODE, EI.EMPNAME,VF.DESIGNNAME,VF.COLORNAME,VF.SizeMtr,pk.qty ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
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
                    sht.Range("A2:D4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A2:D4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A2:D4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

                    sht.Range("A2").Value = "AS ON DATE"+DateTime.Today.ToShortDateString();
                    sht.Range("A2:D2").Merge();
                    sht.Range("A2:D2").Style.Alignment.SetWrapText();
                    sht.Range("A2:D2").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A2:D2").Style.Font.FontSize = 10;
                    sht.Range("A2:D2").Style.Font.Bold = true;

                    //sht.Range("A2").Value = "WEAVING KIT  ISSUE FROM";
                    //sht.Range("A2:K2").Merge();
                    //sht.Range("A2:K2").Style.Alignment.SetWrapText();
                    //sht.Range("A1:K2").Style.Font.FontName = "Arial Unicode MS";
                    //sht.Range("A1:K2").Style.Font.FontSize = 10;
                    //sht.Range("A1:K2").Style.Font.Bold = true;
                    //sht.Range("A3").Value = "WEAVING KIT  BALANCE AS ON DATE";

                    //sht.Range("A3:K3").Style.Alignment.SetWrapText();
                    //sht.Range("A3:K3").Style.Font.FontName = "Arial Unicode MS";
                    //sht.Range("A3:K3").Style.Font.FontSize = 10;
                    //sht.Range("A3:K3").Style.Font.Bold = true;
                    sht.Range("A3").Value = "DIAMOND EXPORTS";
                    sht.Range("A3:D3").Merge();
                    sht.Range("A3:D3").Style.Alignment.SetWrapText();
                    sht.Range("A3:D3").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A3:D3").Style.Font.FontSize = 10;
                    sht.Range("A3:D3").Style.Font.Bold = true;

                    sht.Range("A4").Value = "WEAVING KIT STOCK";
                    sht.Range("A4:D4").Merge();
                    sht.Range("A4:D4").Style.Alignment.SetWrapText();
                    sht.Range("A4:D4").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A4:D4").Style.Font.FontSize = 10;
                    sht.Range("A4:D4").Style.Font.Bold = true;
                    //*******Header
                  
                    sht.Range("A5").Value = "Design Name";
                    sht.Range("B5").Value = "Color Name";
                    sht.Range("C5").Value = "Size";
                    sht.Range("D5").Value = "Count of KIT NO.";
                   

                    sht.Range("A5:D5").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A5:D5").Style.Font.FontSize = 9;
                    sht.Range("A5:D5").Style.Font.Bold = true;
                    sht.Range("S5:D5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("A5:D5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A5:D5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("A5:D5").Style.Alignment.SetWrapText();
                    using (var a = sht.Range("A2:D5"))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }



                    row = 6;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sht.Range("A" + row + ":D" + row).Style.Font.FontName = "Arial Unicode MS";
                        sht.Range("A" + row + ":D" + row).Style.Font.FontSize = 9;

                        sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["DESIGNNAME"]);
                        sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["COLORNAME"]);
                        sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["SIZE"]);
                        sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["pcs"]);
                        //sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["SIZE"]);
                        //sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["kitpcs"]);
                        //sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["USERNAME"]);
                        //sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["COLORNAME"]);
                        //sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["SIZE"]);
                        //sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["QTY"]);
                        //sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["tanalotno"]);

                        using (var a = sht.Range("A" + row + ":D" + row))
                        {
                            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        }
                        row = row + 1;

                    }
                    //*************
                    sht.Columns(1, 26).AdjustToContents();

                    // sht.Columns("K").Width = 30.43;


                    string Fileextension = "xlsx";
                    string filename = UtilityModule.validateFilename("kitbalance_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDQtype, "select IM.ITEM_ID,IM.ITEM_NAME From Item_Master IM Where IM.category_id=" + DDCategory.SelectedValue + " order by ITEM_NAME", true, "--Plz Select--");
        Fillcombo();
        //if (RDIssRecConsumpSummary.Checked == false)
        //{
        //    Fillcombo();
        //}

        /////if(Session["varCompanyNo"].ToString()=="4" && (RDAll.Checked==true || RDOrder.Checked==true || RDReceive.Checked==true))
        /////{

        //if((RDAll.Checked==true || RDOrder.Checked==true || RDReceive.Checked==true))
        //{
            FillQuality();
            FillDesign();
       // }

    }
    protected void Fillcombo()
    {
        Trquality.Visible = false;
        Trdesign.Visible = false;
        Trcolor.Visible = false;
        Trsize.Visible = false;
       // Trshadecolor.Visible = false;
        string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                  " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                  " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
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
                    //case "6":
                    //    Trshadecolor.Visible = true;
                    //    break;
                }
            }
        }

        //if (RDIssRecConsumpSummary.Checked == true)
        //{
        //    Trquality.Visible = false;
        //    Trdesign.Visible = false;
        //    Trcolor.Visible = false;
        //    Trsize.Visible = false;
        //    Trshadecolor.Visible = false;
        //}
        //if (RDItemQualityWiseWeavingPaymentSummary.Checked == true)
        //{
        //    Trdesign.Visible = false;
        //    Trcolor.Visible = false;
        //    Trsize.Visible = false;
        //    Trshadecolor.Visible = false;
        //}

    }

    protected void DDCustCode_SelectedIndexChanged(object sender, EventArgs e)
    {
//        string Str = "LocalOrder+ ' / ' +CustomerOrderNo";
//        if (Session["varCompanyNo"].ToString() == "16")
//        {
//            Str = "CustomerOrderNo";
//        }
//        UtilityModule.ConditionalComboFill(ref DDOrderNo, "Select OrderId, " + Str + @" CustomerOrderNo 
//        From OrderMaster(Nolock) Where CustomerId=" + DDCustCode.SelectedValue + " And CompanyId=" + DDCompany.SelectedValue + @" And 
//        Status=0 Order By CustomerOrderNo", true, "--Select--"); 
    }

    protected void InternalBucketDetail()
    {
        lblmsg.Text = "";
        try
        {
            string str = "", FilterBy = "";
            //if (DDProductionstatus.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Status='" + DDProductionstatus.SelectedItem.Text + "'";
            //    FilterBy = FilterBy + ", Production Status -" + DDProductionstatus.SelectedItem.Text;
            //    if (DDProductionstatus.SelectedIndex == 1)
            //    {
            //        str = str + " and PID.Pqty>0";
            //    }
            //}
            //if (DDProductionstatus.SelectedIndex > 0)
            //{
            //    //str = str + " and PIM.Status='" + DDProductionstatus.SelectedItem.Text + "'";
            //    FilterBy = FilterBy + ", Production Status -" + DDProductionstatus.SelectedItem.Text;
            //    if (DDProductionstatus.SelectedIndex == 1)
            //    {
            //        str = str + " and ls.Issueorderid=0";
            //    }
            //    if (DDProductionstatus.SelectedIndex == 2)
            //    {
            //        str = str + " and ls.Issueorderid>0";
            //    }
            //}
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and ls.Issueorderid=" + DDFolioNo.SelectedValue;
            //    FilterBy = FilterBy + ", Folio No. -" + DDFolioNo.SelectedItem.Text;
            //}
            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and Vf.Item_id=" + DDQtype.SelectedValue;
                FilterBy = FilterBy + ", Item Name -" + DDQtype.SelectedItem.Text;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and Vf.Qualityid=" + DDQuality.SelectedValue;
                FilterBy = FilterBy + ", Quality -" + DDQuality.SelectedItem.Text;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
                FilterBy = FilterBy + ", Design -" + DDDesign.SelectedItem.Text;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " and vf.Colorid=" + DDColor.SelectedValue;
                FilterBy = FilterBy + ", Color -" + DDColor.SelectedItem.Text;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
                FilterBy = FilterBy + ", Size -" + DDSize.SelectedItem.Text;
            }
            if (ChkselectDate.Checked == true)
            {
                //str = str + " and PIM.Assigndate>='" + txtfromDate.Text + "' and PIM.AssignDate<='" + txttodate.Text + "'";                
                str = str + " and ls.Dateadded>='" + txtfromDate.Text + "' and ls.Dateadded<='" + txttodate.Text + "'";
                FilterBy = FilterBy + ", From -" + txtfromDate.Text + " To - " + txttodate.Text;
            }
            //if (DDFoliotype.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Purchasefolio=" + DDFoliotype.SelectedValue;
            //    FilterBy = FilterBy + ", Folio Type -" + DDFoliotype.SelectedItem.Text;
            //}
            if (DDCustCode.SelectedIndex > 0)
            {
                str = str + " and OM.Customerid=" + DDCustCode.SelectedValue;
                FilterBy = FilterBy + ", Customer code -" + DDCustCode.SelectedItem.Text;
            }
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    str = str + " and od.orderid=" + DDOrderNo.SelectedValue;
            //    FilterBy = FilterBy + ", Order No. -" + DDOrderNo.SelectedItem.Text;
            //}
            //SqlParameter[] param = new SqlParameter[4];
            //param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            //param[1] = new SqlParameter("@EMpid", (DDWeaver.SelectedIndex > 0 ? DDWeaver.SelectedValue : "0"));          
            //param[2] = new SqlParameter("@where", str);           

            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETINTERNALBUCKETDETAIL", param);

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETINTERNALBUCKETDETAIL", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@EMpid",  "0");
            cmd.Parameters.AddWithValue("@Where", str);

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

                sht.Range("A1:Q1").Merge();
                sht.Range("A1").Value = "INTERNAL BUCKET DETAILS";
                sht.Range("A2:Q2").Merge();
                sht.Range("A2").Value = "Filter By :  " + FilterBy;
                sht.Row(2).Height = 30;
                sht.Range("A1:Q1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:Q2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:Q2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:Q2").Style.Alignment.SetWrapText();
                sht.Range("A1:Q2").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:Q2").Style.Font.FontSize = 10;
                sht.Range("A1:Q2").Style.Font.Bold = true;
                //*******Header
                sht.Range("A3").Value = "Plan Date";
                sht.Range("B3").Value = "Bucket IssueDate";
                sht.Range("C3").Value = "Shade No";
                sht.Range("D3").Value = "Bin No";
                sht.Range("E3").Value = "Lot No";
                sht.Range("F3").Value = "Tag No";
                sht.Range("G3").Value = "Yarn Req Qty";

                sht.Range("H3").Value = "Yarn Issue Qty";
                sht.Range("I3").Value = "Carpet No";
                sht.Range("J3").Value = "Item Name";
                sht.Range("K3").Value = "Quality";
                sht.Range("L3").Value = "Design";
                sht.Range("M3").Value = "Size";
                sht.Range("N3").Value = "Color";
                sht.Range("O3").Value = "Issue To";
                sht.Range("P3").Value = "Folio No";
                sht.Range("Q3").Value = "Carpet IssDate";

                sht.Range("A3:Q3").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A3:Q3").Style.Font.FontSize = 9;
                sht.Range("A3:Q3").Style.Font.Bold = true;
                //sht.Range("N3:P3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 4;
                int rowfrom = 4;
                //DataView dv = new DataView(ds.Tables[0]);
                //dv.Sort = "Issueorderid";
                //DataSet ds1 = new DataSet();
                //ds1.Tables.Add(dv.ToTable());

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":O" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":O" + row).Style.Font.FontSize = 9;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["TagDate"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Date"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColor"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["BinNo"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["LotNo"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["TagNo"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["CONSMPQTY"]);

                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ISSUEDQTY"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["TstockNo"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["designName"]);
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["size"]);
                    sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                    sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                    sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["FolioNo"]);
                    sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["AssignDate"]);
                    row = row + 1;
                }
                ////GRAND TOAL
                ////=SUM(L4:L929)
                sht.Range("G" + row).FormulaA1 = "=SUM(G" + rowfrom + ":G" + (row - 1) + ")";
                sht.Range("H" + row).FormulaA1 = "=SUM(H" + rowfrom + ":H" + (row - 1) + ")";
                sht.Range("G" + row + ":H" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("G" + row + ":H" + row).Style.Font.FontSize = 9;
                sht.Range("G" + row + ":H" + row).Style.Font.Bold = true;

                //*************
                sht.Columns(1, 20).AdjustToContents();
                //********************
                //***********BOrders
                using (var a = sht.Range("A1" + ":Q" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("InternalBucketDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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

                ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
    protected void WeavingRawBalanceShadeWise()
    {
        lblmsg.Text = "";
        try
        {
            string str = "", FilterBy = "";

            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and Vf.Item_id=" + DDQtype.SelectedValue;
                FilterBy = FilterBy + ", Item Name -" + DDQtype.SelectedItem.Text;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and Vf.Qualityid=" + DDQuality.SelectedValue;
                FilterBy = FilterBy + ", Quality -" + DDQuality.SelectedItem.Text;
            }
            //if (DDshade.SelectedIndex > 0)
            //{
            //    str = str + " and vf.Shadecolorid=" + DDshade.SelectedValue;
            //    FilterBy = FilterBy + ", ShadeColor -" + DDshade.SelectedItem.Text;
            //}

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_getweaverrawbalanceShadeWise", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@empid", 0);
            cmd.Parameters.AddWithValue("@Asondate", txtfromDate.Text);
            cmd.Parameters.AddWithValue("@Where", str);


            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            //*************
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            con.Close();
            con.Dispose();
            //***********
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

                sht.Range("A1:G1").Merge();
                sht.Range("A1").Value = "WEAVER RAW BALANCE SHADEWISE";
                sht.Range("A2:G2").Merge();
                sht.Range("A2").Value = "Filter By :  " + FilterBy;
                sht.Row(2).Height = 30;
                sht.Range("A1:G1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:G2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:G2").Style.Alignment.SetWrapText();
                sht.Range("A1:G2").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:G2").Style.Font.FontSize = 10;
                sht.Range("A1:G2").Style.Font.Bold = true;
                //*******Header
                sht.Range("A3").Value = "Weaver Name";
                sht.Range("B3").Value = "Folio No";
                sht.Range("C3").Value = "Shade No";
                sht.Range("D3").Value = "Consumption Qty";
                sht.Range("E3").Value = "Issue Qty";
                sht.Range("F3").Value = "Balance";

                sht.Range("A3:G3").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A3:G3").Style.Font.FontSize = 9;
                sht.Range("A3:G3").Style.Font.Bold = true;
                sht.Range("D3:G3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A3:G3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A3:G3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A3:G3").Style.Alignment.SetWrapText();


                DataView dv = new DataView(ds.Tables[0]);
                //dv.Sort = "FOLIONO";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());

                row = 4;
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":G" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":G" + row).Style.Font.FontSize = 9;

                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["EmpName"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["IssueOrderId"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["ShadeColorName"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["CONSUMEDQTY"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["ISSUEQTY"]);
                    //Actfullareasqyd = Convert.ToDecimal(ds1.Tables[0].Rows[i]["Actualfullareasqyd"]) * Convert.ToInt32(ds1.Tables[0].Rows[i]["qty"]);
                    sht.Range("F" + row).SetValue(Convert.ToDecimal(ds1.Tables[0].Rows[i]["CONSUMEDQTY"]) - Convert.ToDecimal(ds1.Tables[0].Rows[i]["ISSUEQTY"]));


                    row = row + 1;

                }
                //*************
                sht.Columns(1, 26).AdjustToContents();

                //sht.Columns("J").Width = 9.67;

                using (var a = sht.Range("A1" + ":G" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("Weaverrawbalanceshadewise_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void ChkUnpaidApprovalNo_CheckedChanged(object sender, EventArgs e)
    {
        //if (ChkUnpaidApprovalNo.Checked == true)
        //{
        //    trReportType.Visible = false;
        //    trFolioNo.Visible = false;
        //}
        //else
        //{
        //    trReportType.Visible = true;
        //    trFolioNo.Visible = true;
        //}
    }
    protected void WeaverUnpaidApprovalNo()
    {
        try
        {
            string str = "";

            str = str + " and PHA.CompanyId=" + DDCompany.SelectedValue;

            //if (DDWeaver.SelectedIndex > 0)
            //{
            //    str = str + " and PHA.Empid=" + DDWeaver.SelectedValue;
            //}
            //*****************
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[1] = new SqlParameter("@FromDate", txtfromDate.Text);
            param[2] = new SqlParameter("@ToDate", txttodate.Text);
            param[3] = new SqlParameter("@where", str);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetWeaverUnpaidApprovalNo", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptWeaverUnpaidApprovalNo.rpt";

                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptWeaverUnpaidApprovalNo.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "RawBal", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void ChkWeavingReport_CheckedChanged(object sender, EventArgs e)
    {
        //if (ChkWeavingReport.Checked == true)
        //{
        //    trProductionStatus.Visible = false;
        //    trFolioType.Visible = false;
        //    Trproductiontype.Visible = false;
        //    TRCustomerCode.Visible = false;
        //    TROrderNo.Visible = false;
        //}
        //else
        //{
        //    trProductionStatus.Visible = true;
        //    trFolioType.Visible = true;
        //    Trproductiontype.Visible = true;
        //    TRCustomerCode.Visible = true;
        //    TROrderNo.Visible = true;
        //}
    }
    protected void WeavingFolioDetails()
    {
        lblmsg.Text = "";
        try
        {
            string str = "", FilterBy = "";
            //if (DDProductionstatus.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Status='" + DDProductionstatus.SelectedItem.Text + "'";
            //    FilterBy = FilterBy + ", Production Status -" + DDProductionstatus.SelectedItem.Text;
            //    if (DDProductionstatus.SelectedIndex == 1)
            //    {
            //        str = str + " and PID.Pqty>0";
            //    }
            //}
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Issueorderid=" + DDFolioNo.SelectedValue;
            //    FilterBy = FilterBy + ", Folio No. -" + DDFolioNo.SelectedItem.Text;
            //}
            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and Vf.Item_id=" + DDQtype.SelectedValue;
                FilterBy = FilterBy + ", Item Name -" + DDQtype.SelectedItem.Text;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and Vf.Qualityid=" + DDQuality.SelectedValue;
                FilterBy = FilterBy + ", Quality -" + DDQuality.SelectedItem.Text;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
                FilterBy = FilterBy + ", Design -" + DDDesign.SelectedItem.Text;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " and vf.Colorid=" + DDColor.SelectedValue;
                FilterBy = FilterBy + ", Color -" + DDColor.SelectedItem.Text;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
                FilterBy = FilterBy + ", Size -" + DDSize.SelectedItem.Text;
            }
            if (ChkselectDate.Checked == true)
            {
                str = str + " and PIM.Assigndate>='" + txtfromDate.Text + "' and PIM.AssignDate<='" + txttodate.Text + "'";
                FilterBy = FilterBy + ", From -" + txtfromDate.Text + " To - " + txttodate.Text;
            }
            if (DDUnitname.SelectedIndex > 0)
            {
                str = str + " and PIM.Units=" + DDUnitname.SelectedValue;
                FilterBy = FilterBy + ", Unitname -" + DDUnitname.SelectedItem.Text;
            }
            ////if (DDFoliotype.SelectedIndex > 0)
            ////{
            ////    str = str + " and PIM.Purchasefolio=" + DDFoliotype.SelectedValue;
            ////    FilterBy = FilterBy + ", Folio Type -" + DDFoliotype.SelectedItem.Text;
            ////}
            ////if (DDCustCode.SelectedIndex > 0)
            ////{
            ////    str = str + " and OM.Customerid=" + DDCustCode.SelectedValue;
            ////    FilterBy = FilterBy + ", Customer code -" + DDCustCode.SelectedItem.Text;
            ////}
            ////if (DDOrderNo.SelectedIndex > 0)
            ////{
            ////    str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
            ////    FilterBy = FilterBy + ", Order No. -" + DDOrderNo.SelectedItem.Text;
            ////}
            //SqlParameter[] param = new SqlParameter[4];
            //param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            //param[1] = new SqlParameter("@Processid", 1);
            //param[2] = new SqlParameter("@where", str);
            //param[3] = new SqlParameter("@EMpid", (DDWeaver.SelectedIndex > 0 ? DDWeaver.SelectedValue : "0"));

            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETWEAVINGFOLIODETAILSWITHBAZAARDETAILS", param);

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETWEAVINGFOLIODETAILSWITHBAZAARDETAILS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 500;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Processid", 1);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@empid", "0");
           

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

                sht.Range("A1:U1").Merge();
                sht.Range("A1").Value = "WEAVING FOLIO DETAILS";
                sht.Range("A2:U2").Merge();
                sht.Range("A2").Value = "Filter By :  " + FilterBy;
                sht.Row(2).Height = 30;
                sht.Range("A1:U1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:U2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:U2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:U2").Style.Alignment.SetWrapText();
                sht.Range("A1:U2").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:U2").Style.Font.FontSize = 10;
                sht.Range("A1:U2").Style.Font.Bold = true;
                //*******Header
                sht.Range("A3").Value = "Emp Code";
                sht.Range("B3").Value = "Emp Name";
                sht.Range("C3").Value = "Folio No.";
                sht.Range("D3").Value = "Quality";
                sht.Range("E3").Value = "Design";
                sht.Range("F3").Value = "Color";
                sht.Range("G3").Value = "Folio Issue Date";

                sht.Range("H3").Value = "Boom Wool Wt";
                sht.Range("I3").Value = "Total Wool Issued On Folio";
                sht.Range("J3").Value = "Wool Issue Difference";
                sht.Range("K3").Value = "Seal No On Folio";
                sht.Range("L3").Value = "Bazaar Date";
                sht.Range("M3").Value = "Bazaar Weight";
                sht.Range("N3").Value = "Difference";
                sht.Range("O3").Value = "Bazaar Size";
                sht.Range("P3").Value = "Area";
                sht.Range("Q3").Value = "Rate";
                sht.Range("R3").Value = "Amount";
                sht.Range("S3").Value = "Comm Amt";
                sht.Range("T3").Value = "Penality";
                sht.Range("U3").Value = "Net Amt";

                sht.Range("A3:U3").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A3:U3").Style.Font.FontSize = 9;
                sht.Range("A3:U3").Style.Font.Bold = true;
                sht.Range("P3:U3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                //row = 4;
                int rowfrom = 4;
                DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "empcode", "empname", "IssueOrderId", "Qualityname", "Designname", "colorname", "AssignDate", "CONSMPQTY", "ISSUEQTY", "Item_finished_Id");
                DataView dv1 = new DataView(dtdistinct);
                dv1.Sort = "IssueOrderId";
                DataTable dtdistinct1 = dv1.ToTable();
                row = 4;
                foreach (DataRow dr in dtdistinct1.Rows)
                {
                    sht.Range("A" + row).SetValue(dr["empcode"]);
                    sht.Range("B" + row).SetValue(dr["empname"]);
                    sht.Range("C" + row).SetValue(dr["Issueorderid"]);
                    sht.Range("D" + row).SetValue(dr["Qualityname"]);
                    sht.Range("E" + row).SetValue(dr["Designname"]);
                    sht.Range("F" + row).SetValue(dr["colorname"]);
                    sht.Range("G" + row).SetValue(dr["AssignDate"]);

                    sht.Range("H" + row).SetValue(dr["CONSMPQTY"]);
                    sht.Range("I" + row).SetValue(dr["ISSUEQTY"]);
                    sht.Range("J" + row).SetValue(Convert.ToDecimal(dr["CONSMPQTY"]) - Convert.ToDecimal(dr["ISSUEQTY"]));

                    DataView dvitemdesc = new DataView(ds.Tables[0]);
                    dvitemdesc.RowFilter = "item_finished_Id='" + dr["item_finished_Id"] + "' and IssueOrderId='" + dr["Issueorderid"] + "'";
                    DataSet dsitemdesc = new DataSet();
                    dsitemdesc.Tables.Add(dvitemdesc.ToTable());
                    DataTable dtdistinctitemdesc = dsitemdesc.Tables[0];
                    ////DataTable dtdistinctitemdesc = dsitemdesc.Tables[0].DefaultView.ToTable(true, "Item_Finished_Id", "FolioNo", "Item_Name", "QualityName", "DesignName", "ColorName", "width", "Length", "Rate");
                    int rowfrommerge = 0;
                    decimal Weight = 0;
                    rowfrommerge = row;

                    foreach (DataRow dritemdesc in dtdistinctitemdesc.Rows)
                    {

                        sht.Range("K" + row).SetValue(dritemdesc["TStockNo"]);
                        sht.Range("L" + row).SetValue(dritemdesc["BazaarReceiveDate"]);
                        sht.Range("M" + row).SetValue(dritemdesc["WEIGHT"]);
                        Weight = Weight + Convert.ToDecimal(dritemdesc["WEIGHT"]);

                        sht.Range("O" + row).SetValue(dritemdesc["Size"]);
                        sht.Range("P" + row).SetValue(dritemdesc["Area"]);
                        sht.Range("Q" + row).SetValue(dritemdesc["Rate"]);
                        sht.Range("R" + row).SetValue(dritemdesc["Amount"]);
                        sht.Range("S" + row).SetValue(dritemdesc["CommAmt"]);
                        sht.Range("T" + row).SetValue(dritemdesc["PenalityAmt"]);
                        sht.Range("U" + row).SetValue((Convert.ToDecimal(dritemdesc["Amount"]) + Convert.ToDecimal(dritemdesc["CommAmt"])) - Convert.ToDecimal(dritemdesc["PenalityAmt"]));

                        row = row + 1;

                    }
                    sht.Range("N" + rowfrommerge + ":N" + (row - 1)).Merge();
                    if (dtdistinctitemdesc.Rows.Count > 0)
                    {
                        sht.Range("N" + rowfrommerge).SetValue(Convert.ToDecimal(dr["ISSUEQTY"]) - Weight);

                    }
                    sht.Range("N" + rowfrommerge + ":N" + (row - 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("N" + rowfrommerge + ":N" + (row - 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    row = row + 1;
                }
                sht.Range("G" + row).SetValue("Grand Total");
                sht.Range("H" + row).FormulaA1 = "=SUM(H" + rowfrom + ":H" + (row - 1) + ")";
                sht.Range("I" + row).FormulaA1 = "=SUM(I" + rowfrom + ":I" + (row - 1) + ")";
                sht.Range("J" + row).FormulaA1 = "=SUM(J" + rowfrom + ":J" + (row - 1) + ")";

                sht.Range("M" + row).FormulaA1 = "=SUM(M" + rowfrom + ":M" + (row - 1) + ")";
                sht.Range("N" + row).FormulaA1 = "=SUM(N" + rowfrom + ":N" + (row - 1) + ")";
                sht.Range("P" + row).FormulaA1 = "=SUM(P" + rowfrom + ":P" + (row - 1) + ")";
                sht.Range("R" + row).FormulaA1 = "=SUM(R" + rowfrom + ":R" + (row - 1) + ")";
                sht.Range("S" + row).FormulaA1 = "=SUM(S" + rowfrom + ":S" + (row - 1) + ")";
                sht.Range("T" + row).FormulaA1 = "=SUM(T" + rowfrom + ":T" + (row - 1) + ")";
                sht.Range("U" + row).FormulaA1 = "=SUM(U" + rowfrom + ":U" + (row - 1) + ")";
                sht.Range("G" + row + ":U" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("G" + row + ":U" + row).Style.Font.FontSize = 9;
                sht.Range("G" + row + ":U" + row).Style.Font.Bold = true;

                ////*************
                sht.Columns(1, 20).AdjustToContents();
                //********************
                //***********BOrders
                using (var a = sht.Range("A1" + ":U" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("WeavingFolioDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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

                ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
    protected void WeavingFolioBazaarDetails()
    {
        lblmsg.Text = "";
        try
        {
            string str = "", FilterBy = "";
            //if (DDProductionstatus.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Status='" + DDProductionstatus.SelectedItem.Text + "'";
            //    FilterBy = FilterBy + ", Production Status -" + DDProductionstatus.SelectedItem.Text;
            //    if (DDProductionstatus.SelectedIndex == 1)
            //    {
            //        str = str + " and PID.Pqty>0";
            //    }
            //}
            if (DDUnitname.SelectedIndex > 0)
            {
                str = str + " and PIM.Units=" + DDUnitname.SelectedValue;
                FilterBy = FilterBy + ", Unit Name -" + DDUnitname.SelectedItem.Text;
            }
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Issueorderid=" + DDFolioNo.SelectedValue;
            //    FilterBy = FilterBy + ", Folio No. -" + DDFolioNo.SelectedItem.Text;
            //}
            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and Vf.Item_id=" + DDQtype.SelectedValue;
                FilterBy = FilterBy + ", Item Name -" + DDQtype.SelectedItem.Text;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and Vf.Qualityid=" + DDQuality.SelectedValue;
                FilterBy = FilterBy + ", Quality -" + DDQuality.SelectedItem.Text;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
                FilterBy = FilterBy + ", Design -" + DDDesign.SelectedItem.Text;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " and vf.Colorid=" + DDColor.SelectedValue;
                FilterBy = FilterBy + ", Color -" + DDColor.SelectedItem.Text;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
                FilterBy = FilterBy + ", Size -" + DDSize.SelectedItem.Text;
            }
            if (ChkselectDate.Checked == true)
            {
                str = str + " and VWCR.RECEIVEDATE>='" + txtfromDate.Text + "' and VWCR.RECEIVEDATE<='" + txttodate.Text + "'";
                FilterBy = FilterBy + ", From -" + txtfromDate.Text + " To - " + txttodate.Text;
            }
            //if (DDFoliotype.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Purchasefolio=" + DDFoliotype.SelectedValue;
            //    FilterBy = FilterBy + ", Folio Type -" + DDFoliotype.SelectedItem.Text;
            //}
            //if (DDCustCode.SelectedIndex > 0)
            //{
            //    str = str + " and OM.Customerid=" + DDCustCode.SelectedValue;
            //    FilterBy = FilterBy + ", Customer code -" + DDCustCode.SelectedItem.Text;
            //}
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
            //    FilterBy = FilterBy + ", Order No. -" + DDOrderNo.SelectedItem.Text;
            //}
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@Processid", 1);
            param[2] = new SqlParameter("@where", str);
            param[3] = new SqlParameter("@EMpid", "0");

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETWEAVINGFOLIODETAILSWITHBAZAARDETAILS", param);

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

                sht.Range("A1:U1").Merge();
                sht.Range("A1").Value = "WEAVING FOLIO DETAILS";
                sht.Range("A2:U2").Merge();
                sht.Range("A2").Value = "Filter By :  " + FilterBy;
                sht.Row(2).Height = 30;
                sht.Range("A1:U1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:U2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:U2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:U2").Style.Alignment.SetWrapText();
                sht.Range("A1:U2").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:U2").Style.Font.FontSize = 10;
                sht.Range("A1:U2").Style.Font.Bold = true;
                //*******Header
                sht.Range("A3").Value = "Emp Code";
                sht.Range("B3").Value = "Emp Name";
                sht.Range("C3").Value = "Folio No.";
                sht.Range("D3").Value = "Quality";
                sht.Range("E3").Value = "Design";
                sht.Range("F3").Value = "Color";
                sht.Range("G3").Value = "Folio Issue Date";

                sht.Range("H3").Value = "Boom Wool Wt";
                sht.Range("I3").Value = "Total Wool Issued On Folio";
                sht.Range("J3").Value = "Wool Issue Difference";
                sht.Range("K3").Value = "Seal No On Folio";
                sht.Range("L3").Value = "Bazaar Date";
                sht.Range("M3").Value = "Bazaar Weight";
                sht.Range("N3").Value = "Difference";
                sht.Range("O3").Value = "Bazaar Size";
                sht.Range("P3").Value = "Area";
                sht.Range("Q3").Value = "Rate";
                sht.Range("R3").Value = "Amount";
                sht.Range("S3").Value = "Comm Amt";
                sht.Range("T3").Value = "Penality";
                sht.Range("U3").Value = "Net Amt";

                sht.Range("A3:U3").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A3:U3").Style.Font.FontSize = 9;
                sht.Range("A3:U3").Style.Font.Bold = true;
                sht.Range("P3:U3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                //row = 4;
                int rowfrom = 4;
                DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "empcode", "empname", "IssueOrderId", "Qualityname", "Designname", "colorname", "AssignDate", "CONSMPQTY", "ISSUEQTY", "Item_finished_Id");
                DataView dv1 = new DataView(dtdistinct);
                dv1.Sort = "IssueOrderId";
                DataTable dtdistinct1 = dv1.ToTable();
                row = 4;
                foreach (DataRow dr in dtdistinct1.Rows)
                {
                    sht.Range("A" + row).SetValue(dr["empcode"]);
                    sht.Range("B" + row).SetValue(dr["empname"]);
                    sht.Range("C" + row).SetValue(dr["Issueorderid"]);
                    sht.Range("D" + row).SetValue(dr["Qualityname"]);
                    sht.Range("E" + row).SetValue(dr["Designname"]);
                    sht.Range("F" + row).SetValue(dr["colorname"]);
                    sht.Range("G" + row).SetValue(dr["AssignDate"]);

                    sht.Range("H" + row).SetValue(dr["CONSMPQTY"]);
                    sht.Range("I" + row).SetValue(dr["ISSUEQTY"]);
                    sht.Range("J" + row).SetValue(Convert.ToDecimal(dr["CONSMPQTY"]) - Convert.ToDecimal(dr["ISSUEQTY"]));

                    DataView dvitemdesc = new DataView(ds.Tables[0]);
                    dvitemdesc.RowFilter = "item_finished_Id='" + dr["item_finished_Id"] + "' and IssueOrderId='" + dr["Issueorderid"] + "'";
                    DataSet dsitemdesc = new DataSet();
                    dsitemdesc.Tables.Add(dvitemdesc.ToTable());
                    DataTable dtdistinctitemdesc = dsitemdesc.Tables[0];
                    ////DataTable dtdistinctitemdesc = dsitemdesc.Tables[0].DefaultView.ToTable(true, "Item_Finished_Id", "FolioNo", "Item_Name", "QualityName", "DesignName", "ColorName", "width", "Length", "Rate");
                    int rowfrommerge = 0;
                    decimal Weight = 0;
                    rowfrommerge = row;

                    foreach (DataRow dritemdesc in dtdistinctitemdesc.Rows)
                    {

                        sht.Range("K" + row).SetValue(dritemdesc["TStockNo"]);
                        sht.Range("L" + row).SetValue(dritemdesc["BazaarReceiveDate"]);
                        sht.Range("M" + row).SetValue(dritemdesc["WEIGHT"]);
                        Weight = Weight + Convert.ToDecimal(dritemdesc["WEIGHT"]);

                        sht.Range("O" + row).SetValue(dritemdesc["Size"]);
                        sht.Range("P" + row).SetValue(dritemdesc["Area"]);
                        sht.Range("Q" + row).SetValue(dritemdesc["Rate"]);
                        sht.Range("R" + row).SetValue(dritemdesc["Amount"]);
                        sht.Range("S" + row).SetValue(dritemdesc["CommAmt"]);
                        sht.Range("T" + row).SetValue(dritemdesc["PenalityAmt"]);
                        sht.Range("U" + row).SetValue((Convert.ToDecimal(dritemdesc["Amount"]) + Convert.ToDecimal(dritemdesc["CommAmt"])) - Convert.ToDecimal(dritemdesc["PenalityAmt"]));

                        row = row + 1;

                    }
                    sht.Range("N" + rowfrommerge + ":N" + (row - 1)).Merge();
                    if (dtdistinctitemdesc.Rows.Count > 0)
                    {
                        sht.Range("N" + rowfrommerge).SetValue(Convert.ToDecimal(dr["ISSUEQTY"]) - Weight);

                    }
                    sht.Range("N" + rowfrommerge + ":N" + (row - 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("N" + rowfrommerge + ":N" + (row - 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    row = row + 1;
                }
                sht.Range("G" + row).SetValue("Grand Total");
                sht.Range("H" + row).FormulaA1 = "=SUM(H" + rowfrom + ":H" + (row - 1) + ")";
                sht.Range("I" + row).FormulaA1 = "=SUM(I" + rowfrom + ":I" + (row - 1) + ")";
                sht.Range("J" + row).FormulaA1 = "=SUM(J" + rowfrom + ":J" + (row - 1) + ")";

                sht.Range("M" + row).FormulaA1 = "=SUM(M" + rowfrom + ":M" + (row - 1) + ")";
                sht.Range("N" + row).FormulaA1 = "=SUM(N" + rowfrom + ":N" + (row - 1) + ")";
                sht.Range("P" + row).FormulaA1 = "=SUM(P" + rowfrom + ":P" + (row - 1) + ")";
                sht.Range("R" + row).FormulaA1 = "=SUM(R" + rowfrom + ":R" + (row - 1) + ")";
                sht.Range("S" + row).FormulaA1 = "=SUM(S" + rowfrom + ":S" + (row - 1) + ")";
                sht.Range("T" + row).FormulaA1 = "=SUM(T" + rowfrom + ":T" + (row - 1) + ")";
                sht.Range("U" + row).FormulaA1 = "=SUM(U" + rowfrom + ":U" + (row - 1) + ")";
                sht.Range("G" + row + ":U" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("G" + row + ":U" + row).Style.Font.FontSize = 9;
                sht.Range("G" + row + ":U" + row).Style.Font.Bold = true;

                ////*************
                sht.Columns(1, 20).AdjustToContents();
                //********************
                //***********BOrders
                using (var a = sht.Range("A1" + ":U" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("WeavingFolioDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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

                ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
    protected void IssRecConsumptionSummary()
    {
        if (DDCategory.SelectedValue == "2")
        {
            lblmsg.Text = "";
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
                param[1] = new SqlParameter("@Processid", 1);
                param[2] = new SqlParameter("@FromDate", txtfromDate.Text);
                param[3] = new SqlParameter("@ToDate", txttodate.Text);
                param[4] = new SqlParameter("Item_Id", DDQtype.SelectedValue);


                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETISSRECCONSUMPTIONSUMMARYREPORT", param);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                    }
                    string Path = "";
                    var xapp = new XLWorkbook();
                    var sht = xapp.Worksheets.Add("sheet1");


                    sht.Range("A1:U1").Merge();
                    sht.Range("A1").Value = "DateWise RawMaterial Summary";
                    sht.Range("A2:U2").Merge();
                    sht.Range("A2").Value = "Company Name :  " + DDCompany.SelectedItem.Text;
                    sht.Row(2).Height = 30;
                    sht.Range("A3:U3").Merge();
                    sht.Range("A3").Value = "Date From :  " + txtfromDate.Text + " " + "To" + " " + txttodate.Text;
                    sht.Range("A1:U1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A2:U3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A2:U3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("A2:U3").Style.Alignment.SetWrapText();
                    sht.Range("A1:U3").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A1:U3").Style.Font.FontSize = 10;
                    sht.Range("A1:U3").Style.Font.Bold = true;
                    //*******Header
                    sht.Range("A4").Value = "Date";
                    sht.Range("B4").Value = "Type";
                    sht.Range("A4:B4").Style.Font.Bold = true;

                    int row = 4;
                    int column = 4;
                    int noofrows = 0;
                    int i = 0;
                    int Dynamiccol = 2;
                    int Dynamiccolstart = Dynamiccol + 1;
                    int Dynamiccolend;
                    int Totalcol;
                    decimal GrandTotalIssQty = 0;
                    decimal GrandTotalRecQty = 0;
                    decimal GrandTotalConsumeQty = 0;
                    decimal GrandTotalActualConsumeQty = 0;

                    decimal GrandTotalIssQty2 = 0;
                    decimal GrandTotalRecQty2 = 0;
                    decimal GrandTotalConsumeQty2 = 0;
                    decimal GrandTotalActualConsumeQty2 = 0;
                    decimal GrandTotal = 0;
                    string columnname = "";
                    DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "Item_Name");
                    noofrows = dtdistinct.Rows.Count;

                    for (i = 0; i < noofrows; i++)
                    {
                        //columnname = UtilityModule.GetExcelCellColumnName(i+3);
                        Dynamiccol = Dynamiccol + 1;
                        sht.Cell(row, Dynamiccol).Value = dtdistinct.Rows[i]["Item_Name"].ToString();
                        sht.Cell(row, Dynamiccol).Style.Font.Bold = true;

                        //sht.Range(columnname + column).Value = dtdistinct.Rows[i]["Item_Name"].ToString();
                    }
                    Dynamiccolend = Dynamiccol;
                    Totalcol = Dynamiccolend + 1;
                    sht.Cell(row, Totalcol).Value = "Total";

                    sht.Cell(row, Totalcol).Style.Font.Bold = true;

                    row = row + 1;

                    DataTable dtdistinctDateType = ds.Tables[0].DefaultView.ToTable(true, "Date", "TranType");
                    DataView dv1 = new DataView(dtdistinctDateType);
                    dv1.Sort = "Date";
                    DataTable dtdistinctDateType1 = dv1.ToTable();
                    foreach (DataRow dr in dtdistinctDateType1.Rows)
                    {
                        sht.Range("A" + row).SetValue(dr["Date"]);
                        sht.Range("B" + row).SetValue(dr["TranType"]);

                        decimal TotalSumOneRow = 0;
                        for (int k = Dynamiccolstart; k <= Dynamiccolend; k++)
                        {
                            var itemname = sht.Cell(4, k).Value;
                            decimal IssQty = 0;
                            decimal RecQty = 0;
                            decimal ConsQty = 0;
                            decimal IssRecConQty = 0;
                            decimal ActualConQty = 0;

                            DataRow[] foundRows;
                            foundRows = ds.Tables[0].Select("Date='" + dr["Date"] + "' and TranType='" + dr["TranType"] + "' and Item_Name='" + itemname + "' ");
                            if (foundRows.Length > 0)
                            {
                                if (dr["TranType"].ToString() == "I")
                                {
                                    IssQty = Convert.ToDecimal(ds.Tables[0].Compute("sum(IssQty)", "Date='" + dr["Date"] + "' and TranType='" + dr["TranType"] + "' and Item_Name='" + itemname + "' "));
                                }
                                else if (dr["TranType"].ToString() == "R")
                                {
                                    RecQty = Convert.ToDecimal(ds.Tables[0].Compute("sum(RecQty)", "Date='" + dr["Date"] + "' and TranType='" + dr["TranType"] + "' and Item_Name='" + itemname + "' "));
                                }
                                else if (dr["TranType"].ToString() == "C")
                                {
                                    ConsQty = Convert.ToDecimal(ds.Tables[0].Compute("sum(ConsumeQty)", "Date='" + dr["Date"] + "' and TranType='" + dr["TranType"] + "' and Item_Name='" + itemname + "' "));
                                }
                                else if (dr["TranType"].ToString() == "AC")
                                {
                                    ActualConQty = Convert.ToDecimal(ds.Tables[0].Compute("sum(ActualConsumed)", "Date='" + dr["Date"] + "' and TranType='" + dr["TranType"] + "' and Item_Name='" + itemname + "' "));
                                }
                            }

                            IssRecConQty = IssQty + RecQty + ConsQty + ActualConQty;
                            TotalSumOneRow = TotalSumOneRow + IssRecConQty;
                            sht.Cell(row, k).Value = IssRecConQty;
                        }
                        Totalcol = Dynamiccolend + 1;
                        sht.Cell(row, Totalcol).Value = TotalSumOneRow;
                        row = row + 1;
                    }
                    row = row + 1;
                    sht.Range("A" + row).SetValue("Total");

                    for (int l = Dynamiccolstart; l <= Dynamiccolend; l++)
                    {
                        var itemname = sht.Cell(4, l).Value;

                        GrandTotalIssQty = 0;
                        GrandTotalRecQty = 0;
                        GrandTotalConsumeQty = 0;
                        GrandTotalActualConsumeQty = 0;
                        DataRow[] foundRows1;
                        foundRows1 = ds.Tables[0].Select("TranType='I' and Item_Name='" + itemname + "' ");
                        if (foundRows1.Length > 0)
                        {
                            GrandTotalIssQty = Convert.ToDecimal(ds.Tables[0].Compute("sum(IssQty)", "TranType='I' and Item_Name='" + itemname + "' "));
                        }
                        foundRows1 = ds.Tables[0].Select("TranType='R' and Item_Name='" + itemname + "' ");
                        if (foundRows1.Length > 0)
                        {
                            GrandTotalRecQty = Convert.ToDecimal(ds.Tables[0].Compute("sum(RecQty)", "TranType='R' and Item_Name='" + itemname + "' "));
                        }
                        foundRows1 = ds.Tables[0].Select("TranType='C' and Item_Name='" + itemname + "' ");
                        if (foundRows1.Length > 0)
                        {
                            GrandTotalConsumeQty = Convert.ToDecimal(ds.Tables[0].Compute("sum(ConsumeQty)", "TranType='C' and Item_Name='" + itemname + "' "));
                        }
                        foundRows1 = ds.Tables[0].Select("TranType='AC' and Item_Name='" + itemname + "' ");
                        if (foundRows1.Length > 0)
                        {
                            GrandTotalActualConsumeQty = Convert.ToDecimal(ds.Tables[0].Compute("sum(ActualConsumed)", "TranType='AC' and Item_Name='" + itemname + "' "));
                        }

                        sht.Cell(row, l).Value = " " + "I=" + " " + GrandTotalIssQty + "\r\n " + "R=" + " " + GrandTotalRecQty + "\r\n " + "C=" + " " + GrandTotalConsumeQty + "\r\n " + "AC=" + " " + GrandTotalActualConsumeQty;
                        sht.Row(row).Height = 60;
                    }

                    DataRow[] foundRows2;
                    foundRows2 = ds.Tables[0].Select("TranType='I'");
                    if (foundRows2.Length > 0)
                    {
                        GrandTotalIssQty2 = Convert.ToDecimal(ds.Tables[0].Compute("sum(IssQty)", "TranType='I' "));
                    }
                    foundRows2 = ds.Tables[0].Select("TranType='R' ");
                    if (foundRows2.Length > 0)
                    {
                        GrandTotalRecQty2 = Convert.ToDecimal(ds.Tables[0].Compute("sum(RecQty)", "TranType='R' "));
                    }
                    foundRows2 = ds.Tables[0].Select("TranType='C' ");
                    if (foundRows2.Length > 0)
                    {
                        GrandTotalConsumeQty2 = Convert.ToDecimal(ds.Tables[0].Compute("sum(ConsumeQty)", "TranType='C' "));
                    }
                    foundRows2 = ds.Tables[0].Select("TranType='AC' ");
                    if (foundRows2.Length > 0)
                    {
                        GrandTotalActualConsumeQty2 = Convert.ToDecimal(ds.Tables[0].Compute("sum(ActualConsumed)", "TranType='AC' "));
                    }

                    Totalcol = Dynamiccolend + 1;
                    sht.Cell(row, Totalcol).Value = " " + "I=" + " " + GrandTotalIssQty2 + "\r\n " + "R=" + " " + GrandTotalRecQty2 + "\r\n " + "C=" + " " + GrandTotalConsumeQty2 + "\r\n " + "AC=" + " " + GrandTotalActualConsumeQty2;
                    row = row + 1;

                    ////*************
                    sht.Columns(1, 20).AdjustToContents();
                    //********************
                    //***********BOrders
                    using (var a = sht.Range("A1" + ":U" + row))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                    string Fileextension = "xlsx";
                    string filename = UtilityModule.validateFilename("IssRecConsumptionSummary_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
                }

            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;
            }

        }

    }
    //protected void ItemQualityWiseWeavingPaymentSummary()
    //{
    //    try
    //    {
    //        string str = "select *,'" + txtfromDate.Text + "' as FromDate,'" + txttodate.Text + "' as Todate," + (ChkselectDate.Checked == true ? "1" : "0") + " as Dateflag,case when "+DDFoliotype.SelectedValue+"=1 then 'PURCHASE' Else 'PRODUCTION' End as FolioType From V_WeavercarpetreceiveDetail Where CompanyId=" + DDCompany.SelectedValue;


    //        if (DDQtype.SelectedIndex > 0)
    //        {
    //            str = str + " and Item_id=" + DDQtype.SelectedValue;
    //        }
    //        if (DDQuality.SelectedIndex > 0)
    //        {
    //            str = str + " and Qualityid=" + DDQuality.SelectedValue;
    //        }
    //        if (DDFoliotype.SelectedIndex > 0)
    //        {
    //            str = str + " and Purchasefolio=" + DDFoliotype.SelectedValue;
    //            //FilterBy = FilterBy + ", Folio Type -" + DDFoliotype.SelectedItem.Text;
    //        }
    //        if (ChkselectDate.Checked == true)
    //        {
    //            str = str + " and ReceiveDate>='" + txtfromDate.Text + "' and ReceiveDate<='" + txttodate.Text + "'";
    //        }

    //        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            Session["rptFileName"] = "~\\Reports\\RptWeavingPaymentSummaryQualityWise.rpt";

    //            Session["GetDataset"] = ds;
    //            Session["dsFileName"] = "~\\ReportSchema\\RptWeavingPaymentSummaryQualityWise.xsd";

    //            StringBuilder stb = new StringBuilder();
    //            stb.Append("<script>");
    //            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
    //            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    //        }
    //        else
    //        {
    //            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblmsg.Text = ex.Message;
    //    }
    //}
    protected void FillBazaarReturnGatePassNo()
    {
        string str = "";
//        if (RDBazaarReturnGatePassDetail.Checked == true)
//        {
//            str = @"select distinct PRRS.GatePassNo,PRRS.GatePassNo as GatePassNo1 from PRODUCTIONRECEIVEREJECTEDSTOCK PRRS 
//                    INNER JOIN PROCESS_ISSUE_MASTER_1 PIM ON PRRS.IssueOrderID=PIM.IssueOrderId
//                    Where PRRS.IssueOrderId=" + DDFolioNo.SelectedValue + "";
//        }        
//        UtilityModule.ConditionalComboFill(ref DDReturnGatePassNo, str, true, "---Plz Select---");
    }
    protected void DDFolioNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (RDBazaarReturnGatePassDetail.Checked == true)
        //{
        //    FillBazaarReturnGatePassNo();
        //}
        //else
        //{
        //    DDReturnGatePassNo.Items.Clear();
        //}
    }

    protected void BazaarReturnGatePassDetail()
    {
        try
        {
            string str = "";                      

            //str = str + " and PRRS.CompanyId=" + DDCompany.SelectedValue;           
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and PRRS.IssueOrderID=" + DDFolioNo.SelectedValue;
            //}
            //if (DDReturnGatePassNo.SelectedIndex > 0)
            //{
            //    str = str + " and PRRS.GatePassNo=" + DDReturnGatePassNo.SelectedValue;
            //}            
            //*****************
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[2] = new SqlParameter("@IssueOrderID", 0);           
            param[3] = new SqlParameter("@where", str);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_FORPRODUCTIONRECEIVE_RETURNLOOMGATEPASSREPORTDETAIL", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //if (ChkBazaarReturnGatePassSummary.Checked == true)
                //{
                //    Session["rptFileName"] = "~\\Reports\\RptProductionreceivereturnloomgatepassSummaryReport.rpt";
                //}
                //else
                //{
                    Session["rptFileName"] = "~\\Reports\\RptProductionreceivereturnloomgatepassdetailReport.rpt";
               // }

                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptProductionreceivereturnloomgatepassdetailReport.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "RawBal", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }

    protected void WeaverRawMaterialOnLoom()
    {
       
            lblmsg.Text = "";
            try
            {
                string str = "";

                //str = str + " and PRRS.CompanyId=" + DDCompany.SelectedValue;  
                //if (DDWeaver.SelectedIndex > 0)
                //{
                //    str = str + " and PIM.EMPID=" + DDWeaver.SelectedValue;
                //}  
                //if (DDFolioNo.SelectedIndex > 0)
                //{
                //    str = str + " and PIM.IssueOrderID=" + DDFolioNo.SelectedValue;
                //}                        

                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
                param[1] = new SqlParameter("@Processid", 1);
                param[2] = new SqlParameter("@FromDate", txtfromDate.Text);
                param[3] = new SqlParameter("@ToDate", txttodate.Text);
                param[4] = new SqlParameter("@where", str);


                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETWEAVERRAWMATERIALONLOOMREPORT", param);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                    }
                    string Path = "";
                    var xapp = new XLWorkbook();
                    var sht = xapp.Worksheets.Add("sheet1");


                    sht.Range("A1:U1").Merge();
                    sht.Range("A1").Value = "Raw Material ONLOOM (WEAVER & SLIP WISE)";
                    sht.Range("A2:U2").Merge();
                    sht.Range("A2").Value = "Company Name :  " + DDCompany.SelectedItem.Text;
                    sht.Row(2).Height = 30;
                    sht.Range("A3:U3").Merge();
                    sht.Range("A3").Value = "Date From :  " + txtfromDate.Text + " " + "To" + " " + txttodate.Text;
                    sht.Range("A1:U1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A2:U3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A2:U3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("A2:U3").Style.Alignment.SetWrapText();
                    sht.Range("A1:U3").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A1:U3").Style.Font.FontSize = 10;
                    sht.Range("A1:U3").Style.Font.Bold = true;
                    //*******Header
                    sht.Range("A4").Value = "SR No";
                    sht.Range("B4").Value = "Qlt & Description";
                    sht.Range("C4").Value = "Size";
                    sht.Range("D4").Value = "Slip No";                   
                    sht.Range("E4").Value = "Order pcs";
                    sht.Range("F4").Value = "Bal. Pcs";
                    sht.Range("G4").Value = "Bal. Area Y2";
                    sht.Range("H4").Value = "Tran Date";
                    sht.Range("A4:H4").Style.Font.Bold = true;

                    int row = 4;                   
                    int noofrows = 0;
                    int i = 0;
                    int Dynamiccol = 8;
                    int Dynamiccolstart = Dynamiccol + 1;
                    int Dynamiccolend;
                    int Totalcol;
                    int rowfrom = 5;
                                
                    DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "RawMaterialItemName");
                    noofrows = dtdistinct.Rows.Count;

                    for (i = 0; i < noofrows; i++)
                    {
                        //columnname = UtilityModule.GetExcelCellColumnName(i+3);
                        Dynamiccol = Dynamiccol + 1;
                        sht.Cell(row, Dynamiccol).Value = dtdistinct.Rows[i]["RawMaterialItemName"].ToString();
                        sht.Cell(row, Dynamiccol).Style.Font.Bold = true;

                        //sht.Range(columnname + column).Value = dtdistinct.Rows[i]["Item_Name"].ToString();
                    }
                    Dynamiccolend = Dynamiccol;
                    Totalcol = Dynamiccolend + 1;
                    //sht.Cell(row, Totalcol).Value = "Total";
                    //sht.Cell(row, Totalcol).Style.Font.Bold = true;

                    row = row + 1;

                    int srno = 0;
                    DataTable dtdistinctEmpAddress = ds.Tables[0].DefaultView.ToTable(true, "EmpId","EMPNAME", "ADDRESS");
                    DataView dv1 = new DataView(dtdistinctEmpAddress);
                    dv1.Sort = "EMPNAME";
                    DataTable dtdistinctEmpAddress1 = dv1.ToTable();
                    foreach (DataRow dr in dtdistinctEmpAddress1.Rows)
                    {
                        srno = srno + 1;
                        sht.Range("A" + row).SetValue(srno);
                        sht.Range("B" + row).SetValue(dr["EMPNAME"]);
                        sht.Range("C" + row + ':' + "G" + row).Merge();
                        sht.Range("C" + row).SetValue(dr["ADDRESS"]);

                        sht.Range("A" + row+":C" + row).Style.Font.Bold = true;
                        
                        row = row + 1;

                        int TempIssueOrderId = 0;
                        DataTable dtdistinctItemDetails = ds.Tables[0].DefaultView.ToTable(true, "EmpId","QualityName", "DesignName", "ColorName","Size","Area","SinglePcsArea","IssueOrderId","Qty","RecQty");
                        DataView dv2 = new DataView(dtdistinctItemDetails);
                        dv2.RowFilter = "EmpId='" + dr["EmpId"] + "'";
                        DataTable dtdistinctItemDetails2 = dv2.ToTable();
                        foreach (DataRow dr2 in dtdistinctItemDetails2.Rows)
                         {
                             int BalPcs = Convert.ToInt32(dr2["Qty"]) - Convert.ToInt32(dr2["RecQty"]);

                             if (BalPcs > 0)
                             {

                                 sht.Range("B" + row).SetValue(dr2["QualityName"] + " " + dr2["DesignName"] + " " + dr2["ColorName"]);
                                 sht.Range("C" + row).SetValue(dr2["Size"]);
                                 sht.Range("D" + row).SetValue(dr2["IssueOrderId"]);
                                 sht.Range("E" + row).SetValue(dr2["Qty"]);

                                 sht.Range("F" + row).SetValue(BalPcs);
                                 sht.Range("G" + row).SetValue(Convert.ToDouble(dr2["SinglePcsArea"]) * BalPcs);

                                 if (Convert.ToInt32(dr2["IssueOrderId"]) != TempIssueOrderId)
                                 {
                                     TempIssueOrderId = Convert.ToInt32(dr2["IssueOrderId"]);

                                     DataTable dtdistinctRawMaterialItemDetails = ds.Tables[0].DefaultView.ToTable(true, "IssueOrderId", "RawMaterialIssueDate", "RawMaterialItemName", "IssueQty");
                                     DataView dv3 = new DataView(dtdistinctRawMaterialItemDetails);
                                     dv3.RowFilter = "IssueOrderID='" + dr2["Issueorderid"] + "'";
                                     dv3.Sort = "RawMaterialIssueDate";
                                     DataSet dsRawMaterialitemdesc = new DataSet();
                                     dsRawMaterialitemdesc.Tables.Add(dv3.ToTable());
                                     DataTable dtdistinctDateType1 = dsRawMaterialitemdesc.Tables[0];
                                     sht.Range("H" + row).SetValue(dsRawMaterialitemdesc.Tables[0].Rows[0]["RawMaterialIssueDate"]);
                                     foreach (DataRow dr3 in dtdistinctDateType1.Rows)
                                     {
                                         decimal TotalSumOneRow = 0;
                                         for (int k = Dynamiccolstart; k <= Dynamiccolend; k++)
                                         {
                                             var itemname = sht.Cell(4, k).Value;
                                             decimal IssQty = 0;
                                             decimal IssRecConQty = 0;

                                             DataRow[] foundRows;
                                             foundRows = dsRawMaterialitemdesc.Tables[0].Select("RawMaterialIssueDate='" + dr3["RawMaterialIssueDate"] + "' and IssueOrderId='" + dr3["IssueOrderId"] + "' and RawMaterialItemName='" + itemname + "' ");
                                             if (foundRows.Length > 0)
                                             {
                                                 IssQty = Convert.ToDecimal(dsRawMaterialitemdesc.Tables[0].Compute("sum(IssueQty)", "IssueOrderId='" + dr3["IssueOrderId"] + "' and RawMaterialItemName='" + itemname + "' "));

                                             }
                                             IssRecConQty = IssQty;
                                             TotalSumOneRow = TotalSumOneRow + IssRecConQty;
                                             sht.Cell(row, k).Value = IssRecConQty;
                                         }
                                         //Totalcol = Dynamiccolend + 1;
                                         //sht.Cell(row, Totalcol).Value = TotalSumOneRow;
                                         //row = row + 1;
                                     }

                                 }
                                 row = row + 1;
                             }
                           
                         }
                        row = row + 1;
                        
                    }
                    sht.Range("D" + row).SetValue("Grand Total");
                    sht.Range("E" + row).FormulaA1 = "=SUM(E" + rowfrom + ":E" + (row - 1) + ")";
                    sht.Range("F" + row).FormulaA1 = "=SUM(F" + rowfrom + ":F" + (row - 1) + ")";
                    sht.Range("G" + row).FormulaA1 = "=SUM(G" + rowfrom + ":G" + (row - 1) + ")";

                    sht.Range("D" + row + ":G" + row).Style.Font.Bold = true;

                    for (int k = Dynamiccolstart; k <= Dynamiccolend; k++)
                    {
                        var itemname = sht.Cell(4, k).Value;

                        DataTable dtdistinctRawMaterialItemDetails4 = ds.Tables[0].DefaultView.ToTable(true, "IssueOrderId", "RawMaterialIssueDate", "RawMaterialItemName", "IssueQty");
                        DataView dv4 = new DataView(dtdistinctRawMaterialItemDetails4);                                
                        DataSet dsRawMaterialitemdesc4 = new DataSet();
                        dsRawMaterialitemdesc4.Tables.Add(dv4.ToTable());                               
                        
                        sht.Cell(row, k).Value = dsRawMaterialitemdesc4.Tables[0].Compute("sum(IssueQty)", "RawMaterialItemName='" + itemname + "' ");

                        sht.Cell(row, k).Style.Font.Bold = true;                       
                        
                    } 
                   
                    row = row + 1;

                    ////*************
                    sht.Columns(1, 20).AdjustToContents();
                    //********************
                    //***********BOrders
                    using (var a = sht.Range("A1" + ":U" + row))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                    string Fileextension = "xlsx";
                    string filename = UtilityModule.validateFilename("RawMaterialONLOOM_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
                }

            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;
            }      

    }
    protected void ChkBazaarReceiveOnLoom_CheckedChanged(object sender, EventArgs e)
    {
        //if (ChkBazaarReceiveOnLoom.Checked == true)
        //{
        //    trProductionStatus.Visible = false;
        //    trFolioType.Visible = false;
        //    Trproductiontype.Visible = false;
        //    TRCustomerCode.Visible = false;
        //    TROrderNo.Visible = false;
        //}
        //else
        //{
        //    trProductionStatus.Visible = true;
        //    trFolioType.Visible = true;
        //    Trproductiontype.Visible = true;
        //    TRCustomerCode.Visible = true;
        //    TROrderNo.Visible = true;
        //}
    }
    protected void WeaverBazaarReceiveOnLoom()
    {
        lblmsg.Text = "";
        try
        {
            string str = "";

            //str = str + " and PRRS.CompanyId=" + DDCompany.SelectedValue;  
            //if (DDWeaver.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.EMPID=" + DDWeaver.SelectedValue;
            //}
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.IssueOrderID=" + DDFolioNo.SelectedValue;
            //}

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@Processid", 1);
            param[2] = new SqlParameter("@FromDate", txtfromDate.Text);
            param[3] = new SqlParameter("@ToDate", txttodate.Text);
            param[4] = new SqlParameter("@where", str);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETBAZAARRECEIVEONLOOMREPORT", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");

                sht.Range("A1:U1").Merge();
                sht.Range("A1").Value = "FINAL PURJA (WEAVER & SLIP WISE)";
                sht.Range("A2:U2").Merge();
                sht.Range("A2").Value = "Company Name :  " + DDCompany.SelectedItem.Text;
                sht.Row(2).Height = 30;
                sht.Range("A3:U3").Merge();
                sht.Range("A3").Value = "Date From :  " + txtfromDate.Text + " " + "To" + " " + txttodate.Text;
                sht.Range("A1:U1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:U3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:U3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:U3").Style.Alignment.SetWrapText();
                sht.Range("A1:U3").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:U3").Style.Font.FontSize = 10;
                sht.Range("A1:U3").Style.Font.Bold = true;
                //*******Header
                sht.Range("A4").Value = "SR No";
                sht.Range("B4").Value = "Qlt & Description";
                sht.Range("C4").Value = "Size";
                sht.Range("D4").Value = "Slip No";
                sht.Range("E4").Value = "Order pcs";
                sht.Range("F4").Value = "Baz Pcs";
                sht.Range("G4").Value = "Baz. Area Y2";
                sht.Range("H4").Value = "Tran Date";
                sht.Range("A4:H4").Style.Font.Bold = true;

                int row = 4;
                int noofrows = 0;
                int i = 0;
                int Dynamiccol = 8;
                int Dynamiccolstart = Dynamiccol + 1;
                int Dynamiccolend;
                int Totalcol;
                int rowfrom = 5;

                DataTable dtdistinct = ds.Tables[0].DefaultView.ToTable(true, "RawMaterialItemName");
                noofrows = dtdistinct.Rows.Count;

                for (i = 0; i < noofrows; i++)
                {
                    //columnname = UtilityModule.GetExcelCellColumnName(i+3);
                    Dynamiccol = Dynamiccol + 1;
                    sht.Cell(row, Dynamiccol).Value = dtdistinct.Rows[i]["RawMaterialItemName"].ToString();
                    sht.Cell(row, Dynamiccol).Style.Font.Bold = true;

                    //sht.Range(columnname + column).Value = dtdistinct.Rows[i]["Item_Name"].ToString();
                }
                Dynamiccolend = Dynamiccol;
                Totalcol = Dynamiccolend + 1;
                //sht.Cell(row, Totalcol).Value = "Total";
                //sht.Cell(row, Totalcol).Style.Font.Bold = true;

                row = row + 1;

                int srno = 0;
                DataTable dtdistinctEmpAddress = ds.Tables[0].DefaultView.ToTable(true, "EmpId", "EMPNAME", "ADDRESS");
                DataView dv1 = new DataView(dtdistinctEmpAddress);
                dv1.Sort = "EMPNAME";
                DataTable dtdistinctEmpAddress1 = dv1.ToTable();
                foreach (DataRow dr in dtdistinctEmpAddress1.Rows)
                {
                    srno = srno + 1;
                    sht.Range("A" + row).SetValue(srno);
                    sht.Range("B" + row).SetValue(dr["EMPNAME"]);
                    sht.Range("C" + row + ':' + "G" + row).Merge();
                    sht.Range("C" + row).SetValue(dr["ADDRESS"]);

                    sht.Range("A" + row + ":C" + row).Style.Font.Bold = true;

                    row = row + 1;

                    int TempIssueOrderId = 0;
                    DataTable dtdistinctItemDetails = ds.Tables[0].DefaultView.ToTable(true, "EmpId", "QualityName", "DesignName", "ColorName", "Size", "Area", "SinglePcsArea", "IssueOrderId", "Qty", "RecQty");
                    DataView dv2 = new DataView(dtdistinctItemDetails);
                    dv2.RowFilter = "EmpId='" + dr["EmpId"] + "'";
                    DataTable dtdistinctItemDetails2 = dv2.ToTable();
                    foreach (DataRow dr2 in dtdistinctItemDetails2.Rows)
                    {
                        sht.Range("B" + row).SetValue(dr2["QualityName"] + " " + dr2["DesignName"] + " " + dr2["ColorName"]);
                        sht.Range("C" + row).SetValue(dr2["Size"]);
                        sht.Range("D" + row).SetValue(dr2["IssueOrderId"]);
                        sht.Range("E" + row).SetValue(dr2["Qty"]);
                        //int BalPcs = Convert.ToInt32(dr2["Qty"]) - Convert.ToInt32(dr2["RecQty"]);
                        sht.Range("F" + row).SetValue(dr2["RecQty"]);
                        sht.Range("G" + row).SetValue(Convert.ToDouble(dr2["SinglePcsArea"]) * Convert.ToDouble(dr2["RecQty"]));

                        if (Convert.ToInt32(dr2["IssueOrderId"]) != TempIssueOrderId)
                        {
                            TempIssueOrderId = Convert.ToInt32(dr2["IssueOrderId"]);

                            DataTable dtdistinctRawMaterialItemDetails = ds.Tables[0].DefaultView.ToTable(true, "IssueOrderId", "RawMaterialIssueDate", "RawMaterialItemName", "IssueQty","RawMaterialReceiveQty");
                            DataView dv3 = new DataView(dtdistinctRawMaterialItemDetails);
                            dv3.RowFilter = "IssueOrderID='" + dr2["Issueorderid"] + "'";
                            dv3.Sort = "RawMaterialIssueDate";
                            DataSet dsRawMaterialitemdesc = new DataSet();
                            dsRawMaterialitemdesc.Tables.Add(dv3.ToTable());
                            DataTable dtdistinctDateType1 = dsRawMaterialitemdesc.Tables[0];
                            sht.Range("H" + row).SetValue(dsRawMaterialitemdesc.Tables[0].Rows[0]["RawMaterialIssueDate"]);
                            foreach (DataRow dr3 in dtdistinctDateType1.Rows)
                            {
                                decimal TotalSumOneRow = 0;
                                for (int k = Dynamiccolstart; k <= Dynamiccolend; k++)
                                {
                                    var itemname = sht.Cell(4, k).Value;
                                    decimal IssQty = 0;
                                    decimal ReceiveQty = 0;
                                    decimal IssRecConQty = 0;

                                    DataRow[] foundRows;
                                    foundRows = dsRawMaterialitemdesc.Tables[0].Select("RawMaterialIssueDate='" + dr3["RawMaterialIssueDate"] + "' and IssueOrderId='" + dr3["IssueOrderId"] + "' and RawMaterialItemName='" + itemname + "' ");
                                    if (foundRows.Length > 0)
                                    {
                                        IssQty = Convert.ToDecimal(dsRawMaterialitemdesc.Tables[0].Compute("sum(IssueQty)", "IssueOrderId='" + dr3["IssueOrderId"] + "' and RawMaterialItemName='" + itemname + "' "));
                                    }
                                    if (foundRows.Length > 0)
                                    {
                                        ReceiveQty = Convert.ToDecimal(dsRawMaterialitemdesc.Tables[0].Compute("sum(RawMaterialReceiveQty)", "IssueOrderId='" + dr3["IssueOrderId"] + "' and RawMaterialItemName='" + itemname + "' "));
                                    }
                                    IssRecConQty = IssQty - ReceiveQty;
                                   // TotalSumOneRow = TotalSumOneRow + IssRecConQty;
                                    sht.Cell(row, k).Value = IssRecConQty;
                                }
                                //Totalcol = Dynamiccolend + 1;
                                //sht.Cell(row, Totalcol).Value = TotalSumOneRow;
                                //row = row + 1;
                            }

                        }
                        row = row + 1;

                    }
                    row = row + 1;

                }
                sht.Range("D" + row).SetValue("Grand Total");
                sht.Range("E" + row).FormulaA1 = "=SUM(E" + rowfrom + ":E" + (row - 1) + ")";
                sht.Range("F" + row).FormulaA1 = "=SUM(F" + rowfrom + ":F" + (row - 1) + ")";
                sht.Range("G" + row).FormulaA1 = "=SUM(G" + rowfrom + ":G" + (row - 1) + ")";                

                sht.Range("D" + row + ":G" + row).Style.Font.Bold = true;

                for (int k = Dynamiccolstart; k <= Dynamiccolend; k++)
                {
                    var itemname = sht.Cell(4, k).Value;

                    DataTable dtdistinctRawMaterialItemDetails4 = ds.Tables[0].DefaultView.ToTable(true, "IssueOrderId", "RawMaterialIssueDate", "RawMaterialItemName", "IssueQty","RawMaterialReceiveQty");
                    DataView dv4 = new DataView(dtdistinctRawMaterialItemDetails4);
                    DataSet dsRawMaterialitemdesc4 = new DataSet();
                    dsRawMaterialitemdesc4.Tables.Add(dv4.ToTable());

                    decimal GrandTotalIssueQty = 0;
                    decimal GrandTotalReciveQty = 0;
                    decimal GrandTotalBalanceQty = 0;

                    GrandTotalIssueQty =Convert.ToDecimal(dsRawMaterialitemdesc4.Tables[0].Compute("sum(IssueQty)", "RawMaterialItemName='" + itemname + "' "));
                    GrandTotalReciveQty = Convert.ToDecimal(dsRawMaterialitemdesc4.Tables[0].Compute("sum(RawMaterialReceiveQty)", "RawMaterialItemName='" + itemname + "' "));

                    GrandTotalBalanceQty = GrandTotalIssueQty - GrandTotalReciveQty;

                    sht.Cell(row, k).Value = GrandTotalBalanceQty;

                    sht.Cell(row, k).Style.Font.Bold = true;
                }

                row = row + 1;

                ////*************
                sht.Columns(1, 20).AdjustToContents();
                //********************
                //***********BOrders
                using (var a = sht.Range("A1" + ":U" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("BazaarReceiveONLOOM_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }



    }
    protected void ChkBazaarReturnGatePassSummary_CheckedChanged(object sender, EventArgs e)
    {
        //if (ChkBazaarReturnGatePassSummary.Checked == true)
        //{
        //    trProductionStatus.Visible = false;
        //    trFolioType.Visible = false;
        //    Trproductiontype.Visible = false;
        //    TRCustomerCode.Visible = false;
        //    TROrderNo.Visible = false;
        //}
        //else
        //{
        //    trProductionStatus.Visible = true;
        //    trFolioType.Visible = true;
        //    Trproductiontype.Visible = true;
        //    TRCustomerCode.Visible = true;
        //    TROrderNo.Visible = true;
        //}
    }

    protected void WithTagNoTracking()
    {
        lblmsg.Text = "";
        try
        {
            string str = "", FilterBy = "";

            if (DDCategory.SelectedIndex > 0)
            {
                str = str + " and Vf.Category_id=" + DDCategory.SelectedValue;
                FilterBy = FilterBy + ", Category Name -" + DDCategory.SelectedItem.Text;
            }
            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and Vf.Item_id=" + DDQtype.SelectedValue;
                FilterBy = FilterBy + ", Item Name -" + DDQtype.SelectedItem.Text;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and Vf.Qualityid=" + DDQuality.SelectedValue;
                FilterBy = FilterBy + ", Quality -" + DDQuality.SelectedItem.Text;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
                FilterBy = FilterBy + ", Design -" + DDDesign.SelectedItem.Text;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " and vf.Colorid=" + DDColor.SelectedValue;
                FilterBy = FilterBy + ", Color -" + DDColor.SelectedItem.Text;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
                FilterBy = FilterBy + ", Size -" + DDSize.SelectedItem.Text;
            }
    
            if (ChkselectDate.Checked == true)
            {
                str = str + " and PRM.RECEIVEDATE>='" + txtfromDate.Text + "' and PRM.RECEIVEDATE<='" + txttodate.Text + "'";
                FilterBy = FilterBy + ", From -" + txtfromDate.Text + " To - " + txttodate.Text;
            }

            if (Session["varMasterCompanyIDForERP"].ToString() == "22")
            {
                if (ChkselectDate.Checked == false)
                {
                    //if (ChkForStockDetails.Checked == true)
                    //{
                    //    str = str + " and PRM.RECEIVEDATE>='01-Sep-2021' and PRM.RECEIVEDATE<='" + System.DateTime.Now.ToString("dd-MMM-yyyy") +"'";
                    //}
                }
            }


            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETBAZAARDETAILWITHTAGNOTRACKING", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Processid", 1);           
            cmd.Parameters.AddWithValue("@where", str);
            cmd.Parameters.AddWithValue("@TagNo","");            

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

           
            //SqlParameter[] param = new SqlParameter[4];
            //param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            //param[1] = new SqlParameter("@Processid", 1);
            //param[2] = new SqlParameter("@where", str);
            //param[3] = new SqlParameter("@TagNo", txtTagNo.Text);           

            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETBAZAARDETAILWITHTAGNOTRACKING", param);

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

                sht.Range("A1").Value = "DATE (FROM : " + txtfromDate.Text + " TO : " + txttodate.Text + ")";
                sht.Range("A1:N1").Style.Font.Bold = true;
                sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:N1").Merge();

                //Headers
                sht.Range("A3").Value = "EMP NAME";
                sht.Range("B3").Value = "STOCK NO";
                sht.Range("C3").Value = "FOLIO NO";
                sht.Range("D3").Value = "SHADE";
                sht.Range("E3").Value = "DESIGN";
                sht.Range("F3").Value = "COLOR";
                sht.Range("G3").Value = "SIZE";
                sht.Range("H3").Value = "AREA";
                sht.Range("I3").Value = "LOOM NO";
                sht.Range("J3").Value = "COTTON LOT NO";
                sht.Range("K3").Value = "DATE STAMP";
                sht.Range("L3").Value = "ULL NO";
                sht.Range("M3").Value = "SUPPLIER";
                sht.Range("N3").Value = "LOT NO";       

                //sht.Range("I1:R1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A3:N3").Style.Font.Bold = true;

                row = 4;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["TStockNo"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["IssueOrderID"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["SizeMtr"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["AreaMtr"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["LoomNo"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["CottonLotNo"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["JobIssueDateStamp"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["JobIssueULLNo"]);
                    sht.Range("L" + row).Style.NumberFormat.Format = "@";
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["SupplierName"]);
                    sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["TagNo"]); 

                    row = row + 1;

                }                
                sht.Range("I" + row + ":R" + row).Style.Font.Bold = true;
                sht.Columns(1, 20).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("WithTagNoTracking:"+"_" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm") + "." + Fileextension);
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

                #region
                //GridView GridView1 = new GridView();
                //GridView1.AllowPaging = false;

                //GridView1.DataSource = ds;
                //GridView1.DataBind();
                //Response.Clear();
                //Response.Buffer = true;
                //Response.AddHeader("content-disposition",
                // "attachment;filename=WithTagNoTracking" + DateTime.Now + ".xls");
                //Response.Charset = "";
                //Response.ContentType = "application/vnd.ms-excel";
                //StringWriter sw = new StringWriter();
                //HtmlTextWriter hw = new HtmlTextWriter(sw);

                //for (int i = 0; i < GridView1.Rows.Count; i++)
                //{
                //    //Apply text style to each Row
                //    GridView1.Rows[i].Attributes.Add("class", "textmode");
                //}
                //GridView1.RenderControl(hw);

               

                ////style to format numbers to string
                //string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                //Response.Write(style);
                //Response.Output.Write(sw.ToString());
                //Response.Flush();
                //Response.End();
                //////lblMessage.Text = "Done.....";
                ////*************

                #endregion

            }
            else
            {

                ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }

    protected void ChampoPNMAmtDifferenceReport()
    {
        try
        {
            //string str = "";           

            //if (DDWeaver.SelectedIndex > 0)
            //{
            //    str = str + " and EPN.Empid=" + DDWeaver.SelectedValue;
            //} 
            //if (ChkselectDate.Checked == true)
            //{
            //    str = str + " and PIM.AssignDate>='" + txtfromDate.Text + "' and PIM.AssignDate<='" + txttodate.Text + "'";
                
            //}
            //*****************
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@companyid", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@empid", 0);
            param[2] = new SqlParameter("@FromDate", txtfromDate.Text);
            param[3] = new SqlParameter("@ToDate", txttodate.Text);  
            param[4] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETCHAMPOPNMWEAVINGAMTDIFF", param);
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

                sht.Range("A1").Value = "DATE (FROM : " + txtfromDate.Text + " TO : " + txttodate.Text + ")";
                sht.Range("A1:N1").Style.Font.Bold = true;
                sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:N1").Merge();

                //Headers
                sht.Range("A3").Value = "FOLIO NO";
                sht.Range("B3").Value = "QUALITY NAME";
                sht.Range("C3").Value = "DESIGN NAME";
                sht.Range("D3").Value = "SIZE";
                sht.Range("E3").Value = "CHAMPO QTY";
                sht.Range("F3").Value = "CHAMPO AMT";
                sht.Range("G3").Value = "PNM QTY";
                sht.Range("H3").Value = "PNM AMT";
                sht.Range("I3").Value = "DIFFERENCE AMT";              

                //sht.Range("I1:R1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A3:I3").Style.Font.Bold = true;

                row = 4;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["IssueOrderId"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["Sizeft"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ChampoQty"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ChampoAmt"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["PNMQty"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["PNMAmt"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["DifferenceAmt"]);
                   
                    //sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["JobIssueULLNo"]);
                    //sht.Range("L" + row).Style.NumberFormat.Format = "@";                    
                    row = row + 1;

                }
                sht.Range("I" + row + ":R" + row).Style.Font.Bold = true;
                sht.Columns(1, 20).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("ChampoPNMAmtDiff:" + "_" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm") + "." + Fileextension);
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
                Response.End(); ;
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "RawBal", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void DepartmentRawConsumptionDetail()
    {
        lblmsg.Text = "";
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GET_DEPARTMENT_CONSUMPTIONREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            if (DDCustCode.SelectedIndex > 0)
            {
                cmd.Parameters.AddWithValue("@CustomerID", DDCustCode.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CustomerID", 0);
            }
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    cmd.Parameters.AddWithValue("@OrderID", DDOrderNo.SelectedValue);
            //}
            //else
            //{
            //    cmd.Parameters.AddWithValue("@OrderID", 0);
            //}
            cmd.Parameters.AddWithValue("@ChkSelectDate", ChkselectDate.Checked == true ? 1 : 0);
            cmd.Parameters.AddWithValue("@FromDate", txtfromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", txttodate.Text);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();
            //***********
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

                //*******Header
                sht.Range("A1").Value = "Department Issue No";
                sht.Range("B1").Value = "Issue Date";
                sht.Range("C1").Value = "Required Date";
                sht.Range("D1").Value = "Process Name";
                sht.Range("E1").Value = "Department Name";
                sht.Range("F1").Value = "Customer Code";

                sht.Range("G1").Value = "Order No";
                sht.Range("H1").Value = "Department Order Qty";
                sht.Range("I1").Value = "Raw Material Item Name";
                sht.Range("J1").Value = "Raw Material Quality Name";
                sht.Range("K1").Value = "Raw Material Shade Color ";
                sht.Range("L1").Value = "Consumption Qty";
                sht.Range("M1").Value = "Issue Qty";
                sht.Range("N1").Value = "Rec Qty";
                sht.Range("O1").Value = "Pending Qty";

                sht.Range("A1:O1").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:O1").Style.Font.FontSize = 9;
                sht.Range("A1:O1").Style.Font.Bold = true;
                //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 2;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["IssueNo"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["IssueDate"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["RequiredDate"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ProcessName"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["DepartmentName"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["DepartmentOrderQty"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["ItemName"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["ConsmpQty"]);
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["ISSQTY"]);
                    sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["RECQTY"]);
                    sht.Range("O" + row).SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["ConsmpQty"]) - Convert.ToDouble(ds.Tables[0].Rows[i]["ISSQTY"]));

                    row = row + 1;

                }

                //*************
                sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("DepartmentConsumptionreport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void ChampoExternalWeaverConsumptionDetail()
    {
        lblmsg.Text = "";
        try
        {
            string str = "";
            if (DDUnitname.SelectedIndex > 0)
            {
                str = str + " and PIM.Units=" + DDUnitname.SelectedValue;
            }
            //if (DDProductionstatus.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Status='" + DDProductionstatus.SelectedItem.Text + "'";
            //}
            //if (DDWeaver.SelectedIndex > 0 && DDproductiontype.SelectedValue == "0")
            //{
            //    str = str + " and PIM.Empid=" + DDWeaver.SelectedValue;
            //}
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Issueorderid=" + DDFolioNo.SelectedValue;
            //}

            //if (ChkselectDate.Checked == true)
            //{
            //    str = str + " and Assigndate>='" + txtfromDate.Text + "' and AssignDate<='" + txttodate.Text + "'";
            //}
            if (DDCustCode.SelectedIndex > 0)
            {
                str = str + " and OM.Customerid=" + DDCustCode.SelectedValue;
            }
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
            //}  
            //if (DDproductiontype.SelectedIndex > 0)
            //{
            //    if (DDproductiontype.SelectedValue == "0")
            //    {
            //        str = str + " and PIM.EMPID>0";
            //    }
            //    if (DDproductiontype.SelectedValue == "1")
            //    {
            //        str = str + " and PIM.EMPID=0";
            //    }
            //}
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GET_OUTSIDE_WEAVER_CONSUMPTIONREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@empid", "0");
            cmd.Parameters.AddWithValue("@ChkSelectDate", ChkselectDate.Checked==true ? 1 : 0);
            cmd.Parameters.AddWithValue("@FromDate", txtfromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", txttodate.Text);
            cmd.Parameters.AddWithValue("@ProductionType", 0);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();
            //***********
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

                //*******Header
                sht.Range("A1").Value = "PRODUCTION STATUS";
                sht.Range("B1").Value = "BUYER CODE";
                sht.Range("C1").Value = "CUSTOMER ORDERNO";
                sht.Range("D1").Value = "ORDER DATE";
                sht.Range("E1").Value = "DELIVERY DATE";

                sht.Range("F1").Value = "FOLIO NO";
                sht.Range("G1").Value = "CONTRACTOR NAME";               
                sht.Range("H1").Value = "RAWMATERIAL QUALITY";
                sht.Range("I1").Value = "SHADE COLOR";
                sht.Range("J1").Value = "CONSUMPTION QTY";
                sht.Range("K1").Value = "ISSUED QTY";
                sht.Range("L1").Value = "RECEIVE QTY";
                sht.Range("M1").Value = "ISSUE/RECEIVE BALANCE";
                sht.Range("N1").Value = "PENDING QTY";               


                sht.Range("A1:N1").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:N1").Style.Font.FontSize = 9;
                sht.Range("A1:N1").Style.Font.Bold = true;
                //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 2;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Status"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["OrderDate"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["DispatchDate"]);



                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["IssueOrderId"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);                   
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["RawQuality"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["ConsmpQty"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["WeaverIssueQty"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["ReceivedQty"]);
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["IssuedQty"]);
                    sht.Range("N" + row).SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["ConsmpQty"]) -Convert.ToDouble( ds.Tables[0].Rows[i]["IssuedQty"]));

                    row = row + 1;
                   
                }            
                
                
                //*************
                sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("OutsideWeaverConsumptionreport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }

    protected void InternalfolioDetailDiamondExport()
    {
        lblmsg.Text = "";
        try
        {
            string str = "", FilterBy = "";
            //if (DDProductionstatus.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Status='" + DDProductionstatus.SelectedItem.Text + "'";
            //    FilterBy = FilterBy + ", Production Status -" + DDProductionstatus.SelectedItem.Text;
            //    if (DDProductionstatus.SelectedIndex == 1)
            //    {
            //        str = str + " and PID.Pqty>0";
            //    }
            //}
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Issueorderid=" + DDFolioNo.SelectedValue;
            //    FilterBy = FilterBy + ", Folio No. -" + DDFolioNo.SelectedItem.Text;
            //}
            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and Vf.Item_id=" + DDQtype.SelectedValue;
                FilterBy = FilterBy + ", Item Name -" + DDQtype.SelectedItem.Text;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and Vf.Qualityid=" + DDQuality.SelectedValue;
                FilterBy = FilterBy + ", Quality -" + DDQuality.SelectedItem.Text;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
                FilterBy = FilterBy + ", Design -" + DDDesign.SelectedItem.Text;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " and vf.Colorid=" + DDColor.SelectedValue;
                FilterBy = FilterBy + ", Color -" + DDColor.SelectedItem.Text;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
                FilterBy = FilterBy + ", Size -" + DDSize.SelectedItem.Text;
            }
            if (ChkselectDate.Checked == true)
            {
                str = str + " and PIM.Assigndate>='" + txtfromDate.Text + "' and PIM.AssignDate<='" + txttodate.Text + "'";
                FilterBy = FilterBy + ", From -" + txtfromDate.Text + " To - " + txttodate.Text;
            }
            //if (DDFoliotype.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Purchasefolio=" + DDFoliotype.SelectedValue;
            //    FilterBy = FilterBy + ", Folio Type -" + DDFoliotype.SelectedItem.Text;
            //}
            if (DDCustCode.SelectedIndex > 0)
            {
                str = str + " and OM.Customerid=" + DDCustCode.SelectedValue;
                FilterBy = FilterBy + ", Customer code -" + DDCustCode.SelectedItem.Text;
            }
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
            //    FilterBy = FilterBy + ", Order No. -" + DDOrderNo.SelectedItem.Text;
            //}

            //SqlParameter[] param = new SqlParameter[4];
            //param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            //param[1] = new SqlParameter("@Processid", 1);
            //param[2] = new SqlParameter("@where", str);
            //param[3] = new SqlParameter("@EMpid", (DDWeaver.SelectedIndex > 0 ? DDWeaver.SelectedValue : "0"));

            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETINTERNALFOLIODETAILDIAMONDEXPORT", param);


            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETINTERNALFOLIODETAILDIAMONDEXPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 500;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Processid", 1);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@EMpid", "0");


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

                sht.Range("A1:T1").Merge();
                sht.Range("A1").Value = "INTERNAL FOLIO DETAILS";
                sht.Range("A2:T2").Merge();
                sht.Range("A2").Value = "Filter By :  " + FilterBy;
                sht.Row(2).Height = 30;
                sht.Range("A1:T1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:T2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:T2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:T2").Style.Alignment.SetWrapText();
                sht.Range("A1:T2").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:T2").Style.Font.FontSize = 10;
                sht.Range("A1:T2").Style.Font.Bold = true;
                //*******Header
                sht.Range("A3").Value = "Customer code";
                sht.Range("B3").Value = "Order No.";
                sht.Range("C3").Value = "Folio No.";
                sht.Range("D3").Value = "Assign Date";
                sht.Range("E3").Value = "Reqd Date";
                sht.Range("F3").Value = "Loom No.";
                sht.Range("G3").Value = "Emp. Code";

                sht.Range("H3").Value = "Emp. Name";
                sht.Range("I3").Value = "Emp. Type";
                sht.Range("J3").Value = "Quality Name";
                sht.Range("K3").Value = "Design Name";
                sht.Range("L3").Value = "Color Name";
                sht.Range("M3").Value = "Size";
                sht.Range("N3").Value = "WOQty.";
                sht.Range("O3").Value = "Rec. Qty";
                sht.Range("P3").Value = "Balance Qty";
                sht.Range("Q3").Value = "Pend.Stock No.";
                sht.Range("R3").Value = "Total BOM WT";
                sht.Range("S3").Value = "Wool Issued";
                sht.Range("T3").Value = "Balance";
                sht.Range("U3").Value = "Folio StockNo";
                sht.Range("V3").Value = "Cotton LotNo";

                sht.Range("A3:V3").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A3:V3").Style.Font.FontSize = 9;
                sht.Range("A3:V3").Style.Font.Bold = true;
                sht.Range("N3:P3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 4;
                int rowfrom = 4;
                DataView dv = new DataView(ds.Tables[0]);
                dv.Sort = "Issueorderid";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());

                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":O" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":O" + row).Style.Font.FontSize = 9;

                    sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["customercode"]);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["customerorderno"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Issueorderid"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Assigndate"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Reqbydate"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Loomno"]);
                    sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["empcode"]);

                    sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["empname"]);
                    sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["emptype"]);
                    sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                    sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["Designname"]);
                    sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["colorname"]);
                    sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["size"]);
                    sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["Qty"]);
                    sht.Range("O" + row).SetValue(Convert.ToDecimal(ds1.Tables[0].Rows[i]["Qty"]) - Convert.ToDecimal(ds1.Tables[0].Rows[i]["PQty"]));
                    sht.Range("P" + row).FormulaA1 = "=N" + row + '-' + ("$O$" + row + "");
                    sht.Range("Q" + row).SetValue(ds1.Tables[0].Rows[i]["Pendstockno"]);
                    sht.Range("R" + row).SetValue(ds1.Tables[0].Rows[i]["CONSMPQTY"]);
                    sht.Range("S" + row).SetValue(ds1.Tables[0].Rows[i]["ISSUEQTY"]);
                    sht.Range("T" + row).SetValue(Convert.ToDecimal(ds1.Tables[0].Rows[i]["CONSMPQTY"]) - Convert.ToDecimal(ds1.Tables[0].Rows[i]["ISSUEQTY"]));
                    sht.Range("U" + row).SetValue(ds1.Tables[0].Rows[i]["FolioStockNo"]);
                    sht.Range("V" + row).SetValue(ds1.Tables[0].Rows[i]["TanaLotNo"]);
                    row = row + 1;
                }
                //GRAND TOAL
                //=SUM(L4:L929)
                sht.Range("N" + row).FormulaA1 = "=SUM(N" + rowfrom + ":N" + (row - 1) + ")";
                sht.Range("O" + row).FormulaA1 = "=SUM(O" + rowfrom + ":O" + (row - 1) + ")";
                sht.Range("P" + row).FormulaA1 = "=SUM(P" + rowfrom + ":P" + (row - 1) + ")";
                sht.Range("N" + row + ":P" + row).Style.Font.FontName = "Arial Unicode MS";
                sht.Range("N" + row + ":P" + row).Style.Font.FontSize = 9;
                sht.Range("N" + row + ":P" + row).Style.Font.Bold = true;

                //*************
                sht.Columns(1, 23).AdjustToContents();
                //********************
                //***********BOrders
                using (var a = sht.Range("A1" + ":V" + row))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("InternalFolioDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void WeavingOrderRecBalWithAmountDetail()
    {
        lblmsg.Text = "";
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GET_WeavingOrderRecBalWithAmountDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("CustomerID", DDCustCode.SelectedIndex > 0 ? DDCustCode.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@OrderID","0");
            cmd.Parameters.AddWithValue("@EmpID", "0");
            cmd.Parameters.AddWithValue("@FolioNo", "0");
            cmd.Parameters.AddWithValue("@ChkSelectDate", ChkselectDate.Checked == true ? 1 : 0);
            cmd.Parameters.AddWithValue("@FromDate", txtfromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", txttodate.Text);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();
            //***********
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

                sht.Range("A1").Value = ds.Tables[0].Rows[0]["CompanyName"];
                sht.Range("A1:P1").Style.Font.Bold = true;
                sht.Range("A1:P1").Style.Font.FontSize = 15;
                sht.Range("A1:P1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:P1").Merge();

                string RawTwo = "";
                //if (DDWeaver .SelectedIndex > 0)
                //{
                //    RawTwo = RawTwo + DDWeaver.SelectedItem.Text;
                //}
                RawTwo = RawTwo + " BALANCE ORDER DETAILS FROM DATE : " + txtfromDate.Text + " TO DATE : " + txttodate.Text;

                sht.Range("A2").Value = RawTwo;
                sht.Range("A2:P2").Style.Font.Bold = true;
                sht.Range("A2:P2").Style.Font.FontSize = 12;
                sht.Range("A2:P2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:P2").Merge();

                //*******Header
                sht.Range("A3").Value = "BUYER CODE";
                sht.Range("B3").Value = "CUSTOMER ORDERNO";
                sht.Range("C3").Value = "FOLIO NO";
                sht.Range("D3").Value = "JOB TYPE";
                sht.Range("E3").Value = "ITEM NAME";
                sht.Range("F3").Value = "QUALITY";
                sht.Range("G3").Value = "DESIGN";
                sht.Range("H3").Value = "COLOR";
                sht.Range("I3").Value = "SIZE";
                sht.Range("J3").Value = "ORDER QTY";
                sht.Range("K3").Value = "REC QTY";
                sht.Range("L3").Value = "BAL QTY";
                sht.Range("M3").Value = "BAL AREA";
                sht.Range("N3").Value = "RATE";
                sht.Range("O3").Value = "WEAVING RATE";
                sht.Range("P3").Value = "BAL AMT";

                sht.Range("A3:P3").Style.Font.FontName = "Calibri";
                sht.Range("A3:P3").Style.Font.FontSize = 9;
                sht.Range("A3:P3").Style.Font.Bold = true;

                //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 4;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["FolioNo"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["JobType"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ItemName"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Size"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["OrderQty"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["RecQty"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["BalQty"]);
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["BalArea"]);
                    sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                    sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["BalAmt"]);
                    row = row + 1;
                }


                //*************
                sht.Columns(1, 20).AdjustToContents();

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "P")))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("WeavingOrderRecBalWithAmountDetail_" + DateTime.Now + "." + Fileextension);
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
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }

    protected void FolioWiseConsumptionReport()
    {
        lblmsg.Text = "";
        try
        {
            string str = "", FilterBy = "";
            //if (DDProductionstatus.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Status='" + DDProductionstatus.SelectedItem.Text + "'";
            //    FilterBy = FilterBy + ", Production Status -" + DDProductionstatus.SelectedItem.Text;

            //}
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Issueorderid=" + DDFolioNo.SelectedValue;
            //    FilterBy = FilterBy + ", Folio No. -" + DDFolioNo.SelectedItem.Text;
            //}
            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and Vf.Item_id=" + DDQtype.SelectedValue;
                FilterBy = FilterBy + ", Item Name -" + DDQtype.SelectedItem.Text;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and Vf.Qualityid=" + DDQuality.SelectedValue;
                FilterBy = FilterBy + ", Quality -" + DDQuality.SelectedItem.Text;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
                FilterBy = FilterBy + ", Design -" + DDDesign.SelectedItem.Text;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " and vf.Colorid=" + DDColor.SelectedValue;
                FilterBy = FilterBy + ", Color -" + DDColor.SelectedItem.Text;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
                FilterBy = FilterBy + ", Size -" + DDSize.SelectedItem.Text;
            }
            //if (ChkselectDate.Checked == true)
            //{
            str = str + " and PIM.AssignDate>='" + txtfromDate.Text + "' and PIM.AssignDate<='" + txttodate.Text + "'";
            FilterBy = FilterBy + ", From -" + txtfromDate.Text + " To - " + txttodate.Text;
            //}
            //if (DDFoliotype.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Purchaseflag=" + DDFoliotype.SelectedValue;
            //    FilterBy = FilterBy + ", Folio Type -" + DDFoliotype.SelectedItem.Text;
            //}
            if (DDCustCode.SelectedIndex > 0)
            {
                str = str + " and OM.Customerid=" + DDCustCode.SelectedValue;
                FilterBy = FilterBy + ", Customer code -" + DDCustCode.SelectedItem.Text;
            }
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
            //    FilterBy = FilterBy + ", Order No. -" + DDOrderNo.SelectedItem.Text;
            //}
            //if (DDorderstatus.SelectedIndex == 1)
            //{
            //    str = str + " and PIM.Status='Pending'";
            //    FilterBy = FilterBy + ",Order Status. -'Pending'";
            //}
            //else if (DDorderstatus.SelectedIndex == 2)
            //{
            //    str = str + " and PIM.Status='" + DDorderstatus.SelectedItem.Text + "'";
            //    FilterBy = FilterBy + ",Order Status. -'" + DDorderstatus.SelectedItem.Text + "'";
            //}
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetFolioWiseConsumptionReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@empid",  "0");

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();
            //***********
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

                //*******Header
                sht.Range("A1").Value = "Production Status";
                sht.Range("B1").Value = "BuyerCode";
                sht.Range("C1").Value = "Customer OrderNo";
                sht.Range("D1").Value = "Order Date";
                sht.Range("E1").Value = "Dispatch Date";
                sht.Range("F1").Value = "Folio No";
                sht.Range("G1").Value = "Contractor Name";
                sht.Range("H1").Value = "Item Name";
                sht.Range("I1").Value = "Quality";
                sht.Range("J1").Value = "Design";
                sht.Range("K1").Value = "Color";
                sht.Range("L1").Value = "Size";
                sht.Range("M1").Value = "Pcs";
                sht.Range("N1").Value = "Area";
                sht.Range("O1").Value = "Raw Material Quality";
                sht.Range("P1").Value = "Shade Color";
                sht.Range("Q1").Value = "Consumption Qty";
                //sht.Range("R1").Value = "Issue Qty";


                sht.Range("A1:R1").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:R1").Style.Font.FontSize = 10;
                sht.Range("A1:R1").Style.Font.Bold = true;
                //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 2;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":Q" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":Q" + row).Style.Font.FontSize = 9;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Status"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["OrderDate"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["DispatchDate"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["CHALLANNO"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Width"].ToString() + 'x' + ds.Tables[0].Rows[i]["Length"].ToString());
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["QTY"]);
                    sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["TOTALAREA"]);
                    sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["RawMaterialItem"].ToString() + ' ' + ds.Tables[0].Rows[i]["RawMaterialQuality"]);
                    sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["RawMaterialShadeColor"]);
                    sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["ConsumptionQTY"]);
                    //sht.Range("R" + row).SetValue(ds.Tables[0].Rows[i]["IssuedQty"]);  

                    row = row + 1;

                }  
              
                //*************
                sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("FolioWiseConsumptionreport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void WeaverRawMaterialIssueReport()
    {
        lblmsg.Text = "";
        try
        {
            string str = "", FilterBy = "", str2 = "";            
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Issueorderid=" + DDFolioNo.SelectedValue;
            //    FilterBy = FilterBy + ", Folio No. -" + DDFolioNo.SelectedItem.Text;
            //}
            if (DDCustCode.SelectedIndex > 0)
            {
                str = str + " and OM.Customerid=" + DDCustCode.SelectedValue;
                FilterBy = FilterBy + ", Customer code -" + DDCustCode.SelectedItem.Text;
            }
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
            //    FilterBy = FilterBy + ", Order No. -" + DDOrderNo.SelectedItem.Text;
            //}

            if (DDQtype.SelectedIndex > 0)
            {
                str2 = str2 + " and Vf.Item_id=" + DDQtype.SelectedValue;                
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str2 = str2 + " and Vf.Qualityid=" + DDQuality.SelectedValue;                
            }
            //if (DDshade.SelectedIndex > 0)
            //{
            //    str2 = str2 + " and vf.Shadecolorid=" + DDshade.SelectedValue;
            //}         
            //if (ChkselectDate.Checked == true)
            //{
            str2 = str2 + " and PM.Date>='" + txtfromDate.Text + "' and PM.Date<='" + txttodate.Text + "'";
            FilterBy = FilterBy + ", From -" + txtfromDate.Text + " To - " + txttodate.Text;
            //}
          
          
           
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetWeaverRawMaterialIssueReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@Where2", str2);
            cmd.Parameters.AddWithValue("@empid", "0");

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();
            //***********
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

                //*******Header
                sht.Range("A1").Value = "Issue ChallanDate";
                sht.Range("B1").Value = "Issue ChallanNo";
                sht.Range("C1").Value = "Buyer Code";
                sht.Range("D1").Value = "Buyer OrderNo";
                sht.Range("E1").Value = "Folio No";
                sht.Range("F1").Value = "Party Name";
                sht.Range("G1").Value = "Yarn Quality";
                sht.Range("H1").Value = "Shade No";
                sht.Range("I1").Value = "Physical Formof Yarn";
                sht.Range("J1").Value = "Figure as Weight Machine";
                sht.Range("K1").Value = "No of cone/Bobin";
                sht.Range("L1").Value = "Bell Wt";
                sht.Range("M1").Value = "Issued Qty";              


                sht.Range("A1:M1").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:M1").Style.Font.FontSize = 10;
                sht.Range("A1:M1").Style.Font.Bold = true;
                //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 2;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":Q" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":Q" + row).Style.Font.FontSize = 9;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Date"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["ChalanNo"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ProrderId"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["CONETYPE"]);
                    sht.Range("J" + row).SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["ISSUEQTY"])+Convert.ToDouble(ds.Tables[0].Rows[i]["BellWt"]));
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["NOOFCONE"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["BellWt"].ToString());
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["ISSUEQTY"]);                  

                    row = row + 1;

                }

                //*************
                sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("WeaverRawMaterialIssuereport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void DesignWiseFolioMaterialIssueStatus()
    {
        lblmsg.Text = "";
        try
        {
            string str = "", FilterBy = "", str2 = "", FromDate = "", ToDate = "";
            int CustomerID = 0, OrderID = 0, EmpID = 0, IssueOrderID = 0, DateFlag = 0;

            if (DDCustCode.SelectedIndex > 0)
            {
                CustomerID = Convert.ToInt32(DDCustCode.SelectedValue);
                FilterBy = FilterBy + ", Customer code -" + DDCustCode.SelectedItem.Text;
            }
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    OrderID = Convert.ToInt32(DDOrderNo.SelectedValue);
            //    FilterBy = FilterBy + ", Order No. -" + DDOrderNo.SelectedItem.Text;
            //}
            //if (DDWeaver.SelectedIndex > 0)
            //{
            //    EmpID = Convert.ToInt32(DDWeaver.SelectedValue);
            //    FilterBy = FilterBy + ", Folio No. -" + DDFolioNo.SelectedItem.Text;
            //}
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    IssueOrderID = Convert.ToInt32(DDFolioNo.SelectedValue);
            //    FilterBy = FilterBy + ", Folio No. -" + DDFolioNo.SelectedItem.Text;
            //}
            if (ChkselectDate.Checked == true)
            {
                DateFlag = 1;
                FromDate = txtfromDate.Text;
                ToDate = txttodate.Text;
                FilterBy = FilterBy + ", From -" + txtfromDate.Text + " To - " + txttodate.Text;
            }

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetDesignWiseFolioMaterialIssueStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@OrderID", OrderID);
            cmd.Parameters.AddWithValue("@EmpID", EmpID);
            cmd.Parameters.AddWithValue("@IssueOrderID",IssueOrderID );
            cmd.Parameters.AddWithValue("@DateFlag", DateFlag);
            cmd.Parameters.AddWithValue("@FromDate", FromDate);
            cmd.Parameters.AddWithValue("@ToDate", ToDate);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            con.Close();
            con.Dispose();
            //***********
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

                //*******Header
                sht.Range("A1").Value = "Party Name";
                sht.Range("B1").Value = "Buyer Code";
                sht.Range("C1").Value = "Order No";
                sht.Range("D1").Value = "Shipment Date";
                sht.Range("E1").Value = "FolioNo";                
                sht.Range("F1").Value = "Design Name";
                sht.Range("G1").Value = "Status";

                sht.Range("A1:G1").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:G1").Style.Font.FontSize = 10;
                sht.Range("A1:G1").Style.Font.Bold = true;
                row = 2;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":G" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":G" + row).Style.Font.FontSize = 9;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["DispatchDate"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["IssueOrderID"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Status"]);
                    row = row + 1;
                }

                //*************
                sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("DesignWiseFolioMaterialIssueStatus_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }

    protected void InternalBazaarDetailFolioWiseChampoPanipat2()
    {
        lblmsg.Text = "";
        try
        {
            string str = "", FilterBy = "";
                       
           
            //if (DDQtype.SelectedIndex > 0)
            //{
            //    str = str + " and Vf.Item_id=" + DDQtype.SelectedValue;
            //    FilterBy = FilterBy + ", Item Name -" + DDQtype.SelectedItem.Text;
            //}
            //if (DDQuality.SelectedIndex > 0)
            //{
            //    str = str + " and Vf.Qualityid=" + DDQuality.SelectedValue;
            //    FilterBy = FilterBy + ", Quality -" + DDQuality.SelectedItem.Text;
            //}
            //if (DDDesign.SelectedIndex > 0)
            //{
            //    str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
            //    FilterBy = FilterBy + ", Design -" + DDDesign.SelectedItem.Text;
            //}
            //if (DDColor.SelectedIndex > 0)
            //{
            //    str = str + " and vf.Colorid=" + DDColor.SelectedValue;
            //    FilterBy = FilterBy + ", Color -" + DDColor.SelectedItem.Text;
            //}
            //if (DDSize.SelectedIndex > 0)
            //{
            //    str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
            //    FilterBy = FilterBy + ", Size -" + DDSize.SelectedItem.Text;
            //}
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and PRD.Issueorderid=" + DDFolioNo.SelectedValue;
            //    FilterBy = FilterBy + ", Folio No. -" + DDFolioNo.SelectedItem.Text;
            //}
            if (ChkselectDate.Checked == true)
            {
                str = str + " and PRM.RECEIVEDATE>='" + txtfromDate.Text + "' and PRM.RECEIVEDATE<='" + txttodate.Text + "'";
                FilterBy = FilterBy + ", From -" + txtfromDate.Text + " To - " + txttodate.Text;
            }
           
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@Processid", 1);
            param[2] = new SqlParameter("@where", str);
            param[3] = new SqlParameter("@EMpid", "0");

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETINTERNALBAZAARDETAIL_CHAMPOPANIPAT", param);

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
                sht.Range("A1").Value = "BAZAR DETAIL";
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
                sht.Range("A3").Value = "RECEIVE DATE";
                sht.Range("B3").Value = "REC CHALLANNO";
                sht.Range("C3").Value = "FOLIO NO";
                sht.Range("D3").Value = "EMP NAME";
                sht.Range("E3").Value = "ITEM";
                sht.Range("F3").Value = "QUALITY";
                sht.Range("G3").Value = "DESIGN";
                sht.Range("H3").Value = "COLOR";
                sht.Range("I3").Value = "SIZE";
                sht.Range("J3").Value = "RECEIVE QTY";

                sht.Range("A3:J3").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A3:J3").Style.Font.FontSize = 9;
                sht.Range("A3:J3").Style.Font.Bold = true;
                sht.Range("S3:J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A3:J3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A3:J3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A3:J3").Style.Alignment.SetWrapText();

                row = 4;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":J" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":J" + row).Style.Font.FontSize = 9;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["RECEIVEDATE"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CHALLANNO"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ISSUEORDERID"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["DESIGNNAME"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["COLORNAME"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["SIZE"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["RecQTY"]);
                    row = row + 1;

                }
                //*************
                sht.Columns(1, 26).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("TotalBazaarDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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

                ScriptManager.RegisterStartupScript(Page, GetType(), "Intalt", "alert('No records found for this combination.')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }

    protected void WeavingReceiveWithTDS()
    {
        lblmsg.Text = "";
        try
        {
            string str = "", FilterBy = "";  

            if (DDUnitname.SelectedIndex > 0)
            {
                str = str + "  and PRM.UNITID=" + DDUnitname.SelectedValue;
            }
            //if (DDWeaver.SelectedIndex > 0)
            //{
            //    str = str + " and EI.EMPID=" + DDWeaver.SelectedValue;
            //}
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and PRD.ISSUEORDERID=" + DDFolioNo.SelectedValue;
            //}
            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and VF.ITEM_ID=" + DDQtype.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and VF.QUALITYID=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and VF.DESIGNID=" + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " and VF.COLORID=" + DDColor.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and VF.SIZEID=" + DDSize.SelectedValue;
            }
            if (ChkselectDate.Checked == true)
            {
                str = str + " and PRM.RECEIVEDATE>='" + txtfromDate.Text + "' and PRM.RECEIVEDATE<='" + txttodate.Text + "'";               
                FilterBy = FilterBy + ",Date From -" + txtfromDate.Text + " To - " + txttodate.Text;
               
            }
            //if (DDFoliotype.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.PURCHASEFLAG=" + DDFoliotype.SelectedValue;
            //}
            //if (DDCustCode.SelectedIndex > 0)
            //{
            //    str = str + " and OM.CUSTOMERID=" + DDCustCode.SelectedValue;
            //}
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    str = str + " and OM.ORDERID=" + DDOrderNo.SelectedValue;
            //}
            //if (DDproductiontype.SelectedIndex > 0)
            //{
            //    str = str + " and PRODUCTIONTYPE=" + DDproductiontype.SelectedValue;
            //}


            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETWEAVINGRECEIVEWITHTDSREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", str);


            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            //*************
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            con.Close();
            con.Dispose();
            //***********
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                //sht.PageSetup.AdjustTo(90);
                sht.PageSetup.FitToPages(1, 1);
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                //sht.PageSetup.VerticalDpi = 300;
                //sht.PageSetup.HorizontalDpi = 300;
                sht.PageSetup.Margins.Top = 0.2;
                sht.PageSetup.Margins.Bottom = 0.2;
                sht.PageSetup.Margins.Right = 0.2;
                sht.PageSetup.Margins.Left = 0.2;
                sht.PageSetup.Margins.Header = 0.2;
                sht.PageSetup.Margins.Footer = 0.2;
                //sht.Style.Font.FontName = "Cambria";
                sht.PageSetup.SetScaleHFWithDocument();
                sht.PageSetup.CenterHorizontally = true;

                sht.Column("A").Width = 10.67;
                sht.Column("B").Width = 19.67;
                sht.Column("C").Width = 19.67;
                sht.Column("D").Width = 19.67;
                sht.Column("E").Width = 19.67;
                sht.Column("F").Width = 19.67;
                sht.Column("G").Width = 12.67;
                sht.Column("H").Width = 12.67;
                sht.Column("I").Width = 12.67;
                sht.Column("J").Width = 12.67;
                sht.Column("K").Width = 12.67;
                sht.Column("L").Width = 12.67;
                sht.Column("M").Width = 19.67;
                sht.Column("N").Width = 19.67;
                sht.Column("O").Width = 12.67;

                sht.Row(2).Height = 34;

                ////sht.Cell("A3").Value = "INVOICE #" + ds.Tables[0].Rows[0]["TINVOICENO"] + " DATED " + ds.Tables[0].Rows[0]["INVOICEDATE"];
                sht.Cell("A1").Value = "GSTIN NO" + " " + ds.Tables[0].Rows[0]["GSTNo"].ToString().ToUpper();
                sht.Range("A1:C1").Style.Font.FontName = "Calibri";
                sht.Range("A1:C1").Style.Font.FontSize = 12;
                sht.Range("A1:C1").Style.Font.Bold = true;
                sht.Range("A1:C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A1:C1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A1:C1").Merge();

                sht.Cell("A2").Value = ds.Tables[0].Rows[0]["COMPANYNAME"].ToString().ToUpper();
                sht.Range("A2:C2").Style.Font.FontName = "Calibri";
                sht.Range("A2:C2").Style.Font.FontSize = 22;
                sht.Range("A2:C2").Style.Font.Bold = true;
                sht.Range("A2:C2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A2:C2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:C2").Merge();

                sht.Cell("A3").Value = " " + ds.Tables[0].Rows[0]["CompAddr1"].ToString();
                sht.Range("A3:C4").Style.Font.FontName = "Calibri";
                sht.Range("A3:C4").Style.Font.FontSize = 11;
                sht.Range("A3:C4").Style.Font.Bold = true;
                sht.Range("A3:C4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A3:C4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A3:C4").Merge();
                sht.Range("A3:C4").Style.Alignment.SetWrapText();

                sht.Cell("D1").Value = "WEAVING RECEIVE WITH TDS REPORT";
                sht.Range("D1:K3").Style.Font.FontName = "Calibri";
                sht.Range("D1:K3").Style.Font.FontSize = 22;
                sht.Range("D1:K3").Style.Font.Bold = true;
                sht.Range("D1:K3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("D1:K3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("D1:K3").Merge();

                sht.Cell("D4").Value = "Filter By :  " + FilterBy;
                sht.Range("D4:K4").Style.Font.FontName = "Calibri";
                sht.Range("D4:K4").Style.Font.FontSize = 22;
                sht.Range("D4:K4").Style.Font.Bold = true;
                sht.Range("D4:K4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("D4:K4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("D4:K4").Merge();

                sht.Cell("L1").Value = "PRINT DATE:" + " " + DateTime.Now.ToString();
                sht.Range("L1:O4").Style.Font.FontName = "Calibri";
                sht.Range("L1:O4").Style.Font.FontSize = 11;
                sht.Range("L1:O4").Style.Font.Bold = true;
                sht.Range("L1:O4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("L1:O4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("L1:O4").Merge();

                using (var a = sht.Range("A1:O4"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                int row = 0;

                //sht.Range("A1:N1").Merge();
                //sht.Range("A1").Value = "WEAVING RECEIVE WITH TDS";
                //sht.Range("A2:N2").Merge();
                //sht.Range("A2").Value = "Filter By :  " + FilterBy;
                //sht.Row(2).Height = 30;
                //sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Range("A2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Range("A2:N2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                //sht.Range("A2:N2").Style.Alignment.SetWrapText();
                //sht.Range("A1:N2").Style.Font.FontName = "Arial Unicode MS";
                //sht.Range("A1:N2").Style.Font.FontSize = 10;
                //sht.Range("A1:N2").Style.Font.Bold = true;



                //*******Header
                sht.Range("A5").Value = "SR.NO.";
                sht.Range("B5").Value = "NAME & ADDRESS OF BUNKAR";
                sht.Range("C5").Value = "PAN NO";
                sht.Range("D5").Value = "RECEIVE DATE";
                
                sht.Range("E5").Value = "QUALITY";
                sht.Range("F5").Value = "DESIGN";
                sht.Range("G5").Value = "COLOR";
                sht.Range("H5").Value = "SIZE";
                sht.Range("I5").Value = "NOOF PIECES";

                sht.Range("J5").Value = "WEIGHT (Kg)";
                sht.Range("K5").Value = "RATE";
                sht.Range("L5").Value = "ACTUAL WEAVING";
                sht.Range("M5").Value = "TDS 1%";
                sht.Range("N5").Value = "NET WEAVING";
                sht.Range("O5").Value = "OTHER";

                //sht.Range("D5").Value = "QUALITY";
                //sht.Range("E5").Value = "DESIGN";
                //sht.Range("F5").Value = "COLOR";
                //sht.Range("G5").Value = "SIZE";
                //sht.Range("H5").Value = "NOOF PIECES";

                //sht.Range("I5").Value = "WEIGHT (Kg)";
                //sht.Range("J5").Value = "RATE";
                //sht.Range("K5").Value = "ACTUAL WEAVING";
                //sht.Range("L5").Value = "TDS 1%";
                //sht.Range("M5").Value = "NET WEAVING";
                //sht.Range("N5").Value = "OTHER"; 


                sht.Range("A5:O5").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A3:O5").Style.Font.FontSize = 9;
                sht.Range("A3:O5").Style.Font.Bold = true;
                sht.Range("A3:O5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A3:O5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A3:O5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A3:O5").Style.Alignment.SetWrapText();

                using (var a = sht.Range("A5:O5"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }


                //DataView dv = new DataView(ds.Tables[0]);
                //dv.Sort = "FOLIONO";
                //DataSet ds1 = new DataSet();
                //ds1.Tables.Add(dv.ToTable());

                row = 6;
                int Srno = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {                    

                    sht.Range("A" + row + ":O" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":O" + row).Style.Font.FontSize = 9;
                    //sht.Range("A" + row + ":L" + row).Style.Font.SetBold();
                    sht.Range("A" + row + ":O" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    //sht.Range("A" + row + ":L" + row).Style.Alignment.SetWrapText();
                    sht.Range("A" + row + ":O" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    Srno = Srno + 1;

                    sht.Range("A" + row).SetValue(Srno);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"] + " " + ds.Tables[0].Rows[i]["Address"] + " " + ds.Tables[0].Rows[i]["Address2"] + " " + ds.Tables[0].Rows[i]["Address3"]);
                    sht.Range("B" + row).Style.Alignment.SetWrapText();
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["EmpPanNo"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ReceiveDate"]);

                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["DESIGNNAME"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["COLORNAME"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["SIZE"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["TotalQty"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["WEIGHT"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["RATE"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["AMOUNT"]);
                    decimal TDSAmt = 0;
                    TDSAmt = Math.Round((Convert.ToDecimal(ds.Tables[0].Rows[i]["AMOUNT"]) * 1 / 100),2);
                    sht.Range("M" + row).SetValue(TDSAmt);
                    decimal NewWeavingAmt = 0;
                    NewWeavingAmt = (Convert.ToDecimal(ds.Tables[0].Rows[i]["AMOUNT"]) - TDSAmt);
                    sht.Range("N" + row).SetValue(NewWeavingAmt);
                    sht.Range("O" + row).SetValue("");

                    //sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                    //sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["DESIGNNAME"]);
                    //sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["COLORNAME"]);
                    //sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["SIZE"]);
                    //sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["TotalQty"]);
                    //sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["WEIGHT"]);
                    //sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["RATE"]);
                    //sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["AMOUNT"]);
                    //decimal TDSAmt = 0;
                    //TDSAmt = Math.Round((Convert.ToDecimal(ds.Tables[0].Rows[i]["AMOUNT"]) * 1 / 100), 2);
                    //sht.Range("L" + row).SetValue(TDSAmt);
                    //decimal NewWeavingAmt = 0;
                    //NewWeavingAmt = (Convert.ToDecimal(ds.Tables[0].Rows[i]["AMOUNT"]) - TDSAmt);
                    //sht.Range("M" + row).SetValue(NewWeavingAmt);
                    //sht.Range("N" + row).SetValue("");

                    using (var a = sht.Range("A" + row + ":O" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    row = row + 1;

                }
                ////*************
                //sht.Columns(1, 26).AdjustToContents();

                //sht.Columns("K").Width = 13.43;


                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("WeavingReceiveWithTDS_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }

    protected void WeaverRawMaterialIssRecWithConsumptionReport()
    {
        lblmsg.Text = "";
        try
        {
            string str = "", FilterBy = "", str2 = "";

            //if (DDCustCode.SelectedIndex > 0)
            //{
            //    str = str + " and OM.Customerid=" + DDCustCode.SelectedValue;
            //    FilterBy = FilterBy + ", Customer code -" + DDCustCode.SelectedItem.Text;
            //}
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    str = str + " and OM.orderid=" + DDOrderNo.SelectedValue;
            //    FilterBy = FilterBy + ", Order No. -" + DDOrderNo.SelectedItem.Text;
            //}
            
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.Issueorderid=" + DDFolioNo.SelectedValue;
            //    FilterBy = FilterBy + ", Folio No. -" + DDFolioNo.SelectedItem.Text;
            //}           

            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and Vf.Item_id=" + DDQtype.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and Vf.Qualityid=" + DDQuality.SelectedValue;
            }
            //if (DDshade.SelectedIndex > 0)
            //{
            //    str = str + " and vf.Shadecolorid=" + DDshade.SelectedValue;
            //}
            if (ChkselectDate.Checked == true)
            {
                str = str + " and PIM.AssignDate>='" + txtfromDate.Text + "' and PIM.AssignDate<='" + txttodate.Text + "'";
                FilterBy = FilterBy + ", From -" + txtfromDate.Text + " To - " + txttodate.Text;
            }



            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetWeaverRawMaterialIssRecWithConsumptionReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", str);
            //cmd.Parameters.AddWithValue("@Where2", str2);
            cmd.Parameters.AddWithValue("@empid", "0");

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();
            //***********
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

                sht.Column("A").Width = 25.11;
                sht.Column("B").Width = 25.67;
                sht.Column("C").Width = 25.11;
                sht.Column("D").Width = 15.22;
                sht.Column("E").Width = 15.22;
                sht.Column("F").Width = 15.89;
                sht.Column("G").Width = 15.67;
                sht.Column("H").Width = 15.89;

                sht.Range("A1:H1").Merge();
                sht.Range("A1").SetValue("WEAVER RAW MATERIAL ISS REC WITH CONSUMPTION DETAIL");
                sht.Range("A1:H1").Style.Font.Bold = true;
                sht.Range("A1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:H1").Style.Font.FontName = "Calibri";
                sht.Range("A1:H1").Style.Font.FontSize = 12;
                //sht.Range("A1:M1").Style.Fill.BackgroundColor = XLColor.LightGray;

                //*******Header
                sht.Range("A2").Value = "ITEM NAME";
                sht.Range("B2").Value = "QUALITY NAME";
                sht.Range("C2").Value = "SHADECOLOR";
                sht.Range("D2").Value = "CONSUMPTION QTY";
                sht.Range("E2").Value = "ISS QTY";
                sht.Range("F2").Value = "REC QTY";
                sht.Range("G2").Value = "BAL TO ISS";
                sht.Range("H2").Value = "EXTRA QTY";


                sht.Range("A2:H2").Style.Font.FontName = "Calibri";
                sht.Range("A2:H2").Style.Font.FontSize = 11;
                sht.Range("A2:H2").Style.Font.Bold = true;
                sht.Range("A2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:H2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:H2").Style.Alignment.SetWrapText();


                row = 3;

                int noofrows2 = 0;
                int i2 = 0;
                string EmpName = "";
                string POrderId = "";
                //string OrderColor = "";

                DataTable DtDistinctEmpNameFolioNo = ds.Tables[0].DefaultView.ToTable(true, "EmpName", "EmpId", "ProrderId","FolioChallanNo");
                noofrows2 = DtDistinctEmpNameFolioNo.Rows.Count;

                for (i2 = 0; i2 < noofrows2; i2++)
                {
                    EmpName = DtDistinctEmpNameFolioNo.Rows[i2]["EmpName"].ToString();
                    POrderId = DtDistinctEmpNameFolioNo.Rows[i2]["ProrderId"].ToString();                    

                    sht.Range("A" + row).Value = "EMP NAME:";
                    sht.Range("A" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row).Style.Font.Bold = true;
                    sht.Range("A" + row).Style.Font.FontSize = 11;
                    sht.Range("A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("A" + row).Style.Alignment.WrapText = true;
                    sht.Range("A" + row).Merge();

                    sht.Range("B" + row).SetValue(DtDistinctEmpNameFolioNo.Rows[i2]["EmpName"]);
                    sht.Range("B" + row + ":C" + row).Style.Font.Bold = true;
                    sht.Range("B" + row + ":C" + row).Style.Font.FontName = "Calibri";
                    sht.Range("B" + row + ":C" + row).Style.Font.FontSize = 11;
                    //sht.Range("B" + row + ":D" + row).Style.Font.FontColor = XLColor.White;
                    sht.Range("B" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    //sht.Range("A" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("B" + row + ":C" + row).Style.Alignment.WrapText = true;
                    //sht.Range("B" + row + ":D" + row).Style.Fill.BackgroundColor = XLColor.Black;
                    sht.Range("B" + row + ":C" + row).Merge();


                    sht.Range("D" + row).Value = "FOLIO NO:";
                    sht.Range("D" + row).Style.Font.FontName = "Calibri";
                    sht.Range("D" + row).Style.Font.Bold = true;
                    sht.Range("D" + row).Style.Font.FontSize = 11;
                    sht.Range("D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("D" + row).Style.Alignment.WrapText = true;
                    sht.Range("D" + row).Merge();

                    sht.Range("E" + row).SetValue(DtDistinctEmpNameFolioNo.Rows[i2]["FolioChallanNo"]);
                    sht.Range("E" + row + ":F" + row).Style.Font.Bold = true;
                    sht.Range("E" + row + ":F" + row).Style.Font.FontName = "Calibri";
                    sht.Range("E" + row + ":F" + row).Style.Font.FontSize = 11;
                    sht.Range("E" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("E" + row + ":F" + row).Style.Alignment.WrapText = true;
                    //sht.Range("B" + row + ":D" + row).Style.Fill.BackgroundColor = XLColor.Black;
                    sht.Range("E" + row + ":F" + row).Merge();                   

                    row = row + 1;  

                    string ItemName = "";
                    string QualityName = "";
                    string ShadeColorName = "";
                    decimal ConsumptionQty = 0;                   
                    decimal WeaverIssueQty = 0;
                    decimal WeaverRecQty = 0;

                    DataRow[] foundRows;
                    foundRows = ds.Tables[0].Select("EmpName='" + DtDistinctEmpNameFolioNo.Rows[i2]["EmpName"] + "' and Empid='" + DtDistinctEmpNameFolioNo.Rows[i2]["EmpId"] + "' and PrOrderId='" + DtDistinctEmpNameFolioNo.Rows[i2]["PrOrderID"] + "'");

                    if (foundRows.Length > 0)
                    {
                        foreach (DataRow row3 in foundRows)
                        {
                            ItemName = row3["Item_Name"].ToString();
                            QualityName = row3["QualityName"].ToString();
                            ShadeColorName = row3["ShadeColorName"].ToString();
                            ConsumptionQty = Convert.ToDecimal(row3["ConsumptionQty"].ToString());
                            WeaverIssueQty = Convert.ToDecimal(row3["IssQty"].ToString());
                            WeaverRecQty = Convert.ToDecimal(row3["RecQty"].ToString());


                            sht.Range("A" + row).SetValue(ItemName);
                            sht.Range("B" + row).SetValue(QualityName);
                            sht.Range("C" + row).SetValue(ShadeColorName);
                            sht.Range("D" + row).SetValue(ConsumptionQty);
                            //sht.Range("C" + row).SetValue(IShadeColor);
                            sht.Range("E" + row).SetValue(WeaverIssueQty);
                            sht.Range("F" + row).SetValue(WeaverRecQty);

                            decimal BalToIssQty = (Convert.ToDecimal(ConsumptionQty) - WeaverIssueQty);
                            sht.Range("G" + row).SetValue(Math.Round(BalToIssQty, 3));

                            decimal ExtraQty = (ConsumptionQty - WeaverIssueQty);
                            if (ExtraQty < 0)
                            {
                                sht.Range("H" + row).FormulaA1 = "=ABS(" + (ExtraQty)+")";
                                
                            }
                            else
                            {
                                sht.Range("H" + row).SetValue("0");  
                            }
                                                     

                            sht.Range("A" + row + ":H" + row).Style.Font.Bold = true;
                            sht.Range("A" + row + ":H" + row).Style.Font.FontName = "Calibri";
                            sht.Range("A" + row + ":H" + row).Style.Font.FontSize = 11;
                            sht.Range("A" + row + ":H" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Range("A" + row + ":H" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                            sht.Range("A" + row + ":H" + row).Style.Alignment.WrapText = true;

                            row = row + 1;

                            //break;
                        }
                    }

                    using (var a = sht.Range("A2" + ":H" + (row - 1)))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                    row = row + 1;
                }

                row = row + 1;

                ////*************
                //sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("WeaverRawMaterialIssRecWithConsumptionReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }

    protected void WeavingReceivePaymentSummary()
    {
        lblmsg.Text = "";
        try
        {
            string str = "", FilterBy = "";

            //if (DDUnitname.SelectedIndex > 0)
            //{
            //    str = str + "  and PRM.UNITID=" + DDUnitname.SelectedValue;
            //}
            //if (DDWeaver.SelectedIndex > 0)
            //{
            //    str = str + " and EI.EMPID=" + DDWeaver.SelectedValue;
            //}
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and PRD.ISSUEORDERID=" + DDFolioNo.SelectedValue;
            //}
            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and VF.ITEM_ID=" + DDQtype.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and VF.QUALITYID=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and VF.DESIGNID=" + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " and VF.COLORID=" + DDColor.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and VF.SIZEID=" + DDSize.SelectedValue;
            }
            if (ChkselectDate.Checked == true)
            {
                str = str + " and PRM.RECEIVEDATE>='" + txtfromDate.Text + "' and PRM.RECEIVEDATE<='" + txttodate.Text + "'";
                FilterBy = FilterBy + ",Date From -" + txtfromDate.Text + " To - " + txttodate.Text;

            }
            //if (DDFoliotype.SelectedIndex > 0)
            //{
            //    str = str + " and PIM.PURCHASEFLAG=" + DDFoliotype.SelectedValue;
            //}
            //if (DDCustCode.SelectedIndex > 0)
            //{
            //    str = str + " and OM.CUSTOMERID=" + DDCustCode.SelectedValue;
            //}
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    str = str + " and OM.ORDERID=" + DDOrderNo.SelectedValue;
            //}
            ////if (DDproductiontype.SelectedIndex > 0)
            ////{
            ////    str = str + " and PRODUCTIONTYPE=" + DDproductiontype.SelectedValue;
            ////}


            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETWEAVINGRECEIVEPAYMENTSUMMARYREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", str);


            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            //*************
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            con.Close();
            con.Dispose();
            //***********
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                sht.PageSetup.PageOrientation = XLPageOrientation.Portrait;
                //sht.PageSetup.AdjustTo(90);
                sht.PageSetup.FitToPages(1, 1);
                sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
                //sht.PageSetup.VerticalDpi = 300;
                //sht.PageSetup.HorizontalDpi = 300;
                sht.PageSetup.Margins.Top = 0.2;
                sht.PageSetup.Margins.Bottom = 0.2;
                sht.PageSetup.Margins.Right = 0.2;
                sht.PageSetup.Margins.Left = 0.2;
                sht.PageSetup.Margins.Header = 0.2;
                sht.PageSetup.Margins.Footer = 0.2;
                //sht.Style.Font.FontName = "Cambria";
                sht.PageSetup.SetScaleHFWithDocument();
                sht.PageSetup.CenterHorizontally = true;

                sht.Column("A").Width = 10.67;
                sht.Column("B").Width = 19.67;
                sht.Column("C").Width = 19.67;
                sht.Column("D").Width = 19.67;
                sht.Column("E").Width = 19.67;
                sht.Column("F").Width = 19.67;
                sht.Column("G").Width = 12.67;
                sht.Column("H").Width = 12.67;
                sht.Column("I").Width = 12.67;
                sht.Column("J").Width = 12.67;
                sht.Column("K").Width = 12.67;
                sht.Column("L").Width = 12.67;
                sht.Column("M").Width = 19.67;
                sht.Column("N").Width = 19.67;
                sht.Column("O").Width = 12.67;

                sht.Row(2).Height = 34;


                sht.Cell("A1").Value = "WEAVING RECEIVE PAYMENT SUMMARY";
                sht.Range("A1:O1").Style.Font.FontName = "Calibri";
                sht.Range("A1:O1").Style.Font.FontSize = 22;
                sht.Range("A1:O1").Style.Font.Bold = true;
                sht.Range("A1:O1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:O1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1:O1").Merge();

                sht.Cell("A2").Value = ds.Tables[0].Rows[0]["COMPANYNAME"].ToString().ToUpper();
                sht.Range("A2:O2").Style.Font.FontName = "Calibri";
                sht.Range("A2:O2").Style.Font.FontSize = 22;
                sht.Range("A2:O2").Style.Font.Bold = true;
                sht.Range("A2:O2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:O2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A2:O2").Merge();  

                //sht.Cell("L1").Value = "PRINT DATE:" + " " + DateTime.Now.ToString();
                //sht.Range("L1:O4").Style.Font.FontName = "Calibri";
                //sht.Range("L1:O4").Style.Font.FontSize = 11;
                //sht.Range("L1:O4").Style.Font.Bold = true;
                //sht.Range("L1:O4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //sht.Range("L1:O4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                //sht.Range("L1:O4").Merge();

                using (var a = sht.Range("A1:O2"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                int row = 0;

                //sht.Range("A1:N1").Merge();
                //sht.Range("A1").Value = "WEAVING RECEIVE WITH TDS";
                //sht.Range("A2:N2").Merge();
                //sht.Range("A2").Value = "Filter By :  " + FilterBy;
                //sht.Row(2).Height = 30;
                //sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Range("A2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Range("A2:N2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                //sht.Range("A2:N2").Style.Alignment.SetWrapText();
                //sht.Range("A1:N2").Style.Font.FontName = "Arial Unicode MS";
                //sht.Range("A1:N2").Style.Font.FontSize = 10;
                //sht.Range("A1:N2").Style.Font.Bold = true;



                //*******Header
                sht.Range("A3").Value = "RECEIVE DATE";
                sht.Range("B3").Value = "NAME OF WEAVER";
                sht.Range("C3").Value = "FOLIO NO";              

                sht.Range("D3").Value = "QUALITY";
                sht.Range("E3").Value = "DESIGN";
                sht.Range("F3").Value = "COLOR";
                sht.Range("G3").Value = "SIZE";
                sht.Range("H3").Value = "NOOF PIECES";

                sht.Range("I3").Value = "AREA";
                sht.Range("J3").Value = "RATE";
                sht.Range("K3").Value = "AMOUNT";
                sht.Range("L3").Value = "WOOL YARN KGS";
                sht.Range("M3").Value = "SILK YARN KGS";
                sht.Range("N3").Value = "JUTE YARN KGS";
                sht.Range("O3").Value = "COTTON YARN KGS";                


                sht.Range("A3:O3").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A3:O3").Style.Font.FontSize = 9;
                sht.Range("A3:O3").Style.Font.Bold = true;
                sht.Range("A3:O3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A3:O3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A3:O3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A3:O3").Style.Alignment.SetWrapText();

                using (var a = sht.Range("A3:O3"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }


                //DataView dv = new DataView(ds.Tables[0]);
                //dv.Sort = "FOLIONO";
                //DataSet ds1 = new DataSet();
                //ds1.Tables.Add(dv.ToTable());

                row = 4;
                int Srno = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    sht.Range("A" + row + ":O" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":O" + row).Style.Font.FontSize = 9;
                    //sht.Range("A" + row + ":L" + row).Style.Font.SetBold();
                    sht.Range("A" + row + ":O" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    //sht.Range("A" + row + ":L" + row).Style.Alignment.SetWrapText();
                    sht.Range("A" + row + ":O" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    Srno = Srno + 1;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["ReceiveDate"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                    sht.Range("B" + row).Style.Alignment.SetWrapText();
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["FolioChallanNo"]);                    

                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["DESIGNNAME"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["COLORNAME"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["SIZE"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["TotalQty"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["TotalArea"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["RATE"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["AMOUNT"]);
                    sht.Range("L" + row).SetValue("");
                    sht.Range("M" + row).SetValue("");
                    sht.Range("N" + row).SetValue("");
                    sht.Range("O" + row).SetValue("");

                    //decimal TDSAmt = 0;
                    //TDSAmt = Math.Round((Convert.ToDecimal(ds.Tables[0].Rows[i]["AMOUNT"]) * 1 / 100), 2);
                    //sht.Range("M" + row).SetValue(TDSAmt);
                    //decimal NewWeavingAmt = 0;
                    //NewWeavingAmt = (Convert.ToDecimal(ds.Tables[0].Rows[i]["AMOUNT"]) - TDSAmt);
                    //sht.Range("N" + row).SetValue(NewWeavingAmt);
                    //sht.Range("O" + row).SetValue("");                   

                    using (var a = sht.Range("A" + row + ":O" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    row = row + 1;

                }
                ////*************
                //sht.Columns(1, 26).AdjustToContents();

                //sht.Columns("K").Width = 13.43;


                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("WeavingReceivePaymentSummary_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }


    protected void WeaverPendingQtyExcelReport()
    {

        try
        {
            string str = "select *,'" + txtfromDate.Text + "' as FromDate,'" + txttodate.Text + "' as Todate," + (ChkselectDate.Checked == true ? "1" : "0") + " as Dateflag,FolioType From V_WeavingOrderStatus Where CompanyId=" + DDCompany.SelectedValue;


            if (DDUnitname.SelectedIndex > 0)
            {
                str = str + " and Units=" + DDUnitname.SelectedValue;
            }
            //if (DDProductionstatus.SelectedIndex > 0)
            //{
            //    str = str + " and Status='" + DDProductionstatus.SelectedItem.Text + "'";
            //    if (DDProductionstatus.SelectedIndex == 1)
            //    {
            //        str = str + " and Pqty>0";
            //    }
            //}
            //if (DDWeaver.SelectedIndex > 0)
            //{
            //    str = str + " and Empid=" + DDWeaver.SelectedValue;
            //}
            //if (DDFolioNo.SelectedIndex > 0)
            //{
            //    str = str + " and Issueorderid=" + DDFolioNo.SelectedValue;
            //}
            if (DDQtype.SelectedIndex > 0)
            {
                str = str + " and Item_id=" + DDQtype.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + " and Qualityid=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and DesignId=" + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                str = str + " and Colorid=" + DDColor.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0)
            {
                str = str + " and Sizeid=" + DDSize.SelectedValue;
            }
            if (ChkselectDate.Checked == true)
            {
                str = str + " and Assigndate>='" + txtfromDate.Text + "' and AssignDate<='" + txttodate.Text + "'";
            }
            //if (DDFoliotype.SelectedIndex > 0)
            //{
            //    str = str + " and Purchasefolio=" + DDFoliotype.SelectedValue;
            //}
            if (DDCustCode.SelectedIndex > 0)
            {
                str = str + " and Customerid=" + DDCustCode.SelectedValue;
            }
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    str = str + " and orderid=" + DDOrderNo.SelectedValue;
            //}
            //if (DDproductiontype.SelectedIndex > 0)
            //{
            //    str = str + " and PRODUCTIONTYPE=" + DDproductiontype.SelectedValue;
            //}
            str = str + " Order By EMPNAME,ISSUE_DETAIL_ID";

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(str, con);
            //cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();


            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

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

                sht.Column("A").Width = 25.11;
                sht.Column("B").Width = 25.67;
                sht.Column("C").Width = 25.11;
                sht.Column("D").Width = 15.22;
                sht.Column("E").Width = 15.22;
                sht.Column("F").Width = 15.89;
                sht.Column("G").Width = 15.67;
                sht.Column("H").Width = 15.89;
                sht.Column("I").Width = 15.89;
                sht.Column("J").Width = 15.89;
                sht.Column("K").Width = 15.89;
                sht.Column("L").Width = 15.89;

                sht.Range("A1:L1").Merge();
                sht.Range("A1").SetValue("WEAVER SUMMARY");
                sht.Range("A1:L1").Style.Font.Bold = true;
                sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:L1").Style.Font.FontName = "Calibri";
                sht.Range("A1:L1").Style.Font.FontSize = 12;
                //sht.Range("A1:M1").Style.Fill.BackgroundColor = XLColor.LightGray;

                //*******Header
                sht.Range("A2").Value = "CUST CODE ORDERNO";
                sht.Range("B2").Value = "FOLIO NO";
                sht.Range("C2").Value = "QUALITY";
                sht.Range("D2").Value = "DESIGN";
                sht.Range("E2").Value = "COLOR";
                sht.Range("F2").Value = "SIZE";
                sht.Range("G2").Value = "AREA";
                sht.Range("H2").Value = "QTY";
                sht.Range("I2").Value = "RECQTY";
                sht.Range("J2").Value = "PQTY";
                sht.Range("K2").Value = "ORDER DATE";
                sht.Range("L2").Value = "LAST DATE";


                sht.Range("A2:L2").Style.Font.FontName = "Calibri";
                sht.Range("A2:L2").Style.Font.FontSize = 11;
                sht.Range("A2:L2").Style.Font.Bold = true;
                sht.Range("A2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:L2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:L2").Style.Alignment.SetWrapText();


                row = 3;

                int noofrows2 = 0;
                int i2 = 0;
                string EmpName = "";
                string EmpFather = "";                
                string POrderId = "";
                //string OrderColor = "";

                DataTable DtDistinctEmpNameAddress = ds.Tables[0].DefaultView.ToTable(true, "EmpName", "EmpId", "FatherName", "Address");
                noofrows2 = DtDistinctEmpNameAddress.Rows.Count;

                for (i2 = 0; i2 < noofrows2; i2++)
                {
                    EmpName = DtDistinctEmpNameAddress.Rows[i2]["EmpName"].ToString();
                    EmpFather = DtDistinctEmpNameAddress.Rows[i2]["FatherName"].ToString();                    
                    //POrderId = DtDistinctEmpNameAddress.Rows[i2]["ProrderId"].ToString();

                    sht.Range("A" + row).Value = "EMP NAME:";
                    sht.Range("A" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row).Style.Font.Bold = true;
                    sht.Range("A" + row).Style.Font.FontSize = 11;
                    sht.Range("A" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("A" + row).Style.Alignment.WrapText = true;
                    sht.Range("A" + row).Merge();

                    sht.Range("B" + row).SetValue(DtDistinctEmpNameAddress.Rows[i2]["EmpName"] + " " + "S/O" + DtDistinctEmpNameAddress.Rows[i2]["FatherName"]);
                    sht.Range("B" + row + ":C" + row).Style.Font.Bold = true;
                    sht.Range("B" + row + ":C" + row).Style.Font.FontName = "Calibri";
                    sht.Range("B" + row + ":C" + row).Style.Font.FontSize = 11;
                    //sht.Range("B" + row + ":D" + row).Style.Font.FontColor = XLColor.White;
                    sht.Range("B" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    //sht.Range("A" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("B" + row + ":C" + row).Style.Alignment.WrapText = true;
                    //sht.Range("B" + row + ":D" + row).Style.Fill.BackgroundColor = XLColor.Black;
                    sht.Range("B" + row + ":C" + row).Merge();


                    sht.Range("D" + row).Value = "ADDRESS:";
                    sht.Range("D" + row).Style.Font.FontName = "Calibri";
                    sht.Range("D" + row).Style.Font.Bold = true;
                    sht.Range("D" + row).Style.Font.FontSize = 11;
                    sht.Range("D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("D" + row).Style.Alignment.WrapText = true;
                    sht.Range("D" + row).Merge();

                    sht.Range("E" + row).SetValue(DtDistinctEmpNameAddress.Rows[i2]["Address"]);
                    sht.Range("E" + row + ":F" + row).Style.Font.Bold = true;
                    sht.Range("E" + row + ":F" + row).Style.Font.FontName = "Calibri";
                    sht.Range("E" + row + ":F" + row).Style.Font.FontSize = 11;
                    sht.Range("E" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("E" + row + ":F" + row).Style.Alignment.WrapText = true;
                    //sht.Range("B" + row + ":D" + row).Style.Fill.BackgroundColor = XLColor.Black;
                    sht.Range("E" + row + ":F" + row).Merge();

                    row = row + 1;

                    string CustomerCode = "";
                    string CustomerOrderNo = "";
                    string FolioChallanNo = "";
                    string FolioType = "";
                    string ItemName = "";
                    string QualityName = "";
                    string DesignName = "";
                    string ColorName = "";
                    string Shape = "";
                    string Size = "";
                    decimal Area = 0;
                    int IssQty = 0;
                    int RecQty = 0;
                    int PQty = 0;
                    string OrderDate = "";
                    string LastDate = "";
                    int rowfrom = 0, rowto = 0;
                    //decimal ConsumptionQty = 0;
                    //decimal WeaverIssueQty = 0;
                    //decimal WeaverRecQty = 0;

                    DataRow[] foundRows;
                    foundRows = ds.Tables[0].Select("EmpName='" + DtDistinctEmpNameAddress.Rows[i2]["EmpName"] + "' and Empid='" + DtDistinctEmpNameAddress.Rows[i2]["EmpId"] + "' and FatherName='" + DtDistinctEmpNameAddress.Rows[i2]["FatherName"] + "' and Address='" + DtDistinctEmpNameAddress.Rows[i2]["Address"] + "' ");

                    if (foundRows.Length > 0)
                    {
                        rowfrom = row;
                        foreach (DataRow row3 in foundRows)
                        {
                            CustomerCode = row3["CustomerCode"].ToString();
                            CustomerOrderNo = row3["CustomerOrderNo"].ToString();
                            FolioChallanNo = row3["FolioChallaNo"].ToString();
                            FolioType = row3["FolioType"].ToString();
                            //ItemName = row3["Item_Name"].ToString();
                            QualityName = row3["QualityName"].ToString();
                            DesignName = row3["DesignName"].ToString();
                            ColorName = row3["ColorName"].ToString();
                            Shape = row3["ShapeName"].ToString();
                            Size = row3["Size"].ToString();

                            Area =Convert.ToDecimal(row3["Area"].ToString());
                            IssQty = Convert.ToInt32(row3["Qty"].ToString());
                            PQty = Convert.ToInt32(row3["PQty"].ToString());
                            RecQty = IssQty - PQty;


                            //Area = Convert.ToDecimal(ds.Tables[0].Compute("sum(Area)", "EmpName='" + DtDistinctEmpNameAddress.Rows[i2]["EmpName"] + "' and Empid='" + DtDistinctEmpNameAddress.Rows[i2]["EmpId"] + "' and FatherName='" + DtDistinctEmpNameAddress.Rows[i2]["FatherName"] + "' and Address='" + DtDistinctEmpNameAddress.Rows[i2]["Address"] + "' and Issue_Detail_Id='" + DtDistinctEmpNameAddress.Rows[i2]["Issue_Detail_Id"] + "' "));
                            //IssQty = Convert.ToInt32(ds.Tables[0].Compute("sum(Qty)", "EmpName='" + DtDistinctEmpNameAddress.Rows[i2]["EmpName"] + "' and Empid='" + DtDistinctEmpNameAddress.Rows[i2]["EmpId"] + "' and FatherName='" + DtDistinctEmpNameAddress.Rows[i2]["FatherName"] + "' and Address='" + DtDistinctEmpNameAddress.Rows[i2]["Address"] + "' and Issue_Detail_Id='" + DtDistinctEmpNameAddress.Rows[i2]["Issue_Detail_Id"] + "' "));
                            //PQty = Convert.ToInt32(ds.Tables[0].Compute("sum(PQty)", "EmpName='" + DtDistinctEmpNameAddress.Rows[i2]["EmpName"] + "' and Empid='" + DtDistinctEmpNameAddress.Rows[i2]["EmpId"] + "' and FatherName='" + DtDistinctEmpNameAddress.Rows[i2]["FatherName"] + "' and Address='" + DtDistinctEmpNameAddress.Rows[i2]["Address"] + "' and Issue_Detail_Id='" + DtDistinctEmpNameAddress.Rows[i2]["Issue_Detail_Id"] + "' "));
                            //RecQty = IssQty - PQty;
                            OrderDate = row3["AssignDate"].ToString();
                            LastDate = row3["ReqByDate"].ToString();


                            sht.Range("A" + row).SetValue(CustomerCode + " " + CustomerOrderNo);
                            sht.Range("B" + row).SetValue(FolioChallanNo + "(" + FolioType+")");
                            sht.Range("C" + row).SetValue(QualityName);
                            sht.Range("D" + row).SetValue(DesignName);
                            //sht.Range("C" + row).SetValue(IShadeColor);
                            sht.Range("E" + row).SetValue(ColorName);
                            sht.Range("F" + row).SetValue(Size + " " + Shape);

                            sht.Range("G" + row).SetValue(Area);
                            sht.Range("H" + row).SetValue(IssQty);
                            sht.Range("I" + row).SetValue(RecQty);
                            sht.Range("J" + row).SetValue(PQty);
                            sht.Range("K" + row).SetValue(Convert.ToDateTime(OrderDate).ToString("dd/MM/yyyy")); 
                            sht.Range("L" + row).SetValue(Convert.ToDateTime(LastDate).ToString("dd/MM/yyyy"));

                            //decimal BalToIssQty = (Convert.ToDecimal(ConsumptionQty) - WeaverIssueQty);
                            //sht.Range("G" + row).SetValue(Math.Round(BalToIssQty, 3));

                            //decimal ExtraQty = (ConsumptionQty - WeaverIssueQty);
                            //if (ExtraQty < 0)
                            //{
                            //    sht.Range("H" + row).FormulaA1 = "=ABS(" + (ExtraQty) + ")";

                            //}
                            //else
                            //{
                            //    sht.Range("H" + row).SetValue("0");
                            //}


                            sht.Range("A" + row + ":L" + row).Style.Font.Bold = true;
                            sht.Range("A" + row + ":L" + row).Style.Font.FontName = "Calibri";
                            sht.Range("A" + row + ":L" + row).Style.Font.FontSize = 11;
                            sht.Range("A" + row + ":L" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Range("A" + row + ":L" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                            sht.Range("A" + row + ":L" + row).Style.Alignment.WrapText = true;

                            row = row + 1;

                            //break;
                        }

                        rowto = row - 1;
                        sht.Range("F" + row).SetValue("Total");
                        sht.Range("G" + row).FormulaA1 = "=SUM(G" + rowfrom + ":G" + rowto + ")";
                        sht.Range("H" + row).FormulaA1 = "SUM(H" + rowfrom + ":H" + rowto + ")";
                        sht.Range("I" + row).FormulaA1 = "SUM(I" + rowfrom + ":I" + rowto + ")"; 
                        sht.Range("J" + row).FormulaA1 = "SUM(J" + rowfrom + ":J" + rowto + ")";
                        //sht.Range("I" + row).FormulaA1 = "=L" + row + '-' + "M" + row;
                        sht.Range("F" + row + ":J" + row).Style.Font.Bold = true;
                        sht.Range("F" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("F" + row + ":J" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                      

                    }

                    using (var a = sht.Range("A2" + ":L" + (row - 1)))
                    {
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }

                    row = row + 1;
                }

                row = row + 1;

                ////*************
                //sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("WeaverSummaryExcelReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
       
    }
}